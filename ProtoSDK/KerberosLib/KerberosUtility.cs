// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.Spng;
using Microsoft.Protocols.TestTools.StackSdk.Asn1;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /// <summary>
    /// A static class provides some helper functions. It is called by KerberosPdu and KileDecoder.
    /// </summary>
    public static class KerberosUtility
    {
        #region Encryption / Decryption
        // The same as its overload that getToBeSignedDateCallback = null
        public static byte[] Encrypt(EncryptionType type, byte[] sessionKey, byte[] plainData, int usage)
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
        public static byte[] Encrypt(EncryptionType type, byte[] sessionKey, byte[] plainData, int usage, GetToBeSignedDataFunc getToBeSignedDateCallback)
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
        public static byte[] Decrypt(EncryptionType type, byte[] sessionKey, byte[] cipherData, int usage)
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
        public static byte[] Decrypt(EncryptionType type, byte[] sessionKey, byte[] cipherData, int usage, GetToBeSignedDataFunc getToBeSignedDateCallback)
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

        public static ChecksumType GetChecksumType(EncryptionType eType)
        {
            switch (eType)
            {
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                    return ChecksumType.hmac_sha1_96_aes128;
                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                    return ChecksumType.hmac_sha1_96_aes256;
                case EncryptionType.RC4_HMAC:
                    return ChecksumType.hmac_md5_string;
                case EncryptionType.DES_CBC_MD5:
                    return ChecksumType.rsa_md5;
                case EncryptionType.DES_CBC_CRC:
                    return ChecksumType.CRC32;
            }
            throw new NotImplementedException();
        }
        #endregion Encryption / Decryption

        #region Encode / Decode
        public static byte[] AddGssApiTokenHeader(KerberosApRequest request,
            KerberosConstValue.OidPkt oidPkt = KerberosConstValue.OidPkt.KerberosToken,
            KerberosConstValue.GSSToken gssToken = KerberosConstValue.GSSToken.GSSSPNG)
        {
            byte[] encoded = request.ToBytes();
            byte[] token = KerberosUtility.AddGssApiTokenHeader(ArrayUtility.ConcatenateArrays(
                BitConverter.GetBytes(KerberosUtility.ConvertEndian((ushort)TOK_ID.KRB_AP_REQ)), encoded), oidPkt, gssToken);

            return token;
        }

        /// <summary>
        /// Add a token header for AP request/response, wrap token or getmic token to make them a complete token.
        /// </summary>
        /// <param name="tokenBody">The AP request/response, wrap token or getmic token.
        /// This argument can be null.</param>
        /// <returns>The completed token.</returns>
        public static byte[] AddGssApiTokenHeader(byte[] tokenBody,
            KerberosConstValue.OidPkt oidPkt = KerberosConstValue.OidPkt.KerberosToken,
            KerberosConstValue.GSSToken gssToken = KerberosConstValue.GSSToken.GSSSPNG)
        {
            List<byte> gssDataList = new List<byte>();
            gssDataList.Add(KerberosConstValue.KERBEROS_TAG);

            // kerberos oid (1.2.840.113554.1.2.2)
            byte[] oid;
            oid = KerberosConstValue.GetKerberosOid();

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
                List<byte> lengthList = new List<byte>();
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

            if (gssToken == KerberosConstValue.GSSToken.GSSAPI)
                return gssDataList.ToArray();
            else
                return KerberosUtility.EncodeInitialNegToken(gssDataList.ToArray(), oidPkt);
        }


        /// <summary>
        /// Verify and remove a token header from AP request/response, 
        /// wrap token or getmic token.
        /// </summary>
        /// <param name="completeToken">The complete token got from application message.
        /// This argument can be null. If it is null, null will be returned.</param>
        /// <returns>The token body without the header.</returns>
        /// <exception cref="System.FormatException">Throw FormatException if the token header is incorrect.</exception> 
        public static byte[] VerifyGssApiTokenHeader(byte[] completeToken, KerberosConstValue.OidPkt oidPkt = KerberosConstValue.OidPkt.KerberosToken)
        {
            byte[] oid;
            oid = KerberosConstValue.GetKerberosOid();

            if (completeToken == null || completeToken.Length < KerberosConstValue.GetKerberosOid().Length)
            {
                throw new FormatException("The GSS-API token header is incomplete!");
            }

            if (completeToken[0] != KerberosConstValue.KERBEROS_TAG)
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
        public static int RoundUp(int length, int blockSize)
        {
            return (length + blockSize - 1) / blockSize * blockSize;
        }


        /// <summary>
        /// Wrap a length before the buffer.
        /// </summary>
        /// <param name="buffer">The buffer to be wrapped.</param>
        /// <param name="isBigEndian">Specify if the length is Big-Endian.</param>
        /// <returns>The buffer added length.</returns>
        public static byte[] WrapLength(byte[] buffer, bool isBigEndian)
        {
            if (isBigEndian)
            {
                return ArrayUtility.ConcatenateArrays(
                    BitConverter.GetBytes(KerberosUtility.ConvertEndian((uint)buffer.Length)), buffer);
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
        public static Asn1SequenceOf<KerberosString> String2SeqKerbString(params string[] sourceString)
        {
            if (sourceString == null)
            {
                return null;
            }

            List<KerberosString> kerberosStringList = new List<KerberosString>();
            foreach (string source in sourceString)
            {
                if (source != null)
                {
                    kerberosStringList.Add(new KerberosString(source));
                }
            }

            Asn1SequenceOf<KerberosString> seqString = new Asn1SequenceOf<KerberosString>(kerberosStringList.ToArray());
            return seqString;
        }

        public static string PrincipalName2String(PrincipalName name)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < name.name_string.Elements.Length; i++)
            {
                sb.Append(name.name_string.Elements[i]);
                if (i != name.name_string.Elements.Length - 1)
                    sb.Append("/");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Convert the flags in the format such as Hex string: "'0fa56920014abc'H"
        /// </summary>
        /// <param name="flags">an int value</param>
        /// <returns>A hex string of the flags.</returns>
        public static string ConvertInt2Flags(int flags)
        {
            string output = flags.ToString("x8");
            output = "'" + output + "'H";
            return output;
        }


        /// <summary>
        /// This method will be used to get the Current UTC Time of the client machine.
        /// </summary>
        /// <returns>current time in string </returns>
        public static string GetCurrentUTCTime()
        {
            DateTime currTime = DateTime.Now.ToUniversalTime();

            // year+Month+day+hour+minute+second
            return currTime.ToString("yyyyMMddHHmmss");
        }


        /// <summary>
        /// Get the current timestamp in the format of yyyyMMddhhmmss.
        /// </summary>
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
        public static byte[] GenerateRandomBytes(uint size)
        {
            byte[] randomBytes = new byte[size];
            Random random = new Random(DateTime.Now.Millisecond);
            random.NextBytes(randomBytes);
            return randomBytes;
        }


        /// <summary>
        /// Get Authorization Data from Ticket in AS/TGS response
        /// </summary>
        /// <param name="ticket">Ticket part that includes Auth Data.</param>
        /// <param name="key">The key that encrypts ticket part.</param>
        /// <returns>Authorization Data in the ticket</returns>
        public static AuthorizationData GetAuthDataInTicket(Ticket ticket, byte[] key)
        {
            EncryptionType encryptType = (EncryptionType)ticket.enc_part.etype.Value;
            byte[] clearText = KerberosUtility.Decrypt(
                      encryptType,
                      key,
                      ticket.enc_part.cipher.ByteArrayValue,
                      (int)KeyUsageNumber.AS_REP_TicketAndTGS_REP_Ticket);

            // Decode the ticket.
            Asn1DecodingBuffer decodeBuffer = new Asn1DecodingBuffer(clearText);
            EncTicketPart encTicketPart = new EncTicketPart();
            encTicketPart.BerDecode(decodeBuffer);

            return encTicketPart.authorization_data;
        }


        /// <summary>
        /// Update the Authorization Data part in Ticket with new value.
        /// </summary>
        /// <param name="ticket">Ticket to be updated with new authorizationData</param>
        /// <param name="key">The key that encrypts ticket part.</param>
        /// <param name="authorizationData">New authorizationData to update.</param>
        public static void UpdateAuthDataInTicket(Ticket ticket, byte[] key, AuthorizationData authorizationData)
        {
            EncryptionType encryptType = (EncryptionType)ticket.enc_part.etype.Value;
            byte[] clearText = KerberosUtility.Decrypt(
                      encryptType,
                      key,
                      ticket.enc_part.cipher.ByteArrayValue,
                      (int)KeyUsageNumber.AS_REP_TicketAndTGS_REP_Ticket);

            // Decode the ticket.
            Asn1DecodingBuffer decodeBuffer = new Asn1DecodingBuffer(clearText);
            EncTicketPart encTicketPart = new EncTicketPart();
            encTicketPart.BerDecode(decodeBuffer);

            // Set with new authorization data
            encTicketPart.authorization_data = authorizationData;
            Asn1BerEncodingBuffer ticketBerBuffer = new Asn1BerEncodingBuffer();
            encTicketPart.BerEncode(ticketBerBuffer, true);

            byte[] cipherData = KerberosUtility.Encrypt(
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
        public static ushort ConvertEndian(ushort num)
        {
            return (ushort)((num >> 8) | (num << 8));
        }


        /// <summary>
        /// Swap lower byte and higher byte.
        /// </summary>
        /// <param name="num">The number to be converted.</param>
        /// <returns>The converted number.</returns>
        public static uint ConvertEndian(uint num)
        {
            return (uint)(ConvertEndian((ushort)((num & 0xFFFF0000) >> 16))
                         | (ConvertEndian((ushort)(num & 0xFFFF)) << 16));
        }


        /// <summary>
        /// Swap lower byte and higher byte.
        /// </summary>
        /// <param name="num">The number to be converted.</param>
        /// <returns>The converted number.</returns>
        public static ulong ConvertEndian(ulong num)
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
        public static void RotateRight(byte[] buf, int rrc)
        {
            // Assume that the RRC value is 3 and the token before the rotation is
            // {"header" | aa | bb | cc | dd | ee | ff | gg | hh}.  The token after
            // rotation would be {"header" | ff | gg | hh | aa | bb | cc | dd | ee }, 
            // where {aa | bb | cc |...| hh} would be used to indicate the octet sequence.

            rrc = rrc % buf.Length;
            byte[] temp = ArrayUtility.ConcatenateArrays(ArrayUtility.SubArray(buf, buf.Length - rrc),
                                                         ArrayUtility.SubArray(buf, 0, buf.Length - rrc));
            Array.Copy(temp, buf, buf.Length);
        }


        /// <summary>
        /// Method to covert structure to byte[]
        /// </summary>
        /// <param name="structp">The structure prepare to covert</param>
        /// <returns>the got byte array converted from structure</returns>
        public static byte[] StructToBytes(object structp)
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
        /// Method to covert byte[] to structure.
        /// </summary>
        /// <param name="buffer">The buffer to be converted.</param>
        /// <returns>The structure.</returns>
        public static T ToStruct<T>(byte[] buffer)
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
        public static byte[] HMAC(byte[] key, byte[] buf)
        {
            return CryptoUtility.ComputeMd5Hmac(key, buf);
        }


        /// <summary>
        /// Compute HMAC.
        /// </summary>
        /// <param name="key">The key of HMAC.</param>
        /// <param name="n">The data to be computed.</param>
        /// <returns>The computed result.</returns>
        public static byte[] HMAC(byte[] key, int n)
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
        public static byte[] HMAC(byte[] key, string str)
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
        public static byte[] HMAC(byte[] key, string str, int n)
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
        public static byte[] RC4(byte[] key, byte[] data)
        {
            RC4CryptoServiceProvider rc4Enc = new RC4CryptoServiceProvider();
            byte[] result = new byte[data.Length];
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
        public static byte[] RC4HMAC(byte[] key, byte[] macContent, byte[] data, bool isExport)
        {
            byte[] Kseq = null;
            if (isExport)   // EncryptionType.RC4_HMAC_EXP
            {
                // Kseq = HMAC(Kss, "fortybits", (int32)0);
                Kseq = KerberosUtility.HMAC(key, KerberosConstValue.FORTY_BITS, 0);
                // memset(Kseq+7, 0xab, 9)
                for (int i = 0; i < 9; ++i)
                {
                    Kseq[i + 7] = 0xab;
                }
            }
            else            // EncryptionType.RC4_HMAC
            {
                Kseq = KerberosUtility.HMAC(key, 0);
            }
            Kseq = HMAC(Kseq, macContent);
            return KerberosUtility.RC4(Kseq, data);
        }


        /// <summary>
        /// Do MD2.5 described in [RFC 1964].
        /// </summary>
        /// <param name="key">The key of DES.</param>
        /// <param name="data">The data to be computed.</param>
        /// <returns>The computed result.</returns>
        public static byte[] MD2_5(byte[] key, byte[] data)
        {
            // The checksum is formed by first DES-CBC encrypting a
            // 16-byte zero-block, using a zero IV and a key formed by reversing the
            // bytes of the context key (i.e., if the original key is the 8-byte
            // sequence {aa, bb, cc, dd, ee, ff, gg, hh}, the checksum key will be
            // {hh, gg, ff, ee, dd, cc, bb, aa}). The resulting 16-byte value is
            // logically pre-pended to the "to-be-signed data".  A standard MD5
            // checksum is calculated over the combined data, and the first 8 bytes
            // of the result are stored in the SGN_CKSUM field.

            byte[] reversedKey = new byte[key.Length];
            Array.Copy(key, reversedKey, reversedKey.Length);
            Array.Reverse(reversedKey);
            byte[] preData = DesCbcEncrypt(reversedKey, new byte[KerberosConstValue.DES_BLOCK_SIZE], new byte[16]);

            byte[] result = ArrayUtility.ConcatenateArrays(preData, data);

            MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
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
        public static byte[] DesCbcMac(byte[] key, byte[] iv, byte[] data)
        {
            DESCryptoServiceProvider desEncrypt = new DESCryptoServiceProvider();
            desEncrypt.IV = iv;
            desEncrypt.Key = key;
            desEncrypt.Mode = CipherMode.CBC;
            desEncrypt.Padding = PaddingMode.Zeros;

            byte[] result = desEncrypt.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
            byte[] lastBlock = ArrayUtility.SubArray(result, result.Length - KerberosConstValue.DES_BLOCK_SIZE);
            return lastBlock;
        }


        /// <summary>
        /// Encrypt DES in CBC mode.
        /// </summary>
        /// <param name="key">The key of DES.</param>
        /// <param name="iv">The IV of DES.</param>
        /// <param name="data">The data to be computed.</param>
        /// <returns>The computed result.</returns>
        public static byte[] DesCbcEncrypt(byte[] key, byte[] iv, byte[] data)
        {
            DESCryptoServiceProvider desEncrypt = new DESCryptoServiceProvider();
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
        public static byte[] DesCbcDecrypt(byte[] key, byte[] iv, byte[] data)
        {
            DESCryptoServiceProvider desDecrypt = new DESCryptoServiceProvider();
            desDecrypt.IV = iv;
            desDecrypt.Key = key;
            desDecrypt.Mode = CipherMode.CBC;
            desDecrypt.Padding = PaddingMode.None;

            byte[] result = desDecrypt.CreateDecryptor().TransformFinalBlock(data, 0, data.Length);
            return result;
        }
        #endregion token [rfc4757] and [rfc1964]

        #region Packet api
        /// <summary>
        /// Generate client account's salt.
        /// </summary>
        /// <param name="domain">The realm part of the client's principal identifier.
        /// This argument cannot be null.</param>
        /// <param name="cName">The account to logon the remote machine. Either user account or computer account
        /// This argument cannot be null.</param>
        /// <param name="accountType">The type of the logon account. User or Computer</param>
        /// <returns>Client account's salt</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the input parameter is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when the account type is neither user nor computer.
        /// </exception>
        public static string GenerateSalt(string domain, string cName, KerberosAccountType accountType)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            if (cName == null)
            {
                throw new ArgumentNullException("cName");
            }
            string salt;

            if (accountType == KerberosAccountType.User)
            {
                salt = domain.ToUpper() + cName;
            }
            else if (accountType == KerberosAccountType.Device)
            {
                string computerName = cName;

                if (cName.EndsWith("$"))
                {
                    computerName = cName.Substring(0, cName.Length - 1);
                }
                salt = domain.ToUpper() + "host" + computerName.ToLower() + "." + domain.ToLower();
            }
            else
            {
                throw new NotSupportedException("Kile only support user or computer account.");
            }

            return salt;
        }
        #endregion

        #region Generate Key
        /// <summary>
        /// Make fast armor key
        /// </summary>
        /// <param name="type">Encryption type</param>
        /// <param name="subkey">Subkey, generated by client</param>
        /// <param name="sessionKey">Session key, generated by kdc</param>
        /// <returns></returns>
        public static byte[] MakeArmorKey(EncryptionType type, byte[] subkey, byte[] sessionKey)
        {
            return KeyGenerator.KrbFxCf2(type, subkey, sessionKey, "subkeyarmor", "ticketarmor");
        }

        public static EncryptionKey KrbFxCf2(EncryptionKey protocolKey1, EncryptionKey protocolKey2, string pepper1, string pepper2)
        {
            return new EncryptionKey(new KerbInt32(protocolKey1.keytype.Value),
                new Asn1OctetString(KeyGenerator.KrbFxCf2(
                    (EncryptionType)protocolKey1.keytype.Value,
                    protocolKey1.keyvalue.ByteArrayValue,
                    protocolKey2.keyvalue.ByteArrayValue,
                    pepper1,
                    pepper2)));
        }

        /// <summary>
        /// Generate a new key, the key type and key length are based on the input key
        /// </summary>
        /// <param name="baseKey">Base key for generation</param>
        /// <returns></returns>
        public static EncryptionKey GenerateKey(EncryptionKey baseKey)
        {
            if (baseKey == null || baseKey.keytype == null
                || baseKey.keyvalue == null || baseKey.keyvalue.Value == null)
            {
                return null;
            }

            byte[] keyBuffer = KerberosUtility.GenerateRandomBytes((uint)baseKey.keyvalue.ByteArrayValue.Length);
            EncryptionKey newKey = new EncryptionKey(new KerbInt32(baseKey.keytype.Value), new Asn1OctetString(keyBuffer));
            return newKey;
        }

        public static EncryptionKey MakeKey(EncryptionType type, string password, string salt)
        {
            EncryptionKey key = new EncryptionKey(new KerbInt32((long)type), new Asn1OctetString(KeyGenerator.MakeKey(type, password, salt)));
            return key;
        }
        #endregion

        #region Flags [RFC4120]
        /// <summary>
        /// Convert byte array Kerberos Flags to Integer
        /// </summary>
        /// <param name="flags">Kerberos Flags</param>
        /// <returns>Integer</returns>
        public static int ConvertFlags2Int(byte[] flags)
        {
            if (flags == null || flags.Length == 0)
                return 0;
            else
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < flags.Length; i++)
                {
                    sb.AppendFormat("{0:x2}", flags[i]);
                }
                return int.Parse(sb.ToString(), System.Globalization.NumberStyles.HexNumber);
            }
        }

        /// <summary>
        /// Compare two checksums. Return true if the same, otherwise return false.
        /// If one of the two checksum is null, return false.
        /// </summary>
        /// <param name="chksum1">A checksum.</param>
        /// <param name="chksum2">Another checksum.</param>
        /// <returns></returns>
        public static bool CompareChecksum(Checksum chksum1, Checksum chksum2)
        {
            if (chksum1 == null || chksum2 == null) return false;
            if (chksum1.cksumtype.Value != chksum2.cksumtype.Value) return false;
            if (chksum1.checksum.Value.Length != chksum2.checksum.Value.Length) return false;
            for (int i = 0; i < chksum1.checksum.Value.Length; i++)
            {
                if (chksum1.checksum.Value[i] != chksum2.checksum.Value[i]) return false;
            }
            return true;
        }
        #endregion

        #region Spng
        public static byte[] DecodeNegotiationToken(byte[] token)
        {
            NegotiationToken decoder = new NegotiationToken();
            decoder.BerDecode(new Asn1DecodingBuffer(token));

            Asn1Object type = decoder.GetData();

            switch (decoder.SelectedChoice)
            {
                case NegotiationToken.negTokenInit:
                    return ((NegTokenInit)type).mechToken.ByteArrayValue;
                case NegotiationToken.negTokenResp:
                    return ((NegTokenResp)type).responseToken.ByteArrayValue;
                case NegotiationToken.negTokenInit2:
                    return ((NegTokenInit2)type).mechToken.ByteArrayValue;
                default:
                    return null;
            }
        }

        public static T DecodeNegotiationToken<T>(byte[] token) where T : class
        {
            var negotiationToken = new NegotiationToken();
            negotiationToken.BerDecode(new Asn1DecodingBuffer(token));

            var data = negotiationToken.GetData();
            if (data is T)
            {
                return data as T;
            }
            else
            {
                return null;
            }
        }

        public static byte[] EncodeInitialNegToken(byte[] token,
            KerberosConstValue.OidPkt oidPkt)
        {
            int[] oidInt;
            if (oidPkt == KerberosConstValue.OidPkt.KerberosToken)
                oidInt = KerberosConstValue.GetKerberosOidInt();
            else if (oidPkt == KerberosConstValue.OidPkt.MSKerberosToken)
                oidInt = KerberosConstValue.GetMsKerberosOidInt();
            else
            {
                throw new NotSupportedException("oid not support");
            }

            MechTypeList mechTypeList = new MechTypeList(
                new MechType[]
                {
                    new MechType(oidInt)
                }
                );

            Asn1OctetString octetString = new Asn1OctetString(token);
            NegTokenInit init = new NegTokenInit(mechTypeList, null, new Asn1OctetString(octetString.ByteArrayValue), null);

            NegotiationToken negToken = new NegotiationToken(NegotiationToken.negTokenInit, init);

            MechType spnegoMech = new MechType(KerberosConstValue.GetSpngOidInt());
            InitialNegToken initToken = new InitialNegToken(spnegoMech, negToken);

            Asn1BerEncodingBuffer buffer = new Asn1BerEncodingBuffer();
            initToken.BerEncode(buffer);

            return buffer.Data;
        }

        #endregion

        #region Message Dumping
        /// <summary>
        /// Specify to what level should the message be dumped.
        /// </summary>
        public enum DumpLevel
        {
            /// <summary>
            /// The message is a whole message on the wire.
            /// </summary>
            WholeMessage = 0,
            /// <summary>
            /// The message is a part of a whole message.
            /// </summary>
            PartialMessage = 1
        }

        /// <summary>
        /// Handler of the Dump Message event
        /// </summary>
        /// <param name="messageName">Name of the message</param>
        /// <param name="messageDescription">Description of the message</param>
        /// <param name="dumpLevel">Dump Level</param>
        /// <param name="payload">A byte array representing
        /// the real data of the message</param>
        public delegate void DumpMessageEventHandler(string messageName,
            string messageDescription,
            DumpLevel dumpLevel,
            byte[] payload);

        /// <summary>
        /// Occur when messages are ready for dumping
        /// </summary>
        public static event DumpMessageEventHandler DumpMessage;

        /// <summary>
        /// Dump messages using the existing handler.
        /// If no handler exists, no action would be taken.
        /// </summary>
        /// <param name="messageName">Name of the message</param>
        /// <param name="messageDescription">Description of the message</param>
        /// <param name="dumpLevel">Dump Level</param>
        /// <param name="payload">A byte array representing
        /// the real data of the message</param>
        public static void OnDumpMessage(string messageName,
            string messageDescription,
            DumpLevel dumpLevel,
            byte[] payload)
        {
            if (DumpMessage != null)
            {
                DumpMessage(messageName, messageDescription,
                    dumpLevel, payload);
            }
        }
        #endregion
    }

}
