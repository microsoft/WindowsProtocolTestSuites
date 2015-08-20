// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// A RC4 implementation using native windows api
    /// </summary>
    public class RC4CryptoServiceProvider : RC4
    {
        private IntPtr providerHandle;
        private IntPtr keyHandle;
        private bool disposed;

        // the header is used to form the pbKeyData in CryptImportKey function.
        // this header for ALG_RC4 is unique. the data stands for this structure:
        // typedef struct _PUBLICKEYSTRUC {
        //                                  BYTE bType;
        //                                  BYTE bVersion;
        //                                  WORD reserved;
        //                                  ALG_ID aiKeyAlg;
        //                                }
        private static byte[] blobHeader = new byte[] { 8, 2, 0, 0, 1, 104, 0, 0};

        // prefixdata will be put in front of real key data, its length is always 12.
        private const int keyPrefixDataLength = 12;

        /// <summary>
        /// default constructor
        /// </summary>
        public RC4CryptoServiceProvider()
        {
            providerHandle = IntPtr.Zero;

            bool status = NativeMethods.CryptAcquireContext(ref providerHandle, "RC4", null,
                ProviderType.PROV_RSA_AES, 0);

            if (!status)
            {
                status = NativeMethods.CryptAcquireContext(ref providerHandle, "RC4",
                    null, ProviderType.PROV_RSA_AES, CryptAcquireContextTypes.CRYPT_NEWKEYSET);
                if (!status)
                {
                    throw new CryptographicException("RC4 initializing failed: CryptAcquireContext."
                        + Helper.GetLastErrorCodeString());
                }
            }

            //by defualt it uses 80-bits key
            KeySizeValue = 80;
            IVValue = new byte[0];
        }


        /// <summary>
        /// Gets or sets the secret key for the symmetric algorithm.
        /// </summary>
        public override byte[] Key
        {
            get
            {
                return (byte[])base.KeyValue.Clone();
            }
            set
            {
                base.KeyValue = (byte[])value.Clone();
                ImportKey(base.KeyValue);
                //Represents the size, in bits, of the secret key used by the symmetric algorithm.
                base.KeySizeValue = base.KeyValue.Length * 8;
            }
        }


        /// <summary>
        /// When overridden in a derived class, creates a symmetric encryptor object
        /// with the specified System.Security.Cryptography.SymmetricAlgorithm.Key property
        /// and initialization vector (System.Security.Cryptography.SymmetricAlgorithm.IV).
        /// </summary>
        /// <param name="rgbKey">The secret key to use for the symmetric algorithm.</param>
        /// <param name="rgbIV">The initialization vector to use for the symmetric algorithm.</param>
        /// <returns>A symmetric decryptor object.</returns>
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            // RC4 don't need rgbIV parameter 

            if (rgbKey == null)
            {
                throw new ArgumentNullException("rgbKey");
            }

            return CreateTransformer(rgbKey);
        }


        /// <summary>
        /// When overridden in a derived class, creates a symmetric decryptor object
        /// with the specified System.Security.Cryptography.SymmetricAlgorithm.Key property
        /// and initialization vector (System.Security.Cryptography.SymmetricAlgorithm.IV).
        /// </summary>
        /// <param name="rgbKey">The secret key to use for the symmetric algorithm.</param>
        /// <param name="rgbIV">The initialization vector to use for the symmetric algorithm.</param>
        /// <returns>A symmetric decryptor object.</returns>
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            if (rgbKey == null)
            {
                throw new ArgumentNullException("rgbKey");
            }

            return CreateTransformer(rgbKey);
        }


        /// <summary>
        /// When overridden in a derived class, 
        /// generates a random key (System.Security.Cryptography.SymmetricAlgorithm.Key)
        /// to use for the algorithm.
        /// </summary>
        public override void GenerateKey()
        {
            IntPtr generatedKey = IntPtr.Zero;
            // right shift keysize 16 bits, this is used to form flag used in CryptGenKey function
            int keySize = this.KeySize << 16;
            bool status = NativeMethods.CryptGenKey(providerHandle, AlgId.CALG_RC4,
                keySize | (int)CryptGenKeyTypes.CRYPT_EXPORTABLE, ref generatedKey);

            if (!status)
            {
                throw new CryptographicException("CryptGenKey Fails:" + Helper.GetLastErrorCodeString());
            }

            if (keyHandle != null)
            {
                NativeMethods.CryptDestroyKey(keyHandle);
            }
            keyHandle = generatedKey;

            uint keyLength = 0;
            //determin keyLength value
            status = NativeMethods.CryptExportKey(generatedKey, IntPtr.Zero, 
                CryptExportKeyBlobType.PLAINTEXTKEYBLOB, 0, null, ref keyLength);

            if (!status)
            {
                throw new CryptographicException("CryptExportKey Fails:" + Helper.GetLastErrorCodeString());
            }

            byte[] tempKey = new byte[keyLength];
            status = NativeMethods.CryptExportKey(generatedKey, IntPtr.Zero,
                CryptExportKeyBlobType.PLAINTEXTKEYBLOB, 0, tempKey, ref keyLength);

            if (!status)
            {
                throw new CryptographicException("CryptExportKey Fails:" + Helper.GetLastErrorCodeString());
            }


            Key = new byte[tempKey.Length - keyPrefixDataLength];
            Array.Copy(tempKey, keyPrefixDataLength, Key, 0, Key.Length);
            // Set KeySizeValue, in bit.
            KeySizeValue = Key.Length * 8;
        }


        /// <summary>
        /// RC4 don't support IV
        /// </summary>
        public override void GenerateIV()
        {
            //leave it empty, can't throw NotSupportException because CreateEncrypt() will automately 
            //call this function and RC4 does not support generate IV.
        }


        /// <summary>
        /// import the key for transform
        /// </summary>
        /// <param name="importedKey">the imported key</param>
        private void ImportKey(byte[] importedKey)
        {
            if (importedKey == null)
            {
                throw new ArgumentNullException("importedKey");
            }

            byte[] pbKeyData = new byte[keyPrefixDataLength + importedKey.Length];
            byte[] keyLength = BitConverter.GetBytes(importedKey.Length);

            Array.Copy(blobHeader, 0, pbKeyData, 0, blobHeader.Length);
            Array.Copy(keyLength, 0, pbKeyData, blobHeader.Length, keyLength.Length);
            Array.Copy(importedKey, 0, pbKeyData, keyPrefixDataLength, importedKey.Length);

            IntPtr newKeyHandle = IntPtr.Zero;
            bool status = NativeMethods.CryptImportKey(providerHandle, pbKeyData,
                pbKeyData.Length, IntPtr.Zero, (uint)CryptGenKeyTypes.CRYPT_EXPORTABLE, ref newKeyHandle);

            if (!status)
            {
                throw new CryptographicException("CryptImportKey fails:" + Helper.GetLastErrorCodeString());
            }

            if (keyHandle != IntPtr.Zero)
            {
                NativeMethods.CryptDestroyKey(keyHandle);
            }

            keyHandle = newKeyHandle;
        }

        
        /// <summary>
        /// Create a transformer to encrypt or decrypt data
        /// </summary>
        /// <param name="rgbKey">The secret key used for transforming</param>
        /// <returns>A symmetric decryptor or encryptor object.</returns>
        private ICryptoTransform CreateTransformer(byte[] rgbKey)
        {
            this.Key = rgbKey;

            RC4CryptoTransform encryptor = new RC4CryptoTransform();

            IntPtr transKey = IntPtr.Zero;
            bool status = NativeMethods.CryptDuplicateKey(keyHandle, IntPtr.Zero, 0, ref transKey);

            if (!status)
            {
                throw new CryptographicException("CryptDuplicateKey fails:" + Helper.GetLastErrorCodeString());
            }

            encryptor.keyHandle = transKey;

            return (ICryptoTransform)encryptor;
        }


        /// <summary>
        /// Release all resources
        /// </summary>
        /// <param name="disposing">Indicates GC or user calling this</param>
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

                if (keyHandle != IntPtr.Zero)
                {
                    NativeMethods.CryptDestroyKey(keyHandle);
                    keyHandle = IntPtr.Zero;
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
