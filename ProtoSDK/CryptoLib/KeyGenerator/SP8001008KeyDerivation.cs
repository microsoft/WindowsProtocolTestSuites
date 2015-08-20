// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// Key derivation algorithm defined in NIST Special Publication 800-108
    /// Recommendation for Key Derivation Using Pseudorandom Functions
    /// </summary>
    public static class SP8001008KeyDerivation
    {
        /// <summary>
        /// Get derived key with counter mode and HMACSHA256.
        /// </summary>
        /// <param name="key">Key derivation key, a key that is used as an input to a key derivation function to derive keying material.</param>
        /// <param name="label">A byte array that identifies the purpose for the derived keying material.</param>
        /// <param name="context">A byte array containing the information related to the derived keying material.</param>
        /// <param name="derivedKeyLength">Length of derived key, in bits.</param>
        /// <returns></returns>
        public static byte[] CounterModeHmacSha256KeyDerive(byte[] key, byte[] label, byte[] context, int derivedKeyLength)
        {
            int h = 256;
            
            var hmacsha256 = new HMACSHA256(key);

            int n = (int)Math.Ceiling((double)derivedKeyLength / h);

            byte[] k = new byte[0];
            byte[] result = new byte[0];
            for (uint i = 1; i <= n; i++)
            {
                k = hmacsha256.ComputeHash(
                        BitConverter.GetBytes(i).Reverse()
                            .Concat(label)
                            .Concat(new byte[] { 0x00 })
                            .Concat(context)
                            .Concat(BitConverter.GetBytes(derivedKeyLength).Reverse()).ToArray());
                result = result.Concat(k).ToArray();
            }
            return result.Take(derivedKeyLength / 8).ToArray();
        }
    }
}
