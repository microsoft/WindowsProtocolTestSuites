// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrtp
{
    using System;
    using Microsoft.Protocol.TestSuites;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp;

    /// <summary>
    /// PCCRTP server adapter implementation to test server endpoint.
    /// </summary>
    public partial class PCCRTPServerAdapter : ManagedAdapterBase, IPCCRTPServerAdapter
    {
        #region Fields

        #region Public field

        /// <summary>
        /// The standard segment size.
        /// </summary>
        public const int STANDARDSEGMENTSIZE = 32 * 1024 * 1024;

        /// <summary>
        /// The Content-Range header field in PCCRTP message.
        /// </summary>
        public const string CONTENTRANGE = "Content-Range";

        /// <summary>
        /// The X-P2P-PeerDist header field in PCCRTP message.
        /// </summary>
        public const string XP2PPEERDIST = "X-P2P-PeerDist";

        /// <summary>
        /// The Content-Encoding header field in PCCRTP message.
        /// </summary>
        public const string CONTENTENCODING = "Content-Encoding";

        /// <summary>
        /// The Connection header field in PCCRTP message.
        /// </summary>
        public const string CONNECTION = "Connection";

        #endregion

        #region Private field

        /// <summary>
        /// The standard block size.
        /// </summary>
        private const int STANDARDBBLOCKSIZE = 65536;

        /// <summary>
        /// The Accept-Ranges header field in PCCRTP message.
        /// </summary>
        private const string ACCEPTRANGES = "Accept-Ranges";

        /// <summary>
        /// The Content-Length header field in PCCRTP message.
        /// </summary>
        private const string CONTENTLENGTH = "Content-Length";

        /// <summary>
        /// The Operation System (OS) version that test suite run on.
        /// </summary>
        private static OSVersion sutOsVersion;

        /// <summary>
        /// An instance of class PccrtpClient which mocks the client functionality of PCCRTP. 
        /// </summary>
        private PccrtpClient pccrtpClientStack;

        #endregion

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets sutOsVersion.
        /// </summary>
        public static OSVersion SutOsVersion
        {
            get { return sutOsVersion; }
            set { sutOsVersion = value; }
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
            this.pccrtpClientStack = new PccrtpClient(new Logger(testSite));
            this.Site.DefaultProtocolDocShortName = "MS-PCCRTP";
            SutOsVersion = (OSVersion)Enum.Parse(
                typeof(OSVersion),
                this.GetProperty("Environment.ContentServer.OSVersion"),
                true);
        }

        #endregion

        #region Interface Methods

        /// <summary>
        /// Send a PCCRTP request message and receive a PCCRTP response message.
        /// </summary>
        /// <param name="httpVersion">Indicates the HTTP version type used.</param>
        /// <param name="isRequestPartialContent">Indicates it is requesting partical content or not.</param>
        /// <param name="uri">Indicates the URI on the SUT requested by the client.</param>
        /// <returns>Return the PCCRTP response message received.</returns>
        public PccrtpResponse SendPccrtpRequestMessage(
            HttpVersionType httpVersion,
            bool isRequestPartialContent,
            string uri)
        {
            PccrtpRequest pccrtpRequest = new PccrtpRequest();
            PccrtpResponse pccrtpResponse = new PccrtpResponse();
            string serverAddress = this.GetProperty("Environment.ContentServer.MachineName");
            int port = int.Parse(this.GetProperty("Environment.ContentServer.HTTP.Port"));
            int timeOut = (int)TimeSpan.FromSeconds(
                double.Parse(this.GetProperty("PCCRTP.Protocol.TimeOut"))).TotalMilliseconds;
            int rangeFrom = int.Parse(this.GetProperty("PCCRTP.Protocol.RangeFrom"));
            int rangeTo = int.Parse(this.GetProperty("PCCRTP.Protocol.RangeTo"));

            if (isRequestPartialContent)
            {
                pccrtpRequest = this.pccrtpClientStack.CreatePccrtpRequest(serverAddress, port, uri);
                pccrtpResponse = this.pccrtpClientStack.SendHttpRequest(
                    httpVersion,
                    pccrtpRequest,
                    timeOut,
                    rangeFrom,
                    rangeTo);
            }
            else
            {
                pccrtpRequest = this.pccrtpClientStack.CreatePccrtpRequest(serverAddress, port, uri);
                pccrtpResponse = this.pccrtpClientStack.SendHttpRequest(httpVersion, pccrtpRequest, timeOut);
            }

            if (pccrtpResponse.HttpResponse.ContentEncoding.Equals("peerdist"))
            {
                PccrtpBothRoleCapture.VerifyTransport(pccrtpResponse.HttpResponse.ProtocolVersion.ToString());
                PccrtpBothRoleCapture.VerifyPccrtpCommonHeader(pccrtpResponse.HttpHeader);
                this.VerifyPccrtpResponse(pccrtpResponse);
                this.VerifyContentInfomationStructure(pccrtpResponse);
            }

            return pccrtpResponse;
        }

        #endregion

        #region ManagedAdapterBase Members

        /// <summary>
        /// Reset the adapter.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
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
            if (this.pccrtpClientStack != null)
            {
                this.pccrtpClientStack.Dispose();
            }

            base.Dispose(disposing);
            this.pccrtpClientStack = null;
        }

        #endregion

        #region Private Methods

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

        #endregion
    }
}
