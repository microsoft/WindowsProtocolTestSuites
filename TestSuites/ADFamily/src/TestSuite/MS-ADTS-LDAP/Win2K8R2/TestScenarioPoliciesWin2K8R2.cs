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
    public partial class TestScenarioPoliciesWin2K8R2 : PtfTestClassBase
    {

        public TestScenarioPoliciesWin2K8R2()
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
        [TestCategory("LDS")]
        public void LDAP_TestScenarioPoliciesWin2K8R2S0()
        {
            this.Manager.BeginTest("TestScenarioPoliciesWin2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Default Query Policy,CN=Query-Policies,CN=Di" +
                    "rectory Service,CN=Windows NT,CN=Services,CN=Configuration,DC=adts88\",\"(objectClass=queryPolicy)\",Subtree,[\"lDAPAdminLimits\"],NoExtendedControl,AD_LDS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Default Query Policy,CN=Query-Policies,CN=Directory Service,CN=Windows NT,CN=S" +
                    "ervices,CN=Configuration,DC=adts88", "(objectClass=queryPolicy)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "lDAPAdminLimits",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(1)));
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioPoliciesWin2K8R2S8();
            this.Manager.EndTest();
        }

        private void TestScenarioPoliciesWin2K8R2S8()
        {
            this.Manager.Comment("reaching state \'S8\'");
            int temp0 = this.Manager.ExpectEvent(this.QuiescenceTimeout, true, new ExpectedEvent(TestScenarioPoliciesWin2K8R2.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioPoliciesWin2K8R2S0SearchOpResponseChecker)), new ExpectedEvent(TestScenarioPoliciesWin2K8R2.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioPoliciesWin2K8R2S0SearchOpResponseChecker1)));
            if ((temp0 == 0))
            {
                this.Manager.Comment("reaching state \'S10\'");
                this.Manager.Comment("executing step \'call UnBind()\'");
                this.IAD_LDAPModelAdapterInstance.UnBind();
                this.Manager.Comment("reaching state \'S12\'");
                this.Manager.Comment("checking step \'return UnBind\'");
                this.Manager.Comment("reaching state \'S14\'");
                goto label0;
            }
            if ((temp0 == 1))
            {
                this.Manager.Comment("reaching state \'S9\'");
                this.Manager.Comment("executing step \'call UnBind()\'");
                this.IAD_LDAPModelAdapterInstance.UnBind();
                this.Manager.Comment("reaching state \'S11\'");
                this.Manager.Comment("checking step \'return UnBind\'");
                this.Manager.Comment("reaching state \'S13\'");
                goto label0;
            }
            this.Manager.CheckObservationTimeout(false, new ExpectedEvent(TestScenarioPoliciesWin2K8R2.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioPoliciesWin2K8R2S0SearchOpResponseChecker)), new ExpectedEvent(TestScenarioPoliciesWin2K8R2.SearchOpResponseInfo, null, new SearchOpResponseDelegate1(this.TestScenarioPoliciesWin2K8R2S0SearchOpResponseChecker1)));
        label0:
            ;
        }

        private void TestScenarioPoliciesWin2K8R2S0SearchOpResponseChecker(SearchResp response)
        {
            this.Manager.Comment("checking step \'event SearchOpResponse(retreivalUnsuccessful)\'");
            TestManagerHelpers.AssertAreEqual<SearchResp>(this.Manager, ((SearchResp)(1)), response, "response of SearchOpResponse, state S8");
        }

        private void TestScenarioPoliciesWin2K8R2S0SearchOpResponseChecker1(SearchResp response)
        {
            this.Manager.Comment("checking step \'event SearchOpResponse(retrievalSuccessful)\'");
            TestManagerHelpers.AssertAreEqual<SearchResp>(this.Manager, ((SearchResp)(0)), response, "response of SearchOpResponse, state S8");
        }
        #endregion

        #region Test Starting in S2
        [TestMethodAttribute()]
        [TestCategory("MS-ADTS-LDAP")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("Main")]
        public void LDAP_TestScenarioPoliciesWin2K8R2S2()
        {
            this.Manager.BeginTest("TestScenarioPoliciesWin2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call Initialize()\'");
            this.IAD_LDAPModelAdapterInstance.Initialize();
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return Initialize\'");
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("executing step \'call SearchOpReq(\"CN=Default Query Policy,CN=Query-Policies,CN=Di" +
                    "rectory Service,CN=Windows NT,CN=Services,CN=Configuration,DC=adts88\",\"(objectClass=queryPolicy)\",Subtree,[\"lDAPAdminLimits\"],NoExtendedControl,AD_DS)\'");
            this.IAD_LDAPModelAdapterInstance.SearchOpReq("CN=Default Query Policy,CN=Query-Policies,CN=Directory Service,CN=Windows NT,CN=S" +
                    "ervices,CN=Configuration,DC=adts88", "(objectClass=queryPolicy)", SearchScope.Subtree, this.Make<Microsoft.Modeling.Sequence<string>>(new string[] {
                            "Rep"}, new object[] {
                            this.Make<Microsoft.Xrt.Runtime.RuntimeList<string>>(new string[] {
                                        "Head",
                                        "Tail"}, new object[] {
                                        "lDAPAdminLimits",
                                        ((Microsoft.Xrt.Runtime.RuntimeList<string>)(null))})}), null, ((ADImplementations)(0)));
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return SearchOpReq\'");
            TestScenarioPoliciesWin2K8R2S8();
            this.Manager.EndTest();
        }
        #endregion
    }
}
