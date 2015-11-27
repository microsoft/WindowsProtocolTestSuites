// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// The bind time feature negotiation bitmask is an array of two octets, 
    /// each of which is interpreted as a bitmask.<para/>
    /// Currently, only the two least significant bits in the first element 
    /// of the array are defined.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum RpceBindTimeFeatureNegotiationBitmask : byte
    {
        /// <summary>
        /// No flag is set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Supports security context multiplexing, as specified in section 3.3.1.5.4.
        /// </summary>
        SecurityContextMultiplexingSupported = 0x01,

        /// <summary>
        /// Supports keeping the connection open after sending the orphaned PDU, 
        /// as specified in section 3.3.1.5.11.
        /// </summary>
        KeepConnectionOnOrphanSupported = 0x02,

        /// <summary>
        /// Server supports keeping the connection open after sending the orphaned PDU, 
        /// as specified in section 3.3.1.5.11.
        /// </summary>
        KeepConnectionOnOrphanSupported_Resp = 0x10
    }
}
