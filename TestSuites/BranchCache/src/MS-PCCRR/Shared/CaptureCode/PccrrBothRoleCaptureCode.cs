// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrr
{
    using System;
    using System.Net;
    using System.Runtime.InteropServices;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr;

    /// <summary>
    /// Capture the both client and server role requirements of MS-PCCRR.
    /// </summary>
    public static class PccrrBothRoleCaptureCode
    {
        #region Fields

        /// <summary>
        /// ITestSite used for capture requirements.
        /// </summary>
        private static ITestSite site;

        #endregion

        #region Initialize

        /// <summary>
        /// Initialize ITestSite.
        /// </summary>
        /// <param name="testSite">The adapter's ITestSite instance.</param>
        public static void Initialize(ITestSite testSite)
        {
            site = testSite;
        }

        #endregion

        #region Capture Requirements

        #region Capture requirements related to http

        /// <summary>
        /// Capture http releated requirements.
        /// </summary>
        public static void CaptureHttpRequirements()
        {
            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R8");

            // Verify MS-PCCRR requirement: MS-PCCRR_R8
            site.CaptureRequirement(
                8,
                @"[In Peer Download Transport]Both the Retrieval Protocol request and response message types are included
                in the body of the HTTP messages.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R9001");

            // Verify MS-PCCRR requirement: MS-PCCRR_R9001
            site.CaptureRequirement(
                9001,
                @"[In Peer Download Transport]The payload of each such HTTP request consists solely of a single
                Retrieval Protocol message, with the response message prefixed with an additional length field
                (as defined in section 2.2.2) for reassembly purposes.");
        }

        #endregion Capture requirements related to http

        #region Capture requirements related to BLOCK_RANGE structure

        /// <summary>
        /// Capture BLOCK_RANGE structure releated requirements.
        /// </summary>
        /// <param name="blockRange">BLOCK_RANGE object</param>
        public static void CaptureBlockRangeRequirements(BLOCK_RANGE blockRange)
        {
            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R26");

            // Verify MS-PCCRR requirement: MS-PCCRR_R26
            site.CaptureRequirementIfAreEqual<int>(
                4,
                Marshal.SizeOf(blockRange.Index),
                26,
                @"[In BLOCK_RANGE]Index (4 bytes):  The index of the first block in the range.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R27");

            // Verify MS-PCCRR requirement: MS-PCCRR_R27
            // Eccept the 4 bytes, this sentence is internal behavior, so this requirements can noly be 
            // verified a small part.
            site.CaptureRequirementIfAreEqual<int>(
                4,
                Marshal.SizeOf(blockRange.Count),
                27,
                @"[In BLOCK_RANGE]Count (4 bytes):  Count of consecutive adjacent blocks in that range, including
                the block at the Index location.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R28");

            // Verify MS-PCCRR requirement: MS-PCCRR_R28
            bool isVerifyR28 = blockRange.Count > 0;

            site.CaptureRequirementIfIsTrue(
                isVerifyR28,
                28,
                @"[In BLOCK_RANGE][Count (4 bytes)]The value of this field MUST be greater than 0.");

            // If R26 and R27 is verified successfully, means this RS can be verified directly.
            site.CaptureRequirement(
                25,
                @"[In BLOCK_RANGE]A BLOCK_RANGE is an array of two integers that defines 
                a consecutive array of blocks.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R29");

            // Verify MS-PCCRR requirement: MS-PCCRR_R29
            bool isVerifyR29 = blockRange.Index >= 0x00000000 && blockRange.Index <= 0xFFFFFFFF;

            site.CaptureRequirementIfIsTrue(
                isVerifyR29,
                29,
                @"[In BLOCK_RANGE]Index is integer fields in the range of 0x00000000 to 0xFFFFFFFF.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R2900");

            // Verify MS-PCCRR requirement: MS-PCCRR_R2900
            bool isVerifyR2900 = blockRange.Count >= 0x00000000 && blockRange.Count <= 0xFFFFFFFF;

            site.CaptureRequirementIfIsTrue(
                isVerifyR2900,
                2900,
                @"[In BLOCK_RANGE]Count is integer fields in the range of 0x00000000 to 0xFFFFFFFF.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R30");

            // Verify MS-PCCRR requirement: MS-PCCRR_R30
            bool isVerifyR30 = blockRange.Index >= 0 && blockRange.Index <= 511;

            site.CaptureRequirementIfIsTrue(
                isVerifyR30,
                30,
                @"[In BLOCK_RANGE][Index field]contain a value in the range from 0 to 511 (inclusive) for
                the Index field.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R31");

            // Verify MS-PCCRR requirement: MS-PCCRR_R31
            bool isVerifyR31 = blockRange.Count >= 1 && blockRange.Count <= 511;

            site.CaptureRequirementIfIsTrue(
                isVerifyR31,
                31,
                @"[In BLOCK_RANGE][Count field contains a value in the range from]1 to 511–Index (inclusive)
                for the Count field.");
        }

        #endregion Capture requirements related to BLOCK_RANGE structure

        #region Capture requirements related to message

        /// <summary>
        /// Capture message releated requirements.
        /// </summary>
        public static void CaptureMessageRequirements()
        {
            // MS-PCCRR_R37 and MS-PCCRR_R65 have been verified it successfully, so capture it directly.
            site.CaptureRequirement(
                21,
                @"[In Message Syntax]The valid range of the total message size MUST be from 16 bytes
                to 98,304 bytes (or 96 KB).");
        }

        #endregion Capture requirements related to message

        #region Capture requirements related to MESSAGE_HEADER structure

        /// <summary>
        /// Capture MESSAGE_HEADER structure releated requirements.
        /// </summary>
        /// <param name="messageHeader">Message header object</param>
        /// <param name="uiPayload">The length of http content body.</param>
        public static void CaptureMessageHeaderRequirements(MESSAGE_HEADER messageHeader, int uiPayload)
        {
            // The protocol version of the server-peer, which is configured in ptfconfig file.
            ushort uiMajor = ushort.Parse(
                site.Properties.Get(
                "PCCRR.Protocol.MSG_NEGO_REQ.MinSupportedProtocolVersion.MajorVer"));
            ushort uiMinor = ushort.Parse(
                site.Properties.Get(
                "PCCRR.Protocol.MSG_NEGO_REQ.MinSupportedProtocolVersion.MinorVer"));

            ProtoVersion protVer = messageHeader.ProtVer;

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R47");

            // Verify MS-PCCRR requirement: MS-PCCRR_R47
            // If the following types are verified, means those fields are the layout of message header.
            bool isVerifyR47 = messageHeader.GetType() == typeof(MESSAGE_HEADER) &&
                messageHeader.ProtVer.GetType() == typeof(ProtoVersion) &&
                messageHeader.MsgType.GetType() == typeof(MsgType_Values) &&
                messageHeader.MsgSize.GetType() == typeof(uint) &&
                messageHeader.CryptoAlgoId.GetType() == typeof(CryptoAlgoId_Values);

            site.CaptureRequirementIfIsTrue(
                isVerifyR47,
                47,
                @"[In MESSAGE_HEADER]The layout of the message header is as follows
                [ProtVer,MsgType,MsgSize,CryptoAlgoId].");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R48");

            bool isVerifyR48 = messageHeader.ProtVer.GetType() == typeof(ProtoVersion) &&
                messageHeader.ProtVer.MajorVersion.GetType() == typeof(ushort) &&
                messageHeader.ProtVer.MinorVersion.GetType() == typeof(ushort);

            // Verify MS-PCCRR requirement: MS-PCCRR_R48
            site.CaptureRequirementIfIsTrue(
                isVerifyR48,
                48,
                @"[MESSAGE_HEADER]ProtVer (4 bytes):  Protocol version number, formed by concatenating the
                protocol major version number and protocol minor version number, encoded as follows (where MSB
                is Most Significant Byte and LSB is Least Significant Byte): 1st Byte (Addr: X) is Minor version
                MSB;2nd Byte (Addr: X+1) is Minor version LSB;3rd Byte (Addr: X+2) is Major version MSB;4th Byte
                (Addr: X+3) is Major version LSB.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R49");

            // Verify MS-PCCRR requirement: MS-PCCRR_R49
            // The protVer field will be set in HttpTransport_Receive method when receive a response message
            // Verify its low byte
            site.CaptureRequirementIfAreEqual<ushort>(
                uiMajor,
                protVer.MajorVersion,
                49,
                @"[In MESSAGE_HEADER][ProtVer (4 bytes)]The major version number is encoded in the least
                significant word of the protocol version's DWORD.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R50");

            // Verify MS-PCCRR requirement: MS-PCCRR_R50
            // Verify its high byte
            site.CaptureRequirementIfAreEqual<ushort>(
                uiMinor,
                protVer.MinorVersion,
                50,
                @"[In MESSAGE_HEADER][ProtVer (4 bytes)]The minor version number is encoded in the most significant
                word of the protocol version's DWORD.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R51");

            // Verify MS-PCCRR requirement: MS-PCCRR_R51
            // This is a bug, Major  version number is 2 bytes.
            bool isVerifyR51 = uiMajor >= 0x0000 && uiMajor <= 0xFFFF;

            site.CaptureRequirementIfIsTrue(
                isVerifyR51,
                51,
                @"[In MESSAGE_HEADER][ProtVer (4 bytes)]Major  version number can express the version range of
                0x00000000 to 0xFFFFFFFF.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R5100");

            // Verify MS-PCCRR requirement: MS-PCCRR_R5100
            // This is a bug, Minor version number is 2 bytes.
            bool isVerifyR5100 = uiMinor >= 0x0000 && uiMinor <= 0xFFFF;

            site.CaptureRequirementIfIsTrue(
                isVerifyR5100,
                5100,
                @"[In MESSAGE_HEADER][ProtVer (4 bytes)]Minor version number can express the version range of
                0x00000000 to 0xFFFFFFFF.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R52");

            // Verify MS-PCCRR requirement: MS-PCCRR_R52
            bool isVerifyR52 = ((uint)protVer.MajorVersion & 0x0000FFFF) == 0x00000001 &&
                ((uint)protVer.MinorVersion & 0xFFFF0000) == 0x00000000;

            site.CaptureRequirementIfIsTrue(
                isVerifyR52,
                52,
                @"[In MESSAGE_HEADER][ProtVer (4 bytes)]Currently, the protocol[MS-PCCRR] version number MUST be
                set to {major=1 (0x0001), minor=0 (0x0000)}.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R54");

            // Verify MS-PCCRR requirement: MS-PCCRR_R54
            bool isVerifyR54 = messageHeader.MsgType == MsgType_Values.MSG_NEGO_REQ ||
                messageHeader.MsgType == MsgType_Values.MSG_NEGO_RESP ||
                messageHeader.MsgType == MsgType_Values.MSG_GETBLKLIST ||
                messageHeader.MsgType == MsgType_Values.MSG_GETBLKS ||
                messageHeader.MsgType == MsgType_Values.MSG_BLKLIST ||
                messageHeader.MsgType == MsgType_Values.MSG_BLK;

            site.CaptureRequirementIfIsTrue(
                isVerifyR54,
                54,
                @"[In MESSAGE_HEADER][MsgType (4 bytes)]MUST be set to one of the following values
                [MSG_NEGO_REQ 0x00000000,MSG_NEGO_RESP 0x00000001,MSG_GETBLKLIST 0x00000002,MSG_GETBLKS 0x00000003,
                MSG_BLKLIST0x00000004,MSG_BLK 0x00000005].");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R318");

            // Verify MS-PCCRR requirement: MS-PCCRR_R318
            bool isVerifyR318 = messageHeader.CryptoAlgoId == CryptoAlgoId_Values.V1 ||
                messageHeader.CryptoAlgoId == CryptoAlgoId_Values.AES_128 ||
                messageHeader.CryptoAlgoId == CryptoAlgoId_Values.AES_192 ||
                messageHeader.CryptoAlgoId == CryptoAlgoId_Values.AES_256;

            site.CaptureRequirementIfIsTrue(
                isVerifyR318,
                318,
                @"[In Appendix A: Product Behavior]<3> Section 2.2.3: Windows supports: no encryption, AES_128,
                    AES_ 192, and AES_256.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R64");

            // Verify MS-PCCRR requirement: MS-PCCRR_R64
            site.CaptureRequirementIfAreEqual<uint>(
                (uint)uiPayload,
                 messageHeader.MsgSize,
                64,
                @"[In MESSAGE_HEADER]MsgSize (4 bytes):  Protocol message total size including MESSAGE_HEADER,
                but not including the Transport Header.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R65");

            // Verify MS-PCCRR requirement: MS-PCCRR_R65
            bool isVerifyR65 = messageHeader.MsgSize >= 16 && messageHeader.MsgSize <= 98304;

            site.CaptureRequirementIfIsTrue(
                isVerifyR65,
                65,
                @"[In MESSAGE_HEADER][MsgSize (4 bytes)]The valid range of the total message size MUST be from
                16 bytes to 98,304 bytes (or 96 KB).");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R66");

            // Verify MS-PCCRR requirement: MS-PCCRR_R66
            site.CaptureRequirementIfAreEqual<int>(
                4,
                sizeof(CryptoAlgoId_Values),
                66,
                @"[In MESSAGE_HEADER]CryptoAlgoId (4 bytes):  Encryption algorithm used by the server-role peer
                to encrypt data.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R317");

            if (messageHeader.MsgType == MsgType_Values.MSG_GETBLKS)
            {
                bool isVerifyR317 = messageHeader.CryptoAlgoId == CryptoAlgoId_Values.AES_128;

                // Verify MS-PCCRR requirement: MS-PCCRR_R317
                site.CaptureRequirementIfIsTrue(
                    isVerifyR317,
                    317,
                    @"[In Appendix A: Product Behavior]<3> Section 2.2.3: Windows uses AES_128 as the default encryption
                    algorithm.");

                // Verify MS-PCCRR requirement: MS-PCCRR_R19
                bool isFourBytesAlign = messageHeader.MsgSize % 4 == 0;
                CaptureFieldAlignRequirements(isFourBytesAlign);
            }
        }

        #endregion Capture requirements related to MESSAGE_HEADER structure

        #region Capture requirements related to segment id

        /// <summary>
        /// Capture segment id related requirements.
        /// </summary>
        /// <param name="segmentId">The bytes.</param>
        public static void CaptureSegmentIdRequirements(byte[] segmentId)
        {
            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R326");

            // Verify MS-PCCRR requirement: MS-PCCRR_R326
            site.CaptureRequirementIfAreEqual<int>(
                32,
                (int)segmentId.Length,
                326,
                @"[In Appendix A: Product Behavior]<6> Section 2.2.4.3: By default, Windows implementations use 
                    SHA-256 as the hashing algorithm to generate the SegmentID, which corresponds to a SegmentID 
                    length of 32 bytes.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R319");

            site.CaptureRequirementIfAreEqual<int>(
                32,
                (int)segmentId.Length,
                319,
                @"[In Appendix A: Product Behavior]<4> Section 2.2.4.2: By default, Windows implementations use
                    SHA-256 as the hashing algorithm to generate the SegmentID, which corresponds to a SegmentID
                    length of 32 bytes.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R328");

            site.CaptureRequirementIfAreEqual<int>(
                32,
                (int)segmentId.Length,
                328,
                @"[In Appendix A: Product Behavior]<6> Section 2.2.4.3: the Windows implementation of the Retrieval
                    Protocol only supports SegmentIDs generated using SHA-256.");

            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R321");

            site.CaptureRequirementIfAreEqual<int>(
                32,
                (int)segmentId.Length,
                321,
                @"[In Appendix A: Product Behavior]<4> Section 2.2.4.2: the Windows implementation of the
                    Retrieval Protocol only supports SegmentIDs generated using SHA-256.");
        }

        #endregion Capture requirements related to segment id

        #region Capture requirements related to CommonDataTypes structure

        /// <summary>
        /// Capture Common Data Types structure requirements.
        /// </summary>
        /// <param name="obj">The object.</param>
        public static void CaptureCommonDataTypesRequirements(object obj)
        {
            if (obj is MSG_GETBLKLIST)
            {
                MSG_GETBLKLIST getBlkList = (MSG_GETBLKLIST)obj;

                //
                // Add the debug information
                //
                site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R22");

                // Verify MS-PCCRR requirement: MS-PCCRR_R22
                bool isVerifyR22 = getBlkList.NeededBlocksRangeCount.GetType() == typeof(uint);

                site.CaptureRequirementIfIsTrue(
                    isVerifyR22,
                    22,
                    @"[In Common Data Types]The protocol supports three field types:
                  Integer (DWORD fields as defined in [MS-DTYP] section 2.2.9, transmitted in network byte order).");

                //
                // Add the debug information
                //
                site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R23");

                // Verify MS-PCCRR requirement: MS-PCCRR_R23
                bool isVerifyR23 = getBlkList.NeededBlockRanges.GetType() == typeof(BLOCK_RANGE[]);

                site.CaptureRequirementIfIsTrue(
                    isVerifyR23,
                    23,
                    @"[In Common Data Types][The protocol supports three field types]BLOCK_RANGE_ARRAY ( 
                (Integer [2])[count], i.e. a count-sized array of BLOCK_RANGE fields).");

                //
                // Add the debug information
                //
                site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R24");

                // Verify MS-PCCRR requirement: MS-PCCRR_R24
                bool isVerifyR24 = getBlkList.SegmentID.GetType() == typeof(byte[]);

                site.CaptureRequirementIfIsTrue(
                    isVerifyR24,
                    24,
                    @"[In Common Data Types][The protocol supports three field types]BYTE array (BYTE[count], 
                i.e. a count-sized array of bytes).");

                //
                // Add the debug information
                //
                site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R33");

                // Verify MS-PCCRR requirement: MS-PCCRR_R33
                // if server has any of the requested block ranges. then getBlkList.NeededBlockRanges > 0,
                // BlockRanges is an array of BLOCK_RANGE to store BLOCK_RANGE entries.
                // If the lenght is larger than 0, means it contains BLOCK_RANGE entries.
                bool isVerifyR33 = getBlkList.NeededBlockRanges.Length > 0;

                site.CaptureRequirementIfIsTrue(
                    isVerifyR33,
                    33,
                    @"[In BLOCK_RANGE_ARRAY]Variable-size array containing BLOCK_RANGE entries.");

                //
                // Add the debug information
                //
                site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R34");

                // Verify MS-PCCRR requirement: MS-PCCRR_R34
                // If the type is BLOCK_RANGE[], means that it is declared as follows: typedef BLOCK_RANGE BLOCK_RANGE_ARRAY[].
                bool isVerifyR34 = getBlkList.NeededBlockRanges.GetType() == typeof(BLOCK_RANGE[]);

                site.CaptureRequirementIfIsTrue(
                    isVerifyR34,
                    34,
                    @"[In BLOCK_RANGE_ARRAY]This type[BLOCK_RANGE_ARRAY] is declared as follows:
                 typedef BLOCK_RANGE BLOCK_RANGE_ARRAY[];");
            }
            else if (obj is MSG_BLKLIST)
            {
                MSG_BLKLIST blkList = (MSG_BLKLIST)obj;

                //
                // Add the debug information
                //
                site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R22");

                // Verify MS-PCCRR requirement: MS-PCCRR_R22
                bool isVerifyR22 = blkList.BlockRangeCount.GetType() == typeof(uint);

                site.CaptureRequirementIfIsTrue(
                    isVerifyR22,
                    22,
                    @"[In Common Data Types]The protocol supports three field types:Integer (DWORD fields as defined 
                in [MS-DTYP] section 2.2.9, transmitted in network byte order).");

                //
                // Add the debug information
                //
                site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R23");

                // Verify MS-PCCRR requirement: MS-PCCRR_R23
                bool isVerifyR23 = blkList.BlockRanges.GetType() == typeof(BLOCK_RANGE[]);

                site.CaptureRequirementIfIsTrue(
                    isVerifyR23,
                    23,
                    @"[In Common Data Types][The protocol supports three field types]BLOCK_RANGE_ARRAY ( (Integer [2])[count], 
                i.e. a count-sized array of BLOCK_RANGE fields).");

                //
                // Add the debug information
                //
                site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R24");

                // Verify MS-PCCRR requirement: MS-PCCRR_R24
                bool isVerifyR24 = blkList.ZeroPad.GetType() == typeof(byte[]);

                site.CaptureRequirementIfIsTrue(
                    isVerifyR24,
                    24,
                    @"[In Common Data Types][The protocol supports three field types]BYTE array (BYTE[count], i.e. 
                a count-sized array of bytes).");

                //
                // Add the debug information
                //
                site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R33");

                // Verify MS-PCCRR requirement: MS-PCCRR_R33
                // if server has any of the requested block ranges. then blkList.BlockRanges.Length  > 0,
                // BlockRanges is an array of BLOCK_RANGE to store BLOCK_RANGE entries.
                // If the lenght is larger than 0, means it contains BLOCK_RANGE entries.
                bool isVerifyR33 = blkList.BlockRanges.Length >= 0;

                site.CaptureRequirementIfIsTrue(
                    isVerifyR33,
                    33,
                    @"[In BLOCK_RANGE_ARRAY]Variable-size array containing BLOCK_RANGE entries.");

                //
                // Add the debug information
                //
                site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R34");

                // Verify MS-PCCRR requirement: MS-PCCRR_R34
                // If the type is BLOCK_RANGE[], means that it is declared as follows: typedef BLOCK_RANGE BLOCK_RANGE_ARRAY[].
                bool isVerifyR34 = blkList.BlockRanges.GetType() == typeof(BLOCK_RANGE[]);

                site.CaptureRequirementIfIsTrue(
                    isVerifyR34,
                    34,
                    @"[In BLOCK_RANGE_ARRAY]This type[BLOCK_RANGE_ARRAY] is declared as follows:typedef 
                BLOCK_RANGE BLOCK_RANGE_ARRAY[];");
            }
        }

        #endregion Capture requirements related to CommonDataTypes structure

        #region Capture requirements related to whether the field in message is aligned

        /// <summary>
        /// Capture whether the field in message is aligned.
        /// </summary>
        /// <param name="isFourBytesAlign">is FourBytesAlign</param>
        public static void CaptureFieldAlignRequirements(bool isFourBytesAlign)
        {
            //
            // Add the debug information
            //
            site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R19");

            // Verify MS-PCCRR requirement: MS-PCCRR_R19
            bool isVerifyR19 = isFourBytesAlign;

            site.CaptureRequirementIfIsTrue(
                isVerifyR19,
                19,
                @"[In Message Syntax]Each field is aligned according to the current protocol version’s 
                default alignment, currently 4 bytes.");
        }

        #endregion Capture requirements related to whether the field in message is aligned

        #endregion
    }
}

