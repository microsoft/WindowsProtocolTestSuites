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
    /// PCHC adapter implementation to test client behaviors.
    /// </summary>
    public partial class PchcClientAdapter : ManagedAdapterBase, IPchcClientAdapter
    {
        #region filed

        /// <summary>
        /// Indicates the hosted cache, used to receive the request and send the response.
        /// </summary>
        private PCHCServer pchcServer;

        #endregion

        #region override method

        /// <summary>
        /// Initialize protocol adapter.
        /// </summary>
        /// <param name="testSite">The test site.</param>
        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(ReqConfigurableSite.GetReqConfigurableSite(testSite));
            Site.DefaultProtocolDocShortName = "MS-PCHC";

            PchcBothRoleCaptureCode.Initialize(testSite);
            this.pchcServer = new PCHCServer(
                TransferProtocol.HTTPS,
                int.Parse(this.GetProperty("PCHC.Protocol.NewPort")),
                IPAddressType.IPv4,
                new Logger(testSite));
            this.pchcServer.Start();
        }

        /// <summary>
        /// Reset the adapter
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            // Restart the listen service.
            this.pchcServer.Stop();
            this.pchcServer.Start();
        }

        #endregion

        #region public method

        /// <summary>
        /// Send the corresponding response for the request message.
        /// </summary>
        /// <param name="responseCode">
        /// The ResponseCode specified the hosted cache server is INTERESTED or OK.
        /// 1 is for INTERESTED, 0 is for OK.
        /// </param>
        public void SendResponseMessage(int responseCode)
        {
            RESPONSE_MESSAGE responseMessage;
            RESPONSE_CODE responseCodeStack;

            if (responseCode == 0)
            {
                responseCodeStack = RESPONSE_CODE.OK;
            }
            else if (responseCode == 1)
            {
                responseCodeStack = RESPONSE_CODE.INTERESTED;
            }
            else
            {
                responseCodeStack = (RESPONSE_CODE)0x03;
            }

            responseMessage = this.pchcServer.CreateResponseMessage(responseCodeStack);

            this.pchcServer.SendPackage(responseMessage);
        }

        /// <summary>
        /// Send the Http Status Code 401.
        /// </summary>
        public void SendHttpStatusCode401()
        {
            this.pchcServer.SendHttpStatusCode401();
        }

        /// <summary>
        /// Receive a InitialOfferMessage from the Client.
        /// </summary>
        /// <param name="timeout">Waiting for specified timeout to receive the specified request.</param>
        /// <returns>Return the reveived InitialOfferMessage.</returns>
        public InitialOfferMessage ExpectInitialOfferMessage(TimeSpan timeout)
        {
            try
            {
                INITIAL_OFFER_MESSAGE intiailOfferMsgStack = this.pchcServer.ExpectInitialOfferMessage(
                    this.GetProperty("Environment.SecondContentClient.IPAddress"),
                    timeout);
                this.ValidateInitialOfferMessage(intiailOfferMsgStack);
                PchcBothRoleCaptureCode.ValidateTransport(this.pchcServer.HttpRequestUri.Scheme);
                this.ValidateTransport(this.pchcServer.HttpRequestMethod);
                this.ValidateClientInitialization(
                    this.pchcServer.HttpRequestUri.Host.ToString(),
                    this.pchcServer.HttpRequestUri.Port.ToString(),
                    this.pchcServer.HttpRequestUri.Scheme);

                InitialOfferMessage intiailOfferMsg = ClientHelper.ConvertFromStackForInitialOfferMsg(intiailOfferMsgStack);
                return intiailOfferMsg;
            }
            catch (NoINITIALOFFERMESSAGEReceivedException e)
            {
                throw new NoInitialOfferMessageException(e.Message);
            }
        }

        /// <summary>
        /// Receive a SegmentInfoMessage from the Client.
        /// </summary>
        /// <param name="timeout">Waiting for specified timeout to receive the specified request.</param>
        /// <returns>Return the reveived SegmentInfoMessage.</returns>
        public SegmentInfoMessage ExpectSegmentInfoMessage(TimeSpan timeout)
        {
            try
            {
                SEGMENT_INFO_MESSAGE segmentInfoMsgStack = this.pchcServer.ExpectSegmentInfoMessage(
                    this.GetProperty("Environment.SecondContentClient.IPAddress"),
                    timeout);

                this.ValidateSegmentInfoMessage(
                    segmentInfoMsgStack,
                    this.GetProperty("Environment.SecondContentClient.OSVersion"));

                PchcBothRoleCaptureCode.ValidateTransport(this.pchcServer.HttpRequestUri.Scheme);
                this.ValidateTransport(this.pchcServer.HttpRequestMethod);

                SegmentInfoMessage segmentInfoMsg = ClientHelper.ConvertFromStackForSegmentInfoMsg(segmentInfoMsgStack);
                return segmentInfoMsg;
            }
            catch (NoSEGMENTINFOMESSAGEReceivedException e)
            {
                throw new NoSegmentInfoMessageException(e.Message);
            }
        }

        #endregion

        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the 
        /// runtime from inside the finalizer and you should not reference 
        /// other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing">
        /// If disposing equals false, the method has been called by the 
        /// runtime from inside the finalizer and you should not reference 
        /// other objects. Only unmanaged resources can be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (this.pchcServer != null)
            {
                this.pchcServer.Dispose();
                this.pchcServer = null;
            }
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

