// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.Transport
{
    using System.Net.Sockets;
    using System.Threading;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;

    /// <summary>
    /// Send message over UDP.
    /// </summary>
    public class UdpSender
    {
        /// <summary>
        /// The UDP sender
        /// </summary>
        private UdpClient sender;

        /// <summary>
        /// The queue of the soap message
        /// </summary>
        private DelayQueue<SoapNetworkMessage> requests = new DelayQueue<SoapNetworkMessage>();
        
        /// <summary>
        /// The thread of the sender
        /// </summary>
        private Thread senderThread;

        /// <summary>
        /// Identify the sender whether is stopped
        /// </summary>
        private bool isStopped = false;

        /// <summary>
        /// Initializes a new instance of the UdpSender class.
        /// </summary>
        /// <param name="sender">The UDP sender</param>
        public UdpSender(UdpClient sender)
        {
            this.sender = sender;
        }

        #region Properties

        /// <summary>
        /// Gets the UDP sender
        /// </summary>
        public UdpClient Sender
        {
            get
            {
                return this.sender;
            }
        }

        #endregion

        /// <summary>
        /// Start the thread of send message
        /// </summary>
        public void Start()
        {
            this.senderThread = new Thread(new ParameterizedThreadStart(this.Send));
            this.isStopped = false;
            this.senderThread.Start();
        }

        /// <summary>
        /// Stop the thread of send message
        /// </summary>
        public void Stop()
        {
            this.isStopped = true;
            this.senderThread.Join();
        }

        /// <summary>
        /// Send message to destination
        /// </summary>
        /// <param name="data">Message data</param>
        /// <param name="address">Destination address</param>
        /// <param name="port">Destination port</param>
        public void SendBytes(byte[] data, string address, int port)
        {
            this.requests.Add(new SoapNetworkMessage(data, address, port));
        }

        /// <summary>
        /// Send message to destination
        /// </summary>
        /// <param name="obj">Message data</param>
        private void Send(object obj)
        {
            SoapNetworkMessage nm;
            while (!this.isStopped)
            {
                try
                {
                    nm = this.requests.Poll();
                    if (null == nm)
                    {
                        continue;
                    }

                    lock (this.sender)
                    {
                        this.sender.Send(nm.Payload, nm.Payload.Length, nm.DstAddress, nm.DstPort);
                    }

                    nm.AdjustAfterSend();
                    if (!nm.IsDone())
                    {
                        this.requests.Add(nm);
                    }
                }
                catch (SocketException)
                {
                    this.isStopped = true;
                }
            }
        }
    }
}
