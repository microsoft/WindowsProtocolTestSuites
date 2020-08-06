// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Ldap
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
    using Microsoft.Protocols.TestTools;
    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClassAttribute()]
    public partial class TestScenarioDefunctWin2K8R2 : PtfTestClassBase
    {

        public TestScenarioDefunctWin2K8R2()
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
            this.IAD_LDAPModelAdapterInstance = ((IAD_LDAPModelAdapter)(this.GetAdapter(typeof(IAD_LDAPModelAdapter))));
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
        public void LDAP_TestScenarioDefunctWin2K8R2S0()
        {
            this.Manager.BeginTest("TestScenarioDefunctWin2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S2\'");
            ConstrOnAddOpErrs temp0;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: DefunctClass3"",""Governs-ID: 1.5.840.111216.1.5.36"",""isDefunct: TRUE"",""subClassOf: top"",""distinguishedName: CN=DefunctClass3,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: classSchema""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(new List<string> { "cn: DefunctClass3", "Governs-ID: 1.5.840.111216.1.5.36", "isDefunct: TRUE", "subClassOf: top", "distinguishedName: CN=DefunctClass3,CN=Schema,CN=Configuration,DC=adts88", "objectClass: classSchema" }, ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp0);
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
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: DefunctAttribute"",""attributeId: 1.2.301.15161.1.4.1100.233.26882.28374.8.111927.421.2002441.811270"",""isDefunct: TRUE"",""lDAPDisplayName: DefunctAttribute"",""distinguishedName: CN=DefunctAttribute,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: attributeSchema"",""attributeSyntax: 2.5.5.12"",""oMSyntax: 64"",""isSingleValued: FALSE""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(new List<string> { "cn: DefunctAttribute", "attributeId: 1.2.301.15161.1.4.1100.233.26882.28374.8.111927.421.2002441.811270", "isDefunct: TRUE", "lDAPDisplayName: DefunctAttribute", "distinguishedName: CN=DefunctAttribute,CN=Schema,CN=Configuration,DC=adts88", "objectClass: attributeSchema", "attributeSyntax: 2.5.5.12", "oMSyntax: 64", "isSingleValued: FALSE" }, ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp1);
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
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TempClass"",""Governs-ID: 1.3.6.1.4.1.778.64738.24326.45.761.2"",""objectClassCategory: 1"",""isDefunct: <Not Set>"",""subClassOf: user"",""distinguishedName: CN=TempClass,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: classSchema""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(new List<string> { "cn: TempClass", "Governs-ID: 1.3.6.1.4.1.778.64738.24326.45.761.2", "objectClassCategory: 1", "isDefunct: <Not Set>", "subClassOf: user", "distinguishedName: CN=TempClass,CN=Schema,CN=Configuration,DC=adts88", "objectClass: classSchema" }, ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp2);
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
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TestClass"",""Governs-ID: 1.5.840.1798216.1.5.36.78.5.87"",""subClassOf: top"",""distinguishedName: CN=TestClass,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: classSchema""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(new List<string> { "cn: TestClass", "Governs-ID: 1.5.840.1798216.1.5.36.78.5.87", "subClassOf: top", "distinguishedName: CN=TestClass,CN=Schema,CN=Configuration,DC=adts88", "objectClass: classSchema" }, ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp3);
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
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6746"",""objectClass: top;person;DefunctClass3;user"",""sAMAccountName: user6746"",""distinguishedName: CN=user6746,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(new List<string> { "cn: user6746", "objectClass: top;person;DefunctClass3;user", "sAMAccountName: user6746", "distinguishedName: CN=user6746,CN=Users,DC=adts88", "instanceType: 4", "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88" }, ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp4);
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
            this.Manager.Checkpoint("MS-AD_LDAP_R67");
            this.Manager.Checkpoint("MS-AD_LDAP_R562");
            this.Manager.Checkpoint("MS-AD_LDAP_R133");
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return AddOperation/[out NoSuchAttribute_ERROR_INVALID_PARAMETER]\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.NoSuchAttribute_ERROR_INVALID_PARAMETER, temp4, "errorStatus of AddOperation, state S11");
            this.Manager.Comment("reaching state \'S12\'");
            ConstrOnModOpErrs temp5;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""mayContain: DefunctAttribute""->[""distinguishedName: CN=user,CN=Schema,CN=Configuration,DC=adts88"",""mayContain: <Not Set>""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(new Dictionary<string, IList<string>> { { "mayContain: DefunctAttribute", new List<string> { "distinguishedName: CN=user,CN=Schema,CN=Configuration,DC=adts88", "mayContain: <Not Set>" } } }, ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp5);
            this.Manager.Checkpoint("MS-AD_LDAP_R134");
            this.Manager.Checkpoint("MS-AD_LDAP_R136");
            this.Manager.Checkpoint("MS-AD_LDAP_R1489");
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out NoSuchAttribute_ERROR_INVALID_PARAMETE" +
                    "R]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.NoSuchAttribute_ERROR_INVALID_PARAMETER, temp5, "errorStatus of ModifyOperation, state S13");
            this.Manager.Comment("reaching state \'S14\'");
            ConstrOnModOpErrs temp6;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""isDefunct: TRUE""->[""distinguishedName: CN=Reps-From,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: top;attributeSchema"",""systemFlags: 19"",""isDefunct: <Not Set>""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(new Dictionary<string, IList<string>> { { "isDefunct: TRUE", new List<string> { "distinguishedName: CN=Reps-From,CN=Schema,CN=Configuration,DC=adts88", "objectClass: top;attributeSchema", "systemFlags: 19", "isDefunct: <Not Set>" } } }, ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp6);
            this.Manager.Checkpoint("MS-AD_LDAP_R130");
            this.Manager.Checkpoint("MS-AD_LDAP_R131");
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out UnwillingToPerform_UnKnownError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ((ConstrOnModOpErrs)(1)), temp6, "errorStatus of ModifyOperation, state S15");
            this.Manager.Comment("reaching state \'S16\'");
            ConstrOnModOpErrs temp7;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""isDefunct: TRUE""->[""distinguishedName: CN=Site-Link,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: top;classSchema"",""systemFlags: 16"",""isDefunct: <Not Set>""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(new Dictionary<string, IList<string>> { { "isDefunct: TRUE", new List<string> { "distinguishedName: CN=Site-Link,CN=Schema,CN=Configuration,DC=adts88", "objectClass: top;classSchema", "systemFlags: 16", "isDefunct: <Not Set>" } } }, ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp7);
            this.Manager.Checkpoint("MS-AD_LDAP_R130");
            this.Manager.Checkpoint("MS-AD_LDAP_R132");
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out UnwillingToPerform_UnKnownError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ((ConstrOnModOpErrs)(1)), temp7, "errorStatus of ModifyOperation, state S17");
            this.Manager.Comment("reaching state \'S18\'");
            ConstrOnAddOpErrs temp8;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6746"",""objectClass: top;person;NonExistingClass;user"",""sAMAccountName: user6746"",""distinguishedName: CN=user6746,CN=Users,DC=adts88"",""instanceType: 4"",""objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(new List<string> { "cn: user6746", "objectClass: top;person;NonExistingClass;user", "sAMAccountName: user6746", "distinguishedName: CN=user6746,CN=Users,DC=adts88", "instanceType: 4", "objectCategory: CN=Person,CN=Schema,CN=Configuration,DC=adts88" }, ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp8);
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
            this.Manager.Checkpoint("MS-AD_LDAP_R562");
            this.Manager.Checkpoint("MS-AD_LDAP_R4309");
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return AddOperation/[out NoSuchAttribute_ERROR_INVALID_PARAMETER]\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.NoSuchAttribute_ERROR_INVALID_PARAMETER, temp8, "errorStatus of AddOperation, state S19");
            this.Manager.Comment("reaching state \'S20\'");
            ConstrOnAddOpErrs temp9;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: user6746"",""objectClass: top;person;organizationalPerson;user"",""sAMAccountName: user6746"",""distinguishedName: CN=user6746,CN=Users,DC=adts88"",""instanceType: 4"",""NonExistingAttribute: CN=Person,CN=Schema,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(new List<string> { "cn: user6746", "objectClass: top;person;organizationalPerson;user", "sAMAccountName: user6746", "distinguishedName: CN=user6746,CN=Users,DC=adts88", "instanceType: 4", "NonExistingAttribute: CN=Person,CN=Schema,CN=Configuration,DC=adts88" }, ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp9);
            this.Manager.Checkpoint("MS-AD_LDAP_R577");
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return AddOperation/[out NoSuchAttribute_ERROR_INVALID_PARAMETER]\'" +
                    "");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.NoSuchAttribute_ERROR_INVALID_PARAMETER, temp9, "errorStatus of AddOperation, state S21");
            this.Manager.Comment("reaching state \'S22\'");
            ConstrOnAddOpErrs temp10;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: AdtsTestTdiClass"",""objectClass:classSchema"",""subClassOf: top"",""distinguishedName: CN=AdtsTestTdiClass,CN=Schema,CN=Configuration,DC=adts88"",""Governs-ID: 1.2.6.1.2.1.12.1.1.11.19"",""mayContain: DefunctAttribute""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(new List<string> { "cn: AdtsTestTdiClass", "objectClass:classSchema", "subClassOf: top", "distinguishedName: CN=AdtsTestTdiClass,CN=Schema,CN=Configuration,DC=adts88", "Governs-ID: 1.2.6.1.2.1.12.1.1.11.19", "mayContain: DefunctAttribute" }, ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp10);
            this.Manager.Checkpoint("MS-AD_LDAP_R134");
            this.Manager.Checkpoint("MS-AD_LDAP_R135");
            this.Manager.Checkpoint("MS-AD_LDAP_R513");
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return AddOperation/[out unSpecifiedError]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.unSpecifiedError, temp10, "errorStatus of AddOperation, state S23");
            this.Manager.Comment("reaching state \'S24\'");
            ConstrOnModOpErrs temp11;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""isDefunct: TRUE""->[""distinguishedName: CN=TestClass,CN=Schema,CN=Configuration,DC=adts88"",""objectClass: top;classSchema""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(new Dictionary<string, IList<string>> { { "isDefunct: TRUE", new List<string> { "distinguishedName: CN=TestClass,CN=Schema,CN=Configuration,DC=adts88", "objectClass: top;classSchema" } } }, ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp11);
            this.Manager.Checkpoint("MS-AD_LDAP_R995");
            this.Manager.Checkpoint("MS-AD_LDAP_R66");
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp11, "errorStatus of ModifyOperation, state S25");
            this.Manager.Comment("reaching state \'S26\'");
            ConstrOnAddOpErrs temp12;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: TempObj"",""sAMAccountName: TempObj"",""distinguishedName: CN=TempObj,CN=Users,DC=adts88"",""objectClass: TempClass""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(new List<string> { "cn: TempObj", "sAMAccountName: TempObj", "distinguishedName: CN=TempObj,CN=Users,DC=adts88", "objectClass: TempClass" }, ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp12);
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
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp12, "errorStatus of AddOperation, state S27");
            this.Manager.Comment("reaching state \'S28\'");
            ConstrOnModOpErrs temp13;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""isDefunct: TRUE""->[""distinguishedName: CN=TempClass,CN=Schema,CN=Configuration,DC=adts88"",""isDefunct: <Not Set>""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(new Dictionary<string, IList<string>> { { "isDefunct: TRUE", new List<string> { "distinguishedName: CN=TempClass,CN=Schema,CN=Configuration,DC=adts88", "isDefunct: <Not Set>" } } }, ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp13);
            this.Manager.Checkpoint("MS-AD_LDAP_R66");
            this.Manager.Checkpoint("MS-AD_LDAP_R675");
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.success, temp13, "errorStatus of ModifyOperation, state S29");
            this.Manager.Comment("reaching state \'S30\'");
            ConstrOnModOpErrs temp14;
            this.Manager.Comment(@"executing step 'call ModifyOperation({""description: TempObj""->[""cn: TempObj"",""objectClass: TempClass"",""sAMAccountName: TempObj"",""distinguishedName: CN=TempObj,CN=Users,DC=adts88"",""instanceType: 4"",""description: <Not Set>""]},RIGHT_DS_WRITE_PROPERTYwithSE_ENABLE_DELEGATION_PRIVILEGE,NoExtendedControl,AD_DS,Windows2K8R2,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.ModifyOperation(new Dictionary<string, IList<string>> { { "description: TempObj", new List<string> { "cn: TempObj", "objectClass: TempClass", "sAMAccountName: TempObj", "distinguishedName: CN=TempObj,CN=Users,DC=adts88", "instanceType: 4", "description: <Not Set>" } } }, ((RightsOnAttributes)(0)), null, ((ADImplementations)(0)), ServerVersion.Win2008R2, false, out temp14);
            this.Manager.Checkpoint("MS-AD_LDAP_R692");
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return ModifyOperation/[out ObjectClassViolation_ERROR_DS_OBJECT_C" +
                    "LASS_REQUIRED]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnModOpErrs>(this.Manager, ConstrOnModOpErrs.ObjectClassViolation_ERROR_DS_OBJECT_CLASS_REQUIRED, temp14, "errorStatus of ModifyOperation, state S31");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call UnBind()\'");
            this.IAD_LDAPModelAdapterInstance.UnBind();
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return UnBind\'");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.EndTest();
        }
        #endregion
    }
}
