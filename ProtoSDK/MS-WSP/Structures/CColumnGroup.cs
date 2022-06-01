// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CColumnGroup structure contains information about a property's weight in a single group.
    /// </summary>
    public struct CColumnGroup : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer containing the number of elements in the Props array.
        /// </summary>
        public uint count;

        /// <summary>
        /// A 32-bit unsigned integer specifying group ID, a full property specification that can be used in the corresponding CProbRestriction.
        /// The value of _groupPid MUST satisfy the following result: (0xFFFF0000 & _groupPid) == 0x7FFF0000.
        /// </summary>
        public uint _groupPid;

        /// <summary>
        /// An array of SProperty structures, each specifying a PID and a weight for a property.
        /// </summary>
        public SProperty[] Props;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(count);

            buffer.Add(_groupPid);

            foreach (var prop in Props)
            {
                prop.ToBytes(buffer);
            }
        }
    }
}
