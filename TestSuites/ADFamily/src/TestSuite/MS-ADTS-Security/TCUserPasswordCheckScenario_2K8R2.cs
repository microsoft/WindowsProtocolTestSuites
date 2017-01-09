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
    public partial class TCUserPasswordCheckScenario_2K8R2 : PtfTestClassBase
    {

        public TCUserPasswordCheckScenario_2K8R2()
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
        public void Security_TCUserPasswordCheckScenario_2K8R2S0()
        {
            this.Manager.BeginTest("TCUserPasswordCheckScenario_2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S16\'");
            ProtocolMessageStructures.errorstatus temp0;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp0 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp0, "return of SASLAuthentication, state S24");
            this.Manager.Comment("reaching state \'S32\'");
            ProtocolMessageStructures.errorstatus temp1;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,NotSet,use" +
                    "rPassword,True)\'");
            temp1 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, true);
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp1, "return of AuthorizationCheck, state S40");
            TCUserPasswordCheckScenario_2K8R2S48();
            this.Manager.EndTest();
        }

        private void TCUserPasswordCheckScenario_2K8R2S48()
        {
            this.Manager.Comment("reaching state \'S48\'");
        }
        #endregion

        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCUserPasswordCheckScenario_2K8R2S10()
        {
            this.Manager.BeginTest("TCUserPasswordCheckScenario_2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S21\'");
            ProtocolMessageStructures.errorstatus temp2;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp2 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp2, "return of SASLAuthentication, state S29");
            this.Manager.Comment("reaching state \'S37\'");
            ProtocolMessageStructures.errorstatus temp3;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,userPassword,False)\'");
            temp3 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp3, "return of AuthorizationCheck, state S45");
            TCUserPasswordCheckScenario_2K8R2S48();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCUserPasswordCheckScenario_2K8R2S12()
        {
            this.Manager.BeginTest("TCUserPasswordCheckScenario_2K8R2S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S22\'");
            ProtocolMessageStructures.errorstatus temp4;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp4 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp4, "return of SASLAuthentication, state S30");
            this.Manager.Comment("reaching state \'S38\'");
            ProtocolMessageStructures.errorstatus temp5;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,userPassword,True)\'");
            temp5 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, true);
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp5, "return of AuthorizationCheck, state S46");
            TCUserPasswordCheckScenario_2K8R2S48();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCUserPasswordCheckScenario_2K8R2S14()
        {
            this.Manager.BeginTest("TCUserPasswordCheckScenario_2K8R2S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S23\'");
            ProtocolMessageStructures.errorstatus temp6;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp6 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp6, "return of SASLAuthentication, state S31");
            this.Manager.Comment("reaching state \'S39\'");
            ProtocolMessageStructures.errorstatus temp7;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,NotSet,use" +
                    "rPassword,True)\'");
            temp7 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, true);
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp7, "return of AuthorizationCheck, state S47");
            TCUserPasswordCheckScenario_2K8R2S49();
            this.Manager.EndTest();
        }

        private void TCUserPasswordCheckScenario_2K8R2S49()
        {
            this.Manager.Comment("reaching state \'S49\'");
        }
        #endregion

        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCUserPasswordCheckScenario_2K8R2S2()
        {
            this.Manager.BeginTest("TCUserPasswordCheckScenario_2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S17\'");
            ProtocolMessageStructures.errorstatus temp8;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp8 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp8, "return of SASLAuthentication, state S25");
            this.Manager.Comment("reaching state \'S33\'");
            ProtocolMessageStructures.errorstatus temp9;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,userPassword,False)\'");
            temp9 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp9, "return of AuthorizationCheck, state S41");
            TCUserPasswordCheckScenario_2K8R2S49();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCUserPasswordCheckScenario_2K8R2S4()
        {
            this.Manager.BeginTest("TCUserPasswordCheckScenario_2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S18\'");
            ProtocolMessageStructures.errorstatus temp10;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp10 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp10, "return of SASLAuthentication, state S26");
            this.Manager.Comment("reaching state \'S34\'");
            ProtocolMessageStructures.errorstatus temp11;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,NotSet,use" +
                    "rPassword,False)\'");
            temp11 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp11, "return of AuthorizationCheck, state S42");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCUserPasswordCheckScenario_2K8R2S6()
        {
            this.Manager.BeginTest("TCUserPasswordCheckScenario_2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S19\'");
            ProtocolMessageStructures.errorstatus temp12;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp12 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp12, "return of SASLAuthentication, state S27");
            this.Manager.Comment("reaching state \'S35\'");
            ProtocolMessageStructures.errorstatus temp13;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,userPassword,True)\'");
            temp13 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, true);
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp13, "return of AuthorizationCheck, state S43");
            TCUserPasswordCheckScenario_2K8R2S49();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCUserPasswordCheckScenario_2K8R2S8()
        {
            this.Manager.BeginTest("TCUserPasswordCheckScenario_2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S20\'");
            ProtocolMessageStructures.errorstatus temp14;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp14 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp14, "return of SASLAuthentication, state S28");
            this.Manager.Comment("reaching state \'S36\'");
            ProtocolMessageStructures.errorstatus temp15;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,NotSet,use" +
                    "rPassword,False)\'");
            temp15 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp15, "return of AuthorizationCheck, state S44");
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.EndTest();
        }
        #endregion
    }
}
