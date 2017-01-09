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
    public partial class TestScenarioModifyAD_LDSWin2K8R2 : PtfTestClassBase
    {

        public TestScenarioModifyAD_LDSWin2K8R2()
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
        public void LDAP_TestScenarioModifyAD_LDSWin2K8R2S0()
        {
            this.Manager.BeginTest("TestScenarioModifyAD_LDSWin2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S12\'");
            ConstrOnAddOpErrs temp0;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: ModifyTestUser"",""objectClass: user"",""distinguishedName: CN=ModifyTestUser,CN=ApplicationNamingContext,DC=adts88"",""userPrincipalName: ModifyTestUser@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: ModifyTestUser",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=ModifyTestUser,CN=ApplicationNamingContext,DC=adts88",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: ModifyTestUser@adts",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp0);
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R553");
            this.Manager.Checkpoint("MS-AD_LDAP_R555");
            this.Manager.Checkpoint("MS-AD_LDAP_R557");
            this.Manager.Checkpoint("MS-AD_LDAP_R559");
            this.Manager.Checkpoint("MS-AD_LDAP_R561");
            this.Manager.Checkpoint("MS-AD_LDAP_R582");
            this.Manager.Checkpoint("MS-AD_LDAP_R584");
            this.Manager.Checkpoint("MS-AD_LDAP_R33");
            this.Manager.Checkpoint("MS-AD_LDAP_R569");
            this.Manager.Checkpoint("MS-AD_LDAP_R567");
            this.Manager.Checkpoint("MS-AD_LDAP_R571");
            this.Manager.Checkpoint("MS-AD_LDAP_R544");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp0, "errorStatus of AddOperation, state S18");
            this.Manager.Comment("reaching state \'S24\'");
            ConstrOnAddOpErrs temp1;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: ModifyTestUser1"",""objectClass: user"",""distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88"",""userPrincipalName: ModifyTestUser1@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: ModifyTestUser1",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: ModifyTestUser1@adts",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp1);
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R553");
            this.Manager.Checkpoint("MS-AD_LDAP_R555");
            this.Manager.Checkpoint("MS-AD_LDAP_R557");
            this.Manager.Checkpoint("MS-AD_LDAP_R559");
            this.Manager.Checkpoint("MS-AD_LDAP_R561");
            this.Manager.Checkpoint("MS-AD_LDAP_R582");
            this.Manager.Checkpoint("MS-AD_LDAP_R584");
            this.Manager.Checkpoint("MS-AD_LDAP_R33");
            this.Manager.Checkpoint("MS-AD_LDAP_R569");
            this.Manager.Checkpoint("MS-AD_LDAP_R567");
            this.Manager.Checkpoint("MS-AD_LDAP_R571");
            this.Manager.Checkpoint("MS-AD_LDAP_R544");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp1, "errorStatus of AddOperation, state S30");
            this.Manager.Comment("reaching state \'S36\'");
            ConstrOnModOpErrs temp2;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""userPrincipalName: ModifyTestUser2@adts""->[""objectClass: user"",""distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_LDS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "userPrincipalName: ModifyTestUser2@adts", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "objectClass: user",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88",
                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(1)), ServerVersion.Win2008R2, false, out temp2);
            this.Manager.Checkpoint("MS-AD_LDAP_R729");
            this.Manager.Checkpoint("MS-AD_LDAP_R729");
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp2, "errorStatus of ModifyOperation, state S42");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S10
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("LDS")]
        public void LDAP_TestScenarioModifyAD_LDSWin2K8R2S10()
        {
            this.Manager.BeginTest("TestScenarioModifyAD_LDSWin2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S17\'");
            ConstrOnAddOpErrs temp3;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: ModifyTestUser"",""objectClass: user"",""distinguishedName: CN=ModifyTestUser,CN=ApplicationNamingContext,DC=adts88"",""userPrincipalName: ModifyTestUser@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: ModifyTestUser",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=ModifyTestUser,CN=ApplicationNamingContext,DC=adts88",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: ModifyTestUser@adts",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp3);
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R553");
            this.Manager.Checkpoint("MS-AD_LDAP_R555");
            this.Manager.Checkpoint("MS-AD_LDAP_R557");
            this.Manager.Checkpoint("MS-AD_LDAP_R559");
            this.Manager.Checkpoint("MS-AD_LDAP_R561");
            this.Manager.Checkpoint("MS-AD_LDAP_R582");
            this.Manager.Checkpoint("MS-AD_LDAP_R584");
            this.Manager.Checkpoint("MS-AD_LDAP_R33");
            this.Manager.Checkpoint("MS-AD_LDAP_R569");
            this.Manager.Checkpoint("MS-AD_LDAP_R567");
            this.Manager.Checkpoint("MS-AD_LDAP_R571");
            this.Manager.Checkpoint("MS-AD_LDAP_R544");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp3, "errorStatus of AddOperation, state S23");
            this.Manager.Comment("reaching state \'S29\'");
            ConstrOnAddOpErrs temp4;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: ModifyTestUser1"",""objectClass: user"",""distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88"",""userPrincipalName: ModifyTestUser1@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: ModifyTestUser1",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: ModifyTestUser1@adts",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp4);
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R553");
            this.Manager.Checkpoint("MS-AD_LDAP_R555");
            this.Manager.Checkpoint("MS-AD_LDAP_R557");
            this.Manager.Checkpoint("MS-AD_LDAP_R559");
            this.Manager.Checkpoint("MS-AD_LDAP_R561");
            this.Manager.Checkpoint("MS-AD_LDAP_R582");
            this.Manager.Checkpoint("MS-AD_LDAP_R584");
            this.Manager.Checkpoint("MS-AD_LDAP_R33");
            this.Manager.Checkpoint("MS-AD_LDAP_R569");
            this.Manager.Checkpoint("MS-AD_LDAP_R567");
            this.Manager.Checkpoint("MS-AD_LDAP_R571");
            this.Manager.Checkpoint("MS-AD_LDAP_R544");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp4, "errorStatus of AddOperation, state S35");
            this.Manager.Comment("reaching state \'S41\'");
            ConstrOnModOpErrs temp5;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""lockoutTime: 0""->[""objectClass: user"",""distinguishedName: CN=ModifyTestUser,CN=ApplicationNamingContext,DC=adts88""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_LDS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "lockoutTime: 0", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "objectClass: user",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "distinguishedName: CN=ModifyTestUser,CN=ApplicationNamingContext,DC=adts88",
                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(1)), ServerVersion.Win2008R2, false, out temp5);
            this.Manager.Checkpoint("MS-AD_LDAP_R729");
            this.Manager.Checkpoint("MS-AD_LDAP_R729");
            this.Manager.Checkpoint("MS-AD_LDAP_R733");
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp5, "errorStatus of ModifyOperation, state S47");
            TestScenarioModifyAD_LDSWin2K8R2S50();
            this.Manager.EndTest();
        }

        private void TestScenarioModifyAD_LDSWin2K8R2S50()
        {
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S56\'");
        }
        #endregion

        #region Test Starting in S2
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("LDS")]
        public void LDAP_TestScenarioModifyAD_LDSWin2K8R2S2()
        {
            this.Manager.BeginTest("TestScenarioModifyAD_LDSWin2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S13\'");
            ConstrOnAddOpErrs temp6;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: ModifyTestUser"",""objectClass: user"",""distinguishedName: CN=ModifyTestUser,CN=ApplicationNamingContext,DC=adts88"",""userPrincipalName: ModifyTestUser@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: ModifyTestUser",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=ModifyTestUser,CN=ApplicationNamingContext,DC=adts88",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: ModifyTestUser@adts",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp6);
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R553");
            this.Manager.Checkpoint("MS-AD_LDAP_R555");
            this.Manager.Checkpoint("MS-AD_LDAP_R557");
            this.Manager.Checkpoint("MS-AD_LDAP_R559");
            this.Manager.Checkpoint("MS-AD_LDAP_R561");
            this.Manager.Checkpoint("MS-AD_LDAP_R582");
            this.Manager.Checkpoint("MS-AD_LDAP_R584");
            this.Manager.Checkpoint("MS-AD_LDAP_R33");
            this.Manager.Checkpoint("MS-AD_LDAP_R569");
            this.Manager.Checkpoint("MS-AD_LDAP_R567");
            this.Manager.Checkpoint("MS-AD_LDAP_R571");
            this.Manager.Checkpoint("MS-AD_LDAP_R544");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp6, "errorStatus of AddOperation, state S19");
            this.Manager.Comment("reaching state \'S25\'");
            ConstrOnAddOpErrs temp7;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: ModifyTestUser1"",""objectClass: user"",""distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88"",""userPrincipalName: ModifyTestUser1@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: ModifyTestUser1",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: ModifyTestUser1@adts",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp7);
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R553");
            this.Manager.Checkpoint("MS-AD_LDAP_R555");
            this.Manager.Checkpoint("MS-AD_LDAP_R557");
            this.Manager.Checkpoint("MS-AD_LDAP_R559");
            this.Manager.Checkpoint("MS-AD_LDAP_R561");
            this.Manager.Checkpoint("MS-AD_LDAP_R582");
            this.Manager.Checkpoint("MS-AD_LDAP_R584");
            this.Manager.Checkpoint("MS-AD_LDAP_R33");
            this.Manager.Checkpoint("MS-AD_LDAP_R569");
            this.Manager.Checkpoint("MS-AD_LDAP_R567");
            this.Manager.Checkpoint("MS-AD_LDAP_R571");
            this.Manager.Checkpoint("MS-AD_LDAP_R544");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp7, "errorStatus of AddOperation, state S31");
            this.Manager.Comment("reaching state \'S37\'");
            ConstrOnModOpErrs temp8;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""userPrincipalName: ModifyTestUser@adts""->[""objectClass: user"",""distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_LDS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "userPrincipalName: ModifyTestUser@adts", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "objectClass: user",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88",
                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(1)), ServerVersion.Win2008R2, false, out temp8);
            this.Manager.Checkpoint("MS-AD_LDAP_R729");
            this.Manager.Checkpoint("MS-AD_LDAP_R730");
            this.Manager.Comment("reaching state \'S43\'");
            if (EnvironmentConfig.ServerVer >= ServerVersion.Win2012R2)
            {
                this.Manager.Comment("checking step \'return ModifyOperation/[out ConstraintViolation_ERROR_DS_UPN_VALUE_" +
                    "NOT_UNIQUE_IN_FOREST\'");
                TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_UPN_VALUE_NOT_UNIQUE_IN_FOREST, temp8, "errorStatus of ModifyOperation, If another object exists with a duplicate userPrincipalName value, the operation fails with an extended error of ERROR_DS_UPN_VALUE_NOT_UNIQUE_IN_FOREST.");
            }
            else
            {
                this.Manager.Comment("checking step \'return ModifyOperation/[out ConstraintViolation_ERROR_DS_NAME_NOT_" +
                        "UNIQUE]\'");
                TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.ConstraintViolation_ERROR_DS_NAME_NOT_UNIQUE, temp8, "errorStatus of ModifyOperation, state S43");
            }
            TestScenarioModifyAD_LDSWin2K8R2S49();
            this.Manager.EndTest();
        }

        private void TestScenarioModifyAD_LDSWin2K8R2S49()
        {
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S55\'");
        }
        #endregion

        #region Test Starting in S4
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("LDS")]
        public void LDAP_TestScenarioModifyAD_LDSWin2K8R2S4()
        {
            this.Manager.BeginTest("TestScenarioModifyAD_LDSWin2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S14\'");
            ConstrOnAddOpErrs temp9;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: ModifyTestUser"",""objectClass: user"",""distinguishedName: CN=ModifyTestUser,CN=ApplicationNamingContext,DC=adts88"",""userPrincipalName: ModifyTestUser@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: ModifyTestUser",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=ModifyTestUser,CN=ApplicationNamingContext,DC=adts88",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: ModifyTestUser@adts",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp9);
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R553");
            this.Manager.Checkpoint("MS-AD_LDAP_R555");
            this.Manager.Checkpoint("MS-AD_LDAP_R557");
            this.Manager.Checkpoint("MS-AD_LDAP_R559");
            this.Manager.Checkpoint("MS-AD_LDAP_R561");
            this.Manager.Checkpoint("MS-AD_LDAP_R582");
            this.Manager.Checkpoint("MS-AD_LDAP_R584");
            this.Manager.Checkpoint("MS-AD_LDAP_R33");
            this.Manager.Checkpoint("MS-AD_LDAP_R569");
            this.Manager.Checkpoint("MS-AD_LDAP_R567");
            this.Manager.Checkpoint("MS-AD_LDAP_R571");
            this.Manager.Checkpoint("MS-AD_LDAP_R544");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp9, "errorStatus of AddOperation, state S20");
            this.Manager.Comment("reaching state \'S26\'");
            ConstrOnAddOpErrs temp10;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: ModifyTestUser1"",""objectClass: user"",""distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88"",""userPrincipalName: ModifyTestUser1@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: ModifyTestUser1",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: ModifyTestUser1@adts",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp10);
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R553");
            this.Manager.Checkpoint("MS-AD_LDAP_R555");
            this.Manager.Checkpoint("MS-AD_LDAP_R557");
            this.Manager.Checkpoint("MS-AD_LDAP_R559");
            this.Manager.Checkpoint("MS-AD_LDAP_R561");
            this.Manager.Checkpoint("MS-AD_LDAP_R582");
            this.Manager.Checkpoint("MS-AD_LDAP_R584");
            this.Manager.Checkpoint("MS-AD_LDAP_R33");
            this.Manager.Checkpoint("MS-AD_LDAP_R569");
            this.Manager.Checkpoint("MS-AD_LDAP_R567");
            this.Manager.Checkpoint("MS-AD_LDAP_R571");
            this.Manager.Checkpoint("MS-AD_LDAP_R544");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp10, "errorStatus of AddOperation, state S32");
            this.Manager.Comment("reaching state \'S38\'");
            ConstrOnModOpErrs temp11;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""lockoutTime: 4568""->[""objectClass: user"",""distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_LDS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "lockoutTime: 4568", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "objectClass: user",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88",
                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(1)), ServerVersion.Win2008R2, false, out temp11);
            this.Manager.Checkpoint("MS-AD_LDAP_R729");
            this.Manager.Checkpoint("MS-AD_LDAP_R729");
            this.Manager.Checkpoint("MS-AD_LDAP_R734");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out ConstraintViolation_ERROR_INVALID_PARA" +
                    "METER]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.ConstraintViolation_ERROR_INVALID_PARAMETER, temp11, "errorStatus of ModifyOperation, state S44");
            TestScenarioModifyAD_LDSWin2K8R2S49();
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
        public void LDAP_TestScenarioModifyAD_LDSWin2K8R2S6()
        {
            this.Manager.BeginTest("TestScenarioModifyAD_LDSWin2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S15\'");
            ConstrOnAddOpErrs temp12;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: ModifyTestUser"",""objectClass: user"",""distinguishedName: CN=ModifyTestUser,CN=ApplicationNamingContext,DC=adts88"",""userPrincipalName: ModifyTestUser@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: ModifyTestUser",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=ModifyTestUser,CN=ApplicationNamingContext,DC=adts88",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: ModifyTestUser@adts",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp12);
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R553");
            this.Manager.Checkpoint("MS-AD_LDAP_R555");
            this.Manager.Checkpoint("MS-AD_LDAP_R557");
            this.Manager.Checkpoint("MS-AD_LDAP_R559");
            this.Manager.Checkpoint("MS-AD_LDAP_R561");
            this.Manager.Checkpoint("MS-AD_LDAP_R582");
            this.Manager.Checkpoint("MS-AD_LDAP_R584");
            this.Manager.Checkpoint("MS-AD_LDAP_R33");
            this.Manager.Checkpoint("MS-AD_LDAP_R569");
            this.Manager.Checkpoint("MS-AD_LDAP_R567");
            this.Manager.Checkpoint("MS-AD_LDAP_R571");
            this.Manager.Checkpoint("MS-AD_LDAP_R544");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp12, "errorStatus of AddOperation, state S21");
            this.Manager.Comment("reaching state \'S27\'");
            ConstrOnAddOpErrs temp13;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: ModifyTestUser1"",""objectClass: user"",""distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88"",""userPrincipalName: ModifyTestUser1@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: ModifyTestUser1",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: ModifyTestUser1@adts",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp13);
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R553");
            this.Manager.Checkpoint("MS-AD_LDAP_R555");
            this.Manager.Checkpoint("MS-AD_LDAP_R557");
            this.Manager.Checkpoint("MS-AD_LDAP_R559");
            this.Manager.Checkpoint("MS-AD_LDAP_R561");
            this.Manager.Checkpoint("MS-AD_LDAP_R582");
            this.Manager.Checkpoint("MS-AD_LDAP_R584");
            this.Manager.Checkpoint("MS-AD_LDAP_R33");
            this.Manager.Checkpoint("MS-AD_LDAP_R569");
            this.Manager.Checkpoint("MS-AD_LDAP_R567");
            this.Manager.Checkpoint("MS-AD_LDAP_R571");
            this.Manager.Checkpoint("MS-AD_LDAP_R544");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp13, "errorStatus of AddOperation, state S33");
            this.Manager.Comment("reaching state \'S39\'");
            ConstrOnModOpErrs temp14;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""pwdLastSet: 0""->[""objectClass: user"",""distinguishedName: CN=ModifyTestUser,CN=ApplicationNamingContext,DC=adts88""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_LDS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "pwdLastSet: 0", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "objectClass: user",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "distinguishedName: CN=ModifyTestUser,CN=ApplicationNamingContext,DC=adts88",
                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(1)), ServerVersion.Win2008R2, false, out temp14);
            this.Manager.Checkpoint("MS-AD_LDAP_R729");
            this.Manager.Checkpoint("MS-AD_LDAP_R729");
            this.Manager.Checkpoint("MS-AD_LDAP_R731");
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp14, "errorStatus of ModifyOperation, state S45");
            TestScenarioModifyAD_LDSWin2K8R2S50();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("LDS")]
        public void LDAP_TestScenarioModifyAD_LDSWin2K8R2S8()
        {
            this.Manager.BeginTest("TestScenarioModifyAD_LDSWin2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S16\'");
            ConstrOnAddOpErrs temp15;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: ModifyTestUser"",""objectClass: user"",""distinguishedName: CN=ModifyTestUser,CN=ApplicationNamingContext,DC=adts88"",""userPrincipalName: ModifyTestUser@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: ModifyTestUser",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=ModifyTestUser,CN=ApplicationNamingContext,DC=adts88",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: ModifyTestUser@adts",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp15);
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R553");
            this.Manager.Checkpoint("MS-AD_LDAP_R555");
            this.Manager.Checkpoint("MS-AD_LDAP_R557");
            this.Manager.Checkpoint("MS-AD_LDAP_R559");
            this.Manager.Checkpoint("MS-AD_LDAP_R561");
            this.Manager.Checkpoint("MS-AD_LDAP_R582");
            this.Manager.Checkpoint("MS-AD_LDAP_R584");
            this.Manager.Checkpoint("MS-AD_LDAP_R33");
            this.Manager.Checkpoint("MS-AD_LDAP_R569");
            this.Manager.Checkpoint("MS-AD_LDAP_R567");
            this.Manager.Checkpoint("MS-AD_LDAP_R571");
            this.Manager.Checkpoint("MS-AD_LDAP_R544");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp15, "errorStatus of AddOperation, state S22");
            this.Manager.Comment("reaching state \'S28\'");
            ConstrOnAddOpErrs temp16;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: ModifyTestUser1"",""objectClass: user"",""distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88"",""userPrincipalName: ModifyTestUser1@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: ModifyTestUser1",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: ModifyTestUser1@adts",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp16);
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R553");
            this.Manager.Checkpoint("MS-AD_LDAP_R555");
            this.Manager.Checkpoint("MS-AD_LDAP_R557");
            this.Manager.Checkpoint("MS-AD_LDAP_R559");
            this.Manager.Checkpoint("MS-AD_LDAP_R561");
            this.Manager.Checkpoint("MS-AD_LDAP_R582");
            this.Manager.Checkpoint("MS-AD_LDAP_R584");
            this.Manager.Checkpoint("MS-AD_LDAP_R33");
            this.Manager.Checkpoint("MS-AD_LDAP_R569");
            this.Manager.Checkpoint("MS-AD_LDAP_R567");
            this.Manager.Checkpoint("MS-AD_LDAP_R571");
            this.Manager.Checkpoint("MS-AD_LDAP_R544");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp16, "errorStatus of AddOperation, state S34");
            this.Manager.Comment("reaching state \'S40\'");
            ConstrOnModOpErrs temp17;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""pwdLastSet: 4568152""->[""objectClass: user"",""distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_LDS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "pwdLastSet: 4568152", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "objectClass: user",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "distinguishedName: CN=ModifyTestUser1,CN=ApplicationNamingContext,DC=adts88",
                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(1)), ServerVersion.Win2008R2, false, out temp17);
            this.Manager.Checkpoint("MS-AD_LDAP_R729");
            this.Manager.Checkpoint("MS-AD_LDAP_R729");
            this.Manager.Checkpoint("MS-AD_LDAP_R732");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out ConstraintViolation_ERROR_INVALID_PARA" +
                    "METER]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.ConstraintViolation_ERROR_INVALID_PARAMETER, temp17, "errorStatus of ModifyOperation, state S46");
            TestScenarioModifyAD_LDSWin2K8R2S49();
            this.Manager.EndTest();
        }
        #endregion
    }
}
