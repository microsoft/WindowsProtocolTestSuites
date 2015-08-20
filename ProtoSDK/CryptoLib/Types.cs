// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// Callback function using in decrypt to get to-be-signed data.
    /// </summary>
    /// <param name="data">encrypted/decrypted data.</param>
    /// <returns>to-be-signed data</returns>
    public delegate byte[] GetToBeSignedDataFunc(byte[] data);


    /// <summary>
    /// Specify encryption type.
    /// </summary>
    public enum EncryptionType : int
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represent AES256_CTS_HMAC_SHA1_96  encryption type
        /// </summary>
        AES256_CTS_HMAC_SHA1_96 = 18,

        /// <summary>
        /// Represent AES128_CTS_HMAC_SHA1_96  encryption type
        /// </summary>
        AES128_CTS_HMAC_SHA1_96 = 17,

        /// <summary>
        /// Represent RC4_HMAC  encryption type
        /// </summary>
        RC4_HMAC = 23,

        /// <summary>
        /// Represent RC4_HMAC_EXP  encryption type
        /// </summary>
        RC4_HMAC_EXP = 24,

        /// <summary>
        /// Represent DES_CBC_MD5  encryption type
        /// </summary>
        DES_CBC_MD5 = 3,

        /// <summary>
        /// Represent DES_CBC_CRC  encryption type
        /// </summary>
        DES_CBC_CRC = 1,

        /// <summary>
        /// Represent unused encryption value -135
        /// </summary>
        UnusedValue_135 = -135,

        /// <summary>
        /// Represent unused encryption value -133
        /// </summary>
        UnusedValue_133 = -133,

        /// <summary>
        /// Represent unused encryption value -128
        /// </summary>
        UnusedValue_128 = -128,

        /// <summary>
        /// Represent DES_EDE3_CBC  encryption type in [MS-PKCA].
        /// </summary>
        DES_EDE3_CBC = 15,

        /// <summary>
        /// Represent RC2_CBC  encryption type in [MS-PKCA].
        /// </summary>
        RC2_CBC = 12,
    }


    /// <summary>
    /// Specify checksum type.
    /// </summary>
    public enum ChecksumType : int
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represent CRC32  checksum type
        /// </summary>
        CRC32 = 1,

        /// <summary>
        /// Represent rsa_md4  checksum type
        /// </summary>
        rsa_md4 = 2,

        /// <summary>
        /// Represent rsa_md4_des  checksum type
        /// </summary>
        rsa_md4_des = 3,

        /// <summary>
        /// Represent des_mac  checksum type
        /// </summary>
        des_mac = 4,

        /// <summary>
        /// Represent des_mac_k  checksum type
        /// </summary>
        des_mac_k = 5,

        /// <summary>
        /// Represent rsa_md4_des_k  checksum type
        /// </summary>
        rsa_md4_des_k = 6,

        /// <summary>
        /// Represent rsa_md5  checksum type
        /// </summary>
        rsa_md5 = 7,

        /// <summary>
        /// Represent rsa_md5_des  checksum type
        /// </summary>
        rsa_md5_des = 8,

        /// <summary>
        /// Represent sha1  checksum type
        /// </summary>
        sha1 = -131,

        /// <summary>
        /// Represent hmac_sha1_96_aes128  checksum type
        /// </summary>
        hmac_sha1_96_aes128 = 15,

        /// <summary>
        /// Represent hmac_sha1_96_aes256  checksum type
        /// </summary>
        hmac_sha1_96_aes256 = 16,

        /// <summary>
        /// Represent hmac_md5_string  checksum type
        /// </summary>
        hmac_md5_string = -138,

        /// <summary>
        /// The checksum type used in AP request.
        /// </summary>
        ap_authenticator_8003 = 0x8003,
    }
}