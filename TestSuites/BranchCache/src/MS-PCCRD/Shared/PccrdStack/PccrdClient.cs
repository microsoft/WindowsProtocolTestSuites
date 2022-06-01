// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrd
{
    using System;
    using System.Net;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery;
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.SoapMessage;

    /// <summary>
    /// Receive ProbeMatch for PCCRD
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="message">The soap envelope message</param>
    public delegate void ReceivePccrdMessageHandler(IPEndPoint sender, SoapEnvelope message);

    /// <summary>
    /// Pccrd client
    /// </summary>
    public class PccrdClient : IDisposable
    {
        /// <summary>
        /// Match rule for pccrd
        /// </summary>
        private const string MatchBy = "http://schemas.xmlsoap.org/ws/2005/04/discovery/strcmp0";

        /// <summary>
        /// A instance of WsDiscoveryClient
        /// </summary>
        private WsDiscoveryClient discoveryClient;

        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="PccrdClient"/>class without parameter
        /// </summary>
        public PccrdClient()
        {
            this.discoveryClient = new WsDiscoveryClient();
            this.discoveryClient.MessageArrived += new MessageArrivedEventArgs(this.ReceiveMessage);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PccrdClient"/>class with logger
        /// </summary>
        /// <param name="logger">The logger record the debug information</param>
        public PccrdClient(ILogPrinter logger)
        {
            this.discoveryClient = new WsDiscoveryClient(logger);
            this.discoveryClient.MessageArrived += new MessageArrivedEventArgs(this.ReceiveMessage);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="PccrdClient"/> class
        /// </summary>
        ~PccrdClient()
        {
            Dispose(false);
        }

        /// <summary>
        /// event handler
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly",
            Justification = "Disable the rule for design necessary.")]
        public event ReceivePccrdMessageHandler ReceivePccrdMessage;

        /// <summary>
        /// Gets or sets a value indicating whether if the WS-Discovery client
        /// instance is stopped to receive ProbeMatch message.
        /// </summary>
        public bool Stop
        {
            get { return this.discoveryClient.Stop; }
            set { this.discoveryClient.Stop = value; }
        }

        /// <summary>
        /// Create a Probe Message
        /// </summary>
        /// <param name="types">The types of the probe message</param>
        /// <param name="scopes">The probe message scopes</param>
        /// <returns>Returns Probe message</returns>
        public SoapEnvelope CreateProbe(string types, string[] scopes)
        {
            return this.discoveryClient.CreateProbeMessage(types, scopes, MatchBy);
        }

        /// <summary>
        /// Sends message
        /// </summary>
        /// <param name="reqMsg">out message</param>
        public void SendMessage(SoapEnvelope reqMsg)
        {
            this.discoveryClient.SendMulticast(reqMsg);
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
                    this.discoveryClient.Dispose();
                }

                this.disposed = true;
            }
        }

        /// <summary>
        /// receive message
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="message">The soap envelope message</param>
        private void ReceiveMessage(IPEndPoint sender, SoapEnvelope message)
        {
            this.ReceivePccrdMessage(sender, message);
        }

        #endregion
    }
}

