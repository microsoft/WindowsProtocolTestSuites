// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// The mock client trying visit the proxy.
    /// </summary>
    /// <remarks>
    /// Use the mock client to test if the proxy can deal with
    /// outside requests correctly -- redirect unauthenticated
    /// request to ADFS for authenticate; replay authenticated
    /// request to the correct internal url.
    /// </remarks>
    public class MockClient
    {
        #region Fields and Properties

        private IPAddress _proxyAddress;

        /// <summary>
        /// Gets or sets the IP address and port of the proxy.
        /// </summary>
        public IPAddress ProxyEndpoint
        {
            get { return _proxyAddress; }
            set { _proxyAddress = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="proxyEndpoint">
        /// A string that contains the IP address and port of the proxy.
        /// </param>
        public MockClient(IPAddress proxyEndpoint)
        {
            _proxyAddress = proxyEndpoint;

            // make the client trust all server certificates
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;
        }

        #endregion

        #region Asynchronous Methods

        /// <summary>
        /// Async version of MakeWebAccessRequest.
        /// </summary>
        public Task<HttpResponseMessage> MakeWebAccessRequestAsync(Uri webAppUri, string authToken = null)
        {
            return Task.Factory.StartNew(() => MakeWebAccessRequest(webAppUri, authToken));
        }

        /// <summary>
        /// Async version of MakeRequestToEndpoint.
        /// </summary>
        public Task<HttpResponseMessage> MakeRequestToEndpointAsync(Uri endpointUri, X509Certificate2 clientCertificate = null)
        {
            return Task.Factory.StartNew(() => MakeRequestToEndpoint(endpointUri, clientCertificate));
        }

        #endregion

        #region Synchronous Methods

        /// <summary>
        /// Makes a web application access request to the proxy, with or without an authToken
        /// in the query string.
        /// </summary>
        /// <returns>
        /// The response message from the server.
        /// </returns>
        public HttpResponseMessage MakeWebAccessRequest(Uri webAppUri, string authToken = null)
        {
            var requestUri = new UriBuilder(webAppUri);
            var hostName = requestUri.Host;

            // replace the host name with the proxy IP address
            // and add a Host header in the request instead
            // so that the request directly goes to the proxy
            // without setting a DNS record on the computer
            requestUri.Host = _proxyAddress.ToString();

            // append authToken in the query string if any
            if (authToken != null) {
                requestUri.Query = "authToken=" + authToken;
            }

            var request = (HttpWebRequest) WebRequest.Create(requestUri.Uri);
            request.Method = "GET";
            request.Host = hostName;
            request.AllowAutoRedirect = true;

            return MakeRequest(request);
        }

        /// <summary>
        /// Makes an request to an ADFS endpoint, with or without the client certificate.
        /// </summary>
        /// <returns>
        /// The response message from the server.
        /// </returns>
        public HttpResponseMessage MakeRequestToEndpoint(Uri endpointUri, X509Certificate2 clientCertificate = null)
        {
            var requestUri = new UriBuilder(endpointUri);
            var hostName = requestUri.Host;

            // replace the host name with the proxy IP address
            // and add a Host header in the request instead
            // so that the request directly goes to the proxy
            // without setting a DNS record on the computer
            requestUri.Host = _proxyAddress.ToString();

            var request = (HttpWebRequest) WebRequest.Create(requestUri.Uri);
            request.Method = "GET";
            request.Host = hostName;
            request.AllowAutoRedirect = true;

            // append client certificate if any
            if (clientCertificate != null) {
                request.ClientCertificates = new X509CertificateCollection {clientCertificate};
            }

            return MakeRequest(request);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sends a request to the server and returns the response.
        /// </summary>
        private HttpResponseMessage MakeRequest(HttpWebRequest request)
        {
            HttpResponseMessage response;
            TraceInformation("Request sent to " + _proxyAddress.ToString());
            TraceMessage(request.Host + request.RequestUri.AbsolutePath);

            try {
                // wait to get the response
                response = GetResponseMessage((HttpWebResponse) request.GetResponse());
            }
                // suppress the WebException and return the error response
            catch (WebException ex) {
                // if no response has been retrieved, just return null
                // or return the error response message
                response = ex.Response == null ? null : GetResponseMessage((HttpWebResponse) ex.Response);
            }

            if (response != null) {
                TraceInformation("Response received");
                TraceMessage(response.ToString());
            }

            return response;
        }

        public HttpResponseMessage MakeWebRequestWithBasicAuth(Uri webAppUri, string basicAuth)
        {
            var requestUri = new UriBuilder(webAppUri);

            // replace the host name with the proxy IP address
            // and add a Host header in the request instead
            // so that the request directly goes to the proxy
            // without setting a DNS record on the computer
            var request = (HttpWebRequest)WebRequest.Create(requestUri.Uri);
            request.Method = "GET";
            request.AllowAutoRedirect = true;
            request.Headers.Add(HttpRequestHeader.Authorization, basicAuth);

            return MakeRequest(request);
        }

        /// <summary>
        /// Transform HttpWebResponse to HttpResponseMessage, for the convenience
        /// if someone wants to check the response later.
        /// </summary>
        /// <param name="response">
        /// The HttpWebResponse instance returned from WebClient.GetResponse.
        /// </param>
        /// <returns>
        /// An HttpResponseMessage instance.
        /// </returns>
        private HttpResponseMessage GetResponseMessage(HttpWebResponse response)
        {
            var responseMessage = new HttpResponseMessage {
                Version = response.ProtocolVersion,
                StatusCode = (int) response.StatusCode
            };

            foreach (var key in response.Headers.AllKeys) {
                responseMessage.Headers.AddValue(key, response.Headers[key]);
            }

            if (response.ContentLength > 0) {
                var stream = response.GetResponseStream();
                if (stream != null) {
                    using (var reader = new StreamReader(stream)) {
                        responseMessage.Content.SetString(reader.ReadToEnd());
                    }
                    stream.Close();
                }
            }
            response.Close();

            return responseMessage;
        }

        /// <summary>
        /// Used to log information.
        /// </summary>
        private void TraceInformation(string message)
        {
            Trace.WriteLine(string.Format("MockClient Information : {0} ({1})", message, DateTime.Now));
            Trace.Flush();
        }

        /// <summary>
        /// Used to log a request or response message.
        /// </summary>
        private void TraceMessage(string message)
        {
            Trace.WriteLine(message);
            Trace.Flush();
        }

        #endregion
    }
}
