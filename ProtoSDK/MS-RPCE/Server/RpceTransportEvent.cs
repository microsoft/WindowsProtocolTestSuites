// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// A wrapper of an event occurs in transport.
    /// </summary>
    internal class RpceTransportEvent : TransportEvent
    {
        #region Fields members

        // received data
        private RpcePdu pdu;
        private RpceServerContext serverContext;
        private RpceServerSessionContext sessionContext;

        #endregion


        #region ctor


        /// <summary>
        /// Initialize a RpceTransportEvent.
        /// </summary>
        /// <param name="type">the type of the occurred event.</param>
        /// <param name="remoteEndpoint">the remote endpoint from which the event occurred.</param>
        /// <param name="localEndpoint">the local endpoint from which the event occurred.</param>
        /// <param name="serverContext">The server context.</param>
        /// <param name="sessionContext">The session context.</param>
        /// <param name="pdu">A received PDU.</param>
        internal RpceTransportEvent(
            EventType type, 
            object remoteEndpoint,
            object localEndpoint,
            RpceServerContext serverContext,
            RpceServerSessionContext sessionContext,
            RpcePdu pdu) : base(type, remoteEndpoint, localEndpoint, pdu)
        {
            //because sessionContext might be null when accept a connection, we must pass it in.
            this.serverContext = serverContext;
            this.sessionContext = sessionContext;
            this.pdu = pdu;
        }


        /// <summary>
        /// Initialize a RpceTransportEvent.
        /// </summary>
        /// <param name="type">the type of the occurred event.</param>
        /// <param name="remoteEndpoint">the remote endpoint from which the event occurred.</param>
        /// <param name="localEndpoint">the local endpoint from which the event occurred.</param>
        /// <param name="detail">the details of the occurred event. It may be null if no detail needed.</param>
        internal RpceTransportEvent(
            EventType type,
            object remoteEndpoint,
            object localEndpoint,
            object detail) : base(type, remoteEndpoint, localEndpoint, detail)
        {
        }

        #endregion
        
        
        #region Properties

        internal RpceServerContext ServerContext
        {
            get
            {
                return this.serverContext;
            }
        }

        internal RpceServerSessionContext SessionContext
        {
            get
            {
                return this.sessionContext;
            }
        }

        /// <summary>
        /// The PDU received.
        /// </summary>
        internal RpcePdu Pdu
        {
            get
            {
                return this.pdu;
            }
        }


        #endregion
    }
}
