// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdprfx;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;
using SkiaSharp;

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

        public static SKPaint Paint_Arial_35 = new SKPaint { Typeface = SKTypeface.FromFamilyName("Arial"), TextSize = 35 * 96 / 72 };
        public static SKPaint Paint_Arial_28 = new SKPaint { Typeface = SKTypeface.FromFamilyName("Arial"), TextSize = 28 * 96 / 72 };
        public static SKPaint Paint_Verdana_24 = new SKPaint { Typeface = SKTypeface.FromFamilyName("Verdana"), TextSize = 24 * 96 / 72 };
        public static SKPaint Paint_Verdana_20 = new SKPaint { Typeface = SKTypeface.FromFamilyName("Verdana"), TextSize = 20 * 96 / 72 };

        /// <summary>
        /// Initializes the RdpeiUtility class.
        /// </summary>
        /// <param name="tSite">Instance of ITestSite.</param>
        public static void Initialize(ITestSite tSite)
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
        public static void SendImageToClient(SKImage image, ushort destLeft, ushort destTop, uint frameId)
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

            SKBitmap img = new SKBitmap(150, 100);
            SKCanvas canvas = new SKCanvas(img);
            SKPaint paint = Paint_Arial_35;
            paint.Color = SKColors.White;
            canvas.DrawText("Great!", 0, paint.TextSize, paint);

            SendImageToClient(SKImage.FromBitmap(img), left, top, 0);
        }

        /// <summary>
        /// Send instruction message to the client to trigger interaction.
        /// </summary>
        /// <param name="config">The configuration of the instructions to be sent.</param>
        public static void SendInstruction(RdpeiSUTControlConfig config)
        {
            SKBitmap img = new SKBitmap(config.width, config.height);
            SKCanvas canvas = new SKCanvas(img);
            if (config.instructions != null)
            {
                foreach (RdpeiSUTControlInstruction i in config.instructions)
                {
                    i.paint.Color = SKColors.White;
                    canvas.DrawText(i.text, i.left, i.top + i.paint.TextSize, i.paint);
                }
            }
            ushort left = (ushort)((rdpbcgrAdapter.CapabilitySetting.DesktopWidth - config.width) / 2);
            ushort top = (ushort)((rdpbcgrAdapter.CapabilitySetting.DesktopHeight - config.height) / 2);
            SendImageToClient(SKImage.FromBitmap(img), left, top, 0);
        }

        /// <summary>
        /// Add a button on the client screen.
        /// </summary>
        /// <param name="text">The text of the button.</param>
        /// <param name="left">The left position of the button.</param>
        /// <param name="top">The top position of the button.</param>
        public static void AddButton(string text, ushort left, ushort top)
        {
            SKBitmap img = new SKBitmap(text.Length * 25, 60);
            SKCanvas canvas = new SKCanvas(img);
            SKPaint paint = new SKPaint() { Color = SKColors.Gray };
            canvas.DrawRect(new SKRect(0, 0, text.Length * 25, 60), paint);
            SKPaint paint2 = new SKPaint() { Color = SKColors.Black };
            paint2.Typeface = SKTypeface.FromFamilyName("Verdana");
            paint2.TextSize = 32 * 96 / 72;
            canvas.DrawText(text, 0, paint2.TextSize, paint2);
            SendImageToClient(SKImage.FromBitmap(img), left, top, 1);
        }

        /// <summary>
        /// Send a circle to the client.
        /// </summary>
        /// <param name="diam">The diameter of the circle.</param>
        /// <param name="color">The color of the circle.</param>
        /// <param name="left">The left position of the circle.</param>
        /// <param name="top">The top position of the circle.</param>
        public static void SendCircle(ushort diam, SKColor color, ushort left, ushort top)
        {
            SKBitmap img = new SKBitmap(diam, diam);
            SKCanvas canvas = new SKCanvas(img);
            canvas.DrawOval(diam / 2, diam / 2, diam / 2, diam / 2, new SKPaint { Color = color });
            RdpeiUtility.SendImageToClient(SKImage.FromBitmap(img), left, top, 0);
        }
    }
}