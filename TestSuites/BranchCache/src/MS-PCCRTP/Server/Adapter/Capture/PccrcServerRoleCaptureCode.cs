// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrtp
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrc;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrtp;

    /// <summary>
    /// Verify server adapter requirements about MS-PCCRC.
    /// </summary>
    public partial class PCCRTPServerAdapter
    {
        #region Verify Content Information Structure.

        /// <summary>
        /// Verify requirements about content information structure.
        /// </summary>
        /// <param name="pccrtpResponse">The PCCRTP response message.</param>
        private void VerifyContentInfomationStructure(PccrtpResponse pccrtpResponse)
        {
            Site.DefaultProtocolDocShortName = "MS-PCCRC";

            this.VerifyContentSegmentsBlocks(pccrtpResponse);

            this.VerifySegmentIdentifiersAndKeys(pccrtpResponse);

            this.VerifyDataStructure(pccrtpResponse);

            this.VerifySegmentDescription(pccrtpResponse);

            this.VerifySegmentContentBlocks(pccrtpResponse);

            Site.DefaultProtocolDocShortName = "MS-PCCRTP";
        }

        #endregion

        #region Verify Content, Segments, and Blocks defined in section 2.1.

        /// <summary>
        /// Verify Content, Segments, and Blocks defined in section 2.1.
        /// </summary>
        /// <param name="pccrtpResponse">The HTTP resopnse.</param>
        private void VerifyContentSegmentsBlocks(PccrtpResponse pccrtpResponse)
        {
            Content_Information_Data_Structure contentInfo = pccrtpResponse.ContentInfo;

            #region MS-PCCRC_R3

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                "Verify MS-PCCRC_R3.segment is {0} type",
                pccrtpResponse.HttpHeader[ACCEPTRANGES]);

            bool isVerifyR3 = pccrtpResponse.HttpHeader[ACCEPTRANGES].Equals("bytes");

            // Verify MS-PCCRC requirement: MS-PCCRC_R3
            Site.CaptureRequirementIfIsTrue(
                isVerifyR3,
                3,
                @"[In Content, Segments, and Blocks] Each segment is a binary string.");

            #endregion

            #region MS-PCCRC_R10

            bool isVerifyR10 = true;
            for (int i = 0; i < contentInfo.cSegments - 1; i++)
            {
                if (contentInfo.segments[i].cbBlockSize != STANDARDBBLOCKSIZE)
                {
                    isVerifyR10 = false;
                    break;
                }
            }

            // Check for the last block in the last segment, which may be shorter than the standard block size (64 KB).
            if (contentInfo.segments[contentInfo.cSegments - 1].cbBlockSize > STANDARDBBLOCKSIZE)
            {
                isVerifyR10 = false;
            }

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                "Verify MS-PCCRC_R10. Each block size is {0} 64 kilobytes",
                isVerifyR10 ? string.Empty : "not");

            // Verify MS-PCCRC requirement: MS-PCCRC_R10
            Site.CaptureRequirementIfIsTrue(
                isVerifyR10,
                10,
                @"[In Content, Segments, and Blocks] Each block is a binary string of a fixed size (64 kilobytes), 
                except for the last block in the last segment, which again may be shorter.");

            #endregion

            #region MS-PCCRC_R4

            string allLength = string.Empty;

            if (pccrtpResponse.HttpHeader.ContainsKey(CONTENTRANGE))
            {
                // Get the value of content length in the response for partial request.
                allLength = pccrtpResponse.HttpHeader[CONTENTRANGE];
                allLength = allLength.Substring(allLength.IndexOf('/') + 1);
            }
            else if (pccrtpResponse.HttpHeader.ContainsKey(XP2PPEERDIST))
            {
                // Get the value of content length in the response for full request.
                allLength = pccrtpResponse.HttpHeader[XP2PPEERDIST];
                allLength = allLength.Substring(allLength.LastIndexOf('=') + 1);
            }

            bool isVerifyR4 = true;
            long contentLength = 0;

            try
            {
                contentLength = Convert.ToInt64(allLength);
            }
            catch (FormatException e)
            {
                throw new FormatException(e.ToString());
            }

            long lastSegmentLength = 0;
            for (int i = 0; i < contentInfo.cSegments; i++)
            {
                // If there are multiple segments, check each segment is of a standard size (32 MB).
                if (contentLength > STANDARDSEGMENTSIZE)
                {
                    if (contentInfo.segments[i].cbSegment != STANDARDSEGMENTSIZE)
                    {
                        isVerifyR4 = false;
                        break;
                    }

                    contentLength -= STANDARDSEGMENTSIZE;
                }
                else
                {
                    // Check for the last segment, which may be shorter than the standard segment size (32 MB).
                    if (contentLength != contentInfo.segments[i].cbSegment)
                    {
                        if (i > 0)
                        {
                            lastSegmentLength = contentLength;
                        }

                        isVerifyR4 = false;
                        break;
                    }
                }
            }

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                "Verify MS-PCCRC_R4.segment size is {0}32 megabytes except possibly the last segment",
                 isVerifyR4 ? string.Empty : "not ");

            // Verify MS-PCCRC requirement: MS-PCCRC_R4
            Site.CaptureRequirementIfIsTrue(
                isVerifyR4,
                4,
                @"[In Content, Segments, and Blocks] Each segment is of a standard size (32 megabytes), 
                except possibly the last segment which may be smaller if the content size is not a multiple 
                of the standard segment size.");

            #endregion

            // MS-PCCRC_R63 is blocked by TDI(Techical Document Issue) about inaccurate definition of
            // dwReadBytesInLastSegment if the HTTP request is a range retrieval request.
            if (bool.Parse(Site.Properties.Get("PCCRC.IsTDI.65999.Fixed")))
            {
                #region MS-PCCRC_R63

                // Add the debug information
                Site.Log.Add(
                    LogEntryKind.Debug,
                    @"Verify MS-PCCRC_R63, the actual value of the dwReadBytesInLastSegment is {0}",
                    contentInfo.dwReadBytesInLastSegment);

                // Verify MS-PCCRC requirement: MS-PCCRC_R63
                Site.CaptureRequirementIfAreEqual<long>(
                        lastSegmentLength,
                        contentInfo.dwReadBytesInLastSegment,
                        63,
                        @"[In Content Information Data Structure Version 1.0] dwReadBytesInLastSegment (4 bytes):  Total 
                        number of bytes of the content range which lie within the final segment in the Content 
                        Information data structure.");

                #endregion
            }
        }

        #endregion

        #region Verify Segment Identifiers (HoHoDK) and Keys defined in section 2.2.

        /// <summary>
        /// Verify Segment Identifiers (HoHoDK) and Keys defined in section 2.2.
        /// </summary>
        /// <param name="pccrtpResponse">The HTTP response.</param>
        private void VerifySegmentIdentifiersAndKeys(PccrtpResponse pccrtpResponse)
        {
            #region MS-PCCRC_R26

            bool isVerifyR26 = pccrtpResponse.ContentInfo.dwHashAlgo == dwHashAlgo_Values.V1
                               || pccrtpResponse.ContentInfo.dwHashAlgo == dwHashAlgo_Values.V2
                               || pccrtpResponse.ContentInfo.dwHashAlgo == dwHashAlgo_Values.V3;

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                "Verify MS-PCCRC_R26. the dwhashAlgo of hash is {0}",
                pccrtpResponse.ContentInfo.dwHashAlgo);

            // Verify MS-PCCRTP requirement: MS-PCCRC_R26
            Site.CaptureRequirementIfIsTrue(
                isVerifyR26,
                26,
                @"[In Segment Identifiers (HoHoDk) and Keys] Notation: Hash: The input hash function,
                which MUST be one of the hash functions listed in section 2.3.");

            #endregion

            #region MS-PCCRC_R37

            bool isVerifyR37 = true;
            if (pccrtpResponse.ContentInfo.segments != null && pccrtpResponse.ContentInfo.blocks != null)
            {
                for (int i = 0; i < pccrtpResponse.ContentInfo.segments.Length; i++)
                {
                    if (pccrtpResponse.ContentInfo.segments[i].SegmentHashOfData == null
                        || pccrtpResponse.ContentInfo.segments[i].SegmentSecret == null)
                    {
                        isVerifyR37 = false;
                        break;
                    }
                }
            }
            else
            {
                isVerifyR37 = false;
            }

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R37. BlockHashes, SegmentHashOfData , SegmentSecret are {0}inclueded in the Content 
                Information Data Structure.",
                isVerifyR37 ? string.Empty : "not ");

            // Verify MS-PCCRC requirement: MS-PCCRC_R37
            Site.CaptureRequirementIfIsTrue(
                isVerifyR37,
                37,
                @"[In Segment Identifiers (HoHoDk) and Keys]  ContentInfo includes only the list of block hashes, 
                the HoD, and Kp.");

            #endregion

            #region MS-PCCRC_R3701

            // Verify MS-PCCRC requirement: MS-PCCRC_R3701
            // The ContentInfo is parsed according the definition of Content Information Data Structure by the stack.
            // If ContentInfo improperly includes Ke and HoHoDk, then an exception will be thrown in the stack layer.
            // The program runs into here which has indicated that Ke and HoHoDk are not included in ContentInfo.
            // So MS-PCCRC_R3701 is captured directly.
            Site.CaptureRequirement(
                3701,
                @"[In Segment Identifiers (HoHoDk) and Keys]Ke and HoHoDk are not included in ContentInfo.");

            #endregion
        }

        #endregion

        #region Verify Data Structure of Content Information defined in section 2.3.

        /// <summary>
        /// Verify Data Structure of Content Information defined in section 2.3.
        /// </summary>
        /// <param name="pccrtpResponse">The HTTP resopnse.</param>
        private void VerifyDataStructure(PccrtpResponse pccrtpResponse)
        {
            Content_Information_Data_Structure contentInfo = pccrtpResponse.ContentInfo;

            #region MS-PCCRC_R58

            int hashLength = 0;
            int valueOfDwHashAlgo = 0;
            switch (pccrtpResponse.ContentInfo.dwHashAlgo)
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

            // Add the debug information.
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R58, the actual length of dwHashAlgo is: {0}",
                Marshal.SizeOf(valueOfDwHashAlgo));

            // Verify MS-PCCRC requirement: MS-PCCRC_R58
            Site.CaptureRequirementIfAreEqual<int>(
                4,
                Marshal.SizeOf(valueOfDwHashAlgo),
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
                581,
                @"[In Content Information Data Structure Version 1.0] dwHashAlgo (4 bytes): MUST be one of the following 
                values:0x0000800C,0x0000800D,0x0000800E.");

            #endregion

            #region MS-PCCRC_R59

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R59, , the actual value of dwHashAlgo is 0x{0:X8}.",
                valueOfDwHashAlgo);

            // Verify MS-PCCRC requirement: MS-PCCRTP_R59
            Site.CaptureRequirementIfAreEqual<int>(
                0x0000800C,
                valueOfDwHashAlgo,
                59,
                @"[In Content Information Data Structure Version 1.0] dwHashAlgo (4 bytes):  When use the SHA-256 hash 
                algorithm, the value is 0x0000800C.");

            #endregion

            #region MS-PCCRC_R54

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R54, all fields are {0}in little-endian byte order",
                BitConverter.IsLittleEndian ? string.Empty : "not ");

            // Verify MS-PCCRC requirement: MS-PCCRC_R54
            Site.CaptureRequirementIfIsTrue(
                BitConverter.IsLittleEndian,
                54,
                @"[In Content Information Data Structure Version 1.0] All fields are in little-endian byte order.");

            #endregion

            #region MS-PCCRC_R57

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R57, the actual value of Version is 0x{0:X8}",
                contentInfo.Version);

            // Verify MS-PCCRC requirement: MS-PCCRTP_R57
            Site.CaptureRequirementIfAreEqual<int>(
                0x0100,
                contentInfo.Version,
                57,
                @"[In Content Information Data Structure Version 1.0] Version (2 bytes):  MUST be 0x0100.");

            #endregion

            #region MS-PCCRC_R53

            // The Version is parsed at the first 2 byte word in Content Information by stack. 
            // If MS-PCCRC_R57 is verified successfully, MS-PCCRC_R53 is verified and so captured directly.
            Site.CaptureRequirement(
                53,
                @"[In Content Information Data Structure Version 1.0] Content Information starts with a single 2 byte 
                WORD value representing the data structure version.");

            #endregion

            #region MS-PCCRC_R56

            // The Version is parsed at the first 2 byte word in Content Information by stack. 
            // If MS-PCCRC_R57 is verified successfully, it indicates that the low byte is the minor version number
            // and the high byte is the major version number.
            Site.CaptureRequirement(
                56,
                @"[In Content Information Data Structure Version 1.0] Version (2 bytes):  The low byte is the minor 
                version number and the high byte is the major version number.");

            #endregion

            #region MS-PCCRC_R55

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R55, the actual size of the Version is {0}",
                Marshal.SizeOf(contentInfo.Version));

            // Verify MS-PCCRC requirement: MS-PCCRTP_R55
            Site.CaptureRequirementIfAreEqual<int>(
                2,
                Marshal.SizeOf(contentInfo.Version),
                55,
                @"[In Content Information Data Structure Version 1.0] Version (2 bytes):  Content Information version 
                (0x0100 is version 1.0).");

            #endregion

            #region MS-PCCRC_R64

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R64, the actual value of the cSegments is {0}",
                contentInfo.cSegments);

            // Verify MS-PCCRC requirement: MS-PCCRC_R64
            Site.CaptureRequirementIfAreEqual<int>(
                contentInfo.segments.Length,
                (int)contentInfo.cSegments,
                64,
                @"[In Content Information Data Structure Version 1.0] cSegments (4 bytes):  The number of segments 
                which intersect the content range and hence are contained in the Content Information data structure.");

            #endregion

            #region MS-PCCRC_R65

            bool isVerifyR65 = true;

            for (int i = 0; i < contentInfo.cSegments; i++)
            {
                if (contentInfo.segments[i].cbBlockSize == STANDARDBBLOCKSIZE
                       && contentInfo.segments[i].SegmentHashOfData.Length == hashLength
                       && contentInfo.segments[i].SegmentSecret.Length == hashLength)
                {
                    isVerifyR65 = true;
                }
                else
                {
                    isVerifyR65 = false;
                    break;
                }
            }

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R65, the segments variable {0} contain the Segment start offset, 
                length, block size, SegmentHashofData and SegmentSecret for each segment.",
                isVerifyR65 ? string.Empty : "does't");

            // Verify MS-PCCRC requirement: MS-PCCRC_R65
            Site.CaptureRequirementIfIsTrue(
                isVerifyR65,
                65,
                @"[In Content Information Data Structure Version 1.0] segments (variable):  Segment start offset, length, 
                block size, SegmentHashofData and SegmentSecret for each segment.");

            #endregion
        }

        #endregion

        #region Verify SegmentDescription defined in section 2.3.1.1.

        /// <summary>
        /// Verify SegmentDescription defined in section 2.3.1.1.
        /// </summary>
        /// <param name="pccrtpResponse">The HTTP response.</param>
        private void VerifySegmentDescription(PccrtpResponse pccrtpResponse)
        {
            #region MS-PCCRC_R67

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R67, the actual number of SegmentDescription fields in segments field is {0}",
                pccrtpResponse.ContentInfo.segments.Length);

            // Verify MS-PCCRC requirement: MS-PCCRC_R67
            Site.CaptureRequirementIfAreEqual<uint>(
                pccrtpResponse.ContentInfo.cSegments,
                (uint)pccrtpResponse.ContentInfo.segments.Length,
                67,
                @"[In SegmentDescription] The segments field is composed of a number cSegments of 
                SegmentDescription fields.");

            #endregion

            #region MS-PCCRC_R69

            bool isVerifyR69 = true;
            for (int i = 0; i < pccrtpResponse.ContentInfo.cSegments - 1; i++)
            {
                if (pccrtpResponse.ContentInfo.segments[i].cbSegment != STANDARDSEGMENTSIZE)
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
                69,
                @"[In SegmentDescription] Every segment except for the last segment must be exactly 
                32 megabytes in size.");

            #endregion

            int hashLength = 0;
            switch (pccrtpResponse.ContentInfo.dwHashAlgo)
            {
                case dwHashAlgo_Values.V1:
                    hashLength = 32;
                    break;
                case dwHashAlgo_Values.V2:
                    hashLength = 48;
                    break;
                case dwHashAlgo_Values.V3:
                    hashLength = 64;
                    break;
                default:
                    break;
            }

            #region MS-PCCRC_R74

            bool isVerifyR74 = true;
            bool isVerifyR77 = true;
            bool isVerifyR79 = true;
            bool isVerifyR83 = true;
            bool isVerifyR76 = true;

            for (int i = 1; i < pccrtpResponse.ContentInfo.cSegments; i++)
            {
                ulong offset = pccrtpResponse.ContentInfo.segments[i - 1].ullOffsetInContent 
                    + pccrtpResponse.ContentInfo.segments[i - 1].cbSegment;
                if (pccrtpResponse.ContentInfo.segments[i].ullOffsetInContent != offset)
                {
                    isVerifyR74 = false;
                }

                if (pccrtpResponse.ContentInfo.segments[i].cbBlockSize != STANDARDBBLOCKSIZE)
                {
                    isVerifyR77 = false;
                }

                if (Marshal.SizeOf(pccrtpResponse.ContentInfo.segments[i].cbBlockSize) != 4)
                {
                    isVerifyR76 = false;
                }

                if (pccrtpResponse.ContentInfo.segments[i].SegmentHashOfData.Length != hashLength)
                {
                    isVerifyR79 = false;
                }

                if (pccrtpResponse.ContentInfo.segments[i].SegmentSecret.Length != hashLength)
                {
                    isVerifyR83 = false;
                }
            }

            if (pccrtpResponse.ContentInfo.segments[0].ullOffsetInContent != 0)
            {
                isVerifyR74 = false;
            }

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R74, the ullOffsetInContent in every SegmentDescription field is {0}content offset 
                at which the start of the segment begins.",
                isVerifyR74 ? string.Empty : "not ");

            // Verify MS-PCCRC requirement: MS-PCCRC_R74
            Site.CaptureRequirementIfIsTrue(
                isVerifyR74,
                74,
                @"[In SegmentDescription] ullOffsetInContent (8 bytes):  Content offset at which the start 
                of the segment begins.");

            #endregion

            #region MS-PCCRC_R75

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R75. The actual size of the cbSegment is {0}.",
                Marshal.SizeOf(pccrtpResponse.ContentInfo.segments[0].cbSegment));

            // Verify MS-PCCRC requirement: MS-PCCRC_R75
            Site.CaptureRequirementIfAreEqual<int>(
                4,
                Marshal.SizeOf(pccrtpResponse.ContentInfo.segments[0].cbSegment),
                75,
                @"[In SegmentDescription] cbSegment (4 bytes):  Total number of bytes in the segment, 
                regardless of how many of those bytes intersect the content range.");

            #endregion

            #region MS-PCCRC_R76

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R76. The actual size of the cbBlockSize is {0}4 bytes.",
                isVerifyR76 ? string.Empty : "not ");

            // Verify MS-PCCRC requirement: MS-PCCRC_R76
            Site.CaptureRequirementIfIsTrue(
                isVerifyR76,
                76,
                @"[In SegmentDescription] cbBlockSize (4 bytes):  
                Length of a content block within this segment, in bytes.");

            #endregion

            #region MS-PCCRC_R77

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R77, the block size of every segment is {0}65536 bytes",
                isVerifyR77 ? string.Empty : "not ");

            // Verify MS-PCCRC requirement: MS-PCCRC_R77
            Site.CaptureRequirementIfIsTrue(
                isVerifyR77,
                77,
                @"[In SegmentDescription] cbBlockSize (4 bytes): Every segment MUST use the block size of 65536 bytes.");

            #endregion

            #region MS-PCCRC_R79

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R79, the actual SegmentHashOfData is {0} of length 32",
                isVerifyR79 ? string.Empty : "not ");

            // Verify MS-PCCRC requirement: MS-PCCRC_R79
            Site.CaptureRequirementIfIsTrue(
                isVerifyR79,
                79,
                @"[In SegmentDescription]  SegmentHashOfData (variable): The hash is of length 32 if dwHashAlgo 
                at the start of the Content Information was 0x800C = SHA-256.");

            #endregion

            #region MS-PCCRC_R83

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R83, the actual length of the SegmentSecret is {0}",
                hashLength);

            // Verify MS-PCCRC requirement: MS-PCCRC_R83
            Site.CaptureRequirementIfIsTrue(
                isVerifyR83,
                83,
                @"[In SegmentDescription]  SegmentSecret (variable):The hash is of length 32 if dwHashAlgo 
                at the start of the Content Information was 0x800C = SHA-256.");

            #endregion
        }

        #endregion

        #region Verify SegmentContentBlocks defined in section 2.3.1.2.

        /// <summary>
        /// Verify SegmentContentBlocks defined in section 2.3.1.2.
        /// </summary>
        /// <param name="pccrtpResponse">The HTTP response.</param>
        private void VerifySegmentContentBlocks(PccrtpResponse pccrtpResponse)
        {
            #region MS-PCCRC_R86

            // Add the debug information
            Site.Log.Add(
                LogEntryKind.Debug,
                @"Verify MS-PCCRC_R86, the actual number of SegmentContentBlocks fields in blocks field is {0}",
                pccrtpResponse.ContentInfo.blocks.Length);

            // Verify MS-PCCRC requirement: MS-PCCRC_R86
            Site.CaptureRequirementIfAreEqual<uint>(
                pccrtpResponse.ContentInfo.cSegments,
                (uint)pccrtpResponse.ContentInfo.blocks.Length,
                86,
                @"[In SegmentContentBlocks] The blocks field contains a number cSegments of 
                SegmentContentBlocks fields.");

            #endregion

            #region MS-PCCRC_R89

            bool isVerifyR89 = true;

            int hashLength = 0;
            switch (pccrtpResponse.ContentInfo.dwHashAlgo)
            {
                case dwHashAlgo_Values.V1:
                    hashLength = 32;
                    break;
                case dwHashAlgo_Values.V2:
                    hashLength = 48;
                    break;
                case dwHashAlgo_Values.V3:
                    hashLength = 64;
                    break;
                default:
                    break;
            }

            foreach (SegmentContentBlocks segContentBlocks in pccrtpResponse.ContentInfo.blocks)
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
                @"Verify MS-PCCRC_R89, the actual size of BlockHashes field is {0} cBlocks * {1}",
                isVerifyR89 ? string.Empty : "not ",
                hashLength);

            // Verify MS-PCCRC requirement: MS-PCCRC_R89
            Site.CaptureRequirementIfIsTrue(
                isVerifyR89,
                89,
                @"[In SegmentContentBlocks] BlockHashes (variable):The size of this field is 
                cBlocks * (32, 48 or 64, depending on which hash was used).");

            #endregion

            #region MS-PCCRC_R12

            // The BlockHashes field is parsed according the cBlocks and the hash algorithm by the stack.
            // If MS-PCCRC_R89 is verified successfully, it indicates that the BlockHashes is the hash list 
            // of each block in the order the block in the segment.So MS-PCCRC_R12 is captured directly.
            Site.CaptureRequirement(
                12,
                @"[In Content, Segments, and Blocks] Blocks within a segment are identified by their progressive 
                index within the segment (Block 0 is the first block in the segment, Block 1 the second, and so on).");

            #endregion
        }

        #endregion
    }
}
