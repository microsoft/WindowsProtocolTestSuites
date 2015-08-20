// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;


namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// implement RC4 cryptographic transformations.
    /// </summary>
    internal class RC4CryptoTransform : ICryptoTransform
    {
        internal IntPtr keyHandle;
        private bool disposed;

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
                //rc4's input type is byte[].
                //the minus unit involved in the transform is 1 byte
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
                //rc4's out type is byte[].
                //the minus unit involved in the transform is 1 byte
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
            

            byte[] inputTransformData = new byte[inputCount];
            Array.Copy(inputBuffer, inputOffset, inputTransformData, 0, inputCount);
            int outputTransformDataCount = inputCount;

            bool status = NativeMethods.CryptEncrypt(keyHandle, IntPtr.Zero, 0, 0,
                inputTransformData, ref outputTransformDataCount, inputCount);
            if (!status)
            {
                throw new CryptographicException("CryptEncrypt fails:" + Helper.GetLastErrorCodeString());
            }

            Array.Copy(inputTransformData, 0, outputBuffer, outputOffset, outputTransformDataCount);
            return outputTransformDataCount;
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

            byte[] inputTransformData = new byte[inputCount];
            Array.Copy(inputBuffer, inputOffset, inputTransformData, 0, inputCount);
            int outputTransformDataCount = inputCount;

            bool status = NativeMethods.CryptEncrypt(keyHandle, IntPtr.Zero, 0, 0, 
                inputTransformData, ref outputTransformDataCount, inputCount);

            if (!status)
            {
                throw new CryptographicException("CryptEncrypt fails:" + Helper.GetLastErrorCodeString());
            }

            byte[] outputTransformData = new byte[outputTransformDataCount];
            Array.Copy(inputTransformData, 0, outputTransformData, 0, outputTransformDataCount);

            return outputTransformData;
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
        /// release all resources
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
                }

                // Call the appropriate methods to clean up unmanaged resources.
                // If disposing is false, only the following code is executed.
                if (keyHandle != IntPtr.Zero)
                {
                    NativeMethods.CryptDestroyKey(keyHandle);
                    keyHandle = IntPtr.Zero;
                }
            }
            disposed = true;
        }


        /// <summary>
        /// deconstructor
        /// </summary>
        ~RC4CryptoTransform()
        {
            Dispose(false);
        }
    }
}
