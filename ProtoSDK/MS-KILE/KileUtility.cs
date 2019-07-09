// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Kile
{
    /// <summary>
    /// A static class provides some helper functions. It is called by KilePdu and KileDecoder.
    /// </summary>
    public static class KileUtility
    {
        #region Encryption / Decryption
        // The same as its overload that getToBeSignedDateCallback = null
        internal static byte[] Encrypt(EncryptionType type, byte[] sessionKey, byte[] plainData, int usage)
        {
            return Encrypt(type, sessionKey, plainData, usage, null);
        }

        /// <summary>
        /// Encrypt the plain text with the session key which can be obtained 
        /// from response message. The usage indicated in section 7 of RFC4210 
        /// and section 3 of RFC4757 is used to derive key from the session key.
        /// </summary>
        /// <param name="type">The encryption type selected.</param>
        /// <param name="sessionKey">A session key used to encrypt and it can be obtained
        /// from the KDC's response. This key size should be equal to the symmetric algorithm
        /// key size. This argument can be null. If it is null, null will be returned.</param>
        /// <param name="plainData">The text to be encrypted. This argument can be null. 
        /// If it is null, null will be returned.</param>
        /// <param name="usage">A 32 bits integer used to derive the key.</param>
        /// <param name="getToBeSignedDateCallback">
        /// A callback to get to-be-signed data. 
        /// The method will use plain-text data directly if this parameter is null.
        /// </param>
        /// <returns>The cipher text.</returns>
        internal static byte[] Encrypt(EncryptionType type, byte[] sessionKey, byte[] plainData, int usage, GetToBeSignedDataFunc getToBeSignedDateCallback)
        {
            switch (type)
            {
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                    return AesCtsHmacSha1Crypto.Encrypt(sessionKey, plainData, usage, AesKeyType.Aes128BitsKey, getToBeSignedDateCallback);

                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                    return AesCtsHmacSha1Crypto.Encrypt(sessionKey, plainData, usage, AesKeyType.Aes256BitsKey, getToBeSignedDateCallback);

                case EncryptionType.DES_CBC_CRC:
                    return DesCbcCrypto.Encrypt(sessionKey, plainData, EncryptionType.DES_CBC_CRC, getToBeSignedDateCallback);

                case EncryptionType.DES_CBC_MD5:
                    return DesCbcCrypto.Encrypt(sessionKey, plainData, EncryptionType.DES_CBC_MD5, getToBeSignedDateCallback);

                case EncryptionType.RC4_HMAC:
                    return Rc4HmacCrypto.Encrypt(sessionKey, plainData, usage, EncryptionType.RC4_HMAC, getToBeSignedDateCallback);

                case EncryptionType.RC4_HMAC_EXP:
                    return Rc4HmacCrypto.Encrypt(sessionKey, plainData, usage, EncryptionType.RC4_HMAC_EXP, getToBeSignedDateCallback);

                default:
                    throw new ArgumentException("Unsupported encryption type.");
            }
        }


        // The same as its overload that getToBeSignedDateCallback = null
        internal static byte[] Decrypt(EncryptionType type, byte[] sessionKey, byte[] cipherData, int usage)
        {
            return Decrypt(type, sessionKey, cipherData, usage, null);
        }

        /// <summary>
        /// Decrypt the cipher text with the session key. The usage indicated
        /// in section 7 of RFC4210 and section 3 of RFC4757 is used to derive key 
        /// from the session key.
        /// </summary>
        /// <param name="type">The decryption type selected.</param>
        /// <param name="sessionKey">An session key used to decrypt and it can be obtained
        /// from the KDC's response. This key size should be equal to the symmetric algorithm
        /// key size. This argument can be null. If it is null, null will be returned.</param>
        /// <param name="cipherData">The text to be decrypted. This argument can be null. 
        /// If it is null, null will be returned.</param>
        /// <param name="usage">A 32 bits integer used to derive the key.</param>
        /// <param name="getToBeSignedDateCallback">
        /// A callback to get to-be-signed data. 
        /// The method will use decrypted data directly if this parameter is null.
        /// </param>
        /// <returns>The plain text.</returns>
        internal static byte[] Decrypt(EncryptionType type, byte[] sessionKey, byte[] cipherData, int usage, GetToBeSignedDataFunc getToBeSignedDateCallback)
        {
            switch (type)
            {
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                    return AesCtsHmacSha1Crypto.Decrypt(sessionKey, cipherData, usage, AesKeyType.Aes128BitsKey, getToBeSignedDateCallback);

                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                    return AesCtsHmacSha1Crypto.Decrypt(sessionKey, cipherData, usage, AesKeyType.Aes256BitsKey, getToBeSignedDateCallback);

                case EncryptionType.DES_CBC_CRC:
                    return DesCbcCrypto.Decrypt(sessionKey, cipherData, EncryptionType.DES_CBC_CRC, getToBeSignedDateCallback);

                case EncryptionType.DES_CBC_MD5:
                    return DesCbcCrypto.Decrypt(sessionKey, cipherData, EncryptionType.DES_CBC_MD5, getToBeSignedDateCallback);

                case EncryptionType.RC4_HMAC:
                    return Rc4HmacCrypto.Decrypt(sessionKey, cipherData, usage, EncryptionType.RC4_HMAC, getToBeSignedDateCallback);

                case EncryptionType.RC4_HMAC_EXP:
                    return Rc4HmacCrypto.Decrypt(sessionKey, cipherData, usage, EncryptionType.RC4_HMAC_EXP, getToBeSignedDateCallback);

                default:
                    throw new ArgumentException("Unsupported encryption type.");
            }
        }


        /// <summary>
        /// Generate checksum supported by MS-KILE
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="input">input data</param>
        /// <param name="usage">key usage number</param>
        /// <param name="checksumType">checksum type</param>
        /// <returns>the caculated checksum</returns>
        /// <exception cref="ArgumentException">thrown if the checksum type is not supported</exception>
        public static byte[] GetChecksum(
            byte[] key, 
            byte[] input, 
            int usage, 
            ChecksumType checksumType)
        {
            switch (checksumType)
            {
                case ChecksumType.CRC32:
                case ChecksumType.rsa_md4:
                case ChecksumType.rsa_md5:
                case ChecksumType.sha1:
                    return UnkeyedChecksum.GetMic(input, checksumType);

                case ChecksumType.hmac_sha1_96_aes128:
                    return HmacSha1AesChecksum.GetMic(
                        key, input, usage, AesKeyType.Aes128BitsKey);

                case ChecksumType.hmac_sha1_96_aes256:
                    return HmacSha1AesChecksum.GetMic(
                        key, input, usage, AesKeyType.Aes256BitsKey);

                case ChecksumType.hmac_md5_string:
                    return HmacMd5StringChecksum.GetMic(key, input, usage);

                default:
                    throw new ArgumentException("Unsupported checksum type.");
            }
        }
        #endregion Encryption / Decryption


        #region Encode / Decode
        /// <summary>
        /// Add a token header for AP request/response, wrap token or getmic token to make them a complete token.
        /// </summary>
        /// <param name="tokenBody">The AP request/response, wrap token or getmic token.
        /// This argument can be null.</param>
        /// <returns>The completed token.</returns>
        internal static byte[] AddGssApiTokenHeader(byte[] tokenBody)
        {
            var gssDataList = new List<byte>();
            gssDataList.Add(ConstValue.KERBEROS_TAG);

            // kerberos oid (1.2.840.113554.1.2.2)
            byte[] oid = ConstValue.KERBEROS_OID;

            if (tokenBody == null)
            {
                tokenBody = new byte[0];
            }

            int length = tokenBody.Length + oid.Length;
            if (length > 127)
            {
                // If the indicated value is 128 or more, it shall be represented in two or more octets,
                // with bit 8 of the first octet set to "1" and the remaining bits of the first octet
                // specifying the number of additional octets. The subsequent octets carry the value,
                // 8 bits per octet, most significant digit first. [rfc 2743]
                int temp = length;
                int index = 1;
                var lengthList = new List<byte>();
                lengthList.Add((byte)(temp & 0xFF));
                while ((temp >>= 8) != 0)
                {
                    index++;
                    lengthList.Add((byte)(temp & 0xFF));
                }

                gssDataList.Add((byte)(0x80 | index));
                lengthList.Reverse();
                gssDataList.AddRange(lengthList.ToArray());
            }
            else
            {
                // If the indicated value is less than 128, it shall be represented in a single octet with bit 8 
                // (high order) set to "0" and the remaining bits representing the value. [rfc 2743]
                gssDataList.Add((byte)length);
            }

            gssDataList.AddRange(oid);
            gssDataList.AddRange(tokenBody);

            return gssDataList.ToArray();
        }


        /// <summary>
        /// Verify and remove a token header from AP request/response, 
        /// wrap token or getmic token.
        /// </summary>
        /// <param name="completeToken">The complete token got from application message.
        /// This argument can be null. If it is null, null will be returned.</param>
        /// <returns>The token body without the header.</returns>
        /// <exception cref="System.FormatException">Throw FormatException if the token header is incorrect.</exception> 
        internal static byte[] VerifyGssApiTokenHeader(byte[] completeToken)
        {
            // kerberos oid (1.2.840.113554.1.2.2)
            byte[] oid = ConstValue.KERBEROS_OID;

            if (completeToken == null || completeToken.Length < ConstValue.KERBEROS_OID.Length)
            {
                throw new FormatException("The GSS-API token header is incomplete!");
            }

            if (completeToken[0] != ConstValue.KERBEROS_TAG)
            {
                throw new FormatException("The GSS-API token header is incorrect!");
            }

            int length = 0;
            int index = 2;   // the tag and length fields
            // If the length value is 128 or more.
            if ((completeToken[1] & 0x80) == 0x80)
            {
                // If the indicated value is 128 or more, it shall be represented in two or more octets,
                // with bit 8 of the first octet set to "1" and the remaining bits of the first octet
                // specifying the number of additional octets. The subsequent octets carry the value,
                // 8 bits per octet, most significant digit first.
                int num = completeToken[1] & 0x7F;
                index += num;

                if (num < 1 || num > 4)
                {
                    throw new FormatException("The GSS-API token length is incorrect!");
                }

                for (int i = 0; i < num; ++i)
                {
                    length = (length << 8) | completeToken[2 + i];
                }
            }
            else
            {
                // If the indicated value is less than 128, it shall be represented in a single octet with bit 8 
                // (high order) set to "0" and the remaining bits representing the value. [rfc 2743]
                length = completeToken[1];
            }

            if (!ArrayUtility.CompareArrays(oid, ArrayUtility.SubArray(completeToken, index, oid.Length)))
            {
                throw new FormatException("The GSS-API token oid is incorrect!");
            }

            byte[] tokenBody = ArrayUtility.SubArray(completeToken, index + oid.Length);
            return tokenBody;
        }


        /// <summary>
        /// Compute the multiple size of the input length.
        /// </summary>
        /// <param name="length">The input length.</param>
        /// <param name="blockSize">The block size.</param>
        /// <returns>The multiple size of the input length.</returns>
        internal static int RoundUp(int length, int blockSize)
        {
            return (length + blockSize - 1) / blockSize * blockSize;
        }


        /// <summary>
        /// Wrap a length before the buffer.
        /// </summary>
        /// <param name="buffer">The buffer to be wrapped.</param>
        /// <param name="isBigEndian">Specify if the length is Big-Endian.</param>
        /// <returns>The buffer added length.</returns>
        internal static byte[] WrapLength(byte[] buffer, bool isBigEndian)
        {
            if (isBigEndian)
            {
                return ArrayUtility.ConcatenateArrays(
                    BitConverter.GetBytes(KileUtility.ConvertEndian((uint)buffer.Length)), buffer);
            }
            else
            {
                return ArrayUtility.ConcatenateArrays(BitConverter.GetBytes(buffer.Length), buffer);
            }
        }


        /// <summary>
        /// convert string to _SeqOfKerberosString
        /// </summary>
        /// <param name="sourceString">source string that will be converted.</param>
        /// <returns>a _SeqOfKerberosString string</returns>
        internal static Asn1SequenceOf<KerberosString> String2SeqKerbString(params string[] sourceString)
        {
            if (sourceString == null)
            {
                return null;
            }

            var kerberosStringList = new List<KerberosString>();
            foreach (string source in sourceString)
            {
                if (source != null)
                {
                    kerberosStringList.Add(new KerberosString(source));
                }
            }

            var seqString = new Asn1SequenceOf<KerberosString>(kerberosStringList.ToArray());
            return seqString;
        }


        /// <summary>
        /// Convert the flags in the format such as Hex string: "'0fa56920014abc'H"
        /// </summary>
        /// <param name="flags">an int value</param>
        /// <returns>A hex string of the flags.</returns>
        internal static string ConvertInt2Flags(int flags)
        {
            string output = flags.ToString("x8");
            output = "'" + output + "'H";
            return output;
        }


        /// <summary>
        /// This method will be used to get the Current UTC Time of the client machine.
        /// </summary>
        /// <returns>current time in string </returns>
        internal static string GetCurrentUTCTime()
        {
            DateTime currTime = DateTime.Now.ToUniversalTime();

            // year+Month+day+hour+minute+second
            return currTime.ToString("yyyyMMddHHmmss");
        }


        /// <summary>
        /// Get the current timestamp in the format of yyyyMMddhhmmss.
        /// </summary>
        [CLSCompliant(false)]
        public static KerberosTime CurrentKerberosTime
        {
            get
            {
                string timeStamp = GetCurrentUTCTime() + "Z";
                return new KerberosTime(timeStamp, true);
            }
        }


        /// <summary>
        /// Generate a random byte array.
        /// </summary>
        /// <param name="size">The size of byte array.</param>
        /// <returns>The bytes generated.</returns>
        internal static byte[] GenerateRandomBytes(uint size)
        {
            var randomBytes = new byte[size];
            var random = new Random(DateTime.Now.Millisecond);
            random.NextBytes(randomBytes);
            return randomBytes;
        }


        /// <summary>
        /// Get Authorization Data from Ticket in AS/TGS response
        /// </summary>
        /// <param name="ticket">Ticket part that includes Auth Data.</param>
        /// <param name="key">The key that encrypts ticket part.</param>
        /// <returns>Authorization Data in the ticket</returns>
        [CLSCompliant(false)]
        public static AuthorizationData GetAuthDataInTicket(Ticket ticket, byte[] key)
        {
            EncryptionType encryptType = (EncryptionType)ticket.enc_part.etype.Value;
            byte[] clearText = KileUtility.Decrypt(
                      encryptType,
                      key,
                      ticket.enc_part.cipher.ByteArrayValue,
                      (int)KeyUsageNumber.AS_REP_TicketAndTGS_REP_Ticket);

            // Decode the ticket.
            var decodeBuffer = new Asn1DecodingBuffer(clearText);
            var encTicketPart = new EncTicketPart();
            encTicketPart.BerDecode(decodeBuffer);

            return encTicketPart.authorization_data;
        }


        /// <summary>
        /// Update the Authorization Data part in Ticket with new value.
        /// </summary>
        /// <param name="ticket">Ticket to be updated with new authorizationData</param>
        /// <param name="key">The key that encrypts ticket part.</param>
        /// <param name="authorizationData">New authorizationData to update.</param>
        [CLSCompliant(false)]
        public static void UpdateAuthDataInTicket(Ticket ticket, byte[] key, AuthorizationData authorizationData)
        {
            EncryptionType encryptType = (EncryptionType)ticket.enc_part.etype.Value;
            byte[] clearText = KileUtility.Decrypt(
                      encryptType,
                      key,
                      ticket.enc_part.cipher.ByteArrayValue,
                      (int)KeyUsageNumber.AS_REP_TicketAndTGS_REP_Ticket);

            // Decode the ticket.
            var decodeBuffer = new Asn1DecodingBuffer(clearText);
            var encTicketPart = new EncTicketPart();
            encTicketPart.BerDecode(decodeBuffer);

            // Set with new authorization data
            encTicketPart.authorization_data = authorizationData;
            var ticketBerBuffer = new Asn1BerEncodingBuffer();
            encTicketPart.BerEncode(ticketBerBuffer, true);

            byte[] cipherData = KileUtility.Encrypt(
                encryptType,
                key,
                ticketBerBuffer.Data,
                (int)KeyUsageNumber.AS_REP_TicketAndTGS_REP_Ticket);
            ticket.enc_part = new EncryptedData(new KerbInt32((int)encryptType), null, new Asn1OctetString(cipherData));
        }

        #endregion Encode / Decode


        #region token [rfc4121]
        /// <summary>
        /// Swap lower byte and higher byte.
        /// </summary>
        /// <param name="num">The number to be converted.</param>
        /// <returns>The converted number.</returns>
        internal static ushort ConvertEndian(ushort num)
        {
            return (ushort)((num >> 8) | (num << 8));
        }


        /// <summary>
        /// Swap lower byte and higher byte.
        /// </summary>
        /// <param name="num">The number to be converted.</param>
        /// <returns>The converted number.</returns>
        internal static uint ConvertEndian(uint num)
        {
            return (uint)(ConvertEndian((ushort)((num & 0xFFFF0000) >> 16))
                         | (ConvertEndian((ushort)(num & 0xFFFF)) << 16));
        }


        /// <summary>
        /// Swap lower byte and higher byte.
        /// </summary>
        /// <param name="num">The number to be converted.</param>
        /// <returns>The converted number.</returns>
        internal static ulong ConvertEndian(ulong num)
        {
            const ulong mask = 0xFFFFFFFF00000000;
            return ConvertEndian((uint)((num & mask) >> 32))
                   | ((ulong)ConvertEndian((uint)(num & ~mask)) << 32);
        }


        /// <summary>
        /// do right rotation for a buffer.
        /// </summary>
        /// <param name="buf">buffer to do right rotation</param>
        /// <param name="rrc">right rotation count</param>
        internal static void RotateRight(byte[] buf, int rrc)
        {
            // Assume that the RRC value is 3 and the token before the rotation is
            // {"header" | aa | bb | cc | dd | ee | ff | gg | hh}. The token after
            // rotation would be {"header" | ff | gg | hh | aa | bb | cc | dd | ee }, 
            // where {aa | bb | cc |...| hh} would be used to indicate the octet sequence.

            rrc = rrc % buf.Length;
            byte[] temp = ArrayUtility.ConcatenateArrays(ArrayUtility.SubArray(buf, buf.Length - rrc),
                                                         ArrayUtility.SubArray(buf, 0, buf.Length - rrc));
            Array.Copy(temp, buf, buf.Length);
        }


        /// <summary>
        /// Method to covert struct to byte[]
        /// </summary>
        /// <param name="structp">The struct prepare to covert</param>
        /// <returns>the got byte array converted from struct</returns>
        internal static byte[] StructToBytes(object structp)
        {
            IntPtr ptr = IntPtr.Zero;
            byte[] buffer = null;

            try
            {
                int size = Marshal.SizeOf(structp.GetType());
                ptr = Marshal.AllocHGlobal(size);
                buffer = new byte[size];
                Marshal.StructureToPtr(structp, ptr, false);
                Marshal.Copy(ptr, buffer, 0, size);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }

            return buffer;
        }


        /// <summary>
        /// Method to covert byte[] to struct.
        /// </summary>
        /// <param name="buffer">The buffer to be converted.</param>
        /// <returns>The structure.</returns>
        internal static T ToStruct<T>(byte[] buffer)
        {
            T ret;
            IntPtr intPtr = IntPtr.Zero;

            try
            {
                intPtr = Marshal.AllocHGlobal(buffer.Length);
                Marshal.Copy(buffer, 0, intPtr, buffer.Length);
                ret = (T)Marshal.PtrToStructure(intPtr, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(intPtr);
            }

            return ret;
        }
        #endregion token [rfc4121]


        #region token [rfc4757] and [rfc1964]
        /// <summary>
        /// Compute HMAC.
        /// </summary>
        /// <param name="key">The key of HMAC.</param>
        /// <param name="buf">The data to be computed.</param>
        /// <returns>The computed result.</returns>
        internal static byte[] HMAC(byte[] key, byte[] buf)
        {
            return CryptoUtility.ComputeMd5Hmac(key, buf);
        }


        /// <summary>
        /// Compute HMAC.
        /// </summary>
        /// <param name="key">The key of HMAC.</param>
        /// <param name="n">The data to be computed.</param>
        /// <returns>The computed result.</returns>
        internal static byte[] HMAC(byte[] key, int n)
        {
            byte[] buf = BitConverter.GetBytes(n);
            return HMAC(key, buf);
        }


        /// <summary>
        /// Compute HMAC.
        /// </summary>
        /// <param name="key">The key of HMAC.</param>
        /// <param name="str">The data to be computed.</param>
        /// <returns>The computed result.</returns>
        internal static byte[] HMAC(byte[] key, string str)
        {
            byte[] buf = System.Text.Encoding.ASCII.GetBytes(str);
            return HMAC(key, buf);
        }


        /// <summary>
        /// Compute HMAC.
        /// </summary>
        /// <param name="key">The key of HMAC.</param>
        /// <param name="str">The data to be computed.</param>
        /// <param name="n">The data to be computed.</param>
        /// <returns>The computed result.</returns>
        internal static byte[] HMAC(byte[] key, string str, int n)
        {
            byte[] buf = System.Text.Encoding.ASCII.GetBytes(str);
            return HMAC(key, ArrayUtility.ConcatenateArrays(buf, BitConverter.GetBytes(n)));
        }


        /// <summary>
        /// Compute RC4.
        /// </summary>
        /// <param name="key">The key of RC4.</param>
        /// <param name="data">The data to be computed.</param>
        /// <returns>The computed result.</returns>
        internal static byte[] RC4(byte[] key, byte[] data)
        {
            var rc4Enc = new RC4CryptoServiceProvider();
            var result = new byte[data.Length];
            ICryptoTransform rc4Encrypt = rc4Enc.CreateEncryptor(key, null);
            rc4Encrypt.TransformBlock(data, 0, data.Length, result, 0);
            return result;
        }


        /// <summary>
        /// Compute RC4HMAC.
        /// </summary>
        /// <param name="key">The key to do RC4HMAC.</param>
        /// <param name="macContent">The content to do HMAC.</param>
        /// <param name="data">The data to be computed.</param>
        /// <param name="isExport">True for EncryptionType.RC4_HMAC_EXP, false for EncryptionType.RC4_HMAC.</param>
        /// <returns>The computed result.</returns>
        internal static byte[] RC4HMAC(byte[] key, byte[] macContent, byte[] data, bool isExport)
        {
            byte[] Kseq = null;
            if (isExport)   // EncryptionType.RC4_HMAC_EXP
            {
                // Kseq = HMAC(Kss, "fortybits", (int32)0);
                Kseq = KileUtility.HMAC(key, ConstValue.FORTY_BITS, 0);
                // memset(Kseq+7, 0xab, 9)
                for (int i = 0; i < 9; ++i)
                {
                    Kseq[i + 7] = 0xab;
                }
            }
            else            // EncryptionType.RC4_HMAC
            {
                Kseq = KileUtility.HMAC(key, 0);
            }
            Kseq = HMAC(Kseq, macContent);
            return KileUtility.RC4(Kseq, data);
        }


        /// <summary>
        /// Do MD2.5 described in [RFC 1964].
        /// </summary>
        /// <param name="key">The key of DES.</param>
        /// <param name="data">The data to be computed.</param>
        /// <returns>The computed result.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5350:MD5CannotBeUsed")]                
        internal static byte[] MD2_5(byte[] key, byte[] data)
        {
            // The checksum is formed by first DES-CBC encrypting a
            // 16-byte zero-block, using a zero IV and a key formed by reversing the
            // bytes of the context key (i.e., if the original key is the 8-byte
            // sequence {aa, bb, cc, dd, ee, ff, gg, hh}, the checksum key will be
            // {hh, gg, ff, ee, dd, cc, bb, aa}). The resulting 16-byte value is
            // logically pre-pended to the "to-be-signed data". A standard MD5
            // checksum is calculated over the combined data, and the first 8 bytes
            // of the result are stored in the SGN_CKSUM field.

            var reversedKey = new byte[key.Length];
            Array.Copy(key, reversedKey, reversedKey.Length);
            Array.Reverse(reversedKey);
            byte[] preData = DesCbcEncrypt(reversedKey, new byte[ConstValue.DES_BLOCK_SIZE], new byte[16]);

            byte[] result = ArrayUtility.ConcatenateArrays(preData, data);

            var md5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] md5 = md5CryptoServiceProvider.ComputeHash(result);
            return md5;
        }


        /// <summary>
        /// Do DES-CBC MAC.
        /// </summary>
        /// <param name="key">The key of DES.</param>
        /// <param name="iv">The IV of DES.</param>
        /// <param name="data">The data to be computed.</param>
        /// <returns>The computed result.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5351:DESCannotBeUsed")]       
        internal static byte[] DesCbcMac(byte[] key, byte[] iv, byte[] data)
        {
            var desEncrypt = new DESCryptoServiceProvider();
            desEncrypt.IV = iv;
            desEncrypt.Key = key;
            desEncrypt.Mode = CipherMode.CBC;
            desEncrypt.Padding = PaddingMode.Zeros;

            byte[] result = desEncrypt.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
            byte[] lastBlock = ArrayUtility.SubArray(result, result.Length - ConstValue.DES_BLOCK_SIZE);
            return lastBlock;
        }


        /// <summary>
        /// Encrypt DES in CBC mode.
        /// </summary>
        /// <param name="key">The key of DES.</param>
        /// <param name="iv">The IV of DES.</param>
        /// <param name="data">The data to be computed.</param>
        /// <returns>The computed result.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5351:DESCannotBeUsed")]               
        internal static byte[] DesCbcEncrypt(byte[] key, byte[] iv, byte[] data)
        {
            var desEncrypt = new DESCryptoServiceProvider();
            desEncrypt.IV = iv;
            desEncrypt.Key = key;
            desEncrypt.Mode = CipherMode.CBC;
            desEncrypt.Padding = PaddingMode.None;

            byte[] result = desEncrypt.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
            return result;
        }


        /// <summary>
        /// Decrypt DES in CBC mode.
        /// </summary>
        /// <param name="key">The key of DES.</param>
        /// <param name="iv">The IV of DES.</param>
        /// <param name="data">The data to be computed.</param>
        /// <returns>The computed result.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security.Cryptography", "CA5351:DESCannotBeUsed")]               
        internal static byte[] DesCbcDecrypt(byte[] key, byte[] iv, byte[] data)
        {
            var desDecrypt = new DESCryptoServiceProvider();
            desDecrypt.IV = iv;
            desDecrypt.Key = key;
            desDecrypt.Mode = CipherMode.CBC;
            desDecrypt.Padding = PaddingMode.None;

            byte[] result = desDecrypt.CreateDecryptor().TransformFinalBlock(data, 0, data.Length);
            return result;
        }
        #endregion token [rfc4757] and [rfc1964]


        #region Security Context Methods

        /// <summary>
        /// This takes the given SecurityBuffer array, signs data part, and updates signature into token part
        /// </summary>
        /// <param name="kileRole">Represents client or server</param>
        /// <param name="securityBuffers">Data to sign and token to update.</param>
        /// <exception cref="System.ArgumentException">Thrown when the data or token is not valid.</exception>
        internal static void Sign(KileRole kileRole, params SecurityBuffer[] securityBuffers)
        {
            byte[] token = SspiUtility.ConcatenateReadWriteSecurityBuffers(securityBuffers, SecurityBufferType.Token);

            if (token.Length == 0)
            {
                throw new ArgumentException("No token can be updated for signature.");
            }
            byte[] message = GetToBeSignedDataFromSecurityBuffers(securityBuffers);
            SGN_ALG sgnAlg = GetSgnAlg(kileRole);
            KilePdu pdu = kileRole.GssGetMic(sgnAlg, message);
            byte[] signature = pdu.ToBytes();

            SspiUtility.UpdateSecurityBuffers(securityBuffers, SecurityBufferType.Token, signature);
        }


        /// <summary>
        /// This takes the given byte array and verifies it using the SSPI VerifySignature method.
        /// </summary>
        /// <param name="kileRole">Represents client or server</param>
        /// <param name="securityBuffers">Data and token to verify</param>
        /// <returns>Success if true, Fail if false</returns>
        /// <exception cref="System.ArgumentException">Thrown when the data or token is not valid.</exception>
        internal static bool Verify(KileRole kileRole, params SecurityBuffer[] securityBuffers)
        {
            KilePdu pdu;
            return kileRole.GssVerifyMicEx(securityBuffers, out pdu);
        }


        /// <summary>
        /// Get the data to be signed.
        /// </summary>
        /// <param name="securityBuffers">The security buffer to extract the data to be signed</param>
        /// <returns>The data to be signed</returns>
        internal static byte[] GetToBeSignedDataFromSecurityBuffers(SecurityBuffer[] securityBuffers)
        {
            if (securityBuffers == null)
            {
                throw new ArgumentNullException(nameof(securityBuffers));
            }
            var message = new byte[0];

            for (int i = 0; i < securityBuffers.Length; i++)
            {
                if (securityBuffers[i] == null)
                {
                    throw new ArgumentNullException(nameof(securityBuffers));
                }
                SecurityBufferType securityBufferType = (securityBuffers[i].BufferType & ~SecurityBufferType.AttrMask);

                if (securityBufferType == SecurityBufferType.Data || securityBufferType == SecurityBufferType.Padding)
                {
                    bool skip = (securityBuffers[i].BufferType & SecurityBufferType.ReadOnly) != 0;

                    if (!skip && securityBuffers[i].Buffer != null)
                    {
                        message = ArrayUtility.ConcatenateArrays(message, securityBuffers[i].Buffer);
                    }
                }
            }

            return message;
        }


        /// <summary>
        /// Encrypts Message. User decides what SecBuffers are used.
        /// </summary>
        /// <param name="kileRole">Represents client or server</param>
        /// <param name="securityBuffers">The security buffers to encrypt.</param>
        /// <exception cref="System.ArgumentException">Thrown when the data or token is not valid.</exception>
        internal static void Encrypt(KileRole kileRole, params SecurityBuffer[] securityBuffers)
        {
            byte[] message = SspiUtility.ConcatenateReadWriteSecurityBuffers(securityBuffers, SecurityBufferType.Data);
            byte[] token = SspiUtility.ConcatenateSecurityBuffers(securityBuffers, SecurityBufferType.Token);

            if (token.Length == 0)
            {
                throw new ArgumentException("Token buffer is not valid.");
            }

            SGN_ALG sgnAlg = GetSgnAlg(kileRole);
            KilePdu pdu = kileRole.GssWrap(true, sgnAlg, message);

            byte[] cipherData = null;

            if (pdu.GetType() == typeof(Token4121))
            {
                cipherData = pdu.ToBytes();
            }
            else
            {
                byte[] allData = pdu.ToBytes();
                byte[] paddingData = ((Token1964_4757)pdu).paddingData;
                cipherData = ArrayUtility.SubArray(allData, 0, allData.Length - paddingData.Length);
                SspiUtility.UpdateSecurityBuffers(securityBuffers, SecurityBufferType.Padding, paddingData);
            }
            SspiUtility.UpdateSecurityBuffers(securityBuffers, SecurityBufferType.Data,
                ArrayUtility.SubArray(cipherData, cipherData.Length - message.Length));
            SspiUtility.UpdateSecurityBuffers(securityBuffers, SecurityBufferType.Token,
                ArrayUtility.SubArray(cipherData, 0, cipherData.Length - message.Length));
        }


        /// <summary>
        /// This takes the given byte array, decrypts it, and returns
        /// the original, unencrypted byte array.
        /// </summary>
        /// <param name="kileRole">Represents client or server</param>
        /// <param name="securityBuffers">The security buffers to decrypt.</param>
        /// <exception cref="System.ArgumentException">Thrown when the data or token is not valid.</exception>
        internal static bool Decrypt(KileRole kileRole, params SecurityBuffer[] securityBuffers)
        {
            KilePdu pdu = kileRole.GssUnWrapEx(securityBuffers);
            byte[] decryptedMessage = null;

            if (pdu.GetType() == typeof(Token4121))
            {
                decryptedMessage = ((Token4121)pdu).Data;
            }
            else if (pdu.GetType() == typeof(Token1964_4757))
            {
                Token1964_4757 tokenData = (Token1964_4757)pdu;
                decryptedMessage = tokenData.Data;
                SspiUtility.UpdateSecurityBuffers(securityBuffers, SecurityBufferType.Padding, tokenData.paddingData);
            }
            // else do nothing

            return true;
        }

        #endregion


        #region private methods

        /// <summary>
        /// Get SGN_ALG for encryption and sign.
        /// </summary>
        /// <returns>The SGN_ALG got from context.</returns>
        /// <exception cref="System.FormatException">Thrown when the key is not valid.</exception>
        private static SGN_ALG GetSgnAlg(KileRole kileRole)
        {
            EncryptionKey key = kileRole.Context.ContextKey;

            if (key == null || key.keytype == null || key.keyvalue == null || key.keyvalue.Value == null)
            {
                throw new FormatException("Initialization is not complete successfully!");
            }

            SGN_ALG sgnAlg = SGN_ALG.HMAC;
            EncryptionType type = (EncryptionType)key.keytype.Value;

            if (type == EncryptionType.DES_CBC_MD5 || type == EncryptionType.DES_CBC_CRC)
            {
                sgnAlg = SGN_ALG.DES_MAC_MD5;
            }

            return sgnAlg;
        }

        #endregion
    }
}
