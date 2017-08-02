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
        [TestCategory("AutoReconnect")]
        [Description("This test case is used to ensure SUT can process Auto-Reconnection sequence successfully.")]
        public void BVT_AutoReconnect_PositiveTest()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT can process Auto-Reconnection sequence successfully.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. In Capability Exchange phase, 
                Test Suite sets the AUTORECONNECT_SUPPORTED (0x0008) flag within extraFlags field of General Capability Set in Server Demand Active PDU.
            2.	Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), 
                or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on (how to determine the notification type is described in TD section 3.3.5.10) . 
            3.	Test Suite sends SUT a Save Session Info PDU with a notification type of Logon Info Extended which presents a Server Auto-Reconnect Packet.
            4.	Trigger SUT to start an Auto-Reconnect sequence. This can be implemented by creating a short-term network failure on client side. During the reconnection sequence, 
                Test suite expects SUT presents a Client Auto-Reconnect Packet in the Client Info PDU.
            5.	Test suite does step 2 again to notify SUT the auto-reconnection has been successful.
            */
            #endregion

            #region Test Sequence

            this.TestSite.Assert.IsTrue(isClientSuportAutoReconnect, "SUT should support Auto-Reconnect.");

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            #region First Connection
            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability and set the AUTORECONNECT_SUPPORTED (0x0008) flag within extraFlags field of General Capability Set in Server Demand Active PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability and advertising the support for Auto_Reconnect.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT with Auto-Reconnect information.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.AutoReconnectCookie, ErrorNotificationType_Values.LOGON_FAILED_OTHER);
            
            #endregion

            #region Auto-Reconnect Sequence

            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to start an Auto-Reconnect.");
            int iResult = this.sutControlAdapter.TriggerClientAutoReconnect(this.TestContext.TestName);
            TestSite.Assume.IsTrue(iResult >= 0, "SUT Control Adapter: TriggerClientAutoReconnect should be successful: {0}.", iResult);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.AutoReconnection);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability and advertising the support for Auto_Reconnect.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting an Auto-Reconnect sequence.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, true, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);
            
            #endregion

            #endregion

        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("AutoReconnect")]
        [Description("This test case is used to ensure SUT will continue with the reconnection attempt after received the Server Auto-Reconnect Status PDU. ")]
        public void S3_AutoReconnection_NegativeTest_AutoReconnectionFailed()
        {
            #region Test Description
            /*
            This test case is used to ensure SUT will continue with the reconnection attempt after received the Server Auto-Reconnect Status PDU. 
             
            Test Execution Steps: 
            1.	Trigger SUT to initiate and complete a RDP connection. In Capability Exchange phase, 
                Test Suite sets the AUTORECONNECT_SUPPORTED (0x0008) flag within extraFlags field of General Capability Set in Server Demand Active PDU.
            2.	Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), 
                or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) notification types to notify the client that the user has logged on (how to determine the notification type is described in TD section 3.3.5.10).
            3.	Test Suite sends a Save Session Info PDU with a notification type of Logon Info Extended which presents a Server Auto-Reconnect Packet.
            4.	Trigger SUT to start an Auto-Reconnect sequence. This can be implemented by creating a short-term network failure on client side. 
                During the reconnection sequence, Test suite expects SUT presents a Client Auto-Reconnect Packet in the Client Info PDU.
            5.	After the connection sequence was finished, Test suite sends SUT a Server Auto-Reconnect Status PDU to indicate the automatic reconnection has failed.
            6.	Test Suite expects SUT continue the reconnection attempt (it can be verified if SUT didn’t disconnect after a period).  
            */
            #endregion

            #region Test Sequence

            this.TestSite.Assert.IsTrue(isClientSuportAutoReconnect, "SUT should support Auto-Reconnect.");

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            #region First Connection
            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability and set the AUTORECONNECT_SUPPORTED (0x0008) flag within extraFlags field of General Capability Set in Server Demand Active PDU.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability and advertising the support for Auto_Reconnect.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT with Auto-Reconnect information.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.AutoReconnectCookie, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            #region Auto-Reconnect Sequence

            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to start an Auto-Reconnect.");
            int iResult = this.sutControlAdapter.TriggerClientAutoReconnect(this.TestContext.TestName);
            TestSite.Assume.IsTrue(iResult >= 0, "SUT Control Adapter: TriggerClientAutoReconnect should be successful: {0}.", iResult);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.AutoReconnection);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability and advertising the support for Auto_Reconnect.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting an Auto-Reconnect sequence.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, true, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Auto-Reconnect Status PDU to SUT to notify the auto-reconnection attempt has failed.");
            this.rdpbcgrAdapter.ServerAutoReconnectStatusPdu();

            #endregion
                       
            #endregion
        }
    
    }
}
