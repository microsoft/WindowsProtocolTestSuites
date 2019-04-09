// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpevor.Client
{
    class RdpevorClient
    {
        #region Varibles

        private RdpedycClient rdpedycClient;
        const string RdpevorControlChannelName = "Microsoft::Windows::RDS::Video::Control::v08.01";
        const string RdpevorDataChannelName = "Microsoft::Windows::RDS::Video::Data::v08.01";

        DynamicVirtualChannel rdpevorControlDVC;
        DynamicVirtualChannel rdpevorDataDVC;

        List<RdpevorPdu> receivedList;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rdpedycServer"></param>
        public RdpevorClient(RdpedycClient rdpedycClient)
        {
            this.rdpedycClient = rdpedycClient;
            receivedList = new List<RdpevorPdu>();
        }

        #endregion Constructor

        /// <summary>
        /// Wait for creation of dynamic virtual channel for RDPEVOR
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="transportType"></param>
        /// <returns></returns>
        public bool WaitForRdpevorDvcCreation(TimeSpan timeout, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            try
            {
                rdpevorControlDVC = rdpedycClient.ExpectChannel(timeout, RdpevorControlChannelName, transportType);
                rdpevorDataDVC = rdpedycClient.ExpectChannel(timeout, RdpevorDataChannelName, transportType);
            }
            catch
            {
            }
            if (rdpevorControlDVC != null && rdpevorDataDVC != null)
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
        /// Send a RDPEVOR packet through control channel
        /// </summary>
        /// <param name="evorPdu"></param>
        public void SendRdpevorControlPdu(RdpevorServerPdu evorPdu)
        {
            byte[] data = PduMarshaler.Marshal(evorPdu);
            if (rdpevorControlDVC == null)
            {
                throw new InvalidOperationException("Control DVC instance of RDPEVOR is null, Dynamic virtual channel must be created before sending data.");
            }
            rdpevorControlDVC.Send(data);

        }

        /// <summary>
        /// Send a RDPEVOR packet through data channel
        /// </summary>
        /// <param name="evorPdu"></param>
        public void SendRdpevorDataPdu(RdpevorServerPdu evorPdu)
        {
            byte[] data = PduMarshaler.Marshal(evorPdu);
            if (rdpevorDataDVC == null)
            {
                throw new InvalidOperationException("Data DVC instance of RDPEVOR is null, Dynamic virtual channel must be created before sending data.");
            }
            rdpevorDataDVC.Send(data);

        }

        /// <summary>
        /// Method to expect a RdpevorPdu.
        /// </summary>
        /// <param name="timeout">Timeout</param>
        public T ExpectRdpevorPdu<T>(TimeSpan timeout) where T : RdpevorPdu
        {
            DateTime endTime = DateTime.Now + timeout;

            while (endTime > DateTime.Now)
            {
                if (receivedList.Count > 0)
                {
                    lock (receivedList)
                    {
                        foreach (RdpevorPdu pdu in receivedList)
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
        private void OnDataReceived(byte[] data, uint channelId)
        {
            lock (receivedList)
            {
                RdpevorPdu pdu = new RdpevorPdu();
                bool fSucceed = false;
                bool fResult = PduMarshaler.Unmarshal(data, pdu);
                if (fResult)
                {
                    byte[] pduData = new byte[pdu.Header.cbSize];
                    Array.Copy(data, pduData, pduData.Length);
                    if (pdu.Header.PacketType == PacketTypeValues.TSMM_PACKET_TYPE_PRESENTATION_REQUEST)
                    {
                        TSMM_PRESENTATION_REQUEST request = new TSMM_PRESENTATION_REQUEST();
                        try
                        {
                            fSucceed = PduMarshaler.Unmarshal(pduData, request);
                            receivedList.Add(request);
                        }
                        catch (PDUDecodeException decodeExceptioin)
                        {
                            RdpevorUnkownPdu unkown = new RdpevorUnkownPdu();
                            fSucceed = PduMarshaler.Unmarshal(decodeExceptioin.DecodingData, unkown);
                            receivedList.Add(unkown);
                        }
                    }
                    else if (pdu.Header.PacketType == PacketTypeValues.TSMM_PACKET_TYPE_VIDEO_DATA)
                    {
                        TSMM_VIDEO_DATA notficatioin = new TSMM_VIDEO_DATA();
                        try
                        {
                            fSucceed = PduMarshaler.Unmarshal(pduData, notficatioin);
                            receivedList.Add(notficatioin);
                        }
                        catch (PDUDecodeException decodeExceptioin)
                        {
                            RdpevorUnkownPdu unkown = new RdpevorUnkownPdu();
                            fSucceed = PduMarshaler.Unmarshal(decodeExceptioin.DecodingData, unkown);
                            receivedList.Add(unkown);
                        }
                    }

                }
                if (!fResult || !fSucceed)
                {
                    RdpevorUnkownPdu unkown = new RdpevorUnkownPdu();
                    PduMarshaler.Unmarshal(data, unkown);
                    receivedList.Add(unkown);
                }
            }
        }

        #endregion Private Methods
    }
}
