// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CPMSetBindingsIn message requests the binding of columns to a rowset.
    /// </summary>
    public class CPMSetBindingsIn : IWspInMessage
    {
        #region Fields
        /// <summary>
        /// A 32-bit value representing the handle from the CPMCreateQueryOut message that identifies the query for which to set bindings.
        /// </summary>
        public uint _hCursor;

        /// <summary>
        /// A 32-bit unsigned integer indicating the size, in bytes, of a row.
        /// </summary>
        public uint _cbRow;

        /// <summary>
        /// A 32-bit unsigned integer indicating the length, in bytes, of the fields following the _dummy field.
        /// </summary>
        public uint _cbBindingDesc;

        /// <summary>
        /// This field is unused and MUST be ignored. It can be set to any arbitrary value.
        /// </summary>
        public uint _dummy;

        /// <summary>
        /// A 32-bit unsigned integer indicating the number of elements in the aColumns array.
        /// </summary>
        public uint cColumns;

        /// <summary>
        /// An array of CTableColumn structures describing the columns of a row in the rowset.
        /// </summary>
        public CTableColumn[] aColumns;
        #endregion

        public WspMessageHeader Header { get; set; }

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            var bodyBytes = GetBodyBytes();

            var checksum = Helper.CalculateChecksum(WspMessageHeader_msg_Values.CPMSetBindingsIn, bodyBytes);

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

            buffer.Add(_cbRow);

            var tempBuffer = new WspBuffer();

            tempBuffer.Add(cColumns);

            foreach (var column in aColumns)
            {
                tempBuffer.AlignWrite(4);

                column.ToBytes(tempBuffer);
            }

            var bytesAfterDummy = tempBuffer.GetBytes();

            _cbBindingDesc = (uint)bytesAfterDummy.Length;

            buffer.Add(_cbBindingDesc);

            buffer.Add(_dummy);

            buffer.AddRange(bytesAfterDummy);

            return buffer.GetBytes();
        }
    }
}
