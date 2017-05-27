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
    using ds = System.DirectoryServices.Protocols;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Spec Explorer", "3.5.3146.0")]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class TCAdditionalAttribCheck_2K8R2 : PtfTestClassBase
    {

        public TCAdditionalAttribCheck_2K8R2()
        {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "100000");
        }

        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security.IMS_ADTS_AuthenticationAuth IMS_ADTS_AuthenticationAuthInstance;
        private static ADCommonServerAdapter common;
        #endregion

        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static void ClassInitialize(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext context)
        {
            PtfTestClassBase.Initialize(context);
            common = new ADCommonServerAdapter();
            common.Initialize(BaseTestSite);
            string domainDN = Utilities.DomainDnsNameToDN(common.PrimaryDomainDnsName);
            string PdcFqdn = common.PDCNetbiosName + "." + common.PrimaryDomainDnsName;
            string noaceuserdn = string.Format("CN={0},CN=Users,{1}", MS_ADTS_SecurityRequirementsValidator.NoAceUser, domainDN);
            ds.LdapConnection conn = new ds.LdapConnection(
                new ds.LdapDirectoryIdentifier(PdcFqdn, int.Parse(common.ADDSPortNum)),
                new System.Net.NetworkCredential(common.DomainAdministratorName, common.DomainUserPassword, common.PrimaryDomainDnsName));
            conn.Bind();
            ds.SearchRequest sr = new ds.SearchRequest(
                "CN=Users," + Utilities.ParseDN(common.PrimaryDomainDnsName),
                "(objectclass=*)",
                System.DirectoryServices.Protocols.SearchScope.OneLevel,
                "name");
            bool exists = false;
            ds.SearchResponse srep = conn.SendRequest(sr) as ds.SearchResponse;
            foreach (ds.SearchResultEntry entry in srep.Entries)
            {
                if (string.Equals(
                    entry.DistinguishedName, noaceuserdn, StringComparison.InvariantCultureIgnoreCase))
                {
                    exists = true; //The user exists, skip creating.
                    break;
                }

            }
            // Create the user if it does not exist.
            if (!exists)
            {
                Utilities.NewUser(
                    PdcFqdn,
                    common.ADDSPortNum,
                    "CN=Users," + domainDN,
                    MS_ADTS_SecurityRequirementsValidator.NoAceUser,
                    MS_ADTS_SecurityRequirementsValidator.NoAceUserPassword,
                    common.DomainAdministratorName,
                    common.DomainUserPassword);
                Utilities.DisableInheritance(
                    PdcFqdn,
                    common.ADDSPortNum,
                    noaceuserdn,
                    common.DomainAdministratorName,
                    common.DomainUserPassword);
            }
            Utilities.BackupOrRestoreNtSecurityDescriptor(
                PdcFqdn,
                int.Parse(common.ADDSPortNum),
                noaceuserdn,
                Utilities.SecurityDescriptorBackupFilename,
                new System.Net.NetworkCredential(common.DomainAdministratorName, common.DomainUserPassword, common.PrimaryDomainDnsName));
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Utilities.BackupOrRestoreNtSecurityDescriptor(
                common.PDCNetbiosName + "." + common.PrimaryDomainDnsName,
                int.Parse(common.ADDSPortNum),
                string.Format("CN={0},CN=Users,{1}", MS_ADTS_SecurityRequirementsValidator.NoAceUser, Utilities.DomainDnsNameToDN(common.PrimaryDomainDnsName)),
                Utilities.SecurityDescriptorBackupFilename,
                new System.Net.NetworkCredential(common.DomainAdministratorName, common.DomainUserPassword, common.PrimaryDomainDnsName));
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
        [TestCategory("MS-ADTS-Security_ADLDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S0()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S44\'");
            ProtocolMessageStructures.errorstatus temp0;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp0 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp0, "return of SASLAuthentication, state S66");
            this.Manager.Comment("reaching state \'S88\'");
            ProtocolMessageStructures.errorstatus temp1;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,servicePrincipleName,False)" +
                    "\'");
            temp1 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.servicePrincipleName, false);
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp1, "return of AuthorizationCheck, state S110");
            TCAdditionalAttribCheck_2K8R2S132();
            this.Manager.EndTest();
        }

        private void TCAdditionalAttribCheck_2K8R2S132()
        {
            this.Manager.Comment("reaching state \'S132\'");
        }
        #endregion

        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S10()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S49\'");
            ProtocolMessageStructures.errorstatus temp2;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp2 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp2, "return of SASLAuthentication, state S71");
            this.Manager.Comment("reaching state \'S93\'");
            ProtocolMessageStructures.errorstatus temp3;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_WRITE_PROPERTY,NotSet,moveOperat" +
                    "ion,False)\'");
            temp3 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_WRITE_PROPERTY, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.moveOperation, false);
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp3, "return of AuthorizationCheck, state S115");
            TCAdditionalAttribCheck_2K8R2S133();
            this.Manager.EndTest();
        }

        private void TCAdditionalAttribCheck_2K8R2S133()
        {
            this.Manager.Comment("reaching state \'S133\'");
        }
        #endregion

        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S12()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S50\'");
            ProtocolMessageStructures.errorstatus temp4;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp4 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp4, "return of SASLAuthentication, state S72");
            this.Manager.Comment("reaching state \'S94\'");
            ProtocolMessageStructures.errorstatus temp5;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_WRITE_EXTENDED,NotSet,servicePri" +
                    "ncipleName,False)\'");
            temp5 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_WRITE_EXTENDED, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.servicePrincipleName, false);
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp5, "return of AuthorizationCheck, state S116");
            TCAdditionalAttribCheck_2K8R2S133();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S14()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S51\'");
            ProtocolMessageStructures.errorstatus temp6;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp6 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp6, "return of SASLAuthentication, state S73");
            this.Manager.Comment("reaching state \'S95\'");
            ProtocolMessageStructures.errorstatus temp7;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_WRITE_PROPERTY,NotSet,dnsHostNam" +
                    "e,False)\'");
            temp7 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_WRITE_PROPERTY, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.dnsHostName, false);
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp7, "return of AuthorizationCheck, state S117");
            TCAdditionalAttribCheck_2K8R2S133();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S16()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S52\'");
            ProtocolMessageStructures.errorstatus temp8;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp8 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp8, "return of SASLAuthentication, state S74");
            this.Manager.Comment("reaching state \'S96\'");
            ProtocolMessageStructures.errorstatus temp9;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,moveOperation,False)\'");
            temp9 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.moveOperation, false);
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp9, "return of AuthorizationCheck, state S118");
            TCAdditionalAttribCheck_2K8R2S134();
            this.Manager.EndTest();
        }

        private void TCAdditionalAttribCheck_2K8R2S134()
        {
            this.Manager.Comment("reaching state \'S134\'");
        }
        #endregion

        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S18()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S53\'");
            ProtocolMessageStructures.errorstatus temp10;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp10 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp10, "return of SASLAuthentication, state S75");
            this.Manager.Comment("reaching state \'S97\'");
            ProtocolMessageStructures.errorstatus temp11;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,dnsHostName,False)\'");
            temp11 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.dnsHostName, false);
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp11, "return of AuthorizationCheck, state S119");
            TCAdditionalAttribCheck_2K8R2S134();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S2()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S45\'");
            ProtocolMessageStructures.errorstatus temp12;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp12 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp12, "return of SASLAuthentication, state S67");
            this.Manager.Comment("reaching state \'S89\'");
            ProtocolMessageStructures.errorstatus temp13;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_CREATE_CHILD,NotSet,moveOperatio" +
                    "n,False)\'");
            temp13 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_CREATE_CHILD, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.moveOperation, false);
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp13, "return of AuthorizationCheck, state S111");
            TCAdditionalAttribCheck_2K8R2S133();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S20()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S54\'");
            ProtocolMessageStructures.errorstatus temp14;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp14 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp14, "return of SASLAuthentication, state S76");
            this.Manager.Comment("reaching state \'S98\'");
            ProtocolMessageStructures.errorstatus temp15;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,writeDACLOperation,False)\'");
            temp15 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.writeDACLOperation, false);
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp15, "return of AuthorizationCheck, state S120");
            TCAdditionalAttribCheck_2K8R2S134();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S22()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S55\'");
            ProtocolMessageStructures.errorstatus temp16;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp16 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp16, "return of SASLAuthentication, state S77");
            this.Manager.Comment("reaching state \'S99\'");
            ProtocolMessageStructures.errorstatus temp17;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_CREATE_CHILD,NotSet,moveOperatio" +
                    "n,False)\'");
            temp17 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_CREATE_CHILD, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.moveOperation, false);
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp17, "return of AuthorizationCheck, state S121");
            TCAdditionalAttribCheck_2K8R2S135();
            this.Manager.EndTest();
        }

        private void TCAdditionalAttribCheck_2K8R2S135()
        {
            this.Manager.Comment("reaching state \'S135\'");
        }
        #endregion

        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S24()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S56\'");
            ProtocolMessageStructures.errorstatus temp18;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp18 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp18, "return of SASLAuthentication, state S78");
            this.Manager.Comment("reaching state \'S100\'");
            ProtocolMessageStructures.errorstatus temp19;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_WRITE_PROPERTY,NotSet,servicePri" +
                    "ncipleName,False)\'");
            temp19 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_WRITE_PROPERTY, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.servicePrincipleName, false);
            this.Manager.Comment("reaching state \'S122\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp19, "return of AuthorizationCheck, state S122");
            TCAdditionalAttribCheck_2K8R2S135();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S26()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S57\'");
            ProtocolMessageStructures.errorstatus temp20;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp20 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp20, "return of SASLAuthentication, state S79");
            this.Manager.Comment("reaching state \'S101\'");
            ProtocolMessageStructures.errorstatus temp21;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_WRITE_EXTENDED,NotSet,dnsHostNam" +
                    "e,False)\'");
            temp21 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_WRITE_EXTENDED, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.dnsHostName, false);
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp21, "return of AuthorizationCheck, state S123");
            TCAdditionalAttribCheck_2K8R2S135();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S28()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S58\'");
            ProtocolMessageStructures.errorstatus temp22;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp22 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp22, "return of SASLAuthentication, state S80");
            this.Manager.Comment("reaching state \'S102\'");
            ProtocolMessageStructures.errorstatus temp23;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_WRITE_DAC,NotSet,writeDACLOperation" +
                    ",False)\'");
            temp23 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_WRITE_DAC, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.writeDACLOperation, false);
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp23, "return of AuthorizationCheck, state S124");
            TCAdditionalAttribCheck_2K8R2S135();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S30()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S59\'");
            ProtocolMessageStructures.errorstatus temp24;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp24 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp24, "return of SASLAuthentication, state S81");
            this.Manager.Comment("reaching state \'S103\'");
            ProtocolMessageStructures.errorstatus temp25;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_WRITE_PROPERTY,NotSet,moveOperat" +
                    "ion,False)\'");
            temp25 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_WRITE_PROPERTY, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.moveOperation, false);
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp25, "return of AuthorizationCheck, state S125");
            TCAdditionalAttribCheck_2K8R2S135();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S32()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S60\'");
            ProtocolMessageStructures.errorstatus temp26;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp26 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp26, "return of SASLAuthentication, state S82");
            this.Manager.Comment("reaching state \'S104\'");
            ProtocolMessageStructures.errorstatus temp27;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_WRITE_EXTENDED,NotSet,servicePri" +
                    "ncipleName,False)\'");
            temp27 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_WRITE_EXTENDED, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.servicePrincipleName, false);
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp27, "return of AuthorizationCheck, state S126");
            TCAdditionalAttribCheck_2K8R2S135();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S34()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S61\'");
            ProtocolMessageStructures.errorstatus temp28;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp28 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp28, "return of SASLAuthentication, state S83");
            this.Manager.Comment("reaching state \'S105\'");
            ProtocolMessageStructures.errorstatus temp29;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_WRITE_PROPERTY,NotSet,dnsHostNam" +
                    "e,False)\'");
            temp29 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_WRITE_PROPERTY, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.dnsHostName, false);
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp29, "return of AuthorizationCheck, state S127");
            TCAdditionalAttribCheck_2K8R2S135();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S36()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S62\'");
            ProtocolMessageStructures.errorstatus temp30;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp30 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp30, "return of SASLAuthentication, state S84");
            this.Manager.Comment("reaching state \'S106\'");
            ProtocolMessageStructures.errorstatus temp31;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,moveOperation,False)\'");
            temp31 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.moveOperation, false);
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp31, "return of AuthorizationCheck, state S128");
            TCAdditionalAttribCheck_2K8R2S132();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S38()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S63\'");
            ProtocolMessageStructures.errorstatus temp32;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp32 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp32, "return of SASLAuthentication, state S85");
            this.Manager.Comment("reaching state \'S107\'");
            ProtocolMessageStructures.errorstatus temp33;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,dnsHostName,False)\'");
            temp33 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.dnsHostName, false);
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp33, "return of AuthorizationCheck, state S129");
            TCAdditionalAttribCheck_2K8R2S132();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S4()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S46\'");
            ProtocolMessageStructures.errorstatus temp34;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp34 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp34, "return of SASLAuthentication, state S68");
            this.Manager.Comment("reaching state \'S90\'");
            ProtocolMessageStructures.errorstatus temp35;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_WRITE_PROPERTY,NotSet,servicePri" +
                    "ncipleName,False)\'");
            temp35 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_WRITE_PROPERTY, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.servicePrincipleName, false);
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp35, "return of AuthorizationCheck, state S112");
            TCAdditionalAttribCheck_2K8R2S133();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S40
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S40()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S64\'");
            ProtocolMessageStructures.errorstatus temp36;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp36 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp36, "return of SASLAuthentication, state S86");
            this.Manager.Comment("reaching state \'S108\'");
            ProtocolMessageStructures.errorstatus temp37;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,writeDACLOperation,False)\'");
            temp37 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.writeDACLOperation, false);
            this.Manager.Comment("reaching state \'S130\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp37, "return of AuthorizationCheck, state S130");
            TCAdditionalAttribCheck_2K8R2S132();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S42
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S42()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S65\'");
            ProtocolMessageStructures.errorstatus temp38;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp38 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp38, "return of SASLAuthentication, state S87");
            this.Manager.Comment("reaching state \'S109\'");
            ProtocolMessageStructures.errorstatus temp39;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,servicePrincipleName,False)" +
                    "\'");
            temp39 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.servicePrincipleName, false);
            this.Manager.Comment("reaching state \'S131\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp39, "return of AuthorizationCheck, state S131");
            TCAdditionalAttribCheck_2K8R2S134();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S6()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S47\'");
            ProtocolMessageStructures.errorstatus temp40;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp40 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp40, "return of SASLAuthentication, state S69");
            this.Manager.Comment("reaching state \'S91\'");
            ProtocolMessageStructures.errorstatus temp41;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_DS_WRITE_EXTENDED,NotSet,dnsHostNam" +
                    "e,False)\'");
            temp41 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_DS_WRITE_EXTENDED, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.dnsHostName, false);
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp41, "return of AuthorizationCheck, state S113");
            TCAdditionalAttribCheck_2K8R2S133();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCAdditionalAttribCheck_2K8R2S8()
        {
            this.Manager.BeginTest("TCAdditionalAttribCheck_2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, Common.ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S48\'");
            ProtocolMessageStructures.errorstatus temp42;
            this.Manager.Comment("executing step \'call SASLAuthentication(validUserName,saslgssapi,validPassword,LD" +
                    "AP_PORT,False)\'");
            temp42 = this.IMS_ADTS_AuthenticationAuthInstance.SASLAuthentication(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.SASLChoice)(0)), ((ProtocolMessageStructures.Password)(1)), ProtocolMessageStructures.Port.LDAP_PORT, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R467");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("checking step \'return SASLAuthentication/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp42, "return of SASLAuthentication, state S70");
            this.Manager.Comment("reaching state \'S92\'");
            ProtocolMessageStructures.errorstatus temp43;
            this.Manager.Comment("executing step \'call AuthorizationCheck(RIGHT_WRITE_DAC,NotSet,writeDACLOperation" +
                    ",False)\'");
            temp43 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.RIGHT_WRITE_DAC, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.writeDACLOperation, false);
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp43, "return of AuthorizationCheck, state S114");
            TCAdditionalAttribCheck_2K8R2S133();
            this.Manager.EndTest();
        }
        #endregion
    }
}
