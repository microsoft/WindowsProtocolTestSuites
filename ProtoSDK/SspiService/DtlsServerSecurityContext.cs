// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;
using System.Security.Permissions;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.SspiService
{
    /// <summary>
    /// Security context, which used by DTLS server.
    /// Accept client token to get server token.
    /// </summary>
    public class DtlsServerSecurityContext : IDisposable
    {
        protected IDtlsServerSecurityContext Context;

        /// <summary>
        /// Security package type.
        /// </summary>
        protected SecurityPackageType packageType;

        /// <summary>
        /// Server's principal name.
        /// </summary>
        protected string serverPrincipalName;

        /// <summary>
        /// Bit flags that indicate requests for the context.
        /// </summary>
        protected ServerSecurityContextAttribute securityContextAttributes;

        /// <summary>
        /// The data representation, such as byte ordering, on the target.
        /// </summary>
        protected SecurityTargetDataRepresentation targetDataRepresentaion;

        /// <summary>
        /// Indicates if more fragments need to be output
        /// </summary>
        public bool HasMoreFragments
        {
            get { return this.Context.HasMoreFragments; }
        }

        /// <summary>
        /// The SecPkgContext_StreamSizes structure indicates the sizes of the various stream components for use with the 
        /// message support functions.
        /// </summary>
        public SecurityPackageContextStreamSizes StreamSizes
        {
            get
            {
                return this.Context.StreamSizes;
            }
        }

        /// <summary>
        /// Whether to continue process.
        /// </summary>
        public bool NeedContinueProcessing
        {
            get
            {
                return this.Context.NeedContinueProcessing;
            }
        }

        /// <summary>
        /// Gets or sets sequence number for Verify, Encrypt and Decrypt message.
        /// For Digest SSP, it must be 0.
        /// </summary>
        public uint SequenceNumber
        {
            get
            {
                return this.Context.SequenceNumber;
            }
        }

        /// <summary>
        /// The session Key
        /// </summary>
        public byte[] SessionKey
        {
            get
            {
                return this.Context.SessionKey;
            }
        }

        /// <summary>
        /// The token returned by Sspi.
        /// </summary>
        public byte[] Token
        {
            get
            {
                return this.Context.Token;
            }
        }

        /// <summary>
        /// Queries the sizes of the structures used in the per-message functions.
        /// </summary>
        public SecurityPackageContextSizes ContextSizes
        {
            get
            {
                return this.Context.ContextSizes;
            }
        }

        /// <summary>
        /// Package-specific flags that indicate the quality of protection. A security package can use this parameter
        /// to enable the selection of cryptographic algorithms.
        /// </summary>
        public SECQOP_WRAP QualityOfProtection
        {
            get
            {
                return this.Context.QualityOfProtection;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="packageType">Specifies the name of the security package with which these credentials will be used
        /// </param>
        /// <param name="serverCredential">The credential of server, if null, use default user account.</param>
        /// <param name="serverPrincipal">Server principal name</param>
        /// <param name="contextAttributes">Bit flags that specify the attributes required by the server to establish 
        /// the context</param>
        /// <param name="targetDataRep">The data representation, such as byte ordering, on the target. This parameter 
        /// can be either SECURITY_NATIVE_DREP or SECURITY_NETWORK_DREP.</param>
        [SecurityPermission(SecurityAction.Demand)]
        public DtlsServerSecurityContext(
            SecurityPackageType packageType,
            CertificateCredential serverCredential,
            string serverPrincipal,
            ServerSecurityContextAttribute contextAttributes,
            SecurityTargetDataRepresentation targetDataRep)
        {
            this.packageType = packageType;
            this.serverPrincipalName = serverPrincipal;
            this.securityContextAttributes = contextAttributes;
            this.targetDataRepresentaion = targetDataRep;

            this.Context = new Sspi.DtlsServerSecurityContext(packageType, serverCredential, serverPrincipal, contextAttributes, targetDataRep);
        }

        /// <summary>
        /// Accept client token.
        /// </summary>
        /// <param name="clientToken">Token of client</param>
        /// <exception cref="SspiException">If Accept fail, this exception will be thrown.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        public void Accept(byte[] clientToken)
        {
            this.Context.Accept(clientToken);
        }

        /// <summary>
        /// Sign data according SecBuffers.
        /// </summary>
        /// <param name="securityBuffers">SecurityBuffer array</param>
        /// <exception cref="SspiException">If sign fail, this exception will be thrown.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        public void Sign(params SecurityBuffer[] securityBuffers)
        {
            this.Context.Sign(securityBuffers);
        }

        /// <summary>
        /// Encrypts Message. User decides what SecBuffers are used.
        /// </summary>
        /// <param name="securityBuffers">SecBuffers.</param>
        /// <exception cref="SspiException">If encrypt fail, this exception will be thrown.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        public bool Verify(params SecurityBuffer[] securityBuffers)
        {
            return this.Context.Verify(securityBuffers);
        }

        /// <summary>
        /// Encrypts Message. User decides what SecBuffers are used.
        /// </summary>
        /// <param name="securityBuffers">SecBuffers.</param>
        /// <exception cref="SspiException">If encrypt fail, this exception will be thrown.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        public void Encrypt(params SecurityBuffer[] securityBuffers)
        {
            this.Context.Encrypt(securityBuffers);
        }

        /// <summary>
        /// This takes the given SecBuffers, which are used by SSPI method DecryptMessage.
        /// </summary>
        /// <param name="securityBuffers">SecBuffer.Encrypted data will be filled in SecBuffers.</param>
        /// <returns>If successful, returns true, otherwise false.</returns>
        /// <exception cref="SspiException">If sign fail, this exception will be thrown.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        public bool Decrypt(params SecurityBuffer[] securityBuffers)
        {
            return this.Context.Decrypt(securityBuffers);
        }

        /// <summary>
        /// Dispose Dtls Server Security Context
        /// </summary>
        public void Dispose()
        {
            if (this.Context != null)
            {
                this.Context.Dispose();
            }
        }
    }
}
