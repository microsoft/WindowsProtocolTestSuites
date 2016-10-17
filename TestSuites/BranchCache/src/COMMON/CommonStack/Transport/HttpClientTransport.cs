// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.CommonStack
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    /// <summary>
    /// HttpClientTransport class provides transport layer support.
    /// </summary>
    public class HttpClientTransport : IDisposable
    {
        #region  Fields

        /// <summary>
        /// Indicates the URI on the SUT requested by the client.
        /// </summary>
        private Uri httpUri;

        /// <summary>
        /// Indicates the HTTP request.
        /// </summary>
        private HttpWebRequest httpWebRequest;

        /// <summary>
        /// Indicates the HTTP response
        /// </summary>
        private HttpWebResponse httpWebResponse;

        /// <summary>
        /// Indicates the dispose flag.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Indicates the HTTP request body stream.
        /// </summary>
        private Stream httpBodyStream;

        /// <summary>
        /// Indicates an instance of ILogPrinter.
        /// </summary>
        private ILogPrinter logger;

        /// <summary>
        /// The max size of the buffer.
        /// </summary>
        private int bufferSize = 1024 * 10;

        private string domainName;
        private string userName;
        private string userPassword;
        
        #endregion 

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the HttpClientTransport class.
        /// Constructor of class HttpClientTransport to generate HTTP or HTTPs URI.
        /// </summary>
        /// <param name="transfer">Indicates the transfer protocol is HTTP or HTTPs.</param>
        /// <param name="serverAddress">The address of HTTP server.</param>
        /// <param name="port">The port number of HTTP server.</param>
        /// <param name="fileName">The requested file name.</param>
        /// <param name="logger">An instance of ILogPrinter.</param>
        public HttpClientTransport(
            TransferProtocol transfer,
            string serverAddress,
            int port,
            string fileName,
            ILogPrinter logger) :
            this(transfer, serverAddress, port, fileName, null, null, null)
        {
            if (null == logger)
            {
                throw new ArgumentNullException("logger", "The input parameter \"logger\" is null.");
            }

            this.logger = logger;
            this.logger.AddInfo("The HTTP client transport is created successfully.");
        }

        /// <summary>
        /// Initializes a new instance of the HttpClientTransport class.
        /// Constructor of class HttpClientTransport to generate HTTP or HTTPs URI.
        /// </summary>
        /// <param name="transfer">Indicates the transfer protocol is HTTP or HTTPs.</param>
        /// <param name="serverAddress">The address of HTTP server.</param>
        /// <param name="port">The port number of HTTP server.</param>
        /// <param name="fileName">The requested file name.</param>
        public HttpClientTransport(TransferProtocol transfer, string serverAddress, int port, string fileName)
            : this(transfer, serverAddress, port, fileName, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the HttpClientTransport class.
        /// Constructor of class HttpClientTransport to generate HTTP or HTTPs URI.
        /// </summary>
        /// <param name="transfer">Indicates the transfer protocol is HTTP or HTTPs.</param>
        /// <param name="serverAddress">The address of HTTP server.</param>
        /// <param name="port">The port number of HTTP server.</param>
        /// <param name="fileName">The requested file name.</param>
        /// <param name="domainName">The domain name.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="userPassword">The password.</param>
        public HttpClientTransport(
            TransferProtocol transfer, 
            string serverAddress, 
            int port, 
            string fileName, 
            string domainName,
            string userName, 
            string userPassword)
        {
            string uriString = string.Empty;
            switch (transfer)
            {
                case TransferProtocol.HTTP:
                    uriString = "http://";
                    break;
                case TransferProtocol.HTTPS:
                    uriString = "https://";
                    break;
                default:
                    break;
            }

            uriString = uriString + serverAddress + ":" + port.ToString() + "/" + fileName;
            this.httpUri = new Uri(uriString);
            this.domainName = domainName;
            this.userName = userName;
            this.userPassword = userPassword;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Send HTTP request for the full content from the specified server.
        /// </summary>
        /// <param name="httpVersion">The HTTP version.</param>
        /// <param name="httpHeader">The HTTP header.</param>
        /// <param name="httpBodyData">The HTTP body data.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="timeOut">The number of milliseconds to wait before the request times out.</param>
        public void Send(
            Version httpVersion,
            Dictionary<string, string> httpHeader,
            byte[] httpBodyData,
            HttpMethod httpMethod,
            int timeOut)
        {
            this.httpWebRequest = (HttpWebRequest)WebRequest.Create(this.httpUri);
            this.httpWebRequest.Timeout = timeOut;
            this.httpWebRequest.Method = httpMethod.ToString();

            if (userName != null)
            {
                this.httpWebRequest.Credentials = new NetworkCredential(userName, userPassword, domainName);
            }

            // The default version of HTTP to use for the request is HTTP 1.1.
            if (HttpVersion.Version10 == httpVersion)
            {
                this.httpWebRequest.ProtocolVersion = HttpVersion.Version10;
            }

            if (this.logger != null)
            {
                this.logger.AddDebug(string.Format(
                    @"The HTTP WEB request is created:
                    The requested HTTP URI is:{0}, the HTTP method used:{1}, tshe HTTP version used:{2}",
                    this.httpUri.ToString(),
                    httpMethod.ToString(),
                    this.httpWebRequest.ProtocolVersion.ToString()));
            }

            if (httpHeader != null)
            {
                foreach (string key in httpHeader.Keys)
                {
                    this.httpWebRequest.Headers.Add(key, httpHeader[key]);
                }
            }

            if (httpMethod != HttpMethod.GET && httpBodyData != null)
            {
                this.httpWebRequest.ContentLength = httpBodyData.Length;

                try
                {
                    this.httpBodyStream = this.httpWebRequest.GetRequestStream();
                    this.httpBodyStream.Write(httpBodyData, 0, httpBodyData.Length);
                    this.httpBodyStream.Close();
                }
                catch (ObjectDisposedException e)
                {
                    if (this.logger != null)
                    {
                        this.logger.AddDebug(
                            string.Format("Object disposed exception is catched, detailed information: {0}.", e.Message));
                    }

                    throw;
                }
            }

            if (this.logger != null)
            {
                this.logger.AddInfo("The HTTP full content request is created and sent successfully.");
            }
        }

        /// <summary>
        /// Send HTTP request for the partial content from the specified server.
        /// </summary>
        /// <param name="httpVersion">The HTTP version.</param>
        /// <param name="httpHeader">The HTTP header.</param>
        /// <param name="httpBodyData">The HTTP payload.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="timeOut">The number of milliseconds to wait before the request times out.</param>
        /// <param name="rangeFrom">The start position in bytes at which to the requested data.</param>
        /// <param name="rangeTo">The end position in bytes at which to the requested data.</param>
        public void Send(
            Version httpVersion,
            Dictionary<string, string> httpHeader,
            byte[] httpBodyData,
            HttpMethod httpMethod,
            int timeOut,
            int rangeFrom,
            int rangeTo)
        {
            this.httpWebRequest = (HttpWebRequest)WebRequest.Create(this.httpUri);
            this.httpWebRequest.AddRange(rangeFrom, rangeTo);
            this.httpWebRequest.Timeout = timeOut;
            this.httpWebRequest.Method = httpMethod.ToString();

            if (userName != null)
            {
                this.httpWebRequest.Credentials = new NetworkCredential(userName, userPassword, domainName);
            }

            // The default version of HTTP to use for the request is HTTP 1.1.
            if (HttpVersion.Version10 == httpVersion)
            {
                this.httpWebRequest.ProtocolVersion = HttpVersion.Version10;
            }

            if (this.logger != null)
            {
                this.logger.AddDebug(string.Format(
                    @"The HTTP WEB request is created:
                    The requested HTTP URI is:{0}, the HTTP method used:{1}, the HTTP version used:{2}",
                    this.httpUri.ToString(),
                    httpMethod.ToString(),
                    this.httpWebRequest.ProtocolVersion.ToString()));
            }

            if (httpHeader != null)
            {
                foreach (string key in httpHeader.Keys)
                {
                    this.httpWebRequest.Headers.Add(key, httpHeader[key]);
                }
            }

            if (httpMethod != HttpMethod.GET && httpBodyData != null)
            {
                this.httpWebRequest.ContentLength = httpBodyData.Length;
                try
                {
                    this.httpBodyStream = this.httpWebRequest.GetRequestStream();
                    this.httpBodyStream.Write(httpBodyData, 0, httpBodyData.Length);
                    this.httpBodyStream.Close();
                }
                catch (ObjectDisposedException e)
                {
                    if (this.logger != null)
                    {
                        this.logger.AddDebug(
                            string.Format("Object disposed exception is catched, detailed information: {0}.", e.Message));
                    }

                    throw;
                }
                catch (WebException e)
                {
                    if (this.logger != null)
                    {
                        this.logger.AddDebug(
                            string.Format("Web exception is catched, detailed information: {0}.", e.Message));
                    }

                    throw;
                }
            }

            if (this.logger != null)
            {
                this.logger.AddInfo("The HTTP partial content request is created and sent successfully.");
            }
        }

        /// <summary>
        /// Receive HTTP response from the specified server.
        /// </summary>
        /// <param name="payloadData">Return the whole HTTP response payload.</param> 
        /// <returns>Returns the HTTP web response.</returns>
        public HttpWebResponse Receive(ref byte[] payloadData)
        {
            if (this.logger != null)
            {
                this.logger.AddInfo("Start to receive HTTP response from server.");
            }

            int offset = 0;

            try
            {
                this.httpWebResponse = (HttpWebResponse)this.httpWebRequest.GetResponse();
                Stream responseStream = this.httpWebRequest.GetResponse().GetResponseStream();

                if (this.httpWebResponse.Headers.GetValues("Content-Length") != null)
                {
                    payloadData = new byte[this.httpWebResponse.ContentLength];

                    while (true)
                    {
                        offset += responseStream.Read(payloadData, offset, payloadData.Length - offset);
                        responseStream = this.httpWebRequest.GetResponse().GetResponseStream();
                        if (offset == payloadData.Length)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    List<byte> tempBuffer = new List<byte>();
                    byte[] payloadBuffer = new byte[this.bufferSize];

                    int readSize = 0;
                    while (true)
                    {
                        int tempIndex = 0;
                        readSize = responseStream.Read(payloadBuffer, 0, payloadBuffer.Length);
                        if (readSize == 0)
                        {
                            break;
                        }
                        else
                        {
                            tempBuffer.AddRange(GetBytes(payloadBuffer, ref tempIndex, readSize));
                        }
                    }

                    payloadData = tempBuffer.ToArray();
                }

                responseStream.Close();
                responseStream.Dispose();
            }
            catch (WebException e)
            {
                if (this.logger != null)
                {
                    this.logger.AddDebug(
                        string.Format("Unexpected exception is thrown, detailed information: {0}.", e.Message));
                }

                throw;
            }

            if (this.logger != null)
            {
                this.logger.AddInfo("The HTTP response is received successfully.");
            }

            return this.httpWebResponse;
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
                    if (this.httpWebRequest != null)
                    {
                        this.httpWebRequest.KeepAlive = false;
                        this.httpWebRequest = null;
                    }

                    if (this.httpWebResponse != null)
                    {
                        this.httpWebResponse.Close();
                        this.httpWebResponse = null;
                    }
                }
            }

            // Call the appropriate methods to clean up unmanaged resources.
            // If disposing is false, only the following code is executed:
            this.disposed = true;
        }

        #endregion

        #region Private method

        /// <summary>
        /// Get bytes from bytes.
        /// </summary>
        /// <param name="buffer">the buffer stores the value.</param>
        /// <param name="index">the index of start to parse</param>
        /// <param name="count">count of bytes</param>
        /// <returns>Parsed UInt16 value</returns>
        private static byte[] GetBytes(byte[] buffer, ref int index, int count)
        {
            byte[] byteReturn = null;

            if (count > 0)
            {
                byteReturn = new byte[count];

                Array.Copy(buffer, index, byteReturn, 0, byteReturn.Length);
                index += byteReturn.Length;
            }

            return byteReturn;
        }

        #endregion
    }
}

