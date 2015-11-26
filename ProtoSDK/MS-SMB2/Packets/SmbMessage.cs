// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
    /// The extensions in this document do not change the use of this field.
    /// </summary>
    [Flags]
    public enum Flags
    {
        /// <summary>
        /// LOCK_AND_READ and WRITE_AND_CLOSE supported
        /// </summary>
        SMB_FLAGS_LOCK_AND_READ_OK = 0x01,

        /// <summary>
        /// [not implemented]
        /// </summary>
        SMB_FLAGS_SEND_NO_ACK = 0x02,

        /// <summary>
        /// SMB paths are case-insensitive
        /// </summary>
        SMB_FLAGS_CASE_INSENSITIVE = 0x08,

        /// <summary>
        /// Canonicalized File and pathnames (obsoleted)
        /// </summary>
        SMB_FLAGS_CANONICALIZED_PATHS = 0x10,

        /// <summary>
        /// No Oplocks supported for OPEN, CREATE CREATE_NEW (obsoleted)
        /// </summary>
        SMB_FLAGS_OPLOCK = 0x20,

        /// <summary>
        /// No Notifications supported for OPEN, CREATE CREATE_NEW (obsoleted)
        /// </summary>
        SMB_FLAGS_OPLOCK_NOTIFY_ANY = 0x40,

        /// <summary>
        /// Command - SMB is being sent from the client
        /// </summary>
        SMB_FLAGS_SERVER_TO_REDIR = 0x80
    }

    /// <summary>
    /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, 
    /// indicate various client and server capabilities. This field is as specified in [CIFS] sections 2.4.2 and 3.1.2. 
    /// Unused bit fields SHOULD be set to 0 by the sender when sending a response and MUST be ignored when received by the receiver. 
    /// This field may be constructed using the following possible values.
    /// </summary>
    [Flags]
    public enum Flags2
    {
        /// <summary>
        /// May return long file names
        /// </summary>
        SMB_FLAGS2_KNOWS_LONG_NAMES = 0x01,

        /// <summary>
        /// Understands extended attributes
        /// </summary>
        SMB_FLAGS2_KNOWS_EAS = 0x02,

        /// <summary>
        /// security signature enabled
        /// </summary>
        SMB_FLAGS2_SMB_SECURITY_SIGNATURE = 0x04,

        /// <summary>
        /// Reserved
        /// </summary>
        Reserved = 0x10,

        /// <summary>
        /// SMB packets must be signed
        /// </summary>
        SMB_FLAGS2_SMB_SECURITY_SIGNATURE_REQUIRED = 0x20,

        /// <summary>
        /// Any path name in the request is a long name
        /// </summary>
        SMB_FLAGS2_IS_LONG_NAME = 0x40,

        /// <summary>
        /// Requesting Reparse path
        /// </summary>
        SMB_FLAGS2_REPARSE_PATH = 0x400,

        /// <summary>
        /// Aware of extended security
        /// </summary>
        SMB_FLAGS2_EXTENDED_SECURITY = 0x800,

        /// <summary>
        /// DFS namespace
        /// </summary>
        SMB_FLAGS2_DFS = 0x1000,

        /// <summary>
        /// Paging IO
        /// </summary>
        SMB_FLAGS2_PAGING_IO = 0x2000,

        /// <summary>
        /// Using 32-bit NT status error codes
        /// </summary>
        SMB_FLAGS2_NT_STATUS = 0x4000,

        /// <summary>
        /// Using UNICODE strings
        /// </summary>
        SMB_FLAGS2_UNICODE = 0x8000
    }

    /// <summary>
    /// The Dialect version
    /// </summary>
    [Flags()]
    public enum SmbDialectVersion
    {
        /// <summary>
        /// PC NETWORK PROGRAM 1.0
        /// </summary>
        PC_NETWORK_PROGRAM_1_0 = 0x01,

        /// <summary>
        /// LANMAN1.0
        /// </summary>
        LAN_MAN_1_0 = 0x02,

        /// <summary>
        /// Windows for Workgroups 3.1a
        /// </summary>
        WINDOWS_FOR_WORKGROUPS_3_1_a = 0x04,

        /// <summary>
        /// LM1.2X002
        /// </summary>
        LM1_2X002 = 0x08,

        /// <summary>
        /// LANMAN2.1
        /// </summary>
        LAN_MAN_2_1 = 0x10,

        /// <summary>
        /// NT_LM_0_12
        /// </summary>
        NT_LM_0_12 = 0x20,

        /// <summary>
        /// SMB 2.002
        /// </summary>
        SMB_2_002 = 0x40,

        /// <summary>
        /// SMB 2.???
        /// </summary>
        SMB_2_X = 0x80,

    }

    /// <summary>
    /// Null-terminated ASCII string that indicate the SMB dialect supported by the client.
    /// </summary>
    public static class SMBDialects
    {

        /// <summary>
        /// const string definition
        /// </summary>
        public const string PC_NETWORK_PROGRAM = "PC NETWORK PROGRAM 1.0";

        /// <summary>
        /// const string definition
        /// </summary>
        public const string LAN_MAN_1_0 = "LANMAN1.0";

        /// <summary>
        /// const string definition
        /// </summary>
        public const string WINDOWS_FOR_WORKGROUPS = "Windows for Workgroups 3.1a";

        /// <summary>
        /// const string definition
        /// </summary>
        public const string LM1_2X002 = "LM1.2X002";

        /// <summary>
        /// const string definition
        /// </summary>
        public const string LAN_MAN_2_1 = "LANMAN2.1";

        /// <summary>
        /// const string definition
        /// </summary>
        public const string NT_LM_0_12 = "NT LM 0.12";

        /// <summary>
        /// const string definition
        /// </summary>
        public const string SMB_2_002 = "SMB 2.002";

        /// <summary>
        /// const string definition
        /// </summary>
        public const string SMB_2_X = "SMB 2.???";
    }

    /// <summary>
    ///  All client requests and server responses MUST begin
    ///  with a fixed-size SMB header, as specified in [CIFS]
    ///  section 2.4.2.There are several new Flags2 values in
    ///  the SMB header that are not in CIFS, but part of these
    ///  extensions.When message signing is active for a connection,
    ///  the 8 bytes of the SMB header (the SMB header structure)
    ///  previously marked as Unused and Unused2, as specified
    ///  in [CIFS] section 2.4.2, MUST contain the 8-byte message
    ///  signature. These fields are now called the SecuritySignature
    ///  field. If message signing is negotiated but an SMB
    ///  message is received with an invalid SecuritySignature
    ///  field, the underlying transport connection  SHOULD
    ///  be immediately disconnected. For details on message
    ///  signing, see section.The following is a diagram showing
    ///  the SMB header for reference.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SmbHeader
    {
        /// <summary>
        ///  This field MUST be the 4-byte header (0xFF, S,
        ///  M, B) with the letters represented by their ASCII (American
        ///  Standard Code for Information Interchange) characters
        ///  in network byte order. Note that all other multibyte
        ///  fields in the SMB header MUST be encoded in little-endian
        ///  byte order. This field is as specified in [CIFS] section
        ///  2.4.2.
        /// </summary>
        public uint Protocol;

        /// <summary>
        ///  The operation code that this SMB is requesting
        ///  or responding to. A list of the possible values is
        ///  specified in [CIFS] section 6.1. This field is as specified
        ///  in [CIFS] section 2.4.2. The following commands, listed
        ///  below for completeness, are not SMB commands. The commands
        ///  are used by an unrelated protocol that has adopted
        ///  the SMB message headers, called the SMB Message Delivery
        ///  Protocol, as specified in [MS-MSRP] section 2.2.6.
        ///  An SMB client implementation MUST NOT send these commands,
        ///  and an SMB server implementation MUST reject these
        ///  as bad commands. 
        /// </summary>
        public byte Command;

        /// <summary>
        ///  An error code that indicates success, warning,
        ///  or failure returned by the server for an operation.
        ///  This field MUST indicate the error code of any failed
        ///  server operation. When an error is returned, the server
        ///  MAY choose to return only the header portion of the
        ///  response SMB.This field is represented in network byte
        ///  order below and is as specified in [CIFS] section 2.4.2.If
        ///  a client is capable of receiving 32-bit error returns
        ///  (for details on CAP_STATUS32, see sections  and ),
        ///  the status MUST be returned in a 32-bit error code
        ///  in this field. These 32-bit error codes are specified
        ///  in [MS-ERREF] section 4. Otherwise, this field MUST
        ///  be interpreted as two subfields that are set by the
        ///  server, and then combined to give the error code of
        ///  any failed server operation, as shown below.01234567891
        ///  01234567892 01234567893 01 ErrorClass 
        ///  Reserved Error ErrorClass:
        ///  An 8-bit value that MUST be set to the error class
        ///  for this error. This field is called Status.DosError.ErrorClass,
        ///  as specified in [CIFS]. The possible values for this
        ///  field are as specified in [CIFS] section 7.Reserved:
        ///  An unused 8-bit field. This field SHOULD be set to
        ///  0 when the SMB header is sent, and MUST be ignored
        ///  when the SMB header is received.Error A 16-bit value
        ///  that MUST be set to the error code for this error.
        ///  This field is called Status.DosError.Error, as specified
        ///  in [CIFS]. The possible values for this field are as
        ///  specified in [CIFS] section 7.
        /// </summary>
        public uint Status;

        /// <summary>
        ///  The Flags field contains individual flags, as
        ///  specified in [CIFS] sections 2.4.2 and 3.1.1. The extensions
        ///  in this document do not change the use of this field.
        ///  The SMB_FLAGS_SEND_NO_ACK bit field is not set
        ///  when using direct TCP, NetBIOS over TCP, NetBIOS over
        ///  IPX, or NetBEUI.
        /// </summary>
        public byte Flags;

        /// <summary>
        ///  The Flags2 field contains individual bit flags
        ///  that, depending on the negotiated SMB dialect, indicate
        ///  various client and server capabilities. This field
        ///  is as specified in [CIFS] sections 2.4.2 and 3.1.2.
        ///  Unused bit fields SHOULD be set to 0 by the server
        ///  when sending a response, and MUST be ignored when received
        ///  by the client. This field MAY be constructed using
        ///  the following possible values.
        /// </summary>
        public ushort Flags2;

        /// <summary>
        ///  This field MUST give the 2 high bytes of the process
        ///  identifier if the client wants to use 32-bit process
        ///  IDs, as specified in [CIFS] section 2.4.2. If a client
        ///  uses 16-bit process IDs, this field MUST be set to
        ///  0.-based SMB clients on  and later versions support
        ///  32-bit process IDs and use this field when sending
        ///  the following SMB messages: SMB_COM_NT_CREATE_AND and
        ///  SMB_COM_OPEN_PRINT_FILE. -based SMB clients on  and
        ///  later versions also support and use this field when
        ///  sending SMB_COM_NT_TRANSACT, SMB_COM_TRANSACTION, and
        ///  SMB_COM_TRANSACTION2 messages when the server supports
        ///  the CAP_NT_SMBS bit. (The CAP_NT_SMBS bit was set in
        ///  the Capabilities field in the SMB_COM_NEGOTIATE response.)
        ///   Other -based SMB clients do not support 32-bit process
        ///  IDs and set this field to 0 when sending SMB messages.
        ///  -based SMB servers support 32-bit process IDs when
        ///  receiving SMB messages.
        /// </summary>
        public ushort PidHigh;

        /// <summary>
        ///  This field is the combination of the fields defined
        ///  as Unused and Unused2, as specified in [CIFS] section
        ///  2.4.2. It SHOULD be used to carry a secure signature
        ///  when message signing is enabled, as specified in section
        ///  .
        /// </summary>
        public ulong SecuritySignature;

        /// <summary>
        ///  This field is unused. The sender SHOULD set this to
        ///  0, and the receiver MUST ignore this field.
        /// </summary>
        public ushort Reserve;

        /// <summary>
        ///  This field identifies the subdirectory (or tree) (also
        ///  referred to as a share in this document) on the server
        ///  that the client is accessing, as specified in [CIFS]
        ///  sections 2.4.2 and 3.1.3. The description of this field,
        ///  as specified in [CIFS] section 2.4.2, SHOULD be corrected
        ///  as follows: For messages that do not reference a particular
        ///  tree, this field MUST be set to 0xFFFF or 0. Both values
        ///  are acceptable.
        /// </summary>
        public ushort Tid;

        /// <summary>
        ///  Caller's process ID, as specified in [CIFS] sections
        ///  2.4.2 and 3.1.4; it MUST be generated by the client
        ///  to uniquely identify a process within the client computer.
        ///  If the client wants to use 32-bit process IDs, it also
        ///  MUST set the PidHigh field to the two most significant
        ///  bytes of the caller's process ID.
        /// </summary>
        public ushort Pid;

        /// <summary>
        ///  This field SHOULD identify the authenticated instance
        ///  of the user, as specified in [CIFS] sections 2.4.2
        ///  and 2.8.
        /// </summary>
        public ushort Uid;

        /// <summary>
        ///  This field SHOULD be the multiplex ID that is used to
        ///  associate a response with a request, as specified in
        ///  [CIFS] sections 2.4.2 and 3.1.5.
        /// </summary>
        public ushort Mid;
    }

    /// <summary>
    ///  An SMB_COM_NEGOTIATE request MUST be sent by a client
    ///  as the initial packet that handles dialect and capability
    ///  negotiation. The client SHOULD send a list of SMB dialects
    ///  that it can communicate with. The server response MUST
    ///  be a selection of one of those dialects or an error
    ///  indicating that none of the dialects were acceptable.
    ///  The SMB_COM_NEGOTIATE message MUST be sent from the
    ///  client to the server to establish an SMB connection.
    ///  Only one SMB_COM_NEGOTIATE request SHOULD be sent between
    ///  a client and a server over an SMB transport while an
    ///  SMB connection is active on the SMB transport. Subsequent
    ///  SMB_COM_NEGOTIATE requests from a client to a server
    ///  over the same SMB transport while an SMB connection
    ///  is still established SHOULD be rejected with an error
    ///  response, and no action SHOULD be taken.-based SMB
    ///  clients send only a single SMB_COM_NEGOTIATE request
    ///  over an SMB transport while an SMB connection is active
    ///  on the SMB transport, as specified in [CIFS] section
    ///  4.1.1.The client request for negotiation is identical
    ///  in this specification to what is specified in the base
    ///  protocol, with the exception of the new flags in the
    ///  SMB Header, as specified in section. The SMB_FLAGS2_EXTENDED_SECURITY
    ///  bit, when set, indicates support for specification
    ///  [RFC4178] and GSS authentication. For more information,
    ///  see section 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SmbNegotiateRequest
    {
        /// <summary>
        ///    Word count for this request MUST be 0 because
        ///  there are zero 16-bit WORDs between the WordCount field
        ///  and the byteCount field.
        /// </summary>
        public byte WordCount;

        /// <summary>
        ///  Count of data bytes in the packet. This field has a
        ///  minimum value of 0x0002. This field MUST be the total
        ///  length of the combined BufferFormat and DialectName
        ///  fields.
        /// </summary>
        /// [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 2)]
        public ushort ByteCount;

        /// <summary>
        ///  Array of null-terminated ASCII strings that indicate
        ///  the SMB dialects supported by the client. The protocol
        ///  does not impose any particular structure on the SMB
        ///  dialect strings. Implementers of particular protocols
        ///  MAY choose to include, for example, version numbers
        ///  in the string.To support the extended version of the
        ///  SMB protocol, the client MUST include the NT LM 0.12
        ///  dialect in the SMB_COM_NEGOTIATE client request. The
        ///  list of all possible dialects recognized by various
        ///  -based clients and servers includes the following:
        ///  Symbolic name DialectName string
        ///   PCNET1 PC NETWORK PROGRAM 1.0
        ///  XENIXCORE XENIX CORE
        ///  PCLAN1 PCLAN1.0 
        ///  MSNET103 MICROSOFT NETWORKS
        ///  1.03 MSNET30 MICROSOFT
        ///  NETWORKS 3.0 LANMAN10 LANMAN1.0
        ///   WFW10 Windows for Workgroups
        ///  3.1a  DOSLANMAN12  DOS
        ///  LM1.2X002 LANMAN12 LM1.2X002
        ///   DOSLANMAN21  DOS LANMAN2.1
        ///   LANMAN21  LANMAN2.1
        ///   NTLANMAN  NT LM 0.12
        ///   SMB2  SMB 2.001 -based
        ///  clients list SMB dialects in the DialectName field
        ///  from the earliest supported dialect to the latest supported
        ///  dialect. SMB clients that run on  and later send a
        ///  DialectName array field that contains  dialect strings
        ///  for the following symbolic dialect names: PCNET1, LANMAN10,
        ///  WFW10, LANMAN12, LANMAN21, and NTLANMAN. SMB clients
        ///  that run on  send a DialectName array field that contains
        ///  dialect strings for the following symbolic dialect
        ///  names: PCNET1, XENIXCORE, MSNET103, LANMAN10, WFW10,
        ///  LANMAN12, LANMAN21, and NTLANMAN. SMB clients that
        ///  run on  and  send a DialectName array field that contains
        ///  the dialect strings for the following symbolic dialect
        ///  names: PCNET1, MSNET30, DOSLANMAN12, DOSLANMAN21, WFW10,
        ///  and NTLANMAN.  and future SMB clients that support
        ///  SMB2 will also send the SMB2 dialect string, in addition
        ///  to the dialect string sent by  and later as described
        ///  above. If the SMB2 dialect is negotiated, the Server
        ///  Message Block (SMB) Version 2.0 Protocol, as specified
        ///  in  [MS-SMB2], is used instead of [CIFS] with these
        ///  extensions.
        /// </summary>
        [Size("ByteCount")]
        public byte[] DialectName;
    }

    /// <summary>
    /// Smb command
    /// </summary>
    public enum SmbCommand
    {
        /// <summary>
        /// Negotiate
        /// </summary>
        Negotiate = 0x72
    }

    public static class SmbDialects
    {
        public const string PCNetworkProgram10 = "PC NETWORK PROGRAM 1.0";
        public const string LanMan10 = "LANMAN1.0";
        public const string WindowsForWorkgroups31a = "Windows for Workgroups 3.1a";
        public const string LM12X002 = "LM1.2X002";
        public const string LanMan21 = "LANMAN2.1";
        public const string NTLM012 = "NT LM 0.12";
    }
}
