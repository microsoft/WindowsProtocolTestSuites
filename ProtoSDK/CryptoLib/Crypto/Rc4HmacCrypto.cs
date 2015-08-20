// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// Crypto Class for rc4-hmac and rc4-hmac-exp
    /// </summary>
    public static class Rc4HmacCrypto
    {
        #region Private Methods
        /// <summary>
        /// Calculate salt from key usage number
        /// </summary>
        /// <param name="usage">key usage number</param>
        /// <param name="encryptionType">encryption type</param>
        /// <returns>the caculated salt</returns>
        private static byte[] GetSalt(int usage, EncryptionType encryptionType)
        {
            byte[] keyUsageBytes = BitConverter.GetBytes((int)usage);
            if (encryptionType == EncryptionType.RC4_HMAC)
            {
                return keyUsageBytes;
            }
            else if (encryptionType == EncryptionType.RC4_HMAC_EXP)
            {
                byte[] constantBytes = Encoding.ASCII.GetBytes(ConstValue.FORTY_BITS);
                return ArrayUtility.ConcatenateArrays(constantBytes, keyUsageBytes);
            }
            else
            {
                throw new ArgumentException("Unsupported encryption type.");
            }
        }


        /// <summary>
        /// Modify a Md5-Hmac hash to support RC4_HMAC_EXP
        /// </summary>
        /// <param name="hash">hash data</param>
        /// <param name="encryptionType">encryption type</param>
        /// <returns>the modified hash data</returns>
        private static byte[] GetExpHash(byte[] hash, EncryptionType encryptionType)
        {
            // check inputs
            if (null == hash)
            {
                throw new ArgumentNullException("hash");
            }
            if (hash.Length != ConstValue.MD5_CHECKSUM_SIZE)
            {
                throw new ArgumentException(
                    "Input hash size should equal to MD5_CHECKSUM_SIZE(16 bytes).");
            }

            // [RFC 4757, Section 5]
            // "if (export) memset (K1+7, 0xAB, 9);"
            byte[] modifiedHash = (byte[])hash.Clone();
            if (encryptionType == EncryptionType.RC4_HMAC_EXP)
            {
                for (int i = 7; i < ConstValue.MD5_CHECKSUM_SIZE; i++)
                {
                    modifiedHash[i] = 0xAB;
                }
            }
            return modifiedHash;
        }
        #endregion Private Methods


        #region Internal Methods
        /// <summary>
        /// RC4-HMAC / RC4-HMAC-EXP encryption
        /// </summary>
        /// <param name="key">the secret key</param>
        /// <param name="plain">the to be encrypted plain data</param>
        /// <param name="usage">key usage number</param>
        /// <param name="encryptionType">encryption type</param>
        /// <returns>the encrypted data</returns>
        public static byte[] Encrypt(
            byte[] key,
            byte[] plain,
            int usage,
            EncryptionType encryptionType)
        {
            return Encrypt(key, plain, usage, encryptionType, null);
        }


        /// <summary>
        /// RC4-HMAC / RC4-HMAC-EXP encryption
        /// </summary>
        /// <param name="key">the secret key</param>
        /// <param name="plain">the to be encrypted plain data</param>
        /// <param name="usage">key usage number</param>
        /// <param name="encryptionType">encryption type</param>
        /// <param name="getToBeSignedDateCallback">
        /// A callback to get to-be-signed data. 
        /// The method will use plain-text data directly if this parameter is null.
        /// </param>
        /// <returns>the encrypted data</returns>
        public static byte[] Encrypt(
            byte[] key,
            byte[] plain,
            int usage,
            EncryptionType encryptionType,
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

            // add confounder
            byte[] confounderData = CryptoUtility.CreateConfounder(ConstValue.CONFOUNDER_SIZE);
            byte[] plainData = ArrayUtility.ConcatenateArrays(confounderData, plain);

            // add padding
            plainData = CryptoUtility.AddPadding(plainData, ConstValue.RC4_BLOCK_SIZE);

            // get salt and hash
            byte[] salt = GetSalt(usage, encryptionType);
            byte[] hash = CryptoUtility.ComputeMd5Hmac(key, salt);
            byte[] hashExp = GetExpHash(hash, encryptionType);

            byte[] toBeSignedData;
            if (getToBeSignedDateCallback != null)
            {
                toBeSignedData = getToBeSignedDateCallback(plainData);
            }
            else
            {
                toBeSignedData = plainData;
            }
            
            // get checksum
            byte[] checksum = CryptoUtility.ComputeMd5Hmac(hash, toBeSignedData);

            // get rc4 encryption key
            byte[] rc4Key = CryptoUtility.ComputeMd5Hmac(hashExp, checksum);

            // encrypt
            ICryptoTransform encryptor = CryptoUtility.CreateRc4Encryptor(rc4Key);
            byte[] encryptedData = encryptor.TransformFinalBlock(plainData, 0, plainData.Length);

            // result = checksum + encryptedData
            return ArrayUtility.ConcatenateArrays(checksum, encryptedData);
        }


        /// <summary>
        /// RC4-HMAC / RC4-HMAC-EXP decryption
        /// </summary>
        /// <param name="key">the secret key</param>
        /// <param name="cipher">the encrypted cipher data</param>
        /// <param name="usage">key usage number</param>
        /// <param name="encryptionType">encryption type</param>
        /// <returns>the decrypted data</returns>
        public static byte[] Decrypt(
            byte[] key,
            byte[] cipher,
            int usage,
            EncryptionType encryptionType)
        {
            return Decrypt(key, cipher, usage, encryptionType, null);
        }


        /// <summary>
        /// RC4-HMAC / RC4-HMAC-EXP decryption
        /// </summary>
        /// <param name="key">the secret key</param>
        /// <param name="cipher">the encrypted cipher data</param>
        /// <param name="usage">key usage number</param>
        /// <param name="encryptionType">encryption type</param>
        /// <param name="getToBeSignedDateCallback">
        /// A callback to get to-be-signed data. 
        /// The method will use decrypted data directly if this parameter is null.
        /// </param>
        /// <returns>the decrypted data</returns>
        public static byte[] Decrypt(
            byte[] key,
            byte[] cipher,
            int usage,
            EncryptionType encryptionType,
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

            // get checksum and encrypted data
            byte[] checksum = ArrayUtility.SubArray(cipher, 0, ConstValue.MD5_CHECKSUM_SIZE);
            byte[] encryptedData = ArrayUtility.SubArray(cipher, ConstValue.MD5_CHECKSUM_SIZE);

            // get salt and hash
            byte[] salt = GetSalt(usage, encryptionType);
            byte[] hash = CryptoUtility.ComputeMd5Hmac(key, salt);
            byte[] hashExp = GetExpHash(hash, encryptionType);

            // get rc4 decryption key
            byte[] rc4Key = CryptoUtility.ComputeMd5Hmac(hashExp, checksum);

            // decrypt
            ICryptoTransform decryptor = CryptoUtility.CreateRc4Decryptor(rc4Key);
            byte[] plain = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

            byte[] toBeSignedData;
            if (getToBeSignedDateCallback != null)
            {
                toBeSignedData = getToBeSignedDateCallback(plain);
            }
            else
            {
                toBeSignedData = plain;
            }

            // verify checksum
            byte[] expectedChecksum = CryptoUtility.ComputeMd5Hmac(hash, toBeSignedData);
            if (!ArrayUtility.CompareArrays(checksum, expectedChecksum))
            {
                throw new FormatException("Decryption: invalid checksum.");
            }

            // remove confounder
            return ArrayUtility.SubArray(plain, ConstValue.CONFOUNDER_SIZE);
        }
        #endregion Internal Methods
    }
}