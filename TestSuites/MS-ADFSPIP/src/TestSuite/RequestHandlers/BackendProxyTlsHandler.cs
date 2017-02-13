// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// Handles POST /adfs/backendproxytls
    /// </summary>
    public class PostBackendProxyTlsRequestHandler : ProxyRequestHandlerBase
    {
        public override bool CanHandleRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.ContainsIgnoreCase(Constraints.BackEndProxyTLSUrl) &&
                   request.Method == HttpMethod.POST;
        }

        public override bool VerifyRequest(out string message)
        {
            SerializedRequestWithCertificate serializedRequest;
            
            // check request format
            if (!JSONObject.TryParse<SerializedRequestWithCertificate>(Request.Content.GetString(), out serializedRequest)) {
                message = "The incoming request must be valid BackendProxyTls request.";
                return false;
            }

            // check replayed request uri
            if (!serializedRequest.Request.RequestUri.EqualsIgnoreCase((string)DynamicObject.EndpointUri)) {
                message = "Replayed request must contain the same request uri as the original request.";
                return false;
            }

            // check replayed request client certificate
            if (!serializedRequest.SerializedClientCertificate.EqualsIgnoreCase((string) DynamicObject.ClientCertificate)) {
                message = "Replayed request must contain the same client certificate as the original request.";
                return false;
            }

            // extra headers in section 2.2.1 are not checked here
            // since it is a SHOULD, not a MUST in TD
            message = "Replayed request is verified.";
            return true;
        }
    }
}
