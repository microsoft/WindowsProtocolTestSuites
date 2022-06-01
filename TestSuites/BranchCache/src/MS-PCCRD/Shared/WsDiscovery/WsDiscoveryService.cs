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
    using System.Xml.XPath;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.SoapMessage;
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.Transport;

    /// <summary>
    /// Wsdiscovery service used to transport wsdicovery messages.
    /// </summary>
    public class WsDiscoveryService : IDisposable
    {
        /// <summary>
        /// Transport wsdicovery messages interface
        /// </summary>
        private ITransport transport;

        /// <summary>
        /// Indicates if the service is stopped to listen
        /// </summary>
        private bool isStopped;

        /// <summary>
        /// The service start time
        /// </summary>
        private DateTime serviceStartTime;

        /// <summary>
        /// Base time
        /// </summary>
        private DateTime baseTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// logger used to log debug or comment info
        /// </summary>
        private ILogPrinter logger;

        /// <summary>
        /// The last response message
        /// </summary>
        private string lastRawResp;

        /// <summary>
        /// Initializes a new instance of the WsDiscoveryService class with default transport type
        /// </summary>
        public WsDiscoveryService()
            : this(TransportType.SoapOverUdp)
        {
        }

        /// <summary>
        /// Initializes a new instance of the WsDiscoveryService class with logger and default transport type
        /// </summary>
        /// <param name="logger"> The logger.</param>
        public WsDiscoveryService(ILogPrinter logger)
            : this(TransportType.SoapOverUdp, logger)
        {
        }

        /// <summary>
        /// Initializes a new instance of the WsDiscoveryService class with specified transport type
        /// </summary>
        /// <param name="transportType">Transprot type</param>
        public WsDiscoveryService(TransportType transportType)
        {
            switch (transportType)
            {
                case TransportType.SoapOverUdp:
                    this.transport = new SoapOverUdp(ProtocolStrings.MulticastAddressIPv4, ProtocolStrings.MulticastPort);
                    this.transport.MessageArrived += new Transport.MessageArrived(this.Transport_MessageArrived);
                    this.serviceStartTime = DateTime.UtcNow;
                    break;
                default:
                    throw new InvalidOperationException(string.Format("The transport type {0} is not supported.", transportType));
            }
        }

        /// <summary>
        /// Initializes a new instance of the WsDiscoveryService class with logger and specified transport type
        /// </summary>
        /// <param name="transportType"> The transport type</param>
        /// <param name="logger"> The logger</param>
        public WsDiscoveryService(TransportType transportType, ILogPrinter logger)
            : this(transportType)
        {
            if (null == logger)
            {
                throw new ArgumentNullException("logger", "The input parameter \"logger\" is null.");
            }

            this.logger = logger;
        }

        #region event

        /// <summary>
        /// Receive the wsdicovery message
        /// </summary>
        public event MessageArrivedEventArgs MessageArrived;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the last response message
        /// </summary>
        public string LastRawResp
        {
            get { return this.lastRawResp; }
        }

        #endregion

        /// <summary>
        /// Start the listener to receive wsdiscovery message
        /// </summary>
        public void StartListening()
        {
            this.isStopped = false;
            this.transport.StartListening();
        }

        /// <summary>
        /// Stop the listener to not receive wsdiscovery message
        /// </summary>
        public void StopListening()
        {
            this.isStopped = true;
            this.transport.StopListening();
        }

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

            this.transport.SendBytes(Encoding.UTF8.GetBytes(SoapEnvelope.Serialize(message)), ProtocolStrings.MulticastAddressIPv4, ProtocolStrings.MulticastPort);
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
        /// Create a probe match message
        /// </summary>
        /// <param name="relatesTo">The URI of relates to</param>
        /// <param name="instanceId">Instance ID</param>
        /// <param name="messageNumber">Message number</param>
        /// <param name="matches">Probe match messages</param>
        /// <returns>The response message</returns>
        public SoapEnvelope CreateProbeMatchMessage(string relatesTo, string instanceId, uint messageNumber, ProbeMatchType[] matches)
        {
            WsdHeader header = new WsdHeader();
            header.Action.Value = ProtocolStrings.ProbeMatchesAction;
            header.RelatesTo = new AttributedURI();
            header.RelatesTo.Value = relatesTo;
            header.AppSequence = new AppSequenceType();
            header.AppSequence.MessageNumber = 1;
            header.AppSequence.InstanceId = (uint)(this.serviceStartTime - this.baseTime).Seconds;
            header.To = new AttributedURI();
            header.To.Value = "http://schemas.xmlsoap.org/ws/2004/08/addressing/role/anonymous";
            ProbeMatchOp match = new ProbeMatchOp();
            match.ProbeMatches = new ProbeMatchesType();
            match.ProbeMatches.ProbeMatch = matches;
            SoapEnvelope resp = new SoapEnvelope(SoapMessageVersion.Soap11Wsa10, header, match);
            return resp;
        }

        /// <summary>
        /// Dispose the resource of the class
        /// </summary>
        public void Dispose()
        {
            if (!this.isStopped)
            {
                this.transport.StopListening();
            }

            this.transport.Dispose();
        }

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
            attr.Type = typeof(ProbeOp);
            attrs.XmlElements.Add(attr);

            XmlAttributes attrs2 = new XmlAttributes();
            XmlElementAttribute attr2 = new XmlElementAttribute();
            attr2.ElementName = "Header";
            attr2.Type = typeof(WsdHeader);

            attrs2.XmlElements.Add(attr2);

            XmlAttributeOverrides attros = new XmlAttributeOverrides();
            attros.Add(typeof(SoapEnvelope), "Body", attrs);
            attros.Add(typeof(SoapEnvelope), "Header", attrs2);

            XmlSerializer serializer = new XmlSerializer(typeof(SoapEnvelope), attros);

            try
            {
                XmlTextReader textReader = new XmlTextReader(data);
                textReader.Settings.XmlResolver = null;
                textReader.Settings.DtdProcessing = DtdProcessing.Prohibit;
                result = (SoapEnvelope)serializer.Deserialize(textReader);
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
