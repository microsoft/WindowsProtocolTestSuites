// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// Handles POST /adfs/Proxy/webapplicationproxy/trust
    /// </summary>
    public class PostProxyTrustRequestHandler : ProxyRequestHandlerBase
    {
        public override bool CanHandleRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.ContainsIgnoreCase(Constraints.ProxyTrustUrl) &&
                   request.Method == HttpMethod.POST;
        }

        public override bool VerifyRequest(out string message)
        {
            if (!base.VerifyRequest(out message)) return false;
            message = "The incoming request must be valid ProxyRelyingPartyTrust";
            return JSONObject.TryParse<ProxyRelyingPartyTrust>(Request.Content.GetString());
        }

        public override HttpResponseMessage GetResponse()
        {
            return GetSuccessResponse();
        }

        public HttpResponseMessage GetSuccessResponse()
        {
            ServerDataModel.AddProxyRelyingPartyTrust(JSONObject.Parse<ProxyRelyingPartyTrust>(Request.Content.GetString()));
            
            return new HttpResponseMessage(200);
        }
    }

    /// <summary>
    /// Handles GET /adfs/Proxy/webapplicationproxy/trust
    /// </summary>
    public class GetProxyTrustRequestHandler : ProxyRequestHandlerBase
    {
        public override bool CanHandleRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.ContainsIgnoreCase(Constraints.ProxyTrustUrl) &&
                   request.Method == HttpMethod.GET;
        }

        public override HttpResponseMessage GetResponse()
        {
            return ServerDataModel.ProxyRelyingPartyTrust.Identifier != null
                ? GetSuccessResponse()
                : GetNotFoundResponse();
        }

        public HttpResponseMessage GetNotFoundResponse()
        {
            return new HttpResponseMessage(404);
        }

        public HttpResponseMessage GetSuccessResponse()
        {
            var response = new HttpResponseMessage(200) {ContentType = ApplicationJsonContent};
            response.Content.SetString(ServerDataModel.GetSerializedProxyRelyingPartyTrust());
            return response;
        }
    }
}