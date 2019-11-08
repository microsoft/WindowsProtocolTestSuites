// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using System.Globalization;

namespace Microsoft.Protocols.TestSuites.Rdpedyc
{
    [TestClass]
    public partial class RdpedycTestSuite : RdpTestClassBase
    {
        protected RdpedycAdapter rdpedycAdapter;

        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            RdpTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            RdpTestClassBase.BaseCleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();

            if (!testConfig.isEDYCSupported)
            {
                Site.Assert.Inconclusive("Skip this test case since SUT does not support RDPEDYC.");
            }

            this.rdpedycAdapter = new RdpedycAdapter(testConfig);
            this.rdpedycAdapter.Initialize(Site);
        }

        protected override void TestCleanup()
        {
            if (rdpedycAdapter != null)
            {
                rdpedycAdapter.Dispose();
            }

            base.TestCleanup();
        }
        #endregion

    }
}
