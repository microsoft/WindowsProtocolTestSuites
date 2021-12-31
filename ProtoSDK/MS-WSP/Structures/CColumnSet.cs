// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CColumnSet structure specifies the column numbers to be returned. This structure is always used in reference to a specific CPidMapper structure.
    /// </summary>
    public struct CColumnSet : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer specifying the number of elements in the indexes array.
        /// </summary>
        public uint count;

        /// <summary>
        /// An array of 4-byte unsigned integers each representing a zero-based index into the aPropSpec array in the corresponding CPidMapper structure.
        /// The corresponding property values are returned as columns in the result set.
        /// </summary>
        public uint[] indexes;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(count);

            if (count != 0)
            {
                foreach (var index in indexes)
                {
                    buffer.Add(index);
                }
            }
        }
    }
}
