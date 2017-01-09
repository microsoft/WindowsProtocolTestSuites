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
    public partial class TestScenarioS3 : PtfTestClassBase {
        
        public TestScenarioS3() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "10000");
        }
        
        #region Expect Delegates
        public delegate void CloseDelegate1(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return);
        #endregion
        
        #region Event Metadata
        static System.Reflection.MethodBase CloseInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadManagedAdapter), "Close", typeof(int), typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle).MakeByRefType());
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
        public void LSAD_TestScenarioS3S0() {
            this.Manager.BeginTest("TestScenarioS3S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S138\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp1 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp0);
            this.Manager.Comment("reaching state \'S207\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy2, state S207");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy2, state S207");
            this.Manager.Comment("reaching state \'S276\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp2;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyDnsDomainInformationInt)\'");
            temp2 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformationInt);
            this.Manager.Comment("reaching state \'S345\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp2, "return of SetInformationPolicy2, state S345");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS3S414() {
            this.Manager.Comment("reaching state \'S414\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp3;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp4;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp4 = this.ILsadManagedAdapterInstance.Close(1, out temp3);
            this.Manager.AddReturn(CloseInfo, null, temp3, temp4);
            TestScenarioS3S429();
        }
        
        private void TestScenarioS3S429() {
            this.Manager.Comment("reaching state \'S429\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS3.CloseInfo, null, new CloseDelegate1(this.TestScenarioS3S0CloseChecker)));
            this.Manager.Comment("reaching state \'S436\'");
        }
        
        private void TestScenarioS3S0CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S429");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S429");
        }
        #endregion
        
        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S10() {
            this.Manager.BeginTest("TestScenarioS3S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S143\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp5;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp6;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp6 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp5);
            this.Manager.Comment("reaching state \'S212\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp5, "policyHandle of OpenPolicy2, state S212");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp6, "return of OpenPolicy2, state S212");
            this.Manager.Comment("reaching state \'S281\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp7;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyPrimaryDomainInformation)\'");
            temp7 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPrimaryDomainInformation);
            this.Manager.Comment("reaching state \'S350\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp7, "return of SetInformationPolicy2, state S350");
            this.Manager.Comment("reaching state \'S419\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp8;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp9;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp9 = this.ILsadManagedAdapterInstance.Close(1, out temp8);
            this.Manager.AddReturn(CloseInfo, null, temp8, temp9);
            TestScenarioS3S433();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS3S433() {
            this.Manager.Comment("reaching state \'S433\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS3.CloseInfo, null, new CloseDelegate1(this.TestScenarioS3S10CloseChecker)));
            this.Manager.Comment("reaching state \'S440\'");
        }
        
        private void TestScenarioS3S10CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S433");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S433");
        }
        #endregion
        
        #region Test Starting in S100
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S100() {
            this.Manager.BeginTest("TestScenarioS3S100");
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S188\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp10;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp11;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp11 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp10);
            this.Manager.Comment("reaching state \'S257\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp10, "policyHandle of OpenPolicy2, state S257");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp11, "return of OpenPolicy2, state S257");
            this.Manager.Comment("reaching state \'S326\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp12;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyDnsDomainInformationInt)\'");
            temp12 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformationInt);
            this.Manager.Comment("reaching state \'S395\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp12, "return of SetInformationPolicy2, state S395");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S102
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S102() {
            this.Manager.BeginTest("TestScenarioS3S102");
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S189\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp13;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp14;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp14 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp13);
            this.Manager.Comment("reaching state \'S258\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp13, "policyHandle of OpenPolicy2, state S258");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp14, "return of OpenPolicy2, state S258");
            this.Manager.Comment("reaching state \'S327\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp15;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyDnsDomainInformation)\'");
            temp15 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformation);
            this.Manager.Comment("reaching state \'S396\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp15, "return of SetInformationPolicy2, state S396");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S104
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S104() {
            this.Manager.BeginTest("TestScenarioS3S104");
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S190\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp16;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp17;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp17 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp16);
            this.Manager.Comment("reaching state \'S259\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp16, "policyHandle of OpenPolicy2, state S259");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp17, "return of OpenPolicy2, state S259");
            this.Manager.Comment("reaching state \'S328\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp18;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyAccountDomainInformation)\'");
            temp18 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation);
            this.Manager.Comment("reaching state \'S397\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp18, "return of SetInformationPolicy2, state S397");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S106
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S106() {
            this.Manager.BeginTest("TestScenarioS3S106");
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S191\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp19;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp20;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp20 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp19);
            this.Manager.Comment("reaching state \'S260\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp19, "policyHandle of OpenPolicy2, state S260");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp20, "return of OpenPolicy2, state S260");
            this.Manager.Comment("reaching state \'S329\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp21;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyPdAccountInformation)\'");
            temp21 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPdAccountInformation);
            this.Manager.Comment("reaching state \'S398\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp21, "return of SetInformationPolicy2, state S398");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S108
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S108() {
            this.Manager.BeginTest("TestScenarioS3S108");
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S192\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp22;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp23;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp23 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp22);
            this.Manager.Comment("reaching state \'S261\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp22, "policyHandle of OpenPolicy2, state S261");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp23, "return of OpenPolicy2, state S261");
            this.Manager.Comment("reaching state \'S330\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp24;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyPdAccountInformation)\'");
            temp24 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPdAccountInformation);
            this.Manager.Comment("reaching state \'S399\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp24, "return of SetInformationPolicy2, state S399");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S110
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S110() {
            this.Manager.BeginTest("TestScenarioS3S110");
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S193\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp25;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp26;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp26 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp25);
            this.Manager.Comment("reaching state \'S262\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp25, "policyHandle of OpenPolicy2, state S262");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp26, "return of OpenPolicy2, state S262");
            this.Manager.Comment("reaching state \'S331\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp27;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyModificationInformation)\'");
            temp27 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyModificationInformation);
            this.Manager.Comment("reaching state \'S400\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp27, "return of SetInformationPolicy2, state S400");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S112
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S112() {
            this.Manager.BeginTest("TestScenarioS3S112");
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S194\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp28;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp29;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp29 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp28);
            this.Manager.Comment("reaching state \'S263\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp28, "policyHandle of OpenPolicy2, state S263");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp29, "return of OpenPolicy2, state S263");
            this.Manager.Comment("reaching state \'S332\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp30;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyAuditFullQueryInformation)\'");
            temp30 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullQueryInformation);
            this.Manager.Comment("reaching state \'S401\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp30, "return of SetInformationPolicy2, state S401");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S114
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S114() {
            this.Manager.BeginTest("TestScenarioS3S114");
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S195\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp31;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp32;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp32 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp31);
            this.Manager.Comment("reaching state \'S264\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp31, "policyHandle of OpenPolicy2, state S264");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp32, "return of OpenPolicy2, state S264");
            this.Manager.Comment("reaching state \'S333\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp33;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyAuditFullSetInformation)\'");
            temp33 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullSetInformation);
            this.Manager.Comment("reaching state \'S402\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp33, "return of SetInformationPolicy2, state S402");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S116
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S116() {
            this.Manager.BeginTest("TestScenarioS3S116");
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S196\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp34;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp35;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp35 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp34);
            this.Manager.Comment("reaching state \'S265\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp34, "policyHandle of OpenPolicy2, state S265");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp35, "return of OpenPolicy2, state S265");
            this.Manager.Comment("reaching state \'S334\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp36;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyModificationInformation)\'");
            temp36 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyModificationInformation);
            this.Manager.Comment("reaching state \'S403\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp36, "return of SetInformationPolicy2, state S403");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S118
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S118() {
            this.Manager.BeginTest("TestScenarioS3S118");
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S197\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp37;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp38;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp38 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp37);
            this.Manager.Comment("reaching state \'S266\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp37, "policyHandle of OpenPolicy2, state S266");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp38, "return of OpenPolicy2, state S266");
            this.Manager.Comment("reaching state \'S335\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp39;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyAuditFullQueryInformation)\'");
            temp39 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullQueryInformation);
            this.Manager.Comment("reaching state \'S404\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp39, "return of SetInformationPolicy2, state S404");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S12() {
            this.Manager.BeginTest("TestScenarioS3S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S144\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp40;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp41;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp41 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp40);
            this.Manager.Comment("reaching state \'S213\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp40, "policyHandle of OpenPolicy2, state S213");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp41, "return of OpenPolicy2, state S213");
            this.Manager.Comment("reaching state \'S282\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp42;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyAuditEventsInformation)\'");
            temp42 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditEventsInformation);
            this.Manager.Comment("reaching state \'S351\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp42, "return of SetInformationPolicy2, state S351");
            this.Manager.Comment("reaching state \'S420\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp43;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp44;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp44 = this.ILsadManagedAdapterInstance.Close(1, out temp43);
            this.Manager.AddReturn(CloseInfo, null, temp43, temp44);
            TestScenarioS3S434();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS3S434() {
            this.Manager.Comment("reaching state \'S434\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS3.CloseInfo, null, new CloseDelegate1(this.TestScenarioS3S12CloseChecker)));
            this.Manager.Comment("reaching state \'S441\'");
        }
        
        private void TestScenarioS3S12CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S434");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S434");
        }
        #endregion
        
        #region Test Starting in S120
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S120() {
            this.Manager.BeginTest("TestScenarioS3S120");
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S198\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp45;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp46;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp46 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp45);
            this.Manager.Comment("reaching state \'S267\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp45, "policyHandle of OpenPolicy2, state S267");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp46, "return of OpenPolicy2, state S267");
            this.Manager.Comment("reaching state \'S336\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp47;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyAuditFullSetInformation)\'");
            temp47 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullSetInformation);
            this.Manager.Comment("reaching state \'S405\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp47, "return of SetInformationPolicy2, state S405");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S122
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S122() {
            this.Manager.BeginTest("TestScenarioS3S122");
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S199\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp48;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp49;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp49 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp48);
            this.Manager.Comment("reaching state \'S268\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp48, "policyHandle of OpenPolicy2, state S268");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp49, "return of OpenPolicy2, state S268");
            this.Manager.Comment("reaching state \'S337\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp50;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyAccountDomainInformation)\'");
            temp50 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation);
            this.Manager.Comment("reaching state \'S406\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp50, "return of SetInformationPolicy2, state S406");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S124
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S124() {
            this.Manager.BeginTest("TestScenarioS3S124");
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S200\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp51;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp52;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp52 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp51);
            this.Manager.Comment("reaching state \'S269\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp51, "policyHandle of OpenPolicy2, state S269");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp52, "return of OpenPolicy2, state S269");
            this.Manager.Comment("reaching state \'S338\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp53;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyAuditEventsInformation)\'");
            temp53 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditEventsInformation);
            this.Manager.Comment("reaching state \'S407\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp53, "return of SetInformationPolicy2, state S407");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S126
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S126() {
            this.Manager.BeginTest("TestScenarioS3S126");
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S201\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp54;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp55;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp55 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp54);
            this.Manager.Comment("reaching state \'S270\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp54, "policyHandle of OpenPolicy2, state S270");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp55, "return of OpenPolicy2, state S270");
            this.Manager.Comment("reaching state \'S339\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp56;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyPrimaryDomainInformation)\'");
            temp56 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPrimaryDomainInformation);
            this.Manager.Comment("reaching state \'S408\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp56, "return of SetInformationPolicy2, state S408");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S128
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S128() {
            this.Manager.BeginTest("TestScenarioS3S128");
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S202\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp57;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp58;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp58 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp57);
            this.Manager.Comment("reaching state \'S271\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp57, "policyHandle of OpenPolicy2, state S271");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp58, "return of OpenPolicy2, state S271");
            this.Manager.Comment("reaching state \'S340\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp59;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyDnsDomainInformation)\'");
            temp59 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformation);
            this.Manager.Comment("reaching state \'S409\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp59, "return of SetInformationPolicy2, state S409");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S130
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S130() {
            this.Manager.BeginTest("TestScenarioS3S130");
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S203\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp60;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp61;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp61 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp60);
            this.Manager.Comment("reaching state \'S272\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp60, "policyHandle of OpenPolicy2, state S272");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp61, "return of OpenPolicy2, state S272");
            this.Manager.Comment("reaching state \'S341\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp62;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyLsaServerRoleInformation)\'");
            temp62 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLsaServerRoleInformation);
            this.Manager.Comment("reaching state \'S410\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp62, "return of SetInformationPolicy2, state S410");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S132
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S132() {
            this.Manager.BeginTest("TestScenarioS3S132");
            this.Manager.Comment("reaching state \'S132\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S133\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S204\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp63;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp64;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp64 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp63);
            this.Manager.Comment("reaching state \'S273\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp63, "policyHandle of OpenPolicy2, state S273");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp64, "return of OpenPolicy2, state S273");
            this.Manager.Comment("reaching state \'S342\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp65;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyReplicaSourceInformation)\'");
            temp65 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyReplicaSourceInformation);
            this.Manager.Comment("reaching state \'S411\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp65, "return of SetInformationPolicy2, state S411");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S134
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S134() {
            this.Manager.BeginTest("TestScenarioS3S134");
            this.Manager.Comment("reaching state \'S134\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S135\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S205\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp66;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp67;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp67 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp66);
            this.Manager.Comment("reaching state \'S274\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp66, "policyHandle of OpenPolicy2, state S274");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp67, "return of OpenPolicy2, state S274");
            this.Manager.Comment("reaching state \'S343\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp68;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyAuditLogInformation)\'");
            temp68 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass)(1)));
            this.Manager.Comment("reaching state \'S412\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/NotImplemented\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NotImplemented, temp68, "return of SetInformationPolicy2, state S412");
            TestScenarioS3S415();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS3S415() {
            this.Manager.Comment("reaching state \'S415\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp69;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp70;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp70 = this.ILsadManagedAdapterInstance.Close(1, out temp69);
            this.Manager.AddReturn(CloseInfo, null, temp69, temp70);
            TestScenarioS3S429();
        }
        #endregion
        
        #region Test Starting in S136
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S136() {
            this.Manager.BeginTest("TestScenarioS3S136");
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S137\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S206\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp71;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp72;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp72 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp71);
            this.Manager.Comment("reaching state \'S275\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp71, "policyHandle of OpenPolicy2, state S275");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp72, "return of OpenPolicy2, state S275");
            this.Manager.Comment("reaching state \'S344\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp73;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyAuditLogInformation)\'");
            temp73 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass)(1)));
            this.Manager.Comment("reaching state \'S413\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/NotImplemented\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.NotImplemented, temp73, "return of SetInformationPolicy2, state S413");
            TestScenarioS3S422();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS3S422() {
            this.Manager.Comment("reaching state \'S422\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp74;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp75;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp75 = this.ILsadManagedAdapterInstance.Close(1, out temp74);
            this.Manager.AddReturn(CloseInfo, null, temp74, temp75);
            TestScenarioS3S429();
        }
        #endregion
        
        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S14() {
            this.Manager.BeginTest("TestScenarioS3S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S145\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp76;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp77;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp77 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp76);
            this.Manager.Comment("reaching state \'S214\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp76, "policyHandle of OpenPolicy2, state S214");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp77, "return of OpenPolicy2, state S214");
            this.Manager.Comment("reaching state \'S283\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp78;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyDnsDomainInformationInt)\'");
            temp78 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformationInt);
            this.Manager.Comment("reaching state \'S352\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp78, "return of SetInformationPolicy2, state S352");
            this.Manager.Comment("reaching state \'S421\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp79;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp80;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp80 = this.ILsadManagedAdapterInstance.Close(1, out temp79);
            this.Manager.AddReturn(CloseInfo, null, temp79, temp80);
            TestScenarioS3S435();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS3S435() {
            this.Manager.Comment("reaching state \'S435\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS3.CloseInfo, null, new CloseDelegate1(this.TestScenarioS3S14CloseChecker)));
            this.Manager.Comment("reaching state \'S442\'");
        }
        
        private void TestScenarioS3S14CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S435");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S435");
        }
        #endregion
        
        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S16() {
            this.Manager.BeginTest("TestScenarioS3S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S146\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp81;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp82;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp82 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp81);
            this.Manager.Comment("reaching state \'S215\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp81, "policyHandle of OpenPolicy2, state S215");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp82, "return of OpenPolicy2, state S215");
            this.Manager.Comment("reaching state \'S284\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp83;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyPdAccountInformation)\'");
            temp83 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPdAccountInformation);
            this.Manager.Comment("reaching state \'S353\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp83, "return of SetInformationPolicy2, state S353");
            TestScenarioS3S415();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S18() {
            this.Manager.BeginTest("TestScenarioS3S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S147\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp84;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp85;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp85 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp84);
            this.Manager.Comment("reaching state \'S216\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp84, "policyHandle of OpenPolicy2, state S216");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp85, "return of OpenPolicy2, state S216");
            this.Manager.Comment("reaching state \'S285\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp86;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyPdAccountInformation)\'");
            temp86 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPdAccountInformation);
            this.Manager.Comment("reaching state \'S354\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp86, "return of SetInformationPolicy2, state S354");
            TestScenarioS3S415();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S2() {
            this.Manager.BeginTest("TestScenarioS3S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S139\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp87;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp88;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp88 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp87);
            this.Manager.Comment("reaching state \'S208\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp87, "policyHandle of OpenPolicy2, state S208");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp88, "return of OpenPolicy2, state S208");
            this.Manager.Comment("reaching state \'S277\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp89;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyAccountDomainInformation)\'");
            temp89 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation);
            this.Manager.Comment("reaching state \'S346\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp89, "return of SetInformationPolicy2, state S346");
            TestScenarioS3S415();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S20() {
            this.Manager.BeginTest("TestScenarioS3S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S148\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp90;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp91;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp91 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp90);
            this.Manager.Comment("reaching state \'S217\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp90, "policyHandle of OpenPolicy2, state S217");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp91, "return of OpenPolicy2, state S217");
            this.Manager.Comment("reaching state \'S286\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp92;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyModificationInformation)\'");
            temp92 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyModificationInformation);
            this.Manager.Comment("reaching state \'S355\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp92, "return of SetInformationPolicy2, state S355");
            TestScenarioS3S415();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S22() {
            this.Manager.BeginTest("TestScenarioS3S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S149\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp93;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp94;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp94 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp93);
            this.Manager.Comment("reaching state \'S218\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp93, "policyHandle of OpenPolicy2, state S218");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp94, "return of OpenPolicy2, state S218");
            this.Manager.Comment("reaching state \'S287\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp95;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyAuditFullQueryInformation)\'");
            temp95 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullQueryInformation);
            this.Manager.Comment("reaching state \'S356\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp95, "return of SetInformationPolicy2, state S356");
            TestScenarioS3S415();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S24() {
            this.Manager.BeginTest("TestScenarioS3S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S150\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp96;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp97;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp97 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp96);
            this.Manager.Comment("reaching state \'S219\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp96, "policyHandle of OpenPolicy2, state S219");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp97, "return of OpenPolicy2, state S219");
            this.Manager.Comment("reaching state \'S288\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp98;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyAuditFullSetInformation)\'");
            temp98 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullSetInformation);
            this.Manager.Comment("reaching state \'S357\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp98, "return of SetInformationPolicy2, state S357");
            TestScenarioS3S415();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S26() {
            this.Manager.BeginTest("TestScenarioS3S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S151\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp99;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp100;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp100 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp99);
            this.Manager.Comment("reaching state \'S220\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp99, "policyHandle of OpenPolicy2, state S220");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp100, "return of OpenPolicy2, state S220");
            this.Manager.Comment("reaching state \'S289\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp101;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyModificationInformation)\'");
            temp101 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyModificationInformation);
            this.Manager.Comment("reaching state \'S358\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp101, "return of SetInformationPolicy2, state S358");
            TestScenarioS3S415();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S28() {
            this.Manager.BeginTest("TestScenarioS3S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S152\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp102;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp103;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp103 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp102);
            this.Manager.Comment("reaching state \'S221\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp102, "policyHandle of OpenPolicy2, state S221");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp103, "return of OpenPolicy2, state S221");
            this.Manager.Comment("reaching state \'S290\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp104;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyAuditFullQueryInformation)\'");
            temp104 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullQueryInformation);
            this.Manager.Comment("reaching state \'S359\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp104, "return of SetInformationPolicy2, state S359");
            TestScenarioS3S415();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S30() {
            this.Manager.BeginTest("TestScenarioS3S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S153\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp105;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp106;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp106 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp105);
            this.Manager.Comment("reaching state \'S222\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp105, "policyHandle of OpenPolicy2, state S222");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp106, "return of OpenPolicy2, state S222");
            this.Manager.Comment("reaching state \'S291\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp107;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyAuditFullSetInformation)\'");
            temp107 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullSetInformation);
            this.Manager.Comment("reaching state \'S360\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp107, "return of SetInformationPolicy2, state S360");
            TestScenarioS3S415();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S32() {
            this.Manager.BeginTest("TestScenarioS3S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S154\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp108;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp109;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp109 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp108);
            this.Manager.Comment("reaching state \'S223\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp108, "policyHandle of OpenPolicy2, state S223");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp109, "return of OpenPolicy2, state S223");
            this.Manager.Comment("reaching state \'S292\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp110;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyAccountDomainInformation)\'");
            temp110 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation);
            this.Manager.Comment("reaching state \'S361\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp110, "return of SetInformationPolicy2, state S361");
            TestScenarioS3S415();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S34() {
            this.Manager.BeginTest("TestScenarioS3S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S155\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp111;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp112;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp112 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp111);
            this.Manager.Comment("reaching state \'S224\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp111, "policyHandle of OpenPolicy2, state S224");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp112, "return of OpenPolicy2, state S224");
            this.Manager.Comment("reaching state \'S293\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp113;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyAuditEventsInformation)\'");
            temp113 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditEventsInformation);
            this.Manager.Comment("reaching state \'S362\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp113, "return of SetInformationPolicy2, state S362");
            TestScenarioS3S415();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S36() {
            this.Manager.BeginTest("TestScenarioS3S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S156\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp114;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp115;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp115 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp114);
            this.Manager.Comment("reaching state \'S225\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp114, "policyHandle of OpenPolicy2, state S225");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp115, "return of OpenPolicy2, state S225");
            this.Manager.Comment("reaching state \'S294\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp116;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyPrimaryDomainInformation)\'");
            temp116 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPrimaryDomainInformation);
            this.Manager.Comment("reaching state \'S363\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp116, "return of SetInformationPolicy2, state S363");
            TestScenarioS3S415();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S38() {
            this.Manager.BeginTest("TestScenarioS3S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S157\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp117;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp118;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp118 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp117);
            this.Manager.Comment("reaching state \'S226\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp117, "policyHandle of OpenPolicy2, state S226");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp118, "return of OpenPolicy2, state S226");
            this.Manager.Comment("reaching state \'S295\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp119;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyDnsDomainInformation)\'");
            temp119 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformation);
            this.Manager.Comment("reaching state \'S364\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp119, "return of SetInformationPolicy2, state S364");
            TestScenarioS3S415();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S4() {
            this.Manager.BeginTest("TestScenarioS3S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S140\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp120;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp121;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp121 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp120);
            this.Manager.Comment("reaching state \'S209\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp120, "policyHandle of OpenPolicy2, state S209");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp121, "return of OpenPolicy2, state S209");
            this.Manager.Comment("reaching state \'S278\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp122;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyReplicaSourceInformation)\'");
            temp122 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyReplicaSourceInformation);
            this.Manager.Comment("reaching state \'S347\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp122, "return of SetInformationPolicy2, state S347");
            this.Manager.Comment("reaching state \'S416\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp123;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp124;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp124 = this.ILsadManagedAdapterInstance.Close(1, out temp123);
            this.Manager.AddReturn(CloseInfo, null, temp123, temp124);
            TestScenarioS3S430();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS3S430() {
            this.Manager.Comment("reaching state \'S430\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS3.CloseInfo, null, new CloseDelegate1(this.TestScenarioS3S4CloseChecker)));
            this.Manager.Comment("reaching state \'S437\'");
        }
        
        private void TestScenarioS3S4CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S430");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S430");
        }
        #endregion
        
        #region Test Starting in S40
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S40() {
            this.Manager.BeginTest("TestScenarioS3S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S158\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp125;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp126;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp126 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp125);
            this.Manager.Comment("reaching state \'S227\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp125, "policyHandle of OpenPolicy2, state S227");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp126, "return of OpenPolicy2, state S227");
            this.Manager.Comment("reaching state \'S296\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp127;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyLsaServerRoleInformation)\'");
            temp127 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLsaServerRoleInformation);
            this.Manager.Comment("reaching state \'S365\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp127, "return of SetInformationPolicy2, state S365");
            TestScenarioS3S415();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S42
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S42() {
            this.Manager.BeginTest("TestScenarioS3S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S159\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp128;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp129;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp129 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp128);
            this.Manager.Comment("reaching state \'S228\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp128, "policyHandle of OpenPolicy2, state S228");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp129, "return of OpenPolicy2, state S228");
            this.Manager.Comment("reaching state \'S297\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp130;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyReplicaSourceInformation)\'");
            temp130 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyReplicaSourceInformation);
            this.Manager.Comment("reaching state \'S366\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp130, "return of SetInformationPolicy2, state S366");
            TestScenarioS3S415();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S44
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S44() {
            this.Manager.BeginTest("TestScenarioS3S44");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S160\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp131;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp132;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp132 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp131);
            this.Manager.Comment("reaching state \'S229\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp131, "policyHandle of OpenPolicy2, state S229");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp132, "return of OpenPolicy2, state S229");
            this.Manager.Comment("reaching state \'S298\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp133;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyDnsDomainInformationInt)\'");
            temp133 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformationInt);
            this.Manager.Comment("reaching state \'S367\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp133, "return of SetInformationPolicy2, state S367");
            TestScenarioS3S415();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S46
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S46() {
            this.Manager.BeginTest("TestScenarioS3S46");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S161\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp134;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp135;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp135 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp134);
            this.Manager.Comment("reaching state \'S230\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp134, "policyHandle of OpenPolicy2, state S230");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp135, "return of OpenPolicy2, state S230");
            this.Manager.Comment("reaching state \'S299\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp136;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyAccountDomainInformation)\'");
            temp136 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation);
            this.Manager.Comment("reaching state \'S368\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp136, "return of SetInformationPolicy2, state S368");
            TestScenarioS3S422();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S48
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S48() {
            this.Manager.BeginTest("TestScenarioS3S48");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S162\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp137;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp138;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp138 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp137);
            this.Manager.Comment("reaching state \'S231\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp137, "policyHandle of OpenPolicy2, state S231");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp138, "return of OpenPolicy2, state S231");
            this.Manager.Comment("reaching state \'S300\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp139;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyReplicaSourceInformation)\'");
            temp139 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyReplicaSourceInformation);
            this.Manager.Comment("reaching state \'S369\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp139, "return of SetInformationPolicy2, state S369");
            this.Manager.Comment("reaching state \'S423\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp140;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp141;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp141 = this.ILsadManagedAdapterInstance.Close(1, out temp140);
            this.Manager.AddReturn(CloseInfo, null, temp140, temp141);
            TestScenarioS3S430();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S50
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S50() {
            this.Manager.BeginTest("TestScenarioS3S50");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S163\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp142;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp143;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp143 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp142);
            this.Manager.Comment("reaching state \'S232\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp142, "policyHandle of OpenPolicy2, state S232");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp143, "return of OpenPolicy2, state S232");
            this.Manager.Comment("reaching state \'S301\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp144;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyDnsDomainInformation)\'");
            temp144 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformation);
            this.Manager.Comment("reaching state \'S370\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp144, "return of SetInformationPolicy2, state S370");
            this.Manager.Comment("reaching state \'S424\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp145;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp146;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp146 = this.ILsadManagedAdapterInstance.Close(1, out temp145);
            this.Manager.AddReturn(CloseInfo, null, temp145, temp146);
            TestScenarioS3S431();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS3S431() {
            this.Manager.Comment("reaching state \'S431\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS3.CloseInfo, null, new CloseDelegate1(this.TestScenarioS3S50CloseChecker)));
            this.Manager.Comment("reaching state \'S438\'");
        }
        
        private void TestScenarioS3S50CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S431");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S431");
        }
        #endregion
        
        #region Test Starting in S52
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S52() {
            this.Manager.BeginTest("TestScenarioS3S52");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S164\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp147;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp148;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp148 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp147);
            this.Manager.Comment("reaching state \'S233\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp147, "policyHandle of OpenPolicy2, state S233");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp148, "return of OpenPolicy2, state S233");
            this.Manager.Comment("reaching state \'S302\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp149;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyLsaServerRoleInformation)\'");
            temp149 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLsaServerRoleInformation);
            this.Manager.Comment("reaching state \'S371\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp149, "return of SetInformationPolicy2, state S371");
            this.Manager.Comment("reaching state \'S425\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp150;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp151;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp151 = this.ILsadManagedAdapterInstance.Close(1, out temp150);
            this.Manager.AddReturn(CloseInfo, null, temp150, temp151);
            TestScenarioS3S432();
            this.Manager.EndTest();
        }
        
        private void TestScenarioS3S432() {
            this.Manager.Comment("reaching state \'S432\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TestScenarioS3.CloseInfo, null, new CloseDelegate1(this.TestScenarioS3S52CloseChecker)));
            this.Manager.Comment("reaching state \'S439\'");
        }
        
        private void TestScenarioS3S52CloseChecker(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle handleAfterClose, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus @return) {
            this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, handleAfterClose, "handleAfterClose of Close, state S432");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), @return, "return of Close, state S432");
        }
        #endregion
        
        #region Test Starting in S54
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S54() {
            this.Manager.BeginTest("TestScenarioS3S54");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S165\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp152;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp153;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp153 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp152);
            this.Manager.Comment("reaching state \'S234\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp152, "policyHandle of OpenPolicy2, state S234");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp153, "return of OpenPolicy2, state S234");
            this.Manager.Comment("reaching state \'S303\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp154;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyPrimaryDomainInformation)\'");
            temp154 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPrimaryDomainInformation);
            this.Manager.Comment("reaching state \'S372\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp154, "return of SetInformationPolicy2, state S372");
            this.Manager.Comment("reaching state \'S426\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp155;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp156;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp156 = this.ILsadManagedAdapterInstance.Close(1, out temp155);
            this.Manager.AddReturn(CloseInfo, null, temp155, temp156);
            TestScenarioS3S433();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S56
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S56() {
            this.Manager.BeginTest("TestScenarioS3S56");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S166\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp157;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp158;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp158 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp157);
            this.Manager.Comment("reaching state \'S235\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp157, "policyHandle of OpenPolicy2, state S235");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp158, "return of OpenPolicy2, state S235");
            this.Manager.Comment("reaching state \'S304\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp159;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyAuditEventsInformation)\'");
            temp159 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditEventsInformation);
            this.Manager.Comment("reaching state \'S373\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp159, "return of SetInformationPolicy2, state S373");
            this.Manager.Comment("reaching state \'S427\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp160;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp161;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp161 = this.ILsadManagedAdapterInstance.Close(1, out temp160);
            this.Manager.AddReturn(CloseInfo, null, temp160, temp161);
            TestScenarioS3S434();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S58
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S58() {
            this.Manager.BeginTest("TestScenarioS3S58");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S167\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp162;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp163;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp163 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp162);
            this.Manager.Comment("reaching state \'S236\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp162, "policyHandle of OpenPolicy2, state S236");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp163, "return of OpenPolicy2, state S236");
            this.Manager.Comment("reaching state \'S305\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp164;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyDnsDomainInformationInt)\'");
            temp164 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformationInt);
            this.Manager.Comment("reaching state \'S374\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp164, "return of SetInformationPolicy2, state S374");
            this.Manager.Comment("reaching state \'S428\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp165;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp166;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp166 = this.ILsadManagedAdapterInstance.Close(1, out temp165);
            this.Manager.AddReturn(CloseInfo, null, temp165, temp166);
            TestScenarioS3S435();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S6() {
            this.Manager.BeginTest("TestScenarioS3S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S141\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp167;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp168;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp168 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp167);
            this.Manager.Comment("reaching state \'S210\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp167, "policyHandle of OpenPolicy2, state S210");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp168, "return of OpenPolicy2, state S210");
            this.Manager.Comment("reaching state \'S279\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp169;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyDnsDomainInformation)\'");
            temp169 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformation);
            this.Manager.Comment("reaching state \'S348\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp169, "return of SetInformationPolicy2, state S348");
            this.Manager.Comment("reaching state \'S417\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp170;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp171;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp171 = this.ILsadManagedAdapterInstance.Close(1, out temp170);
            this.Manager.AddReturn(CloseInfo, null, temp170, temp171);
            TestScenarioS3S431();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S60
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S60() {
            this.Manager.BeginTest("TestScenarioS3S60");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S168\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp172;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp173;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp173 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp172);
            this.Manager.Comment("reaching state \'S237\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp172, "policyHandle of OpenPolicy2, state S237");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp173, "return of OpenPolicy2, state S237");
            this.Manager.Comment("reaching state \'S306\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp174;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyPdAccountInformation)\'");
            temp174 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPdAccountInformation);
            this.Manager.Comment("reaching state \'S375\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp174, "return of SetInformationPolicy2, state S375");
            TestScenarioS3S422();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S62
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S62() {
            this.Manager.BeginTest("TestScenarioS3S62");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S169\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp175;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp176;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp176 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp175);
            this.Manager.Comment("reaching state \'S238\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp175, "policyHandle of OpenPolicy2, state S238");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp176, "return of OpenPolicy2, state S238");
            this.Manager.Comment("reaching state \'S307\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp177;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyPdAccountInformation)\'");
            temp177 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPdAccountInformation);
            this.Manager.Comment("reaching state \'S376\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp177, "return of SetInformationPolicy2, state S376");
            TestScenarioS3S422();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S64
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S64() {
            this.Manager.BeginTest("TestScenarioS3S64");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S170\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp178;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp179;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp179 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp178);
            this.Manager.Comment("reaching state \'S239\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp178, "policyHandle of OpenPolicy2, state S239");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp179, "return of OpenPolicy2, state S239");
            this.Manager.Comment("reaching state \'S308\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp180;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyModificationInformation)\'");
            temp180 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyModificationInformation);
            this.Manager.Comment("reaching state \'S377\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp180, "return of SetInformationPolicy2, state S377");
            TestScenarioS3S422();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S66
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S66() {
            this.Manager.BeginTest("TestScenarioS3S66");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S171\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp181;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp182;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp182 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp181);
            this.Manager.Comment("reaching state \'S240\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp181, "policyHandle of OpenPolicy2, state S240");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp182, "return of OpenPolicy2, state S240");
            this.Manager.Comment("reaching state \'S309\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp183;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyAuditFullQueryInformation)\'");
            temp183 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullQueryInformation);
            this.Manager.Comment("reaching state \'S378\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp183, "return of SetInformationPolicy2, state S378");
            TestScenarioS3S422();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S68
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S68() {
            this.Manager.BeginTest("TestScenarioS3S68");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S172\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp184;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp185;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp185 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp184);
            this.Manager.Comment("reaching state \'S241\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp184, "policyHandle of OpenPolicy2, state S241");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp185, "return of OpenPolicy2, state S241");
            this.Manager.Comment("reaching state \'S310\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp186;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyAuditFullSetInformation)\'");
            temp186 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullSetInformation);
            this.Manager.Comment("reaching state \'S379\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp186, "return of SetInformationPolicy2, state S379");
            TestScenarioS3S422();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S70
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S70() {
            this.Manager.BeginTest("TestScenarioS3S70");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S173\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp187;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp188;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp188 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp187);
            this.Manager.Comment("reaching state \'S242\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp187, "policyHandle of OpenPolicy2, state S242");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp188, "return of OpenPolicy2, state S242");
            this.Manager.Comment("reaching state \'S311\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp189;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyModificationInformation)\'");
            temp189 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyModificationInformation);
            this.Manager.Comment("reaching state \'S380\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp189, "return of SetInformationPolicy2, state S380");
            TestScenarioS3S422();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S72
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S72() {
            this.Manager.BeginTest("TestScenarioS3S72");
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S174\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp190;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp191;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp191 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp190);
            this.Manager.Comment("reaching state \'S243\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp190, "policyHandle of OpenPolicy2, state S243");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp191, "return of OpenPolicy2, state S243");
            this.Manager.Comment("reaching state \'S312\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp192;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyAuditFullQueryInformation)\'");
            temp192 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullQueryInformation);
            this.Manager.Comment("reaching state \'S381\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp192, "return of SetInformationPolicy2, state S381");
            TestScenarioS3S422();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S74
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S74() {
            this.Manager.BeginTest("TestScenarioS3S74");
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S175\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp193;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp194;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp194 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp193);
            this.Manager.Comment("reaching state \'S244\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp193, "policyHandle of OpenPolicy2, state S244");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp194, "return of OpenPolicy2, state S244");
            this.Manager.Comment("reaching state \'S313\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp195;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyAuditFullSetInformation)\'");
            temp195 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditFullSetInformation);
            this.Manager.Comment("reaching state \'S382\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp195, "return of SetInformationPolicy2, state S382");
            TestScenarioS3S422();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S76
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S76() {
            this.Manager.BeginTest("TestScenarioS3S76");
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S176\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp196;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp197;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp197 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp196);
            this.Manager.Comment("reaching state \'S245\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp196, "policyHandle of OpenPolicy2, state S245");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp197, "return of OpenPolicy2, state S245");
            this.Manager.Comment("reaching state \'S314\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp198;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyAccountDomainInformation)\'");
            temp198 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAccountDomainInformation);
            this.Manager.Comment("reaching state \'S383\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidParameter\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidParameter, temp198, "return of SetInformationPolicy2, state S383");
            TestScenarioS3S422();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S78
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S78() {
            this.Manager.BeginTest("TestScenarioS3S78");
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S177\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp199;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp200;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp200 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp199);
            this.Manager.Comment("reaching state \'S246\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp199, "policyHandle of OpenPolicy2, state S246");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp200, "return of OpenPolicy2, state S246");
            this.Manager.Comment("reaching state \'S315\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp201;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyAuditEventsInformation)\'");
            temp201 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditEventsInformation);
            this.Manager.Comment("reaching state \'S384\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp201, "return of SetInformationPolicy2, state S384");
            TestScenarioS3S422();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S8() {
            this.Manager.BeginTest("TestScenarioS3S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S142\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp202;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp203;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1935,out _)\'");
            temp203 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1935u, out temp202);
            this.Manager.Comment("reaching state \'S211\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp202, "policyHandle of OpenPolicy2, state S211");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp203, "return of OpenPolicy2, state S211");
            this.Manager.Comment("reaching state \'S280\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp204;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyLsaServerRoleInformation)\'");
            temp204 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLsaServerRoleInformation);
            this.Manager.Comment("reaching state \'S349\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp204, "return of SetInformationPolicy2, state S349");
            this.Manager.Comment("reaching state \'S418\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp205;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp206;
            this.Manager.Comment("executing step \'call Close(1,out _)\'");
            temp206 = this.ILsadManagedAdapterInstance.Close(1, out temp205);
            this.Manager.AddReturn(CloseInfo, null, temp205, temp206);
            TestScenarioS3S432();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S80
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S80() {
            this.Manager.BeginTest("TestScenarioS3S80");
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S178\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp207;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp208;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp208 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp207);
            this.Manager.Comment("reaching state \'S247\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp207, "policyHandle of OpenPolicy2, state S247");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp208, "return of OpenPolicy2, state S247");
            this.Manager.Comment("reaching state \'S316\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp209;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyPrimaryDomainInformation)\'");
            temp209 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPrimaryDomainInformation);
            this.Manager.Comment("reaching state \'S385\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp209, "return of SetInformationPolicy2, state S385");
            TestScenarioS3S422();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S82
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S82() {
            this.Manager.BeginTest("TestScenarioS3S82");
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S179\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp210;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp211;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp211 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp210);
            this.Manager.Comment("reaching state \'S248\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp210, "policyHandle of OpenPolicy2, state S248");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp211, "return of OpenPolicy2, state S248");
            this.Manager.Comment("reaching state \'S317\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp212;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyDnsDomainInformation)\'");
            temp212 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformation);
            this.Manager.Comment("reaching state \'S386\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp212, "return of SetInformationPolicy2, state S386");
            TestScenarioS3S422();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S84
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S84() {
            this.Manager.BeginTest("TestScenarioS3S84");
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S180\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp213;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp214;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp214 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp213);
            this.Manager.Comment("reaching state \'S249\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp213, "policyHandle of OpenPolicy2, state S249");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp214, "return of OpenPolicy2, state S249");
            this.Manager.Comment("reaching state \'S318\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp215;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyLsaServerRoleInformation)\'");
            temp215 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLsaServerRoleInformation);
            this.Manager.Comment("reaching state \'S387\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp215, "return of SetInformationPolicy2, state S387");
            TestScenarioS3S422();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S86
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S86() {
            this.Manager.BeginTest("TestScenarioS3S86");
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S181\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp216;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp217;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp217 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp216);
            this.Manager.Comment("reaching state \'S250\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp216, "policyHandle of OpenPolicy2, state S250");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp217, "return of OpenPolicy2, state S250");
            this.Manager.Comment("reaching state \'S319\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp218;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyReplicaSourceInformation)\'");
            temp218 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyReplicaSourceInformation);
            this.Manager.Comment("reaching state \'S388\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp218, "return of SetInformationPolicy2, state S388");
            TestScenarioS3S422();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S88
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S88() {
            this.Manager.BeginTest("TestScenarioS3S88");
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S182\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp219;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp220;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,1928,out _)\'");
            temp220 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 1928u, out temp219);
            this.Manager.Comment("reaching state \'S251\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp219, "policyHandle of OpenPolicy2, state S251");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp220, "return of OpenPolicy2, state S251");
            this.Manager.Comment("reaching state \'S320\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp221;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(53,PolicyDnsDomainInformationInt)\'");
            temp221 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(53, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyDnsDomainInformationInt);
            this.Manager.Comment("reaching state \'S389\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/InvalidHandle\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.InvalidHandle, temp221, "return of SetInformationPolicy2, state S389");
            TestScenarioS3S422();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S90
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S90() {
            this.Manager.BeginTest("TestScenarioS3S90");
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S183\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp222;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp223;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp223 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp222);
            this.Manager.Comment("reaching state \'S252\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp222, "policyHandle of OpenPolicy2, state S252");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp223, "return of OpenPolicy2, state S252");
            this.Manager.Comment("reaching state \'S321\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp224;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyAuditLogInformation)\'");
            temp224 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass)(1)));
            this.Manager.Comment("reaching state \'S390\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp224, "return of SetInformationPolicy2, state S390");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S92
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S92() {
            this.Manager.BeginTest("TestScenarioS3S92");
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S184\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp225;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp226;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp226 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp225);
            this.Manager.Comment("reaching state \'S253\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp225, "policyHandle of OpenPolicy2, state S253");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp226, "return of OpenPolicy2, state S253");
            this.Manager.Comment("reaching state \'S322\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp227;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyLsaServerRoleInformation)\'");
            temp227 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyLsaServerRoleInformation);
            this.Manager.Comment("reaching state \'S391\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp227, "return of SetInformationPolicy2, state S391");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S94
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S94() {
            this.Manager.BeginTest("TestScenarioS3S94");
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S185\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp228;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp229;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp229 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp228);
            this.Manager.Comment("reaching state \'S254\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp228, "policyHandle of OpenPolicy2, state S254");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp229, "return of OpenPolicy2, state S254");
            this.Manager.Comment("reaching state \'S323\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp230;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyPrimaryDomainInformation)\'");
            temp230 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyPrimaryDomainInformation);
            this.Manager.Comment("reaching state \'S392\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp230, "return of SetInformationPolicy2, state S392");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S96
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S96() {
            this.Manager.BeginTest("TestScenarioS3S96");
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S186\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp231;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp232;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp232 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp231);
            this.Manager.Comment("reaching state \'S255\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp231, "policyHandle of OpenPolicy2, state S255");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp232, "return of OpenPolicy2, state S255");
            this.Manager.Comment("reaching state \'S324\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp233;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyReplicaSourceInformation)\'");
            temp233 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyReplicaSourceInformation);
            this.Manager.Comment("reaching state \'S393\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp233, "return of SetInformationPolicy2, state S393");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
        
        #region Test Starting in S98
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-LSAD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        public void LSAD_TestScenarioS3S98() {
            this.Manager.BeginTest("TestScenarioS3S98");
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
            this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S187\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp234;
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp235;
            this.Manager.Comment("executing step \'call OpenPolicy2(Null,7,out _)\'");
            temp235 = this.ILsadManagedAdapterInstance.OpenPolicy2(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 7u, out temp234);
            this.Manager.Comment("reaching state \'S256\'");
            this.Manager.Comment("checking step \'return OpenPolicy2/[out Valid]:Success\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp234, "policyHandle of OpenPolicy2, state S256");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp235, "return of OpenPolicy2, state S256");
            this.Manager.Comment("reaching state \'S325\'");
            Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp236;
            this.Manager.Comment("executing step \'call SetInformationPolicy2(1,PolicyAuditEventsInformation)\'");
            temp236 = this.ILsadManagedAdapterInstance.SetInformationPolicy2(1, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.InformationClass.PolicyAuditEventsInformation);
            this.Manager.Comment("reaching state \'S394\'");
            this.Manager.Comment("checking step \'return SetInformationPolicy2/AccessDenied\'");
            TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.AccessDenied, temp236, "return of SetInformationPolicy2, state S394");
            TestScenarioS3S414();
            this.Manager.EndTest();
        }
        #endregion
    }
}
