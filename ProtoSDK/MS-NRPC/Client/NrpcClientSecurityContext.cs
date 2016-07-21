// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    /// NRPC client Security Support Provider Interface (SSPI).
    /// </summary>
    public class NrpcClientSecurityContext : ClientSecurityContext, IDisposable
    {
        // Max length of NETBIOS name
        private const int MAX_LENGTH_OF_NETBIOS_NAME = 16;

        // Max lengh of DNS name
        private const int MAX_LENGTH_OF_DNS_NAME = 256;

        // Timeout for negotiate RPC call
        private readonly TimeSpan timeout = TimeSpan.FromMinutes(1);

        // Credential to setup secure channel
        private MachineAccountCredential credential;

        // Token
        private byte[] token;

        // Need continue processing
        private bool needContinueProcessing;

        // NRPC client to netogiate session key
        internal NrpcClient nrpc;

        // the type of secure channel to use in a logon transaction.
        private _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType;


        /// <summary>
        /// Initialize an instance of NrpcClientSecurityContext class. 
        /// By calling this constructor, the class will setup a new secure 
        /// channel between client and server.
        /// </summary>
        /// <param name="domainName">
        /// The NRPC domain name.
        /// </param>
        /// <param name="serverName">
        /// The NRPC server name.
        /// </param>
        /// <param name="credential">
        /// The credential to setup the secure channel.
        /// </param>
        /// <param name="requestConfidentiality">
        /// A Boolean setting that indicates that the caller is requiring 
        /// encryption of messages so that they cannot be read while in transit. 
        /// Requesting this service results in Netlogon encrypting the message.
        /// </param>
        /// <param name="clientCapabilities">
        /// The client capability.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when domainName, serverName or credential is null.
        /// </exception>
        public NrpcClientSecurityContext(
            string domainName,
            string serverName,
            MachineAccountCredential credential,
            bool requestConfidentiality,
            NrpcNegotiateFlags clientCapabilities)
        {
            if (string.IsNullOrWhiteSpace(domainName))
            {
                throw new ArgumentNullException("domainName cannot be null or empty");
            }
            if (string.IsNullOrWhiteSpace(serverName))
            {
                throw new ArgumentNullException("serverName cannot be null or empty");
            }
            if (credential == null)
            {
                throw new ArgumentNullException("credential cannot be null");
            }

            this.nrpc = NrpcClient.CreateNrpcClient(domainName);
            this.nrpc.Context.PrimaryName = serverName;
            this.nrpc.Context.SealSecureChannel = requestConfidentiality;
            this.nrpc.Context.NegotiateFlags = clientCapabilities;

            this.credential = credential;
            this.secureChannelType = _NETLOGON_SECURE_CHANNEL_TYPE.WorkstationSecureChannel;
        }


        /// <summary>
        /// Initialize an instance of NrpcClientSecurityContext class. 
        /// By calling this constructor, the class will setup a new secure 
        /// channel between client and server.
        /// </summary>
        /// <param name="domainName">
        /// The NRPC domain name.
        /// </param>
        /// <param name="serverName">
        /// The NRPC server name.
        /// </param>
        /// <param name="credential">
        /// The credential to setup the secure channel.
        /// </param>
        /// <param name="requestConfidentiality">
        /// A Boolean setting that indicates that the caller is requiring 
        /// encryption of messages so that they cannot be read while in transit. 
        /// Requesting this service results in Netlogon encrypting the message.
        /// </param>
        /// <param name="clientCapabilities">
        /// The client capability.
        /// </param>
        /// <param name="secureChannelType">
        /// the type of secure channel to use in a logon transaction.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when domainName, serverName or credential is null.
        /// </exception>
        public NrpcClientSecurityContext(
            string domainName,
            string serverName,
            MachineAccountCredential credential,
            bool requestConfidentiality,
            NrpcNegotiateFlags clientCapabilities,
            _NETLOGON_SECURE_CHANNEL_TYPE secureChannelType
            )
            : this(domainName,
              serverName,
              credential,
              requestConfidentiality,
              clientCapabilities)
        {
            this.secureChannelType = secureChannelType;
        }


        #region Properties - Context

        /// <summary>
        /// NRPC client context.
        /// </summary>
        public NrpcClientContext Context
        {
            get
            {
                return nrpc.Context;
            }
        }

        #endregion


        /// <summary>
        /// Initialize method is not used for NRPC SSPI.<para/>
        /// NRPC SSPI will negotiate security context in its own RPC call. 
        /// </summary>
        /// <param name="inToken">
        /// A token returned from server SSPI; 
        /// if it's set to null, indicates to initialize a new client token.
        /// </param>
        /// <exception cref="SspiException">
        /// Thrown when server returned token is invalid.
        /// </exception>
        public override void Initialize(byte[] inToken)
        {
            if (inToken == null)
            {
                //Initialize a new token.
                if (nrpc.Context.SessionKey == null)
                {
                    //Negotiate a session key.
                    NrpcNegotiateFlags clientCapabilities = nrpc.Context.NegotiateFlags;

                    ushort[] nrpcTcpEndpoints = NrpcUtility.QueryNrpcTcpEndpoint(nrpc.Context.PrimaryName);
                    if (nrpcTcpEndpoints == null || nrpcTcpEndpoints.Length == 0)
                    {
                        throw new InvalidOperationException("Server doesn't support NRPC protocol.");
                    }

                    nrpc.BindOverTcp(
                        nrpc.Context.PrimaryName,
                        nrpcTcpEndpoints[0],
                        null,
                        timeout);
                    nrpc.NetrServerReqChallenge(credential.MachineName);
                    nrpc.NetrServerAuthenticate3(
                        credential.AccountName,
                        this.secureChannelType,
                        ref clientCapabilities,
                        credential.Password);
                }

                NL_AUTH_MESSAGE nlAuthMessage = nrpc.CreateNlAuthMessage();
                this.token = ArrayUtility.ConcatenateArrays(
                    BitConverter.GetBytes((uint)nlAuthMessage.MessageType),
                    BitConverter.GetBytes((uint)nlAuthMessage.Flags),
                    nlAuthMessage.Buffer);
                this.needContinueProcessing = true;
            }
            else
            {
                this.token = null;
                this.needContinueProcessing = false;

                NL_AUTH_MESSAGE nlAuthMessage = new NL_AUTH_MESSAGE();
                int offset = 0;
                nlAuthMessage.MessageType = (MessageType_Values)BitConverter.ToInt32(inToken, offset);
                offset += sizeof(int); // cannot call: Marshal.SizeOf(nlAuthMessage.MessageType);
                nlAuthMessage.Flags = (NL_AUTH_MESSAGE_Flags_Value)BitConverter.ToUInt32(inToken, offset);
                offset += sizeof(NL_AUTH_MESSAGE_Flags_Value);
                nlAuthMessage.Buffer = ArrayUtility.SubArray(inToken, offset, inToken.Length - offset);
                if (!nrpc.ValidateNlAuthMessage(nlAuthMessage))
                {
                    //validate server returned token failed.
                    throw new SspiException("Server returned token is invalid.");
                }
            }
        }


        /// <summary>
        /// Session Key
        /// </summary>
        public override byte[] SessionKey
        {
            get
            {
                return nrpc.Context.SessionKey;
            }
        }


        /// <summary>
        /// Sign method makes a signature for given plain-text message.
        /// </summary>
        /// <param name="securityBuffers">
        /// A list of security buffers that contains data to sign and output signature.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when length of securityBuffers is zero.
        /// </exception>
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

            nrpc.Sign(securityBuffers);
        }


        /// <summary>
        /// Verify method validates the signature against given plain-text message.
        /// </summary>
        /// <param name="securityBuffers">
        /// A list of security buffers that contains data to sign and output signature.
        /// </param>
        /// <returns>
        /// Returns true if signature is correct; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when length of securityBuffers is zero.
        /// </exception>
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

            return nrpc.Verify(securityBuffers);
        }


        /// <summary>
        /// Encrypt method encrypts the given plain-text message.
        /// </summary>
        /// <param name="securityBuffers">
        /// A list of security buffers that contains data to sign and output signature.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when length of securityBuffers is zero.
        /// </exception>
        /// <exception cref="SspiException">
        /// Thrown when total length of securityBuffers is not enough to contain encrypted data.
        /// </exception>
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

            nrpc.Encrypt(securityBuffers);
        }


        /// <summary>
        /// Decrypt method decrypts the given encrypted message, 
        /// and returns the plain-text message.
        /// </summary>
        /// <param name="securityBuffers">
        /// A list of security buffers that contains data to sign and output signature.
        /// </param>
        /// <returns>
        /// Returns true if signature is correct; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when length of securityBuffers is zero.
        /// </exception>
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

            return nrpc.Decrypt(securityBuffers);
        }


        /// <summary>
        /// Security package type.
        /// </summary>
        public override SecurityPackageType PackageType
        {
            get
            {
                return SecurityPackageType.NetLogon;
            }
        }


        /// <summary>
        /// Token, it should be send to server SSPI if not null.
        /// </summary>
        public override byte[] Token
        {
            get
            {
                return token;
            }
        }


        /// <summary>
        /// Indicates whether client needs to continue processing server returned token. 
        /// Always return false.
        /// </summary>
        public override bool NeedContinueProcessing
        {
            get
            {
                return needContinueProcessing;
            }
        }


        /// <summary>
        /// Sequence number. 
        /// NRPC maintains a ulong sequence number, the returned value will be truncated to uint.
        /// </summary>
        public override uint SequenceNumber
        {
            get
            {
                return (uint)nrpc.Context.SequenceNumber;
            }
            set
            {
                nrpc.Context.SequenceNumber = value;
            }
        }


        /// <summary>
        /// A structure contains sizes of security package context.
        /// </summary>
        public override SecurityPackageContextSizes ContextSizes
        {
            get
            {
                int maxSignatureSize;
                if ((nrpc.Context.NegotiateFlags & NrpcNegotiateFlags.SupportsAESAndSHA2)
                    == NrpcNegotiateFlags.SupportsAESAndSHA2)
                {
                    //Create an dummy instance of NL_AUTH_SHA2_SIGNATURE to get the size.
                    NL_AUTH_SHA2_SIGNATURE nlAuthSha2Signature = new NL_AUTH_SHA2_SIGNATURE();
                    nlAuthSha2Signature.SealAlgorithm =
                        nrpc.Context.SealSecureChannel
                        ? NL_AUTH_SHA2_SIGNATURE_SealAlgorithm_Values.AES128
                        : NL_AUTH_SHA2_SIGNATURE_SealAlgorithm_Values.NotEncrypted;
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
                    nlAuthSignature.SealAlgorithm =
                        nrpc.Context.SealSecureChannel
                        ? SealAlgorithm_Values.RC4
                        : SealAlgorithm_Values.NotEncrypted;
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
                sizes.BlockSize = nrpc.Context.SealSecureChannel ? (uint)1 : 0;
                sizes.SecurityTrailerSize = nrpc.Context.SealSecureChannel ? sizes.MaxSignatureSize : 0;
                return sizes;
            }
        }


        /// <summary>
        /// A Boolean setting that indicates that the caller is requiring 
        /// encryption of messages so that they cannot be read while in transit. 
        /// Requesting this service results in Netlogon encrypting the message.
        /// </summary>
        public bool Confidentiality
        {
            get
            {
                return nrpc.Context.SealSecureChannel;
            }
            set
            {
                nrpc.Context.SealSecureChannel = value;
            }
        }


        #region IDisposable Members

        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Dispose method.
        /// </summary>
        /// <param name="disposing">
        /// True to release both managed and unmanaged resources.<para/>
        /// False to release unmanaged resources only.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources.
                if (nrpc != null)
                {
                    nrpc.Dispose();
                    nrpc = null;
                }
            }

            // Release unmanaged resources.
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~NrpcClientSecurityContext()
        {
            Dispose(false);
        }

        #endregion
    }
}
