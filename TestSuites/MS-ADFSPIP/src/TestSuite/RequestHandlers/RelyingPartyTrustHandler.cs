// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// Handles GET /adfs/Proxy/relyingpartytrusts
    /// </summary>
    public class GetProxyRelyingPartyTrustRequestHandler : ProxyRequestHandlerBase
    {
        public override bool CanHandleRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.ContainsIgnoreCase(Constraints.RelyingPartyTrustUrl) &&
                   request.Method == HttpMethod.GET;
        }

        public override HttpResponseMessage GetResponse()
        {
            return GetSuccessResponse();
        }

        public HttpResponseMessage GetSuccessResponse()
        {
            var response = new HttpResponseMessage(200) {ContentType = ApplicationJsonContent};
            response.Content.SetString(ServerDataModel.GetSerializedRelyingPartyTrusts());
            return response;
        }
    }


}
