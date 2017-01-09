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
    public partial class TestScenarioS4 : PtfTestClassBase {
        
        public TestScenarioS4() {
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
        public void LSAT_TestScenarioS4S0() {
            this.Manager.BeginTest("TestScenarioS4S0");
            this.Manager.Comment("reaching state \'S0\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp0;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp0 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp0);
            this.Manager.Comment("reaching state \'S1\'");
            int temp15 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S0InitializeChecker)), new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S0InitializeChecker1)));
            if ((temp15 == 0)) {
                this.Manager.Comment("reaching state \'S32\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp1;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp2;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,2048,out _)\'");
                temp2 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp1);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S64\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp1, "openPolicyHandle of OpenPolicy, state S64");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp2, "return of OpenPolicy, state S64");
                this.Manager.Comment("reaching state \'S96\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp3;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp4;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp5;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"DoesNotExist\"},LookUpWKSTA,out _,out _)\'");
                temp5 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp3, out temp4);
                this.Manager.Checkpoint("MS-LSAT_R400");
                this.Manager.Checkpoint("MS-LSAT_R432");
                this.Manager.Comment("reaching state \'S128\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:NoneMapped\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp3, "translateSids of LookUpNames, state S128");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp4, "mapCount of LookUpNames, state S128");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.NoneMapped, temp5, "return of LookUpNames, state S128");
                TestScenarioS4S160();
                goto label0;
            }
            if ((temp15 == 1)) {
                this.Manager.Comment("reaching state \'S33\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp8;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp9;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,2048,out _)\'");
                temp9 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp8);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S65\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp8, "openPolicyHandle of OpenPolicy, state S65");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp9, "return of OpenPolicy, state S65");
                this.Manager.Comment("reaching state \'S97\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp10;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp11;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp12;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"DoesNotExist\"},LookUpWKSTA,out _,out _)\'");
                temp12 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp10, out temp11);
                this.Manager.Checkpoint("MS-LSAT_R400");
                this.Manager.Checkpoint("MS-LSAT_R432");
                this.Manager.Comment("reaching state \'S129\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:NoneMapped\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp10, "translateSids of LookUpNames, state S129");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp11, "mapCount of LookUpNames, state S129");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.NoneMapped, temp12, "return of LookUpNames, state S129");
                TestScenarioS4S161();
                goto label0;
            }
            throw new InvalidOperationException("never reached");
        label0:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S0InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S1");
        }
        
        private void TestScenarioS4S160() {
            this.Manager.Comment("reaching state \'S160\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp6;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp7;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp7 = this.ILsatAdapterInstance.Close(1, out temp6);
            this.Manager.Checkpoint("MS-LSAT_R181");
            this.Manager.Checkpoint("MS-LSAT_R183");
            this.Manager.AddReturn(CloseInfo, null, temp6, temp7);
            TestScenarioS4S164();
        }
        
        private void TestScenarioS4S164() {
            this.Manager.Comment("reaching state \'S164\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.CloseInfo, null, new CloseDelegate1(this.TestScenarioS4S0CloseChecker)));
            this.Manager.Comment("reaching state \'S166\'");
        }
        
        private void TestScenarioS4S0CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S164");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, @return, "return of Close, state S164");
        }
        
        private void TestScenarioS4S0InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S1");
        }
        
        private void TestScenarioS4S161() {
            this.Manager.Comment("reaching state \'S161\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp13;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp14;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp14 = this.ILsatAdapterInstance.Close(1, out temp13);
            this.Manager.Checkpoint("MS-LSAT_R181");
            this.Manager.Checkpoint("MS-LSAT_R183");
            this.Manager.AddReturn(CloseInfo, null, temp13, temp14);
            TestScenarioS4S165();
        }
        
        private void TestScenarioS4S165() {
            this.Manager.Comment("reaching state \'S165\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.CloseInfo, null, new CloseDelegate1(this.TestScenarioS4S0CloseChecker1)));
            this.Manager.Comment("reaching state \'S167\'");
        }
        
        private void TestScenarioS4S0CloseChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S165");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, @return, "return of Close, state S165");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS4S10() {
            this.Manager.BeginTest("TestScenarioS4S10");
            this.Manager.Comment("reaching state \'S10\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp16;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp16 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp16);
            this.Manager.Comment("reaching state \'S11\'");
            int temp31 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S10InitializeChecker)), new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S10InitializeChecker1)));
            if ((temp31 == 0)) {
                this.Manager.Comment("reaching state \'S42\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp17;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp18;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
                temp18 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp17);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S74\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp17, "openPolicyHandle of OpenPolicy, state S74");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp18, "return of OpenPolicy, state S74");
                this.Manager.Comment("reaching state \'S106\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp19;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp20;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp21;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"DoesNotExist\"},Invalid,out _,out _)\'");
                temp21 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp19, out temp20);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S138\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp19, "translateSids of LookUpNames, state S138");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp20, "mapCount of LookUpNames, state S138");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp21, "return of LookUpNames, state S138");
                TestScenarioS4S162();
                goto label1;
            }
            if ((temp31 == 1)) {
                this.Manager.Comment("reaching state \'S43\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp24;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp25;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
                temp25 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp24);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S75\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp24, "openPolicyHandle of OpenPolicy, state S75");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp25, "return of OpenPolicy, state S75");
                this.Manager.Comment("reaching state \'S107\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp26;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp27;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp28;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"DoesNotExist\"},Invalid,out _,out _)\'");
                temp28 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp26, out temp27);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S139\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp26, "translateSids of LookUpNames, state S139");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp27, "mapCount of LookUpNames, state S139");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp28, "return of LookUpNames, state S139");
                TestScenarioS4S163();
                goto label1;
            }
            throw new InvalidOperationException("never reached");
        label1:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S10InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S11");
        }
        
        private void TestScenarioS4S162() {
            this.Manager.Comment("reaching state \'S162\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp22;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp23;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp23 = this.ILsatAdapterInstance.Close(1, out temp22);
            this.Manager.Checkpoint("MS-LSAT_R181");
            this.Manager.Checkpoint("MS-LSAT_R183");
            this.Manager.AddReturn(CloseInfo, null, temp22, temp23);
            TestScenarioS4S165();
        }
        
        private void TestScenarioS4S10InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S11");
        }
        
        private void TestScenarioS4S163() {
            this.Manager.Comment("reaching state \'S163\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp29;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp30;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp30 = this.ILsatAdapterInstance.Close(1, out temp29);
            this.Manager.Checkpoint("MS-LSAT_R181");
            this.Manager.Checkpoint("MS-LSAT_R183");
            this.Manager.AddReturn(CloseInfo, null, temp29, temp30);
            TestScenarioS4S164();
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS4S12() {
            this.Manager.BeginTest("TestScenarioS4S12");
            this.Manager.Comment("reaching state \'S12\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp32;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp32 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp32);
            this.Manager.Comment("reaching state \'S13\'");
            int temp43 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S12InitializeChecker)), new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S12InitializeChecker1)));
            if ((temp43 == 0)) {
                this.Manager.Comment("reaching state \'S44\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp33;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp34;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
                temp34 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp33);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S76\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp33, "openPolicyHandle of OpenPolicy, state S76");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp34, "return of OpenPolicy, state S76");
                this.Manager.Comment("reaching state \'S108\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp35;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp36;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp37;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalNa" +
                        "me\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},Invalid,out _,out _)\'");
                temp37 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp35, out temp36);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S140\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp35, "translateSids of LookUpNames, state S140");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp36, "mapCount of LookUpNames, state S140");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp37, "return of LookUpNames, state S140");
                TestScenarioS4S162();
                goto label2;
            }
            if ((temp43 == 1)) {
                this.Manager.Comment("reaching state \'S45\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp38;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp39;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
                temp39 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp38);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S77\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp38, "openPolicyHandle of OpenPolicy, state S77");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp39, "return of OpenPolicy, state S77");
                this.Manager.Comment("reaching state \'S109\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp40;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp41;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp42;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalNa" +
                        "me\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},Invalid,out _,out _)\'");
                temp42 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp40, out temp41);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S141\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp40, "translateSids of LookUpNames, state S141");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp41, "mapCount of LookUpNames, state S141");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp42, "return of LookUpNames, state S141");
                TestScenarioS4S163();
                goto label2;
            }
            throw new InvalidOperationException("never reached");
        label2:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S12InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S13");
        }
        
        private void TestScenarioS4S12InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S13");
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS4S14() {
            this.Manager.BeginTest("TestScenarioS4S14");
            this.Manager.Comment("reaching state \'S14\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp44;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp44 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp44);
            this.Manager.Comment("reaching state \'S15\'");
            int temp55 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S14InitializeChecker)), new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S14InitializeChecker1)));
            if ((temp55 == 0)) {
                this.Manager.Comment("reaching state \'S46\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp45;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp46;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
                temp46 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp45);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S78\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp45, "openPolicyHandle of OpenPolicy, state S78");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp46, "return of OpenPolicy, state S78");
                this.Manager.Comment("reaching state \'S110\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp47;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp48;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp49;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalNa" +
                        "me\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},LookUpWKSTA,out _,out _" +
                        ")\'");
                temp49 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp47, out temp48);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S142\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp47, "translateSids of LookUpNames, state S142");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp48, "mapCount of LookUpNames, state S142");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp49, "return of LookUpNames, state S142");
                TestScenarioS4S162();
                goto label3;
            }
            if ((temp55 == 1)) {
                this.Manager.Comment("reaching state \'S47\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp50;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp51;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
                temp51 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp50);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S79\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp50, "openPolicyHandle of OpenPolicy, state S79");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp51, "return of OpenPolicy, state S79");
                this.Manager.Comment("reaching state \'S111\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp52;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp53;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp54;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalNa" +
                        "me\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},LookUpWKSTA,out _,out _" +
                        ")\'");
                temp54 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp52, out temp53);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S143\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp52, "translateSids of LookUpNames, state S143");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp53, "mapCount of LookUpNames, state S143");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp54, "return of LookUpNames, state S143");
                TestScenarioS4S163();
                goto label3;
            }
            throw new InvalidOperationException("never reached");
        label3:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S14InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S15");
        }
        
        private void TestScenarioS4S14InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S15");
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS4S16() {
            this.Manager.BeginTest("TestScenarioS4S16");
            this.Manager.Comment("reaching state \'S16\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp56;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp56 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp56);
            this.Manager.Comment("reaching state \'S17\'");
            int temp67 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S16InitializeChecker)), new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S16InitializeChecker1)));
            if ((temp67 == 0)) {
                this.Manager.Comment("reaching state \'S48\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp57;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp58;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,2048,out _)\'");
                temp58 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp57);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S80\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp57, "openPolicyHandle of OpenPolicy, state S80");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp58, "return of OpenPolicy, state S80");
                this.Manager.Comment("reaching state \'S112\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp59;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp60;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp61;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"Un" +
                        "QualifiedName1\",\"IsolatedName1\"},LookUpWKSTA,out _,out _)\'");
                temp61 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp59, out temp60);
                this.Manager.Checkpoint("MS-LSAT_R396");
                this.Manager.Checkpoint("MS-LSAT_R430");
                this.Manager.Comment("reaching state \'S144\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Valid,out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(0)), temp59, "translateSids of LookUpNames, state S144");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(0)), temp60, "mapCount of LookUpNames, state S144");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp61, "return of LookUpNames, state S144");
                TestScenarioS4S161();
                goto label4;
            }
            if ((temp67 == 1)) {
                this.Manager.Comment("reaching state \'S49\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp62;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp63;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,2048,out _)\'");
                temp63 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp62);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S81\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp62, "openPolicyHandle of OpenPolicy, state S81");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp63, "return of OpenPolicy, state S81");
                this.Manager.Comment("reaching state \'S113\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp64;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp65;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp66;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"Un" +
                        "QualifiedName1\",\"IsolatedName1\"},LookUpWKSTA,out _,out _)\'");
                temp66 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp64, out temp65);
                this.Manager.Checkpoint("MS-LSAT_R396");
                this.Manager.Checkpoint("MS-LSAT_R430");
                this.Manager.Comment("reaching state \'S145\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Valid,out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(0)), temp64, "translateSids of LookUpNames, state S145");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(0)), temp65, "mapCount of LookUpNames, state S145");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp66, "return of LookUpNames, state S145");
                TestScenarioS4S160();
                goto label4;
            }
            throw new InvalidOperationException("never reached");
        label4:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S16InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S17");
        }
        
        private void TestScenarioS4S16InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S17");
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS4S18() {
            this.Manager.BeginTest("TestScenarioS4S18");
            this.Manager.Comment("reaching state \'S18\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp68;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp68 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp68);
            this.Manager.Comment("reaching state \'S19\'");
            int temp79 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S18InitializeChecker)), new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S18InitializeChecker1)));
            if ((temp79 == 0)) {
                this.Manager.Comment("reaching state \'S50\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp69;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp70;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,2048,out _)\'");
                temp70 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp69);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S82\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp69, "openPolicyHandle of OpenPolicy, state S82");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp70, "return of OpenPolicy, state S82");
                this.Manager.Comment("reaching state \'S114\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp71;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp72;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp73;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQual" +
                        "ifiedName1\"},Invalid,out _,out _)\'");
                temp73 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp71, out temp72);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S146\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp71, "translateSids of LookUpNames, state S146");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp72, "mapCount of LookUpNames, state S146");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp73, "return of LookUpNames, state S146");
                TestScenarioS4S161();
                goto label5;
            }
            if ((temp79 == 1)) {
                this.Manager.Comment("reaching state \'S51\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp74;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp75;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,2048,out _)\'");
                temp75 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp74);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S83\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp74, "openPolicyHandle of OpenPolicy, state S83");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp75, "return of OpenPolicy, state S83");
                this.Manager.Comment("reaching state \'S115\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp76;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp77;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp78;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQual" +
                        "ifiedName1\"},Invalid,out _,out _)\'");
                temp78 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp76, out temp77);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S147\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp76, "translateSids of LookUpNames, state S147");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp77, "mapCount of LookUpNames, state S147");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp78, "return of LookUpNames, state S147");
                TestScenarioS4S160();
                goto label5;
            }
            throw new InvalidOperationException("never reached");
        label5:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S18InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S19");
        }
        
        private void TestScenarioS4S18InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S19");
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS4S2() {
            this.Manager.BeginTest("TestScenarioS4S2");
            this.Manager.Comment("reaching state \'S2\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp80;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp80 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp80);
            this.Manager.Comment("reaching state \'S3\'");
            int temp91 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S2InitializeChecker)), new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S2InitializeChecker1)));
            if ((temp91 == 0)) {
                this.Manager.Comment("reaching state \'S34\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp81;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp82;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
                temp82 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp81);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S66\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp81, "openPolicyHandle of OpenPolicy, state S66");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp82, "return of OpenPolicy, state S66");
                this.Manager.Comment("reaching state \'S98\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp83;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp84;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp85;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQual" +
                        "ifiedName1\"},LookUpWKSTA,out _,out _)\'");
                temp85 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp83, out temp84);
                this.Manager.Checkpoint("MS-LSAT_R435");
                this.Manager.Checkpoint("MS-LSAT_R398");
                this.Manager.Comment("reaching state \'S130\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp83, "translateSids of LookUpNames, state S130");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp84, "mapCount of LookUpNames, state S130");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp85, "return of LookUpNames, state S130");
                TestScenarioS4S162();
                goto label6;
            }
            if ((temp91 == 1)) {
                this.Manager.Comment("reaching state \'S35\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp86;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp87;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
                temp87 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp86);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S67\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp86, "openPolicyHandle of OpenPolicy, state S67");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp87, "return of OpenPolicy, state S67");
                this.Manager.Comment("reaching state \'S99\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp88;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp89;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp90;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQual" +
                        "ifiedName1\"},LookUpWKSTA,out _,out _)\'");
                temp90 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp88, out temp89);
                this.Manager.Checkpoint("MS-LSAT_R435");
                this.Manager.Checkpoint("MS-LSAT_R398");
                this.Manager.Comment("reaching state \'S131\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp88, "translateSids of LookUpNames, state S131");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp89, "mapCount of LookUpNames, state S131");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp90, "return of LookUpNames, state S131");
                TestScenarioS4S163();
                goto label6;
            }
            throw new InvalidOperationException("never reached");
        label6:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S2InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S3");
        }
        
        private void TestScenarioS4S2InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S3");
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS4S20() {
            this.Manager.BeginTest("TestScenarioS4S20");
            this.Manager.Comment("reaching state \'S20\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp92;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp92 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp92);
            this.Manager.Comment("reaching state \'S21\'");
            int temp103 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S20InitializeChecker)), new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S20InitializeChecker1)));
            if ((temp103 == 0)) {
                this.Manager.Comment("reaching state \'S52\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp93;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp94;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,2048,out _)\'");
                temp94 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp93);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S84\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp93, "openPolicyHandle of OpenPolicy, state S84");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp94, "return of OpenPolicy, state S84");
                this.Manager.Comment("reaching state \'S116\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp95;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp96;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp97;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"DoesNotExist\"},Invalid,out _,out _)\'");
                temp97 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp95, out temp96);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S148\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp95, "translateSids of LookUpNames, state S148");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp96, "mapCount of LookUpNames, state S148");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp97, "return of LookUpNames, state S148");
                TestScenarioS4S161();
                goto label7;
            }
            if ((temp103 == 1)) {
                this.Manager.Comment("reaching state \'S53\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp98;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp99;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,2048,out _)\'");
                temp99 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp98);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S85\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp98, "openPolicyHandle of OpenPolicy, state S85");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp99, "return of OpenPolicy, state S85");
                this.Manager.Comment("reaching state \'S117\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp100;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp101;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp102;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"DoesNotExist\"},Invalid,out _,out _)\'");
                temp102 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp100, out temp101);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S149\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp100, "translateSids of LookUpNames, state S149");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp101, "mapCount of LookUpNames, state S149");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp102, "return of LookUpNames, state S149");
                TestScenarioS4S160();
                goto label7;
            }
            throw new InvalidOperationException("never reached");
        label7:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S20InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S21");
        }
        
        private void TestScenarioS4S20InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S21");
        }
        #endregion
        
        #region Test Starting in S22
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS4S22() {
            this.Manager.BeginTest("TestScenarioS4S22");
            this.Manager.Comment("reaching state \'S22\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp104;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp104 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp104);
            this.Manager.Comment("reaching state \'S23\'");
            int temp115 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S22InitializeChecker)), new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S22InitializeChecker1)));
            if ((temp115 == 0)) {
                this.Manager.Comment("reaching state \'S54\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp105;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp106;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,2048,out _)\'");
                temp106 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp105);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S86\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp105, "openPolicyHandle of OpenPolicy, state S86");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp106, "return of OpenPolicy, state S86");
                this.Manager.Comment("reaching state \'S118\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp107;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp108;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp109;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalNa" +
                        "me\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},Invalid,out _,out _)\'");
                temp109 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp107, out temp108);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S150\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp107, "translateSids of LookUpNames, state S150");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp108, "mapCount of LookUpNames, state S150");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp109, "return of LookUpNames, state S150");
                TestScenarioS4S161();
                goto label8;
            }
            if ((temp115 == 1)) {
                this.Manager.Comment("reaching state \'S55\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp110;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp111;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,2048,out _)\'");
                temp111 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp110);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S87\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp110, "openPolicyHandle of OpenPolicy, state S87");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp111, "return of OpenPolicy, state S87");
                this.Manager.Comment("reaching state \'S119\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp112;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp113;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp114;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalNa" +
                        "me\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},Invalid,out _,out _)\'");
                temp114 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp112, out temp113);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S151\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp112, "translateSids of LookUpNames, state S151");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp113, "mapCount of LookUpNames, state S151");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp114, "return of LookUpNames, state S151");
                TestScenarioS4S160();
                goto label8;
            }
            throw new InvalidOperationException("never reached");
        label8:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S22InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S23");
        }
        
        private void TestScenarioS4S22InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S23");
        }
        #endregion
        
        #region Test Starting in S24
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS4S24() {
            this.Manager.BeginTest("TestScenarioS4S24");
            this.Manager.Comment("reaching state \'S24\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp116;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp116 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp116);
            this.Manager.Comment("reaching state \'S25\'");
            int temp127 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S24InitializeChecker)), new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S24InitializeChecker1)));
            if ((temp127 == 0)) {
                this.Manager.Comment("reaching state \'S56\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp117;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp118;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,2048,out _)\'");
                temp118 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp117);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S88\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp117, "openPolicyHandle of OpenPolicy, state S88");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp118, "return of OpenPolicy, state S88");
                this.Manager.Comment("reaching state \'S120\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp119;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp120;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp121;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalNa" +
                        "me\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},LookUpWKSTA,out _,out _" +
                        ")\'");
                temp121 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp119, out temp120);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S152\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp119, "translateSids of LookUpNames, state S152");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp120, "mapCount of LookUpNames, state S152");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp121, "return of LookUpNames, state S152");
                TestScenarioS4S161();
                goto label9;
            }
            if ((temp127 == 1)) {
                this.Manager.Comment("reaching state \'S57\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp122;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp123;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,2048,out _)\'");
                temp123 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp122);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S89\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp122, "openPolicyHandle of OpenPolicy, state S89");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp123, "return of OpenPolicy, state S89");
                this.Manager.Comment("reaching state \'S121\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp124;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp125;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp126;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"InvalidIsolatedName\",\"InvalidUserPrincipalNa" +
                        "me\",\"InvalidFullQualifiedName\",\"InvalidUnQualifiedName\"},LookUpWKSTA,out _,out _" +
                        ")\'");
                temp126 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "InvalidIsolatedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUserPrincipalName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidFullQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "InvalidUnQualifiedName", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp124, out temp125);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S153\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp124, "translateSids of LookUpNames, state S153");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp125, "mapCount of LookUpNames, state S153");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp126, "return of LookUpNames, state S153");
                TestScenarioS4S160();
                goto label9;
            }
            throw new InvalidOperationException("never reached");
        label9:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S24InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S25");
        }
        
        private void TestScenarioS4S24InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S25");
        }
        #endregion
        
        #region Test Starting in S26
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS4S26() {
            this.Manager.BeginTest("TestScenarioS4S26");
            this.Manager.Comment("reaching state \'S26\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp128;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp128 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp128);
            this.Manager.Comment("reaching state \'S27\'");
            int temp139 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S26InitializeChecker)), new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S26InitializeChecker1)));
            if ((temp139 == 0)) {
                this.Manager.Comment("reaching state \'S58\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp129;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp130;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,2048,out _)\'");
                temp130 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp129);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S90\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp129, "openPolicyHandle of OpenPolicy, state S90");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp130, "return of OpenPolicy, state S90");
                this.Manager.Comment("reaching state \'S122\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp131;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp132;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp133;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"Un" +
                        "QualifiedName1\",\"IsolatedName1\"},Invalid,out _,out _)\'");
                temp133 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp131, out temp132);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S154\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp131, "translateSids of LookUpNames, state S154");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp132, "mapCount of LookUpNames, state S154");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp133, "return of LookUpNames, state S154");
                TestScenarioS4S161();
                goto label10;
            }
            if ((temp139 == 1)) {
                this.Manager.Comment("reaching state \'S59\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp134;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp135;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,2048,out _)\'");
                temp135 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp134);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S91\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp134, "openPolicyHandle of OpenPolicy, state S91");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp135, "return of OpenPolicy, state S91");
                this.Manager.Comment("reaching state \'S123\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp136;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp137;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp138;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"Un" +
                        "QualifiedName1\",\"IsolatedName1\"},Invalid,out _,out _)\'");
                temp138 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp136, out temp137);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S155\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp136, "translateSids of LookUpNames, state S155");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp137, "mapCount of LookUpNames, state S155");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp138, "return of LookUpNames, state S155");
                TestScenarioS4S160();
                goto label10;
            }
            throw new InvalidOperationException("never reached");
        label10:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S26InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S27");
        }
        
        private void TestScenarioS4S26InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S27");
        }
        #endregion
        
        #region Test Starting in S28
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS4S28() {
            this.Manager.BeginTest("TestScenarioS4S28");
            this.Manager.Comment("reaching state \'S28\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp140;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp140 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp140);
            this.Manager.Comment("reaching state \'S29\'");
            int temp151 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S28InitializeChecker)), new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S28InitializeChecker1)));
            if ((temp151 == 0)) {
                this.Manager.Comment("reaching state \'S60\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp141;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp142;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,2048,out _)\'");
                temp142 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp141);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S92\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp141, "openPolicyHandle of OpenPolicy, state S92");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp142, "return of OpenPolicy, state S92");
                this.Manager.Comment("reaching state \'S124\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp143;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp144;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp145;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQual" +
                        "ifiedName1\"},LookUpWKSTA,out _,out _)\'");
                temp145 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp143, out temp144);
                this.Manager.Checkpoint("MS-LSAT_R397");
                this.Manager.Checkpoint("MS-LSAT_R431");
                this.Manager.Comment("reaching state \'S156\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:SomeNotMapped\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp143, "translateSids of LookUpNames, state S156");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp144, "mapCount of LookUpNames, state S156");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.SomeNotMapped, temp145, "return of LookUpNames, state S156");
                TestScenarioS4S161();
                goto label11;
            }
            if ((temp151 == 1)) {
                this.Manager.Comment("reaching state \'S61\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp146;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp147;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,2048,out _)\'");
                temp147 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 2048u, out temp146);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S93\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp146, "openPolicyHandle of OpenPolicy, state S93");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp147, "return of OpenPolicy, state S93");
                this.Manager.Comment("reaching state \'S125\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp148;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp149;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp150;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQual" +
                        "ifiedName1\"},LookUpWKSTA,out _,out _)\'");
                temp150 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp148, out temp149);
                this.Manager.Checkpoint("MS-LSAT_R397");
                this.Manager.Checkpoint("MS-LSAT_R431");
                this.Manager.Comment("reaching state \'S157\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:SomeNotMapped\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp148, "translateSids of LookUpNames, state S157");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp149, "mapCount of LookUpNames, state S157");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.SomeNotMapped, temp150, "return of LookUpNames, state S157");
                TestScenarioS4S160();
                goto label11;
            }
            throw new InvalidOperationException("never reached");
        label11:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S28InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S29");
        }
        
        private void TestScenarioS4S28InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S29");
        }
        #endregion
        
        #region Test Starting in S30
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS4S30() {
            this.Manager.BeginTest("TestScenarioS4S30");
            this.Manager.Comment("reaching state \'S30\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp152;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp152 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp152);
            this.Manager.Comment("reaching state \'S31\'");
            int temp163 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S30InitializeChecker)), new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S30InitializeChecker1)));
            if ((temp163 == 0)) {
                this.Manager.Comment("reaching state \'S62\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp153;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp154;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
                temp154 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp153);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S94\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp153, "openPolicyHandle of OpenPolicy, state S94");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp154, "return of OpenPolicy, state S94");
                this.Manager.Comment("reaching state \'S126\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp155;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp156;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp157;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"Un" +
                        "QualifiedName1\",\"IsolatedName1\"},Invalid,out _,out _)\'");
                temp157 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp155, out temp156);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S158\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp155, "translateSids of LookUpNames, state S158");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp156, "mapCount of LookUpNames, state S158");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp157, "return of LookUpNames, state S158");
                TestScenarioS4S162();
                goto label12;
            }
            if ((temp163 == 1)) {
                this.Manager.Comment("reaching state \'S63\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp158;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp159;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
                temp159 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp158);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S95\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp158, "openPolicyHandle of OpenPolicy, state S95");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp159, "return of OpenPolicy, state S95");
                this.Manager.Comment("reaching state \'S127\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp160;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp161;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp162;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"Un" +
                        "QualifiedName1\",\"IsolatedName1\"},Invalid,out _,out _)\'");
                temp162 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp160, out temp161);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S159\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp160, "translateSids of LookUpNames, state S159");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp161, "mapCount of LookUpNames, state S159");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp162, "return of LookUpNames, state S159");
                TestScenarioS4S163();
                goto label12;
            }
            throw new InvalidOperationException("never reached");
        label12:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S30InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S31");
        }
        
        private void TestScenarioS4S30InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S31");
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("MS-LSAT")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("PDC")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAT_TestScenarioS4S4() {
            this.Manager.BeginTest("TestScenarioS4S4");
            this.Manager.Comment("reaching state \'S4\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp164;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp164 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp164);
            this.Manager.Comment("reaching state \'S5\'");
            int temp175 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S4InitializeChecker)), new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S4InitializeChecker1)));
            if ((temp175 == 0)) {
                this.Manager.Comment("reaching state \'S36\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp165;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp166;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
                temp166 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp165);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S68\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp165, "openPolicyHandle of OpenPolicy, state S68");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp166, "return of OpenPolicy, state S68");
                this.Manager.Comment("reaching state \'S100\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp167;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp168;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp169;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"DoesNotExist\"},LookUpWKSTA,out _,out _)\'");
                temp169 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp167, out temp168);
                this.Manager.Checkpoint("MS-LSAT_R435");
                this.Manager.Checkpoint("MS-LSAT_R398");
                this.Manager.Comment("reaching state \'S132\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp167, "translateSids of LookUpNames, state S132");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp168, "mapCount of LookUpNames, state S132");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp169, "return of LookUpNames, state S132");
                TestScenarioS4S162();
                goto label13;
            }
            if ((temp175 == 1)) {
                this.Manager.Comment("reaching state \'S37\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp170;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp171;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
                temp171 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp170);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S69\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp170, "openPolicyHandle of OpenPolicy, state S69");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp171, "return of OpenPolicy, state S69");
                this.Manager.Comment("reaching state \'S101\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp172;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp173;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp174;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"DoesNotExist\"},LookUpWKSTA,out _,out _)\'");
                temp174 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp172, out temp173);
                this.Manager.Checkpoint("MS-LSAT_R435");
                this.Manager.Checkpoint("MS-LSAT_R398");
                this.Manager.Comment("reaching state \'S133\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp172, "translateSids of LookUpNames, state S133");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp173, "mapCount of LookUpNames, state S133");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp174, "return of LookUpNames, state S133");
                TestScenarioS4S163();
                goto label13;
            }
            throw new InvalidOperationException("never reached");
        label13:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S4InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S5");
        }
        
        private void TestScenarioS4S4InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
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
        public void LSAT_TestScenarioS4S6() {
            this.Manager.BeginTest("TestScenarioS4S6");
            this.Manager.Comment("reaching state \'S6\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp176;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp176 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp176);
            this.Manager.Comment("reaching state \'S7\'");
            int temp187 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S6InitializeChecker)), new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S6InitializeChecker1)));
            if ((temp187 == 0)) {
                this.Manager.Comment("reaching state \'S38\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp177;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp178;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
                temp178 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp177);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S70\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp177, "openPolicyHandle of OpenPolicy, state S70");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp178, "return of OpenPolicy, state S70");
                this.Manager.Comment("reaching state \'S102\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp179;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp180;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp181;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"Un" +
                        "QualifiedName1\",\"IsolatedName1\"},LookUpWKSTA,out _,out _)\'");
                temp181 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp179, out temp180);
                this.Manager.Checkpoint("MS-LSAT_R435");
                this.Manager.Checkpoint("MS-LSAT_R398");
                this.Manager.Comment("reaching state \'S134\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp179, "translateSids of LookUpNames, state S134");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp180, "mapCount of LookUpNames, state S134");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp181, "return of LookUpNames, state S134");
                TestScenarioS4S162();
                goto label14;
            }
            if ((temp187 == 1)) {
                this.Manager.Comment("reaching state \'S39\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp182;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp183;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
                temp183 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp182);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S71\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp182, "openPolicyHandle of OpenPolicy, state S71");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp183, "return of OpenPolicy, state S71");
                this.Manager.Comment("reaching state \'S103\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp184;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp185;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp186;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"UserPrincipalName1\",\"FullQualifiedName1\",\"Un" +
                        "QualifiedName1\",\"IsolatedName1\"},LookUpWKSTA,out _,out _)\'");
                temp186 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                            "Element"}, new object[] {
                                                            Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "UnQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "IsolatedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel)(1)), out temp184, out temp185);
                this.Manager.Checkpoint("MS-LSAT_R435");
                this.Manager.Checkpoint("MS-LSAT_R398");
                this.Manager.Comment("reaching state \'S135\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:AccessDenied\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp184, "translateSids of LookUpNames, state S135");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp185, "mapCount of LookUpNames, state S135");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(0)), temp186, "return of LookUpNames, state S135");
                TestScenarioS4S163();
                goto label14;
            }
            throw new InvalidOperationException("never reached");
        label14:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S6InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S7");
        }
        
        private void TestScenarioS4S6InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
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
        public void LSAT_TestScenarioS4S8() {
            this.Manager.BeginTest("TestScenarioS4S8");
            this.Manager.Comment("reaching state \'S8\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig temp188;
            this.Manager.Comment("executing step \'call Initialize(Disable,1)\'");
            temp188 = this.ILsatAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.AnonymousAccess)(0)), 1u);
            this.Manager.AddReturn(InitializeInfo, null, temp188);
            this.Manager.Comment("reaching state \'S9\'");
            int temp199 = this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S8InitializeChecker)), new ExpectedReturn(TestScenarioS4.InitializeInfo, null, new InitializeDelegate1(this.TestScenarioS4S8InitializeChecker1)));
            if ((temp199 == 0)) {
                this.Manager.Comment("reaching state \'S40\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp189;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp190;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
                temp190 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp189);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S72\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp189, "openPolicyHandle of OpenPolicy, state S72");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp190, "return of OpenPolicy, state S72");
                this.Manager.Comment("reaching state \'S104\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp191;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp192;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp193;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQual" +
                        "ifiedName1\"},Invalid,out _,out _)\'");
                temp193 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp191, out temp192);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S136\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp191, "translateSids of LookUpNames, state S136");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp192, "mapCount of LookUpNames, state S136");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp193, "return of LookUpNames, state S136");
                TestScenarioS4S162();
                goto label15;
            }
            if ((temp199 == 1)) {
                this.Manager.Comment("reaching state \'S41\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle temp194;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp195;
                this.Manager.Comment("executing step \'call OpenPolicy(Null,1,out _)\'");
                temp195 = this.ILsatAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.RootDirectory)(0)), 1u, out temp194);
                this.Manager.Checkpoint("MS-LSAT_R178");
                this.Manager.Checkpoint("MS-LSAT_R173");
                this.Manager.Comment("reaching state \'S73\'");
                this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.Handle)(0)), temp194, "openPolicyHandle of OpenPolicy, state S73");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus.Success, temp195, "return of OpenPolicy, state S73");
                this.Manager.Comment("reaching state \'S105\'");
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid temp196;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount temp197;
                Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus temp198;
                this.Manager.Comment("executing step \'call LookUpNames(1,{\"UserPrincipalName1\",\"DoesNotExist\",\"FullQual" +
                        "ifiedName1\"},Invalid,out _,out _)\'");
                temp198 = this.ILsatAdapterInstance.LookUpNames(1, this.Make<Microsoft.Modeling.Set<string>>(new string[] {
                                "Rep"}, new object[] {
                                Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Xrt.Runtime.Singleton>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Xrt.Runtime.Singleton>(), "UserPrincipalName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                        "Element"}, new object[] {
                                                        Microsoft.Xrt.Runtime.Singleton.Single})), "DoesNotExist", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                    "Element"}, new object[] {
                                                    Microsoft.Xrt.Runtime.Singleton.Single})), "FullQualifiedName1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Xrt.Runtime.Singleton>>(new string[] {
                                                "Element"}, new object[] {
                                                Microsoft.Xrt.Runtime.Singleton.Single}))}), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.LookUpLevel.Invalid, out temp196, out temp197);
                this.Manager.Checkpoint("MS-LSAT_R399");
                this.Manager.Comment("reaching state \'S137\'");
                this.Manager.Comment("checking step \'return LookUpNames/[out Invalid,out Invalid]:InvalidParameter\'");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.TranslatedSid)(1)), temp196, "translateSids of LookUpNames, state S137");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.MappedCount)(1)), temp197, "mapCount of LookUpNames, state S137");
                TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ErrorStatus)(1)), temp198, "return of LookUpNames, state S137");
                TestScenarioS4S163();
                goto label15;
            }
            throw new InvalidOperationException("never reached");
        label15:
;
            this.Manager.EndTest();
        }
        
        private void TestScenarioS4S8InitializeChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/DomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(0)), @return, "return of Initialize, state S9");
        }
        
        private void TestScenarioS4S8InitializeChecker1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig @return) {
            this.Manager.Comment("checking step \'return Initialize/NonDomainController\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsat.ProtocolServerConfig)(1)), @return, "return of Initialize, state S9");
        }
        #endregion
    }
}
