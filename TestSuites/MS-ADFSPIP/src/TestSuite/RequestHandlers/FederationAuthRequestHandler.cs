// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// Handles POST /adfs/backendproxytls
    /// </summary>
    public class FederationAuthRequestHandler : ProxyRequestHandlerBase
    {
        public override bool CanHandleRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.ContainsIgnoreCase(Constraints.FederationAuthUrl) &&
                   request.Method == HttpMethod.GET;
        }

        /// <summary>
        /// Parameters provided in request query string.
        /// </summary>
        protected class FedauthParameters
        {
            public string Version;
            public string Action;
            public string Realm;
            public string AppRealm;
            public string ReturnUrl;
        }

        /// <summary>
        /// Parse the query string into a FedauthParameters instance.
        /// </summary>
        protected FedauthParameters ParseFedauthQueryString(string queryString)
        {
            queryString = queryString.Replace("?", string.Empty);
            var param = new FedauthParameters();

            foreach (var entry in queryString.Split('&')) {
                var key   = entry.Split('=')[0];
                var value = entry.Split('=')[1];

                // set the field with the same name as the query string key
                param.SetFieldValue(key, Uri.UnescapeDataString(value), true);
            }

            return param;
        }

        public override bool VerifyRequest(out string message)
        {
            // parse query string parameters
            var param = ParseFedauthQueryString(Request.RequestUri.Query);

            // check version
            if (param.Version != "1.0") {
                message = "Version of the protocol MUST be 1.0.";
                return false;
            }

            // check Action
            if (!param.Action.EqualsIgnoreCase("signin")) {
                message = "Action on authentication request MUST be signin";
                return false;
            }

            // check Realm
            if (!param.Realm.EqualsIgnoreCase(Constraints.DefaultProxyRelyingPartyTrustIdentifier)) {
                message = "Realm MUST be [Client State].ProxyRelyingPartyTrustIdentifier.";
                return false;
            }

            // retrieve published application 
            var endpoint = ServerDataModel.GetPublishedEndpoint((string) DynamicObject.InitialUrl);

            if (endpoint == null) {
                message = "Endpoint with the url was not found.";
                return false;
            }

            // check AppRealm
            // There is an TDI here, the AppRealm is the relying party ID, not the URL of the endpoint
            if (!param.AppRealm.EqualsIgnoreCase(endpoint.ADFSRelyingPartyID)) {
                message = "AppRealm must match the relying party ID.";
                return false;
            }

            // check ReturnUrl
            if (!param.ReturnUrl.EqualsIgnoreCase(endpoint.FrontendUrl)) {
                message = "ReturnUrl must match the URL of the published application.";
                return false;
            }

            // validation passed
            message = "Query string parameters vefified.";
            return true;
        }
    }
}