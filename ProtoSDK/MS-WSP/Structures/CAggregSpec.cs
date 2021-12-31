// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    public enum CAggregSpec_type_Values : byte
    {
        /// <summary>
        /// No aggregation is used.
        /// </summary>
        DBAGGTTYPE_BYNONE = 0x00,

        /// <summary>
        /// Sum of the idColumn property value from each result in the group. Valid only for numeric properties.
        /// </summary>
        DBAGGTTYPE_SUM = 0x01,

        /// <summary>
        /// Maximum value of the idColumn property value from each result in the group. Valid only for numeric or filetime properties.
        /// </summary>
        DBAGGTTYPE_MAX = 0x02,

        /// <summary>
        /// Minimum value of the idColumn property value from each result in the group. Valid only for numeric or filetime properties.
        /// </summary>
        DBAGGTTYPE_MIN = 0x03,

        /// <summary>
        /// Average value of the idColumn property value from each result in the group. Valid only for numeric properties.
        /// </summary>
        DBAGGTTYPE_AVG = 0x04,

        /// <summary>
        /// Count of the number of leaf results in the group.
        /// </summary>
        DBAGGTTYPE_COUNT = 0x05,

        /// <summary>
        /// Count of immediate children of the group.
        /// </summary>
        DBAGGTTYPE_CHILDCOUNT = 0x06,

        /// <summary>
        /// Most frequent N idColumn values from the results in the group. Additionally includes a count for how many times each value occurred and a document identifier for a result that has each returned value.
        /// </summary>
        DBAGGTTYPE_BYFREQ = 0x07,

        /// <summary>
        /// First N idColumn values from leaf results found in a group.
        /// </summary>
        DBAGGTTYPE_FIRST = 0x08,

        /// <summary>
        /// Lower and upper bounds of the idColumn values found in the group results group. Only valid for filetime properties.
        /// </summary>
        DBAGGTTYPE_DATERANGE = 0x09,

        /// <summary>
        /// N idRepresentative values, each selected from one of the result subsets that have a unique idColumn value. Each value is also returned with a document identifier that has the idRepresentative value. 
        /// </summary>
        DBAGGTTYPE_REPRESENTATIVEOF = 0x0a,

        /// <summary>
        /// Edit distance between the results in a completion group and the primary query string in the Completions grouping clause, as specified in CCompletionCategSpec.
        /// </summary>
        DBAGGTTYPE_EDITDISTANCE = 0x0b,
    }

    /// <summary>
    /// The CAggregSpec structure contains information about an individual aggregate.
    /// </summary>
    public struct CAggregSpec : IWspStructure
    {
        /// <summary>
        /// An 8-bit unsigned integer specifying the type of aggregation used.
        /// </summary>
        public CAggregSpec_type_Values type;

        /// <summary>
        /// A 32-bit unsigned integer specifying the number of characters in the Alias field.
        /// </summary>
        public uint ccAlias;

        /// <summary>
        /// A non-null-terminated Unicode string that represents the alias name for the aggregate. The ccAlias field contains the length of the string.
        /// </summary>
        public string Alias;

        /// <summary>
        /// Property ID for the column to be aggregated over.
        /// </summary>
        public uint idColumn;

        /// <summary>
        /// An optional 32-bit unsigned integer that is the number of elements to return for First, ByFreq, and RepresentativeOf aggregates.
        /// </summary>
        public uint ulMaxNumToReturn;

        /// <summary>
        /// An optional 32-bit unsigned integer that is the representative property ID requested for the RepresentativeOf aggregate.
        /// </summary>
        public uint idRepresentative;

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            buffer.Add(type);
            buffer.AlignWrite(4);

            buffer.Add(ccAlias);

            if (ccAlias != 0)
            {
                buffer.AddUnicodeString(Alias, false);
            }

            buffer.Add(idColumn);

            switch (type)
            {
                case CAggregSpec_type_Values.DBAGGTTYPE_FIRST:
                case CAggregSpec_type_Values.DBAGGTTYPE_BYFREQ:
                case CAggregSpec_type_Values.DBAGGTTYPE_REPRESENTATIVEOF:
                    buffer.Add(ulMaxNumToReturn);
                    break;
            }

            switch (type)
            {
                case CAggregSpec_type_Values.DBAGGTTYPE_REPRESENTATIVEOF:
                    buffer.Add(idRepresentative);
                    break;
            }
        }
    }
}
