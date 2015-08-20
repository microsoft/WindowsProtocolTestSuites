// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// A MD4 implementation using windows native security api
    /// </summary>
    public class MD4CryptoServiceProvider : MD4
    {
        private IntPtr providerHandle;
        private IntPtr hashHandle;
        private bool disposed;

        /// <summary>
        /// Initialize MD4CryptoServiceProvider
        /// </summary>
        public MD4CryptoServiceProvider()
        {
            providerHandle = IntPtr.Zero;
            hashHandle = IntPtr.Zero;
            //MD4's hash size is always 128, in bit;
            this.HashSizeValue = 128;

            bool status = NativeMethods.CryptAcquireContext(ref providerHandle, "MD4", null,
                ProviderType.PROV_RSA_AES, 0);

            if (!status)
            {
                status = NativeMethods.CryptAcquireContext(ref providerHandle, "MD4", null,
                    ProviderType.PROV_RSA_AES, CryptAcquireContextTypes.CRYPT_NEWKEYSET);
                if (!status)
                {
                    throw new CryptographicException("MD4 initializing failed: CryptAcquireContext." 
                        + Helper.GetLastErrorCodeString());
                }
            }
        }


        /// <summary>
        /// Initialize the hash
        /// </summary>
        public override void Initialize()
        {
            bool status = NativeMethods.CryptCreateHash(providerHandle, AlgId.CALG_MD4, IntPtr.Zero, 
                0, ref hashHandle);

            if (!status)
            {
                throw new CryptographicException("Initialize failed: CryptCreateHash."
                    + Helper.GetLastErrorCodeString());
            }
        }


        /// <summary>
        /// routes data written to the object into
        /// the hash algorithm for computing the hash.
        /// </summary>
        /// <param name="array">The input to compute the hash code for.</param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
        /// <param name="cbSize">The number of bytes in the byte array to use as data.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            bool status;
            byte[] input = new byte[cbSize];

            Array.Copy(array, ibStart, input, 0, cbSize);
            status = NativeMethods.CryptHashData(hashHandle, input, (uint)cbSize, 0);

            if (!status)
            {
                throw new CryptographicException("HashCore failed: CryptHashData." 
                    + Helper.GetLastErrorCodeString());
            }
        }


        /// <summary>
        /// finalizes the hash computation after
        /// the last data is processed by the cryptographic stream object.
        /// </summary>
        /// <returns>The computed hash code.</returns>
        protected override byte[] HashFinal()
        {
            byte[] hashSizeBuffer = new byte[sizeof(int)];
            uint hashSize = (uint)hashSizeBuffer.Length;

            bool status = NativeMethods.CryptGetHashParam(hashHandle, HashParameters.HP_HASHSIZE, hashSizeBuffer,
                ref hashSize, 0);

            if (!status)
            {
                throw new CryptographicException("HashFinal failed: CryptGetHashParam." 
                    + Helper.GetLastErrorCodeString());
            }

            hashSize = (uint)BitConverter.ToUInt32(hashSizeBuffer, 0);
            byte[] hashValue = new byte[hashSize];

            status = NativeMethods.CryptGetHashParam(hashHandle, HashParameters.HP_HASHVAL,
                hashValue, ref hashSize, 0);

            if (!status)
            {
                throw new CryptographicException("HashFinal failed: CryptGetHashParam." 
                    + Helper.GetLastErrorCodeString());
            }

            if (hashHandle != IntPtr.Zero)
            {
                NativeMethods.CryptDestroyHash(hashHandle);
                hashHandle = IntPtr.Zero;
            }

            return hashValue;
        }


        /// <summary>
        /// release resources
        /// </summary>
        /// <param name="disposing">indicates GC or user calling this function</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

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
                if (hashHandle != IntPtr.Zero)
                {
                    NativeMethods.CryptDestroyHash(hashHandle);
                    hashHandle = IntPtr.Zero;
                }
                if (providerHandle != IntPtr.Zero)
                {
                    NativeMethods.CryptReleaseContext(providerHandle, 0);
                    providerHandle = IntPtr.Zero;
                }
            }
            disposed = true;
        }
    }
}
