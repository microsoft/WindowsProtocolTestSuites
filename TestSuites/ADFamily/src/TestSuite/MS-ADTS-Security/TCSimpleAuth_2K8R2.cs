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
    public partial class TCSimpleAuth_2K8R2 : PtfTestClassBase
    {

        public TCSimpleAuth_2K8R2()
        {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "100000");
        }

        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security.IMS_ADTS_AuthenticationAuth IMS_ADTS_AuthenticationAuthInstance;
        private static ADCommonServerAdapter common;
        #endregion

        #region Class Initialization and Cleanup
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void ClassInitialize(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext context)
        {
            PtfTestClassBase.Initialize(context);
            common = new ADCommonServerAdapter();
            common.Initialize(BaseTestSite);
            Utilities.BackupOrRestoreNtSecurityDescriptor(
                common.PDCNetbiosName + "." + common.PrimaryDomainDnsName,
                int.Parse(common.ADDSPortNum),
                "CN=NTDS Quotas,CN=Configuration," + Utilities.DomainDnsNameToDN(common.PrimaryDomainDnsName),
                Utilities.SecurityDescriptorBackupFilename,
                new System.Net.NetworkCredential(common.DomainAdministratorName, common.DomainUserPassword, common.PrimaryDomainDnsName));
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void ClassCleanup()
        {
            PtfTestClassBase.Cleanup();
            Utilities.BackupOrRestoreNtSecurityDescriptor(
                common.PDCNetbiosName + "." + common.PrimaryDomainDnsName,
                int.Parse(common.ADDSPortNum),
                "CN=NTDS Quotas,CN=Configuration," + Utilities.DomainDnsNameToDN(common.PrimaryDomainDnsName),
                Utilities.SecurityDescriptorBackupFilename,
                new System.Net.NetworkCredential(common.DomainAdministratorName, common.DomainUserPassword, common.PrimaryDomainDnsName));
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
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S0()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S444\'");
            ProtocolMessageStructures.errorstatus temp0;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,True" +
                    ")\'");
            temp0 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S666\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp0, "return of SimpleBind, state S666");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S888()
        {
            this.Manager.Comment("reaching state \'S888\'");
        }
        #endregion

        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S10()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S449\'");
            ProtocolMessageStructures.errorstatus temp1;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp1 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S671\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp1, "return of SimpleBind, state S671");
            this.Manager.Comment("reaching state \'S893\'");
            ProtocolMessageStructures.errorstatus temp2;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp2 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1006\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp2, "return of AuthorizationCheck, state S1006");
            this.Manager.Comment("reaching state \'S1112\'");
            ProtocolMessageStructures.errorstatus temp3;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",False)\'");
            temp3 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", false);
            this.Manager.Checkpoint("MS-ADTS-Security_R35");
            this.Manager.Checkpoint("MS-ADTS-Security_R486");
            this.Manager.Comment("reaching state \'S1164\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp3, "return of PasswordChangeOperation, state S1164");
            TCSimpleAuth_2K8R2S1208();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1208()
        {
            this.Manager.Comment("reaching state \'S1208\'");
        }
        #endregion

        #region Test Starting in S100
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S100()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S100");
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S494\'");
            ProtocolMessageStructures.errorstatus temp4;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp4 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S716\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp4, "return of SimpleBind, state S716");
            this.Manager.Comment("reaching state \'S938\'");
            ProtocolMessageStructures.errorstatus temp5;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,NotSet,msDS_QuotaU" +
                    "sed,False)\'");
            temp5 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1051\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp5, "return of AuthorizationCheck, state S1051");
            TCSimpleAuth_2K8R2S1136();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1136()
        {
            this.Manager.Comment("reaching state \'S1136\'");
        }
        #endregion

        #region Test Starting in S102
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S102()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S102");
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S495\'");
            ProtocolMessageStructures.errorstatus temp6;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp6 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S717\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp6, "return of SimpleBind, state S717");
            this.Manager.Comment("reaching state \'S939\'");
            ProtocolMessageStructures.errorstatus temp7;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,NotSet,use" +
                    "rPassword,False)\'");
            temp7 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1052\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp7, "return of AuthorizationCheck, state S1052");
            this.Manager.Comment("reaching state \'S1137\'");
            ProtocolMessageStructures.errorstatus temp8;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",False)" +
                    "\'");
            temp8 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1185\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp8, "return of PasswordChangeOperation, state S1185");
            TCSimpleAuth_2K8R2S1213();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1213()
        {
            this.Manager.Comment("reaching state \'S1213\'");
        }
        #endregion

        #region Test Starting in S104
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S104()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S104");
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S496\'");
            ProtocolMessageStructures.errorstatus temp9;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp9 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S718\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp9, "return of SimpleBind, state S718");
            this.Manager.Comment("reaching state \'S940\'");
            ProtocolMessageStructures.errorstatus temp10;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,DS_Query_Self_Quot" +
                    "a,msDS_QuotaUsed,False)\'");
            temp10 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R45");
            this.Manager.Comment("reaching state \'S1053\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp10, "return of AuthorizationCheck, state S1053");
            this.Manager.Comment("reaching state \'S1138\'");
            ProtocolMessageStructures.errorstatus temp11;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",False)\'");
            temp11 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1186\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp11, "return of PasswordChangeOperation, state S1186");
            TCSimpleAuth_2K8R2S1213();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S106
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S106()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S106");
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S497\'");
            ProtocolMessageStructures.errorstatus temp12;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp12 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S719\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp12, "return of SimpleBind, state S719");
            this.Manager.Comment("reaching state \'S941\'");
            ProtocolMessageStructures.errorstatus temp13;
            this.Manager.Comment("executing step \'call AuthorizationCheck(ACC_SYS_SEC_READ_CONTROL,NotSet,nTSecurit" +
                    "yDescriptor,False)\'");
            temp13 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.ACC_SYS_SEC_READ_CONTROL, ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R39");
            this.Manager.Comment("reaching state \'S1054\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp13, "return of AuthorizationCheck, state S1054");
            this.Manager.Comment("reaching state \'S1139\'");
            ProtocolMessageStructures.errorstatus temp14;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",True)\'" +
                    "");
            temp14 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1187\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp14, "return of PasswordChangeOperation, state S1187");
            TCSimpleAuth_2K8R2S1213();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S108
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S108()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S108");
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S498\'");
            ProtocolMessageStructures.errorstatus temp15;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp15 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S720\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp15, "return of SimpleBind, state S720");
            this.Manager.Comment("reaching state \'S942\'");
            ProtocolMessageStructures.errorstatus temp16;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,DS_Query_Self_Quot" +
                    "a,msDS_QuotaEffective,False)\'");
            temp16 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R41");
            this.Manager.Comment("reaching state \'S1055\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp16, "return of AuthorizationCheck, state S1055");
            this.Manager.Comment("reaching state \'S1140\'");
            ProtocolMessageStructures.errorstatus temp17;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",True)\'");
            temp17 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1188\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp17, "return of PasswordChangeOperation, state S1188");
            TCSimpleAuth_2K8R2S1213();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S110
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S110()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S110");
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S499\'");
            ProtocolMessageStructures.errorstatus temp18;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp18 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S721\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp18, "return of SimpleBind, state S721");
            this.Manager.Comment("reaching state \'S943\'");
            ProtocolMessageStructures.errorstatus temp19;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,nTSecurityDesc" +
                    "riptor,False)\'");
            temp19 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Comment("reaching state \'S1056\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp19, "return of AuthorizationCheck, state S1056");
            TCSimpleAuth_2K8R2S1136();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S112
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S112()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S112");
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S500\'");
            ProtocolMessageStructures.errorstatus temp20;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp20 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S722\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp20, "return of SimpleBind, state S722");
            this.Manager.Comment("reaching state \'S944\'");
            ProtocolMessageStructures.errorstatus temp21;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,nTSecurityDescriptor,False)" +
                    "\'");
            temp21 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Comment("reaching state \'S1057\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp21, "return of AuthorizationCheck, state S1057");
            TCSimpleAuth_2K8R2S1136();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S114
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S114()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S114");
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S501\'");
            ProtocolMessageStructures.errorstatus temp22;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp22 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S723\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp22, "return of SimpleBind, state S723");
            this.Manager.Comment("reaching state \'S945\'");
            ProtocolMessageStructures.errorstatus temp23;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,msDS_QuotaUsed" +
                    ",False)\'");
            temp23 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1058\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp23, "return of AuthorizationCheck, state S1058");
            TCSimpleAuth_2K8R2S1136();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S116
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S116()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S116");
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S502\'");
            ProtocolMessageStructures.errorstatus temp24;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp24 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S724\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp24, "return of SimpleBind, state S724");
            this.Manager.Comment("reaching state \'S946\'");
            ProtocolMessageStructures.errorstatus temp25;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_QuotaUsed,False)\'");
            temp25 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1059\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp25, "return of AuthorizationCheck, state S1059");
            TCSimpleAuth_2K8R2S1136();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S118
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S118()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S118");
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S503\'");
            ProtocolMessageStructures.errorstatus temp26;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp26 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S725\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp26, "return of SimpleBind, state S725");
            this.Manager.Comment("reaching state \'S947\'");
            ProtocolMessageStructures.errorstatus temp27;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,User_Change_Password,userPassword," +
                    "False)\'");
            temp27 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S1060\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp27, "return of AuthorizationCheck, state S1060");
            TCSimpleAuth_2K8R2S1136();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S12()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S450\'");
            ProtocolMessageStructures.errorstatus temp28;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp28 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S672\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp28, "return of SimpleBind, state S672");
            this.Manager.Comment("reaching state \'S894\'");
            ProtocolMessageStructures.errorstatus temp29;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp29 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1007\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp29, "return of AuthorizationCheck, state S1007");
            this.Manager.Comment("reaching state \'S1113\'");
            ProtocolMessageStructures.errorstatus temp30;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",True)\'");
            temp30 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", true);
            this.Manager.Checkpoint("MS-ADTS-Security_R35");
            this.Manager.Checkpoint("MS-ADTS-Security_R486");
            this.Manager.Comment("reaching state \'S1165\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp30, "return of PasswordChangeOperation, state S1165");
            TCSimpleAuth_2K8R2S1208();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S120
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S120()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S120");
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S504\'");
            ProtocolMessageStructures.errorstatus temp31;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp31 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S726\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp31, "return of SimpleBind, state S726");
            this.Manager.Comment("reaching state \'S948\'");
            ProtocolMessageStructures.errorstatus temp32;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,NotSet,msDS_QuotaE" +
                    "ffective,False)\'");
            temp32 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1061\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp32, "return of AuthorizationCheck, state S1061");
            TCSimpleAuth_2K8R2S1136();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S122
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S122()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S122");
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S505\'");
            ProtocolMessageStructures.errorstatus temp33;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp33 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S727\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp33, "return of SimpleBind, state S727");
            this.Manager.Comment("reaching state \'S949\'");
            ProtocolMessageStructures.errorstatus temp34;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_QuotaEffective,False)\'" +
                    "");
            temp34 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1062\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp34, "return of AuthorizationCheck, state S1062");
            TCSimpleAuth_2K8R2S1136();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S124
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S124()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S124");
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S506\'");
            ProtocolMessageStructures.errorstatus temp35;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp35 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S728\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp35, "return of SimpleBind, state S728");
            this.Manager.Comment("reaching state \'S950\'");
            ProtocolMessageStructures.errorstatus temp36;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,userPassword,False)\'");
            temp36 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S1063\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp36, "return of AuthorizationCheck, state S1063");
            TCSimpleAuth_2K8R2S1136();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S126
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S126()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S126");
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S507\'");
            ProtocolMessageStructures.errorstatus temp37;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp37 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S729\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp37, "return of SimpleBind, state S729");
            this.Manager.Comment("reaching state \'S951\'");
            ProtocolMessageStructures.errorstatus temp38;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp38 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1064\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp38, "return of AuthorizationCheck, state S1064");
            this.Manager.Comment("reaching state \'S1141\'");
            ProtocolMessageStructures.errorstatus temp39;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",False)\'");
            temp39 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", false);
            this.Manager.Checkpoint("MS-ADTS-Security_R32");
            this.Manager.Checkpoint("MS-ADTS-Security_R35");
            this.Manager.Checkpoint("MS-ADTS-Security_R486");
            this.Manager.Comment("reaching state \'S1189\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp39, "return of PasswordChangeOperation, state S1189");
            TCSimpleAuth_2K8R2S1214();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1214()
        {
            this.Manager.Comment("reaching state \'S1214\'");
        }
        #endregion

        #region Test Starting in S128
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S128()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S128");
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S508\'");
            ProtocolMessageStructures.errorstatus temp40;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp40 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S730\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp40, "return of SimpleBind, state S730");
            this.Manager.Comment("reaching state \'S952\'");
            ProtocolMessageStructures.errorstatus temp41;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp41 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1065\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp41, "return of AuthorizationCheck, state S1065");
            this.Manager.Comment("reaching state \'S1142\'");
            ProtocolMessageStructures.errorstatus temp42;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",True)\'");
            temp42 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", true);
            this.Manager.Checkpoint("MS-ADTS-Security_R32");
            this.Manager.Checkpoint("MS-ADTS-Security_R35");
            this.Manager.Checkpoint("MS-ADTS-Security_R486");
            this.Manager.Comment("reaching state \'S1190\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp42, "return of PasswordChangeOperation, state S1190");
            TCSimpleAuth_2K8R2S1214();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S130
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S130()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S130");
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S509\'");
            ProtocolMessageStructures.errorstatus temp43;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp43 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S731\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp43, "return of SimpleBind, state S731");
            this.Manager.Comment("reaching state \'S953\'");
            ProtocolMessageStructures.errorstatus temp44;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp44 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1066\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp44, "return of AuthorizationCheck, state S1066");
            this.Manager.Comment("reaching state \'S1143\'");
            ProtocolMessageStructures.errorstatus temp45;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",False)" +
                    "\'");
            temp45 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1191\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp45, "return of PasswordChangeOperation, state S1191");
            TCSimpleAuth_2K8R2S1214();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S132
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S132()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S132");
            this.Manager.Comment("reaching state \'S132\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S133\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S510\'");
            ProtocolMessageStructures.errorstatus temp46;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp46 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S732\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp46, "return of SimpleBind, state S732");
            this.Manager.Comment("reaching state \'S954\'");
            ProtocolMessageStructures.errorstatus temp47;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,NotSet,use" +
                    "rPassword,False)\'");
            temp47 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1067\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp47, "return of AuthorizationCheck, state S1067");
            this.Manager.Comment("reaching state \'S1144\'");
            ProtocolMessageStructures.errorstatus temp48;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",False)" +
                    "\'");
            temp48 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1192\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp48, "return of PasswordChangeOperation, state S1192");
            TCSimpleAuth_2K8R2S1215();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1215()
        {
            this.Manager.Comment("reaching state \'S1215\'");
        }
        #endregion

        #region Test Starting in S134
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S134()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S134");
            this.Manager.Comment("reaching state \'S134\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S135\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S511\'");
            ProtocolMessageStructures.errorstatus temp49;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp49 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S733\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp49, "return of SimpleBind, state S733");
            this.Manager.Comment("reaching state \'S955\'");
            ProtocolMessageStructures.errorstatus temp50;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,DS_Query_Self_Quot" +
                    "a,msDS_QuotaUsed,False)\'");
            temp50 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R45");
            this.Manager.Comment("reaching state \'S1068\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp50, "return of AuthorizationCheck, state S1068");
            this.Manager.Comment("reaching state \'S1145\'");
            ProtocolMessageStructures.errorstatus temp51;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",False)\'");
            temp51 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1193\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp51, "return of PasswordChangeOperation, state S1193");
            TCSimpleAuth_2K8R2S1215();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S136
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S136()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S136");
            this.Manager.Comment("reaching state \'S136\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S137\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S512\'");
            ProtocolMessageStructures.errorstatus temp52;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp52 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S734\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp52, "return of SimpleBind, state S734");
            this.Manager.Comment("reaching state \'S956\'");
            ProtocolMessageStructures.errorstatus temp53;
            this.Manager.Comment("executing step \'call AuthorizationCheck(ACC_SYS_SEC_READ_CONTROL,NotSet,nTSecurit" +
                    "yDescriptor,False)\'");
            temp53 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.ACC_SYS_SEC_READ_CONTROL, ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R39");
            this.Manager.Comment("reaching state \'S1069\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp53, "return of AuthorizationCheck, state S1069");
            this.Manager.Comment("reaching state \'S1146\'");
            ProtocolMessageStructures.errorstatus temp54;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",True)\'" +
                    "");
            temp54 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1194\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp54, "return of PasswordChangeOperation, state S1194");
            TCSimpleAuth_2K8R2S1215();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S138
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S138()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S138");
            this.Manager.Comment("reaching state \'S138\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S139\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S513\'");
            ProtocolMessageStructures.errorstatus temp55;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp55 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S735\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp55, "return of SimpleBind, state S735");
            this.Manager.Comment("reaching state \'S957\'");
            ProtocolMessageStructures.errorstatus temp56;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,DS_Query_Self_Quot" +
                    "a,msDS_QuotaEffective,False)\'");
            temp56 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R41");
            this.Manager.Comment("reaching state \'S1070\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp56, "return of AuthorizationCheck, state S1070");
            this.Manager.Comment("reaching state \'S1147\'");
            ProtocolMessageStructures.errorstatus temp57;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",True)\'");
            temp57 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1195\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp57, "return of PasswordChangeOperation, state S1195");
            TCSimpleAuth_2K8R2S1215();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S14()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S451\'");
            ProtocolMessageStructures.errorstatus temp58;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp58 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S673\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp58, "return of SimpleBind, state S673");
            this.Manager.Comment("reaching state \'S895\'");
            ProtocolMessageStructures.errorstatus temp59;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp59 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1008\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp59, "return of AuthorizationCheck, state S1008");
            this.Manager.Comment("reaching state \'S1114\'");
            ProtocolMessageStructures.errorstatus temp60;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",False)" +
                    "\'");
            temp60 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1166\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp60, "return of PasswordChangeOperation, state S1166");
            TCSimpleAuth_2K8R2S1208();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S140
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S140()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S140");
            this.Manager.Comment("reaching state \'S140\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S141\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S514\'");
            ProtocolMessageStructures.errorstatus temp61;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp61 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S736\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp61, "return of SimpleBind, state S736");
            this.Manager.Comment("reaching state \'S958\'");
            ProtocolMessageStructures.errorstatus temp62;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,NotSet,msDS_QuotaU" +
                    "sed,False)\'");
            temp62 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1071\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp62, "return of AuthorizationCheck, state S1071");
            TCSimpleAuth_2K8R2S1148();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1148()
        {
            this.Manager.Comment("reaching state \'S1148\'");
        }
        #endregion

        #region Test Starting in S142
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S142()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S142");
            this.Manager.Comment("reaching state \'S142\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S143\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S515\'");
            ProtocolMessageStructures.errorstatus temp63;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp63 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S737\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp63, "return of SimpleBind, state S737");
            this.Manager.Comment("reaching state \'S959\'");
            ProtocolMessageStructures.errorstatus temp64;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,nTSecurityDesc" +
                    "riptor,False)\'");
            temp64 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Comment("reaching state \'S1072\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp64, "return of AuthorizationCheck, state S1072");
            TCSimpleAuth_2K8R2S1148();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S144
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S144()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S144");
            this.Manager.Comment("reaching state \'S144\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S145\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S516\'");
            ProtocolMessageStructures.errorstatus temp65;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp65 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S738\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp65, "return of SimpleBind, state S738");
            this.Manager.Comment("reaching state \'S960\'");
            ProtocolMessageStructures.errorstatus temp66;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,nTSecurityDescriptor,False)" +
                    "\'");
            temp66 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Comment("reaching state \'S1073\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp66, "return of AuthorizationCheck, state S1073");
            TCSimpleAuth_2K8R2S1148();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S146
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S146()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S146");
            this.Manager.Comment("reaching state \'S146\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S147\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S517\'");
            ProtocolMessageStructures.errorstatus temp67;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp67 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S739\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp67, "return of SimpleBind, state S739");
            this.Manager.Comment("reaching state \'S961\'");
            ProtocolMessageStructures.errorstatus temp68;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,msDS_QuotaUsed" +
                    ",False)\'");
            temp68 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1074\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp68, "return of AuthorizationCheck, state S1074");
            TCSimpleAuth_2K8R2S1148();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S148
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S148()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S148");
            this.Manager.Comment("reaching state \'S148\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S149\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S518\'");
            ProtocolMessageStructures.errorstatus temp69;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp69 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S740\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp69, "return of SimpleBind, state S740");
            this.Manager.Comment("reaching state \'S962\'");
            ProtocolMessageStructures.errorstatus temp70;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_QuotaUsed,False)\'");
            temp70 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1075\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp70, "return of AuthorizationCheck, state S1075");
            TCSimpleAuth_2K8R2S1148();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S150
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S150()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S150");
            this.Manager.Comment("reaching state \'S150\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S151\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S519\'");
            ProtocolMessageStructures.errorstatus temp71;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp71 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S741\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp71, "return of SimpleBind, state S741");
            this.Manager.Comment("reaching state \'S963\'");
            ProtocolMessageStructures.errorstatus temp72;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,User_Change_Password,userPassword," +
                    "False)\'");
            temp72 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S1076\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp72, "return of AuthorizationCheck, state S1076");
            TCSimpleAuth_2K8R2S1148();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S152
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S152()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S152");
            this.Manager.Comment("reaching state \'S152\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S153\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S520\'");
            ProtocolMessageStructures.errorstatus temp73;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp73 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S742\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp73, "return of SimpleBind, state S742");
            this.Manager.Comment("reaching state \'S964\'");
            ProtocolMessageStructures.errorstatus temp74;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,msDS_QuotaEffe" +
                    "ctive,False)\'");
            temp74 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1077\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp74, "return of AuthorizationCheck, state S1077");
            TCSimpleAuth_2K8R2S1148();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S154
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S154()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S154");
            this.Manager.Comment("reaching state \'S154\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S155\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S521\'");
            ProtocolMessageStructures.errorstatus temp75;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp75 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S743\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp75, "return of SimpleBind, state S743");
            this.Manager.Comment("reaching state \'S965\'");
            ProtocolMessageStructures.errorstatus temp76;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,NotSet,msDS_QuotaE" +
                    "ffective,False)\'");
            temp76 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1078\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp76, "return of AuthorizationCheck, state S1078");
            TCSimpleAuth_2K8R2S1148();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S156
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S156()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S156");
            this.Manager.Comment("reaching state \'S156\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S157\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S522\'");
            ProtocolMessageStructures.errorstatus temp77;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp77 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S744\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp77, "return of SimpleBind, state S744");
            this.Manager.Comment("reaching state \'S966\'");
            ProtocolMessageStructures.errorstatus temp78;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,userPassword,False)\'");
            temp78 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S1079\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp78, "return of AuthorizationCheck, state S1079");
            TCSimpleAuth_2K8R2S1148();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S158
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S158()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S158");
            this.Manager.Comment("reaching state \'S158\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S159\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S523\'");
            ProtocolMessageStructures.errorstatus temp79;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp79 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S745\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp79, "return of SimpleBind, state S745");
            this.Manager.Comment("reaching state \'S967\'");
            ProtocolMessageStructures.errorstatus temp80;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp80 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1080\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp80, "return of AuthorizationCheck, state S1080");
            this.Manager.Comment("reaching state \'S1149\'");
            ProtocolMessageStructures.errorstatus temp81;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",True)\'");
            temp81 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", true);
            this.Manager.Checkpoint("MS-ADTS-Security_R33");
            this.Manager.Checkpoint("MS-ADTS-Security_R35");
            this.Manager.Checkpoint("MS-ADTS-Security_R486");
            this.Manager.Comment("reaching state \'S1196\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp81, "return of PasswordChangeOperation, state S1196");
            TCSimpleAuth_2K8R2S1216();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1216()
        {
            this.Manager.Comment("reaching state \'S1216\'");
        }
        #endregion

        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S16()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S452\'");
            ProtocolMessageStructures.errorstatus temp82;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp82 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S674\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp82, "return of SimpleBind, state S674");
            this.Manager.Comment("reaching state \'S896\'");
            ProtocolMessageStructures.errorstatus temp83;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,NotSet,msDS_QuotaU" +
                    "sed,False)\'");
            temp83 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1009\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp83, "return of AuthorizationCheck, state S1009");
            TCSimpleAuth_2K8R2S1115();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1115()
        {
            this.Manager.Comment("reaching state \'S1115\'");
        }
        #endregion

        #region Test Starting in S160
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S160()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S160");
            this.Manager.Comment("reaching state \'S160\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S161\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S524\'");
            ProtocolMessageStructures.errorstatus temp84;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp84 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S746\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp84, "return of SimpleBind, state S746");
            this.Manager.Comment("reaching state \'S968\'");
            ProtocolMessageStructures.errorstatus temp85;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp85 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1081\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp85, "return of AuthorizationCheck, state S1081");
            this.Manager.Comment("reaching state \'S1150\'");
            ProtocolMessageStructures.errorstatus temp86;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",False)\'");
            temp86 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1197\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp86, "return of PasswordChangeOperation, state S1197");
            TCSimpleAuth_2K8R2S1216();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S162
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S162()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S162");
            this.Manager.Comment("reaching state \'S162\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S163\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S525\'");
            ProtocolMessageStructures.errorstatus temp87;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp87 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S747\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp87, "return of SimpleBind, state S747");
            this.Manager.Comment("reaching state \'S969\'");
            ProtocolMessageStructures.errorstatus temp88;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp88 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1082\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp88, "return of AuthorizationCheck, state S1082");
            this.Manager.Comment("reaching state \'S1151\'");
            ProtocolMessageStructures.errorstatus temp89;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",True)\'" +
                    "");
            temp89 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1198\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp89, "return of PasswordChangeOperation, state S1198");
            TCSimpleAuth_2K8R2S1216();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S164
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S164()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S164");
            this.Manager.Comment("reaching state \'S164\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S165\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S526\'");
            ProtocolMessageStructures.errorstatus temp90;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp90 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S748\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp90, "return of SimpleBind, state S748");
            this.Manager.Comment("reaching state \'S970\'");
            ProtocolMessageStructures.errorstatus temp91;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,NotSet,msDS_QuotaU" +
                    "sed,False)\'");
            temp91 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1083\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp91, "return of AuthorizationCheck, state S1083");
            TCSimpleAuth_2K8R2S1152();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1152()
        {
            this.Manager.Comment("reaching state \'S1152\'");
        }
        #endregion

        #region Test Starting in S166
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S166()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S166");
            this.Manager.Comment("reaching state \'S166\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S167\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S527\'");
            ProtocolMessageStructures.errorstatus temp92;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp92 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S749\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp92, "return of SimpleBind, state S749");
            this.Manager.Comment("reaching state \'S971\'");
            ProtocolMessageStructures.errorstatus temp93;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,NotSet,use" +
                    "rPassword,False)\'");
            temp93 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1084\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp93, "return of AuthorizationCheck, state S1084");
            this.Manager.Comment("reaching state \'S1153\'");
            ProtocolMessageStructures.errorstatus temp94;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",False)" +
                    "\'");
            temp94 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1199\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp94, "return of PasswordChangeOperation, state S1199");
            TCSimpleAuth_2K8R2S1217();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1217()
        {
            this.Manager.Comment("reaching state \'S1217\'");
        }
        #endregion

        #region Test Starting in S168
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S168()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S168");
            this.Manager.Comment("reaching state \'S168\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S169\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S528\'");
            ProtocolMessageStructures.errorstatus temp95;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp95 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S750\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp95, "return of SimpleBind, state S750");
            this.Manager.Comment("reaching state \'S972\'");
            ProtocolMessageStructures.errorstatus temp96;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,DS_Query_Self_Quot" +
                    "a,msDS_QuotaUsed,False)\'");
            temp96 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R45");
            this.Manager.Comment("reaching state \'S1085\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp96, "return of AuthorizationCheck, state S1085");
            this.Manager.Comment("reaching state \'S1154\'");
            ProtocolMessageStructures.errorstatus temp97;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",False)\'");
            temp97 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1200\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp97, "return of PasswordChangeOperation, state S1200");
            TCSimpleAuth_2K8R2S1217();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S170
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S170()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S170");
            this.Manager.Comment("reaching state \'S170\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S171\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S529\'");
            ProtocolMessageStructures.errorstatus temp98;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp98 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S751\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp98, "return of SimpleBind, state S751");
            this.Manager.Comment("reaching state \'S973\'");
            ProtocolMessageStructures.errorstatus temp99;
            this.Manager.Comment("executing step \'call AuthorizationCheck(ACC_SYS_SEC_READ_CONTROL,NotSet,nTSecurit" +
                    "yDescriptor,False)\'");
            temp99 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.ACC_SYS_SEC_READ_CONTROL, ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R39");
            this.Manager.Comment("reaching state \'S1086\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp99, "return of AuthorizationCheck, state S1086");
            this.Manager.Comment("reaching state \'S1155\'");
            ProtocolMessageStructures.errorstatus temp100;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",True)\'" +
                    "");
            temp100 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1201\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp100, "return of PasswordChangeOperation, state S1201");
            TCSimpleAuth_2K8R2S1217();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S172
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S172()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S172");
            this.Manager.Comment("reaching state \'S172\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S173\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S530\'");
            ProtocolMessageStructures.errorstatus temp101;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp101 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S752\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp101, "return of SimpleBind, state S752");
            this.Manager.Comment("reaching state \'S974\'");
            ProtocolMessageStructures.errorstatus temp102;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,DS_Query_Self_Quot" +
                    "a,msDS_QuotaEffective,False)\'");
            temp102 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R41");
            this.Manager.Comment("reaching state \'S1087\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp102, "return of AuthorizationCheck, state S1087");
            this.Manager.Comment("reaching state \'S1156\'");
            ProtocolMessageStructures.errorstatus temp103;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",True)\'");
            temp103 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1202\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp103, "return of PasswordChangeOperation, state S1202");
            TCSimpleAuth_2K8R2S1217();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S174
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S174()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S174");
            this.Manager.Comment("reaching state \'S174\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S175\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S531\'");
            ProtocolMessageStructures.errorstatus temp104;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp104 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S753\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp104, "return of SimpleBind, state S753");
            this.Manager.Comment("reaching state \'S975\'");
            ProtocolMessageStructures.errorstatus temp105;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,nTSecurityDesc" +
                    "riptor,False)\'");
            temp105 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Comment("reaching state \'S1088\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp105, "return of AuthorizationCheck, state S1088");
            TCSimpleAuth_2K8R2S1152();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S176
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S176()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S176");
            this.Manager.Comment("reaching state \'S176\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S177\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S532\'");
            ProtocolMessageStructures.errorstatus temp106;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp106 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S754\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp106, "return of SimpleBind, state S754");
            this.Manager.Comment("reaching state \'S976\'");
            ProtocolMessageStructures.errorstatus temp107;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,nTSecurityDescriptor,False)" +
                    "\'");
            temp107 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Comment("reaching state \'S1089\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp107, "return of AuthorizationCheck, state S1089");
            TCSimpleAuth_2K8R2S1152();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S178
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S178()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S178");
            this.Manager.Comment("reaching state \'S178\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S179\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S533\'");
            ProtocolMessageStructures.errorstatus temp108;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp108 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S755\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp108, "return of SimpleBind, state S755");
            this.Manager.Comment("reaching state \'S977\'");
            ProtocolMessageStructures.errorstatus temp109;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,msDS_QuotaUsed" +
                    ",False)\'");
            temp109 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1090\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp109, "return of AuthorizationCheck, state S1090");
            TCSimpleAuth_2K8R2S1152();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S18()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S453\'");
            ProtocolMessageStructures.errorstatus temp110;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp110 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S675\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp110, "return of SimpleBind, state S675");
            this.Manager.Comment("reaching state \'S897\'");
            ProtocolMessageStructures.errorstatus temp111;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,NotSet,use" +
                    "rPassword,False)\'");
            temp111 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1010\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp111, "return of AuthorizationCheck, state S1010");
            this.Manager.Comment("reaching state \'S1116\'");
            ProtocolMessageStructures.errorstatus temp112;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",False)" +
                    "\'");
            temp112 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1167\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp112, "return of PasswordChangeOperation, state S1167");
            TCSimpleAuth_2K8R2S1209();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1209()
        {
            this.Manager.Comment("reaching state \'S1209\'");
        }
        #endregion

        #region Test Starting in S180
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S180()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S180");
            this.Manager.Comment("reaching state \'S180\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S181\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S534\'");
            ProtocolMessageStructures.errorstatus temp113;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp113 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S756\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp113, "return of SimpleBind, state S756");
            this.Manager.Comment("reaching state \'S978\'");
            ProtocolMessageStructures.errorstatus temp114;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_QuotaUsed,False)\'");
            temp114 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1091\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp114, "return of AuthorizationCheck, state S1091");
            TCSimpleAuth_2K8R2S1152();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S182
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S182()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S182");
            this.Manager.Comment("reaching state \'S182\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S183\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S535\'");
            ProtocolMessageStructures.errorstatus temp115;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp115 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S757\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp115, "return of SimpleBind, state S757");
            this.Manager.Comment("reaching state \'S979\'");
            ProtocolMessageStructures.errorstatus temp116;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,User_Change_Password,userPassword," +
                    "False)\'");
            temp116 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S1092\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp116, "return of AuthorizationCheck, state S1092");
            TCSimpleAuth_2K8R2S1152();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S184
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S184()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S184");
            this.Manager.Comment("reaching state \'S184\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S185\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S536\'");
            ProtocolMessageStructures.errorstatus temp117;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp117 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S758\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp117, "return of SimpleBind, state S758");
            this.Manager.Comment("reaching state \'S980\'");
            ProtocolMessageStructures.errorstatus temp118;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,msDS_QuotaEffe" +
                    "ctive,False)\'");
            temp118 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1093\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp118, "return of AuthorizationCheck, state S1093");
            TCSimpleAuth_2K8R2S1152();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S186
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S186()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S186");
            this.Manager.Comment("reaching state \'S186\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S187\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S537\'");
            ProtocolMessageStructures.errorstatus temp119;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp119 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S759\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp119, "return of SimpleBind, state S759");
            this.Manager.Comment("reaching state \'S981\'");
            ProtocolMessageStructures.errorstatus temp120;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,NotSet,msDS_QuotaE" +
                    "ffective,False)\'");
            temp120 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1094\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp120, "return of AuthorizationCheck, state S1094");
            TCSimpleAuth_2K8R2S1152();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S188
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S188()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S188");
            this.Manager.Comment("reaching state \'S188\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S189\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S538\'");
            ProtocolMessageStructures.errorstatus temp121;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp121 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S760\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp121, "return of SimpleBind, state S760");
            this.Manager.Comment("reaching state \'S982\'");
            ProtocolMessageStructures.errorstatus temp122;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_QuotaEffective,False)\'" +
                    "");
            temp122 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1095\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp122, "return of AuthorizationCheck, state S1095");
            TCSimpleAuth_2K8R2S1152();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S190
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S190()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S190");
            this.Manager.Comment("reaching state \'S190\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S191\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S539\'");
            ProtocolMessageStructures.errorstatus temp123;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp123 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S761\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp123, "return of SimpleBind, state S761");
            TCSimpleAuth_2K8R2S983();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S983()
        {
            this.Manager.Comment("reaching state \'S983\'");
        }
        #endregion

        #region Test Starting in S192
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S192()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S192");
            this.Manager.Comment("reaching state \'S192\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S193\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S540\'");
            ProtocolMessageStructures.errorstatus temp124;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,invalidPassword,LDAP_SSL_GC_PORT,Fa" +
                    "lse)\'");
            temp124 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S762\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp124, "return of SimpleBind, state S762");
            TCSimpleAuth_2K8R2S983();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S194
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S194()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S194");
            this.Manager.Comment("reaching state \'S194\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S195\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S541\'");
            ProtocolMessageStructures.errorstatus temp125;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,validPassword,LDAP_PORT,True)\'");
            temp125 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S763\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp125, "return of SimpleBind, state S763");
            TCSimpleAuth_2K8R2S984();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S984()
        {
            this.Manager.Comment("reaching state \'S984\'");
        }
        #endregion

        #region Test Starting in S196
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S196()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S196");
            this.Manager.Comment("reaching state \'S196\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S197\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S542\'");
            ProtocolMessageStructures.errorstatus temp126;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,validPassword,LDAP_SSL_PORT,False)\'" +
                    "");
            temp126 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S764\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp126, "return of SimpleBind, state S764");
            TCSimpleAuth_2K8R2S983();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S198
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S198()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S198");
            this.Manager.Comment("reaching state \'S198\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S199\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S543\'");
            ProtocolMessageStructures.errorstatus temp127;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp127 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S765\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp127, "return of SimpleBind, state S765");
            this.Manager.Comment("reaching state \'S985\'");
            ProtocolMessageStructures.errorstatus temp128;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,nTSecurityDesc" +
                    "riptor,False)\'");
            temp128 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Comment("reaching state \'S1096\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp128, "return of AuthorizationCheck, state S1096");
            TCSimpleAuth_2K8R2S1121();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1121()
        {
            this.Manager.Comment("reaching state \'S1121\'");
        }
        #endregion

        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S2()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S445\'");
            ProtocolMessageStructures.errorstatus temp129;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp129 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S667\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp129, "return of SimpleBind, state S667");
            this.Manager.Comment("reaching state \'S889\'");
            ProtocolMessageStructures.errorstatus temp130;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp130 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1002\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp130, "return of AuthorizationCheck, state S1002");
            this.Manager.Comment("reaching state \'S1108\'");
            ProtocolMessageStructures.errorstatus temp131;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",False)\'");
            temp131 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", false);
            this.Manager.Checkpoint("MS-ADTS-Security_R35");
            this.Manager.Checkpoint("MS-ADTS-Security_R486");
            this.Manager.Comment("reaching state \'S1160\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp131, "return of PasswordChangeOperation, state S1160");
            TCSimpleAuth_2K8R2S1206();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1206()
        {
            this.Manager.Comment("reaching state \'S1206\'");
        }
        #endregion

        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S20()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S454\'");
            ProtocolMessageStructures.errorstatus temp132;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp132 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S676\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp132, "return of SimpleBind, state S676");
            this.Manager.Comment("reaching state \'S898\'");
            ProtocolMessageStructures.errorstatus temp133;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,DS_Query_Self_Quot" +
                    "a,msDS_QuotaUsed,False)\'");
            temp133 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R45");
            this.Manager.Comment("reaching state \'S1011\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp133, "return of AuthorizationCheck, state S1011");
            this.Manager.Comment("reaching state \'S1117\'");
            ProtocolMessageStructures.errorstatus temp134;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",False)\'");
            temp134 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1168\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp134, "return of PasswordChangeOperation, state S1168");
            TCSimpleAuth_2K8R2S1209();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S200
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S200()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S200");
            this.Manager.Comment("reaching state \'S200\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S201\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S544\'");
            ProtocolMessageStructures.errorstatus temp135;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,validPassword,LDAP_PORT,False)\'");
            temp135 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S766\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp135, "return of SimpleBind, state S766");
            TCSimpleAuth_2K8R2S986();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S986()
        {
            this.Manager.Comment("reaching state \'S986\'");
        }
        #endregion

        #region Test Starting in S202
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S202()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S202");
            this.Manager.Comment("reaching state \'S202\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S203\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S545\'");
            ProtocolMessageStructures.errorstatus temp136;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,validPassword,LDAP_GC_PORT,False)\'");
            temp136 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S767\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp136, "return of SimpleBind, state S767");
            TCSimpleAuth_2K8R2S986();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S204
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S204()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S204");
            this.Manager.Comment("reaching state \'S204\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S205\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S546\'");
            ProtocolMessageStructures.errorstatus temp137;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_PORT,False)\'" +
                    "");
            temp137 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S768\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp137, "return of SimpleBind, state S768");
            this.Manager.Comment("reaching state \'S987\'");
            ProtocolMessageStructures.errorstatus temp138;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,msDS_QuotaEffe" +
                    "ctive,False)\'");
            temp138 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1097\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp138, "return of AuthorizationCheck, state S1097");
            TCSimpleAuth_2K8R2S1136();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S206
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S206()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S206");
            this.Manager.Comment("reaching state \'S206\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S207\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S547\'");
            ProtocolMessageStructures.errorstatus temp139;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,False)\'");
            temp139 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S769\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp139, "return of SimpleBind, state S769");
            this.Manager.Comment("reaching state \'S988\'");
            ProtocolMessageStructures.errorstatus temp140;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_QuotaEffective,False)\'" +
                    "");
            temp140 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1098\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp140, "return of AuthorizationCheck, state S1098");
            TCSimpleAuth_2K8R2S1127();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1127()
        {
            this.Manager.Comment("reaching state \'S1127\'");
        }
        #endregion

        #region Test Starting in S208
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S208()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S208");
            this.Manager.Comment("reaching state \'S208\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S209\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S548\'");
            ProtocolMessageStructures.errorstatus temp141;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,validPassword,LDAP_GC_PORT,True)\'");
            temp141 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S770\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp141, "return of SimpleBind, state S770");
            TCSimpleAuth_2K8R2S984();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S210
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S210()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S210");
            this.Manager.Comment("reaching state \'S210\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S211\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S549\'");
            ProtocolMessageStructures.errorstatus temp142;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,invalidPassword,LDAP_GC_PORT,True)\'" +
                    "");
            temp142 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S771\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp142, "return of SimpleBind, state S771");
            TCSimpleAuth_2K8R2S984();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S212
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S212()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S212");
            this.Manager.Comment("reaching state \'S212\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S213\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S550\'");
            ProtocolMessageStructures.errorstatus temp143;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,invalidPassword,LDAP_PORT,True)\'");
            temp143 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S772\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp143, "return of SimpleBind, state S772");
            TCSimpleAuth_2K8R2S984();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S214
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S214()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S214");
            this.Manager.Comment("reaching state \'S214\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S215\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S551\'");
            ProtocolMessageStructures.errorstatus temp144;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,invalidPassword,LDAP_PORT,False)\'");
            temp144 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S773\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp144, "return of SimpleBind, state S773");
            TCSimpleAuth_2K8R2S986();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S216
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S216()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S216");
            this.Manager.Comment("reaching state \'S216\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S217\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S552\'");
            ProtocolMessageStructures.errorstatus temp145;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,invalidPassword,LDAP_GC_PORT,False)" +
                    "\'");
            temp145 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S774\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp145, "return of SimpleBind, state S774");
            TCSimpleAuth_2K8R2S986();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S218
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S218()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S218");
            this.Manager.Comment("reaching state \'S218\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S219\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S553\'");
            ProtocolMessageStructures.errorstatus temp146;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp146 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S775\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp146, "return of SimpleBind, state S775");
            this.Manager.Comment("reaching state \'S989\'");
            ProtocolMessageStructures.errorstatus temp147;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,userPassword,False)\'");
            temp147 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S1099\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp147, "return of AuthorizationCheck, state S1099");
            TCSimpleAuth_2K8R2S1121();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S22()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S455\'");
            ProtocolMessageStructures.errorstatus temp148;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp148 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S677\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp148, "return of SimpleBind, state S677");
            this.Manager.Comment("reaching state \'S899\'");
            ProtocolMessageStructures.errorstatus temp149;
            this.Manager.Comment("executing step \'call AuthorizationCheck(ACC_SYS_SEC_READ_CONTROL,NotSet,nTSecurit" +
                    "yDescriptor,False)\'");
            temp149 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.ACC_SYS_SEC_READ_CONTROL, ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R39");
            this.Manager.Comment("reaching state \'S1012\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp149, "return of AuthorizationCheck, state S1012");
            this.Manager.Comment("reaching state \'S1118\'");
            ProtocolMessageStructures.errorstatus temp150;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",True)\'" +
                    "");
            temp150 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1169\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp150, "return of PasswordChangeOperation, state S1169");
            TCSimpleAuth_2K8R2S1209();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S220
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S220()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S220");
            this.Manager.Comment("reaching state \'S220\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S221\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S554\'");
            ProtocolMessageStructures.errorstatus temp151;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp151 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S776\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp151, "return of SimpleBind, state S776");
            this.Manager.Comment("reaching state \'S990\'");
            ProtocolMessageStructures.errorstatus temp152;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,userPassword,False)\'");
            temp152 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S1100\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp152, "return of AuthorizationCheck, state S1100");
            TCSimpleAuth_2K8R2S1127();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S222
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S222()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S222");
            this.Manager.Comment("reaching state \'S222\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S223\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S555\'");
            ProtocolMessageStructures.errorstatus temp153;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp153 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S777\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp153, "return of SimpleBind, state S777");
            this.Manager.Comment("reaching state \'S991\'");
            ProtocolMessageStructures.errorstatus temp154;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp154 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1101\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp154, "return of AuthorizationCheck, state S1101");
            this.Manager.Comment("reaching state \'S1157\'");
            ProtocolMessageStructures.errorstatus temp155;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",True)\'" +
                    "");
            temp155 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1203\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp155, "return of PasswordChangeOperation, state S1203");
            TCSimpleAuth_2K8R2S1212();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1212()
        {
            this.Manager.Comment("reaching state \'S1212\'");
        }
        #endregion

        #region Test Starting in S224
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S224()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S224");
            this.Manager.Comment("reaching state \'S224\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S225\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S556\'");
            ProtocolMessageStructures.errorstatus temp156;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,invalidPassword,LDAP_SSL_PORT,False" +
                    ")\'");
            temp156 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S778\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp156, "return of SimpleBind, state S778");
            TCSimpleAuth_2K8R2S983();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S226
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S226()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S226");
            this.Manager.Comment("reaching state \'S226\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S227\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S557\'");
            ProtocolMessageStructures.errorstatus temp157;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,invalidPassword,LDAP_GC_PORT,True" +
                    ")\'");
            temp157 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S779\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp157, "return of SimpleBind, state S779");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S992()
        {
            this.Manager.Comment("reaching state \'S992\'");
        }
        #endregion

        #region Test Starting in S228
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S228()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S228");
            this.Manager.Comment("reaching state \'S228\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S229\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S558\'");
            ProtocolMessageStructures.errorstatus temp158;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,validPassword,LDAP_GC_PORT,True)\'" +
                    "");
            temp158 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S780\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp158, "return of SimpleBind, state S780");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S230
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S230()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S230");
            this.Manager.Comment("reaching state \'S230\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S231\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S559\'");
            ProtocolMessageStructures.errorstatus temp159;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,validPassword,LDAP_PORT,True)\'");
            temp159 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S781\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp159, "return of SimpleBind, state S781");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S232
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S232()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S232");
            this.Manager.Comment("reaching state \'S232\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S233\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S560\'");
            ProtocolMessageStructures.errorstatus temp160;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,invalidPassword,LDAP_PORT,True)\'");
            temp160 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S782\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp160, "return of SimpleBind, state S782");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S234
        // Mapping a SPN to multiple objects is not allowed on Windows 2012R2.
        // This test case is doing so.
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S234()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S234");
            this.Manager.Comment("reaching state \'S234\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S235\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S561\'");
            ProtocolMessageStructures.errorstatus temp161;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,validPassword,LDAP_SSL_" +
                    "GC_PORT,False)\'");
            temp161 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S783\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp161, "return of SimpleBind, state S783");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S236
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S236()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S236");
            this.Manager.Comment("reaching state \'S236\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S237\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S562\'");
            ProtocolMessageStructures.errorstatus temp162;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,validPassword,LDAP_SSL_GC_PORT,Fa" +
                    "lse)\'");
            temp162 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S784\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp162, "return of SimpleBind, state S784");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S238
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S238()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S238");
            this.Manager.Comment("reaching state \'S238\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S239\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S563\'");
            ProtocolMessageStructures.errorstatus temp163;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,invalidPassword,LDAP_SS" +
                    "L_GC_PORT,False)\'");
            temp163 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S785\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp163, "return of SimpleBind, state S785");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S24()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S456\'");
            ProtocolMessageStructures.errorstatus temp164;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp164 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S678\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp164, "return of SimpleBind, state S678");
            this.Manager.Comment("reaching state \'S900\'");
            ProtocolMessageStructures.errorstatus temp165;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp165 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1013\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp165, "return of AuthorizationCheck, state S1013");
            this.Manager.Comment("reaching state \'S1119\'");
            ProtocolMessageStructures.errorstatus temp166;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",True)\'" +
                    "");
            temp166 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1170\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp166, "return of PasswordChangeOperation, state S1170");
            TCSimpleAuth_2K8R2S1206();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S240
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S240()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S240");
            this.Manager.Comment("reaching state \'S240\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S241\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S564\'");
            ProtocolMessageStructures.errorstatus temp167;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,invalidPassword,LDAP_SS" +
                    "L_PORT,False)\'");
            temp167 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S786\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp167, "return of SimpleBind, state S786");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S242
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S242()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S242");
            this.Manager.Comment("reaching state \'S242\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S243\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S565\'");
            ProtocolMessageStructures.errorstatus temp168;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,invalidPassword,LDAP_SSL_GC_PORT,Fa" +
                    "lse)\'");
            temp168 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S787\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp168, "return of SimpleBind, state S787");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S244
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S244()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S244");
            this.Manager.Comment("reaching state \'S244\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S245\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S566\'");
            ProtocolMessageStructures.errorstatus temp169;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,invalidPassword,LDAP_SSL_GC_PORT," +
                    "False)\'");
            temp169 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S788\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp169, "return of SimpleBind, state S788");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S246
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S246()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S246");
            this.Manager.Comment("reaching state \'S246\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S247\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S567\'");
            ProtocolMessageStructures.errorstatus temp170;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,invalidPassword,LDAP_SSL_PORT,False" +
                    ")\'");
            temp170 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S789\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp170, "return of SimpleBind, state S789");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S248
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S248()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S248");
            this.Manager.Comment("reaching state \'S248\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S249\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S568\'");
            ProtocolMessageStructures.errorstatus temp171;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,invalidPassword,LDAP_SSL_PORT,Fal" +
                    "se)\'");
            temp171 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S790\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp171, "return of SimpleBind, state S790");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S250
        // Mapping a SPN to multiple objects is not allowed on Windows 2012R2.
        // This test case is doing so.
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S250()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S250");
            this.Manager.Comment("reaching state \'S250\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S251\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S569\'");
            ProtocolMessageStructures.errorstatus temp172;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,validPassword,LDAP_SSL_" +
                    "PORT,False)\'");
            temp172 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S791\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp172, "return of SimpleBind, state S791");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S252
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S252()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S252");
            this.Manager.Comment("reaching state \'S252\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S253\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S570\'");
            ProtocolMessageStructures.errorstatus temp173;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,validPassword,LDAP_SSL_PORT,False" +
                    ")\'");
            temp173 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S792\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp173, "return of SimpleBind, state S792");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S254
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S254()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S254");
            this.Manager.Comment("reaching state \'S254\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S255\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S571\'");
            ProtocolMessageStructures.errorstatus temp174;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,invalidPassword,LDAP_PORT,False)\'");
            temp174 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S793\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp174, "return of SimpleBind, state S793");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S256
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S256()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S256");
            this.Manager.Comment("reaching state \'S256\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S257\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S572\'");
            ProtocolMessageStructures.errorstatus temp175;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,invalidPassword,LDAP_PO" +
                    "RT,False)\'");
            temp175 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S794\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp175, "return of SimpleBind, state S794");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S258
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S258()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S258");
            this.Manager.Comment("reaching state \'S258\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S259\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S573\'");
            ProtocolMessageStructures.errorstatus temp176;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,invalidPassword,LDAP_PORT,False)\'" +
                    "");
            temp176 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S795\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp176, "return of SimpleBind, state S795");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S26()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S457\'");
            ProtocolMessageStructures.errorstatus temp177;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp177 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S679\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp177, "return of SimpleBind, state S679");
            this.Manager.Comment("reaching state \'S901\'");
            ProtocolMessageStructures.errorstatus temp178;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,DS_Query_Self_Quot" +
                    "a,msDS_QuotaEffective,False)\'");
            temp178 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R41");
            this.Manager.Comment("reaching state \'S1014\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp178, "return of AuthorizationCheck, state S1014");
            this.Manager.Comment("reaching state \'S1120\'");
            ProtocolMessageStructures.errorstatus temp179;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",True)\'");
            temp179 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1171\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp179, "return of PasswordChangeOperation, state S1171");
            TCSimpleAuth_2K8R2S1209();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S260
        // Mapping a SPN to multiple objects is not allowed on Windows 2012R2.
        // This test case is doing so.
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S260()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S260");
            this.Manager.Comment("reaching state \'S260\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S261\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S574\'");
            ProtocolMessageStructures.errorstatus temp180;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,validPassword,LDAP_PORT" +
                    ",False)\'");
            temp180 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S796\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp180, "return of SimpleBind, state S796");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S262
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S262()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S262");
            this.Manager.Comment("reaching state \'S262\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S263\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S575\'");
            ProtocolMessageStructures.errorstatus temp181;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,validPassword,LDAP_PORT,False)\'");
            temp181 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S797\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp181, "return of SimpleBind, state S797");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S264
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S264()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S264");
            this.Manager.Comment("reaching state \'S264\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S265\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S576\'");
            ProtocolMessageStructures.errorstatus temp182;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,invalidPassword,LDAP_GC_PORT,False)" +
                    "\'");
            temp182 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S798\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp182, "return of SimpleBind, state S798");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S266
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S266()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S266");
            this.Manager.Comment("reaching state \'S266\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S267\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S577\'");
            ProtocolMessageStructures.errorstatus temp183;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,validPassword,LDAP_GC_PORT,False)" +
                    "\'");
            temp183 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S799\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp183, "return of SimpleBind, state S799");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S268
        // Mapping a SPN to multiple objects is not allowed on Windows 2012R2.
        // This test case is doing so.
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S268()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S268");
            this.Manager.Comment("reaching state \'S268\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S269\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S578\'");
            ProtocolMessageStructures.errorstatus temp184;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,validPassword,LDAP_GC_P" +
                    "ORT,False)\'");
            temp184 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S800\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp184, "return of SimpleBind, state S800");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S270
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S270()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S270");
            this.Manager.Comment("reaching state \'S270\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S271\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S579\'");
            ProtocolMessageStructures.errorstatus temp185;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,invalidPassword,LDAP_GC" +
                    "_PORT,False)\'");
            temp185 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S801\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp185, "return of SimpleBind, state S801");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S272
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S272()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S272");
            this.Manager.Comment("reaching state \'S272\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S273\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S580\'");
            ProtocolMessageStructures.errorstatus temp186;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,invalidPassword,LDAP_GC_PORT,True)\'" +
                    "");
            temp186 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S802\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp186, "return of SimpleBind, state S802");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S274
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S274()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S274");
            this.Manager.Comment("reaching state \'S274\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S275\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S581\'");
            ProtocolMessageStructures.errorstatus temp187;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,invalidPassword,LDAP_GC" +
                    "_PORT,True)\'");
            temp187 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S803\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp187, "return of SimpleBind, state S803");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S276
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S276()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S276");
            this.Manager.Comment("reaching state \'S276\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S277\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S582\'");
            ProtocolMessageStructures.errorstatus temp188;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,invalidPassword,LDAP_GC_PORT,Fals" +
                    "e)\'");
            temp188 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S804\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp188, "return of SimpleBind, state S804");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S278
        // Mapping a SPN to multiple objects is not allowed on Windows 2012R2.
        // This test case is doing so.
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S278()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S278");
            this.Manager.Comment("reaching state \'S278\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S279\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S583\'");
            ProtocolMessageStructures.errorstatus temp189;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,validPassword,LDAP_GC_P" +
                    "ORT,True)\'");
            temp189 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S805\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp189, "return of SimpleBind, state S805");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S28()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S458\'");
            ProtocolMessageStructures.errorstatus temp190;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp190 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S680\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp190, "return of SimpleBind, state S680");
            this.Manager.Comment("reaching state \'S902\'");
            ProtocolMessageStructures.errorstatus temp191;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,nTSecurityDesc" +
                    "riptor,False)\'");
            temp191 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Comment("reaching state \'S1015\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp191, "return of AuthorizationCheck, state S1015");
            TCSimpleAuth_2K8R2S1115();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S280
        // Mapping a SPN to multiple objects is not allowed on Windows 2012R2.
        // This test case is doing so.
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S280()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S280");
            this.Manager.Comment("reaching state \'S280\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S281\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S584\'");
            ProtocolMessageStructures.errorstatus temp192;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,validPassword,LDAP_PORT" +
                    ",True)\'");
            temp192 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S806\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp192, "return of SimpleBind, state S806");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S282
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S282()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S282");
            this.Manager.Comment("reaching state \'S282\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S283\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S585\'");
            ProtocolMessageStructures.errorstatus temp193;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,invalidPassword,LDAP_PO" +
                    "RT,True)\'");
            temp193 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S807\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp193, "return of SimpleBind, state S807");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S284
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S284()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S284");
            this.Manager.Comment("reaching state \'S284\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S285\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S586\'");
            ProtocolMessageStructures.errorstatus temp194;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,invalidPassword,LDAP_PORT,True)\'");
            temp194 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S808\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp194, "return of SimpleBind, state S808");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S286
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S286()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S286");
            this.Manager.Comment("reaching state \'S286\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S287\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S587\'");
            ProtocolMessageStructures.errorstatus temp195;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,invalidPassword,LDAP_SSL_GC_PORT," +
                    "True)\'");
            temp195 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S809\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp195, "return of SimpleBind, state S809");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S288
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S288()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S288");
            this.Manager.Comment("reaching state \'S288\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S289\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S588\'");
            ProtocolMessageStructures.errorstatus temp196;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,invalidPassword,LDAP_SSL_GC_PORT,Tr" +
                    "ue)\'");
            temp196 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S810\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp196, "return of SimpleBind, state S810");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S290
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S290()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S290");
            this.Manager.Comment("reaching state \'S290\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S291\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S589\'");
            ProtocolMessageStructures.errorstatus temp197;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,invalidPassword,LDAP_SSL_GC_PORT,Tr" +
                    "ue)\'");
            temp197 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S811\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp197, "return of SimpleBind, state S811");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S292
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S292()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S292");
            this.Manager.Comment("reaching state \'S292\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S293\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S590\'");
            ProtocolMessageStructures.errorstatus temp198;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,invalidPassword,LDAP_SS" +
                    "L_GC_PORT,True)\'");
            temp198 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S812\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp198, "return of SimpleBind, state S812");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S294
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S294()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S294");
            this.Manager.Comment("reaching state \'S294\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S295\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S591\'");
            ProtocolMessageStructures.errorstatus temp199;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,validPassword,LDAP_SSL_GC_PORT,True" +
                    ")\'");
            temp199 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S813\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp199, "return of SimpleBind, state S813");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S296
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S296()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S296");
            this.Manager.Comment("reaching state \'S296\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S297\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S592\'");
            ProtocolMessageStructures.errorstatus temp200;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,validPassword,LDAP_SSL_PORT,True)\'");
            temp200 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S814\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp200, "return of SimpleBind, state S814");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S298
        // Mapping a SPN to multiple objects is not allowed on Windows 2012R2.
        // This test case is doing so.
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S298()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S298");
            this.Manager.Comment("reaching state \'S298\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S299\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S593\'");
            ProtocolMessageStructures.errorstatus temp201;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,validPassword,LDAP_SSL_" +
                    "PORT,True)\'");
            temp201 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S815\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp201, "return of SimpleBind, state S815");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S30()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S459\'");
            ProtocolMessageStructures.errorstatus temp202;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp202 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S681\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp202, "return of SimpleBind, state S681");
            this.Manager.Comment("reaching state \'S903\'");
            ProtocolMessageStructures.errorstatus temp203;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,nTSecurityDescriptor,False)" +
                    "\'");
            temp203 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Comment("reaching state \'S1016\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp203, "return of AuthorizationCheck, state S1016");
            TCSimpleAuth_2K8R2S1115();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S300
        // Mapping a SPN to multiple objects is not allowed on Windows 2012R2.
        // This test case is doing so.
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S300()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S300");
            this.Manager.Comment("reaching state \'S300\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S301\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S594\'");
            ProtocolMessageStructures.errorstatus temp204;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,validPassword,LDAP_SSL_" +
                    "GC_PORT,True)\'");
            temp204 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S816\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp204, "return of SimpleBind, state S816");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S302
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S302()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S302");
            this.Manager.Comment("reaching state \'S302\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S303\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S595\'");
            ProtocolMessageStructures.errorstatus temp205;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,invalidPassword,LDAP_SS" +
                    "L_PORT,True)\'");
            temp205 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S817\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp205, "return of SimpleBind, state S817");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S304
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S304()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S304");
            this.Manager.Comment("reaching state \'S304\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S305\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S596\'");
            ProtocolMessageStructures.errorstatus temp206;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,invalidPassword,LDAP_SSL_PORT,Tru" +
                    "e)\'");
            temp206 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S818\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp206, "return of SimpleBind, state S818");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S306
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S306()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S306");
            this.Manager.Comment("reaching state \'S306\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S307\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S597\'");
            ProtocolMessageStructures.errorstatus temp207;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,validPassword,LDAP_SSL_PORT,True)" +
                    "\'");
            temp207 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S819\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp207, "return of SimpleBind, state S819");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S308
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S308()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S308");
            this.Manager.Comment("reaching state \'S308\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S309\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S598\'");
            ProtocolMessageStructures.errorstatus temp208;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_PORT,True)\'");
            temp208 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S820\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp208, "return of SimpleBind, state S820");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S310
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S310()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S310");
            this.Manager.Comment("reaching state \'S310\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S311\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S599\'");
            ProtocolMessageStructures.errorstatus temp209;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,invalidPassword,LDAP_SSL_PORT,True)" +
                    "\'");
            temp209 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S821\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp209, "return of SimpleBind, state S821");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S312
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S312()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S312");
            this.Manager.Comment("reaching state \'S312\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S313\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S600\'");
            ProtocolMessageStructures.errorstatus temp210;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,invalidPassword,LDAP_SSL_PORT,True)" +
                    "\'");
            temp210 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S822\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp210, "return of SimpleBind, state S822");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S314
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S314()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S314");
            this.Manager.Comment("reaching state \'S314\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S315\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S601\'");
            ProtocolMessageStructures.errorstatus temp211;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,validPassword,LDAP_SSL_GC_PORT,Tr" +
                    "ue)\'");
            temp211 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S823\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp211, "return of SimpleBind, state S823");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S316
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S316()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S316");
            this.Manager.Comment("reaching state \'S316\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S317\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S602\'");
            ProtocolMessageStructures.errorstatus temp212;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp212 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S824\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp212, "return of SimpleBind, state S824");
            TCSimpleAuth_2K8R2S993();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S993()
        {
            this.Manager.Comment("reaching state \'S993\'");
        }
        #endregion

        #region Test Starting in S318
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S318()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S318");
            this.Manager.Comment("reaching state \'S318\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S319\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S603\'");
            ProtocolMessageStructures.errorstatus temp213;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,invalidPassword,LDAP_SSL_GC_PORT,Fa" +
                    "lse)\'");
            temp213 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S825\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp213, "return of SimpleBind, state S825");
            TCSimpleAuth_2K8R2S993();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S32()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S460\'");
            ProtocolMessageStructures.errorstatus temp214;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp214 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S682\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp214, "return of SimpleBind, state S682");
            this.Manager.Comment("reaching state \'S904\'");
            ProtocolMessageStructures.errorstatus temp215;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,msDS_QuotaUsed" +
                    ",False)\'");
            temp215 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1017\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp215, "return of AuthorizationCheck, state S1017");
            TCSimpleAuth_2K8R2S1115();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S320
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S320()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S320");
            this.Manager.Comment("reaching state \'S320\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S321\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S604\'");
            ProtocolMessageStructures.errorstatus temp216;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,validPassword,LDAP_PORT,True)\'");
            temp216 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S826\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp216, "return of SimpleBind, state S826");
            TCSimpleAuth_2K8R2S994();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S994()
        {
            this.Manager.Comment("reaching state \'S994\'");
        }
        #endregion

        #region Test Starting in S322
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S322()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S322");
            this.Manager.Comment("reaching state \'S322\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S323\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S605\'");
            ProtocolMessageStructures.errorstatus temp217;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,validPassword,LDAP_SSL_PORT,False)\'" +
                    "");
            temp217 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S827\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp217, "return of SimpleBind, state S827");
            TCSimpleAuth_2K8R2S993();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S324
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S324()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S324");
            this.Manager.Comment("reaching state \'S324\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S325\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S606\'");
            ProtocolMessageStructures.errorstatus temp218;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,True)\'");
            temp218 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S828\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp218, "return of SimpleBind, state S828");
            this.Manager.Comment("reaching state \'S995\'");
            ProtocolMessageStructures.errorstatus temp219;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,NotSet,msDS_QuotaE" +
                    "ffective,False)\'");
            temp219 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1102\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp219, "return of AuthorizationCheck, state S1102");
            TCSimpleAuth_2K8R2S1115();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S326
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S326()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S326");
            this.Manager.Comment("reaching state \'S326\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S327\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S607\'");
            ProtocolMessageStructures.errorstatus temp220;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,validPassword,LDAP_PORT,False)\'");
            temp220 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S829\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp220, "return of SimpleBind, state S829");
            TCSimpleAuth_2K8R2S996();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S996()
        {
            this.Manager.Comment("reaching state \'S996\'");
        }
        #endregion

        #region Test Starting in S328
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S328()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S328");
            this.Manager.Comment("reaching state \'S328\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S329\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S608\'");
            ProtocolMessageStructures.errorstatus temp221;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,validPassword,LDAP_GC_PORT,False)\'");
            temp221 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S830\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp221, "return of SimpleBind, state S830");
            TCSimpleAuth_2K8R2S996();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S330
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S330()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S330");
            this.Manager.Comment("reaching state \'S330\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S331\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S609\'");
            ProtocolMessageStructures.errorstatus temp222;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_PORT,False)\'" +
                    "");
            temp222 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S831\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp222, "return of SimpleBind, state S831");
            this.Manager.Comment("reaching state \'S997\'");
            ProtocolMessageStructures.errorstatus temp223;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_QuotaEffective,False)\'" +
                    "");
            temp223 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1103\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp223, "return of AuthorizationCheck, state S1103");
            TCSimpleAuth_2K8R2S1148();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S332
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S332()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S332");
            this.Manager.Comment("reaching state \'S332\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S333\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S610\'");
            ProtocolMessageStructures.errorstatus temp224;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp224 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S832\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp224, "return of SimpleBind, state S832");
            this.Manager.Comment("reaching state \'S998\'");
            ProtocolMessageStructures.errorstatus temp225;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,userPassword,False)\'");
            temp225 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S1104\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp225, "return of AuthorizationCheck, state S1104");
            TCSimpleAuth_2K8R2S1115();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S334
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S334()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S334");
            this.Manager.Comment("reaching state \'S334\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S335\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S611\'");
            ProtocolMessageStructures.errorstatus temp226;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,False)\'");
            temp226 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S833\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp226, "return of SimpleBind, state S833");
            this.Manager.Comment("reaching state \'S999\'");
            ProtocolMessageStructures.errorstatus temp227;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,userPassword,False)\'");
            temp227 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S1105\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp227, "return of AuthorizationCheck, state S1105");
            TCSimpleAuth_2K8R2S1152();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S336
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S336()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S336");
            this.Manager.Comment("reaching state \'S336\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S337\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S612\'");
            ProtocolMessageStructures.errorstatus temp228;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,validPassword,LDAP_GC_PORT,True)\'");
            temp228 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S834\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp228, "return of SimpleBind, state S834");
            TCSimpleAuth_2K8R2S994();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S338
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S338()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S338");
            this.Manager.Comment("reaching state \'S338\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S339\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S613\'");
            ProtocolMessageStructures.errorstatus temp229;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,invalidPassword,LDAP_GC_PORT,True)\'" +
                    "");
            temp229 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S835\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp229, "return of SimpleBind, state S835");
            TCSimpleAuth_2K8R2S994();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S34()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S461\'");
            ProtocolMessageStructures.errorstatus temp230;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp230 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S683\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp230, "return of SimpleBind, state S683");
            this.Manager.Comment("reaching state \'S905\'");
            ProtocolMessageStructures.errorstatus temp231;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_QuotaUsed,False)\'");
            temp231 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1018\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp231, "return of AuthorizationCheck, state S1018");
            TCSimpleAuth_2K8R2S1115();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S340
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S340()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S340");
            this.Manager.Comment("reaching state \'S340\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S341\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S614\'");
            ProtocolMessageStructures.errorstatus temp232;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,invalidPassword,LDAP_PORT,True)\'");
            temp232 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S836\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp232, "return of SimpleBind, state S836");
            TCSimpleAuth_2K8R2S994();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S342
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S342()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S342");
            this.Manager.Comment("reaching state \'S342\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S343\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S615\'");
            ProtocolMessageStructures.errorstatus temp233;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,invalidPassword,LDAP_PORT,False)\'");
            temp233 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S837\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp233, "return of SimpleBind, state S837");
            TCSimpleAuth_2K8R2S996();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S344
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S344()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S344");
            this.Manager.Comment("reaching state \'S344\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S345\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S616\'");
            ProtocolMessageStructures.errorstatus temp234;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,invalidPassword,LDAP_GC_PORT,False)" +
                    "\'");
            temp234 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S838\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp234, "return of SimpleBind, state S838");
            TCSimpleAuth_2K8R2S996();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S346
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S346()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S346");
            this.Manager.Comment("reaching state \'S346\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S347\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S617\'");
            ProtocolMessageStructures.errorstatus temp235;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp235 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S839\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp235, "return of SimpleBind, state S839");
            this.Manager.Comment("reaching state \'S1000\'");
            ProtocolMessageStructures.errorstatus temp236;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp236 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1106\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp236, "return of AuthorizationCheck, state S1106");
            this.Manager.Comment("reaching state \'S1158\'");
            ProtocolMessageStructures.errorstatus temp237;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",True)\'" +
                    "");
            temp237 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1204\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp237, "return of PasswordChangeOperation, state S1204");
            TCSimpleAuth_2K8R2S1214();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S348
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S348()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S348");
            this.Manager.Comment("reaching state \'S348\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S349\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S618\'");
            ProtocolMessageStructures.errorstatus temp238;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp238 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S840\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp238, "return of SimpleBind, state S840");
            this.Manager.Comment("reaching state \'S1001\'");
            ProtocolMessageStructures.errorstatus temp239;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp239 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1107\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp239, "return of AuthorizationCheck, state S1107");
            this.Manager.Comment("reaching state \'S1159\'");
            ProtocolMessageStructures.errorstatus temp240;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",False)" +
                    "\'");
            temp240 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1205\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp240, "return of PasswordChangeOperation, state S1205");
            TCSimpleAuth_2K8R2S1216();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S350
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S350()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S350");
            this.Manager.Comment("reaching state \'S350\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S351\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S619\'");
            ProtocolMessageStructures.errorstatus temp241;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,invalidPassword,LDAP_SSL_PORT,False" +
                    ")\'");
            temp241 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S841\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp241, "return of SimpleBind, state S841");
            TCSimpleAuth_2K8R2S993();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S352
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S352()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S352");
            this.Manager.Comment("reaching state \'S352\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S353\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S620\'");
            ProtocolMessageStructures.errorstatus temp242;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,invalidPassword,LDAP_GC_PORT,True" +
                    ")\'");
            temp242 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S842\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp242, "return of SimpleBind, state S842");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S354
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S354()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S354");
            this.Manager.Comment("reaching state \'S354\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S355\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S621\'");
            ProtocolMessageStructures.errorstatus temp243;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,validPassword,LDAP_GC_PORT,True)\'" +
                    "");
            temp243 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S843\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp243, "return of SimpleBind, state S843");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S356
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S356()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S356");
            this.Manager.Comment("reaching state \'S356\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S357\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S622\'");
            ProtocolMessageStructures.errorstatus temp244;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,validPassword,LDAP_PORT,True)\'");
            temp244 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S844\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp244, "return of SimpleBind, state S844");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S358
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S358()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S358");
            this.Manager.Comment("reaching state \'S358\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S359\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S623\'");
            ProtocolMessageStructures.errorstatus temp245;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,invalidPassword,LDAP_PORT,True)\'");
            temp245 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S845\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp245, "return of SimpleBind, state S845");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S36()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S462\'");
            ProtocolMessageStructures.errorstatus temp246;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp246 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S684\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp246, "return of SimpleBind, state S684");
            this.Manager.Comment("reaching state \'S906\'");
            ProtocolMessageStructures.errorstatus temp247;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,User_Change_Password,userPassword," +
                    "False)\'");
            temp247 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S1019\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp247, "return of AuthorizationCheck, state S1019");
            TCSimpleAuth_2K8R2S1115();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S360
        // Mapping a SPN to multiple objects is not allowed on Windows 2012R2.
        // This test case is doing so.
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S360()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S360");
            this.Manager.Comment("reaching state \'S360\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S361\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S624\'");
            ProtocolMessageStructures.errorstatus temp248;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,validPassword,LDAP_SSL_" +
                    "GC_PORT,False)\'");
            temp248 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S846\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp248, "return of SimpleBind, state S846");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S362
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S362()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S362");
            this.Manager.Comment("reaching state \'S362\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S363\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S625\'");
            ProtocolMessageStructures.errorstatus temp249;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,validPassword,LDAP_SSL_GC_PORT,Fa" +
                    "lse)\'");
            temp249 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S847\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp249, "return of SimpleBind, state S847");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S364
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S364()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S364");
            this.Manager.Comment("reaching state \'S364\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S365\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S626\'");
            ProtocolMessageStructures.errorstatus temp250;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,invalidPassword,LDAP_SS" +
                    "L_GC_PORT,False)\'");
            temp250 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S848\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp250, "return of SimpleBind, state S848");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S366
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S366()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S366");
            this.Manager.Comment("reaching state \'S366\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S367\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S627\'");
            ProtocolMessageStructures.errorstatus temp251;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,invalidPassword,LDAP_SS" +
                    "L_PORT,False)\'");
            temp251 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S849\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp251, "return of SimpleBind, state S849");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S368
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S368()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S368");
            this.Manager.Comment("reaching state \'S368\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S369\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S628\'");
            ProtocolMessageStructures.errorstatus temp252;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,invalidPassword,LDAP_SSL_GC_PORT,Fa" +
                    "lse)\'");
            temp252 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Comment("reaching state \'S850\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp252, "return of SimpleBind, state S850");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S370
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S370()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S370");
            this.Manager.Comment("reaching state \'S370\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S371\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S629\'");
            ProtocolMessageStructures.errorstatus temp253;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,invalidPassword,LDAP_SSL_GC_PORT," +
                    "False)\'");
            temp253 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S851\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp253, "return of SimpleBind, state S851");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S372
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S372()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S372");
            this.Manager.Comment("reaching state \'S372\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S373\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S630\'");
            ProtocolMessageStructures.errorstatus temp254;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,invalidPassword,LDAP_SSL_PORT,False" +
                    ")\'");
            temp254 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Comment("reaching state \'S852\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp254, "return of SimpleBind, state S852");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S374
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S374()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S374");
            this.Manager.Comment("reaching state \'S374\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S375\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S631\'");
            ProtocolMessageStructures.errorstatus temp255;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,invalidPassword,LDAP_SSL_PORT,Fal" +
                    "se)\'");
            temp255 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S853\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp255, "return of SimpleBind, state S853");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S376
        // Mapping a SPN to multiple objects is not allowed on Windows 2012R2.
        // This test case is doing so.
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S376()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S376");
            this.Manager.Comment("reaching state \'S376\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S377\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S632\'");
            ProtocolMessageStructures.errorstatus temp256;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,validPassword,LDAP_SSL_" +
                    "PORT,False)\'");
            temp256 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S854\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp256, "return of SimpleBind, state S854");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S378
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S378()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S378");
            this.Manager.Comment("reaching state \'S378\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S379\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S633\'");
            ProtocolMessageStructures.errorstatus temp257;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,validPassword,LDAP_SSL_PORT,False" +
                    ")\'");
            temp257 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S855\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp257, "return of SimpleBind, state S855");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S38()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S463\'");
            ProtocolMessageStructures.errorstatus temp258;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp258 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S685\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp258, "return of SimpleBind, state S685");
            this.Manager.Comment("reaching state \'S907\'");
            ProtocolMessageStructures.errorstatus temp259;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,msDS_QuotaEffe" +
                    "ctive,False)\'");
            temp259 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1020\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp259, "return of AuthorizationCheck, state S1020");
            TCSimpleAuth_2K8R2S1115();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S380
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S380()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S380");
            this.Manager.Comment("reaching state \'S380\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S381\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S634\'");
            ProtocolMessageStructures.errorstatus temp260;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,invalidPassword,LDAP_PORT,False)\'");
            temp260 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Comment("reaching state \'S856\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp260, "return of SimpleBind, state S856");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S382
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S382()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S382");
            this.Manager.Comment("reaching state \'S382\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S383\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S635\'");
            ProtocolMessageStructures.errorstatus temp261;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,invalidPassword,LDAP_PO" +
                    "RT,False)\'");
            temp261 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S857\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp261, "return of SimpleBind, state S857");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S384
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S384()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S384");
            this.Manager.Comment("reaching state \'S384\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S385\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S636\'");
            ProtocolMessageStructures.errorstatus temp262;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,invalidPassword,LDAP_PORT,False)\'" +
                    "");
            temp262 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S858\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp262, "return of SimpleBind, state S858");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S386
        // Mapping a SPN to multiple objects is not allowed on Windows 2012R2.
        // This test case is doing so.
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S386()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S386");
            this.Manager.Comment("reaching state \'S386\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S387\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S637\'");
            ProtocolMessageStructures.errorstatus temp263;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,validPassword,LDAP_PORT" +
                    ",False)\'");
            temp263 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S859\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp263, "return of SimpleBind, state S859");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S388
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S388()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S388");
            this.Manager.Comment("reaching state \'S388\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S389\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S638\'");
            ProtocolMessageStructures.errorstatus temp264;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,validPassword,LDAP_PORT,False)\'");
            temp264 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S860\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp264, "return of SimpleBind, state S860");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S390
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S390()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S390");
            this.Manager.Comment("reaching state \'S390\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S391\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S639\'");
            ProtocolMessageStructures.errorstatus temp265;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,invalidPassword,LDAP_GC_PORT,False)" +
                    "\'");
            temp265 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Comment("reaching state \'S861\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp265, "return of SimpleBind, state S861");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S392
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S392()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S392");
            this.Manager.Comment("reaching state \'S392\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S393\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S640\'");
            ProtocolMessageStructures.errorstatus temp266;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,validPassword,LDAP_GC_PORT,False)" +
                    "\'");
            temp266 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S862\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp266, "return of SimpleBind, state S862");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S394
        // Mapping a SPN to multiple objects is not allowed on Windows 2012R2.
        // This test case is doing so.
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S394()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S394");
            this.Manager.Comment("reaching state \'S394\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S395\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S641\'");
            ProtocolMessageStructures.errorstatus temp267;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,validPassword,LDAP_GC_P" +
                    "ORT,False)\'");
            temp267 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S863\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp267, "return of SimpleBind, state S863");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S396
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S396()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S396");
            this.Manager.Comment("reaching state \'S396\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S397\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S642\'");
            ProtocolMessageStructures.errorstatus temp268;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,invalidPassword,LDAP_GC" +
                    "_PORT,False)\'");
            temp268 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S864\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp268, "return of SimpleBind, state S864");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S398
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S398()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S398");
            this.Manager.Comment("reaching state \'S398\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S399\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S643\'");
            ProtocolMessageStructures.errorstatus temp269;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,invalidPassword,LDAP_GC_PORT,True)\'" +
                    "");
            temp269 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Comment("reaching state \'S865\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp269, "return of SimpleBind, state S865");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S4()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S446\'");
            ProtocolMessageStructures.errorstatus temp270;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp270 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S668\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp270, "return of SimpleBind, state S668");
            this.Manager.Comment("reaching state \'S890\'");
            ProtocolMessageStructures.errorstatus temp271;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp271 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1003\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp271, "return of AuthorizationCheck, state S1003");
            this.Manager.Comment("reaching state \'S1109\'");
            ProtocolMessageStructures.errorstatus temp272;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",True)\'");
            temp272 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", true);
            this.Manager.Checkpoint("MS-ADTS-Security_R35");
            this.Manager.Checkpoint("MS-ADTS-Security_R486");
            this.Manager.Comment("reaching state \'S1161\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp272, "return of PasswordChangeOperation, state S1161");
            TCSimpleAuth_2K8R2S1206();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S40
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S40()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S464\'");
            ProtocolMessageStructures.errorstatus temp273;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp273 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S686\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp273, "return of SimpleBind, state S686");
            this.Manager.Comment("reaching state \'S908\'");
            ProtocolMessageStructures.errorstatus temp274;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_QuotaEffective,False)\'" +
                    "");
            temp274 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1021\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp274, "return of AuthorizationCheck, state S1021");
            TCSimpleAuth_2K8R2S1115();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S400
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S400()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S400");
            this.Manager.Comment("reaching state \'S400\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S401\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S644\'");
            ProtocolMessageStructures.errorstatus temp275;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,invalidPassword,LDAP_GC" +
                    "_PORT,True)\'");
            temp275 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S866\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp275, "return of SimpleBind, state S866");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S402
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S402()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S402");
            this.Manager.Comment("reaching state \'S402\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S403\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S645\'");
            ProtocolMessageStructures.errorstatus temp276;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,invalidPassword,LDAP_GC_PORT,Fals" +
                    "e)\'");
            temp276 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R10159");
            this.Manager.Comment("reaching state \'S867\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp276, "return of SimpleBind, state S867");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S404
        // Mapping a SPN to multiple objects is not allowed on Windows 2012R2.
        // This test case is doing so.
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S404()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S404");
            this.Manager.Comment("reaching state \'S404\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S405\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S646\'");
            ProtocolMessageStructures.errorstatus temp277;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,validPassword,LDAP_GC_P" +
                    "ORT,True)\'");
            temp277 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S868\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp277, "return of SimpleBind, state S868");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S406
        // Mapping a SPN to multiple objects is not allowed on Windows 2012R2.
        // This test case is doing so.
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S406()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S406");
            this.Manager.Comment("reaching state \'S406\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S407\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S647\'");
            ProtocolMessageStructures.errorstatus temp278;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,validPassword,LDAP_PORT" +
                    ",True)\'");
            temp278 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S869\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp278, "return of SimpleBind, state S869");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S408
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S408()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S408");
            this.Manager.Comment("reaching state \'S408\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S409\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S648\'");
            ProtocolMessageStructures.errorstatus temp279;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,invalidPassword,LDAP_PO" +
                    "RT,True)\'");
            temp279 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R158");
            this.Manager.Comment("reaching state \'S870\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp279, "return of SimpleBind, state S870");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S410
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S410()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S410");
            this.Manager.Comment("reaching state \'S410\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S411\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S649\'");
            ProtocolMessageStructures.errorstatus temp280;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,invalidPassword,LDAP_PORT,True)\'");
            temp280 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Comment("reaching state \'S871\'");
            this.Manager.Comment("checking step \'return SimpleBind/invalidCredentials\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.invalidCredentials, temp280, "return of SimpleBind, state S871");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S412
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S412()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S412");
            this.Manager.Comment("reaching state \'S412\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S413\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S650\'");
            ProtocolMessageStructures.errorstatus temp281;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,invalidPassword,LDAP_SSL_GC_PORT," +
                    "True)\'");
            temp281 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S872\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp281, "return of SimpleBind, state S872");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S414
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S414()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S414");
            this.Manager.Comment("reaching state \'S414\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S415\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S651\'");
            ProtocolMessageStructures.errorstatus temp282;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,invalidPassword,LDAP_SSL_GC_PORT,Tr" +
                    "ue)\'");
            temp282 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S873\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp282, "return of SimpleBind, state S873");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S416
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S416()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S416");
            this.Manager.Comment("reaching state \'S416\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S417\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S652\'");
            ProtocolMessageStructures.errorstatus temp283;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,invalidPassword,LDAP_SSL_GC_PORT,Tr" +
                    "ue)\'");
            temp283 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S874\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp283, "return of SimpleBind, state S874");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S418
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S418()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S418");
            this.Manager.Comment("reaching state \'S418\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S419\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S653\'");
            ProtocolMessageStructures.errorstatus temp284;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,invalidPassword,LDAP_SS" +
                    "L_GC_PORT,True)\'");
            temp284 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S875\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp284, "return of SimpleBind, state S875");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S42
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S42()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S465\'");
            ProtocolMessageStructures.errorstatus temp285;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp285 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S687\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp285, "return of SimpleBind, state S687");
            this.Manager.Comment("reaching state \'S909\'");
            ProtocolMessageStructures.errorstatus temp286;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,NotSet,msDS_QuotaU" +
                    "sed,False)\'");
            temp286 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1022\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp286, "return of AuthorizationCheck, state S1022");
            TCSimpleAuth_2K8R2S1121();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S420
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S420()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S420");
            this.Manager.Comment("reaching state \'S420\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S421\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S654\'");
            ProtocolMessageStructures.errorstatus temp287;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,validPassword,LDAP_SSL_GC_PORT,True" +
                    ")\'");
            temp287 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S876\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp287, "return of SimpleBind, state S876");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S422
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S422()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S422");
            this.Manager.Comment("reaching state \'S422\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S423\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S655\'");
            ProtocolMessageStructures.errorstatus temp288;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,validPassword,LDAP_SSL_PORT,True)\'");
            temp288 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S877\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp288, "return of SimpleBind, state S877");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S424
        // Mapping a SPN to multiple objects is not allowed on Windows 2012R2.
        // This test case is doing so.
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S424()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S424");
            this.Manager.Comment("reaching state \'S424\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S425\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S656\'");
            ProtocolMessageStructures.errorstatus temp289;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,validPassword,LDAP_SSL_" +
                    "PORT,True)\'");
            temp289 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S878\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp289, "return of SimpleBind, state S878");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S426
        // Mapping a SPN to multiple objects is not allowed on Windows 2012R2.
        // This test case is doing so.
        [Microsoft.VisualStudio.TestTools.UnitTesting.Ignore]
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S426()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S426");
            this.Manager.Comment("reaching state \'S426\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S427\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S657\'");
            ProtocolMessageStructures.errorstatus temp290;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,validPassword,LDAP_SSL_" +
                    "GC_PORT,True)\'");
            temp290 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S879\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp290, "return of SimpleBind, state S879");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S428
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S428()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S428");
            this.Manager.Comment("reaching state \'S428\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S429\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S658\'");
            ProtocolMessageStructures.errorstatus temp291;
            this.Manager.Comment("executing step \'call SimpleBind(nameMapsMoreThanOneObject,invalidPassword,LDAP_SS" +
                    "L_PORT,True)\'");
            temp291 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.nameMapsMoreThanOneObject, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S880\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp291, "return of SimpleBind, state S880");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S430
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S430()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S430");
            this.Manager.Comment("reaching state \'S430\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S431\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S659\'");
            ProtocolMessageStructures.errorstatus temp292;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,invalidPassword,LDAP_SSL_PORT,Tru" +
                    "e)\'");
            temp292 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S881\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp292, "return of SimpleBind, state S881");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S432
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S432()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S432");
            this.Manager.Comment("reaching state \'S432\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S433\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S660\'");
            ProtocolMessageStructures.errorstatus temp293;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,validPassword,LDAP_SSL_PORT,True)" +
                    "\'");
            temp293 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S882\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp293, "return of SimpleBind, state S882");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S434
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S434()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S434");
            this.Manager.Comment("reaching state \'S434\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S435\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S661\'");
            ProtocolMessageStructures.errorstatus temp294;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_PORT,True)\'");
            temp294 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S883\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp294, "return of SimpleBind, state S883");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S436
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S436()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S436");
            this.Manager.Comment("reaching state \'S436\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S437\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S662\'");
            ProtocolMessageStructures.errorstatus temp295;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,invalidPassword,LDAP_SSL_PORT,True)" +
                    "\'");
            temp295 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S884\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp295, "return of SimpleBind, state S884");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S438
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S438()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S438");
            this.Manager.Comment("reaching state \'S438\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S439\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S663\'");
            ProtocolMessageStructures.errorstatus temp296;
            this.Manager.Comment("executing step \'call SimpleBind(anonymousUser,invalidPassword,LDAP_SSL_PORT,True)" +
                    "\'");
            temp296 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(ProtocolMessageStructures.name.anonymousUser, ((ProtocolMessageStructures.Password)(0)), ProtocolMessageStructures.Port.LDAP_SSL_PORT, true);
            this.Manager.Comment("reaching state \'S885\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp296, "return of SimpleBind, state S885");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S44
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S44()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S44");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S466\'");
            ProtocolMessageStructures.errorstatus temp297;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp297 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S688\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp297, "return of SimpleBind, state S688");
            this.Manager.Comment("reaching state \'S910\'");
            ProtocolMessageStructures.errorstatus temp298;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,NotSet,use" +
                    "rPassword,False)\'");
            temp298 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1023\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp298, "return of AuthorizationCheck, state S1023");
            this.Manager.Comment("reaching state \'S1122\'");
            ProtocolMessageStructures.errorstatus temp299;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",False)" +
                    "\'");
            temp299 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1172\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp299, "return of PasswordChangeOperation, state S1172");
            TCSimpleAuth_2K8R2S1210();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1210()
        {
            this.Manager.Comment("reaching state \'S1210\'");
        }
        #endregion

        #region Test Starting in S440
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S440()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S440");
            this.Manager.Comment("reaching state \'S440\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S441\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S664\'");
            ProtocolMessageStructures.errorstatus temp300;
            this.Manager.Comment("executing step \'call SimpleBind(invalidUserName,validPassword,LDAP_SSL_GC_PORT,Tr" +
                    "ue)\'");
            temp300 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S886\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp300, "return of SimpleBind, state S886");
            TCSimpleAuth_2K8R2S888();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S442
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S442()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S442");
            this.Manager.Comment("reaching state \'S442\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S443\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S665\'");
            ProtocolMessageStructures.errorstatus temp301;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,True" +
                    ")\'");
            temp301 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, true);
            this.Manager.Comment("reaching state \'S887\'");
            this.Manager.Comment("checking step \'return SimpleBind/unwillingToPerform\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.unwillingToPerform, temp301, "return of SimpleBind, state S887");
            TCSimpleAuth_2K8R2S992();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S46
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S46()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S46");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S467\'");
            ProtocolMessageStructures.errorstatus temp302;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp302 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S689\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp302, "return of SimpleBind, state S689");
            this.Manager.Comment("reaching state \'S911\'");
            ProtocolMessageStructures.errorstatus temp303;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,DS_Query_Self_Quot" +
                    "a,msDS_QuotaUsed,False)\'");
            temp303 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R45");
            this.Manager.Comment("reaching state \'S1024\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp303, "return of AuthorizationCheck, state S1024");
            this.Manager.Comment("reaching state \'S1123\'");
            ProtocolMessageStructures.errorstatus temp304;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",False)\'");
            temp304 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1173\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp304, "return of PasswordChangeOperation, state S1173");
            TCSimpleAuth_2K8R2S1210();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S48
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S48()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S48");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S468\'");
            ProtocolMessageStructures.errorstatus temp305;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp305 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S690\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp305, "return of SimpleBind, state S690");
            this.Manager.Comment("reaching state \'S912\'");
            ProtocolMessageStructures.errorstatus temp306;
            this.Manager.Comment("executing step \'call AuthorizationCheck(ACC_SYS_SEC_READ_CONTROL,NotSet,nTSecurit" +
                    "yDescriptor,False)\'");
            temp306 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.ACC_SYS_SEC_READ_CONTROL, ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R39");
            this.Manager.Comment("reaching state \'S1025\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp306, "return of AuthorizationCheck, state S1025");
            this.Manager.Comment("reaching state \'S1124\'");
            ProtocolMessageStructures.errorstatus temp307;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",True)\'" +
                    "");
            temp307 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1174\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp307, "return of PasswordChangeOperation, state S1174");
            TCSimpleAuth_2K8R2S1210();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S50
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S50()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S50");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S469\'");
            ProtocolMessageStructures.errorstatus temp308;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp308 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S691\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp308, "return of SimpleBind, state S691");
            this.Manager.Comment("reaching state \'S913\'");
            ProtocolMessageStructures.errorstatus temp309;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp309 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1026\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp309, "return of AuthorizationCheck, state S1026");
            this.Manager.Comment("reaching state \'S1125\'");
            ProtocolMessageStructures.errorstatus temp310;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",True)\'" +
                    "");
            temp310 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1175\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp310, "return of PasswordChangeOperation, state S1175");
            TCSimpleAuth_2K8R2S1208();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S52
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S52()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S52");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S470\'");
            ProtocolMessageStructures.errorstatus temp311;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp311 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S692\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp311, "return of SimpleBind, state S692");
            this.Manager.Comment("reaching state \'S914\'");
            ProtocolMessageStructures.errorstatus temp312;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,DS_Query_Self_Quot" +
                    "a,msDS_QuotaEffective,False)\'");
            temp312 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R41");
            this.Manager.Comment("reaching state \'S1027\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp312, "return of AuthorizationCheck, state S1027");
            this.Manager.Comment("reaching state \'S1126\'");
            ProtocolMessageStructures.errorstatus temp313;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",True)\'");
            temp313 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1176\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp313, "return of PasswordChangeOperation, state S1176");
            TCSimpleAuth_2K8R2S1210();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S54
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S54()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S54");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S471\'");
            ProtocolMessageStructures.errorstatus temp314;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp314 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S693\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp314, "return of SimpleBind, state S693");
            this.Manager.Comment("reaching state \'S915\'");
            ProtocolMessageStructures.errorstatus temp315;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,nTSecurityDescriptor,False)" +
                    "\'");
            temp315 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Comment("reaching state \'S1028\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp315, "return of AuthorizationCheck, state S1028");
            TCSimpleAuth_2K8R2S1121();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S56
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S56()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S56");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S472\'");
            ProtocolMessageStructures.errorstatus temp316;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp316 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S694\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp316, "return of SimpleBind, state S694");
            this.Manager.Comment("reaching state \'S916\'");
            ProtocolMessageStructures.errorstatus temp317;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,msDS_QuotaUsed" +
                    ",False)\'");
            temp317 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1029\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp317, "return of AuthorizationCheck, state S1029");
            TCSimpleAuth_2K8R2S1121();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S58
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S58()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S58");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S473\'");
            ProtocolMessageStructures.errorstatus temp318;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp318 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S695\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp318, "return of SimpleBind, state S695");
            this.Manager.Comment("reaching state \'S917\'");
            ProtocolMessageStructures.errorstatus temp319;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_QuotaUsed,False)\'");
            temp319 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1030\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp319, "return of AuthorizationCheck, state S1030");
            TCSimpleAuth_2K8R2S1121();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S6()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S447\'");
            ProtocolMessageStructures.errorstatus temp320;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp320 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S669\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp320, "return of SimpleBind, state S669");
            this.Manager.Comment("reaching state \'S891\'");
            ProtocolMessageStructures.errorstatus temp321;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp321 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1004\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp321, "return of AuthorizationCheck, state S1004");
            this.Manager.Comment("reaching state \'S1110\'");
            ProtocolMessageStructures.errorstatus temp322;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",False)" +
                    "\'");
            temp322 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1162\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp322, "return of PasswordChangeOperation, state S1162");
            TCSimpleAuth_2K8R2S1206();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S60
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S60()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S60");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S474\'");
            ProtocolMessageStructures.errorstatus temp323;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp323 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S696\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp323, "return of SimpleBind, state S696");
            this.Manager.Comment("reaching state \'S918\'");
            ProtocolMessageStructures.errorstatus temp324;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,User_Change_Password,userPassword," +
                    "False)\'");
            temp324 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S1031\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp324, "return of AuthorizationCheck, state S1031");
            TCSimpleAuth_2K8R2S1121();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S62
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S62()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S62");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S475\'");
            ProtocolMessageStructures.errorstatus temp325;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp325 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S697\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp325, "return of SimpleBind, state S697");
            this.Manager.Comment("reaching state \'S919\'");
            ProtocolMessageStructures.errorstatus temp326;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,msDS_QuotaEffe" +
                    "ctive,False)\'");
            temp326 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1032\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp326, "return of AuthorizationCheck, state S1032");
            TCSimpleAuth_2K8R2S1121();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S64
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S64()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S64");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S476\'");
            ProtocolMessageStructures.errorstatus temp327;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp327 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S698\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp327, "return of SimpleBind, state S698");
            this.Manager.Comment("reaching state \'S920\'");
            ProtocolMessageStructures.errorstatus temp328;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,NotSet,msDS_QuotaE" +
                    "ffective,False)\'");
            temp328 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1033\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp328, "return of AuthorizationCheck, state S1033");
            TCSimpleAuth_2K8R2S1121();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S66
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S66()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S66");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S477\'");
            ProtocolMessageStructures.errorstatus temp329;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_PORT,True)\'");
            temp329 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, true);
            this.Manager.Checkpoint("MS-ADTS-Security_R153");
            this.Manager.Comment("reaching state \'S699\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp329, "return of SimpleBind, state S699");
            this.Manager.Comment("reaching state \'S921\'");
            ProtocolMessageStructures.errorstatus temp330;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_QuotaEffective,False)\'" +
                    "");
            temp330 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1034\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp330, "return of AuthorizationCheck, state S1034");
            TCSimpleAuth_2K8R2S1121();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S68
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S68()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S68");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S478\'");
            ProtocolMessageStructures.errorstatus temp331;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp331 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S700\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp331, "return of SimpleBind, state S700");
            this.Manager.Comment("reaching state \'S922\'");
            ProtocolMessageStructures.errorstatus temp332;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,NotSet,msDS_QuotaU" +
                    "sed,False)\'");
            temp332 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1035\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp332, "return of AuthorizationCheck, state S1035");
            TCSimpleAuth_2K8R2S1127();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S70
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S70()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S70");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S479\'");
            ProtocolMessageStructures.errorstatus temp333;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp333 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S701\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp333, "return of SimpleBind, state S701");
            this.Manager.Comment("reaching state \'S923\'");
            ProtocolMessageStructures.errorstatus temp334;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,NotSet,use" +
                    "rPassword,False)\'");
            temp334 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1036\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp334, "return of AuthorizationCheck, state S1036");
            this.Manager.Comment("reaching state \'S1128\'");
            ProtocolMessageStructures.errorstatus temp335;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",False)" +
                    "\'");
            temp335 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1177\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp335, "return of PasswordChangeOperation, state S1177");
            TCSimpleAuth_2K8R2S1211();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1211()
        {
            this.Manager.Comment("reaching state \'S1211\'");
        }
        #endregion

        #region Test Starting in S72
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S72()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S72");
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S480\'");
            ProtocolMessageStructures.errorstatus temp336;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp336 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S702\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp336, "return of SimpleBind, state S702");
            this.Manager.Comment("reaching state \'S924\'");
            ProtocolMessageStructures.errorstatus temp337;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,DS_Query_Self_Quot" +
                    "a,msDS_QuotaUsed,False)\'");
            temp337 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R45");
            this.Manager.Comment("reaching state \'S1037\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp337, "return of AuthorizationCheck, state S1037");
            this.Manager.Comment("reaching state \'S1129\'");
            ProtocolMessageStructures.errorstatus temp338;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",False)\'");
            temp338 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1178\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp338, "return of PasswordChangeOperation, state S1178");
            TCSimpleAuth_2K8R2S1211();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S74
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S74()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S74");
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S481\'");
            ProtocolMessageStructures.errorstatus temp339;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp339 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S703\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp339, "return of SimpleBind, state S703");
            this.Manager.Comment("reaching state \'S925\'");
            ProtocolMessageStructures.errorstatus temp340;
            this.Manager.Comment("executing step \'call AuthorizationCheck(ACC_SYS_SEC_READ_CONTROL,NotSet,nTSecurit" +
                    "yDescriptor,False)\'");
            temp340 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.ACC_SYS_SEC_READ_CONTROL, ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R39");
            this.Manager.Comment("reaching state \'S1038\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp340, "return of AuthorizationCheck, state S1038");
            this.Manager.Comment("reaching state \'S1130\'");
            ProtocolMessageStructures.errorstatus temp341;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",True)\'" +
                    "");
            temp341 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1179\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp341, "return of PasswordChangeOperation, state S1179");
            TCSimpleAuth_2K8R2S1211();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S76
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S76()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S76");
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S482\'");
            ProtocolMessageStructures.errorstatus temp342;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp342 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S704\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp342, "return of SimpleBind, state S704");
            this.Manager.Comment("reaching state \'S926\'");
            ProtocolMessageStructures.errorstatus temp343;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp343 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1039\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp343, "return of AuthorizationCheck, state S1039");
            this.Manager.Comment("reaching state \'S1131\'");
            ProtocolMessageStructures.errorstatus temp344;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",True)\'" +
                    "");
            temp344 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1180\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp344, "return of PasswordChangeOperation, state S1180");
            TCSimpleAuth_2K8R2S1207();
            this.Manager.EndTest();
        }

        private void TCSimpleAuth_2K8R2S1207()
        {
            this.Manager.Comment("reaching state \'S1207\'");
        }
        #endregion

        #region Test Starting in S78
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S78()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S78");
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S483\'");
            ProtocolMessageStructures.errorstatus temp345;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp345 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S705\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp345, "return of SimpleBind, state S705");
            this.Manager.Comment("reaching state \'S927\'");
            ProtocolMessageStructures.errorstatus temp346;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,DS_Query_Self_Quot" +
                    "a,msDS_QuotaEffective,False)\'");
            temp346 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R41");
            this.Manager.Comment("reaching state \'S1040\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp346, "return of AuthorizationCheck, state S1040");
            this.Manager.Comment("reaching state \'S1132\'");
            ProtocolMessageStructures.errorstatus temp347;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",True)\'");
            temp347 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", true);
            this.Manager.Comment("reaching state \'S1181\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp347, "return of PasswordChangeOperation, state S1181");
            TCSimpleAuth_2K8R2S1211();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S8()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S448\'");
            ProtocolMessageStructures.errorstatus temp348;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp348 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S670\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp348, "return of SimpleBind, state S670");
            this.Manager.Comment("reaching state \'S892\'");
            ProtocolMessageStructures.errorstatus temp349;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp349 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1005\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp349, "return of AuthorizationCheck, state S1005");
            this.Manager.Comment("reaching state \'S1111\'");
            ProtocolMessageStructures.errorstatus temp350;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",False)" +
                    "\'");
            temp350 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1163\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp350, "return of PasswordChangeOperation, state S1163");
            TCSimpleAuth_2K8R2S1207();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S80
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S80()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S80");
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S484\'");
            ProtocolMessageStructures.errorstatus temp351;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp351 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S706\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp351, "return of SimpleBind, state S706");
            this.Manager.Comment("reaching state \'S928\'");
            ProtocolMessageStructures.errorstatus temp352;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,nTSecurityDesc" +
                    "riptor,False)\'");
            temp352 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Comment("reaching state \'S1041\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp352, "return of AuthorizationCheck, state S1041");
            TCSimpleAuth_2K8R2S1127();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S82
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S82()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S82");
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S485\'");
            ProtocolMessageStructures.errorstatus temp353;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp353 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S707\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp353, "return of SimpleBind, state S707");
            this.Manager.Comment("reaching state \'S929\'");
            ProtocolMessageStructures.errorstatus temp354;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,nTSecurityDescriptor,False)" +
                    "\'");
            temp354 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(0)), false);
            this.Manager.Comment("reaching state \'S1042\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp354, "return of AuthorizationCheck, state S1042");
            TCSimpleAuth_2K8R2S1127();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S84
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S84()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S84");
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S486\'");
            ProtocolMessageStructures.errorstatus temp355;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp355 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S708\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp355, "return of SimpleBind, state S708");
            this.Manager.Comment("reaching state \'S930\'");
            ProtocolMessageStructures.errorstatus temp356;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,msDS_QuotaUsed" +
                    ",False)\'");
            temp356 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1043\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp356, "return of AuthorizationCheck, state S1043");
            TCSimpleAuth_2K8R2S1127();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S86
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S86()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S86");
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S487\'");
            ProtocolMessageStructures.errorstatus temp357;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp357 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S709\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp357, "return of SimpleBind, state S709");
            this.Manager.Comment("reaching state \'S931\'");
            ProtocolMessageStructures.errorstatus temp358;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_QuotaUsed,False)\'");
            temp358 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_QuotaUsed, false);
            this.Manager.Comment("reaching state \'S1044\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp358, "return of AuthorizationCheck, state S1044");
            TCSimpleAuth_2K8R2S1127();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S88
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S88()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S88");
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S488\'");
            ProtocolMessageStructures.errorstatus temp359;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp359 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S710\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp359, "return of SimpleBind, state S710");
            this.Manager.Comment("reaching state \'S932\'");
            ProtocolMessageStructures.errorstatus temp360;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,User_Change_Password,userPassword," +
                    "False)\'");
            temp360 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Comment("reaching state \'S1045\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp360, "return of AuthorizationCheck, state S1045");
            TCSimpleAuth_2K8R2S1127();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S90
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S90()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S90");
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S489\'");
            ProtocolMessageStructures.errorstatus temp361;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp361 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S711\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp361, "return of SimpleBind, state S711");
            this.Manager.Comment("reaching state \'S933\'");
            ProtocolMessageStructures.errorstatus temp362;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Query_Self_Quota,msDS_QuotaEffe" +
                    "ctive,False)\'");
            temp362 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Query_Self_Quota, ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1046\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp362, "return of AuthorizationCheck, state S1046");
            TCSimpleAuth_2K8R2S1127();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S92
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S92()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S92");
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S490\'");
            ProtocolMessageStructures.errorstatus temp363;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_GC_PORT,False)\'");
            temp363 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S712\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp363, "return of SimpleBind, state S712");
            this.Manager.Comment("reaching state \'S934\'");
            ProtocolMessageStructures.errorstatus temp364;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY,NotSet,msDS_QuotaE" +
                    "ffective,False)\'");
            temp364 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(1)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ((ProtocolMessageStructures.AttribsToCheck)(1)), false);
            this.Manager.Comment("reaching state \'S1047\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp364, "return of AuthorizationCheck, state S1047");
            TCSimpleAuth_2K8R2S1127();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S94
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S94()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S94");
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S491\'");
            ProtocolMessageStructures.errorstatus temp365;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp365 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S713\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp365, "return of SimpleBind, state S713");
            this.Manager.Comment("reaching state \'S935\'");
            ProtocolMessageStructures.errorstatus temp366;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp366 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1048\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp366, "return of AuthorizationCheck, state S1048");
            this.Manager.Comment("reaching state \'S1133\'");
            ProtocolMessageStructures.errorstatus temp367;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",False)\'");
            temp367 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", false);
            this.Manager.Checkpoint("MS-ADTS-Security_R32");
            this.Manager.Checkpoint("MS-ADTS-Security_R35");
            this.Manager.Checkpoint("MS-ADTS-Security_R486");
            this.Manager.Comment("reaching state \'S1182\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp367, "return of PasswordChangeOperation, state S1182");
            TCSimpleAuth_2K8R2S1212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S96
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S96()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S96");
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S492\'");
            ProtocolMessageStructures.errorstatus temp368;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp368 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S714\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp368, "return of SimpleBind, state S714");
            this.Manager.Comment("reaching state \'S936\'");
            ProtocolMessageStructures.errorstatus temp369;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp369 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1049\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp369, "return of AuthorizationCheck, state S1049");
            this.Manager.Comment("reaching state \'S1134\'");
            ProtocolMessageStructures.errorstatus temp370;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(validPassword,\"PasswordNew\",True)\'");
            temp370 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(1)), "PasswordNew", true);
            this.Manager.Checkpoint("MS-ADTS-Security_R32");
            this.Manager.Checkpoint("MS-ADTS-Security_R35");
            this.Manager.Checkpoint("MS-ADTS-Security_R486");
            this.Manager.Comment("reaching state \'S1183\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp370, "return of PasswordChangeOperation, state S1183");
            TCSimpleAuth_2K8R2S1212();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S98
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCSimpleAuth_2K8R2S98()
        {
            this.Manager.BeginTest("TCSimpleAuth_2K8R2S98");
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S493\'");
            ProtocolMessageStructures.errorstatus temp371;
            this.Manager.Comment("executing step \'call SimpleBind(validUserName,validPassword,LDAP_SSL_GC_PORT,Fals" +
                    "e)\'");
            temp371 = this.IMS_ADTS_AuthenticationAuthInstance.SimpleBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_SSL_GC_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R151");
            this.Manager.Checkpoint("MS-ADTS-Security_R201");
            this.Manager.Checkpoint("MS-ADTS-Security_R152");
            this.Manager.Comment("reaching state \'S715\'");
            this.Manager.Comment("checking step \'return SimpleBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp371, "return of SimpleBind, state S715");
            this.Manager.Comment("reaching state \'S937\'");
            ProtocolMessageStructures.errorstatus temp372;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_READ_PROPERTY_USEROBJ,User_Chang" +
                    "e_Password,userPassword,False)\'");
            temp372 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_READ_PROPERTY_USEROBJ, ((ProtocolMessageStructures.ControlAccessRights)(1)), ProtocolMessageStructures.AttribsToCheck.userPassword, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R49");
            this.Manager.Comment("reaching state \'S1050\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp372, "return of AuthorizationCheck, state S1050");
            this.Manager.Comment("reaching state \'S1135\'");
            ProtocolMessageStructures.errorstatus temp373;
            this.Manager.Comment("executing step \'call PasswordChangeOperation(invalidPassword,\"PasswordNew\",False)" +
                    "\'");
            temp373 = this.IMS_ADTS_AuthenticationAuthInstance.PasswordChangeOperation(((ProtocolMessageStructures.Password)(0)), "PasswordNew", false);
            this.Manager.Comment("reaching state \'S1184\'");
            this.Manager.Comment("checking step \'return PasswordChangeOperation/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp373, "return of PasswordChangeOperation, state S1184");
            TCSimpleAuth_2K8R2S1212();
            this.Manager.EndTest();
        }
        #endregion
    }
}
