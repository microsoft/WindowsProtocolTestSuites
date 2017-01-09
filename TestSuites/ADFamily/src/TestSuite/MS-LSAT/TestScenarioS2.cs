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
    public partial class TestScenarioS2 : PtfTestClassBase {
        
        public TestScenarioS2() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "10000000");
        }
        
        #region Expect Delegates
        public delegate void InitializeDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return);
        
        public delegate void CloseDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus @return);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase InitializeInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ILsatAdapter), "Initialize", typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess), typeof(uint));
        
        static System.Reflection.MethodBase CloseInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ILsatAdapter), "Close", typeof(int), typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle).MakeByRefType());
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
        public void LSAT_TestScenarioS2S0() {
            this.Manager.BeginTest("TestScenarioS2S0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp0;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp0 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp21 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S0InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S0InitializeChecker1)));
            if ((temp21 == 0)) {
                this.Manager.Comment("reaching state \'S64\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp1;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp2;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp2 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp1);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S128\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp1, "openPolicyHandle2 of OpenPolicy2, state S128");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp2, "return of OpenPolicy2, state S128");
                this.Manager.Comment("reaching state \'S192\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp3;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp4;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp5;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"DoesNotExist\"},MSBSet,LookUpWKSTA,out _,out" +
                        " _)\'");
                temp5 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp3, out temp4);
                this.Manager.Checkpoint("MS-LSAT_R282");
                this.Manager.Checkpoint("MS-LSAT_R314");
                this.Manager.Comment("reaching state \'S256\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:NoneMapped\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp3, "translateSids of LookUpNames3, state S256");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp4, "mapCount of LookUpNames3, state S256");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.NoneMapped, temp5, "return of LookUpNames3, state S256");
                TestScenarioS2S320();
                goto label0;
            }
            if ((temp21 == 1)) {
                this.Manager.Comment("reaching state \'S65\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp11;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp12;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp12 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp11);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S129\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp11, "openPolicyHandle2 of OpenPolicy2, state S129");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp12, "return of OpenPolicy2, state S129");
                this.Manager.Comment("reaching state \'S193\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp13;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp14;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp15;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"DoesNotExist\"},MSBSet,LookUpWKSTA,out _,out" +
                        " _)\'");
                temp15 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp13, out temp14);
                this.Manager.Checkpoint("MS-LSAT_R282");
                this.Manager.Checkpoint("MS-LSAT_R314");
                this.Manager.Comment("reaching state \'S257\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:NoneMapped\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp13, "translateSids of LookUpNames3, state S257");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp14, "mapCount of LookUpNames3, state S257");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.NoneMapped, temp15, "return of LookUpNames3, state S257");
                TestScenarioS2S321();
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S0InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S1");
        }
        
        private void TestScenarioS2S320() {
            this.Manager.Comment("reaching state \'S320\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp6;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp7;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp8;
            this.Manager.Comment("executing step \'call LookUpSids2(1,{\"DoesNotExist\"},LookUpWKSTA,out _,out _)\'");
            temp8 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp6, out temp7);
            this.Manager.Checkpoint("MS-LSAT_R517");
            this.Manager.Checkpoint("MS-LSAT_R562");
            this.Manager.Comment("reaching state \'S352\'");
            this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:NoneMapped\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp6, "translateNames of LookUpSids2, state S352");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp7, "mapCount of LookUpSids2, state S352");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.NoneMapped, temp8, "return of LookUpSids2, state S352");
            TestScenarioS2S384();
        }
        
        private void TestScenarioS2S384() {
            this.Manager.Comment("reaching state \'S384\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp9;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp10;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp10 = this.ILsatAdapterInstance.Close(1, out temp9);
            this.Manager.Checkpoint("MS-LSAT_R181");
            this.Manager.Checkpoint("MS-LSAT_R183");
            this.Manager.AddReturn(CloseInfo, null, temp9, temp10);
            TestScenarioS2S388();
        }
        
        private void TestScenarioS2S388() {
            this.Manager.Comment("reaching state \'S388\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.CloseInfo, null, new CloseDelegate1(this.TestScenarioS2S0CloseChecker)));
            this.Manager.Comment("reaching state \'S390\'");
        }
        
        private void TestScenarioS2S0CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S388");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, @return, "return of Close, state S388");
        }
        
        private void TestScenarioS2S0InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S1");
        }
        
        private void TestScenarioS2S321() {
            this.Manager.Comment("reaching state \'S321\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp16;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp17;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp18;
            this.Manager.Comment("executing step \'call LookUpSids2(1,{\"DoesNotExist\"},LookUpWKSTA,out _,out _)\'");
            temp18 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp16, out temp17);
            this.Manager.Checkpoint("MS-LSAT_R517");
            this.Manager.Checkpoint("MS-LSAT_R562");
            this.Manager.Comment("reaching state \'S353\'");
            this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:NoneMapped\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp16, "translateNames of LookUpSids2, state S353");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp17, "mapCount of LookUpSids2, state S353");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.NoneMapped, temp18, "return of LookUpSids2, state S353");
            TestScenarioS2S385();
        }
        
        private void TestScenarioS2S385() {
            this.Manager.Comment("reaching state \'S385\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp19;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp20;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp20 = this.ILsatAdapterInstance.Close(1, out temp19);
            this.Manager.Checkpoint("MS-LSAT_R181");
            this.Manager.Checkpoint("MS-LSAT_R183");
            this.Manager.AddReturn(CloseInfo, null, temp19, temp20);
            TestScenarioS2S389();
        }
        
        private void TestScenarioS2S389() {
            this.Manager.Comment("reaching state \'S389\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.CloseInfo, null, new CloseDelegate1(this.TestScenarioS2S0CloseChecker1)));
            this.Manager.Comment("reaching state \'S391\'");
        }
        
        private void TestScenarioS2S0CloseChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S389");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, @return, "return of Close, state S389");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S10() {
            this.Manager.BeginTest("TestScenarioS2S10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp22;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp22 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp22);
            this.Manager.Comment("reaching state \'S11\'");
            int temp43 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S10InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S10InitializeChecker1)));
            if ((temp43 == 0)) {
                this.Manager.Comment("reaching state \'S74\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp23;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp24;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp24 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp23);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S138\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp23, "openPolicyHandle2 of OpenPolicy2, state S138");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp24, "return of OpenPolicy2, state S138");
                this.Manager.Comment("reaching state \'S202\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp25;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp26;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp27;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQua" +
                        "lifiedName1\"},MSBSet,LookUpWKSTA,out _,out _)\'");
                temp27 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp25, out temp26);
                this.Manager.Checkpoint("MS-LSAT_R280");
                this.Manager.Checkpoint("MS-LSAT_R317");
                this.Manager.Comment("reaching state \'S266\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp25, "translateSids of LookUpNames3, state S266");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp26, "mapCount of LookUpNames3, state S266");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp27, "return of LookUpNames3, state S266");
                this.Manager.Comment("reaching state \'S330\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp28;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp29;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp30;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid1\",\"SecurityPrincipalSid" +
                        "2\",\"DomainSid1\",\"DomainSid2\"},Invalid,out _,out _)\'");
                temp30 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "SecurityPrincipalSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp28, out temp29);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S362\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp28, "translateNames of LookUpSids2, state S362");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp29, "mapCount of LookUpSids2, state S362");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp30, "return of LookUpSids2, state S362");
                TestScenarioS2S386();
                goto label1;
            }
            if ((temp43 == 1)) {
                this.Manager.Comment("reaching state \'S75\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp33;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp34;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp34 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp33);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S139\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp33, "openPolicyHandle2 of OpenPolicy2, state S139");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp34, "return of OpenPolicy2, state S139");
                this.Manager.Comment("reaching state \'S203\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp35;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp36;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp37;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"U" +
                        "nQualifiedName1\",\"IsolatedName1\"},MSBNotSet,LookUpWKSTA,out _,out _)\'");
                temp37 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp35, out temp36);
                this.Manager.Checkpoint("MS-LSAT_R280");
                this.Manager.Checkpoint("MS-LSAT_R317");
                this.Manager.Comment("reaching state \'S267\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp35, "translateSids of LookUpNames3, state S267");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp36, "mapCount of LookUpNames3, state S267");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp37, "return of LookUpNames3, state S267");
                this.Manager.Comment("reaching state \'S331\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp38;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp39;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp40;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid1\",\"SecurityPrincipalSid" +
                        "2\",\"DomainSid1\",\"DomainSid2\"},Invalid,out _,out _)\'");
                temp40 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "SecurityPrincipalSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp38, out temp39);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S363\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp38, "translateNames of LookUpSids2, state S363");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp39, "mapCount of LookUpSids2, state S363");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp40, "return of LookUpSids2, state S363");
                TestScenarioS2S387();
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S10InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S11");
        }
        
        private void TestScenarioS2S386() {
            this.Manager.Comment("reaching state \'S386\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp31;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp32;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp32 = this.ILsatAdapterInstance.Close(1, out temp31);
            this.Manager.Checkpoint("MS-LSAT_R181");
            this.Manager.Checkpoint("MS-LSAT_R183");
            this.Manager.AddReturn(CloseInfo, null, temp31, temp32);
            TestScenarioS2S388();
        }
        
        private void TestScenarioS2S10InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S11");
        }
        
        private void TestScenarioS2S387() {
            this.Manager.Comment("reaching state \'S387\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp41;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp42;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp42 = this.ILsatAdapterInstance.Close(1, out temp41);
            this.Manager.Checkpoint("MS-LSAT_R181");
            this.Manager.Checkpoint("MS-LSAT_R183");
            this.Manager.AddReturn(CloseInfo, null, temp41, temp42);
            TestScenarioS2S389();
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S12() {
            this.Manager.BeginTest("TestScenarioS2S12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp44;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp44 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp44);
            this.Manager.Comment("reaching state \'S13\'");
            int temp61 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S12InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S12InitializeChecker1)));
            if ((temp61 == 0)) {
                this.Manager.Comment("reaching state \'S76\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp45;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp46;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp46 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp45);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S140\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp45, "openPolicyHandle2 of OpenPolicy2, state S140");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp46, "return of OpenPolicy2, state S140");
                this.Manager.Comment("reaching state \'S204\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp47;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp48;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp49;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQua" +
                        "lifiedName1\"},MSBNotSet,LookUpWKSTA,out _,out _)\'");
                temp49 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp47, out temp48);
                this.Manager.Checkpoint("MS-LSAT_R280");
                this.Manager.Checkpoint("MS-LSAT_R317");
                this.Manager.Comment("reaching state \'S268\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp47, "translateSids of LookUpNames3, state S268");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp48, "mapCount of LookUpNames3, state S268");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp49, "return of LookUpNames3, state S268");
                this.Manager.Comment("reaching state \'S332\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp50;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp51;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp52;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"DoesNotExist\"},Invalid,out _,out _)\'");
                temp52 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp50, out temp51);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S364\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp50, "translateNames of LookUpSids2, state S364");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp51, "mapCount of LookUpSids2, state S364");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp52, "return of LookUpSids2, state S364");
                TestScenarioS2S386();
                goto label2;
            }
            if ((temp61 == 1)) {
                this.Manager.Comment("reaching state \'S77\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp53;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp54;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp54 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp53);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S141\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp53, "openPolicyHandle2 of OpenPolicy2, state S141");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp54, "return of OpenPolicy2, state S141");
                this.Manager.Comment("reaching state \'S205\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp55;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp56;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp57;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"U" +
                        "nQualifiedName1\",\"IsolatedName1\"},MSBSet,LookUpWKSTA,out _,out _)\'");
                temp57 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp55, out temp56);
                this.Manager.Checkpoint("MS-LSAT_R280");
                this.Manager.Checkpoint("MS-LSAT_R317");
                this.Manager.Comment("reaching state \'S269\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp55, "translateSids of LookUpNames3, state S269");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp56, "mapCount of LookUpNames3, state S269");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp57, "return of LookUpNames3, state S269");
                this.Manager.Comment("reaching state \'S333\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp58;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp59;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp60;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"DoesNotExist\"},Invalid,out _,out _)\'");
                temp60 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp58, out temp59);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S365\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp58, "translateNames of LookUpSids2, state S365");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp59, "mapCount of LookUpSids2, state S365");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp60, "return of LookUpSids2, state S365");
                TestScenarioS2S387();
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S12InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S13");
        }
        
        private void TestScenarioS2S12InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S13");
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S14() {
            this.Manager.BeginTest("TestScenarioS2S14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp62;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp62 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp62);
            this.Manager.Comment("reaching state \'S15\'");
            int temp79 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S14InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S14InitializeChecker1)));
            if ((temp79 == 0)) {
                this.Manager.Comment("reaching state \'S78\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp63;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp64;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp64 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp63);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S142\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp63, "openPolicyHandle2 of OpenPolicy2, state S142");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp64, "return of OpenPolicy2, state S142");
                this.Manager.Comment("reaching state \'S206\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp65;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp66;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp67;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalN" +
                        "ame\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},MSBNotSet,Invalid,out " +
                        "_,out _)\'");
                temp67 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp65, out temp66);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S270\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp65, "translateSids of LookUpNames3, state S270");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp66, "mapCount of LookUpNames3, state S270");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp67, "return of LookUpNames3, state S270");
                this.Manager.Comment("reaching state \'S334\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp68;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp69;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp70;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid1\",\"DomainSid1\",\"DoesNot" +
                        "Exist\"},Invalid,out _,out _)\'");
                temp70 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp68, out temp69);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S366\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp68, "translateNames of LookUpSids2, state S366");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp69, "mapCount of LookUpSids2, state S366");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp70, "return of LookUpSids2, state S366");
                TestScenarioS2S386();
                goto label3;
            }
            if ((temp79 == 1)) {
                this.Manager.Comment("reaching state \'S79\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp71;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp72;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp72 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp71);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S143\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp71, "openPolicyHandle2 of OpenPolicy2, state S143");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp72, "return of OpenPolicy2, state S143");
                this.Manager.Comment("reaching state \'S207\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp73;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp74;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp75;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalN" +
                        "ame\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},MSBNotSet,Invalid,out " +
                        "_,out _)\'");
                temp75 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp73, out temp74);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S271\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp73, "translateSids of LookUpNames3, state S271");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp74, "mapCount of LookUpNames3, state S271");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp75, "return of LookUpNames3, state S271");
                this.Manager.Comment("reaching state \'S335\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp76;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp77;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp78;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid1\",\"DomainSid1\",\"DoesNot" +
                        "Exist\"},Invalid,out _,out _)\'");
                temp78 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp76, out temp77);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S367\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp76, "translateNames of LookUpSids2, state S367");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp77, "mapCount of LookUpSids2, state S367");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp78, "return of LookUpSids2, state S367");
                TestScenarioS2S387();
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S14InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S15");
        }
        
        private void TestScenarioS2S14InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S15");
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S16() {
            this.Manager.BeginTest("TestScenarioS2S16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp80;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp80 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp80);
            this.Manager.Comment("reaching state \'S17\'");
            int temp97 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S16InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S16InitializeChecker1)));
            if ((temp97 == 0)) {
                this.Manager.Comment("reaching state \'S80\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp81;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp82;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp82 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp81);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S144\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp81, "openPolicyHandle2 of OpenPolicy2, state S144");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp82, "return of OpenPolicy2, state S144");
                this.Manager.Comment("reaching state \'S208\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp83;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp84;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp85;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalN" +
                        "ame\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},MSBNotSet,LookUpWKSTA," +
                        "out _,out _)\'");
                temp85 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp83, out temp84);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S272\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp83, "translateSids of LookUpNames3, state S272");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp84, "mapCount of LookUpNames3, state S272");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp85, "return of LookUpNames3, state S272");
                this.Manager.Comment("reaching state \'S336\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp86;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp87;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp88;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid2\",\"InvalidSecurityPrinc" +
                        "ipalSid\",\"DomainSid2\",\"InvalidDomainSid\"},LookUpWKSTA,out _,out _)\'");
                temp88 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidSecurityPrincipalSid", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidDomainSid", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp86, out temp87);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S368\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp86, "translateNames of LookUpSids2, state S368");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp87, "mapCount of LookUpSids2, state S368");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp88, "return of LookUpSids2, state S368");
                TestScenarioS2S386();
                goto label4;
            }
            if ((temp97 == 1)) {
                this.Manager.Comment("reaching state \'S81\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp89;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp90;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp90 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp89);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S145\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp89, "openPolicyHandle2 of OpenPolicy2, state S145");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp90, "return of OpenPolicy2, state S145");
                this.Manager.Comment("reaching state \'S209\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp91;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp92;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp93;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalN" +
                        "ame\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},MSBNotSet,LookUpWKSTA," +
                        "out _,out _)\'");
                temp93 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp91, out temp92);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S273\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp91, "translateSids of LookUpNames3, state S273");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp92, "mapCount of LookUpNames3, state S273");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp93, "return of LookUpNames3, state S273");
                this.Manager.Comment("reaching state \'S337\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp94;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp95;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp96;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid2\",\"InvalidSecurityPrinc" +
                        "ipalSid\",\"DomainSid2\",\"InvalidDomainSid\"},LookUpWKSTA,out _,out _)\'");
                temp96 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidSecurityPrincipalSid", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidDomainSid", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp94, out temp95);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S369\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp94, "translateNames of LookUpSids2, state S369");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp95, "mapCount of LookUpSids2, state S369");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp96, "return of LookUpSids2, state S369");
                TestScenarioS2S387();
                goto label4;
            }
            throw new InvalidOperationException("never reached");
        label4:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S16InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S17");
        }
        
        private void TestScenarioS2S16InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S17");
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S18() {
            this.Manager.BeginTest("TestScenarioS2S18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp98;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp98 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp98);
            this.Manager.Comment("reaching state \'S19\'");
            int temp115 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S18InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S18InitializeChecker1)));
            if ((temp115 == 0)) {
                this.Manager.Comment("reaching state \'S82\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp99;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp100;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp100 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp99);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S146\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp99, "openPolicyHandle2 of OpenPolicy2, state S146");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp100, "return of OpenPolicy2, state S146");
                this.Manager.Comment("reaching state \'S210\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp101;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp102;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp103;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalN" +
                        "ame\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},MSBSet,LookUpWKSTA,out" +
                        " _,out _)\'");
                temp103 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp101, out temp102);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S274\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp101, "translateSids of LookUpNames3, state S274");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp102, "mapCount of LookUpNames3, state S274");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp103, "return of LookUpNames3, state S274");
                TestScenarioS2S322();
                goto label5;
            }
            if ((temp115 == 1)) {
                this.Manager.Comment("reaching state \'S83\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp107;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp108;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp108 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp107);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S147\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp107, "openPolicyHandle2 of OpenPolicy2, state S147");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp108, "return of OpenPolicy2, state S147");
                this.Manager.Comment("reaching state \'S211\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp109;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp110;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp111;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalN" +
                        "ame\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},MSBSet,LookUpWKSTA,out" +
                        " _,out _)\'");
                temp111 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp109, out temp110);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S275\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp109, "translateSids of LookUpNames3, state S275");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp110, "mapCount of LookUpNames3, state S275");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp111, "return of LookUpNames3, state S275");
                TestScenarioS2S323();
                goto label5;
            }
            throw new InvalidOperationException("never reached");
        label5:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S18InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S19");
        }
        
        private void TestScenarioS2S322() {
            this.Manager.Comment("reaching state \'S322\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp104;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp105;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp106;
            this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid2\",\"InvalidSecurityPrinc" +
                    "ipalSid\",\"DomainSid2\",\"InvalidDomainSid\"},Invalid,out _,out _)\'");
            temp106 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidSecurityPrincipalSid", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidDomainSid", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp104, out temp105);
            this.Manager.Checkpoint("MS-LSAT_R516");
            this.Manager.Comment("reaching state \'S354\'");
            this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp104, "translateNames of LookUpSids2, state S354");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp105, "mapCount of LookUpSids2, state S354");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp106, "return of LookUpSids2, state S354");
            TestScenarioS2S386();
        }
        
        private void TestScenarioS2S18InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S19");
        }
        
        private void TestScenarioS2S323() {
            this.Manager.Comment("reaching state \'S323\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp112;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp113;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp114;
            this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid2\",\"InvalidSecurityPrinc" +
                    "ipalSid\",\"DomainSid2\",\"InvalidDomainSid\"},Invalid,out _,out _)\'");
            temp114 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidSecurityPrincipalSid", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidDomainSid", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                            "Element"}, new object[] {
                                            Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp112, out temp113);
            this.Manager.Checkpoint("MS-LSAT_R516");
            this.Manager.Comment("reaching state \'S355\'");
            this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp112, "translateNames of LookUpSids2, state S355");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp113, "mapCount of LookUpSids2, state S355");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp114, "return of LookUpSids2, state S355");
            TestScenarioS2S387();
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S2() {
            this.Manager.BeginTest("TestScenarioS2S2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp116;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp116 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp116);
            this.Manager.Comment("reaching state \'S3\'");
            int temp127 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S2InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S2InitializeChecker1)));
            if ((temp127 == 0)) {
                this.Manager.Comment("reaching state \'S66\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp117;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp118;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp118 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp117);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S130\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp117, "openPolicyHandle2 of OpenPolicy2, state S130");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp118, "return of OpenPolicy2, state S130");
                this.Manager.Comment("reaching state \'S194\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp119;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp120;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp121;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"U" +
                        "nQualifiedName1\",\"IsolatedName1\"},MSBSet,LookUpWKSTA,out _,out _)\'");
                temp121 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp119, out temp120);
                this.Manager.Checkpoint("MS-LSAT_R280");
                this.Manager.Checkpoint("MS-LSAT_R317");
                this.Manager.Comment("reaching state \'S258\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp119, "translateSids of LookUpNames3, state S258");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp120, "mapCount of LookUpNames3, state S258");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp121, "return of LookUpNames3, state S258");
                TestScenarioS2S322();
                goto label6;
            }
            if ((temp127 == 1)) {
                this.Manager.Comment("reaching state \'S67\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp122;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp123;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp123 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp122);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S131\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp122, "openPolicyHandle2 of OpenPolicy2, state S131");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp123, "return of OpenPolicy2, state S131");
                this.Manager.Comment("reaching state \'S195\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp124;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp125;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp126;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQua" +
                        "lifiedName1\"},MSBSet,LookUpWKSTA,out _,out _)\'");
                temp126 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp124, out temp125);
                this.Manager.Checkpoint("MS-LSAT_R280");
                this.Manager.Checkpoint("MS-LSAT_R317");
                this.Manager.Comment("reaching state \'S259\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp124, "translateSids of LookUpNames3, state S259");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp125, "mapCount of LookUpNames3, state S259");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp126, "return of LookUpNames3, state S259");
                TestScenarioS2S323();
                goto label6;
            }
            throw new InvalidOperationException("never reached");
        label6:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S2InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S3");
        }
        
        private void TestScenarioS2S2InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S3");
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S20() {
            this.Manager.BeginTest("TestScenarioS2S20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp128;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp128 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp128);
            this.Manager.Comment("reaching state \'S21\'");
            int temp139 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S20InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S20InitializeChecker1)));
            if ((temp139 == 0)) {
                this.Manager.Comment("reaching state \'S84\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp129;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp130;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp130 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp129);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S148\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp129, "openPolicyHandle2 of OpenPolicy2, state S148");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp130, "return of OpenPolicy2, state S148");
                this.Manager.Comment("reaching state \'S212\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp131;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp132;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp133;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalN" +
                        "ame\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},MSBSet,Invalid,out _,o" +
                        "ut _)\'");
                temp133 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp131, out temp132);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Checkpoint("MS-LSAT_R286");
                this.Manager.Comment("reaching state \'S276\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp131, "translateSids of LookUpNames3, state S276");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp132, "mapCount of LookUpNames3, state S276");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp133, "return of LookUpNames3, state S276");
                TestScenarioS2S322();
                goto label7;
            }
            if ((temp139 == 1)) {
                this.Manager.Comment("reaching state \'S85\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp134;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp135;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp135 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp134);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S149\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp134, "openPolicyHandle2 of OpenPolicy2, state S149");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp135, "return of OpenPolicy2, state S149");
                this.Manager.Comment("reaching state \'S213\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp136;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp137;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp138;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalN" +
                        "ame\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},MSBSet,Invalid,out _,o" +
                        "ut _)\'");
                temp138 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp136, out temp137);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Checkpoint("MS-LSAT_R286");
                this.Manager.Comment("reaching state \'S277\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp136, "translateSids of LookUpNames3, state S277");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp137, "mapCount of LookUpNames3, state S277");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp138, "return of LookUpNames3, state S277");
                TestScenarioS2S323();
                goto label7;
            }
            throw new InvalidOperationException("never reached");
        label7:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S20InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S21");
        }
        
        private void TestScenarioS2S20InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S21");
        }
        #endregion
        
        #region Test Starting in S22
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S22() {
            this.Manager.BeginTest("TestScenarioS2S22");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp140;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp140 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp140);
            this.Manager.Comment("reaching state \'S23\'");
            int temp151 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S22InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S22InitializeChecker1)));
            if ((temp151 == 0)) {
                this.Manager.Comment("reaching state \'S86\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp141;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp142;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp142 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp141);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S150\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp141, "openPolicyHandle2 of OpenPolicy2, state S150");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp142, "return of OpenPolicy2, state S150");
                this.Manager.Comment("reaching state \'S214\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp143;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp144;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp145;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQua" +
                        "lifiedName1\"},MSBSet,Invalid,out _,out _)\'");
                temp145 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp143, out temp144);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Checkpoint("MS-LSAT_R286");
                this.Manager.Comment("reaching state \'S278\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp143, "translateSids of LookUpNames3, state S278");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp144, "mapCount of LookUpNames3, state S278");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp145, "return of LookUpNames3, state S278");
                TestScenarioS2S322();
                goto label8;
            }
            if ((temp151 == 1)) {
                this.Manager.Comment("reaching state \'S87\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp146;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp147;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp147 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp146);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S151\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp146, "openPolicyHandle2 of OpenPolicy2, state S151");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp147, "return of OpenPolicy2, state S151");
                this.Manager.Comment("reaching state \'S215\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp148;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp149;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp150;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQua" +
                        "lifiedName1\"},MSBSet,Invalid,out _,out _)\'");
                temp150 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp148, out temp149);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Checkpoint("MS-LSAT_R286");
                this.Manager.Comment("reaching state \'S279\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp148, "translateSids of LookUpNames3, state S279");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp149, "mapCount of LookUpNames3, state S279");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp150, "return of LookUpNames3, state S279");
                TestScenarioS2S323();
                goto label8;
            }
            throw new InvalidOperationException("never reached");
        label8:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S22InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S23");
        }
        
        private void TestScenarioS2S22InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S23");
        }
        #endregion
        
        #region Test Starting in S24
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S24() {
            this.Manager.BeginTest("TestScenarioS2S24");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp152;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp152 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp152);
            this.Manager.Comment("reaching state \'S25\'");
            int temp163 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S24InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S24InitializeChecker1)));
            if ((temp163 == 0)) {
                this.Manager.Comment("reaching state \'S88\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp153;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp154;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp154 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp153);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S152\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp153, "openPolicyHandle2 of OpenPolicy2, state S152");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp154, "return of OpenPolicy2, state S152");
                this.Manager.Comment("reaching state \'S216\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp155;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp156;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp157;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQua" +
                        "lifiedName1\"},MSBNotSet,Invalid,out _,out _)\'");
                temp157 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp155, out temp156);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S280\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp155, "translateSids of LookUpNames3, state S280");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp156, "mapCount of LookUpNames3, state S280");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp157, "return of LookUpNames3, state S280");
                TestScenarioS2S322();
                goto label9;
            }
            if ((temp163 == 1)) {
                this.Manager.Comment("reaching state \'S89\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp158;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp159;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp159 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp158);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S153\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp158, "openPolicyHandle2 of OpenPolicy2, state S153");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp159, "return of OpenPolicy2, state S153");
                this.Manager.Comment("reaching state \'S217\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp160;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp161;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp162;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQua" +
                        "lifiedName1\"},MSBNotSet,Invalid,out _,out _)\'");
                temp162 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp160, out temp161);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S281\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp160, "translateSids of LookUpNames3, state S281");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp161, "mapCount of LookUpNames3, state S281");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp162, "return of LookUpNames3, state S281");
                TestScenarioS2S323();
                goto label9;
            }
            throw new InvalidOperationException("never reached");
        label9:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S24InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S25");
        }
        
        private void TestScenarioS2S24InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S25");
        }
        #endregion
        
        #region Test Starting in S26
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S26() {
            this.Manager.BeginTest("TestScenarioS2S26");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp164;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp164 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp164);
            this.Manager.Comment("reaching state \'S27\'");
            int temp175 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S26InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S26InitializeChecker1)));
            if ((temp175 == 0)) {
                this.Manager.Comment("reaching state \'S90\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp165;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp166;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp166 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp165);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S154\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp165, "openPolicyHandle2 of OpenPolicy2, state S154");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp166, "return of OpenPolicy2, state S154");
                this.Manager.Comment("reaching state \'S218\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp167;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp168;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp169;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"U" +
                        "nQualifiedName1\",\"IsolatedName1\"},MSBSet,Invalid,out _,out _)\'");
                temp169 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp167, out temp168);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Checkpoint("MS-LSAT_R286");
                this.Manager.Comment("reaching state \'S282\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp167, "translateSids of LookUpNames3, state S282");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp168, "mapCount of LookUpNames3, state S282");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp169, "return of LookUpNames3, state S282");
                TestScenarioS2S322();
                goto label10;
            }
            if ((temp175 == 1)) {
                this.Manager.Comment("reaching state \'S91\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp170;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp171;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp171 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp170);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S155\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp170, "openPolicyHandle2 of OpenPolicy2, state S155");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp171, "return of OpenPolicy2, state S155");
                this.Manager.Comment("reaching state \'S219\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp172;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp173;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp174;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"U" +
                        "nQualifiedName1\",\"IsolatedName1\"},MSBSet,Invalid,out _,out _)\'");
                temp174 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp172, out temp173);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Checkpoint("MS-LSAT_R286");
                this.Manager.Comment("reaching state \'S283\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp172, "translateSids of LookUpNames3, state S283");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp173, "mapCount of LookUpNames3, state S283");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp174, "return of LookUpNames3, state S283");
                TestScenarioS2S323();
                goto label10;
            }
            throw new InvalidOperationException("never reached");
        label10:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S26InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S27");
        }
        
        private void TestScenarioS2S26InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S27");
        }
        #endregion
        
        #region Test Starting in S28
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S28() {
            this.Manager.BeginTest("TestScenarioS2S28");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp176;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp176 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp176);
            this.Manager.Comment("reaching state \'S29\'");
            int temp187 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S28InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S28InitializeChecker1)));
            if ((temp187 == 0)) {
                this.Manager.Comment("reaching state \'S92\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp177;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp178;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp178 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp177);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S156\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp177, "openPolicyHandle2 of OpenPolicy2, state S156");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp178, "return of OpenPolicy2, state S156");
                this.Manager.Comment("reaching state \'S220\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp179;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp180;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp181;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"U" +
                        "nQualifiedName1\",\"IsolatedName1\"},MSBNotSet,Invalid,out _,out _)\'");
                temp181 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp179, out temp180);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S284\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp179, "translateSids of LookUpNames3, state S284");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp180, "mapCount of LookUpNames3, state S284");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp181, "return of LookUpNames3, state S284");
                TestScenarioS2S322();
                goto label11;
            }
            if ((temp187 == 1)) {
                this.Manager.Comment("reaching state \'S93\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp182;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp183;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp183 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp182);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S157\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp182, "openPolicyHandle2 of OpenPolicy2, state S157");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp183, "return of OpenPolicy2, state S157");
                this.Manager.Comment("reaching state \'S221\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp184;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp185;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp186;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"U" +
                        "nQualifiedName1\",\"IsolatedName1\"},MSBNotSet,Invalid,out _,out _)\'");
                temp186 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp184, out temp185);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S285\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp184, "translateSids of LookUpNames3, state S285");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp185, "mapCount of LookUpNames3, state S285");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp186, "return of LookUpNames3, state S285");
                TestScenarioS2S323();
                goto label11;
            }
            throw new InvalidOperationException("never reached");
        label11:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S28InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S29");
        }
        
        private void TestScenarioS2S28InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S29");
        }
        #endregion
        
        #region Test Starting in S30
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S30() {
            this.Manager.BeginTest("TestScenarioS2S30");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp188;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp188 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp188);
            this.Manager.Comment("reaching state \'S31\'");
            int temp199 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S30InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S30InitializeChecker1)));
            if ((temp199 == 0)) {
                this.Manager.Comment("reaching state \'S94\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp189;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp190;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp190 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp189);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S158\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp189, "openPolicyHandle2 of OpenPolicy2, state S158");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp190, "return of OpenPolicy2, state S158");
                this.Manager.Comment("reaching state \'S222\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp191;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp192;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp193;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"DoesNotExist\"},MSBSet,Invalid,out _,out _)\'" +
                        "");
                temp193 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp191, out temp192);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Checkpoint("MS-LSAT_R286");
                this.Manager.Comment("reaching state \'S286\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp191, "translateSids of LookUpNames3, state S286");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp192, "mapCount of LookUpNames3, state S286");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp193, "return of LookUpNames3, state S286");
                TestScenarioS2S322();
                goto label12;
            }
            if ((temp199 == 1)) {
                this.Manager.Comment("reaching state \'S95\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp194;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp195;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp195 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp194);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S159\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp194, "openPolicyHandle2 of OpenPolicy2, state S159");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp195, "return of OpenPolicy2, state S159");
                this.Manager.Comment("reaching state \'S223\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp196;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp197;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp198;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"DoesNotExist\"},MSBSet,Invalid,out _,out _)\'" +
                        "");
                temp198 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp196, out temp197);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Checkpoint("MS-LSAT_R286");
                this.Manager.Comment("reaching state \'S287\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp196, "translateSids of LookUpNames3, state S287");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp197, "mapCount of LookUpNames3, state S287");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp198, "return of LookUpNames3, state S287");
                TestScenarioS2S323();
                goto label12;
            }
            throw new InvalidOperationException("never reached");
        label12:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S30InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S31");
        }
        
        private void TestScenarioS2S30InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S31");
        }
        #endregion
        
        #region Test Starting in S32
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S32() {
            this.Manager.BeginTest("TestScenarioS2S32");
            this.Manager.Comment("reaching state \'S32\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp200;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp200 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp200);
            this.Manager.Comment("reaching state \'S33\'");
            int temp211 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S32InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S32InitializeChecker1)));
            if ((temp211 == 0)) {
                this.Manager.Comment("reaching state \'S96\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp201;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp202;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp202 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp201);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S160\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp201, "openPolicyHandle2 of OpenPolicy2, state S160");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp202, "return of OpenPolicy2, state S160");
                this.Manager.Comment("reaching state \'S224\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp203;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp204;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp205;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"U" +
                        "nQualifiedName1\",\"IsolatedName1\"},MSBNotSet,LookUpWKSTA,out _,out _)\'");
                temp205 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp203, out temp204);
                this.Manager.Checkpoint("MS-LSAT_R278");
                this.Manager.Comment("reaching state \'S288\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Valid,out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(0)), temp203, "translateSids of LookUpNames3, state S288");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(0)), temp204, "mapCount of LookUpNames3, state S288");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp205, "return of LookUpNames3, state S288");
                TestScenarioS2S321();
                goto label13;
            }
            if ((temp211 == 1)) {
                this.Manager.Comment("reaching state \'S97\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp206;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp207;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp207 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp206);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S161\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp206, "openPolicyHandle2 of OpenPolicy2, state S161");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp207, "return of OpenPolicy2, state S161");
                this.Manager.Comment("reaching state \'S225\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp208;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp209;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp210;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"U" +
                        "nQualifiedName1\",\"IsolatedName1\"},MSBNotSet,LookUpWKSTA,out _,out _)\'");
                temp210 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp208, out temp209);
                this.Manager.Checkpoint("MS-LSAT_R278");
                this.Manager.Comment("reaching state \'S289\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Valid,out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(0)), temp208, "translateSids of LookUpNames3, state S289");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(0)), temp209, "mapCount of LookUpNames3, state S289");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp210, "return of LookUpNames3, state S289");
                TestScenarioS2S320();
                goto label13;
            }
            throw new InvalidOperationException("never reached");
        label13:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S32InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S33");
        }
        
        private void TestScenarioS2S32InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S33");
        }
        #endregion
        
        #region Test Starting in S34
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S34() {
            this.Manager.BeginTest("TestScenarioS2S34");
            this.Manager.Comment("reaching state \'S34\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp212;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp212 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp212);
            this.Manager.Comment("reaching state \'S35\'");
            int temp229 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S34InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S34InitializeChecker1)));
            if ((temp229 == 0)) {
                this.Manager.Comment("reaching state \'S98\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp213;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp214;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp214 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp213);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S162\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp213, "openPolicyHandle2 of OpenPolicy2, state S162");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp214, "return of OpenPolicy2, state S162");
                this.Manager.Comment("reaching state \'S226\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp215;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp216;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp217;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"U" +
                        "nQualifiedName1\",\"IsolatedName1\"},MSBSet,LookUpWKSTA,out _,out _)\'");
                temp217 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp215, out temp216);
                this.Manager.Checkpoint("MS-LSAT_R278");
                this.Manager.Comment("reaching state \'S290\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Valid,out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(0)), temp215, "translateSids of LookUpNames3, state S290");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(0)), temp216, "mapCount of LookUpNames3, state S290");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp217, "return of LookUpNames3, state S290");
                this.Manager.Comment("reaching state \'S338\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp218;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp219;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp220;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid1\",\"SecurityPrincipalSid" +
                        "2\",\"DomainSid1\",\"DomainSid2\"},LookUpWKSTA,out _,out _)\'");
                temp220 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "SecurityPrincipalSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp218, out temp219);
                this.Manager.Checkpoint("MS-LSAT_R507");
                this.Manager.Checkpoint("MS-LSAT_R513");
                this.Manager.Comment("reaching state \'S370\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Valid,out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(0)), temp218, "translateNames of LookUpSids2, state S370");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(0)), temp219, "mapCount of LookUpSids2, state S370");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp220, "return of LookUpSids2, state S370");
                TestScenarioS2S385();
                goto label14;
            }
            if ((temp229 == 1)) {
                this.Manager.Comment("reaching state \'S99\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp221;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp222;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp222 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp221);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S163\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp221, "openPolicyHandle2 of OpenPolicy2, state S163");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp222, "return of OpenPolicy2, state S163");
                this.Manager.Comment("reaching state \'S227\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp223;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp224;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp225;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"U" +
                        "nQualifiedName1\",\"IsolatedName1\"},MSBSet,LookUpWKSTA,out _,out _)\'");
                temp225 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp223, out temp224);
                this.Manager.Checkpoint("MS-LSAT_R278");
                this.Manager.Comment("reaching state \'S291\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Valid,out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(0)), temp223, "translateSids of LookUpNames3, state S291");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(0)), temp224, "mapCount of LookUpNames3, state S291");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp225, "return of LookUpNames3, state S291");
                this.Manager.Comment("reaching state \'S339\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp226;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp227;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp228;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid1\",\"SecurityPrincipalSid" +
                        "2\",\"DomainSid1\",\"DomainSid2\"},LookUpWKSTA,out _,out _)\'");
                temp228 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "SecurityPrincipalSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp226, out temp227);
                this.Manager.Checkpoint("MS-LSAT_R507");
                this.Manager.Checkpoint("MS-LSAT_R513");
                this.Manager.Comment("reaching state \'S371\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Valid,out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(0)), temp226, "translateNames of LookUpSids2, state S371");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(0)), temp227, "mapCount of LookUpSids2, state S371");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp228, "return of LookUpSids2, state S371");
                TestScenarioS2S384();
                goto label14;
            }
            throw new InvalidOperationException("never reached");
        label14:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S34InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S35");
        }
        
        private void TestScenarioS2S34InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S35");
        }
        #endregion
        
        #region Test Starting in S36
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S36() {
            this.Manager.BeginTest("TestScenarioS2S36");
            this.Manager.Comment("reaching state \'S36\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp230;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp230 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp230);
            this.Manager.Comment("reaching state \'S37\'");
            int temp247 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S36InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S36InitializeChecker1)));
            if ((temp247 == 0)) {
                this.Manager.Comment("reaching state \'S100\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp231;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp232;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp232 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp231);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S164\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp231, "openPolicyHandle2 of OpenPolicy2, state S164");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp232, "return of OpenPolicy2, state S164");
                this.Manager.Comment("reaching state \'S228\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp233;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp234;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp235;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalN" +
                        "ame\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},MSBNotSet,Invalid,out " +
                        "_,out _)\'");
                temp235 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp233, out temp234);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S292\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp233, "translateSids of LookUpNames3, state S292");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp234, "mapCount of LookUpNames3, state S292");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp235, "return of LookUpNames3, state S292");
                this.Manager.Comment("reaching state \'S340\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp236;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp237;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp238;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid1\",\"SecurityPrincipalSid" +
                        "2\",\"DomainSid1\",\"DomainSid2\"},Invalid,out _,out _)\'");
                temp238 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "SecurityPrincipalSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp236, out temp237);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S372\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp236, "translateNames of LookUpSids2, state S372");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp237, "mapCount of LookUpSids2, state S372");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp238, "return of LookUpSids2, state S372");
                TestScenarioS2S385();
                goto label15;
            }
            if ((temp247 == 1)) {
                this.Manager.Comment("reaching state \'S101\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp239;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp240;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp240 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp239);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S165\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp239, "openPolicyHandle2 of OpenPolicy2, state S165");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp240, "return of OpenPolicy2, state S165");
                this.Manager.Comment("reaching state \'S229\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp241;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp242;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp243;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalN" +
                        "ame\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},MSBNotSet,Invalid,out " +
                        "_,out _)\'");
                temp243 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp241, out temp242);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S293\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp241, "translateSids of LookUpNames3, state S293");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp242, "mapCount of LookUpNames3, state S293");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp243, "return of LookUpNames3, state S293");
                this.Manager.Comment("reaching state \'S341\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp244;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp245;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp246;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid1\",\"SecurityPrincipalSid" +
                        "2\",\"DomainSid1\",\"DomainSid2\"},Invalid,out _,out _)\'");
                temp246 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "SecurityPrincipalSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp244, out temp245);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S373\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp244, "translateNames of LookUpSids2, state S373");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp245, "mapCount of LookUpSids2, state S373");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp246, "return of LookUpSids2, state S373");
                TestScenarioS2S384();
                goto label15;
            }
            throw new InvalidOperationException("never reached");
        label15:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S36InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S37");
        }
        
        private void TestScenarioS2S36InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S37");
        }
        #endregion
        
        #region Test Starting in S38
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S38() {
            this.Manager.BeginTest("TestScenarioS2S38");
            this.Manager.Comment("reaching state \'S38\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp248;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp248 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp248);
            this.Manager.Comment("reaching state \'S39\'");
            int temp265 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S38InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S38InitializeChecker1)));
            if ((temp265 == 0)) {
                this.Manager.Comment("reaching state \'S102\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp249;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp250;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp250 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp249);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S166\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp249, "openPolicyHandle2 of OpenPolicy2, state S166");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp250, "return of OpenPolicy2, state S166");
                this.Manager.Comment("reaching state \'S230\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp251;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp252;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp253;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalN" +
                        "ame\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},MSBNotSet,LookUpWKSTA," +
                        "out _,out _)\'");
                temp253 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp251, out temp252);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S294\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp251, "translateSids of LookUpNames3, state S294");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp252, "mapCount of LookUpNames3, state S294");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp253, "return of LookUpNames3, state S294");
                this.Manager.Comment("reaching state \'S342\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp254;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp255;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp256;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"DoesNotExist\"},Invalid,out _,out _)\'");
                temp256 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp254, out temp255);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S374\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp254, "translateNames of LookUpSids2, state S374");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp255, "mapCount of LookUpSids2, state S374");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp256, "return of LookUpSids2, state S374");
                TestScenarioS2S385();
                goto label16;
            }
            if ((temp265 == 1)) {
                this.Manager.Comment("reaching state \'S103\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp257;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp258;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp258 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp257);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S167\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp257, "openPolicyHandle2 of OpenPolicy2, state S167");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp258, "return of OpenPolicy2, state S167");
                this.Manager.Comment("reaching state \'S231\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp259;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp260;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp261;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalN" +
                        "ame\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},MSBNotSet,LookUpWKSTA," +
                        "out _,out _)\'");
                temp261 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp259, out temp260);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S295\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp259, "translateSids of LookUpNames3, state S295");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp260, "mapCount of LookUpNames3, state S295");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp261, "return of LookUpNames3, state S295");
                this.Manager.Comment("reaching state \'S343\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp262;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp263;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp264;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"DoesNotExist\"},Invalid,out _,out _)\'");
                temp264 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp262, out temp263);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S375\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp262, "translateNames of LookUpSids2, state S375");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp263, "mapCount of LookUpSids2, state S375");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp264, "return of LookUpSids2, state S375");
                TestScenarioS2S384();
                goto label16;
            }
            throw new InvalidOperationException("never reached");
        label16:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S38InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S39");
        }
        
        private void TestScenarioS2S38InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S39");
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S4() {
            this.Manager.BeginTest("TestScenarioS2S4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp266;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp266 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp266);
            this.Manager.Comment("reaching state \'S5\'");
            int temp283 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S4InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S4InitializeChecker1)));
            if ((temp283 == 0)) {
                this.Manager.Comment("reaching state \'S68\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp267;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp268;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp268 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp267);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S132\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp267, "openPolicyHandle2 of OpenPolicy2, state S132");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp268, "return of OpenPolicy2, state S132");
                this.Manager.Comment("reaching state \'S196\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp269;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp270;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp271;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"U" +
                        "nQualifiedName1\",\"IsolatedName1\"},MSBNotSet,LookUpWKSTA,out _,out _)\'");
                temp271 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp269, out temp270);
                this.Manager.Checkpoint("MS-LSAT_R280");
                this.Manager.Checkpoint("MS-LSAT_R317");
                this.Manager.Comment("reaching state \'S260\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp269, "translateSids of LookUpNames3, state S260");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp270, "mapCount of LookUpNames3, state S260");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp271, "return of LookUpNames3, state S260");
                this.Manager.Comment("reaching state \'S324\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp272;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp273;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp274;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid1\",\"SecurityPrincipalSid" +
                        "2\",\"DomainSid1\",\"DomainSid2\"},LookUpWKSTA,out _,out _)\'");
                temp274 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "SecurityPrincipalSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp272, out temp273);
                this.Manager.Checkpoint("MS-LSAT_R566");
                this.Manager.Checkpoint("MS-LSAT_R515");
                this.Manager.Comment("reaching state \'S356\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp272, "translateNames of LookUpSids2, state S356");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp273, "mapCount of LookUpSids2, state S356");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp274, "return of LookUpSids2, state S356");
                TestScenarioS2S386();
                goto label17;
            }
            if ((temp283 == 1)) {
                this.Manager.Comment("reaching state \'S69\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp275;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp276;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp276 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp275);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S133\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp275, "openPolicyHandle2 of OpenPolicy2, state S133");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp276, "return of OpenPolicy2, state S133");
                this.Manager.Comment("reaching state \'S197\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp277;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp278;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp279;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQua" +
                        "lifiedName1\"},MSBNotSet,LookUpWKSTA,out _,out _)\'");
                temp279 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp277, out temp278);
                this.Manager.Checkpoint("MS-LSAT_R280");
                this.Manager.Checkpoint("MS-LSAT_R317");
                this.Manager.Comment("reaching state \'S261\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp277, "translateSids of LookUpNames3, state S261");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp278, "mapCount of LookUpNames3, state S261");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp279, "return of LookUpNames3, state S261");
                this.Manager.Comment("reaching state \'S325\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp280;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp281;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp282;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid1\",\"SecurityPrincipalSid" +
                        "2\",\"DomainSid1\",\"DomainSid2\"},LookUpWKSTA,out _,out _)\'");
                temp282 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "SecurityPrincipalSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp280, out temp281);
                this.Manager.Checkpoint("MS-LSAT_R566");
                this.Manager.Checkpoint("MS-LSAT_R515");
                this.Manager.Comment("reaching state \'S357\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp280, "translateNames of LookUpSids2, state S357");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp281, "mapCount of LookUpSids2, state S357");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp282, "return of LookUpSids2, state S357");
                TestScenarioS2S387();
                goto label17;
            }
            throw new InvalidOperationException("never reached");
        label17:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S4InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S5");
        }
        
        private void TestScenarioS2S4InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S5");
        }
        #endregion
        
        #region Test Starting in S40
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S40() {
            this.Manager.BeginTest("TestScenarioS2S40");
            this.Manager.Comment("reaching state \'S40\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp284;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp284 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp284);
            this.Manager.Comment("reaching state \'S41\'");
            int temp301 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S40InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S40InitializeChecker1)));
            if ((temp301 == 0)) {
                this.Manager.Comment("reaching state \'S104\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp285;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp286;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp286 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp285);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S168\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp285, "openPolicyHandle2 of OpenPolicy2, state S168");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp286, "return of OpenPolicy2, state S168");
                this.Manager.Comment("reaching state \'S232\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp287;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp288;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp289;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalN" +
                        "ame\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},MSBSet,LookUpWKSTA,out" +
                        " _,out _)\'");
                temp289 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp287, out temp288);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S296\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp287, "translateSids of LookUpNames3, state S296");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp288, "mapCount of LookUpNames3, state S296");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp289, "return of LookUpNames3, state S296");
                this.Manager.Comment("reaching state \'S344\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp290;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp291;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp292;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid1\",\"DomainSid1\",\"DoesNot" +
                        "Exist\"},Invalid,out _,out _)\'");
                temp292 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp290, out temp291);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S376\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp290, "translateNames of LookUpSids2, state S376");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp291, "mapCount of LookUpSids2, state S376");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp292, "return of LookUpSids2, state S376");
                TestScenarioS2S385();
                goto label18;
            }
            if ((temp301 == 1)) {
                this.Manager.Comment("reaching state \'S105\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp293;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp294;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp294 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp293);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S169\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp293, "openPolicyHandle2 of OpenPolicy2, state S169");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp294, "return of OpenPolicy2, state S169");
                this.Manager.Comment("reaching state \'S233\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp295;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp296;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp297;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalN" +
                        "ame\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},MSBSet,LookUpWKSTA,out" +
                        " _,out _)\'");
                temp297 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp295, out temp296);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S297\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp295, "translateSids of LookUpNames3, state S297");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp296, "mapCount of LookUpNames3, state S297");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp297, "return of LookUpNames3, state S297");
                this.Manager.Comment("reaching state \'S345\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp298;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp299;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp300;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid1\",\"DomainSid1\",\"DoesNot" +
                        "Exist\"},Invalid,out _,out _)\'");
                temp300 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp298, out temp299);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S377\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp298, "translateNames of LookUpSids2, state S377");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp299, "mapCount of LookUpSids2, state S377");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp300, "return of LookUpSids2, state S377");
                TestScenarioS2S384();
                goto label18;
            }
            throw new InvalidOperationException("never reached");
        label18:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S40InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S41");
        }
        
        private void TestScenarioS2S40InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S41");
        }
        #endregion
        
        #region Test Starting in S42
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S42() {
            this.Manager.BeginTest("TestScenarioS2S42");
            this.Manager.Comment("reaching state \'S42\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp302;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp302 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp302);
            this.Manager.Comment("reaching state \'S43\'");
            int temp319 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S42InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S42InitializeChecker1)));
            if ((temp319 == 0)) {
                this.Manager.Comment("reaching state \'S106\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp303;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp304;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp304 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp303);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S170\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp303, "openPolicyHandle2 of OpenPolicy2, state S170");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp304, "return of OpenPolicy2, state S170");
                this.Manager.Comment("reaching state \'S234\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp305;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp306;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp307;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalN" +
                        "ame\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},MSBSet,Invalid,out _,o" +
                        "ut _)\'");
                temp307 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp305, out temp306);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Checkpoint("MS-LSAT_R286");
                this.Manager.Comment("reaching state \'S298\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp305, "translateSids of LookUpNames3, state S298");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp306, "mapCount of LookUpNames3, state S298");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp307, "return of LookUpNames3, state S298");
                this.Manager.Comment("reaching state \'S346\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp308;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp309;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp310;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid2\",\"InvalidSecurityPrinc" +
                        "ipalSid\",\"DomainSid2\",\"InvalidDomainSid\"},LookUpWKSTA,out _,out _)\'");
                temp310 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidSecurityPrincipalSid", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidDomainSid", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp308, out temp309);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S378\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp308, "translateNames of LookUpSids2, state S378");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp309, "mapCount of LookUpSids2, state S378");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp310, "return of LookUpSids2, state S378");
                TestScenarioS2S385();
                goto label19;
            }
            if ((temp319 == 1)) {
                this.Manager.Comment("reaching state \'S107\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp311;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp312;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp312 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp311);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S171\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp311, "openPolicyHandle2 of OpenPolicy2, state S171");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp312, "return of OpenPolicy2, state S171");
                this.Manager.Comment("reaching state \'S235\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp313;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp314;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp315;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalN" +
                        "ame\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},MSBSet,Invalid,out _,o" +
                        "ut _)\'");
                temp315 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp313, out temp314);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Checkpoint("MS-LSAT_R286");
                this.Manager.Comment("reaching state \'S299\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp313, "translateSids of LookUpNames3, state S299");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp314, "mapCount of LookUpNames3, state S299");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp315, "return of LookUpNames3, state S299");
                this.Manager.Comment("reaching state \'S347\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp316;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp317;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp318;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid2\",\"InvalidSecurityPrinc" +
                        "ipalSid\",\"DomainSid2\",\"InvalidDomainSid\"},LookUpWKSTA,out _,out _)\'");
                temp318 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidSecurityPrincipalSid", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidDomainSid", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp316, out temp317);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S379\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp316, "translateNames of LookUpSids2, state S379");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp317, "mapCount of LookUpSids2, state S379");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp318, "return of LookUpSids2, state S379");
                TestScenarioS2S384();
                goto label19;
            }
            throw new InvalidOperationException("never reached");
        label19:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S42InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S43");
        }
        
        private void TestScenarioS2S42InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S43");
        }
        #endregion
        
        #region Test Starting in S44
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S44() {
            this.Manager.BeginTest("TestScenarioS2S44");
            this.Manager.Comment("reaching state \'S44\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp320;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp320 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp320);
            this.Manager.Comment("reaching state \'S45\'");
            int temp337 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S44InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S44InitializeChecker1)));
            if ((temp337 == 0)) {
                this.Manager.Comment("reaching state \'S108\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp321;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp322;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp322 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp321);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S172\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp321, "openPolicyHandle2 of OpenPolicy2, state S172");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp322, "return of OpenPolicy2, state S172");
                this.Manager.Comment("reaching state \'S236\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp323;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp324;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp325;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQua" +
                        "lifiedName1\"},MSBSet,Invalid,out _,out _)\'");
                temp325 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp323, out temp324);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Checkpoint("MS-LSAT_R286");
                this.Manager.Comment("reaching state \'S300\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp323, "translateSids of LookUpNames3, state S300");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp324, "mapCount of LookUpNames3, state S300");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp325, "return of LookUpNames3, state S300");
                this.Manager.Comment("reaching state \'S348\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp326;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp327;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp328;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid2\",\"InvalidSecurityPrinc" +
                        "ipalSid\",\"DomainSid2\",\"InvalidDomainSid\"},Invalid,out _,out _)\'");
                temp328 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidSecurityPrincipalSid", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidDomainSid", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp326, out temp327);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S380\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp326, "translateNames of LookUpSids2, state S380");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp327, "mapCount of LookUpSids2, state S380");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp328, "return of LookUpSids2, state S380");
                TestScenarioS2S385();
                goto label20;
            }
            if ((temp337 == 1)) {
                this.Manager.Comment("reaching state \'S109\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp329;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp330;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp330 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp329);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S173\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp329, "openPolicyHandle2 of OpenPolicy2, state S173");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp330, "return of OpenPolicy2, state S173");
                this.Manager.Comment("reaching state \'S237\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp331;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp332;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp333;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQua" +
                        "lifiedName1\"},MSBSet,Invalid,out _,out _)\'");
                temp333 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp331, out temp332);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Checkpoint("MS-LSAT_R286");
                this.Manager.Comment("reaching state \'S301\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp331, "translateSids of LookUpNames3, state S301");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp332, "mapCount of LookUpNames3, state S301");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp333, "return of LookUpNames3, state S301");
                this.Manager.Comment("reaching state \'S349\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp334;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp335;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp336;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid2\",\"InvalidSecurityPrinc" +
                        "ipalSid\",\"DomainSid2\",\"InvalidDomainSid\"},Invalid,out _,out _)\'");
                temp336 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidSecurityPrincipalSid", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidDomainSid", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp334, out temp335);
                this.Manager.Checkpoint("MS-LSAT_R516");
                this.Manager.Comment("reaching state \'S381\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp334, "translateNames of LookUpSids2, state S381");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp335, "mapCount of LookUpSids2, state S381");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp336, "return of LookUpSids2, state S381");
                TestScenarioS2S384();
                goto label20;
            }
            throw new InvalidOperationException("never reached");
        label20:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S44InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S45");
        }
        
        private void TestScenarioS2S44InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S45");
        }
        #endregion
        
        #region Test Starting in S46
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S46() {
            this.Manager.BeginTest("TestScenarioS2S46");
            this.Manager.Comment("reaching state \'S46\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp338;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp338 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp338);
            this.Manager.Comment("reaching state \'S47\'");
            int temp355 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S46InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S46InitializeChecker1)));
            if ((temp355 == 0)) {
                this.Manager.Comment("reaching state \'S110\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp339;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp340;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp340 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp339);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S174\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp339, "openPolicyHandle2 of OpenPolicy2, state S174");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp340, "return of OpenPolicy2, state S174");
                this.Manager.Comment("reaching state \'S238\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp341;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp342;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp343;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQua" +
                        "lifiedName1\"},MSBNotSet,Invalid,out _,out _)\'");
                temp343 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp341, out temp342);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S302\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp341, "translateSids of LookUpNames3, state S302");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp342, "mapCount of LookUpNames3, state S302");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp343, "return of LookUpNames3, state S302");
                this.Manager.Comment("reaching state \'S350\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp344;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp345;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp346;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid1\",\"DomainSid1\",\"DoesNot" +
                        "Exist\"},LookUpWKSTA,out _,out _)\'");
                temp346 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp344, out temp345);
                this.Manager.Checkpoint("MS-LSAT_R514");
                this.Manager.Checkpoint("MS-LSAT_R563");
                this.Manager.Comment("reaching state \'S382\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:SomeNotMapped\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp344, "translateNames of LookUpSids2, state S382");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp345, "mapCount of LookUpSids2, state S382");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.SomeNotMapped, temp346, "return of LookUpSids2, state S382");
                TestScenarioS2S385();
                goto label21;
            }
            if ((temp355 == 1)) {
                this.Manager.Comment("reaching state \'S111\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp347;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp348;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp348 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp347);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S175\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp347, "openPolicyHandle2 of OpenPolicy2, state S175");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp348, "return of OpenPolicy2, state S175");
                this.Manager.Comment("reaching state \'S239\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp349;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp350;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp351;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQua" +
                        "lifiedName1\"},MSBNotSet,Invalid,out _,out _)\'");
                temp351 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp349, out temp350);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S303\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp349, "translateSids of LookUpNames3, state S303");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp350, "mapCount of LookUpNames3, state S303");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp351, "return of LookUpNames3, state S303");
                this.Manager.Comment("reaching state \'S351\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp352;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp353;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp354;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid1\",\"DomainSid1\",\"DoesNot" +
                        "Exist\"},LookUpWKSTA,out _,out _)\'");
                temp354 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp352, out temp353);
                this.Manager.Checkpoint("MS-LSAT_R514");
                this.Manager.Checkpoint("MS-LSAT_R563");
                this.Manager.Comment("reaching state \'S383\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:SomeNotMapped\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp352, "translateNames of LookUpSids2, state S383");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp353, "mapCount of LookUpSids2, state S383");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.SomeNotMapped, temp354, "return of LookUpSids2, state S383");
                TestScenarioS2S384();
                goto label21;
            }
            throw new InvalidOperationException("never reached");
        label21:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S46InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S47");
        }
        
        private void TestScenarioS2S46InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S47");
        }
        #endregion
        
        #region Test Starting in S48
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S48() {
            this.Manager.BeginTest("TestScenarioS2S48");
            this.Manager.Comment("reaching state \'S48\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp356;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp356 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp356);
            this.Manager.Comment("reaching state \'S49\'");
            int temp367 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S48InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S48InitializeChecker1)));
            if ((temp367 == 0)) {
                this.Manager.Comment("reaching state \'S112\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp357;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp358;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp358 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp357);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S176\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp357, "openPolicyHandle2 of OpenPolicy2, state S176");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp358, "return of OpenPolicy2, state S176");
                this.Manager.Comment("reaching state \'S240\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp359;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp360;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp361;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"U" +
                        "nQualifiedName1\",\"IsolatedName1\"},MSBSet,Invalid,out _,out _)\'");
                temp361 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp359, out temp360);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Checkpoint("MS-LSAT_R286");
                this.Manager.Comment("reaching state \'S304\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp359, "translateSids of LookUpNames3, state S304");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp360, "mapCount of LookUpNames3, state S304");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp361, "return of LookUpNames3, state S304");
                TestScenarioS2S321();
                goto label22;
            }
            if ((temp367 == 1)) {
                this.Manager.Comment("reaching state \'S113\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp362;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp363;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp363 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp362);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S177\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp362, "openPolicyHandle2 of OpenPolicy2, state S177");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp363, "return of OpenPolicy2, state S177");
                this.Manager.Comment("reaching state \'S241\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp364;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp365;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp366;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"U" +
                        "nQualifiedName1\",\"IsolatedName1\"},MSBSet,Invalid,out _,out _)\'");
                temp366 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp364, out temp365);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Checkpoint("MS-LSAT_R286");
                this.Manager.Comment("reaching state \'S305\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp364, "translateSids of LookUpNames3, state S305");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp365, "mapCount of LookUpNames3, state S305");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp366, "return of LookUpNames3, state S305");
                TestScenarioS2S320();
                goto label22;
            }
            throw new InvalidOperationException("never reached");
        label22:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S48InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S49");
        }
        
        private void TestScenarioS2S48InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S49");
        }
        #endregion
        
        #region Test Starting in S50
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S50() {
            this.Manager.BeginTest("TestScenarioS2S50");
            this.Manager.Comment("reaching state \'S50\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp368;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp368 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp368);
            this.Manager.Comment("reaching state \'S51\'");
            int temp379 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S50InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S50InitializeChecker1)));
            if ((temp379 == 0)) {
                this.Manager.Comment("reaching state \'S114\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp369;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp370;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp370 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp369);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S178\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp369, "openPolicyHandle2 of OpenPolicy2, state S178");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp370, "return of OpenPolicy2, state S178");
                this.Manager.Comment("reaching state \'S242\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp371;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp372;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp373;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"U" +
                        "nQualifiedName1\",\"IsolatedName1\"},MSBNotSet,Invalid,out _,out _)\'");
                temp373 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp371, out temp372);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S306\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp371, "translateSids of LookUpNames3, state S306");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp372, "mapCount of LookUpNames3, state S306");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp373, "return of LookUpNames3, state S306");
                TestScenarioS2S321();
                goto label23;
            }
            if ((temp379 == 1)) {
                this.Manager.Comment("reaching state \'S115\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp374;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp375;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp375 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp374);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S179\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp374, "openPolicyHandle2 of OpenPolicy2, state S179");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp375, "return of OpenPolicy2, state S179");
                this.Manager.Comment("reaching state \'S243\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp376;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp377;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp378;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"U" +
                        "nQualifiedName1\",\"IsolatedName1\"},MSBNotSet,Invalid,out _,out _)\'");
                temp378 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp376, out temp377);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S307\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp376, "translateSids of LookUpNames3, state S307");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp377, "mapCount of LookUpNames3, state S307");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp378, "return of LookUpNames3, state S307");
                TestScenarioS2S320();
                goto label23;
            }
            throw new InvalidOperationException("never reached");
        label23:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S50InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S51");
        }
        
        private void TestScenarioS2S50InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S51");
        }
        #endregion
        
        #region Test Starting in S52
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S52() {
            this.Manager.BeginTest("TestScenarioS2S52");
            this.Manager.Comment("reaching state \'S52\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp380;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp380 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp380);
            this.Manager.Comment("reaching state \'S53\'");
            int temp391 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S52InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S52InitializeChecker1)));
            if ((temp391 == 0)) {
                this.Manager.Comment("reaching state \'S116\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp381;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp382;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp382 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp381);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S180\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp381, "openPolicyHandle2 of OpenPolicy2, state S180");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp382, "return of OpenPolicy2, state S180");
                this.Manager.Comment("reaching state \'S244\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp383;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp384;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp385;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"DoesNotExist\"},MSBSet,Invalid,out _,out _)\'" +
                        "");
                temp385 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp383, out temp384);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Checkpoint("MS-LSAT_R286");
                this.Manager.Comment("reaching state \'S308\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp383, "translateSids of LookUpNames3, state S308");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp384, "mapCount of LookUpNames3, state S308");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp385, "return of LookUpNames3, state S308");
                TestScenarioS2S321();
                goto label24;
            }
            if ((temp391 == 1)) {
                this.Manager.Comment("reaching state \'S117\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp386;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp387;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp387 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp386);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S181\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp386, "openPolicyHandle2 of OpenPolicy2, state S181");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp387, "return of OpenPolicy2, state S181");
                this.Manager.Comment("reaching state \'S245\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp388;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp389;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp390;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"DoesNotExist\"},MSBSet,Invalid,out _,out _)\'" +
                        "");
                temp390 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp388, out temp389);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Checkpoint("MS-LSAT_R286");
                this.Manager.Comment("reaching state \'S309\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp388, "translateSids of LookUpNames3, state S309");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp389, "mapCount of LookUpNames3, state S309");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp390, "return of LookUpNames3, state S309");
                TestScenarioS2S320();
                goto label24;
            }
            throw new InvalidOperationException("never reached");
        label24:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S52InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S53");
        }
        
        private void TestScenarioS2S52InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S53");
        }
        #endregion
        
        #region Test Starting in S54
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S54() {
            this.Manager.BeginTest("TestScenarioS2S54");
            this.Manager.Comment("reaching state \'S54\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp392;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp392 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp392);
            this.Manager.Comment("reaching state \'S55\'");
            int temp403 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S54InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S54InitializeChecker1)));
            if ((temp403 == 0)) {
                this.Manager.Comment("reaching state \'S118\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp393;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp394;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp394 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp393);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S182\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp393, "openPolicyHandle2 of OpenPolicy2, state S182");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp394, "return of OpenPolicy2, state S182");
                this.Manager.Comment("reaching state \'S246\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp395;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp396;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp397;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"DoesNotExist\"},MSBNotSet,Invalid,out _,out " +
                        "_)\'");
                temp397 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp395, out temp396);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S310\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp395, "translateSids of LookUpNames3, state S310");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp396, "mapCount of LookUpNames3, state S310");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp397, "return of LookUpNames3, state S310");
                TestScenarioS2S321();
                goto label25;
            }
            if ((temp403 == 1)) {
                this.Manager.Comment("reaching state \'S119\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp398;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp399;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp399 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp398);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S183\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp398, "openPolicyHandle2 of OpenPolicy2, state S183");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp399, "return of OpenPolicy2, state S183");
                this.Manager.Comment("reaching state \'S247\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp400;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp401;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp402;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"DoesNotExist\"},MSBNotSet,Invalid,out _,out " +
                        "_)\'");
                temp402 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp400, out temp401);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S311\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp400, "translateSids of LookUpNames3, state S311");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp401, "mapCount of LookUpNames3, state S311");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp402, "return of LookUpNames3, state S311");
                TestScenarioS2S320();
                goto label25;
            }
            throw new InvalidOperationException("never reached");
        label25:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S54InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S55");
        }
        
        private void TestScenarioS2S54InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S55");
        }
        #endregion
        
        #region Test Starting in S56
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S56() {
            this.Manager.BeginTest("TestScenarioS2S56");
            this.Manager.Comment("reaching state \'S56\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp404;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp404 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp404);
            this.Manager.Comment("reaching state \'S57\'");
            int temp415 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S56InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S56InitializeChecker1)));
            if ((temp415 == 0)) {
                this.Manager.Comment("reaching state \'S120\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp405;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp406;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp406 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp405);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S184\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp405, "openPolicyHandle2 of OpenPolicy2, state S184");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp406, "return of OpenPolicy2, state S184");
                this.Manager.Comment("reaching state \'S248\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp407;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp408;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp409;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQua" +
                        "lifiedName1\"},MSBSet,LookUpWKSTA,out _,out _)\'");
                temp409 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp407, out temp408);
                this.Manager.Checkpoint("MS-LSAT_R279");
                this.Manager.Checkpoint("MS-LSAT_R313");
                this.Manager.Comment("reaching state \'S312\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:SomeNotMapped\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp407, "translateSids of LookUpNames3, state S312");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp408, "mapCount of LookUpNames3, state S312");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.SomeNotMapped, temp409, "return of LookUpNames3, state S312");
                TestScenarioS2S321();
                goto label26;
            }
            if ((temp415 == 1)) {
                this.Manager.Comment("reaching state \'S121\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp410;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp411;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp411 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp410);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S185\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp410, "openPolicyHandle2 of OpenPolicy2, state S185");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp411, "return of OpenPolicy2, state S185");
                this.Manager.Comment("reaching state \'S249\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp412;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp413;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp414;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQua" +
                        "lifiedName1\"},MSBSet,LookUpWKSTA,out _,out _)\'");
                temp414 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp412, out temp413);
                this.Manager.Checkpoint("MS-LSAT_R279");
                this.Manager.Checkpoint("MS-LSAT_R313");
                this.Manager.Comment("reaching state \'S313\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:SomeNotMapped\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp412, "translateSids of LookUpNames3, state S313");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp413, "mapCount of LookUpNames3, state S313");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.SomeNotMapped, temp414, "return of LookUpNames3, state S313");
                TestScenarioS2S320();
                goto label26;
            }
            throw new InvalidOperationException("never reached");
        label26:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S56InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S57");
        }
        
        private void TestScenarioS2S56InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S57");
        }
        #endregion
        
        #region Test Starting in S58
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S58() {
            this.Manager.BeginTest("TestScenarioS2S58");
            this.Manager.Comment("reaching state \'S58\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp416;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp416 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp416);
            this.Manager.Comment("reaching state \'S59\'");
            int temp427 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S58InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S58InitializeChecker1)));
            if ((temp427 == 0)) {
                this.Manager.Comment("reaching state \'S122\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp417;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp418;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp418 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp417);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S186\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp417, "openPolicyHandle2 of OpenPolicy2, state S186");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp418, "return of OpenPolicy2, state S186");
                this.Manager.Comment("reaching state \'S250\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp419;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp420;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp421;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQua" +
                        "lifiedName1\"},MSBNotSet,LookUpWKSTA,out _,out _)\'");
                temp421 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp419, out temp420);
                this.Manager.Checkpoint("MS-LSAT_R279");
                this.Manager.Checkpoint("MS-LSAT_R313");
                this.Manager.Comment("reaching state \'S314\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:SomeNotMapped\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp419, "translateSids of LookUpNames3, state S314");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp420, "mapCount of LookUpNames3, state S314");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.SomeNotMapped, temp421, "return of LookUpNames3, state S314");
                TestScenarioS2S321();
                goto label27;
            }
            if ((temp427 == 1)) {
                this.Manager.Comment("reaching state \'S123\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp422;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp423;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp423 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp422);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S187\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp422, "openPolicyHandle2 of OpenPolicy2, state S187");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp423, "return of OpenPolicy2, state S187");
                this.Manager.Comment("reaching state \'S251\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp424;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp425;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp426;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQua" +
                        "lifiedName1\"},MSBNotSet,LookUpWKSTA,out _,out _)\'");
                temp426 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp424, out temp425);
                this.Manager.Checkpoint("MS-LSAT_R279");
                this.Manager.Checkpoint("MS-LSAT_R313");
                this.Manager.Comment("reaching state \'S315\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:SomeNotMapped\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp424, "translateSids of LookUpNames3, state S315");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp425, "mapCount of LookUpNames3, state S315");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.SomeNotMapped, temp426, "return of LookUpNames3, state S315");
                TestScenarioS2S320();
                goto label27;
            }
            throw new InvalidOperationException("never reached");
        label27:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S58InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S59");
        }
        
        private void TestScenarioS2S58InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S59");
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S6() {
            this.Manager.BeginTest("TestScenarioS2S6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp428;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp428 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp428);
            this.Manager.Comment("reaching state \'S7\'");
            int temp445 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S6InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S6InitializeChecker1)));
            if ((temp445 == 0)) {
                this.Manager.Comment("reaching state \'S70\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp429;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp430;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp430 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp429);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S134\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp429, "openPolicyHandle2 of OpenPolicy2, state S134");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp430, "return of OpenPolicy2, state S134");
                this.Manager.Comment("reaching state \'S198\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp431;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp432;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp433;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"DoesNotExist\"},MSBNotSet,LookUpWKSTA,out _," +
                        "out _)\'");
                temp433 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp431, out temp432);
                this.Manager.Checkpoint("MS-LSAT_R280");
                this.Manager.Checkpoint("MS-LSAT_R317");
                this.Manager.Comment("reaching state \'S262\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp431, "translateSids of LookUpNames3, state S262");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp432, "mapCount of LookUpNames3, state S262");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp433, "return of LookUpNames3, state S262");
                this.Manager.Comment("reaching state \'S326\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp434;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp435;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp436;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"DoesNotExist\"},LookUpWKSTA,out _,out _)\'");
                temp436 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp434, out temp435);
                this.Manager.Checkpoint("MS-LSAT_R566");
                this.Manager.Checkpoint("MS-LSAT_R515");
                this.Manager.Comment("reaching state \'S358\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp434, "translateNames of LookUpSids2, state S358");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp435, "mapCount of LookUpSids2, state S358");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp436, "return of LookUpSids2, state S358");
                TestScenarioS2S386();
                goto label28;
            }
            if ((temp445 == 1)) {
                this.Manager.Comment("reaching state \'S71\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp437;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp438;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp438 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp437);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S135\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp437, "openPolicyHandle2 of OpenPolicy2, state S135");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp438, "return of OpenPolicy2, state S135");
                this.Manager.Comment("reaching state \'S199\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp439;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp440;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp441;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"DoesNotExist\"},MSBSet,LookUpWKSTA,out _,out" +
                        " _)\'");
                temp441 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp439, out temp440);
                this.Manager.Checkpoint("MS-LSAT_R280");
                this.Manager.Checkpoint("MS-LSAT_R317");
                this.Manager.Comment("reaching state \'S263\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp439, "translateSids of LookUpNames3, state S263");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp440, "mapCount of LookUpNames3, state S263");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp441, "return of LookUpNames3, state S263");
                this.Manager.Comment("reaching state \'S327\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp442;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp443;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp444;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"DoesNotExist\"},LookUpWKSTA,out _,out _)\'");
                temp444 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp442, out temp443);
                this.Manager.Checkpoint("MS-LSAT_R566");
                this.Manager.Checkpoint("MS-LSAT_R515");
                this.Manager.Comment("reaching state \'S359\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp442, "translateNames of LookUpSids2, state S359");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp443, "mapCount of LookUpSids2, state S359");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp444, "return of LookUpSids2, state S359");
                TestScenarioS2S387();
                goto label28;
            }
            throw new InvalidOperationException("never reached");
        label28:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S6InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S7");
        }
        
        private void TestScenarioS2S6InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S7");
        }
        #endregion
        
        #region Test Starting in S60
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S60() {
            this.Manager.BeginTest("TestScenarioS2S60");
            this.Manager.Comment("reaching state \'S60\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp446;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp446 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp446);
            this.Manager.Comment("reaching state \'S61\'");
            int temp457 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S60InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S60InitializeChecker1)));
            if ((temp457 == 0)) {
                this.Manager.Comment("reaching state \'S124\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp447;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp448;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp448 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp447);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S188\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp447, "openPolicyHandle2 of OpenPolicy2, state S188");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp448, "return of OpenPolicy2, state S188");
                this.Manager.Comment("reaching state \'S252\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp449;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp450;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp451;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"DoesNotExist\"},MSBNotSet,LookUpWKSTA,out _," +
                        "out _)\'");
                temp451 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp449, out temp450);
                this.Manager.Checkpoint("MS-LSAT_R282");
                this.Manager.Checkpoint("MS-LSAT_R314");
                this.Manager.Comment("reaching state \'S316\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:NoneMapped\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp449, "translateSids of LookUpNames3, state S316");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp450, "mapCount of LookUpNames3, state S316");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.NoneMapped, temp451, "return of LookUpNames3, state S316");
                TestScenarioS2S321();
                goto label29;
            }
            if ((temp457 == 1)) {
                this.Manager.Comment("reaching state \'S125\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp452;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp453;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,2048,out _)\'");
                temp453 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp452);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S189\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp452, "openPolicyHandle2 of OpenPolicy2, state S189");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp453, "return of OpenPolicy2, state S189");
                this.Manager.Comment("reaching state \'S253\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp454;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp455;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp456;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"DoesNotExist\"},MSBNotSet,LookUpWKSTA,out _," +
                        "out _)\'");
                temp456 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp454, out temp455);
                this.Manager.Checkpoint("MS-LSAT_R282");
                this.Manager.Checkpoint("MS-LSAT_R314");
                this.Manager.Comment("reaching state \'S317\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:NoneMapped\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp454, "translateSids of LookUpNames3, state S317");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp455, "mapCount of LookUpNames3, state S317");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.NoneMapped, temp456, "return of LookUpNames3, state S317");
                TestScenarioS2S320();
                goto label29;
            }
            throw new InvalidOperationException("never reached");
        label29:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S60InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S61");
        }
        
        private void TestScenarioS2S60InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S61");
        }
        #endregion
        
        #region Test Starting in S62
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S62() {
            this.Manager.BeginTest("TestScenarioS2S62");
            this.Manager.Comment("reaching state \'S62\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp458;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp458 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp458);
            this.Manager.Comment("reaching state \'S63\'");
            int temp469 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S62InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S62InitializeChecker1)));
            if ((temp469 == 0)) {
                this.Manager.Comment("reaching state \'S126\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp459;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp460;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp460 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp459);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S190\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp459, "openPolicyHandle2 of OpenPolicy2, state S190");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp460, "return of OpenPolicy2, state S190");
                this.Manager.Comment("reaching state \'S254\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp461;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp462;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp463;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"DoesNotExist\"},MSBNotSet,Invalid,out _,out " +
                        "_)\'");
                temp463 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp461, out temp462);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S318\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp461, "translateSids of LookUpNames3, state S318");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp462, "mapCount of LookUpNames3, state S318");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp463, "return of LookUpNames3, state S318");
                TestScenarioS2S323();
                goto label30;
            }
            if ((temp469 == 1)) {
                this.Manager.Comment("reaching state \'S127\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp464;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp465;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp465 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp464);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S191\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp464, "openPolicyHandle2 of OpenPolicy2, state S191");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp465, "return of OpenPolicy2, state S191");
                this.Manager.Comment("reaching state \'S255\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp466;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp467;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp468;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"DoesNotExist\"},MSBNotSet,Invalid,out _,out " +
                        "_)\'");
                temp468 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp466, out temp467);
                this.Manager.Checkpoint("MS-LSAT_R281");
                this.Manager.Comment("reaching state \'S319\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp466, "translateSids of LookUpNames3, state S319");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp467, "mapCount of LookUpNames3, state S319");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp468, "return of LookUpNames3, state S319");
                TestScenarioS2S322();
                goto label30;
            }
            throw new InvalidOperationException("never reached");
        label30:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S62InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S63");
        }
        
        private void TestScenarioS2S62InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S63");
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS2S8() {
            this.Manager.BeginTest("TestScenarioS2S8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp470;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp470 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp470);
            this.Manager.Comment("reaching state \'S9\'");
            int temp487 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S8InitializeChecker)), new ExpectedReturn(TestScenarioS2.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS2S8InitializeChecker1)));
            if ((temp487 == 0)) {
                this.Manager.Comment("reaching state \'S72\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp471;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp472;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp472 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp471);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S136\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp471, "openPolicyHandle2 of OpenPolicy2, state S136");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp472, "return of OpenPolicy2, state S136");
                this.Manager.Comment("reaching state \'S200\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp473;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp474;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp475;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"DoesNotExist\"},MSBSet,LookUpWKSTA,out _,out" +
                        " _)\'");
                temp475 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp473, out temp474);
                this.Manager.Checkpoint("MS-LSAT_R280");
                this.Manager.Checkpoint("MS-LSAT_R317");
                this.Manager.Comment("reaching state \'S264\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp473, "translateSids of LookUpNames3, state S264");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp474, "mapCount of LookUpNames3, state S264");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp475, "return of LookUpNames3, state S264");
                this.Manager.Comment("reaching state \'S328\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp476;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp477;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp478;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid1\",\"DomainSid1\",\"DoesNot" +
                        "Exist\"},LookUpWKSTA,out _,out _)\'");
                temp478 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp476, out temp477);
                this.Manager.Checkpoint("MS-LSAT_R566");
                this.Manager.Checkpoint("MS-LSAT_R515");
                this.Manager.Comment("reaching state \'S360\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp476, "translateNames of LookUpSids2, state S360");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp477, "mapCount of LookUpSids2, state S360");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp478, "return of LookUpSids2, state S360");
                TestScenarioS2S386();
                goto label31;
            }
            if ((temp487 == 1)) {
                this.Manager.Comment("reaching state \'S73\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp479;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp480;
                this.Manager.Comment("executing step \'call OpenPolicy2(Null,1,out _)\'");
                temp480 = this.ILsatAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp479);
                this.Manager.Checkpoint("MS-LSAT_R166");
                this.Manager.Checkpoint("MS-LSAT_R161");
                this.Manager.Comment("reaching state \'S137\'");
                this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp479, "openPolicyHandle2 of OpenPolicy2, state S137");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp480, "return of OpenPolicy2, state S137");
                this.Manager.Comment("reaching state \'S201\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp481;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp482;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp483;
                this.Manager.Comment("executing step \'call LookUpNames3(1,{\"DoesNotExist\"},MSBNotSet,LookUpWKSTA,out _," +
                        "out _)\'");
                temp483 = this.ILsatAdapterInstance.LookUpNames3(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpOption)(1)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp481, out temp482);
                this.Manager.Checkpoint("MS-LSAT_R280");
                this.Manager.Checkpoint("MS-LSAT_R317");
                this.Manager.Comment("reaching state \'S265\'");
                this.Manager.Comment("checking step \'return LookUpNames3/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp481, "translateSids of LookUpNames3, state S265");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp482, "mapCount of LookUpNames3, state S265");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp483, "return of LookUpNames3, state S265");
                this.Manager.Comment("reaching state \'S329\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames temp484;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp485;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp486;
                this.Manager.Comment("executing step \'call LookUpSids2(1,{\"SecurityPrincipalSid1\",\"DomainSid1\",\"DoesNot" +
                        "Exist\"},LookUpWKSTA,out _,out _)\'");
                temp486 = this.ILsatAdapterInstance.LookUpSids2(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "SecurityPrincipalSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DomainSid1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp484, out temp485);
                this.Manager.Checkpoint("MS-LSAT_R566");
                this.Manager.Checkpoint("MS-LSAT_R515");
                this.Manager.Comment("reaching state \'S361\'");
                this.Manager.Comment("checking step \'return LookUpSids2/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranstlatedNames)(1)), temp484, "translateNames of LookUpSids2, state S361");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp485, "mapCount of LookUpSids2, state S361");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp486, "return of LookUpSids2, state S361");
                TestScenarioS2S387();
                goto label31;
            }
            throw new InvalidOperationException("never reached");
        label31:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS2S8InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S9");
        }
        
        private void TestScenarioS2S8InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S9");
        }
        #endregion
    }
}
