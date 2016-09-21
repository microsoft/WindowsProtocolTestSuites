// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pchc
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The message type, 2 bytes.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1028:EnumStorageShouldBeInt32",
        Justification = @"According to the technical document,
            the PCHC_MESSAGE_TYPE is an unsigned 16-bit field.")]
    public enum PCHCMessageType : ushort
    {
        /// <summary>
        /// Only used by Fxcop
        /// </summary>
        None = 0,

        /// <summary>
        /// 0x0001, The message is an INITIAL_OFFER_MESSAGE (section 2.2.1.3).
        /// </summary>
        INITIAL_OFFER_MESSAGE = 0x0001,

        /// <summary>
        /// 0x0002, The message is a SEGMENT_INFO_MESSAGE (section 2.2.1.4).
        /// </summary>
        SEGMENT_INFO_MESSAGE = 0x0002
    }
}
