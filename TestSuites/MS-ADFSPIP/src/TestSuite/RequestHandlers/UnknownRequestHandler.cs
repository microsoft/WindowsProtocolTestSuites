// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP.RequestHandlers
{
    /// <summary>
    /// Handles all unknown requests. Throws exception for unknown requests.
    /// </summary>
    class UnknownRequestHandler : ProxyRequestHandlerBase
    {
        public override bool CanHandleRequest(HttpRequestMessage request)
        {
            return true;
        }

        public override bool VerifyRequest(out string message)
        {
            message = "Received unknown request: " + Request.RequestUri.AbsolutePath;
            return false;
        }

        public override HttpResponseMessage GetResponse()
        {
            throw new Exception("Received unknown request: " + Request.RequestUri.AbsolutePath);
        }
    }
}
