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
        [TestCategory("Interactive")]
        [TestCategory("RDP7.0")]
        [TestCategory("RDPBCGR")]
        [TestCategory("FastPathInput")]
        [Description("This test case is used to verify the Client Input Events when Fast-Path is enabled.")]
        public void BVT_ClientInputTest_FastPath()
        {
            #region Test Description
            /*
            This test case is used to verify the Fast-Path Input Events, including Keyboard Event or Unicode Keyboard Event, Mouse Event or 
        Extended Mouse Event, Synchronize Event, Client Refresh Rect and Client Suppress Output.
            
            Test Execution Steps:  
            1.	Trigger SUT to initiate and complete a RDP connection. In Capability Exchange phase, Test Suite set inputFlags field to 0x0008 (RDP 5.0, 5.1) or 0x0020 (RDP 5.2 or higher) in Input Capability Set.
            2.	After the connection sequence has been finished, Test Suite sends a Save Session Info PDU with a notification type of the INFOTYPE_LOGON (0x00000000), INFOTYPE_LOGON_LONG (0x00000001), 
                or INFOTYPE_LOGON_PLAINNOTIFY (0x00000002) to notify the SUT that the user has logged on (how to determine the notification type is described in TD section 3.3.5.10) . 
            3.	Trigger SUT to send a Client Input Event PDU which contains Keyboard Event or Unicode Keyboard Event, Mouse Event or 
                Extended Mouse Event.
            4.  Trigger SUT to send Client Refresh Rect PDU and Client Suppress Output PDU.
            5.	Test Suite verifies the received Client Input Event PDU.
            */
            #endregion

            #region Test Sequence

            this.TestSite.Assert.IsTrue(isClientSupportFastPathInput, "RDP client should support FastPath input for executing this test case.");

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with Bitmap Host Cache supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability. Fast-Path Input is supported.");
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true);

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            this.TestSite.Log.Add(LogEntryKind.Comment, @"Triggering SUT to generate input events, including Keyboard Event or Unicode Keyboard Event, Mouse Event or 
            Extended Mouse Event, Synchronize Event, Client Refresh Rect and Client Suppress Output.");
            int iResult = this.sutControlAdapter.TriggerInputEvents(this.TestContext.TestName);
            this.TestSite.Assume.IsTrue(iResult >= 0, "SUT Control Adapter: TriggerInputEvents should be successful: {0}. This test case must be run under \"interactive\" mode", iResult);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Fast-Path Input PDU with a Mouse Event.");
            bool bMouseEventReceived = this.rdpbcgrAdapter.WaitForFastPathInputPdu(eventCode_Values.FASTPATH_INPUT_EVENT_MOUSE, shortWaitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Fast-Path Input PDU with an Extended Mouse Event.");
            bool bExtendedMouseEventReceived = this.rdpbcgrAdapter.WaitForFastPathInputPdu(eventCode_Values.FASTPATH_INPUT_EVENT_MOUSEX, shortWaitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Fast-Path Input PDU with a Keyboard Event.");
            bool bKeyboardEventReceived = this.rdpbcgrAdapter.WaitForFastPathInputPdu(eventCode_Values.FASTPATH_INPUT_EVENT_SCANCODE, shortWaitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Fast-Path Input PDU with a Unicode Keyboard Event.");
            bool bUnicodeKeyboardEventReceived = this.rdpbcgrAdapter.WaitForFastPathInputPdu(eventCode_Values.FASTPATH_INPUT_EVENT_UNICODE, shortWaitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Fast-Path Input PDU with a Synchronize Event.");
            bool bSynchronizeEventReceived = this.rdpbcgrAdapter.WaitForFastPathInputPdu(eventCode_Values.FASTPATH_INPUT_EVENT_SYNC, shortWaitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Refresh Rect PDU.");
            bool bClientRefreshRectPDUReceived = this.rdpbcgrAdapter.WaitForClientPacket<Client_Refresh_Rect_Pdu>(shortWaitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting SUT to send a Client Suppress Output PDU.");
            bool bClientSuppressOutputPDUReceived = this.rdpbcgrAdapter.WaitForClientPacket<Client_Suppress_Output_Pdu>(shortWaitTime);

            this.TestSite.Log.Add(LogEntryKind.Debug, "Received Mouse Event: {0}", bMouseEventReceived);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Received Extended Mouse Event: {0}", bExtendedMouseEventReceived);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Received Keyboard Event: {0}", bKeyboardEventReceived);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Received Unicode Keyboard Event: {0}", bUnicodeKeyboardEventReceived);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Received Synchronize Event: {0}", bSynchronizeEventReceived);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Received Client Refresh Rect PDU: {0}", bClientRefreshRectPDUReceived);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Received Client Suppress Output PDU: {0}", bClientSuppressOutputPDUReceived);

            bool bMouseEventPassed = bMouseEventReceived || bExtendedMouseEventReceived;
            bool bKeyboardEventPassed = bKeyboardEventReceived || bUnicodeKeyboardEventReceived;
            bool bControlServerGrphicsOutputPassed = bClientSuppressOutputPDUReceived;

            this.TestSite.Assert.IsTrue(bMouseEventPassed && bKeyboardEventPassed && bSynchronizeEventReceived && bControlServerGrphicsOutputPassed,
                @"Successfully received the followings: Keyboard Event or Unicode Keyboard Event, Mouse Event or Extended Mouse Event, Synchronize Event, 
                  Client Refresh Rect and Client Suppress Output.");

            #endregion
        }

    }
}
