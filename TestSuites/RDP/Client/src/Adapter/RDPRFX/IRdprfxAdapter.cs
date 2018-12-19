// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdprfx;
using System.Drawing;

namespace Microsoft.Protocols.TestSuites.Rdprfx
{
    public interface IRdprfxAdapter:IAdapter
    {
        /// <summary>
        /// Wait for connection.
        /// </summary>
        void Accept();

        /// <summary>
        /// Accept an existing RDP session which established outside.
        /// </summary>
        /// <param name="rdpbcgrServer">RdpbcgrServer object.</param>
        /// <param name="serverContext">RdpbcgrServerSessionContext object.</param>
        void Accept(RdpbcgrServer rdpbcgrServer, RdpbcgrServerSessionContext serverContext);

        /// <summary>
        /// Method to receive, decode and check client connection type and  color depth.
        /// </summary>
        void ReceiveAndCheckClientCoreData();

        /// <summary>
        /// Method to receive, decode and check client capabilities.
        /// </summary>
        /// <param name="serverMaxRequestSize">The MaxRequestSize field of the server-to-client Multifragment Update Capability Set.</param>
        /// <param name="supportedRfxCaps">Output the TS_RFX_ICAP array supported by the client.</param>
        void ReceiveAndCheckClientCapabilities(uint serverMaxRequestSize, out TS_RFX_ICAP[] supportedRfxCaps);

        /// <summary>
        /// This method expect a TS_FRAME_ACKNOWLEDGE_PDU from client.
        /// </summary>
        /// <param name="expectedFrameId">The expected frame id.</param>
        /// <param name="ackTimeout">The time span to wait.</param>
        void ExpectTsFrameAcknowledgePdu(uint expectedFrameId, TimeSpan ackTimeout);

        /// <summary>
        /// Method to send one frame of encoded data message to client.
        /// </summary>
        /// <param name="image">The image to be sent.</param>
        /// <param name="opMode">Indicates the operational mode.</param>
        /// <param name="entropy">Indicates the entropy algorithm.</param>
        /// <param name="destLeft">Left bound of the frame.</param>
        /// <param name="destTop">Left bound of the frame.</param>
        void SendImageToClient(System.Drawing.Image image, OperationalMode opMode, EntropyAlgorithm entropy, ushort destLeft, ushort destTop);

        /// <summary>
        /// Method to send one image without encoding to client.
        /// </summary>
        /// <param name="image">The image to be sent.</param>
        /// <param name="destLeft">Left bound of the frame.</param>
        /// <param name="destTop">Left bound of the frame.</param>
        void SendImageToClientWithoutEncoding(System.Drawing.Image image, ushort destLeft, ushort destTop);

        /// <summary>
        /// Method to send all pending encoded data of a frame to RDP client.
        /// </summary>
        /// <param name="destLeft">Left bound of the frame.</param>
        /// <param name="destTop">Left bound of the frame.</param>
        /// <param name="width">The width of the frame.</param>
        /// <param name="height">The height of the frame.</param>
        void FlushEncodedData(ushort destLeft, ushort destTop, ushort width = RdprfxServer.TileSize, ushort height = RdprfxServer.TileSize);

        /// <summary>
        /// Method to convert all encoded data into byte array.
        /// </summary>
        /// <return> return RFX encoded data into byte array </return>
        byte[] GetEncodedData();

        /// <summary>
        /// Method to send TS_RFX_SYNC to client.
        /// </summary>
        void SendTsRfxSync();

        /// <summary>
        /// Method to send TS_RFX_CODEC_VERSIONS to client.
        /// </summary>
        void SendTsRfxCodecVersions();

        /// <summary>
        /// Method to send TS_RFX_CHANNELS to client.
        /// </summary>
        void SendTsRfxChannels();

        /// <summary>
        /// Method to send TS_RFX_CHANNELS to client.
        /// </summary>
        void SendTsRfxChannels(short width, short height);

        /// <summary>
        /// Method to send TS_RFX_CONTEXT to client.
        /// </summary>
        /// <param name="isImageMode">Indicates the operational mode.</param>
        /// <param name="entropy">Indicates the entropy algorithm.</param>
        void SendTsRfxContext(OperationalMode opMode, EntropyAlgorithm entropy);

        /// <summary>
        /// Method to send TS_RFX_FRAME_BEGIN to client.
        /// </summary>
        /// <param name="frameIdx">The frame index.</param>
        void SendTsRfxFrameBegin(uint frameIdx);

        /// <summary>
        /// Method to send TS_RFX_REGION to client.
        /// </summary>
        /// <param name="rects">Array of rects, if this parameter is null, will send a 64*64 rect </param>
        /// <param name="numRectsZero">A boolean variable to indicate whether the numRectsZero field of the TS_RFX_REGION is zero </param>
        void SendTsRfxRegion(Rectangle[] rects = null, bool numRectsZero = false);

        /// <summary>
        /// Method to send TS_RFX_TILESET to client.
        /// </summary>
        /// <param name="opMode">Indicates the operational mode.</param>
        /// <param name="entropy">Indicates the entropy algorithm.</param>
        /// <param name="tileImage">The image for a tile to be sent. The width and height must be less than or equals with 64.</param>
        /// <param name="codecQuantVals">Quant values array</param>
        /// <param name="quantIdxY">Index of Y component in Quant value array</param>
        /// <param name="quantIdxCb">Index of Cb component in Quant value array</param>
        /// <param name="quantIdxCr">Index of Cr component in Quant value array</param>
        void SendTsRfxTileSet(OperationalMode opMode, EntropyAlgorithm entropy, System.Drawing.Image tileImage,
            TS_RFX_CODEC_QUANT[] codecQuantVals = null, byte quantIdxY = 0, byte quantIdxCb = 0, byte quantIdxCr = 0);

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
        void SendTsRfxTileSet(OperationalMode opMode, EntropyAlgorithm entropy, Image[] tileImages, TILE_POSITION[] positions,
            TS_RFX_CODEC_QUANT[] codecQuantVals = null, byte[] quantIdxYs = null, byte[] quantIdxCbs = null, byte[] quantIdxCrs = null);

        /// <summary>
        /// Method to send TS_RFX_FRAME_END to client.
        /// </summary>
        void SendTsRfxFrameEnd();

        /// <summary>
        /// Set the type of current test.
        /// </summary>
        /// <param name="testType">The test type.</param>
        void SetTestType(RdprfxNegativeType testType);

        /// <summary>
        /// Method to check if the input pair of the operation mode and entropy algorithm is supported by the client.
        /// </summary>
        /// <param name="opMode">The operation mode.</param>
        /// <param name="entropy">The entropy algorithm.</param>
        /// <returns></returns>
        bool CheckIfClientSupports(OperationalMode opMode, EntropyAlgorithm entropy);
    }
}
