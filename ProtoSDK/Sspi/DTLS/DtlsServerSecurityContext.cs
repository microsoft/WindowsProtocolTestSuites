// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;


namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// Security context, which used by DTLS server.
    /// Accept client token to get server token.
    /// </summary>
    public class DtlsServerSecurityContext : SspiServerSecurityContext
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
            SspiUtility.DtlsAcquireCredentialsHandle(
                packageType,
                serverCredential,
                serverPrincipal,
                NativeMethods.SECPKG_CRED_INBOUND,
                out this.credentialHandle);

            bStreamSizes = false;
            hasMoreFragments = false;
        }

        /// <summary>
        /// Accept client token.
        /// </summary>
        /// <param name="clientToken">Token of client</param>
        /// <exception cref="SspiException">If Accept fail, this exception will be thrown.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        public override void Accept(byte[] clientToken)
        {
            SecurityBuffer[] inSecurityBuffers;
            //There must be two buffers.
            inSecurityBuffers = new SecurityBuffer[3];
            //The first buffer must be of type SECBUFFER_TOKEN and contain the security token 
            //received from the client.
            inSecurityBuffers[0] = new SecurityBuffer(SecurityBufferType.Token, clientToken);
            //The second buffer should be of type SECBUFFER_EMPTY.
            inSecurityBuffers[1] = new SecurityBuffer(SecurityBufferType.Empty, new byte[0]);
            //The 3nd buffer should be of type Extra.
            inSecurityBuffers[2] = new SecurityBuffer(SecurityBufferType.Extra, new byte[0]);
            SecurityBufferDescWrapper inputBufferDescWrapper = new SecurityBufferDescWrapper(inSecurityBuffers);

            SecurityBuffer[] outSecurityBuffers;
            outSecurityBuffers = new SecurityBuffer[2];
            //1 token
            outSecurityBuffers[0] = new SecurityBuffer(SecurityBufferType.Token, new byte[NativeMethods.MAX_TOKEN_SIZE]);
            //2 alert
            outSecurityBuffers[1] = new SecurityBuffer(SecurityBufferType.Alert, new byte[NativeMethods.MAX_TOKEN_SIZE]);
            SecurityBufferDescWrapper outputBufferDescWrapper = new SecurityBufferDescWrapper(outSecurityBuffers);

            SecurityInteger timeStamp;
            uint contextAttribute;

            uint hResult = 0;

            if (this.contextHandle.LowPart == IntPtr.Zero || this.contextHandle.HighPart == IntPtr.Zero)
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
    }
}
