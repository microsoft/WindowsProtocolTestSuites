// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the interfaces for TransportStack, from which other interface extend.
    /// </summary>
    internal interface ITransport : IDisposable
    {
        /// <summary>
        /// get a bool value that indicates whether there is data received from transport.
        /// </summary>
        bool IsDataAvailable
        {
            get;
        }


        /// <summary>
        /// add transport event to transport, TSD can invoke ExpectTransportEvent to get it.
        /// </summary>
        /// <param name="transportEvent">
        /// a TransportEvent object that contains the event to add to the queue
        /// </param>
        void AddEvent(TransportEvent transportEvent);


        /// <summary>
        /// to update the config of transport at runtime.
        /// </summary>
        /// <param name="config">
        /// a TransportConfig object that contains the config to update
        /// </param>
        void UpdateConfig(TransportConfig config);


        /// <summary>
        /// expect transport event from transport.<para/>
        /// if event arrived and packet data buffer is empty, return event directly.<para/>
        /// decode packet from packet data buffer, return packet if arrived, otherwise, return event.
        /// </summary>
        /// <param name="timeout">
        /// a TimeSpan struct that specifies the timeout of waiting for a packet or event from the transport.
        /// </param>
        /// <returns>
        /// a TransportEvent object that contains the expected event from transport.
        /// </returns>
        TransportEvent ExpectTransportEvent(TimeSpan timeout);
    }
}
