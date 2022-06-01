// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CCategorizationSpec structure specifies how grouping is done at one level in a hierarchical query.
    /// </summary>
    public struct CCategorizationSpec : IWspStructure
    {
        /// <summary>
        /// A CColumnSet structure indicating the columns to return at that level in a hierarchical result set.
        /// </summary>
        public CColumnSet _csColumns;

        /// <summary>
        /// A CCategSpec structure specifying the type of categorization and the column for the group.
        /// </summary>
        public CCategSpec _Spec;

        /// <summary>
        /// A CAggregSet structure specifying aggregate information for the group.
        /// </summary>
        public CAggregSet _AggregSet;

        /// <summary>
        /// A CSortAggregSet structure specifying default sorting for the group.
        /// </summary>
        public CSortAggregSet _SortAggregSet;

        /// <summary>
        /// A CInGroupSortAggregSets structure specifying sorting for the group with regard to the parent group's range boundaries.
        /// </summary>
        public CInGroupSortAggregSets _InGroupSortAggregSets;

        /// <summary>
        /// A 32-bit unsigned integer. Reserved.
        /// Note MUST be set to 0x00000000.
        /// </summary>
        public uint _cMaxResults;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            _csColumns.ToBytes(buffer);

            _Spec.ToBytes(buffer);

            _AggregSet.ToBytes(buffer);

            _SortAggregSet.ToBytes(buffer);

            _InGroupSortAggregSets.ToBytes(buffer);

            buffer.Add(_cMaxResults);
        }
    }
}
