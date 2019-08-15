// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// DECIMAL is used to represent an exact numeric value with a fixed precision and fixed scale.
    /// </summary>
    public struct DECIMAL : IWspStructure
    {
        /// <summary>
        /// The highest 32 bits of the 96-bit integer.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public UInt64 Hi32;

        /// <summary>
        /// The lowest 32 bits of the 96-bit integer.
        /// </summary>
        public UInt64 Lo32;

        /// <summary>
        /// The middle 32 bits of the 96-bit integer.
        /// </summary>
        public UInt64 Mid32;

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
