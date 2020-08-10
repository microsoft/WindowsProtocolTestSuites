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
    public partial class TCTDOOperation_Scenario3_2K8R2 : PtfTestClassBase
    {

        public TCTDOOperation_Scenario3_2K8R2()
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
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S0()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S40\'");
            bool temp0;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=TACOAndTAWF,TrustDir=TRUST_DIRECTION" +
                    "_DISABLED,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
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
                            ProtocolMessageStructures.TrustAttributesEnum.TACOAndTAWF,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R750");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/False\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, temp0, "return of SetInformationTDO, state S60");
            TCTDOOperation_Scenario3_2K8R2S80();
            this.Manager.EndTest();
        }

        private void TCTDOOperation_Scenario3_2K8R2S80()
        {
            this.Manager.Comment("reaching state \'S80\'");
        }
        #endregion

        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S10()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S45\'");
            bool temp1;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject1"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO1"",TrustPartner=""Domain1"",TrustAttr=TRUST_ATTRIBUTE_USES_RC4_ENCRYPTION,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID1"",TrustType=NotSet,EncryptionType=NotSet))'");
            temp1 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_USES_RC4_ENCRYPTION,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp1, "return of SetInformationTDO, state S65");
            this.Manager.Comment("reaching state \'S85\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }

        private void TCTDOOperation_Scenario3_2K8R2S99()
        {
            this.Manager.Comment("reaching state \'S99\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCTDOOperation_Scenario3_2K8R2.DeleteTDOInfo, null, new DeleteTDODelegate1(this.TCTDOOperation_Scenario3_2K8R2S10DeleteTDOChecker)));
            this.Manager.Comment("reaching state \'S109\'");
        }

        private void TCTDOOperation_Scenario3_2K8R2S10DeleteTDOChecker()
        {
            this.Manager.Comment("checking step \'return DeleteTDO\'");
        }
        #endregion

        #region Test Starting in S12
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S12()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S12");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S46\'");
            bool temp2;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject1"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO1"",TrustPartner=""Domain1"",TrustAttr=TRUST_ATTRIBUTE_QUARANTINED_DOMAIN,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID1"",TrustType=NotSet,EncryptionType=NotSet))'");
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
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_QUARANTINED_DOMAIN,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S66\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp2, "return of SetInformationTDO, state S66");
            this.Manager.Comment("reaching state \'S86\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S14
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S14()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S14");
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S15\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S47\'");
            bool temp3;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=TRUST_ATTRIBUTE_UPLEVEL_ONLY,TrustDi" +
                    "r=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionTyp" +
                    "e=NotSet))\'");
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
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_UPLEVEL_ONLY,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S67\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp3, "return of SetInformationTDO, state S67");
            this.Manager.Comment("reaching state \'S87\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S16
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S16()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S16");
            this.Manager.Comment("reaching state \'S16\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S17\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S48\'");
            bool temp4;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject1"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO1"",TrustPartner=""Domain1"",TrustAttr=TRUST_ATTRIBUTE_CROSS_ORGANIZATION,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID1"",TrustType=NotSet,EncryptionType=NotSet))'");
            temp4 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_CROSS_ORGANIZATION,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S68\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp4, "return of SetInformationTDO, state S68");
            this.Manager.Comment("reaching state \'S88\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S18
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S18()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S18");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S49\'");
            bool temp5;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=TRUST_ATTRIBUTE_NON_TRANSITIVE,Trust" +
                    "Dir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionT" +
                    "ype=NotSet))\'");
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
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_NON_TRANSITIVE,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S69\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp5, "return of SetInformationTDO, state S69");
            this.Manager.Comment("reaching state \'S89\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S2()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S41\'");
            bool temp6;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject1"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO1"",TrustPartner=""Domain1"",TrustAttr=TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID1"",TrustType=NotSet,EncryptionType=NotSet))'");
            temp6 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp6, "return of SetInformationTDO, state S61");
            this.Manager.Comment("reaching state \'S81\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S20
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S20()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S20");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S50\'");
            bool temp7;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=TAFTAndTAWF,TrustDir=TRUST_DIRECTION" +
                    "_DISABLED,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
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
                            ProtocolMessageStructures.TrustAttributesEnum.TAFTAndTAWF,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R750");
            this.Manager.Comment("reaching state \'S70\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/False\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, temp7, "return of SetInformationTDO, state S70");
            TCTDOOperation_Scenario3_2K8R2S80();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S22
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S22()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S22");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S51\'");
            bool temp8;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject1"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO1"",TrustPartner=""Domain1"",TrustAttr=TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID1"",TrustType=NotSet,EncryptionType=NotSet))'");
            temp8 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_TREAT_AS_EXTERNAL,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S71\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp8, "return of SetInformationTDO, state S71");
            this.Manager.Comment("reaching state \'S90\'");
            bool temp9;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp9 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Comment("reaching state \'S100\'");
            this.Manager.Comment("checking step \'return TDOOperation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp9, "return of TDOOperation, state S100");
            this.Manager.Comment("reaching state \'S110\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S24
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S24()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S24");
            this.Manager.Comment("reaching state \'S24\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S25\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S52\'");
            bool temp10;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=R,TrustDir=TRUST_DIRECTION_DISABLED," +
                    "TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
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
                            ProtocolMessageStructures.TrustAttributesEnum.R,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S72\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp10, "return of SetInformationTDO, state S72");
            this.Manager.Comment("reaching state \'S91\'");
            bool temp11;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp11 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Comment("reaching state \'S101\'");
            this.Manager.Comment("checking step \'return TDOOperation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp11, "return of TDOOperation, state S101");
            this.Manager.Comment("reaching state \'S111\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S26
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S26()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S26");
            this.Manager.Comment("reaching state \'S26\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S27\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S53\'");
            bool temp12;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=O,TrustDir=TRUST_DIRECTION_DISABLED," +
                    "TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
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
                            ProtocolMessageStructures.TrustAttributesEnum.O,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S73\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp12, "return of SetInformationTDO, state S73");
            this.Manager.Comment("reaching state \'S92\'");
            bool temp13;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp13 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Comment("reaching state \'S102\'");
            this.Manager.Comment("checking step \'return TDOOperation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp13, "return of TDOOperation, state S102");
            this.Manager.Comment("reaching state \'S112\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S28
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S28()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S28");
            this.Manager.Comment("reaching state \'S28\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S29\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S54\'");
            bool temp14;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=TRUST_ATTRIBUTE_WITHIN_FOREST,TrustD" +
                    "ir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionTy" +
                    "pe=NotSet))\'");
            temp14 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_WITHIN_FOREST,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S74\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp14, "return of SetInformationTDO, state S74");
            this.Manager.Comment("reaching state \'S93\'");
            bool temp15;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp15 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Comment("reaching state \'S103\'");
            this.Manager.Comment("checking step \'return TDOOperation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp15, "return of TDOOperation, state S103");
            this.Manager.Comment("reaching state \'S113\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S30
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S30()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S30");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S55\'");
            bool temp16;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject1"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO1"",TrustPartner=""Domain1"",TrustAttr=TRUST_ATTRIBUTE_USES_RC4_ENCRYPTION,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID1"",TrustType=NotSet,EncryptionType=NotSet))'");
            temp16 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_USES_RC4_ENCRYPTION,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S75\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp16, "return of SetInformationTDO, state S75");
            this.Manager.Comment("reaching state \'S94\'");
            bool temp17;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp17 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Comment("reaching state \'S104\'");
            this.Manager.Comment("checking step \'return TDOOperation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp17, "return of TDOOperation, state S104");
            this.Manager.Comment("reaching state \'S114\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S32
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S32()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S32");
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S56\'");
            bool temp18;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject1"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO1"",TrustPartner=""Domain1"",TrustAttr=TRUST_ATTRIBUTE_QUARANTINED_DOMAIN,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID1"",TrustType=NotSet,EncryptionType=NotSet))'");
            temp18 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_QUARANTINED_DOMAIN,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S76\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp18, "return of SetInformationTDO, state S76");
            this.Manager.Comment("reaching state \'S95\'");
            bool temp19;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp19 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Comment("reaching state \'S105\'");
            this.Manager.Comment("checking step \'return TDOOperation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp19, "return of TDOOperation, state S105");
            this.Manager.Comment("reaching state \'S115\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S34
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S34()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S34");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S57\'");
            bool temp20;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=TRUST_ATTRIBUTE_UPLEVEL_ONLY,TrustDi" +
                    "r=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionTyp" +
                    "e=NotSet))\'");
            temp20 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_UPLEVEL_ONLY,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S77\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp20, "return of SetInformationTDO, state S77");
            this.Manager.Comment("reaching state \'S96\'");
            bool temp21;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp21 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Comment("reaching state \'S106\'");
            this.Manager.Comment("checking step \'return TDOOperation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp21, "return of TDOOperation, state S106");
            this.Manager.Comment("reaching state \'S116\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S36
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S36()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S36");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S37\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S58\'");
            bool temp22;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject1"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO1"",TrustPartner=""Domain1"",TrustAttr=TRUST_ATTRIBUTE_CROSS_ORGANIZATION,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID1"",TrustType=NotSet,EncryptionType=NotSet))'");
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
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_CROSS_ORGANIZATION,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S78\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp22, "return of SetInformationTDO, state S78");
            this.Manager.Comment("reaching state \'S97\'");
            bool temp23;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp23 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Comment("reaching state \'S107\'");
            this.Manager.Comment("checking step \'return TDOOperation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp23, "return of TDOOperation, state S107");
            this.Manager.Comment("reaching state \'S117\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S38
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S38()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S38");
            this.Manager.Comment("reaching state \'S38\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S39\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S59\'");
            bool temp24;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=TRUST_ATTRIBUTE_NON_TRANSITIVE,Trust" +
                    "Dir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionT" +
                    "ype=NotSet))\'");
            temp24 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_NON_TRANSITIVE,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S79\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp24, "return of SetInformationTDO, state S79");
            this.Manager.Comment("reaching state \'S98\'");
            bool temp25;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp25 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Comment("reaching state \'S108\'");
            this.Manager.Comment("checking step \'return TDOOperation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp25, "return of TDOOperation, state S108");
            this.Manager.Comment("reaching state \'S118\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S4()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S42\'");
            bool temp26;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=R,TrustDir=TRUST_DIRECTION_DISABLED," +
                    "TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp26 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.R,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp26, "return of SetInformationTDO, state S62");
            this.Manager.Comment("reaching state \'S82\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S6()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S43\'");
            bool temp27;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=O,TrustDir=TRUST_DIRECTION_DISABLED," +
                    "TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionType=NotSet))\'");
            temp27 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.O,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp27, "return of SetInformationTDO, state S63");
            this.Manager.Comment("reaching state \'S83\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario3_2K8R2S8()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario3_2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S44\'");
            bool temp28;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=TRUST_ATTRIBUTE_WITHIN_FOREST,TrustD" +
                    "ir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=\"SID1\",TrustType=NotSet,EncryptionTy" +
                    "pe=NotSet))\'");
            temp28 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject1", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO1",
                            "Domain1",
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_WITHIN_FOREST,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.NotSet,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp28, "return of SetInformationTDO, state S64");
            this.Manager.Comment("reaching state \'S84\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario3_2K8R2S99();
            this.Manager.EndTest();
        }
        #endregion
    }
}
