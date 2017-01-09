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
    [TestClass]
    public partial class TCFastAuth_2K8R2 : PtfTestClassBase
    {

        public TCFastAuth_2K8R2()
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
        public void Security_TCFastAuth_2K8R2S0()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S64\'");
            ProtocolMessageStructures.errorstatus temp0;
            this.Manager.Comment("executing step \'call FastBind(invalidUserName,validPassword,LDAP_PORT,True)\'");
            temp0 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp0, "return of FastBind, state S96");
            TCFastAuth_2K8R2S128();
            this.Manager.EndTest();
        }

        private void TCFastAuth_2K8R2S128()
        {
            this.Manager.Comment("reaching state \'S128\'");
        }
        #endregion

        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S10()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S69\'");
            ProtocolMessageStructures.errorstatus temp1;
            this.Manager.Comment("executing step \'call FastBind(invalidUserName,invalidPassword,LDAP_GC_PORT,True)\'" +
                    "");
            temp1 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp1, "return of FastBind, state S101");
            TCFastAuth_2K8R2S130();
            this.Manager.EndTest();
        }

        private void TCFastAuth_2K8R2S130()
        {
            this.Manager.Comment("reaching state \'S130\'");
        }
        #endregion

        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S12()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S70\'");
            ProtocolMessageStructures.errorstatus temp2;
            this.Manager.Comment("executing step \'call FastBind(invalidUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp2 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp2, "return of FastBind, state S102");
            TCFastAuth_2K8R2S130();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S14()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S71\'");
            ProtocolMessageStructures.errorstatus temp3;
            this.Manager.Comment("executing step \'call FastBind(validUserName,invalidPassword,LDAP_PORT,False)\'");
            temp3 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp3, "return of FastBind, state S103");
            TCFastAuth_2K8R2S130();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S16()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S72\'");
            ProtocolMessageStructures.errorstatus temp4;
            this.Manager.Comment("executing step \'call FastBind(validUserName,invalidPassword,LDAP_GC_PORT,False)\'");
            temp4 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp4, "return of FastBind, state S104");
            TCFastAuth_2K8R2S130();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S18()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S73\'");
            ProtocolMessageStructures.errorstatus temp5;
            this.Manager.Comment("executing step \'call FastBind(invalidUserName,invalidPassword,LDAP_PORT,False)\'");
            temp5 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp5, "return of FastBind, state S105");
            TCFastAuth_2K8R2S130();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S2()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S65\'");
            ProtocolMessageStructures.errorstatus temp6;
            this.Manager.Comment("executing step \'call FastBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp6 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return FastBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp6, "return of FastBind, state S97");
            TCFastAuth_2K8R2S129();
            this.Manager.EndTest();
        }

        private void TCFastAuth_2K8R2S129()
        {
            this.Manager.Comment("reaching state \'S129\'");
        }
        #endregion

        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S20()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S74\'");
            ProtocolMessageStructures.errorstatus temp7;
            this.Manager.Comment("executing step \'call FastBind(validUserName,invalidPassword,LDAP_PORT,True)\'");
            temp7 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp7, "return of FastBind, state S106");
            TCFastAuth_2K8R2S130();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S22()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S75\'");
            ProtocolMessageStructures.errorstatus temp8;
            this.Manager.Comment("executing step \'call FastBind(invalidUserName,invalidPassword,LDAP_PORT,True)\'");
            temp8 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp8, "return of FastBind, state S107");
            TCFastAuth_2K8R2S130();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S24()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S76\'");
            ProtocolMessageStructures.errorstatus temp9;
            this.Manager.Comment("executing step \'call FastBind(invalidUserName,invalidPassword,LDAP_GC_PORT,False)" +
                    "\'");
            temp9 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp9, "return of FastBind, state S108");
            TCFastAuth_2K8R2S130();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S26()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S77\'");
            ProtocolMessageStructures.errorstatus temp10;
            this.Manager.Comment("executing step \'call FastBind(validUserName,invalidPassword,LDAP_GC_PORT,True)\'");
            temp10 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp10, "return of FastBind, state S109");
            TCFastAuth_2K8R2S130();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S28()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S78\'");
            ProtocolMessageStructures.errorstatus temp11;
            this.Manager.Comment("executing step \'call FastBind(invalidUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp11 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp11, "return of FastBind, state S110");
            TCFastAuth_2K8R2S130();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S30()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S79\'");
            ProtocolMessageStructures.errorstatus temp12;
            this.Manager.Comment("executing step \'call FastBind(invalidUserName,validPassword,LDAP_PORT,False)\'");
            temp12 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp12, "return of FastBind, state S111");
            TCFastAuth_2K8R2S130();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S32()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S80\'");
            ProtocolMessageStructures.errorstatus temp13;
            this.Manager.Comment("executing step \'call FastBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp13 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("checking step \'return FastBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp13, "return of FastBind, state S112");
            TCFastAuth_2K8R2S131();
            this.Manager.EndTest();
        }

        private void TCFastAuth_2K8R2S131()
        {
            this.Manager.Comment("reaching state \'S131\'");
        }
        #endregion

        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S34()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S81\'");
            ProtocolMessageStructures.errorstatus temp14;
            this.Manager.Comment("executing step \'call FastBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp14 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return FastBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp14, "return of FastBind, state S113");
            TCFastAuth_2K8R2S131();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S36()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S82\'");
            ProtocolMessageStructures.errorstatus temp15;
            this.Manager.Comment("executing step \'call FastBind(validUserName,validPassword,LDAP_PORT,False)\'");
            temp15 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("checking step \'return FastBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp15, "return of FastBind, state S114");
            TCFastAuth_2K8R2S131();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S38()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S83\'");
            ProtocolMessageStructures.errorstatus temp16;
            this.Manager.Comment("executing step \'call FastBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp16 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return FastBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp16, "return of FastBind, state S115");
            TCFastAuth_2K8R2S131();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S4()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S66\'");
            ProtocolMessageStructures.errorstatus temp17;
            this.Manager.Comment("executing step \'call FastBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp17 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("checking step \'return FastBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp17, "return of FastBind, state S98");
            TCFastAuth_2K8R2S129();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S40
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S40()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S84\'");
            ProtocolMessageStructures.errorstatus temp18;
            this.Manager.Comment("executing step \'call FastBind(invalidUserName,invalidPassword,LDAP_GC_PORT,True)\'" +
                    "");
            temp18 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp18, "return of FastBind, state S116");
            TCFastAuth_2K8R2S128();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S42
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S42()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S85\'");
            ProtocolMessageStructures.errorstatus temp19;
            this.Manager.Comment("executing step \'call FastBind(invalidUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp19 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp19, "return of FastBind, state S117");
            TCFastAuth_2K8R2S128();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S44
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S44()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S44");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S86\'");
            ProtocolMessageStructures.errorstatus temp20;
            this.Manager.Comment("executing step \'call FastBind(validUserName,invalidPassword,LDAP_PORT,False)\'");
            temp20 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp20, "return of FastBind, state S118");
            TCFastAuth_2K8R2S128();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S46
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S46()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S46");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S87\'");
            ProtocolMessageStructures.errorstatus temp21;
            this.Manager.Comment("executing step \'call FastBind(validUserName,invalidPassword,LDAP_GC_PORT,False)\'");
            temp21 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp21, "return of FastBind, state S119");
            TCFastAuth_2K8R2S128();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S48
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S48()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S48");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S88\'");
            ProtocolMessageStructures.errorstatus temp22;
            this.Manager.Comment("executing step \'call FastBind(invalidUserName,invalidPassword,LDAP_PORT,False)\'");
            temp22 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp22, "return of FastBind, state S120");
            TCFastAuth_2K8R2S128();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S50
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S50()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S50");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S89\'");
            ProtocolMessageStructures.errorstatus temp23;
            this.Manager.Comment("executing step \'call FastBind(validUserName,invalidPassword,LDAP_PORT,True)\'");
            temp23 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp23, "return of FastBind, state S121");
            TCFastAuth_2K8R2S128();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S52
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S52()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S52");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S90\'");
            ProtocolMessageStructures.errorstatus temp24;
            this.Manager.Comment("executing step \'call FastBind(invalidUserName,invalidPassword,LDAP_PORT,True)\'");
            temp24 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp24, "return of FastBind, state S122");
            TCFastAuth_2K8R2S128();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S54
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S54()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S54");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S91\'");
            ProtocolMessageStructures.errorstatus temp25;
            this.Manager.Comment("executing step \'call FastBind(invalidUserName,invalidPassword,LDAP_GC_PORT,False)" +
                    "\'");
            temp25 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp25, "return of FastBind, state S123");
            TCFastAuth_2K8R2S128();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S56
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S56()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S56");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S92\'");
            ProtocolMessageStructures.errorstatus temp26;
            this.Manager.Comment("executing step \'call FastBind(validUserName,invalidPassword,LDAP_GC_PORT,True)\'");
            temp26 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp26, "return of FastBind, state S124");
            TCFastAuth_2K8R2S128();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S58
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S58()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S58");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S93\'");
            ProtocolMessageStructures.errorstatus temp27;
            this.Manager.Comment("executing step \'call FastBind(invalidUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp27 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp27, "return of FastBind, state S125");
            TCFastAuth_2K8R2S128();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S6()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S67\'");
            ProtocolMessageStructures.errorstatus temp28;
            this.Manager.Comment("executing step \'call FastBind(validUserName,validPassword,LDAP_PORT,False)\'");
            temp28 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return FastBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp28, "return of FastBind, state S99");
            TCFastAuth_2K8R2S129();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S60
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S60()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S60");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S94\'");
            ProtocolMessageStructures.errorstatus temp29;
            this.Manager.Comment("executing step \'call FastBind(invalidUserName,validPassword,LDAP_PORT,False)\'");
            temp29 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp29, "return of FastBind, state S126");
            TCFastAuth_2K8R2S128();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S62
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S62()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S62");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S95\'");
            ProtocolMessageStructures.errorstatus temp30;
            this.Manager.Comment("executing step \'call FastBind(invalidUserName,validPassword,LDAP_PORT,True)\'");
            temp30 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return FastBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp30, "return of FastBind, state S127");
            TCFastAuth_2K8R2S130();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCFastAuth_2K8R2S8()
        {
            this.Manager.BeginTest("TCFastAuth_2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S68\'");
            ProtocolMessageStructures.errorstatus temp31;
            this.Manager.Comment("executing step \'call FastBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp31 = this.IMS_ADTS_AuthenticationAuthInstance.FastBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("checking step \'return FastBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp31, "return of FastBind, state S100");
            TCFastAuth_2K8R2S129();
            this.Manager.EndTest();
        }
        #endregion
    }
}
