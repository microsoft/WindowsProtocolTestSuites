// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    [TestClass]
    public class DrsrLDSDemotionTests : DrsrTestClassBase
    {
        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static new void ClassInitialize(TestContext context)
        {
            DrsrTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            DrsrTestClassBase.BaseCleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        /// <summary>
        /// Basic DRS test for IDL_DRSInitDemotion and IDL_DRSFinishDemotion
        /// </summary>
        [BVT]
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.LDS)]
        [Priority(0)]
        [RequireDcPartner]
        [Ignore]
        [Description("Basic DRS test for IDL_DRSInitDemotion and IDL_DRSFinishDemotion")]
        [BreakEnvironment]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSLDSDemotion_V1_Success()
        {
            DrsrTestChecker.Check();
            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            drsTestClient.DrsInitDemotion(EnvironmentConfig.Machine.WritableDC1);

            drsTestClient.DrsFinishDemotion(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS.DS_DEMOTE_COMMIT_DEMOTE | DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS.DS_DEMOTE_DELETE_CSMETA | DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS.DS_DEMOTE_UNREGISTER_SCPS | DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS.DS_DEMOTE_UNREGISTER_SPNS, EnvironmentConfig.Machine.WritableDC2);
        }

        /// <summary>
        /// Basic DRS test for IDL_DRSInitDemotion and IDL_DRSFinishDemotion
        /// </summary>
        [BVT]
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.LDS)]
        [Priority(0)]
        [Ignore]
        [RequireDcPartner]
        [Description("Basic rollback test for IDL_DRSInitDemotion and IDL_DRSFinishDemotion")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSLDSDemotion_V1_Rollback_Success()
        {
            DrsrTestChecker.Check();
            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            drsTestClient.DrsInitDemotion(EnvironmentConfig.Machine.WritableDC1);

            drsTestClient.DrsFinishDemotion(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS.DS_DEMOTE_COMMIT_DEMOTE | DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS.DS_DEMOTE_DELETE_CSMETA | DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS.DS_DEMOTE_UNREGISTER_SCPS | DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS.DS_DEMOTE_UNREGISTER_SPNS | DRS_MSG_FINISH_DEMOTIONREQ_OPERATIONS.DS_DEMOTE_ROLLBACK_DEMOTE, EnvironmentConfig.Machine.WritableDC2);
        }
    }


}
