// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.CommonStack
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// The http server side.
    /// </summary>
    public class HttpServerTransport : IDisposable
    {
        #region field

        /// <summary>
        /// The const variable SEND_DATA_SIZE specifies size of the data sent every time.
        /// </summary>
        private const int SENDDATASIZE = 2048;

        /// <summary>
        /// The simultaneous connection max number.
        /// </summary>
        private const int CONNECTIONMAXNUMBER = 500;

        /// <summary>
        /// Http request event handle.
        /// </summary>
        private EventHandler<HttpRequestEventArg> httpRequestEventHandle;

        /// <summary>
        /// The http listener.
        /// </summary>
        private HttpListener httpListener;

        /// <summary>
        /// The http request manager thread.
        /// </summary>
        private Thread httpRequestManagerThread;

        /// <summary>
        /// The http server's state.
        /// </summary>
        private long runState = (long)State.Stopped;

        /// <summary>
        /// Track whether Dispose has been called.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The logger.
        /// </summary>
        private ILogPrinter logger;

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Initializes a new instance of the HttpServerTransport class.
        /// </summary>
        /// <param name="transferProtocol">The transport type will be used</param>
        /// <param name="listenPort">The listen port.</param>
        /// <param name="resource">The resource.</param>
        public HttpServerTransport(
            TransferProtocol transferProtocol,
            int listenPort,
            string resource)
        {
            this.httpListener = new HttpListener();

            this.httpListener.Prefixes.Add(
                this.GetListenPrefix(transferProtocol, listenPort, resource));
        }

        /// <summary>
        /// Initializes a new instance of the HttpServerTransport class.
        /// </summary>
        /// <param name="transferProtocol">The transport type will be used</param>
        /// <param name="listenPort">The listen port.</param>
        /// <param name="ipaddressType">The IP address type.</param>
        /// <param name="resource">The resource.</param>
        public HttpServerTransport(
            TransferProtocol transferProtocol,
            int listenPort,
            IPAddressType ipaddressType,
            string resource)
        {
            this.httpListener = new HttpListener();

            this.httpListener.Prefixes.Add(
                this.GetListenPrefix(transferProtocol, listenPort, resource));
        }

        /// <summary>
        /// Initializes a new instance of the HttpServerTransport class.
        /// </summary>
        /// <param name="transferProtocol">The transport type will be used</param>
        /// <param name="listenPort">The listen port.</param>
        /// <param name="ipaddressType">The IP address type.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="logger">The logger.</param>
        public HttpServerTransport(
            TransferProtocol transferProtocol, 
            int listenPort, 
            IPAddressType ipaddressType,
            string resource, 
            ILogPrinter logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger", "The input parameter \"logger\" is null.");
            }

            this.logger = logger;

            this.httpListener = new HttpListener();

            string prefix = this.GetListenPrefix(transferProtocol, listenPort, resource);

            this.httpListener.Prefixes.Add(prefix);

            this.logger.AddDebug(string.Format(
                "Initialize the http server transport and add prefix: {0} to http listener.",
                prefix));
        }

        /// <summary>
        /// Finalizes an instance of the HttpServerTransport class
        /// This destructor will run only if the Dispose method 
        /// does not get called.
        /// It gives your base class the opportunity to finalize.
        /// Do not provide destructors in types derived from this class.
        /// </summary>
        ~HttpServerTransport()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #endregion

        #region property

        /// <summary>
        /// Gets the http server's state.
        /// </summary>
        public State RunState
        {
            get
            {
                return (State)Interlocked.Read(ref this.runState);
            }
        }

        /// <summary>
        /// Gets or sets the http request event handle.
        /// </summary>
        public EventHandler<HttpRequestEventArg> HttpRequestEventHandle
        {
            get { return this.httpRequestEventHandle; }
            set { this.httpRequestEventHandle = value; }
        }

        #endregion

        #region public method

        /// <summary>
        /// Start http server to listen and receive the http request.
        /// </summary>
        /// <exception cref="ThreadStateException">Represents Exception about the Thread State</exception>
        /// <exception cref="TimeoutException">Unable to start the request handling process in 10 seconds</exception>
        public void StartHttpServer()
        {
            if (this.httpRequestManagerThread == null || this.httpRequestManagerThread.ThreadState == ThreadState.Running)
            {
                this.httpRequestManagerThread = new Thread(new ThreadStart(this.HttpRequestManagerStart));
            }
            else if (this.httpRequestManagerThread.ThreadState == ThreadState.Running)
            {
                throw new ThreadStateException("The http request manager thread is already running!");
            }

            if (this.httpRequestManagerThread.ThreadState != ThreadState.Unstarted)
            {
                throw new ThreadStateException("The http request manager thread is not initialized.");
            }

            this.httpRequestManagerThread.Start();

            // Wait for 10 seconds.
            long waitMilliseconds = 10000;
            DateTime startTime = DateTime.Now;

            while (this.RunState != State.Started)
            {
                Thread.Sleep(100);
                if ((DateTime.Now - startTime).TotalMilliseconds > waitMilliseconds)
                {
                    throw new TimeoutException(
                        string.Format(
                            "Unable to start the request handling process within {0}", 
                            (DateTime.Now - startTime).TotalSeconds));
                }
            }

            if (this.logger != null)
            {
                this.logger.AddDebug("Http server process is started successfully.");
            }
        }

        /// <summary>
        /// Send the response message.
        /// </summary>
        /// <param name="header">Http header.</param>
        /// <param name="payLoadData">The payload data.</param>
        /// <param name="httpListenerContext">The payload data.</param>
        public void Send(
            Dictionary<string, string> header,
            byte[] payLoadData,
            HttpListenerContext httpListenerContext)
        {
            if (httpListenerContext == null)
            {
                throw new ArgumentNullException("httpListenerContext");
            }

            if (this.RunState == State.Stopped)
            {
                throw new InvalidOperationException("The http server process is stopped.");
            }

            while (this.RunState == State.Stopping)
            {
                Thread.Sleep(100);
            }

            HttpListenerResponse response = httpListenerContext.Response;
            
            // Add the response header.
            if (header != null)
            {
                foreach (string key in header.Keys)
                {
                    response.AddHeader(key, header[key]);
                }
            }

            if (payLoadData != null)
            {
                byte[][] data = new byte[][] { payLoadData };
                for (int i = 0; i < data.Length; i++)
                {
                    // Set length of Write method, 
                    // This is used to separate large message to small blocks, default 10KB per block
                    int len = SENDDATASIZE;
                    for (int index = 0; index < data[i].Length; index += len)
                    {
                        // Send same size data every time, except the last one
                        if (data[i].Length - index < len)
                        {
                            len = data[i].Length - index;
                        }

                        response.OutputStream.Write(data[i], index, len);
                    }
                }
            }

            response.OutputStream.Close();
            response.Close();

            if (this.logger != null)
            {
                this.logger.AddDebug("The response message is sent successfully.");
            }
        }

        /// <summary>
        /// Send the response message.
        /// </summary>
        /// <param name="statusCode">Status code.</param>
        /// <param name="header">Http header.</param>
        /// <param name="payLoadData">The payload data.</param>
        /// <param name="httpListenerContext">The http listener context.</param>
        /// <param name="needNextRequest">Whether need next request before sending response.</param>
        public void Send(
            int statusCode,
            Dictionary<string, string> header, 
            byte[] payLoadData, 
            HttpListenerContext httpListenerContext,
            bool needNextRequest)
        {
            if (httpListenerContext == null)
            {
                throw new ArgumentNullException("httpListenerContext");
            }

            if (this.RunState == State.Stopped)
            {
                throw new InvalidOperationException("The http server process is stopped.");
            }

            while (this.RunState == State.Stopping)
            {
                Thread.Sleep(100);
            }

            HttpListenerResponse response = httpListenerContext.Response;

            response.StatusCode = statusCode;

            // Add the response header.
            if (header != null)
            {
                foreach (string key in header.Keys)
                {
                    response.AddHeader(key, header[key]);
                }
            }

            if (payLoadData != null)
            {
                byte[][] data = new byte[][] { payLoadData };
                for (int i = 0; i < data.Length; i++)
                {
                    // Set length of Write method, 
                    // This is used to separate large message to small blocks, default 10KB per block
                    int len = SENDDATASIZE;
                    for (int index = 0; index < data[i].Length; index += len)
                    {
                        // Send same size data every time, except the last one
                        if (data[i].Length - index < len)
                        {
                            len = data[i].Length - index;
                        }

                        response.OutputStream.Write(data[i], index, len);
                    }
                }
            }

            if (needNextRequest == false)
            {
                response.OutputStream.Close();
                response.Close();

                if (this.logger != null)
                {
                    this.logger.AddDebug("The response message is sent successfully.");
                }
            }
        }

        /// <summary>
        /// Stop http server to listen and receive the http request.
        /// </summary>
        public void StopHttpServer()
        {
            Interlocked.Exchange(ref this.runState, (long)State.Stopping);

            if (this.httpListener.IsListening)
            {
                this.httpListener.Stop();
            }

            if (!this.httpListener.IsListening && this.logger != null)
            {
                this.logger.AddDebug("Http server process is stopped listening successfully.");
            }

            // Wait for 10 seconds.
            long waitMilliseconds = 10000;
            DateTime startTime = DateTime.Now;

            while (this.RunState != State.Stopped)
            {
                Thread.Sleep(100);
                if ((DateTime.Now - startTime).TotalMilliseconds > waitMilliseconds)
                {
                    throw new TimeoutException("Unable to stop the http server process.");
                }
            }

            this.httpRequestManagerThread = null;

            if (this.logger != null)
            {
                this.logger.AddDebug("Http server process is stopped successfully.");
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
        /// The thread start method for http request manager thread.
        /// </summary>
        private void HttpRequestManagerStart()
        {
            Interlocked.Exchange(ref this.runState, (long)State.Starting);
            try
            {
                if (!this.httpListener.IsListening)
                {
                    this.httpListener.Start();
                }

                if (this.httpListener.IsListening)
                {
                    Interlocked.Exchange(ref this.runState, (long)State.Started);

                    if (this.logger != null)
                    {
                        this.logger.AddDebug("Http server process is listening now.");
                    }
                }

                try
                {
                    while (this.RunState == State.Started)
                    {
                        HttpListenerContext context = this.httpListener.GetContext();
                        this.RaiseIncomingRequest(context);
                    }
                }
                catch (HttpListenerException e)
                {
                    if (this.logger != null)
                    {
                        this.logger.AddWarning(
                            string.Format("Unexpected exception error code: {0}; detail information: {1}.", e.ErrorCode, e.Message));
                    }
                }
            }
            finally
            {
                Interlocked.Exchange(ref this.runState, (long)State.Stopped);
            }
        }

        /// <summary>
        /// The event handle for the received http request.
        /// </summary>
        /// <param name="context">The http listener context.</param>
        private void RaiseIncomingRequest(HttpListenerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (this.logger != null)
            {
                this.logger.AddDebug(string.Format(
                    "Http request is received, the remote endpoint's ip address is {0}, the port is {1}",
                    context.Request.RemoteEndPoint.Address,
                    context.Request.RemoteEndPoint.Port));
            }

            HttpRequestEventArg newHttpReqeust = new HttpRequestEventArg(context);

            try
            {
                if (this.httpRequestEventHandle != null)
                {
                    this.httpRequestEventHandle.BeginInvoke(this, newHttpReqeust, null, null);
                }
            }
            catch (InvalidOperationException e)
            {
                if (this.logger != null)
                {
                    this.logger.AddWarning(
                        string.Format("Unexpected exception detailed information: {0}.", e.Message));
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
                    if (this.logger != null)
                    {
                        this.logger = null;
                    }

                    if (this.RunState != State.Stopped)
                    {
                        this.StopHttpServer();
                    }

                    if (this.httpRequestManagerThread != null)
                    {
                        this.httpRequestManagerThread.Abort();
                        this.httpRequestManagerThread = null;
                    }

                    if (this.httpListener != null)
                    {
                        this.httpListener.Close();
                        this.httpListener = null;
                    }
                }
            }

            // Note disposing has been done.
            this.disposed = true;
        }

        /// <summary>
        /// Get the listen prefix.
        /// </summary>
        /// <param name="transferProtocol">The transport type will be used</param>
        /// <param name="ipaddressType">The ip address type.</param>
        /// <param name="hostedCacheModeListenPort">The hosted cache mode listen port.</param>
        /// <param name="resource">The customer resource data.</param>
        /// <returns>The listen prefix string.</returns>
        private string GetListenPrefix(
            TransferProtocol transferProtocol, 
            int hostedCacheModeListenPort, 
            string resource)
        {
            string transportTypeStr;
            string listenPrefix = null;

            if (resource == null)
            {
                resource = string.Empty;
            }
            else if (!resource.EndsWith("/") && resource != string.Empty)
            {
                resource = resource + "/";
            }

            if (transferProtocol == TransferProtocol.HTTPS)
            {
                transportTypeStr = "https";
            }
            else if (transferProtocol == TransferProtocol.HTTP)
            {
                transportTypeStr = "http";
            }
            else
            {
                throw new ArgumentException("The supported transport type in pchc is http or https for this version.", "transportType");
            }

            listenPrefix =
                        transportTypeStr + "://+:"
                        + hostedCacheModeListenPort + "/" + resource;

            return listenPrefix;
        }

        #endregion
    }
}

