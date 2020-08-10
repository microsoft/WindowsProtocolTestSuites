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
    using Microsoft.Protocols.TestSuites.ActiveDirectory.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [System.CodeDom.Compiler.GeneratedCodeAttribute("Spec Explorer", "3.5.3146.0")]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class TCSASLAuth_2K8R2 : PtfTestClassBase
    {

        public TCSASLAuth_2K8R2()
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
        public void Security_TCSASLAuth_2K8R2S0()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S512\'");
            ProtocolMessageStructures.errorstatus temp0;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,invalidPassw" +
                    "ord,LDAP_SSL_GC_PORT,True)\'");
            temp0 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S768\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp0, "return of SASLAuthentication, state S768");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }

        private void TCSASLAuth_2K8R2S1024()
        {
            this.Manager.Comment("reaching state \'S1024\'");
        }
        #endregion

        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S10()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S517\'");
            ProtocolMessageStructures.errorstatus temp1;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,validPassword,LD" +
                    "AP_SSL_PORT,False)\'");
            temp1 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R468");
            this.Manager.Comment("reaching state \'S773\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp1, "return of SASLAuthentication, state S773");
            this.Manager.Comment("reaching state \'S1029\'");
            ProtocolMessageStructures.errorstatus temp2;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,userPassword,False)\'");
            temp2 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S1041\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp2, "return of AuthorizationCheck, state S1041");
            TCSASLAuth_2K8R2S1053();
            this.Manager.EndTest();
        }

        private void TCSASLAuth_2K8R2S1053()
        {
            this.Manager.Comment("reaching state \'S1053\'");
        }
        #endregion

        #region Test Starting in S100
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S100()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S100");
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S562\'");
            ProtocolMessageStructures.errorstatus temp3;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,validPassword," +
                    "LDAP_PORT,False)\'");
            temp3 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S818\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp3, "return of SASLAuthentication, state S818");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }

        private void TCSASLAuth_2K8R2S1025()
        {
            this.Manager.Comment("reaching state \'S1025\'");
        }
        #endregion

        #region Test Starting in S102
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S102()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S102");
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S563\'");
            ProtocolMessageStructures.errorstatus temp4;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,invalidPassword," +
                    "LDAP_PORT,False)\'");
            temp4 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S819\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp4, "return of SASLAuthentication, state S819");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S104
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S104()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S104");
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S564\'");
            ProtocolMessageStructures.errorstatus temp5;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,invalidPasswo" +
                    "rd,LDAP_PORT,False)\'");
            temp5 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S820\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp5, "return of SASLAuthentication, state S820");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S106
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S106()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S106");
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S565\'");
            ProtocolMessageStructures.errorstatus temp6;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,invalidPasswor" +
                    "d,LDAP_PORT,False)\'");
            temp6 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S821\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp6, "return of SASLAuthentication, state S821");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S108
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S108()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S108");
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S566\'");
            ProtocolMessageStructures.errorstatus temp7;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,invalidPassword," +
                    "LDAP_PORT,False)\'");
            temp7 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S822\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp7, "return of SASLAuthentication, state S822");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S110
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S110()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S110");
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S567\'");
            ProtocolMessageStructures.errorstatus temp8;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,invalidPasswor" +
                    "d,LDAP_PORT,False)\'");
            temp8 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S823\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp8, "return of SASLAuthentication, state S823");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S112
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S112()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S112");
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S568\'");
            ProtocolMessageStructures.errorstatus temp9;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,invalidPass" +
                    "word,LDAP_PORT,False)\'");
            temp9 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S824\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp9, "return of SASLAuthentication, state S824");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S114
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S114()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S114");
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S569\'");
            ProtocolMessageStructures.errorstatus temp10;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,validPassword," +
                    "LDAP_PORT,True)\'");
            temp10 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S825\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp10, "return of SASLAuthentication, state S825");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S116
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S116()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S116");
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S570\'");
            ProtocolMessageStructures.errorstatus temp11;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,validPasswo" +
                    "rd,LDAP_PORT,True)\'");
            temp11 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S826\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp11, "return of SASLAuthentication, state S826");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S118
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S118()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S118");
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S571\'");
            ProtocolMessageStructures.errorstatus temp12;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,validPassword," +
                    "LDAP_PORT,True)\'");
            temp12 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S827\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp12, "return of SASLAuthentication, state S827");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S12()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S518\'");
            ProtocolMessageStructures.errorstatus temp13;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,validPassword," +
                    "LDAP_SSL_PORT,False)\'");
            temp13 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S774\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp13, "return of SASLAuthentication, state S774");
            this.Manager.Comment("reaching state \'S1030\'");
            ProtocolMessageStructures.errorstatus temp14;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,User_Force_Change_Password,userPas" +
                    "sword,False)\'");
            temp14 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.User_Force_Change_Password, ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S1042\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp14, "return of AuthorizationCheck, state S1042");
            TCSASLAuth_2K8R2S1053();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S120
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S120()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S120");
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S572\'");
            ProtocolMessageStructures.errorstatus temp15;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,validPasswor" +
                    "d,LDAP_PORT,True)\'");
            temp15 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S828\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp15, "return of SASLAuthentication, state S828");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S122
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S122()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S122");
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S573\'");
            ProtocolMessageStructures.errorstatus temp16;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,invalidPassword," +
                    "LDAP_PORT,True)\'");
            temp16 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S829\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp16, "return of SASLAuthentication, state S829");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S124
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S124()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S124");
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S574\'");
            ProtocolMessageStructures.errorstatus temp17;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,invalidPasswor" +
                    "d,LDAP_PORT,True)\'");
            temp17 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S830\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp17, "return of SASLAuthentication, state S830");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S126
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S126()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S126");
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S575\'");
            ProtocolMessageStructures.errorstatus temp18;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,invalidPasswo" +
                    "rd,LDAP_PORT,True)\'");
            temp18 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S831\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp18, "return of SASLAuthentication, state S831");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S128
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S128()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S128");
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S576\'");
            ProtocolMessageStructures.errorstatus temp19;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,invalidPassword," +
                    "LDAP_PORT,True)\'");
            temp19 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S832\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp19, "return of SASLAuthentication, state S832");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S130
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S130()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S130");
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S577\'");
            ProtocolMessageStructures.errorstatus temp20;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,invalidPasswor" +
                    "d,LDAP_SSL_PORT,False)\'");
            temp20 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S833\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp20, "return of SASLAuthentication, state S833");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S132
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S132()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S132");
            this.Manager.Comment("reaching state \'S132\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S133\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S578\'");
            ProtocolMessageStructures.errorstatus temp21;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,validPasswor" +
                    "d,LDAP_SSL_PORT,False)\'");
            temp21 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S834\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp21, "return of SASLAuthentication, state S834");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S134
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S134()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S134");
            this.Manager.Comment("reaching state \'S134\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S135\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S579\'");
            ProtocolMessageStructures.errorstatus temp22;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,validPassword," +
                    "LDAP_SSL_PORT,False)\'");
            temp22 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S835\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp22, "return of SASLAuthentication, state S835");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S136
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S136()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S136");
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S137\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S580\'");
            ProtocolMessageStructures.errorstatus temp23;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,invalidPasswor" +
                    "d,LDAP_SSL_PORT,False)\'");
            temp23 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S836\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp23, "return of SASLAuthentication, state S836");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S138
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S138()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S138");
            this.Manager.Comment("reaching state \'S138\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S139\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S581\'");
            ProtocolMessageStructures.errorstatus temp24;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,invalidPass" +
                    "word,LDAP_SSL_PORT,False)\'");
            temp24 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S837\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp24, "return of SASLAuthentication, state S837");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S14()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S519\'");
            ProtocolMessageStructures.errorstatus temp25;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_SSL_PORT,False)\'");
            temp25 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S775\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp25, "return of SASLAuthentication, state S775");
            this.Manager.Comment("reaching state \'S1031\'");
            ProtocolMessageStructures.errorstatus temp26;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,NotSet,use" +
                    "rPassword,False)\'");
            temp26 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1043\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp26, "return of AuthorizationCheck, state S1043");
            this.Manager.Comment("reaching state \'S1054\'");
            ProtocolMessageStructures.errorstatus temp27;
            this.Manager.Comment("executing step \'call AdminPasswordReset(\"PasswordNew1\",True)\'");
            temp27 = this.IMS_ADTS_AuthenticationAuthInstance.AdminPasswordReset("PasswordNew1", true);
            this.Manager.Checkpoint("MS-ADTS-Security_R37");
            this.Manager.Comment("reaching state \'S1063\'");
            this.Manager.Comment("checking step \'return AdminPasswordReset/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp27, "return of AdminPasswordReset, state S1063");
            TCSASLAuth_2K8R2S1069();
            this.Manager.EndTest();
        }

        private void TCSASLAuth_2K8R2S1069()
        {
            this.Manager.Comment("reaching state \'S1069\'");
        }
        #endregion

        #region Test Starting in S140
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S140()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S140");
            this.Manager.Comment("reaching state \'S140\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S141\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S582\'");
            ProtocolMessageStructures.errorstatus temp28;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,invalidPasswor" +
                    "d,LDAP_SSL_PORT,False)\'");
            temp28 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S838\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp28, "return of SASLAuthentication, state S838");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S142
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S142()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S142");
            this.Manager.Comment("reaching state \'S142\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S143\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S583\'");
            ProtocolMessageStructures.errorstatus temp29;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,invalidPassw" +
                    "ord,LDAP_SSL_PORT,False)\'");
            temp29 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S839\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp29, "return of SASLAuthentication, state S839");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S144
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S144()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S144");
            this.Manager.Comment("reaching state \'S144\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S145\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S584\'");
            ProtocolMessageStructures.errorstatus temp30;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,invalidPassword," +
                    "LDAP_SSL_PORT,False)\'");
            temp30 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S840\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp30, "return of SASLAuthentication, state S840");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S146
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S146()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S146");
            this.Manager.Comment("reaching state \'S146\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S147\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S585\'");
            ProtocolMessageStructures.errorstatus temp31;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,invalidPasswo" +
                    "rd,LDAP_SSL_PORT,False)\'");
            temp31 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S841\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp31, "return of SASLAuthentication, state S841");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S148
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S148()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S148");
            this.Manager.Comment("reaching state \'S148\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S149\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S586\'");
            ProtocolMessageStructures.errorstatus temp32;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,validPassword," +
                    "LDAP_SSL_PORT,False)\'");
            temp32 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S842\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp32, "return of SASLAuthentication, state S842");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S150
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S150()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S150");
            this.Manager.Comment("reaching state \'S150\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S151\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S587\'");
            ProtocolMessageStructures.errorstatus temp33;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,validPasswo" +
                    "rd,LDAP_SSL_PORT,False)\'");
            temp33 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S843\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp33, "return of SASLAuthentication, state S843");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S152
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S152()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S152");
            this.Manager.Comment("reaching state \'S152\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S153\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S588\'");
            ProtocolMessageStructures.errorstatus temp34;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,validPassword," +
                    "LDAP_SSL_GC_PORT,False)\'");
            temp34 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S844\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp34, "return of SASLAuthentication, state S844");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S154
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S154()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S154");
            this.Manager.Comment("reaching state \'S154\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S155\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S589\'");
            ProtocolMessageStructures.errorstatus temp35;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,invalidPasswor" +
                    "d,LDAP_SSL_GC_PORT,False)\'");
            temp35 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S845\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp35, "return of SASLAuthentication, state S845");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S156
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S156()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S156");
            this.Manager.Comment("reaching state \'S156\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S157\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S590\'");
            ProtocolMessageStructures.errorstatus temp36;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,invalidPasswor" +
                    "d,LDAP_GC_PORT,False)\'");
            temp36 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S846\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp36, "return of SASLAuthentication, state S846");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S158
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S158()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S158");
            this.Manager.Comment("reaching state \'S158\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S159\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S591\'");
            ProtocolMessageStructures.errorstatus temp37;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,invalidPassw" +
                    "ord,LDAP_PORT,True)\'");
            temp37 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S847\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp37, "return of SASLAuthentication, state S847");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S16()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S520\'");
            ProtocolMessageStructures.errorstatus temp38;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,validPassword" +
                    ",LDAP_SSL_PORT,False)\'");
            temp38 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R470");
            this.Manager.Comment("reaching state \'S776\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp38, "return of SASLAuthentication, state S776");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }

        private void TCSASLAuth_2K8R2S1026()
        {
            this.Manager.Comment("reaching state \'S1026\'");
            ProtocolMessageStructures.errorstatus temp39;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Force" +
                    "_Change_Password,userPassword,False)\'");
            temp39 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ProtocolMessageStructures.ControlAccessRights.User_Force_Change_Password, ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1038\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp39, "return of AuthorizationCheck, state S1038");
            this.Manager.Comment("reaching state \'S1050\'");
            ProtocolMessageStructures.errorstatus temp40;
            this.Manager.Comment("executing step \'call AdminPasswordReset(\"PasswordNew1\",True)\'");
            temp40 = this.IMS_ADTS_AuthenticationAuthInstance.AdminPasswordReset("PasswordNew1", true);
            this.Manager.Checkpoint("MS-ADTS-Security_R32");
            this.Manager.Checkpoint("MS-ADTS-Security_R36");
            this.Manager.Comment("reaching state \'S1060\'");
            this.Manager.Comment("checking step \'return AdminPasswordReset/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp40, "return of AdminPasswordReset, state S1060");
            TCSASLAuth_2K8R2S1068();
        }

        private void TCSASLAuth_2K8R2S1068()
        {
            this.Manager.Comment("reaching state \'S1068\'");
        }
        #endregion

        #region Test Starting in S160
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S160()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S160");
            this.Manager.Comment("reaching state \'S160\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S161\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S592\'");
            ProtocolMessageStructures.errorstatus temp41;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,invalidPasswor" +
                    "d,LDAP_PORT,True)\'");
            temp41 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S848\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp41, "return of SASLAuthentication, state S848");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S162
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S162()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S162");
            this.Manager.Comment("reaching state \'S162\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S163\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S593\'");
            ProtocolMessageStructures.errorstatus temp42;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,invalidPasswor" +
                    "d,LDAP_PORT,True)\'");
            temp42 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S849\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp42, "return of SASLAuthentication, state S849");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S164
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S164()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S164");
            this.Manager.Comment("reaching state \'S164\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S165\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S594\'");
            ProtocolMessageStructures.errorstatus temp43;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,invalidPass" +
                    "word,LDAP_PORT,True)\'");
            temp43 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S850\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp43, "return of SASLAuthentication, state S850");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S166
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S166()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S166");
            this.Manager.Comment("reaching state \'S166\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S167\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S595\'");
            ProtocolMessageStructures.errorstatus temp44;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,invalidPassword," +
                    "LDAP_SSL_GC_PORT,False)\'");
            temp44 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S851\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp44, "return of SASLAuthentication, state S851");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S168
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S168()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S168");
            this.Manager.Comment("reaching state \'S168\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S169\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S596\'");
            ProtocolMessageStructures.errorstatus temp45;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,invalidPasswo" +
                    "rd,LDAP_SSL_GC_PORT,False)\'");
            temp45 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S852\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp45, "return of SASLAuthentication, state S852");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S170
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S170()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S170");
            this.Manager.Comment("reaching state \'S170\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S171\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S597\'");
            ProtocolMessageStructures.errorstatus temp46;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,validPasswo" +
                    "rd,LDAP_SSL_GC_PORT,False)\'");
            temp46 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S853\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp46, "return of SASLAuthentication, state S853");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S172
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S172()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S172");
            this.Manager.Comment("reaching state \'S172\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S173\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S598\'");
            ProtocolMessageStructures.errorstatus temp47;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,validPassword," +
                    "LDAP_SSL_GC_PORT,False)\'");
            temp47 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S854\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp47, "return of SASLAuthentication, state S854");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S174
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S174()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S174");
            this.Manager.Comment("reaching state \'S174\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S175\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S599\'");
            ProtocolMessageStructures.errorstatus temp48;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,invalidPasswor" +
                    "d,LDAP_SSL_GC_PORT,False)\'");
            temp48 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S855\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp48, "return of SASLAuthentication, state S855");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S176
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S176()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S176");
            this.Manager.Comment("reaching state \'S176\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S177\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S600\'");
            ProtocolMessageStructures.errorstatus temp49;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,validPasswor" +
                    "d,LDAP_SSL_GC_PORT,False)\'");
            temp49 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S856\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp49, "return of SASLAuthentication, state S856");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S178
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S178()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S178");
            this.Manager.Comment("reaching state \'S178\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S179\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S601\'");
            ProtocolMessageStructures.errorstatus temp50;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,invalidPasswor" +
                    "d,LDAP_SSL_GC_PORT,False)\'");
            temp50 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S857\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp50, "return of SASLAuthentication, state S857");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S18()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S521\'");
            ProtocolMessageStructures.errorstatus temp51;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,validPassword," +
                    "LDAP_GC_PORT,False)\'");
            temp51 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S777\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp51, "return of SASLAuthentication, state S777");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S180
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S180()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S180");
            this.Manager.Comment("reaching state \'S180\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S181\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S602\'");
            ProtocolMessageStructures.errorstatus temp52;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,invalidPass" +
                    "word,LDAP_SSL_GC_PORT,False)\'");
            temp52 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S858\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp52, "return of SASLAuthentication, state S858");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S182
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S182()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S182");
            this.Manager.Comment("reaching state \'S182\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S183\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S603\'");
            ProtocolMessageStructures.errorstatus temp53;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,invalidPassw" +
                    "ord,LDAP_SSL_GC_PORT,False)\'");
            temp53 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S859\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp53, "return of SASLAuthentication, state S859");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S184
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S184()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S184");
            this.Manager.Comment("reaching state \'S184\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S185\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S604\'");
            ProtocolMessageStructures.errorstatus temp54;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,invalidPasswor" +
                    "d,LDAP_GC_PORT,False)\'");
            temp54 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S860\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp54, "return of SASLAuthentication, state S860");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S186
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S186()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S186");
            this.Manager.Comment("reaching state \'S186\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S187\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S605\'");
            ProtocolMessageStructures.errorstatus temp55;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,invalidPassw" +
                    "ord,LDAP_GC_PORT,False)\'");
            temp55 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S861\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp55, "return of SASLAuthentication, state S861");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S188
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S188()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S188");
            this.Manager.Comment("reaching state \'S188\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S189\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S606\'");
            ProtocolMessageStructures.errorstatus temp56;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,invalidPassword," +
                    "LDAP_SSL_GC_PORT,False)\'");
            temp56 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S862\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp56, "return of SASLAuthentication, state S862");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S190
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S190()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S190");
            this.Manager.Comment("reaching state \'S190\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S191\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S607\'");
            ProtocolMessageStructures.errorstatus temp57;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,invalidPasswor" +
                    "d,LDAP_GC_PORT,True)\'");
            temp57 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S863\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp57, "return of SASLAuthentication, state S863");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S192
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S192()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S192");
            this.Manager.Comment("reaching state \'S192\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S193\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S608\'");
            ProtocolMessageStructures.errorstatus temp58;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,invalidPassw" +
                    "ord,LDAP_GC_PORT,True)\'");
            temp58 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S864\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp58, "return of SASLAuthentication, state S864");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S194
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S194()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S194");
            this.Manager.Comment("reaching state \'S194\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S195\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S609\'");
            ProtocolMessageStructures.errorstatus temp59;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,invalidPasswor" +
                    "d,LDAP_SSL_PORT,True)\'");
            temp59 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S865\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp59, "return of SASLAuthentication, state S865");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S196
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S196()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S196");
            this.Manager.Comment("reaching state \'S196\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S197\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S610\'");
            ProtocolMessageStructures.errorstatus temp60;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_SSL_PORT,True)\'");
            temp60 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S866\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp60, "return of SASLAuthentication, state S866");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S198
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S198()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S198");
            this.Manager.Comment("reaching state \'S198\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S199\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S611\'");
            ProtocolMessageStructures.errorstatus temp61;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,invalidPassword," +
                    "LDAP_SSL_GC_PORT,True)\'");
            temp61 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S867\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp61, "return of SASLAuthentication, state S867");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S2()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S513\'");
            ProtocolMessageStructures.errorstatus temp62;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,invalidPasswor" +
                    "d,LDAP_GC_PORT,True)\'");
            temp62 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S769\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp62, "return of SASLAuthentication, state S769");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S20()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S522\'");
            ProtocolMessageStructures.errorstatus temp63;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,validPassword" +
                    ",LDAP_GC_PORT,False)\'");
            temp63 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R470");
            this.Manager.Comment("reaching state \'S778\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp63, "return of SASLAuthentication, state S778");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S200
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S200()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S200");
            this.Manager.Comment("reaching state \'S200\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S201\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S612\'");
            ProtocolMessageStructures.errorstatus temp64;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_SSL_GC_PORT,True)\'");
            temp64 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S868\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp64, "return of SASLAuthentication, state S868");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S202
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S202()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S202");
            this.Manager.Comment("reaching state \'S202\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S203\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S613\'");
            ProtocolMessageStructures.errorstatus temp65;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,validPassword," +
                    "LDAP_SSL_GC_PORT,True)\'");
            temp65 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S869\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp65, "return of SASLAuthentication, state S869");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S204
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S204()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S204");
            this.Manager.Comment("reaching state \'S204\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S205\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S614\'");
            ProtocolMessageStructures.errorstatus temp66;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,invalidPasswor" +
                    "d,LDAP_SSL_GC_PORT,True)\'");
            temp66 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S870\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp66, "return of SASLAuthentication, state S870");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S206
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S206()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S206");
            this.Manager.Comment("reaching state \'S206\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S207\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S615\'");
            ProtocolMessageStructures.errorstatus temp67;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,validPassword," +
                    "LDAP_SSL_PORT,True)\'");
            temp67 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S871\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp67, "return of SASLAuthentication, state S871");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S208
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S208()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S208");
            this.Manager.Comment("reaching state \'S208\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S209\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S616\'");
            ProtocolMessageStructures.errorstatus temp68;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,invalidPasswor" +
                    "d,LDAP_SSL_PORT,True)\'");
            temp68 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S872\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp68, "return of SASLAuthentication, state S872");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S210
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S210()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S210");
            this.Manager.Comment("reaching state \'S210\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S211\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S617\'");
            ProtocolMessageStructures.errorstatus temp69;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,invalidPassword," +
                    "LDAP_SSL_PORT,True)\'");
            temp69 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S873\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp69, "return of SASLAuthentication, state S873");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S212
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S212()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S212");
            this.Manager.Comment("reaching state \'S212\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S213\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S618\'");
            ProtocolMessageStructures.errorstatus temp70;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,validPassword" +
                    ",LDAP_SSL_PORT,True)\'");
            temp70 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S874\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp70, "return of SASLAuthentication, state S874");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S214
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S214()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S214");
            this.Manager.Comment("reaching state \'S214\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S215\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S619\'");
            ProtocolMessageStructures.errorstatus temp71;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,validPassword," +
                    "LDAP_SSL_PORT,True)\'");
            temp71 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S875\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp71, "return of SASLAuthentication, state S875");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S216
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S216()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S216");
            this.Manager.Comment("reaching state \'S216\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S217\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S620\'");
            ProtocolMessageStructures.errorstatus temp72;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,validPassword,LD" +
                    "AP_SSL_PORT,True)\'");
            temp72 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S876\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp72, "return of SASLAuthentication, state S876");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S218
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S218()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S218");
            this.Manager.Comment("reaching state \'S218\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S219\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S621\'");
            ProtocolMessageStructures.errorstatus temp73;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,validPassword," +
                    "LDAP_SSL_PORT,True)\'");
            temp73 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S877\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp73, "return of SASLAuthentication, state S877");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S22()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S523\'");
            ProtocolMessageStructures.errorstatus temp74;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_GC_PORT,False)\'");
            temp74 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S779\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp74, "return of SASLAuthentication, state S779");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S220
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S220()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S220");
            this.Manager.Comment("reaching state \'S220\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S221\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S622\'");
            ProtocolMessageStructures.errorstatus temp75;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,validPasswo" +
                    "rd,LDAP_SSL_PORT,True)\'");
            temp75 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S878\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp75, "return of SASLAuthentication, state S878");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S222
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S222()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S222");
            this.Manager.Comment("reaching state \'S222\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S223\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S623\'");
            ProtocolMessageStructures.errorstatus temp76;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,validPasswor" +
                    "d,LDAP_SSL_PORT,True)\'");
            temp76 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S879\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp76, "return of SASLAuthentication, state S879");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S224
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S224()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S224");
            this.Manager.Comment("reaching state \'S224\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S225\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S624\'");
            ProtocolMessageStructures.errorstatus temp77;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,invalidPassw" +
                    "ord,LDAP_SSL_PORT,True)\'");
            temp77 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S880\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp77, "return of SASLAuthentication, state S880");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S226
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S226()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S226");
            this.Manager.Comment("reaching state \'S226\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S227\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S625\'");
            ProtocolMessageStructures.errorstatus temp78;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,invalidPasswo" +
                    "rd,LDAP_SSL_PORT,True)\'");
            temp78 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S881\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp78, "return of SASLAuthentication, state S881");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S228
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S228()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S228");
            this.Manager.Comment("reaching state \'S228\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S229\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S626\'");
            ProtocolMessageStructures.errorstatus temp79;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,invalidPassword," +
                    "LDAP_SSL_PORT,True)\'");
            temp79 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S882\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp79, "return of SASLAuthentication, state S882");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S230
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S230()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S230");
            this.Manager.Comment("reaching state \'S230\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S231\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S627\'");
            ProtocolMessageStructures.errorstatus temp80;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,invalidPasswor" +
                    "d,LDAP_SSL_PORT,True)\'");
            temp80 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S883\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp80, "return of SASLAuthentication, state S883");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S232
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S232()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S232");
            this.Manager.Comment("reaching state \'S232\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S233\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S628\'");
            ProtocolMessageStructures.errorstatus temp81;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,invalidPass" +
                    "word,LDAP_SSL_PORT,True)\'");
            temp81 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S884\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp81, "return of SASLAuthentication, state S884");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S234
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S234()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S234");
            this.Manager.Comment("reaching state \'S234\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S235\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S629\'");
            ProtocolMessageStructures.errorstatus temp82;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,validPasswo" +
                    "rd,LDAP_SSL_GC_PORT,True)\'");
            temp82 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S885\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp82, "return of SASLAuthentication, state S885");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S236
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S236()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S236");
            this.Manager.Comment("reaching state \'S236\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S237\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S630\'");
            ProtocolMessageStructures.errorstatus temp83;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,validPassword," +
                    "LDAP_SSL_GC_PORT,True)\'");
            temp83 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S886\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp83, "return of SASLAuthentication, state S886");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S238
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S238()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S238");
            this.Manager.Comment("reaching state \'S238\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S239\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S631\'");
            ProtocolMessageStructures.errorstatus temp84;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,validPasswor" +
                    "d,LDAP_SSL_GC_PORT,True)\'");
            temp84 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S887\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp84, "return of SASLAuthentication, state S887");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S24()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S524\'");
            ProtocolMessageStructures.errorstatus temp85;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,validPassword,LD" +
                    "AP_GC_PORT,False)\'");
            temp85 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R468");
            this.Manager.Comment("reaching state \'S780\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp85, "return of SASLAuthentication, state S780");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S240
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S240()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S240");
            this.Manager.Comment("reaching state \'S240\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S241\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S632\'");
            ProtocolMessageStructures.errorstatus temp86;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,validPassword,LD" +
                    "AP_SSL_GC_PORT,True)\'");
            temp86 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S888\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp86, "return of SASLAuthentication, state S888");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S242
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S242()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S242");
            this.Manager.Comment("reaching state \'S242\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S243\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S633\'");
            ProtocolMessageStructures.errorstatus temp87;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,validPassword" +
                    ",LDAP_SSL_GC_PORT,True)\'");
            temp87 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S889\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp87, "return of SASLAuthentication, state S889");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S244
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S244()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S244");
            this.Manager.Comment("reaching state \'S244\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S245\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S634\'");
            ProtocolMessageStructures.errorstatus temp88;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,validPassword," +
                    "LDAP_SSL_GC_PORT,True)\'");
            temp88 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S890\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp88, "return of SASLAuthentication, state S890");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S246
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S246()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S246");
            this.Manager.Comment("reaching state \'S246\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S247\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S635\'");
            ProtocolMessageStructures.errorstatus temp89;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,invalidPasswor" +
                    "d,LDAP_SSL_GC_PORT,True)\'");
            temp89 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S891\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp89, "return of SASLAuthentication, state S891");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S248
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S248()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S248");
            this.Manager.Comment("reaching state \'S248\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S249\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S636\'");
            ProtocolMessageStructures.errorstatus temp90;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,invalidPassword," +
                    "LDAP_SSL_GC_PORT,True)\'");
            temp90 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S892\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp90, "return of SASLAuthentication, state S892");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S250
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S250()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S250");
            this.Manager.Comment("reaching state \'S250\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S251\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S637\'");
            ProtocolMessageStructures.errorstatus temp91;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,invalidPasswo" +
                    "rd,LDAP_SSL_GC_PORT,True)\'");
            temp91 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S893\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp91, "return of SASLAuthentication, state S893");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S252
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S252()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S252");
            this.Manager.Comment("reaching state \'S252\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S253\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S638\'");
            ProtocolMessageStructures.errorstatus temp92;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,invalidPasswor" +
                    "d,LDAP_SSL_GC_PORT,True)\'");
            temp92 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S894\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp92, "return of SASLAuthentication, state S894");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S254
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S254()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S254");
            this.Manager.Comment("reaching state \'S254\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S255\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S639\'");
            ProtocolMessageStructures.errorstatus temp93;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,invalidPass" +
                    "word,LDAP_SSL_GC_PORT,True)\'");
            temp93 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S895\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp93, "return of SASLAuthentication, state S895");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S256
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S256()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S256");
            this.Manager.Comment("reaching state \'S256\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S257\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S640\'");
            ProtocolMessageStructures.errorstatus temp94;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,validPassword," +
                    "LDAP_GC_PORT,True)\'");
            temp94 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S896\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp94, "return of SASLAuthentication, state S896");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }

        private void TCSASLAuth_2K8R2S1032()
        {
            this.Manager.Comment("reaching state \'S1032\'");
            ProtocolMessageStructures.errorstatus temp95;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Force" +
                    "_Change_Password,userPassword,False)\'");
            temp95 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ProtocolMessageStructures.ControlAccessRights.User_Force_Change_Password, ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1044\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp95, "return of AuthorizationCheck, state S1044");
            this.Manager.Comment("reaching state \'S1055\'");
            ProtocolMessageStructures.errorstatus temp96;
            this.Manager.Comment("executing step \'call AdminPasswordReset(\"PasswordNew1\",True)\'");
            temp96 = this.IMS_ADTS_AuthenticationAuthInstance.AdminPasswordReset("PasswordNew1", true);
            this.Manager.Checkpoint("MS-ADTS-Security_R32");
            this.Manager.Checkpoint("MS-ADTS-Security_R36");
            this.Manager.Comment("reaching state \'S1064\'");
            this.Manager.Comment("checking step \'return AdminPasswordReset/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp96, "return of AdminPasswordReset, state S1064");
            TCSASLAuth_2K8R2S1070();
        }

        private void TCSASLAuth_2K8R2S1070()
        {
            this.Manager.Comment("reaching state \'S1070\'");
        }
        #endregion

        #region Test Starting in S258
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S258()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S258");
            this.Manager.Comment("reaching state \'S258\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S259\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S641\'");
            ProtocolMessageStructures.errorstatus temp97;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_GC_PORT,True)\'");
            temp97 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S897\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp97, "return of SASLAuthentication, state S897");
            this.Manager.Comment("reaching state \'S1033\'");
            ProtocolMessageStructures.errorstatus temp98;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,NotSet,use" +
                    "rPassword,False)\'");
            temp98 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1045\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp98, "return of AuthorizationCheck, state S1045");
            this.Manager.Comment("reaching state \'S1056\'");
            ProtocolMessageStructures.errorstatus temp99;
            this.Manager.Comment("executing step \'call AdminPasswordReset(\"PasswordNew1\",False)\'");
            temp99 = this.IMS_ADTS_AuthenticationAuthInstance.AdminPasswordReset("PasswordNew1", false);
            this.Manager.Checkpoint("MS-ADTS-Security_R37");
            this.Manager.Comment("reaching state \'S1065\'");
            this.Manager.Comment("checking step \'return AdminPasswordReset/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp99, "return of AdminPasswordReset, state S1065");
            TCSASLAuth_2K8R2S1071();
            this.Manager.EndTest();
        }

        private void TCSASLAuth_2K8R2S1071()
        {
            this.Manager.Comment("reaching state \'S1071\'");
        }
        #endregion

        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S26()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S525\'");
            ProtocolMessageStructures.errorstatus temp100;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,validPassword," +
                    "LDAP_SSL_GC_PORT,False)\'");
            temp100 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S781\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp100, "return of SASLAuthentication, state S781");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S260
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S260()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S260");
            this.Manager.Comment("reaching state \'S260\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S261\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S642\'");
            ProtocolMessageStructures.errorstatus temp101;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,validPassword" +
                    ",LDAP_GC_PORT,True)\'");
            temp101 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R470");
            this.Manager.Comment("reaching state \'S898\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp101, "return of SASLAuthentication, state S898");
            this.Manager.Comment("reaching state \'S1034\'");
            ProtocolMessageStructures.errorstatus temp102;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Force" +
                    "_Change_Password,userPassword,False)\'");
            temp102 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ProtocolMessageStructures.ControlAccessRights.User_Force_Change_Password, ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1046\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp102, "return of AuthorizationCheck, state S1046");
            this.Manager.Comment("reaching state \'S1057\'");
            ProtocolMessageStructures.errorstatus temp103;
            this.Manager.Comment("executing step \'call AdminPasswordReset(\"PasswordNew1\",False)\'");
            temp103 = this.IMS_ADTS_AuthenticationAuthInstance.AdminPasswordReset("PasswordNew1", false);
            this.Manager.Checkpoint("MS-ADTS-Security_R32");
            this.Manager.Checkpoint("MS-ADTS-Security_R36");
            this.Manager.Comment("reaching state \'S1066\'");
            this.Manager.Comment("checking step \'return AdminPasswordReset/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp103, "return of AdminPasswordReset, state S1066");
            TCSASLAuth_2K8R2S1070();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S262
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S262()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S262");
            this.Manager.Comment("reaching state \'S262\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S263\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S643\'");
            ProtocolMessageStructures.errorstatus temp104;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,validPassword,LD" +
                    "AP_SSL_PORT,False)\'");
            temp104 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R468");
            this.Manager.Comment("reaching state \'S899\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp104, "return of SASLAuthentication, state S899");
            this.Manager.Comment("reaching state \'S1035\'");
            ProtocolMessageStructures.errorstatus temp105;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,userPassword,False)\'");
            temp105 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S1047\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp105, "return of AuthorizationCheck, state S1047");
            TCSASLAuth_2K8R2S1058();
            this.Manager.EndTest();
        }

        private void TCSASLAuth_2K8R2S1058()
        {
            this.Manager.Comment("reaching state \'S1058\'");
        }
        #endregion

        #region Test Starting in S264
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S264()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S264");
            this.Manager.Comment("reaching state \'S264\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S265\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S644\'");
            ProtocolMessageStructures.errorstatus temp106;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,validPassword," +
                    "LDAP_SSL_PORT,False)\'");
            temp106 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S900\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp106, "return of SASLAuthentication, state S900");
            this.Manager.Comment("reaching state \'S1036\'");
            ProtocolMessageStructures.errorstatus temp107;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,User_Force_Change_Password,userPas" +
                    "sword,False)\'");
            temp107 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.User_Force_Change_Password, ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S1048\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp107, "return of AuthorizationCheck, state S1048");
            TCSASLAuth_2K8R2S1058();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S266
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S266()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S266");
            this.Manager.Comment("reaching state \'S266\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S267\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S645\'");
            ProtocolMessageStructures.errorstatus temp108;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_SSL_PORT,False)\'");
            temp108 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S901\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp108, "return of SASLAuthentication, state S901");
            this.Manager.Comment("reaching state \'S1037\'");
            ProtocolMessageStructures.errorstatus temp109;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,NotSet,use" +
                    "rPassword,False)\'");
            temp109 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1049\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp109, "return of AuthorizationCheck, state S1049");
            this.Manager.Comment("reaching state \'S1059\'");
            ProtocolMessageStructures.errorstatus temp110;
            this.Manager.Comment("executing step \'call AdminPasswordReset(\"PasswordNew1\",True)\'");
            temp110 = this.IMS_ADTS_AuthenticationAuthInstance.AdminPasswordReset("PasswordNew1", true);
            this.Manager.Checkpoint("MS-ADTS-Security_R37");
            this.Manager.Comment("reaching state \'S1067\'");
            this.Manager.Comment("checking step \'return AdminPasswordReset/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp110, "return of AdminPasswordReset, state S1067");
            TCSASLAuth_2K8R2S1071();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S268
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S268()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S268");
            this.Manager.Comment("reaching state \'S268\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S269\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S646\'");
            ProtocolMessageStructures.errorstatus temp111;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,validPassword" +
                    ",LDAP_SSL_PORT,False)\'");
            temp111 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R470");
            this.Manager.Comment("reaching state \'S902\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp111, "return of SASLAuthentication, state S902");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S270
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S270()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S270");
            this.Manager.Comment("reaching state \'S270\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S271\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S647\'");
            ProtocolMessageStructures.errorstatus temp112;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,validPassword," +
                    "LDAP_GC_PORT,False)\'");
            temp112 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S903\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp112, "return of SASLAuthentication, state S903");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S272
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S272()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S272");
            this.Manager.Comment("reaching state \'S272\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S273\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S648\'");
            ProtocolMessageStructures.errorstatus temp113;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,validPassword" +
                    ",LDAP_GC_PORT,False)\'");
            temp113 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R470");
            this.Manager.Comment("reaching state \'S904\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp113, "return of SASLAuthentication, state S904");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S274
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S274()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S274");
            this.Manager.Comment("reaching state \'S274\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S275\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S649\'");
            ProtocolMessageStructures.errorstatus temp114;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_GC_PORT,False)\'");
            temp114 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S905\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp114, "return of SASLAuthentication, state S905");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S276
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S276()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S276");
            this.Manager.Comment("reaching state \'S276\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S277\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S650\'");
            ProtocolMessageStructures.errorstatus temp115;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,validPassword,LD" +
                    "AP_GC_PORT,False)\'");
            temp115 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R468");
            this.Manager.Comment("reaching state \'S906\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp115, "return of SASLAuthentication, state S906");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S278
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S278()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S278");
            this.Manager.Comment("reaching state \'S278\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S279\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S651\'");
            ProtocolMessageStructures.errorstatus temp116;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,validPassword," +
                    "LDAP_SSL_GC_PORT,False)\'");
            temp116 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S907\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp116, "return of SASLAuthentication, state S907");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S28()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S526\'");
            ProtocolMessageStructures.errorstatus temp117;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,validPassword," +
                    "LDAP_PORT,False)\'");
            temp117 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S782\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp117, "return of SASLAuthentication, state S782");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S280
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S280()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S280");
            this.Manager.Comment("reaching state \'S280\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S281\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S652\'");
            ProtocolMessageStructures.errorstatus temp118;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,validPassword," +
                    "LDAP_PORT,False)\'");
            temp118 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S908\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp118, "return of SASLAuthentication, state S908");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S282
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S282()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S282");
            this.Manager.Comment("reaching state \'S282\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S283\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S653\'");
            ProtocolMessageStructures.errorstatus temp119;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,validPassword" +
                    ",LDAP_SSL_GC_PORT,False)\'");
            temp119 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R470");
            this.Manager.Comment("reaching state \'S909\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp119, "return of SASLAuthentication, state S909");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S284
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S284()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S284");
            this.Manager.Comment("reaching state \'S284\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S285\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S654\'");
            ProtocolMessageStructures.errorstatus temp120;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,validPassword,LD" +
                    "AP_SSL_GC_PORT,False)\'");
            temp120 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R468");
            this.Manager.Comment("reaching state \'S910\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp120, "return of SASLAuthentication, state S910");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S286
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S286()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S286");
            this.Manager.Comment("reaching state \'S286\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S287\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S655\'");
            ProtocolMessageStructures.errorstatus temp121;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_SSL_GC_PORT,False)\'");
            temp121 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S911\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp121, "return of SASLAuthentication, state S911");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S288
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S288()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S288");
            this.Manager.Comment("reaching state \'S288\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S289\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S656\'");
            ProtocolMessageStructures.errorstatus temp122;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,validPassword" +
                    ",LDAP_PORT,False)\'");
            temp122 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R470");
            this.Manager.Comment("reaching state \'S912\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp122, "return of SASLAuthentication, state S912");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S290
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S290()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S290");
            this.Manager.Comment("reaching state \'S290\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S291\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S657\'");
            ProtocolMessageStructures.errorstatus temp123;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp123 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R468");
            this.Manager.Comment("reaching state \'S913\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp123, "return of SASLAuthentication, state S913");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S292
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S292()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S292");
            this.Manager.Comment("reaching state \'S292\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S293\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S658\'");
            ProtocolMessageStructures.errorstatus temp124;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp124 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S914\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp124, "return of SASLAuthentication, state S914");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S294
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S294()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S294");
            this.Manager.Comment("reaching state \'S294\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S295\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S659\'");
            ProtocolMessageStructures.errorstatus temp125;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,validPassword,LD" +
                    "AP_GC_PORT,True)\'");
            temp125 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R468");
            this.Manager.Comment("reaching state \'S915\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp125, "return of SASLAuthentication, state S915");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S296
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S296()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S296");
            this.Manager.Comment("reaching state \'S296\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S297\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S660\'");
            ProtocolMessageStructures.errorstatus temp126;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,True)\'");
            temp126 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S916\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp126, "return of SASLAuthentication, state S916");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S298
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S298()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S298");
            this.Manager.Comment("reaching state \'S298\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S299\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S661\'");
            ProtocolMessageStructures.errorstatus temp127;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,validPassword," +
                    "LDAP_PORT,True)\'");
            temp127 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S917\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp127, "return of SASLAuthentication, state S917");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S30()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S527\'");
            ProtocolMessageStructures.errorstatus temp128;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,validPassword" +
                    ",LDAP_SSL_GC_PORT,False)\'");
            temp128 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R470");
            this.Manager.Comment("reaching state \'S783\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp128, "return of SASLAuthentication, state S783");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S300
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S300()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S300");
            this.Manager.Comment("reaching state \'S300\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S301\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S662\'");
            ProtocolMessageStructures.errorstatus temp129;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,validPassword" +
                    ",LDAP_PORT,True)\'");
            temp129 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R470");
            this.Manager.Comment("reaching state \'S918\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp129, "return of SASLAuthentication, state S918");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S302
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S302()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S302");
            this.Manager.Comment("reaching state \'S302\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S303\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S663\'");
            ProtocolMessageStructures.errorstatus temp130;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,validPassword,LD" +
                    "AP_PORT,True)\'");
            temp130 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R468");
            this.Manager.Comment("reaching state \'S919\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp130, "return of SASLAuthentication, state S919");
            TCSASLAuth_2K8R2S1032();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S304
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S304()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S304");
            this.Manager.Comment("reaching state \'S304\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S305\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S664\'");
            ProtocolMessageStructures.errorstatus temp131;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,invalidPasswor" +
                    "d,LDAP_GC_PORT,True)\'");
            temp131 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S920\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp131, "return of SASLAuthentication, state S920");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S306
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S306()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S306");
            this.Manager.Comment("reaching state \'S306\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S307\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S665\'");
            ProtocolMessageStructures.errorstatus temp132;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,invalidPassword," +
                    "LDAP_GC_PORT,True)\'");
            temp132 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S921\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp132, "return of SASLAuthentication, state S921");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S308
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S308()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S308");
            this.Manager.Comment("reaching state \'S308\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S309\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S666\'");
            ProtocolMessageStructures.errorstatus temp133;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,invalidPasswo" +
                    "rd,LDAP_GC_PORT,True)\'");
            temp133 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S922\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp133, "return of SASLAuthentication, state S922");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S310
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S310()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S310");
            this.Manager.Comment("reaching state \'S310\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S311\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S667\'");
            ProtocolMessageStructures.errorstatus temp134;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,invalidPassword," +
                    "LDAP_GC_PORT,True)\'");
            temp134 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S923\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp134, "return of SASLAuthentication, state S923");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S312
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S312()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S312");
            this.Manager.Comment("reaching state \'S312\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S313\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S668\'");
            ProtocolMessageStructures.errorstatus temp135;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,invalidPasswor" +
                    "d,LDAP_GC_PORT,True)\'");
            temp135 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S924\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp135, "return of SASLAuthentication, state S924");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S314
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S314()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S314");
            this.Manager.Comment("reaching state \'S314\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S315\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S669\'");
            ProtocolMessageStructures.errorstatus temp136;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,validPasswor" +
                    "d,LDAP_GC_PORT,True)\'");
            temp136 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S925\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp136, "return of SASLAuthentication, state S925");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S316
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S316()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S316");
            this.Manager.Comment("reaching state \'S316\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S317\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S670\'");
            ProtocolMessageStructures.errorstatus temp137;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,validPassword," +
                    "LDAP_GC_PORT,True)\'");
            temp137 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S926\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp137, "return of SASLAuthentication, state S926");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S318
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S318()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S318");
            this.Manager.Comment("reaching state \'S318\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S319\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S671\'");
            ProtocolMessageStructures.errorstatus temp138;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,invalidPass" +
                    "word,LDAP_GC_PORT,True)\'");
            temp138 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S927\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp138, "return of SASLAuthentication, state S927");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S32()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S528\'");
            ProtocolMessageStructures.errorstatus temp139;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,validPassword,LD" +
                    "AP_SSL_GC_PORT,False)\'");
            temp139 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R468");
            this.Manager.Comment("reaching state \'S784\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp139, "return of SASLAuthentication, state S784");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S320
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S320()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S320");
            this.Manager.Comment("reaching state \'S320\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S321\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S672\'");
            ProtocolMessageStructures.errorstatus temp140;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,invalidPassword," +
                    "LDAP_SSL_PORT,False)\'");
            temp140 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S928\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp140, "return of SASLAuthentication, state S928");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S322
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S322()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S322");
            this.Manager.Comment("reaching state \'S322\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S323\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S673\'");
            ProtocolMessageStructures.errorstatus temp141;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,validPasswo" +
                    "rd,LDAP_GC_PORT,True)\'");
            temp141 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S929\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp141, "return of SASLAuthentication, state S929");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S324
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S324()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S324");
            this.Manager.Comment("reaching state \'S324\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S325\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S674\'");
            ProtocolMessageStructures.errorstatus temp142;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,validPassword," +
                    "LDAP_GC_PORT,True)\'");
            temp142 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S930\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp142, "return of SASLAuthentication, state S930");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S326
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S326()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S326");
            this.Manager.Comment("reaching state \'S326\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S327\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S675\'");
            ProtocolMessageStructures.errorstatus temp143;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,validPasswo" +
                    "rd,LDAP_GC_PORT,False)\'");
            temp143 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S931\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp143, "return of SASLAuthentication, state S931");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S328
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S328()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S328");
            this.Manager.Comment("reaching state \'S328\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S329\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S676\'");
            ProtocolMessageStructures.errorstatus temp144;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,validPasswor" +
                    "d,LDAP_GC_PORT,False)\'");
            temp144 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S932\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp144, "return of SASLAuthentication, state S932");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S330
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S330()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S330");
            this.Manager.Comment("reaching state \'S330\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S331\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S677\'");
            ProtocolMessageStructures.errorstatus temp145;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,validPassword," +
                    "LDAP_GC_PORT,False)\'");
            temp145 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S933\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp145, "return of SASLAuthentication, state S933");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S332
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S332()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S332");
            this.Manager.Comment("reaching state \'S332\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S333\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S678\'");
            ProtocolMessageStructures.errorstatus temp146;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,validPassword," +
                    "LDAP_GC_PORT,False)\'");
            temp146 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S934\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp146, "return of SASLAuthentication, state S934");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S334
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S334()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S334");
            this.Manager.Comment("reaching state \'S334\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S335\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S679\'");
            ProtocolMessageStructures.errorstatus temp147;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,invalidPassword," +
                    "LDAP_GC_PORT,False)\'");
            temp147 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S935\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp147, "return of SASLAuthentication, state S935");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S336
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S336()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S336");
            this.Manager.Comment("reaching state \'S336\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S337\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S680\'");
            ProtocolMessageStructures.errorstatus temp148;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,invalidPasswo" +
                    "rd,LDAP_GC_PORT,False)\'");
            temp148 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S936\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp148, "return of SASLAuthentication, state S936");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S338
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S338()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S338");
            this.Manager.Comment("reaching state \'S338\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S339\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S681\'");
            ProtocolMessageStructures.errorstatus temp149;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,invalidPassword," +
                    "LDAP_GC_PORT,False)\'");
            temp149 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S937\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp149, "return of SASLAuthentication, state S937");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S34()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S529\'");
            ProtocolMessageStructures.errorstatus temp150;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_SSL_GC_PORT,False)\'");
            temp150 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S785\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp150, "return of SASLAuthentication, state S785");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S340
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S340()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S340");
            this.Manager.Comment("reaching state \'S340\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S341\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S682\'");
            ProtocolMessageStructures.errorstatus temp151;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,invalidPasswor" +
                    "d,LDAP_GC_PORT,False)\'");
            temp151 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S938\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp151, "return of SASLAuthentication, state S938");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S342
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S342()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S342");
            this.Manager.Comment("reaching state \'S342\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S343\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S683\'");
            ProtocolMessageStructures.errorstatus temp152;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,invalidPass" +
                    "word,LDAP_GC_PORT,False)\'");
            temp152 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S939\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp152, "return of SASLAuthentication, state S939");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S344
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S344()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S344");
            this.Manager.Comment("reaching state \'S344\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S345\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S684\'");
            ProtocolMessageStructures.errorstatus temp153;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,invalidPasswor" +
                    "d,LDAP_PORT,False)\'");
            temp153 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S940\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp153, "return of SASLAuthentication, state S940");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S346
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S346()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S346");
            this.Manager.Comment("reaching state \'S346\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S347\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S685\'");
            ProtocolMessageStructures.errorstatus temp154;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,validPasswor" +
                    "d,LDAP_PORT,False)\'");
            temp154 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S941\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp154, "return of SASLAuthentication, state S941");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S348
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S348()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S348");
            this.Manager.Comment("reaching state \'S348\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S349\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S686\'");
            ProtocolMessageStructures.errorstatus temp155;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,invalidPassw" +
                    "ord,LDAP_PORT,False)\'");
            temp155 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S942\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp155, "return of SASLAuthentication, state S942");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S350
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S350()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S350");
            this.Manager.Comment("reaching state \'S350\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S351\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S687\'");
            ProtocolMessageStructures.errorstatus temp156;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,validPassword," +
                    "LDAP_PORT,False)\'");
            temp156 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S943\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp156, "return of SASLAuthentication, state S943");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S352
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S352()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S352");
            this.Manager.Comment("reaching state \'S352\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S353\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S688\'");
            ProtocolMessageStructures.errorstatus temp157;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,validPasswo" +
                    "rd,LDAP_PORT,False)\'");
            temp157 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S944\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp157, "return of SASLAuthentication, state S944");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S354
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S354()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S354");
            this.Manager.Comment("reaching state \'S354\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S355\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S689\'");
            ProtocolMessageStructures.errorstatus temp158;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,validPassword," +
                    "LDAP_PORT,False)\'");
            temp158 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S945\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp158, "return of SASLAuthentication, state S945");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S356
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S356()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S356");
            this.Manager.Comment("reaching state \'S356\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S357\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S690\'");
            ProtocolMessageStructures.errorstatus temp159;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,invalidPassword," +
                    "LDAP_PORT,False)\'");
            temp159 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S946\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp159, "return of SASLAuthentication, state S946");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S358
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S358()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S358");
            this.Manager.Comment("reaching state \'S358\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S359\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S691\'");
            ProtocolMessageStructures.errorstatus temp160;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,invalidPasswo" +
                    "rd,LDAP_PORT,False)\'");
            temp160 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S947\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp160, "return of SASLAuthentication, state S947");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S36()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S530\'");
            ProtocolMessageStructures.errorstatus temp161;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,validPassword" +
                    ",LDAP_PORT,False)\'");
            temp161 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R470");
            this.Manager.Comment("reaching state \'S786\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp161, "return of SASLAuthentication, state S786");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S360
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S360()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S360");
            this.Manager.Comment("reaching state \'S360\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S361\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S692\'");
            ProtocolMessageStructures.errorstatus temp162;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,invalidPasswor" +
                    "d,LDAP_PORT,False)\'");
            temp162 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S948\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp162, "return of SASLAuthentication, state S948");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S362
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S362()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S362");
            this.Manager.Comment("reaching state \'S362\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S363\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S693\'");
            ProtocolMessageStructures.errorstatus temp163;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,invalidPassword," +
                    "LDAP_PORT,False)\'");
            temp163 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S949\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp163, "return of SASLAuthentication, state S949");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S364
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S364()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S364");
            this.Manager.Comment("reaching state \'S364\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S365\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S694\'");
            ProtocolMessageStructures.errorstatus temp164;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,invalidPasswor" +
                    "d,LDAP_PORT,False)\'");
            temp164 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S950\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp164, "return of SASLAuthentication, state S950");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S366
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S366()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S366");
            this.Manager.Comment("reaching state \'S366\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S367\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S695\'");
            ProtocolMessageStructures.errorstatus temp165;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,invalidPass" +
                    "word,LDAP_PORT,False)\'");
            temp165 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S951\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp165, "return of SASLAuthentication, state S951");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S368
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S368()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S368");
            this.Manager.Comment("reaching state \'S368\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S369\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S696\'");
            ProtocolMessageStructures.errorstatus temp166;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,validPassword," +
                    "LDAP_PORT,True)\'");
            temp166 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S952\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp166, "return of SASLAuthentication, state S952");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S370
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S370()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S370");
            this.Manager.Comment("reaching state \'S370\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S371\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S697\'");
            ProtocolMessageStructures.errorstatus temp167;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,validPasswo" +
                    "rd,LDAP_PORT,True)\'");
            temp167 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S953\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp167, "return of SASLAuthentication, state S953");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S372
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S372()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S372");
            this.Manager.Comment("reaching state \'S372\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S373\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S698\'");
            ProtocolMessageStructures.errorstatus temp168;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,validPassword," +
                    "LDAP_PORT,True)\'");
            temp168 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S954\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp168, "return of SASLAuthentication, state S954");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S374
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S374()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S374");
            this.Manager.Comment("reaching state \'S374\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S375\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S699\'");
            ProtocolMessageStructures.errorstatus temp169;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,validPasswor" +
                    "d,LDAP_PORT,True)\'");
            temp169 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S955\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp169, "return of SASLAuthentication, state S955");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S376
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S376()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S376");
            this.Manager.Comment("reaching state \'S376\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S377\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S700\'");
            ProtocolMessageStructures.errorstatus temp170;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,invalidPassword," +
                    "LDAP_PORT,True)\'");
            temp170 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S956\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp170, "return of SASLAuthentication, state S956");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S378
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S378()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S378");
            this.Manager.Comment("reaching state \'S378\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S379\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S701\'");
            ProtocolMessageStructures.errorstatus temp171;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,invalidPasswor" +
                    "d,LDAP_PORT,True)\'");
            temp171 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S957\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp171, "return of SASLAuthentication, state S957");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S38()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S531\'");
            ProtocolMessageStructures.errorstatus temp172;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp172 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R468");
            this.Manager.Comment("reaching state \'S787\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp172, "return of SASLAuthentication, state S787");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S380
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S380()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S380");
            this.Manager.Comment("reaching state \'S380\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S381\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S702\'");
            ProtocolMessageStructures.errorstatus temp173;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,invalidPasswo" +
                    "rd,LDAP_PORT,True)\'");
            temp173 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S958\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp173, "return of SASLAuthentication, state S958");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S382
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S382()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S382");
            this.Manager.Comment("reaching state \'S382\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S383\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S703\'");
            ProtocolMessageStructures.errorstatus temp174;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,invalidPassword," +
                    "LDAP_PORT,True)\'");
            temp174 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S959\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp174, "return of SASLAuthentication, state S959");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S384
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S384()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S384");
            this.Manager.Comment("reaching state \'S384\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S385\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S704\'");
            ProtocolMessageStructures.errorstatus temp175;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,invalidPasswor" +
                    "d,LDAP_SSL_PORT,False)\'");
            temp175 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S960\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp175, "return of SASLAuthentication, state S960");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S386
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S386()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S386");
            this.Manager.Comment("reaching state \'S386\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S387\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S705\'");
            ProtocolMessageStructures.errorstatus temp176;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,validPasswor" +
                    "d,LDAP_SSL_PORT,False)\'");
            temp176 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S961\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp176, "return of SASLAuthentication, state S961");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S388
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S388()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S388");
            this.Manager.Comment("reaching state \'S388\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S389\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S706\'");
            ProtocolMessageStructures.errorstatus temp177;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,validPassword," +
                    "LDAP_SSL_PORT,False)\'");
            temp177 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S962\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp177, "return of SASLAuthentication, state S962");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S390
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S390()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S390");
            this.Manager.Comment("reaching state \'S390\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S391\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S707\'");
            ProtocolMessageStructures.errorstatus temp178;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,invalidPasswor" +
                    "d,LDAP_SSL_PORT,False)\'");
            temp178 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S963\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp178, "return of SASLAuthentication, state S963");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S392
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S392()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S392");
            this.Manager.Comment("reaching state \'S392\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S393\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S708\'");
            ProtocolMessageStructures.errorstatus temp179;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,invalidPass" +
                    "word,LDAP_SSL_PORT,False)\'");
            temp179 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S964\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp179, "return of SASLAuthentication, state S964");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S394
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S394()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S394");
            this.Manager.Comment("reaching state \'S394\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S395\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S709\'");
            ProtocolMessageStructures.errorstatus temp180;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,invalidPasswor" +
                    "d,LDAP_SSL_PORT,False)\'");
            temp180 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S965\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp180, "return of SASLAuthentication, state S965");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S396
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S396()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S396");
            this.Manager.Comment("reaching state \'S396\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S397\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S710\'");
            ProtocolMessageStructures.errorstatus temp181;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,invalidPassw" +
                    "ord,LDAP_SSL_PORT,False)\'");
            temp181 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S966\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp181, "return of SASLAuthentication, state S966");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S398
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S398()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S398");
            this.Manager.Comment("reaching state \'S398\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S399\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S711\'");
            ProtocolMessageStructures.errorstatus temp182;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,invalidPassword," +
                    "LDAP_SSL_PORT,False)\'");
            temp182 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S967\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp182, "return of SASLAuthentication, state S967");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S4()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S514\'");
            ProtocolMessageStructures.errorstatus temp183;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,validPassword," +
                    "LDAP_GC_PORT,True)\'");
            temp183 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S770\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp183, "return of SASLAuthentication, state S770");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S40
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S40()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S532\'");
            ProtocolMessageStructures.errorstatus temp184;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp184 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S788\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp184, "return of SASLAuthentication, state S788");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S400
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S400()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S400");
            this.Manager.Comment("reaching state \'S400\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S401\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S712\'");
            ProtocolMessageStructures.errorstatus temp185;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,invalidPasswo" +
                    "rd,LDAP_SSL_PORT,False)\'");
            temp185 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S968\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp185, "return of SASLAuthentication, state S968");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S402
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S402()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S402");
            this.Manager.Comment("reaching state \'S402\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S403\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S713\'");
            ProtocolMessageStructures.errorstatus temp186;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,validPassword," +
                    "LDAP_SSL_PORT,False)\'");
            temp186 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S969\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp186, "return of SASLAuthentication, state S969");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S404
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S404()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S404");
            this.Manager.Comment("reaching state \'S404\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S405\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S714\'");
            ProtocolMessageStructures.errorstatus temp187;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,validPasswo" +
                    "rd,LDAP_SSL_PORT,False)\'");
            temp187 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S970\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp187, "return of SASLAuthentication, state S970");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S406
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S406()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S406");
            this.Manager.Comment("reaching state \'S406\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S407\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S715\'");
            ProtocolMessageStructures.errorstatus temp188;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,validPassword," +
                    "LDAP_SSL_GC_PORT,False)\'");
            temp188 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S971\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp188, "return of SASLAuthentication, state S971");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S408
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S408()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S408");
            this.Manager.Comment("reaching state \'S408\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S409\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S716\'");
            ProtocolMessageStructures.errorstatus temp189;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,invalidPasswor" +
                    "d,LDAP_SSL_GC_PORT,False)\'");
            temp189 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S972\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp189, "return of SASLAuthentication, state S972");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S410
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S410()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S410");
            this.Manager.Comment("reaching state \'S410\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S411\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S717\'");
            ProtocolMessageStructures.errorstatus temp190;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,invalidPasswor" +
                    "d,LDAP_GC_PORT,False)\'");
            temp190 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S973\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp190, "return of SASLAuthentication, state S973");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S412
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S412()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S412");
            this.Manager.Comment("reaching state \'S412\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S413\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S718\'");
            ProtocolMessageStructures.errorstatus temp191;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,invalidPassw" +
                    "ord,LDAP_PORT,True)\'");
            temp191 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S974\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp191, "return of SASLAuthentication, state S974");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S414
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S414()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S414");
            this.Manager.Comment("reaching state \'S414\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S415\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S719\'");
            ProtocolMessageStructures.errorstatus temp192;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,invalidPasswor" +
                    "d,LDAP_PORT,True)\'");
            temp192 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S975\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp192, "return of SASLAuthentication, state S975");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S416
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S416()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S416");
            this.Manager.Comment("reaching state \'S416\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S417\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S720\'");
            ProtocolMessageStructures.errorstatus temp193;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,invalidPasswor" +
                    "d,LDAP_PORT,True)\'");
            temp193 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S976\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp193, "return of SASLAuthentication, state S976");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S418
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S418()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S418");
            this.Manager.Comment("reaching state \'S418\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S419\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S721\'");
            ProtocolMessageStructures.errorstatus temp194;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,invalidPass" +
                    "word,LDAP_PORT,True)\'");
            temp194 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S977\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp194, "return of SASLAuthentication, state S977");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S42
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S42()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S533\'");
            ProtocolMessageStructures.errorstatus temp195;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,validPassword,LD" +
                    "AP_GC_PORT,True)\'");
            temp195 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R468");
            this.Manager.Comment("reaching state \'S789\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp195, "return of SASLAuthentication, state S789");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S420
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S420()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S420");
            this.Manager.Comment("reaching state \'S420\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S421\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S722\'");
            ProtocolMessageStructures.errorstatus temp196;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,invalidPassword," +
                    "LDAP_SSL_GC_PORT,False)\'");
            temp196 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S978\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp196, "return of SASLAuthentication, state S978");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S422
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S422()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S422");
            this.Manager.Comment("reaching state \'S422\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S423\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S723\'");
            ProtocolMessageStructures.errorstatus temp197;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,invalidPasswo" +
                    "rd,LDAP_SSL_GC_PORT,False)\'");
            temp197 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S979\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp197, "return of SASLAuthentication, state S979");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S424
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S424()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S424");
            this.Manager.Comment("reaching state \'S424\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S425\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S724\'");
            ProtocolMessageStructures.errorstatus temp198;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,validPasswo" +
                    "rd,LDAP_SSL_GC_PORT,False)\'");
            temp198 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S980\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp198, "return of SASLAuthentication, state S980");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S426
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S426()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S426");
            this.Manager.Comment("reaching state \'S426\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S427\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S725\'");
            ProtocolMessageStructures.errorstatus temp199;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,validPassword," +
                    "LDAP_SSL_GC_PORT,False)\'");
            temp199 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S981\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp199, "return of SASLAuthentication, state S981");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S428
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S428()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S428");
            this.Manager.Comment("reaching state \'S428\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S429\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S726\'");
            ProtocolMessageStructures.errorstatus temp200;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,invalidPasswor" +
                    "d,LDAP_SSL_GC_PORT,False)\'");
            temp200 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S982\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp200, "return of SASLAuthentication, state S982");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S430
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S430()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S430");
            this.Manager.Comment("reaching state \'S430\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S431\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S727\'");
            ProtocolMessageStructures.errorstatus temp201;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,validPasswor" +
                    "d,LDAP_SSL_GC_PORT,False)\'");
            temp201 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S983\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp201, "return of SASLAuthentication, state S983");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S432
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S432()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S432");
            this.Manager.Comment("reaching state \'S432\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S433\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S728\'");
            ProtocolMessageStructures.errorstatus temp202;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,invalidPasswor" +
                    "d,LDAP_SSL_GC_PORT,False)\'");
            temp202 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S984\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp202, "return of SASLAuthentication, state S984");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S434
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S434()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S434");
            this.Manager.Comment("reaching state \'S434\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S435\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S729\'");
            ProtocolMessageStructures.errorstatus temp203;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,invalidPass" +
                    "word,LDAP_SSL_GC_PORT,False)\'");
            temp203 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S985\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp203, "return of SASLAuthentication, state S985");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S436
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S436()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S436");
            this.Manager.Comment("reaching state \'S436\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S437\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S730\'");
            ProtocolMessageStructures.errorstatus temp204;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,invalidPassw" +
                    "ord,LDAP_SSL_GC_PORT,False)\'");
            temp204 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S986\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp204, "return of SASLAuthentication, state S986");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S438
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S438()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S438");
            this.Manager.Comment("reaching state \'S438\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S439\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S731\'");
            ProtocolMessageStructures.errorstatus temp205;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,invalidPasswor" +
                    "d,LDAP_GC_PORT,False)\'");
            temp205 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S987\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp205, "return of SASLAuthentication, state S987");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S44
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S44()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S44");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S534\'");
            ProtocolMessageStructures.errorstatus temp206;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,True)\'");
            temp206 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S790\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp206, "return of SASLAuthentication, state S790");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S440
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S440()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S440");
            this.Manager.Comment("reaching state \'S440\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S441\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S732\'");
            ProtocolMessageStructures.errorstatus temp207;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,invalidPassw" +
                    "ord,LDAP_GC_PORT,False)\'");
            temp207 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S988\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp207, "return of SASLAuthentication, state S988");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S442
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S442()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S442");
            this.Manager.Comment("reaching state \'S442\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S443\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S733\'");
            ProtocolMessageStructures.errorstatus temp208;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,invalidPassword," +
                    "LDAP_SSL_GC_PORT,False)\'");
            temp208 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S989\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp208, "return of SASLAuthentication, state S989");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S444
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S444()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S444");
            this.Manager.Comment("reaching state \'S444\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S445\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S734\'");
            ProtocolMessageStructures.errorstatus temp209;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,invalidPasswor" +
                    "d,LDAP_GC_PORT,True)\'");
            temp209 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S990\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp209, "return of SASLAuthentication, state S990");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S446
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S446()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S446");
            this.Manager.Comment("reaching state \'S446\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S447\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S735\'");
            ProtocolMessageStructures.errorstatus temp210;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,invalidPassw" +
                    "ord,LDAP_GC_PORT,True)\'");
            temp210 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S991\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp210, "return of SASLAuthentication, state S991");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S448
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S448()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S448");
            this.Manager.Comment("reaching state \'S448\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S449\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S736\'");
            ProtocolMessageStructures.errorstatus temp211;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,invalidPasswor" +
                    "d,LDAP_SSL_PORT,True)\'");
            temp211 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S992\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp211, "return of SASLAuthentication, state S992");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S450
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S450()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S450");
            this.Manager.Comment("reaching state \'S450\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S451\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S737\'");
            ProtocolMessageStructures.errorstatus temp212;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_SSL_PORT,True)\'");
            temp212 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S993\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp212, "return of SASLAuthentication, state S993");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S452
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S452()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S452");
            this.Manager.Comment("reaching state \'S452\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S453\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S738\'");
            ProtocolMessageStructures.errorstatus temp213;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,invalidPassword," +
                    "LDAP_SSL_GC_PORT,True)\'");
            temp213 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S994\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp213, "return of SASLAuthentication, state S994");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S454
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S454()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S454");
            this.Manager.Comment("reaching state \'S454\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S455\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S739\'");
            ProtocolMessageStructures.errorstatus temp214;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_SSL_GC_PORT,True)\'");
            temp214 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S995\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp214, "return of SASLAuthentication, state S995");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S456
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S456()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S456");
            this.Manager.Comment("reaching state \'S456\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S457\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S740\'");
            ProtocolMessageStructures.errorstatus temp215;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,validPassword," +
                    "LDAP_SSL_GC_PORT,True)\'");
            temp215 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S996\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp215, "return of SASLAuthentication, state S996");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S458
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S458()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S458");
            this.Manager.Comment("reaching state \'S458\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S459\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S741\'");
            ProtocolMessageStructures.errorstatus temp216;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,invalidPasswor" +
                    "d,LDAP_SSL_GC_PORT,True)\'");
            temp216 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S997\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp216, "return of SASLAuthentication, state S997");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S46
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S46()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S46");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S535\'");
            ProtocolMessageStructures.errorstatus temp217;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,validPassword," +
                    "LDAP_PORT,True)\'");
            temp217 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S791\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp217, "return of SASLAuthentication, state S791");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S460
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S460()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S460");
            this.Manager.Comment("reaching state \'S460\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S461\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S742\'");
            ProtocolMessageStructures.errorstatus temp218;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,validPassword," +
                    "LDAP_SSL_PORT,True)\'");
            temp218 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S998\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp218, "return of SASLAuthentication, state S998");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S462
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S462()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S462");
            this.Manager.Comment("reaching state \'S462\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S463\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S743\'");
            ProtocolMessageStructures.errorstatus temp219;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,invalidPasswor" +
                    "d,LDAP_SSL_PORT,True)\'");
            temp219 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S999\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp219, "return of SASLAuthentication, state S999");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S464
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S464()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S464");
            this.Manager.Comment("reaching state \'S464\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S465\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S744\'");
            ProtocolMessageStructures.errorstatus temp220;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,invalidPassword," +
                    "LDAP_SSL_PORT,True)\'");
            temp220 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S1000\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp220, "return of SASLAuthentication, state S1000");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S466
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S466()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S466");
            this.Manager.Comment("reaching state \'S466\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S467\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S745\'");
            ProtocolMessageStructures.errorstatus temp221;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,validPassword" +
                    ",LDAP_SSL_PORT,True)\'");
            temp221 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S1001\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp221, "return of SASLAuthentication, state S1001");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S468
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S468()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S468");
            this.Manager.Comment("reaching state \'S468\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S469\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S746\'");
            ProtocolMessageStructures.errorstatus temp222;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,validPassword," +
                    "LDAP_SSL_PORT,True)\'");
            temp222 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S1002\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp222, "return of SASLAuthentication, state S1002");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S470
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S470()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S470");
            this.Manager.Comment("reaching state \'S470\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S471\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S747\'");
            ProtocolMessageStructures.errorstatus temp223;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,validPassword,LD" +
                    "AP_SSL_PORT,True)\'");
            temp223 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S1003\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp223, "return of SASLAuthentication, state S1003");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S472
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S472()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S472");
            this.Manager.Comment("reaching state \'S472\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S473\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S748\'");
            ProtocolMessageStructures.errorstatus temp224;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,validPassword," +
                    "LDAP_SSL_PORT,True)\'");
            temp224 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S1004\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp224, "return of SASLAuthentication, state S1004");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S474
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S474()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S474");
            this.Manager.Comment("reaching state \'S474\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S475\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S749\'");
            ProtocolMessageStructures.errorstatus temp225;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,validPasswo" +
                    "rd,LDAP_SSL_PORT,True)\'");
            temp225 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S1005\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp225, "return of SASLAuthentication, state S1005");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S476
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S476()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S476");
            this.Manager.Comment("reaching state \'S476\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S477\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S750\'");
            ProtocolMessageStructures.errorstatus temp226;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,validPasswor" +
                    "d,LDAP_SSL_PORT,True)\'");
            temp226 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S1006\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp226, "return of SASLAuthentication, state S1006");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S478
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S478()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S478");
            this.Manager.Comment("reaching state \'S478\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S479\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S751\'");
            ProtocolMessageStructures.errorstatus temp227;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,invalidPassw" +
                    "ord,LDAP_SSL_PORT,True)\'");
            temp227 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S1007\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp227, "return of SASLAuthentication, state S1007");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S48
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S48()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S48");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S536\'");
            ProtocolMessageStructures.errorstatus temp228;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,validPassword" +
                    ",LDAP_PORT,True)\'");
            temp228 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R470");
            this.Manager.Comment("reaching state \'S792\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp228, "return of SASLAuthentication, state S792");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S480
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S480()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S480");
            this.Manager.Comment("reaching state \'S480\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S481\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S752\'");
            ProtocolMessageStructures.errorstatus temp229;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,invalidPasswo" +
                    "rd,LDAP_SSL_PORT,True)\'");
            temp229 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S1008\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp229, "return of SASLAuthentication, state S1008");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S482
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S482()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S482");
            this.Manager.Comment("reaching state \'S482\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S483\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S753\'");
            ProtocolMessageStructures.errorstatus temp230;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,invalidPassword," +
                    "LDAP_SSL_PORT,True)\'");
            temp230 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S1009\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp230, "return of SASLAuthentication, state S1009");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S484
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S484()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S484");
            this.Manager.Comment("reaching state \'S484\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S485\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S754\'");
            ProtocolMessageStructures.errorstatus temp231;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,invalidPasswor" +
                    "d,LDAP_SSL_PORT,True)\'");
            temp231 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S1010\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp231, "return of SASLAuthentication, state S1010");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S486
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S486()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S486");
            this.Manager.Comment("reaching state \'S486\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S487\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S755\'");
            ProtocolMessageStructures.errorstatus temp232;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,invalidPass" +
                    "word,LDAP_SSL_PORT,True)\'");
            temp232 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S1011\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp232, "return of SASLAuthentication, state S1011");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S488
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S488()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S488");
            this.Manager.Comment("reaching state \'S488\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S489\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S756\'");
            ProtocolMessageStructures.errorstatus temp233;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,validPasswo" +
                    "rd,LDAP_SSL_GC_PORT,True)\'");
            temp233 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S1012\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp233, "return of SASLAuthentication, state S1012");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S490
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S490()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S490");
            this.Manager.Comment("reaching state \'S490\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S491\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S757\'");
            ProtocolMessageStructures.errorstatus temp234;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,validPassword," +
                    "LDAP_SSL_GC_PORT,True)\'");
            temp234 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S1013\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp234, "return of SASLAuthentication, state S1013");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S492
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S492()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S492");
            this.Manager.Comment("reaching state \'S492\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S493\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S758\'");
            ProtocolMessageStructures.errorstatus temp235;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,validPasswor" +
                    "d,LDAP_SSL_GC_PORT,True)\'");
            temp235 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S1014\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp235, "return of SASLAuthentication, state S1014");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S494
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S494()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S494");
            this.Manager.Comment("reaching state \'S494\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S495\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S759\'");
            ProtocolMessageStructures.errorstatus temp236;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,validPassword,LD" +
                    "AP_SSL_GC_PORT,True)\'");
            temp236 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S1015\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp236, "return of SASLAuthentication, state S1015");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S496
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S496()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S496");
            this.Manager.Comment("reaching state \'S496\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S497\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S760\'");
            ProtocolMessageStructures.errorstatus temp237;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,validPassword" +
                    ",LDAP_SSL_GC_PORT,True)\'");
            temp237 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S1016\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp237, "return of SASLAuthentication, state S1016");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S498
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S498()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S498");
            this.Manager.Comment("reaching state \'S498\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S499\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S761\'");
            ProtocolMessageStructures.errorstatus temp238;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,validPassword," +
                    "LDAP_SSL_GC_PORT,True)\'");
            temp238 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S1017\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp238, "return of SASLAuthentication, state S1017");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S50
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S50()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S50");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S537\'");
            ProtocolMessageStructures.errorstatus temp239;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,validPassword,LD" +
                    "AP_PORT,True)\'");
            temp239 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R468");
            this.Manager.Comment("reaching state \'S793\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp239, "return of SASLAuthentication, state S793");
            TCSASLAuth_2K8R2S1026();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S500
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S500()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S500");
            this.Manager.Comment("reaching state \'S500\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S501\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S762\'");
            ProtocolMessageStructures.errorstatus temp240;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,invalidPasswor" +
                    "d,LDAP_SSL_GC_PORT,True)\'");
            temp240 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S1018\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp240, "return of SASLAuthentication, state S1018");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S502
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S502()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S502");
            this.Manager.Comment("reaching state \'S502\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S503\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S763\'");
            ProtocolMessageStructures.errorstatus temp241;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,invalidPassword," +
                    "LDAP_SSL_GC_PORT,True)\'");
            temp241 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S1019\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp241, "return of SASLAuthentication, state S1019");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S504
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S504()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S504");
            this.Manager.Comment("reaching state \'S504\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S505\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S764\'");
            ProtocolMessageStructures.errorstatus temp242;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,invalidPasswo" +
                    "rd,LDAP_SSL_GC_PORT,True)\'");
            temp242 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S1020\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp242, "return of SASLAuthentication, state S1020");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S506
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S506()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S506");
            this.Manager.Comment("reaching state \'S506\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S507\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S765\'");
            ProtocolMessageStructures.errorstatus temp243;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,invalidPasswor" +
                    "d,LDAP_SSL_GC_PORT,True)\'");
            temp243 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S1021\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp243, "return of SASLAuthentication, state S1021");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S508
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S508()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S508");
            this.Manager.Comment("reaching state \'S508\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S509\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S766\'");
            ProtocolMessageStructures.errorstatus temp244;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,invalidPass" +
                    "word,LDAP_SSL_GC_PORT,True)\'");
            temp244 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S1022\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp244, "return of SASLAuthentication, state S1022");
            TCSASLAuth_2K8R2S1024();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S510
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S510()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S510");
            this.Manager.Comment("reaching state \'S510\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S511\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S767\'");
            ProtocolMessageStructures.errorstatus temp245;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,invalidPassw" +
                    "ord,LDAP_SSL_GC_PORT,True)\'");
            temp245 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S1023\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp245, "return of SASLAuthentication, state S1023");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S52
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S52()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S52");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S538\'");
            ProtocolMessageStructures.errorstatus temp246;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,invalidPassword," +
                    "LDAP_GC_PORT,True)\'");
            temp246 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S794\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp246, "return of SASLAuthentication, state S794");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S54
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S54()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S54");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S539\'");
            ProtocolMessageStructures.errorstatus temp247;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,invalidPasswo" +
                    "rd,LDAP_GC_PORT,True)\'");
            temp247 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S795\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp247, "return of SASLAuthentication, state S795");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S56
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S56()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S56");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S540\'");
            ProtocolMessageStructures.errorstatus temp248;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,invalidPassword," +
                    "LDAP_GC_PORT,True)\'");
            temp248 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S796\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp248, "return of SASLAuthentication, state S796");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S58
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S58()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S58");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S541\'");
            ProtocolMessageStructures.errorstatus temp249;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,invalidPasswor" +
                    "d,LDAP_GC_PORT,True)\'");
            temp249 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S797\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp249, "return of SASLAuthentication, state S797");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S6()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S515\'");
            ProtocolMessageStructures.errorstatus temp250;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_GC_PORT,True)\'");
            temp250 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S771\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp250, "return of SASLAuthentication, state S771");
            this.Manager.Comment("reaching state \'S1027\'");
            ProtocolMessageStructures.errorstatus temp251;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,NotSet,use" +
                    "rPassword,False)\'");
            temp251 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1039\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp251, "return of AuthorizationCheck, state S1039");
            this.Manager.Comment("reaching state \'S1051\'");
            ProtocolMessageStructures.errorstatus temp252;
            this.Manager.Comment("executing step \'call AdminPasswordReset(\"PasswordNew1\",False)\'");
            temp252 = this.IMS_ADTS_AuthenticationAuthInstance.AdminPasswordReset("PasswordNew1", false);
            this.Manager.Checkpoint("MS-ADTS-Security_R37");
            this.Manager.Comment("reaching state \'S1061\'");
            this.Manager.Comment("checking step \'return AdminPasswordReset/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp252, "return of AdminPasswordReset, state S1061");
            TCSASLAuth_2K8R2S1069();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S60
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S60()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S60");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S542\'");
            ProtocolMessageStructures.errorstatus temp253;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,validPasswor" +
                    "d,LDAP_GC_PORT,True)\'");
            temp253 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S798\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp253, "return of SASLAuthentication, state S798");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S62
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S62()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S62");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S543\'");
            ProtocolMessageStructures.errorstatus temp254;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,validPassword," +
                    "LDAP_GC_PORT,True)\'");
            temp254 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S799\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp254, "return of SASLAuthentication, state S799");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S64
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S64()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S64");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S544\'");
            ProtocolMessageStructures.errorstatus temp255;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,invalidPass" +
                    "word,LDAP_GC_PORT,True)\'");
            temp255 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S800\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp255, "return of SASLAuthentication, state S800");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S66
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S66()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S66");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S545\'");
            ProtocolMessageStructures.errorstatus temp256;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,invalidPassword," +
                    "LDAP_SSL_PORT,False)\'");
            temp256 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S801\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp256, "return of SASLAuthentication, state S801");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S68
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S68()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S68");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S546\'");
            ProtocolMessageStructures.errorstatus temp257;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,validPasswo" +
                    "rd,LDAP_GC_PORT,True)\'");
            temp257 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S802\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp257, "return of SASLAuthentication, state S802");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S70
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S70()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S70");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S547\'");
            ProtocolMessageStructures.errorstatus temp258;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,validPassword," +
                    "LDAP_GC_PORT,True)\'");
            temp258 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S803\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp258, "return of SASLAuthentication, state S803");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S72
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S72()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S72");
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S548\'");
            ProtocolMessageStructures.errorstatus temp259;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,validPasswo" +
                    "rd,LDAP_GC_PORT,False)\'");
            temp259 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S804\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp259, "return of SASLAuthentication, state S804");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S74
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S74()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S74");
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S549\'");
            ProtocolMessageStructures.errorstatus temp260;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,validPasswor" +
                    "d,LDAP_GC_PORT,False)\'");
            temp260 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S805\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp260, "return of SASLAuthentication, state S805");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S76
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S76()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S76");
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S550\'");
            ProtocolMessageStructures.errorstatus temp261;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,validPassword," +
                    "LDAP_GC_PORT,False)\'");
            temp261 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S806\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp261, "return of SASLAuthentication, state S806");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S78
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S78()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S78");
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S551\'");
            ProtocolMessageStructures.errorstatus temp262;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslspnego,validPassword," +
                    "LDAP_GC_PORT,False)\'");
            temp262 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S807\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp262, "return of SASLAuthentication, state S807");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S8()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S516\'");
            ProtocolMessageStructures.errorstatus temp263;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,validPassword" +
                    ",LDAP_GC_PORT,True)\'");
            temp263 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R470");
            this.Manager.Comment("reaching state \'S772\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp263, "return of SASLAuthentication, state S772");
            this.Manager.Comment("reaching state \'S1028\'");
            ProtocolMessageStructures.errorstatus temp264;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Force" +
                    "_Change_Password,userPassword,False)\'");
            temp264 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ProtocolMessageStructures.ControlAccessRights.User_Force_Change_Password, ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1040\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp264, "return of AuthorizationCheck, state S1040");
            this.Manager.Comment("reaching state \'S1052\'");
            ProtocolMessageStructures.errorstatus temp265;
            this.Manager.Comment("executing step \'call AdminPasswordReset(\"PasswordNew1\",False)\'");
            temp265 = this.IMS_ADTS_AuthenticationAuthInstance.AdminPasswordReset("PasswordNew1", false);
            this.Manager.Checkpoint("MS-ADTS-Security_R32");
            this.Manager.Checkpoint("MS-ADTS-Security_R36");
            this.Manager.Comment("reaching state \'S1062\'");
            this.Manager.Comment("checking step \'return AdminPasswordReset/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp265, "return of AdminPasswordReset, state S1062");
            TCSASLAuth_2K8R2S1068();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S80
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S80()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S80");
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S552\'");
            ProtocolMessageStructures.errorstatus temp266;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslspnego,invalidPassword," +
                    "LDAP_GC_PORT,False)\'");
            temp266 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S808\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp266, "return of SASLAuthentication, state S808");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S82
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S82()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S82");
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S553\'");
            ProtocolMessageStructures.errorstatus temp267;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,sasldigestMD5,invalidPasswo" +
                    "rd,LDAP_GC_PORT,False)\'");
            temp267 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S809\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp267, "return of SASLAuthentication, state S809");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S84
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S84()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S84");
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S554\'");
            ProtocolMessageStructures.errorstatus temp268;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,invalidPassword," +
                    "LDAP_GC_PORT,False)\'");
            temp268 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S810\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp268, "return of SASLAuthentication, state S810");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S86
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S86()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S86");
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S555\'");
            ProtocolMessageStructures.errorstatus temp269;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslexternal,invalidPasswor" +
                    "d,LDAP_GC_PORT,False)\'");
            temp269 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S811\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp269, "return of SASLAuthentication, state S811");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S88
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S88()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S88");
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S556\'");
            ProtocolMessageStructures.errorstatus temp270;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,invalidPass" +
                    "word,LDAP_GC_PORT,False)\'");
            temp270 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S812\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp270, "return of SASLAuthentication, state S812");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S90
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S90()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S90");
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S557\'");
            ProtocolMessageStructures.errorstatus temp271;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,invalidPasswor" +
                    "d,LDAP_PORT,False)\'");
            temp271 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S813\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp271, "return of SASLAuthentication, state S813");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S92
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S92()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S92");
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S558\'");
            ProtocolMessageStructures.errorstatus temp272;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,validPasswor" +
                    "d,LDAP_PORT,False)\'");
            temp272 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S814\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp272, "return of SASLAuthentication, state S814");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S94
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S94()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S94");
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S559\'");
            ProtocolMessageStructures.errorstatus temp273;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslexternal,invalidPassw" +
                    "ord,LDAP_PORT,False)\'");
            temp273 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.saslexternal, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S815\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp273, "return of SASLAuthentication, state S815");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S96
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S96()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S96");
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S560\'");
            ProtocolMessageStructures.errorstatus temp274;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,saslgssapi,validPassword," +
                    "LDAP_PORT,False)\'");
            temp274 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S816\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp274, "return of SASLAuthentication, state S816");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S98
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSASLAuth_2K8R2S98()
        {
            this.Manager.BeginTest("TCSASLAuth_2K8R2S98");
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S561\'");
            ProtocolMessageStructures.errorstatus temp275;
            this.Manager.Comment("executing step \'call SASLAuthentication(invalidUserName,sasldigestMD5,validPasswo" +
                    "rd,LDAP_PORT,False)\'");
            temp275 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(0)), ProtocolMessageStructures.SASLChoice.sasldigestMD5, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S817\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp275, "return of SASLAuthentication, state S817");
            TCSASLAuth_2K8R2S1025();
            this.Manager.EndTest();
        }
        #endregion
    }
}
