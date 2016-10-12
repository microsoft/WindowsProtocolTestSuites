// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// ResponseCode values.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1028:EnumStorageShouldBeInt32",
        Justification = @"According to the technical document,
            the RESPONSE_CODE is an unsigned 8-bit field.")]
    public enum RESPONSE_CODE : byte
    {
        /// <summary>
        /// OK 0x00:The server has sufficient information to download content from the client.
        /// </summary>
        OK = 0x00,

        /// <summary>
        /// INTERESTED 0x01:The server needs the range of block hashes from the client before it can 
        /// download content from the client.
        /// </summary>
        INTERESTED = 0x01
    }
}
