// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// Handles POST /adfs/Proxy/relyingpartytrusts/{identity}/publishedSettings
    /// </summary>
    public class PostProxyPublishSettingsRequestHandler : ProxyRequestHandlerBase
    {
        public override bool CanHandleRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.ContainsIgnoreCase(Constraints.RelyingPartyTrustUrl) &&
                   request.RequestUri.AbsolutePath.ContainsIgnoreCase("publishedSettings") &&
                   request.Method == HttpMethod.POST;
        }

        public override bool VerifyRequest(out string message)
        {
            if(!base.VerifyRequest(out message)) return false;
            message = "The incoming request must contain valid RelyingPartyTrustPublishingSettings";
            return JSONObject.TryParse<RelyingPartyTrustPublishingSettings>(Request.Content.GetString());
        }

        public override HttpResponseMessage GetResponse()
        {
            return GetSucessResponse();
        }

        public HttpResponseMessage GetSucessResponse()
        {
            // ServerDataModel.SetPublishedEndpoint();
            return  new HttpResponseMessage(200){ContentType = TextHtmlContent};
        }

        public HttpResponseMessage GetConflictResponse()
        {
            // If the publishing settings specified in Relying Party Trust 
            // Publishing Settings have been set previously the server MUST 
            // return a HTTP error code of 409.
            return new HttpResponseMessage(409);
        }
    }

    /// <summary>
    /// Handles DELETE /adfs/Proxy/relyingpartytrusts/{identity}/publishedSettings
    /// </summary>
    public class DeleteProxyPublishSettingsRequestHandler : ProxyRequestHandlerBase
    {
        public override bool CanHandleRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.ContainsIgnoreCase(Constraints.RelyingPartyTrustUrl) &&
                   request.RequestUri.AbsolutePath.ContainsIgnoreCase("publishedSettings") &&
                   request.Method == HttpMethod.DELETE;
        }

        public override bool VerifyRequest(out string message)
        {
            if(!base.VerifyRequest(out message)) return false;
            message = "The incoming request must contain valid RelyingPartyTrustPublishingSettings";
            return JSONObject.TryParse<RelyingPartyTrustPublishingSettings>(Request.Content.GetString());
        }

        public override HttpResponseMessage GetResponse()
        {
            return GetSucessResponse();
        }

        public HttpResponseMessage GetSucessResponse()
        {
            // ServerDataModel.ResetPublishedEndpoint();
            return  new HttpResponseMessage(200){ContentType = TextHtmlContent};
        }

        public HttpResponseMessage GetNotFoundResponse()
        {
            // the publishing settings specified in Relying Party Trust
            // Publishing Settings have not been set previously the server
            // MUST return a HTTP error code of 404.
            return new HttpResponseMessage(404);
        }
    }

}
