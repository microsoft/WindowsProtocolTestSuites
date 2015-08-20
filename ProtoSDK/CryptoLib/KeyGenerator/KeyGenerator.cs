// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic
{
    /// <summary>
    /// Key Generator
    /// (called by KilePdu and KileDecoder)
    /// </summary>
    public static class KeyGenerator
    {
        /// <summary>
        /// Generate key according to password, salt and encryption type
        /// </summary>
        /// <param name="type">encryption type</param>
        /// <param name="password">password</param>
        /// <param name="salt">salt</param>
        /// <returns>the generated key in bytes</returns>
        public static byte[] MakeKey(EncryptionType type, string password, string salt)
        {
            switch (type)
            {
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                    {
                        return AesKey.MakeStringToKey(password, salt, 
                            AesKey.DEFAULT_ITERATION_COUNT, AesKeyType.Aes128BitsKey);
                    }

                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                    {
                        return AesKey.MakeStringToKey(password, salt, 
                            AesKey.DEFAULT_ITERATION_COUNT, AesKeyType.Aes256BitsKey);
                    }

                case EncryptionType.DES_CBC_CRC:
                case EncryptionType.DES_CBC_MD5:
                    {
                        return DesKey.MakeStringToKey(password, salt);
                    }

                case EncryptionType.RC4_HMAC:
                case EncryptionType.RC4_HMAC_EXP:
                    {
                        return Rc4Key.MakeStringToKey(password);
                    }

                default:
                    throw new ArgumentException("Unsupported encryption type.");
            }
        }

        public static byte[] KrbFxCf2(EncryptionType type, byte[] protocolKey1, byte[] protocolKey2, string pepper1, string pepper2)
        {
            byte[] octetString1 = null;
            byte[] octetString2 = null;
            switch (type)
            {
                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                    {
                        octetString1 = AesPrfPlus(protocolKey1, Encoding.UTF8.GetBytes(pepper1));
                        octetString2 = AesPrfPlus(protocolKey2, Encoding.UTF8.GetBytes(pepper2));
                        break;
                    }
                case EncryptionType.RC4_HMAC:
                    {
                        octetString1 = Rc4PrfPlus(protocolKey1, Encoding.UTF8.GetBytes(pepper1));
                        octetString2 = Rc4PrfPlus(protocolKey2, Encoding.UTF8.GetBytes(pepper2));
                        break;
                    }
                default:
                    throw new NotSupportedException("Encryption type not support");
            }

            return RandomToKey(type, ExclusiveOr(octetString1, octetString2));
        }

        private static byte[] AesPrfPlus(byte[] key, byte[] sharedInfo)
        {
            int prfPlusLength = key.Length;
            byte[] prfPlus = new byte[prfPlusLength];
            byte[] newSharedInfo = new byte[sharedInfo.Length + 1];
            byte counter = 1;
            int index = 0;
            while (true)
            {
                newSharedInfo[0] = counter;
                Buffer.BlockCopy(sharedInfo, 0, newSharedInfo, 1, sharedInfo.Length);

                byte[] prf = PseudoRandom(EncryptionType.AES256_CTS_HMAC_SHA1_96,key, newSharedInfo);
                //byte[] prf = AesKey.DR(key, newSharedInfo, AesKeyType.Aes256BitsKey);
                if (prfPlusLength >= ConstValue.AES_BLOCK_SIZE)
                {
                    Buffer.BlockCopy(prf, 0, prfPlus, index, ConstValue.AES_BLOCK_SIZE);
                    prfPlusLength -= ConstValue.AES_BLOCK_SIZE;
                    index += ConstValue.AES_BLOCK_SIZE;
                }

                else
                {
                    Buffer.BlockCopy(prf, 0, prfPlus, index, prfPlusLength);
                    break;
                }

                counter++;
                if (counter > 255)
                    throw new OverflowException();
            }

            return prfPlus;
        }

        private static byte[] Rc4PrfPlus(byte[] key, byte[] sharedInfo)
        {
            int prfPlusLength = key.Length;
            byte[] prfPlus = new byte[prfPlusLength];
            byte[] newSharedInfo = new byte[sharedInfo.Length + 1];
            byte counter = 1;
            int index = 0;
            while (true)
            {
                newSharedInfo[0] = counter;
                Buffer.BlockCopy(sharedInfo, 0, newSharedInfo, 1, sharedInfo.Length);

                byte[] prf = PseudoRandom(EncryptionType.RC4_HMAC, key, newSharedInfo);
                if (prfPlusLength >= prf.Length)
                {
                    Buffer.BlockCopy(prf, 0, prfPlus, index, prf.Length);
                    prfPlusLength -= prf.Length;
                    index += prf.Length;
                }

                else
                {
                    Buffer.BlockCopy(prf, 0, prfPlus, index, prfPlusLength);
                    break;
                }

                counter++;
                if (counter > 255)
                    throw new OverflowException();
            }

            return prfPlus;
        }
        
        private static byte[] PseudoRandom(EncryptionType type, byte[] protocolKey, byte[] sharedInfo)
        {
            switch (type)
            {
                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                    {
                        //tmp1 = H (octet-string)
                        // byte[] pepperBytes = Encoding.UTF8.GetBytes(pepper);
                        byte[] tmp1 = CryptoUtility.ComputeSha1(sharedInfo);
                        //byte[] tmp1 = UnkeyedChecksum.GetMic(pepperBytes, ChecksumType.sha1);

                        //tmp2 = truncate tmp1 to multiple of m
                        byte[] tmp2 = Truncate(type, tmp1);

                        //PRF = E (DK(protocol-key, prfconstant), tmp2, initial-cipher-state)
                        return PRF(type, protocolKey, tmp2);
                    }
                case EncryptionType.RC4_HMAC:
                    {
                        return CryptoUtility.ComputeHmacSha1(protocolKey, sharedInfo);
                    }
                default:
                    throw new NotSupportedException("Encryption type not support");
            }
        }

        private static byte[] ExclusiveOr(byte[] array1, byte[] array2)
        {
            byte[] result = new byte[array1.Length];
            for (int i = 0; i < array1.Length; i++)
                result[i] = Convert.ToByte(array1[i] ^ array2[i]);
            return result;
        }

        private static byte[] RandomToKey(EncryptionType type, byte[] random)
        {
            switch (type)
            {
                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                    {
                        return AesKey.RandomToKey(random);
                    }
                case EncryptionType.DES_CBC_CRC:
                case EncryptionType.DES_CBC_MD5:
                    {
                        return DesKey.DesRandomToKey(random);
                    }
                case EncryptionType.RC4_HMAC:
                    {
                        return Rc4Key.RandomToKey(random);
                    }
                default:
                    throw new NotSupportedException("Encryption type not support");
            }
        }
        
        private static byte[] Truncate(EncryptionType type, byte[] array)
        {
            int m = 0;
            switch (type)
            {
                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                    {
                        m = ConstValue.AES_BLOCK_SIZE;
                        break;
                    }
                case EncryptionType.DES_CBC_CRC:
                case EncryptionType.DES_CBC_MD5:
                    {
                        m = ConstValue.DES_BLOCK_SIZE;
                        break;
                    }
                default:
                    throw new NotSupportedException("Encryption type not support");
            }

            int length = (array.Length / m) * m;
            byte[] result = new byte[m];
            Buffer.BlockCopy(array, 0, result, 0, length);
            return result;
        }

        private static byte[] PRF(EncryptionType type, byte[] protocolKey, byte[] truncated)
        {            
            byte[] prf = Encoding.UTF8.GetBytes("prf");
            switch (type)
            {
                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                    {
                        byte[] temp = AesKey.DK(protocolKey, prf, AesKeyType.Aes256BitsKey);
                        byte[] initialVector = new byte[ConstValue.AES_BLOCK_SIZE];
                        CipherTextStealingMode aesCtsCrypto = CryptoUtility.CreateAesCtsCrypto(temp, initialVector);
                        byte[] encryptedData = aesCtsCrypto.EncryptFinal(truncated, 0, truncated.Length);
                        return encryptedData;                        
                    }
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                    {
                        byte[] temp = AesKey.DK(protocolKey, prf, AesKeyType.Aes128BitsKey);
                        break;
                    }

                default:
                    throw new NotSupportedException("Encryption type not support");
            }

            throw new NotImplementedException();
        }
    }
}