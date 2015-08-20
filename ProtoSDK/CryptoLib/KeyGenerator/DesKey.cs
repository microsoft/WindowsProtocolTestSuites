// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// DES Key Generator
    /// </summary>
    public static class DesKey
    {
        #region Constants
        /// <summary>
        /// Weak key table
        /// (defined in National Bureau of Standards, U.S. Department of Commerce,
        /// "Guidelines for implementing and using NBS Data Encryption Standard,"
        /// Federal Information Processing Standards Publication 74, Washington, DC, 1981)
        /// </summary>
        private static readonly ulong[] weakKeys = new ulong[] {
            0x0101010101010101, 0xFEFEFEFEFEFEFEFE, 0xE0E0E0E0F1F1F1F1, 0x1F1F1F1F0E0E0E0E,
            0x011F011F010E010E, 0x1F011F010E010E01, 0x01E001E001F101F1, 0xE001E001F101F101,
            0x01FE01FE01FE01FE, 0xFE01FE01FE01FE01, 0x1FE01FE00EF10EF1, 0xE01FE01FF10EF10E,
            0x1FFE1FFE0EFE0EFE, 0xFE1FFE1FFE0EFE0E, 0xE0FEE0FEF1FEF1FE, 0xFEE0FEE0FEF1FEF1
        };
        #endregion Constants


        #region Private Methods
        /// <summary>
        /// Remove the MSB (Most Significant Bit) in each octet
        /// (in big endian mode) and concatenates the result
        /// [RFC 3961, Section 6.2, removeMSBits()]
        /// </summary>
        /// <param name="inputData">input 8 bytes</param>
        /// <returns>output 7 bytes</returns>
        private static byte[] RemoveMSBits(byte[] inputData)
        {
            // check input
            if (null == inputData || ConstValue.DES_BLOCK_SIZE != inputData.Length)
            {
                throw new ArgumentException("The input data must contain exactly 8 bytes.");
            }

            // remove the most significant bit from each byte
            List<char> newBits = new List<char>();
            foreach (byte b in inputData)
            {
                List<char> temp = new List<char>(CryptoUtility.GetBits(b));
                temp.RemoveAt(0);
                newBits.AddRange(temp);
            }

            // parse the 56 bits to 7 bytes
            return CryptoUtility.ConvertBitsToBytes(newBits);
        }


        /// <summary>
        /// Treat a 56-bit block as a binary string and reverse it
        /// [RFC 3961, Section 6.2, reverse(56bitblock)]
        /// </summary>
        /// <param name="inputData">input data to be reversed</param>
        /// <returns>the reversed data</returns>
        private static byte[] Reverse(byte[] inputData)
        {
            // Check input
            if (null == inputData || 7 != inputData.Length)
            {
                throw new ArgumentException("The inputData should be a 56 bits value.");
            }

            // Get all bits
            List<char> allBits = new List<char>();
            foreach (byte b in inputData)
            {
                allBits.AddRange(CryptoUtility.GetBits(b));
            }

            // Reverse
            allBits.Reverse();

            // Convert bits to bytes
            return CryptoUtility.ConvertBitsToBytes(allBits);
        }


        /// <summary>
        /// Add DES Parity Bits
        /// (Copies a 56-bit block into a 64-bit block, 
        /// left shifts content in each octet, and add DES parity bit)
        /// [RFC 3961, Section 6.2, add_parity_bits(56bitblock)]
        /// </summary>
        /// <param name="inputData">the input 56-bit data</param>
        /// <returns>the parity-added 64-bit data</returns>
        private static byte[] AddParityBits(byte[] inputData)
        {
            // check input
            if (null == inputData || 7 != inputData.Length)
            {
                throw new ArgumentException("The inputData should be a 56 bits value.");
            }

            // get all bits
            List<char> allBits = new List<char>();
            foreach (byte b in inputData)
            {
                allBits.AddRange(CryptoUtility.GetBits(b));
            }

            // insert parity bits
            List<char> newBits = new List<char>();
            for (int i = 0; i < ConstValue.DES_BLOCK_SIZE; i++)
            {
                // get 7 bits
                List<char> temp = allBits.GetRange(7 * i, 7);
                
                // count the number of ones
                bool even = true;
                foreach (char bit in temp)
                {
                    if (bit == '1')
                    {
                        even = !even;
                    }
                }

                // if the number of 1 in an octet is even, the least significant bit will be 1
                temp.Add(even ? '1' : '0');
                newBits.AddRange(temp);
            }

            // convert to bytes
            return CryptoUtility.ConvertBitsToBytes(newBits);
        }


        /// <summary>
        /// Fix parity bits in input data
        /// </summary>
        /// <param name="inputData">input data</param>
        /// <returns>parity-fixed data</returns>
        private static byte[] FixParity(byte[] inputData)
        {
            List<char> newBits = new List<char>();
            for (int i = 0; i < inputData.Length; i++)
            {
                char[] bits = CryptoUtility.GetBits(inputData[i]);

                // check the first 7 bits
                bool even = true;
                for (int j = 0; j < bits.Length - 1; j++)
                {
                    if (bits[j] == '1')
                    {
                        even = !even;
                    }
                }

                // Reset the last bit
                bits[bits.Length - 1] = even ? '1' : '0';
                newBits.AddRange(bits);
            }
            return CryptoUtility.ConvertBitsToBytes(newBits);
        }


        /// <summary>
        /// The key is corrected when the parity is fixed and 
        /// assure the key is not "weak key" or "semi-weak key"
        /// [RFC 3961, Section 6.2, key_correction(key))
        /// </summary>
        /// <param name="key">input key data</param>
        /// <returns>the corrected key data</returns>
        private static byte[] KeyCorrection(byte[] key)
        {
            // fix parity
            byte[] newKey = FixParity(key);

            // convert to little endian
            Array.Reverse(newKey);
            ulong weakKeyTest = BitConverter.ToUInt64(newKey, 0);

            // Recovery the order
            Array.Reverse(newKey);

            // if it is weak key or semi-weak key, correct it
            List<ulong> weakKeyList = new List<ulong>(weakKeys);
            if (weakKeyList.Contains(weakKeyTest))
            {
                // XOR with 0x00000000000000F0
                newKey[7] ^= 0xF0;
            }
            return newKey;
        }


        /// <summary>
        /// Generate DES key from specified string and salt
        /// [RFC 3961, Section 6.2, mit_des_string_to_key(string, salt)]
        /// </summary>
        /// <param name="password">password in UTF-8</param>
        /// <param name="salt">salt in UTF-8</param>
        /// <returns>the generated DES key (8 bytes)</returns>
        private static byte[] MitDesStringToKey(string password, string salt)
        {
            // check input
            if (null == password)
            {
                throw new ArgumentNullException("password");
            }
            if (null == salt)
            {
                throw new ArgumentNullException("salt");
            }

            // initialize input buffer
            List<byte> inputBytes = new List<byte>();
            inputBytes.AddRange(Encoding.UTF8.GetBytes(password));
            inputBytes.AddRange(Encoding.UTF8.GetBytes(salt));

            //Add padding to 8 byte boundary
            int inputLength = inputBytes.Count + (ConstValue.DES_BLOCK_SIZE - inputBytes.Count % 
                ConstValue.DES_BLOCK_SIZE) % ConstValue.DES_BLOCK_SIZE;

            byte[] input = new byte[inputLength];
            Array.Copy(inputBytes.ToArray(), 0, input, 0, inputBytes.Count);

            // initialize temporary buffers
            byte[] blockBuffer = new byte[ConstValue.DES_BLOCK_SIZE];
            byte[] sevenBytesBuffer = new byte[7];
            byte[] fanFoldBuffer = new byte[7];

            // fan-fold padded value
            bool odd = true;
            for (int i = 0; i < input.Length; i += ConstValue.BYTE_SIZE)
            {
                // get a new block
                Array.Copy(input, i, blockBuffer, 0, blockBuffer.Length);
       
                // remove most significant bits
                sevenBytesBuffer = RemoveMSBits(blockBuffer);
                
                // do reverse
                if (!odd)
                {
                    sevenBytesBuffer = Reverse(sevenBytesBuffer);
                }
                odd = !odd;

                // do fan-fold
                for (int j = 0; j < fanFoldBuffer.Length; j++)
                {
                    fanFoldBuffer[j] ^= sevenBytesBuffer[j];
                }
            }

            // convert to a 64-bit intermediate key
            byte[] intermediateKey = KeyCorrection(AddParityBits(fanFoldBuffer));

            // encryption
            // (DES key is generated from intermediate key, the IV also uses the intermediate key)
            ICryptoTransform encryptor =
                CryptoUtility.CreateDesCbcEncryptor(intermediateKey, intermediateKey, PaddingMode.Zeros);
            byte[] result = encryptor.TransformFinalBlock(input, 0, input.Length);
            if (result.Length < 2 * ConstValue.DES_BLOCK_SIZE)
            {
                throw new FormatException("DES CBC Encryption Error.");
            }

            // DES key is the key-corrected last block of the encryption value
            byte[] lastBlock = new byte[ConstValue.DES_BLOCK_SIZE];
            Array.Copy(result, result.Length - ConstValue.DES_BLOCK_SIZE, lastBlock, 0, lastBlock.Length);
            return KeyCorrection(lastBlock);
        }
        #endregion Private Methods


        #region Internal Methods
        /// <summary>
        /// Generate an encryption key from password and salt
        /// [RFC 3961, Section 6.2, des_string_to_key(string,salt,params)]
        /// </summary>
        /// <param name="password">password</param>
        /// <param name="salt">salt</param>
        /// <returns>the encrypted key in bytes</returns>
        internal static byte[] MakeStringToKey(string password, string salt)
        {
            return MitDesStringToKey(password, salt);
        }
        #endregion Internal Methods


        #region Public Methods
        /// <summary>
        /// Calculate the des key.
        /// [RFC 3961, Section 6.2, des_random_to_key(bitstring)]
        /// </summary>
        /// <param name="bits">A 56 bits string</param>
        /// <returns>The des key</returns>
        /// <exception cref="System.ArgumentException">Thrown when the input data is null or the length is not 7.
        /// </exception>
        public static byte[] DesRandomToKey(byte[] bits)
        {
            return KeyCorrection(AddParityBits(bits));
        }
        #endregion Public Methods
    }
}