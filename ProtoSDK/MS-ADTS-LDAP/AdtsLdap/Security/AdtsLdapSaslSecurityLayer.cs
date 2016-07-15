// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// the SASL security layer, over SASL bind,<para/>
    /// which provides authentication and message security over Ntlm or Kerberos.
    /// </summary>
    public class AdtsLdapSaslSecurityLayer : AdtsLdapSecurityLayer
    {
        #region Fileds

        /// <summary>
        /// a const int value that specifies the initialize value of signature length.
        /// </summary>
        private const int SIGNATURE_LENGTH_INITIALIZE_VALUE = -1;

        /// <summary>
        /// a SecurityContext object that specifies the security provider.<para/>
        /// this field will never be null.
        /// </summary>
        private SecurityContext securityContext;

        /// <summary>
        /// an AdtsLdapSecurityBuffer object that manages the received data.<para/>
        /// this field will never be null.
        /// </summary>
        private AdtsLdapSecurityBuffer receivedBuffer;

        /// <summary>
        /// an int value that contains the length of signature.
        /// </summary>
        private int signatureLength;

        #endregion

        #region Properties

        /// <summary>
        /// set an int value that contains the length of signature.
        /// </summary>
        internal int SignatureLength
        {
            set
            {
                this.signatureLength = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">
        /// a SecurityContext object that specifies the security provider.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// thrown when context is null.
        /// </exception>
        public AdtsLdapSaslSecurityLayer(SecurityContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.securityContext = context;
            this.receivedBuffer = new AdtsLdapSecurityBuffer();
            this.signatureLength = SIGNATURE_LENGTH_INITIALIZE_VALUE;
        }

        #endregion

        #region Methods

        /// <summary>
        /// encoding the data with security provider.
        /// </summary>
        /// <param name="data">
        /// a bytes data that contains the data to be encoded with security provider.
        /// </param>
        /// <returns>
        /// a bytes data that contains the encoded data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// thrown when data is null.
        /// </exception>
        public override byte[] Encode(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            // if the SASL security provider is not ready, do not encode.
            if (this.securityContext.NeedContinueProcessing
                || !this.UsingMessageSecurity)
            {
                return data;
            }

            byte[] signature = null;
            byte[] encryptedMessage = null;

            encryptedMessage = this.securityContext.Encrypt(data, out signature);

            // update the signature length
            this.signatureLength = signature.Length;

            return ArrayUtility.ConcatenateArrays<byte>(
                new AdtsLdapSaslSecurityHeader(signature, encryptedMessage).ToBytes(),
                signature,
                encryptedMessage);
        }


        /// <summary>
        /// decoding the data with security provider
        /// </summary>
        /// <param name="data">
        /// a bytes data that contains the data to be decoded with security provider.
        /// </param>
        /// <returns>
        /// a bytes data that contains the decoded data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// thrown when data is null.
        /// </exception>
        public override byte[] Decode(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            // if the SASL security provider is not ready, do not decode.
            if (this.securityContext.NeedContinueProcessing
                || !this.UsingMessageSecurity)
            {
                this.consumedData = false;

                return data;
            }

            // store the data to buffer.
            this.receivedBuffer.AddReceivedData(data);
            this.consumedData = true;
            this.consumedLength = data.Length;

            // SaslHeader is used to decode the whole packet from buffer.
            AdtsLdapSaslSecurityHeader saslHeader = new AdtsLdapSaslSecurityHeader();

            // unmarshal the sasl header
            saslHeader.FromBytes(this.receivedBuffer.Peek(AdtsLdapSaslSecurityHeader.SIZE_OF_LENGTH));

            // if the sasl header is not valid, maybe data is not enough, consumed the received data and return.
            if (!saslHeader.IsValid(this.receivedBuffer.Length))
            {
                return new byte[0];
            }

            // update the signature length if needed
            if (this.signatureLength < 0)
            {
                this.signatureLength = (int)this.securityContext.ContextSizes.MaxSignatureSize;
            }

            // get a copy of data.
            byte[] bufferData = this.receivedBuffer.Data;

            // get the signature.
            byte[] signature = ArrayUtility.SubArray<byte>(
                bufferData, AdtsLdapSaslSecurityHeader.SIZE_OF_LENGTH, this.signatureLength);

            // get the encrypted message
            byte[] encryptedMessage = ArrayUtility.SubArray<byte>(
                bufferData, AdtsLdapSaslSecurityHeader.SIZE_OF_LENGTH + this.signatureLength,
                saslHeader.Length - this.signatureLength);

            // if the sasl header is valid, decrypt it with security provider.
            byte[] decryptedMessage = this.securityContext.Decrypt(encryptedMessage, signature);

            // remove the sasl header bytes data from the buffer.
            this.receivedBuffer.Remove(AdtsLdapSaslSecurityHeader.SIZE_OF_LENGTH + saslHeader.Length);

            return decryptedMessage;
        }

        #endregion
    }
}