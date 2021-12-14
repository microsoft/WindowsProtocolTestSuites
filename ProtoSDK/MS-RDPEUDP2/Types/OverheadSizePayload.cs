// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2.Types
{
    /// <summary>
    /// The OverheadSize payload carries the overhead bytes for a packet at the RDP-UDP2 layer.
    /// </summary>
    public struct OverheadSizePayload : IRdpeudp2PayloadBase
    {
        /// <summary>
        /// An 8-bit unsigned integer that specifies the average header size of the extra bytes prepended from the RDP-UDP2 transport layer to the raw UDP layer.
        /// </summary>
        public byte OverheadSize;

        public OverheadSizePayload(ReadOnlySpan<byte> data, out int consumedLength)
        {
            OverheadSize = data[0];

            consumedLength = 1;
        }

        public byte[] ToBytes()
        {
            return new byte[] { OverheadSize };
        }
    }
}
