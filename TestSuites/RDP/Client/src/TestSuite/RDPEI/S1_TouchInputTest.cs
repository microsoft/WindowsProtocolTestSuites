// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpei;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdpegt;

namespace Microsoft.Protocols.TestSuites.Rdpei
{
    public partial class RdpeiTestSuite
    {
        #region BVT Test Cases

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEI")]        
        [Description("Test the initialization of touch remoting transactions.")]
        public void Rdpei_TouchInputTest_Positive_TouchReadiness()
        {
            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            Site.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEI ...");
            bool bProtocolSupported = this.rdpeiServer.CreateRdpeiDvc(waitTime);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            // RDPEI initializing phase
            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_SC_READY_PDU.");
            RDPINPUT_SC_READY_PDU scReadyPdu = this.rdpeiServer.CreateRdpInputScReadyPdu();
            this.rdpeiServer.SendRdpInputScReadyPdu(scReadyPdu);

            Site.Log.Add(LogEntryKind.Debug, "Expecting RDPINPUT_CS_READY_PDU ...");
            RDPINPUT_CS_READY_PDU csReadyPdu = this.rdpeiServer.ExpectRdpInputCsReadyPdu(waitTime);
            VerifyRdpInputCsReadyPdu(csReadyPdu);
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEI")]        
        [Description("Test the content of the RDPINPUT_TOUCH_EVENT_PDU message.")]
        public void Rdpei_TouchInputTest_Positive_SingleTouchEvent()
        {
            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            Site.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEI ...");
            bool bProtocolSupported = this.rdpeiServer.CreateRdpeiDvc(waitTime);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            // RDPEI initializing phase
            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_SC_READY_PDU.");
            RDPINPUT_SC_READY_PDU scReadyPdu = this.rdpeiServer.CreateRdpInputScReadyPdu();
            this.rdpeiServer.SendRdpInputScReadyPdu(scReadyPdu);

            Site.Log.Add(LogEntryKind.Debug, "Expecting RDPINPUT_CS_READY_PDU ...");
            RDPINPUT_CS_READY_PDU csReadyPdu = this.rdpeiServer.ExpectRdpInputCsReadyPdu(waitTime);
            TestSite.Assert.IsTrue(csReadyPdu != null, "Client is expected to send RDPINPUT_CS_READY_PDU to the server.");

            this.rdpeiSUTControlAdapter.TriggerOneTouchEventOnClient(this.TestContext.TestName);
            // RDPEI running phase
            Site.Log.Add(LogEntryKind.Debug, "Expecting RDPINPUT_TOUCH_EVENT_PDU ...");
            RDPINPUT_TOUCH_EVENT_PDU touchEventPdu = this.rdpeiServer.ExpectRdpInputTouchEventPdu(waitTime);
            VerifyRdpInputTouchEventPdu(touchEventPdu, true);

            if (isManagedAdapter)
            {
                RdpeiUtility.SendConfirmImage();
            }
        }
        #endregion

        #region Non-BVT Positive Test Cases

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEI")]
        [TestCategory("TouchSimulated")]
        [Description("Test the position of the contacts in RDPINPUT_TOUCH_EVENT_PDU message.")]
        public void Rdpei_TouchInputTest_Positive_SingleTouchContactPosition()
        {
            if (isInteractiveAdapter)
            {
                Site.Assert.Inconclusive("This case will not be bun when using interactive client control adapter.");
            }

            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            Site.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEI ...");
            bool bProtocolSupported = this.rdpeiServer.CreateRdpeiDvc(waitTime);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            // RDPEI initializing phase
            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_SC_READY_PDU.");
            RDPINPUT_SC_READY_PDU scReadyPdu = this.rdpeiServer.CreateRdpInputScReadyPdu();
            this.rdpeiServer.SendRdpInputScReadyPdu(scReadyPdu);

            Site.Log.Add(LogEntryKind.Debug, "Expecting RDPINPUT_CS_READY_PDU ...");
            RDPINPUT_CS_READY_PDU csReadyPdu = this.rdpeiServer.ExpectRdpInputCsReadyPdu(waitTime);
            TestSite.Assert.IsTrue(csReadyPdu != null, "Client is expected to send RDPINPUT_CS_READY_PDU to the server.");

            this.rdpeiSUTControlAdapter.TriggerPositionSpecifiedTouchEventOnClient("Rdpei_TouchInputTest_Positive_SingleTouchEvent");
            // RDPEI running phase
            ushort width = this.rdpbcgrAdapter.CapabilitySetting.DesktopWidth;
            ushort height = this.rdpbcgrAdapter.CapabilitySetting.DesktopHeight;
            // The diameter of the circle to be sent to the client.
            ushort diam = 64;
            Random random = new Random();
            // The left and top position of the circles to be sent to the client.
            //ushort[] arr = { 0, 0, 0, (ushort)(height - diam), (ushort)(width - diam), 0, (ushort)(width - diam), (ushort)(height - diam), (ushort)(random.Next(width - diam * 2) + diam), (ushort)(random.Next(height - diam * 2) + diam) };
            ushort[] arr = { 0, 0, 0, (ushort)(height - diam), (ushort)(width - diam), 0, (ushort)(width - diam), (ushort)(height - diam), (ushort)(width / 2 - diam / 2), (ushort)(height / 2 - diam / 2) };
            bool isFirstFrame = true;
            for (int i = 0; i < 5; i++)
            {
                ushort left = arr[i * 2];
                ushort top = arr[i * 2 + 1];

                if (isManagedAdapter)
                {
                    RdpeiUtility.SendCircle(diam, Color.Red, left, top);
                }
                Site.Log.Add(LogEntryKind.Debug, "Expecting RDPINPUT_TOUCH_EVENT_PDU ...");

                ushort preLeft = (i == 0) ? (ushort)(width + 1) : arr[(i - 1) * 2];
                ushort preTop = (i == 0) ? (ushort)(height + 1) : arr[2 * i - 1];
                bool isExpectedFrameReceived = false;
                while (!isExpectedFrameReceived)
                {
                    RDPINPUT_TOUCH_EVENT_PDU touchEventPdu = this.rdpeiServer.ExpectRdpInputTouchEventPdu(waitTime);
                    VerifyRdpInputTouchEventPdu(touchEventPdu, isFirstFrame);
                    if (isFirstFrame)
                    {
                        isFirstFrame = false;
                    }
                    foreach (RDPINPUT_TOUCH_FRAME f in touchEventPdu.frames)
                    {
                        foreach (RDPINPUT_CONTACT_DATA d in f.contacts)
                        {
                            int contactX = d.x.ToInt();
                            int contactY = d.y.ToInt();
                            // Consume the RDPINPUT_TOUCH_EVENT_PDU received from last touch action.
                            if (contactX >= preLeft && contactX <= preLeft + diam && contactY >= preTop && contactY <= preTop + diam)
                            {
                                continue;
                            }
                            // Touch out of the valid range, send the unexpected position instruction to the client, and fail the case.
                            if (contactX < left || contactX > left + diam || contactY < top || contactY > top + diam)
                            {
                                if (isManagedAdapter)
                                {
                                    ushort l = (contactX < diam / 2) ? (ushort)0 : (contactX > (ushort)(rdpbcgrAdapter.CapabilitySetting.DesktopWidth - diam / 2) ? (ushort)(rdpbcgrAdapter.CapabilitySetting.DesktopWidth - diam) : (ushort)(contactX - diam / 2));
                                    ushort t = (contactY < diam / 2) ? (ushort)0 : (contactY > (ushort)(rdpbcgrAdapter.CapabilitySetting.DesktopHeight - diam / 2) ? (ushort)(rdpbcgrAdapter.CapabilitySetting.DesktopHeight - diam) : (ushort)(contactY - diam / 2));
                                    RdpeiUtility.SendCircle(diam, Color.Green, l, t);

                                    RdpeiUtility.SendInstruction(RdpeiSUTControlData.UnexpectedPositionNotice);
                                }
                                Site.Assert.IsTrue(false, "Client is expected to send a RDPINPUT_TOUCH_EVENT_PDU whose contact position near ({0}, {1}), not ({2}, {3}).", left + diam / 2, top + diam / 2, contactX, contactY);
                            }
                            else
                            {
                                isExpectedFrameReceived = true;
                            }
                        }
                    }
                }
                // Update on the client screen.
                if (isManagedAdapter)
                {
                    RdpeiUtility.SendCircle(diam, Color.Black, left, top);
                }
            }
            if (isManagedAdapter)
            {
                RdpeiUtility.SendConfirmImage();
            }
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEI")]
        [TestCategory("TouchSimulated")]
        [Description("Test the multitouch of RDPINPUT_TOUCH_EVENT_PDU message.")]
        public void Rdpei_TouchInputTest_Positive_MultiTouchEvent()
        {
            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            Site.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEI ...");
            bool bProtocolSupported = this.rdpeiServer.CreateRdpeiDvc(waitTime);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            // RDPEI initializing phase
            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_SC_READY_PDU.");
            RDPINPUT_SC_READY_PDU scReadyPdu = this.rdpeiServer.CreateRdpInputScReadyPdu();
            this.rdpeiServer.SendRdpInputScReadyPdu(scReadyPdu);

            Site.Log.Add(LogEntryKind.Debug, "Expecting RDPINPUT_CS_READY_PDU ...");
            RDPINPUT_CS_READY_PDU csReadyPdu = this.rdpeiServer.ExpectRdpInputCsReadyPdu(waitTime);
            TestSite.Assert.IsTrue(csReadyPdu != null, "Client is expected to send RDPINPUT_CS_READY_PDU to the server.");

            ushort contactCount = (csReadyPdu.maxTouchContacts > 5) ? (ushort)5 : csReadyPdu.maxTouchContacts;
            this.rdpeiSUTControlAdapter.TriggerMultiTouchEventOnClient(this.TestContext.TestName, contactCount);

            // RDPEI running phase
            Site.Log.Add(LogEntryKind.Debug, "Expecting multitouch RDPINPUT_TOUCH_EVENT_PDU ...");
            bool isFirstFrame = true;
            DateTime endTime = DateTime.Now + waitTime;
            bool isExpectedFrameReceived = false;
            while (DateTime.Now < endTime && !isExpectedFrameReceived)
            {
                RDPINPUT_TOUCH_EVENT_PDU touchEventPdu = this.rdpeiServer.ExpectRdpInputTouchEventPdu(waitTime);
                Site.Assert.IsNotNull(touchEventPdu, "Client is expected to send to the server a RDPINPUT_TOUCH_EVENT_PDU whose contactCount is {0}.", contactCount);
                VerifyRdpInputTouchEventPdu(touchEventPdu, isFirstFrame);
                if (isFirstFrame)
                {
                    isFirstFrame = false;
                }
                foreach (RDPINPUT_TOUCH_FRAME f in touchEventPdu.frames)
                {
                    if (f.contactCount.ToUShort() == contactCount)
                    {
                        isExpectedFrameReceived = true;
                        break;
                    }
                }
            }
            Site.Assert.IsTrue(isExpectedFrameReceived, "Client is expected to send to the server a RDPINPUT_TOUCH_EVENT_PDU whose contactCount is {0}.", contactCount);
            if (isManagedAdapter)
            {
                RdpeiUtility.SendConfirmImage();
            }
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEI")]
        [TestCategory("TouchSimulated")]
        [Description("Test the state transition of contacts in RDPINPUT_TOUCH_EVENT_PDU and RDPINPUT_DISMISS_HOVERING_CONTACT_PDU messages.")]
        public void Rdpei_TouchInputTest_Positive_ContactStateTransition()
        {
            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            Site.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEI ...");
            bool bProtocolSupported = this.rdpeiServer.CreateRdpeiDvc(waitTime);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            // RDPEI initializing phase
            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_SC_READY_PDU.");
            RDPINPUT_SC_READY_PDU scReadyPdu = this.rdpeiServer.CreateRdpInputScReadyPdu();
            this.rdpeiServer.SendRdpInputScReadyPdu(scReadyPdu);

            Site.Log.Add(LogEntryKind.Debug, "Expecting RDPINPUT_CS_READY_PDU ...");
            RDPINPUT_CS_READY_PDU csReadyPdu = this.rdpeiServer.ExpectRdpInputCsReadyPdu(waitTime);
            TestSite.Assert.IsTrue(csReadyPdu != null, "Client is expected to send RDPINPUT_CS_READY_PDU to the server.");

            this.rdpeiSUTControlAdapter.TriggerContinuousTouchEventOnClient(this.TestContext.TestName);
            // RDPEI running phase
            this.stateMachine = new TouchContactStateMachine();
            this.stateMachine.Initialize();

            DateTime expireTime = DateTime.Now + waitTime;
            Site.Log.Add(LogEntryKind.Debug, "Expecting RDPINPUT_TOUCH_EVENT_PDU or RDPINPUT_DISMISS_HOVERING_CONTACT_PDU ...");
            bool isFirstFrame = true;
            while (DateTime.Now < expireTime)
            {
                RDPINPUT_PDU inputPdu = this.rdpeiServer.ExpectRdpInputPdu(waitTime);
                // Check the state transition of every contact in every received pdu.
                VerifyAndUpdateContactState(inputPdu, isFirstFrame);
                if (isFirstFrame)
                {
                    isFirstFrame = false;
                }
            }
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Positive")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEI")]        
        [Description("Test the RDPINPUT_DISMISS_HOVERING_PDU message. Only if the client device supports proximity.")]
        public void Rdpei_TouchInputTest_Positive_DismissHoveringContact()
        {
            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            Site.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEI ...");
            bool bProtocolSupported = this.rdpeiServer.CreateRdpeiDvc(waitTime);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            // RDPEI initializing phase
            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_SC_READY_PDU.");
            RDPINPUT_SC_READY_PDU scReadyPdu = this.rdpeiServer.CreateRdpInputScReadyPdu();
            this.rdpeiServer.SendRdpInputScReadyPdu(scReadyPdu);

            Site.Log.Add(LogEntryKind.Debug, "Expecting RDPINPUT_CS_READY_PDU ...");
            RDPINPUT_CS_READY_PDU csReadyPdu = this.rdpeiServer.ExpectRdpInputCsReadyPdu(waitTime);
            TestSite.Assert.IsTrue(csReadyPdu != null, "Client is expected to send RDPINPUT_CS_READY_PDU to the server.");

            // Trigger the user to determin whether the client device supports proximity. If not, negative value will be returned when using interactive adapter.
            if (this.rdpeiSUTControlAdapter.TriggerDismissHoveringContactPduOnClient(this.TestContext.TestName) < 0)
            {
                TestSite.Assert.Inconclusive("The client device does not support proximity.");
            }
            // RDPEI running phase
            RDPINPUT_TOUCH_EVENT_PDU touchEventPdu = this.rdpeiServer.ExpectRdpInputTouchEventPdu(waitTime);

            this.stateMachine = new TouchContactStateMachine();
            this.stateMachine.Initialize();
            VerifyAndUpdateContactState(touchEventPdu, true);
            // Verify the user input.
            ushort left = (ushort)(rdpbcgrAdapter.CapabilitySetting.DesktopWidth - 160);
            ushort top = (ushort)(rdpbcgrAdapter.CapabilitySetting.DesktopHeight - 120);
            ushort width = 100;
            ushort height = 60;
            if (touchEventPdu != null && touchEventPdu.frames != null)
            {
                foreach (RDPINPUT_TOUCH_FRAME f in touchEventPdu.frames)
                {
                    foreach (RDPINPUT_CONTACT_DATA d in f.contacts)
                    {
                        int x = d.x.ToInt();
                        int y = d.y.ToInt();
                        if (x >= left && x <= (left + width) && y >= top && y <= (top + height))
                        {
                            TestSite.Assert.Inconclusive("The client device does not support proximity.");
                        }
                    }
                }
            }
            // The client supports proximity, waiting for RDPINPUT_DISMISS_HOVERING_CONTACT_PDU.
            Site.Log.Add(LogEntryKind.Debug, "Expecting RDPINPUT_DISMISS_HOVERING_CONTACT_PDU ...");
            DateTime endTime = DateTime.Now + waitTime;
            bool isExpectedFrameReceived = false;
            while (DateTime.Now < endTime && !isExpectedFrameReceived)
            {
                RDPINPUT_PDU pdu = this.rdpeiServer.ExpectRdpInputPdu(waitTime);
                // Intent to check whether the state transition of received RDPINPUT_DISMISS_HOVERING_CONTACT_PDU is valid.
                VerifyAndUpdateContactState(pdu, false);
                if (pdu != null && pdu is RDPINPUT_DISMISS_HOVERING_CONTACT_PDU)
                {
                    isExpectedFrameReceived = true;
                }
            }
            TestSite.Assert.IsTrue(isExpectedFrameReceived, "Client is expected to send RDPINPUT_DISMISS_HOVERING_CONTACT_PDU to the server.");
            if (isManagedAdapter)
            {
                RdpeiUtility.SendConfirmImage();
            }
        }

        #endregion

        #region Non-BVT Negetive Test Cases

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEI")]
        [TestCategory("TouchSimulated")]
        [Description("Ensure the client will ignore the RDPINPUT_SC_READY_PDU message with invalid eventId.")]
        public void Rdpei_TouchInputTest_Negative_InvalidEventIdInInitializingPhase()
        {
            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            Site.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEI ...");
            bool bProtocolSupported = this.rdpeiServer.CreateRdpeiDvc(waitTime);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            // RDPEI initializing phase
            Site.Log.Add(LogEntryKind.Debug, "Sending an invalid PDU.");
            RDPINPUT_INVALID_PDU invalidPdu = CreateRdpInputInvalidPdu(0xFFFF, 10, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF });
            SendRdpInvalidPdu(invalidPdu);
            RDPINPUT_CS_READY_PDU csReadyPdu = this.rdpeiServer.ExpectRdpInputCsReadyPdu(shortWaitTime);
            Site.Assert.IsNull(csReadyPdu, "The client should ignore the RDPINPUT_SC_READY_PDU message with invalid eventId.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEI")]
        [TestCategory("TouchSimulated")]
        [Description("Ensure the client will drop the connection when the pduLength in SC_READY message is inconsistent with the length of the message.")]
        public void Rdpei_TouchInputTest_Negative_InvalidScReadyPduLength()
        {
            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            Site.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEI ...");
            bool bProtocolSupported = this.rdpeiServer.CreateRdpeiDvc(waitTime);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            // RDPEI initializing phase
            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_SC_READY_PDU with PduLength set to 6.");
            RDPINPUT_SC_READY_PDU scReadyPdu = this.rdpeiServer.CreateRdpInputScReadyPdu(EventId_Values.EVENTID_SC_READY, 6);
            this.rdpeiServer.SendRdpInputScReadyPdu(scReadyPdu);

            // Expect the client to ignore the RDPINPUT_SC_READY_PDU with invalid pduLength. 
            RDPINPUT_CS_READY_PDU csReadyPdu = this.rdpeiServer.ExpectRdpInputCsReadyPdu(shortWaitTime);
            Site.Assert.IsNull(csReadyPdu, "The client should ignore the RDPINPUT_SC_READY_PDU message when the pduLength field is inconsistent with the amount of data read from DVC.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEI")]
        [TestCategory("TouchSimulated")]
        [Description("Ensure the client will not send RDPINPUT_TOUCH_EVENT_PDU to the server without negotiation.")]
        public void Rdpei_TouchInputTest_Negative_TouchEventWithoutNegotiation()
        {
            Site.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection();

            Site.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEI ...");
            bool bProtocolSupported = this.rdpeiServer.CreateRdpeiDvc(waitTime);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            this.rdpeiSUTControlAdapter.TriggerContinuousTouchEventOnClient("Rdpei_TouchInputTest_Negative_TouchEventWithoutNegotiation");

            RDPINPUT_TOUCH_EVENT_PDU touchEventPdu = this.rdpeiServer.ExpectRdpInputTouchEventPdu(waitTime);
            Site.Assert.IsNull(touchEventPdu, "The client is not expected to send a RDPINPUT_TOUCH_EVENT_PDU.");
        }

        #endregion
    }
}
