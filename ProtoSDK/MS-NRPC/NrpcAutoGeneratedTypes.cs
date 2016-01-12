// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Runtime.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nrpc
{
    /// <summary>
    ///  The NL_AUTH_SIGNATURE structure is a security token
    ///  that defines the authentication signature used by Netlogon
    ///  to execute Netlogon methods over a secure channel.
    ///  It follows the security trailer that a security provider
    ///  MUST associate with a signed or encrypted message.
    ///  A security trailer or sec_trailer structure ([MS-RPCE]
    ///  section) has syntax equivalent to the auth_verifier_co_t
    ///  structure, as specified in "Common Authentication Verifier
    ///  Encodings" in [C706-Ch13Security] section 13.2.6.1.
    ///  When Netlogon is functioning as its own SSP for the
    ///  RPC connection, this structure contains the signature,
    ///  a sequence number, and if encryption is requested,
    ///  a confounder. See section.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\05b99afd-1b6a-4207-b16e-959e6a483867.xml
    //  </remarks>
    public partial struct NL_AUTH_SIGNATURE
    {

        /// <summary>
        ///  A 16-bit little-endian integer that identifies the algorithm
        ///  that is used for signature computation. The only supported
        ///  signature algorithm is HMAC-MD5, as specified in [RFC2104].
        ///  The SignatureAlgorithm field MUST contain the following
        ///  value.
        /// </summary>
        public SignatureAlgorithm_Values SignatureAlgorithm;

        /// <summary>
        ///  A 16-bit little-endian integer that identifies the algorithm
        ///  used for encryption. The only supported encryption
        ///  algorithm is RSA-RC4. The SealAlgorithm field MUST
        ///  contain one of the following values.
        /// </summary>
        public SealAlgorithm_Values SealAlgorithm;

        /// <summary>
        ///  A 2-byte padding field. Both bytes MUST be set to 0xFF.
        /// </summary>
        public Pad_Values Pad;

        /// <summary>
        ///  Specifies properties of the structure. No flags are
        ///  currently defined. Both bytes MUST be set to zero and
        ///  MUST be ignored on receipt.
        /// </summary>
        [StaticSize(2, StaticSizeMode.Elements)]
        public byte[] Flags;

        /// <summary>
        ///  A 64-bit little-endian integer containing the sequence
        ///  number of the RPC message. For more information about
        ///  how to calculate the sequence number, see section.
        /// </summary>
        [StaticSize(8, StaticSizeMode.Elements)]
        public byte[] SequenceNumber;

        /// <summary>
        ///  A 64-bit value containing the final checksum of the
        ///  signature and the RPC message. For more information
        ///  about how to calculate the checksum, see section.
        /// </summary>
        [StaticSize(8, StaticSizeMode.Elements)]
        public byte[] Checksum;

        /// <summary>
        ///  A buffer used when the structure is used for encryption
        ///  in addition to signing. The bytes are filled with random
        ///  data that is used by the encryption algorithm. If the
        ///  structure is used only for signing, the confounder
        ///  is not included. For information about the confounder
        ///  and encrypting the data, see section.
        /// </summary>
        [StaticSize(8, StaticSizeMode.Elements)]
        [MarshalingCondition("IsRequestConfidentiality")]
        public byte[] Confounder;

        /// <summary>
        /// IsRequestConfidentiality
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static bool IsRequestConfidentiality(MarshalingType marshalingType, object value)
        {
            return (((NL_AUTH_SIGNATURE)value).SealAlgorithm == SealAlgorithm_Values.RC4);
        }
    }

    /// <summary>
    /// Signature algorithm
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags()]
    public enum SignatureAlgorithm_Values : ushort
    {

        /// <summary>
        ///  The packet is signed using HMAC-MD5.
        /// </summary>
        HMACMD5 = 0x0077,
    }

    /// <summary>
    /// Seal algorithm
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags()]
    public enum SealAlgorithm_Values : ushort
    {

        /// <summary>
        ///  The packet is not encrypted.
        /// </summary>
        NotEncrypted = 0xFFFF,

        /// <summary>
        ///  The packet is encrypted using RC4.
        /// </summary>
        RC4 = 0x007A,
    }

    /// <summary>
    /// Pad
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Pad_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0xFFFF,
    }

    /// <summary>
    ///  The NETLOGON_SECURE_CHANNEL_TYPE enumeration specifies
    ///  the type of secure channel to use in a logon transaction.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\4d1235e3-2c96-4e9f-a147-3cb338a0d09f.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags()]
    public enum _NETLOGON_SECURE_CHANNEL_TYPE
    {

        /// <summary>
        ///  An unauthenticated channel type. This value MUST NOT
        ///  be used in the Netlogon RPC calls between a client
        ///  and a remote server.
        /// </summary>
        NullSecureChannel = 0,

        /// <summary>
        ///  A secure channel between the local Windows NT LAN Manager
        ///  (NTLM) security provider and the Netlogon server. The
        ///  client and the server are the same machine for this
        ///  channel type. This value MUST NOT be used in the Netlogon
        ///  RPC calls between a client and a remote server. The
        ///  error code STATUS_INVALID_PARAMETER SHOULD be returned.
        /// </summary>
        MsvApSecureChannel = 1,

        /// <summary>
        ///  A secure channel from a domain member to a domain controller
        ///  (DC).
        /// </summary>
        WorkstationSecureChannel = 2,

        /// <summary>
        ///  A secure channel between two DCs, connected through
        ///  a trust relationship created between two windows_2000_server
        ///  or windows_server_2003domains. A trusted domain object
        ///  (TDO) is used in this type of channel.
        /// </summary>
        TrustedDnsDomainSecureChannel = 3,

        /// <summary>
        ///  A secure channel between two DCs, connected through
        ///  a trust relationship created between two domains, one
        ///  or both of which is a windows_nt_4_0domain.
        /// </summary>
        TrustedDomainSecureChannel = 4,

        /// <summary>
        ///  Secure channel from a LAN Manager server to a domain
        ///  controller. This value is no longer supported, and
        ///  it MUST NOT be used in the Netlogon RPC calls between
        ///  a client and a remote server. The error code STATUS_INVALID_PARAMETER
        ///  SHOULD be returned.
        /// </summary>
        UasServerSecureChannel = 5,

        /// <summary>
        ///  A secure channel from a backup domain controller to
        ///  a primary domain controller.
        /// </summary>
        ServerSecureChannel = 6,

        /// <summary>
        ///  Secure channel from a read-only domain controller (RODC)
        ///  to a domain controller.Added in windows_vista, and
        ///  supported in windows_server_2008, windows_7, and windows_server_7.
        /// </summary>
        CdcServerSecureChannel = 7,
    }

    /// <summary>
    ///  The NL_AUTH_MESSAGE structure is a token containing
    ///  information that is part of the first message in establishing
    ///  a security context between a client and a server. It
    ///  is used for establishing the secure session when Netlogon
    ///  functions as a security support provider (SSP). For
    ///  details about NL_AUTH_MESSAGE construction, see section
    ///  .
    /// </summary>
    //  <remarks>
    //   MS-NRPC\525f0bf3-ebd9-4360-9343-685767eb3c67.xml
    //  </remarks>
    public partial struct NL_AUTH_MESSAGE
    {

        /// <summary>
        ///  A 32-bit unsigned integer. This value is used to indicate
        ///  whether the message is a negotiate request message
        ///  sent from a client to a server, or a negotiate response
        ///  message sent from the server to the client. MessageType
        ///  MUST be one, and only one, of the following.
        /// </summary>
        public MessageType_Values MessageType;

        /// <summary>
        ///  A set of bit flags indicating the principal names carried
        ///  in the request. A flag is TRUE (or set) if its value
        ///  is equal to 1. These flags are set only in negotiate
        ///  messages. The value is constructed from one or more
        ///  bit flags from the following table.
        /// </summary>
        public NL_AUTH_MESSAGE_Flags_Value Flags;

        /// <summary>
        ///  Text buffer that contains a concatenation of null-terminated
        ///  strings for each of the name flags set in the Flags
        ///  field. The order is the same as the order of the Flags
        ///  values (A–E). This buffer is only used in negotiate
        ///  messages. For negotiate response messages, the buffer
        ///  contains a NULL character.
        /// </summary>
        public byte[] Buffer;
    }

    /// <summary>
    /// Message type
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags()]
    public enum MessageType_Values : int
    {

        /// <summary>
        ///  This is a negotiate request message.
        /// </summary>
        NegotiateRequest = 0x00000000,

        /// <summary>
        ///  This is a negotiate response message.
        /// </summary>
        NegotiateResponse = 0x00000001,
    }

    /// <summary>
    ///  The IPv4_Sockaddr structure specifies the format of
    ///  an IPv4 socket address. This structure is built as
    ///  if on a little-endian machine, and is treated as a
    ///  byte array.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\75981bfb-a7bb-41bc-8705-388531af6ed8.xml
    //  </remarks>
    public partial struct IPv4_Sockaddr
    {

        /// <summary>
        ///  The address family; MUST be 0x0002.
        /// </summary>
        [StaticSize(2, StaticSizeMode.Elements)]
        public byte[] AddressFamily;

        /// <summary>
        ///  An IP port number.
        /// </summary>
        [StaticSize(2, StaticSizeMode.Elements)]
        public byte[] Port;

        /// <summary>
        ///  An IP address, as specified in [RFC791].
        /// </summary>
        [StaticSize(4, StaticSizeMode.Elements)]
        public byte[] Address;

        /// <summary>
        ///  Set to zero. Ignored by the server.
        /// </summary>
        [StaticSize(8, StaticSizeMode.Elements)]
        public byte[] Padding;
    }

    /// <summary>
    ///  The NETLOGON_LOGON_INFO_CLASS enumeration identifies
    ///  a particular type of logon information block.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\8c7808e5-4e5c-420e-9c90-47286da2218f.xml
    //  </remarks>
    [Flags()]
    public enum _NETLOGON_LOGON_INFO_CLASS
    {

        /// <summary>
        ///  Logon information provided pertains to an interactive
        ///  account logon. Interactive account logon requires a
        ///  user to physically input credentials at the client
        ///  that are then authenticated by the DC.windows_nt_4_0,
        ///  windows_2000, windows_xp, windows_server_2003, and
        ///  windows_server_2008, windows_7, and windows_server_7support
        ///  NetlogonInteractiveInformation type.
        /// </summary>
        NetlogonInteractiveInformation = 1,

        /// <summary>
        ///  Logon information provided pertains to a network account
        ///  logon. Network logon is transparent to the user. The
        ///  user has already input his or her credentials during
        ///  interactive logon and has been authenticated by the
        ///  server or DC. These credentials are used again to log
        ///  the user onto another network resource without prompting
        ///  the user for his or her credentials.windows_nt_4_0,
        ///  windows_2000, windows_xp, windows_server_2003, and
        ///  windows_server_2008, windows_7, and windows_server_7support
        ///  NetlogonNetworkInformation type.
        /// </summary>
        NetlogonNetworkInformation = 2,

        /// <summary>
        ///  Logon information provided pertains to a service account
        ///  logon. A service account acts as a nonprivileged user
        ///  on the local computer and presents anonymous credentials
        ///  to any remote server.windows_nt_4_0, windows_2000,
        ///  windows_xp, windows_server_2003, and windows_server_2008,
        ///  windows_7, and windows_server_7support NetlogonServiceInformation
        ///  type.
        /// </summary>
        NetlogonServiceInformation = 3,

        /// <summary>
        ///  Logon information provided pertains to a generic account
        ///  logon. This type of account logon is for generic pass-through
        ///  authentication, as specified in section , that enables
        ///  servers to forward NTLM and Digest authentication credentials
        ///  to a DC for authorization.windows_nt_4_0, windows_2000,
        ///  windows_xp, windows_server_2003, and windows_server_2008,
        ///  windows_7, and windows_server_7support NetlogonGenericInformation
        ///  type.
        /// </summary>
        NetlogonGenericInformation = 4,

        /// <summary>
        ///  Logon information provided pertains to a transitive
        ///  interactive account logon and can be passed through
        ///  transitive trust links.windows_2000, windows_xp, windows_server_2003,
        ///  and windows_server_2008, windows_7, and windows_server_7support
        ///  NetlogonInteractiveTransitiveInformation type.
        /// </summary>
        NetlogonInteractiveTransitiveInformation = 5,

        /// <summary>
        ///  Logon information provided pertains to a transitive
        ///  network account logon and can be passed through transitive
        ///  trust links.windows_2000, windows_xp, windows_server_2003,
        ///  and windows_server_2008, windows_7, and windows_server_7support
        ///  NetlogonNetworkTransitiveInformation type.
        /// </summary>
        NetlogonNetworkTransitiveInformation = 6,

        /// <summary>
        ///  Logon information provided pertains to a transitive
        ///  service account logon and can be passed through transitive
        ///  trust links.windows_2000, windows_xp, windows_server_2003,
        ///  and windows_server_2008, windows_7, and windows_server_7support
        ///  NetlogonServiceTransitiveInformation type.
        /// </summary>
        NetlogonServiceTransitiveInformation = 7,
    }

    /// <summary>
    ///  The NETLOGON_VALIDATION_INFO_CLASS enumeration selects
    ///  the type of logon information block being used.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\95154ae4-d305-43e5-82e4-d5353e0f117c.xml
    //  </remarks>
    [Flags()]
    public enum _NETLOGON_VALIDATION_INFO_CLASS
    {

        /// <summary>
        ///  Associated structure is NETLOGON_VALIDATION_UAS_INFO.windows_nt_4_0,
        ///  windows_2000, windows_xp, windows_server_2003, and
        ///  windows_server_2008, windows_7, and windows_server_7:
        ///  NETLOGON_VALIDATION_INFO_CLASS enumeration has NetlogonValidationUasInfo
        ///  type defined. This value is used by LAN Manager in
        ///  support of LAN Manager products, and is beyond the
        ///  scope of this document.
        /// </summary>
        NetlogonValidationUasInfo = 1,

        /// <summary>
        ///  Associated structure is NETLOGON_VALIDATION_SAM_INFO
        ///  (Section 2.2.1.4.11).windows_nt_4_0, windows_2000,
        ///  windows_xp, windows_server_2003, and windows_server_2008,
        ///  windows_7, and windows_server_7support NetlogonValidationSamInfo
        ///  type.
        /// </summary>
        NetlogonValidationSamInfo = 2,

        /// <summary>
        ///  Associated structure is NETLOGON_VALIDATION_SAM_INFO2
        ///  (Section 2.2.1.4.12).windows_nt_4_0, windows_2000,
        ///  windows_xp, windows_server_2003, and windows_server_2008,
        ///  windows_7, and windows_server_7support NetlogonValidationSamInfo2
        ///  type.
        /// </summary>
        NetlogonValidationSamInfo2 = 3,

        /// <summary>
        ///  Associated structure is NETLOGON_VALIDATION_GENERIC_INFO2
        ///  (Section 2.2.1.4.8).windows_nt_4_0, windows_2000, windows_xp,
        ///  windows_server_2003, and windows_server_2008, windows_7,
        ///  and windows_server_7support NetlogonValidationGenericInfo
        ///  type.
        /// </summary>
        NetlogonValidationGenericInfo = 4,

        /// <summary>
        ///  Associated structure is NETLOGON_VALIDATION_GENERIC_INFO2
        ///  (Section 2.2.1.4.8).windows_nt_4_0, windows_2000, windows_xp,
        ///  windows_server_2003, and windows_server_2008, windows_7,
        ///  and windows_server_7support NetlogonValidationGenericInfo2
        ///  type.
        /// </summary>
        NetlogonValidationGenericInfo2 = 5,

        /// <summary>
        ///  Associated structure is NETLOGON_VALIDATION_SAM_INFO4
        ///  (Section 2.2.1.4.13).windows_nt_4_0, windows_2000,
        ///  windows_xp, windows_server_2003, and windows_server_2008,
        ///  windows_7, and windows_server_7support NetlogonValidationSamInfo4
        ///  type.
        /// </summary>
        NetlogonValidationSamInfo4 = 6,
    }

    /// <summary>
    ///  DBChangeInfo fields
    /// </summary>
    //  <remarks>
    //   MS-NRPC\b3a7e5f3-d669-4f54-acb8-e85580eaf46d.xml
    //  </remarks>
    public partial struct nested_NETLOGON_DB_CHANGE_Announcement_Message_DBChangeInfo
    {

        /// <summary>
        ///  A 32-bit value that identifies the database as follows:
        /// </summary>
        public DBIndex_Values DBIndex;

        /// <summary>
        ///  A 64-bit value that contains the database serial number
        ///  for the database identified by the DBIndex field.
        /// </summary>
        public _LARGE_INTEGER LargeSerialNumber;

        /// <summary>
        ///  The time in UTC of the database creation expressed as
        ///  an 8-byte value in the TIME format in a FILETIME structure,
        ///  as specified in [MS-RPCE]Appendix A.In what follows,
        ///  the above message is referred to as the announcement
        ///  message.
        /// </summary>
        public _LARGE_INTEGER DateAndTime;
    }

    /// <summary>
    /// DB index
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum DBIndex_Values : uint
    {

        /// <summary>
        ///  Indicates the SAM database.
        /// </summary>
        SamDatabase = 0x00000000,

        /// <summary>
        ///  Indicates the SAM built-in database.
        /// </summary>
        SamBuiltInDatabase = 0x00000001,

        /// <summary>
        ///  Indicates the LSA database.
        /// </summary>
        LsaDatabase = 0x00000002,
    }

    /// <summary>
    ///  The following is the format of the payload of a mailslot
    ///  message used in Netlogon replication, as specified
    ///  in section. The message is used to indicate that one
    ///  or more changes have taken place in the account database,
    ///  and carries an indication of the changes from the PDC
    ///  to the BDC. Because it is sent in the open, this is
    ///  a hint, and the BDC must connect to the PDC over a
    ///  reliable transport and secure connection to obtain
    ///  the actual change.The DBChangeInfo field represents
    ///  information about a state of one of the databases (Security
    ///  Account Manager Built-in, Security Account Manager,
    ///  or Local Security Authority). The number of DBChangeInfo
    ///  fields is specified by the DBCount field. The format
    ///  of the DBChangeInfo field is described below. The fields
    ///  in the above diagram are in little-endian format and
    ///  have the following meanings:
    /// </summary>
    //  <remarks>
    //   MS-NRPC\b3a7e5f3-d669-4f54-acb8-e85580eaf46d.xml
    //  </remarks>
    public partial struct NETLOGON_DB_CHANGE_Announcement_Message
    {

        /// <summary>
        ///  A two-byte field identifying the message. MUST be set
        ///  to 0x000A.
        /// </summary>
        public NETLOGON_DB_CHANGE_Announcement_Message_MessageType_Values MessageType;

        /// <summary>
        ///  The low DWORD part of the 64-bit database serial number
        ///  of the SAM database .
        /// </summary>
        public uint LowSerialNumber;

        /// <summary>
        ///  An unsigned 32-bit value representing the time stamp
        ///  for the SAM database creation time. This MUST be expressed
        ///  as the number of seconds elapsed since midnight of
        ///  January 1, 1970.
        /// </summary>
        public uint DateAndTime;

        /// <summary>
        ///  An unsigned 32-bit value that specifies the message
        ///  interval in seconds between change announcements sent
        ///  to the BDCs.
        /// </summary>
        public uint Pulse;

        /// <summary>
        ///  An unsigned 32-bit value that indicates the number of
        ///  seconds the recipient of the message SHOULD The recipient
        ///  of the message waits for the indicated number of seconds
        ///  before contacting the sender. wait before contacting
        ///  the sender.
        /// </summary>
        public uint Random;

        /// <summary>
        ///  The null-terminated name of the PDC sending the message.
        ///  MUST be encoded in the original equipment manufacturer
        ///  (OEM) character set.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] PrimaryDCName;

        /// <summary>
        ///  The null-terminated domain name encoded in the OEM character
        ///  set. The domain name is padded to a multiple of 2 bytes
        ///  for alignment reasons.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] DomainName;

        /// <summary>
        ///  The null-terminated name of the PDC sending the message.
        ///  MUST be encoded in the Unicode character set.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] UnicodePrimaryDCName;

        /// <summary>
        ///  The null-terminated domain name. MUST be encoded in
        ///  the Unicode character set.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public ushort[] UnicodeDomainName;

        /// <summary>
        ///  An unsigned 32-bit value representing the number of
        ///  DBChangeInfo fields in the message.
        /// </summary>
        public uint DBCount;

        /// <summary>
        ///   </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public nested_NETLOGON_DB_CHANGE_Announcement_Message_DBChangeInfo[] DBChangeInfo;

        /// <summary>
        ///  An unsigned 32-bit value specifying the size in bytes
        ///  of the DomainSid field.
        /// </summary>
        public uint DomainSidSize;

        /// <summary>
        ///  The SID of the domain.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public byte[] DomainSid;

        /// <summary>
        ///  An unsigned 32-bit value containing the version of the
        ///  message format. MUST be set to 0x00000001.
        /// </summary>
        public MessageFormatVersion_Values MessageFormatVersion;

        /// <summary>
        ///  An unsigned 32-bit field identifying the message. MUST
        ///  be set to 0xFFFFFFFF.
        /// </summary>
        public MessageToken_Values MessageToken;
    }

    /// <summary>
    /// NETLOGON_DB_CHANGE announcement message type
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum NETLOGON_DB_CHANGE_Announcement_Message_MessageType_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x000A,
    }

    /// <summary>
    /// Message format version
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum MessageFormatVersion_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x00000001,
    }

    /// <summary>
    /// Message token
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum MessageToken_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0xFFFFFFFF,
    }

    /// <summary>
    ///  The NETLOGON_DELTA_TYPE enumeration defines an enumerated
    ///  set of possible database changes.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\f8a8cd32-426d-45f1-be45-e0dc5c1c1359.xml
    //  </remarks>
    [Flags()]
    public enum _NETLOGON_DELTA_TYPE
    {

        /// <summary>
        ///  Adds or changes a domain Security Account Manager (SAM)
        ///  account.
        /// </summary>
        AddOrChangeDomain = 1,

        /// <summary>
        ///  Adds or changes a group SAM account.
        /// </summary>
        AddOrChangeGroup = 2,

        /// <summary>
        ///  Deletes a group SAM account.
        /// </summary>
        DeleteGroup = 3,

        /// <summary>
        ///  Renames a group SAM account.
        /// </summary>
        RenameGroup = 4,

        /// <summary>
        ///  Adds or changes a user SAM account.
        /// </summary>
        AddOrChangeUser = 5,

        /// <summary>
        ///  Deletes a user SAM account.
        /// </summary>
        DeleteUser = 6,

        /// <summary>
        ///  Renames a user SAM account.
        /// </summary>
        RenameUser = 7,

        /// <summary>
        ///  Changes a group membership record.
        /// </summary>
        ChangeGroupMembership = 8,

        /// <summary>
        ///  Adds or changes an alias.
        /// </summary>
        AddOrChangeAlias = 9,

        /// <summary>
        ///  Deletes an alias.
        /// </summary>
        DeleteAlias = 10,

        /// <summary>
        ///  Renames an alias.
        /// </summary>
        RenameAlias = 11,

        /// <summary>
        ///  Changes the membership record for an alias.
        /// </summary>
        ChangeAliasMembership = 12,

        /// <summary>
        ///  Adds or changes an LSA policy.
        /// </summary>
        AddOrChangeLsaPolicy = 13,

        /// <summary>
        ///  Adds or changes a trusted domain account.
        /// </summary>
        AddOrChangeLsaTDomain = 14,

        /// <summary>
        ///  Deletes a trusted domain account.
        /// </summary>
        DeleteLsaTDomain = 15,

        /// <summary>
        ///  Adds or changes an LSA user or machine account.
        /// </summary>
        AddOrChangeLsaAccount = 16,

        /// <summary>
        ///  Deletes an LSA user or machine account.
        /// </summary>
        DeleteLsaAccount = 17,

        /// <summary>
        ///  Adds or changes an LSA encrypted data block.
        /// </summary>
        AddOrChangeLsaSecret = 18,

        /// <summary>
        ///  Deletes an LSA encrypted data block.
        /// </summary>
        DeleteLsaSecret = 19,

        /// <summary>
        ///  Deletes a group account based on a string name.In windows_nt_4_0
        ///  replication, this type requires NegotiateFlag=0x00000010.
        ///  For more information, see the Capability Negotiation
        ///  bullet in section  and the NegotiateFlags field description
        ///  in sections  (NetrServerAuthenticate2) and  (NetrServerAuthenticate3).
        /// </summary>
        DeleteGroupByName = 20,

        /// <summary>
        ///  Deletes a user account based on a string name.In windows_nt_4_0
        ///  replication, this type requires NegotiateFlag=0x00000010.
        ///  For more information, see the Capability Negotiation
        ///  bullet in section  and the NegotiateFlags field description
        ///  in sections  (NetrServerAuthenticate2) and  (NetrServerAuthenticate3).
        /// </summary>
        DeleteUserByName = 21,

        /// <summary>
        ///  Updates the database serial number.In windows_nt_4_0
        ///  replication, this type requires NegotiateFlag=0x00000010.
        ///  For more information, see the Capability Negotiation
        ///  bullet in section  and the NegotiateFlags field description
        ///  in sections  (NetrServerAuthenticate2) and  (NetrServerAuthenticate3).
        /// </summary>
        SerialNumberSkip = 22,
    }

    /// <summary>
    ///  The IPv6_Sockaddr structure specifies the format of
    ///  an IPv6 socket address. This structure is built as
    ///  if on a little-endian machine, and is treated as a
    ///  byte array.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\f8e21b2b-6ea5-4eb9-a350-acdabe33cc93.xml
    //  </remarks>
    public partial struct IPv6_Sockaddr
    {

        /// <summary>
        ///  Address family; MUST be 0x0017.
        /// </summary>
        [StaticSize(2, StaticSizeMode.Elements)]
        public byte[] AddressFamily;

        /// <summary>
        ///  An IP port number.
        /// </summary>
        [StaticSize(2, StaticSizeMode.Elements)]
        public byte[] Port;

        /// <summary>
        ///  Flow information.
        /// </summary>
        [StaticSize(4, StaticSizeMode.Elements)]
        public byte[] FlowInfo;

        /// <summary>
        ///  An IP address, as specified in [RFC3493].
        /// </summary>
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] Address;

        /// <summary>
        ///  Set of interfaces for a scope, as specified in [RFC3493].
        /// </summary>
        [StaticSize(4, StaticSizeMode.Elements)]
        public byte[] ScopeID;
    }

    /// <summary>
    ///  The NL_AUTH_SHA2_SIGNATURE structure is a security token
    ///  that defines the SHA2 authentication signature used
    ///  by Netlogon to execute Netlogon methods over a secure
    ///  channel. The NL_AUTH_SHA2_SIGNATURE structure is supported
    ///  by windows_server_2008, windows_7 and windows_server_7.
    ///  It follows the security trailer that a security provider
    ///  MUST associate with a signed or encrypted message.
    ///  A security trailer or sec_trailer structure ([MS-RPCE]
    ///  section) has syntax equivalent to the auth_verifier_co_t
    ///  structure, as specified in [C706-Ch13Security], "Common
    ///  Authentication Verifier Encodings". When Netlogon is
    ///  functioning as its own SSP for the RPC connection,
    ///  this structure contains the signature, a sequence number,
    ///  and (if encryption is requested) a confounder. See
    ///  section.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\fc77c8aa-6c21-446b-a822-4da26cc8a9a8.xml
    //  </remarks>
    public partial struct NL_AUTH_SHA2_SIGNATURE
    {

        /// <summary>
        ///  A 16-bit little-endian integer that identifies the algorithm
        ///  that is used for signature computation. The only supported
        ///  signature algorithm is HMAC-SHA256 [RFC4634]. The SignatureAlgorithm
        ///  field MUST contain the following value.
        /// </summary>
        public NL_AUTH_SHA2_SIGNATURE_SignatureAlgorithm_Values SignatureAlgorithm;

        /// <summary>
        ///  A 16-bit little-endian integer that identifies the algorithm
        ///  used for encryption. The only supported encryption
        ///  algorithm is AES-128 [FIPS197]. The SealAlgorithm field
        ///  MUST contain one of the following values.
        /// </summary>
        public NL_AUTH_SHA2_SIGNATURE_SealAlgorithm_Values SealAlgorithm;

        /// <summary>
        ///  A 2-byte padding field. Both bytes MUST be set to 0xFF.
        /// </summary>
        public NL_AUTH_SHA2_SIGNATURE_Pad_Values Pad;

        /// <summary>
        ///  Specifies properties of the structure. No Flags are
        ///  currently defined. Both bytes MUST be set to zero and
        ///  MUST be ignored on receipt.
        /// </summary>
        public Flags_Values Flags;

        /// <summary>
        ///  A 64-bit little-endian integer containing the SequenceNumber
        ///  of the RPC message. For more information about how
        ///  to calculate the SequenceNumber, see section
        /// </summary>
        [StaticSize(8, StaticSizeMode.Elements)]
        public byte[] SequenceNumber;

        /// <summary>
        ///  A 256-bit value containing the final Checksum of the
        ///  signature and the RPC message. For more information
        ///  about how to calculate the Checksum, see section.
        /// </summary>
        [StaticSize(8, StaticSizeMode.Elements)]
        public byte[] Checksum;

        /// <summary>
        ///  A buffer that is employed when the structure is used
        ///  for encryption, in addition to signing. The bytes are
        ///  filled with random data that is used by the encryption
        ///  algorithm. If the structure is used only for signing,
        ///  the Confounder is not included. For information about
        ///  the Confounder and encrypting the data, see section
        ///  .
        /// </summary>
        [StaticSize(8, StaticSizeMode.Elements)]
        [MarshalingCondition("IsRequestConfidentiality")]
        public byte[] Confounder;

        /// <summary>
        /// We changed the length of Checksum from 32 to 8, 
        /// put extra 24 bytes here.
        /// </summary>
        [StaticSize(24, StaticSizeMode.Elements)]
        public byte[] Dummy;

        /// <summary>
        /// IsRequestConfidentiality
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static bool IsRequestConfidentiality(MarshalingType marshalingType, object value)
        {
            return (((NL_AUTH_SHA2_SIGNATURE)value).SealAlgorithm == NL_AUTH_SHA2_SIGNATURE_SealAlgorithm_Values.AES128);
        }
    }

    /// <summary>
    /// NL_AUTH_SHA2_SIGNATURE signature algorithm
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum NL_AUTH_SHA2_SIGNATURE_SignatureAlgorithm_Values : ushort
    {

        /// <summary>
        ///  The packet is signed using HMAC-SHA256.
        /// </summary>
        HMACSHA256 = 0x0013,
    }

    /// <summary>
    /// NL_AUTH_SHA2_SIGNATURE seal algorithm
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags()]
    public enum NL_AUTH_SHA2_SIGNATURE_SealAlgorithm_Values : ushort
    {

        /// <summary>
        ///  The packet is not encrypted.
        /// </summary>
        NotEncrypted = 0xFFFF,

        /// <summary>
        ///  The packet is encrypted using AES-128.
        /// </summary>
        AES128 = 0x001A,
    }

    /// <summary>
    /// NL_AUTH_SHA2_SIGNATURE pad
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum NL_AUTH_SHA2_SIGNATURE_Pad_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0xFFFF,
    }

    /// <summary>
    /// Flags
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Flags_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0000,
    }

    /// <summary>
    ///  The SYNC_STATE enumeration tracks the progress of synchronization
    ///  of the database between BDCs and PDCs. Synchronization
    ///  is initiated by the client calling NetrDatabaseSync2.
    ///  All references to SyncContext in the following synchronization
    ///  state descriptions refer to the SyncContext parameter
    ///  in that method.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\ffce9bbc-6ae5-4478-8f45-e82c3847aaa2.xml
    //  </remarks>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags()]
    public enum _SYNC_STATE
    {

        /// <summary>
        ///  A state that MUST be used unless the current synchronization
        ///  is the restart of a full synchronization.
        /// </summary>
        NormalState = 0,

        /// <summary>
        ///  The SyncContext parameter is the domainRID with which
        ///  to continue.
        /// </summary>
        DomainState = 1,

        /// <summary>
        ///  The SyncContext parameter is the global groupRID with
        ///  which to continue.
        /// </summary>
        GroupState = 2,

        /// <summary>
        ///  Not used.
        /// </summary>
        UasBuiltInGroupState = 3,

        /// <summary>
        ///  The SyncContext parameter is the user RID with which
        ///  to continue.
        /// </summary>
        UserState = 4,

        /// <summary>
        ///  The SyncContext parameter is the global groupRID with
        ///  which to continue.
        /// </summary>
        GroupMemberState = 5,

        /// <summary>
        ///  The SyncContext parameter MUST have a value of 0, indicating
        ///  synchronization restarts at the first databasealias
        ///  and that AddOrChangeAlias (see NETLOGON_DELTA_TYPE
        ///  enumeration, ) was the last account change being performed
        ///  prior to the restart.
        /// </summary>
        AliasState = 6,

        /// <summary>
        ///  The SyncContext parameter MUST have a value of 0, indicating
        ///  synchronization restarts at the first databasealias
        ///  and that ChangeAliasMembership (see NETLOGON_DELTA_TYPE
        ///  enumeration, ) was the last account change being performed
        ///  prior to the restart.
        /// </summary>
        AliasMemberState = 7,

        /// <summary>
        ///  The database has finished synchronization.
        /// </summary>
        SamDoneState = 8,
    }


    /// <summary>
    ///  The NLPR_LOGON_HOURS structure contains the logon policy
    ///  information for when a user account is permitted to
    ///  authenticate.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\05a64da2-6e5f-40cc-9848-700634100613.xml
    //  </remarks>
    public partial struct _NLPR_LOGON_HOURS
    {

        /// <summary>
        ///  UnitsPerWeek member.
        /// </summary>
        public ushort UnitsPerWeek;

        /// <summary>
        ///  LogonHours member.
        /// </summary>
        [Length("(UnitsPerWeek + 7)/8")]
        [Size("1260")]
        public byte[] LogonHours;
    }

    /// <summary>
    ///  The DOMAIN_NAME_BUFFER structure defines information
    ///  returned by the NetrEnumerateTrustedDomains method,
    ///  as specified in section. The structure is used to
    ///  describe a set of trusteddomain names.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\11743dc4-7a2d-4464-b351-50aeb8801b8b.xml
    //  </remarks>
    public partial struct _DOMAIN_NAME_BUFFER
    {

        /// <summary>
        ///  The size, in bytes, of the buffer pointed to by the
        ///  DomainNames field.
        /// </summary>
        public uint DomainNameByteCount;

        /// <summary>
        ///  The Unicode string buffer that contains the list of
        ///  trusteddomains, in MULTI-SZ format. MULTI-SZ format
        ///  is a UTF-16 string composed of one or more substrings.
        ///  Each substring is separated from adjacent substrings
        ///  by the UTF-16 null character, 0x0000. After the final
        ///  substring, the MULTI-SZ format string is terminated
        ///  by two UTF-16 null characters. For example, if there
        ///  are three trusteddomains, DOMAIN1, DOMAIN2, and DOMAIN3,
        ///  the DomainNames string buffer would have the following
        ///  form:DOMAIN1&lt;null&gt;DOMAIN2&lt;null&gt;DOMAIN3&lt;null&gt;&lt;null&gt;
        ///  where &lt;null&gt; is the UTF-16 null character, 0x0000.
        /// </summary>
        [Size("DomainNameByteCount")]
        public byte[] DomainNames;
    }

    /// <summary>
    ///  The NLPR_USER_PRIVATE_INFO structure defines a data
    ///  buffer that is optionally encrypted with the session
    ///  key, as detailed in this section. The structure is
    ///  used to carry user account passwords as follows.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\1333d230-8d17-4c16-a09d-0b7c785d591a.xml
    //  </remarks>
    public partial struct _NLPR_USER_PRIVATE_INFO
    {

        /// <summary>
        ///  Is either TRUE (0x01) or FALSE (0x00). The SensitiveData
        ///  field indicates whether or not the data is encrypted
        ///  as follows. If this field is 0x00, then the data is
        ///  not encrypted. If the field is set to 0x01, the data
        ///  pointed to by the Data field is encrypted with the
        ///  session key used on the secure channel between the
        ///  client and the server exchanging this data structure
        ///  to the client. The encryption algorithm is RC4 if the
        ///  flag C is set in the negotiated flags between the client
        ///  and the server, as specified in section ; otherwise
        ///  the encryption algorithm is DES.
        /// </summary>
        public byte SensitiveData;

        /// <summary>
        ///  The size, in bytes, of the Data field.
        /// </summary>
        public uint DataLength;

        /// <summary>
        ///  A pointer to a buffer with a size of DataLength. If
        ///  the SensitiveData field is set to TRUE, this data is
        ///  encrypted as described in the description of the SensitiveData
        ///  field. The buffer content prior to encryption (if any)
        ///  is shown in the following table.01234567891 01234567892
        ///  01234567893 01DataTypeLmLengthLmMaximumLengthUnused1LmHash[0..3]LmHash[4..7]LmHash[8..11]LmHash[12..15]NtLengthNtMaximumLengthUnused2NtHash[0..3]NtHash[4..7]NtHash[8..11]NtHash[12..15]LmHistoryLengthLmHistoryMaximumLengthUnused3NtHistoryLengthNtHistoryMaximumLengthUnused4NtHistoryArray
        ///  (variable length) . . . LmHistoryArray (variable length)
        ///  . . .DataType: An unsigned integer. This value MUST
        ///  be 0x00000002.LmLength: An unsigned (short) integer.
        ///  This value MUST be either 0x0010 or 0x0000. If 0x0010,
        ///  the LmHash field contains the LM hash of the user password
        ///  (specified in [MS-NLMP]). If 0x0000, the value of the
        ///  LmHash field is undefined and MUST be ignored upon
        ///  receipt.LmMaximumLength: This value MUST be the same
        ///  value as LmLength. Unused1: This value MUST be zero
        ///  and ignored on receipt.LmHash: The encrypted ([MS-SAMR]
        ///  section) LM OWF ([MS-NLMP] section) of the user password.
        ///  The 16-byte encryption key is created by concatenating
        ///  four times the relative ID (from the given user’s SID).
        ///  NtLength: An unsigned (short) integer. This value MUST
        ///  be either 0x0010 or 0x0000. If 0x0010, the NtHash field
        ///  contains the NT hash of the user password (specified
        ///  in [MS-NLMP]). If 0x0000, the value of the NtHash field
        ///  is undefined and MUST be ignored upon receipt.NtMaximumLength:
        ///  This value MUST be the same value as NtLength.Unused2:
        ///  This value SHOULD be zero and ignored on receipt.NtHash:
        ///  The encrypted ([MS-SAMR] section) NT OWF ([MS-NLMP]
        ///  section) of the user password. The 16-byte encryption
        ///  key is created by concatenating four times the relative
        ///  ID (from the given user’s SID). LmHistoryLength: An
        ///  unsigned (short) integer. This value is the length,
        ///  in bytes, of the LmHistoryArray field.LmHistoryMaximumLength:
        ///  This value MUST be the same value as LmHistoryLength.Unused3:
        ///  This value SHOULD be zero and ignored on receipt.NtHistoryLength:
        ///  An unsigned (short) integer. This value is the length,
        ///  in bytes, of the NtHistoryArray field.NtHistoryMaximumLength:
        ///  This value MUST be the same value as NtHistoryLength.Unused4:
        ///  This value SHOULD be zero and ignored on receipt.NtHistoryArray:
        ///  An array of NT hash values of user passwords for the
        ///  given user. The array is ordered so that the first
        ///  element is the hash of the current password and the
        ///  last element is the hash of the oldest password. The
        ///  number of elements in the array is the value of the
        ///  NtHistoryLength field divided by 0x0010.LmHistoryArray:
        ///  An array of LM hash values of user passwords for the
        ///  given user. The array is ordered so that the first
        ///  element is the hash of the current password and the
        ///  last element is the hash of the oldest password.The
        ///  number of elements in the array is the value of the
        ///  LmHistoryLength field divided by 0x0010.
        /// </summary>
        [Size("DataLength")]
        public byte[] Data;
    }

    /// <summary>
    ///  The NL_DNS_NAME_INFO structure provides the information
    ///  on a DNS name (record) (as specified in [RFC2782])
    ///  to be updated by the DsrUpdateReadOnlyServerDnsRecords
    ///  method . The DsrUpdateReadOnlyServerDnsRecords method
    ///  will update DNS as requested by the Register field's
    ///  value in this structure.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\1c7e6520-45a2-43e1-ac0b-43e771f85b14.xml
    //  </remarks>
    public partial struct _NL_DNS_NAME_INFO
    {

        /// <summary>
        ///  The type of DNS name, which MUST be one, and only one,
        ///  of the following:
        /// </summary>
        public Type_Values Type;

        /// <summary>
        ///  The string that will be based on the DnsDomainInfoType
        ///  defined below.
        /// </summary>
        [String()]
        public string DnsDomainInfo;

        /// <summary>
        ///  The type of DnsDomainInfo member, which MUST be one,
        ///  and only one, of the following.
        /// </summary>
        public DnsDomainInfoType_Values DnsDomainInfoType;

        /// <summary>
        ///  The priority for DNS SRV records.
        /// </summary>
        public uint Priority;

        /// <summary>
        ///  The weight for DNS SRV records.
        /// </summary>
        public uint Weight;

        /// <summary>
        ///  The port for the DNS SRV record.
        /// </summary>
        public uint Port;

        /// <summary>
        ///  TRUE indicates to register the DNS name; FALSE indicates
        ///  to deregister the DNS name.
        /// </summary>
        public byte Register;

        /// <summary>
        ///  The update status of the DNS name. Status MUST be set
        ///  to 0x00000000 on success; otherwise, it contains a
        ///  nonzero error code.Added in longhorn_server.
        /// </summary>
        public uint Status;
    }

    /// <summary>
    /// Type
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags()]
    public enum Type_Values : uint
    {

        /// <summary>
        ///  _ldap._tcp.&lt;SiteName&gt;._sites.&lt;DnsDomainName&gt;.Allows
        ///  a client to find an LDAP server in the domain named
        ///  by &lt;DnsDomainName&gt;, and is in the site named by &lt;SiteName&gt;.
        /// </summary>
        NlDnsLdapAtSite = 22,

        /// <summary>
        ///  _ldap._tcp.&lt;SiteName&gt;._sites.gc._msdcs.&lt;DnsForestName&gt;.Allows
        ///  a client to find a DC serving a Global Catalog (GC)
        ///  in the forest named by &lt;DnsForestName&gt;, and is in the
        ///  site named by &lt;SiteName&gt;.
        /// </summary>
        NlDnsGcAtSite = 25,

        /// <summary>
        ///  &lt;DsaGuid&gt;._msdcs.&lt;DnsForestName&gt;.Allows a client to
        ///  find a DC in the forest named by &lt;DnsForestName&gt; based
        ///  on the DSA GUID. For a definition of DSA GUID, see
        ///  [MS-ADTS] section.
        /// </summary>
        NlDnsDsaCname = 28,

        /// <summary>
        ///  _kerberos._tcp.&lt;SiteName&gt;._sites.dc._msdcs.&lt;DnsDomainName&gt;.Allows
        ///  a client to find a DC running a Kerberos KDC in the
        ///  domain named by &lt;DnsDomainName&gt;, and is in the site
        ///  named by &lt;SiteName&gt;.
        /// </summary>
        NlDnsKdcAtSite = 30,

        /// <summary>
        ///  _ldap._tcp.&lt;SiteName&gt;._sites.dc._msdcs.&lt;DnsDomainName&gt;.Allows
        ///  a client to find a DC in the domain named by &lt;DnsDomainName&gt;,
        ///  and is in the site named by &lt;SiteName&gt;.
        /// </summary>
        NlDnsDcAtSite = 32,

        /// <summary>
        ///  _kerberos._tcp.&lt;SiteName&gt;._sites.&lt;DnsDomainName&gt;.Allows
        ///  a client to find a RFC-1510 compliant Kerberos KDC
        ///  in the domain named by &lt;DnsDomainName&gt;, and is in the
        ///  site named by &lt;SiteName&gt;.
        /// </summary>
        NlDnsRfc1510KdcAtSite = 34,

        /// <summary>
        ///  _gc._tcp.&lt;SiteName&gt;._sites.&lt;DnsForestName&gt;.Allows a
        ///  client to find a Global Catalog (GC) server in the
        ///  forest named by &lt;DnsForestName&gt;, and is in the site
        ///  named by &lt;SiteName&gt;.
        /// </summary>
        NlDnsGenericGcAtSite = 36,
    }

    /// <summary>
    /// Dns domain info type
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum DnsDomainInfoType_Values : uint
    {

        /// <summary>
        ///  The DnsDomainInfo member is a DNSdomain name.
        /// </summary>
        NlDnsDomainName = 1,

        /// <summary>
        ///  The DnsDomainInfo member is a DNSdomain name alias.
        /// </summary>
        NlDnsDomainNameAlias = 2,

        /// <summary>
        ///  The DnsDomainInfo member is a DNS forest name.
        /// </summary>
        NlDnsForestName = 3,

        /// <summary>
        ///  The DnsDomainInfo member is a DNS forest name alias.
        /// </summary>
        NlDnsForestNameAlias = 4,

        /// <summary>
        ///  The DnsDomainInfo member is a non-domain NC (application
        ///  NC) name. For a definition of application NC, see [MS-ADTS]
        ///  section.
        /// </summary>
        NlDnsNdncDomainName = 5,

        /// <summary>
        ///  The DnsDomainInfo member is a DNS record name that is
        ///  required to be deregistered. This is valid only for
        ///  deregistration in which the Register value is set to
        ///  FALSE. For the types of DNS record name, see [MS-ADTS]
        ///  section.
        /// </summary>
        NlDnsRecordName = 6,
    }

    /// <summary>
    ///  The NL_SOCKET_ADDRESS structure contains a socket address.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\235d0c09-b9db-44c5-9d4d-806919be657b.xml
    //  </remarks>
    public partial struct _NL_SOCKET_ADDRESS
    {

        /// <summary>
        ///  A pointer to an octet string. The format of the lpSockaddr
        ///  member when an IPv4 socket address is used is specified
        ///  in section. The format of the lpSockaddr member when
        ///  an IPv6 socket address is used is specified in section
        ///  .
        /// </summary>
        [Size("iSockaddrLength")]
        public byte[] lpSockaddr;

        /// <summary>
        ///  The length of  the octet string pointed to by lpSockaddr,
        ///  in bytes.
        /// </summary>
        public uint iSockaddrLength;
    }

    /// <summary>
    ///  The NETLOGON_DELTA_GROUP_MEMBER structure contains information
    ///  about members of a group by providing pointers to a
    ///  list of group members and their respective attributes.
    ///  This structure is used to replicate the group membership
    ///  data from the PDC to a BDC, as detailed in section
    ///  .All fields of this structure, except the fields detailed
    ///  following the structure definition, have the same meanings
    ///  as the identically named fields of the SAMPR_GET_MEMBERS_BUFFER
    ///  structure, as specified in [MS-SAMR] section. The
    ///  last four fields of the structure (DummyLong1, DummyLong2,
    ///  DummyLong3, and DummyLong4) are not found in [MS-SAMR].
    /// </summary>
    //  <remarks>
    //   MS-NRPC\3d4c5636-1f2a-4896-a717-d8598ffaffab.xml
    //  </remarks>
    public partial struct _NETLOGON_DELTA_GROUP_MEMBER
    {

        /// <summary>
        ///  Members member.
        /// </summary>
        [Size("MemberCount")]
        public uint[] Members;

        /// <summary>
        ///  Attributes member.
        /// </summary>
        [Size("MemberCount")]
        public uint[] Attributes;

        /// <summary>
        ///  MemberCount member.
        /// </summary>
        public uint MemberCount;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong1;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong2;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong4;
    }

    /// <summary>
    ///  The NETLOGON_LSA_POLICY_INFO structure defines Local
    ///  Security Authority (LSA) policy information as an unsigned
    ///  character buffer. For details, see [LSAPOLICY] and
    ///  [MS-LSAD].
    /// </summary>
    //  <remarks>
    //   MS-NRPC\4676ac83-2099-4e33-b680-502fd8a32dbd.xml
    //  </remarks>
    public partial struct _NETLOGON_LSA_POLICY_INFO
    {

        /// <summary>
        ///  This field is not used, and is set to zero.
        /// </summary>
        public uint LsaPolicySize;

        /// <summary>
        ///  This field is not used, and is initialized to NULL.
        /// </summary>
        [Size("LsaPolicySize")]
        public byte[] LsaPolicy;
    }

    /// <summary>
    ///  The STRING structure contains the length, the maximum
    ///  length, and a pointer to a buffer containing the string.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\4a896c09-a39c-4878-94f0-9e4f8a419f32.xml
    //  </remarks>
    public partial struct _STRING
    {

        /// <summary>
        ///  The length of the data pointed to by Buffer, in bytes.
        /// </summary>
        public ushort Length;

        /// <summary>
        ///  The total allocated length of the data pointed to by
        ///  Buffer, in bytes.The value is ignored by the windows_nt_4_0
        ///  implementation.
        /// </summary>
        public ushort MaximumLength;

        /// <summary>
        ///  A pointer to a buffer containing the character string.
        /// </summary>
        [Length("Length")]
        [Size("MaximumLength")]
        public byte[] Buffer;
    }

    /// <summary>
    ///  The UAS_INFO_0 structure was for the support of LAN
    ///  Manager products and is beyond the scope of this document.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\5140443c-ef1e-48e8-b9c7-15d34cb91423.xml
    //  </remarks>
    public partial struct _UAS_INFO_0
    {

        /// <summary>
        ///  ComputerName member.
        /// </summary>
        [Inline()]
        [StaticSize(16, StaticSizeMode.Elements)]
        public byte[] ComputerName;

        /// <summary>
        ///  TimeCreated member.
        /// </summary>
        public uint TimeCreated;

        /// <summary>
        ///  SerialNumber member.
        /// </summary>
        public uint SerialNumber;
    }

    /// <summary>
    ///  The NL_TRUST_PASSWORD structure defines a buffer for
    ///  carrying a computer account password, or a trust password,
    ///  to be transmitted over the wire. This structure was
    ///  introduced in windows_2000_server and is present in
    ///  windows_xp, windows_server_2003, windows_vista, longhorn_server,
    ///  windows_7, and windows_server_7. It is transported
    ///  as an input parameter to the NetrServerPasswordSet2
    ///  method, as specified in section. Domain members use
    ///  NetrServerPasswordSet2 to change their computer account
    ///  password. The primary domain controller uses NetrServerPasswordSet2
    ///  to change trust passwords for all directly trusted
    ///  domains. The NL_TRUST_PASSWORD structure is encrypted
    ///  using the RC4 algorithm before it is sent over the
    ///  wire.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\52d5bd86-5caf-47aa-aae4-cadf7339ec83.xml
    //  </remarks>
    public partial struct _NL_TRUST_PASSWORD
    {

        /// <summary>
        ///  Array of Unicode characters that is treated as a byte
        ///  buffer containing the password, as follows:For a computer
        ///  account password, the buffer has the following format.Computer
        ///  account password buffer format The first (512 - Length)
        ///  bytes MUST be randomly generated data that serves as
        ///  an additional source of entropy during encryption.
        ///  The last Length bytes of the buffer MUST contain the
        ///  clear text password.For a domaintrust password, the
        ///  buffer has the following format:Domain trust password
        ///  buffer formatThe last Length bytes of the buffer contain
        ///  the clear text password. The 12 bytes preceding the
        ///  password are filled with the password version information
        ///  as defined below. The rest of the buffer is filled
        ///  with randomly generated data.The PasswordVersion part
        ///  of the preceding diagram has the following format:Password
        ///  version buffer formatWhere ReservedField, PasswordVersionNumber,
        ///  and PasswordVersionPresent are the fields of the NL_PASSWORD_VERSION
        ///  structure, as specified in section. The PasswordVersionPresent
        ///  field is used to indicate whether the buffer contains
        ///  a computer account password or a trust password: If
        ///  the value of the PasswordVersionPresent field is 0x02231968,
        ///  then the buffer contains a trust password; otherwise
        ///  the buffer contains a computer account password.
        /// </summary>
        [Inline()]
        [StaticSize(256, StaticSizeMode.Elements)]
        public ushort[] Buffer;

        /// <summary>
        ///  The length of the password, in bytes.
        /// </summary>
        public uint Length;
    }

    /// <summary>
    ///  The NLPR_QUOTA_LIMITS structure defines a set of system
    ///  resources that are available to a domain user.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\55d7ea86-c4cc-4a5e-8414-426673cb77d1.xml
    //  </remarks>
    public partial struct _NLPR_QUOTA_LIMITS
    {

        /// <summary>
        ///  Specifies the number of bytes of paged pool memory assigned
        ///  to the user. The paged pool is an area of system memory
        ///  (physical memory used by the operating system) for
        ///  objects that can be written to disk when they are not
        ///  being used.
        /// </summary>
        public uint PagedPoolLimit;

        /// <summary>
        ///  Specifies the number of bytes of nonpaged pool memory
        ///  assigned to the user. The nonpaged pool is an area
        ///  of system memory for objects that cannot be written
        ///  to disk but MUST remain in physical memory as long
        ///  as they are allocated.
        /// </summary>
        public uint NonPagedPoolLimit;

        /// <summary>
        ///  Specifies the minimum set size assigned to the user.
        ///  The working set of a process is the set of memory pages
        ///  currently visible to the process in physical RAM memory.
        ///  These pages are present in memory when the application
        ///  is running and available for an application to use
        ///  without triggering a page fault.
        /// </summary>
        public uint MinimumWorkingSetSize;

        /// <summary>
        ///  Specifies the maximum set size assigned to the user.
        /// </summary>
        public uint MaximumWorkingSetSize;

        /// <summary>
        ///  Specifies the maximum size, in bytes, of the paging
        ///  file, which is a reserved space on disk that backs
        ///  up committed physical memory on the computer.
        /// </summary>
        public uint PagefileLimit;

        /// <summary>
        ///  SHOULD be set to zero and MUST be ignored on receipt.
        /// </summary>
        public _OLD_LARGE_INTEGER Reserved;
    }

    /// <summary>
    ///  The CYPHER_BLOCK structure defines an encrypted eight-character
    ///  string. The type of encryption used is application-dependent.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\56a8d298-dbeb-4eb5-ad98-76536ec352f8.xml
    //  </remarks>
    public partial struct _CYPHER_BLOCK
    {

        /// <summary>
        ///  An encrypted eight-character string.
        /// </summary>
        [Inline()]
        [StaticSize(8, StaticSizeMode.Elements)]
        public byte[] data;
    }

    /// <summary>
    ///  The NETLOGON_LOGOFF_UAS_INFO structure was for the support
    ///  of LAN Manager products and is beyond the scope of
    ///  this document.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\60f42175-eff8-40cd-a083-6ee43a64fcbd.xml
    //  </remarks>
    public partial struct _NETLOGON_LOGOFF_UAS_INFO
    {

        /// <summary>
        ///  Duration member.
        /// </summary>
        public uint Duration;

        /// <summary>
        ///  LogonCount member.
        /// </summary>
        public ushort LogonCount;
    }

    /// <summary>
    ///  The NLPR_MODIFIED_COUNT structure specifies a count
    ///  for the number of times an account's database has been
    ///  modified.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\75dd3699-8abc-45d2-9fb7-e7a9a78779f5.xml
    //  </remarks>
    public partial struct _NLPR_MODIFIED_COUNT
    {

        /// <summary>
        ///  An OLD_LARGE_INTEGER structure, as specified in [MS-SAMR]
        ///  section , that contains the number of modifications
        ///  made to the database since its creation. This value
        ///  is the database serial number.
        /// </summary>
        public _OLD_LARGE_INTEGER ModifiedCount;
    }

    /// <summary>
    ///  The NETLOGON_VALIDATION_UAS_INFO structure was for the
    ///  support of LAN Manager products and is beyond the scope
    ///  of this document.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\7bd204f6-aba3-464c-b028-55e49f7999a0.xml
    //  </remarks>
    public partial struct _NETLOGON_VALIDATION_UAS_INFO
    {

        /// <summary>
        ///  usrlog1_eff_name member.
        /// </summary>
        [String()]
        public string usrlog1_eff_name;

        /// <summary>
        ///  usrlog1_priv member.
        /// </summary>
        public uint usrlog1_priv;

        /// <summary>
        ///  usrlog1_auth_flags member.
        /// </summary>
        public uint usrlog1_auth_flags;

        /// <summary>
        ///  usrlog1_num_logons member.
        /// </summary>
        public uint usrlog1_num_logons;

        /// <summary>
        ///  usrlog1_bad_pw_count member.
        /// </summary>
        public uint usrlog1_bad_pw_count;

        /// <summary>
        ///  usrlog1_last_logon member.
        /// </summary>
        public uint usrlog1_last_logon;

        /// <summary>
        ///  usrlog1_last_logoff member.
        /// </summary>
        public uint usrlog1_last_logoff;

        /// <summary>
        ///  usrlog1_logoff_time member.
        /// </summary>
        public uint usrlog1_logoff_time;

        /// <summary>
        ///  usrlog1_kickoff_time member.
        /// </summary>
        public uint usrlog1_kickoff_time;

        /// <summary>
        ///  usrlog1_password_age member.
        /// </summary>
        public uint usrlog1_password_age;

        /// <summary>
        ///  usrlog1_pw_can_change member.
        /// </summary>
        public uint usrlog1_pw_can_change;

        /// <summary>
        ///  usrlog1_pw_must_change member.
        /// </summary>
        public uint usrlog1_pw_must_change;

        /// <summary>
        ///  usrlog1_computer member.
        /// </summary>
        [String()]
        public string usrlog1_computer;

        /// <summary>
        ///  usrlog1_domain member.
        /// </summary>
        [String()]
        public string usrlog1_domain;

        /// <summary>
        ///  usrlog1_script_path member.
        /// </summary>
        [String()]
        public string usrlog1_script_path;

        /// <summary>
        ///  usrlog1_reserved1 member.
        /// </summary>
        public uint usrlog1_reserved1;
    }

    /// <summary>
    ///  The NL_OSVERSIONINFO_V1 structure specifies the values
    ///  used to update the operatingSystemVersion and operatingSystem
    ///  attributes on the client's computer account object
    ///  in Active Directory on a normal (writable) DC.The normal
    ///  (writable) DC cannot be a windows_server_2003 or a
    ///  windows_2000_serverDC.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\98229ab4-425a-41d8-87ce-48aacb302907.xml
    //  </remarks>
    public partial struct _NL_OSVERSIONINFO_V1
    {

        /// <summary>
        ///  The size, in bytes, of this data structure. Set this
        ///  member to sizeof(NL_OSVERSIONINFO_V1).
        /// </summary>
        public uint dwOSVersionInfoSize;

        /// <summary>
        ///  The major version number of the operating system. This
        ///  member can be one of the following values.
        /// </summary>
        public dwMajorVersion_Values dwMajorVersion;

        /// <summary>
        ///  The minor version number of the operating system. This
        ///  member can be one of the following values.
        /// </summary>
        public dwMinorVersion_Values dwMinorVersion;

        /// <summary>
        ///  The build number of the operating system.
        /// </summary>
        public uint dwBuildNumber;

        /// <summary>
        ///  The operating system platform. This member can be 0x00000002.
        /// </summary>
        public uint dwPlatformId;

        /// <summary>
        ///  A null-terminated string, such as "Service Pack 3",
        ///  that indicates the latest service pack installed on
        ///  the system. If no service pack has been installed,
        ///  the string is empty.
        /// </summary>
        [Inline()]
        [StaticSize(128, StaticSizeMode.Elements)]
        public ushort[] szCSDVersion;

        /// <summary>
        ///  The major version number of the latest service pack
        ///  installed on the system. For example, for "Service
        ///  Pack 3", the major version number is 3. If no service
        ///  pack has been installed, the value is 0.
        /// </summary>
        public ushort wServicePackMajor;

        /// <summary>
        ///  The minor version number of the latest service pack
        ///  installed on the system. For example, for "Service
        ///  Pack 3", the minor version number is 0.
        /// </summary>
        public ushort wServicePackMinor;

        /// <summary>
        ///  A bit mask that identifies the product suites available
        ///  on the system. This member can be a combination of
        ///  the following values.
        /// </summary>
        public wSuiteMask_Values wSuiteMask;

        /// <summary>
        ///  Any additional information about the system. This member
        ///  can be one of the following values.
        /// </summary>
        public wProductType_Values wProductType;

        /// <summary>
        ///  Reserved for future use.Added in longhorn_server.
        /// </summary>
        public byte wReserved;
    }

    /// <summary>
    /// Major version
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags()]
    public enum dwMajorVersion_Values : uint
    {

        /// <summary>
        ///  The operating system is windows_nt_4_0.
        /// </summary>
        WindowsNT4 = 4,

        /// <summary>
        ///  The operating system is windows_2000, windows_xp, windows_server_2003,
        ///  or windows_server_2003_r2.
        /// </summary>
        Windows2000_XP_2003_2003R2 = 5,

        /// <summary>
        ///  The operating system is longhorn_client or longhorn_server.
        /// </summary>
        WindowsLonghorn = 6,
    }

    /// <summary>
    /// Minor version
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum dwMinorVersion_Values : uint
    {

        /// <summary>
        ///  The operating system is windows_nt_4_0, windows_2000,
        ///  longhorn_client, or longhorn_server.
        /// </summary>
        WindowsNT4_2000_Longhorn = 0,

        /// <summary>
        ///  The operating system is windows_xp.
        /// </summary>
        WindowsXP = 1,

        /// <summary>
        ///  The operating system is windows_xp_professional_x64,
        ///  windows_server_2003, or windows_server_2003_r2.
        /// </summary>
        WindowsXPx64_2003_2003R2 = 2,
    }

    /// <summary>
    /// Suite mask
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum wSuiteMask_Values : ushort
    {

        /// <summary>
        ///  Microsoft BackOffice components are installed.
        /// </summary>
        VER_SUITE_BACKOFFICE = 0x00000004,

        /// <summary>
        ///  windows_server_2003_web_edition is installed.
        /// </summary>
        VER_SUITE_BLADE = 0x00000400,

        /// <summary>
        ///  windows_server_2003_compute_cluster is installed.
        /// </summary>
        VER_SUITE_COMPUTE_SERVER = 0x00004000,

        /// <summary>
        ///  windows_2000_datacenter_server, windows_server_2003_datacenter_edition,
        ///  or longhorn_server_datacenter_editionis installed.
        /// </summary>
        VER_SUITE_DATACENTER = 0x00000080,

        /// <summary>
        ///  windows_nt_server_4_0_enterprise_edition, windows_2000_advanced_server,,
        ///  windows_server_2003_enterprise_edition or longhorn_server_enterprise_editionis
        ///  installed. For this bit flag, see Remarks section.
        /// </summary>
        VER_SUITE_ENTERPRISE = 0x00000002,

        /// <summary>
        ///  windows_xp Embedded is installed.
        /// </summary>
        VER_SUITE_EMBEDDEDNT = 0x00000040,

        /// <summary>
        ///  windows_xp_home_edition, longhorn_client Home Basic,
        ///  or longhorn_client Home Premium is installed.
        /// </summary>
        VER_SUITE_PERSONAL = 0x00000200,

        /// <summary>
        ///  Remote Desktop is supported, but only one interactive
        ///  session is supported. This value is set unless the
        ///  system is running in application server mode.
        /// </summary>
        VER_SUITE_SINGLEUSERTS = 0x00000100,

        /// <summary>
        ///  Microsoft Small Business Server was once installed on
        ///  the system, but may have been upgraded to another version
        ///  of windows. For this bit flag, see Remarks section.
        /// </summary>
        VER_SUITE_SMALLBUSINESS = 0x00000001,

        /// <summary>
        ///  Microsoft Small Business Server is installed with the
        ///  restrictive client license in force. For this bit flag,
        ///  see Remarks section.
        /// </summary>
        VER_SUITE_SMALLBUSINESS_RESTRICTED = 0x00000020,

        /// <summary>
        ///  wss_2003 or wss_2003_r2 is installed.
        /// </summary>
        VER_SUITE_STORAGE_SERVER = 0x00002000,

        /// <summary>
        ///  Terminal Services is installed. This value is always
        ///  set. If VER_SUITE_TERMINAL is set but VER_SUITE_SINGLEUSERTS
        ///  is not set, the system is running in application server
        ///  mode.
        /// </summary>
        VER_SUITE_TERMINAL = 0x00000010,
    }

    /// <summary>
    /// Product type
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum wProductType_Values : byte
    {

        /// <summary>
        ///  The system is a domain controller.
        /// </summary>
        VER_NT_DOMAIN_CONTROLLER = 0x0000002,

        /// <summary>
        ///  The system is a server. Note that a server that is also
        ///  a domain controller is reported as VER_NT_DOMAIN_CONTROLLER,
        ///  not VER_NT_SERVER.
        /// </summary>
        VER_NT_SERVER = 0x0000003,

        /// <summary>
        ///  The operating system is windows_nt_workstation_4_0,
        ///  windows_2000_professional,, windows_xp_home_edition,
        ///  windows_xp_professional, or longhorn_client.
        /// </summary>
        VER_NT_WORKSTATION = 0x0000001,
    }

    /// <summary>
    ///  The NETLOGON_INFO_4 structure defines information that
    ///  is returned as part of an administrative query of the
    ///  status of the Netlogon server, as detailed in the description
    ///  of the NetrLogonControl2Ex method in section. This
    ///  structure is used to convey information about the status
    ///  and properties of the secure channel to a DC in the
    ///  primary or directly trusteddomain containing the user
    ///  account specified by the caller of the NetrLogonControl2Ex
    ///  method.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\a2b4c311-1831-4de6-bf5e-fe8427323144.xml
    //  </remarks>
    public partial struct _NETLOGON_INFO_4
    {

        /// <summary>
        ///  A pointer to a null-terminated Unicode string that contains
        ///  the DNS or NetBIOS name of a DC that is used on the
        ///  secure channel for the primary or directly trusted
        ///  domain containing the specified user account. The name
        ///  is the fully qualified domain name (FQDN) if the DC
        ///  was discovered using the discovery mechanism based
        ///  on the DNS query and LDAP ping ([MS-ADTS] section ).
        ///  The name is the NetBIOS name if the DC was discovered
        ///  using the mailslot-based mechanism ([MS-ADTS] section
        ///  ).
        /// </summary>
        [String()]
        public string netlog4_trusted_dc_name;

        /// <summary>
        ///  A pointer to a null-terminated Unicode string that contains
        ///  the NetBIOS name of the primary or directly trusted
        ///  domain containing the specified user account.
        /// </summary>
        [String()]
        public string netlog4_trusted_domain_name;
    }

    /// <summary>
    ///  The NETLOGON_CONTROL_DATA_INFORMATION union is used
    ///  as input to the NetrLogonControl2 method, as specified
    ///  in section , and the NetrLogonControl2Ex method, as
    ///  specified in section. This union selects a data type,
    ///  based on the FunctionCode parameter passed to the method.
    ///  For details about FunctionCode values, see NetrLogonControl2Ex,
    ///  section.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\ab0e203d-5f69-469c-8c57-7b6a0b1308b9.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _NETLOGON_CONTROL_DATA_INFORMATION
    {

        /// <summary>
        ///  A pointer to a null-terminated Unicode string that contains
        ///  a trusteddomain name. Switched on the DWORD ([MS-DTYP]
        ///  section) values 0x00000005, 0x00000006, 0x00000009,
        ///  and 0x0000000A. The DWORD values are equivalent to
        ///  FunctionCode values. For a complete list of the Netlogon
        ///  function codes and their associated meanings, see NetrLogonControl2Ex,
        ///  section.
        /// </summary>
        [Case("5,6,9,10")]
        [String()]
        public string TrustedDomainName;

        /// <summary>
        ///  A DWORD that contains an implementation-specific debug
        ///  flag. Switched on the value 0x0000FFFE.
        /// </summary>
        [Case("65534")]
        public uint DebugFlag;

        /// <summary>
        ///  A pointer to null-terminated Unicode string that contains
        ///  a user name. Switched on the DWORD value 0x00000008.
        /// </summary>
        [Case("8")]
        [String()]
        public string UserName;
    }

    /// <summary>
    ///  The NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES_V1 structure
    ///  specifies the values returned from the normal (writable)
    ///  DC.The normal (writable) DC cannot be a windows_2000_server
    ///  or a windows_server_2003DC.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\ab43a762-24e2-45c3-8701-1838589d80fe.xml
    //  </remarks>
    public partial struct _NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES_V1
    {

        /// <summary>
        ///  The NetBIOS name of the writable domain controller receiving
        ///  NetrChainSetClientAttributes.The read-only domain controller
        ///  (RODC) that invoked the method NetrChainSetClientAttributes
        ///  will attempt to replicate the computer account object
        ///  from HubName to itself, ignoring errors.
        /// </summary>
        [String()]
        public string HubName;

        /// <summary>
        ///  The client's DNS host name, if any, from the attribute
        ///  dNSHostName on the client's computer account object
        ///  in Active Directory on the writable domain controller.
        ///  If there was an update to the attribute dNSHostName
        ///  by the writable domain controller as a result of receiving
        ///  NetrChainSetClientAttributes, this value will hold
        ///  the previous value of that attribute.
        /// </summary>
        [String()]
        public string[] OldDnsHostName;

        /// <summary>
        ///  The supported encryption algorithms received from the
        ///  NetrLogonGetDomainInfo request, in the SupportedEncType
        ///  field in the NETLOGON_DOMAIN_INFO structure.Added in
        ///  longhorn_server.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public uint[] SupportedEncTypes;
    }

    /// <summary>
    ///  The NETLOGON_DUMMY1 union serves as a placeholder.windows
    ///  never uses this structure.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\ab6e857a-df5e-4e3d-af1e-a878abd37b7d.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct NETLOGON_DUMMY1
    {

        /// <summary>
        ///  The field is selected when the switched DWORD ([MS-DTYP]
        ///  section) value is 1.
        /// </summary>
        [Case("1")]
        public uint Dummy;
    }

    /// <summary>
    ///  The NETLOGON_INFO_1 structure defines information returned
    ///  as part of an administrative query, as detailed in
    ///  the description of the NetrLogonControl2Ex method in
    ///  section. This structure is used to convey information
    ///  about the state and properties of the secure channel
    ///  to a DC in the primary domain of the queried server.
    ///  Additionally, for windows_nt_4_0backup domain controllers,
    ///  this structure contains information about the state
    ///  of the database synchronization.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\afeb873c-4826-4beb-a35c-ae73a708b108.xml
    //  </remarks>
    public partial struct _NETLOGON_INFO_1
    {

        /// <summary>
        ///  A set of bit flags that have the following meanings.
        ///  A flag is TRUE (or set) if its value is equal to 1.
        ///  The value is constructed from zero or more bit flags
        ///  from the following table.
        /// </summary>
        public uint netlog1_flags;

        /// <summary>
        ///  The integer value that indicates the connection status
        ///  (section) of the secure channel to a DC in the primary
        ///  domain of the queried server. See section  for more
        ///  information.
        /// </summary>
        public uint netlog1_pdc_connection_status;
    }

    /// <summary>
    ///  The LM_CHALLENGE structure carries a LAN Manager authentication
    ///  challenge.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\bb8eca90-41bc-4ca4-8419-defdf043e01b.xml
    //  </remarks>
    public partial struct LM_CHALLENGE
    {

        /// <summary>
        ///  A string of eight characters that contains a LAN Manager
        ///  authentication challenge, which is an unencrypted nonce.
        /// </summary>
        [Inline()]
        [StaticSize(8, StaticSizeMode.Elements)]
        public byte[] data;
    }

    /// <summary>
    ///  The NETLOGON_INFO_3 structure defines information returned
    ///  as part of an administrative query of the status of
    ///  the Netlogon server, as detailed in the description
    ///  of the NetrLogonControl2Ex method in section. This
    ///  structure is used to return the number of NTLM logons
    ///  attempted on the queried server since the last restart.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\be92a2cd-950c-4b26-8cd3-475065406b92.xml
    //  </remarks>
    public partial struct _NETLOGON_INFO_3
    {

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        /// </summary>
        public netlog3_flags_Values netlog3_flags;

        /// <summary>
        ///  The number of NTLM logon attempts made on the server
        ///  since the last restart.
        /// </summary>
        public uint netlog3_logon_attempts;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        /// </summary>
        public netlog3_reserved1_Values netlog3_reserved1;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        /// </summary>
        public netlog3_reserved2_Values netlog3_reserved2;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        /// </summary>
        public netlog3_reserved3_Values netlog3_reserved3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        /// </summary>
        public netlog3_reserved4_Values netlog3_reserved4;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        /// </summary>
        public netlog3_reserved5_Values netlog3_reserved5;
    }

    /// <summary>
    /// netlog3 flags
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum netlog3_flags_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// netlog3 reserved
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum netlog3_reserved1_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// netlog3 reserved
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum netlog3_reserved2_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// netlog3 reserved
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum netlog3_reserved3_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// netlog3 reserved
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum netlog3_reserved4_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// netlog reserved
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum netlog3_reserved5_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The NETLOGON_INFO_2 structure defines information returned
    ///  as part of an administrative query of the status of
    ///  the Netlogon server, as detailed in the description
    ///  of the NetrLogonControl2Ex method in section. This
    ///  structure is used to convey information about the status
    ///  and properties of the secure channel to a DC in the
    ///  primary or directly trusteddomain specified by the
    ///  caller of the NetrLogonControl2Ex method.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\c48a8700-4c17-4afb-8866-f0cfc0c0a671.xml
    //  </remarks>
    public partial struct _NETLOGON_INFO_2
    {

        /// <summary>
        ///  A set of bit flags describing the following control
        ///  query responses from the DC. A flag is TRUE (or set)
        ///  if its value is equal to 1. The value is constructed
        ///  from zero or more bit flags from the following table.
        /// </summary>
        public uint netlog2_flags;

        /// <summary>
        ///  Unless the C bit is set in netlog2_flags field, this
        ///  field indicates the connection status (section) of
        ///  the secure channel to a DC in the primary domain of
        ///  the queried server. If the C bit is set in netlog2_flags
        ///  field, this field indicates the connection status of
        ///  verifying the secure channel to the DC in the specified
        ///  domain (specified by the caller of the NetrLogonControl2Ex
        ///  method; see section  for more information).
        /// </summary>
        public uint netlog2_pdc_connection_status;

        /// <summary>
        ///  A pointer to a null-terminated Unicode string that contains
        ///  the DNS or NetBIOS name of the DC used on the secure
        ///  channel for the specified domain. The name is the fully
        ///  qualified domain name (FQDN) if the DC was discovered
        ///  using the discovery mechanism based on the DNS query
        ///  and LDAP ping ([MS-ADTS] section ). The name is the
        ///  NetBIOS name if the DC was discovered using the mailslot-based
        ///  mechanism ([MS-ADTS] section ).
        /// </summary>
        [String()]
        public string netlog2_trusted_dc_name;

        /// <summary>
        ///  An integer value that indicates the connection status
        ///  (section) of the secure channel to the DC in the specified
        ///  domain.
        /// </summary>
        public uint netlog2_tc_connection_status;
    }

    /// <summary>
    ///  The NLPR_CR_CIPHER_VALUE structure defines an encrypted
    ///  string buffer that contains the value of an LSA Secret
    ///  Object as specified in [MS-LSAD].
    /// </summary>
    //  <remarks>
    //   MS-NRPC\c6fcb497-4b10-4595-8fe7-96bf961ba292.xml
    //  </remarks>
    public partial struct _NLPR_CR_CIPHER_VALUE
    {

        /// <summary>
        ///  The length, in bytes, of the used portion of the buffer.
        /// </summary>
        public uint Length;

        /// <summary>
        ///  The maximum length, in bytes, of the buffer.
        /// </summary>
        public uint MaximumLength;

        /// <summary>
        ///  A pointer to a buffer that contains the secret data
        ///  encrypted with the session key used on the secure channel
        ///  between the client and the server exchanging this data
        ///  structure. The encryption algorithm is RC4 if the flag
        ///  C is set in the negotiated flags between the client
        ///  and the server as detailed in section ; otherwise the
        ///  encryption algorithm is DES.
        /// </summary>
        [Length("Length")]
        [Size("MaximumLength")]
        public byte[] Buffer;
    }

    /// <summary>
    ///  The NL_PASSWORD_VERSION structure defines a password
    ///  version number that is used to distinguish between
    ///  different versions of information passed in the Buffer
    ///  field of the NL_TRUST_PASSWORD structure. The NL_PASSWORD_VERSION
    ///  structure is prepended to the password in the buffer
    ///  of NL_TRUST_PASSWORD. This structure is only used for
    ///  interdomain trust accounts.This structure was introduced
    ///  in windows_2000 and is present in windows_xp, windows_server_2003,
    ///  windows_vista, windows_server_2008, windows_7, and
    ///  windows_server_7.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\cba0e87e-b91f-411b-9be8-6fd321e126b7.xml
    //  </remarks>
    public partial struct _NL_PASSWORD_VERSION
    {

        /// <summary>
        ///  MUST be set to zero when sent and MUST be ignored on
        ///  receipt.
        /// </summary>
        public ReservedField_Values ReservedField;

        /// <summary>
        ///  Integer value that contains the current password version
        ///  number. The password version number is incremented
        ///  by one when a new password is generated; the value
        ///  for the first password is one.
        /// </summary>
        public uint PasswordVersionNumber;

        /// <summary>
        ///  MUST be 0x02231968. This member is relevant only for
        ///  server-to-server communication.MUST be 0x02231968,
        ///  which is a constant used to indicate that the password
        ///  version number is present and is stored in PasswordVersionNumber.
        /// </summary>
        public PasswordVersionPresent_Values PasswordVersionPresent;
    }

    /// <summary>
    /// Reserved field
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum ReservedField_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// Password version present
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum PasswordVersionPresent_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x02231968,
    }

    /// <summary>
    ///  The NETLOGON_CREDENTIAL structure contains 8 bytes of
    ///  data that have two distinct uses: for session-key negotiation
    ///  and for building a Netlogon authenticator.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\d55e2632-7163-4f6c-b662-4b870e8cc1cd.xml
    //  </remarks>
    public partial struct _NETLOGON_CREDENTIAL
    {

        /// <summary>
        ///  The meaning of the 8 bytes of data contained in this
        ///  structure is determined by the following:When session-key
        ///  negotiation is performed, the data field carries an
        ///  8-byte challenge. Also see section.When the NETLOGON_CREDENTIAL
        ///  is used as part of a NETLOGON_AUTHENTICATOR structure,
        ///  the data field carries 8 bytes of encrypted data, as
        ///  specified in sections  and .
        /// </summary>
        [Inline()]
        [StaticSize(8, StaticSizeMode.Elements)]
        public byte[] data;
    }

    /// <summary>
    ///  The NETLOGON_VALIDATION_GENERIC_INFO2 structure defines
    ///  a structure that contains account information in binary
    ///  format. Microsoft implementations of authentication
    ///  protocols make use of this structure to return generic
    ///  account information upon successful logon validation.
    ///  For an example of using the NETLOGON_VALIDATION_GENERIC_INFO2
    ///  structure, see any of the examples in [MS-APDS].
    /// </summary>
    //  <remarks>
    //   MS-NRPC\e89f8f5b-0985-41a0-b110-1ae0ea2bd183.xml
    //  </remarks>
    public partial struct _NETLOGON_VALIDATION_GENERIC_INFO2
    {

        /// <summary>
        ///  An integer value that contains the length of the data
        ///  referenced by ValidationData, in bytes.
        /// </summary>
        public uint DataLength;

        /// <summary>
        ///  A pointer to a buffer that contains the logon validation
        ///  information.
        /// </summary>
        [Size("DataLength")]
        public byte[] ValidationData;
    }

    /// <summary>
    ///  The GROUP_MEMBERSHIP structure identifies the group
    ///  to which an account belongs.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\efb926f6-48c3-4c29-8a45-86a67bfaaf27.xml
    //  </remarks>
    public partial struct _GROUP_MEMBERSHIP
    {

        /// <summary>
        ///  The relative identifier (RID) for a particular group.
        /// </summary>
        public uint RelativeId;

        /// <summary>
        ///  A set of values that describe the group membership attributes
        ///  set for the RID specified in RelativeId. The value
        ///  is constructed from one or more bit flags from the
        ///  following table.
        /// </summary>
        public uint Attributes;
    }

    /// <summary>
    ///  The NETLOGON_CAPABILITIES union carries the supported
    ///  Netlogon capabilities.This union is supported in windows_7,
    ///  and windows_server_7.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\f03cad67-077f-4042-80b0-cdc38dca9968.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _NETLOGON_CAPABILITIES
    {

        /// <summary>
        ///  A 32-bit set of bit flags that identify the server's
        ///  capabilities (section ). This field is selected when
        ///  the switched DWORD ([MS-DTYP] section) value is 1.
        /// </summary>
        [Case("1")]
        public uint ServerCapabilities;
    }


    /// <summary>
    ///  The NETLOGON_RENAME_ALIAS structure specifies a rename
    ///  of an alias.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\15426b42-7d07-40e4-bca7-933823d1b40e.xml
    //  </remarks>
    public partial struct _NETLOGON_DELTA_RENAME_ALIAS
    {

        /// <summary>
        ///  An RPC_UNICODE_STRING structure, as specified in [MS-DTYP]
        ///  section , that contains the previous name of the alias.
        /// </summary>
        public _RPC_UNICODE_STRING OldName;

        /// <summary>
        ///  An RPC_UNICODE_STRING structure, as specified in [MS-DTYP]
        ///  section , that contains the new name to assign to the
        ///  alias.
        /// </summary>
        public _RPC_UNICODE_STRING NewName;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString1;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString2;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString3;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString4;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong1;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong2;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong4;
    }

    /// <summary>
    ///  The NL_SITE_NAME_EX_ARRAY structure defines an array
    ///  of site and subnet names. This structure extends the
    ///  NL_SITE_NAME_ARRAY structure by adding an array of
    ///  subnets that correspond to the sites.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\2b40b2eb-480c-42e9-b864-3de716db892d.xml
    //  </remarks>
    public partial struct _NL_SITE_NAME_EX_ARRAY
    {

        /// <summary>
        ///  The number of entries in SiteNames and SubnetNames.
        /// </summary>
        public uint EntryCount;

        /// <summary>
        ///  A pointer to an array of null-terminated Unicode strings
        ///  that contain site names. For information about sites,
        ///  see [MS-ADTS] section.
        /// </summary>
        [Size("EntryCount")]
        public _RPC_UNICODE_STRING[] SiteNames;

        /// <summary>
        ///  A pointer to an array of null-terminated Unicode strings
        ///  that contain subnet names. For information about subnets,
        ///  see [MS-ADTS] section.
        /// </summary>
        [Size("EntryCount")]
        public _RPC_UNICODE_STRING[] SubnetNames;
    }

    /// <summary>
    ///  The LM_OWF_PASSWORD structure carries a one-way function
    ///  (OWF) of a LAN Manager password. The LM_OWF_PASSWORD
    ///  structure MAY be encrypted, as specified by each method
    ///  that uses this structure. See the NetrServerPasswordSet
    ///  method in section  for encryption information.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\30a12be7-a4ed-4ef2-bd88-83961edb8f26.xml
    //  </remarks>
    public partial struct _LM_OWF_PASSWORD
    {

        /// <summary>
        ///  An array of CYPHER_BLOCK data structures that contains
        ///  the LMOWFv1 of a password. LMOWFv1 is specified in
        ///  NTLM v1 Authentication in [MS-NLMP] section.
        /// </summary>
        [Inline()]
        [StaticSize(2, StaticSizeMode.Elements)]
        public _CYPHER_BLOCK[] data;
    }

    /// <summary>
    ///  The NETLOGON_DELTA_ALIAS structure contains information
    ///  about a SAM alias. This structure is used to replicate
    ///  the SAM alias data from the PDC to a BDC.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\38155d03-b5dc-445d-9fcb-a6938be5972d.xml
    //  </remarks>
    public partial struct _NETLOGON_DELTA_ALIAS
    {

        /// <summary>
        ///  An RPC_UNICODE_STRING structure, as specified in [MS-DTYP]
        ///  section , that contains the alias name.
        /// </summary>
        public _RPC_UNICODE_STRING Name;

        /// <summary>
        ///  The RID for the alias.
        /// </summary>
        public uint RelativeId;

        /// <summary>
        ///  A SECURITY_INFORMATION structure, as specified in [MS-DTYP]
        ///  section , that contains security settings for the alias.
        /// </summary>
        public uint SecurityInformation;

        /// <summary>
        ///  The size, in bytes, of the SecurityDescriptor field.
        /// </summary>
        public uint SecuritySize;

        /// <summary>
        ///  A pointer to a SECURITY_DESCRIPTOR structure, as specified
        ///  in [MS-DTYP] section , that describes the security
        ///  information for the alias object.
        /// </summary>
        [Size("SecuritySize")]
        public byte[] SecurityDescriptor;

        /// <summary>
        ///  An RPC_UNICODE_STRING structure, as specified in [MS-DTYP]
        ///  section , that contains the administrative comment
        ///  string for the alias.
        /// </summary>
        public _RPC_UNICODE_STRING Comment;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString2;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString3;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString4;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong1;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong2;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong4;
    }

    /// <summary>
    ///  The NETLOGON_WORKSTATION_INFO structure defines information
    ///  returned by the NetrLogonGetDomainInfo method, as specified
    ///  in . It is used to convey information about a member
    ///  workstation from the client side to the server side.This
    ///  structure is introduced in windows_2000 and is present
    ///  in windows_xp, windows_server_2003, windows_vista,
    ///  windows_server_2008, windows_7, and windows_server_2008_r2.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\3ae9e9a9-a303-4fa5-8e11-823d9e7e1e61.xml
    //  </remarks>
    public partial struct _NETLOGON_WORKSTATION_INFO
    {

        /// <summary>
        ///  A NETLOGON_LSA_POLICY_INFO structure, as specified in
        ///  section , that contains the LSA policy for this domain.
        /// </summary>
        public _NETLOGON_LSA_POLICY_INFO LsaPolicy;

        /// <summary>
        ///  A null-terminated Unicode string that contains the DNS
        ///  host name of the client.
        /// </summary>
        [String()]
        public string DnsHostName;

        /// <summary>
        ///  A null-terminated Unicode string that contains the name
        ///  of the site where the workstation resides.
        /// </summary>
        [String()]
        public string SiteName;

        /// <summary>
        ///  MUST be set to NULL and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        [String()]
        public string Dummy1;

        /// <summary>
        ///  MUST be set to NULL and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        [String()]
        public string Dummy2;

        /// <summary>
        ///  MUST be set to NULL and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        [String()]
        public string Dummy3;

        /// <summary>
        ///  MUST be set to NULL and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        [String()]
        public string Dummy4;

        /// <summary>
        ///  An OSVERSIONINFOEX ([MS-RPRN], section) structure that
        ///  contains the version number of the operating system
        ///  installed on the client machine.
        /// </summary>
        public _RPC_UNICODE_STRING OsVersion;

        /// <summary>
        ///  A null-terminated Unicode string that contains the name
        ///  of the operating system installed on the client machine.The
        ///  name of the client's operating system is used. The
        ///  following are the strings used by Windows: For windows_2000_professional
        ///  SKUs: "Windows 2000"For windows_2000_server SKUs: "Windows
        ///  2000 Server"For windows_xp_professional SKUs: "Windows
        ///  XP Professional"For windows_server_2003 SKUs: "Windows
        ///  Server 2003"For windows_vista and windows_7 SKUs: The
        ///  name of the product is used. For example, for client_windows_7ult,
        ///  the string "Windows 7 Ultimate" is used.For windows_server_2008
        ///  and windows_server_7 SKUs: The name of the product
        ///  is used. For example, for Windows Server 2008 Enterprise,
        ///  the string "Windows Server 2008 Enterprise" is used.
        ///  The DC that receives this data structure updates the
        ///  operatingSystem attribute of the client's machine account
        ///  object in Active Directory, as specified in [MS-ADA3],
        ///  section.
        /// </summary>
        public _RPC_UNICODE_STRING OsName;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  specified in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString3;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  specified in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString4;

        /// <summary>
        ///  A set of bit flags specifying workstation behavior.
        ///  A flag is TRUE (or set) if its value is equal to 1.
        ///  The value is constructed from zero or more bit flags
        ///  from the following table.
        /// </summary>
        public uint WorkstationFlags;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint KerberosSupportedEncryptionTypes;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public DummyLong3_Values DummyLong3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public DummyLong4_Values DummyLong4;
    }

    /// <summary>
    /// Dummy long
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum DummyLong2_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// Dummy long
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum DummyLong3_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// Dummy long
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum DummyLong4_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The NETLOGON_DELTA_DELETE_GROUP structure contains information
    ///  about a group to be deleted in the database. This structure
    ///  is used for replicating the SAM group data from the
    ///  PDC to a BDC, as detailed in section.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\3ef06890-0519-4f6e-889d-ec76cd865d7c.xml
    //  </remarks>
    public partial struct _NETLOGON_DELTA_DELETE_GROUP
    {

        /// <summary>
        ///  A null-terminated Unicode string that contains the name
        ///  of the group to delete.
        /// </summary>
        [String()]
        public string AccountName;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  specified in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString1;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  specified in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString2;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  specified in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString3;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  specified in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString4;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong1;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong2;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong4;
    }

    /// <summary>
    ///  The NETLOGON_DELTA_DELETE_USER structure contains information
    ///  about a user account to be deleted in the database.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\3fb60f5f-e33d-427b-b540-ac3b84342d5d.xml
    //  </remarks>
    public partial struct _NETLOGON_DELTA_DELETE_USER
    {

        /// <summary>
        ///  A null-terminated Unicode string that contains the name
        ///  of the user to delete.
        /// </summary>
        [String()]
        public string AccountName;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  specified in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString1;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  specified in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString2;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  specified in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString3;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  specified in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString4;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong1;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong2;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong4;
    }

    /// <summary>
    ///  The NL_SITE_NAME_ARRAY structure defines an array of
    ///  site names.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\4c5fb44a-6d68-420d-970e-68ad5436bbd3.xml
    //  </remarks>
    public partial struct _NL_SITE_NAME_ARRAY
    {

        /// <summary>
        ///  The number of entries in SiteNames.
        /// </summary>
        public uint EntryCount;

        /// <summary>
        ///  A pointer to an array of null-terminated RPC_UNICODE_STRING
        ///  strings that contain site names. For more information
        ///  about sites, see [MS-ADTS] section.
        /// </summary>
        [Size("EntryCount")]
        public _RPC_UNICODE_STRING[] SiteNames;
    }

    /// <summary>
    ///  The NETLOGON_SID_AND_ATTRIBUTES structure contains a
    ///  security identifier (SID) and its attributes.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\5454e6c6-f786-4327-8667-d542b5ebb7c7.xml
    //  </remarks>
    public partial struct _NETLOGON_SID_AND_ATTRIBUTES
    {

        /// <summary>
        ///  A pointer to a security identifier (SID).
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] Sid;

        /// <summary>
        ///  A set of bit flags that contains the set of security
        ///  attributes assigned to this SID. A bit is TRUE (or
        ///  set) if its value is equal to 1. The value is constructed
        ///  from one or more bit flags from the following table.
        /// </summary>
        public uint Attributes;
    }

    /// <summary>
    ///  The NETLOGON_WORKSTATION_INFORMATION union selects between
    ///  two parameters of type NETLOGON_WORKSTATION_INFO structure,
    ///  as specified in section , based on the value of the
    ///  Level parameter of the NetrLogonGetDomainInfo method,
    ///  as specified in section.: This structure was introduced
    ///  in windows_2000 and is present in windows_xp, windows_server_2003,
    ///  windows_vista, windows_server_2008, windows_7, and
    ///  windows_server_7.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\7520d43a-751d-4d52-9282-44bb48fcc6d4.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _NETLOGON_WORKSTATION_INFORMATION
    {

        /// <summary>
        ///  Field is selected when the switched DWORD ([MS-DTYP]
        ///  section) constant is 0x00000001.
        /// </summary>
        [Case("1")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_WORKSTATION_INFO[] WorkstationInfo;

        /// <summary>
        ///  Field is selected when the switched DWORD constant is
        ///  0x00000002. This field MUST be set such that the length
        ///  is zero and the pointer is NULL on transmission and
        ///  MUST be ignored on receipt.This field was created and
        ///  used in windows_2000 Beta 1 and is hard-coded to zero
        ///  length with a NULL pointer.
        /// </summary>
        [Case("2")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_WORKSTATION_INFO[] LsaPolicyInfo;
    }

    /// <summary>
    ///  The NETLOGON_AUTHENTICATOR structure defines an authentication
    ///  credential.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\76c93227-942a-4687-ab9d-9d972ffabdab.xml
    //  </remarks>
    public partial struct _NETLOGON_AUTHENTICATOR
    {

        /// <summary>
        ///  A NETLOGON_CREDENTIAL structure that contains the encrypted
        ///  portion of the authenticator.
        /// </summary>
        public _NETLOGON_CREDENTIAL Credential;

        /// <summary>
        ///  An integer value that contains the time of day at which
        ///  the client constructed this authentication credential,
        ///  represented as the number of elapsed seconds since
        ///  00:00:00 of January 1, 1970. The authenticator is constructed
        ///  just before making a call to a method that requires
        ///  its usage.
        /// </summary>
        public uint Timestamp;
    }

    /// <summary>
    ///  The NETLOGON_ONE_DOMAIN_INFO structure defines information
    ///  about a single domain. It is in turn contained in the
    ///  NETLOGON_DOMAIN_INFO structure, as specified in section
    ///  . The NETLOGON_DOMAIN_INFO structure describes domain
    ///  relationships and is generated as output from the NetrLogonGetDomainInfo
    ///  method, as specified in section.This structure was
    ///  introduced in windows_2000 and is present in windows_xp,
    ///  windows_server_2003, windows_vista, windows_server_2008,
    ///  windows_7, and windows_server_7.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\77cd9776-8612-4dab-9748-bfb07247fd4c.xml
    //  </remarks>
    public partial struct _NETLOGON_ONE_DOMAIN_INFO
    {

        /// <summary>
        ///  A null-terminated Unicode string that contains the NetBIOS
        ///  name of the domain being described. This field MUST
        ///  NOT be an empty string.
        /// </summary>
        public _RPC_UNICODE_STRING DomainName;

        /// <summary>
        ///  A null-terminated Unicode string that contains the DNSdomain
        ///  name for this domain. This field MUST NOT be an empty
        ///  string.
        /// </summary>
        public _RPC_UNICODE_STRING DnsDomainName;

        /// <summary>
        ///  A null-terminated Unicode string that contains the DNSforest
        ///  name for this domain.
        /// </summary>
        public _RPC_UNICODE_STRING DnsForestName;

        /// <summary>
        ///  A globally unique 128-bit identifier for this domain.
        /// </summary>
        public System.Guid DomainGuid;

        /// <summary>
        ///  The security identifier (SID) for this domain.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] DomainSid;

        /// <summary>
        ///  An RPC_UNICODE_STRING structure, as specified in [MS-DTYP]
        ///  section , which does not point to a Unicode string,
        ///  but in fact points to a buffer of size 16, in bytes,
        ///  in the following format.01234567891 01234567892 01234567893
        ///  01FlagsParentIndexTrustTypeTrustAttributesThis structure
        ///  is supplementary domaintrust information that contains
        ///  the following fields of a DS_DOMAIN_TRUSTSW structure:
        ///  Flags, ParentIndex, TrustType, and TrustAttributes.
        ///  For more information on usage in NetrLogonGetDomainInfo,
        ///  see section. For more information on the DS_DOMAIN_TRUSTSW
        ///  structure, see section.
        /// </summary>
        public _RPC_UNICODE_STRING TrustExtension;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString2;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString3;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString4;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public DummyLong1_Values DummyLong1;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public _NETLOGON_ONE_DOMAIN_INFO_DummyLong2_Values DummyLong2;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public _NETLOGON_ONE_DOMAIN_INFO_DummyLong3_Values DummyLong3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public _NETLOGON_ONE_DOMAIN_INFO_DummyLong4_Values DummyLong4;
    }

    /// <summary>
    /// Dummy long
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum DummyLong1_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// NETLOGON_ONE_DOMAIN_INFO dummy long
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum _NETLOGON_ONE_DOMAIN_INFO_DummyLong2_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// NETLOGON_ONE_DOMAIN_INFO dummy long
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum _NETLOGON_ONE_DOMAIN_INFO_DummyLong3_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// NETLOGON_ONE_DOMAIN_INFO dummy long
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum _NETLOGON_ONE_DOMAIN_INFO_DummyLong4_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  The NETLOGON_DELTA_ID_UNION union defines an account
    ///  identifier type that is selected based on the requested
    ///  database change.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\797e1033-d40b-400b-a71a-522f820bac33.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _NETLOGON_DELTA_ID_UNION
    {

        /// <summary>
        ///  A 32-bit RID whose type is selected when the following
        ///  delta types are switched: AddOrChangeDomain(1), AddOrChangeGroup(2),
        ///  RenameGroup(4), DeleteGroup(3), AddOrChangeUser(5),
        ///  DeleteUser(6), RenameUser(7), ChangeGroupMembership(8),
        ///  AddOrChangeAlias(9), DeleteAlias(10), RenameAlias(11),
        ///  ChangeAliasMembership(12), DeleteGroupByName(20), and
        ///  DeleteUserByName(21).
        /// </summary>
        [Case("1,2,3,4,5,6,7,8,9,10,11,12,20,21")]
        public uint Rid;

        /// <summary>
        ///  A pointer to a SID whose type is selected when the following
        ///  delta types are switched: AddOrChangeLsaPolicy(13),
        ///  AddOrChangeLsaDomain(14), DeleteLsaTDomain(15), AddOrChangeLsaAccount(16),
        ///  and DeleteLsaAccount(17).
        /// </summary>
        [Case("13,14,15,16,17")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] Sid;

        /// <summary>
        ///  A null-terminated Unicode string that contains an identifier
        ///  name. This identifier type is selected when the following
        ///  delta types are switched: AddOrChangeLsaSecret(18)
        ///  and DeleteLsaSecret(19).
        /// </summary>
        [Case("18,19")]
        [String()]
        public string Name;
    }

    /// <summary>
    ///  The DS_DOMAIN_TRUSTSW structure defines information
    ///  about a domaintrust. It is part of the NETLOGON_TRUSTED_DOMAIN_ARRAY
    ///  structure returned by the DsrEnumerateDomainTrusts
    ///  method, as specified in section. This structure contains
    ///  naming information and trust-related information for
    ///  a specific trusteddomain.This structure was introduced
    ///  in windows_2000 and is present in windows_xp, windows_server_2003,
    ///  windows_vista, windows_server_2008, windows_7, and
    ///  windows_server_7.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\7de9866e-d3ef-4a9f-98a5-c2dcff1e61c1.xml
    //  </remarks>
    public partial struct _DS_DOMAIN_TRUSTSW
    {

        /// <summary>
        ///  A pointer to a null-terminated Unicode string that contains
        ///  the NetBIOS name of the trusteddomain.
        /// </summary>
        [String()]
        public string NetbiosDomainName;

        /// <summary>
        ///  A pointer to a null-terminated Unicode string that contains
        ///  the fully qualified domain name (FQDN) of the trusted
        ///  domain.
        /// </summary>
        [String()]
        public string DnsDomainName;

        /// <summary>
        ///  A set of bit flags that defines the domaintrust attributes.
        ///  A flag is TRUE (or set) if its value is equal to 1.
        ///  The value is constructed from zero or more bit flags
        ///  from the following table.
        /// </summary>
        public uint Flags;

        /// <summary>
        ///  An integer value that contains the index in the NETLOGON_TRUSTED_DOMAIN_ARRAY
        ///  array (returned by the DsrEnumerateDomainTrusts method)
        ///  that corresponds to the parent domain of the domain
        ///  represented by this structure. This field is only set
        ///  if all of the following conditions are met:The A flag
        ///  was specified in the Flags parameter of the DsrEnumerateDomainTrusts
        ///  method. The Flags field of this structure, DS_DOMAIN_TRUSTSW,
        ///  does not contain the C flag. Otherwise, it MUST be
        ///  set to zero and MUST be ignored.
        /// </summary>
        public uint ParentIndex;

        /// <summary>
        ///  A set of bit flags describing the type of domain with
        ///  which the trust is associated. A flag is TRUE (or set)
        ///  if its value is equal to 1. TrustType MUST be one,
        ///  and only one, of the following bits.
        /// </summary>
        public uint TrustType;

        /// <summary>
        ///  A set of bit flags describing trust link attributes.
        ///  A flag is true (or set) if its value is equal to 1.
        ///  The value is constructed from zero or more bit flags
        ///  from the following table, with the exception that bit
        ///  F cannot be combined with E or D.
        /// </summary>
        public uint TrustAttributes;

        /// <summary>
        ///  A pointer to an SID structure that identifies the current
        ///  domain. If the TrustType field is set to C or D, the
        ///  value is 0.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] DomainSid;

        /// <summary>
        ///  A GUID that identifies the current domain.
        /// </summary>
        public System.Guid DomainGuid;
    }

    /// <summary>
    ///  The NETLOGON_DELTA_ACCOUNTS structure contains the settings
    ///  and privileges for a Local Security Authority (LSA)
    ///  account. This structure is used for replicating the
    ///  LSA account data from the primary domain controller
    ///  (PDC) to a backup domain controller (BDC).
    /// </summary>
    //  <remarks>
    //   MS-NRPC\7f60a1a7-99d8-49ba-91c0-6e027237b009.xml
    //  </remarks>
    public partial struct _NETLOGON_DELTA_ACCOUNTS
    {

        /// <summary>
        ///  The number of privileges associated with the LSA account.
        /// </summary>
        public uint PrivilegeEntries;

        /// <summary>
        ///  A bit flag describing the properties of the account
        ///  privileges. A flag is TRUE (or set) if its value is
        ///  equal to 1. PrivilegeControl MAY be the following value.
        /// </summary>
        public uint PrivilegeControl;

        /// <summary>
        ///  Pointer to an array of unsigned 32-bit values that contain
        ///  a set of bit flags describing each privilege's attributes.
        ///  An attribute is TRUE (or set) if its value is equal
        ///  to 1. The value is constructed from zero or more bit
        ///  flags from the following table.
        /// </summary>
        [Size("PrivilegeEntries")]
        public uint[] PrivilegeAttributes;

        /// <summary>
        ///  A pointer to an array of privilege names represented
        ///  as RPC_UNICODE_STRING structures. See [MS-DTYP] section
        ///   for a specification of the RPC_UNICODE_STRING structure.
        ///  The names of the privileges are implementation-specific.
        /// </summary>
        [Size("PrivilegeEntries")]
        public _RPC_UNICODE_STRING[] PrivilegeNames;

        /// <summary>
        ///  An NLPR_QUOTA_LIMITS structure that describes the account's
        ///  current quota settings. For more details about the
        ///  NLPR_QUOTA_LIMITS structure, see section.
        /// </summary>
        public _NLPR_QUOTA_LIMITS QuotaLimits;

        /// <summary>
        ///  A set of the following bit flags that specify the ways
        ///  in which the account is permitted to access the system
        ///  as detailed in POLICY_MODE_INTERACTIVE, POLICY_MODE_NETWORK,
        ///  POLICY_MODE_BATCH, POLICY_MODE_SERVICE, and POLICY_MODE_PROXY
        ///  of [MS-LSAD]. See [MS-LSAD] for the specification of
        ///  these bit values and allowed combinations.
        /// </summary>
        public uint SystemAccessFlags;

        /// <summary>
        ///  A SECURITY_INFORMATION structure, as specified in [MS-DTYP]
        ///  section , that specifies portions of a security descriptor
        ///  about the trusted domain.
        /// </summary>
        public uint SecurityInformation;

        /// <summary>
        ///  The size, in bytes, of the SecurityDescriptor field.
        /// </summary>
        public uint SecuritySize;

        /// <summary>
        ///  A pointer to a SECURITY_DESCRIPTOR structure, as specified
        ///  in [MS-DTYP] section , that describes the security
        ///  settings for the account object.
        /// </summary>
        [Size("SecuritySize")]
        public byte[] SecurityDescriptor;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString1;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString2;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString3;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString4;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong1;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong2;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong4;
    }

    /// <summary>
    ///  The NETLOGON_DELTA_DOMAIN structure contains information
    ///  about a domain. Most of the fields in this structure
    ///  are obtained by querying the database. This structure
    ///  is used to replicate the domain data from the PDC to
    ///  a BDC, as detailed in section.All fields of this structure,
    ///  except the fields detailed following the structure
    ///  definition, have the same meaning as the identically
    ///  named fields in the Domain Fields section ([MS-SAMR]
    ///  section ).
    /// </summary>
    //  <remarks>
    //   MS-NRPC\803be876-d755-4187-a011-eaf5bf2755ab.xml
    //  </remarks>
    public partial struct _NETLOGON_DELTA_DOMAIN
    {

        /// <summary>
        ///  DomainName member.
        /// </summary>
        public _RPC_UNICODE_STRING DomainName;

        /// <summary>
        ///  OemInformation member.
        /// </summary>
        public _RPC_UNICODE_STRING OemInformation;

        /// <summary>
        ///  ForceLogoff member.
        /// </summary>
        public _OLD_LARGE_INTEGER ForceLogoff;

        /// <summary>
        ///  MinPasswordLength member.
        /// </summary>
        public ushort MinPasswordLength;

        /// <summary>
        ///  PasswordHistoryLength member.
        /// </summary>
        public ushort PasswordHistoryLength;

        /// <summary>
        ///  MaxPasswordAge member.
        /// </summary>
        public _OLD_LARGE_INTEGER MaxPasswordAge;

        /// <summary>
        ///  MinPasswordAge member.
        /// </summary>
        public _OLD_LARGE_INTEGER MinPasswordAge;

        /// <summary>
        ///  DomainModifiedCount member.
        /// </summary>
        public _OLD_LARGE_INTEGER DomainModifiedCount;

        /// <summary>
        ///  DomainCreationTime member.
        /// </summary>
        public _OLD_LARGE_INTEGER DomainCreationTime;

        /// <summary>
        ///  A SECURITY_INFORMATION structure, as specified in [MS-DTYP]
        ///  section , that specifies portions of a security descriptor
        ///  about the domain.
        /// </summary>
        public uint SecurityInformation;

        /// <summary>
        ///  The size, in bytes, of the SecurityDescriptor field.
        /// </summary>
        public uint SecuritySize;

        /// <summary>
        ///  A pointer to a SECURITY_DESCRIPTOR structure, as specified
        ///  in [MS-DTYP] section , that contains the security settings
        ///  for the domain object.
        /// </summary>
        [Size("SecuritySize")]
        public byte[] SecurityDescriptor;

        /// <summary>
        ///  Has the same meaning as the CreationTime field specified
        ///  in [MS-SAMR] section.
        /// </summary>
        public _RPC_UNICODE_STRING DomainCreationTime1;

        /// <summary>
        ///  An RPC_UNICODE_STRING structure, as specified in [MS-DTYP]
        ///  section , that contains the domain lockout information
        ///  detailed in [MS-SAMR]. The Buffer field points to the
        ///  SAMPR_DOMAIN_LOCKOUT_INFORMATION structure, as specified
        ///  in [MS-SAMR] section , and the Length and MaximumLength
        ///  fields are set to the size in bytes of the SAMPR_DOMAIN_LOCKOUT_INFORMATION
        ///  structure pointed to by the Buffer field.
        /// </summary>
        public _RPC_UNICODE_STRING DomainLockoutInformation;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  specified in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString2;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  specified in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString3;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  specified in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString4;

        /// <summary>
        ///  PasswordProperties member.
        /// </summary>
        public uint PasswordProperties;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong2;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong4;
    }

    /// <summary>
    ///  The NETLOGON_LOGON_IDENTITY_INFO structure defines a
    ///  logon identity within a domain.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\81c44fa0-0a27-41b3-b607-de39cce7ea1d.xml
    //  </remarks>
    public partial struct _NETLOGON_LOGON_IDENTITY_INFO
    {

        /// <summary>
        ///  Contains the NetBIOS name of the domain of the account.
        /// </summary>
        public _RPC_UNICODE_STRING LogonDomainName;

        /// <summary>
        ///  A set of bit flags that contain information pertaining
        ///  to the logon validation processing. A flag is TRUE
        ///  (or set) if its value is equal to 1. The value is constructed
        ///  from zero or more bit flags from the following table.
        /// </summary>
        public uint ParameterControl;

        /// <summary>
        ///  MUST be set to zero when sent and MUST be ignored on
        ///  receipt.
        /// </summary>
        public _OLD_LARGE_INTEGER Reserved;

        /// <summary>
        ///  Contains the name of the user.
        /// </summary>
        public _RPC_UNICODE_STRING UserName;

        /// <summary>
        ///  Contains the NetBIOS name of the workstation from which
        ///  the user is logging on.
        /// </summary>
        public _RPC_UNICODE_STRING Workstation;
    }

    /// <summary>
    ///  The NL_DNS_NAME_INFO_ARRAY structure provides the information
    ///  on DNS names (records) to be updated by the DsrUpdateReadOnlyServerDnsRecords
    ///  method.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\8ae8a92e-b63a-43fd-9350-97f1cab1c148.xml
    //  </remarks>
    public partial struct _NL_DNS_NAME_INFO_ARRAY
    {

        /// <summary>
        ///  The number of entries in the DnsNamesInfo field.
        /// </summary>
        public uint EntryCount;

        /// <summary>
        ///  The pointer to an array of NL_DNS_NAME_INFO structure,
        ///  which contains DNS names info.Added in longhorn_server.
        /// </summary>
        [Size("EntryCount")]
        public _NL_DNS_NAME_INFO[] DnsNamesInfo;
    }

    /// <summary>
    ///  The NETLOGON_RENAME_USER structure specifies a rename
    ///  of a user account.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\96943cc2-4412-45df-8ac0-f4b3d323a27e.xml
    //  </remarks>
    public partial struct _NETLOGON_DELTA_RENAME_USER
    {

        /// <summary>
        ///  An RPC_UNICODE_STRING structure, as specified in [MS-DTYP]
        ///  section , that contains the user account's previous
        ///  name.
        /// </summary>
        public _RPC_UNICODE_STRING OldName;

        /// <summary>
        ///  An RPC_UNICODE_STRING structure, as specified in [MS-DTYP]
        ///  section , that contains the new name to assign to the
        ///  user account.
        /// </summary>
        public _RPC_UNICODE_STRING NewName;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString1;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString2;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString3;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString4;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong1;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong2;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong4;
    }

    /// <summary>
    ///  The DOMAIN_CONTROLLER_INFOW structure defines information
    ///  returned by the following methods: DsrGetDcName, DsrGetDcNameEx,
    ///  and DsrGetDcNameEx2. This structure is used to describe
    ///  naming and addressing information about a domain controller
    ///  (DC).This structure is introduced in windows_2000 and
    ///  is present in windows_xp, windows_server_2003, windows_vista,
    ///  windows_server_2008, windows_7, and windows_server_2008_r2.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\9b85a7a4-8d34-4b9e-9500-bf8644ebfc06.xml
    //  </remarks>
    public partial struct _DOMAIN_CONTROLLER_INFOW
    {

        /// <summary>
        ///  A null-terminated UTF-16 string that contains a NetBIOS
        ///  or fully qualified domain name (FQDN) (2) of the DC,
        ///  prefixed with "\\".
        /// </summary>
        [String()]
        public string DomainControllerName;

        /// <summary>
        ///  A pointer to a null-terminated Unicode string that contains
        ///  the DC address, prefixed with \\. The string can be
        ///  either a textual representation of an IPv4/IPv6 addressIPv6
        ///  is supported starting with windows_vista and is supported
        ///  in windows_server_2008, windows_7, and windows_server_7.
        ///  or the NetBIOS name of the DC, determined by the DomainControllerAddressType
        ///  field.
        /// </summary>
        [String()]
        public string DomainControllerAddress;

        /// <summary>
        ///  A 32-bit value indicating the DC address type, which
        ///  MUST be one, and only one, of the following.
        /// </summary>
        public DomainControllerAddressType_Values DomainControllerAddressType;

        /// <summary>
        ///  A globally unique identifier (GUID) structure that contains
        ///  an identifier for the domain. When there is no domainGUID,
        ///  this field MUST be set to zero.windows_nt-based domain
        ///  controllers do not have a domainGUID. A GUID can be
        ///  used across all computers and networks wherever a unique
        ///  identifier is required.
        /// </summary>
        public System.Guid DomainGuid;

        /// <summary>
        ///  A Unicode string that contains the NetBIOS or fully
        ///  qualified domain name (FQDN) (2) of the domain.
        /// </summary>
        [String()]
        public string DomainName;

        /// <summary>
        ///  A pointer to a null-terminated Unicode string that contains
        ///  the fully qualified domain name (FQDN) of the forest.
        /// </summary>
        [String()]
        public string DnsForestName;

        /// <summary>
        ///  A set of bit flags that describe the features and roles
        ///  of the DC. A flag is TRUE (or set) if its value is
        ///  equal to 1. The value is constructed from zero or more
        ///  bit flags from the following table, with the exception
        ///  that bit J cannot be combined with A, B, D, E, F, or
        ///  I.
        /// </summary>
        public uint Flags;

        /// <summary>
        ///  Pointer to a null-terminated Unicode string that contains
        ///  the site name that is associated with the DC. When
        ///  there is no associated site, this field MUST be NULL.windows_nt-based
        ///  domain controllers do not have an associated site.
        /// </summary>
        [String()]
        public string DcSiteName;

        /// <summary>
        ///  Pointer to a null-terminated Unicode string that contains
        ///  the client's site name. When there is no client site
        ///  name, this field MUST be NULL.
        /// </summary>
        [String()]
        public string ClientSiteName;
    }

    /// <summary>
    /// Domain controller address type
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum DomainControllerAddressType_Values : uint
    {

        /// <summary>
        ///  The address is a string that contains an IPv4 address
        ///  in dotted-decimal notation (for example, 192.168.0.1),
        ///  or an IPv6 address in colon-separated notation.For
        ///  windows_vista, windows_server_2008, windows_7, and
        ///  windows_server_7, this address can be an IPv4 or IPv6
        ///  address. For all other versions of windows, this will
        ///  be an IPv4 address.
        /// </summary>
        IpAddress = 0x00000001,

        /// <summary>
        ///  The address is a NetBIOS name.
        /// </summary>
        NetBiosName = 0x00000002,
    }

    /// <summary>
    ///  The USER_SESSION_KEY structure defines an encrypted
    ///  user session key.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\9c07428c-a907-4590-a2c1-bb95a009e527.xml
    //  </remarks>
    public partial struct _USER_SESSION_KEY
    {

        /// <summary>
        ///  A two-element CYPHER_BLOCK structure, as specified in
        ///  section , that contains the 16-byte encrypted user
        ///  session key.
        /// </summary>
        [Inline()]
        [StaticSize(2, StaticSizeMode.Elements)]
        public _CYPHER_BLOCK[] data;
    }

    /// <summary>
    ///  The NL_GENERIC_RPC_DATA structure defines a format for
    ///  marshaling arrays of unsigned long values and Unicode
    ///  strings, by value, over RPC.This structure was introduced
    ///  in windows_xp and is present in windows_server_2003,
    ///  windows_vista, windows_server_2008, windows_7, and
    ///  windows_server_7. The NL_GENERIC_RPC_DATA structure
    ///  can be used to transmit generic data over RPC from
    ///  the server to a client.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\a1c7f3ca-c8b1-4514-9fd5-ed3460c83a4d.xml
    //  </remarks>
    public partial struct _NL_GENERIC_RPC_DATA
    {

        /// <summary>
        ///  The number of entries in UlongData.
        /// </summary>
        public uint UlongEntryCount;

        /// <summary>
        ///  A pointer to an array of unsigned 32-bit integer values.
        /// </summary>
        [Size("UlongEntryCount")]
        public uint[] UlongData;

        /// <summary>
        ///  The number of entries in UnicodeStringData.
        /// </summary>
        public uint UnicodeStringEntryCount;

        /// <summary>
        ///  A pointer to an array of Unicode string structures.
        /// </summary>
        [Size("UnicodeStringEntryCount")]
        public _RPC_UNICODE_STRING[] UnicodeStringData;
    }

    /// <summary>
    ///  The NETLOGON_DELTA_GROUP structure contains information
    ///  about a SAM group account. This structure is used for
    ///  replicating the group data from the PDC to a BDC, as
    ///  detailed in section.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\b7949b4e-0fc7-40d4-8841-8d69230e2d79.xml
    //  </remarks>
    public partial struct _NETLOGON_DELTA_GROUP
    {

        /// <summary>
        ///  A RPC_UNICODE_STRING structure that contains the group
        ///  name.
        /// </summary>
        public _RPC_UNICODE_STRING Name;

        /// <summary>
        ///  The RID for the group.
        /// </summary>
        public uint RelativeId;

        /// <summary>
        ///  A set of bit flags that describe attributes of the SID.
        ///  An attribute is true (or set) if its value is equal
        ///  to 1. The value is constructed from one or more bit
        ///  flags from the following table.
        /// </summary>
        public uint Attributes;

        /// <summary>
        ///  An RPC_UNICODE_STRING structure, as specified in [MS-DTYP]
        ///  section , that contains an administrative comment for
        ///  the group.
        /// </summary>
        public _RPC_UNICODE_STRING AdminComment;

        /// <summary>
        ///  A SECURITY_INFORMATION structure, as specified in [MS-DTYP]
        ///  section , that specifies portions of a security descriptor
        ///  about the group.
        /// </summary>
        public uint SecurityInformation;

        /// <summary>
        ///  The size, in bytes, of the SecurityDescriptor field.
        /// </summary>
        public uint SecuritySize;

        /// <summary>
        ///  A pointer to a SECURITY_DESCRIPTOR structure, as specified
        ///  in [MS-DTYP] section , that contains the security settings
        ///  of the group object.
        /// </summary>
        [Size("SecuritySize")]
        public byte[] SecurityDescriptor;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString1;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString2;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString3;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString4;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong1;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong2;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public uint DummyLong4;
    }

    /// <summary>
    ///  The NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES union defines
    ///  versioning. Currently, only version 1 is supported.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\b8491488-cbb6-452b-bab1-b0f4390bca59.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES
    {

        /// <summary>
        ///  An NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES_V1 structure.Added
        ///  in longhorn_server.
        /// </summary>
        [Case("1")]
        public _NL_OUT_CHAIN_SET_CLIENT_ATTRIBUTES_V1 V1;
    }

    /// <summary>
    ///  The NETLOGON_DELTA_POLICY structure contains information
    ///  about the LSA policy. This structure is used for replicating
    ///  the LSA policy data from the PDC to a BDC, as detailed
    ///  in section.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\b9ee3608-efd1-4d57-b4da-5a13b9ec1fa1.xml
    //  </remarks>
    public partial struct _NETLOGON_DELTA_POLICY
    {

        /// <summary>
        ///  This field has the same meaning as the identically named
        ///  field of the POLICY_AUDIT_LOG_INFO structure, as specified
        ///  in [MS-LSAD] section.
        /// </summary>
        public uint MaximumLogSize;

        /// <summary>
        ///  This field has the same meaning as the identically named
        ///  field of the POLICY_AUDIT_LOG_INFO structure, as specified
        ///  in [MS-LSAD] section.
        /// </summary>
        public _OLD_LARGE_INTEGER AuditRetentionPeriod;

        /// <summary>
        ///  This field has the same meaning as the identically named
        ///  field of the LSAPR_POLICY_AUDIT_EVENTS_INFO structure,
        ///  as specified in [MS-LSAD] section.
        /// </summary>
        public byte AuditingMode;

        /// <summary>
        ///  This field has the same meaning as the identically named
        ///  field of the LSAPR_POLICY_AUDIT_EVENTS_INFO structure,
        ///  as specified in [MS-LSAD] section.
        /// </summary>
        public uint MaximumAuditEventCount;

        /// <summary>
        ///  This field has the same meaning as the identically named
        ///  field of the LSAPR_POLICY_AUDIT_EVENTS_INFO structure,
        ///  as specified in [MS-LSAD] section.
        /// </summary>
        [Size("MaximumAuditEventCount + 1")]
        public uint[] EventAuditingOptions;

        /// <summary>
        ///  An RPC_UNICODE_STRING structure, as specified in [MS-DTYP]
        ///  section , that contains the NetBIOS name of the primary
        ///  domain.
        /// </summary>
        public _RPC_UNICODE_STRING PrimaryDomainName;

        /// <summary>
        ///  A pointer to the SID for the primary domain.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] PrimaryDomainSid;

        /// <summary>
        ///  An NLPR_QUOTA_LIMITS structure, as specified in section
        ///  , that contains information about system resource quotas
        ///  imposed on an account.
        /// </summary>
        public _NLPR_QUOTA_LIMITS QuotaLimits;

        /// <summary>
        ///  An OLD_LARGE_INTEGER structure, as specified in [MS-SAMR]
        ///  section , that contains the count that is incremented
        ///  each time the database is modified. This count is the
        ///  database serial number for the database.
        /// </summary>
        public _OLD_LARGE_INTEGER ModifiedId;

        /// <summary>
        ///  A 64-bit time stamp, equivalent to a FILETIME, specifying
        ///  when the database was created.
        /// </summary>
        public _OLD_LARGE_INTEGER DatabaseCreationTime;

        /// <summary>
        ///  A SECURITY_INFORMATION bit flag that contains security
        ///  information about the policy. For details about SECURITY_INFORMATION
        ///  structure, see [MS-DTYP] section.
        /// </summary>
        public uint SecurityInformation;

        /// <summary>
        ///  The size, in bytes, of the SecurityDescriptor field.
        /// </summary>
        public uint SecuritySize;

        /// <summary>
        ///  A pointer to a SECURITY_DESCRIPTOR structure, as specified
        ///  in [MS-DTYP] section , that describes the security
        ///  settings for the LSA policy object.
        /// </summary>
        [Size("SecuritySize")]
        public byte[] SecurityDescriptor;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString1;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString2;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString3;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString4;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong1;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong2;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored upon receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong4;
    }

    /// <summary>
    ///  The NETLOGON_VALIDATION_SAM_INFO4 structure extends
    ///  NETLOGON_VALIDATION_SAM_INFO2, as specified in section
    ///  , by storing the fully qualified domain name (FQDN)
    ///  of the domain of the user account and the user principal.All
    ///  fields of this structure, except the fields detailed
    ///  following the structure definition, have the same meaning
    ///  as the identically named fields in the KERB_VALIDATION_INFO
    ///  structure, as specified in [MS-PAC] section. Additionally,
    ///  fields of this structure that are defined as OLD_LARGE_INTEGER
    ///  are 64-bit timestamps equivalent to the identically
    ///  named fields in the KERB_VALIDATION_INFO structure
    ///  of FILETIME type.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\bccfdba9-0c38-485e-b751-d4de1935781d.xml
    //  </remarks>
    public partial struct _NETLOGON_VALIDATION_SAM_INFO4
    {

        /// <summary>
        ///  LogonTime member.
        /// </summary>
        public _OLD_LARGE_INTEGER LogonTime;

        /// <summary>
        ///  LogoffTime member.
        /// </summary>
        public _OLD_LARGE_INTEGER LogoffTime;

        /// <summary>
        ///  KickOffTime member.
        /// </summary>
        public _OLD_LARGE_INTEGER KickOffTime;

        /// <summary>
        ///  PasswordLastSet member.
        /// </summary>
        public _OLD_LARGE_INTEGER PasswordLastSet;

        /// <summary>
        ///  PasswordCanChange member.
        /// </summary>
        public _OLD_LARGE_INTEGER PasswordCanChange;

        /// <summary>
        ///  PasswordMustChange member.
        /// </summary>
        public _OLD_LARGE_INTEGER PasswordMustChange;

        /// <summary>
        ///  EffectiveName member.
        /// </summary>
        public _RPC_UNICODE_STRING EffectiveName;

        /// <summary>
        ///  FullName member.
        /// </summary>
        public _RPC_UNICODE_STRING FullName;

        /// <summary>
        ///  LogonScript member.
        /// </summary>
        public _RPC_UNICODE_STRING LogonScript;

        /// <summary>
        ///  ProfilePath member.
        /// </summary>
        public _RPC_UNICODE_STRING ProfilePath;

        /// <summary>
        ///  HomeDirectory member.
        /// </summary>
        public _RPC_UNICODE_STRING HomeDirectory;

        /// <summary>
        ///  HomeDirectoryDrive member.
        /// </summary>
        public _RPC_UNICODE_STRING HomeDirectoryDrive;

        /// <summary>
        ///  LogonCount member.
        /// </summary>
        public ushort LogonCount;

        /// <summary>
        ///  BadPasswordCount member.
        /// </summary>
        public ushort BadPasswordCount;

        /// <summary>
        ///  UserId member.
        /// </summary>
        public uint UserId;

        /// <summary>
        ///  PrimaryGroupId member.
        /// </summary>
        public uint PrimaryGroupId;

        /// <summary>
        ///  GroupCount member.
        /// </summary>
        public uint GroupCount;

        /// <summary>
        ///  GroupIds member.
        /// </summary>
        [Size("GroupCount")]
        public _GROUP_MEMBERSHIP[] GroupIds;

        /// <summary>
        ///  UserFlags member.
        /// </summary>
        public uint UserFlags;

        /// <summary>
        ///  UserSessionKey member.
        /// </summary>
        public _USER_SESSION_KEY UserSessionKey;

        /// <summary>
        ///  An RPC_UNICODE_STRING structure that contains the NetBIOS
        ///  name of the server that populates this structure.
        /// </summary>
        public _RPC_UNICODE_STRING LogonServer;

        /// <summary>
        ///  LogonDomainName member.
        /// </summary>
        public _RPC_UNICODE_STRING LogonDomainName;

        /// <summary>
        ///  LogonDomainId member.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] LogonDomainId;

        /// <summary>
        ///  Contains the LM key.
        /// </summary>
        [Inline()]
        [StaticSize(8, StaticSizeMode.Elements)]
        public byte[] LMKey;

        /// <summary>
        ///  UserAccountControl member.
        /// </summary>
        public uint UserAccountControl;

        /// <summary>
        ///  SubAuthStatus member.
        /// </summary>
        public uint SubAuthStatus;

        /// <summary>
        ///  LastSuccessfulILogon member.
        /// </summary>
        public _OLD_LARGE_INTEGER LastSuccessfulILogon;

        /// <summary>
        ///  LastFailedILogon member.
        /// </summary>
        public _OLD_LARGE_INTEGER LastFailedILogon;

        /// <summary>
        ///  FailedILogonCount member.
        /// </summary>
        public uint FailedILogonCount;

        /// <summary>
        ///  An unsigned 32-bit integer. This member is reserved.
        ///  MUST be zero when sent, and MUST be ignored on receipt.
        /// </summary>
        [Inline()]
        [StaticSize(1, StaticSizeMode.Elements)]
        public uint[] Reserved4;

        /// <summary>
        ///  SidCount member.
        /// </summary>
        public uint SidCount;

        /// <summary>
        ///  ExtraSids member.
        /// </summary>
        [Size("SidCount")]
        public _NETLOGON_SID_AND_ATTRIBUTES[] ExtraSids;

        /// <summary>
        ///  Contains the fully qualified domain name (FQDN) of the
        ///  domain of the user account.
        /// </summary>
        public _RPC_UNICODE_STRING DnsLogonDomainName;

        /// <summary>
        ///  Contains the user principal name (UPN).
        /// </summary>
        public _RPC_UNICODE_STRING Upn;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. Expansion strings have a function similar
        ///  to that of dummy fields, as detailed in section.
        /// </summary>
        public _RPC_UNICODE_STRING ExpansionString1;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. Expansion strings have a function similar
        ///  to that of dummy fields, as detailed in section.
        /// </summary>
        public _RPC_UNICODE_STRING ExpansionString2;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. Expansion strings have a function similar
        ///  to that of dummy fields, as detailed in section.
        /// </summary>
        public _RPC_UNICODE_STRING ExpansionString3;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. Expansion strings have a function similar
        ///  to that of dummy fields, as detailed in section.
        /// </summary>
        public _RPC_UNICODE_STRING ExpansionString4;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. Expansion strings have a function similar
        ///  to that of dummy fields, as detailed in section.
        /// </summary>
        public _RPC_UNICODE_STRING ExpansionString5;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. Expansion strings have a function similar
        ///  to that of dummy fields, as detailed in section.
        /// </summary>
        public _RPC_UNICODE_STRING ExpansionString6;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. Expansion strings have a function similar
        ///  to that of dummy fields, as detailed in section.
        /// </summary>
        public _RPC_UNICODE_STRING ExpansionString7;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. Expansion strings have a function similar
        ///  to that of dummy fields, as detailed in section.
        /// </summary>
        public _RPC_UNICODE_STRING ExpansionString8;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. Expansion strings have a function similar
        ///  to that of dummy fields, as detailed in section.
        /// </summary>
        public _RPC_UNICODE_STRING ExpansionString9;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. Expansion strings have a function similar
        ///  to that of dummy fields, as detailed in section.
        /// </summary>
        public _RPC_UNICODE_STRING ExpansionString10;
    }

    /// <summary>
    ///  The NETLOGON_GENERIC_INFO structure defines a structure
    ///  that contains logon information in binary format. Microsoft
    ///  implementations of authentication protocols make use
    ///  of this structure for passing generic logon data through
    ///  the Netlogon secure channel to a DC in the domain that
    ///  contains the user account to use the domain'sdatabase.
    ///  For an example of using the NETLOGON_GENERIC_INFO structure,
    ///  see any of the examples documented in [MS-APDS].
    /// </summary>
    //  <remarks>
    //   MS-NRPC\c03d6d95-9b5e-4329-9645-eedcdd167f6e.xml
    //  </remarks>
    public partial struct _NETLOGON_GENERIC_INFO
    {

        /// <summary>
        ///  The NETLOGON_LOGON_IDENTITY_INFO structure, as specified
        ///  in section , contains information about the logon identity.
        ///  The LogonDomainName field of the NETLOGON_LOGON_IDENTITY_INFO
        ///  structure indicates the target domain that contains
        ///  the user account.
        /// </summary>
        public _NETLOGON_LOGON_IDENTITY_INFO Identity;

        /// <summary>
        ///  Contains the name of the security provider, such as
        ///  Kerberos, to which the data will be delivered on the
        ///  domain controller in the target domain that was specified
        ///  in the Identity field. This name MUST match the name
        ///  of an existing security provider; otherwise, the Security
        ///  Support Provider Interface (SSPI) returns a package
        ///  not found error.
        /// </summary>
        public _RPC_UNICODE_STRING PackageName;

        /// <summary>
        ///  The length, in bytes, of LogonData.
        /// </summary>
        public uint DataLength;

        /// <summary>
        ///  A pointer to a block of binary data that contains the
        ///  information to be sent to the security package referenced
        ///  in PackageName. This data is opaque to Netlogon.
        /// </summary>
        [Size("DataLength")]
        public byte[] LogonData;
    }

    /// <summary>
    ///  The NETLOGON_RENAME_GROUP structure specifies a rename
    ///  of a group.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\c48a2216-2c51-412b-aeb6-c6666004b53f.xml
    //  </remarks>
    public partial struct _NETLOGON_DELTA_RENAME_GROUP
    {

        /// <summary>
        ///  An RPC_UNICODE_STRING structure, as specified in [MS-DTYP]
        ///  section , that contains the group's previous name.
        /// </summary>
        public _RPC_UNICODE_STRING OldName;

        /// <summary>
        ///  An RPC_UNICODE_STRING structure, as specified in [MS-DTYP]
        ///  section , that contains the new name to assign to the
        ///  group.
        /// </summary>
        public _RPC_UNICODE_STRING NewName;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString1;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString2;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString3;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString4;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong1;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong2;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong4;
    }

    /// <summary>
    ///  The NETLOGON_CONTROL_QUERY_INFORMATION union selects
    ///  an appropriate NETLOGON_INFO data type, based on the
    ///  value of the QueryLevel parameter to the NetrLogonControl2Ex
    ///  method described in section.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\c5d2469c-f0c3-47de-8795-0e22f1070337.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _NETLOGON_CONTROL_QUERY_INFORMATION
    {

        /// <summary>
        ///  This field is selected when the switched DWORD ([MS-DTYP]
        ///  section) value is 1. For more information about NETLOGON_INFO_1,
        ///  see section.
        /// </summary>
        [Case("1")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_INFO_1[] NetlogonInfo1;

        /// <summary>
        ///  This field is selected when the switched DWORD value
        ///  is 2. For more information about NETLOGON_INFO_2, see
        ///  section.
        /// </summary>
        [Case("2")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_INFO_2[] NetlogonInfo2;

        /// <summary>
        ///  This field is selected when the switched DWORD value
        ///  is 3. For more information about NETLOGON_INFO_3, see
        ///  section.
        /// </summary>
        [Case("3")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_INFO_3[] NetlogonInfo3;

        /// <summary>
        ///  This field is selected when the switched DWORD value
        ///  is 4. For more information about NETLOGON_INFO_4, see
        ///  section.
        /// </summary>
        [Case("4")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_INFO_4[] NetlogonInfo4;
    }

    /// <summary>
    ///  The NETLOGON_DELTA_SECRET structure contains information
    ///  about the LSA secret object, as specified in [MS-LSAD].
    ///  This structure is used for to replicate the LSA secret
    ///  object data from the PDC to a BDC, as detailed in section
    ///  .
    /// </summary>
    //  <remarks>
    //   MS-NRPC\ddf4e131-ef89-425f-a70b-88fdd73afb51.xml
    //  </remarks>
    public partial struct _NETLOGON_DELTA_SECRET
    {

        /// <summary>
        ///  An NLPR_CR_CIPHER_VALUE structure, as specified in section
        ///  , that contains the encrypted current value of the
        ///  LSA secret.
        /// </summary>
        public _NLPR_CR_CIPHER_VALUE CurrentValue;

        /// <summary>
        ///  A 64-bit time stamp, equivalent to a FILETIME, at which
        ///  the current value of the LSA secret object was set.
        /// </summary>
        public _OLD_LARGE_INTEGER CurrentValueSetTime;

        /// <summary>
        ///  An NLPR_CR_CIPHER_VALUE structure, as specified in section
        ///  , that contains the encrypted previous (old) value
        ///  of the LSA secret.
        /// </summary>
        public _NLPR_CR_CIPHER_VALUE OldValue;

        /// <summary>
        ///  A 64-bit time stamp, equivalent to a FILETIME, at which
        ///  the previous value of the LSA secret object was set.
        /// </summary>
        public _OLD_LARGE_INTEGER OldValueSetTime;

        /// <summary>
        ///  A SECURITY_INFORMATION structure, as specified in [MS-DTYP]
        ///  section , that specifies portions of a security descriptor
        ///  about the secret object.
        /// </summary>
        public uint SecurityInformation;

        /// <summary>
        ///  The size, in bytes, of the SecurityDescriptor member.
        /// </summary>
        public uint SecuritySize;

        /// <summary>
        ///  A pointer to a SECURITY_DESCRIPTOR structure, as specified
        ///  in of [MS-DTYP] section  that describes the security
        ///  settings for the LSA secret object.
        /// </summary>
        [Size("SecuritySize")]
        public byte[] SecurityDescriptor;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString1;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString2;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString3;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString4;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong1;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong2;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong4;
    }

    /// <summary>
    ///  The NETLOGON_NETWORK_INFO structure defines information
    ///  that describes a network account logon.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\e17b03b8-c1d2-43a1-98db-cf8d05b9c6a8.xml
    //  </remarks>
    public partial struct _NETLOGON_NETWORK_INFO
    {

        /// <summary>
        ///  NETLOGON_LOGON_IDENTITY_INFO structure, as specified
        ///  in section , that contains information about the logon
        ///  identity.
        /// </summary>
        public _NETLOGON_LOGON_IDENTITY_INFO Identity;

        /// <summary>
        ///  LM_CHALLENGE structure, as specified in section , that
        ///  contains the network authentication challenge. For
        ///  details about challenges, see [MS-NLMP].
        /// </summary>
        public LM_CHALLENGE LmChallenge;

        /// <summary>
        ///  String that contains the NT response (see [MS-NLMP])
        ///  to the network authentication challenge.
        /// </summary>
        public _STRING NtChallengeResponse;

        /// <summary>
        ///  String that contains the LAN Manager response (see [MS-NLMP])
        ///  to the network authentication challenge.
        /// </summary>
        public _STRING LmChallengeResponse;
    }

    /// <summary>
    ///  The NLPR_SID_INFORMATION structure is used to form a
    ///  wrapper for a SID; it is used to transmit a SID during
    ///  certain replication operations. See section  for details.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\e936e7db-27cb-40dd-acf7-eec6ac0e9ed5.xml
    //  </remarks>
    public partial struct _NLPR_SID_INFORMATION
    {

        /// <summary>
        ///  A pointer to a SID structure.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] SidPointer;
    }

    /// <summary>
    ///  The NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES_V1 structure specifies
    ///  the values to update on the client's computer account
    ///  object in Active Directory on a normal (writable) domain
    ///  controller.The normal (writable) DC cannot be a windows_2000_server
    ///  or a windows_server_2003domain controller.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\f4ed8790-d7e8-4ca1-8062-d25784ed79e3.xml
    //  </remarks>
    public partial struct _NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES_V1
    {

        /// <summary>
        ///  A NULL or null-terminated Unicode string that is used
        ///  to update the attribute dNSHostName on the client's
        ///  computer account object in Active Directory.
        /// </summary>
        [String()]
        public string ClientDnsHostName;

        /// <summary>
        ///  If not NULL, the attribute operatingSystemVersion on
        ///  the client's computer account in Active Directory (using
        ///  the ABNF Syntax as specified in [RFC2234]) is set to:If
        ///  OsVersionInfo.dwBuildNumber is 0:operatingSystemVersion
        ///  = MajorVersion "." MinorVersion MajorVersion = OsVersionInfo.dwMajorVersion
        ///  MinorVersion = OsVersionInfo.dwMinorVersionOtherwise:operatingSystemVersion
        ///  = MajorVersion "." MinorVersion "."               
        ///            BuildNumber MajorVersion = OsVersionInfo.dwMajorVersion
        ///  MinorVersion = OsVersionInfo.dwMinorVersion BuildNumber
        ///  = OsVersionInfo.dwBuildNumber
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NL_OSVERSIONINFO_V1[] OsVersionInfo;

        /// <summary>
        ///  NULL or a null-terminated Unicode string that is used
        ///  to update the attribute operatingSystem on the client's
        ///  computer account object in Active Directory.Added in
        ///  longhorn_server.
        /// </summary>
        [String()]
        public string OsName;
    }

    /// <summary>
    ///  The NT_OWF_PASSWORD structure defines a one-way function
    ///  (OWF) of a windows_ntdomain password. The NT_OWF_PASSWORD
    ///  structure MAY be encrypted, as specified by each method
    ///  that uses this structure. When this structure is encrypted,
    ///  Netlogon methods typically use the DES encryption algorithm
    ///  in ECB mode, as specified in [MS-SAMR] section , Encrypting
    ///  an NT Hash or LM Hash Value with a Specified Key. The
    ///  session key is the specified 16-byte key used to derive
    ///  its keys using the 16-byte value process, as specified
    ///  in [MS-SAMR] section. For specific encryption information,
    ///  see the individual methods, such as NetrServerTrustPasswordsGet
    ///  and NetrServerGetTrustInfo.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\f7458576-d538-47cd-b387-b6f98449a0ee.xml
    //  </remarks>
    public partial struct _NT_OWF_PASSWORD
    {

        /// <summary>
        ///  An array of CYPHER_BLOCK structures that contains the
        ///  NTOWFv1 of a password. NTOWFv1 is specified in NTLM
        ///  v1 Authentication in [MS-NLMP] section.
        /// </summary>
        [Inline()]
        [StaticSize(2, StaticSizeMode.Elements)]
        public _CYPHER_BLOCK[] data;
    }

    /// <summary>
    ///  The NETLOGON_DELTA_TRUSTED_DOMAINS structure contains
    ///  information about a trusted domain. This structure
    ///  is used for replicating the trusted domain data from
    ///  the PDC to a BDC.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\fe95340e-db88-4fdd-85c6-bc8ad1c4ff8c.xml
    //  </remarks>
    public partial struct _NETLOGON_DELTA_TRUSTED_DOMAINS
    {

        /// <summary>
        ///  An RPC_UNICODE_STRING structure, as specified in [MS-DTYP]
        ///  section , that contains the NetBIOS name of the trusted
        ///  domain.
        /// </summary>
        public _RPC_UNICODE_STRING DomainName;

        /// <summary>
        ///  Number of domain controller (DC) names listed in the
        ///  ControllerNames field.Starting with windows_2000, NumControllerEntries
        ///  is always set to zero in this structure.
        /// </summary>
        public uint NumControllerEntries;

        /// <summary>
        ///  Pointer to an array of RPC_UNICODE_STRING structures,
        ///  as specified in [MS-DTYP] section , that contain the
        ///  NetBIOS names of the DCs in the trusted domain. The
        ///  only restriction is the maximum value of the 32-bit
        ///  unsigned integer enforced by RPC.Starting with windows_2000,
        ///  ControllerNames is always set to NULL in this structure.
        /// </summary>
        [Size("NumControllerEntries")]
        public _RPC_UNICODE_STRING[] ControllerNames;

        /// <summary>
        ///  A SECURITY_INFORMATION structure, as specified in [MS-DTYP]
        ///  section , that specifies portions of a security descriptor
        ///  about the trusted domain.
        /// </summary>
        public uint SecurityInformation;

        /// <summary>
        ///  Size, in bytes, of the SecurityDescriptor field.
        /// </summary>
        public uint SecuritySize;

        /// <summary>
        ///  Pointer to a SECURITY_DESCRIPTOR structure, as specified
        ///  in of [MS-DTYP] section  that describes the security
        ///  settings for the trusted domain object.
        /// </summary>
        [Size("SecuritySize")]
        public byte[] SecurityDescriptor;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString1;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString2;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString3;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString4;

        /// <summary>
        ///  The value that contains the POSIX offset for the trusted
        ///  domain, as specified in [MS-ADTS] section.
        /// </summary>
        public uint TrustedPosixOffset;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong2;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong4;
    }

    /// <summary>
    ///  The NETLOGON_VALIDATION_SAM_INFO structure defines account
    ///  information retrieved from a database upon a successful
    ///  user logon validation.All fields of this structure,
    ///  except the fields detailed following the structure
    ///  definition, have the same meaning as the identically
    ///  named fields in the KERB_VALIDATION_INFO structure,
    ///  as specified in [MS-PAC] section. Additionally, fields
    ///  of this structure that are defined as OLD_LARGE_INTEGER
    ///  are 64-bit timestamps equivalent to the identically
    ///  named fields in the KERB_VALIDATION_INFO structure
    ///  of FILETIME type.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\142a277f-e161-45ac-8b95-b94bb169b5da.xml
    //  </remarks>
    public partial struct _NETLOGON_VALIDATION_SAM_INFO
    {

        /// <summary>
        ///  LogonTime member.
        /// </summary>
        public _OLD_LARGE_INTEGER LogonTime;

        /// <summary>
        ///  LogoffTime member.
        /// </summary>
        public _OLD_LARGE_INTEGER LogoffTime;

        /// <summary>
        ///  KickOffTime member.
        /// </summary>
        public _OLD_LARGE_INTEGER KickOffTime;

        /// <summary>
        ///  PasswordLastSet member.
        /// </summary>
        public _OLD_LARGE_INTEGER PasswordLastSet;

        /// <summary>
        ///  PasswordCanChange member.
        /// </summary>
        public _OLD_LARGE_INTEGER PasswordCanChange;

        /// <summary>
        ///  PasswordMustChange member.
        /// </summary>
        public _OLD_LARGE_INTEGER PasswordMustChange;

        /// <summary>
        ///  EffectiveName member.
        /// </summary>
        public _RPC_UNICODE_STRING EffectiveName;

        /// <summary>
        ///  FullName member.
        /// </summary>
        public _RPC_UNICODE_STRING FullName;

        /// <summary>
        ///  LogonScript member.
        /// </summary>
        public _RPC_UNICODE_STRING LogonScript;

        /// <summary>
        ///  ProfilePath member.
        /// </summary>
        public _RPC_UNICODE_STRING ProfilePath;

        /// <summary>
        ///  HomeDirectory member.
        /// </summary>
        public _RPC_UNICODE_STRING HomeDirectory;

        /// <summary>
        ///  HomeDirectoryDrive member.
        /// </summary>
        public _RPC_UNICODE_STRING HomeDirectoryDrive;

        /// <summary>
        ///  LogonCount member.
        /// </summary>
        public ushort LogonCount;

        /// <summary>
        ///  BadPasswordCount member.
        /// </summary>
        public ushort BadPasswordCount;

        /// <summary>
        ///  UserId member.
        /// </summary>
        public uint UserId;

        /// <summary>
        ///  PrimaryGroupId member.
        /// </summary>
        public uint PrimaryGroupId;

        /// <summary>
        ///  GroupCount member.
        /// </summary>
        public uint GroupCount;

        /// <summary>
        ///  GroupIds member.
        /// </summary>
        [Size("GroupCount")]
        public _GROUP_MEMBERSHIP[] GroupIds;

        /// <summary>
        ///  UserFlags member.
        /// </summary>
        public uint UserFlags;

        /// <summary>
        ///  UserSessionKey member.
        /// </summary>
        public _USER_SESSION_KEY UserSessionKey;

        /// <summary>
        ///  An RPC_UNICODE_STRING structure that contains the NetBIOS
        ///  name of the server that populates this structure.
        /// </summary>
        public _RPC_UNICODE_STRING LogonServer;

        /// <summary>
        ///  LogonDomainName member.
        /// </summary>
        public _RPC_UNICODE_STRING LogonDomainName;

        /// <summary>
        ///  LogonDomainId member.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] LogonDomainId;

        /// <summary>
        ///  A ten-element array of unsigned 32-bit integers. This
        ///  member has a function similar to that of dummy fields,
        ///  as detailed in section. Each element of the array
        ///  MUST be zero when sent, and MUST be ignored on receipt.
        /// </summary>
        [Inline()]
        [StaticSize(10, StaticSizeMode.Elements)]
        public uint[] ExpansionRoom;
    }

    /// <summary>
    ///  The NETLOGON_SERVICE_INFO structure defines information
    ///  about a service account logon. Windows services use
    ///  service accounts as their run-time security identity.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\1c9f2af2-e164-4548-9428-eae68a03537e.xml
    //  </remarks>
    public partial struct _NETLOGON_SERVICE_INFO
    {

        /// <summary>
        ///  NETLOGON_LOGON_IDENTITY_INFO structure, as specified
        ///  in section , containing information about the logon
        ///  identity.
        /// </summary>
        public _NETLOGON_LOGON_IDENTITY_INFO Identity;

        /// <summary>
        ///  LM_OWF_PASSWORD structure, as specified in section ,
        ///  that contains the LMOWFv1 of a password. LMOWFv1 is
        ///  specified in NTLM v1 Authentication in [MS-NLMP] section
        ///  .
        /// </summary>
        public _LM_OWF_PASSWORD LmOwfPassword;

        /// <summary>
        ///  NT_OWF_PASSWORD structure, as specified in section ,
        ///  that contains the NTOWFv1 of a password. NTOWFv1 is
        ///  specified in NTLM v1 Authentication in [MS-NLMP] section
        ///  .
        /// </summary>
        public _NT_OWF_PASSWORD NtOwfPassword;
    }

    /// <summary>
    ///  The NETLOGON_TRUSTED_DOMAIN_ARRAY structure defines
    ///  information returned by the NetrEnumerateTrustedDomainsEx
    ///  method, as specified in section.This structure was
    ///  introduced in windows_2000 and is present in windows_xp,
    ///  windows_server_2003, windows_vista, windows_server_2008,
    ///  windows_7, and windows_server_7. It contains an array
    ///  of DS_DOMAIN_TRUSTSW structures, as specified in section
    ///  , that describe domainstrusted by the server processing
    ///  the call.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\251cf1be-2932-4d33-8532-595cf42e8091.xml
    //  </remarks>
    public partial struct _NETLOGON_TRUSTED_DOMAIN_ARRAY
    {

        /// <summary>
        ///  The number of entries in the Domains field.
        /// </summary>
        public uint DomainCount;

        /// <summary>
        ///  The data structure that contains an array of DS_DOMAIN_TRUSTSW
        ///  structures, as specified in section , that represent
        ///  trusted domains.
        /// </summary>
        [Size("DomainCount")]
        public _DS_DOMAIN_TRUSTSW[] Domains;
    }

    /// <summary>
    ///  The NETLOGON_VALIDATION_SAM_INFO2 structure is an extension
    ///  to NETLOGON_VALIDATION_SAM_INFO, as specified in section
    ///  , with support for storing extra SIDs.All fields of
    ///  this structure, except the fields detailed following
    ///  the structure definition, have the same meaning as
    ///  the identically named fields in the KERB_VALIDATION_INFO
    ///  structure as specified in [MS-PAC] section. Additionally,
    ///  fields of this structure that are defined as OLD_LARGE_INTEGER
    ///  are 64-bit timestamps equivalent to the identically
    ///  named fields in the KERB_VALIDATION_INFO structure
    ///  of FILETIME type.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\2a12e289-7904-4ecb-9d83-6732200230c0.xml
    //  </remarks>
    public partial struct _NETLOGON_VALIDATION_SAM_INFO2
    {

        /// <summary>
        ///  LogonTime member.
        /// </summary>
        public _OLD_LARGE_INTEGER LogonTime;

        /// <summary>
        ///  LogoffTime member.
        /// </summary>
        public _OLD_LARGE_INTEGER LogoffTime;

        /// <summary>
        ///  KickOffTime member.
        /// </summary>
        public _OLD_LARGE_INTEGER KickOffTime;

        /// <summary>
        ///  PasswordLastSet member.
        /// </summary>
        public _OLD_LARGE_INTEGER PasswordLastSet;

        /// <summary>
        ///  PasswordCanChange member.
        /// </summary>
        public _OLD_LARGE_INTEGER PasswordCanChange;

        /// <summary>
        ///  PasswordMustChange member.
        /// </summary>
        public _OLD_LARGE_INTEGER PasswordMustChange;

        /// <summary>
        ///  EffectiveName member.
        /// </summary>
        public _RPC_UNICODE_STRING EffectiveName;

        /// <summary>
        ///  FullName member.
        /// </summary>
        public _RPC_UNICODE_STRING FullName;

        /// <summary>
        ///  LogonScript member.
        /// </summary>
        public _RPC_UNICODE_STRING LogonScript;

        /// <summary>
        ///  ProfilePath member.
        /// </summary>
        public _RPC_UNICODE_STRING ProfilePath;

        /// <summary>
        ///  HomeDirectory member.
        /// </summary>
        public _RPC_UNICODE_STRING HomeDirectory;

        /// <summary>
        ///  HomeDirectoryDrive member.
        /// </summary>
        public _RPC_UNICODE_STRING HomeDirectoryDrive;

        /// <summary>
        ///  LogonCount member.
        /// </summary>
        public ushort LogonCount;

        /// <summary>
        ///  BadPasswordCount member.
        /// </summary>
        public ushort BadPasswordCount;

        /// <summary>
        ///  UserId member.
        /// </summary>
        public uint UserId;

        /// <summary>
        ///  PrimaryGroupId member.
        /// </summary>
        public uint PrimaryGroupId;

        /// <summary>
        ///  GroupCount member.
        /// </summary>
        public uint GroupCount;

        /// <summary>
        ///  GroupIds member.
        /// </summary>
        [Size("GroupCount")]
        public _GROUP_MEMBERSHIP[] GroupIds;

        /// <summary>
        ///  UserFlags member.
        /// </summary>
        public uint UserFlags;

        /// <summary>
        ///  UserSessionKey member.
        /// </summary>
        public _USER_SESSION_KEY UserSessionKey;

        /// <summary>
        ///  An RPC_UNICODE_STRING structure that contains the NetBIOS
        ///  name of the server that populates this structure.
        /// </summary>
        public _RPC_UNICODE_STRING LogonServer;

        /// <summary>
        ///  LogonDomainName member.
        /// </summary>
        public _RPC_UNICODE_STRING LogonDomainName;

        /// <summary>
        ///  LogonDomainId member.
        /// </summary>
        [StaticSize(1, StaticSizeMode.Elements)]
        public _RPC_SID[] LogonDomainId;

        /// <summary>
        ///  A ten-element array of unsigned 32-bit integers. This
        ///  member has a function similar to that of dummy fields,
        ///  as detailed in section. Each element of the array
        ///  MUST be zero when sent, and MUST be ignored on receipt.
        /// </summary>
        [Inline()]
        [StaticSize(10, StaticSizeMode.Elements)]
        public uint[] ExpansionRoom;

        /// <summary>
        ///  SidCount member.
        /// </summary>
        public uint SidCount;

        /// <summary>
        ///  ExtraSids member.
        /// </summary>
        [Size("SidCount")]
        public _NETLOGON_SID_AND_ATTRIBUTES[] ExtraSids;
    }

    /// <summary>
    ///  The NETLOGON_DOMAIN_INFO structure defines information
    ///  returned as output from the NetrLogonGetDomainInfo
    ///  method, as specified in section. It contains information
    ///  about a domain, including naming information and a
    ///  list of trusted domains.This structure was introduced
    ///  in windows_2000 and is present in windows_xp, windows_server_2003,
    ///  windows_vista, windows_server_2008, windows_7, and
    ///  windows_server_7.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\440c3430-ed5f-481d-b602-74db422df3c8.xml
    //  </remarks>
    public partial struct _NETLOGON_DOMAIN_INFO
    {

        /// <summary>
        ///  A NETLOGON_ONE_DOMAIN_INFO structure, as specified in
        ///  section , that contains information about the domain
        ///  of which the server is a member.
        /// </summary>
        public _NETLOGON_ONE_DOMAIN_INFO PrimaryDomain;

        /// <summary>
        ///  The number of trusted domains listed in TrustedDomains.
        /// </summary>
        public uint TrustedDomainCount;

        /// <summary>
        ///  A pointer to an array of NETLOGON_ONE_DOMAIN_INFO structures,
        ///  as specified in section , which contain information
        ///  about domains with which the current domain has a trust
        ///  relationship.
        /// </summary>
        [Size("TrustedDomainCount")]
        public _NETLOGON_ONE_DOMAIN_INFO[] TrustedDomains;

        /// <summary>
        ///  A NETLOGON_LSA_POLICY_INFO data structure that contains
        ///  the LSA policy for this domain. This field is not used.
        ///  The LsaPolicy.LsaPolicySize field is set to zero, and
        ///  the LsaPolicy.LsaPolicy field is set to NULL.
        /// </summary>
        public _NETLOGON_LSA_POLICY_INFO LsaPolicy;

        /// <summary>
        ///  A null-terminated Unicode string that contains the Active
        ///  DirectoryDNS host name for the client.
        /// </summary>
        public _RPC_UNICODE_STRING DnsHostNameInDs;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString2;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString3;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString4;

        /// <summary>
        ///  A set of bit flags that specify workstation behavior.
        ///  A flag is TRUE (or set) if its value is equal to 1.
        ///  The value is constructed from zero or more bit flags
        ///  from the following table.
        /// </summary>
        public uint WorkstationFlags;

        /// <summary>
        ///  A set of bit flags that specify the encryption types
        ///  supported, as specified in [MS-LSAD] section. See
        ///  [MS-LSAD] for a specification of these bit values and
        ///  their allowed combinations.SupportedEncTypes was added
        ///  in windows_vista and windows_server_2008. windows_server_2003
        ///  and client and server versions of windows_nt, windows_2000,
        ///  and windows_xp  ignore this field.
        /// </summary>
        public uint SupportedEncTypes;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public _NETLOGON_DOMAIN_INFO_DummyLong3_Values DummyLong3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is specified in
        ///  section.
        /// </summary>
        public _NETLOGON_DOMAIN_INFO_DummyLong4_Values DummyLong4;
    }

    /// <summary>
    /// NETLOGON_ONE_DOMAIN_INFO dummy long
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum _NETLOGON_DOMAIN_INFO_DummyLong3_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// NETLOGON_ONE_DOMAIN_INFO dummy long
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum _NETLOGON_DOMAIN_INFO_DummyLong4_Values : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    ///  NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES union.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\548d7519-53d1-4891-92d5-f7d240c73496.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES
    {

        /// <summary>
        ///  An NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES_V1 structure. Added
        ///  in longhorn_server.
        /// </summary>
        [Case("1")]
        public _NL_IN_CHAIN_SET_CLIENT_ATTRIBUTES_V1 V1;
    }

    /// <summary>
    ///  The NETLOGON_VALIDATION union defines a union of all
    ///  types of user validation information values.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\5eb0b7cb-3a55-477b-92fc-c236bd5873fa.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _NETLOGON_VALIDATION
    {

        /// <summary>
        ///  This field is selected when the validation information
        ///  type is NetlogonValidationSamInfo. The selected data
        ///  type is NETLOGON_VALIDATION_SAM_INFO, as specified
        ///  in section.
        /// </summary>
        [Case("2")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_VALIDATION_SAM_INFO[] ValidationSam;

        /// <summary>
        ///  This field is selected when the validation information
        ///  type is NetlogonValidationSamInfo2. The selected data
        ///  type is NETLOGON_VALIDATION_SAM_INFO2, as specified
        ///  in section.
        /// </summary>
        [Case("3")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_VALIDATION_SAM_INFO2[] ValidationSam2;

        /// <summary>
        ///  This field is selected when the validation information
        ///  type is NetlogonValidationGenericInfo2. The selected
        ///  data type is NETLOGON_VALIDATION_GENERIC_INFO2, as
        ///  specified in section.
        /// </summary>
        [Case("5")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_VALIDATION_GENERIC_INFO2[] ValidationGeneric2;

        /// <summary>
        ///  This field is selected when the validation information
        ///  type is NetlogonValidationSamInfo4. The selected data
        ///  type is NETLOGON_VALIDATION_SAM_INFO4, as specified
        ///  in section.
        /// </summary>
        [Case("6")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_VALIDATION_SAM_INFO4[] ValidationSam4;
    }

    /// <summary>
    ///  The NETLOGON_INTERACTIVE_INFO structure defines information
    ///  about an interactive logon instance.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\af76351f-ef69-46bc-a451-e4c4b99bac4a.xml
    //  </remarks>
    public partial struct _NETLOGON_INTERACTIVE_INFO
    {

        /// <summary>
        ///  A NETLOGON_LOGON_IDENTITY_INFO structure, as specified
        ///  in section , that contains information about the logon
        ///  identity.
        /// </summary>
        public _NETLOGON_LOGON_IDENTITY_INFO Identity;

        /// <summary>
        ///  An LM_OWF_PASSWORD structure, as specified in section
        ///  , that contains the LMOWFv1 of a password. LMOWFv1
        ///  is specified in NTLM v1 Authentication in [MS-NLMP]
        ///  section.
        /// </summary>
        public _LM_OWF_PASSWORD LmOwfPassword;

        /// <summary>
        ///  An NT_OWF_PASSWORD structure, as specified in section
        ///  , that contains the NTOWFv1 of a password. NTOWFv1
        ///  is specified in NTLM v1 Authentication in [MS-NLMP]
        ///  section.
        /// </summary>
        public _NT_OWF_PASSWORD NtOwfPassword;
    }

    /// <summary>
    ///  The NETLOGON_DELTA_USER structure contains information
    ///  about a SAM user account. This structure is used for
    ///  replicating the user account data from the PDC to a
    ///  BDC, as detailed in section.All fields of this structure,
    ///  except the fields detailed following the structure
    ///  definition, have the same meanings as the identically
    ///  named fields in the Common User Fields, as specified
    ///  in [MS-SAMR] section  and the SAMPR_USER_INTERNAL1_INFORMATION
    ///  fields, as specified in [MS-SAMR] section.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\c82fcc5a-59b6-47f7-9f69-dec916ebed2c.xml
    //  </remarks>
    public partial struct _NETLOGON_DELTA_USER
    {

        /// <summary>
        ///  UserName member.
        /// </summary>
        public _RPC_UNICODE_STRING UserName;

        /// <summary>
        ///  FullName member.
        /// </summary>
        public _RPC_UNICODE_STRING FullName;

        /// <summary>
        ///  UserId member.
        /// </summary>
        public uint UserId;

        /// <summary>
        ///  PrimaryGroupId member.
        /// </summary>
        public uint PrimaryGroupId;

        /// <summary>
        ///  HomeDirectory member.
        /// </summary>
        public _RPC_UNICODE_STRING HomeDirectory;

        /// <summary>
        ///  HomeDirectoryDrive member.
        /// </summary>
        public _RPC_UNICODE_STRING HomeDirectoryDrive;

        /// <summary>
        ///  ScriptPath member.
        /// </summary>
        public _RPC_UNICODE_STRING ScriptPath;

        /// <summary>
        ///  AdminComment member.
        /// </summary>
        public _RPC_UNICODE_STRING AdminComment;

        /// <summary>
        ///  WorkStations member.
        /// </summary>
        public _RPC_UNICODE_STRING WorkStations;

        /// <summary>
        ///  LastLogon member.
        /// </summary>
        public _OLD_LARGE_INTEGER LastLogon;

        /// <summary>
        ///  LastLogoff member.
        /// </summary>
        public _OLD_LARGE_INTEGER LastLogoff;

        /// <summary>
        ///  LogonHours member.
        /// </summary>
        public _NLPR_LOGON_HOURS LogonHours;

        /// <summary>
        ///  BadPasswordCount member.
        /// </summary>
        public ushort BadPasswordCount;

        /// <summary>
        ///  LogonCount member.
        /// </summary>
        public ushort LogonCount;

        /// <summary>
        ///  PasswordLastSet member.
        /// </summary>
        public _OLD_LARGE_INTEGER PasswordLastSet;

        /// <summary>
        ///  AccountExpires member.
        /// </summary>
        public _OLD_LARGE_INTEGER AccountExpires;

        /// <summary>
        ///  UserAccountControl member.
        /// </summary>
        public uint UserAccountControl;

        /// <summary>
        ///  EncryptedNtOwfPassword member.
        /// </summary>
        public _NT_OWF_PASSWORD EncryptedNtOwfPassword;

        /// <summary>
        ///  EncryptedLmOwfPassword member.
        /// </summary>
        public _LM_OWF_PASSWORD EncryptedLmOwfPassword;

        /// <summary>
        ///  NtPasswordPresent member.
        /// </summary>
        public byte NtPasswordPresent;

        /// <summary>
        ///  LmPasswordPresent member.
        /// </summary>
        public byte LmPasswordPresent;

        /// <summary>
        ///  PasswordExpired member.
        /// </summary>
        public byte PasswordExpired;

        /// <summary>
        ///  UserComment member.
        /// </summary>
        public _RPC_UNICODE_STRING UserComment;

        /// <summary>
        ///  Parameters member.
        /// </summary>
        public _RPC_UNICODE_STRING Parameters;

        /// <summary>
        ///  CountryCode member.
        /// </summary>
        public ushort CountryCode;

        /// <summary>
        ///  CodePage member.
        /// </summary>
        public ushort CodePage;

        /// <summary>
        ///  An NLPR_USER_PRIVATE_INFO structure, as specified in
        ///  section , containing the PrivateData field of the SAMPR_USER_INFORMATION
        ///  structure, as specified in [MS-SAMR] section.
        /// </summary>
        public _NLPR_USER_PRIVATE_INFO PrivateData;

        /// <summary>
        ///  A SECURITY_INFORMATION structure, as specified in [MS-DTYP]
        ///  section , that specifies portions of a security descriptor
        ///  about the user account.
        /// </summary>
        public uint SecurityInformation;

        /// <summary>
        ///  The size, in bytes, of SecurityDescriptor.
        /// </summary>
        public uint SecuritySize;

        /// <summary>
        ///  A pointer to a SECURITY_DESCRIPTOR structure, as specified
        ///  in [MS-DTYP] section , that specifies the security
        ///  settings for the user account object.
        /// </summary>
        [Size("SecuritySize")]
        public byte[] SecurityDescriptor;

        /// <summary>
        ///  ProfilePath member.
        /// </summary>
        public _RPC_UNICODE_STRING ProfilePath;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString2;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString3;

        /// <summary>
        ///  MUST contain 0 for the Length field, 0 for the MaximumLength
        ///  field, and NULL for the Buffer field. It is ignored
        ///  upon receipt. The Netlogon usage of dummy fields is
        ///  described in section.
        /// </summary>
        public _RPC_UNICODE_STRING DummyString4;

        /// <summary>
        ///  The high part (the first 32 bits) of the LastBadPasswordTime
        ///  field of the SAMPR_USER_INTERNAL3_INFORMATION structure,
        ///  as specified in [MS-SAMR] section.
        /// </summary>
        public uint DummyLong1;

        /// <summary>
        ///  The high part (the first 32 bits) of the LastBadPasswordTime
        ///  field of the SAMPR_USER_INTERNAL3_INFORMATION structure,
        ///  as specified in [MS-SAMR] section.
        /// </summary>
        public uint DummyLong2;

        /// <summary>
        ///  The high part (the first 32 bits) of the LastBadPasswordTime
        ///  field of the SAMPR_USER_INTERNAL3_INFORMATION structure,
        ///  as specified in [MS-SAMR] section.
        /// </summary>
        public uint DummyLong3;

        /// <summary>
        ///  The high part (the first 32 bits) of the LastBadPasswordTime
        ///  field of the SAMPR_USER_INTERNAL3_INFORMATION structure,
        ///  as specified in [MS-SAMR] section.
        /// </summary>
        public uint DummyLong4;
    }

    /// <summary>
    ///  The NETLOGON_LEVEL union defines a union of all types
    ///  of logon information.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\d0128545-f74b-4d89-afd9-42621dff24c4.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _NETLOGON_LEVEL
    {

        /// <summary>
        ///  This field is selected when the logon information type
        ///  is NetlogonInteractiveInformation. The data type is
        ///  NETLOGON_INTERACTIVE_INFO, as specified in section
        ///  .
        /// </summary>
        [Case("1")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_INTERACTIVE_INFO[] LogonInteractive;

        /// <summary>
        ///  This field is selected when the logon information type
        ///  is NetlogonInteractiveTransitiveInformation. The data
        ///  type is NETLOGON_INTERACTIVE_INFO, as specified in
        ///  section.
        /// </summary>
        [Case("5")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_INTERACTIVE_INFO[] LogonInteractiveTransitive;

        /// <summary>
        ///  This field is selected when the logon information type
        ///  is NetlogonServiceInformation. The data type is NETLOGON_SERVICE_INFO,
        ///  as specified in section.
        /// </summary>
        [Case("3")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_SERVICE_INFO[] LogonService;

        /// <summary>
        ///  This field is selected when the logon information type
        ///  is NetlogonServiceTransitiveInformation. The data type
        ///  is NETLOGON_SERVICE_INFO, as specified in section.
        /// </summary>
        [Case("7")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_SERVICE_INFO[] LogonServiceTransitive;

        /// <summary>
        ///  This field is selected when the logon information type
        ///  is NetlogonNetworkInformation. The data type is NETLOGON_NETWORK_INFO,
        ///  as specified in section.
        /// </summary>
        [Case("2")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_NETWORK_INFO[] LogonNetwork;

        /// <summary>
        ///  This field is selected when the logon information type
        ///  is NetlogonNetworkTransitiveInformation. The data type
        ///  is NETLOGON_NETWORK_INFO, as specified in section.
        /// </summary>
        [Case("6")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_NETWORK_INFO[] LogonNetworkTransitive;

        /// <summary>
        ///  This field is selected when the logon information type
        ///  is NetlogonGenericInformation. The data type is NETLOGON_GENERIC_INFO,
        ///  as specified in section.
        /// </summary>
        [Case("4")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_GENERIC_INFO[] LogonGeneric;
    }

    /// <summary>
    ///  The NLPR_SID_ARRAY structure defines an array of pointers
    ///  to security identifier structures.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\d89f92c6-bc7c-4af3-a96d-835e9ab03ca0.xml
    //  </remarks>
    public partial struct _NLPR_SID_ARRAY
    {

        /// <summary>
        ///  The number of pointers in the Sids array.
        /// </summary>
        public uint Count;

        /// <summary>
        ///  An array of NLPR_SID_INFORMATION structures, as specified
        ///  in section , each of which is a pointer to a SID.
        /// </summary>
        [Size("Count")]
        public _NLPR_SID_INFORMATION[] Sids;
    }

    /// <summary>
    ///  The NETLOGON_DOMAIN_INFORMATION union selects either
    ///  a NETLOGON_DOMAIN_INFO, as specified in section , or
    ///  a NETLOGON_LSA_POLICY_INFO, as specified in section
    ///  , data type based on the value of the Level parameter
    ///  to the NetrLogonGetDomainInfo method, as specified
    ///  in section.This structure was introduced in windows_2000
    ///  and is present in windows_xp, windows_server_2003,
    ///  windows_vista, windows_server_2008, windows_7, and
    ///  windows_server_7.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\30777159-70ef-4982-90a1-f8e93aaeb985.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _NETLOGON_DOMAIN_INFORMATION
    {

        /// <summary>
        ///  This field is selected when the switched DWORD ([MS-DTYP]
        ///  section) value is set to 0x00000001. The union contains
        ///  a NETLOGON_DOMAIN_INFO structure, as specified in section
        ///  .
        /// </summary>
        [Case("1")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_DOMAIN_INFO[] DomainInfo;

        /// <summary>
        ///  This field is selected when the switched DWORD value
        ///  is set to 0x00000002. The union contains a NETLOGON_LSA_POLICY_INFO
        ///  structure, as specified in section.
        /// </summary>
        [Case("2")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_LSA_POLICY_INFO[] LsaPolicyInfo;
    }

    /// <summary>
    ///  The NETLOGON_DELTA_ALIAS_MEMBER structure contains all
    ///  the members of a SAM alias. This structure is used
    ///  for replicating the SAM alias data from the PDC to
    ///  a BDC, as detailed in section.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\aeca7467-5e13-417c-a4cb-71d1f6476090.xml
    //  </remarks>
    public partial struct _NETLOGON_DELTA_ALIAS_MEMBER
    {

        /// <summary>
        ///  An NLPR_SID_ARRAY structure, as specified in section
        ///  , that contains an array of SIDs for each member of
        ///  the alias.
        /// </summary>
        public _NLPR_SID_ARRAY Members;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong1;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong2;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong3;

        /// <summary>
        ///  MUST be set to zero and MUST be ignored on receipt.
        ///  The Netlogon usage of dummy fields is described in
        ///  section.
        /// </summary>
        public uint DummyLong4;
    }

    /// <summary>
    ///  The NETLOGON_DELTA_UNION union defines a union of all
    ///  types of database changes (deltas).
    /// </summary>
    //  <remarks>
    //   MS-NRPC\3d07c30c-9f0e-4aa3-a9d0-8fc348fd1fe1.xml
    //  </remarks>
    [Union("System.Int32")]
    public partial struct _NETLOGON_DELTA_UNION
    {

        /// <summary>
        ///  A pointer to a NETLOGON_DELTA_DOMAIN structure, as specified
        ///  in section , that describes a domain. This structure
        ///  is selected when the delta type is AddOrChangeDomain.
        /// </summary>
        [Case("1")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_DELTA_DOMAIN[] DeltaDomain;

        /// <summary>
        ///  A pointer to a NETLOGON_DELTA_GROUP structure, as specified
        ///  in section , that describes a group account. This structure
        ///  is selected when the delta type is AddOrChangeGroup.
        /// </summary>
        [Case("2")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_DELTA_GROUP[] DeltaGroup;

        /// <summary>
        ///  A pointer to a NETLOGON_RENAME_GROUP structure, as specified
        ///  in section , that describes a rename of a group account.
        ///  This structure is selected when the delta type is RenameGroup.
        /// </summary>
        [Case("4")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_DELTA_RENAME_GROUP[] DeltaRenameGroup;

        /// <summary>
        ///  A pointer to a NETLOGON_DELTA_USER structure, as specified
        ///  in section , that describes a domain user account.
        ///  This structure is selected when the delta type is AddOrChangeUser.
        /// </summary>
        [Case("5")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_DELTA_USER[] DeltaUser;

        /// <summary>
        ///  A pointer to a NETLOGON_RENAME_USER structure, as specified
        ///  in section , that describes a rename of a user account.
        ///  This structure is selected when the delta type is RenameUser.
        /// </summary>
        [Case("7")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_DELTA_RENAME_USER[] DeltaRenameUser;

        /// <summary>
        ///  A pointer to a NETLOGON_DELTA_GROUP_MEMBER structure,
        ///  as specified in section , that describes a group membership.
        ///  This structure is selected when the delta type is ChangeGroupMembership.
        /// </summary>
        [Case("8")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_DELTA_GROUP_MEMBER[] DeltaGroupMember;

        /// <summary>
        ///  A pointer to a NETLOGON_DELTA_ALIAS structure, as specified
        ///  in section , that describes an alias. This structure
        ///  is selected when the delta type is AddOrChangeAlias.
        /// </summary>
        [Case("9")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_DELTA_ALIAS[] DeltaAlias;

        /// <summary>
        ///  A pointer to a NETLOGON_RENAME_ALIAS structure, as specified
        ///  in section , that describes a rename of an alias. This
        ///  structure is selected when the delta type is RenameAlias.
        /// </summary>
        [Case("11")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_DELTA_RENAME_ALIAS[] DeltaRenameAlias;

        /// <summary>
        ///  A pointer to a NETLOGON_DELTA_ALIAS_MEMBER structure,
        ///  as specified in section , that describes an alias membership.
        ///  This structure is selected when the delta type is ChangeAliasMembership.
        /// </summary>
        [Case("12")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_DELTA_ALIAS_MEMBER[] DeltaAliasMember;

        /// <summary>
        ///  A pointer to a NETLOGON_DELTA_POLICY structure, as specified
        ///  in section , that describes an LSA policy. This structure
        ///  is selected when the delta type is AddOrChangeLsaPolicy.
        /// </summary>
        [Case("13")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_DELTA_POLICY[] DeltaPolicy;

        /// <summary>
        ///  A pointer to a NETLOGON_DELTA_TRUSTED_DOMAINS structure,
        ///  as specified in section , that describes a trusted
        ///  domain. This structure is selected when the delta type
        ///  is AddOrChangeLsaTDomain.
        /// </summary>
        [Case("14")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_DELTA_TRUSTED_DOMAINS[] DeltaTDomains;

        /// <summary>
        ///  A pointer to a NETLOGON_DELTA_ACCOUNTS structure, as
        ///  specified in section , that describes an LSA account.
        ///  This structure is selected when the delta type is AddOrChangeLsaAccount.
        /// </summary>
        [Case("16")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_DELTA_ACCOUNTS[] DeltaAccounts;

        /// <summary>
        ///  A pointer to a NETLOGON_DELTA_SECRET structure, as specified
        ///  in section , that describes a LSA secret object as
        ///  detailed in [MS-LSAD]. This structure is selected when
        ///  the delta type is AddOrChangeLsaSecret.
        /// </summary>
        [Case("18")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_DELTA_SECRET[] DeltaSecret;

        /// <summary>
        ///  A pointer to a NETLOGON_DELTA_DELETE_GROUP structure,
        ///  as specified in section , that describes a group account
        ///  deletion. This structure is selected when the delta
        ///  type is DeleteGroupByName.
        /// </summary>
        [Case("20")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_DELTA_DELETE_GROUP[] DeltaDeleteGroup;

        /// <summary>
        ///  A pointer to a NETLOGON_DELTA_DELETE_USER structure,
        ///  as specified in section , that describes a user account
        ///  deletion. This structure is selected when the delta
        ///  type is DeleteUserByName.
        /// </summary>
        [Case("21")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NETLOGON_DELTA_DELETE_USER[] DeltaDeleteUser;

        /// <summary>
        ///  A pointer to an NLPR_MODIFIED_COUNT structure, as specified
        ///  in section , that holds the database serial number.
        ///  This structure is selected when the delta type is SerialNumberSkip.
        /// </summary>
        [Case("22")]
        [StaticSize(1, StaticSizeMode.Elements)]
        public _NLPR_MODIFIED_COUNT[] DeltaSerialNumberSkip;
    }

    /// <summary>
    ///  The NETLOGON_DELTA_ENUM structure defines a common structure
    ///  that encapsulates all possible types of database changes.
    ///  Database changes, in the context of Netlogon, are called
    ///  deltas.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\20e5863c-0142-40f6-a914-f352a9d66047.xml
    //  </remarks>
    public partial struct _NETLOGON_DELTA_ENUM
    {

        /// <summary>
        ///  One of the values from the NETLOGON_DELTA_TYPE enumeration,
        ///  as specified in section.
        /// </summary>
        public _NETLOGON_DELTA_TYPE DeltaType;

        /// <summary>
        ///  One of the NETLOGON_DELTA_ID_UNION types selected based
        ///  on the value of the DeltaType field.
        /// </summary>
        [Switch("DeltaType")]
        public _NETLOGON_DELTA_ID_UNION DeltaID;

        /// <summary>
        ///  One of the NETLOGON_DELTA_UNION types selected based
        ///  on the value of the DeltaType field.
        /// </summary>
        [Switch("DeltaType")]
        public _NETLOGON_DELTA_UNION DeltaUnion;
    }

    /// <summary>
    ///  The NETLOGON_DELTA_ENUM_ARRAY structure defines an array
    ///  of delta objects.
    /// </summary>
    //  <remarks>
    //   MS-NRPC\82d47318-c4d8-4583-86a6-ce186504bcba.xml
    //  </remarks>
    public partial struct _NETLOGON_DELTA_ENUM_ARRAY
    {

        /// <summary>
        ///  The number of elements in the Deltas field.
        /// </summary>
        public uint CountReturned;

        /// <summary>
        ///  An array of NETLOGON_DELTA_ENUM structures, as specified
        ///  in section.
        /// </summary>
        [Size("CountReturned")]
        public _NETLOGON_DELTA_ENUM[] Deltas;
    }

    /// <summary>
    /// Database ID
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum DatabaseID_Values : uint
    {

        /// <summary>
        ///  Indicates the SAM database.
        /// </summary>
        SamDatabase = 0x00000000,

        /// <summary>
        ///  Indicates the SAM built-in database.
        /// </summary>
        SamBuiltInDatabase = 0x00000001,

        /// <summary>
        ///  Indicates the LSA database.
        /// </summary>
        LsaDatabase = 0x00000002,
    }

    /// <summary>
    /// Function code
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags()]
    public enum FunctionCode_Values : uint
    {

        /// <summary>
        ///  No operation; only the requested information is returned.
        /// </summary>
        NETLOGON_CONTROL_QUERY = 0x00000001,

        /// <summary>
        ///  Forces a BDC to perform an immediate partial synchronization
        ///  of all databasesas detailed in section.Only supported
        ///  on servers that are windows_nt_4_0BDCs; otherwise,
        ///  the ERROR_NOT_SUPPORTED error is returned from a server
        ///  that is not a windows_nt_4_0BDC.
        /// </summary>
        NETLOGON_CONTROL_REPLICATE = 0x00000002,

        /// <summary>
        ///  Forces a BDC to perform an immediate full synchronization
        ///  of all databases.Only supported on servers that are
        ///  windows_nt_4_0BDCs; otherwise, the ERROR_NOT_SUPPORTED
        ///  error is returned from a server that is not a windows_nt_4_0BDC.
        /// </summary>
        NETLOGON_CONTROL_SYNCHRONIZE = 0x00000003,

        /// <summary>
        ///  Forces a PDC to immediately send announcement messages
        ///  to ask each BDC to replicate the databasefor details
        ///  (see section ). Supported only on PDC servers; otherwise,
        ///  the ERROR_NOT_SUPPORTED error is returned from a server
        ///  that is not a PDC.
        /// </summary>
        NETLOGON_CONTROL_PDC_REPLICATE = 0x00000004,

        /// <summary>
        ///  Forces the server to rediscover a domain controller
        ///  in the specified domain and to set up a secure channel
        ///  to the discovered DC (for details, see section ). The
        ///  domain name is specified in the TrustedDomainName field
        ///  of the Data parameter. The domain controller for the
        ///  domain specified in the TrustedDomainName parameter
        ///  is rediscovered in the same way as the DC is initially
        ///  discovered as described in [MS-DISO] section [MS-DISO]
        ///  section 5. If a DC cannot be contacted the ERROR_NO_LOGON_SERVERS
        ///  error is returned.If the server is paused, the ERROR_SERVICE_NOT_ACTIVE
        ///  error is returned.
        /// </summary>
        NETLOGON_CONTROL_REDISCOVER = 0x00000005,

        /// <summary>
        ///  Queries the status of the secure channel to the DC in
        ///  the specified domain, stored in the ConnectionStatus
        ///  field of the ServerSessionInfo table (see section ),
        ///  requesting the status about the last usage of the secure
        ///  channel. The domain name is specified in the TrustedDomainName
        ///  field of the Data parameter.
        /// </summary>
        NETLOGON_CONTROL_TC_QUERY = 0x00000006,

        /// <summary>
        ///  Notifies the Netlogon server that a new network transport
        ///  has been added. Cached DC data SHOULD be flushed. The
        ///  data in LocatedDCsCache and FailedDiscoveryCache SHOULD
        ///  be flushed.
        /// </summary>
        NETLOGON_CONTROL_TRANSPORT_NOTIFY = 0x00000007,

        /// <summary>
        ///  Queries the name of a trusted domain that contains an
        ///  account for a user with the specified name. The user
        ///  name is specified in the UserName field of the Data
        ///  parameter. QueryLevel MUST be set to 4 for this control;
        ///  otherwise, the error ERROR_INVALID_PARAMETER is returned
        ///  by the method. The server MUST be a DC; otherwise,
        ///  the error ERROR_NOT_SUPPORTED is returned.
        /// </summary>
        NETLOGON_CONTROL_FIND_USER = 0x00000008,

        /// <summary>
        ///  Causes the server to generate a new secret key (password)
        ///  and to set it on the account used by the DC in the
        ///  specified domain (see section) for setting up the
        ///  secure channel from the server. The domain name is
        ///  specified in the TrustedDomainName field of the Data
        ///  parameter. If the account is a trust account, the server
        ///  MUST be a PDC; otherwise, the error ERROR_INVALID_DOMAIN_ROLE
        ///  is returned by the method.
        /// </summary>
        NETLOGON_CONTROL_CHANGE_PASSWORD = 0x00000009,

        /// <summary>
        ///  Verifies the current status of the server's secure channel
        ///  to a DC in the specified domain. In contrast, the NETLOGON_CONTROL_TC_QUERY
        ///  control returns the status of the secure channel the
        ///  last time that it was used. The domain name is specified
        ///  in the TrustedDomainName field of the Data parameter.
        ///  QueryLevel MUST be set to 2 for this control; otherwise,
        ///  the error ERROR_INVALID_LEVEL is returned by the method.
        /// </summary>
        NETLOGON_CONTROL_TC_VERIFY = 0x0000000A,

        /// <summary>
        ///  Forces the DC to reregister all of its DNS records.The
        ///  server is a windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, or windows_server_7DC; otherwise, the ERROR_NOT_SUPPORTED
        ///  error is returned.
        /// </summary>
        NETLOGON_CONTROL_FORCE_DNS_REG = 0x0000000B,

        /// <summary>
        ///  Queries the status of DNS updates performed by the Netlogon
        ///  server. If any DNS registration or deregistration errors
        ///  occurred on the last update, the result is negative.
        ///  The QueryLevel parameter MUST be set to 1; otherwise,
        ///  the error ERROR_INVALID_LEVEL is returned. The server
        ///  is a windows_server_2003, windows_vista, windows_server_2008,
        ///  windows_7, or windows_server_7DC; otherwise, the ERROR_NOT_SUPPORTED
        ///  error is returned.
        /// </summary>
        NETLOGON_CONTROL_QUERY_DNS_REG = 0x0000000C,

        /// <summary>
        ///  This value is used for debugging purposes and does not
        ///  affect the Netlogon protocol behavior.In windows, the
        ///  server copies to a backup file the contents of a file
        ///  that contains a cache of database changes.
        /// </summary>
        NETLOGON_CONTROL_BACKUP_CHANGE_LOG = 0x0000FFFC,

        /// <summary>
        ///  This value is used for debugging purposes and does not
        ///  affect the Netlogon protocol behavior.In windows, the
        ///  server truncates the contents of a debug file that
        ///  contains debugging information about the Netlogon service
        ///  operations.
        /// </summary>
        NETLOGON_CONTROL_TRUNCATE_LOG = 0x0000FFFD,

        /// <summary>
        ///  This value is used for debugging purposes and does not
        ///  affect the Netlogon protocol behavior.In windows, the
        ///  server sets the level of verbosity of output into the
        ///  debug file that contains debugging information about
        ///  the Netlogon service operations. The level of verbosity
        ///  to set is specified in the DebugFlag field of the Data
        ///  parameter.
        /// </summary>
        NETLOGON_CONTROL_SET_DBFLAG = 0x0000FFFE,

        /// <summary>
        ///  This value is used for debugging purposes and MUST be
        ///  used only with checked builds.In windows, if the NetrLogonControl2Ex
        ///  method is called with the function code NETLOGON_CONTROL_BREAKPOINT
        ///  and the operating system is not a checked build, the
        ///  method returns ERROR_NOT_SUPPORTED. Calling NetrLogonControl2Ex
        ///  with this function code does not affect the Netlogon
        ///  protocol behavior.In windows, the server breaks into
        ///  the debugger if it is attached to the computer that
        ///  supports debugging.
        /// </summary>
        NETLOGON_CONTROL_BREAKPOINT = 0x0000FFFF,
    }

    /// <summary>
    /// Query level
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum QueryLevel_Values : uint
    {

        /// <summary>
        ///  A NETLOGON_INFO_1 structure is returned.
        /// </summary>
        NetlogonInfo1 = 0x00000001,

        /// <summary>
        ///  A NETLOGON_INFO_2 structure is returned.
        /// </summary>
        NetlogonInfo2 = 0x00000002,

        /// <summary>
        ///  A NETLOGON_INFO_3 structure is returned.
        /// </summary>
        NetlogonInfo3 = 0x00000003,

        /// <summary>
        ///  A NETLOGON_INFO_4 structure is returned.
        /// </summary>
        NetlogonInfo4 = 0x00000004,
    }

    /// <summary>
    /// Level
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags()]
    public enum Level_Values : uint
    {

        /// <summary>
        ///  The buffer contains a NETLOGON_DOMAIN_INFO structure.
        /// </summary>
        NetlogonDomainInfo = 0x00000001,

        /// <summary>
        ///  The buffer contains a NETLOGON_LSA_POLICY_INFO structure.
        /// </summary>
        NetlogonLsaPolicyInfo = 0x00000002,
    }

    /// <summary>
    /// Pdc same site
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags()]
    public enum PdcSameSite_Values : int
    {

        /// <summary>
        ///  The PDC is not in the same site as the server specified
        ///  by ServerName.
        /// </summary>
        False = 0,

        /// <summary>
        ///  The PDC is in the same site as the server specified
        ///  by ServerName.
        /// </summary>
        True = 1,
    }


    /// <summary>
    /// A two-byte set of bit flags that describes the properties 
    /// of the message. A flag is TRUE (or set) if its value is 
    /// equal to 1. The value is constructed from zero or more bit 
    /// flags from the following table, with the exception that bit 
    /// C cannot be combined with bit D.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum CHANGELOG_ENTRY_Flags_Values : ushort
    {
        /// <summary>
        /// No flag is set.
        /// </summary>
        None = 0,

        /// <summary>
        /// A: The object requires immediate replication at 
        /// the moment that the object is changed.
        /// </summary>
        RequiresImmediateReplication = 0x01,

        /// <summary>
        /// B: The object is an account with a changed password.
        /// </summary>
        AccountWithChangedPassword = 0x02,

        /// <summary>
        /// C: The optional ObjectSid field is included in the 
        /// message. Cannot be combined with flag D.
        /// </summary>
        IncludeObjectSid = 0x04,

        /// <summary>
        /// D: The optional ObjectName field is included in the 
        /// message. Cannot be combined with flag C.
        /// </summary>
        IncludeObjectName = 0x08,

        /// <summary>
        /// E: The object is the first object changed after a 
        /// promotion of a BDC to a new PDC.
        /// </summary>
        FirstObjectChanged = 0x10,
    }


    /// <summary>
    /// The following CHANGELOG_ENTRY structure pointed to by the ChangeLogEntry 
    /// parameter carries information about the account object being queried.
    /// </summary>
    public partial struct CHANGELOG_ENTRY
    {
        /// <summary>
        /// The database serial number that corresponds to this 
        /// account object (64-bit integer).
        /// </summary>
        public ulong SerialNumber;

        /// <summary>
        /// The RID of the object (32-bit integer).
        /// </summary>
        public uint ObjectRid;

        /// <summary>
        /// A two-byte set of bit flags that describes the properties 
        /// of the message. A flag is TRUE (or set) if its value is 
        /// equal to 1. The value is constructed from zero or more bit 
        /// flags from the following table, with the exception that bit 
        /// C cannot be combined with bit D.
        /// </summary>
        public CHANGELOG_ENTRY_Flags_Values Flags;

        /// <summary>
        /// The 8-bit integer identifier of the database containing the object. 
        /// MUST be one, and only one, of the following values.
        /// </summary>
        public DBIndex_Values DBIndex;

        /// <summary>
        /// One of the NETLOGON_DELTA_TYPE values specified in section 2.2.1.5.28.
        /// </summary>
        public _NETLOGON_DELTA_TYPE DeltaType;

        /// <summary>
        /// The SID of the object. Included only if flag C is set. This is 
        /// an RPC_SID structure, as defined in [MS-DTYP] section 2.4.2.2.
        /// </summary>
        [MarshalingCondition("IsObjectSidPresent")]
        public _RPC_SID ObjectSid;

        /// <summary>
        /// The name of the object. ObjectName is a null-terminated Unicode 
        /// string, and is included only if flag D is set.
        /// </summary>
        [MarshalingCondition("IsObjectNamePresent")]
        public string ObjectName;

        /// <summary>
        /// IsObjectNamePresent
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static bool IsObjectSidPresent(MarshalingType marshalingType, object value)
        {
            return (((CHANGELOG_ENTRY)value).Flags & CHANGELOG_ENTRY_Flags_Values.IncludeObjectSid) != 0;
        }

        /// <summary>
        /// IsObjectNamePresent
        /// </summary>
        /// <param name="marshalingType">Marshal/Unmarshal</param>
        /// <param name="value">object</param>
        /// <returns>true if request confidentiality</returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static bool IsObjectNamePresent(MarshalingType marshalingType, object value)
        {
            return (((CHANGELOG_ENTRY)value).Flags & CHANGELOG_ENTRY_Flags_Values.IncludeObjectName) != 0;
        }

    }


    /// <summary>
    /// A set of bit flags indicating the principal names carried 
    /// in the request. A flag is TRUE (or set) if its value is 
    /// equal to 1. These flags are set only in negotiate messages. 
    /// The value is constructed from one or more bit flags from 
    /// the following table.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum NL_AUTH_MESSAGE_Flags_Value : uint
    {
        /// <summary>
        /// A: Buffer contains a NetBIOS domain name as an 
        /// OEM_STRING ([MS-CIFS] section 2.2.1.1).
        /// </summary>
        NetbiosOemDomainName = 0x01,

        /// <summary>
        /// B: Buffer contains a NetBIOS computer name as an 
        /// OEM_STRING ([MS-CIFS] section 2.2.1.1).
        /// </summary>
        NetbiosOemComputerName = 0x02,

        /// <summary>
        /// C: Buffer contains a DNS domain name as a 
        /// compressed UTF-8 string, as specified in [RFC1035] section 4.1.4.
        /// </summary>
        DnsCompressedDomainName = 0x04,

        /// <summary>
        /// D: Buffer contains a DNS host name as a 
        /// compressed UTF-8 string, as specified in [RFC1035] section 4.1.4.
        /// </summary>
        DnsCompressedHostName = 0x08,

        /// <summary>
        /// E: Buffer contains a NetBIOS computer name as a 
        /// compressed UTF-8 string, as specified in [RFC1035] section 4.1.4.
        /// </summary>
        NetbiosCompressedComputerName = 0x10,
    }
}
