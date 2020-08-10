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
    public partial class TCMutualAuth_2K8R2 : PtfTestClassBase
    {

        public TCMutualAuth_2K8R2()
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
        public void Security_TCMutualAuth_2K8R2S0()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S60\'");
            ProtocolMessageStructures.errorstatus temp0;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,invalidPassword,False)\'");
            temp0 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R229");
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp0, "return of MutualAuthBind, state S90");
            TCMutualAuth_2K8R2S120();
            this.Manager.EndTest();
        }

        private void TCMutualAuth_2K8R2S120()
        {
            this.Manager.Comment("reaching state \'S120\'");
        }
        #endregion

        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S10()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S65\'");
            ProtocolMessageStructures.errorstatus temp1;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,True)\'");
            temp1 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp1, "return of MutualAuthBind, state S95");
            this.Manager.Comment("reaching state \'S125\'");
            ProtocolMessageStructures.errorstatus temp2;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_METADATA,DS_Replication_Manage_To" +
                    "pology,msDS_ReplAttributeMetaData,False)\'");
            temp2 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_METADATA, ProtocolMessageStructures.ControlAccessRights.DS_Replication_Manage_Topology, ProtocolMessageStructures.AttribsToCheck.msDS_ReplAttributeMetaData, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R57");
            this.Manager.Checkpoint("MS-ADTS-Security_R59");
            this.Manager.Comment("reaching state \'S142\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp2, "return of AuthorizationCheck, state S142");
            TCMutualAuth_2K8R2S154();
            this.Manager.EndTest();
        }

        private void TCMutualAuth_2K8R2S154()
        {
            this.Manager.Comment("reaching state \'S154\'");
        }
        #endregion

        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S12()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S66\'");
            ProtocolMessageStructures.errorstatus temp3;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,True)\'");
            temp3 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S96\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp3, "return of MutualAuthBind, state S96");
            this.Manager.Comment("reaching state \'S126\'");
            ProtocolMessageStructures.errorstatus temp4;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_METADATA,NotSet,msDS_ReplAttribut" +
                    "eMetaData,False)\'");
            temp4 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_METADATA, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_ReplAttributeMetaData, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R57");
            this.Manager.Checkpoint("MS-ADTS-Security_R59");
            this.Manager.Comment("reaching state \'S143\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp4, "return of AuthorizationCheck, state S143");
            TCMutualAuth_2K8R2S154();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S14()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S67\'");
            ProtocolMessageStructures.errorstatus temp5;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,True)\'");
            temp5 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp5, "return of MutualAuthBind, state S97");
            this.Manager.Comment("reaching state \'S127\'");
            ProtocolMessageStructures.errorstatus temp6;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_ReplAttributeMetaData," +
                    "False)\'");
            temp6 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_ReplAttributeMetaData, false);
            this.Manager.Comment("reaching state \'S144\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp6, "return of AuthorizationCheck, state S144");
            TCMutualAuth_2K8R2S155();
            this.Manager.EndTest();
        }

        private void TCMutualAuth_2K8R2S155()
        {
            this.Manager.Comment("reaching state \'S155\'");
        }
        #endregion

        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S16()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S68\'");
            ProtocolMessageStructures.errorstatus temp7;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,True)\'");
            temp7 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp7, "return of MutualAuthBind, state S98");
            this.Manager.Comment("reaching state \'S128\'");
            ProtocolMessageStructures.errorstatus temp8;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Replication_Manage_Topology,msD" +
                    "S_ReplAttributeMetaData,False)\'");
            temp8 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Replication_Manage_Topology, ProtocolMessageStructures.AttribsToCheck.msDS_ReplAttributeMetaData, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R57");
            this.Manager.Checkpoint("MS-ADTS-Security_R59");
            this.Manager.Comment("reaching state \'S145\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp8, "return of AuthorizationCheck, state S145");
            TCMutualAuth_2K8R2S156();
            this.Manager.EndTest();
        }

        private void TCMutualAuth_2K8R2S156()
        {
            this.Manager.Comment("reaching state \'S156\'");
        }
        #endregion

        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S18()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S69\'");
            ProtocolMessageStructures.errorstatus temp9;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,True)\'");
            temp9 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp9, "return of MutualAuthBind, state S99");
            this.Manager.Comment("reaching state \'S129\'");
            ProtocolMessageStructures.errorstatus temp10;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_METADATA,NotSet,msDS_ReplValueMet" +
                    "aData,False)\'");
            temp10 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_METADATA, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_ReplValueMetaData, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R63");
            this.Manager.Checkpoint("MS-ADTS-Security_R64");
            this.Manager.Comment("reaching state \'S146\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp10, "return of AuthorizationCheck, state S146");
            TCMutualAuth_2K8R2S156();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S2()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S61\'");
            ProtocolMessageStructures.errorstatus temp11;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,True)\'");
            temp11 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp11, "return of MutualAuthBind, state S91");
            this.Manager.Comment("reaching state \'S121\'");
            ProtocolMessageStructures.errorstatus temp12;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Replication_Manage_Topology,msD" +
                    "S_ReplAttributeMetaData,False)\'");
            temp12 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Replication_Manage_Topology, ProtocolMessageStructures.AttribsToCheck.msDS_ReplAttributeMetaData, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R57");
            this.Manager.Checkpoint("MS-ADTS-Security_R59");
            this.Manager.Comment("reaching state \'S138\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp12, "return of AuthorizationCheck, state S138");
            TCMutualAuth_2K8R2S154();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S20()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S70\'");
            ProtocolMessageStructures.errorstatus temp13;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,True)\'");
            temp13 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp13, "return of MutualAuthBind, state S100");
            this.Manager.Comment("reaching state \'S130\'");
            ProtocolMessageStructures.errorstatus temp14;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_METADATA,DS_Replication_Manage_To" +
                    "pology,msDS_ReplValueMetaData,False)\'");
            temp14 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_METADATA, ProtocolMessageStructures.ControlAccessRights.DS_Replication_Manage_Topology, ProtocolMessageStructures.AttribsToCheck.msDS_ReplValueMetaData, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R63");
            this.Manager.Checkpoint("MS-ADTS-Security_R64");
            this.Manager.Comment("reaching state \'S147\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp14, "return of AuthorizationCheck, state S147");
            TCMutualAuth_2K8R2S156();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S22()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S71\'");
            ProtocolMessageStructures.errorstatus temp15;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,True)\'");
            temp15 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp15, "return of MutualAuthBind, state S101");
            this.Manager.Comment("reaching state \'S131\'");
            ProtocolMessageStructures.errorstatus temp16;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Replication_Manage_Topology,msD" +
                    "S_ReplValueMetaData,False)\'");
            temp16 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Replication_Manage_Topology, ProtocolMessageStructures.AttribsToCheck.msDS_ReplValueMetaData, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R63");
            this.Manager.Checkpoint("MS-ADTS-Security_R64");
            this.Manager.Comment("reaching state \'S148\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp16, "return of AuthorizationCheck, state S148");
            TCMutualAuth_2K8R2S156();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S24()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S72\'");
            ProtocolMessageStructures.errorstatus temp17;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,True)\'");
            temp17 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp17, "return of MutualAuthBind, state S102");
            this.Manager.Comment("reaching state \'S132\'");
            ProtocolMessageStructures.errorstatus temp18;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_METADATA,DS_Replication_Manage_To" +
                    "pology,msDS_ReplAttributeMetaData,False)\'");
            temp18 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_METADATA, ProtocolMessageStructures.ControlAccessRights.DS_Replication_Manage_Topology, ProtocolMessageStructures.AttribsToCheck.msDS_ReplAttributeMetaData, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R57");
            this.Manager.Checkpoint("MS-ADTS-Security_R59");
            this.Manager.Comment("reaching state \'S149\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp18, "return of AuthorizationCheck, state S149");
            TCMutualAuth_2K8R2S156();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S26()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S73\'");
            ProtocolMessageStructures.errorstatus temp19;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,True)\'");
            temp19 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp19, "return of MutualAuthBind, state S103");
            this.Manager.Comment("reaching state \'S133\'");
            ProtocolMessageStructures.errorstatus temp20;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_METADATA,NotSet,msDS_ReplAttribut" +
                    "eMetaData,False)\'");
            temp20 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_METADATA, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_ReplAttributeMetaData, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R57");
            this.Manager.Checkpoint("MS-ADTS-Security_R59");
            this.Manager.Comment("reaching state \'S150\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp20, "return of AuthorizationCheck, state S150");
            TCMutualAuth_2K8R2S156();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S28()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S74\'");
            ProtocolMessageStructures.errorstatus temp21;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,True)\'");
            temp21 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp21, "return of MutualAuthBind, state S104");
            this.Manager.Comment("reaching state \'S134\'");
            ProtocolMessageStructures.errorstatus temp22;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_ReplAttributeMetaData," +
                    "False)\'");
            temp22 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_ReplAttributeMetaData, false);
            this.Manager.Comment("reaching state \'S151\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp22, "return of AuthorizationCheck, state S151");
            TCMutualAuth_2K8R2S157();
            this.Manager.EndTest();
        }

        private void TCMutualAuth_2K8R2S157()
        {
            this.Manager.Comment("reaching state \'S157\'");
        }
        #endregion

        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S30()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S75\'");
            ProtocolMessageStructures.errorstatus temp23;
            this.Manager.Comment("executing step \'call MutualAuthBind(invalidUserName,validPassword,True)\'");
            temp23 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp23, "return of MutualAuthBind, state S105");
            TCMutualAuth_2K8R2S135();
            this.Manager.EndTest();
        }

        private void TCMutualAuth_2K8R2S135()
        {
            this.Manager.Comment("reaching state \'S135\'");
        }
        #endregion

        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S32()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S76\'");
            ProtocolMessageStructures.errorstatus temp24;
            this.Manager.Comment("executing step \'call MutualAuthBind(invalidUserName,invalidPassword,True)\'");
            temp24 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), true);
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp24, "return of MutualAuthBind, state S106");
            TCMutualAuth_2K8R2S135();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S34()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S77\'");
            ProtocolMessageStructures.errorstatus temp25;
            this.Manager.Comment("executing step \'call MutualAuthBind(invalidUserName,validPassword,False)\'");
            temp25 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R229");
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp25, "return of MutualAuthBind, state S107");
            TCMutualAuth_2K8R2S135();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S36()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S78\'");
            ProtocolMessageStructures.errorstatus temp26;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,False)\'");
            temp26 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R229");
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp26, "return of MutualAuthBind, state S108");
            TCMutualAuth_2K8R2S135();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S38()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S79\'");
            ProtocolMessageStructures.errorstatus temp27;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,invalidPassword,True)\'");
            temp27 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), true);
            this.Manager.Comment("reaching state \'S109\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp27, "return of MutualAuthBind, state S109");
            TCMutualAuth_2K8R2S135();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S4()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S62\'");
            ProtocolMessageStructures.errorstatus temp28;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,True)\'");
            temp28 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp28, "return of MutualAuthBind, state S92");
            this.Manager.Comment("reaching state \'S122\'");
            ProtocolMessageStructures.errorstatus temp29;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_METADATA,NotSet,msDS_ReplValueMet" +
                    "aData,False)\'");
            temp29 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_METADATA, ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_ReplValueMetaData, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R63");
            this.Manager.Checkpoint("MS-ADTS-Security_R64");
            this.Manager.Comment("reaching state \'S139\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp29, "return of AuthorizationCheck, state S139");
            TCMutualAuth_2K8R2S154();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S40
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S40()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S40");
            this.Manager.Comment("reaching state \'S40\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S41\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S80\'");
            ProtocolMessageStructures.errorstatus temp30;
            this.Manager.Comment("executing step \'call MutualAuthBind(invalidUserName,invalidPassword,False)\'");
            temp30 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R229");
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp30, "return of MutualAuthBind, state S110");
            TCMutualAuth_2K8R2S135();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S42
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S42()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S42");
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S81\'");
            ProtocolMessageStructures.errorstatus temp31;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,True)\'");
            temp31 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp31, "return of MutualAuthBind, state S111");
            this.Manager.Comment("reaching state \'S136\'");
            ProtocolMessageStructures.errorstatus temp32;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_ReplValueMetaData,Fals" +
                    "e)\'");
            temp32 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_ReplValueMetaData, false);
            this.Manager.Comment("reaching state \'S152\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp32, "return of AuthorizationCheck, state S152");
            TCMutualAuth_2K8R2S155();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S44
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S44()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S44");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S82\'");
            ProtocolMessageStructures.errorstatus temp33;
            this.Manager.Comment("executing step \'call MutualAuthBind(invalidUserName,validPassword,True)\'");
            temp33 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp33, "return of MutualAuthBind, state S112");
            TCMutualAuth_2K8R2S120();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S46
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S46()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S46");
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S83\'");
            ProtocolMessageStructures.errorstatus temp34;
            this.Manager.Comment("executing step \'call MutualAuthBind(invalidUserName,invalidPassword,True)\'");
            temp34 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), true);
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp34, "return of MutualAuthBind, state S113");
            TCMutualAuth_2K8R2S120();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S48
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S48()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S48");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S84\'");
            ProtocolMessageStructures.errorstatus temp35;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,True)\'");
            temp35 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp35, "return of MutualAuthBind, state S114");
            this.Manager.Comment("reaching state \'S137\'");
            ProtocolMessageStructures.errorstatus temp36;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,NotSet,msDS_ReplValueMetaData,Fals" +
                    "e)\'");
            temp36 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ((ProtocolMessageStructures.ControlAccessRights)(0)), ProtocolMessageStructures.AttribsToCheck.msDS_ReplValueMetaData, false);
            this.Manager.Comment("reaching state \'S153\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp36, "return of AuthorizationCheck, state S153");
            TCMutualAuth_2K8R2S157();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S50
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S50()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S50");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S85\'");
            ProtocolMessageStructures.errorstatus temp37;
            this.Manager.Comment("executing step \'call MutualAuthBind(invalidUserName,validPassword,False)\'");
            temp37 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(1)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R229");
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp37, "return of MutualAuthBind, state S115");
            TCMutualAuth_2K8R2S120();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S52
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S52()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S52");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S86\'");
            ProtocolMessageStructures.errorstatus temp38;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,False)\'");
            temp38 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R229");
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp38, "return of MutualAuthBind, state S116");
            TCMutualAuth_2K8R2S120();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S54
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S54()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S54");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S87\'");
            ProtocolMessageStructures.errorstatus temp39;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,invalidPassword,True)\'");
            temp39 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), true);
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp39, "return of MutualAuthBind, state S117");
            TCMutualAuth_2K8R2S120();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S56
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S56()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S56");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_LDS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(1)));
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S88\'");
            ProtocolMessageStructures.errorstatus temp40;
            this.Manager.Comment("executing step \'call MutualAuthBind(invalidUserName,invalidPassword,False)\'");
            temp40 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(0)), ((ProtocolMessageStructures.Password)(0)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R229");
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp40, "return of MutualAuthBind, state S118");
            TCMutualAuth_2K8R2S120();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S58
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S58()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S58");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S89\'");
            ProtocolMessageStructures.errorstatus temp41;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,invalidPassword,False)\'");
            temp41 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(0)), false);
            this.Manager.Checkpoint("MS-ADTS-Security_R229");
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/failure\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ProtocolMessageStructures.errorstatus.failure, temp41, "return of MutualAuthBind, state S119");
            TCMutualAuth_2K8R2S135();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S6()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S63\'");
            ProtocolMessageStructures.errorstatus temp42;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,True)\'");
            temp42 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp42, "return of MutualAuthBind, state S93");
            this.Manager.Comment("reaching state \'S123\'");
            ProtocolMessageStructures.errorstatus temp43;
            this.Manager.Comment("executing step \'call AuthorizationCheck(DS_READ_METADATA,DS_Replication_Manage_To" +
                    "pology,msDS_ReplValueMetaData,False)\'");
            temp43 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(ProtocolMessageStructures.AccessRights.DS_READ_METADATA, ProtocolMessageStructures.ControlAccessRights.DS_Replication_Manage_Topology, ProtocolMessageStructures.AttribsToCheck.msDS_ReplValueMetaData, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R63");
            this.Manager.Checkpoint("MS-ADTS-Security_R64");
            this.Manager.Comment("reaching state \'S140\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp43, "return of AuthorizationCheck, state S140");
            TCMutualAuth_2K8R2S154();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCMutualAuth_2K8R2S8()
        {
            this.Manager.BeginTest("TCMutualAuth_2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call InitializeSession(True,Windows2008R2,AD_DS)\'");
            this.IMS_ADTS_AuthenticationAuthInstance.InitializeSession(true, ServerVersion.Win2008R2, ((ProtocolMessageStructures.ADTypes)(0)));
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return InitializeSession\'");
            this.Manager.Comment("reaching state \'S64\'");
            ProtocolMessageStructures.errorstatus temp44;
            this.Manager.Comment("executing step \'call MutualAuthBind(validUserName,validPassword,True)\'");
            temp44 = this.IMS_ADTS_AuthenticationAuthInstance.MutualAuthBind(((ProtocolMessageStructures.name)(1)), ((ProtocolMessageStructures.Password)(1)), true);
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("checking step \'return MutualAuthBind/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp44, "return of MutualAuthBind, state S94");
            this.Manager.Comment("reaching state \'S124\'");
            ProtocolMessageStructures.errorstatus temp45;
            this.Manager.Comment("executing step \'call AuthorizationCheck(NotSet,DS_Replication_Manage_Topology,msD" +
                    "S_ReplValueMetaData,False)\'");
            temp45 = this.IMS_ADTS_AuthenticationAuthInstance.AuthorizationCheck(((ProtocolMessageStructures.AccessRights)(0)), ProtocolMessageStructures.ControlAccessRights.DS_Replication_Manage_Topology, ProtocolMessageStructures.AttribsToCheck.msDS_ReplValueMetaData, false);
            this.Manager.Checkpoint("MS-ADTS-Security_R63");
            this.Manager.Checkpoint("MS-ADTS-Security_R64");
            this.Manager.Comment("reaching state \'S141\'");
            this.Manager.Comment("checking step \'return AuthorizationCheck/success\'");
            TestManagerHelpers.AssertAreEqual<ProtocolMessageStructures.errorstatus>(this.Manager, ((ProtocolMessageStructures.errorstatus)(0)), temp45, "return of AuthorizationCheck, state S141");
            TCMutualAuth_2K8R2S154();
            this.Manager.EndTest();
        }
        #endregion
    }
}
