// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// Handles POST /adfs/Proxy/RenewTrust
    /// </summary>
    public class PostProxyRenewTrustRequestHandler : ProxyRequestHandlerBase
    {
        public override bool CanHandleRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.ContainsIgnoreCase(Constraints.RenewTrustUrl) &&
                   request.Method == HttpMethod.POST;
        }

        public override bool VerifyRequest(out string message)
        {
            if(!base.VerifyRequest(out message)) return false;
            message = "The incoming request must be valid RenewTrustRequest";
            return JSONObject.TryParse<ProxyTrustRenewal>(Request.Content.GetString());
        }

        public override HttpResponseMessage GetResponse()
        {
            return GetSuccessResponse();
        }

        public HttpResponseMessage GetSuccessResponse()
        {
            ServerDataModel.AddProxyTrustCertificate(JSONObject.Parse<ProxyTrustRenewal>(Request.Content.GetString()));

            return new HttpResponseMessage(200) {ContentType = TextHtmlContent};
        }
    }
}
