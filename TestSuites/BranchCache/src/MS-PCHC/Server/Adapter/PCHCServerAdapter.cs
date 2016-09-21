// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pchc
{
    using System;
    using Microsoft.Protocol.TestSuites;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;

    /// <summary>
    /// PCHC server adapter implementation.
    /// </summary>
    public partial class PchcServerAdapter : ManagedAdapterBase, IPchcServerAdapter
    {
        #region field

        /// <summary>
        /// The request resource located on hostedcache, it is fixed defined.
        /// </summary>
        public const string PCHCRESOURCE = "C574AC30-5794-4AEE-B1BB-6651C5315029";

        /// <summary>
        /// The pchc protocol default port number.
        /// </summary>
        public const int PCHCPROTOCOLPORT = 443;

        /// <summary>
        /// The pchcclient entity behaviors to test SUT(HostedCache)
        /// </summary>
        private PCHCClient pchcClient;

        /// <summary>
        /// The request whole uri of the content in the HostedCache server
        /// </summary>
        private string contentUri = string.Empty;

        /// <summary>
        /// The used transportType to request the source
        /// </summary>
        private TransferProtocol transportType;

        /// <summary>
        /// The used Port of HTTPS transport protocol
        /// </summary>
        private int httpsListenPort;

        /// <summary>
        /// The HostedCache machine name
        /// </summary>
        private string hostedCacheMachineName;

        #endregion

        /// <summary>
        /// Initialize protocol adapter.
        /// </summary>
        /// <param name="testSite">The test site instance associated with the current adapter.</param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(ReqConfigurableSite.GetReqConfigurableSite(testSite));

            // The protocol name.
            Site.DefaultProtocolDocShortName = "MS-PCHC";

            PchcBothRoleCaptureCode.Initialize(testSite);
            this.InitializePchcClient(testSite);
        }

        /// <summary>
        /// Initialize pchc client.
        /// </summary>
        /// <param name="testSite">The test site instance associated with the current adapter.</param>
        public void InitializePchcClient(ITestSite testSite)
        {
            if (this.GetProperty("PCHC.TransportType").ToLower() == "Https".ToLower())
            {
                this.transportType = TransferProtocol.HTTPS;
            }
            else if (this.GetProperty("PCHC.TransportType").ToLower() == "Http".ToLower())
            {
                this.transportType = TransferProtocol.HTTP;
            }

            this.httpsListenPort = PCHCPROTOCOLPORT;
            this.hostedCacheMachineName = this.GetProperty("Environment.HostedCacheServer.MachineName");

            this.pchcClient = new PCHCClient(
                this.transportType,
                this.hostedCacheMachineName,
                this.httpsListenPort,
                PCHCRESOURCE,
                new Logger(testSite));
        }

        /// <summary>
        /// Reset the adapter
        /// </summary>
        public override void Reset()
        {
            base.Reset();
        }

        /// <summary>
        /// This action is used to send INITIAL_OFFER_MESSAGE request to and receive the
        /// correspondent Response Message from the hosted cache server.
        /// </summary>
        /// <param name="paddingInMessageHeader">An array formed by bytes for message header padding</param>
        /// <param name="pccrrPort">
        /// The port on which MS-PCCRR server-role will be listening if the hosted cache server initiates the 
        /// Peer Content Caching and Retrieval: Retrieval Protocol (PCCRR) framework [MS-PCCRR] as a client-role peer to
        /// retrieve the missing blocks from the test suite.
        /// </param>
        /// <param name="paddingInConnectionInformation">An array formed by bytes for connection information padding</param>
        /// <param name="hash">Include segment id</param>
        /// <returns>Return the response message of the InitialOfferMessage</returns>
        public ResponseMessage SendInitialOfferMessage(
            byte[] paddingInMessageHeader,
            int pccrrPort,
            byte[] paddingInConnectionInformation,
            byte[] hash)
        {
            // Create the INITIAL_OFFER_MESSAGE struct defined in stack for SendInitialOfferMessage method
            INITIAL_OFFER_MESSAGE initialOfferMessageStack
                = this.pchcClient.CreateInitialOfferMessage(
                pccrrPort,
                hash);

            initialOfferMessageStack.MsgHeader.Padding = paddingInMessageHeader;
            initialOfferMessageStack.ConnectionInfo.Padding = paddingInConnectionInformation;

            try
            {
                RESPONSE_MESSAGE responseMessageStack = 
                    this.pchcClient.SendInitialOfferMessage(initialOfferMessageStack);
                if (responseMessageStack.ResponseCode == RESPONSE_CODE.INTERESTED 
                    || responseMessageStack.ResponseCode == RESPONSE_CODE.OK)
                {
                    this.contentUri = this.transportType.ToString().ToLower() + "://"
                        + this.hostedCacheMachineName + ":" + this.httpsListenPort + "/" + PCHCRESOURCE;

                    PchcBothRoleCaptureCode.ValidateTransport(this.transportType.ToString());
                    this.ValidateTransport(this.pchcClient.HTTPMethod, this.contentUri);
                    this.ValidateServerInitialization(this.httpsListenPort);
                }

                // Convert the RESPONSE_MESSAGE to the ResponseMessage struct defined in adapter
                ResponseMessage responseMessage = ServerHelper.ConvertFromStackForResponseMsg(responseMessageStack);
                this.ValidateInitialOfferMessageResponse(responseMessage);
                return responseMessage;
            }
            catch (HttpStatusCode401Exception e)
            {
                throw new HttpUnauthenticationException(e.Message);
            }
            catch (NoRESPONSEMESSAGEException)
            {
                throw new NoResponseMessageException();
            }
        }

        /// <summary>
        /// This action is used to send SEGMENT_INFO_MESSAGE request to and receive the
        /// correspondent Response Message from the hosted cache server.
        /// </summary>
        /// <param name="paddingInMessageHeader">An array formed by bytes for message header padding</param>
        /// <param name="pccrrPort">The port on which MS-PCCRR server-role will be listening. </param>
        /// <param name="paddingInConnectionInformation">An array formed by bytes for connection information padding</param>
        /// <param name="segmentInformation">The segment information.</param>
        /// <returns>Return the response message of the SegmentInfoMessage</returns>
        public ResponseMessage SendSegmentInfoMessage(
            byte[] paddingInMessageHeader,
            int pccrrPort,
            byte[] paddingInConnectionInformation,
            SegmentInformation segmentInformation)
        {
            // Convert the struct segmentInformation in adapter to the format in stack
            Content_Information_Data_Structure segmentInformationStack =
                ServerHelper.ConvertTostackForContentInfo(segmentInformation);

            // Create the SEGMENT_INFO_MESSAGE struct defined in stack for SendSegmentInfoMessage method
            SEGMENT_INFO_MESSAGE segmentInfoMessage = this.pchcClient.CreateSegmentInfoMessage(
                 pccrrPort,
                 segmentInformationStack);

            segmentInfoMessage.MsgHeader.Padding = paddingInMessageHeader;
            segmentInfoMessage.ConnectionInfo.Padding = paddingInConnectionInformation;

            this.ValidateSegmentInfoMessage(segmentInfoMessage);
            try
            {
                RESPONSE_MESSAGE responseMessageStack = this.pchcClient.SendSegmentInfoMessage(segmentInfoMessage);

                ResponseMessage responseMessage = ServerHelper.ConvertFromStackForResponseMsg(responseMessageStack);

                this.ValidateSegmentInfoResponse(responseMessage);

                return responseMessage;
            }
            catch (NoRESPONSEMESSAGEException)
            {
                throw new NoResponseMessageException();
            }
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
            if (this.pchcClient != null)
            {
                this.pchcClient.Dispose();
            }

            base.Dispose(disposing);
            this.pchcClient = null;
        }

        /// <summary>
        /// Get the value of property in ptfconfig file.
        /// </summary>
        /// <param name="propName">The property name in ptfconfig file.</param>
        /// <returns>Return the value of property.</returns>
        private string GetProperty(string propName)
        {
            if (string.IsNullOrEmpty(propName))
            {
                throw new ArgumentNullException(propName, "The value of propName can't be null or empty.");
            }

            string propValue = string.Empty;
            propValue = Site.Properties[propName];
            return propValue;
        }
    }
}
