// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.BranchCache
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrd;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery;
    using Microsoft.Protocols.TestTools.StackSdk.WsDiscovery.SoapMessage;

    /// <summary>
    /// The content caching and retrieval framework adapter.
    /// </summary>
    public class BranchCacheFrameworkAdapter : ManagedAdapterBase, IBranchCacheFrameworkAdapter
    {
        /// <summary>
        /// The request resource located on hosted cache, it is fixed defined.
        /// </summary>
        public const string PCHCRESOURCE = "C574AC30-5794-4AEE-B1BB-6651C5315029";

        /// <summary>
        /// The pchc protocol default port number.
        /// </summary>
        private const int PCHCPROTOCOLPORT = 443;

        /// <summary>
        /// The pccrtp client entity used to behavior as a client point.
        /// </summary>
        private PccrtpClient pccrtpStackClient;

        /// <summary>
        /// The pchc client entity used to behavior as a client point.
        /// </summary>
        private PCHCClient pchcClient;

        /// <summary>
        /// The pccrr server entity is used as a server point.
        /// </summary>
        private PccrrServer pccrrServer;

        /// <summary>
        /// The pccrr server entity is used as a server point.
        /// </summary>
        private PccrdServer pccrdServer;

        /// <summary>
        /// Is received Discovery request.
        /// </summary>
        private bool isReceived;

        /// <summary>
        /// List of returned.
        /// </summary>
        private List<string> returned = new List<string>();

        /// <summary>
        /// The pccrtp response with "peerdist" header.
        /// </summary>
        private PccrtpResponse pccrtpResponse;

        /// <summary>
        /// Receive probe message.
        /// </summary>
        public event ReceiveProbeMsgHandler ReceiveProbeMessage;

        /// <summary>
        /// Receive MS-PCCRR request message handle.
        /// </summary>
        public event EventHandler<ReceivedPccrrRequestEventArg> ReceivePccrrRequestHandler;

        /// <summary>
        /// Initialize the frame work adapter
        /// </summary>
        /// <param name="testSite">The test site instance associated with the current adapter.</param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            this.pccrtpStackClient = new PccrtpClient();

            TransferProtocol transpotType = TransferProtocol.HTTPS;

            string hostedCacheMachineName = Site.Properties.Get("Environment.HostedCacheServer.MachineName");
            this.pchcClient =
                new PCHCClient(transpotType, hostedCacheMachineName, PCHCPROTOCOLPORT, PCHCRESOURCE);
        }

        /// <summary>
        /// Reset the adapter
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            this.isReceived = false;
            this.pccrtpResponse = null;
            if (this.pccrrServer != null)
            {
                this.pccrrServer.CloseConnections();
                this.pccrrServer = null;
            }
        }

        /// <summary>
        /// Send the ProbeMatch message.
        /// </summary>
        /// <param name="relatesTo">The relatesTo.</param>
        /// <param name="instanceId">The instanceId.</param>
        /// <param name="messageNumber">The messageNumber.</param>
        /// <param name="matches">The matches.</param>
        /// <param name="ip">The ip address of content client.</param>
        /// <param name="port">The port of content client.</param>
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

            this.pccrdServer.SendUnicast(
                this.pccrdServer.CreateProbeMatchMessage(relatesTo, instanceId, messageNumber, probeMatches),
                ip,
                port);
        }

        /// <summary>
        /// Start the MS-PCCRD server listening to incoming MS-PCCRD request message.
        /// </summary>
        public void StartPccrdServerListening()
        {
            try
            {
                this.pccrdServer = new PccrdServer();
                this.pccrdServer.ReceiveProbeMessage += new ReceiveProbeMessageHandler(this.Server_ReceiveProbeMessage);
                this.pccrdServer.StartListening();
            }
            catch (ThreadStateException e)
            {
                Site.Log.Add(
                    LogEntryKind.Debug,
                    "Can't start pccrd server listening. Detailed information: {0}",
                    e.Message);
                throw;
            }
        }

        /// <summary>
        /// Start the MS-PCCRR server listening to incoming MS-PCCRR request message.
        /// </summary>
        /// <param name="port">The listening port.</param>
        public void StartPccrrServerListening(int port)
        {
            try
            {
                this.pccrrServer =
                    new PccrrServer(
                        port, 
                        string.Empty, 
                        IPAddressType.IPv4);
                this.pccrrServer.MessageArrived
                    += new Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr.MessageArrivedEventArgs(
                        this.RetrievalTransport_Receive);

                this.pccrrServer.StartListening();
            }
            catch (ThreadStateException e)
            {
                Site.Log.Add(
                    LogEntryKind.Debug,
                    "Can't start pccrr server listening. Detailed information: {0}",
                    e.Message);
                throw;
            }
        }

        /// <summary>
        /// Get the segment IDs computed using content information that is got from the content server.
        /// </summary>
        /// <param name="serverAddress"> The content server address.</param>
        /// <param name="port"> The port that the content server is listening.</param>
        /// <param name="path"> The path of the file to request.</param>
        /// <returns> Returns content info if success.</returns>
        public string[] GetSegmentIds(string serverAddress, int port, string path)
        {
            PccrtpRequest pccrtpRequest = this.pccrtpStackClient.CreatePccrtpRequest(serverAddress, port, path);
            PccrtpResponse pccrtpStackResponse = this.pccrtpStackClient.SendHttpRequest(
                Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp.HttpVersionType.HttpVersion11,
                pccrtpRequest,
                int.Parse(Site.Properties["PCCRR.Protocol.timeout"]) * 1000);
            List<byte[]> segmentIDs = (List<byte[]>)this.pccrtpStackClient.GetSegmentId(pccrtpStackResponse);

            List<string> segmentIdStr = new List<string>();
            foreach (byte[] segmentId in segmentIDs)
            {
                segmentIdStr.Add(ToHexString(segmentId));
            }

            return segmentIdStr.ToArray();
        }

        /// <summary>
        /// Get the byte array list of segment IDs computed using content information that is got from the content server.
        /// </summary>
        /// <param name="serverAddress"> The content server address.</param>
        /// <param name="port"> The port that the content server is listening.</param>
        /// <param name="uri"> The path of the file to request.</param>
        /// <returns> Returns segment id if success.</returns>
        public ICollection<byte[]> GetSegmentIdsByteArray(string serverAddress, int port, string uri)
        {
            if (this.pccrtpResponse == null)
            {
                this.GetPccrtpResponse(serverAddress, port, uri);
            }

            ICollection<byte[]> segmentID = (List<byte[]>)this.pccrtpStackClient.GetSegmentId(this.pccrtpResponse);

            return segmentID;
        }

        /// <summary>
        /// Get the content Info from the content server.
        /// </summary>
        /// <param name="serverAddress"> The content server address.</param>
        /// <param name="port"> The port that the content server is listening.</param>
        /// <param name="uri"> The path of the file to request.</param>
        /// <returns> Returns content information if success.</returns>
        public Content_Information_Data_Structure GetContentInfo(string serverAddress, int port, string uri)
        {
            if (this.pccrtpResponse == null)
            {
                this.GetPccrtpResponse(serverAddress, port, uri);
            }

            Content_Information_Data_Structure contentInfoStack = this.pccrtpResponse.ContentInfo;

            return contentInfoStack;
        }

        /// <summary>
        /// Offer the block hashes to the hosted cache server.
        /// </summary>
        /// <param name="contentInfo">The content information</param>
        public void OfferHostedCacheContentInfo(Content_Information_Data_Structure contentInfo)
        {
            // It's important to make sure the hosted cache server having the block hash.
            int connectionInfoPort = int.Parse(Site.Properties["PCHC.SegmentInfoMessage.PccrrTransPort"]);

            SEGMENT_INFO_MESSAGE segmentInfoMsgStack = this.pchcClient.CreateSegmentInfoMessage(
                connectionInfoPort,
                contentInfo);
            this.pchcClient.SendSegmentInfoMessage(segmentInfoMsgStack);
        }

        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// </summary>
        /// <param name="disposing">
        /// If disposing equals false, the method has been called by the 
        /// runtime from inside the finalizer and you should not reference 
        /// other objects. Only unmanaged resources can be disposed.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (this.pccrrServer != null)
            {
                this.pccrrServer.Dispose();
            }

            if (this.pccrdServer != null)
            {
                this.pccrdServer.Dispose();
            }

            if (this.pchcClient != null)
            {
                this.pchcClient.Dispose();
            }

            if (this.pccrtpStackClient != null)
            {
                this.pccrtpStackClient.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Convert input byte array to Hex string.
        /// </summary>
        /// <param name="data"> Input byte array.</param>
        /// <returns> Returns Hex string.</returns>
        private static string ToHexString(byte[] data)
        {
            string hexStr = string.Empty;
            if (data != null)
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    builder.Append(data[i].ToString("X2"));
                }

                hexStr = builder.ToString();
            }

            return hexStr;
        }

        /// <summary>
        /// The handle of MessageArrivedEventArgs happened.
        /// </summary>
        /// <param name="remoteAddr">Represents the remote endpoint address</param>
        /// <param name="pccrrPacket">The received pccrrPacket, used to judge whether
        /// it is a PccrrGETBLKLISTRequestPacket</param>
        private void RetrievalTransport_Receive(IPEndPoint remoteAddr, PccrrPacket pccrrPacket)
        {
            if (pccrrPacket.PacketType == MsgType_Values.MSG_GETBLKLIST)
            {
                PccrrGETBLKLISTRequestPacket package = (PccrrGETBLKLISTRequestPacket)pccrrPacket;

                int port = package.Uri.Port;
                byte[] segmentID = package.MsgGetBLKLIST.SegmentID;
                this.ReceivePccrrRequestHandler(this, new ReceivedPccrrRequestEventArg(port, segmentID));
            }
            else if (pccrrPacket.PacketType == MsgType_Values.MSG_GETBLKS)
            {
                PccrrGETBLKSRequestPacket package = (PccrrGETBLKSRequestPacket)pccrrPacket;

                int port = package.Uri.Port;
                byte[] segmentID = package.MsgGetBLKS.SegmentID;
                this.ReceivePccrrRequestHandler(this, new ReceivedPccrrRequestEventArg(port, segmentID));
            }
        }

        /// <summary>
        /// Get the pccrtp response with http header "peerdist".
        /// </summary>
        /// <param name="serverAddress"> The content server address.</param>
        /// <param name="port"> The port that the content server is listening.</param>
        /// <param name="uri"> The path of the file to request.</param>
        /// <returns>Only true can be returned that specifies the pccrtp response with
        /// http header "peerdist" is received.</returns>
        /// <exception cref="Exception">No http response with "peerdist" header is received,</exception>
        private bool GetPccrtpResponse(string serverAddress, int port, string uri)
        {
            PccrtpRequest pccrtpRequest = this.pccrtpStackClient.CreatePccrtpRequest(serverAddress, port, uri);
            PccrtpResponse pccrtpResponseTemp = this.pccrtpStackClient.SendHttpRequest(
                Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp.HttpVersionType.HttpVersion11,
                pccrtpRequest,
                int.Parse(Site.Properties["PCCRTP.Protocol.TimeOut"]) * 1000);
            int timesOfSendingPccrtpMsg = 1;

            while (!pccrtpResponseTemp.HttpResponse.ContentEncoding.Equals("peerdist"))
            {
                if (timesOfSendingPccrtpMsg >= 3)
                {
                    throw new InvalidOperationException(string.Format(
                        "Send {0} times Pccrtp request message, no http response with \"peerdist\" header is received.",
                        timesOfSendingPccrtpMsg));
                }

                pccrtpResponseTemp = this.pccrtpStackClient.SendHttpRequest(
                    Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp.HttpVersionType.HttpVersion11,
                    pccrtpRequest,
                    int.Parse(Site.Properties["PCCRTP.Protocol.TimeOut"]) * 1000);
                timesOfSendingPccrtpMsg++;
            }

            this.pccrtpResponse = pccrtpResponseTemp;
            return true;
        }

        /// <summary>
        /// Receive probe message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The probe message.</param>
        private void Server_ReceiveProbeMessage(IPEndPoint sender, SoapEnvelope message)
        {
            WsdHeader header = (WsdHeader)message.Header;
            ProbeType probe = ((ProbeOp)message.Body).Probe;
            if (!this.isReceived && (!this.returned.Contains(header.MessageID.Value)))
            {
                this.isReceived = true;

                ProbeMsg probeMsg = new ProbeMsg(header.MessageID.Value, probe.Types, probe.Scopes.Text[0]);
                this.ReceiveProbeMessage(sender, probeMsg);
            }
        }
    }
}

