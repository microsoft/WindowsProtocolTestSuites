// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Win32;
using Microsoft.Protocols.TestSuites.Rdp;
using System.Management;

namespace Microsoft.Protocols.TestSuites.Rdpbcgr
{
    public partial class RdpbcgrTestSuite
    {
        [TestMethod]
        [Priority(0)]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPBCGR")]
        [Description(@"This test case is used to verify SUT can send Heartbeat PDU periodically to notify the connection exist. ")]
        public void S8_HealthMonitoring_PositiveTest()
        {
            string rdpVersion = this.Site.Properties["RDP.Version"];
            if (!string.IsNullOrEmpty(rdpVersion))
            {
                Version rdpVer = new Version(rdpVersion);
                Version win81Ver = new Version("8.1");

                var result = rdpVer.CompareTo(win81Ver);
                if (result < 0) // RDP Version is bellow 8.1(Windows Server 2012R2)
                {
                    Site.Assert.Inconclusive("Skip this test case as RDP in Windows Server 2012 is RDP 8.0 and not support Heartbeat PDU.");
                }
            }

            #region Test Steps
            //1. Initiate and complete an RDP connection to RDP Server (SUT). In basic exchange phase, notify the support of Heartbeat PDU in earlyCapabilityFlags of core data
            //2. Test suite expects Server Heartbeat PDU several times, verify the messages received.
            #endregion Test Steps

            #region Test Code
            this.Site.Log.Add(LogEntryKind.Comment, "To test Health Monitoring, the RDP server should be configured to support heartbeat PDU.");

            this.Site.Log.Add(LogEntryKind.Comment, "Establish transport connection with RDP Server, encrypted protocol is {0}.", transportProtocol.ToString());
            rdpbcgrAdapter.ConnectToServer(this.transportProtocol);

            string[] SVCNames = new string[] { RdpConstValue.SVCNAME_RDPEDYC };
            rdpbcgrAdapter.EstablishRDPConnection(requestProtocol, SVCNames, CompressionType.PACKET_COMPR_TYPE_NONE,
                false, // Is reconnect
                true,  // Is auto logon
                supportHeartbeatPDU: true);

            int times = 5;
            int period = 0;
            DateTime startTime = DateTime.Now;
            for (int i = 0; i < times + 1; i++)
            {
                Server_Heartbeat_PDU heartbeatPdu = rdpbcgrAdapter.ExpectPacket<Server_Heartbeat_PDU>(timeout);
                Site.Assert.IsNotNull(heartbeatPdu, "RDP Server MUST send heartbeat PDU periodly if it support heartbeat PDU.");
                period = heartbeatPdu.period;
                if (i == 0)
                {
                    startTime = DateTime.Now;
                }
            }
            DateTime stopTime = DateTime.Now;

            Site.Log.Add(LogEntryKind.Comment, "After received {0} heartbeat PDU, actual duration is {1} seconds, the period in heartbeat PDU is {2}.", times, (stopTime - startTime).TotalSeconds, period);
            #endregion Test Code
        }
    }
}