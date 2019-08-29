// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// The column returned in CPMGetRowsOut message.
    /// </summary>
    public struct Column
    {
        public int? Status;
        public int? Length;
        public CTableVariant rowVariant;
        public object Data;
    }

    /// <summary>
    /// The row structure returned in CPMGetRowsOut message.
    /// </summary>
    public struct Row
    {
        public Column[] Columns;
    }

    /// <summary>
    /// The CPMCreateRowsOut message contains a response to a CPMCreateRowsIn message.
    /// </summary>
    public struct CPMGetRowsOut : IWspOutMessage
    {
        /// <summary>
        /// A 32-bit unsigned integer indicating the number of rows returned in Rows.
        /// </summary>
        public UInt32 _cRowsReturned;

        /// <summary>
        /// A 32-bit unsigned integer. 
        /// </summary>
        public RowSeekType eType;

        /// <summary>
        /// A 32-bit value representing the handle of the rowset chapter.
        /// </summary>
        public UInt32 _chapt;

        /// <summary>
        /// This field MUST contain a structure of the type indicated by the eType field.
        /// </summary>
        public object SeekDescription;

        /// <summary>
        /// Row data is formatted as prescribed by column information in the most recent CPMSetBindingsIn message. 
        /// The fixed-sized area (at the beginning of the row buffer) MUST contain a CTableVariant for each column, 
        /// stored at the offset specified in the most recent CPMSetBindingsIn message. 
        /// </summary>
        public CTableVariant[] tableVariants;

        /// <summary>
        /// The actual data of the rows, stored starting at the end of the buffer.
        /// </summary>
        public string[] RowsData;

        /// <summary>
        /// This field MUST be of sufficient length (0 to _cbReserved-1 bytes) to pad the Rows field to _cbReserved offset from the beginning of a message, 
        /// where _cbReserved is the value in the CPMGetRowsIn message. Padding bytes used in this field can be any arbitrary value. 
        /// This field MUST be ignored by the receiver.
        /// </summary>
        public byte[] paddingRows;

        /// <summary>
        /// The rows array 
        /// </summary>
        public Row[] Rows;

        public IWspInMessage Request { get; set; }

        public WspMessageHeader Header { get; set; }

        public CPMSetBindingsIn BindingRequest { get; set; }

        public bool Is64Bit;

        public void FromBytes(WspBuffer buffer)
        {
            var header = new WspMessageHeader();

            var initialReadOffset = buffer.ReadOffset;
            header.FromBytes(buffer);
            Header = header;

            // Do not parse message body if the status in header is not success.
            if (header._status != (uint)WspErrorCode.SUCCESS && header._status != (uint)WspErrorCode.DB_S_ENDOFROWSET)
            {
                return;
            }

            _cRowsReturned = buffer.ToStruct<UInt32>();

            eType = buffer.ToStruct<RowSeekType>();

            _chapt = buffer.ToStruct<UInt32>();

            switch (eType)
            {
                case RowSeekType.eRowSeekNone:
                    SeekDescription = null;
                    break;
                case RowSeekType.eRowSeekNext:
                    SeekDescription = buffer.ToStruct<CRowSeekNext>();
                    break;
                case RowSeekType.eRowSeekAt:
                    SeekDescription = buffer.ToStruct<CRowSeekAt>();
                    break;
                case RowSeekType.eRowSeekAtRatio:
                    SeekDescription = buffer.ToStruct<CRowSeekAtRatio>();
                    break;
                case RowSeekType.eRowSeekByBookmark:
                    SeekDescription = buffer.ToStruct<CRowSeekByBookmark>();
                    break;
            }

            int rowStartIndex = initialReadOffset + (int)((CPMGetRowsIn)Request)._cbReserved;
            int paddingSize = rowStartIndex - buffer.ReadOffset;
            paddingRows = buffer.ReadBytes(paddingSize);
            
            rowStartIndex = buffer.ReadOffset;
            Rows = new Row[_cRowsReturned];
            uint cColumns = BindingRequest.cColumns;
            for (int i = 0; i < _cRowsReturned; i++)
            {
                Rows[i].Columns = new Column[cColumns];

                for (int j = 0; j < cColumns; j++)
                {
                    if (BindingRequest.aColumns[j].StatusOffset != null)
                    {
                        Rows[i].Columns[j].Status = buffer.Peek<byte>(rowStartIndex + BindingRequest.aColumns[j].StatusOffset.Value);
                    }

                    if (BindingRequest.aColumns[j].LengthOffset != null)
                    {
                        Rows[i].Columns[j].Length = buffer.Peek<byte>(rowStartIndex + BindingRequest.aColumns[j].LengthOffset.Value);
                    }

                    if (BindingRequest.aColumns[j].ValueOffset != null)
                    {
                        int valueOffset = rowStartIndex + BindingRequest.aColumns[j].ValueOffset.Value;

                        if (BindingRequest.aColumns[j].vType == vType_Values.VT_VARIANT)
                        {
                            Rows[i].Columns[j].rowVariant = new CTableVariant();
                            Rows[i].Columns[j].rowVariant.Is64bit = this.Is64Bit;
                            WspBuffer rowVariantBuffer = new WspBuffer(buffer.ReadBytesFromOffset(valueOffset, BindingRequest.aColumns[j].ValueSize.Value));
                            Rows[i].Columns[j].rowVariant.FromBytes(rowVariantBuffer);
                            object dataOffset;
                            if (this.Is64Bit)
                            {
                                dataOffset = (Int64)Rows[i].Columns[j].rowVariant.Offset - (int)((CPMGetRowsIn)Request)._ulClientBase;
                            }
                            else
                            {
                                dataOffset = (Int32)Rows[i].Columns[j].rowVariant.Offset - (int)((CPMGetRowsIn)Request)._ulClientBase;
                            }

                            // TypeMarshal does not support read data by 64-bit offset, so cast dataOffset to int here.
                            Rows[i].Columns[j].Data = ReadValueByType(Rows[i].Columns[j].rowVariant.vType, (int)dataOffset, buffer); 
                        }
                        else
                        {
                            Rows[i].Columns[j].Data = ReadValueByType(BindingRequest.aColumns[j].vType, valueOffset, buffer);
                        }
                    }
                }

                rowStartIndex += (int)((CPMGetRowsIn)Request)._cbRowWidth;
            }
        }

        public void ToBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        private object ReadValueByType(vType_Values vType, int valueOffset, WspBuffer buffer)
        {
            if (vType.HasFlag(vType_Values.VT_VECTOR) || vType.HasFlag(vType_Values.VT_ARRAY))
            {
                throw new NotImplementedException();
            }

            object value = null;
            switch (vType)
            {
                case vType_Values.VT_EMPTY:
                case vType_Values.VT_NULL:
                    break;
                case vType_Values.VT_I1:
                case vType_Values.VT_UI1:
                    value = buffer.Peek<byte>(valueOffset);
                    break;
                case vType_Values.VT_I2:
                case vType_Values.VT_UI2:
                case vType_Values.VT_BOOL:
                    value = buffer.Peek<ushort>(valueOffset);
                    break;
                case vType_Values.VT_I4:
                case vType_Values.VT_INT:
                    value = buffer.Peek<int>(valueOffset);
                    break;
                case vType_Values.VT_R4:
                    value = buffer.Peek<float>(valueOffset);
                    break;
                case vType_Values.VT_UI4:
                case vType_Values.VT_UINT:
                case vType_Values.VT_ERROR:
                    value = buffer.Peek<uint>(valueOffset);
                    break;
                case vType_Values.VT_I8:
                    value = buffer.Peek<Int64>(valueOffset);
                    break;
                case vType_Values.VT_UI8:
                    value = buffer.Peek<UInt64>(valueOffset);
                    break;
                case vType_Values.VT_R8:
                    value = buffer.Peek<double>(valueOffset);
                    break;
                case vType_Values.VT_CY: // An 8-byte two's complement integer (vValue divided by 10,000).
                    throw new NotImplementedException();
                case vType_Values.VT_DATE:
                    value = DateTime.FromOADate(buffer.Peek<double>(valueOffset));
                    break;
                case vType_Values.VT_FILETIME:
                    value = DateTime.FromFileTime(buffer.Peek<Int64>(valueOffset));
                    break;
                case vType_Values.VT_DECIMAL:
                    value = buffer.Peek<DECIMAL>(valueOffset);
                    break;
                case vType_Values.VT_CLSID:
                    value = buffer.Peek<Guid>(valueOffset);
                    break;
                case vType_Values.VT_BLOB: // A 4-byte unsigned integer count of bytes in the blob, followed by that many bytes of data.
                case vType_Values.VT_BLOB_OBJECT:
                    int blobSize = buffer.Peek<int>(valueOffset);
                    value = buffer.ReadBytesFromOffset(valueOffset + 4, blobSize);
                    break;
                case vType_Values.VT_BSTR:
                    int strSize = buffer.Peek<int>(valueOffset);
                    var bytes = buffer.ReadBytesFromOffset(valueOffset + 4, strSize);

                    //TODO: just use UTF8 for a temporary solution
                    // For vType set to VT_BSTR, this field is a set of characters in an OEM–selected character set. 
                    // The client and server MUST be configured to have interoperable character sets. There is no requirement that it be null-terminated.
                    value = System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length); 
                    break;
                case vType_Values.VT_LPSTR:
                    {
                        throw new NotImplementedException();
                    }
                    break;
                case vType_Values.VT_LPWSTR:
                    {
                        int strLength = GetNullTerminatedUnicodeStringLength(buffer, valueOffset);
                        value = System.Text.Encoding.Unicode.GetString(buffer.ReadBytesFromOffset(valueOffset, strLength), 0, strLength);
                    }
                    break;
                case vType_Values.VT_COMPRESSED_LPWSTR:
                    value = buffer.Peek<VT_COMPRESSED_LPWSTR>(valueOffset);
                    break;
            }

            return value;
        }

        private int GetNullTerminatedUnicodeStringLength(WspBuffer buffer, int startOffset)
        {
            int length = 0;
            while (buffer.Peek<ushort>(startOffset) !=0 )
            {
                startOffset += 2;
                length += 2;
            }

            return length;
        }
    }
}
