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
        #region BVT Cases

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEI")]        
        [Description("Test the RDPINPUT_SUSPEND_TOUCH_PDU message.")]
        public void Rdpei_TouchControlTest_Positive_Suspend()
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

            // RDPEI running phase
            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_SUSPEND_TOUCH_PDU.");
            RDPINPUT_SUSPEND_TOUCH_PDU suspendPdu = this.rdpeiServer.CreateRdpInputSuspendTouchPdu();
            this.rdpeiServer.SendRdpInputSuspendTouchPdu(suspendPdu);

            this.rdpeiSUTControlAdapter.TriggerContinuousTouchEventOnClient(this.TestContext.TestName);

            // Expect to reveice nothing after sending the RDPINPUT_SUSPEND_TOUCH_PDU message.
            RDPINPUT_PDU pdu = this.rdpeiServer.ExpectRdpInputPdu(waitTime);
            TestSite.Assert.IsNull(pdu, "Client must suspend the transmission of touch frames after receiving a RDPINPUT_SUSPEND_TOUCH_PDU message.");
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEI")]        
        [Description("Test the RDPINPUT_RESUME_TOUCH_PDU message.")]
        public void Rdpei_TouchControlTest_Positive_Resume()
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

            // RDPEI running phase
            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_SUSPEND_TOUCH_PDU.");
            RDPINPUT_SUSPEND_TOUCH_PDU suspendPdu = this.rdpeiServer.CreateRdpInputSuspendTouchPdu();
            this.rdpeiServer.SendRdpInputSuspendTouchPdu(suspendPdu);

            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_RESUME_TOUCH_PDU.");
            RDPINPUT_RESUME_TOUCH_PDU resumePdu = this.rdpeiServer.CreateRdpInputResumeTouchPdu();
            this.rdpeiServer.SendRdpInputResumeTouchPdu(resumePdu);

            this.rdpeiSUTControlAdapter.TriggerOneTouchEventOnClient(this.TestContext.TestName);

            Site.Log.Add(LogEntryKind.Debug, "Expecting RDPINPUT_TOUCH_EVENT_PDU ...");
            RDPINPUT_TOUCH_EVENT_PDU touchEventPdu = this.rdpeiServer.ExpectRdpInputTouchEventPdu(waitTime);
            VerifyRdpInputTouchEventPdu(touchEventPdu, true);

            if (isManagedAdapter)
            {
                RdpeiUtility.SendConfirmImage();
            }
        }

        #endregion

        #region Non-BVT Negative Test Cases

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEI")]
        [TestCategory("TouchSimulated")]
        [Description("Ensure the client will drop the connection when the pduLength in RDPINPUT_SUSPEND_TOUCH_PDU message is inconsistent with the length of the message.")]
        public void Rdpei_TouchControlTest_Negative_InvalidSuspendPduLength()
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

            // RDPEI running phase
            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_SUSPEND_TOUCH_PDU with invalid PduLength.");
            RDPINPUT_INVALID_PDU invalidPdu = CreateRdpInputInvalidPdu((ushort)EventId_Values.EVENTID_SUSPEND_TOUCH, 16, null);
            SendRdpInvalidPdu(invalidPdu);

            // Expect the client to ignore the RDPINPUT_SUSPEND_TOUCH_PDU with invalid pduLength. 
            this.rdpeiSUTControlAdapter.TriggerOneTouchEventOnClient(this.TestContext.TestName);

            Site.Log.Add(LogEntryKind.Debug, "Expecting RDPINPUT_TOUCH_EVENT_PDU ...");
            RDPINPUT_TOUCH_EVENT_PDU touchEventPdu = this.rdpeiServer.ExpectRdpInputTouchEventPdu(waitTime);
            Site.Assert.IsNotNull(touchEventPdu, "Client should ignore the RDPINPUT_SUSPEND_TOUCH_PDU message when the pduLength field is inconsistent with the amount of data read from DVC.");
            VerifyRdpInputTouchEventPdu(touchEventPdu, true);

            if (isManagedAdapter)
            {
                RdpeiUtility.SendConfirmImage();
            }
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEI")]
        [TestCategory("TouchSimulated")]
        [Description("Ensure the client will drop the connection when the pduLength in RDPINPUT_RESUME_TOUCH_PDU message is inconsistent with the length of the message.")]
        public void Rdpei_TouchControlTest_Negative_InvalidResumePduLength()
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

            // RDPEI running phase

            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_SUSPEND_TOUCH_PDU.");
            RDPINPUT_SUSPEND_TOUCH_PDU suspendPdu = this.rdpeiServer.CreateRdpInputSuspendTouchPdu();
            this.rdpeiServer.SendRdpInputSuspendTouchPdu(suspendPdu);

            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_RESUME_TOUCH_PDU with invalid PduLength.");
            RDPINPUT_INVALID_PDU invalidPdu = CreateRdpInputInvalidPdu((ushort)EventId_Values.EVENTID_RESUME_TOUCH, 16, null);
            SendRdpInvalidPdu(invalidPdu);

            // Expect the client to ignore the RDPINPUT_RESUME_TOUCH_PDU with invalid pduLength. 
            this.rdpeiSUTControlAdapter.TriggerContinuousTouchEventOnClient(this.TestContext.TestName);

            // Expect to reveice nothing.
            RDPINPUT_PDU pdu = this.rdpeiServer.ExpectRdpInputPdu(waitTime);
            TestSite.Assert.IsNull(pdu, "Client should ignore the RDPINPUT_RESUME_TOUCH_PDU message when the pduLength field is inconsistent with the amount of data read from DVC.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEI")]
        [TestCategory("TouchSimulated")]
        [Description("Ensure the client will ignore the message with invalid eventId during running phase.")]
        public void Rdpei_TouchControlTest_Negative_InvalidEventIdInRunningPhase()
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
            TestSite.Assert.IsNotNull(csReadyPdu, "Client is expected to send RDPINPUT_CS_READY_PDU to the server.");

            // RDPEI running phase
            Site.Log.Add(LogEntryKind.Debug, "Sending an invalid PDU.");
            RDPINPUT_INVALID_PDU invalidPdu = CreateRdpInputInvalidPdu(0xFFFF, 6, null);
            SendRdpInvalidPdu(invalidPdu);

            this.rdpeiSUTControlAdapter.TriggerOneTouchEventOnClient(this.TestContext.TestName);

            Site.Log.Add(LogEntryKind.Debug, "Expecting RDPINPUT_TOUCH_EVENT_PDU ...");
            RDPINPUT_TOUCH_EVENT_PDU touchEventPdu = this.rdpeiServer.ExpectRdpInputTouchEventPdu(waitTime);
            VerifyRdpInputTouchEventPdu(touchEventPdu, true);

            if (isManagedAdapter)
            {
                RdpeiUtility.SendConfirmImage();
            }
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEI")]
        [TestCategory("TouchSimulated")]
        [Description("Test the client will ignore the duplicated RDPINPUT_SUSPEND_TOUCH_PDU message.")]
        public void Rdpei_TouchControlTest_Negative_DuplicatedSuspend()
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

            // RDPEI running phase
            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_SUSPEND_TOUCH_PDU.");
            RDPINPUT_SUSPEND_TOUCH_PDU suspendPdu = this.rdpeiServer.CreateRdpInputSuspendTouchPdu();
            this.rdpeiServer.SendRdpInputSuspendTouchPdu(suspendPdu);

            // Send a duplicated suspend message to test the client will ignore it, since the Touch Remoting Suspended ADM element is already set to TRUE.
            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_SUSPEND_TOUCH_PDU.");
            this.rdpeiServer.SendRdpInputSuspendTouchPdu(suspendPdu);

            this.rdpeiSUTControlAdapter.TriggerContinuousTouchEventOnClient(this.TestContext.TestName);

            // Expect to reveice nothing after sending the RDPINPUT_SUSPEND_TOUCH_PDU message.
            RDPINPUT_PDU pdu = this.rdpeiServer.ExpectRdpInputPdu(waitTime);
            TestSite.Assert.IsNull(pdu, "Client must suspend the transmission of touch frames after receiving a RDPINPUT_SUSPEND_TOUCH_PDU message.");
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Negative")]
        [TestCategory("RDP8.0")]
        [TestCategory("RDPEI")]
        [TestCategory("TouchSimulated")]
        [Description("Test the client will ignore the duplicated RDPINPUT_RESUME_TOUCH_PDU message.")]
        public void Rdpei_TouchControlTest_Negative_DuplicatedResume()
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

            // RDPEI running phase
            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_SUSPEND_TOUCH_PDU.");
            RDPINPUT_SUSPEND_TOUCH_PDU suspendPdu = this.rdpeiServer.CreateRdpInputSuspendTouchPdu();
            this.rdpeiServer.SendRdpInputSuspendTouchPdu(suspendPdu);

            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_RESUME_TOUCH_PDU.");
            RDPINPUT_RESUME_TOUCH_PDU resumePdu = this.rdpeiServer.CreateRdpInputResumeTouchPdu();
            this.rdpeiServer.SendRdpInputResumeTouchPdu(resumePdu);

            // Send a duplicated resume message to test the client will ignore it, since the Touch Remoting Suspended ADM element is already set to FALSE.
            Site.Log.Add(LogEntryKind.Debug, "Sending a RDPINPUT_RESUME_TOUCH_PDU.");
            this.rdpeiServer.SendRdpInputResumeTouchPdu(resumePdu);

            this.rdpeiSUTControlAdapter.TriggerOneTouchEventOnClient(this.TestContext.TestName);

            Site.Log.Add(LogEntryKind.Debug, "Expecting RDPINPUT_TOUCH_EVENT_PDU ...");
            RDPINPUT_TOUCH_EVENT_PDU touchEventPdu = this.rdpeiServer.ExpectRdpInputTouchEventPdu(waitTime);
            VerifyRdpInputTouchEventPdu(touchEventPdu, true);

            if (isManagedAdapter)
            {
                RdpeiUtility.SendConfirmImage();
            }
        }

        #endregion
    }
}
