// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// An implementation of Desl algorithm
    /// </summary>
    public static class Desl
    {
        // the key length used by Des with ECB mode
        private const int ecbModeDesKeyLength = 8;

        // the desl key length
        private const int deslKeyLength = 16;

        // the length of three 7-bytes key.
        private const int paddedKeyLength = 21;

        /// <summary>
        /// Encrypt message with the specified key
        /// </summary>
        /// <param name="key">the encrypt key</param>
        /// <param name="data">the data to be encrypted</param>
        /// <returns>the encrypted data in byte array</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5351:DESCannotBeUsed")]
        public static byte[] Encrypt(byte[] key, byte[] data)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (key.Length != deslKeyLength)
            {
                throw new ArgumentException("length of key should be 128 bits", "key");
            }

            byte[] paddedKey = new byte[paddedKeyLength];
            Array.Copy(key, paddedKey, key.Length);
            //generate 8-bytes key using original 7-bytes key by insert 0 ervery 7bits.
            //use 0-6 bytes in the paddedKey
            byte[] key1 = LMHashManaged.GenerateDesKey(paddedKey, 0);
            //use 7-13 bytes in the paddedKey
            byte[] key2 = LMHashManaged.GenerateDesKey(paddedKey, 7);
            //use 14-20 bytes in the paddedKey
            byte[] key3 = LMHashManaged.GenerateDesKey(paddedKey, 14);

            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Mode = CipherMode.ECB;
                ICryptoTransform encryptor1 = des.CreateEncryptor(key1, des.IV);
                ICryptoTransform encryptor2 = des.CreateEncryptor(key2, des.IV);
                ICryptoTransform encryptor3 = des.CreateEncryptor(key3, des.IV);

                byte[] result1 = encryptor1.TransformFinalBlock(data, 0, data.Length);
                byte[] result2 = encryptor2.TransformFinalBlock(data, 0, data.Length);
                byte[] result3 = encryptor3.TransformFinalBlock(data, 0, data.Length);

                return Helper.ConcatenateByteArrays(result1, result2, result3);
            }
        }
    }
}
