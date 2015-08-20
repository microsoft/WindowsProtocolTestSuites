// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Compression.Mppc
{
    /// <summary>
    /// Compressed mode, 8k or 64k
    /// </summary>
    public enum SlidingWindowSize
    {
        /// <summary>
        /// use 8k sliding window
        /// </summary>
        EightKB,

        /// <summary>
        /// use 64k sliding window
        /// </summary>
        SixtyFourKB,
    }
}
