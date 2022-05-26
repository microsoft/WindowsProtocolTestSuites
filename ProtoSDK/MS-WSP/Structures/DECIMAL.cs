// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// DECIMAL is used to represent an exact numeric value with a fixed precision and fixed scale.
    /// </summary>
    public struct DECIMAL : IWspStructure
    {
        /// <summary>
        /// The high 32 bits of the number.
        /// </summary>
        public uint Hi32;

        /// <summary>
        /// The low 64 bits of the number.
        /// </summary>
        public ulong Lo64;

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
