// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// The CSortAggregSet structure contains information about group sorting.
    /// </summary>
    public struct CSortAggregSet : IWSPObject
    {
        /// <summary>
        /// A 32-bit unsigned integer specifying the number of entries in SortKeys.
        /// </summary>
        public UInt32 cCount;

        /// <summary>
        /// An array of CAggregSortKey structures, each describing a sort order.
        /// </summary>
        public CAggregSortKey[] SortKeys;

        public void ToBytes(WSPBuffer buffer)
        {
            buffer.Add(cCount);

            foreach (var sortKey in SortKeys)
            {
                sortKey.ToBytes(buffer);
            }
        }
    }
}
