// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// The CPMCreateQueryIn message creates a new query.
    /// </summary>
    public struct CPMCreateQueryIn : IWSPObject
    {
        /// <summary>
        /// A 32-bit unsigned integer indicating the number of bytes from the beginning of this field to the end of the message.
        /// </summary>
        public UInt32 Size;

        /// <summary>
        /// A byte field indicating if the ColumnSet field is present. If the value is set to 0x00, then no information will be returned from the query.
        /// </summary>
        public byte CColumnSetPresent;

        /// <summary>
        /// A CColumnSet structure containing the property offsets for properties in CPidMapper that are returned as a column. If no properties are in the column set, then no information will be returned from the query.
        /// </summary>
        public CColumnSet ColumnSet;

        /// <summary>
        /// A byte field indicating whether the RestrictionArray field is present.
        /// If set to any nonzero value, the RestrictionArray field MUST be present. If set to 0x00, RestrictionArray MUST be absent.
        /// </summary>
        public byte CRestrictionPresent;

        /// <summary>
        /// A CRestrictionArray structure containing the command tree of the query.
        /// </summary>
        public CRestrictionArray RestrictionArray;

        /// <summary>
        /// A byte field indicating whether the SortSet field is present.
        /// Note If set to any nonzero value, the SortSet field MUST be present.If set to 0x00, SortSet MUST be absent.
        /// </summary>
        public byte CSortSetPresent;

        /// <summary>
        /// A CInGroupSortAggregSets structure indicating the sort order of the query.
        /// </summary>
        public CInGroupSortAggregSets SortSet;

        /// <summary>
        /// A byte field indicating whether the CCategorizationSet field is present.
        /// Note If set to any nonzero value, the CCategorizationSet field MUST be present.If set to 0x00, CCategorizationSet MUST be absent.
        /// </summary>
        public byte CCategorizationSetPresent;

        /// <summary>
        /// A CCategorizationSet structure that contains the groups for the query.
        /// </summary>
        public CCategorizationSet CCategorizationSet;

        /// <summary>
        /// A CRowsetProperties structure providing configuration information for the query.
        /// </summary>
        public CRowsetProperties RowSetProperties;

        /// <summary>
        /// A CPidMapper structure that maps from property offsets to full property descriptions.
        /// </summary>
        public CPidMapper PidMapper;

        /// <summary>
        /// A CColumnGroupArray structure, describing property weights for probabilistic ranking.
        /// </summary>
        public CColumnGroupArray GroupArray;

        /// <summary>
        /// A 32-bit unsigned integer, indicating the user's locale for this query, as specified in [MS-LCID].
        /// </summary>
        public UInt32 Lcid;

        public void ToBytes(WSPBuffer buffer)
        {
            var tempBuffer = new WSPBuffer();

            tempBuffer.Add(Size);

            int tempOffset = tempBuffer.Offset;

            tempBuffer.Add(CColumnSetPresent);

            if (CColumnSetPresent == 0x01)
            {
                tempBuffer.Align(4);

                ColumnSet.ToBytes(tempBuffer);
            }

            tempBuffer.Add(CRestrictionPresent);

            if (CRestrictionPresent != 0x00)
            {
                RestrictionArray.ToBytes(tempBuffer);
            }

            tempBuffer.Add(CSortSetPresent);

            if (CSortSetPresent != 0x00)
            {
                tempBuffer.Align(4);

                SortSet.ToBytes(tempBuffer);
            }

            tempBuffer.Add(CCategorizationSetPresent);

            if (CCategorizationSetPresent != 0x00)
            {
                tempBuffer.Align(4);

                CCategorizationSet.ToBytes(tempBuffer);
            }

            tempBuffer.Align(4);

            RowSetProperties.ToBytes(tempBuffer);

            PidMapper.ToBytes(tempBuffer);

            GroupArray.ToBytes(tempBuffer);

            tempBuffer.Add(Lcid);

            Size = (UInt32)tempBuffer.Offset;

            buffer.Add(Size);

            var bytesAfterSize = tempBuffer.GetBytes().Skip(tempOffset).ToArray();

            buffer.AddRange(bytesAfterSize);
        }
    }
}
