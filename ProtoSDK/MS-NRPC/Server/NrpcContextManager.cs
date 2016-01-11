// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    ///  A class to manage all the session contexts of the server
    /// </summary>
    internal class NrpcContextManager 
    {
        private Dictionary<RpceServerSessionContext, NrpcServerSessionContext> sessionContextMap;

        private object lockObj;
        /// <summary>
        /// Initialize a NRPC context manager class.
        /// </summary>
        internal NrpcContextManager()
        {
            sessionContextMap = new Dictionary<RpceServerSessionContext, NrpcServerSessionContext>();
            lockObj = new object();
        }


        /// <summary>
        ///  Look up the NRPC session context using the session context of RPCE
        /// </summary>
        /// <param name="rpceSessionContext">The RPCE layer session context</param>
        /// <param name="sessionContext">The  corresponding NRPC session context</param>
        /// <returns>Whether the rpce session is a new session</returns>
        internal bool LookupSessionContext(RpceServerSessionContext rpceSessionContext,
            out NrpcServerSessionContext sessionContext)
        {
            lock (lockObj)
            {
                if (sessionContextMap.ContainsKey(rpceSessionContext))
                {
                    sessionContext = sessionContextMap[rpceSessionContext];
                    return false;
                }
                else
                {
                    sessionContext = new NrpcServerSessionContext();
                    sessionContextMap[rpceSessionContext] = sessionContext;
                    return true;
                }
            }
        }


        /// <summary>
        ///  Removes the link between the rpce session context with its corresponding
        ///  nrpc session context.
        /// </summary>
        /// <param name="rpceSessionContext">The rpce layer session context</param>
        internal void RemoveSessionContext(RpceServerSessionContext rpceSessionContext)
        {
            lock (lockObj)
            {
                sessionContextMap.Remove(rpceSessionContext);
            }
        }
    }
}
