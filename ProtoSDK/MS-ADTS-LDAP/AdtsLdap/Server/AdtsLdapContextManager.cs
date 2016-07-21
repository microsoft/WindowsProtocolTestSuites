// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// Context manager that manages contexts of LDAP server.
    /// </summary>
    internal class AdtsLdapContextManager : IDisposable
    {
        /// <summary>
        /// The context dictionary.
        /// Key  : endpoint(ip address + remote port + TCP/UDP).
        /// Value: server context.
        /// </summary>
        private volatile Dictionary<string, AdtsLdapContext> serverContexts;

        /// <summary>
        /// Lock object for thread-safe synchronization.
        /// </summary>
        private object lockObject;

        /// <summary>
        /// Constructor.
        /// </summary>
        internal AdtsLdapContextManager()
        {
            this.lockObject = new object();
            this.serverContexts = new Dictionary<string, AdtsLdapContext>();
        }


        /// <summary>
        /// Remove all contexts.
        /// </summary>
        internal void Clear()
        {
            this.serverContexts.Clear();
        }


        /// <summary>
        /// Gets a context.
        /// </summary>
        /// <param name="clientAddress">Address of client.</param>
        /// <param name="isTcp">Indicates whether the connection is TCP.</param>
        /// <returns>The context that corresponds to the information provided.</returns>
        internal AdtsLdapContext GetContext(
            IPEndPoint clientAddress,
            bool isTcp)
        {
            string key = GetKey(clientAddress, isTcp);
            lock (this.lockObject)
            {
                if (this.serverContexts.ContainsKey(key))
                {
                    return this.serverContexts[key];
                }
            }
            return null;
        }


        /// <summary>
        /// Adds a context.
        /// </summary>
        /// <param name="context">The context to be added</param>
        /// <exception cref="ArgumentException">
        /// Thrown if the context exists already.
        /// </exception>
        internal void AddContext(AdtsLdapContext context, bool isTcp)
        {
            string key = GetKey(context.RemoteAddress, isTcp);
            lock (this.lockObject)
            {
                this.serverContexts.Add(key, context);
            }
        }


        /// <summary>
        /// Removes the context. If the context doesn't exist, do nothing.
        /// </summary>
        /// <param name="clientAddress">Address of client.</param>
        /// <param name="isTcp">Indicates whether the connection is TCP.</param>
        internal void RemoveContext(IPEndPoint clientAddress, bool isTcp)
        {
            string key = GetKey(clientAddress, isTcp);
            lock (this.lockObject)
            {
                if (this.serverContexts.ContainsKey(key))
                {
                    this.serverContexts.Remove(key);
                }
            }
        }


        /// <summary>
        /// Gets a string key representing a connecting client.
        /// </summary>
        /// <param name="clientAddress">Address of client.</param>
        /// <param name="isTcp">Indicates whether the connection is TCP.</param>
        /// <returns>The string key.</returns>
        internal string GetKey(IPEndPoint clientAddress, bool isTcp)
        {
            // Just use "1" for TCP identifier and "0" for UDP to create the key.
            return clientAddress.ToString() + (isTcp ? "1" : "0");
        }


        #region IDisposable Members

        /// <summary>
        /// the dispose flags 
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Release the managed and unmanaged resources. 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // Take this object out of the finalization queue of the GC:
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release resources. 
        /// </summary>
        /// <param name = "disposing">
        /// If disposing equals true, Managed and unmanaged resources are disposed. if false, Only unmanaged resources 
        /// can be disposed. 
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types:
                    if (this.serverContexts != null)
                    {
                        // dispose all security object in context.
                        foreach (AdtsLdapContext context in this.serverContexts.Values)
                        {
                            if (context != null && context.Security != null)
                            {
                                context.Security.Dispose();
                            }
                        }

                        this.serverContexts = null;
                    }
                }

                // Call the appropriate methods to clean up unmanaged resources.
                // If disposing is false, only the following code is executed:

                this.disposed = true;
            }
        }


        /// <summary>
        /// finalizer 
        /// </summary>
        ~AdtsLdapContextManager()
        {
            Dispose(false);
        }

        #endregion
}
}
