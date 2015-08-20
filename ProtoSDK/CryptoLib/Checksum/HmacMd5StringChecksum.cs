// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// Hmac-Md5-String Checksum
    /// </summary>
    public static class HmacMd5StringChecksum
    {
        /// <summary>
        /// Get Hmac-Md5-String Checksum
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="input">input data</param>
        /// <param name="usage">key usage number</param>
        /// <returns>the caculated checksum</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5350:MD5CannotBeUsed")]
        public static byte[] GetMic(
            byte[] key,
            byte[] input,
            int usage)
        {
            // get sign key
            byte[] signatureData = Encoding.ASCII.GetBytes(ConstValue.SIGNATURE_KEY);
            HMACMD5 hmacMd5 = new HMACMD5(key);
            byte[] signKey = hmacMd5.ComputeHash(signatureData);
            hmacMd5.Key = signKey;

            // toBeHashedData = keyUsageData + inputData
            byte[] usageData = BitConverter.GetBytes(usage);
            byte[] toBeHashedData = ArrayUtility.ConcatenateArrays(usageData, input);

            // hash result
            byte[] md5Hash = CryptoUtility.ComputeMd5(toBeHashedData);
            return hmacMd5.ComputeHash(md5Hash);
        }
    }
}