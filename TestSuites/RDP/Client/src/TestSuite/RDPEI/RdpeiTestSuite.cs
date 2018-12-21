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

namespace Microsoft.Protocols.TestSuites.Rdpei
{
    [TestClass]
    public partial class RdpeiTestSuite : RdpTestClassBase
    {
        #region Adapter Instances

        private RdpeiServer rdpeiServer;
        private IRdpeiSUTControlAdapter rdpeiSUTControlAdapter;
        private RdpedycServer rdpedycServer;
        private bool isManagedAdapter;
        private bool isInteractiveAdapter;
        private TouchContactStateMachine stateMachine;

        #endregion

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

            this.rdpeiSUTControlAdapter = (IRdpeiSUTControlAdapter)this.TestSite.GetAdapter(typeof(IRdpeiSUTControlAdapter));
            this.rdpeiSUTControlAdapter.Reset();
            this.rdpbcgrAdapter.TurnVerificationOff(false);
            RdpeiUtility.Initialized(this.TestSite);

            isManagedAdapter = Site.Config.GetAdapterConfig("IRdpeiSUTControlAdapter").GetType().Name.Equals("ManagedAdapterConfig");
            isInteractiveAdapter = Site.Config.GetAdapterConfig("IRdpeiSUTControlAdapter").GetType().Name.Equals("InteractiveAdapterConfig");
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);
        }

        protected override void TestCleanup()
        {
            // TestCleanup() may be not main thread
            DynamicVCException.SetCleanUp(true);

            base.TestCleanup();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Dispose virtual channel manager.");

            if (rdpedycServer != null)
                rdpedycServer.Dispose();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger client to close all RDP connections for clean up.");
            int iResult = this.sutControlAdapter.TriggerClientDisconnectAll(this.TestContext.TestName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "The result of TriggerClientDisconnectAll is {0}.", iResult);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Stop RDP listening.");
            this.rdpbcgrAdapter.StopRDPListening();

            DynamicVCException.SetCleanUp(false);
        }
        #endregion

        #region Private Methods

        //Set default server capabilities
        private void SetServerCapabilitiesWithRemoteFxSupported()
        {
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true,
               true, maxRequestSize,
               true,
               true, 1,
               true, CmdFlags_Values.SURFCMDS_FRAMEMARKER | CmdFlags_Values.SURFCMDS_SETSURFACEBITS | CmdFlags_Values.SURFCMDS_STREAMSURFACEBITS,
               true, true, true);
        }

        //Start RDP connection.
        private void StartRDPConnection(bool createReliableUDPtransport = false,
            bool createLossyUDPtransport = false)
        {

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol, true);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            SetServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion
            rdpedycServer = new RdpedycServer(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);
            rdpedycServer.ExchangeCapabilities(waitTime);
            rdpeiServer = new RdpeiServer(rdpedycServer);

        }

        //Stop RDP connection.
        private void StopRDPConnection()
        {
            int iResult = this.sutControlAdapter.TriggerClientDisconnectAll(this.TestContext.TestName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "The result of TriggerClientDisconnectAll is {0}.", iResult);
            this.rdpbcgrAdapter.Reset();
            this.rdpeiServer.Reset();
        }

        #endregion

        #region Verify received pdus

        public void VerifyRdpInputCsReadyPdu(RDPINPUT_CS_READY_PDU pdu)
        {
            Site.Assert.IsNotNull(pdu, "Client is expected to send RDPINPUT_CS_READY_PDU to the server.");
            Site.Assert.IsNotNull(pdu.header, "The header of RDPINPUT_CS_READY_PDU should not be null.");
            Site.Assert.AreEqual(pdu.header.eventId, EventId_Values.EVENTID_CS_READY, "The eventId in the header of RDPINPUT_CS_READY_PDU is expected to be EVENTID_CS_READY.");
            Site.Assert.AreEqual(pdu.header.pduLength, pdu.Length(), "The length of the RDPINPUT_CS_READY_PDU message is expected to be the same with the pduLength in header.");
            bool verifyProtocolVersion = pdu.protocolVersion == RDPINPUT_CS_READY_ProtocolVersion.RDPINPUT_PROTOCOL_V100 || pdu.protocolVersion == RDPINPUT_CS_READY_ProtocolVersion.RDPINPUT_PROTOCOL_V101
                || pdu.protocolVersion == RDPINPUT_CS_READY_ProtocolVersion.RDPINPUT_PROTOCOL_V200;
            Site.Assert.IsTrue(verifyProtocolVersion, "The protocolVersion in RDPINPUT_CS_READY_PDU is expected to be RDPINPUT_PROTOCOL_V1.");
            
        }

        public void VerifyRdpInputDismissHoveringContactPdu(RDPINPUT_DISMISS_HOVERING_CONTACT_PDU pdu)
        {
            Site.Assert.IsNotNull(pdu, "Client is expected to send RDPINPUT_DISMISS_HOVERING_CONTACT_PDU to the server.");
            Site.Assert.IsNotNull(pdu.header, "The header of RDPINPUT_DISMISS_HOVERING_CONTACT_PDU should not be null.");
            Site.Assert.AreEqual(pdu.header.eventId, EventId_Values.EVENTID_DISMISS_HOVERING_CONTACT, "The eventId in the header of RDPINPUT_DISMISS_HOVERING_CONTACT_PDU is expected to be EVENTID_DISMISS_HOVERING_CONTACT.");
            Site.Assert.AreEqual(pdu.header.pduLength, pdu.Length(), "The length of the RDPINPUT_DISMISS_HOVERING_CONTACT_PDU message is expected to be the same with the pduLength in header.");
        }

        /// <summary>
        /// This method is used to verify the structure of RDPINPUT_TOUCH_EVENT_PDU.
        /// </summary>
        /// <param name="pdu">The RDPINPUT_TOUCH_EVENT_PDU.</param>
        /// <param name="isFirstFrame">Whether the input pdu is the first pdu that client has transmitted.</param>
        public void VerifyRdpInputTouchEventPdu(RDPINPUT_TOUCH_EVENT_PDU pdu, bool isFirstFrame)
        {
            Site.Assert.IsNotNull(pdu, "Client is expected to send RDPINPUT_TOUCH_EVENT_PDU to the server.");
            Site.Assert.IsNotNull(pdu.header, "The header of RDPINPUT_TOUCH_EVENT_PDU should not be null.");
            Site.Assert.IsNotNull(pdu.frames, "The array of RDPINPUT_TOUCH_FRAME in RDPINPUT_TOUCH_EVENT_PDU should not be null.");
            Site.Assert.AreEqual(pdu.header.pduLength, pdu.Length(), "The length of the RDPINPUT_TOUCH_EVENT_PDU message is expected to be the same with the pduLength in header.");
            Site.Assert.AreEqual((int)pdu.frameCount.ToUShort(), pdu.frames.Length, "The size of the array frames in RDPINPUT_TOUCH_EVENT_PDU is expected to be the same with the frameCount .");
            if (isFirstFrame)
            {
                Site.Assert.IsTrue(pdu.frames[0].frameOffset.ToULong() == 0, "The field frameOffset of the first frame being transmitted must be set to zero.");
            }
            foreach (RDPINPUT_TOUCH_FRAME f in pdu.frames)
            {
                Site.Assert.AreEqual((int)f.contactCount.ToUShort(), f.contacts.Length, "The size of the array contacts in RDPINPUT_TOUCH_FRAME is expected to be the same with the contactCount .");
                foreach (RDPINPUT_CONTACT_DATA d in f.contacts)
                {
                    if (((RDPINPUT_CONTACT_DATA_FieldsPresent)(d.fieldsPresent.ToUShort())).HasFlag(RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_CONTACTRECT_PRESENT))
                    {
                        Site.Assert.IsNotNull(d.contactRectBottom, "The contactRectBottom is not expected to be null when flag CONTACT_DATA_CONTACTRECT_PRESENT in filedsPresent is set.");
                        Site.Assert.IsNotNull(d.contactRectTop, "The contactRectTop is not expected to be null when flag CONTACT_DATA_CONTACTRECT_PRESENT in filedsPresent is set.");
                        Site.Assert.IsNotNull(d.contactRectLeft, "The contactRectLeft is not expected to be null when flag CONTACT_DATA_CONTACTRECT_PRESENT in filedsPresent is set.");
                        Site.Assert.IsNotNull(d.contactRectRight, "The contactRectRight is not expected to be null when flag CONTACT_DATA_CONTACTRECT_PRESENT in filedsPresent is set.");
                    }
                    if (((RDPINPUT_CONTACT_DATA_FieldsPresent)(d.fieldsPresent.ToUShort())).HasFlag(RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_ORIENTATION_PRESENT))
                    {
                        Site.Assert.IsNotNull(d.orientation, "The orientation is not expected to be null when flag CONTACT_DATA_ORIENTATION_PRESENT in filedsPresent is set.");
                    }
                    if (((RDPINPUT_CONTACT_DATA_FieldsPresent)(d.fieldsPresent.ToUShort())).HasFlag(RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_PRESSURE_PRESENT))
                    {
                        Site.Assert.IsNotNull(d.pressure, "The orientation is not expected to be null when flag CONTACT_DATA_PRESSURE_PRESENT in filedsPresent is set.");
                    }

                    Site.Assert.IsTrue(Enum.IsDefined(typeof(ValidStateFlagCombinations), d.contactFlags.ToUInt()), "The value of contactFlags does not contain a valid combination of the contact state flags.");
                }
            }
        }

        /// <summary>
        /// This method is used to verify the structure of RDPINPUT_TOUCH_EVENT_PDU or RDPINPUT_DISMISS_HOVERING_CONTACT_PDU, and update the contact transition state according to the contacts contained in the message.
        /// </summary>
        /// <param name="inputPdu">The input pdu.</param>
        /// <param name="isFirstFrame">Whether the input pdu is the first pdu that client has transmitted.</param>
        public void VerifyAndUpdateContactState(RDPINPUT_PDU inputPdu, bool isFirstFrame)
        {
            if (inputPdu == null)
            {
                return;
            }
            if (inputPdu is RDPINPUT_TOUCH_EVENT_PDU)
            {
                RDPINPUT_TOUCH_EVENT_PDU pdu = inputPdu as RDPINPUT_TOUCH_EVENT_PDU;
                Site.Assert.IsNotNull(pdu.header, "The header of RDPINPUT_TOUCH_EVENT_PDU should not be null.");
                Site.Assert.IsNotNull(pdu.frames, "The array of RDPINPUT_TOUCH_FRAME in RDPINPUT_TOUCH_EVENT_PDU should not be null.");
                Site.Assert.AreEqual(pdu.header.pduLength, pdu.Length(), "The length of the RDPINPUT_TOUCH_EVENT_PDU message is expected to be the same with the pduLength in header.");
                Site.Assert.AreEqual((int)pdu.frameCount.ToUShort(), pdu.frames.Length, "The size of the array frames in RDPINPUT_TOUCH_EVENT_PDU is expected to be the same with the frameCount .");
                if (isFirstFrame)
                {
                    Site.Assert.IsTrue(pdu.frames[0].frameOffset.ToULong() == 0, "The field frameOffset of the first frame being transmitted must be set to zero.");
                }
                foreach (RDPINPUT_TOUCH_FRAME f in pdu.frames)
                {
                    Site.Assert.AreEqual((int)f.contactCount.ToUShort(), f.contacts.Length, "The size of the array contacts in RDPINPUT_TOUCH_FRAME is expected to be the same with the contactCount .");
                    foreach (RDPINPUT_CONTACT_DATA d in f.contacts)
                    {
                        if (((RDPINPUT_CONTACT_DATA_FieldsPresent)(d.fieldsPresent.ToUShort())).HasFlag(RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_CONTACTRECT_PRESENT))
                        {
                            Site.Assert.IsNotNull(d.contactRectBottom, "The contactRectBottom is not expected to be null when flag CONTACT_DATA_CONTACTRECT_PRESENT in filedsPresent is set.");
                            Site.Assert.IsNotNull(d.contactRectTop, "The contactRectTop is not expected to be null when flag CONTACT_DATA_CONTACTRECT_PRESENT in filedsPresent is set.");
                            Site.Assert.IsNotNull(d.contactRectLeft, "The contactRectLeft is not expected to be null when flag CONTACT_DATA_CONTACTRECT_PRESENT in filedsPresent is set.");
                            Site.Assert.IsNotNull(d.contactRectRight, "The contactRectRight is not expected to be null when flag CONTACT_DATA_CONTACTRECT_PRESENT in filedsPresent is set.");
                        }
                        if (((RDPINPUT_CONTACT_DATA_FieldsPresent)(d.fieldsPresent.ToUShort())).HasFlag(RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_ORIENTATION_PRESENT))
                        {
                            Site.Assert.IsNotNull(d.orientation, "The orientation is not expected to be null when flag CONTACT_DATA_ORIENTATION_PRESENT in filedsPresent is set.");
                        }
                        if (((RDPINPUT_CONTACT_DATA_FieldsPresent)(d.fieldsPresent.ToUShort())).HasFlag(RDPINPUT_CONTACT_DATA_FieldsPresent.CONTACT_DATA_PRESSURE_PRESENT))
                        {
                            Site.Assert.IsNotNull(d.pressure, "The orientation is not expected to be null when flag CONTACT_DATA_PRESSURE_PRESENT in filedsPresent is set.");
                        }

                        Site.Assert.IsTrue(Enum.IsDefined(typeof(ValidStateFlagCombinations), d.contactFlags.ToUInt()), "The value of contactFlags does not contain a valid combination of the contact state flags.");
                        ValidStateFlagCombinations temp = (ValidStateFlagCombinations)(d.contactFlags.ToUInt());
                        byte result = stateMachine.UpdateContactsMap(d.contactId, temp, d.x.ToInt(), d.y.ToInt());

                        Site.Assert.AreNotEqual(result, 1, "The contact state transition is invalid.");
                        Site.Assert.AreNotEqual(result, 2, "The contact position cannot change when transitioning from 'engaged' state to 'hovering' or 'out of range' state.");
                    }
                }
            }
            else if (inputPdu is RDPINPUT_DISMISS_HOVERING_CONTACT_PDU)
            {
                RDPINPUT_DISMISS_HOVERING_CONTACT_PDU pdu = inputPdu as RDPINPUT_DISMISS_HOVERING_CONTACT_PDU;
                Site.Assert.IsNotNull(pdu.header, "The header of RDPINPUT_DISMISS_HOVERING_CONTACT_PDU should not be null.");
                Site.Assert.AreEqual(pdu.header.eventId, EventId_Values.EVENTID_DISMISS_HOVERING_CONTACT, "The eventId in the header of RDPINPUT_DISMISS_HOVERING_CONTACT_PDU is expected to be EVENTID_DISMISS_HOVERING_CONTACT.");
                Site.Assert.AreEqual(pdu.header.pduLength, pdu.Length(), "The length of the RDPINPUT_DISMISS_HOVERING_CONTACT_PDU message is expected to be the same with the pduLength in header.");
                TouchContactAttribute attr;
                if (stateMachine.contactStateMap.TryGetValue(pdu.contactId, out attr))
                {
                    Site.Assert.AreEqual(attr.state, TouchContactState.Hovering, "The contact {0} is expected to be in hovering state.", pdu.contactId);
                    stateMachine.UpdateContactsMap(pdu.contactId, ValidStateFlagCombinations.UPDATE, 0, 0);
                }
                else
                {
                    Site.Assert.IsTrue(false, "The contact {0} is expected to be in hovering state, but it's out of range.", pdu.contactId);
                }
            }
            else
            {
                Site.Assert.IsTrue(false, "Unexpected message.");
            }
        }

        #endregion

        #region Invalid Rdp Input Pdu

        public class RDPINPUT_INVALID_PDU : BasePDU
        {
            public ushort eventId;

            public uint pduLength;

            public byte[] data;

            #region Encoding/Decoding

            public override void Encode(PduMarshaler marshaler)
            {
                marshaler.WriteUInt16(eventId);
                marshaler.WriteUInt32(pduLength);
                if (data != null)
                {
                    marshaler.WriteBytes(data);
                }
            }

            public override bool Decode(PduMarshaler marshaler)
            {
                try
                {
                    eventId = marshaler.ReadUInt16();
                    pduLength = marshaler.ReadUInt32();
                    data = marshaler.ReadToEnd();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            #endregion
        }

        public RDPINPUT_INVALID_PDU CreateRdpInputInvalidPdu(ushort eventId, uint pduLength, byte[] data)
        {
            RDPINPUT_INVALID_PDU pdu = new RDPINPUT_INVALID_PDU();
            pdu.eventId = eventId;
            pdu.pduLength = pduLength;
            pdu.data = data;
            return pdu;
        }

        public void SendRdpInvalidPdu(RDPINPUT_INVALID_PDU pdu)
        {
            byte[] data = PduMarshaler.Marshal(pdu);

            //Sleep 100 millisecond to this packet to be merged with another packet in TCP channel.
            System.Threading.Thread.Sleep(100);

            rdpeiServer.RdpeiDVChannel.Send(data);
        }

        #endregion
    }
}

