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
    public partial class TCTDOOperation_Scenario6_2K8R2 : PtfTestClassBase
    {

        public TCTDOOperation_Scenario6_2K8R2()
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
        public void Security_TCTDOOperation_Scenario6_2K8R2S0()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario6_2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S12\'");
            bool temp0;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject1"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO1"",TrustPartner=""Domain1"",TrustAttr=TRUST_ATTRIBUTE_FOREST_TRANSITIVE,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID1"",TrustType=TRUST_TYPE_MIT,EncryptionType=NotSet))'");
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
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_FOREST_TRANSITIVE,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.TRUST_TYPE_MIT,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S18\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp0, "return of SetInformationTDO, state S18");
            this.Manager.Comment("reaching state \'S24\'");
            bool temp1;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject2"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO2"",TrustPartner=""Domain2"",TrustAttr=TRUST_ATTRIBUTE_FOREST_TRANSITIVE,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID2"",TrustType=TRUST_TYPE_MIT,EncryptionType=NotSet))'");
            temp1 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject2", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO2",
                            "Domain2",
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_FOREST_TRANSITIVE,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID2",
                            ProtocolMessageStructures.TrustTypeEnum.TRUST_TYPE_MIT,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S30\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp1, "return of SetInformationTDO, state S30");
            this.Manager.Comment("reaching state \'S36\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.Comment("reaching state \'S42\'");
            this.Manager.Comment("checking step \'return DeleteTDO\'");
            this.Manager.Comment("reaching state \'S48\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S10
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario6_2K8R2S10()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario6_2K8R2S10");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S17\'");
            bool temp2;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject1"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO1"",TrustPartner=""Domain1"",TrustAttr=TRUST_ATTRIBUTE_FOREST_TRANSITIVE,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID1"",TrustType=TRUST_TYPE_MIT,EncryptionType=NotSet))'");
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
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_FOREST_TRANSITIVE,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.TRUST_TYPE_MIT,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S23\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp2, "return of SetInformationTDO, state S23");
            this.Manager.Comment("reaching state \'S29\'");
            bool temp3;
            this.Manager.Comment("executing step \'call TrustedForestInformation(\"TrustObject1\",Trusted_Forest_Infor" +
                    "mation(NetBiosName=\"System1\",ValueSID=\"SID1\",sdcBit=Set,ndcBit=Set,recType=Fores" +
                    "tTrustTopLevelName))\'");
            temp3 = this.IMS_ADTS_TDOAdapterInstance.TrustedForestInformation("TrustObject1", this.Make<ProtocolMessageStructures.Trusted_Forest_Information>(new string[] {
                            "NetBiosName",
                            "ValueSID",
                            "sdcBit",
                            "ndcBit",
                            "recType"}, new object[] {
                            "System1",
                            "SID1",
                            ProtocolMessageStructures.SDCFlag.Set,
                            ProtocolMessageStructures.NDCFlag.Set,
                            ProtocolMessageStructures.RecordType.ForestTrustTopLevelName}));
            this.Manager.Comment("reaching state \'S35\'");
            this.Manager.Comment("checking step \'return TrustedForestInformation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp3, "return of TrustedForestInformation, state S35");
            this.Manager.Comment("reaching state \'S41\'");
            bool temp4;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject2"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO2"",TrustPartner=""Domain2"",TrustAttr=TRUST_ATTRIBUTE_FOREST_TRANSITIVE,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID2"",TrustType=TRUST_TYPE_MIT,EncryptionType=NotSet))'");
            temp4 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject2", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO2",
                            "Domain2",
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_FOREST_TRANSITIVE,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID2",
                            ProtocolMessageStructures.TrustTypeEnum.TRUST_TYPE_MIT,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S47\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp4, "return of SetInformationTDO, state S47");
            this.Manager.Comment("reaching state \'S53\'");
            bool temp5;
            this.Manager.Comment("executing step \'call TrustedForestInformation(\"TrustObject2\",Trusted_Forest_Infor" +
                    "mation(NetBiosName=\"System2\",ValueSID=\"SID1\",sdcBit=NotSet,ndcBit=Set,recType=Fo" +
                    "restTrustTopLevelName))\'");
            temp5 = this.IMS_ADTS_TDOAdapterInstance.TrustedForestInformation("TrustObject2", this.Make<ProtocolMessageStructures.Trusted_Forest_Information>(new string[] {
                            "NetBiosName",
                            "ValueSID",
                            "sdcBit",
                            "ndcBit",
                            "recType"}, new object[] {
                            "System2",
                            "SID1",
                            ProtocolMessageStructures.SDCFlag.NotSet,
                            ProtocolMessageStructures.NDCFlag.Set,
                            ProtocolMessageStructures.RecordType.ForestTrustTopLevelName}));
            this.Manager.Comment("reaching state \'S58\'");
            this.Manager.Comment("checking step \'return TrustedForestInformation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp5, "return of TrustedForestInformation, state S58");
            this.Manager.Comment("reaching state \'S63\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario6_2K8R2S54();
            this.Manager.EndTest();
        }

        private void TCTDOOperation_Scenario6_2K8R2S54()
        {
            this.Manager.Comment("reaching state \'S54\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCTDOOperation_Scenario6_2K8R2.DeleteTDOInfo, null, new DeleteTDODelegate1(this.TCTDOOperation_Scenario6_2K8R2S10DeleteTDOChecker)));
            this.Manager.Comment("reaching state \'S59\'");
        }

        private void TCTDOOperation_Scenario6_2K8R2S10DeleteTDOChecker()
        {
            this.Manager.Comment("checking step \'return DeleteTDO\'");
        }
        #endregion

        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario6_2K8R2S2()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario6_2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S13\'");
            bool temp6;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject1"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO1"",TrustPartner=""Domain1"",TrustAttr=TRUST_ATTRIBUTE_FOREST_TRANSITIVE,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID1"",TrustType=TRUST_TYPE_MIT,EncryptionType=NotSet))'");
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
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_FOREST_TRANSITIVE,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.TRUST_TYPE_MIT,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S19\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp6, "return of SetInformationTDO, state S19");
            this.Manager.Comment("reaching state \'S25\'");
            bool temp7;
            this.Manager.Comment("executing step \'call TrustedForestInformation(\"TrustObject1\",Trusted_Forest_Infor" +
                    "mation(NetBiosName=\"System1\",ValueSID=\"SID1\",sdcBit=Set,ndcBit=Set,recType=Fores" +
                    "tTrustTopLevelName))\'");
            temp7 = this.IMS_ADTS_TDOAdapterInstance.TrustedForestInformation("TrustObject1", this.Make<ProtocolMessageStructures.Trusted_Forest_Information>(new string[] {
                            "NetBiosName",
                            "ValueSID",
                            "sdcBit",
                            "ndcBit",
                            "recType"}, new object[] {
                            "System1",
                            "SID1",
                            ProtocolMessageStructures.SDCFlag.Set,
                            ProtocolMessageStructures.NDCFlag.Set,
                            ProtocolMessageStructures.RecordType.ForestTrustTopLevelName}));
            this.Manager.Comment("reaching state \'S31\'");
            this.Manager.Comment("checking step \'return TrustedForestInformation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp7, "return of TrustedForestInformation, state S31");
            this.Manager.Comment("reaching state \'S37\'");
            bool temp8;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject2"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO2"",TrustPartner=""Domain2"",TrustAttr=TRUST_ATTRIBUTE_FOREST_TRANSITIVE,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID2"",TrustType=TRUST_TYPE_MIT,EncryptionType=NotSet))'");
            temp8 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject2", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO2",
                            "Domain2",
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_FOREST_TRANSITIVE,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID2",
                            ProtocolMessageStructures.TrustTypeEnum.TRUST_TYPE_MIT,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S43\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp8, "return of SetInformationTDO, state S43");
            this.Manager.Comment("reaching state \'S49\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario6_2K8R2S54();
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S4
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario6_2K8R2S4()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario6_2K8R2S4");
            this.Manager.Comment("reaching state \'S4\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S14\'");
            bool temp9;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject1"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO1"",TrustPartner=""Domain1"",TrustAttr=TRUST_ATTRIBUTE_FOREST_TRANSITIVE,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID1"",TrustType=TRUST_TYPE_MIT,EncryptionType=NotSet))'");
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
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_FOREST_TRANSITIVE,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.TRUST_TYPE_MIT,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S20\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp9, "return of SetInformationTDO, state S20");
            this.Manager.Comment("reaching state \'S26\'");
            bool temp10;
            this.Manager.Comment("executing step \'call TrustedForestInformation(\"TrustObject1\",Trusted_Forest_Infor" +
                    "mation(NetBiosName=\"System1\",ValueSID=\"SID1\",sdcBit=Set,ndcBit=Set,recType=Fores" +
                    "tTrustTopLevelName))\'");
            temp10 = this.IMS_ADTS_TDOAdapterInstance.TrustedForestInformation("TrustObject1", this.Make<ProtocolMessageStructures.Trusted_Forest_Information>(new string[] {
                            "NetBiosName",
                            "ValueSID",
                            "sdcBit",
                            "ndcBit",
                            "recType"}, new object[] {
                            "System1",
                            "SID1",
                            ProtocolMessageStructures.SDCFlag.Set,
                            ProtocolMessageStructures.NDCFlag.Set,
                            ProtocolMessageStructures.RecordType.ForestTrustTopLevelName}));
            this.Manager.Comment("reaching state \'S32\'");
            this.Manager.Comment("checking step \'return TrustedForestInformation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp10, "return of TrustedForestInformation, state S32");
            this.Manager.Comment("reaching state \'S38\'");
            bool temp11;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject2"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO2"",TrustPartner=""Domain2"",TrustAttr=TRUST_ATTRIBUTE_FOREST_TRANSITIVE,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID2"",TrustType=TRUST_TYPE_MIT,EncryptionType=NotSet))'");
            temp11 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject2", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO2",
                            "Domain2",
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_FOREST_TRANSITIVE,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID2",
                            ProtocolMessageStructures.TrustTypeEnum.TRUST_TYPE_MIT,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S44\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp11, "return of SetInformationTDO, state S44");
            this.Manager.Comment("reaching state \'S50\'");
            bool temp12;
            this.Manager.Comment("executing step \'call TrustedForestInformation(\"TrustObject2\",Trusted_Forest_Infor" +
                    "mation(NetBiosName=\"System2\",ValueSID=\"SID1\",sdcBit=Set,ndcBit=Set,recType=Fores" +
                    "tTrustTopLevelName))\'");
            temp12 = this.IMS_ADTS_TDOAdapterInstance.TrustedForestInformation("TrustObject2", this.Make<ProtocolMessageStructures.Trusted_Forest_Information>(new string[] {
                            "NetBiosName",
                            "ValueSID",
                            "sdcBit",
                            "ndcBit",
                            "recType"}, new object[] {
                            "System2",
                            "SID1",
                            ProtocolMessageStructures.SDCFlag.Set,
                            ProtocolMessageStructures.NDCFlag.Set,
                            ProtocolMessageStructures.RecordType.ForestTrustTopLevelName}));
            this.Manager.Comment("reaching state \'S55\'");
            this.Manager.Comment("checking step \'return TrustedForestInformation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp12, "return of TrustedForestInformation, state S55");
            this.Manager.Comment("reaching state \'S60\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.Comment("reaching state \'S64\'");
            this.Manager.Comment("checking step \'return DeleteTDO\'");
            this.Manager.Comment("reaching state \'S65\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S6
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario6_2K8R2S6()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario6_2K8R2S6");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S15\'");
            bool temp13;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject1"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO1"",TrustPartner=""Domain1"",TrustAttr=TRUST_ATTRIBUTE_FOREST_TRANSITIVE,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID1"",TrustType=TRUST_TYPE_MIT,EncryptionType=NotSet))'");
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
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_FOREST_TRANSITIVE,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.TRUST_TYPE_MIT,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S21\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp13, "return of SetInformationTDO, state S21");
            this.Manager.Comment("reaching state \'S27\'");
            bool temp14;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject2"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO2"",TrustPartner=""Domain2"",TrustAttr=TRUST_ATTRIBUTE_FOREST_TRANSITIVE,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID2"",TrustType=TRUST_TYPE_MIT,EncryptionType=NotSet))'");
            temp14 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject2", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO2",
                            "Domain2",
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_FOREST_TRANSITIVE,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID2",
                            ProtocolMessageStructures.TrustTypeEnum.TRUST_TYPE_MIT,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S33\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp14, "return of SetInformationTDO, state S33");
            this.Manager.Comment("reaching state \'S39\'");
            bool temp15;
            this.Manager.Comment("executing step \'call TrustedForestInformation(\"TrustObject2\",Trusted_Forest_Infor" +
                    "mation(NetBiosName=\"System2\",ValueSID=\"SID1\",sdcBit=Set,ndcBit=Set,recType=Fores" +
                    "tTrustTopLevelName))\'");
            temp15 = this.IMS_ADTS_TDOAdapterInstance.TrustedForestInformation("TrustObject2", this.Make<ProtocolMessageStructures.Trusted_Forest_Information>(new string[] {
                            "NetBiosName",
                            "ValueSID",
                            "sdcBit",
                            "ndcBit",
                            "recType"}, new object[] {
                            "System2",
                            "SID1",
                            ProtocolMessageStructures.SDCFlag.Set,
                            ProtocolMessageStructures.NDCFlag.Set,
                            ProtocolMessageStructures.RecordType.ForestTrustTopLevelName}));
            this.Manager.Comment("reaching state \'S45\'");
            this.Manager.Comment("checking step \'return TrustedForestInformation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp15, "return of TrustedForestInformation, state S45");
            this.Manager.Comment("reaching state \'S51\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.Comment("reaching state \'S56\'");
            this.Manager.Comment("checking step \'return DeleteTDO\'");
            this.Manager.Comment("reaching state \'S61\'");
            this.Manager.EndTest();
        }
        #endregion

        #region Test Starting in S8
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario6_2K8R2S8()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario6_2K8R2S8");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S9\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S16\'");
            bool temp16;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject1"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO1"",TrustPartner=""Domain1"",TrustAttr=TRUST_ATTRIBUTE_FOREST_TRANSITIVE,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID1"",TrustType=TRUST_TYPE_MIT,EncryptionType=NotSet))'");
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
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_FOREST_TRANSITIVE,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.TRUST_TYPE_MIT,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S22\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp16, "return of SetInformationTDO, state S22");
            this.Manager.Comment("reaching state \'S28\'");
            bool temp17;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject2"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO2"",TrustPartner=""Domain2"",TrustAttr=TRUST_ATTRIBUTE_FOREST_TRANSITIVE,TrustDir=TRUST_DIRECTION_DISABLED,TrustDomain_Sid=""SID2"",TrustType=TRUST_TYPE_MIT,EncryptionType=NotSet))'");
            temp17 = this.IMS_ADTS_TDOAdapterInstance.SetInformationTDO("TrustObject2", this.Make<ProtocolMessageStructures.TRUSTED_DOMAIN_OBJECT>(new string[] {
                            "FlatName",
                            "TrustPartner",
                            "TrustAttr",
                            "TrustDir",
                            "TrustDomain_Sid",
                            "TrustType",
                            "EncryptionType"}, new object[] {
                            "TDO2",
                            "Domain2",
                            ProtocolMessageStructures.TrustAttributesEnum.TRUST_ATTRIBUTE_FOREST_TRANSITIVE,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID2",
                            ProtocolMessageStructures.TrustTypeEnum.TRUST_TYPE_MIT,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S34\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp17, "return of SetInformationTDO, state S34");
            this.Manager.Comment("reaching state \'S40\'");
            bool temp18;
            this.Manager.Comment("executing step \'call TrustedForestInformation(\"TrustObject2\",Trusted_Forest_Infor" +
                    "mation(NetBiosName=\"System2\",ValueSID=\"SID1\",sdcBit=NotSet,ndcBit=Set,recType=Fo" +
                    "restTrustTopLevelName))\'");
            temp18 = this.IMS_ADTS_TDOAdapterInstance.TrustedForestInformation("TrustObject2", this.Make<ProtocolMessageStructures.Trusted_Forest_Information>(new string[] {
                            "NetBiosName",
                            "ValueSID",
                            "sdcBit",
                            "ndcBit",
                            "recType"}, new object[] {
                            "System2",
                            "SID1",
                            ProtocolMessageStructures.SDCFlag.NotSet,
                            ProtocolMessageStructures.NDCFlag.Set,
                            ProtocolMessageStructures.RecordType.ForestTrustTopLevelName}));
            this.Manager.Comment("reaching state \'S46\'");
            this.Manager.Comment("checking step \'return TrustedForestInformation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp18, "return of TrustedForestInformation, state S46");
            this.Manager.Comment("reaching state \'S52\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.Comment("reaching state \'S57\'");
            this.Manager.Comment("checking step \'return DeleteTDO\'");
            this.Manager.Comment("reaching state \'S62\'");
            this.Manager.EndTest();
        }
        #endregion
    }
}
