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
    public partial class TestScenarioRootDSEModifyWin2K8R2 : PtfTestClassBase
    {

        public TestScenarioRootDSEModifyWin2K8R2()
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
        [TestCategory("RODC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S0()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S48\'");
            ConstrOnModOpErrs temp0;
            this.Manager.Comment("executing step \'call ModifyOperation({\"rODCPurgeAccount:CN=adts_user10,CN=Users,DC" +
                    "=+adts88\"->[\"distinguishedName: null\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DEL" +
                    "EGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,True,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "rODCPurgeAccount:CN=adts_user10,CN=Users,DC=+adts88", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, true, out temp0);
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out OperationsError_ERROR_DS_OBJ_NOT_FOUND" +
                    "]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.OperationsError_ERROR_DS_OBJ_NOT_FOUND, temp0, "errorStatus of ModifyOperation, state S72");
            TestScenarioRootDSEModifyWin2K8R2S96();
            this.Manager.EndTest();
        }

        private void TestScenarioRootDSEModifyWin2K8R2S96()
        {
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S100\'");
        }
        #endregion

        #region Test Starting in S10
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S10()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S53\'");
            ConstrOnModOpErrs temp1;
            this.Manager.Comment("executing step \'call ModifyOperation({\"recalcHierarchy:1\"->[\"distinguishedName: n" +
                    "ull\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedContr" +
                    "ol,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "recalcHierarchy:1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp1);
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp1, "errorStatus of ModifyOperation, state S77");
            TestScenarioRootDSEModifyWin2K8R2S97();
            this.Manager.EndTest();
        }

        private void TestScenarioRootDSEModifyWin2K8R2S97()
        {
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S101\'");
        }
        #endregion

        #region Test Starting in S12
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("RODC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S12()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S54\'");
            ConstrOnModOpErrs temp2;
            this.Manager.Comment("executing step \'call ModifyOperation({\"rODCPurgeAccount:CN=adts_user10,CN=Users,DC" +
                    "=adts88\"->[\"distinguishedName: null\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELE" +
                    "GATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "rODCPurgeAccount:CN=adts_user10,CN=Users,DC=adts88", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp2);
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out OperationsError_ERROR_DS_GENERIC_ERROR" +
                    "]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.OperationsError_ERROR_DS_GENERIC_ERROR, temp2, "errorStatus of ModifyOperation, state S78");
            TestScenarioRootDSEModifyWin2K8R2S96();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S14
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S14()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S55\'");
            ConstrOnModOpErrs temp3;
            this.Manager.Comment("executing step \'call ModifyOperation({\"becomeInfrastructureMaster:1\"->[\"distingui" +
                    "shedName: null\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoEx" +
                    "tendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "becomeInfrastructureMaster:1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp3);
            this.Manager.Checkpoint("MS-AD_LDAP_R4200");
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp3, "errorStatus of ModifyOperation, state S79");
            TestScenarioRootDSEModifyWin2K8R2S97();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S16
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S16()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S56\'");
            ConstrOnModOpErrs temp4;
            this.Manager.Comment("executing step \'call ModifyOperation({\"currentTime:1\"->[\"distinguishedName: null\"" +
                    "]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,A" +
                    "D_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "currentTime:1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp4);
            this.Manager.Checkpoint("MS-AD_LDAP_R209");
            this.Manager.Checkpoint("MS-AD_LDAP_R1089");
            this.Manager.Checkpoint("MS-AD_LDAP_R1090");
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out UnwillingToPerform_UnKnownError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ((ConstrOnModOpErrs)(1)), temp4, "errorStatus of ModifyOperation, state S80");
            TestScenarioRootDSEModifyWin2K8R2S96();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S18
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S18()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S57\'");
            ConstrOnModOpErrs temp5;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""replicateSingleObject:CN=Configuration,DC=FAKELDAP,DC=com:CN=one,CN=adts_user1,CN=Users,DC=adts88""->[""distinguishedName: null""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "replicateSingleObject:CN=Configuration,DC=FAKELDAP,DC=com:CN=one,CN=adts_user1,CN=Use" +
                                    "rs,DC=adts88", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp5);
            this.Manager.Checkpoint("MS-AD_LDAP_R4207");
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out OperationsError_ERROR_DS_OBJ_NOT_FOUND" +
                    "]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.OperationsError_ERROR_DS_OBJ_NOT_FOUND, temp5, "errorStatus of ModifyOperation, state S81");
            TestScenarioRootDSEModifyWin2K8R2S96();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S2()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S49\'");
            ConstrOnModOpErrs temp6;
            this.Manager.Comment("executing step \'call ModifyOperation({\"becomePdcWithCheckPoint:1\"->[\"distinguishe" +
                    "dName: null\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExten" +
                    "dedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "becomePdcWithCheckPoint:1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp6);
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp6, "errorStatus of ModifyOperation, state S73");
            TestScenarioRootDSEModifyWin2K8R2S97();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S20
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S20()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S58\'");
            ConstrOnModOpErrs temp7;
            this.Manager.Comment("executing step \'call ModifyOperation({\"doLinkCleanup:1\"->[\"distinguishedName: nul" +
                    "l\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl" +
                    ",AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "doLinkCleanup:1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp7);
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp7, "errorStatus of ModifyOperation, state S82");
            TestScenarioRootDSEModifyWin2K8R2S97();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S22
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S22()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S59\'");
            ConstrOnModOpErrs temp8;
            this.Manager.Comment("executing step \'call ModifyOperation({\"doOnlineDefrag:60\"->[\"distinguishedName: n" +
                    "ull\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedContr" +
                    "ol,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "doOnlineDefrag:60", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp8);
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp8, "errorStatus of ModifyOperation, state S83");
            TestScenarioRootDSEModifyWin2K8R2S97();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S24
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S24()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S60\'");
            ConstrOnModOpErrs temp9;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""replicateSingleObject:CN=user6750,CN=Users,DC=adts88:CN=one,CN=adts_user1,CN=Users,DC=adts88""->[""distinguishedName: null""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "replicateSingleObject:CN=user6750,CN=Users,DC=adts88:CN=one,CN=adts_user1,CN=User" +
                                    "s,DC=adts88", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp9);
            this.Manager.Checkpoint("MS-AD_LDAP_R4205");
            this.Manager.Checkpoint("MS-AD_LDAP_R4206");
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out OperationsError_ERROR_DS_OBJ_NOT_FOUND" +
                    "]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.OperationsError_ERROR_DS_OBJ_NOT_FOUND, temp9, "errorStatus of ModifyOperation, state S84");
            TestScenarioRootDSEModifyWin2K8R2S96();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S26
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
        [TestCategory("SDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S26()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S61\'");
            ConstrOnModOpErrs temp10;
            this.Manager.Comment("executing step \'call ModifyOperation({\"runProtectAdminGroupsTask:NotPDCFSMOOwner\"" +
                    "->[\"distinguishedName: null\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_P" +
                    "RIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "runProtectAdminGroupsTask:NotPDCFSMOOwner", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp10);
            this.Manager.Checkpoint("MS-AD_LDAP_R1004210");
            this.Manager.Checkpoint("MS-AD_LDAP_R4193");
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out Success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp10, "errorStatus of ModifyOperation, state S85");
            TestScenarioRootDSEModifyWin2K8R2S96();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S28
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S28()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S62\'");
            ConstrOnModOpErrs temp11;
            this.Manager.Comment("executing step \'call ModifyOperation({\"becomePdc:1\"->[\"distinguishedName: null\"]}" +
                    ",RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_" +
                    "DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "becomePdc:1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp11);
            this.Manager.Checkpoint("MS-AD_LDAP_R4201");
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp11, "errorStatus of ModifyOperation, state S86");
            TestScenarioRootDSEModifyWin2K8R2S97();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S30
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S30()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S63\'");
            ConstrOnModOpErrs temp12;
            this.Manager.Comment("executing step \'call ModifyOperation({\"updateCachedMemberships:1\"->[\"distinguishe" +
                    "dName: null\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExten" +
                    "dedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "updateCachedMemberships:1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp12);
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp12, "errorStatus of ModifyOperation, state S87");
            TestScenarioRootDSEModifyWin2K8R2S97();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S32
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S32()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S64\'");
            ConstrOnModOpErrs temp13;
            this.Manager.Comment("executing step \'call ModifyOperation({\"becomeSchemaMaster:1\"->[\"distinguishedName" +
                    ": null\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedCo" +
                    "ntrol,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "becomeSchemaMaster:1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp13);
            this.Manager.Checkpoint("MS-AD_LDAP_R4203");
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp13, "errorStatus of ModifyOperation, state S88");
            TestScenarioRootDSEModifyWin2K8R2S97();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S34
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S34()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S65\'");
            ConstrOnModOpErrs temp14;
            this.Manager.Comment("executing step \'call ModifyOperation({\"fixupInheritance:1\"->[\"distinguishedName: " +
                    "null\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedCont" +
                    "rol,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "fixupInheritance:1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp14);
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp14, "errorStatus of ModifyOperation, state S89");
            TestScenarioRootDSEModifyWin2K8R2S97();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S36
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S36()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S66\'");
            ConstrOnModOpErrs temp15;
            this.Manager.Comment("executing step \'call ModifyOperation({\"doGarbageCollectionPhantomsNow:1\"->[\"disti" +
                    "nguishedName: null\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE," +
                    "NoExtendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "doGarbageCollectionPhantomsNow:1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp15);
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp15, "errorStatus of ModifyOperation, state S90");
            TestScenarioRootDSEModifyWin2K8R2S97();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S38
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S38()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S67\'");
            ConstrOnModOpErrs temp16;
            this.Manager.Comment("executing step \'call ModifyOperation({\"defaultNamingContext:1\"->[\"distinguishedNa" +
                    "me: null\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtended" +
                    "Control,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "defaultNamingContext:1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp16);
            this.Manager.Checkpoint("MS-AD_LDAP_R209");
            this.Manager.Checkpoint("MS-AD_LDAP_R1089");
            this.Manager.Checkpoint("MS-AD_LDAP_R1090");
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out UnwillingToPerform_UnKnownError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ((ConstrOnModOpErrs)(1)), temp16, "errorStatus of ModifyOperation, state S91");
            TestScenarioRootDSEModifyWin2K8R2S96();
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
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S4()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S50\'");
            ConstrOnModOpErrs temp17;
            this.Manager.Comment("executing step \'call ModifyOperation({\"doGarbageCollection:1\"->[\"distinguishedNam" +
                    "e: null\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedC" +
                    "ontrol,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "doGarbageCollection:1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp17);
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp17, "errorStatus of ModifyOperation, state S74");
            TestScenarioRootDSEModifyWin2K8R2S97();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S40
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S40()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S68\'");
            ConstrOnModOpErrs temp18;
            this.Manager.Comment("executing step \'call ModifyOperation({\"schemaUpdateNow:1\"->[\"distinguishedName: n" +
                    "ull\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedContr" +
                    "ol,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "schemaUpdateNow:1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp18);
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp18, "errorStatus of ModifyOperation, state S92");
            TestScenarioRootDSEModifyWin2K8R2S97();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S42
        // [MS-ADTS] section 3.1.1.3.3.11 invalidateRidPool
        // This invalidateRidPool operation will break environment if succeeded.
        // And it will take a while before the rid pool is refreshed.
        // Therefore, change this case to a negative case against RODC with return error unwillingToPerform / ERROR_INVALID_PARAMETER.
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("RODC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S42()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S69\'");
            ConstrOnModOpErrs temp19;
            this.Manager.Comment("executing step \'call ModifyOperation({\"invalidateRidPool:1\"->[\"distinguishedName:" +
                    " null\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedCon" +
                    "trol,AD_DS,Windows2K8R2,True,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "invalidateRidPool:1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, true, out temp19);
            this.Manager.Checkpoint("MS-AD_LDAP_R4470");
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out UnwillingToPerform_ERROR_INVALID_PARAMETER]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.UnwillingToPerform_ERROR_INVALID_PARAMETER, temp19, "errorStatus of ModifyOperation, state S93");
            TestScenarioRootDSEModifyWin2K8R2S97();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S44
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S44()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S44");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S70\'");
            ConstrOnModOpErrs temp20;
            this.Manager.Comment("executing step \'call ModifyOperation({\"becomeDomainMaster:1\"->[\"distinguishedName" +
                    ": null\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedCo" +
                    "ntrol,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "becomeDomainMaster:1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp20);
            this.Manager.Checkpoint("MS-AD_LDAP_R4199");
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp20, "errorStatus of ModifyOperation, state S94");
            TestScenarioRootDSEModifyWin2K8R2S97();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S46
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("RODC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S46()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S46");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S71\'");
            ConstrOnModOpErrs temp21;
            this.Manager.Comment("executing step \'call ModifyOperation({\"rODCPurgeAccount:CN=adts_user10,CN=Users,DC" +
                    "=adts88\"->[\"distinguishedName: null\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELE" +
                    "GATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,True,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "rODCPurgeAccount:CN=adts_user10,CN=Users,DC=adts88", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, true, out temp21);
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp21, "errorStatus of ModifyOperation, state S95");
            TestScenarioRootDSEModifyWin2K8R2S97();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S6()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S51\'");
            ConstrOnModOpErrs temp22;
            this.Manager.Comment("executing step \'call ModifyOperation({\"checkPhantoms:1\"->[\"distinguishedName: nul" +
                    "l\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl" +
                    ",AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "checkPhantoms:1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp22);
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp22, "errorStatus of ModifyOperation, state S75");
            TestScenarioRootDSEModifyWin2K8R2S97();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("Main")]
        public void LDAP_TestScenarioRootDSEModifyWin2K8R2S8()
        {
            this.Manager.BeginTest("TestScenarioRootDSEModifyWin2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S52\'");
            ConstrOnModOpErrs temp23;
            this.Manager.Comment("executing step \'call ModifyOperation({\"becomeRidMaster:1\"->[\"distinguishedName: n" +
                    "ull\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedContr" +
                    "ol,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "becomeRidMaster:1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: null",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp23);
            this.Manager.Checkpoint("MS-AD_LDAP_R4202");
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp23, "errorStatus of ModifyOperation, state S76");
            TestScenarioRootDSEModifyWin2K8R2S97();
            this.Manager.EndTest();
        }
        #endregion
    }
}
