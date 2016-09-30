// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpefs
{
    public class RdpefsServer
    {
        #region variable
        private const string rdpefsChannelName = "rdpdr";
        private RdpedycServer rdpedycServer;
        private DynamicVirtualChannel rdpefsDVC;
        private List<RdpefsPDU> receivedList;     
        #endregion

        #region Contructor
        public RdpefsServer() { }

        public RdpefsServer(RdpedycServer rdpedycServer)
        {
            this.rdpedycServer = rdpedycServer;
            receivedList = new List<RdpefsPDU>();
        }
        #endregion 

        #region Dispose
        /// <summary>
        /// Clear received list pdu.
        /// </summary>
        public void ClearReceivedList()
        {
            if (this.receivedList != null)
            {
                this.receivedList.Clear();
            }
        }

        /// <summary>
        /// Release the managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (rdpedycServer != null)
                rdpedycServer.Dispose();
            ClearReceivedList();
        }

        #endregion

        #region Create dynamic DVC
        /// <summary>
        /// Create efs DVC
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <param name="channelId">DVC channel ID</param>
        /// <returns>return create results.</returns>
        public bool CreateRdpefsDvc(TimeSpan timeout, uint? channelId = null)
        {
            const ushort priority = 0;
            try
            {
                if(channelId == null)
                    channelId = DynamicVirtualChannel.NewChannelId();

                rdpefsDVC = rdpedycServer.CreateChannel(timeout, priority, rdpefsChannelName, DynamicVC_TransportType.RDP_UDP_Reliable, OnDataReceived, channelId);
            }
            catch
            {
                throw new InvalidOperationException("DVC createed failed.");
            }

            return rdpefsDVC == null;
        }
        #endregion 

        #region Send & Receive Methods
        /// <summary>
        /// Send EFS PDU.
        /// </summary>
        /// <param name="pdu">the PDU sent to Client</param>
        public void SendRdpefsPdu(RdpefsPDU pdu)
        {
            byte[] data = PduMarshaler.Marshal(pdu);
            if(rdpefsDVC == null)
            {
                throw new InvalidOperationException("DVC instance of RDPEFS is null, Dynamic virtual channel must be created before sending data.");
            }

            //Will send compressed packet with "true" as parameter: rdpefsDVC.Send(data, true);            
            rdpefsDVC.Send(data);
        }

        /// <summary>
        /// Expect efs PDU
        /// </summary>
        /// <typeparam name="T">PDU type</typeparam>
        /// <param name="timeout">timeout</param>
        /// <returns>Return the expected PDU</returns>
        public T ExpectRdpefsPdu<T>(TimeSpan timeout) where T : RdpefsPDU
        {
            DateTime endTime = DateTime.Now + timeout;

            while (endTime > DateTime.Now)
            {
                if (receivedList.Count > 0)
                {
                    lock (receivedList)
                    {
                        foreach (RdpefsPDU pdu in receivedList)
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

        #endregion

        #region encode & decode messages for static vc on bcgr.
        /// <summary>
        /// Encode Rpdefs pdu.
        /// </summary>
        /// <typeparam name="T">Rdpefs PDU</typeparam>
        /// <param name="pdu">PDU instance</param>
        /// <returns>Encoded byte value</returns>
        public byte[] EncodeServerPdu<T>(T pdu) where T : RdpefsPDU
        {
            return PduMarshaler.Marshal(pdu);
        }

        /// <summary>
        /// Decode Rdpefs pdu
        /// </summary>
        /// <typeparam name="T">Rdpefs PDU</typeparam>
        /// <param name="data">data to be parsed</param>
        /// <param name="pdu">Pdu instance</param>
        /// <returns>Return result</returns>
        public bool DecodeClientPdu<T>(byte[] data, T pdu) where T : RdpefsPDU
        {
            return PduMarshaler.Unmarshal(data, pdu);
        }
        #endregion

        #region Private Methods
        private void OnDataReceived(byte[] data, uint channelId)
        {
            lock(receivedList)
            {
                RdpefsPDU pdu = new RdpefsPDU();
                bool fSucceed = false;
                bool fResult = PduMarshaler.Unmarshal(data, pdu);
                if(fResult)
                {
                    if(pdu.Header.PacketId == PacketId_Values.PAKID_CORE_CLIENTID_CONFIRM)
                    {
                        // Header(4) + VersionMajor(2) + VersionMinor(2) + ClientId(4) = 12
                        DR_CORE_CLIENT_ANNOUNCE_RSP response = new DR_CORE_CLIENT_ANNOUNCE_RSP();
                        if(PduMarshaler.Unmarshal(data, response))
                        {
                            receivedList.Add(response);
                        }
                    }
                    else if(pdu.Header.PacketId == PacketId_Values.PAKID_CORE_CLIENT_NAME)
                    {
                        // Header(4) + UnicodeFlag(4) + CodePage(4) + ComputerNameLen(4) + ComputerNmae(ComputerNameLen)
                        DR_CORE_CLIENT_NAME_REQ request = new DR_CORE_CLIENT_NAME_REQ();
                        if(PduMarshaler.Unmarshal(data, request))
                        {
                            receivedList.Add(request);
                        }
                    }
                    else if(pdu.Header.PacketId == PacketId_Values.PAKID_CORE_CLIENT_CAPABILITY)
                    {
                        // Header(4) + numCapabilities(2) + Padding(2) + CapabilityMessage(numCapabilities * CAPABILITE_SET)
                        DR_CORE_CAPABILITY_RSP response = new DR_CORE_CAPABILITY_RSP();
                        if(PduMarshaler.Unmarshal(data, response))
                        {
                            receivedList.Add(response);
                        }
                    }
                    else if(pdu.Header.PacketId == PacketId_Values.PAKID_CORE_DEVICELIST_ANNOUNCE)
                    {
                        // Header(4) + DeviceCount(4) + DeviceList(DeviceCount * DEVICE_ANNONUNCE)
                        DR_CORE_DEVICELIST_ANNOUNCE_REQ request = new DR_CORE_DEVICELIST_ANNOUNCE_REQ();
                        if(PduMarshaler.Unmarshal(data, request))
                        {
                            receivedList.Add(request);
                        }
                    }
                }
                if (!fSucceed || !fResult)
                {
                    RdpefsUnknownPdu unknown = new RdpefsUnknownPdu();
                    if(PduMarshaler.Unmarshal(data, unknown))
                    {
                        receivedList.Add(unknown);
                    }
                }
            }
        }
        #endregion
    }
}
