// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs;
using Microsoft.Protocols.TestTools.ExtendedLogging;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    /// <summary>
    /// MS-RDPBCGR Server Decoder Class
    /// </summary>
    internal class RdpbcgrServerDecoder
    {
        #region Field members
        // RDPBCGR Server
        private RdpbcgrServer server;
        #endregion Private Class Members


        #region Properties
        #endregion Properties


        #region Constructor
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="rdpbcgrServer">The server.</param>
        public RdpbcgrServerDecoder(RdpbcgrServer rdpbcgrServer)
        {
            this.server = rdpbcgrServer;
        }
        #endregion Constructor


        #region Private methods
        #region Private Methods: Helper Functions
        /// <summary>
        /// Get specified length of bytes from a byte array
        /// (start index is updated according to the specified length)
        /// </summary>
        /// <param name="data">data in byte array</param>
        /// <param name="startIndex">start index</param>
        /// <param name="bytesToRead">specified length</param>
        /// <returns>bytes of specified length</returns>
        private byte[] GetBytes(byte[] data, ref int startIndex, int bytesToRead)
        {
            // if input data is null
            if (null == data)
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_DATA_NULL_REF);
            }

            // if index is out of range
            if ((startIndex < 0) || (startIndex + bytesToRead > data.Length))
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_DATA_INDEX_OUT_OF_RANGE);
            }

            // read bytes of specific length
            byte[] dataRead = new byte[bytesToRead];
            Array.Copy(data, startIndex, dataRead, 0, bytesToRead);

            // update index
            startIndex += bytesToRead;
            return dataRead;
        }

        private string GetString(byte[] data, ref int startIndex, int bytesToRead)
        {
            byte[] bytes = GetBytes(data, ref startIndex, bytesToRead);
            return bytes.ToString();
        }


        /// <summary>
        /// Check if the subFlag exists in flag
        /// </summary>
        /// <param name="flag">flag in UInt32</param>
        /// <param name="subFlag">sub flag in UInt32</param>
        /// <returns>exists/not</returns>
        private bool IsFlagExist(UInt32 flag, UInt32 subFlag)
        {
            return ((UInt32)(flag & subFlag) == subFlag);
        }


        /// <summary>
        /// Check if the subFlag exists in flag
        /// </summary>
        /// <param name="flag">flag in Uint16</param>
        /// <param name="subFlag">sub flag in Uint16</param>
        /// <returns>exists/not</returns>
        private bool IsFlagExist(UInt16 flag, UInt16 subFlag)
        {
            return IsFlagExist((UInt32)flag, (UInt32)subFlag);
        }


        /// <summary>
        /// Check if the subFlag exists in flag
        /// </summary>
        /// <param name="flag">flag in Byte</param>
        /// <param name="subFlag">sub flag in Byte</param>
        /// <returns>exists/not</returns>
        private bool IsFlagExist(byte flag, byte subFlag)
        {
            return IsFlagExist((UInt32)flag, (UInt32)subFlag);
        }


        /// <summary>
        /// Get Security Header Type by Server Context
        /// </summary>
        /// <returns>Security Header Type</returns>
        private SecurityHeaderType GetSecurityHeaderTypeByContext(RdpbcgrServerSessionContext serverSessionContext)
        {
            SecurityHeaderType securityHeaderType;
            switch (serverSessionContext.RdpEncryptionLevel)
            {
                case EncryptionLevel.ENCRYPTION_LEVEL_NONE:
                    securityHeaderType = SecurityHeaderType.None;
                    break;

                case EncryptionLevel.ENCRYPTION_LEVEL_LOW:
                //securityHeaderType = SecurityHeaderType.Basic;
                //break;

                case EncryptionLevel.ENCRYPTION_LEVEL_CLIENT_COMPATIBLE:
                case EncryptionLevel.ENCRYPTION_LEVEL_HIGH:
                case EncryptionLevel.ENCRYPTION_LEVEL_FIPS:
                default: //To enable invalid encryption level test.
                    // The following logic is implemented according to actual situation observed,
                    // since related TD section is involved with [TDI #39940]
                    if (serverSessionContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT
                        || serverSessionContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT
                        || serverSessionContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)
                    {
                        securityHeaderType = SecurityHeaderType.NonFips;
                    }
                    else if (serverSessionContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
                    {
                        securityHeaderType = SecurityHeaderType.Fips;
                    }
                    else
                    {
                        securityHeaderType = SecurityHeaderType.None;
                    }
                    break;
            }
            return securityHeaderType;
        }


        /// <summary>
        /// Get Share Control Header Type
        /// [Reference to 2.2.8.1.1.1.1 Share Control Header (TS_SHARECONTROLHEADER)]
        /// </summary>
        /// <param name="header">a TS_SHARECONTROLHEADER</param>
        /// <returns>the Share Control Header Type</returns>
        private ShareControlHeaderType GetShareControlHeaderType(TS_SHARECONTROLHEADER header)
        {
            // Get "type" info (In "typAndVersionLow" field, the lower four bits represents "type")
            int type = header.pduType.typeAndVersionLow & 0x0f;

            // Type cast
            return (ShareControlHeaderType)type;
        }




        /// <summary>
        /// For Standard Redirection PDU type, an encryption is needed. 
        /// So if there's encryptionLevel is ENCRYPTION_LEVEL_LOW, update the securityHeaderType according to TD.
        /// </summary>
        /// <param name="securityHeaderType">security header type</param>
        private void UpdateSecurityHeaderType(RdpbcgrServerSessionContext serverSessionContext, ref SecurityHeaderType securityHeaderType)
        {
            // Redirection PDU need to decrypt
            if (serverSessionContext.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_LOW)
            {
                if (serverSessionContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT
                    || serverSessionContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT
                    || serverSessionContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)
                {
                    securityHeaderType = SecurityHeaderType.NonFips;
                }
                else if (serverSessionContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
                {
                    securityHeaderType = SecurityHeaderType.Fips;
                }
                else
                {
                    securityHeaderType = SecurityHeaderType.None;
                }
            }
        }


        /// <summary>
        /// Decrypt Send Data Request
        /// </summary>
        /// <param name="userData">user data</param>
        /// <param name="securityHeaderType">security header type</param>
        /// <returns>decrypted user data</returns>
        private byte[] DecryptSendDataRequest(RdpbcgrServerSessionContext serverSessionContext, byte[] userData, SecurityHeaderType securityHeaderType)
        {
            // Parse security header
            int index = 0;
            TS_SECURITY_HEADER header = ParseTsSecurityHeader(userData, ref index, securityHeaderType);

            // If header is absent, data is not encrypted, return directly
            if (null == header)
            {
                return userData;
            }

            // Get remain data with security header removed
            int remainLength = userData.Length - index;
            byte[] remainData = GetBytes(userData, ref index, remainLength);

            // Header is present, but still data is not encrypted, return directly
            bool isEncryptFlagExist = IsFlagExist((UInt16)header.flags,
                (UInt16)TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT);
            bool isRedirectFlagExist = IsFlagExist((UInt16)header.flags,
                (UInt16)TS_SECURITY_HEADER_flags_Values.SEC_REDIRECTION_PKT);
            if ((!isEncryptFlagExist) && (!isRedirectFlagExist))
            {
                return remainData;
            }

            // Get data signature (Fips/NonFips only)
            byte[] signature = null;
            switch (securityHeaderType)
            {
                case SecurityHeaderType.NonFips:
                    TS_SECURITY_HEADER1 nonFipsHeader = (TS_SECURITY_HEADER1)header;
                    signature = nonFipsHeader.dataSignature;
                    break;

                case SecurityHeaderType.Fips:
                    TS_SECURITY_HEADER2 fipsHeader = (TS_SECURITY_HEADER2)header;
                    signature = fipsHeader.dataSignature;
                    break;

                case SecurityHeaderType.None:
                    signature = null;
                    break;

                case SecurityHeaderType.Basic:
                    signature = null;
                    break;

                default:
                    throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
            }

            // Check if "Salted MAC Generation" was used in PDU generation       
            bool isSalted = IsFlagExist((UInt16)header.flags,
                (UInt16)TS_SECURITY_HEADER_flags_Values.SEC_SECURE_CHECKSUM);

            // Decryption
            byte[] decryptedData = null;
            if (!serverSessionContext.ServerDecrypt(remainData, signature, isSalted, out decryptedData))
            {
                // If decryption failed
                throw new FormatException(ConstValue.ERROR_MESSAGE_DECRYPTION_FAILED);
            }
            return decryptedData;
        }


        /// <summary>
        /// Verify Packet Data Length
        /// </summary>
        /// <param name="actualLength">actual length</param>
        /// <param name="expectedLength">expected length</param>
        /// <param name="errorMessage">error message</param>
        private void VerifyDataLength(int actualLength, int expectedLength, string errorMessage)
        {
            if (actualLength != expectedLength)
            {
                throw new FormatException(errorMessage);
            }
        }
        #endregion Private Methods: Helper Functions


        #region Private Methods: Base Type Parsers
        /// <summary>
        /// Parse one byte
        /// (parser index will be updated by one)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="index">parser index</param>
        /// <returns>the parsed byte</returns>
        private byte ParseByte(byte[] data, ref int index)
        {
            // if input data is null
            if (null == data)
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_DATA_NULL_REF);
            }

            // if index is out of range
            if (index < 0 || index >= data.Length)
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_DATA_INDEX_OUT_OF_RANGE);
            }

            // get a byte
            byte b = data[index];

            // update parser index
            ++index;

            return b;
        }


        /// <summary>
        /// Parse UInt16
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="index">parser index</param>
        /// <param name="isBigEndian">big endian format flag</param>
        /// <returns>parsed UInt16 number</returns>
        private UInt16 ParseUInt16(byte[] data, ref int index, bool isBigEndian)
        {
            // Read 2 bytes
            byte[] bytes = GetBytes(data, ref index, sizeof(UInt16));

            // Big Endian format requires reversed byte order
            if (isBigEndian)
            {
                Array.Reverse(bytes, 0, sizeof(UInt16));
            }

            // Convert
            return BitConverter.ToUInt16(bytes, 0);
        }


        /// <summary>
        /// Parse UInt32
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="index">parser index</param>
        /// <param name="isBigEndian">big endian format flag</param>
        /// <returns>parsed UInt32 number</returns>
        private UInt32 ParseUInt32(byte[] data, ref int index, bool isBigEndian)
        {
            // Read 4 bytes
            byte[] bytes = GetBytes(data, ref index, sizeof(UInt32));

            // Big Endian format requires reversed byte order
            if (isBigEndian)
            {
                Array.Reverse(bytes, 0, sizeof(UInt32));
            }

            // Convert
            return BitConverter.ToUInt32(bytes, 0);
        }

        private string ParseString(byte[] data, ref int index, int len)
        {
            byte[] bytes = GetBytes(data, ref index, len);
            return bytes.ToString();
        }

        private string ParseUnicodeString(byte[] data, bool isZeroTerminated)
        {
            try
            {
                var result = new StringBuilder(data.Length / 2);

                int currentIndex = 0;

                while (currentIndex < data.Length)
                {
                    UInt16 code = ParseUInt16(data, ref currentIndex, false);
                    char charCode = (char)code;
                    result.Append(charCode);
                }

                if (isZeroTerminated)
                {
                    int index = result.Length - 1;
                    if (result[index] != 0)
                    {
                        throw new Exception();
                    }
                    while (index > 0)
                    {
                        if (result[index - 1] != 0)
                        {
                            break;
                        }
                        index--;
                    }
                    result.Remove(index, result.Length - index);
                }

                return result.ToString();
            }
            catch
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_INVALID_UNICODE_STRING);
            }
        }

        #endregion Private Methods: Base Type Parsers


        #region Private Methods: PDU Sub Field Parsers
        #region Sub Field Parsers: Common Fields
        /// <summary>
        /// Parse TpktHeader
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TpktHeader</returns>
        private TpktHeader ParseTpktHeader(byte[] data, ref int currentIndex)
        {
            TpktHeader header = new TpktHeader();

            // TpktHeader: version
            header.version = ParseByte(data, ref currentIndex);

            // TpktHeader: reserved
            header.reserved = ParseByte(data, ref currentIndex);

            // TpktHeader: length
            header.length = ParseUInt16(data, ref currentIndex, true);

            return header;
        }


        /// <summary>
        /// Parse X224 Data
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>X224 Data</returns>
        private X224 ParseX224Data(byte[] data, ref int currentIndex)
        {
            X224 x224Data = new X224();

            // X224: length
            x224Data.length = ParseByte(data, ref currentIndex);

            // X224: type
            x224Data.type = ParseByte(data, ref currentIndex);

            // X224: eot
            x224Data.eot = ParseByte(data, ref currentIndex);

            return x224Data;
        }


        /// <summary>
        /// Parse MCS Common Header
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <param name="securityHeaderType">security header type</param>
        /// <returns>MCS Common Header</returns>
        private SlowPathPduCommonHeader ParseMcsCommonHeader(
            byte[] data,
            ref int currentIndex,
            SecurityHeaderType securityHeaderType)
        {
            SlowPathPduCommonHeader header = new SlowPathPduCommonHeader();

            // McsCommonHeader: TpktHeader
            header.tpktHeader = ParseTpktHeader(data, ref currentIndex);

            // McsCommonHeader: x224Data
            header.x224Data = ParseX224Data(data, ref currentIndex);

            // McsCommonHeader: type
            header.type = ParseByte(data, ref currentIndex);

            // McsCommonHeader: initiator
            header.initiator = (UInt16)(ParseUInt16(data, ref currentIndex, true)
                + ConstValue.CHANNEL_INITIATOR_FILTER);

            // McsCommonHeader: channelId
            header.channelId = ParseUInt16(data, ref currentIndex, true);

            // McsCommonHeader: level(dataPriority & segmentation)
            header.level = ParseByte(data, ref currentIndex);

            // McsCommonHeader: user data length
            byte length1 = ParseByte(data, ref currentIndex);
            if ((length1 & 0x80) == 0x80)
            {
                byte length2 = ParseByte(data, ref currentIndex);
                header.userDataLength = (uint)(((length1 & 0x7F) << 8) + length2);
            }
            else
            {
                header.userDataLength = length1;
            }

            // McsCommonHeader: securityHeader
            header.securityHeader = ParseTsSecurityHeader(data, ref currentIndex, securityHeaderType);

            return header;
        }


        /// <summary>
        /// Parse TS_SHAREDATAHEADER
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SHAREDATAHEADER</returns>
        private TS_SHAREDATAHEADER ParseTsShareDataHeader(byte[] data, ref int currentIndex)
        {
            TS_SHAREDATAHEADER header = new TS_SHAREDATAHEADER();

            // TS_SHAREDATAHEADER: shareControlHeader
            header.shareControlHeader = ParseTsShareControlHeader(data, ref currentIndex);

            // TS_SHAREDATAHEADER: shareId
            header.shareId = ParseUInt32(data, ref currentIndex, false);

            // TS_SHAREDATAHEADER: pad1
            header.pad1 = ParseByte(data, ref currentIndex);

            // TS_SHAREDATAHEADER: streamId
            header.streamId = (streamId_Values)ParseByte(data, ref currentIndex);

            // TS_SHAREDATAHEADER: uncompressedLength
            header.uncompressedLength = ParseUInt16(data, ref currentIndex, false);

            // TS_SHAREDATAHEADER: pduType2
            header.pduType2 = (pduType2_Values)ParseByte(data, ref currentIndex);

            // TS_SHAREDATAHEADER: compressedType
            header.compressedType = (compressedType_Values)ParseByte(data, ref currentIndex);

            // TS_SHAREDATAHEADER: compressedLength
            header.compressedLength = ParseUInt16(data, ref currentIndex, false);

            return header;
        }


        /// <summary>
        /// Parse TS_SHARECONTROLHEADER
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SHARECONTROLHEADER</returns>
        private TS_SHARECONTROLHEADER ParseTsShareControlHeader(byte[] data, ref int currentIndex)
        {
            TS_SHARECONTROLHEADER header = new TS_SHARECONTROLHEADER();

            // TS_SHARECONTROLHEADER: totalLength
            header.totalLength = ParseUInt16(data, ref currentIndex, false);

            // TS_SHARECONTROLHEADER: pduType
            header.pduType.typeAndVersionLow = ParseByte(data, ref currentIndex);
            header.pduType.versionHigh = (versionHigh_Values)ParseByte(data, ref currentIndex);

            // TS_SHARECONTROLHEADER: pduSource
            header.pduSource = ParseUInt16(data, ref currentIndex, false);

            return header;
        }


        /// <summary>
        /// Parse MCS Domain PDU
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>MCS Domain PDU</returns>
        private DomainMCSPDU ParseMcsDomainPdu(byte[] data, ref int currentIndex)
        {
            // initialize decode buffer
            byte[] temp = GetBytes(data, ref currentIndex, (data.Length - currentIndex));
            Asn1DecodingBuffer buffer = new Asn1DecodingBuffer(temp);

            // decode
            DomainMCSPDU domainPdu = new DomainMCSPDU();
            domainPdu.PerDecode(buffer);
            return domainPdu;
        }


        #region Parser: Security Header
        /// <summary>
        /// Parse Security Header
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <param name="headerType">security header type</param>
        /// <returns>Security Header</returns>
        private TS_SECURITY_HEADER ParseTsSecurityHeader(
            byte[] data,
            ref int currentIndex,
            SecurityHeaderType headerType)
        {
            // parse security header by type
            TS_SECURITY_HEADER header;
            switch (headerType)
            {
                // without header
                case SecurityHeaderType.None:
                    header = null;
                    break;

                // basic header
                case SecurityHeaderType.Basic:
                    header = ParseTsSecurityHeaderBasic(data, ref currentIndex);
                    break;

                // non-fips header
                case SecurityHeaderType.NonFips:
                    header = ParseTsSecurityHeaderNonFips(data, ref currentIndex);
                    break;

                // fips header
                case SecurityHeaderType.Fips:
                    header = ParseTsSecurityHeaderFips(data, ref currentIndex);
                    break;

                default:
                    throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
            }
            return header;
        }


        /// <summary>
        /// Parse TS_SECURITY_HEADER (BASIC)
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SECURITY_HEADER</returns>
        private TS_SECURITY_HEADER ParseTsSecurityHeaderBasic(byte[] data, ref int currentIndex)
        {
            TS_SECURITY_HEADER header = new TS_SECURITY_HEADER();

            // TS_SECURITY_HEADER: flags
            header.flags = (TS_SECURITY_HEADER_flags_Values)ParseUInt16(data, ref currentIndex, false);

            // TS_SECURITY_HEADER: flagsHi
            header.flagsHi = ParseUInt16(data, ref currentIndex, false);

            return header;
        }


        /// <summary>
        /// Parse TS_SECURITY_HEADER (NON-FIPS)
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SECURITY_HEADER</returns>
        private TS_SECURITY_HEADER ParseTsSecurityHeaderNonFips(byte[] data, ref int currentIndex)
        {
            TS_SECURITY_HEADER1 header = new TS_SECURITY_HEADER1();

            // TS_SECURITY_HEADER1: flags
            header.flags = (TS_SECURITY_HEADER_flags_Values)ParseUInt16(data, ref currentIndex, false);

            // TS_SECURITY_HEADER1: flagsHi
            header.flagsHi = ParseUInt16(data, ref currentIndex, false);

            // TS_SECURITY_HEADER1: dataSignature
            header.dataSignature = GetBytes(data, ref currentIndex,
                ConstValue.TS_SECURITY_HEADER_DATA_SIGNATURE_LENGTH);

            return header;
        }


        /// <summary>
        /// Parse TS_SECURITY_HEADER (FIPS)
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SECURITY_HEADER</returns>
        private TS_SECURITY_HEADER ParseTsSecurityHeaderFips(byte[] data, ref int currentIndex)
        {
            TS_SECURITY_HEADER2 header = new TS_SECURITY_HEADER2();

            // TS_SECURITY_HEADER2: flags
            header.flags = (TS_SECURITY_HEADER_flags_Values)ParseUInt16(data, ref currentIndex, false);

            // TS_SECURITY_HEADER2: flagsHi
            header.flagsHi = ParseUInt16(data, ref currentIndex, false);

            // TS_SECURITY_HEADER2: length
            header.length = (TS_SECURITY_HEADER2_length_Values)ParseUInt16(data, ref currentIndex, false);

            // TS_SECURITY_HEADER2: version
            header.version = ParseByte(data, ref currentIndex);

            // TS_SECURITY_HEADER2: padlen
            header.padlen = ParseByte(data, ref currentIndex);

            // TS_SECURITY_HEADER2: dataSignature
            header.dataSignature = GetBytes(data, ref currentIndex,
                ConstValue.TS_SECURITY_HEADER_DATA_SIGNATURE_LENGTH);

            return header;
        }
        #endregion Parser: Security Header

        /// <summary>
        /// Parse Network Detection Response Structures
        /// </summary>
        /// <param name="data"></param>
        /// <param name="currentIndex"></param>
        /// <returns>A specific Network Detection Response Structure</returns>
        public NETWORK_DETECTION_RESPONSE ParseNetworkDetectionResponse(byte[] data, ref int currentIndex, bool isSubHeader = false)
        {
            byte headerLength = (byte)data.Length;
            HeaderTypeId_Values headerTypeId = HeaderTypeId_Values.TYPE_ID_AUTODETECT_RESPONSE;
            if (isSubHeader)
            {
                headerLength += 2;
            }
            else
            {
                headerLength = ParseByte(data, ref currentIndex);
                headerTypeId = (HeaderTypeId_Values)ParseByte(data, ref currentIndex);
            }
            ushort sequenceNumber = ParseUInt16(data, ref currentIndex, false);
            AUTO_DETECT_RESPONSE_TYPE responseType = (AUTO_DETECT_RESPONSE_TYPE)ParseUInt16(data, ref currentIndex, false);
            if (responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_RTT_RESPONSE)
            {
                RDP_RTT_RESPONSE rttResp = new RDP_RTT_RESPONSE();
                rttResp.headerLength = headerLength;
                rttResp.headerTypeId = headerTypeId;
                rttResp.sequenceNumber = sequenceNumber;
                rttResp.responseType = responseType;
                return rttResp;
            }
            else if (responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_BW_RESULTS_AFTER_CONNECT || responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_BW_RESULTS_DURING_CONNECT)
            {
                RDP_BW_RESULTS bwResults = new RDP_BW_RESULTS();
                bwResults.headerLength = headerLength;
                bwResults.headerTypeId = headerTypeId;
                bwResults.sequenceNumber = sequenceNumber;
                bwResults.responseType = responseType;
                bwResults.timeDelta = ParseUInt32(data, ref currentIndex, false);
                bwResults.byteCount = ParseUInt32(data, ref currentIndex, false);
                return bwResults;
            }
            else if (responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_NETCHAR_SYNC)
            {
                RDP_NETCHAR_SYNC netCharSync = new RDP_NETCHAR_SYNC();
                netCharSync.headerLength = headerLength;
                netCharSync.headerTypeId = headerTypeId;
                netCharSync.sequenceNumber = sequenceNumber;
                netCharSync.responseType = responseType;
                netCharSync.bandwidth = ParseUInt32(data, ref currentIndex, false);
                netCharSync.rtt = ParseUInt32(data, ref currentIndex, false);
                return netCharSync;
            }
            else
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
            }

        }
        #endregion Sub Field Parsers: Common Fields


        #region Sub Field Parsers: X224 Request PDU
        /// <summary>
        /// Parse X224Crq
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>X224Crq</returns>
        private X224Crq ParseX224Crq(byte[] data, ref int currentIndex)
        {
            X224Crq crq = new X224Crq();

            // X224Crq: LengthIndicator
            crq.lengthIndicator = ParseByte(data, ref currentIndex);

            // X224Crq: TypeCredit
            crq.typeCredit = ParseByte(data, ref currentIndex);

            // X224Crq: DestRef
            crq.destRef = ParseUInt16(data, ref currentIndex, true);

            // X224Crq: SrcRef
            crq.srcRef = ParseUInt16(data, ref currentIndex, true);

            // X224Crq: ClassOptions
            crq.classOptions = ParseByte(data, ref currentIndex);

            return crq;
        }

        /// <summary>
        /// Parse An optional RDP Negotiation Request
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>RDP_NEG_REQ</returns>
        private RDP_NEG_REQ ParseRdpNegReq(byte[] data, ref int currentIndex)
        {
            RDP_NEG_REQ negReq = new RDP_NEG_REQ();
            negReq.type = (type_Values)ParseByte(data, ref currentIndex);
            negReq.flags = (RDP_NEG_REQ_flags_Values)ParseByte(data, ref currentIndex);
            negReq.length = (length_Values)ParseUInt16(data, ref currentIndex, false);
            negReq.requestedProtocols = (requestedProtocols_Values)ParseUInt32(data, ref currentIndex, false);

            return negReq;
        }

        private RDP_NEG_CORRELATION_INFO ParseRdpNegCorrelationInfo(byte[] data, ref int currentIndex)
        {
            RDP_NEG_CORRELATION_INFO negCorrelationInfo = new RDP_NEG_CORRELATION_INFO();
            negCorrelationInfo.type = (RDP_NEG_CORRELATION_INFO_Type)ParseByte(data, ref currentIndex);
            negCorrelationInfo.flags = ParseByte(data, ref currentIndex);
            negCorrelationInfo.length = ParseUInt16(data, ref currentIndex, false);
            negCorrelationInfo.correlationId = GetBytes(data, ref currentIndex, 16);
            negCorrelationInfo.reserved = GetBytes(data, ref currentIndex, 16);

            return negCorrelationInfo;
        }
        /// <summary>
        /// Parse An optional RDP RoutingToken
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>byte contain routing token</returns>
        private void ParseRoutingTokenOrCookie(byte[] data, ref byte[] routingToken, ref String cookie, ref int currentIndex)
        {
            bool isPresent = false;
            int offset;
            for (offset = 0; currentIndex + offset < data.Length - 1; offset++)
            {
                if (data[currentIndex + offset] == 0x0D && data[currentIndex + offset + 1] == 0x0A)
                {
                    isPresent = true;
                    break;
                }
            }

            if (isPresent)
            {
                routingToken = new byte[offset + 2];
                Array.Copy(data, currentIndex, routingToken, 0, offset + 2);
                cookie = ASCIIEncoding.ASCII.GetString(routingToken);
                if (cookie.StartsWith("Cookie"))
                {// cookie is present
                    routingToken = null;
                }
                else
                {
                    cookie = null;
                }
                currentIndex += (offset + 2);
            }
            else
            {
                routingToken = null;
                cookie = null;
            }
        }
        #endregion Sub Field Parsers: X224 Request PDU


        #region Sub Field Parsers: Mcs Connect Request PDU
        /// <summary>
        /// Parse TS_UD_CS_CORE
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_UD_CS_CORE</returns>
        private TS_UD_CS_CORE ParseTsUdCsCore(byte[] data, ref int currentIndex, int length)
        {
            int endIndex = currentIndex + length - 1;

            TS_UD_CS_CORE coreData = new TS_UD_CS_CORE();
            coreData.header.type = (TS_UD_HEADER_type_Values)ParseUInt16(data, ref currentIndex, false);
            coreData.header.length = ParseUInt16(data, ref currentIndex, false);
            coreData.version = (TS_UD_CS_CORE_version_Values)ParseUInt32(data, ref currentIndex, false);
            coreData.desktopWidth = ParseUInt16(data, ref currentIndex, false);
            coreData.desktopHeight = ParseUInt16(data, ref currentIndex, false);
            coreData.colorDepth = (colorDepth_Values)ParseUInt16(data, ref currentIndex, false);
            coreData.SASSequence = ParseUInt16(data, ref currentIndex, false);
            coreData.keyboardLayout = ParseUInt32(data, ref currentIndex, false);
            coreData.clientBuild = ParseUInt32(data, ref currentIndex, false);
            coreData.clientName = ParseString(data, ref currentIndex, 32);
            coreData.keyboardType = (keyboardType_Values)ParseUInt32(data, ref currentIndex, false);
            coreData.keyboardSubType = ParseUInt32(data, ref currentIndex, false);
            coreData.keyboardFunctionKey = ParseUInt32(data, ref currentIndex, false);
            coreData.imeFileName = ParseString(data, ref currentIndex, 64);
            //below fields are optional fields, verify the length before parsing.
            if (currentIndex < endIndex)
                coreData.postBeta2ColorDepth = new UInt16Class(ParseUInt16(data, ref currentIndex, false));
            if (currentIndex < endIndex)
                coreData.clientProductId = new UInt16Class(ParseUInt16(data, ref currentIndex, false));
            if (currentIndex < endIndex)
                coreData.serialNumber = new UInt32Class(ParseUInt32(data, ref currentIndex, false));
            if (currentIndex < endIndex)
                coreData.highColorDepth = new UInt16Class(ParseUInt16(data, ref currentIndex, false));
            if (currentIndex < endIndex)
                coreData.supportedColorDepths = new UInt16Class(ParseUInt16(data, ref currentIndex, false));
            if (currentIndex < endIndex)
                coreData.earlyCapabilityFlags = new UInt16Class(ParseUInt16(data, ref currentIndex, false));
            if (currentIndex < endIndex)
                coreData.clientDigProductId = ParseString(data, ref currentIndex, 64);
            if (currentIndex < endIndex)
                coreData.connnectionType = new ByteClass(ParseByte(data, ref currentIndex));
            if (currentIndex < endIndex)
                coreData.pad1octets = new ByteClass(ParseByte(data, ref currentIndex));
            if (currentIndex < endIndex)
                coreData.serverSelectedProtocol = new UInt32Class(ParseUInt32(data, ref currentIndex, false));
            if (currentIndex < endIndex)
                coreData.desktopPhysicalWidth = new UInt32Class(ParseUInt32(data, ref currentIndex, false));
            if (currentIndex < endIndex)
                coreData.desktopPhysicalHeight = new UInt32Class(ParseUInt32(data, ref currentIndex, false));
            if (currentIndex < endIndex)
                coreData.desktopOrientation = (TS_UD_CS_CORE_desktopOrientation_values)ParseUInt16(data, ref currentIndex, false);
            if (currentIndex < endIndex)
                coreData.desktopScaleFactor = new UInt32Class(ParseUInt32(data, ref currentIndex, false));
            if (currentIndex < endIndex)
                coreData.deviceScaleFactor = new UInt32Class(ParseUInt32(data, ref currentIndex, false));

            return coreData;
        }


        /// <summary>
        /// Parse TS_UD_CS_NET
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_UD_CS_NET</returns>
        private TS_UD_CS_NET ParseTsUdCsNet(byte[] data, ref int currentIndex)
        {
            TS_UD_CS_NET netData = new TS_UD_CS_NET();


            // TS_UD_CS_NET: Header
            netData.header.type = (TS_UD_HEADER_type_Values)ParseUInt16(data, ref currentIndex, false);
            netData.header.length = ParseUInt16(data, ref currentIndex, false);

            // TS_UD_SC_NET: channelCount
            netData.channelCount = ParseUInt32(data, ref currentIndex, false);

            // TS_UD_SC_NET: channelIdArray
            netData.channelDefArray = new List<CHANNEL_DEF>();
            for (int i = 0; i < netData.channelCount; i++)
            {
                CHANNEL_DEF channelDef;
                byte[] name = GetBytes(data, ref currentIndex, 8);
                ASCIIEncoding converter = new ASCIIEncoding();
                channelDef.name = converter.GetString(name);
                channelDef.options = (Channel_Options)ParseUInt32(data, ref currentIndex, false);
                netData.channelDefArray.Add(channelDef);
            }

            return netData;
        }


        /// <summary>
        /// Parse TS_UD_SC_SEC1
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_UD_SC_SEC1</returns>
        private TS_UD_CS_SEC ParseTsUdCsSec(byte[] data, ref int currentIndex)
        {
            TS_UD_CS_SEC secData = new TS_UD_CS_SEC();

            // reserve the start index
            int startIndex = currentIndex;

            // TS_UD_SC_SEC1: header
            secData.header.type = (TS_UD_HEADER_type_Values)ParseUInt16(data, ref currentIndex, false);
            secData.header.length = ParseUInt16(data, ref currentIndex, false);

            // TS_UD_SC_SEC1: encryptionMethod
            secData.encryptionMethods = (encryptionMethod_Values)ParseUInt32(data, ref currentIndex, false);

            // TS_UD_SC_SEC1: encryptionLevel
            secData.extEncryptionMethods = (encryptionMethod_Values)ParseUInt32(data, ref currentIndex, false);

            return secData;
        }

        /// <summary>
        /// Parse TS_UD_CS_CLUSTER
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_UD_CS_CLUSTER</returns>
        private TS_UD_CS_CLUSTER ParseTsUdCsCluster(byte[] data, ref int currentIndex)
        {
            TS_UD_CS_CLUSTER cluData = new TS_UD_CS_CLUSTER();
            cluData.header.type = (TS_UD_HEADER_type_Values)ParseUInt16(data, ref currentIndex, false);
            cluData.header.length = ParseUInt16(data, ref currentIndex, false);
            cluData.Flags = (Flags_Values)ParseUInt32(data, ref currentIndex, false);
            cluData.RedirectedSessionID = ParseUInt32(data, ref currentIndex, false);

            return cluData;
        }

        /// <summary>
        /// Parse TS_UD_CS_MONITOR
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_UD_CS_MONITOR</returns>
        private TS_UD_CS_MONITOR ParseTsUdCsMon(byte[] data, ref int currentIndex)
        {
            TS_UD_CS_MONITOR monData = new TS_UD_CS_MONITOR();
            monData.header.type = (TS_UD_HEADER_type_Values)ParseUInt16(data, ref currentIndex, false);
            monData.header.length = ParseUInt16(data, ref currentIndex, false);
            monData.Flags = ParseUInt32(data, ref currentIndex, false);
            monData.monitorCount = ParseUInt32(data, ref currentIndex, false);
            monData.monitorDefArray = new Collection<TS_MONITOR_DEF>();
            for (int i = 0; i < monData.monitorCount; ++i)
            {
                TS_MONITOR_DEF monDef;
                monDef.left = ParseUInt32(data, ref currentIndex, false);
                monDef.top = ParseUInt32(data, ref currentIndex, false);
                monDef.right = ParseUInt32(data, ref currentIndex, false);
                monDef.bottom = ParseUInt32(data, ref currentIndex, false);
                monDef.flags = (Flags_TS_MONITOR_DEF)(ParseUInt32(data, ref currentIndex, false));
                monData.monitorDefArray.Add(monDef);
            }

            return monData;
        }

        /// <summary>
        /// Parse TS_UD_CS_MCS_MSGCHANNEL
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_UD_CS_MCS_MSGCHANNEL</returns>
        private TS_UD_CS_MCS_MSGCHANNEL ParseTsUdCsMcsMsgChannel(byte[] data, ref int currentIndex)
        {
            TS_UD_CS_MCS_MSGCHANNEL msgChannelData = new TS_UD_CS_MCS_MSGCHANNEL();
            msgChannelData.header.type = (TS_UD_HEADER_type_Values)ParseUInt16(data, ref currentIndex, false);
            msgChannelData.header.length = ParseUInt16(data, ref currentIndex, false);
            msgChannelData.flags = (FLAGS_TS_UD_CS_MCS_MSGCHANNEL)(ParseUInt32(data, ref currentIndex, false));

            return msgChannelData;
        }

        /// <summary>
        /// Parse TS_UD_CS_MULTITRANSPORT
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_UD_CS_MULTITRANSPORT</returns>
        private TS_UD_CS_MULTITRANSPORT ParseTsUdCsMultiTransport(byte[] data, ref int currentIndex)
        {
            TS_UD_CS_MULTITRANSPORT multiTransData = new TS_UD_CS_MULTITRANSPORT();
            multiTransData.header.type = (TS_UD_HEADER_type_Values)ParseUInt16(data, ref currentIndex, false);
            multiTransData.header.length = ParseUInt16(data, ref currentIndex, false);
            multiTransData.flags = (MULTITRANSPORT_TYPE_FLAGS)(ParseUInt32(data, ref currentIndex, false));

            return multiTransData;
        }

        /// <summary>
        /// Parse TS_UD_CS_MONITOR_EX
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_UD_CS_MONITOR_EX</returns>
        private TS_UD_CS_MONITOR_EX ParseTsUdCsMonitorEX(byte[] data, ref int currentIndex)
        {
            TS_UD_CS_MONITOR_EX monitorEx = new TS_UD_CS_MONITOR_EX();
            monitorEx.header.type = (TS_UD_HEADER_type_Values)ParseUInt16(data, ref currentIndex, false);
            monitorEx.header.length = ParseUInt16(data, ref currentIndex, false);
            monitorEx.flags = ParseUInt32(data, ref currentIndex, false);
            monitorEx.monitorAttributeSize = ParseUInt32(data, ref currentIndex, false);
            monitorEx.monitorCount = ParseUInt32(data, ref currentIndex, false);

            List<TS_MONITOR_ATTRIBUTES> attributeList = new List<TS_MONITOR_ATTRIBUTES>();
            while (attributeList.Count < monitorEx.monitorCount)
            {
                TS_MONITOR_ATTRIBUTES attribute = new TS_MONITOR_ATTRIBUTES();
                attribute.physicalWidth = ParseUInt32(data, ref currentIndex, false);
                attribute.physicalHeight = ParseUInt32(data, ref currentIndex, false);
                attribute.orientation = (TS_MONITOR_ATTRIBUTES_orientation_values)ParseUInt32(data, ref currentIndex, false);
                attribute.desktopScaleFactor = ParseUInt32(data, ref currentIndex, false);
                attribute.deviceScaleFactor = ParseUInt32(data, ref currentIndex, false);

                attributeList.Add(attribute);
            }
            monitorEx.monitorAttributesArray = attributeList.ToArray();

            return monitorEx;
        }

        #endregion Sub Field Parsers: Mcs Connect Response


        #region Sub Field Parsers: Client Security Exchange PDU
        /// <summary>
        /// Parse TS_SECURITY_PACKET
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="serverSessionContext">the server session context</param>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SECURITY_PACKET</returns>
        private TS_SECURITY_PACKET ParseSecurityExchange(
            RdpbcgrServerSessionContext serverSessionContext,
            byte[] data,
            ref int currentIndex)
        {
            TS_SECURITY_PACKET secExchangeData = new TS_SECURITY_PACKET();
            secExchangeData.length = ParseUInt32(data, ref currentIndex, false);

            byte[] encryptedRandom = GetBytes(data, ref currentIndex, (int)secExchangeData.length);

            byte[] decryptedRandom = RdpbcgrUtility.DecryptClientRandom(
                encryptedRandom,
                serverSessionContext.ServerPrivateExponent,
                serverSessionContext.ServerModulus);

            if (decryptedRandom.Length != ConstValue.CLIENT_RANDOM_NUMBER_SIZE)
            {
                Array.Resize<byte>(ref decryptedRandom, ConstValue.CLIENT_RANDOM_NUMBER_SIZE);
            }

            int startIndex = 0;
            //secExchangeData.clientRandom =
            //    GetBytes(decryptedRandom, ref startIndex, (int)secExchangeData.length - 40);
            secExchangeData.clientRandom =
                GetBytes(decryptedRandom, ref startIndex, decryptedRandom.Length);

            return secExchangeData;
        }
        #endregion Sub Field Parsers: Client Security Exchange PDU


        #region Sub Field Parsers: Client Info PDU
        /// <summary>
        /// Parse TS_INFO_PACKET
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_INFO_PACKET</returns>
        private TS_INFO_PACKET ParseClientInfo(byte[] data, ref int currentIndex)
        {
            TS_INFO_PACKET infoData = new TS_INFO_PACKET();
            infoData.CodePage = ParseUInt32(data, ref currentIndex, false);
            infoData.flags = (flags_Values)ParseUInt32(data, ref currentIndex, false);
            infoData.cbDomain = ParseUInt16(data, ref currentIndex, false);
            infoData.cbUserName = ParseUInt16(data, ref currentIndex, false);
            infoData.cbPassword = ParseUInt16(data, ref currentIndex, false);
            infoData.cbAlternateShell = ParseUInt16(data, ref currentIndex, false);
            infoData.cbWorkingDir = ParseUInt16(data, ref currentIndex, false);
            Encoding converter = new UnicodeEncoding();

            if ((infoData.flags & flags_Values.INFO_UNICODE) == flags_Values.INFO_UNICODE)
            {
                converter = new UnicodeEncoding();

                //Include the null terminator
                byte[] domain = GetBytes(data, ref currentIndex, (int)infoData.cbDomain + 2);
                infoData.Domain = converter.GetString(domain);

                byte[] userName = GetBytes(data, ref currentIndex, (int)infoData.cbUserName + 2);
                infoData.UserName = converter.GetString(userName);

                byte[] password = GetBytes(data, ref currentIndex, (int)infoData.cbPassword + 2);
                infoData.Password = converter.GetString(password);

                byte[] alternateShell = GetBytes(data, ref currentIndex, (int)infoData.cbAlternateShell + 2);
                infoData.AlternateShell = converter.GetString(alternateShell);

                byte[] workingDir = GetBytes(data, ref currentIndex, (int)infoData.cbWorkingDir + 2);
                infoData.WorkingDir = converter.GetString(workingDir);
            }

            else
            {
                converter = new ASCIIEncoding();

                byte[] domain = GetBytes(data, ref currentIndex, (int)infoData.cbDomain + 1);
                infoData.Domain = converter.GetString(domain);

                byte[] userName = GetBytes(data, ref currentIndex, (int)infoData.cbUserName + 1);
                infoData.UserName = converter.GetString(userName);

                byte[] password = GetBytes(data, ref currentIndex, (int)infoData.cbPassword + 1);
                infoData.Password = converter.GetString(password);

                byte[] alternateShell = GetBytes(data, ref currentIndex, (int)infoData.cbAlternateShell + 1);
                infoData.AlternateShell = converter.GetString(alternateShell);

                byte[] workingDir = GetBytes(data, ref currentIndex, (int)infoData.cbWorkingDir + 1);
                infoData.WorkingDir = converter.GetString(workingDir);
            }

            if (currentIndex < data.Length)
            {
                infoData.extraInfo = ParseExtendedInfo(data, ref currentIndex, converter);
            }

            return infoData;
        }

        /// <summary>
        /// Parse TS_EXTENDED_INFO_PACKET
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_EXTENDED_INFO_PACKET</returns>
        private TS_EXTENDED_INFO_PACKET ParseExtendedInfo(byte[] data, ref int currentIndex, Encoding converter)
        {
            TS_EXTENDED_INFO_PACKET extData = new TS_EXTENDED_INFO_PACKET();
            extData.clientAddressFamily = (clientAddressFamily_Values)ParseUInt16(data, ref currentIndex, false);

            extData.cbClientAddress = ParseUInt16(data, ref currentIndex, false);
            byte[] clientAddress = GetBytes(data, ref currentIndex, extData.cbClientAddress);
            extData.clientAddress = converter.GetString(clientAddress);

            extData.cbClientDir = ParseUInt16(data, ref currentIndex, false);
            byte[] clientDir = GetBytes(data, ref currentIndex, extData.cbClientDir);
            extData.clientDir = converter.GetString(clientDir);

            extData.clientTimeZone = ParseTimeZone(data, ref currentIndex);
            extData.clientSessionId = ParseUInt32(data, ref currentIndex, false);
            extData.performanceFlags = (performanceFlags_Values)ParseUInt32(data, ref currentIndex, false);
            extData.cbAutoReconnectLen = ParseUInt16(data, ref currentIndex, false);
            if (extData.cbAutoReconnectLen != 0)
            {
                extData.autoReconnectCookie = ParseAutoReconnectCookie(data, ref currentIndex);
            }
            if (currentIndex < data.Length)
                extData.reserved1 = new UInt16Class(ParseUInt16(data, ref currentIndex, false));
            if (currentIndex < data.Length)
                extData.reserved2 = new UInt16Class(ParseUInt16(data, ref currentIndex, false));
            if (currentIndex < data.Length)
            {
                extData.cbDynamicDSTTimeZoneKeyName = new UInt16Class(ParseUInt16(data, ref currentIndex, false));
                if (extData.cbDynamicDSTTimeZoneKeyName.actualData != 0)
                {
                    byte[] timeZoneKeyName = GetBytes(data, ref currentIndex, extData.cbDynamicDSTTimeZoneKeyName.actualData);
                    extData.dynamicDSTTimeZoneKeyName = converter.GetString(timeZoneKeyName);
                }
            }
            if (currentIndex < data.Length)
                extData.dynamicDaylightTimeDisabled = new UInt16Class(ParseUInt16(data, ref currentIndex, false));

            return extData;
        }

        /// <summary>
        /// Parse TS_TIME_ZONE_INFORMATION
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_TIME_ZONE_INFORMATION</returns>
        private TS_TIME_ZONE_INFORMATION ParseTimeZone(byte[] data, ref int currentIndex)
        {
            TS_TIME_ZONE_INFORMATION timeZoneData = new TS_TIME_ZONE_INFORMATION();
            timeZoneData.Bias = (int)ParseUInt32(data, ref currentIndex, false);
            timeZoneData.StandardName = GetString(data, ref currentIndex, 64);
            timeZoneData.StandardDate = ParseSystemTime(data, ref currentIndex);
            timeZoneData.StandardBias = (int)ParseUInt32(data, ref currentIndex, false);
            timeZoneData.DaylightName = GetString(data, ref currentIndex, 64);
            timeZoneData.DaylightDate = ParseSystemTime(data, ref currentIndex);
            timeZoneData.DaylightBias = (int)ParseUInt32(data, ref currentIndex, false);
            return timeZoneData;
        }

        /// <summary>
        /// Parse TS_SYSTEMTIME
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SYSTEMTIME</returns>
        private TS_SYSTEMTIME ParseSystemTime(byte[] data, ref int currentIndex)
        {
            TS_SYSTEMTIME sysTimeData = new TS_SYSTEMTIME();
            sysTimeData.wYear = ParseUInt16(data, ref currentIndex, false);
            sysTimeData.wMonth = (wMonth_Values)ParseUInt16(data, ref currentIndex, false);
            sysTimeData.wDayOfWeek = (wDayOfWeek_Values)ParseUInt16(data, ref currentIndex, false);
            sysTimeData.wDay = (wDay_Values)ParseUInt16(data, ref currentIndex, false);
            sysTimeData.wHour = ParseUInt16(data, ref currentIndex, false);
            sysTimeData.wMinute = ParseUInt16(data, ref currentIndex, false);
            sysTimeData.wSecond = ParseUInt16(data, ref currentIndex, false);
            sysTimeData.wMilliseconds = ParseUInt16(data, ref currentIndex, false);

            return sysTimeData;
        }

        /// <summary>
        /// Parse ARC_CS_PRIVATE_PACKET
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>ARC_CS_PRIVATE_PACKET</returns>
        private ARC_CS_PRIVATE_PACKET ParseAutoReconnectCookie(byte[] data, ref int currentIndex)
        {
            ARC_CS_PRIVATE_PACKET arcData = new ARC_CS_PRIVATE_PACKET();
            arcData.cbLen = (ARC_CS_PRIVATE_PACKET_cbLen_Values)ParseUInt32(data, ref currentIndex, false);
            arcData.Version = (ARC_CS_PRIVATE_PACKET_Version_Values)ParseUInt32(data, ref currentIndex, false);
            arcData.LogonId = ParseUInt32(data, ref currentIndex, false);
            arcData.SecurityVerifier = GetBytes(data, ref currentIndex, 16);

            return arcData;
        }
        #endregion Sub Field Parsers: Client Info PDU


        #region Sub Field Parsers: Client Confirm Active PDU
        /// <summary>
        /// Parse TS_CONFIRM_ACTIVE_PDU
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_CONFIRM_ACTIVE_PDU</returns>
        private TS_CONFIRM_ACTIVE_PDU ParseTsConfirmActivePdu(byte[] data, ref int currentIndex)
        {
            TS_CONFIRM_ACTIVE_PDU pdu = new TS_CONFIRM_ACTIVE_PDU();
            pdu.shareControlHeader = ParseTsShareControlHeader(data, ref currentIndex);
            pdu.shareId = ParseUInt32(data, ref currentIndex, false);
            pdu.originatorId = (originatorId_Values)ParseUInt16(data, ref currentIndex, false);
            pdu.lengthSourceDescriptor = ParseUInt16(data, ref currentIndex, false);
            pdu.lengthCombinedCapabilities = ParseUInt16(data, ref currentIndex, false);
            pdu.sourceDescriptor =
                GetBytes(data, ref currentIndex, pdu.lengthSourceDescriptor);
            pdu.numberCapabilities = ParseUInt16(data, ref currentIndex, false);
            pdu.pad2Octets = ParseUInt16(data, ref currentIndex, false);
            pdu.capabilitySets = new Collection<ITsCapsSet>();
            while (currentIndex < data.Length)
            {
                pdu.capabilitySets.Add(ParseTsCapsSet(data, ref currentIndex));
            }

            return pdu;
        }

        /// <summary>
        /// Parse ITsCapsSet
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>ITsCapsSet</returns>
        private ITsCapsSet ParseTsCapsSet(byte[] data, ref int currentIndex)
        {
            ITsCapsSet set = null;
            int tempIndex = currentIndex;

            // TS_CAPS_SET: capabilitySetType
            capabilitySetType_Values capSetType = (capabilitySetType_Values)ParseUInt16(data, ref tempIndex, false);

            // TS_CAPS_SET: lengthCapability
            ushort length = ParseUInt16(data, ref tempIndex, false);

            // TS_CAPS_SET: capabilityData
            byte[] capabilityData = GetBytes(data, ref currentIndex, length);

            switch (capSetType)
            {
                // General Capability Set
                case capabilitySetType_Values.CAPSTYPE_GENERAL:
                    set = ParseCapsTypeGeneral(capabilityData);
                    break;

                // Revision 1 Bitmap Cache Capability Set.
                case capabilitySetType_Values.CAPSTYPE_BITMAPCACHE:
                    set = ParseCapsTypeBitmapCache(capabilityData);
                    break;

                // Window Activation Capability Set
                case capabilitySetType_Values.CAPSTYPE_ACTIVATION:
                    set = ParseCapsTypeActivation(capabilityData);
                    break;

                // Bitmap Capability Set    
                case capabilitySetType_Values.CAPSTYPE_BITMAP:
                    set = ParseCapsTypeBitmap(capabilityData);
                    break;

                // Order Capability Set
                case capabilitySetType_Values.CAPSTYPE_ORDER:
                    set = ParseCapsTypeOrder(capabilityData);
                    break;

                // Pointer Capability Set
                case capabilitySetType_Values.CAPSTYPE_POINTER:
                    set = ParseCapsTypePointer(capabilityData);
                    break;

                // Share Capability Set
                case capabilitySetType_Values.CAPSTYPE_SHARE:
                    set = ParseCapsTypeShare(capabilityData);
                    break;

                // Color Table Cache Capability Set
                case capabilitySetType_Values.CAPSTYPE_COLORCACHE:
                    set = ParseCapsTypeColorCache(capabilityData);
                    break;

                // Input Capability Set
                case capabilitySetType_Values.CAPSTYPE_INPUT:
                    set = ParseCapsTypeInput(capabilityData);
                    break;

                // Font Capability Set
                case capabilitySetType_Values.CAPSTYPE_FONT:
                    set = ParseCapsTypeFont(capabilityData);
                    break;

                // Bitmap Cache Host Support Capability Set
                case capabilitySetType_Values.CAPSTYPE_BITMAPCACHE_HOSTSUPPORT:
                    set = ParseCapsTypeBitmapCacheHostSupport(capabilityData);
                    break;

                // Virtual Channel Capability Set
                case capabilitySetType_Values.CAPSTYPE_VIRTUALCHANNEL:
                    set = ParseCapsTypeVirtualChannel(capabilityData);
                    break;

                // DrawNineGrid Cache Capability Set
                case capabilitySetType_Values.CAPSTYPE_DRAWNINEGRIDCACHE:
                    set = ParseCapsTypeDrawNineGridCache(capabilityData);
                    break;

                // Draw GDI+ Cache Capability Set
                case capabilitySetType_Values.CAPSTYPE_DRAWGDIPLUS:
                    set = ParseCapsTypeDrawGdiPlus(capabilityData);
                    break;

                // Remote Programs Capability Set
                case capabilitySetType_Values.CAPSTYPE_RAIL:
                    set = ParseCapsTypeRail(capabilityData);
                    break;

                // Window List Capability Set
                case capabilitySetType_Values.CAPSTYPE_WINDOW:
                    set = ParseCapsTypeWindow(capabilityData);
                    break;

                // Desktop Composition Extension Capability Set
                case capabilitySetType_Values.CAPSETTYPE_COMPDESK:
                    set = ParseCapsTypeCompDesk(capabilityData);
                    break;

                // Multifragment Update Capability Set
                case capabilitySetType_Values.CAPSETTYPE_MULTIFRAGMENTUPDATE:
                    set = ParseCapsTypeMultiFragmentUpdate(capabilityData);
                    break;

                // Large Pointer Capability Set
                case capabilitySetType_Values.CAPSETTYPE_LARGE_POINTER:
                    set = ParseCapsTypeLargePointer(capabilityData);
                    break;

                // Surface Commands Capability Set
                case capabilitySetType_Values.CAPSETTYPE_SURFACE_COMMANDS:
                    set = ParseCapsTypeSurfaceCommands(capabilityData);
                    break;

                // Bitmap Codecs Capability Set
                case capabilitySetType_Values.CAPSETTYPE_BITMAP_CODECS:
                    set = ParseCapsTypeBitmapCodecs(capabilityData);
                    break;

                // Revision 2 Bitmap Cache Capability Set.
                case capabilitySetType_Values.CAPSTYPE_BITMAPCACHE_REV2:
                    set = ParseCapsTypeBitmapCacheRev2(capabilityData);
                    break;

                // Brush Capability Set.
                case capabilitySetType_Values.CAPSTYPE_BRUSH:
                    set = ParseCapsTypeBrush(capabilityData);
                    break;

                // Glyph Cache Capability Set.
                case capabilitySetType_Values.CAPSTYPE_GLYPHCACHE:
                    set = ParseCapsTypeGlyphCache(capabilityData);
                    break;

                // Offscreen Bitmap Cache Capability Set.
                case capabilitySetType_Values.CAPSTYPE_OFFSCREENCACHE:
                    set = ParseCapsTypeOffScreenCache(capabilityData);
                    break;

                // Sound Capability Set.
                case capabilitySetType_Values.CAPSTYPE_SOUND:
                    set = ParseCapsTypeSound(capabilityData);
                    break;

                // Control Capability Set.
                case capabilitySetType_Values.CAPSTYPE_CONTROL:
                    set = PaseCapsTypeControl(capabilityData);
                    break;

                // TS_FRAME_ACKNOWLEDGE_CAPABILITYSET.
                case capabilitySetType_Values.CAPSETTYPE_FRAME_ACKNOWLEDGE:
                    set = PaseCapsTypeFrameAcknowledge(capabilityData);
                    break;

                default:
                    throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
            }
            return set;
        }


        #region Capbility Sets
        /// <summary>
        /// Parse TS_GENERAL_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_GENERAL_CAPABILITYSET</returns>
        private TS_GENERAL_CAPABILITYSET ParseCapsTypeGeneral(byte[] data)
        {
            TS_GENERAL_CAPABILITYSET set = RdpbcgrUtility.ToStruct<TS_GENERAL_CAPABILITYSET>(data);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }

        /// <summary>
        /// Parse TS_BITMAPCACHE_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_BITMAPCACHE_CAPABILITYSET</returns>
        private TS_BITMAPCACHE_CAPABILITYSET ParseCapsTypeBitmapCache(byte[] data)
        {
            TS_BITMAPCACHE_CAPABILITYSET set = RdpbcgrUtility.ToStruct<TS_BITMAPCACHE_CAPABILITYSET>(data);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }

        /// <summary>
        /// Parse TS_WINDOWACTIVATION_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_WINDOWACTIVATION_CAPABILITYSET</returns>
        private TS_WINDOWACTIVATION_CAPABILITYSET ParseCapsTypeActivation(byte[] data)
        {
            TS_WINDOWACTIVATION_CAPABILITYSET set = RdpbcgrUtility.ToStruct<TS_WINDOWACTIVATION_CAPABILITYSET>(data);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }


        /// <summary>
        /// Parse TS_BITMAP_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_BITMAP_CAPABILITYSET</returns>
        private TS_BITMAP_CAPABILITYSET ParseCapsTypeBitmap(byte[] data)
        {
            TS_BITMAP_CAPABILITYSET set = RdpbcgrUtility.ToStruct<TS_BITMAP_CAPABILITYSET>(data);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }


        /// <summary>
        /// Parse TS_ORDER_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_ORDER_CAPABILITYSET</returns>
        private TS_ORDER_CAPABILITYSET ParseCapsTypeOrder(byte[] data)
        {
            int currentIndex = 0;
            TS_ORDER_CAPABILITYSET set = new TS_ORDER_CAPABILITYSET();

            // TS_ORDER_CAPABILITYSET: capabilitySetType
            set.capabilitySetType = (capabilitySetType_Values)ParseUInt16(data, ref currentIndex, false);

            // TS_ORDER_CAPABILITYSET: lengthCapability
            set.lengthCapability = ParseUInt16(data, ref currentIndex, false);

            // TS_ORDER_CAPABILITYSET: terminalDescriptor
            set.terminalDescriptor = GetBytes(data, ref currentIndex,
                ConstValue.TS_ORDER_CAPABILITYSET_TERMINAL_DESCRIPTOR_LENGTH);

            // TS_ORDER_CAPABILITYSET: pad4octetsA
            set.pad4octetsA = ParseUInt32(data, ref currentIndex, false);

            // TS_ORDER_CAPABILITYSET: desktopSaveXGranularity
            set.desktopSaveXGranularity = ParseUInt16(data, ref currentIndex, false);

            // TS_ORDER_CAPABILITYSET: desktopSaveYGranularity
            set.desktopSaveYGranularity = ParseUInt16(data, ref currentIndex, false);

            // TS_ORDER_CAPABILITYSET: pad2octetsA
            set.pad2octetsA = ParseUInt16(data, ref currentIndex, false);

            // TS_ORDER_CAPABILITYSET: maximumOrderLevel
            set.maximumOrderLevel = ParseUInt16(data, ref currentIndex, false);

            // TS_ORDER_CAPABILITYSET: numberFonts
            set.numberFonts = ParseUInt16(data, ref currentIndex, false); ;

            // TS_ORDER_CAPABILITYSET: orderFlags
            set.orderFlags = (orderFlags_Values)ParseUInt16(data, ref currentIndex, false);

            // TS_ORDER_CAPABILITYSET: orderSupport
            set.orderSupport = GetBytes(data, ref currentIndex,
                ConstValue.TS_ORDER_CAPABILITYSET_ORDER_SUPPORT_LENGTH);

            // TS_ORDER_CAPABILITYSET: textFlags
            set.textFlags = ParseUInt16(data, ref currentIndex, false);

            // TS_ORDER_CAPABILITYSET: orderSupportExFlags
            set.orderSupportExFlags = (orderSupportExFlags_values)ParseUInt16(data, ref currentIndex, false);

            // TS_ORDER_CAPABILITYSET: pad4octetsB
            set.pad4octetsB = ParseUInt32(data, ref currentIndex, false);

            // TS_ORDER_CAPABILITYSET: desktopSaveSize
            set.desktopSaveSize = ParseUInt32(data, ref currentIndex, false);

            // TS_ORDER_CAPABILITYSET: pad2octetsC
            set.pad2octetsC = ParseUInt16(data, ref currentIndex, false);

            // TS_ORDER_CAPABILITYSET: pad2octetsD
            set.pad2octetsD = ParseUInt16(data, ref currentIndex, false);

            // TS_ORDER_CAPABILITYSET: textANSICodePage
            set.textANSICodePage = ParseUInt16(data, ref currentIndex, false);

            // TS_ORDER_CAPABILITYSET: pad2octetsE
            set.pad2octetsE = ParseUInt16(data, ref currentIndex, false);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, currentIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }


        /// <summary>
        /// Parse TS_POINTER_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_POINTER_CAPABILITYSET</returns>
        private TS_POINTER_CAPABILITYSET ParseCapsTypePointer(byte[] data)
        {
            TS_POINTER_CAPABILITYSET set = RdpbcgrUtility.ToStruct<TS_POINTER_CAPABILITYSET>(data);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }


        /// <summary>
        /// Parse TS_SHARE_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_SHARE_CAPABILITYSET</returns>
        private TS_SHARE_CAPABILITYSET ParseCapsTypeShare(byte[] data)
        {
            TS_SHARE_CAPABILITYSET set = RdpbcgrUtility.ToStruct<TS_SHARE_CAPABILITYSET>(data);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }


        /// <summary>
        /// Parse TS_COLORCACHE_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_COLORCACHE_CAPABILITYSET</returns>
        private TS_COLORCACHE_CAPABILITYSET ParseCapsTypeColorCache(byte[] data)
        {
            TS_COLORCACHE_CAPABILITYSET set = new TS_COLORCACHE_CAPABILITYSET();

            // TS_COLORCACHE_CAPABILITYSET: rawData
            set.rawData = data;

            return set;
        }


        /// <summary>
        /// Parse TS_INPUT_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_INPUT_CAPABILITYSET</returns>
        private TS_INPUT_CAPABILITYSET ParseCapsTypeInput(byte[] data)
        {
            int currentIndex = 0;
            TS_INPUT_CAPABILITYSET set = new TS_INPUT_CAPABILITYSET();

            // TS_INPUT_CAPABILITYSET: capabilitySetType
            set.capabilitySetType = (capabilitySetType_Values)ParseUInt16(data, ref currentIndex, false);

            // TS_INPUT_CAPABILITYSET: lengthCapability
            set.lengthCapability = ParseUInt16(data, ref currentIndex, false);

            // TS_INPUT_CAPABILITYSET: inputFlags
            set.inputFlags = (inputFlags_Values)ParseUInt16(data, ref currentIndex, false);

            // TS_INPUT_CAPABILITYSET: pad2octetsA
            set.pad2octetsA = ParseUInt16(data, ref currentIndex, false);

            // TS_INPUT_CAPABILITYSET: keyboardLayout
            set.keyboardLayout = ParseUInt32(data, ref currentIndex, false);

            // TS_INPUT_CAPABILITYSET: keyboardType
            set.keyboardType = (TS_INPUT_CAPABILITYSET_keyboardType_Values)ParseUInt32(data, ref currentIndex, false);

            // TS_INPUT_CAPABILITYSET: keyboardSubType
            set.keyboardSubType = ParseUInt32(data, ref currentIndex, false);

            // TS_INPUT_CAPABILITYSET: keyboardFunctionKey
            set.keyboardFunctionKey = ParseUInt32(data, ref currentIndex, false);

            // TS_INPUT_CAPABILITYSET: imeFileName
            byte[] imeFileName = GetBytes(data, ref currentIndex,
                ConstValue.TS_INPUT_CAPABILITYSET_IME_FILE_NAME_LENGTH);
            set.imeFileName = Encoding.Unicode.GetString(imeFileName);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, currentIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }


        /// <summary>
        /// Parse TS_FONT_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_FONT_CAPABILITYSET</returns>
        private TS_FONT_CAPABILITYSET ParseCapsTypeFont(byte[] data)
        {
            TS_FONT_CAPABILITYSET set = RdpbcgrUtility.ToStruct<TS_FONT_CAPABILITYSET>(data);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }


        /// <summary>
        /// Parse TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET</returns>
        private TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET ParseCapsTypeBitmapCacheHostSupport(byte[] data)
        {
            TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET set =
                RdpbcgrUtility.ToStruct<TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET>(data);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }

        /// <summary>
        /// Parse TS_BITMAPCACHE_CAPABILITYSET_REV2
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_BITMAPCACHE_CAPABILITYSET_REV2</returns>
        private TS_BITMAPCACHE_CAPABILITYSET_REV2 ParseCapsTypeBitmapCacheRev2(byte[] data)
        {
            int currentIndex = 0;
            TS_BITMAPCACHE_CAPABILITYSET_REV2 set = new TS_BITMAPCACHE_CAPABILITYSET_REV2();
            set.capabilitySetType = (capabilitySetType_Values)ParseUInt16(data, ref currentIndex, false);
            set.lengthCapability = ParseUInt16(data, ref currentIndex, false);
            set.CacheFlags = (CacheFlags_Values)ParseUInt16(data, ref currentIndex, false);
            set.pad2 = ParseByte(data, ref currentIndex);
            set.NumCellCaches = ParseByte(data, ref currentIndex);
            set.BitmapCache1CellInfo = ParseBitmapCacheCellInfo(data, ref currentIndex);
            set.BitmapCache2CellInfo = ParseBitmapCacheCellInfo(data, ref currentIndex);
            set.BitmapCache3CellInfo = ParseBitmapCacheCellInfo(data, ref currentIndex);
            set.BitmapCache4CellInfo = ParseBitmapCacheCellInfo(data, ref currentIndex);
            set.BitmapCache5CellInfo = ParseBitmapCacheCellInfo(data, ref currentIndex);
            set.Pad3 = GetBytes(data, ref currentIndex, 12);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, currentIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }

        /// <summary>
        /// Parse TS_BRUSH_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_BRUSH_CAPABILITYSET</returns>
        private TS_BRUSH_CAPABILITYSET ParseCapsTypeBrush(byte[] data)
        {
            TS_BRUSH_CAPABILITYSET set = RdpbcgrUtility.ToStruct<TS_BRUSH_CAPABILITYSET>(data);

            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }

        /// <summary>
        /// Parse TS_GLYPHCACHE_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_GLYPHCACHE_CAPABILITYSET</returns>
        private TS_GLYPHCACHE_CAPABILITYSET ParseCapsTypeGlyphCache(byte[] data)
        {
            int currentIndex = 0;
            TS_GLYPHCACHE_CAPABILITYSET set = new TS_GLYPHCACHE_CAPABILITYSET();
            set.capabilitySetType = (capabilitySetType_Values)ParseUInt16(data, ref currentIndex, false);
            set.lengthCapability = ParseUInt16(data, ref currentIndex, false);
            set.GlyphCache = new TS_CACHE_DEFINITION[10];
            for (int i = 0; i < 10; i++)
            {
                set.GlyphCache[i] = ParseGlyphCache(data, ref currentIndex);
            }
            set.FragCache = ParseFragCache(data, ref currentIndex);
            set.GlyphSupportLevel = (GlyphSupportLevel_Values)ParseUInt16(data, ref currentIndex, false);
            set.pad2octets = ParseUInt16(data, ref currentIndex, false);

            VerifyDataLength(data.Length, currentIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;

        }

        /// <summary>
        /// Parse TS_CACHE_DEFINITION
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_CACHE_DEFINITION</returns>
        private TS_CACHE_DEFINITION ParseGlyphCache(byte[] data, ref int currentIndex)
        {
            TS_CACHE_DEFINITION cacheData = new TS_CACHE_DEFINITION();
            cacheData.CacheEntries = ParseUInt16(data, ref currentIndex, false);
            cacheData.CacheMaximumCellSize = ParseUInt16(data, ref currentIndex, false);

            return cacheData;
        }

        /// <summary>
        /// Parse TS_CACHE_DEFINITION
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_CACHE_DEFINITION</returns>
        private TS_CACHE_DEFINITION ParseFragCache(byte[] data, ref int currentIndex)
        {
            TS_CACHE_DEFINITION cacheData = new TS_CACHE_DEFINITION();
            cacheData.CacheEntries = ParseUInt16(data, ref currentIndex, false);
            cacheData.CacheMaximumCellSize = ParseUInt16(data, ref currentIndex, false);

            return cacheData;
        }

        /// <summary>
        /// Parse TS_OFFSCREEN_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_OFFSCREEN_CAPABILITYSET</returns>
        private TS_OFFSCREEN_CAPABILITYSET ParseCapsTypeOffScreenCache(byte[] data)
        {
            TS_OFFSCREEN_CAPABILITYSET set = RdpbcgrUtility.ToStruct<TS_OFFSCREEN_CAPABILITYSET>(data);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }

        /// <summary>
        /// Parse TS_SOUND_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_SOUND_CAPABILITYSET</returns>
        private TS_SOUND_CAPABILITYSET ParseCapsTypeSound(byte[] data)
        {
            TS_SOUND_CAPABILITYSET set = RdpbcgrUtility.ToStruct<TS_SOUND_CAPABILITYSET>(data);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }

        /// <summary>
        /// Parse TS_CONTROL_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_CONTROL_CAPABILITYSET</returns>
        private TS_CONTROL_CAPABILITYSET PaseCapsTypeControl(byte[] data)
        {
            TS_CONTROL_CAPABILITYSET set = RdpbcgrUtility.ToStruct<TS_CONTROL_CAPABILITYSET>(data);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }

        /// <summary>
        /// Parse TS_BITMAPCACHE_CELL_CACHE_INFO
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_BITMAPCACHE_CELL_CACHE_INFO</returns>
        private TS_BITMAPCACHE_CELL_CACHE_INFO ParseBitmapCacheCellInfo(byte[] data, ref int index)
        {
            TS_BITMAPCACHE_CELL_CACHE_INFO cellCacheInfo = new TS_BITMAPCACHE_CELL_CACHE_INFO();
            cellCacheInfo.NumEntriesAndK = ParseUInt32(data, ref index, false);

            return cellCacheInfo;
        }


        /// <summary>
        /// Parse TS_VIRTUALCHANNEL_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_VIRTUALCHANNEL_CAPABILITYSET</returns>
        private TS_VIRTUALCHANNEL_CAPABILITYSET ParseCapsTypeVirtualChannel(byte[] data)
        {
            TS_VIRTUALCHANNEL_CAPABILITYSET set = RdpbcgrUtility.ToStruct<TS_VIRTUALCHANNEL_CAPABILITYSET>(data);

            // VCChunkSize is optional 
            // (RdpbcgrUtility.ToStruct() will provide it with an invalid value when it's absent)
            if (data.Length + Marshal.SizeOf(set.VCChunkSize) == Marshal.SizeOf(set))
            {
                // VCChunckSize is absent, correct its value to default 0
                set.VCChunkSize = 0;
            }
            else
            {
                // Check if data length is consistent with the decoded struct length
                VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            }
            return set;
        }


        /// <summary>
        /// Parse TS_RAIL_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_RAIL_CAPABILITYSET</returns>
        private TS_DRAWNINEGRIDCACHE_CAPABILITYSET ParseCapsTypeDrawNineGridCache(byte[] data)
        {
            TS_DRAWNINEGRIDCACHE_CAPABILITYSET set = new TS_DRAWNINEGRIDCACHE_CAPABILITYSET();

            // TS_DRAWNINEGRIDCACHE_CAPABILITYSET: rawData
            set.rawData = data;

            return set;
        }


        /// <summary>
        /// Parse TS_RAIL_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_RAIL_CAPABILITYSET</returns>
        private TS_DRAWGRIDPLUS_CAPABILITYSET ParseCapsTypeDrawGdiPlus(byte[] data)
        {
            TS_DRAWGRIDPLUS_CAPABILITYSET set = new TS_DRAWGRIDPLUS_CAPABILITYSET();

            // TS_DRAWGRIDPLUS_CAPABILITYSET: rawData
            set.rawData = data;

            return set;
        }


        /// <summary>
        /// Parse TS_RAIL_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_RAIL_CAPABILITYSET</returns>
        private TS_RAIL_CAPABILITYSET ParseCapsTypeRail(byte[] data)
        {
            TS_RAIL_CAPABILITYSET set = new TS_RAIL_CAPABILITYSET();

            // TS_RAIL_CAPABILITYSET: rawData
            set.rawData = data;

            return set;
        }


        /// <summary>
        /// Parse TS_WINDOWACTIVATION_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_WINDOWACTIVATION_CAPABILITYSET</returns>
        private TS_WINDOW_CAPABILITYSET ParseCapsTypeWindow(byte[] data)
        {
            TS_WINDOW_CAPABILITYSET set = new TS_WINDOW_CAPABILITYSET();

            // TS_WINDOW_CAPABILITYSET: rawData
            set.rawData = data;

            return set;
        }


        /// <summary>
        /// Parse TS_COMPDESK_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_COMPDESK_CAPABILITYSET</returns>
        private TS_COMPDESK_CAPABILITYSET ParseCapsTypeCompDesk(byte[] data)
        {
            TS_COMPDESK_CAPABILITYSET set = RdpbcgrUtility.ToStruct<TS_COMPDESK_CAPABILITYSET>(data);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }


        /// <summary>
        /// Parse TS_MULTIFRAGMENTUPDATE_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_MULTIFRAGMENTUPDATE_CAPABILITYSET</returns>
        private TS_MULTIFRAGMENTUPDATE_CAPABILITYSET ParseCapsTypeMultiFragmentUpdate(byte[] data)
        {
            TS_MULTIFRAGMENTUPDATE_CAPABILITYSET set =
                RdpbcgrUtility.ToStruct<TS_MULTIFRAGMENTUPDATE_CAPABILITYSET>(data);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }


        /// <summary>
        /// Parse TS_LARGE_POINTER_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_LARGE_POINTER_CAPABILITYSET</returns>
        private TS_LARGE_POINTER_CAPABILITYSET ParseCapsTypeLargePointer(byte[] data)
        {
            TS_LARGE_POINTER_CAPABILITYSET set = RdpbcgrUtility.ToStruct<TS_LARGE_POINTER_CAPABILITYSET>(data);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }


        /// <summary>
        /// Parse TS_SURFCMDS_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_SURFCMDS_CAPABILITYSET</returns>
        private TS_SURFCMDS_CAPABILITYSET ParseCapsTypeSurfaceCommands(byte[] data)
        {
            TS_SURFCMDS_CAPABILITYSET set = RdpbcgrUtility.ToStruct<TS_SURFCMDS_CAPABILITYSET>(data);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }


        /// <summary>
        /// Parse TS_BITMAPCODECS_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>TS_BITMAPCODECS_CAPABILITYSET</returns>
        private TS_BITMAPCODECS_CAPABILITYSET ParseCapsTypeBitmapCodecs(byte[] data)
        {
            int currentIndex = 0;
            TS_BITMAPCODECS_CAPABILITYSET set = new TS_BITMAPCODECS_CAPABILITYSET();

            // TS_BITMAPCODECS_CAPABILITYSET: capabilitySetType
            set.capabilitySetType = (capabilitySetType_Values)ParseUInt16(data, ref currentIndex, false);

            // TS_BITMAPCODECS_CAPABILITYSET: lengthCapability
            set.lengthCapability = ParseUInt16(data, ref currentIndex, false);

            // TS_BITMAPCODECS_CAPABILITYSET: supportedBitmapCodecs
            set.supportedBitmapCodecs = ParseTsBitmapCodecs(data, ref currentIndex);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, currentIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }


        /// <summary>
        /// Parse TS_BITMAPCODECS
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_BITMAPCODECS</returns>
        private TS_BITMAPCODECS ParseTsBitmapCodecs(byte[] data, ref int currentIndex)
        {
            TS_BITMAPCODECS codecs = new TS_BITMAPCODECS();

            // TS_BITMAPCODECS: bitmapCodecCount
            codecs.bitmapCodecCount = ParseByte(data, ref currentIndex);

            // TS_BITMAPCODECS: bitmapCodecArray
            codecs.bitmapCodecArray = new TS_BITMAPCODEC[codecs.bitmapCodecCount];
            for (int i = 0; i < codecs.bitmapCodecArray.Length; i++)
            {
                codecs.bitmapCodecArray[i] = ParseTsBitmapCodec(data, ref currentIndex);
            }

            return codecs;
        }


        /// <summary>
        /// Parse TS_BITMAPCODEC
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_BITMAPCODEC</returns>
        private TS_BITMAPCODEC ParseTsBitmapCodec(byte[] data, ref int currentIndex)
        {
            TS_BITMAPCODEC codec = new TS_BITMAPCODEC();

            // TS_BITMAPCODEC: codecGUID
            codec.codecGUID = ParseTsBitmapCodecGuid(data, ref currentIndex);

            // TS_BITMAPCODEC: codecID
            codec.codecID = ParseByte(data, ref currentIndex);

            // TS_BITMAPCODEC: codecPropertiesLength
            codec.codecPropertiesLength = ParseUInt16(data, ref currentIndex, false);

            // TS_BITMAPCODEC: codecProperties
            codec.codecProperties = GetBytes(data, ref currentIndex, (int)codec.codecPropertiesLength);

            return codec;
        }


        /// <summary>
        /// Parse TS_BITMAPCODEC_GUID
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parse</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_BITMAPCODEC_GUID</returns>
        private TS_BITMAPCODEC_GUID ParseTsBitmapCodecGuid(byte[] data, ref int currentIndex)
        {
            // Get TS_BITMAPCODEC_GUID data (16 bytes)
            TS_BITMAPCODEC_GUID guid = new TS_BITMAPCODEC_GUID();
            byte[] guidData = GetBytes(data, ref currentIndex, Marshal.SizeOf(guid));

            // Get TS_BITMAPCODEC_GUID
            guid = RdpbcgrUtility.ToStruct<TS_BITMAPCODEC_GUID>(guidData);
            return guid;
        }

        /// <summary>
        /// Parse TS_FRAME_ACKNOWLEDGE_CAPABILITYSET
        /// </summary>
        /// <param name="data">data to be parse</param>
        /// <returns>TS_FRAME_ACKNOWLEDGE_CAPABILITYSET</returns>
        private TS_FRAME_ACKNOWLEDGE_CAPABILITYSET PaseCapsTypeFrameAcknowledge(byte[] data)
        {
            TS_FRAME_ACKNOWLEDGE_CAPABILITYSET set = RdpbcgrUtility.ToStruct<TS_FRAME_ACKNOWLEDGE_CAPABILITYSET>(data);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }

        #endregion Capbility Sets
        #endregion Sub Field Parsers: Client Confirm Active PDU


        #region Sub Field Parsers: Synchronize PDU
        /// <summary>
        /// Parse TS_SYNCHRONIZE_PDU
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SYNCHRONIZE_PDU</returns>
        private TS_SYNCHRONIZE_PDU ParseTsSynchronizePdu(byte[] data, ref int currentIndex)
        {
            TS_SYNCHRONIZE_PDU pdu = new TS_SYNCHRONIZE_PDU();

            // TS_SYNCHRONIZE_PDU: shareDataHeader
            pdu.shareDataHeader = ParseTsShareDataHeader(data, ref currentIndex);

            // TS_SYNCHRONIZE_PDU: messageType 
            pdu.messageType = (messageType_Values)ParseUInt16(data, ref currentIndex, false);

            // TS_SYNCHRONIZE_PDU: targetUser
            pdu.targetUser = ParseUInt16(data, ref currentIndex, false);

            return pdu;
        }
        #endregion Sub Field Parsers: Synchronize PDU


        #region Sub Field Parsers: Control PDU Cooperate
        /// <summary>
        /// Parse TS_CONTROL_PDU
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_CONTROL_PDU</returns>
        private TS_CONTROL_PDU ParseTsControlPdu(byte[] data, ref int currentIndex)
        {
            TS_CONTROL_PDU pdu = new TS_CONTROL_PDU();

            // TS_CONTROL_PDU: shareDataHeader
            pdu.shareDataHeader = ParseTsShareDataHeader(data, ref currentIndex);

            // TS_CONTROL_PDU: action
            pdu.action = (action_Values)ParseUInt16(data, ref currentIndex, false);

            // TS_CONTROL_PDU: grantId
            pdu.grantId = ParseUInt16(data, ref currentIndex, false);

            // TS_CONTROL_PDU: controlId
            pdu.controlId = ParseUInt32(data, ref currentIndex, false);

            return pdu;
        }
        #endregion Sub Field Parsers: Control PDU Cooperate


        #region Sub Field Parsers: Client Persistent Key PDU
        /// <summary>
        /// Parse TS_BITMAPCACHE_PERSISTENT_LIST_PDU
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_BITMAPCACHE_PERSISTENT_LIST_PDU</returns>
        private TS_BITMAPCACHE_PERSISTENT_LIST_PDU ParseTsPersistentListPdu(byte[] data, ref int currentIndex)
        {
            TS_BITMAPCACHE_PERSISTENT_LIST_PDU pdu = new TS_BITMAPCACHE_PERSISTENT_LIST_PDU();

            // TS_CONTROL_PDU: shareDataHeader
            pdu.shareDataHeader = ParseTsShareDataHeader(data, ref currentIndex);

            pdu.numEntriesCache0 = ParseUInt16(data, ref currentIndex, false);
            pdu.numEntriesCache1 = ParseUInt16(data, ref currentIndex, false);
            pdu.numEntriesCache2 = ParseUInt16(data, ref currentIndex, false);
            pdu.numEntriesCache3 = ParseUInt16(data, ref currentIndex, false);
            pdu.numEntriesCache4 = ParseUInt16(data, ref currentIndex, false);
            pdu.totalEntriesCache0 = ParseUInt16(data, ref currentIndex, false);
            pdu.totalEntriesCache1 = ParseUInt16(data, ref currentIndex, false);
            pdu.totalEntriesCache2 = ParseUInt16(data, ref currentIndex, false);
            pdu.totalEntriesCache3 = ParseUInt16(data, ref currentIndex, false);
            pdu.totalEntriesCache4 = ParseUInt16(data, ref currentIndex, false);
            pdu.bBitMask = (bBitMask_Values)ParseByte(data, ref currentIndex);
            pdu.Pad2 = ParseByte(data, ref currentIndex);
            pdu.Pad3 = ParseUInt16(data, ref currentIndex, false);
            pdu.entries = new List<TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY>();
            while (currentIndex < data.Length)
            {
                pdu.entries.Add(ParseEntry(data, ref currentIndex));
            }

            return pdu;
        }

        /// <summary>
        /// Parse TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY</returns>
        private TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY ParseEntry(byte[] data, ref int currentIndex)
        {
            TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY entry = new TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY();
            entry.Key1 = ParseUInt32(data, ref currentIndex, false);
            entry.Key2 = ParseUInt32(data, ref currentIndex, false);

            return entry;
        }
        #endregion Sub Field Parsers: Client Persistent Key PDU


        #region Sub Field Parsers: Client Font List PDU
        /// <summary>
        /// Parse TS_FONT_LIST_PDU
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_FONT_LIST_PDU</returns>
        private TS_FONT_LIST_PDU ParseTsFontListPdu(byte[] data, ref int currentIndex)
        {
            TS_FONT_LIST_PDU pdu = new TS_FONT_LIST_PDU();
            pdu.shareDataHeader = ParseTsShareDataHeader(data, ref currentIndex);
            pdu.numberFonts = ParseUInt16(data, ref currentIndex, false);
            pdu.totalNumFonts = ParseUInt16(data, ref currentIndex, false);
            pdu.listFlags = ParseUInt16(data, ref currentIndex, false);
            pdu.entrySize = ParseUInt16(data, ref currentIndex, false);

            return pdu;
        }
        #endregion Sub Field Parsers: Client Font List PDU


        #region Sub Field Parsers: Client Shutdown Request PDU
        /// <summary>
        /// Parse TS_SHUTDOWN_REQ_PDU
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SHUTDOWN_REQ_PDU</returns>
        private TS_SHUTDOWN_REQ_PDU ParseTsShutdownReuqestPdu(byte[] data, ref int currentIndex)
        {
            TS_SHUTDOWN_REQ_PDU pdu = new TS_SHUTDOWN_REQ_PDU();
            pdu.shareDataHeader = ParseTsShareDataHeader(data, ref currentIndex);

            return pdu;
        }
        #endregion


        #region Sub Field Parsers: Slow Path Input Event PDU
        /// <summary>
        /// Parse TS_INPUT_EVENT
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_INPUT_EVENT</returns>
        private TS_INPUT_EVENT ParseSlowPathInputEvent(byte[] data, ref int currentIndex)
        {
            TS_INPUT_EVENT inputEvent = new TS_INPUT_EVENT();
            inputEvent.eventTime = ParseUInt32(data, ref currentIndex, false);
            inputEvent.messageType = (TS_INPUT_EVENT_messageType_Values)ParseUInt16(data, ref currentIndex, false);
            switch (inputEvent.messageType)
            {
                case TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_SYNC:
                    inputEvent.slowPathInputData = ParseSyncEvent(data, ref currentIndex);
                    break;
                case TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_SCANCODE:
                    inputEvent.slowPathInputData = ParseKeyboardEvent(data, ref currentIndex);
                    break;
                case TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_UNICODE:
                    inputEvent.slowPathInputData = ParseUniKeyboardEvent(data, ref currentIndex);
                    break;
                case TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_MOUSE:
                    inputEvent.slowPathInputData = ParseMouseEvent(data, ref currentIndex);
                    break;
                case TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_MOUSEX:
                    inputEvent.slowPathInputData = ParseExtMouseEvent(data, ref currentIndex);
                    break;
                case TS_INPUT_EVENT_messageType_Values.INPUT_EVENT_UNUSED:
                    inputEvent.slowPathInputData = ParseUnusedEvent(data, ref currentIndex);
                    break;
                default:
                    throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
            }

            return inputEvent;
        }

        /// <summary>
        /// Parse TS_KEYBOARD_EVENT
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_KEYBOARD_EVENT</returns>
        private TS_KEYBOARD_EVENT ParseKeyboardEvent(byte[] data, ref int currentIndex)
        {
            TS_KEYBOARD_EVENT eventData = new TS_KEYBOARD_EVENT();
            eventData.keyboardFlags = (keyboardFlags_Values)ParseUInt16(data, ref currentIndex, false);
            eventData.keyCode = ParseUInt16(data, ref currentIndex, false);
            eventData.pad2Octets = ParseUInt16(data, ref currentIndex, false);

            return eventData;
        }

        /// <summary>
        /// Parse TS_UNICODE_KEYBOARD_EVENT
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_UNICODE_KEYBOARD_EVENT</returns>
        private TS_UNICODE_KEYBOARD_EVENT ParseUniKeyboardEvent(byte[] data, ref int currentIndex)
        {
            TS_UNICODE_KEYBOARD_EVENT eventData = new TS_UNICODE_KEYBOARD_EVENT();
            eventData.keyboardFlags = (keyboardFlags_Values)ParseUInt16(data, ref currentIndex, false);
            eventData.unicodeCode = ParseUInt16(data, ref currentIndex, false);
            eventData.pad2Octets = ParseUInt16(data, ref currentIndex, false);

            return eventData;
        }

        /// <summary>
        /// Parse TS_POINTER_EVENT
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_POINTER_EVENT</returns>
        private TS_POINTER_EVENT ParseMouseEvent(byte[] data, ref int currentIndex)
        {
            TS_POINTER_EVENT eventData = new TS_POINTER_EVENT();
            eventData.pointerFlags = (pointerFlags_Values)ParseUInt16(data, ref currentIndex, false);
            eventData.xPos = ParseUInt16(data, ref currentIndex, false);
            eventData.yPos = ParseUInt16(data, ref currentIndex, false);

            return eventData;
        }

        /// <summary>
        /// Parse TS_POINTERX_EVENT
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_POINTERX_EVENT</returns>
        private TS_POINTERX_EVENT ParseExtMouseEvent(byte[] data, ref int currentIndex)
        {
            TS_POINTERX_EVENT eventData = new TS_POINTERX_EVENT();
            eventData.pointerFlags = (TS_POINTERX_EVENT_pointerFlags_Values)ParseUInt16(data, ref currentIndex, false);
            eventData.xPos = ParseUInt16(data, ref currentIndex, false);
            eventData.yPos = ParseUInt16(data, ref currentIndex, false);

            return eventData;
        }

        /// <summary>
        /// Parse TS_UNUSED_EVENT
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_UNUSED_EVENT</returns>
        private TS_UNUSED_EVENT ParseUnusedEvent(byte[] data, ref int currentIndex)
        {
            TS_UNUSED_EVENT eventData = new TS_UNUSED_EVENT();
            eventData.pad4Octets = ParseUInt32(data, ref currentIndex, false);
            eventData.pad2Octets = ParseUInt16(data, ref currentIndex, false);

            return eventData;
        }

        /// <summary>
        /// Parse TS_SYNC_EVENT
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SYNC_EVENT</returns>
        private TS_SYNC_EVENT ParseSyncEvent(byte[] data, ref int currentIndex)
        {
            TS_SYNC_EVENT eventData = new TS_SYNC_EVENT();
            eventData.pad2Octets = ParseUInt16(data, ref currentIndex, false);
            eventData.toggleFlags = (toggleFlags_Values)ParseUInt32(data, ref currentIndex, false);

            return eventData;
        }
        #endregion


        #region Sub Field Parsers: Controlling Server Graphics Output
        /// <summary>
        /// Parse TS_RECTANGLE16
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_RECTANGLE16</returns>
        private TS_RECTANGLE16 ParseInclusiveRect(byte[] data, ref int currentIndex)
        {
            TS_RECTANGLE16 rectData = new TS_RECTANGLE16();
            rectData.left = ParseUInt16(data, ref currentIndex, false);
            rectData.top = ParseUInt16(data, ref currentIndex, false);
            rectData.right = ParseUInt16(data, ref currentIndex, false);
            rectData.bottom = ParseUInt16(data, ref currentIndex, false);

            return rectData;
        }

        /// <summary>
        /// Parse TS_REFRESH_RECT_PDU
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_REFRESH_RECT_PDU</returns>
        private TS_REFRESH_RECT_PDU ParseTsRefreshRectPdu(byte[] data, ref int currentIndex)
        {
            TS_REFRESH_RECT_PDU pdu = new TS_REFRESH_RECT_PDU();
            pdu.shareDataHeader = ParseTsShareDataHeader(data, ref currentIndex);
            pdu.numberOfAreas = ParseByte(data, ref currentIndex);
            pdu.pad3Octects = GetBytes(data, ref currentIndex, 3);
            pdu.areasToRefresh = new Collection<TS_RECTANGLE16>();
            for (int i = 0; i < pdu.numberOfAreas; ++i)
            {
                pdu.areasToRefresh.Add(ParseInclusiveRect(data, ref currentIndex));
            }

            return pdu;
        }

        /// <summary>
        /// Parse TS_SUPPRESS_OUTPUT_PDU
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SUPPRESS_OUTPUT_PDU</returns>
        private TS_SUPPRESS_OUTPUT_PDU ParseTsSuppressOutputPdu(byte[] data, ref int currentIndex)
        {
            TS_SUPPRESS_OUTPUT_PDU pdu = new TS_SUPPRESS_OUTPUT_PDU();
            pdu.shareDataHeader = ParseTsShareDataHeader(data, ref currentIndex);
            pdu.allowDisplayUpdates = (AllowDisplayUpdates_SUPPRESS_OUTPUT)ParseByte(data, ref currentIndex);
            pdu.pad3Octects = GetBytes(data, ref currentIndex, 3);
            if (pdu.allowDisplayUpdates == AllowDisplayUpdates_SUPPRESS_OUTPUT.ALLOW_DISPLAY_UPDATES)
            {
                pdu.desktopRect = ParseInclusiveRect(data, ref currentIndex);
            }

            return pdu;
        }
        #endregion Sub Field Parsers: Controlling Server Graphics Output

        #region Sub Field Parsers: Virtual Channel PDU
        /// <summary>
        /// Parse CHANNEL_PDU_HEADER
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>CHANNEL_PDU_HEADER</returns>
        CHANNEL_PDU_HEADER ParseChannelPduHeader(byte[] data, ref int currentIndex)
        {
            CHANNEL_PDU_HEADER header = new CHANNEL_PDU_HEADER();

            // CHANNEL_PDU_HEADER: length
            header.length = ParseUInt32(data, ref currentIndex, false);

            // CHANNEL_PDU_HEADER: flags
            header.flags = (CHANNEL_PDU_HEADER_flags_Values)ParseUInt32(data, ref currentIndex, false);

            return header;
        }
        #endregion Sub Field Parsers: Virtual Channel PDU


        #region Sub Field Parsers: Fast-Path Update PDU
        /// <summary>
        /// Parse TS_FP_FIPS_INFO
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_FP_FIPS_INFO</returns>
        private TS_FP_FIPS_INFO ParseTsFpFipsInfo(byte[] data, ref int currentIndex)
        {
            TS_FP_FIPS_INFO info = new TS_FP_FIPS_INFO();

            // TS_FP_FIPS_INFO: length
            info.length = (TS_FP_FIPS_INFO_length_Values)ParseUInt16(data, ref currentIndex, false);

            // TS_FP_FIPS_INFO: version
            info.version = ParseByte(data, ref currentIndex);

            // TS_FP_FIPS_INFO: padlen
            info.padlen = ParseByte(data, ref currentIndex);

            return info;
        }


        /// <summary>
        /// Parse TS_FP_UPDATE array
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>parsed TS_FP_UPDATE array</returns>
        private Collection<TS_FP_INPUT_EVENT> ParseTsFpInputEvents(byte[] data, ref int currentIndex)
        {
            Collection<TS_FP_INPUT_EVENT> collectionEvents = new Collection<TS_FP_INPUT_EVENT>();

            while (currentIndex < data.Length)
            {
                TS_FP_INPUT_EVENT inputEvent = new TS_FP_INPUT_EVENT();

                byte eventHeader = ParseByte(data, ref currentIndex);
                byte eventFlags;
                eventCode_Values eventCode;
                GetFpEventHeaderInfo(eventHeader, out eventFlags, out eventCode);
                inputEvent.eventHeader.eventFlagsAndCode = eventHeader;

                switch (eventCode)
                {
                    case eventCode_Values.FASTPATH_INPUT_EVENT_MOUSE:
                        inputEvent.eventData = ParseTsFpInputMouse(data, ref currentIndex);
                        break;
                    case eventCode_Values.FASTPATH_INPUT_EVENT_MOUSEX:
                        inputEvent.eventData = ParseTsFpInputMousex(data, ref currentIndex);
                        break;
                    case eventCode_Values.FASTPATH_INPUT_EVENT_SCANCODE:
                        inputEvent.eventData = ParseTsFpInputScancode(data, ref currentIndex);
                        break;
                    case eventCode_Values.FASTPATH_INPUT_EVENT_SYNC:
                        inputEvent.eventData = null;
                        break;
                    case eventCode_Values.FASTPATH_INPUT_EVENT_UNICODE:
                        inputEvent.eventData = ParseTsFpInputUnicode(data, ref currentIndex);
                        break;
                    case eventCode_Values.FASTPATH_INPUT_EVENT_QOE_TIMESTAMP:
                        inputEvent.eventData = ParseTsFpInputQoETimeStamp(data, ref currentIndex);
                        break;
                    default:
                        throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
                }
                collectionEvents.Add(inputEvent);
            }

            return collectionEvents;
        }


        #region Fast-Path Update Attribute Parsers
        /// <summary>
        /// Parse TS_FP_POINTER_EVENT 
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>parsed TS_FP_POINTERX_EVENT</returns>
        private TS_FP_POINTER_EVENT ParseTsFpInputMouse(byte[] data, ref int currentIndex)
        {
            TS_FP_POINTER_EVENT mouse = new TS_FP_POINTER_EVENT();

            mouse.pointerFlags = ParseUInt16(data, ref currentIndex, false);
            mouse.xPos = ParseUInt16(data, ref currentIndex, false);
            mouse.yPos = ParseUInt16(data, ref currentIndex, false);

            return mouse;
        }

        /// <summary>
        /// Parse TS_FP_POINTERX_EVENT
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>parsed TS_FP_POINTERX_EVENT</returns>
        private TS_FP_POINTERX_EVENT ParseTsFpInputMousex(byte[] data, ref int currentIndex)
        {
            TS_FP_POINTERX_EVENT mousex = new TS_FP_POINTERX_EVENT();

            mousex.pointerFlags = ParseUInt16(data, ref currentIndex, false);
            mousex.xPos = ParseUInt16(data, ref currentIndex, false);
            mousex.yPos = ParseUInt16(data, ref currentIndex, false);

            return mousex;
        }

        /// <summary>
        /// Parse TS_FP_KEYBOARD_EVENT
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>parsed TS_FP_KEYBOARD_EVENT</returns>
        private TS_FP_KEYBOARD_EVENT ParseTsFpInputScancode(byte[] data, ref int currentIndex)
        {
            TS_FP_KEYBOARD_EVENT scancode = new TS_FP_KEYBOARD_EVENT();

            scancode.keyCode = ParseByte(data, ref currentIndex);

            return scancode;
        }

        /// <summary>
        /// Parse TS_FP_UNICODE_KEYBOARD_EVENT
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>parsed TS_FP_UNICODE_KEYBOARD_EVENTy</returns>
        private TS_FP_UNICODE_KEYBOARD_EVENT ParseTsFpInputUnicode(byte[] data, ref int currentIndex)
        {
            TS_FP_UNICODE_KEYBOARD_EVENT unicode = new TS_FP_UNICODE_KEYBOARD_EVENT();

            unicode.unicodeCode = ParseUInt16(data, ref currentIndex, false);

            return unicode;
        }

        /// <summary>
        /// Parse TS_FP_QOETIMESTAMP_EVENT
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>parsed TS_FP_QOETIMESTAMP_EVENT</returns>
        private TS_FP_QOETIMESTAMP_EVENT ParseTsFpInputQoETimeStamp(byte[] data, ref int currentIndex)
        {
            TS_FP_QOETIMESTAMP_EVENT qoeTimeStamp = new TS_FP_QOETIMESTAMP_EVENT();

            qoeTimeStamp.timestamp = ParseUInt32(data, ref currentIndex, false);

            return qoeTimeStamp;
        }
        #endregion


        #region Fast-Path Update Parsers' helper functions
        /// <summary>
        /// Decrypt Fast-path Update Data
        /// </summary>
        /// <param name="remainData">data to be decrypted</param>
        /// <param name="signatureData">signature data</param>
        /// <param name="isSalted">if the MAC signature was created by "salted MAC generation"</param>
        /// <returns>decrypted Fast-path Update Data</returns>
        private byte[] DecryptFastPathInputData(RdpbcgrServerSessionContext serverSessionContext, byte[] remainData, byte[] signatureData, bool isSalted)
        {
            if (null == signatureData)
            {
                // No need to decrypt, return directly
                return remainData;
            }
            else
            {
                // Decryption
                byte[] decryptedData = null;
                if (!serverSessionContext.ServerDecrypt(remainData, signatureData, isSalted, out decryptedData))
                {
                    // Decryptioin failed
                    throw new FormatException(ConstValue.ERROR_MESSAGE_DECRYPTION_FAILED);
                }
                return decryptedData;
            }
        }


        /// <summary>
        /// Get information from Fast-path Update Header
        /// </summary>
        /// <param name="updateHeader">update header</param>
        /// <param name="updateCode">update code</param>
        /// <param name="fragmentation">fragmentation</param>
        /// <param name="compression">compression</param>
        private void GetFpEventHeaderInfo(
          byte eventHeader,
          out byte eventFlags,
          out eventCode_Values eventCode)
        {

            byte flags = (byte)(eventHeader & 0x1f);
            eventFlags = flags;

            byte code = (byte)((eventHeader & 0xe0) >> 5);
            eventCode = (eventCode_Values)code;
        }
        #endregion Fast-Path Update Parsers' helper functions
        #endregion Sub Field Parsers: Fast-Path Update PDU
        #endregion Private Methods: PDU Sub Field Parsers


        #region Private Methods: PDU Decoder Switches
        #region Switch level 1
        /// <summary>
        /// Switch Decode MCS PDU
        /// </summary>
        /// <param name="serverSessionContext">the server session context</param>
        /// <param name="data">data to be parsed</param>
        /// <returns>decode MCS PDU</returns>
        private StackPacket SwitchDecodeMcsPDU(RdpbcgrServerSessionContext serverSessionContext, byte[] data)
        {
            // Check data length
            if (ConstValue.MCS_CONNECT_INITIAL_PDU_INDICATOR_INDEX >= data.Length)
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_DATA_INDEX_OUT_OF_RANGE);
            }

            // Decode by MCS PDU Type
            StackPacket pdu = null;
            byte mcsPduType = data[ConstValue.MCS_CONNECT_INITIAL_PDU_INDICATOR_INDEX];
            if (ConstValue.MCS_CONNECT_INITIAL_PDU_INDICATOR_VALUE == mcsPduType)
            {
                // Decode MCS Connect Response PDU
                pdu = DecodeMcsConnectInitialPDU(data);
            }
            else
            {
                // Decode MCS Domain PDU
                pdu = SwitchDecodeMcsDomainPDU(serverSessionContext, data);
            }
            return pdu;
        }
        #endregion Switch Level 1


        #region Switch Level 2
        /// <summary>
        /// Switch Decode MCS Domain PDU
        /// </summary>
        /// <param name="serverSessionContext">the server session context</param>
        /// <param name="data">data to be parsed</param>
        /// <returns>decoded MCS Domain PDU</returns>
        private StackPacket SwitchDecodeMcsDomainPDU(RdpbcgrServerSessionContext serverSessionContext, byte[] data)
        {
            // Skip TpktHeader and X224Data
            int tempIndex = ConstValue.TPKT_HEADER_AND_X224_DATA_LENGTH;

            // Decode Domain MCS PDU
            DomainMCSPDU domainPdu = ParseMcsDomainPdu(data, ref tempIndex);

            // Switch decoders by pdu element name 
            StackPacket pdu = null;
            switch (domainPdu.ElemName)
            {
                // Erect Domain Request PDU
                case ConstValue.MCS_DOMAIN_PDU_NAME_ERECT_DOMAIN_REQUEST:
                    pdu = DecodeMcsErectDomainRequestPDU(data);
                    break;

                // Attach User Request PDU
                case ConstValue.MCS_DOMAIN_PDU_NAME_ATTACH_USER_REQUEST:
                    pdu = DecodeMcsAttachUserRequestPDU(data);
                    break;

                // Channel Join Request PDU
                case ConstValue.MCS_DOMAIN_PDU_NAME_CHANNEL_JOIN_REQUEST:
                    pdu = DecodeMcsChannelJoinRequestPDU(data);
                    break;

                // Send Data Request PDU
                case ConstValue.MCS_DOMAIN_PDU_NAME_SEND_DATA_REQUEST:
                    pdu = DecodeMcsSendDataRequestPDU(serverSessionContext, data, domainPdu);
                    break;

                // Disconnect Provider Ultimatum PDU
                case ConstValue.MCS_DOMAIN_PDU_NAME_DISCONNECT_PROVIDER_ULTIMATUM:
                    pdu = DecodeDisconnectProviderUltimatumPDU(data);
                    break;

                default:
                    throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
            }
            return pdu;
        }
        #endregion Switch Level 2


        #region Switch Level 3
        /// <summary>
        /// Decode MCS Send Data Request PDU
        /// </summary>
        /// <param name="serverSessionContext">the server session context</param>
        /// <param name="data">data to be parsed</param>
        /// <param name="domainPdu">Mcs Domain PDU</param>
        /// <returns>decoded MCS Send Data Request PDU</returns>
        private StackPacket DecodeMcsSendDataRequestPDU(RdpbcgrServerSessionContext serverSessionContext, byte[] data, DomainMCSPDU domainPdu)
        {
            SendDataRequest indication = (SendDataRequest)domainPdu.GetData();
            byte[] userData = indication.userData.ByteArrayValue;

            bool isSecurityExchange;
            bool isClientInfo;
            bool isAutoDetectResponsePDU;
            bool isMultitransportErrorPDU;

            // Get Security Header Type
            SecurityHeaderType securityHeaderType = GetSecurityHeaderTypeByContext(serverSessionContext);

            int i = 0;
            if (userData.Length == ParseUInt16(userData, ref i, false)
                || (securityHeaderType == SecurityHeaderType.None && serverSessionContext.IOChannelId != indication.channelId.Value && serverSessionContext.McsMsgChannelId != indication.channelId.Value))
            {
                // the packet not transmitted in IO Channel cannot be Security Exchange PDU or Client Info PDU, so set all to false.
                isSecurityExchange = false;
                isClientInfo = false;
                isAutoDetectResponsePDU = false;
                isMultitransportErrorPDU = false;
            }
            else
            {
                int tempIndex = 0;
                TS_SECURITY_HEADER header = ParseTsSecurityHeader(userData, ref tempIndex, SecurityHeaderType.Basic);

                // Check if PDU is Security Exchange PDU
                isSecurityExchange = IsFlagExist((UInt16)header.flags,
                    (UInt16)TS_SECURITY_HEADER_flags_Values.SEC_EXCHANGE_PKT);

                //Check if PDU is Client Info PDU
                isClientInfo = IsFlagExist((UInt16)header.flags, (UInt16)TS_SECURITY_HEADER_flags_Values.SEC_INFO_PKT);

                //check if PDU is Auto Detect Response PDU
                isAutoDetectResponsePDU = IsFlagExist((UInt16)header.flags, (UInt16)TS_SECURITY_HEADER_flags_Values.SEC_AUTODETECT_RSP);

                //check if PDU is Client Initiate Multitransport Error PDU
                isMultitransportErrorPDU = IsFlagExist((UInt16)header.flags, (UInt16)TS_SECURITY_HEADER_flags_Values.SEC_TRANSPORT_RSP);
            }

            if (isSecurityExchange)
            {
                securityHeaderType = SecurityHeaderType.Basic;
                byte[] decryptedUserData = DecryptSendDataRequest(serverSessionContext, userData, securityHeaderType);
                return DecodeSecurityExchangePdu(serverSessionContext, data, decryptedUserData, securityHeaderType);
            }
            else if (isClientInfo)
            {
                if (serverSessionContext.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE &&
                    serverSessionContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE)
                {
                    securityHeaderType = SecurityHeaderType.Basic;
                }
                else if (serverSessionContext.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS &&
                    serverSessionContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
                {
                    securityHeaderType = SecurityHeaderType.Fips;
                }
                else
                {
                    securityHeaderType = SecurityHeaderType.NonFips;
                }
                byte[] decryptedUserData = DecryptSendDataRequest(serverSessionContext, userData, securityHeaderType);
                return DecodeClientInfoPdu(data, decryptedUserData, securityHeaderType);
            }
            else if (isAutoDetectResponsePDU)
            {
                if (serverSessionContext.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE &&
                    serverSessionContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE)
                {
                    securityHeaderType = SecurityHeaderType.Basic;
                }
                else if (serverSessionContext.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS &&
                    serverSessionContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
                {
                    securityHeaderType = SecurityHeaderType.Fips;
                }
                else
                {
                    securityHeaderType = SecurityHeaderType.NonFips;
                }
                byte[] decryptedUserData = DecryptSendDataRequest(serverSessionContext, userData, securityHeaderType);
                return DecodeClientAutoDetectResponsePDU(data, decryptedUserData, securityHeaderType);
            }
            else if (isMultitransportErrorPDU)
            {
                if (serverSessionContext.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_NONE &&
                    serverSessionContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_NONE)
                {
                    securityHeaderType = SecurityHeaderType.Basic;
                }
                else if (serverSessionContext.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS &&
                    serverSessionContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
                {
                    securityHeaderType = SecurityHeaderType.Fips;
                }
                else
                {
                    securityHeaderType = SecurityHeaderType.NonFips;
                }
                byte[] decryptedUserData = DecryptSendDataRequest(serverSessionContext, userData, securityHeaderType);
                return DecodeClientInitiateMultitransportResponsePDU(data, decryptedUserData, securityHeaderType);
            }
            else
            {
                if (!serverSessionContext.IsClientToServerEncrypted)
                    securityHeaderType = SecurityHeaderType.Basic;
                // Get decrypted user data
                byte[] decryptedUserData = DecryptSendDataRequest(serverSessionContext, userData, securityHeaderType);

                // Check channel ID (IO Channel ID/Virual Channel ID)
                if (serverSessionContext.IOChannelId != indication.channelId.Value)
                {
                    // Decode Virtual Channel PDU
                    return DecodeVirtualChannelPDU(data, decryptedUserData, securityHeaderType);
                }

                else
                {
                    // Decode other Send Data Indication PDUs
                    return SwitchDecodeMcsSendDataRequestPDU(data, decryptedUserData, securityHeaderType);
                }
            }
        }
        #endregion Switch Level 3


        #region Switch Level 4
        /// <summary>
        /// Switch Decode MCS Send Data Request PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data</param>
        /// <param name="securityHeaderType">security header type</param>
        /// <returns>decoded MCS Send Data Request PDU</returns>
        private StackPacket SwitchDecodeMcsSendDataRequestPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType securityHeaderType)
        {
            // Parse "share control header"
            int currentIndex = 0;
            TS_SHARECONTROLHEADER header = ParseTsShareControlHeader(decryptedUserData, ref currentIndex);
            ShareControlHeaderType shareControlHeaderType = GetShareControlHeaderType(header);

            // Switch decoder by share control header type
            StackPacket pdu = null;
            switch (shareControlHeaderType)
            {
                // Confirm Active PDU
                case ShareControlHeaderType.PDUTYPE_CONFIRMACTIVEPDU:
                    pdu = DecodeConfirmActivePDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Data PDU
                case ShareControlHeaderType.PDUTYPE_DATAPDU:
                    pdu = SwitchDecodeMcsDataPDU(data, decryptedUserData, securityHeaderType);
                    break;

                case (ShareControlHeaderType)9:
                    pdu = DecodeClientInfoPdu(data, decryptedUserData, securityHeaderType);
                    break;

                default:
                    throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);

            }
            return pdu;
        }
        #endregion Switch Level 4


        #region Switch Level 5
        /// <summary>
        /// Switch Decode MCS Data PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data</param>
        /// <param name="securityHeaderType">security header type</param>
        /// <returns>decoded MCS Data PDU</returns>
        private StackPacket SwitchDecodeMcsDataPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType securityHeaderType)
        {
            // Parser index
            int currentIndex = 0;

            // Share data header
            TS_SHAREDATAHEADER dataHeader = ParseTsShareDataHeader(decryptedUserData, ref currentIndex);

            // Switch decoder by pduType2
            StackPacket pdu = null;
            switch (dataHeader.pduType2)
            {
                // Control PDU
                case pduType2_Values.PDUTYPE2_CONTROL:
                    pdu = DecodeControlPDU(data, decryptedUserData, securityHeaderType);
                    break;

                case pduType2_Values.PDUTYPE2_INPUT:
                    pdu = DecodeSlowPathInputEventPDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Synchronize PDU
                case pduType2_Values.PDUTYPE2_SYNCHRONIZE:
                    pdu = DecodeSynchronizePDU(data, decryptedUserData, securityHeaderType);
                    break;

                case pduType2_Values.PDUTYPE2_REFRESH_RECT:
                    pdu = DecodeRefreshRectPDU(data, decryptedUserData, securityHeaderType);
                    break;

                case pduType2_Values.PDUTYPE2_SUPPRESS_OUTPUT:
                    pdu = DecodeSuppressOutputPDU(data, decryptedUserData, securityHeaderType);
                    break;

                case pduType2_Values.PDUTYPE2_SHUTDOWN_REQUEST:
                    pdu = DecodeShutdownRequestPDU(data, decryptedUserData, securityHeaderType);
                    break;

                case pduType2_Values.PDUTYPE2_FONTLIST:
                    pdu = DecodeFontListPDU(data, decryptedUserData, securityHeaderType);
                    break;

                case pduType2_Values.PDUTYPE2_BITMAPCACHE_PERSISTENT_LIST:
                    pdu = DecodePersistentKeyListPDU(data, decryptedUserData, securityHeaderType);
                    break;

                case pduType2_Values.PDUTYPE2_FRAME_ACKNOWLEDGE:
                    pdu = DecodeFrameAcknowledgePDU(data, decryptedUserData, securityHeaderType);
                    break;

                default:
                    throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
            }
            return pdu;
        }
        #endregion Switch Level 5
        #endregion Private Methods: PDU Decoder Switches


        #region Private Methods: Decoder Callback
        /// <summary>
        /// Decode RDPBCGR PDUs from the received message bytes
        /// </summary>
        /// <param name="endPoint">the endpoint from which the message bytes are received.</param>
        /// <param name="receivedBytes">the received message bytes to be decoded.</param>
        /// <param name="consumedLength">the length of message bytes consumed by decoder.</param>
        /// <param name="expectedLength">the length of message bytes the decoder expects to receive.</param>
        /// <returns>the stack packets decoded from the received message bytes.</returns>
        internal StackPacket[] DecodePacketCallback(
            object endPoint,
            byte[] receivedBytes,
            out int consumedLength,
            out int expectedLength)
        {
            StackPacket pdu = null;
            expectedLength = 0;
            consumedLength = 0;

            for (int i = 0; i < server.ServerContext.SessionContexts.Count; i++)
            {
                if (endPoint == server.ServerContext.SessionContexts[i].Identity)
                {
                    // Get bytes for only one packet
                    byte[] packetData = GetPacket(receivedBytes, server.ServerContext.SessionContexts[i]);
                    if (null == packetData)
                    {
                        // Received bytes does not contain enough data
                        consumedLength = 0;
                        return null;
                    }

                    consumedLength = packetData.Length;

                    try
                    {
                        // ETW Provider Dump Message
                        string messageName;
                        if (ConstValue.SLOW_PATH_PDU_INDICATOR_VALUE == packetData[ConstValue.SLOW_PATH_PDU_INDICATOR_INDEX])
                        {
                            // Slow-Path
                            messageName = "RDPBCGR:ReceivedSlowPathPDU";
                        }
                        else
                        {
                            // Fast-Path
                            messageName = "RDPBCGR:ReceivedFastPathPDU";
                        }
                        ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer0, "Received Original RDPBCGR Message", packetData);

                        pdu = DecodePdu(server.ServerContext.SessionContexts[i], packetData);
                    }
                    catch (FormatException e)
                    {
                        pdu = new ErrorPdu(e, packetData);
                    }

                    // Update client context and client
                    server.ServerContext.SessionContexts[i].UpdateContext(pdu);
                    server.CheckDecryptionCount(server.ServerContext.SessionContexts[i]);

                    break;
                }
                else
                {
                    pdu = null;
                }
            }
            return new StackPacket[] { pdu };
        }


        /// <summary>
        /// Get a complete packet buffer from received bytes
        /// </summary>
        /// <param name="receivedBytes">received bytes</param>
        /// <param name="sessionContext">session context</param>
        /// <returns>data buffer contains a complete packet</returns>
        private byte[] GetPacket(byte[] receivedBytes, RdpbcgrServerSessionContext sessionContext)
        {
            if (receivedBytes == null || receivedBytes.Length == 0)
            {
                return null;
            }

            if (sessionContext.IsAuthenticatingRDSTLS)
            {
                // for RDSTLS, check whether whole PDU is ready
                int consumedBytes = GetLengthOfRDSTLSAuthenticationPDU(receivedBytes);
                if (consumedBytes == 0)
                {
                    // incomplete PDU
                    return null;
                }
                var buffer = new byte[consumedBytes];
                Array.Copy(receivedBytes, buffer, consumedBytes);
                return buffer;
            }
            else
            {
                // for non-RDSTLS, check X.224 header for PDUs

                // Get packet length according to PDU type (slow-path/fast-path)
                int packetLength = 0;
                if (ConstValue.SLOW_PATH_PDU_INDICATOR_VALUE == receivedBytes[ConstValue.SLOW_PATH_PDU_INDICATOR_INDEX])
                {
                    // Slow-path PDU
                    if (receivedBytes.Length < Marshal.SizeOf(typeof(TpktHeader)))
                    {
                        // the buffer doesn't contain the complete slow-path pdu header
                        return null;
                    }

                    // receivedBytes[2] and receivedBytes[3] make the length field of TpktHeader
                    int tempIndex = 2;
                    packetLength = ParseUInt16(receivedBytes, ref tempIndex, true);
                }
                else
                {
                    // Fast-path PDU
                    if (1 == receivedBytes.Length)
                    {
                        // the buffer doesn't contain the complete fast-path pdu header
                        return null;
                    }

                    // "length2"(receivedBytes[2]) does not exists in received data
                    // but "length1"(receivedBytes[1]) indicates that length2 exists
                    if ((2 == receivedBytes.Length)
                        && ((receivedBytes[1] & ConstValue.MOST_SIGNIFICANT_BIT_FILTER) != receivedBytes[1]))
                    {
                        return null;
                    }

                    // receivedBytes[1] and receivedBytes[2] are the corresponding
                    // "length1" and "length2" fields in TS_FP_UPDATE_PDU
                    packetLength = RdpbcgrUtility.CalculateFpUpdatePduLength(receivedBytes[1], receivedBytes[2]);
                }

                // Received bytes does not contain enough data
                if (receivedBytes.Length < packetLength)
                {
                    return null;
                }

                // Copy data to buffer
                byte[] buffer = new byte[packetLength];
                Array.Copy(receivedBytes, buffer, packetLength);
                return buffer;
            }
        }

        #endregion Private Methods: Decoder Callback


        #region Public Methods: PDU Decoder Entrance
        /// <summary>
        /// Decode PDU 
        /// (entrance method for decoders)
        /// </summary>
        /// <param name="serverSessionContext">the server session context</param>
        /// <param name="data">data to be parsed</param>
        /// <returns>decoded PDU</returns>
        public StackPacket DecodePdu(RdpbcgrServerSessionContext serverSessionContext, byte[] data)
        {
            // Check data length
            if (null == data)
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_DATA_NULL_REF);
            }
            if (0 == data.Length)
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_DATA_INDEX_OUT_OF_RANGE);
            }

            // Check slow-path/fast-path type
            StackPacket pdu = null;

            if (serverSessionContext.IsAuthenticatingRDSTLS)
            {
                pdu = SwitchRDSTLSAuthenticationPDU(serverSessionContext, data);
            }
            else
            {
                if (ConstValue.SLOW_PATH_PDU_INDICATOR_VALUE == data[ConstValue.SLOW_PATH_PDU_INDICATOR_INDEX])
                {
                    // Slow-Path Situation
                    if (data.Length <= ConstValue.X224_TPDU_TYPE_INDICATOR_INDEX)
                    {
                        throw new FormatException(ConstValue.ERROR_MESSAGE_DATA_INDEX_OUT_OF_RANGE);
                    }

                    X224_TPDU_TYPE x224Type = (X224_TPDU_TYPE)data[ConstValue.X224_TPDU_TYPE_INDICATOR_INDEX];
                    switch (x224Type)
                    {
                        // X224 Connection Confirm PDU
                        case X224_TPDU_TYPE.ConnectionRequest:
                            pdu = DecodeX224ConnectionRequestPDU(data);
                            break;

                        // MCS PDU
                        case X224_TPDU_TYPE.Data:
                            pdu = SwitchDecodeMcsPDU(serverSessionContext, data);
                            break;

                        default:
                            throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
                    }
                }
                else
                {
                    // Fast-Path Situation
                    pdu = DecodeTsFpInputPDU(serverSessionContext, data);
                }
            }

            if (pdu == null)
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_UNRECOGNIZED_PDU);
            }

            return pdu;
        }
        #endregion Public Methods: PDU Decoder Entrance


        #region Public Methods: PDU Decoders
        #region  PDU Decoders: 10 types of PDU in connection sequence
        /// <summary>
        /// Decode X.224 Connection Request PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>decoded X.224 Connection Request PDU</returns>
        public StackPacket DecodeX224ConnectionRequestPDU(byte[] data)
        {
            // initialize
            int currentIndex = 0;

            // TpktHeader
            TpktHeader tpktHeader = ParseTpktHeader(data, ref currentIndex);

            // X224Crq
            X224Crq x224Crq = ParseX224Crq(data, ref currentIndex);

            Client_X_224_Connection_Request_Pdu requestPdu = new Client_X_224_Connection_Request_Pdu();
            requestPdu.tpktHeader = tpktHeader;
            requestPdu.x224Crq = x224Crq;

            if (tpktHeader.length > currentIndex)
            {
                //routingToken or cookie                 
                ParseRoutingTokenOrCookie(data, ref requestPdu.routingToken, ref requestPdu.cookie, ref currentIndex);

                if (tpktHeader.length > currentIndex)
                {
                    requestPdu.rdpNegData = ParseRdpNegReq(data, ref currentIndex);

                    if (requestPdu.rdpNegData.flags.HasFlag(RDP_NEG_REQ_flags_Values.CORRELATION_INFO_PRESENT) && tpktHeader.length > currentIndex)
                    {
                        requestPdu.rdpCorrelationInfo = ParseRdpNegCorrelationInfo(data, ref currentIndex);
                    }

                }
                else
                {
                    requestPdu.rdpNegData = null;
                }
            }

            // Check if data length exceeded expectation
            if (data.Length != currentIndex)
            {

                server.AddWarning("{0} bytes of extra data found when decoding X224 Connection Request PDU", data.Length - currentIndex);
            }

            // temprory command out for winblue behavior change
            // VerifyDataLength(data.Length, currentIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return requestPdu;
        }

        /// <summary>
        /// Decode MCS Connect Initial PDU with GCC Conference Create Initial
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>Decoded MCS Connect Initial PDU with GCC Conference Create Initial</returns>
        public StackPacket DecodeMcsConnectInitialPDU(byte[] data)
        {
            // initialize
            int currentIndex = 0;
            Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request pdu =
                new Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request();

            // McsConnectResponse: TpktHeader
            pdu.tpktHeader = ParseTpktHeader(data, ref currentIndex);

            // McsConnectResponse: X224
            pdu.x224Data = ParseX224Data(data, ref currentIndex);

            // T125 Data: decode McsConnectResponse
            int t125DataLength = data.Length - currentIndex;
            if (t125DataLength <= 0)
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_DATA_INDEX_OUT_OF_RANGE);
            }
            byte[] t125Data = new byte[t125DataLength];
            Array.Copy(data, currentIndex, t125Data, 0, t125Data.Length);
            Connect_Initial mcsConnectInitial = new Connect_Initial();
            Asn1DecodingBuffer decodeBuffer = new Asn1DecodingBuffer(t125Data);
            mcsConnectInitial.BerDecode(decodeBuffer);

            // McsConnectResponse:result
            pdu.mcsCi.targetParameters.maxChannelIds = (long)mcsConnectInitial.targetParameters.maxChannelIds.Value;
            pdu.mcsCi.targetParameters.maxHeight = (long)mcsConnectInitial.targetParameters.maxHeight.Value;
            pdu.mcsCi.targetParameters.maxMcsPduSize = (long)mcsConnectInitial.targetParameters.maxMCSPDUsize.Value;
            pdu.mcsCi.targetParameters.maxTokenIds = (long)mcsConnectInitial.targetParameters.maxTokenIds.Value;
            pdu.mcsCi.targetParameters.maxUserIds = (long)mcsConnectInitial.targetParameters.maxUserIds.Value;
            pdu.mcsCi.targetParameters.minThroughput = (long)mcsConnectInitial.targetParameters.minThroughput.Value;
            pdu.mcsCi.targetParameters.numPriorities = (long)mcsConnectInitial.targetParameters.numPriorities.Value;
            pdu.mcsCi.targetParameters.protocolVersion = (long)mcsConnectInitial.targetParameters.protocolVersion.Value;

            pdu.mcsCi.minimumParameters.maxChannelIds = (long)mcsConnectInitial.minimumParameters.maxChannelIds.Value;
            pdu.mcsCi.minimumParameters.maxHeight = (long)mcsConnectInitial.minimumParameters.maxHeight.Value;
            pdu.mcsCi.minimumParameters.maxMcsPduSize = (long)mcsConnectInitial.minimumParameters.maxMCSPDUsize.Value;
            pdu.mcsCi.minimumParameters.maxTokenIds = (long)mcsConnectInitial.minimumParameters.maxTokenIds.Value;
            pdu.mcsCi.minimumParameters.maxUserIds = (long)mcsConnectInitial.minimumParameters.maxUserIds.Value;
            pdu.mcsCi.minimumParameters.minThroughput = (long)mcsConnectInitial.minimumParameters.minThroughput.Value;
            pdu.mcsCi.minimumParameters.numPriorities = (long)mcsConnectInitial.minimumParameters.numPriorities.Value;
            pdu.mcsCi.minimumParameters.protocolVersion = (long)mcsConnectInitial.minimumParameters.protocolVersion.Value;

            pdu.mcsCi.maximumParameters.maxChannelIds = (long)mcsConnectInitial.maximumParameters.maxChannelIds.Value;
            pdu.mcsCi.maximumParameters.maxHeight = (long)mcsConnectInitial.maximumParameters.maxHeight.Value;
            pdu.mcsCi.maximumParameters.maxMcsPduSize = (long)mcsConnectInitial.maximumParameters.maxMCSPDUsize.Value;
            pdu.mcsCi.maximumParameters.maxTokenIds = (long)mcsConnectInitial.maximumParameters.maxTokenIds.Value;
            pdu.mcsCi.maximumParameters.maxUserIds = (long)mcsConnectInitial.maximumParameters.maxUserIds.Value;
            pdu.mcsCi.maximumParameters.minThroughput = (long)mcsConnectInitial.maximumParameters.minThroughput.Value;
            pdu.mcsCi.maximumParameters.numPriorities = (long)mcsConnectInitial.maximumParameters.numPriorities.Value;
            pdu.mcsCi.maximumParameters.protocolVersion = (long)mcsConnectInitial.maximumParameters.protocolVersion.Value;

            // T125 User Data: get McsConnectResponse's user data
            //byte[] userData = new byte[mcsConnectInitial.userData.Length];
            //Stream mscInput = mcsConnectInitial.userData.toInputStream();
            //mscInput.Read(userData, 0, userData.Length);
            byte[] userData = mcsConnectInitial.userData.ByteArrayValue;

            // T125 User Data: decode McsConnectResponse's user data
            Asn1DecodingBuffer connectDataBuffer = new Asn1DecodingBuffer(userData);
            ConnectData connectData = new ConnectData();
            connectData.PerDecode(connectDataBuffer);

            // Gcc Data: get Gcc data
            int gccDataLength = userData.Length - ConstValue.GCC_CCI_DATA_OFFSET;
            if (gccDataLength <= 0)
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_DATA_INDEX_OUT_OF_RANGE);
            }
            byte[] gccData = new byte[gccDataLength];
            Array.Copy(userData, ConstValue.GCC_CCI_DATA_OFFSET, gccData, 0, gccData.Length);

            // Gcc Data: decode Gcc data
            ConnectGCCPDU gccPdu = new ConnectGCCPDU();
            Asn1DecodingBuffer gccPduBuffer = new Asn1DecodingBuffer(gccData);
            gccPdu.PerDecode(gccPduBuffer);

            // McsConnectResponse: H221Key
            ConferenceCreateRequest conferenceRequest = (ConferenceCreateRequest)gccPdu.GetData();
            H221NonStandardIdentifier identifier =
                (H221NonStandardIdentifier)conferenceRequest.userData.Elements[0].key.GetData();
            pdu.mcsCi.gccPdu.h221Key = Encoding.ASCII.GetString(identifier.ByteArrayValue);

            // Gcc User Data: get Gcc user data
            byte[] gccUserData = conferenceRequest.userData.Elements[0].value.ByteArrayValue;

            // Reset current index
            currentIndex = 0;
            while (currentIndex < gccUserData.Length)
            {
                // Peek data type
                int tempIndex = currentIndex;
                int orgIndex = currentIndex;
                TS_UD_HEADER_type_Values type =
                    (TS_UD_HEADER_type_Values)ParseUInt16(gccUserData, ref tempIndex, false);
                ushort userDataLength = ParseUInt16(gccUserData, ref tempIndex, false);

                // Parse data by type
                switch (type)
                {
                    case TS_UD_HEADER_type_Values.CS_CORE:
                        pdu.mcsCi.gccPdu.clientCoreData = ParseTsUdCsCore(gccUserData, ref currentIndex, userDataLength);
                        break;

                    case TS_UD_HEADER_type_Values.CS_NET:
                        pdu.mcsCi.gccPdu.clientNetworkData = ParseTsUdCsNet(gccUserData, ref currentIndex);
                        break;

                    case TS_UD_HEADER_type_Values.CS_SECURITY:
                        pdu.mcsCi.gccPdu.clientSecurityData = ParseTsUdCsSec(gccUserData, ref currentIndex);
                        break;

                    case TS_UD_HEADER_type_Values.CS_CLUSTER:
                        pdu.mcsCi.gccPdu.clientClusterData = ParseTsUdCsCluster(gccUserData, ref currentIndex);
                        break;

                    case TS_UD_HEADER_type_Values.CS_MONITOR:
                        pdu.mcsCi.gccPdu.clientMonitorData = ParseTsUdCsMon(gccUserData, ref currentIndex);
                        break;

                    case TS_UD_HEADER_type_Values.CS_MCS_MSGCHANNEL:
                        pdu.mcsCi.gccPdu.clientMessageChannelData = ParseTsUdCsMcsMsgChannel(gccUserData, ref currentIndex);
                        break;

                    case TS_UD_HEADER_type_Values.CS_MULTITRANSPORT:
                        pdu.mcsCi.gccPdu.clientMultitransportChannelData = ParseTsUdCsMultiTransport(gccUserData, ref currentIndex);
                        break;

                    case TS_UD_HEADER_type_Values.CS_MONITOR_EX:
                        pdu.mcsCi.gccPdu.clientMonitorExtendedData = ParseTsUdCsMonitorEX(gccUserData, ref currentIndex);
                        break;

                    default:
                        break;
                        //throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
                }
                currentIndex = orgIndex + userDataLength;
            }

            // Check if data length exceeded expectation
            VerifyDataLength(gccUserData.Length, currentIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// Decode MCS Erect Domain Request PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>Decode MCS Erect Domain Request PDU</returns>
        public StackPacket DecodeMcsErectDomainRequestPDU(byte[] data)
        {
            // initialize
            int currentIndex = 0;
            Client_MCS_Erect_Domain_Request pdu = new Client_MCS_Erect_Domain_Request();

            // McsAttachUserConfirmPDU: TpktHeader
            pdu.tpktHeader = ParseTpktHeader(data, ref currentIndex);

            // McsAttachUserConfirmPDU: x224Data
            pdu.x224Data = ParseX224Data(data, ref currentIndex);

            ErectDomainRequest erectDomainRequest = (ErectDomainRequest)ParseMcsDomainPdu(data, ref currentIndex).GetData();

            pdu.subHeight = (int)erectDomainRequest.subHeight.Value;
            pdu.subInterval = (int)erectDomainRequest.subInterval.Value;

            // Check if data length exceeded expectation
            VerifyDataLength(data.Length, currentIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }

        /// <summary>
        /// Decode MCS Attach User Request PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>decoded MCS Attach User Request PDU</returns>
        public StackPacket DecodeMcsAttachUserRequestPDU(byte[] data)
        {
            // initialize
            int currentIndex = 0;
            Client_MCS_Attach_User_Request pdu = new Client_MCS_Attach_User_Request();

            // McsAttachUserConfirmPDU: TpktHeader
            pdu.tpktHeader = ParseTpktHeader(data, ref currentIndex);

            // McsAttachUserConfirmPDU: x224Data
            pdu.x224Data = ParseX224Data(data, ref currentIndex);

            AttachUserRequest attachUserRequest = (AttachUserRequest)ParseMcsDomainPdu(data, ref currentIndex).GetData();

            // Check if data length exceeded expectation
            VerifyDataLength(data.Length, currentIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// Decode MCS Channel Join Request PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>decoded MCS Channel Join Request PDU</returns>
        public StackPacket DecodeMcsChannelJoinRequestPDU(byte[] data)
        {
            // initialize
            int currentIndex = 0;
            Client_MCS_Channel_Join_Request pdu = new Client_MCS_Channel_Join_Request();

            // McsChannelJoinConfirm: TpktHeader
            pdu.tpktHeader = ParseTpktHeader(data, ref currentIndex);

            // McsChannelJoinConfirm: x224Data
            pdu.x224Data = ParseX224Data(data, ref currentIndex);

            // McsChannelJoinConfirm: channelJoinConfirm
            ChannelJoinRequest channelJoinReq = (ChannelJoinRequest)ParseMcsDomainPdu(data, ref currentIndex).GetData();
            pdu.userChannelId = (long)channelJoinReq.initiator.Value;
            pdu.mcsChannelId = (long)channelJoinReq.channelId.Value;

            // Check if data length exceeded expectation
            VerifyDataLength(data.Length, currentIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// Decode Security Exchange PDU
        /// </summary>
        /// <param name="serverSessionContext">the server session context</param>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data</param>
        /// <param name="type">the security header type</param>
        /// <returns>Decoded Security Exchange PDU</returns>
        public StackPacket DecodeSecurityExchangePdu(
            RdpbcgrServerSessionContext serverSessionContext,
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            int currentIndex = 0;
            Client_Security_Exchange_Pdu pdu = new Client_Security_Exchange_Pdu();
            pdu.commonHeader = ParseMcsCommonHeader(data, ref currentIndex, type);

            int userDataIndex = 0;
            pdu.securityExchangePduData = ParseSecurityExchange(
                serverSessionContext,
                decryptedUserData,
                ref userDataIndex);

            // ETW Provider Dump Message
            if (pdu.commonHeader.securityHeader != null)
            {
                // RDP Standard Security
                string messageName = "RDPBCGR:" + pdu.GetType().Name;
                ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, pdu.GetType().Name, decryptedUserData);
            }

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// Decode Client Info PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data</param>
        /// <param name="type">the security header type</param>
        /// <returns>Decoded Client Info PDU</returns>
        public StackPacket DecodeClientInfoPdu(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            int currentIndex = 0;
            Client_Info_Pdu pdu = new Client_Info_Pdu();
            pdu.commonHeader = ParseMcsCommonHeader(data, ref currentIndex, type);

            int userDataIndex = 0;
            pdu.infoPacket = ParseClientInfo(decryptedUserData, ref userDataIndex);

            // ETW Provider Dump Message
            if (type != SecurityHeaderType.Basic)
            {
                // RDP Standard Security
                string messageName = "RDPBCGR:" + pdu.GetType().Name;
                ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, pdu.GetType().Name, decryptedUserData);
            }

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }

        /// <summary>
        /// Decode Client Auto Detect Response PDU
        /// </summary>
        /// <param name="data"></param>
        /// <param name="decryptedUserData"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public StackPacket DecodeClientAutoDetectResponsePDU(byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Client_Auto_Detect_Response_PDU pdu = new Client_Auto_Detect_Response_PDU();

            int dataIndex = 0;

            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            int userDataIndex = 0;
            pdu.autodetectRspPduData = ParseNetworkDetectionResponse(decryptedUserData, ref userDataIndex);

            // ETW Provider Dump Message
            if (type != SecurityHeaderType.Basic)
            {
                // RDP Standard Security
                string messageName = "RDPBCGR:" + pdu.GetType().Name;
                ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, pdu.GetType().Name, decryptedUserData);
            }

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);

            return pdu;
        }

        /// <summary>
        /// Decode a Client Initiate Multitransport Response PDU
        /// </summary>
        /// <param name="data"></param>
        /// <param name="decryptedUserData"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public StackPacket DecodeClientInitiateMultitransportResponsePDU(byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Client_Initiate_Multitransport_Response_PDU pdu = new Client_Initiate_Multitransport_Response_PDU();

            int dataIndex = 0;

            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            int userDataIndex = 0;
            pdu.requestId = ParseUInt32(decryptedUserData, ref userDataIndex, false);
            pdu.hrResponse = (HrResponse_Value)ParseUInt32(decryptedUserData, ref userDataIndex, false);

            // ETW Provider Dump Message
            if (pdu.commonHeader.securityHeader != null)
            {
                // RDP Standard Security
                string messageName = "RDPBCGR:" + pdu.GetType().Name;
                ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, pdu.GetType().Name, decryptedUserData);
            }

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);

            return pdu;
        }

        /// <summary>
        /// Decode Confirm Active PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Confirm Active PDU</returns>
        public StackPacket DecodeConfirmActivePDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Client_Confirm_Active_Pdu pdu = new Client_Confirm_Active_Pdu();

            // data index
            int dataIndex = 0;

            // DemandActivePDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // DemandActivePDU: demandActivePduData
            pdu.confirmActivePduData = ParseTsConfirmActivePdu(decryptedUserData, ref userDataIndex);

            // ETW Provider Dump Message
            if (pdu.commonHeader.securityHeader != null)
            {
                // RDP Standard Security
                string messageName = "RDPBCGR:" + pdu.GetType().Name;
                ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, pdu.GetType().Name, decryptedUserData);
            }

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// Decode Synchronize PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Synchronize PDU</returns>
        public StackPacket DecodeSynchronizePDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Client_Synchronize_Pdu pdu = new Client_Synchronize_Pdu();

            // data index
            int dataIndex = 0;

            // SynchronizePDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // SynchronizePDU: synchronizePduData
            pdu.synchronizePduData = ParseTsSynchronizePdu(decryptedUserData, ref userDataIndex);

            // ETW Provider Dump Message
            if (pdu.commonHeader.securityHeader != null)
            {
                // RDP Standard Security
                string messageName = "RDPBCGR:" + pdu.GetType().Name;
                ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, pdu.GetType().Name, decryptedUserData);
            }

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// Decode Control PDU 
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Control PDU</returns>
        public StackPacket DecodeControlPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            // data index
            int dataIndex = 0;

            // ControlPDU: commonHeader
            SlowPathPduCommonHeader commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // ControlPDU: controlPduData
            TS_CONTROL_PDU controlPduData = ParseTsControlPdu(decryptedUserData, ref userDataIndex);

            // Get pdu by action type
            StackPacket pdu;
            if (controlPduData.action == action_Values.CTRLACTION_COOPERATE)
            {
                // Control PDU - cooperate
                Client_Control_Pdu_Cooperate cooperatePdu = new Client_Control_Pdu_Cooperate();
                cooperatePdu.commonHeader = commonHeader;
                cooperatePdu.controlPduData = controlPduData;
                pdu = cooperatePdu;

                // ETW Provider Dump Message
                if (cooperatePdu.commonHeader.securityHeader != null)
                {
                    // RDP Standard Security
                    string messageName = "RDPBCGR:" + cooperatePdu.GetType().Name;
                    ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, cooperatePdu.GetType().Name, decryptedUserData);
                }
            }
            else if (controlPduData.action == action_Values.CTRLACTION_REQUEST_CONTROL)
            {
                // Control PDU - granted control
                Client_Control_Pdu_Request_Control requestPdu = new Client_Control_Pdu_Request_Control();
                requestPdu.commonHeader = commonHeader;
                requestPdu.controlPduData = controlPduData;
                pdu = requestPdu;

                // ETW Provider Dump Message
                if (requestPdu.commonHeader.securityHeader != null)
                {
                    // RDP Standard Security
                    string messageName = "RDPBCGR:" + requestPdu.GetType().Name;
                    ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, requestPdu.GetType().Name, decryptedUserData);
                }
            }
            else
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
            }

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// Decode Persistent Key List PDU 
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Control PDU</returns>
        public StackPacket DecodePersistentKeyListPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Client_Persistent_Key_List_Pdu pdu = new Client_Persistent_Key_List_Pdu();

            // data index
            int dataIndex = 0;

            // FontMapPDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // FontMapPDU: fontMapPduData
            pdu.persistentKeyListPduData = ParseTsPersistentListPdu(decryptedUserData, ref userDataIndex);

            // ETW Provider Dump Message
            if (pdu.commonHeader.securityHeader != null)
            {
                // RDP Standard Security
                string messageName = "RDPBCGR:" + pdu.GetType().Name;
                ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, pdu.GetType().Name, decryptedUserData);
            }

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// Decode Font List PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Font List PDU</returns>
        public StackPacket DecodeFontListPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Client_Font_List_Pdu pdu = new Client_Font_List_Pdu();

            // data index
            int dataIndex = 0;

            // FontMapPDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // FontMapPDU: fontMapPduData
            pdu.fontListPduData = ParseTsFontListPdu(decryptedUserData, ref userDataIndex);

            // ETW Provider Dump Message
            if (pdu.commonHeader.securityHeader != null)
            {
                // RDP Standard Security
                string messageName = "RDPBCGR:" + pdu.GetType().Name;
                ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, pdu.GetType().Name, decryptedUserData);
            }

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion  PDU Decoders: 10 types of PDU in connection sequence


        #region PDU Decoders: 2 types of PDU in disconnection sequence
        /// <summary>
        /// Decode MCS Disconnect Provider Ultimatum PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>decoded Disconnect Provider Ultimatum PDU</returns>
        public StackPacket DecodeDisconnectProviderUltimatumPDU(byte[] data)
        {
            // initialize
            int currentIndex = 0;
            MCS_Disconnect_Provider_Ultimatum_Pdu pdu = new MCS_Disconnect_Provider_Ultimatum_Pdu();

            // MCS_Disconnect_Provider_Ultimatum_Pdu: tpktHeader
            pdu.tpktHeader = ParseTpktHeader(data, ref currentIndex);

            // MCS_Disconnect_Provider_Ultimatum_Pdu: x224Data
            pdu.x224Data = ParseX224Data(data, ref currentIndex);

            // MCS_Disconnect_Provider_Ultimatum_Pdu: disconnectProviderUltimatum
            pdu.disconnectProvider =
                (DisconnectProviderUltimatum)ParseMcsDomainPdu(data, ref currentIndex).GetData();

            // Check if data length exceeded expectation
            VerifyDataLength(data.Length, currentIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// Decode Server Shutdown Request PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Shutdown Request PDU</returns>
        public StackPacket DecodeShutdownRequestPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Client_Shutdown_Request_Pdu pdu = new Client_Shutdown_Request_Pdu();

            // current index
            int currentIndex = 0;

            // ShutdownRequestDeniedPDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref currentIndex, type);

            // user data index
            int userDataIndex = 0;

            // ShutdownRequestDeniedPDU: 
            pdu.shutdownRequestPduData = ParseTsShutdownReuqestPdu(decryptedUserData, ref userDataIndex);

            // ETW Provider Dump Message
            if (pdu.commonHeader.securityHeader != null)
            {
                // RDP Standard Security
                string messageName = "RDPBCGR:" + pdu.GetType().Name;
                ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, pdu.GetType().Name, decryptedUserData);
            }

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion PDU Decoders: 2 types of PDU in disconnection sequence


        #region PDU Decoders: 2 types of PDU in keyboard and mouse input
        /// <summary>
        /// Decode Slow-Path Update PDU including Slow-Path Graphics Update PDU and Slow-Path Pointer Update PDU.
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Slow-Path Update PDU</returns>
        public StackPacket DecodeSlowPathInputEventPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            TS_INPUT_PDU pdu = new TS_INPUT_PDU();

            // data index
            int dataIndex = 0;

            // SlowPathOutputPDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // SlowPathOutputPDU: slowPathUpdates
            pdu.shareDataHeader = ParseTsShareDataHeader(decryptedUserData, ref userDataIndex);

            pdu.numberEvents = ParseUInt16(decryptedUserData, ref userDataIndex, false);
            pdu.pad2Octets = ParseUInt16(decryptedUserData, ref userDataIndex, false);
            pdu.slowPathInputEvents = new Collection<TS_INPUT_EVENT>();

            // ETW Provider Dump Message
            if (pdu.commonHeader.securityHeader != null)
            {
                // RDP Standard Security
                string messageName = "RDPBCGR:" + pdu.GetType().Name;
                ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, pdu.GetType().Name, decryptedUserData);
            }

            while (userDataIndex < decryptedUserData.Length)
            {
                pdu.slowPathInputEvents.Add(ParseSlowPathInputEvent(decryptedUserData, ref userDataIndex));
            }

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// Decode Fast-path Update PDU
        /// </summary>
        /// <param name="serverSessionContext">server session context</param>
        /// <param name="data">data to be parsed</param>
        /// <returns>decoded Fast-path Update PDU</returns>
        public StackPacket DecodeTsFpInputPDU(RdpbcgrServerSessionContext serverSessionContext, byte[] data)
        {
            int currentIndex = 0;
            TS_FP_INPUT_PDU pdu = new TS_FP_INPUT_PDU();

            pdu.fpInputHeader = new nested_TS_FP_INPUT_PDU_fpInputHeader(ParseByte(data, ref currentIndex));

            var actionCode = pdu.fpInputHeader.action;

            byte numberEvents = (byte)pdu.fpInputHeader.numEvents;

            var encryptionFlags = pdu.fpInputHeader.flags;

            pdu.length1 = ParseByte(data, ref currentIndex);

            if ((ConstValue.MOST_SIGNIFICANT_BIT_FILTER & pdu.length1) != pdu.length1)
            {
                // length2 is present (since the most significant bit of length1 is set)
                pdu.length2 = ParseByte(data, ref currentIndex);
            }

            // TS_FP_UPDATE_PDU: fipsInformation
            if (EncryptionLevel.ENCRYPTION_LEVEL_FIPS == serverSessionContext.RdpEncryptionLevel)
            {
                pdu.fipsInformation = ParseTsFpFipsInfo(data, ref currentIndex);
            }

            // TS_FP_UPDATE_PDU: dataSignature
            if (IsFlagExist((byte)encryptionFlags, (byte)encryptionFlagsChgd_Values.FASTPATH_OUTPUT_ENCRYPTED))
            {
                // pdu were encrypted, data signature exists
                pdu.dataSignature = GetBytes(data, ref currentIndex,
                    ConstValue.TS_FP_UPDATE_PDU_DATA_SIGNATURE_LENGTH);
            }
            else
            {
                pdu.dataSignature = null;
            }

            // Decryption
            bool isSalted = IsFlagExist((byte)encryptionFlags,
                (byte)encryptionFlagsChgd_Values.FASTPATH_OUTPUT_SECURE_CHECKSUM);
            byte[] remainData = GetBytes(data, ref currentIndex, (data.Length - currentIndex));
            byte[] decryptedData = DecryptFastPathInputData(serverSessionContext, remainData, pdu.dataSignature, isSalted);

            // Decrypted data index
            int decryptedDataIndex = 0;

            //[yunzed]
            if (numberEvents == 0)
            {
                Console.WriteLine("numberEvents is 0, so parse the additional numberEvents");
                pdu.numberEvents = ParseByte(decryptedData, ref decryptedDataIndex);
            }

            // TS_FP_UPDATE_PDU: fpOutputUpdates
            pdu.fpInputEvents = ParseTsFpInputEvents(decryptedData, ref decryptedDataIndex);

            // ETW Provider Dump Message
            if (pdu.dataSignature != null)
            {
                // Fast-Path encrypted
                string messageName = "RDPBCGR:" + pdu.GetType().Name;
                ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, pdu.GetType().Name, decryptedData);
            }

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedData.Length, decryptedDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion PDU Decoders: 4 types of PDU in keyboard and mouse input


        #region PDU Decoders: 2 type of PDU in control server graphics output
        /// <summary>
        /// Decode Refresh Rect PDU.
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Refresh Rect PDU</returns>
        public StackPacket DecodeRefreshRectPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Client_Refresh_Rect_Pdu pdu = new Client_Refresh_Rect_Pdu();
            int dataIndex = 0;
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);
            int userDataIndex = 0;
            pdu.refreshRectPduData = ParseTsRefreshRectPdu(decryptedUserData, ref userDataIndex);

            // ETW Provider Dump Message
            if (pdu.commonHeader.securityHeader != null)
            {
                // RDP Standard Security
                string messageName = "RDPBCGR:" + pdu.GetType().Name;
                ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, pdu.GetType().Name, decryptedUserData);
            }

            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// Decode Suppress Output PDU.
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Suppress Output PDU</returns>
        public StackPacket DecodeSuppressOutputPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Client_Suppress_Output_Pdu pdu = new Client_Suppress_Output_Pdu();
            int dataIndex = 0;
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);
            int userDataIndex = 0;
            pdu.suppressOutputPduData = ParseTsSuppressOutputPdu(decryptedUserData, ref userDataIndex);

            // ETW Provider Dump Message
            if (pdu.commonHeader.securityHeader != null)
            {
                // RDP Standard Security
                string messageName = "RDPBCGR:" + pdu.GetType().Name;
                ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, pdu.GetType().Name, decryptedUserData);
            }

            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion PDU Decoders: 1 type of PDU in display update notifications


        #region PDU Decoders: 1 type of PDU in virtual channels
        /// <summary>
        /// Decode Virtual Channel PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Virtual Channel PDU</returns>
        public StackPacket DecodeVirtualChannelPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Virtual_Channel_RAW_Pdu pdu = new Virtual_Channel_RAW_Pdu();

            // data index
            int dataIndex = 0;

            // Virtual_Channel_RAW_Pdu: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // Virtual_Channel_RAW_PDU: channelPduHeader
            pdu.channelPduHeader = ParseChannelPduHeader(decryptedUserData, ref userDataIndex);

            // Virtual_Channel_RAW_PDU: virtualChannelData
            int remainLength = decryptedUserData.Length - Marshal.SizeOf(pdu.channelPduHeader);
            pdu.virtualChannelData = GetBytes(decryptedUserData, ref userDataIndex, remainLength);

            /*
            // ETW Provider Dump Message
            if (pdu.commonHeader.securityHeader != null)
            {
                // RDP Standard Security
                string messageName = "RDPBCGR:" + pdu.GetType().Name;
                ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, pdu.GetType().Name, decryptedUserData);
            }
             * */

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion PDU Decoders: 1 type of PDU in virtual channels

        #region PDU Decoders: 1 type of PDU in RDPRFX
        /// <summary>
        /// Decode Persistent Key List PDU 
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Control PDU</returns>
        public StackPacket DecodeFrameAcknowledgePDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            TS_FRAME_ACKNOWLEDGE_PDU pdu = new TS_FRAME_ACKNOWLEDGE_PDU();

            // data index
            int dataIndex = 0;

            // SlowPathOutputPDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // Share Data Header
            pdu.shareDataHeader = ParseTsShareDataHeader(decryptedUserData, ref userDataIndex);

            //frame id
            pdu.frameID = ParseUInt32(decryptedUserData, ref userDataIndex, false);

            // ETW Provider Dump Message
            if (pdu.commonHeader.securityHeader != null)
            {
                // RDP Standard Security
                string messageName = "RDPBCGR:" + pdu.GetType().Name;
                ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, pdu.GetType().Name, decryptedUserData);
            }

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);

            return pdu;
        }
        #endregion

        #region PDU Decoders: 2 type of PDU in RDSTLS authentication
        /// <summary>
        /// Get the length of RDSTLS Authentication PDU.
        /// </summary>
        /// <param name="data">Buffer containing the PDU data.</param>
        /// <returns>Length of RDSTLS PDU, or zero if more data needed.</returns>
        public int GetLengthOfRDSTLSAuthenticationPDU(byte[] data)
        {
            try
            {
                int result = 0;
                int currentIndex = 0;

                // check common header
                var version = (RDSTLS_VersionEnum)ParseUInt16(data, ref currentIndex, false);
                if (version != RDSTLS_VersionEnum.RDSTLS_VERSION_1)
                {
                    throw new FormatException(ConstValue.ERROR_MESSAGE_UNRECOGNIZED_PDU);
                }

                var pduType = (RDSTLS_PduTypeEnum)ParseUInt16(data, ref currentIndex, false);
                if (pduType != RDSTLS_PduTypeEnum.RDSTLS_TYPE_AUTHREQ)
                {
                    throw new FormatException(ConstValue.ERROR_MESSAGE_UNRECOGNIZED_PDU);
                }

                var dataType = (RDSTLS_DataTypeEnum)ParseUInt16(data, ref currentIndex, false);
                bool invalid = false;
                switch (dataType)
                {
                    case RDSTLS_DataTypeEnum.RDSTLS_DATA_PASSWORD_CREDS:
                        result = GetLengthOfRDSTLSAuthenticationRequestPDUwithPasswordCredentials(data);
                        break;
                    case RDSTLS_DataTypeEnum.RDSTLS_DATA_AUTORECONNECT_COOKIE:
                        result = CheckRDSTLSAuthenticationRequestPDUwithAutoReconnectCookie(data);
                        break;
                    default:
                        invalid = true;
                        break;
                }
                if (invalid)
                {
                    throw new FormatException(ConstValue.ERROR_MESSAGE_UNRECOGNIZED_PDU);
                }

                return result;
            }
            catch (FormatException formatException)
            {
                if (formatException.Message == ConstValue.ERROR_MESSAGE_DATA_INDEX_OUT_OF_RANGE)
                {
                    // more data needed
                    return 0;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get the length of RDSTLS authentication PDU with password credentials.
        /// </summary>
        /// <param name="data">Buffer containing the PDU data.</param>
        /// <returns>Length of RDSTLS PDU, or zero if more data needed.</returns>
        public int GetLengthOfRDSTLSAuthenticationRequestPDUwithPasswordCredentials(byte[] data)
        {
            try
            {
                int currentIndex = 0;

                // check common header
                var version = (RDSTLS_VersionEnum)ParseUInt16(data, ref currentIndex, false);
                if (version != RDSTLS_VersionEnum.RDSTLS_VERSION_1)
                {
                    throw new FormatException(ConstValue.ERROR_MESSAGE_UNRECOGNIZED_PDU);
                }

                var pduType = (RDSTLS_PduTypeEnum)ParseUInt16(data, ref currentIndex, false);
                if (pduType != RDSTLS_PduTypeEnum.RDSTLS_TYPE_AUTHREQ)
                {
                    throw new FormatException(ConstValue.ERROR_MESSAGE_UNRECOGNIZED_PDU);
                }

                var dataType = (RDSTLS_DataTypeEnum)ParseUInt16(data, ref currentIndex, false);
                if (dataType != RDSTLS_DataTypeEnum.RDSTLS_DATA_PASSWORD_CREDS)
                {
                    throw new FormatException(ConstValue.ERROR_MESSAGE_UNRECOGNIZED_PDU);
                }

                // check redirection guid
                int redirectionGuidLength = ParseUInt16(data, ref currentIndex, false);
                currentIndex += redirectionGuidLength;

                // check user name
                int userNameLength = ParseUInt16(data, ref currentIndex, false);
                currentIndex += userNameLength;

                // check domain
                int domainLength = ParseUInt16(data, ref currentIndex, false);
                currentIndex += domainLength;

                // check password
                int passwordLength = ParseUInt16(data, ref currentIndex, false);
                currentIndex += passwordLength;

                if (currentIndex > data.Length)
                {
                    // more data needed
                    return 0;
                }

                return currentIndex;
            }
            catch (FormatException formatException)
            {
                if (formatException.Message == ConstValue.ERROR_MESSAGE_DATA_INDEX_OUT_OF_RANGE)
                {
                    // more data needed
                    return 0;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get the length of RDSTLS authentication PDU with auto-reconnect cookie.
        /// </summary>
        /// <param name="data">Buffer containing the PDU data.</param>
        /// <returns>Length of RDSTLS PDU, or zero if more data needed.</returns>
        public int CheckRDSTLSAuthenticationRequestPDUwithAutoReconnectCookie(byte[] data)
        {
            try
            {
                int currentIndex = 0;

                // check common header
                var version = (RDSTLS_VersionEnum)ParseUInt16(data, ref currentIndex, false);
                if (version != RDSTLS_VersionEnum.RDSTLS_VERSION_1)
                {
                    throw new FormatException(ConstValue.ERROR_MESSAGE_UNRECOGNIZED_PDU);
                }

                var pduType = (RDSTLS_PduTypeEnum)ParseUInt16(data, ref currentIndex, false);
                if (pduType != RDSTLS_PduTypeEnum.RDSTLS_TYPE_AUTHREQ)
                {
                    throw new FormatException(ConstValue.ERROR_MESSAGE_UNRECOGNIZED_PDU);
                }

                var dataType = (RDSTLS_DataTypeEnum)ParseUInt16(data, ref currentIndex, false);
                if (dataType != RDSTLS_DataTypeEnum.RDSTLS_DATA_AUTORECONNECT_COOKIE)
                {
                    throw new FormatException(ConstValue.ERROR_MESSAGE_UNRECOGNIZED_PDU);
                }

                // check session ID
                UInt32 sessionID = ParseUInt32(data, ref currentIndex, false);

                // check auto reconnect cookie
                int autoReconnectCookieLength = ParseUInt16(data, ref currentIndex, false);
                currentIndex += autoReconnectCookieLength;

                if (currentIndex > data.Length)
                {
                    // more data needed
                    return 0;
                }

                return currentIndex;
            }
            catch (FormatException formatException)
            {
                if (formatException.Message == ConstValue.ERROR_MESSAGE_DATA_INDEX_OUT_OF_RANGE)
                {
                    // more data needed
                    return 0;
                }
                else
                {
                    throw;
                }
            }
        }

        public StackPacket SwitchRDSTLSAuthenticationPDU(RdpbcgrServerSessionContext serverSessionContext, byte[] data)
        {
            StackPacket pdu = null;

            int currentIndex = 0;

            var header = ParseRDSTLSCommonHeader(data, ref currentIndex);

            if (header.Version != RDSTLS_VersionEnum.RDSTLS_VERSION_1)
            {
                return null;
            }

            if (header.PduType != RDSTLS_PduTypeEnum.RDSTLS_TYPE_AUTHREQ)
            {
                return null;
            }

            switch (header.DataType)
            {
                case RDSTLS_DataTypeEnum.RDSTLS_DATA_PASSWORD_CREDS:
                    pdu = DecodeRDSTLSAuthenticationRequestPDUwithPasswordCredentials(data);
                    break;

                case RDSTLS_DataTypeEnum.RDSTLS_DATA_AUTORECONNECT_COOKIE:
                    pdu = DecodeRDSTLSAuthenticationRequestPDUwithAutoReconnectCookie(data);
                    break;

                default:
                    pdu = null;
                    break;
            }

            return pdu;
        }

        public RDSTLS_CommonHeader ParseRDSTLSCommonHeader(byte[] data, ref int currentIndex)
        {
            var pdu = new RDSTLS_CommonHeader();
            pdu.Version = (RDSTLS_VersionEnum)ParseUInt16(data, ref currentIndex, false);
            pdu.PduType = (RDSTLS_PduTypeEnum)ParseUInt16(data, ref currentIndex, false);
            pdu.DataType = (RDSTLS_DataTypeEnum)ParseUInt16(data, ref currentIndex, false);
            return pdu;
        }

        public RDSTLS_AuthenticationRequestPDUwithPasswordCredentials DecodeRDSTLSAuthenticationRequestPDUwithPasswordCredentials(byte[] data)
        {
            var pdu = new RDSTLS_AuthenticationRequestPDUwithPasswordCredentials();

            int currentIndex = 0;

            pdu.Header = ParseRDSTLSCommonHeader(data, ref currentIndex);

            // parse redirection guid
            pdu.RedirectionGuidLength = ParseUInt16(data, ref currentIndex, false);
            pdu.RedirectionGuid = GetBytes(data, ref currentIndex, pdu.RedirectionGuidLength);

            // parse user name
            pdu.UserNameLength = ParseUInt16(data, ref currentIndex, false);

            var userNameBytes = GetBytes(data, ref currentIndex, pdu.UserNameLength);
            pdu.UserName = ParseUnicodeString(userNameBytes, true);

            // parse domain
            pdu.DomainLength = ParseUInt16(data, ref currentIndex, false);

            var domainBytes = GetBytes(data, ref currentIndex, pdu.DomainLength);
            pdu.Domain = ParseUnicodeString(domainBytes, true);

            // parse password
            pdu.PasswordLength = ParseUInt16(data, ref currentIndex, false);

            pdu.Password = GetBytes(data, ref currentIndex, pdu.PasswordLength);

            // check total length
            if (currentIndex != data.Length)
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            }

            return pdu;
        }

        public RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookie DecodeRDSTLSAuthenticationRequestPDUwithAutoReconnectCookie(byte[] data)
        {
            var pdu = new RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookie();

            int currentIndex = 0;

            pdu.Header = ParseRDSTLSCommonHeader(data, ref currentIndex);

            // parse session ID
            pdu.SessionID = ParseUInt32(data, ref currentIndex, false);

            // parse auto reconnect cookie
            pdu.AutoReconnectCookieLength = ParseUInt16(data, ref currentIndex, false);

            pdu.AutoReconnectCookie = GetBytes(data, ref currentIndex, pdu.AutoReconnectCookieLength);

            // check total length
            if (currentIndex != data.Length)
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            }

            return pdu;
        }

        #endregion

        #endregion Public Methods: PDU Decoders
        #endregion
    }
}
