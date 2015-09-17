// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Sspi
{
    /// <summary>
    /// Enums defined by SSP
    /// </summary>
    public enum SecurityPackageType
    {
        /// <summary>
        /// Unknow security package.
        /// </summary>
        Unknown,

        ///<summary>
        ///Microsoft Negotiate is a security support provider (SSP) that acts as an application layer between SSPI and the other SSPs. 
        ///When an application calls into SSPI to log on to a network, it can specify an SSP to process the request. 
        ///If the application specifies Negotiate, Negotiate analyzes the request and picks the best SSP to handle the request 
        ///based on customer-configured security policy.
        ///</summary>
        Negotiate,

        ///<summary>
        ///The Kerberos protocol defines how clients interact with a network authentication service. 
        ///Clients obtain tickets from the Kerberos Key Distribution Center (KDC), 
        ///and they present these tickets to servers when connections are established.
        ///</summary>
        Kerberos,

        ///<summary>
        ///Windows NT Challenge/Response (NTLM) is the authentication protocol used on networks 
        ///that include systems running the Windows NT operating system and on stand-alone systems. 
        ///NTLM stands for Windows NT LAN Manager, a name chosen to distinguish this more advanced 
        ///challenge/response-based protocol from its weaker predecessor LAN Manager (LM). 
        ///</summary>
        Ntlm,

        ///<summary>
        ///Secure Channel, also known as Schannel, is a security support provider (SSP) 
        ///that contains a set of security protocols that provide identity authentication and secure, 
        ///private communication through encryption.
        ///</summary>
        Schannel,

        ///<summary>
        ///Credential Security Support Provider, The Credential Security Support Provider (CredSSP) Protocol 
        ///(see [MS-CSSP]) is essentially the amalgamation of TLS with Kerberos and NT LAN Manager (NTLM). 
        ///Besides enabling authentication of the remote computer’s identity, the Credential Security Support 
        ///Provider (CredSSP) Protocol also facilitates user authentication and the transfer of user credentials 
        ///from client to server, hence enabling single-sign-on scenarios
        ///</summary>
        CredSsp,

        /// <summary>
        /// NetLogon
        /// </summary>
        NetLogon,
    }

    /// <summary>
    /// Bit flags that indicate requests for the context. Not all packages can support all requirements. Flags used 
    /// for this parameter are prefixed with ISC_REQ_, for example, ISC_REQ_DELEGATE. 
    /// It is used in ClientSecurityContext class.
    /// Reference: http://msdn.microsoft.com/en-us/library/aa375506(VS.85).aspx
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), Flags]
    public enum ClientSecurityContextAttribute : uint
    {
        /// <summary>
        /// No bits to be set.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// The server can use the context to authenticate to other servers as the client. The ISC_REQ_MUTUAL_AUTH 
        /// flag must be set for this flag to work. Valid for Kerberos. Ignore this flag for constrained delegation.
        /// </summary>
        Delegate = 0x00000001,

        /// <summary>
        /// The mutual authentication policy of the service will be satisfied.
        /// Caution  This does not necessarily mean that mutual authentication is performed, only that the 
        /// authentication policy of the service is satisfied. To ensure that mutual authentication is performed,
        /// call the QueryContextAttributes (General) function.
        /// </summary>
        MutualAuth = 0x00000002,

        /// <summary>
        /// Detect replayed messages that have been encoded by using the EncryptMessage or MakeSignature functions.
        /// </summary>
        ReplayDetect = 0x00000004,

        /// <summary>
        ///Detect messages received out of sequence.
        /// </summary>
        SequenceDetect = 0x00000008,

        /// <summary>
        /// Encrypt messages by using the EncryptMessage function.
        /// </summary>
        Confidentiality = 0x00000010,

        /// <summary>
        /// A new session key must be negotiated.
        /// This value is supported only by the Kerberos security package.
        /// </summary>
        UseSessionKey = 0x00000020,

        /// <summary>
        /// Schannel must not attempt to supply credentials for the client automatically.
        /// </summary>
        UseSuppliedCreds = 0x00000080,

        /// <summary>
        /// The security package must allocate memory. The caller must eventually call the FreeContextBuffer function
        /// to free memory allocated by the security package.
        /// </summary>
        AllocMemory = 0x00000100,

        /// <summary>
        ///  This flag is added for use with Microsoft's implementation of Distributed Computing Environment.
        /// </summary>
        DceStyle = 0x00000200,

        /// <summary>
        /// A datagram-type communications channel should be used.
        /// </summary>
        Datagram = 0x00000400,

        /// <summary>
        /// The security context will not handle formatting messages. This value is the default for the Kerberos, 
        /// Negotiate, and NTLM security packages.
        /// </summary>
        Connection = 0x00000800,

        /// <summary>
        /// When errors occur, the remote party will be notified.
        /// </summary>
        ExtendedError = 0x00004000,

        /// <summary>
        /// Support a stream-oriented connection.
        /// </summary>
        Stream = 0x00008000,

        /// <summary>
        /// Sign messages and verify signatures by using the EncryptMessage and MakeSignature functions.
        /// </summary>
        Integrity = 0x00010000,

        /// <summary>
        /// This flag allows the client to indicate to the server that it should only allow the server application
        /// to identify the client by name and ID, but not to impersonate the client.
        /// </summary>
        Identify = 0x00020000,

        /// <summary>
        /// Schannel must not authenticate the server automatically
        /// </summary>
        MutualCredValidataion = 0x00080000,
    }

    /// <summary>
    /// Bit flags that specify the attributes required by the server to establish the context. Bit flags can be 
    /// combined by using bitwise-OR operations.
    /// It is used in ServerSecurityContext class.
    /// Reference: http://msdn.microsoft.com/en-us/library/aa374703(VS.85).aspx
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), Flags]
    public enum ServerSecurityContextAttribute : uint
    {
        /// <summary>
        /// No bits to be set.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// The server is allowed to impersonate the client. Valid for Kerberos. Ignore this flag for constrained 
        /// delegation.
        /// </summary>
        Delegate = 0x00000001,

        /// <summary>
        /// The client is required to supply a certificate to be used for client authentication
        /// </summary>
        MutualAuth = 0x00000002,

        /// <summary>
        /// Detect replayed packets.
        /// </summary>
        ReplayDetect = 0x00000004,

        /// <summary>
        /// Detect messages received out of sequence.
        /// </summary>
        SequenceDetect = 0x00000008,

        /// <summary>
        /// Encrypt and decrypt messages. 
        /// The Digest SSP supports this flag for SASL only.
        /// </summary>
        Confidentiality = 0x00000010,

        /// <summary>
        /// Digest and Schannel will allocate output buffers for you. When you have finished using the output buffers,
        /// free them by calling the FreeContextBuffer function.
        /// </summary>
        AllocMemory = 0x00000100,

        /// <summary>
        /// This flag is added for use with Microsoft's implementation of Distributed Computing Environment.
        /// </summary>
        DceStyle = 0x00000200,

        /// <summary>
        /// A datagram-type communications channel should be used.
        /// </summary>
        Datagram = 0x00000400,

        /// <summary>
        /// The security context will not handle formatting messages.
        /// </summary>
        Connection = 0x00000800,

        /// <summary>
        /// When errors occur, the remote party will be notified.
        /// </summary>
        ExtendedError = 0x00008000,

        /// <summary>
        /// This flag is supported only by Schannel.
        /// </summary>
        Stream = 0x00010000,

        /// <summary>
        /// Sign messages and verify signatures.
        /// Schannel does not support this flag.
        /// </summary>
        Integrity = 0x00020000,
        
        /// <summary>
        /// This flag allows the client to indicate to the server that it should only allow the server application
        /// to identify the client by name and ID, but not to impersonate the client.
        /// </summary>
        Identify = 0x00080000,

        /// <summary>
        /// Use Digest for HTTP. Omit this flag to use Digest as an SASL mechanism.
        /// </summary>
        Http = 0x10000000,
    }

    /// <summary>
    /// The data representation, such as byte ordering, on the target. 
    /// This parameter can be either SECURITY_NATIVE_DREP or SECURITY_NETWORK_DREP.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), 
    SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
    public enum SecurityTargetDataRepresentation : uint
    {
        /// <summary>
        /// SECURITY_NETWORK_DREP
        /// </summary>
        SecurityNetworkDrep = 0x00000000,

        /// <summary>
        /// SECURITY_NATIVE_DREP
        /// </summary>
        SecurityNativeDrep = 0x00000010,
    }

    /// <summary>
    /// Bit flags that indicate the type of buffer.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum SecurityBufferType : uint
    {
        /// <summary>
        /// This is a placeholder in the buffer array. The caller can supply several such entries in the array, 
        /// and the security package can return information in them.
        /// </summary>
        Empty = 0x0,

        /// <summary>
        /// This buffer type is used for common data. The security package can read and write this data
        /// </summary>
        Data = 0x1,

        /// <summary>
        /// This buffer type is used to indicate the security token portion of the message. This is read-only 
        /// for input parameters or read/write for output parameters.
        /// </summary>
        Token = 0x2,

        /// <summary>
        /// These are transport-to-package–specific parameters. 
        /// </summary>
        PkgParams = 0x3,

        /// <summary>
        /// The security package uses this value to indicate the number of missing bytes in a particular message. 
        /// The pvBuffer member is ignored in this type.
        /// </summary>
        Missing = 0x4,
        
        /// <summary>
        /// The security package uses this value to indicate the number of extra or unprocessed bytes in a message.
        /// </summary>
        Extra = 0x5,

        /// <summary>
        /// This buffer type indicates a protocol-specific trailer for a particular record. It is not usually of 
        /// interest to callers.
        /// </summary>
        StreamTrailer = 0x6,

        /// <summary>
        /// This buffer type indicates a protocol-specific header for a particular record. It is not usually of 
        /// interest to callers.
        /// </summary>
        StreamHeader = 0x7,

        /// <summary>
        /// non-data padding
        /// </summary>
        Padding = 0x9,

        /// <summary>
        /// This buffer type indicates a protocol-specific list of object identifiers (OIDs). It is not usually 
        /// of interest to callers.
        /// </summary>
        Mechlist = 0xB,

        /// <summary>
        /// The buffer contains a signature of a SECBUFFER_MECHLIST buffer. It is not usually of interest to callers
        /// </summary>
        MechlistSignature = 0xC,

        /// <summary>
        /// The buffer contains channel binding information
        /// </summary>
        ChannelBindings = 0xE,

        /// <summary>
        /// The buffer contains a DOMAIN_PASSWORD_INFORMATION structure
        /// </summary>
        ChangePassResponse = 0xF,

        /// <summary>
        /// The buffer contains an alert message.
        /// </summary>
        Alert = 0x11,

        /// <summary>
        /// The buffer is read-only with a checksum.
        /// </summary>
        ReadOnlyWithChecksum = 0x10000000,

        /// <summary>
        /// The buffer is read-only with no checksum. This flag is intended for sending header information to the
        /// security package for computing the checksum. The package can read this buffer, but cannot modify it.
        /// </summary>
        ReadOnly = 0x80000000,

        /// <summary>
        /// The buffer contains a bitmask for a SECBUFFER_READONLY_WITH_CHECKSUM buffer.
        /// </summary>
        AttrMask = 0xF0000000,
    }

    
    /// <summary>
    /// FQOP parameter type of EncryptMessage method
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum SECQOP_WRAP : uint
    {
        /// <summary>
        /// When using the Digest SSP, this parameter must be set to zero
        /// </summary>
        NONE = 0x000000000,

        /// <summary>
        /// Produce a header or trailer but do not encrypt the message.
        /// </summary>
        SECQOP_WRAP_NO_ENCRYPT = 0x80000001,

        /// <summary>
        /// Send an Schannel alert message. In this case, the pMessage parameter must contain a standard two-byte
        /// SSL/TLS event code. This value is supported only by the Schannel SSP.
        /// </summary>
        SECQOP_WRAP_OOB_DATA = 0x40000000
    }
}
