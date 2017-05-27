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
    public partial class TestScenarioSearchAD_DSWin2K8R2 : PtfTestClassBase
    {

        public TestScenarioSearchAD_DSWin2K8R2()
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
        public void LDAP_TestScenarioSearchAD_DSWin2K8R2S0()
        {
            this.Manager.BeginTest("TestScenarioSearchAD_DSWin2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=adts_user1,CN=Users,DC=adts88\",\"objectClass:" +
                    " user\",WholeTree,[\"objectSid\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=adts_user1,CN=Users,DC=adts88", "objectClass: user", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "objectSid",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Account,CN=Schema,CN=Configuration,DC=adts88" +
                    "\",\"objectClass: classSchema\",WholeTree,[\"objectSid\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Account,CN=Schema,CN=Configuration,DC=adts88", "objectClass: classSchema", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "objectSid",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=S-1-5-4,CN=ForeignSecurityPrincipals,DC=adts" +
                    "88\",\"objectClass: foreignSecurityPrincipal\",WholeTree,[\"objectSid\"],NoExtendedCo" +
                    "ntrol,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=S-1-5-4,CN=ForeignSecurityPrincipals,DC=adts88", "objectClass: foreignSecurityPrincipal", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "objectSid",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"objectCategory: contact\",WholeTree," +
                    "[\"sAMAccountName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "objectCategory: contact", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Computers,DC=adts88\",\"objectClass: computer\"" +
                    ",WholeTree,[\"dNSHostName\",\"sAMAccountName\",\"msDS-AdditionalDnsHostName\",\"objectS" +
                    "id\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Computers,DC=adts88", "objectClass: computer", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "dNSHostName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "sAMAccountName",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "msDS-AdditionalDnsHostName",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "objectSid",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Users,DC=adts88\",\"(&(objectCategory=person)(" +
                    "objectClass=user)(!userAccountControl:1.2.840.113556.1.4.803:=2))\",WholeTree,[\"s" +
                    "AMAccountName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Users,DC=adts88", "(&(objectCategory=person)(objectClass=user)(!userAccountControl:1.2.840.113556.1." +
                    "4.803:=2))", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Users,DC=adts88\",\"(&(objectCategory=person)(" +
                    "objectClass=user)(!userAccountControl:1.2.840.113556.1.4.804:=2))\",WholeTree,[\"s" +
                    "AMAccountName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Users,DC=adts88", "(&(objectCategory=person)(objectClass=user)(!userAccountControl:1.2.840.113556.1." +
                    "4.804:=2))", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Schema,CN=Configuration,DC=adts88\",\"(&(objec" +
                    "tClass=attributeSchema)(systemFlags:1.2.840.113556.1.4.804:=4))\",WholeTree,[\"lDA" +
                    "PDisplayName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Schema,CN=Configuration,DC=adts88", "(&(objectClass=attributeSchema)(systemFlags:1.2.840.113556.1.4.804:=4))", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "lDAPDisplayName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(memberOf:1.2.840.113556.1.4.1941:=" +
                    "CN=Enterprise Admins,CN=Users,DC=adts88)\",WholeTree,[\"sAMAccountName\"],NoExtende" +
                    "dControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(memberOf:1.2.840.113556.1.4.1941:=CN=Enterprise Admins,CN=Users,DC=adts88)", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=ADTS88,CN=Partitions,CN=Configuration,DC=adt" +
                    "s88\",\"objectClass: crossRef\",WholeTree,[\"nCName\",\"dnsRoot\",\"Enabled\"],NoExtended" +
                    "Control,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=ADTS88,CN=Partitions,CN=Configuration,DC=adts88", "objectClass: crossRef", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "nCName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "dnsRoot",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "Enabled",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S110\'");
            ConstrOnAddOpErrs temp0;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: CustomCrossRef"",""Enabled: FALSE"",""dnsRoot: CustomCrossRef.adts88"",""nCName: DC=CustomCrossRef,DC=custom,DC=com"",""objectClass: crossRef"",""distinguishedName: CN=CustomCrossRef,CN=Partitions,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,NonWindows,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: CustomCrossRef",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "Enabled: FALSE",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "dnsRoot: CustomCrossRef.adts88",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "nCName: DC=CustomCrossRef,DC=custom,DC=com",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "objectClass: crossRef",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "distinguishedName: CN=CustomCrossRef,CN=Partitions,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.NonWin, null, ((ADImplementations)(0)), false, out temp0);
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
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp0, "errorStatus of AddOperation, state S115");
            TestScenarioSearchAD_DSWin2K8R2S120();
            this.Manager.EndTest();
        }

        private void TestScenarioSearchAD_DSWin2K8R2S120()
        {
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=CustomCrossRef,CN=Partitions,CN=Configuratio" +
                    "n,DC=adts88\",\"objectClass=crossRef\",WholeTree,[\"nCName\",\"Enabled\",\"dnsRoot\"],NoE" +
                    "xtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=CustomCrossRef,CN=Partitions,CN=Configuration,DC=adts88", "objectClass=crossRef", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "nCName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "Enabled",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "dnsRoot",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Administrators,CN=Builtin,DC=adts88\",\"(membe" +
                    "r: *)\",WholeTree,[\"(member;range=0-2)\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Administrators,CN=Builtin,DC=adts88", "(member: *)", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "(member;range=0-2)",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S132\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"<WKGUID=a9d1ca15768811d1aded00c04fd8d5cd,DC=adt" +
                    "s88>\",\"(objectClass=user)\",WholeTree,[\"distinguishedName\"],NoExtendedControl,AD_" +
                    "DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("<WKGUID=a9d1ca15768811d1aded00c04fd8d5cd,DC=adts88>", "(objectClass=user)", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "distinguishedName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S135\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioSearchAD_DSWin2K8R2S138();
        }

        private void TestScenarioSearchAD_DSWin2K8R2S138()
        {
            this.Manager.Comment("reaching state \'S138\'");
            int temp1 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TestScenarioSearchAD_DSWin2K8R2.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioSearchAD_DSWin2K8R2S0SearchOpResponseChecker)), new ExpectedEvent(TestScenarioSearchAD_DSWin2K8R2.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioSearchAD_DSWin2K8R2S0SearchOpResponseChecker1)));
            if ((temp1 == 0))
            {
                this.Manager.Comment("reaching state \'S139\'");
                this.Manager.Comment("executing step \'call UnBind()\'");
                this.IAD_LDAPModelAdapterInstance.UnBind();
                this.Manager.Comment("reaching state \'S141\'");
                this.Manager.Comment("checking step \'return UnBind\'");
                this.Manager.Comment("reaching state \'S143\'");
                goto label0;
            }
            if ((temp1 == 1))
            {
                this.Manager.Comment("reaching state \'S140\'");
                this.Manager.Comment("executing step \'call UnBind()\'");
                this.IAD_LDAPModelAdapterInstance.UnBind();
                this.Manager.Comment("reaching state \'S142\'");
                this.Manager.Comment("checking step \'return UnBind\'");
                this.Manager.Comment("reaching state \'S144\'");
                goto label0;
            }
            this.Manager.CheckObservationTimeout(false, new ExpectedEvent(TestScenarioSearchAD_DSWin2K8R2.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioSearchAD_DSWin2K8R2S0SearchOpResponseChecker)), new ExpectedEvent(TestScenarioSearchAD_DSWin2K8R2.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioSearchAD_DSWin2K8R2S0SearchOpResponseChecker1)));
        label0:
            ;
        }

        private void TestScenarioSearchAD_DSWin2K8R2S0SearchOpResponseChecker(SearchResp response)
        {
            this.Manager.Comment("checking step \'event SearchOpResponse(retreivalUnsuccessful)\'");
            TestManagerHelpers.AssertAreEqual<SearchResp>(this.Manager, ((SearchResp)(1)), response, "response of SearchOpResponse, state S138");
        }

        private void TestScenarioSearchAD_DSWin2K8R2S0SearchOpResponseChecker1(SearchResp response)
        {
            this.Manager.Comment("checking step \'event SearchOpResponse(retrievalSuccessful)\'");
            TestManagerHelpers.AssertAreEqual<SearchResp>(this.Manager, ((SearchResp)(0)), response, "response of SearchOpResponse, state S138");
        }
        #endregion

        #region Test Starting in S2
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioSearchAD_DSWin2K8R2S2()
        {
            this.Manager.BeginTest("TestScenarioSearchAD_DSWin2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=adts_user1,CN=Users,DC=adts88\",\"objectClass:" +
                    " user\",WholeTree,[\"objectSid\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=adts_user1,CN=Users,DC=adts88", "objectClass: user", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "objectSid",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Account,CN=Schema,CN=Configuration,DC=adts88" +
                    "\",\"objectClass: classSchema\",WholeTree,[\"objectSid\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Account,CN=Schema,CN=Configuration,DC=adts88", "objectClass: classSchema", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "objectSid",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=S-1-5-4,CN=ForeignSecurityPrincipals,DC=adts" +
                    "88\",\"objectClass: foreignSecurityPrincipal\",WholeTree,[\"objectSid\"],NoExtendedCo" +
                    "ntrol,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=S-1-5-4,CN=ForeignSecurityPrincipals,DC=adts88", "objectClass: foreignSecurityPrincipal", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "objectSid",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"objectCategory: contact\",WholeTree," +
                    "[\"sAMAccountName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "objectCategory: contact", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Computers,DC=adts88\",\"objectClass: computer\"" +
                    ",WholeTree,[\"dNSHostName\",\"sAMAccountName\",\"msDS-AdditionalDnsHostName\",\"objectS" +
                    "id\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Computers,DC=adts88", "objectClass: computer", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "dNSHostName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "sAMAccountName",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "msDS-AdditionalDnsHostName",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "objectSid",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Users,DC=adts88\",\"(&(objectCategory=person)(" +
                    "objectClass=user)(!userAccountControl:1.2.840.113556.1.4.803:=2))\",WholeTree,[\"s" +
                    "AMAccountName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Users,DC=adts88", "(&(objectCategory=person)(objectClass=user)(!userAccountControl:1.2.840.113556.1." +
                    "4.803:=2))", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Users,DC=adts88\",\"(&(objectCategory=person)(" +
                    "objectClass=user)(!userAccountControl:1.2.840.113556.1.4.804:=2))\",WholeTree,[\"s" +
                    "AMAccountName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Users,DC=adts88", "(&(objectCategory=person)(objectClass=user)(!userAccountControl:1.2.840.113556.1." +
                    "4.804:=2))", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Schema,CN=Configuration,DC=adts88\",\"(&(objec" +
                    "tClass=attributeSchema)(systemFlags:1.2.840.113556.1.4.804:=4))\",WholeTree,[\"lDA" +
                    "PDisplayName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Schema,CN=Configuration,DC=adts88", "(&(objectClass=attributeSchema)(systemFlags:1.2.840.113556.1.4.804:=4))", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "lDAPDisplayName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(memberOf:1.2.840.113556.1.4.1941:=" +
                    "CN=Enterprise Admins,CN=Users,DC=adts88)\",WholeTree,[\"sAMAccountName\"],NoExtende" +
                    "dControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(memberOf:1.2.840.113556.1.4.1941:=CN=Enterprise Admins,CN=Users,DC=adts88)", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=ADTS88,CN=Partitions,CN=Configuration,DC=adt" +
                    "s88\",\"objectClass: crossRef\",WholeTree,[\"nCName\",\"dnsRoot\",\"Enabled\"],NoExtended" +
                    "Control,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=ADTS88,CN=Partitions,CN=Configuration,DC=adts88", "objectClass: crossRef", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "nCName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "dnsRoot",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "Enabled",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S111\'");
            ConstrOnAddOpErrs temp2;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: CustomCrossRef"",""Enabled: FALSE"",""dnsRoot: CustomCrossRef.adts88"",""nCName: DC=CustomCrossRef,DC=custom,DC=com"",""objectClass: crossRef"",""distinguishedName: CN=CustomCrossRef,CN=Partitions,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: CustomCrossRef",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "Enabled: FALSE",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "dnsRoot: CustomCrossRef.adts88",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "nCName: DC=CustomCrossRef,DC=custom,DC=com",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "objectClass: crossRef",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "distinguishedName: CN=CustomCrossRef,CN=Partitions,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ((ServerVersion)(0)), null, ((ADImplementations)(0)), false, out temp2);
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
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp2, "errorStatus of AddOperation, state S116");
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=CustomCrossRef,CN=Partitions,CN=Configuratio" +
                    "n,DC=adts88\",\"objectClass=crossRef\",WholeTree,[\"nCName\",\"Enabled\",\"dnsRoot\"],NoE" +
                    "xtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=CustomCrossRef,CN=Partitions,CN=Configuration,DC=adts88", "objectClass=crossRef", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "nCName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "Enabled",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "dnsRoot",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Administrators,CN=Builtin,DC=adts88\",\"(membe" +
                    "r: *)\",WholeTree,[\"(member;range=0-2)\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Administrators,CN=Builtin,DC=adts88", "(member: *)", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "(member;range=0-2)",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S133\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"<GUID=a5127683905336458dc57f180a0adf16>\",\"(obje" +
                    "ctClass=user)\",WholeTree,[\"distinguishedName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("<GUID=a5127683905336458dc57f180a0adf16>", "(objectClass=user)", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "distinguishedName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioSearchAD_DSWin2K8R2S138();
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
        public void LDAP_TestScenarioSearchAD_DSWin2K8R2S4()
        {
            this.Manager.BeginTest("TestScenarioSearchAD_DSWin2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=adts_user1,CN=Users,DC=adts88\",\"objectClass:" +
                    " user\",WholeTree,[\"objectSid\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=adts_user1,CN=Users,DC=adts88", "objectClass: user", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "objectSid",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Account,CN=Schema,CN=Configuration,DC=adts88" +
                    "\",\"objectClass: classSchema\",WholeTree,[\"objectSid\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Account,CN=Schema,CN=Configuration,DC=adts88", "objectClass: classSchema", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "objectSid",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=S-1-5-4,CN=ForeignSecurityPrincipals,DC=adts" +
                    "88\",\"objectClass: foreignSecurityPrincipal\",WholeTree,[\"objectSid\"],NoExtendedCo" +
                    "ntrol,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=S-1-5-4,CN=ForeignSecurityPrincipals,DC=adts88", "objectClass: foreignSecurityPrincipal", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "objectSid",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"objectCategory: contact\",WholeTree," +
                    "[\"sAMAccountName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "objectCategory: contact", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Computers,DC=adts88\",\"objectClass: computer\"" +
                    ",WholeTree,[\"dNSHostName\",\"sAMAccountName\",\"msDS-AdditionalDnsHostName\",\"objectS" +
                    "id\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Computers,DC=adts88", "objectClass: computer", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "dNSHostName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "sAMAccountName",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "msDS-AdditionalDnsHostName",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "objectSid",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Users,DC=adts88\",\"(&(objectCategory=person)(" +
                    "objectClass=user)(!userAccountControl:1.2.840.113556.1.4.803:=2))\",WholeTree,[\"s" +
                    "AMAccountName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Users,DC=adts88", "(&(objectCategory=person)(objectClass=user)(!userAccountControl:1.2.840.113556.1." +
                    "4.803:=2))", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Users,DC=adts88\",\"(&(objectCategory=person)(" +
                    "objectClass=user)(!userAccountControl:1.2.840.113556.1.4.804:=2))\",WholeTree,[\"s" +
                    "AMAccountName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Users,DC=adts88", "(&(objectCategory=person)(objectClass=user)(!userAccountControl:1.2.840.113556.1." +
                    "4.804:=2))", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Schema,CN=Configuration,DC=adts88\",\"(&(objec" +
                    "tClass=attributeSchema)(systemFlags:1.2.840.113556.1.4.804:=4))\",WholeTree,[\"lDA" +
                    "PDisplayName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Schema,CN=Configuration,DC=adts88", "(&(objectClass=attributeSchema)(systemFlags:1.2.840.113556.1.4.804:=4))", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "lDAPDisplayName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(memberOf:1.2.840.113556.1.4.1941:=" +
                    "CN=Enterprise Admins,CN=Users,DC=adts88)\",WholeTree,[\"sAMAccountName\"],NoExtende" +
                    "dControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(memberOf:1.2.840.113556.1.4.1941:=CN=Enterprise Admins,CN=Users,DC=adts88)", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=ADTS88,CN=Partitions,CN=Configuration,DC=adt" +
                    "s88\",\"objectClass: crossRef\",WholeTree,[\"nCName\",\"dnsRoot\",\"Enabled\"],NoExtended" +
                    "Control,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=ADTS88,CN=Partitions,CN=Configuration,DC=adts88", "objectClass: crossRef", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "nCName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "dnsRoot",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "Enabled",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S112\'");
            ConstrOnAddOpErrs temp3;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: CustomCrossRef"",""Enabled: FALSE"",""dnsRoot: CustomCrossRef.adts88"",""nCName: DC=CustomCrossRef,DC=custom,DC=com"",""objectClass: crossRef"",""distinguishedName: CN=CustomCrossRef,CN=Partitions,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K3,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: CustomCrossRef",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "Enabled: FALSE",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "dnsRoot: CustomCrossRef.adts88",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "nCName: DC=CustomCrossRef,DC=custom,DC=com",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "objectClass: crossRef",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "distinguishedName: CN=CustomCrossRef,CN=Partitions,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ((ServerVersion)(1)), null, ((ADImplementations)(0)), false, out temp3);
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
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp3, "errorStatus of AddOperation, state S117");
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=CustomCrossRef,CN=Partitions,CN=Configuratio" +
                    "n,DC=adts88\",\"objectClass=crossRef\",WholeTree,[\"nCName\",\"Enabled\",\"dnsRoot\"],NoE" +
                    "xtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=CustomCrossRef,CN=Partitions,CN=Configuration,DC=adts88", "objectClass=crossRef", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "nCName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "Enabled",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "dnsRoot",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Administrators,CN=Builtin,DC=adts88\",\"(membe" +
                    "r: *)\",WholeTree,[\"(member;range=0-2)\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Administrators,CN=Builtin,DC=adts88", "(member: *)", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "(member;range=0-2)",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S134\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"<SID=S-1-5-21-126476475-3050469762-4276591986-1" +
                    "104>\",\"(objectClass=user)\",WholeTree,[\"distinguishedName\"],NoExtendedControl,AD_" +
                    "DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("<SID=S-1-5-21-126476475-3050469762-4276591986-1104>", "(objectClass=user)", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "distinguishedName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S137\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioSearchAD_DSWin2K8R2S138();
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
        public void LDAP_TestScenarioSearchAD_DSWin2K8R2S6()
        {
            this.Manager.BeginTest("TestScenarioSearchAD_DSWin2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=adts_user1,CN=Users,DC=adts88\",\"objectClass:" +
                    " user\",WholeTree,[\"objectSid\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=adts_user1,CN=Users,DC=adts88", "objectClass: user", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "objectSid",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Account,CN=Schema,CN=Configuration,DC=adts88" +
                    "\",\"objectClass: classSchema\",WholeTree,[\"objectSid\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Account,CN=Schema,CN=Configuration,DC=adts88", "objectClass: classSchema", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "objectSid",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=S-1-5-4,CN=ForeignSecurityPrincipals,DC=adts" +
                    "88\",\"objectClass: foreignSecurityPrincipal\",WholeTree,[\"objectSid\"],NoExtendedCo" +
                    "ntrol,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=S-1-5-4,CN=ForeignSecurityPrincipals,DC=adts88", "objectClass: foreignSecurityPrincipal", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "objectSid",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"objectCategory: contact\",WholeTree," +
                    "[\"sAMAccountName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "objectCategory: contact", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Computers,DC=adts88\",\"objectClass: computer\"" +
                    ",WholeTree,[\"dNSHostName\",\"sAMAccountName\",\"msDS-AdditionalDnsHostName\",\"objectS" +
                    "id\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Computers,DC=adts88", "objectClass: computer", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "dNSHostName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "sAMAccountName",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "msDS-AdditionalDnsHostName",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "objectSid",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Users,DC=adts88\",\"(&(objectCategory=person)(" +
                    "objectClass=user)(!userAccountControl:1.2.840.113556.1.4.803:=2))\",WholeTree,[\"s" +
                    "AMAccountName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Users,DC=adts88", "(&(objectCategory=person)(objectClass=user)(!userAccountControl:1.2.840.113556.1." +
                    "4.803:=2))", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Users,DC=adts88\",\"(&(objectCategory=person)(" +
                    "objectClass=user)(!userAccountControl:1.2.840.113556.1.4.804:=2))\",WholeTree,[\"s" +
                    "AMAccountName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Users,DC=adts88", "(&(objectCategory=person)(objectClass=user)(!userAccountControl:1.2.840.113556.1." +
                    "4.804:=2))", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Schema,CN=Configuration,DC=adts88\",\"(&(objec" +
                    "tClass=attributeSchema)(systemFlags:1.2.840.113556.1.4.804:=4))\",WholeTree,[\"lDA" +
                    "PDisplayName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Schema,CN=Configuration,DC=adts88", "(&(objectClass=attributeSchema)(systemFlags:1.2.840.113556.1.4.804:=4))", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "lDAPDisplayName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(memberOf:1.2.840.113556.1.4.1941:=" +
                    "CN=Enterprise Admins,CN=Users,DC=adts88)\",WholeTree,[\"sAMAccountName\"],NoExtende" +
                    "dControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(memberOf:1.2.840.113556.1.4.1941:=CN=Enterprise Admins,CN=Users,DC=adts88)", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=ADTS88,CN=Partitions,CN=Configuration,DC=adt" +
                    "s88\",\"objectClass: crossRef\",WholeTree,[\"nCName\",\"dnsRoot\",\"Enabled\"],NoExtended" +
                    "Control,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=ADTS88,CN=Partitions,CN=Configuration,DC=adts88", "objectClass: crossRef", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "nCName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "dnsRoot",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "Enabled",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S113\'");
            ConstrOnAddOpErrs temp4;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: CustomCrossRef"",""Enabled: FALSE"",""dnsRoot: CustomCrossRef.adts88"",""nCName: DC=CustomCrossRef,DC=custom,DC=com"",""objectClass: crossRef"",""distinguishedName: CN=CustomCrossRef,CN=Partitions,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: CustomCrossRef",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "Enabled: FALSE",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "dnsRoot: CustomCrossRef.adts88",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "nCName: DC=CustomCrossRef,DC=custom,DC=com",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "objectClass: crossRef",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "distinguishedName: CN=CustomCrossRef,CN=Partitions,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008, null, ((ADImplementations)(0)), false, out temp4);
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
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp4, "errorStatus of AddOperation, state S118");
            TestScenarioSearchAD_DSWin2K8R2S120();
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
        public void LDAP_TestScenarioSearchAD_DSWin2K8R2S8()
        {
            this.Manager.BeginTest("TestScenarioSearchAD_DSWin2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=adts_user1,CN=Users,DC=adts88\",\"objectClass:" +
                    " user\",WholeTree,[\"objectSid\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=adts_user1,CN=Users,DC=adts88", "objectClass: user", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "objectSid",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Account,CN=Schema,CN=Configuration,DC=adts88" +
                    "\",\"objectClass: classSchema\",WholeTree,[\"objectSid\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Account,CN=Schema,CN=Configuration,DC=adts88", "objectClass: classSchema", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "objectSid",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=S-1-5-4,CN=ForeignSecurityPrincipals,DC=adts" +
                    "88\",\"objectClass: foreignSecurityPrincipal\",WholeTree,[\"objectSid\"],NoExtendedCo" +
                    "ntrol,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=S-1-5-4,CN=ForeignSecurityPrincipals,DC=adts88", "objectClass: foreignSecurityPrincipal", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "objectSid",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"objectCategory: contact\",WholeTree," +
                    "[\"sAMAccountName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "objectCategory: contact", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Computers,DC=adts88\",\"objectClass: computer\"" +
                    ",WholeTree,[\"dNSHostName\",\"sAMAccountName\",\"msDS-AdditionalDnsHostName\",\"objectS" +
                    "id\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Computers,DC=adts88", "objectClass: computer", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "dNSHostName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "sAMAccountName",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "msDS-AdditionalDnsHostName",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "objectSid",
                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Users,DC=adts88\",\"(&(objectCategory=person)(" +
                    "objectClass=user)(!userAccountControl:1.2.840.113556.1.4.803:=2))\",WholeTree,[\"s" +
                    "AMAccountName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Users,DC=adts88", "(&(objectCategory=person)(objectClass=user)(!userAccountControl:1.2.840.113556.1." +
                    "4.803:=2))", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Users,DC=adts88\",\"(&(objectCategory=person)(" +
                    "objectClass=user)(!userAccountControl:1.2.840.113556.1.4.804:=2))\",WholeTree,[\"s" +
                    "AMAccountName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Users,DC=adts88", "(&(objectCategory=person)(objectClass=user)(!userAccountControl:1.2.840.113556.1." +
                    "4.804:=2))", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Schema,CN=Configuration,DC=adts88\",\"(&(objec" +
                    "tClass=attributeSchema)(systemFlags:1.2.840.113556.1.4.804:=4))\",WholeTree,[\"lDA" +
                    "PDisplayName\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Schema,CN=Configuration,DC=adts88", "(&(objectClass=attributeSchema)(systemFlags:1.2.840.113556.1.4.804:=4))", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "lDAPDisplayName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(memberOf:1.2.840.113556.1.4.1941:=" +
                    "CN=Enterprise Admins,CN=Users,DC=adts88)\",WholeTree,[\"sAMAccountName\"],NoExtende" +
                    "dControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(memberOf:1.2.840.113556.1.4.1941:=CN=Enterprise Admins,CN=Users,DC=adts88)", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=ADTS88,CN=Partitions,CN=Configuration,DC=adt" +
                    "s88\",\"objectClass: crossRef\",WholeTree,[\"nCName\",\"dnsRoot\",\"Enabled\"],NoExtended" +
                    "Control,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=ADTS88,CN=Partitions,CN=Configuration,DC=adts88", "objectClass: crossRef", SearchScope.WholeTree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "nCName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "dnsRoot",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "Enabled",
                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S114\'");
            ConstrOnAddOpErrs temp5;
            this.Manager.Comment(@"executing step 'call AddOperation([""cn: CustomCrossRef"",""Enabled: FALSE"",""dnsRoot: CustomCrossRef.adts88"",""nCName: DC=CustomCrossRef,DC=custom,DC=com"",""objectClass: crossRef"",""distinguishedName: CN=CustomCrossRef,CN=Partitions,CN=Configuration,DC=adts88""],RIGHT_DS_CREATE_CHILDwithSE_ENABLE_DELEGATION_PRIVILEGE,RIGHT_DS_ADD_GUID,Windows2K8R2,NoExtendedControl,AD_DS,False,out _)'");
            this.IAD_LDAPModelAdapterInstance.AddOperation(this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "cn: CustomCrossRef",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "Enabled: FALSE",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "dnsRoot: CustomCrossRef.adts88",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "nCName: DC=CustomCrossRef,DC=custom,DC=com",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "objectClass: crossRef",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "distinguishedName: CN=CustomCrossRef,CN=Partitions,CN=Configuration,DC=adts88",
                                                                                                    ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})}), ((RightsOnParentObjects)(0)), ((NCRight)(0)), ServerVersion.Win2008R2, null, ((ADImplementations)(0)), false, out temp5);
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
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return AddOperation/[out success]\'");
            TestManagerHelpers.AssertAreEqual<ConstrOnAddOpErrs>(this.Manager, ConstrOnAddOpErrs.success, temp5, "errorStatus of AddOperation, state S119");
            TestScenarioSearchAD_DSWin2K8R2S120();
            this.Manager.EndTest();
        }
        #endregion
    }
}
