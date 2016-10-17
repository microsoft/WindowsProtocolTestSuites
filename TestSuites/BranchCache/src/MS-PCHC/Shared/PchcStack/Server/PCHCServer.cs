// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;

    /// <summary>
    /// The PCHC server class which is used to exchange the pchc message between the client.
    /// </summary>
    public class PCHCServer : IDisposable
    {
        #region field

        /// <summary>
        /// Pchc resource;
        /// </summary>
        private const string PCHCRESOURCE = "C574AC30-5794-4AEE-B1BB-6651C5315029";

        /// <summary>
        /// The HttpServerTransport instance is used to as the http server process.
        /// </summary>
        private HttpServerTransport httpServerTransport;

        /// <summary>
        /// The http request payload.
        /// </summary>
        private byte[] httpRequestPayload;

        /// <summary>
        /// The max size of the buffer.
        /// </summary>
        private int bufferSize = 1024 * 10;

        /// <summary>
        /// The http request.
        /// </summary>
        private HttpListenerRequest httpRequest;

        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The http request uri.
        /// </summary>
        private Uri httpRequestUri;

        /// <summary>
        /// The http method is used in the request from the client.
        /// </summary>
        private string httpRequestMethod;

        /// <summary>
        /// The locker.
        /// </summary>
        private object locker = new object();

        /// <summary>
        /// The logger
        /// </summary>
        private ILogPrinter logger;

        /// <summary>
        /// The queue of the incoming initial offer message.
        /// </summary>
        private Queue<HttpListenerContext> initialQueue = new Queue<HttpListenerContext>();

        /// <summary>
        /// The queue of the incoming segment info message.
        /// </summary>
        private Queue<HttpListenerContext> segmentQueue = new Queue<HttpListenerContext>();

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Initializes a new instance of the PCHCServer class
        /// with the default transport type: Https; hosted cache server listen port: 443; IPAddress type: IPv4;
        /// </summary>
        public PCHCServer()
            : this(TransferProtocol.HTTPS, 443, IPAddressType.IPv4)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PCHCServer class
        /// with specified logger instance and the default transport type: Https; hosted cache server listen port: 443; IPAddress type: IPv4;
        /// </summary>
        /// <param name="logger">The logger.</param>
        public PCHCServer(ILogPrinter logger)
            : this(TransferProtocol.HTTPS, 443, IPAddressType.IPv4, logger)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PCHCServer class 
        /// with the specified osted Cache Mode Listen Port and IPAddress type.
        /// </summary>
        /// <param name="transferProtocol">The transport type will be used</param>
        /// <param name="hostedCacheModeListenPort">
        /// The specified BranchCache service's listen port in hosted cache mode.
        /// </param>
        /// <param name="ipaddressType">The IP address type.</param>
        public PCHCServer(TransferProtocol transferProtocol, int hostedCacheModeListenPort, IPAddressType ipaddressType)
        {
            this.httpServerTransport = new HttpServerTransport(
                transferProtocol,
                hostedCacheModeListenPort,
                ipaddressType,
                PCHCRESOURCE);

            this.httpServerTransport.HttpRequestEventHandle += new EventHandler<HttpRequestEventArg>(this.ReceiveHttpRequest);
        }

        /// <summary>
        /// Initializes a new instance of the PCHCServer class 
        /// with the specified osted Cache Mode Listen Port and IPAddress type.
        /// </summary>
        /// <param name="transferProtocol">The transport type will be used</param>
        /// <param name="hostedCacheModeListenPort">
        /// The specified BranchCache service's listen port in hosted cache mode.
        /// </param>
        /// <param name="ipaddressType">The IP address type.</param>
        /// <param name="logger">The logger.</param>
        public PCHCServer(
            TransferProtocol transferProtocol,
            int hostedCacheModeListenPort,
            IPAddressType ipaddressType,
            ILogPrinter logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger", "The input parameter \"logger\" is null.");
            }

            this.logger = logger;

            this.httpServerTransport = new HttpServerTransport(
                transferProtocol,
                hostedCacheModeListenPort,
                ipaddressType,
                PCHCRESOURCE,
                logger);

            this.httpServerTransport.HttpRequestEventHandle
                += new EventHandler<HttpRequestEventArg>(this.ReceiveHttpRequest);

            this.logger.AddDebug(@"Register the delegate EventHandle with generic parameter HttpRequestEverntArg 
                and the handle method is ReceiveHttpRequest.");
        }

        /// <summary>
        /// Finalizes an instance of the PCHCServer class.
        /// This destructor will run only if the Dispose method 
        /// does not get called.
        /// It gives your base class the opportunity to finalize.
        /// Do not provide destructors in types derived from this class.
        /// </summary>
        ~PCHCServer()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #endregion

        #region property

        /// <summary>
        /// Gets the http method which is used in the request from the client 
        /// </summary>
        public string HttpRequestMethod
        {
            get { return this.httpRequestMethod; }
        }

        /// <summary>
        /// Gets the http request uri.
        /// </summary>
        public Uri HttpRequestUri
        {
            get { return this.httpRequestUri; }
        }

        #endregion

        #region public mothod

        /// <summary>
        /// Start the http server process to listen and receive and http request.
        /// </summary>
        public void Start()
        {
            if (this.httpServerTransport.RunState != State.Started)
            {
                this.httpServerTransport.StartHttpServer();

                if (this.logger != null)
                {
                    this.logger.AddInfo("The http server process is started.");
                }

                return;
            }
            else
            {
                if (this.logger != null)
                {
                    this.logger.AddInfo("The http server process is already started.");
                }
            }
        }
       
        /// <summary>
        /// Expect the INITIAL_OFFER_MESSAGE request from the client.
        /// </summary>
        /// <param name="ipaddress">The expected ipAddress of the remote endpoint which send request.</param>
        /// <param name="timeout">The waiting timeout.</param>
        /// <returns>Return the INITIAL_OFFER_MESSAGE received.</returns>
        /// <exception cref="NoINITIALOFFERMESSAGEReceivedException">
        /// Throw when no INITIAL_OFFER_MESSAGE request from the client.
        /// </exception>
        public INITIAL_OFFER_MESSAGE ExpectInitialOfferMessage(string ipaddress, TimeSpan timeout)
        {
            // Make sure the timeout is not less than 1000 milliseconds.
            if (timeout.TotalMilliseconds < 1000)
            {
                if (this.logger != null)
                {
                    this.logger.AddWarning(string.Format(
                        "The timeout total milliseconds: {0} from the param \"timeout\" is too small.",
                        timeout.TotalMilliseconds));
                }

                // Set the timeout to the default 5000 milliseconds.
                timeout = TimeSpan.FromMilliseconds(double.Parse("5000"));

                if (this.logger != null)
                {
                    this.logger.AddInfo(string.Format(
                        "The timeout total milliseconds from the param \"timeout\" is set to the default value: {0} milliseconds.",
                        timeout.TotalMilliseconds));
                }
            }

            DateTime startTime = DateTime.Now;

            // Waiting for the initial offer message request until timeout.
            while (this.initialQueue.Count == 0)
            {
                // Waiting 100 milliseconds for the reqest.
                Thread.Sleep(100);
                if ((DateTime.Now - startTime).TotalMilliseconds > timeout.TotalMilliseconds)
                {
                    if (this.logger != null)
                    {
                        this.logger.AddWarning(string.Format(
                            "Waiting for {0} milliseconds, no expected INITIAL_OFFER_MESSAGE is received.",
                            timeout.TotalMilliseconds));
                    }

                    throw new NoINITIALOFFERMESSAGEReceivedException(string.Format(
                        "Waiting for {0} milliseconds, no expected INITIAL_OFFER_MESSAGE is received.",
                        timeout.TotalMilliseconds));
                }
            }

            lock (this.initialQueue)
            {
                while (!this.initialQueue.Peek().Request.RemoteEndPoint.Address.Equals(IPAddress.Parse(ipaddress)))
                {
                    this.initialQueue.Dequeue();
                    if (this.initialQueue.Count == 0)
                    {
                        throw new InvalidOperationException();
                    }
                }

                this.httpRequest = this.initialQueue.Peek().Request;
            }

            try
            {
                this.DecomposeHttpRequest(this.httpRequest);
            }
            catch (ObjectDisposedException e)
            {
                if (this.logger != null)
                {
                    this.logger.AddWarning(
                            string.Format("Object disposed exception is catched, detailed information: {0}.", e.Message));
                }
                else
                {
                    throw;
                }
            }

            return DecodeMessage.DecodeInitialOfferMessage(this.httpRequestPayload);
        }

        /// <summary>
        /// Expect the SEGMENT_INFO_MESSAGE request from the client.
        /// </summary>
        /// <param name="ipaddress">The expected ipAddress of the remote endpoint which send request.</param>
        /// <param name="timeout">The waiting timeout.</param>
        /// <returns>Return the SEGMENT_INFO_MESSAGE received.</returns>
        /// <exception cref="NoSEGMENTINFOMESSAGEReceivedException">
        /// Throw when no SEGMENT_INFO_MESSAGE request from the client.
        /// </exception>
        public SEGMENT_INFO_MESSAGE ExpectSegmentInfoMessage(string ipaddress, TimeSpan timeout)
        {
            // Make sure the timeout is not less than 1000 milliseconds.
            if (timeout.TotalMilliseconds < 1000)
            {
                if (this.logger != null)
                {
                    this.logger.AddWarning(string.Format(
                        "The timeout total milliseconds: {0} from the param \"timeout\" is too small.",
                        timeout.TotalMilliseconds));
                }

                // Set the timeout to the default 5000 milliseconds.
                timeout = TimeSpan.FromMilliseconds(double.Parse("5000"));

                if (this.logger != null)
                {
                    this.logger.AddInfo(string.Format(
                        "The timeout total milliseconds from the param \"timeout\" is set to the default value: {0} milliseconds.",
                        timeout.TotalMilliseconds));
                }
            }

            DateTime startTime = DateTime.Now;

            while (this.segmentQueue.Count == 0)
            {
                // Waiting for the reqest.
                Thread.Sleep(100);
                if ((DateTime.Now - startTime).TotalMilliseconds > timeout.TotalMilliseconds)
                {
                    throw new NoSEGMENTINFOMESSAGEReceivedException(string.Format(
                        "Waiting for {0} milliseconds, no expected SEGMENT_INFO_MESSAGE is received.",
                        timeout.TotalMilliseconds));
                }
            }

            while (!this.segmentQueue.Peek().Request.RemoteEndPoint.Address.Equals(IPAddress.Parse(ipaddress)))
            {
                this.segmentQueue.Dequeue();
                if (this.segmentQueue.Count == 0)
                {
                    throw new InvalidOperationException();
                }

                this.httpRequest = this.segmentQueue.Peek().Request;
            }

            try
            {
                this.DecomposeHttpRequest(this.httpRequest);
            }
            catch (ObjectDisposedException e)
            {
                if (this.logger != null)
                {
                    this.logger.AddWarning(
                            string.Format("Object disposed exception is catched, detailed information: {0}.", e.Message));
                }
                else
                {
                    throw;
                }
            }

            return DecodeMessage.DecodeSegmentInfoMessage(this.httpRequestPayload);
        }

        /// <summary>
        /// Create pchc response message.
        /// </summary>
        /// <param name="responseCode">
        /// The response code indicates the hosted cache server response to the client request message
        /// </param>
        /// <returns>Return a pchc response message.</returns>
        public RESPONSE_MESSAGE CreateResponseMessage(RESPONSE_CODE responseCode)
        {
            RESPONSE_MESSAGE responseMessage;

            // Size (4 bytes):  Total message size in bytes, excluding this field.
            // ResponseCode (1 byte):  A code that indicates the server response to the client request message.
            responseMessage.TransportHeader.Size = 1;
            responseMessage.ResponseCode = responseCode;

            if (this.logger != null)
            {
                this.logger.AddDebug(string.Format(
                    "Pchc response message is created with reposne code: {0}",
                    (RESPONSE_CODE)responseCode));
            }

            return responseMessage;
        }

        /// <summary>
        /// Send Reponse Message to the client.
        /// </summary>
        /// <param name="responseMessage">The response message.</param>
        public void SendPackage(RESPONSE_MESSAGE responseMessage)
        {
            this.SendByte(EncodeMessage.EncodeResponseMessage(responseMessage));

            if (this.logger != null)
            {
                this.logger.AddDebug("Pchc response message is sent successfully.");
            }
        }

        /// <summary>
        /// Send http status code 401.
        /// </summary>
        public void SendHttpStatusCode401()
        {
            Dictionary<string, string> responseHeader = new Dictionary<string, string>();
            responseHeader.Add("WWW-Authenticate", "Negotiate");

            try
            {
                lock (this.initialQueue)
                {
                    lock (this.segmentQueue)
                    {
                        this.httpServerTransport.Send(
                            (int)HttpStatusCode.Unauthorized,
                            responseHeader,
                            null,
                            this.initialQueue.Count != 0 ? this.initialQueue.Peek() : this.segmentQueue.Peek(),
                            false);
                    }
                }
            }
            catch (ObjectDisposedException e)
            {
                if (this.logger != null)
                {
                    this.logger.AddWarning(
                        string.Format("Object disposed exception is catched, detailed information: {0}.", e.Message));
                }
                else
                {
                    throw;
                }
            }

            if (this.logger != null)
            {
                this.logger.AddDebug("Http status code 401 is sent successfully.");
            }

            lock (this.initialQueue)
            {
                lock (this.segmentQueue)
                {
                    // Consume the http listener context after the response is sent.
                    this.initialQueue.Clear();
                    this.segmentQueue.Clear();
                }
            }
        }

        /// <summary>
        /// Stop the http server process and Release this object resources.
        /// </summary>
        public void Stop()
        {
            lock (this.initialQueue)
            {
                lock (this.segmentQueue)
                {
                    // Release this object resources.
                    this.initialQueue.Clear();
                    this.segmentQueue.Clear();
                }
            }

            this.httpRequest = null;
            this.httpRequestPayload = null;

            // Stop the http server process.
            if (this.httpServerTransport.RunState != State.Stopped)
            {
                this.httpServerTransport.StopHttpServer();
            }
        }

        /// <summary>
        /// Implement IDisposable.
        /// Do not make this method virtual.
        /// A derived class should not be able to override this method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        #endregion

        #region private method

        /// <summary>
        /// THe handle for the received http request event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">THe http request event.</param>
        private void ReceiveHttpRequest(object sender, HttpRequestEventArg e)
        {
            lock (this.locker)
            {
                HttpListenerRequest request = e.ListenerContext.Request;
                this.DecomposeHttpRequest(request);
                PCHC_MESSAGE_TYPE msgType = DecodeMessage.GetMessageType(this.httpRequestPayload);
                if (msgType == PCHC_MESSAGE_TYPE.INITIAL_OFFER_MESSAGE)
                {
                    lock (this.initialQueue)
                    {
                        this.initialQueue.Enqueue(e.ListenerContext);
                    }
                }
                else if (msgType == PCHC_MESSAGE_TYPE.SEGMENT_INFO_MESSAGE)
                {
                    lock (this.segmentQueue)
                    {
                        this.segmentQueue.Enqueue(e.ListenerContext);
                    }
                }
            }
        }

        /// <summary>
        /// Get the request body byte array from the specified http request.
        /// </summary>
        /// <param name="httpListenerRequest">The specified http request.</param>
        /// <exception cref="ObjectDisposedException">
        /// Throw when an operation is performed on a disposed object.
        /// </exception>
        private void DecomposeHttpRequest(HttpListenerRequest httpListenerRequest)
        {
            if (httpListenerRequest == null)
            {
                throw new ArgumentNullException("httpListenerRequest");
            }

            this.httpRequestUri = httpListenerRequest.Url;
            this.httpRequestMethod = httpListenerRequest.HttpMethod;

            try
            {
                Stream requestStream = httpListenerRequest.InputStream;

                byte[] payloadBuffer = new byte[this.bufferSize];
                int readSize = 0;
                while (true)
                {
                    int tempIndex = 0;
                    readSize = requestStream.Read(payloadBuffer, 0, payloadBuffer.Length);
                    if (readSize == 0)
                    {
                        break;
                    }
                    else
                    {
                        this.httpRequestPayload = DecodeMessage.GetBytes(payloadBuffer, ref tempIndex, readSize);
                    }
                }

                requestStream.Close();
            }
            catch (ObjectDisposedException e)
            {
                if (this.logger != null)
                {
                    this.logger.AddWarning(
                        string.Format("Object disposed exception is catched, detailed information: {0}.", e.Message));
                }
                else
                {
                    throw;
                }
            }

            if (this.logger != null)
            {
                this.logger.AddDebug("Unmarshal PCHC request message is successfully.");
            }
        }

        /// <summary>
        /// Send the response payload.
        /// </summary>
        /// <param name="reponseMessagePayload">The response message payload.</param>
        private void SendByte(byte[] reponseMessagePayload)
        {
            try
            {
                lock (this.initialQueue)
                {
                    lock (this.segmentQueue)
                    {
                        this.httpServerTransport.Send(
                            null,
                            reponseMessagePayload,
                            this.initialQueue.Count != 0 ? this.initialQueue.Peek() : this.segmentQueue.Peek());
                    }
                }
            }
            catch (ObjectDisposedException e)
            {
                if (this.logger != null)
                {
                    this.logger.AddWarning(
                        string.Format("Object disposed exception is catched, detailed information: {0}.", e.Message));
                }
                else
                {
                    throw;
                }
            }

            lock (this.initialQueue)
            {
                lock (this.segmentQueue)
                {
                    // Consume the http listener context after the response is sent.
                    this.initialQueue.Clear();
                    this.segmentQueue.Clear();
                }
            }
        }

        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the 
        /// runtime from inside the finalizer and you should not reference 
        /// other objects. Only unmanaged resources can be disposed
        /// </summary>
        /// <param name="disposing">Specify which scenario is used.</param>
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    if (this.httpServerTransport != null)
                    {
                        this.httpServerTransport.Dispose();
                        this.httpServerTransport = null;
                    }

                    if (this.httpRequest != null)
                    {
                        this.httpRequest = null;
                    }

                    if (this.logger != null)
                    {
                        this.logger = null;
                    }
                }
            }

            // Note disposing has been done.
            this.disposed = true;
        }

        #endregion
    }
}
