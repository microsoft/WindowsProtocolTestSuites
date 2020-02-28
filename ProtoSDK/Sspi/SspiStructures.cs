// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// Security handle
    /// http://msdn.microsoft.com/en-us/library/aa380495(VS.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SecurityHandle
    {
        internal IntPtr LowPart;
        internal IntPtr HighPart;
    }

    /// <summary>
    /// SECURITY_INTEGER is a structure that holds a numeric value. It is used in defining other types.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SecurityInteger
    {
        /// <summary>
        /// Least significant digits.
        /// </summary>
        public uint LowPart;

        /// <summary>
        /// Most significant digits.
        /// </summary>
        public int HighPart;
    }

    /// <summary>
    /// SecPkgContext_Lifespan Structure
    /// http://msdn.microsoft.com/en-us/library/aa380087(v=VS.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SecurityPackageContextLifespan
    {
        /// <summary>
        /// Time at which the context was established.
        /// </summary>
        public SecurityInteger tsStart;

        /// <summary>
        /// Time at which the context will expire.
        /// </summary>
        public SecurityInteger tsExpiry;
    }

    /// <summary>
    /// The SecPkgContext_StreamSizes structure indicates the sizes of the various stream components for use with the 
    /// message support functions. 
    /// http://msdn.microsoft.com/en-us/library/aa380098(v=VS.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SecurityPackageContextStreamSizes
    {
        /// <summary>
        /// Specifies the size, in bytes, of the header portion. If zero, no header is used.
        /// </summary>
        public uint Header;

        /// <summary>
        /// Specifies the maximum size, in bytes, of the trailer portion. If zero, no trailer is used.
        /// </summary>
        public uint Trailer;

        /// <summary>
        /// Specifies the size, in bytes, of the largest message that can be encapsulated.
        /// </summary>
        public uint MaximumMessage;

        /// <summary>
        /// Specifies the number of buffers to pass.
        /// </summary>
        public uint Buffers;

        /// <summary>
        /// Specifies the preferred integral size of the messages. For example, eight indicates that messages should be
        /// of size zero mod eight for optimal performance. Messages other than this block size can be padded.
        /// </summary>
        public uint BlockSize;
    }

    /// <summary>
    /// SecPkgContext_SessionKey Structure
    /// http://msdn.microsoft.com/en-us/library/aa380096(v=VS.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SecurityPackageContextSessionKey
    {
        /// <summary>
        /// Size, in bytes, of the session key.
        /// </summary>
        internal uint SessionKeyLength;

        /// <summary>
        /// The session key for the security context.
        /// </summary>
        internal IntPtr SessionKey;

        /// <summary>
        /// Get byte array of Session key
        /// </summary>
        /// <returns></returns>
        [SecurityPermission(SecurityAction.Demand)]
        internal byte[] GetSessionKey()
        {
            byte[] key = null;

            if (this.SessionKeyLength > 0)
            {
                key = new byte[this.SessionKeyLength];
                Marshal.Copy(this.SessionKey, key, 0, key.Length);
            }
            return key;
        }
    }


    /// <summary>
    /// The SecBuffer structure describes a buffer allocated by a transport application to pass to a security package.
    /// reference: http://msdn.microsoft.com/en-us/library/aa379814(VS.85).aspx
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable"),
    StructLayout(LayoutKind.Sequential)]
    internal struct SspiSecurityBuffer
    {
        /// <summary>
        /// Length of buffer.
        /// </summary>
        internal uint bufferLength;

        /// <summary>
        /// Bit flags that indicate the type of buffer.
        /// </summary>
        internal uint bufferType;

        /// <summary>
        /// A buffer contains data.
        /// </summary>
        internal IntPtr pSecBuffer;

        /// <summary>
        /// Convert SecurityBuffer to SspiSecurityBuffer.
        /// </summary>
        /// <param name="securityBuffer">SecurityBuffer</param>
        /// <exception cref="ArgumentNullException">If securityBuffer is null, this exception will be thrown.
        /// </exception>
        internal SspiSecurityBuffer(SecurityBuffer securityBuffer)
        {
            if (securityBuffer == null)
            {
                throw new ArgumentNullException("securityBuffer");
            }

            if (securityBuffer.Buffer == null || securityBuffer.Buffer.Length == 0)
            {
                this.bufferLength = 0;
                this.pSecBuffer = IntPtr.Zero;
            }
            else
            {
                this.bufferLength = (uint)securityBuffer.Buffer.Length;
                this.pSecBuffer = Marshal.AllocHGlobal((int)this.bufferLength);
                Marshal.Copy(securityBuffer.Buffer, 0, this.pSecBuffer, (int)this.bufferLength);
            }
            this.bufferType = (uint)securityBuffer.BufferType;
        }
    }

    /// <summary>
    /// Defined SCHANNEL_CRED structure of SSPI.
    /// http://msdn.microsoft.com/en-us/library/aa379810(VS.85).aspx
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable")]
    internal struct SchannelCred
    {
        /// <summary>
        /// Set to SCHANNEL_CRED_VERSION.
        /// </summary>
        internal uint dwVersion;

        /// <summary>
        /// The number of structures in the paCred array.
        /// </summary>
        internal uint cCreds;

        /// <summary>
        /// An array of pointers to CERT_CONTEXT structures.
        /// </summary>
        internal IntPtr paCred;

        /// <summary>
        /// Optional. Valid for server applications only.Handle to a certificate store that contains self-signed
        /// root certificates for certification authorities (CAs) trusted by the application. This member is used
        /// only by server-side applications that require client authentication.
        /// </summary>
        internal IntPtr hRootStore;

        /// <summary>
        /// Reserved
        /// </summary>
        internal uint cMappers;

        /// <summary>
        /// Reserved
        /// </summary>
        internal IntPtr aphMappers;

        /// <summary>
        /// Number of algorithms in the palgSupportedAlgs array.
        /// </summary>
        internal uint cSupportedAlgs;

        /// <summary>
        /// Optional. A pointer to an array of ALG_ID algorithm identifiers that represent the algorithms supported by
        /// connections made with credentials acquired using this structure. 
        /// </summary>
        internal IntPtr palgSupportedAlgs;

        /// <summary>
        /// Optional. A DWORD that contains a bit string that represents the protocols supported by connections made 
        /// with credentials acquired by using this structure. 
        /// </summary>
        internal uint grbitEnabledProtocols;

        /// <summary>
        /// nimum bulk encryption cipher strength, in bits, allowed for connections.
        /// </summary>
        internal uint dwMinimumCipherStrength;

        /// <summary>
        /// The number of milliseconds that Schannel keeps the session in its session cache.
        /// </summary>
        internal uint dwMaximumCipherStrength;

        /// <summary>
        /// The number of milliseconds that Schannel keeps the session in its session cache. After this time has 
        /// passed, any new connections between the client and the server require a new Schannel session
        /// </summary>
        internal uint dwSessionLifespan;

        /// <summary>
        /// Contains bit flags that control the behavior of Schannel.
        /// </summary>
        internal uint dwFlags;

        /// <summary>
        /// Reserved. Must be zero.
        /// </summary>
        internal uint dwCredFormat;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="credential">Certificate credential</param>
        /// <param name="enabledProtocols">Enabled protocols</param>
        internal SchannelCred(CertificateCredential credential, uint enabledProtocols)
        {
            //There is 1 structures in the paCred array.
            const uint CountOfCred = 1;

            this.dwVersion = NativeMethods.SCHANNEL_CRED_VERSION;
            if (credential != null && credential.Certificate != null)
            {
                this.cCreds = CountOfCred;
                this.paCred = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
                Marshal.WriteIntPtr(this.paCred, credential.Certificate.Handle);
            }
            else
            {
                this.paCred = IntPtr.Zero;
                this.cCreds = 0;
            }
            this.hRootStore = IntPtr.Zero;
            this.cMappers = 0;
            this.aphMappers = IntPtr.Zero;
            this.cSupportedAlgs = 0;
            this.palgSupportedAlgs = IntPtr.Zero;
            this.grbitEnabledProtocols = enabledProtocols;
            this.dwMinimumCipherStrength = 0;
            this.dwMaximumCipherStrength = 0;
            this.dwSessionLifespan = 0;
            if (credential == null || credential.Certificate == null)
            {
                this.dwFlags = NativeMethods.SCH_CRED_MANUAL_CRED_VALIDATION | NativeMethods.SCH_CRED_NO_DEFAULT_CREDS;
            }
            else
            {
                this.dwFlags = 0;
            }
            this.dwCredFormat = 0;
        }
    }

    /// <summary>
    /// The SecBufferDesc structure describes an array of SecBuffer structures to pass from a transport application 
    /// to a security package. 
    /// reference: http://msdn.microsoft.com/en-us/library/aa379815(VS.85).aspx
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable"),
    StructLayout(LayoutKind.Sequential)]
    internal struct SecurityBufferDesc
    {
        /// <summary>
        /// Specifies the version number of this structure. This member must be SECBUFFER_VERSION.
        /// </summary>
        internal int ulVersion;

        /// <summary>
        /// Indicates the number of SecBuffer structures in the pBuffers array
        /// </summary>
        internal int cBuffers;

        /// <summary>
        /// Pointer to an array of SecBuffer structures
        /// </summary>
        internal IntPtr pBuffers;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="secBuffers">SecBuffer array</param>
        internal SecurityBufferDesc(params SecurityBuffer[] secBuffers)
        {
            this.ulVersion = NativeMethods.SECBUFFER_VERSION;
            this.cBuffers = 0;
            this.pBuffers = IntPtr.Zero;

            if (secBuffers != null && secBuffers.Length != 0)
            {
                this.cBuffers = secBuffers.Length;
                int bufferLength = Marshal.SizeOf(typeof(SspiSecurityBuffer));
                this.pBuffers = Marshal.AllocHGlobal(bufferLength * this.cBuffers);
                for (int i = 0; i < secBuffers.Length; i++)
                {
                    SspiSecurityBuffer sspiSecurityBuffer = new SspiSecurityBuffer(secBuffers[i]);
                    IntPtr pBuffer = new IntPtr(pBuffers.ToInt64() + bufferLength * i);
                    Marshal.StructureToPtr(sspiSecurityBuffer, pBuffer, false);
                }
            }
        }


        /// <summary>
        /// Get buffers from buffer pointer.
        /// </summary>
        /// <returns>SecBuffers contained in SecBufferDesc</returns>
        [SecurityPermission(SecurityAction.Demand)]
        internal SspiSecurityBuffer[] GetBuffers()
        {
            SspiSecurityBuffer[] securityBuffers = new SspiSecurityBuffer[this.cBuffers];
            int bufferLength = Marshal.SizeOf(typeof(SspiSecurityBuffer));

            for (int i = 0; i < this.cBuffers; i++)
            {
                securityBuffers[i] = (SspiSecurityBuffer)Marshal.PtrToStructure(
                    new IntPtr(this.pBuffers.ToInt64() + bufferLength * i),
                    typeof(SspiSecurityBuffer));
            }
            return securityBuffers;
        }
    }

    /// <summary>
    /// Wrapper of SecurityBufferDesc for store pointers to allocated by marshal.
    /// </summary>
    internal struct SecurityBufferDescWrapper
    {
        /// <summary>
        /// Security Buffer Desc
        /// </summary>
        internal SecurityBufferDesc securityBufferDesc;

        /// <summary>
        /// Store the address and size of a block of memory allocated by marshal
        /// </summary>
        private struct MarshalMemory
        {
            /// <summary>
            /// The pointer points to the beginning of the memory block
            /// </summary>
            internal IntPtr pointer;
            /// <summary>
            /// Size of the memory block 
            /// </summary>
            internal uint size;
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="pointer">The pointer points to the beginning of the memory block</param>
            /// <param name="size">Size of the memory block</param>
            internal MarshalMemory(IntPtr pointer, uint size)
            {
                this.pointer = pointer;
                this.size = size;
            }
        }

        /// <summary>
        /// Pointers to memory allocated by marshal.
        /// </summary>
        private MarshalMemory[] marshalMemory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="secBuffers">Security buffers</param>
        internal SecurityBufferDescWrapper(params SecurityBuffer[] secBuffers)
        {
            this.securityBufferDesc = new SecurityBufferDesc(secBuffers);
            List<MarshalMemory> memoryList = new List<MarshalMemory>();
            //Store information of memory allocated by marshal
            foreach (SspiSecurityBuffer securityBuffer in this.securityBufferDesc.GetBuffers())
            {
                if (securityBuffer.pSecBuffer != IntPtr.Zero)
                {
                    memoryList.Add(new MarshalMemory(securityBuffer.pSecBuffer, securityBuffer.bufferLength));
                }
            }
            this.marshalMemory = memoryList.ToArray();
        }

        /// <summary>
        /// Free memory in SecurityBufferDesc.
        /// </summary>
        [SecurityPermission(SecurityAction.Demand)]
        internal void FreeSecurityBufferDesc()
        {
            SspiSecurityBuffer[] buffers = this.securityBufferDesc.GetBuffers();

            //Free all marshal allocated memory
            foreach (MarshalMemory mem in this.marshalMemory)
            {
                try
                {
                    Marshal.FreeHGlobal(mem.pointer);
                }
                catch (Exception e) //Catch Exception instead of COMException 
                {
                    // Catch this exception in case the mem.pointer is an invalid handler
                    throw new InvalidOperationException ("Unable to free allocated memory with Marshal.FreeHGlobal: " + e.Message);                    
                }
            }

            //Free memory allocated by security package 
            Array.Sort(buffers, delegate (SspiSecurityBuffer x, SspiSecurityBuffer y)
            {
                return x.pSecBuffer.ToInt64().CompareTo(y.pSecBuffer.ToInt64());
            });
            //buffers are sorted by address order(low to high), To avoid the possibility of two buffers overlaps some address place,
            //use variable nextMinAddr to indicate the min address should next buffer have to avoid freeing some memory multiple times
            long nextMinAddr = 0;
            foreach (SspiSecurityBuffer buf in buffers)
            {
                if (isAllocatedBySecurityPackage(buf.pSecBuffer) && buf.pSecBuffer.ToInt64() >= nextMinAddr)
                {
                    NativeMethods.FreeContextBuffer(buf.pSecBuffer);
                    nextMinAddr = buf.pSecBuffer.ToInt64() + buf.bufferLength;
                }
            }
            //Free SecBufferDesc
            this.marshalMemory = new MarshalMemory[0];
            Marshal.FreeHGlobal(this.securityBufferDesc.pBuffers);
            this.securityBufferDesc.pBuffers = IntPtr.Zero;
            this.securityBufferDesc.cBuffers = 0;
        }

        /// <summary>
        /// Check if the pointer is allocated by security package. 
        /// </summary>
        /// <param name="pointer">Pointer to be checked</param>
        /// <returns>Return true if the pointer is allocated by security package, else return false.</returns>
        private bool isAllocatedBySecurityPackage(IntPtr pointer)
        {
            if (pointer == IntPtr.Zero)
            {
                return false;
            }
            foreach (MarshalMemory mem in this.marshalMemory)
            {
                long offset = pointer.ToInt64() - mem.pointer.ToInt64();
                if (offset >= 0 && offset < mem.size)
                {
                    return false;
                }
            }
            return true;
        }
    }


    /// <summary>
    /// Allows you to pass a particular user name and password to the run-time library for the purpose of 
    /// authentication.
    /// Reference: http://msdn.microsoft.com/en-us/library/aa380131(VS.85).aspx
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable")]
    internal struct SecurityWinntAuthIdentity
    {
        /// <summary>
        /// String containing the user name.
        /// </summary>
        public IntPtr User;

        /// <summary>
        /// Length of the usersting in characters, not including the terminating null.
        /// </summary>
        public int UserLength;

        /// <summary>
        /// String containing the domain name or the workgroup name.
        /// </summary>
        public IntPtr Domain;

        /// <summary>
        /// Length of the domain string in characters, not including the terminating null.
        /// </summary>
        public int DomainLength;

        /// <summary>
        /// String containing the user's password in the domain or workgroup.
        /// </summary>
        public IntPtr Password;

        /// <summary>
        /// Length of the password string in characters, not including the terminating null.
        /// </summary>
        public int PasswordLength;

        /// <summary>
        /// The strings in this structure are in ANSI format or Unicode format.
        /// </summary>
        public int Flags;


        /// <summary>
        /// Constructor. Convert account 
        /// </summary>
        /// <param name="credential">Account credential</param>
        public SecurityWinntAuthIdentity(AccountCredential credential)
        {
            this.User = IntPtr.Zero;
            this.UserLength = 0;
            this.Domain = IntPtr.Zero;
            this.DomainLength = 0;
            this.Password = IntPtr.Zero;
            this.PasswordLength = 0;
            this.Flags = NativeMethods.SEC_WINNT_AUTH_IDENTITY_UNICODE;

            if (credential != null)
            {
                if (credential.AccountName != null)
                {
                    this.UserLength = credential.AccountName.Length;
                    if (this.UserLength != 0)
                    {
                        this.User = Marshal.StringToHGlobalUni(credential.AccountName);
                    }
                }
                if (credential.DomainName != null)
                {
                    this.DomainLength = credential.DomainName.Length;
                    if (this.DomainLength != 0)
                    {
                        this.Domain = Marshal.StringToHGlobalUni(credential.DomainName);
                    }
                }
                if (credential.Password != null)
                {
                    this.PasswordLength = credential.Password.Length;
                    if (this.PasswordLength != 0)
                    {
                        this.Password = Marshal.StringToHGlobalUni(credential.Password);
                    }
                }
            }
        }
    }

    /// <summary>
    /// The SecPkgContext_KeyInfo structure contains information about the session keys used in a security context.
    /// http://msdn.microsoft.com/en-us/library/aa380086(v=VS.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SspiSecurityPackageContextKeyInfo
    {
        /// <summary>
        /// Pointer to a null-terminated string that contains the name, if available, of the algorithm used for 
        /// generating signatures, for example "MD5" or "SHA-2".
        /// </summary>
        internal IntPtr sSignatureAlgorithmName;

        /// <summary>
        /// Pointer to a null-terminated string that contains the name, if available, of the algorithm used for 
        /// encrypting messages. Reserved for future use.
        /// </summary>
        internal IntPtr sEncryptAlgorithmName;

        /// <summary>
        /// Specifies the effective key length, in bits, for the session key. This is typically 40, 56, or 128 bits.
        /// </summary>
        internal uint KeySize;

        /// <summary>
        /// Specifies the algorithm identifier (ALG_ID) used for generating signatures, if available.
        /// </summary>
        internal uint SignatureAlgorithm;

        /// <summary>
        /// Specifies the algorithm identifier (ALG_ID) used for encrypting messages. Reserved for future use.
        /// </summary>
        internal uint EncryptAlgorithm;
    }

    /// <summary>
    /// The SecPkgContext_Authority structure contains the name of the authenticating authority if one is available.
    /// It can be a certification authority (CA) or the name of a server or domain that authenticated the connection. 
    /// http://msdn.microsoft.com/en-us/library/aa379818(v=VS.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SecurityPackageContextAuthority
    {
        /// <summary>
        /// Pointer to a null-terminated string containing the name of the authenticating authority, if available.
        /// </summary>
        internal IntPtr sAuthorityName;
    }

    /// <summary>
    /// The SecPkgInfo structure provides general information about a security package, such as its name 
    /// and capabilities.
    /// http://msdn.microsoft.com/en-us/library/aa380104(v=VS.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SspiSecurityPackageInformation
    {
        /// <summary>
        /// Set of bit flags that describes the capabilities of the security package. 
        /// </summary>
        internal uint fCapabilities;

        /// <summary>
        /// Specifies the version of the package protocol. Must be 1.
        /// </summary>
        internal ushort wVersion;

        /// <summary>
        /// Specifies a DCE RPC identifier, if appropriate. If the package does not implement one of the DCE registered
        /// security systems, the reserved value SECPKG_ID_NONE is used.
        /// </summary>
        internal ushort wRpcId;

        /// <summary>
        /// Specifies the maximum size, in bytes, of the token.
        /// </summary>
        internal uint cbMaxToken;

        /// <summary>
        /// Pointer to a null-terminated string that contains the name of the security package.
        /// </summary>
        internal IntPtr Name;

        /// <summary>
        /// Pointer to a null-terminated string. This can be any additional string passed back by the package.
        /// </summary>
        internal IntPtr Comment;
    }

    /// <summary>
    /// The SecPkgContext_PackageInfo structure contains the name of a security support provider (SSP). This structure 
    /// is returned by the QueryContextAttributes (General) function. It would most often be used when the SSP in use 
    /// was established using the Negotiatesecurity package.
    /// http://msdn.microsoft.com/en-us/library/aa380092(v=VS.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SecurityPackageContextPackageInformation
    {
        /// <summary>
        /// Pointer to a SecPkgInfo structure containing the name of the SSP in use.
        /// </summary>
        internal IntPtr packageInfo;
    }

    /// <summary>
    /// The SecPkgContext_NegotiationInfo structure contains information on the security package that is being set up 
    /// or has been set up, and also gives the status on the negotiation to set up the security package.
    /// http://msdn.microsoft.com/en-us/library/aa380091(v=VS.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SspiSecurityPackageContextNegotiationInfo
    {
        /// <summary>
        /// Pointer to a SecPkgInfo structure that provides general information about the security package chosen in
        /// the negotiate process, such as the name and capabilities of the package.
        /// </summary>
        internal IntPtr PackageInfo;

        /// <summary>
        /// Indicator of the state of the negotiation for the security package identified in the PackageInfo member.
        /// This attribute can be queried from the context handle before the setup is complete, such as when ISC 
        /// returns SEC_I_CONTINUE_NEEDED.
        /// </summary>
        internal uint NegotiationState;
    }

    /// <summary>
    /// The SecPkgContext_NativeNames structure returns the client and server principal names from the outbound ticket.
    /// This structure is valid only for client outbound tickets.
    /// http://msdn.microsoft.com/en-us/library/aa380090(v=VS.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SspiSecurityPackageContextNativeNames
    {
        /// <summary>
        /// Pointer to a null-terminated string that represents the principal name for the client in the outbound 
        /// ticket. This string should never be NULL when querying a security context negotiated with Kerberos.
        /// </summary>
        internal IntPtr sClientName;

        /// <summary>
        /// Pointer to a null-terminated string that represents the principal name for the server in the outbound
        /// ticket. This string should never be NULL when querying a security context negotiated with Kerberos.
        /// </summary>
        internal IntPtr sServerName;
    }

    /// <summary>
    /// The SecPkgContext_TargetInformation structure returns information about the credential used for the security
    /// context.
    /// http://msdn.microsoft.com/en-us/library/aa380099(v=VS.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SecurityPackageContextTargetInformation
    {
        /// <summary>
        /// Size, in bytes, of MarshalledTargetInfo
        /// </summary>
        internal uint MarshalledTargetInfoLength;

        /// <summary>
        /// Array of bytes that represent the credential, if a credential is provided by a credential manager.
        /// </summary>
        internal IntPtr MarshalledTargetInfo;

        [SecurityPermission(SecurityAction.Demand)]
        internal byte[] GetTargetInfo()
        {
            byte[] targetInfo = null;

            if (this.MarshalledTargetInfoLength > 0)
            {
                targetInfo = new byte[this.MarshalledTargetInfoLength];
                Marshal.Copy(this.MarshalledTargetInfo, targetInfo, 0, targetInfo.Length);
            }
            return targetInfo;
        }
    }

    /// <summary>
    /// The CERT_CONTEXT structure contains both the encoded and decoded representations of a certificate. A 
    /// certificate context returned by one of the functions defined in Wincrypt.h must be freed by calling the 
    /// CertFreeCertificateContext function. 
    /// http://msdn.microsoft.com/en-us/library/aa377189(v=VS.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SspiCertContext
    {
        /// <summary>
        /// Type of encoding used. It is always acceptable to specify both the certificate and message encoding 
        /// types by combining them with a bitwise-OR operation.
        /// </summary>
        internal uint dwCertEncodingType;

        /// <summary>
        /// A pointer to a buffer that contains the encoded certificate.
        /// </summary>
        internal IntPtr pbCertEncoded;

        /// <summary>
        /// The size, in bytes, of the encoded certificate.
        /// </summary>
        internal uint cbCertEncoded;

        /// <summary>
        /// The address of a CERT_INFO structure that contains the certificate information.
        /// </summary>
        internal IntPtr pCertInfo;

        /// <summary>
        /// A handle to the certificate store that contains the certificate context.
        /// </summary>
        internal IntPtr hCertStore;
    }

    /// <summary>
    /// The SecPkgContext_IssuerListInfoEx structure holds a list of trusted certification authorities (CAs).
    /// This structure is used by the Schannel security package InitializeSecurityContext (Schannel) function.
    /// This attribute is supported only by the Schannel security support provider (SSP).
    /// http://msdn.microsoft.com/en-us/library/aa380078(v=VS.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SspiSecurityPackageContextIssuerListInfo
    {
        /// <summary>
        /// A pointer to an array of CERT_NAME_BLOB structures that contains a list of the names of CAs that the server
        /// trusts.
        /// </summary>
        internal IntPtr aIssuers;

        /// <summary>
        /// The number of names in aIssuers.
        /// </summary>
        internal uint cIssuers;
    }

    /// <summary>
    /// The CRYPT_INTEGER_BLOB structure contains an arbitrary array of bytes. The structure definition includes 
    /// aliases appropriate to the various functions that use it.
    /// http://msdn.microsoft.com/en-us/library/aa381414(v=VS.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct CryptoApiBlob
    {
        /// <summary>
        /// A DWORD variable that contains the count, in bytes, of data.
        /// </summary>
        internal uint cbData;

        /// <summary>
        /// A pointer to the data buffer.
        /// </summary>
        internal IntPtr pbData;

        [SecurityPermission(SecurityAction.Demand)]
        internal byte[] GetData()
        {
            byte[] data = null;

            if (cbData > 0)
            {
                data = new byte[cbData];
                Marshal.Copy(this.pbData, data, 0, data.Length);
            }
            return data;
        }
    }

    /// <summary>
    /// The SecPkgContext_ConnectionInfo structure contains protocol and cipher information.
    /// http://msdn.microsoft.com/en-us/library/aa379819(v=VS.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SecurityPackageContextConnectionInfo
    {
        /// <summary>
        /// Protocol used to establish this connection. 
        /// </summary>
        public uint Protocol;

        /// <summary>
        /// Algorithm identifier (ALG_ID) for the bulk encryption cipher used by this connection
        /// </summary>
        public uint Cipher;

        /// <summary>
        /// Strength of the bulk encryption cipher, in bits. Can be one of the following values: 0, 40, 56, 128, 168,
        /// or 256.
        /// </summary>
        public uint CipherStrength;

        /// <summary>
        /// ALG_ID indicating the hash used for generating Message Authentication Codes (MACs). 
        /// </summary>
        public uint Hash;

        /// <summary>
        /// Strength of the hash, in bits: 128 or 160.
        /// </summary>
        public uint HashStrength;

        /// <summary>
        /// ALG_ID indicating the key exchange algorithm used to generate the shared master secret
        /// </summary>
        public uint Exch;

        /// <summary>
        /// Strength of the key exchange, in bits. Typically, this member contains one of the following values: 512,
        /// 768, 1024, or 2048.
        /// </summary>
        public uint ExchStrength;
    }

    /// <summary>
    /// The SecPkgContext_EapKeyBlock structure contains key data used by the EAP TLS Authentication Protocol. For 
    /// information about the EAP TLS Authentication Protocol, see http://www.ietf.org/rfc/rfc2716.txt.
    /// http://msdn.microsoft.com/en-us/library/aa379822(v=VS.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SecurityPackageContextEapKeyBlock
    {
        /// <summary>
        /// An array of 128 bytes that contain key data used by the EAP TLS Authentication Protocol.
        /// </summary>
        public byte[] rgbKeys;

        /// <summary>
        /// An array of 64 bytes that contain initialization vector data used by the EAP TLS Authentication Protocol.
        /// </summary>
        public byte[] rgbIVs;
    }

    /// <summary>
    /// The SecPkgContext_SessionAppData structure stores application data for a session context.
    /// This attribute is supported only by the Schannel security support provider (SSP).
    /// http://msdn.microsoft.com/en-us/library/aa380095(v=VS.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct SecurityPackageContextSessionAppData
    {
        /// <summary>
        /// Reserved for future use.
        /// </summary>
        internal uint dwFlag;

        /// <summary>
        /// Count of bytes used by pbAppData.
        /// </summary>
        internal uint cbAppData;

        /// <summary>
        /// Pointer to a BYTE that represents the session application data.
        /// </summary>
        internal IntPtr pbAppData;

        /// <summary>
        /// Get byte array of AppData.
        /// </summary>
        /// <returns>byte array of app data</returns>
        [SecurityPermission(SecurityAction.Demand)]
        internal byte[] GetAppData()
        {
            byte[] appData = null;

            if (this.cbAppData > 0)
            {
                appData = new byte[this.cbAppData];
                Marshal.Copy(this.pbAppData, appData, 0, appData.Length);
            }
            return appData;
        }
    }

    /// <summary>
    /// The LSA_UNICODE_STRING structure is used by various Local Security Authority (LSA) functions
    /// to specify a Unicode string.<para/>
    /// http://msdn.microsoft.com/en-us/library/ms721841%28v=VS.85%29.aspx
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable")]
    [StructLayout(LayoutKind.Sequential)]
    internal struct LsaUnicodeString
    {
        /// <summary>
        /// Specifies the length, in bytes, of the string pointed to by the Buffer member, not including
        /// the terminating null character, if any.
        /// </summary>
        internal ushort Length;

        /// <summary>
        /// Specifies the total size, in bytes, of the memory allocated for Buffer. Up to MaximumLength
        /// bytes can be written into the buffer without trampling memory.
        /// </summary>
        internal ushort MaximumLength;

        /// <summary>
        /// Pointer to a wide character string. Note that the strings returned by the various LSA functions
        /// might not be null terminated.
        /// </summary>
        internal IntPtr Buffer;
    }

    /// <summary>
    /// The LSA_OBJECT_ATTRIBUTES structure is used with the LsaOpenPolicy function to specify the attributes
    /// of the connection to the Policy object.<para/>
    /// http://msdn.microsoft.com/en-us/library/ms721829%28v=VS.85%29.aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct LsaObjectAttributes
    {
        /// <summary>
        /// Specifies the size, in bytes, of the LSA_OBJECT_ATTRIBUTES structure.
        /// </summary>
        internal ulong Length;

        /// <summary>
        /// Should be NULL.
        /// </summary>
        internal IntPtr RootDirectory;

        /// <summary>
        /// Should be NULL.
        /// </summary>
        internal LsaUnicodeString ObjectName;

        /// <summary>
        /// Should be zero.
        /// </summary>
        internal int Attributes;

        /// <summary>
        /// Should be NULL.
        /// </summary>
        internal IntPtr SecurityDescriptor;

        /// <summary>
        /// Should be NULL.
        /// </summary>
        internal IntPtr SecurityQualityOfService;
    }

    /// <summary>
    /// The ACCESS_MASK data type is a double word value that defines standard,
    /// specific, and generic rights. These rights are used in access control entries
    /// (ACEs) and are the primary means of specifying the requested or granted access to an object.
    /// http://msdn.microsoft.com/en-us/library/ms721916%28VS.85%29.aspx
    /// </summary>
    internal enum AccessMask : int
    {
        /// <summary>
        /// This access type is needed to view sensitive information, such as the names of
        /// accounts established for trusted domain relationships.
        /// </summary>
        POLICY_GET_PRIVATE_INFORMATION = 0x00000004,
    }
}
