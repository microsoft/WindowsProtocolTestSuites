// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Dynamic;
using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// The abstract class for all proxy request handler.
    /// The handler class verifies client request, 
    /// generates corresponding response, etc.
    /// </summary>
    public abstract class ProxyRequestHandlerBase : IRequestHandler, IRequestValidator
    {
        private readonly dynamic _dynamicObjects = new ExpandoObject();
 
        /// <summary>
        /// The HTTP request to deal with.
        /// </summary>
        public HttpRequestMessage Request { get; set; }

        /// <summary>
        /// ServerDataModel is available to all request handlers 
        /// so that they can use the data stored in the ServerDataModel
        /// instance to generate response.
        /// </summary>
        public ServerDataModel ServerDataModel { get; set; }

        /// <summary>
        /// A dynamic object which allows anything to be put in it.
        /// If extra objects are needed to handle the request, use this property.
        /// </summary>
        public dynamic DynamicObject { get { return _dynamicObjects; } }

        /// <summary>
        /// Indicates that the request is verified.
        /// </summary>
        protected bool Verified = false;

        // constants used by all ProxyRequestHandlers
        protected const string ApplicationJsonContent = "application/json;charset=UTF-8";
        protected const string TextHtmlContent        = "text/html; charset=utf-8";
        protected const string AuthenticationType     = "Basic Realm=\"\"";

        /// <summary>
        /// Check whether the hander can handle the specified request.
        /// </summary>
        /// <param name="request">
        /// The incoming HTTP request.
        /// </param>
        /// <returns>
        /// True if this handler can handle the request; false, otherwise
        /// </returns>
        public virtual bool CanHandleRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.ContainsIgnoreCase(Constraints.AdfsProxyUrl);
        }

        /// <summary>
        /// Verify the client request.
        /// </summary>
        /// <param name="message">
        /// Returns the message during verification. 
        /// </param>
        /// <returns>
        /// True is the client request is valid; False, otherwise.
        /// </returns>
        public virtual bool VerifyRequest(out string message)
        {
            Verified = true;
            
            // if no client certificate provided, just return true;
            // because TD requires to check the certificate when it is provided.
            if (Request.ClientCertificate == null) {
                message = "Client certificate not provided.";
                return true;
            }

            //TODO: verify the client certificate is actually stored in the data model
            message = "Verified client certificate: " + Request.ClientCertificate.SubjectName.Name;
            return true;
        }

        /// <summary>
        /// The normal response when the client request is valid.
        /// </summary>
        /// <returns>
        /// Response to the client request.
        /// </returns>
        public virtual HttpResponseMessage GetResponse()
        {
            return new HttpResponseMessage(200);
        }
    }
}
