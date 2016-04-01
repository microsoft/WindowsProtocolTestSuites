// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    /// <summary>
    /// Base class for Static Virtual Channel Manager
    /// This class is used to manage all SVCs in a RDP connection
    /// </summary>
    public abstract class StaticVirtualChannelManager : IDisposable
    {

        #region protected variables

        /// <summary>
        /// Timeout
        /// </summary>
        protected TimeSpan timeout = new TimeSpan(0, 0, 1);

        /// <summary>
        /// Whether the manager is running
        /// </summary>
        protected bool running;

        /// <summary>
        /// Dictionary of channels, use channel ID as key
        /// </summary>
        protected Dictionary<UInt16, StaticVirtualChannel> channelDicById;

        /// <summary>
        /// Dictionary of channels, use channel name as key
        /// </summary>
        protected Dictionary<string, StaticVirtualChannel> channelDicByName;

        /// <summary>
        /// Thread used to process received SVC data automatically
        /// </summary>
        protected Thread receiveThread;

        protected TimeSpan waitInterval = new TimeSpan(0, 0, 0, 0, 10);

        #endregion protected variables

        #region Properties

        /// <summary>
        /// Whether the automatic receive thread is running
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return running;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Expect a static virtual channel PDU
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public abstract StackPacket ExpectPacket(TimeSpan timeout, out UInt16 channelId);

        /// <summary>
        /// Start receive loop, which will automatically process static virtual channel data received
        /// </summary>
        public void Start()
        {
            if (!this.running)
            {
                this.running = true;
                this.receiveThread = new Thread(ReceiveLoop);
                this.receiveThread.Start();
            }
        }

        /// <summary>
        /// Stop receive loop
        /// </summary>
        public void Stop()
        {
            running = false;
            if (receiveThread.IsAlive)
            {
                receiveThread.Abort();
                receiveThread.Join();
            }            
        }

        /// <summary>
        /// Get Static virtual channel by channel ID
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public StaticVirtualChannel GetChannelById(UInt16 channelId)
        {
            if (channelDicById != null && channelDicById.ContainsKey(channelId))
            {
                return channelDicById[channelId];
            }
            return null;
        }

        /// <summary>
        /// Get static virtual channel by Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StaticVirtualChannel GetChannelByName(string name)
        {
            if (channelDicByName != null && channelDicByName.ContainsKey(name))
            {
                return channelDicByName[name];
            }
            return null;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (this.running)
            {
                this.Stop();
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Receive loop
        /// </summary>
        private void ReceiveLoop()
        {
            while (running)
            {
                UInt16 channelId;
                try
                {
                    StackPacket packet = ExpectPacket(timeout, out channelId);

                    if (packet != null
                        && channelDicById != null && channelDicById.ContainsKey(channelId))
                    {
                        channelDicById[channelId].ReceivePackets(packet);
                    }
                }
                catch
                {
                    // catch exception in not main thread.
                }
                Thread.Sleep(waitInterval);
            }
        }

        #endregion Private Methods

        #region Internal Methods

         /// <summary>
        /// Split and compress the complete virtual channel data into chunk data.
        /// </summary>
        /// <param name="completeData">The compete virtual channel data. This argument can be null.</param>
        /// <returns>The splitted chunk data.</returns>
        internal ChannelChunk[] SplitToChunks(UInt16 channelId, byte[] completeData)
        {
            if (!channelDicById.ContainsKey(channelId))
            {                
                throw new ArgumentException("The channel id does not exist!");
            }
            StaticVirtualChannel channel = channelDicById[channelId];
            return channel.SplitToChunks(completeData);
        }

        #endregion Internal Methos
    }
}
