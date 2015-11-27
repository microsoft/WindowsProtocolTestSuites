// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    ///  A class to manage all the RPCE server contexts.
    /// </summary>
    internal sealed class RpceContextManager
    {
        #region Field members

        private Dictionary<string, RpceServerContext> serverContexts;

        #endregion


        #region ctor
        
        /// <summary>
        /// Initialize a RPCE context manager class.
        /// </summary>
        internal RpceContextManager()
        {
            this.serverContexts = new Dictionary<string, RpceServerContext>();
        }
        
        #endregion


        #region Properties

        /// <summary>
        /// A read-only collection for serverContexts.
        /// </summary>
        internal ReadOnlyCollection<RpceServerContext> ServerContexts
        {
            get
            {
                lock (this.serverContexts)
                {
                    return new ReadOnlyCollection<RpceServerContext>(
                        new List<RpceServerContext>(this.serverContexts.Values));
                }
            }
        }

        #endregion

        #region Manage RPCE Server Contexts

        // Build the key of server context dictionary.
        internal static string BuildServerContextKey(string protocolSequence, string endpoint)
        {
            protocolSequence = protocolSequence.ToUpper();
            endpoint = endpoint.ToUpper();
            return String.Format("{0}:{1}", protocolSequence, endpoint);
        }


        // Add a server context to context collection.
        internal void AddServerContext(RpceServerContext serverContext)
        {
            string key = BuildServerContextKey(serverContext.ProtocolSequence, serverContext.Endpoint);
            lock (this.serverContexts)
            {
                if (this.serverContexts.ContainsKey(key))
                {
                    throw new InvalidOperationException("Server context already exists.");
                }
                this.serverContexts.Add(key, serverContext);
            }
        }


        // Remove serverContext from context collection.
        internal void RemoveServerContext(RpceServerContext serverContext)
        {
            string key = BuildServerContextKey(serverContext.ProtocolSequence, serverContext.Endpoint);
            lock (this.serverContexts)
            {
                if (this.serverContexts.ContainsKey(key))
                {
                    this.serverContexts.Remove(key);
                }
            }
        }


        // Lookup a RpceServerContext in dictionary.
        internal RpceServerContext LookupServerContext(string protocolSequence, string endpoint)
        {
            RpceServerContext serverContext;
            string key = BuildServerContextKey(protocolSequence, endpoint);

            lock (this.serverContexts)
            {
                if (!this.serverContexts.TryGetValue(key, out serverContext))
                {
                    serverContext = null;
                }
            }

            return serverContext;
        }


        /// <summary>
        /// Lookup the RPCE session context.
        /// </summary>
        /// <param name="remoteEndpoint">remote Endpoint. (IPEndPoint or SmbFile)</param>
        /// <returns>The sessionContext with the specific key.</returns>
        internal RpceServerSessionContext LookupSessionContext(object remoteEndpoint)
        {
            lock (this.serverContexts)
            {
                foreach (KeyValuePair<string, RpceServerContext> item in this.serverContexts)
                {
                    RpceServerSessionContext sessionContext 
                        = item.Value.LookupSessionContext(remoteEndpoint);
                    if (sessionContext != null)
                    {
                        return sessionContext;
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
