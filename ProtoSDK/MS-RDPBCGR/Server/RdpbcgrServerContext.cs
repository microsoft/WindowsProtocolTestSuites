// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Compression.Mppc;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    /// <summary>
    /// Session Context Manager
    /// </summary>
    public class RdpbcgrServerContext : IDisposable
    {
        #region Field members
        //The collection for the RdpbcgrServerSessionContext.
        private Collection<RdpbcgrServerSessionContext> sessions;
        #endregion


        #region Constructor
        /// <summary>
        /// Perform all operation and store all the info for the RDPBCGR server context managing.
        /// </summary>
        internal RdpbcgrServerContext()
        {
            sessions = new Collection<RdpbcgrServerSessionContext>();
        }
        #endregion


        #region Properties
        /// <summary>
        /// Provide collection for sessions in the RDPBCGR server.
        /// </summary>
        public ReadOnlyCollection<RdpbcgrServerSessionContext> SessionContexts
        {
            get
            {
                return new ReadOnlyCollection<RdpbcgrServerSessionContext>(sessions);
            }
        }
        #endregion


        #region Internal methods
        /// <summary>
        /// Lookup the session context by session key
        /// </summary>
        /// <param name="sessionKey">the identity of connection</param>
        /// <returns>the connection object.</returns>
        public RdpbcgrServerSessionContext LookupSession(object identity)
        {
            foreach (RdpbcgrServerSessionContext sessionContext in this.SessionContexts)
            {
                if (sessionContext.Identity == identity)
                {
                    return sessionContext;
                }
            }

            return null;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            foreach (RdpbcgrServerSessionContext sessionContext in this.SessionContexts)
            {
                if (sessionContext!= null)
                {
                    sessionContext.Dispose();
                }
              
            }
        }
        /// <summary>
        /// Add session context to server context.
        /// </summary>
        /// <param name="sessionContext">Add session context.</param>
        internal void AddSession(RdpbcgrServerSessionContext sessionContext)
        {
            lock (this.sessions)
            {
                if (this.sessions.Contains(sessionContext))
                {
                    throw new InvalidOperationException(
                        "the sessionContext is exists in the sessionContext collection, can not add duplicate sessionContext");
                }

                this.sessions.Add(sessionContext);
            }
        }


        /// <summary>
        /// Remove session context from server context. If connection does not exists, do nothing.
        /// </summary>
        /// <param name="sessionContext">Add session context.</param>
        internal void RemoveSession(RdpbcgrServerSessionContext sessionContext)
        {
            lock (this.sessions)
            {
                if (this.SessionContexts.Contains(sessionContext))
                {
                    this.sessions.Remove(sessionContext);
                }
            }
        }
        #endregion
    }
}
