// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.WsDiscovery
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.SoapMessage;
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.Transport;

    /// <summary>
    /// Receive the wsdicovery message
    /// </summary>
    /// <param name="sender">Remote IP address</param>
    /// <param name="message">The wsdicovery message</param>
    public delegate void MessageArrivedEventArgs(IPEndPoint sender, SoapEnvelope message);

    /// <summary>
    /// Wsdiscovery client used to transport wsdicovery messages.
    /// </summary>
    public class WsDiscoveryClient : IDisposable
    {
        /// <summary>
        /// Multicast address
        /// </summary>
        public const string MultiAddress = "239.255.255.250";

        /// <summary>
        /// Multicast port
        /// </summary>
        public const int MultiPort = 3702;

        /// <summary>
        /// Transport wsdicovery messages interface
        /// </summary>
        private ITransport transport;

        /// <summary>
        /// Identify the instance whether is disposed
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Indicates if the WS-Discovery client instance is stopped to receive ProbeMatch message.
        /// </summary>
        private bool stopped = false;

        /// <summary>
        /// Last response message
        /// </summary>
        private string lastRawResp;

        /// <summary>
        /// Output log
        /// </summary>
        private ILogPrinter logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WsDiscoveryClient"/> class
        /// </summary>
        public WsDiscoveryClient()
            : this(TransportType.SoapOverUdp)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WsDiscoveryClient"/> class
        /// </summary>
        /// <param name="logger">A instance used to output log</param>
        public WsDiscoveryClient(ILogPrinter logger)
            : this(TransportType.SoapOverUdp, logger)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WsDiscoveryClient"/> class
        /// </summary>
        /// <param name="transportType">Transport type</param>
        public WsDiscoveryClient(TransportType transportType)
        {
            switch (transportType)
            {
                case TransportType.SoapOverUdp:
                    this.transport = new SoapOverUdp();
                    this.transport.MessageArrived += new Transport.MessageArrived(this.Transport_MessageArrived);
                    this.transport.StartListening();
                    break;
                default:
                    throw new InvalidOperationException(string.Format("The transport type {0} is not supported.", transportType));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WsDiscoveryClient"/> class
        /// </summary>
        /// <param name="transportType">Transport type</param>
        /// <param name="logger">A instance used to output log</param>
        public WsDiscoveryClient(TransportType transportType, ILogPrinter logger)
            : this(transportType)
        {
            if (null == logger)
            {
                throw new ArgumentNullException("logger", "The input parameter \"logger\" is null.");
            }

            this.logger = logger;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="WsDiscoveryClient"/> class
        /// </summary>
        ~WsDiscoveryClient()
        {
            Dispose(false);
        }

        /// <summary>
        /// Receive the wsdicovery message
        /// </summary>
        public event MessageArrivedEventArgs MessageArrived;

        #region Properties

        /// <summary>
        /// Gets or sets transport wsdicovery messages interface
        /// </summary>
        public ITransport Transport
        {
            get { return this.transport; }
            set { this.transport = value; }
        }

        /// <summary>
        /// Gets last response message
        /// </summary>
        public string LastRawResp
        {
            get { return this.lastRawResp; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the WS-Discovery client instance is stopped to receive ProbeMatch message.
        /// </summary>
        public bool Stop
        {
            get
            {
                return this.stopped;
            }

            set
            {
                bool stop = value;
                if (stop && !this.stopped)
                {
                    this.transport.StopListening();
                    this.stopped = true;
                }

                if (!stop && this.stopped)
                {
                    this.transport.StartListening();
                    this.stopped = false;
                }

                this.stopped = stop;
            }
        }

        #endregion

        /// <summary>
        /// Send multicast message
        /// </summary>
        /// <param name="message">The multicast message</param>
        public void SendMulticast(SoapEnvelope message)
        {
            if (null == message)
            {
                throw new ArgumentNullException("message", "The input parameter \"message\" is null.");
            }

            this.transport.SendBytes(Encoding.UTF8.GetBytes(SoapEnvelope.Serialize(message)), MultiAddress, MultiPort);
        }

        /// <summary>
        /// Send unicast message
        /// </summary>
        /// <param name="message">The unicast message</param>
        /// <param name="address">Destination IP address</param>
        /// <param name="port">Destination port</param>
        public void SendUnicast(SoapEnvelope message, string address, int port)
        {
            if (null == message)
            {
                throw new ArgumentNullException("message", "The input parameter \"message\" is null.");
            }

            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException("ipAddress", "The input parameter \"ipAddress\" is null or empty.");
            }

            if (0 == port)
            {
                throw new ArgumentNullException("ipAddress", "The input parameter \"ipAddress\" is 0.");
            }

            this.transport.SendBytes(Encoding.UTF8.GetBytes(SoapEnvelope.Serialize(message)), address, port);
        }

        /// <summary>
        /// Create a probe message
        /// </summary>
        /// <param name="types">Probe type</param>
        /// <param name="scopes">The scopes value</param>
        /// <param name="matchBy">Match by</param>
        /// <returns>The probe message</returns>
        public SoapEnvelope CreateProbeMessage(string types, string[] scopes, string matchBy)
        {
            ProbeType probe = new ProbeType(types, scopes, matchBy);
            return new SoapEnvelope(new ProbeOp(probe));
        }

        #region IDispose

        /// <summary>
        /// Release all resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release all resources
        /// </summary>
        /// <param name="disposing">Indicate user or GC calling this method</param>
        protected virtual void Dispose(bool disposing)
        {
            this.transport.StopListening();
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.transport.Dispose();
                }

                this.disposed = true;
            }
        }

        #endregion

        /// <summary>
        /// Receive the wsdicovery message
        /// </summary>
        /// <param name="remoteAddr">Remote IP address</param>
        /// <param name="data">The wsdicovery message</param>
        private void Transport_MessageArrived(IPEndPoint remoteAddr, byte[] data)
        {
            this.lastRawResp = Encoding.UTF8.GetString(data);
            this.MessageArrived(remoteAddr, this.Deserialize(this.lastRawResp));
        }

        /// <summary>
        /// Deserialize the wsdiscovery message
        /// </summary>
        /// <param name="data">The wsdiscovery message data</param>
        /// <returns>The format of soap message</returns>
        private SoapEnvelope Deserialize(string data)
        {
            SoapEnvelope result = null;

            XmlAttributes attrs = new XmlAttributes();

            XmlElementAttribute attr = new XmlElementAttribute();
            attr.ElementName = "Body";
            attr.Type = typeof(ProbeMatchOp);
            attrs.XmlElements.Add(attr);

            XmlAttributes attrs2 = new XmlAttributes();
            XmlElementAttribute attr2 = new XmlElementAttribute();
            attr2.ElementName = "Header";
            attr2.Type = typeof(WsdHeader);

            attrs2.XmlElements.Add(attr2);

            XmlAttributeOverrides attros = new XmlAttributeOverrides();
            attros.Add(typeof(SoapEnvelope), "Body", attrs);
            attros.Add(typeof(SoapEnvelope), "Header", attrs2);

            XmlSerializer a = new XmlSerializer(typeof(SoapEnvelope), attros);

            try
            {
                using (XmlReader strReader = XmlReader.Create(data))
                {
                    result = (SoapEnvelope)a.Deserialize(strReader);
                }
            }
            catch (InvalidOperationException e)
            {
                if (null != this.logger)
                {
                    this.logger.AddDebug("An error is encountered while trying to deserialize the received data");
                    this.logger.AddDebug("The received data is " + data);
                }

                throw new InvalidOperationException(@"Failed to deserialize the received data from on-wire.
                      Please check the logs for details. The exception message from .Net framwork is" + e.Message);
            }

            return result;
        }
    }
}

