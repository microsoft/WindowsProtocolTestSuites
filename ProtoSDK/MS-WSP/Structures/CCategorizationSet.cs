// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CCategorizationSet structure contains information on how the grouping is done at each level in a hierarchical result set.
    /// </summary>
    public struct CCategorizationSet : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer containing the number of elements in the categories array.
        /// </summary>
        public uint count;

        /// <summary>
        /// Array of CCategorizationSpec structures specifying the grouping for each level in a hierarchical query. The first structure specifies the top level.
        /// </summary>
        public CCategorizationSpec[] categories;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(count);

            if (categories != null)
            {
                foreach (var category in categories)
                {
                    category.ToBytes(buffer);
                }
            }
        }
    }
}
