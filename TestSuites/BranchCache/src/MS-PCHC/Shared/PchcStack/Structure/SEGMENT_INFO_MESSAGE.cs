// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc
{
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;

    /// <summary>
    /// A SEGMENT_INFO_MESSAGE is a request message sent by the client to the hosted 
    /// cache containing the segment hash of data (HoD) for the previously offered segment, 
    /// as well as the range of block hashes in the segment. Whether a SEGMENT_INFO_MESSAGE 
    /// is sent depends on the hosted cache's response to the previous INITIAL_OFFER_MESSAGE 
    /// containing the same HoHoDk.
    /// </summary>
    public struct SEGMENT_INFO_MESSAGE
    {
        /// <summary>
        /// MessageHeader (8 bytes): A MESSAGE_HEADER structure (section 2.2.1.1), 
        /// with the Type field set to 0x0002.
        /// </summary>
        public MESSAGE_HEADER MsgHeader;

        /// <summary>
        /// ConnectionInformation (8 bytes): A CONNECTION_INFORMATION structure (section 2.2.1.2).
        /// </summary>
        public CONNECTION_INFORMATION ConnectionInfo;

        /// <summary>
        /// PEERDIST_CONTENT_TAG (16 bytes): A structure consisting of 16 bytes of opaque data. 
        /// This field contains a tag supplied by a higher protocol layer on the client. 
        /// The tag is added to the information being sent by the client to the hosted cache. 
        /// The data is then passed to the appropriate layer on the hosted cache.
        /// </summary>
        public byte[] ContentTag;

        /// <summary>
        /// SEGMENT_INFORMATION (variable): A content information data structure ([MS-PCCRC] section 2.3)
        /// </summary>
        public Content_Information_Data_Structure SegmentInfo;
    }
}
