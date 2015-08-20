// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// A managed code implementation of Lm hash algorithm
    /// </summary>
    public class LMHashManaged : LMHash
    {
        private int inputPasswordLength;

        //Lm hash computes hash using 14 bytes password data.
        //if input password data is not 14 bytes, it will be
        //adjusted to 14 bytes
        private const int lmHashPasswordLength = 14;

        private byte[] lmHashPassword = new byte[lmHashPasswordLength];

        //the length of the key used in des encryption.
        private const int desKeyLength = 8;
        
        //lm hash will always compute hash using this value
        private readonly string lmHashData = "KGS!@#$%";


        /// <summary>
        /// Initialize LMHashManaged class
        /// </summary>
        public LMHashManaged()
        {
            // the hashsize in bit for this Algorithm is always 16*8 in bit.
            this.HashSizeValue = 16 * 8;
        }


        /// <summary>
        /// Initialize process of LMHash.
        /// </summary>
        public override void Initialize()
        {
            Array.Clear(lmHashPassword, 0, lmHashPassword.Length);
            inputPasswordLength = 0;
        }


        /// <summary>
        /// routes data written to the object into the hash algorithm for computing the hash.
        /// </summary>
        /// <param name="array">The input to compute the hash code for.</param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
        /// <param name="cbSize">The number of bytes in the byte array to use as data.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            // this function actually does not do hash.
            // it prepares the key. 

            if (inputPasswordLength < lmHashPasswordLength)
            {
                if (cbSize < lmHashPasswordLength - inputPasswordLength)
                {
                    Array.Copy(array, ibStart, lmHashPassword, 0, cbSize);
                    inputPasswordLength += cbSize;
                }
                else
                {
                    Array.Copy(array, ibStart, lmHashPassword, 0, lmHashPasswordLength - inputPasswordLength);
                    inputPasswordLength = lmHashPassword.Length;
                }
            }
            else
            {
                // ignore the data
            }
        }


        /// <summary>
        /// finalizes the hash computation after
        /// the last data is processed by the cryptographic stream object.
        /// </summary>
        /// <returns>The computed hash code.</returns>
        protected override byte[] HashFinal()
        {
            byte[] hashData = InternalComputeHash(Encoding.ASCII.GetString(lmHashPassword, 0, inputPasswordLength));

            // should initialize data before next round hash process
            Initialize();

            return hashData;
        }


        /// <summary>
        /// Compute hash for the password
        /// </summary>
        /// <param name="password">the password</param>
        /// <returns>the computed hash code</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5351:DESCannotBeUsed")]
        private byte[] InternalComputeHash(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            //prepare password, padding to 14 bytes if not enough, otherwise cutting the part exceeding 14 bytes
            password = password.ToUpper();
            byte[] passwordBuffer;
            if (password.Length > lmHashPasswordLength)
            {
                passwordBuffer = Encoding.ASCII.GetBytes(password.ToCharArray(), 0, lmHashPasswordLength);
            }
            else
            {
                byte[] temp = Encoding.ASCII.GetBytes(password);
                passwordBuffer = new byte[lmHashPasswordLength];
                Array.Copy(temp, passwordBuffer, temp.Length);
            }

            //insert zero ervery 7 bits to generate two keys used in Des encrypt
            byte[] password1 = GenerateDesKey(passwordBuffer, 0);
            //get the key starting from half of passwordArray's length position.
            byte[] password2 = GenerateDesKey(passwordBuffer, passwordBuffer.Length/2);

            //use Des with ECB CipherMode and NONE padding option to encrypt "KGS!@#$%"
            byte[] hashDataBuffer = Encoding.ASCII.GetBytes(this.lmHashData);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.None;

            // DES class performs weak key check so that some short-length keys may fail
            // We call its method to create ICryptoTransform directly to bypass the check.
            const string methodName = "_NewEncryptor";
            object[] desParams = new object[] { password1, des.Mode, null, des.FeedbackSize, 0 };
            ICryptoTransform ct1 = (ICryptoTransform)ObjectUtility.InvokeMethod(des, methodName, desParams);
            byte[] hash1 = ct1.TransformFinalBlock(hashDataBuffer, 0, hashDataBuffer.Length);

            desParams[0] = password2;
            ICryptoTransform ct2 = (ICryptoTransform)ObjectUtility.InvokeMethod(des, methodName, desParams);
            byte[] hash2 = ct2.TransformFinalBlock(hashDataBuffer, 0, hashDataBuffer.Length);

            //concatenate the two encrypted text to one.
            byte[] ConcatenatedArray = Helper.ConcatenateByteArrays(hash1, hash2);

            return ConcatenatedArray;
        }


        /// <summary>
        /// Generate des key by insert 0 bit every 7 bits
        /// </summary>
        /// <param name="password">the password data</param>
        /// <param name="offset">the offset from where converting start</param>
        /// <returns>the generated 8-bytes des key</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow")]
        public static byte[] GenerateDesKey(byte[] password, int offset)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", "offset should be larger than or equal to zero");
            }

            if ((offset + 7) > password.Length)
            {
                throw new ArgumentException("there should be at least 7 bytes can be used to generate des key",
                    "offset");
            }

            byte[] key = new byte[desKeyLength];
            byte a = 0;
            byte b = 0;

            for (int i = 0; i < (key.Length - 1); i++)
            {
                //caculate content of the key from continuous two bytes of password
                //0xfe means to get the high 7-bits data.
                b = password[i + offset];
                key[i] = (byte)(((a << (desKeyLength - i)) + (b >> i)) & 0xfe);
                a = b;
            }
            //caculate the last byte of the key;
            key[desKeyLength - 1] = (byte)(password[desKeyLength - 2 + offset] << 1);

            return key;
        }
    }
}
