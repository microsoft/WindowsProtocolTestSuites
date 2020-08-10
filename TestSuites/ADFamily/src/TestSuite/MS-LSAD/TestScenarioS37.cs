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
    public partial class TestScenarioS37 : PtfTestClassBase {
        
        public TestScenarioS37() {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "10000");
        }
        
        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadManagedAdapter ILsadManagedAdapterInstance;
        
        private Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadSutControlAdapter ISUTControlAdapterInstance;
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
            this.ISUTControlAdapterInstance = ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadSutControlAdapter)(this.Manager.GetAdapter(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ILsadSutControlAdapter))));
        }
        
        protected override void TestCleanup() {
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion
        
        //#region Test Starting in S0
        //[TestCategory("PDC")]
        //[TestCategory("DomainWin2008R2")]
        //[TestCategory("ForestWin2008R2")]
        //[TestCategory("MS-LSAD")]

        //[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        //public void LSAD_TestScenarioS37S0()
        //{
        //    this.Manager.BeginTest("TestScenarioS37S0");
        //    this.Manager.Comment("reaching state \'S0\'");
        //    this.Manager.Comment("executing step \'call Initialize(DomainController,Disable,Windows2k8,2,True)\'");
        //    this.ILsadManagedAdapterInstance.Initialize(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ProtocolServerConfig)(0)), ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.AnonymousAccess)(0)), Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Server.Windows2k8, 2, true);
        //    this.Manager.Comment("reaching state \'S1\'");
        //    this.Manager.Comment("checking step \'return Initialize\'");
        //    this.Manager.Comment("reaching state \'S2\'");
        //    Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp0;
        //    Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp1;
        //    this.Manager.Comment("executing step \'call OpenPolicy(Null,3507,out _)\'");
        //    temp1 = this.ILsadManagedAdapterInstance.OpenPolicy(((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.RootDirectory)(0)), 3507u, out temp0);
        //    this.Manager.Comment("reaching state \'S3\'");
        //    this.Manager.Comment("checking step \'return OpenPolicy/[out Valid]:Success\'");
        //    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle)(0)), temp0, "policyHandle of OpenPolicy, state S3");
        //    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp1, "return of OpenPolicy, state S3");
        //    this.Manager.Comment("reaching state \'S4\'");
        //    bool temp2;
        //    this.Manager.Comment("executing step \'call StopDirectoryService()\'");
        //    temp2 = this.ISUTControlAdapterInstance.StopDirectoryService();
        //    this.Manager.Comment("reaching state \'S5\'");
        //    this.Manager.Comment("checking step \'return StopDirectoryService/True\'");
        //    TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp2, "return of StopDirectoryService, state S5");
        //    this.Manager.Comment("reaching state \'S6\'");
        //    Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle temp3;
        //    Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp4;
        //    this.Manager.Comment("executing step \'call CreateSecret(1,\"SecretName\",46,out _)\'");
        //    temp4 = this.ILsadManagedAdapterInstance.CreateSecret(1, "SecretName", 46u, out temp3);
        //    this.Manager.Comment("reaching state \'S7\'");
        //    this.Manager.Comment("checking step \'return CreateSecret/[out Invalid]:DirectoryServiceRequired\'");
        //    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.SecretHandle)(1)), temp3, "secretHandle of CreateSecret, state S7");
        //    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus.DirectoryServiceRequired, temp4, "return of CreateSecret, state S7");
        //    this.Manager.Comment("reaching state \'S8\'");
        //    bool temp5;
        //    this.Manager.Comment("executing step \'call StartDirectoryService()\'");
        //    temp5 = this.ISUTControlAdapterInstance.StartDirectoryService();
        //    this.Manager.Comment("reaching state \'S9\'");
        //    this.Manager.Comment("checking step \'return StartDirectoryService/True\'");
        //    TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp5, "return of StartDirectoryService, state S9");
        //    this.Manager.Comment("reaching state \'S10\'");
        //    Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle temp6;
        //    Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus temp7;
        //    this.Manager.Comment("executing step \'call Close(1,out _)\'");
        //    temp7 = this.ILsadManagedAdapterInstance.Close(1, out temp6);
        //    this.Manager.Comment("reaching state \'S11\'");
        //    this.Manager.Comment("checking step \'return Close/[out Null]:Success\'");
        //    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle>(this.Manager, Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.Handle.Null, temp6, "handleAfterClose of Close, state S11");
        //    TestManagerHelpers.AssertAreEqual<Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus>(this.Manager, ((Microsoft.Protocols.TestSuites.ActiveDirectory.Lsad.ErrorStatus)(0)), temp7, "return of Close, state S11");
        //    this.Manager.Comment("reaching state \'S12\'");
        //    this.Manager.EndTest();
        //}
        //#endregion
    }
}
