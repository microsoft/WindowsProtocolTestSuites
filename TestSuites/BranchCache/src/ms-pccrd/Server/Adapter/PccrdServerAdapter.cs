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
    /// A implementation of IPccrdServerAdapter.
    /// </summary>
    public partial class PccrdServerAdapter : ManagedAdapterBase, IPccrdServerAdapter
    {
        /// <summary>
        /// A instance of PccrdClient class.
        /// </summary>
        private PccrdClient client;

        /// <summary>
        /// A variable to indicate if start to receive income messages
        /// </summary>
        private bool startReceiving = false;

        /// <summary>
        /// A collection of received messages
        /// </summary>
        private List<string> returned = new List<string>();

        /// <summary>
        /// Event handler to receive ProbeMatchMsg
        /// </summary>
        public event ReceiveProbeMatchMsgHandler ReceiveProbeMatchMessage;

        /// <summary>
        /// Initialize the adapter.
        /// </summary>
        /// <param name="testSite"> test site.</param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(ReqConfigurableSite.GetReqConfigurableSite(testSite));
            
            this.client = new PccrdClient(new Logger(testSite));
            this.client.ReceivePccrdMessage += new ReceivePccrdMessageHandler(this.Client_ReceivePccrdMessage);
            this.startReceiving = true;
            Site.DefaultProtocolDocShortName = "MS-PCCRD";
        }

        /// <summary>
        /// Send the probe message.
        /// </summary>
        /// <param name="type">The probe message type.</param>
        /// <param name="scope">The scope of the probe message.</param>
        public void SendProbeMessage(string type, string scope)
        {
            SoapEnvelope req = this.client.CreateProbe(type, new string[] { scope });
            this.client.SendMessage(req);
            this.startReceiving = true;
        }

        /// <summary>
        /// Reset the adapter.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            this.startReceiving = false;
            this.client.Stop = true;
            this.returned.Clear();
            this.client.Stop = false;
        }

        /// <summary>
        /// Dispose managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing">If disposing equals false, the method has been called by the 
        /// runtime from inside the finalizer and you should not reference 
        /// other objects. Only unmanaged resources can be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this.client.Dispose();
            }
        }

        /// <summary>
        /// Receive the pccrd messages
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="message">The soap envelope message</param>
        private void Client_ReceivePccrdMessage(IPEndPoint sender, SoapEnvelope message)
        {
            Status status = new Status();
            status.ErrorCode = string.Empty;
            status.FaultType = FaultType.OK;

            WsdHeader header = (WsdHeader)message.Header;
            if (this.startReceiving && (!this.returned.Contains(header.MessageID.Value)))
            {
                this.returned.Add(header.MessageID.Value);
                ProbeMatchesType matches = ((ProbeMatchOp)message.Body).ProbeMatches;
                this.CaptureHeaderRequirements(header);
                this.CaptureProbeMatchesRequirements(matches);
                PccrdBothRoleCaptureCode.CaptureCommonRequirements(Site);
                foreach (ProbeMatchType match in matches.ProbeMatch)
                {
                    PccrdBothRoleCaptureCode.CaptureTypesElementRequirements(match.Types, Site);
                    PccrdBothRoleCaptureCode.CaptureScopesElementRequirements(match.Scopes, Site);
                }

                ProbeMatchMsg probeMatchMsg = new ProbeMatchMsg();
                probeMatchMsg.InstanceId = header.AppSequence.InstanceId.ToString();
                probeMatchMsg.MessageNumber = header.AppSequence.MessageNumber;
                PeerProperties[] result = new PeerProperties[matches.ProbeMatch.Length];

                for (int i = 0; i < matches.ProbeMatch.Length; i++)
                {
                    result[i] = new PeerProperties();
                    result[i].Address = matches.ProbeMatch[i].EndpointReference.Address.Value;
                    result[i].BlockCount = matches.ProbeMatch[i].Any[0].InnerText;
                    result[i].MetadataVersion = matches.ProbeMatch[i].MetadataVersion;
                    result[i].Scopes = matches.ProbeMatch[i].Scopes.Text[0];
                    result[i].Types = matches.ProbeMatch[i].Types;
                    result[i].XAddrs = matches.ProbeMatch[i].XAddrs;
                }

                probeMatchMsg.Matches = result;
                this.ReceiveProbeMatchMessage(status, probeMatchMsg);
            }
        }
    }
}

