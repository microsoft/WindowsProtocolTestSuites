// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    public partial class RdpbcgrTestSuite
    {
        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case is used to verify the RDP server can send and process uncompressed Static Virtual Channel PDU.")]
        public void S5_StaticVirtualChannel_PositiveTest_CompressionNotSupported()
        {
            #region Test Steps
            //1. Initiate and complete an RDP connection to RDP Server (SUT). Create a static virtual channel with name "drdynvc", In Capability Exchange phase, Test Suite sets the flags field to VCCAPS_NO_COMPR (0x00000000) flag in Virtual Channel Capability Set.
            //2. Expect SUT to send a Static virtual channel PDU, which contains a DVC Capabilities Request PDU
            //3. Test suite responses a Static virtual channel PDU, which contains a DVC Capabilities Response PDU
            #endregion Test Steps
            #region Test Code

            this.Site.Log.Add(LogEntryKind.Comment, "Establish transport connection with RDP Server, encrypted protocol is {0}.", testConfig.transportProtocol.ToString());
            rdpbcgrAdapter.ConnectToServer(testConfig.transportProtocol);

            string[] SVCNames = new string[] { RdpConstValue.SVCNAME_RDPEDYC };
            rdpbcgrAdapter.EstablishRDPConnection(testConfig.requestProtocol, SVCNames, CompressionType.PACKET_COMPR_TYPE_NONE,
                false, // Is reconnect
                true,  // Is auto logon
                supportSVCCompression:false);

            this.Site.Log.Add(LogEntryKind.Comment, "Generate Static virtual channel traffics.");
            rdpbcgrAdapter.GenerateStaticVirtualChannelTraffics(NegativeType.None);

            #endregion Test Code
        }
    }
}