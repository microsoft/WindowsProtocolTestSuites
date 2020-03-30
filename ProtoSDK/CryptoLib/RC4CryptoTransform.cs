// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// Implement RC4 cryptographic transformations.
    /// </summary>
    internal class RC4CryptoTransform : ICryptoTransform
    {
        private const int VectorSize = 256;
        private byte[] s;
        private int a;
        private int b;
        private bool disposed;

        public RC4CryptoTransform(byte[] key)
        {
            a = 0;
            b = 0;
            Initialize(key);
        }

        /// <summary>
        /// Gets a value indicating whether the current transform can be reused.
        /// </summary>
        public bool CanReuseTransform
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether multiple blocks can be transformed.
        /// </summary>
        public bool CanTransformMultipleBlocks
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the input block size.
        /// </summary>
        public int InputBlockSize
        {
            get
            {
                // RC4's input type is byte[].
                // The minimum unit involved in the transform is 1 byte.
                return 1;
            }
        }

        /// <summary>
        /// Gets the output block size.
        /// </summary>
        public int OutputBlockSize
        {
            get
            {
                // RC4's out type is byte[].
                // the minimum unit involved in the transform is 1 byte.
                return 1;
            }
        }

        /// <summary>
        /// Transforms the specified region of the input byte array and copies the resulting
        /// transform to the specified region of the output byte array.
        /// </summary>
        /// <param name="inputBuffer">The input for which to compute the transform.</param>
        /// <param name="inputOffset">The offset into the input byte array from which to begin using data.</param>
        /// <param name="inputCount">The number of bytes in the input byte array to use as data.</param>
        /// <param name="outputBuffer">The output to which to write the transform.</param>
        /// <param name="outputOffset">The offset into the output byte array from which to begin writing data.</param>
        /// <returns>The number of bytes written.</returns>
        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount,
            byte[] outputBuffer, int outputOffset)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }

            if (inputOffset < 0)
            {
                throw new ArgumentOutOfRangeException("inputOffset");
            }

            if ((inputCount + inputOffset) > inputBuffer.Length)
            {
                throw new ArgumentOutOfRangeException("inputCount");
            }

            if (outputBuffer == null)
            {
                throw new ArgumentNullException("outputBuffer");
            }

            if (outputOffset < 0)
            {
                throw new ArgumentOutOfRangeException("outputOffset");
            }

            byte[] output = Transform(inputBuffer, inputOffset, inputCount);
            Array.Copy(output, 0, outputBuffer, outputOffset, output.Length);
            return output.Length;
        }

        /// <summary>
        /// Transforms the specified region of the specified byte array.
        /// </summary>
        /// <param name="inputBuffer">The input for which to compute the transform.</param>
        /// <param name="inputOffset">The offset into the byte array from which to begin using data.</param>
        /// <param name="inputCount">The number of bytes in the byte array to use as data.</param>
        /// <returns>The computed transform.</returns>
        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException("inputBuffer");
            }

            if (inputOffset < 0)
            {
                throw new ArgumentOutOfRangeException("inputOffset");
            }

            if ((inputCount + inputOffset) > inputBuffer.Length)
            {
                throw new ArgumentOutOfRangeException("inputCount");
            }

            return Transform(inputBuffer, inputOffset, inputCount);
        }

        /// <summary>
        /// Release all resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release all resources
        /// </summary>
        /// <param name="disposing">Indicates GC or user calling this function</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types
                    Array.Clear(s, 0, s.Length);
                }
            }
            disposed = true;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~RC4CryptoTransform()
        {
            Dispose(false);
        }

        private void Initialize(byte[] key)
        {
            s = new byte[VectorSize];
            for (int i = 0; i < VectorSize; ++i)
            {
                s[i] = (byte)i;
            }

            for (int i = 0, j = 0; i < VectorSize; ++i)
            {
                j = (j + s[i] + key[i % key.Length]) % VectorSize;
                Swap(s, i, j);
            }
        }

        private void Swap(byte[] arr, int i, int j)
        {
            byte tmp = arr[i];
            arr[i] = arr[j];
            arr[j] = tmp;
        }

        private byte[] Transform(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            byte[] output = new byte[inputCount];
            inputBuffer.CopyTo(output, inputOffset);

            for (int i = inputOffset; i < inputCount; ++i)
            {
                a = (a + 1) % VectorSize;
                b = (b + s[a]) % VectorSize;
                Swap(s, a, b);
                int t = (s[a] + s[b]) % VectorSize;
                byte k = s[t];
                output[i] = (byte)(output[i] ^ k);
            }
            return output;
        }
    }
}
