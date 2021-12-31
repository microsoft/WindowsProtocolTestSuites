// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CRangeCategSpec structure contains information about ranges for grouping into range-specified buckets.
    /// </summary>
    public struct CRangeCategSpec : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer. Reserved. This field can be set to any arbitrary value when sent.
        /// </summary>
        public uint _lcid;

        /// <summary>
        /// A 32-bit unsigned integer, indicating the number of RANGEBOUNDARY structures in aRangeBegin.
        /// </summary>
        public uint cRange;

        /// <summary>
        /// An array of RANGEBOUNDARY structures, specifying a set of ranges for which grouping is performed.
        /// Note that the first range is from minimum value to the boundary, represented by the first RANGEBOUNDARY structure.
        /// The next range is from where the first boundary cut off to the boundary represented by the second RANGEBOUNDARY structure, and so on.
        /// The last range includes all the items greater than the last RANGEBOUNDARY structure to the maximum value.
        /// There will be a total of cRange + 1 ranges. Values with vType set to VT_NULL and VT_EMPTY are always in the last group, regardless of sort order.
        /// </summary>
        public RANGEBOUNDARY[] aRangeBegin;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(_lcid);

            buffer.Add(cRange);

            foreach (var range in aRangeBegin)
            {
                buffer.AlignWrite(4);

                range.ToBytes(buffer);
            }
        }
    }
}
