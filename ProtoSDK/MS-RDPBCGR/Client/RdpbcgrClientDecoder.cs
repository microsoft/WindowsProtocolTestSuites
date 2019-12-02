// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    /// <summary>
    /// MS-RDPBCGR Decoder Class
    /// </summary>
    public class RdpbcgrDecoder
    {
        #region Private Class Members
        /// <summary>
        /// RDPBCGR Client
        /// (is updated during packet receiving process)
        /// </summary>
        private RdpbcgrClient client;

        /// <summary>
        /// RDPBCGR Client Context 
        /// (provides information for data decryption/decompression)
        /// </summary>
        private RdpbcgrClientContext clientContext;
        #endregion Private Class Members


        #region Constructor
        /// <summary>
        /// RDPBCGR Decoder Constructor
        /// </summary>
        /// <param name="rdpbcgrClient">client</param>
        /// <param name="rdpbcgrClientContext">client context</param>
        public RdpbcgrDecoder(RdpbcgrClient rdpbcgrClient, RdpbcgrClientContext rdpbcgrClientContext)
        {
            // initialize client
            client = rdpbcgrClient;

            // initialize client context
            clientContext = rdpbcgrClientContext;
        }
        #endregion Constructor


        #region Private Methods: Helper Functions
        /// <summary>
        /// Get specified length of bytes from a byte array
        /// (start index is updated according to the specified length)
        /// </summary>
        /// <param name="data">data in byte array</param>
        /// <param name="startIndex">start index</param>
        /// <param name="bytesToRead">specified length</param>
        /// <returns>bytes of specified length</returns>
        private static byte[] GetBytes(byte[] data, ref int startIndex, int bytesToRead)
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
        /// Check if Server License Error PDU secuirty header exists at the front part of user data
        /// </summary>
        /// <param name="userData">user data to be checked</param>
        /// <returns>exists/not</returns>
        private bool IsLicenseErrorSecurityHeaderExist(byte[] userData)
        {
            // Peek "basic security header" at the front part of user data
            int startIndex = 0;
            TS_SECURITY_HEADER header = ParseTsSecurityHeader(
                userData, ref startIndex, SecurityHeaderType.Basic);

            // Check if this "security header" is for Server License Error PDU
            return IsFlagExist((UInt16)header.flags, (UInt16)TS_SECURITY_HEADER_flags_Values.SEC_LICENSE_PKT);
        }

        /// <summary>
        /// Check if Auto-Detect Request PDU secuirty header exists at the front part of user data
        /// </summary>
        /// <param name="userData">user data to be checked</param>
        /// <returns>exists/not</returns>
        private bool IsAutoDetectSecurityHeaderExist(byte[] userData)
        {
            // Peek "basic security header" at the front part of user data
            int startIndex = 0;
            TS_SECURITY_HEADER header = ParseTsSecurityHeader(
                userData, ref startIndex, SecurityHeaderType.Basic);

            // Check if this "security header" is for Server License Error PDU
            return IsFlagExist((UInt16)header.flags, (UInt16)TS_SECURITY_HEADER_flags_Values.SEC_AUTODETECT_REQ);
        }

        /// <summary>
        /// Check if Heartbeat PDU secuirty header exists at the front part of user data
        /// </summary>
        /// <param name="userData">user data to be checked</param>
        /// <returns>exists/not</returns>
        private bool IsHeartbeatSecurityHeaderExist(byte[] userData)
        {
            // Peek "basic security header" at the front part of user data
            int startIndex = 0;
            TS_SECURITY_HEADER header = ParseTsSecurityHeader(
                userData, ref startIndex, SecurityHeaderType.Basic);

            // Check if this "security header" is for Server License Error PDU
            return IsFlagExist((UInt16)header.flags, (UInt16)TS_SECURITY_HEADER_flags_Values.SEC_HEARTBEAT);
        }

        /// <summary>
        /// Check if Initiate Multitransport Request secuirty header exists at the front part of user data
        /// </summary>
        /// <param name="userData">user data to be checked</param>
        /// <returns>exists/not</returns>
        private bool IsInitiateMultitransportRequestSecurityHeaderExist(byte[] userData)
        {
            // Peek "basic security header" at the front part of user data
            int startIndex = 0;
            TS_SECURITY_HEADER header = ParseTsSecurityHeader(
                userData, ref startIndex, SecurityHeaderType.Basic);

            // Check if this "security header" is for Server License Error PDU
            return IsFlagExist((UInt16)header.flags, (UInt16)TS_SECURITY_HEADER_flags_Values.SEC_TRANSPORT_REQ);
        }

        /// <summary>
        /// Get Security Header Type by Client Context
        /// (not applicable to "Server License Error PDU", 
        /// since it always has at least the basic type header)
        /// </summary>
        /// <returns>Security Header Type</returns>
        private SecurityHeaderType GetSecurityHeaderTypeByContext()
        {
            SecurityHeaderType securityHeaderType;
            switch (clientContext.RdpEncryptionLevel)
            {
                case EncryptionLevel.ENCRYPTION_LEVEL_NONE:
                    securityHeaderType = SecurityHeaderType.None;
                    break;

                case EncryptionLevel.ENCRYPTION_LEVEL_LOW:
                    securityHeaderType = SecurityHeaderType.Basic;
                    break;

                case EncryptionLevel.ENCRYPTION_LEVEL_CLIENT_COMPATIBLE:
                case EncryptionLevel.ENCRYPTION_LEVEL_HIGH:
                case EncryptionLevel.ENCRYPTION_LEVEL_FIPS:
                    // The following logic is implemented according to actual situation observed,
                    // since related TD section is involved with [TDI #39940]
                    if (clientContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT
                        || clientContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT
                        || clientContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)
                    {
                        securityHeaderType = SecurityHeaderType.NonFips;
                    }
                    else if (clientContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
                    {
                        securityHeaderType = SecurityHeaderType.Fips;
                    }
                    else
                    {
                        securityHeaderType = SecurityHeaderType.None;
                    }
                    break;

                default:
                    throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
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
        /// Check if the PDU is a Standard Redirection PDU.
        /// </summary>
        /// <param name="userData">user data</param>
        /// <param name="securityHeaderType">security header type</param>
        /// <returns>If the PDU is a Standard Redirection PDU.</returns>
        private bool IsStandardRedirectionPdu(byte[] userData, SecurityHeaderType securityHeaderType)
        {
            // Parse security header
            int index = 0;
            TS_SECURITY_HEADER header = ParseTsSecurityHeader(userData, ref index, securityHeaderType);

            if (null == header)
            {
                return false;
            }

            return IsFlagExist((UInt16)header.flags, (UInt16)TS_SECURITY_HEADER_flags_Values.SEC_REDIRECTION_PKT);
        }


        /// <summary>
        /// For Standard Redirection PDU type, an encryption is needed. 
        /// So if there's encryptionLevel is ENCRYPTION_LEVEL_LOW, update the securityHeaderType according to TD.
        /// </summary>
        /// <param name="securityHeaderType">security header type</param>
        private void UpdateSecurityHeaderType(ref SecurityHeaderType securityHeaderType)
        {
            // Redirection PDU need to decrypt
            if (clientContext.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_LOW)
            {
                if (clientContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT
                    || clientContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT
                    || clientContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)
                {
                    securityHeaderType = SecurityHeaderType.NonFips;
                }
                else if (clientContext.RdpEncryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS)
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
        /// Decrypt Send Data Indication
        /// </summary>
        /// <param name="userData">user data</param>
        /// <param name="securityHeaderType">security header type</param>
        /// <returns>decrypted user data</returns>
        private byte[] DecryptSendDataIndication(byte[] userData, SecurityHeaderType securityHeaderType)
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

            if (isRedirectFlagExist)
            {
                SecurityHeaderType oldType = securityHeaderType;
                UpdateSecurityHeaderType(ref securityHeaderType);
                if (oldType != securityHeaderType)
                {
                    index = 0;
                    header = ParseTsSecurityHeader(userData, ref index, securityHeaderType);
                    remainLength = userData.Length - index;
                    remainData = GetBytes(userData, ref index, remainLength);
                }
            }

            // Get data signature (Fips/NonFips only)
            byte[] signature;
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

                case SecurityHeaderType.Basic:
                    signature = null;
                    break;

                case SecurityHeaderType.None:
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
            if (!clientContext.Decrypt(remainData, signature, isSalted, out decryptedData))
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
        private static UInt16 ParseUInt16(byte[] data, ref int index, bool isBigEndian)
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
        public static UInt32 ParseUInt32(byte[] data, ref int index, bool isBigEndian)
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

        /// <summary>
        /// Parse UInt64
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="index">parser index</param>
        /// <param name="isBigEndian">big endian format flag</param>
        /// <returns>parsed UInt64 number</returns>
        private UInt64 ParseUInt64(byte[] data, ref int index, bool isBigEndian = false)
        {
            byte[] bytes = GetBytes(data, ref index, sizeof(UInt64));
            if (isBigEndian)
            {
                Array.Reverse(bytes, 0, sizeof(UInt64));
            }

            return BitConverter.ToUInt64(bytes, 0);
        }

        private string ParseUnicodeString(byte[] data, ref int index, int size, bool isBigEndian = false, bool isZeroTerminated = true)
        {
            try
            {
                if (size <= 0 || size % 2 != 0)
                {
                    throw new FormatException(ConstValue.ERROR_MESSAGE_INVALID_UNICODE_STRING);
                }

                string result = string.Empty;
                int i = 0;
                UInt16 character = 0;

                while (i < size)
                {
                    character = ParseUInt16(data, ref index, isBigEndian);

                    result += (char)character;
                }

                if (isZeroTerminated)
                {
                    if (character != 0)
                    {
                        throw new FormatException(ConstValue.ERROR_MESSAGE_INVALID_UNICODE_STRING);
                    }
                }

                return result;
            }
            catch
            {
                throw;
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
        #endregion Sub Field Parsers: Common Fields


        #region Sub Field Parsers: X224 Confirm PDU
        /// <summary>
        /// Parse X224Ccf
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>X224Ccf</returns>
        private X224Ccf ParseX224Ccf(byte[] data, ref int currentIndex)
        {
            X224Ccf ccf = new X224Ccf();

            // X224Crq: LengthIndicator
            ccf.lengthIndicator = ParseByte(data, ref currentIndex);

            // X224Crq: TypeCredit
            ccf.typeCredit = ParseByte(data, ref currentIndex);

            // X224Crq: DestRef
            ccf.destRef = ParseUInt16(data, ref currentIndex, true);

            // X224Crq: SrcRef
            ccf.srcRef = ParseUInt16(data, ref currentIndex, true);

            // X224Crq: ClassOptions
            ccf.classOptions = ParseByte(data, ref currentIndex);

            return ccf;
        }


        /// <summary>
        /// Parse RDP_NEG_RSP
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>RDP_NEG_RSP</returns>
        private RDP_NEG_RSP ParseRdpNegRsp(byte[] data, ref int currentIndex)
        {
            RDP_NEG_RSP rdpNegRsp = new RDP_NEG_RSP();

            // RDP_NEG_RSP: type
            rdpNegRsp.type = (RDP_NEG_RSP_type_Values)ParseByte(data, ref currentIndex);

            // RDP_NEG_RSP: flags
            rdpNegRsp.flags = (RDP_NEG_RSP_flags_Values)ParseByte(data, ref currentIndex);

            // RDP_NEG_RSP: length
            rdpNegRsp.length = (RDP_NEG_RSP_length_Values)ParseUInt16(data, ref currentIndex, false);

            // RDP_NEG_RSP: selectedProtocol
            rdpNegRsp.selectedProtocol = (selectedProtocols_Values)ParseUInt32(data, ref currentIndex, false);

            return rdpNegRsp;
        }


        /// <summary>
        /// Parse RDP_NEG_FAILURE
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>RDP_NEG_FAILURE</returns>
        private RDP_NEG_FAILURE ParseRdpNegFailure(byte[] data, ref int currentIndex)
        {
            RDP_NEG_FAILURE rdpNegFailure = new RDP_NEG_FAILURE();

            // RDP_NEG_FAILURE: type
            rdpNegFailure.type = (RDP_NEG_FAILURE_type_Values)ParseByte(data, ref currentIndex);

            // RDP_NEG_FAILURE: flags
            rdpNegFailure.flags = (RDP_NEG_FAILURE_flags_Values)ParseByte(data, ref currentIndex);

            // RDP_NEG_FAILURE: length
            rdpNegFailure.length = (RDP_NEG_FAILURE_length_Values)ParseUInt16(data, ref currentIndex, false);

            // RDP_NEG_FAILURE: failureCode 
            rdpNegFailure.failureCode = (failureCode_Values)ParseUInt32(data, ref currentIndex, false);

            return rdpNegFailure;
        }
        #endregion Sub Field Parsers: X224 Confirm PDU


        #region Sub Field Parsers: Mcs Connect Response
        /// <summary>
        /// Parse TS_UD_SC_CORE
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_UD_SC_CORE</returns>
        private TS_UD_SC_CORE ParseTsUdScCore(byte[] data, ref int currentIndex)
        {
            TS_UD_SC_CORE coreData = new TS_UD_SC_CORE();

            int startIndex = currentIndex;
            // TS_UD_SC_CORE: Header
            coreData.header.type = (TS_UD_HEADER_type_Values)ParseUInt16(data, ref currentIndex, false);
            coreData.header.length = ParseUInt16(data, ref currentIndex, false);

            // TS_UD_SC_CORE: version
            coreData.version = (TS_UD_SC_CORE_version_Values)ParseUInt32(data, ref currentIndex, false);

            // TS_UD_SC_CORE: clientRequestedProtocols
            coreData.clientRequestedProtocols = (requestedProtocols_Values)ParseUInt32(data, ref currentIndex, false);

            if (currentIndex <= startIndex + coreData.header.length - 1)
                coreData.earlyCapabilityFlags = (SC_earlyCapabilityFlags_Values)ParseUInt32(data, ref currentIndex, false);

            return coreData;
        }


        /// <summary>
        /// Parse TS_UD_SC_NET
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_UD_SC_NET</returns>
        private TS_UD_SC_NET ParseTsUdScNet(byte[] data, ref int currentIndex)
        {
            TS_UD_SC_NET netData = new TS_UD_SC_NET();

            // reserve current index
            int reservedIndex = currentIndex;

            // TS_UD_SC_NET: Header
            netData.header.type = (TS_UD_HEADER_type_Values)ParseUInt16(data, ref currentIndex, false);
            netData.header.length = ParseUInt16(data, ref currentIndex, false);

            // TS_UD_SC_NET: MCSChannelID
            netData.MCSChannelId = ParseUInt16(data, ref currentIndex, false);

            // TS_UD_SC_NET: channelCount
            netData.channelCount = ParseUInt16(data, ref currentIndex, false);

            // TS_UD_SC_NET: channelIdArray
            netData.channelIdArray = new ushort[netData.channelCount];
            for (int i = 0; i < netData.channelIdArray.Length; i++)
            {
                netData.channelIdArray[i] = ParseUInt16(data, ref currentIndex, false);
            }

            // TS_UD_SC_NET: Pad
            // (the even/odd checking option described in TD is granted to adapter, 
            // thus adpter can verify their consistency)
            int remainDataLength = netData.header.length - (currentIndex - reservedIndex);
            netData.Pad = GetBytes(data, ref currentIndex, remainDataLength);

            return netData;
        }


        /// <summary>
        /// Parse TS_UD_SC_SEC1
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_UD_SC_SEC1</returns>
        private TS_UD_SC_SEC1 ParseTsUdScSec1(byte[] data, ref int currentIndex)
        {
            TS_UD_SC_SEC1 secData = new TS_UD_SC_SEC1();

            // reserve the start index
            int startIndex = currentIndex;

            // TS_UD_SC_SEC1: header
            secData.header.type = (TS_UD_HEADER_type_Values)ParseUInt16(data, ref currentIndex, false);
            secData.header.length = ParseUInt16(data, ref currentIndex, false);

            // caculate the end index
            int dataEndIndex = secData.header.length + startIndex;

            // TS_UD_SC_SEC1: encryptionMethod
            secData.encryptionMethod = (EncryptionMethods)ParseUInt32(data, ref currentIndex, false);

            // TS_UD_SC_SEC1: encryptionLevel
            secData.encryptionLevel = (EncryptionLevel)ParseUInt32(data, ref currentIndex, false);

            // TS_UD_SC_SEC1: optional data fields (which can be present/absent)
            if (currentIndex < dataEndIndex)
            {
                // TS_UD_SC_SEC1: serverRandomLen (present)
                secData.serverRandomLen = new UInt32Class(ParseUInt32(data, ref currentIndex, false));

                // TS_UD_SC_SEC1: serverCertLen (present)
                secData.serverCertLen = new UInt32Class(ParseUInt32(data, ref currentIndex, false));

                // TS_UD_SC_SEC1: serverRandom (present)
                secData.serverRandom = GetBytes(data, ref currentIndex, (int)secData.serverRandomLen.actualData);

                // TS_UD_SC_SEC1: serverCertificate (present)
                secData.serverCertificate = DecodeServerCertificate(data, ref currentIndex, secData.serverCertLen.actualData);
            }
            else
            {
                // TS_UD_SC_SEC1: serverRandomLen (absent)
                secData.serverRandomLen = null;

                // TS_UD_SC_SEC1: serverCertLen (absent)
                secData.serverCertLen = null;

                // TS_UD_SC_SEC1: serverRandom (absent)
                secData.serverRandom = null;

                // TS_UD_SC_SEC1: serverCertificate (absent)
                secData.serverCertificate = null;
            }
            return secData;
        }

        /// <summary>
        /// Parse TS_UD_SC_MCS_MSGCHANNEL
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_UD_SC_MCS_MSGCHANNEL</returns>
        private TS_UD_SC_MCS_MSGCHANNEL ParseTsUdScMSGChannel(byte[] data, ref int currentIndex)
        {
            TS_UD_SC_MCS_MSGCHANNEL channelData = new TS_UD_SC_MCS_MSGCHANNEL();

            channelData.header.type = (TS_UD_HEADER_type_Values)ParseUInt16(data, ref currentIndex, false);
            channelData.header.length = ParseUInt16(data, ref currentIndex, false);
            channelData.MCSChannelID = ParseUInt16(data, ref currentIndex, false);

            return channelData;
        }

        /// <summary>
        /// Parse TS_UD_SC_MULTITRANSPORT
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_UD_SC_MULTITRANSPORT</returns>
        private TS_UD_SC_MULTITRANSPORT ParseTsUdScMultiTransport(byte[] data, ref int currentIndex)
        {
            TS_UD_SC_MULTITRANSPORT multitransport = new TS_UD_SC_MULTITRANSPORT();

            multitransport.header.type = (TS_UD_HEADER_type_Values)ParseUInt16(data, ref currentIndex, false);
            multitransport.header.length = ParseUInt16(data, ref currentIndex, false);
            multitransport.flags = (MULTITRANSPORT_TYPE_FLAGS)ParseUInt32(data, ref currentIndex, false);

            return multitransport;
        }

        /// <summary>
        /// Parse PROPRIETARYSERVERCERTIFICATE
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>PROPRIETARYSERVERCERTIFICATE</returns>
        public static PROPRIETARYSERVERCERTIFICATE ParseProprietaryServerCertificate(byte[] data, ref int currentIndex)
        {
            PROPRIETARYSERVERCERTIFICATE cert = new PROPRIETARYSERVERCERTIFICATE();

            // serverCertificate: dwSigAlgId
            cert.dwSigAlgId = (dwSigAlgId_Values)ParseUInt32(data, ref currentIndex, false);

            // serverCertificate: dwKeyAlgId
            cert.dwKeyAlgId = (dwKeyAlgId_Values)ParseUInt32(data, ref currentIndex, false);

            // serverCertificate: wPublicKeyBlobType
            cert.wPublicKeyBlobType = (wPublicKeyBlobType_Values)ParseUInt16(data, ref currentIndex, false);

            // serverCertificate: wPublicKeyBlobLen
            cert.wPublicKeyBlobLen = ParseUInt16(data, ref currentIndex, false);

            // serverCertificate: PublicKeyBlob
            cert.PublicKeyBlob = ParseRsaPublicKey(data, ref currentIndex);

            // serverCertificate: wSignatureBlobType
            cert.wSignatureBlobType = (wSignatureBlobType_Values)ParseUInt16(data, ref currentIndex, false);

            // serverCertificate: wSignatureBlobLen
            cert.wSignatureBlobLen = ParseUInt16(data, ref currentIndex, false);

            // serverCertificate: SignatureBlob
            cert.SignatureBlob = GetBytes(data, ref currentIndex, (int)cert.wSignatureBlobLen);

            return cert;
        }

        /// <summary>
        /// Parse X509 Certificate Chain
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <param name="size">cert data size</param>
        /// <returns>X509 Certificate Chain</returns>
        public static X509_CERTIFICATE_CHAIN ParseX509CertificateChain(byte[] data, ref int currentIndex, int size)
        {
            X509_CERTIFICATE_CHAIN cert = new X509_CERTIFICATE_CHAIN();

            cert.NumCertBlobs = (int)ParseUInt32(data, ref currentIndex, false);
            cert.CertBlobArray = new CERT_BLOB[cert.NumCertBlobs];
            for (int i = 0; i < cert.CertBlobArray.Length; i++)
            {
                cert.CertBlobArray[i].cbCert = (int)ParseUInt32(data, ref currentIndex, false);
                cert.CertBlobArray[i].abCert = GetBytes(data, ref currentIndex, cert.CertBlobArray[i].cbCert);
            }
            cert.Padding = GetBytes(data, ref currentIndex, 8 + 4 * cert.NumCertBlobs);

            return cert;
        }

        /// <summary>
        /// Parse RSA_PUBLIC_KEY
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>RSA_PUBLIC_KEY</returns>
        private static RSA_PUBLIC_KEY ParseRsaPublicKey(byte[] data, ref int currentIndex)
        {
            RSA_PUBLIC_KEY key = new RSA_PUBLIC_KEY();

            // RSA_PUBLIC_KEY: magic
            key.magic = (magic_Values)ParseUInt32(data, ref currentIndex, false);

            // RSA_PUBLIC_KEY: keylen
            key.keylen = ParseUInt32(data, ref currentIndex, false);

            // RSA_PUBLIC_KEY: keylen
            key.bitlen = ParseUInt32(data, ref currentIndex, false);

            // RSA_PUBLIC_KEY: datalen
            key.datalen = ParseUInt32(data, ref currentIndex, false);

            // RSA_PUBLIC_KEY: pubExp
            key.pubExp = ParseUInt32(data, ref currentIndex, false);

            // RSA_PUBLIC_KEY: modulus
            key.modulus = GetBytes(data, ref currentIndex, (int)key.keylen);

            return key;
        }

        private static void DecodeX509RSAPublicKey(byte[] publicKey, out byte[] modulus, out byte[] publicExponent)
        {
            // An RSA public key should be represented with the ASN.1 type RSAPublicKey:
            //    RSAPublicKey::= SEQUENCE {
            //        modulus         INTEGER,  --n
            //        publicExponent  INTEGER   --e
            //    }

            if (publicKey == null)
            {
                throw new Exception("publicKey should not be null!");
            }

            // 0x30 stands for "SEQUENCE", 0x02 stands for "INTEGER". publicKey[1] is the size of the SEQUENCE which we don't care.
            if (publicKey[0] != 0x30 || publicKey[2] != 0x02)
            {
                throw new Exception("Invalid publicKey!");
            }

            int modulusLength = publicKey[3]; // publicKey[3] 
            modulus = new byte[modulusLength];
            Buffer.BlockCopy(publicKey, 4, modulus, 0, modulusLength);

            // 0x02 stands for "INTEGER"
            if (publicKey[4 + modulusLength] != 0x02)
            {
                throw new Exception("Invalid publicKey!");
            }

            int publicExponentLength = publicKey[5 + modulusLength];
            publicExponent = new byte[publicExponentLength];
            Buffer.BlockCopy(publicKey, 6 + modulusLength, publicExponent, 0, publicExponentLength);

            // ASN.1 uses big-endian, but the Proprietary Server Certificate uses little-endian, to utilize them, save little-endian format for both cases.
            Array.Reverse(publicExponent);
            Array.Reverse(modulus);
        }

        /// <summary>
        /// Decode the SERVER_CERTIFICATE structure
        /// </summary>
        public static SERVER_CERTIFICATE DecodeServerCertificate(byte[] data, ref int currentIndex, uint serverCertLen)
        {
            SERVER_CERTIFICATE cert = new SERVER_CERTIFICATE();
            // According to [MS-RDPBCGR] section 2.2.1.4.3.1, there is a "t" (1-byte field) in the dwVersion.
            // The real version is the 0-30 bit.
            var version = ParseUInt32(data, ref currentIndex, false);
            cert.dwVersion = (SERVER_CERTIFICATE_dwVersion_Values)(version & 0x0FFFF);
            if (cert.dwVersion == SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_1)
            {
                // proprietary server certificate
                cert.certData = RdpbcgrDecoder.ParseProprietaryServerCertificate(data, ref currentIndex);
            }
            else if (cert.dwVersion == SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_2)
            {
                // X509 certificate chain
                cert.certData = RdpbcgrDecoder.ParseX509CertificateChain(data, ref currentIndex, (int)serverCertLen - 4);
            }
            else
            {
                throw new Exception($"Invalid dwVersion of SERVER_CERTIFICATE: {version}");
            }

            return cert;
        }

        /// <summary>
        /// Decode the public key from the specified certificate
        /// </summary>
        public static void DecodePubicKey(SERVER_CERTIFICATE cert, out byte[] publicExponent, out byte[] modulus)
        {
            publicExponent = null;
            modulus = null;
            if (cert.dwVersion == SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_1)
            {
                // proprietary server certificate
                var proprietaryCert = (PROPRIETARYSERVERCERTIFICATE)cert.certData;
                RSA_PUBLIC_KEY rsaPublicKey = proprietaryCert.PublicKeyBlob;
                publicExponent = BitConverter.GetBytes(rsaPublicKey.pubExp);
                modulus = rsaPublicKey.modulus;
            }
            else if (cert.dwVersion == SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_2)
            {
                // X509 certificate chain
                var x509CertChain = (X509_CERTIFICATE_CHAIN)cert.certData;
                int len = x509CertChain.CertBlobArray.Length;
                // The public key is in the leaf node of the cert chain.
                var x509Cert = new X509Certificate2(x509CertChain.CertBlobArray[len - 1].abCert);
                var publicKey = x509Cert.PublicKey.EncodedKeyValue.RawData;
                RdpbcgrDecoder.DecodeX509RSAPublicKey(publicKey, out modulus, out publicExponent);
            }
            else
            {
                throw new Exception($"Invalid dwVersion of SERVER_CERTIFICATE: {cert.dwVersion}");
            }
        }

        #endregion Sub Field Parsers: Mcs Connect Response


        #region Sub Field Parsers: Server Auto-Reconnect Status PDU
        /// <summary>
        /// Parse TS_AUTORECONNECT_STATUS_PDU
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_AUTORECONNECT_STATUS_PDU</returns>
        private TS_AUTORECONNECT_STATUS_PDU ParseTsAutoReconnectStatusPdu(byte[] data, ref int currentIndex)
        {
            TS_AUTORECONNECT_STATUS_PDU pdu = new TS_AUTORECONNECT_STATUS_PDU();

            // TS_AUTORECONNECT_STATUS_PDU: shareDataHeader
            pdu.shareDataHeader = ParseTsShareDataHeader(data, ref currentIndex);

            // TS_AUTORECONNECT_STATUS_PDU: arcStatus
            pdu.arcStatus = ParseUInt32(data, ref currentIndex, false);

            return pdu;
        }
        #endregion Sub Field Parsers: Server Auto-Reconnect Status PDU


        #region Sub Field Parsers: License Error PDU
        /// <summary>
        /// Parse LICENSE_PREAMBLE
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>LICENSE_PREAMBLE</returns>
        private LICENSE_PREAMBLE ParseLicensePreamble(byte[] data, ref int currentIndex)
        {
            LICENSE_PREAMBLE preamble = new LICENSE_PREAMBLE();

            // LICENSE_PREAMBLE: bMsgType
            preamble.bMsgType = (bMsgType_Values)ParseByte(data, ref currentIndex);

            // LICENSE_PREAMBLE: bVersion
            preamble.bVersion = (bVersion_Values)ParseByte(data, ref currentIndex);

            // LICENSE_PREAMBLE: wMsgSize
            preamble.wMsgSize = ParseUInt16(data, ref currentIndex, false);

            return preamble;
        }


        /// <summary>
        /// Parse LICENSE_ConstValue.ERROR_MESSAGE
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>LICENSE_ConstValue.ERROR_MESSAGE</returns>
        private LICENSE_ERROR_MESSAGE ParseLicenseErrorMessage(byte[] data, ref int currentIndex)
        {
            LICENSE_ERROR_MESSAGE message = new LICENSE_ERROR_MESSAGE();

            // LICENSE_ERROR_MESSAGE: dwErrorCode
            message.dwErrorCode = (dwErrorCode_Values)ParseUInt32(data, ref currentIndex, false);

            // LICENSE_ERROR_MESSAGE: dwStateTransition
            message.dwStateTransition = (dwStateTransition_Values)ParseUInt32(data, ref currentIndex, false);

            // LICENSE_ERROR_MESSAGE: bbErrorInfo
            message.bbErrorInfo.wBlobType = (wBlobType_Values)ParseUInt16(data, ref currentIndex, false);
            message.bbErrorInfo.wBlobLen = ParseUInt16(data, ref currentIndex, true);
            message.bbErrorInfo.blobData = GetBytes(data, ref currentIndex, (int)message.bbErrorInfo.wBlobLen);

            return message;
        }
        #endregion Sub Field Parsers: License Error PDU


        #region Sub Field Parsers: Demand Active PDU
        /// <summary>
        /// Parse TS_DEMAND_ACTIVE_PDU
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_DEMAND_ACTIVE_PDU</returns>
        private TS_DEMAND_ACTIVE_PDU ParseTsDemandActivePdu(byte[] data, ref int currentIndex)
        {
            TS_DEMAND_ACTIVE_PDU pdu = new TS_DEMAND_ACTIVE_PDU();

            // TS_DEMAND_ACTIVE_PDU: shareControlHeader
            pdu.shareControlHeader = ParseTsShareControlHeader(data, ref currentIndex);

            // TS_DEMAND_ACTIVE_PDU: shareId
            pdu.shareId = ParseUInt32(data, ref currentIndex, false);

            // TS_DEMAND_ACTIVE_PDU: lengthSourceDescriptor
            pdu.lengthSourceDescriptor = ParseUInt16(data, ref currentIndex, false);

            // TS_DEMAND_ACTIVE_PDU: lengthCombinedCapabilities
            pdu.lengthCombinedCapabilities = ParseUInt16(data, ref currentIndex, false);

            // TS_DEMAND_ACTIVE_PDU: sourceDescriptor
            pdu.sourceDescriptor = GetBytes(data, ref currentIndex, pdu.lengthSourceDescriptor);

            // TS_DEMAND_ACTIVE_PDU: numberCapabilities
            pdu.numberCapabilities = ParseUInt16(data, ref currentIndex, false);

            // TS_DEMAND_ACTIVE_PDU: pad2Octets
            pdu.pad2Octets = ParseUInt16(data, ref currentIndex, false);

            // TS_DEMAND_ACTIVE_PDU: capabilitySets
            pdu.capabilitySets = new Collection<ITsCapsSet>();
            for (int i = 0; i < pdu.numberCapabilities; i++)
            {
                pdu.capabilitySets.Add(ParseTsCapsSet(data, ref currentIndex));
            }

            // TS_DEMAND_ACTIVE_PDU: sessionId
            pdu.sessionId = ParseUInt32(data, ref currentIndex, false);

            return pdu;
        }


        /// <summary>
        /// Parse TS_CAPS_SET
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_CAPS_SET</returns>
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

                // Bitmap Codecs Capability Set
                case capabilitySetType_Values.CAPSETTYPE_FRAME_ACKNOWLEDGE:
                    set = ParseCapsTypeFrameAcknowledge(capabilityData);
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
            set.imeFileName = BitConverter.ToString(imeFileName);

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
        private TS_FRAME_ACKNOWLEDGE_CAPABILITYSET ParseCapsTypeFrameAcknowledge(byte[] data)
        {
            TS_FRAME_ACKNOWLEDGE_CAPABILITYSET set = RdpbcgrUtility.ToStruct<TS_FRAME_ACKNOWLEDGE_CAPABILITYSET>(data);

            // Check if data length is consistent with the decoded struct length
            VerifyDataLength(data.Length, Marshal.SizeOf(set), ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            return set;
        }

        #endregion Capbility Sets
        #endregion Sub Field Parsers: Demand Active PDU


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


        #region Sub Field Parsers: Font Map PDU
        /// <summary>
        /// Parse TS_FONT_MAP_PDU
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_FONT_MAP_PDU</returns>
        private TS_FONT_MAP_PDU ParseTsFontMapPdu(byte[] data, ref int currentIndex)
        {
            TS_FONT_MAP_PDU pdu = new TS_FONT_MAP_PDU();

            // TS_FONT_MAP_PDU: shareDataHeader
            pdu.shareDataHeader = ParseTsShareDataHeader(data, ref currentIndex);

            // TS_FONT_MAP_PDU: numberEntries
            pdu.numberEntries = ParseUInt16(data, ref currentIndex, false);

            // TS_FONT_MAP_PDU: totalNumEntries
            pdu.totalNumEntries = ParseUInt16(data, ref currentIndex, false);

            // TS_FONT_MAP_PDU: mapFlags
            pdu.mapFlags = ParseUInt16(data, ref currentIndex, false);

            // TS_FONT_MAP_PDU: entrySize
            pdu.entrySize = ParseUInt16(data, ref currentIndex, false);

            return pdu;
        }
        #endregion Sub Field Parsers: Font Map PDU


        #region Sub Field Parsers: Deactivate All PDU
        /// <summary>
        /// Parse TS_DEACTIVATE_ALL_PDU
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_DEACTIVATE_ALL_PDU</returns>
        private TS_DEACTIVATE_ALL_PDU ParseTsDeactivateAllPdu(byte[] data, ref int currentIndex)
        {
            TS_DEACTIVATE_ALL_PDU pdu = new TS_DEACTIVATE_ALL_PDU();

            // TS_DEACTIVATE_ALL_PDU: shareControlHeader
            pdu.shareControlHeader = ParseTsShareControlHeader(data, ref currentIndex);

            // TS_DEACTIVATE_ALL_PDU: shareId
            pdu.shareId = ParseUInt32(data, ref currentIndex, false);

            // TS_DEACTIVATE_ALL_PDU: lengthSourceDescriptor
            pdu.lengthSourceDescriptor = ParseUInt16(data, ref currentIndex, false);

            // TS_DEACTIVATE_ALL_PDU: sourceDescriptor
            pdu.sourceDescriptor = GetBytes(data, ref currentIndex, (int)pdu.lengthSourceDescriptor);

            return pdu;
        }
        #endregion Sub Field Parsers: Deactivate All PDU


        #region Sub Field Parsers: Shutdown Request Denied PDU
        /// <summary>
        /// Parse TS_SHUTDOWN_DENIED_PDU
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SHUTDOWN_DENIED_PDU</returns>
        private TS_SHUTDOWN_DENIED_PDU ParseTsShutdownDeniedPdu(byte[] data, ref int currentIndex)
        {
            TS_SHUTDOWN_DENIED_PDU pdu = new TS_SHUTDOWN_DENIED_PDU();

            // TS_SHUTDOWN_DENIED_PDU: shareDataHeader
            pdu.shareDataHeader = ParseTsShareDataHeader(data, ref currentIndex);

            return pdu;
        }
        #endregion Sub Field Parsers: Shutdown Request Denied PDU


        #region Sub Field Parsers: Set Error Info PDU
        /// <summary>
        /// Parse TS_SET_ERROR_INFO_PDU
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SET_ERROR_INFO_PDU</returns>
        private TS_SET_ERROR_INFO_PDU ParseTsSetErrorInfoPdu(byte[] data, ref int currentIndex)
        {
            TS_SET_ERROR_INFO_PDU pdu = new TS_SET_ERROR_INFO_PDU();

            // TS_SET_ERROR_INFO_PDU: shareDataHeader
            pdu.shareDataHeader = ParseTsShareDataHeader(data, ref currentIndex);

            // TS_SET_ERROR_INFO_PDU: errorInfo
            pdu.errorInfo = (errorInfo_Values)ParseUInt32(data, ref currentIndex, false);

            return pdu;
        }
        #endregion Sub Field Parsers: Set Error Info PDU


        #region Sub Field Parsers: Set Keyboard Indicators PDU
        /// <summary>
        /// Parse TS_SET_KEYBOARD_INDICATORS_PDU
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SET_KEYBOARD_INDICATORS_PDU</returns>
        private TS_SET_KEYBOARD_INDICATORS_PDU ParseTsSetKeyboardIndicatorsPdu(byte[] data, ref int currentIndex)
        {
            TS_SET_KEYBOARD_INDICATORS_PDU pdu = new TS_SET_KEYBOARD_INDICATORS_PDU();

            // TS_SET_KEYBOARD_INDICATORS_PDU: shareDataHeader
            pdu.shareDataHeader = ParseTsShareDataHeader(data, ref currentIndex);

            // TS_SET_KEYBOARD_INDICATORS_PDU: UnitId
            pdu.UnitId = ParseUInt16(data, ref currentIndex, false);

            // TS_SET_KEYBOARD_INDICATORS_PDU: LedFlags
            pdu.LedFlags = (LedFlags_Values)ParseUInt16(data, ref currentIndex, false);

            return pdu;
        }
        #endregion Sub Field Parsers: Set Keyboard Indicators PDU


        #region Sub Field Parsers: Set Keyboard IME Status PDU
        /// <summary>
        /// Parse TS_SET_KEYBOARD_IME_STATUS_PDU
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SET_KEYBOARD_IME_STATUS_PDU</returns>
        private TS_SET_KEYBOARD_IME_STATUS_PDU ParseTsSetKeyboardImeStatusPdu(byte[] data, ref int currentIndex)
        {
            TS_SET_KEYBOARD_IME_STATUS_PDU pdu = new TS_SET_KEYBOARD_IME_STATUS_PDU();

            // TS_SET_KEYBOARD_IME_STATUS_PDU: shareDataHeader
            pdu.shareDataHeader = ParseTsShareDataHeader(data, ref currentIndex);

            // TS_SET_KEYBOARD_IME_STATUS_PDU: UnitId
            pdu.UnitId = ParseUInt16(data, ref currentIndex, false);

            // TS_SET_KEYBOARD_IME_STATUS_PDU: ImeOpen
            pdu.ImeState = ParseUInt32(data, ref currentIndex, false);

            // TS_SET_KEYBOARD_IME_STATUS_PDU: ImeConvMode
            pdu.ImeConvMode = ParseUInt32(data, ref currentIndex, false);

            return pdu;
        }
        #endregion Sub Field Parsers: Set Keyboard IME Status PDU


        #region Sub Field Parsers: Play Sound PDU
        /// <summary>
        /// Parse TS_PLAY_SOUND_PDU_DATA
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_PLAY_SOUND_PDU_DATA</returns>
        private TS_PLAY_SOUND_PDU_DATA ParseTsPlaySoundPduData(byte[] data, ref int currentIndex)
        {
            TS_PLAY_SOUND_PDU_DATA pdu = new TS_PLAY_SOUND_PDU_DATA();

            // TS_PLAY_SOUND_PDU_DATA: shareDataHeader
            pdu.shareDataHeader = ParseTsShareDataHeader(data, ref currentIndex);

            // TS_PLAY_SOUND_PDU_DATA: duration
            pdu.duration = ParseUInt32(data, ref currentIndex, false);

            // TS_PLAY_SOUND_PDU_DATA: frequency
            pdu.frequency = ParseUInt32(data, ref currentIndex, false);

            return pdu;
        }
        #endregion Sub Field Parsers: Play Sound PDU


        #region Sub Field Parsers: Save Session Info PDU
        /// <summary>
        /// Parse TS_SAVE_SESSION_INFO_PDU_DATA
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SAVE_SESSION_INFO_PDU_DATA</returns>
        private TS_SAVE_SESSION_INFO_PDU_DATA ParseTsSaveSessionInfoPduData(byte[] data, ref int currentIndex)
        {
            TS_SAVE_SESSION_INFO_PDU_DATA pdu = new TS_SAVE_SESSION_INFO_PDU_DATA();

            // TS_SAVE_SESSION_INFO_PDU_DATA: shareDataHeader
            pdu.shareDataHeader = ParseTsShareDataHeader(data, ref currentIndex);

            // TS_SAVE_SESSION_INFO_PDU_DATA: infoType
            pdu.infoType = (infoType_Values)ParseUInt32(data, ref currentIndex, false);

            // TS_SAVE_SESSION_INFO_PDU_DATA: infoData
            pdu.infoData = ParseInfoData(data, ref currentIndex, pdu.infoType);

            return pdu;
        }


        /// <summary>
        /// Parse Info Data by Type
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <param name="infoType">info type</param>
        /// <returns>parsed info data</returns>
        private object ParseInfoData(byte[] data, ref int currentIndex, infoType_Values infoType)
        {
            // Parse Info Data by Type
            object infoData;
            switch (infoType)
            {
                // Logon Info Version 1
                case infoType_Values.INFOTYPE_LOGON:
                    infoData = (object)ParseTsLogonInfo(data, ref currentIndex);
                    break;

                // Logon Info Version 2
                case infoType_Values.INFOTYPE_LOGON_LONG:
                    infoData = (object)ParseTsLogonInfoVersion2(data, ref currentIndex);
                    break;

                // Plain Notify
                case infoType_Values.INFOTYPE_LOGON_PLAINNOTIFY:
                    infoData = (object)ParseTsPlainNotify(data, ref currentIndex);
                    break;

                // Logon Info Extended
                case infoType_Values.INFOTYPE_LOGON_EXTENDED_INF:
                    infoData = (object)ParseTsLogonInfoExtended(data, ref currentIndex);
                    break;

                default:
                    throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
            }
            return infoData;
        }


        /// <summary>
        /// Parse TS_LOGON_INFO
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_LOGON_INFO</returns>
        private TS_LOGON_INFO ParseTsLogonInfo(byte[] data, ref int currentIndex)
        {
            TS_LOGON_INFO info = new TS_LOGON_INFO();

            // TS_LOGON_INFO: cbDomain
            info.cbDomain = ParseUInt32(data, ref currentIndex, false);

            // TS_LOGON_INFO: Domain
            byte[] domainBytes = GetBytes(data, ref currentIndex, ConstValue.TS_LOGON_INFO_DOMAIN_LENGTH);
            info.Domain = Encoding.Unicode.GetString(domainBytes, 0, (int)info.cbDomain);

            // TS_LOGON_INFO: cbUserName
            info.cbUserName = ParseUInt32(data, ref currentIndex, false);

            // TS_LOGON_INFO: UserName
            byte[] usernameBytes = GetBytes(data, ref currentIndex, ConstValue.TS_LOGON_INFO_USER_NAME_LENGTH);
            info.UserName = Encoding.Unicode.GetString(usernameBytes, 0, (int)info.cbUserName);

            // TS_LOGON_INFO: SessionId
            info.SessionId = ParseUInt32(data, ref currentIndex, false);

            return info;
        }


        /// <summary>
        /// Parse TS_LOGON_INFO_VERSION_2
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_LOGON_INFO_VERSION_2</returns>
        private TS_LOGON_INFO_VERSION_2 ParseTsLogonInfoVersion2(byte[] data, ref int currentIndex)
        {
            TS_LOGON_INFO_VERSION_2 info = new TS_LOGON_INFO_VERSION_2();

            // TS_LOGON_INFO_VERSION_2: 
            info.Version = (TS_LOGON_INFO_VERSION_2_Version_Values)ParseUInt16(data, ref currentIndex, false);

            // TS_LOGON_INFO_VERSION_2: Size
            info.Size = ParseUInt32(data, ref currentIndex, false);

            // TS_LOGON_INFO_VERSION_2: SessionId
            info.SessionId = ParseUInt32(data, ref currentIndex, false);

            // TS_LOGON_INFO_VERSION_2: cbDomain
            info.cbDomain = ParseUInt32(data, ref currentIndex, false);

            // TS_LOGON_INFO_VERSION_2: cbUserName
            info.cbUserName = ParseUInt32(data, ref currentIndex, false);

            // TS_LOGON_INFO_VERSION_2: Pad
            info.Pad = GetBytes(data, ref currentIndex, ConstValue.TS_LOGON_INFO_VERSION_2_PAD_LENGTH);

            // TS_LOGON_INFO_VERSION_2: Domain
            byte[] domainBytes = GetBytes(data, ref currentIndex, (int)info.cbDomain);
            info.Domain = Encoding.Unicode.GetString(domainBytes);

            // TS_LOGON_INFO_VERSION_2: UserName
            byte[] usernameBytes = GetBytes(data, ref currentIndex, (int)info.cbUserName);
            info.UserName = Encoding.Unicode.GetString(usernameBytes);

            return info;
        }


        /// <summary>
        /// Parse TS_PLAIN_NOTIFY
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_PLAIN_NOTIFY</returns>
        private TS_PLAIN_NOTIFY ParseTsPlainNotify(byte[] data, ref int currentIndex)
        {
            TS_PLAIN_NOTIFY notify = new TS_PLAIN_NOTIFY();

            // TS_PLAIN_NOTIFY: Pad
            notify.Pad = GetBytes(data, ref currentIndex, ConstValue.TS_PLAIN_NOTIFY_PAD_LENGTH);

            return notify;
        }


        /// <summary>
        /// Parse TS_LOGON_INFO_EXTENDED
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_LOGON_INFO_EXTENDED</returns>
        private TS_LOGON_INFO_EXTENDED ParseTsLogonInfoExtended(byte[] data, ref int currentIndex)
        {
            TS_LOGON_INFO_EXTENDED info = new TS_LOGON_INFO_EXTENDED();

            // TS_LOGON_INFO_EXTENDED: Length
            info.Length = ParseUInt16(data, ref currentIndex, false);

            // TS_LOGON_INFO_EXTENDED: FieldsPresent
            info.FieldsPresent = (FieldsPresent_Values)ParseUInt32(data, ref currentIndex, false);

            // TS_LOGON_INFO_EXTENDED: LogonFields
            info.LogonFields = ParseTsLogonInfoFields(data, ref currentIndex, info.FieldsPresent);

            // TS_LOGON_INFO_EXTENDED: Pad
            info.Pad = GetBytes(data, ref currentIndex, ConstValue.TS_LOGON_INFO_EXTENDED_PAD_LENGTH);

            return info;
        }


        /// <summary>
        /// Parse TS_LOGON_INFO_FIELD array
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <param name="presentFlag">fields present flag</param>
        /// <returns>TS_LOGON_INFO_FIELD array</returns>
        private TS_LOGON_INFO_FIELD[] ParseTsLogonInfoFields(
            byte[] data,
            ref int currentIndex,
            FieldsPresent_Values presentFlag)
        {
            // list to save info field(s)
            List<TS_LOGON_INFO_FIELD> list = new List<TS_LOGON_INFO_FIELD>();

            // Auto reconnect field (optional)
            if (IsFlagExist((uint)presentFlag, (uint)FieldsPresent_Values.LOGON_EX_AUTORECONNECTCOOKIE))
            {
                TS_LOGON_INFO_FIELD autoReconnectField = new TS_LOGON_INFO_FIELD();
                autoReconnectField.cbFieldData = ParseUInt32(data, ref currentIndex, false);
                autoReconnectField.FieldData = ParseArcScPrivatePacket(data, ref currentIndex);
                list.Add(autoReconnectField);
            }

            // Logon error info field (optional)
            if (IsFlagExist((uint)presentFlag, (uint)FieldsPresent_Values.LOGON_EX_LOGONERRORS))
            {
                TS_LOGON_INFO_FIELD logonErrorField = new TS_LOGON_INFO_FIELD();
                logonErrorField.cbFieldData = ParseUInt32(data, ref currentIndex, false);
                logonErrorField.FieldData = ParseTsLogonErrorsInfo(data, ref currentIndex);
                list.Add(logonErrorField);
            }

            // copy fields to array
            TS_LOGON_INFO_FIELD[] fields = new TS_LOGON_INFO_FIELD[list.Count];
            for (int i = 0; i < list.Count; ++i)
            {
                fields[i] = list[i];
            }
            return fields;
        }


        /// <summary>
        /// Parse ARC_SC_PRIVATE_PACKET
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>ARC_SC_PRIVATE_PACKET</returns>
        private ARC_SC_PRIVATE_PACKET ParseArcScPrivatePacket(byte[] data, ref int currentIndex)
        {
            ARC_SC_PRIVATE_PACKET packet = new ARC_SC_PRIVATE_PACKET();

            // ARC_SC_PRIVATE_PACKET: cbLen
            packet.cbLen = (cbLen_Values)ParseUInt32(data, ref currentIndex, false);

            // ARC_SC_PRIVATE_PACKET: Version
            packet.Version = (Version_Values)ParseUInt32(data, ref currentIndex, false);

            // ARC_SC_PRIVATE_PACKET: LogonId
            packet.LogonId = ParseUInt32(data, ref currentIndex, false);

            // ARC_SC_PRIVATE_PACKET: ArcRandomBits
            packet.ArcRandomBits = GetBytes(data, ref currentIndex,
                ConstValue.ARC_SC_PRIVATE_PACKET_ARC_RANDOM_BITS_LENGTH);

            return packet;
        }


        /// <summary>
        /// Parse TS_LOGON_ERRORS_INFO
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_LOGON_ERRORS_INFO</returns>
        private TS_LOGON_ERRORS_INFO ParseTsLogonErrorsInfo(byte[] data, ref int currentIndex)
        {
            TS_LOGON_ERRORS_INFO info = new TS_LOGON_ERRORS_INFO();

            // TS_LOGON_ERRORS_INFO: ErrorNotificationType
            info.ErrorNotificationType = (ErrorNotificationType_Values)ParseUInt32(data, ref currentIndex, false);

            // TS_LOGON_ERRORS_INFO: ErrorNotificationData
            info.ErrorNotificationData = (ErrorNotificationData_Values)ParseUInt32(data, ref currentIndex, false);

            return info;
        }
        #endregion Sub Field Parsers: Save Session Info PDU


        #region Sub Field Parsers: Slow-Path Output PDU
        /// <summary>
        /// Parse Slow-path Update PDU array
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>Slow-path Update PDU array</returns>
        private RdpbcgrSlowPathUpdatePdu[] ParseSlowPathUpdates(byte[] data, ref int currentIndex)
        {
            // A list of slow-path update PDUs
            List<RdpbcgrSlowPathUpdatePdu> listPdu = new List<RdpbcgrSlowPathUpdatePdu>();

            // Parse one by one
            while (currentIndex < data.Length)
            {
                // Get pduType2 and updateType
                TS_SHAREDATAHEADER shareDataHeader = ParseTsShareDataHeader(data, ref currentIndex);

                // Get update data
                int updateDataLength = shareDataHeader.shareControlHeader.totalLength
                    - Marshal.SizeOf(shareDataHeader);
                byte[] updateData = GetBytes(data, ref currentIndex, updateDataLength);

                // Decompress update data if needed
                updateData = clientContext.Decompress(updateData, shareDataHeader.compressedType);

                // Get a Slow-Path Update PDU
                RdpbcgrSlowPathUpdatePdu pdu = null;
                if (pduType2_Values.PDUTYPE2_UPDATE == shareDataHeader.pduType2)
                {
                    // Slow-Path Graphics Update PDU
                    pdu = ParseTsGraphicsUpdatePdu(updateData, shareDataHeader);
                    listPdu.Add(pdu);
                }
                else if (pduType2_Values.PDUTYPE2_POINTER == shareDataHeader.pduType2)
                {
                    // Slow-Path Pointer Update PDU
                    pdu = ParseTsPointerPdu(updateData, shareDataHeader);
                    listPdu.Add(pdu);
                }
                else
                {
                    throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
                }
            }

            // Copy list to array
            RdpbcgrSlowPathUpdatePdu[] arrayPdu = new RdpbcgrSlowPathUpdatePdu[listPdu.Count];
            for (int i = 0; i < arrayPdu.Length; i++)
            {
                arrayPdu[i] = listPdu[i];
            }
            return arrayPdu;
        }
        #endregion Sub Field Parsers: Slow-Path Output PDU


        #region Sub Field Parsers: Slow-Path Graphics Update PDU
        /// <summary>
        /// Parse Slow-Path Graphics Update PDU
        /// </summary>
        /// <param name="updateData">update data (decompressed if were compressed</param>
        /// <param name="shareDataHeader">share data header</param>
        /// <returns>Slow-path Graphics Update PDU</returns>
        private RdpbcgrSlowPathUpdatePdu ParseTsGraphicsUpdatePdu(
            byte[] updateData,
            TS_SHAREDATAHEADER shareDataHeader)
        {
            // Get update type
            int tempIndex = 0;
            updateType_Values updateType = (updateType_Values)ParseUInt16(updateData, ref tempIndex, false);

            // Parse pdu by update type
            RdpbcgrSlowPathUpdatePdu pdu;
            switch (updateType)
            {
                // TS_UPDATE_ORDERS_PDU_DATA
                case updateType_Values.UPDATETYPE_ORDERS:
                    pdu = ParseTsUpdateOrdersPduData(updateData, shareDataHeader);
                    break;

                // TS_UPDATE_BITMAP_PDU_DATA
                case updateType_Values.UPDATETYPE_BITMAP:
                    pdu = ParseTsUpdateBitmapPduData(updateData, shareDataHeader);
                    break;

                // TS_UPDATE_PALETTE_PDU_DATA
                case updateType_Values.UPDATETYPE_PALETTE:
                    pdu = ParseTsUpdatePalettePduData(updateData, shareDataHeader);
                    break;

                // TS_UPDATE_SYNC_PDU_DATA
                case updateType_Values.UPDATETYPE_SYNCHRONIZE:
                    pdu = ParseTsUpdateSyncPduData(updateData, shareDataHeader);
                    break;

                default:
                    throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
            }
            return pdu;
        }


        #region TS_UPDATE_ORDERS
        /// <summary>
        /// Parse TS_UPDATE_ORDERS
        /// </summary>
        /// <param name="updateData">update data (decompressed if were compressed)</param>
        /// <param name="shareDataHeader">share data header</param>
        /// <returns>TS_UPDATE_ORDERS</returns>
        private TS_UPDATE_ORDERS ParseTsUpdateOrdersPduData(
            byte[] updateData,
            TS_SHAREDATAHEADER shareDataHeader)
        {
            // index of update data
            int index = 0;
            TS_UPDATE_ORDERS pduData = new TS_UPDATE_ORDERS();

            // TS_UPDATE_ORDERS: shareDataHeader
            pduData.shareDataHeader = shareDataHeader;

            // TS_UPDATE_ORDERS: updateType
            pduData.updateType = (updateType_Values)ParseUInt16(updateData, ref index, false);

            // TS_UPDATE_ORDERS: pad2OctetA
            pduData.pad2OctetA = ParseUInt16(updateData, ref index, false);

            // TS_UPDATE_ORDERS: numberOrders
            pduData.numberOrders = ParseUInt16(updateData, ref index, false);

            // TS_UPDATE_ORDERS: pad2OctetsB
            pduData.pad2OctetsB = ParseUInt16(updateData, ref index, false);

            // TS_UPDATE_ORDERS: orderData
            pduData.orderData = GetBytes(updateData, ref index, (updateData.Length - index));

            return pduData;
        }
        #endregion TS_UPDATE_ORDERS


        #region TS_UPDATE_BITMAP
        /// <summary>
        /// Parse TS_UPDATE_BITMAP
        /// </summary>
        /// <param name="updateData">update data (decompressed if were compressed)</param>
        /// <param name="shareDataHeader">share data header</param>
        /// <returns>TS_UPDATE_BITMAP</returns>
        private TS_UPDATE_BITMAP ParseTsUpdateBitmapPduData(
            byte[] updateData,
            TS_SHAREDATAHEADER shareDataHeader)
        {
            // index of update data
            int index = 0;
            TS_UPDATE_BITMAP pduData = new TS_UPDATE_BITMAP();

            // TS_UPDATE_BITMAP: shareDataHeader
            pduData.shareDataHeader = shareDataHeader;

            // TS_UPDATE_BITMAP: bitmapData
            pduData.bitmapData = ParseTsUpdateBitmapData(updateData, ref index);

            // Check if data length exceeded expectation
            VerifyDataLength(updateData.Length, index, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pduData;
        }


        /// <summary>
        /// Parse TS_UPDATE_BITMAP_DATA
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_UPDATE_BITMAP_DATA</returns>
        private TS_UPDATE_BITMAP_DATA ParseTsUpdateBitmapData(byte[] data, ref int currentIndex)
        {
            TS_UPDATE_BITMAP_DATA bitmapData = new TS_UPDATE_BITMAP_DATA();

            // TS_UPDATE_BITMAP: updateType
            bitmapData.updateType = ParseUInt16(data, ref currentIndex, false);

            // TS_UPDATE_BITMAP: numberRectangles
            bitmapData.numberRectangles = ParseUInt16(data, ref currentIndex, false);

            // TS_UPDATE_BITMAP: rectangles
            bitmapData.rectangles = new TS_BITMAP_DATA[bitmapData.numberRectangles];
            for (int i = 0; i < bitmapData.rectangles.Length; i++)
            {
                bitmapData.rectangles[i] = ParseTsBitmapData(data, ref currentIndex);
            }
            return bitmapData;
        }


        /// <summary>
        /// Parse TS_BITMAP_DATA
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_BITMAP_DATA</returns>
        private TS_BITMAP_DATA ParseTsBitmapData(byte[] data, ref int currentIndex)
        {
            TS_BITMAP_DATA bitmapData = new TS_BITMAP_DATA();

            // TS_BITMAP_DATA: destLeft
            bitmapData.destLeft = ParseUInt16(data, ref currentIndex, false);

            // TS_BITMAP_DATA: destTop
            bitmapData.destTop = ParseUInt16(data, ref currentIndex, false);

            // TS_BITMAP_DATA: destRight
            bitmapData.destRight = ParseUInt16(data, ref currentIndex, false);

            // TS_BITMAP_DATA: destBottom
            bitmapData.destBottom = ParseUInt16(data, ref currentIndex, false);

            // TS_BITMAP_DATA: width
            bitmapData.width = ParseUInt16(data, ref currentIndex, false);

            // TS_BITMAP_DATA: height
            bitmapData.height = ParseUInt16(data, ref currentIndex, false);

            // TS_BITMAP_DATA: bitsPerPixel
            bitmapData.bitsPerPixel = ParseUInt16(data, ref currentIndex, false);

            // TS_BITMAP_DATA: Flags
            bitmapData.Flags = (TS_BITMAP_DATA_Flags_Values)ParseUInt16(data, ref currentIndex, false);

            // TS_BITMAP_DATA: bitmapLength
            bitmapData.bitmapLength = ParseUInt16(data, ref currentIndex, false);

            // 32bpp is not supported
            if (bitmapData.bitsPerPixel == ConstValue.BITS_PER_PIXEL_32)
            {
                throw new NotSupportedException("32bpp compressed bitmaps are not supported!");
            }

            // If header is absent
            if (IsFlagExist((UInt16)bitmapData.Flags, (UInt16)TS_BITMAP_DATA_Flags_Values.NO_BITMAP_COMPRESSION_HDR))
            {
                // TS_BITMAP_DATA: bitmapDataStream
                bitmapData.bitmapDataStream = GetBytes(data, ref currentIndex, bitmapData.bitmapLength);
            }
            else
            {
                // reserve current parser index
                int reservedIndex = currentIndex;

                // TS_BITMAP_DATA: bitmapComprHdr
                bitmapData.bitmapComprHdr = ParseTsCdHeader(data, ref currentIndex);

                // TS_BITMAP_DATA: bitmapDataStream
                int remainLength = bitmapData.bitmapLength - (currentIndex - reservedIndex);
                bitmapData.bitmapDataStream = GetBytes(data, ref currentIndex, remainLength);
            }

            // Decompress if bitmapData were compressed
            if (IsFlagExist((UInt16)bitmapData.Flags, (UInt16)TS_BITMAP_DATA_Flags_Values.BITMAP_COMPRESSION))
            {
                RleDecompressor.Decompress(
                    bitmapData.bitmapDataStream,
                    (ColorDepth)bitmapData.bitsPerPixel,
                    bitmapData.width,
                    bitmapData.height);
            }
            return bitmapData;
        }


        /// <summary>
        /// Parse TS_CD_HEADER
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_CD_HEADER</returns>
        private TS_CD_HEADER ParseTsCdHeader(byte[] data, ref int currentIndex)
        {
            TS_CD_HEADER header = new TS_CD_HEADER();

            // TS_CD_HEADER: cbCompFirstRowSize
            header.cbCompFirstRowSize = (cbCompFirstRowSize_Values)ParseUInt16(data, ref currentIndex, false);

            // TS_CD_HEADER: cbCompMainBodySize
            header.cbCompMainBodySize = ParseUInt16(data, ref currentIndex, false);

            // TS_CD_HEADER: cbScanWidth
            header.cbScanWidth = ParseUInt16(data, ref currentIndex, false);

            // TS_CD_HEADER: cbUncompressedSize
            header.cbUncompressedSize = ParseUInt16(data, ref currentIndex, false);

            return header;
        }
        #endregion TS_UPDATE_BITMAP


        #region TS_UPDATE_PALETTE
        /// <summary>
        /// Parse TS_UPDATE_PALETTE
        /// </summary>
        /// <param name="updateData">update data (decompressed if were compressed)</param>
        /// <param name="shareDataHeader">share data header</param>
        /// <returns>TS_UPDATE_PALETTE</returns>
        private TS_UPDATE_PALETTE ParseTsUpdatePalettePduData(
            byte[] updateData,
            TS_SHAREDATAHEADER shareDataHeader)
        {
            // index of update data
            int index = 0;
            TS_UPDATE_PALETTE pduData = new TS_UPDATE_PALETTE();

            // TS_UPDATE_PALETTE: shareDataHeader
            pduData.shareDataHeader = shareDataHeader;

            // TS_UPDATE_PALETTE: paletteData
            pduData.paletteData = ParseTsUpdatePaletteData(updateData, ref index);

            // [Commented out for TDI #41402]
            // Check if data length exceeded expectation
            // VerifyDataLength(updateData.Length, index, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pduData;
        }


        /// <summary>
        /// parse TS_UPDATE_PALETTE_DATA
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_UPDATE_PALETTE_DATA</returns>
        private TS_UPDATE_PALETTE_DATA ParseTsUpdatePaletteData(byte[] data, ref int currentIndex)
        {
            TS_UPDATE_PALETTE_DATA paletteData = new TS_UPDATE_PALETTE_DATA();

            // TS_UPDATE_PALETTE: updateType
            paletteData.updateType = (updateType_Values)ParseUInt16(data, ref currentIndex, false);

            // TS_UPDATE_PALETTE: pad2Octets
            paletteData.pad2Octets = ParseUInt16(data, ref currentIndex, false);

            // TS_UPDATE_PALETTE: numberColors
            paletteData.numberColors = ParseUInt32(data, ref currentIndex, false);

            // TS_UPDATE_PALETTE: paletteData
            paletteData.paletteEntries = new TS_PALETTE_ENTRY[paletteData.numberColors];
            for (int i = 0; i < paletteData.paletteEntries.Length; i++)
            {
                paletteData.paletteEntries[i] = ParseTsPaletteEntry(data, ref currentIndex);
            }

            return paletteData;
        }

        /// <summary>
        /// Parse TS_PALETTE_ENTRY
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_PALETTE_ENTRY</returns>
        private TS_PALETTE_ENTRY ParseTsPaletteEntry(byte[] data, ref int currentIndex)
        {
            TS_PALETTE_ENTRY entry = new TS_PALETTE_ENTRY();

            // TS_PALETTE_ENTRY: red
            entry.red = ParseByte(data, ref currentIndex);

            // TS_PALETTE_ENTRY: green
            entry.green = ParseByte(data, ref currentIndex);

            // TS_PALETTE_ENTRY: blue
            entry.blue = ParseByte(data, ref currentIndex);

            return entry;
        }
        #endregion TS_UPDATE_PALETTE


        #region TS_UPDATE_SYNC
        /// <summary>
        /// Parse TS_UPDATE_SYNC
        /// </summary>
        /// <param name="updateData">update data (decompressed if were compressed)</param>
        /// <param name="shareDataHeader">share data header</param>
        /// <returns>TS_UPDATE_SYNC</returns>
        private TS_UPDATE_SYNC ParseTsUpdateSyncPduData(
            byte[] updateData,
            TS_SHAREDATAHEADER shareDataHeader)
        {
            // index of update data
            int index = 0;
            TS_UPDATE_SYNC pduData = new TS_UPDATE_SYNC();

            // TS_UPDATE_SYNC: shareDataHeader
            pduData.shareDataHeader = shareDataHeader;

            // TS_UPDATE_SYNC: updateType
            pduData.updateType = ParseUInt16(updateData, ref index, false);

            // TS_UPDATE_SYNC: pad2Octets
            pduData.pad2Octets = ParseUInt16(updateData, ref index, false);

            // Check if data length exceeded expectation
            VerifyDataLength(updateData.Length, index, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pduData;
        }
        #endregion TS_UPDATE_SYNC
        #endregion Sub Field Parsers: Slow-Path Graphics Update PDU


        #region Sub Field Parsers: Slow-Path Pointer Update PDU
        /// <summary>
        /// Parse Slow-Path Pointer Update PDU
        /// </summary>
        /// <param name="updateData">update data (decompressed if were compressed</param>
        /// <param name="shareDataHeader">share data header</param>
        /// <returns>Slow-Path Pointer Update PDU</returns>
        public TS_POINTER_PDU ParseTsPointerPdu(
            byte[] updateData,
            TS_SHAREDATAHEADER shareDataHeader)
        {
            // index of update data
            int index = 0;
            TS_POINTER_PDU pdu = new TS_POINTER_PDU();

            // TS_POINTER_PDU: shareDataHeader
            pdu.shareDataHeader = shareDataHeader;

            // TS_POINTER_PDU: messageType
            pdu.messageType = ParseUInt16(updateData, ref index, false);

            // TS_POINTER_PDU: pad2Octets
            pdu.pad2Octets = ParseUInt16(updateData, ref index, false);

            // TS_POINTER_PDU: pointerAttributeData
            int remainLength = updateData.Length - index;
            switch (pdu.messageType)
            {
                // TS_PTRMSGTYPE_SYSTEM: 4 bytes
                case (ushort)TS_POINTER_PDU_messageType_Values.TS_PTRMSGTYPE_SYSTEM:
                    pdu.pointerAttributeData = GetBytes(updateData, ref index,
                        ConstValue.TS_PTRMSGTYPE_SYSTEM_DATA_LENGTH);
                    break;

                // TS_PTRMSGTYPE_POSITION: 4 bytes
                case (ushort)TS_POINTER_PDU_messageType_Values.TS_PTRMSGTYPE_POSITION:
                    pdu.pointerAttributeData = GetBytes(updateData, ref index,
                        ConstValue.TS_PTRMSGTYPE_POSITION_DATA_LENGTH);
                    break;

                // TS_PTRMSGTYPE_COLOR: variable number of bytes
                case (ushort)TS_POINTER_PDU_messageType_Values.TS_PTRMSGTYPE_COLOR:
                    pdu.pointerAttributeData = GetBytes(updateData, ref index, remainLength);
                    break;

                // TS_PTRMSGTYPE_CACHED: 2 bytes
                case (ushort)TS_POINTER_PDU_messageType_Values.TS_PTRMSGTYPE_CACHED:
                    pdu.pointerAttributeData = GetBytes(updateData, ref index,
                        ConstValue.TS_PTRMSGTYPE_CACHED_DATA_LENGTH);
                    break;

                // TS_PTRMSGTYPE_POINTER: variable number of bytes
                case (ushort)TS_POINTER_PDU_messageType_Values.TS_PTRMSGTYPE_POINTER:
                    pdu.pointerAttributeData = GetBytes(updateData, ref index, remainLength);
                    break;

                default:
                    throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
            }

            // Check if data length exceeded expectation
            VerifyDataLength(updateData.Length, index, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion Sub Field Parsers: Slow-Path Pointer Update PDU


        #region Sub Field Parsers: Monitor Layout PDU
        /// <summary>
        /// Parse TS_MONITOR_DEF
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_MONITOR_DEF</returns>
        private TS_MONITOR_DEF ParseTsMonitorDef(byte[] data, ref int currentIndex)
        {
            TS_MONITOR_DEF def = new TS_MONITOR_DEF();

            // TS_MONITOR_DEF: left
            def.left = ParseUInt32(data, ref currentIndex, false);

            // TS_MONITOR_DEF: top
            def.top = ParseUInt32(data, ref currentIndex, false);

            // TS_MONITOR_DEF: right
            def.right = ParseUInt32(data, ref currentIndex, false);

            // TS_MONITOR_DEF: bottom
            def.bottom = ParseUInt32(data, ref currentIndex, false);

            // TS_MONITOR_DEF: flags
            def.flags = (Flags_TS_MONITOR_DEF)ParseUInt32(data, ref currentIndex, false);

            return def;
        }
        #endregion Sub Field Parsers: Monitor Layout PDU


        #region Sub Field Parsers: Server Redirection PDU
        /// <summary>
        /// Parse RDP_SERVER_REDIRECTION_PACKET
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>RDP_SERVER_REDIRECTION_PACKET</returns>
        private RDP_SERVER_REDIRECTION_PACKET ParseRdpServerRedirectionPacket(
            byte[] data,
            ref int currentIndex)
        {
            RDP_SERVER_REDIRECTION_PACKET packet = new RDP_SERVER_REDIRECTION_PACKET();

            int startIndex = currentIndex;

            // RDP_SERVER_REDIRECTION_PACKET: flags
            packet.Flags = (RDP_SERVER_REDIRECTION_PACKET_FlagsEnum)ParseUInt16(data, ref currentIndex, false);

            // RDP_SERVER_REDIRECTION_PACKET: length
            packet.Length = ParseUInt16(data, ref currentIndex, false);

            // RDP_SERVER_REDIRECTION_PACKET: sessionId
            packet.SessionID = ParseUInt32(data, ref currentIndex, false);

            // RDP_SERVER_REDIRECTION_PACKET: redirFlags
            packet.RedirFlags = (RedirectionFlags)ParseUInt32(data, ref currentIndex, false);

            if ((packet.RedirFlags & RedirectionFlags.LB_TARGET_NET_ADDRESS) == RedirectionFlags.LB_TARGET_NET_ADDRESS)
            {
                // RDP_SERVER_REDIRECTION_PACKET: targetNetAddress
                packet.TargetNetAddressLength = ParseUInt32(data, ref currentIndex, false);
                packet.TargetNetAddress = ParseUnicodeString(data, ref currentIndex, (int)packet.TargetNetAddressLength);
            }

            if ((packet.RedirFlags & RedirectionFlags.LB_LOAD_BALANCE_INFO) == RedirectionFlags.LB_LOAD_BALANCE_INFO)
            {
                // RDP_SERVER_REDIRECTION_PACKET: loadBalanceInfo
                packet.LoadBalanceInfoLength = ParseUInt32(data, ref currentIndex, false);
                packet.LoadBalanceInfo = GetBytes(data, ref currentIndex, (int)packet.LoadBalanceInfoLength);
            }

            if ((packet.RedirFlags & RedirectionFlags.LB_USERNAME) == RedirectionFlags.LB_USERNAME)
            {
                // RDP_SERVER_REDIRECTION_PACKET: userName
                packet.UserNameLength = ParseUInt32(data, ref currentIndex, false);
                packet.UserName = ParseUnicodeString(data, ref currentIndex, (int)packet.UserNameLength);
            }

            if ((packet.RedirFlags & RedirectionFlags.LB_DOMAIN) == RedirectionFlags.LB_DOMAIN)
            {
                // RDP_SERVER_REDIRECTION_PACKET: domain
                packet.DomainLength = ParseUInt32(data, ref currentIndex, false);
                packet.Domain = ParseUnicodeString(data, ref currentIndex, (int)packet.DomainLength);
            }

            if ((packet.RedirFlags & RedirectionFlags.LB_PASSWORD) == RedirectionFlags.LB_PASSWORD)
            {
                // RDP_SERVER_REDIRECTION_PACKET: password
                packet.PasswordLength = ParseUInt32(data, ref currentIndex, false);
                packet.Password = GetBytes(data, ref currentIndex, (int)packet.PasswordLength);
            }

            if ((packet.RedirFlags & RedirectionFlags.LB_TARGET_FQDN) == RedirectionFlags.LB_TARGET_FQDN)
            {
                // RDP_SERVER_REDIRECTION_PACKET: targetFqdn
                packet.TargetFQDNLength = ParseUInt32(data, ref currentIndex, false);
                packet.TargetFQDN = ParseUnicodeString(data, ref currentIndex, (int)packet.TargetFQDNLength);
            }

            if ((packet.RedirFlags & RedirectionFlags.LB_TARGET_NETBIOS_NAME)
                == RedirectionFlags.LB_TARGET_NETBIOS_NAME)
            {
                // RDP_SERVER_REDIRECTION_PACKET: targetNetBiosName
                packet.TargetNetBiosNameLength = ParseUInt32(data, ref currentIndex, false);
                packet.TargetNetBiosName = ParseUnicodeString(data, ref currentIndex, (int)packet.TargetNetBiosNameLength);
            }

            if (packet.RedirFlags.HasFlag(RedirectionFlags.LB_CLIENT_TSV_URL))
            {
                packet.TsvUrlLength = ParseUInt32(data, ref currentIndex, false);
                packet.TsvUrl = GetBytes(data, ref currentIndex, (int)packet.TsvUrlLength);
            }

            if ((packet.RedirFlags & RedirectionFlags.LB_TARGET_NET_ADDRESSES)
                == RedirectionFlags.LB_TARGET_NET_ADDRESSES)
            {
                // RDP_SERVER_REDIRECTION_PACKET: targetNetAddresses
                packet.TargetNetAddressesLength = ParseUInt32(data, ref currentIndex, false);
                packet.TargetNetAddresses.addressCount = ParseUInt32(data, ref currentIndex, false);
                packet.TargetNetAddresses.address = new TARGET_NET_ADDRESS[packet.TargetNetAddresses.addressCount];
                for (int i = 0; i < packet.TargetNetAddresses.addressCount; ++i)
                {
                    packet.TargetNetAddresses.address[i].addressLength = ParseUInt32(data, ref currentIndex, false);
                    packet.TargetNetAddresses.address[i].address = ParseUnicodeString(data, ref currentIndex, (int)packet.TargetNetAddresses.address[i].addressLength);
                }
            }

            // check if there's paddings
            int remainBytes = packet.Length - (currentIndex - startIndex);
            if (remainBytes > 0)
            {
                packet.Pad = GetBytes(data, ref currentIndex, remainBytes);
            }

            return packet;
        }
        #endregion Sub Field Parsers: Server Redirection PDU


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
        private TS_FP_UPDATE[] ParseTsFpUpdates(byte[] data, ref int currentIndex)
        {
            List<TS_FP_UPDATE> listUpdate = new List<TS_FP_UPDATE>();

            // One by one parse TS_FP_UPDATE
            while (currentIndex < data.Length)
            {
                TS_FP_UPDATE update = null;

                // Update header
                byte updateHeader = ParseByte(data, ref currentIndex);

                // Get infomation from updateHeader
                updateCode_Values updateCode;
                fragmentation_Value fragmentation;
                compression_Values compression;
                GetFpUpdateHeaderInfo(updateHeader, out updateCode, out fragmentation, out compression);

                // Get compressionFlags (optional)
                compressedType_Values compressionFlags = 0;
                if (compression_Values.FASTPATH_OUTPUT_COMPRESSION_USED == compression)
                {
                    compressionFlags = (compressedType_Values)ParseByte(data, ref currentIndex);
                }

                // Get size and update data
                UInt16 updateDataSize = ParseUInt16(data, ref currentIndex, false);
                byte[] updateData = GetBytes(data, ref currentIndex, updateDataSize);

                // Decompress update data (according to compressionFlags)
                byte[] decompressedUpdateData = clientContext.Decompress(updateData, compressionFlags);

                // Parse fast-path updates by updateCode
                switch (updateCode)
                {
                    // Fast-Path Orders Update
                    case updateCode_Values.FASTPATH_UPDATETYPE_ORDERS:
                        update = ParseTsFpUpdateOrders(updateHeader, compressionFlags,
                            updateDataSize, decompressedUpdateData);
                        break;

                    // Fast-Path Bitmap Update
                    case updateCode_Values.FASTPATH_UPDATETYPE_BITMAP:
                        update = ParseTsFpUpdateBitmap(updateHeader, compressionFlags,
                            updateDataSize, decompressedUpdateData);
                        break;

                    // Fast-Path Palette Update
                    case updateCode_Values.FASTPATH_UPDATETYPE_PALETTE:
                        update = ParseTsFpUpdatePalette(updateHeader, compressionFlags,
                            updateDataSize, decompressedUpdateData);
                        break;

                    // Fast-Path Synchronize Update 
                    case updateCode_Values.FASTPATH_UPDATETYPE_SYNCHRONIZE:
                        update = ParseTsFpUpdateSynchronize(
                            updateHeader, compressionFlags, updateDataSize);
                        break;

                    // Fast-Path Surface commands Update 
                    case updateCode_Values.FASTPATH_UPDATETYPE_SURFCMDS:
                        update = ParseTsFpSurfCmds(updateHeader, compressionFlags,
                            updateDataSize, decompressedUpdateData);
                        break;

                    // Fast-Path System Pointer Hidden Update
                    case updateCode_Values.FASTPATH_UPDATETYPE_PTR_NULL:
                        update = ParseTsFpSystemPointerHiddenAttribute(
                            updateHeader, compressionFlags, updateDataSize);
                        break;

                    // Fast-Path System Pointer Default Update
                    case updateCode_Values.FASTPATH_UPDATETYPE_PTR_DEFAULT:
                        update = ParseTsFpSystemPointerDefaultAttribute(
                            updateHeader, compressionFlags, updateDataSize);
                        break;

                    // Fast-Path Pointer Position Update 
                    case updateCode_Values.FASTPATH_UPDATETYPE_PTR_POSITION:
                        update = ParseTsFpPointerPosAttribute(updateHeader, compressionFlags,
                            updateDataSize, decompressedUpdateData);
                        break;

                    // Fast-Path Color Pointer Update 
                    case updateCode_Values.FASTPATH_UPDATETYPE_COLOR:
                        update = ParseTsFpColorPointerAttribute(updateHeader, compressionFlags,
                            updateDataSize, decompressedUpdateData);
                        break;

                    // Fast-Path Cached Pointer Update 
                    case updateCode_Values.FASTPATH_UPDATETYPE_CACHED:
                        update = ParseTsFpCachedPointerAttribute(updateHeader, compressionFlags,
                            updateDataSize, decompressedUpdateData);
                        break;

                    // Fast-Path New Pointer Update
                    case updateCode_Values.FASTPATH_UPDATETYPE_POINTER:
                        update = ParseTsFpPointerAttribute(updateHeader, compressionFlags,
                            updateDataSize, decompressedUpdateData);
                        break;

                    default:
                        throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
                }
                listUpdate.Add(update);
            }

            // Copy list to array
            TS_FP_UPDATE[] updates = new TS_FP_UPDATE[listUpdate.Count];
            for (int i = 0; i < updates.Length; i++)
            {
                updates[i] = listUpdate[i];
            }
            return updates;
        }


        #region Fast-Path Update Attribute Parsers
        /// <summary>
        /// Parse TS_FP_UPDATE_ORDERS
        /// </summary>
        /// <param name="updateHeader">update header</param>
        /// <param name="compressionFlags">compression flags</param>
        /// <param name="size">update data size(before decompression)</param>
        /// <param name="updateData">update data(decompressed)</param>
        /// <returns>TS_FP_UPDATE_ORDERS</returns>
        private TS_FP_UPDATE_ORDERS ParseTsFpUpdateOrders(
            byte updateHeader,
            compressedType_Values compressionFlags,
            UInt16 size,
            byte[] updateData)
        {
            TS_FP_UPDATE_ORDERS orders = new TS_FP_UPDATE_ORDERS();

            // TS_FP_UPDATE_ORDERS: updateHeader
            orders.updateHeader = updateHeader;

            // TS_FP_UPDATE_ORDERS: compressionFlags
            orders.compressionFlags = compressionFlags;

            // TS_FP_UPDATE_ORDERS: size
            orders.size = size;

            // TS_FP_UPDATE_ORDERS: updateOrders
            orders.updateOrders = updateData;

            return orders;
        }


        /// <summary>
        /// Parse TS_FP_UPDATE_BITMAP
        /// </summary>
        /// <param name="updateHeader">update header</param>
        /// <param name="compressionFlags">compression flags</param>
        /// <param name="size">update data size(before decompression)</param>
        /// <param name="updateData">update data(decompressed)</param>
        /// <returns>TS_FP_UPDATE_BITMAP</returns>
        private TS_FP_UPDATE_BITMAP ParseTsFpUpdateBitmap(
            byte updateHeader,
            compressedType_Values compressionFlags,
            UInt16 size,
            byte[] updateData)
        {
            TS_FP_UPDATE_BITMAP bitmap = new TS_FP_UPDATE_BITMAP();

            // TS_FP_UPDATE_BITMAP: updateHeader
            bitmap.updateHeader = updateHeader;

            // TS_FP_UPDATE_BITMAP: compressionFlags
            bitmap.compressionFlags = compressionFlags;

            // TS_FP_UPDATE_BITMAP: size
            bitmap.size = size;

            // TS_FP_UPDATE_BITMAP: bitmapUpdateData
            int index = 0;
            bitmap.bitmapUpdateData = ParseTsUpdateBitmapData(updateData, ref index);

            // Check if data length exceeded expectation
            VerifyDataLength(updateData.Length, index, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return bitmap;
        }


        /// <summary>
        /// Parse TS_FP_UPDATE_PALETTE
        /// </summary>
        /// <param name="updateHeader">update header</param>
        /// <param name="compressionFlags">compression flags</param>
        /// <param name="size">update data size(before decompression)</param>
        /// <param name="updateData">update data(decompressed)</param>
        /// <returns>TS_FP_UPDATE_PALETTE</returns>
        private TS_FP_UPDATE_PALETTE ParseTsFpUpdatePalette(
            byte updateHeader,
            compressedType_Values compressionFlags,
            UInt16 size,
            byte[] updateData)
        {
            TS_FP_UPDATE_PALETTE palette = new TS_FP_UPDATE_PALETTE();

            // TS_FP_UPDATE_PALETTE: updateHeader
            palette.updateHeader = updateHeader;

            // TS_FP_UPDATE_PALETTE: compressionFlags
            palette.compressionFlags = compressionFlags;

            // TS_FP_UPDATE_PALETTE: size
            palette.size = size;

            // TS_FP_UPDATE_PALETTE: paletteUpdateData
            int index = 0;
            palette.paletteUpdateData = ParseTsUpdatePaletteData(updateData, ref index);

            // Check if data length exceeded expectation
            VerifyDataLength(updateData.Length, index, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return palette;
        }


        /// <summary>
        /// Parse TS_FP_UPDATE_SYNCHRONIZE
        /// </summary>
        /// <param name="updateHeader">update header</param>
        /// <param name="compressionFlags">compression flags</param>
        /// <param name="size">update data size(before decompression)</param>
        /// <returns>TS_FP_UPDATE_SYNCHRONIZE</returns>
        private TS_FP_UPDATE_SYNCHRONIZE ParseTsFpUpdateSynchronize(
            byte updateHeader,
            compressedType_Values compressionFlags,
            UInt16 size)
        {
            TS_FP_UPDATE_SYNCHRONIZE sync = new TS_FP_UPDATE_SYNCHRONIZE();

            // TS_FP_UPDATE_SYNCHRONIZE: updateHeader
            sync.updateHeader = updateHeader;

            // TS_FP_UPDATE_SYNCHRONIZE: compressionFlags
            sync.compressionFlags = compressionFlags;

            // TS_FP_UPDATE_SYNCHRONIZE: size
            sync.size = size;

            return sync;
        }


        /// <summary>
        /// Parse TS_FP_SURFCMDS
        /// </summary>
        /// <param name="updateHeader">update header</param>
        /// <param name="compressionFlags">compression flags</param>
        /// <param name="size">update data size(before decompression)</param>
        /// <param name="updateData">update data(decompressed)</param>
        /// <returns>TS_FP_SURFCMDS</returns>
        private TS_FP_SURFCMDS ParseTsFpSurfCmds(
            byte updateHeader,
            compressedType_Values compressionFlags,
            UInt16 size,
            byte[] updateData)
        {
            TS_FP_SURFCMDS cmds = new TS_FP_SURFCMDS();

            // TS_FP_SURFCMDS: updateHeader
            cmds.updateHeader = updateHeader;

            // TS_FP_SURFCMDS: compressionFlags
            cmds.compressionFlags = compressionFlags;

            // TS_FP_SURFCMDS: size
            cmds.size = size;

            // TS_FP_SURFCMDS: surfaceCommands
            int index = 0;
            cmds.surfaceCommands = ParseTsSurfCmd(updateData, ref index);

            // Check if data length exceeded expectation
            VerifyDataLength(updateData.Length, index, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return cmds;
        }


        /// <summary>
        /// Parse TS_SURFCMD array
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SURFCMD array</returns>
        private TS_SURFCMD[] ParseTsSurfCmd(byte[] data, ref int currentIndex)
        {
            List<TS_SURFCMD_SET_SURF_BITS> listSurfCmd = new List<TS_SURFCMD_SET_SURF_BITS>();

            // One by one parse TS_SURFCMD_SET_SURF_BITS
            while (currentIndex < data.Length)
            {
                TS_SURFCMD_SET_SURF_BITS bits = ParseTsSurfCmdSetSurfBits(data, ref currentIndex);
                listSurfCmd.Add(bits);
            }

            // Copy list to array
            TS_SURFCMD[] surfCmds = new TS_SURFCMD[listSurfCmd.Count];
            for (int i = 0; i < surfCmds.Length; i++)
            {
                surfCmds[i] = listSurfCmd[i];
            }
            return surfCmds;
        }


        /// <summary>
        /// Parse TS_SURFCMD_SET_SURF_BITS
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_SURFCMD_SET_SURF_BITS</returns>
        private TS_SURFCMD_SET_SURF_BITS ParseTsSurfCmdSetSurfBits(byte[] data, ref int currentIndex)
        {
            TS_SURFCMD_SET_SURF_BITS bits = new TS_SURFCMD_SET_SURF_BITS();

            // TS_SURFCMD_SET_SURF_BITS: cmdType
            bits.cmdType = (cmdType_Values)ParseUInt32(data, ref currentIndex, false);

            // TS_SURFCMD_SET_SURF_BITS: destLeft
            bits.destLeft = ParseUInt16(data, ref currentIndex, false);

            // TS_SURFCMD_SET_SURF_BITS: destTop
            bits.destTop = ParseUInt16(data, ref currentIndex, false);

            // TS_SURFCMD_SET_SURF_BITS: destRight
            bits.destRight = ParseUInt16(data, ref currentIndex, false);

            // TS_SURFCMD_SET_SURF_BITS: destBottom
            bits.destBottom = ParseUInt16(data, ref currentIndex, false);

            // TS_SURFCMD_SET_SURF_BITS: bitmapData
            bits.bitmapData = ParseTsBitmapDataEx(data, ref currentIndex);

            return bits;
        }


        /// <summary>
        /// Parse TS_BITMAP_DATA_EX
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_BITMAP_DATA_EX</returns>
        private TS_BITMAP_DATA_EX ParseTsBitmapDataEx(byte[] data, ref int currentIndex)
        {
            TS_BITMAP_DATA_EX bitmapDataEx = new TS_BITMAP_DATA_EX();

            // TS_BITMAP_DATA_EX: bpp
            bitmapDataEx.bpp = ParseByte(data, ref currentIndex);

            // TS_BITMAP_DATA_EX: flags
            bitmapDataEx.flags = (TSBitmapDataExFlags_Values)ParseByte(data, ref currentIndex);

            // TS_BITMAP_DATA_EX: reserved
            bitmapDataEx.reserved = ParseByte(data, ref currentIndex);

            // TS_BITMAP_DATA_EX: codecID
            bitmapDataEx.codecID = ParseByte(data, ref currentIndex);

            // TS_BITMAP_DATA_EX: width
            bitmapDataEx.width = ParseUInt16(data, ref currentIndex, false);

            // TS_BITMAP_DATA_EX: height
            bitmapDataEx.height = ParseUInt16(data, ref currentIndex, false);

            // TS_BITMAP_DATA_EX: bitmapDataLength
            bitmapDataEx.bitmapDataLength = ParseUInt32(data, ref currentIndex, false);

            if (IsFlagExist((byte)bitmapDataEx.flags, (byte)TSBitmapDataExFlags_Values.EX_COMPRESSED_BITMAP_HEADER_PRESENT))
            {
                bitmapDataEx.exBitmapDataHeader = ParseExBitmapdataHeader(data, ref currentIndex);
            }

            // TS_BITMAP_DATA_EX: bitmapData
            bitmapDataEx.bitmapData = GetBytes(data, ref currentIndex, (int)bitmapDataEx.bitmapDataLength);

            return bitmapDataEx;
        }

        /// <summary>
        /// Parse TS_COMPRESSED_BITMAP_HEADER_EX
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns></returns>
        private TS_COMPRESSED_BITMAP_HEADER_EX ParseExBitmapdataHeader(
            byte[] data,
            ref int currentIndex)
        {
            TS_COMPRESSED_BITMAP_HEADER_EX exTsCompressedBitmapHeader = new TS_COMPRESSED_BITMAP_HEADER_EX();
            exTsCompressedBitmapHeader.highUniqueId = ParseUInt32(data, ref currentIndex, false);
            exTsCompressedBitmapHeader.lowUniqueId = ParseUInt32(data, ref currentIndex, false);
            exTsCompressedBitmapHeader.tmMilliseconds = ParseUInt64(data, ref currentIndex, false);
            exTsCompressedBitmapHeader.tmSeconds = ParseUInt64(data, ref currentIndex, false);

            return exTsCompressedBitmapHeader;
        }


        /// <summary>
        /// Parse TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE
        /// </summary>
        /// <param name="updateHeader">update header</param>
        /// <param name="compressionFlags">compression flags</param>
        /// <param name="size">update data size(before decompression)</param>
        /// <returns>TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE</returns>
        private TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE ParseTsFpSystemPointerHiddenAttribute(
            byte updateHeader,
            compressedType_Values compressionFlags,
            UInt16 size)
        {
            TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE hiddenAttribute = new TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE();

            // TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE: updateHeader
            hiddenAttribute.updateHeader = updateHeader;

            // TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE: compressionFlags
            hiddenAttribute.compressionFlags = compressionFlags;

            // TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE: size
            hiddenAttribute.size = size;

            return hiddenAttribute;
        }


        /// <summary>
        /// Parse TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE
        /// </summary>
        /// <param name="updateHeader">update header</param>
        /// <param name="compressionFlags">compression flags</param>
        /// <param name="size">update data size(before decompression)</param>
        /// <returns>TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE</returns>
        private TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE ParseTsFpSystemPointerDefaultAttribute(
            byte updateHeader,
            compressedType_Values compressionFlags,
            UInt16 size)
        {
            TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE defaultAttribute = new TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE();

            // TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE: updateHeader
            defaultAttribute.updateHeader = updateHeader;

            // TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE: compressionFlags
            defaultAttribute.compressionFlags = compressionFlags;

            // TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE: size
            defaultAttribute.size = size;

            return defaultAttribute;
        }


        /// <summary>
        /// Parse TS_FP_POINTERPOSATTRIBUTE
        /// </summary>
        /// <param name="updateHeader">update header</param>
        /// <param name="compressionFlags">compression flags</param>
        /// <param name="size">update data size(before decompression)</param>
        /// <param name="updateData">update data(decompressed)</param>
        /// <returns>TS_FP_POINTERPOSATTRIBUTE</returns>
        private TS_FP_POINTERPOSATTRIBUTE ParseTsFpPointerPosAttribute(
            byte updateHeader,
            compressedType_Values compressionFlags,
            UInt16 size,
            byte[] updateData)
        {
            TS_FP_POINTERPOSATTRIBUTE positionAttribute = new TS_FP_POINTERPOSATTRIBUTE();

            // TS_FP_POINTERPOSATTRIBUTE: updateHeader
            positionAttribute.updateHeader = updateHeader;

            // TS_FP_POINTERPOSATTRIBUTE: compressionFlags
            positionAttribute.compressionFlags = compressionFlags;

            // TS_FP_POINTERPOSATTRIBUTE: size
            positionAttribute.size = size;

            // TS_FP_POINTERPOSATTRIBUTE: pointerPositionUpdateData
            int index = 0;
            positionAttribute.pointerPositionUpdateData = ParseTsPointerPosAttribute(updateData, ref index);

            // Check if data length exceeded expectation
            VerifyDataLength(updateData.Length, index, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return positionAttribute;
        }


        /// <summary>
        /// Parse TS_POINTERPOSATTRIBUTE
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_POINTERPOSATTRIBUTE</returns>
        private TS_POINTERPOSATTRIBUTE ParseTsPointerPosAttribute(byte[] data, ref int currentIndex)
        {
            TS_POINTERPOSATTRIBUTE attribute = new TS_POINTERPOSATTRIBUTE();

            // TS_POINTERPOSATTRIBUTE: position
            attribute.position = ParseTsPoint16(data, ref currentIndex);

            return attribute;
        }


        /// <summary>
        /// Parse TS_POINT16
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_POINT16</returns>
        private TS_POINT16 ParseTsPoint16(byte[] data, ref int currentIndex)
        {
            TS_POINT16 point = new TS_POINT16();

            // TS_POINT16: xPos
            point.xPos = ParseUInt16(data, ref currentIndex, false);

            // TS_POINT16: yPos
            point.yPos = ParseUInt16(data, ref currentIndex, false);

            return point;
        }


        /// <summary>
        /// Parse TS_FP_COLORPOINTERATTRIBUTE
        /// </summary>
        /// <param name="updateHeader">update header</param>
        /// <param name="compressionFlags">compression flags</param>
        /// <param name="size">update data size(before decompression)</param>
        /// <param name="updateData">update data(decompressed)</param>
        /// <returns>TS_FP_COLORPOINTERATTRIBUTE</returns>
        private TS_FP_COLORPOINTERATTRIBUTE ParseTsFpColorPointerAttribute(
            byte updateHeader,
            compressedType_Values compressionFlags,
            UInt16 size,
            byte[] updateData)
        {
            TS_FP_COLORPOINTERATTRIBUTE attribute = new TS_FP_COLORPOINTERATTRIBUTE();

            // TS_FP_COLORPOINTERATTRIBUTE: updateHeader
            attribute.updateHeader = updateHeader;

            // TS_FP_COLORPOINTERATTRIBUTE: compressionFlags
            attribute.compressionFlags = compressionFlags;

            // TS_FP_COLORPOINTERATTRIBUTE: size
            attribute.size = size;

            // TS_FP_COLORPOINTERATTRIBUTE: colorPointerUpdateData
            int index = 0;
            attribute.colorPointerUpdateData = ParseTsColorPointerAttribute(updateData, ref index);

            // Check if data length exceeded expectation
            VerifyDataLength(updateData.Length, index, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return attribute;
        }


        /// <summary>
        /// Parse TS_COLORPOINTERATTRIBUTE
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_COLORPOINTERATTRIBUTE</returns>
        private TS_COLORPOINTERATTRIBUTE ParseTsColorPointerAttribute(byte[] data, ref int currentIndex)
        {
            TS_COLORPOINTERATTRIBUTE attribute = new TS_COLORPOINTERATTRIBUTE();

            // TS_COLORPOINTERATTRIBUTE: cacheIndex
            attribute.cacheIndex = ParseUInt16(data, ref currentIndex, false);

            // TS_COLORPOINTERATTRIBUTE: hotSpot
            attribute.hotSpot = ParseTsPoint16(data, ref currentIndex);

            // TS_COLORPOINTERATTRIBUTE: width
            attribute.width = ParseUInt16(data, ref currentIndex, false);

            // TS_COLORPOINTERATTRIBUTE: height
            attribute.height = ParseUInt16(data, ref currentIndex, false);

            // TS_COLORPOINTERATTRIBUTE: lengthAndMask
            attribute.lengthAndMask = ParseUInt16(data, ref currentIndex, false);

            // TS_COLORPOINTERATTRIBUTE: lengthXorMask
            attribute.lengthXorMask = ParseUInt16(data, ref currentIndex, false);

            // TS_COLORPOINTERATTRIBUTE: xorMaskData
            attribute.xorMaskData = GetBytes(data, ref currentIndex, (int)attribute.lengthXorMask);

            // TS_COLORPOINTERATTRIBUTE: andMaskData
            attribute.andMaskData = GetBytes(data, ref currentIndex, (int)attribute.lengthAndMask);

            // TS_COLORPOINTERATTRIBUTE: pad
            if (currentIndex < data.Length)
            {
                attribute.pad = ParseByte(data, ref currentIndex);
            }

            return attribute;
        }


        /// <summary>
        /// Parse TS_FP_CACHEDPOINTERATTRIBUTE
        /// </summary>
        /// <param name="updateHeader">update header</param>
        /// <param name="compressionFlags">compression flags</param>
        /// <param name="size">update data size(before decompression)</param>
        /// <param name="updateData">update data(decompressed)</param>
        /// <returns>TS_FP_CACHEDPOINTERATTRIBUTE</returns>
        private TS_FP_CACHEDPOINTERATTRIBUTE ParseTsFpCachedPointerAttribute(
            byte updateHeader,
            compressedType_Values compressionFlags,
            UInt16 size,
            byte[] updateData)
        {
            TS_FP_CACHEDPOINTERATTRIBUTE attribute = new TS_FP_CACHEDPOINTERATTRIBUTE();

            // TS_FP_CACHEDPOINTERATTRIBUTE: updateHeader
            attribute.updateHeader = updateHeader;

            // TS_FP_CACHEDPOINTERATTRIBUTE: compressionFlags
            attribute.compressionFlags = compressionFlags;

            // TS_FP_CACHEDPOINTERATTRIBUTE: size
            attribute.size = size;

            // TS_FP_CACHEDPOINTERATTRIBUTE: cachedPointerUpdateData
            int index = 0;
            attribute.cachedPointerUpdateData = ParseTsCachePointerAttribute(updateData, ref index);

            // Check if data length exceeded expectation
            VerifyDataLength(updateData.Length, index, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return attribute;
        }


        /// <summary>
        /// Parse TS_CACHEDPOINTERATTRIBUTE
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_CACHEDPOINTERATTRIBUTE</returns>
        private TS_CACHEDPOINTERATTRIBUTE ParseTsCachePointerAttribute(byte[] data, ref int currentIndex)
        {
            TS_CACHEDPOINTERATTRIBUTE attribute = new TS_CACHEDPOINTERATTRIBUTE();

            // TS_CACHEDPOINTERATTRIBUTE: cacheIndex
            attribute.cacheIndex = ParseUInt16(data, ref currentIndex, false);

            return attribute;
        }


        /// <summary>
        /// Parse TS_FP_POINTERATTRIBUTE
        /// </summary>
        /// <param name="updateHeader">update header</param>
        /// <param name="compressionFlags">compression flags</param>
        /// <param name="size">update data size(before decompression)</param>
        /// <param name="updateData">update data(decompressed)</param>
        /// <returns>TS_FP_POINTERATTRIBUTE</returns>
        private TS_FP_POINTERATTRIBUTE ParseTsFpPointerAttribute(
            byte updateHeader,
            compressedType_Values compressionFlags,
            UInt16 size,
            byte[] updateData)
        {
            TS_FP_POINTERATTRIBUTE attribute = new TS_FP_POINTERATTRIBUTE();

            // TS_FP_POINTERATTRIBUTE: updateHeader
            attribute.updateHeader = updateHeader;

            // TS_FP_POINTERATTRIBUTE: compressionFlags
            attribute.compressionFlags = compressionFlags;

            // TS_FP_POINTERATTRIBUTE: size
            attribute.size = size;

            // TS_FP_POINTERATTRIBUTE: newPointerUpdateData
            int index = 0;
            attribute.newPointerUpdateData = ParseTsPointerAttribute(updateData, ref index);

            // [Commented out for TDI #41402]
            // Check if data length exceeded expectation
            // VerifyDataLength(updateData.Length, index, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return attribute;
        }


        /// <summary>
        /// Parse TS_POINTERATTRIBUTE
        /// (parser index is updated according to parsed length)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="currentIndex">current parser index</param>
        /// <returns>TS_POINTERATTRIBUTE</returns>
        private TS_POINTERATTRIBUTE ParseTsPointerAttribute(byte[] data, ref int currentIndex)
        {
            TS_POINTERATTRIBUTE attribute = new TS_POINTERATTRIBUTE();

            // TS_POINTERATTRIBUTE: xorBpp
            attribute.xorBpp = ParseUInt16(data, ref currentIndex, false);

            // TS_POINTERATTRIBUTE: attribute
            attribute.colorPtrAttr = ParseTsColorPointerAttribute(data, ref currentIndex);

            return attribute;
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
        private byte[] DecryptFastPathUpdateData(byte[] remainData, byte[] signatureData, bool isSalted)
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
                if (!clientContext.Decrypt(remainData, signatureData, isSalted, out decryptedData))
                {
                    // Decryptioin failed
                    throw new FormatException(ConstValue.ERROR_MESSAGE_DECRYPTION_FAILED);
                }
                return decryptedData;
            }
        }


        /// <summary>
        /// Get information from Fast-path Output Header
        /// </summary>
        /// <param name="fpOutputHeader">fast-path output header</param>
        /// <param name="actionCode">action code</param>
        /// <param name="encryptionFlags">encryption flags</param>
        private void GetFpOutputHeaderInfo(
            byte fpOutputHeader,
            out nested_TS_FP_UPDATE_PDU_fpOutputHeader_actionCode_Values actionCode,
            out encryptionFlagsChgd_Values encryptionFlags)
        {
            // The following logic is derived from TD section [2.2.9.1.2]
            // fpOutputHeader is a 1-byte, bit-packed field formed by:
            // actionCode(2 bits) + reserved(4 bits) + encryptionFlags(2 bits)

            // action code
            byte code = (byte)(fpOutputHeader & 0x03);
            actionCode = (nested_TS_FP_UPDATE_PDU_fpOutputHeader_actionCode_Values)code;

            // encryption flags
            byte flags = (byte)((fpOutputHeader & 0xc0) >> 6);
            encryptionFlags = (encryptionFlagsChgd_Values)flags;

            return;
        }


        /// <summary>
        /// Get information from Fast-path Update Header
        /// </summary>
        /// <param name="updateHeader">update header</param>
        /// <param name="updateCode">update code</param>
        /// <param name="fragmentation">fragmentation</param>
        /// <param name="compression">compression</param>
        private void GetFpUpdateHeaderInfo(
          byte updateHeader,
          out updateCode_Values updateCode,
          out fragmentation_Value fragmentation,
          out compression_Values compression)
        {
            // The following logic is derived from TD section [2.2.9.1.2.1]
            // updateHeader is a 1-byte, bit-packed field formed by:
            // updateCode(4 bits) + fragmentation(2 bits) + compression(2 bits)

            // updateCode
            byte code = (byte)(updateHeader & 0x0f);
            updateCode = (updateCode_Values)code;

            // fragmentation
            byte frag = (byte)((updateHeader & 0x30) >> 4);
            fragmentation = (fragmentation_Value)frag;

            // compression
            byte comp = (byte)((updateHeader & 0xc0) >> 6);
            compression = (compression_Values)comp;

            return;
        }
        #endregion Fast-Path Update Parsers' helper functions
        #endregion Sub Field Parsers: Fast-Path Update PDU

        #region Sub Field Parsers: others

        /// <summary>
        /// Parse NETWORK_DETECTION_REQUEST
        /// </summary>
        /// <param name="data"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        public NETWORK_DETECTION_REQUEST ParseNetworkDectectRequest(byte[] data, ref int currentIndex)
        {
            NETWORK_DETECTION_REQUEST detectRequest = null;
            byte headerLength = ParseByte(data, ref currentIndex);
            HeaderTypeId_Values headerTypeId = (HeaderTypeId_Values)ParseByte(data, ref currentIndex);
            ushort sequenceNumber = ParseUInt16(data, ref currentIndex, false);
            AUTO_DETECT_REQUEST_TYPE requestType = (AUTO_DETECT_REQUEST_TYPE)ParseUInt16(data, ref currentIndex, false);

            if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_RTT_REQUEST_IN_CONNECTTIME
                || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_RTT_REQUEST_AFTER_CONNECTTIME)
            {
                RDP_RTT_REQUEST req = new RDP_RTT_REQUEST();
                req.headerLength = headerLength;
                req.headerTypeId = headerTypeId;
                req.sequenceNumber = sequenceNumber;
                req.requestType = requestType;

                detectRequest = req;
            }
            else if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_IN_CONNECTTIME
                || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_AFTER_CONNECTTIME_OR_RELIABLEUDP
                || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_AFTER_CONNECTTIME_OR_LOSSYUDP)
            {
                RDP_BW_START req = new RDP_BW_START();
                req.headerLength = headerLength;
                req.headerTypeId = headerTypeId;
                req.sequenceNumber = sequenceNumber;
                req.requestType = requestType;

                detectRequest = req;
            }
            else if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_PAYLOAD)
            {
                RDP_BW_PAYLOAD req = new RDP_BW_PAYLOAD();
                req.headerLength = headerLength;
                req.headerTypeId = headerTypeId;
                req.sequenceNumber = sequenceNumber;
                req.requestType = requestType;
                req.payloadLength = ParseUInt16(data, ref currentIndex, false);
                req.payload = GetBytes(data, ref currentIndex, req.payloadLength);

                detectRequest = req;
            }
            else if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_IN_CONNECTTIME
                || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_AFTER_CONNECTTIME_OR_RELIABLEUDP
                || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_AFTER_CONNECTTIME_OR_LOSSYUDP)
            {
                RDP_BW_STOP req = new RDP_BW_STOP();
                req.headerLength = headerLength;
                req.headerTypeId = headerTypeId;
                req.sequenceNumber = sequenceNumber;
                req.requestType = requestType;
                if (req.headerLength > 0x06)
                {
                    req.payloadLength = ParseUInt16(data, ref currentIndex, false);
                    req.payload = GetBytes(data, ref currentIndex, req.payloadLength);
                }

                detectRequest = req;
            }
            else
            {
                RDP_NETCHAR_RESULT req = new RDP_NETCHAR_RESULT();
                req.headerLength = headerLength;
                req.headerTypeId = headerTypeId;
                req.sequenceNumber = sequenceNumber;
                req.requestType = requestType;
                if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BANDWIDTH_AVERAGERTT)
                {
                    req.bandwidth = ParseUInt32(data, ref currentIndex, false);
                    req.averageRTT = ParseUInt32(data, ref currentIndex, false);
                }
                else if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_AVERAGERTT)
                {
                    req.baseRTT = ParseUInt32(data, ref currentIndex, false);
                    req.averageRTT = ParseUInt32(data, ref currentIndex, false);
                }
                else if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_BANDWIDTH_AVERAGERTT)
                {
                    req.baseRTT = ParseUInt32(data, ref currentIndex, false);
                    req.bandwidth = ParseUInt32(data, ref currentIndex, false);
                    req.averageRTT = ParseUInt32(data, ref currentIndex, false);
                }
                detectRequest = req;
            }


            return detectRequest;
        }

        #endregion Sub Field Parsers: others
        #endregion Private Methods: PDU Sub Field Parsers


        #region Private Methods: PDU Decoder Switches
        #region Switch level 1
        /// <summary>
        /// Switch Decode MCS PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>decode MCS PDU</returns>
        private StackPacket SwitchDecodeMcsPDU(byte[] data)
        {
            // Check data length
            if (ConstValue.MCS_CONNECT_RESPONSE_PDU_INDICATOR_INDEX >= data.Length)
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_DATA_INDEX_OUT_OF_RANGE);
            }

            // Decode by MCS PDU Type
            StackPacket pdu = null;
            byte mcsPduType = data[ConstValue.MCS_CONNECT_RESPONSE_PDU_INDICATOR_INDEX];
            if (ConstValue.MCS_CONNECT_RESPONSE_PDU_INDICATOR_VALUE == mcsPduType)
            {
                // Decode MCS Connect Response PDU
                pdu = DecodeMcsConnectResponsePDU(data);
            }
            else
            {
                // Decode MCS Domain PDU
                pdu = SwitchDecodeMcsDomainPDU(data);
            }
            return pdu;
        }
        #endregion Switch Level 1


        #region Switch Level 2
        /// <summary>
        /// Switch Decode MCS Domain PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>decoded MCS Domain PDU</returns>
        private StackPacket SwitchDecodeMcsDomainPDU(byte[] data)
        {
            // Skip TpktHeader and X224Data
            int tempIndex = ConstValue.TPKT_HEADER_AND_X224_DATA_LENGTH;

            // Decode Domain MCS PDU
            DomainMCSPDU domainPdu = ParseMcsDomainPdu(data, ref tempIndex);

            // Switch decoders by pdu element name 
            StackPacket pdu = null;
            switch (domainPdu.ElemName)
            {
                // Channel Join Confirm PDU
                case ConstValue.MCS_DOMAIN_PDU_NAME_CHANNEL_JOIN_CONFIRM:
                    pdu = DecodeMcsChannelJoinConfirmPDU(data);
                    break;

                // Send Data Indication PDU
                case ConstValue.MCS_DOMAIN_PDU_NAME_SEND_DATA_INDICATION:
                    pdu = DecodeMcsSendDataIndicationPDU(data, domainPdu);
                    break;

                // Attach User Confirm PDU
                case ConstValue.MCS_DOMAIN_PDU_NAME_ATTACH_USER_CONFIRM:
                    pdu = DecodeMcsAttachUserConfirmPDU(data);
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
        /// Decode MCS Send Data Indication PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="domainPdu">Mcs Domain PDU</param>
        /// <returns>decoded MCS Send Data Indication PDU</returns>
        private StackPacket DecodeMcsSendDataIndicationPDU(byte[] data, DomainMCSPDU domainPdu)
        {
            // Get Send Data Indication
            SendDataIndication indication = (SendDataIndication)domainPdu.GetData();
            byte[] userData = indication.userData.ByteArrayValue;

            // Get Security Header Type
            SecurityHeaderType securityHeaderType = GetSecurityHeaderTypeByContext();

            // Peek Security Header
            bool isLicenseErrorPdu = false;
            if (clientContext.IsWaitingLicenseErrorPdu)
            {
                isLicenseErrorPdu = IsLicenseErrorSecurityHeaderExist(userData);
            }

            // Decode PDU
            if (isLicenseErrorPdu)
            {
                // License Error PDU's Basic Security Header content is alway present
                int tempIndex = 0;
                TS_SECURITY_HEADER header = ParseTsSecurityHeader(userData, ref tempIndex, SecurityHeaderType.Basic);

                // Check if PDU is encrypted
                bool isEncryptedPdu = IsFlagExist((UInt16)header.flags,
                    (UInt16)TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT);

                // Correct Server License Error PDU's security header type if needed
                if (!isEncryptedPdu || EncryptionLevel.ENCRYPTION_LEVEL_NONE == clientContext.RdpEncryptionLevel)
                {
                    securityHeaderType = SecurityHeaderType.Basic;
                }

                // Get decrypted user data
                byte[] decryptedUserData = DecryptSendDataIndication(userData, securityHeaderType);

                // Decode Server License Error PDU
                return DecodeLicenseErrorPDU(data, decryptedUserData, securityHeaderType);
            }
            else
            {
                // Get decrypted user data
                byte[] decryptedUserData = DecryptSendDataIndication(userData, securityHeaderType);

                if (clientContext.MessageChannelId == indication.channelId.Value)
                {
                    if (IsInitiateMultitransportRequestSecurityHeaderExist(userData))
                    {
                        return DecodeServerInitiateMultitransportRequest(data, decryptedUserData, securityHeaderType);
                    }
                    else if (IsHeartbeatSecurityHeaderExist(userData))
                    {
                        return DecodeServerHeartbeatPDU(data, decryptedUserData, securityHeaderType);
                    }
                    else
                    {
                        return DecodeServerAutoDetectRequestPDU(data, decryptedUserData, securityHeaderType);
                    }
                }
                // Check channel ID (IO Channel ID/Virual Channel ID)
                else if (clientContext.IOChannelId != indication.channelId.Value)
                {
                    // Decode Virtual Channel PDU
                    return DecodeVirtualChannelPDU(data, decryptedUserData, securityHeaderType);
                }
                else if (IsStandardRedirectionPdu(userData, securityHeaderType))
                {
                    return DecodeServerRedirectionPDU(data, decryptedUserData, securityHeaderType);
                }
                else
                {
                    // Decode other Send Data Indication PDUs
                    return SwitchDecodeMcsSendDataIndicationPDU(data, decryptedUserData, securityHeaderType);
                }
            }
        }
        #endregion Switch Level 3


        #region Switch Level 4
        /// <summary>
        /// Switch Decode MCS Send Data Indication PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data</param>
        /// <param name="securityHeaderType">security header type</param>
        /// <returns>decoded MCS Send Data Indication PDU</returns>
        private StackPacket SwitchDecodeMcsSendDataIndicationPDU(
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
                // Demand Active PDU
                case ShareControlHeaderType.PDUTYPE_DEMANDACTIVEPDU:
                    pdu = DecodeDemandActivePDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Deactivate All PDU
                case ShareControlHeaderType.PDUTYPE_DEACTIVATEALLPDU:
                    pdu = DecodeDeactivateAllPDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Data PDU
                case ShareControlHeaderType.PDUTYPE_DATAPDU:
                    pdu = SwitchDecodeMcsDataPDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Server Redirection PDU
                case ShareControlHeaderType.PDUTYPE_SERVER_REDIR_PKT:
                    pdu = DecodeEnhancedServerRedirectionPDU(data, decryptedUserData, securityHeaderType);
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
                // Synchronize PDU
                case pduType2_Values.PDUTYPE2_SYNCHRONIZE:
                    pdu = DecodeSynchronizePDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Control PDU
                case pduType2_Values.PDUTYPE2_CONTROL:
                    pdu = DecodeControlPDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Font Map PDU
                case pduType2_Values.PDUTYPE2_FONTMAP:
                    pdu = DecodeFontMapPDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Shutdown-Denied PDU
                case pduType2_Values.PDUTYPE2_SHUTDOWN_DENIED:
                    pdu = DecodeShutdownRequestDeniedPDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Set Error Info PDU
                case pduType2_Values.PDUTYPE2_SET_ERROR_INFO_PDU:
                    pdu = DecodeSetErrorInfoPDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Save Session Info PDU
                case pduType2_Values.PDUTYPE2_SAVE_SESSION_INFO:
                    pdu = DecodeSaveSessionInfoPDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Play Sound PDU
                case pduType2_Values.PDUTYPE2_PLAY_SOUND:
                    pdu = DecodePlaySoundPDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Set Keyboard Indicators PDU
                case pduType2_Values.PDUTYPE2_SET_KEYBOARD_INDICATORS:
                    pdu = DecodeSetKeyboardIndicatorsPDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Set Keyboard IME Status PDU
                case pduType2_Values.PDUTYPE2_SET_KEYBOARD_IME_STATUS:
                    pdu = DecodeSetKeyboardImeStatusPDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Update PDU
                case pduType2_Values.PDUTYPE2_UPDATE:
                    pdu = DecodeSlowPathUpdatePDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Pointer PDU
                case pduType2_Values.PDUTYPE2_POINTER:
                    pdu = DecodeSlowPathUpdatePDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Status Info PDU
                case pduType2_Values.PDUTYPE2_STATUS_INFO_PDU:
                    pdu = DecodeStatusInfoPDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Monitor Layout PDU
                case pduType2_Values.PDUTYPE2_MONITOR_LAYOUT_PDU:
                    pdu = DecodeMonitorLayoutPDU(data, decryptedUserData, securityHeaderType);
                    break;

                // Monitor Layout PDU
                case pduType2_Values.PDUTYPE2_ARC_STATUS_PDU:
                    pdu = DecodeAutoReconnectStatusPDU(data, decryptedUserData, securityHeaderType);
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

            // Get bytes for only one packet
            byte[] packetData = GetPacket(receivedBytes);
            if (packetData == null || packetData.Length == 0)
            {
                // Received bytes does not contain enough data
                consumedLength = 0;
                return null;
            }
            consumedLength = packetData.Length;

            // Decode PDU
            try
            {
                pdu = DecodePdu(packetData);
            }
            catch (Exception e)
            {
                pdu = new ErrorPdu(e);
            }

            // Update client context and client
            clientContext.UpdateContext(pdu);
            client.CheckDecryptionCount();
            return new StackPacket[] { pdu };
        }


        /// <summary>
        /// Get a complete packet buffer from received bytes
        /// </summary>
        /// <param name="receivedBytes">received bytes</param>
        /// <returns>data buffer contains a complete packet</returns>
        private byte[] GetPacket(byte[] receivedBytes)
        {
            if (receivedBytes == null || receivedBytes.Length == 0)
            {
                return null;
            }

            // Get packet length according to PDU type (slow-path/fast-path)
            int packetLength = 0;

            if (clientContext.IsExpectingEarlyUserAuthorizationResultPDU)
            {
                var pduUsedToCalculateSize = new Early_User_Authorization_Result_PDU();

                int expectedLength = pduUsedToCalculateSize.ToBytes().Length;

                if (receivedBytes.Length < expectedLength)
                {
                    // Not enough data for Early User Authorization Result PDU.
                    return null;
                }

                packetLength = expectedLength;
            }
            else if (ConstValue.SLOW_PATH_PDU_INDICATOR_VALUE == receivedBytes[ConstValue.SLOW_PATH_PDU_INDICATOR_INDEX])
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



        #endregion Private Methods: Decoder Callback


        #region Public Methods: PDU Decoder Entrance
        /// <summary>
        /// Decode PDU 
        /// (entrance method for decoders)
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>decoded PDU</returns>
        public StackPacket DecodePdu(byte[] data)
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

            StackPacket pdu = null;

            if (clientContext.IsAuthenticatingRDSTLS)
            {
                // switch RDSTLS PDU
                pdu = SwitchRDSTLSAuthenticationPDU(data);
            }
            else if (clientContext.IsExpectingEarlyUserAuthorizationResultPDU)
            {
                var pduUsedToCalculateSize = new Early_User_Authorization_Result_PDU();

                int expectedLength = pduUsedToCalculateSize.ToBytes().Length;

                if (data.Length != expectedLength)
                {
                    // Inconsistent size for Early User Authorization Result PDU.
                    throw new FormatException(ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
                }

                pdu = DecodeEarlyUserAuthorizationResultPDU(data);

                // Reset since one Early User Authorization Result PDU is expected.
                clientContext.IsExpectingEarlyUserAuthorizationResultPDU = false;
            }
            else
            {
                // Check slow-path/fast-path type
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
                        case X224_TPDU_TYPE.ConnectionConfirm:
                            pdu = DecodeX224ConnectionConfirmPDU(data);
                            break;

                        // MCS PDU
                        case X224_TPDU_TYPE.Data:
                            pdu = SwitchDecodeMcsPDU(data);
                            break;

                        default:
                            throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
                    }
                }
                else
                {
                    // Fast-Path Situation
                    pdu = DecodeTsFpUpdatePDU(data);
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
        /// [TD Reference 3.2.5.3.2]
        /// Decode X.224 Connection Confirm PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>decoded X.224 Connection Confirm PDU</returns>
        public StackPacket DecodeX224ConnectionConfirmPDU(byte[] data)
        {
            // initialize
            int currentIndex = 0;
            StackPacket pdu = null;

            // TpktHeader
            TpktHeader tpktHeader = ParseTpktHeader(data, ref currentIndex);

            // X224Ccf
            X224Ccf x224Ccf = ParseX224Ccf(data, ref currentIndex);

            // RDP_NEG_RSP/RDP_NEG_FAILURE (optional field)
            if (currentIndex < data.Length)
            {
                // check type
                byte pduType = data[currentIndex];
                if (pduType == (byte)RDP_NEG_RSP_type_Values.V1)
                {
                    // X224 Connection Confirm PDU
                    Server_X_224_Connection_Confirm_Pdu confirmPdu = new Server_X_224_Connection_Confirm_Pdu();

                    // Server_X_224_Connection_Confirm_Pdu: tpktHeader & x224Ccf
                    confirmPdu.tpktHeader = tpktHeader;
                    confirmPdu.x224Ccf = x224Ccf;

                    // Server_X_224_Connection_Confirm_Pdu: rdpNegData
                    confirmPdu.rdpNegData = ParseRdpNegRsp(data, ref currentIndex);

                    pdu = confirmPdu;
                }
                else if (pduType == (byte)RDP_NEG_FAILURE_type_Values.V1)
                {
                    // X224 Negotiate Failure PDU
                    Server_X_224_Negotiate_Failure_Pdu failurePdu = new Server_X_224_Negotiate_Failure_Pdu();

                    // Server_X_224_Negotiate_Failure_Pdu: tpktHeader & x224Ccf
                    failurePdu.tpktHeader = tpktHeader;
                    failurePdu.x224Ccf = x224Ccf;

                    // Server_X_224_Negotiate_Failure_Pdu: rdpNegFailure
                    failurePdu.rdpNegFailure = ParseRdpNegFailure(data, ref currentIndex);

                    pdu = failurePdu;
                }
                else
                {
                    throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
                }
            }
            else
            {
                // X224 Connection Confirm PDU
                Server_X_224_Connection_Confirm_Pdu confirmPdu = new Server_X_224_Connection_Confirm_Pdu();

                // Server_X_224_Connection_Confirm_Pdu: tpktHeader
                confirmPdu.tpktHeader = tpktHeader;

                // Server_X_224_Connection_Confirm_Pdu: x224Ccf
                confirmPdu.x224Ccf = x224Ccf;

                // Server_X_224_Connection_Confirm_Pdu: rdpNegData (absent)
                confirmPdu.rdpNegData = null;

                pdu = confirmPdu;
            }

            // Check if data length exceeded expectation
            VerifyDataLength(data.Length, currentIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }

        /// <summary>
        /// Decode Early User Authorization Result PDU.
        /// </summary>
        /// <param name="data">Data containing the PDU to be decoded.</param>
        /// <returns>A decoded Early User Authorization Result PDU.</returns>
        public StackPacket DecodeEarlyUserAuthorizationResultPDU(byte[] data)
        {
            int currentIndex = 0;

            var earlyUserAuthorizationResultPDU = new Early_User_Authorization_Result_PDU();

            earlyUserAuthorizationResultPDU.authorizationResult = (Authorization_Result_value)ParseUInt32(data, ref currentIndex, false);


            // Check if data length exceeded expectation.
            VerifyDataLength(data.Length, currentIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);

            return earlyUserAuthorizationResultPDU;
        }

        /// <summary>
        /// [TD Reference 3.2.5.3.4]
        /// Decode MCS Connect Response PDU with GCC Conference Create Response
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>decoded MCS Connect Response PDU with GCC Conference Create Response PDU</returns>
        public StackPacket DecodeMcsConnectResponsePDU(byte[] data)
        {
            // initialize
            int currentIndex = 0;
            Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response pdu =
                new Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response();

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
            Connect_Response mcsConnectResponse = new Connect_Response();
            Asn1DecodingBuffer decodeBuffer = new Asn1DecodingBuffer(t125Data);
            mcsConnectResponse.BerDecode(decodeBuffer);

            // McsConnectResponse:result
            pdu.mcsCrsp.result = (int)mcsConnectResponse.result.Value;
            byte[] userData = mcsConnectResponse.userData.ByteArrayValue;

            // T125 Data: decode McsConnectResponse's user data
            Asn1DecodingBuffer connectDataBuffer = new Asn1DecodingBuffer(userData);
            ConnectData connectData = new ConnectData();
            connectData.PerDecode(connectDataBuffer);

            // T125 Data: get Gcc data
            int gccDataLength = userData.Length - ConstValue.GCC_DATA_OFFSET;
            if (gccDataLength <= 0)
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_DATA_INDEX_OUT_OF_RANGE);
            }
            byte[] gccData = new byte[gccDataLength];
            Array.Copy(userData, ConstValue.GCC_DATA_OFFSET, gccData, 0, gccData.Length);

            // T125 Data: decode Gcc user data
            ConnectGCCPDU gccPdu = new ConnectGCCPDU();
            Asn1DecodingBuffer gccPduBuffer = new Asn1DecodingBuffer(gccData);
            gccPdu.PerDecode(gccPduBuffer);

            // McsConnectResponse: H221Key
            ConferenceCreateResponse conferenceResponse = (ConferenceCreateResponse)gccPdu.GetData();
            H221NonStandardIdentifier identifier =
                (H221NonStandardIdentifier)conferenceResponse.userData.Elements[0].key.GetData();
            pdu.mcsCrsp.gccPdu.H221Key = Encoding.ASCII.GetString(identifier.ByteArrayValue);

            // McsConnectResponse: ccrResult
            pdu.mcsCrsp.gccPdu.ccrResult = (int)conferenceResponse.result.Value;

            // McsConnectResponse: nodeID
            pdu.mcsCrsp.gccPdu.nodeID = (int)conferenceResponse.nodeID.Value;

            // McsConnectResponse: tag
            pdu.mcsCrsp.gccPdu.tag = (int)conferenceResponse.tag.Value;

            // T125 Data: get Gcc user data
            byte[] gccUserData = conferenceResponse.userData.Elements[0].value.ByteArrayValue;

            // Reset current index
            currentIndex = 0;
            while (currentIndex < gccUserData.Length)
            {
                // Peek data type
                int tempIndex = currentIndex;
                TS_UD_HEADER_type_Values type =
                    (TS_UD_HEADER_type_Values)ParseUInt16(gccUserData, ref tempIndex, false);

                // Parse data by type
                switch (type)
                {
                    case TS_UD_HEADER_type_Values.SC_CORE:
                        pdu.mcsCrsp.gccPdu.serverCoreData = ParseTsUdScCore(gccUserData, ref currentIndex);
                        break;

                    case TS_UD_HEADER_type_Values.SC_NET:
                        pdu.mcsCrsp.gccPdu.serverNetworkData = ParseTsUdScNet(gccUserData, ref currentIndex);
                        break;

                    case TS_UD_HEADER_type_Values.SC_SECURITY:
                        pdu.mcsCrsp.gccPdu.serverSecurityData = ParseTsUdScSec1(gccUserData, ref currentIndex);
                        break;

                    case TS_UD_HEADER_type_Values.SC_MCS_MSGCHANNEL:
                        pdu.mcsCrsp.gccPdu.serverMessageChannelData = ParseTsUdScMSGChannel(gccUserData, ref currentIndex);
                        break;

                    case TS_UD_HEADER_type_Values.SC_MULTITRANSPORT:
                        pdu.mcsCrsp.gccPdu.serverMultitransportChannelData = ParseTsUdScMultiTransport(gccUserData, ref currentIndex);
                        break;

                    default:
                        throw new FormatException(ConstValue.ERROR_MESSAGE_ENUM_UNRECOGNIZED);
                }
            }

            // Check if data length exceeded expectation
            VerifyDataLength(gccUserData.Length, currentIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// [TD Reference 3.2.5.3.7]
        /// Decode MCS Attach User Confirm PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>decoded MCS Attach User Confirm PDU</returns>
        public StackPacket DecodeMcsAttachUserConfirmPDU(byte[] data)
        {
            // initialize
            int currentIndex = 0;
            Server_MCS_Attach_User_Confirm_Pdu pdu = new Server_MCS_Attach_User_Confirm_Pdu();

            // McsAttachUserConfirmPDU: TpktHeader
            pdu.tpktHeader = ParseTpktHeader(data, ref currentIndex);

            // McsAttachUserConfirmPDU: x224Data
            pdu.x224Data = ParseX224Data(data, ref currentIndex);

            // McsAttachUserConfirmPDU:
            pdu.attachUserConfirm = (AttachUserConfirm)ParseMcsDomainPdu(data, ref currentIndex).GetData();

            // Check if data length exceeded expectation
            VerifyDataLength(data.Length, currentIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// [TD Reference 3.2.5.3.9]
        /// Decode MCS Channel Join Confirm PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>decoded MCS Channel Join Confirm PDU</returns>
        public StackPacket DecodeMcsChannelJoinConfirmPDU(byte[] data)
        {
            // initialize
            int currentIndex = 0;
            Server_MCS_Channel_Join_Confirm_Pdu pdu = new Server_MCS_Channel_Join_Confirm_Pdu();

            // McsChannelJoinConfirm: TpktHeader
            pdu.tpktHeader = ParseTpktHeader(data, ref currentIndex);

            // McsChannelJoinConfirm: x224Data
            pdu.x224Data = ParseX224Data(data, ref currentIndex);

            // McsChannelJoinConfirm: channelJoinConfirm
            pdu.channelJoinConfirm = (ChannelJoinConfirm)ParseMcsDomainPdu(data, ref currentIndex).GetData();

            // Check if data length exceeded expectation
            VerifyDataLength(data.Length, currentIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// [TD Reference 3.2.5.3.12]
        /// Decode License Error PDU - Valid Client
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded License Error PDU</returns>
        public StackPacket DecodeLicenseErrorPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Server_License_Error_Pdu_Valid_Client pdu = new Server_License_Error_Pdu_Valid_Client();

            // data index
            int currentIndex = 0;

            // LicenseErrorPDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref currentIndex, type);

            // user data index
            int userDataIndex = 0;

            // LicenseErrorPDU: preamble
            pdu.preamble = ParseLicensePreamble(decryptedUserData, ref userDataIndex);

            if (pdu.preamble.bMsgType == bMsgType_Values.ERROR_ALERT)
            {
                // LicenseErrorPDU: validClientMessage
                pdu.validClientMessage = ParseLicenseErrorMessage(decryptedUserData, ref userDataIndex);

                // has received ERROR_ALERT packet, change client context status
                clientContext.IsWaitingLicenseErrorPdu = false;
            }
            else
            {
                // RDPELE Type PDU
                RdpelePdu elePdu = new RdpelePdu(clientContext);
                elePdu.commonHeader = pdu.commonHeader;
                elePdu.preamble = pdu.preamble;

                elePdu.rdpeleData = new byte[decryptedUserData.Length - userDataIndex];
                Buffer.BlockCopy(decryptedUserData, userDataIndex, elePdu.rdpeleData, 0, decryptedUserData.Length - userDataIndex);

                // If this is the last RDPELE message, change the client context status to end licensing packets processing.
                if (pdu.preamble.bMsgType == bMsgType_Values.NEW_LICENSE
                    || pdu.preamble.bMsgType == bMsgType_Values.UPGRADE_LICENSE)
                {
                    clientContext.IsWaitingLicenseErrorPdu = false;
                }

                return elePdu;
            }

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// [TD Reference 3.2.5.3.13.1]
        /// Decode Demand Active PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Demand Active PDU</returns>
        public StackPacket DecodeDemandActivePDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Server_Demand_Active_Pdu pdu = new Server_Demand_Active_Pdu();

            // data index
            int dataIndex = 0;

            // DemandActivePDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // DemandActivePDU: demandActivePduData
            pdu.demandActivePduData = ParseTsDemandActivePdu(decryptedUserData, ref userDataIndex);

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// [TD Reference 3.2.5.3.19]
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
            Server_Synchronize_Pdu pdu = new Server_Synchronize_Pdu();

            // data index
            int dataIndex = 0;

            // SynchronizePDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // SynchronizePDU: synchronizePduData
            pdu.synchronizePduData = ParseTsSynchronizePdu(decryptedUserData, ref userDataIndex);

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// [TD Reference 3.2.5.3.20]
        /// Decode Control PDU - Cooperate
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
                Server_Control_Pdu_Cooperate cooperatePdu = new Server_Control_Pdu_Cooperate();
                cooperatePdu.commonHeader = commonHeader;
                cooperatePdu.controlPduData = controlPduData;
                pdu = cooperatePdu;
            }
            else if (controlPduData.action == action_Values.CTRLACTION_GRANTED_CONTROL)
            {
                // Control PDU - granted control
                Server_Control_Pdu_Granted_Control grantedPdu = new Server_Control_Pdu_Granted_Control();
                grantedPdu.commonHeader = commonHeader;
                grantedPdu.controlPduData = controlPduData;
                pdu = grantedPdu;
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
        /// [TD Reference 3.2.5.3.22]
        /// Decode Font Map PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Font Map PDU</returns>
        public StackPacket DecodeFontMapPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Server_Font_Map_Pdu pdu = new Server_Font_Map_Pdu();

            // data index
            int dataIndex = 0;

            // FontMapPDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // FontMapPDU: fontMapPduData
            pdu.fontMapPduData = ParseTsFontMapPdu(decryptedUserData, ref userDataIndex);

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion  PDU Decoders: 10 types of PDU in connection sequence


        #region PDU Decoders: 2 types of PDU in disconnection sequence
        /// <summary>
        /// [TD Reference 3.1.5.1]
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
        /// [TD Reference 2.2.2.3]
        /// Decode Server Shutdown Request Denied PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Shutdown Request Denied PDU</returns>
        public StackPacket DecodeShutdownRequestDeniedPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Server_Shutdown_Request_Denied_Pdu pdu = new Server_Shutdown_Request_Denied_Pdu();

            // data index
            int dataIndex = 0;

            // ShutdownRequestDeniedPDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // ShutdownRequestDeniedPDU: 
            pdu.shutdownRequestDeniedPduData = ParseTsShutdownDeniedPdu(decryptedUserData, ref userDataIndex);

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion PDU Decoders: 2 types of PDU in disconnection sequence


        #region PDU Decoders: 1 type of PDU in deactivation-reactivation sequence
        /// <summary>
        /// Decode Deactivate All PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Deactivate All PDU</returns>
        public StackPacket DecodeDeactivateAllPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Server_Deactivate_All_Pdu pdu = new Server_Deactivate_All_Pdu();

            // data index
            int dataIndex = 0;

            // DeactivateAllPDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // DeactivateAllPDU: deactivateAllPduData
            pdu.deactivateAllPduData = ParseTsDeactivateAllPdu(decryptedUserData, ref userDataIndex);

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion PDU Decoders: 1 type of PDU in deactivation-reactivation sequence


        #region PDU Decoders: 1 type of PDU in auto-reconnect sequence
        /// <summary>
        /// Decode Auto-Reconnect Status PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Auto-Reconnect Status PDU</returns>
        public StackPacket DecodeAutoReconnectStatusPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Server_Auto_Reconnect_Status_Pdu pdu = new Server_Auto_Reconnect_Status_Pdu();

            // data index
            int dataIndex = 0;

            // AutoReconnectStatusPDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // AutoReconnectStatusPDU: arcStatusPduData
            pdu.arcStatusPduData = ParseTsAutoReconnectStatusPdu(decryptedUserData, ref userDataIndex);

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion PDU Decoders: 1 type of PDU in auto-reconnect sequence


        #region PDU Decoders: 2 types of PDU in server error reporting and status updates
        /// <summary>
        /// Decode Set Error Info PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Set Error Info PDU</returns>
        public StackPacket DecodeSetErrorInfoPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Server_Set_Error_Info_Pdu pdu = new Server_Set_Error_Info_Pdu();

            // data index
            int dataIndex = 0;

            // SetErrorInfoPDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // SetErrorInfoPDU: errorInfoPduData
            pdu.errorInfoPduData = ParseTsSetErrorInfoPdu(decryptedUserData, ref userDataIndex);

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// Decode Server Status Info PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Server Status Info PDU</returns>
        public StackPacket DecodeStatusInfoPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Server_Status_Info_Pdu pdu = new Server_Status_Info_Pdu();

            // data index
            int dataIndex = 0;

            // StatusInfoPDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // StatusInfoPDU: shareDataHeader
            pdu.shareDataHeader = ParseTsShareDataHeader(decryptedUserData, ref userDataIndex);

            // StatusInfoPDU: statusCode
            pdu.statusCode = (StatusCode_Values)ParseUInt32(decryptedUserData, ref userDataIndex, false);

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion PDU Decoders: 2 types of PDU in server error reporting and status updates


        #region PDU Decoders: 2 types of PDU in keyboard and mouse input
        /// <summary>
        /// Decode Set Keyboard Indicators PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Set Keyboard Indicators PDU</returns>
        public StackPacket DecodeSetKeyboardIndicatorsPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Server_Set_Keyboard_Indicators_Pdu pdu = new Server_Set_Keyboard_Indicators_Pdu();

            // data index
            int dataIndex = 0;

            // SetKeyboardIndicatorsPDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // SetKeyboardIndicatorsPDU: setKeyBdIndicatorsPduData
            pdu.setKeyBdIndicatorsPduData = ParseTsSetKeyboardIndicatorsPdu(decryptedUserData, ref userDataIndex);

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// Decode Set Keyboard IME Status PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Set Keyboard IME Status PDU</returns>
        public StackPacket DecodeSetKeyboardImeStatusPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Server_Set_Keyboard_IME_Status_Pdu pdu = new Server_Set_Keyboard_IME_Status_Pdu();

            // data index
            int dataIndex = 0;

            // SetKeyboardImeStatusPDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // SetKeyboardImeStatusPDU: setKeyBdImeStatusPduData
            pdu.setKeyBdImeStatusPduData = ParseTsSetKeyboardImeStatusPdu(decryptedUserData, ref userDataIndex);

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion PDU Decoders:  2 types of PDU in keyboard and mouse input


        #region PDU Decoders: 4 types of PDU in basic output
        /// <summary>
        /// Decode Slow-Path Update PDU including Slow-Path Graphics Update PDU and Slow-Path Pointer Update PDU.
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Slow-Path Update PDU</returns>
        public StackPacket DecodeSlowPathUpdatePDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            SlowPathOutputPdu pdu = new SlowPathOutputPdu();

            // data index
            int dataIndex = 0;

            // SlowPathOutputPDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // SlowPathOutputPDU: slowPathUpdates
            pdu.slowPathUpdates = ParseSlowPathUpdates(decryptedUserData, ref userDataIndex);

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// Decode Fast-path Update PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <returns>decoded Fast-path Update PDU</returns>
        public StackPacket DecodeTsFpUpdatePDU(byte[] data)
        {
            // initialize
            int currentIndex = 0;
            TS_FP_UPDATE_PDU pdu = new TS_FP_UPDATE_PDU();

            // TS_FP_UPDATE_PDU: fpOutputHeader
            pdu.fpOutputHeader = ParseByte(data, ref currentIndex);

            // Get infomation from fpOutputHeader
            nested_TS_FP_UPDATE_PDU_fpOutputHeader_actionCode_Values actionCode;
            encryptionFlagsChgd_Values encryptionFlags;
            GetFpOutputHeaderInfo(pdu.fpOutputHeader, out actionCode, out encryptionFlags);

            // TS_FP_UPDATE_PDU: length1
            pdu.length1 = ParseByte(data, ref currentIndex);

            // TS_FP_UPDATE_PDU: length2 (optional)
            if ((ConstValue.MOST_SIGNIFICANT_BIT_FILTER & pdu.length1) != pdu.length1)
            {
                // length2 is present (since the most significant bit of length1 is set)
                pdu.length2 = ParseByte(data, ref currentIndex);
            }

            // TS_FP_UPDATE_PDU: fipsInformation
            if (EncryptionLevel.ENCRYPTION_LEVEL_FIPS == clientContext.RdpEncryptionLevel)
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
            byte[] decryptedData = DecryptFastPathUpdateData(remainData, pdu.dataSignature, isSalted);

            // Decrypted data index
            int decryptedDataIndex = 0;

            // TS_FP_UPDATE_PDU: fpOutputUpdates
            pdu.fpOutputUpdates = ParseTsFpUpdates(decryptedData, ref decryptedDataIndex);

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedData.Length, decryptedDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// Decode Play Sound PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Play Sound PDU</returns>
        public StackPacket DecodePlaySoundPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Server_Play_Sound_Pdu pdu = new Server_Play_Sound_Pdu();

            // data index
            int dataIndex = 0;

            // PlaySoundPDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // PlaySoundPDU: 
            pdu.playSoundPduData = ParseTsPlaySoundPduData(decryptedUserData, ref userDataIndex);

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion PDU Decoders: 4 types of PDU in basic output


        #region PDU Decoders: 1 type of PDU in login notifications
        /// <summary>
        /// Decode Save Session Info PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Save Session Info PDU</returns>
        public StackPacket DecodeSaveSessionInfoPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Server_Save_Session_Info_Pdu pdu = new Server_Save_Session_Info_Pdu();

            // data index
            int dataIndex = 0;

            // SaveSessionInfoPDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // SaveSessionInfoPDU: saveSessionInfoPduData
            pdu.saveSessionInfoPduData = ParseTsSaveSessionInfoPduData(decryptedUserData, ref userDataIndex);

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion PDU Decoders: 1 type of PDU in login notifications


        #region PDU Decoders: 1 type of PDU in display update notifications
        /// <summary>
        /// Decode Monitor Layout PDU
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Monitor Layout PDU</returns>
        public StackPacket DecodeMonitorLayoutPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            TS_MONITOR_LAYOUT_PDU pdu = new TS_MONITOR_LAYOUT_PDU();

            // data index
            int dataIndex = 0;

            // TS_MONITOR_LAYOUT_PDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // TS_MONITOR_LAYOUT_PDU: shareDataHeader
            pdu.shareDataHeader = ParseTsShareDataHeader(decryptedUserData, ref userDataIndex);

            // TS_MONITOR_LAYOUT_PDU: monitorCount
            pdu.monitorCount = ParseUInt32(decryptedUserData, ref userDataIndex, false);

            // TS_MONITOR_LAYOUT_PDU: monitorDefArray
            pdu.monitorDefArray = new TS_MONITOR_DEF[pdu.monitorCount];
            for (int i = 0; i < pdu.monitorDefArray.Length; i++)
            {
                pdu.monitorDefArray[i] = ParseTsMonitorDef(decryptedUserData, ref userDataIndex);
            }

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion PDU Decoders: 1 type of PDU in display update notifications


        #region PDU Decoders: 1 type of PDU in server redirection
        /// <summary>
        /// Decode Standard Security Server Redirection PDU.
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Server Redirection PDU</returns>
        public StackPacket DecodeServerRedirectionPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Server_Redirection_Pdu pdu = new Server_Redirection_Pdu();

            // data index
            int dataIndex = 0;

            UpdateSecurityHeaderType(ref type);

            // Server_Redirection_Pdu: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            // Server_Redirection_PDU: serverRedirectionPdu
            pdu.serverRedirectionPdu = ParseRdpServerRedirectionPacket(decryptedUserData, ref userDataIndex);

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }


        /// <summary>
        /// Decode Enhanced Security Server Redirection PDU.
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data to be parsed</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Server Redirection PDU</returns>
        public StackPacket DecodeEnhancedServerRedirectionPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Enhanced_Security_Server_Redirection_Pdu pdu = new Enhanced_Security_Server_Redirection_Pdu();

            // data index
            int dataIndex = 0;

            // Server_Redirection_Pdu: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);

            // user data index
            int userDataIndex = 0;

            pdu.shareControlHeader = ParseTsShareControlHeader(decryptedUserData, ref userDataIndex);

            pdu.pad = ParseUInt16(decryptedUserData, ref userDataIndex, false);

            // Server_Redirection_PDU: serverRedirectionPdu
            pdu.serverRedirectionPdu = ParseRdpServerRedirectionPacket(decryptedUserData, ref userDataIndex);

            // check if there's paddings
            int remainBytes = decryptedUserData.Length - userDataIndex;
            if (remainBytes > 0)
            {
                pdu.pad1Octet = GetBytes(decryptedUserData, ref userDataIndex, remainBytes);
            }

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion PDU Decoders: 1 type of PDU in server redirection


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
            Virtual_Channel_RAW_Server_Pdu pdu = new Virtual_Channel_RAW_Server_Pdu();

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

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion PDU Decoders: 1 type of PDU in virtual channels

        #region PDU Decoder : 1 type of PDU in Multitransport Bootstrapping
        /// <summary>
        /// Decode Server Initiate multitransport request
        /// </summary>
        /// <param name="data">data to be parsed</param>
        /// <param name="decryptedUserData">decrypted user data</param>
        /// <param name="type">security header type</param>
        /// <returns>decoded Server Initiate multitransport request</returns>
        public StackPacket DecodeServerInitiateMultitransportRequest(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Server_Initiate_Multitransport_Request_PDU pdu = new Server_Initiate_Multitransport_Request_PDU();

            // data index
            int dataIndex = 0;

            // Virtual_Channel_RAW_Pdu: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);


            // user data index
            int userDataIndex = 0;

            if (type == SecurityHeaderType.None)
            {
                pdu.commonHeader.securityHeader = ParseTsSecurityHeaderBasic(decryptedUserData, ref userDataIndex);
            }

            // requestId
            pdu.requestId = ParseUInt32(decryptedUserData, ref userDataIndex, false);

            // request protocol
            pdu.requestedProtocol = (Multitransport_Protocol_value)ParseUInt16(decryptedUserData, ref userDataIndex, false);

            // reserved
            pdu.reserved = ParseUInt16(decryptedUserData, ref userDataIndex, false);

            // security cookie
            pdu.securityCookie = GetBytes(decryptedUserData, ref userDataIndex, 16);

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }
        #endregion PDU Decoder : 1 type of PDU in Multitransport Bootstrapping

        #region PDU Decorder: 1 type of PDU in Connection Health Monitoring

        /// <summary>
        /// Decode Server Heartbeat PDU
        /// </summary>
        /// <param name="data"></param>
        /// <param name="decryptedUserData"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public StackPacket DecodeServerHeartbeatPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Server_Heartbeat_PDU pdu = new Server_Heartbeat_PDU();

            // data index
            int dataIndex = 0;

            // Server_Heartbeat_PDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);


            // user data index
            int userDataIndex = 0;

            if (type == SecurityHeaderType.None)
            {
                pdu.commonHeader.securityHeader = ParseTsSecurityHeaderBasic(decryptedUserData, ref userDataIndex);
            }

            // reserved
            pdu.reserved = ParseByte(decryptedUserData, ref userDataIndex);

            // reserved
            pdu.period = ParseByte(decryptedUserData, ref userDataIndex);

            // reserved
            pdu.count1 = ParseByte(decryptedUserData, ref userDataIndex);

            // reserved
            pdu.count2 = ParseByte(decryptedUserData, ref userDataIndex);

            // Check if data length exceeded expectation
            VerifyDataLength(decryptedUserData.Length, userDataIndex, ConstValue.ERROR_MESSAGE_DATA_LENGTH_EXCEEDED);
            return pdu;
        }

        #endregion PDU Decorder: 1 type of PDU in Connection Health Monitoring

        #region PDU Decorder : 1 type of PDU in Auto-detect

        public StackPacket DecodeServerAutoDetectRequestPDU(
            byte[] data,
            byte[] decryptedUserData,
            SecurityHeaderType type)
        {
            Server_Auto_Detect_Request_PDU pdu = new Server_Auto_Detect_Request_PDU();

            // data index
            int dataIndex = 0;

            // Server_Heartbeat_PDU: commonHeader
            pdu.commonHeader = ParseMcsCommonHeader(data, ref dataIndex, type);


            // user data index
            int userDataIndex = 0;

            if (type == SecurityHeaderType.None)
            {
                pdu.commonHeader.securityHeader = ParseTsSecurityHeaderBasic(decryptedUserData, ref userDataIndex);
            }

            pdu.autoDetectReqData = ParseNetworkDectectRequest(decryptedUserData, ref userDataIndex);

            return pdu;
        }


        #endregion PDU Decorder : 1 type of PDU in Auto-detect

        #region RDSTLS
        public StackPacket SwitchRDSTLSAuthenticationPDU(byte[] data)
        {
            StackPacket pdu = null;

            int currentIndex = 0;

            var header = ParseRDSTLSCommonHeader(data, ref currentIndex);

            if (header.Version != RDSTLS_VersionEnum.RDSTLS_VERSION_1)
            {
                return null;
            }

            switch (header.PduType)
            {
                case RDSTLS_PduTypeEnum.RDSTLS_TYPE_CAPABILITIES:
                    if (header.DataType == RDSTLS_DataTypeEnum.RDSTLS_DATA_CAPABILITIES)
                    {
                        pdu = DecodeRDSTLSCapabilitiesPDU(data);
                    }
                    else
                    {
                        pdu = null;
                    }
                    break;

                case RDSTLS_PduTypeEnum.RDSTLS_TYPE_AUTHRSP:
                    if (header.DataType == RDSTLS_DataTypeEnum.RDSTLS_DATA_RESULT_CODE)
                    {
                        pdu = DecodeRDSTLSAuthenticationResponsePDU(data);
                    }
                    else
                    {
                        pdu = null;
                    }
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

        public RDSTLS_CapabilitiesPDU DecodeRDSTLSCapabilitiesPDU(byte[] data)
        {
            var pdu = new RDSTLS_CapabilitiesPDU();

            int currentIndex = 0;

            pdu.Header = ParseRDSTLSCommonHeader(data, ref currentIndex);

            if (pdu.Header.DataType != RDSTLS_DataTypeEnum.RDSTLS_DATA_CAPABILITIES)
            {
                return null;
            }

            // parse supported versions
            pdu.SupportedVersions = (RDSTLS_VersionEnum)ParseUInt16(data, ref currentIndex, false);

            // check total length
            if (currentIndex != data.Length)
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            }

            return pdu;
        }

        public RDSTLS_AuthenticationResponsePDU DecodeRDSTLSAuthenticationResponsePDU(byte[] data)
        {
            var pdu = new RDSTLS_AuthenticationResponsePDU();

            int currentIndex = 0;

            pdu.Header = ParseRDSTLSCommonHeader(data, ref currentIndex);

            if (pdu.Header.DataType != RDSTLS_DataTypeEnum.RDSTLS_DATA_CAPABILITIES)
            {
                return null;
            }

            // parse result code
            pdu.ResultCode = (RDSTLS_ResultCodeEnum)ParseUInt32(data, ref currentIndex, false);

            // check total length
            if (currentIndex != data.Length)
            {
                throw new FormatException(ConstValue.ERROR_MESSAGE_DATA_LENGTH_INCONSISTENT);
            }

            return pdu;
        }

        #endregion

        #endregion Public Methods: PDU Decoders
    }
}
