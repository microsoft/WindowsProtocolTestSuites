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
    public partial class TCTDOOperation_Scenario1_2K8R2 : PtfTestClassBase
    {

        public TCTDOOperation_Scenario1_2K8R2()
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
        public void Security_TCTDOOperation_Scenario1_2K8R2S0()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario1_2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S4\'");
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
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp0, "return of SetInformationTDO, state S6");
            this.Manager.Comment("reaching state \'S8\'");
            bool temp1;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp1 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Comment("reaching state \'S10\'");
            this.Manager.Comment("checking step \'return TDOOperation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp1, "return of TDOOperation, state S10");
            this.Manager.Comment("reaching state \'S12\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario1_2K8R2S14();
            this.Manager.EndTest();
        }

        private void TCTDOOperation_Scenario1_2K8R2S14()
        {
            this.Manager.Comment("reaching state \'S14\'");
            this.Manager.ExpectReturn(this.QuiescenceTimeout, true, new ExpectedReturn(TCTDOOperation_Scenario1_2K8R2.DeleteTDOInfo, null, new DeleteTDODelegate1(this.TCTDOOperation_Scenario1_2K8R2S0DeleteTDOChecker)));
            this.Manager.Comment("reaching state \'S15\'");
        }

        private void TCTDOOperation_Scenario1_2K8R2S0DeleteTDOChecker()
        {
            this.Manager.Comment("checking step \'return DeleteTDO\'");
        }
        #endregion

        #region Test Starting in S2
        [TestCategory("PDC")]
        [TestCategory("MS-ADTS-Security_ADDS")]
        [TestCategory("DomainWin2008R2")]
        [TestMethod]
        public void Security_TCTDOOperation_Scenario1_2K8R2S2()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario1_2K8R2S2");
            this.Manager.Comment("reaching state \'S2\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S5\'");
            bool temp2;
            this.Manager.Comment(@"executing step 'call SetInformationTDO(""TrustObject1"",TRUSTED_DOMAIN_OBJECT(FlatName=""TDO1"",TrustPartner=""Domain1"",TrustAttr=TRUST_ATTRIBUTE_FOREST_TRANSITIVE,TrustDir=TRUST_DIRECTION_OUTBOUND,TrustDomain_Sid=""SID1"",TrustType=TRUST_TYPE_MIT,EncryptionType=NotSet))'");
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
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_OUTBOUND,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.TRUST_TYPE_MIT,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp2, "return of SetInformationTDO, state S7");
            this.Manager.Comment("reaching state \'S9\'");
            bool temp3;
            this.Manager.Comment("executing step \'call TDOOperation(\"TrustObject1\")\'");
            temp3 = this.IMS_ADTS_TDOAdapterInstance.TDOOperation("TrustObject1");
            this.Manager.Comment("reaching state \'S11\'");
            this.Manager.Comment("checking step \'return TDOOperation/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp3, "return of TDOOperation, state S11");
            this.Manager.Comment("reaching state \'S13\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.AddReturn(DeleteTDOInfo, null);
            TCTDOOperation_Scenario1_2K8R2S14();
            this.Manager.EndTest();
        }
        #endregion
    }
}
