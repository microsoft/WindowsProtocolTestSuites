// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedisp;
using Microsoft.Protocols.TestSuites.Rdpegfx;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdprfx;
using System.Drawing;

namespace Microsoft.Protocols.TestSuites.Rdpedisp
{
    public interface IRdpedispAdapter : IAdapter
    {

        /// <summary>
        /// Attach a RdpegfxAdapter object
        /// </summary>
        /// <param name="rdpegfxAdapter">the source RdpegfxAdapter object</param>
        void AttachRdpegfxAdapter(IRdpegfxAdapter rdpegfxAdapter);

        /// <summary>
        /// Attach a RdprfxAdapter object
        /// </summary>
        /// <param name="rdpegfxAdapter">the source RdprfxAdapter object</param>
        void AttachRdprfxAdapter(IRdprfxAdapter rdprfxAdapter);

        /// <summary>
        /// Attach a RdpbcgrAdapter object
        /// </summary>
        /// <param name="rdpegfxAdapter">the source RdpbcgrAdapter object</param>
        void AttachRdpbcgrAdapter(IRdpbcgrAdapter rdpbcgrAdapter);

        /// <summary>
        /// Initialize this protocol with create control and data channels.
        /// </summary>
        /// <param name="rdpedycserver">RDPEDYC Server instance</param>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <returns>true if client supports this protocol; otherwise, return false.</returns>
        bool ProtocolInitialize(RdpedycServer rdpedycserver, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP);

        /// <summary>
        /// Method to send DISPLAYCONTROL_CAPS_PDU PDU to client
        /// </summary>
        /// <param name="maxNumMonitors">A 32-bit unsigned integer that specifies the maximum number of monitors supported by the server.</param>
        /// <param name="maxMonitorAreaFactorA">A 32-bit unsigned integer that is used to specify the maximum monitor area supported by the server. </param>
        /// <param name="maxMonitorAreaFactorB">A 32-bit unsigned integer that is used to specify the maximum monitor area supported by the server. </param>
        void sendCapsPDU(
            uint maxNumMonitors,
            uint maxMonitorAreaFactorA,
            uint maxMonitorAreaFactorB);

        /// <summary>
        /// Method to expect DISPLAYCONTROL_MONITOR_LAYOUT_PDU PDU
        /// </summary>
        /// <returns></returns>
        DISPLAYCONTROL_MONITOR_LAYOUT_PDU expectMonitorLayoutPDU();

        /// <summary>
        /// Clear ReceiveList of RdpedispServer
        /// </summary>
        void ClearReceivedList();

        /// <summary>
        /// Method to send client a display configuration change notification by Deactivation-Reactivation Sequence 
        /// </summary>
        void initiateDeactivationReactivation(ushort desktopWidth, ushort desktopHeight);

        /// <summary>
        /// Using RDPRFX to send image whose resolution is Width x Height
        /// </summary>
        /// <param name="imagefile">Image to send</param>
        /// <param name="width">width of image to send</param>
        /// <param name="height">height of image to send</param>
        bool RdprfxSendImage(Image image, ushort width, ushort height);

        /// <summary>
        /// Method to send client a display configuration change notification by sending surface management commands to restart graphics pipeline 
        /// </summary>
        /// <param name="width">Width of screen</param>
        /// <param name="height">Height of Screen</param>
        void restartGraphicsPipeline(ushort width, ushort height);
    }
}
