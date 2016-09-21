// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrd
{
    using System;
    using System.Net;
    using System.Xml;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery;
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.SoapMessage;

    /// <summary>
    /// receive Probe message for Pccrd
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="message">The soap envelope message</param>
    public delegate void ReceiveProbeMessageHandler(IPEndPoint sender, SoapEnvelope message);

    /// <summary>
    /// pccrd server
    /// </summary>
    public class PccrdServer : IDisposable
    {
        /// <summary>
        /// A instance of WsDiscoveryService
        /// </summary>
        private WsDiscoveryService service;

        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="PccrdServer"/>class without parameter
        /// </summary>
        public PccrdServer()
        {
            this.service = new WsDiscoveryService();
            this.service.MessageArrived += new MessageArrivedEventArgs(this.HandleRequest);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PccrdServer"/>class
        /// </summary>
        /// <param name="logger"> The specified logger.</param>
        public PccrdServer(ILogPrinter logger)
        {
            this.service = new WsDiscoveryService(logger);
            this.service.MessageArrived += new MessageArrivedEventArgs(this.HandleRequest);
        }

        /// <summary>
        ///  Finalizes an instance of the <see cref="PccrdServer"/> class
        /// </summary>
        ~PccrdServer()
        {
            Dispose(false);
        }

        /// <summary>
        /// event handler
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
            Justification = "Disable the warning for design necessary")]
        public event ReceiveProbeMessageHandler ReceiveProbeMessage;

        /// <summary>
        /// Start to listen incoming requests
        /// </summary>
        public void StartListening()
        {
            this.service.StartListening();
        }

        /// <summary>
        /// Stop to listen incoming requests
        /// </summary>
        public void StopListening()
        {
            this.service.StopListening();
        }

        /// <summary>
        /// Create Probematch message
        /// </summary>
        /// <param name="relatesTo">The probe message relates to</param>
        /// <param name="instanceId">The instanceId of the probe message</param>
        /// <param name="messageNumber">The messageNumber of probe message</param>
        /// <param name="matches">The probe matches</param>
        /// <returns>return probeMatch message</returns>
        public SoapEnvelope CreateProbeMatchMessage(
            string relatesTo,
            string instanceId,
            uint messageNumber,
            CustomProbeMatchType[] matches)
        {
            ProbeMatchType[] wsdiscoveryMatches = new ProbeMatchType[matches.Length];
            XmlDocument doc = new XmlDocument();

            for (int i = 0; i < matches.Length; i++)
            {
                wsdiscoveryMatches[i] = new ProbeMatchType();
                wsdiscoveryMatches[i].EndpointReference = matches[i].EndpointReference;
                wsdiscoveryMatches[i].MetadataVersion = matches[i].MetadataVersion;
                wsdiscoveryMatches[i].Scopes = matches[i].Scopes;
                wsdiscoveryMatches[i].Types = matches[i].Types;
                wsdiscoveryMatches[i].XAddrs = matches[i].XAddrs;
                XmlElement peerDist = doc.CreateElement(
                    "PeerDist",
                    "PeerDistData",
                    "http://schemas.microsoft.com/p2p/2007/09/PeerDistributionDiscovery");
                XmlElement blockCount = doc.CreateElement(
                    "PeerDist",
                    "BlockCount",
                    "http://schemas.microsoft.com/p2p/2007/09/PeerDistributionDiscovery");
                blockCount.InnerText = matches[i].PeerDistData.BlockCount;
                peerDist.AppendChild(blockCount);
                wsdiscoveryMatches[i].Any = new XmlElement[] { peerDist };
            }

            return this.service.CreateProbeMatchMessage(relatesTo, instanceId, messageNumber, wsdiscoveryMatches);
        }

        /// <summary>
        /// send unicast message
        /// </summary>
        /// <param name="message">The soap envelope message</param>
        /// <param name="ipaddress">The remote ipaddress</param>
        /// <param name="port">The used port number for transporting</param>
        public void SendUnicast(SoapEnvelope message, string ipaddress, int port)
        {
            this.service.SendUnicast(message, ipaddress, port);
        }

        /// <summary>
        /// receive message
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="message">The soap envelope message</param>
        public void HandleRequest(IPEndPoint sender, SoapEnvelope message)
        {
            this.ReceiveProbeMessage(sender, message);
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
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.service.Dispose();
                }

                this.disposed = true;
            }
        }

        #endregion
    }
}

