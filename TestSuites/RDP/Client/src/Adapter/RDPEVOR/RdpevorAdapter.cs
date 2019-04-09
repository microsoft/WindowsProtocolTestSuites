// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpevor;
using Microsoft.Protocols.TestSuites.Rdpbcgr;

namespace Microsoft.Protocols.TestSuites.Rdpevor
{
    public class RdpevorAdapter : ManagedAdapterBase, IRdpevorAdapter
    {
        #region Private Variables

        const string RdpevorControlChannelName = "Microsoft::Windows::RDS::Video::Control::v08.01";
        const string RdpevorDataChannelName = "Microsoft::Windows::RDS::Video::Data::v08.01";
        const int LargestSampleSize =  959; //used 959, according to the max size of DTLS encryption
        
        RdpevorServer rdpevorServer;
        TimeSpan waitTime;
        RdpevorNegativeType testType;        
        #endregion

        #region IAdapter Members

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
           
            #region WaitTime
            string strWaitTime = Site.Properties["WaitTime"];
            if (strWaitTime != null)
            {
                int waitSeconds = Int32.Parse(strWaitTime);
                waitTime = new TimeSpan(0, 0, waitSeconds);
            }
            else
            {
                waitTime = new TimeSpan(0, 0, 20);
            }
            #endregion

        }

        public override void Reset()
        {
            base.Reset();
            rdpevorServer = null;
            testType = RdpevorNegativeType.None;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Do Something.
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IRdpevorAdapter Members


        /// <summary>
        /// Initialize this protocol with create control and data channels.
        /// </summary>
        /// <param name="rdpedycServer">RDPEDYC Server instance</param>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <returns>true if client supports this protocol; otherwise, return false.</returns>
        public bool ProtocolInitialize(RdpedycServer rdpedycServer, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            if (!rdpedycServer.IsMultipleTransportCreated(transportType))
            {
                rdpedycServer.CreateMultipleTransport(transportType);
            }

            this.rdpevorServer = new RdpevorServer(rdpedycServer);
            bool success = false;
            //Create control virtual channel.
            try
            {
                success = rdpevorServer.CreateRdpevorDvc(waitTime, transportType);
            }
            catch(Exception e)
            {
                Site.Log.Add(LogEntryKind.Comment, "Exception occurred when creating RDPEVOR channels: {0}", e.Message);
            }

            return success;
        }

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
        public void SendPresentationRequest(byte presentationId, CommandValues command, uint srcWidth, uint srcHeight, uint scaledWidth, uint scaledHeight, ulong geometryId, VideoSubtype videoType, byte[] extraData)
        {
            TSMM_PRESENTATION_REQUEST request = rdpevorServer.CreatePresentationRequest(presentationId, command, srcWidth, srcHeight, scaledWidth, scaledHeight, geometryId, videoType, extraData);
            if (this.testType == RdpevorNegativeType.PresentationRequest_InvalidPacketLength)
            {
                //Set the packet length to an invalid value.
                request.Header.cbSize = (uint)(request.Header.cbSize - 1);
            }
            else if (this.testType == RdpevorNegativeType.PresentationRequest_InvalidVersion)
            {
                //Set version to an invalid value.
                request.Version = RdpevorVersionValues.InvalidValue;
            }

            rdpevorServer.SendRdpevorControlPdu(request);
        }

        public void SendVideoPacket(byte presentationId, TsmmVideoData_FlagsValues flags, ushort packetIndex, ushort totalPacketsInSample, uint SampleNumber, byte[] packetData, ulong timeStamp, bool isCompressed = false)
        {
            TSMM_VIDEO_DATA videoDataPacket = rdpevorServer.CreateVideoDataPacket(presentationId, flags, packetIndex, totalPacketsInSample, SampleNumber, packetData, timeStamp);
            if (this.testType == RdpevorNegativeType.VideoData_InvalidPacketLength)
            {
                //Set the packet length to an invalid value.
                videoDataPacket.Header.cbSize = (uint)(videoDataPacket.Header.cbSize - 1);
            }
            else if (this.testType == RdpevorNegativeType.VideoData_InvalidVersion)
            {
                //Set version to an invalid value.
                videoDataPacket.Version = RdpevorVersionValues.InvalidValue;
            }
            
            rdpevorServer.SendRdpevorDataPdu(videoDataPacket, isCompressed);
        }

        /// <summary>
        /// Method to sent a sample to client.
        /// </summary>
        /// <param name="presentationId">This is the same number as the PresentationId field in the TSMM_PRESENTATION_REQUEST message.</param>
        /// <param name="isKeyFrame">Is the sample a key frame.</param>
        /// <param name="SampleNumber">The number of sample in the video stream.</param>
        /// <param name="isCompressed">Whether the packet will be compressed before sent.</param>
        public void SendVideoSample(byte presentationId, bool isKeyFrame, uint SampleNumber, byte[] sampleData, ulong timeStamp, bool isCompressed=false)
        {
            if (sampleData == null)
            {
                return;
            }
            ushort totalPackets = (ushort)(((sampleData.Length - 1) / LargestSampleSize) + 1);
            for (ushort packetIndex = 1; packetIndex <= totalPackets; packetIndex++)
            {
                TsmmVideoData_FlagsValues flags = TsmmVideoData_FlagsValues.None;
                if(isKeyFrame)
                {
                    flags |= TsmmVideoData_FlagsValues.TSMM_VIDEO_DATA_FLAG_KEYFRAME;
                }
                int packetSize = LargestSampleSize;
                if (packetIndex == totalPackets)
                {
                    packetSize = sampleData.Length % LargestSampleSize;
                    flags |= TsmmVideoData_FlagsValues.TSMM_VIDEO_DATA_FLAG_HAS_TIMESTAMPS;
                }
                byte[] packetData = new byte[packetSize];
                Array.Copy(sampleData, (packetIndex - 1) * LargestSampleSize, packetData, 0, packetSize);
                              
                SendVideoPacket(presentationId, flags, packetIndex, totalPackets, SampleNumber, packetData, timeStamp, isCompressed);                
                //System.Threading.Thread.Sleep(100);//Sleep 0.1 second to avoid this packet is merged with next one in TCP layer.
            }
        }       

        /// <summary>
        /// Method to expect a TSMM_PRESENTATION_RESPONSE from client.
        /// </summary>
        /// <param name="presentationId">The expected presentation Id.</param>
        public void ExpectPresentationiResponse(byte presentationId)
        {
            DateTime beginTime = DateTime.Now;
            DateTime endTime = beginTime + waitTime;
            TSMM_PRESENTATION_RESPONSE response = null;
            while (response == null && DateTime.Now < endTime)
            {
                System.Threading.Thread.Sleep(100);
                response = rdpevorServer.ExpectRdpevorPdu<TSMM_PRESENTATION_RESPONSE>(waitTime);
                if (response != null && response.PresentatioinId != presentationId)
                {
                    response = null;
                }
            }
            Site.Assert.IsTrue(response != null, "TSMM_PRESENTATION_RESPONSE received.");
        }

        /// <summary>
        /// Method to expect a TSMM_CLIENT_NOTIFICATION from client.
        /// </summary>
        /// <param name="presentationId">The expected presentation Id.</param>
        /// <param name="notificationType">The expected notification type</param>
        public void ExpectClientNotification(byte presentationId, NotificationTypeValues notificationType)
        {
            DateTime beginTime = DateTime.Now;
            DateTime endTime = beginTime + waitTime;
            TSMM_CLIENT_NOTIFICATION notification = null;
            while (notification == null && DateTime.Now < endTime)
            {
                System.Threading.Thread.Sleep(100);
                notification = rdpevorServer.ExpectRdpevorPdu<TSMM_CLIENT_NOTIFICATION>(waitTime);
                if (notification.PresentatioinId != presentationId)
                {
                    notification = null;
                }
            }
            Site.Assert.IsTrue(notification != null, "TSMM_CLIENT_NOTIFICATION received.");
        }

        /// <summary>
        /// Set the test type to positive or negative test types.
        /// </summary>
        /// <param name="negType">The type of current test. None means positive test, others are negative tests.</param>
        public void SetTestType(RdpevorNegativeType negType)
        {
            testType = negType;
        }              

        #endregion
    }
}
