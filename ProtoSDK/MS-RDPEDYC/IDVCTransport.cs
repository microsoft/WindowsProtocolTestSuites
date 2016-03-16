// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpedyc
{
    public delegate void ReceivePacket(DynamicVCPDU pdu);
    public interface IDVCTransport : IDisposable
    {
        /// <summary>
        /// Send a Dynamic virtual channel PDU
        /// </summary>
        /// <param name="pdu"></param>
        void Send(DynamicVCPDU pdu);

        /// <summary>
        /// Event called when receive a Dynamic Virtual channel PDU
        /// </summary>
        event ReceivePacket Received;

        /// <summary>
        /// Transport type
        /// </summary>
        DynamicVC_TransportType TransportType {get;}
    }
}
