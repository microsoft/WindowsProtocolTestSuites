// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// Define p/invoke of Windows SSPI
    /// </summary>
    internal static class NativeMethods
    {
        internal const int MAX_TOKEN_SIZE = 12288;

        internal const uint SEC_E_OK = 0;
        internal const uint SEC_I_CONTINUE_NEEDED = 0x00090312;
        internal const uint SEC_I_COMPLETE_NEEDED = 0x00090313;
        internal const uint SEC_I_COMPLETE_AND_CONTINUE = 0x00090314;
        internal const uint SEC_I_MESSAGE_FRAGMENT = 0x00090364;

        internal const uint SECPKG_CRED_INBOUND = 1;
        internal const uint SECPKG_CRED_OUTBOUND = 2;

        // Security Context Attributes: defined in sspi.h
        // reference: http://msdn.microsoft.com/en-us/library/aa379326(VS.85).aspx
        internal const uint SECPKG_ATTR_SIZES = 0;
        internal const uint SECPKG_ATTR_NAMES = 1;
        internal const uint SECPKG_ATTR_LIFESPAN = 2;
        internal const uint SECPKG_ATTR_DCE_INFO = 3;
        internal const uint SECPKG_ATTR_STREAM_SIZES = 4;
        internal const uint SECPKG_ATTR_KEY_INFO = 5;
        internal const uint SECPKG_ATTR_AUTHORITY = 6;
        internal const uint SECPKG_ATTR_PROTO_INFO = 7;
        internal const uint SECPKG_ATTR_PASSWORD_EXPIRY = 8;
        internal const uint SECPKG_ATTR_SESSION_KEY = 9;
        internal const uint SECPKG_ATTR_PACKAGE_INFO = 10;
        internal const uint SECPKG_ATTR_USER_FLAGS = 11;
        internal const uint SECPKG_ATTR_NEGOTIATION_INFO = 12;
        internal const uint SECPKG_ATTR_NATIVE_NAMES = 13;
        internal const uint SECPKG_ATTR_FLAGS = 14;
        // These attributes exist only in Win XP and greater
        internal const uint SECPKG_ATTR_USE_VALIDATED = 15;
        internal const uint SECPKG_ATTR_CREDENTIAL_NAME = 16;
        internal const uint SECPKG_ATTR_TARGET_INFORMATION = 17;
        internal const uint SECPKG_ATTR_ACCESS_TOKEN = 18;
        // These attributes exist only in Win2K3 and greater
        internal const uint SECPKG_ATTR_TARGET = 19;
        internal const uint SECPKG_ATTR_AUTHENTICATION_ID = 20;
        // These attributes exist only in Win2K3SP1 and greater
        internal const uint SECPKG_ATTR_LOGOFF_TIME = 21;
        //Defined in schannel.h
        internal const uint SECPKG_ATTR_REMOTE_CERT_CONTEXT = 0x53;   // returns PCCERT_CONTEXT
        internal const uint SECPKG_ATTR_LOCAL_CERT_CONTEXT = 0x54;   // returns PCCERT_CONTEXT
        internal const uint SECPKG_ATTR_ROOT_STORE = 0x55;   // returns HCERTCONTEXT to the root store
        internal const uint SECPKG_ATTR_ISSUER_LIST_EX = 0x59;   // returns SecPkgContext_IssuerListInfoEx
        internal const uint SECPKG_ATTR_CONNECTION_INFO = 0x5a;   // returns SecPkgContext_ConnectionInfo
        internal const uint SECPKG_ATTR_EAP_KEY_BLOCK = 0x5b;   // returns SecPkgContext_EapKeyBlock
        internal const uint SECPKG_ATTR_APP_DATA = 0x5e;  // sets/returns SecPkgContext_SessionAppData

        // Specifies the version number of SecBufferDesc.
        internal const int SECBUFFER_VERSION = 0;
        //Schannel version
        internal const uint SCHANNEL_CRED_VERSION = 0x00000004;

        //identifiers for schannel, defined in Schannel.h please see 
        //http://msdn.microsoft.com/en-us/library/aa379810(VS.85).aspx
        //Private Communications Technology 1.0 server side. (Obsolete.)
        internal const uint SP_PROT_PCT1_SERVER = 0x00000001;
        //Private Communications Technology 1.0 client side. (Obsolete.)
        internal const uint SP_PROT_PCT1_CLIENT = 0x00000002;
        //Secure Sockets Layer 2.0 server side. Superseded by SP_PROT_TLS1_SERVER.
        internal const uint SP_PROT_SSL2_SERVER = 0x00000004;
        //Secure Sockets Layer 2.0 client side. Superseded by SP_PROT_TLS1_CLIENT.
        internal const uint SP_PROT_SSL2_CLIENT = 0x00000008;
        //Secure Sockets Layer 3.0 server side.
        internal const uint SP_PROT_SSL3_SERVER = 0x00000010;
        //Secure Sockets Layer 3.0 client side.
        internal const uint SP_PROT_SSL3_CLIENT = 0x00000020;
        //Transport Layer Security 1.0 server side.
        internal const uint SP_PROT_TLS1_SERVER = 0x00000040;
        //Transport Layer Security 1.0 client side.
        internal const uint SP_PROT_TLS1_CLIENT = 0x00000080;
        internal const uint SP_PROT_UNI_SERVER = 0x40000000;
        internal const uint SP_PROT_UNI_CLIENT = 0x80000000;
        internal const uint SP_PROT_CLIENTS = SP_PROT_PCT1_CLIENT
            | SP_PROT_SSL2_CLIENT
            | SP_PROT_SSL3_CLIENT
            | SP_PROT_UNI_CLIENT
            | SP_PROT_TLS1_CLIENT;
        internal const uint SP_PROT_SERVERS = SP_PROT_PCT1_SERVER
            | SP_PROT_SSL2_SERVER
            | SP_PROT_SSL3_SERVER
            | SP_PROT_UNI_SERVER
            | SP_PROT_TLS1_SERVER;
        //DTLS 1.0 server side
        internal const uint SP_PROT_DTLS_SERVER = 0x00010000;
        //DTLS 1.0 client side
        internal const uint SP_PROT_DTLS_CLIENT = 0x00020000;

        //dwFlag values of Schannel_Cred.
        internal const uint SCH_CRED_MANUAL_CRED_VALIDATION = 0x00000008;
        internal const uint SCH_CRED_NO_DEFAULT_CREDS = 0x00000010;
        //Identity of Unicode in SEC_WINNT_AUTH_IDENTITY.
        internal const int SEC_WINNT_AUTH_IDENTITY_UNICODE = 0x2;

        /// <summary>
        /// The AcquireCredentialsHandle (General) function acquires a handle to preexisting credentials of a security 
        /// principal. This handle is required by the InitializeSecurityContext (General) and AcceptSecurityContext 
        /// (General) functions. 
        /// Reference: http://msdn.microsoft.com/en-us/library/aa374712(VS.85).aspx
        /// </summary>
        /// <param name="pszPrincipal">A pointer to a null-terminated string that specifies the name of the principal
        /// whose credentials the handle will reference.</param>
        /// <param name="pszPacket">A pointer to a null-terminated string that specifies the name of the security 
        /// package with which these credentials will be used. </param>
        /// <param name="fCredentialUse">A flag that indicates how these credentials will be used. </param>
        /// <param name="PAuthenticationID">A pointer to a locally unique identifier (LUID) that identifies the user.
        /// This parameter is provided for file-system processes such as network redirectors. This parameter can be 
        /// NULL.</param>
        /// <param name="pAuthData">A pointer to package-specific data. This parameter can be NULL, which indicates 
        /// that the default credentials for that package must be used. </param>
        /// <param name="pGetKeyFn">This parameter is not used and should be set to NULL.</param>
        /// <param name="pvGetKeyArgument">This parameter is not used and should be set to NULL.</param>
        /// <param name="phCredential">A pointer to a CredHandle structure to receive the credential handle.</param>
        /// <param name="ptsExpiry">A pointer to a TimeStamp structure that receives the time at which the returned 
        /// credentials expire.</param>
        /// <returns>If the function succeeds, the function returns SEC_E_OK. 
        /// If fails, return non-zero value.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"),
        DllImport("secur32", CharSet = CharSet.Unicode)]
        internal static extern uint AcquireCredentialsHandle(
            string pszPrincipal,
            string pszPacket,
            uint fCredentialUse,
            IntPtr PAuthenticationID,
            IntPtr pAuthData,
            IntPtr pGetKeyFn,
            IntPtr pvGetKeyArgument,
            out SecurityHandle phCredential,
            out SecurityInteger ptsExpiry);

        /// <summary>
        /// The InitializeSecurityContext (General) function initiates the client side, outbound security context from a 
        /// credential handle.
        /// Reference: http://msdn.microsoft.com/en-us/library/aa375506(VS.85).aspx
        /// </summary>
        /// <param name="phCredential">A handle to the credentials returned by AcquireCredentialsHandle (General). This 
        /// handle is used to build the security context.</param>
        /// <param name="phContext">A pointer to a CtxtHandle structure.</param>
        /// <param name="pszTargetName">A pointer to a null-terminated string that indicates the target of the context. 
        /// </param>
        /// <param name="fContextReq">Bit flags that indicate requests for the context. </param>
        /// <param name="Reserved1">This parameter is reserved and must be set to zero.</param>
        /// <param name="TargetDataRep">The data representation, such as byte ordering, on the target. This parameter 
        /// can be either SECURITY_NATIVE_DREP or SECURITY_NETWORK_DREP.</param>
        /// <param name="serverTokenDesc">A pointer to a SecBufferDesc structure that contains pointers to the buffers supplied 
        /// as input to the package. The pointer must be NULL on the first call to the function.</param>
        /// <param name="Reserved2">This parameter is reserved and must be set to zero.</param>
        /// <param name="phNewContext">A pointer to a CtxtHandle structure. </param>
        /// <param name="pOutput">A pointer to a SecBufferDesc structure that contains pointers to the SecBuffer
        /// structure that receives the output data.</param>
        /// <param name="pfContextAttr">A pointer to a variable to receive a set of bit flags that indicate the
        /// attributes of the established context.</param>
        /// <param name="ptsExpiry">A pointer to a TimeStamp structure that receives the expiration time of the 
        /// context. </param>
        /// <returns>A status code indicating various success/failure situations.
        /// Refer to: http://msdn.microsoft.com/en-us/library/aa375506(VS.85).aspx</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"),
        DllImport("secur32", CharSet = CharSet.Unicode)]
        internal static extern uint InitializeSecurityContext(
            ref SecurityHandle phCredential,
            IntPtr phContext,
            string pszTargetName,
            int fContextReq,
            int Reserved1,
            int TargetDataRep,
            IntPtr serverTokenDesc,
            int Reserved2,
            out SecurityHandle phNewContext,
            out SecurityBufferDesc pOutput,
            out uint pfContextAttr,
            out SecurityInteger ptsExpiry);

        /// <summary>
        /// The InitializeSecurityContext (General) function initiates the client side, outbound security context from a 
        /// credential handle.
        /// Reference: http://msdn.microsoft.com/en-us/library/aa375506(VS.85).aspx
        /// </summary>
        /// <param name="phCredential">A handle to the credentials returned by AcquireCredentialsHandle (General). This 
        /// handle is used to build the security context.</param>
        /// <param name="phContext">A pointer to a CtxtHandle structure.</param>
        /// <param name="pszTargetName">A pointer to a null-terminated string that indicates the target of the context. 
        /// </param>
        /// <param name="fContextReq">Bit flags that indicate requests for the context. </param>
        /// <param name="Reserved1">This parameter is reserved and must be set to zero.</param>
        /// <param name="TargetDataRep">The data representation, such as byte ordering, on the target. This parameter 
        /// can be either SECURITY_NATIVE_DREP or SECURITY_NETWORK_DREP.</param>
        /// <param name="serverTokenDesc">A pointer to a SecBufferDesc structure that contains pointers to the buffers supplied 
        /// as input to the package. The pointer must be NULL on the first call to the function.</param>
        /// <param name="Reserved2">This parameter is reserved and must be set to zero.</param>
        /// <param name="phNewContext">A pointer to a CtxtHandle structure. </param>
        /// <param name="pOutput">A pointer to a SecBufferDesc structure that contains pointers to the SecBuffer
        /// structure that receives the output data.</param>
        /// <param name="pfContextAttr">A pointer to a variable to receive a set of bit flags that indicate the
        /// attributes of the established context.</param>
        /// <param name="ptsExpiry">A pointer to a TimeStamp structure that receives the expiration time of the 
        /// context. </param>
        /// <returns>A status code indicating various success/failure situations.
        /// Refer to: http://msdn.microsoft.com/en-us/library/aa375506(VS.85).aspx</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"),
        DllImport("secur32", CharSet = CharSet.Unicode)]
        internal static extern uint InitializeSecurityContext(
            ref SecurityHandle phCredential,
            ref SecurityHandle phContext,
            string pszTargetName,
            int fContextReq,
            int Reserved1,
            int TargetDataRep,
            ref SecurityBufferDesc serverTokenDesc,
            int Reserved2,
            out SecurityHandle phNewContext,
            out SecurityBufferDesc pOutput,
            out uint pfContextAttr,
            out SecurityInteger ptsExpiry);

        /// <summary>
        /// The InitializeSecurityContext (General) function initiates the client side, outbound security context from a 
        /// credential handle.
        /// Reference: http://msdn.microsoft.com/en-us/library/aa375506(VS.85).aspx
        /// </summary>
        /// <param name="phCredential">A handle to the credentials returned by AcquireCredentialsHandle (General). This 
        /// handle is used to build the security context.</param>
        /// <param name="phContext">A pointer to a CtxtHandle structure.</param>
        /// <param name="pszTargetName">A pointer to a null-terminated string that indicates the target of the context. 
        /// </param>
        /// <param name="fContextReq">Bit flags that indicate requests for the context. </param>
        /// <param name="Reserved1">This parameter is reserved and must be set to zero.</param>
        /// <param name="TargetDataRep">The data representation, such as byte ordering, on the target. This parameter 
        /// can be either SECURITY_NATIVE_DREP or SECURITY_NETWORK_DREP.</param>
        /// <param name="serverTokenDesc">A pointer to a SecBufferDesc structure that contains pointers to the buffers supplied 
        /// as input to the package. The pointer must be NULL on the first call to the function.</param>
        /// <param name="Reserved2">This parameter is reserved and must be set to zero
        /// .</param>
        /// <param name="phNewContext">A pointer to a CtxtHandle structure. </param>
        /// <param name="pOutput">A pointer to a SecBufferDesc structure that contains pointers to the SecBuffer
        /// structure that receives the output data.</param>
        /// <param name="pfContextAttr">A pointer to a variable to receive a set of bit flags that indicate the
        /// attributes of the established context.</param>
        /// <param name="ptsExpiry">A pointer to a TimeStamp structure that receives the expiration time of the 
        /// context. </param>
        /// <returns>A status code indicating various success/failure situations.
        /// Refer to: http://msdn.microsoft.com/en-us/library/aa375506(VS.85).aspx</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"),
        DllImport("secur32", CharSet = CharSet.Unicode)]
        internal static extern uint InitializeSecurityContext(
            ref SecurityHandle phCredential,
            IntPtr phContext,
            string pszTargetName,
            int fContextReq,
            int Reserved1,
            int TargetDataRep,
            ref SecurityBufferDesc serverTokenDesc,
            int Reserved2,
            out SecurityHandle phNewContext,
            out SecurityBufferDesc pOutput,
            out uint pfContextAttr,
            out SecurityInteger ptsExpiry);

        /// <summary>
        /// The AcceptSecurityContext (General) function enables the server component of a transport application
        /// to establish a security context between the server and a remote client. 
        /// Reference: http://msdn.microsoft.com/en-us/library/aa374703(VS.85).aspx
        /// </summary>
        /// <param name="phCredential">A handle to the credentials of the server.</param>
        /// <param name="phContext">A pointer to a CtxtHandle structure. </param>
        /// <param name="pInput">A pointer to a SecBufferDesc structure generated by a client call to 
        /// InitializeSecurityContext (General) that contains the input buffer descriptor.</param>
        /// <param name="fContextReq">Bit flags that specify the attributes required by the server to establish 
        /// the context. </param>
        /// <param name="TargetDataRep">The data representation, such as byte ordering, on the target. This parameter
        /// can be either SECURITY_NATIVE_DREP or SECURITY_NETWORK_DREP.</param>
        /// <param name="phNewContext">A pointer to a CtxtHandle structure.</param>
        /// <param name="pOutput">A pointer to a SecBufferDesc structure that contains the output buffer descriptor. 
        /// </param>
        /// <param name="pfContextAttr">A pointer to a variable that receives a set of bit flags that indicate the 
        /// attributes of the established context.</param>
        /// <param name="ptsExpiry">A pointer to a TimeStamp structure that receives the expiration time of the 
        /// context. </param>
        /// <returns>A status code indicating various success/failure situations.
        /// Refer to: http://msdn.microsoft.com/en-us/library/aa374703(VS.85).aspx </returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"),
        DllImport("secur32", CharSet = CharSet.Unicode)]
        internal static extern uint AcceptSecurityContext(
            ref SecurityHandle phCredential,
            IntPtr phContext,
            ref SecurityBufferDesc pInput,
            uint fContextReq,
            uint TargetDataRep,
            ref SecurityHandle phNewContext,
            out SecurityBufferDesc pOutput,
            out uint pfContextAttr,
            out SecurityInteger ptsExpiry);

        /// <summary>
        /// The AcceptSecurityContext (General) function enables the server component of a transport application
        /// to establish a security context between the server and a remote client. 
        /// Reference: http://msdn.microsoft.com/en-us/library/aa374703(VS.85).aspx
        /// </summary>
        /// <param name="phCredential">A handle to the credentials of the server.</param>
        /// <param name="phContext">A pointer to a CtxtHandle structure. </param>
        /// <param name="pInput">A pointer to a SecBufferDesc structure generated by a client call to 
        /// InitializeSecurityContext (General) that contains the input buffer descriptor.</param>
        /// <param name="fContextReq">Bit flags that specify the attributes required by the server to establish 
        /// the context. </param>
        /// <param name="TargetDataRep">The data representation, such as byte ordering, on the target. This parameter
        /// can be either SECURITY_NATIVE_DREP or SECURITY_NETWORK_DREP.</param>
        /// <param name="phNewContext">A pointer to a CtxtHandle structure.</param>
        /// <param name="pOutput">A pointer to a SecBufferDesc structure that contains the output buffer descriptor. 
        /// </param>
        /// <param name="pfContextAttr">A pointer to a variable that receives a set of bit flags that indicate the 
        /// attributes of the established context.</param>
        /// <param name="ptsExpiry">A pointer to a TimeStamp structure that receives the expiration time of the 
        /// context. </param>
        /// <returns>A status code indicating various success/failure situations.
        /// Refer to: http://msdn.microsoft.com/en-us/library/aa374703(VS.85).aspx</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"),
        DllImport("secur32", CharSet = CharSet.Unicode)]
        internal static extern uint AcceptSecurityContext(
            ref SecurityHandle phCredential,
            ref SecurityHandle phContext,
            ref SecurityBufferDesc pInput,
            uint fContextReq,
            uint TargetDataRep,
            ref SecurityHandle phNewContext,
            out SecurityBufferDesc pOutput,
            out uint pfContextAttr,
            out SecurityInteger ptsExpiry);

        /// <summary>
        /// The CompleteAuthToken function completes an authentication token. 
        /// Reference: http://msdn.microsoft.com/en-us/library/aa374764(VS.85).aspx
        /// </summary>
        /// <param name="phContext">A handle of the context that needs to be completed.</param>
        /// <param name="SecBufferDesc">A pointer to a SecBufferDesc structure that contains the buffer descriptor for
        /// the entire message.</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"),
        DllImport("secur32", CharSet = CharSet.Unicode)]
        internal static extern uint CompleteAuthToken(
            ref SecurityHandle phContext,
            ref SecurityBufferDesc SecBufferDesc);

        /// <summary>
        /// The EncryptMessage (General) function encrypts a message to provide privacy. 
        /// Reference: http://msdn.microsoft.com/en-us/library/aa375378(VS.85).aspx
        /// </summary>
        /// <param name="phContext">A handle to the security context to be used to encrypt the message.</param>
        /// <param name="fQOP">Package-specific flags that indicate the quality of protection.</param>
        /// <param name="pMessage">A pointer to a SecBufferDesc structure.</param>
        /// <param name="MessageSeqNo">The sequence number that the transport application assigned to the message. 
        /// </param>
        /// <returns>If the function succeeds, the function returns SEC_E_OK.
        /// If fails, return non-zero value.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"),
        DllImport("secur32", CharSet = CharSet.Unicode)]
        internal static extern uint EncryptMessage(
            ref SecurityHandle phContext,
            uint fQOP,
            ref SecurityBufferDesc pMessage,
            uint MessageSeqNo);

        /// <summary>
        /// The DecryptMessage (General) function decrypts a message. 
        /// Reference: http://msdn.microsoft.com/en-us/library/aa375211(VS.85).aspx
        /// </summary>
        /// <param name="phContext">A handle to the security context to be used to decrypt the message.</param>
        /// <param name="pMessage">A pointer to a SecBufferDesc structure. </param>
        /// <param name="MessageSeqNo">The sequence number expected by the transport application, if any. If the 
        /// transport application does not maintain sequence numbers, this parameter must be set to zero.</param>
        /// <param name="pfQOP">A pointer to a variable of type ULONG that receives package-specific flags that 
        /// indicate the quality of protection.
        /// When using the Schannel SSP, this parameter is not used and should be set to NULL.</param>
        /// <returns>If the function verifies that the message was received in the correct sequence, the function 
        /// returns SEC_E_OK.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"),
        DllImport("secur32", CharSet = CharSet.Unicode)]
        internal static extern uint DecryptMessage(
            ref SecurityHandle phContext,
            ref SecurityBufferDesc pMessage,
            uint MessageSeqNo,
            out uint pfQOP);

        /// <summary>
        /// The MakeSignature function generates a cryptographic checksum of the message, and also includes sequencing
        /// information to prevent message loss or insertion. 
        /// Reference: http://msdn.microsoft.com/en-us/library/aa378736(VS.85).aspx
        /// </summary>
        /// <param name="phContext">A handle to the security context to use to sign the message.</param>
        /// <param name="fQOP">Package-specific flags that indicate the quality of protection. A security package can 
        /// use this parameter to enable the selection of cryptographic algorithms.</param>
        /// <param name="pMessage">A pointer to a SecBufferDesc structure.</param>
        /// <param name="MessageSeqNo">The sequence number that the transport application assigned to the message. If 
        /// the transport application does not maintain sequence numbers, this parameter is zero.</param>
        /// <returns>If the function succeeds, the function returns SEC_E_OK.
        /// If fails, return non-zero value.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"),
        DllImport("secur32", CharSet = CharSet.Unicode)]
        internal static extern uint MakeSignature(
            ref SecurityHandle phContext,
            uint fQOP,
            ref SecurityBufferDesc pMessage,
            uint MessageSeqNo);

        /// <summary>
        /// Verifies the signature of a message received that was signed by the sender by using the MakeSignature 
        /// function.
        /// Reference: http://msdn.microsoft.com/en-us/library/aa380540(VS.85).aspx
        /// </summary>
        /// <param name="phContext">A handle to the security context to use for the message.</param>
        /// <param name="pMessage">Pointer to a SecBufferDesc structure that references a set of SecBuffer structures
        /// that contain the message and signature to verify. The signature is in a SecBuffer structure of type 
        /// SECBUFFER_TOKEN.</param>
        /// <param name="MessageSeqNo">Specifies the sequence number expected by the transport application, if any. 
        /// If the transport application does not maintain sequence numbers, this parameter is zero.</param>
        /// <param name="pfQOP">Pointer to a ULONG variable that receives package-specific flags that indicate the 
        /// quality of protection.</param>
        /// <returns>If the function verifies that the message was received in the correct sequence and has not 
        /// been modified, the return value is SEC_E_OK.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"),
        DllImport("secur32", CharSet = CharSet.Unicode)]
        internal static extern uint VerifySignature(
            ref SecurityHandle phContext,
            ref SecurityBufferDesc pMessage,
            uint MessageSeqNo,
            out uint pfQOP);

        /// <summary>
        /// Enables a transport application to query a security package for certain attributes of a security context.
        /// Reference: http://msdn.microsoft.com/en-us/library/aa379326(VS.85).aspx
        /// </summary>
        /// <param name="phContext">A handle to the security context to be queried.</param>
        /// <param name="ulAttribute">Specifies the attribute of the context to be returned.</param>
        /// <param name="pBuffer">A pointer to a structure that receives the attributes. The type of structure pointed
        /// to depends on the value specified in the ulAttribute parameter.</param>
        /// <returns>If the function succeeds, the return value is SEC_E_OK.
        /// If fails, return non-zero value.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"),
        DllImport("secur32", CharSet = CharSet.Unicode)]
        internal static extern uint QueryContextAttributes(
              ref SecurityHandle phContext,
              uint ulAttribute,
              IntPtr pBuffer);

        /// <summary>
        /// The FreeCredentialsHandle function notifies the security system that the credentials are no longer needed.
        /// Reference: http://msdn.microsoft.com/en-us/library/aa375417(VS.85).aspx
        /// </summary>
        /// <param name="phCredential">A pointer to the credential handle obtained by using the 
        /// AcquireCredentialsHandle (General) function.</param>
        /// <returns>If the function succeeds, the function returns SEC_E_OK.
        /// If fails, return non-zero value.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"),
        DllImport("secur32", CharSet = CharSet.Unicode)]
        internal static extern uint FreeCredentialsHandle(ref SecurityHandle phCredential);

        /// <summary>
        /// The DeleteSecurityContext function deletes the local data structures associated with the specified security
        /// context.
        /// Reference: http://msdn.microsoft.com/en-us/library/aa375354(VS.85).aspx
        /// </summary>
        /// <param name="phContext">Handle of the security context to delete.</param>
        /// <returns>If the function succeeds, the return value is SEC_E_OK.
        /// If fails, return non-zero value.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode"),
        DllImport("secur32", CharSet = CharSet.Unicode)]
        internal static extern uint DeleteSecurityContext(ref SecurityHandle phContext);

        /// <summary>
        /// The FreeContextBuffer function enables callers of security package functions to free a memory buffer that
        /// was allocated by the security package as a result of calls to InitializeSecurityContext (General) and
        /// AcceptSecurityContext (General).
        /// http://msdn.microsoft.com/en-us/library/aa375416(v=VS.85).aspx
        /// </summary>
        /// <param name="pContextBuffer">A pointer to memory allocated by the security package.</param>
        /// <returns>If the function succeeds, the function returns SEC_E_OK.
        /// If the function fails, it returns a nonzero error code.</returns>
        [DllImport("secur32", CharSet = CharSet.Unicode)]
        internal static extern void FreeContextBuffer(IntPtr pContextBuffer);

        /// <summary>
        /// Retrieves and decrypts a string.
        /// http://msdn.microsoft.com/en-us/library/ms721811%28v=VS.85%29.aspx
        /// </summary>
        /// <param name="policyHandle">
        /// A handle to a Policy object. The handle must have the POLICY_GET_PRIVATE_INFORMATION access right.
        /// For more information, see Opening a Policy Object Handle.
        /// </param>
        /// <param name="keyName">
        /// Pointer to an LSA_UNICODE_STRING structure that contains the name of the key
        /// under which the private data is stored.
        /// </param>
        /// <param name="privateData">
        /// Pointer to a variable that receives a pointer to an LSA_UNICODE_STRING structure
        /// that contains the private data.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns STATUS_SUCCESS.<para/>
        /// e function fails, it returns an NTSTATUS value, which can be the following value or one of
        /// the LSA Policy Function Return Values.
        /// </returns>
        [DllImport("advapi32", CharSet = CharSet.Unicode)]
        internal static extern NtStatus LsaRetrievePrivateData(
            IntPtr policyHandle, ref LsaUnicodeString keyName, out IntPtr privateData);

        /// <summary>
        /// The LsaOpenPolicy function opens a handle to the Policy object on a local or remote system.
        /// http://msdn.microsoft.com/en-us/library/aa378299%28v=VS.85%29.aspx
        /// </summary>
        /// <param name="systemName">
        /// A pointer to an LSA_UNICODE_STRING structure that contains the name of the target system.<para/>
        /// The name can have the form "ComputerName" or "\\ComputerName".<para/>
        /// If this parameter is NULL, the function opens the Policy object on the local system.
        /// </param>
        /// <param name="objectAttributes">
        /// A pointer to an LSA_OBJECT_ATTRIBUTES structure that specifies the connection attributes.<para/>
        /// The structure members are not used; initialize them to NULL or zero.
        /// </param>
        /// <param name="desiredAccess">
        /// An ACCESS_MASK that specifies the requested access rights.<para/>
        /// The function fails if the DACL of the target system does not allow the caller the requested access.<para/>
        /// To determine the access rights that you need, see the documentation for
        /// the LSA functions with which you want to use the policy handle.
        /// </param>
        /// <param name="policyHandle">
        /// A pointer to an LSA_HANDLE variable that receives a handle to the Policy object.<para/>
        /// When you no longer need this handle, pass it to the LsaClose function to close it.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns STATUS_SUCCESS.<para/>
        /// If the function fails, it returns an NTSTATUS code. For more information,
        /// see LSA Policy Function Return Values.
        /// </returns>
        [DllImport("advapi32", CharSet = CharSet.Unicode)]
        internal static extern NtStatus LsaOpenPolicy(
            IntPtr systemName, ref LsaObjectAttributes objectAttributes,
            AccessMask desiredAccess, out IntPtr policyHandle);

        /// <summary>
        /// The LsaClose function closes a handle to a Policy or TrustedDomain object.
        /// http://msdn.microsoft.com/en-us/library/ms721787%28v=VS.85%29.aspx
        /// </summary>
        /// <param name="objectHandle">
        /// A handle to a Policy object returned by the LsaOpenPolicy function or to a TrustedDomain object returned
        /// by the LsaOpenTrustedDomainByName function.
        /// Following the completion of this call, the handle is no longer valid.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns STATUS_SUCCESS.<para/>
        /// If the function fails, it returns an NTSTATUS code. For more information,
        /// see LSA Policy Function Return Values.
        /// </returns>
        [DllImport("advapi32", CharSet = CharSet.Unicode)]
        internal static extern NtStatus LsaClose(IntPtr objectHandle);

        /// <summary>
        /// The LsaFreeMemory function frees memory allocated for an output buffer by an LSA function call.
        /// LSA functions that return variable-length output buffers always allocate the buffer on behalf of the caller.
        /// The caller must free this memory by passing the returned buffer pointer to LsaFreeMemory when the
        /// memory is no longer required.<para/>
        /// http://msdn.microsoft.com/en-us/library/ms721796%28v=VS.85%29.aspx
        /// </summary>
        /// <param name="buffer">
        /// Pointer to memory buffer that was allocated by an LSA function call. If LsaFreeMemory is successful,
        /// this buffer is freed.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns STATUS_SUCCESS.<para/>
        /// If the function fails, it returns an NTSTATUS code. For more information,
        /// see LSA Policy Function Return Values.
        /// </returns>
        [DllImport("advapi32", CharSet = CharSet.Unicode)]
        internal static extern NtStatus LsaFreeMemory(IntPtr buffer);
    }
}
