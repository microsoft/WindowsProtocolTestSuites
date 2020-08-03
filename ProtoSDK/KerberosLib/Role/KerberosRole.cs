// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /// <summary>
    /// Base class of KileClient and KileServer.
    /// Maintain the common functions.
    /// </summary>
    public abstract class KerberosRole : IDisposable
    {
        #region Private Members
        /// <summary>
        /// Represents whether this object has been disposed.
        /// </summary>
        private bool disposed;

        #endregion

        #region Properties

        /// <summary>
        /// Contains all the important state variables in the context.
        /// </summary>
        public abstract KerberosContext Context
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Decode Kerberos PDUs from received message bytes
        /// </summary>
        /// <param name="endPoint">An endpoint from which the message bytes are received</param>
        /// <param name="receivedBytes">The received bytes to be decoded</param>
        /// <param name="consumedLength">Length of message bytes consumed by decoder</param>
        /// <param name="expectedLength">Length of message bytes the decoder expects to receive</param>
        /// <returns>The decoded Kerberos PDUs</returns>
        /// <exception cref="System.FormatException">thrown when a Kerberos message type is unsupported</exception>
        public abstract KerberosPdu[] DecodePacketCallback(object endPoint,
                                                byte[] receivedBytes,
                                                out int consumedLength,
                                                out int expectedLength);

        /// <summary>
        /// Updated context based on Kerberos pdu
        /// </summary>
        /// <param name="pdu">Kerberos pdu</param>
        public virtual void UpdateContext(KerberosPdu pdu)
        {
        }

        #region Wrap/UnWrap, GetMic/VerifyMic
        /// <summary>
        /// Create a Gss_Wrap token. Then use KilePdu.ToBytes() to get the byte array.
        /// </summary>
        /// <param name="isEncrypted">If encrypt the message.</param>
        /// <param name="signAlgorithm">Specify the checksum type.
        /// This is only used for encryption types DES and RC4.</param>
        /// <param name="message">The message to be wrapped. This argument can be null.</param>
        /// <returns>The created Gss_Wrap token.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when the encryption type is not supported.</exception>
        public KerberosPdu GssWrap(bool isEncrypted, SGN_ALG signAlgorithm, byte[] message)
        {
            KerberosPdu pdu = null;
            EncryptionKey key = Context.ContextKey;
            switch ((EncryptionType)key.keytype.Value)
            {
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                    pdu = GssWrap4121(isEncrypted, message, Context.IsInitiator);
                    break;

                case EncryptionType.DES_CBC_CRC:
                case EncryptionType.DES_CBC_MD5:
                    pdu = GssWrap1964(isEncrypted, signAlgorithm, message);
                    break;

                case EncryptionType.RC4_HMAC:
                case EncryptionType.RC4_HMAC_EXP:
                    pdu = GssWrap4757(isEncrypted, signAlgorithm, message);
                    break;

                default:
                    throw new NotSupportedException("The Encryption Type can only be AES128_CTS_HMAC_SHA1_96, "
                        + "AES256_CTS_HMAC_SHA1_96, DES_CBC_CRC, DES_CBC_MD5, RC4_HMAC or RC4_HMAC_EXP.");
            }

            return pdu;
        }

        /// <summary>
        /// Decode a Gss_Wrap token from security buffers
        /// </summary>
        /// <param name="securityBuffers">Security buffers</param>
        /// <returns>The decoded Gss_Wrap token.</returns>
        public KerberosPdu GssUnWrapEx(params SecurityBuffer[] securityBuffers)
        {
            return GssUnWrapEx(Context, securityBuffers);
        }

        /// <summary>
        /// Decode a Gss_Wrap token from security buffers
        /// </summary>
        /// <param name="context">The context of decoding</param>
        /// <param name="securityBuffers">Security buffers</param>
        /// <returns>The decoded Gss_Wrap token.</returns>
        internal static KerberosPdu GssUnWrapEx(KerberosContext context, SecurityBuffer[] securityBuffers)
        {
            KerberosPdu pdu = null;
            EncryptionKey key = context.ContextKey;
            switch ((EncryptionType)key.keytype.Value)
            {
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                    var token4121Pdu = new Token4121(context);
                    token4121Pdu.FromSecurityBuffers(securityBuffers);
                    pdu = token4121Pdu;
                    break;

                case EncryptionType.DES_CBC_CRC:
                case EncryptionType.DES_CBC_MD5:
                case EncryptionType.RC4_HMAC:
                case EncryptionType.RC4_HMAC_EXP:
                    var token1964or4757Pdu = new Token1964_4757(context);
                    token1964or4757Pdu.FromSecurityBuffers(securityBuffers);
                    pdu = token1964or4757Pdu;
                    break;

                default:
                    throw new NotSupportedException("The Encryption Type can only be AES128_CTS_HMAC_SHA1_96, "
                        + "AES256_CTS_HMAC_SHA1_96, DES_CBC_CRC, DES_CBC_MD5, RC4_HMAC or RC4_HMAC_EXP.");
            }

            return pdu;
        }

        /// <summary>
        /// Create a Gss_GetMic token. Then use KilePdu.ToBytes() to get the byte array.
        /// </summary>
        /// <param name="signAlgorithm">Specify the checksum type.
        /// This is only used for encryption types DES and RC4.</param>
        /// <param name="message">The message to be computed signature. This argument can be null.</param>
        /// <returns>The created Gss_GetMic token, NotSupportedException.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when the encryption is not supported.</exception>
        public KerberosPdu GssGetMic(SGN_ALG signAlgorithm, byte[] message)
        {
            KerberosPdu pdu = null;
            EncryptionKey key = Context.ContextKey;
            switch ((EncryptionType)key.keytype.Value)
            {
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                    pdu = GssGetMic4121(message, Context.IsInitiator);
                    break;

                case EncryptionType.DES_CBC_CRC:
                case EncryptionType.DES_CBC_MD5:
                case EncryptionType.RC4_HMAC:
                case EncryptionType.RC4_HMAC_EXP:
                    pdu = GssGetMic1964_4757(signAlgorithm, message);
                    break;

                default:
                    throw new NotSupportedException("The Encryption Type can only be AES128_CTS_HMAC_SHA1_96, "
                        + "AES256_CTS_HMAC_SHA1_96, DES_CBC_CRC, DES_CBC_MD5, RC4_HMAC or RC4_HMAC_EXP.");
            }

            return pdu;
        }

        public bool GssVerifyMicEx(SecurityBuffer[] securityBuffers, out KerberosPdu pdu)
        {
            return GssVerifyMicEx(Context, securityBuffers, out pdu);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Create a Gss_GetMic [RFC4121] token.
        /// </summary>
        /// <param name="message">The message to be wrapped. This argument can be null.</param>
        /// <param name="isInitiator">If the sender is initiator.</param>
        /// <returns>The created Gss_GetMic token.</returns>
        private Token4121 GssGetMic4121(byte[] message, bool isInitiator)
        {
            var token = new Token4121(Context);
            var tokenHeader = new TokenHeader4121();
            tokenHeader.tok_id = TOK_ID.Mic4121;
            tokenHeader.flags = WrapFlag.None;
            if (!isInitiator)
            {
                tokenHeader.flags |= WrapFlag.SentByAcceptor;
            }

            if (Context.AcceptorSubKey != null)
            {
                tokenHeader.flags |= WrapFlag.AcceptorSubkey;
            }

            tokenHeader.filler = KerberosConstValue.TOKEN_FILLER_1_BYTE;
            tokenHeader.ec = KerberosConstValue.TOKEN_FILLER_2_BYTE;
            tokenHeader.rrc = KerberosConstValue.TOKEN_FILLER_2_BYTE;
            tokenHeader.snd_seq = Context.CurrentLocalSequenceNumber;

            token.TokenHeader = tokenHeader;
            token.Data = message;

            return token;
        }

        /// <summary>
        /// Create a Gss_GetMic [RFC1964] token.
        /// </summary>
        /// <param name="signAlgorithm">Specify the checksum type.</param>
        /// <param name="message">The message to be wrapped. This argument can be null.</param>
        /// <returns>The created Gss_GetMic token.</returns>
        private Token1964_4757 GssGetMic1964_4757(SGN_ALG signAlgorithm, byte[] message)
        {
            return WrapToken1964_4757(GssTokenType.Mic1964_4757, message, false, signAlgorithm);
        }

        internal static bool GssVerifyMicEx(KerberosContext context, SecurityBuffer[] securityBuffers, out KerberosPdu pdu)
        {
            pdu = null;
            bool isVerified = true;
            EncryptionKey key = context.ContextKey;

            switch ((EncryptionType)key.keytype.Value)
            {
                case EncryptionType.AES128_CTS_HMAC_SHA1_96:
                case EncryptionType.AES256_CTS_HMAC_SHA1_96:
                    var micPdu4121 = new Token4121(context);
                    try
                    {
                        micPdu4121.FromSecurityBuffers(securityBuffers);
                    }
                    catch (FormatException)
                    {
                        isVerified = false;
                    }
                    pdu = micPdu4121;
                    break;

                case EncryptionType.DES_CBC_CRC:
                case EncryptionType.DES_CBC_MD5:
                case EncryptionType.RC4_HMAC:
                case EncryptionType.RC4_HMAC_EXP:
                    var micPdu1964_4757 = new Token1964_4757(context);
                    try
                    {
                        micPdu1964_4757.FromSecurityBuffers(securityBuffers);
                    }
                    catch (FormatException)
                    {
                        isVerified = false;
                    }
                    pdu = micPdu1964_4757;
                    break;

                default:
                    throw new NotSupportedException("The Encryption Type can only be AES128_CTS_HMAC_SHA1_96, "
                        + "AES256_CTS_HMAC_SHA1_96, DES_CBC_CRC, DES_CBC_MD5, RC4_HMAC or RC4_HMAC_EXP.");
            }

            return isVerified;
        }

        /// <summary>
        /// Create a Gss_Wrap [RFC4121] token.
        /// </summary>
        /// <param name="isEncrypted">If encrypt the message.</param>
        /// <param name="message">The message to be wrapped. This argument can be null.</param>
        /// <param name="isInitiator">If the sender is initiator.</param>
        /// <returns>The created Gss_Wrap token.</returns>
        private Token4121 GssWrap4121(bool isEncrypted, byte[] message, bool isInitiator)
        {
            var token = new Token4121(Context);
            var tokenHeader = new TokenHeader4121();
            tokenHeader.tok_id = TOK_ID.Wrap4121;
            tokenHeader.flags = isEncrypted ? WrapFlag.Sealed : WrapFlag.None;
            if (!isInitiator)
            {
                tokenHeader.flags |= WrapFlag.SentByAcceptor;
            }

            if (Context.AcceptorSubKey != null)
            {
                tokenHeader.flags |= WrapFlag.AcceptorSubkey;
            }

            tokenHeader.filler = KerberosConstValue.TOKEN_FILLER_1_BYTE;
            tokenHeader.ec = 16;
            // [MS-KILE] The RRC field ([RFC4121] section 4.2.5) is 12 if no encryption is requested or 28 if encryption is requested. 
            tokenHeader.rrc = isEncrypted ? (ushort)28 : (ushort)12;
            tokenHeader.snd_seq = Context.CurrentLocalSequenceNumber;

            token.TokenHeader = tokenHeader;
            token.Data = message;

            return token;
        }

        /// <summary>
        /// Create a Gss_Wrap [RFC1964] token.
        /// </summary>
        /// <param name="isEncrypted">If encrypt the message.</param>
        /// <param name="signAlgorithm">Specify the checksum type.</param>
        /// <param name="message">The message to be wrapped. This argument can be null.</param>
        /// <returns>The created Gss_Wrap token.</returns>
        private Token1964_4757 GssWrap1964(bool isEncrypted, SGN_ALG signAlgorithm, byte[] message)
        {
            return WrapToken1964_4757(GssTokenType.Wrap1964, message, isEncrypted, signAlgorithm);
        }

        /// <summary>
        /// Create a Gss_Wrap [RFC4757] token.
        /// </summary>
        /// <param name="isEncrypted">If encrypt the message.</param>
        /// <param name="signAlgorithm">Specify the checksum type.</param>
        /// <param name="message">The message to be wrapped. This argument can be null.</param>
        /// <returns>The created Gss_Wrap token.</returns>
        private Token1964_4757 GssWrap4757(bool isEncrypted, SGN_ALG signAlgorithm, byte[] message)
        {
            return WrapToken1964_4757(GssTokenType.Wrap4757, message, isEncrypted, signAlgorithm);
        }

        private Token1964_4757 WrapToken1964_4757(GssTokenType tokenType, byte[] message, bool isEncrypted, SGN_ALG signAlgorithm)
        {
            var token = new Token1964_4757(Context);
            TokenHeader1964_4757 tokenHeader = new TokenHeader1964_4757();
            tokenHeader.filler = KerberosConstValue.TOKEN_FILLER_2_BYTE;
            tokenHeader.sng_alg = signAlgorithm;

            switch (tokenType)
            {
                case GssTokenType.Wrap4757:
                    {
                        tokenHeader.tok_id = TOK_ID.Wrap1964_4757;
                        tokenHeader.seal_alg = isEncrypted ? SEAL_ALG.RC4 : SEAL_ALG.NONE;
                    }
                    break;
                case GssTokenType.Wrap1964:
                    {
                        tokenHeader.tok_id = TOK_ID.Wrap1964_4757;
                        tokenHeader.seal_alg = isEncrypted ? SEAL_ALG.DES : SEAL_ALG.NONE;
                    }
                    break;
                case GssTokenType.Mic1964_4757:
                    {
                        tokenHeader.tok_id = TOK_ID.Mic1964_4757;
                        tokenHeader.seal_alg = SEAL_ALG.NONE;
                    }
                    break;
            }
            token.TokenHeader = tokenHeader;
            token.Data = message;

            return token;
        }

        #endregion

        #region IDisposable
        /// <summary>
        /// Release the managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //Release managed resource.
                }

                //Note disposing has been done.
                disposed = true;
            }
        }


        /// <summary>
        /// Destruct this instance.
        /// </summary>
        ~KerberosRole()
        {
            Dispose(false);
        }
        #endregion
    }

    internal enum GssTokenType
    {
        Wrap4757,
        Wrap1964,
        Mic1964_4757
    }
}
