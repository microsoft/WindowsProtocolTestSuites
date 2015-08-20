// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// AES Key Generator
    /// </summary>
    public static class UnkeyedChecksum
    {
        /// <summary>
        /// Calculate a unkeyed checksum (CRC32, rsa_md4, rsa_md5, sha1)
        /// </summary>
        /// <param name="input">input data</param>
        /// <param name="checksumType">checksum type</param>
        /// <returns>the calculated checksum</returns>
        public static byte[] GetMic(byte[] input, ChecksumType checksumType)
        {
            switch (checksumType)
            {
                case ChecksumType.CRC32:
                    return CryptoUtility.ComputeCRC32(input);
                    
                case ChecksumType.rsa_md4:
                    return CryptoUtility.ComputeMd4(input);
                    
                case ChecksumType.rsa_md5:
                    return CryptoUtility.ComputeMd5(input);
                    
                case ChecksumType.sha1:
                    return CryptoUtility.ComputeSha1(input);

                default:
                    throw new ArgumentException(
                        "Not a valid unkeyed checksum type.");
            }
        }
    }
}