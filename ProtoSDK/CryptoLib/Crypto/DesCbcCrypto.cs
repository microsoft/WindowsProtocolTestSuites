// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// Crypto Class for des-cbc-crc32 and des-cbc-md5
    /// </summary>
    public static class DesCbcCrypto
    {
        #region Private Methods
        /// <summary>
        /// Get checksum size by encryption type
        /// </summary>
        /// <param name="encryptionType">encryption type</param>
        /// <returns>checksum type</returns>
        private static int GetChecksumSize(EncryptionType encryptionType)
        {
            if (encryptionType == EncryptionType.DES_CBC_CRC)
            {
                return ConstValue.CRC32_CHECKSUM_SIZE;
            }
            else if (encryptionType == EncryptionType.DES_CBC_MD5)
            {
                return ConstValue.MD5_CHECKSUM_SIZE;
            }
            else
            {
                throw new ArgumentException("Unsupported encryption type.");
            }
        }


        /// <summary>
        /// Calculate checksum according to encryption type
        /// </summary>
        /// <param name="input">input data</param>
        /// <param name="encryptionType">encryption type</param>
        /// <returns>the calculated checksum</returns>
        private static byte[] CalculateChecksum(byte[] input, EncryptionType encryptionType)
        {
            if (encryptionType == EncryptionType.DES_CBC_CRC)
            {
                return CryptoUtility.ComputeCRC32(input);
            }
            else if (encryptionType == EncryptionType.DES_CBC_MD5)
            {
                return CryptoUtility.ComputeMd5(input);
            }
            else
            {
                throw new ArgumentException("Unsupported encryption type.");
            }
        }


        /// <summary>
        /// Get the initial vector for DES-CBC crypto
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="encryptionType">encryption type</param>
        /// <returns>the initial vector</returns>
        private static byte[] GetInitialVector(byte[] key, EncryptionType encryptionType)
        {
            if (encryptionType == EncryptionType.DES_CBC_CRC)
            {
                // [RFC 3961, Section 6.2.3]
                // "The des-cbc-crc encryption type uses DES in CBC mode 
                // with the key used as the initialization vector."
                return key;
            }
            else if (encryptionType == EncryptionType.DES_CBC_MD5)
            {
                // [RFC 3961, Section 6.2.1]
                // "The des-cbc-md5 encryption mode encrypts information under 
                // DES in CBC mode with an all-zero initial vector."
                return new byte[ConstValue.DES_BLOCK_SIZE];
            }
            else
            {
                throw new ArgumentException("Unsupported encryption type."); 
            }
        }
        #endregion Private Methods


        #region Internal Methods
        /// <summary>
        /// DES-CBC-MD5 / DES-CBC-CRC32 encryption
        /// [RFC 3961, Section 6.2, DES-Based Encryption and Checksum Types]
        ///  "One generates a random confounder of one block, placing it in 'confounder'; 
        ///  zeros out the 'checksum' field (of length appropriate to exactly hold the checksum
        ///  to be computed); adds the necessary padding; calculates the appropriate checksum 
        ///  over the whole sequence, placing the result in 'checksum'; and then encrypts
        ///  using the specified encryption type and the appropriate key."
        /// </summary>
        /// <param name="key">the secret key</param>
        /// <param name="plain">the to be encrypted plain data</param>
        /// <param name="encryptionType">encryption type</param>
        /// <returns>the encrypted data</returns>
        public static byte[] Encrypt(
            byte[] key,
            byte[] plain,
            EncryptionType encryptionType)
        {
            return Encrypt(key, plain, encryptionType, null);
        }


        /// <summary>
        /// DES-CBC-MD5 / DES-CBC-CRC32 encryption
        /// [RFC 3961, Section 6.2, DES-Based Encryption and Checksum Types]
        ///  "One generates a random confounder of one block, placing it in 'confounder'; 
        ///  zeros out the 'checksum' field (of length appropriate to exactly hold the checksum
        ///  to be computed); adds the necessary padding; calculates the appropriate checksum 
        ///  over the whole sequence, placing the result in 'checksum'; and then encrypts
        ///  using the specified encryption type and the appropriate key."
        /// </summary>
        /// <param name="key">the secret key</param>
        /// <param name="plain">the to be encrypted plain data</param>
        /// <param name="encryptionType">encryption type</param>
        /// <param name="getToBeSignedDateCallback">
        /// A callback to get to-be-signed data. 
        /// The method will use plain-text data directly if this parameter is null.
        /// </param>
        /// <returns>the encrypted data</returns>
        public static byte[] Encrypt(
            byte[] key,
            byte[] plain,
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
            
            // toBePaddedData = confounderData + emptyChecksumData + plainData
            int checksumSize = GetChecksumSize(encryptionType);
            byte[] toBeEncryptedData = ArrayUtility.ConcatenateArrays(
                CryptoUtility.CreateConfounder(ConstValue.DES_BLOCK_SIZE),
                new byte[checksumSize],
                plain);

            // add padding
            byte[] paddedData = CryptoUtility.AddPadding(toBeEncryptedData, ConstValue.DES_BLOCK_SIZE);

            byte[] toBeSignedData;
            if (getToBeSignedDateCallback != null)
            {
                toBeSignedData = getToBeSignedDateCallback(paddedData);
            }
            else
            {
                toBeSignedData = paddedData;
            }

            // replace the empty checksum with the caculated checksum
            byte[] checksum = CalculateChecksum(toBeSignedData, encryptionType);
            Array.Copy(checksum, 0, paddedData, ConstValue.DES_BLOCK_SIZE, checksum.Length);

            // des-cbc encryption
            byte[] initialVector = GetInitialVector(key, encryptionType);
            ICryptoTransform encryptor = CryptoUtility.CreateDesCbcEncryptor(key, initialVector, PaddingMode.None);
            
            return encryptor.TransformFinalBlock(paddedData, 0, paddedData.Length);
        }


        /// <summary>
        /// DES-CBC-MD5 / DES-CBC-CRC32 decryption
        /// </summary>
        /// <param name="key">the secret key</param>
        /// <param name="cipher">the encrypted cipher data</param>
        /// <param name="encryptionType">encryption type</param>
        /// <returns>the decrypted data</returns>
        public static byte[] Decrypt(
            byte[] key,
            byte[] cipher,
            EncryptionType encryptionType)
        {
            return Decrypt(key, cipher, encryptionType, null);
        }


        /// <summary>
        /// DES-CBC-MD5 / DES-CBC-CRC32 decryption
        /// </summary>
        /// <param name="key">the secret key</param>
        /// <param name="cipher">the encrypted cipher data</param>
        /// <param name="encryptionType">encryption type</param>
        /// <param name="getToBeSignedDateCallback">
        /// A callback to get to-be-signed data. 
        /// The method will use decrypted data directly if this parameter is null.
        /// </param>
        /// <returns>the decrypted data</returns>
        public static byte[] Decrypt(
            byte[] key,
            byte[] cipher,
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

            // des-cbc decryption
            byte[] initialVector = GetInitialVector(key, encryptionType);
            ICryptoTransform decryptor = CryptoUtility.CreateDesCbcDecryptor(key, initialVector, PaddingMode.None);
            byte[] decryptedData = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);

            // get checksum, set checksum field to zeros
            int checksumSize = GetChecksumSize(encryptionType);
            byte[] checksum = ArrayUtility.SubArray<byte>(decryptedData,
                    ConstValue.DES_BLOCK_SIZE, checksumSize);
            Array.Clear(decryptedData, ConstValue.DES_BLOCK_SIZE, checksumSize);

            byte[] toBeSignedData;
            if (getToBeSignedDateCallback != null)
            {
                toBeSignedData = getToBeSignedDateCallback(decryptedData);
            }
            else
            {
                toBeSignedData = decryptedData;
            }

            // verify checksum
            byte[] expectedChecksum = CalculateChecksum(toBeSignedData, encryptionType);
            if (!ArrayUtility.CompareArrays<byte>(checksum, expectedChecksum))
            {
                throw new FormatException(
                    "Decryption: inconsistent checksum.");
            }

            // remove confounder and checksum
            decryptedData = ArrayUtility.SubArray<byte>(decryptedData, ConstValue.DES_BLOCK_SIZE + checksumSize);
            
            return decryptedData;
        }
        #endregion Internal Methods
    }
}