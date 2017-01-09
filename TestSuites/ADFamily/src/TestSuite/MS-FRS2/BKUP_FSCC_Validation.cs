// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.SpecExplorer.Runtime;
using System.Reflection;
using Microsoft.SpecExplorer;
using FRS2Model;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.MS_FRS2
{
    [TestClass]
    public partial class BKUP_FSCC_Validation : TestClassBase
    {
        #region Variables

        IFRS2ManagedAdapter iAdapter;

        #endregion
                
        #region Test Suite Initialization

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
            ITestSite testSite = TestClassBase.BaseTestSite;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        #endregion

        #region Test Case Initialization

        protected override void TestInitialize()
        {
            // put here code which shall be run before every test case execution,
            // e.g. initialization of adapters:
            // protocolAdapter = Site.GetAdapter<IProtocolAdapter>();
            iAdapter = Site.GetAdapter<IFRS2ManagedAdapter>();
            FRS2ManagedAdapter.PreCheck();
            iAdapter.GeneralInitialize();
        }

        protected override void TestCleanup()
        {
            // put here code which shall be run after every test case execution,
            // e.g. reseting of adapters:
            // protocolAdapter.Reset(); 
            iAdapter.Reset();
        }

        #endregion

        #region Test Cases

        [TestCategory("SDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestMethod]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("AD")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategory("MS-FRS2")]
        public void FRS2_ValidateFileStreamData()
        {
            iAdapter.SetTraditionalTestFlag();

            iAdapter.CheckConnectivity("P", 5);
            ProtocolVersionReturned protocolVersion;
            UpstreamFlagValueReturned upStreamFlags;
            iAdapter.EstablishConnection("P", 5, ProtocolVersion.FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER, out protocolVersion, out upStreamFlags);
            iAdapter.EstablishSession(5, 6);
            iAdapter.AsyncPoll(5);
            iAdapter.RequestVersionVector(22, 5, 6, VERSION_REQUEST_TYPE.REQUEST_NORMAL_SYNC, VERSION_CHANGE_TYPE.CHANGE_ALL, VVGeneration.ValidValue);
            iAdapter.AsyncPoll(5);

            iAdapter.BkupFsccValidation();
        }

        #endregion

    }        
}
