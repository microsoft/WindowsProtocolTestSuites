// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// The CColumnGroupArray structure contains a set of property groups with weights for each property.
    /// </summary>
    public struct CColumnGroupArray : IWSPObject
    {
        /// <summary>
        /// A 32-bit unsigned integer containing the number of elements in the aGroupArray array.
        /// </summary>
        public UInt32 count;

        /// <summary>
        /// An array of CColumnGroup structures indicating individual weights for each property, which are used in probabilistic ranking.
        /// </summary>
        public CColumnGroup[] aGroupArray;

        public void ToBytes(WSPBuffer buffer)
        {
            buffer.Add(count);

            foreach (var group in aGroupArray)
            {
                group.ToBytes(buffer);
            }
        }
    }
}
