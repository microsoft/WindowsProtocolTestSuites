// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.CommonStack
{
    using System;
    using System.Net;

    /// <summary>
    /// The http request event arg.
    /// </summary>
    public class HttpRequestEventArg : EventArgs
    {
        /// <summary>
        /// The HttpListenerContext instance.
        /// </summary>
        private HttpListenerContext listenerContext;

        /// <summary>
        /// Initializes a new instance of the HttpRequestEventArg class.
        /// </summary>
        /// <param name="listenerContext">The HttpListenerContext instance.</param>
        public HttpRequestEventArg(HttpListenerContext listenerContext)
        {
            this.listenerContext = listenerContext;
        }

        /// <summary>
        /// Gets the property: HttpListenerContext instance.
        /// </summary>
        public HttpListenerContext ListenerContext
        {
            get { return this.listenerContext; }
        }
    }
}
