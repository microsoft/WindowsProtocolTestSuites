// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// Manage endpoints
    /// </summary>
    public abstract class EndpointManager<T1, T2>
        where T1 : class 
        where T2 : class
    {
        /// <summary>
        /// Store two types of endpoint
        /// </summary>
        internal class EndpointPair<S1, S2>
            where S1 : class
            where S2 : class
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
            private S1 underProtocolEndpoint;
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
            private S2 currentProtocolEndpoint;

            /// <summary>
            /// The underlying protocol endpoint
            /// </summary>
            public S1 UnderProtocolEndpoint
            {
                get
                {
                    return underProtocolEndpoint;
                }
                set
                {
                    underProtocolEndpoint = value;
                }
            }

            /// <summary>
            /// The current protocol endpoint
            /// </summary>
            public S2 CurrentProtocolEndpoint
            {
                get
                {
                    return currentProtocolEndpoint;
                }
                set
                {
                    currentProtocolEndpoint = value;
                }
            }
        }

        /// <summary>
        /// Contains all endpoints
        /// </summary>
        private List<EndpointPair<T1, T2>> endPointPairs;

        /// <summary>
        /// Constructor
        /// </summary>
        protected EndpointManager()
        {
            endPointPairs = new List<EndpointPair<T1, T2>>();
        }


        /// <summary>
        /// Resolve underlying protocol endpoint from current protocol endpoint
        /// </summary>
        /// <param name="currentProtocolEndpoint">The currentProtocol endpoint</param>
        /// <returns>The underlying protocol endpoint</returns>
        public T1 GetUnderProtocolEndpoint(T2 currentProtocolEndpoint)
        {
            foreach (EndpointPair<T1, T2> endpointPair in endPointPairs)
            {
                if (endpointPair.CurrentProtocolEndpoint.Equals(currentProtocolEndpoint))
                {
                    return endpointPair.UnderProtocolEndpoint;
                }
            }

            throw new InvalidOperationException("The endpoint does not exist");
        }


        /// <summary>
        /// Resolve current protocol  endpoint from underlying protocol endpoint
        /// </summary>
        /// <param name="underProtocolEndpoint">The underlying protocol endpoint</param>
        /// <returns>The current protocol endpoint</returns>
        public T2 GetCurrentProtocolEndpoint(T1 underProtocolEndpoint)
        {
            foreach (EndpointPair<T1, T2> endpointPair in endPointPairs)
            {
                if (endpointPair.UnderProtocolEndpoint.Equals(underProtocolEndpoint))
                {
                    return endpointPair.CurrentProtocolEndpoint;
                }
            }

            throw new InvalidOperationException("The endpoint does not exist");
        }


        /// <summary>
        /// Add underlying protocol endpoint and return created current protocol endpoint
        /// </summary>
        /// <param name="underProtoclEndpoint">The underlying protocol endpoint</param>
        /// <returns>The new created current protocol endpoint</returns>
        public T2 AddEndpoint(T1 underProtoclEndpoint)
        {
            EndpointPair<T1, T2> endpointPair = new EndpointPair<T1, T2>();
            endpointPair.UnderProtocolEndpoint = underProtoclEndpoint;
            endpointPair.CurrentProtocolEndpoint = CreateCurrentProtocolEndpoint(underProtoclEndpoint);

            for (int i = 0; i < endPointPairs.Count; i++)
            {
                if (endPointPairs[i].UnderProtocolEndpoint.Equals(underProtoclEndpoint))
                {
                    throw new InvalidOperationException("The endpoint is existed.");
                }
            }

            endPointPairs.Add(endpointPair);

            return endpointPair.CurrentProtocolEndpoint;
        }


        /// <summary>
        /// Delete the endpoint, both current protocol endpoint and underlying protocol endpoint
        /// will be deleted. And after deleting, the underlying protocol endpoint will be returned
        /// </summary>
        /// <param name="currentProtocolEndpoint">The endpoint used by current protocol</param>
        /// <returns>The corresponding underlying protocol endpoint</returns>
        public T1 DeleteEndpoint(T2 currentProtocolEndpoint)
        {
            for (int i = 0; i < endPointPairs.Count; i++)
            {
                if (currentProtocolEndpoint.Equals(endPointPairs[i].CurrentProtocolEndpoint))
                {
                    T1 underProtocolEndpoint = endPointPairs[i].UnderProtocolEndpoint;
                    endPointPairs.RemoveAt(i);
                    return underProtocolEndpoint;
                }
            }

            throw new InvalidOperationException("The endpoint can't be found."); 
        }


        /// <summary>
        /// Create Rap endpoint information from smb endpoint
        /// </summary>
        /// <param name="underProtocolEndpoint">The underlying protocol endpoint</param>
        /// <returns>The rap endpoint</returns>
        protected abstract T2 CreateCurrentProtocolEndpoint(T1 underProtocolEndpoint);
    }
}
