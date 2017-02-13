// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// MS-ADFSPIP Client Adapter
    /// </summary>
    public interface IMSADFSPIPClientAdapter : IAdapter
    {
        /// <summary>
        /// The IDs of the requests that have not been responsed to.
        /// </summary>
        Queue<Guid> PendingRequests { get; }

        /// <summary>
        /// Receive request from the client with default timeout value.
        /// </summary>
        /// <remarks>
        /// This method will block the current thread until it gets a
        /// request or timeout.
        /// </remarks>
        /// <returns>
        /// The request message, or null if no request is received.
        /// </returns>
        HttpRequestMessage ExpectRequest();

        /// <summary>
        /// Receive request from the client.
        /// </summary>
        /// <remarks>
        /// This method will block the current thread until it gets a
        /// request or timeout.
        /// </remarks>
        /// <param name="timeout">
        /// The timeout value.
        /// </param>
        /// <returns>
        /// The request message, or null if no request is received.
        /// </returns>
        HttpRequestMessage ExpectRequest(TimeSpan timeout);

        /// <summary>
        /// Send response to the client, which responses the earliest 
        /// request that does not get a response.
        /// </summary>
        /// <param name="response">
        /// The response message to send.
        /// </param>
        void SendResponse(HttpResponseMessage response);

        /// <summary>
        /// Bind the server certificate to SSL port 443.
        /// </summary>
        /// <param name="certificate">
        /// The server certificate.
        /// </param>
        void BindCertificate(X509Certificate2 certificate);
    }
}
