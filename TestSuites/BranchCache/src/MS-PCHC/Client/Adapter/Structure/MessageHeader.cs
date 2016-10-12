// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pchc
{
    /// <summary>
    /// All Peer Content Caching and Retrieval: Hosted Cache Protocol [MS-PCHC] 
    /// request messages use a common header, which consists of the following fields.
    /// </summary>
    public struct MessageHeader
    {
        /// <summary>
        /// MinorVersion (1 byte): The minor part of the version, which MUST be 0x00.
        /// </summary>
        public byte MinorVersion;

        /// <summary>
        /// MajorVersion (1 byte): The major part of the version, which MUST be 0x01.
        /// </summary>
        public byte MajorVersion;

        /// <summary>
        /// Type (2 bytes): A 16-bit unsigned integer that specifies the message type.
        /// </summary>
        public PCHCMessageType MsgType;

        /// <summary>
        /// Padding (4 bytes): The value of this field is indeterminate and MUST be ignored on processing
        /// </summary>
        public byte[] Padding;
    }
}
