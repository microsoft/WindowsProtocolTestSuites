// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// The SProperty structure contains information about single property weight.
    /// </summary>
    public struct SProperty : IWSPObject
    {
        /// <summary>
        /// A 32-bit unsigned integer specifying a property identifier.
        /// </summary>
        public UInt32 _pid;

        /// <summary>
        /// A 32-bit unsigned integer specifying the weight to be used in probabilistic ranking.
        /// </summary>
        public UInt32 _weight;

        public void ToBytes(WSPBuffer buffer)
        {
            buffer.Add(this);
        }
    }
}
