// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.Transport
{
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// Send soap message over UDP.
    /// </summary>
    public class SoapOverUdp : ITransport
    {
        /// <summary>
        /// Number of times to repeat unicast messages.
        /// </summary>
        public const int UnicastUdpRepeat = 0;

        /// <summary>
        /// Number of times to repeat multicast messages.
        /// </summary>
        public const int MulticastUdpRepeat = 4;
        
        /// <summary>
        /// Minimum initial delay for resend.
        /// </summary>
        public const int UdpMinDelay = 50;
        
        /// <summary>
        /// Maximum initial delay for resend.
        /// </summary>
        public const int UdpMaxDelay = 250;
        
        /// <summary>
        /// Maximum delay between resent messages.
        /// </summary>
        public const int UdpUpperDelay = 500;

        /// <summary>
        /// UDP client
        /// </summary>
        private UdpClient client;

        /// <summary>
        /// The unicast listener
        /// </summary>
        private UnicastListener unicastListener;

        /// <summary>
        /// The multicast listener
        /// </summary>
        private MulticastListener multicastListener;

        /// <summary>
        /// The UDP sender
        /// </summary>
        private UdpSender sender;

        /// <summary>
        /// Initializes a new instance of the SoapOverUdp class.
        /// </summary>
        public SoapOverUdp()
        {
            this.client = GetUdpClient();
            this.sender = new UdpSender(this.client);
            this.unicastListener = new UnicastListener(this.client);
            this.unicastListener.UnicastMsgArrived += new UnicastListener.UnicastMessageArrived(this.UnicastListenerMessageArrived);
        }

        /// <summary>
        /// Initializes a new instance of the SoapOverUdp class.
        /// </summary>
        /// <param name="multicastAddress">The multicast address</param>
        /// <param name="multicastPort">The multicast port</param>
        public SoapOverUdp(string multicastAddress, int multicastPort)
        {
            this.client = GetUdpClient();
            this.sender = new UdpSender(this.client);
            this.unicastListener = new UnicastListener(this.client);
            this.unicastListener.UnicastMsgArrived += new UnicastListener.UnicastMessageArrived(this.UnicastListenerMessageArrived);
            this.multicastListener = new MulticastListener(multicastAddress, multicastPort);
            this.multicastListener.MulticastMsgArrived += new MulticastListener.MulticastMessageArrived(this.MulticastListener_multicastMsgArrived);
        }

        #region event

        /// <summary>
        /// Receive the soap message
        /// </summary>
        public event MessageArrived MessageArrived;

        #endregion

        /// <summary>
        /// Send the soap message over UDP
        /// </summary>
        /// <param name="bytes">The message data</param>
        /// <param name="address">The destination address</param>
        /// <param name="port">The destination port</param>
        public void SendBytes(byte[] bytes, string address, int port)
        {
            this.sender.SendBytes(bytes, address, port);
        }
        
        /// <summary>
        /// Start the unicast and multicast listener 
        /// </summary>
        public void StartListening()
        {
            this.sender.Start();
            this.unicastListener.StartListening();
            if (null != this.multicastListener)
            {
                this.multicastListener.StartListening();
            }
        }

        /// <summary>
        /// Stop the unicast and multicast listener
        /// </summary>
        public void StopListening()
        {
            this.sender.Stop();
            if (null != this.multicastListener)
            {
                this.multicastListener.StopListening();
            }

            this.unicastListener.StopListening();
        }

        /// <summary>
        /// Dispose the class resource
        /// </summary>
        public void Dispose()
        {
            this.client.Close();
        }

        #region private methods

        /// <summary>
        /// Get a instance of the UDP client
        /// </summary>
        /// <returns>The instance of the UDP client</returns>
        private static UdpClient GetUdpClient()
        {
            // try to use a random port among the dynamic port range (49152-65535) as specified in RFC6335 section 6 Port Number Ranges
            int port = new System.Random().Next(65535 - 49152) + 49152;
            UdpClient client;
            for (;;)
            {
                try
                {
                    client = new UdpClient(port++);
                    return client;
                }
                catch (SocketException)
                { 
                    // Nothing to do here
                }
            }
        }

        /// <summary>
        /// Receive the unicast message
        /// </summary>
        /// <param name="remoteAddr">Remote address</param>
        /// <param name="data">Message data</param>
        private void UnicastListenerMessageArrived(IPEndPoint remoteAddr, byte[] data)
        {
            this.MessageArrived(remoteAddr, data);
        }

        /// <summary>
        /// Receive the multicast message
        /// </summary>
        /// <param name="remoteAddr">Remote address</param>
        /// <param name="data">Message data</param>
        private void MulticastListener_multicastMsgArrived(IPEndPoint remoteAddr, byte[] data)
        {
            this.MessageArrived(remoteAddr, data);
        }

        #endregion
    }
}
