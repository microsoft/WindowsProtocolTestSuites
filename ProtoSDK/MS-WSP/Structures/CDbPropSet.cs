// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CDbPropSet structure contains a set of properties.
    /// </summary>
    public struct CDbPropSet : IWspStructure
    {
        /// <summary>
        /// A GUID identifying the property set.
        /// </summary>
        public Guid guidPropertySet;

        /// <summary>
        /// A 32-bit unsigned integer containing the number of elements in the aProps array.
        /// </summary>
        public uint cProperties;

        /// <summary>
        /// An array of CDbProp structures containing properties.
        /// Structures in the array MUST be separated by 0 to 3 padding bytes such that each structure begins at an offset that is a multiple of 4 bytes from the beginning of the message that contains this array.
        /// If padding bytes are present, the value they contain is arbitrary.
        /// The content of the padding bytes MUST be ignored by the receiver.
        /// </summary>
        public CDbProp[] aProps;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(guidPropertySet);

            buffer.Add(cProperties, 4);

            foreach (var prop in aProps)
            {
                buffer.AlignWrite(4);

                prop.ToBytes(buffer);
            }
        }
    }
}
