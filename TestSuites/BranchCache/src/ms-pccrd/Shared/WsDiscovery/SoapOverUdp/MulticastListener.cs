// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.Transport
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// This class is used for listening multicast message.
    /// </summary>
    public class MulticastListener
    {
        /// <summary>
        /// The listener of multicast message
        /// </summary>
        private UdpClient listener;

        /// <summary>
        /// The thread of the listener
        /// </summary>
        private Thread listenerThread;

        /// <summary>
        /// Identify the listener whether is stopped
        /// </summary>
        private bool isStopped = false;

        /// <summary>
        /// The IP address of multicast
        /// </summary>
        private IPAddress multicastAddr;

        /// <summary>
        /// Initializes a new instance of the <see cref="MulticastListener"/> class
        /// </summary>
        /// <param name="multiAddress">Multicast address</param>
        /// <param name="port">The listening port</param>
        public MulticastListener(string multiAddress, int port)
        {
            this.listener = new UdpClient(port);
            this.multicastAddr = IPAddress.Parse(multiAddress);
            this.listener.JoinMulticastGroup(this.multicastAddr);
        }

        /// <summary>
        /// Receive the multicast message
        /// </summary>
        /// <param name="remoteAddr">Remote IP address</param>
        /// <param name="data">The multicast message</param>
        public delegate void MulticastMessageArrived(IPEndPoint remoteAddr, byte[] data);

        /// <summary>
        /// Receive the multicast message
        /// </summary>
        public event MulticastMessageArrived MulticastMsgArrived;

        /// <summary>
        /// Gets the listener of multicast message
        /// </summary>
        public UdpClient Listener
        {
            get
            {
                return this.listener;
            }
        }

        /// <summary>
        /// Start the listener of multicast message 
        /// </summary>
        public void StartListening()
        {
            this.listenerThread = new Thread(new ParameterizedThreadStart(this.ReceiveLoop));
            this.isStopped = false;
            this.listenerThread.Start();
        }

        /// <summary>
        /// Stop the listener of multicast message 
        /// </summary>
        public void StopListening()
        {
            this.isStopped = true;
            this.listenerThread.Join();
            this.listener.DropMulticastGroup(this.multicastAddr);
            this.listener.Close();
        }

        /// <summary>
        /// Receive multicast message
        /// </summary>
        /// <param name="obj">The object obj</param>
        private void ReceiveLoop(object obj)
        {
            byte[] data = null;
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 0);
            while (!this.isStopped)
            {
                if (this.listener.Available > 0)
                {
                    lock (this.listener)
                    {
                        if (this.listener.Available > 0)
                        {
                            data = this.listener.Receive(ref ip);
                        }
                    }

                    if (null != data && data.Length > 0)
                    {
                        this.MulticastMsgArrived(ip, data);
                        data = null;
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}
