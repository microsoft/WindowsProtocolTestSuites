// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// Crypto Class for AES_GMAC
    /// </summary>
    public static class AesGmac
    {
        public static (byte[] ciphertext, byte[] tag) ComputeHash(byte[] key, byte[] nonce, byte[] aadBytes)
        {
            using (var aes = new AesGcm(key))
            {
                var plaintextBytes = Encoding.UTF8.GetBytes("");
                var ciphertext = new byte[plaintextBytes.Length];
                var tag = new byte[AesGcm.TagByteSizes.MaxSize]; // MaxSize = 16

                aes.Encrypt(nonce, plaintextBytes, ciphertext, tag, aadBytes);

                return (ciphertext, tag);
            }
        }
    }
}
