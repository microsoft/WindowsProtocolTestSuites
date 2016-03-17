// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpei
{
    /// <summary>
    /// The server implementation of MS-RDPEI server.
    /// </summary>
    public class RdpeiServer
    {
        #region Private Variables

        RdpedycServer rdpedycServer;
        DynamicVirtualChannel rdpeiDVC;
        List<RDPINPUT_PDU> receivedList;

        const string rdpeiChannelName = "Microsoft::Windows::RDS::Input";
        const int PacketsInterval = 100; // The interval in milliseconds between sending/receiving two packets.
        uint rdpeiChannelId;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of RdpeiServer
        /// </summary>
        public RdpeiServer(RdpedycServer edycServer)
        {
            receivedList = new List<RDPINPUT_PDU>();
            rdpedycServer = edycServer;
        }

        #endregion

        /// <summary>
        /// Reset the RdpeiServer instance.
        /// </summary>
        public void Reset()
        {
            rdpedycServer = null;
            rdpeiDVC = null;
            receivedList.Clear();
        }

        #region Properties

        /// <summary>
        /// The dynamic virtual channel created for remote touch.
        /// </summary>
        public DynamicVirtualChannel RdpeiDVChannel
        {
            get
            {
                return rdpeiDVC;
            }
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Create dynamic virtual channel.
        /// </summary>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <param name="timeout">Timeout</param>
        /// <returns>true if client supports this protocol; otherwise, return false.</returns>
        public bool CreateRdpeiDvc(TimeSpan timeout, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            
            const ushort priority = 0;
            try
            {
                rdpeiDVC = rdpedycServer.CreateChannel(timeout, priority, rdpeiChannelName, transportType, OnDataReceived);
                rdpeiChannelId = rdpeiDVC.ChannelId;
            }
            catch
            {
            }
            if (rdpeiDVC != null)
            {
                return true;
            }
            return false;
        }

        

        #endregion

        #region Create Methods

        /// <summary>
        /// Method to create a RDPINPUT_SC_READY_PDU.
        /// </summary>
        /// <param name="eventId">A 16-bit unsigned integer that identifies the type of the input event PDU.</param>
        /// <param name="pduLength">A 32-bit unsigned integer that specifies the length of the input event PDU in bytes.</param>
        /// <param name="protocolVersion">A 32-bit unsigned integer that specifies the input protocol version.</param>
        /// <returns>The created RDPINPUT_SC_READY_PDU.</returns>
        public RDPINPUT_SC_READY_PDU CreateRdpInputScReadyPdu(EventId_Values eventId = EventId_Values.EVENTID_SC_READY, uint pduLength = 10, RDPINPUT_SC_READY_ProtocolVersion protocolVersion = RDPINPUT_SC_READY_ProtocolVersion.RDPINPUT_PROTOCOL_V100)
        {
            RDPINPUT_SC_READY_PDU pdu = new RDPINPUT_SC_READY_PDU();
            pdu.header.eventId = eventId;
            pdu.header.pduLength = pduLength;
            pdu.protocolVersion = protocolVersion;
            return pdu;
        }

        /// <summary>
        /// Method to create a RDPINPUT_SUSPEND_TOUCH_PDU.
        /// </summary>
        /// <returns>The created RDPINPUT_SUSPEND_TOUCH_PDU.</returns>
        public RDPINPUT_SUSPEND_TOUCH_PDU CreateRdpInputSuspendTouchPdu()
        {
            RDPINPUT_SUSPEND_TOUCH_PDU pdu = new RDPINPUT_SUSPEND_TOUCH_PDU();
            pdu.header.eventId = EventId_Values.EVENTID_SUSPEND_TOUCH;
            pdu.header.pduLength = pdu.Length();
            return pdu;
        }

        /// <summary>
        /// Method to create a RDPINPUT_RESUME_TOUCH_PDU.
        /// </summary>
        /// <returns>RDPINPUT_RESUME_TOUCH_PDU</returns>
        public RDPINPUT_RESUME_TOUCH_PDU CreateRdpInputResumeTouchPdu()
        {
            RDPINPUT_RESUME_TOUCH_PDU pdu = new RDPINPUT_RESUME_TOUCH_PDU();
            pdu.header.eventId = EventId_Values.EVENTID_RESUME_TOUCH;
            pdu.header.pduLength = pdu.Length();
            return pdu;
        }

        #endregion

        #region Send Methods

        /// <summary>
        /// Method to send a RDPINPUT_SC_READY_PDU to client.
        /// </summary>
        /// <param name="pdu">A RDPINPUT_SC_READY_PDU structure.</param>
        public void SendRdpInputScReadyPdu(RDPINPUT_SC_READY_PDU pdu)
        {
            byte[] data = PduMarshaler.Marshal(pdu);

            // Sleep some time to avoid this packet to be merged with other packets in TCP level.
            System.Threading.Thread.Sleep(PacketsInterval);
            if (rdpeiDVC == null)
            {
                throw new InvalidOperationException("DVC instance of RDPEI is null, Dynamic virtual channel must be created before sending data.");
            }
            rdpeiDVC.Send(data);
        }

        /// <summary>
        /// Method to send a RDPINPUT_SUSPEND_TOUCH_PDU to client.
        /// </summary>
        /// <param name="pdu">A RDPINPUT_SUSPEND_TOUCH_PDU structure.</param>
        public void SendRdpInputSuspendTouchPdu(RDPINPUT_SUSPEND_TOUCH_PDU pdu)
        {
            byte[] data = PduMarshaler.Marshal(pdu);

            // Sleep some time to avoid this packet to be merged with other packets in TCP level.
            System.Threading.Thread.Sleep(PacketsInterval);
            if (rdpeiDVC == null)
            {
                throw new InvalidOperationException("DVC instance of RDPEI is null, Dynamic virtual channel must be created before sending data.");
            }
            rdpeiDVC.Send(data);
        }

        /// <summary>
        /// Method to send a RDPINPUT_RESUME_TOUCH_PDU to client.
        /// </summary>
        /// <param name="pdu">A RDPINPUT_RESUME_TOUCH_PDU structure.</param>
        public void SendRdpInputResumeTouchPdu(RDPINPUT_RESUME_TOUCH_PDU pdu)
        {
            byte[] data = PduMarshaler.Marshal(pdu);

            // Sleep some time to avoid this packet to be merged with other packets in TCP level.
            System.Threading.Thread.Sleep(PacketsInterval);
            if (rdpeiDVC == null)
            {
                throw new InvalidOperationException("DVC instance of RDPEI is null, Dynamic virtual channel must be created before sending data.");
            }
            rdpeiDVC.Send(data);
        }

        #endregion

        #region Receive Methods

        /// <summary>
        /// Expect a RDPINPUT_CS_READY_PDU.
        /// </summary>
        /// <param name="timeout">TimeOut</param>
        /// <returns>A RDPINPUT_CS_READY_PDU instance.</returns>
        public RDPINPUT_CS_READY_PDU ExpectRdpInputCsReadyPdu(TimeSpan timeout)
        {
            RDPINPUT_CS_READY_PDU pdu = ExpectRdpeiPdu<RDPINPUT_CS_READY_PDU>(timeout);                
            return pdu;
        }

        /// <summary>
        /// Expect a RDPINPUT_TOUCH_EVENT_PDU.
        /// </summary>
        /// <param name="timeout">TimeOut</param>
        /// <returns>A RDPINPUT_TOUCH_EVENT_PDU instance.</returns>
        public RDPINPUT_TOUCH_EVENT_PDU ExpectRdpInputTouchEventPdu(TimeSpan timeout)
        {
            RDPINPUT_TOUCH_EVENT_PDU pdu = ExpectRdpeiPdu<RDPINPUT_TOUCH_EVENT_PDU>(timeout);               
            return pdu;
        }

        /// <summary>
        /// Expect a RDPINPUT_DISMISS_HOVERING_CONTACT_PDU.
        /// </summary>
        /// <param name="timeout">TimeOut</param>
        /// <returns>A RDPINPUT_DISMISS_HOVERING_CONTACT_PDU instance.</returns>
        public RDPINPUT_DISMISS_HOVERING_CONTACT_PDU ExpectRdpInputDismissHoveringContactPdu(TimeSpan timeout)
        {
            RDPINPUT_DISMISS_HOVERING_CONTACT_PDU pdu = ExpectRdpeiPdu<RDPINPUT_DISMISS_HOVERING_CONTACT_PDU>(timeout);                
            return pdu;
        }

        /// <summary>
        /// Expect a RDPINPUT_TOUCH_EVENT_PDU or a RDPINPUT_DISMISS_HOVERING_CONTACT_PDU or a RDPINPUT_CS_READY_PDU.
        /// </summary>
        /// <param name="timeout">TimeOut</param>
        /// <returns>A RDPINPUT_TOUCH_EVENT_PDU or a RDPINPUT_DISMISS_HOVERING_CONTACT_PDU or a RDPINPUT_CS_READY_PDU instance.</returns>
        public RDPINPUT_PDU ExpectRdpInputPdu(TimeSpan timeout)
        {
            RDPINPUT_PDU pdu = ExpectRdpeiPdu<RDPINPUT_PDU>(timeout);    
            return pdu;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method to expect a specified type of Rdpei Pdu.
        /// </summary>
        /// <typeparam name="T">The specified type.</typeparam>
        /// <param name="timeout">Timeout</param>
        /// <returns>Return the specified type of Pdu otherwise, return null.</returns>
        private T ExpectRdpeiPdu<T>(TimeSpan timeout) where T : RDPINPUT_PDU
        {
            DateTime endTime = DateTime.Now + timeout;
            while (endTime > DateTime.Now)
            {
                if (receivedList.Count > 0)
                {
                    lock (receivedList)
                    {
                        foreach (RDPINPUT_PDU pdu in receivedList)
                        {
                            T response = pdu as T;
                            if (response != null)
                            {
                                receivedList.Remove(pdu);
                                return response;
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(PacketsInterval);
            }

            return null;
        }


        /// <summary>
        /// The callback method to receive data from transport layer.
        /// </summary>
        private void OnDataReceived(byte[] data, uint channelId)
        {
            lock (receivedList)
            {

                RDPINPUT_PDU pdu = new RDPINPUT_PDU();
                bool fResult = PduMarshaler.Unmarshal(data, pdu);
                if (fResult)
                {
                    byte[] pduData = new byte[pdu.header.pduLength];
                    Array.Copy(data, pduData, pduData.Length);
                    RDPINPUT_PDU msg = pdu;
                    if (pdu.header.eventId == EventId_Values.EVENTID_CS_READY)
                    {
                        RDPINPUT_CS_READY_PDU request = new RDPINPUT_CS_READY_PDU();
                        if (PduMarshaler.Unmarshal(pduData, request))
                        {
                            msg = request;
                        }
                    }
                    else if (pdu.header.eventId == EventId_Values.EVENTID_DISMISS_HOVERING_CONTACT)
                    {
                        RDPINPUT_DISMISS_HOVERING_CONTACT_PDU request = new RDPINPUT_DISMISS_HOVERING_CONTACT_PDU();
                        if (PduMarshaler.Unmarshal(pduData, request))
                        {
                            msg = request;
                        }
                    }
                    else if (pdu.header.eventId == EventId_Values.EVENTID_TOUCH)
                    {
                        RDPINPUT_TOUCH_EVENT_PDU request = new RDPINPUT_TOUCH_EVENT_PDU();
                        if (PduMarshaler.Unmarshal(pduData, request))
                        {
                            msg = request;
                        }
                    }
                    receivedList.Add(msg);
                }


            }

        }
        #endregion Private Methods
    }
}
