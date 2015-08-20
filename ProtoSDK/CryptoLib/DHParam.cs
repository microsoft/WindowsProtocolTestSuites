// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Numerics;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// Used to calculate parameters used for DH algorithm.
    /// </summary>
    public static class DHParam
    {
        /// <summary>
        /// Compute parameter Q
        /// Refer to doc RFC 3279 section 2.3.3 
        /// </summary>
        /// <param name="p">The p parameter</param>
        /// <returns>The Q parameter</returns>
        public static byte[] ComputeQ(byte[] p)
        {
            if (p == null)
            {
                throw new ArgumentNullException("p");
            }

            BigInteger num = Utility.LoadBytesLittleEndian(p);

            byte[] result = ((num - 1) / 2).ToByteArray();

            if (result[result.Length - 1] == 0)
            {
                Array.Resize(ref result, result.Length - 1);
            }
            Array.Reverse(result);

            return result;
        }


        /// <summary>
        /// Compute public key with private key.
        /// Refer to doc RFC 3279 section 2.3.3
        /// </summary>
        /// <param name="baseNum">The base number</param>
        /// <param name="privateKey">The private key</param>
        /// <param name="prime">The prime</param>
        /// <returns>The public key</returns>
        public static byte[] GetPublicKey(byte[] baseNum, byte[] privateKey, byte[] prime)
        {
            if (baseNum == null)
            {
                throw new ArgumentNullException("baseNum");
            }
            if (privateKey == null)
            {
                throw new ArgumentNullException("privateKey");
            }
            if (prime == null)
            {
                throw new ArgumentNullException("prime");
            }

            BigInteger G = Utility.LoadBytesLittleEndian(baseNum);
            BigInteger PrivateKey = Utility.LoadBytesLittleEndian(privateKey);
            BigInteger P = Utility.LoadBytesLittleEndian(prime);

            byte[] result = BigInteger.ModPow(G, PrivateKey, P).ToByteArray();

            if (result[result.Length - 1] == 0)
            {
                Array.Resize(ref result, result.Length - 1);
            }
            Array.Reverse(result);

            return result;
        }
    }
}