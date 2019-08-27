// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// The CTableVariant structure contains the fixed-size portion of a variable length data type stored in the CPMGetRowsOut message.
    /// </summary>
    public struct CTableVariant : IWspStructure
    {
        /// <summary>
        /// A type indicator, indicating the type of vValue. It MUST be one of the values under the vType field, as specified in section 2.2.1.1.
        /// </summary>
        public vType_Values vType;

        /// <summary>
        /// Not used. Can be set to any arbitrary value when sent and it MUST be ignored on receipt.
        /// </summary>
        public ushort reserved1;

        /// <summary>
        /// Not used. Can be set to any arbitrary value when sent and it MUST be ignored on receipt.
        /// </summary>
        public uint reserved2;
        /// <summary>
        /// Element count. This field is 4 bytes and is present only if the vType is a VT_VECTOR.
        /// </summary>
        public UInt32? Count;

        /// <summary>
        /// An offset to variable length data (for example, a string). 
        /// This MUST be a 32-bit value (4 bytes long) if 32-bit offsets are being used (per the rules in section 2.2.3.12), or a 64-byte value (8 bytes long) if 64-bit offsets are being used.
        /// </summary>
        public object Offset;

        public bool Is64bit;

        public void FromBytes(WspBuffer buffer)
        {
            vType = buffer.ToStruct<vType_Values>();
            reserved1 = buffer.ToStruct<ushort>();
            reserved2 = buffer.ToStruct<uint>();
            if (vType == vType_Values.VT_VECTOR)
            {
                throw new NotImplementedException();
            }
            else
            {
                if (Is64bit)
                    Offset = buffer.ToStruct<Int64>();
                else
                    Offset = buffer.ToStruct<Int32>();
            }
        }

        public void ToBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
