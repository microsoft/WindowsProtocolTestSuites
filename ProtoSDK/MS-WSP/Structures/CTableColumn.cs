// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CTableColumn structure contains a column of a CPMSetBindingsIn message.
    /// </summary>
    public struct CTableColumn : IWspStructure
    {
        /// <summary>
        /// A CFullPropSpec structure.
        /// </summary>
        public CFullPropSpec PropSpec;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the type of data value contained in the column.
        /// </summary>
        public CBaseStorageVariant_vType_Values vType;

        /// <summary>
        /// This field MUST be set to one of the aggregation type values specified under the type field in section 2.2.1.25.
        /// </summary>
        public CAggregSpec_type_Values? AggregateType;

        /// <summary>
        /// An unsigned 2-byte integer specifying the offset of the column value in the row.
        /// </summary>
        public ushort? ValueOffset;

        /// <summary>
        /// An unsigned 2-byte integer specifying the size of the column value in bytes.
        /// </summary>
        public ushort? ValueSize;

        /// <summary>
        /// An unsigned 2-byte integer specifying the offset of the column status in the row.
        /// </summary>
        public ushort? StatusOffset;

        /// <summary>
        /// An unsigned 2-byte integer specifying the offset of the column length in the row.
        /// In CPMGetRowsOut, length is represented by a 32-bit unsigned integer by the offset specified in LengthOffset.
        /// </summary>
        public ushort? LengthOffset;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            PropSpec.ToBytes(buffer);

            buffer.Add((uint)vType, 4);

            byte AggregateUsed;

            if (AggregateType.HasValue)
            {
                AggregateUsed = 0x01;

                buffer.Add(AggregateUsed);

                buffer.Add(AggregateType.Value);
            }
            else
            {
                AggregateUsed = 0x00;

                buffer.Add(AggregateUsed);
            }

            byte ValueUsed;

            if (ValueOffset.HasValue && ValueSize.HasValue)
            {
                ValueUsed = 0x01;

                buffer.Add(ValueUsed);

                buffer.AlignWrite(2);

                buffer.Add(ValueOffset.Value);

                buffer.Add(ValueSize.Value);
            }
            else if (!ValueOffset.HasValue && !ValueSize.HasValue)
            {
                ValueUsed = 0x00;

                buffer.Add(ValueUsed);
            }
            else
            {
                throw new InvalidOperationException("ValueOffset and ValueSize should be present or absent at the same time!");
            }

            byte StatusUsed;

            if (StatusOffset.HasValue)
            {
                StatusUsed = 0x01;

                buffer.Add(StatusUsed);

                buffer.AlignWrite(2);

                buffer.Add(StatusOffset.Value);
            }
            else
            {
                StatusUsed = 0x00;

                buffer.Add(StatusUsed);
            }

            byte LengthUsed;

            if (LengthOffset.HasValue)
            {
                LengthUsed = 0x01;

                buffer.Add(LengthUsed);

                buffer.AlignWrite(2);

                buffer.Add(LengthOffset.Value);
            }
            else
            {
                LengthUsed = 0x00;

                buffer.Add(LengthUsed);
            }
        }
    }
}
