// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Ldap
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Microsoft.SpecExplorer.Runtime.Testing;
    using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
    using Microsoft.Protocols.TestTools;
    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [System.CodeDom.Compiler.GeneratedCodeAttribute("Spec Explorer", "3.5.3146.0")]
    [TestClassAttribute()]
    public partial class TestScenarioExtendedOperationsWin2K8R2 : PtfTestClassBase
    {

        public TestScenarioExtendedOperationsWin2K8R2()
        {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "30000");
        }

        #region Adapter Instances
        private IAD_LDAPModelAdapter IAD_LDAPModelAdapterInstance;
        #endregion

        #region Class Initialization and Cleanup
        [ClassInitializeAttribute()]
        public static void ClassInitialize(TestContext context)
        {
            PtfTestClassBase.Initialize(context);
        }

        [ClassCleanupAttribute()]
        public static void ClassCleanup()
        {
            PtfTestClassBase.Cleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            this.InitializeTestManager();
            this.IAD_LDAPModelAdapterInstance = ((IAD_LDAPModelAdapter)(this.Manager.GetAdapter(typeof(IAD_LDAPModelAdapter))));
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion

        #region Test Starting in S0
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("LDS")]
        public void LDAP_TestScenarioExtendedOperationsWin2K8R2S0()
        {
            this.Manager.BeginTest("TestScenarioExtendedOperationsWin2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S8\'");
            ExtendedOperationResponse temp0;
            this.Manager.Comment("executing step \'call LDAPExtendedOperation(LDAP_TTL_REFRESH_OID,Windows2K8R2,AD_L" +
                    "DS,out _)\'");
            this.IAD_LDAPModelAdapterInstance.LDAPExtendedOperation(
                ExtendedOperation.LDAP_TTL_REFRESH_OID,
                ServerVersion.Win2008R2,
                ((ADImplementations)(1)), out temp0);
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("checking step \'return LDAPExtendedOperation/[out Valid]\'");
            TestManagerHelpers.AssertAreEqual<ExtendedOperationResponse>(this.Manager, ((ExtendedOperationResponse)(0)), temp0, "response of LDAPExtendedOperation, state S12");
            TestScenarioExtendedOperationsWin2K8R2S16();
            this.Manager.EndTest();
        }

        private void TestScenarioExtendedOperationsWin2K8R2S16()
        {
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S18\'");
        }
        #endregion

        #region Test Starting in S2
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioExtendedOperationsWin2K8R2S2()
        {
            this.Manager.BeginTest("TestScenarioExtendedOperationsWin2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S9\'");
            ExtendedOperationResponse temp1;
            this.Manager.Comment("executing step \'call LDAPExtendedOperation(LDAP_TTL_REFRESH_OID,Windows2K8R2,AD_D" +
                    "S,out _)\'");
            this.IAD_LDAPModelAdapterInstance.LDAPExtendedOperation(
                ExtendedOperation.LDAP_TTL_REFRESH_OID,
                ServerVersion.Win2008R2,
                ((ADImplementations)(0)),
                out temp1);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return LDAPExtendedOperation/[out Valid]\'");
            TestManagerHelpers.AssertAreEqual<ExtendedOperationResponse>(this.Manager, ((ExtendedOperationResponse)(0)), temp1, "response of LDAPExtendedOperation, state S13");
            TestScenarioExtendedOperationsWin2K8R2S16();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioExtendedOperationsWin2K8R2S4()
        {
            this.Manager.BeginTest("TestScenarioExtendedOperationsWin2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S10\'");
            ExtendedOperationResponse temp2;
            this.Manager.Comment("executing step \'call LDAPExtendedOperation(LDAP_SERVER_WHO_AM_I_OID,Windows2K8R2," +
                    "AD_DS,out _)\'");
            this.IAD_LDAPModelAdapterInstance.LDAPExtendedOperation(
                ExtendedOperation.LDAP_SERVER_WHO_AM_I_OID,
                ServerVersion.Win2008R2,
                ((ADImplementations)(0)),
                out temp2);
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("checking step \'return LDAPExtendedOperation/[out Valid]\'");
            TestManagerHelpers.AssertAreEqual<ExtendedOperationResponse>(this.Manager, ((ExtendedOperationResponse)(0)), temp2, "response of LDAPExtendedOperation, state S14");
            TestScenarioExtendedOperationsWin2K8R2S16();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("LDS")]
        public void LDAP_TestScenarioExtendedOperationsWin2K8R2S6()
        {
            this.Manager.BeginTest("TestScenarioExtendedOperationsWin2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S11\'");
            ExtendedOperationResponse temp3;
            this.Manager.Comment("executing step \'call LDAPExtendedOperation(LDAP_SERVER_WHO_AM_I_OID,Windows2K8R2," +
                    "AD_LDS,out _)\'");
            this.IAD_LDAPModelAdapterInstance.LDAPExtendedOperation(
                ExtendedOperation.LDAP_SERVER_WHO_AM_I_OID,
                ServerVersion.Win2008R2,
                ((ADImplementations)(1)),
                out temp3);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return LDAPExtendedOperation/[out Valid]\'");
            TestManagerHelpers.AssertAreEqual<ExtendedOperationResponse>(this.Manager, ((ExtendedOperationResponse)(0)), temp3, "response of LDAPExtendedOperation, state S15");
            TestScenarioExtendedOperationsWin2K8R2S16();
            this.Manager.EndTest();
        }
        #endregion
    }
}
