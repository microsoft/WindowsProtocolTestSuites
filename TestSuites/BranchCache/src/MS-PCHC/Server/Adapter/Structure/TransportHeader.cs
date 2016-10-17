// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pchc
{
    /// <summary>
    /// The transport adds the following header in front of any response protocol message.
    /// </summary>
    public struct TransportHeader
    {
        /// <summary>
        /// Size (4 bytes): Total message size in bytes, excluding this field.
        /// </summary>
        public uint Size;
    }
}
