// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    using System;
    using Microsoft.Protocols.TestTools.StackSdk;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc;
    using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

    /// <summary>
    ///  NRPC client Security Support Provider Interface (SSPI).
    /// </summary>
    public class NrpcCustomClientSecurityContext : NrpcClientSecurityContext
    {
        /// <summary>
        ///  A token containing information that is part
        ///  of the first message in establishing a security context between a client
        ///  and a server.
        /// </summary>
        private NL_AUTH_MESSAGE nlnlAuthMsgToken;

        /// <summary>
        /// A security token that defines the
        /// SHA2 authentication signature used by Netlogon to execute Netlogon methods
        /// over a secure channel.
        /// </summary>
        private NL_AUTH_SHA2_SIGNATURE nlnlAuthSha2Sign;

        /// <summary>
        /// A security token that defines the authentication
        /// signature used by Netlogon to execute Netlogon methods over a secure channel.
        /// </summary>
        private NL_AUTH_SIGNATURE nlnlAuthSign;

        /// <summary>
        /// Is Signature Correct
        /// </summary>
        private bool isSignatureCorrect;

        /// <summary>
        /// Initializes a new instance of the NrpcCustomClientSecurityContext class. By calling this constructor,
        /// the class will setup a new secure channel between client and server.
        /// </summary>
        /// <param name="domainName">The NRPC domain name.</param>
        /// <param name="serverName"> The NRPC server name.</param>
        /// <param name="credential">The credential to setup the secure channel.</param>
        /// <param name="requestConfidentiality">A Boolean setting that indicates that the caller is requiring 
        /// encryption of messages so that they cannot be read while in transit.
        ///  Requesting this service results in Netlogon encrypting the message.</param>
        /// <param name="clientCapabilities">The client capability.</param>
        public NrpcCustomClientSecurityContext(
            string domainName, 
            string serverName, 
            MachineAccountCredential credential, 
            bool requestConfidentiality,
            NrpcNegotiateFlags clientCapabilities)
            : base(domainName, serverName, credential, requestConfidentiality, clientCapabilities)
        {
        }

        /// <summary>
        /// Gets NL_AUTH_MESSAGE token if is secure channel.
        /// </summary>
        public NL_AUTH_MESSAGE NlAuthMsgToken
        {
            get
            {
                return this.nlnlAuthMsgToken;
            }
        }

        /// <summary>
        /// Gets NL_AUTH_SHA2_SIGNATURE token if AES is negotiate.
        /// </summary>
        public NL_AUTH_SHA2_SIGNATURE NlAuthSha2Sign
        {
            get
            {
                return this.nlnlAuthSha2Sign;
            }
        }

        /// <summary>
        /// Gets NL_AUTH_SIGNATURE if AES is not negotiate.
        /// </summary>
        public NL_AUTH_SIGNATURE NlAuthSign
        {
            get
            { 
                return this.nlnlAuthSign; 
            }
        }

        /// <summary>
        /// Gets a value indicating whether the signature is correct.
        /// </summary>
        public bool IsSignatureCorrect
        {
            get
            {
                return this.isSignatureCorrect;
            }
        }

        /// <summary>
        /// Initialize the security context and negotiate session key.
        /// </summary>
        /// <param name="inToken">The token returned by the server.</param>
        public override void Initialize(byte[] inToken)
        {
            if (null != inToken)
            {
                this.nlnlAuthMsgToken = new NL_AUTH_MESSAGE();
                int offset = 0;
                this.nlnlAuthMsgToken.MessageType = (MessageType_Values)BitConverter.ToInt32(inToken, offset);
                offset += sizeof(int);
                this.nlnlAuthMsgToken.Flags = (NL_AUTH_MESSAGE_Flags_Value)BitConverter.ToUInt32(inToken, offset);
                offset += sizeof(NL_AUTH_MESSAGE_Flags_Value);
                this.nlnlAuthMsgToken.Buffer = ArrayUtility.SubArray(inToken, offset, inToken.Length - offset);
            }

            base.Initialize(inToken);
        }

        /// <summary>
        /// Decrypt method decrypts the given encrypted message, and returns the plain-text message.
        /// </summary>
        /// <param name="securityBuffers">A list of security buffers that contains data to sign and output signature.
        /// </param>
        /// <returns>Returns true if signature is correct; otherwise, false.</returns>
        public override bool Decrypt(params SecurityBuffer[] securityBuffers)
        {
            if (null == securityBuffers || 0 == securityBuffers.Length)
            {
                throw new ArgumentNullException("securityBuffers");
            }

            foreach (SecurityBuffer buffer in securityBuffers)
            {
                if (null == buffer)
                {
                    throw new ArgumentException("one or more values in securityBuffers is NULL");
                }
            }

            byte[] token = SspiUtility.ConcatenateSecurityBuffers(securityBuffers, SecurityBufferType.Token);

            if (0 != (Context.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2))
            {
                this.nlnlAuthSha2Sign = TypeMarshal.ToStruct<NL_AUTH_SHA2_SIGNATURE>(token);
            }
            else
            {
                this.nlnlAuthSign = TypeMarshal.ToStruct<NL_AUTH_SIGNATURE>(token);
            }

            this.isSignatureCorrect = base.Decrypt(securityBuffers);
            return this.isSignatureCorrect;
        }

         /// <summary>
         ///  Verify method validates the signature against given plain-text message.
         /// </summary>
         /// <param name="securityBuffers">A list of security buffers that contains data to sign and output signature.
         /// </param>
         /// <returns> Returns true if signature is correct; otherwise, false.</returns>
        public override bool Verify(params SecurityBuffer[] securityBuffers)
        {
            if (null == securityBuffers || 0 == securityBuffers.Length)
            {
                throw new ArgumentNullException("securityBuffers");
            }

            foreach (SecurityBuffer buffer in securityBuffers)
            {
                if (null == buffer)
                {
                    throw new ArgumentException("one or more values in securityBuffers is NULL");
                }
            }

            byte[] token = SspiUtility.ConcatenateSecurityBuffers(securityBuffers, SecurityBufferType.Token);

            if (0 != (Context.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2))
            {
                this.nlnlAuthSha2Sign = TypeMarshal.ToStruct<NL_AUTH_SHA2_SIGNATURE>(token);
            }
            else
            {
                this.nlnlAuthSign = TypeMarshal.ToStruct<NL_AUTH_SIGNATURE>(token);
            }

            this.isSignatureCorrect = base.Verify(securityBuffers);
            return this.isSignatureCorrect;
        }
    }
}
