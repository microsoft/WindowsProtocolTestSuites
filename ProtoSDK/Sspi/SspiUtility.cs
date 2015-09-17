// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Globalization;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// Utility class of SSPI.
    /// </summary>
    public static class SspiUtility
    {
        //A pointer to a null-terminated string that specifies the name of the security package with which these
        //credentials will be used.
        private const string Negotiate = "Negotiate";
        private const string Kerberos = "Kerberos";
        private const string Ntlm = "NTLM";
        private const string Schannel = "Microsoft Unified Security Protocol Provider";
        private const string CredSsp = "CredSsp";

        //Security context attributes defined by SSPI.
        //reference: http://msdn.microsoft.com/en-us/library/aa379326(VS.85).aspx
        private const string SECPKG_ATTR_SIZES = "SECPKG_ATTR_SIZES";
        private const string SECPKG_ATTR_NAMES = "SECPKG_ATTR_NAMES";
        private const string SECPKG_ATTR_LIFESPAN = "SECPKG_ATTR_LIFESPAN";
        private const string SECPKG_ATTR_DCE_INFO = "SECPKG_ATTR_DCE_INFO";
        private const string SECPKG_ATTR_STREAM_SIZES = "SECPKG_ATTR_STREAM_SIZES";
        private const string SECPKG_ATTR_KEY_INFO = "SECPKG_ATTR_KEY_INFO";
        private const string SECPKG_ATTR_AUTHORITY = "SECPKG_ATTR_AUTHORITY";
        private const string SECPKG_ATTR_PROTO_INFO = "SECPKG_ATTR_PROTO_INFO";
        private const string SECPKG_ATTR_PASSWORD_EXPIRY = "SECPKG_ATTR_PASSWORD_EXPIRY";
        private const string SECPKG_ATTR_SESSION_KEY = "SECPKG_ATTR_SESSION_KEY";
        private const string SECPKG_ATTR_PACKAGE_INFO = "SECPKG_ATTR_PACKAGE_INFO";
        private const string SECPKG_ATTR_USER_FLAGS = "SECPKG_ATTR_USER_FLAGS";
        private const string SECPKG_ATTR_NEGOTIATION_INFO = "SECPKG_ATTR_NEGOTIATION_INFO";
        private const string SECPKG_ATTR_NATIVE_NAMES = "SECPKG_ATTR_NATIVE_NAMES";
        private const string SECPKG_ATTR_FLAGS = "SECPKG_ATTR_FLAGS";
        // These attributes exist only in Win XP and greater
        private const string SECPKG_ATTR_USE_VALIDATED = "SECPKG_ATTR_USE_VALIDATED";
        private const string SECPKG_ATTR_CREDENTIAL_NAME = "SECPKG_ATTR_CREDENTIAL_NAME";
        private const string SECPKG_ATTR_TARGET_INFORMATION = "SECPKG_ATTR_TARGET_INFORMATION";
        private const string SECPKG_ATTR_ACCESS_TOKEN = "SECPKG_ATTR_ACCESS_TOKEN";
        // These attributes exist only in Win2K3 and greater
        private const string SECPKG_ATTR_TARGET = "SECPKG_ATTR_TARGET";
        private const string SECPKG_ATTR_AUTHENTICATION_ID = "SECPKG_ATTR_AUTHENTICATION_ID";
        // These attributes exist only in Win2K3SP1 and greater
        private const string SECPKG_ATTR_LOGOFF_TIME = "SECPKG_ATTR_LOGOFF_TIME";
        //Defined in schannel.h
        private const string SECPKG_ATTR_REMOTE_CERT_CONTEXT = "SECPKG_ATTR_REMOTE_CERT_CONTEXT";
        private const string SECPKG_ATTR_LOCAL_CERT_CONTEXT = "SECPKG_ATTR_LOCAL_CERT_CONTEXT";
        private const string SECPKG_ATTR_ROOT_STORE = "SECPKG_ATTR_ROOT_STORE";
        private const string SECPKG_ATTR_ISSUER_LIST_EX = "SECPKG_ATTR_ISSUER_LIST_EX";
        private const string SECPKG_ATTR_CONNECTION_INFO = "SECPKG_ATTR_CONNECTION_INFO";
        private const string SECPKG_ATTR_EAP_KEY_BLOCK = "SECPKG_ATTR_EAP_KEY_BLOCK";
        private const string SECPKG_ATTR_APP_DATA = "SECPKG_ATTR_APP_DATA";

        //Schannel key static size defined in SecPkgContext_EapKeyBlock Structure.
        //http://msdn.microsoft.com/en-us/library/aa379822(v=VS.85).aspx
        private const int SchannelKeySize = 128;
        private const int SchannelIvSize = 64;

        private static byte[] ConcatenateSecurityBuffers(
            SecurityBuffer[] securityBuffers, 
            SecurityBufferType[] targetTypes,
            bool bothReadOnlyAndReadWrite)
        {
            byte[] buf = new byte[0];
            for (int i = 0; i < securityBuffers.Length; i++)
            {
                SecurityBufferType securityBufferType = (securityBuffers[i].BufferType & ~SecurityBufferType.AttrMask);
                bool typeMatch = false;
                for (int j = 0; j < targetTypes.Length; j++)
                {
                    if (securityBufferType == targetTypes[j])
                    {
                        typeMatch = true;
                        break;
                    }
                }
                if (typeMatch)
                {
                    bool skip = !bothReadOnlyAndReadWrite
                        && (((securityBuffers[i].BufferType & SecurityBufferType.ReadOnly) != 0)
                        || ((securityBuffers[i].BufferType & SecurityBufferType.ReadOnlyWithChecksum) != 0));

                    if (!skip && securityBuffers[i].Buffer != null)
                    {
                        buf = ArrayUtility.ConcatenateArrays(buf, securityBuffers[i].Buffer);
                    }
                }
            }
            return buf;
        }

        /// <summary>
        /// Concatenate all buffers of a specified type in the list.
        /// </summary>
        /// <param name="securityBuffers">Input security buffers.</param>
        /// <param name="targetTypes">Specified types.</param>
        /// <returns>A single byte array with all buffers concatenated.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers is null.
        /// </exception>
        public static byte[] ConcatenateSecurityBuffers(SecurityBuffer[] securityBuffers, params SecurityBufferType[] targetTypes)
        {
            if (securityBuffers == null || securityBuffers.Length == 0)
            {
                throw new ArgumentNullException("securityBuffers");
            }
            for (int i = 0; i < securityBuffers.Length; i++)
            {
                if (securityBuffers[i] == null)
                {
                    throw new ArgumentNullException("securityBuffers");
                }
            }

            return ConcatenateSecurityBuffers(securityBuffers, targetTypes, true);
        }


        /// <summary>
        /// Concatenate all read-write (READONLY flag is not set) buffers of a specified type in the list.
        /// </summary>
        /// <param name="securityBuffers">Input security buffers.</param>
        /// <param name="targetTypes">Specified types.</param>
        /// <returns>A single byte array with all buffers concatenated.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers is null.
        /// </exception>
        public static byte[] ConcatenateReadWriteSecurityBuffers(
            SecurityBuffer[] securityBuffers, 
            params SecurityBufferType[] targetTypes)
        {
            if (securityBuffers == null || securityBuffers.Length == 0)
            {
                throw new ArgumentNullException("securityBuffers");
            }
            for (int i = 0; i < securityBuffers.Length; i++)
            {
                if (securityBuffers[i] == null)
                {
                    throw new ArgumentNullException("securityBuffers");
                }
            }

            return ConcatenateSecurityBuffers(securityBuffers, targetTypes, false);
        }


        /// <summary>
        /// Update buffers of a specified type in the list. 
        /// Buffer will be separated automatically to fit the original length of a security buffer. 
        /// If Buffer field of an input security buffer is null, it means the length is unlimited 
        /// (that is all remaining data will be copied into it). 
        /// Only read-write (READONLY flag is not set) security buffer will be updated.
        /// </summary>
        /// <param name="securityBuffers">Input security buffers.</param>
        /// <param name="targetType">A specified type.</param>
        /// <param name="buffer">The buffer to be updated into security buffers.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers or buffer is null.
        /// </exception>
        /// <exception cref="SspiException">
        /// Total length of security buffers is not enough.
        /// </exception>
        public static void UpdateSecurityBuffers(SecurityBuffer[] securityBuffers, SecurityBufferType targetType, byte[] buffer)
        {
            UpdateSecurityBuffers(securityBuffers, new SecurityBufferType[] { targetType }, buffer);
        }

        
        /// <summary>
        /// Update buffers of a specified type in the list. 
        /// Buffer will be separated automatically to fit the original length of a security buffer. 
        /// If Buffer field of an input security buffer is null, it means the length is unlimited 
        /// (that is all remaining data will be copied into it). 
        /// Only read-write (READONLY flag is not set) security buffer will be updated.
        /// </summary>
        /// <param name="securityBuffers">Input security buffers.</param>
        /// <param name="targetTypes">Specified types.</param>
        /// <param name="buffer">The buffer to be updated into security buffers.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when securityBuffers or buffer is null.
        /// </exception>
        /// <exception cref="SspiException">
        /// Total length of security buffers is not enough.
        /// </exception>
        public static void UpdateSecurityBuffers(SecurityBuffer[] securityBuffers, SecurityBufferType[] targetTypes, byte[] buffer)
        {
            if (securityBuffers == null || securityBuffers.Length == 0)
            {
                throw new ArgumentNullException("securityBuffers");
            }
            for (int i = 0; i < securityBuffers.Length; i++)
            {
                if (securityBuffers[i] == null)
                {
                    throw new ArgumentNullException("securityBuffers");
                }
            }
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            int offset = 0;
            for (int i = 0; i < securityBuffers.Length; i++)
            {
                SecurityBufferType securityBufferType = (securityBuffers[i].BufferType & ~SecurityBufferType.AttrMask);
                bool isReadOnly = ((securityBuffers[i].BufferType & SecurityBufferType.ReadOnly) != 0)
                               || ((securityBuffers[i].BufferType & SecurityBufferType.ReadOnlyWithChecksum) != 0);
                bool typeMatch = false;
                for (int j = 0; j < targetTypes.Length; j++)
                {
                    if (securityBufferType == targetTypes[j])
                    {
                        typeMatch = true;
                        break;
                    }
                }
                if (typeMatch && !isReadOnly)
                {
                    int length = buffer.Length - offset;
                    if (securityBuffers[i].Buffer != null && securityBuffers[i].Buffer.Length < length)
                    {
                        length = securityBuffers[i].Buffer.Length;
                    }

                    securityBuffers[i].Buffer = ArrayUtility.SubArray(
                        buffer,
                        offset,
                        length);

                    offset += length;
                }
            }

            if (offset < buffer.Length)
            {
                throw new SspiException("Total length of security buffers is not enough.");
            }
            else if (offset > buffer.Length)
            {
                //Unlikely to happen
                throw new InvalidOperationException("Extra data were written to security buffers.");
            }
        }


        #region Windows SSPI

        /// <summary>
        /// Verify message whether follow the format: MESSAGE_LENGTH(4 bytes)|MESSAGE.
        /// </summary>
        /// <param name="message">Message contains header.</param>
        /// <returns>If true, verify successful, otherwise fail.</returns>
        internal static bool VerifyMessageHeader(byte[] message)
        {
            if (message == null)
            {
                return false;
            }

            //Verify message header
            if (message.Length < sizeof(int))
            {
                return false;
            }
            else
            {
                int messageLength = BitConverter.ToInt32(message, 0);
                //The value of header should be less than or equal length of message body and signature(optional).
                if (messageLength > message.Length - sizeof(int))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Free un-managed resources in SecurityWinntAuthIdentity
        /// </summary>
        /// <param name="authIdentity">Security Winnt Auth Identity.</param>
        [SecurityPermission(SecurityAction.Demand)]
        internal static void FreeSecurityWinntAuthIdentity(SecurityWinntAuthIdentity authIdentity)
        {
            if (authIdentity.User != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(authIdentity.User);
                authIdentity.User = IntPtr.Zero;
            }

            if (authIdentity.Domain != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(authIdentity.Domain);
                authIdentity.Domain = IntPtr.Zero;
            }

            if (authIdentity.Password != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(authIdentity.Password);
                authIdentity.Password = IntPtr.Zero;
            }
        }


        /// <summary>
        /// Free un-managed memory in SchannelCred.
        /// </summary>
        /// <param name="schannelCred">SchannelCred structure</param>
        [SecurityPermission(SecurityAction.Demand)]
        internal static void FreeSchannelCred(SchannelCred schannelCred)
        {
            if (schannelCred.paCred != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(schannelCred.paCred);
                schannelCred.paCred = IntPtr.Zero;
            }
        }


        /// <summary>
        /// Free un-managed memory in CredSspCred
        /// </summary>
        /// <param name="credSsp">CredSspCred structure</param>
        [SecurityPermission(SecurityAction.Demand)]
        internal static void FreeCredSspCred(CredSspCred credSsp)
        {
            if (credSsp.pSchannelCred != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(credSsp.pSchannelCred);
                credSsp.pSchannelCred = IntPtr.Zero;
            }
            if (credSsp.pSpnegoCred != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(credSsp.pSpnegoCred);
                credSsp.pSpnegoCred = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Convert SecurityPackage type to package string used by SSPI.
        /// </summary>
        /// <param name="packageType">Package type</param>
        /// <returns>Package string used by SSPI</returns>
        /// <exception cref="ArgumentException">If packageType is not supported, throw this exception.</exception>
        internal static string GetPackageStringName(SecurityPackageType packageType)
        {
            string package = string.Empty;
            switch (packageType)
            {
                case SecurityPackageType.CredSsp:
                    package = CredSsp;
                    break;
                case SecurityPackageType.Kerberos:
                    package = Kerberos;
                    break;
                case SecurityPackageType.Negotiate:
                    package = Negotiate;
                    break;
                case SecurityPackageType.Ntlm:
                    package = Ntlm;
                    break;
                case SecurityPackageType.Schannel:
                    package = Schannel;
                    break;
                default:
                    throw new ArgumentException("Invlid packageType value.", "packageType");
            }

            return package;
        }


        /// <summary>
        /// Create un-managed memory of SecurityWinntAuthIdentity structure
        /// </summary>
        /// <param name="authIndentity">SecurityWinntAuthIdentity structure</param>
        /// <returns>IntPtr of SecurityWinntAuthIdentity</returns>
        [SecurityPermission(SecurityAction.Demand)]
        internal static IntPtr CreateAuthData(SecurityWinntAuthIdentity authIndentity)
        {
            IntPtr pAuthData = Marshal.AllocHGlobal(Marshal.SizeOf(authIndentity));

            Marshal.StructureToPtr(authIndentity, pAuthData, false);
            return pAuthData;
        }


        /// <summary>
        /// Create un-managed memory of SchannelCred structure.
        /// </summary>
        /// <param name="schannelCred">Schannel_Cred structure</param>
        /// <returns>IntPtr of SchannelCred</returns>
        [SecurityPermission(SecurityAction.Demand)]
        internal static IntPtr CreateAuthData(SchannelCred schannelCred)
        {
            IntPtr pAuthData = Marshal.AllocHGlobal(Marshal.SizeOf(schannelCred));

            Marshal.StructureToPtr(schannelCred, pAuthData, false);
            return pAuthData;
        }


        /// <summary>
        /// Create un-managed memory of CredSspCred structure.
        /// </summary>
        /// <param name="credSsp">CredSspCred structure</param>
        /// <returns>IntPtr of CredSspCred</returns>
        [SecurityPermission(SecurityAction.Demand)]
        internal static IntPtr CreateAuthData(CredSspCred credSsp)
        {
            IntPtr pAuthData = Marshal.AllocHGlobal(Marshal.SizeOf(credSsp));

            Marshal.StructureToPtr(credSsp, pAuthData, false);
            return pAuthData;
        }


        /// <summary>
        /// Query Session key by context handle.
        /// </summary>
        /// <param name="packageType">Package type</param>
        /// <param name="contextHandle">context handle</param>
        /// <returns>Session key</returns>
        [SecurityPermission(SecurityAction.Demand)]
        internal static byte[] QuerySessionKey(SecurityPackageType packageType, ref SecurityHandle contextHandle)
        {
            IntPtr pSessionKey = IntPtr.Zero;
            byte[] sessionKey = null;

            if (packageType == SecurityPackageType.Schannel || packageType == SecurityPackageType.CredSsp)
            {
                pSessionKey = Marshal.AllocHGlobal(SchannelKeySize + SchannelIvSize);
                uint hResult = NativeMethods.QueryContextAttributes(
                    ref contextHandle,
                    NativeMethods.SECPKG_ATTR_EAP_KEY_BLOCK,
                    pSessionKey);
                if (hResult == NativeMethods.SEC_E_OK)
                {
                    if (pSessionKey != IntPtr.Zero)
                    {
                        sessionKey = new byte[SchannelKeySize];
                        Marshal.Copy(pSessionKey, sessionKey, 0, SchannelKeySize);
                    }
                }
                Marshal.FreeHGlobal(pSessionKey);
            }
            else
            {
                pSessionKey = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SecurityPackageContextSessionKey)));
                uint hResult = NativeMethods.QueryContextAttributes(
                    ref contextHandle,
                    NativeMethods.SECPKG_ATTR_SESSION_KEY,
                    pSessionKey);
                if (hResult == NativeMethods.SEC_E_OK)
                {
                    if (pSessionKey != IntPtr.Zero)
                    {
                        SecurityPackageContextSessionKey contextSessionKey = (SecurityPackageContextSessionKey)
                            Marshal.PtrToStructure(pSessionKey, typeof(SecurityPackageContextSessionKey));

                        sessionKey = new byte[contextSessionKey.SessionKeyLength];
                        Marshal.Copy(contextSessionKey.SessionKey, sessionKey, 0, sessionKey.Length);
                        NativeMethods.FreeContextBuffer(contextSessionKey.SessionKey);
                    }
                }
                Marshal.FreeHGlobal(pSessionKey);
            }
            
            return sessionKey;
        }


        /// <summary>
        /// Query ContextSizes attribute.
        /// </summary>
        /// <param name="contextHandle">context handle</param>
        /// <returns>Context sizes</returns>
        /// <exception cref="SspiException">If verify fail, throw this exception.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        internal static SecurityPackageContextSizes QueryContextSizes(ref SecurityHandle contextHandle)
        {
            IntPtr pContextSizes = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SecurityPackageContextSizes)));

            uint hResult = NativeMethods.QueryContextAttributes(
                ref contextHandle,
                NativeMethods.SECPKG_ATTR_SIZES,
                pContextSizes);

            if (hResult == NativeMethods.SEC_E_OK)
            {
                SecurityPackageContextSizes contextSizes = (SecurityPackageContextSizes)Marshal.PtrToStructure(
                        pContextSizes,
                        typeof(SecurityPackageContextSizes));
                Marshal.FreeHGlobal(pContextSizes);
                return contextSizes;
            }
            else
            {
                Marshal.FreeHGlobal(pContextSizes);
                throw new SspiException("Query ContextSizes failed.", hResult);
            }
        }


        /// <summary>
        /// Sign message with MakeSignature of SSPI.
        /// </summary>
        /// <param name="contextHandle">Context handle</param>
        /// <param name="sequenceNumber">Sequence number</param>
        /// <param name="securityBuffers">SecurityBufferDesc for MakeSignature.</param>
        /// <exception cref="SspiException">If verify fail, throw this exception.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        internal static void MakeSignature(
            ref SecurityHandle contextHandle,
            uint sequenceNumber,
            params SecurityBuffer[] securityBuffers)
        {
            SecurityBufferDescWrapper securityBufferDescWrapper = new SecurityBufferDescWrapper(securityBuffers);

            uint hResult = NativeMethods.MakeSignature(
                ref contextHandle, 
                0,
                ref securityBufferDescWrapper.securityBufferDesc,
                sequenceNumber);
            if (hResult != NativeMethods.SEC_E_OK)
            {
                securityBufferDescWrapper.FreeSecurityBufferDesc();
                throw new SspiException("Sign failed.", hResult);
            }
            else
            {
                SspiSecurityBuffer[] buffers = securityBufferDescWrapper.securityBufferDesc.GetBuffers();
                if (buffers != null)
                {
                    for (int i = 0; i < buffers.Length; i++)
                    {
                        if (securityBuffers[i].BufferType == SecurityBufferType.Token)
                        {
                            securityBuffers[i].Buffer = new byte[buffers[i].bufferLength];
                            Marshal.Copy(
                                buffers[i].pSecBuffer, 
                                securityBuffers[i].Buffer,
                                0, 
                                (int)buffers[i].bufferLength);
                        }
                    }
                }
                securityBufferDescWrapper.FreeSecurityBufferDesc();
            }
        }


        /// <summary>
        /// Verify signature with VerifySignature of SSPI.
        /// </summary>
        /// <param name="contextHandle">context handle</param>
        /// <param name="securityBuffers">SecurityBufferDesc for VerifySignature.</param>
        /// <returns>If successful, return true.</returns>
        /// <param name="sequenceNumber">Sequence number</param>
        /// <exception cref="SspiException">If verify fail, throw this exception.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        internal static bool VerifySignature(
            ref SecurityHandle contextHandle, 
            uint sequenceNumber, 
            params SecurityBuffer[] securityBuffers)
        {
            SecurityBufferDescWrapper securityBufferDescWrapper = new SecurityBufferDescWrapper(securityBuffers);
            uint pfQop;

            uint hResult = NativeMethods.VerifySignature(
                ref contextHandle,
                ref securityBufferDescWrapper.securityBufferDesc,
                sequenceNumber, 
                out pfQop);
            securityBufferDescWrapper.FreeSecurityBufferDesc();
            if (hResult == NativeMethods.SEC_E_OK)
            {
                return true;
            }
            else
            {
                throw new SspiException("Verify failed.", hResult);
            }
        }


        /// <summary>
        /// Encrypt message with EncryptMessage of SSPI
        /// </summary>
        /// <param name="contextHandle">context handle</param>
        /// <param name="sequenceNumber">sequence number</param>
        /// <param name="qualityOfProtection">The quality of protection flag</param>
        /// <param name="securityBuffers">SecurityBufferDesc for EncryptMessage</param>
        /// <exception cref="SspiException">If verify fail, throw this exception.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        internal static void Encrypt(
            ref SecurityHandle contextHandle,
            uint sequenceNumber,
            SECQOP_WRAP qualityOfProtection,
            params SecurityBuffer[] securityBuffers)
        {
            SecurityBufferDescWrapper encryptBufferDescWrapper = new SecurityBufferDescWrapper(securityBuffers);

            uint hResult = NativeMethods.EncryptMessage(
                ref contextHandle,
                (uint)qualityOfProtection,
                ref encryptBufferDescWrapper.securityBufferDesc,
                sequenceNumber);
            if (hResult == NativeMethods.SEC_E_OK)
            {
                SspiSecurityBuffer[] buffers = encryptBufferDescWrapper.securityBufferDesc.GetBuffers();
                for (int i = 0; i < securityBuffers.Length; i++)
                {
                    securityBuffers[i].Buffer = new byte[buffers[i].bufferLength];
                    if (buffers[i].pSecBuffer != IntPtr.Zero)
                    {
                        Marshal.Copy(buffers[i].pSecBuffer,
                            securityBuffers[i].Buffer,
                            0,
                            securityBuffers[i].Buffer.Length);
                    }
                }
                encryptBufferDescWrapper.FreeSecurityBufferDesc();
            }
            else
            {
                encryptBufferDescWrapper.FreeSecurityBufferDesc();
                throw new SspiException("Encrypt failed.", hResult);
            }
        }


        /// <summary>
        /// Decrypt message with DecryptMessage Of SSPI
        /// </summary>
        /// <param name="contextHandle">context handle</param>
        /// <param name="sequenceNumber">sequence number</param>
        /// <param name="securityBuffers">SecurityBufferDesc for DecryptMessage</param>
        /// <exception cref="SspiException">If verify fail, throw this exception.</exception>
        [SecurityPermission(SecurityAction.Demand)]
        internal static bool Decrypt(
            ref SecurityHandle contextHandle,
            uint sequenceNumber,
            params SecurityBuffer[] securityBuffers)
        {
            SecurityBufferDescWrapper decryptBufferDescWrapper = new SecurityBufferDescWrapper(securityBuffers);
            //A variable of type ULONG that receives package-specific flags that indicate the quality of protection.
            //Only for out parameter.
            uint pfQop;

            uint hResult = NativeMethods.DecryptMessage(
                ref contextHandle,
                ref decryptBufferDescWrapper.securityBufferDesc,
                sequenceNumber,
                out pfQop);
            if (hResult == NativeMethods.SEC_E_OK)
            {
                SspiSecurityBuffer[] buffers = decryptBufferDescWrapper.securityBufferDesc.GetBuffers();
                for (int i = 0; i < securityBuffers.Length; i++)
                {
                    securityBuffers[i].Buffer = new byte[buffers[i].bufferLength];
                    if (buffers[i].pSecBuffer != IntPtr.Zero)
                    {
                        Marshal.Copy(buffers[i].pSecBuffer,
                            securityBuffers[i].Buffer,
                            0,
                            securityBuffers[i].Buffer.Length);
                        securityBuffers[i].BufferType = (SecurityBufferType)buffers[i].bufferType;
                    }
                }
                decryptBufferDescWrapper.FreeSecurityBufferDesc();
                return true;
            }
            else
            {
                decryptBufferDescWrapper.FreeSecurityBufferDesc();
                throw new SspiException("Decrypt failed.", hResult);
            }
        }


        /// <summary>
        /// Free credential handle
        /// </summary>
        /// <param name="credentialHandle">credential handle</param>
        internal static void FreeCredentialsHandle(ref SecurityHandle credentialHandle)
        {
            uint hResult = NativeMethods.FreeCredentialsHandle(ref credentialHandle);
            if (hResult == NativeMethods.SEC_E_OK)
            {
                credentialHandle.HighPart = IntPtr.Zero;
                credentialHandle.LowPart = IntPtr.Zero;
            }
        }


        /// <summary>
        /// Delete context handle
        /// </summary>
        /// <param name="contextHandle">context handle</param>
        internal static void DeleteSecurityContext(ref SecurityHandle contextHandle)
        {
            uint hResult = NativeMethods.DeleteSecurityContext(ref contextHandle);
            if (hResult == NativeMethods.SEC_E_OK)
            {
                contextHandle.LowPart = IntPtr.Zero;
                contextHandle.HighPart = IntPtr.Zero;
            }
        }


        /// <summary>
        /// Acquire credential with AcquireCredentialsHandle of SSPI.
        /// </summary>
        /// <param name="packageType">Security package type</param>
        /// <param name="accountCredential">Account credential</param>
        /// <param name="serverPrincipal">Server principal name</param>
        /// <param name="fCredentialUse">A flag that indicates how these credentials will be used.</param>
        /// <param name="credentialHandle">Credential handle</param>
        [SecurityPermission(SecurityAction.Demand)]
        internal static void AcquireCredentialsHandle(
            SecurityPackageType packageType,
            AccountCredential accountCredential,
            string serverPrincipal,
            uint fCredentialUse,
            out SecurityHandle credentialHandle)
        {
            string stringPackage = SspiUtility.GetPackageStringName(packageType);

            SecurityInteger expiryTime;
            SecurityWinntAuthIdentity authIdentity = new SecurityWinntAuthIdentity(accountCredential);
            IntPtr pAuthData = IntPtr.Zero;
            SchannelCred schannelCred = new SchannelCred();
            schannelCred.dwVersion = NativeMethods.SCHANNEL_CRED_VERSION;
            schannelCred.cCreds = 0;
            schannelCred.paCred = IntPtr.Zero;
            CredSspCred credSsp = new CredSspCred();

            switch (packageType)
            {
                case SecurityPackageType.Ntlm:
                case SecurityPackageType.Kerberos:
                case SecurityPackageType.Negotiate:
                    pAuthData = SspiUtility.CreateAuthData(authIdentity);
                    break;
                case SecurityPackageType.Schannel:
                    pAuthData = SspiUtility.CreateAuthData(schannelCred);
                    break;
                case SecurityPackageType.CredSsp:
                    credSsp.Type = CredSspSubmitType.CredsspSubmitBufferBoth;
                    credSsp.pSchannelCred = CreateAuthData(schannelCred);
                    credSsp.pSpnegoCred = CreateAuthData(authIdentity);
                    pAuthData = CreateAuthData(credSsp);
                    break;
                //default, if other values, exception will be thrown by GetPackageStringName.
                default:
                    throw new ArgumentException("Invlid packageType value.", "packageType");
            }

            uint result = NativeMethods.AcquireCredentialsHandle(
                serverPrincipal,
                stringPackage,
                fCredentialUse,
                IntPtr.Zero,
                pAuthData,
                IntPtr.Zero,
                IntPtr.Zero,
                out credentialHandle,
                out expiryTime);
            //Free memory
            switch (packageType)
            {
                case SecurityPackageType.Ntlm:
                case SecurityPackageType.Kerberos:
                case SecurityPackageType.Negotiate:
                    SspiUtility.FreeSecurityWinntAuthIdentity(authIdentity);
                    break;
                case SecurityPackageType.Schannel:
                    stringPackage = Schannel;
                    SspiUtility.FreeSchannelCred(schannelCred);
                    break;
                case SecurityPackageType.CredSsp:
                    SspiUtility.FreeSecurityWinntAuthIdentity(authIdentity);
                    SspiUtility.FreeSchannelCred(schannelCred);
                    SspiUtility.FreeCredSspCred(credSsp);
                    break;
                //default, if other values, exception will be thrown by GetPackageStringName.
                default:
                    throw new ArgumentException("Invlid packageType value.", "packageType");
            }
            Marshal.FreeHGlobal(pAuthData);

            if (result != NativeMethods.SEC_E_OK)
            {
                throw new SspiException("AquireCredentialsHandle failed", result);
            }
        }


        /// <summary>
        /// Acquire credential with AcquireCredentialsHandle of SSPI.
        /// </summary>
        /// <param name="packageType">Security package type</param>
        /// <param name="certificateCredential">Account credential</param>
        /// <param name="serverPrincipal">Server principal name</param>
        /// <param name="fCredentialUse">A flag that indicates how these credentials will be used.</param>
        /// <param name="credentialHandle">Credential handle</param>
        [SecurityPermission(SecurityAction.Demand)]
        internal static void AcquireCredentialsHandle(
            SecurityPackageType packageType,
            CertificateCredential certificateCredential,
            string serverPrincipal,
            uint fCredentialUse,
            out SecurityHandle credentialHandle)
        {
            string stringPackage = SspiUtility.GetPackageStringName(packageType);
            SecurityInteger expiryTime = new SecurityInteger();
            uint enabledProtocols;

            if (fCredentialUse == NativeMethods.SECPKG_CRED_OUTBOUND)
            {
                enabledProtocols = NativeMethods.SP_PROT_CLIENTS;
            }
            else
            {
                enabledProtocols = NativeMethods.SP_PROT_SERVERS;
            }

            SchannelCred schannelCred = new SchannelCred(certificateCredential, enabledProtocols);
            CredSspCred sspCred = new CredSspCred();
            IntPtr pAuthData = IntPtr.Zero;

            if (packageType == SecurityPackageType.Schannel)
            {
                pAuthData = SspiUtility.CreateAuthData(schannelCred);
            }
            else if (packageType == SecurityPackageType.CredSsp)
            {
                sspCred.pSchannelCred = SspiUtility.CreateAuthData(schannelCred);
                sspCred.pSpnegoCred = IntPtr.Zero;
                sspCred.Type = CredSspSubmitType.CredsspSubmitBufferBoth;
                pAuthData = SspiUtility.CreateAuthData(sspCred);
            }
            
            uint result = NativeMethods.AcquireCredentialsHandle(
                serverPrincipal,
                stringPackage,
                fCredentialUse,
                IntPtr.Zero,
                pAuthData,
                IntPtr.Zero,
                IntPtr.Zero,
                out credentialHandle,
                out expiryTime);
            //Free memory
            SspiUtility.FreeSchannelCred(schannelCred);
            if (pAuthData != IntPtr.Zero)
            {
                if (packageType == SecurityPackageType.CredSsp)
                {
                    Marshal.FreeHGlobal(sspCred.pSchannelCred);
                }
                Marshal.FreeHGlobal(pAuthData);
            }

            if (result != NativeMethods.SEC_E_OK)
            {
                throw new SspiException("AquireCredentialsHandle fails.", result);
            }
        }


        /// <summary>
        /// Acquire credential with AcquireCredentialsHandle of DTLS.
        /// </summary>
        /// <param name="packageType">Security package type</param>
        /// <param name="certificateCredential">Account credential</param>
        /// <param name="serverPrincipal">Server principal name</param>
        /// <param name="fCredentialUse">A flag that indicates how these credentials will be used.</param>
        /// <param name="credentialHandle">Credential handle</param>
        [SecurityPermission(SecurityAction.Demand)]
        internal static void DtlsAcquireCredentialsHandle(
            SecurityPackageType packageType,
            CertificateCredential certificateCredential,
            string serverPrincipal,
            uint fCredentialUse,
            out SecurityHandle credentialHandle)
        {
            string stringPackage = SspiUtility.GetPackageStringName(packageType);
            SecurityInteger expiryTime = new SecurityInteger();
            uint enabledProtocols;

            if (fCredentialUse == NativeMethods.SECPKG_CRED_OUTBOUND)
            {
                enabledProtocols = NativeMethods.SP_PROT_DTLS_CLIENT;
            }
            else
            {
                enabledProtocols = NativeMethods.SP_PROT_DTLS_SERVER;
            }

            SchannelCred schannelCred = new SchannelCred(certificateCredential, enabledProtocols);
            CredSspCred sspCred = new CredSspCred();
            IntPtr pAuthData = IntPtr.Zero;

            if (packageType == SecurityPackageType.Schannel)
            {
                pAuthData = SspiUtility.CreateAuthData(schannelCred);
            }
            else if (packageType == SecurityPackageType.CredSsp)
            {
                sspCred.pSchannelCred = SspiUtility.CreateAuthData(schannelCred);
                sspCred.pSpnegoCred = IntPtr.Zero;
                sspCred.Type = CredSspSubmitType.CredsspSubmitBufferBoth;
                pAuthData = SspiUtility.CreateAuthData(sspCred);
            }

            uint result = NativeMethods.AcquireCredentialsHandle(
                null,
                stringPackage,
                fCredentialUse,
                IntPtr.Zero,
                pAuthData,
                IntPtr.Zero,
                IntPtr.Zero,
                out credentialHandle,
                out expiryTime);
            //Free memory
            SspiUtility.FreeSchannelCred(schannelCred);
            if (pAuthData != IntPtr.Zero)
            {
                if (packageType == SecurityPackageType.CredSsp)
                {
                    Marshal.FreeHGlobal(sspCred.pSchannelCred);
                }
                Marshal.FreeHGlobal(pAuthData);
            }

            if (result != NativeMethods.SEC_E_OK)
            {
                throw new SspiException("AquireCredentialsHandle fails.", result);
            }
        }

        #region Query context attributes
        /// <summary>
        /// Query context attribute by attribute name.
        /// </summary>
        /// <param name="contextHandle">context handle</param>
        /// <param name="contextAttribute">context attribute name</param>
        /// <returns>Pointer of attribute value.</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SecurityPermission(SecurityAction.Demand)]
        internal static object QueryContextAttributes(ref SecurityHandle contextHandle, string contextAttribute)
        {
            uint attribute = 0;
            IntPtr pBuffer = IntPtr.Zero;
            string stringAttribute = contextAttribute.ToUpper(CultureInfo.InvariantCulture);

            switch (stringAttribute)
            {
                case SECPKG_ATTR_SIZES:
                    attribute = NativeMethods.SECPKG_ATTR_SIZES;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SecurityPackageContextSizes)));
                    break;
                case SECPKG_ATTR_LIFESPAN:
                    attribute = NativeMethods.SECPKG_ATTR_LIFESPAN;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SecurityPackageContextLifespan)));
                    break;
                case SECPKG_ATTR_STREAM_SIZES:
                    attribute = NativeMethods.SECPKG_ATTR_STREAM_SIZES;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SecurityPackageContextStreamSizes)));
                    break;
                case SECPKG_ATTR_KEY_INFO:
                    attribute = NativeMethods.SECPKG_ATTR_KEY_INFO;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SspiSecurityPackageContextKeyInfo)));
                    break;
                case SECPKG_ATTR_AUTHORITY:
                    attribute = NativeMethods.SECPKG_ATTR_AUTHORITY;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SecurityPackageContextAuthority)));
                    break;
                case SECPKG_ATTR_PASSWORD_EXPIRY:
                    attribute = NativeMethods.SECPKG_ATTR_PASSWORD_EXPIRY;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SecurityInteger)));
                    break;
                case SECPKG_ATTR_SESSION_KEY:
                    attribute = NativeMethods.SECPKG_ATTR_SESSION_KEY;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SecurityPackageContextSessionKey)));
                    break;
                case SECPKG_ATTR_PACKAGE_INFO:
                    attribute = NativeMethods.SECPKG_ATTR_PACKAGE_INFO;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
                    break;
                case SECPKG_ATTR_NEGOTIATION_INFO:
                    attribute = NativeMethods.SECPKG_ATTR_NEGOTIATION_INFO;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SspiSecurityPackageContextNegotiationInfo)));
                    break;
                case SECPKG_ATTR_NATIVE_NAMES:
                    attribute = NativeMethods.SECPKG_ATTR_NATIVE_NAMES;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SspiSecurityPackageContextNativeNames)));
                    break;
                case SECPKG_ATTR_FLAGS:
                    attribute = NativeMethods.SECPKG_ATTR_FLAGS;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)));
                    break;
                case SECPKG_ATTR_TARGET_INFORMATION:
                    attribute = NativeMethods.SECPKG_ATTR_TARGET_INFORMATION;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SecurityPackageContextTargetInformation)));
                    break;
                case SECPKG_ATTR_REMOTE_CERT_CONTEXT:
                    attribute = NativeMethods.SECPKG_ATTR_REMOTE_CERT_CONTEXT;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
                    break;
                case SECPKG_ATTR_LOCAL_CERT_CONTEXT:
                    attribute = NativeMethods.SECPKG_ATTR_LOCAL_CERT_CONTEXT;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
                    break;
                case SECPKG_ATTR_ROOT_STORE:
                    attribute = NativeMethods.SECPKG_ATTR_ROOT_STORE;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
                    break;
                case SECPKG_ATTR_ISSUER_LIST_EX:
                    attribute = NativeMethods.SECPKG_ATTR_ISSUER_LIST_EX;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SspiSecurityPackageContextIssuerListInfo)));
                    break;
                case SECPKG_ATTR_CONNECTION_INFO:
                    attribute = NativeMethods.SECPKG_ATTR_CONNECTION_INFO;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SecurityPackageContextConnectionInfo)));
                    break;
                case SECPKG_ATTR_EAP_KEY_BLOCK:
                    attribute = NativeMethods.SECPKG_ATTR_EAP_KEY_BLOCK;
                    pBuffer = Marshal.AllocHGlobal(SchannelKeySize + SchannelIvSize);
                    break;
                case SECPKG_ATTR_APP_DATA:
                    attribute = NativeMethods.SECPKG_ATTR_APP_DATA;
                    pBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SecurityPackageContextSessionAppData)));
                    break;
                default:
                    throw new ArgumentException("Error context attribute name.", "contextAttribute");
            }

            uint hResult = NativeMethods.QueryContextAttributes(ref contextHandle, attribute, pBuffer);
            if (hResult == NativeMethods.SEC_E_OK)
            {
                object attributeValue;
                switch (stringAttribute)
                {
                    case SECPKG_ATTR_SIZES:
                        attributeValue = Marshal.PtrToStructure(pBuffer, typeof(SecurityPackageContextSizes));
                        break;
                    case SECPKG_ATTR_LIFESPAN:
                        attributeValue = Marshal.PtrToStructure(pBuffer, typeof(SecurityPackageContextLifespan));
                        break;
                    case SECPKG_ATTR_STREAM_SIZES:
                        attributeValue = Marshal.PtrToStructure(pBuffer, typeof(SecurityPackageContextStreamSizes));
                        break;
                    case SECPKG_ATTR_KEY_INFO:
                        SspiSecurityPackageContextKeyInfo keyInfo;

                        keyInfo = (SspiSecurityPackageContextKeyInfo)Marshal.PtrToStructure(
                            pBuffer,
                            typeof(SspiSecurityPackageContextKeyInfo));
                        attributeValue = new SecurityPackageContextKeyInfo(keyInfo);
                        Marshal.FreeHGlobal(keyInfo.sSignatureAlgorithmName);
                        Marshal.FreeHGlobal(keyInfo.sEncryptAlgorithmName);
                        break;
                    case SECPKG_ATTR_AUTHORITY:
                        SecurityPackageContextAuthority authority;

                        authority = (SecurityPackageContextAuthority)Marshal.PtrToStructure(
                            pBuffer,
                            typeof(SecurityPackageContextAuthority));
                        attributeValue = Marshal.PtrToStringUni(authority.sAuthorityName);
                        break;
                    case SECPKG_ATTR_PASSWORD_EXPIRY:
                        attributeValue = Marshal.PtrToStructure(pBuffer, typeof(SecurityInteger));
                        break;
                    case SECPKG_ATTR_SESSION_KEY:
                        SecurityPackageContextSessionKey sessionKey;

                        sessionKey = (SecurityPackageContextSessionKey)Marshal.PtrToStructure(
                            pBuffer,
                            typeof(SecurityPackageContextSessionKey));
                        attributeValue = sessionKey.GetSessionKey();
                        Marshal.FreeHGlobal(sessionKey.SessionKey);
                        break;
                    case SECPKG_ATTR_PACKAGE_INFO:
                        SspiSecurityPackageInformation packageInfo;

                        packageInfo = (SspiSecurityPackageInformation)Marshal.PtrToStructure(
                            Marshal.ReadIntPtr(pBuffer),
                            typeof(SspiSecurityPackageInformation));
                        attributeValue = new SecurityPackageInformation(packageInfo);
                        break;
                    case SECPKG_ATTR_NEGOTIATION_INFO:
                        SspiSecurityPackageContextNegotiationInfo negotiationInfo;

                        negotiationInfo = (SspiSecurityPackageContextNegotiationInfo)Marshal.PtrToStructure(
                            pBuffer,
                            typeof(SspiSecurityPackageContextNegotiationInfo));
                        attributeValue = new SecurityPackageContextNegotiationInfo(negotiationInfo);
                        Marshal.FreeHGlobal(negotiationInfo.PackageInfo);
                        break;
                    case SECPKG_ATTR_NATIVE_NAMES:
                        SspiSecurityPackageContextNativeNames nativeNames;

                        nativeNames = (SspiSecurityPackageContextNativeNames)Marshal.PtrToStructure(
                            pBuffer,
                            typeof(SspiSecurityPackageContextNativeNames));
                        attributeValue = new SecurityPackageContextNativeNames(nativeNames);
                        break;
                    case SECPKG_ATTR_FLAGS:
                        attributeValue = (uint)Marshal.ReadInt32(pBuffer);
                        break;
                    case SECPKG_ATTR_TARGET_INFORMATION:
                        SecurityPackageContextTargetInformation targetInformation;

                        targetInformation = (SecurityPackageContextTargetInformation)Marshal.PtrToStructure(
                            pBuffer, 
                            typeof(SecurityPackageContextTargetInformation));
                        attributeValue = targetInformation.GetTargetInfo();
                        Marshal.FreeHGlobal(targetInformation.MarshalledTargetInfo);
                        break;
                    case SECPKG_ATTR_REMOTE_CERT_CONTEXT:
                        SspiCertContext remoteCertContext;

                        remoteCertContext = (SspiCertContext)Marshal.PtrToStructure(
                            Marshal.ReadIntPtr(pBuffer), 
                            typeof(SspiCertContext));
                        attributeValue = new CertContext(remoteCertContext);
                        Marshal.FreeHGlobal(remoteCertContext.pbCertEncoded);
                        break;
                    case SECPKG_ATTR_LOCAL_CERT_CONTEXT:
                        SspiCertContext localCertContext;

                        localCertContext = (SspiCertContext)Marshal.PtrToStructure(
                            Marshal.ReadIntPtr(pBuffer), 
                            typeof(SspiCertContext));
                        attributeValue = new CertContext(localCertContext);
                        Marshal.FreeHGlobal(localCertContext.pbCertEncoded);
                        break;
                    case SECPKG_ATTR_ROOT_STORE:
                        attributeValue = Marshal.ReadIntPtr(pBuffer);
                        break;
                    case SECPKG_ATTR_ISSUER_LIST_EX:
                        SspiSecurityPackageContextIssuerListInfo issuerList;

                        issuerList = (SspiSecurityPackageContextIssuerListInfo)Marshal.PtrToStructure(
                            pBuffer,
                            typeof(SspiSecurityPackageContextIssuerListInfo));
                        attributeValue = new SecurityPackageContextIssuerListInfo(issuerList);
                        NativeMethods.FreeContextBuffer(issuerList.aIssuers);
                        break;
                    case SECPKG_ATTR_CONNECTION_INFO:
                        SecurityPackageContextConnectionInfo connectionInfo;

                        connectionInfo = (SecurityPackageContextConnectionInfo)Marshal.PtrToStructure(
                            pBuffer,
                            typeof(SecurityPackageContextConnectionInfo));
                        attributeValue = connectionInfo;
                        break;
                    case SECPKG_ATTR_EAP_KEY_BLOCK:
                        SecurityPackageContextEapKeyBlock eapKeyBlock = new SecurityPackageContextEapKeyBlock();

                        eapKeyBlock.rgbKeys = new byte[SchannelKeySize];
                        Marshal.Copy(pBuffer, eapKeyBlock.rgbKeys, 0, SchannelKeySize);
                        eapKeyBlock.rgbIVs = new byte[SchannelIvSize];
                        Marshal.Copy(new IntPtr(pBuffer.ToInt64() + SchannelKeySize),
                            eapKeyBlock.rgbIVs,
                            0,
                            SchannelIvSize);
                        attributeValue = eapKeyBlock;
                        break;
                    case SECPKG_ATTR_APP_DATA:
                        SecurityPackageContextSessionAppData sessionAppData;

                        sessionAppData = (SecurityPackageContextSessionAppData)Marshal.PtrToStructure(
                            pBuffer, 
                            typeof(SecurityPackageContextSessionAppData));
                        attributeValue = sessionAppData.GetAppData();
                        Marshal.FreeHGlobal(sessionAppData.pbAppData);
                        break;
                    default:
                        throw new ArgumentException("Error context attribute name.", "contextAttribute");
                }
                Marshal.FreeHGlobal(pBuffer);

                return attributeValue;
            }
            else
            {
                Marshal.FreeHGlobal(pBuffer);
                throw new SspiException("QueryContextAttributes failed.", hResult);
            }
        }
        #endregion

        #endregion Windows SSPI

        #region Ntsec API

        /// <summary>
        /// retrieve the local machine account password.
        /// </summary>
        /// <returns>
        /// a string that specifies the local macine account password.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// thrown when LsaOpenPolicy STATUS_ACCESS_DENIED, please run the program as administrator
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when LsaOpenPolicy failed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when LsaRetrievePrivateData failed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when local machine account is invalid
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// thrown when local machine account password is invalid
        /// </exception>
        [SecurityPermission(SecurityAction.Demand)]
        public static string RetrieveLocalMachineAccountPassword()
        {
            NtStatus ntsResult = NtStatus.STATUS_SUCCESS;

            // Open PolicyHandle
            IntPtr policyHandle = IntPtr.Zero;
            LsaObjectAttributes objectAttributes = new LsaObjectAttributes();

            ntsResult = NativeMethods.LsaOpenPolicy(
                IntPtr.Zero, // NULL, the function opens the Policy object on the local system.
                ref objectAttributes, AccessMask.POLICY_GET_PRIVATE_INFORMATION, out policyHandle);

            if (ntsResult == NtStatus.STATUS_ACCESS_DENIED)
            {
                throw new InvalidOperationException(
                    "LsaOpenPolicy STATUS_ACCESS_DENIED, please run the program as administrator");
            }

            if (ntsResult != NtStatus.STATUS_SUCCESS)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "LsaOpenPolicy failed, Result={0}", ntsResult));
            }

            IntPtr privateData = IntPtr.Zero;
            LsaUnicodeString keyName = new LsaUnicodeString();
            try
            {
                // Retrieve PrivateData
                string localMachineNameKey = "$MACHINE.ACC";
                keyName.Buffer = Marshal.StringToHGlobalUni(localMachineNameKey);
                keyName.Length = (ushort)(UnicodeEncoding.CharSize * localMachineNameKey.Length);
                keyName.MaximumLength = (ushort)(keyName.Length + UnicodeEncoding.CharSize);

                ntsResult = NativeMethods.LsaRetrievePrivateData(policyHandle, ref keyName, out privateData);

                if (ntsResult != NtStatus.STATUS_SUCCESS)
                {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.InvariantCulture, "LsaRetrievePrivateData failed, Result={0}", ntsResult));
                }

                //Get Local Machine Account Password
                LsaUnicodeString localMachineAccount =
                    (LsaUnicodeString)Marshal.PtrToStructure(privateData, typeof(LsaUnicodeString));

                if (localMachineAccount.Buffer == IntPtr.Zero)
                {
                    throw new InvalidOperationException("local machine account is invalid");
                }

                if (localMachineAccount.Length == 0)
                {
                    throw new InvalidOperationException("local machine account password is invalid");
                }

                return Marshal.PtrToStringUni(
                    localMachineAccount.Buffer, localMachineAccount.Length / UnicodeEncoding.CharSize);
            }
            finally
            {
                if (privateData != IntPtr.Zero)
                {
                    NativeMethods.LsaFreeMemory(privateData);
                }
                if (keyName.Buffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(keyName.Buffer);
                }
                if (policyHandle != IntPtr.Zero)
                {
                    NativeMethods.LsaClose(policyHandle);
                }
            }
        }

        #endregion
    }
}
