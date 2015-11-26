// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Pac
{
    /// <summary>
    ///  Two PAC_SIGNATURE_DATA structures are appended
    ///  to the PAC that stores the server and KDC signatures.
    ///   These structures are placed after the Buffers array
    ///  of the topmost PACTYPE structure, at the offsets specified
    ///  in the Offset fields in each of the corresponding PAC_INFO_BUFFER
    ///  structures in the Buffers array. The  ulType field
    ///  of the PAC_INFO_BUFFER corresponding to the server
    ///  signature contains the value 0x00000006 and the ulType
    ///  field of the PAC_INFO_BUFFER corresponding to the KDC
    ///  signature contains the value 0x00000007.  PAC signatures
    ///  can be generated only when the PAC is used by the Kerberos
    ///  protocol because the keys used to create and verify
    ///  the signatures are the keys known to the KDC.  No other
    ///  protocol can use these PAC signatures.
    /// </summary>
    public abstract class PacSignatureData : PacInfoBuffer
    {
        /// <summary>
        /// TD section 2.8.1 Server Signature:
        /// The Key Usage Value MUST be KERB_NON_KERB_CKSUM_SALT
        /// (17) [MS-KILE] (section 3.1.5.9).
        /// </summary>
        private const int KerbNonKerbCksumSalt = 17;

        /// <summary>
        /// TD section 2.8.1 Server Signature:
        /// KERB_CHECKSUM_HMAC_MD5 0xFFFFFF76
        /// Signature size is 16 bytes.
        /// </summary>
        private const int KerbChecksumHmacMd5Size = 16;

        /// <summary>
        /// TD section 2.8.1 Server Signature:
        /// HMAC_SHA1_96_AES128 0x0000000F
        /// HMAC_SHA1_96_AES256 0x00000010
        /// Signature size is 12 bytes.
        /// </summary>
        private const int KerbChecksumHmacSha1Size = 12;

        /// <summary>
        /// The native PAC_SIGNATURE_DATA object.
        /// </summary>
        public PAC_SIGNATURE_DATA NativePacSignatureData;

        
        /// <summary>
        /// Decode specified buffer from specified index, with specified count
        /// of bytes, into the instance of current class.
        /// </summary>
        /// <param name="buffer">The specified buffer.</param>
        /// <param name="index">The specified index from beginning of buffer.</param>
        /// <param name="count">The specified count of bytes to be decoded.</param>
        internal override void DecodeBuffer(byte[] buffer, int index, int count)
        {
            NativePacSignatureData =
                PacUtility.MemoryToObject<PAC_SIGNATURE_DATA>(buffer, index, count);
        }


        /// <summary>
        /// Encode the instance of current class into byte array,
        /// according to TD specification.
        /// </summary>
        /// <returns>The encoded byte array</returns>
        internal override byte[] EncodeBuffer()
        {
            return PacUtility.ObjectToMemory(NativePacSignatureData);
        }


        /// <summary>
        /// Calculate checksum length according to specified checksum type.
        /// </summary>
        /// <param name="signatureType">The specified checksum type.</param>
        /// <returns>The calculated checksum length.</returns>
        internal static int CalculateSignatureLength(PAC_SIGNATURE_DATA_SignatureType_Values signatureType)
        {
            switch (signatureType)
            {
                case PAC_SIGNATURE_DATA_SignatureType_Values.KERB_CHECKSUM_HMAC_MD5:
                    return KerbChecksumHmacMd5Size;
                case PAC_SIGNATURE_DATA_SignatureType_Values.HMAC_SHA1_96_AES128:
                    return KerbChecksumHmacSha1Size;
                case PAC_SIGNATURE_DATA_SignatureType_Values.HMAC_SHA1_96_AES256:
                    return KerbChecksumHmacSha1Size;
                default:
                    throw new ArgumentOutOfRangeException("signatureType");
            }
        }


        /// <summary>
        /// Sign input plaintext bytes into checksum bytes.
        /// </summary>
        /// <param name="input">The specified plaintext bytes.</param>
        /// <param name="type">The specified checksum algorithm.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>The signed checksum bytes.</returns>
        internal static byte[] Sign(byte[] input, PAC_SIGNATURE_DATA_SignatureType_Values type, byte[] key)
        {
            switch (type)
            {
                case PAC_SIGNATURE_DATA_SignatureType_Values.HMAC_SHA1_96_AES128:
                    return HmacSha1AesChecksum.GetMic(
                        key,
                        input,
                        PacSignatureData.KerbNonKerbCksumSalt,
                        AesKeyType.Aes128BitsKey);

                case PAC_SIGNATURE_DATA_SignatureType_Values.HMAC_SHA1_96_AES256:
                    return HmacSha1AesChecksum.GetMic(
                        key,
                        input,
                        PacSignatureData.KerbNonKerbCksumSalt,
                        AesKeyType.Aes256BitsKey);

                case PAC_SIGNATURE_DATA_SignatureType_Values.KERB_CHECKSUM_HMAC_MD5:
                    return HmacMd5StringChecksum.GetMic(
                        key,
                        input,
                        PacSignatureData.KerbNonKerbCksumSalt);

                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }


        /// <summary>
        /// Calculate size of current instance's encoded buffer, in bytes.
        /// </summary>
        /// <returns>The size of current instance's encoded buffer, in bytes.</returns>
        internal override int CalculateSize()
        {
            PAC_SIGNATURE_DATA_SignatureType_Values type = NativePacSignatureData.SignatureType;
            // The structure contains following part:
            // SignatureType (4 bytes)
            // Signature (variable, calculated by SignatureType)
            return sizeof(uint) + CalculateSignatureLength(type);
        }
    }


    /// <summary>
    /// Class for Server checksum (TD section 2.8).
    /// </summary>
    public class PacServerSignature : PacSignatureData
    {
        /// <summary>
        /// Get the ulType of current instance's PAC_INFO_BUFFER.
        /// </summary>
        /// <returns>The ulType of current instance's PAC_INFO_BUFFER.</returns>
        internal override PAC_INFO_BUFFER_Type_Values GetBufferInfoType()
        {
            return PAC_INFO_BUFFER_Type_Values.ServerChecksum;
        }
    }


    /// <summary>
    /// KDC (privilege server) checksum (TD section 2.8).
    /// </summary>
    public class PacKdcSignature : PacSignatureData
    {
        /// <summary>
        /// Get the ulType of current instance's PAC_INFO_BUFFER.
        /// </summary>
        /// <returns>The ulType of current instance's PAC_INFO_BUFFER.</returns>
        internal override PAC_INFO_BUFFER_Type_Values GetBufferInfoType()
        {
            return PAC_INFO_BUFFER_Type_Values.KdcChecksum;
        }
    }
}
