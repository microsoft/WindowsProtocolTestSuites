// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CInGroupSortAggregSets structure contains information on how the group is sorted with regard to the parent's group ranges.
    /// </summary>
    public struct CInGroupSortAggregSets : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer specifying the number of entries in SortSets.
        /// </summary>
        public uint cCount;

        /// <summary>
        /// A 4-byte field that must be ignored.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// An array of CSortSet structures.
        /// </summary>
        public CSortSet[] SortSets;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(cCount, 4);

            if (cCount > 0)
            {
                buffer.Add(Reserved);

                foreach (var sortSet in SortSets)
                {
                    sortSet.ToBytes(buffer);
                }
            }
        }
    }
}
