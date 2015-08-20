// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// Crypto Class for AES_CMAC_128
    /// </summary>
    public static class AesCmac128
    {
        private static byte[] Xor128(byte[] dataA, int offsetA, byte[] dataB, int offsetB)
        {
            byte[] output = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                output[i] = (byte)(dataA[offsetA + i] ^ dataB[offsetB + i]);
            }
            return output;
        }

        private static byte[] Xor128(byte[] dataA, byte[] dataB)
        {
            return Xor128(dataA, 0, dataB, 0);
        }

        private static byte[] LeftshiftOnebit(byte[] input)
        {
            byte[] output = new byte[16];
            byte overflow = 0;
            for (int i = 15; i >= 0; i--)
            {
                output[i] = (byte)(input[i] << 1);
                output[i] |= overflow;
                overflow = (byte)((input[i] & 0x80) == 0x80 ? 1 : 0);
            }
            return output;
        }

        /// <summary>
        /// Encrypt message by AES with given secret key and key size.
        /// </summary>
        /// <param name="key">The secret key for the AES algorithm.</param>
        /// <param name="message">Message to be encrypted.</param>
        /// <returns>Message has been encrypted.</returns>
        private static byte[] EncrypteMessageByAes(byte[] key, byte[] message)
        {
            Aes aesAlg = Aes.Create();
            aesAlg.KeySize = 128;
            aesAlg.IV = new byte[16];
            aesAlg.Key = key;
            ICryptoTransform encryptor = aesAlg.CreateEncryptor();
            byte[] encryptedMessage = EncrypteMessageByAes(encryptor, message);
            return encryptedMessage;
        }

        /// <summary>
        /// Encrypt message by AES with given secret key and key size.
        /// </summary>
        /// <param name="encryptor">A symmetric encryptor object with key and IV</param>
        /// <param name="message">Message to be encrypted.</param>
        /// <returns>Message has been encrypted.</returns>
        private static byte[] EncrypteMessageByAes(ICryptoTransform encryptor, byte[] message)
        {
            byte[] encryptedMessage = new byte[16];
            byte[] encryptedBytes = encryptor.TransformFinalBlock(message, 0, message.Length).ToArray();
            for (int i = 0; i < 16; i++)
            {
                encryptedMessage[i] = encryptedBytes[i];
            }
            return encryptedMessage;
        }

        /// <summary>
        /// The subkey generation algorithm
        /// </summary>
        /// <param name="key">128-bit key</param>
        /// <param name="firstSubKey">128-bit first subkey</param>
        /// <param name="secondSubKey">128-bit second subkey</param>
        private static void GenerateSubKey(byte[] key, out byte[] firstSubKey, out byte[] secondSubKey)
        {
            byte[] L = new byte[16];
            // For CMAC Calculation
            // const_Zero is 0x00000000000000000000000000000000     
            // const_Rb   is 0x00000000000000000000000000000087
            byte[] const_Zero = new byte[] {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte[] const_Rb = new byte[] {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x87 };
            byte[] tmp = new byte[16];
            firstSubKey = new byte[16]; 
            secondSubKey = new byte[16]; 

            L = EncrypteMessageByAes(key, const_Zero); // AES-128 with key K is applied to an all-zero input block

            if ((L[0] & 0x80) == 0)
            { /* If MSB(L) = 0, then K1 = L << 1 */
                firstSubKey = LeftshiftOnebit(L);
            }
            else
            {    /* Else K1 = ( L << 1 ) (+) Rb */
                tmp = LeftshiftOnebit(L);
                firstSubKey = Xor128(tmp, const_Rb);
            }

            if ((firstSubKey[0] & 0x80) == 0)
            { // If MSB(K1) = 0, then K2 = L << 1 
                secondSubKey = LeftshiftOnebit(firstSubKey);
            }
            else
            { // Else K2 ( L << 1 ) (+) Rb 
                tmp = LeftshiftOnebit(firstSubKey);
                secondSubKey = Xor128(tmp, const_Rb);
            }
            return;
        }

        /// <summary>
        /// padding is the concatenation of source data and a single '1',
        /// followed by the minimum number of '0's, so that the total length is equal to 128 bits.
        /// </summary>
        /// <param name="sourceData">The original data.</param>
        /// <param name="offset">The offset where padding starts.</param>
        /// <param name="length">Length for padding.</param>
        /// <returns>The padded data.</returns>
        private static byte[] Padding(byte[] sourceData, int offset, int length)
        {
            byte[] paddedData = new byte[16];
            /* original last block */
            for (int i = 0; i < 16; i++)
            {
                if (i < length)
                {
                    paddedData[i] = sourceData[offset + i];
                }
                else if (i == length)
                {
                    paddedData[i] = 0x80;
                }
                else
                {
                    paddedData[i] = 0x00;
                }
            }
            return paddedData;
        }

        /// <summary>
        /// Encrypte message By AES_CMAC_128
        /// </summary>
        /// <param name="key">128-bit key</param>
        /// <param name="message">Message to be encrypted.</param>
        /// <returns>Message has been encrypted.</returns>
        public static byte[] ComputeHash(byte[] key, byte[] message)
        {
            byte[] encryptedMessage = new byte[16];
            int length = message.Length;
            byte[] X = new byte[16];
            byte[] Y = new byte[16];
            byte[] M_last = new byte[16];
            byte[] padded = new byte[16];
            byte[] K1 = new byte[16];
            byte[] K2 = new byte[16];

            int n, i, flag;
            GenerateSubKey(key, out K1, out K2);

            n = (length + 15) / 16;       /* n is number of rounds */

            if (n == 0)
            {
                n = 1;
                flag = 0;
            }
            else
            {
                if ((length % 16) == 0)
                { /* last block is a complete block */
                    flag = 1;
                }
                else
                { /* last block is not complete block */
                    flag = 0;
                }
            }

            if (flag != 0)
            { /* last block is complete block */
                M_last = Xor128(message, 16 * (n - 1), K1, 0);
            }
            else
            {
                padded = Padding(message, 16 * (n - 1), length % 16); 
                M_last = Xor128(padded, K2);
            }

            for (i = 0; i < 16; i++)
            {
                X[i] = 0;
            }

            Aes aesAlg = Aes.Create();
            aesAlg.KeySize = 128;
            aesAlg.IV = new byte[16];
            aesAlg.Key = key;
            ICryptoTransform encryptor = aesAlg.CreateEncryptor();
            for (i = 0; i < n - 1; i++)
            {
                Y = Xor128(X, 0, message, 16 * i);
                X = EncrypteMessageByAes(encryptor, Y);
            }

            Y = Xor128(X, M_last);
            X = EncrypteMessageByAes(encryptor, Y);

            for (i = 0; i < 16; i++)
            {
                encryptedMessage[i] = X[i];
            }
            return encryptedMessage;
        }

    }

}
