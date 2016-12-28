// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat 
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Microsoft.SpecExplorer.Runtime.Testing;
    using Microsoft.Protocols.TestTools;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Spec Explorer", "3.4.2965.0")]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class TestScenarioS1 : PtfTestClassBase {
        
        public TestScenarioS1() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "10000000");
        }
        
        #region Expect Delegates
        public delegate void InitializeDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase InitializeInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ILsatAdapter), "Initialize", typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess), typeof(uint));
        #endregion
        
        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ILsatAdapter ILsatAdapterInstance;
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
            this.ILsatAdapterInstance = ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ILsatAdapter)(this.Manager.GetAdapter(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ILsatAdapter))));
        }
        
        protected override void TestCleanup() {
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion
        
        #region Test Starting in S0
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS1S0() {
            this.Manager.BeginTest("TestScenarioS1S0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp0;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp0 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp7 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS1.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS1S0InitializeChecker)), new ExpectedReturn(TestScenarioS1.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS1S0InitializeChecker1)));
            if ((temp7 == 0)) {
                this.Manager.Comment("reaching state \'S12\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp1;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp2;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp3;
                this.Manager.Comment("executing step \'call WinLookUpNames2(Invalid,{\"DoesNotExist\"},out _,out _)\'");
                temp3 = this.ILsatAdapterInstance.WinLookUpNames2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LSAHandle)(1)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), out temp1, out temp2);
                this.Manager.Comment("reaching state \'S24\'");
                this.Manager.Comment("checking step \'return WinLookUpNames2/[out Invalid,out Invalid]:InvalidServerStat" +
                        "e\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp1, "translateSids of WinLookUpNames2, state S24");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp2, "mapCount of WinLookUpNames2, state S24");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.InvalidServerState, temp3, "return of WinLookUpNames2, state S24");
                TestScenarioS1S36();
                goto label0;
            }
            if ((temp7 == 1)) {
                this.Manager.Comment("reaching state \'S13\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp4;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp5;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp6;
                this.Manager.Comment("executing step \'call WinLookUpNames2(Valid,{\"DoesNotExist\"},out _,out _)\'");
                temp6 = this.ILsatAdapterInstance.WinLookUpNames2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LSAHandle)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), out temp4, out temp5);
                this.Manager.Checkpoint("MS-LSAT_R222");
                this.Manager.Checkpoint("MS-LSAT_R255");
                this.Manager.Comment("reaching state \'S25\'");
                this.Manager.Comment("checking step \'return WinLookUpNames2/[out Invalid,out Invalid]:NoneMapped\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp4, "translateSids of WinLookUpNames2, state S25");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp5, "mapCount of WinLookUpNames2, state S25");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.NoneMapped, temp6, "return of WinLookUpNames2, state S25");
                TestScenarioS1S37();
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS1S0InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S1");
        }
        
        private void TestScenarioS1S36() {
            this.Manager.Comment("reaching state \'S36\'");
        }
        
        private void TestScenarioS1S0InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S1");
        }
        
        private void TestScenarioS1S37() {
            this.Manager.Comment("reaching state \'S37\'");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS1S10() {
            this.Manager.BeginTest("TestScenarioS1S10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp8;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp8 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp8);
            this.Manager.Comment("reaching state \'S11\'");
            int temp15 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS1.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS1S10InitializeChecker)), new ExpectedReturn(TestScenarioS1.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS1S10InitializeChecker1)));
            if ((temp15 == 0)) {
                this.Manager.Comment("reaching state \'S22\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp9;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp10;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp11;
                this.Manager.Comment("executing step \'call WinLookUpNames2(Valid,{\"UserPrincipalName1\",\"DoesNotExist\",\"" +
                        "FullQualifiedName1\"},out _,out _)\'");
                temp11 = this.ILsatAdapterInstance.WinLookUpNames2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LSAHandle)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), out temp9, out temp10);
                this.Manager.Checkpoint("MS-LSAT_R218");
                this.Manager.Checkpoint("MS-LSAT_R254");
                this.Manager.Comment("reaching state \'S34\'");
                this.Manager.Comment("checking step \'return WinLookUpNames2/[out Invalid,out Invalid]:SomeNotMapped\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp9, "translateSids of WinLookUpNames2, state S34");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp10, "mapCount of WinLookUpNames2, state S34");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.SomeNotMapped, temp11, "return of WinLookUpNames2, state S34");
                TestScenarioS1S37();
                goto label1;
            }
            if ((temp15 == 1)) {
                this.Manager.Comment("reaching state \'S23\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp12;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp13;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp14;
                this.Manager.Comment("executing step \'call WinLookUpNames2(Valid,{\"UserPrincipalName1\",\"FullQualifiedNa" +
                        "me1\",\"UnQualifiedName1\",\"IsolatedName1\"},out _,out _)\'");
                temp14 = this.ILsatAdapterInstance.WinLookUpNames2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LSAHandle)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), out temp12, out temp13);
                this.Manager.Comment("reaching state \'S35\'");
                this.Manager.Comment("checking step \'return WinLookUpNames2/[out Invalid,out Invalid]:InvalidServerStat" +
                        "e\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp12, "translateSids of WinLookUpNames2, state S35");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp13, "mapCount of WinLookUpNames2, state S35");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.InvalidServerState, temp14, "return of WinLookUpNames2, state S35");
                TestScenarioS1S36();
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS1S10InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S11");
        }
        
        private void TestScenarioS1S10InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S11");
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS1S2() {
            this.Manager.BeginTest("TestScenarioS1S2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp16;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp16 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp16);
            this.Manager.Comment("reaching state \'S3\'");
            int temp23 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS1.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS1S2InitializeChecker)), new ExpectedReturn(TestScenarioS1.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS1S2InitializeChecker1)));
            if ((temp23 == 0)) {
                this.Manager.Comment("reaching state \'S14\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp17;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp18;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp19;
                this.Manager.Comment("executing step \'call WinLookUpNames2(Valid,{\"UserPrincipalName1\",\"FullQualifiedNa" +
                        "me1\",\"UnQualifiedName1\",\"IsolatedName1\"},out _,out _)\'");
                temp19 = this.ILsatAdapterInstance.WinLookUpNames2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LSAHandle)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), out temp17, out temp18);
                this.Manager.Checkpoint("MS-LSAT_R253");
                this.Manager.Checkpoint("MS-LSAT_R217");
                this.Manager.Comment("reaching state \'S26\'");
                this.Manager.Comment("checking step \'return WinLookUpNames2/[out Valid,out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(0)), temp17, "translateSids of WinLookUpNames2, state S26");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(0)), temp18, "mapCount of WinLookUpNames2, state S26");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp19, "return of WinLookUpNames2, state S26");
                TestScenarioS1S37();
                goto label2;
            }
            if ((temp23 == 1)) {
                this.Manager.Comment("reaching state \'S15\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp20;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp21;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp22;
                this.Manager.Comment("executing step \'call WinLookUpNames2(Valid,{\"DoesNotExist\"},out _,out _)\'");
                temp22 = this.ILsatAdapterInstance.WinLookUpNames2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LSAHandle)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), out temp20, out temp21);
                this.Manager.Comment("reaching state \'S27\'");
                this.Manager.Comment("checking step \'return WinLookUpNames2/[out Invalid,out Invalid]:InvalidServerStat" +
                        "e\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp20, "translateSids of WinLookUpNames2, state S27");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp21, "mapCount of WinLookUpNames2, state S27");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.InvalidServerState, temp22, "return of WinLookUpNames2, state S27");
                TestScenarioS1S36();
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS1S2InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S3");
        }
        
        private void TestScenarioS1S2InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S3");
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS1S4() {
            this.Manager.BeginTest("TestScenarioS1S4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp24;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp24 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp24);
            this.Manager.Comment("reaching state \'S5\'");
            int temp31 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS1.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS1S4InitializeChecker)), new ExpectedReturn(TestScenarioS1.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS1S4InitializeChecker1)));
            if ((temp31 == 0)) {
                this.Manager.Comment("reaching state \'S16\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp25;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp26;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp27;
                this.Manager.Comment("executing step \'call WinLookUpNames2(Invalid,{\"UserPrincipalName1\",\"DoesNotExist\"" +
                        ",\"FullQualifiedName1\"},out _,out _)\'");
                temp27 = this.ILsatAdapterInstance.WinLookUpNames2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LSAHandle)(1)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), out temp25, out temp26);
                this.Manager.Checkpoint("MS-LSAT_R220");
                this.Manager.Comment("reaching state \'S28\'");
                this.Manager.Comment("checking step \'return WinLookUpNames2/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp25, "translateSids of WinLookUpNames2, state S28");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp26, "mapCount of WinLookUpNames2, state S28");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp27, "return of WinLookUpNames2, state S28");
                TestScenarioS1S37();
                goto label3;
            }
            if ((temp31 == 1)) {
                this.Manager.Comment("reaching state \'S17\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp28;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp29;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp30;
                this.Manager.Comment("executing step \'call WinLookUpNames2(Valid,{\"UserPrincipalName1\",\"DoesNotExist\",\"" +
                        "FullQualifiedName1\"},out _,out _)\'");
                temp30 = this.ILsatAdapterInstance.WinLookUpNames2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LSAHandle)(0)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), out temp28, out temp29);
                this.Manager.Comment("reaching state \'S29\'");
                this.Manager.Comment("checking step \'return WinLookUpNames2/[out Invalid,out Invalid]:InvalidServerStat" +
                        "e\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp28, "translateSids of WinLookUpNames2, state S29");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp29, "mapCount of WinLookUpNames2, state S29");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.InvalidServerState, temp30, "return of WinLookUpNames2, state S29");
                TestScenarioS1S36();
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS1S4InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S5");
        }
        
        private void TestScenarioS1S4InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S5");
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS1S6() {
            this.Manager.BeginTest("TestScenarioS1S6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp32;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp32 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp32);
            this.Manager.Comment("reaching state \'S7\'");
            int temp39 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS1.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS1S6InitializeChecker)), new ExpectedReturn(TestScenarioS1.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS1S6InitializeChecker1)));
            if ((temp39 == 0)) {
                this.Manager.Comment("reaching state \'S18\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp33;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp34;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp35;
                this.Manager.Comment("executing step \'call WinLookUpNames2(Invalid,{\"UserPrincipalName1\",\"FullQualified" +
                        "Name1\",\"UnQualifiedName1\",\"IsolatedName1\"},out _,out _)\'");
                temp35 = this.ILsatAdapterInstance.WinLookUpNames2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LSAHandle)(1)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), out temp33, out temp34);
                this.Manager.Checkpoint("MS-LSAT_R220");
                this.Manager.Comment("reaching state \'S30\'");
                this.Manager.Comment("checking step \'return WinLookUpNames2/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp33, "translateSids of WinLookUpNames2, state S30");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp34, "mapCount of WinLookUpNames2, state S30");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp35, "return of WinLookUpNames2, state S30");
                TestScenarioS1S37();
                goto label4;
            }
            if ((temp39 == 1)) {
                this.Manager.Comment("reaching state \'S19\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp36;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp37;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp38;
                this.Manager.Comment("executing step \'call WinLookUpNames2(Invalid,{\"UserPrincipalName1\",\"DoesNotExist\"" +
                        ",\"FullQualifiedName1\"},out _,out _)\'");
                temp38 = this.ILsatAdapterInstance.WinLookUpNames2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LSAHandle)(1)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), out temp36, out temp37);
                this.Manager.Comment("reaching state \'S31\'");
                this.Manager.Comment("checking step \'return WinLookUpNames2/[out Invalid,out Invalid]:InvalidServerStat" +
                        "e\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp36, "translateSids of WinLookUpNames2, state S31");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp37, "mapCount of WinLookUpNames2, state S31");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.InvalidServerState, temp38, "return of WinLookUpNames2, state S31");
                TestScenarioS1S36();
                goto label4;
            }
            throw new InvalidOperationException("never reached");
        label4:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS1S6InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S7");
        }
        
        private void TestScenarioS1S6InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S7");
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS1S8() {
            this.Manager.BeginTest("TestScenarioS1S8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp40;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp40 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp40);
            this.Manager.Comment("reaching state \'S9\'");
            int temp47 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS1.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS1S8InitializeChecker)), new ExpectedReturn(TestScenarioS1.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS1S8InitializeChecker1)));
            if ((temp47 == 0)) {
                this.Manager.Comment("reaching state \'S20\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp41;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp42;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp43;
                this.Manager.Comment("executing step \'call WinLookUpNames2(Invalid,{\"DoesNotExist\"},out _,out _)\'");
                temp43 = this.ILsatAdapterInstance.WinLookUpNames2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LSAHandle)(1)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), out temp41, out temp42);
                this.Manager.Checkpoint("MS-LSAT_R220");
                this.Manager.Comment("reaching state \'S32\'");
                this.Manager.Comment("checking step \'return WinLookUpNames2/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp41, "translateSids of WinLookUpNames2, state S32");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp42, "mapCount of WinLookUpNames2, state S32");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp43, "return of WinLookUpNames2, state S32");
                TestScenarioS1S37();
                goto label5;
            }
            if ((temp47 == 1)) {
                this.Manager.Comment("reaching state \'S21\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp44;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp45;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp46;
                this.Manager.Comment("executing step \'call WinLookUpNames2(Invalid,{\"UserPrincipalName1\",\"FullQualified" +
                        "Name1\",\"UnQualifiedName1\",\"IsolatedName1\"},out _,out _)\'");
                temp46 = this.ILsatAdapterInstance.WinLookUpNames2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LSAHandle)(1)), this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), out temp44, out temp45);
                this.Manager.Comment("reaching state \'S33\'");
                this.Manager.Comment("checking step \'return WinLookUpNames2/[out Invalid,out Invalid]:InvalidServerStat" +
                        "e\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp44, "translateSids of WinLookUpNames2, state S33");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp45, "mapCount of WinLookUpNames2, state S33");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.InvalidServerState, temp46, "return of WinLookUpNames2, state S33");
                TestScenarioS1S36();
                goto label5;
            }
            throw new InvalidOperationException("never reached");
        label5:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS1S8InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S9");
        }
        
        private void TestScenarioS1S8InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S9");
        }
        #endregion
    }
}
