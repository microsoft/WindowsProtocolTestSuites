// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// A utility class for crypto
    /// </summary>
    public static class CryptoUtility
    {
        #region Common
        /// <summary>
        /// Convert a byte to its binary presentation in char array
        /// (for example, byte b = 3, which will be presented by 
        /// char array {'0', '0', '0', '0', '0', '0', '1', '1'})
        /// </summary>
        /// <param name="b">the byte</param>
        /// <returns>the byte's binary presentation</returns>
        internal static char[] GetBits(byte b)
        {
            // initialize result array to '0'
            char[] result = new char[ConstValue.BYTE_SIZE] { '0', '0', '0', '0', '0', '0', '0', '0' };

            // get the binary
            char[] binary = Convert.ToString(b, 2).ToCharArray();

            // copy binary to result array
            Array.Copy(binary, 0, result, result.Length - binary.Length, binary.Length);
            return result;
        }


        /// <summary>
        /// Convert a list of binary bits to bytes
        /// (for example, char array {'0', '0', '0', '0', '0', '0', '1', '1'}
        /// will be converted to byte b = 3)
        /// </summary>
        /// <param name="bits">bits represented by chars ('0' and '1')</param>
        /// <returns>the converted byte array</returns>
        internal static byte[] ConvertBitsToBytes(List<char> bits)
        {
            if (null == bits)
            {
                throw new ArgumentNullException("bits");
            }
            if (bits.Count % ConstValue.BYTE_SIZE != 0)
            {
                throw new ArgumentException("Bits length should be a multiply of 8");
            }

            byte[] result = new byte[bits.Count / ConstValue.BYTE_SIZE];
            for (int i = 0; i < result.Length; i++)
            {
                string s = new string(bits.GetRange(i * ConstValue.BYTE_SIZE, ConstValue.BYTE_SIZE).ToArray());
                result[i] = Convert.ToByte(s, 2);
            }
            return result;
        }


        /// <summary>
        /// Create the confounder data with random bytes
        /// </summary>
        /// <param name="confounderSize">expected size of the confounder data</param>
        /// <returns>the confounder data</returns>
        internal static byte[] CreateConfounder(uint confounderSize)
        {
            // fill the confounder buffer with random bytes
            byte[] confounder = new byte[confounderSize];
            Random random = new Random();
            random.NextBytes(confounder);
            return confounder;
        }


        /// <summary>
        /// Add Padding (padding mode = zeros)
        /// </summary>
        /// <param name="input">input data</param>
        /// <param name="blockSize">round up block size</param>
        /// <returns>the padded data</returns>
        internal static byte[] AddPadding(byte[] input, int blockSize)
        {
            // check input
            if (null == input)
            {
                throw new ArgumentNullException("input");
            }

            // check if it needs padding
            int remainLength = input.Length % blockSize;
            if (0 == remainLength)
            {
                return input;
            }

            // add zeros as padding
            int paddingLength = blockSize - remainLength;
            return ArrayUtility.ConcatenateArrays(input, new byte[paddingLength]);
        }
        #endregion Common


        #region Hash Algorithms
        /// <summary>
        /// Compute CRC32 hash for input data
        /// </summary>
        /// <param name="input">input data</param>
        /// <returns>the CRC32 hash data</returns>
        internal static byte[] ComputeCRC32(byte[] input)
        {
            return Crc32.ComputeChecksum(input, false);
        }


        /// <summary>
        /// Compute Hmac-Sha1 hash based on a secret key and input data
        /// </summary>
        /// <param name="key">the secret key</param>
        /// <param name="input">the to be hashed input</param>
        /// <returns>the Hmac-Sha1 hash data</returns>
        internal static byte[] ComputeHmacSha1(byte[] key, byte[] input)
        {
            HMACSHA1 hmacSha1 = new HMACSHA1(key);
            return hmacSha1.ComputeHash(input);
        }


        /// <summary>
        /// Compute MD5 hash for input data
        /// </summary>
        /// <param name="input">input data</param>
        /// <returns>the MD5 hash data</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5350:MD5CannotBeUsed")]
        internal static byte[] ComputeMd5(byte[] input)
        {
            MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
            return md5CryptoServiceProvider.ComputeHash(input);
        }


        /// <summary>
        ///  Compute Md5-Hmac hash based on a secret key and input data
        /// </summary>
        /// <param name="key">the secret key</param>
        /// <param name="input">the to be hashed input</param>
        /// <returns>the Md5-Hmac hash data</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5350:MD5CannotBeUsed")]
        public static byte[] ComputeMd5Hmac(byte[] key, byte[] input)
        {
            HMACMD5 hmacMd5 = new HMACMD5(key);
            return hmacMd5.ComputeHash(input);
        }


        /// <summary>
        /// Compute MD4 hash for input data
        /// </summary>
        /// <param name="input">input data</param>
        /// <returns>the MD4 hash</returns>
        internal static byte[] ComputeMd4(byte[] input)
        {
            MD4 md4 = MD4.Create();
            md4.Initialize();
            return md4.ComputeHash(input);
        }


        /// <summary>
        /// Compute Sha1 hash for input data
        /// </summary>
        /// <param name="input">input data</param>
        /// <returns>the Sha1 hash</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5354:SHA1CannotBeUsed")]
        internal static byte[] ComputeSha1(byte[] input)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            return sha1.ComputeHash(input);
        }
        #endregion Hash Algorithms


        #region DES Crypto Related
        /// <summary>
        /// Create a DES-CBC encryptor
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="initialVector">the initialization vector</param>
        /// <param name="padding">the padding mode</param>
        /// <returns>the DES-CBC encryptor</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5351:DESCannotBeUsed")]
        internal static ICryptoTransform CreateDesCbcEncryptor(
            byte[] key,
            byte[] initialVector,
            PaddingMode padding)
        {
            // check inputs
            if (null == key)
            {
                throw new ArgumentNullException("key");
            }
            if (null == initialVector)
            {
                throw new ArgumentNullException("initialVector");
            }

            // Set crypto to DES-CBC mode
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.CBC;
            des.BlockSize = ConstValue.DES_CBC_BLOCK_SIZE;

            // Set padding mode
            des.Padding = padding;

            // Create encryptor from key and initilize vector
            return des.CreateEncryptor(key, initialVector);
        }


        /// <summary>
        /// Create a DES-CBC decryptor
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="initialVector">the initialization vector</param>
        /// <param name="padding">the padding mode</param>
        /// <returns>the DES-CBC decryptor</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5351:DESCannotBeUsed")]
        internal static ICryptoTransform CreateDesCbcDecryptor(
            byte[] key,
            byte[] initialVector,
            PaddingMode padding)
        {
            // check inputs
            if (null == key)
            {
                throw new ArgumentNullException("key");
            }
            if (null == initialVector)
            {
                throw new ArgumentNullException("initialVector");
            }

            // Set crypto to DES-CBC mode
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.CBC;
            des.BlockSize = ConstValue.DES_CBC_BLOCK_SIZE;

            // Set padding mode
            des.Padding = padding;

            // Create encryptor from key and initilize vector
            return des.CreateDecryptor(key, initialVector);
        }
        #endregion DES Crypto Related


        #region AES Crypto Related
        /// <summary>
        /// Create AES-CTS encryptor/decryptor
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="initialVector">the initialization vector</param>
        /// <returns>the AES-CTS encryptor</returns>
        internal static CipherTextStealingMode CreateAesCtsCrypto(
            byte[] key,
            byte[] initialVector)
        {
            // check inputs
            if (null == key)
            {
                throw new ArgumentNullException("key");
            }
            if (null == initialVector)
            {
                throw new ArgumentNullException("initialVector");
            }

            // initialize AES
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.Key = key;
            aes.IV = initialVector;

            // create AES-CTS encryptor/decryptor
            return new CipherTextStealingMode(aes);
        }
        #endregion AES Crypto Related


        #region RC4 Crypto Related
        /// <summary>
        /// Create a RC4 encryptor
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>the RC4 encryptor</returns>
        internal static ICryptoTransform CreateRc4Encryptor(byte[] key)
        {
            // check input
            if (null == key)
            {
                throw new ArgumentNullException("key");
            }

            // CryptoLib RC4 needs no initial vector
            RC4CryptoServiceProvider rc4 = new RC4CryptoServiceProvider();
            return rc4.CreateEncryptor(key, null);
        }


        /// <summary>
        /// Create a RC4 decryptor
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>the RC4 decryptor</returns>
        internal static ICryptoTransform CreateRc4Decryptor(byte[] key)
        {
            // check input
            if (null == key)
            {
                throw new ArgumentNullException("key");
            }

            // CryptoLib RC4 needs no initial vector
            RC4CryptoServiceProvider rc4 = new RC4CryptoServiceProvider();
            return rc4.CreateDecryptor(key, null);
        }
        #endregion RC4 Crypto Related
    }
}