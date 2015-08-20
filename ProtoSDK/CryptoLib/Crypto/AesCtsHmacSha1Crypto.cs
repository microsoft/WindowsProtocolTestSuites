// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// Crypto Class for aes128-cts-hmac-sha1-96 and aes256-cts-hmac-sha1-96
    /// </summary>
    public static class AesCtsHmacSha1Crypto
    {
        #region Internal Methods
        /// <summary>
        /// Encrypt in aes128-cts-hmac-sha1-96 or aes256-cts-hmac-sha1-96
        /// </summary>
        /// <param name="key">key data</param>
        /// <param name="plain">plain data to be encrypted</param>
        /// <param name="usage">key usage number</param>
        /// <param name="aesKeyType">aes key type (128bit/256bit)</param>
        /// <returns>the encrypted data</returns>
        public static byte[] Encrypt(
            byte[] key,
            byte[] plain,
            int usage,
            AesKeyType aesKeyType)
        {
            return Encrypt(key, plain, usage, aesKeyType, null);
        }
        

        /// <summary>
        /// Encrypt in aes128-cts-hmac-sha1-96 or aes256-cts-hmac-sha1-96
        /// </summary>
        /// <param name="key">key data</param>
        /// <param name="plain">plain data to be encrypted</param>
        /// <param name="usage">key usage number</param>
        /// <param name="aesKeyType">aes key type (128bit/256bit)</param>
        /// <param name="getToBeSignedDateCallback">
        /// A callback to get to-be-signed data. 
        /// The method will use plain-text data directly if this parameter is null.
        /// </param>
        /// <returns>the encrypted data</returns>
        public static byte[] Encrypt(
            byte[] key,
            byte[] plain,
            int usage,
            AesKeyType aesKeyType,
            GetToBeSignedDataFunc getToBeSignedDateCallback)
        {
            // check inputs
            if (null == key)
            {
                throw new ArgumentNullException("key");
            }
            if (null == plain)
            {
                throw new ArgumentNullException("plain");
            }

            // add confounder data
            byte[] plainData = ArrayUtility.ConcatenateArrays(
                CryptoUtility.CreateConfounder(ConstValue.AES_BLOCK_SIZE),
                plain);

            // use ke key (the encryption key) to encrypt the plain data
            byte[] ke = AesKey.MakeDerivedKey(key, usage, DerivedKeyType.Ke, aesKeyType);
            byte[] initialVector = new byte[ConstValue.AES_BLOCK_SIZE];
            CipherTextStealingMode aesCtsCrypto = CryptoUtility.CreateAesCtsCrypto(ke, initialVector);
            byte[] encryptedData = aesCtsCrypto.EncryptFinal(plainData, 0, plainData.Length);

            byte[] toBeSignedData;
            if (getToBeSignedDateCallback != null)
            {
                toBeSignedData = getToBeSignedDateCallback(plainData);
            }
            else
            {
                toBeSignedData = plainData;
            }

            // use ki key (the integrity key) to generate checksum
            byte[] ki = AesKey.MakeDerivedKey(key, usage, DerivedKeyType.Ki, aesKeyType);
            byte[] hmacData = CryptoUtility.ComputeHmacSha1(ki, toBeSignedData);
            hmacData = ArrayUtility.SubArray<byte>(hmacData, 0, ConstValue.HMAC_HASH_OUTPUT_SIZE);

            // result: encryptedData + hmacData
            return ArrayUtility.ConcatenateArrays(encryptedData, hmacData);
        }


        /// <summary>
        /// Decrypt in aes128-cts-hmac-sha1-96 or aes256-cts-hmac-sha1-96
        /// </summary>
        /// <param name="key">key data</param>
        /// <param name="cipher">cipher data to be decrypted</param>
        /// <param name="usage">key usage number</param>
        /// <param name="aesKeyType">aes key type (128bit/256bit)</param>
        /// <returns>the decrypted data</returns>
        public static byte[] Decrypt(
            byte[] key,
            byte[] cipher,
            int usage,
            AesKeyType aesKeyType)
        {
            return Decrypt(key, cipher, usage, aesKeyType, null);
        }


        /// <summary>
        /// Decrypt in aes128-cts-hmac-sha1-96 or aes256-cts-hmac-sha1-96
        /// </summary>
        /// <param name="key">key data</param>
        /// <param name="cipher">cipher data to be decrypted</param>
        /// <param name="usage">key usage number</param>
        /// <param name="aesKeyType">aes key type (128bit/256bit)</param>
        /// <param name="getToBeSignedDateCallback">
        /// A callback to get to-be-signed data. 
        /// The method will use decrypted data directly if this parameter is null.
        /// </param>
        /// <returns>the decrypted data</returns>
        public static byte[] Decrypt(
            byte[] key,
            byte[] cipher,
            int usage,
            AesKeyType aesKeyType,
            GetToBeSignedDataFunc getToBeSignedDateCallback)
        {
            // check inputs
            if (null == key)
            {
                throw new ArgumentNullException("key");
            }
            if (null == cipher)
            {
                throw new ArgumentNullException("cipher");
            }

            // the cipher has two parts: encrypted(confounder + plain) + hmac
            byte[] encryptedData = ArrayUtility.SubArray<byte>(
                cipher, 0, cipher.Length - ConstValue.HMAC_HASH_OUTPUT_SIZE);
            byte[] hmacData = ArrayUtility.SubArray<byte>(
                cipher, cipher.Length - ConstValue.HMAC_HASH_OUTPUT_SIZE);

            // use ke key (the encryption key) to decrypt
            byte[] ke = AesKey.MakeDerivedKey(key, usage, DerivedKeyType.Ke, aesKeyType);
            byte[] initialVector = new byte[ConstValue.AES_BLOCK_SIZE];
            CipherTextStealingMode aesCtsCrypto = CryptoUtility.CreateAesCtsCrypto(ke, initialVector);
            byte[] decryptedData = aesCtsCrypto.DecryptFinal(encryptedData, 0, encryptedData.Length);

            byte[] toBeSignedData;
            if (getToBeSignedDateCallback != null)
            {
                toBeSignedData = getToBeSignedDateCallback(decryptedData);
            }
            else
            {
                toBeSignedData = decryptedData;
            }

            // use ki key (the integrity key) to verify hmac data
            byte[] ki = AesKey.MakeDerivedKey(key, usage, DerivedKeyType.Ki, aesKeyType);
            byte[] expectedHmacData = CryptoUtility.ComputeHmacSha1(ki, toBeSignedData);
            expectedHmacData = ArrayUtility.SubArray<byte>(expectedHmacData, 0, ConstValue.HMAC_HASH_OUTPUT_SIZE);
            if (!ArrayUtility.CompareArrays<byte>(hmacData, expectedHmacData))
            {
                throw new FormatException(
                    "Decryption: failed integrity check in hmac checksum.");
            }

            // remove confounder data
            decryptedData = ArrayUtility.SubArray<byte>(decryptedData, ConstValue.AES_BLOCK_SIZE, 
                decryptedData.Length - ConstValue.AES_BLOCK_SIZE);
            return decryptedData;
        }
        #endregion Internal Methods
    }
}