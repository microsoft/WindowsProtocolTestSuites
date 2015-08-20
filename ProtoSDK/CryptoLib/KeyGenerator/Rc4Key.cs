// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// RC4 key generator
    /// </summary>
    internal static class Rc4Key
    {
        /// <summary>
        /// Derive an RC4 key based on password
        /// </summary>
        /// <param name="password">user password</param>
        /// <returns>the generated RC4 key of 16 bytes</returns>
        internal static byte[] MakeStringToKey(string password)
        {
            if (null == password)
            {
                throw new ArgumentException("Input password should not be null.");
            }

            // password bytes
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);

            // compute md4 hash
            return CryptoUtility.ComputeMd4(passwordBytes);
        }
        /// <summary>
        /// RandomToKey generates a key from a random bitstring of a specific size.
        /// All the bits of the input string are assumed to be equally random, 
        /// even though the entropy present in the random source may be limited.
        /// [RFC 3961, Page 4]
        /// 
        /// For RC4, random-to-key function simply returns as what is given according to the test vector
        /// [RFC 6113, Page 45]
        /// </summary>
        /// <param name="random">the random bitstring</param>
        /// <returns>the generated key</returns>
        public static byte[] RandomToKey(byte[] random)
        {
            return random;
        }
    }
}
