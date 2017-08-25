// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestSuites.Rdp;

namespace Microsoft.Protocols.TestSuites.Rdprfx
{
    [TestClass]
    public partial class RdprfxTestSuite : RdpTestClassBase
    {

        #region Adapter Instances
        private IRdprfxAdapter rdprfxAdapter;
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

            this.rdprfxAdapter = (IRdprfxAdapter)this.TestSite.GetAdapter(typeof(IRdprfxAdapter));
            this.rdprfxAdapter.Reset();
            this.rdpbcgrAdapter.TurnVerificationOff(true);
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();

            this.TestSite.Log.Add(LogEntryKind.Comment, "Trigger client to close all RDP connections for clean up.");
            int iResult = this.sutControlAdapter.TriggerClientDisconnectAll(this.TestContext.TestName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "The result of TriggerClientDisconnectAll is {0}.", iResult);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Stop RDP listening.");
            this.rdpbcgrAdapter.StopRDPListening();
        }
        #endregion

        #region Private Methods

        private void receiveAndLogClientRfxCapabilites()
        {
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

        private void setServerCapabilitiesWithRemoteFxSupported()
        {
            this.rdpbcgrAdapter.SetServerCapability(true, true, true, true, true, true, true, true, true, true,
               true, maxRequestSize,
               true,
               true, 1,
               true, CmdFlags_Values.SURFCMDS_FRAMEMARKER | CmdFlags_Values.SURFCMDS_SETSURFACEBITS | CmdFlags_Values.SURFCMDS_STREAMSURFACEBITS,
               true, true, true);
        }

        private void StartRDPConnection()
        {

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol, true);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            //Initial the RDPRFX adapter context.
            rdprfxAdapter.Accept(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);
        }

        private void StopRDPConnection()
        {
            int iResult = this.sutControlAdapter.TriggerClientDisconnectAll(this.TestContext.TestName);
            this.TestSite.Log.Add(LogEntryKind.Debug, "The result of TriggerClientDisconnectAll is {0}.", iResult);
            this.rdpbcgrAdapter.Reset();
            this.rdprfxAdapter.Reset();
        }

        private TS_RFX_CODEC_QUANT[] GenerateCodecQuantVals()
        {
            TS_RFX_CODEC_QUANT[] quantVals = new TS_RFX_CODEC_QUANT[3];
            quantVals[0].LL3_LH3 = 0x66;
            quantVals[0].HL3_HH3 = 0x66;
            quantVals[0].LH2_HL2 = 0x77;
            quantVals[0].HH2_LH1 = 0x88;
            quantVals[0].HL1_HH1 = 0x98;

            quantVals[1].LL3_LH3 = 0x66;
            quantVals[1].HL3_HH3 = 0x66;
            quantVals[1].LH2_HL2 = 0x99;
            quantVals[1].HH2_LH1 = 0x99;
            quantVals[1].HL1_HH1 = 0x99;

            quantVals[2].LL3_LH3 = 0x66;
            quantVals[2].HL3_HH3 = 0x88;
            quantVals[2].LH2_HL2 = 0x77;
            quantVals[2].HH2_LH1 = 0x88;
            quantVals[2].HL1_HH1 = 0x98;

            return quantVals;
        }
        #endregion

        private void FourTilesComposeOneRectWithoutCommonBoundary(OperationalMode mode)
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, the TS_RFX_REGION structure contains rectangular which is composed by four tiles.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol, true);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            //Initial the RDPRFX adapter context.
            rdprfxAdapter.Accept(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);

            receiveAndLogClientRfxCapabilites();

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = mode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Check if the above setting is supported by client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "the input pair of Operational Mode ({0}) / Entropy Algorithm ({1}) is not supported by client, so stop running this test case.", opMode, enAlgorithm);
                return;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Start to send encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create rectangles whose height is TileSize, this rectangle is used for TS_RFX_REGION structure to clip the bitmap.");
            Rectangle clipRect = new Rectangle(32, 32, RgbTile.TileSize, RgbTile.TileSize);
            Rectangle[] rects = new Rectangle[] { clipRect };

            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxChannels(RgbTile.TileSize + RgbTile.TileSize / 2, RgbTile.TileSize + RgbTile.TileSize / 2);
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);

            rdprfxAdapter.SendTsRfxFrameBegin(0);
            rdprfxAdapter.SendTsRfxRegion(rects);
            TILE_POSITION[] positions = new TILE_POSITION[4]{
                new TILE_POSITION{ xIdx = 0, yIdx = 0 },
                new TILE_POSITION{ xIdx = 0, yIdx = 1 },
                new TILE_POSITION{ xIdx = 1, yIdx = 0 },
                new TILE_POSITION{ xIdx = 1, yIdx = 1 }
            };
            rdprfxAdapter.SendTsRfxTileSet(
                opMode,
                enAlgorithm,
                new Image[] { image_64X64, image_64X64, image_64X64, image_64X64 },
                positions);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop, RgbTile.TileSize + RgbTile.TileSize / 2, RgbTile.TileSize + RgbTile.TileSize / 2);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            Rectangle compareRect = new Rectangle(32, 32, image_64X64.Width, image_64X64.Height);
            this.VerifySUTDisplay(true, compareRect);

            #endregion
        }

        private void FourTilesComposeOneRectWithCommonBoundary(OperationalMode mode)
        {
            #region Test Description
            /*
             Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, the TS_RFX_REGION structure contains rectangular which is composed by four tiles.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol, true);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            //Initial the RDPRFX adapter context.
            rdprfxAdapter.Accept(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);

            receiveAndLogClientRfxCapabilites();

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = mode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Check if the above setting is supported by client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "the input pair of Operational Mode ({0}) / Entropy Algorithm ({1}) is not supported by client, so stop running this test case.", opMode, enAlgorithm);
                return;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Start to send encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create rectangles whose height is TileSize + TileSize/2, this rectangle is used for TS_RFX_REGION structure to clip the bitmap.");
            Rectangle clipRect = new Rectangle(32, 32, RgbTile.TileSize + RgbTile.TileSize / 2, RgbTile.TileSize + RgbTile.TileSize / 2);
            Rectangle[] rects = new Rectangle[] { clipRect };

            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxChannels(RgbTile.TileSize * 2, RgbTile.TileSize * 2);
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);

            rdprfxAdapter.SendTsRfxFrameBegin(0);
            rdprfxAdapter.SendTsRfxRegion(rects);
            TILE_POSITION[] positions = new TILE_POSITION[4]{
                new TILE_POSITION{ xIdx = 0, yIdx = 0 },
                new TILE_POSITION{ xIdx = 0, yIdx = 1 },
                new TILE_POSITION{ xIdx = 1, yIdx = 0 },
                new TILE_POSITION{ xIdx = 1, yIdx = 1 }
            };
            rdprfxAdapter.SendTsRfxTileSet(
                opMode,
                enAlgorithm,
                new Image[] { image_64X64, image_64X64, image_64X64, image_64X64 },
                positions);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop, RgbTile.TileSize * 2, RgbTile.TileSize * 2);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            Rectangle compareRect = new Rectangle(32, 32, RgbTile.TileSize + RgbTile.TileSize / 2, RgbTile.TileSize + RgbTile.TileSize / 2);
            this.VerifySUTDisplay(true, compareRect);

            #endregion
        }

        private void SendListOfRects(OperationalMode mode)
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, the TS_RFX_REGION structure contains a list of rectangles.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol, true);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            //Initial the RDPRFX adapter context.
            rdprfxAdapter.Accept(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);

            receiveAndLogClientRfxCapabilites();

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = mode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Check if the above setting is supported by client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "the input pair of Operational Mode ({0}) / Entropy Algorithm ({1}) is not supported by client, so stop running this test case.", opMode, enAlgorithm);
                return;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Start to send encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create rectangles list, this rectangle list is used for TS_RFX_REGION structure to clip the bitmap.");
            Rectangle clipRect1 = new Rectangle(0, 0, RgbTile.TileSize * 2, RgbTile.TileSize * 2);
            Rectangle clipRect2 = new Rectangle(RgbTile.TileSize * 3, RgbTile.TileSize * 3, RgbTile.TileSize * 2, RgbTile.TileSize * 2);

            Rectangle[] rects = new Rectangle[] { clipRect1, clipRect2 };

            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxChannels(RgbTile.TileSize * 5, RgbTile.TileSize * 5);
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);

            var bitmapBlue = new Bitmap(RgbTile.TileSize, RgbTile.TileSize);
            Graphics graphics = Graphics.FromImage(bitmapBlue);
            graphics.FillRectangle(Brushes.Blue, new Rectangle(0, 0, RgbTile.TileSize, RgbTile.TileSize));
            var bitmapRed = new Bitmap(RgbTile.TileSize, RgbTile.TileSize);
            graphics = Graphics.FromImage(bitmapRed);
            graphics.FillRectangle(Brushes.Red, new Rectangle(0, 0, RgbTile.TileSize, RgbTile.TileSize));

            rdprfxAdapter.SendTsRfxFrameBegin(0);
            rdprfxAdapter.SendTsRfxRegion(rects);

            TILE_POSITION[] positions = new TILE_POSITION[8]{
                new TILE_POSITION{ xIdx = 0, yIdx = 0 },
                new TILE_POSITION{ xIdx = 1, yIdx = 0 },
                new TILE_POSITION{ xIdx = 0, yIdx = 1 },
                new TILE_POSITION{ xIdx = 1, yIdx = 1 },
                new TILE_POSITION{ xIdx = 3, yIdx = 3 },
                new TILE_POSITION{ xIdx = 3, yIdx = 4 },
                new TILE_POSITION{ xIdx = 4, yIdx = 3 },
                new TILE_POSITION{ xIdx = 4, yIdx = 4 }
            };

            rdprfxAdapter.SendTsRfxTileSet(
                opMode,
                enAlgorithm,
                new Image[] { bitmapBlue, bitmapBlue, bitmapBlue, bitmapBlue, bitmapRed, bitmapRed, bitmapRed, bitmapRed },
                positions);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop, RgbTile.TileSize * 5, RgbTile.TileSize * 5);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            Rectangle compareRect = new Rectangle(destLeft, destTop, RgbTile.TileSize * 5, RgbTile.TileSize * 5);
            this.VerifySUTDisplay(true, compareRect);

            #endregion
        }

        private void SendListOfRectsOverlap(OperationalMode mode)
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, the TS_RFX_REGION structure contains a list of rectangular which overlap with each other.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol, true);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            //Initial the RDPRFX adapter context.
            rdprfxAdapter.Accept(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);

            receiveAndLogClientRfxCapabilites();

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = mode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Check if the above setting is supported by client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "the input pair of Operational Mode ({0}) / Entropy Algorithm ({1}) is not supported by client, so stop running this test case.", opMode, enAlgorithm);
                return;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Start to send encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create rectangles list, this rectangle list is used for TS_RFX_REGION structure to clip the bitmap.");
            Rectangle clipRect1 = new Rectangle(0, 0, RgbTile.TileSize * 2, RgbTile.TileSize * 2);
            Rectangle clipRect2 = new Rectangle(RgbTile.TileSize, RgbTile.TileSize, RgbTile.TileSize * 2, RgbTile.TileSize * 2);

            Rectangle[] rects = new Rectangle[] { clipRect1, clipRect2 };

            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxChannels(RgbTile.TileSize * 3, RgbTile.TileSize * 3);
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);

            var bitmapBlue = new Bitmap(RgbTile.TileSize, RgbTile.TileSize);
            Graphics graphics = Graphics.FromImage(bitmapBlue);
            graphics.FillRectangle(Brushes.Blue, new Rectangle(0, 0, RgbTile.TileSize, RgbTile.TileSize));
            var bitmapRed = new Bitmap(RgbTile.TileSize, RgbTile.TileSize);
            graphics = Graphics.FromImage(bitmapRed);
            graphics.FillRectangle(Brushes.Red, new Rectangle(0, 0, RgbTile.TileSize, RgbTile.TileSize));
            var bitmapYellow = new Bitmap(RgbTile.TileSize, RgbTile.TileSize);
            graphics = Graphics.FromImage(bitmapYellow);
            graphics.FillRectangle(Brushes.Yellow, new Rectangle(0, 0, RgbTile.TileSize, RgbTile.TileSize));

            rdprfxAdapter.SendTsRfxFrameBegin(0);
            rdprfxAdapter.SendTsRfxRegion(rects);

            TILE_POSITION[] positions = new TILE_POSITION[7]{
                new TILE_POSITION{ xIdx = 0, yIdx = 0 },
                new TILE_POSITION{ xIdx = 1, yIdx = 0 },
                new TILE_POSITION{ xIdx = 0, yIdx = 1 },
                new TILE_POSITION{ xIdx = 1, yIdx = 1 },
                new TILE_POSITION{ xIdx = 1, yIdx = 2 },
                new TILE_POSITION{ xIdx = 2, yIdx = 1 },
                new TILE_POSITION{ xIdx = 2, yIdx = 2 },
            };

            rdprfxAdapter.SendTsRfxTileSet(
                opMode,
                enAlgorithm,
                new Image[] { bitmapBlue, bitmapBlue, bitmapBlue, bitmapYellow, bitmapRed, bitmapRed, bitmapRed },
                positions);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop, RgbTile.TileSize * 3, RgbTile.TileSize * 3);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            Rectangle compareRect = new Rectangle(destLeft, destTop, RgbTile.TileSize * 3, RgbTile.TileSize * 3);
            this.VerifySUTDisplay(true, compareRect);

            #endregion
        }

        private void SendListOfRectsOverlapWithDuplicateTiles(OperationalMode mode)
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, the TS_RFX_REGION structure contains a list of rectangular which overlap with each other and contains a duplicated tile.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol, true);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            //Initial the RDPRFX adapter context.
            rdprfxAdapter.Accept(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);

            receiveAndLogClientRfxCapabilites();

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = mode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Check if the above setting is supported by client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "the input pair of Operational Mode ({0}) / Entropy Algorithm ({1}) is not supported by client, so stop running this test case.", opMode, enAlgorithm);
                return;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Start to send encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create rectangles list, this rectangle list is used for TS_RFX_REGION structure to clip the bitmap.");
            Rectangle clipRect1 = new Rectangle(0, 0, RgbTile.TileSize * 2, RgbTile.TileSize * 2);
            Rectangle clipRect2 = new Rectangle(RgbTile.TileSize, RgbTile.TileSize, RgbTile.TileSize * 2, RgbTile.TileSize * 2);

            Rectangle[] rects = new Rectangle[] { clipRect1, clipRect2 };

            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxChannels(RgbTile.TileSize * 3, RgbTile.TileSize * 3);
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);

            var bitmapBlue = new Bitmap(RgbTile.TileSize, RgbTile.TileSize);
            Graphics graphics = Graphics.FromImage(bitmapBlue);
            graphics.FillRectangle(Brushes.Blue, new Rectangle(0, 0, RgbTile.TileSize, RgbTile.TileSize));
            var bitmapRed = new Bitmap(RgbTile.TileSize, RgbTile.TileSize);
            graphics = Graphics.FromImage(bitmapRed);
            graphics.FillRectangle(Brushes.Red, new Rectangle(0, 0, RgbTile.TileSize, RgbTile.TileSize));

            rdprfxAdapter.SendTsRfxFrameBegin(0);
            rdprfxAdapter.SendTsRfxRegion(rects);

            TILE_POSITION[] positions = new TILE_POSITION[8]{
                new TILE_POSITION{ xIdx = 0, yIdx = 0 },
                new TILE_POSITION{ xIdx = 1, yIdx = 0 },
                new TILE_POSITION{ xIdx = 0, yIdx = 1 },
                new TILE_POSITION{ xIdx = 1, yIdx = 1 },
                new TILE_POSITION{ xIdx = 1, yIdx = 1 },
                new TILE_POSITION{ xIdx = 1, yIdx = 2 },
                new TILE_POSITION{ xIdx = 2, yIdx = 1 },
                new TILE_POSITION{ xIdx = 2, yIdx = 2 },
            };

            rdprfxAdapter.SendTsRfxTileSet(
                opMode,
                enAlgorithm,
                new Image[] { bitmapBlue, bitmapBlue, bitmapBlue, bitmapBlue, bitmapRed, bitmapRed, bitmapRed, bitmapRed },
                positions);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop, RgbTile.TileSize * 3, RgbTile.TileSize * 3);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            Rectangle compareRect = new Rectangle(destLeft, destTop, RgbTile.TileSize * 3, RgbTile.TileSize * 3);
            this.VerifySUTDisplay(true, compareRect);

            #endregion
        }

        private void NegtiveTest_DuplicatedTile(OperationalMode mode)
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, the TS_RFX_REGION structure contains rectangular which contains a duplicated tile.
            Step 5: [RDPRFX] Expect the client terminates the RDP connection.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol, true);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            //Initial the RDPRFX adapter context.
            rdprfxAdapter.Accept(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);

            receiveAndLogClientRfxCapabilites();

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = mode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Check if the above setting is supported by client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "the input pair of Operational Mode ({0}) / Entropy Algorithm ({1}) is not supported by client, so stop running this test case.", opMode, enAlgorithm);
                return;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Start to send encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create rectangles list, this rectangle list is used for TS_RFX_REGION structure to clip the bitmap.");
            Rectangle clipRect1 = new Rectangle(0, 0, RgbTile.TileSize * 2, RgbTile.TileSize * 2);

            Rectangle[] rects = new Rectangle[] { clipRect1 };

            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxChannels(RgbTile.TileSize * 2, RgbTile.TileSize * 2);
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);

            var bitmapBlue = new Bitmap(RgbTile.TileSize, RgbTile.TileSize);
            Graphics graphics = Graphics.FromImage(bitmapBlue);
            graphics.FillRectangle(Brushes.Blue, new Rectangle(0, 0, RgbTile.TileSize, RgbTile.TileSize));
            var bitmapRed = new Bitmap(RgbTile.TileSize, RgbTile.TileSize);
            graphics = Graphics.FromImage(bitmapRed);
            graphics.FillRectangle(Brushes.Red, new Rectangle(0, 0, RgbTile.TileSize, RgbTile.TileSize));

            rdprfxAdapter.SendTsRfxFrameBegin(0);
            rdprfxAdapter.SendTsRfxRegion(rects);

            TILE_POSITION[] positions = new TILE_POSITION[5]{
                new TILE_POSITION{ xIdx = 1, yIdx = 1 },
                new TILE_POSITION{ xIdx = 0, yIdx = 0 },
                new TILE_POSITION{ xIdx = 1, yIdx = 0 },
                new TILE_POSITION{ xIdx = 0, yIdx = 1 },
                new TILE_POSITION{ xIdx = 1, yIdx = 1 },
            };

            rdprfxAdapter.SendTsRfxTileSet(
                opMode,
                enAlgorithm,
                new Image[] { bitmapRed, bitmapBlue, bitmapBlue, bitmapBlue, bitmapBlue },
                positions);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop, RgbTile.TileSize * 2, RgbTile.TileSize * 2);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the client terminates the RDP connection.");
            bool bDisconnected = rdpbcgrAdapter.WaitForDisconnection(waitTime);

            this.TestSite.Assert.IsTrue(bDisconnected, "Client is expected to drop the connection if received duplicated tiles in Video Mode when Image Mode is in effect.");


            #endregion
        }

        private void SendNumRectsSetToZero(OperationalMode mode)
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, the TS_RFX_REGION structure contains numRects field of TS_RFX_REGION which is set to zero.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol, true);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            //Initial the RDPRFX adapter context.
            rdprfxAdapter.Accept(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);

            receiveAndLogClientRfxCapabilites();

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = mode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Check if the above setting is supported by client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "the input pair of Operational Mode ({0}) / Entropy Algorithm ({1}) is not supported by client, so stop running this test case.", opMode, enAlgorithm);
                return;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Start to send encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create rectangles whose height is TileSize/2, this rectangle is used for TS_RFX_REGION structure to clip the bitmap.");
            Rectangle clipRect = new Rectangle(destLeft, destTop, RgbTile.TileSize * 10, RgbTile.TileSize * 10);
            Rectangle[] rects = new Rectangle[] { clipRect };

            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxChannels(RgbTile.TileSize * 3, RgbTile.TileSize * 10);
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);

            rdprfxAdapter.SendTsRfxFrameBegin(0);
            rdprfxAdapter.SendTsRfxRegion(rects, true);
            TILE_POSITION[] positions = new TILE_POSITION[4]{
                new TILE_POSITION{ xIdx = 0, yIdx = 0 },
                new TILE_POSITION{ xIdx = 2, yIdx = 0 },
                new TILE_POSITION{ xIdx = 0, yIdx = 9 },
                new TILE_POSITION{ xIdx = 2, yIdx = 9 }
            };

            rdprfxAdapter.SendTsRfxTileSet(
                opMode,
                enAlgorithm,
                new Image[] { image_64X64, image_64X64, image_64X64, image_64X64 },
                positions);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop, RgbTile.TileSize * 3, RgbTile.TileSize * 10);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            Rectangle compareRect = new Rectangle(destLeft, destTop, RgbTile.TileSize * 3, RgbTile.TileSize * 10);
            this.VerifySUTDisplay(true, compareRect);

            #endregion
        }

        private void SendOutOfRects(OperationalMode mode)
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send Encode Header Messages to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages (encoded with RLGR1) to SUT, when some tiles are out of the rectangle in TS_RFX_REGION.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol, true);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);

            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            //Initial the RDPRFX adapter context.
            rdprfxAdapter.Accept(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);

            receiveAndLogClientRfxCapabilites();

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = mode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR1;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Check if the above setting is supported by client.
            if (!this.rdprfxAdapter.CheckIfClientSupports(opMode, enAlgorithm))
            {
                this.TestSite.Log.Add(LogEntryKind.Comment, "the input pair of Operational Mode ({0}) / Entropy Algorithm ({1}) is not supported by client, so stop running this test case.", opMode, enAlgorithm);
                return;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Start to send encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Create rectangles whose height is TileSize/2, this rectangle is used for TS_RFX_REGION structure to clip the bitmap.");
            Rectangle clipRect = new Rectangle(64, 64, RgbTile.TileSize, RgbTile.TileSize);
            Rectangle[] rects = new Rectangle[] { clipRect };

            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxChannels(RgbTile.TileSize * 2, RgbTile.TileSize * 2);
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);

            rdprfxAdapter.SendTsRfxFrameBegin(0);
            rdprfxAdapter.SendTsRfxRegion(rects);
            TILE_POSITION[] positions = new TILE_POSITION[4]{
                new TILE_POSITION{ xIdx = 0, yIdx = 0 },
                new TILE_POSITION{ xIdx = 0, yIdx = 1 },
                new TILE_POSITION{ xIdx = 1, yIdx = 0 },
                new TILE_POSITION{ xIdx = 1, yIdx = 1 }
            };
            rdprfxAdapter.SendTsRfxTileSet(
                opMode,
                enAlgorithm,
                new Image[] { image_64X64, image_64X64, image_64X64, image_64X64 },
                positions);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop, RgbTile.TileSize * 2, RgbTile.TileSize * 2);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Verify output on SUT Display if the verifySUTDisplay entry in PTF config is true.");
            Rectangle compareRect = new Rectangle(destLeft, destTop, RgbTile.TileSize * 2, RgbTile.TileSize * 2);
            this.VerifySUTDisplay(true, compareRect);

            #endregion
        }

        private void ChannelBoundaryTest_Channel(short channelWidth, short channelHeight)
        {
            #region Test Description
            /*
            Step 1: [RDPBCGR] establishing the connection.
            Step 2: [RDPBCGR] send Frame Maker Command (Begin) to SUT.
            Step 3: [RDPRFX] Send an smallest channel channelWidth X channelHeight to SUT.
            Step 4: [RDPRFX] Send one frame of Encode Data Messages to SUT.
            Step 5: [RDPBCGR] send Frame Maker Command (End) to SUT.
            Step 6: [RDPRFX] Expect SUT sends a TS_FRAME_ACKNOWLEDGE_PDU.
            */
            #endregion

            #region Test Sequence

            //Start RDP listening.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Starting RDP listening.");
            this.rdpbcgrAdapter.StartRDPListening(transportProtocol);

            #region Trigger client to connect
            //Trigger client to connect. 
            this.TestSite.Log.Add(LogEntryKind.Comment, "Triggering SUT to initiate a RDP connection to server.");
            triggerClientRDPConnect(transportProtocol);
            #endregion

            #region RDPBCGR Connection

            //Waiting for the transport level connection request.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting the transport layer connection request.");
            this.rdpbcgrAdapter.ExpectTransportConnection(RDPSessionType.Normal);


            //Set Server Capability with RomoteFX codec supported.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Setting Server Capability.");
            setServerCapabilitiesWithRemoteFxSupported();

            //Waiting for the RDP connection sequence.
            this.TestSite.Log.Add(LogEntryKind.Comment, "Establishing RDP connection.");
            this.rdpbcgrAdapter.EstablishRDPConnection(selectedProtocol, enMethod, enLevel, true, false, rdpServerVersion);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Server Save Session Info PDU to SUT to notify user has logged on.");
            this.rdpbcgrAdapter.ServerSaveSessionInfo(LogonNotificationType.UserLoggedOn, ErrorNotificationType_Values.LOGON_FAILED_OTHER);

            #endregion

            //Initial the RDPRFX adapter context.
            rdprfxAdapter.Accept(this.rdpbcgrAdapter.ServerStack, this.rdpbcgrAdapter.SessionContext);

            TS_RFX_ICAP[] clientSupportedCaps;
            rdprfxAdapter.ReceiveAndCheckClientCapabilities(maxRequestSize, out clientSupportedCaps);

            uint frameId = 0; //The index of the sending frame.
            OperationalMode opMode = OperationalMode.ImageMode;
            EntropyAlgorithm enAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
            ushort destLeft = 0; //the left bound of the frame.
            ushort destTop = 0; //the top bound of the frame.

            //Set OperationalMode/EntropyAlgorithm to valid pair.
            if (clientSupportedCaps != null)
            {
                opMode = (OperationalMode)clientSupportedCaps[0].flags;
                enAlgorithm = (EntropyAlgorithm)clientSupportedCaps[0].entropyBits;
            }

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (Begin) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending one frame of encoded bitmap data to client. Operational Mode = {0}, Entropy Algorithm = {1}, destLeft = {2}, destTop = {3}.",
                opMode, enAlgorithm, destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment,
                "Sending the encode header messages in the order of TS_RFX_SYNC -> TS_RFX_CHANNELS -> TS_RFX_CODECVERSIONS -> TS_RFX_CONTEXT.");
            rdprfxAdapter.SendTsRfxSync();
            rdprfxAdapter.SendTsRfxChannels(channelWidth, channelHeight);
            rdprfxAdapter.SendTsRfxCodecVersions();
            rdprfxAdapter.SendTsRfxContext(opMode, enAlgorithm);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending the encode data messages to client.");
            rdprfxAdapter.SendTsRfxFrameBegin(0);
            rdprfxAdapter.SendTsRfxRegion();
            rdprfxAdapter.SendTsRfxTileSet(opMode, enAlgorithm, image_64X64);
            rdprfxAdapter.SendTsRfxFrameEnd();
            rdprfxAdapter.FlushEncodedData(destLeft, destTop);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Sending Frame Marker Command (End) with frameID: {0}.", frameId);
            rdpbcgrAdapter.SendFrameMarkerCommand(frameAction_Values.SURFACECMD_FRAMEACTION_END, frameId);

            this.TestSite.Log.Add(LogEntryKind.Comment, "Expecting TS_FRAME_ACKNOWLEDGE_PDU.");
            rdprfxAdapter.ExpectTsFrameAcknowledgePdu(frameId, waitTime);
            #endregion
        }
    }
}
