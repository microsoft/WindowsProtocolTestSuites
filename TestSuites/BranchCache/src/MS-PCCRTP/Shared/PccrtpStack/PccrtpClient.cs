// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;

    /// <summary>
    /// The test client.
    /// </summary>
    public class PccrtpClient : IDisposable
    {
        #region Fields

        /// <summary>
        /// Initialize an HTTP client transport.
        /// </summary>
        private HttpClientTransport httpClientTransport;

        /// <summary>
        /// The HTTP web response.
        /// </summary>
        private HttpWebResponse httpWebResponse;

        /// <summary>
        /// The dispose flag.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The HTTP response.
        /// </summary>
        private PccrtpResponse pccrtpResponse = new PccrtpResponse();

        /// <summary>
        /// The HTTP request.
        /// </summary>
        private PccrtpRequest pccrtpRequest = new PccrtpRequest();

        /// <summary>
        /// Indicates an instance of ILogPrinter.
        /// </summary>
        private ILogPrinter logger;

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the PccrtpClient class.
        /// </summary>
        public PccrtpClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PccrtpClient class.
        /// </summary>
        /// <param name="logger">An instance of ILogPrinter.</param>
        public PccrtpClient(ILogPrinter logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Finalizes an instance of the PccrtpClient class.
        /// </summary>
        ~PccrtpClient()
        {
            Dispose(false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create PCCRTP request message.
        /// </summary>
        /// <param name="serverAddress">The server address.</param>
        /// <param name="port">The port.</param>
        /// <param name="requestFileName">The request uri.</param>
        /// <param name="version">Version of branch cache.</param>
        /// <param name="missingData">True if this is a missing data request; false otherwise.</param>
        /// <returns>Returns the HTTP response.</returns>
        public PccrtpRequest CreatePccrtpRequest(
            string serverAddress,
            int port,
            string requestFileName,
            BranchCacheVersion version = BranchCacheVersion.V1,
            bool missingData = false)
        {
            this.pccrtpRequest.ServerAddress = serverAddress;
            this.pccrtpRequest.Port = port;
            this.pccrtpRequest.RequestFileName = requestFileName;

            Dictionary<string, string> tempHttpHeader = new Dictionary<string, string>();

            tempHttpHeader.Add(PccrtpConsts.AcceptEncodingHttpHeader, "peerdist");
            switch (version)
            {
                case BranchCacheVersion.V1:
                    tempHttpHeader.Add(PccrtpConsts.XP2PPeerDistHttpHeader, "Version=1.0" + (missingData ? ",MissingDataRequest=true" : ""));
                    break;
                case BranchCacheVersion.V2:
                    tempHttpHeader.Add(PccrtpConsts.XP2PPeerDistHttpHeader, "Version=1.1" + (missingData ? ",MissingDataRequest=true" : ""));
                    tempHttpHeader.Add(PccrtpConsts.XP2PPeerDistExHttpHeader, "MinContentInformation=1.0, MaxContentInformation=2.0");
                    break;
                default:
                    throw new NotImplementedException();
            }
            this.pccrtpRequest.HttpHeader = tempHttpHeader;
            return this.pccrtpRequest;
        }

        /// <summary>
        /// Create HTTP request message with custom HTTP header.
        /// </summary>
        /// <param name="serverAddress">The server address.</param>
        /// <param name="port">The port</param>
        /// <param name="fileName">The reuqest uri.</param>
        /// <param name="httpHeader">The HTTP header.</param>
        /// <returns>Returns the HTTP request.</returns>
        public PccrtpRequest CreateHttpRequest(
            string serverAddress, 
            int port, 
            string fileName, 
            Dictionary<string, string> httpHeader)
        {
            this.pccrtpRequest.ServerAddress = serverAddress;
            this.pccrtpRequest.Port = port;
            this.pccrtpRequest.RequestFileName = fileName;
            this.pccrtpRequest.HttpHeader = httpHeader;
            return this.pccrtpRequest;
        }

        /// <summary>
        /// Send HTTP request for the full content and receive HTTP response from the server.
        /// </summary>
        /// <param name="httpVersion">he HTTP version.</param>
        /// <param name="request">The HTTP reqeust.</param>
        /// <param name="timeOut">The time out to wait the response.</param>
        /// <returns>Returns the recieved HTTP response.</returns>
        public PccrtpResponse SendHttpRequest(
            HttpVersionType httpVersion,
            PccrtpRequest request,
            int timeOut)
        {
            byte[] payloadBuffer = null;

            if (this.logger != null)
            {
                this.httpClientTransport = new HttpClientTransport(
                    TransferProtocol.HTTP,
                    request.ServerAddress,
                    request.Port,
                    request.RequestFileName,
                    this.logger);
            }
            else
            {
                this.httpClientTransport = new HttpClientTransport(
                    TransferProtocol.HTTP,
                    request.ServerAddress,
                    request.Port,
                    request.RequestFileName);
            }

            if (HttpVersionType.HttpVersion10 == (HttpVersionType)httpVersion)
            {
                this.httpClientTransport.Send(HttpVersion.Version10, request.HttpHeader, null, HttpMethod.GET, timeOut);
            }
            else
            {
                // The default version of HTTP to use for the request is HTTP 1.1.
                this.httpClientTransport.Send(HttpVersion.Version11, request.HttpHeader, null, HttpMethod.GET, timeOut);
            }

            this.httpWebResponse = this.httpClientTransport.Receive(ref payloadBuffer);

            this.pccrtpResponse.DecodeHttpHeader(this.httpWebResponse);
            this.pccrtpResponse.PayloadData = payloadBuffer;
            this.pccrtpResponse.HttpResponse = this.httpWebResponse;

            return this.pccrtpResponse;
        }

        /// <summary>
        /// Send HTTP request for partial content and receive HTTP response from the server.
        /// </summary>
        /// <param name="httpVersion">The HTTP version.</param>
        /// <param name="request">The PCCRTP request.</param>
        /// <param name="timeOut">The number of milliseconds to wait before the request times out</param>
        /// <param name="rangeFrom">The start position at which to the requested data.</param>
        /// <param name="rangeTo">The end position at which to the requested data.</param>
        /// <returns>Returns the PCCRTP response.</returns>
        public PccrtpResponse SendHttpRequest(
            HttpVersionType httpVersion,
            PccrtpRequest request,
            int timeOut,
            int rangeFrom,
            int rangeTo)
        {
            byte[] payloadBuffer = null;

            if (this.logger != null)
            {
                this.httpClientTransport = new HttpClientTransport(
                    TransferProtocol.HTTP,
                    request.ServerAddress,
                    request.Port,
                    request.RequestFileName,
                    this.logger);
            }
            else
            {
                this.httpClientTransport = new HttpClientTransport(
                    TransferProtocol.HTTP,
                    request.ServerAddress,
                    request.Port,
                    request.RequestFileName);
            }

            if (HttpVersionType.HttpVersion10 == (HttpVersionType)httpVersion)
            {
                this.httpClientTransport.Send(
                    HttpVersion.Version10,
                    request.HttpHeader,
                    null,
                    HttpMethod.GET,
                    timeOut,
                    rangeFrom,
                    rangeTo);
            }
            else
            {
                // The default version of HTTP to use for the request is HTTP 1.1.
                this.httpClientTransport.Send(
                    HttpVersion.Version11,
                    request.HttpHeader,
                    null,
                    HttpMethod.GET,
                    timeOut,
                    rangeFrom,
                    rangeTo);
            }

            this.httpWebResponse = this.httpClientTransport.Receive(ref payloadBuffer);

            this.pccrtpResponse.DecodeHttpHeader(this.httpWebResponse);
            this.pccrtpResponse.PayloadData = payloadBuffer;
            this.pccrtpResponse.HttpResponse = this.httpWebResponse;

            this.pccrtpResponse.HttpResponse = this.httpWebResponse;

            return this.pccrtpResponse;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Release the managed and unmanaged resources. 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // Take this object out of the finalization queue of the GC:
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release resources. 
        /// </summary>
        /// <param name = "disposing">
        /// If disposing equals true, managed and unmanaged resources are disposed. 
        /// If false, only unmanaged resources can be disposed. 
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types:
                    if (this.httpClientTransport != null)
                    {
                        this.httpClientTransport.Dispose();
                        this.httpClientTransport = null;
                    }
                }

                // Call the appropriate methods to clean up unmanaged resources.
                // If disposing is false, only the following code is executed:
                this.disposed = true;
            }
        }

        #endregion
    }
}
