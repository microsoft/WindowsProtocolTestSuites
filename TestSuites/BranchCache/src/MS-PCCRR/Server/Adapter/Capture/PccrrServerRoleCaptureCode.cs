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
    /// This is adapter capture code as server role.
    /// </summary>
    public partial class PccrrServerAdapter : ManagedAdapterBase, IPccrrServerAdapter
    {
        #region Verify requirements related to HttpResponse

        /// <summary>
        /// Capture properties HttpResponse related requirements.
        /// </summary>
        private void VerifyHttpResponse()
        {
            // Root path
            const string StrPath = "116B50EB-ECE2-41ac-8429-9F9E963361B7";
            const string StrSlash = "/";

            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R12");

            // Verify MS-PCCRR requirement: MS-PCCRR_R12
            Site.CaptureRequirementIfAreEqual<string>(
                StrPath + StrSlash,
                PccrrUri,
                12,
                @"[In Peer Download Transport]Each peer implements the server role by reserving the URL under the root
                path of {116B50EB-ECE2-41ac-8429-9F9E963361B7}/ and listening for POST requests on it.");

            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R270");

            // Since the request is received, the following requirement can be verified directly
            Site.CaptureRequirement(
                270,
                @"[In Initialization]The server is initialized by starting to listen for incoming HTTP requests 
                on the URL specified in section 2.1.1.");
        }

        #endregion Verify requirements related to HttpResponse

        #region Verify requirements related to TRANSPORT_RESPONSE_HEADER structure

        /// <summary>
        /// Capture TRANSPORT_RESPONSE_HEADER structure related requirements.
        /// </summary>
        /// <param name="transportResponseHeader">TRANSPORT_RESPONSE_HEADER object</param>
        private void VerifyTransportResponseHeader(TRANSPORT_RESPONSE_HEADER transportResponseHeader)
        {
            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R9");

            // Verify MS-PCCRR requirement: MS-PCCRR_R9
            Site.CaptureRequirementIfAreEqual<uint>(
                (uint)this.uiPayload,
                transportResponseHeader.Size,
                9,
                @"[In Peer Download Transport]The payload of each such HTTP response consists solely of a single
                Retrieval Protocol message, with the response message prefixed with an additional length field
                (as defined in section 2.2.2) for reassembly purposes.");

            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R36");

            // Verify MS-PCCRR requirement: MS-PCCRR_R36
            Site.CaptureRequirementIfAreEqual<int>(
                4,
                Marshal.SizeOf(transportResponseHeader.Size),
                36,
                @"[In TRANSPORT_RESPONSE_HEADER]Size (4 bytes):  Total message size, in bytes, excluding this field.");

            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R37");

            // Verify MS-PCCRR requirement: MS-PCCRR_R37
            bool isVerifyR37 = transportResponseHeader.Size >= 16 && transportResponseHeader.Size <= 98304;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR37,
                37,
                @"[In TRANSPORT_RESPONSE_HEADER][Size (4 bytes)]The valid range of the total message size MUST be
                from 16 bytes to 98,304 bytes (or 96 KB).");

            // If it can run here, means stack code have been implemented successfully, so can capture it derictly.
            Site.CaptureRequirement(
                35,
                @"[In TRANSPORT_RESPONSE_HEADER]The transport adds the following header in front of response-type
                protocol messages for reassembly purposes:Size.");
        }

        #endregion Verify requirements related to TRANSPORT_RESPONSE_HEADER structure

        #region Verify requirements related to MESSAGE_HEADER structure

        /// <summary>
        /// Capture MESSAGE_HEADER structure related requirements.
        /// </summary>
        /// <param name="messageHeader">Message header object</param>
        private void VerifyMessageHeader(MESSAGE_HEADER messageHeader)
        {
            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R67");

            // Verify MS-PCCRR requirement: MS-PCCRR_R67
            bool isVerifyR67 = messageHeader.CryptoAlgoId == CryptoAlgoId_Values.V1 ||
                messageHeader.CryptoAlgoId == CryptoAlgoId_Values.AES_128 ||
                messageHeader.CryptoAlgoId == CryptoAlgoId_Values.AES_192 ||
                messageHeader.CryptoAlgoId == CryptoAlgoId_Values.AES_256;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR67,
                67,
                @"[In MESSAGE_HEADER][CryptoAlgoId (4 bytes)]MUST be one of the following values[0x00000000,AES_128 
                0x00000001,AES_192 0x00000002,AES_256 0x00000003].<3>");
        }

        #endregion Verify requirements related to MESSAGE_HEADER structure

        #region Verify requirements related to RESPONSE_MESSAGE structure

        /// <summary>
        /// Capture RESPONSE_MESSAGE structure related requirements.
        /// </summary>
        /// <param name="responseMessage">RESPONSE_MESSAGE object</param>
        private void VerifyResponseMessage(RESPONSE_MESSAGE responseMessage)
        {
            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R127");

            // Verify MS-PCCRR requirement: MS-PCCRR_R127
            Site.CaptureRequirementIfAreEqual<int>(
                4,
                Marshal.SizeOf(responseMessage.TRANSPORTRESPONSEHEADER),
                127,
                @"[In Response Message]TRANSPORT_RESPONSE_HEADER (4 bytes):  Transport response header.");

            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R128");

            // Verify MS-PCCRR requirement: MS-PCCRR_R128
            Site.CaptureRequirementIfAreEqual<int>(
                16,
                Marshal.SizeOf(responseMessage.MESSAGEHEADER),
                128,
                @"[In Response Message]MESSAGE_HEADER (16 bytes):  Message header.");

            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R129");

            // Verify MS-PCCRR requirement: MS-PCCRR_R129
            bool isVerifyR129 = responseMessage.MESSAGEBODY is MSG_BLKLIST
                || responseMessage.MESSAGEBODY is MSG_BLK
                || responseMessage.MESSAGEBODY is MSG_NEGO_RESP;

            Site.CaptureRequirementIfIsTrue(
                isVerifyR129,
                129,
                @"[In Response Message]MESSAGE_BODY (variable):  Message body, which may contain either a MSG_BLKLIST 
                or a MSG_BLK message.");

            // MS-PCCRR_R127,MS-PCCRR_R128 and MS-PCCRR_R129 have been verified it successfully, so capture it directly.
            Site.CaptureRequirement(
                126,
                @"[In Response Message]The complete layout of a response-type Peer Content Caching and Retrieval: 
                  Retrieval Protocol message is as follows[TRANSPORT_RESPONSE_HEADER,MESSAGE_HEADER,MESSAGE_BODY 
                  (variable)].");
        }

        #endregion Verify requirements related to RESPONSE_MESSAGE structure

        #region Verify requirements related to MSG_NEGO_RESP structure

        /// <summary>
        /// Capture MSG_NEGO_RESP structure related requirements.
        /// </summary>
        /// <param name="msgNegoResp">The msgNegoResp.</param>
        private void VerifyMsgNegoResp(MSG_NEGO_RESP msgNegoResp)
        {
            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R132");

            bool isVerifyR132 = msgNegoResp.MinSupporteProtocolVersion.GetType() == typeof(ProtoVersion);

            // Verify MS-PCCRR requirement: MS-PCCRR_R132
            Site.CaptureRequirementIfIsTrue(
                isVerifyR132,
                132,
                @"[In MSG_NEGO_RESP]MinSupportedProtocolVersion (4 bytes):  Minimum protocol version supported by 
                the requesting peer.");

            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R134");

            bool isVerifyR134 = msgNegoResp.MaxSupporteProtocolVersion.GetType() == typeof(ProtoVersion);

            // Verify MS-PCCRR requirement: MS-PCCRR_R134
            Site.CaptureRequirementIfIsTrue(
                isVerifyR134,
                134,
                @"[In MSG_NEGO_RESP]MaxSupportedProtocolVersion (4 bytes):  Maximum protocol version supported by 
                the requesting peer.");

            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R131");

            bool isVerifyR131 = msgNegoResp.MinSupporteProtocolVersion.GetType() == typeof(ProtoVersion) &&
                msgNegoResp.MaxSupporteProtocolVersion.GetType() == typeof(ProtoVersion);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR131,
                131,
                @"[In MSG_NEGO_RESP]The message is sent in response to a Negotiation Request message or to 
                any other request message with a protocol version not supported by the server-role peer.");

            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R133");

            bool isVerifyR133 = msgNegoResp.MinSupporteProtocolVersion.GetType() == typeof(ProtoVersion);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR133,
                133,
                @"[In MSG_NEGO_RESP]The protocol version[Minimum protocol version] is encoded identically to 
                the ProtVer field defined in section 2.2.3.");

            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R135");

            bool isVerifyR135 = msgNegoResp.MaxSupporteProtocolVersion.GetType() == typeof(ProtoVersion);

            Site.CaptureRequirementIfIsTrue(
                isVerifyR135,
                135,
                @"[In MSG_NEGO_RESP]The protocol version[Maximum protocol version] is encoded identically to 
                the ProtVer field defined in section 2.2.3.");

            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R130");

            // Since the R133 and R135 have already contains the logic with R130, and R133 and R135 have been verified, 
            // the following requirement can be verified directly
            Site.CaptureRequirement(
                130,
                @"[In MSG_NEGO_RESP]The MSG_NEGO_RESP (Negotiation Response) message is the response message 
                containing the minimum and maximum protocol version supported by the responding server-role peer.");

            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R168");

            bool isVerifyR168 = msgNegoResp.MinSupporteProtocolVersion.GetType() == typeof(ProtoVersion) &&
                msgNegoResp.MaxSupporteProtocolVersion.GetType() == typeof(ProtoVersion);

            // Verify MS-PCCRR requirement: MS-PCCRR_R168
            Site.CaptureRequirementIfIsTrue(
                isVerifyR168,
                168,
                @"[In Protocol Details][Protocol Version Negotiation]The server responds with a Negotiation Response 
                message (MSG_NEGO_RESP (section 2.2.5.1)), declaring the minimum and maximum protocol versions it supports.");
        }

        #endregion Verify requirements related to MSG_NEGO_RESP structure

        #region Verify requirements related to MSG_BLKLIST structure

        /// <summary>
        /// Capture MSG_BLKLIST structure related requirements.
        /// </summary>
        /// <param name="blkList">MSG_BLKLIST object</param>
        private void VerifyMsgBlkList(MSG_BLKLIST blkList)
        {
            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R136");

            // Verify MS-PCCRR requirement: MS-PCCRR_R136
            Site.CaptureRequirementIfAreEqual<uint>(
                (uint)blkList.SegmentId.Length,
                blkList.SizeOfSegmentId,
                136,
                @"[In MSG_BLKLIST]SizeOfSegmentId (4 bytes):  The size, in bytes, of the subsequent SegmentId field.");

            if (null != blkList.ZeroPad)
            {
                bool isAllZero = true;

                // If the length is zero, the flag will remain in true
                // else, the bytes will be checked in foreach statements
                foreach (byte b in blkList.ZeroPad)
                {
                    if (b != 0)
                    {
                        isAllZero = false;
                        break;
                    }
                }

                // Add the debug information
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R140");

                // Verify MS-PCCRR requirement: MS-PCCRR_R140
                bool isVerifyR140 = isAllZero;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR140,
                    140,
                    @"[In MSG_BLKLIST][ZeroPad (variable)]Each byte's value MUST be set to zero.");

                // Add the debug information
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R139");

                // Verify MS-PCCRR requirement: MS-PCCRR_R139
                bool isVerifyR139 = blkList.ZeroPad.Length >= 0 && blkList.ZeroPad.Length <= 3;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR139,
                    139,
                    @"[In MSG_BLKLIST]ZeroPad (variable):  A sequence of N bytes added (only as needed) to restore 
                    4-byte alignment, where 0 <= N <= 3.");

                if (null != blkList.BlockRanges && blkList.BlockRangeCount != 0)
                {
                    // Add the debug information
                    Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R141");

                    // Verify MS-PCCRR requirement: MS-PCCRR_R141
                    Site.CaptureRequirementIfAreEqual<uint>(
                        (uint)blkList.BlockRanges.Length,
                        blkList.BlockRangeCount,
                        141,
                        @"[In MSG_BLKLIST]BlockRangeCount (4 bytes):  Number of items in the subsequent block range array.");
                }
            }
        }

        #endregion Verify requirements related to MSG_BLKLIST structure

        #region Verify requirements related to MSG_BLK structure

        /// <summary>
        /// Capture MSG_BLK structure related requirements.
        /// </summary>
        /// <param name="blk">MSG_BLK object</param>
        private void VerifyMsgBlk(MSG_BLK blk)
        {
            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R146");

            // Verify MS-PCCRR requirement: MS-PCCRR_R146
            Site.CaptureRequirementIfAreEqual<int>(
                blk.SegmentId.Length,
                (int)blk.SizeOfSegmentId,
                146,
                @"[In MSG_BLK]SizeOfSegmentId (4 bytes):  The size, in bytes, of the subsequent SegmentId field.");

            if (null != blk.ZeroPad)
            {
                bool isAllZero = true;

                // If the length is zero, the flag will remain in true
                // else, the bytes will be checked in foreach statements
                foreach (byte b in blk.ZeroPad)
                {
                    if (b != 0)
                    {
                        isAllZero = false;
                        break;
                    }
                }

                // Add the debug information
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R14900");

                // Verify MS-PCCRR requirement: MS-PCCRR_R14900
                bool isVerifyR14900 = isAllZero;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR14900,
                    14900,
                    @"[In MSG_BLK]ZeroPad (variable): Each byte's value MUST be set to zero.");

                // Add the debug information
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R149");

                // Verify MS-PCCRR requirement: MS-PCCRR_R149
                bool isVerifyR149 = blk.ZeroPad.Length >= 0 && blk.ZeroPad.Length <= 3;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR149,
                    149,
                    @"[In MSG_BLK]ZeroPad (variable):  A sequence of N bytes added (only as needed) to restore 
                    4-byte alignment, where 0 <= N <= 3. ");
            }

            // Add the debug information
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R153");

            // Verify MS-PCCRR requirement: MS-PCCRR_R153
            Site.CaptureRequirementIfAreEqual<uint>(
                (uint)blk.Block.Length,
                blk.SizeOfBlock,
                153,
                @"[In MSG_BLK]SizeOfBlock (4 bytes):The size, in bytes, of the subsequent Block field.");

            if (null != blk.ZeroPad2)
            {
                bool isAllZero = true;

                // If the length is zero, the flag will remain in true
                // else, the bytes will be checked in foreach statements
                foreach (byte b in blk.ZeroPad2)
                {
                    if (b != 0)
                    {
                        isAllZero = false;
                        break;
                    }
                }

                // Add the debug information
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R157");

                // Verify MS-PCCRR requirement: MS-PCCRR_R157
                bool isVerifyR157 = isAllZero;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR157,
                    157,
                    @"[In MSG_BLK][ZeroPad_2 (variable)]Each byte's value MUST be set to zero.");

                // Add the debug information
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R156");

                // Verify MS-PCCRR requirement: MS-PCCRR_R156
                bool isVerifyR156 = blk.ZeroPad2.Length >= 0 && blk.ZeroPad2.Length <= 3;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR156,
                    156,
                    @"[In MSG_BLK]ZeroPad_2 (variable):  A sequence of N bytes added (only as needed) to restore 
                    4-byte alignment, where 0 <= N <= 3.");
            }

            // Verify requirement MS-PCCRR_R158 and MS-PCCRR_R159
            string isR158Implementated = Site.Properties.Get("PCCRR.SHOULDMAY.R158Implementated");
            bool isR159Satisfied = blk.SizeOfVrfBlock == 0;

            if (isWindows)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R159");

                // Verify MS-PCCRR requirement: MS-PCCRR_R159
                Site.CaptureRequirementIfIsTrue(
                    isR159Satisfied,
                    159,
                    @"[In MSG_BLK]SizeOfVrfBlock (4 bytes):  The size, in bytes, of the subsequent VrfBlock field, 
                    which is  zero.[In windows]");

                if (null == isR158Implementated)
                {
                    Site.Properties.Add("PCCRR.SHOULDMAY.R158Implementated", Boolean.TrueString);
                    isR158Implementated = Boolean.TrueString;
                }
            }

            if (null != isR158Implementated)
            {
                bool implement = Boolean.Parse(isR158Implementated);
                bool isSatisfied = isR159Satisfied;

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R158");

                //
                // Verify MS-PCCRR requirement: MS-PCCRR_R158
                //
                Site.CaptureRequirementIfAreEqual<bool>(
                    implement,
                    isSatisfied,
                    158,
                    @"[In MSG_BLK]SizeOfVrfBlock (4 bytes):  The size, in bytes, of the subsequent VrfBlock field, 
                    which SHOULD be zero.");
            }

            // Verify requirement MS-PCCRR_R161 and MS-PCCRR_R162
            string isR161Implementated = Site.Properties.Get("PCCRR.SHOULDMAY.R161Implementated");
            bool isR162Satisfied = blk.VrfBlock.Length == 0;

            if (isWindows)
            {
                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R162");

                // Verify MS-PCCRR requirement: MS-PCCRR_R162
                Site.CaptureRequirementIfIsTrue(
                    isR162Satisfied,
                    162,
                    @"[In MSG_BLK]VrfBlock (variable):  is empty.[In windows]");

                if (null == isR161Implementated)
                {
                    Site.Properties.Add("PCCRR.SHOULDMAY.R161Implementated", Boolean.TrueString);
                    isR161Implementated = Boolean.TrueString;
                }
            }

            if (null != isR161Implementated)
            {
                bool implement = Boolean.Parse(isR161Implementated);
                bool isSatisfied = isR162Satisfied;

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R161");

                // Verify MS-PCCRR requirement: MS-PCCRR_R161
                Site.CaptureRequirementIfAreEqual<bool>(
                    implement,
                    isSatisfied,
                    161,
                    @"[In MSG_BLK]VrfBlock (variable):  SHOULD be empty.");
            }

            if (null != blk.ZeroPad3)
            {
                bool isAllZero = true;

                // If the length is zero, the flag will remain in true
                // else, the bytes will be checked in foreach statements
                foreach (byte b in blk.ZeroPad3)
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
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R16300");

                // Verify MS-PCCRR requirement: MS-PCCRR_R16300
                bool isVerifyR16300 = isAllZero;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR16300,
                    16300,
                    @"[In MSG_BLK]ZeroPad_3 (variable): Each byte's value MUST be set to zero.");

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R163");

                // Verify MS-PCCRR requirement: MS-PCCRR_R163
                bool isVerifyR163 = blk.ZeroPad3.Length >= 0 && blk.ZeroPad3.Length <= 3;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR163,
                    163,
                    @"[In MSG_BLK]ZeroPad_3 (variable):  A sequence of N bytes added (only as needed) to restore 
                    4-byte alignment, where 0 <= N <= 3. ");

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R164");

                // Verify MS-PCCRR requirement: MS-PCCRR_R164
                bool isVerifyR164 = isAllZero;

                Site.CaptureRequirementIfIsTrue(
                    isVerifyR164,
                    164,
                    @"[In MSG_BLK]SizeOfIVBlock (4 bytes):  The size, in bytes, of the subsequent IVBlock field.");

                //
                // Add the debug information
                //
                Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R305");

                // Verify MS-PCCRR requirement: MS-PCCRR_R305
                Site.CaptureRequirement(
                    305,
                    @"[In MSG_GETBLKS Request Received]4.The SegmentID field in the response message MUST be set to
                    the segment ID of the request");
            }

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R147");

            Site.CaptureRequirementIfIsNotNull(
                blk.SegmentId,
                147,
                @"[In MSG_BLK]SegmentId (variable):  The Public Segment Identifier for the target 
                segment of content (also known as HoHoDk).");

            //
            // Add the debug information
            //
            Site.Log.Add(LogEntryKind.Debug, "Verify MS-PCCRR_R155");

            Site.CaptureRequirementIfIsNotNull(
                blk.Block,
                155,
                @"[In MSG_BLK]Block (variable):  The actual block of data, encrypted according to 
                the cryptographic algorithm specified in the header of the message itself, not including 
                the initialization vector.");
        }

        #endregion Verify requirements related to MSG_BLK structure
    }
}

