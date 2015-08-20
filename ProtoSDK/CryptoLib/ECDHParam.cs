// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Numerics;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// The encryption mode used for ECDH key exchange.
    /// </summary>
    public enum EcdhEncryptionMode
    {
        /// <summary>
        /// ECPRGF256Random | groupP-256 | secp256r1 in [RFC 5349].
        /// </summary>
        ECDH256,

        /// <summary>
        /// ECPRGF384Random | groupP-384 | secp384r1 in [RFC 5349].
        /// </summary>
        ECDH384,

        /// <summary>
        /// ECPRGF521Random | groupP-521 | secp521r1 in [RFC 5349].
        /// </summary>
        ECDH521,
    }


    /// <summary>
    /// Used to calculate parameters used for ECDH algorithm.
    /// </summary>
    public class ECDHParam
    {
        /// <summary>
        /// Represent one in MpInt.
        /// </summary>
        private static readonly BigInteger mpIntOne = new BigInteger(1);

        /// <summary>
        /// Represent two in MpInt.
        /// </summary>
        private static readonly BigInteger mpIntTwo = new BigInteger(2);

        /// <summary>
        /// Represent three in MpInt.
        /// </summary>
        private static readonly BigInteger mpIntThree = new BigInteger(3);

        /// <summary>
        /// Represent minus one in MpInt.
        /// </summary>
        private static readonly BigInteger mpIntMinusOne = new BigInteger(-1);

        /// <summary>
        /// Represent zero in MpInt.
        /// </summary>
        private static readonly BigInteger mpIntZero = new BigInteger(0);

        /// <summary>
        /// Parameter p in T = (p, a, b, G, n, h)
        /// </summary>
        private BigInteger prime;

        /// <summary>
        /// Parameter a in T = (p, a, b, G, n, h)
        /// </summary>
        private BigInteger parameterA;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="encryptMode">The Ecdh encryption mode</param>
        /// <exception cref="System.ArgumentException">Thrown when the encryption mode is not valid.</exception>
        public ECDHParam(EcdhEncryptionMode encryptMode)
        {
            if (encryptMode == EcdhEncryptionMode.ECDH256)
            {
                parameterA = Utility.LoadBytesLittleEndian(ConstValue.ECDH_P256_A);
                prime = Utility.LoadBytesLittleEndian(ConstValue.ECDH_P256_Prime);
            }
            else if (encryptMode == EcdhEncryptionMode.ECDH384)
            {
                parameterA = Utility.LoadBytesLittleEndian(ConstValue.ECDH_P384_A);
                prime = Utility.LoadBytesLittleEndian(ConstValue.ECDH_P384_Prime);
            }
            else if (encryptMode == EcdhEncryptionMode.ECDH521)
            {
                parameterA = Utility.LoadBytesLittleEndian(ConstValue.ECDH_P521_A);
                prime = Utility.LoadBytesLittleEndian(ConstValue.ECDH_P521_Prime);
            }
            else
            {
                throw new ArgumentException("The Ecdh encryption mode is not valid.");
            }
        }


        /// <summary>
        /// Calculate key for ECDH
        /// Refer to doc [SEC1] http://www.secg.org Section 2.2.1
        /// </summary>
        /// <param name="d">The private key</param>
        /// <param name="Q">The remote public key</param>
        /// <returns>The key.</returns>
        private BigInteger[] ComputeKey(BigInteger d, BigInteger[] Q)
        {
            if (d == mpIntOne)
            {
                return Q;
            }
            else if (d % mpIntTwo == BigInteger.Zero)
            {
                return ComputeKey(d / mpIntTwo, AddPToG(Q, Q));
            }
            else
            {
                return AddPToG(ComputeKey(d / mpIntTwo, AddPToG(Q, Q)), Q);
            }
        }


        /// <summary>
        /// Calculate key for ECDH, using formula Q = dG
        /// Refer to doc [SEC1] http://www.secg.org Section 2.2.1
        /// </summary>
        /// <param name="d">The private key</param>
        /// <param name="G">The remote public key Q or the parameter G</param>
        /// <returns>The local public key or the share key.</returns>
        private byte[] ComputeKey(byte[] d, byte[] G)
        {
            byte[] tempG = new byte[G.Length];
            Array.Copy(G, tempG, G.Length);
            Array.Reverse(tempG);
            byte[] X = ArrayUtility.SubArray(tempG, tempG.Length / 2);
            byte[] Y = ArrayUtility.SubArray(tempG, 0, tempG.Length / 2);
            Array.Resize(ref X, tempG.Length / 2 + 1);
            Array.Resize(ref Y, tempG.Length / 2 + 1);
            BigInteger d1 = Utility.LoadBytesLittleEndian(d);
            BigInteger Gx = new BigInteger(X);
            BigInteger Gy = new BigInteger(Y);
            BigInteger[] result = ComputeKey(d1, new BigInteger[] { Gx, Gy });
            byte[] Qx = result[0].ToByteArray();
            byte[] Qy = result[1].ToByteArray();

            if (Qx[Qx.Length - 1] == 0)
            {
                Array.Resize(ref Qx, Qx.Length - 1);
            }
            Array.Reverse(Qx);

            if (Qy[Qy.Length - 1] == 0)
            {
                Array.Resize(ref Qy, Qy.Length - 1);
            }
            Array.Reverse(Qy);

            if (Qx.Length < d.Length)
            {
                Qx = ArrayUtility.ConcatenateArrays(new byte[d.Length - Qx.Length], Qx);
            }
            if (Qy.Length < d.Length)
            {
                Qy = ArrayUtility.ConcatenateArrays(new byte[d.Length - Qy.Length], Qy);
            }

            return ArrayUtility.ConcatenateArrays(Qx, Qy);
        }


        /// <summary>
        /// Calculate key for ECDH
        /// Refer to doc [SEC1] http://www.secg.org Section 2.2.1
        /// </summary>
        /// <param name="d">The private key</param>
        /// <param name="Q">The remote public key. The length should be an even number</param>
        /// <returns>The share key. The value X direction of the point</returns>
        /// <exception cref="System.ArgumentException">Thrown when the parameters are not valid.</exception>
        public byte[] ComputeShareKey(byte[] d, byte[] Q)
        {
            if (d == null || Q == null || Q.Length % 2 != 0)
            {
                throw new ArgumentException(
                    "The input parameters is null or the length of remote public key is an odd number.");
            }
            byte[] shareKey = ComputeKey(d, Q);
            return ArrayUtility.SubArray(shareKey, 0, d.Length);
        }


        /// <summary>
        /// Calculate key for ECDH
        /// Refer to doc [SEC1] http://www.secg.org Section 2.2.1
        /// </summary>
        /// <param name="d">The private key</param>
        /// <param name="G">Parameter G in T = (p, a, b, G, n, h) in uncompressed form without prefix 04,
        /// defined in [SEC2]. The length should be an even number</param>
        /// <returns>The local public key.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the parameters are not valid.</exception>
        public byte[] ComputePublicKey(byte[] d, byte[] G)
        {
            if (d == null || G == null || G.Length % 2 != 0)
            {
                throw new ArgumentException(
                    "The input parameters is null or the length of parameter G is an odd number.");
            }
            return ComputeKey(d, G);
        }


        /// <summary>
        /// Add point P to G
        /// Refer to doc [SEC1] http://www.secg.org, Section 2.2.1
        /// </summary>
        /// <param name="P">The point P, including Px, Py.</param>
        /// <param name="Q">The point Q, including Qx, Qy.</param>
        /// <returns>The result after adding P to Q</returns>
        private BigInteger[] AddPToG(BigInteger[] P, BigInteger[] Q)
        {
            BigInteger Px = P[0];
            BigInteger Py = P[1];
            BigInteger Qx = Q[0];
            BigInteger Qy = Q[1];

            BigInteger coefficient;
            if (Px == Qx && Py == Qy)
            {
                BigInteger converse = GetConverse(mpIntTwo * Py, prime);
                coefficient = ECDHMod((mpIntThree * BigInteger.Pow(Px, 2) + parameterA) * converse, prime);
            }
            else
            {
                if (Qx >= Px)
                {
                    BigInteger converse = GetConverse(Qx - Px, prime);
                    coefficient = ECDHMod((Qy - Py) * converse, prime);
                }
                else
                {
                    BigInteger converse = GetConverse(Px - Qx, prime);
                    coefficient = ECDHMod((Py - Qy) * converse, prime);
                }
            }
            BigInteger temp = ECDHMod((BigInteger.Pow(coefficient, 2) - Px - Qx), prime);
            Py = ECDHMod((coefficient * (Px - temp) - Py), prime);
            Px = temp;
            return new BigInteger[] { Px, Py };
        }


        /// <summary>
        /// Get modulus in ECDH algorithm.
        /// </summary>
        /// <param name="baseNumber">The base number to calculate.</param>
        /// <param name="modulus">The parameter modulus.</param>
        /// <returns>The result</returns>
        private static BigInteger ECDHMod(BigInteger baseNumber, BigInteger modulus)
        {
            BigInteger result = BigInteger.Zero;
            if (baseNumber > BigInteger.Zero)
            {
                result = baseNumber % modulus;
            }
            else if (baseNumber < BigInteger.Zero)
            {
                BigInteger multiple = baseNumber * mpIntMinusOne / modulus;
                result = baseNumber + modulus * multiple;

                if (result < BigInteger.Zero)
                {
                    result += modulus;
                }
            }
            return result;
        }


        /// <summary>
        /// Get converse data for multiplication.
        /// Refer to Euclid's algorithm
        /// </summary>
        /// <param name="baseNumber">The base number to calculate.</param>
        /// <param name="modulus">The parameter modulus.</param>
        /// <returns>The converse data.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the converse data does exist.</exception>
        private static BigInteger GetConverse(BigInteger baseNumber, BigInteger modulus)
        {
            BigInteger[] arrayX = new BigInteger[] { mpIntOne, mpIntZero, modulus };
            BigInteger[] arrayY = new BigInteger[] { mpIntZero, mpIntOne, baseNumber };

            while (true)
            {
                if (arrayY[2] == mpIntZero)
                {
                    throw new ArgumentException("The converse number does not exist.");
                }
                if (arrayY[2] == mpIntOne)
                {
                    return arrayY[1];
                }
                BigInteger quotient = arrayX[2] / arrayY[2];
                BigInteger[] arrayT = new BigInteger[] { arrayX[0] - quotient * arrayY[0], arrayX[1] - quotient * arrayY[1],
                    arrayX[2] - quotient * arrayY[2] };
                arrayX = arrayY;
                arrayY = arrayT;
            }
        }
    }
}