// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrd
{
    using System.Net;
    using Microsoft.Protocols.TestTools;

    /// <summary>
    /// Delegation to receive probe message
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="probeMsg">The Probe message is part of the WS-Discovery standard</param>
    public delegate void ReceiveProbeMsgHandler(IPEndPoint sender, ProbeMsg probeMsg);

    /// <summary>
    /// A interface for pccrd client adapter
    /// </summary>
    public interface IPccrdClientAdapter : IAdapter
    {
        /// <summary>
        /// Receive event to record the received probe message
        /// </summary>
        event ReceiveProbeMsgHandler ReceiveProbeMessage;

        /// <summary>
        /// Start to listen the requests.
        /// </summary>
        void StartListening();

        /// <summary>
        /// Stop to listen the requests.
        /// </summary>
        void StopListening();

        /// <summary>
        /// Sends ProbeMatch messages.
        /// </summary>
        /// <param name="relatesTo">RelatesTo field</param>
        /// <param name="instanceId">InstaceId field identifier for the current instance
        /// of the device being published</param>
        /// <param name="messageNumber">MessageNumber field</param>
        /// <param name="matches">The service property matches</param>
        /// <param name="ip">The ip address</param>
        /// <param name="port">The used port number</param>
        void SendProbeMatchMessage(
            string relatesTo,
            string instanceId,
            uint messageNumber,
            ServiceProperty[] matches,
            string ip,
            int port);
    }
}
