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
        [Description(@"This test case is used to ensure SUT can process the Auto-Reconnection sequence successfully. ")]
        public void S2_AutoReconnect_PositiveTest()
        {
            #region Test Steps
            //1. Initiate and complete an RDP connection with SUT. In Capability Exchange phase, Test Suite sets the AUTORECONNECT_SUPPORTED (0x0008) flag within the extraFlags field of the General Capability Set in Server Demand Active PDU.
            //2. Test Suite expects a Save Session Info PDU with a notification type of either INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the that the user has logged on. 
            //3. Test Suite expects SUT a Save Session Info PDU with a notification type of Logon Info Extended which presents a Server Auto-Reconnect Packet.
            //4. Test suite close the connection.
            //5. Test suite initiate a new connection and start an Auto-Reconnect sequence. During the reconnection sequence, Test suite sends a Client Auto-Reconnect Packet in the Client Info PDU.
            //6. Test suite expects a Save Session Info PDU with a notification type of either INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) again to notify the that the user has logged on.
            #endregion Test Steps

            #region Test Code


            this.Site.Log.Add(LogEntryKind.Comment, "Establish transport connection with RDP Server, encrypted protocol is {0}.", testConfig.transportProtocol.ToString());
            rdpbcgrAdapter.ConnectToServer(testConfig.transportProtocol);

            string[] SVCNames = new string[] { RdpConstValue.SVCNAME_RDPEDYC };
            rdpbcgrAdapter.EstablishRDPConnection(testConfig.requestProtocol, SVCNames, CompressionType.PACKET_COMPR_TYPE_RDP61,
                false, // Is reconnect
                true,  // Is auto logon
                supportAutoReconnect: true);

            this.Site.Assume.IsTrue(rdpbcgrAdapter.IsServerSupportAutoReconnect(), "To run test case for auto reconnect, the RDP server should be configured to support auto reconnect.");

            this.Site.Log.Add(LogEntryKind.Comment, "Wait RDP server to notify user logon.");
            rdpbcgrAdapter.WaitForLogon(testConfig.timeout, true);

            this.Site.Log.Add(LogEntryKind.Comment, "Disconnect transport layer connection with RDP server.");
            rdpbcgrAdapter.Disconnect();
            // Wait a few time before reconnect
            System.Threading.Thread.Sleep(1000);

            this.Site.Log.Add(LogEntryKind.Comment, "Establish transport connection with RDP Server, encrypted protocol is {0}.", testConfig.transportProtocol.ToString());
            rdpbcgrAdapter.ConnectToServer(testConfig.transportProtocol);

            this.Site.Log.Add(LogEntryKind.Comment, "Reconnect to RDP Server.");
            rdpbcgrAdapter.EstablishRDPConnection(testConfig.requestProtocol, SVCNames, CompressionType.PACKET_COMPR_TYPE_RDP61,
                true,  // Is reconnect
                true,  // Is auto logon
                supportAutoReconnect: true);

            this.Site.Log.Add(LogEntryKind.Comment, "Wait RDP server to notify user logon.");
            rdpbcgrAdapter.WaitForLogon(testConfig.timeout);

            #endregion Test Code
        }
    }
}