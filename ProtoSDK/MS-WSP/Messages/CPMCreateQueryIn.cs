// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    /// <summary>
    /// The CPMCreateQueryIn message creates a new query.
    /// </summary>
    public class CPMCreateQueryIn : IWspInMessage
    {
        /// <summary>
        /// A 32-bit unsigned integer indicating the number of bytes from the beginning of this field to the end of the message.
        /// </summary>
        public uint Size;

        /// <summary>
        /// A CColumnSet structure containing the property offsets for properties in CPidMapper that are returned as a column. If no properties are in the column set, then no information will be returned from the query.
        /// </summary>
        public CColumnSet? ColumnSet;

        /// <summary>
        /// A CRestrictionArray structure containing the command tree of the query.
        /// </summary>
        public CRestrictionArray? RestrictionArray;

        /// <summary>
        /// A CInGroupSortAggregSets structure indicating the sort order of the query.
        /// </summary>
        public CInGroupSortAggregSets? SortSet;

        /// <summary>
        /// A CCategorizationSet structure that contains the groups for the query.
        /// </summary>
        public CCategorizationSet? CCategorizationSet;

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
        public uint Lcid;

        public WspMessageHeader Header { get; set; }

        public void FromBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void ToBytes(WspBuffer buffer)
        {
            var bodyBytes = GetBodyBytes();

            var checksum = Helper.CalculateChecksum(WspMessageHeader_msg_Values.CPMCreateQueryIn, bodyBytes);

            var header = Header;

            header._ulChecksum = checksum;

            Header = header;

            Header.ToBytes(buffer);

            buffer.AddRange(bodyBytes);
        }

        public byte[] GetBodyBytes()
        {
            var buffer = new WspBuffer();

            var tempBuffer = new WspBuffer();

            tempBuffer.Add(Size);

            int tempOffset = tempBuffer.WriteOffset;

            byte CColumnSetPresent;
            if (ColumnSet.HasValue)
            {
                CColumnSetPresent = 0x01;

                tempBuffer.Add(CColumnSetPresent);

                tempBuffer.AlignWrite(4);

                ColumnSet.Value.ToBytes(tempBuffer);
            }
            else
            {
                CColumnSetPresent = 0x00;

                tempBuffer.Add(CColumnSetPresent);
            }

            byte CRestrictionPresent;

            if (RestrictionArray.HasValue)
            {
                CRestrictionPresent = 0x01;

                tempBuffer.Add(CRestrictionPresent);

                RestrictionArray.Value.ToBytes(tempBuffer);
            }
            else
            {
                CRestrictionPresent = 0x00;

                tempBuffer.Add(CRestrictionPresent);
            }

            byte CSortSetPresent;

            if (SortSet.HasValue)
            {
                CSortSetPresent = 0x01;

                tempBuffer.Add(CSortSetPresent);

                tempBuffer.AlignWrite(4);

                SortSet.Value.ToBytes(tempBuffer);
            }
            else
            {
                CSortSetPresent = 0x00;

                tempBuffer.Add(CSortSetPresent);
            }

            byte CCategorizationSetPresent;

            if (CCategorizationSet.HasValue)
            {
                CCategorizationSetPresent = 0x01;

                tempBuffer.Add(CCategorizationSetPresent);

                tempBuffer.AlignWrite(4);

                CCategorizationSet.Value.ToBytes(tempBuffer);
            }
            else
            {
                CCategorizationSetPresent = 0x00;

                tempBuffer.Add(CCategorizationSetPresent);
            }

            tempBuffer.AlignWrite(4);

            RowSetProperties.ToBytes(tempBuffer);

            PidMapper.ToBytes(tempBuffer);

            GroupArray.ToBytes(tempBuffer);

            tempBuffer.Add(Lcid);

            Size = (uint)tempBuffer.WriteOffset;

            buffer.Add(Size);

            var bytesAfterSize = tempBuffer.GetBytes().Skip(tempOffset).ToArray();

            buffer.AddRange(bytesAfterSize);

            return buffer.GetBytes();
        }
    }
}
