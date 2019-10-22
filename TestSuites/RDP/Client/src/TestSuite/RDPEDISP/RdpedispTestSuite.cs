// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdpegfx;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedisp;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx;
using Microsoft.Protocols.TestSuites.Rdprfx;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Protocols.TestSuites.Rdpedisp
{
    [TestClass]
    public partial class RdpedispTestSuite : RdpTestClassBase
    {
        #region private variables
        public enum NotificationType
        {
            DeactivationReactivation,
            SurfaceManagementCommand
        }
        NotificationType notificationType = NotificationType.SurfaceManagementCommand;

        // some constants for testing
        ushort originalDesktopWidth;
        ushort originalDesktopHeight;
        ushort changedDesktopWidth;
        ushort changedDesktopHeight;
        ushort originalMonitorNumber;
        ushort changedMonitorNumber;
        const uint maxNumMonitors = 4;
        const uint maxMonitorAreaFactorA = 2560;
        const uint maxMonitorAreaFactorB = 1600;

        // Sleep time to wait for SUT Control Adapter done
        const int SUTAdapterWaitTime = 15000;

        #endregion

        #region Adapter Instances
        private RdpedycServer rdpedycServer;
        private IRdpegfxAdapter rdpegfxAdapter;
        private IRdpedispAdapter rdpedispAdapter;
        private IRdprfxAdapter rdprfxAdapter;
        private IRdpedispSUTControlAdapter rdpedispSutControlAdapter;

        #endregion

        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            RdpTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            RdpTestClassBase.BaseCleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();

            try
            {
                originalDesktopWidth = Convert.ToUInt16(Site.Properties["originalDesktopWidth"]);
                originalDesktopHeight = Convert.ToUInt16(Site.Properties["originalDesktopHeight"]);
                changedDesktopWidth = Convert.ToUInt16(Site.Properties["changedDesktopWidth"]);
                changedDesktopHeight = Convert.ToUInt16(Site.Properties["changedDesktopHeight"]);
                originalMonitorNumber = Convert.ToUInt16(Site.Properties["originalMonitorNumber"]);
                changedMonitorNumber = Convert.ToUInt16(Site.Properties["changedMonitorNumber"]);
            }
            catch (Exception)
            {
                originalDesktopWidth = 1024;
                originalDesktopHeight = 768;
                changedDesktopWidth = 1152;
                changedDesktopHeight = 864;
                originalMonitorNumber = 1;
                changedMonitorNumber = 2;
            }

            this.rdpedispAdapter = (IRdpedispAdapter)this.TestSite.GetAdapter(typeof(IRdpedispAdapter));
            this.rdpedispAdapter.Reset();
            this.rdpedispSutControlAdapter = (IRdpedispSUTControlAdapter)this.TestSite.GetAdapter(typeof(IRdpedispSUTControlAdapter));
            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening with transport protocol: {0}", transportProtocol.ToString());
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);
        }

        protected override void TestCleanup()
        {
            // TestCleanup() may be not main thread
            DynamicVCException.SetCleanUp(true);

            base.TestCleanup();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Dispose virtual channel manager.");
            if (rdpedycServer != null)
                rdpedycServer.Dispose();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger client to close all RDP connections for clean up.");
            //int iResult = this.sutControlAdapter.TriggerClientDisconnectAll();
            //this.TestSite.Log.Add(LogEntryKind.Debug, "The result of TriggerClientDisconnectAll is {0}.", iResult);
            StopRDPConnection();


            this.TestSite.Log.Add(LogEntryKind.Comment, "Stop RDP listening.");
            this.rdpbcgrAdapter.StopRDPListening();

            DynamicVCException.SetCleanUp(false);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Verify the version of operation system or MSTSC
        /// </summary>
        private void VerifyRDPVersion()
        {
            this.Site.Assert.AreEqual<String>(this.Site.Properties["RDP.Version"], "8.1", "MS-RDPEDISP only support RDP 8.1 and above.");
        }

        /// <summary>
        /// Trigger client to initialize display settings
        /// </summary>
        private void InitializeDisplaySetting()
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger client to initialize display settings ...");
            int result = this.rdpedispSutControlAdapter.TriggerInitializeDisplaySettings(this.TestContext.TestName, originalDesktopWidth, originalDesktopHeight, OrientationToDEVMODEValue(MonitorLayout_OrientationValues.ORIENTATION_LANDSCAPE));
            this.TestSite.Assert.IsTrue(result >= 0, "Cannot initialize display settings on client automatically");
            System.Threading.Thread.Sleep(SUTAdapterWaitTime);
        }

        /// <summary>
        /// Start RDP connection
        /// </summary>
        private void StartRDPConnection()
        {

            #region Trigger client to connect

            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol, true);
            #endregion

            #region RDPBCGR Connection

            // Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server RDPRFX Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            // Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            rdpedycServer = new RdpedycServer(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);
            rdpedycServer.ExchangeCapabilities(waitTime);
            if (notificationType == NotificationType.SurfaceManagementCommand)
            {
                this.rdpegfxAdapter = (IRdpegfxAdapter)this.TestSite.GetAdapter(typeof(IRdpegfxAdapter));
                this.rdpegfxAdapter.Reset();
                this.rdprfxAdapter = null;
                // RDPEGFX capability exchange
                RDPEGFX_CapabilityExchange();
                this.rdpedispAdapter.AttachRdpbcgrAdapter(this.rdpbcgrAdapter);
                this.rdpedispAdapter.AttachRdpegfxAdapter(rdpegfxAdapter);
            }
            else
            {
                this.rdprfxAdapter = (IRdprfxAdapter)this.TestSite.GetAdapter(typeof(IRdprfxAdapter));
                this.rdprfxAdapter.Reset();                
                //Initial the RDPRFX adapter context.
                rdprfxAdapter.Accept(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);
                receiveAndLogClientRfxCapabilites();
                this.rdpedispAdapter.AttachRdpbcgrAdapter(this.rdpbcgrAdapter);
                this.rdpedispAdapter.AttachRdprfxAdapter(this.rdprfxAdapter);
            }
        }

        /// <summary>
        /// Stop RDP connection
        /// </summary>
        private void StopRDPConnection()
        {
            int iResult = this.sutControlAdapter.TriggerClientDisconnectAll(this.TestContext.TestName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "The result of TriggerClientDisconnectAll is {0}.", iResult);
            this.rdpbcgrAdapter.Reset();
            if(this.rdpegfxAdapter != null)
                this.rdpegfxAdapter.Reset();
            if(this.rdprfxAdapter != null)
                this.rdprfxAdapter.Reset();
        }

        /// <summary>
        /// Trigger client to connect to server
        /// </summary>
        /// <param name="testType">NotificationType</param>
        private void RDPConnect(NotificationType testType, bool initialize = true)
        {
            // initialize display settings on client
            if (initialize) InitializeDisplaySetting();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Start a RDP connection");
            notificationType = testType;
            this.StartRDPConnection();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a dynamic virtual channel for MS-RDPEDISP");
            this.rdpedispAdapter.ProtocolInitialize(rdpedycServer);

            // send an orignal image wait for BUG 5670070 Fixed
            //Image gridsImage = LoadImage();
            //this.Site.Assume.AreNotEqual<Image>(null, gridsImage, "Cannot load the Grids image");
            //this.rdpedispAdapter.RdprfxSendImage(gridsImage, changedDesktopWidth, changedDesktopHeight);
            //System.Threading.Thread.Sleep(1000);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Send Display Control capability PDU");
            this.rdpedispAdapter.sendCapsPDU(maxNumMonitors, maxMonitorAreaFactorA, maxMonitorAreaFactorB);
        }

        /// <summary>
        /// Load Rdpedisp test image RdpedispTestImage
        /// </summary>
        /// <returns></returns>
        private Bitmap LoadImage()
        {
            // Load Rdpedisp test image
            String RdpedispTestImagePath;
            Bitmap testImage = null;
            if (!PtfPropUtility.GetStringPtfProperty(this.TestSite, "RdpedispTestImage", out RdpedispTestImagePath))
            {
                RdpedispTestImagePath = "";
                return null;
            }
            try
            {
                testImage = new Bitmap(RdpedispTestImagePath);
            }
            catch (System.IO.FileNotFoundException)
            {
                this.TestSite.Log.Add(LogEntryKind.TestError, "File {0} not find.", RdpedispTestImagePath);
            }
            return testImage;
        }

        /// <summary>
        /// Covert MonitorLayout_OrientationValues to DEVMODE orientation value
        /// </summary>
        /// <param name="orientation">MonitorLayout_OrientationValues orientation</param>
        /// <returns></returns>
        private int OrientationToDEVMODEValue(MonitorLayout_OrientationValues orientation)
        {
            switch (orientation)
            {
                case MonitorLayout_OrientationValues.ORIENTATION_LANDSCAPE:
                    return 0;
                case MonitorLayout_OrientationValues.ORIENTATION_PORTRAIT:
                    return 1;
                case MonitorLayout_OrientationValues.ORIENTATION_LANDSCAPE_FLIPPED:
                    return 2;
                case MonitorLayout_OrientationValues.ORIENTATION_PORTRAIT_FLIPPED:
                    return 3;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Covert DEVMODE orientation value to MonitorLayout_OrientationValues
        /// </summary>
        /// <param name="value">DEVMODE orientation value</param>
        /// <returns></returns>
        private MonitorLayout_OrientationValues DEVMODEValueToOrientation(int value)
        {
            switch (value)
            {
                case 0:
                    return MonitorLayout_OrientationValues.ORIENTATION_LANDSCAPE;
                case 1:
                    return MonitorLayout_OrientationValues.ORIENTATION_PORTRAIT;
                case 2:
                    return MonitorLayout_OrientationValues.ORIENTATION_LANDSCAPE_FLIPPED;
                case 3:
                    return MonitorLayout_OrientationValues.ORIENTATION_PORTRAIT_FLIPPED;
                default:
                    return MonitorLayout_OrientationValues.ORIENTATION_LANDSCAPE;
            }
        }

       
        /// <summary>
        /// Check whether two monitors are overlapped
        /// </summary>
        /// <param name="monitor1">Monitor 1</param>
        /// <param name="Monitor2">Monitor 2</param>
        /// <returns></returns>
        private bool isOverlap(DISPLAYCONTROL_MONITOR_LAYOUT monitor1, DISPLAYCONTROL_MONITOR_LAYOUT Monitor2)
        {
            bool overlap = true;
            // Monitor2 in left of Monitor1
            if (Monitor2.Left + Monitor2.Width <= monitor1.Left) overlap = false;
            // Monitor2 is above of Monitor1
            if (Monitor2.Top + Monitor2.Height <= monitor1.Top) overlap = false;
            // Monitor2 in right of Monitor1
            if (Monitor2.Left >= monitor1.Left + monitor1.Width) overlap = false;
            // Monitor2 is beneath of Monitor1
            if (Monitor2.Top >= monitor1.Top + monitor1.Height) overlap = false;
            return overlap;
        }

        /// <summary>
        /// Check whether two monitors are adjacent and not overlapped
        /// </summary>
        /// <param name="monitor1">Monitor 1</param>
        /// <param name="monitor2">Monitor 2</param>
        /// <returns></returns>
        private bool isAdjacent(DISPLAYCONTROL_MONITOR_LAYOUT monitor1, DISPLAYCONTROL_MONITOR_LAYOUT monitor2)
        {
            bool adjacent = false;
            // Monitor2 is adjacent to left of monitor1
            if (monitor2.Left + monitor2.Width == monitor1.Left
                && monitor2.Top + monitor2.Height >= monitor1.Top
                && monitor2.Top <= monitor1.Top + monitor1.Height)
                adjacent = true;
            // Monitor2 is adjacent to top of monitor1
            if (monitor2.Top + monitor2.Height == monitor1.Top
                && monitor2.Left + monitor2.Width >= monitor1.Left
                && monitor2.Left <= monitor1.Left + monitor1.Width)
                adjacent = true;
            // Monitor2 is adjacent to right of monitor1
            if (monitor2.Left == monitor1.Left + monitor1.Width
                && monitor2.Top + monitor1.Height >= monitor1.Top
                && monitor2.Top <= monitor1.Top + monitor1.Height)
                adjacent = true;
            // Monitor2 is adjacent to bottom of monitor1
            if (monitor2.Top == monitor1.Top + monitor1.Height
                && monitor2.Left + monitor2.Width >= monitor1.Left
                && monitor2.Left <= monitor1.Left + monitor1.Width)
                adjacent = true;
            return adjacent;
        }

        /// <summary>
        /// Check none of the specified monitors overlap
        /// </summary>
        /// <param name="monitors">Monitor array</param>
        /// <returns></returns>
        private bool VerifyMonitorsOverlap(DISPLAYCONTROL_MONITOR_LAYOUT[] monitors){
            bool anyOverlap = false;
            for (int i = 0; i < monitors.Length-1; i++)
            {
                for (int j = i+1; j < monitors.Length; j++)
                {
                    if (isOverlap(monitors[i], monitors[j])) anyOverlap = true;
                }
            }
            return anyOverlap;
        }

        /// <summary>
        /// Check each monitor is adjacent to at least one other monitor (even if only at a single point) 
        /// </summary>
        /// <param name="monitors">Monitor array</param>
        /// <returns></returns>
        private bool VerifyMonitorsAdjacent(DISPLAYCONTROL_MONITOR_LAYOUT[] monitors)
        {
            for (int i = 0; i < monitors.Length - 1; i++)
            {
                bool adjacent = false;
                for (int j = i + 1; j < monitors.Length; j++)
                {
                    if (isAdjacent(monitors[i], monitors[j])) adjacent = true;
                }
                if (!adjacent) return false;
            }
            return true;
        }

        private void RDPEGFX_CapabilityExchange()
        {
            this.TestSite.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEGFX ...");
            bool bProtocolSupported = this.rdpegfxAdapter.ProtocolInitialize(this.rdpedycServer);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            this.TestSite.Log.Add(LogEntryKind.Debug, "Expecting capability advertise from client.");
            RDPGFX_CAPS_ADVERTISE capsAdv = this.rdpegfxAdapter.ExpectCapabilityAdvertise();
            this.TestSite.Assert.IsNotNull(capsAdv, "RDPGFX_CAPS_ADVERTISE is received.");

            this.TestSite.Log.Add(LogEntryKind.Debug, "Sending capability confirm to client.");
            // Set first capset in capability advertise request, if no capdata in request, use default flag.
            CapsFlags capFlag = CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT;
            if (capsAdv.capsSetCount > 0)
                capFlag = (CapsFlags)BitConverter.ToUInt32(capsAdv.capsSets[0].capsData, 0);
            this.rdpegfxAdapter.SendCapabilityConfirm(capFlag);
        }

        /// <summary>
        /// Copy from RDPRFX test suite code
        /// </summary>
        private void receiveAndLogClientRfxCapabilites()
        {
            uint maxRequestSize = 38055; // MS-RDPBCGR section 2.2.7.2.7
            TS_RFX_ICAP[] clientRfxCaps;

            this.TestSite.Log.Add(LogEntryKind.Comment, "Receive and check client capabilities...");
            rdprfxAdapter.ReceiveAndCheckClientCapabilities(maxRequestSize, out clientRfxCaps);

            if (clientRfxCaps != null)
            {
                foreach (TS_RFX_ICAP iCap in clientRfxCaps)
                {
                    OperationalMode opMode = (OperationalMode)iCap.flags;
                    EntropyAlgorithm enAlg = (EntropyAlgorithm)iCap.entropyBits;
                    this.TestSite.Log.Add(LogEntryKind.Comment, "Client supports ({0}, {1}).", opMode, enAlg);
                }
            }
        }

        /// <summary>
        /// Copy from RDPRFX test suite code
        /// </summary>
        private void setServerCapabilitiesWithRemoteFxSupported()
        {
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true,
               true, maxRequestSize,
               true,
               true, 1,
               true, CmdFlags_Values.SURFCMDS_FRAMEMARKER | CmdFlags_Values.SURFCMDS_SETSURFACEBITS | CmdFlags_Values.SURFCMDS_STREAMSURFACEBITS,
               true, true, true);
        }

        private void SendInstruction(ushort screenWidth, ushort screenHeight, Bitmap bitmap)
        {
            this.TestSite.Log.Add(LogEntryKind.Comment, "Create a surface and fill it with white color.");
            // Create & output a surface 
            RDPGFX_RECT16 surfRect = RdpegfxTestUtility.ConvertToRect(new RDPGFX_POINT16(0, 0), screenWidth, screenHeight);
            Surface surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);
            this.TestSite.Assert.IsNotNull(surf, "Surface {0} is created", surf.Id);

            // Send solid fill request to client to fill surface with white color
            RDPGFX_RECT16 fillSurfRect = RdpegfxTestUtility.ConvertToRect(new RDPGFX_POINT16(0, 0), screenWidth, screenHeight);
            RDPGFX_RECT16[] fillRects = { fillSurfRect };  // Relative to surface
            uint fid = this.rdpegfxAdapter.SolidFillSurface(surf, RdpegfxTestUtility.ToRdpgfx_Color32(Color.White), fillRects);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface is filled with solid color in frame: {0}", fid);
            this.rdpegfxAdapter.ExpectFrameAck(fid);
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);

            ushort startX = (ushort) (screenWidth / 2 - bitmap.Width / 2);
            ushort startY = (ushort) (screenHeight / 2 - bitmap.Height / 2);
            surfRect = RdpegfxTestUtility.ConvertToRect(new RDPGFX_POINT16(startX, startY), (ushort)bitmap.Width, (ushort)bitmap.Height);
            surf = this.rdpegfxAdapter.CreateAndOutputSurface(surfRect, PixelFormat.PIXEL_FORMAT_XRGB_8888);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Instruction Image to client.");
            List<Dictionary<uint, byte[]>> layerDataList = this.rdpegfxAdapter.RfxProgressiveCodecEncode(surf, bitmap, PixelFormat.PIXEL_FORMAT_XRGB_8888,
                                                            false, true, ImageQuality_Values.Lossless, false, false, true);
            fid = SendRfxProgCodecEcodedData(layerDataList);
            this.rdpegfxAdapter.ExpectFrameAck(fid);

            // Delete the surface
            this.rdpegfxAdapter.DeleteSurface(surf.Id);
            this.TestSite.Log.Add(LogEntryKind.Debug, "Surface {0} is deleted", surf.Id);
        }

        /// <summary>
        /// Copy from RDPEGFX test suite
        /// Send Rfx Progressive Codec Encoded bitmap data to client
        /// </summary>
        /// <param name="layerDataList"> It indicates the encoded data in different layer </param>
        /// <param name="positiveTest"> It indicates if used in positive test case </param>
        /// <returns> It returns frame id of last sent frame </returns>
        uint SendRfxProgCodecEcodedData(List<Dictionary<uint, byte[]>> layerDataList, bool positiveTest = true)
        {
            ushort frCount = 0;

            if (layerDataList == null) return 0xffffffff; // Return invalid fid if no layer data

            // Send Rfx Progressive encoded image data 
            for (byte i = 0; i < layerDataList.Count; i++) // Loop layer by layer
            {

                Dictionary<uint, byte[]> frDict = layerDataList[i];
                foreach (KeyValuePair<uint, byte[]> pair in frDict)     // Loop frame by frame in a layer
                {
                    this.rdpegfxAdapter.SendRdpegfxFrameInSegment(pair.Value);  // Send a frame
                    frCount++;
                    if (positiveTest)  // Positive test case
                    {
                        if (frCount == frDict.Count * layerDataList.Count)
                        {
                            // Last frame, return fid and check result at outside
                            return pair.Key;
                        }
                        else
                        {
                            // Not last frame, check frame ack 
                            // If frame ack received, positive test OK and continue
                            // If no frame ack received, positive test NOK, assert it! 
                            this.rdpegfxAdapter.ExpectFrameAck(pair.Key);   // Wait for frame ack
                        }
                    }
                    else  // Negative test case, return fid and check result at outside
                    {
                        return pair.Key;
                    }
                }

                this.TestSite.Log.Add(LogEntryKind.Comment, "the layer {0} encoded bitmap data is sent OK!", i + 1);
                // If layer number larger than 1 and not the last layer data, wait for a while before sending next layer data
                if (layerDataList.Count > 1 && i < layerDataList.Count - 1)
                {
                    Thread.Sleep(RdpegfxTestUtility.delaySeconds * 1000);
                }
            }

            return 0xffffffff;  // Reaching here mean abnormal case, return invalid fid
        }

        #endregion
    }
}
