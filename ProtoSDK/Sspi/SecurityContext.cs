// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// An abstract class to store security context used in SSPI.
    /// </summary>
    public abstract class SecurityContext
    {

        #region methods

        /// <summary>
        /// This takes the given byte array, signs it, and returns the signature.
        /// </summary>
        /// <param name="messageToBeSigned">Message to be signed.</param>
        /// <returns>Signature of message</returns>
        /// <exception cref="SspiException">If sign fail, this exception will be thrown.</exception>
        public byte[] Sign(byte[] messageToBeSigned)
        {
            SecurityBuffer messageBuffer = new SecurityBuffer(SecurityBufferType.Data, messageToBeSigned);
            SecurityBuffer tokenBuffer = new SecurityBuffer(
                SecurityBufferType.Token, 
                new byte[NativeMethods.MAX_TOKEN_SIZE]);

            Sign(messageBuffer, tokenBuffer);
            return tokenBuffer.Buffer;
        }


        /// <summary>
        /// Sign data according SecBuffers.
        /// </summary>
        /// <param name="securityBuffers">SecurityBuffer array</param>
        public abstract void Sign(params SecurityBuffer[] securityBuffers);


        /// <summary>
        /// This takes the given message, sign it and returns another byte array containing the original message
        /// and signature, the format of the returned byte array is as follow:
        /// |MESSAGE_LENGTH(4 bytes)|MESSAGE|SIGNATURE|
        /// </summary>
        /// <param name="messageToBeSigned">Message to be signed.</param>
        /// <returns>Signed message and signature, which contains the header.</returns>
        /// <exception cref="SspiException">If sign fail, this exception will be thrown.</exception>
        public byte[] SignMessage(byte[] messageToBeSigned)
        {
            SecurityBuffer messageBuffer = new SecurityBuffer(SecurityBufferType.Data, messageToBeSigned);
            SecurityBuffer signatureBuffer = new SecurityBuffer(
                SecurityBufferType.Token, 
                new byte[NativeMethods.MAX_TOKEN_SIZE]);

            Sign(messageBuffer, signatureBuffer);

            int messageLength = messageBuffer.Buffer.Length;
            byte[] signedMessage = ArrayUtility.ConcatenateArrays(
                BitConverter.GetBytes(messageLength),
                messageBuffer.Buffer,
                signatureBuffer.Buffer);
            return signedMessage;
        }


        /// <summary>
        /// This takes the given byte array and verifies it using SSPI VerifySignature method.
        /// </summary>
        /// <param name="messageToBeVerified">Signed message to be verified.</param>
        /// <param name="signature">The signature of the message.</param>
        /// <exception cref="SspiException">If verify fail, this exception will be thrown.</exception>
        public bool Verify(byte[] messageToBeVerified, byte[] signature)
        {
            SecurityBuffer messageBuffer = new SecurityBuffer(SecurityBufferType.Data, messageToBeVerified);
            SecurityBuffer signatureBuffer = new SecurityBuffer(SecurityBufferType.Token, signature);

            return Verify(messageBuffer, signatureBuffer);
        }


        /// <summary>
        /// Verify Data according SecBuffers.
        /// </summary>
        /// <param name="securityBuffers">SecBuffer array</param>
        /// <returns>True if the signature matches the signed message, otherwise false</returns>
        public abstract bool Verify(params SecurityBuffer[] securityBuffers);


        /// <summary>
        /// This takes the given message and verifies it using SSPI(VerifySignature). The given message
        /// should be formatted as follow:
        /// |MESSAGE_LENGTH(4 bytes)|MESSAGE
        /// </summary>
        /// <param name="messageToBeVerified">Signed message to be verified</param>
        /// <param name="signature">Signature</param>
        /// <returns>If true, verify successful, otherwise failed.</returns>
        /// <exception cref="SspiException">If verify fail, this exception will be thrown.</exception>
        /// <exception cref="ArgumentNullException">If messageToBeVerified is null, this exception will be thrown.
        /// </exception>
        /// <exception cref="ArgumentException">If messageToBeVerified is not formatted as 
        /// "MESSAGE_LENGTH(4 bytes)|MESSAGE", this exception will be thrown.</exception>
        public bool VerifyMessage(byte[] messageToBeVerified, byte[] signature)
        {
            if (!SspiUtility.VerifyMessageHeader(messageToBeVerified))
            {
                throw new ArgumentException(
                    "Value of message header is not consistent with the actual length of message.",
                    "messageToBeVerified");
            }
            
            //Remove header.
            byte[] messageBody = new byte[messageToBeVerified.Length - sizeof(int)];

            Array.Copy(messageToBeVerified, sizeof(int), messageBody, 0, messageBody.Length);
            return Verify(messageBody, signature);
        }


        /// <summary>
        /// Encrypts the message and returns another byte array containing encrypted message.
        /// Schannel is not supported.
        /// </summary>
        /// <param name="messageToBeEncrypted">Message to be encrypted.</param>
        /// <param name="signature">Generated signature</param>
        /// <returns>Encrypted message</returns>
        public byte[] Encrypt(byte[] messageToBeEncrypted, out byte[] signature)
        {
            SecurityBuffer messageBuffer = new SecurityBuffer(SecurityBufferType.Data, messageToBeEncrypted);
            SecurityBuffer tokenBuffer = new SecurityBuffer(
                SecurityBufferType.Token, 
                new byte[NativeMethods.MAX_TOKEN_SIZE]);

            Encrypt(messageBuffer, tokenBuffer);
            signature = tokenBuffer.Buffer;
            return messageBuffer.Buffer;
        }


        /// <summary>
        /// Encrypts Message. User decides what SecBuffers are used.
        /// </summary>
        /// <param name="securityBuffers">SecBuffers.</param>
        /// <exception cref="SspiException">If encrypt fail, this exception will be thrown.</exception>
        public abstract void Encrypt(params SecurityBuffer[] securityBuffers);


        /// <summary>
        /// Encrypts the message and returns another byte array containing message header and encrypted message.
        /// Schannel is not supported.
        /// The returned message will be formatted as follow:
        /// MESSAGE_LENGTH(4 bytes)|MESSAGE|Signature(optional)
        /// </summary>
        /// <param name="messageToBeEncrypted">Message to be encrypted.</param>
        /// <returns>encrypted message</returns>
        public byte[] EncryptMessage(byte[] messageToBeEncrypted)
        {
            byte[] encryptedMessage;
            byte[] signature;

            encryptedMessage = Encrypt(messageToBeEncrypted, out signature);
            int messageLength = encryptedMessage.Length;
            byte[] message = ArrayUtility.ConcatenateArrays(
                BitConverter.GetBytes(messageLength),
                encryptedMessage,
                signature);
            
            return message;
        }


        /// <summary>
        /// Decrypts the encrypted message and returns decrypted message.
        /// Schannel is not supported.
        /// </summary>
        /// <param name="messageToBeDecrypted">Message to be decrypted.</param>
        /// <param name="signature">Signature of the message, for windows sspi, signature can't be null.</param>
        /// <returns>Decrypted message</returns>
        public byte[] Decrypt(byte[] messageToBeDecrypted, byte[] signature)
        {
            SecurityBuffer messageBuffer = new SecurityBuffer(SecurityBufferType.Data, messageToBeDecrypted);
            SecurityBuffer tokenBuffer = new SecurityBuffer(SecurityBufferType.Token, signature);

            Decrypt(messageBuffer, tokenBuffer);
            return messageBuffer.Buffer;
        }


        /// <summary>
        /// This takes the given SecBuffers, which are used by SSPI method DecryptMessage.
        /// </summary>
        /// <param name="securityBuffers">SecBuffer.Encrypted data will be filled in SecBuffers.</param>
        /// <returns>If successful, returns true, otherwise false.</returns>
        /// <exception cref="SspiException">If sign fail, this exception will be thrown.</exception>
        public abstract bool Decrypt(params SecurityBuffer[] securityBuffers);


        /// <summary>
        /// Decrypts the encrypted message(contains message header) and returns decrypted message.
        /// Schannel is not supported.
        /// The given message must be formatted as follow:
        /// MESSAGE_LENGTH(4 bytes)|MESSAGE
        /// </summary>
        /// <param name="messageToBeDecrypted">Message to be decrypted</param>
        /// <returns>Decrypted message</returns>
        /// <exception cref="ArgumentNullException">If messageToBeDecrypted is null, this exception will be thrown.
        /// </exception>
        /// <exception cref="ArgumentException">If messageToBeDecrypted is not formatted as 
        /// "MESSAGE_LENGTH(4 bytes)|MESSAGE", this exception will be thrown.</exception>
        public byte[] DecryptMessage(byte[] messageToBeDecrypted)
        {
            if (!SspiUtility.VerifyMessageHeader(messageToBeDecrypted))
            {
                throw new ArgumentException(
                    "Value of message header is not consistent with the actual length of message.", 
                    "messageToBeDecrypted");
            }

            //Remove message header
            int messageLength = BitConverter.ToInt32(messageToBeDecrypted, 0);
            byte[] message = ArrayUtility.SubArray(messageToBeDecrypted, sizeof(int), messageLength);
            byte[] signature = ArrayUtility.SubArray(messageToBeDecrypted, sizeof(int) + messageLength);

            return Decrypt(message, signature);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Package type
        /// </summary>
        public abstract SecurityPackageType PackageType
        {
            get;
        }


        /// <summary>
        /// The token returned by Sspi.
        /// </summary>
        public abstract byte[] Token
        {
            get;
        }


        /// <summary>
        /// Whether to continue process.
        /// </summary>
        public abstract bool NeedContinueProcessing
        {
            get;
        }


        /// <summary>
        /// Gets or sets sequence number for Verify, Encrypt and Decrypt message.
        /// For Digest SSP, it must be 0.
        /// </summary>
        public abstract uint SequenceNumber
        {
            get;
            set;
        }


        /// <summary>
        /// The session Key
        /// </summary>
        public abstract byte[] SessionKey
        {
            get;
        }


        /// <summary>
        /// Queries the sizes of the structures used in the per-message functions.
        /// </summary>
        public abstract SecurityPackageContextSizes ContextSizes
        {
            get;
        }

        #endregion

    }
}
