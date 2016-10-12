// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrd
{
    using Microsoft.Protocols.TestTools;

    /// <summary>
    /// Delegation of message receiver
    /// </summary>
    /// <param name="status">The probeMatchMsg status</param>
    /// <param name="probeMatchMsg">The ProbeMatchMsg respone to probe message</param>
    public delegate void ReceiveProbeMatchMsgHandler(Status status, ProbeMatchMsg probeMatchMsg);

    /// <summary>
    /// Pccrd server adapter interface.
    /// </summary>
    public interface IPccrdServerAdapter : IAdapter
    {
        /// <summary>
        /// Event handler
        /// </summary>
        event ReceiveProbeMatchMsgHandler ReceiveProbeMatchMessage;

        /// <summary>
        /// Used to send Probe message.
        /// </summary>
        /// <param name="type">The probe message type.</param>
        /// <param name="scope">The scope of probe message.</param>
        void SendProbeMessage(string type, string scope);
    }
}
