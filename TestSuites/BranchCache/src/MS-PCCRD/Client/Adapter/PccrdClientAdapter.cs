// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrd
{
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.Protocol.TestSuites;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrd;
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery;
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.SoapMessage;

    /// <summary>
    /// A implementation of IPccrdClientAdapter.
    /// </summary>
    public partial class PccrdClientAdapter : ManagedAdapterBase, IPccrdClientAdapter
    {
        /// <summary>
        ///  A instance of PccrdServer class.
        /// </summary>
        private PccrdServer server;

        /// <summary>
        /// Track whether the ProbeMessage is received
        /// </summary>
        private bool receive;

        /// <summary>
        /// The returned soap envelope message
        /// </summary>
        private List<string> returned = new List<string>();

        /// <summary>
        /// event handler
        /// </summary>
        public event ReceiveProbeMsgHandler ReceiveProbeMessage;

        /// <summary>
        /// Initialize the adapter.
        /// </summary>
        /// <param name="testSite"> test site.</param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(ReqConfigurableSite.GetReqConfigurableSite(testSite));
            this.server = new PccrdServer(new Logger(testSite));
            this.server.ReceiveProbeMessage += new ReceiveProbeMessageHandler(this.Server_ReceiveProbeMessage);
            Site.DefaultProtocolDocShortName = "MS-PCCRD";
        }

        /// <summary>
        /// Start to listen requests.
        /// </summary>
        public void StartListening()
        {
            this.receive = true;
            this.server.StartListening();
        }

        /// <summary>
        /// Stop to listen requests.
        /// </summary>
        public void StopListening()
        {
            this.receive = false;
            this.server.StopListening();
        }

        /// <summary>
        /// Sends ProbeMatch message.
        /// </summary>
        /// <param name="relatesTo"> "relatesTo" field in soap header.</param>
        /// <param name="instanceId">instanceId field in soap  identifier for the current
        /// instance of the device being published</param>
        /// <param name="messageNumber">The message number</param>
        /// <param name="matches">The service property matches</param>
        /// <param name="ip">The remote ip address</param>
        /// <param name="port">The used port number for transporting</param>
        public void SendProbeMatchMessage(
            string relatesTo,
            string instanceId,
            uint messageNumber,
            ServiceProperty[] matches,
            string ip,
            int port)
        {
            CustomProbeMatchType[] probeMatches = new CustomProbeMatchType[matches.Length];
            
            for (int i = 0; i < matches.Length; i++)
            {
                CustomProbeMatchType match = new CustomProbeMatchType();
                match.EndpointReference = new EndpointReferenceType();
                match.EndpointReference.Address = new AttributedURI();
                match.EndpointReference.Address.Value = matches[i].Address;
                match.MetadataVersion = matches[i].MetadataVersion;
                match.Scopes = new ScopesType(new string[] { matches[i].Scopes });
                match.Types = matches[i].Types;
                match.XAddrs = matches[i].XAddrs;
                match.PeerDistData = new PeerDistData(matches[i].BlockCount);
                probeMatches[i] = match;
            }

            this.server.SendUnicast(
                this.server.CreateProbeMatchMessage(relatesTo, instanceId, messageNumber, probeMatches),
                ip,
                port);
        }

        /// <summary>
        /// Reset the adapter.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            this.receive = false;
            this.returned.Clear();
        }

        /// <summary>
        /// Dispose managed resource and unmanaged resource.
        /// </summary>
        /// <param name="disposing"> If disposing equals false, the method has been called by the 
        /// runtime from inside the finalizer and you should not reference 
        /// other objects. Only unmanaged resources can be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this.server.Dispose();
            }
        }

        /// <summary>
        /// receive message handler
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="message">The soap envelope message</param>
        private void Server_ReceiveProbeMessage(IPEndPoint sender, SoapEnvelope message)
        {
            WsdHeader header = (WsdHeader)message.Header;
            ProbeType probe = ((ProbeOp)message.Body).Probe;
            if (this.receive && (!this.returned.Contains(header.MessageID.Value)))
            {
                this.CaptureHeaderRequirements(header);
                this.CaptureProbeRequirements(probe);
                PccrdBothRoleCaptureCode.CaptureCommonRequirements(Site);
                PccrdBothRoleCaptureCode.CaptureTypesElementRequirements(probe.Types, Site);
                PccrdBothRoleCaptureCode.CaptureScopesElementRequirements(probe.Scopes, Site);

                ProbeMsg probeMsg = new ProbeMsg(header.MessageID.Value, probe.Types, probe.Scopes.Text[0]);
                this.ReceiveProbeMessage(sender, probeMsg);
            }
        }
    }
}
