// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    public enum CCategSpec_ulCategType_Values : uint
    {
        /// <summary>
        /// Unique categorization. Each unique value forms a category.
        /// </summary>
        CATEGORIZE_UNIQUE = 0x00000000,

        /// <summary>
        /// Range categorization. Ranges are explicitly specified in CRangeCategSpec.
        /// </summary>
        CATEGORIZE_RANGE = 0x00000003,

        /// <summary>
        /// Categorization by completion suggestions. All the parameters for specifying how these groups are built are in CCompletionCategSpec.
        /// </summary>
        CATEGORIZE_COMPLETION = 0x00000004,
    }

    /// <summary>
    /// The CCategSpec structure contains information about which grouping to perform over query results.
    /// </summary>
    public struct CCategSpec : IWspStructure
    {
        /// <summary>
        /// Indicating the type of grouping to perform.
        /// </summary>
        public CCategSpec_ulCategType_Values _ulCategType;

        /// <summary>
        /// A CSort structure, specifying the sort order for the group.
        /// </summary>
        public CSort _sortKey;

        /// <summary>
        /// A CRangeCategSpec structure specifying the range values. This field MUST be omitted if _ulCategType is set to CATEGORIZE_UNIQUE; otherwise it MUST be present.
        /// </summary>
        public CRangeCategSpec? CRangeCategSpec;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(_ulCategType);

            _sortKey.ToBytes(buffer);

            if (CRangeCategSpec.HasValue)
            {
                CRangeCategSpec.Value.ToBytes(buffer);
            }
        }
    }
}
