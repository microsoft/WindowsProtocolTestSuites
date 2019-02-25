// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegt
{
    public class RdpegtClient
    {
        #region Variables

        private const string RdpegtChannelName = "Microsoft::Windows::RDS::Geometry::v08.01";
        
        // Instance of RDPEDYC client
        private RdpedycClient rdpedycClient;

        // Dynamic virtual channel of RDPEGFX
        private DynamicVirtualChannel rdpegtDVC;

        // Buffer of received packets
        private List<MAPPED_GEOMETRY_PACKET> receivedList;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rdpedycClient"></param>
        public RdpegtClient(RdpedycClient rdpedycClient)
        {
            this.rdpedycClient = rdpedycClient;
            receivedList = new List<MAPPED_GEOMETRY_PACKET>();
        }

        #endregion Constructor

        /// <summary>
        /// Wait for creation of dynamic virtual channel for RDPEGT
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="transportType"></param>
        /// <returns></returns>
        public bool WaitForRdpegtDvcCreation(TimeSpan timeout, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            try
            {
                rdpegtDVC = rdpedycClient.ExpectChannel(timeout, RdpegtChannelName, transportType);
            }
            catch
            {
            }
            if (rdpegtDVC != null)
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
        
        #region Receive Methods

        /// <summary>
        /// Method to expect a MAPPED_GEOMETRY_PACKET.
        /// </summary>
        /// <param name="timeout">Timeout</param>
        public MAPPED_GEOMETRY_PACKET ExpectRdpegfxPdu(TimeSpan timeout)
        {
            DateTime endTime = DateTime.Now + timeout;

            while (endTime > DateTime.Now)
            {
                if (receivedList.Count > 0)
                {
                    lock (receivedList)
                    {
                        foreach (MAPPED_GEOMETRY_PACKET pdu in receivedList)
                        {
                            MAPPED_GEOMETRY_PACKET response = pdu;
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

        #endregion Receive Methods

        #region Private Methods

        /// <summary>
        /// The callback method to receive data from transport layer.
        /// </summary>
        private void OnDataReceived(byte[] data, uint channelID)
        {
            MAPPED_GEOMETRY_PACKET pdu = new MAPPED_GEOMETRY_PACKET();
            bool fResult = PduMarshaler.Unmarshal(data, pdu);
            if (fResult)
            {
                lock (receivedList)
                {
                    receivedList.Add(pdu);
                }
            }
        }

        #endregion Private Methods
    }
}
