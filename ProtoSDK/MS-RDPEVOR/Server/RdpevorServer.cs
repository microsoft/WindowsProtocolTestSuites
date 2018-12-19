// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpevor
{
    public class RdpevorServer
    {

        #region Varibles

        const int sizeOfVideoPacketFixedFields = 41; //The total size in bytes of TSMM_VIDEO_DATA fixed fields is 41.
        const int sizeOfPresentationFixedFields = 69; //The total size in bytes of TSMM_PRESENTATION_REQUEST fixed fields is 69.
        const byte FrameRate = 0x1D; //reuse the data by Windows. 
        const ushort AverageBitrateKbps = 0x12C0;//reuse the data by Windows.
        ulong lastPacketTimestamp;

        private RdpedycServer rdpedycServer;
        const string RdpevorControlChannelName = "Microsoft::Windows::RDS::Video::Control::v08.01";
        const string RdpevorDataChannelName = "Microsoft::Windows::RDS::Video::Data::v08.01";

        DynamicVirtualChannel rdpevorControlDVC;
        DynamicVirtualChannel rdpevorDataDVC;

        List<RdpevorPdu> receivedList;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rdpedycServer"></param>
        public RdpevorServer(RdpedycServer rdpedycServer)
        {
            this.rdpedycServer = rdpedycServer;
            receivedList = new List<RdpevorPdu>();
        }

        #endregion Constructor

        /// <summary>
        /// Create dynamic virtual channel.
        /// </summary>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <param name="timeout">Timeout</param>
        /// <returns>true if client supports this protocol; otherwise, return false.</returns>
        public bool CreateRdpevorDvc(TimeSpan timeout, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            const ushort priority = 0;
            
            rdpevorControlDVC = rdpedycServer.CreateChannel(timeout, priority, RdpevorControlChannelName, transportType, OnDataReceived);
            rdpevorDataDVC = rdpedycServer.CreateChannel(timeout, priority, RdpevorDataChannelName, transportType, OnDataReceived);
            
            if (rdpevorControlDVC != null && rdpevorDataDVC != null)
            {
                return true;
            }
            return false;
        }

        #region Send/Receive Methods

        /// <summary>
        /// Send a RDPEVOR packet through control channel
        /// </summary>
        /// <param name="evorPdu"></param>
        public void SendRdpevorControlPdu(RdpevorServerPdu evorPdu)
        {
            byte[] data = PduMarshaler.Marshal(evorPdu);
            if (rdpevorControlDVC == null)
            {
                throw new InvalidOperationException("Control DVC instance of RDPEVOR is null, Dynamic virtual channel must be created before sending data.");
            }
            rdpevorControlDVC.Send(data);
            
        }

        /// <summary>
        /// Send a RDPEVOR packet through data channel
        /// </summary>
        /// <param name="evorPdu">The pdu to send</param>
        /// <param name="isCompressed"> Whether to compress the pdu before send</param>
        public void SendRdpevorDataPdu(RdpevorServerPdu evorPdu, bool isCompressed )
        {
            byte[] data = PduMarshaler.Marshal(evorPdu);
                       
            if (rdpevorDataDVC == null)
            {
                throw new InvalidOperationException("Data DVC instance of RDPEVOR is null, Dynamic virtual channel must be created before sending data.");
            }
            rdpevorDataDVC.Send(data, isCompressed);            
        }

        public void SendDataCompressedDvcPdu(DataCompressedDvcPdu compressedPdu)
        {
            byte[] data = PduMarshaler.Marshal(compressedPdu);
            if (rdpevorDataDVC == null)
            {
                throw new InvalidOperationException("DataCompressedDvcPduinstance of RDPEVOR is null, Dynamic virtual channel must be created before sending data.");
            }
            rdpevorDataDVC.Send(data, true);

        }
        /// <summary>
        /// Method to expect a RdpevorPdu.
        /// </summary>
        /// <param name="timeout">Timeout</param>
        public T ExpectRdpevorPdu<T>(TimeSpan timeout) where T : RdpevorPdu
        {
            DateTime endTime = DateTime.Now + timeout;

            while (endTime > DateTime.Now)
            {
                if (receivedList.Count > 0)
                {
                    lock (receivedList)
                    {
                        foreach (RdpevorPdu pdu in receivedList)
                        {
                            T response = pdu as T;
                            if (response != null)
                            {
                                receivedList.Remove(pdu);
                                return response;
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(100);
            }
            return null;
        }

        #endregion Send/Receive Methods

        #region Create methods

        /// <summary>
        /// Method to create a TSMM_PRESENTATION_REQUEST packet.
        /// </summary>
        /// <param name="presentationId">A number that uniquely identifies the video stream on the server.</param>
        /// <param name="command">A number that identifies which operation the client is to perform. </param>
        /// <param name="srcWidth">This is the width of the video stream after scaling back to the original resolution.</param>
        /// <param name="srcHeight">This is the height of the video stream after scaling back to the original resolution.</param>
        /// <param name="scaledWidth">This is the width of the video stream.</param>
        /// <param name="scaledHeight">This is the height of the video stream.</param>
        /// <param name="geometryId">This field is used to correlate this video data with its geometry.</param>
        /// <param name="videoSubTypeId">This field identifies the Media Foundation video subtype of the video stream.</param>
        public TSMM_PRESENTATION_REQUEST CreatePresentationRequest(
            byte presentationId,
            CommandValues command,
            uint srcWidth,
            uint srcHeight,
            uint scaledWidth,
            uint scaledHeight,
            UInt64 geometryId,
            VideoSubtype videoSubType,
            byte[] extraData)
        {
            TSMM_PRESENTATION_REQUEST request = new TSMM_PRESENTATION_REQUEST();
            request.Header.PacketType = PacketTypeValues.TSMM_PACKET_TYPE_PRESENTATION_REQUEST;
            request.PresentatioinId = presentationId;
            request.Version = RdpevorVersionValues.RDP8;
            request.Command = command;

            //The InvalidCommand packet is just for negative test, all other fiels are set same as Start packet.
            if (command == CommandValues.Start || command == CommandValues.InvalidCommand)
            {
                request.FrameRate = FrameRate;
                request.AverageBitrateKbps = AverageBitrateKbps;
                request.Reserved = 0;
                request.SourceWidth = srcWidth;
                request.SourceHeight = srcHeight;
                request.ScaledWidth = srcWidth;
                request.ScaledHeight = srcHeight;
                request.hnsTimestampOffset = (ulong)DateTime.Now.ToFileTimeUtc();
                request.GeometryMappingId = geometryId;
                request.VideoSubtypeId = RdpevorTestData.GetH264VideoSubtypeId();
                if (videoSubType == VideoSubtype.MFVideoFormat_H264)
                {
                    request.VideoSubtypeId = RdpevorTestData.GetH264VideoSubtypeId();
                }
                request.pExtraData = extraData;
                request.cbExtra = (uint)extraData.Length;
                request.Reserved2 = 0;
                request.Header.cbSize = (uint)(sizeOfPresentationFixedFields + request.pExtraData.Length);
            }
            else if (command == CommandValues.Stop)
            {
                request.Header.cbSize = 0x45;

                //If the command is to stop the presentation, only the Header, PresentationId, Version, and Command fields are valid.
                //So set all other fields to 0.
                request.FrameRate = 0;
                request.AverageBitrateKbps = 0;
                request.Reserved = 0;
                request.SourceWidth = 0;
                request.SourceHeight = 0;
                request.ScaledWidth = 0;
                request.ScaledHeight = 0;
                request.hnsTimestampOffset = 0;
                request.GeometryMappingId = 0;
                request.VideoSubtypeId = new byte[16];
                request.cbExtra = 0;
                request.pExtraData = null;//TDI
                request.Reserved2 = 0;
            }
            return request;
        }

        /// <summary>
        /// Method to create a TSMM_VIDEO_DATA packet.
        /// </summary>
        /// <param name="presentationId">This is the same number as the PresentationId field in the TSMM_PRESENTATION_REQUEST message.</param>
        /// <param name="flags">The bits of this integer indicate attributes of this message. </param>
        /// <param name="packetIndex">This field contains the index of the current packet within the larger sample. </param>
        /// <param name="totalPacketsInSample">This field contains the number of packets that make up the current sample.</param>
        /// <param name="SampleNumber">The sample index in this presentation.</param>
        /// <param name="videoData">The video data to be sent.</param>
        public TSMM_VIDEO_DATA CreateVideoDataPacket(byte presentationId, TsmmVideoData_FlagsValues flags, ushort packetIndex, ushort totalPacketsInSample, uint SampleNumber, byte[] videoRawData, ulong timeStamp)
        {

            TSMM_VIDEO_DATA videoData = new TSMM_VIDEO_DATA();
            videoData.Header.PacketType = PacketTypeValues.TSMM_PACKET_TYPE_VIDEO_DATA;
            videoData.PresentatioinId = presentationId;
            videoData.Version = RdpevorVersionValues.RDP8;
            videoData.Flags = flags;
            videoData.Reserved = 0;
            if (packetIndex != totalPacketsInSample)
            {
                //HnsTimestamp and HnsDuration are only effective in the last packet of a sample.
                videoData.HnsTimestamp = 0;
                videoData.HnsDuration = 0;
            }
            else
            {
                videoData.HnsTimestamp = timeStamp;
                videoData.HnsDuration = timeStamp - lastPacketTimestamp;
                lastPacketTimestamp = videoData.HnsTimestamp;
            }
            videoData.CurrentPacketIndex = packetIndex;
            videoData.PacketsInSample = totalPacketsInSample;
            videoData.SampleNumber = SampleNumber;
            if (videoRawData != null)
            {
                videoData.pSample = new byte[videoRawData.Length];
                Array.Copy(videoRawData, videoData.pSample, videoRawData.Length);               
                videoData.Header.cbSize = (uint)(sizeOfVideoPacketFixedFields + videoData.pSample.Length);
                videoData.cbSample = (uint)videoData.pSample.Length;
            }
            else
            {
                videoData.pSample = null;
                videoData.Header.cbSize = sizeOfVideoPacketFixedFields;
                videoData.cbSample = 0;
            }
            videoData.Reserved2 = 0;
            return videoData;
        }
        #endregion Create Methods

        #region Private Methods

        /// <summary>
        /// The callback method to receive data from transport layer.
        /// </summary>
        private void OnDataReceived(byte[] data, uint channelId)
        {
            lock (receivedList)
            {
                RdpevorPdu pdu = new RdpevorPdu();
                bool fSucceed = false;
                bool fResult = PduMarshaler.Unmarshal(data, pdu);
                if (fResult)
                {
                    byte[] pduData = new byte[pdu.Header.cbSize];
                    Array.Copy(data, pduData, pduData.Length);
                    if (pdu.Header.PacketType == PacketTypeValues.TSMM_PACKET_TYPE_PRESENTATION_RESPONSE)
                    {
                        TSMM_PRESENTATION_RESPONSE request = new TSMM_PRESENTATION_RESPONSE();
                        try
                        {
                            fSucceed = PduMarshaler.Unmarshal(pduData, request);
                            receivedList.Add(request);
                        }
                        catch (PDUDecodeException decodeExceptioin)
                        {
                            RdpevorUnkownPdu unkown = new RdpevorUnkownPdu();
                            fSucceed = PduMarshaler.Unmarshal(decodeExceptioin.DecodingData, unkown);
                            receivedList.Add(unkown);
                        }
                    }
                    else if (pdu.Header.PacketType == PacketTypeValues.TSMM_PACKET_TYPE_CLIENT_NOTIFICATION)
                    {
                        TSMM_CLIENT_NOTIFICATION notficatioin = new TSMM_CLIENT_NOTIFICATION();
                        try
                        {
                            fSucceed = PduMarshaler.Unmarshal(pduData, notficatioin);
                            receivedList.Add(notficatioin);
                        }
                        catch (PDUDecodeException decodeExceptioin)
                        {
                            RdpevorUnkownPdu unkown = new RdpevorUnkownPdu();
                            fSucceed = PduMarshaler.Unmarshal(decodeExceptioin.DecodingData, unkown);
                            receivedList.Add(unkown);
                        }
                    }

                }
                if (!fResult || !fSucceed)
                {
                    RdpevorUnkownPdu unkown = new RdpevorUnkownPdu();
                    PduMarshaler.Unmarshal(data, unkown);
                    receivedList.Add(unkown);
                }
            }
        }
        #endregion Private Methods
    }
}
