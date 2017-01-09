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
    public partial class TestScenarioTreeDeleteWin2K8R2 : PtfTestClassBase
    {

        public TestScenarioTreeDeleteWin2K8R2()
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
        public void LDAP_TestScenarioTreeDeleteWin2K8R2S0()
        {
            this.Manager.BeginTest("TestScenarioTreeDeleteWin2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S34\'");
            ConstrOnAddOpErrs temp0;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: userTreeDel"",""objectClass: container"",""distinguishedName: CN=userTreeDel,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: userTreeDel",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: container",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=userTreeDel,CN=Users,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp0);
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
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp0, "errorStatus of AddOperation, state S51");
            this.Manager.Comment("reaching state \'S68\'");
            ConstrOnAddOpErrs temp1;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
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
                                                                            "distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp1);
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
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp1, "errorStatus of AddOperation, state S85");
            this.Manager.Comment("reaching state \'S102\'");
            ConstrOnDelOpErr temp2;
            this.Manager.Comment("executing step \'call TreeDeleteOperation(\"CN=userTreeDel,CN=Users,DC=adts88\",RIGH" +
                    "T_DS_DELETE_TREE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.TreeDeleteOperation("CN=userTreeDel,CN=Users,DC=adts88", ((RightsOnObjects)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp2);
            this.Manager.Checkpoint("MS-AD_LDAP_R930");
            this.Manager.Checkpoint("MS-AD_LDAP_R903");
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return TreeDeleteOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.success, temp2, "errorStatus of TreeDeleteOperation, state S119");
            TestScenarioTreeDeleteWin2K8R2S136();
            this.Manager.EndTest();
        }

        private void TestScenarioTreeDeleteWin2K8R2S136()
        {
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S138\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S140\'");
        }
        #endregion

        #region Test Starting in S10
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioTreeDeleteWin2K8R2S10()
        {
            this.Manager.BeginTest("TestScenarioTreeDeleteWin2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S39\'");
            ConstrOnAddOpErrs temp3;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: userTreeDel"",""objectClass: container"",""distinguishedName: CN=userTreeDel,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: userTreeDel",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: container",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=userTreeDel,CN=Users,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp3);
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
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp3, "errorStatus of AddOperation, state S56");
            this.Manager.Comment("reaching state \'S73\'");
            ConstrOnAddOpErrs temp4;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
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
                                                                            "distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88",
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
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp4, "errorStatus of AddOperation, state S90");
            this.Manager.Comment("reaching state \'S107\'");
            ConstrOnDelOpErr temp5;
            this.Manager.Comment("executing step \'call TreeDeleteOperation(\"CN=RID Set,CN=WIN-6IEHBFZ8AMV,OU=Domain" +
                    " Controllers,DC=adts88\",RIGHT_DS_DELETE_TREE,NoExtendedControl,AD_DS,Windows2K8R" +
                    "2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.TreeDeleteOperation("CN=RID Set,CN=WIN-6IEHBFZ8AMV,OU=Domain Controllers,DC=adts88", ((RightsOnObjects)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp5);
            this.Manager.Checkpoint("MS-AD_LDAP_R930");
            this.Manager.Checkpoint("MS-AD_LDAP_R896");
            this.Manager.Checkpoint("MS-AD_LDAP_R898");
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("checking step \'return TreeDeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp5, "errorStatus of TreeDeleteOperation, state S124");
            TestScenarioTreeDeleteWin2K8R2S137();
            this.Manager.EndTest();
        }

        private void TestScenarioTreeDeleteWin2K8R2S137()
        {
            this.Manager.Comment("reaching state \'S137\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S139\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S141\'");
        }
        #endregion

        #region Test Starting in S12
        [Ignore]
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
        [TestCategory("BreakEnvironment")]
        public void LDAP_TestScenarioTreeDeleteWin2K8R2S12()
        {
            this.Manager.BeginTest("TestScenarioTreeDeleteWin2K8R2S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S40\'");
            ConstrOnAddOpErrs temp6;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: userTreeDel"",""objectClass: container"",""distinguishedName: CN=userTreeDel,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: userTreeDel",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: container",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=userTreeDel,CN=Users,DC=adts88",
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
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp6, "errorStatus of AddOperation, state S57");
            this.Manager.Comment("reaching state \'S74\'");
            ConstrOnAddOpErrs temp7;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
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
                                                                            "distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp7);
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
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp7, "errorStatus of AddOperation, state S91");
            this.Manager.Comment("reaching state \'S108\'");
            ConstrOnDelOpErr temp8;
            this.Manager.Comment("executing step \'call TreeDeleteOperation(\"CN=WIN-6IEHBFZ8AMV,OU=Domain Controller" +
                    "s,DC=adts88\",RIGHT_DS_DELETE_TREE,NoExtendedControl,AD_DS,Windows2K8R2,False,out" +
                    " _)\'");
            this.IAD_LDAPModelAdapterInstance.TreeDeleteOperation("CN=WIN-6IEHBFZ8AMV,OU=Domain Controllers,DC=adts88", ((RightsOnObjects)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp8);
            this.Manager.Checkpoint("MS-AD_LDAP_R930");
            this.Manager.Checkpoint("MS-AD_LDAP_R896");
            this.Manager.Checkpoint("MS-AD_LDAP_R897");
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return TreeDeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp8, "errorStatus of TreeDeleteOperation, state S125");
            TestScenarioTreeDeleteWin2K8R2S137();
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
        public void LDAP_TestScenarioTreeDeleteWin2K8R2S14()
        {
            this.Manager.BeginTest("TestScenarioTreeDeleteWin2K8R2S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S41\'");
            ConstrOnAddOpErrs temp9;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: userTreeDel"",""objectClass: container"",""distinguishedName: CN=userTreeDel,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: userTreeDel",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: container",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=userTreeDel,CN=Users,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp9);
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
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp9, "errorStatus of AddOperation, state S58");
            this.Manager.Comment("reaching state \'S75\'");
            ConstrOnAddOpErrs temp10;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
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
                                                                            "distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp10);
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
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp10, "errorStatus of AddOperation, state S92");
            this.Manager.Comment("reaching state \'S109\'");
            ConstrOnDelOpErr temp11;
            this.Manager.Comment("executing step \'call TreeDeleteOperation(\"OU=Domain Controllers,DC=adts88\",RIGHT_" +
                    "DS_DELETE_TREE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.TreeDeleteOperation("OU=Domain Controllers,DC=adts88", ((RightsOnObjects)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp11);
            this.Manager.Checkpoint("MS-AD_LDAP_R930");
            this.Manager.Checkpoint("MS-AD_LDAP_R896");
            this.Manager.Checkpoint("MS-AD_LDAP_R898");
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("checking step \'return TreeDeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp11, "errorStatus of TreeDeleteOperation, state S126");
            TestScenarioTreeDeleteWin2K8R2S137();
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
        public void LDAP_TestScenarioTreeDeleteWin2K8R2S16()
        {
            this.Manager.BeginTest("TestScenarioTreeDeleteWin2K8R2S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S42\'");
            ConstrOnAddOpErrs temp12;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: userTreeDel"",""objectClass: container"",""distinguishedName: CN=userTreeDel,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: userTreeDel",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: container",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=userTreeDel,CN=Users,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp12);
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
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp12, "errorStatus of AddOperation, state S59");
            this.Manager.Comment("reaching state \'S76\'");
            ConstrOnAddOpErrs temp13;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
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
                                                                            "distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp13);
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
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp13, "errorStatus of AddOperation, state S93");
            this.Manager.Comment("reaching state \'S110\'");
            ConstrOnDelOpErr temp14;
            this.Manager.Comment("executing step \'call TreeDeleteOperation(\"CN=ADTS88,CN=Partitions,CN=Configuratio" +
                    "n,DC=adts88\",RIGHT_DS_DELETE_TREE,NoExtendedControl,AD_DS,Windows2K8R2,False,out" +
                    " _)\'");
            this.IAD_LDAPModelAdapterInstance.TreeDeleteOperation("CN=ADTS88,CN=Partitions,CN=Configuration,DC=adts88", ((RightsOnObjects)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp14);
            this.Manager.Checkpoint("MS-AD_LDAP_R930");
            this.Manager.Checkpoint("MS-AD_LDAP_R899");
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return TreeDeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp14, "errorStatus of TreeDeleteOperation, state S127");
            TestScenarioTreeDeleteWin2K8R2S137();
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
        public void LDAP_TestScenarioTreeDeleteWin2K8R2S18()
        {
            this.Manager.BeginTest("TestScenarioTreeDeleteWin2K8R2S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S43\'");
            ConstrOnAddOpErrs temp15;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: userTreeDel"",""objectClass: container"",""distinguishedName: CN=userTreeDel,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: userTreeDel",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: container",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=userTreeDel,CN=Users,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp15);
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
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp15, "errorStatus of AddOperation, state S60");
            this.Manager.Comment("reaching state \'S77\'");
            ConstrOnAddOpErrs temp16;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
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
                                                                            "distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88",
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
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp16, "errorStatus of AddOperation, state S94");
            this.Manager.Comment("reaching state \'S111\'");
            ConstrOnDelOpErr temp17;
            this.Manager.Comment("executing step \'call TreeDeleteOperation(\"CN=Enterprise Configuration,CN=Partitio" +
                    "ns,CN=Configuration,DC=adts88\",RIGHT_DS_DELETE_TREE,NoExtendedControl,AD_DS,Wind" +
                    "ows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.TreeDeleteOperation("CN=Enterprise Configuration,CN=Partitions,CN=Configuration,DC=adts88", ((RightsOnObjects)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp17);
            this.Manager.Checkpoint("MS-AD_LDAP_R930");
            this.Manager.Checkpoint("MS-AD_LDAP_R899");
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("checking step \'return TreeDeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp17, "errorStatus of TreeDeleteOperation, state S128");
            TestScenarioTreeDeleteWin2K8R2S137();
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
        public void LDAP_TestScenarioTreeDeleteWin2K8R2S2()
        {
            this.Manager.BeginTest("TestScenarioTreeDeleteWin2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S35\'");
            ConstrOnAddOpErrs temp18;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: userTreeDel"",""objectClass: container"",""distinguishedName: CN=userTreeDel,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: userTreeDel",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: container",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=userTreeDel,CN=Users,DC=adts88",
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
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp18, "errorStatus of AddOperation, state S52");
            this.Manager.Comment("reaching state \'S69\'");
            ConstrOnAddOpErrs temp19;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
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
                                                                            "distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp19);
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
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp19, "errorStatus of AddOperation, state S86");
            this.Manager.Comment("reaching state \'S103\'");
            ConstrOnDelOpErr temp20;
            this.Manager.Comment("executing step \'call TreeDeleteOperation(\"CN=NTDS Settings,CN=WIN-6IEHBFZ8AMV,CN=" +
                    "Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,DC=adts88\",RIGHT_DS" +
                    "_DELETE_TREE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.TreeDeleteOperation("CN=NTDS Settings,CN=WIN-6IEHBFZ8AMV,CN=Servers,CN=Default-First-Site-Name,CN=Site" +
                    "s,CN=Configuration,DC=adts88", ((RightsOnObjects)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp20);
            this.Manager.Checkpoint("MS-AD_LDAP_R930");
            this.Manager.Checkpoint("MS-AD_LDAP_R896");
            this.Manager.Checkpoint("MS-AD_LDAP_R897");
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("checking step \'return TreeDeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp20, "errorStatus of TreeDeleteOperation, state S120");
            TestScenarioTreeDeleteWin2K8R2S137();
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
        public void LDAP_TestScenarioTreeDeleteWin2K8R2S20()
        {
            this.Manager.BeginTest("TestScenarioTreeDeleteWin2K8R2S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S44\'");
            ConstrOnAddOpErrs temp21;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: userTreeDel"",""objectClass: container"",""distinguishedName: CN=userTreeDel,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: userTreeDel",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: container",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=userTreeDel,CN=Users,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp21);
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
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp21, "errorStatus of AddOperation, state S61");
            this.Manager.Comment("reaching state \'S78\'");
            ConstrOnAddOpErrs temp22;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
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
                                                                            "distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp22);
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
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp22, "errorStatus of AddOperation, state S95");
            this.Manager.Comment("reaching state \'S112\'");
            ConstrOnDelOpErr temp23;
            this.Manager.Comment("executing step \'call TreeDeleteOperation(\"CN=Enterprise Schema,CN=Partitions,CN=C" +
                    "onfiguration,DC=adts88\",RIGHT_DS_DELETE_TREE,NoExtendedControl,AD_DS,Windows2K8R" +
                    "2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.TreeDeleteOperation("CN=Enterprise Schema,CN=Partitions,CN=Configuration,DC=adts88", ((RightsOnObjects)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp23);
            this.Manager.Checkpoint("MS-AD_LDAP_R930");
            this.Manager.Checkpoint("MS-AD_LDAP_R899");
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return TreeDeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp23, "errorStatus of TreeDeleteOperation, state S129");
            TestScenarioTreeDeleteWin2K8R2S137();
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
        public void LDAP_TestScenarioTreeDeleteWin2K8R2S22()
        {
            this.Manager.BeginTest("TestScenarioTreeDeleteWin2K8R2S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S45\'");
            ConstrOnAddOpErrs temp24;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: userTreeDel"",""objectClass: container"",""distinguishedName: CN=userTreeDel,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: userTreeDel",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: container",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=userTreeDel,CN=Users,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp24);
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
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp24, "errorStatus of AddOperation, state S62");
            this.Manager.Comment("reaching state \'S79\'");
            ConstrOnAddOpErrs temp25;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
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
                                                                            "distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp25);
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
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp25, "errorStatus of AddOperation, state S96");
            this.Manager.Comment("reaching state \'S113\'");
            ConstrOnDelOpErr temp26;
            this.Manager.Comment("executing step \'call TreeDeleteOperation(\"CN=NTDS Quotas,CN=Configuration,DC=adts" +
                    "88\",RIGHT_DS_DELETE_TREE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.TreeDeleteOperation("CN=NTDS Quotas,CN=Configuration,DC=adts88", ((RightsOnObjects)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp26);
            this.Manager.Checkpoint("MS-AD_LDAP_R930");
            this.Manager.Checkpoint("MS-AD_LDAP_R903");
            this.Manager.Checkpoint("MS-AD_LDAP_R909");
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("checking step \'return TreeDeleteOperation/[out UnwillingToPerform_ERROR_DS_CANT_D" +
                    "ELETE]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ((ConstrOnDelOpErr)(1)), temp26, "errorStatus of TreeDeleteOperation, state S130");
            TestScenarioTreeDeleteWin2K8R2S137();
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
        public void LDAP_TestScenarioTreeDeleteWin2K8R2S24()
        {
            this.Manager.BeginTest("TestScenarioTreeDeleteWin2K8R2S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S46\'");
            ConstrOnAddOpErrs temp27;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: userTreeDel"",""objectClass: container"",""distinguishedName: CN=userTreeDel,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: userTreeDel",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: container",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=userTreeDel,CN=Users,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp27);
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
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp27, "errorStatus of AddOperation, state S63");
            this.Manager.Comment("reaching state \'S80\'");
            ConstrOnAddOpErrs temp28;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
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
                                                                            "distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88",
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
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp28, "errorStatus of AddOperation, state S97");
            this.Manager.Comment("reaching state \'S114\'");
            ConstrOnDelOpErr temp29;
            this.Manager.Comment("executing step \'call TreeDeleteOperation(\"CN=AdtsTestSite,CN=Sites,CN=Configurati" +
                    "on,DC=adts88\",RIGHT_DS_DELETE_TREE,NoExtendedControl,AD_DS,Windows2K8R2,False,ou" +
                    "t _)\'");
            this.IAD_LDAPModelAdapterInstance.TreeDeleteOperation("CN=AdtsTestSite,CN=Sites,CN=Configuration,DC=adts88", ((RightsOnObjects)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp29);
            this.Manager.Checkpoint("MS-AD_LDAP_R930");
            this.Manager.Checkpoint("MS-AD_LDAP_R903");
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return TreeDeleteOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.success, temp29, "errorStatus of TreeDeleteOperation, state S131");
            TestScenarioTreeDeleteWin2K8R2S136();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S26
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioTreeDeleteWin2K8R2S26()
        {
            this.Manager.BeginTest("TestScenarioTreeDeleteWin2K8R2S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S47\'");
            ConstrOnAddOpErrs temp30;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: userTreeDel"",""objectClass: container"",""distinguishedName: CN=userTreeDel,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: userTreeDel",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: container",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=userTreeDel,CN=Users,DC=adts88",
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
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp30, "errorStatus of AddOperation, state S64");
            this.Manager.Comment("reaching state \'S81\'");
            ConstrOnAddOpErrs temp31;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
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
                                                                            "distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp31);
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
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp31, "errorStatus of AddOperation, state S98");
            this.Manager.Comment("reaching state \'S115\'");
            ConstrOnDelOpErr temp32;
            this.Manager.Comment("executing step \'call TreeDeleteOperation(\"CN=Schedule,CN=Schema,CN=Configuration," +
                    "DC=adts88\",RIGHT_DS_DELETE_TREE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _" +
                    ")\'");
            this.IAD_LDAPModelAdapterInstance.TreeDeleteOperation("CN=Schedule,CN=Schema,CN=Configuration,DC=adts88", ((RightsOnObjects)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp32);
            this.Manager.Checkpoint("MS-AD_LDAP_R930");
            this.Manager.Checkpoint("MS-AD_LDAP_R903");
            this.Manager.Checkpoint("MS-AD_LDAP_R914");
            this.Manager.Checkpoint("MS-AD_LDAP_R66");
            this.Manager.Comment("reaching state \'S132\'");
            this.Manager.Comment("checking step \'return TreeDeleteOperation/[out UnwillingToPerform_ERROR_DS_CANT_D" +
                    "ELETE]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ((ConstrOnDelOpErr)(1)), temp32, "errorStatus of TreeDeleteOperation, state S132");
            TestScenarioTreeDeleteWin2K8R2S137();
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
        public void LDAP_TestScenarioTreeDeleteWin2K8R2S28()
        {
            this.Manager.BeginTest("TestScenarioTreeDeleteWin2K8R2S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S48\'");
            ConstrOnAddOpErrs temp33;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: userTreeDel"",""objectClass: container"",""distinguishedName: CN=userTreeDel,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: userTreeDel",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: container",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=userTreeDel,CN=Users,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp33);
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
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp33, "errorStatus of AddOperation, state S65");
            this.Manager.Comment("reaching state \'S82\'");
            ConstrOnAddOpErrs temp34;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
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
                                                                            "distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp34);
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
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp34, "errorStatus of AddOperation, state S99");
            this.Manager.Comment("reaching state \'S116\'");
            ConstrOnDelOpErr temp35;
            this.Manager.Comment("executing step \'call TreeDeleteOperation(\"DC=adts88\",RIGHT_DS_DELETE_TREE,NoExten" +
                    "dedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.TreeDeleteOperation("DC=adts88", ((RightsOnObjects)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp35);
            this.Manager.Checkpoint("MS-AD_LDAP_R930");
            this.Manager.Checkpoint("MS-AD_LDAP_R931");
            this.Manager.Comment("reaching state \'S133\'");
            this.Manager.Comment("checking step \'return TreeDeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp35, "errorStatus of TreeDeleteOperation, state S133");
            TestScenarioTreeDeleteWin2K8R2S137();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S30
        [Ignore]
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
        [TestCategory("BreakEnvironment")]
        public void LDAP_TestScenarioTreeDeleteWin2K8R2S30()
        {
            this.Manager.BeginTest("TestScenarioTreeDeleteWin2K8R2S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S49\'");
            ConstrOnAddOpErrs temp36;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: userTreeDel"",""objectClass: container"",""distinguishedName: CN=userTreeDel,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: userTreeDel",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: container",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=userTreeDel,CN=Users,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp36);
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
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp36, "errorStatus of AddOperation, state S66");
            this.Manager.Comment("reaching state \'S83\'");
            ConstrOnAddOpErrs temp37;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
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
                                                                            "distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp37);
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
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp37, "errorStatus of AddOperation, state S100");
            this.Manager.Comment("reaching state \'S117\'");
            ConstrOnDelOpErr temp38;
            this.Manager.Comment("executing step \'call TreeDeleteOperation(\"CN=520ec681-1336-471b-93af-c27b780083d9" +
                    ",CN=Partitions,CN=Configuration,DC=adts88\",RIGHT_DS_DELETE_TREE,NoExtendedContro" +
                    "l,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.TreeDeleteOperation("CN=520ec681-1336-471b-93af-c27b780083d9,CN=Partitions,CN=Configuration,DC=adts88", ((RightsOnObjects)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp38);
            this.Manager.Checkpoint("MS-AD_LDAP_R930");
            this.Manager.Checkpoint("MS-AD_LDAP_R903");
            this.Manager.Comment("reaching state \'S134\'");
            this.Manager.Comment("checking step \'return TreeDeleteOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.success, temp38, "errorStatus of TreeDeleteOperation, state S134");
            TestScenarioTreeDeleteWin2K8R2S136();
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
        public void LDAP_TestScenarioTreeDeleteWin2K8R2S32()
        {
            this.Manager.BeginTest("TestScenarioTreeDeleteWin2K8R2S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S50\'");
            ConstrOnAddOpErrs temp39;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: userTreeDel"",""objectClass: container"",""distinguishedName: CN=userTreeDel,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: userTreeDel",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: container",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=userTreeDel,CN=Users,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp39);
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
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp39, "errorStatus of AddOperation, state S67");
            this.Manager.Comment("reaching state \'S84\'");
            ConstrOnAddOpErrs temp40;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
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
                                                                            "distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88",
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
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp40, "errorStatus of AddOperation, state S101");
            this.Manager.Comment("reaching state \'S118\'");
            ConstrOnDelOpErr temp41;
            this.Manager.Comment("executing step \'call TreeDeleteOperation(\"CN=Users,DC=adts88\",RIGHT_DS_DELETE_TRE" +
                    "E,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.TreeDeleteOperation("CN=Users,DC=adts88", ((RightsOnObjects)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp41);
            this.Manager.Checkpoint("MS-AD_LDAP_R930");
            this.Manager.Checkpoint("MS-AD_LDAP_R932");
            this.Manager.Comment("reaching state \'S135\'");
            this.Manager.Comment("checking step \'return TreeDeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp41, "errorStatus of TreeDeleteOperation, state S135");
            TestScenarioTreeDeleteWin2K8R2S137();
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
        public void LDAP_TestScenarioTreeDeleteWin2K8R2S4()
        {
            this.Manager.BeginTest("TestScenarioTreeDeleteWin2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S36\'");
            ConstrOnAddOpErrs temp42;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: userTreeDel"",""objectClass: container"",""distinguishedName: CN=userTreeDel,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: userTreeDel",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: container",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=userTreeDel,CN=Users,DC=adts88",
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
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp42, "errorStatus of AddOperation, state S53");
            this.Manager.Comment("reaching state \'S70\'");
            ConstrOnAddOpErrs temp43;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
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
                                                                            "distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp43);
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
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp43, "errorStatus of AddOperation, state S87");
            this.Manager.Comment("reaching state \'S104\'");
            ConstrOnDelOpErr temp44;
            this.Manager.Comment("executing step \'call TreeDeleteOperation(\"CN=WIN-6IEHBFZ8AMV,CN=Servers,CN=Defaul" +
                    "t-First-Site-Name,CN=Sites,CN=Configuration,DC=adts88\",RIGHT_DS_DELETE_TREE,NoEx" +
                    "tendedControl,AD_DS,Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.TreeDeleteOperation("CN=WIN-6IEHBFZ8AMV,CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuratio" +
                    "n,DC=adts88", ((RightsOnObjects)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp44);
            this.Manager.Checkpoint("MS-AD_LDAP_R930");
            this.Manager.Checkpoint("MS-AD_LDAP_R896");
            this.Manager.Checkpoint("MS-AD_LDAP_R897");
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return TreeDeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp44, "errorStatus of TreeDeleteOperation, state S121");
            TestScenarioTreeDeleteWin2K8R2S137();
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
        public void LDAP_TestScenarioTreeDeleteWin2K8R2S6()
        {
            this.Manager.BeginTest("TestScenarioTreeDeleteWin2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S37\'");
            ConstrOnAddOpErrs temp45;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: userTreeDel"",""objectClass: container"",""distinguishedName: CN=userTreeDel,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: userTreeDel",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: container",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=userTreeDel,CN=Users,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp45);
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
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp45, "errorStatus of AddOperation, state S54");
            this.Manager.Comment("reaching state \'S71\'");
            ConstrOnAddOpErrs temp46;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
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
                                                                            "distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp46);
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
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp46, "errorStatus of AddOperation, state S88");
            this.Manager.Comment("reaching state \'S105\'");
            ConstrOnDelOpErr temp47;
            this.Manager.Comment("executing step \'call TreeDeleteOperation(\"CN=Servers,CN=Default-First-Site-Name,C" +
                    "N=Sites,CN=Configuration,DC=adts88\",RIGHT_DS_DELETE_TREE,NoExtendedControl,AD_DS" +
                    ",Windows2K8R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.TreeDeleteOperation("CN=Servers,CN=Default-First-Site-Name,CN=Sites,CN=Configuration,DC=adts88", ((RightsOnObjects)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp47);
            this.Manager.Checkpoint("MS-AD_LDAP_R930");
            this.Manager.Checkpoint("MS-AD_LDAP_R896");
            this.Manager.Checkpoint("MS-AD_LDAP_R897");
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("checking step \'return TreeDeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp47, "errorStatus of TreeDeleteOperation, state S122");
            TestScenarioTreeDeleteWin2K8R2S137();
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
        public void LDAP_TestScenarioTreeDeleteWin2K8R2S8()
        {
            this.Manager.BeginTest("TestScenarioTreeDeleteWin2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S38\'");
            ConstrOnAddOpErrs temp48;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: userTreeDel"",""objectClass: container"",""distinguishedName: CN=userTreeDel,CN=Users,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: userTreeDel",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "objectClass: container",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "distinguishedName: CN=userTreeDel,CN=Users,DC=adts88",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp48);
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
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp48, "errorStatus of AddOperation, state S55");
            this.Manager.Comment("reaching state \'S72\'");
            ConstrOnAddOpErrs temp49;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6745"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6745"",""distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
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
                                                                            "distinguishedName: CN=user6745,CN=userTreeDel,CN=Users,DC=adts88",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "instanceType: 4",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp49);
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
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp49, "errorStatus of AddOperation, state S89");
            this.Manager.Comment("reaching state \'S106\'");
            ConstrOnDelOpErr temp50;
            this.Manager.Comment("executing step \'call TreeDeleteOperation(\"CN=Default-First-Site-Name,CN=Sites,CN=" +
                    "Configuration,DC=adts88\",RIGHT_DS_DELETE_TREE,NoExtendedControl,AD_DS,Windows2K8" +
                    "R2,False,out _)\'");
            this.IAD_LDAPModelAdapterInstance.TreeDeleteOperation("CN=Default-First-Site-Name,CN=Sites,CN=Configuration,DC=adts88", ((RightsOnObjects)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp50);
            this.Manager.Checkpoint("MS-AD_LDAP_R930");
            this.Manager.Checkpoint("MS-AD_LDAP_R896");
            this.Manager.Checkpoint("MS-AD_LDAP_R897");
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return TreeDeleteOperation/[out UnSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnDelOpErr>(this.Manager, ConstrOnDelOpErr.UnSpecifiedError, temp50, "errorStatus of TreeDeleteOperation, state S123");
            TestScenarioTreeDeleteWin2K8R2S137();
            this.Manager.EndTest();
        }
        #endregion
    }
}
