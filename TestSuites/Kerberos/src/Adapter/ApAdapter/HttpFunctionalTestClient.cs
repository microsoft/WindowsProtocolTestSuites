// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Protocol.TestSuites.Kerberos.Adapter
{
   public class HttpFunctionalTestClient : IDisposable
    {
        #region Private fields
        private Uri uri;
        private WebRequest client;
        private WebResponse response;
        private byte[] serverGssToken;
        private bool disposed;
        private WebExceptionStatus status;
        #endregion

        #region Constructor
        public HttpFunctionalTestClient(string uriString, byte[] authInfo = null)
        {
            uri = new Uri(uriString);
            client = WebRequest.Create(uri);
            client.Method = "Get";
            client.ContentType = "text/xml";
            serverGssToken = authInfo;
            if (authInfo != null)
            {
                SetNegoAuthHeader(authInfo);
            }
            disposed = false;
        }

        #endregion

        #region Methods
        public void SetHttpRequestMethod(string method)
        {
            client.Method = method;
        }

        public void SetHttpRequestContentType(string contentType)
        {
            client.ContentType = contentType;
        }

        public void SetCredentials(string username, string password)
        {
            NetworkCredential myCred = new NetworkCredential(username, password);
            client.Credentials = myCred;
        }

        public void SetBasicAuthHeader(string username, string password)
        {
            serverGssToken = new ASCIIEncoding().GetBytes(username + ":" + password);
            string auth;
            auth = Convert.ToBase64String(serverGssToken);
            client.Headers["Authorization"] = "Basic " + auth;
        }

        public void SetNegoAuthHeader(byte[] authInfo)
        {
            serverGssToken = authInfo;

            string auth;
            auth = Convert.ToBase64String(authInfo);
            client.Headers["Authorization"] = "Negotiate " + auth;
        }

        public HttpStatusCode GetHttpResponse()
        {
            try
            {
                response = client.GetResponse();
            }
            catch (WebException e)
            {
                status = e.Status;
                response = e.Response;
                return ((HttpWebResponse)e.Response).StatusCode;
            }
            //Fix this status to enum
            return HttpStatusCode.OK;
        }

        public WebResponse GetHttpWebResponse()
        {
            try
            {
                response = client.GetResponse();
            }
            catch (WebException e)
            {
                status = e.Status;
                response = e.Response;


                return (e.Response);
            }
            return response;
        }

        public byte[] GetNegoAuthHeader()
        {
            if (response != null)
            {
                var authHeader = response.Headers["WWW-Authenticate"];
                Regex g = new Regex(@"(?<scheme>[^\s,]+)\s+(?<param>[^\s,]+)*");
                MatchCollection matches = g.Matches(authHeader);
                foreach (Match match in matches)
                {
                    string scheme = match.Groups["scheme"].Value;
                    if (string.Equals(scheme, "Negotiate", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string param = match.Groups["param"].Value;
                        serverGssToken = Convert.FromBase64String(param);
                        return serverGssToken;
                    };
                }
                return serverGssToken;
            }
            else
            {
                return null;
            }
        }

        public string[] GetAuthMethods()
        {
            if (response != null && status == WebExceptionStatus.ProtocolError)
            {
                var authHeader = response.Headers["WWW-Authenticate"];
                string[] methods = authHeader.Split(',');
                return methods;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region IDispose

        /// <summary>
        /// Release all resources
        /// </summary>
        public void Dispose()
        {
            if (client != null)
                client.Abort();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release all resources
        /// </summary>
        /// <param name="disposing">Indicate user or GC calling this method</param>
        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    response.Close();
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Deconstructor
        /// </summary>
        ~HttpFunctionalTestClient()
        {
            Dispose(false);
        }

        #endregion
    }
}
