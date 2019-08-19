// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// The CPMCreateRowsOut message contains a response to a CPMCreateRowsIn message.
    /// </summary>
    public struct CPMCreateRowsOut : IWspOutMessage
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

        public IWspInMessage Request { get; set; }

        public WspMessageHeader Header { get; set; }


        public void FromBytes(WspBuffer buffer)
        {
            var header = new WspMessageHeader();

            header.FromBytes(buffer);

            Header = header;

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



            int categorizationCount = 0;

            var createQueryIn = (CPMCreateQueryIn)Request;

            if (createQueryIn.CCategorizationSet.HasValue)
            {
                categorizationCount = (int)createQueryIn.CCategorizationSet.Value.count;
            }

            aCursors = new UInt32[categorizationCount + 1];

            for (int i = 0; i < aCursors.Length; i++)
            {
                UInt32 cursor = buffer.ToStruct<UInt32>();

                aCursors[i] = cursor;
            }
        }

        public void ToBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
