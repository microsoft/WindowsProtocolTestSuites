// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// A managed code implementation of MD4 hash algorithm [RFC1320].
    /// </summary>
    public class MD4Managed : MD4
    {
        // A 512-bit buffer is used to compute the message digest, which is 64 bytes
        private const int MD4BlockSize = 64;
        private byte[] buffer;

        // MD4 produces a 128-bit message digest, which is 4 words
        private uint[] state;

        // A 64-bit representation of the length of input message
        private uint[] count;

        /// <summary>
        /// Initialize MD4Managed class.
        /// </summary>
        public MD4Managed()
        {
            buffer = new byte[MD4BlockSize];
            state = new uint[4];
            count = new uint[2];

            InitializeState();
        }

        /// <summary>
        /// Initialize process of MD4.
        /// </summary>
        public override void Initialize()
        {
            InitializeState();
        }

        /// <summary>
        /// Routes data written to the object into the hash algorithm for computing the hash.
        /// </summary>
        /// <param name="array">The input to compute the hash code for.</param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
        /// <param name="cbSize">The number of bytes in the byte array to use as data.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            byte[] input = new byte[cbSize];
            Array.Copy(array, ibStart, input, 0, cbSize);

            MD4Update(input, cbSize);
        }

        /// <summary>
        /// Finalizes the hash computation after
        /// the last data is processed by the cryptographic stream object.
        /// </summary>
        /// <returns>The computed hash code.</returns>
        protected override byte[] HashFinal()
        {
            int HashSizeInBytes = HashSizeValue / 8;
            byte[] digest = new byte[HashSizeInBytes];

            byte[] bits = new byte[8];
            byte[] padding;
            int index, padLen;

            /* Save number of bits */
            Buffer.BlockCopy(count, 0, bits, 0, 8);

            /* Pad out to 56 mod 64 */
            index = (int)((count[0] >> 3) & 0x3f);
            padLen = (index < 56) ? (56 - index) : (120 - index);
            padding = new byte[padLen];
            Array.Clear(padding, 0, padding.Length);
            padding[0] = 0x80;
            MD4Update(padding, padLen);

            /* Append length (before padding) */
            MD4Update(bits, 8);

            /* Store state in digest */
            Buffer.BlockCopy(state, 0, digest, 0, digest.Length);

            /* Zeroize sensitive information */
            Array.Clear(buffer, 0, buffer.Length);
            Array.Clear(state, 0, state.Length);
            Array.Clear(count, 0, count.Length);

            return digest;
        }

        private void InitializeState()
        {
            Array.Clear(buffer, 0, buffer.Length);

            state[0] = 0x67452301;
            state[1] = 0xefcdab89;
            state[2] = 0x98badcfe;
            state[3] = 0x10325476;

            count[0] = 0;
            count[1] = 0;
        }

        private static uint F(uint x, uint y, uint z)
        {
            return ((x & y) | (~x & z));
        }

        private static uint G(uint x, uint y, uint z)
        {
            return ((x & y) | (x & z) | (y & z));
        }

        private static uint H(uint x, uint y, uint z)
        {
            return (x ^ y ^ z);
        }

        private static uint RotateLeft(uint x, int n)
        {
            return (x << n | x >> (32 - n));
        }

        private static void FF(ref uint a, uint b, uint c, uint d, uint x, int s)
        {
            a += F(b, c, d) + x;
            a = RotateLeft(a, s);
        }

        private static void GG(ref uint a, uint b, uint c, uint d, uint x, int s)
        {
            a += G(b, c, d) + x + 0x5a827999;
            a = RotateLeft(a, s);
        }

        private static void HH(ref uint a, uint b, uint c, uint d, uint x, int s)
        {
            a += H(b, c, d) + x + 0x6ed9eba1;
            a = RotateLeft(a, s);
        }

        private void MD4Transform(byte[] block)
        {
            if (block.Length != MD4BlockSize)
            {
                throw new ArgumentException($"block should be a byte array of length {MD4BlockSize}.");
            }

            uint a = state[0], b = state[1], c = state[2], d = state[3];
            uint[] x = new uint[MD4BlockSize / sizeof(uint)];
            Buffer.BlockCopy(block, 0, x, 0, MD4BlockSize);

            /* Round 1 */
            FF(ref a, b, c, d, x[ 0], 3);
            FF(ref d, a, b, c, x[ 1], 7);
            FF(ref c, d, a, b, x[ 2], 11);
            FF(ref b, c, d, a, x[ 3], 19);
            FF(ref a, b, c, d, x[ 4], 3);
            FF(ref d, a, b, c, x[ 5], 7);
            FF(ref c, d, a, b, x[ 6], 11);
            FF(ref b, c, d, a, x[ 7], 19);
            FF(ref a, b, c, d, x[ 8], 3);
            FF(ref d, a, b, c, x[ 9], 7);
            FF(ref c, d, a, b, x[10], 11);
            FF(ref b, c, d, a, x[11], 19);
            FF(ref a, b, c, d, x[12], 3);
            FF(ref d, a, b, c, x[13], 7);
            FF(ref c, d, a, b, x[14], 11);
            FF(ref b, c, d, a, x[15], 19);

            /* Round 2 */
            GG(ref a, b, c, d, x[ 0], 3);
            GG(ref d, a, b, c, x[ 4], 5);
            GG(ref c, d, a, b, x[ 8], 9);
            GG(ref b, c, d, a, x[12], 13);
            GG(ref a, b, c, d, x[ 1], 3);
            GG(ref d, a, b, c, x[ 5], 5);
            GG(ref c, d, a, b, x[ 9], 9);
            GG(ref b, c, d, a, x[13], 13);
            GG(ref a, b, c, d, x[ 2], 3);
            GG(ref d, a, b, c, x[ 6], 5);
            GG(ref c, d, a, b, x[10], 9);
            GG(ref b, c, d, a, x[14], 13);
            GG(ref a, b, c, d, x[ 3], 3);
            GG(ref d, a, b, c, x[ 7], 5);
            GG(ref c, d, a, b, x[11], 9);
            GG(ref b, c, d, a, x[15], 13);

            /* Round 3 */
            HH(ref a, b, c, d, x[ 0], 3);
            HH(ref d, a, b, c, x[ 8], 9);
            HH(ref c, d, a, b, x[ 4], 11);
            HH(ref b, c, d, a, x[12], 15);
            HH(ref a, b, c, d, x[ 2], 3);
            HH(ref d, a, b, c, x[10], 9);
            HH(ref c, d, a, b, x[ 6], 11);
            HH(ref b, c, d, a, x[14], 15);
            HH(ref a, b, c, d, x[ 1], 3);
            HH(ref d, a, b, c, x[ 9], 9);
            HH(ref c, d, a, b, x[ 5], 11);
            HH(ref b, c, d, a, x[13], 15);
            HH(ref a, b, c, d, x[ 3], 3);
            HH(ref d, a, b, c, x[11], 9);
            HH(ref c, d, a, b, x[ 7], 11);
            HH(ref b, c, d, a, x[15], 15);

            state[0] += a;
            state[1] += b;
            state[2] += c;
            state[3] += d;
        }

        private void MD4Update(byte[] input, int inputLen)
        {
            int i, index, partLen;

            /* Compute number of bytes mod 64 */
            index = (int)(count[0] >> 3) & 0x3F;
            /* Update number of bits */
            count[0] += ((uint)inputLen << 3);
            if (count[0] < ((uint)inputLen << 3))
                count[1]++;
            count[1] += ((uint)inputLen >> 29);

            partLen = 64 - index;

            /* Transform as many times as possible */
            if (inputLen >= partLen)
            {
                Buffer.BlockCopy(input, 0, buffer, index, partLen);
                MD4Transform(buffer);

                for (i = partLen; i + 63 < inputLen; i += 64)
                {
                    byte[] block = new byte[MD4BlockSize];
                    Buffer.BlockCopy(input, i, block, 0, MD4BlockSize);
                    MD4Transform(block);
                }

                index = 0;
            }
            else
            {
                i = 0;
            }

            /* Buffer remaining input */
            Buffer.BlockCopy(input, i, buffer, index, inputLen - i);
        }
    }
}
