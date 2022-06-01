// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CColumnGroupArray structure contains a set of property groups with weights for each property.
    /// </summary>
    public struct CColumnGroupArray : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer containing the number of elements in the aGroupArray array.
        /// </summary>
        public uint count;

        /// <summary>
        /// An array of CColumnGroup structures indicating individual weights for each property, which are used in probabilistic ranking.
        /// </summary>
        public CColumnGroup[] aGroupArray;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(count);

            if (aGroupArray != null)
            {
                foreach (var group in aGroupArray)
                {
                    group.ToBytes(buffer);
                }
            }
        }
    }
}
