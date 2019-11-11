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
        [Description(@"This test case is used to verify SUT can send fast-path update message correctly.")]
        public void S4_Output_PositiveTest_FastPathOutput()
        {
            #region Test Steps
            //1. Initiate and complete an RDP connection with SUT. In Capability Exchange phase, Test Suite advised support fast-path output in TS_GENERAL_CAPABILITYSET.
            //2. Test Suite expects a Save Session Info PDU with a notification type of either INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the that the user has logged on
            //3. Test suite sends several Client Fast-Path Input Event PDUs, each PDU contains a TS_FP_INPUT_EVENT structure as following:
            //    Fast-Path Keyboard Event
            //    Fast-Path Mouse Event
            //    Fast-Path Extended Mouse Event
            //    Fast-Path Synchronize Event
            //    Fast-Path Unicode Keyboard Event
            //4. Test suite wait for 3 minutes, and verify each Server Fast-Path Update PDU received. The Server Fast-Path Update PDU may include:
            //    Fast-Path Orders Update
            //    Fast-Path Bitmap Update
            //    Fast-Path Palette Update
            //    Fast-Path Synchronize Update
            //    Fast-Path System Pointer Hidden Update
            //    Fast-Path System Pointer Default Update
            //    Fast-Path Pointer Position Update
            //    Fast-Path Color Pointer Update
            //    Fast-Path Cached Pointer Update
            //    Fast-Path New Pointer Update
            #endregion Test Steps

            #region Test Code

            this.Site.Log.Add(LogEntryKind.Comment, "Establish transport connection with RDP Server, encrypted protocol is {0}.", testConfig.transportProtocol.ToString());
            rdpbcgrAdapter.ConnectToServer(testConfig.transportProtocol);

            string[] SVCNames = new string[] { RdpConstValue.SVCNAME_RDPEDYC };
            rdpbcgrAdapter.EstablishRDPConnection(testConfig.requestProtocol, SVCNames, CompressionType.PACKET_COMPR_TYPE_RDP61,
                false, // Is reconnect
                true,  // Is auto logon
                supportFastPathInput:true,
                supportFastPathOutput:true);

            this.Site.Log.Add(LogEntryKind.Comment, "Send several Client Fast-Path Input Event PDUs.");
            rdpbcgrAdapter.GenerateFastPathInputs();

            this.Site.Log.Add(LogEntryKind.Comment, "Wait a period to receive and verify all fast-path update PDUs received ");
            rdpbcgrAdapter.ExpectFastpathOutputs(testConfig.timeout);

            #endregion Test Code
        }
    }
}