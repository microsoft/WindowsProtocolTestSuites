// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// The factory class to get the request handler 
    /// corresponding to a certain request.
    /// </summary>
    public class ProxyRequestHandlerFactory : IRequestHandlerFactory
    {
        private readonly ServerDataModel _serverDataModel;
        private readonly List<IRequestHandler> _handlerPool;

        /// <summary>
        /// Get or set the default request handler.
        /// </summary>
        public IRequestHandler DefaultRequestHander { get; set; }

        public ProxyRequestHandlerFactory(ServerDataModel serverDataModel)
        {
            _handlerPool = new List<IRequestHandler>();
            _serverDataModel = serverDataModel;
        }

        /// <summary>
        /// Get the proper handler instance to handle the given request.
        /// </summary>
        /// <param name="request">The request from the client.</param>
        /// <returns>The handler instance which can handle the request.</returns>
        public IRequestHandler GetRequestHandler(HttpRequestMessage request)
        {
            // Find a handerl in the handler pool
            foreach (var handler in _handlerPool)
            {
                if (!handler.CanHandleRequest(request)) continue;
                handler.Request = request;
                return handler;
            }

            // If no handler was found, use default handler
            if (DefaultRequestHander != null) {
                DefaultRequestHander.Request = request;
                return DefaultRequestHander;
            }
            // if default handler is null, return null        
            return null;
        }

        /// <summary>
        /// Register a request handler in the hander pool.
        /// The factory can only return the request handlers registered.
        /// </summary>
        /// <typeparam name="T">A request handler.</typeparam>
        public void RegisterRequestHandler<T>() where T : IRequestHandler
        {
            var handler = (ProxyRequestHandlerBase)Activator.CreateInstance(typeof(T));

            handler.ServerDataModel = _serverDataModel;
            
            _handlerPool.Add(handler);
        }
    }
}
