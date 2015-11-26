// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.Protocols.TestTools.StackSdk.Messages;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Pac
{
    /// <summary>
    ///  The PAC_CREDENTIAL_INFO structure serves as the header
    ///  for the credential information. The PAC_CREDENTIAL_INFO
    ///  header indicates the encryption algorithm that was
    ///  used to encrypt the data that follows it. The data
    ///  that follows is an encrypted, IDL-serialized PAC_CREDENTIAL_DATA
    ///  structure that contains the user's actual credentials.
    ///  Note that this structure cannot be used by protocols
    ///  other than the Kerberos protocol; the encryption method
    ///  relies on the encryption key currently in use by the
    ///  Kerberos AS-REQ ([RFC4120] section 3.1 and [MS-KILE])
    ///  message.This buffer is inserted into the PAC only when
    ///  initial authentication is done through the PKINIT protocol
    ///  (as specified in [RFC4556]) and is inserted only during
    ///  initial logon; it is not included when the ticket-granting
    ///  ticket (TGT) is used for further authentication.A PAC_CREDENTIAL_INFO
    ///  structure contains the encrypted user's credentials.
    ///   The encryption key usage number [RFC4120] used in
    ///  the encryption is KERB_NON_KERB_SALT (16). The encryption
    ///  key used is the AS reply key. The PAC credentials buffer
    ///  SHOULD be included only when PKINIT [RFC4556] is used.
    ///  Therefore, the AS reply key is derived based on PKINIT.The
    ///  byte array of encrypted data is computed per the procedures
    ///  specified in [RFC3961].
    /// </summary>
    public class PacCredentialInfo : PacInfoBuffer
    {
        /// <summary>
        /// The Key Usage Number used in credential,
        /// defined in TD section 2.6.1:
        /// "Key Usage Number ([RFC4120] sections 4 and 7.5.1) is KERB_NON_KERB_SALT (16)"
        /// </summary>
        private const int KerbNonKerbSalt = 16;

        /// <summary>
        /// The native PAC_CREDENTIAL_INFO object.
        /// </summary>
        public PAC_CREDENTIAL_INFO NativePacCredentialInfo;


        /// <summary>
        /// Encrypt a PacCredentialData instance. The encrypted data
        /// can be accessed from SerializedData property.
        /// </summary>
        /// <param name="credentialData">The _PAC_CREDENTIAL_DATA instance to be encrypted.</param>
        /// <param name="key">The encrypt key.</param>
        public void Encrypt(_PAC_CREDENTIAL_DATA credentialData, byte[] key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            byte[] plain = null;
            using (SafeIntPtr ptr = TypeMarshal.ToIntPtr(credentialData))
            {
                plain = PacUtility.NdrMarshal(ptr, FormatString.OffsetCredentialData);
            }
            NativePacCredentialInfo.SerializedData = Encrypt(key, plain, NativePacCredentialInfo.EncryptionType);
        }


        /// <summary>
        /// Decrypt the data of SerializedData property into
        /// a PacCredentialData instance.
        /// </summary>
        /// <param name="key">The decrypt key.</param>
        /// <returns>The decrypted _PAC_CREDENTIAL_DATA instance.</returns>
        public _PAC_CREDENTIAL_DATA Decrypt(byte[] key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            byte[] plain = Decrypt(
                key,
                NativePacCredentialInfo.SerializedData,
                NativePacCredentialInfo.EncryptionType);

            return PacUtility.NdrUnmarshal<_PAC_CREDENTIAL_DATA>(plain, 0, plain.Length, FormatString.OffsetCredentialData);
        }


        /// <summary>
        /// Decode specified buffer from specified index, with specified count
        /// of bytes, into the instance of current class.
        /// </summary>
        /// <param name="buffer">The specified buffer.</param>
        /// <param name="index">The specified index from beginning of buffer.</param>
        /// <param name="count">The specified count of bytes to be decoded.</param>
        internal override void DecodeBuffer(byte[] buffer, int index, int count)
        {
            NativePacCredentialInfo =
                PacUtility.MemoryToObject<PAC_CREDENTIAL_INFO>(buffer, index, count);

            int headerLength = sizeof(uint) + sizeof(uint);

            // This is vary length member without pre-defined calculate method.
            // Need to decode manually.
            NativePacCredentialInfo.SerializedData = new byte[count - headerLength];

            Buffer.BlockCopy(buffer, index + headerLength, NativePacCredentialInfo.SerializedData, 0, count - headerLength);
        }


        /// <summary>
        /// Encode the instance of current class into byte array,
        /// according to TD specification.
        /// </summary>
        /// <returns>The encoded byte array</returns>
        internal override byte[] EncodeBuffer()
        {
            byte[] version = BitConverter.GetBytes((int)NativePacCredentialInfo.Version);
            byte[] type = BitConverter.GetBytes((int)NativePacCredentialInfo.EncryptionType);
            byte[] data = NativePacCredentialInfo.SerializedData;

            byte[] result = new byte[version.Length + type.Length + data.Length];

            Buffer.BlockCopy(version, 0, result, 0, version.Length);
            Buffer.BlockCopy(type, 0, result, version.Length, type.Length);
            Buffer.BlockCopy(data, 0, result, version.Length + type.Length, data.Length);

            return result;
        }


        /// <summary>
        /// Calculate size of current instance's encoded buffer, in bytes.
        /// </summary>
        /// <returns>The size of current instance's encoded buffer, in bytes.</returns>
        internal override int CalculateSize()
        {
            int dataLength = NativePacCredentialInfo.SerializedData.Length;
            // The structure contains following part:
            // Version (4 bytes)
            // EncryptionType (4 bytes)
            // SerializedData (variable)
            return sizeof(uint) + sizeof(uint) + dataLength;
        }


        /// <summary>
        /// Get the ulType of current instance's PAC_INFO_BUFFER.
        /// </summary>
        /// <returns>The ulType of current instance's PAC_INFO_BUFFER.</returns>
        internal override PAC_INFO_BUFFER_Type_Values GetBufferInfoType()
        {
            return PAC_INFO_BUFFER_Type_Values.CredentialsInformation;
        }


        /// <summary>
        /// Encrypt specified plain text to cypher, according to specified encryption type.
        /// </summary>
        /// <param name="key">The encrypt key.</param>
        /// <param name="plain">The specified plain text.</param>
        /// <param name="type">The specified encryption type.</param>
        /// <returns>The encrypted cypher.</returns>
        private static byte[] Encrypt(byte[] key, byte[] plain, EncryptionType_Values type)
        {
            switch (type)
            {
                case EncryptionType_Values.DES_CBC_CRC:
                    return DesCbcCrypto.Encrypt(key, plain, EncryptionType.DES_CBC_CRC);

                case EncryptionType_Values.DES_CBC_MD5:
                    return DesCbcCrypto.Encrypt(key, plain, EncryptionType.DES_CBC_MD5);

                case EncryptionType_Values.AES128_CTS_HMAC_SHA1_96:
                    return AesCtsHmacSha1Crypto.Encrypt(key, plain, KerbNonKerbSalt, AesKeyType.Aes128BitsKey);

                case EncryptionType_Values.AES256_CTS_HMAC_SHA1_96:
                    return AesCtsHmacSha1Crypto.Encrypt(key, plain, KerbNonKerbSalt, AesKeyType.Aes256BitsKey);

                case EncryptionType_Values.RC4_HMAC:
                    return Rc4HmacCrypto.Encrypt(key, plain, KerbNonKerbSalt, EncryptionType.RC4_HMAC);

                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }


        /// <summary>
        /// Decrypt specified cypher to plain text, according to specified encryption type.
        /// </summary>
        /// <param name="key">The decrypt key.</param>
        /// <param name="cypher">The specified cypher.</param>
        /// <param name="type">The specified encryption type.</param>
        /// <returns>Yhe decrypted plain text.</returns>
        private static byte[] Decrypt(byte[] key, byte[] cypher, EncryptionType_Values type)
        {
            switch (type)
            {
                case EncryptionType_Values.DES_CBC_CRC:
                    return DesCbcCrypto.Decrypt(key, cypher, EncryptionType.DES_CBC_CRC);

                case EncryptionType_Values.DES_CBC_MD5:
                    return DesCbcCrypto.Decrypt(key, cypher, EncryptionType.DES_CBC_MD5);

                case EncryptionType_Values.AES128_CTS_HMAC_SHA1_96:
                    return AesCtsHmacSha1Crypto.Decrypt(key, cypher, KerbNonKerbSalt, AesKeyType.Aes128BitsKey);

                case EncryptionType_Values.AES256_CTS_HMAC_SHA1_96:
                    return AesCtsHmacSha1Crypto.Decrypt(key, cypher, KerbNonKerbSalt, AesKeyType.Aes256BitsKey);

                case EncryptionType_Values.RC4_HMAC:
                    return Rc4HmacCrypto.Decrypt(key, cypher, KerbNonKerbSalt, EncryptionType.RC4_HMAC);

                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }
    }
}
