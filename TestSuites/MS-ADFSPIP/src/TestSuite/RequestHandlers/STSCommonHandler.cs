// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// Handles GET /FederationMetadata/2007-06/FederationMetadata.xml
    /// </summary>
    public class GetFederationMetadataRequestHandler : ProxyRequestHandlerBase
    {
        public override bool CanHandleRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.ContainsIgnoreCase(Constraints.FederationMetadataUrl) &&
                   request.Method == HttpMethod.GET;
        }

        public override HttpResponseMessage GetResponse()
        {
            return GetSuccessResponse();
        }

        public HttpResponseMessage GetSuccessResponse()
        {
            var response = new HttpResponseMessage(200) {ContentType = TextHtmlContent};
            response.Content.SetString(ServerDataModel.GetFederationMetadata());
            return response;
        }
    }

    /// <summary>
    /// GET /adfs/Proxy/GetConfiguration
    /// </summary>
    public class GetConfigurationRequestHandler : ProxyRequestHandlerBase
    {
        public override bool CanHandleRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.ContainsIgnoreCase(Constraints.GetSTSConfigurationUrl) &&
                   request.Method == HttpMethod.GET;
        }

        public override HttpResponseMessage GetResponse()
        {
            return GetSuccessResponse();
        }

        public HttpResponseMessage GetSuccessResponse()
        {
            var response = new HttpResponseMessage(200) {ContentType = TextHtmlContent};
            response.Content.SetString(ServerDataModel.GetSerializedStsConfiguration());
            return response;
        }
    }
}