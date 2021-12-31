// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The SProperty structure contains information about single property weight.
    /// </summary>
    public struct SProperty : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer specifying a property identifier.
        /// </summary>
        public uint _pid;

        /// <summary>
        /// A 32-bit unsigned integer specifying the weight to be used in probabilistic ranking.
        /// </summary>
        public uint _weight;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(this);
        }
    }
}
