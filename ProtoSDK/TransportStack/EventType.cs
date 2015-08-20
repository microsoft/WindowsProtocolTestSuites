// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Transport
{
    /// <summary>
    /// the type of event maybe occur in transport.
    /// </summary>
    [Flags]
    public enum EventType
    {
        /// <summary>
        /// A connect event occurred in transport.
        /// </summary>
        Connected = 1,

        /// <summary>
        /// A disconnect event occurred in transport.
        /// </summary>
        Disconnected = 2,

        /// <summary>
        /// A packet was received in transport.
        /// </summary>
        ReceivedPacket = 4,

        /// <summary>
        /// An exception was thrown in transport.
        /// </summary>
        Exception = 8,
    }
}
