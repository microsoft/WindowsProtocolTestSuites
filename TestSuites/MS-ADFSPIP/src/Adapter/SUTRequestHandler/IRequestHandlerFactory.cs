// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// The factory class to retrieve a handler which handles
    /// a certain HTTP request.
    /// </summary>
    public interface IRequestHandlerFactory
    {
        /// <summary>
        /// Register a handler type into the factory, so that the 
        /// factory knows such a handler exists.
        /// </summary>
        /// <typeparam name="T">
        /// A request handler class which inherits IRequestHandler.
        /// </typeparam>
        void RegisterRequestHandler<T>() where T : IRequestHandler;

        /// <summary>
        /// Get the proper handler which handles the request.
        /// </summary>
        /// <param name="request">
        /// The HTTP request to handle.
        /// </param>
        /// <returns>
        /// An instance of IRequestHandler which can handle the request.
        /// </returns>
        IRequestHandler GetRequestHandler(HttpRequestMessage request);
    }
}
