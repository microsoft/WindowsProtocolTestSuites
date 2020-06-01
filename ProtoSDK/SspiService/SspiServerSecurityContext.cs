// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp;
using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.SspiService
{
    public class SspiServerSecurityContext: ServerSecurityContext, IDisposable
    {
        #region fields
        protected ServerSecurityContext Context;

        /// <summary>
        /// Server's principal name.
        /// </summary>
        protected string serverPrincipalName;

        /// <summary>
        /// Security package type.
        /// </summary>
        protected SecurityPackageType packageType;

        /// <summary>
        /// Quality of protection
        /// </summary>
        private SECQOP_WRAP qualityOfProtection;

        /// <summary>
        /// Bit flags that indicate requests for the context.
        /// </summary>
        protected ServerSecurityContextAttribute securityContextAttributes;

        /// <summary>
        /// The data representation, such as byte ordering, on the target.
        /// </summary>
        protected SecurityTargetDataRepresentation targetDataRepresentaion;

        #endregion

        #region properties
        /// <summary>
        /// Gets server principal name
        /// </summary>
        public virtual string ServerPrincipalName
        {
            get
            {
                return this.serverPrincipalName;
            }
        }

        /// <summary>
        /// Package type
        /// </summary>
        public override SecurityPackageType PackageType
        {
            get
            {
                return this.packageType;
            }
        }

        /// <summary>
        /// Whether to continue process.
        /// </summary>
        public override bool NeedContinueProcessing
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
        public override uint SequenceNumber
        {
            get
            {
                return this.Context.SequenceNumber;
            }
            set
            {
                this.Context.SequenceNumber = value;
            }
        }

        /// <summary>
        /// The session Key
        /// </summary>
        public override byte[] SessionKey
        {
            get
            {
                return this.Context.SessionKey;
            }
        }

        /// <summary>
        /// The token returned by Sspi.
        /// </summary>
        public override byte[] Token
        {
            get
            {
                return this.Context.Token;
            }
        }

        /// <summary>
        /// Queries the sizes of the structures used in the per-message functions.
        /// </summary>
        public override SecurityPackageContextSizes ContextSizes
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
                return this.qualityOfProtection;
            }
            set
            {
                this.qualityOfProtection = value;
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        protected SspiServerSecurityContext()
        {

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
        public SspiServerSecurityContext(
            SecurityPackageType packageType,
            AccountCredential serverCredential,
            string serverPrincipal,
            ServerSecurityContextAttribute contextAttributes,
            SecurityTargetDataRepresentation targetDataRep)
        {
            this.packageType = packageType;
            this.serverPrincipalName = serverPrincipal;
            this.securityContextAttributes = contextAttributes;
            this.targetDataRepresentaion = targetDataRep;
            this.AcquireCredentialsHandle(serverCredential);
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
        public SspiServerSecurityContext(
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
            this.AcquireCredentialsHandle(serverCredential);
        }

        /// <summary>
        /// AcquireCredentialsHandle
        /// </summary>
        /// <param name="accountCredential"></param>
        protected void AcquireCredentialsHandle<T>(T accountCredential)
        {
            switch (packageType)
            {
                case SecurityPackageType.Ntlm:
                    {
                        if (accountCredential is AccountCredential)
                        {
                            var credential = accountCredential as AccountCredential;
                            this.Context = new NlmpServerSecurityContext(
                                        NegotiateTypes.NTLM_NEGOTIATE_OEM | NegotiateTypes.NTLMSSP_NEGOTIATE_NTLM,
                                        new NlmpClientCredential(string.Empty, credential.DomainName,
                                            credential.AccountName, credential.Password),
                                        !string.IsNullOrEmpty(credential.DomainName),
                                        credential.DomainName,
                                        this.serverPrincipalName
                            );
                        }
                        else
                        {
                            throw new NotSupportedException("NTLM only support AccountCredential, Please provide an AccountCredential and try again.");
                        }
                        return;
                    }
                    //case SecurityPackageType.Kerberos:
                    //    throw new NotImplementedException();
                    //case SecurityPackageType.Negotiate:
                    //    throw new NotImplementedException();
                    //case SecurityPackageType.Schannel:
                    //    throw new NotImplementedException();
                    //case SecurityPackageType.CredSsp:
                    //    throw new NotImplementedException();
                    //default:
                    //    throw new NotImplementedException();
            }

            this.UseNativeSSP(accountCredential);
        }

        /// <summary>
        /// Use Native SSP to do auth
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accountCredential"></param>
        private void UseNativeSSP<T>(T accountCredential)
        {
            if (accountCredential is AccountCredential)
            {
                this.Context = new Sspi.SspiServerSecurityContext(
                    packageType,
                    accountCredential as AccountCredential,
                    this.ServerPrincipalName,
                    securityContextAttributes,
                    targetDataRepresentaion
               );
            }
            else if (accountCredential is CertificateCredential)
            {
                this.Context = new Sspi.SspiServerSecurityContext(
                    packageType,
                    accountCredential as CertificateCredential,
                    this.ServerPrincipalName,
                    securityContextAttributes,
                    targetDataRepresentaion
               );
            }
        }

        #region

        /// <summary>
        /// Sign data according SecBuffers.
        /// </summary>
        /// <param name="securityBuffers">SecurityBuffer array</param>
        /// <exception cref="SspiException">If sign fail, this exception will be thrown.</exception>
        public override void Sign(params SecurityBuffer[] securityBuffers)
        {
            this.Context.Sign(securityBuffers);
        }


        /// <summary>
        /// Encrypts Message. User decides what SecBuffers are used.
        /// </summary>
        /// <param name="securityBuffers">SecBuffers.</param>
        /// <exception cref="SspiException">If encrypt fail, this exception will be thrown.</exception>
        public override bool Verify(params SecurityBuffer[] securityBuffers)
        {
            return this.Context.Verify(securityBuffers);
        }


        /// <summary>
        /// Encrypts Message. User decides what SecBuffers are used.
        /// </summary>
        /// <param name="securityBuffers">SecBuffers.</param>
        /// <exception cref="SspiException">If encrypt fail, this exception will be thrown.</exception>
        public override void Encrypt(params SecurityBuffer[] securityBuffers)
        {
            this.Context.Encrypt(securityBuffers);
        }


        /// <summary>
        /// This takes the given SecBuffers, which are used by SSPI method DecryptMessage.
        /// </summary>
        /// <param name="securityBuffers">SecBuffer.Encrypted data will be filled in SecBuffers.</param>
        /// <returns>If successful, returns true, otherwise false.</returns>
        /// <exception cref="SspiException">If sign fail, this exception will be thrown.</exception>
        public override bool Decrypt(params SecurityBuffer[] securityBuffers)
        {
            return this.Context.Decrypt(securityBuffers);
        }

        public override object QueryContextAttributes(string contextAttribute)
        {
            return this.Context.QueryContextAttributes(contextAttribute);
        }

        /// <summary>
        /// Accept client token.
        /// </summary>
        /// <param name="clientToken">Token of client</param>
        /// <exception cref="SspiException">If Accept fail, this exception will be thrown.</exception>
        public override void Accept(byte[] clientToken)
        {
            this.Context.Accept(clientToken);
        }

        /// <summary>
        /// Dispose Server Security Context
        /// </summary>
        public void Dispose()
        {
            this.Context = null;
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
