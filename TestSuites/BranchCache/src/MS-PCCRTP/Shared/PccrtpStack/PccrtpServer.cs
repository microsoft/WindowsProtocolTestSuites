// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;

    /// <summary>
    /// The test server.
    /// </summary>
    public class PccrtpServer : IDisposable
    {
        #region Fields

        /// <summary>
        /// The ContentEncoding header field in PCCRTP message.
        /// </summary>
        private const string CONTENTENCODING = "Content-Encoding";

        /// <summary>
        /// The X-P2P-PeerDist header field in PCCRTP message.
        /// </summary>
        private const string XP2PPEERDIST = "X-P2P-PeerDist";

        /// <summary>
        /// Initialize an HTTP server transport .
        /// </summary>
        private HttpServerTransport httpServerTransport;

        /// <summary>
        /// The http listener context.
        /// </summary>
        private HttpListenerContext httpListenerContext;

        /// <summary>
        /// Initialize an instance of PccrtpRequest class.
        /// </summary>
        private PccrtpRequest pccrtpRequest = new PccrtpRequest();

        /// <summary>
        /// The dispose flag.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Indicates an instance of ILogPrinter.
        /// </summary>
        private ILogPrinter logger;

        #endregion

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the PccrtpServer class with specified logger instance.
        /// </summary>
        /// <param name="listenPort">Indicates the PCCRTP server's listen port.</param>
        /// <param name="addressType">Indicates the PCCRTP server's address type is Ipv4 or Ipv6.</param>
        /// <param name="logger">An instance of ILogPrinter.</param>
        public PccrtpServer(int listenPort, IPAddressType addressType, ILogPrinter logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger", "The input parameter \"logger\" is null.");
            }

            this.logger = logger;
            this.httpServerTransport = new HttpServerTransport(
                TransferProtocol.HTTP,
                listenPort,
                addressType,
                null,
                this.logger);

            this.httpServerTransport.HttpRequestEventHandle += new EventHandler<HttpRequestEventArg>(this.OnHttpRequestMessageReceived);
        }

        /// <summary>
        /// Initializes a new instance of the PccrtpServer class with specified logger instance.
        /// </summary>
        /// <param name="listenPort">Indicates the PCCRTP server's listen port.</param>
        /// <param name="addressType">Indicates the PCCRTP server's address type is Ipv4 or Ipv6.</param>
        public PccrtpServer(int listenPort, IPAddressType addressType)
        {
            this.httpServerTransport = new HttpServerTransport(
                TransferProtocol.HTTP,
                listenPort,
                addressType,
                null);

            this.httpServerTransport.HttpRequestEventHandle += new EventHandler<HttpRequestEventArg>(this.OnHttpRequestMessageReceived);
        }

        /// <summary>
        /// Finalizes an instance of the PccrtpServer class.
        /// </summary>
        ~PccrtpServer()
        {
            Dispose(false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The method is used to start PCCRTP server and start to listen request input.
        /// </summary>
        public void StartServer()
        {
            if (this.httpServerTransport.RunState != State.Started)
            {
                this.httpServerTransport.StartHttpServer();

                if (this.logger != null)
                {
                    this.logger.AddInfo("The PCCRTP server is started.");
                }

                return;
            }
            else
            {
                if (this.logger != null)
                {
                    this.logger.AddInfo("The PCCRTP server is already started.");
                }
            }
        }

        /// <summary>
        /// The method is used to receive the PCCRTP request.
        /// </summary>
        /// <param name="timeOut">The timeout to wait the response.</param>
        /// <returns>Returns the received PCCRTP request.</returns>
        public PccrtpRequest ReceivePccrtpRequest(TimeSpan timeOut)
        {
            DateTime startTime = DateTime.Now;

            // Waiting for HTTP request until timeout.
            while (this.httpListenerContext == null)
            {
                // Waiting 100 milliseconds for the request.
                Thread.Sleep(100);
                if ((DateTime.Now - startTime).TotalMilliseconds > timeOut.TotalMilliseconds)
                {
                    if (this.logger != null)
                    {
                        this.logger.AddWarning(string.Format(
                            "Waiting for {0} milliseconds, no expected HTTP request is received.",
                            timeOut.TotalMilliseconds));
                    }

                    throw new TimeoutException(
                        string.Format(
                        "Waiting for {0} milliseconds, no expected HTTP request is received.",
                        timeOut.TotalMilliseconds));
                }
            }

            return this.pccrtpRequest;
        }

        /// <summary>
        /// Generate PCCRTP response message to reply the client request.
        /// </summary>
        /// <param name="resourceLocator">The client request URI.</param>
        /// <param name="serverSecret">The server secret set on the server endpoint.</param>
        /// <returns>Returns the PCCRTP response message.</returns>
        public PccrtpResponse GenerateResponseMessage(string resourceLocator, string serverSecret)
        {
            PccrtpResponse pccrtpResponse = new PccrtpResponse();
            Dictionary<string, string> tempHttpHeader = new Dictionary<string, string>();
            HashGeneration hashHelp = new HashGeneration(serverSecret, dwHashAlgo_Values.SHA256);
            byte[] fileData = PccrcUtility.ReadFile(resourceLocator);
            Content_Information_Data_Structure contentInfo = hashHelp.GenerateContentInformation(fileData);

            tempHttpHeader.Add(CONTENTENCODING, "peerdist");
            tempHttpHeader.Add(XP2PPEERDIST, "Version=1.0, ContentLength=" + fileData.Length);
            pccrtpResponse.HttpHeader = tempHttpHeader;

            pccrtpResponse.PayloadData = contentInfo.ToByteArray();

            return pccrtpResponse;
        }

        /// <summary>
        /// The method is used to send PCCRTP response message.
        /// </summary>
        /// <param name="response">The PCCRTP response message.</param>
        public void SentPccrtpResponse(PccrtpResponse response)
        {
            this.httpServerTransport.Send(
                response.HttpHeader,
                response.PayloadData,
                this.httpListenerContext);

            this.httpListenerContext = null;
        }

        /// <summary>
        /// The method is used to stop the PCCRTP server and start to listen request input.
        /// </summary>
        public void StopServer()
        {
            if (this.httpServerTransport.RunState != State.Stopped)
            {
                // Stop HTTP server.
                this.httpServerTransport.StopHttpServer();

                if (this.logger != null)
                {
                    this.logger.AddInfo("The PCCRTP server is stopped.");
                }
            }
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
                    if (this.httpServerTransport != null)
                    {
                        this.httpServerTransport.Dispose();
                        this.httpServerTransport = null;
                    }

                    if (this.httpListenerContext != null)
                    {
                        this.httpListenerContext = null;
                    }

                    if (this.logger != null)
                    {
                        this.logger = null;
                    }
                }
            }

            // Call the appropriate methods to clean up unmanaged resources.
            // If disposing is false, only the following code is executed:
            this.disposed = true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// The handle to HTTP request message.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The MessageReceivedEvent args.</param>
        private void OnHttpRequestMessageReceived(object sender, HttpRequestEventArg e)
        {
            lock (this)
            {
                this.pccrtpRequest.HttpRequest = e.ListenerContext.Request;
                this.httpListenerContext = e.ListenerContext;
            }
        }

        #endregion
    }
}
