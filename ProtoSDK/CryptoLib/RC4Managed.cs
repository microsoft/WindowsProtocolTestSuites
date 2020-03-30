// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// A managed code implementation of RC4 stream cipher.
    /// </summary>
    public class RC4Managed : RC4
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public RC4Managed()
        {
            // By defualt it uses 128-bits key
            KeySizeValue = 128;
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
                // Represents the size, in bits, of the secret key used by the symmetric algorithm.
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
            // RC4 doesn't need rgbIV parameter
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
            // RC4 doesn't need rgbIV parameter
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
            Random rnd = new Random();
            byte[] newKey = new byte[KeySizeValue / 8];
            rnd.NextBytes(newKey);
            Key = newKey;
        }

        /// <summary>
        /// RC4 doesn't support IV.
        /// </summary>
        public override void GenerateIV()
        {
            // leave it empty, can't throw NotSupportException because CreateEncrypt() will automately
            // call this function and RC4 does not support generate IV.
        }

        /// <summary>
        /// Create a transformer to encrypt or decrypt data
        /// </summary>
        /// <param name="rgbKey">The secret key used for transforming</param>
        /// <returns>A symmetric decryptor or encryptor object.</returns>
        private ICryptoTransform CreateTransformer(byte[] rgbKey)
        {
            this.Key = rgbKey;

            RC4CryptoTransform transformer = new RC4CryptoTransform(rgbKey);
            return transformer;
        }
    }
}
