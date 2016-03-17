// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedisp
{
    public class RdpedispServer
    {
        #region Variables

        const int CapsPduSize = 20; // Size of DISPLAYCONTROL_CAPS_PDU is fixed to 20

        const String RdpedispChannelName = "Microsoft::Windows::RDS::DisplayControl";

        private RdpedycServer rdpedycServer;
        private DynamicVirtualChannel RdpedispDVC;

        private List<RdpedispPdu> receivedList;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rdpedycserver"></param>
        public RdpedispServer(RdpedycServer rdpedycserver)
        {
            this.rdpedycServer = rdpedycserver;
            receivedList = new List<RdpedispPdu>();
        }

        #endregion Constructor

        /// <summary>
        /// Create dynamic virtual channel.
        /// </summary>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <param name="timeout">Timeout</param>
        /// <returns>true if client supports this protocol; otherwise, return false.</returns>
        public bool CreateRdpedispDvc(TimeSpan timeout, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {

            const ushort priority = 0;
            try
            {
                RdpedispDVC = rdpedycServer.CreateChannel(timeout, priority, RdpedispChannelName, transportType, OnDataReceived);
            }
            catch
            {
            }
            if (RdpedispDVC != null)
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
        /// Send a RDPEDISP Pdu
        /// </summary>
        /// <param name="pdu"></param>
        public void SendRdpedispPdu(RdpedispPdu pdu)
        {
            byte[] data = PduMarshaler.Marshal(pdu);
            if (RdpedispDVC == null)
            {
                throw new InvalidOperationException("DVC instance of RDPEDISP is null, Dynamic virtual channel must be created before sending data.");
            }
            RdpedispDVC.Send(data);
        }
        /// <summary>
        /// Method to expect a RdpedispPdu.
        /// </summary>
        /// <param name="timeout">Timeout</param>
        public T ExpectRdpedispPdu<T>(TimeSpan timeout) where T : RdpedispPdu
        {
            DateTime endTime = DateTime.Now + timeout;

            while (endTime > DateTime.Now)
            {
                if (receivedList.Count > 0)
                {
                    lock (receivedList)
                    {
                        foreach (RdpedispPdu pdu in receivedList)
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

        #region Create Methods

        /// <summary>
        /// Method to create DISPLAYCONTROL_CAPS_PDU PDU
        /// </summary>
        /// <param name="maxNumMonitors">A 32-bit unsigned integer that specifies the maximum number of monitors supported by the server</param>
        /// <param name="maxMonitorAreaFactorA">A 32-bit unsigned integer that is used to specify the maximum monitor area supported by the server. </param>
        /// <param name="maxMonitorAreaFactorB">A 32-bit unsigned integer that is used to specify the maximum monitor area supported by the server. </param>
        /// <returns></returns>
        public DISPLAYCONTROL_CAPS_PDU createDisplayControlCapsPdu(
            uint maxNumMonitors,
            uint maxMonitorAreaFactorA,
            uint maxMonitorAreaFactorB)
        {
            DISPLAYCONTROL_CAPS_PDU capsPdu = new DISPLAYCONTROL_CAPS_PDU();
            capsPdu.Header.Type = PDUTypeValues.DISPLAYCONTROL_PDU_TYPE_CAPS;
            capsPdu.Header.Length = CapsPduSize;
            capsPdu.MaxNumMonitors = maxNumMonitors;
            capsPdu.MaxMonitorAreaFactorA = maxMonitorAreaFactorA;
            capsPdu.MaxMonitorAreaFactorB = maxMonitorAreaFactorB;
            return capsPdu;
        }

        #endregion Create Methods

        #region Private Methods

        /// <summary>
        /// The callback method to receive data from transport layer.
        /// </summary>
        private void OnDataReceived(byte[] data, uint channelID)
        {
            lock (receivedList)
            {

                RdpedispPdu basePDU = new RdpedispPdu();
                bool fSucceed = false;
                bool fResult = PduMarshaler.Unmarshal(data, basePDU);
                if (fResult)
                {
                    byte[] pduData = new byte[basePDU.Header.Length];
                    Array.Copy(data, pduData, basePDU.Header.Length);
                    if (basePDU.Header.Type == PDUTypeValues.DISPLAYCONTROL_PDU_TYPE_CAPS)
                    {
                        DISPLAYCONTROL_CAPS_PDU capsPDU = new DISPLAYCONTROL_CAPS_PDU();
                        try
                        {
                            fSucceed = PduMarshaler.Unmarshal(pduData, capsPDU);
                            receivedList.Add(capsPDU);
                        }
                        catch (PDUDecodeException decodeException)
                        {
                            RdpedispUnkownPdu unkonw = new RdpedispUnkownPdu();
                            fSucceed = PduMarshaler.Unmarshal(decodeException.DecodingData, unkonw);
                            receivedList.Add(unkonw);
                        }
                    }
                    else if (basePDU.Header.Type == PDUTypeValues.DISPLAYCONTROL_PDU_TYPE_MONITOR_LAYOUT)
                    {
                        DISPLAYCONTROL_MONITOR_LAYOUT_PDU monitorLayoutPDU = new DISPLAYCONTROL_MONITOR_LAYOUT_PDU();
                        try
                        {
                            fSucceed = PduMarshaler.Unmarshal(pduData, monitorLayoutPDU);
                            receivedList.Add(monitorLayoutPDU);
                        }
                        catch (PDUDecodeException decodeException)
                        {
                            RdpedispUnkownPdu unkonw = new RdpedispUnkownPdu();
                            fSucceed = PduMarshaler.Unmarshal(decodeException.DecodingData, unkonw);
                            receivedList.Add(unkonw);
                        }
                    }
                    else
                    {
                        RdpedispUnkownPdu unkonw = new RdpedispUnkownPdu();
                        fSucceed = PduMarshaler.Unmarshal(pduData, unkonw);
                        receivedList.Add(unkonw);
                    }
                }
                if (!fSucceed || !fResult)
                {
                    RdpedispUnkownPdu unkonw = new RdpedispUnkownPdu();
                    fSucceed = PduMarshaler.Unmarshal(data, unkonw);
                    receivedList.Add(unkonw);
                }

            }
        }

        #endregion Private Methods
    }
}
