// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    public enum CPMGetRowsIn_eType_Values : uint
    {
        /// <summary>
        /// There is no SeekDescription; the SeekDescription field is omitted.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// SeekDescription contains a CRowSeekNext structure.
        /// </summary>
        eRowSeekNext = 0x00000001,

        /// <summary>
        /// SeekDescription contains a CRowSeekAt structure.
        /// </summary>
        eRowSeekAt = 0x00000002,

        /// <summary>
        /// SeekDescription contains a CRowSeekAtRatio structure.
        /// </summary>
        eRowSeekAtRatio = 0x00000003,

        /// <summary>
        /// SeekDescription contains a CRowSeekByBookmark structure.
        /// </summary>
        eRowSeekByBookmark = 0x00000004,
    }

    public struct InvalidSeekDescription : IWspSeekDescription
    {
        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            return;
        }
    }

    /// <summary>
    /// The CPMGetRowsIn message requests rows from a query.
    /// </summary>
    public class CPMGetRowsIn : IWspInMessage
    {
        /// <summary>
        /// A 32-bit value representing the handle from the CPMCreateQueryOut message identifying the query for which to retrieve rows.
        /// </summary>
        public uint _hCursor;

        /// <summary>
        /// A 32-bit unsigned integer indicating the maximum number of rows that the client will receive in response to this message.
        /// </summary>
        public uint _cRowsToTransfer;

        /// <summary>
        /// A 32-bit unsigned integer indicating the length of a row, in bytes.
        /// </summary>
        public uint _cbRowWidth;

        /// <summary>
        /// A 32-bit unsigned integer indicating the size of the message beginning with eType.
        /// </summary>
        public uint _cbSeek;

        /// <summary>
        /// A 32-bit unsigned integer indicating the size, in bytes, of a CPMGetRowsOut message (without the Rows and SeekDescriptions fields).
        /// This value in this field is added to the value of the _cbSeek field, and then is to be used to calculate the offset of Rows field in the CPMGetRowsOut message.
        /// </summary>
        public uint _cbReserved;

        /// <summary>
        /// A 32-bit unsigned integer.
        /// Note This field MUST be set to the maximum of the value of _cbRowWidth or 1000 times the value of _cRowsToTransfer, rounded up to the nearest 512 byte multiple.The value MUST NOT exceed 0x00004000.
        /// </summary>
        public uint _cbReadBuffer;

        /// <summary>
        /// A 32-bit unsigned integer indicating the base value to use for pointer calculations in the row buffer.
        /// If 64-bit offsets are being used, the reserved2 field of the message header is used as the upper 32-bits and _ulClientBase as the lower 32-bits of a 64-bit value. See section 2.2.3.12.
        /// </summary>
        public uint _ulClientBase;

        /// <summary>
        /// A 32-bit unsigned integer indicating the order in which to fetch the rows that MUST be set to one of the following values.
        /// 0x00000000	The rows are to be fetched in forward order.
        /// 0x00000001	The rows are to be fetched in reverse order.
        /// </summary>
        public uint _fBwdFetch;

        /// <summary>
        /// A 32-bit unsigned integer indicating the type of operation to perform.
        /// </summary>
        public CPMGetRowsIn_eType_Values eType;

        /// <summary>
        /// A 32-bit value representing the handle of the rowset chapter.
        /// </summary>
        public uint _chapt;

        /// <summary>
        /// This field MUST contain a structure of the type indicated by the eType value.
        /// </summary>
        public IWspSeekDescription SeekDescription;

        public WspMessageHeader Header { get; set; }

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            var bodyBytes = GetBodyBytes();

            var checksum = Helper.CalculateChecksum(WspMessageHeader_msg_Values.CPMGetRowsIn, bodyBytes);

            var header = Header;

            header._ulChecksum = checksum;

            Header = header;

            Header.ToBytes(buffer);

            buffer.AddRange(bodyBytes);
        }

        public byte[] GetBodyBytes()
        {
            var buffer = new WspBuffer();

            buffer.Add(_hCursor);

            buffer.Add(_cRowsToTransfer);

            buffer.Add(_cbRowWidth);

            var tempBuffer = new WspBuffer();

            tempBuffer.Add(eType);

            tempBuffer.Add(_chapt);

            switch (SeekDescription)
            {
                case CRowSeekNext seek:
                    seek.ToBytes(tempBuffer);
                    break;

                case CRowSeekAt seek:
                    seek.ToBytes(tempBuffer);
                    break;

                case CRowSeekAtRatio seek:
                    seek.ToBytes(tempBuffer);
                    break;

                case CRowSeekByBookmark seek:
                    seek.ToBytes(tempBuffer);
                    break;

                default:
                    break;
            }

            var bytesFromEType = tempBuffer.GetBytes();

            _cbSeek = (uint)bytesFromEType.Length;

            buffer.Add(_cbSeek);

            buffer.Add(_cbReserved);

            buffer.Add(_cbReadBuffer);

            buffer.Add(_ulClientBase);

            buffer.Add(_fBwdFetch);

            buffer.AddRange(bytesFromEType);

            return buffer.GetBytes();
        }
    }
}
