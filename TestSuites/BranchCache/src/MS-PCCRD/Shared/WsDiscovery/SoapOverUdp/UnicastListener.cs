// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.Transport
{
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// This class is used for listening unicast message.
    /// </summary>
    public class UnicastListener
    {
        /// <summary>
        /// The listener of unicast message
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
        /// Initializes a new instance of the UnicastListener class.
        /// </summary>
        /// <param name="listener">The listener of unicast</param>
        public UnicastListener(UdpClient listener)
        {
            this.listener = listener;
        }

        /// <summary>
        /// Receive the unicast message
        /// </summary>
        /// <param name="remoteAddr">Remote IP address</param>
        /// <param name="data">The unicast message</param>
        public delegate void UnicastMessageArrived(IPEndPoint remoteAddr, byte[] data);

        /// <summary>
        /// Receive the unicast message
        /// </summary>
        public event UnicastMessageArrived UnicastMsgArrived;

        /// <summary>
        /// Gets the listener of unicast message
        /// </summary>
        public UdpClient Listener
        {
            get
            {
                return this.listener;
            }
        }

        /// <summary>
        /// Start the listener of unicast 
        /// </summary>
        public void StartListening()
        {
            this.listenerThread = new Thread(new ParameterizedThreadStart(this.ReceiveLoop));
            this.isStopped = false;
            this.listenerThread.Start();
        }

        /// <summary>
        /// Stop the listener of unicast 
        /// </summary>
        public void StopListening()
        {
            this.isStopped = true;
            this.listenerThread.Join();
        }

        /// <summary>
        /// Receive unicast message.
        /// </summary>
        /// <param name="obj"> The object.</param>
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
                        this.UnicastMsgArrived(ip, data);
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
