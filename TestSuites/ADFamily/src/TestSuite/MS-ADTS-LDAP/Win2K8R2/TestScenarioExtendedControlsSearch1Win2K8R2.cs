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
    public partial class TestScenarioExtendedControlsSearch1Win2K8R2 : PtfTestClassBase
    {

        public TestScenarioExtendedControlsSearch1Win2K8R2()
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
        public void LDAP_TestScenarioExtendedControlsSearch1Win2K8R2S0()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch1Win2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Users,DC=adts88\",\"(objectClass=user)\",Subtre" +
                    "e,[\"sAMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaU" +
                    "sed\",\"msDS-QuotaEffective\",\"member;range=1-0\",\"controlValue\"],OffsetRangeError,A" +
                    "D_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Users,DC=adts88", "(objectClass=user)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
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
                                                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                            "Head",
                                                                                                                            "Tail"}, new object[] {
                                                                                                                            "controlValue",
                                                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})})}), ExtendedControl.OffsetRangeError, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch1Win2K8R2S24();
            this.Manager.EndTest();
        }

        private void TestScenarioExtendedControlsSearch1Win2K8R2S24()
        {
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment(@"executing step 'call SearchOpReq(""CN=Users,DC=adts88"",""(objectClass=user)"",Subtree,[""sAMAccountName"",""cn"",""ntSecurityDescriptor"",""member;range=10-0"",""msDS-QuotaUsed"",""msDS-QuotaEffective"",""member;range=1-0"",""controlValue""],LDAP_SERVER_GET_STATS_OIDWithSO_EXTENDED_FMT,AD_DS)'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Users,DC=adts88", "(objectClass=user)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
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
                                                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                            "Head",
                                                                                                                            "Tail"}, new object[] {
                                                                                                                            "controlValue",
                                                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})})}), ExtendedControl.LDAP_SERVER_GET_STATS_OIDWithSO_EXTENDED_FMT, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            this.Manager.Comment("reaching state \'S26\'");
            int temp0 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TestScenarioExtendedControlsSearch1Win2K8R2.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioExtendedControlsSearch1Win2K8R2S0SearchOpResponseChecker)), new ExpectedEvent(TestScenarioExtendedControlsSearch1Win2K8R2.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioExtendedControlsSearch1Win2K8R2S0SearchOpResponseChecker1)));
            if ((temp0 == 0))
            {
                this.Manager.Comment("reaching state \'S27\'");
                this.Manager.Comment("executing step \'call UnBind()\'");
                this.IAD_LDAPModelAdapterInstance.UnBind();
                this.Manager.Comment("reaching state \'S29\'");
                this.Manager.Comment("checking step \'return UnBind\'");
                this.Manager.Comment("reaching state \'S31\'");
                goto label0;
            }
            if ((temp0 == 1))
            {
                this.Manager.Comment("reaching state \'S28\'");
                this.Manager.Comment("executing step \'call UnBind()\'");
                this.IAD_LDAPModelAdapterInstance.UnBind();
                this.Manager.Comment("reaching state \'S30\'");
                this.Manager.Comment("checking step \'return UnBind\'");
                this.Manager.Comment("reaching state \'S32\'");
                goto label0;
            }
            this.Manager.CheckObservationTimeout(false, new ExpectedEvent(TestScenarioExtendedControlsSearch1Win2K8R2.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioExtendedControlsSearch1Win2K8R2S0SearchOpResponseChecker)), new ExpectedEvent(TestScenarioExtendedControlsSearch1Win2K8R2.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioExtendedControlsSearch1Win2K8R2S0SearchOpResponseChecker1)));
        label0:
            ;
        }

        private void TestScenarioExtendedControlsSearch1Win2K8R2S0SearchOpResponseChecker(SearchResp response)
        {
            this.Manager.Comment("checking step \'event SearchOpResponse(retreivalUnsuccessful)\'");
            TestManagerHelpers.AssertAreEqual<SearchResp>(this.Manager, ((SearchResp)(1)), response, "response of SearchOpResponse, state S26");
        }

        private void TestScenarioExtendedControlsSearch1Win2K8R2S0SearchOpResponseChecker1(SearchResp response)
        {
            this.Manager.Comment("checking step \'event SearchOpResponse(retrievalSuccessful)\'");
            TestManagerHelpers.AssertAreEqual<SearchResp>(this.Manager, ((SearchResp)(0)), response, "response of SearchOpResponse, state S26");
        }
        #endregion

        #region Test Starting in S10
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioExtendedControlsSearch1Win2K8R2S10()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch1Win2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Users,DC=adts88\",\"(objectClass=user)\",Subtre" +
                    "e,[\"sAMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaU" +
                    "sed\",\"msDS-QuotaEffective\",\"member;range=1-0\",\"controlValue\"],NotSpecifiedContro" +
                    "l,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Users,DC=adts88", "(objectClass=user)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
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
                                                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                            "Head",
                                                                                                                            "Tail"}, new object[] {
                                                                                                                            "controlValue",
                                                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})})}), ExtendedControl.NotSpecifiedControl, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch1Win2K8R2S24();
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
        public void LDAP_TestScenarioExtendedControlsSearch1Win2K8R2S2()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch1Win2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Users,DC=adts88\",\"(objectClass=user)\",Subtre" +
                    "e,[\"sAMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaU" +
                    "sed\",\"msDS-QuotaEffective\",\"member;range=1-0\",\"controlValue\"],LDAP_CONTROL_VLVRE" +
                    "QUEST,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Users,DC=adts88", "(objectClass=user)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
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
                                                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                            "Head",
                                                                                                                            "Tail"}, new object[] {
                                                                                                                            "controlValue",
                                                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})})}), ExtendedControl.LDAP_CONTROL_VLVREQUEST, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch1Win2K8R2S24();
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
        public void LDAP_TestScenarioExtendedControlsSearch1Win2K8R2S4()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch1Win2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Users,DC=adts88\",\"(objectClass=user)\",Subtre" +
                    "e,[\"sAMAccountName\",\"cn\",\"ntSecurityDescriptor\",\"member;range=10-0\",\"msDS-QuotaU" +
                    "sed\",\"msDS-QuotaEffective\",\"member;range=1-0\",\"controlValue\"],LDAP_SERVER_SORT_O" +
                    "ID,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Users,DC=adts88", "(objectClass=user)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
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
                                                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                            "Head",
                                                                                                                            "Tail"}, new object[] {
                                                                                                                            "controlValue",
                                                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})})}), ExtendedControl.LDAP_SERVER_SORT_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch1Win2K8R2S24();
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
        public void LDAP_TestScenarioExtendedControlsSearch1Win2K8R2S6()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch1Win2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment(@"executing step 'call SearchOpReq(""CN=Users,DC=adts88"",""(objectClass=user)"",Subtree,[""sAMAccountName"",""cn"",""ntSecurityDescriptor"",""member;range=10-0"",""msDS-QuotaUsed"",""msDS-QuotaEffective"",""member;range=1-0"",""controlValue""],LDAP_SERVER_GET_STATS_OID,AD_DS)'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Users,DC=adts88", "(objectClass=user)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
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
                                                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                            "Head",
                                                                                                                            "Tail"}, new object[] {
                                                                                                                            "controlValue",
                                                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})})}), ExtendedControl.LDAP_SERVER_GET_STATS_OID, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch1Win2K8R2S24();
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
        public void LDAP_TestScenarioExtendedControlsSearch1Win2K8R2S8()
        {
            this.Manager.BeginTest("TestScenarioExtendedControlsSearch1Win2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment(@"executing step 'call SearchOpReq(""CN=Users,DC=adts88"",""(objectClass=user)"",Subtree,[""sAMAccountName"",""cn"",""ntSecurityDescriptor"",""member;range=10-0"",""msDS-QuotaUsed"",""msDS-QuotaEffective"",""member;range=1-0"",""controlValue""],LDAP_PAGED_RESULT_OID_STRING,AD_DS)'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Users,DC=adts88", "(objectClass=user)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
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
                                                                                                                this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                                                                                                            "Head",
                                                                                                                            "Tail"}, new object[] {
                                                                                                                            "controlValue",
                                                                                                                            ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})})})})})})})})}), ExtendedControl.LDAP_PAGED_RESULT_OID_STRING, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioExtendedControlsSearch1Win2K8R2S24();
            this.Manager.EndTest();
        }
        #endregion
    }
}
