// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.Transport
{
    using System;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;

    /// <summary>
    /// The properties of the soap message.
    /// </summary>
    public class SoapNetworkMessage : Delayed
    {
        /// <summary>
        /// Random instance
        /// </summary>
        private static Random randomGenerator = new Random(SoapOverUdp.UdpMinDelay);

        /// <summary>
        /// Destination address
        /// </summary>
        private string dstAddress;

        /// <summary>
        /// Destination port
        /// </summary>
        private int dstPort;

        /// <summary>
        /// Payload data in the soap message
        /// </summary>
        private byte[] payload;

        /// <summary>
        /// Delay time
        /// </summary>
        private int delay;

        /// <summary>
        /// UDP repeat times
        /// </summary>
        private int udpRepeat;

        /// <summary>
        /// Next sent time
        /// </summary>
        private DateTime nextSend;
             
        /// <summary>
        /// Initializes a new instance of the SoapNetworkMessage class.
        /// </summary>
        /// <param name="payload">Payload data in the soap message</param>
        /// <param name="address">Destination address</param>
        /// <param name="port">Destination port</param>
        public SoapNetworkMessage(byte[] payload, string address, int port)
        {
            this.payload = payload;
            this.dstAddress = address;
            this.dstPort = port;
            if ((address == ProtocolStrings.MulticastAddressIPv4 || address == ProtocolStrings.MulticastAddressIPv6)
                && (port == ProtocolStrings.MulticastPort))
            {
                this.udpRepeat = SoapOverUdp.MulticastUdpRepeat;
            }
            else
            {
                this.udpRepeat = SoapOverUdp.UnicastUdpRepeat;
            }

            this.delay = randomGenerator.Next(SoapOverUdp.UdpMinDelay, SoapOverUdp.UdpMaxDelay);
        }

        #region Properties

        /// <summary>
        /// Gets the payload data in the soap message
        /// </summary>
        public byte[] Payload
        {
            get
            {
                return this.payload;
            }
        }

        /// <summary>
        /// Gets the destination address
        /// </summary>
        public string DstAddress
        {
            get { return this.dstAddress; }
        }

        /// <summary>
        /// Gets the destination port
        /// </summary>
        public int DstPort
        {
            get { return this.dstPort; }
        }

        #endregion

        /// <summary>
        /// Increase delay time
        /// </summary>
        public void IncreaseDelay()
        {
            this.delay = this.delay * 2;
            if (this.delay > SoapOverUdp.UdpUpperDelay)
            {
                this.delay = SoapOverUdp.UdpUpperDelay;
            }
        }

        /// <summary>
        /// Decrease repeat times
        /// </summary>
        public void DecreaseUDP_REPEAT()
        {
            this.udpRepeat--;
        }

        /// <summary>
        /// Is done
        /// </summary>
        /// <returns> The result.</returns>
        public bool IsDone()
        {
            return this.udpRepeat <= 0;
        }

        /// <summary>
        /// set timestamp for next send
        /// </summary>
        public void AdjustAfterSend()
        {
            this.DecreaseUDP_REPEAT();
            this.IncreaseDelay();

            // set timestamp for next send
            this.nextSend = DateTime.Now.AddMilliseconds(this.delay);
        }

        /// <summary>
        /// Returns the remaining delay associated with this message in milliseconds.
        /// </summary>
        /// <returns> Returns the remaining delayin milliseconds. 
        /// Zero or negative values indicate that the delay has already elapsed.</returns>
        public override double GetDelay()
        {
            return (this.nextSend - DateTime.Now).TotalMilliseconds;
        }

        /// <summary>
        /// Compare the delays of two messages.
        /// </summary>
        /// <param name="o"> Delayed messages.</param>
        /// <returns> Returns the result. 0 if equal, -1 if o has smaller delay, 1 if o has larger delay.</returns>
        public override int CompareTo(Delayed o)
        {
            double a = o.GetDelay();
            double b = this.GetDelay();

            if (a < b)
            {
                return -1;
            }
            else if (a > b)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
