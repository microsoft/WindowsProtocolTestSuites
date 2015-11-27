// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// Event Filter.
    /// </summary>
    internal class RpceEventFilter
    {
        #region field members

        private EventType eventType;
        private object remoteEndpoint;

        #endregion


        #region ctor

        /// <summary>
        /// Initialize Rpce Event Filter.
        /// </summary>
        /// <param name="eventType">The eventType of the TransportEvent.</param>
        /// <param name="remoteEndpoint">The key of the sessionContext.</param>
        internal RpceEventFilter(EventType eventType, object remoteEndpoint)
            : this(eventType)
        {
            this.remoteEndpoint = remoteEndpoint;
        }


        /// <summary>
        /// Initialize Rpce Event Filter.
        /// </summary>
        /// <param name="eventType">The eventType of the TransportEvent.</param>
        internal RpceEventFilter(EventType eventType)
        {
            // Always throw exception when there's any in event queue.
            this.eventType = eventType | EventType.Exception;
        }

        #endregion


        #region public method

        /// <summary>
        /// A callback to filter.
        /// </summary>
        /// <param name="obj">obj to filter.</param>
        /// <returns>true if the obj match the condition.</returns>
        internal bool EventFilter(RpceTransportEvent obj)
        {
            if (remoteEndpoint == null)
            {
                return (obj.EventType & this.eventType) != 0;
            }
            else
            {
                return (obj.EventType & this.eventType) != 0
                    && this.remoteEndpoint.Equals(obj.RemoteEndPoint);
            }
        }

        #endregion
    }
}
