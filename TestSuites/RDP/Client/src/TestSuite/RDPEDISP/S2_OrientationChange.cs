// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedisp;

namespace Microsoft.Protocols.TestSuites.Rdpedisp
{
    public partial class RdpedispTestSuite : RdpTestClassBase
    {
        
        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Interactive")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEDISP")]
        [Description("This test is used to test orientation change (Portrait) and notification with surface management commands")]
        public void S2_RDPEDISP_OrientationChange_RestartGraphicsPipeline()
        {
            #region Test Steps
            // 1. Establish the RDP connection between server and client.
            // 2. Open a Display Control dynamic virtual channel named "Microsoft::Windows::RDS::DisplayControl".
            // 3. Test suite should send a DISPLAYCONTROL_CAPS_PDU to client.
            // 4. Trigger client to change screen orientation from Landscape to Portrait and maximize the window of RDP session.
            // 5. Expect DISPLAYCONTROL_MONITOR_LAYOUT_PDU from client and verify this message.
            // 6. If the above requirements are all satisfied, test suite should send surface management commands to restart the graphics pipeline.
            // 7. Expect client to change screen orientation of the remote desktop session.
            #endregion

            #region Test Code

            RDPConnect(NotificationType.SurfaceManagementCommand);

            ChangeDesktopOrientation(this.TestContext.TestName, MonitorLayout_OrientationValues.ORIENTATION_PORTRAIT);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send Surface management commands");
            this.rdpedispAdapter.restartGraphicsPipeline(originalDesktopHeight, originalDesktopWidth);

            // wait time to display the result and then restore the default configuration
            System.Threading.Thread.Sleep(1000);
            this.InitializeDisplaySetting();
            #endregion
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Interactive")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEDISP")]
        [Description("This test is used to test orientation change (Portrait) and notification with Deactivation-Reactivation Sequence")]
        public void S2_RDPEDISP_OrientationChange_DeactivationReactivation()
        {
            #region Test Steps
            // 1. Establish the RDP connection between server and client.
            // 2. Open a Display Control dynamic virtual channel named "Microsoft::Windows::RDS::DisplayControl".
            // 3. Test suite should send a DISPLAYCONTROL_CAPS_PDU to client.
            // 4. Trigger client to change screen orientation from Landscape to Portrait and maximize the window of RDP session.
            // 5. Expect DISPLAYCONTROL_MONITOR_LAYOUT_PDU from client and verify this message.
            // 6. If the above requirements are all satisfied, test suite should initiate a Deactivation-Reactivation Sequence.
            // 7. Expect client to change screen orientation of the remote desktop session.
            #endregion

            #region Test Code

            RDPConnect(NotificationType.DeactivationReactivation);

            ChangeDesktopOrientation(this.TestContext.TestName, MonitorLayout_OrientationValues.ORIENTATION_PORTRAIT);
                        
            this.TestSite.Log.Add(LogEntryKind.Comment, "Initialize Deactivation-Reactivation Sequence");
            this.rdpedispAdapter.initiateDeactivationReactivation(originalDesktopHeight, originalDesktopWidth);
            Bitmap testImage = LoadImage();
            this.Site.Assume.AreNotEqual<Image>(null, testImage, "Cannot load the test image");
            this.rdpedispAdapter.RdprfxSendImage(testImage, originalDesktopHeight, originalDesktopWidth);

            // wait time to display the result and then restore the default configuration
            System.Threading.Thread.Sleep(1000);
            this.InitializeDisplaySetting();
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Interactive")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEDISP")]
        [Description("This test is used to test orientation changes and notification with surface management commands")]
        public void S2_RDPEDISP_OrientationAllChange_RestartGraphicsPipeline()
        {
            #region Test Steps
            // 1. Establish the RDP connection between server and client.
            // 2. Open a Display Control dynamic virtual channel named "Microsoft::Windows::RDS::DisplayControl".
            // 3. Test suite should send a DISPLAYCONTROL_CAPS_PDU to client.
            // 4. Trigger client to change screen orientation from Landscape to Portrait and maximize the window of RDP session.
            // 5. Expect DISPLAYCONTROL_MONITOR_LAYOUT_PDU from client and verify this message.
            // 6. If the above requirements are all satisfied, test suite should send surface management commands to restart the graphics pipeline.
            // 7. Expect client to change screen orientation of the remote desktop session.
            // 8. Trigger client to change screen orientation from Landscape (flipped) to Portrait (flipped) and maximize the window of RDP session.
            // 9. Repeat Step 5 to 7.
            // 10. Trigger client to change screen orientation from Portrait (flipped) to Landscape to and maximize the window of RDP session.
            // 11. Repeat Step 5 to 7.
            #endregion

            #region Test Code

            RDPConnect(NotificationType.SurfaceManagementCommand);

            ChangeDesktopOrientation(this.TestContext.TestName, MonitorLayout_OrientationValues.ORIENTATION_PORTRAIT);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send Surface management commands");
            this.rdpedispAdapter.restartGraphicsPipeline(originalDesktopHeight, originalDesktopWidth);

            System.Threading.Thread.Sleep(1000);

            ChangeDesktopOrientation(this.TestContext.TestName, MonitorLayout_OrientationValues.ORIENTATION_LANDSCAPE_FLIPPED);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send Surface management commands");
            this.rdpedispAdapter.restartGraphicsPipeline(originalDesktopWidth, originalDesktopHeight);

            System.Threading.Thread.Sleep(1000);

            ChangeDesktopOrientation(this.TestContext.TestName, MonitorLayout_OrientationValues.ORIENTATION_PORTRAIT_FLIPPED);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send Surface management commands");
            this.rdpedispAdapter.restartGraphicsPipeline(originalDesktopHeight, originalDesktopWidth);

            // wait time to display the result and then restore the default configuration
            System.Threading.Thread.Sleep(1000);
            this.InitializeDisplaySetting();
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Interactive")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEDISP")]
        [Description("This test is used to test orientation changes and notification with Deactivation-Reactivation Sequence")]
        public void S2_RDPEDISP_OrientationAllChange_DeactivationReactivation()
        {
            #region Test Steps
            // 1. Establish the RDP connection between server and client.
            // 2. Open a Display Control dynamic virtual channel named "Microsoft::Windows::RDS::DisplayControl".
            // 3. Test suite should send a DISPLAYCONTROL_CAPS_PDU to client.
            // 4. Trigger client to change screen orientation from Landscape to Portrait and maximize the window of RDP session.
            // 5. Expect DISPLAYCONTROL_MONITOR_LAYOUT_PDU from client and verify this message.
            // 6. If the above requirements are all satisfied, test suite should initiate a Deactivation-Reactivation Sequence.
            // 7. Expect client to change screen orientation of the remote desktop session.
            // 8. Trigger client to change screen orientation from Landscape (flipped) to Portrait (flipped) and maximize the window of RDP session.
            // 9. Repeat Step 5 to 7.
            // 10. Trigger client to change screen orientation from Portrait (flipped) to Landscape to and maximize the window of RDP session.
            // 11. Repeat Step 5 to 7.
            #endregion

            #region Test Code

            RDPConnect(NotificationType.DeactivationReactivation);

            ChangeDesktopOrientation(this.TestContext.TestName, MonitorLayout_OrientationValues.ORIENTATION_PORTRAIT);                       
            
            this.TestSite.Log.Add(LogEntryKind.Comment, "Initialize Deactivation-Reactivation Sequence");
            this.rdpedispAdapter.initiateDeactivationReactivation(originalDesktopHeight, originalDesktopWidth);
            Bitmap testImage = LoadImage();
            this.Site.Assume.AreNotEqual<Image>(null, testImage, "Cannot load the test image");
            this.rdpedispAdapter.RdprfxSendImage(testImage, originalDesktopHeight, originalDesktopWidth);

            rdpedycServer.Dispose();
            this.sutControlAdapter.TriggerClientDisconnect(this.TestContext.TestName);

            RDPConnect(NotificationType.DeactivationReactivation, false);

            ChangeDesktopOrientation(this.TestContext.TestName, MonitorLayout_OrientationValues.ORIENTATION_LANDSCAPE_FLIPPED);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Initialize Deactivation-Reactivation Sequence");
            this.rdpedispAdapter.initiateDeactivationReactivation(originalDesktopWidth, originalDesktopHeight);
            this.rdpedispAdapter.RdprfxSendImage(testImage, originalDesktopWidth, originalDesktopHeight);

            rdpedycServer.Dispose();
            this.sutControlAdapter.TriggerClientDisconnect(this.TestContext.TestName);

            // TODO: Solve the conflict between rdpbcgr and rdpedyc BUG #6736 and merge two cases
            RDPConnect(NotificationType.DeactivationReactivation, false);

            ChangeDesktopOrientation(this.TestContext.TestName, MonitorLayout_OrientationValues.ORIENTATION_PORTRAIT_FLIPPED);                       

            this.TestSite.Log.Add(LogEntryKind.Comment, "Initialize Deactivation-Reactivation Sequence");
            this.rdpedispAdapter.initiateDeactivationReactivation(originalDesktopHeight, originalDesktopWidth);
            this.Site.Assume.AreNotEqual<Image>(null, testImage, "Cannot load the test image");
            this.rdpedispAdapter.RdprfxSendImage(testImage, originalDesktopHeight, originalDesktopWidth);

            // wait time to display the result and then restore the default configuration
            System.Threading.Thread.Sleep(1000);
            this.InitializeDisplaySetting();
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("Interactive")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEDISP")]
        [Description("This test is used to test orientation changes and notification with surface management commands")]
        public void S2_RDPEDISP_OrientationAllChange_EnhancedAdapterDemo()
        {
            #region Test Steps
            // 1. Establish the RDP connection between server and client.
            // 2. Open a Display Control dynamic virtual channel named "Microsoft::Windows::RDS::DisplayControl".
            // 3. Test suite should send a DISPLAYCONTROL_CAPS_PDU to client.
            // 4. Trigger client to change screen orientation from Landscape to Portrait and maximize the window of RDP session.
            // 5. Expect DISPLAYCONTROL_MONITOR_LAYOUT_PDU from client and verify this message.
            // 6. If the above requirements are all satisfied, test suite should send surface management commands to restart the graphics pipeline.
            // 7. Expect client to change screen orientation of the remote desktop session.
            // 8. Trigger client to change screen orientation from Landscape (flipped) to Portrait (flipped) and maximize the window of RDP session.
            // 9. Repeat Step 5 to 7.
            // 10. Trigger client to change screen orientation from Portrait (flipped) to Landscape to and maximize the window of RDP session.
            // 11. Repeat Step 5 to 7.
            #endregion

            #region Test Code
            // For Surface RT you should use the following as screen width
            // ushort screenWidth = (ushort)(originalDesktopWidth - 2);
            ushort screenWidth = originalDesktopWidth;
            ushort screenHeight = originalDesktopHeight;

            RDPConnect(NotificationType.SurfaceManagementCommand, false);

            Bitmap instructionBitmap = new Bitmap(Site.Properties["RdpedispOrientationChange1Image"]);
            SendInstruction(screenWidth, screenHeight, instructionBitmap);

            ChangeDesktopOrientation(this.TestContext.TestName, MonitorLayout_OrientationValues.ORIENTATION_PORTRAIT, true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send Surface management commands");
            this.rdpegfxAdapter.ResetGraphics(originalDesktopHeight, originalDesktopWidth);

            System.Threading.Thread.Sleep(1000);

            instructionBitmap = new Bitmap(Site.Properties["RdpedispOrientationChange2Image"]);
            SendInstruction(screenHeight, screenWidth, instructionBitmap);

            ChangeDesktopOrientation(this.TestContext.TestName, MonitorLayout_OrientationValues.ORIENTATION_LANDSCAPE_FLIPPED, true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send Surface management commands");
            this.rdpegfxAdapter.ResetGraphics(originalDesktopWidth, originalDesktopHeight);

            System.Threading.Thread.Sleep(1000);

            instructionBitmap = new Bitmap(Site.Properties["RdpedispOrientationChange3Image"]);
            SendInstruction(screenWidth, screenHeight, instructionBitmap);

            ChangeDesktopOrientation(this.TestContext.TestName, MonitorLayout_OrientationValues.ORIENTATION_PORTRAIT_FLIPPED, true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send Surface management commands");
            this.rdpegfxAdapter.ResetGraphics(originalDesktopHeight, originalDesktopWidth);

            // wait time to display the result and then restore the default configuration
            System.Threading.Thread.Sleep(1000);

            instructionBitmap = new Bitmap(Site.Properties["RdpedispOrientationChange4Image"]);
            SendInstruction(screenHeight, screenWidth, instructionBitmap);

            ChangeDesktopOrientation(this.TestContext.TestName, MonitorLayout_OrientationValues.ORIENTATION_LANDSCAPE, true);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send Surface management commands");
            this.rdpegfxAdapter.ResetGraphics(originalDesktopWidth, originalDesktopHeight);

            instructionBitmap = new Bitmap(Site.Properties["RdpedispEndImage"]);
            SendInstruction(screenWidth, screenHeight, instructionBitmap);

            // wait time to display the result and then restore the default configuration
            System.Threading.Thread.Sleep(1000);
            #endregion
        }

        /// <summary>
        /// Common test body
        /// </summary>
        private void ChangeDesktopOrientation(string caseName, MonitorLayout_OrientationValues orientation, bool enhancedAdapter = false)
        {
            if (!enhancedAdapter)
            {
                int result = this.rdpedispSutControlAdapter.TriggerOrientationChangeOnClient(caseName, OrientationToDEVMODEValue(orientation));
                if (result >= 0)
                {
                    // Wait for screen orientation change
                    System.Threading.Thread.Sleep(1000);
                    result = this.rdpedispSutControlAdapter.TriggerMaximizeRDPClientWindow(caseName);
                    // Wait for maximize RDP client Window
                    System.Threading.Thread.Sleep(4000);
                }
                this.TestSite.Assert.IsTrue(result >= 0, "Test case fails due to fail operation.");
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect Display Monitor Layout PDU to change orientation");
            DISPLAYCONTROL_MONITOR_LAYOUT_PDU monitorLayoutPDU = this.rdpedispAdapter.expectMonitorLayoutPDU();

            this.Site.Assert.AreEqual<uint>(40, monitorLayoutPDU.MonitorLayoutSize, "This field MUST be set to 40 bytes, the size of the DISPLAYCONTROL_MONITOR_LAYOUT structure (MS-RDPEDISP section 2.2.2.2.1).");
            this.Site.Assert.AreEqual<uint>(1, monitorLayoutPDU.NumMonitors, "Only one Monitor for this case");
            DISPLAYCONTROL_MONITOR_LAYOUT monitor = monitorLayoutPDU.Monitors[0];
            this.Site.Assert.AreEqual<MonitorLayout_FlagValues>(MonitorLayout_FlagValues.DISPLAYCONTROL_MONITOR_PRIMARY, monitor.Flags, "Client should set this monitor to primary monitor");
            this.Site.Assert.AreEqual<int>(0, monitor.Left, "The left point of the only one monitor should be 0");
            this.Site.Assert.AreEqual<int>(0, monitor.Top, "The Top point of the only one monitor should be 0");
            if (orientation == MonitorLayout_OrientationValues.ORIENTATION_PORTRAIT || orientation == MonitorLayout_OrientationValues.ORIENTATION_PORTRAIT_FLIPPED)
            {
                this.Site.Assert.AreEqual<uint>(originalDesktopHeight, monitor.Width, "The width of monitor MUST be {0}", originalDesktopHeight);
                this.Site.Assert.AreEqual<uint>(originalDesktopWidth, monitor.Height, "The height of monitor MUST be {0}", originalDesktopWidth);
            }
            else
            {
                this.Site.Assert.AreEqual<uint>(originalDesktopWidth, monitor.Width, "The width of monitor MUST be {0}", originalDesktopWidth);
                this.Site.Assert.AreEqual<uint>(originalDesktopHeight, monitor.Height, "The height of monitor MUST be {0}", originalDesktopHeight);
            }
            this.Site.Assert.AreEqual<MonitorLayout_OrientationValues>(orientation, monitor.Orientation, "The Orientation of monitor MUST be {0}", orientation);

            // Clear receive buffer
            this.rdpedispAdapter.ClearReceivedList();
        }
    }
}
