// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeusb
{
    public class RdpeusbClient
    {
        #region Variables

        private const string RdpeusbChannelName = "URBDRC";

        private RdpedycClient rdpedycClient;

        private Dictionary<uint, DynamicVirtualChannel> rdpeusbChannelDicbyId;

        public Dictionary<uint, QueueManager> receivedDvcData;

        RdpeusbServerPduParser pduParser;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rdpedycClient"></param>
        public RdpeusbClient(RdpedycClient rdpedycClient)
        {
            this.rdpedycClient = rdpedycClient;
            this.rdpeusbChannelDicbyId = new Dictionary<uint, DynamicVirtualChannel>();
            this.receivedDvcData = new Dictionary<uint, QueueManager>();
            this.pduParser = new RdpeusbServerPduParser();
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Wait for creation of dynamic virtual channel for RDPEUSB
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="transportType"></param>
        /// <returns></returns>
        public DynamicVirtualChannel WaitForRdpeusbDvcCreation(TimeSpan timeout, DynamicVC_TransportType transportType = DynamicVC_TransportType.RDP_TCP)
        {
            DynamicVirtualChannel channel = null;
            try
            {
                channel = rdpedycClient.ExpectChannel(timeout, RdpeusbChannelName, transportType);
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
