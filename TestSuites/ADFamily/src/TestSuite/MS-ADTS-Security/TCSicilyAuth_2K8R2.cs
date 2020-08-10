// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Microsoft.SpecExplorer.Runtime.Testing;
    using Microsoft.Protocols.TestTools;
    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [System.CodeDom.Compiler.GeneratedCodeAttribute("Spec Explorer", "3.5.3146.0")]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class TCSicilyAuth_2K8R2 : PtfTestClassBase
    {

        public TCSicilyAuth_2K8R2()
        {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "100000");
        }

        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security.IMS_ADTS_AuthenticationAuth IMS_ADTS_AuthenticationAuthInstance;
        #endregion

        #region Class Initialization and Cleanup
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void ClassInitialize(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext context)
        {
            PtfTestClassBase.Initialize(context);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void ClassCleanup()
        {
            PtfTestClassBase.Cleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            this.InitializeTestManager();
            this.IMS_ADTS_AuthenticationAuthInstance = ((Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security.IMS_ADTS_AuthenticationAuth)(this.Manager.GetAdapter(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security.IMS_ADTS_AuthenticationAuth))));
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion

        #region Test Starting in S0
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S0()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S176\'");
            ProtocolMessageStructures.errorstatus temp0;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,invalidPassword,LDAP_SSL_PORT,Tru" +
                    "e)\'");
            temp0 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S264\'");
            this.Manager.Comment("checking step \'return SicilyBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp0, "return of SicilyBind, state S264");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }

        private void TCSicilyAuth_2K8R2S352()
        {
            this.Manager.Comment("reaching state \'S352\'");
        }
        #endregion

        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S10()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S181\'");
            ProtocolMessageStructures.errorstatus temp1;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp1 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S269\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp1, "return of SicilyBind, state S269");
            this.Manager.Comment("reaching state \'S357\'");
            ProtocolMessageStructures.errorstatus temp2;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPLVECTOR,NotSet,msDS_NCReplCurs" +
                    "ors,False)\'");
            temp2 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPLVECTOR, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_NCReplCursors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R77");
            this.Manager.Checkpoint("MS-ADTS-Security_R79");
            this.Manager.Checkpoint("MS-ADTS-Security_R81");
            this.Manager.Comment("reaching state \'S394\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp2, "return of AuthorizationCheck, state S394");
            TCSicilyAuth_2K8R2S426();
            this.Manager.EndTest();
        }

        private void TCSicilyAuth_2K8R2S426()
        {
            this.Manager.Comment("reaching state \'S426\'");
        }
        #endregion

        #region Test Starting in S100
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S100()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S100");
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S226\'");
            ProtocolMessageStructures.errorstatus temp3;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,validPassword,LDAP_SSL_GC_PORT,Tr" +
                    "ue)\'");
            temp3 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S314\'");
            this.Manager.Comment("checking step \'return SicilyBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp3, "return of SicilyBind, state S314");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }

        private void TCSicilyAuth_2K8R2S377()
        {
            this.Manager.Comment("reaching state \'S377\'");
        }
        #endregion

        #region Test Starting in S102
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSicilyAuth_2K8R2S102()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S102");
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S227\'");
            ProtocolMessageStructures.errorstatus temp4;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,validPassword,LDAP_SSL_PORT,True)" +
                    "\'");
            temp4 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S315\'");
            this.Manager.Comment("checking step \'return SicilyBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp4, "return of SicilyBind, state S315");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S104
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S104()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S104");
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S228\'");
            ProtocolMessageStructures.errorstatus temp5;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_SSL_PORT,True)\'");
            temp5 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S316\'");
            this.Manager.Comment("checking step \'return SicilyBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp5, "return of SicilyBind, state S316");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S106
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S106()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S106");
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S229\'");
            ProtocolMessageStructures.errorstatus temp6;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_SSL_GC_PORT,True" +
                    ")\'");
            temp6 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S317\'");
            this.Manager.Comment("checking step \'return SicilyBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp6, "return of SicilyBind, state S317");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S108
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S108()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S108");
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S230\'");
            ProtocolMessageStructures.errorstatus temp7;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,invalidPassword,LDAP_SSL_PORT,True)" +
                    "\'");
            temp7 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S318\'");
            this.Manager.Comment("checking step \'return SicilyBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp7, "return of SicilyBind, state S318");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S110
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S110()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S110");
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S231\'");
            ProtocolMessageStructures.errorstatus temp8;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp8 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S319\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp8, "return of SicilyBind, state S319");
            this.Manager.Comment("reaching state \'S383\'");
            ProtocolMessageStructures.errorstatus temp9;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_NCReplInboundNeighbors" +
                    ",False)\'");
            temp9 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_NCReplInboundNeighbors, false);
            this.Manager.Comment("reaching state \'S419\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp9, "return of AuthorizationCheck, state S419");
            TCSicilyAuth_2K8R2S427();
            this.Manager.EndTest();
        }

        private void TCSicilyAuth_2K8R2S427()
        {
            this.Manager.Comment("reaching state \'S427\'");
        }
        #endregion

        #region Test Starting in S112
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S112()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S112");
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S232\'");
            ProtocolMessageStructures.errorstatus temp10;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,invalidPassword,LDAP_PORT,True)\'");
            temp10 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S320\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp10, "return of SicilyBind, state S320");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S114
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S114()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S114");
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S233\'");
            ProtocolMessageStructures.errorstatus temp11;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_PORT,False)\'");
            temp11 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S321\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp11, "return of SicilyBind, state S321");
            this.Manager.Comment("reaching state \'S384\'");
            ProtocolMessageStructures.errorstatus temp12;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPSTO,NotSet,msDS_NCReplOutbound" +
                    "Neighbors,False)\'");
            temp12 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPSTO, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_NCReplOutboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R72");
            this.Manager.Checkpoint("MS-ADTS-Security_R73");
            this.Manager.Checkpoint("MS-ADTS-Security_R74");
            this.Manager.Comment("reaching state \'S420\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp12, "return of AuthorizationCheck, state S420");
            TCSicilyAuth_2K8R2S428();
            this.Manager.EndTest();
        }

        private void TCSicilyAuth_2K8R2S428()
        {
            this.Manager.Comment("reaching state \'S428\'");
        }
        #endregion

        #region Test Starting in S116
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S116()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S116");
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S234\'");
            ProtocolMessageStructures.errorstatus temp13;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp13 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S322\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp13, "return of SicilyBind, state S322");
            this.Manager.Comment("reaching state \'S385\'");
            ProtocolMessageStructures.errorstatus temp14;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPSFORM,NotSet,msDS_NCReplInboun" +
                    "dNeighbors,False)\'");
            temp14 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPSFORM, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_NCReplInboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R67");
            this.Manager.Checkpoint("MS-ADTS-Security_R68");
            this.Manager.Checkpoint("MS-ADTS-Security_R69");
            this.Manager.Comment("reaching state \'S421\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp14, "return of AuthorizationCheck, state S421");
            TCSicilyAuth_2K8R2S428();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S118
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S118()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S118");
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S235\'");
            ProtocolMessageStructures.errorstatus temp15;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp15 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S323\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp15, "return of SicilyBind, state S323");
            this.Manager.Comment("reaching state \'S386\'");
            ProtocolMessageStructures.errorstatus temp16;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,Repl_Man_Topo_REPLVECTOR,msDS_NCRe" +
                    "plCursors,False)\'");
            temp16 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.Repl_Man_Topo_REPLVECTOR, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplCursors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R77");
            this.Manager.Checkpoint("MS-ADTS-Security_R79");
            this.Manager.Checkpoint("MS-ADTS-Security_R81");
            this.Manager.Comment("reaching state \'S422\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp16, "return of AuthorizationCheck, state S422");
            TCSicilyAuth_2K8R2S428();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S12()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S182\'");
            ProtocolMessageStructures.errorstatus temp17;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp17 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S270\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp17, "return of SicilyBind, state S270");
            this.Manager.Comment("reaching state \'S358\'");
            ProtocolMessageStructures.errorstatus temp18;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPLVECTOR,Repl_Mon_Topo_REPLVECT" +
                    "OR,msDS_NCReplCursors,False)\'");
            temp18 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPLVECTOR, ProtocolMessageStructures.ControlAccessRights.Repl_Mon_Topo_REPLVECTOR, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplCursors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R77");
            this.Manager.Checkpoint("MS-ADTS-Security_R79");
            this.Manager.Checkpoint("MS-ADTS-Security_R81");
            this.Manager.Comment("reaching state \'S395\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp18, "return of AuthorizationCheck, state S395");
            TCSicilyAuth_2K8R2S426();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S120
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S120()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S120");
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S236\'");
            ProtocolMessageStructures.errorstatus temp19;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_SSL_PORT,False)\'" +
                    "");
            temp19 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S324\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp19, "return of SicilyBind, state S324");
            this.Manager.Comment("reaching state \'S387\'");
            ProtocolMessageStructures.errorstatus temp20;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPLVECTOR,NotSet,msDS_NCReplCurs" +
                    "ors,False)\'");
            temp20 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPLVECTOR, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_NCReplCursors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R77");
            this.Manager.Checkpoint("MS-ADTS-Security_R79");
            this.Manager.Checkpoint("MS-ADTS-Security_R81");
            this.Manager.Comment("reaching state \'S423\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp20, "return of AuthorizationCheck, state S423");
            TCSicilyAuth_2K8R2S428();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S122
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S122()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S122");
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S237\'");
            ProtocolMessageStructures.errorstatus temp21;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp21 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S325\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp21, "return of SicilyBind, state S325");
            this.Manager.Comment("reaching state \'S388\'");
            ProtocolMessageStructures.errorstatus temp22;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPLVECTOR,Repl_Mon_Topo_REPLVECT" +
                    "OR,msDS_NCReplCursors,False)\'");
            temp22 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPLVECTOR, ProtocolMessageStructures.ControlAccessRights.Repl_Mon_Topo_REPLVECTOR, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplCursors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R77");
            this.Manager.Checkpoint("MS-ADTS-Security_R79");
            this.Manager.Checkpoint("MS-ADTS-Security_R81");
            this.Manager.Comment("reaching state \'S424\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp22, "return of AuthorizationCheck, state S424");
            TCSicilyAuth_2K8R2S428();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S124
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S124()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S124");
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S238\'");
            ProtocolMessageStructures.errorstatus temp23;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,validPassword,LDAP_PORT,True)\'");
            temp23 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S326\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp23, "return of SicilyBind, state S326");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S126
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S126()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S126");
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S239\'");
            ProtocolMessageStructures.errorstatus temp24;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,invalidPassword,LDAP_SSL_GC_PORT," +
                    "False)\'");
            temp24 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S327\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp24, "return of SicilyBind, state S327");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S128
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S128()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S128");
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S240\'");
            ProtocolMessageStructures.errorstatus temp25;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,invalidPassword,LDAP_SSL_GC_PORT,Fa" +
                    "lse)\'");
            temp25 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S328\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp25, "return of SicilyBind, state S328");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S130
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S130()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S130");
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S241\'");
            ProtocolMessageStructures.errorstatus temp26;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,invalidPassword,LDAP_PORT,False)\'");
            temp26 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S329\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp26, "return of SicilyBind, state S329");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S132
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S132()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S132");
            this.Manager.Comment("reaching state \'S132\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S133\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S242\'");
            ProtocolMessageStructures.errorstatus temp27;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,validPassword,LDAP_PORT,False)\'");
            temp27 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S330\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp27, "return of SicilyBind, state S330");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S134
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S134()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S134");
            this.Manager.Comment("reaching state \'S134\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S135\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S243\'");
            ProtocolMessageStructures.errorstatus temp28;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,validPassword,LDAP_SSL_GC_PORT,Fa" +
                    "lse)\'");
            temp28 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S331\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp28, "return of SicilyBind, state S331");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S136
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S136()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S136");
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S137\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S244\'");
            ProtocolMessageStructures.errorstatus temp29;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp29 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S332\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp29, "return of SicilyBind, state S332");
            this.Manager.Comment("reaching state \'S389\'");
            ProtocolMessageStructures.errorstatus temp30;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_NCReplInboundNeighbors" +
                    ",False)\'");
            temp30 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_NCReplInboundNeighbors, false);
            this.Manager.Comment("reaching state \'S425\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp30, "return of AuthorizationCheck, state S425");
            TCSicilyAuth_2K8R2S429();
            this.Manager.EndTest();
        }

        private void TCSicilyAuth_2K8R2S429()
        {
            this.Manager.Comment("reaching state \'S429\'");
        }
        #endregion

        #region Test Starting in S138
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S138()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S138");
            this.Manager.Comment("reaching state \'S138\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S139\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S245\'");
            ProtocolMessageStructures.errorstatus temp31;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,invalidPassword,LDAP_PORT,False)\'" +
                    "");
            temp31 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S333\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp31, "return of SicilyBind, state S333");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S14()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S183\'");
            ProtocolMessageStructures.errorstatus temp32;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp32 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S271\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp32, "return of SicilyBind, state S271");
            this.Manager.Comment("reaching state \'S359\'");
            ProtocolMessageStructures.errorstatus temp33;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPLVECTOR,Repl_Man_Topo_REPLVECT" +
                    "OR,msDS_NCReplCursors,False)\'");
            temp33 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPLVECTOR, ProtocolMessageStructures.ControlAccessRights.Repl_Man_Topo_REPLVECTOR, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplCursors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R77");
            this.Manager.Checkpoint("MS-ADTS-Security_R79");
            this.Manager.Checkpoint("MS-ADTS-Security_R81");
            this.Manager.Comment("reaching state \'S396\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp33, "return of AuthorizationCheck, state S396");
            TCSicilyAuth_2K8R2S426();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S140
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S140()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S140");
            this.Manager.Comment("reaching state \'S140\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S141\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S246\'");
            ProtocolMessageStructures.errorstatus temp34;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,invalidPassword,LDAP_SSL_PORT,Fal" +
                    "se)\'");
            temp34 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S334\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp34, "return of SicilyBind, state S334");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S142
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S142()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S142");
            this.Manager.Comment("reaching state \'S142\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S143\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S247\'");
            ProtocolMessageStructures.errorstatus temp35;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,invalidPassword,LDAP_GC_PORT,Fals" +
                    "e)\'");
            temp35 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S335\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp35, "return of SicilyBind, state S335");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S144
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S144()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S144");
            this.Manager.Comment("reaching state \'S144\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S145\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S248\'");
            ProtocolMessageStructures.errorstatus temp36;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,validPassword,LDAP_SSL_PORT,False" +
                    ")\'");
            temp36 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S336\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp36, "return of SicilyBind, state S336");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S146
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S146()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S146");
            this.Manager.Comment("reaching state \'S146\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S147\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S249\'");
            ProtocolMessageStructures.errorstatus temp37;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,validPassword,LDAP_GC_PORT,False)" +
                    "\'");
            temp37 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S337\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp37, "return of SicilyBind, state S337");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S148
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S148()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S148");
            this.Manager.Comment("reaching state \'S148\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S149\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S250\'");
            ProtocolMessageStructures.errorstatus temp38;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,invalidPassword,LDAP_GC_PORT,True" +
                    ")\'");
            temp38 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S338\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp38, "return of SicilyBind, state S338");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S150
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S150()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S150");
            this.Manager.Comment("reaching state \'S150\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S151\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S251\'");
            ProtocolMessageStructures.errorstatus temp39;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,validPassword,LDAP_GC_PORT,True)\'" +
                    "");
            temp39 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S339\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp39, "return of SicilyBind, state S339");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S152
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S152()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S152");
            this.Manager.Comment("reaching state \'S152\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S153\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S252\'");
            ProtocolMessageStructures.errorstatus temp40;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,invalidPassword,LDAP_GC_PORT,True)\'" +
                    "");
            temp40 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S340\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp40, "return of SicilyBind, state S340");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S154
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S154()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S154");
            this.Manager.Comment("reaching state \'S154\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S155\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S253\'");
            ProtocolMessageStructures.errorstatus temp41;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,invalidPassword,LDAP_PORT,True)\'");
            temp41 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S341\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp41, "return of SicilyBind, state S341");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S156
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S156()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S156");
            this.Manager.Comment("reaching state \'S156\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S157\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S254\'");
            ProtocolMessageStructures.errorstatus temp42;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,invalidPassword,LDAP_SSL_PORT,False" +
                    ")\'");
            temp42 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S342\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp42, "return of SicilyBind, state S342");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S158
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S158()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S158");
            this.Manager.Comment("reaching state \'S158\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S159\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S255\'");
            ProtocolMessageStructures.errorstatus temp43;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,invalidPassword,LDAP_GC_PORT,False)" +
                    "\'");
            temp43 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S343\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp43, "return of SicilyBind, state S343");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S16()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S184\'");
            ProtocolMessageStructures.errorstatus temp44;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp44 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S272\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp44, "return of SicilyBind, state S272");
            this.Manager.Comment("reaching state \'S360\'");
            ProtocolMessageStructures.errorstatus temp45;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,Repl_Man_Topo_REPSTO,msDS_NCReplOu" +
                    "tboundNeighbors,False)\'");
            temp45 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.Repl_Man_Topo_REPSTO, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplOutboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R72");
            this.Manager.Checkpoint("MS-ADTS-Security_R73");
            this.Manager.Checkpoint("MS-ADTS-Security_R74");
            this.Manager.Comment("reaching state \'S397\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp45, "return of AuthorizationCheck, state S397");
            TCSicilyAuth_2K8R2S426();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S160
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S160()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S160");
            this.Manager.Comment("reaching state \'S160\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S161\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S256\'");
            ProtocolMessageStructures.errorstatus temp46;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,invalidPassword,LDAP_SSL_GC_PORT," +
                    "True)\'");
            temp46 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S344\'");
            this.Manager.Comment("checking step \'return SicilyBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp46, "return of SicilyBind, state S344");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S162
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S162()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S162");
            this.Manager.Comment("reaching state \'S162\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S163\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S257\'");
            ProtocolMessageStructures.errorstatus temp47;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,invalidPassword,LDAP_SSL_GC_PORT,Tr" +
                    "ue)\'");
            temp47 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S345\'");
            this.Manager.Comment("checking step \'return SicilyBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp47, "return of SicilyBind, state S345");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S164
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S164()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S164");
            this.Manager.Comment("reaching state \'S164\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S165\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S258\'");
            ProtocolMessageStructures.errorstatus temp48;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,validPassword,LDAP_SSL_GC_PORT,Tr" +
                    "ue)\'");
            temp48 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S346\'");
            this.Manager.Comment("checking step \'return SicilyBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp48, "return of SicilyBind, state S346");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S166
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S166()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S166");
            this.Manager.Comment("reaching state \'S166\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S167\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S259\'");
            ProtocolMessageStructures.errorstatus temp49;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,validPassword,LDAP_SSL_PORT,True)" +
                    "\'");
            temp49 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S347\'");
            this.Manager.Comment("checking step \'return SicilyBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp49, "return of SicilyBind, state S347");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S168
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S168()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S168");
            this.Manager.Comment("reaching state \'S168\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S169\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S260\'");
            ProtocolMessageStructures.errorstatus temp50;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_SSL_PORT,True)\'");
            temp50 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S348\'");
            this.Manager.Comment("checking step \'return SicilyBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp50, "return of SicilyBind, state S348");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S170
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S170()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S170");
            this.Manager.Comment("reaching state \'S170\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S171\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S261\'");
            ProtocolMessageStructures.errorstatus temp51;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_SSL_GC_PORT,True" +
                    ")\'");
            temp51 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S349\'");
            this.Manager.Comment("checking step \'return SicilyBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp51, "return of SicilyBind, state S349");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S172
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S172()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S172");
            this.Manager.Comment("reaching state \'S172\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S173\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S262\'");
            ProtocolMessageStructures.errorstatus temp52;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,invalidPassword,LDAP_SSL_PORT,True)" +
                    "\'");
            temp52 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S350\'");
            this.Manager.Comment("checking step \'return SicilyBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp52, "return of SicilyBind, state S350");
            TCSicilyAuth_2K8R2S352();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S174
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S174()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S174");
            this.Manager.Comment("reaching state \'S174\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S175\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S263\'");
            ProtocolMessageStructures.errorstatus temp53;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,invalidPassword,LDAP_SSL_PORT,Tru" +
                    "e)\'");
            temp53 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S351\'");
            this.Manager.Comment("checking step \'return SicilyBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp53, "return of SicilyBind, state S351");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S18()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S185\'");
            ProtocolMessageStructures.errorstatus temp54;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp54 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S273\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp54, "return of SicilyBind, state S273");
            this.Manager.Comment("reaching state \'S361\'");
            ProtocolMessageStructures.errorstatus temp55;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPSFORM,Repl_Mon_Topo_REPSFROM,m" +
                    "sDS_NCReplInboundNeighbors,False)\'");
            temp55 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPSFORM, ProtocolMessageStructures.ControlAccessRights.Repl_Mon_Topo_REPSFROM, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplInboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R67");
            this.Manager.Checkpoint("MS-ADTS-Security_R68");
            this.Manager.Checkpoint("MS-ADTS-Security_R69");
            this.Manager.Comment("reaching state \'S398\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp55, "return of AuthorizationCheck, state S398");
            TCSicilyAuth_2K8R2S426();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S2()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S177\'");
            ProtocolMessageStructures.errorstatus temp56;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp56 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S265\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp56, "return of SicilyBind, state S265");
            this.Manager.Comment("reaching state \'S353\'");
            ProtocolMessageStructures.errorstatus temp57;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,Repl_Mon_Topo_REPLVECTOR,msDS_NCRe" +
                    "plCursors,False)\'");
            temp57 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.Repl_Mon_Topo_REPLVECTOR, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplCursors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R77");
            this.Manager.Checkpoint("MS-ADTS-Security_R79");
            this.Manager.Checkpoint("MS-ADTS-Security_R81");
            this.Manager.Comment("reaching state \'S390\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp57, "return of AuthorizationCheck, state S390");
            TCSicilyAuth_2K8R2S426();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S20()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S186\'");
            ProtocolMessageStructures.errorstatus temp58;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp58 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S274\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp58, "return of SicilyBind, state S274");
            this.Manager.Comment("reaching state \'S362\'");
            ProtocolMessageStructures.errorstatus temp59;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPSFORM,Repl_Man_Topo_REPSFROM,m" +
                    "sDS_NCReplInboundNeighbors,False)\'");
            temp59 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPSFORM, ProtocolMessageStructures.ControlAccessRights.Repl_Man_Topo_REPSFROM, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplInboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R67");
            this.Manager.Checkpoint("MS-ADTS-Security_R68");
            this.Manager.Checkpoint("MS-ADTS-Security_R69");
            this.Manager.Comment("reaching state \'S399\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp59, "return of AuthorizationCheck, state S399");
            TCSicilyAuth_2K8R2S426();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S22()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S187\'");
            ProtocolMessageStructures.errorstatus temp60;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp60 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S275\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp60, "return of SicilyBind, state S275");
            this.Manager.Comment("reaching state \'S363\'");
            ProtocolMessageStructures.errorstatus temp61;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_NCReplCursors,False)\'");
            temp61 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_NCReplCursors, false);
            this.Manager.Comment("reaching state \'S400\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp61, "return of AuthorizationCheck, state S400");
            TCSicilyAuth_2K8R2S427();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S24()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S188\'");
            ProtocolMessageStructures.errorstatus temp62;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp62 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S276\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp62, "return of SicilyBind, state S276");
            this.Manager.Comment("reaching state \'S364\'");
            ProtocolMessageStructures.errorstatus temp63;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_NCReplOutboundNeighbor" +
                    "s,False)\'");
            temp63 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_NCReplOutboundNeighbors, false);
            this.Manager.Comment("reaching state \'S401\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp63, "return of AuthorizationCheck, state S401");
            TCSicilyAuth_2K8R2S427();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S26()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S189\'");
            ProtocolMessageStructures.errorstatus temp64;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp64 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S277\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp64, "return of SicilyBind, state S277");
            this.Manager.Comment("reaching state \'S365\'");
            ProtocolMessageStructures.errorstatus temp65;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,Repl_Mon_Topo_REPLVECTOR,msDS_NCRe" +
                    "plCursors,False)\'");
            temp65 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.Repl_Mon_Topo_REPLVECTOR, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplCursors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R77");
            this.Manager.Checkpoint("MS-ADTS-Security_R79");
            this.Manager.Checkpoint("MS-ADTS-Security_R81");
            this.Manager.Comment("reaching state \'S402\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp65, "return of AuthorizationCheck, state S402");
            TCSicilyAuth_2K8R2S428();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S28()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S190\'");
            ProtocolMessageStructures.errorstatus temp66;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp66 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S278\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp66, "return of SicilyBind, state S278");
            this.Manager.Comment("reaching state \'S366\'");
            ProtocolMessageStructures.errorstatus temp67;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPLVECTOR,Repl_Man_Topo_REPLVECT" +
                    "OR,msDS_NCReplCursors,False)\'");
            temp67 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPLVECTOR, ProtocolMessageStructures.ControlAccessRights.Repl_Man_Topo_REPLVECTOR, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplCursors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R77");
            this.Manager.Checkpoint("MS-ADTS-Security_R79");
            this.Manager.Checkpoint("MS-ADTS-Security_R81");
            this.Manager.Comment("reaching state \'S403\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp67, "return of AuthorizationCheck, state S403");
            TCSicilyAuth_2K8R2S428();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S30()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S191\'");
            ProtocolMessageStructures.errorstatus temp68;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp68 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S279\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp68, "return of SicilyBind, state S279");
            this.Manager.Comment("reaching state \'S367\'");
            ProtocolMessageStructures.errorstatus temp69;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPSTO,Repl_Man_Topo_REPSTO,msDS_" +
                    "NCReplOutboundNeighbors,False)\'");
            temp69 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPSTO, ProtocolMessageStructures.ControlAccessRights.Repl_Man_Topo_REPSTO, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplOutboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R72");
            this.Manager.Checkpoint("MS-ADTS-Security_R73");
            this.Manager.Checkpoint("MS-ADTS-Security_R74");
            this.Manager.Comment("reaching state \'S404\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp69, "return of AuthorizationCheck, state S404");
            TCSicilyAuth_2K8R2S428();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S32()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S192\'");
            ProtocolMessageStructures.errorstatus temp70;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp70 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S280\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp70, "return of SicilyBind, state S280");
            this.Manager.Comment("reaching state \'S368\'");
            ProtocolMessageStructures.errorstatus temp71;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPSTO,Repl_Mon_Topo_REPSTO,msDS_" +
                    "NCReplOutboundNeighbors,False)\'");
            temp71 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPSTO, ProtocolMessageStructures.ControlAccessRights.Repl_Mon_Topo_REPSTO, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplOutboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R72");
            this.Manager.Checkpoint("MS-ADTS-Security_R73");
            this.Manager.Checkpoint("MS-ADTS-Security_R74");
            this.Manager.Comment("reaching state \'S405\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp71, "return of AuthorizationCheck, state S405");
            TCSicilyAuth_2K8R2S428();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S34()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S193\'");
            ProtocolMessageStructures.errorstatus temp72;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp72 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S281\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp72, "return of SicilyBind, state S281");
            this.Manager.Comment("reaching state \'S369\'");
            ProtocolMessageStructures.errorstatus temp73;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,Repl_Mon_Topo_REPSFROM,msDS_NCRepl" +
                    "InboundNeighbors,False)\'");
            temp73 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.Repl_Mon_Topo_REPSFROM, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplInboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R67");
            this.Manager.Checkpoint("MS-ADTS-Security_R68");
            this.Manager.Checkpoint("MS-ADTS-Security_R69");
            this.Manager.Comment("reaching state \'S406\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp73, "return of AuthorizationCheck, state S406");
            TCSicilyAuth_2K8R2S428();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S36()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S194\'");
            ProtocolMessageStructures.errorstatus temp74;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp74 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S282\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp74, "return of SicilyBind, state S282");
            this.Manager.Comment("reaching state \'S370\'");
            ProtocolMessageStructures.errorstatus temp75;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,Repl_Mon_Topo_REPSTO,msDS_NCReplOu" +
                    "tboundNeighbors,False)\'");
            temp75 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.Repl_Mon_Topo_REPSTO, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplOutboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R72");
            this.Manager.Checkpoint("MS-ADTS-Security_R73");
            this.Manager.Checkpoint("MS-ADTS-Security_R74");
            this.Manager.Comment("reaching state \'S407\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp75, "return of AuthorizationCheck, state S407");
            TCSicilyAuth_2K8R2S428();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S38()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S195\'");
            ProtocolMessageStructures.errorstatus temp76;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp76 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S283\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp76, "return of SicilyBind, state S283");
            this.Manager.Comment("reaching state \'S371\'");
            ProtocolMessageStructures.errorstatus temp77;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,Repl_Man_Topo_REPSFROM,msDS_NCRepl" +
                    "InboundNeighbors,False)\'");
            temp77 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.Repl_Man_Topo_REPSFROM, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplInboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R67");
            this.Manager.Checkpoint("MS-ADTS-Security_R68");
            this.Manager.Checkpoint("MS-ADTS-Security_R69");
            this.Manager.Comment("reaching state \'S408\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp77, "return of AuthorizationCheck, state S408");
            TCSicilyAuth_2K8R2S428();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S4()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S178\'");
            ProtocolMessageStructures.errorstatus temp78;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp78 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S266\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp78, "return of SicilyBind, state S266");
            this.Manager.Comment("reaching state \'S354\'");
            ProtocolMessageStructures.errorstatus temp79;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPSTO,NotSet,msDS_NCReplOutbound" +
                    "Neighbors,False)\'");
            temp79 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPSTO, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_NCReplOutboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R72");
            this.Manager.Checkpoint("MS-ADTS-Security_R73");
            this.Manager.Checkpoint("MS-ADTS-Security_R74");
            this.Manager.Comment("reaching state \'S391\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp79, "return of AuthorizationCheck, state S391");
            TCSicilyAuth_2K8R2S426();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S40
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S40()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S196\'");
            ProtocolMessageStructures.errorstatus temp80;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp80 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S284\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp80, "return of SicilyBind, state S284");
            this.Manager.Comment("reaching state \'S372\'");
            ProtocolMessageStructures.errorstatus temp81;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,Repl_Man_Topo_REPSTO,msDS_NCReplOu" +
                    "tboundNeighbors,False)\'");
            temp81 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.Repl_Man_Topo_REPSTO, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplOutboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R72");
            this.Manager.Checkpoint("MS-ADTS-Security_R73");
            this.Manager.Checkpoint("MS-ADTS-Security_R74");
            this.Manager.Comment("reaching state \'S409\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp81, "return of AuthorizationCheck, state S409");
            TCSicilyAuth_2K8R2S428();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S42
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S42()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S197\'");
            ProtocolMessageStructures.errorstatus temp82;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp82 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S285\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp82, "return of SicilyBind, state S285");
            this.Manager.Comment("reaching state \'S373\'");
            ProtocolMessageStructures.errorstatus temp83;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPSFORM,Repl_Mon_Topo_REPSFROM,m" +
                    "sDS_NCReplInboundNeighbors,False)\'");
            temp83 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPSFORM, ProtocolMessageStructures.ControlAccessRights.Repl_Mon_Topo_REPSFROM, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplInboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R67");
            this.Manager.Checkpoint("MS-ADTS-Security_R68");
            this.Manager.Checkpoint("MS-ADTS-Security_R69");
            this.Manager.Comment("reaching state \'S410\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp83, "return of AuthorizationCheck, state S410");
            TCSicilyAuth_2K8R2S428();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S44
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S44()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S44");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S198\'");
            ProtocolMessageStructures.errorstatus temp84;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp84 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S286\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp84, "return of SicilyBind, state S286");
            this.Manager.Comment("reaching state \'S374\'");
            ProtocolMessageStructures.errorstatus temp85;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPSFORM,Repl_Man_Topo_REPSFROM,m" +
                    "sDS_NCReplInboundNeighbors,False)\'");
            temp85 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPSFORM, ProtocolMessageStructures.ControlAccessRights.Repl_Man_Topo_REPSFROM, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplInboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R67");
            this.Manager.Checkpoint("MS-ADTS-Security_R68");
            this.Manager.Checkpoint("MS-ADTS-Security_R69");
            this.Manager.Comment("reaching state \'S411\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp85, "return of AuthorizationCheck, state S411");
            TCSicilyAuth_2K8R2S428();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S46
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S46()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S46");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S199\'");
            ProtocolMessageStructures.errorstatus temp86;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp86 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S287\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp86, "return of SicilyBind, state S287");
            this.Manager.Comment("reaching state \'S375\'");
            ProtocolMessageStructures.errorstatus temp87;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_NCReplCursors,False)\'");
            temp87 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_NCReplCursors, false);
            this.Manager.Comment("reaching state \'S412\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp87, "return of AuthorizationCheck, state S412");
            TCSicilyAuth_2K8R2S429();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S48
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S48()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S48");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S200\'");
            ProtocolMessageStructures.errorstatus temp88;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp88 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S288\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp88, "return of SicilyBind, state S288");
            this.Manager.Comment("reaching state \'S376\'");
            ProtocolMessageStructures.errorstatus temp89;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_NCReplOutboundNeighbor" +
                    "s,False)\'");
            temp89 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_NCReplOutboundNeighbors, false);
            this.Manager.Comment("reaching state \'S413\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp89, "return of AuthorizationCheck, state S413");
            TCSicilyAuth_2K8R2S429();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S50
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S50()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S50");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S201\'");
            ProtocolMessageStructures.errorstatus temp90;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,invalidPassword,LDAP_PORT,True)\'");
            temp90 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S289\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp90, "return of SicilyBind, state S289");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S52
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S52()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S52");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S202\'");
            ProtocolMessageStructures.errorstatus temp91;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_PORT,False)\'");
            temp91 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S290\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp91, "return of SicilyBind, state S290");
            this.Manager.Comment("reaching state \'S378\'");
            ProtocolMessageStructures.errorstatus temp92;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPSTO,Repl_Man_Topo_REPSTO,msDS_" +
                    "NCReplOutboundNeighbors,False)\'");
            temp92 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPSTO, ProtocolMessageStructures.ControlAccessRights.Repl_Man_Topo_REPSTO, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplOutboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R72");
            this.Manager.Checkpoint("MS-ADTS-Security_R73");
            this.Manager.Checkpoint("MS-ADTS-Security_R74");
            this.Manager.Comment("reaching state \'S414\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp92, "return of AuthorizationCheck, state S414");
            TCSicilyAuth_2K8R2S426();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S54
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S54()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S54");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S203\'");
            ProtocolMessageStructures.errorstatus temp93;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp93 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S291\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp93, "return of SicilyBind, state S291");
            this.Manager.Comment("reaching state \'S379\'");
            ProtocolMessageStructures.errorstatus temp94;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPSTO,Repl_Mon_Topo_REPSTO,msDS_" +
                    "NCReplOutboundNeighbors,False)\'");
            temp94 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPSTO, ProtocolMessageStructures.ControlAccessRights.Repl_Mon_Topo_REPSTO, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplOutboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R72");
            this.Manager.Checkpoint("MS-ADTS-Security_R73");
            this.Manager.Checkpoint("MS-ADTS-Security_R74");
            this.Manager.Comment("reaching state \'S415\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp94, "return of AuthorizationCheck, state S415");
            TCSicilyAuth_2K8R2S426();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S56
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S56()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S56");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S204\'");
            ProtocolMessageStructures.errorstatus temp95;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp95 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S292\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp95, "return of SicilyBind, state S292");
            this.Manager.Comment("reaching state \'S380\'");
            ProtocolMessageStructures.errorstatus temp96;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,Repl_Mon_Topo_REPSFROM,msDS_NCRepl" +
                    "InboundNeighbors,False)\'");
            temp96 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.Repl_Mon_Topo_REPSFROM, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplInboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R67");
            this.Manager.Checkpoint("MS-ADTS-Security_R68");
            this.Manager.Checkpoint("MS-ADTS-Security_R69");
            this.Manager.Comment("reaching state \'S416\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp96, "return of AuthorizationCheck, state S416");
            TCSicilyAuth_2K8R2S426();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S58
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S58()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S58");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S205\'");
            ProtocolMessageStructures.errorstatus temp97;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_SSL_PORT,False)\'" +
                    "");
            temp97 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S293\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp97, "return of SicilyBind, state S293");
            this.Manager.Comment("reaching state \'S381\'");
            ProtocolMessageStructures.errorstatus temp98;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,Repl_Mon_Topo_REPSTO,msDS_NCReplOu" +
                    "tboundNeighbors,False)\'");
            temp98 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.Repl_Mon_Topo_REPSTO, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplOutboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R72");
            this.Manager.Checkpoint("MS-ADTS-Security_R73");
            this.Manager.Checkpoint("MS-ADTS-Security_R74");
            this.Manager.Comment("reaching state \'S417\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp98, "return of AuthorizationCheck, state S417");
            TCSicilyAuth_2K8R2S426();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S6()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S179\'");
            ProtocolMessageStructures.errorstatus temp99;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp99 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S267\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp99, "return of SicilyBind, state S267");
            this.Manager.Comment("reaching state \'S355\'");
            ProtocolMessageStructures.errorstatus temp100;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_REPSFORM,NotSet,msDS_NCReplInboun" +
                    "dNeighbors,False)\'");
            temp100 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_REPSFORM, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_NCReplInboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R67");
            this.Manager.Checkpoint("MS-ADTS-Security_R68");
            this.Manager.Checkpoint("MS-ADTS-Security_R69");
            this.Manager.Comment("reaching state \'S392\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp100, "return of AuthorizationCheck, state S392");
            TCSicilyAuth_2K8R2S426();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S60
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S60()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S60");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S206\'");
            ProtocolMessageStructures.errorstatus temp101;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp101 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S294\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp101, "return of SicilyBind, state S294");
            this.Manager.Comment("reaching state \'S382\'");
            ProtocolMessageStructures.errorstatus temp102;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,Repl_Man_Topo_REPSFROM,msDS_NCRepl" +
                    "InboundNeighbors,False)\'");
            temp102 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.Repl_Man_Topo_REPSFROM, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplInboundNeighbors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R67");
            this.Manager.Checkpoint("MS-ADTS-Security_R68");
            this.Manager.Checkpoint("MS-ADTS-Security_R69");
            this.Manager.Comment("reaching state \'S418\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp102, "return of AuthorizationCheck, state S418");
            TCSicilyAuth_2K8R2S426();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S62
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S62()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S62");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S207\'");
            ProtocolMessageStructures.errorstatus temp103;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,validPassword,LDAP_PORT,True)\'");
            temp103 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S295\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp103, "return of SicilyBind, state S295");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S64
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S64()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S64");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S208\'");
            ProtocolMessageStructures.errorstatus temp104;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,invalidPassword,LDAP_SSL_GC_PORT," +
                    "False)\'");
            temp104 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S296\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp104, "return of SicilyBind, state S296");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S66
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S66()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S66");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S209\'");
            ProtocolMessageStructures.errorstatus temp105;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,invalidPassword,LDAP_SSL_GC_PORT,Fa" +
                    "lse)\'");
            temp105 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S297\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp105, "return of SicilyBind, state S297");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S68
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S68()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S68");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S210\'");
            ProtocolMessageStructures.errorstatus temp106;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,invalidPassword,LDAP_PORT,False)\'");
            temp106 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S298\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp106, "return of SicilyBind, state S298");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S70
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S70()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S70");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S211\'");
            ProtocolMessageStructures.errorstatus temp107;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,validPassword,LDAP_PORT,False)\'");
            temp107 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S299\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp107, "return of SicilyBind, state S299");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S72
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S72()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S72");
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S212\'");
            ProtocolMessageStructures.errorstatus temp108;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,validPassword,LDAP_SSL_GC_PORT,Fa" +
                    "lse)\'");
            temp108 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S300\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp108, "return of SicilyBind, state S300");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S74
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S74()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S74");
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S213\'");
            ProtocolMessageStructures.errorstatus temp109;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,invalidPassword,LDAP_PORT,False)\'" +
                    "");
            temp109 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S301\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp109, "return of SicilyBind, state S301");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S76
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S76()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S76");
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S214\'");
            ProtocolMessageStructures.errorstatus temp110;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,invalidPassword,LDAP_SSL_PORT,Fal" +
                    "se)\'");
            temp110 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S302\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp110, "return of SicilyBind, state S302");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S78
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S78()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S78");
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S215\'");
            ProtocolMessageStructures.errorstatus temp111;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,invalidPassword,LDAP_GC_PORT,Fals" +
                    "e)\'");
            temp111 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S303\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp111, "return of SicilyBind, state S303");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S8()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S180\'");
            ProtocolMessageStructures.errorstatus temp112;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp112 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R193");
            this.Manager.Comment("reaching state \'S268\'");
            this.Manager.Comment("checking step \'return SicilyBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp112, "return of SicilyBind, state S268");
            this.Manager.Comment("reaching state \'S356\'");
            ProtocolMessageStructures.errorstatus temp113;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,Repl_Man_Topo_REPLVECTOR,msDS_NCRe" +
                    "plCursors,False)\'");
            temp113 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.Repl_Man_Topo_REPLVECTOR, ProtocolMessageStructures.AttribsToCheck.msDS_NCReplCursors, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R77");
            this.Manager.Checkpoint("MS-ADTS-Security_R79");
            this.Manager.Checkpoint("MS-ADTS-Security_R81");
            this.Manager.Comment("reaching state \'S393\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp113, "return of AuthorizationCheck, state S393");
            TCSicilyAuth_2K8R2S426();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S80
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S80()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S80");
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S216\'");
            ProtocolMessageStructures.errorstatus temp114;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,validPassword,LDAP_SSL_PORT,False" +
                    ")\'");
            temp114 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S304\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp114, "return of SicilyBind, state S304");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S82
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S82()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S82");
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S217\'");
            ProtocolMessageStructures.errorstatus temp115;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,validPassword,LDAP_GC_PORT,False)" +
                    "\'");
            temp115 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S305\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp115, "return of SicilyBind, state S305");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S84
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S84()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S84");
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S218\'");
            ProtocolMessageStructures.errorstatus temp116;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,invalidPassword,LDAP_GC_PORT,True" +
                    ")\'");
            temp116 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S306\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp116, "return of SicilyBind, state S306");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S86
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S86()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S86");
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S219\'");
            ProtocolMessageStructures.errorstatus temp117;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,validPassword,LDAP_GC_PORT,True)\'" +
                    "");
            temp117 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S307\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp117, "return of SicilyBind, state S307");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S88
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S88()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S88");
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S220\'");
            ProtocolMessageStructures.errorstatus temp118;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,invalidPassword,LDAP_GC_PORT,True)\'" +
                    "");
            temp118 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S308\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp118, "return of SicilyBind, state S308");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S90
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S90()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S90");
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S221\'");
            ProtocolMessageStructures.errorstatus temp119;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,invalidPassword,LDAP_PORT,True)\'");
            temp119 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S309\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp119, "return of SicilyBind, state S309");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S92
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S92()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S92");
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S222\'");
            ProtocolMessageStructures.errorstatus temp120;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,invalidPassword,LDAP_SSL_PORT,False" +
                    ")\'");
            temp120 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S310\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp120, "return of SicilyBind, state S310");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S94
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S94()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S94");
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S223\'");
            ProtocolMessageStructures.errorstatus temp121;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,invalidPassword,LDAP_GC_PORT,False)" +
                    "\'");
            temp121 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S311\'");
            this.Manager.Comment("checking step \'return SicilyBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp121, "return of SicilyBind, state S311");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S96
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S96()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S96");
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S224\'");
            ProtocolMessageStructures.errorstatus temp122;
            this.Manager.Comment("executing step \'call SicilyBind(invalidUserName,invalidPassword,LDAP_SSL_GC_PORT," +
                    "True)\'");
            temp122 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S312\'");
            this.Manager.Comment("checking step \'return SicilyBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp122, "return of SicilyBind, state S312");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S98
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        [DeploymentItem(@"C:\Program Files\Microsoft Message Analyzer\Microsoft.WindowsAzure.Storage.dll")]
        public void Security_TCSicilyAuth_2K8R2S98()
        {
            this.Manager.BeginTest("TCSicilyAuth_2K8R2S98");
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S225\'");
            ProtocolMessageStructures.errorstatus temp123;
            this.Manager.Comment("executing step \'call SicilyBind(validUserName,invalidPassword,LDAP_SSL_GC_PORT,Tr" +
                    "ue)\'");
            temp123 = this.IMS_ADTS_AuthenticationAuthInstance.SicilyBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S313\'");
            this.Manager.Comment("checking step \'return SicilyBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp123, "return of SicilyBind, state S313");
            TCSicilyAuth_2K8R2S377();
            this.Manager.EndTest();
        }
        #endregion
    }
}
