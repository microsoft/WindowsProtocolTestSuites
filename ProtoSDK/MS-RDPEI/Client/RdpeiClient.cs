// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpei
{
    public class RdpeiClient
    {
        #region Variables
        const string rdpeiChannelName = "Microsoft::Windows::RDS::Input";

        // Instance of RDPEDYC client
        private RdpedycClient rdpedycClient;

        // Dynamic virtual channel of RDPEI
        private DynamicVirtualChannel rdpeiDVC;

        // Buffer of received packets
        private List<RDPINPUT_PDU> receivedList;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rdpedycClient"></param>
        public RdpeiClient(RdpedycClient rdpedycClient)
        {
            this.rdpedycClient = rdpedycClient;
            receivedList = new List<RDPINPUT_PDU>();
        }

        #endregion Constructor

        /// <summary>
        /// Wait for creation of dynamic virtual channel for RDPEGFX
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="transportType"></param>
        /// <returns></returns>
        public bool WaitForRdpegfxDvcCreation(TimeSpan timeout, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            try
            {
                rdpeiDVC = rdpedycClient.ExpectChannel(timeout, rdpeiChannelName, transportType);
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

        /// <summary>
        /// Clear ReceiveList 
        /// </summary>
        public void ClearReceivedList()
        {
            if (this.receivedList != null)
            {
                this.receivedList.Clear();
            }
        }

        #region Send/Receive Methods

        /// <summary>
        /// Send a Pdu
        /// </summary>
        /// <param name="pdu"></param>
        public void SendRdpeiPdu(RDPINPUT_PDU pdu)
        {
            byte[] data = PduMarshaler.Marshal(pdu);
            if (rdpeiDVC == null)
            {
                throw new InvalidOperationException("DVC instance of RDPEI is null, Dynamic virtual channel must be created before sending data.");
            }
            rdpeiDVC.Send(data);
        }

        /// <summary>
        /// Method to expect a RdpegfxPdu.
        /// </summary>
        /// <param name="timeout">Timeout</param>
        public T ExpectRdpeiPdu<T>(TimeSpan timeout) where T : RDPINPUT_PDU
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
                System.Threading.Thread.Sleep(100);
            }
            return null;
        }

        #endregion Send/Receive Methods

        #region Private Methods

        /// <summary>
        /// The callback method to receive data from transport layer.
        /// </summary>
        private void OnDataReceived(byte[] data, uint channelID)
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
                    if (pdu.header.eventId == EventId_Values.EVENTID_SC_READY)
                    {
                        RDPINPUT_SC_READY_PDU request = new RDPINPUT_SC_READY_PDU();
                        if (PduMarshaler.Unmarshal(pduData, request))
                        {
                            msg = request;
                        }
                    }
                    else if (pdu.header.eventId == EventId_Values.EVENTID_SUSPEND_TOUCH)
                    {
                        RDPINPUT_SUSPEND_TOUCH_PDU request = new RDPINPUT_SUSPEND_TOUCH_PDU();
                        if (PduMarshaler.Unmarshal(pduData, request))
                        {
                            msg = request;
                        }
                    }
                    else if (pdu.header.eventId == EventId_Values.EVENTID_RESUME_TOUCH)
                    {
                        RDPINPUT_RESUME_TOUCH_PDU request = new RDPINPUT_RESUME_TOUCH_PDU();
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
