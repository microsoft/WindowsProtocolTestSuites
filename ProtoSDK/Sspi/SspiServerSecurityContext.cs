// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// Security context, which used by server.
    /// Accept client token to get server token.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class SspiServerSecurityContext : ServerSecurityContext, IDisposable
    {
        /// <summary>
        /// Qulity of protection
        /// </summary>
        private SECQOP_WRAP qualityOfProtection;

        /// <summary>
        /// Flag to dispose
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Security package type.
        /// </summary>
        protected SecurityPackageType packageType;

        /// <summary>
        /// Whether continue to process.
        /// </summary>
        protected bool needContinueProcessing = true;

        /// <summary>
        /// Server's principal name.
        /// </summary>
        protected string serverPrincipalName;

        /// <summary>
        /// Sequence number for Verify, Encrypt and Decrypt message.
        /// </summary>
        protected uint sequenceNumber;

        /// <summary>
        /// Session key.
        /// </summary>
        protected byte[] sessionKey;

        /// <summary>
        /// Token.
        /// </summary>
        protected byte[] token;

        /// <summary>
        /// Bit flags that indicate requests for the context.
        /// </summary>
        protected ServerSecurityContextAttribute securityContextAttributes;

        /// <summary>
        /// The data representation, such as byte ordering, on the target.
        /// </summary>
        protected SecurityTargetDataRepresentation targetDataRepresentaion;

        /// <summary>
        /// Credential handle.
        /// </summary>
        protected SecurityHandle credentialHandle;

        /// <summary>
        /// Context handle.
        /// </summary>
        protected SecurityHandle contextHandle;

        /// <summary>
        /// Constructor.
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
        [SecurityPermission(SecurityAction.Demand)]
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
            SspiUtility.AcquireCredentialsHandle(
                packageType,
                serverCredential,
                serverPrincipal,
                NativeMethods.SECPKG_CRED_INBOUND,
                out this.credentialHandle);
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
            SspiUtility.AcquireCredentialsHandle(
                packageType,
                serverCredential,
                serverPrincipal,
                NativeMethods.SECPKG_CRED_INBOUND,
                out this.credentialHandle);
        }


        /// <summary>
        /// Destructor
        /// </summary>
        ~SspiServerSecurityContext()
        {
            Dispose(false);
        }


        /// <summary>
        /// Sign data according SecBuffers.
        /// </summary>
        /// <param name="securityBuffers">SecurityBuffer array</param>
        /// <exception cref="SspiException">If sign fail, this exception will be thrown.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        public override void Sign(params SecurityBuffer[] securityBuffers)
        {
            SspiUtility.MakeSignature(ref this.contextHandle, this.sequenceNumber, securityBuffers);
        }


        /// <summary>
        /// Encrypts Message. User decides what SecBuffers are used.
        /// </summary>
        /// <param name="securityBuffers">SecBuffers.</param>
        /// <exception cref="SspiException">If encrypt fail, this exception will be thrown.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        public override bool Verify(params SecurityBuffer[] securityBuffers)
        {
            return SspiUtility.VerifySignature(ref this.contextHandle, this.sequenceNumber, securityBuffers);
        }


        /// <summary>
        /// Encrypts Message. User decides what SecBuffers are used.
        /// </summary>
        /// <param name="securityBuffers">SecBuffers.</param>
        /// <exception cref="SspiException">If encrypt fail, this exception will be thrown.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        public override void Encrypt(params SecurityBuffer[] securityBuffers)
        {
            SspiUtility.Encrypt(ref this.contextHandle, this.sequenceNumber, this.qualityOfProtection, securityBuffers);
        }


        /// <summary>
        /// This takes the given SecBuffers, which are used by SSPI method DecryptMessage.
        /// </summary>
        /// <param name="securityBuffers">SecBuffer.Encrypted data will be filled in SecBuffers.</param>
        /// <returns>If successful, returns true, otherwise false.</returns>
        /// <exception cref="SspiException">If sign fail, this exception will be thrown.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        public override bool Decrypt(params SecurityBuffer[] securityBuffers)
        {
            return SspiUtility.Decrypt(ref this.contextHandle, this.sequenceNumber, securityBuffers);
        }


        /// <summary>
        /// Accept client token.
        /// </summary>
        /// <param name="clientToken">Token of client</param>
        /// <exception cref="SspiException">If Accept fail, this exception will be thrown.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        public override void Accept(byte[] clientToken)
        {
            SecurityBuffer[] securityBuffers;
            if (this.packageType == SecurityPackageType.CredSsp)
            {
                //There must be two buffers.
                securityBuffers = new SecurityBuffer[2];
                //The first buffer must be of type SECBUFFER_TOKEN and contain the security token 
                //received from the client.
                securityBuffers[0] = new SecurityBuffer(SecurityBufferType.Token, clientToken);
                //The second buffer should be of type SECBUFFER_EMPTY.
                securityBuffers[1] = new SecurityBuffer(SecurityBufferType.Empty, new byte[0]);
            }
            else
            {
                securityBuffers = new SecurityBuffer[] { new SecurityBuffer(SecurityBufferType.Token, clientToken) };
            }


            SecurityBufferDescWrapper inputBufferDescWrapper = new SecurityBufferDescWrapper(securityBuffers);
            SecurityBuffer serverTokenBuffer = new SecurityBuffer(
                SecurityBufferType.Token,
                new byte[NativeMethods.MAX_TOKEN_SIZE]);
            SecurityBufferDescWrapper outputBufferDescWrapper = new SecurityBufferDescWrapper(serverTokenBuffer);
            SecurityInteger timeStamp;
            uint contextAttribute;

            uint hResult = 0;

            if (this.contextHandle.LowPart == IntPtr.Zero && this.contextHandle.HighPart == IntPtr.Zero)
            {
                hResult = NativeMethods.AcceptSecurityContext(
                    ref this.credentialHandle,
                    IntPtr.Zero,
                    ref inputBufferDescWrapper.securityBufferDesc,
                    (uint)this.securityContextAttributes,
                    (uint)this.targetDataRepresentaion,
                    ref this.contextHandle,
                    out outputBufferDescWrapper.securityBufferDesc,
                    out contextAttribute,
                    out timeStamp);
            }
            else
            {
                hResult = NativeMethods.AcceptSecurityContext(
                    ref this.credentialHandle,
                    ref this.contextHandle,
                    ref inputBufferDescWrapper.securityBufferDesc,
                    (uint)this.securityContextAttributes,
                    (uint)this.targetDataRepresentaion,
                    ref this.contextHandle,
                    out outputBufferDescWrapper.securityBufferDesc,
                    out contextAttribute,
                    out timeStamp);
            }

            inputBufferDescWrapper.FreeSecurityBufferDesc();

            if (hResult == NativeMethods.SEC_E_OK)
            {
                this.needContinueProcessing = false;
            }
            else if (hResult == NativeMethods.SEC_I_CONTINUE_NEEDED)
            {
                this.needContinueProcessing = true;
            }
            else
            {
                throw new SspiException("Accept failed.", hResult);
            }
            //Get token
            this.token = null;
            SspiSecurityBuffer[] buffers = outputBufferDescWrapper.securityBufferDesc.GetBuffers();
            for (int i = 0; i < buffers.Length; i++)
            {
                if (buffers[i].bufferType == (uint)SecurityBufferType.Token)
                {
                    if (buffers[i].bufferLength > 0)
                    {
                        this.token = new byte[buffers[i].bufferLength];
                        Marshal.Copy(buffers[i].pSecBuffer, this.token, 0, this.token.Length);
                    }
                    break;
                }
            }
            outputBufferDescWrapper.FreeSecurityBufferDesc();
        }


        /// <summary>
        /// Release managed and un-managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release resources.
        /// </summary>
        /// <param name="disposing">If true, release all resource, otherwise release managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //release un-managed resources.
                    SspiUtility.DeleteSecurityContext(ref this.contextHandle);
                    SspiUtility.FreeCredentialsHandle(ref this.credentialHandle);
                }
                this.disposed = true;
            }
        }

        /// <summary>
        /// Query context attribute by Sspi QueryContextAttributes method.
        /// </summary>
        /// <param name="contextAttribute">Attribute name same as const name in msdn: 
        /// http://msdn.microsoft.com/en-us/library/aa379326(VS.85).aspx </param>
        /// <returns>The attribute value</returns>
        /// <exception cref="SspiException">If QueryContextAttributes fail, this exception will be thrown.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        public object QueryContextAttributes(string contextAttribute)
        {
            return SspiUtility.QueryContextAttributes(ref this.contextHandle, contextAttribute);
        }

        #region override properties
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
                return this.needContinueProcessing;
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
                return this.sequenceNumber;
            }
            set
            {
                this.sequenceNumber = value;
            }
        }


        /// <summary>
        /// The session Key
        /// </summary>
        public override byte[] SessionKey
        {
            [SecurityPermission(SecurityAction.Demand)]
            get
            {
                return SspiUtility.QuerySessionKey(this.packageType, ref this.contextHandle);
            }
        }


        /// <summary>
        /// The token returned by Sspi.
        /// </summary>
        public override byte[] Token
        {
            get
            {
                return this.token;
            }
        }


        /// <summary>
        /// Queries the sizes of the structures used in the per-message functions.
        /// </summary>
        public override SecurityPackageContextSizes ContextSizes
        {
            [SecurityPermission(SecurityAction.Demand)]
            get
            {
                return SspiUtility.QueryContextSizes(ref this.contextHandle);
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
    }
}
