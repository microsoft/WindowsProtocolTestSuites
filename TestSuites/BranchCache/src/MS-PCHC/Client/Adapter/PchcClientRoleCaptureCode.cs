// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pchc
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pchc;
    using Microsoft.Protocols.TestTools.StackSdk.CommonStack;

    /// <summary>
    /// Verify the client role requirements of MS-PCHC.
    /// </summary>
    public partial class PchcClientAdapter
    {
        /// <summary>
        /// The protocol doc short name of MS-PCCRC.
        /// </summary>
        private const string PCCRCDOCSHORTNAME = "MS-PCCRC";

        /// <summary>
        /// The standard segment size.
        /// </summary>
        private const int STANDARDSEGMENTSIZE = 32 * 1024 * 1024;

        #region Capture PCCRC RS

        /// <summary>
        /// Validate the segmentInformation In SEGMTNE_INFO_MESSAGE.
        /// </summary>
        /// <param name="segmentInformation">The segmetnInformation.</param>
        private void ValidateSegmentInformationFromPccrc(Content_Information_Data_Structure segmentInformation)
        {
            int valueOfDwHashAlgo = 0;
            int hashLength = 0;
            switch (segmentInformation.dwHashAlgo)
            {
                case dwHashAlgo_Values.V1:
                    valueOfDwHashAlgo = 0x0000800C;
                    hashLength = 32;
                    break;
                case dwHashAlgo_Values.V2:
                    hashLength = 48;
                    valueOfDwHashAlgo = 0x0000800D;
                    break;
                case dwHashAlgo_Values.V3:
                    hashLength = 64;
                    valueOfDwHashAlgo = 0x0000800E;
                    break;
                default:
                    break;
            }
            
            #region MS-PCCRC_R54
            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R54, all fields are {0}in little-endian byte order",
                BitConverter.IsLittleEndian ? string.Empty : "not ");

            // Verify MS-PCCRC requirement: MS-PCCRC_R54
            Site.CaptureRequirementIfIsTrue(
                BitConverter.IsLittleEndian,
                PCCRCDOCSHORTNAME,
                54,
                @"[In Content Information Data Structure Version] All fields[Version,dwHashAlgo,
                dwOffsetInFirstSegment,dwReadBytesInLastSegment,cSegments,segments (variable),
                blocks (variable)]  are in little-endian byte order.");

            #endregion

            #region MS-PCCRC_R55
            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R55, the actual value of Version is 0x{0:X8}",
                segmentInformation.Version);

            // Verify MS-PCCRC requirement: MS-PCCRC_R55
            Site.CaptureRequirementIfAreEqual<int>(
                0x0100,
                segmentInformation.Version,
                PCCRCDOCSHORTNAME,
                55,
                @"[In Content Information Data Structure Version] Version (2 bytes):  Content Information
                version (0x0100 is version 1.0).");

            #endregion
           
            this.ValidatePccrcSegmentInfoVersion(segmentInformation, valueOfDwHashAlgo, hashLength);

            this.ValidatePccrcCSegment(segmentInformation, hashLength);

            #region MS-PCCRC_R86

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R86, the actual number of SegmentContentBlocks fields in blocks field is {0}",
                segmentInformation.blocks.Length);

            // Verify MS-PCCRC requirement: MS-PCCRC_R86
            Site.CaptureRequirementIfAreEqual<uint>(
                segmentInformation.cSegments,
                (uint)segmentInformation.blocks.Length,
                PCCRCDOCSHORTNAME,
                86,
                @"[In SegmentContentBlocks] The blocks field contains a number cSegments of 
                SegmentContentBlocks fields.");

            #endregion

            #region MS-PCCRC_R89

            bool isVerifyR89 = true;

            foreach (SegmentContentBlocks segContentBlocks in segmentInformation.blocks)
            {
                if (segContentBlocks.BlockHashes.Length != segContentBlocks.cBlocks * hashLength)
                {
                    isVerifyR89 = false;
                    break;
                }
            }

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R89, the actual size of BlockHashes field is {0} cBlokcs * {1}",
                isVerifyR89 ? string.Empty : "not ",
                hashLength);

            // Verify MS-PCCRC requirement: MS-PCCRC_R89
            Site.CaptureRequirementIfIsTrue(
                isVerifyR89,
                PCCRCDOCSHORTNAME,
                89,
                @"[In SegmentContentBlocks] BlockHashes (variable):The size of this field is 
                cBlocks * (32, 48 or 64, depending on which hash was used).");
            #endregion
        }

        /// <summary>
        /// Validate pccrc cSegments part.
        /// </summary>
        /// <param name="segmentInformation">Content information data structure.</param>
        /// <param name="hashLength">The hash length.</param>
        private void ValidatePccrcCSegment(Content_Information_Data_Structure segmentInformation, int hashLength)
        {
            #region MS-PCCRC_R67

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R67, the actual number of SegmentDescription fields in segments field is {0}",
                segmentInformation.segments.Length);

            // Verify MS-PCCRC requirement: MS-PCCRC_R67
            Site.CaptureRequirementIfAreEqual<uint>(
                segmentInformation.cSegments,
                (uint)segmentInformation.segments.Length,
                PCCRCDOCSHORTNAME,
                67,
                @"[In SegmentDescription] The segments field is composed of a number cSegments of 
                SegmentDescription fields.");

            #endregion

            #region MS-PCCRC_R69
            bool isVerifyR69 = true;            
            for (int i = 0; i < segmentInformation.cSegments - 1; i++)
            {
                if (segmentInformation.segments[i].cbSegment != STANDARDSEGMENTSIZE)
                {
                    isVerifyR69 = false;
                    break;
                }
            }

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R69, the actual size of every segment except for the last segment is {0}32 megabytes",
                isVerifyR69 ? string.Empty : "not ");

            // Verify MS-PCCRC requirement: MS-PCCRC_R69
            Site.CaptureRequirementIfIsTrue(
                isVerifyR69,
                PCCRCDOCSHORTNAME,
                69,
                @"[In SegmentDescription] Every segment except for the last segment must be exactly 
                32 megabytes in size");

            #endregion

            #region MS-PCCRC_R74
            bool isVerifyR74 = true;

            for (int i = 1; i < segmentInformation.cSegments; i++)
            {
                ulong offset = segmentInformation.segments[i - 1].ullOffsetInContent
                    + segmentInformation.segments[i - 1].cbSegment;
                if (segmentInformation.segments[i].ullOffsetInContent != offset)
                {
                    isVerifyR74 = false;
                    break;
                }
            }

            if (segmentInformation.segments[0].ullOffsetInContent != 0)
            {
                isVerifyR74 = false;
            }

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R74, the ullOffsetInContent in every SegmentDescription field is 
                {0}content offset at which the start of the segment begins.",
                isVerifyR74 ? string.Empty : "not ");

            // Verify MS-PCCRC requirement: MS-PCCRC_R74
            Site.CaptureRequirementIfIsTrue(
                isVerifyR74,
                PCCRCDOCSHORTNAME,
                74,
                @"[In SegmentDescription] ullOffsetInContent (8 bytes):  Content offset at which the start 
                of the segment begins.");

            #endregion

            #region MS-PCCRC_R75

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R75. The actual size of the cbSegment is {0}.",
                Marshal.SizeOf(segmentInformation.segments[0].cbSegment));

            // Verify MS-PCCRC requirement: MS-PCCRC_R75
            Site.CaptureRequirementIfAreEqual<int>(
                4,
                Marshal.SizeOf(segmentInformation.segments[0].cbSegment),
                PCCRCDOCSHORTNAME,
                75,
                @"[In SegmentDescription] cbSegment (4 bytes):  Total number of bytes in the segment, 
                regardless of how many of those bytes intersect the content range.");

            #endregion

            #region MS-PCCRC_R76

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R76. The actual size of the cbSegment is {0}.",
                Marshal.SizeOf(segmentInformation.segments[0].cbBlockSize));

            // Verify MS-PCCRC requirement: MS-PCCRC_R76
            Site.CaptureRequirementIfAreEqual<int>(
                4,
                Marshal.SizeOf(segmentInformation.segments[0].cbBlockSize),
                PCCRCDOCSHORTNAME,
                76,
                @"[In SegmentDescription] cbBlockSize (4 bytes):  
                Length of a content block within this segment, in bytes.");

            #endregion

            #region MS-PCCRC_R77
            bool isVerifyR77 = true;
            for (int i = 0; i < segmentInformation.cSegments; i++)
            {
                if (segmentInformation.segments[i].cbBlockSize != 65536)
                {
                    isVerifyR77 = false;
                }
            }

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R77, the block size of every segment is {0}65536 bytes",
                isVerifyR77 ? string.Empty : "not ");

            // Verify MS-PCCRC requirement: MS-PCCRC_R77
            Site.CaptureRequirementIfIsTrue(
                isVerifyR77,
                PCCRCDOCSHORTNAME,
                77,
                @"[In SegmentDescription] cbBlockSize (4 bytes): 
                Every segment MUST use the block size of 65536 bytes.");

            #endregion

            #region MS-PCCRC_R79
            bool isVerifyR79 = true;
            for (int i = 0; i < segmentInformation.cSegments; i++)
            {
                if (segmentInformation.segments[i].SegmentHashOfData.Length != hashLength)
                {
                    isVerifyR79 = false;
                }
            }

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R79, the SegmentHashOfData is {0} of length 32",
                isVerifyR79 ? string.Empty : "not ");

            // Verify MS-PCCRC requirement: MS-PCCRC_R79
            Site.CaptureRequirementIfIsTrue(
                isVerifyR79,
                PCCRCDOCSHORTNAME,
                79,
                @"[In SegmentDescription]  SegmentHashOfData (variable): The hash is of length 32 if dwHashAlgo 
                at the start of the Content Information was 0x800C = SHA-256.");

            #endregion           

            #region MS-PCCRC_R83
            bool isVerifyR83 = true;
            for (int i = 0; i < segmentInformation.cSegments; i++)
            {
                if (segmentInformation.segments[i].SegmentSecret.Length != hashLength)
                {
                    isVerifyR83 = false;
                }
            }

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R83, the actual length of the SegmentSecret is {0}",
                hashLength);

            // Verify MS-PCCRC requirement: MS-PCCRC_R83
            Site.CaptureRequirementIfIsTrue(
                isVerifyR83,
                PCCRCDOCSHORTNAME,
                83,
                @"[In SegmentDescription]  SegmentSecret (variable):The hash is of length 32 if dwHashAlgo 
                at the start of the Content Information was 0x800C = SHA-256.");

            #endregion
        }

        /// <summary>
        /// Validate pccrc segment info version part.
        /// </summary>
        /// <param name="segmentInformation">Content Information data structure.</param>
        /// <param name="valueOfDwHashAlgo">Value of DwHashAlgo.</param>
        /// <param name="hashLength">The length of hash.</param>
        private void ValidatePccrcSegmentInfoVersion(Content_Information_Data_Structure segmentInformation, int valueOfDwHashAlgo, int hashLength)
        {
            #region MS-PCCRC_R57

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R57, the actual value of Version is 0x{0:X8}",
                segmentInformation.Version);

            // Verify MS-PCCRC requirement: MS-PCCRC_R57
            Site.CaptureRequirementIfAreEqual<int>(
                0x0100,
                segmentInformation.Version,
                PCCRCDOCSHORTNAME,
                57,
                @"[In Content Information Data Structure Version] Version (2 bytes):  MUST be 0x0100.");

            #endregion

            #region MS-PCCRC_R53
            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R53:Content Information starts the data structure version.
                The Version is parsed at the first 2 byte word in Content Information by stack.
                If MS-PCCRC_R57 is verified successfully, MS-PCCRC_R53 is captured directly.");

            Site.CaptureRequirement(
                PCCRCDOCSHORTNAME,
                53,
                @"[In Content Information Data Structure Version] Content Information starts with 
                a single 2 byte WORD value representing the data structure version.");

            #endregion

            #region MS-PCCRC_R56
            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R56, Version is parsed at the first 2 byte word in Content Information by stack. 
                If MS-PCCRC_R57 is verified successfully, it indicates that the low byte is the minor version 
                numberand the high byte is the major version number.");

            Site.CaptureRequirement(
                PCCRCDOCSHORTNAME,
                56,
                @"[In Content Information Data Structure Version] Version (2 bytes):  The low byte is the minor
                version number and the high byte is the major version number.");

            #endregion

            #region MS-PCCRC_R58

            // Add the debug information.
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R58, the actual length of dwHashAlgo is: {0}",
                Marshal.SizeOf(valueOfDwHashAlgo));

            // Verify MS-PCCRC requirement: MS-PCCRC_R58
            Site.CaptureRequirementIfAreEqual<int>(
                4,
                Marshal.SizeOf(valueOfDwHashAlgo),
                PCCRCDOCSHORTNAME,
                58,
                @"[In Content Information Data Structure Version 1.0] dwHashAlgo (4 bytes):  Hash algorithm to use. <2> ");

            #endregion

            #region MS-PCCRC_R581

            bool isVerifyR581 = valueOfDwHashAlgo == 0x0000800C
                || valueOfDwHashAlgo == 0x0000800D
                || valueOfDwHashAlgo == 0x0000800E;

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R581, the actual value of dwHashAlgo is 0x{0:X8}.",
                valueOfDwHashAlgo);

            // Verify MS-PCCRC requirement: MS-PCCRC_R581
            Site.CaptureRequirementIfIsTrue(
                isVerifyR581,
                PCCRCDOCSHORTNAME,
                581,
                @"[In Content Information Data Structure Version 1.0] dwHashAlgo (4 bytes): MUST be one of the following 
                values:0x0000800C,0x0000800D,0x0000800E.");

            #endregion

            #region MS-PCCRC_R59

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R59, When segmentInformation.dwHashAlgo is 0x0000800C, 
                it is the SHA-256 hash algorithm. The actual value of dwHashAlgo is 0x{0:X8}.",
                valueOfDwHashAlgo);

            // Verify MS-PCCRC requirement: MS-PCCRC_R59
            Site.CaptureRequirementIfAreEqual<int>(
                0x0000800C,
                valueOfDwHashAlgo,
                PCCRCDOCSHORTNAME,
                59,
                @"[In Content Information Data Structure Version] dwHashAlgo (4 bytes):  
                When use the SHA-256 hash algorithm, the value is 0x0000800C.");

            #endregion

            #region MS-PCCRC_R62

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R62, dwOffsetInFirstSegment (4 bytes). 
                In MS-PCHC client, only the size of the dwOffsetInFirstSegment will be verified.");

            // Verify MS-PCCRC requirement: MS-PCCRC_R62
            Site.CaptureRequirementIfAreEqual<int>(
                Marshal.SizeOf(segmentInformation.dwOffsetInFirstSegment),
                4,
                PCCRCDOCSHORTNAME,
                62,
                @"[In Content Information Data Structure Version] dwOffsetInFirstSegment (4 bytes):  
                Number of bytes into the first segment within the Content Information data structure at 
                which the content range begins.");
            #endregion

            #region MS-PCCRC_R63
            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R63, dwReadBytesInLastSegment (4 bytes).
                In MS_PCHC client, only the size of the dwReadBytesInLastSegment will be verified.");

            // Verify MS-PCCRC requirement: MS-PCCRC_R63
            Site.CaptureRequirementIfAreEqual<int>(
                Marshal.SizeOf(segmentInformation.dwReadBytesInLastSegment),
                4,
                PCCRCDOCSHORTNAME,
                63,
                @"[In Content Information Data Structure Version] dwReadBytesInLastSegment (4 bytes):  
                Total number of bytes of the content range which lie within the final segment in the 
                Content Information data structure.");
            #endregion

            #region MS-PCCRC_R64

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R64, the actual value of the cSegments is {0}, 
                the size of the cSegments is {1} bytes.",
                segmentInformation.cSegments,
                Marshal.SizeOf(segmentInformation.cSegments));

            // Verify MS-PCCRC requirement: MS-PCCRC_R64
            Site.CaptureRequirementIfAreEqual<int>(
                segmentInformation.segments.Length,
                (int)segmentInformation.cSegments,
                PCCRCDOCSHORTNAME,
                64,
                @"[In Content Information Data Structure Version] cSegments (4 bytes):  
                The number of segments which intersect the content range and hence are contained 
                in the Content Information data structure.");

            #endregion

            #region MS-PCCRC_R65
            bool isVerifyR65 = true;

            for (int i = 0; i < segmentInformation.cSegments; i++)
            {
                if (!(segmentInformation.segments[i].cbBlockSize == 65536
                       && segmentInformation.segments[i].SegmentHashOfData.Length == hashLength
                       && segmentInformation.segments[i].SegmentSecret.Length == hashLength))
                {
                    isVerifyR65 = false;
                    break;
                }
            }

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R65, the segments variable {0} the Segment start offset, 
                length, block size, SegmentHashofData and SegmentSecret for each segment.",
                isVerifyR65 ? "contains" : "does't contain");

            // Verify MS-PCCRC requirement: MS-PCCRC_R65
            Site.CaptureRequirementIfIsTrue(
                isVerifyR65,
                PCCRCDOCSHORTNAME,
                65,
                @"[In Content Information Data Structure Version] segments (variable):  Segment start offset, length, 
                block size, SegmentHashofData and SegmentSecret for each segment.");

            #endregion
        }

        #endregion

        #region Capture PCHC Client RS

        /// <summary>
        /// Validate the transport between the client and the hosted cahce.
        /// </summary>
        /// <param name="method">The transport method.</param>
        private void ValidateTransport(string method)
        {
            // Capture MS-PCHC R2
            Site.CaptureRequirementIfAreEqual<string>(
               "post",
               method.ToLower(),
               2,
               "[In Transport] The client sends a request message as the payload of an HTTP POST request");
        }

        /// <summary>
        /// Capture the INITIAL_OFFER_MESSAGE message RS
        /// </summary>
        /// <param name="initialOfferMsg">A INITIAL_OFFER_MESSAGE message.</param>
        private void ValidateInitialOfferMessage(INITIAL_OFFER_MESSAGE initialOfferMsg)
        {
            // Validate the MessageHeader in INITIAL_OFFER_MESSAGE.
            this.ValidateRequestMessageHeader(initialOfferMsg.MsgHeader);

            // Validate the Connectioninformation in  INITIAL_OFFER_MESSAGE.
            this.ValidateConnectionInformation(initialOfferMsg.ConnectionInfo);

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R21:The PCHC_MESSAGE_TYPE {0} specifies the INITIAL_OFFER_MESSAGE.",
                (int)initialOfferMsg.MsgHeader.MsgType);

            // Capture MS-PCHC R21
            Site.CaptureRequirementIfAreEqual<PCHC_MESSAGE_TYPE>(               
                PCHC_MESSAGE_TYPE.INITIAL_OFFER_MESSAGE,
                initialOfferMsg.MsgHeader.MsgType,
                21,
                @"[In MESSAGE_HEADER] [Type (2 bytes):a 16-bit unsigned integer that specifies the 
                message type]The value 0x0001 represent message which is an INITIAL_OFFER_MESSAGE. ");

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R29:The initialOfferMsg is a INITIAL_OFFER_MESSAGE structure which of cause consists of 
                the fields [MessageHeader,ConnectionInformation,Hash].");

            // Capture MS-PCHC R29
            Site.CaptureRequirement(
                29,
                @"[In INITIAL_OFFER_MESSAGE] An INITIAL_OFFER_MESSAGE consists of the following fields
                [MessageHeader,ConnectionInformation,Hash].");

            int messageHeaderSize = System.Runtime.InteropServices.Marshal.SizeOf(initialOfferMsg.MsgHeader);
            
            // Capture MS-PCHC R30 and MS-PCHC R129 
            Site.Assert.AreEqual<int>(
                8,
                messageHeaderSize,
                @"Verify MS-PCHC_R30 and MS-PCHC_R129:The messageHeader is an instance of MessageHeader[8 bytes].");            

            // Capture MS-PCHC R129
            Site.CaptureRequirement(
                129,
                @"[In Request Messages] MessageHeader (8 bytes): A MESSAGE_HEADER structure (section 2.2.1.1).");

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R30:The PCHC_MESSAGE_TYPE {0} specifies the INITIAL_OFFER_MESSAGE.",
                (int)initialOfferMsg.MsgHeader.MsgType);

            // Capture MS-PCHC R30
            Site.CaptureRequirementIfAreEqual<PCHC_MESSAGE_TYPE>(
                PCHC_MESSAGE_TYPE.INITIAL_OFFER_MESSAGE,
                initialOfferMsg.MsgHeader.MsgType,
                30,
                @"[In INITIAL_OFFER_MESSAGE] MessageHeader (8 bytes):  A MESSAGE_HEADER structure 
                (section 2.2.1.1), with the Type field set to 0x0001.");

            // Capture MS-PCHC R33
            int sizeofInitialOfferMsg = System.Runtime.InteropServices.Marshal.SizeOf(initialOfferMsg);
            int sizeofMsgHeader = System.Runtime.InteropServices.Marshal.SizeOf(initialOfferMsg.MsgHeader);
            int sizeofConnectInfo = System.Runtime.InteropServices.Marshal.SizeOf(initialOfferMsg.ConnectionInfo);

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R33:The hash is a byte array. And initialOfferMsg is received from the client.
                The total message size is {0}, the sum of the filed sizes that precede the Hash field is 
                messageHeader: {1} + connectInformation: {2} = {3}. And the actual Hash size is {4}",
                sizeofInitialOfferMsg,
                sizeofMsgHeader,
                sizeofConnectInfo,
                sizeofMsgHeader + sizeofConnectInfo,
                initialOfferMsg.Hash.Length);

            Site.CaptureRequirementIfAreEqual<int>(
                initialOfferMsg.Hash.Length,
                (sizeofInitialOfferMsg - sizeofMsgHeader - sizeofConnectInfo) * 8,
                33,
                @"[In INITIAL_OFFER_MESSAGE] The size of this field[The Hash field] is calculated as the total 
                message size minus the sum of the field sizes that precede the Hash field.");
        }

        /// <summary>
        /// Capture SEGMENT_INFO_MESSAGE message RS
        /// </summary>
        /// <param name="segmentInfoMsg">A SEGMENT_INFO_MESSAGE message.</param>
        /// <param name="platformOsVersion">The operation system.</param>
        private void ValidateSegmentInfoMessage(SEGMENT_INFO_MESSAGE segmentInfoMsg, string platformOsVersion)
        {
            // Validate the MessageHeader in SEGMENT_INFO_MESSAGE.
            this.ValidateRequestMessageHeader(segmentInfoMsg.MsgHeader);

            // Validate the connectionInformation in SEGMENT_INFO_MESSAGE.
            this.ValidateConnectionInformation(segmentInfoMsg.ConnectionInfo);

            // Valdiate the SegmentInformation in SEGMENT_INFO_MESSAGE.
            this.ValidateSegmentInformation(segmentInfoMsg.SegmentInfo);

            // Validate the SegmentInformation about Pccrc 
            this.ValidateSegmentInformationFromPccrc(segmentInfoMsg.SegmentInfo);

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R22:The value of PCHCMessageType.SEGMENT_INFO_MESSAGE is {0}.",
                (int)segmentInfoMsg.MsgHeader.MsgType);

            // Capture MS-PCHC R22
            Site.CaptureRequirementIfAreEqual<PCHCMessageType>(               
                PCHCMessageType.SEGMENT_INFO_MESSAGE,
                (PCHCMessageType)segmentInfoMsg.MsgHeader.MsgType,
                22,
                @"[In MESSAGE_HEADER][Type (2 bytes):a 16-bit unsigned integer that specifies the message type]The
                value 0x0002 represent message which is a SEGMENT_INFO_MESSAGE. ");

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R36:The segmentInfoMsg is an instance of SEGMENT_INFO_MESSAGE which of cause consists of 
                [MessageHeader, ConnectionInformation, ContentTag, SegmentInformation (variable)]");

            // Capture MS-PCHC R36
            Site.CaptureRequirement(
                36,
                @"[In SEGMENT_INFO_MESSAGE] A SEGMENT_INFO_MESSAGE consists of the following fields[MessageHeader, 
                ConnectionInformation, ContentTag, SegmentInformation (variable)].");

            int sizeofMsgHeader = System.Runtime.InteropServices.Marshal.SizeOf(segmentInfoMsg.MsgHeader);

            // Capture MS-PCHC R37 and MS-PCHC R129 
            Site.Assert.AreEqual<int>(
                8,
                sizeofMsgHeader,
                @"Verify MS-PCHC_R37 and MS-PCHC_R129:The messageHeader is an instance of MessageHeader[8 bytes].");

            // Capture MS-PCHC R129
            Site.CaptureRequirement(
                129,
                @"[In Request Messages] MessageHeader (8 bytes): A MESSAGE_HEADER structure (section 2.2.1.1).");

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R37:The value of PCHCMessageType.SEGMENT_INFO_MESSAGE is {0}.",
                (int)segmentInfoMsg.MsgHeader.MsgType);

            // Capture MS-PCHC R37
            Site.CaptureRequirementIfAreEqual<PCHCMessageType>(
                PCHCMessageType.SEGMENT_INFO_MESSAGE,
                (PCHCMessageType)segmentInfoMsg.MsgHeader.MsgType,
                37,
                @"[In SEGMENT_INFO_MESSAGE] A SEGMENT_INFO_MESSAGE consists of MessageHeader (8 bytes):  
                A MESSAGE_HEADER structure (section 2.2.1.1), with the Type field set to 0x0002.");          
            
            int sizeofConnectInfo = System.Runtime.InteropServices.Marshal.SizeOf(segmentInfoMsg.ConnectionInfo);

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R38:The segmentInfoMsg Contains a CONNECTION_INFORMATION(8 bytes), 
                but the actual size of the ConnectionInfo is: {0}.",
                sizeofConnectInfo);

            Site.CaptureRequirementIfAreEqual<int>(
                8,
                sizeofConnectInfo,                 
                38,
                @"[In SEGMENT_INFO_MESSAGE] A SEGMENT_INFO_MESSAGE consists of ConnectionInformation (8 bytes):
                A CONNECTION_INFORMATION structure (section 2.2.1.2).");

            // Capture MS-PCHC R39
            int size = segmentInfoMsg.ContentTag.Length;
            Site.CaptureRequirementIfAreEqual<int>(                
                16,
                size,
                39,
                @"[In SEGMENT_INFO_MESSAGE] A SEGMENT_INFO_MESSAGE consists of ContentTag (16 bytes): 
                A structure consisting of 16 bytes of opaque data.");

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R41:[In SEGMENT_INFO_MESSAGE] [ContentTag ] The tag of 
                SEGMENT_INFO_MESSAGE is added to the information being sent by the client to the hosted cache.");

            // Capture MS-PCHC R41
            // If MS-PCHC_R39 is verified successfully, specify the ContentTag is added in. 
            // So MS-PCHC_R41 is captured directly.
            Site.CaptureRequirement(
                41,
                @"[In SEGMENT_INFO_MESSAGE] [ContentTag ] The tag of SEGMENT_INFO_MESSAGE is added to the 
                information being sent by the client to the hosted cache.");

            string contentTag = Encoding.ASCII.GetString(segmentInfoMsg.ContentTag);
            int endIndex = contentTag.IndexOf('\0');
            string contentTagNoSpace = contentTag.Substring(0, endIndex);
            bool isEqualASC = contentTagNoSpace.Equals("WinINet")
                 || contentTagNoSpace.Equals("WebIO")
                 || contentTagNoSpace.Equals("BITS-4.0");
            byte[] contentTagByteValue = new byte[16] 
                {
                    0x35, 0xDB, 0x04, 0x5D, 0x14, 0x23, 0x45, 0x53, 0xA0, 0x51, 0x0D, 0xC2, 0xE1, 0x5E, 0x6C, 0x4C
                };
            bool isEqualContentTagByteArray = true;
            for (int i = 0; i < 16; i++)
            {
                isEqualContentTagByteArray = isEqualContentTagByteArray && (segmentInfoMsg.ContentTag[i] == contentTagByteValue[i]);
            }

            if (platformOsVersion.ToLower().Equals(ClientRoleOSVersion.Win2K8.ToString().ToLower())
                || platformOsVersion.ToLower().Equals(ClientRoleOSVersion.Win2K8R2.ToString().ToLower())
                || platformOsVersion.ToLower().Equals(ClientRoleOSVersion.Win7.ToString().ToLower())
                || platformOsVersion.ToLower().Equals(ClientRoleOSVersion.WinVista.ToString().ToLower()))
            {
                // Add the debug information
                Site.Log.Add(
                    LogEntryKind.Debug,
                    @"Verify MS-PCHC_R124:The segmentInfoMsg Contain a CONTENT_TAG[16 bytes]. The CONTENT_TAG's 
                    expected values can be:The ASCII string WinINet.The ASCII string WebIO. The ASCII string BITS-4.0.
                    The binary byte array: 0x35, 0xDB, 0x04, 0x5D, 0x14, 0x23, 0x45, 0x53, 0xA0, 0x51, 0x0D, 
                    0xC2, 0xE1, 0x5E, 0x6C, 0x4C. The actual value is: {0}",
                    contentTagNoSpace);

                // Capture MS-PCHC R124
                Site.CaptureRequirementIfIsTrue(
                    isEqualASC || isEqualContentTagByteArray,
                    124,
                    @"<3> Section 3.2.1: In the Windows implementation, the values of the content tag can be the following:
                    The ASCII string ""WinINet"".
                    The ASCII string ""WebIO"".
                    The ASCII string ""BITS-4.0"".
                    The binary byte array {0x35, 0xDB, 0x04, 0x5D, 0x14, 0x23, 0x45, 0x53, 0xA0, 0x51, 0x0D, 0xC2, 0xE1,
                    0x5E, 0x6C, 0x4C}.");
            }
        }

        /// <summary>
        /// Validate client initialization RS.
        /// </summary>
        /// <param name="serverComputerName">The hosted cache computer name.</param>
        /// <param name="httpsPort">The port the hosted cache will listening.</param>
        /// <param name="transProtocol">The transport protocol.</param>
        private void ValidateClientInitialization(
            string serverComputerName,
            string httpsPort,
            string transProtocol)
        {
            // Capture MS-PCHC R8
            string expectServerComputerName = string.Format(this.GetProperty("Environment.HostedCacheServer.MachineName"));
            string expectHttpsPort = string.Format(this.GetProperty("PCHC.Protocol.NewPort"));

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R8:The client initialize contain the HostedCache computer name and the port of https. 
                The expected computer name is: {0}, the actual computer name is: {1};
                The expected port of https is: {2}, the actual port of https: is {3}", 
                expectServerComputerName, 
                serverComputerName, 
                expectHttpsPort, 
                httpsPort);

            Site.CaptureRequirementIfIsTrue(
                expectServerComputerName.ToLower().Equals(serverComputerName.ToLower())
                && expectHttpsPort.ToLower().Equals(httpsPort.ToLower()),
                8,
                @"[In Transport] The client MUST be configured with the location, including machine name 
                and port number, of the hosted cache that it will connect to when it has content to offer.");

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R106:The client initialize contain the DNS name and the TCP port of hosted cache. 
                The expected computer name is: {0}, the actual computer name is: {1};
                The expected port of https is: {2}, the actual port of https: is {3}",
                expectServerComputerName,
                serverComputerName,
                expectHttpsPort,
                httpsPort);

            // Capture MS-PCHC R106
            Site.CaptureRequirementIfIsTrue(
                expectServerComputerName.ToLower().Equals(serverComputerName.ToLower())
                && expectHttpsPort.ToLower().Equals(httpsPort.ToLower()),
                106,
                @"[In Initialization] The client initialization MUST explicitly include the following information: 
                The fully qualified DNS name and the TCP port of the hosted cache.");

            // Capture MS-PCHC R107
            Site.CaptureRequirementIfAreEqual<string>(
                "https",
                transProtocol.ToLower(),
                107,
                @"[In Initialization] The client initialization MUST explicitly include the following 
                information: The chain's root certificate such that it is compatible with HTTPS server 
                authentication [RFC2818].");
        }

        /// <summary>
        /// Validate the MESSAGE_HEADER of request message.
        /// </summary>
        /// <param name="messageHeader">A MESSAGE_HEADER in the request message.</param>
        private void ValidateRequestMessageHeader(MESSAGE_HEADER messageHeader)
        {
            // Capture MS-PCHC R15
            string typeofMinorVersion = messageHeader.MinorVersion.GetType().ToString();
            string typeofMajorVersion = messageHeader.MajorVersion.GetType().ToString();
            int versionSize = 0;
            if (typeofMinorVersion.Equals("System.Byte") && typeofMajorVersion.Equals("System.Byte"))
            {
                versionSize = 2;
            }

            Site.CaptureRequirementIfAreEqual<int>(
                2,
                versionSize,
                15,
                @"[In MESSAGE_HEADER] Version (2 bytes): The message version, 
                expressed as major and minor values.");
           
            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R17, capture it directly for the message header is unmarshalled correctly from stack.");

            // Capture MS-PCHC R17
            Site.CaptureRequirement(
                17,
                @"[In MESSAGE_HEADER] Note that the order of the subfields[MinorVersion,MajorVersion] 
                is reversed; the MinorVersion subfield occurs first.");

            // Capture MS-PCHC R18
            Site.CaptureRequirementIfAreEqual<byte>(
                0x00,
                messageHeader.MinorVersion,
                18,
                "[In MESSAGE_HEADER] MinorVersion (1 byte):  The minor part of the version, which MUST be 0x00.");

            // Capture MS-PCHC R19
            Site.CaptureRequirementIfAreEqual<byte>(
                0x01,
                messageHeader.MajorVersion,
                19,
                "[In MESSAGE_HEADER] MajorVersion (1 byte):  The major part of the version, which MUST be 0x01.");

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R16:The Version is 1.0. The minor version is 0x00 and verified in MS-PCHC_R18,
                major version is 0x01 and verified in MS-PCHC_R19,
                the actual minor versionis {0} and the actual major version is{1}",
                (int)messageHeader.MajorVersion,
                (int)messageHeader.MinorVersion);

            // Capture MS-PCHC R16
            Site.CaptureRequirement(
                16,
                "[In MESSAGE_HEADER] The version[expressed as major and minor values] MUST be 1.0.");

            // Capture MS-PCHC R20
            int messageTypeSize = sizeof(PCHCMessageType);
            Site.CaptureRequirementIfAreEqual<int>(
                2,
                messageTypeSize,
                20,
                "[In MESSAGE_HEADER] Type (2 bytes):  A 16-bit unsigned integer .");

            // Capture MS-PCHC R23
            Site.CaptureRequirementIfAreEqual<int>(
                4,
                messageHeader.Padding.Length,
                23,
                "[In MESSAGE_HEADER] Padding (4 bytes).");

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R14:The messageHeader is an instance of MESSAGE_HEADER.
                Above requirements about Version, Type, Padding are already valideted.");

            // Capture MS-PCHC R14
            Site.CaptureRequirement(
                14,
                @"[In MESSAGE_HEADER] All Peer Content Caching and Retrieval: 
                Hosted Cache Protocol [MS-PCHC] request messages use a common header, 
                which consists of the following fields[Version, Type, Padding].");
        }

        /// <summary>
        /// Validate the CONNECTION_INFORMATION of request message.
        /// </summary>
        /// <param name="connectInfo">A CONNECTION_INFORMATION in the request message.</param>
        private void ValidateConnectionInformation(CONNECTION_INFORMATION connectInfo)
        {
            // Capture MS-PCHC R25
            int portSize = System.Runtime.InteropServices.Marshal.SizeOf(connectInfo.Port);
            Site.CaptureRequirementIfAreEqual<int>(
                2,
                portSize,
                25,
                @"[In CONNECTION_INFORMATION] Port (2 bytes):  A 16-bit unsigned integer that MUST 
                be set by the client to the port on which it is listening as a server-role peer, 
                for use with the retrieval protocol.");

            // Capture MS-PCHC R26
            Site.CaptureRequirementIfAreEqual<int>(
                6,
                connectInfo.Padding.Length,
                26,
                "[In CONNECTION_INFORMATION] Padding (6 bytes).");

            int connectInfoSize = System.Runtime.InteropServices.Marshal.SizeOf(connectInfo);
            Site.CaptureRequirementIfAreEqual<int>(
                8,
                connectInfoSize,
                31,
                @"[In INITIAL_OFFER_MESSAGE] ConnectionInformation (8 bytes):  
                A CONNECTION_INFORMATION structure (section 2.2.1.2).");

            // Capture MS-PCHC R130
            Site.CaptureRequirementIfAreEqual<int>(
                8,
                connectInfoSize,                
                130,
                @"[In Request Messages]ConnectionInformation (8 bytes): 
                A CONNECTION_INFORMATION structure (section 2.2.1.2).");
        }

        /// <summary>
        /// Validate the SegmentInformation in the Segment_info-Message.
        /// </summary>
        /// <param name="segmentInformation">The segmentInformation need to be validate.</param>
        private void ValidateSegmentInformation(Content_Information_Data_Structure segmentInformation)
        {
            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R43:The SegmentInformation is an instance of  Content_Information_Data_Structure.");

            // Capture MS-PCHC R43
            Site.CaptureRequirement(
                43,
                @"[In SEGMENT_INFO_MESSAGE] SegmentInformation (variable): 
                A Content Information data structure ([MS-PCCRC] section 2.3).");

            // Add the debug information           
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCHC_R45:The SegmentInformation is an instance of Content_Information_Data_Structure 
                which surely contains the subfields of the segment's Content Information data structure, 
                and SegmentContentBlocks. And can be validated unmarshalled correctly from stack.");

            // Capture MS-PCHC R45
            Site.CaptureRequirement(               
                45,
                @"[In SEGMENT_INFO_MESSAGE] The SegmentInformation field also contains the subfields of 
                the segment's Content Information data structure, SegmentDescription, and SegmentContentBlocks, 
                as specified in [MS-PCCRC] sections 2.3.1.1 and 2.3.1.2, respectively.");

            // Capture MS-PCHC R49
            Site.CaptureRequirementIfAreEqual<uint>(
                1,
                segmentInformation.cSegments,
                49,
                "[In SEGMENT_INFO_MESSAGE, SEGMENT_INFORMATION (variable)] The cSegments field MUST be set to 1.");

            // Capture MS-PCHC R50
            Site.Assert.IsInstanceOfType(
                segmentInformation.segments,
                typeof(SegmentDescription[]),
                @"Validate the segments is SegmentDescription array, in MS-PCHC client, 
                only the size of the SegmentDescription will be verified,the actual is {0} array.",
                segmentInformation.segments);

            Site.CaptureRequirementIfAreEqual<int>(
                1,
                segmentInformation.segments.Length,
                50,
                @"[In SEGMENT_INFO_MESSAGE, SEGMENT_INFORMATION (variable)] The segments field MUST contain the single 
                SegmentDescription ([MS-PCCRC] section 2.3.1.1) in the original Content Information data structure 
                corresponding to the segment being offered.");

            // Capture MS-PCHC R51
            Site.Assert.IsInstanceOfType(
                segmentInformation.blocks,
                typeof(SegmentContentBlocks[]),
                @"Validate the blocks is SegmentContentBlocks array, in MS-PCHC client, 
                only the size of the SegmentContentBlocks will be verified, the actual is {0} array.",
                segmentInformation.blocks);

            Site.CaptureRequirementIfAreEqual<int>(
                1,
                segmentInformation.blocks.Length,
                51,
                @"[In SEGMENT_INFO_MESSAGE, SEGMENT_INFORMATION (variable)] The blocks field MUST 
                contain a single SegmentContentBlocks ([MS-PCCRC] section 2.3.1.2) corresponding 
                to the segment being offered, copied from the blocks field in the original 
                Content Information data structure.");
        }

        #endregion
    }
}

