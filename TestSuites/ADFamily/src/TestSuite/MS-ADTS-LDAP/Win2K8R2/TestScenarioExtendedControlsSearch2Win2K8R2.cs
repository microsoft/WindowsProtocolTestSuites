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
    public partial class TestScenarioExtendedControlsSearch2Win2K8R2 : PtfTestClassBase
    {

        public TestScenarioExtendedControlsSearch2Win2K8R2()
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S0()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=container)\",Subtree,[\"" +
                    "sAMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaUsed\"" +
                    ",\"msDS-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_SEARCH_OPTIONS_OID,AD_DS)" +
                    "\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=container)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_SEARCH_OPTIONS_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
            this.Manager.EndTest();
        }

        private void TestScenarioExtendedControlsSearch2Win2K8R2S84()
        {
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=user)\",Subtree,[\"sAMAc" +
                    "countName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaUsed\",\"msD" +
                    "S-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID,AD_D" +
                    "S)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=user)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=user)\",Subtree,[\"sAMAc" +
                    "countName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaUsed\",\"msD" +
                    "S-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_SHOW_DEACTIVATED_LINK_OIDWithV" +
                    "alue,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=user)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_SHOW_DEACTIVATED_LINK_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S88\'");
            int temp0 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TestScenarioExtendedControlsSearch2Win2K8R2.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioExtendedControlsSearch2Win2K8R2S0SearchOpResponseChecker)), new ExpectedEvent(TestScenarioExtendedControlsSearch2Win2K8R2.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioExtendedControlsSearch2Win2K8R2S0SearchOpResponseChecker1)));
            if ((temp0 == 0))
            {
                this.Manager.Comment("reaching state \'S89\'");
                this.Manager.Comment("executing step \'call UnBind()\'");
                this.IAD_LDAPModelAdapterInstance.UnBind();
                this.Manager.Comment("reaching state \'S91\'");
                this.Manager.Comment("checking step \'return UnBind\'");
                this.Manager.Comment("reaching state \'S93\'");
                goto label0;
            }
            if ((temp0 == 1))
            {
                this.Manager.Comment("reaching state \'S90\'");
                this.Manager.Comment("executing step \'call UnBind()\'");
                this.IAD_LDAPModelAdapterInstance.UnBind();
                this.Manager.Comment("reaching state \'S92\'");
                this.Manager.Comment("checking step \'return UnBind\'");
                this.Manager.Comment("reaching state \'S94\'");
                goto label0;
            }
            this.Manager.CheckObservationTimeout(false, new ExpectedEvent(TestScenarioExtendedControlsSearch2Win2K8R2.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioExtendedControlsSearch2Win2K8R2S0SearchOpResponseChecker)), new ExpectedEvent(TestScenarioExtendedControlsSearch2Win2K8R2.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioExtendedControlsSearch2Win2K8R2S0SearchOpResponseChecker1)));
        label0:
            ;
        }

        private void TestScenarioExtendedControlsSearch2Win2K8R2S0SearchOpResponseChecker(SearchResp response)
        {
            this.Manager.Comment("checking step \'event SearchOpResponse(retreivalUnsuccessful)\'");
            TestManagerHelpers.AssertAreEqual<SearchResp>(this.Manager, ((SearchResp)(1)), response, "response of SearchOpResponse, state S88");
        }

        private void TestScenarioExtendedControlsSearch2Win2K8R2S0SearchOpResponseChecker1(SearchResp response)
        {
            this.Manager.Comment("checking step \'event SearchOpResponse(retrievalSuccessful)\'");
            TestManagerHelpers.AssertAreEqual<SearchResp>(this.Manager, ((SearchResp)(0)), response, "response of SearchOpResponse, state S88");
        }
        #endregion

        #region Test Starting in S10
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
		[TestCategory("PDC")]
		[TestCategory("DomainWin2008R2")]
		[TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S10()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=user)\",Subtree,[\"sAMAc" +
                    "countName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaUsed\",\"msD" +
                    "S-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_EXTENDED_DN_OID,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=user)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_EXTENDED_DN_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S12()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=msDS-QuotaContainer)\"," +
                    "Subtree,[\"sAMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-" +
                    "QuotaUsed\",\"msDS-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_QUOTA_CONTROL_O" +
                    "ID,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=msDS-QuotaContainer)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_QUOTA_CONTROL_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S14()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=computer)\",Subtree,[\"s" +
                    "AMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaUsed\"," +
                    "\"msDS-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_INPUT_DN_OID,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=computer)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_DN_INPUT_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S16()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(&(isDeleted=TRUE)(objectClass=user" +
                    "))\",Subtree,[\"sAMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"m" +
                    "sDS-QuotaUsed\",\"msDS-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_SHOW_DELETE" +
                    "D_OID,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(&(isDeleted=TRUE)(objectClass=user))", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_SHOW_DELETED_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S18()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=user)\",Subtree,[\"sAMAc" +
                    "countName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaUsed\",\"msD" +
                    "S-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_VERIFY_NAME_OID,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=user)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_VERIFY_NAME_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S2()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=container)\",Subtree,[\"" +
                    "sAMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaUsed\"" +
                    ",\"msDS-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_SHUTDOWN_NOTIFY_OID,AD_DS" +
                    ")\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=container)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_SHUTDOWN_NOTIFY_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S20()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=user)\",Subtree,[\"sAMAc" +
                    "countName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaUsed\",\"msD" +
                    "S-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_FORCE_UPDATE_OID,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=user)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_FORCE_UPDATE_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S22()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=user)\",Subtree,[\"sAMAc" +
                    "countName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaUsed\",\"msD" +
                    "S-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_SHUTDOWN_NOTIFY_OID,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=user)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_SHUTDOWN_NOTIFY_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S24()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=msDS-QuotaContainer)\"," +
                    "Subtree,[\"sAMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-" +
                    "QuotaUsed\",\"msDS-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_VERIFY_NAME_OID" +
                    ",AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=msDS-QuotaContainer)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_VERIFY_NAME_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S26()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=computer)\",Subtree,[\"s" +
                    "AMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaUsed\"," +
                    "\"msDS-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_VERIFY_NAME_OID,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=computer)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_VERIFY_NAME_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S28()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(&(isDeleted=TRUE)(objectClass=user" +
                    "))\",Subtree,[\"sAMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"m" +
                    "sDS-QuotaUsed\",\"msDS-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_VERIFY_NAME" +
                    "_OID,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(&(isDeleted=TRUE)(objectClass=user))", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_VERIFY_NAME_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S30()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=msDS-QuotaContainer)\"," +
                    "Subtree,[\"sAMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-" +
                    "QuotaUsed\",\"msDS-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_SHUTDOWN_NOTIFY" +
                    "_OID,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=msDS-QuotaContainer)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_SHUTDOWN_NOTIFY_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S32()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=msDS-QuotaContainer)\"," +
                    "Subtree,[\"sAMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-" +
                    "QuotaUsed\",\"msDS-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_FORCE_UPDATE_OI" +
                    "D,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=msDS-QuotaContainer)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_FORCE_UPDATE_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S34()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=computer)\",Subtree,[\"s" +
                    "AMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaUsed\"," +
                    "\"msDS-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_SHUTDOWN_NOTIFY_OID,AD_DS)" +
                    "\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=computer)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_SHUTDOWN_NOTIFY_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S36()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment(@"executing step 'call SearchOpReq(""DC=adts88"",""(&(isDeleted=TRUE)(objectClass=user))"",Subtree,[""sAMAccountName"",""cn"",""ntSecurityDescriptor"",""member;range=10-0"",""msDS-QuotaUsed"",""msDS-QuotaEffective"",""member;range=1-0""],LDAP_SERVER_SHUTDOWN_NOTIFY_OID,AD_DS)'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(&(isDeleted=TRUE)(objectClass=user))", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_SHUTDOWN_NOTIFY_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S38()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=computer)\",Subtree,[\"s" +
                    "AMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaUsed\"," +
                    "\"msDS-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_FORCE_UPDATE_OID,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=computer)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_FORCE_UPDATE_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S4()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=container)\",Subtree,[\"" +
                    "sAMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaUsed\"" +
                    ",\"msDS-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_VERIFY_NAME_OID,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=container)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_VERIFY_NAME_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S40()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(&(isDeleted=TRUE)(objectClass=user" +
                    "))\",Subtree,[\"sAMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"m" +
                    "sDS-QuotaUsed\",\"msDS-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_FORCE_UPDAT" +
                    "E_OID,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(&(isDeleted=TRUE)(objectClass=user))", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_FORCE_UPDATE_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S6()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=container)\",Subtree,[\"" +
                    "sAMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaUsed\"" +
                    ",\"msDS-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_FORCE_UPDATE_OID,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=container)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_FORCE_UPDATE_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
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
        public void LDAP_TestScenarioExtendedControlsSearch2Win2K8R2S8()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch2Win2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"DC=adts88\",\"(objectClass=container)\",Subtree,[\"" +
                    "sAMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaUsed\"" +
                    ",\"msDS-QuotaEffective\",\"member;range=1-0\"],LDAP_SERVER_DOMAIN_SCOPE_OID,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("DC=adts88", "(objectClass=container)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "sAMAccountName",
                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                    "Head",
                                                    "Tail"}, new object[] {
                                                    "cn",
                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                "Head",
                                                                "Tail"}, new object[] {
                                                                "ntSecurityDescriptor",
                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                            "Head",
                                                                            "Tail"}, new object[] {
                                                                            "member;range=10-0",
                                                                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                        "Head",
                                                                                        "Tail"}, new object[] {
                                                                                        "msDS-QuotaUsed",
                                                                                        this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                    "Head",
                                                                                                    "Tail"}, new object[] {
                                                                                                    "msDS-QuotaEffective",
                                                                                                    this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                "Head",
                                                                                                                "Tail"}, new object[] {
                                                                                                                "member;range=1-0",
                                                                                                                ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})}), ExtendedControl.LDAP_SERVER_DOMAIN_SCOPE_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch2Win2K8R2S84();
            this.Manager.EndTest();
        }
        #endregion
    }
}
