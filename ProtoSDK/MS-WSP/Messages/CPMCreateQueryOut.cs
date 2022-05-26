// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CPMCreateQueryOut message contains a response to a CPMCreateQueryIn message.
    /// </summary>
    public class CPMCreateQueryOut : IWspOutMessage
    {
        /// <summary>
        /// A 32-bit unsigned integer. MUST be set to one of the following values.
        /// Note An informative value indicating whether the query can be expected to provide results faster.
        /// 0x00000000	For the query provided in CPMCreateQueryIn, there would be a greater latency in delivering query results.
        /// 0x00000001	If _fTrueSequential is set to true, results can be returned sequentially without the server incurring the cost of processing the entire result set before returning the first result.
        /// </summary>
        public uint _fTrueSequential;

        /// <summary>
        /// A Boolean value indicating whether the document identifiers pointed by the cursors are unique throughout query results. MUST be set to one of the following values.
        /// 0x00000000	The cursors are unique only throughout the rowset.
        /// 0x00000001	The cursors are unique across multiple query results.
        /// </summary>
        public uint _fWorkIdUnique;

        /// <summary>
        /// An array of 32-bit unsigned integers representing the handles to cursors with the number of elements equal to the number of categories in the CategorizationSet field of CPMCreateQueryIn message plus one element, representing an uncategorized cursor.
        /// </summary>
        public uint[] aCursors;

        public IWspInMessage Request { get; set; }

        public WspMessageHeader Header { get; set; }

        public void FromBytes(WspBuffer buffer)
        {
            var header = new WspMessageHeader();

            header.FromBytes(buffer);

            Header = header;

            _fTrueSequential = buffer.ToStruct<uint>();

            _fWorkIdUnique = buffer.ToStruct<uint>();

            int categorizationCount = 0;

            var createQueryIn = (CPMCreateQueryIn)Request;

            if (createQueryIn.CCategorizationSet.HasValue)
            {
                categorizationCount = (int)createQueryIn.CCategorizationSet.Value.count;
            }

            aCursors = new uint[categorizationCount + 1];

            for (int i = 0; i < aCursors.Length; i++)
            {
                uint cursor = buffer.ToStruct<uint>();

                aCursors[i] = cursor;
            }
        }

        public void ToBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
