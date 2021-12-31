// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    public enum CAggregSortKey_order_Values : uint
    {
        /// <summary>
        /// The rows are to be sorted in ascending order based on the values in the column specified.
        /// </summary>
        QUERY_SORTASCEND = 0x00000000,

        /// <summary>
        /// The rows are to be sorted in descending order based on the values in the column specified.
        /// </summary>
        QUERY_DESCEND = 0x00000001,
    }

    /// <summary>
    /// The CAggregSortKey structure contains information about sort order over single column.
    /// </summary>
    public struct CAggregSortKey : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer specifying sort order.
        /// </summary>
        public CAggregSortKey_order_Values order;

        /// <summary>
        /// A CAggregSpec structure specifying which column to sort by.
        /// </summary>
        public CAggregSpec ColumnSpec;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(order);

            ColumnSpec.ToBytes(buffer);
        }
    }
}
