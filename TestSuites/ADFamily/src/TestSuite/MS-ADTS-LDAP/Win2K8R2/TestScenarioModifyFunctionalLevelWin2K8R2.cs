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
    public partial class TestScenarioModifyFunctionalLevelWin2K8R2 : PtfTestClassBase
    {

        public TestScenarioModifyFunctionalLevelWin2K8R2()
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
        public void LDAP_TestScenarioModifyFunctionalLevelWin2K8R2S0()
        {
            this.Manager.BeginTest("TestScenarioModifyFunctionalLevelWin2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S18\'");
            ConstrOnModOpErrs temp0;
            this.Manager.Comment("executing step \'call ModifyOperation({\"msDS-Behavior-Version: 4\"->[\"distinguished" +
                    "Name: CN=RODC,DC=writableDC\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_P" +
                    "RIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "msDS-Behavior-Version: 4", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: CN=RODC,DC=writableDC",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp0);
            this.Manager.Checkpoint("MS-AD_LDAP_R4343");
            this.Manager.Checkpoint("MS-AD_LDAP_R4341");
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            if (EnvironmentConfig.ServerVer < ServerVersion.Win2012)

                TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp0, "errorStatus of ModifyOperation, state S27");
            else
                TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION, temp0, "errorStatus of ModifyOperation, state S27");
            TestScenarioModifyFunctionalLevelWin2K8R2S36();
            this.Manager.EndTest();
        }

        private void TestScenarioModifyFunctionalLevelWin2K8R2S36()
        {
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S40\'");
        }
        #endregion

        #region Test Starting in S10
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("CDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioModifyFunctionalLevelWin2K8R2S10()
        {
            this.Manager.BeginTest("TestScenarioModifyFunctionalLevelWin2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S23\'");
            ConstrOnModOpErrs temp1;
            this.Manager.Comment("executing step \'call ModifyOperation({\"msDS-Behavior-Version: 4\"->[\"distinguished" +
                    "Name: CN=RODC,DC=WritableDCNotSameDomain\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE" +
                    "_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "msDS-Behavior-Version: 4", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: CN=RODC,DC=WritableDCNotSameDomain",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp1);
            this.Manager.Checkpoint("MS-AD_LDAP_R4343");
            this.Manager.Checkpoint("MS-AD_LDAP_R4342");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out UnwillingToPerform_ERROR_DS_ILLEGAL_MO" +
                    "D_OPERATION]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION, temp1, "errorStatus of ModifyOperation, state S32");
            TestScenarioModifyFunctionalLevelWin2K8R2S37();
            this.Manager.EndTest();
        }

        private void TestScenarioModifyFunctionalLevelWin2K8R2S37()
        {
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S41\'");
        }
        #endregion

        #region Test Starting in S12
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("SDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioModifyFunctionalLevelWin2K8R2S12()
        {
            this.Manager.BeginTest("TestScenarioModifyFunctionalLevelWin2K8R2S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S24\'");
            ConstrOnModOpErrs temp2;
            this.Manager.Comment("executing step \'call ModifyOperation({\"msDS-Behavior-Version: 4\"->[\"distinguished" +
                    "Name: CN=Partitions,CN=Configuration,DC=NotPDCFSMO\"]},RIGHT_DS_WRITE_PROPERTYwit" +
                    "hSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _" +
                    ")\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "msDS-Behavior-Version: 4", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: CN=Partitions,CN=Configuration,DC=NotPDCFSMO",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp2);
            this.Manager.Checkpoint("MS-AD_LDAP_R4343");
            this.Manager.Checkpoint("MS-AD_LDAP_R4344");
            this.Manager.Checkpoint("MS-AD_LDAP_R4352");
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out referral_ERROR_DS_REFERRAL]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.referral_ERROR_DS_REFERRAL, temp2, "errorStatus of ModifyOperation, state S33");
            TestScenarioModifyFunctionalLevelWin2K8R2S37();
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
        public void LDAP_TestScenarioModifyFunctionalLevelWin2K8R2S14()
        {
            this.Manager.BeginTest("TestScenarioModifyFunctionalLevelWin2K8R2S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S25\'");
            ConstrOnModOpErrs temp3;
            this.Manager.Comment("executing step \'call ModifyOperation({\"msDS-Behavior-Version: 5\"->[\"distinguished" +
                    "Name: DC=adts88\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoE" +
                    "xtendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "msDS-Behavior-Version: 5", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: DC=adts88",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp3);
            this.Manager.Checkpoint("MS-AD_LDAP_R4343");
            this.Manager.Checkpoint("MS-AD_LDAP_R4339");
            this.Manager.Checkpoint("MS-AD_LDAP_R4348");
            this.Manager.Checkpoint("MS-AD_LDAP_R4354");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out UnwillingToPerform_ERROR_DS_LOW_DSA_VE" +
                    "RSION]\'");
            if (EnvironmentConfig.ServerVer < ServerVersion.Win2012)
            {
                TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_LOW_DSA_VERSION, temp3, "errorStatus of ModifyOperation, state S34");
            }
            else if (EnvironmentConfig.ServerVer == ServerVersion.Win2012)
            {
                TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp3, "error status should be success");
            }
            else if (EnvironmentConfig.ServerVer > ServerVersion.Win2012)
            {
                TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_HIGH_DSA_VERSION, temp3, "errorStatus of ModifyOperation, state S34");
            }

            TestScenarioModifyFunctionalLevelWin2K8R2S37();
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
        public void LDAP_TestScenarioModifyFunctionalLevelWin2K8R2S16()
        {
            this.Manager.BeginTest("TestScenarioModifyFunctionalLevelWin2K8R2S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S26\'");
            ConstrOnModOpErrs temp4;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""msDS-Behavior-Version: 0""->[""instanceType: 4"",""objectCategory: CN=Cross-Ref-Container,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: top;crossRefContainer"",""distinguishedName: CN=Partitions,CN=Configuration,DC=adts88"",""msDS-Behavior-Version: 2""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "msDS-Behavior-Version: 0", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "instanceType: 4",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "objectCategory: CN=Cross-Ref-Container,CN=Schema,CN=Configuration,DC=adts88",
                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                            "Head",
                                                                                            "Tail"}, new object[] {
                                                                                            "objectClass: top;crossRefContainer",
                                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                        "Head",
                                                                                                        "Tail"}, new object[] {
                                                                                                        "distinguishedName: CN=Partitions,CN=Configuration,DC=adts88",
                                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                    "Head",
                                                                                                                    "Tail"}, new object[] {
                                                                                                                    "msDS-Behavior-Version: 2",
                                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp4);
            this.Manager.Checkpoint("MS-AD_LDAP_R4343");
            this.Manager.Checkpoint("MS-AD_LDAP_R4339");
            this.Manager.Checkpoint("MS-AD_LDAP_R710");
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out UnwillingToPerform_ERROR_DS_ILLEGAL_MO" +
                    "D_OPERATION]\'");
            if (EnvironmentConfig.ServerVer >= ServerVersion.Win2012)
            {
                TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_HIGH_DSA_VERSION, temp4, "errorStatus of ModifyOperation, state S35");

            }
            else
                TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION, temp4, "errorStatus of ModifyOperation, state S35");
            TestScenarioModifyFunctionalLevelWin2K8R2S37();
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
        public void LDAP_TestScenarioModifyFunctionalLevelWin2K8R2S2()
        {
            this.Manager.BeginTest("TestScenarioModifyFunctionalLevelWin2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S19\'");
            ConstrOnModOpErrs temp5;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""msDS-Behavior-Version: 1""->[""instanceType: 5"",""objectCategory: CN=Domain-DNS,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: domainDNS"",""distinguishedName: DC=adts88"",""msDS-Behavior-Version: 2""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "msDS-Behavior-Version: 1", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "instanceType: 5",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "objectCategory: CN=Domain-DNS,CN=Schema,CN=Configuration,DC=adts88",
                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                            "Head",
                                                                                            "Tail"}, new object[] {
                                                                                            "objectClass: domainDNS",
                                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                        "Head",
                                                                                                        "Tail"}, new object[] {
                                                                                                        "distinguishedName: DC=adts88",
                                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                    "Head",
                                                                                                                    "Tail"}, new object[] {
                                                                                                                    "msDS-Behavior-Version: 2",
                                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp5);
            this.Manager.Checkpoint("MS-AD_LDAP_R4343");
            this.Manager.Checkpoint("MS-AD_LDAP_R4339");
            this.Manager.Checkpoint("MS-AD_LDAP_R4346");
            this.Manager.Checkpoint("MS-AD_LDAP_R4358");
            this.Manager.Checkpoint("MS-AD_LDAP_R4602");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out UnwillingToPerform_ERROR_DS_ILLEGAL_MO" +
                    "D_OPERATION]\'");
            if (EnvironmentConfig.ServerVer < ServerVersion.Win2012)
            {
                TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION, temp5, "errorStatus of ModifyOperation, state S28");
            }
            else
                TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_HIGH_DSA_VERSION, temp5, "errorStatus of ModifyOperation, state S28");
            TestScenarioModifyFunctionalLevelWin2K8R2S37();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("RODC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioModifyFunctionalLevelWin2K8R2S4()
        {
            this.Manager.BeginTest("TestScenarioModifyFunctionalLevelWin2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S20\'");
            ConstrOnModOpErrs temp6;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""msDS-Behavior-Version: 2""->[""instanceType: 4"",""objectCategory: CN=NTDS-DSA,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: top;applicationSettings;nTDSDSA"",""distinguishedName: CN=NTDS Settings,CN=WIN-6IEHBFZ8AMV,CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,DC=adts88"",""msDS-Behavior-Version: 3""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "msDS-Behavior-Version: 2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "instanceType: 4",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "objectCategory: CN=NTDS-DSA,CN=Schema,CN=Configuration,DC=adts88",
                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                            "Head",
                                                                                            "Tail"}, new object[] {
                                                                                            "objectClass: top;applicationSettings;nTDSDSA",
                                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                        "Head",
                                                                                                        "Tail"}, new object[] {
                                                                                                        "distinguishedName: CN=NTDS Settings,CN=WIN-6IEHBFZ8AMV,CN=Servers,CN=Default-Firs" +
                                                                                                            "t-Site-Name,CN=Sites,CN=Configuration,DC=adts88",
                                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                    "Head",
                                                                                                                    "Tail"}, new object[] {
                                                                                                                    "msDS-Behavior-Version: 3",
                                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp6);
            this.Manager.Checkpoint("MS-AD_LDAP_R4343");
            this.Manager.Checkpoint("MS-AD_LDAP_R4339");
            this.Manager.Checkpoint("MS-AD_LDAP_R710");
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out UnwillingToPerform_ERROR_DS_ILLEGAL_MO" +
                    "D_OPERATION]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION, temp6, "errorStatus of ModifyOperation, state S29");
            TestScenarioModifyFunctionalLevelWin2K8R2S37();
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
        public void LDAP_TestScenarioModifyFunctionalLevelWin2K8R2S6()
        {
            this.Manager.BeginTest("TestScenarioModifyFunctionalLevelWin2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S21\'");
            ConstrOnModOpErrs temp7;
            this.Manager.Comment("executing step \'call ModifyOperation({\"msDS-Behavior-Version: 4\"->[\"distinguished" +
                    "Name: CN=Users,DC=adts88\"]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIV" +
                    "ILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "msDS-Behavior-Version: 4", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: CN=Users,DC=adts88",
                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp7);
            this.Manager.Checkpoint("MS-AD_LDAP_R4343");
            this.Manager.Checkpoint("MS-AD_LDAP_R4340");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out UnwillingToPerform_ERROR_DS_ILLEGAL_MO" +
                    "D_OPERATION]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION, temp7, "errorStatus of ModifyOperation, state S30");
            TestScenarioModifyFunctionalLevelWin2K8R2S37();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("RODC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioModifyFunctionalLevelWin2K8R2S8()
        {
            this.Manager.BeginTest("TestScenarioModifyFunctionalLevelWin2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S22\'");
            ConstrOnModOpErrs temp8;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""msDS-Behavior-Version: 4""->[""instanceType: 5"",""objectCategory: CN=NTDS-DSA,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: top;applicationSettings;nTDSDSA"",""distinguishedName: CN=NTDS Settings,CN=WIN-6IEHBFZ8AMV,CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,DC=adts88"",""msDS-Behavior-Version: 2""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "msDS-Behavior-Version: 4", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "instanceType: 5",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "objectCategory: CN=NTDS-DSA,CN=Schema,CN=Configuration,DC=adts88",
                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                            "Head",
                                                                                            "Tail"}, new object[] {
                                                                                            "objectClass: top;applicationSettings;nTDSDSA",
                                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                        "Head",
                                                                                                        "Tail"}, new object[] {
                                                                                                        "distinguishedName: CN=NTDS Settings,CN=WIN-6IEHBFZ8AMV,CN=Servers,CN=Default-Firs" +
                                                                                                            "t-Site-Name,CN=Sites,CN=Configuration,DC=adts88",
                                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                    "Head",
                                                                                                                    "Tail"}, new object[] {
                                                                                                                    "msDS-Behavior-Version: 2",
                                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp8);
            this.Manager.Checkpoint("MS-AD_LDAP_R4343");
            this.Manager.Checkpoint("MS-AD_LDAP_R4339");
            this.Manager.Checkpoint("MS-AD_LDAP_R4601");
            this.Manager.Checkpoint("MS-AD_LDAP_R710");
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            if (EnvironmentConfig.ServerVer <= ServerVersion.Win2008R2)
                TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp8, "errorStatus of ModifyOperation, state S31");
            else
                TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.UnwillingToPerform_ERROR_DS_ILLEGAL_MOD_OPERATION, temp8, "errorStatus of ModifyOperation, state S31");

            TestScenarioModifyFunctionalLevelWin2K8R2S36();
            this.Manager.EndTest();
        }
        #endregion
    }
}
