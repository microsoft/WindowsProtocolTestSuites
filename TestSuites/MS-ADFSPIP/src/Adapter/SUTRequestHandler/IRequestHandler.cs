// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// Defines the very basic methods to handle a HTTP request.
    /// </summary>
    public interface IRequestHandler
    {
        /// <summary>
        /// Get or the set the request message handled by the handler.
        /// </summary>
        HttpRequestMessage Request { get; set; }
        
        /// <summary>
        /// Check whether the hander can handle the specified request.
        /// </summary>
        /// <param name="request">The http request.</param>
        /// <returns>
        /// True if this handler can handle the request; false, otherwise
        /// </returns>
        bool CanHandleRequest(HttpRequestMessage request);

        /// <summary>
        /// The normal response when the client request is valid.
        /// </summary>
        /// <returns>
        /// Response to the client request.
        /// </returns>
        HttpResponseMessage GetResponse();
    }
}
