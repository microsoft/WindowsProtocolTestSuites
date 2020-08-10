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
    public partial class TCTDOOperation_Scenario4_2K8R2 : PtfTestClassBase
    {

        public TCTDOOperation_Scenario4_2K8R2()
        {
            this.SetSwitch("ProceedControlTimeout", "100");
            this.SetSwitch("QuiescenceTimeout", "10000");
        }

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
        public void Security_TCTDOOperation_Scenario4_2K8R2S0()
        {
            this.Manager.BeginTest("TCTDOOperation_Scenario4_2K8R2S0");
            this.Manager.Comment("reaching state \'S0\'");
            this.Manager.Comment("executing step \'call InitializeOSVersion(Windows2008R2,HIGHER_DS_BEHAVIOR_WIN2003" +
                    ")\'");
            this.IMS_ADTS_TDOAdapterInstance.InitializeOSVersion(Common.ServerVersion.Win2008R2, ProtocolMessageStructures.ForestFunctionalLevel.HIGHER_DS_BEHAVIOR_WIN2003);
            this.Manager.Comment("reaching state \'S1\'");
            this.Manager.Comment("checking step \'return InitializeOSVersion\'");
            this.Manager.Comment("reaching state \'S2\'");
            bool temp0;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject1\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO1\",TrustPartner=\"Domain1\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_DISA" +
                    "BLED,TrustDomain_Sid=\"SID1\",TrustType=TRUST_TYPE_MIT,EncryptionType=NotSet))\'");
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
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.TRUST_TYPE_MIT,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R10626");
            this.Manager.Checkpoint("MS-ADTS-Security_R600");
            this.Manager.Comment("reaching state \'S3\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/True\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, true, temp0, "return of SetInformationTDO, state S3");
            this.Manager.Comment("reaching state \'S4\'");
            bool temp1;
            this.Manager.Comment("executing step \'call SetInformationTDO(\"TrustObject2\",TRUSTED_DOMAIN_OBJECT(FlatN" +
                    "ame=\"TDO2\",TrustPartner=\"Domain2\",TrustAttr=NotSet,TrustDir=TRUST_DIRECTION_DISA" +
                    "BLED,TrustDomain_Sid=\"SID1\",TrustType=TRUST_TYPE_MIT,EncryptionType=NotSet))\'");
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
                            ProtocolMessageStructures.TrustAttributesEnum.NotSet,
                            ProtocolMessageStructures.TrustDirectionEnum.TRUST_DIRECTION_DISABLED,
                            "SID1",
                            ProtocolMessageStructures.TrustTypeEnum.TRUST_TYPE_MIT,
                            ProtocolMessageStructures.SupportedEncryptionTypes.NotSet}));
            this.Manager.Checkpoint("MS-ADTS-Security_R627");
            this.Manager.Comment("reaching state \'S5\'");
            this.Manager.Comment("checking step \'return SetInformationTDO/False\'");
            TestManagerHelpers.AssertAreEqual<bool>(this.Manager, false, temp1, "return of SetInformationTDO, state S5");
            this.Manager.Comment("reaching state \'S6\'");
            this.Manager.Comment("executing step \'call DeleteTDO()\'");
            this.IMS_ADTS_TDOAdapterInstance.DeleteTDO();
            this.Manager.Comment("reaching state \'S7\'");
            this.Manager.Comment("checking step \'return DeleteTDO\'");
            this.Manager.Comment("reaching state \'S8\'");
            this.Manager.EndTest();
        }
        #endregion
    }
}
