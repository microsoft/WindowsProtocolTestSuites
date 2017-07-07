// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpevor;
using Microsoft.Protocols.TestSuites.Rdpbcgr;

namespace Microsoft.Protocols.TestSuites.Rdpevor
{
    /// <summary>
    /// The protocol adapter of MS-RDPEVOR client test suite.
    /// </summary>
    public interface IRdpevorAdapter: IAdapter
    {

        /// <summary>
        /// Initialize this protocol with create control and data channels.
        /// </summary>
        /// <param name="rdpedycServer">RDPEDYC Server instance</param>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <returns>true if client supports this protocol; otherwise, return false.</returns>
        bool ProtocolInitialize(RdpedycServer rdpedycServer, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP);
      
        /// <summary>
        /// Method to send a TSMM_PRESENTATION_REQUEST to client.
        /// </summary>
        /// <param name="presentationId">A number that uniquely identifies the video stream on the server.</param>
        /// <param name="command">A number that identifies which operation the client is to perform. </param>
        /// <param name="srcWidth">This is the width of the video stream after scaling back to the original resolution.</param>
        /// <param name="srcHeight">This is the height of the video stream after scaling back to the original resolution.</param>
        /// <param name="scaledWidth">This is the width of the video stream.</param>
        /// <param name="scaledHeight">This is the height of the video stream.</param>
        /// <param name="geometryId">This field is used to correlate this video data with its geometry.</param>
        /// <param name="videoType">This field identifies the Media Foundation video subtype of the video stream.</param>
        void SendPresentationRequest(
            byte presentationId, 
            CommandValues command, 
            uint srcWidth,
            uint srcHeight,
            uint scaledWidth,
            uint scaledHeight,
            UInt64 geometryId,
            VideoSubtype videoType,
            byte[] extraData);

        /// <summary>
        /// Method to send a TSMM_VIDEO_DATA to client.
        /// </summary>
        /// <param name="presentationId">This is the same number as the PresentationId field in the TSMM_PRESENTATION_REQUEST message.</param>
        /// <param name="flags">The bits of this integer indicate attributes of this message. </param>
        /// <param name="packetIndex">This field contains the index of the current packet within the larger sample. </param>
        /// <param name="totalPacketsInSample">This field contains the number of packets that make up the current sample.</param>
        /// <param name="SampleNumber">This field contains the index of current sample in current presentation.</param>
        /// <param name="packetData">The video data in bytes which to be sent.</param>
        /// <param name="isCompressed">Whether to compress the data before sent.</param>
        void SendVideoPacket(byte presentationId, TsmmVideoData_FlagsValues flags, ushort packetIndex, ushort totalPacketsInSample, uint SampleNumber, byte[] packetData, ulong timeStamp, bool isCompressed =false);

        /// <summary>
        /// Method to sent a sample to client.
        /// </summary>
        /// <param name="presentationId">This is the same number as the PresentationId field in the TSMM_PRESENTATION_REQUEST message.</param>
        /// <param name="isKeyFrame">Is the sample a key frame.</param>
        /// <param name="SampleNumber">The number of sample in the video stream.</param>
        /// <param name="sampleData">The sample data in bytes which to be sent.</param>
        /// <param name="isCompressed">Whether the packet will be compressed before sent.</param>
        void SendVideoSample(byte presentationId, bool isKeyFrame, uint SampleNumber, byte[] sampleData, ulong timeStamp, bool isCompressed =false);
             
        /// <summary>
        /// Method to expect a TSMM_PRESENTATION_RESPONSE from client.
        /// </summary>
        /// <param name="presentationId">The expected presentation Id.</param>
        void ExpectPresentationiResponse(byte presentationId);

        /// <summary>
        /// Method to expect a TSMM_CLIENT_NOTIFICATION from client.
        /// </summary>
        /// <param name="presentationId">The expected presentation Id.</param>
        /// <param name="notificationType">The expected notification type</param>
        void ExpectClientNotification(byte presentationId, NotificationTypeValues notificationType);

        /// <summary>
        /// Set the test type to positive or negative test types.
        /// </summary>
        /// <param name="negType">The type of current test. None means positive test, others are negative tests.</param>
        void SetTestType(RdpevorNegativeType negType);
    }
}
