// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// Group types of diffie-hellman key exchange. It extends oakley groups, adding ECP256 and ECP384.
    /// </summary>
    public enum DhGroupType
    {
        /// <summary>
        /// Default 768-bit MODP group [RFC2412]
        /// </summary>
        MODP768Bit,

        /// <summary>
        /// Alternate 1024-bit MODP group [RFC2412]
        /// </summary>
        MODP1024Bit,

        /// <summary>
        /// 2048-bit MODP group [RFC3526]
        /// </summary>
        MODP2048Bit,

        /// <summary>
        /// ECP_256 [ECP]
        /// </summary>
        ECP256,

        /// <summary>
        /// ECP_384 [ECP]
        /// </summary>
        ECP384,
    }


    /// <summary>
    /// Oakley class implements Diffie-hellman key exchange algorithm.
    /// </summary>
    public class Oakley : DiffieHellman
    {
        #region Members

        // MODP768 Bit Group, this prime is the hex string of 
        // 2^768 - 2 ^704 - 1 + 2^64 * { [2^638 pi] + 149686 }
        // as defined in [RFC 2412]
        private const string Modp768BitPrimeString =
            "155251809230070893513091813125848175563133404943" +
            "451431320235119490296623994910210725866945387659" + 
            "164244291000768028886422915080371891804634263272" +
            "761303128298374438082089019628850917069131659317" +
            "5367469551763119843371637221007210577919";

        // MODP768 Bit Group, this prime is the hex string of 
        // 2^1024 - 2^960 - 1 + 2^64 * { [2^894 pi] + 129093 }
        // as defined in [RFC 2412]
        private const string Modp1024BitPrimeString =
            "179769313486231590770839156793787453197860296048" +
            "756011706444423684197180216158519368947833795864" +
            "925541502180565485980503646440548199239100050792" +
            "877003355816639229553136239076508735759914822574" +
            "862575007425302077447712589550957937778424442426" +
            "617334727629299387668709205606050270810842907692" +
            "932019128194467627007";

        // MODP1024 Bit Group, this prime is the hex string of
        // 2^2048 - 2^1984 - 1 + 2^64 * { [2^1918 pi] + 124476 }
        // as defined in [RFC 3526]
        private const string Modp2048BitPrimeString =
            "323170060713110073003389139264238282488179412411" +
            "402391128420097514007417066343542226196894173635" +
            "693471179017379097041917546058732091950288537589" +
            "861856221532121754125149017745202702357960782362" +
            "488842461894775876411059286460994117232454266225" +
            "221932305409190376805242355191256797158701170010" +
            "580558776510388618472802579760549035697325615261" +
            "670813393617995413364765591603683178967290731783" +
            "845896806396719009772021941686472258710314113364" +
            "293195361934716365332097170774482279885885653692" +
            "086452966360772502689555059283627511211740969729" +
            "980684105543595848665832916421362182310789909994" +
            "48652468262416972035911852507045361090559";

        private const int bitLength = 1024;

        // Length of random exponents, in bits.
        private int randomExponentLength;

        // Exponent used to calculate the initiator key.
        private BigInteger exponent;

        // The big prime value of the DH group, used to calculate the key.
        private BigInteger prime;

        // The base value used to generate key exchange data.
        private BigInteger baseNum;

        #endregion Members

        #region Public Methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="groupType">DH group defined in [RFC2412] and MS-AIPS.</param>
        public Oakley(DhGroupType groupType)
        {
            // The prime used for DH group
            string primeString = null;

            // Generator of the prime, both 768 bit and 1024 bit use the same value.

            switch (groupType)
            {
                case DhGroupType.MODP768Bit:
                    primeString = Modp768BitPrimeString;
                    break;
                case DhGroupType.MODP1024Bit:
                    primeString = Modp1024BitPrimeString;
                    break;
                case DhGroupType.MODP2048Bit:
                    primeString = Modp2048BitPrimeString;
                    break;
                case DhGroupType.ECP256:
                case DhGroupType.ECP384:
                    throw new NotImplementedException();
                default:
                    throw new InvalidOperationException("Invalid DH group type");
            }

            this.randomExponentLength = bitLength;
            this.exponent = RandomBigInteger(randomExponentLength);
            this.prime = BigInteger.Parse(primeString);
            this.baseNum = new BigInteger(new byte[] { 0x02 });
        }


        /// <summary>
        /// Gets Key exchange data by DH group.
        /// </summary>
        /// <returns>Key data</returns>
        public override byte[] GenerateKeyData()
        {
            byte[] result = BigInteger.ModPow(baseNum, exponent, prime).ToByteArray();

            if (result[result.Length - 1] == 0)
            {
                Array.Resize(ref result, result.Length - 1);
            }
            Array.Reverse(result);

            return result;
        }


        /// <summary>
        /// Generates a key with key exchange data.
        /// </summary>
        /// <param name="keyData">Key exchange data.</param>
        /// <exception cref="ArgumentNullException">Raised when keyData is null.</exception>
        /// <exception cref="StackException">Raised when exponent or prime is not initialized.</exception>
        /// <returns>Generated key.</returns>
        public override byte[] GenerateKey(byte[] keyData)
        {
            if (keyData == null)
            {
                throw new ArgumentNullException("keyData");
            }
            if (exponent.IsZero || prime.IsZero)
            {
                throw new InvalidOperationException("exponent or prime not properly initiated");
            }

            BigInteger basePrime = Utility.LoadBytesLittleEndian(keyData);

            byte[] result = BigInteger.ModPow(basePrime, exponent, prime).ToByteArray();

            if (result[result.Length - 1] == 0)
            {
                Array.Resize(ref result, result.Length - 1);
            }
            Array.Reverse(result);

            return result;
        }
        #endregion Public Methods

        #region Helper methods

        /// <summary>
        /// Create a random BigInteger
        /// </summary>
        /// <param name="bits">The BigInteger bits count</param>
        /// <returns>A random BigInteger</returns>
        private static BigInteger RandomBigInteger(int bits)
        {
            Random random = new Random();
            byte[] buffer = new byte[bits >> 3];
            random.NextBytes(buffer);
            buffer[buffer.Length - 1] = 0;

            return new BigInteger(buffer);
        }

        #endregion Helper methods
    }
}
