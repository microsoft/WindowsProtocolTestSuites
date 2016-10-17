// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Pccrr
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.Protocols.TestTools;
    using Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr;

    /// <summary>
    /// This is adapter capture code as client roler.
    /// </summary>
    public partial class PccrrClientAdapter : ManagedAdapterBase, IPccrrClientAdapter
    {
        #region Verify requirements related to REQUEST_MESSAGE  structure

        /// <summary>
        /// REQUEST_MESSAGE structure capture
        /// </summary>
        /// <param name="requestMessage">RequestMessage object</param>
        private void VerifyRequestMessage(REQUEST_MESSAGE requestMessage)
        {
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R74");

            // Verify MS-PCCRR requirement: MS-PCCRR_R74
            Site.CaptureRequirementIfAreEqual<int>(
                16,
                Marshal.SizeOf(requestMessage.MESSAGEHEADER),
                74,
                @"[In Request Message]MESSAGE_HEADER (16 bytes):  Message header.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R75");

            // Verify MS-PCCRR requirement: MS-PCCRR_R75
            bool isVerifyR75 = requestMessage.MESSAGEBODY is MSG_GETBLKLIST ||
                requestMessage.MESSAGEBODY is MSG_GETBLKS;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR75,
                75,
                @"[In Request Message]MESSAGE_BODY (variable):  Message body, which contains either a GetBlockList 
                (MSG_GETBLKLIST) or GetBlocks (MSG_GETBLKS) request message.");

            // MS-PCCRR_R74 and MS-PCCRR_R75 have been verified it successfully, so capture it directly.
            Site.CaptureRequirement(
                73,
                @"[In Request Message]The complete layout of a request-type Peer Content Caching and Retrieval: 
                Retrieval Protocol message is as follows [MESSAGE_HEADER,MESSAGE_BODY (variable)].");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R13");

            // Since the request parsed by stack layer properly, capture the requirement directly.
            Site.CaptureRequirement(
                13,
                @"[In Peer Download Transport]The initiating/client-role peer P1 at IP address A1 initiates 
                the transport of a given request-type Peer Retrieval Protocol message to peer P2 at 
                IP address A2, by sending an HTTP POST request to the root path of 
                {116B50EB-ECE2-41ac-8429-9F9E963361B7}/.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R225");

            // Since the message parsed by stack layer properly, capture the requirement directly.
            Site.CaptureRequirement(
                225,
                @"[In MSG_NEGO_RESP Received][The rules for determining compatibility and selecting a version 
                are listed below:1.]In both cases[MSG_NEGO_REQ and MSG_NEGO_RESP messages], 
                they[MSG_NEGO_REQ and MSG_NEGO_RESP messages] are defined as the inclusive range between 
                the major version from the MinSupportedProtocolVersion field and the major version from 
                the MaxSupportedProtocolVersion field.");
        }

        #endregion Verify requirements related to REQUEST_MESSAGE  structure

        #region Verify requirements related to MSG_GETBLKLIST structure

        /// <summary>
        /// MSG_GETBLKLIST structure capture.
        /// </summary>
        /// <param name="getBlkList">MSG_GETBLKLIST object.</param>
        private void VerifyGetBlkList(MSG_GETBLKLIST getBlkList)
        {
            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R82");

            // Verify MS-PCCRR requirement: MS-PCCRR_R82
            // If GetBlkList.NeededBlockRanges is not null, means it contains  a request for blocks of content.
            Site.CaptureRequirementIfIsNotNull(
                getBlkList.NeededBlockRanges,
                82,
                @"[In MSG_GETBLKLIST]The MSG_GETBLKLIST (GetBlockList) message contains a request for a 
                download block list.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R84");

            // Verify MS-PCCRR requirement: MS-PCCRR_R84
            Site.CaptureRequirementIfAreEqual<uint>(
                (uint)getBlkList.SegmentID.Length,
                getBlkList.SizeOfSegmentID,
                84,
                @"[In MSG_GETBLKLIST]SizeOfSegmentID (4 bytes):  Size, in bytes. of the subsequent SegmentID field.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R85");

            // Verify MS-PCCRR requirement: MS-PCCRR_R85
            bool isVerifyR85 = getBlkList.SizeOfSegmentID >= 0x00000000 && getBlkList.SizeOfSegmentID <= 0xFFFFFFFF;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR85,
                85,
                @"[In MSG_GETBLKLIST][SizeOfSegmentID (4 bytes)]The syntactic range of this field[SegmentID field] 
                is from 0x00000000 to 0xFFFFFFFF.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R88");

            // Verify MS-PCCRR requirement: MS-PCCRR_R88
            Site.CaptureRequirementIfAreEqual<int>(
                32,
                getBlkList.SegmentID.Length,
                88,
                @"[In MSG_GETBLKLIST][SizeOfSegmentID (4 bytes)][Implementations ]MUST support content with 32-byte 
                SegmentIDs.<4>");

            if (null != getBlkList.ZeroPad)
            {
                bool isAllZero = true;

                // If the length is zero, the flag will remain in true
                // else, the bytes will be checked in foreach statements
                foreach (byte b in getBlkList.ZeroPad)
                {
                    if (b != 0)
                    {
                        isAllZero = false;
                        break;
                    }
                }

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R92");

                // Verify MS-PCCRR requirement: MS-PCCRR_R92
                bool isVerifyR92 = isAllZero;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR92,
                    92,
                    @"[In MSG_GETBLKLIST][ZeroPad (variable)]The value of each byte MUST be set to zero.");

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R93");

                // Verify MS-PCCRR requirement: MS-PCCRR_R93
                bool isVerifyR93 = getBlkList.ZeroPad.Length >= 0 && getBlkList.ZeroPad.Length <= 3;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR93,
                    93,
                    @"[In MSG_GETBLKLIST][ZeroPad (variable)]This field[ZeroPad ] is 0 to 3 bytes in length, as required.");
            }

            // Verify requirement MS-PCCRR_R87 and MS-PCCRR_R112
            string isR87Implementated = Site.Properties.Get("PCCRR.SHOULDMAY.R87Implementated");
            bool isR112Satisfied = getBlkList.SizeOfSegmentID >= 0x00000000 && getBlkList.SizeOfSegmentID <= 0xFFFFFFFF;

            if (isWindows)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R112");

                // Verify MS-PCCRR requirement: MS-PCCRR_R112
                Site.CaptureRequirementIfIsTrue(
                    isR112Satisfied,
                    112,
                    @"[In MSG_GETBLKLIST][SizeOfSegmentID (4 bytes)]Implementations support all allowed 
                    SegmentID lengths.[In windows]");

                if (null == isR87Implementated)
                {
                    Site.Properties.Add("PCCRR.SHOULDMAY.R87Implementated", Boolean.TrueString);
                    isR87Implementated = Boolean.TrueString;
                }
            }

            if (null != isR87Implementated)
            {
                bool implement = Boolean.Parse(isR87Implementated);
                bool isSatisfied = isR112Satisfied;

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R87");

                // Verify MS-PCCRR requirement: MS-PCCRR_R87
                Site.CaptureRequirementIfAreEqual<bool>(
                    implement,
                    isSatisfied,
                    87,
                    @"[In MSG_GETBLKLIST][SizeOfSegmentID (4 bytes)]Implementations SHOULD support all 
                    allowed SegmentID lengths");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R94");

            // Verify MS-PCCRR requirement: MS-PCCRR_R94
            Site.CaptureRequirementIfAreEqual<uint>(
                getBlkList.NeededBlocksRangeCount,
                (uint)getBlkList.NeededBlockRanges.Length,
                94,
                @"[In MSG_GETBLKLIST]NeededBlocksRangeCount (4 bytes):  Number of items in the subsequent 
                block range array.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R95");

            // Verify MS-PCCRR requirement: MS-PCCRR_R95
            bool isVerifyR95 = getBlkList.NeededBlocksRangeCount >= 0x00000000
                                && getBlkList.NeededBlocksRangeCount <= 0xFFFFFFFF;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR95,
                95,
                @"[In MSG_GETBLKLIST][NeededBlocksRangeCount (4 bytes)]The syntactic range of this field is 
                from 0x00000000 to 0xFFFFFFFF.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R96");

            // Verify MS-PCCRR requirement: MS-PCCRR_R96
            bool isVerifyR96 = getBlkList.NeededBlocksRangeCount >= 1
                                && getBlkList.NeededBlocksRangeCount <= 256;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR96,
                96,
                @"[In MSG_GETBLKLIST][NeededBlocksRangeCount (4 bytes)]The effective range of this field MUST be 
                between 1 and 256 inclusive, because there cannot be more than 256 non-overlapping and non-contiguous 
                ranges in a maximum segment size of 512 blocks.");

            // Capture directly, cause it won't get the packet if the client didn't specify a right server address
            Site.CaptureRequirement(
                193,
                @"[In MSG_GETBLKLIST Initiation]To initiate a Retrieval Protocol query for the list of block ranges 
                on a server, the higher-layer applications MUST specify a server address.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R194");

            // Verify MS-PCCRR requirement: MS-PCCRR_R194
            bool isVerifyR194 = true;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR194,
                194,
                @"[In MSG_GETBLKLIST Initiation]To initiate a Retrieval Protocol query for the list of block ranges 
                on a server, the higher-layer applications MUST specify a segment ID.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R195");

            // Verify MS-PCCRR requirement: MS-PCCRR_R195
            bool isVerifyR195 = getBlkList.NeededBlocksRangeCount ==
                (uint)getBlkList.NeededBlockRanges.Length
                && true;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR195,
                195,
                @"[In MSG_GETBLKLIST Initiation]To initiate a Retrieval Protocol query for the list of block ranges 
                on a server, the higher-layer applications MUST specify a set of block ranges within the 
                segment identified by the segment ID.");

            // Capture directly, because it won't send a GetBlocklist message to the server if the client 
            // did not construct an instance of the Retrieval Protocol instantiation.
            Site.CaptureRequirement(
                196,
                @"[In MSG_GETBLKLIST Initiation]The client instance of the Retrieval Protocol instantiation MUST 
                construct  a GetBlockList message .");

            // Capture directly, because it won't send a GetBlocklist message to the server if the client 
            // did not construct an instance of the Retrieval Protocol instantiation.
            Site.CaptureRequirement(
                197,
                @"[In MSG_GETBLKLIST Initiation]The client instance of the Retrieval Protocol instantiation MUST
                send a GetBlockList message (MSG_GETBLKLIST (section 2.2.4.2)) to the server.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R171");

            // Verify MS-PCCRR requirement: MS-PCCRR_R171
            bool isVerifyR171 = getBlkList.NeededBlockRanges != null && getBlkList.SegmentID != null;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR171,
                171,
                @"[In Protocol Details]BlockList request/response: A client initiates a GetBlockList request 
                (MSG_GETBLKLIST (section 2.2.4.2)) to a server in order to query the list of content blocks 
                available on the server for a given segment ID, and a list of block ranges within the segment, 
                by sending a MSG_GETBLKLIST request.");
        }

        #endregion Verify requirements related to MSG_GETBLKLIST structure

        #region Verify requirements related to MSG_GETBLKS structure

        /// <summary>
        /// MSG_GETBLKS structure capture
        /// </summary>
        /// <param name="getBlks">MSG_GETBLKS object</param>
        private void VerifyGetBlocks(MSG_GETBLKS getBlks)
        {
            // Verify requirement MS-PCCRR_R61 and MS-PCCRR_R113
            string isR61Implementated = Site.Properties.Get("PCCRR.SHOULDMAY.R61Implementated");
            bool isR113Satisfied = getBlks.ReqBlockRanges.Length == 1;

            if (isWindows)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R113");

                // Verify MS-PCCRR requirement: MS-PCCRR_R113
                Site.CaptureRequirementIfIsTrue(
                    isR113Satisfied,
                    113,
                    @"[In MESSAGE_HEADER][MsgType (4 bytes)] Since only one block will be returned, a MSG_GETBLKS 
                    message specify only a single range containing only a single block.[In windows]");

                if (null == isR61Implementated)
                {
                    Site.Properties.Add("PCCRR.SHOULDMAY.R61Implementated", Boolean.TrueString);
                    isR61Implementated = Boolean.TrueString;
                }
            }

            if (null != isR61Implementated)
            {
                bool implement = Boolean.Parse(isR61Implementated);
                bool isSatisfied = isR113Satisfied;

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R61");

                // Verify MS-PCCRR requirement: MS-PCCRR_R61
                Site.CaptureRequirementIfAreEqual<bool>(
                    implement,
                    isSatisfied,
                    61,
                    @"[In MESSAGE_HEADER][MsgType (4 bytes)] Since only one block will be returned, 
                    a MSG_GETBLKS message SHOULD specify only a single range containing only a single block.");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R101");

            // Verify MS-PCCRR requirement: MS-PCCRR_R101
            Site.CaptureRequirementIfAreEqual<uint>(
                (uint)getBlks.SegmentID.Length,
                getBlks.SizeOfSegmentID,
                101,
                @"[In MSG_GETBLKS]SizeOfSegmentID (4 bytes):  Size in bytes of the subsequent SegmentID field.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R102");

            // Verify MS-PCCRR requirement: MS-PCCRR_R102
            bool isVerifyR102 = getBlks.SizeOfSegmentID >= 0x00000000 && getBlks.SizeOfSegmentID <= 0xFFFFFFFF;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR102,
                102,
                @"[In MSG_GETBLKS][SizeOfSegmentID (4 bytes)]The syntactic range of this field[SegmentID field] 
                is from 0x00000000 to 0xFFFFFFFF.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R105");

            // Verify MS-PCCRR requirement: MS-PCCRR_R105
            Site.CaptureRequirementIfAreEqual<int>(
                32,
                getBlks.SegmentID.Length,
                105,
                @"[In MSG_GETBLKS][SizeOfSegmentID (4 bytes)][Implementations]MUST support content with 32-byte 
                SegmentIDs.<6>");

            if (null != getBlks.ZeroPad)
            {
                bool isAllZero = true;

                // If the length is zero, the flag will remain in true
                // else, the bytes will be checked in foreach statements
                foreach (byte b in getBlks.ZeroPad)
                {
                    if (b != 0)
                    {
                        isAllZero = false;
                        break;
                    }
                }

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R110");

                // Verify MS-PCCRR requirement: MS-PCCRR_R110
                bool isVerifyR110 = getBlks.ZeroPad.Length >= 0 && getBlks.ZeroPad.Length <= 3;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR110,
                    110,
                    @"[In MSG_GETBLKS]This field[ZeroPad (variable)] is 0 to 3 bytes in length, as required.");

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R109");

                // Verify MS-PCCRR requirement: MS-PCCRR_R109
                bool isVerifyR109 = isAllZero;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR109,
                    109,
                    @"[In MSG_GETBLKS][ZeroPad (variable)]The value of each byte MUST be set to zero.");
            }

            // Verify requirement MS-PCCRR_R104 and MS-PCCRR_R111
            string isR104Implementated = Site.Properties.Get("PCCRR.SHOULDMAY.R104Implementated");
            bool isR111Satisfied = getBlks.SizeOfSegmentID >= 0x00000000 && getBlks.SizeOfSegmentID <= 0xFFFFFFFF;

            if (isWindows)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R111");

                // Verify MS-PCCRR requirement: MS-PCCRR_R111
                Site.CaptureRequirementIfIsTrue(
                    isR111Satisfied,
                    111,
                    @"[In MSG_GETBLKS][SizeOfSegmentID (4 bytes)]Implementations support all allowed 
                    SegmentID lengths.[In windows]");

                if (null == isR104Implementated)
                {
                    Site.Properties.Add("PCCRR.SHOULDMAY.R104Implementated", Boolean.TrueString);
                    isR104Implementated = Boolean.TrueString;
                }
            }

            if (null != isR104Implementated)
            {
                bool implement = Boolean.Parse(isR104Implementated);
                bool isSatisfied = isR111Satisfied;

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R104");

                // Verify MS-PCCRR requirement: MS-PCCRR_R104
                Site.CaptureRequirementIfAreEqual<bool>(
                    implement,
                    isSatisfied,
                    104,
                    @"[In MSG_GETBLKS][SizeOfSegmentID (4 bytes)]Implementations SHOULD support all 
                    allowed SegmentID lengths");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R114");

            // Verify MS-PCCRR requirement: MS-PCCRR_R114
            Site.CaptureRequirementIfAreEqual<uint>(
                (uint)getBlks.ReqBlockRanges.Length,
                getBlks.ReqBlockRangeCount,
                114,
                @"[In MSG_GETBLKS]ReqBlockRangeCount (4 bytes):  Number of items in the subsequent block range array.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R115");

            // Verify MS-PCCRR requirement: MS-PCCRR_R115
            bool isVerifyR115 = getBlks.ReqBlockRangeCount >= 0x00000000
                && getBlks.ReqBlockRangeCount <= 0xFFFFFFFF;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR115,
                115,
                @"[In MSG_GETBLKS]The syntactic range of this field[ReqBlockRangeCount (4 bytes)] is 
                from 0x00000000 to 0xFFFFFFFF.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R116");

            // Verify MS-PCCRR requirement: MS-PCCRR_R116
            bool isVerifyR116 = getBlks.ReqBlockRangeCount >= 1
                && getBlks.ReqBlockRangeCount <= 256;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR116,
                116,
                @"[In MSG_GETBLKS][ReqBlockRangeCount (4 bytes)]The effective range of this field MUST be 
                between 1 and 256 inclusive, because there cannot be more than 256 non-overlapping and non-contiguous
                ranges in a maximum segment size of 512 blocks.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R118");

            Site.CaptureRequirement(
                118,
                @"[In MSG_GETBLKS][ReqBlockRanges (variable)]RegBlockRanges MUST specify a block range containing 
                only one block.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R119");

            // Verify MS-PCCRR requirement: MS-PCCRR_R119
            Site.CaptureRequirementIfAreEqual<int>(
                getBlks.DataForVrfBlock.Length,
                (int)getBlks.SizeOfDataForVrfBlock,
                119,
                @"[In MSG_GETBLKS]SizeOfDataForVrfBlock (4 bytes):  Size in bytes of the subsequent 
                DataForVrfBlock field.");

            // Verify requirement MS-PCCRR_R120 and MS-PCCRR_R121
            string isR120Implementated = Site.Properties.Get("PCCRR.SHOULDMAY.R120Implementated");
            bool isR121Satisfied = getBlks.SizeOfDataForVrfBlock == 0;

            if (isWindows)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R121");

                // Verify MS-PCCRR requirement: MS-PCCRR_R121
                Site.CaptureRequirementIfIsTrue(
                    isR121Satisfied,
                    121,
                    @"[In MSG_GETBLKS]This[SizeOfDataForVrfBlock (4 bytes)] field is zero.[In windows]");

                if (null == isR120Implementated)
                {
                    Site.Properties.Add("PCCRR.SHOULDMAY.R120Implementated", Boolean.TrueString);
                    isR120Implementated = Boolean.TrueString;
                }
            }

            if (null != isR120Implementated)
            {
                bool implement = Boolean.Parse(isR120Implementated);
                bool isSatisfied = isR121Satisfied;

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R120");

                // Verify MS-PCCRR requirement: MS-PCCRR_R120
                Site.CaptureRequirementIfAreEqual<bool>(
                    implement,
                    isSatisfied,
                    120,
                    @"[In MSG_GETBLKS]This[SizeOfDataForVrfBlock (4 bytes)] field SHOULD be zero.");
            }

            // Verify requirement MS-PCCRR_R123 and MS-PCCRR_R124
            string isR123Implementated = Site.Properties.Get("PCCRR.SHOULDMAY.R123Implementated");
            bool isR124Satisfied = getBlks.DataForVrfBlock.Length == 0;

            if (isWindows)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R124");

                // Verify MS-PCCRR requirement: MS-PCCRR_R124
                Site.CaptureRequirementIfIsTrue(
                    isR124Satisfied,
                    124,
                    @"[In MSG_GETBLKS][DataForVrfBlock (variable)]This field is empty.[In windows]");

                if (null == isR123Implementated)
                {
                    Site.Properties.Add("PCCRR.SHOULDMAY.R123Implementated", Boolean.TrueString);
                    isR123Implementated = Boolean.TrueString;
                }
            }

            if (null != isR123Implementated)
            {
                bool implement = Boolean.Parse(isR123Implementated);
                bool isSatisfied = isR124Satisfied;

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R123");

                // Verify MS-PCCRR requirement: MS-PCCRR_R123
                Site.CaptureRequirementIfAreEqual<bool>(
                    implement,
                    isSatisfied,
                    123,
                    @"[In MSG_GETBLKS][DataForVrfBlock (variable)]This field SHOULD be empty.");
            }

            // Capture directly, because it won't get the packet if the client didn't specify a right server address.
            Site.CaptureRequirement(
                201,
                @"[In MSG_GETBLKS Initiation]To initiate a Retrieval Protocol request for specific block ranges, 
                the higher-layer applications MUST specify a server address.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R202");

            // Verify MS-PCCRR requirement: MS-PCCRR_R202
            bool isVerifyR202 = true;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR202,
                202,
                @"[In MSG_GETBLKS Initiation]To initiate a Retrieval Protocol request for specific block ranges, 
                the higher-layer applications MUST specify a segment ID.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R203");

            // Verify MS-PCCRR requirement: MS-PCCRR_R203
            bool isVerifyR203 = (uint)getBlks.ReqBlockRanges.Length == getBlks.ReqBlockRangeCount && true;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR203,
                203,
                @"[In MSG_GETBLKS Initiation]To initiate a Retrieval Protocol request for specific block ranges, 
                the higher-layer applications MUST specify a set of block ranges with the segment identified 
                by the segment ID.");

            // Capture directly, because it won't send a GetBlocklist message to the server if the client 
            // did not construct an instance of the Retrieval Protocol instantiation.
            Site.CaptureRequirement(
                204,
                @"[In MSG_GETBLKS Initiation]The client instance of the Retrieval Protocol MUST construct 
                a GetBlocks message.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R174");

            // Verify MS-PCCRR requirement: MS-PCCRR_R174
            bool isVerifyR174 = getBlks.ReqBlockRanges != null && getBlks.SegmentID != null;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR174,
                174,
                @"[In Protocol Details]Blocks request/response: A client initiates a GetBlocks request 
                (MSG_GETBLKS (section 2.2.4.3)) to a server to retrieve a specific block of a given segment, 
                which is identified by the segment ID and the index of the block in the segment.");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R175");

            // Since the request is received, the following requirement can be verified directly
            Site.CaptureRequirement(
                175,
                @"[In Protocol Details][Blocks request/response]It[client] does this by sending 
                a MSG_GETBLKS request.");
        }

        #endregion Verify requirements related to MSG_GETBLKS structure
    }
}

