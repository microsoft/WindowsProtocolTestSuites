// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CPidMapper structure contains an array of property specifications and serves to map from a property offset to a full property specification.
    /// The more compact property offsets are used to name properties in other parts of the protocol.
    /// Since offsets are more compact, they allow shorter property references in other parts of the protocol.
    /// </summary>
    public struct CPidMapper : IWspStructure
    {
        /// <summary>
        /// A 32-bit unsigned integer containing the number of elements in the aPropSpec array.
        /// </summary>
        public uint count;

        /// <summary>
        /// Array of CFullPropSpec structures indicating the properties to return.
        /// </summary>
        public CFullPropSpec[] aPropSpec;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(count);
            if (aPropSpec != null)
            {
                foreach (var property in aPropSpec)
                {
                    property.ToBytes(buffer);
                }
            }
        }
    }
}
