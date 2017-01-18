// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// Handles POST /adfs/Proxy/EstablishTrust
    /// </summary>
    public class PostProxyEstablishTrustRequestHandler : ProxyRequestHandlerBase
    {
        public override bool CanHandleRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.ContainsIgnoreCase(Constraints.EstablishTrustUrl) &&
                   request.Method == HttpMethod.POST;
        }

        public override bool VerifyRequest(out string message)
        {
            if (!base.VerifyRequest(out message)) return false;
            message = "The incoming request must be valid EstablishTrustRequest";
            return JSONObject.TryParse<ProxyTrust>(Request.Content.GetString());
        }

        public override HttpResponseMessage GetResponse()
        {
            return string.IsNullOrEmpty(Request.Authorization) ? GetUnauthorizedResponse() : GetSuccessResponse();
        }

        public HttpResponseMessage GetUnauthorizedResponse()
        {
            return new HttpResponseMessage(HttpStatusCode.Unauthorized) {WwwAuthenticate = AuthenticationType};

        }

        public HttpResponseMessage GetSuccessResponse()
        {
            // add proxy certificate to the server data model
            ServerDataModel.AddProxyTrustCertificate(JSONObject.Parse<ProxyTrust>(Request.Content.GetString()));

            return new HttpResponseMessage(200) {ContentType = TextHtmlContent};
        }

        public HttpResponseMessage GetBadRequestResponse()
        {
            // The server MUST validate that the [Proxy Trust].SerializedTrustCertificate 
            // has an extended key usage (EKU) for client authentication and is within 
            // the validity period. Or the server MUST return a HTTP error code of 400.
            return new HttpResponseMessage(400);
        }
    }

}
