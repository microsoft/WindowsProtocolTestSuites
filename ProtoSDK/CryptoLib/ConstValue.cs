// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// Define const values used in this project.
    /// </summary>
    public static class ConstValue
    {
        #region Encryption and Checksum

        /// <summary>
        /// (8 bits) The length of byte in bits
        /// </summary>
        public const int BYTE_SIZE = 8;

        /// <summary>
        /// (16 bytes = 128 bits) Size of AES encryption block
        /// </summary>
        public const int AES_BLOCK_SIZE = 16;

        /// <summary>
        /// (8 bytes = 64 bits) Size of DES encryption block
        /// </summary>
        public const int DES_BLOCK_SIZE = 8;

        /// <summary>
        /// (1 byte) Size of RC4 encryption block
        /// </summary>
        public const int RC4_BLOCK_SIZE = 1;

        /// <summary>
        /// (64 bits) Block Size in DES-CBC
        /// </summary>
        internal const int DES_CBC_BLOCK_SIZE = 64;

        /// <summary>
        /// (16 bytes) MD5 Checksum Size
        /// </summary>
        public const int MD5_CHECKSUM_SIZE = 16;

        /// <summary>
        /// (4 bytes) CRC32 Checksum Size
        /// </summary>
        public const int CRC32_CHECKSUM_SIZE = 4;

        /// <summary>
        /// (12 bytes) HMAC Hash Output Size
        /// [RFC 3962, Section 6]
        /// </summary>
        public const int HMAC_HASH_OUTPUT_SIZE = 12;

        /// <summary>
        /// The signaturekey in wrap and mic token for [rfc4757].
        /// </summary>
        internal const string SIGNATURE_KEY = "signaturekey\0";

        /// <summary>
        /// The fortybits in wrap and mic token for [rfc4757].
        /// </summary>
        internal const string FORTY_BITS = "fortybits\0";

        /// <summary>
        /// The size of confounder in wrap token for [rfc1964] and [rfc4757].
        /// </summary>
        internal const int CONFOUNDER_SIZE = 8;

        /// <summary>
        /// The key length of AES256_CTS_HMAC_SHA1_96.
        /// </summary>
        public const int AES_256_KEY_LENGTH = 32;

        /// <summary>
        /// The key length of AES128_CTS_HMAC_SHA1_96.
        /// </summary>
        public const int AES_128_KEY_LENGTH = 16;

        /// <summary>
        /// The key length of RC4.
        /// </summary>
        public const int RC4_KEY_LENGTH = 16;

        /// <summary>
        /// The key length of DES_CBC_CRC and DES_CBC_MD5.
        /// </summary>
        public const int DES_KEY_LENGTH = 8;

        #endregion


        #region ECC parameters

        /// <summary>
        /// Parameter p in p256r1 T = (p, a, b, G, n, h)
        /// Refer to doc [SEC2] http://www.secg.org Section 2.4.2
        /// </summary>
        internal static readonly byte[] ECDH_P256_Prime = 
        {
            0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
        };

        /// <summary>
        /// The parameter A for ECDH_P256
        /// Refer to doc [SEC2] http://www.secg.org Section 2.4.2
        /// </summary>
        internal static readonly byte[] ECDH_P256_A = 
        {
            0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfc
        };

        /// <summary>
        /// Parameter p in p384r1 T = (p, a, b, G, n, h)
        /// Refer to doc [SEC2] http://www.secg.org Section 2.5.2
        /// </summary>
        internal static readonly byte[] ECDH_P384_Prime =
        {
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfe,
            0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xff
        };

        /// <summary>
        /// The parameter A for ECDH_P384
        /// Refer to doc [SEC2] http://www.secg.org Section 2.5.2
        /// </summary>
        internal static readonly byte[] ECDH_P384_A =
        {
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfe,
            0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff, 0xfc
        };

        /// <summary>
        /// Parameter p in p521r1 T = (p, a, b, G, n, h)
        /// Refer to doc [SEC2] http://www.secg.org Section 2.6.2
        /// </summary>
        internal static readonly byte[] ECDH_P521_Prime =
        {
            0x01, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff
        };

        /// <summary>
        /// The parameter A for ECDH_P521
        /// Refer to doc [SEC2] http://www.secg.org Section 2.6.2
        /// </summary>
        internal static readonly byte[] ECDH_P521_A = 
        {
            0x01, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
            0xff, 0xfc
        };

        #endregion
    }
}