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
    public partial class TestScenarioAddAD_LDSWin2K8R2 : PtfTestClassBase
    {

        public TestScenarioAddAD_LDSWin2K8R2()
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
        public void LDAP_TestScenarioAddAD_LDSWin2K8R2S0()
        {
            this.Manager.BeginTest("TestScenarioAddAD_LDSWin2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S12\'");
            ConstrOnAddOpErrs temp0;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: SPUser1"",""objectClass: top;securityPrincipal;person;organizationalPerson;user"",""distinguishedName: CN=SPUser1,CN=Schema,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: SPUser1",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: top;securityPrincipal;person;organizationalPerson;user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=SPUser1,CN=Schema,CN=Configuration,CN={368E6FB2-DBCB-41A1-B" +
                                                                    "65B-18FAC4B5516E}",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp0);
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
            this.Manager.Checkpoint("MS-AD_LDAP_R563");
            this.Manager.Checkpoint("MS-AD_LDAP_R34");
            this.Manager.Checkpoint("MS-AD_LDAP_R584");
            this.Manager.Checkpoint("MS-AD_LDAP_R33");
            this.Manager.Checkpoint("MS-AD_LDAP_R569");
            this.Manager.Checkpoint("MS-AD_LDAP_R567");
            this.Manager.Checkpoint("MS-AD_LDAP_R571");
            this.Manager.Checkpoint("MS-AD_LDAP_R544");
            this.Manager.Checkpoint("MS-AD_LDAP_R617");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("checking step \'return AddOperation/[out unSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.unSpecifiedError, temp0, "errorStatus of AddOperation, state S18");
            TestScenarioAddAD_LDSWin2K8R2S24();
            this.Manager.EndTest();
        }

        private void TestScenarioAddAD_LDSWin2K8R2S24()
        {
            this.Manager.Comment("reaching state \'S24\'");
            ConstrOnAddOpErrs temp1;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestUser7"",""objectClass: user"",""distinguishedName: CN=TestUser7,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}"",""userPrincipalName: TestUser6@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestUser7",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=TestUser7,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2" +
                                                                    "-DBCB-41A1-B65B-18FAC4B5516E}",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: TestUser6@adts",
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
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp1, "errorStatus of AddOperation, state S29");
            this.Manager.Comment("reaching state \'S34\'");
            ConstrOnAddOpErrs temp2;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestUser10"",""objectClass: user"",""distinguishedName: CN=TestUser10,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestUser10",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=TestUser10,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB" +
                                                                    "2-DBCB-41A1-B65B-18FAC4B5516E}",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp2);
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
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp2, "errorStatus of AddOperation, state S39");
            this.Manager.Comment("reaching state \'S44\'");
            ConstrOnDelOpErr temp3;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=TestUser10,CN=DirectoryUpdates,CN=Config" +
                    "uration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}\",RIGHT_DS_DELETE_CHILD,RIGHT_D" +
                    "ELETE,NoExtendedControl,AD_LDS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=TestUser10,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18" +
                    "FAC4B5516E}", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(1)), ServerVersion.Win2008R2, false, out temp3);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.success, temp3, "errorStatus of DeleteOperation, state S49");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S64\'");
        }
        #endregion

        #region Test Starting in S10
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("LDS")]
        public void LDAP_TestScenarioAddAD_LDSWin2K8R2S10()
        {
            this.Manager.BeginTest("TestScenarioAddAD_LDSWin2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S17\'");
            ConstrOnAddOpErrs temp4;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: SPUser"",""objectClass: top;securityPrincipal;person;organizationalPerson;user"",""distinguishedName: CN=SPUser,CN=ApplicationNamingContext,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: SPUser",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: top;securityPrincipal;person;organizationalPerson;user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=SPUser,CN=ApplicationNamingContext,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp4);
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
            this.Manager.Checkpoint("MS-AD_LDAP_R563");
            this.Manager.Checkpoint("MS-AD_LDAP_R34");
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
            this.Manager.Checkpoint("MS-AD_LDAP_R615");
            this.Manager.Checkpoint("MS-AD_LDAP_R615");
            this.Manager.Checkpoint("MS-AD_LDAP_R615");
            this.Manager.Checkpoint("MS-AD_LDAP_R615");
            this.Manager.Checkpoint("MS-AD_LDAP_R615");
            this.Manager.Checkpoint("MS-AD_LDAP_R615");
            this.Manager.Checkpoint("MS-AD_LDAP_R615");
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp4, "errorStatus of AddOperation, state S23");
            this.Manager.Comment("reaching state \'S28\'");
            ConstrOnAddOpErrs temp5;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestUser7"",""objectClass: user"",""distinguishedName: CN=TestUser7,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}"",""userPrincipalName: TestUser6@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestUser7",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=TestUser7,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2" +
                                                                    "-DBCB-41A1-B65B-18FAC4B5516E}",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: TestUser6@adts",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp5);
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
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp5, "errorStatus of AddOperation, state S33");
            this.Manager.Comment("reaching state \'S38\'");
            ConstrOnAddOpErrs temp6;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestUser10"",""objectClass: user"",""distinguishedName: CN=TestUser10,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestUser10",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=TestUser10,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB" +
                                                                    "2-DBCB-41A1-B65B-18FAC4B5516E}",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp6);
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
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp6, "errorStatus of AddOperation, state S43");
            this.Manager.Comment("reaching state \'S48\'");
            ConstrOnDelOpErr temp7;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=TestUser10,CN=DirectoryUpdates,CN=Config" +
                    "uration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}\",RIGHT_DS_DELETE_CHILD,RIGHT_D" +
                    "ELETE,NoExtendedControl,AD_LDS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=TestUser10,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18" +
                    "FAC4B5516E}", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(1)), ServerVersion.Win2008R2, false, out temp7);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.success, temp7, "errorStatus of DeleteOperation, state S53");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("LDS")]
        public void LDAP_TestScenarioAddAD_LDSWin2K8R2S2()
        {
            this.Manager.BeginTest("TestScenarioAddAD_LDSWin2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S13\'");
            ConstrOnAddOpErrs temp8;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestGroup"",""objectClass: group"",""distinguishedName: CN=TestGroup,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestGroup",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: group",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=TestGroup,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2" +
                                                                    "-DBCB-41A1-B65B-18FAC4B5516E}",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp8);
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
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp8, "errorStatus of AddOperation, state S19");
            this.Manager.Comment("reaching state \'S25\'");
            ConstrOnAddOpErrs temp9;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestUser7"",""objectClass: user"",""distinguishedName: CN=TestUser7,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}"",""userPrincipalName: TestUser6@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestUser7",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=TestUser7,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2" +
                                                                    "-DBCB-41A1-B65B-18FAC4B5516E}",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: TestUser6@adts",
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
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp9, "errorStatus of AddOperation, state S30");
            this.Manager.Comment("reaching state \'S35\'");
            ConstrOnAddOpErrs temp10;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestUser10"",""objectClass: user"",""distinguishedName: CN=TestUser10,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestUser10",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=TestUser10,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB" +
                                                                    "2-DBCB-41A1-B65B-18FAC4B5516E}",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp10);
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
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp10, "errorStatus of AddOperation, state S40");
            this.Manager.Comment("reaching state \'S45\'");
            ConstrOnDelOpErr temp11;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=TestUser10,CN=DirectoryUpdates,CN=Config" +
                    "uration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}\",RIGHT_DS_DELETE_CHILD,RIGHT_D" +
                    "ELETE,NoExtendedControl,AD_LDS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=TestUser10,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18" +
                    "FAC4B5516E}", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(1)), ServerVersion.Win2008R2, false, out temp11);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.success, temp11, "errorStatus of DeleteOperation, state S50");
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
		[TestCategory("LDS")]
        public void LDAP_TestScenarioAddAD_LDSWin2K8R2S4()
        {
            this.Manager.BeginTest("TestScenarioAddAD_LDSWin2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S14\'");
            ConstrOnAddOpErrs temp12;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestUser6"",""objectClass: user"",""distinguishedName: CN=TestUser6,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}"",""userPrincipalName: TestUser6@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestUser6",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=TestUser6,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2" +
                                                                    "-DBCB-41A1-B65B-18FAC4B5516E}",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: TestUser6@adts",
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
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp12, "errorStatus of AddOperation, state S20");
            this.Manager.Comment("reaching state \'S26\'");
            ConstrOnAddOpErrs temp13;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestUser7"",""objectClass: user"",""distinguishedName: CN=TestUser7,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}"",""userPrincipalName: TestUser6@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestUser7",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=TestUser7,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2" +
                                                                    "-DBCB-41A1-B65B-18FAC4B5516E}",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: TestUser6@adts",
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
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R619");
            this.Manager.Comment("reaching state \'S31\'");
            if (EnvironmentConfig.ServerVer >= ServerVersion.Win2012R2)
            {
                this.Manager.Comment("checking step \'return AddOperation/[out ConstraintViolation_ERROR_DS_UPN_VALUE_" +
                    "NOT_UNIQUE_IN_FOREST\'");
                TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.ConstraintViolation_ERROR_DS_UPN_VALUE_NOT_UNIQUE_IN_FOREST, temp13, "errorStatus of ModifyOperation, If another object exists with a duplicate userPrincipalName value, the operation fails with an extended error of ERROR_DS_UPN_VALUE_NOT_UNIQUE_IN_FOREST.");
            }
            else
            {
                this.Manager.Comment("checking step \'return AddOperation/[out AttributeOrValueExists_ERROR_DS_NAME_NOT_" +
                        "UNIQUE]\'");
                TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.AttributeOrValueExists_ERROR_DS_NAME_NOT_UNIQUE, temp13, "errorStatus of AddOperation, state S31");
            }
            this.Manager.Comment("reaching state \'S36\'");
            ConstrOnAddOpErrs temp14;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestUser10"",""objectClass: user"",""distinguishedName: CN=TestUser10,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestUser10",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=TestUser10,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB" +
                                                                    "2-DBCB-41A1-B65B-18FAC4B5516E}",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp14);
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
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp14, "errorStatus of AddOperation, state S41");
            this.Manager.Comment("reaching state \'S46\'");
            ConstrOnDelOpErr temp15;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=TestUser10,CN=DirectoryUpdates,CN=Config" +
                    "uration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}\",RIGHT_DS_DELETE_CHILD,RIGHT_D" +
                    "ELETE,NoExtendedControl,AD_LDS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=TestUser10,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18" +
                    "FAC4B5516E}", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(1)), ServerVersion.Win2008R2, false, out temp15);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.success, temp15, "errorStatus of DeleteOperation, state S51");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S66\'");
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
        public void LDAP_TestScenarioAddAD_LDSWin2K8R2S6()
        {
            this.Manager.BeginTest("TestScenarioAddAD_LDSWin2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S15\'");
            ConstrOnAddOpErrs temp16;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestUser8"",""objectClass: user"",""distinguishedName: CN=TestUser8,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestUser8",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=TestUser8,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2" +
                                                                    "-DBCB-41A1-B65B-18FAC4B5516E}",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp16);
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
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp16, "errorStatus of AddOperation, state S21");
            this.Manager.Comment("reaching state \'S27\'");
            ConstrOnAddOpErrs temp17;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestUser7"",""objectClass: user"",""distinguishedName: CN=TestUser7,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}"",""userPrincipalName: TestUser6@adts""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestUser7",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=TestUser7,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2" +
                                                                    "-DBCB-41A1-B65B-18FAC4B5516E}",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "userPrincipalName: TestUser6@adts",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp17);
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
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp17, "errorStatus of AddOperation, state S32");
            this.Manager.Comment("reaching state \'S37\'");
            ConstrOnAddOpErrs temp18;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestUser10"",""objectClass: user"",""distinguishedName: CN=TestUser10,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestUser10",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=TestUser10,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB" +
                                                                    "2-DBCB-41A1-B65B-18FAC4B5516E}",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp18);
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
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Checkpoint("MS-AD_LDAP_R618");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp18, "errorStatus of AddOperation, state S42");
            this.Manager.Comment("reaching state \'S47\'");
            ConstrOnDelOpErr temp19;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=TestUser10,CN=DirectoryUpdates,CN=Config" +
                    "uration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}\",RIGHT_DS_DELETE_CHILD,RIGHT_D" +
                    "ELETE,NoExtendedControl,AD_LDS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=TestUser10,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18" +
                    "FAC4B5516E}", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(1)), ServerVersion.Win2008R2, false, out temp19);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.success, temp19, "errorStatus of DeleteOperation, state S52");
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S67\'");
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
        public void LDAP_TestScenarioAddAD_LDSWin2K8R2S8()
        {
            this.Manager.BeginTest("TestScenarioAddAD_LDSWin2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S16\'");
            ConstrOnAddOpErrs temp20;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestUser9"",""objectClass: user"",""distinguishedName: CN=TestUser9,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2-DBCB-41A1-B65B-18FAC4B5516E}"",""badPwdCount: 5""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_LDS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestUser9",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=TestUser9,CN=DirectoryUpdates,CN=Configuration,CN={368E6FB2" +
                                                                    "-DBCB-41A1-B65B-18FAC4B5516E}",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "badPwdCount: 5",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(1)), false, out temp20);
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R620");
            this.Manager.Checkpoint("MS-AD_LDAP_R621");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("checking step \'return AddOperation/[out ConstraintViolation_ERROR_DS_ATTRIBUTE_OW" +
                    "NED_BY_SAM]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.ConstraintViolation_ERROR_DS_ATTRIBUTE_OWNED_BY_SAM, temp20, "errorStatus of AddOperation, state S22");
            TestScenarioAddAD_LDSWin2K8R2S24();
            this.Manager.EndTest();
        }
        #endregion
    }
}
