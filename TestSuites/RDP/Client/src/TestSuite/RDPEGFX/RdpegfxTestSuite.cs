// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpegfx;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdp;

namespace Microsoft.Protocols.TestSuites.Rdpegfx
{
    [TestClass]
    public partial class RdpegfxTestSuite : RdpTestClassBase
    {
        #region Adapter Instances
        private IRdpegfxAdapter rdpegfxAdapter;
        private RdpedycServer rdpedycServer;

        private RdpegfxTestUtility testData;

        private bool isH264AVC420Supported;
        private bool isH264AVC444Supported;
        private bool isSmallCache;
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
            this.rdpegfxAdapter = (IRdpegfxAdapter)this.TestSite.GetAdapter(typeof(IRdpegfxAdapter));
            this.rdpegfxAdapter.Reset();
            this.GetTestData();
            this.rdpbcgrAdapter.TurnVerificationOff(true);

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
            StopRDPConnection();
            this.TestSite.Log.Add(LogEntryKind.Comment, "Stop RDP listening.");
            this.rdpbcgrAdapter.StopRDPListening();

            DynamicVCException.SetCleanUp(false);
        }
        #endregion

        #region Private Methods

        //Set default server capabilities
        private void setServerCapabilitiesWithRemoteFxSupported()
        {
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true,
               true, maxRequestSize,
               true,
               true, 1,
               true, CmdFlags_Values.SURFCMDS_FRAMEMARKER | CmdFlags_Values.SURFCMDS_SETSURFACEBITS | CmdFlags_Values.SURFCMDS_STREAMSURFACEBITS,
               true, true, true);
        }

        /// <summary>
        /// Start RDP connection
        /// </summary>
        private void StartRDPConnection(bool isSoftSync = false)
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
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            // Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");

            MULTITRANSPORT_TYPE_FLAGS flags = MULTITRANSPORT_TYPE_FLAGS.None;
            if (isSoftSync)
            {
                flags = MULTITRANSPORT_TYPE_FLAGS.SOFTSYNC_TCP_TO_UDP | MULTITRANSPORT_TYPE_FLAGS.TRANSPORTTYPE_UDPFECL;
            }
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion, flags, true, isSoftSync);

            if (isSoftSync)
            {
                Site.Assert.IsTrue(this.rdpbcgrAdapter.SessionContext.MultitransportTypeFlagsInMCSConnectIntialPdu.HasFlag(MULTITRANSPORT_TYPE_FLAGS.SOFTSYNC_TCP_TO_UDP),
                   "Client Should support Soft-Sync, flags: {0}",
                   this.rdpbcgrAdapter.SessionContext.MultitransportTypeFlagsInMCSConnectIntialPdu);
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            this.rdpedycServer = new RdpedycServer(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);
            rdpegfxAdapter.AttachRdpbcgrAdapter(this.rdpbcgrAdapter);
            rdpedycServer.ExchangeCapabilities(waitTime);

        }

        /// <summary>
        /// Stop RDP connection
        /// </summary>
        private void StopRDPConnection()
        {
            int iResult = this.sutControlAdapter.TriggerClientDisconnectAll(this.TestContext.TestName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "The result of TriggerClientDisconnectAll is {0}.", iResult);
            this.rdpbcgrAdapter.Reset();
            this.rdpegfxAdapter.Reset();
        }

        /// <summary>
        /// Get Test Image Data
        /// </summary>
        private void GetTestData()
        {

            testData = new RdpegfxTestUtility();

            // Load clearcodec image
            String RdpegfxClearCodecImagePath;
            if (!PtfPropUtility.GetStringPtfProperty(TestSite, "ClearCodecImage", out RdpegfxClearCodecImagePath))
            {
                RdpegfxClearCodecImagePath = "";
            }

            try
            {
                testData.ClearCodecImage = (Bitmap)Image.FromFile(RdpegfxClearCodecImagePath);
            }
            catch (System.IO.FileNotFoundException)
            {
                testData.ClearCodecImage = RdpegfxTestUtility.captureFromScreen(0, 0, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            }

            // Load RfxProgressiveCodec image
            String RdpegfxRfxProgCodecImagePath;
            if (!PtfPropUtility.GetStringPtfProperty(TestSite, "RfxProgressiveCodecImage", out RdpegfxRfxProgCodecImagePath))
            {
                RdpegfxRfxProgCodecImagePath = "";
            }
            try
            {
                testData.RfxProgCodecImage = (Bitmap)Image.FromFile(RdpegfxRfxProgCodecImagePath);
            }
            catch (System.IO.FileNotFoundException)
            {
                testData.RfxProgCodecImage = RdpegfxTestUtility.captureFromScreen(0, 0, RdpegfxTestUtility.surfWidth, RdpegfxTestUtility.surfHeight);
            }

        }

        /// <summary>
        /// Load H264 test data for a test suite.
        /// </summary>
        /// <param name="H264TestDataPath"></param>
        /// <returns></returns>
        private RdpegfxH264TestDatas GetH264TestData(string H264TestDataPath)
        {
            RdpegfxH264TestDatas H264TestData = null;

            XmlSerializer serializer = new XmlSerializer(typeof(RdpegfxH264TestDatas));
            FileStream fs = new FileStream(H264TestDataPath, FileMode.Open);
            XmlTextReader reader = new XmlTextReader(fs);
            reader.XmlResolver = null;
            reader.DtdProcessing = System.Xml.DtdProcessing.Prohibit; // Prohibit DTD processing when using XmlTextReader on untrusted sources
            H264TestData = (RdpegfxH264TestDatas)serializer.Deserialize(reader);
            fs.Close();

            return H264TestData;

        }

        /// <summary>
        /// Method to do capability exchange with RDP client.
        /// This function is recommended to be called by other test cases to do capability exchange.
        /// </summary>
        private void RDPEGFX_CapabilityExchange(DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP, bool isSoftSync = false)
        {
            this.TestSite.Log.Add(LogEntryKind.Debug, "Establishing RDP connection ...");
            StartRDPConnection(isSoftSync);

            this.TestSite.Log.Add(LogEntryKind.Debug, "Creating dynamic virtual channels for MS-RDPEGFX ...");
            bool bProtocolSupported = isSoftSync ? InitializeForSoftSync(transportType) : this.rdpegfxAdapter.ProtocolInitialize(this.rdpedycServer, transportType);
            TestSite.Assert.IsTrue(bProtocolSupported, "Client should support this protocol.");

            this.TestSite.Log.Add(LogEntryKind.Debug, "Expecting capability advertise from client.");
            RDPGFX_CAPS_ADVERTISE capsAdv = this.rdpegfxAdapter.ExpectCapabilityAdvertise();
            this.TestSite.Assert.IsNotNull(capsAdv, "RDPGFX_CAPS_ADVERTISE is received.");

            this.isH264AVC420Supported = false;
            this.isH264AVC444Supported = false;
            this.isSmallCache = false;

            this.TestSite.Log.Add(LogEntryKind.Debug, "Sending capability confirm to client.");
            // Set first capset in capability advertise request, if no capdata in request, use default flag.
            CapsFlags capFlag = CapsFlags.RDPGFX_CAPS_FLAG_DEFAULT;
            CapsVersions version = CapsVersions.RDPGFX_CAPVERSION_8;
            if (capsAdv.capsSetCount > 0)
            {
                foreach (RDPGFX_CAPSET capSet in capsAdv.capsSets)
                {
                    CapsFlags flag = (CapsFlags)BitConverter.ToUInt32(capSet.capsData, 0);
                    if (capSet.version >= version)
                    {
                        version = capSet.version;
                        capFlag = flag;
                    }

                    if (capSet.version == CapsVersions.RDPGFX_CAPVERSION_81
                        && (flag & CapsFlags.RDPGFX_CAPS_FLAG_AVC420_ENABLED) == CapsFlags.RDPGFX_CAPS_FLAG_AVC420_ENABLED)
                    {
                        this.isH264AVC420Supported = true;
                    }
                    else if (capSet.version == CapsVersions.RDPGFX_CAPVERSION_10
                        || capSet.version == CapsVersions.RDPGFX_CAPVERSION_102
                        || capSet.version == CapsVersions.RDPGFX_CAPVERSION_103
                        || capSet.version == CapsVersions.RDPGFX_CAPVERSION_104
                        || capSet.version == CapsVersions.RDPGFX_CAPVERSION_105
                        || capSet.version == CapsVersions.RDPGFX_CAPVERSION_106

                        && (flag & CapsFlags.RDPGFX_CAPS_FLAG_AVC_DISABLED) == 0)
                    {
                        this.isH264AVC420Supported = true;
                        this.isH264AVC444Supported = true;
                    }

                    if ((flag & CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE) == CapsFlags.RDPGFX_CAPS_FLAG_SMALL_CACHE)
                    {
                        this.isSmallCache = true;
                    }

                    /// The bitmap cache of RDPGFX_CAPSET_VERSION103 MUST be constrained to 16MB in size
                    if (capSet.version == CapsVersions.RDPGFX_CAPVERSION_103)
                    {
                        this.isSmallCache = true;
                    }
                }
            }
            this.rdpegfxAdapter.SendCapabilityConfirm(capFlag, version);
        }

        /// <summary>
        /// Create graphic dynamic virtual channel over UDP transport.
        /// </summary>
        private bool InitializeForSoftSync(DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_UDP_Reliable)
        {
            uint? channelId = null;

            if (!rdpedycServer.IsMultipleTransportCreated(transportType))
            {
                rdpedycServer.CreateMultipleTransport(transportType);

                this.TestSite.Log.Add(LogEntryKind.Comment, "Expect for Client Initiate Multitransport PDU to indicate that the client was able to successfully complete the multitransport initiation request.");
                rdpbcgrAdapter.WaitForPacket<Client_Initiate_Multitransport_Response_PDU>(waitTime);
                TestSite.Assert.IsTrue(
                    rdpbcgrAdapter.SessionContext.ClientInitiateMultitransportResponsePDU.hrResponse == HrResponse_Value.S_OK,
                    "hrResponse field should be {0}", HrResponse_Value.S_OK);

                channelId = DynamicVirtualChannel.NewChannelId();

                List<uint> list = new List<uint>();
                list.Add((uint)channelId);

                Dictionary<TunnelType_Value, List<uint>> channelListDic = new Dictionary<TunnelType_Value, List<uint>>();

                TunnelType_Value tunnelType = TunnelType_Value.TUNNELTYPE_UDPFECR;
                if (transportType == DynamicVC_TransportType.RDP_UDP_Lossy)
                {
                    tunnelType = TunnelType_Value.TUNNELTYPE_UDPFECL;
                }
                channelListDic.Add(tunnelType, list);

                rdpedycServer.SoftSyncNegotiate(waitTime, channelListDic);

            }
            return this.rdpegfxAdapter.CreateEGFXDvc(rdpedycServer, transportType, channelId);
        }

        /// <summary>
        /// Whether H264 codec, AVC420 or AVC444, is supported
        /// </summary>
        /// <returns>true if H264 codec is supported, otherwise return false</returns>
        private bool IsH264Supported()
        {
            return isH264AVC420Supported || isH264AVC444Supported;
        }

        /// <summary>
        /// Send Rfx Codec Encoded bitmap data to client
        /// </summary>
        /// <param name="frDataDict"> It indicates the encoded frames </param>
        /// <returns> It returns frame id of last sent frame </returns>
        uint SendRfxCodecEcodedData(Dictionary<uint, byte[]> frDataDict)
        {
            uint frCount = 0;

            if (frDataDict == null) return 0xffffffff; // return invalid fid if no layer data

            foreach (KeyValuePair<uint, byte[]> pair in frDataDict)
            {
                this.rdpegfxAdapter.SendRdpegfxFrameInSegment(pair.Value);
                frCount++;

                if (frCount == frDataDict.Count)
                {
                    // Last frame, return fid and check result at outside
                    return pair.Key;
                }
                else
                {
                    // Not last frame, check frame ack 
                    // If frame ack received, positive test OK and continue
                    // If no frame ack received, positive test NOK, assert it! 
                    this.rdpegfxAdapter.ExpectFrameAck(pair.Key);
                }
            }
            return 0xffffffff;  // Reaching here mean abnormal case, return invalid fid
        }

        /// <summary>
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

        /// <summary>
        /// Verify SUT Display
        /// </summary>
        /// <param name="usingRemoteFX">Whether the output image is using RemoteFX codec</param>
        /// <param name="rect">The Rectangle on the image to be compared</param>
        /// <param name="callStackIndex">Call stack index from the test method, which help to get test method name in the function</param>
        private void VerifySUTDisplay(bool usingRemoteFX, RDPGFX_RECT16 rect, int callStackIndex = 1)
        {
            Rectangle compareRect = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
            base.VerifySUTDisplay(usingRemoteFX, compareRect, callStackIndex + 1);
        }

        /// <summary>
        /// Verify SUT Display
        /// </summary>
        /// <param name="usingRemoteFX">Whether the output image is using RemoteFX codec</param>
        /// <param name="rects">Array of rectangles on the image to be compared</param>
        /// <param name="callStackIndex">Call stack index from the test method, which help to get test method name in the function</param>
        private void VerifySUTDisplay(bool usingRemoteFX, RDPGFX_RECT16[] rects, int callStackIndex = 1)
        {
            if (rects != null && rects.Length > 0)
            {
                foreach (RDPGFX_RECT16 rect in rects)
                {
                    Rectangle compareRect = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
                    base.VerifySUTDisplay(usingRemoteFX, compareRect, callStackIndex + 1);
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Utility class used for RDPEGFX test. The fields defined here are only used internally.
    /// </summary>
    public class RdpegfxTestUtility
    {
        // Fields
        public static uint desktopWidth = 1024;
        public static uint desktopHeight = 768;
        public static uint maxMonitorCount = 16;
        public static RDPGFX_POINT16 surfPos = new RDPGFX_POINT16(64, 64);  // Relative to virtual desktop
        public static RDPGFX_POINT16 surfPos2 = new RDPGFX_POINT16(320, 320); // Relative to virtual desktop
        public static RDPGFX_POINT16 surfPos3 = new RDPGFX_POINT16(0, 0);  // Relative to virtual desktop
        public static RDPGFX_POINT16 surfPos4 = new RDPGFX_POINT16(192, 192);
        public static ushort surfWidth = 256;
        public static ushort surfHeight = 256;
        public static ushort surfWidth2 = 256;
        public static ushort surfHeight2 = 256;
        public static ushort surfWidth3 = 356;
        public static ushort surfHeight3 = 356;
        public static ushort largeSurfWidth = 512;
        public static ushort largeSurfHeight = 512;
        public static RDPGFX_COLOR32 fillColorGreen = ToRdpgfx_Color32(Color.Green);
        public static RDPGFX_COLOR32 fillColorRed = ToRdpgfx_Color32(Color.Red);
        public static RDPGFX_COLOR32 fillColorBlue = ToRdpgfx_Color32(Color.Blue);
        public static RDPGFX_COLOR32 fillColorARGB = new RDPGFX_COLOR32(0xff, 0xff, 0xff, 0xff);  // ARGB format color
        public static RDPGFX_POINT16 imgPos = new RDPGFX_POINT16(0, 0);            // Relative to surface
        public static RDPGFX_POINT16 imgPos2 = new RDPGFX_POINT16(64, 64);          // Relative to surface
        public static RDPGFX_POINT16 imgPos3 = new RDPGFX_POINT16(100, 100);          // Relative to surface
        public static RDPGFX_POINT16 imgPos4 = new RDPGFX_POINT16(512, 512);          // Relative to surface
        public static RDPGFX_RECT16 cacheRect = new RDPGFX_RECT16(0, 0, 64, 64);  // Relative to source surface
        public static RDPGFX_RECT16 largeCacheRect = new RDPGFX_RECT16(0, 0, 512, 512); // Relative to source surface
        public static RDPGFX_RECT16 copySrcRect = new RDPGFX_RECT16(0, 0, 64, 64); // Relative to source surface
        public static RDPGFX_RECT16 copySrcRect2 = new RDPGFX_RECT16(0, 0, 512, 512); // Relative to source surface
        public static ushort smallWidth = 64;
        public static ushort smallHeight = 64;
        public static ushort ccBandWidth = 50;
        public static ushort ccBandHeight = 20;
        public static ushort ccBandWidth2 = 41;
        public static ushort ccBandHeight2 = 25;
        public static ushort ccMaxBandHeight = 52;
        public static ushort ccSubcodecWidth = 16;
        public static ushort ccSubcodecHeight = 8;
        public static ushort ccSubcodecMaxWidth = 127;
        public static ushort ccSmallGlyphWidth = 1;
        public static ushort ccSmallGlyphHeight = 2;
        public static ushort ccGlyphWidth = 64;
        public static ushort ccGlyphIndex = 999;
        public static ushort ccMaxGlyphSize = 1024;
        public static ushort ccMaxGlyphIndexNum = 4000;
        public static ushort MaxBmpWidth = 32766;  // TDI number is 0xffff. This value is default value of RDP8.0
        public static ushort MaxBmpHeight = 32766; // TDI number is 0xffff. This value is default value of RDP8.0
        public static ushort ccLargeBandWidth = 256;
        public static ushort ccLargeBandHeight = 256;
        public static uint segmentPartSize = 65535; // The size of the pure data in a RDP8_BULK_ENCODED_DATA structure       

        public static uint maxRfxProgCodecContextId = 0xffffffff;
        public static byte delaySeconds = 1;

        public static ulong cacheKey = 0xf0f0f0f07f7f7f7f;

        public static ushort maxCacheSlot = 25600;

        public static ushort maxCacheSlotForSmallCache = 4096;

        public static int maxCacheSize = 100; // 100M
        public static int maxCacheSizeForSmallCache = 16; //16M

        public Bitmap ClearCodecImage;
        public Bitmap RfxProgCodecImage;


        /// <summary>
        /// Transform a C# color into RDPGRX_COLOR32 structure
        /// </summary>
        /// <param name="c"> Color to be converted </param>
        /// <returns> RDPGRX_COLOR32 structure color </returns>
        public static RDPGFX_COLOR32 ToRdpgfx_Color32(Color c)
        {
            return new RDPGFX_COLOR32(c.B, c.G, c.R, c.A);
        }
        /// <summary>
        /// Convert a position and width/height into Rectangle structure
        /// </summary>
        /// <param name="pos"> Left-top position of a rectangle </param>
        /// <param name="width"> Width of a rectangle </param>
        /// <param name="height"> Height of a rectangle </param>
        /// <returns> Converted rectangle </returns>
        public static RDPGFX_RECT16 ConvertToRect(RDPGFX_POINT16 pos, ushort width, ushort height)
        {
            return new RDPGFX_RECT16(pos.x, pos.y, (ushort)(pos.x + width), (ushort)(pos.y + height));
        }

        /// <summary>
        /// Capture a bitmap from screen
        /// </summary>
        /// <param name="left"> Left position in screen for captured bitmap </param>
        /// <param name="top"> Top position in screen for captured bitmap </param>
        /// <param name="width"> Width of captured bitmap </param>
        /// <param name="height"> Height of captured bitmap </param>
        /// <returns> The captured bitmap </returns>
        public static Bitmap captureFromScreen(int left, int top, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            g.CopyFromScreen(left, top, 0, 0, new Size(width, height));
            g.Dispose();
            return bitmap;
        }

        /// <summary>
        /// Capture a bitmap from an image
        /// </summary>
        /// <param name="srcImage"> The source of captured bitmap </param>
        /// <param name="capPos"> Left-top position in source image for captured bitmap  </param>
        /// <param name="width"> Width of captured bitmap </param>
        /// <param name="height"> Height of captured bitmap </param>
        /// <param name="bgImage"> The image after a part is captured </param>
        /// <returns> The captured bitmap </returns>
        public static Bitmap captureFromImage(Image srcImage, RDPGFX_POINT16 capPos, int width, int height, out Image bgImage)
        {
            // Cut a bitmap(width * height) from position(left,top) of srcImage
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            Rectangle destRect = new Rectangle(0, 0, width, height);
            Rectangle srcRect = new Rectangle(capPos.x, capPos.y, width, height);
            g.DrawImage(srcImage, destRect, srcRect, GraphicsUnit.Pixel);

            // Fill the cut rectangle with solid color at position(left, top) 
            bgImage = srcImage;
            Bitmap bgBmp = (Bitmap)bgImage;
            Color brushColor = bgBmp.GetPixel(capPos.x, capPos.y);
            SolidBrush pixelBrush = new SolidBrush(brushColor);
            Graphics srcg = Graphics.FromImage(srcImage);
            srcg.FillRectangle(pixelBrush, srcRect);
            return bitmap;
        }

        /// <summary>
        /// Draw an image with a diagonal
        /// </summary>
        /// <param name="srcImage"> The source image </param>
        /// <returns> The image with diagonal </returns>
        public static Bitmap drawDiagonal(Image srcImage)
        {
            Bitmap newBmp = (Bitmap)srcImage;
            for (int x = 0; x < newBmp.Width; x++)
            {
                int y = (x * newBmp.Height) / newBmp.Width;
                newBmp.SetPixel(x, y, Color.Black);
            }

            return newBmp;
        }

        /// <summary>
        /// Draw an image with a color
        /// </summary>
        /// <param name="w"> Width of an image </param>
        /// <param name="h"> Height of an image </param>
        /// <param name="c"> The color to fill image </param>
        /// <returns> The drawn bitmap </returns>
        public static Bitmap DrawImage(int w, int h, Color c)
        {
            Bitmap bitmap = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(bitmap);
            SolidBrush pixelBrush = new SolidBrush(c);
            g.FillRectangle(pixelBrush, 0, 0, w, h);

            return bitmap;
        }

        /// <summary>
        /// Draw an image with gradient color.
        /// The image is only used for clearcode subcodec test, and the width of image can't exceed 128.
        /// </summary>
        /// <param name="w"> Width of an image </param>
        /// <param name="h"> Height of an image </param>
        /// <param name="startColor"> The start color to draw image </param>
        /// <returns> The result bitmap </returns>
        public static Bitmap DrawGradientImage(int w, int h, Color startColor)
        {
            Bitmap bitmap = new Bitmap(w, h);

            for (int y = 0; y < h; y++)
            {
                int argb = startColor.ToArgb();
                for (int x = 0; x < w; x++)
                {
                    Color c = Color.FromArgb(argb);
                    bitmap.SetPixel(x, y, c);
                    argb++;         // Change color for next pixel
                }
            }

            return bitmap;
        }

        /// <summary>
        /// Draw a bitmap with each pixel has unique color. 
        /// The image is only used for clearcode subcodec test, and the pixel number in image can't exceed 128.
        /// </summary>
        /// <param name="w"> Width of image </param>
        /// <param name="h"> Height of image </param>
        /// <param name="startColor"> The start color to draw image </param>
        /// <returns> The result bitmap </returns>
        public static Bitmap DrawImageWithUniqueColor(int w, int h, Color startColor)
        {
            Bitmap bitmap = new Bitmap(w, h);
            int argb = startColor.ToArgb();
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color c = Color.FromArgb(argb);
                    bitmap.SetPixel(x, y, c);
                    argb++;                // Change color for next pixel
                }
            }

            return bitmap;
        }

        /// <summary>
        /// Draw an image for surface, which has a bitmap in surface.
        /// </summary>
        /// <param name="surf"> Indicate the size of surface image </param>
        /// <param name="bgcolor"> Indicate the background color of surface image </param>
        /// <param name="pic"> The bitmap to be drawn in surface image </param>
        /// <param name="picPos"> The bitmap position in surface </param>
        /// <returns> A surface image </returns>
        public static Bitmap DrawSurfImage(Surface surf, Color bgcolor, Bitmap pic, RDPGFX_POINT16 picPos)
        {
            Bitmap bitmap = new Bitmap(surf.Width, surf.Height);
            Graphics g = Graphics.FromImage(bitmap);
            SolidBrush pixelBrush = new SolidBrush(bgcolor);
            g.FillRectangle(pixelBrush, 0, 0, surf.Width, surf.Height);
            for (int x = picPos.x; (x < surf.Width) && (x < (picPos.x + pic.Width)); x++)
            {
                for (int y = picPos.y; (y < surf.Height) && (y < (picPos.y + pic.Height)); y++)
                {
                    Color c = pic.GetPixel(x - picPos.x, y - picPos.y);
                    bitmap.SetPixel(x, y, c);
                }
            }

            return bitmap;
        }
    }
}
