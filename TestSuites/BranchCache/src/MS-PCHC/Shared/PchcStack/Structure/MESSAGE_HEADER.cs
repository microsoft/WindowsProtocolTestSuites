// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc
{
    /// <summary>
    /// The common header of request message.
    /// </summary>
    public struct MESSAGE_HEADER
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
        [ByteOrder(EndianType.BigEndian)]
        public PCHC_MESSAGE_TYPE MsgType;

        /// <summary>
        /// Padding (4 bytes): The value of this field is indeterminate and MUST be ignored on processing
        /// </summary>
        [StaticSize(4)]
        public byte[] Padding;
    }
}
