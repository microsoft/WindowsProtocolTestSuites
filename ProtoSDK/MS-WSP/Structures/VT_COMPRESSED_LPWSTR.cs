// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The VT_COMPRESSED_LPWSTR structure contains a compressed version of a null-terminated, 16-bit Unicode string.
    /// </summary>
    public struct VT_COMPRESSED_LPWSTR : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer, indicating the number of characters in the compressed Unicode string, excluding the terminating null character.
        /// A value of 0x00000000 indicates that no such string is present.
        /// </summary>
        public uint ccLen;

        /// <summary>
        /// A sequence of bytes, each representing the lower byte of a two-byte Unicode character, where the higher byte of the character is always set to zero.
        /// Note that only the first 255 Unicode characters can be represented with this encoding scheme. This field MUST be absent if ccLen is set to 0x00000000.
        /// </summary>
        public byte[] bytes;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(ccLen);

            buffer.AddRange(bytes);
        }
    }
}
