// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdpegfx;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx;
using System.Drawing;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdprfx;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedisp;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;


namespace Microsoft.Protocols.TestSuites.Rdpedisp
{
    class RdpedispAdapter : ManagedAdapterBase, IRdpedispAdapter
    {

        #region Private Variables

        const String RdpedispChannelName = "Microsoft::Windows::RDS::DisplayControl";
        
        RdpedispServer rdpedispServer;
        TimeSpan waitTime;
        IRdpegfxAdapter rdpegfxAdapter;
        IRdprfxAdapter rdprfxAdapter;
        IRdpbcgrAdapter rdpbcgrAdapter;

        #endregion

        #region IAdapter Members

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            rdpegfxAdapter = null;
            rdprfxAdapter = null;
            rdpbcgrAdapter = null;

            #region WaitTime
            string strWaitTime = Site.Properties["WaitTime"];
            if (strWaitTime != null)
            {
                int waitSeconds = Int32.Parse(strWaitTime);
                waitTime = new TimeSpan(0, 0, waitSeconds);
            }
            else
            {
                waitTime = new TimeSpan(0, 0, 20);
            }
            #endregion

        }

        public override void Reset()
        {
            base.Reset();        
            rdpegfxAdapter = null;
            rdprfxAdapter = null;
            rdpbcgrAdapter = null;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #endregion


        #region IRdpedispAdapter Methods

        /// <summary>
        /// Attach a RdpegfxAdapter object
        /// </summary>
        /// <param name="rdpegfxAdapter">the source RdpegfxAdapter object</param>
        public void AttachRdpegfxAdapter(IRdpegfxAdapter rdpegfxAdapter)
        {
            this.rdpegfxAdapter = rdpegfxAdapter;
            this.rdpegfxAdapter.AttachRdpbcgrAdapter(this.rdpbcgrAdapter);
        }

        /// <summary>
        /// Attach a RdprfxAdapter object
        /// </summary>
        /// <param name="rdpegfxAdapter">the source RdprfxAdapter object</param>
        public void AttachRdprfxAdapter(IRdprfxAdapter rdprfxAdapter)
        {
            this.rdprfxAdapter = rdprfxAdapter;
        }

        /// <summary>
        /// Attach a RdpbcgrAdapter object
        /// </summary>
        /// <param name="rdpegfxAdapter">the source RdpbcgrAdapter object</param>
        public void AttachRdpbcgrAdapter(IRdpbcgrAdapter rdpbcgrAdapter)
        {
            this.rdpbcgrAdapter = rdpbcgrAdapter;
        }

        /// <summary>
        /// Initialize this protocol with create control and data channels.
        /// </summary>
        /// <param name="rdpedycserver">RDPEDYC Server instance</param>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <returns>true if client supports this protocol; otherwise, return false.</returns>
        public bool ProtocolInitialize(RdpedycServer rdpedycServer, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            if (!rdpedycServer.IsMultipleTransportCreated(transportType))
            {
                rdpedycServer.CreateMultipleTransport(transportType);
            }
            this.rdpedispServer = new RdpedispServer(rdpedycServer);
            bool success = false;
            try
            {
                success = rdpedispServer.CreateRdpedispDvc(waitTime, transportType);
            }
            catch (Exception e)
            {
                Site.Log.Add(LogEntryKind.Comment, "Exception occurred when creating RDPEDISP channels: {1}", e.Message);
            }
            return success;
        }

        /// <summary>
        /// Method to send DISPLAYCONTROL_CAPS_PDU PDU to client
        /// </summary>
        /// <param name="maxNumMonitors">A 32-bit unsigned integer that specifies the maximum number of monitors supported by the server.</param>
        /// <param name="maxMonitorAreaFactorA">A 32-bit unsigned integer that is used to specify the maximum monitor area supported by the server. </param>
        /// <param name="maxMonitorAreaFactorB">A 32-bit unsigned integer that is used to specify the maximum monitor area supported by the server. </param>
        public void sendCapsPDU(uint maxNumMonitors, uint maxMonitorAreaFactorA, uint maxMonitorAreaFactorB)
        {
            DISPLAYCONTROL_CAPS_PDU capsPDU = rdpedispServer.createDisplayControlCapsPdu(maxNumMonitors, maxMonitorAreaFactorA, maxMonitorAreaFactorB);
            this.rdpedispServer.SendRdpedispPdu(capsPDU);
        }

        /// <summary>
        /// Method to expect DISPLAYCONTROL_MONITOR_LAYOUT_PDU PDU
        /// </summary>
        /// <returns></returns>
        public DISPLAYCONTROL_MONITOR_LAYOUT_PDU expectMonitorLayoutPDU()
        {
            DISPLAYCONTROL_MONITOR_LAYOUT_PDU MonitorLayoutPDU = rdpedispServer.ExpectRdpedispPdu<DISPLAYCONTROL_MONITOR_LAYOUT_PDU>(waitTime);            
            return MonitorLayoutPDU;
        }

        /// <summary>
        /// Clear ReceiveList of RdpedispServer
        /// </summary>
        public void ClearReceivedList()
        {
            this.rdpedispServer.ClearReceivedList();
        }

        /// <summary>
        /// Method to send client a display configuration change notification by Deactivation-Reactivation Sequence 
        /// </summary>
        public void initiateDeactivationReactivation(ushort desktopWidth, ushort desktopHeight)
        {
            // set DesktopWidth and DesktopHeight properties of Bitmap Capability Set in Rdpbcgr
            this.rdpbcgrAdapter.CapabilitySetting.DesktopWidth = desktopWidth;
            this.rdpbcgrAdapter.CapabilitySetting.DesktopHeight = desktopHeight;
            // Initial a Deactivation-Reactivation Sequence
            this.rdpbcgrAdapter.DeactivateReactivate();
        }

        /// <summary>
        /// Using RDPRFX to send image whose resolution is Width x Height
        /// </summary>
        /// <param name="imagefile">Image to send</param>
        /// <param name="width">Width of image to send</param>
        /// <param name="height">Height of image to send</param>
        public bool RdprfxSendImage(Image image, ushort width, ushort height)
        {
            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = OperationalMode.ImageMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            // Crop Image
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            Rectangle section = new Rectangle(0, 0, width, height);
            graphics.DrawImage(image, 0, 0, section, GraphicsUnit.Pixel);

            //Check if the above setting is supported by the client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                return false;
            }
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            rdprfxAdapter.SendImageToClient(bitmap, opMode, enAlgorithm, destLeft, destTop);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);
            return true;
        }

        /// <summary>
        /// Method to send client a display configuration change notification by sending surface management commands to restart graphics pipeline 
        /// </summary>
        /// <param name="width">Width of screen</param>
        /// <param name="height">Height of Screen</param>
        public void restartGraphicsPipeline(ushort width, ushort height)
        {
            // create RDPGFX_RESET_GRAPHICS_PDU with only one monitor
            this.rdpegfxAdapter.ResetGraphics(width, height);
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = new RDPGFX_RECT16(0, 0, width, height);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            // Send solid fill request to client to fill surface with green color
            RDPGFX_RECT16 fillSurfRect = new RDPGFX_RECT16(0, 0, width, height);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, new RDPGFX_COLOR32(Color.Green.R, Color.Green.G, Color.Green.B, Color.Green.A), fillRects);
            // Expect the client to send a frame acknowledge pdu
            // If the server receives the message, it indicates that the client has been successfully decoded the logical frame of graphics commands
            this.rdpegfxAdapter.ExpectFrameAck(fid);
            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
        }

        #endregion IRdpedispAdapter Methods
    }
}
