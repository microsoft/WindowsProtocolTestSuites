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
        [TestCategory("RDP8.1")]
        [TestCategory("RDPBCGR")]
        [TestCategory("HeartBeat")]
        [Description(@"This test verify client can receive Server Heartbeat PDU, and will auto-reconnect if not receive a heartbeat after certain time")]
        public void BVT_HealthMonitoring_PositiveTest_SendHeartbeat()
        {
            #region Test Description
            /*
            This test case is used to verify client can receive Server Heartbeat PDU, and will auto-reconnect if not receive a heartbeat after certain time. 
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. In Capability Exchange phase, Test Suite sets the flags field to VCCAPS_NO_COMPR (0x00000000) flag in Virtual Channel Capability Set.
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on (how to determine the notification type is described in TD section 3.3.5.10) . 
            3.  Test Suite sends SUT a Save Session Info PDU with a notification type of Logon Info Extended which presents a Server Auto-Reconnect Packet.
            4.  Test Suite sends a Heartbeat PDU
            5.  Test Suite doesn't send Heartbeat PDU again and wait client to reconnect.            
            */
            #endregion

            #region Test Sequence
            this.TestSite.Assert.IsTrue(isClientSupportRDPEFS, "To execute test cases of S7, RDP client should support [MS-RDPEFS]: Remote Desktop Protocol: File System Virtual Channel Extension.");
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            #region First connection
            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Virtual Channel compression is not supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, false, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            //Check whether the client support Heartbeat PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Check whether the client support Heartbeat PDU");
            this.rdpbcgrAdapter.CheckHeartbeatSupport();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT with Auto-Reconnect information.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.AutoReconnectCookie, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion First connection

            #region Send Heartbeat PDU
            //Send a Server Heartbeat PDU
            byte period = 1;
            byte warningCount = 1;
            byte reconnectCount = 2;
            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending a heartbeat PDU: Period={0} s; Warning count={1}; Reconnect count={2}.", period, warningCount, reconnectCount);
            this.rdpbcgrAdapter.SendServerHeartbeatPDU(period, warningCount, reconnectCount);
            #endregion Send Heartbeat PDU

            #region Reconnect

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.AutoReconnection);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability and advertising the support for Auto_Reconnect.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting an Auto-Reconnect sequence.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, true, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion Reconnect

            #endregion
        }
    }
}
