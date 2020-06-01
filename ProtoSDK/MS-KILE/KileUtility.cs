// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Security.Cryptographic;
using Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Kile
{
    /// <summary>
    /// A static class provides some helper functions. It is called by KilePdu and KileDecoder.
    /// </summary>
    public static class KileUtility
    {
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
            SGN_ALG sgnAlg = GetSgnAlg(kileRole.Context.ContextKey);
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

            SGN_ALG sgnAlg = GetSgnAlg(kileRole.Context.ContextKey);
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

        #region private
        /// <summary>
        /// Get SGN_ALG for encryption and sign.
        /// </summary>
        /// <returns>The SGN_ALG got from context.</returns>
        /// <exception cref="System.FormatException">Thrown when the key is not valid.</exception>
        internal static SGN_ALG GetSgnAlg(EncryptionKey contextKey)
        {
            if (contextKey == null || contextKey.keytype == null || contextKey.keyvalue == null || contextKey.keyvalue.Value == null)
            {
                throw new FormatException("Initialization is not complete successfully!");
            }

            SGN_ALG sgnAlg = SGN_ALG.HMAC;
            EncryptionType type = (EncryptionType)contextKey.keytype.Value;

            if (type == EncryptionType.DES_CBC_MD5 || type == EncryptionType.DES_CBC_CRC)
            {
                sgnAlg = SGN_ALG.DES_MAC_MD5;
            }

            return sgnAlg;
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
        #endregion
    }
}
