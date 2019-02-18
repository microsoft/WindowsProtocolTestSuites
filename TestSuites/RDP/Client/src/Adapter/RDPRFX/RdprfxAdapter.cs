// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;
using Microsoft.Protocols.TestSuites.Rdp;
using Microsoft.Protocols.TestSuites.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk;
using System.IO;

namespace Microsoft.Protocols.TestSuites.Rdprfx
{
    public partial class RdprfxAdapter : ManagedAdapterBase, IRdprfxAdapter
    {
        #region Variables
        private ITestSite site;
        private RdpbcgrServer rdpbcgrServerStack;

        private IRdpbcgrAdapter rdpbcgrAdapter;

        private RdpbcgrServerSessionContext rdpbcgrSessionContext;
        private Collection<ITsCapsSet> ConfirmCapabilitySets;
        private RdprfxServerDecoder rdprfxServerDecoder;
        private RdprfxServer rdprfxServer;

        //Abstract Data Model
        private OperationalMode admOperationMode = OperationalMode.ImageMode;
        private EntropyAlgorithm admEntropyAlgorithm = EntropyAlgorithm.CLW_ENTROPY_RLGR3;
        private uint admFrameIndex; //the last frame id used in TS_RFX_FRAME_BEGIN.
        private uint frameMakerFrameId; //the last frame id used in Frame Marker Command.

        private UInt16Class supportedColorDepths;
        private ByteClass networkConnectionType;

        private bool is_Client_Multifragment_Update_CapabilitySet_Received;
        private TS_MULTIFRAGMENTUPDATE_CAPABILITYSET client_Multifragment_Update_CapabilitySet;
        private uint s2cMaxRequestSize; //the MaxRequestSize field of the server-to-client Multifragment Update Capability Set.

        private bool is_Client_Large_Pointer_Capability_Set_Received;
        private TS_LARGE_POINTER_CAPABILITYSET client_Large_Pointer_Capability_Set;

        private bool is_Client_Revision2_Bitmap_Cache_Capability_Set_Received;
        private TS_BITMAPCACHE_CAPABILITYSET_REV2 client_Revision2_Bitmap_Cache_Capability_Set;

        private bool is_TS_FRAME_ACKNOWLEDGE_CAPABILITYSET_Received;
        private TS_FRAME_ACKNOWLEDGE_CAPABILITYSET clientTS_FRAME_ACKNOWLEDGE_CAPABILITYSET;

        private bool is_Client_Surface_Commands_Capability_Set_Received;
        private TS_SURFCMDS_CAPABILITYSET client_Surface_Commands_Capability_Set;
        private bool clientupportStreamSurfaceBits;

        private bool is_Client_Bitmap_Codecs_Capability_Set_Received;
        private TS_BITMAPCODECS_CAPABILITYSET client_Bitmap_Codecs_Capability_Set;
        private bool is_TS_RFX_CLNT_CAPS_CONTAINER_Received;
        private TS_RFX_CLNT_CAPS_CONTAINER client_RFX_Caps_Container;
        private byte remoteFXCodecID;

        private List<StackPacket> pduCache;
        private List<byte> pendingBuffer;
        private object syncLocker;

        private RdprfxNegativeType currentTestType;

        #endregion
        
        #region IAdapter Methods

        public override void Initialize(ITestSite testSite)
        {
            this.site = testSite;
            rdprfxServerDecoder = new RdprfxServerDecoder();
            rdprfxServer = new RdprfxServer();
            pduCache = new List<StackPacket>();
            pendingBuffer = new List<byte>();
            syncLocker = new object();
            currentTestType = RdprfxNegativeType.None;
        }

        public override void Reset()
        {
            base.Reset();
            pduCache.Clear();
            pendingBuffer.Clear();
            currentTestType = RdprfxNegativeType.None;
        }

        public new TestTools.ITestSite Site
        {
            get { return site; }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (this.rdpbcgrServerStack != null)
            {
                this.rdpbcgrServerStack.Dispose();
            }
        }

        #endregion

        #region IRdprfxAdapter Methods

        /// <summary>
        /// Wait for connection.
        /// </summary>
        public void Accept()
        {
            this.rdpbcgrAdapter = Site.GetAdapter<IRdpbcgrAdapter>();
            this.rdpbcgrServerStack = this.rdpbcgrAdapter.ServerStack;
            this.rdpbcgrSessionContext = this.rdpbcgrAdapter.SessionContext;
            this.rdpbcgrAdapter.TS_FRAME_ACKNOWLEDGE_PDUReceived += new TS_FRAME_ACKNOWLEDGE_PDUHandler(this.VerifyTS_FRAME_ACKNOWLEDGE_PDU);
        }

        /// <summary>
        /// Accept an existing RDP session which established outside.
        /// </summary>
        /// <param name="rdpbcgrServer">RdpbcgrServer object.</param>
        /// <param name="serverContext">RdpbcgrServerSessionContext object.</param>
        public void Accept(RdpbcgrServer rdpbcgrServer, RdpbcgrServerSessionContext serverContext)
        {
            this.rdpbcgrAdapter = Site.GetAdapter<IRdpbcgrAdapter>();
            this.rdpbcgrServerStack = rdpbcgrServer;
            this.rdpbcgrSessionContext = serverContext;
            this.rdpbcgrAdapter.TS_FRAME_ACKNOWLEDGE_PDUReceived += new TS_FRAME_ACKNOWLEDGE_PDUHandler(this.VerifyTS_FRAME_ACKNOWLEDGE_PDU);
        }

        /// <summary>
        /// Method to receive, decode and check client connection type and  color depth.
        /// </summary>
        public void ReceiveAndCheckClientCoreData()
        {
            this.networkConnectionType = rdpbcgrSessionContext.ClientNetworkConnectionType;
            this.supportedColorDepths = rdpbcgrSessionContext.supportedColorDepths;
            VerifyColorDepths();
        }

        /// <summary>
        /// Method to receive and decode client capabilities.
        /// </summary>
        /// <param name="serverMaxRequestSize">The MaxRequestSize field of the server-to-client Multifragment Update Capability Set. </param>
        /// <param name="supportedRfxCaps">Output the TS_RFX_ICAP array supported by the client.</param>
        public void ReceiveAndCheckClientCapabilities(uint serverMaxRequestSize, out TS_RFX_ICAP[] supportedRfxCaps)
        {
            supportedRfxCaps = null;
            s2cMaxRequestSize = serverMaxRequestSize;
            ConfirmCapabilitySets =  this.rdpbcgrSessionContext.ConfirmCapabilitySets;
            foreach (ITsCapsSet capSet in ConfirmCapabilitySets)
            {
                if (capSet is TS_MULTIFRAGMENTUPDATE_CAPABILITYSET)
                {
                    this.is_Client_Multifragment_Update_CapabilitySet_Received = true;
                    this.client_Multifragment_Update_CapabilitySet = (TS_MULTIFRAGMENTUPDATE_CAPABILITYSET)capSet;
                }
                else if (capSet is TS_LARGE_POINTER_CAPABILITYSET)
                {
                    this.is_Client_Large_Pointer_Capability_Set_Received = true;
                    this.client_Large_Pointer_Capability_Set = (TS_LARGE_POINTER_CAPABILITYSET)capSet;
                }
                else if (capSet is TS_BITMAPCACHE_CAPABILITYSET_REV2)
                {
                    this.is_Client_Revision2_Bitmap_Cache_Capability_Set_Received = true;
                    this.client_Revision2_Bitmap_Cache_Capability_Set = (TS_BITMAPCACHE_CAPABILITYSET_REV2)capSet;
                }
                else if (capSet is TS_FRAME_ACKNOWLEDGE_CAPABILITYSET)
                {
                    this.is_TS_FRAME_ACKNOWLEDGE_CAPABILITYSET_Received = true;
                    this.clientTS_FRAME_ACKNOWLEDGE_CAPABILITYSET = (TS_FRAME_ACKNOWLEDGE_CAPABILITYSET)capSet;
                }
                else if (capSet is TS_SURFCMDS_CAPABILITYSET)
                {
                    this.is_Client_Surface_Commands_Capability_Set_Received = true;
                    this.client_Surface_Commands_Capability_Set = (TS_SURFCMDS_CAPABILITYSET)capSet;
                    if ((this.client_Surface_Commands_Capability_Set.cmdFlags & CmdFlags_Values.SURFCMDS_STREAMSURFACEBITS) == CmdFlags_Values.SURFCMDS_STREAMSURFACEBITS)
                    {
                        this.clientupportStreamSurfaceBits = true;
                    }
                }
                else if (capSet is TS_BITMAPCODECS_CAPABILITYSET)
                {
                    this.is_Client_Bitmap_Codecs_Capability_Set_Received = true;
                    this.client_Bitmap_Codecs_Capability_Set = (TS_BITMAPCODECS_CAPABILITYSET)capSet;
                    foreach (TS_BITMAPCODEC codec in this.client_Bitmap_Codecs_Capability_Set.supportedBitmapCodecs.bitmapCodecArray)
                    {
                        if (is_REMOTEFX_CODEC_GUID(codec.codecGUID))
                        {
                            is_TS_RFX_CLNT_CAPS_CONTAINER_Received = true;
                            remoteFXCodecID = codec.codecID;
                            this.client_RFX_Caps_Container =  rdprfxServerDecoder.Decode_TS_RFX_CLNT_CAPS_CONTAINER(codec.codecProperties);
                            supportedRfxCaps = this.client_RFX_Caps_Container.capsData.capsetsData[0].icapsData;
                            break;
                        }
                    }
                }
            }

            //Verify Client Capabilities
            VerifyClientCapabilities();
        }

        /// <summary>
        /// This method expect a TS_FRAME_ACKNOWLEDGE_PDU from client.
        /// </summary>
        /// <param name="expectedFrameId">The expected frame id.</param>
        /// <param name="ackTimeout">The time span to wait.</param>
        public void ExpectTsFrameAcknowledgePdu(uint expectedFrameId, TimeSpan ackTimeout)
        {
            this.frameMakerFrameId = expectedFrameId;
            if (this.rdpbcgrAdapter != null)
            {
                this.rdpbcgrAdapter.WaitForPacket<TS_FRAME_ACKNOWLEDGE_PDU>(ackTimeout);
            }
            else if (this.rdpbcgrServerStack != null && this.rdpbcgrSessionContext != null)
            {
                StackPacket receivedPdu = null;
                TS_FRAME_ACKNOWLEDGE_PDU ackPdu = null;
                bool isReceived = false;
                TimeSpan leftTime = ackTimeout;
                DateTime expiratedTime = DateTime.Now + ackTimeout;

                foreach (StackPacket pdu in pduCache)
                {
                    ackPdu = pdu as TS_FRAME_ACKNOWLEDGE_PDU;
                    if (ackPdu != null)
                    {
                        isReceived = true;
                        pduCache.Remove(pdu);
                        break;
                    }
                }

                while (!isReceived && leftTime.CompareTo(new TimeSpan(0)) > 0)
                {
                    try
                    {
                        receivedPdu = this.rdpbcgrServerStack.ExpectPdu(this.rdpbcgrSessionContext, leftTime);
                        ackPdu = receivedPdu as TS_FRAME_ACKNOWLEDGE_PDU;
                        if (ackPdu != null)
                        {
                            isReceived = true;
                            break;
                        }
                        else
                        {
                            Site.Log.Add(LogEntryKind.Debug, "Received and cached Pdu: {0}.", receivedPdu.GetType());
                            pduCache.Add(receivedPdu);
                        }
                    }
                    catch (TimeoutException)
                    {
                        Site.Assert.Fail("Timeout when expecting {0}", typeof(TS_FRAME_ACKNOWLEDGE_PDU));
                    }
                    catch (InvalidOperationException ex)
                    {
                        //break;
                        Site.Log.Add(LogEntryKind.Warning, "Exception thrown out when receiving client PDUs {0}.", ex.Message);
                    }
                    finally
                    {
                        System.Threading.Thread.Sleep(100);//Wait some time for next packet.
                        leftTime = expiratedTime - DateTime.Now;
                    }
                }
                if (isReceived)
                {
                    this.VerifyTS_FRAME_ACKNOWLEDGE_PDU(ackPdu);
                }
                else
                {
                    site.Assert.Fail("Timeout when expecting {0}.", typeof(TS_FRAME_ACKNOWLEDGE_PDU));
                }
            }
        }

        #region Encode Header Messages

        /// <summary>
        /// Method to send TS_RFX_SYNC to client.
        /// </summary>
        public void SendTsRfxSync()
        {
            TS_RFX_SYNC rfxSync = rdprfxServer.CreateTsRfxSync();
            if (currentTestType == RdprfxNegativeType.UnspecifiedBlockType)
            {
                rfxSync.BlockT.blockType = blockType_Value.InvalidType;
            }
            else if (currentTestType == RdprfxNegativeType.TsRfxSync_InvalidBlockLen)
            {
                rfxSync.BlockT.blockLen = rfxSync.BlockT.blockLen - 1; //invalid block length
            }
            else if (currentTestType == RdprfxNegativeType.TsRfxSync_InvalidMagic)
            {
                rfxSync.magic = 0xBBBBBBBB; //invalid value other than 0xCACCACCA
            }
            else if (currentTestType == RdprfxNegativeType.TsRfxSync_InvalidVersion)
            {
                rfxSync.version = 0x0000; //invalid value other than 0x0100
            }

            AddToPendingList(rfxSync);
        }

        /// <summary>
        /// Method to send TS_RFX_CODEC_VERSIONS to client.
        /// </summary>
        public void SendTsRfxCodecVersions() 
        {
            TS_RFX_CODEC_VERSIONS rfxVersions = rdprfxServer.CreateTsRfxCodecVersions();
            if (currentTestType == RdprfxNegativeType.TsRfxCodecVersions_InvalidCodecId)
            {
                rfxVersions.codecs.codecId = 0x00; //invalid value other than 0x01
            }
            else if (currentTestType == RdprfxNegativeType.TsRfxCodecVersions_InvalidVersion)
            {
                rfxVersions.codecs.version = 0x0000; //invalid value other than 0x0100;
            }


            AddToPendingList(rfxVersions);
        }

        /// <summary>
        /// Method to send TS_RFX_CHANNELS to client.
        /// </summary>
        public void SendTsRfxChannels() 
        {
            TS_RFX_CHANNELS rfxChannels = rdprfxServer.CreateTsRfxChannels();
            if (this.currentTestType == RdprfxNegativeType.TsRfxChannelT_InvalidWidth_TooSmall)
            {
                rfxChannels.channels[0].width = 0; //set to an invalid value which less than 1.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxChannelT_InvalidWidth_TooBig)
            {
                rfxChannels.channels[0].width = 4097; //set to an invalid value which greater than 4096.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxChannelT_InvalidHeight_TooSmall)
            {
                rfxChannels.channels[0].height = 0; //set to an invalid value which less than 1.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxChannelT_InvalidHeight_TooBig)
            {
                rfxChannels.channels[0].height = 2049; //set to an invalid value which greater than 2048.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxChannels_InvalidChannelId)
            {
                rfxChannels.channels[0].channelId = 0x01; //set to an invalid value other than 0x00.
            }

            AddToPendingList(rfxChannels);
        }

        /// <summary>
        /// Method to send TS_RFX_CHANNELS to client.
        /// </summary>
        /// <param name="width">The width of the channel</param>
        /// <param name="height">The height of the channel</param>
        public void SendTsRfxChannels(short width, short height)
        {
            TS_RFX_CHANNELS rfxChannels = rdprfxServer.CreateTsRfxChannels();
            rfxChannels.channels[0].width = width;
            rfxChannels.channels[0].height = height;

            AddToPendingList(rfxChannels);
        }

        /// <summary>
        /// Method to send TS_RFX_CONTEXT to client.
        /// </summary>
        /// <param name="isImageMode">Indicates the operational mode.</param>
        /// <param name="entropy">Indicates the entropy algorithm.</param>
        public void SendTsRfxContext(OperationalMode opMode, EntropyAlgorithm entropy)
        {
            this.admEntropyAlgorithm = entropy;
            this.admOperationMode = opMode;

            TS_RFX_CONTEXT rfxContext = rdprfxServer.CreateTsRfxContext(opMode, entropy);
            if (this.currentTestType == RdprfxNegativeType.TsRfxContext_InvalidCtxId)
            {
                rfxContext.ctxId = 0x01; //set to an invalid value other than 0x00.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxContext_InvalidTileSize)
            {
                rfxContext.tileSize = 0x0080; //set to an invalid value other than 0x0040.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxContext_InvalidCct)
            {
                rfxContext.properties &= 0xFFF7; //set "cct" to an invalid value: 0x0.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxContext_InvalidXft)
            {
                rfxContext.properties &= 0xFE1F; //set "xft" to an invalid value: 0x0.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxContext_InvalidQt)
            {
                rfxContext.properties &= 0x9FFF; //set "qt" to an invalid value: 0x0.
            }

            AddToPendingList(rfxContext);
        }

        #endregion

        #region Encode Data Messages

        /// <summary>
        /// Method to send TS_RFX_FRAME_BEGIN to client.
        /// </summary>
        /// <param name="frameIdx">The frame index.</param>
        public void SendTsRfxFrameBegin(uint frameIdx) 
        {
            this.admFrameIndex = frameIdx;
            TS_RFX_FRAME_BEGIN rfxBegin = rdprfxServer.CreateTsRfxFrameBegin(frameIdx);
            if (this.currentTestType == RdprfxNegativeType.TsRfxFrameBegin_InvalidBlockLen)
            {
                rfxBegin.CodecChannelT.blockLen -= 1;//Set to invalid block len which less than the actual.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxCodecChannelT_InvalidCodecId)
            {
                rfxBegin.CodecChannelT.codecId = 0x00; //set to an invalid value other than 0x01.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxCodecChannelT_InvalidChannelId)
            {
                rfxBegin.CodecChannelT.channelId = 0x01; //set to an invalid value other than 0x00.
            }

            AddToPendingList(rfxBegin);
        }

        /// <summary>
        /// Method to send TS_RFX_REGION to client.
        /// </summary> 
        /// <param name="rects">Array of rects, if this parameter is null, will send a 64*64 rect </param>
        /// <param name="numRectsZero">A boolean varialbe to indicate whether the numRectsZero field of TS_RFX_REGION is zero</param>
        public void SendTsRfxRegion(Rectangle[] rects = null, bool numRectsZero = false) 
        {
            TS_RFX_REGION rfxRegion = rdprfxServer.CreateTsRfxRegion(rects, numRectsZero);
            if (this.currentTestType == RdprfxNegativeType.TsRfxRegion_InvalidRegionFlags)
            {
                rfxRegion.regionFlags = 0x00; //set to an invalid value other than 0x01.
            }
            else if(this.currentTestType == RdprfxNegativeType.TsRfxRegion_InvalidRegionType)
            {
                rfxRegion.regionType = 0xBBBB; //set to an invalid value other than 0xCAC1.
            }

            if (this.rdpbcgrAdapter.SimulatedScreen != null)
            {
                this.rdpbcgrAdapter.SimulatedScreen.SetRemoteFXRegion(rfxRegion);
            }

            AddToPendingList(rfxRegion);
        }

        /// <summary>
        /// Method to send TS_RFX_TILESET to client.
        /// </summary>
        /// <param name="isImageMode">Indicates the operational mode.</param>
        /// <param name="entropy">Indicates the entropy algorithm.</param>
        /// <param name="tileImage">The image for a tile. The width and height must be less than or equals with 64.</param>
        /// <param name="codecQuantVals">Quant values array</param>
        /// <param name="quantIdxY">Index of Y component in Quant value array</param>
        /// <param name="quantIdxCb">Index of Cb component in Quant value array</param>
        /// <param name="quantIdxCr">Index of Cr component in Quant value array</param>
        public void SendTsRfxTileSet(OperationalMode opMode, EntropyAlgorithm entropy, Image tileImage,
            TS_RFX_CODEC_QUANT[] codecQuantVals = null, byte quantIdxY = 0, byte quantIdxCb = 0, byte quantIdxCr = 0)
        {
            this.admEntropyAlgorithm = entropy;
            this.admOperationMode = opMode;
            TS_RFX_TILESET rfxTileSet = rdprfxServer.CreateTsRfxTileSet(opMode, entropy, tileImage, codecQuantVals, quantIdxY, quantIdxCb, quantIdxCr);
            if (this.currentTestType == RdprfxNegativeType.TsRfxTileSet_InvalidIdx)
            {
                rfxTileSet.idx = 0x0001; //set to an invalid value other than 0x0000.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxTileSet_InvalidLt)
            {
                rfxTileSet.properties &= 0xFFFE; //set "lt" to an invalid value: 0x0.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxTileSet_InvalidCct)
            {
                rfxTileSet.properties &= 0xFFCF; //set "cct" to an invalid value: 0x0.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxTileSet_InvalidXft)
            {
                rfxTileSet.properties &= 0xFC3F; //set "xft" to an invalid value: 0x0.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxTileSet_InvalidQt)
            {
                rfxTileSet.properties &= 0x3FFF; //set "xft" to an invalid value: 0x0.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxTileSet_InvalidTileSize)
            {
                rfxTileSet.tileSize = 0x80; //set to an invalid value other than 0x40.
            }

            if (this.rdpbcgrAdapter.SimulatedScreen != null)
            {
                this.rdpbcgrAdapter.SimulatedScreen.SetRemoteFXTileSet(rfxTileSet, entropy);
            }

            AddToPendingList(rfxTileSet);
            if (!CheckIfClientSupports(opMode, entropy))
            {
                Site.Log.Add(LogEntryKind.Debug, "The client Cap is not supported: OperationalMode = {0}, EntropyAlgorithm = {1}",
                    opMode.ToString(),
                    entropy.ToString());
            }
        }

        /// <summary>
        /// Method to send TS_RFX_TILESET to client.
        /// </summary>
        /// <param name="opMode">Indicates the operational mode.</param>
        /// <param name="entropy">Indicates the entropy algorithm.</param>
        /// <param name="tileImages">The image array for tiles to be sent. The width and height must be less than or equals with 64.</param>
        /// <param name="positions">A TILE_POSITION array indicating the positions of each tile images</param>
        /// <param name="codecQuantVals">Quant values array</param>
        /// <param name="quantIdxYs">Index array of Y component in Quant value array</param>
        /// <param name="quantIdxCbs">Index array of Cb component in Quant value array</param>
        /// <param name="quantIdxCrs">Index array of Cr component in Quant value array</param>
        public void SendTsRfxTileSet(OperationalMode opMode, EntropyAlgorithm entropy, Image[] tileImages, TILE_POSITION[] positions,
            TS_RFX_CODEC_QUANT[] codecQuantVals = null, byte[] quantIdxYs = null, byte[] quantIdxCbs = null, byte[] quantIdxCrs = null)
        {
            this.admEntropyAlgorithm = entropy;
            this.admOperationMode = opMode;
            TS_RFX_TILESET rfxTileSet = rdprfxServer.CreateTsRfxTileSet(opMode, entropy, tileImages, positions, codecQuantVals, quantIdxYs, quantIdxCbs, quantIdxCrs);
            if (this.currentTestType == RdprfxNegativeType.TsRfxTileSet_InvalidIdx)
            {
                rfxTileSet.idx = 0x0001; //set to an invalid value other than 0x0000.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxTileSet_InvalidLt)
            {
                rfxTileSet.properties &= 0xFFFE; //set "lt" to an invalid value: 0x0.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxTileSet_InvalidCct)
            {
                rfxTileSet.properties &= 0xFFCF; //set "cct" to an invalid value: 0x0.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxTileSet_InvalidXft)
            {
                rfxTileSet.properties &= 0xFC3F; //set "xft" to an invalid value: 0x0.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxTileSet_InvalidQt)
            {
                rfxTileSet.properties &= 0x3FFF; //set "xft" to an invalid value: 0x0.
            }
            else if (this.currentTestType == RdprfxNegativeType.TsRfxTileSet_InvalidTileSize)
            {
                rfxTileSet.tileSize = 0x80; //set to an invalid value other than 0x40.
            }

            if (this.rdpbcgrAdapter.SimulatedScreen != null)
            {
                this.rdpbcgrAdapter.SimulatedScreen.SetRemoteFXTileSet(rfxTileSet, entropy);
            }

            AddToPendingList(rfxTileSet);
            if (!CheckIfClientSupports(opMode, entropy))
            {
                Site.Log.Add(LogEntryKind.Debug, "The client Cap is not supported: OperationalMode = {0}, EntropyAlgorithm = {1}",
                    opMode.ToString(),
                    entropy.ToString());
            }
        }

        /// <summary>
        /// Method to send TS_RFX_FRAME_END to client.
        /// </summary>
        public void SendTsRfxFrameEnd() 
        {
            TS_RFX_FRAME_END rfxEnd = rdprfxServer.CreateTsRfxFrameEnd();
            AddToPendingList(rfxEnd);
        }

        #endregion

        /// <summary>
        /// Method to send one frame of encoded data message to client.
        /// </summary>
        /// <param name="image">The image to be sent.</param>
        /// <param name="opMode">Indicates the operational mode.</param>
        /// <param name="entropy">Indicates the entropy algorithm.</param>
        /// <param name="destLeft">Left bound of the frame.</param>
        /// <param name="destTop">Left bound of the frame.</param>
        public void SendImageToClient(System.Drawing.Image image, OperationalMode opMode, EntropyAlgorithm entropy, ushort destLeft, ushort destTop)
        {
            if (image == null)
            {
                Site.Log.Add(LogEntryKind.Debug, "[In iRdprfxAdapter.SendImageToClient Method] The image to be send is null.");
                return;
            }

            TileImage[] tileImageArr = RdprfxTileUtils.SplitToTileImage(image, RdprfxServer.TileSize, RdprfxServer.TileSize);

            for (int idx = 0; idx < tileImageArr.Length; idx++)
            {
                if (idx == 0 || opMode == OperationalMode.ImageMode)
                {
                    SendTsRfxSync();
                    SendTsRfxCodecVersions();
                    SendTsRfxChannels();
                    SendTsRfxContext(opMode, entropy);
                }
                SendTsRfxFrameBegin((uint)idx);
                SendTsRfxRegion();
                SendTsRfxTileSet(opMode, entropy, tileImageArr[idx].image);
                SendTsRfxFrameEnd();
                FlushEncodedData((ushort)(destLeft + tileImageArr[idx].x), (ushort)(destTop + tileImageArr[idx].y));
                if (currentTestType != RdprfxNegativeType.None)
                {
                    // Only send one message if it is in a negative test case.
                    break;
                }
            }
        }

        /// <summary>
        /// Method to send one frame of unencoded data message to client.
        /// </summary>
        /// <param name="image">The image to be sent.</param>
        /// <param name="opMode">Indicates the operational mode.</param>
        /// <param name="entropy">Indicates the entropy algorithm.</param>
        /// <param name="destLeft">Left bound of the frame.</param>
        /// <param name="destTop">Left bound of the frame.</param>
        public void SendImageToClientWithoutEncoding(System.Drawing.Image image, ushort destLeft, ushort destTop)
        {
            if (image == null)
            {
                Site.Log.Add(LogEntryKind.Debug, "[In iRdprfxAdapter.SendImageToClient Method] The image to be send is null.");
                return;
            }

            TS_SURFCMD_STREAM_SURF_BITS surfStreamCmd = Create_TS_SURFCMD_STREAM_SURF_BITS(TSBitmapDataExFlags_Values.None, 0);
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
            Byte[] byteImage = memoryStream.ToArray();

            // remove the BITMAP header data at the beginning
            int rgbLength = RgbTile.TileSize * RgbTile.TileSize * 4;
            Byte[] rgbArray = new Byte[rgbLength];
            int delta = byteImage.Length - rgbLength;
            for (int i = 0; i < rgbLength; i++)
            {
                rgbArray[i] = byteImage[delta + i];
            }

            surfStreamCmd.bitmapData.bitmapDataLength = (uint)rgbArray.Length;
            surfStreamCmd.bitmapData.bitmapData = rgbArray;

            surfStreamCmd.destLeft = destLeft;
            surfStreamCmd.destTop = destTop;
            checked
            {
                surfStreamCmd.destBottom = (ushort)(destTop + image.Width);
                surfStreamCmd.destRight = (ushort)(destLeft + image.Height);
            }
            surfStreamCmd.bitmapData.width = (ushort)image.Width;
            surfStreamCmd.bitmapData.height = (ushort)image.Height;
            SendSurfaceCmd_StreamSurfBits(surfStreamCmd);

            if (this.rdpbcgrAdapter.SimulatedScreen != null)
            {
                this.rdpbcgrAdapter.SimulatedScreen.RenderUncompressedImage(image, destLeft, destTop);
            }
        }

        /// <summary>
        /// Set the type of current test.
        /// </summary>
        /// <param name="testType">The test type.</param>
        public void SetTestType(RdprfxNegativeType testType)
        {
            currentTestType = testType;
        }

        /// <summary>
        /// Method to send all pending encoded data of a frame to RDP client.
        /// </summary>
        /// <param name="destLeft">Left bound of the frame.</param>
        /// <param name="destTop">Left bound of the frame.</param>
        /// <param name="width">The width of the frame.</param>
        /// <param name="height">The height of the frame.</param>
        public void FlushEncodedData(ushort destLeft, ushort destTop, ushort width = RdprfxServer.TileSize, ushort height = RdprfxServer.TileSize)
        {
            lock (syncLocker)
            {
                if (pendingBuffer.Count > 0)
                {
                    LogServerADMInfo();

                    TS_SURFCMD_STREAM_SURF_BITS surfStreamCmd = Create_TS_SURFCMD_STREAM_SURF_BITS(TSBitmapDataExFlags_Values.None, remoteFXCodecID);
                    surfStreamCmd.bitmapData.bitmapDataLength = (uint)(pendingBuffer.Count);
                    surfStreamCmd.bitmapData.bitmapData = pendingBuffer.ToArray();
                    pendingBuffer.Clear();

                    surfStreamCmd.destLeft = destLeft;
                    surfStreamCmd.destTop = destTop;
                    checked
                    {
                        surfStreamCmd.destBottom = (ushort)(destTop + height);
                        surfStreamCmd.destRight = (ushort)(destLeft + width);
                    }
                    surfStreamCmd.bitmapData.width = width;
                    surfStreamCmd.bitmapData.height = height;
                    SendSurfaceCmd_StreamSurfBits(surfStreamCmd);

                } 
            }

            if (this.rdpbcgrAdapter.SimulatedScreen != null)
            {
                this.rdpbcgrAdapter.SimulatedScreen.RenderRemoteFXTile(destLeft, destTop);
            }
        }

        /// <summary>
        /// Method to convert all encoded data into byte array.
        /// </summary>
        /// <return> return RFX encoded data into byte array </return>
        public byte[] GetEncodedData()
        {
            lock (syncLocker)
            {
               return pendingBuffer.ToArray();
            }
        }

        /// <summary>
        /// Method to check if the input pair of the operation mode and entropy algorithm is supported by the client.
        /// </summary>
        /// <param name="opMode">The operation mode.</param>
        /// <param name="entropy">The entropy algorithm.</param>
        /// <returns></returns>
        public bool CheckIfClientSupports(OperationalMode opMode, EntropyAlgorithm entropy)
        {
            TS_RFX_ICAP[] iCaps = this.client_RFX_Caps_Container.capsData.capsetsData[0].icapsData;
            foreach (TS_RFX_ICAP icap in iCaps)
            {
                if ((icap.flags & (byte)OperationalMode.ImageMode) == (byte)0 &&
                    ((byte)icap.entropyBits == (byte)entropy))
                {
                    //OperationalMode.ImageMode is not set, both the image mode and the video mode of the codec are supported
                    return true;
                }
                else if ((byte)icap.entropyBits == (byte)entropy)
                {
                    //OperationalMode.ImageMode is set, only image mode is supported
                    if (opMode == OperationalMode.ImageMode)
                        return true;
                }
                
            }
            return false;
        }
        #endregion

        #region private methods

        /// <summary>
        /// Method to send RDPRFX raw data.
        /// </summary>
        /// <param name="surfStreamCmd">TS_SURFCMD_STREAM_SURF_BITS to be send.</param>
        private void SendSurfaceCmd_StreamSurfBits(TS_SURFCMD_STREAM_SURF_BITS surfStreamCmd)
        {
            if (rdpbcgrAdapter != null)
            {
                rdpbcgrAdapter.SendStreamSurfaceBitsCommand(surfStreamCmd);
            }
            else if (this.rdpbcgrServerStack != null && this.rdpbcgrSessionContext != null)
            {
                //TS_FRAME_MARKER frameMakerCmdBegin = new TS_FRAME_MARKER();
                //frameMakerCmdBegin.cmdType = cmdType_Values.CMDTYPE_FRAME_MARKER;
                //frameMakerCmdBegin.frameAction = frameAction_Values.SURFACECMD_FRAMEACTION_BEGIN;
                //frameMakerCmdBegin.frameId = frameIndex;

                //TS_FRAME_MARKER frameMakerCmdEnd = new TS_FRAME_MARKER();
                //frameMakerCmdEnd.cmdType = cmdType_Values.CMDTYPE_FRAME_MARKER;
                //frameMakerCmdEnd.frameAction = frameAction_Values.SURFACECMD_FRAMEACTION_END;
                //frameMakerCmdEnd.frameId = frameIndex++;

                TS_FP_SURFCMDS surfCmds = new TS_FP_SURFCMDS();
                surfCmds.updateHeader = (byte)(((int)updateCode_Values.FASTPATH_UPDATETYPE_SURFCMDS & 0x0f)
                | (((int)fragmentation_Value.FASTPATH_FRAGMENT_SINGLE) << 4)
                | ((int)compressedType_Values.None << 6));
                surfCmds.compressionFlags = compressedType_Values.None;
                int subLength = 8 + 8 + 22;
                if(surfStreamCmd.bitmapData.exBitmapDataHeader != null)
                {
                    subLength += 24;
                }
                surfCmds.size = (ushort)(subLength + surfStreamCmd.bitmapData.bitmapDataLength);//size of TS_SURFCMD_STREAM_SURF_BITS;
                surfCmds.surfaceCommands = new TS_SURFCMD[1];
                surfCmds.surfaceCommands[0] = surfStreamCmd;

                TS_FP_UPDATE_PDU fpOutput;
                TS_FP_UPDATE[] updates = new TS_FP_UPDATE[1];
                updates[0] = surfCmds;
                fpOutput = rdpbcgrServerStack.CreateFastPathUpdatePdu(rdpbcgrSessionContext, updates);
                rdpbcgrServerStack.SendPdu(rdpbcgrSessionContext, fpOutput);
            }
        }

        /// <summary>
        /// Send a Rdprfx message.
        /// </summary>
        /// <param name="rfxMessage">The Rdprfx message to be sent.</param>
        private void AddToPendingList(IMarshalable rfxMessage)
        {
            lock (syncLocker)
            {
                byte[] data = rfxMessage.ToBytes();
                pendingBuffer.AddRange(data);
            }
        }

        private bool is_REMOTEFX_CODEC_GUID(TS_BITMAPCODEC_GUID guidObj)
        {
            //CODEC_GUID_REMOTEFX
            //0x76772F12 BD72 4463 AF B3 B7 3C 9C 6F 78 86
            bool rtnValue;
            rtnValue = (guidObj.codecGUID1 == 0x76772F12) &&
                (guidObj.codecGUID2 == 0xBD72) &&
                (guidObj.codecGUID3 == 0x4463) &&
                (guidObj.codecGUID4 == 0xAF) &&
                (guidObj.codecGUID5 == 0xB3) &&
                (guidObj.codecGUID6 == 0xB7) &&
                (guidObj.codecGUID7 == 0x3C) &&
                (guidObj.codecGUID8 == 0x9C) &&
                (guidObj.codecGUID9 == 0x6F) &&
                (guidObj.codecGUID10 == 0x78) &&
                (guidObj.codecGUID11== 0x86);

            return rtnValue;
        }
        // TODO: alignment need captures to verifty flags = TSBitmapDataExFlags_Values.EX_COMPRESSED_BITMAP_HEADER_PRESENT
        private TS_SURFCMD_STREAM_SURF_BITS Create_TS_SURFCMD_STREAM_SURF_BITS(TSBitmapDataExFlags_Values flags, byte codecId)
        {
            TS_SURFCMD_STREAM_SURF_BITS surfStreamCmd = new TS_SURFCMD_STREAM_SURF_BITS();
            surfStreamCmd.cmdType = cmdType_Values.CMDTYPE_STREAM_SURFACE_BITS;
            surfStreamCmd.destLeft = 0;
            surfStreamCmd.destTop = 0;
            surfStreamCmd.destRight = 0;
            surfStreamCmd.destBottom = 0;
            surfStreamCmd.bitmapData = Create_TS_BITMAP_DATA_EX(flags, codecId);
            return surfStreamCmd;
        }

        private TS_BITMAP_DATA_EX Create_TS_BITMAP_DATA_EX(TSBitmapDataExFlags_Values flags, byte codecId)
        {
            TS_BITMAP_DATA_EX tsBitmapDataEx = new TS_BITMAP_DATA_EX();
            tsBitmapDataEx.bpp = 32; // Hard code
            tsBitmapDataEx.flags = flags;
            tsBitmapDataEx.reserved = 0; // It Must be set to zero.
            tsBitmapDataEx.codecID = codecId;
            tsBitmapDataEx.width = 0;
            tsBitmapDataEx.height = 0;
            // bitmapDataLength and bitmapData was handled in call method. 
            if(flags.HasFlag(TSBitmapDataExFlags_Values.EX_COMPRESSED_BITMAP_HEADER_PRESENT))
            {
                tsBitmapDataEx.exBitmapDataHeader = Create_TS_COMPRESSED_BITMAP_HEADER_EX();
            }
            return tsBitmapDataEx;
        }

        private TS_COMPRESSED_BITMAP_HEADER_EX Create_TS_COMPRESSED_BITMAP_HEADER_EX()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            TS_COMPRESSED_BITMAP_HEADER_EX tsCompressedBitmapHeaderEx = new TS_COMPRESSED_BITMAP_HEADER_EX();
            tsCompressedBitmapHeaderEx.highUniqueId = (uint)rnd.Next();
            tsCompressedBitmapHeaderEx.lowUniqueId = (uint)rnd.Next();
            ulong creatTime = (ulong)DateTime.UtcNow.ToUniversalTime().Subtract(new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)).TotalMilliseconds;
            tsCompressedBitmapHeaderEx.tmMilliseconds = creatTime % 1000;
            tsCompressedBitmapHeaderEx.tmSeconds = creatTime / 1000;
            return tsCompressedBitmapHeaderEx;
        }

        private void LogServerADMInfo()
        {
            Site.Log.Add(LogEntryKind.Debug, "Sending encoded bitmap to client with the settings: OperationalMode = {0}, EntropyAlgorithm = {1}, FrameId = {2}",
                this.admOperationMode, this.admEntropyAlgorithm, this.admFrameIndex);
        }

        #endregion

    }
}
