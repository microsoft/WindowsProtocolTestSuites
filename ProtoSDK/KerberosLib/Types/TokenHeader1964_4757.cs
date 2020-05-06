// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /// <summary>
    /// Token header of rfc[1964] and rfc[4757].
    /// </summary>
    public struct TokenHeader1964_4757
    {
        /// <summary>
        /// Identification field.
        /// </summary>
        public TOK_ID tok_id;

        /// <summary>
        /// Checksum algorithm indicator.
        ///  00 00 - DES MAC MD5
        ///  01 00 - MD2.5
        ///  02 00 - DES MAC
        /// </summary>
        public SGN_ALG sng_alg;

        /// <summary>
        /// ff ff - none
        /// 00 00 - DES
        /// </summary>
        public SEAL_ALG seal_alg;

        /// <summary>
        /// Contains ff ff.
        /// </summary>
        public ushort filler;

        /// <summary>
        /// Encrypted sequence number field. 8 bytes.
        /// </summary>
        public byte[] snd_seq;

        /// <summary>
        /// Checksum of plaintext padded data, calculated according to algorithm specified in SGN_ALG field.8 bytes.
        /// </summary>
        public byte[] sng_cksum;
    }

    public class Token1964_4757 : KerberosPdu
    {
        /// <summary>
        /// The token header.
        /// </summary>
        private TokenHeader1964_4757 tokenHeader;

        /// <summary>
        /// The orignial data.
        /// </summary>
        private byte[] data;

        private KerberosContext Context;

        /// <summary>
        /// Padding data
        /// </summary>
        internal byte[] paddingData;

        /// <summary>
        /// The token header.
        /// </summary>
        public TokenHeader1964_4757 TokenHeader
        {
            get
            {
                return tokenHeader;
            }
            set
            {
                tokenHeader = value;
            }
        }

        /// <summary>
        /// The orignial data.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        /// <summary>
        /// Create an instance.
        /// </summary>
        /// <param name="kileContext">The context of the client or server.</param>
        public Token1964_4757(KerberosContext context)
        {
            this.Context = context;
        }


        /// <summary>
        /// Decode the KILE PDU from the message bytes.
        /// </summary>
        /// <param name="buffer">The byte array to be decoded.</param>
        /// <exception cref="System.FormatException">thrown when the format of input buffer is not correct</exception>
        public override void FromBytes(byte[] buffer)
        {
            byte[] tokenBody = KerberosUtility.VerifyGssApiTokenHeader(buffer);
            if (tokenBody == null || tokenBody.Length < KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE)
            {
                throw new FormatException("The token body is incomplete!");
            }

            TOK_ID tokId = (TOK_ID)KerberosUtility.ConvertEndian(BitConverter.ToUInt16(tokenBody, 0));

            if (tokId == TOK_ID.Wrap1964_4757)
            {
                int tokenSize = buffer.Length - tokenBody.Length;
                tokenSize += KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE + KerberosConstValue.CHECKSUM_SIZE_RFC1964 + KerberosConstValue.SEQUENCE_NUMBER_SIZE;
                if (tokId == TOK_ID.Wrap1964_4757)
                {
                    tokenSize += KerberosConstValue.CONFOUNDER_SIZE;
                }

                if (buffer.Length < tokenSize)
                {
                    throw new FormatException("The token body is incomplete!");
                }

                FromSecurityBuffers(
                    new SecurityBuffer(SecurityBufferType.Token, ArrayUtility.SubArray(buffer, 0, tokenSize)),
                    new SecurityBuffer(SecurityBufferType.Data, ArrayUtility.SubArray(buffer, tokenSize)));
            }
            else if (tokId == TOK_ID.Mic1964_4757)
            {
                FromSecurityBuffers(
                    new SecurityBuffer(SecurityBufferType.Token, buffer),
                    new SecurityBuffer(SecurityBufferType.Data, data));
            }
            else
            {
                throw new FormatException("Other tok_id is not supported.");
            }
        }


        /// <summary>
        /// Decode the KILE PDU from the security buffers.
        /// </summary>
        /// <param name="securityBuffers">Security buffers</param>
        public void FromSecurityBuffers(params SecurityBuffer[] securityBuffers)
        {
            byte[] tokenBody = SspiUtility.ConcatenateSecurityBuffers(securityBuffers, SecurityBufferType.Token);
            tokenBody = KerberosUtility.VerifyGssApiTokenHeader(tokenBody);
            if (tokenBody == null
                || tokenBody.Length <
                (KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE + KerberosConstValue.CHECKSUM_SIZE_RFC1964 + KerberosConstValue.SEQUENCE_NUMBER_SIZE))
            {
                throw new FormatException("The token body is incomplete!");
            }

            byte[] header = ArrayUtility.SubArray(tokenBody, 0, KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE);
            byte[] sequence = ArrayUtility.SubArray(tokenBody,
                                                    KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE,
                                                    KerberosConstValue.SEQUENCE_NUMBER_SIZE);
            byte[] checksum = ArrayUtility.SubArray(tokenBody,
                KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE + KerberosConstValue.SEQUENCE_NUMBER_SIZE,
                KerberosConstValue.CHECKSUM_SIZE_RFC1964);
            byte[] confounder = new byte[0];

            // Get fields from byte array. The numbers below are the offset of these fields.
            tokenHeader = new TokenHeader1964_4757();
            tokenHeader.tok_id = (TOK_ID)KerberosUtility.ConvertEndian(BitConverter.ToUInt16(header, 0));
            tokenHeader.sng_alg = (SGN_ALG)KerberosUtility.ConvertEndian(BitConverter.ToUInt16(header, 2));
            tokenHeader.seal_alg = (SEAL_ALG)KerberosUtility.ConvertEndian(BitConverter.ToUInt16(header, 4));
            tokenHeader.filler = KerberosUtility.ConvertEndian(BitConverter.ToUInt16(header, 6));

            #region verify header
            if (tokenHeader.tok_id != TOK_ID.Wrap1964_4757 && tokenHeader.tok_id != TOK_ID.Mic1964_4757)
            {
                throw new FormatException("The token ID is incorrect! tok_id = " + (ushort)tokenHeader.tok_id);
            }

            if (tokenHeader.sng_alg != SGN_ALG.DES_MAC && tokenHeader.sng_alg != SGN_ALG.DES_MAC_MD5
                && tokenHeader.sng_alg != SGN_ALG.MD2_5 && tokenHeader.sng_alg != SGN_ALG.HMAC)
            {
                throw new FormatException("The token sng_alg is incorrect! sng_alg = " + (ushort)tokenHeader.sng_alg);
            }

            if (tokenHeader.seal_alg != SEAL_ALG.DES && tokenHeader.seal_alg != SEAL_ALG.RC4
                && tokenHeader.seal_alg != SEAL_ALG.NONE)
            {
                throw new FormatException("The token seal_alg is incorrect! seal_alg = "
                    + (ushort)tokenHeader.seal_alg);
            }

            if (tokenHeader.filler != KerberosConstValue.TOKEN_FILLER_2_BYTE)
            {
                throw new FormatException("The token filler is incorrect! filler = " + tokenHeader.filler);
            }
            #endregion verify header

            byte[] cipher = null;
            if (tokenHeader.tok_id == TOK_ID.Wrap1964_4757)   // wrap token
            {
                // the tokenBody must >= header + sequnce number + checksum + confounder
                if (tokenBody.Length !=
                (KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE + KerberosConstValue.CHECKSUM_SIZE_RFC1964 + KerberosConstValue.SEQUENCE_NUMBER_SIZE
                 + KerberosConstValue.CONFOUNDER_SIZE))
                {
                    throw new FormatException("The token body is incomplete!");
                }
                confounder = ArrayUtility.SubArray(
                    tokenBody,
                    KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE + KerberosConstValue.CHECKSUM_SIZE_RFC1964 + KerberosConstValue.SEQUENCE_NUMBER_SIZE);
                cipher = ArrayUtility.ConcatenateArrays(
                    confounder,
                    SspiUtility.ConcatenateReadWriteSecurityBuffers(securityBuffers, SecurityBufferType.Data, SecurityBufferType.Padding));
            }
            else  // mic token, the data is given by user
            {
                if (tokenBody.Length !=
                (KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE + KerberosConstValue.CHECKSUM_SIZE_RFC1964 + KerberosConstValue.SEQUENCE_NUMBER_SIZE))
                {
                    throw new FormatException("The token body is incomplete!");
                }
            }

            #region decrypt data
            EncryptionKey key = Context.SessionKey;
            bool isExport = (EncryptionType)key.keytype.Value == EncryptionType.RC4_HMAC_EXP;
            byte[] plainText = data;
            if (tokenHeader.seal_alg == SEAL_ALG.DES)
            {
                // The key used is derived from the established context key by XOR-ing the context key 
                // with the hexadecimal constant f0f0f0f0f0f0f0f0.
                byte[] Klocal = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    Klocal[i] = (byte)(key.keyvalue.Value[i] ^ 0xF0);
                }
                // The data is encrypted using DES-CBC, with an IV of zero.
                plainText = KerberosUtility.DesCbcDecrypt(Klocal, new byte[8], cipher);
            }
            else if (tokenHeader.seal_alg == SEAL_ALG.RC4)
            {
                // for (i = 0; i < 16; i++) Klocal[i] = Kss[i] ^ 0xF0;
                byte[] Klocal = new byte[16];
                for (int i = 0; i < 16; i++)
                {
                    Klocal[i] = (byte)(key.keyvalue.Value[i] ^ 0xF0);
                }
                //byte[] seqBuffer = BitConverter.GetBytes((uint)Context);
                //Array.Reverse(seqBuffer);
                //plainText = KerberosUtility.RC4HMAC(Klocal, seqBuffer, cipher, isExport);
            }
            else
            {
                if (tokenHeader.tok_id == TOK_ID.Wrap1964_4757)
                {
                    plainText = cipher;
                }
            }

            if (tokenHeader.tok_id == TOK_ID.Wrap1964_4757)
            {
                // The plaintext data is padded to the next highest multiple of 8 bytes, 
                // by appending between 1 and 8 bytes, the value of each such byte being
                // the total number of pad bytes.  For example, given data of length 20
                // bytes, four pad bytes will be appended, and each byte will contain
                // the hex value 04.  
                // no padding is possible.
                byte pad = plainText[plainText.Length - 1];
                if (pad > 8 || pad < 1)
                {
                    pad = 0;
                }
                for (int i = 1; i < pad; ++i)
                {
                    if (plainText[plainText.Length - i - 1] != pad)
                    {
                        pad = 0;
                    }
                }
                paddingData = new byte[pad];

                for (int i = 0; i < paddingData.Length; i++)
                {
                    paddingData[i] = pad;
                }

                confounder = ArrayUtility.SubArray(plainText, 0, KerberosConstValue.CONFOUNDER_SIZE);

                plainText = ArrayUtility.SubArray(plainText,
                                             KerberosConstValue.CONFOUNDER_SIZE,
                                             plainText.Length - KerberosConstValue.CONFOUNDER_SIZE);
                SspiUtility.UpdateSecurityBuffers(
                    securityBuffers,
                    new SecurityBufferType[] { SecurityBufferType.Data, SecurityBufferType.Padding },
                    plainText);

                data = ArrayUtility.SubArray(plainText, 0, plainText.Length - pad);
            }
            #endregion decrypt data

            #region verify checksum
            byte[] check = null;
            byte[] toChecksumText = KerberosUtility.GetToBeSignedDataFromSecurityBuffers(securityBuffers);
            toChecksumText = ArrayUtility.ConcatenateArrays(header, confounder, toChecksumText);
            if ((EncryptionType)key.keytype.Value == EncryptionType.RC4_HMAC
                || (EncryptionType)key.keytype.Value == EncryptionType.RC4_HMAC_EXP)
            {
                TokenKeyUsage usage = TokenKeyUsage.USAGE_WRAP;
                if (tokenHeader.tok_id == TOK_ID.Mic1964_4757)
                {
                    usage = TokenKeyUsage.USAGE_MIC;
                }
                toChecksumText = ArrayUtility.ConcatenateArrays(BitConverter.GetBytes((int)usage), toChecksumText);
            }

            var md5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] md5 = md5CryptoServiceProvider.ComputeHash(toChecksumText);

            switch (tokenHeader.sng_alg)
            {
                case SGN_ALG.DES_MAC_MD5:
                    check = KerberosUtility.DesCbcMac(key.keyvalue.ByteArrayValue, new byte[KerberosConstValue.DES_BLOCK_SIZE], md5);
                    break;

                case SGN_ALG.MD2_5:
                    check = KerberosUtility.MD2_5(key.keyvalue.ByteArrayValue, toChecksumText);
                    break;

                case SGN_ALG.DES_MAC:
                    throw new NotSupportedException("DES_MAC is not supported currently.");

                case SGN_ALG.HMAC:
                    byte[] keySign = KerberosUtility.HMAC(key.keyvalue.ByteArrayValue, KerberosConstValue.SIGNATURE_KEY);
                    check = KerberosUtility.HMAC(keySign, md5);
                    break;

                default:
                    break;
            }
            tokenHeader.sng_cksum = ArrayUtility.SubArray(check, 0, KerberosConstValue.CHECKSUM_SIZE_RFC1964);
            if (!ArrayUtility.CompareArrays(tokenHeader.sng_cksum, checksum))
            {
                throw new FormatException("The checksum is incorrect!");
            }
            #endregion verify checksum

            #region verify sequence number
            byte[] seqBuf = null;
            switch ((EncryptionType)key.keytype.Value)
            {
                case EncryptionType.DES_CBC_CRC:
                case EncryptionType.DES_CBC_MD5:
                    uint seqNumber = KerberosUtility.ConvertEndian((uint)Context.CurrentRemoteSequenceNumber);
                    seqBuf = SetSequenceBuffer(seqNumber, !Context.IsInitiator);
                    tokenHeader.snd_seq = KerberosUtility.DesCbcDecrypt(key.keyvalue.ByteArrayValue,
                                                                    tokenHeader.sng_cksum,
                                                                    sequence);
                    break;

                case EncryptionType.RC4_HMAC:
                case EncryptionType.RC4_HMAC_EXP:
                    seqBuf = SetSequenceBuffer((uint)Context.CurrentRemoteSequenceNumber, !Context.IsInitiator);
                    tokenHeader.snd_seq = KerberosUtility.RC4HMAC(key.keyvalue.ByteArrayValue,
                                                              tokenHeader.sng_cksum,
                                                              sequence,
                                                              isExport);
                    break;

                default:
                    throw new NotSupportedException("RFC 1964 and 4757 only support encryption algorithm " +
                        "DES_CBC_CRC, DES_CBC_MD5, RC4_HMAC and RC4_HMAC_EXP!");
            }

            if (!ArrayUtility.CompareArrays(tokenHeader.snd_seq, seqBuf))
            {
                throw new FormatException("The sequence number is incorrect!");
            }
            #endregion verify sequence number
        }


        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        /// <exception cref="System.NotSupportedException">thrown when any type of tok_id, sng_alg or seal_alg is
        /// not supported.</exception>
        public override byte[] ToBytes()
        {
            if (data == null)
            {
                return null;
            }

            TokenHeader1964_4757 header1964_4757 = tokenHeader;
            TOK_ID tokId = tokenHeader.tok_id;
            if (tokId != TOK_ID.Wrap1964_4757 && tokId != TOK_ID.Mic1964_4757)
            {
                throw new NotSupportedException("tok_id = " + (ushort)tokenHeader.tok_id);
            }

            SGN_ALG sngAlg = tokenHeader.sng_alg;
            if (sngAlg != SGN_ALG.DES_MAC && sngAlg != SGN_ALG.DES_MAC_MD5
                && sngAlg != SGN_ALG.MD2_5 && sngAlg != SGN_ALG.HMAC)
            {
                throw new NotSupportedException("sng_alg = " + tokenHeader.sng_alg);
            }

            SEAL_ALG sealAlg = tokenHeader.seal_alg;
            if (sealAlg != SEAL_ALG.DES && sealAlg != SEAL_ALG.RC4 && sealAlg != SEAL_ALG.NONE)
            {
                throw new NotSupportedException("seal_alg = " + tokenHeader.seal_alg);
            }

            #region convert big-endian
            header1964_4757.tok_id = (TOK_ID)KerberosUtility.ConvertEndian((ushort)tokenHeader.tok_id);
            header1964_4757.sng_alg = (SGN_ALG)KerberosUtility.ConvertEndian((ushort)tokenHeader.sng_alg);
            header1964_4757.seal_alg = (SEAL_ALG)KerberosUtility.ConvertEndian((ushort)tokenHeader.seal_alg);
            byte[] headercheck = ArrayUtility.SubArray(KerberosUtility.StructToBytes(header1964_4757),
                                                       0,
                                                       KerberosConstValue.HEADER_FIRST_8_BYTE_SIZE);
            #endregion convert big-endian

            #region create plainText
            EncryptionKey key = Context.SessionKey;
            byte[] plainText = data;
            byte paddingLength = 0;

            if (tokId == TOK_ID.Wrap1964_4757)  // wrap token
            {
                #region compute pad
                byte[] pad = null;
                switch ((EncryptionType)key.keytype.Value)
                {
                    case EncryptionType.DES_CBC_CRC:
                    case EncryptionType.DES_CBC_MD5:
                        // The plaintext data is padded to the next highest multiple of 8 bytes, 
                        // by appending between 1 and 8 bytes, the value of each such byte being
                        // the total number of pad bytes.  For example, given data of length 20
                        // bytes, four pad bytes will be appended, and each byte will contain
                        // the hex value 04.  
                        int padLength = 8 - (data.Length % 8);
                        pad = new byte[padLength];
                        for (int i = 0; i < padLength; ++i)
                        {
                            pad[i] = (byte)padLength;
                        }
                        break;

                    case EncryptionType.RC4_HMAC:
                    case EncryptionType.RC4_HMAC_EXP:
                        // All padding is rounded up to 1 byte.
                        pad = new byte[] { 1 };
                        break;

                    default:
                        throw new NotSupportedException("RFC 1964 and 4757 only support encryption algorithm " +
                        "DES_CBC_CRC, DES_CBC_MD5, RC4_HMAC and RC4_HMAC_EXP!");
                }
                paddingLength = (byte)pad.Length;
                #endregion compute pad

                byte[] confounder = KerberosUtility.GenerateRandomBytes(KerberosConstValue.CONFOUNDER_SIZE);
                plainText = ArrayUtility.ConcatenateArrays(confounder, data, pad);
            }
            #endregion create plainText

            #region compute checksum
            byte[] check = null;
            byte[] toChecksumText = ArrayUtility.ConcatenateArrays(headercheck,
                                                                  plainText);
            if ((EncryptionType)key.keytype.Value == EncryptionType.RC4_HMAC
                || (EncryptionType)key.keytype.Value == EncryptionType.RC4_HMAC_EXP)
            {
                TokenKeyUsage usage = TokenKeyUsage.USAGE_WRAP;
                if (tokId == TOK_ID.Mic1964_4757)
                {
                    usage = TokenKeyUsage.USAGE_MIC;
                }
                toChecksumText = ArrayUtility.ConcatenateArrays(BitConverter.GetBytes((int)usage), toChecksumText);
            }

            var md5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] md5 = md5CryptoServiceProvider.ComputeHash(toChecksumText);

            switch (sngAlg)
            {
                case SGN_ALG.DES_MAC_MD5:
                    check = KerberosUtility.DesCbcMac(key.keyvalue.ByteArrayValue, new byte[KerberosConstValue.DES_BLOCK_SIZE], md5);
                    break;

                case SGN_ALG.MD2_5:
                    check = KerberosUtility.MD2_5(key.keyvalue.ByteArrayValue, toChecksumText);
                    break;

                case SGN_ALG.DES_MAC:
                    throw new NotSupportedException("DES_MAC is not supported currently.");

                case SGN_ALG.HMAC:
                    byte[] keySign = KerberosUtility.HMAC(key.keyvalue.ByteArrayValue, KerberosConstValue.SIGNATURE_KEY);
                    check = KerberosUtility.HMAC(keySign, md5);
                    break;

                default:
                    break;
            }
            tokenHeader.sng_cksum = ArrayUtility.SubArray(check, 0, KerberosConstValue.CHECKSUM_SIZE_RFC1964);
            #endregion compute checksum

            #region compute sequence number
            byte[] seqBuf = null;
            bool isExport = (EncryptionType)key.keytype.Value == EncryptionType.RC4_HMAC_EXP;
            switch ((EncryptionType)key.keytype.Value)
            {
                case EncryptionType.DES_CBC_CRC:
                case EncryptionType.DES_CBC_MD5:
                    uint seqNumber = KerberosUtility.ConvertEndian((uint)Context.CurrentLocalSequenceNumber);
                    seqBuf = SetSequenceBuffer(seqNumber, Context.IsInitiator);
                    tokenHeader.snd_seq = KerberosUtility.DesCbcEncrypt(key.keyvalue.ByteArrayValue,
                                                                    tokenHeader.sng_cksum,
                                                                    seqBuf);
                    break;

                case EncryptionType.RC4_HMAC:
                case EncryptionType.RC4_HMAC_EXP:
                    seqBuf = SetSequenceBuffer((uint)Context.CurrentLocalSequenceNumber, Context.IsInitiator);
                    tokenHeader.snd_seq = KerberosUtility.RC4HMAC(key.keyvalue.ByteArrayValue,
                                                              tokenHeader.sng_cksum,
                                                              seqBuf,
                                                              isExport);
                    break;

                default:
                    tokenHeader.snd_seq = seqBuf;
                    break;
            }
            #endregion compute sequence number

            #region compute encrypted data
            byte[] encData = null;
            if (sealAlg == SEAL_ALG.DES)
            {
                // The key used is derived from the established context key by XOR-ing the context key 
                // with the hexadecimal constant f0f0f0f0f0f0f0f0.
                byte[] Klocal = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    Klocal[i] = (byte)(key.keyvalue.Value[i] ^ 0xF0);
                }
                // The data is encrypted using DES-CBC, with an IV of zero.
                encData = KerberosUtility.DesCbcEncrypt(Klocal, new byte[8], plainText);
            }
            else if (sealAlg == SEAL_ALG.RC4)
            {
                // for (i = 0; i < 16; i++) Klocal[i] = Kss[i] ^ 0xF0;
                byte[] Klocal = new byte[16];
                for (int i = 0; i < 16; i++)
                {
                    Klocal[i] = (byte)(key.keyvalue.Value[i] ^ 0xF0);
                }
                byte[] seqBuffer = BitConverter.GetBytes((uint)Context.CurrentLocalSequenceNumber);
                Array.Reverse(seqBuffer);
                encData = KerberosUtility.RC4HMAC(Klocal, seqBuffer, plainText, isExport);
            }
            else
            {
                if (tokId == TOK_ID.Wrap1964_4757)
                {
                    encData = plainText;
                }
            }
            #endregion compute encrypted data

            byte[] allData = ArrayUtility.ConcatenateArrays(headercheck,
                                                            tokenHeader.snd_seq,
                                                            tokenHeader.sng_cksum,
                                                            encData);

            paddingData = ArrayUtility.SubArray(allData, allData.Length - paddingLength);

            return KerberosUtility.AddGssApiTokenHeader(allData);
        }


        /// <summary>
        /// Set sequence buffer with the given sequence number.
        /// </summary>
        /// <param name="sequenceNumber">The specified sequence number.</param>
        /// <param name="isInitiator">If the sender is the initiator.</param>
        /// <returns>The sequence buffer.</returns>
        private byte[] SetSequenceBuffer(uint sequenceNumber, bool isInitiator)
        {
            /* From [RFC 4757]
             * if (direction == sender_is_initiator)
            {
                memset(&Token.SEND_SEQ[4], 0xff, 4)
            }
            else if (direction == sender_is_acceptor)
            {
                memset(&Token.SEND_SEQ[4], 0, 4)
            }
            Token.SEND_SEQ[0] = (seq_num & 0xff000000) >> 24;
            Token.SEND_SEQ[1] = (seq_num & 0x00ff0000) >> 16;
            Token.SEND_SEQ[2] = (seq_num & 0x0000ff00) >> 8;
            Token.SEND_SEQ[3] = (seq_num & 0x000000ff);
             * */

            byte[] seqBuffer = new byte[KerberosConstValue.SEQUENCE_NUMBER_SIZE];
            if (!isInitiator)  // sender_is_acceptor
            {
                seqBuffer[4] = KerberosConstValue.TOKEN_FILLER_1_BYTE;
                seqBuffer[5] = KerberosConstValue.TOKEN_FILLER_1_BYTE;
                seqBuffer[6] = KerberosConstValue.TOKEN_FILLER_1_BYTE;
                seqBuffer[7] = KerberosConstValue.TOKEN_FILLER_1_BYTE;
            }
            seqBuffer[0] = (byte)((sequenceNumber & 0xff000000) >> 24);
            seqBuffer[1] = (byte)((sequenceNumber & 0x00ff0000) >> 16);
            seqBuffer[2] = (byte)((sequenceNumber & 0x0000ff00) >> 8);
            seqBuffer[3] = (byte)(sequenceNumber & 0x000000ff);
            return seqBuffer;
        }
    }
}