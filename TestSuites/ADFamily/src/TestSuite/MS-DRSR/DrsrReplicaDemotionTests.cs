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
    public class DrsrReplicaDemotionTests : DrsrTestClassBase
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
        /// test DRS replica demotion success for schema NC
        /// </summary>
        [BVT]
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.LDS)]
        [Priority(0)]
        [Description("Basic DRS test for IDL_DRSReplicaDemotion for schema NC")]
        [Ignore]
        [BreakEnvironment]
        [RequireDcPartner]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaDemotion_V1_Success_SchemaNC()
        {
            DrsrTestChecker.Check();

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            drsTestClient.DrsReplicaDemotion(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_REPLICA_DEMOTIONREQ_FLAGS.DS_REPLICA_DEMOTE_TRY_ALL_SRCS, NamingContext.SchemaNC, EnvironmentConfig.Machine.WritableDC2);
        }

        /// <summary>
        /// test DRS replica demotion success for config NC
        /// </summary>
        [BVT]
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.LDS)]
        [Priority(0)]
        [Description("Basic DRS test for IDL_DRSReplicaDemotion for config NC")]
        [Ignore]
        [BreakEnvironment]
        [RequireDcPartner]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaDemotion_V1_Success_ConfigNC()
        {
            DrsrTestChecker.Check();

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            drsTestClient.DrsReplicaDemotion(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_REPLICA_DEMOTIONREQ_FLAGS.DS_REPLICA_DEMOTE_TRY_ALL_SRCS, NamingContext.ConfigNC, EnvironmentConfig.Machine.WritableDC2);
        }

        /// <summary>
        /// test DRS replica demotion success for config NC
        /// </summary>
        [BVT]
        [TestCategory("Win2003,BreakEnvironment")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [Description("Basic DRS test for IDL_DRSReplicaDemotion for config NC")]
        [Ignore]
        [BreakEnvironment]
        [RequireDcPartner]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaDemotion_V1_Success_DomainNC()
        {
            DrsrTestChecker.Check();

            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            drsTestClient.DrsReplicaDemotion(EnvironmentConfig.Machine.WritableDC1, DRS_MSG_REPLICA_DEMOTIONREQ_FLAGS.DS_REPLICA_DEMOTE_TRY_ALL_SRCS, NamingContext.DomainNC, EnvironmentConfig.Machine.WritableDC2);
        }
    }


}
