﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using System.Drawing;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedisp;

namespace Microsoft.Protocols.TestSuites.Rdpedisp
{
    public partial class RdpedispTestSuite : RdpTestClassBase
    {
        
        [TestMethod]
        [Priority(1)]
        [TestCategory("BVT")]
        [TestCategory("Interactive")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEDISP")]
        [Description("This test is used to test addition and removal of monitor and notification with surface management commands")]
        public void S3_RDPEDISP_MonitorAdditionRemoval_RestartGraphicsPipeline()
        {
            #region Test Steps
            // 1. Establish the RDP connection between server and client.
            // 2. Open a Display Control dynamic virtual channel named "Microsoft::Windows::RDS::DisplayControl".
            // 3. Test suite should send a DISPLAYCONTROL_CAPS_PDU to client.
            // 4. Trigger client to change number of monitors by adding a monitor and maximize the window of RDP session.
            // 5. Expect DISPLAYCONTROL_MONITOR_LAYOUT_PDU from client and verify this message:
            // 6. If the above requirements are all satisfied, test suite should send surface management commands to restart the graphics pipeline.
            // 7. Expect client to change number or monitors for the remote desktop session.
            // 8. Trigger client to change number of monitors by removing a monitor and maximize the window of RDP session.
            // 9. Repeat Step 5 to 7.
            #endregion

            #region Test Code

            RDPConnect(NotificationType.SurfaceManagementCommand);

            MonitorAddition();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send Surface management commands");
            this.rdpedispAdapter.restartGraphicsPipeline(originalDesktopWidth, originalDesktopHeight);

            MonitorRemoval();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send Surface management commands");
            this.rdpedispAdapter.restartGraphicsPipeline(originalDesktopWidth, originalDesktopHeight);
            #endregion
        }

        [TestMethod]
        [Priority(1)]
        [TestCategory("BVT")]
        [TestCategory("Interactive")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEDISP")]
        [Description("This test is used to test addition and removal of monitor and notification with Deactivation-Reactivation Sequence")]
        public void S3_RDPEDISP_MonitorAdditionRemoval_DeactivationReactivation()
        {
            #region Test Steps
            // 1. Establish the RDP connection between server and client.
            // 2. Open a Display Control dynamic virtual channel named "Microsoft::Windows::RDS::DisplayControl".
            // 3. Test suite should send a DISPLAYCONTROL_CAPS_PDU to client.
            // 4. Trigger client to change number of monitors by adding a monitor and maximize the window of RDP session.
            // 5. Expect DISPLAYCONTROL_MONITOR_LAYOUT_PDU from client and verify this message:
            // 6. If the above requirements are all satisfied, test suite should initiate a Deactivation-Reactivation Sequence.
            // 7. Expect client to change number or monitors for the remote desktop session.
            // 8. Trigger client to change number of monitors by removing a monitor and maximize the window of RDP session.
            // 9. Repeat Step 5 to 7.
            #endregion

            #region Test Code

            RDPConnect(NotificationType.DeactivationReactivation);

            MonitorAddition();

            MonitorRemoval();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Initialize Deactivation-Reactivation Sequence");
            this.rdpedispAdapter.initiateDeactivationReactivation(originalDesktopWidth, originalDesktopHeight);
            Image testImage = LoadImage();
            this.Site.Assume.AreNotEqual<Image>(null, testImage, "Cannot load the test image");
            this.rdpedispAdapter.RdprfxSendImage(testImage, originalDesktopWidth, originalDesktopHeight);
            #endregion
        }

       

        /// <summary>
        /// Common test body Monitor Addition
        /// </summary>
        private void MonitorAddition()
        {

            // interactive adapter
            // ToDo: automatic method
            int result = this.rdpedispSutControlAdapter.TriggerMonitorAdditionRemovalOnClient("Add a monitor and \"Extend these displays\"");
            this.TestSite.Assert.IsTrue(result >= 0, "Test case fails due to fail operation.");

            // Add a monitor
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect Display Monitor Layout PDU to add monitors");
            DISPLAYCONTROL_MONITOR_LAYOUT_PDU monitorLayoutPDU = this.rdpedispAdapter.expectMonitorLayoutPDU();
            this.Site.Assert.AreEqual<uint>(40, monitorLayoutPDU.MonitorLayoutSize, "This field MUST be set to 40 bytes, the size of the DISPLAYCONTROL_MONITOR_LAYOUT structure (MS-RDPEDISP section 2.2.2.2.1).");
            this.Site.Assert.AreEqual<uint>(2, monitorLayoutPDU.NumMonitors, "One monitor should be added");
            DISPLAYCONTROL_MONITOR_LAYOUT[] monitors = monitorLayoutPDU.Monitors;
            foreach (var monitor in monitors)
            {
                this.TestSite.Assert.IsTrue(monitor.Width >= 200 && monitor.Width <= 8192, "The width MUST be greater than or equal to 200 pixels and less than or equal to 8192 pixels (MS-RDPEDISP section 2.2.2.2.1)");
                this.TestSite.Assert.IsTrue(monitor.Width % 2 == 0, "The width MUST NOT be an odd value");
                this.TestSite.Assert.IsTrue(monitor.Height >= 200 && monitor.Height <= 8192, "The height MUST be greater than or equal to 200 pixels and less than or equal to 8192 pixels (MS-RDPEDISP section 2.2.2.2.1)");
            }
            // To verify none of the specified monitors overlap
            bool overlap = VerifyMonitorsOverlap(monitors);
            this.TestSite.Assert.IsFalse(overlap, "Condition none of the specified monitors overlap is required. (MS-RDPEDISP section 3.1.5.2)");
            // To verify each monitor is adjacent to at least one other monitor (even if only at a single point)
            bool adjacent = VerifyMonitorsAdjacent(monitors);
            this.TestSite.Assert.IsTrue(adjacent, "Condition each monitor is adjacent to at least one other monitor (even if only at a single point) is required. (MS-RDPEDISP section 3.1.5.2)");
        }

        /// <summary>
        /// Common test body Monitor Removal
        /// </summary>
        private void MonitorRemoval()
        {
            // interactive adapter
            // ToDo: automatic method
            int result = this.rdpedispSutControlAdapter.TriggerMonitorAdditionRemovalOnClient("Remove a monitor");
            this.TestSite.Assert.IsTrue(result >= 0, "Test case fails due to fail operation.");

            // Remove a monitor
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect Display Monitor Layout PDU to add monitors");
            DISPLAYCONTROL_MONITOR_LAYOUT_PDU monitorLayoutPDU = this.rdpedispAdapter.expectMonitorLayoutPDU();
            this.Site.Assert.AreEqual<uint>(40, monitorLayoutPDU.MonitorLayoutSize, "This field MUST be set to 40 bytes, the size of the DISPLAYCONTROL_MONITOR_LAYOUT structure (MS-RDPEDISP section 2.2.2.2.1).");
            this.Site.Assert.AreEqual<uint>(1, monitorLayoutPDU.NumMonitors, "another monitor should be removed");
            DISPLAYCONTROL_MONITOR_LAYOUT primaryMonitor = monitorLayoutPDU.Monitors[0];
            this.Site.Assert.AreEqual<MonitorLayout_FlagValues>(MonitorLayout_FlagValues.DISPLAYCONTROL_MONITOR_PRIMARY, primaryMonitor.Flags, "Client should set this monitor to primary monitor");
            this.Site.Assert.AreEqual<int>(0, primaryMonitor.Left, "The left point of the only one monitor should be 0");
            this.Site.Assert.AreEqual<int>(0, primaryMonitor.Top, "The Top point of the only one monitor should be 0");
            this.TestSite.Assert.IsTrue(primaryMonitor.Width >= 200 && primaryMonitor.Width <= 8192, "The width MUST be greater than or equal to 200 pixels and less than or equal to 8192 pixels (MS-RDPEDISP section 2.2.2.2.1)");
            this.TestSite.Assert.IsTrue(primaryMonitor.Width % 2 == 0, "The width MUST NOT be an odd value");
            this.TestSite.Assert.IsTrue(primaryMonitor.Height >= 200 && primaryMonitor.Height <= 8192, "The height MUST be greater than or equal to 200 pixels and less than or equal to 8192 pixels (MS-RDPEDISP section 2.2.2.2.1)");
        }
    }
}
