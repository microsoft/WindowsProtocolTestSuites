// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    public enum dwOrder_Values : UInt32
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

    public enum dwIndividual_Values : UInt32
    {
        /// <summary>
        /// The complete property is used for sorting, resulting in a single row for each result.
        /// </summary>
        QUERY_SORTALL = 0x00000000,

        /// <summary>
        /// Each element of the VT_VECTOR is used for sorting independently, possibly resulting in multiple rows for a single result.
        /// </summary>
        QUERY_SORTINDIVIDUAL = 0x00000001,
    }

    /// <summary>
    /// The CSort structure identifies a column, direction, and locale to sort by.
    /// </summary>
    public struct CSort : IWSPObject
    {
        /// <summary>
        /// A 32-bit unsigned integer. This is the index in CPidMapper for the property to sort by.
        /// </summary>
        public UInt32 pidColumn;

        /// <summary>
        /// A 32-bit unsigned integer specifying how to sort based on the column.
        /// </summary>
        public dwOrder_Values dwOrder;

        /// <summary>
        /// A 32-bit unsigned integer. dwIndividual specifies how to treat properties of type VT_VECTOR with regard to sorting.
        /// </summary>
        public dwIndividual_Values dwIndividual;

        /// <summary>
        /// A 32-bit unsigned integer indicating the locale (as specified in [MS-LCID]) of the column.
        /// The locale determines the sorting rules to use when sorting textual values.
        /// The GSS can use the appropriate operating system facilities to do this.
        /// </summary>
        public UInt32 locale;

        public void ToBytes(WSPBuffer buffer)
        {
            buffer.Add(this);
        }
    }
}
