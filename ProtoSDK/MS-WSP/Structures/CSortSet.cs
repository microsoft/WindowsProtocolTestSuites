// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CSortSet structure contains the sort order of the query.
    /// </summary>
    public struct CSortSet : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer specifying the number of elements in sortArray.
        /// </summary>
        public uint count;

        /// <summary>
        /// An array of CSort structures describing the order in which to sort the results of the query.
        /// </summary>
        public CSort[] sortArray;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(count);

            foreach (var sort in sortArray)
            {
                buffer.AlignWrite(4);

                sort.ToBytes(buffer);
            }
        }
    }
}
