// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2.Types
{
    /// <summary>
    /// Encode or decode RDPEUDP2 payload.
    /// </summary>
    public interface IRdpeudp2PayloadBase
    {
        /// <summary>
        /// Get bytes according to this payload.
        /// </summary>
        /// <returns>The payload bytes.</returns>
        byte[] ToBytes();
    }
}
