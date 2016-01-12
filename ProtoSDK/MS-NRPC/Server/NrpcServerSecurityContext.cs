// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// NRPC server Security Support Provider Interface (SSPI).
    /// </summary>
    public class NrpcServerSecurityContext : ServerSecurityContext
    {

        private byte[] token;
        private NrpcClientSessionInfo clientSessionInfo;
        private ulong sequenceNumber;

        internal const uint SEC_E_INVALID_TOKEN = 0x80090308;
        private const char NULL = '\0';

        // Max length of NETBIOS name
        private const int MAX_LENGTH_OF_NETBIOS_NAME = 16;

        // Max length of DNS name
        private const int MAX_LENGTH_OF_DNS_NAME = 256;

        #region override properties
        /// <summary>
        /// Package type
        /// </summary>
        public override SecurityPackageType PackageType
        {
            get
            {
                return SecurityPackageType.NetLogon;
            }
        }


        /// <summary>
        /// The token returned by Sspi.
        /// </summary>
        public override byte[] Token
        {
            get
            {
                return token;
            }
        }


        /// <summary>
        /// Whether to continue process.
        /// </summary>
        public override bool NeedContinueProcessing
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// Gets or sets sequence number for Verify, Encrypt and Decrypt message.
        /// For Digest SSP, it must be 0.
        /// </summary>
        public override uint SequenceNumber
        {
            get
            {
                return (uint)sequenceNumber;
            }
            set
            {
                sequenceNumber = value;
            }
        }


        /// <summary>
        /// The session Key
        /// </summary>
        public override byte[] SessionKey
        {
            get
            {
                return clientSessionInfo.SessionKey;
            }
        }


        /// <summary>
        /// Queries the sizes of the structures used in the per-message functions.
        /// </summary>
        public override SecurityPackageContextSizes ContextSizes
        {
            get
            {
                int maxSignatureSize;
                if ((clientSessionInfo.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2)
                    == NrpcNegotiateFlags.SupportsAESAndSHA2)
                {
                    //Create an dummy instance of NL_AUTH_SHA2_SIGNATURE to get the size.
                    NL_AUTH_SHA2_SIGNATURE nlAuthSha2Signature = new NL_AUTH_SHA2_SIGNATURE();
                    nlAuthSha2Signature.SealAlgorithm = NL_AUTH_SHA2_SIGNATURE_SealAlgorithm_Values.AES128;
                    nlAuthSha2Signature.SequenceNumber = new byte[NrpcUtility.NL_AUTH_SIGNATURE_SEQNUM_LENGTH];
                    nlAuthSha2Signature.Checksum = new byte[NrpcUtility.NL_AUTH_SIGNATURE_CHECKSUM_LENGTH];
                    nlAuthSha2Signature.Confounder = new byte[NrpcUtility.NL_AUTH_SIGNATURE_CONFOUNDER_LENGTH];
                    nlAuthSha2Signature.Dummy = new byte[NrpcUtility.NL_AUTH_SHA2_SIGNATURE_DUMMY_LENGTH];
                    maxSignatureSize = TypeMarshal.GetBlockMemorySize(nlAuthSha2Signature);
                }
                else
                {
                    //Create an dummy instance of NL_AUTH_SIGNATURE to get the size.
                    NL_AUTH_SIGNATURE nlAuthSignature = new NL_AUTH_SIGNATURE();
                    nlAuthSignature.SealAlgorithm = SealAlgorithm_Values.RC4;
                    nlAuthSignature.Flags = new byte[NrpcUtility.NL_AUTH_SIGNATURE_FLAGS_LENGTH];
                    nlAuthSignature.SequenceNumber = new byte[NrpcUtility.NL_AUTH_SIGNATURE_SEQNUM_LENGTH];
                    nlAuthSignature.Checksum = new byte[NrpcUtility.NL_AUTH_SIGNATURE_CHECKSUM_LENGTH];
                    nlAuthSignature.Confounder = new byte[NrpcUtility.NL_AUTH_SIGNATURE_CONFOUNDER_LENGTH];
                    maxSignatureSize = TypeMarshal.GetBlockMemorySize(nlAuthSignature);
                }

                SecurityPackageContextSizes sizes = new SecurityPackageContextSizes();
                sizes.MaxTokenSize
                    = sizeof(uint) //length of MessageType 
                    + sizeof(uint) //length of Flags
                    + MAX_LENGTH_OF_NETBIOS_NAME // max length of a NetBIOS domain name
                    + MAX_LENGTH_OF_NETBIOS_NAME // max length of a NetBIOS computer name
                    + MAX_LENGTH_OF_DNS_NAME // max length of a DNS domain name
                    + MAX_LENGTH_OF_DNS_NAME // max length of a DNS host name
                    + MAX_LENGTH_OF_NETBIOS_NAME; // max length of NetBIOS computer name
                sizes.MaxSignatureSize = (uint)maxSignatureSize;
                // Indicates the minimum size of messages for encryption. This value MUST be 1.
                sizes.BlockSize = 1;
                sizes.SecurityTrailerSize = sizes.MaxSignatureSize;
                return sizes;
            }
        }
        #endregion

        /// <summary>
        /// Constructor, initialize a NRPC server security context
        /// </summary>
        /// <param name="nrpcClientSessionInfo">A struct carrying client session info</param>
        /// <exception cref="ArgumentException">Thrown when nrpcClientSessionInfo has some 
        /// important info missed.</exception>
        public NrpcServerSecurityContext(NrpcClientSessionInfo nrpcClientSessionInfo)
        {
            if (nrpcClientSessionInfo.ComputerName == null
                || nrpcClientSessionInfo.SessionKey == null
                || nrpcClientSessionInfo.SharedSecret == null
                || nrpcClientSessionInfo.ServerStoredCredential == null)
            {
                throw new ArgumentException(
                    "nrpcClientSessionInfo has important information missed", "nrpcClientSessionInfo");
            }

            clientSessionInfo.AccountRid = nrpcClientSessionInfo.AccountRid;
            clientSessionInfo.ClientSequenceNumber = nrpcClientSessionInfo.ClientSequenceNumber;
            clientSessionInfo.ComputerName = nrpcClientSessionInfo.ComputerName;
            clientSessionInfo.NegotiateFlags = nrpcClientSessionInfo.NegotiateFlags;
            clientSessionInfo.SecureChannelType = nrpcClientSessionInfo.SecureChannelType;
            clientSessionInfo.ServerSequenceNumber = nrpcClientSessionInfo.ServerSequenceNumber;
            clientSessionInfo.ServerStoredCredential = new byte[nrpcClientSessionInfo.ServerStoredCredential.Length];
            Array.Copy(nrpcClientSessionInfo.ServerStoredCredential,
                clientSessionInfo.ServerStoredCredential,
                nrpcClientSessionInfo.ServerStoredCredential.Length);
            clientSessionInfo.SessionKey = new byte[nrpcClientSessionInfo.SessionKey.Length];
            Array.Copy(nrpcClientSessionInfo.SessionKey,
                clientSessionInfo.SessionKey,
                nrpcClientSessionInfo.SessionKey.Length);
            clientSessionInfo.SharedSecret = nrpcClientSessionInfo.SharedSecret;
        }



        /// <summary>
        /// Constructor, initialize a NRPC server security context
        /// </summary>
        internal NrpcServerSecurityContext()
            : base()
        {
        }


        /// <summary>
        /// Accept client token.
        /// </summary>
        /// <param name="inToken">client token</param>
        /// <exception cref="ArgumentNullException">Thrown when inToken is null.</exception>
        /// <exception cref="SspiException">If accept fail, this exception will be thrown.</exception>
        public override void Accept(byte[] inToken)
        {
            if (inToken == null)
            {
                throw new ArgumentNullException("inToken");
            }

            if (!ValidateNlAuthMessage(inToken))
            {
                throw new SspiException("Accept failed.", SEC_E_INVALID_TOKEN);
            }

            this.sequenceNumber = 0;

            NL_AUTH_MESSAGE nlAuthMessage = CreateNlAuthMessage();

            this.token = ArrayUtility.ConcatenateArrays(
                BitConverter.GetBytes((uint)nlAuthMessage.MessageType),
                BitConverter.GetBytes((uint)nlAuthMessage.Flags),
                nlAuthMessage.Buffer);
        }


        /// <summary>
        /// Sign data according SecBuffers.
        /// </summary>
        /// <param name="securityBuffers">SecurityBuffer array</param>
        /// <exception cref="ArgumentNullException">Thrown when securityBuffers is null. </exception>
        /// <exception cref="ArgumentException">Thrown when length of securityBuffers is zero or
        /// securityBuffers have null element.</exception>
        public override void Sign(params SecurityBuffer[] securityBuffers)
        {
            if (securityBuffers == null)
            {
                throw new ArgumentNullException("securityBuffers");
            }

            if (securityBuffers.Length == 0)
            {
                throw new ArgumentException("Length of securityBuffer cannot be zero.", "securityBuffers");
            }

            for (int i = 0; i < securityBuffers.Length; i++)
            {
                if (securityBuffers[i] == null)
                {
                    throw new ArgumentException("securityBuffers have null element", "securityBuffers");
                }
            }

            NrpcUtility.InitialNetlogonSignatureToken(
                ((clientSessionInfo.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2) != 0),
                ref sequenceNumber,
                SessionKey,
                false,
                false,
                securityBuffers);
        }


        /// <summary>
        /// Verify Data according SecBuffers.
        /// </summary>
        /// <param name="securityBuffers">SecBuffer array</param>
        /// <returns>True if the signature matches the signed message, otherwise false</returns>
        /// <exception cref="ArgumentNullException">Thrown when securityBuffers is null. </exception>
        /// <exception cref="ArgumentException">Thrown when length of securityBuffers is zero or 
        /// securityBuffers have null element.</exception>
        public override bool Verify(params SecurityBuffer[] securityBuffers)
        {
            if (securityBuffers == null)
            {
                throw new ArgumentNullException("securityBuffers");
            }

            if (securityBuffers.Length == 0)
            {
                throw new ArgumentException("Length of securityBuffer cannot be zero.", "securityBuffers");
            }

            for (int i = 0; i < securityBuffers.Length; i++)
            {
                if (securityBuffers[i] == null)
                {
                    throw new ArgumentException("securityBuffers have null element", "securityBuffers");
                }
            }

            return NrpcUtility.ValidateNetlogonSignatureToken(
                ((clientSessionInfo.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2) != 0),
                ref sequenceNumber,
                SessionKey,
                false,
                true,
                securityBuffers);
        }


        /// <summary>
        /// Encrypts Message. User decides what SecBuffers are used.
        /// </summary>
        /// <param name="securityBuffers">SecBuffers.</param>
        /// <exception cref="ArgumentNullException">Thrown when securityBuffers is null. </exception>
        /// <exception cref="ArgumentException">Thrown when length of securityBuffers is zero or 
        /// securityBuffers have null element.</exception>
        public override void Encrypt(params SecurityBuffer[] securityBuffers)
        {
            if (securityBuffers == null)
            {
                throw new ArgumentNullException("securityBuffers");
            }

            if (securityBuffers.Length == 0)
            {
                throw new ArgumentException("Length of securityBuffer cannot be zero.", "securityBuffers");
            }

            for (int i = 0; i < securityBuffers.Length; i++)
            {
                if (securityBuffers[i] == null)
                {
                    throw new ArgumentException("securityBuffers have null element", "securityBuffers");
                }
            }

            NrpcUtility.InitialNetlogonSignatureToken(
                ((clientSessionInfo.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2) != 0),
                ref sequenceNumber,
                SessionKey,
                true,
                false,
                securityBuffers);
        }


        /// <summary>
        /// This takes the given SecBuffers, which are used by SSPI method DecryptMessage.
        /// </summary>
        /// <param name="securityBuffers">SecBuffer.Encrypted data will be filled in SecBuffers.</param>
        /// <returns>If successful, returns true, otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when securityBuffers is null. </exception>
        /// <exception cref="ArgumentException">Thrown when length of securityBuffers is zero or 
        /// securityBuffers have null element.</exception>
        public override bool Decrypt(params SecurityBuffer[] securityBuffers)
        {
            if (securityBuffers == null)
            {
                throw new ArgumentNullException("securityBuffers");
            }

            if (securityBuffers.Length == 0)
            {
                throw new ArgumentException("Length of securityBuffer cannot be zero.", "securityBuffers");
            }

            for (int i = 0; i < securityBuffers.Length; i++)
            {
                if (securityBuffers[i] == null)
                {
                    throw new ArgumentException("securityBuffers have null element", "securityBuffers");
                }
            }

            return NrpcUtility.ValidateNetlogonSignatureToken(
                ((clientSessionInfo.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2) != 0),
                ref sequenceNumber,
                SessionKey,
                true,
                true,
                securityBuffers);
        }


        #region Private Methods

        /// <summary>
        /// Validate NL_AUTH_MESSAGE when the server receives token from client
        /// MessageType is not set to 0x00000000. 
        /// contains at least one domain name and one computer name
        /// </summary>
        /// <param name="inToken">token from client</param>
        /// <returns>True if validate pass; otherwise, false</returns>
        /// <exception cref="ArgumentException">Thrown when length of inToken is not large enough.</exception>
        private bool ValidateNlAuthMessage(byte[] inToken)
        {
            if (inToken.Length <= (sizeof(MessageType_Values) + sizeof(NL_AUTH_MESSAGE_Flags_Value)))
            {
                throw new ArgumentException("The token is invalid", "inToken");
            }

            NL_AUTH_MESSAGE nlAuthMessage = new NL_AUTH_MESSAGE();

            // convert inToken to a NL_AUTH_MESSAGE structure
            int offset = 0;
            nlAuthMessage.MessageType = (MessageType_Values)BitConverter.ToInt32(inToken, offset);
            offset += sizeof(MessageType_Values);
            nlAuthMessage.Flags = (NL_AUTH_MESSAGE_Flags_Value)BitConverter.ToUInt32(inToken, offset);
            offset += sizeof(NL_AUTH_MESSAGE_Flags_Value);
            nlAuthMessage.Buffer = ArrayUtility.SubArray(inToken, offset, inToken.Length - offset);

            // check message type
            if (nlAuthMessage.MessageType != MessageType_Values.NegotiateRequest)
            {
                return false;
            }

            // check domain name and computer name, must present both
            string domainName = null;
            string computerName = null;
            List<string> nameList = new List<string>();

            foreach (string name in Encoding.ASCII.GetString(nlAuthMessage.Buffer).Split(NULL))
            {
                if (!string.IsNullOrEmpty(name))
                {
                    nameList.Add(name);
                }
            }

            if (nameList.Count == 0)
            {
                return false;
            }

            int index = 0;
            if ((nlAuthMessage.Flags & NL_AUTH_MESSAGE_Flags_Value.NetbiosOemDomainName) != 0
                && index < nameList.Count)
            {
                domainName = nameList[index++];
            }
            if ((nlAuthMessage.Flags & NL_AUTH_MESSAGE_Flags_Value.NetbiosOemComputerName) != 0
                && index < nameList.Count)
            {
                computerName = nameList[index++];
            }
            if ((nlAuthMessage.Flags & NL_AUTH_MESSAGE_Flags_Value.DnsCompressedDomainName) != 0
                && index < nameList.Count)
            {
                domainName = 
                    Rfc1035Utility.FromCompressedUtf8String(Encoding.ASCII.GetBytes(nameList[index++]));
            }
            if ((nlAuthMessage.Flags & NL_AUTH_MESSAGE_Flags_Value.DnsCompressedHostName) != 0
                && index < nameList.Count)
            {
                computerName = 
                    Rfc1035Utility.FromCompressedUtf8String(Encoding.ASCII.GetBytes(nameList[index++]));
            }
            if ((nlAuthMessage.Flags & NL_AUTH_MESSAGE_Flags_Value.NetbiosCompressedComputerName) != 0
                && index < nameList.Count)
            {
                computerName =
                    Rfc1035Utility.FromCompressedUtf8String(Encoding.ASCII.GetBytes(nameList[index++]));
            }

            if (domainName == null || computerName == null)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Create an instance of NL_AUTH_MESSAGE.
        /// </summary>
        /// <returns>Created NL_AUTH_MESSAGE structure</returns>
        private NL_AUTH_MESSAGE CreateNlAuthMessage()
        {
            NL_AUTH_MESSAGE nlAuthMessage = new NL_AUTH_MESSAGE();
            nlAuthMessage.MessageType = MessageType_Values.NegotiateResponse;
            nlAuthMessage.Flags = (NL_AUTH_MESSAGE_Flags_Value)0;
            nlAuthMessage.Buffer = BitConverter.GetBytes(NULL);

            return nlAuthMessage;
        }
        #endregion
    }
}
