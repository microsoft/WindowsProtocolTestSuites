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
    public partial class TestScenarioDefunctWin2K8R2FnLevel2K3 : PtfTestClassBase
    {

        public TestScenarioDefunctWin2K8R2FnLevel2K3()
        {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "30000");
        }

        #region Expect Delegates
        public delegate void SearchOpResponseDelegate1(SearchResp response);
        #endregion

        #region Event Metadata
        static System.Reflection.EventInfo SearchOpResponseInfo = TestManagerHelpers.GetEventInfo(typeof(IAD_LDAPModelAdapter), "SearchOpResponse");
        #endregion

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
            this.Manager.Subscribe(SearchOpResponseInfo, this.IAD_LDAPModelAdapterInstance);
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
        [TestCategory("Main")]
        public void LDAP_TestScenarioDefunctWin2K8R2FnLevel2K3S0()
        {
            this.Manager.BeginTest("TestScenarioDefunctWin2K8R2FnLevel2K3S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S2\'");
            ConstrOnAddOpErrs temp0;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestDefunctClass1"",""Governs-ID: 1.5.840.111216.1.5.36.78.5"",""subClassOf: top"",""distinguishedName: CN=TestDefunctClass1,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: classSchema""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestDefunctClass1",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "Governs-ID: 1.5.840.111216.1.5.36.78.5",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "subClassOf: top",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=TestDefunctClass1,CN=Schema,CN=Configuration,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "objectClass: classSchema",
                                                                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp0);
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
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp0, "errorStatus of AddOperation, state S3");
            this.Manager.Comment("reaching state \'S4\'");
            ConstrOnAddOpErrs temp1;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: DefunctClass1"",""isDefunct: TRUE"",""Governs-ID: 1.2.840.119516.1.5.76"",""subClassOf: top"",""distinguishedName: CN=DefunctClass1,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: classSchema""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: DefunctClass1",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "isDefunct: TRUE",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "Governs-ID: 1.2.840.119516.1.5.76",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "subClassOf: top",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "distinguishedName: CN=DefunctClass1,CN=Schema,CN=Configuration,DC=adts88",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectClass: classSchema",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp1);
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
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp1, "errorStatus of AddOperation, state S5");
            this.Manager.Comment("reaching state \'S6\'");
            ConstrOnAddOpErrs temp2;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestClass1"",""isDefunct: <Not Set>"",""Governs-ID: 1.5.840.179821.5.7467436.87"",""subClassOf: user"",""objectClassCategory: 1"",""distinguishedName: CN=TestClass1,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: classSchema""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestClass1",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "isDefunct: <Not Set>",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "Governs-ID: 1.5.840.179821.5.7467436.87",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "subClassOf: user",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "objectClassCategory: 1",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "distinguishedName: CN=TestClass1,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "objectClass: classSchema",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp2);
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
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp2, "errorStatus of AddOperation, state S7");
            this.Manager.Comment("reaching state \'S8\'");
            ConstrOnAddOpErrs temp3;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: DefunctAttribute10"",""attributeId: 1.2.741.15161.1.4.1690.233.25812.2837418.112027.421.2002411.816770"",""isDefunct: TRUE"",""lDAPDisplayName: DefunctAttribute10"",""distinguishedName: CN=DefunctAttribute10,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: attributeSchema"",""attributeSyntax: 2.5.5.12"",""oMSyntax: 64"",""isSingleValued: FALSE""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: DefunctAttribute10",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "attributeId: 1.2.741.15161.1.4.1690.233.25812.2837418.112027.421.2002411.816770",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "isDefunct: TRUE",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "lDAPDisplayName: DefunctAttribute10",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "distinguishedName: CN=DefunctAttribute10,CN=Schema,CN=Configuration,DC=adts88",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectClass: attributeSchema",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "attributeSyntax: 2.5.5.12",
                                                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                            "Head",
                                                                                                                            "Tail"}, new object[] {
                                                                                                                            "oMSyntax: 64",
                                                                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                                        "Head",
                                                                                                                                        "Tail"}, new object[] {
                                                                                                                                        "isSingleValued: FALSE",
                                                                                                                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp3);
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
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp3, "errorStatus of AddOperation, state S9");
            this.Manager.Comment("reaching state \'S10\'");
            ConstrOnAddOpErrs temp4;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: DefunctClass3"",""isDefunct: <Not Set>"",""Governs-ID: 1.5.840.111216.1.5.36"",""isDefunct: TRUE"",""subClassOf: top"",""distinguishedName: CN=DefunctClass3,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: classSchema""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: DefunctClass3",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "isDefunct: <Not Set>",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "Governs-ID: 1.5.840.111216.1.5.36",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "isDefunct: TRUE",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "subClassOf: top",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "distinguishedName: CN=DefunctClass3,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "objectClass: classSchema",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp4);
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
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp4, "errorStatus of AddOperation, state S11");
            this.Manager.Comment("reaching state \'S12\'");
            ConstrOnAddOpErrs temp5;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: DefunctAttribute8"",""attributeId: 1.2.301.15161.1.4.1200.233.25712.97174.8.112027.421.2002411.81137"",""isDefunct: TRUE"",""lDAPDisplayName: DefunctAttribute8"",""distinguishedName: CN=DefunctAttribute8,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: attributeSchema"",""attributeSyntax: 2.5.5.12"",""oMSyntax: 64"",""isSingleValued: FALSE""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: DefunctAttribute8",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "attributeId: 1.2.301.15161.1.4.1200.233.25712.97174.8.112027.421.2002411.81137",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "isDefunct: TRUE",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "lDAPDisplayName: DefunctAttribute8",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "distinguishedName: CN=DefunctAttribute8,CN=Schema,CN=Configuration,DC=adts88",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectClass: attributeSchema",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "attributeSyntax: 2.5.5.12",
                                                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                            "Head",
                                                                                                                            "Tail"}, new object[] {
                                                                                                                            "oMSyntax: 64",
                                                                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                                        "Head",
                                                                                                                                        "Tail"}, new object[] {
                                                                                                                                        "isSingleValued: FALSE",
                                                                                                                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp5);
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
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp5, "errorStatus of AddOperation, state S13");
            this.Manager.Comment("reaching state \'S14\'");
            ConstrOnAddOpErrs temp6;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: DefunctAttribute"",""attributeId: 1.2.301.15161.1.4.1100.233.26882.28374.8.111927.421.2002441.811270"",""isDefunct: TRUE"",""lDAPDisplayName: DefunctAttribute"",""distinguishedName: CN=DefunctAttribute,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: attributeSchema"",""attributeSyntax: 2.5.5.12"",""oMSyntax: 64"",""isSingleValued: FALSE""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: DefunctAttribute",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "attributeId: 1.2.301.15161.1.4.1100.233.26882.28374.8.111927.421.2002441.811270",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "isDefunct: TRUE",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "lDAPDisplayName: DefunctAttribute",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "distinguishedName: CN=DefunctAttribute,CN=Schema,CN=Configuration,DC=adts88",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectClass: attributeSchema",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "attributeSyntax: 2.5.5.12",
                                                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                            "Head",
                                                                                                                            "Tail"}, new object[] {
                                                                                                                            "oMSyntax: 64",
                                                                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                                        "Head",
                                                                                                                                        "Tail"}, new object[] {
                                                                                                                                        "isSingleValued: FALSE",
                                                                                                                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp6);
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
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp6, "errorStatus of AddOperation, state S15");
            this.Manager.Comment("reaching state \'S16\'");
            ConstrOnAddOpErrs temp7;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestDefunctAttribute1"",""attributeId: 1.2.311.479131.1.4.1136.233"",""lDAPDisplayName: TestDefunctAttribute1"",""distinguishedName: CN=TestDefunctAttribute1,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: attributeSchema"",""attributeSyntax: 2.5.5.12"",""oMSyntax: 64"",""isSingleValued: FALSE""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestDefunctAttribute1",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "attributeId: 1.2.311.479131.1.4.1136.233",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "lDAPDisplayName: TestDefunctAttribute1",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=TestDefunctAttribute1,CN=Schema,CN=Configuration,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "objectClass: attributeSchema",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "attributeSyntax: 2.5.5.12",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "oMSyntax: 64",
                                                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                            "Head",
                                                                                                                            "Tail"}, new object[] {
                                                                                                                            "isSingleValued: FALSE",
                                                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp7);
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
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp7, "errorStatus of AddOperation, state S17");
            this.Manager.Comment("reaching state \'S18\'");
            ConstrOnAddOpErrs temp8;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: DefunctAttribute9"",""isDefunct: <Not Set>"",""attributeId: 1.2.311.15161.9.1.1136.233.21012.26574.7.112027.421.20231.811270"",""lDAPDisplayName: DefunctAttribute9"",""distinguishedName: CN=DefunctAttribute9,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: attributeSchema"",""attributeSyntax: 2.5.5.12"",""oMSyntax: 64"",""isSingleValued: FALSE""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: DefunctAttribute9",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "isDefunct: <Not Set>",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "attributeId: 1.2.311.15161.9.1.1136.233.21012.26574.7.112027.421.20231.811270",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "lDAPDisplayName: DefunctAttribute9",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "distinguishedName: CN=DefunctAttribute9,CN=Schema,CN=Configuration,DC=adts88",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectClass: attributeSchema",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "attributeSyntax: 2.5.5.12",
                                                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                            "Head",
                                                                                                                            "Tail"}, new object[] {
                                                                                                                            "oMSyntax: 64",
                                                                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                                        "Head",
                                                                                                                                        "Tail"}, new object[] {
                                                                                                                                        "isSingleValued: FALSE",
                                                                                                                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp8);
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
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp8, "errorStatus of AddOperation, state S19");
            this.Manager.Comment("reaching state \'S20\'");
            ConstrOnAddOpErrs temp9;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: ADTSFirstClass"",""isDefunct: <Not Set>"",""Governs-ID: 1.2.840.111816.1.5.76"",""subClassOf: top"",""distinguishedName: CN=ADTSFirstClass,CN=Schema,CN=Configuration,DC=adts88"",""mayContain: DefunctAttribute9"",""objectClass: classSchema""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: ADTSFirstClass",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "isDefunct: <Not Set>",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "Governs-ID: 1.2.840.111816.1.5.76",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "subClassOf: top",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "distinguishedName: CN=ADTSFirstClass,CN=Schema,CN=Configuration,DC=adts88",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "mayContain: DefunctAttribute9",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "objectClass: classSchema",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp9);
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
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp9, "errorStatus of AddOperation, state S21");
            this.Manager.Comment("reaching state \'S22\'");
            ConstrOnAddOpErrs temp10;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: AdtsSecondClass"",""isDefunct: <Not Set>"",""Governs-ID: 1.2.840.119116.8.5.76"",""subClassOf: top"",""distinguishedName: CN=AdtsSecondClass,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: classSchema""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: AdtsSecondClass",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "isDefunct: <Not Set>",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "Governs-ID: 1.2.840.119116.8.5.76",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "subClassOf: top",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "distinguishedName: CN=AdtsSecondClass,CN=Schema,CN=Configuration,DC=adts88",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectClass: classSchema",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp10);
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
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp10, "errorStatus of AddOperation, state S23");
            this.Manager.Comment("reaching state \'S24\'");
            ConstrOnAddOpErrs temp11;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: AdtsThirdClass"",""isDefunct: <Not Set>"",""Governs-ID: 1.2.840.111106.2.5.76"",""subClassOf: top"",""distinguishedName: CN=AdtsThirdClass,CN=Schema,CN=Configuration,DC=adts88"",""possSuperiors: AdtsSecondClass"",""objectClass: classSchema""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: AdtsThirdClass",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "isDefunct: <Not Set>",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "Governs-ID: 1.2.840.111106.2.5.76",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "subClassOf: top",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "distinguishedName: CN=AdtsThirdClass,CN=Schema,CN=Configuration,DC=adts88",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "possSuperiors: AdtsSecondClass",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "objectClass: classSchema",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp11);
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
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp11, "errorStatus of AddOperation, state S25");
            this.Manager.Comment("reaching state \'S26\'");
            ConstrOnModOpErrs temp12;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""isDefunct: FALSE""->[""distinguishedName:CN=DefunctAttribute10,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: top;attributeSchema"",""isDefunct: TRUE""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "isDefunct: FALSE", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName:CN=DefunctAttribute10,CN=Schema,CN=Configuration,DC=adts88",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "objectClass: top;attributeSchema",
                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                            "Head",
                                                                                            "Tail"}, new object[] {
                                                                                            "isDefunct: TRUE",
                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp12);
            this.Manager.Checkpoint("MS-AD_LDAP_R148");
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp12, "errorStatus of ModifyOperation, state S27");
            this.Manager.Comment("reaching state \'S28\'");
            ConstrOnModOpErrs temp13;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""isDefunct: TRUE""->[""distinguishedName:CN=TestDefunctAttribute1,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: top;attributeSchema"",""isDefunct: <Not Set>""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "isDefunct: TRUE", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName:CN=TestDefunctAttribute1,CN=Schema,CN=Configuration,DC=adts88",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "objectClass: top;attributeSchema",
                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                            "Head",
                                                                                            "Tail"}, new object[] {
                                                                                            "isDefunct: <Not Set>",
                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp13);
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R66");
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp13, "errorStatus of ModifyOperation, state S29");
            this.Manager.Comment("reaching state \'S30\'");
            ConstrOnModOpErrs temp14;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""isDefunct: TRUE""->[""distinguishedName:CN=TestDefunctClass1,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: top;classSchema"",""isDefunct: <Not Set>""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "isDefunct: TRUE", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName:CN=TestDefunctClass1,CN=Schema,CN=Configuration,DC=adts88",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "objectClass: top;classSchema",
                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                            "Head",
                                                                                            "Tail"}, new object[] {
                                                                                            "isDefunct: <Not Set>",
                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp14);
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R66");
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp14, "errorStatus of ModifyOperation, state S31");
            this.Manager.Comment("reaching state \'S32\'");
            ConstrOnModOpErrs temp15;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""isDefunct: FALSE""->[""distinguishedName:CN=TestDefunctClass1,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: top;classSchema"",""isDefunct: TRUE""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "isDefunct: FALSE", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName:CN=TestDefunctClass1,CN=Schema,CN=Configuration,DC=adts88",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "objectClass: top;classSchema",
                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                            "Head",
                                                                                            "Tail"}, new object[] {
                                                                                            "isDefunct: TRUE",
                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp15);
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp15, "errorStatus of ModifyOperation, state S33");
            this.Manager.Comment("reaching state \'S34\'");
            ConstrOnModOpErrs temp16;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""description: newdescription""->[""distinguishedName:CN=TestDefunctAttribute1,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: top;attributeSchema"",""description: <not set>"",""isDefunct: TRUE""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "description: newdescription", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName:CN=TestDefunctAttribute1,CN=Schema,CN=Configuration,DC=adts88",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "objectClass: top;attributeSchema",
                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                            "Head",
                                                                                            "Tail"}, new object[] {
                                                                                            "description: <not set>",
                                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                        "Head",
                                                                                                        "Tail"}, new object[] {
                                                                                                        "isDefunct: TRUE",
                                                                                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp16);
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp16, "errorStatus of ModifyOperation, state S35");
            this.Manager.Comment("reaching state \'S36\'");
            ConstrOnModOpErrs temp17;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""isDefunct: TRUE""->[""distinguishedName:CN=Given-Name,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: top;attributeSchema"",""isDefunct: <Not Set>""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "isDefunct: TRUE", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName:CN=Given-Name,CN=Schema,CN=Configuration,DC=adts88",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "objectClass: top;attributeSchema",
                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                            "Head",
                                                                                            "Tail"}, new object[] {
                                                                                            "isDefunct: <Not Set>",
                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp17);
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R993");
            this.Manager.Checkpoint("MS-AD_LDAP_R143");
            this.Manager.Checkpoint("MS-AD_LDAP_R994");
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out UnwillingToPerform_UnKnownError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ((ConstrOnModOpErrs)(1)), temp17, "errorStatus of ModifyOperation, state S37");
            this.Manager.Comment("reaching state \'S38\'");
            ConstrOnModOpErrs temp18;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""isDefunct: TRUE""->[""distinguishedName:CN=top,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: top;classSchema"",""isDefunct: <Not Set>""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "isDefunct: TRUE", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName:CN=top,CN=Schema,CN=Configuration,DC=adts88",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "objectClass: top;classSchema",
                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                            "Head",
                                                                                            "Tail"}, new object[] {
                                                                                            "isDefunct: <Not Set>",
                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp18);
            this.Manager.Checkpoint("MS-AD_LDAP_R144");
            this.Manager.Checkpoint("MS-AD_LDAP_R996");
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out UnwillingToPerform_UnKnownError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ((ConstrOnModOpErrs)(1)), temp18, "errorStatus of ModifyOperation, state S39");
            this.Manager.Comment("reaching state \'S40\'");
            ConstrOnModOpErrs temp19;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""isDefunct: TRUE""->[""distinguishedName:CN=ADTSFirstClass,CN=Schema,CN=Configuration,DC=adts88"",""isDefunct: <Not Set>""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "isDefunct: TRUE", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName:CN=ADTSFirstClass,CN=Schema,CN=Configuration,DC=adts88",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "isDefunct: <Not Set>",
                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp19);
            this.Manager.Checkpoint("MS-AD_LDAP_R66");
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp19, "errorStatus of ModifyOperation, state S41");
            this.Manager.Comment("reaching state \'S42\'");
            ConstrOnModOpErrs temp20;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""isDefunct: TRUE""->[""distinguishedName:CN=DefunctAttribute9,CN=Schema,CN=Configuration,DC=adts88"",""isDefunct: <Not Set>""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "isDefunct: TRUE", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName:CN=DefunctAttribute9,CN=Schema,CN=Configuration,DC=adts88",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "isDefunct: <Not Set>",
                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp20);
            this.Manager.Checkpoint("MS-AD_LDAP_R66");
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp20, "errorStatus of ModifyOperation, state S43");
            this.Manager.Comment("reaching state \'S44\'");
            ConstrOnModOpErrs temp21;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""isDefunct: FALSE""->[""distinguishedName:CN=ADTSFirstClass,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: top;classSchema"",""isDefunct: TRUE""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "isDefunct: FALSE", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName:CN=ADTSFirstClass,CN=Schema,CN=Configuration,DC=adts88",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "objectClass: top;classSchema",
                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                            "Head",
                                                                                            "Tail"}, new object[] {
                                                                                            "isDefunct: TRUE",
                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp21);
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R997");
            this.Manager.Checkpoint("MS-AD_LDAP_R998");
            this.Manager.Checkpoint("MS-AD_LDAP_R145");
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out UnwillingToPerform_UnKnownError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ((ConstrOnModOpErrs)(1)), temp21, "errorStatus of ModifyOperation, state S45");
            this.Manager.Comment("reaching state \'S46\'");
            ConstrOnModOpErrs temp22;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""isDefunct: TRUE""->[""distinguishedName:CN=AdtsThirdClass,CN=Schema,CN=Configuration,DC=adts88"",""isDefunct: <Not Set>""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "isDefunct: TRUE", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName:CN=AdtsThirdClass,CN=Schema,CN=Configuration,DC=adts88",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "isDefunct: <Not Set>",
                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp22);
            this.Manager.Checkpoint("MS-AD_LDAP_R66");
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp22, "errorStatus of ModifyOperation, state S47");
            this.Manager.Comment("reaching state \'S48\'");
            ConstrOnModOpErrs temp23;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""isDefunct: TRUE""->[""distinguishedName:CN=AdtsSecondClass,CN=Schema,CN=Configuration,DC=adts88"",""isDefunct: <Not Set>""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "isDefunct: TRUE", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName:CN=AdtsSecondClass,CN=Schema,CN=Configuration,DC=adts88",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "isDefunct: <Not Set>",
                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp23);
            this.Manager.Checkpoint("MS-AD_LDAP_R66");
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp23, "errorStatus of ModifyOperation, state S49");
            this.Manager.Comment("reaching state \'S50\'");
            ConstrOnModOpErrs temp24;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""isDefunct: FALSE""->[""distinguishedName:CN=AdtsThirdClass,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: top;classSchema"",""isDefunct: TRUE""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "isDefunct: FALSE", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName:CN=AdtsThirdClass,CN=Schema,CN=Configuration,DC=adts88",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "objectClass: top;classSchema",
                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                            "Head",
                                                                                            "Tail"}, new object[] {
                                                                                            "isDefunct: TRUE",
                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp24);
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R999");
            this.Manager.Checkpoint("MS-AD_LDAP_R1000");
            this.Manager.Checkpoint("MS-AD_LDAP_R145");
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out UnwillingToPerform_UnKnownError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ((ConstrOnModOpErrs)(1)), temp24, "errorStatus of ModifyOperation, state S51");
            this.Manager.Comment("reaching state \'S52\'");
            ConstrOnModOpErrs temp25;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""lDAPDisplayName: DefunctAttribute3""->[""distinguishedName: CN=DefunctAttribute8,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: top;attributeSchema"",""lDAPDisplayName: DefunctAttribute8""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "lDAPDisplayName: DefunctAttribute3", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: CN=DefunctAttribute8,CN=Schema,CN=Configuration,DC=adts88",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "objectClass: top;attributeSchema",
                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                            "Head",
                                                                                            "Tail"}, new object[] {
                                                                                            "lDAPDisplayName: DefunctAttribute8",
                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp25);
            this.Manager.Checkpoint("MS-AD_LDAP_R149");
            this.Manager.Checkpoint("MS-AD_LDAP_R150");
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp25, "errorStatus of ModifyOperation, state S53");
            this.Manager.Comment("reaching state \'S54\'");
            ConstrOnModOpErrs temp26;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""lDAPDisplayName: DefunctClass2""->[""distinguishedName: CN=DefunctClass1,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: top;classSchema"",""lDAPDisplayName: DefunctClass1""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "lDAPDisplayName: DefunctClass2", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: CN=DefunctClass1,CN=Schema,CN=Configuration,DC=adts88",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "objectClass: top;classSchema",
                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                            "Head",
                                                                                            "Tail"}, new object[] {
                                                                                            "lDAPDisplayName: DefunctClass1",
                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp26);
            this.Manager.Checkpoint("MS-AD_LDAP_R149");
            this.Manager.Checkpoint("MS-AD_LDAP_R151");
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp26, "errorStatus of ModifyOperation, state S55");
            this.Manager.Comment("reaching state \'S56\'");
            ConstrOnAddOpErrs temp27;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: AdtsTestTdiClass"",""objectClass:classSchema"",""subClassOf: top"",""distinguishedName: CN=AdtsTestTdiClass,CN=Schema,CN=Configuration,DC=adts88"",""Governs-ID: 1.2.6.1.2.1.12.1.1.11.19"",""mayContain: DefunctAttribute""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: AdtsTestTdiClass",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass:classSchema",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "subClassOf: top",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=AdtsTestTdiClass,CN=Schema,CN=Configuration,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "Governs-ID: 1.2.6.1.2.1.12.1.1.11.19",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "mayContain: DefunctAttribute",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp27);
            this.Manager.Checkpoint("MS-AD_LDAP_R134");
            this.Manager.Checkpoint("MS-AD_LDAP_R135");
            this.Manager.Checkpoint("MS-AD_LDAP_R513");
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return AddOperation/[out unSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.unSpecifiedError, temp27, "errorStatus of AddOperation, state S57");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=DefunctClass3,CN=Schema,CN=Configuration,DC=" +
                    "adts88\",\"(objectClass=classSchema)\",Subtree,[\"isDefunct\"],NoExtendedControl,AD_D" +
                    "S)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=DefunctClass3,CN=Schema,CN=Configuration,DC=adts88", "(objectClass=classSchema)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "isDefunct",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S60\'");
            ConstrOnAddOpErrs temp28;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestClass1Obj"",""objectClass:TestClass1"",""sAMAccountName: TestClass1Obj"",""distinguishedName: CN=TestClass1Obj,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: TestClass1Obj",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass:TestClass1",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "sAMAccountName: TestClass1Obj",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=TestClass1Obj,CN=Users,DC=adts88",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp28);
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
            this.Manager.Checkpoint("MS-AD_LDAP_R584");
            this.Manager.Checkpoint("MS-AD_LDAP_R33");
            this.Manager.Checkpoint("MS-AD_LDAP_R569");
            this.Manager.Checkpoint("MS-AD_LDAP_R571");
            this.Manager.Checkpoint("MS-AD_LDAP_R544");
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp28, "errorStatus of AddOperation, state S61");
            this.Manager.Comment("reaching state \'S62\'");
            ConstrOnModOpErrs temp29;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""isDefunct: TRUE""->[""distinguishedName: CN=TestClass1,CN=Schema,CN=Configuration,DC=adts88"",""isDefunct: <Not Set>""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(this.Make<Microsoft.Modeling.Map<string, Microsoft.Modeling.Sequence<string>>>(new string[] {
                            "Rep"}, new object[] {
                            Microsoft.Xrt.Runtime.RuntimeSupport.UpdateMap<string, Microsoft.Modeling.Sequence<string>>(Microsoft.Xrt.Runtime.RuntimeSupport.MakeMap<string, Microsoft.Modeling.Sequence<string>>(), "isDefunct: TRUE", this.Make<Microsoft.Xrt.Runtime.RuntimeMapElement<Microsoft.Modeling.Sequence<string>>>(new string[] {
                                            "Element"}, new object[] {
                                            this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                                                        "Rep"}, new object[] {
                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                    "Head",
                                                                    "Tail"}, new object[] {
                                                                    "distinguishedName: CN=TestClass1,CN=Schema,CN=Configuration,DC=adts88",
                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                "Head",
                                                                                "Tail"}, new object[] {
                                                                                "isDefunct: <Not Set>",
                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}))}), ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp29);
            this.Manager.Checkpoint("MS-AD_LDAP_R66");
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp29, "errorStatus of ModifyOperation, state S63");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=TestClass1Obj,CN=Users,DC=adts88\",\"(objectClass=user)\",Subtree,[\"objectClass\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=TestClass1Obj,CN=Users,DC=adts88", "(objectClass=user)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "objectClass",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S66\'");
            ConstrOnDelOpErr temp30;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=TestClass1Obj,CN=Users,DC=adts88\",RIGHT_" +
                    "DS_DELETE_CHILD,RIGHT_DELETE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=TestClass1Obj,CN=Users,DC=adts88", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp30);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Checkpoint("MS-AD_LDAP_R903");
            this.Manager.Checkpoint("MS-AD_LDAP_R888");
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.success, temp30, "errorStatus of DeleteOperation, state S67");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=ADTSFirstClass,CN=Schema,CN=Configuration,DC" +
                    "=adts88\",\"(objectClass=classschema)\",Subtree,[\"mayContain\"],NoExtendedControl,AD" +
                    "_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=ADTSFirstClass,CN=Schema,CN=Configuration,DC=adts88", "(objectClass=classschema)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "mayContain",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S70\'");
            int temp31 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TestScenarioDefunctWin2K8R2FnLevel2K3.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioDefunctWin2K8R2FnLevel2K3S0SearchOpResponseChecker)), new ExpectedEvent(TestScenarioDefunctWin2K8R2FnLevel2K3.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioDefunctWin2K8R2FnLevel2K3S0SearchOpResponseChecker1)));
            if ((temp31 == 0))
            {
                this.Manager.Comment("reaching state \'S71\'");
                this.Manager.Comment("executing step \'call UnBind()\'");
                this.IAD_LDAPModelAdapterInstance.UnBind();
                this.Manager.Comment("reaching state \'S73\'");
                this.Manager.Comment("checking step \'return UnBind\'");
                this.Manager.Comment("reaching state \'S75\'");
                goto label0;
            }
            if ((temp31 == 1))
            {
                this.Manager.Comment("reaching state \'S72\'");
                this.Manager.Comment("executing step \'call UnBind()\'");
                this.IAD_LDAPModelAdapterInstance.UnBind();
                this.Manager.Comment("reaching state \'S74\'");
                this.Manager.Comment("checking step \'return UnBind\'");
                this.Manager.Comment("reaching state \'S76\'");
                goto label0;
            }
            this.Manager.CheckObservationTimeout(false, new ExpectedEvent(TestScenarioDefunctWin2K8R2FnLevel2K3.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioDefunctWin2K8R2FnLevel2K3S0SearchOpResponseChecker)), new ExpectedEvent(TestScenarioDefunctWin2K8R2FnLevel2K3.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioDefunctWin2K8R2FnLevel2K3S0SearchOpResponseChecker1)));
        label0:
            ;
            this.Manager.EndTest();
        }

        private void TestScenarioDefunctWin2K8R2FnLevel2K3S0SearchOpResponseChecker(SearchResp response)
        {
            this.Manager.Comment("checking step \'event SearchOpResponse(retrievalSuccessful)\'");
            TestManagerHelpers.AssertAreEqual<SearchResp>(this.Manager, ((SearchResp)(0)), response, "response of SearchOpResponse, state S70");
        }

        private void TestScenarioDefunctWin2K8R2FnLevel2K3S0SearchOpResponseChecker1(SearchResp response)
        {
            this.Manager.Comment("checking step \'event SearchOpResponse(retreivalUnsuccessful)\'");
            TestManagerHelpers.AssertAreEqual<SearchResp>(this.Manager, ((SearchResp)(1)), response, "response of SearchOpResponse, state S70");
        }
        #endregion
    }
}
