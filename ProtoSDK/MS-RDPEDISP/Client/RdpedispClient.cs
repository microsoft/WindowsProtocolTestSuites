// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedisp
{
    class RdpedispClient
    {
        bool forceRestriction = false; // Restriction in MS-RDPEDISP section 2.2.2.2.1 is forced or not
        const uint FixedMonitorLayoutSize = 40; // Size of DISPLAYCONTROL_MONITOR_LAYOUT is fixed to 40
        const String RdpedispChannelName = "Microsoft::Windows::RDS::DisplayControl";

        const uint MinMonitorSize = 200;
        const uint MaxMonitorSize = 8192;

        private RdpedycClient rdpedycClient;
        private DynamicVirtualChannel RdpedispDVC;
        private List<RdpedispPdu> receivedList;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rdpedycClient"></param>
        public RdpedispClient(RdpedycClient rdpedycClient)
        {
            this.rdpedycClient = rdpedycClient;
            receivedList = new List<RdpedispPdu>();
        }

        #endregion Constructor

        /// <summary>
        /// Wait for creation of dynamic virtual channel for RDPEDISP
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="transportType"></param>
        /// <returns></returns>
        public bool WaitForRdpedispDvcCreation(TimeSpan timeout, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            try
            {
                RdpedispDVC = rdpedycClient.ExpectChannel(timeout, RdpedispChannelName, transportType);
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
        /// Method to create DISPLAYCONTROL_MONITOR_LAYOUT structure
        /// </summary>
        /// <param name="flags">A 32-bit unsigned integer that specifies monitor configuration flags.</param>
        /// <param name="left">A 32-bit signed integer that specifies the x-coordinate of the upper-left corner of the display monitor.</param>
        /// <param name="top">A 32-bit signed integer that specifies the y-coordinate of the upper-left corner of the display monitor.</param>
        /// <param name="width">A 32-bit unsigned integer that specifies the width of the monitor in pixels. </param>
        /// <param name="height">A 32-bit unsigned integer that specifies the height of the monitor in pixels. </param>
        /// <param name="physicalWidth">A 32-bit unsigned integer that specifies the physical width of the monitor, in millimeters (mm).</param>
        /// <param name="physicalHeight">A 32-bit unsigned integer that specifies the physical height of the monitor, in millimeters.</param>
        /// <param name="orientation">A 32-bit unsigned integer that specifies the orientation of the monitor in degrees.</param>
        /// <param name="desktopScaleFactor">A 32-bit, unsigned integer that specifies the desktop scale factor of the monitor.</param>
        /// <param name="deviceScaleFactor">A 32-bit, unsigned integer that specifies the device scale factor of the monitor. </param>
        /// <returns></returns>
        public DISPLAYCONTROL_MONITOR_LAYOUT createMonitorLayout(
            MonitorLayout_FlagValues flags,
            int left,
            int top,
            uint width,
            uint height,
            uint physicalWidth,
            uint physicalHeight,
            MonitorLayout_OrientationValues orientation,
            uint desktopScaleFactor,
            uint deviceScaleFactor)
        {
            DISPLAYCONTROL_MONITOR_LAYOUT monitorLayout = new DISPLAYCONTROL_MONITOR_LAYOUT();
            monitorLayout.Flags = flags;
            monitorLayout.Left = left;
            monitorLayout.Top = top;
            if (forceRestriction && width >= MinMonitorSize && width <= MaxMonitorSize && width % 2 == 0)
            {
                monitorLayout.Width = width;
            }
            else
            {
                return null;
            }
            if (forceRestriction && height >= MinMonitorSize && height <= MaxMonitorSize)
            {
                monitorLayout.Height = height;
            }
            else
            {
                return null;
            }
            monitorLayout.PhysicalWidth = physicalWidth;
            monitorLayout.PhysicalHeight = physicalHeight;
            monitorLayout.Orientation = orientation;
            monitorLayout.DesktopScaleFactor = desktopScaleFactor;
            monitorLayout.DeviceScaleFactor = desktopScaleFactor;
            return monitorLayout;
        }

        /// <summary>
        /// Method to create DISPLAYCONTROL_MONITOR_LAYOUT_PDU structure
        /// </summary>
        /// <param name="monitors">Array of monitors</param>
        /// <returns></returns>
        public DISPLAYCONTROL_MONITOR_LAYOUT_PDU createMonitorLayoutPDU(DISPLAYCONTROL_MONITOR_LAYOUT[] monitors){
            DISPLAYCONTROL_MONITOR_LAYOUT_PDU monitorLayoutPDU = new DISPLAYCONTROL_MONITOR_LAYOUT_PDU();
            monitorLayoutPDU.Header.Type = PDUTypeValues.DISPLAYCONTROL_PDU_TYPE_MONITOR_LAYOUT;
            if (null == monitors || monitors.Length == 0)
            {
                monitorLayoutPDU.Header.Length = 16;
                monitorLayoutPDU.MonitorLayoutSize = FixedMonitorLayoutSize;
                // TODO: illegal NumMonitors?
                monitorLayoutPDU.NumMonitors = 0;
                return monitorLayoutPDU;
            }else{
                monitorLayoutPDU.Header.Length = 16 + FixedMonitorLayoutSize * (uint) monitors.Length;
                monitorLayoutPDU.MonitorLayoutSize = FixedMonitorLayoutSize;
                monitorLayoutPDU.NumMonitors = (uint) monitors.Length;
                // TODO: illegal NumMonitors?
                monitorLayoutPDU.Monitors = monitors;
                return monitorLayoutPDU;
            }
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
