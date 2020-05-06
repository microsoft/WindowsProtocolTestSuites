// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /// <summary>
    /// The Attribute of flag.
    /// </summary>
    [Flags]
    public enum WrapFlag : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicate the sender is the context acceptor.
        /// </summary>
        SentByAcceptor = 0x01,

        /// <summary>
        /// Indicate confidentiality is provided for. It SHALL NOT be set in MIC tokens.
        /// </summary>
        Sealed = 0x02,

        /// <summary>
        /// A subkey asserted by the context acceptor is used to protect the message.
        /// </summary>
        AcceptorSubkey = 0x04,
    }

    /// <summary>
    /// Token header of rfc[4121].
    /// </summary>
    public struct TokenHeader4121
    {
        /// <summary>
        /// Identification field.
        /// </summary>
        public TOK_ID tok_id;

        /// <summary>
        /// Attributes field.
        /// </summary>
        public WrapFlag flags;

        /// <summary>
        /// Contains the hex value FF.
        /// </summary>
        public byte filler;

        /// <summary>
        /// Contains the "extra count" field.
        /// </summary>
        public ushort ec;

        /// <summary>
        /// Contains the "right rotation count".
        /// </summary>
        public ushort rrc;

        /// <summary>
        /// Sequence number field in clear text.
        /// </summary>
        public UInt64 snd_seq;
    }

    public class Token4121 : KerberosPdu
    {
        /// <summary>
        /// The token header.
        /// </summary>
        private TokenHeader4121 tokenHeader;

        /// <summary>
        /// The orignial data.
        /// </summary>
        private byte[] data;

        private KerberosContext Context;

        /// <summary>
        /// The token header.
        /// </summary>
        public TokenHeader4121 TokenHeader
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
        /// <param name="context">The context of the client.</param>
        public Token4121(KerberosContext context)
        {
            this.Context = context;
        }


        /// <summary>
        /// Decode the KILE PDU from the message bytes.
        /// </summary>
        /// <param name="buffer">The byte array to be decoded.</param>
        /// <exception cref="System.FormatException">Thrown when the format of input parameter is not correct.
        /// </exception>
        public override void FromBytes(byte[] buffer)
        {
            if (buffer == null || buffer.Length < Marshal.SizeOf(typeof(TokenHeader4121)))
            {
                throw new FormatException("The token body is incomplete!");
            }

            byte[] tokenBody = buffer;
            if (buffer[0] == KerberosConstValue.KERBEROS_TAG)
            {
                tokenBody = KerberosUtility.VerifyGssApiTokenHeader(buffer);
            }

            tokenHeader = KerberosUtility.ToStruct<TokenHeader4121>(tokenBody);
            ushort ec = KerberosUtility.ConvertEndian(tokenHeader.ec);
            TOK_ID tokId = (TOK_ID)KerberosUtility.ConvertEndian((ushort)tokenHeader.tok_id);

            if (tokId == TOK_ID.Wrap4121)
            {
                int tokenSize = buffer.Length - tokenBody.Length;
                tokenSize += Marshal.SizeOf(typeof(TokenHeader4121)); //clearhdr
                if ((tokenHeader.flags & WrapFlag.Sealed) != 0)
                {
                    tokenSize += Marshal.SizeOf(typeof(TokenHeader4121)); //enchdr
                    tokenSize += Cryptographic.ConstValue.HMAC_HASH_OUTPUT_SIZE; //checksum
                    tokenSize += Cryptographic.ConstValue.AES_BLOCK_SIZE; // confounder
                    tokenSize += ec; //EC bytes of padding
                }
                else
                {
                    tokenSize += ec; //checksum
                }

                if (buffer.Length < tokenSize)
                {
                    throw new FormatException("The token body is incomplete!");
                }

                FromSecurityBuffers(
                    new SecurityBuffer(SecurityBufferType.Token, ArrayUtility.SubArray(buffer, 0, tokenSize)),
                    new SecurityBuffer(SecurityBufferType.Data, ArrayUtility.SubArray(buffer, tokenSize)));
            }
            else if (tokId == TOK_ID.Mic4121)
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
        /// Decode the KILE PDU from security buffers.
        /// </summary>
        /// <param name="securityBuffers">Security buffers</param>
        public void FromSecurityBuffers(params SecurityBuffer[] securityBuffers)
        {
            byte[] tokenBody = SspiUtility.ConcatenateSecurityBuffers(securityBuffers, SecurityBufferType.Token);
            if (tokenBody[0] == KerberosConstValue.KERBEROS_TAG)
            {
                tokenBody = KerberosUtility.VerifyGssApiTokenHeader(tokenBody);
            }

            if (tokenBody.Length < Marshal.SizeOf(typeof(TokenHeader4121)))
            {
                throw new FormatException("The token body is incomplete!");
            }

            tokenHeader = KerberosUtility.ToStruct<TokenHeader4121>(tokenBody);
            ushort rrc = KerberosUtility.ConvertEndian(tokenHeader.rrc);
            ushort ec = KerberosUtility.ConvertEndian(tokenHeader.ec);
            TOK_ID tokId = (TOK_ID)KerberosUtility.ConvertEndian((ushort)tokenHeader.tok_id);

            byte[] cipher;
            if (tokId == TOK_ID.Wrap4121)
            {
                cipher = ArrayUtility.ConcatenateArrays(
                    ArrayUtility.SubArray(tokenBody, Marshal.SizeOf(typeof(TokenHeader4121))),
                    SspiUtility.ConcatenateReadWriteSecurityBuffers(securityBuffers, SecurityBufferType.Data, SecurityBufferType.Padding));
            }
            else
            {
                cipher = ArrayUtility.SubArray(tokenBody, Marshal.SizeOf(typeof(TokenHeader4121)));
            }

            #region set keyusage
            TokenKeyUsage keyUsage;
            if (tokId == TOK_ID.Wrap4121)  // wrap token
            {
                if ((tokenHeader.flags & WrapFlag.SentByAcceptor) == WrapFlag.SentByAcceptor)
                {
                    keyUsage = TokenKeyUsage.KG_USAGE_ACCEPTOR_SEAL;
                }
                else
                {
                    keyUsage = TokenKeyUsage.KG_USAGE_INITIATOR_SEAL;
                }

                if ((tokenHeader.flags & WrapFlag.Sealed) == WrapFlag.None)
                {
                    tokenHeader.ec = 0;
                }

                tokenHeader.rrc = 0;
            }
            else  // mic token
            {
                if ((tokenHeader.flags & WrapFlag.SentByAcceptor) == WrapFlag.SentByAcceptor)
                {
                    keyUsage = TokenKeyUsage.KG_USAGE_ACCEPTOR_SIGN;
                }
                else
                {
                    keyUsage = TokenKeyUsage.KG_USAGE_INITIATOR_SIGN;
                }
            }
            #endregion set keyusage

            byte[] header = KerberosUtility.StructToBytes(tokenHeader);

            #region convert big-endian
            tokenHeader.tok_id = (TOK_ID)KerberosUtility.ConvertEndian((ushort)tokenHeader.tok_id);
            tokenHeader.snd_seq = KerberosUtility.ConvertEndian(tokenHeader.snd_seq);
            tokenHeader.ec = ec;
            tokenHeader.rrc = rrc;
            #endregion convert big-endian

            #region verify header
            if (tokenHeader.tok_id != TOK_ID.Wrap4121 && tokenHeader.tok_id != TOK_ID.Mic4121)
            {
                throw new FormatException("The token ID is incorrect! tok_id = " + (ushort)tokenHeader.tok_id);
            }

            if (tokenHeader.filler != KerberosConstValue.TOKEN_FILLER_1_BYTE)
            {
                throw new FormatException("The token filler is incorrect! filler = " + tokenHeader.filler);
            }

            if (tokId == TOK_ID.Mic4121)
            {
                if (tokenHeader.ec != KerberosConstValue.TOKEN_FILLER_2_BYTE)
                {
                    throw new FormatException("The token ec is incorrect! ec = " + (ushort)tokenHeader.ec);
                }

                if (tokenHeader.rrc != KerberosConstValue.TOKEN_FILLER_2_BYTE)
                {
                    throw new FormatException("The token rrc is incorrect! rrc = " + (ushort)tokenHeader.rrc);
                }
            }
            #endregion verify header

            #region set context key
            EncryptionKey key = Context.SessionKey;
            #endregion

            if (tokId == TOK_ID.Wrap4121)
            {
                //The RRC field ([RFC4121] section 4.2.5) is 12 if no encryption is requested or 28 if encryption is requested. 
                //The RRC field is chosen such that all the data can be encrypted in place. 
                //The trailing meta-data H1 is rotated by RRC+EC bytes, 
                //which is different from RRC alone ([RFC4121] section 4.2.5).
                if ((tokenHeader.flags & WrapFlag.Sealed) != 0)
                {
                    rrc += ec;
                }
                KerberosUtility.RotateRight(cipher, cipher.Length - rrc);
            }

            if (tokId == TOK_ID.Wrap4121 && (tokenHeader.flags & WrapFlag.Sealed) == WrapFlag.Sealed)
            {
                GetToBeSignedDataFunc getToBeSignedDataCallback = delegate (byte[] decryptedData)
                {
                    //Coding according to MS-KILE section 4.3 GSS_WrapEx with AES128-CTS-HMAC-SHA1-96
                    //And Figure 4: Example of RRC with output message with 4 buffers

                    //cipher block size, c
                    //This is the block size of the block cipher underlying the
                    //encryption and decryption functions indicated above, used for key
                    //derivation and for the size of the message confounder and initial
                    //vector.  (If a block cipher is not in use, some comparable
                    //parameter should be determined.)  It must be at least 5 octets.
                    int cipherBlockSize;
                    EncryptionType eType = (EncryptionType)key.keytype.Value;
                    if (eType != EncryptionType.AES256_CTS_HMAC_SHA1_96 && eType != EncryptionType.AES128_CTS_HMAC_SHA1_96)
                    {
                        //etype other than AES is not supported.
                        return decryptedData;
                    }
                    cipherBlockSize = Cryptographic.ConstValue.AES_BLOCK_SIZE;
                    cipherBlockSize = Math.Max(cipherBlockSize, 5);
                    int dataLength = SspiUtility.ConcatenateReadWriteSecurityBuffers(securityBuffers, SecurityBufferType.Data, SecurityBufferType.Padding).Length;
                    int hdrSize = Marshal.SizeOf(typeof(TokenHeader4121));

                    byte[] confounder = ArrayUtility.SubArray(decryptedData, 0, cipherBlockSize);
                    byte[] plainText = ArrayUtility.SubArray(decryptedData, cipherBlockSize, dataLength);
                    byte[] filler = ArrayUtility.SubArray(decryptedData, confounder.Length + dataLength, ec);
                    byte[] enchdr = ArrayUtility.SubArray(decryptedData, decryptedData.Length - hdrSize);
                    SspiUtility.UpdateSecurityBuffers(
                        securityBuffers,
                        new SecurityBufferType[] { SecurityBufferType.Data, SecurityBufferType.Padding },
                        plainText);

                    byte[] toBeSignedData = KerberosUtility.GetToBeSignedDataFromSecurityBuffers(securityBuffers);
                    toBeSignedData = ArrayUtility.ConcatenateArrays(
                        confounder,
                        toBeSignedData,
                        filler,
                        enchdr);
                    return toBeSignedData;
                };

                // wrap & confidentiality is provided
                // {"header" | encrypt(plaintext-data | filler | "header")}
                byte[] plainBuffer = KerberosUtility.Decrypt((EncryptionType)key.keytype.Value,
                                                       key.keyvalue.ByteArrayValue,
                                                       cipher,
                                                       (int)keyUsage,
                                                       getToBeSignedDataCallback);
                if (plainBuffer.Length < Marshal.SizeOf(typeof(TokenHeader4121)) + ec)
                {
                    throw new FormatException("The encrypted data is incomplete!");
                }

                byte[] headerBuffer = ArrayUtility.SubArray(plainBuffer,
                                                            plainBuffer.Length - Marshal.SizeOf(typeof(TokenHeader4121)));
                if (!ArrayUtility.CompareArrays(header, headerBuffer))
                {
                    throw new FormatException("The encrypted data is incorrect!");
                }

                data = ArrayUtility.SubArray(plainBuffer,
                                             0,
                                             plainBuffer.Length - Marshal.SizeOf(typeof(TokenHeader4121)) - ec);
            }
            else   // no confidentiality is provided
            {
                byte[] check = cipher;
                if (tokId == TOK_ID.Wrap4121)
                {
                    // {"header" | plaintext-data | get_mic(plaintext-data | "header")}
                    data = ArrayUtility.SubArray(cipher, 0, cipher.Length - ec);
                    check = ArrayUtility.SubArray(cipher, cipher.Length - ec);
                }
                else
                {
                    data = KerberosUtility.GetToBeSignedDataFromSecurityBuffers(securityBuffers);
                }

                ChecksumType checksumType = ChecksumType.hmac_sha1_96_aes256;
                if ((EncryptionType)key.keytype.Value == EncryptionType.AES128_CTS_HMAC_SHA1_96)
                {
                    checksumType = ChecksumType.hmac_sha1_96_aes128;
                }

                byte[] checksum = KerberosUtility.GetChecksum(
                    key.keyvalue.ByteArrayValue,
                    ArrayUtility.ConcatenateArrays(data, header),
                    (int)keyUsage,
                    checksumType);

                if (!ArrayUtility.CompareArrays(check, checksum))
                {
                    throw new FormatException("The checksum is incorrect!");
                }
            }
        }


        /// <summary>
        /// Encode this class into byte array.
        /// </summary>
        /// <returns>The byte array of the class.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when the type of tok_id is not supported.</exception>
        public override byte[] ToBytes()
        {
            if (data == null)
            {
                return null;
            }

            if (tokenHeader.tok_id != TOK_ID.Wrap4121 && tokenHeader.tok_id != TOK_ID.Mic4121)
            {
                throw new NotSupportedException("tok_id = " + (ushort)tokenHeader.tok_id);
            }

            #region convert big-endian
            TokenHeader4121 header4121 = tokenHeader;
            header4121.tok_id = (TOK_ID)KerberosUtility.ConvertEndian((ushort)tokenHeader.tok_id);
            header4121.snd_seq = KerberosUtility.ConvertEndian(tokenHeader.snd_seq);
            header4121.ec = KerberosUtility.ConvertEndian(tokenHeader.ec);
            #endregion convert big-endian

            #region set keyusage
            TokenKeyUsage keyUsage;
            if (tokenHeader.tok_id == TOK_ID.Wrap4121)  // wrap token
            {
                if ((tokenHeader.flags & WrapFlag.SentByAcceptor) == WrapFlag.SentByAcceptor)
                {
                    keyUsage = TokenKeyUsage.KG_USAGE_ACCEPTOR_SEAL;
                }
                else
                {
                    keyUsage = TokenKeyUsage.KG_USAGE_INITIATOR_SEAL;
                }

                if ((tokenHeader.flags & WrapFlag.Sealed) == WrapFlag.None)
                {
                    header4121.ec = 0;
                }

                header4121.rrc = 0;
            }
            else  // mic token
            {
                if ((tokenHeader.flags & WrapFlag.SentByAcceptor) == WrapFlag.SentByAcceptor)
                {
                    keyUsage = TokenKeyUsage.KG_USAGE_ACCEPTOR_SIGN;
                }
                else
                {
                    keyUsage = TokenKeyUsage.KG_USAGE_INITIATOR_SIGN;
                }
            }
            #endregion set keyusage

            #region set context key
            EncryptionKey key = Context.SessionKey;
            #endregion set context key

            // plainBuf = plaintext-data | "header"
            byte[] headerBuf = KerberosUtility.StructToBytes(header4121);
            byte[] plainBuf = ArrayUtility.ConcatenateArrays(data, headerBuf);

            byte[] cipher = null;
            if (tokenHeader.tok_id == TOK_ID.Wrap4121 && (tokenHeader.flags & WrapFlag.Sealed) == WrapFlag.Sealed)
            {
                // wrap & confidentiality is provided
                // {"header" | encrypt(plaintext-data | filler | "header")}
                if (tokenHeader.ec != 0)
                {
                    byte[] filler = new byte[tokenHeader.ec];
                    plainBuf = ArrayUtility.ConcatenateArrays(data, filler, headerBuf);
                }

                cipher = KerberosUtility.Encrypt((EncryptionType)key.keytype.Value,
                                             key.keyvalue.ByteArrayValue,
                                             plainBuf,
                                             (int)keyUsage);
            }
            else   // no confidentiality is provided or mic token
            {
                // {"header" | plaintext-data | get_mic(plaintext-data | "header")}
                ChecksumType checksumType = ChecksumType.hmac_sha1_96_aes256;
                if ((EncryptionType)key.keytype.Value == EncryptionType.AES128_CTS_HMAC_SHA1_96)
                {
                    checksumType = ChecksumType.hmac_sha1_96_aes128;
                }

                cipher = KerberosUtility.GetChecksum(key.keyvalue.ByteArrayValue, plainBuf, (int)keyUsage, checksumType);
                if (tokenHeader.tok_id == TOK_ID.Wrap4121)
                {
                    header4121.ec = KerberosUtility.ConvertEndian((ushort)cipher.Length);
                    cipher = ArrayUtility.ConcatenateArrays(data, cipher);
                }
            }

            #region set rrc
            if (tokenHeader.tok_id == TOK_ID.Wrap4121)
            {
                KerberosUtility.RotateRight(cipher, tokenHeader.rrc);
                header4121.rrc = KerberosUtility.ConvertEndian(tokenHeader.rrc);
                headerBuf = KerberosUtility.StructToBytes(header4121);
            }
            #endregion set rrc

            return ArrayUtility.ConcatenateArrays(headerBuf, cipher);
        }
    }
}