// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeusb
{
    public class RdpeusbServer
    {
        #region Variables

        private const string RdpeusbChannelName = "URBDRC";

        private RdpedycServer rdpedycServer;

        private Dictionary<uint, DynamicVirtualChannel> rdpeusbChannelDicbyId;

        public Dictionary<uint, QueueManager> receivedDvcData;

        RdpeusbClientPduParser pduParser;

        #endregion Variables

        #region Properties

        public uint RequestCompletionInterfaceId
        {
            get
            {
                if (pduParser != null)
                {
                    return pduParser.RequestCompletionInterfaceId;
                }
                return 0;
            }

            set
            {
                if (pduParser != null)
                {
                    pduParser.RequestCompletionInterfaceId = value;
                }
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// RDPEDYC Server
        /// </summary>
        /// <param name="rdpedycServer"></param>
        public RdpeusbServer(RdpedycServer rdpedycServer)
        {
            this.rdpedycServer = rdpedycServer;
            this.rdpeusbChannelDicbyId = new Dictionary<uint, DynamicVirtualChannel>();
            this.receivedDvcData = new Dictionary<uint, QueueManager>();
            this.pduParser = new RdpeusbClientPduParser();
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Create dynamic virtual channel.
        /// </summary>
        /// <param name="transportType">selected transport type for created channels</param>
        /// <param name="timeout">Timeout</param>
        /// <returns>DVC created</returns>
        public DynamicVirtualChannel CreateRdpeusbDvc(TimeSpan timeout, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {

            const ushort priority = 0;
            DynamicVirtualChannel channel = null;
            try
            {
                channel = rdpedycServer.CreateChannel(timeout, priority, RdpeusbChannelName, transportType, OnDataReceived);
            }
            catch
            {
            }
            if (channel != null)
            {
                rdpeusbChannelDicbyId.Add(channel.ChannelId, channel);
                return channel;
            }
            return null;
        }
             
        /// <summary>
        /// Expect a RDPEUSB packet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public T ExpectRdpeusbPdu<T>(uint channelId, TimeSpan timeout) where T : EusbPdu
        {
            return ExpectRdpeusbPdu(channelId, timeout) as T;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Call back function for corresponding DVC
        /// </summary>
        /// <param name="data"></param>
        /// <param name="channelId"></param>
        private void OnDataReceived(byte[] data, uint channelId)
        {
            lock (receivedDvcData)
            {
                if (!receivedDvcData.ContainsKey(channelId))
                {
                    receivedDvcData.Add(channelId, new QueueManager());
                }
                receivedDvcData[channelId].AddObject(data);
            }
        }

        private EusbPdu ExpectRdpeusbPdu(uint channelId, TimeSpan timeout)
        {
            lock (receivedDvcData)
            {
                if (!receivedDvcData.ContainsKey(channelId))
                {
                    receivedDvcData.Add(channelId, new QueueManager());
                }
            }

            byte[] data = null;
            try
            {
                data = (byte[])(receivedDvcData[channelId].GetObject(ref timeout));
            }
            catch (TimeoutException)
            {
                return null;
            }

            if (null == data)
            {
                return null;
            }

            EusbPdu pdu = pduParser.ParsePdu(data);

            return pdu;
        }

        #endregion Private Methods

    }
}
