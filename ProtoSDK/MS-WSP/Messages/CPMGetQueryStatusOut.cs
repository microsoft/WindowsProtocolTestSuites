// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
{
    [Flags]
    public enum QStatus_Values : uint
    {
        /// <summary>
        /// The asynchronous query is still running.
        /// </summary>
        STAT_BUSY = 0x00000000,

        /// <summary>
        /// The query is in an error state.
        /// </summary>
        STAT_ERROR = 0x00000001,

        /// <summary>
        /// The query is complete and rows can be requested.
        /// </summary>
        STAT_DONE = 0x00000002,

        /// <summary>
        /// The query is complete, but updates are resulting in additional query computation.
        /// </summary>
        STAT_REFRESH = 0x00000003,

        /// <summary>
        /// Noise words were replaced by wildcard characters in the content query.
        /// </summary>
        STAT_NOISE_WORDS = 0x00000010,

        /// <summary>
        /// The results of the query might be incorrect because the query involved modified but unindexed files.
        /// </summary>
        STAT_CONTENT_OUT_OF_DATE = 0x00000020,

        /// <summary>
        /// The content query was too complex to complete or required enumeration instead of use of the content index.
        /// </summary>
        STAT_CONTENT_QUERY_INCOMPLETE = 0x00000080,

        /// <summary>
        /// The results of the query might be incorrect because the query execution reached the maximum allowable time.
        /// </summary>
        STAT_TIME_LIMIT_EXCEEDED = 0x00000100
    }

    /// <summary>
    /// The CPMGetQueryStatusOut message replies to a CPMGetQueryStatusIn message with the status of the query.
    /// </summary>
    public class CPMGetQueryStatusOut : IWspOutMessage
    {
        #region Fields
        /// <summary>
        /// A 32-bit unsigned integer. A bitmask of values defined in the following tables that describe the query.
        /// </summary>
        public QStatus_Values _QStatus;
        #endregion

        public IWspInMessage Request { get; set; }

        public WspMessageHeader Header { get; set; }

        public void FromBytes(WspBuffer buffer)
        {
            Header = buffer.ToStruct<WspMessageHeader>();

            _QStatus = buffer.ToStruct<QStatus_Values>();
        }

        public void ToBytes(WspBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
