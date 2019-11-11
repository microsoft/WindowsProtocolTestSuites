// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdp;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    [TestClass]
    public partial class RdpbcgrTestSuite : RdpTestClassBase
    {
        protected RdpbcgrAdapter rdpbcgrAdapter;

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
            this.rdpbcgrAdapter = new RdpbcgrAdapter(testConfig);
            this.rdpbcgrAdapter.Initialize(Site);
        }

        protected override void TestCleanup()
        {
            if (rdpbcgrAdapter != null)
            {
                rdpbcgrAdapter.Reset();
            }
            base.TestCleanup();
        }
        #endregion
                
    }
}