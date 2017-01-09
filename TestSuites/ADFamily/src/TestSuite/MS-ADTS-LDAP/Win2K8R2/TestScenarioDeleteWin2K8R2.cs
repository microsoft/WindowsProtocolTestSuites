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
    public partial class TestScenarioDeleteWin2K8R2 : PtfTestClassBase
    {

        public TestScenarioDeleteWin2K8R2()
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
        [TestCategory("Main")]
        public void LDAP_TestScenarioDeleteWin2K8R2S0()
        {
            this.Manager.BeginTest("TestScenarioDeleteWin2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S24\'");
            ConstrOnAddOpErrs temp0;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: user6745",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: top;person;organizationalPerson;user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "sAMAccountName: user6745",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=user6745,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp0);
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R546");
            this.Manager.Checkpoint("MS-AD_LDAP_R551");
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
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp0, "errorStatus of AddOperation, state S36");
            this.Manager.Comment("reaching state \'S48\'");
            ConstrOnAddOpErrs temp1;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: AdtsTestSite"",""objectClass: site"",""allowedChildClasses: licensingSiteSettings"",""distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: AdtsTestSite",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: site",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "allowedChildClasses: licensingSiteSettings",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp1);
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
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp1, "errorStatus of AddOperation, state S60");
            this.Manager.Comment("reaching state \'S72\'");
            ConstrOnAddOpErrs temp2;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: Licensing Site Settings"",""objectClass: licensingSiteSettings"",""distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: Licensing Site Settings",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: licensingSiteSettings",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configu" +
                                                                    "ration,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp2);
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
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp2, "errorStatus of AddOperation, state S84");
            this.Manager.Comment("reaching state \'S96\'");
            ConstrOnDelOpErr temp3;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=WIN-6IEHBFZ8AMV,CN=Servers,CN=Default-Fi" +
                    "rst-Site-Name,CN=Sites,CN=Configuration,DC=adts88\",RIGHT_DS_DELETE_CHILD,RIGHT_D" +
                    "ELETE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=WIN-6IEHBFZ8AMV,CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuratio" +
                    "n,DC=adts88", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp3);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Checkpoint("MS-AD_LDAP_R896");
            this.Manager.Checkpoint("MS-AD_LDAP_R897");
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp3, "errorStatus of DeleteOperation, state S108");
            TestScenarioDeleteWin2K8R2S120();
            this.Manager.EndTest();
        }

        private void TestScenarioDeleteWin2K8R2S120()
        {
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S126\'");
        }
        #endregion

        #region Test Starting in S10
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioDeleteWin2K8R2S10()
        {
            this.Manager.BeginTest("TestScenarioDeleteWin2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S29\'");
            ConstrOnAddOpErrs temp4;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: user6745",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: top;person;organizationalPerson;user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "sAMAccountName: user6745",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=user6745,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp4);
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R546");
            this.Manager.Checkpoint("MS-AD_LDAP_R551");
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
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp4, "errorStatus of AddOperation, state S41");
            this.Manager.Comment("reaching state \'S53\'");
            ConstrOnAddOpErrs temp5;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: AdtsTestSite"",""objectClass: site"",""allowedChildClasses: licensingSiteSettings"",""distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: AdtsTestSite",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: site",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "allowedChildClasses: licensingSiteSettings",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp5);
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
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp5, "errorStatus of AddOperation, state S65");
            this.Manager.Comment("reaching state \'S77\'");
            ConstrOnAddOpErrs temp6;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: Licensing Site Settings"",""objectClass: licensingSiteSettings"",""distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: Licensing Site Settings",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: licensingSiteSettings",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configu" +
                                                                    "ration,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp6);
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
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp6, "errorStatus of AddOperation, state S89");
            this.Manager.Comment("reaching state \'S101\'");
            ConstrOnDelOpErr temp7;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=NTDS Quotas,CN=Configuration,DC=adts88\"," +
                    "RIGHT_DS_DELETE_CHILD,RIGHT_DELETE,NoExtendedControl,AD_DS,Windows2K8R2,False,ou" +
                    "t _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=NTDS Quotas,CN=Configuration,DC=adts88", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp7);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Checkpoint("MS-AD_LDAP_R903");
            this.Manager.Checkpoint("MS-AD_LDAP_R909");
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out UnwillingToPerform_ERROR_DS_CANT_DELET" +
                    "E]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ((ConstrOnDelOpErr)(1)), temp7, "errorStatus of DeleteOperation, state S113");
            TestScenarioDeleteWin2K8R2S120();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S12
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioDeleteWin2K8R2S12()
        {
            this.Manager.BeginTest("TestScenarioDeleteWin2K8R2S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S30\'");
            ConstrOnAddOpErrs temp8;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: user6745",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: top;person;organizationalPerson;user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "sAMAccountName: user6745",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=user6745,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp8);
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R546");
            this.Manager.Checkpoint("MS-AD_LDAP_R551");
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
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp8, "errorStatus of AddOperation, state S42");
            this.Manager.Comment("reaching state \'S54\'");
            ConstrOnAddOpErrs temp9;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: AdtsTestSite"",""objectClass: site"",""allowedChildClasses: licensingSiteSettings"",""distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: AdtsTestSite",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: site",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "allowedChildClasses: licensingSiteSettings",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp9);
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
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp9, "errorStatus of AddOperation, state S66");
            this.Manager.Comment("reaching state \'S78\'");
            ConstrOnAddOpErrs temp10;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: Licensing Site Settings"",""objectClass: licensingSiteSettings"",""distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: Licensing Site Settings",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: licensingSiteSettings",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configu" +
                                                                    "ration,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp10);
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
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp10, "errorStatus of AddOperation, state S90");
            this.Manager.Comment("reaching state \'S102\'");
            ConstrOnDelOpErr temp11;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=AdtsTestSite,CN=Sites,CN=Configuration,D" +
                    "C=adts88\",RIGHT_DS_DELETE_CHILD,RIGHT_DELETE,NoExtendedControl,AD_DS,Windows2K8R" +
                    "2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp11);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Checkpoint("MS-AD_LDAP_R903");
            this.Manager.Checkpoint("MS-AD_LDAP_R911");
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out NotAllowedOnNonLeaf_ERROR_DS_CHILDREN_" +
                    "EXIST]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.NotAllowedOnNonLeaf_ERROR_DS_CHILDREN_EXIST, temp11, "errorStatus of DeleteOperation, state S114");
            TestScenarioDeleteWin2K8R2S120();
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
        public void LDAP_TestScenarioDeleteWin2K8R2S14()
        {
            this.Manager.BeginTest("TestScenarioDeleteWin2K8R2S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S31\'");
            ConstrOnAddOpErrs temp12;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: user6745",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: top;person;organizationalPerson;user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "sAMAccountName: user6745",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=user6745,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp12);
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R546");
            this.Manager.Checkpoint("MS-AD_LDAP_R551");
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
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp12, "errorStatus of AddOperation, state S43");
            this.Manager.Comment("reaching state \'S55\'");
            ConstrOnAddOpErrs temp13;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: AdtsTestSite"",""objectClass: site"",""allowedChildClasses: licensingSiteSettings"",""distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: AdtsTestSite",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: site",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "allowedChildClasses: licensingSiteSettings",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp13);
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
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp13, "errorStatus of AddOperation, state S67");
            this.Manager.Comment("reaching state \'S79\'");
            ConstrOnAddOpErrs temp14;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: Licensing Site Settings"",""objectClass: licensingSiteSettings"",""distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: Licensing Site Settings",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: licensingSiteSettings",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configu" +
                                                                    "ration,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp14);
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
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp14, "errorStatus of AddOperation, state S91");
            this.Manager.Comment("reaching state \'S103\'");
            ConstrOnDelOpErr temp15;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=Schedule,CN=Schema,CN=Configuration,DC=a" +
                    "dts88\",RIGHT_DS_DELETE_CHILD,RIGHT_DELETE,NoExtendedControl,AD_DS,Windows2K8R2,F" +
                    "alse,out _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=Schedule,CN=Schema,CN=Configuration,DC=adts88", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp15);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Checkpoint("MS-AD_LDAP_R903");
            this.Manager.Checkpoint("MS-AD_LDAP_R914");
            this.Manager.Checkpoint("MS-AD_LDAP_R66");
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out UnwillingToPerform_ERROR_DS_CANT_DELET" +
                    "E]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ((ConstrOnDelOpErr)(1)), temp15, "errorStatus of DeleteOperation, state S115");
            TestScenarioDeleteWin2K8R2S120();
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
        public void LDAP_TestScenarioDeleteWin2K8R2S16()
        {
            this.Manager.BeginTest("TestScenarioDeleteWin2K8R2S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S32\'");
            ConstrOnAddOpErrs temp16;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: user6745",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: top;person;organizationalPerson;user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "sAMAccountName: user6745",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=user6745,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp16);
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R546");
            this.Manager.Checkpoint("MS-AD_LDAP_R551");
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
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp16, "errorStatus of AddOperation, state S44");
            this.Manager.Comment("reaching state \'S56\'");
            ConstrOnAddOpErrs temp17;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: AdtsTestSite"",""objectClass: site"",""allowedChildClasses: licensingSiteSettings"",""distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: AdtsTestSite",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: site",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "allowedChildClasses: licensingSiteSettings",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp17);
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
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp17, "errorStatus of AddOperation, state S68");
            this.Manager.Comment("reaching state \'S80\'");
            ConstrOnAddOpErrs temp18;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: Licensing Site Settings"",""objectClass: licensingSiteSettings"",""distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: Licensing Site Settings",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: licensingSiteSettings",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configu" +
                                                                    "ration,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp18);
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
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp18, "errorStatus of AddOperation, state S92");
            this.Manager.Comment("reaching state \'S104\'");
            ConstrOnDelOpErr temp19;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=user6745,CN=Users,DC=adts88\",RIGHT_DS_DE" +
                    "LETE_CHILD,RIGHT_DELETE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=user6745,CN=Users,DC=adts88", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp19);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Checkpoint("MS-AD_LDAP_R903");
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.success, temp19, "errorStatus of DeleteOperation, state S116");
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S128\'");
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
        public void LDAP_TestScenarioDeleteWin2K8R2S18()
        {
            this.Manager.BeginTest("TestScenarioDeleteWin2K8R2S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S33\'");
            ConstrOnAddOpErrs temp20;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: user6745",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: top;person;organizationalPerson;user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "sAMAccountName: user6745",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=user6745,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp20);
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R546");
            this.Manager.Checkpoint("MS-AD_LDAP_R551");
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
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp20, "errorStatus of AddOperation, state S45");
            this.Manager.Comment("reaching state \'S57\'");
            ConstrOnAddOpErrs temp21;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: AdtsTestSite"",""objectClass: site"",""allowedChildClasses: licensingSiteSettings"",""distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: AdtsTestSite",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: site",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "allowedChildClasses: licensingSiteSettings",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp21);
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
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp21, "errorStatus of AddOperation, state S69");
            this.Manager.Comment("reaching state \'S81\'");
            ConstrOnAddOpErrs temp22;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: Licensing Site Settings"",""objectClass: licensingSiteSettings"",""distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: Licensing Site Settings",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: licensingSiteSettings",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configu" +
                                                                    "ration,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp22);
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
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp22, "errorStatus of AddOperation, state S93");
            this.Manager.Comment("reaching state \'S105\'");
            ConstrOnDelOpErr temp23;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=WIN-6IEHBFZ8AMV,OU=Domain Controllers,DC" +
                    "=adts88\",RIGHT_DS_DELETE_CHILD,RIGHT_DELETE,NoExtendedControl,AD_DS,Windows2K8R2" +
                    ",False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=WIN-6IEHBFZ8AMV,OU=Domain Controllers,DC=adts88", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp23);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Checkpoint("MS-AD_LDAP_R896");
            this.Manager.Checkpoint("MS-AD_LDAP_R897");
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp23, "errorStatus of DeleteOperation, state S117");
            TestScenarioDeleteWin2K8R2S120();
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
        public void LDAP_TestScenarioDeleteWin2K8R2S2()
        {
            this.Manager.BeginTest("TestScenarioDeleteWin2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S25\'");
            ConstrOnAddOpErrs temp24;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: user6745",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: top;person;organizationalPerson;user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "sAMAccountName: user6745",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=user6745,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp24);
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R546");
            this.Manager.Checkpoint("MS-AD_LDAP_R551");
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
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp24, "errorStatus of AddOperation, state S37");
            this.Manager.Comment("reaching state \'S49\'");
            ConstrOnAddOpErrs temp25;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: AdtsTestSite"",""objectClass: site"",""allowedChildClasses: licensingSiteSettings"",""distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: AdtsTestSite",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: site",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "allowedChildClasses: licensingSiteSettings",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp25);
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
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp25, "errorStatus of AddOperation, state S61");
            this.Manager.Comment("reaching state \'S73\'");
            ConstrOnAddOpErrs temp26;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: Licensing Site Settings"",""objectClass: licensingSiteSettings"",""distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: Licensing Site Settings",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: licensingSiteSettings",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configu" +
                                                                    "ration,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp26);
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
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp26, "errorStatus of AddOperation, state S85");
            this.Manager.Comment("reaching state \'S97\'");
            ConstrOnDelOpErr temp27;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=NTDS Settings,CN=WIN-6IEHBFZ8AMV,CN=Serv" +
                    "ers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,DC=adts88\",RIGHT_DS_DEL" +
                    "ETE_CHILD,RIGHT_DELETE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=NTDS Settings,CN=WIN-6IEHBFZ8AMV,CN=Servers,CN=Default-First-Site-Name,CN=Site" +
                    "s,CN=Configuration,DC=adts88", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp27);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Checkpoint("MS-AD_LDAP_R896");
            this.Manager.Checkpoint("MS-AD_LDAP_R897");
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp27, "errorStatus of DeleteOperation, state S109");
            TestScenarioDeleteWin2K8R2S120();
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
        public void LDAP_TestScenarioDeleteWin2K8R2S20()
        {
            this.Manager.BeginTest("TestScenarioDeleteWin2K8R2S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S34\'");
            ConstrOnAddOpErrs temp28;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: user6745",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: top;person;organizationalPerson;user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "sAMAccountName: user6745",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=user6745,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp28);
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R546");
            this.Manager.Checkpoint("MS-AD_LDAP_R551");
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
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp28, "errorStatus of AddOperation, state S46");
            this.Manager.Comment("reaching state \'S58\'");
            ConstrOnAddOpErrs temp29;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: AdtsTestSite"",""objectClass: site"",""allowedChildClasses: licensingSiteSettings"",""distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: AdtsTestSite",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: site",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "allowedChildClasses: licensingSiteSettings",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp29);
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
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp29, "errorStatus of AddOperation, state S70");
            this.Manager.Comment("reaching state \'S82\'");
            ConstrOnAddOpErrs temp30;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: Licensing Site Settings"",""objectClass: licensingSiteSettings"",""distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: Licensing Site Settings",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: licensingSiteSettings",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configu" +
                                                                    "ration,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp30);
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
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp30, "errorStatus of AddOperation, state S94");
            this.Manager.Comment("reaching state \'S106\'");
            ConstrOnDelOpErr temp31;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=RID Set,CN=WIN-6IEHBFZ8AMV,OU=Domain Con" +
                    "trollers,DC=adts88\",RIGHT_DS_DELETE_CHILD,RIGHT_DELETE,NoExtendedControl,AD_DS,W" +
                    "indows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=RID Set,CN=WIN-6IEHBFZ8AMV,OU=Domain Controllers,DC=adts88", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp31);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Checkpoint("MS-AD_LDAP_R896");
            this.Manager.Checkpoint("MS-AD_LDAP_R898");
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp31, "errorStatus of DeleteOperation, state S118");
            TestScenarioDeleteWin2K8R2S120();
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
        public void LDAP_TestScenarioDeleteWin2K8R2S22()
        {
            this.Manager.BeginTest("TestScenarioDeleteWin2K8R2S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S35\'");
            ConstrOnAddOpErrs temp32;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: user6745",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: top;person;organizationalPerson;user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "sAMAccountName: user6745",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=user6745,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp32);
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R546");
            this.Manager.Checkpoint("MS-AD_LDAP_R551");
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
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp32, "errorStatus of AddOperation, state S47");
            this.Manager.Comment("reaching state \'S59\'");
            ConstrOnAddOpErrs temp33;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: AdtsTestSite"",""objectClass: site"",""allowedChildClasses: licensingSiteSettings"",""distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: AdtsTestSite",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: site",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "allowedChildClasses: licensingSiteSettings",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp33);
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
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp33, "errorStatus of AddOperation, state S71");
            this.Manager.Comment("reaching state \'S83\'");
            ConstrOnAddOpErrs temp34;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: Licensing Site Settings"",""objectClass: licensingSiteSettings"",""distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: Licensing Site Settings",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: licensingSiteSettings",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configu" +
                                                                    "ration,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp34);
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
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp34, "errorStatus of AddOperation, state S95");
            this.Manager.Comment("reaching state \'S107\'");
            ConstrOnDelOpErr temp35;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=ADTS88,CN=Partitions,CN=Configuration,DC" +
                    "=adts88\",RIGHT_DS_DELETE_CHILD,RIGHT_DELETE,NoExtendedControl,AD_DS,Windows2K8R2" +
                    ",False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=ADTS88,CN=Partitions,CN=Configuration,DC=adts88", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp35);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Checkpoint("MS-AD_LDAP_R899");
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp35, "errorStatus of DeleteOperation, state S119");
            TestScenarioDeleteWin2K8R2S120();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [Ignore]
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
        [TestCategory("BreakEnvironment")]
        public void LDAP_TestScenarioDeleteWin2K8R2S4()
        {
            this.Manager.BeginTest("TestScenarioDeleteWin2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S26\'");
            ConstrOnAddOpErrs temp36;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: user6745",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: top;person;organizationalPerson;user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "sAMAccountName: user6745",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=user6745,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp36);
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R546");
            this.Manager.Checkpoint("MS-AD_LDAP_R551");
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
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp36, "errorStatus of AddOperation, state S38");
            this.Manager.Comment("reaching state \'S50\'");
            ConstrOnAddOpErrs temp37;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: AdtsTestSite"",""objectClass: site"",""allowedChildClasses: licensingSiteSettings"",""distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: AdtsTestSite",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: site",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "allowedChildClasses: licensingSiteSettings",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp37);
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
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp37, "errorStatus of AddOperation, state S62");
            this.Manager.Comment("reaching state \'S74\'");
            ConstrOnAddOpErrs temp38;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: Licensing Site Settings"",""objectClass: licensingSiteSettings"",""distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: Licensing Site Settings",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: licensingSiteSettings",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configu" +
                                                                    "ration,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp38);
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
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp38, "errorStatus of AddOperation, state S86");
            this.Manager.Comment("reaching state \'S98\'");
            ConstrOnDelOpErr temp39;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=520ec681-1336-471b-93af-c27b780083d9,CN=" +
                    "Partitions,CN=Configuration,DC=adts88\",RIGHT_DS_DELETE_CHILD,RIGHT_DELETE,NoExte" +
                    "ndedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=520ec681-1336-471b-93af-c27b780083d9,CN=Partitions,CN=Configuration,DC=adts88", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp39);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Checkpoint("MS-AD_LDAP_R903");
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.success, temp39, "errorStatus of DeleteOperation, state S110");
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S127\'");
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
        public void LDAP_TestScenarioDeleteWin2K8R2S6()
        {
            this.Manager.BeginTest("TestScenarioDeleteWin2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S27\'");
            ConstrOnAddOpErrs temp40;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: user6745",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: top;person;organizationalPerson;user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "sAMAccountName: user6745",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=user6745,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp40);
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R546");
            this.Manager.Checkpoint("MS-AD_LDAP_R551");
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
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp40, "errorStatus of AddOperation, state S39");
            this.Manager.Comment("reaching state \'S51\'");
            ConstrOnAddOpErrs temp41;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: AdtsTestSite"",""objectClass: site"",""allowedChildClasses: licensingSiteSettings"",""distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: AdtsTestSite",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: site",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "allowedChildClasses: licensingSiteSettings",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp41);
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
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp41, "errorStatus of AddOperation, state S63");
            this.Manager.Comment("reaching state \'S75\'");
            ConstrOnAddOpErrs temp42;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: Licensing Site Settings"",""objectClass: licensingSiteSettings"",""distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: Licensing Site Settings",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: licensingSiteSettings",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configu" +
                                                                    "ration,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp42);
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
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp42, "errorStatus of AddOperation, state S87");
            this.Manager.Comment("reaching state \'S99\'");
            ConstrOnDelOpErr temp43;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=Enterprise Configuration,CN=Partitions,C" +
                    "N=Configuration,DC=adts88\",RIGHT_DS_DELETE_CHILD,RIGHT_DELETE,NoExtendedControl," +
                    "AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=Enterprise Configuration,CN=Partitions,CN=Configuration,DC=adts88", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp43);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Checkpoint("MS-AD_LDAP_R899");
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp43, "errorStatus of DeleteOperation, state S111");
            TestScenarioDeleteWin2K8R2S120();
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
        public void LDAP_TestScenarioDeleteWin2K8R2S8()
        {
            this.Manager.BeginTest("TestScenarioDeleteWin2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S28\'");
            ConstrOnAddOpErrs temp44;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: user6745",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: top;person;organizationalPerson;user",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "sAMAccountName: user6745",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=user6745,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp44);
            this.Manager.Checkpoint("MS-AD_LDAP_R576");
            this.Manager.Checkpoint("MS-AD_LDAP_R154");
            this.Manager.Checkpoint("MS-AD_LDAP_R155");
            this.Manager.Checkpoint("MS-AD_LDAP_R574");
            this.Manager.Checkpoint("MS-AD_LDAP_R520");
            this.Manager.Checkpoint("MS-AD_LDAP_R546");
            this.Manager.Checkpoint("MS-AD_LDAP_R551");
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
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp44, "errorStatus of AddOperation, state S40");
            this.Manager.Comment("reaching state \'S52\'");
            ConstrOnAddOpErrs temp45;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: AdtsTestSite"",""objectClass: site"",""allowedChildClasses: licensingSiteSettings"",""distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: AdtsTestSite",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: site",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "allowedChildClasses: licensingSiteSettings",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "distinguishedName: CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp45);
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
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp45, "errorStatus of AddOperation, state S64");
            this.Manager.Comment("reaching state \'S76\'");
            ConstrOnAddOpErrs temp46;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: Licensing Site Settings"",""objectClass: licensingSiteSettings"",""distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: Licensing Site Settings",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: licensingSiteSettings",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=Licensing Site Settings,CN=AdtsTestSite,CN=Sites,CN=Configu" +
                                                                    "ration,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp46);
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
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp46, "errorStatus of AddOperation, state S88");
            this.Manager.Comment("reaching state \'S100\'");
            ConstrOnDelOpErr temp47;
            this.Manager.Comment("executing step \'call DeleteOperation(\"CN=Enterprise Schema,CN=Partitions,CN=Confi" +
                    "guration,DC=adts88\",RIGHT_DS_DELETE_CHILD,RIGHT_DELETE,NoExtendedControl,AD_DS,W" +
                    "indows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.DeleteOperation("CN=Enterprise Schema,CN=Partitions,CN=Configuration,DC=adts88", RightsOnParentObjects.RIGHT_DS_DELETE_CHILD, ((RightsOnObjects)(1)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp47);
            this.Manager.Checkpoint("MS-AD_LDAP_R901");
            this.Manager.Checkpoint("MS-AD_LDAP_R899");
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("checking step \'return DeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp47, "errorStatus of DeleteOperation, state S112");
            TestScenarioDeleteWin2K8R2S120();
            this.Manager.EndTest();
        }
        #endregion
    }
}
