// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CPMGetQueryStatusExOut message replies to a CPMGetQueryStatusExIn message with both the status of the query and other status information.
    /// </summary>
    public class CPMGetQueryStatusExOut : IWspOutMessage
    {
        #region Fields
        /// <summary>
        /// A 32-bit unsigned integer. A bitmask of values defined in the following tables that describe the query.
        /// </summary>
        public QStatus_Values _QStatus;

        /// <summary>
        /// A 32-bit unsigned integer indicating the number of documents that have been indexed.
        /// </summary>
        public uint _cFilteredDocuments;

        /// <summary>
        /// A 32-bit unsigned integer indicating the number of documents that remain to be indexed.
        /// </summary>
        public uint _cDocumentsToFilter;

        /// <summary>
        /// A 32-bit unsigned integer indicating the denominator of the ratio of documents that the query has finished processing.
        /// </summary>
        public uint _dwRatioFinishedDenominator;

        /// <summary>
        /// A 32-bit unsigned integer indicating the numerator of the ratio of documents that the query has finished processing.
        /// </summary>
        public uint _dwRatioFinishedNumerator;

        /// <summary>
        /// A 32-bit unsigned integer indicating the approximate position of the bookmark in the rowset in terms of rows.
        /// </summary>
        public uint _iRowBmk;

        /// <summary>
        /// A 32-bit unsigned integer specifying the total number of rows in the rowset.
        /// </summary>
        public uint _cRowsTotal;

        /// <summary>
        /// A 32-bit unsigned integer specifying the maximum rank found in the rowset.
        /// </summary>
        public uint _maxRank;

        /// <summary>
        /// A 32-bit unsigned integer specifying the number of unique results returned in the rowset. 
        /// </summary>
        public uint _cResultsFound;

        /// <summary>
        /// A 32-bit unsigned integer that defines a unique WHEREID for referring to the CRestrictionArray used to construct the rowset. 
        /// </summary>
        public uint _whereID;
        #endregion

        public IWspInMessage Request { get; set; }

        public WspMessageHeader Header { get; set; }

        public void FromBytes(WspBuffer buffer)
        {
            Header = buffer.ToStruct<WspMessageHeader>();

            _QStatus = buffer.ToStruct<QStatus_Values>();

            _cFilteredDocuments = buffer.ToStruct<uint>();

            _cDocumentsToFilter = buffer.ToStruct<uint>();

            _dwRatioFinishedDenominator = buffer.ToStruct<uint>();

            _dwRatioFinishedNumerator = buffer.ToStruct<uint>();

            _iRowBmk = buffer.ToStruct<uint>();

            _cRowsTotal = buffer.ToStruct<uint>();

            _maxRank = buffer.ToStruct<uint>();

            _cResultsFound = buffer.ToStruct<uint>();

            _whereID = buffer.ToStruct<uint>();
        }

        public void ToBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
