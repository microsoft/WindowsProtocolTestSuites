// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedisp;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestTools;
using System.Drawing;

namespace Microsoft.Protocols.TestSuites.Rdpedisp
{
    public partial class RdpedispTestSuite : RdpTestClassBase
    {

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEDISP")]   
        [Description("This test is used to test resolution change and notification with surface management commands")]
        public void S1_RDPEDISP_ResolutionChange_RestartGraphicsPipeline()
        {
            #region Test Steps
            // 1. Establish the RDP connection between server and client.
            // 2. Open a Display Control dynamic virtual channel named "Microsoft::Windows::RDS::DisplayControl".
            // 3. Test suite should send a DISPLAYCONTROL_CAPS_PDU to client.
            // 4. Trigger SUT to change screen resolution and maximize the window of RDP session.
            // 5. Expect DISPLAYCONTROL_MONITOR_LAYOUT_PDU from client and verify this message.
            // 6. If the above requirements are all satisfied, test suite should send surface management commands to restart the graphics pipeline.
            // 7. Expect client to change screen resolution of the remote desktop session.
            #endregion

            #region Test Code
            RDPConnect(NotificationType.SurfaceManagementCommand);

            ChangeDesktopResolution(this.TestContext.TestName);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send Surface management commands");
            this.rdpedispAdapter.restartGraphicsPipeline(changedDesktopWidth, changedDesktopHeight);

            // wait time to display the result and then restore the default configuration
            System.Threading.Thread.Sleep(1000);
            this.InitializeDisplaySetting();
            #endregion
        }

        [TestMethod]
        [Priority(0)]
        [TestCategory("BVT")]
        [TestCategory("Positive")]
        [TestCategory("RDP8.1")]
        [TestCategory("RDPEDISP")]
        [Description("This test is used to test resolution change and notification with Deactivation-Reactivation Sequence")]
        public void S1_RDPEDISP_ResolutionChange_DeactivationReactivation()
        {
            #region Test Steps
            // 1. Establish the RDP connection between server and client.
            // 2. Open a Display Control dynamic virtual channel named "Microsoft::Windows::RDS::DisplayControl".
            // 3. Test suite should send a DISPLAYCONTROL_CAPS_PDU to client.
            // 4. Trigger SUT to change screen resolution and maximize the window of RDP session.
            // 5. Expect DISPLAYCONTROL_MONITOR_LAYOUT_PDU from client and verify this message.
            // 6. If the above requirements are all satisfied, test suite should initiate a Deactivation-Reactivation Sequence.
            // 7. Expect client to change screen resolution of the remote desktop session.
            #endregion

            #region Test Code
            RDPConnect(NotificationType.DeactivationReactivation);

            ChangeDesktopResolution(this.TestContext.TestName);
                        
            this.TestSite.Log.Add(LogEntryKind.Comment, "Initialize Deactivation-Reactivation Sequence");
            this.rdpedispAdapter.initiateDeactivationReactivation(changedDesktopWidth, changedDesktopHeight);
            Image testImage = LoadImage();
            this.Site.Assume.AreNotEqual<Image>(null, testImage, "Cannot load the test image");
            this.rdpedispAdapter.RdprfxSendImage(testImage, changedDesktopWidth, changedDesktopHeight);

            // wait time to display the result and then restore the default configuration
            System.Threading.Thread.Sleep(1000);
            this.InitializeDisplaySetting();
            #endregion
        }

        /// <summary>
        /// Common test body
        /// </summary>
        private void ChangeDesktopResolution(string caseName)
        {
            int result = this.rdpedispSutControlAdapter.TriggerResolutionChangeOnClient(caseName, changedDesktopWidth, changedDesktopHeight);
            System.Threading.Thread.Sleep(SUTAdapterWaitTime);
            this.TestSite.Assert.IsTrue(result >= 0, "Test case fails due to fail operation.");

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expect Display Monitor Layout PDU to change resolution");
            DISPLAYCONTROL_MONITOR_LAYOUT_PDU monitorLayoutPDU = this.rdpedispAdapter.expectMonitorLayoutPDU();
            this.Site.Assert.IsNotNull(monitorLayoutPDU, "Client should send a DISPLAYCONTROL_MONITOR_LAYOUT PDU to change resolution.");
            this.Site.Assert.AreEqual<uint>(40, monitorLayoutPDU.MonitorLayoutSize, "This field MUST be set to 40 bytes, the size of the DISPLAYCONTROL_MONITOR_LAYOUT structure (MS-RDPEDISP section 2.2.2.2.1).");
            this.Site.Assert.AreEqual<uint>(1, monitorLayoutPDU.NumMonitors, "Only one Monitor for this case");
            DISPLAYCONTROL_MONITOR_LAYOUT monitor = monitorLayoutPDU.Monitors[0];
            this.Site.Assert.AreEqual<MonitorLayout_FlagValues>(MonitorLayout_FlagValues.DISPLAYCONTROL_MONITOR_PRIMARY, monitor.Flags, "Client should set this monitor to primary monitor");
            this.Site.Assert.AreEqual<int>(0, monitor.Left, "The left point of the only one monitor should be 0");
            this.Site.Assert.AreEqual<int>(0, monitor.Top, "The Top point of the only one monitor should be 0");
            this.Site.Assert.AreEqual<uint>(changedDesktopWidth, monitor.Width, "The width of monitor MUST be {0}", changedDesktopWidth);
            this.Site.Assert.AreEqual<uint>(changedDesktopHeight, monitor.Height, "The height of monitor MUST be {0}", changedDesktopHeight);
            this.Site.Assert.AreEqual<MonitorLayout_OrientationValues>(MonitorLayout_OrientationValues.ORIENTATION_LANDSCAPE, monitor.Orientation, "The height of monitor MUST be LANDSCAPE");
        }
    }
}
