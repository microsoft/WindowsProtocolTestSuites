// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// SecurityContext used by DTLS client.
    /// Supports DTLS 1.0
    /// Invokes InitializeSecurityContext function of SSPI
    /// </summary>
    public class DtlsClientSecurityContext : SspiClientSecurityContext
    {
        bool bStreamSizes;
        SecurityPackageContextStreamSizes dtlsStreamSizes;
        bool hasMoreFragments;
        uint lastHResult;

        /// <summary>
        /// Indicates if more fragments need to be output
        /// </summary>
        public bool HasMoreFragments
        {
            get { return hasMoreFragments; }
        }

        /// <summary>
        /// The SecPkgContext_StreamSizes structure indicates the sizes of the various stream components for use with the 
        /// message support functions.
        /// </summary>
        public SecurityPackageContextStreamSizes StreamSizes
        {
            [SecurityPermission(SecurityAction.Demand)]
            get
            {
                if (!bStreamSizes)
                {
                    object sizesObj = this.QueryContextAttributes("SECPKG_ATTR_STREAM_SIZES");
                    dtlsStreamSizes = (SecurityPackageContextStreamSizes)sizesObj;
                    bStreamSizes = true;
                }
                return dtlsStreamSizes;
            }
        }

        /// <summary>
        /// Constructor with client credential, principal of server, ContextAttributes and TargetDataRep.
        /// </summary>
        /// <param name="packageType">Specifies the name of the security package with which these credentials will be used
        /// </param>
        /// <param name="clientCredential">Client account credential</param>
        /// <param name="serverPrincipal">Server principal name</param>
        /// <param name="contextAttributes">Context attributes</param>
        /// <param name="targetDataRep">The data representation, such as byte ordering, on the target. 
        /// This parameter can be either SECURITY_NATIVE_DREP or SECURITY_NETWORK_DREP.</param>
        [SecurityPermission(SecurityAction.Demand)]
        public DtlsClientSecurityContext(
            SecurityPackageType packageType,
            CertificateCredential clientCredential,
            string serverPrincipal,
            ClientSecurityContextAttribute contextAttributes,
            SecurityTargetDataRepresentation targetDataRep)
        {
            if (clientCredential == null) clientCredential = new CertificateCredential(null);
            this.packageType = packageType;
            this.serverPrincipalName = serverPrincipal;
            this.securityContextAttributes = contextAttributes;
            this.targetDataRepresentaion = targetDataRep;

            SspiUtility.DtlsAcquireCredentialsHandle(
                packageType,
                clientCredential,
                serverPrincipal,
                NativeMethods.SECPKG_CRED_OUTBOUND,
                out this.credentialHandle);

            bStreamSizes = false;
            hasMoreFragments = false;
        }

        /// <summary>
        /// Initialize SecurityContext with a server token.
        /// </summary>
        /// <param name="serverToken">Server Token</param>
        /// <exception cref="SspiException">If Initialize fail, this exception will be thrown.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        public override void Initialize(byte[] serverToken)
        {
            uint hResult;
            SecurityBuffer[] inSecurityBuffers;
            //On calls to this function after the initial call, there must be two buffers.
            inSecurityBuffers = new SecurityBuffer[2];
            //The first has type SECBUFFER_TOKEN and contains the token received from the server.
            inSecurityBuffers[0] = new SecurityBuffer(SecurityBufferType.Token, serverToken);
            //The second buffer has type SECBUFFER_EMPTY; set both the pvBuffer and cbBuffer members to zero.
            inSecurityBuffers[1] = new SecurityBuffer(SecurityBufferType.Empty, new byte[0]);
            SecurityBufferDescWrapper serverTokenDescWrapper = new SecurityBufferDescWrapper(inSecurityBuffers);

            SecurityBuffer[] outSecurityBuffers;
            if(serverToken == null)
            {
                //First Initialize, only token
                outSecurityBuffers = new SecurityBuffer[1];
                outSecurityBuffers[0] = new SecurityBuffer(SecurityBufferType.Token, new byte[NativeMethods.MAX_TOKEN_SIZE]);
            }
            else
            { 
                //Token and Ablert
                outSecurityBuffers = new SecurityBuffer[2];
                outSecurityBuffers[0] = new SecurityBuffer(SecurityBufferType.Token, new byte[NativeMethods.MAX_TOKEN_SIZE]);
                outSecurityBuffers[1] = new SecurityBuffer(SecurityBufferType.Alert, new byte[NativeMethods.MAX_TOKEN_SIZE]);
            }
            SecurityBufferDescWrapper outBufferDescWrapper = new SecurityBufferDescWrapper(outSecurityBuffers);
            
            uint outContextAttribute;
            SecurityInteger expiryTime = new SecurityInteger();

            if (serverToken == null && (this.contextHandle.LowPart == IntPtr.Zero || this.contextHandle.HighPart == IntPtr.Zero))
            {
                hResult = NativeMethods.InitializeSecurityContext(
                    ref this.credentialHandle,
                    IntPtr.Zero,
                    this.serverPrincipalName,
                    (int)this.securityContextAttributes,
                    0,
                    (int)this.targetDataRepresentaion,
                    IntPtr.Zero,
                    0,
                    out this.contextHandle,
                    out outBufferDescWrapper.securityBufferDesc,
                    out outContextAttribute,
                    out expiryTime);
            }
            else
            {
                if (this.contextHandle.LowPart == IntPtr.Zero || this.contextHandle.HighPart == IntPtr.Zero)
                {
                    hResult = NativeMethods.InitializeSecurityContext(
                        ref this.credentialHandle,
                        IntPtr.Zero,
                        this.serverPrincipalName,
                        (int)this.securityContextAttributes,
                        0,
                        (int)this.targetDataRepresentaion,
                        ref serverTokenDescWrapper.securityBufferDesc,
                        0,
                        out this.contextHandle,
                        out outBufferDescWrapper.securityBufferDesc,
                        out outContextAttribute,
                        out expiryTime);
                }
                else
                {
                    hResult = NativeMethods.InitializeSecurityContext(
                        ref this.credentialHandle,
                        ref this.contextHandle,
                        this.serverPrincipalName,
                        (int)this.securityContextAttributes,
                        0,
                        (int)this.targetDataRepresentaion,
                        ref serverTokenDescWrapper.securityBufferDesc,
                        0,
                        out this.contextHandle,
                        out outBufferDescWrapper.securityBufferDesc,
                        out outContextAttribute,
                        out expiryTime);
                }
            }

            serverTokenDescWrapper.FreeSecurityBufferDesc();

            lastHResult = hResult;
            if (hResult == NativeMethods.SEC_E_OK)
            {
                this.needContinueProcessing = false;
                this.hasMoreFragments = false;
            }
            else if (hResult == NativeMethods.SEC_I_CONTINUE_NEEDED)
            {
                this.needContinueProcessing = true;
                this.hasMoreFragments = false;
            }
            else if (hResult == NativeMethods.SEC_I_MESSAGE_FRAGMENT)
            {
                this.needContinueProcessing = true;
                this.hasMoreFragments = true;
            }
            else
            {
                throw new SspiException("Initialize failed.", hResult);
            }

            //Get token if success.
            this.token = null;
            SspiSecurityBuffer[] outBuffers = outBufferDescWrapper.securityBufferDesc.GetBuffers();
            for (int i = 0; i < outBuffers.Length; i++)
            {
                if (outBuffers[i].bufferType == (uint)SecurityBufferType.Token)
                {
                    if (outBuffers[i].bufferLength > 0)
                    {
                        this.token = new byte[outBuffers[i].bufferLength];
                        Marshal.Copy(outBuffers[i].pSecBuffer, this.token, 0, this.token.Length);
                    }
                    break;
                }
            }
            outBufferDescWrapper.FreeSecurityBufferDesc();
        }

    }
}
