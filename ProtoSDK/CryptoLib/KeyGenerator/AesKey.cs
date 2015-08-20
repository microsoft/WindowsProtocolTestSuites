// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// An enum to indicate the AES key size in bits.
    /// </summary>
    public enum AesKeyType : int
    {
        /// <summary>
        /// AES key type not specified
        /// </summary>
        None = 0,

        /// <summary>
        /// 128 bits AES key
        /// </summary>
        Aes128BitsKey = 128,

        /// <summary>
        /// 192 bits AES key
        /// </summary>
        Aes192BitsKey = 192,

        /// <summary>
        /// 256 bits AES key
        /// </summary>
        Aes256BitsKey = 256
    }


    /// <summary>
    /// Derived Key Types (RFC3961)
    /// </summary>
    public enum DerivedKeyType : int
    {
        /// <summary>
        /// An added value used as the default value.
        /// </summary>
        None = 0,

        /// <summary>
        /// Used to derive key to generate mic in Checksum mechanism.
        /// </summary>
        Kc = 0x99,

        /// <summary>
        /// Used to derive key to encrypt data.
        /// </summary>
        Ke = 0xAA,

        /// <summary>
        /// Used to derive key to calculate checksum in Encryption mechanism.
        /// </summary>
        Ki = 0x55
    }


    /// <summary>
    /// AES Key Generator
    /// </summary>
    public static class AesKey
    {
        #region Constants
        /// <summary>
        /// Default Interation Count
        /// [RFC3962 Section 4, Page 2]
        /// </summary>
        internal const uint DEFAULT_ITERATION_COUNT = 4096;


        /// <summary>
        /// ASCII encoding for the string "Kerberos"
        /// </summary>
        private readonly static byte[] KERBEROS_CONSTANT = new byte[] { 0x6b, 0x65, 0x72, 0x62, 0x65, 0x72, 0x6f, 0x73 };
        #endregion Constants


        #region Private Methods
        /// <summary>
        /// DK is the key-derivation function described in RFC 3961
        /// [RFC 3961 section 5.1 A Key Derivation Function]
        /// </summary>
        /// <param name="baseKey">the base key</param>
        /// <param name="wellKnownConstant">the "well-known constant"</param>
        /// <param name="aesKeyType">AES key type which decides key size</param>
        /// <returns>the derived key in bytes</returns>
        public static byte[] DK(
            byte[] baseKey, 
            byte[] wellKnownConstant, 
            AesKeyType aesKeyType)
        {
            // caculate DR value
            byte[] drBytes = DR(baseKey, wellKnownConstant, aesKeyType);

            // caculate Random
            return RandomToKey(drBytes);
        }


        /// <summary>
        /// DR is the random-octet generation function described in RFC 3961
        /// [RFC 3961 section 5.1 A Key Derivation Function]
        /// </summary>
        /// <param name="baseKey">the base key which is to be derived from</param>
        /// <param name="wellKnownConstant">the "well-known constant"</param>
        /// <param name="aesKeyType">AES key type which decides key size</param>
        /// <returns>the pseudorandom octets</returns>
        private static byte[] DR(
            byte[] baseKey, 
            byte[] wellKnownConstant, 
            AesKeyType aesKeyType)
        {
            // to be encrypted data
            byte[] toBeEncrypted = new byte[wellKnownConstant.Length];
            wellKnownConstant.CopyTo(toBeEncrypted, 0);

            // n-fold the "well-known constant" if needed
            if (wellKnownConstant.Length != ConstValue.AES_BLOCK_SIZE)
            {
                toBeEncrypted = NFold(wellKnownConstant, ConstValue.AES_BLOCK_SIZE * ConstValue.BYTE_SIZE);
            }

            // AES key size
            uint aesKeySize = (uint)aesKeyType / ConstValue.BYTE_SIZE;

            // initialize key array
            byte[] rawkey = new byte[aesKeySize];

            // count means the total number of bytes has been copy to the rawkey.
            // length means how length of bytes should be copy to the rawkey array.
            uint count = 0;
            uint length = 0;

            // The initialCipherVector should be all zeros.
            byte[] initialCipherVector = new byte[ConstValue.AES_BLOCK_SIZE];
            
            // AES-CTS encryptor
            CipherTextStealingMode aesCtsCrypto = CryptoUtility.CreateAesCtsCrypto(baseKey, initialCipherVector);
            while (count < aesKeySize)
            {
                byte[] cipherBlock = aesCtsCrypto.EncryptFinal(toBeEncrypted, 0, toBeEncrypted.Length);
                length = (aesKeySize - count <= cipherBlock.Length ? (aesKeySize - count) : Convert.ToUInt32(cipherBlock.Length));
                Array.Copy(cipherBlock, 0, rawkey, count, length);
                count += length;
                toBeEncrypted = cipherBlock;
            }
            return rawkey;
        }


        /// <summary>
        /// RandomToKey generates a key from a random bitstring of a specific size.
        /// All the bits of the input string are assumed to be equally random, 
        /// even though the entropy present in the random source may be limited.
        /// [RFC 3961, Page 4]
        /// 
        /// For AES, random-to-key function simply returns as what is given
        /// [RFC 3961, Page 15]
        /// </summary>
        /// <param name="random">the random bitstring</param>
        /// <returns>the generated key</returns>
        public static byte[] RandomToKey(byte[] random)
        {
            return random;
        }


        /// <summary>
        /// Generate the "well-known constant"
        /// [RFC 3961, Page 15]
        /// the "well-known constant" used for the DK function is the key usage number, 
        /// expressed as four octets in big-endian order, followed by one octet indicated below:
        /// Kc = DK(base-key, usage | 0x99); 
        /// Ke = DK(base-key, usage | 0xAA);
        /// Ki = DK(base-key, usage | 0x55);
        /// </summary>
        /// <param name="usage">key usage number</param>
        /// <param name="derivedKeyType">the derived key type</param>
        /// <returns>the "well-known constant"</returns>
        private static byte[] GetWellKnownConstant(int usage, DerivedKeyType derivedKeyType)
        {
            // the "well-known constant" contains 5 bytes
            byte[] wellKnownConstant = new byte[5];

            // the first 4 bytes = usage number in big endian 
            byte[] usageBytes = BitConverter.GetBytes(usage);
            Array.Reverse(usageBytes);
            usageBytes.CopyTo(wellKnownConstant, 0);

            // the 5th byte = the derivedKeyType
            wellKnownConstant[4] = (byte)derivedKeyType;
            return wellKnownConstant;
        }


        /// <summary>
        /// N-Fold is an algorithm that takes m input bits and "stretches" them
        /// to form N output bits with equal contribution from each input bit to
        /// the output, as described in Blumenthal, U. and S. Bellovin, "A Better
        /// Key Schedule for DES-Like Ciphers", Proceedings of PRAGOCRYPT '96,1996.
        /// </summary>
        /// <param name="input">The to be n-folded input data</param>
        /// <param name="outputBits">The expected output length in bits</param>
        /// <returns>The n-folded data</returns>
        private static byte[] NFold(byte[] input, uint outputBits)
        {
            // check inputs
            if (null == input)
            {
                throw new ArgumentNullException("input");
            }
            if (0 != outputBits % ConstValue.BYTE_SIZE)
            {
                throw new ArgumentException(
                    "The desired output length in bits should be a multiply of 8 bits.");
            }

            // input and output length in bytes
            int inLength = input.Length;
            int outLength = (int)outputBits / ConstValue.BYTE_SIZE;

            // caculate their lowest common multiplier
            int lcm = CalculateLowestCommonMultiple(outLength, inLength);

            // "stretch" the data length to the LCM value
            byte[] stretchedData = new byte[lcm];
            int count = lcm / inLength;
            for (int i = 0; i < count; i++)
            {
                // expand
                Array.Copy(input, 0, stretchedData, i * inLength, inLength);

                // rotate 13 bits right
                input = Rotate13(input);
            }

            // divide the stretched data to (LCM/outLength) blocks 
            // then calculate their "one's complement addition"
            byte[] output = new byte[outLength];
            byte[] blockData = new byte[outLength];
            int blockCount = lcm / outLength;
            for (int i = 0; i < blockCount; i++)
            {
                // get a block
                Array.Copy(stretchedData, i * outLength, blockData, 0, blockData.Length);

                // addition
                output = OCADD(output, blockData);
            }
            return output;
        }


        /// <summary>
        /// The LCM function called by N-Fold Algorithm
        /// (calculate the Lowest Common Multiple of two integer)
        /// </summary>
        /// <param name="n">value n</param>
        /// <param name="k">value k</param>
        /// <returns>the caculated LCM value</returns>
        private static int CalculateLowestCommonMultiple(int n, int k)
        {
            int a = n;
            int b = k;
            int c;

            while (b != 0)
            {
                c = b;
                b = a % b;
                a = c;
            }
            return (n * k / a);
        }


        /// <summary>
        /// The ROT13 function called by N-Fold Algorithm
        /// (which rotates a string to 13 bits right)
        /// </summary>
        /// <param name="input">input string</param>
        private static byte[] Rotate13(byte[] input)
        {
            if (null == input)
            {
                throw new ArgumentNullException("input");
            }

            // get all the bits
            List<char> listBits = new List<char>();
            foreach (byte b in input)
            {
                listBits.AddRange(CryptoUtility.GetBits(b));
            }

            // rotate all the bits to 13-bit right
            List<char> listBitsRotated = new List<char>(listBits);
            for (int i = 0; i < listBits.Count; i++)
            {
                if (i + 13 < listBitsRotated.Count)
                {
                    listBitsRotated[i + 13] = listBits[i];
                }
                else
                {
                    int index = (i + 13) % listBitsRotated.Count;
                    listBitsRotated[index] = listBits[i];
                }
            }

            // covert the rotated data to bytes
            return CryptoUtility.ConvertBitsToBytes(listBitsRotated);
        }


        /// <summary>
        /// The OCADD function called by N-Fold Algorithm
        /// (calculate "one's complement addition" between two byte array)
        /// </summary>
        /// <param name="leftBuffer">one byte array in addition operation</param>
        /// <param name="rightBuffer">the other byte array in addition operation</param>
        /// <returns>The operation result</returns>
        private static byte[] OCADD(byte[] leftBuffer, byte[] rightBuffer)
        {
            // check inputs
            if (null == leftBuffer)
            {
                throw new ArgumentNullException("leftBuffer");
            }
            if (null == rightBuffer)
            {
                throw new ArgumentNullException("rightBuffer");
            }    
            if (leftBuffer.Length != rightBuffer.Length)
            {
                throw new ArgumentException("The input buffer lengths should be equal");
            }

            // initialize sum buffer
            byte[] sumBuffer = new byte[leftBuffer.Length];
            byte[] zeroBuffer = new byte[leftBuffer.Length];

            // the carry value
            int carry = 0;
            for (int i = leftBuffer.Length - 1; i >= 0; i--)
            {
                // caculate sum
                int sum = leftBuffer[i] + rightBuffer[i] + carry;
                sumBuffer[i] = (byte)(sum & 0xff);

                // reset carry
                if (sum > 0xff)
                {
                    carry = 1;
                }
                else
                {
                    carry = 0;
                }
            }

            // if there is a left over carry bit, add it back in
            if (1 == carry)
            {
                bool done = false;
                for (int j = leftBuffer.Length - 1; j >= 0; j--)
                {
                    if (sumBuffer[j] != 0xff)
                    {
                        sumBuffer[j]++;
                        done = true;
                        break;
                    }
                }

                if (!done)
                {
                    Array.Copy(zeroBuffer, sumBuffer, zeroBuffer.Length);
                }
            }
            return sumBuffer;
        }
        #endregion Private Methods


        #region Internal Methods
        /// <summary>
        /// Generate a derived key from base key
        /// [RFC 3961 Section 5.1 A Key Derivation Function]
        /// </summary>
        /// <param name="baseKey">the base key</param>
        /// <param name="usage">key usage</param>
        /// <param name="derivedKeyType">the derived key type</param>
        /// <param name="aesKeyType">AES key type which decides key size</param>
        /// <returns>the derived key in bytes</returns>
        internal static byte[] MakeDerivedKey(
            byte[] baseKey,
            int usage, 
            DerivedKeyType derivedKeyType, 
            AesKeyType aesKeyType)
        {
            if (null == baseKey)
            {
                throw new ArgumentNullException("baseKey");
            }

            // generate the well-known constant
            byte[] wellKnownConstant = GetWellKnownConstant(usage, derivedKeyType);

            // generate the derived key
            return DK(baseKey, wellKnownConstant, aesKeyType);
        }


        /// <summary>
        /// Generate an encryption key from password and salt
        /// [RFC 3962 Section 4 Key Generation from Pass Phrases or Random Data]
        /// (The pseudorandom function used by PBKDF2 will be a SHA-1 HMAC of 
        /// the passphrase and salt, as described in Appendix B.1 to PKCS#5)
        /// </summary>
        /// <param name="password">password</param>
        /// <param name="salt">salt</param>
        /// <param name="iterationCount">interation count</param>
        /// <param name="keyType">AES key type which decides key size</param>
        /// <returns>the encrypted key in bytes</returns>
        internal static byte[] MakeStringToKey(
            string password, 
            string salt, 
            uint iterationCount, 
            AesKeyType keyType)
        {
            if (null == password)
            {
                throw new ArgumentNullException("password");
            }
            if (null == salt)
            {
                throw new ArgumentNullException("salt");
            }

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            int keySize = (int)keyType / ConstValue.BYTE_SIZE;
            
            // generate the intermediate key
            Rfc2898DeriveBytes PBKDF2 = new Rfc2898DeriveBytes(passwordBytes, saltBytes, (int)iterationCount);
            byte[] intermediateKey = PBKDF2.GetBytes(keySize);
            intermediateKey = RandomToKey(intermediateKey);

            // generate the final key
            return DK(intermediateKey, KERBEROS_CONSTANT, keyType);
        }
        #endregion Internal Methods
    }
}