// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrr
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The type of content in server. Used in sut-client.
    /// </summary>
    public enum _CONTENTTYPE
    {
        /// <summary>
        /// Three or fewer consecutive blocks on the server peer 1 or hosted-cache server
        /// </summary>
        ThreeOrFewerConsecutiveBlks,

        /// <summary>
        /// More than three consecutive blocks on the server peer 1 or hosted-cache server
        /// </summary>
        MoreThanThreeConsecutiveBlks,
    }

    /// <summary>
    /// Indicate which hash algorithm to use.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1008:EnumsShouldHaveZeroValue",
        Justification = @"According to the technical document, the DwHashAlgo_Values don't have zero.")]
    public enum DWHashAlgValues : int
    {
        /// <summary>
        ///  Use the SHA-256 hash algorithm.
        /// </summary>
        V1 = 0x0000800C,

        /// <summary>
        ///  Use the SHA-384 hash algorithm.
        /// </summary>
        V2 = 0x0000800D,

        /// <summary>
        ///  Use the SHA-512 hash algorithm.
        /// </summary>
        V3 = 0x0000800E,
    }

    /// <summary>
    ///  A BLOCK_RANGE is an array of two integers that defines
    ///  a consecutive array of blocks.
    /// </summary>
    public partial struct _BLOCKRANGE
    {
        /// <summary>
        ///  The index of the first block in the range.
        /// </summary>
        public uint Index;

        /// <summary>
        ///  Count of consecutive adjacent blocks in that range,
        ///  including the block at the Index location. The value
        ///  of this field MUST be greater than 0.
        /// </summary>
        public uint Count;
    }
}
