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
    public partial class TCTDOOperation_Scenario2_2K8R2 : PtfTestClassBase
    {

        public TCTDOOperation_Scenario2_2K8R2()
        {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "10000");
        }

        #region Expect Delegates
        public delegate void DeleteTDODelegate1();
        #endregion

        #region Event Metadata
        static System.Reflection.MethodBase DeleteTDOInfo = TestManagerHelpers.GetMethodInfo(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security.IMS_ADTS_TDOAdapter), "DeleteTDO");
        #endregion

        #region Adapter Instances
        private Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security.IMS_ADTS_TDOAdapter IMS_ADTS_TDOAdapterInstance;
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
            this.IMS_ADTS_TDOAdapterInstance = ((Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security.IMS_ADTS_TDOAdapter)(this.Manager.GetAdapter(typeof(Microsoft.Protocols.TestSuites.ActiveDirectory.Adts.Security.IMS_ADTS_TDOAdapter))));
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion

        #region Test Starting in S0
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario2_2K8R2S0()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario2_2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S32\'");
            bool temp0;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_BIDI" +
                    "RECTIONAL,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp0 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.NotSet,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_BIDIRECTIONAL,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp0, "return of SetInformationTDO, state S48");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("executing step \'call CreateTrustedAccounts(\"TrustObject1\",InterDomain_Trust_Info(" +
                    "cnAttribute=\"Acc1\",sAMAccName=\"Acc1\",interDomainAccType=NotSet,accControl=ADS_UF" +
                    "_INTERDOMAIN_TRUST_ACCOUNT))\'");
            this.IMS_ADTS_TDOAdapterInstance.CreateTrustedAccounts("TrustObject1", this.Make<ProtocolMessageStructures.InterDomain_Trust_Info>(new string[] {
                            "cnAttribute",
                            "sAMAccName",
                            "interDomainAccType",
                            "accControl"}, new object[] {
                            "Acc1",
                            "Acc1",
                            ProtocolMessageStructures.sAMAccountType.NotSet,
                            ProtocolMessageStructures.userAccountControl.ADS_UF_INTERDOMAIN_TRUST_ACCOUNT}));
            this.Manager.Comment("reaching state \'S80\'");
            this.Manager.Comment("checking step \'return CreateTrustedAccounts\'");
            this.Manager.Comment("reaching state \'S96\'");
            bool temp1;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp1 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("checking step \'return TDOOperation/False\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, temp1, "return of TDOOperation, state S112");
            this.Manager.Comment("reaching state \'S121\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario2_2K8R2S113();
            this.Manager.EndTest();
        }

        private void TCTDOOperation_Scenario2_2K8R2S113()
        {
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCTDOOperation_Scenario2_2K8R2.DeleteTDOInfo, null, new DeleteTDODelegate1(this.TCTDOOperation_Scenario2_2K8R2S0DeleteTDOChecker)));
            this.Manager.Comment("reaching state \'S122\'");
        }

        private void TCTDOOperation_Scenario2_2K8R2S0DeleteTDOChecker()
        {
            this.Manager.Comment("checking step \'return DeleteTDO\'");
        }
        #endregion

        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario2_2K8R2S10()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario2_2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S37\'");
            bool temp2;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_INBO" +
                    "UND,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp2 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.NotSet,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_INBOUND,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S53\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp2, "return of SetInformationTDO, state S53");
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("executing step \'call CreateTrustedAccounts(\"TrustObject1\",InterDomain_Trust_Info(" +
                    "cnAttribute=\"Acc1\",sAMAccName=\"Acc1\",interDomainAccType=NotSet,accControl=NotSet" +
                    "))\'");
            this.IMS_ADTS_TDOAdapterInstance.CreateTrustedAccounts("TrustObject1", this.Make<ProtocolMessageStructures.InterDomain_Trust_Info>(new string[] {
                            "cnAttribute",
                            "sAMAccName",
                            "interDomainAccType",
                            "accControl"}, new object[] {
                            "Acc1",
                            "Acc1",
                            ProtocolMessageStructures.sAMAccountType.NotSet,
                            ProtocolMessageStructures.userAccountControl.NotSet}));
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("checking step \'return CreateTrustedAccounts\'");
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario2_2K8R2S113();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario2_2K8R2S12()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario2_2K8R2S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S38\'");
            bool temp3;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_INBO" +
                    "UND,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp3 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.NotSet,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_INBOUND,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp3, "return of SetInformationTDO, state S54");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("executing step \'call CreateTrustedAccounts(\"TrustObject1\",InterDomain_Trust_Info(" +
                    "cnAttribute=\"Acc1\",sAMAccName=\"Acc1\",interDomainAccType=SAM_TRUST_ACCOUNT,accCon" +
                    "trol=ADS_UF_INTERDOMAIN_TRUST_ACCOUNT))\'");
            this.IMS_ADTS_TDOAdapterInstance.CreateTrustedAccounts("TrustObject1", this.Make<ProtocolMessageStructures.InterDomain_Trust_Info>(new string[] {
                            "cnAttribute",
                            "sAMAccName",
                            "interDomainAccType",
                            "accControl"}, new object[] {
                            "Acc1",
                            "Acc1",
                            ProtocolMessageStructures.sAMAccountType.SAM_TRUST_ACCOUNT,
                            ProtocolMessageStructures.userAccountControl.ADS_UF_INTERDOMAIN_TRUST_ACCOUNT}));
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("checking step \'return CreateTrustedAccounts\'");
            this.Manager.Comment("reaching state \'S102\'");
            bool temp4;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp4 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Checkpoint("MS-ADTS-Security_R663");
            this.Manager.Checkpoint("MS-ADTS-Security_R664");
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("checking step \'return TDOOperation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp4, "return of TDOOperation, state S114");
            this.Manager.Comment("reaching state \'S123\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario2_2K8R2S113();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario2_2K8R2S14()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario2_2K8R2S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S39\'");
            bool temp5;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_INBO" +
                    "UND,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp5 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.NotSet,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_INBOUND,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp5, "return of SetInformationTDO, state S55");
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("executing step \'call CreateTrustedAccounts(\"TrustObject1\",InterDomain_Trust_Info(" +
                    "cnAttribute=\"Acc1\",sAMAccName=\"Acc1\",interDomainAccType=SAM_TRUST_ACCOUNT,accCon" +
                    "trol=NotSet))\'");
            this.IMS_ADTS_TDOAdapterInstance.CreateTrustedAccounts("TrustObject1", this.Make<ProtocolMessageStructures.InterDomain_Trust_Info>(new string[] {
                            "cnAttribute",
                            "sAMAccName",
                            "interDomainAccType",
                            "accControl"}, new object[] {
                            "Acc1",
                            "Acc1",
                            ProtocolMessageStructures.sAMAccountType.SAM_TRUST_ACCOUNT,
                            ProtocolMessageStructures.userAccountControl.NotSet}));
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("checking step \'return CreateTrustedAccounts\'");
            this.Manager.Comment("reaching state \'S103\'");
            bool temp6;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp6 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("checking step \'return TDOOperation/False\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, temp6, "return of TDOOperation, state S115");
            this.Manager.Comment("reaching state \'S124\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario2_2K8R2S113();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario2_2K8R2S16()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario2_2K8R2S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S40\'");
            bool temp7;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_INBO" +
                    "UND,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp7 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.NotSet,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_INBOUND,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp7, "return of SetInformationTDO, state S56");
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("executing step \'call CreateTrustedAccounts(\"TrustObject1\",InterDomain_Trust_Info(" +
                    "cnAttribute=\"Acc1\",sAMAccName=\"Acc1\",interDomainAccType=NotSet,accControl=ADS_UF" +
                    "_INTERDOMAIN_TRUST_ACCOUNT))\'");
            this.IMS_ADTS_TDOAdapterInstance.CreateTrustedAccounts("TrustObject1", this.Make<ProtocolMessageStructures.InterDomain_Trust_Info>(new string[] {
                            "cnAttribute",
                            "sAMAccName",
                            "interDomainAccType",
                            "accControl"}, new object[] {
                            "Acc1",
                            "Acc1",
                            ProtocolMessageStructures.sAMAccountType.NotSet,
                            ProtocolMessageStructures.userAccountControl.ADS_UF_INTERDOMAIN_TRUST_ACCOUNT}));
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("checking step \'return CreateTrustedAccounts\'");
            this.Manager.Comment("reaching state \'S104\'");
            bool temp8;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp8 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("checking step \'return TDOOperation/False\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, temp8, "return of TDOOperation, state S116");
            this.Manager.Comment("reaching state \'S125\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario2_2K8R2S113();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario2_2K8R2S18()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario2_2K8R2S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S41\'");
            bool temp9;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_BIDI" +
                    "RECTIONAL,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp9 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.NotSet,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_BIDIRECTIONAL,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp9, "return of SetInformationTDO, state S57");
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("executing step \'call CreateTrustedAccounts(\"TrustObject1\",InterDomain_Trust_Info(" +
                    "cnAttribute=\"Acc1\",sAMAccName=\"Acc1\",interDomainAccType=SAM_TRUST_ACCOUNT,accCon" +
                    "trol=ADS_UF_INTERDOMAIN_TRUST_ACCOUNT))\'");
            this.IMS_ADTS_TDOAdapterInstance.CreateTrustedAccounts("TrustObject1", this.Make<ProtocolMessageStructures.InterDomain_Trust_Info>(new string[] {
                            "cnAttribute",
                            "sAMAccName",
                            "interDomainAccType",
                            "accControl"}, new object[] {
                            "Acc1",
                            "Acc1",
                            ProtocolMessageStructures.sAMAccountType.SAM_TRUST_ACCOUNT,
                            ProtocolMessageStructures.userAccountControl.ADS_UF_INTERDOMAIN_TRUST_ACCOUNT}));
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("checking step \'return CreateTrustedAccounts\'");
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario2_2K8R2S113();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario2_2K8R2S2()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario2_2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S33\'");
            bool temp10;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_BIDI" +
                    "RECTIONAL,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp10 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.NotSet,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_BIDIRECTIONAL,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp10, "return of SetInformationTDO, state S49");
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("executing step \'call CreateTrustedAccounts(\"TrustObject1\",InterDomain_Trust_Info(" +
                    "cnAttribute=\"Acc1\",sAMAccName=\"Acc1\",interDomainAccType=NotSet,accControl=NotSet" +
                    "))\'");
            this.IMS_ADTS_TDOAdapterInstance.CreateTrustedAccounts("TrustObject1", this.Make<ProtocolMessageStructures.InterDomain_Trust_Info>(new string[] {
                            "cnAttribute",
                            "sAMAccName",
                            "interDomainAccType",
                            "accControl"}, new object[] {
                            "Acc1",
                            "Acc1",
                            ProtocolMessageStructures.sAMAccountType.NotSet,
                            ProtocolMessageStructures.userAccountControl.NotSet}));
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("checking step \'return CreateTrustedAccounts\'");
            this.Manager.Comment("reaching state \'S97\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario2_2K8R2S113();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario2_2K8R2S20()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario2_2K8R2S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S42\'");
            bool temp11;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_BIDI" +
                    "RECTIONAL,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp11 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.NotSet,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_BIDIRECTIONAL,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp11, "return of SetInformationTDO, state S58");
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("executing step \'call CreateTrustedAccounts(\"TrustObject1\",InterDomain_Trust_Info(" +
                    "cnAttribute=\"Acc1\",sAMAccName=\"Acc1\",interDomainAccType=SAM_TRUST_ACCOUNT,accCon" +
                    "trol=NotSet))\'");
            this.IMS_ADTS_TDOAdapterInstance.CreateTrustedAccounts("TrustObject1", this.Make<ProtocolMessageStructures.InterDomain_Trust_Info>(new string[] {
                            "cnAttribute",
                            "sAMAccName",
                            "interDomainAccType",
                            "accControl"}, new object[] {
                            "Acc1",
                            "Acc1",
                            ProtocolMessageStructures.sAMAccountType.SAM_TRUST_ACCOUNT,
                            ProtocolMessageStructures.userAccountControl.NotSet}));
            this.Manager.Comment("reaching state \'S90\'");
            this.Manager.Comment("checking step \'return CreateTrustedAccounts\'");
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario2_2K8R2S113();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario2_2K8R2S22()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario2_2K8R2S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S43\'");
            bool temp12;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_BIDI" +
                    "RECTIONAL,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp12 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.NotSet,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_BIDIRECTIONAL,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S59\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp12, "return of SetInformationTDO, state S59");
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("executing step \'call CreateTrustedAccounts(\"TrustObject1\",InterDomain_Trust_Info(" +
                    "cnAttribute=\"Acc1\",sAMAccName=\"Acc1\",interDomainAccType=NotSet,accControl=ADS_UF" +
                    "_INTERDOMAIN_TRUST_ACCOUNT))\'");
            this.IMS_ADTS_TDOAdapterInstance.CreateTrustedAccounts("TrustObject1", this.Make<ProtocolMessageStructures.InterDomain_Trust_Info>(new string[] {
                            "cnAttribute",
                            "sAMAccName",
                            "interDomainAccType",
                            "accControl"}, new object[] {
                            "Acc1",
                            "Acc1",
                            ProtocolMessageStructures.sAMAccountType.NotSet,
                            ProtocolMessageStructures.userAccountControl.ADS_UF_INTERDOMAIN_TRUST_ACCOUNT}));
            this.Manager.Comment("reaching state \'S91\'");
            this.Manager.Comment("checking step \'return CreateTrustedAccounts\'");
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario2_2K8R2S113();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario2_2K8R2S24()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario2_2K8R2S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S44\'");
            bool temp13;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_BIDI" +
                    "RECTIONAL,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp13 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.NotSet,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_BIDIRECTIONAL,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp13, "return of SetInformationTDO, state S60");
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("executing step \'call CreateTrustedAccounts(\"TrustObject1\",InterDomain_Trust_Info(" +
                    "cnAttribute=\"Acc1\",sAMAccName=\"Acc1\",interDomainAccType=NotSet,accControl=NotSet" +
                    "))\'");
            this.IMS_ADTS_TDOAdapterInstance.CreateTrustedAccounts("TrustObject1", this.Make<ProtocolMessageStructures.InterDomain_Trust_Info>(new string[] {
                            "cnAttribute",
                            "sAMAccName",
                            "interDomainAccType",
                            "accControl"}, new object[] {
                            "Acc1",
                            "Acc1",
                            ProtocolMessageStructures.sAMAccountType.NotSet,
                            ProtocolMessageStructures.userAccountControl.NotSet}));
            this.Manager.Comment("reaching state \'S92\'");
            this.Manager.Comment("checking step \'return CreateTrustedAccounts\'");
            this.Manager.Comment("reaching state \'S108\'");
            bool temp14;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp14 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("checking step \'return TDOOperation/False\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, temp14, "return of TDOOperation, state S117");
            this.Manager.Comment("reaching state \'S126\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario2_2K8R2S113();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario2_2K8R2S26()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario2_2K8R2S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S45\'");
            bool temp15;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_BIDI" +
                    "RECTIONAL,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp15 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.NotSet,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_BIDIRECTIONAL,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp15, "return of SetInformationTDO, state S61");
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("executing step \'call CreateTrustedAccounts(\"TrustObject1\",InterDomain_Trust_Info(" +
                    "cnAttribute=\"Acc1\",sAMAccName=\"Acc1\",interDomainAccType=SAM_TRUST_ACCOUNT,accCon" +
                    "trol=ADS_UF_INTERDOMAIN_TRUST_ACCOUNT))\'");
            this.IMS_ADTS_TDOAdapterInstance.CreateTrustedAccounts("TrustObject1", this.Make<ProtocolMessageStructures.InterDomain_Trust_Info>(new string[] {
                            "cnAttribute",
                            "sAMAccName",
                            "interDomainAccType",
                            "accControl"}, new object[] {
                            "Acc1",
                            "Acc1",
                            ProtocolMessageStructures.sAMAccountType.SAM_TRUST_ACCOUNT,
                            ProtocolMessageStructures.userAccountControl.ADS_UF_INTERDOMAIN_TRUST_ACCOUNT}));
            this.Manager.Comment("reaching state \'S93\'");
            this.Manager.Comment("checking step \'return CreateTrustedAccounts\'");
            this.Manager.Comment("reaching state \'S109\'");
            bool temp16;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp16 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Checkpoint("MS-ADTS-Security_R663");
            this.Manager.Checkpoint("MS-ADTS-Security_R664");
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("checking step \'return TDOOperation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp16, "return of TDOOperation, state S118");
            this.Manager.Comment("reaching state \'S127\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario2_2K8R2S113();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario2_2K8R2S28()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario2_2K8R2S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S46\'");
            bool temp17;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_BIDI" +
                    "RECTIONAL,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp17 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.NotSet,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_BIDIRECTIONAL,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp17, "return of SetInformationTDO, state S62");
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("executing step \'call CreateTrustedAccounts(\"TrustObject1\",InterDomain_Trust_Info(" +
                    "cnAttribute=\"Acc1\",sAMAccName=\"Acc1\",interDomainAccType=SAM_TRUST_ACCOUNT,accCon" +
                    "trol=NotSet))\'");
            this.IMS_ADTS_TDOAdapterInstance.CreateTrustedAccounts("TrustObject1", this.Make<ProtocolMessageStructures.InterDomain_Trust_Info>(new string[] {
                            "cnAttribute",
                            "sAMAccName",
                            "interDomainAccType",
                            "accControl"}, new object[] {
                            "Acc1",
                            "Acc1",
                            ProtocolMessageStructures.sAMAccountType.SAM_TRUST_ACCOUNT,
                            ProtocolMessageStructures.userAccountControl.NotSet}));
            this.Manager.Comment("reaching state \'S94\'");
            this.Manager.Comment("checking step \'return CreateTrustedAccounts\'");
            this.Manager.Comment("reaching state \'S110\'");
            bool temp18;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp18 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Comment("reaching state \'S119\'");
            this.Manager.Comment("checking step \'return TDOOperation/False\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, temp18, "return of TDOOperation, state S119");
            this.Manager.Comment("reaching state \'S128\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario2_2K8R2S113();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario2_2K8R2S30()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario2_2K8R2S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S47\'");
            bool temp19;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_INBO" +
                    "UND,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp19 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.NotSet,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_INBOUND,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp19, "return of SetInformationTDO, state S63");
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("executing step \'call CreateTrustedAccounts(\"TrustObject1\",InterDomain_Trust_Info(" +
                    "cnAttribute=\"Acc1\",sAMAccName=\"Acc1\",interDomainAccType=NotSet,accControl=NotSet" +
                    "))\'");
            this.IMS_ADTS_TDOAdapterInstance.CreateTrustedAccounts("TrustObject1", this.Make<ProtocolMessageStructures.InterDomain_Trust_Info>(new string[] {
                            "cnAttribute",
                            "sAMAccName",
                            "interDomainAccType",
                            "accControl"}, new object[] {
                            "Acc1",
                            "Acc1",
                            ProtocolMessageStructures.sAMAccountType.NotSet,
                            ProtocolMessageStructures.userAccountControl.NotSet}));
            this.Manager.Comment("reaching state \'S95\'");
            this.Manager.Comment("checking step \'return CreateTrustedAccounts\'");
            this.Manager.Comment("reaching state \'S111\'");
            bool temp20;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp20 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Comment("reaching state \'S120\'");
            this.Manager.Comment("checking step \'return TDOOperation/False\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, temp20, "return of TDOOperation, state S120");
            this.Manager.Comment("reaching state \'S129\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario2_2K8R2S113();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario2_2K8R2S4()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario2_2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S34\'");
            bool temp21;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_INBO" +
                    "UND,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp21 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.NotSet,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_INBOUND,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S50\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp21, "return of SetInformationTDO, state S50");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("executing step \'call CreateTrustedAccounts(\"TrustObject1\",InterDomain_Trust_Info(" +
                    "cnAttribute=\"Acc1\",sAMAccName=\"Acc1\",interDomainAccType=SAM_TRUST_ACCOUNT,accCon" +
                    "trol=ADS_UF_INTERDOMAIN_TRUST_ACCOUNT))\'");
            this.IMS_ADTS_TDOAdapterInstance.CreateTrustedAccounts("TrustObject1", this.Make<ProtocolMessageStructures.InterDomain_Trust_Info>(new string[] {
                            "cnAttribute",
                            "sAMAccName",
                            "interDomainAccType",
                            "accControl"}, new object[] {
                            "Acc1",
                            "Acc1",
                            ProtocolMessageStructures.sAMAccountType.SAM_TRUST_ACCOUNT,
                            ProtocolMessageStructures.userAccountControl.ADS_UF_INTERDOMAIN_TRUST_ACCOUNT}));
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("checking step \'return CreateTrustedAccounts\'");
            this.Manager.Comment("reaching state \'S98\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario2_2K8R2S113();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario2_2K8R2S6()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario2_2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S35\'");
            bool temp22;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_INBO" +
                    "UND,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp22 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.NotSet,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_INBOUND,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp22, "return of SetInformationTDO, state S51");
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("executing step \'call CreateTrustedAccounts(\"TrustObject1\",InterDomain_Trust_Info(" +
                    "cnAttribute=\"Acc1\",sAMAccName=\"Acc1\",interDomainAccType=SAM_TRUST_ACCOUNT,accCon" +
                    "trol=NotSet))\'");
            this.IMS_ADTS_TDOAdapterInstance.CreateTrustedAccounts("TrustObject1", this.Make<ProtocolMessageStructures.InterDomain_Trust_Info>(new string[] {
                            "cnAttribute",
                            "sAMAccName",
                            "interDomainAccType",
                            "accControl"}, new object[] {
                            "Acc1",
                            "Acc1",
                            ProtocolMessageStructures.sAMAccountType.SAM_TRUST_ACCOUNT,
                            ProtocolMessageStructures.userAccountControl.NotSet}));
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("checking step \'return CreateTrustedAccounts\'");
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario2_2K8R2S113();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("TDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario2_2K8R2S8()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario2_2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S36\'");
            bool temp23;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_INBO" +
                    "UND,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp23 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.NotSet,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_INBOUND,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp23, "return of SetInformationTDO, state S52");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("executing step \'call CreateTrustedAccounts(\"TrustObject1\",InterDomain_Trust_Info(" +
                    "cnAttribute=\"Acc1\",sAMAccName=\"Acc1\",interDomainAccType=NotSet,accControl=ADS_UF" +
                    "_INTERDOMAIN_TRUST_ACCOUNT))\'");
            this.IMS_ADTS_TDOAdapterInstance.CreateTrustedAccounts("TrustObject1", this.Make<ProtocolMessageStructures.InterDomain_Trust_Info>(new string[] {
                            "cnAttribute",
                            "sAMAccName",
                            "interDomainAccType",
                            "accControl"}, new object[] {
                            "Acc1",
                            "Acc1",
                            ProtocolMessageStructures.sAMAccountType.NotSet,
                            ProtocolMessageStructures.userAccountControl.ADS_UF_INTERDOMAIN_TRUST_ACCOUNT}));
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("checking step \'return CreateTrustedAccounts\'");
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario2_2K8R2S113();
            this.Manager.EndTest();
        }
        #endregion
    }
}
