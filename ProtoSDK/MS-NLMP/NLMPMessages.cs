// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp
{
    #region Structures from TD

    /// <summary>
    /// the common header of all nlmp packet
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NLMP_HEADER
    {
        /// <summary>
        /// An 8-byte character array that MUST contain the ASCII string 
        /// ('N', 'T', 'L', 'M', 'S', 'S', 'P', '\0').
        /// </summary>
        public byte[] Signature;

        /// <summary>
        /// A 32-bit unsigned integer that indicates the message type. 
        /// This field MUST be set to 0x00000001.
        /// </summary>
        public MessageType_Values MessageType;
    }

    /// <summary>
    /// A 32-bit unsigned integer that indicates the message type. This field MUST be set to 0x00000001.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1008:EnumsShouldHaveZeroValue"
        )]
    [SuppressMessage(
        "Microsoft.Design",
        "CA1028:EnumStorageShouldBeInt32"
        )]
    public enum MessageType_Values : uint
    {
        /// <summary>
        /// NEGOTIATE_MESSAGE
        /// </summary>
        NEGOTIATE = 0x00000001,

        /// <summary>
        /// CHALLENGE_MESSAGE
        /// </summary>
        CHALLENGE = 0x00000002,

        /// <summary>
        /// AUTHENTICATE_MESSAGE
        /// </summary>
        AUTHENTICATE = 0x00000003,
    }

    /// <summary>
    /// the fields of message, which contains len, maxlen and offset.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MESSAGE_FIELDS
    {
        /// <summary>
        /// A 16-bit unsigned integer that defines the size, in bytes, of Field in Payload.
        /// </summary>
        public ushort Len;

        /// <summary>
        /// A 16-bit unsigned integer that SHOULD be set to the value of Len and MUST be ignored on receipt.
        /// </summary>
        public ushort MaxLen;

        /// <summary>
        /// A 32-bit unsigned integer that defines the offset, in bytes, from the beginning of the MESSAGE to Field 
        /// in Payload.
        /// </summary>
        public uint BufferOffset;
    }

    /// <summary>
    /// 2.2.1.1 NEGOTIATE_MESSAGE The NEGOTIATE_MESSAGE defines an NTLM Negotiate message that is sent from the client
    /// to the server. This message allows the client to specify its supported NTLM options to the server.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NEGOTIATE_MESSAGE
    {
        /// <summary>
        /// A NEGOTIATE structure that contains a set of bit flags, as defined in section 2.2.2.5. The client sets 
        /// flags to indicate options it 
        /// supports.
        /// </summary>
        public NegotiateTypes NegotiateFlags;

        /// <summary>
        /// If the NTLMSSP_NEGOTIATE_OEM_DOMAIN_SUPPLIED flag is not set in NegotiateFlags, indicating that no 
        /// DomainName is supplied in Payload.
        /// </summary>
        public MESSAGE_FIELDS DomainNameFields;

        /// <summary>
        /// If the NTLMSSP_NEGOTIATE_OEM_WORKSTATION_SUPPLIED flag is not set in NegotiateFlags, indicating that no 
        /// WorkstationName is supplied in Payload.
        /// </summary>
        public MESSAGE_FIELDS WorkstationFields;

        /// <summary>
        /// A VERSION structure (as defined in section 2.2.2.10) that is present only when the 
        /// NTLMSSP_NEGOTIATE_VERSION flag is set in the NegotiateFlags field. This structure is used for debugging 
        /// purposes only. In normal (non-debugging) protocol messages, it is ignored and does not affect the NTLM 
        /// message processing.
        /// </summary>
        public VERSION Version;

        /// <summary>
        /// If DomainNameLen does not equal 0x0000, DomainName MUST be a byte-array that contains the name of the 
        /// client authentication domain that MUST be encoded using the OEM character set. Otherwise, this data is 
        /// not present.
        /// </summary>
        public byte[] DomainName;

        /// <summary>
        /// If WorkstationLen does not equal 0x0000, WorkstationName MUST be a byte array that contains the name of 
        /// the client machine that MUST be encoded using the OEM character set. Otherwise, this data is not present.
        /// </summary>
        public byte[] WorkstationName;
    }

    /// <summary>
    /// 2.2.1.2 CHALLENGE_MESSAGE The CHALLENGE_MESSAGE defines an NTLM challenge message that is sent from the server
    /// to the client. The CHALLENGE_MESSAGE is used by the server to challenge the client to prove its identity. For
    /// connection-oriented requests, the CHALLENGE_MESSAGE generated by the server is in response to the 
    /// NEGOTIATE_MESSAGE (section 2.2.1.1) from the client.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CHALLENGE_MESSAGE
    {
        /// <summary>
        /// If the NTLMSSP_REQUEST_TARGET flag is not set in NegotiateFlags, indicating that no TargetName is required
        /// </summary>
        public MESSAGE_FIELDS TargetNameFields;

        /// <summary>
        /// A NEGOTIATE structure that contains a set of bit flags, as defined by section 2.2.2.5. The server sets 
        /// flags to indicate options it supports or, if there has been a NEGOTIATE_MESSAGE (section 2.2.1.1), the 
        /// choices it has made from the options offered by the client.
        /// </summary>
        public NegotiateTypes NegotiateFlags;

        /// <summary>
        /// A 64-bit value that contains the NTLM challenge. The challenge is a 64-bit nonce. The processing of the 
        /// ServerChallenge is specified in sections 3.1.5 and 3.2.5.
        /// </summary>
        public ulong ServerChallenge;

        /// <summary>
        /// An 8-byte array whose elements MUST be 0x00 and MUST be ignored on receipt.
        /// </summary>
        public ulong Reserved;

        /// <summary>
        /// If the NTLMSSP_NEGOTIATE_TARGET_INFO flag of NegotiateFlags is clear,indicating that no TargetInfo is 
        /// required
        /// </summary>
        public MESSAGE_FIELDS TargetInfoFields;

        /// <summary>
        /// A VERSION structure (as defined in section 2.2.2.10) that is present only when the 
        /// NTLMSSP_NEGOTIATE_VERSION flag is set in the NegotiateFlags field. This structure is used for debugging 
        /// purposes only. In normal (non-debugging) protocol messages, it is ignored and does not affect the NTLM 
        /// message processing.
        /// </summary>
        public VERSION Version;

        /// <summary>
        /// If TargetNameLen does not equal 0x0000, TargetName MUST be a byte array that contains the name of the 
        /// server authentication realm, and MUST be expressed in the negotiated character set. A server that is a 
        /// member of a domain returns the domain of which it is a member, and a server that is not a member of a 
        /// domain returns the server name.
        /// </summary>
        public byte[] TargetName;

        /// <summary>
        /// If TargetInfoLen does not equal 0x0000, TargetInfo MUST be a byte array that contains a sequence of 
        /// AV_PAIR structures. The AV_PAIR structure is defined in section 2.2.2.1. The length of each AV_PAIR is 
        /// determined by its AvLen field (plus 4 bytes).
        /// </summary>
        public byte[] TargetInfo;
    }

    /// <summary>
    /// 2.2.1.3 AUTHENTICATE_MESSAGE The AUTHENTICATE_MESSAGE defines an NTLM authenticate message that is sent from 
    /// the client to the server after the CHALLENGE_MESSAGE (section 2.2.1.2) is processed by the client.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AUTHENTICATE_MESSAGE
    {
        /// <summary>
        /// If the client chooses not to send an LmChallengeResponse to the server.
        /// </summary>
        public MESSAGE_FIELDS LmChallengeResponseFields;

        /// <summary>
        /// If the client chooses not to send an NtChallengeResponse to the server.
        /// </summary>
        public MESSAGE_FIELDS NtChallengeResponseFields;

        /// <summary>
        /// If the client chooses not to send a DomainName to the server.
        /// </summary>
        public MESSAGE_FIELDS DomainNameFields;

        /// <summary>
        /// If the client chooses not to send a UserName to the server.
        /// </summary>
        public MESSAGE_FIELDS UserNameFields;

        /// <summary>
        /// If the client chooses not to send Workstation to the server.
        /// </summary>
        public MESSAGE_FIELDS WorkstationFields;

        /// <summary>
        /// If the NTLMSSP_NEGOTIATE_KEY_EXCH flag is not set in NegotiateFlags, indicating that no 
        /// EncryptedRandomSessionKey is supplied.
        /// </summary>
        public MESSAGE_FIELDS EncryptedRandomSessionKeyFields;

        /// <summary>
        /// In connectionless mode, a NEGOTIATE structure that contains a set of bit flags (section 2.2.2.5) and 
        /// represents the conclusion of negotiation—the choices the client has made from the options the server 
        /// offered in the CHALLENGE_MESSAGE. In connection-oriented mode, a NEGOTIATE structure that contains the set
        /// of bit flags (section 2.2.2.5) negotiated in the previous messages.
        /// </summary>
        public NegotiateTypes NegotiateFlags;

        /// <summary>
        /// A VERSION structure (section 2.2.2.10) that is present only when the NTLMSSP_NEGOTIATE_VERSION flag is set
        /// in the NegotiateFlags field. This structure is used for debugging purposes only. In normal protocol 
        /// messages, it is ignored and does not affect the NTLM message processing.
        /// </summary>
        public VERSION Version;

        /// <summary>
        /// A MIC SHOULD be present when a CHALLENGE_MESSAGE TargetInfo field (section 2.2.1.2) has a MsvAvTimestamp 
        /// present. The Client MUST set the AV_PAIR structure (section 2.2.2.1) AvId field to MsvAvFlags and the 
        /// Value field bit 0x2 to 1 in the AUTHENTICATE_MESSAGE from the client when providing a MIC.
        /// </summary>
        public byte[] MIC;

        /// <summary>
        /// The domain or computer name hosting the user account. DomainName MUST be encoded in the negotiated 
        /// character set.
        /// </summary>
        public byte[] DomainName;

        /// <summary>
        /// The name of the user to be authenticated. UserName MUST be encoded in the negotiated character set.
        /// </summary>
        public byte[] UserName;

        /// <summary>
        /// The name of the computer to which the user is logged on. Workstation MUST be encoded in the negotiated 
        /// character set.
        /// </summary>
        public byte[] Workstation;

        /// <summary>
        /// An LM_RESPONSE or LMv2_RESPONSE structure that contains the computed LM response to the challenge. If 
        /// NTLM v2 authentication is configured, LmChallengeResponse MUST be an LMv2_RESPONSE structure (section 
        /// 2.2.2.4). Otherwise, it MUST be an LM_RESPONSE structure (section 2.2.2.3).
        /// </summary>
        public byte[] LmChallengeResponse;

        /// <summary>
        /// An NTLM_RESPONSE or NTLMv2_RESPONSE structure that contains the computed NT response to the challenge.
        /// If NTLM v2 authentication is configured,NtChallengeResponse MUST be an NTLMv2_RESPONSE (section 2.2.2.8). 
        /// Otherwise, it MUST be an NTLM_RESPONSE structure (section 2.2.2.6).
        /// </summary>
        public byte[] NtChallengeResponse;

        /// <summary>
        /// The client's encrypted random session key. EncryptedRandomSessionKey and its usage are defined in sections
        /// 3.1.5 and 3.2.5.
        /// </summary>
        public byte[] EncryptedRandomSessionKey;
    }

    /// <summary>
    /// 2.2.2.1 AV_PAIR The AV_PAIR structure defines an attribute/value pair. Sequences of AV_PAIR structures are
    /// used in the CHALLENGE_MESSAGE and AUTHENTICATE_MESSAGE messages.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AV_PAIR
    {
        /// <summary>
        /// A 16-bit unsigned integer that defines the information type in the Value field. The contents of this field
        /// MUST be one of the values from the following table. The corresponding Value field in this AV_PAIR MUST 
        /// contain the information specified in the description of that AvId.
        /// </summary>
        public AV_PAIR_IDs AvId;

        /// <summary>
        /// A 16-bit unsigned integer that defines the length, in bytes, of Value.
        /// </summary>
        public ushort AvLen;

        /// <summary>
        /// A variable-length byte-array that contains the value defined for this AV pair entry. The contents of this 
        /// field depend on the type expressed in the AvId field. The available types and resulting format and
        /// contents of this field are specified in the table within the AvId field description in this topic.
        /// </summary>
        public byte[] Value;
    }

    /// <summary>
    /// The corresponding Value field in this AV_PAIR MUST contain the information specified in the description of 
    /// that AvId.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1028:EnumStorageShouldBeInt32"
        )]
    public enum AV_PAIR_IDs : ushort
    {
        /// <summary>
        /// Indicates that this is the last AV_PAIR in the list. AvLen MUST be 0. This type of information MUST be 
        /// present in the AV pair list.
        /// </summary>
        MsvAvEOL = 0,

        /// <summary>
        /// The server's NetBIOS computer name. The name MUST be in Unicode, and is not null-terminated. This type of 
        /// information MUST be present in the 
        /// AV_pair list.
        /// </summary>
        MsvAvNbComputerName = 1,

        /// <summary>
        /// The server's NetBIOS domain name. The name MUST be in Unicode, and is not null-terminated. This type of 
        /// information MUST be present in the AV_pair list.
        /// </summary>
        MsvAvNbDomainName = 2,

        /// <summary>
        /// The fully qualified domain name (FQDN (1)) of the computer. The name MUST be in Unicode, and is not 
        /// null-terminated.
        /// </summary>
        MsvAvDnsComputerName = 3,

        /// <summary>
        /// The FQDN (2) of the domain. The name MUST be in Unicode, and is not null-terminated.
        /// </summary>
        MsvAvDnsDomainName = 4,

        /// <summary>
        /// The FQDN (2) of the forest. The name MUST be in Unicode, and is not null-terminated.
        /// </summary>
        MsvAvDnsTreeName = 5,

        /// <summary>
        /// A 32-bit value indicating server or client configuration. 0x00000001: indicates to the client that the
        /// account authentication is constrained.0x00000002: indicates that the client is providing message integrity
        /// in the MIC field (section 2.2.1.3) in the AUTHENTICATE_MESSAGE.
        /// </summary>
        MsvAvFlags = 6,

        /// <summary>
        /// A FILETIME structure ([MS-DTYP] section 2.3.1) in little-endian byte order that contains the server local 
        /// time.
        /// </summary>
        MsvAvTimestamp = 7,

        /// <summary>
        /// A Restriction_Encoding structure (section 2.2.2.2). The Value field contains a structure representing the 
        /// integrity level of the security principal, as well as a MachineID created at computer startup to identify
        /// the calling machine. 
        /// </summary>
        MsAvRestrictions = 8,

        /// <summary>
        /// The SPN of the target server. The name MUST be in Unicode and is not null-terminated. 
        /// </summary>
        MsvAvTargetName = 9,

        /// <summary>
        /// A channel bindings hash. The Value field contains an MD5 hash ([RFC4121] section 4.1.1.2) of a 
        /// gss_channel_bindings_struct ([RFC2744] section 3.11).An all-zero value of the hash is used to indicate 
        /// absence of channel bindings.
        /// </summary>
        MsvChannelBindings = 10,
    }

    /// <summary>
    /// 2.2.2.2 Restriction_Encoding The Restriction_Encoding structure defines in NTLM allow platform-specific 
    /// restrictions to be encoded within an authentication exchange. The client produces additional restrictions to
    /// be applied to the server when authorization decisions are to be made. If the server does not support the
    /// restrictions, then the client's authorization on the server is unchanged.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Restriction_Encoding
    {
        /// <summary>
        /// A 32-bit unsigned integer that defines the length, in bytes, of AV_PAIR Value.
        /// </summary>
        public uint Size;

        /// <summary>
        /// A 32-bit integer value containing 0x00000000.
        /// </summary>
        public uint Z4;

        /// <summary>
        /// Indicates an integrity level is present in SubjectIntegrityLevel.
        /// </summary>
        public uint IntegrityLevel;

        /// <summary>
        /// A 32-bit integer value indicating an integrity level of the client.
        /// </summary>
        public uint SubjectIntegrityLevel;

        /// <summary>
        /// A 256-bit random number created at computer startup to identify the calling machine.
        /// </summary>
        public byte[] MachineID;
    }

    /// <summary>
    /// 2.2.2.3 LM_RESPONSE The LM_RESPONSE structure defines the NTLM v1 authentication LmChallengeResponse in the 
    /// AUTHENTICATE_MESSAGE. This response is used only when NTLM v1 authentication is configured.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LM_RESPONSE
    {
        /// <summary>
        /// A 24-byte array of unsigned char that contains the client's LmChallengeResponse as defined in section 
        /// 3.3.1.
        /// </summary>
        public byte[] Response;
    }

    /// <summary>
    /// 2.2.2.4 LMv2_RESPONSE The LMv2_RESPONSE structure defines the NTLM v2 authentication LmChallengeResponse in 
    /// the AUTHENTICATE_MESSAGE. This response is used only when NTLM v2 authentication is configured.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LMv2_RESPONSE
    {
        /// <summary>
        /// A 16-byte array of unsigned char that contains the client's LM challenge-response. This is the portion of 
        /// the LmChallengeResponse field to which the HMAC_MD5 algorithm has been applied, as defined in section 
        /// 3.3.2. Specifically, Response corresponds to the result of applying the HMAC_MD5 algorithm, using the key
        /// ResponseKeyLM, to a message consisting of the concatenation of the ResponseKeyLM, ServerChallenge and 
        /// ClientChallenge.
        /// </summary>
        public byte[] Response;

        /// <summary>
        /// An 8-byte array of unsigned char that contains the client's ClientChallenge, as defined in section 
        /// 3.1.5.1.2.
        /// </summary>
        public ulong ChallengeFromClient;
    }

    /// <summary>
    /// 2.2.2.5 NEGOTIATE During NTLM authentication, each of the following flags is a possible value of the 
    /// NegotiateFlags field of the NEGOTIATE_MESSAGE, CHALLENGE_MESSAGE, and AUTHENTICATE_MESSAGE, unless otherwise 
    /// noted. These flags define client or server NTLM capabilities supported by the sender.
    /// </summary>
    [Flags]
    [SuppressMessage(
        "Microsoft.Design",
        "CA1028:EnumStorageShouldBeInt32"
        )]
    public enum NegotiateTypes : uint
    {
        /// <summary>
        /// If set, requests 56-bit encryption. If the client sends NTLMSSP_NEGOTIATE_56 to the server in the 
        /// NEGOTIATE_MESSAGE, the server MUST return NTLMSSP_NEGOTIATE_56 to the client in the CHALLENGE_MESSAGE only
        /// if the client sets NTLMSSP_NEGOTIATE_SEAL or NTLMSSP_NEGOTIATE_SIGN. Otherwise it is ignored. If both 
        /// NTLMSSP_NEGOTIATE_56 and NTLMSSP_NEGOTIATE_128 are requested and supported by the client and server, 
        /// NTLMSSP_NEGOTIATE_56 and NTLMSSP_NEGOTIATE_128 will both be returned to the client. Clients and servers 
        /// that set NTLMSSP_NEGOTIATE_SEAL SHOULD set NTLMSSP_NEGOTIATE_56 if it is supported. An alternate name for
        /// this field is NTLMSSP_NEGOTIATE_56.
        /// </summary>
        NTLMSSP_NEGOTIATE_56 = 0x80000000,

        /// <summary>
        /// If set, requests an explicit key exchange. If the client sends NTLMSSP_NEGOTIATE_KEY_EXCH to the server 
        /// in the NEGOTIATE_MESSAGE, the server MUST return NTLMSSP_NEGOTIATE_KEY_EXCH to the client in the
        /// CHALLENGE_MESSAGE and use key exchange only if the client sets NTLMSSP_NEGOTIATE_SIGN or 
        /// NTLMSSP_NEGOTIATE_SEAL. Otherwise it is ignored. This capability SHOULD be used because message integrity 
        /// or confidentiality can be provided only when this flag is negotiated and a key exchange key is created.
        /// See sections  3.1.5.1 and 3.1.5.2 for details. An alternate name for this field is 
        /// NTLMSSP_NEGOTIATE_KEY_EXCH.
        /// </summary>
        NTLMSSP_NEGOTIATE_KEY_EXCH = 0x40000000,

        /// <summary>
        /// If set, requests 128-bit session key negotiation. An alternate name for this field is 
        /// NTLMSSP_NEGOTIATE_128. If the client sends NTLMSSP_NEGOTIATE_128 to the server in the NEGOTIATE_MESSAGE,
        /// the server MUST return NTLMSSP_NEGOTIATE_128 to the client in the CHALLENGE_MESSAGE only if the client 
        /// sets NTLMSSP_NEGOTIATE_SEAL or NTLMSSP_NEGOTIATE_SIGN. Otherwise it is ignored. If both 
        /// NTLMSSP_NEGOTIATE_56 and NTLMSSP_NEGOTIATE_128 are requested and supported by the client and server, 
        /// NTLMSSP_NEGOTIATE_56 and NTLMSSP_NEGOTIATE_128 will both be returned to the client. Clients and servers 
        /// that set NTLMSSP_NEGOTIATE_SEAL SHOULD set NTLMSSP_NEGOTIATE_128 if it is supported. An alternate name
        /// for this field is NTLMSSP_NEGOTIATE_128. 
        /// </summary>
        NTLMSSP_NEGOTIATE_128 = 0x20000000,

        /// <summary>
        /// If set, requests the protocol version number. The data corresponding to this flag is provided in the 
        /// Version field of the NEGOTIATE_MESSAGE, the CHALLENGE_MESSAGE, and the AUTHENTICATE_MESSAGE. An alternate 
        /// name for this field is NTLMSSP_NEGOTIATE_VERSION.
        /// </summary>
        NTLMSSP_NEGOTIATE_VERSION = 0x02000000,

        /// <summary>
        /// If set, requests extended information about the server authentication realm to be sent as AV_PAIR in the 
        /// TargetInfo payload (section 2.2.2.7). If the client sends NTLMSSP_NEGOTIATE_TARGET_INFO to the server in 
        /// the NEGOTIATE_MESSAGE, the server MUST support the request and return NTLMSSP_NEGOTIATE_TARGET_INFO to 
        /// the client in the CHALLENGE_MESSAGE. In that case, the data corresponding to this flag is provided by the 
        /// server in the TargetInfo field of the CHALLENGE_MESSAGE. An alternate name for this field is 
        /// NTLMSSP_NEGOTIATE_TARGET_INFO.
        /// </summary>
        NTLMSSP_NEGOTIATE_TARGET_INFO = 0x00800000,

        /// <summary>
        /// If set, requests the usage of the LMOWF (section 3.3). An alternate name for this field is 
        /// NTLMSSP_REQUEST_NON_NT_SESSION_KEY.
        /// </summary>
        NTLMSSP_REQUEST_NON_NT_SESSION_KEY = 0x00400000,

        /// <summary>
        /// If set, requests an identify level token. An alternate name for this field is NTLMSSP_NEGOTIATE_IDENTIFY.
        /// </summary>
        NTLMSSP_NEGOTIATE_IDENTIFY = 0x00100000,

        /// <summary>
        /// If set, requests usage of the NTLM v2 session security. NTLM v2 session security is a misnomer because it
        /// is not NTLM v2. It is NTLM v1 using the extended session security that is also in NTLM v2. 
        /// NTLMSSP_NEGOTIATE_LM_KEY and NTLMSSP_NEGOTIATE_EXTENDED_SESSIONSECURITY are mutually exclusive. If both
        /// NTLMSSP_NEGOTIATE_EXTENDED_SESSIONSECURITY and NTLMSSP_NEGOTIATE_LM_KEY are requested, 
        /// NTLMSSP_NEGOTIATE_EXTENDED_SESSIONSECURITY alone MUST be returned to the client. NTLM v2 authentication 
        /// session key generation MUST be supported by both the client and the DC in order to be used, and extended 
        /// session security signing and sealing requires support from the client and the server in order to be used. 
        /// An alternate name for this field is NTLMSSP_NEGOTIATE_EXTENDED_SESSIONSECURITY.
        /// </summary>
        NTLMSSP_NEGOTIATE_EXTENDED_SESSIONSECURITY = 0x00080000,

        /// <summary>
        /// If set, TargetName MUST be a share name. The data corresponding to this flag is provided by the server in 
        /// the TargetName field of the CHALLENGE_MESSAGE. If set, then NTLMSSP_TARGET_TYPE_SERVER and 
        /// NTLMSSP_TARGET_TYPE_DOMAIN MUST NOT be set. This flag MUST be ignored in the NEGOTIATE_MESSAGE and the
        /// AUTHENTICATE_MESSAGE. An alternate name for this field is NTLMSSP_TARGET_TYPE_SHARE.
        /// </summary>
        NTLMSSP_TARGET_TYPE_SHARE = 0x00040000,

        /// <summary>
        /// If set, TargetName MUST be a server name. The data corresponding to this flag is provided by the server
        /// in the TargetName field of the CHALLENGE_MESSAGE. If set, then NTLMSSP_TARGET_TYPE_SHARE and 
        /// NTLMSSP_TARGET_TYPE_DOMAIN MUST NOT be set. This flag MUST be ignored in the NEGOTIATE_MESSAGE and the 
        /// AUTHENTICATE_MESSAGE. An alternate name for this field is NTLMSSP_TARGET_TYPE_SERVER.
        /// </summary>
        NTLMSSP_TARGET_TYPE_SERVER = 0x00020000,

        /// <summary>
        /// If set, TargetName MUST be a domain name. The data corresponding to this flag is provided by the server 
        /// in the TargetName field of the CHALLENGE_MESSAGE. If set, then NTLMSSP_TARGET_TYPE_SHARE and 
        /// NTLMSSP_TARGET_TYPE_SERVER MUST NOT be set. This flag MUST be ignored in the NEGOTIATE_MESSAGE and the 
        /// AUTHENTICATE_MESSAGE. An alternate name for this field is NTLMSSP_TARGET_TYPE_DOMAIN.
        /// </summary>
        NTLMSSP_TARGET_TYPE_DOMAIN = 0x00010000,

        /// <summary>
        /// If set, requests the presence of a signature block on all messages. NTLMSSP_NEGOTIATE_ALWAYS_SIGN MUST be 
        /// set in the NEGOTIATE_MESSAGE to the server and the CHALLENGE_MESSAGE to the client. 
        /// NTLMSSP_NEGOTIATE_ALWAYS_SIGN is overridden by NTLMSSP_NEGOTIATE_SIGN and NTLMSSP_NEGOTIATE_SEAL, if they 
        /// are supported. An alternate name for this field is NTLMSSP_NEGOTIATE_ALWAYS_SIGN.
        /// </summary>
        NTLMSSP_NEGOTIATE_ALWAYS_SIGN = 0x00008000,

        /// <summary>
        /// This flag indicates whether the Workstation field is present. If this flag is not set, the Workstation 
        /// field MUST be ignored. If this flag is set, the length field of the Workstation field specifies whether 
        /// the workstation name is nonempty or not. An alternate name for this field is 
        /// NTLMSSP_NEGOTIATE_OEM_WORKSTATION_SUPPLIED.
        /// </summary>
        NTLMSSP_NEGOTIATE_OEM_WORKSTATION_SUPPLIED = 0x00002000,

        /// <summary>
        /// If set, the domain name is provided (section 2.2.1.1). An alternate name for this field is 
        /// NTLMSSP_NEGOTIATE_OEM_DOMAIN_SUPPLIED.
        /// </summary>
        NTLMSSP_NEGOTIATE_OEM_DOMAIN_SUPPLIED = 0x00001000,

        /// <summary>
        /// If set, the connection SHOULD be anonymous. 
        /// </summary>
        NTLMSSP_NEGOTIATE_ANONYMOUS = 0x00000800,

        /// <summary>
        /// If set, LM authentication is not allowed and only NT authentication is used. An alternate name for this 
        /// field is NTLMSSP_NEGOTIATE_NT_ONLY.
        /// </summary>
        NTLMSSP_NEGOTIATE_NT_ONLY = 0x00000400,

        /// <summary>
        /// If set, requests usage of the NTLM v1 session security protocol. NTLMSSP_NEGOTIATE_NTLM MUST be set in 
        /// the NEGOTIATE_MESSAGE to the server and the CHALLENGE_MESSAGE to the client. An alternate name for this 
        /// field is NTLMSSP_NEGOTIATE_NTLM.
        /// </summary>
        NTLMSSP_NEGOTIATE_NTLM = 0x00000200,

        /// <summary>
        /// If set, requests LAN Manager (LM) session key computation.NTLMSSP_NEGOTIATE_LM_KEY and 
        /// NTLMSSP_NEGOTIATE_EXTENDED_SESSIONSECURITY are mutually exclusive. If both NTLMSSP_NEGOTIATE_LM_KEY and 
        /// NTLMSSP_NEGOTIATE_EXTENDED_SESSIONSECURITY are requested, NTLMSSP_NEGOTIATE_EXTENDED_SESSIONSECURITY alone
        /// MUST be returned to the client. NTLM v2 authentication session key generation MUST be supported by both 
        /// the client and the DC in order to be used, and extended session security signing and sealing requires 
        /// support from the client and the server to be used. An alternate name for this field is 
        /// NTLMSSP_NEGOTIATE_LM_KEY.
        /// </summary>
        NTLMSSP_NEGOTIATE_LM_KEY = 0x00000080,

        /// <summary>
        /// If set, requests connectionless authentication. If NTLMSSP_NEGOTIATE_DATAGRAM is set, then 
        /// NTLMSSP_NEGOTIATE_KEY_EXCH MUST always be set in the AUTHENTICATE_MESSAGE to the server and the 
        /// CHALLENGE_MESSAGE to the client. An alternate name for this field is NTLMSSP_NEGOTIATE_DATAGRAM.
        /// </summary>
        NTLMSSP_NEGOTIATE_DATAGRAM = 0x00000040,

        /// <summary>
        /// If set, requests session key negotiation for message confidentiality. If the client sends 
        /// NTLMSSP_NEGOTIATE_SEAL to the server in the NEGOTIATE_MESSAGE, the server MUST return 
        /// NTLMSSP_NEGOTIATE_SEAL to the client in the CHALLENGE_MESSAGE. Clients and servers that set 
        /// NTLMSSP_NEGOTIATE_SEAL SHOULD always set NTLMSSP_NEGOTIATE_56 and NTLMSSP_NEGOTIATE_128, if they are 
        /// supported. An alternate name for this field is NTLMSSP_NEGOTIATE_SEAL.
        /// </summary>
        NTLMSSP_NEGOTIATE_SEAL = 0x00000020,

        /// <summary>
        /// If set, requests session key negotiation for message signatures. If the client sends 
        /// NTLMSSP_NEGOTIATE_SIGN to the server in the NEGOTIATE_MESSAGE, the server MUST return 
        /// NTLMSSP_NEGOTIATE_SIGN to the client in the CHALLENGE_MESSAGE. An alternate name for this field is 
        /// NTLMSSP_NEGOTIATE_SIGN.
        /// </summary>
        NTLMSSP_NEGOTIATE_SIGN = 0x00000010,

        /// <summary>
        /// If set, a TargetName field of the CHALLENGE_MESSAGE (section 2.2.1.2) MUST be supplied. An alternate
        /// name for this field is 
        /// NTLMSSP_REQUEST_TARGET.
        /// </summary>
        NTLMSSP_REQUEST_TARGET = 0x00000004,

        /// <summary>
        /// If set, requests OEM character set encoding. An alternate name for this field is NTLM_NEGOTIATE_OEM. See 
        /// bit A for details.
        /// </summary>
        NTLM_NEGOTIATE_OEM = 0x00000002,

        /// <summary>
        /// If set, requests Unicode character set encoding. An alternate name for this field is 
        /// NTLMSSP_NEGOTIATE_UNICODE.
        /// </summary>
        NTLMSSP_NEGOTIATE_UNICODE = 0x00000001,
    }

    /// <summary>
    /// 2.2.2.6 NTLM v1 Response: NTLM_RESPONSE The NTLM_RESPONSE structure defines the NTLM v1 authentication 
    /// NtChallengeResponse in the AUTHENTICATE_MESSAGE. This response is only used when NTLM v1 authentication is 
    /// configured.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NTLMv1_RESPONSE
    {
        /// <summary>
        /// A 24-byte array of unsigned char that contains the client's NtChallengeResponse (section 3.3.1).
        /// </summary>
        public byte[] Response;
    }

    /// <summary>
    /// An 8-bit unsigned char that contains the current version of the challenge response type. This field MUST be 
    /// 0x01.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1008:EnumsShouldHaveZeroValue"
        )]
    [SuppressMessage(
        "Microsoft.Design",
        "CA1028:EnumStorageShouldBeInt32"
        )]
    public enum RespType_Values : byte
    {
        /// <summary>
        /// the only value
        /// </summary>
        V1 = 0x01,
    }

    /// <summary>
    /// An 8-bit unsigned char that contains the maximum supported version of the challenge response type. This field
    /// MUST be 0x01.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1008:EnumsShouldHaveZeroValue"
        )]
    [SuppressMessage(
        "Microsoft.Design",
        "CA1028:EnumStorageShouldBeInt32"
        )]
    public enum HiRespType_Values : byte
    {
        /// <summary>
        /// the only value
        /// </summary>
        V1 = 0x01,
    }

    /// <summary>
    /// 2.2.2.7 NTLM v2: NTLMv2_CLIENT_CHALLENGE The NTLMv2_CLIENT_CHALLENGE structure defines the client challenge 
    /// in the AUTHENTICATE_MESSAGE. This structure is used only when NTLM v2 authentication is configured.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NTLMv2_CLIENT_CHALLENGE
    {
        /// <summary>
        /// An 8-bit unsigned char that contains the current version of the challenge response type. This field MUST
        /// be 0x01.
        /// </summary>
        public RespType_Values RespType;

        /// <summary>
        /// An 8-bit unsigned char that contains the maximum supported version of the challenge response type. This
        /// field MUST be 0x01.
        /// </summary>
        public HiRespType_Values HiRespType;

        /// <summary>
        /// A 16-bit unsigned integer that SHOULD be 0x0000 and MUST be ignored on receipt.
        /// </summary>
        public ushort Reserved1;

        /// <summary>
        /// A 32-bit unsigned integer that SHOULD be 0x00000000 and MUST be ignored on receipt.
        /// </summary>
        public uint Reserved2;

        /// <summary>
        /// A 64-bit unsigned integer that contains the current system time, represented as the number of 100 
        /// nanosecond ticks elapsed since midnight of January 1, 1601 (UTC).
        /// </summary>
        public ulong TimeStamp;

        /// <summary>
        /// An 8-byte array of unsigned char that contains the client's ClientChallenge (section 3.1.5.1.2).
        /// </summary>
        public ulong ChallengeFromClient;

        /// <summary>
        /// A 32-bit unsigned integer that SHOULD be 0x00000000 and MUST be ignored on receipt.
        /// </summary>
        public uint Reserved3;

        /// <summary>
        /// A byte array that contains a sequence of AV_PAIR structures (section 2.2.2.1). The sequence contains the 
        /// server-naming context and is terminated by an AV_PAIR structure with an AvId field of MsvAvEOL.
        /// </summary>
        public byte[] AvPairs;
    }

    /// <summary>
    /// 2.2.2.8 NTLM2 V2 Response: NTLMv2_RESPONSE The NTLMv2_RESPONSE structure defines the NTLMv2 authentication 
    /// NtChallengeResponse in the AUTHENTICATE_MESSAGE. This response is used only when NTLMv2 authentication is 
    /// configured.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NTLMv2_RESPONSE
    {
        /// <summary>
        /// A 16-byte array of unsigned char that contains the client's NT challenge-response as defined in section 
        /// 3.3.2. Response corresponds to the NTProofStr variable from section 3.3.2.
        /// </summary>
        public byte[] Response;

        /// <summary>
        /// A variable-length byte array that contains the ClientChallenge as defined in section 3.3.2.
        /// ChallengeFromClient corresponds to the temp variable from section 3.3.2.
        /// </summary>
        public byte[] NTLMv2_CLIENT_CHALLENGE;
    }

    /// <summary>
    /// 2.2.2.9.1 NTLMSSP_MESSAGE_SIGNATURE This version of the NTLMSSP_MESSAGE_SIGNATURE structure MUST be used when
    /// the NTLMSSP_NEGOTIATE_EXTENDED_SESSIONSECURITY flag is not negotiated.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NTLMSSP_MESSAGE_SIGNATURE
    {
        /// <summary>
        /// A 32-bit unsigned integer that contains the signature version. This field MUST be 0x00000001.
        /// </summary>
        public SignatureVersion_Values Version;

        /// <summary>
        /// A 4-byte array that contains the random pad for the message.
        /// </summary>
        public uint RandomPad;

        /// <summary>
        /// A 4-byte array that contains the checksum for the message.
        /// </summary>
        public uint Checksum;

        /// <summary>
        /// A 32-bit unsigned integer that contains the NTLM sequence number for this application message.
        /// </summary>
        public uint SeqNum;
    }

    /// <summary>
    /// A 32-bit unsigned integer that contains the signature version. This field MUST be 0x00000001.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1008:EnumsShouldHaveZeroValue"
        )]
    [SuppressMessage(
        "Microsoft.Design",
        "CA1028:EnumStorageShouldBeInt32"
        )]
    public enum SignatureVersion_Values : uint
    {
        /// <summary>
        /// the only value
        /// </summary>
        V1 = 0x00000001,
    }

    /// <summary>
    /// 2.2.2.9.2 NTLMSSP_MESSAGE_SIGNATURE for Extended Session Security This version of the 
    /// NTLMSSP_MESSAGE_SIGNATURE structure MUST be used when the NTLMSSP_NEGOTIATE_EXTENDED_SESSIONSECURITY flag is 
    /// negotiated.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NTLMSSP_MESSAGE_SIGNATURE_Extended
    {
        /// <summary>
        /// A 32-bit unsigned integer that contains the signature version. This field MUST be 0x00000001.
        /// </summary>
        public ExtendedSignatureVersion_Values Version;

        /// <summary>
        /// An 8-byte array that contains the checksum for the message.
        /// </summary>
        public ulong Checksum;

        /// <summary>
        /// A 32-bit unsigned integer that contains the NTLM sequence number for this application message.
        /// </summary>
        public uint SeqNum;
    }

    /// <summary>
    /// A 32-bit unsigned integer that contains the signature version. This field MUST be 0x00000001.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1008:EnumsShouldHaveZeroValue"
        )]
    [SuppressMessage(
        "Microsoft.Design",
        "CA1028:EnumStorageShouldBeInt32"
        )]
    public enum ExtendedSignatureVersion_Values : uint
    {
        /// <summary>
        /// the only value
        /// </summary>
        V1 = 0x00000001,
    }

    /// <summary>
    /// 2.2.2.10 VERSION The VERSION structure contains Windows version information that SHOULD be ignored. This 
    /// structure is used for debugging purposes only and its value does not affect NTLM message processing. It 
    /// is present in the NEGOTIATE_MESSAGE, CHALLENGE_MESSAGE, and AUTHENTICATE_MESSAGE messages only if 
    /// NTLMSSP_NEGOTIATE_VERSION is negotiated.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VERSION
    {
        /// <summary>
        /// An 8-bit unsigned integer that contains the minor version number of the Windows operating system in use.
        /// This field MUST contain one of the following values:
        /// </summary>
        public VERSION_MAJOR ProductMajorVersion;

        /// <summary>
        /// An 8-bit unsigned integer that contains the minor version number of the Windows operating system in use.
        /// This field MUST contain one of the following values:
        /// </summary>
        public VERSION_MINOR ProductMinorVersion;

        /// <summary>
        /// A 16-bit unsigned integer that contains the build number of the Windows operating system in use. This 
        /// field MUST be set to any 16-bit quantity that identifies the operating system build number.
        /// </summary>
        public ushort ProductBuild;

        /// <summary>
        /// A 24-bit data area that SHOULD be set to zero and MUST be ignored by the receiver.
        /// </summary>
        public VERSION_Reserved Reserved;

        /// <summary>
        /// An 8-bit unsigned integer that contains a value indicating the current revision of the NTLMSSP in use. 
        /// This field MUST contain one of the following values:
        /// </summary>
        public VERSION_REVERSION NTLMRevisionCurrent;
    }

    /// <summary>
    /// A 24-bit data area that SHOULD be set to zero and MUST be ignored by the receiver.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VERSION_Reserved
    {
        /// <summary>
        /// Reserved1
        /// </summary>
        public byte Reserved1;

        /// <summary>
        /// Reserved2
        /// </summary>
        public byte Reserved2;

        /// <summary>
        /// Reserved3
        /// </summary>
        public byte Reserved3;
    }

    /// <summary>
    /// An 8-bit unsigned integer that contains the minor version number of the Windows operating system in use. This 
    /// field MUST contain one of the following values:
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1028:EnumStorageShouldBeInt32"
        )]
    [SuppressMessage(
        "Microsoft.Design",
        "CA1008:EnumsShouldHaveZeroValue"
        )]
    public enum VERSION_MAJOR : byte
    {
        /// <summary>
        /// The major version of the Windows operating system is 0x05.
        /// </summary>
        WINDOWS_MAJOR_VERSION_5 = 0x05,

        /// <summary>
        /// The major version of the Windows operating system is 0x06.
        /// </summary>
        WINDOWS_MAJOR_VERSION_6 = 0x06,
    }

    /// <summary>
    /// An 8-bit unsigned integer that contains the minor version number of the Windows operating system in use. This
    /// field MUST contain one of the following values:
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1028:EnumStorageShouldBeInt32"
        )]
    public enum VERSION_MINOR : byte
    {
        /// <summary>
        /// The minor version of the Windows operating system is 0x00.
        /// </summary>
        WINDOWS_MINOR_VERSION_0 = 0x00,

        /// <summary>
        /// The minor version of the Windows operating system is 0x01.
        /// </summary>
        WINDOWS_MINOR_VERSION_1 = 0x01,

        /// <summary>
        /// The minor version of the Windows operating system is 0x02.
        /// </summary>
        WINDOWS_MINOR_VERSION_2 = 0x02,

        /// <summary>
        /// The minor version of the Windows operating system is 0x03.
        /// </summary>
        WINDOWS_MINOR_VERSION_3 = 0x03,
    }

    /// <summary>
    /// An 8-bit unsigned integer that contains a value indicating the current revision of the NTLMSSP in use. This 
    /// field MUST contain one of the following values:
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1008:EnumsShouldHaveZeroValue"
        )]
    [SuppressMessage(
        "Microsoft.Design",
        "CA1028:EnumStorageShouldBeInt32"
        )]
    public enum VERSION_REVERSION : byte
    {
        /// <summary>
        /// Version 15 of the NTLMSSP is in use.
        /// </summary>
        NTLMSSP_REVISION_W2K3 = 0x0F,

        /// <summary>
        /// Version 10 of the NTLMSSP is in use.
        /// </summary>
        NTLMSSP_REVISION_W2K3_RC1 = 0x0A,
    }

    #endregion

    #region Structures from StackSdk

    /// <summary>
    /// The NTLM authentication version is not negotiated by the protocol. It must be configured on both the client 
    /// and the server prior to authentication. The version is selected by the client, and requested during the 
    /// protocol negotiation. If the server does not support the version selected by the client, authentication fails.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum NlmpVersion
    {
        /// <summary>
        /// NTLM v1 session
        /// </summary>
        v1 = 0x01,

        /// <summary>
        /// NTLM v1 session
        /// </summary>
        v2 = 0x02,
    }

    /// <summary>
    /// the information of client authenticate.<para/>
    /// this struct is used to retrieve the authenticate information from the authenticate packet of client.
    /// </summary>
    internal struct ClientAuthenticateInfomation
    {
        /// <summary>
        /// the domain name of user to authenticate
        /// </summary>
        public string DomainName;

        /// <summary>
        /// the user name of user to authenticate
        /// </summary>
        public string UserName;

        /// <summary>
        /// the server name from client. client send the server name in the authenticate packet. server name is not a
        /// string but a byte[] constrains the AV_PAIRS.
        /// </summary>
        public byte[] ServerName;

        /// <summary>
        /// the time from client. client send client to server in the authenticate packet. if client does not send the
        /// time in the authenticate packet, return the current time of server.
        /// </summary>
        public ulong ClientTime;

        /// <summary>
        /// the client challenge from client. client send the challenge to server in the authenticate packet.
        /// </summary>
        public ulong ClientChallenge;

        /// <summary>
        /// version of client using: NTLMv1 or NTLMv2.
        /// </summary>
        public NlmpVersion Version;
    }

    /// <summary>
    /// the validation information returned from domain controller
    /// in domain environment, nlmp should pass through the client authenticate packet to DC for validation 
    /// see [MS-NLMP] section 3.3.1 and 3.3.2
    /// </summary>
    public struct DcValidationInfo
    {
        /// <summary>
        /// validation status
        /// </summary>
        public NtStatus status;

        /// <summary>
        /// 
        /// </summary>
        public byte[] sessionKey;
    }

    /// <summary>
    /// Verify authenticate packet in dc delegate method
    /// </summary>
    /// <param name="nlmpAuthenticatePacket">nlmp authenticate packet</param>
    /// <param name="serverChallenge">server challenge</param>
    /// <returns>DC validation info</returns>
    public delegate DcValidationInfo VerifyAuthenticatePacketInDcMethod(
       NlmpAuthenticatePacket nlmpAuthenticatePacket,
        ulong serverChallenge);

    #endregion
}
