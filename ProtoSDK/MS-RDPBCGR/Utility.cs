// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.ExtendedLogging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    /// <summary>
    /// Provide some utilities.
    /// </summary>
    public static class RdpbcgrUtility
    {
        /// <summary>
        /// Generate random data.
        /// </summary>
        /// <param name="length">The count of the data.</param>
        /// <returns>The random data.</returns>
        public static byte[] GenerateRandom(int length)
        {
            byte[] clientRandom = new byte[length];
            // a random seed
            Random random = new Random((int)System.DateTime.Now.Ticks);
            random.NextBytes(clientRandom);
            return clientRandom;
        }

        // Dump Level layer definition for ETW Provider
        public const DumpLevel DumpLevel_Layer0 = (DumpLevel)0;
        public const DumpLevel DumpLevel_Layer1 = (DumpLevel)1;
        public const DumpLevel DumpLevel_Layer2 = (DumpLevel)2;
        public const DumpLevel DumpLevel_Layer3 = (DumpLevel)3;
        public const DumpLevel DumpLevel_LayerTLS = (DumpLevel)10;

        /// <summary>
        /// ETW provider dump messages
        /// </summary>
        /// <param name="TypeName">Message Name</param>
        /// <param name="EncodeBytes">Encrypted or original bytes</param>
        /// <param name="isRDPStandardSecurity">Whether it is RDP Standard Security</param>
        /// <param name="decryptedBytes">Decrypted part of message</param>
        public static void ETWProviderDump(String TypeName, byte[] EncodeBytes, bool isRDPStandardSecurity = false, byte[] decryptedBytes = null)
        {
            // ETW Provider Dump Code
            string messageName;
            if (ConstValue.SLOW_PATH_PDU_INDICATOR_VALUE == EncodeBytes[ConstValue.SLOW_PATH_PDU_INDICATOR_INDEX])
            {
                // Slow-Path
                messageName = "RDPBCGR:SentSlowPathPDU";
            }
            else
            {
                // Fast-Path
                messageName = "RDPBCGR:SentFastPathPDU";
            }
            ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer0, TypeName, EncodeBytes);
            // Dump decrypted structure
            if (isRDPStandardSecurity)
            {
                // RDP Standard Security
                messageName = "RDPBCGR:" + TypeName;
                ExtendedLogger.DumpMessage(messageName, RdpbcgrUtility.DumpLevel_Layer3, TypeName, decryptedBytes);
            }
        }



        /// <summary>
        /// Generate encrypted random according to section 5.3.4.1 Encrypting Client Random.
        /// </summary>
        /// <param name="randomData">Random data to be encrypted. This argument can be null.</param>
        /// <param name="exponent">Exponent of RSA. This argument can be null.</param>
        /// <param name="modulus">Modulus of RSA. This argument can be null.</param>
        /// <returns>The encrypted random data.</returns>
        public static byte[] GenerateEncryptedRandom(byte[] randomData, byte[] exponent, byte[] modulus)
        {
            if (randomData == null || exponent == null || modulus == null)
            {
                return null;
            }

            byte[] encryptedData = RSAEncrypt(randomData, exponent, modulus);

            // The result may contain extra zero in the end (the extra zero in the end indicate it is a positive number),
            // the actual length should subtract the zero length.
            // So divide the length by 8 and multiple the length by 8 again.
            // [MS-RDPBCGR] 5.3.4.1 The resultant encrypted client random is copied into a zeroed-out buffer, which is of size: (bitlen / 8) + 8
            // So add 8 bytes padding in the end.
            byte[] result = new byte[encryptedData.Length / 8 * 8 + 8];

            Array.Copy(encryptedData, result, encryptedData.Length);
            return result;
        }


        /// <summary>
        /// Decrypt client encrypted random according to section 5.3.4.2 Decrypting Client Random.
        /// </summary>
        /// <param name="randomData">Encrypted random data to be decrypted. This argument can be null.</param>
        /// <param name="exponent">Exponent of RSA. This argument can be null.</param>
        /// <param name="modulus">Modulus of RSA. This argument can be null.</param>
        /// <returns>The encrypted random data.</returns>
        public static byte[] DecryptClientRandom(byte[] randomData, byte[] exponent, byte[] modulus)
        {
            if (randomData == null)
            {
                throw new ArgumentNullException("randomData");
            }

            if (exponent == null)
            {
                throw new ArgumentNullException("exponent");
            }

            if (modulus == null)
            {
                throw new ArgumentNullException("modulus");
            }

            byte[] encryptedRandom = new byte[randomData.Length - 8];

            if (randomData.Length < 8)
            {
                throw new ArgumentOutOfRangeException("randomData", "The length of randomData is less than 8 bytes!");
            }

            Array.Copy(randomData, encryptedRandom, randomData.Length - 8);
            byte[] result = RSAEncrypt(encryptedRandom, exponent, modulus);
            //int copyLength = (result.Length > encryptedResult.Length) ? encryptedResult.Length : result.Length;
            byte[] encryptedResult = new byte[result.Length];
            Array.Copy(result, encryptedResult, result.Length);
            return encryptedResult;
        }


        /// <summary>
        /// Produce the signature for certificate and encrypt it with private exponent.
        /// </summary>
        /// <param name="certificate">The certificate to be assigned signature.</param>
        /// <param name="privateExp">The private exponent used to encrypt the signature.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5350:MD5CannotBeUsed")]
        public static byte[] SignProprietaryCertificate(PROPRIETARYSERVERCERTIFICATE certificate, byte[] privateExp)
        {
            if (privateExp == null)
            {
                throw new ArgumentNullException("privateExp");
            }

            // compute hash
            // PublicKeyBlob = wBlobType + wBlobLen + PublicKeyBytes
            // hash = MD5(dwVersion + dwSigAlgID + dwKeyAlgID + PublicKeyBlob)
            SERVER_CERTIFICATE_dwVersion_Values dwVersion = SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_1;

            byte[] publicKeyBytes = ConcatenateArrays(BitConverter.GetBytes((uint)certificate.PublicKeyBlob.magic),
                                                      BitConverter.GetBytes(certificate.PublicKeyBlob.keylen),
                                                      BitConverter.GetBytes(certificate.PublicKeyBlob.bitlen),
                                                      BitConverter.GetBytes(certificate.PublicKeyBlob.datalen),
                                                      BitConverter.GetBytes(certificate.PublicKeyBlob.pubExp),
                                                      certificate.PublicKeyBlob.modulus);

            byte[] publicKeyBlob = ConcatenateArrays(BitConverter.GetBytes((ushort)certificate.wPublicKeyBlobType),
                                                     BitConverter.GetBytes((ushort)certificate.wPublicKeyBlobLen),
                                                     publicKeyBytes);

            byte[] certBlob = ConcatenateArrays(BitConverter.GetBytes((uint)dwVersion),
                                                BitConverter.GetBytes((uint)certificate.dwSigAlgId),
                                                BitConverter.GetBytes((uint)certificate.dwKeyAlgId),
                                                publicKeyBlob);

            // Suppress "CA5350:MD5CannotBeUsed" message since MD5 is used according protocol definition of MS-RDPBCGR 
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(certBlob);

            // combine signature
            byte[] signature = new byte[ConstValue.PROPRIETARY_CERTIFICATE_SIGNATURE_SIZE];
            Array.Copy(hash, signature, hash.Length);
            // The 17th byte of the array should be 0x00, 
            // the 18th through the 62nd byte should be 0xFF, 
            // while the 63rd byte should be 0x01.
            signature[16] = 0;
            signature[62] = 0x01;
            for (int i = 17; i < 62; ++i)
            {
                signature[i] = 0xff;
            }

            // encrypt received signature            
            byte[] encryptSig = RSAEncrypt(signature,
                                           privateExp,
                                           certificate.PublicKeyBlob.modulus);
            byte[] result = new byte[encryptSig.Length];
            result = RdpbcgrUtility.CloneByteArray(encryptSig);
            return result;
        }

        /// <summary>
        /// Produce the signature for certificate and encrypt it with private exponent.
        /// </summary>
        /// <param name="certificate">The certificate to be assigned signature.</param>
        /// <returns>The signature of the certificate</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5350:MD5CannotBeUsed")]
        public static byte[] SignProprietaryCertificate(PROPRIETARYSERVERCERTIFICATE certificate)
        {
            byte[] privateExp = {
                                    0x87, 0xa7, 0x19, 0x32, 0xda, 0x11, 0x87, 0x55,
                                    0x58, 0x00, 0x16, 0x16, 0x25, 0x65, 0x68, 0xf8,
                                    0x24, 0x3e, 0xe6, 0xfa, 0xe9, 0x67, 0x49, 0x94,
                                    0xcf, 0x92, 0xcc, 0x33, 0x99, 0xe8, 0x08, 0x60,
                                    0x17, 0x9a, 0x12, 0x9f, 0x24, 0xdd, 0xb1, 0x24,
                                    0x99, 0xc7, 0x3a, 0xb8, 0x0a, 0x7b, 0x0d, 0xdd,
                                    0x35, 0x07, 0x79, 0x17, 0x0b, 0x51, 0x9b, 0xb3,
                                    0xc7, 0x10, 0x01, 0x13, 0xe7, 0x3f, 0xf3, 0x5f
                                };
            byte[] modulus = {
                                 0x3d, 0x3a, 0x5e, 0xbd, 0x72, 0x43, 0x3e, 0xc9,
                                 0x4d, 0xbb, 0xc1, 0x1e, 0x4a, 0xba, 0x5f, 0xcb,
                                 0x3e, 0x88, 0x20, 0x87, 0xef, 0xf5, 0xc1, 0xe2,
                                 0xd7, 0xb7, 0x6b, 0x9a, 0xf2, 0x52, 0x45, 0x95,
                                 0xce, 0x63, 0x65, 0x6b, 0x58, 0x3a, 0xfe, 0xef,
                                 0x7c, 0xe7, 0xbf, 0xfe, 0x3d, 0xf6, 0x5c, 0x7d,
                                 0x6c, 0x5e, 0x06, 0x09, 0x1a, 0xf5, 0x61, 0xbb,
                                 0x20, 0x93, 0x09, 0x5f, 0x05, 0x6d, 0xea, 0x87
                             };
            if (privateExp == null)
            {
                throw new ArgumentNullException("privateExp");
            }

            // compute hash
            // PublicKeyBlob = wBlobType + wBlobLen + PublicKeyBytes
            // hash = MD5(dwVersion + dwSigAlgID + dwKeyAlgID + PublicKeyBlob)
            SERVER_CERTIFICATE_dwVersion_Values dwVersion = SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_1;

            byte[] publicKeyBytes = ConcatenateArrays(BitConverter.GetBytes((uint)certificate.PublicKeyBlob.magic),
                                                      BitConverter.GetBytes(certificate.PublicKeyBlob.keylen),
                                                      BitConverter.GetBytes(certificate.PublicKeyBlob.bitlen),
                                                      BitConverter.GetBytes(certificate.PublicKeyBlob.datalen),
                                                      BitConverter.GetBytes(certificate.PublicKeyBlob.pubExp),
                                                      certificate.PublicKeyBlob.modulus);

            byte[] publicKeyBlob = ConcatenateArrays(BitConverter.GetBytes((ushort)certificate.wPublicKeyBlobType),
                                                     BitConverter.GetBytes((ushort)certificate.wPublicKeyBlobLen),
                                                     publicKeyBytes);

            byte[] certBlob = ConcatenateArrays(BitConverter.GetBytes((uint)dwVersion),
                                                BitConverter.GetBytes((uint)certificate.dwSigAlgId),
                                                BitConverter.GetBytes((uint)certificate.dwKeyAlgId),
                                                publicKeyBlob);

            // Suppress "CA5350:MD5CannotBeUsed" message since MD5 is used according protocol definition of MS-RDPBCGR 
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(certBlob);

            // combine signature
            byte[] signature = new byte[ConstValue.PROPRIETARY_CERTIFICATE_SIGNATURE_SIZE];
            Array.Copy(hash, signature, hash.Length);
            // The 17th byte of the array should be 0x00, 
            // the 18th through the 62nd byte should be 0xFF, 
            // while the 63rd byte should be 0x01.
            signature[16] = 0;
            signature[62] = 0x01;
            for (int i = 17; i < 62; ++i)
            {
                signature[i] = 0xff;
            }

            // encrypt received signature            
            byte[] encryptSig = RSAEncrypt(signature,
                                           privateExp,
                                           modulus);
            byte[] result = new byte[encryptSig.Length];
            result = RdpbcgrUtility.CloneByteArray(encryptSig);
            return result;
        }

        /// <summary>
        /// Validate the Proprietary Certificate according to section 5.3.3.1.3 Validating a Proprietary Certificate.
        /// </summary>
        /// <param name="certificate">Got from Server MCS Connect Response PDU with GCC Conference Create Response.
        /// </param>
        /// <returns>Whether the certificate is valid.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5350:MD5CannotBeUsed")]
        public static bool ValidateProprietaryCertificate(PROPRIETARYSERVERCERTIFICATE certificate)
        {
            // compute hash
            // PublicKeyBlob = wBlobType + wBlobLen + PublicKeyBytes
            // hash = MD5(dwVersion + dwSigAlgID + dwKeyAlgID + PublicKeyBlob)
            SERVER_CERTIFICATE_dwVersion_Values dwVersion = SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_1;

            byte[] publicKeyBytes = ConcatenateArrays(BitConverter.GetBytes((uint)certificate.PublicKeyBlob.magic),
                                                      BitConverter.GetBytes(certificate.PublicKeyBlob.keylen),
                                                      BitConverter.GetBytes(certificate.PublicKeyBlob.bitlen),
                                                      BitConverter.GetBytes(certificate.PublicKeyBlob.datalen),
                                                      BitConverter.GetBytes(certificate.PublicKeyBlob.pubExp),
                                                      certificate.PublicKeyBlob.modulus);

            byte[] publicKeyBlob = ConcatenateArrays(BitConverter.GetBytes((ushort)certificate.wPublicKeyBlobType),
                                                     BitConverter.GetBytes((ushort)certificate.wPublicKeyBlobLen),
                                                     publicKeyBytes);

            byte[] certBlob = ConcatenateArrays(BitConverter.GetBytes((uint)dwVersion),
                                                BitConverter.GetBytes((uint)certificate.dwSigAlgId),
                                                BitConverter.GetBytes((uint)certificate.dwKeyAlgId),
                                                publicKeyBlob);

            // Suppress "CA5350:MD5CannotBeUsed" message since MD5 is used according protocol definition of MS-RDPBCGR 
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(certBlob);

            // combine signature
            byte[] signature = new byte[ConstValue.PROPRIETARY_CERTIFICATE_SIGNATURE_SIZE];
            Array.Copy(hash, signature, hash.Length);
            // The 17th byte of the array should be 0x00, 
            // the 18th through the 62nd byte should be 0xFF, 
            // while the 63rd byte should be 0x01.
            signature[16] = 0;
            signature[62] = 0x01;
            for (int i = 17; i < 62; ++i)
            {
                signature[i] = 0xff;
            }

            // decrypt received signature            
            byte[] decryptSig = RSAEncrypt(certificate.SignatureBlob,
                                           ConstValue.PROPRIETARY_CERTIFICATE_EXPONENT,
                                           ConstValue.PROPRIETARY_CERTIFICATE_MODULUS);

            // compare the two signature
            return AreEqual(signature, decryptSig);
        }

        public static bool IsEven(long i)
        {
            return (i & (long)1) == 0 ? true : false;
        }


        /// <summary>
        /// If the two byte arrays are equal.
        /// </summary>
        /// <param name="array1">Specify the first buffer.</param>
        /// <param name="array2">Specify the second buffer.</param>
        /// <returns>If the two byte arrays are equal.</returns>
        public static bool AreEqual(byte[] array1, byte[] array2)
        {
            if (array1 == null || array2 == null)
            {
                if (array1 == null && array2 == null)
                {
                    return true;
                }

                return false;
            }

            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; ++i)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// This method is used to reverse the bits of a byte.
        /// </summary>
        /// <param name="aByte">Specify the byte to be reversed</param>
        /// <returns>The reversed byte.</returns>
        internal static byte ReverseByte(byte aByte)
        {
            byte mask1 = 0x80;
            byte mask2 = 0x01;
            byte result = 0;
            for (int i = 8; i > 0; i -= 2)
            {
                result |= (byte)((aByte & mask1) >> (i - 1));
                result |= (byte)((aByte & mask2) << (i - 1));
                mask1 >>= 1;
                mask2 <<= 1;
            }

            return result;
        }


        /// <summary>
        /// Method to covert struct to byte[]
        /// </summary>
        /// <param name="structp">The struct prepare to covert</param>
        /// <returns>the got byte array converted from struct</returns>
        internal static byte[] StructToBytes(object structp)
        {
            IntPtr ptr = IntPtr.Zero;
            byte[] buffer = null;

            try
            {
                int size = Marshal.SizeOf(structp.GetType());
                ptr = Marshal.AllocHGlobal(size);
                buffer = new byte[size];
                Marshal.StructureToPtr(structp, ptr, false);
                Marshal.Copy(ptr, buffer, 0, size);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }

            return buffer;
        }


        /// <summary>
        /// Method to covert byte[] to struct.
        /// </summary>
        /// <param name="buffer">The buffer to be converted.</param>
        /// <returns>The structure.</returns>
        internal static T ToStruct<T>(byte[] buffer)
        {
            T ret;
            IntPtr intPtr = IntPtr.Zero;

            try
            {
                intPtr = Marshal.AllocHGlobal(buffer.Length);
                Marshal.Copy(buffer, 0, intPtr, buffer.Length);
                ret = (T)Marshal.PtrToStructure(intPtr, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(intPtr);
            }

            return ret;
        }


        /// <summary>
        /// This method is used to fill X224 structure.
        /// </summary>
        /// <param name="x224Data">Data to be filled.</param>
        internal static void FillX224Data(ref X224 x224Data)
        {
            x224Data.type = ConstValue.X224_DATA_TYPE;
            x224Data.eot = ConstValue.X224_DATA_TYPE_EOT;
            x224Data.length = ConstValue.X224_DATA_TYPE_LENGTH;
        }


        /// <summary>
        /// This method is used to fill TpktHeader structure.
        /// </summary>
        /// <param name="tpktHeader">Header to be filled.</param>
        /// <param name="length">The value to be filled in length field of TpktHeader.</param>
        internal static void FillTpktHeader(ref TpktHeader tpktHeader, ushort length)
        {
            tpktHeader.version = ConstValue.TPKT_HEADER_VERSION;
            tpktHeader.reserved = 0;

            // Octets 3 and 4 are the unsigned 16-bit binary encoding of the TPKT length. 
            // Octet 3 is Most significant octet of TPKT length and Octet 4 is Least significant octet of TPKT length.
            tpktHeader.length = (ushort)(length >> 8 | length << 8);
        }


        /// <summary>
        /// This method is used to fill common header for input PDU.
        /// </summary>
        /// <param name="commonHeader">The header to be filled.</param>
        /// <param name="flag">Flag to be set in TS_SECURITY_HEADER.</param>
        /// <param name="context">Specify the user channel Id, I/O channel Id and encryption level.</param>
        public static void FillCommonHeader(
            RdpbcgrClientContext context,
            ref SlowPathPduCommonHeader commonHeader,
            TS_SECURITY_HEADER_flags_Values flag)
        {
            FillTpktHeader(ref commonHeader.tpktHeader, 0);
            FillX224Data(ref commonHeader.x224Data);
            commonHeader.initiator = (ushort)context.UserChannelId;
            commonHeader.channelId = (ushort)context.IOChannelId;
            commonHeader.securityHeader = GenerateSecurityHeader(flag, context.RdpEncryptionMethod);
        }


        internal static void FillCommonHeader(
            RdpbcgrServerSessionContext context,
            ref SlowPathPduCommonHeader commonHeader,
            TS_SECURITY_HEADER_flags_Values flag,
            ushort length = 0,
            byte level = 0,
            byte type = 0,
            byte userDataLength = 0
            )
        {
            FillTpktHeader(ref commonHeader.tpktHeader, length);
            FillX224Data(ref commonHeader.x224Data);
            commonHeader.initiator = (ushort)context.ServerChannelId;
            commonHeader.channelId = (ushort)context.IOChannelId;
            commonHeader.level = level;
            commonHeader.type = type;
            commonHeader.userDataLength = userDataLength;
            //commonHeader.securityHeader = GenerateSecurityHeader(flag, context.RdpEncryptionMethod);
            commonHeader.securityHeader = GenerateSecurityHeaderForServerPDU(flag, context.RdpEncryptionMethod, context.RdpEncryptionLevel);
        }

        /// <summary>
        /// This method is used to fill common header for Server Redirection PDU.
        /// </summary>
        /// <param name="context">Server Session Context.</param>
        /// <param name="commonHeader">The ref of common Header to be filled.</param>
        /// <param name="flag">Security Flag.</param>
        internal static void FillCommonHeaderForServerRedirectionPDU(
            RdpbcgrServerSessionContext context,
            ref SlowPathPduCommonHeader commonHeader,
            TS_SECURITY_HEADER_flags_Values flag,
            ushort length = 0,
            byte level = 0,
            byte type = 0,
            byte userDataLength = 0
            )
        {
            FillTpktHeader(ref commonHeader.tpktHeader, length);
            FillX224Data(ref commonHeader.x224Data);
            commonHeader.initiator = (ushort)context.ServerChannelId;
            commonHeader.channelId = (ushort)context.IOChannelId;
            commonHeader.level = level;
            commonHeader.type = type;
            commonHeader.userDataLength = userDataLength;
            commonHeader.securityHeader = GenerateSecurityHeader(flag, context.RdpEncryptionMethod);
        }


        /// <summary>
        /// This method is used to fill common header for virtual channel PDU.
        /// </summary>
        /// <param name="commonHeader">The header to be filled.</param>
        /// <param name="flag">Flag to be set in TS_SECURITY_HEADER.</param>
        /// <param name="context">Specify the user channel Id and encryption level.</param>
        /// <param name="channelId">Specify the virtual channel Id.</param>
        internal static void FillCommonHeader(ref SlowPathPduCommonHeader commonHeader,
                                              TS_SECURITY_HEADER_flags_Values flag,
                                              RdpbcgrClientContext context,
                                              long channelId)
        {
            FillTpktHeader(ref commonHeader.tpktHeader, 0);
            FillX224Data(ref commonHeader.x224Data);
            commonHeader.initiator = (ushort)context.UserChannelId;
            commonHeader.channelId = (ushort)channelId;
            commonHeader.securityHeader = GenerateSecurityHeader(flag, context.RdpEncryptionMethod);
        }

        /// <summary>
        /// This method is used to fill common header for virtual channel PDU.
        /// </summary>
        /// <param name="commonHeader">The header to be filled.</param>
        /// <param name="flag">Flag to be set in TS_SECURITY_HEADER.</param>
        /// <param name="context">Specify the user channel Id and encryption level.</param>
        /// <param name="channelId">Specify the virtual channel Id.</param>
        /// <param name="autoDetectReqData"></param>
        internal static void FillCommonHeader(ref SlowPathPduCommonHeader commonHeader,
                                              TS_SECURITY_HEADER_flags_Values flag,
                                              RdpbcgrServerSessionContext context,
                                              long channelId)
        {
            FillTpktHeader(ref commonHeader.tpktHeader, 0);
            FillX224Data(ref commonHeader.x224Data);
            commonHeader.initiator = (ushort)context.ServerChannelId;
            commonHeader.channelId = (ushort)channelId;
            commonHeader.securityHeader = GenerateSecurityHeaderForServerPDU(flag, context.RdpEncryptionMethod, context.RdpEncryptionLevel);
        }

        /// <summary>
        /// This method is used to fill Share Control Header structure.
        /// </summary>
        /// <param name="controlHeader">Header to be filled.</param>
        /// <param name="length">The value to be filled in length field of controlHeader.</param>
        /// <param name="type">The value to be filled in pduType field of controlHeader.</param>
        /// <param name="pduSource">The value to be filled in pduSource field of controlHeader.</param>
        internal static void FillShareControlHeader(ref TS_SHARECONTROLHEADER controlHeader,
                                                    ushort length,
                                                    ShareControlHeaderType type,
                                                    ushort pduSource)
        {
            controlHeader.totalLength = length;
            controlHeader.pduType.versionHigh = 0;
            controlHeader.pduType.typeAndVersionLow = (byte)((byte)type
                                                    | (byte)ControlHeaderVersionLow.TS_PROTOCOL_VERSION);
            controlHeader.pduSource = pduSource;
        }


        /// <summary>
        /// This method is used to fill Share Data Header structure.
        /// </summary>
        /// <param name="shareHeader">Header to be filled.</param>
        /// <param name="payloadLength">The length of the data following Share Data Header.</param>
        /// <param name="context">Specify the user channel Id and share Id.</param>
        /// <param name="streamId">The value to be filled in streamId field of shareHeader.</param>
        /// <param name="pduType2">The value to be filled in pduType2 field of shareHeader.</param>
        /// <param name="compressedType">The value to be filled in compressedType field of shareHeader.</param>
        /// <param name="compressedLength">The value to be filled in compressedLength field of shareHeader.</param>
        internal static void FillShareDataHeader(ref TS_SHAREDATAHEADER shareHeader,
                                                 ushort payloadLength,
                                                 RdpbcgrClientContext context,
                                                 streamId_Values streamId,
                                                 pduType2_Values pduType2,
                                                 compressedType_Values compressedType,
                                                 ushort compressedLength)
        {
            FillShareControlHeader(ref shareHeader.shareControlHeader,
                                   (ushort)(payloadLength + Marshal.SizeOf(shareHeader)),
                                   ShareControlHeaderType.PDUTYPE_DATAPDU,
                                   (ushort)(context.UserChannelId));
            shareHeader.shareId = context.ShareId;
            shareHeader.pad1 = 0;
            shareHeader.streamId = streamId;
            shareHeader.uncompressedLength = shareHeader.shareControlHeader.totalLength;
            shareHeader.pduType2 = pduType2;
            shareHeader.compressedType = compressedType;
            shareHeader.compressedLength = compressedLength;
        }

        internal static void FillShareDataHeader(ref TS_SHAREDATAHEADER shareHeader,
                                                 ushort payloadLength,
                                                 RdpbcgrServerSessionContext context,
                                                 streamId_Values streamId,
                                                 pduType2_Values pduType2,
                                                 compressedType_Values compressedType,
                                                 ushort compressedLength)
        {
            FillShareControlHeader(ref shareHeader.shareControlHeader,
                                   (ushort)(payloadLength + Marshal.SizeOf(shareHeader)),
                                   ShareControlHeaderType.PDUTYPE_DATAPDU,
                                   (ushort)(context.UserChannelId));
            shareHeader.shareId = context.ShareId;
            shareHeader.pad1 = 0;
            shareHeader.streamId = streamId;
            shareHeader.uncompressedLength = shareHeader.shareControlHeader.totalLength;
            shareHeader.pduType2 = pduType2;
            shareHeader.compressedType = compressedType;
            shareHeader.compressedLength = compressedLength;
        }


        /// <summary>
        /// This method is used to generate a security header.
        /// </summary>
        /// <param name="flag">Flag to be set in TS_SECURITY_HEADER.</param>
        /// <param name="encryptionMethod">Specify which kind of security header will be generated.</param>
        /// <param name="autoDetectReqData">autoDetectReqData to be filled in TS_SECURITY_HEADER</param>
        /// <returns>A security header.</returns>
        internal static TS_SECURITY_HEADER GenerateSecurityHeader(TS_SECURITY_HEADER_flags_Values flag,
                                                                  EncryptionMethods encryptionMethod)
        {
            TS_SECURITY_HEADER securityHeader = null;

            if (encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT
                | encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT
                | encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)  // non-fips security header
            {
                TS_SECURITY_HEADER1 header = new TS_SECURITY_HEADER1();
                header.flags = flag | TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT;
                header.flagsHi = 0;
                header.dataSignature = null;
                securityHeader = header;
            }
            else if (encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS) // fips security header
            {
                TS_SECURITY_HEADER2 header = new TS_SECURITY_HEADER2();
                header.flags = flag | TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT;
                header.flagsHi = 0;
                header.length = TS_SECURITY_HEADER2_length_Values.V1;
                header.version = ConstValue.TSFIPS_VERSION1;
                header.padlen = 0;
                header.dataSignature = null;
                securityHeader = header;
            }
            // else no security header
            if ((flag.HasFlag(TS_SECURITY_HEADER_flags_Values.SEC_AUTODETECT_REQ)
                || flag.HasFlag(TS_SECURITY_HEADER_flags_Values.SEC_AUTODETECT_RSP)
                || flag.HasFlag(TS_SECURITY_HEADER_flags_Values.SEC_TRANSPORT_REQ)
                || flag.HasFlag(TS_SECURITY_HEADER_flags_Values.SEC_HEARTBEAT)
                || flag.HasFlag(TS_SECURITY_HEADER_flags_Values.SEC_LICENSE_PKT))
                && securityHeader == null)
            {
                //if flag contain SEC_AUTODETECT_REQ, it's for auto-detect, the securityheader must present
                securityHeader = new TS_SECURITY_HEADER();
                securityHeader.flags = flag;
                securityHeader.flagsHi = 0;
            }
            return securityHeader;
        }

        /// <summary>
        /// This method is used to generate a security header for RDP server PDUs.
        /// </summary>
        /// <param name="flag">Flag to be set in TS_SECURITY_HEADER.</param>
        /// <param name="encryptionMethod">Specify which kind of security header will be generated.</param>
        /// <param name="autoDetectReqData">autoDetectReqData to be filled in TS_SECURITY_HEADER</param>
        /// <returns>A security header.</returns>
        internal static TS_SECURITY_HEADER GenerateSecurityHeaderForServerPDU(TS_SECURITY_HEADER_flags_Values flag,
                                                                  EncryptionMethods encryptionMethod, EncryptionLevel encryptionLevel)
        {
            TS_SECURITY_HEADER securityHeader = null;

            if (encryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_LOW)
            {
                securityHeader = new TS_SECURITY_HEADER();
                securityHeader.flags = flag;
                securityHeader.flagsHi = 0;
            }
            else if (encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_40BIT
                | encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_56BIT
                | encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_128BIT)  // non-fips security header
            {
                TS_SECURITY_HEADER1 header = new TS_SECURITY_HEADER1();
                header.flags = flag | TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT;
                header.flagsHi = 0;
                header.dataSignature = null;
                securityHeader = header;
            }
            else if (encryptionMethod == EncryptionMethods.ENCRYPTION_METHOD_FIPS) // fips security header
            {
                TS_SECURITY_HEADER2 header = new TS_SECURITY_HEADER2();
                header.flags = flag | TS_SECURITY_HEADER_flags_Values.SEC_ENCRYPT;
                header.flagsHi = 0;
                header.length = TS_SECURITY_HEADER2_length_Values.V1;
                header.version = ConstValue.TSFIPS_VERSION1;
                header.padlen = 0;
                header.dataSignature = null;
                securityHeader = header;
            }
            // else no security header
            if (((flag & TS_SECURITY_HEADER_flags_Values.SEC_AUTODETECT_REQ) == TS_SECURITY_HEADER_flags_Values.SEC_AUTODETECT_REQ
                || (flag & TS_SECURITY_HEADER_flags_Values.SEC_TRANSPORT_REQ) == TS_SECURITY_HEADER_flags_Values.SEC_TRANSPORT_REQ
                || (flag & TS_SECURITY_HEADER_flags_Values.SEC_HEARTBEAT) == TS_SECURITY_HEADER_flags_Values.SEC_HEARTBEAT)
                && securityHeader == null)
            {
                //if flag contain SEC_AUTODETECT_REQ, it's for auto-detect, the securityheader must present
                securityHeader = new TS_SECURITY_HEADER();
                securityHeader.flags = flag;
                securityHeader.flagsHi = 0;
            }

            return securityHeader;
        }

        /// <summary>
        /// generate a RTT Measure Request.
        /// </summary>
        /// <param name="requestType">request type, must be RDP_RTT_REQUEST_IN_AUTO_DETECT_PDU or RDP_RTT_REQUEST_IN_HEADER</param>
        /// <param name="sequenceNumber"></param>
        /// <returns>The generated RDP_RTT_REQUEST</returns>
        public static RDP_RTT_REQUEST GenerateRTTMeasureRequest(AUTO_DETECT_REQUEST_TYPE requestType, ushort sequenceNumber)
        {
            if (!(requestType == AUTO_DETECT_REQUEST_TYPE.RDP_RTT_REQUEST_IN_CONNECTTIME || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_RTT_REQUEST_AFTER_CONNECTTIME))
                throw new ArgumentException("When generate RDP_RTT_REQUEST, request type must be RDP_RTT_REQUEST_IN_CONNECTTIME or RDP_RTT_REQUEST_AFTER_CONNECTTIME");
            RDP_RTT_REQUEST rttReq = new RDP_RTT_REQUEST();
            rttReq.headerLength = 0x06;
            rttReq.headerTypeId = HeaderTypeId_Values.TYPE_ID_AUTODETECT_REQUEST;
            rttReq.sequenceNumber = sequenceNumber;
            rttReq.requestType = requestType;

            return rttReq;
        }

        public static RDP_RTT_RESPONSE GenerateRTTMeasureResponse(ushort sequenceNumber)
        {
            RDP_RTT_RESPONSE rttResp = new RDP_RTT_RESPONSE();
            rttResp.headerLength = 0x06;
            rttResp.headerTypeId = HeaderTypeId_Values.TYPE_ID_AUTODETECT_RESPONSE;
            rttResp.sequenceNumber = sequenceNumber;
            rttResp.responseType = AUTO_DETECT_RESPONSE_TYPE.RDP_RTT_RESPONSE;

            return rttResp;
        }
        /// <summary>
        /// Generate a Bandwidth Measure Start
        /// </summary>
        /// <param name="requestType">Request type, must be RDP_BW_START_IN_HEADER_RELIABLE, RDP_BW_START_IN_HEADER_LOSSY or RDP_BW_START_IN_AUTO_DETECT_PDU</param>
        /// <param name="sequenceNumber"></param>
        /// <returns>The generated RDP_BW_START</returns>
        public static RDP_BW_START GenerateBandwidthMeasureStart(AUTO_DETECT_REQUEST_TYPE requestType, ushort sequenceNumber)
        {
            if (!(requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_IN_CONNECTTIME || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_AFTER_CONNECTTIME_OR_LOSSYUDP || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_START_AFTER_CONNECTTIME_OR_RELIABLEUDP))
                throw new ArgumentException("When generate RDP_BW_START, request type must be RDP_BW_START_IN_CONNECTTIME, RDP_BW_START_AFTER_CONNECTTIME_OR_LOSSYUDP or RDP_BW_START_AFTER_CONNECTTIME_OR_RELIABLEUDP");
            RDP_BW_START bwStart = new RDP_BW_START();
            bwStart.headerLength = 0x06;
            bwStart.headerTypeId = HeaderTypeId_Values.TYPE_ID_AUTODETECT_REQUEST;
            bwStart.sequenceNumber = sequenceNumber;
            bwStart.requestType = requestType;

            return bwStart;
        }

        /// <summary>
        /// Generate Bandwidth Measure Payload 
        /// </summary>
        /// <param name="requestType">Request type, must be RDP_BW_PAYLOAD</param>
        /// <param name="sequenceNumber"></param>
        /// <param name="payloadLength"></param>
        /// <param name="payload"></param>
        /// <returns>The generated RDP_BW_PAYLOAD</returns>
        public static RDP_BW_PAYLOAD GenerateBandwidthMeasurePayload(AUTO_DETECT_REQUEST_TYPE requestType, ushort sequenceNumber, ushort payloadLength, byte[] payload)
        {
            if (!(requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_PAYLOAD))
                throw new ArgumentException("When generate RDP_BW_PAYLOAD, request type must be RDP_BW_PAYLOAD");
            RDP_BW_PAYLOAD bwPayload = new RDP_BW_PAYLOAD();
            bwPayload.headerLength = 0x08;
            bwPayload.headerTypeId = HeaderTypeId_Values.TYPE_ID_AUTODETECT_REQUEST;
            bwPayload.sequenceNumber = sequenceNumber;
            bwPayload.requestType = requestType;
            bwPayload.payloadLength = payloadLength;
            bwPayload.payload = RdpbcgrUtility.CloneByteArray(payload);

            return bwPayload;
        }

        /// <summary>
        /// Generate Bandwidth Measure Stop
        /// </summary>
        /// <param name="requestType">Request Type, must be RDP_BW_STOP_IN_HEADER_RELIABLE, RDP_BW_STOP_IN_HEADER_LOSSY or RDP_BW_STOP_IN_AUTO_DETECT_PDU</param>
        /// <param name="sequenceNumber"></param>
        /// <param name="payloadLength"></param>
        /// <param name="payload"></param>
        /// <returns>The generated RDP_BW_STOP</returns>
        public static RDP_BW_STOP GenerateBandwidthMeasureStop(AUTO_DETECT_REQUEST_TYPE requestType, ushort sequenceNumber, ushort payloadLength = 0, byte[] payload = null)
        {
            if (!(requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_IN_CONNECTTIME || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_AFTER_CONNECTTIME_OR_RELIABLEUDP || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_AFTER_CONNECTTIME_OR_LOSSYUDP))
                throw new ArgumentException("When generate RDP_BW_STOP, request type must be RDP_BW_STOP_IN_CONNECTTIME, RDP_BW_STOP_AFTER_CONNECTTIME_OR_RELIABLEUDP or RDP_BW_STOP_AFTER_CONNECTTIME_OR_LOSSYUDP");
            RDP_BW_STOP bwStop = new RDP_BW_STOP();
            if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_BW_STOP_IN_CONNECTTIME)
            {
                bwStop.headerLength = 0x08;
                bwStop.payloadLength = payloadLength;
                bwStop.payload = RdpbcgrUtility.CloneByteArray(payload);
            }
            else
                bwStop.headerLength = 0x06;
            bwStop.headerTypeId = HeaderTypeId_Values.TYPE_ID_AUTODETECT_REQUEST;
            bwStop.sequenceNumber = sequenceNumber;
            bwStop.requestType = requestType;

            return bwStop;
        }

        /// <summary>
        /// Generate a RDP_BW_RESULTS
        /// </summary>
        /// <param name="responseType"></param>
        /// <param name="sequenceNumber"></param>
        /// <param name="timeDelta"></param>
        /// <param name="byteCount"></param>
        /// <returns></returns>
        public static RDP_BW_RESULTS GenerateBandwidthMeasureResults(AUTO_DETECT_RESPONSE_TYPE responseType, ushort sequenceNumber, uint timeDelta, uint byteCount)
        {
            if (!(responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_BW_RESULTS_AFTER_CONNECT || responseType == AUTO_DETECT_RESPONSE_TYPE.RDP_BW_RESULTS_DURING_CONNECT))
            {
                throw new ArgumentException("When generate RDP_BW_RESULTS, response type must be RDP_BW_RESULTS_AFTER_CONNECT or RDP_BW_RESULTS_DURING_CONNECT");
            }

            RDP_BW_RESULTS bwResult = new RDP_BW_RESULTS();
            bwResult.headerLength = 0x0E;
            bwResult.headerTypeId = HeaderTypeId_Values.TYPE_ID_AUTODETECT_RESPONSE;
            bwResult.responseType = responseType;
            bwResult.sequenceNumber = sequenceNumber;
            bwResult.timeDelta = timeDelta;
            bwResult.byteCount = byteCount;

            return bwResult;
        }

        /// <summary>
        /// Generate Network Characteristics Result
        /// </summary>
        /// <param name="requestType">Request Type, must be RDP_NETCHAR_RESULT_BANDWIDTH_AVERAGERTT, RDP_NETCHAR_RESULT_BASERTT_AVERAGERTT or RDP_NETCHAR_RESULT_BASERTT_BANDWIDTH_AVERAGERTT</param>
        /// <param name="sequenceNumber"></param>
        /// <param name="baseRTT"></param>
        /// <param name="bandwidth"></param>
        /// <param name="averageRTT"></param>
        /// <returns>Generated RDP_NETCHAR_RESULT</returns>
        public static RDP_NETCHAR_RESULT GenerateNetworkCharacteristicsResult(AUTO_DETECT_REQUEST_TYPE requestType, ushort sequenceNumber, uint baseRTT, uint bandwidth, uint averageRTT)
        {
            if (!(requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BANDWIDTH_AVERAGERTT || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_AVERAGERTT || requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_BANDWIDTH_AVERAGERTT))
                throw new ArgumentException("When generate RTT Measure Request, request type must be RDP_NETCHAR_RESULT_BANDWIDTH_AVERAGERTT, RDP_NETCHAR_RESULT_BASERTT_AVERAGERTT or RDP_NETCHAR_RESULT_BASERTT_BANDWIDTH_AVERAGERTT");
            RDP_NETCHAR_RESULT netResult = new RDP_NETCHAR_RESULT();
            if (requestType == AUTO_DETECT_REQUEST_TYPE.RDP_NETCHAR_RESULT_BASERTT_BANDWIDTH_AVERAGERTT)
            {
                netResult.headerLength = 0x12;
            }
            else
            {
                netResult.headerLength = 0x0E;
            }
            netResult.headerTypeId = HeaderTypeId_Values.TYPE_ID_AUTODETECT_REQUEST;
            netResult.requestType = requestType;
            netResult.sequenceNumber = sequenceNumber;
            netResult.baseRTT = baseRTT;
            netResult.bandwidth = bandwidth;
            netResult.averageRTT = averageRTT;

            return netResult;
        }

        /// <summary>
        /// Generate Network Characteristics Sync 
        /// </summary>
        /// <param name="sequenceNumber">A 16-bit unsigned integer that specifies the message sequence number.</param>
        /// <param name="bandwidth">A 32-bit unsigned integer that specifies the previously detected bandwidth in kilobits per second.</param>
        /// <param name="rtt">A 32-bit unsigned integer that specifies the previously detected round-trip time in milliseconds.</param>
        /// <returns></returns>
        public static RDP_NETCHAR_SYNC GenerateNetworkCharacteristicsSync(ushort sequenceNumber, uint bandwidth, uint rtt)
        {
            RDP_NETCHAR_SYNC netSync = new RDP_NETCHAR_SYNC();
            netSync.headerLength = 0x0E;
            netSync.headerTypeId = HeaderTypeId_Values.TYPE_ID_AUTODETECT_RESPONSE;
            netSync.sequenceNumber = sequenceNumber;
            netSync.responseType = AUTO_DETECT_RESPONSE_TYPE.RDP_NETCHAR_SYNC;
            netSync.bandwidth = bandwidth;
            netSync.rtt = rtt;

            return netSync;
        }

        /// <summary>
        /// Convert byte list to byte array and reszie the length field of the tpktHeader.
        /// </summary>
        public static byte[] ToBytes(byte[] sendBuffer)
        {
            // If the tpkeHeader length has not been set (= 0), reset it.
            // Otherwise, keep the old value.
            if (sendBuffer.Length >= Marshal.SizeOf(typeof(TpktHeader))
                && sendBuffer[2] == 0 && sendBuffer[3] == 0)
            {
                ResetTpktHeaderLength(sendBuffer);
            }
            // else do nothing

            return sendBuffer;
        }

        /// <summary>
        /// Convert byte list to byte array and reszie the length field of the tpktHeader.
        /// </summary>
        /// <param name="sendBuffer">The list to be converted.</param>
        /// <returns>The converted byte array.</returns>
        internal static byte[] ToBytes(List<byte> sendBuffer)
        {
            byte[] allDataBuffer = sendBuffer.ToArray();

            return ToBytes(allDataBuffer);
        }


        /// <summary>
        /// Clone a capabiliy set.
        /// </summary>
        /// <param name="capSet">The capabiliy set to be cloned.</param>
        /// <returns>The cloned capabiliy set.</returns>
        internal static ITsCapsSet CloneCapabilitySet(ITsCapsSet capSet)
        {
            ITsCapsSet cloneSet = capSet;

            if (capSet.GetType() == typeof(TS_ORDER_CAPABILITYSET))
            {
                TS_ORDER_CAPABILITYSET capability = (TS_ORDER_CAPABILITYSET)capSet;
                capability.terminalDescriptor = CloneByteArray(capability.terminalDescriptor);
                capability.orderSupport = CloneByteArray(capability.orderSupport);
                cloneSet = capability;
            }
            else if (capSet.GetType() == typeof(TS_BITMAPCACHE_CAPABILITYSET_REV2))
            {
                TS_BITMAPCACHE_CAPABILITYSET_REV2 capability = (TS_BITMAPCACHE_CAPABILITYSET_REV2)capSet;
                capability.Pad3 = CloneByteArray(capability.Pad3);
                cloneSet = capability;
            }
            else if (capSet.GetType() == typeof(TS_GLYPHCACHE_CAPABILITYSET))
            {
                TS_GLYPHCACHE_CAPABILITYSET capability = (TS_GLYPHCACHE_CAPABILITYSET)capSet;
                if (capability.GlyphCache != null)
                {
                    capability.GlyphCache = (TS_CACHE_DEFINITION[])capability.GlyphCache.Clone();
                }

                cloneSet = capability;
            }
            else if (capSet.GetType() == typeof(TS_BITMAPCODECS_CAPABILITYSET))
            {
                TS_BITMAPCODECS_CAPABILITYSET capability = (TS_BITMAPCODECS_CAPABILITYSET)capSet;

                if (capability.supportedBitmapCodecs.bitmapCodecArray != null)
                {
                    capability.supportedBitmapCodecs.bitmapCodecArray =
                        (TS_BITMAPCODEC[])capability.supportedBitmapCodecs.bitmapCodecArray.Clone();
                    for (int j = 0; j < capability.supportedBitmapCodecs.bitmapCodecArray.Length; ++j)
                    {
                        capability.supportedBitmapCodecs.bitmapCodecArray[j].codecProperties =
                          CloneByteArray(capability.supportedBitmapCodecs.bitmapCodecArray[j].codecProperties);
                    }

                    cloneSet = capability;
                }
            }
            // else no more operation

            return cloneSet;
        }


        /// <summary>
        /// Clone byte array.
        /// </summary>
        /// <param name="byteArray">The byte array to be cloned.</param>
        /// <returns>The cloned byte array.</returns>
        internal static byte[] CloneByteArray(byte[] byteArray)
        {
            return (byteArray == null) ? null : byteArray.ToArray();
        }

        internal static string CloneString(string sourceString)
        {
            return (sourceString == null) ? null : String.Copy(sourceString);
        }


        /// <summary>
        /// Link several byte arrays into one byte array.
        /// </summary>
        /// <param name="arrays">The byte arrays.</param>
        /// <returns>The whole byte array.</returns>
        internal static byte[] ConcatenateArrays(params byte[][] arrays)
        {
            if (arrays == null)
            {
                return null;
            }

            List<byte> arrayList = new List<byte>();

            foreach (byte[] array in arrays)
            {
                if (array != null)
                {
                    arrayList.AddRange(array);
                }
            }

            return arrayList.ToArray();
        }


        /// <summary>
        /// Encode string into byte array.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <returns></returns>
        public static byte[] EncodeUnicodeStringToBytes(string sourceString)
        {
            var result = new List<byte>();
            RdpbcgrEncoder.EncodeUnicodeString(result, sourceString, RdpbcgrEncoder.CalculateUnicodeStringEncodingSize(sourceString, true));
            return result.ToArray();
        }

        /// <summary>
        /// Return the size of TS_FP_INPUT_PDU
        /// </summary>
        public static int GetPduSize(TS_FP_INPUT_PDU pdu)
        {
            // [MS-RDPBCGR] section 2.2.8.1.2	Client Fast-Path Input Event PDU (TS_FP_INPUT_PDU)
            int pduSize = 0;
            pduSize += 1; // pdu.fpInputHeader
            pduSize += 1; // pdu.length1
            if ((ConstValue.MOST_SIGNIFICANT_BIT_FILTER & pdu.length1) != pdu.length1)
            {
                // length1's most significant bit is set, then length2 is present.
                pduSize += 1; // Optional: pdu.length2;
            }

            if (pdu.fipsInformation.length != 0)
            {
                pduSize += 4; // Optional: pdu.fipsInformation
            }

            if (pdu.dataSignature != null)
            {
                pduSize += 8; //Optional: pdu.dataSignature 
            }

            int numberEvents = pdu.fpInputHeader.numEvents;
            if (numberEvents == 0)
            {
                numberEvents = pdu.numberEvents;
                if (numberEvents != 0)
                {
                    pduSize += 1; //Optional: pdu.numberEvents
                }
                else
                {
                    return pduSize;
                }
            }

            // [MS-RDPBCGR] section 2.2.8.1.2.2	Fast-Path Input Event (TS_FP_INPUT_EVENT)
            TS_FP_INPUT_EVENT inputEvent;
            for (int i = 0; i < numberEvents; i++)
            {
                inputEvent = pdu.fpInputEvents[i];
                pduSize += 1; // inputEvent.eventHeader;
                if (inputEvent.eventData != null)
                {
                    pduSize += Marshal.SizeOf(inputEvent.eventData.GetType());
                }
            }

            return pduSize;
        }

        /// <summary>
        /// Return the size of TS_FP_UPDATE_PDU
        /// </summary>
        public static int GetPduSize(TS_FP_UPDATE_PDU pdu)
        {
            // [MS-RDPBCGR] section 2.2.9.1.2	Server Fast-Path Update PDU (TS_FP_UPDATE_PDU)
            int pduSize = 0;
            pduSize += 1; // pdu.fpInputHeader
            pduSize += 1; // pdu.length1
            if ((ConstValue.MOST_SIGNIFICANT_BIT_FILTER & pdu.length1) != pdu.length1)
            {
                // length1's most significant bit is set, then length2 is present.
                pduSize += 1; // Optional: pdu.length2;
            }

            if (pdu.fipsInformation.length != 0)
            {
                pduSize += 4; // Optional: pdu.fipsInformation
            }

            if (pdu.dataSignature != null)
            {
                pduSize += 8; //Optional: pdu.dataSignature 
            }

            // [MS-RDPBCGR] section 2.2.9.1.2.1	Fast-Path Update (TS_FP_UPDATE)
            TS_FP_UPDATE fpUpdate;
            for (int i = 0; i < pdu.fpOutputUpdates.Length; i++)
            {
                fpUpdate = pdu.fpOutputUpdates[i];
                pduSize += 1; // fpUpdate.updateHeader
                byte comp = (byte)((fpUpdate.updateHeader & 0xc0) >> 6);
                if ((compression_Values)comp == compression_Values.FASTPATH_OUTPUT_COMPRESSION_USED)
                {
                    pduSize += 1; //Optional: fpUpdate.compressionFlags 
                }

                pduSize += 2; //fpUpdate.size

                pduSize += fpUpdate.size; //updateData 
            }

            return pduSize;
        }

        /// <summary>
        /// Calculate the overall length of TS_FP_INPUT_PDU or TS_FP_UPDATE_PDU
        /// (based on field values of "length1" and "length2")
        /// </summary>
        /// <param name="length1">value of length1 field</param>
        /// <param name="length2">value of length2 field</param>
        /// <returns>caculated PDU length</returns>
        public static UInt16 CalculateFpUpdatePduLength(byte length1, byte length2)
        {
            if ((ConstValue.MOST_SIGNIFICANT_BIT_FILTER & length1) == length1)
            {
                // when length1's most significant bit is not set
                // only length1 is considered
                return (UInt16)length1;
            }
            else
            {
                // when length1's most significant bit is set
                // length1 and length2 are concatenated
                byte[] buffer = new byte[2];
                buffer[0] = length2;
                buffer[1] = (byte)(ConstValue.MOST_SIGNIFICANT_BIT_FILTER & length1);
                UInt16 length = BitConverter.ToUInt16(buffer, 0);
                return length;
            }
        }

        /// <summary>
        /// Encode the certificate.
        /// </summary>
        /// <param name="certificate">The certificate to be encoded.</param>
        /// <returns></returns>
        public static byte[] EncodeCertificate(X509Certificate2 certificate)
        {
            var container = new TARGET_CERTIFICATE_CONTAINER();

            container.elements = new CERTIFICATE_META_ELEMENT[1];

            var element = new CERTIFICATE_META_ELEMENT();

            element.type = (UInt32)CERTIFICATE_META_ELEMENT_TypeEnum.ELEMENT_TYPE_CERTIFICATE;

            element.encoding = (UInt32)CERTIFICATE_META_ELEMENT_EncodingEnum.ENCODING_TYPE_ASN1_DER;

            element.elementSize = (UInt32)certificate.RawData.Length;

            element.elementData = certificate.RawData;

            container.elements[0] = element;

            // Encode using Base64 in Unicode format
            var encodedString = Convert.ToBase64String(container.Encode());

            var result = EncodeUnicodeStringToBytes(encodedString);

            return result;
        }

        #region private methods
        /// <summary>
        /// Do RSA encryption. Follow the example in [MS-RDPBCGR] 4.8
        /// </summary>
        /// <param name="data">The data to be encrypted.</param>
        /// <param name="exponent">Exponent of RSA.</param>
        /// <param name="modulus">Modulus of RSA.</param>
        /// <returns>Encrypted data.</returns>
        private static byte[] RSAEncrypt(byte[] data, byte[] exponent, byte[] modulus)
        {
            // Add an extra zero after the end of the byte array, to indicate that it is a positive number.
            byte[] tempData = new byte[data.Length + 1];
            byte[] tempExponent = new byte[exponent.Length + 1];
            byte[] tempModulus = new byte[modulus.Length + 1];
            Buffer.BlockCopy(data, 0, tempData, 0, data.Length);
            Buffer.BlockCopy(exponent, 0, tempExponent, 0, exponent.Length);
            Buffer.BlockCopy(modulus, 0, tempModulus, 0, modulus.Length);

            BigInteger mpData = new BigInteger(tempData);
            BigInteger mpExponent = new BigInteger(tempExponent);
            BigInteger mpModulus = new BigInteger(tempModulus);

            // mpResult = mpData ^ mpExponent mod mpModulus.
            BigInteger mpResult = BigInteger.ModPow(mpData, mpExponent, mpModulus);

            byte[] result = mpResult.ToByteArray();

            return result;
        }

        /// <summary>
        /// This method is used to set the actual length in TPKT header.
        /// </summary>
        /// <param name="allDataBuffer">Specify the buffer of the TPKT PDU.</param>
        private static void ResetTpktHeaderLength(byte[] allDataBuffer)
        {
            int length = allDataBuffer.Length;

            // set the length of the whole PDU in TPKT header
            if (length >= Marshal.SizeOf(typeof(TpktHeader)))
            {
                // allDataBuffer[2] and allDataBuffer[3] make the length field of TPKT header
                allDataBuffer[2] = (byte)((length >> 8) & 0xFF);
                allDataBuffer[3] = (byte)(length & 0xFF);
            }
            // else do nothing
        }
        #endregion private methods
    }
}
