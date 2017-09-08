// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdprfx;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;

namespace Microsoft.Protocols.TestSuites.Rdpei
{
    /// <summary>
    /// Utility class to support the UI interactive with Remote Touch client side.
    /// </summary>
    public static class RdpeiUtility
    {
        private static ITestSite Site;
        private static IRdpbcgrAdapter rdpbcgrAdapter;
        private static IRdprfxAdapter rdprfxAdapter;

        /// <summary>
        /// Initializes the RdpeiUtility class.
        /// </summary>
        /// <param name="tSite">Instance of ITestSite.</param>
        public static void Initialized(ITestSite tSite)
        {
            if (Site == null)
            {
                Site = tSite;
                rdpbcgrAdapter = Site.GetAdapter<IRdpbcgrAdapter>();
                rdprfxAdapter = Site.GetAdapter<IRdprfxAdapter>();
            }
        }

        /// <summary>
        /// Method to send an image to the client.
        /// </summary>
        /// <param name="image">The image to be sent.</param>
        /// <param name="destLeft">The left bound of the frame.</param>
        /// <param name="destTop">The top bound of the frame.</param>
        /// <param name="frameId">The index of the sending frame.</param>
        public static void SendImageToClient(Image image, ushort destLeft, ushort destTop, uint frameId)
        {
            OperationalMode opMode = OperationalMode.ImageMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;

            Site.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            Site.Log.Add(LogEntryKind.Comment, "Sending encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);
            rdprfxAdapter.SendImageToClient(image, opMode, enAlgorithm, destLeft, destTop);

            Site.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);
        }

        /// <summary>
        /// Send confirm message to the client after interaction.
        /// </summary>
        public static void SendConfirmImage()
        {
            ushort left = (ushort)((rdpbcgrAdapter.CapabilitySetting.DesktopWidth - 150) / 2);
            ushort top = (ushort)((rdpbcgrAdapter.CapabilitySetting.DesktopHeight - 100) / 2 + 150);

            Bitmap img = new Bitmap(150, 100);
            Graphics graph = Graphics.FromImage(img);
            graph.DrawString("Great!", new Font("Arial", 35), new SolidBrush(Color.White), 0, 0, new StringFormat());

            SendImageToClient(img, left, top, 0);
        }

        /// <summary>
        /// Send instruction message to the client to trigger interaction.
        /// </summary>
        /// <param name="config">The configuration of the instructions to be sent.</param>
        public static void SendInstruction(RdpeiSUTControlConfig config)
        {
            Bitmap img = new Bitmap(config.width, config.height);
            Graphics graph = Graphics.FromImage(img);
            if (config.instructions != null)
            {
                foreach (RdpeiSUTControlInstruction i in config.instructions)
                {
                    graph.DrawString(i.text, i.font, new SolidBrush(Color.White), i.left, i.top, new StringFormat());
                }
            }
            ushort left = (ushort)((rdpbcgrAdapter.CapabilitySetting.DesktopWidth - config.width) / 2);
            ushort top = (ushort)((rdpbcgrAdapter.CapabilitySetting.DesktopHeight - config.height) / 2);
            SendImageToClient(img, left, top, 0);
        }

        /// <summary>
        /// Add a button on the client screen.
        /// </summary>
        /// <param name="text">The text of the button.</param>
        /// <param name="left">The left position of the button.</param>
        /// <param name="top">The top position of the button.</param>
        public static void AddButton(string text, ushort left, ushort top)
        {
            Bitmap img = new Bitmap(text.Length * 25, 60);
            Graphics graph = Graphics.FromImage(img);
            graph.FillRectangle(new SolidBrush(Color.Gray), 0, 0, text.Length * 25, 60);
            graph.DrawString(text, new Font("Verdana", 32), new SolidBrush(Color.Black), 0, 0, new StringFormat());
            SendImageToClient(img, left, top, 1);
        }

        /// <summary>
        /// Send a circle to the client.
        /// </summary>
        /// <param name="diam">The diameter of the circle.</param>
        /// <param name="color">The color of the circle.</param>
        /// <param name="left">The left position of the circle.</param>
        /// <param name="top">The top position of the circle.</param>
        public static void SendCircle(ushort diam, Color color, ushort left, ushort top)
        {
            Bitmap img = new Bitmap(diam, diam);
            Graphics graph = Graphics.FromImage(img);
            graph.FillEllipse(new SolidBrush(color), 0, 0, diam, diam);
            RdpeiUtility.SendImageToClient(img, left, top, 0);
        }
    }
}