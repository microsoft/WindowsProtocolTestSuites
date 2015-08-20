// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// The implementation of CTS mode for symmetric algorithms (such as AES, etc.).
    /// </summary>
    public class CipherTextStealingMode
    {
        #region Private Variables
        /// <summary>
        /// When each block-size plain text is encrypted, 
        /// the cipher should be temporarily stored as cipher state 
        /// for XOR operation with the next block-size plain text
        /// </summary>
        private byte[] cipherState;

        /// <summary>
        /// The Initialize Vector
        /// </summary>
        private byte[] iv;

        /// <summary>
        /// The encryption Block Size of the specific symmetric algorithm (in bytes)
        /// </summary>
        private int blockSize;

        /// <summary>
        /// The encryptor
        /// </summary>
        private ICryptoTransform encryptor;

        /// <summary>
        /// The decryptor
        /// </summary>
        private ICryptoTransform decryptor;
        #endregion Private Variables


        #region Constructor
        /// <summary>
        /// Initialize CipherTextStealingMode with a specific symmetric algorithm
        /// </summary>
        /// <param name="symmetricAlgorithm">The symmetric algorithm</param>
        public CipherTextStealingMode(SymmetricAlgorithm symmetricAlgorithm)
        {
            // in CTS Mode there is no padding
            symmetricAlgorithm.Padding = PaddingMode.None;

            // set the symmetric algorithm's mode to ECB
            // (for single block encryption and decryption)
            symmetricAlgorithm.Mode = CipherMode.ECB;

            // get the symmetric algorithm's block size in bytes
            blockSize = symmetricAlgorithm.BlockSize / 8;
            if (blockSize != symmetricAlgorithm.IV.Length)
            {
                throw new ArgumentException(
                    "The IV size should equal to the block size.");
            }

            // initialize local IV
            iv = symmetricAlgorithm.IV;

            // initialize cipher state using the symmetric algorithms's IV
            cipherState = new byte[blockSize];
            symmetricAlgorithm.IV.CopyTo(cipherState, 0);

            // create encryptor and decryptor
            encryptor = symmetricAlgorithm.CreateEncryptor();
            decryptor = symmetricAlgorithm.CreateDecryptor();
        }
        #endregion Constructor


        #region Private Methods: Encryption
        /// <summary>
        /// Encrypt in CBC Mode
        /// </summary>
        /// <param name="inputBuffer">input buffer</param>
        /// <param name="inputOffset">the offset of which the to be encrypted data begins</param>
        /// <param name="inputCount">the length of to be encrypted data</param>
        /// <returns>the encrypted data</returns>
        private byte[] EncryptWithCBCMode(
            byte[] inputBuffer,
            int inputOffset,
            int inputCount)
        {
            // encryption
            List<byte> result = new List<byte>();
            int endIndex = inputOffset + inputCount;
            while (inputOffset < endIndex)
            {
                // xor a block, encrypt it, and update cipher state
                byte[] blockBuffer = XorCipherState(inputBuffer, inputOffset, cipherState, blockSize);
                blockBuffer = encryptor.TransformFinalBlock(blockBuffer, 0, blockBuffer.Length);
                blockBuffer.CopyTo(cipherState, 0);
                inputOffset += blockSize;

                // save the block to result
                result.AddRange(blockBuffer);
            }
            return result.ToArray();
        }


        /// <summary>
        /// Encrypt in CTS Mode
        /// </summary>
        /// <param name="inputBuffer">input buffer</param>
        /// <param name="inputOffset">the offset of which the to be encrypted data begins</param>
        /// <param name="inputCount">the length of to be encrypted data</param>
        /// <returns>the encrypted data</returns>
        private byte[] EncryptWithCTSMode(
            byte[] inputBuffer,
            int inputOffset,
            int inputCount)
        {
            // caculate if the to-be-encrypted data is exactly a multiply of the block size
            int remainLength = inputCount % blockSize;
            if (0 == remainLength)
            {
                // first encrypt in CBC mode
                byte[] outputBuffer = EncryptWithCBCMode(inputBuffer, inputOffset, inputCount);

                // then swap the last two blocks
                int lastBlockIndex = outputBuffer.Length - blockSize;
                int nextToLastBlockIndex = outputBuffer.Length - 2 * blockSize;
                byte[] lastBlock = ArrayUtility.SubArray<byte>(outputBuffer, outputBuffer.Length - blockSize);
                Array.Copy(outputBuffer, nextToLastBlockIndex, outputBuffer, lastBlockIndex, blockSize);
                Array.Copy(lastBlock, 0, outputBuffer, nextToLastBlockIndex, blockSize);
                return outputBuffer;
            }
            else
            {
                // encrypt the input data without the last two blocks
                List<byte> result = new List<byte>();
                int frontLength = inputCount - blockSize - remainLength;
                if (frontLength > 0)
                {
                    byte[] frontOutputBuffer = EncryptWithCBCMode(inputBuffer, inputOffset, frontLength);
                    inputOffset += frontLength;
                    result.AddRange(frontOutputBuffer);
                }

                // encrypt the next to last block            
                byte[] nextToLastBlock = XorCipherState(inputBuffer, inputOffset, cipherState, blockSize);
                nextToLastBlock = encryptor.TransformFinalBlock(nextToLastBlock, 0, nextToLastBlock.Length);
                Array.Copy(nextToLastBlock, 0, cipherState, 0, blockSize);
                nextToLastBlock = ArrayUtility.SubArray<byte>(nextToLastBlock, 0, remainLength);
                
                // encrypt the last block
                inputOffset += blockSize;
                byte[] lastBlock = XorCipherState(inputBuffer, inputOffset, cipherState, remainLength);
                lastBlock = encryptor.TransformFinalBlock(lastBlock, 0, lastBlock.Length);

                // swap the last two blocks
                result.AddRange(lastBlock);
                result.AddRange(nextToLastBlock);
                return result.ToArray();
            }
        }
        #endregion Private Methods: Encryption


        #region Private Methods: Decryption
        /// <summary>
        /// Decrypt in CBC Mode
        /// </summary>
        /// <param name="inputBuffer">input buffer</param>
        /// <param name="inputOffset">the offset of which the to be decrypted data begins</param>
        /// <param name="inputCount">the length of to be decrypted data</param>
        /// <returns>the decrypted data</returns>
        private byte[] DecryptWithCBCMode(
            byte[] inputBuffer,
            int inputOffset,
            int inputCount)
        {
            // decryption
            List<byte> result = new List<byte>();
            int endIndex = inputOffset + inputCount;
            while (inputOffset < endIndex)
            {
                // decrypt a block, xor it with cipher state
                byte[] blockBuffer = ArrayUtility.SubArray<byte>(inputBuffer, inputOffset, blockSize);
                blockBuffer = decryptor.TransformFinalBlock(blockBuffer, 0, blockBuffer.Length);
                blockBuffer = XorCipherState(blockBuffer, 0, cipherState, blockSize);

                // fetch new cipher state
                Array.Copy(inputBuffer, inputOffset, cipherState, 0, blockSize);

                // update index
                inputOffset += blockSize;

                // save the block to result
                result.AddRange(blockBuffer);
            }
            return result.ToArray();
        }


        /// <summary>
        /// Decrypt in CTS Mode
        /// </summary>
        /// <param name="inputBuffer">input buffer</param>
        /// <param name="inputOffset">the offset of which the to be decrypted data begins</param>
        /// <param name="inputCount">the length of to be decrypted data</param>
        /// <returns>the decrypted data</returns>
        private byte[] DecryptWithCTSMode(
            byte[] inputBuffer,
            int inputOffset,
            int inputCount)
        {
            // caculate if the to-be-decrypted data is exactly a multiply of the block size
            int remainLength = inputCount % blockSize;
            if (0 == remainLength)
            {
                // first swap the last two blocks
                int lastBlockIndex = inputBuffer.Length - blockSize;
                int nextToLastBlockIndex = inputBuffer.Length - 2 * blockSize;
                byte[] lastBlock = ArrayUtility.SubArray<byte>(inputBuffer, inputBuffer.Length - blockSize);
                Array.Copy(inputBuffer, nextToLastBlockIndex, inputBuffer, lastBlockIndex, blockSize);
                Array.Copy(lastBlock, 0, inputBuffer, nextToLastBlockIndex, blockSize);

                // then decrypt
                return DecryptWithCBCMode(inputBuffer, inputOffset, inputCount);
            }
            else
            {
                // decrypt the input data without the last two blocks
                List<byte> result = new List<byte>();
                int frontLength = inputCount - blockSize - remainLength;
                if (frontLength > 0)
                {
                    byte[] frontOutputBuffer = DecryptWithCBCMode(inputBuffer, inputOffset, frontLength);
                    inputOffset += frontLength;
                    result.AddRange(frontOutputBuffer);
                }

                // decrypt the next to last block, then xor its result with the last block
                byte[] tempBlock = decryptor.TransformFinalBlock(inputBuffer, inputOffset, blockSize);
                byte[] lastBlock = ArrayUtility.SubArray<byte>(inputBuffer, inputOffset + blockSize, remainLength);
                byte[] nextToLastBlock = XorCipherState(tempBlock, 0, lastBlock, lastBlock.Length);
                
                // decrypt the last block
                lastBlock.CopyTo(tempBlock, 0);
                lastBlock = decryptor.TransformFinalBlock(tempBlock, 0, tempBlock.Length);
                lastBlock = XorCipherState(lastBlock, 0, cipherState, blockSize);

                // swap the last two blocks
                result.AddRange(lastBlock);
                result.AddRange(nextToLastBlock);
                return result.ToArray();
            }
        }
        #endregion Private Methods: Decryption


        #region Private Methods: Helpers
        /// <summary>
        /// XOR a block of data in the input buffer with current cipher state
        /// (The first xorSize bytes in cipher state are used for the operation)
        /// </summary>
        /// <param name="inputBuffer">input buffer</param>
        /// <param name="inputOffset">input offset</param>
        /// <param name="cipherStateBuffer">cipher state buffer</param>
        /// <param name="xorSize">the size in cipher state that used for XOR operation</param>
        /// <returns>the XOR result of one block size</returns>
        private byte[] XorCipherState(
            byte[] inputBuffer,
            int inputOffset,
            byte[] cipherStateBuffer,
            int xorSize)
        {
            byte[] blockBuffer = (byte[])cipherStateBuffer.Clone();
            for (int i = 0; i < xorSize; i++)
            {
                blockBuffer[i] = (byte)(inputBuffer[inputOffset + i] ^ cipherStateBuffer[i]);
            }
            return blockBuffer;
        }
        #endregion Private Methods: Helpers


        #region Internal Methods
        /// <summary>
        /// Computes the encryption transformation for the specified region of the specified byte array.
        /// </summary>
        /// <param name="inputBuffer">The input on which to perform the operation on.</param>
        /// <param name="inputOffset">The offset into the byte array from which to begin using data from.</param>
        /// <param name="inputCount">The number of bytes in the byte array to use as data.</param>
        /// <returns>The computed transformation</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when input buffer is null</exception>
        /// <exception cref="System.ArgumentException">Thrown when invalid argument is detected</exception>
        public byte[] EncryptFinal(
            byte[] inputBuffer,
            int inputOffset,
            int inputCount)
        {
            // Check input
            if (null == inputBuffer)
            {
                throw new ArgumentNullException("inputBuffer");
            }
            if (inputBuffer.Length < blockSize)
            {
                throw new ArgumentException(
                    "The input data to be encrypted should be at least one block size.");
            }
            if (inputOffset + inputCount > inputBuffer.Length)
            {
                throw new ArgumentException(
                   "The to-be encrypted data should not exceed the input array length.");
            }

            // Do encryption according to the to be encrypted data length
            byte[] result;
            if (inputCount == blockSize)
            {
                // exactly one block
                result = EncryptWithCBCMode(inputBuffer, inputOffset, inputCount);
            }
            else
            {
                // larger than one block
                result = EncryptWithCTSMode(inputBuffer, inputOffset, inputCount);
            }

            // Reset cipher state
            iv.CopyTo(cipherState, 0);
            return result;
        }


        /// <summary>
        /// Computes the decryption transformation for the specified region of the specified byte array.
        /// </summary>
        /// <param name="inputBuffer">The input on which to perform the operation on.</param>
        /// <param name="inputOffset">The offset into the byte array from which to begin using data from.</param>
        /// <param name="inputCount">The number of bytes in the byte array to use as data.</param>
        /// <returns>The computed transformation</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when input buffer is null</exception>
        /// <exception cref="System.ArgumentException">Thrown when invalid argument is detected</exception>
        public byte[] DecryptFinal(
            byte[] inputBuffer,
            int inputOffset,
            int inputCount)
        {
            // Check input
            if (null == inputBuffer)
            {
                throw new ArgumentNullException("inputBuffer");
            }
            if (inputBuffer.Length < blockSize)
            {
                throw new ArgumentException(
                    "The input data to be decrypted should be at least one block size.");
            }
            if (inputOffset + inputCount > inputBuffer.Length)
            {
                throw new ArgumentException(
                   "The to-be decrypted data should not exceed the input array length.");
            }

            // Do decryption according to the to be encrypted data length
            byte[] result;
            if (inputCount == blockSize)
            {
                // exactly one block
                result = DecryptWithCBCMode(inputBuffer, inputOffset, inputCount);
            }
            else
            {
                // larger than one block
                result = DecryptWithCTSMode(inputBuffer, inputOffset, inputCount);
            }

            // Reset cipher state
            iv.CopyTo(cipherState, 0);
            return result;
        }
        #endregion Internal Methods
    }
}
