// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrtp
{
    using System;
    using Microsoft.Protocol.TestSuites;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;

    /// <summary>
    /// PCCRTP client adapter implementation to test client endpoint.
    /// </summary>
    public partial class PCCRTPClientAdapter : ManagedAdapterBase, IPCCRTPClientAdapter
    {
        #region Fields

        /// <summary>
        /// The Accept-Encoding header field in PCCRTP message.
        /// </summary>
        private const string ACCEPTENCODING = "Accept-Encoding";

        /// <summary>
        /// The X-P2P-PeerDist header field in PCCRTP message.
        /// </summary>
        private const string XP2PPEERDIST = "X-P2P-PeerDist";

        /// <summary>
        /// Indicates that whether the client was able to retrieve metadata from its peers.
        /// </summary>
        private static bool isMissingDataRequest;

        /// <summary>
        /// Indicates the IP version is IPv4 or IPv6.
        /// </summary>
        private bool isIpv4;

        /// <summary>
        /// Indicates the listening port that the request is sent to.
        /// </summary>
        private int listenPort;

        /// <summary>
        /// The Operation System (OS) version that test suite run on.
        /// </summary>
        private OSVersion sutOsVersion;

        /// <summary>
        /// Initialize an instance of PccrtpServer class.
        /// </summary>
        private PccrtpServer pccrtpServerStack;

        /// <summary>
        /// Initialize an instance of PccrtpRequest class.
        /// </summary>
        private PccrtpRequest pccrtpRequest = new PccrtpRequest();

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets a value indicating whether the client was able to retrieve metadata from its peers.
        /// </summary>
        public static bool IsMissingDataRequest
        {
            get
            {
                return isMissingDataRequest;
            }

            set
            {
                isMissingDataRequest = value;
            }
        }

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Initialize adapter.
        /// </summary>
        /// <param name="testSite">The test site instance associated with the current adapter.</param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(ReqConfigurableSite.GetReqConfigurableSite(testSite));
            PccrtpBothRoleCapture.Initialize(testSite);
            this.Site.DefaultProtocolDocShortName = "MS-PCCRTP";
            this.sutOsVersion = (OSVersion)Enum.Parse(
                typeof(OSVersion),
                this.Site.Properties["Environment.DistributedSUT.OSVersion"].ToString(),
                true);
            this.isIpv4 = bool.Parse(Site.Properties["PCCRTP.Protocol.Address.isIPv4"]);
            this.listenPort = int.Parse(Site.Properties["Environment.ContentServer.HTTP.Port"]);

            if (this.isIpv4)
            {
                this.pccrtpServerStack = new PccrtpServer(this.listenPort, IPAddressType.IPv4, new Logger(testSite));
            }
            else
            {
                this.pccrtpServerStack = new PccrtpServer(this.listenPort, IPAddressType.IPv6, new Logger(testSite));
            }

            this.pccrtpServerStack.StartServer();
        }

        #endregion

        #region Interface Methods

        /// <summary>
        /// Receive a PCCRTP request message from the HTTP/1.1 client (SUT) when testing client endpoint.
        /// </summary>
        /// <returns>Return the PCCRTP request message received.</returns>
        public PccrtpRequest ReceivePccrtpRequestMessage()
        {
            TimeSpan timeOut = TimeSpan.FromSeconds(double.Parse(Site.Properties["PCCRTP.Protocol.TimeOut"]));
            this.pccrtpRequest = this.pccrtpServerStack.ReceivePccrtpRequest(timeOut);
            this.pccrtpRequest.DecodeHttpHeader(this.pccrtpRequest.HttpRequest);
            PccrtpBothRoleCapture.VerifyTransport(this.pccrtpRequest.HttpRequest.ProtocolVersion.ToString());
            PccrtpBothRoleCapture.VerifyPccrtpCommonHeader(this.pccrtpRequest.HttpHeader);
            this.VerifyPccrtpRequestRequirements();

            return this.pccrtpRequest;
        }

        /// <summary>
        /// Generate and send an PCCRTP response message to the HTTP/1.1 client (SUT) when testing client endpoint.
        /// </summary>
        /// <param name="requestFileFullPath">Indicate that local full path of the requested file on the driver computer.
        /// </param>
        public void SendPccrtpResponseMessage(string requestFileFullPath)
        {
            PccrtpResponse pccrtpResponse = this.pccrtpServerStack.GenerateResponseMessage(
                requestFileFullPath,
                Site.Properties["PCCRTP.Protocol.ServerSecret"]);

            this.pccrtpServerStack.SentPccrtpResponse(pccrtpResponse);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Reset the adapter.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            this.pccrtpServerStack.StopServer();
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
            if (this.pccrtpServerStack != null)
            {
                this.pccrtpServerStack.Dispose();
            }

            base.Dispose(disposing);
            this.pccrtpServerStack = null;
        }

        #endregion
    }
}
