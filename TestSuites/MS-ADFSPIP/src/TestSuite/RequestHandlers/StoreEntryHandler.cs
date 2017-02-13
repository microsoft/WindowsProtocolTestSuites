// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text.RegularExpressions;
using Microsoft.Protocols.TestTools.StackSdk.Identity.ADFSPIP;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// Handles GET /adfs/Proxy/webapplicationproxy/store
    /// and     GET /adfs/Proxy/webapplicationproxy/store/{key}
    /// </summary>
    public class GetProxyStoreEntryRequestHandler : ProxyRequestHandlerBase
    {
        public override bool CanHandleRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.ContainsIgnoreCase(Constraints.StoreUrl) &&
                   request.Method == HttpMethod.GET;
        }

        public override HttpResponseMessage GetResponse()
        {
            return GetSuccessResponse();
        }

        public HttpResponseMessage GetSuccessResponse()
        {
            var response = new HttpResponseMessage(200) { ContentType = ApplicationJsonContent };

            // if the absolute path ENDs with store url, it means to get all the store entries
            if (Regex.IsMatch(Request.RequestUri.AbsolutePath, Constraints.StoreUrl + "$", RegexOptions.IgnoreCase))
            {
                response.Content.SetString(ServerDataModel.GetSerializedStoreEntry());
            }
            else
            {
                // otherwise, get a specific entry
                var entryKey = Request.RequestUri.AbsolutePath.Substring(
                    Request.RequestUri.AbsolutePath.IndexOf(Constraints.StoreUrl, 0,
                        StringComparison.InvariantCultureIgnoreCase) +
                    Constraints.StoreUrl.Length + 1);
                response.Content.SetString(ServerDataModel.GetSerializedStoreEntry(entryKey));
            }
            return response;
        }
    }

    public class PostProxyStoreEntryRequestHandler : PutProxyStoreEntryRequestHandler
    {
        public override bool CanHandleRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.ContainsIgnoreCase(Constraints.StoreUrl) &&
                  request.Method == HttpMethod.POST;
        }

        public override HttpResponseMessage GetResponse()
        {
            var requestedEntry = JSONObject.Parse<StoreEntry>(Request.Content.GetString());
            ServerDataModel.AddStoreEntry(requestedEntry);
            var response = new HttpResponseMessage(HttpStatusCode.OK) { ContentType = ApplicationJsonContent };
            response.Content.SetString(ServerDataModel.GetSerializedStoreEntry(requestedEntry.key));
            return response;
        }
    }

    /// <summary>
    /// Handles PUT /adfs/Proxy/webapplicationproxy/store/{key}
    /// </summary>
    public class PutProxyStoreEntryRequestHandler : ProxyRequestHandlerBase
    {
        public override bool CanHandleRequest(HttpRequestMessage request)
        {
            return request.RequestUri.AbsolutePath.ContainsIgnoreCase(Constraints.StoreUrl) &&
                   request.Method == HttpMethod.PUT;
        }

        public override bool VerifyRequest(out string message)
        {
            if (!base.VerifyRequest(out message)) return false;
            message = "The incoming request must be valid StoreEntry";
            return JSONObject.TryParse<StoreEntry>(Request.Content.GetString());
        }

        public override HttpResponseMessage GetResponse()
        {
            return GetSuccessResponse();
        }

        public HttpResponseMessage GetSuccessResponse()
        {
            var requestedEntry = JSONObject.Parse<StoreEntry>(Request.Content.GetString());
            ServerDataModel.UpdateStoreEntry(requestedEntry);
            var response = new HttpResponseMessage(HttpStatusCode.OK) { ContentType = ApplicationJsonContent };
            response.Content.SetString(ServerDataModel.GetSerializedStoreEntry(requestedEntry.key));
            return response;
        }
    }
}
