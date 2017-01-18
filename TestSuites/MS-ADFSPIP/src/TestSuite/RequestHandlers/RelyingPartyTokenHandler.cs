// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP.RequestHandlers
{
    /// <summary>
    /// Handles POST /adfs/Proxy/relyingpartytoken
    /// </summary>
    public class RelyingPartyTokenHandler : ProxyRequestHandlerBase
    {
        public override bool CanHandleRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.ContainsIgnoreCase(AdfsServicePathPairs.ActiveEndpointAuthenticationURL) &&
                   request.Method == HttpMethod.POST;
        }

        public override bool VerifyRequest(out string message)
        {
            if (!base.VerifyRequest(out message)) return false;
            message = "The incoming request must be valid EstablishTrustRequest";
            return JSONObject.TryParse<BasicAuthParameters>(Request.Content.GetString());
        }

        public override HttpResponseMessage GetResponse()
        {
            var requestData = JSONObject.Parse<BasicAuthParameters>(Request.Content.GetString());
            return requestData.Username.ToLower().Equals(EnvironmentConfig.DomainAdminUser.ToLower()) ? GetSuccessResponse() : GetUnauthorizedResponse();
        }

        public HttpResponseMessage GetUnauthorizedResponse()
        {
            return new HttpResponseMessage(HttpStatusCode.Unauthorized) { WwwAuthenticate = AuthenticationType };

        }

        public HttpResponseMessage GetSuccessResponse()
        {
            // add proxy certificate to the server data model
            ServerDataModel.AddProxyTrustCertificate(JSONObject.Parse<ProxyTrust>(Request.Content.GetString()));

            HttpResponseMessage response = new HttpResponseMessage(200);
            ProxyTokenWrapper ptw = new ProxyTokenWrapper() { AuthToken = ServerDataModel.GetAuthToken() };

            response.Content.SetString(ptw.ToString());
            response.ContentType = ApplicationJsonContent;
            return response;
        }

        public HttpResponseMessage GetBadRequestResponse()
        {
            // The server MUST validate that the [Proxy Trust].SerializedTrustCertificate 
            // has an extended key usage (EKU) for client authentication and is within 
            // the validity period. Or the server MUST return a HTTP error code of 400.
            return new HttpResponseMessage(400);
        }

        protected class BasicAuthParameters : JSONObject
        {
            public string AppRealm;
            public string Username;
            public string Password;
            public string Realm;
            public string UserCertificate;
            public string DeviceCertificate;
            public List<KeyValue> HttpHeaders;
        }

        protected class KeyValue : JSONObject
        {
            public string Key;
            public string Value;
        }
    }
}
