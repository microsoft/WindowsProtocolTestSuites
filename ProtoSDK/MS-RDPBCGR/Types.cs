// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk.Asn1;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Gcc;
using Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr.Mcs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr
{
    /// <summary>
    /// Specify the encrypted protocol will be used for RDPBCGR transport.
    /// </summary> 
    public enum EncryptedProtocol
    {
        /// <summary>
        /// Standard RDP Security
        /// </summary>
        Rdp,

        /// <summary>
        /// Enhanced RDP Security, External Security Protocol is TLS with Negotiation-Based Approach
        /// </summary>
        NegotiationTls,

        /// <summary>
        /// Enhanced RDP Security, External Security Protocol is CredSSP with Negotiation-Based Approach
        /// </summary>
        NegotiationCredSsp,

        /// <summary>
        /// Enhanced RDP Security, External Security Protocol is CredSSP with Direct Approach
        /// </summary>
        DirectCredSsp,
    }

    /// <summary>
    /// Specify the type of security header.
    /// </summary> 
    public enum SecurityHeaderType : byte
    {
        /// <summary>
        /// No security header
        /// </summary>
        None,

        /// <summary>
        /// Basic security header type
        /// </summary>
        Basic,

        /// <summary>
        /// security header type for Non-FIPS
        /// </summary>
        NonFips,

        /// <summary>
        /// security header type for FIPS
        /// </summary>
        Fips,
    }

    /// <summary>
    /// A class wraps UInt32. This class is used especially for some optional integer fields.
    /// To set it null means not the field of this type will not present, or else it is present.
    /// </summary> 
    public class UInt32Class
    {
        /// <summary>
        /// The actual data hold.
        /// </summary> 
        public UInt32 actualData;

        /// <summary>
        /// Constructor to fill actualData
        /// </summary> 
        public UInt32Class(UInt32 interger)
        {
            actualData = interger;
        }
    }

    /// <summary>
    /// A class wraps UInt16. This class is used especially for some optional integer fields.
    /// To set it null means not the field of this type will not present, or else it is present.
    /// </summary> 
    public class UInt16Class
    {
        /// <summary>
        /// The actual data hold.
        /// </summary> 
        public UInt16 actualData;

        /// <summary>
        /// Constructor to fill actualData
        /// </summary> 
        public UInt16Class(UInt16 interger)
        {
            actualData = interger;
        }
    }

    /// <summary>
    /// A class wraps byte. This class is used especially for some optional integer fields.
    /// To set it null means not the field of this type will not present, or else it is present.
    /// </summary> 
    public class ByteClass
    {
        /// <summary>
        /// The actual data hold.
        /// </summary> 
        public byte actualData;

        /// <summary>
        /// Constructor to fill actualData
        /// </summary> 
        public ByteClass(byte interger)
        {
            actualData = interger;
        }
    }

    /// <summary>
    /// A type of PDU to indicate the session key should be updated.
    /// </summary> 
    public class UpdateSessionKeyPdu : StackPacket
    {
        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            return null;
        }


        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            return null;
        }
    }

    /// <summary>
    /// A type of PDU to indicate some errors.
    /// </summary> 
    public class ErrorPdu : RdpbcgrClientPdu
    {
        /// <summary>
        /// The exception caught from decoder.
        /// </summary> 
        private Exception e;

        /// <summary>
        /// The data of the error PDU in bytes.
        /// </summary>
        private byte[] pduData;

        /// <summary>
        /// The error message.
        /// </summary> 
        public string ErrorMessage
        {
            get
            {
                return e.ToString();
            }
        }

        /// <summary>
        /// Create an ErrorPdu.
        /// </summary>
        /// <param name="e">The decoding exception.</param>
        public ErrorPdu(Exception e)
        {
            this.e = e;
        }

        /// <summary>
        /// Create an ErrorPdu.
        /// </summary>
        /// <param name="e">The decoding exception.</param>
        /// <param name="packetData">The pdu data in bytes.</param>
        public ErrorPdu(Exception e, byte[] packetData)
        {
            this.e = e;
            if (packetData != null)
            {
                pduData = new byte[packetData.Length];
                Array.Copy(packetData, pduData, packetData.Length);
            }
        }


        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            return null;
        }


        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            return pduData;
        }
    }

    /// <summary>
    /// A type of PDU to indicate the PDU is a RDPBCGR PDU. It's a base class for all input/output PDUs.
    /// </summary> 
    public abstract class RdpbcgrClientPdu : StackPacket
    {
        /// <summary>
        /// The client context used to do encryption.
        /// </summary>
        protected RdpbcgrClientContext context;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        protected RdpbcgrClientPdu(RdpbcgrClientContext clientContext)
        {
            context = clientContext;
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        protected RdpbcgrClientPdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            return null;
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            return null;
        }
    }

    public abstract class RdpbcgrServerPdu : StackPacket
    {
        /// <summary>
        /// The server context used to do encryption.
        /// </summary>
        protected RdpbcgrServerSessionContext serverSessionContext;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        protected RdpbcgrServerPdu(RdpbcgrServerSessionContext session)
        {
            serverSessionContext = session;
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        protected RdpbcgrServerPdu()
        {
        }
    }

    /// <summary>
    /// A TPKT packet Header.
    /// </summary>
    public struct TpktHeader
    {
        /// <summary>
        /// A version number, with binary value 00000011.
        /// </summary>
        public byte version;

        /// <summary>
        /// Reserved.
        /// </summary>
        public byte reserved;

        /// <summary>
        /// Octets 3 and 4 are the unsigned 16-bit binary encoding of the TPKT length. 
        /// This is the length of the entire packet in octets, including both the packet header and the TPDU.
        /// </summary>
        public UInt16 length;
    };

    /// <summary>
    /// A x224 request header.
    /// </summary> 
    [StructLayout(LayoutKind.Explicit, Size = 7)]
    public struct X224Crq
    {
        /// <summary>
        /// lengthIndicator field of X224
        /// </summary>
        [FieldOffset(0)]
        public byte lengthIndicator;

        /// <summary>
        /// typeCredit field of X224
        /// </summary>
        [FieldOffset(1)]
        public byte typeCredit;

        /// <summary>
        /// destRef field of X224
        /// </summary>
        [FieldOffset(2)]
        public UInt16 destRef;

        /// <summary>
        /// srcRef field of X224
        /// </summary>
        [FieldOffset(4)]
        public UInt16 srcRef;

        /// <summary>
        /// classOptions field of X224
        /// </summary>
        [FieldOffset(6)]
        public byte classOptions;
    };

    /// <summary>
    /// A x224 connection confirm header.
    /// </summary> 
    [StructLayout(LayoutKind.Explicit, Size = 7)]
    public struct X224Ccf
    {
        /// <summary>
        /// lengthIndicator field of X224
        /// </summary>
        [FieldOffset(0)]
        public byte lengthIndicator;

        /// <summary>
        /// typeCredit field of X224
        /// </summary>
        [FieldOffset(1)]
        public byte typeCredit;

        /// <summary>
        /// destRef field of X224
        /// </summary>
        [FieldOffset(2)]
        public UInt16 destRef;

        /// <summary>
        /// srcRef field of X224
        /// </summary>
        [FieldOffset(4)]
        public UInt16 srcRef;

        /// <summary>
        /// classOptions field of X224
        /// </summary>
        [FieldOffset(6)]
        public byte classOptions;
    };

    /// <summary>
    /// A common x224 header.
    /// </summary> 
    public struct X224
    {
        /// <summary>
        /// The length of X224 header
        /// </summary>
        public byte length;

        /// <summary>
        /// The type of X224 header
        /// </summary>
        public byte type;

        /// <summary>
        /// The eot of X224 header
        /// </summary>
        public byte eot;
    };

    /// <summary>
    /// A slow path header for input/output PDUs.
    /// </summary>
    public struct SlowPathPduCommonHeader
    {

        /// <summary>
        ///  A TPKT Header, as specified in [T123] section 8.
        /// </summary>
        public TpktHeader tpktHeader;

        /// <summary>
        ///  An X.224 Class 0 Data TPDU, as specified in  [X224]
        ///  section 13.7.
        /// </summary>
        public X224 x224Data;

        /// <summary>
        /// For client input PDUs, this means user channel id.
        /// </summary>
        public ushort initiator;

        /// <summary>
        /// Used for both client input and output PDUs.
        /// </summary>
        public ushort channelId;

        /// <summary>
        /// The type in T125 PDU.
        /// The field is used by output PDUs only.
        /// </summary>
        public byte type;

        /// <summary>
        /// The level in T125 PDU.
        /// The field is used by output PDUs only.
        /// </summary>
        public byte level;

        /// <summary>
        /// The length of userData in T125 PDU.
        /// The field is used by output PDUs only.
        /// </summary>
        public uint userDataLength;

        /// <summary>
        ///  The security header.
        /// </summary>
        public TS_SECURITY_HEADER securityHeader;

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public SlowPathPduCommonHeader Clone()
        {
            SlowPathPduCommonHeader cloneHeader = this;
            if (securityHeader != null)
            {
                cloneHeader.securityHeader = securityHeader.Clone();
            }

            return cloneHeader;
        }
    }

    /// <summary>
    ///  The X.224 Connection Request PDU is a Standard RDP Connection
    ///  Sequence PDU sent from client to server during the
    ///  Connection Initiation phase (see section ).
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/_rfc_ms-rdpbcgr2_1_1_1.xml
    //  </remarks>
    public partial class Client_X_224_Connection_Request_Pdu : RdpbcgrClientPdu
    {

        /// <summary>
        ///  A TPKT Header, as specified in [T123] section 8.
        /// </summary>
        public TpktHeader tpktHeader;

        /// <summary>
        ///  An X.224 Class 0 Connection Request TPDU, as specified
        ///  in [X224] section 13.3.
        /// </summary>
        public X224Crq x224Crq;

        /// <summary>
        ///  Optional and variable-length routing token bytes used
        ///  for load balancing terminated by a carriage-return
        ///  (CR) and line-feed (LF) ANSI sequence. For more information,
        ///  see [MSFT-SDLBTS]. The length of the routing token
        ///  and CR+LF sequence is included in the X.224 Connection
        ///  Request Length Indicator field.
        /// </summary>
        public byte[] routingToken;

        /// <summary>
        ///  optional and variable-length ANSI character string 
        ///  terminated by a 0x0D0A two-byte sequence.
        ///  This field MUST NOT be present if the routingToken field is present.
        /// </summary>
        public String cookie;

        /// <summary>
        ///  An optional RDP Negotiation Request structure. The length
        ///  of this negotiation structure is included in the X.224
        ///  Connection Request Length Indicator field.
        /// </summary>
        public RDP_NEG_REQ rdpNegData;

        /// <summary>
        /// An optional Correlation Info (section 2.2.1.1.2) structure. 
        /// This field MUST be present if the CORRELATION_INFO_PRESENT (0x08) flag is set in the flags field of the RDP Negotiation Request structure, 
        /// encapsulated within the optional rdpNegRsp field. If the CORRELATION_INFO_PRESENT (0x08) flag is not set, then this field MUST NOT be present.
        /// </summary>
        public RDP_NEG_CORRELATION_INFO rdpCorrelationInfo;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Client_X_224_Connection_Request_Pdu(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Client_X_224_Connection_Request_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(totalBuffer, tpktHeader);
            RdpbcgrEncoder.EncodeStructure(totalBuffer, x224Crq);
            if (routingToken != null)
            {
                RdpbcgrEncoder.EncodeBytes(totalBuffer, routingToken);
            }

            if (rdpNegData != null)
            {
                RdpbcgrEncoder.EncodeStructure(totalBuffer, (byte)rdpNegData.type);
                RdpbcgrEncoder.EncodeStructure(totalBuffer, (byte)rdpNegData.flags);
                RdpbcgrEncoder.EncodeStructure(totalBuffer, (ushort)rdpNegData.length);
                RdpbcgrEncoder.EncodeStructure(totalBuffer, (uint)rdpNegData.requestedProtocols);
            }
            if (rdpCorrelationInfo != null)
            {
                RdpbcgrEncoder.EncodeStructure(totalBuffer, (byte)rdpCorrelationInfo.type);
                RdpbcgrEncoder.EncodeStructure(totalBuffer, (byte)rdpCorrelationInfo.flags);
                RdpbcgrEncoder.EncodeStructure(totalBuffer, (ushort)rdpCorrelationInfo.length);
                RdpbcgrEncoder.EncodeBytes(totalBuffer, rdpCorrelationInfo.correlationId);
                RdpbcgrEncoder.EncodeBytes(totalBuffer, rdpCorrelationInfo.reserved);
            }

            return totalBuffer.ToArray();
        }


        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Client_X_224_Connection_Request_Pdu cloneX224Pdu = new Client_X_224_Connection_Request_Pdu(context);

            cloneX224Pdu.tpktHeader = tpktHeader;
            cloneX224Pdu.x224Crq = x224Crq;

            if (routingToken != null)
            {
                cloneX224Pdu.routingToken = (byte[])routingToken.Clone();
            }

            if (rdpNegData != null)
            {
                cloneX224Pdu.rdpNegData = new RDP_NEG_REQ();
                cloneX224Pdu.rdpNegData.type = rdpNegData.type;
                cloneX224Pdu.rdpNegData.flags = rdpNegData.flags;
                cloneX224Pdu.rdpNegData.length = rdpNegData.length;
                cloneX224Pdu.rdpNegData.requestedProtocols = rdpNegData.requestedProtocols;
            }

            return cloneX224Pdu;
        }
    }

    /// <summary>
    ///  The X.224 Connection Confirm PDU is a Standard RDP Connection
    ///  Sequence PDU sent from server to client during the
    ///  Connection Initiation phase (see section ). It is sent
    ///  as a response to the X.224 Connection Request PDU.
    /// </summary>
    //  <remarks>
    //   file:///D:/TD/_rfc_ms-rdpbcgr2_1_1_2.xml
    //  </remarks>
    public partial class Server_X_224_Connection_Confirm_Pdu : RdpbcgrServerPdu
    {

        /// <summary>
        ///  A TPKT Header, as specified in [T123] section 8.
        /// </summary>
        public TpktHeader tpktHeader;

        /// <summary>
        ///  An X.224 Class 0 Connection Confirm TPDU, as specified
        ///  in [X224] section 13.4.
        /// </summary>
        public X224Ccf x224Ccf;

        /// <summary>
        ///  Optional RDP Negotiation Response structure or an optional
        ///  RDP Negotiation Failure structure. The length of the
        ///  negotiation structure is included in the X.224 Connection
        ///  Confirm Length Indicator field.
        /// </summary>
        public RDP_NEG_RSP rdpNegData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext"></param>
        public Server_X_224_Connection_Confirm_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_X_224_Connection_Confirm_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(totalBuffer, tpktHeader);
            RdpbcgrEncoder.EncodeStructure(totalBuffer, x224Ccf);
            if (rdpNegData != null)
            {
                RdpbcgrEncoder.EncodeStructure(totalBuffer, (byte)rdpNegData.type);
                RdpbcgrEncoder.EncodeStructure(totalBuffer, (byte)rdpNegData.flags);
                RdpbcgrEncoder.EncodeStructure(totalBuffer, (ushort)rdpNegData.length);
                RdpbcgrEncoder.EncodeStructure(totalBuffer, (uint)rdpNegData.selectedProtocol);
            }

            byte[] encodedBytes = totalBuffer.ToArray();

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes);

            return encodedBytes;
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Server_X_224_Connection_Confirm_Pdu cloneServerX224Confirm = new Server_X_224_Connection_Confirm_Pdu(serverSessionContext);

            cloneServerX224Confirm.tpktHeader = tpktHeader;
            cloneServerX224Confirm.x224Ccf = x224Ccf;
            if (rdpNegData != null)
            {
                cloneServerX224Confirm.rdpNegData = new RDP_NEG_RSP();
                cloneServerX224Confirm.rdpNegData.flags = rdpNegData.flags;
                cloneServerX224Confirm.rdpNegData.length = rdpNegData.length;
                cloneServerX224Confirm.rdpNegData.selectedProtocol = rdpNegData.selectedProtocol;
                cloneServerX224Confirm.rdpNegData.type = rdpNegData.type;
            }

            return cloneServerX224Confirm;
        }
    }

    /// <summary>
    /// The RDP Negotiation Failure structure is used by a server to inform the client of 
    /// a failure that has occurred while preparing security for the connection.
    /// </summary>
    public partial class Server_X_224_Negotiate_Failure_Pdu : RdpbcgrServerPdu
    {

        /// <summary>
        ///  A TPKT Header, as specified in [T123] section 8.
        /// </summary>
        public TpktHeader tpktHeader;

        /// <summary>
        ///  An X.224 Class 0 Connection Confirm TPDU, as specified
        ///  in [X224] section 13.4.
        /// </summary>
        public X224Ccf x224Ccf;

        /// <summary>
        ///  Optional RDP Negotiation Response structure or an optional
        ///  RDP Negotiation Failure structure. The length of the
        ///  negotiation structure is included in the X.224 Connection
        ///  Confirm Length Indicator field.
        /// </summary>
        public RDP_NEG_FAILURE rdpNegFailure;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext"></param>
        public Server_X_224_Negotiate_Failure_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_X_224_Negotiate_Failure_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(totalBuffer, tpktHeader);
            RdpbcgrEncoder.EncodeStructure(totalBuffer, x224Ccf);

            if (rdpNegFailure != null)
            {
                RdpbcgrEncoder.EncodeStructure(totalBuffer, (byte)rdpNegFailure.type);
                RdpbcgrEncoder.EncodeStructure(totalBuffer, (byte)rdpNegFailure.flags);
                RdpbcgrEncoder.EncodeStructure(totalBuffer, (ushort)rdpNegFailure.length);
                RdpbcgrEncoder.EncodeStructure(totalBuffer, (uint)rdpNegFailure.failureCode);
            }

            byte[] encodedBytes = totalBuffer.ToArray();

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes);

            return encodedBytes;
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Server_X_224_Negotiate_Failure_Pdu cloneServerX224Failure = new Server_X_224_Negotiate_Failure_Pdu(serverSessionContext);

            cloneServerX224Failure.tpktHeader = tpktHeader;
            cloneServerX224Failure.x224Ccf = x224Ccf;
            if (rdpNegFailure != null)
            {
                cloneServerX224Failure.rdpNegFailure = new RDP_NEG_FAILURE();
                cloneServerX224Failure.rdpNegFailure.failureCode = rdpNegFailure.failureCode;
                cloneServerX224Failure.rdpNegFailure.flags = rdpNegFailure.flags;
                cloneServerX224Failure.rdpNegFailure.length = rdpNegFailure.length;
                cloneServerX224Failure.rdpNegFailure.type = rdpNegFailure.type;
            }

            return cloneServerX224Failure;
        }
    }

    /// <summary>
    /// DomainParameter used in MCSConnectInitial.
    /// </summary>
    public struct DomainParameter
    {
        /// <summary>
        /// The maxChannelIds field of DomainParameters in [T125].
        /// </summary>
        public long maxChannelIds;

        /// <summary>
        /// The maxUserIds field of DomainParameters in [T125].
        /// </summary>
        public long maxUserIds;

        /// <summary>
        /// The maxTokenIds field of DomainParameters in [T125].
        /// </summary>
        public long maxTokenIds;

        /// <summary>
        /// The numPriorities field of DomainParameters in [T125].
        /// </summary>
        public long numPriorities;

        /// <summary>
        /// The minThroughput field of DomainParameters in [T125].
        /// </summary>
        public long minThroughput;

        /// <summary>
        /// The maxHeight field of DomainParameters in [T125].
        /// </summary>
        public long maxHeight;

        /// <summary>
        /// The maxMcsPduSize field of DomainParameters in [T125].
        /// </summary>
        public long maxMcsPduSize;

        /// <summary>
        /// The protocolVersion field of DomainParameters in [T125].
        /// </summary>
        public long protocolVersion;
    }

    /// <summary>
    /// Gcc used in MCSConnectInitial.
    /// </summary>
    public class ConnectGCC
    {
        /// <summary>
        /// The conferenceName field of GCC Conference Create Request in [T124].
        /// </summary>
        public string conferenceName;

        /// <summary>
        /// The convenerPassword field of GCC Conference Create Request in [T124].
        /// </summary>
        public string convenerPassword;

        /// <summary>
        /// The password field of GCC Conference Create Request in [T124].
        /// </summary>
        public string password;

        /// <summary>
        /// The lockedConference field of GCC Conference Create Request in [T124].
        /// </summary>
        public bool lockedConference;

        /// <summary>
        /// The listedConference field of GCC Conference Create Request in [T124].
        /// </summary>
        public bool listedConference;

        /// <summary>
        /// The conductibleConference field of GCC Conference Create Request in [T124].
        /// </summary>
        public bool conductibleConference;

        /// <summary>
        /// The terminationMethod field of GCC Conference Create Request in [T124].
        /// </summary>
        public int terminationMethod;

        /// <summary>
        /// The conductorPrivileges field of GCC Conference Create Request in [T124].
        /// </summary>
        public int[] conductorPrivileges;

        /// <summary>
        /// The conductedPrivileges field of GCC Conference Create Request in [T124].
        /// </summary>
        public int[] conductedPrivileges;

        /// <summary>
        /// The nonConductedPrivileges field of GCC Conference Create Request in [T124].
        /// </summary>
        public int[] nonConductedPrivileges;

        /// <summary>
        /// The conferenceDescription field of GCC Conference Create Request in [T124].
        /// </summary>
        public string conferenceDescription;

        /// <summary>
        /// The callerIdentifier field of GCC Conference Create Request in [T124].
        /// </summary>
        public string callerIdentifier;

        /// <summary>
        /// The H.221 non-standard key which MUST be embedded at the start of the userData field
        /// of GCC Conference Create Request in [T124].
        /// </summary>
        public string h221Key;

        /// <summary>
        /// Client Core Data.
        /// </summary>
        public TS_UD_CS_CORE clientCoreData;

        /// <summary>
        /// Client Security Data.
        /// </summary>
        public TS_UD_CS_SEC clientSecurityData;

        /// <summary>
        /// Optional and variable-length Client Network Data.
        /// </summary>
        public TS_UD_CS_NET clientNetworkData;

        /// <summary>
        /// Optional Client Cluster Data.
        /// </summary>
        public TS_UD_CS_CLUSTER clientClusterData;

        /// <summary>
        /// Optional Client Monitor Data.
        /// </summary>
        public TS_UD_CS_MONITOR clientMonitorData;

        /// <summary>
        /// Optional Client Message Channel Data 
        /// </summary>
        public TS_UD_CS_MCS_MSGCHANNEL clientMessageChannelData;

        /// <summary>
        /// Optional Client Multitransport Channel Data
        /// </summary>
        public TS_UD_CS_MULTITRANSPORT clientMultitransportChannelData;

        /// <summary>
        /// Client Monitor Extended Data structure
        /// </summary>
        public TS_UD_CS_MONITOR_EX clientMonitorExtendedData;

        public ConnectGCC()
        {
            clientCoreData = new TS_UD_CS_CORE();
            clientSecurityData = new TS_UD_CS_SEC();
        }
    }

    /// <summary>
    /// The main fields of MCS Connect Initial PDU.
    /// </summary>
    public class MCSConnectInitial
    {
        /// <summary>
        /// The callingDomainSelector field of MCS Connect Initial PDU in [T125].
        /// </summary>
        public byte[] callingDomainSelector;

        /// <summary>
        /// The calledDomainSelector field of MCS Connect Initial PDU in [T125].
        /// </summary>
        public byte[] calledDomainSelector;

        /// <summary>
        /// The upwardFlag field of MCS Connect Initial PDU in [T125].
        /// </summary>
        public bool upwardFlag;

        /// <summary>
        /// The targetParameters field of MCS Connect Initial PDU in [T125].
        /// </summary>
        public DomainParameter targetParameters;

        /// <summary>
        /// The minimumParameters field of MCS Connect Initial PDU in [T125].
        /// </summary>
        public DomainParameter minimumParameters;

        /// <summary>
        /// The maximumParameters field of MCS Connect Initial PDU in [T125].
        /// </summary>
        public DomainParameter maximumParameters;

        /// <summary>
        /// The userData field of MCS Connect Initial PDU in [T125].
        /// </summary>
        public ConnectGCC gccPdu;

        public MCSConnectInitial()
        {
            gccPdu = new ConnectGCC();
        }
    }

    /// <summary>
    ///  The MCS Connect Initial PDU is a Standard RDP Connection
    ///  Sequence PDU sent from client to server during the
    ///  Basic Settings Exchange phase (see Section ). It is
    ///  sent after receiving the X.224 Connection Confirm PDU.
    ///  The MCS Connect Initial PDU encapsulates a GCC Conference
    ///  Create Request, which encapsulates concatenated blocks
    ///  of settings data. A basic high-level overview of the
    ///  nested structure for the Client MCS Connect Initial
    ///  PDU is illustrated in Figure 2. Note that the order
    ///  of the settings data blocks is allowed to vary from
    ///  that shown in Figure 2 and the message syntax layout
    ///  which follows. This is possible because each data block
    ///  is identified by a User Data Header structure (see
    ///  section ).
    /// </summary>
    //  <remarks>
    //   file:///C:/Documents and Settings/test/Desktop/PAC/latest_XMLS_16may/RDPBCGR/_rfc_ms-rdpbcgr2_1_1_3.xml
    //  </remarks>
    public partial class Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request : RdpbcgrClientPdu
    {
        /// <summary>
        ///  A TPKT Header, as specified in [T123] section 8.
        /// </summary>
        public TpktHeader tpktHeader;

        /// <summary>
        ///  An X.224 Class 0 Data TPDU, as specified in [X224] section
        ///  13.7.
        /// </summary>
        public X224 x224Data;

        /// <summary>
        ///  Variable-length BER-encoded MCS Connect Initial PDU
        ///  (using definite-length encoding) as described in [T125]
        ///  (the ASN.1 structure definition is detailed in  [T125]
        ///  section 7, part 2). The userData field of the MCS Connect
        ///  Initial PDU contains the GCC Conference Create Request
        ///  data. The maximum allowed size of this user data is
        ///  1024 bytes, which implies that the combined size of
        ///  the gccCCrq, clientCoreData, clientSecurity, clientNetworkData
        ///  and clientClusterData fields must be less than 1024
        ///  bytes.
        /// </summary>
        public MCSConnectInitial mcsCi;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request()
        {
            mcsCi = new MCSConnectInitial();
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(totalBuffer, tpktHeader);
            RdpbcgrEncoder.EncodeStructure(totalBuffer, x224Data);

            if (mcsCi != null)
            {
                byte[] gccData = EncodeGccData(mcsCi.gccPdu);

                #region Filling MCS Connect Initial PDU
                Connect_Initial connectInitial = new Connect_Initial();
                connectInitial.calledDomainSelector = new Asn1OctetString(mcsCi.calledDomainSelector);
                connectInitial.callingDomainSelector = new Asn1OctetString(mcsCi.callingDomainSelector);
                connectInitial.upwardFlag = new Asn1Boolean(mcsCi.upwardFlag);

                connectInitial.targetParameters = new DomainParameters(new Asn1Integer(mcsCi.targetParameters.maxChannelIds),
                                                                       new Asn1Integer(mcsCi.targetParameters.maxUserIds),
                                                                       new Asn1Integer(mcsCi.targetParameters.maxTokenIds),
                                                                       new Asn1Integer(mcsCi.targetParameters.numPriorities),
                                                                       new Asn1Integer(mcsCi.targetParameters.minThroughput),
                                                                       new Asn1Integer(mcsCi.targetParameters.maxHeight),
                                                                       new Asn1Integer(mcsCi.targetParameters.maxMcsPduSize),
                                                                       new Asn1Integer(mcsCi.targetParameters.protocolVersion));

                connectInitial.minimumParameters = new DomainParameters(new Asn1Integer(mcsCi.minimumParameters.maxChannelIds),
                                                                        new Asn1Integer(mcsCi.minimumParameters.maxUserIds),
                                                                        new Asn1Integer(mcsCi.minimumParameters.maxTokenIds),
                                                                        new Asn1Integer(mcsCi.minimumParameters.numPriorities),
                                                                        new Asn1Integer(mcsCi.minimumParameters.minThroughput),
                                                                        new Asn1Integer(mcsCi.minimumParameters.maxHeight),
                                                                        new Asn1Integer(mcsCi.minimumParameters.maxMcsPduSize),
                                                                        new Asn1Integer(mcsCi.minimumParameters.protocolVersion));

                connectInitial.maximumParameters = new DomainParameters(new Asn1Integer(mcsCi.maximumParameters.maxChannelIds),
                                                                        new Asn1Integer(mcsCi.maximumParameters.maxUserIds),
                                                                        new Asn1Integer(mcsCi.maximumParameters.maxTokenIds),
                                                                        new Asn1Integer(mcsCi.maximumParameters.numPriorities),
                                                                        new Asn1Integer(mcsCi.maximumParameters.minThroughput),
                                                                        new Asn1Integer(mcsCi.maximumParameters.maxHeight),
                                                                        new Asn1Integer(mcsCi.maximumParameters.maxMcsPduSize),
                                                                        new Asn1Integer(mcsCi.maximumParameters.protocolVersion));

                connectInitial.userData = new Asn1OctetString(gccData);
                #endregion Filling MCS Connect Initial PDU

                #region Encode MCS Connect Initial PDU
                Asn1BerEncodingBuffer berEncodeBuffer = new Asn1BerEncodingBuffer();
                connectInitial.BerEncode(berEncodeBuffer);
                #endregion MCS Connect Initial PDU

                if (berEncodeBuffer.Data != null)
                {
                    RdpbcgrEncoder.EncodeBytes(totalBuffer, berEncodeBuffer.Data);
                }
            }

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request cloneMCSInitialPdu =
               new Client_MCS_Connect_Initial_Pdu_with_GCC_Conference_Create_Request(context);

            cloneMCSInitialPdu.tpktHeader = tpktHeader;
            cloneMCSInitialPdu.x224Data = x224Data;
            if (mcsCi != null)
            {
                cloneMCSInitialPdu.mcsCi = new MCSConnectInitial();
                cloneMCSInitialPdu.mcsCi.calledDomainSelector = RdpbcgrUtility.CloneByteArray(mcsCi.calledDomainSelector);
                cloneMCSInitialPdu.mcsCi.callingDomainSelector = RdpbcgrUtility.CloneByteArray(mcsCi.callingDomainSelector);
                cloneMCSInitialPdu.mcsCi.maximumParameters = mcsCi.maximumParameters;
                cloneMCSInitialPdu.mcsCi.minimumParameters = mcsCi.minimumParameters;
                cloneMCSInitialPdu.mcsCi.targetParameters = mcsCi.targetParameters;
                cloneMCSInitialPdu.mcsCi.upwardFlag = mcsCi.upwardFlag;

                if (mcsCi.gccPdu != null)
                {
                    #region clone mcsCi.gccPdu filed
                    cloneMCSInitialPdu.mcsCi.gccPdu = new ConnectGCC();
                    cloneMCSInitialPdu.mcsCi.gccPdu.callerIdentifier = mcsCi.gccPdu.callerIdentifier;
                    cloneMCSInitialPdu.mcsCi.gccPdu.conductedPrivileges =
                        (mcsCi.gccPdu.conductedPrivileges == null) ?
                        null : (int[])mcsCi.gccPdu.conductedPrivileges.Clone();
                    cloneMCSInitialPdu.mcsCi.gccPdu.conductibleConference = mcsCi.gccPdu.conductibleConference;
                    cloneMCSInitialPdu.mcsCi.gccPdu.conductorPrivileges =
                        (mcsCi.gccPdu.conductorPrivileges == null) ?
                        null : (int[])mcsCi.gccPdu.conductorPrivileges.Clone();
                    cloneMCSInitialPdu.mcsCi.gccPdu.conferenceDescription = mcsCi.gccPdu.conferenceDescription;
                    cloneMCSInitialPdu.mcsCi.gccPdu.conferenceName = mcsCi.gccPdu.conferenceName;
                    cloneMCSInitialPdu.mcsCi.gccPdu.convenerPassword = mcsCi.gccPdu.convenerPassword;
                    cloneMCSInitialPdu.mcsCi.gccPdu.h221Key = mcsCi.gccPdu.h221Key;
                    cloneMCSInitialPdu.mcsCi.gccPdu.listedConference = mcsCi.gccPdu.listedConference;
                    cloneMCSInitialPdu.mcsCi.gccPdu.lockedConference = mcsCi.gccPdu.lockedConference;
                    cloneMCSInitialPdu.mcsCi.gccPdu.nonConductedPrivileges =
                        (mcsCi.gccPdu.nonConductedPrivileges == null) ?
                        null : (int[])mcsCi.gccPdu.nonConductedPrivileges.Clone();
                    cloneMCSInitialPdu.mcsCi.gccPdu.password = mcsCi.gccPdu.password;
                    cloneMCSInitialPdu.mcsCi.gccPdu.terminationMethod = mcsCi.gccPdu.terminationMethod;

                    if (mcsCi.gccPdu.clientCoreData != null)
                    {
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData = new TS_UD_CS_CORE();
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.header =
                            mcsCi.gccPdu.clientCoreData.header;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.version =
                            mcsCi.gccPdu.clientCoreData.version;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.desktopHeight =
                            mcsCi.gccPdu.clientCoreData.desktopHeight;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.desktopWidth =
                            mcsCi.gccPdu.clientCoreData.desktopWidth;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.colorDepth =
                            mcsCi.gccPdu.clientCoreData.colorDepth;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.SASSequence =
                            mcsCi.gccPdu.clientCoreData.SASSequence;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.keyboardLayout =
                            mcsCi.gccPdu.clientCoreData.keyboardLayout;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.clientBuild =
                            mcsCi.gccPdu.clientCoreData.clientBuild;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.clientDigProductId =
                            mcsCi.gccPdu.clientCoreData.clientDigProductId;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.clientName =
                            mcsCi.gccPdu.clientCoreData.clientName;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.clientProductId =
                            mcsCi.gccPdu.clientCoreData.clientProductId;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.connnectionType =
                            mcsCi.gccPdu.clientCoreData.connnectionType;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.earlyCapabilityFlags =
                            mcsCi.gccPdu.clientCoreData.earlyCapabilityFlags;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.highColorDepth =
                            mcsCi.gccPdu.clientCoreData.highColorDepth;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.imeFileName =
                            mcsCi.gccPdu.clientCoreData.imeFileName;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.keyboardFunctionKey =
                            mcsCi.gccPdu.clientCoreData.keyboardFunctionKey;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.keyboardSubType =
                            mcsCi.gccPdu.clientCoreData.keyboardSubType;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.keyboardType =
                            mcsCi.gccPdu.clientCoreData.keyboardType;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.pad1octets =
                            mcsCi.gccPdu.clientCoreData.pad1octets;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.postBeta2ColorDepth =
                            mcsCi.gccPdu.clientCoreData.postBeta2ColorDepth;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.serialNumber =
                            mcsCi.gccPdu.clientCoreData.serialNumber;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.serverSelectedProtocol =
                            mcsCi.gccPdu.clientCoreData.serverSelectedProtocol;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.supportedColorDepths =
                            mcsCi.gccPdu.clientCoreData.supportedColorDepths;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.desktopPhysicalWidth =
                            mcsCi.gccPdu.clientCoreData.desktopPhysicalWidth;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.desktopPhysicalHeight =
                            mcsCi.gccPdu.clientCoreData.desktopPhysicalHeight;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.desktopOrientation =
                            mcsCi.gccPdu.clientCoreData.desktopOrientation;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.desktopScaleFactor =
                            mcsCi.gccPdu.clientCoreData.desktopScaleFactor;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientCoreData.deviceScaleFactor =
                            mcsCi.gccPdu.clientCoreData.deviceScaleFactor;
                    }

                    if (mcsCi.gccPdu.clientClusterData != null)
                    {
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientClusterData = new TS_UD_CS_CLUSTER();
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientClusterData.Flags =
                            mcsCi.gccPdu.clientClusterData.Flags;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientClusterData.header =
                            mcsCi.gccPdu.clientClusterData.header;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientClusterData.RedirectedSessionID =
                            mcsCi.gccPdu.clientClusterData.RedirectedSessionID;
                    }

                    if (mcsCi.gccPdu.clientMonitorData != null)
                    {
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientMonitorData = new TS_UD_CS_MONITOR();
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientMonitorData.Flags =
                            mcsCi.gccPdu.clientMonitorData.Flags;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientMonitorData.header =
                            mcsCi.gccPdu.clientMonitorData.header;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientMonitorData.monitorCount =
                            mcsCi.gccPdu.clientMonitorData.monitorCount;
                        if (mcsCi.gccPdu.clientMonitorData.monitorDefArray != null)
                        {
                            cloneMCSInitialPdu.mcsCi.gccPdu.clientMonitorData.monitorDefArray =
                                new Collection<TS_MONITOR_DEF>();
                            for (int count = 0; count < mcsCi.gccPdu.clientMonitorData.monitorDefArray.Count; count++)
                            {
                                cloneMCSInitialPdu.mcsCi.gccPdu.clientMonitorData.monitorDefArray.Add(
                                    mcsCi.gccPdu.clientMonitorData.monitorDefArray[count]);
                            }

                        }
                    }

                    if (mcsCi.gccPdu.clientNetworkData != null)
                    {
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientNetworkData = new TS_UD_CS_NET();
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientNetworkData.header =
                            mcsCi.gccPdu.clientNetworkData.header;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientNetworkData.channelCount =
                            mcsCi.gccPdu.clientNetworkData.channelCount;
                        if (mcsCi.gccPdu.clientNetworkData.channelDefArray != null)
                        {
                            cloneMCSInitialPdu.mcsCi.gccPdu.clientNetworkData.channelDefArray =
                                new List<CHANNEL_DEF>();
                            cloneMCSInitialPdu.mcsCi.gccPdu.clientNetworkData.channelDefArray.AddRange
                                (mcsCi.gccPdu.clientNetworkData.channelDefArray);
                        }
                    }

                    if (mcsCi.gccPdu.clientSecurityData != null)
                    {
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientSecurityData = new TS_UD_CS_SEC();
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientSecurityData.encryptionMethods =
                            mcsCi.gccPdu.clientSecurityData.encryptionMethods;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientSecurityData.extEncryptionMethods =
                            mcsCi.gccPdu.clientSecurityData.extEncryptionMethods;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientSecurityData.header =
                            mcsCi.gccPdu.clientSecurityData.header;
                    }

                    if (mcsCi.gccPdu.clientMessageChannelData != null)
                    {
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientMessageChannelData = new TS_UD_CS_MCS_MSGCHANNEL();
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientMessageChannelData.header = mcsCi.gccPdu.clientMessageChannelData.header;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientMessageChannelData.flags = mcsCi.gccPdu.clientMessageChannelData.flags;
                    }

                    if (mcsCi.gccPdu.clientMultitransportChannelData != null)
                    {
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientMultitransportChannelData = new TS_UD_CS_MULTITRANSPORT();
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientMultitransportChannelData.header = mcsCi.gccPdu.clientMultitransportChannelData.header;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientMultitransportChannelData.flags = mcsCi.gccPdu.clientMultitransportChannelData.flags;
                    }

                    if (mcsCi.gccPdu.clientMonitorExtendedData != null)
                    {
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientMonitorExtendedData = new TS_UD_CS_MONITOR_EX();
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientMonitorExtendedData.header = mcsCi.gccPdu.clientMonitorExtendedData.header;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientMonitorExtendedData.flags = mcsCi.gccPdu.clientMonitorExtendedData.flags;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientMonitorExtendedData.monitorAttributeSize = mcsCi.gccPdu.clientMonitorExtendedData.monitorAttributeSize;
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientMonitorExtendedData.monitorCount = mcsCi.gccPdu.clientMonitorExtendedData.monitorCount;

                        List<TS_MONITOR_ATTRIBUTES> attributes = new List<TS_MONITOR_ATTRIBUTES>();
                        foreach (TS_MONITOR_ATTRIBUTES attribute in mcsCi.gccPdu.clientMonitorExtendedData.monitorAttributesArray)
                        {
                            attributes.Add(attribute.Clone());
                        }
                        cloneMCSInitialPdu.mcsCi.gccPdu.clientMonitorExtendedData.monitorAttributesArray = attributes.ToArray();


                    }
                    #endregion clone mcsCi.gccPdu filed
                }
            }

            return cloneMCSInitialPdu;
        }


        /// <summary>
        /// Encode GCC Conference Create Request according to [T124].
        /// </summary>
        /// <param name="gccPdu">The data to be encoded.</param>
        /// <returns>The encoded data.</returns>
        private static byte[] EncodeGccData(ConnectGCC gccPdu)
        {
            if (gccPdu == null)
            {
                return null;
            }

            #region Encode userData
            List<byte> userData = new List<byte>();

            #region Encode clientCoreData structure TS_UD_CS_CORE
            if (gccPdu.clientCoreData != null)
            {
                List<byte> coreData = new List<byte>();
                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.header);
                RdpbcgrEncoder.EncodeStructure(coreData, (uint)gccPdu.clientCoreData.version);
                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.desktopWidth);
                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.desktopHeight);
                RdpbcgrEncoder.EncodeStructure(coreData, (ushort)gccPdu.clientCoreData.colorDepth);
                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.SASSequence);
                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.keyboardLayout);
                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.clientBuild);
                RdpbcgrEncoder.EncodeUnicodeString(coreData, gccPdu.clientCoreData.clientName,
                                                   ConstValue.CLIENT_CORE_DATA_CLIENT_NAME_SIZE);
                RdpbcgrEncoder.EncodeStructure(coreData, (uint)gccPdu.clientCoreData.keyboardType);
                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.keyboardSubType);
                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.keyboardFunctionKey);
                RdpbcgrEncoder.EncodeUnicodeString(coreData, gccPdu.clientCoreData.imeFileName,
                                                   ConstValue.CLIENT_CORE_DATA_IME_FILE_NAME_SIZE);

                if (gccPdu.clientCoreData.postBeta2ColorDepth == null)
                {
                    goto labelCoreEnd;
                }

                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.postBeta2ColorDepth.actualData);

                if (gccPdu.clientCoreData.clientProductId == null)
                {
                    goto labelCoreEnd;
                }

                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.clientProductId.actualData);

                if (gccPdu.clientCoreData.serialNumber == null)
                {
                    goto labelCoreEnd;
                }

                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.serialNumber.actualData);

                if (gccPdu.clientCoreData.highColorDepth == null)
                {
                    goto labelCoreEnd;
                }

                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.highColorDepth.actualData);

                if (gccPdu.clientCoreData.supportedColorDepths == null)
                {
                    goto labelCoreEnd;
                }

                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.supportedColorDepths.actualData);

                if (gccPdu.clientCoreData.earlyCapabilityFlags == null)
                {
                    goto labelCoreEnd;
                }

                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.earlyCapabilityFlags.actualData);

                if (gccPdu.clientCoreData.clientDigProductId == null)
                {
                    goto labelCoreEnd;
                }

                RdpbcgrEncoder.EncodeUnicodeString(coreData, gccPdu.clientCoreData.clientDigProductId,
                                                   ConstValue.CLIENT_CORE_DATA_CLIENT_DIG_PRODUCT_ID_SIZE);

                if (gccPdu.clientCoreData.connnectionType == null)
                {
                    goto labelCoreEnd;
                }

                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.connnectionType.actualData);

                if (gccPdu.clientCoreData.pad1octets == null)
                {
                    goto labelCoreEnd;
                }

                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.pad1octets.actualData);

                if (gccPdu.clientCoreData.serverSelectedProtocol == null)
                {
                    goto labelCoreEnd;
                }

                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.serverSelectedProtocol.actualData);

                if (gccPdu.clientCoreData.desktopPhysicalWidth == null)
                {
                    goto labelCoreEnd;
                }

                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.desktopPhysicalWidth.actualData);

                if (gccPdu.clientCoreData.desktopPhysicalHeight == null)
                {
                    goto labelCoreEnd;
                }

                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.desktopPhysicalHeight.actualData);


                RdpbcgrEncoder.EncodeStructure(coreData, (ushort)gccPdu.clientCoreData.desktopOrientation);

                if (gccPdu.clientCoreData.desktopScaleFactor == null)
                {
                    goto labelCoreEnd;
                }

                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.desktopScaleFactor.actualData);

                if (gccPdu.clientCoreData.deviceScaleFactor == null)
                {
                    goto labelCoreEnd;
                }

                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.clientCoreData.deviceScaleFactor.actualData);

            labelCoreEnd:
                userData.AddRange(coreData.ToArray());
            }
            #endregion Encode clientCoreData structure TS_UD_CS_CORE

            #region Encode clientSecurityData TS_UD_CS_SEC
            if (gccPdu.clientSecurityData != null)
            {
                List<byte> securityData = new List<byte>();
                RdpbcgrEncoder.EncodeStructure(securityData, gccPdu.clientSecurityData.header);
                RdpbcgrEncoder.EncodeStructure(securityData, (uint)gccPdu.clientSecurityData.encryptionMethods);
                RdpbcgrEncoder.EncodeStructure(securityData, (uint)gccPdu.clientSecurityData.extEncryptionMethods);
                userData.AddRange(securityData.ToArray());
            }
            #endregion Encode clientSecurityData TS_UD_CS_SEC

            #region Encode clientNetworkData TS_UD_CS_NET
            if (gccPdu.clientNetworkData != null)
            {
                List<byte> networkData = new List<byte>();
                RdpbcgrEncoder.EncodeStructure(networkData, gccPdu.clientNetworkData.header);
                RdpbcgrEncoder.EncodeStructure(networkData, gccPdu.clientNetworkData.channelCount);
                for (int i = 0; i < gccPdu.clientNetworkData.channelCount; ++i)
                {
                    RdpbcgrEncoder.EncodeAnsiString(networkData,
                                                    gccPdu.clientNetworkData.channelDefArray[i].name,
                                                    ConstValue.CHANNEL_DEF_NAME_SIZE);
                    RdpbcgrEncoder.EncodeStructure(networkData,
                                                   (uint)gccPdu.clientNetworkData.channelDefArray[i].options);
                }

                userData.AddRange(networkData.ToArray());
            }
            #endregion Encode clientNetworkData TS_UD_CS_NET

            #region Encode clientClusterData TS_UD_CS_CLUSTER
            if (gccPdu.clientClusterData != null)
            {
                List<byte> clusterData = new List<byte>();
                RdpbcgrEncoder.EncodeStructure(clusterData, gccPdu.clientClusterData.header);
                RdpbcgrEncoder.EncodeStructure(clusterData, (uint)gccPdu.clientClusterData.Flags);
                RdpbcgrEncoder.EncodeStructure(clusterData, gccPdu.clientClusterData.RedirectedSessionID);
                userData.AddRange(clusterData.ToArray());
            }
            #endregion Encode clientClusterData TS_UD_CS_CLUSTER

            #region Encode clientMonitorData TS_UD_CS_MONITOR
            if (gccPdu.clientMonitorData != null)
            {
                List<byte> monitorData = new List<byte>();
                RdpbcgrEncoder.EncodeStructure(monitorData, gccPdu.clientMonitorData.header);
                RdpbcgrEncoder.EncodeStructure(monitorData, gccPdu.clientMonitorData.Flags);
                RdpbcgrEncoder.EncodeStructure(monitorData, gccPdu.clientMonitorData.monitorCount);

                for (int i = 0; i < gccPdu.clientMonitorData.monitorCount; ++i)
                {
                    RdpbcgrEncoder.EncodeStructure(monitorData,
                                                   gccPdu.clientMonitorData.monitorDefArray[i].left);
                    RdpbcgrEncoder.EncodeStructure(monitorData,
                                                   gccPdu.clientMonitorData.monitorDefArray[i].top);
                    RdpbcgrEncoder.EncodeStructure(monitorData,
                                                   gccPdu.clientMonitorData.monitorDefArray[i].right);
                    RdpbcgrEncoder.EncodeStructure(monitorData,
                                                   gccPdu.clientMonitorData.monitorDefArray[i].bottom);
                    RdpbcgrEncoder.EncodeStructure(monitorData,
                                                   (uint)gccPdu.clientMonitorData.monitorDefArray[i].flags);
                }

                userData.AddRange(monitorData.ToArray());
            }
            #endregion Encode clientMonitorData TS_UD_CS_MONITOR

            #region Encode clientMessageChannelData TS_UD_CS_MCS_MSGCHANNEL
            if (gccPdu.clientMessageChannelData != null)
            {
                List<byte> messageChannelData = new List<byte>();
                RdpbcgrEncoder.EncodeStructure(messageChannelData, gccPdu.clientMessageChannelData.header);
                RdpbcgrEncoder.EncodeStructure(messageChannelData, (uint)gccPdu.clientMessageChannelData.flags);
                userData.AddRange(messageChannelData.ToArray());
            }
            #endregion Encode clientMessageChannelData TS_UD_CS_MCS_MSGCHANNEL

            #region Encode clientMultitransportChannelData TS_UD_CS_MULTITRANSPORT
            if (gccPdu.clientMultitransportChannelData != null)
            {
                List<byte> multitransportChannelData = new List<byte>();
                RdpbcgrEncoder.EncodeStructure(multitransportChannelData, gccPdu.clientMultitransportChannelData.header);
                RdpbcgrEncoder.EncodeStructure(multitransportChannelData, (uint)gccPdu.clientMultitransportChannelData.flags);
                userData.AddRange(multitransportChannelData.ToArray());
            }
            #endregion Encode clientMultitransportChannelData TS_UD_CS_MONITOR_EX

            #region Encode clientMonitorExtendedData TS_UD_CS_MULTITRANSPORT

            if (gccPdu.clientMonitorExtendedData != null)
            {
                List<byte> monitorExtendData = new List<byte>();

                RdpbcgrEncoder.EncodeStructure(monitorExtendData, gccPdu.clientMonitorExtendedData.header);
                RdpbcgrEncoder.EncodeStructure(monitorExtendData, gccPdu.clientMonitorExtendedData.flags);
                RdpbcgrEncoder.EncodeStructure(monitorExtendData, gccPdu.clientMonitorExtendedData.monitorAttributeSize);
                RdpbcgrEncoder.EncodeStructure(monitorExtendData, gccPdu.clientMonitorExtendedData.monitorCount);

                foreach (var attribute in gccPdu.clientMonitorExtendedData.monitorAttributesArray)
                {
                    RdpbcgrEncoder.EncodeStructure(monitorExtendData, attribute.physicalWidth);
                    RdpbcgrEncoder.EncodeStructure(monitorExtendData, attribute.physicalHeight);
                    RdpbcgrEncoder.EncodeStructure(monitorExtendData, (uint)attribute.orientation);
                    RdpbcgrEncoder.EncodeStructure(monitorExtendData, attribute.desktopScaleFactor);
                    RdpbcgrEncoder.EncodeStructure(monitorExtendData, attribute.deviceScaleFactor);
                }

            }
            #endregion Encode clientMonitorExtendedData TS_UD_CS_MONITOR_EX

            #endregion Encode userData

            #region Filling GCC Conference Create Request
            ConferenceCreateRequest gccCCrq = new ConferenceCreateRequest();
            if (gccPdu.conferenceName != null)
            {
                gccCCrq.conferenceName = new ConferenceName(gccPdu.conferenceName);
            }

            if (gccPdu.convenerPassword != null)
            {
                gccCCrq.convenerPassword = new Password(gccPdu.convenerPassword);
            }

            if (gccPdu.password != null)
            {
                gccCCrq.password = new Password(gccPdu.password);
            }

            gccCCrq.lockedConference = new Asn1Boolean(gccPdu.lockedConference);
            gccCCrq.listedConference = new Asn1Boolean(gccPdu.listedConference);
            gccCCrq.conductibleConference = new Asn1Boolean(gccPdu.conductibleConference);
            gccCCrq.terminationMethod = new TerminationMethod(gccPdu.terminationMethod);

            if (gccPdu.conductorPrivileges != null)
            {
                List<Privilege> privilege = new List<Privilege>();
                foreach (int i in gccPdu.conductorPrivileges)
                {
                    privilege.Add(new Privilege(i));
                }

                gccCCrq.conductorPrivileges = new Asn1SetOf<Privilege>(privilege.ToArray());
            }

            if (gccPdu.conductedPrivileges != null)
            {
                List<Privilege> privilege = new List<Privilege>();
                foreach (int i in gccPdu.conductedPrivileges)
                {
                    privilege.Add(new Privilege(i));
                }

                gccCCrq.conductedPrivileges = new Asn1SetOf<Privilege>(privilege.ToArray());
            }

            if (gccPdu.nonConductedPrivileges != null)
            {
                List<Privilege> privilege = new List<Privilege>();
                foreach (int i in gccPdu.nonConductedPrivileges)
                {
                    privilege.Add(new Privilege(i));
                }

                gccCCrq.nonConductedPrivileges = new Asn1SetOf<Privilege>(privilege.ToArray());
            }

            if (gccPdu.conferenceDescription != null)
            {
                gccCCrq.conferenceDescription = new TextString(gccPdu.conferenceDescription);
            }

            if (gccPdu.callerIdentifier != null)
            {
                gccCCrq.callerIdentifier = new TextString(gccPdu.callerIdentifier);
            }

            //gccCCrq.extElem1 = new Asn1OpenExt();

            UserDataElement[] dataElements = new UserDataElement[1];
            dataElements[0] = new UserDataElement();
            dataElements[0].key = new Key(Key.h221NonStandard,
                                          new H221NonStandardIdentifier(ConstValue.H221_NON_STANDARD_KEY));
            dataElements[0].value = new Asn1OctetString(userData.ToArray());
            gccCCrq.userData = new UserData(dataElements);

            ConnectGCCPDU cGccPdu = new ConnectGCCPDU(ConnectGCCPDU.conferenceCreateRequest, gccCCrq);
            #endregion Filling GCC Conference Create Request

            #region Encode ConnectData
            Asn1PerEncodingBuffer perBuffer = new Asn1PerEncodingBuffer(true);
            cGccPdu.PerEncode(perBuffer);
            byte[] asnEncodedData = perBuffer.ByteArrayData;
            Key key = new Key(Key.obj, new Asn1ObjectIdentifier(ConstValue.MCS_ATTRIBUTE_ID));
            ConnectData connectData = new ConnectData(key, new Asn1OctetString(asnEncodedData));
            Asn1PerEncodingBuffer perBuffer2 = new Asn1PerEncodingBuffer(true);
            connectData.PerEncode(perBuffer2);
            #endregion Encode ConnectData

            return perBuffer2.ByteArrayData;
        }
    }

    /// <summary>
    /// Gcc used in MCSConnectInitial.
    /// </summary>
    public class ConnectGCCRsp
    {
        /// <summary>
        ///  The key field of MCS Connect Response PDU.
        /// </summary>
        public string H221Key;

        /// <summary>
        ///  The tag field of Conference Create Response field.
        /// </summary>
        public int tag;

        /// <summary>
        ///  The nodeID field of Conference Create Response field.
        /// </summary>
        public int nodeID;


        /// <summary>
        ///  The result field of Conference Create Response field.
        /// </summary>
        public int ccrResult;


        /// <summary>
        ///  Server Core Data structure.
        /// </summary>
        public TS_UD_SC_CORE serverCoreData;

        /// <summary>
        ///  Variable-length Server Network Data structure.
        /// </summary>
        public TS_UD_SC_NET serverNetworkData;

        /// <summary>
        ///  Variable-length Server Security Data structure.
        /// </summary>
        public TS_UD_SC_SEC1 serverSecurityData;

        /// <summary>
        /// Optional Server Message Channel Data 
        /// </summary>
        public TS_UD_SC_MCS_MSGCHANNEL serverMessageChannelData;

        /// <summary>
        /// Optional Server Multitransport Channel Data 
        /// </summary>
        public TS_UD_SC_MULTITRANSPORT serverMultitransportChannelData;

        public ConnectGCCRsp()
        {
            serverCoreData = new TS_UD_SC_CORE();
            serverNetworkData = new TS_UD_SC_NET();
            serverSecurityData = new TS_UD_SC_SEC1();
        }
    }

    /// <summary>
    /// The main fields of MCS Connect Initial PDU.
    /// </summary>
    public class MCSConnectResponse
    {
        /// <summary>
        ///  The result field of MCS Connect Response PDU.
        /// </summary>
        public int result;

        public int calledConnectId;

        public DomainParameter domainParameters;

        /// <summary>
        /// The userData field of MCS Connect Initial PDU in [T125].
        /// </summary>
        public ConnectGCCRsp gccPdu;

        public MCSConnectResponse()
        {
            gccPdu = new ConnectGCCRsp();
        }
    }

    public partial class Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response : RdpbcgrServerPdu
    {
        /// <summary>
        ///  A TPKT Header, as specified in [T123] section 8.
        /// </summary>
        public TpktHeader tpktHeader;

        /// <summary>
        ///  An X.224 Class 0 Data TPDU, as specified in [X224] section
        ///  13.7.
        /// </summary>
        public X224 x224Data;

        /// <summary>
        ///  Variable-length BER-encoded MCS Connect Initial PDU
        ///  (using definite-length encoding) as described in [T125]
        ///  (the ASN.1 structure definition is detailed in  [T125]
        ///  section 7, part 2). The userData field of the MCS Connect
        ///  Initial PDU contains the GCC Conference Create Request
        ///  data. The maximum allowed size of this user data is
        ///  1024 bytes, which implies that the combined size of
        ///  the gccCCrq, clientCoreData, clientSecurity, clientNetworkData
        ///  and clientClusterData fields must be less than 1024
        ///  bytes.
        /// </summary>
        public MCSConnectResponse mcsCrsp;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
            mcsCrsp = new MCSConnectResponse();
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response()
        {
            mcsCrsp = new MCSConnectResponse();
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(totalBuffer, tpktHeader);
            RdpbcgrEncoder.EncodeStructure(totalBuffer, x224Data);

            if (mcsCrsp != null)
            {
                byte[] gccData = EncodeGccRspData(mcsCrsp.gccPdu);

                #region Filling MCS Connect Response PDU
                Connect_Response connectResponse = new Connect_Response();
                connectResponse.result = new Result(mcsCrsp.result);
                connectResponse.calledConnectId = new Asn1Integer(mcsCrsp.calledConnectId);
                connectResponse.domainParameters = new DomainParameters(new Asn1Integer(mcsCrsp.domainParameters.maxChannelIds),
                                                                       new Asn1Integer(mcsCrsp.domainParameters.maxUserIds),
                                                                       new Asn1Integer(mcsCrsp.domainParameters.maxTokenIds),
                                                                       new Asn1Integer(mcsCrsp.domainParameters.numPriorities),
                                                                       new Asn1Integer(mcsCrsp.domainParameters.minThroughput),
                                                                       new Asn1Integer(mcsCrsp.domainParameters.maxHeight),
                                                                       new Asn1Integer(mcsCrsp.domainParameters.maxMcsPduSize),
                                                                       new Asn1Integer(mcsCrsp.domainParameters.protocolVersion));
                connectResponse.userData = new Asn1OctetString(gccData);
                #endregion Filling MCS Connect Response PDU

                #region Encode MCS Connect Initial PDU
                Asn1BerEncodingBuffer berEncodeBuffer = new Asn1BerEncodingBuffer();
                connectResponse.BerEncode(berEncodeBuffer);
                #endregion MCS Connect Initial PDU

                RdpbcgrEncoder.EncodeBytes(totalBuffer, berEncodeBuffer.Data);
            }

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes);

            return encodedBytes;
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response cloneMCSRspPdu =
                new Server_MCS_Connect_Response_Pdu_with_GCC_Conference_Create_Response(serverSessionContext);

            cloneMCSRspPdu.tpktHeader = tpktHeader;
            cloneMCSRspPdu.x224Data = x224Data;

            if (mcsCrsp != null)
            {
                cloneMCSRspPdu.mcsCrsp = new MCSConnectResponse();
                cloneMCSRspPdu.mcsCrsp.gccPdu = null;
                cloneMCSRspPdu.mcsCrsp.result = mcsCrsp.result;
                //cloneMCSRspPdu.mcsCrsp.calledConnectId = mcsCrsp.calledConnectId;
                //cloneMCSRspPdu.mcsCrsp.domainParameters = mcsCrsp.domainParameters;

                if (mcsCrsp.gccPdu != null)
                {
                    cloneMCSRspPdu.mcsCrsp.gccPdu = new ConnectGCCRsp();
                    cloneMCSRspPdu.mcsCrsp.gccPdu.serverCoreData = null;
                    cloneMCSRspPdu.mcsCrsp.gccPdu.serverNetworkData = null;
                    cloneMCSRspPdu.mcsCrsp.gccPdu.serverSecurityData = null;
                    cloneMCSRspPdu.mcsCrsp.gccPdu.nodeID = mcsCrsp.gccPdu.nodeID;
                    cloneMCSRspPdu.mcsCrsp.gccPdu.tag = mcsCrsp.gccPdu.tag;
                    cloneMCSRspPdu.mcsCrsp.gccPdu.ccrResult = mcsCrsp.gccPdu.ccrResult;
                    cloneMCSRspPdu.mcsCrsp.gccPdu.H221Key = mcsCrsp.gccPdu.H221Key;

                    if (mcsCrsp.gccPdu.serverCoreData != null)
                    {
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverCoreData = new TS_UD_SC_CORE();
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverCoreData.header =
                            mcsCrsp.gccPdu.serverCoreData.header;
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverCoreData.version =
                            mcsCrsp.gccPdu.serverCoreData.version;
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverCoreData.clientRequestedProtocols =
                            mcsCrsp.gccPdu.serverCoreData.clientRequestedProtocols;
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverCoreData.earlyCapabilityFlags =
                            mcsCrsp.gccPdu.serverCoreData.earlyCapabilityFlags;
                    }
                    if (mcsCrsp.gccPdu.serverNetworkData != null)
                    {
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverNetworkData = new TS_UD_SC_NET();
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverNetworkData.header =
                            mcsCrsp.gccPdu.serverNetworkData.header;
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverNetworkData.MCSChannelId =
                            mcsCrsp.gccPdu.serverNetworkData.MCSChannelId;
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverNetworkData.channelCount =
                            mcsCrsp.gccPdu.serverNetworkData.channelCount;
                        if (mcsCrsp.gccPdu.serverNetworkData.channelIdArray != null)
                        {
                            cloneMCSRspPdu.mcsCrsp.gccPdu.serverNetworkData.channelIdArray =
                                new ushort[mcsCrsp.gccPdu.serverNetworkData.channelIdArray.Length];
                            mcsCrsp.gccPdu.serverNetworkData.channelIdArray.CopyTo(
                                cloneMCSRspPdu.mcsCrsp.gccPdu.serverNetworkData.channelIdArray,
                                0);
                        }
                        if (mcsCrsp.gccPdu.serverNetworkData.Pad != null)
                        {
                            cloneMCSRspPdu.mcsCrsp.gccPdu.serverNetworkData.Pad =
                                RdpbcgrUtility.CloneByteArray(mcsCrsp.gccPdu.serverNetworkData.Pad);
                        }
                    }
                    if (mcsCrsp.gccPdu.serverSecurityData != null)
                    {
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverSecurityData = new TS_UD_SC_SEC1();
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverSecurityData.header =
                            mcsCrsp.gccPdu.serverSecurityData.header;
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverSecurityData.encryptionLevel =
                            mcsCrsp.gccPdu.serverSecurityData.encryptionLevel;
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverSecurityData.encryptionMethod =
                            mcsCrsp.gccPdu.serverSecurityData.encryptionMethod;
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverSecurityData.serverRandomLen =
                            mcsCrsp.gccPdu.serverSecurityData.serverRandomLen;
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverSecurityData.serverCertLen =
                            mcsCrsp.gccPdu.serverSecurityData.serverCertLen;
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverSecurityData.serverRandom =
                            RdpbcgrUtility.CloneByteArray(mcsCrsp.gccPdu.serverSecurityData.serverRandom);
                        //////not implemented yet!

                        if (mcsCrsp.gccPdu.serverSecurityData.serverCertificate != null)
                        {
                            cloneMCSRspPdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate = new SERVER_CERTIFICATE();
                            cloneMCSRspPdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate.dwVersion =
                                mcsCrsp.gccPdu.serverSecurityData.serverCertificate.dwVersion;

                            if (mcsCrsp.gccPdu.serverSecurityData.serverCertificate.dwVersion ==
                                SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_1)
                            {
                                PROPRIETARYSERVERCERTIFICATE cert = (PROPRIETARYSERVERCERTIFICATE)
                                    mcsCrsp.gccPdu.serverSecurityData.serverCertificate.certData;
                                cert.SignatureBlob = RdpbcgrUtility.CloneByteArray(cert.SignatureBlob);
                                cert.PublicKeyBlob.modulus = RdpbcgrUtility.CloneByteArray(cert.PublicKeyBlob.modulus);
                                cloneMCSRspPdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate.certData = cert;
                            }
                            else
                            {
                                X509_CERTIFICATE_CHAIN x509cert = (X509_CERTIFICATE_CHAIN)mcsCrsp.gccPdu.serverSecurityData.serverCertificate.certData;
                                for (int i = 0; i < x509cert.CertBlobArray.Length; i++)
                                {
                                    x509cert.CertBlobArray[i].abCert = RdpbcgrUtility.CloneByteArray(x509cert.CertBlobArray[i].abCert);
                                }
                                x509cert.Padding = RdpbcgrUtility.CloneByteArray(x509cert.Padding);
                                cloneMCSRspPdu.mcsCrsp.gccPdu.serverSecurityData.serverCertificate.certData = x509cert;
                            }
                        }
                    }
                    if (mcsCrsp.gccPdu.serverMessageChannelData != null)
                    {
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverMessageChannelData = new TS_UD_SC_MCS_MSGCHANNEL();
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverMessageChannelData.header = mcsCrsp.gccPdu.serverMessageChannelData.header;
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverMessageChannelData.MCSChannelID = mcsCrsp.gccPdu.serverMessageChannelData.MCSChannelID;
                    }
                    if (mcsCrsp.gccPdu.serverMultitransportChannelData != null)
                    {
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverMultitransportChannelData = new TS_UD_SC_MULTITRANSPORT();
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverMultitransportChannelData.header = mcsCrsp.gccPdu.serverMultitransportChannelData.header;
                        cloneMCSRspPdu.mcsCrsp.gccPdu.serverMultitransportChannelData.flags = mcsCrsp.gccPdu.serverMultitransportChannelData.flags;
                    }
                }
            }
            return cloneMCSRspPdu;
        }

        /// <summary>
        /// Encode GCC Conference Create Request according to [T124].
        /// </summary>
        /// <param name="gccPdu">The data to be encoded.</param>
        /// <returns>The encoded data.</returns>
        private static byte[] EncodeGccRspData(ConnectGCCRsp gccPdu)
        {
            if (gccPdu == null)
            {
                return null;
            }

            #region Encode userData
            List<byte> userData = new List<byte>();

            #region Encode serverCoreData structure TS_UD_CS_CORE
            if (gccPdu.serverCoreData != null)
            {
                List<byte> coreData = new List<byte>();
                RdpbcgrEncoder.EncodeStructure(coreData, gccPdu.serverCoreData.header);
                RdpbcgrEncoder.EncodeStructure(coreData, (uint)gccPdu.serverCoreData.version);
                RdpbcgrEncoder.EncodeStructure(coreData, (uint)gccPdu.serverCoreData.clientRequestedProtocols);
                //earlyCapabilityFlags is optional.
                if (gccPdu.serverCoreData.earlyCapabilityFlags != 0)
                    RdpbcgrEncoder.EncodeStructure(coreData, (uint)gccPdu.serverCoreData.earlyCapabilityFlags);

                userData.AddRange(coreData.ToArray());
            }
            #endregion Encode serverCoreData structure TS_UD_CS_CORE

            #region Encode serverNetworkData TS_UD_CS_NET
            if (gccPdu.serverNetworkData != null)
            {
                List<byte> networkData = new List<byte>();
                RdpbcgrEncoder.EncodeStructure(networkData, gccPdu.serverNetworkData.header);
                RdpbcgrEncoder.EncodeStructure(networkData, gccPdu.serverNetworkData.MCSChannelId);
                RdpbcgrEncoder.EncodeStructure(networkData, gccPdu.serverNetworkData.channelCount);
                for (int i = 0; i < gccPdu.serverNetworkData.channelCount; ++i)
                {
                    RdpbcgrEncoder.EncodeStructure(networkData, gccPdu.serverNetworkData.channelIdArray[i]);
                }
                if (gccPdu.serverNetworkData.Pad != null)
                {
                    RdpbcgrEncoder.EncodeBytes(networkData, gccPdu.serverNetworkData.Pad);
                }
                userData.AddRange(networkData.ToArray());
            }
            #endregion Encode serverNetworkData TS_UD_CS_NET

            #region Encode serverSecurityData TS_UD_CS_SEC
            if (gccPdu.serverSecurityData != null)
            {
                List<byte> securityData = new List<byte>();
                RdpbcgrEncoder.EncodeStructure(securityData, gccPdu.serverSecurityData.header);
                RdpbcgrEncoder.EncodeStructure(securityData, (uint)gccPdu.serverSecurityData.encryptionMethod);
                RdpbcgrEncoder.EncodeStructure(securityData, (uint)gccPdu.serverSecurityData.encryptionLevel);


                if (gccPdu.serverSecurityData.encryptionMethod != EncryptionMethods.ENCRYPTION_METHOD_NONE ||
                    gccPdu.serverSecurityData.encryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE)
                {
                    RdpbcgrEncoder.EncodeStructure(securityData, gccPdu.serverSecurityData.serverRandomLen.actualData);
                    RdpbcgrEncoder.EncodeStructure(securityData, gccPdu.serverSecurityData.serverCertLen.actualData);
                    if (gccPdu.serverSecurityData.serverRandom != null)
                    {
                        RdpbcgrEncoder.EncodeBytes(securityData, gccPdu.serverSecurityData.serverRandom);
                    }

                    #region Encode certificate
                    if (gccPdu.serverSecurityData.serverCertificate != null)
                    {
                        RdpbcgrEncoder.EncodeStructure(securityData, (uint)gccPdu.serverSecurityData.serverCertificate.dwVersion);
                        if (gccPdu.serverSecurityData.serverCertificate.dwVersion == SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_1)
                        {
                            PROPRIETARYSERVERCERTIFICATE cert = (PROPRIETARYSERVERCERTIFICATE)gccPdu.serverSecurityData.serverCertificate.certData;
                            RdpbcgrEncoder.EncodeStructure(securityData, (uint)cert.dwSigAlgId);
                            RdpbcgrEncoder.EncodeStructure(securityData, (uint)cert.dwKeyAlgId);
                            RdpbcgrEncoder.EncodeStructure(securityData, (ushort)cert.wPublicKeyBlobType);
                            RdpbcgrEncoder.EncodeStructure(securityData, cert.wPublicKeyBlobLen);
                            RdpbcgrEncoder.EncodeStructure(securityData, (uint)cert.PublicKeyBlob.magic);
                            RdpbcgrEncoder.EncodeStructure(securityData, cert.PublicKeyBlob.keylen);
                            RdpbcgrEncoder.EncodeStructure(securityData, cert.PublicKeyBlob.bitlen);
                            RdpbcgrEncoder.EncodeStructure(securityData, cert.PublicKeyBlob.datalen);
                            RdpbcgrEncoder.EncodeStructure(securityData, cert.PublicKeyBlob.pubExp);
                            RdpbcgrEncoder.EncodeBytes(securityData, cert.PublicKeyBlob.modulus);
                            RdpbcgrEncoder.EncodeStructure(securityData, (ushort)cert.wSignatureBlobType);
                            RdpbcgrEncoder.EncodeStructure(securityData, cert.wSignatureBlobLen);
                            RdpbcgrEncoder.EncodeBytes(securityData, cert.SignatureBlob);
                        }
                        else if (gccPdu.serverSecurityData.serverCertificate.dwVersion == SERVER_CERTIFICATE_dwVersion_Values.CERT_CHAIN_VERSION_2)
                        {
                            X509_CERTIFICATE_CHAIN cert = (X509_CERTIFICATE_CHAIN)gccPdu.serverSecurityData.serverCertificate.certData;
                            RdpbcgrEncoder.EncodeStructure(securityData, (uint)cert.NumCertBlobs);
                            for (int i = 0; i < cert.CertBlobArray.Length; i++)
                            {
                                RdpbcgrEncoder.EncodeStructure(securityData, cert.CertBlobArray[i].cbCert);
                                RdpbcgrEncoder.EncodeBytes(securityData, cert.CertBlobArray[i].abCert);
                            }
                            RdpbcgrEncoder.EncodeBytes(securityData, cert.Padding);
                        }
                    }
                    #endregion
                }
                userData.AddRange(securityData.ToArray());
            }
            #endregion Encode serverSecurityData TS_UD_CS_SEC

            #region Encode serverMessageChannelData TS_UD_SC_MCS_MSGCHANNEL
            if (gccPdu.serverMessageChannelData != null)
            {
                List<byte> msgChannelData = new List<byte>();
                RdpbcgrEncoder.EncodeStructure(msgChannelData, gccPdu.serverMessageChannelData.header);
                RdpbcgrEncoder.EncodeStructure(msgChannelData, gccPdu.serverMessageChannelData.MCSChannelID);
                userData.AddRange(msgChannelData.ToArray());
            }
            #endregion

            #region Encode serverMultitransportChannelData TS_UD_SC_MULTITRANSPORT
            if (gccPdu.serverMultitransportChannelData != null)
            {
                List<byte> multiTranChannelData = new List<byte>();
                RdpbcgrEncoder.EncodeStructure(multiTranChannelData, gccPdu.serverMultitransportChannelData.header);
                RdpbcgrEncoder.EncodeStructure(multiTranChannelData, (uint)gccPdu.serverMultitransportChannelData.flags);
                userData.AddRange(multiTranChannelData.ToArray());
            }
            #endregion

            #endregion Encode userData

            #region Filling GCC Conference Create Request
            ConferenceCreateResponse gccCCrsp = new ConferenceCreateResponse();
            gccCCrsp.nodeID = new UserID(gccPdu.nodeID);
            gccCCrsp.tag = new Asn1Integer(gccPdu.tag);
            gccCCrsp.result = new ConferenceCreateResponse_result(gccPdu.ccrResult);
            //gccCCrsp.extElem1 = new Asn1OpenExt();
            UserDataElement[] dataElements = new UserDataElement[1];
            dataElements[0] = new UserDataElement();
            dataElements[0].key = new Key(Key.h221NonStandard,
                                          new H221NonStandardIdentifier(gccPdu.H221Key));
            dataElements[0].value = new Asn1OctetString(userData.ToArray());
            gccCCrsp.userData = new UserData(dataElements);
            ConnectGCCPDU cGccPdu = new ConnectGCCPDU(ConnectGCCPDU.conferenceCreateResponse, gccCCrsp);
            #endregion Filling GCC Conference Create Request

            #region Encode ConnectData
            Asn1PerEncodingBuffer perBuffer = new Asn1PerEncodingBuffer(true);
            cGccPdu.PerEncode(perBuffer);
            byte[] asnEncodedData = perBuffer.ByteArrayData;
            Key key = new Key(Key.obj, new Asn1ObjectIdentifier(ConstValue.MCS_ATTRIBUTE_ID));
            ConnectData connectData = new ConnectData(key, new Asn1OctetString(asnEncodedData));
            Asn1PerEncodingBuffer perBuffer2 = new Asn1PerEncodingBuffer(true);
            connectData.PerEncode(perBuffer2);
            #endregion Encode ConnectData

            #region Workaround for Windows encoding issue

            //reset the ConnectPDULength to one-byte length: 0x2A
            byte[] returnButter = null;
            if ((perBuffer2.ByteArrayData[7] & 0x80) == 0x80)
            {
                //If length is two-byte, delete one byte
                returnButter = new byte[perBuffer2.ByteArrayData.Length - 1];
                Array.Copy(perBuffer2.ByteArrayData, returnButter, 7);
                Array.Copy(perBuffer2.ByteArrayData, 8, returnButter, 7, returnButter.Length - 7);
            }
            else
            {
                returnButter = new byte[perBuffer2.ByteArrayData.Length];
                Array.Copy(perBuffer2.ByteArrayData, returnButter, perBuffer2.ByteArrayData.Length);
            }
            returnButter[7] = 0x2A;

            #endregion

            return returnButter;
        }
    }

    /// <summary>
    ///  The MCS Erect Domain Request PDU is an RDP Connection Sequence PDU 
    ///  sent from client to server during the Channel Connection phase 
    ///  (see section 1.3.1.1). It is sent after receiving the MCS Connect Response PDU
    ///  (section 2.2.1.4).
    /// </summary>
    public partial class Client_MCS_Erect_Domain_Request : RdpbcgrClientPdu
    {

        /// <summary>
        ///  A TPKT Header, as specified in [T123] section 8.
        /// </summary>
        public TpktHeader tpktHeader;

        /// <summary>
        ///  An X.224 Class 0 Data TPDU, as specified in [X224] section
        ///  13.7.
        /// </summary>
        public X224 x224Data;

        /// <summary>
        ///  The subHeight field of MCS Erect Domain Request PDU.
        /// </summary>
        public int subHeight;

        /// <summary>
        ///  The subInterval field of MCS Erect Domain Request PDU.
        /// </summary>
        public int subInterval;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Client_MCS_Erect_Domain_Request(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Client_MCS_Erect_Domain_Request()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(totalBuffer, tpktHeader);
            RdpbcgrEncoder.EncodeStructure(totalBuffer, x224Data);

            ErectDomainRequest erectDomain = new ErectDomainRequest(new Asn1Integer(subHeight), new Asn1Integer(subInterval));
            DomainMCSPDU mcsDomain = new DomainMCSPDU(DomainMCSPDU.erectDomainRequest, erectDomain);
            RdpbcgrEncoder.EncodeDomainMcsPdu(totalBuffer, mcsDomain);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }


        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Client_MCS_Erect_Domain_Request cloneMcsErectPdu = new Client_MCS_Erect_Domain_Request(context);

            cloneMcsErectPdu.subHeight = subHeight;
            cloneMcsErectPdu.subInterval = subInterval;
            cloneMcsErectPdu.tpktHeader = tpktHeader;
            cloneMcsErectPdu.x224Data = x224Data;

            return cloneMcsErectPdu;
        }
    }

    /// <summary>
    ///  The MCS Attach User Request PDU is an RDP Connection Sequence
    ///  PDU sent from client to server during the Channel Connection
    ///  phase (see section 1.3.1.1) to request a user channel ID. It is
    ///  sent after transmitting the MCS Erect Domain Request PDU (section 2.2.1.5).
    /// </summary>
    public partial class Client_MCS_Attach_User_Request : RdpbcgrClientPdu
    {

        /// <summary>
        ///  A TPKT Header, as specified in [T123] section 8.
        /// </summary>
        public TpktHeader tpktHeader;

        /// <summary>
        ///  An X.224 Class 0 Data TPDU, as specified in [X224] section
        ///  13.7.
        /// </summary>
        public X224 x224Data;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Client_MCS_Attach_User_Request(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Client_MCS_Attach_User_Request()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(totalBuffer, tpktHeader);
            RdpbcgrEncoder.EncodeStructure(totalBuffer, x224Data);

            AttachUserRequest attachUserRequest = new AttachUserRequest();
            DomainMCSPDU mcsDomain = new DomainMCSPDU(DomainMCSPDU.attachUserRequest, attachUserRequest);
            RdpbcgrEncoder.EncodeDomainMcsPdu(totalBuffer, mcsDomain);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Client_MCS_Attach_User_Request cloneMcsAttachPdu = new Client_MCS_Attach_User_Request(context);

            cloneMcsAttachPdu.tpktHeader = tpktHeader;
            cloneMcsAttachPdu.x224Data = x224Data;

            return cloneMcsAttachPdu;
        }
    }

    /// <summary>
    ///  The MCS Attach User Confirm PDU is an RDP Connection Sequence 
    ///  PDU sent from server to client during the Channel Connection 
    ///  phase (see section 1.3.1.1). It is sent as a response to the MCS
    ///  Attach User Request PDU (section 2.2.1.6).
    /// </summary>
    public partial class Server_MCS_Attach_User_Confirm_Pdu : RdpbcgrServerPdu
    {

        /// <summary>
        ///  A TPKT Header, as specified in [T123] section 8.
        /// </summary>
        public TpktHeader tpktHeader;

        /// <summary>
        ///  An X.224 Class 0 Data TPDU, as specified in [X224] section
        ///  13.7.
        /// </summary>
        public X224 x224Data;

        /// <summary>
        ///  T125 PDU.
        /// </summary>
        public AttachUserConfirm attachUserConfirm;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public Server_MCS_Attach_User_Confirm_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_MCS_Attach_User_Confirm_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(totalBuffer, tpktHeader);
            RdpbcgrEncoder.EncodeStructure(totalBuffer, x224Data);

            DomainMCSPDU mcsDomain = new DomainMCSPDU(DomainMCSPDU.attachUserConfirm, attachUserConfirm);
            RdpbcgrEncoder.EncodeDomainMcsPdu(totalBuffer, mcsDomain);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes);

            return encodedBytes;
        }

        public override StackPacket Clone()
        {
            Server_MCS_Attach_User_Confirm_Pdu cloneMcsAttachPdu = new Server_MCS_Attach_User_Confirm_Pdu(serverSessionContext);

            cloneMcsAttachPdu.tpktHeader = tpktHeader;
            cloneMcsAttachPdu.x224Data = x224Data;
            cloneMcsAttachPdu.attachUserConfirm = new AttachUserConfirm(
                new Result(attachUserConfirm.result.Value),
                new UserId(attachUserConfirm.initiator.Value));

            return cloneMcsAttachPdu;
        }
    }

    /// <summary>
    ///  The MCS Channel Join Request PDU is an RDP Connection Sequence 
    ///  PDU sent from client to server during the Channel Connection
    ///  phase (see section 1.3.1.1). It is sent after receiving the MCS
    ///  Attach User Confirm PDU (section 2.2.1.7). The client uses the MCS
    ///  Channel Join Request PDU to join the user channel obtained from the 
    ///  Attach User Confirm PDU, the I/O channel and all of the static virtual 
    ///  channels obtained from the Server Network Data structure (section 2.2.1.4.4).
    /// </summary>
    public partial class Client_MCS_Channel_Join_Request : RdpbcgrClientPdu
    {

        /// <summary>
        ///  A TPKT Header, as specified in [T123] section 8.
        /// </summary>
        public TpktHeader tpktHeader;

        /// <summary>
        ///  An X.224 Class 0 Data TPDU, as specified in [X224] section
        ///  13.7.
        /// </summary>
        public X224 x224Data;

        /// <summary>
        ///  The user channel id.
        /// </summary>
        public long userChannelId;

        /// <summary>
        ///  The virtual channel id want to join.
        /// </summary>
        public long mcsChannelId;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Client_MCS_Channel_Join_Request(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Client_MCS_Channel_Join_Request()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(totalBuffer, tpktHeader);
            RdpbcgrEncoder.EncodeStructure(totalBuffer, x224Data);

            ChannelJoinRequest joinRequest = new ChannelJoinRequest(new UserId(userChannelId), new ChannelId(mcsChannelId));
            DomainMCSPDU mcsDomain = new DomainMCSPDU(DomainMCSPDU.channelJoinRequest, joinRequest);
            RdpbcgrEncoder.EncodeDomainMcsPdu(totalBuffer, mcsDomain);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }


        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Client_MCS_Channel_Join_Request cloneMcsChanneelJoinPdu = new Client_MCS_Channel_Join_Request(context);

            cloneMcsChanneelJoinPdu.mcsChannelId = mcsChannelId;
            cloneMcsChanneelJoinPdu.userChannelId = userChannelId;
            cloneMcsChanneelJoinPdu.tpktHeader = tpktHeader;
            cloneMcsChanneelJoinPdu.x224Data = x224Data;

            return cloneMcsChanneelJoinPdu;
        }
    }

    /// <summary>
    ///  The MCS Channel Join Confirm PDU is an RDP Connection 
    ///  Sequence PDU sent from server to client during the Channel
    ///  Connection phase (see section 1.3.1.1). It is sent as a response 
    ///  to the MCS Channel Join Request PDU (section 2.2.1.8).
    /// </summary>
    public partial class Server_MCS_Channel_Join_Confirm_Pdu : RdpbcgrServerPdu
    {

        /// <summary>
        ///  A TPKT Header, as specified in [T123] section 8.
        /// </summary>
        public TpktHeader tpktHeader;

        /// <summary>
        ///  An X.224 Class 0 Data TPDU, as specified in [X224] section
        ///  13.7.
        /// </summary>
        public X224 x224Data;

        /// <summary>
        ///  T125 PDU.
        /// </summary>
        public ChannelJoinConfirm channelJoinConfirm;

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public Server_MCS_Channel_Join_Confirm_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_MCS_Channel_Join_Confirm_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(totalBuffer, tpktHeader);
            RdpbcgrEncoder.EncodeStructure(totalBuffer, x224Data);

            DomainMCSPDU mcsDomain = new DomainMCSPDU(DomainMCSPDU.channelJoinConfirm, channelJoinConfirm);
            RdpbcgrEncoder.EncodeDomainMcsPdu(totalBuffer, mcsDomain);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes);

            return encodedBytes;
        }

        public override StackPacket Clone()
        {
            Server_MCS_Channel_Join_Confirm_Pdu cloneChannelJoinConfirmPdu = new Server_MCS_Channel_Join_Confirm_Pdu(serverSessionContext);
            cloneChannelJoinConfirmPdu.tpktHeader = tpktHeader;
            cloneChannelJoinConfirmPdu.x224Data = x224Data;
            cloneChannelJoinConfirmPdu.channelJoinConfirm = new ChannelJoinConfirm(
                new Result(channelJoinConfirm.result.Value),
                new UserId(channelJoinConfirm.initiator.Value),
                new ChannelId((long)channelJoinConfirm.requested.Value),
                new ChannelId(channelJoinConfirm.channelId.Value));

            return cloneChannelJoinConfirmPdu;
        }
    }

    /// <summary>
    ///  The Security Exchange PDU is a Standard RDP Connection
    ///  Sequence PDU sent from client to server during the
    ///  RDP Security Commencement phase (see section ). It
    ///  MAY be sent after receiving all requested MCS Channel
    ///  Join Confirm PDUs. 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/_rfc_ms-rdpbcgr2_1_1_10.xml
    //  </remarks>
    public partial class Client_Security_Exchange_Pdu : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Security Exchange PDU, as
        ///  specified in section .
        /// </summary>
        public TS_SECURITY_PACKET securityExchangePduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Client_Security_Exchange_Pdu(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Client_Security_Exchange_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> randomBuffer = new List<byte>();
            if (securityExchangePduData.clientRandom != null)
            {
                if (context.ServerExponent == null || context.ServerModulus == null)
                {
                    throw new InvalidDataException(
                        "The public key of the certificate to encrypt client random number is invalid.");
                }

                byte[] encryptedClientRandom = RdpbcgrUtility.GenerateEncryptedRandom(securityExchangePduData.clientRandom,
                                                                               context.ServerExponent,
                                                                               context.ServerModulus);
                securityExchangePduData.length = (uint)encryptedClientRandom.Length;
                RdpbcgrEncoder.EncodeStructure(randomBuffer, securityExchangePduData.length);
                RdpbcgrEncoder.EncodeBytes(randomBuffer, encryptedClientRandom);

                context.ClientRandomNumber = securityExchangePduData.clientRandom;
                context.GenerateSessionKey();
            }
            else
            {
                RdpbcgrEncoder.EncodeStructure(randomBuffer, 0);
            }

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, randomBuffer.ToArray(), context);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }


        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Client_Security_Exchange_Pdu cloneSecurityPdu = new Client_Security_Exchange_Pdu(context);

            cloneSecurityPdu.commonHeader = commonHeader.Clone();
            cloneSecurityPdu.securityExchangePduData = securityExchangePduData;
            cloneSecurityPdu.securityExchangePduData.clientRandom =
                RdpbcgrUtility.CloneByteArray(securityExchangePduData.clientRandom);

            return cloneSecurityPdu;
        }
    }

    /// <summary>
    ///  The TS_SECURITY_PACKET structure contains the encrypted
    ///  client random value which is used together with the
    ///  server random (see section ) to derive session keys
    ///  to secure the connection (see sections  and ). 
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_10_1.xml
    //  </remarks>
    public partial struct TS_SECURITY_PACKET
    {
        /// <summary>
        ///  32-bit unsigned integer.  The size in bytes of the buffer
        ///  containing the encrypted client random value, not including
        ///  the header length.
        /// </summary>
        public uint length;

        /// <summary>
        ///  The client random value encrypted with the public key
        ///  of the server (see section ).
        /// </summary>
        public byte[] clientRandom;
    }

    /// <summary>
    ///  The Client Info PDU is a Standard RDP Connection
    ///  Sequence PDU sent from client to server during the
    ///  Secure Settings Exchange phase (see section ). It is
    ///  sent after transmitting a Security Exchange PDU or,
    ///  if the Security Exchange PDU was not sent, it is transmitted
    ///  after receiving all requested MCS Channel Join Confirm
    ///  PDUs. 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/_rfc_ms-rdpbcgr2_1_1_11.xml
    //  </remarks>
    public partial class Client_Info_Pdu : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Client Info PDU, as specified
        ///  in section .
        /// </summary>
        public TS_INFO_PACKET infoPacket;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Client_Info_Pdu(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Client_Info_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] infoData = EncodeInfoData(infoPacket);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, infoData, context);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Client_Info_Pdu cloneClientInfoPdu = new Client_Info_Pdu(context);

            cloneClientInfoPdu.commonHeader = commonHeader.Clone();

            if (infoPacket != null)
            {
                cloneClientInfoPdu.infoPacket = new TS_INFO_PACKET();
                cloneClientInfoPdu.infoPacket.AlternateShell = infoPacket.AlternateShell;
                cloneClientInfoPdu.infoPacket.cbAlternateShell = infoPacket.cbAlternateShell;
                cloneClientInfoPdu.infoPacket.cbDomain = infoPacket.cbDomain;
                cloneClientInfoPdu.infoPacket.cbPassword = infoPacket.cbPassword;
                cloneClientInfoPdu.infoPacket.cbUserName = infoPacket.cbUserName;
                cloneClientInfoPdu.infoPacket.cbWorkingDir = infoPacket.cbWorkingDir;
                cloneClientInfoPdu.infoPacket.CodePage = infoPacket.CodePage;
                cloneClientInfoPdu.infoPacket.Domain = infoPacket.Domain;
                cloneClientInfoPdu.infoPacket.Password = infoPacket.Password;
                cloneClientInfoPdu.infoPacket.UserName = infoPacket.UserName;
                cloneClientInfoPdu.infoPacket.WorkingDir = infoPacket.WorkingDir;
                cloneClientInfoPdu.infoPacket.flags = infoPacket.flags;

                if (infoPacket.extraInfo != null)
                {
                    cloneClientInfoPdu.infoPacket.extraInfo = new TS_EXTENDED_INFO_PACKET();
                    if (infoPacket.extraInfo.autoReconnectCookie != null)
                    {
                        cloneClientInfoPdu.infoPacket.extraInfo.autoReconnectCookie = new ARC_CS_PRIVATE_PACKET();
                        cloneClientInfoPdu.infoPacket.extraInfo.autoReconnectCookie.cbLen =
                            infoPacket.extraInfo.autoReconnectCookie.cbLen;
                        cloneClientInfoPdu.infoPacket.extraInfo.autoReconnectCookie.LogonId =
                            infoPacket.extraInfo.autoReconnectCookie.LogonId;
                        cloneClientInfoPdu.infoPacket.extraInfo.autoReconnectCookie.SecurityVerifier =
                            RdpbcgrUtility.CloneByteArray(infoPacket.extraInfo.autoReconnectCookie.SecurityVerifier);
                        cloneClientInfoPdu.infoPacket.extraInfo.autoReconnectCookie.Version =
                            infoPacket.extraInfo.autoReconnectCookie.Version;
                    }

                    cloneClientInfoPdu.infoPacket.extraInfo.cbAutoReconnectLen =
                        infoPacket.extraInfo.cbAutoReconnectLen;
                    cloneClientInfoPdu.infoPacket.extraInfo.cbClientAddress = infoPacket.extraInfo.cbClientAddress;
                    cloneClientInfoPdu.infoPacket.extraInfo.cbClientDir = infoPacket.extraInfo.cbClientDir;
                    cloneClientInfoPdu.infoPacket.extraInfo.clientAddress = infoPacket.extraInfo.clientAddress;
                    cloneClientInfoPdu.infoPacket.extraInfo.clientAddressFamily =
                        infoPacket.extraInfo.clientAddressFamily;
                    cloneClientInfoPdu.infoPacket.extraInfo.clientDir = infoPacket.extraInfo.clientDir;
                    cloneClientInfoPdu.infoPacket.extraInfo.clientSessionId = infoPacket.extraInfo.clientSessionId;
                    cloneClientInfoPdu.infoPacket.extraInfo.clientTimeZone = infoPacket.extraInfo.clientTimeZone;
                    cloneClientInfoPdu.infoPacket.extraInfo.performanceFlags = infoPacket.extraInfo.performanceFlags;
                    cloneClientInfoPdu.infoPacket.extraInfo.reserved1 = infoPacket.extraInfo.reserved1;
                    cloneClientInfoPdu.infoPacket.extraInfo.reserved2 = infoPacket.extraInfo.reserved2;

                    cloneClientInfoPdu.infoPacket.extraInfo.cbDynamicDSTTimeZoneKeyName = infoPacket.extraInfo.cbDynamicDSTTimeZoneKeyName;
                    if (infoPacket.extraInfo.cbDynamicDSTTimeZoneKeyName != null && infoPacket.extraInfo.cbDynamicDSTTimeZoneKeyName.actualData > 0)
                    {
                        cloneClientInfoPdu.infoPacket.extraInfo.dynamicDSTTimeZoneKeyName = (string)infoPacket.extraInfo.dynamicDSTTimeZoneKeyName.Clone();
                    }
                    cloneClientInfoPdu.infoPacket.extraInfo.dynamicDaylightTimeDisabled = infoPacket.extraInfo.dynamicDaylightTimeDisabled;
                }

            }

            return cloneClientInfoPdu;
        }

        /// <summary>
        /// Encode infoPacket field.
        /// </summary>
        /// <param name="infoPacket">The data to be encoded.</param>
        /// <returns>The encoded data.</returns>
        private static byte[] EncodeInfoData(TS_INFO_PACKET infoPacket)
        {
            List<byte> infoBuffer = new List<byte>();
            if (infoPacket != null)
            {
                RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.CodePage);
                RdpbcgrEncoder.EncodeStructure(infoBuffer, (uint)infoPacket.flags);
                RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.cbDomain);
                RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.cbUserName);
                RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.cbPassword);
                RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.cbAlternateShell);
                RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.cbWorkingDir);
                if ((infoPacket.flags & flags_Values.INFO_UNICODE) == flags_Values.INFO_UNICODE)
                {
                    // +2 means add a null-terminator for a unicode string
                    RdpbcgrEncoder.EncodeUnicodeString(infoBuffer, infoPacket.Domain, (uint)(infoPacket.cbDomain + 2));
                    RdpbcgrEncoder.EncodeUnicodeString(infoBuffer,
                                                       infoPacket.UserName,
                                                       (uint)(infoPacket.cbUserName + 2));
                    RdpbcgrEncoder.EncodeUnicodeString(infoBuffer,
                                                       infoPacket.Password,
                                                       (uint)(infoPacket.cbPassword + 2));
                    RdpbcgrEncoder.EncodeUnicodeString(infoBuffer,
                                                       infoPacket.AlternateShell,
                                                       (uint)(infoPacket.cbAlternateShell + 2));
                    RdpbcgrEncoder.EncodeUnicodeString(infoBuffer,
                                                       infoPacket.WorkingDir,
                                                       (uint)(infoPacket.cbWorkingDir + 2));
                }
                else
                {
                    // +1 means add a null-terminator for an ansi string
                    RdpbcgrEncoder.EncodeAnsiString(infoBuffer, infoPacket.Domain, (uint)(infoPacket.cbDomain + 1));
                    RdpbcgrEncoder.EncodeAnsiString(infoBuffer,
                                                    infoPacket.UserName,
                                                    (uint)(infoPacket.cbUserName + 1));
                    RdpbcgrEncoder.EncodeAnsiString(infoBuffer,
                                                    infoPacket.Password,
                                                    (uint)(infoPacket.cbPassword + 1));
                    RdpbcgrEncoder.EncodeAnsiString(infoBuffer,
                                                    infoPacket.AlternateShell,
                                                    (uint)(infoPacket.cbAlternateShell + 1));
                    RdpbcgrEncoder.EncodeAnsiString(infoBuffer,
                                                    infoPacket.WorkingDir,
                                                    (uint)(infoPacket.cbWorkingDir + 1));
                }

                if (infoPacket.extraInfo != null)
                {
                    RdpbcgrEncoder.EncodeStructure(infoBuffer, (ushort)infoPacket.extraInfo.clientAddressFamily);
                    RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.extraInfo.cbClientAddress);
                    if ((infoPacket.flags & flags_Values.INFO_UNICODE) == flags_Values.INFO_UNICODE)
                    {
                        RdpbcgrEncoder.EncodeUnicodeString(infoBuffer,
                                                           infoPacket.extraInfo.clientAddress,
                                                           infoPacket.extraInfo.cbClientAddress);
                    }
                    else
                    {
                        RdpbcgrEncoder.EncodeAnsiString(infoBuffer,
                                                        infoPacket.extraInfo.clientAddress,
                                                        infoPacket.extraInfo.cbClientAddress);
                    }

                    RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.extraInfo.cbClientDir);
                    if ((infoPacket.flags & flags_Values.INFO_UNICODE) == flags_Values.INFO_UNICODE)
                    {
                        RdpbcgrEncoder.EncodeUnicodeString(infoBuffer,
                                                           infoPacket.extraInfo.clientDir,
                                                           infoPacket.extraInfo.cbClientDir);
                    }
                    else
                    {
                        RdpbcgrEncoder.EncodeAnsiString(infoBuffer,
                                                        infoPacket.extraInfo.clientDir,
                                                        infoPacket.extraInfo.cbClientDir);
                    }

                    RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.extraInfo.clientTimeZone.Bias);
                    RdpbcgrEncoder.EncodeUnicodeString(infoBuffer,
                                                       infoPacket.extraInfo.clientTimeZone.StandardName,
                                                       ConstValue.TIME_ZONE_STANDARD_NAME_SIZE);
                    RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.extraInfo.clientTimeZone.StandardDate);
                    RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.extraInfo.clientTimeZone.StandardBias);
                    RdpbcgrEncoder.EncodeUnicodeString(infoBuffer,
                                                       infoPacket.extraInfo.clientTimeZone.DaylightName,
                                                       ConstValue.TIME_ZONE_DAYLIGHT_NAME_SIZE);
                    RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.extraInfo.clientTimeZone.DaylightDate);
                    RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.extraInfo.clientTimeZone.DaylightBias);

                    RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.extraInfo.clientSessionId);
                    RdpbcgrEncoder.EncodeStructure(infoBuffer, (uint)infoPacket.extraInfo.performanceFlags);
                    RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.extraInfo.cbAutoReconnectLen);

                    if (infoPacket.extraInfo.cbAutoReconnectLen != 0)
                    {
                        RdpbcgrEncoder.EncodeStructure(infoBuffer,
                                                       (uint)infoPacket.extraInfo.autoReconnectCookie.cbLen);
                        RdpbcgrEncoder.EncodeStructure(infoBuffer,
                                                       (uint)infoPacket.extraInfo.autoReconnectCookie.Version);
                        RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.extraInfo.autoReconnectCookie.LogonId);
                        RdpbcgrEncoder.EncodeBytes(infoBuffer,
                                                   infoPacket.extraInfo.autoReconnectCookie.SecurityVerifier); ;
                    }

                    if (infoPacket.extraInfo.reserved1 != null)
                    {
                        RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.extraInfo.reserved1.actualData);
                    }

                    if (infoPacket.extraInfo.reserved2 != null)
                    {
                        RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.extraInfo.reserved2.actualData);
                    }

                    if (infoPacket.extraInfo.cbDynamicDSTTimeZoneKeyName != null)
                    {
                        RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.extraInfo.cbDynamicDSTTimeZoneKeyName.actualData);
                    }

                    if (infoPacket.extraInfo.dynamicDSTTimeZoneKeyName != null)
                    {
                        RdpbcgrEncoder.EncodeUnicodeString(infoBuffer,
                                                           infoPacket.extraInfo.dynamicDSTTimeZoneKeyName,
                                                           infoPacket.extraInfo.cbDynamicDSTTimeZoneKeyName.actualData);
                    }

                    if (infoPacket.extraInfo.dynamicDaylightTimeDisabled != null)
                    {
                        RdpbcgrEncoder.EncodeStructure(infoBuffer, infoPacket.extraInfo.dynamicDaylightTimeDisabled.actualData);
                    }
                }
            }

            return infoBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_INFO_PACKET structure contains extra information
    ///  not passed to the server during the Basic Settings
    ///  Exchange phase (see section ) of the Standard RDP Connection
    ///  Sequence, primarily to ensure that it gets encrypted
    ///  (as auto-logon password data and other sensitive information
    ///  is passed here). 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //  _rfc_ms-rdpbcgr2_1_1_11_1_1.xml
    //  </remarks>
    public partial class TS_INFO_PACKET
    {

        /// <summary>
        ///  A 32-bit unsigned integer.  This field contains the
        ///  ANSI codepage descriptor being used by the client (for
        ///  a list of code pages, see [MSDN-CP]). However, if the
        ///  Info Packet is in Unicode (the INFO_UNICODE flag is
        ///  set), then the CodePage field is overridden to contain
        ///  the active input locale identifier in the low word
        ///  (for a list of possible input locales, see [MSDN-MUI]).
        /// </summary>
        public uint CodePage;

        /// <summary>
        ///  A 32-bit unsigned integer.  Option flags. 				 				
        /// </summary>
        public flags_Values flags;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  character data in the Domain field. This size excludes
        ///  the length of the mandatory null terminator.
        /// </summary>
        public ushort cbDomain;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  character data in the UserName field. This size excludes
        ///  the length of the mandatory null terminator.
        /// </summary>
        public ushort cbUserName;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  character data in the Password field. This size excludes
        ///  the length of the mandatory null terminator.
        /// </summary>
        public ushort cbPassword;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  character data in the AlternateShell field. This size
        ///  excludes the length of the mandatory null terminator.
        /// </summary>
        public ushort cbAlternateShell;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  character data in the WorkingDir field. This size excludes
        ///  the length of the mandatory null terminator.
        /// </summary>
        public ushort cbWorkingDir;

        /// <summary>
        ///  Variable length logon domain of the user (the length
        ///  in bytes is given by the cbDomain field). The maximum
        ///  length allowed by RDP 4.0 and RDP 5.0 servers is 52
        ///  bytes (including the mandatory null terminator). RDP
        ///  5.1 and later allow a maximum length of 512 bytes (including
        ///  the mandatory null terminator). The field must contain
        ///  at least a null terminator character in ANSI or Unicode
        ///  format (depending on the presence of the INFO_UNICODE
        ///  flag).
        /// </summary>
        public string Domain;

        /// <summary>
        ///  Variable length logon user name of the user (the length
        ///  in bytes is given by the cbUserName field). The maximum
        ///  length allowed by RDP 4.0 and RDP 5.0 servers is 44
        ///  bytes (including the mandatory null terminator). RDP
        ///  5.1 and later allow a maximum length of 512 bytes (including
        ///  the mandatory null terminator). The field must contain
        ///  at least a null terminator character in ANSI or Unicode
        ///  format (depending on the presence of the INFO_UNICODE
        ///  flag).
        /// </summary>
        public string UserName;

        /// <summary>
        ///  Variable length logon password of the user (the length
        ///  in bytes is given by the cbPassword field). The maximum
        ///  length allowed by RDP 4.0 and RDP 5.0 servers is 32
        ///  bytes (including the mandatory null terminator). RDP
        ///  5.1 and later allow a maximum length of 512 bytes (including
        ///  the mandatory null terminator). The field must contain
        ///  at least a null terminator character in ANSI or Unicode
        ///  format (depending on the presence of the INFO_UNICODE
        ///  flag).
        /// </summary>
        public string Password;

        /// <summary>
        ///  Variable length path to the executable file of an alternate
        ///  shell, e.g. "c:\dir\prog.exe" (the length in bytes
        ///  is given by the cbAlternateShell field). The maximum
        ///  allowed length is 512 bytes (including the mandatory
        ///  null terminator). This field MUST only be initialized
        ///  if the client is requesting a shell other than the
        ///  default. The field must contain at least a null terminator
        ///  character in ANSI or Unicode format (depending on the
        ///  presence of the INFO_UNICODE flag).
        /// </summary>
        public string AlternateShell;

        /// <summary>
        ///  Variable length directory that contains the executable
        ///  file specified in the AlternateShell field or any related
        ///  files (the length in bytes is given by the cbWorkingDir
        ///  field). The maximum allowed length is 512 bytes (including
        ///  the mandatory null terminator). This field MAY be initialized
        ///  if the client is requesting a shell other than the
        ///  default. The field must contain at least a null terminator
        ///  character in ANSI or Unicode format (depending on the
        ///  presence of the INFO_UNICODE flag).
        /// </summary>
        public string WorkingDir;

        /// <summary>
        ///  Optional and variable length extended information added
        ///  in RDP 5.0, as specified in section .
        /// </summary>
        public TS_EXTENDED_INFO_PACKET extraInfo;
    }

    /// <summary>
    /// The flag type of Client Info PDU.
    /// </summary>
    public enum flags_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates that the client machine has a mouse attached.
        /// </summary>
        INFO_MOUSE = 0x00000001,

        /// <summary>
        ///  Indicates that the CTRL+ALT+DEL (or the equivalent)
        ///  secure access keyboard sequence is not required at
        ///  the logon prompt.
        /// </summary>
        INFO_DISABLECTRLALTDEL = 0x00000002,

        /// <summary>
        ///  The client requests auto logon using the included user
        ///  name, password and domain.
        /// </summary>
        INFO_AUTOLOGON = 0x00000008,

        /// <summary>
        ///  Indicates that the character set for strings in the
        ///  Info Packet is Unicode. If this flag is absent, the
        ///  character set is ANSI. The presence of this flag affects
        ///  the meaning of the CodePage field in the Info Packet
        ///  structure.
        /// </summary>
        INFO_UNICODE = 0x00000010,

        /// <summary>
        ///  Specifies whether the alternate shell (specified in
        ///  the AlternateShell field of the Info Packet structure)
        ///  should be started in a maximized state.
        /// </summary>
        INFO_MAXIMIZESHELL = 0x00000020,

        /// <summary>
        ///  Indicates that the client wants to be informed of the
        ///  user name and domain used to log on  to the server,
        ///  as well as the ID of the session to which the user
        ///  connected. The Save Session Info PDU is sent from the
        ///  server to notify the client of this information using
        ///  a Logon Info Version 1 or Logon Info Version 2 structure.
        /// </summary>
        INFO_LOGONNOTIFY = 0x00000040,

        /// <summary>
        ///  Indicates that the CompressionTypeMask is valid and
        ///  contains the highest compression package type supported
        ///  by the client.
        /// </summary>
        INFO_COMPRESSION = 0x00000080,

        /// <summary>
        ///  Indicates the highest compression package type supported.
        ///  See the discussion which follows this table for more
        ///  information.
        /// </summary>
        CompressionTypeMask = 0x00001E00,

        /// <summary>
        ///  Indicates that the client uses the Windows key on Windows-compatible
        ///  keyboards.
        /// </summary>
        INFO_ENABLEWINDOWSKEY = 0x00000100,

        /// <summary>
        ///  Requests that any audio played in a remote session be
        ///  played on the remote computer (see [MS-RDPEA]).
        /// </summary>
        INFO_REMOTECONSOLEAUDIO = 0x00002000,

        /// <summary>
        ///  Indicates that the client will only send encrypted packets
        ///  to the server if encryption is in force. Setting this
        ///  flag prevents the server from processing unencrypted
        ///  packets in man-in-the-middle attack scenarios. This
        ///  flag is only understood by RDP 5.2 and later servers.
        /// </summary>
        INFO_FORCE_ENCRYPTED_CS_PDU = 0x00004000,

        /// <summary>
        ///  Indicates that the remote connection being established
        ///  is for the purpose of launching remote programs (see
        ///  Section [MS-RDPERP]). This flag is only understood
        ///  by RDP 6.0 and later servers.
        /// </summary>
        INFO_RAIL = 0x00008000,

        /// <summary>
        ///  Indicates a request for logon error notifications using
        ///  the Save Session Info PDU. This flag is only understood
        ///  by RDP 6.0 and later servers.
        /// </summary>
        INFO_LOGONERRORS = 0x00010000,

        /// <summary>
        ///  Indicates that the mouse which is connected to the client
        ///  machine has a scroll wheel. This flag is only understood
        ///  by RDP 6.0 and later servers.
        /// </summary>
        INFO_MOUSE_HAS_WHEEL = 0x00020000,

        /// <summary>
        ///  Indicates that the Password field in the Info Packet
        ///  contains a smart card personal identification number
        ///  (PIN). This flag is only understood by RDP 6.0 and
        ///  later servers.
        /// </summary>
        INFO_PASSWORD_IS_SC_PIN = 0x00040000,

        /// <summary>
        ///  Indicates that no audio redirection or playback (see
        ///  [MS-RDPEA]) should take place. This flag is only understood
        ///  by RDP 6.0 and later servers.
        /// </summary>
        INFO_NOAUDIOPLAYBACK = 0x00080000,

        /// <summary>
        ///  Any user credentials sent on the wire during the RDP
        ///  Connection Sequence (see Sections  and ) have been
        ///  retrieved from a credential store and were not obtained
        ///  directly from the user.
        /// </summary>
        INFO_USING_SAVED_CREDS = 0x00100000,

        /// <summary>
        /// Indicates that the redirection of client-side audio input to a session hosted on a remote server is supported using the protocol defined in [MS-RDPEAI] sections 2 and 3. 
        /// This flag is understood only by RDP 7.0, 7.1, 8.0, and 8.1 servers.
        /// </summary>
        INFO_AUDIOCAPTURE = 0x00200000,

        /// <summary>
        /// Indicates that video redirection or playback (using the protocol defined in [MS-RDPEV] sections 2 and 3) MUST NOT take place. 
        /// This flag is understood only by RDP 7.0, 7.1, 8.0, and 8.1 servers.
        /// </summary>
        INFO_VIDEO_DISABLE = 0x00400000,

        /// <summary>
        /// An unused flag that is reserved for future use. This flag MUST NOT be set.
        /// </summary>
        INFO_RESERVED1 = 0x00800000,

        /// <summary>
        /// An unused flag that is reserved for future use. This flag MUST NOT be set.
        /// </summary>
        INFO_RESERVED2 = 0x01000000,

        /// <summary>
        /// Indicates that the client supports Hi-Def RAIL ([MS-RDPERP] section 1.3.3). The INFO_HIDEF_RAIL_SUPPORTED flag MUST be ignored if the INFO_RAIL (0x00008000) flag is not specified. 
        /// Furthermore, a client that specifies the INFO_HIDEF_RAIL_SUPPORTED flag MUST send the Remote Programs Capability Set ([MS-RDPERP] section 2.2.1.1.1) to the server.The INFO_HIDEF_RAIL_SUPPORTED flag is understood only by RDP 8.1 servers.
        /// </summary>
        INFO_HIDEF_RAIL_SUPPORTED = 0x02000000

    }

    /// <summary>
    ///  The CompressionTypeMask is a 4-bit enumerated value containing 
    ///  the highest compression package support available on the client. 
    ///  The packages codes are:
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum CompressionType : byte
    {
        /// <summary>
        ///  MPPC 8K compression (see MPPC-8K)
        /// </summary>
        PACKET_COMPR_TYPE_8K = 0,

        /// <summary>
        ///  MPPC 64K compression (see MPPC-64K)
        /// </summary>
        PACKET_COMPR_TYPE_64K = 1,

        /// <summary>
        ///  RDP 6.0 bulk compression (see  [MS-RDPEGDI] section
        ///  ).
        /// </summary>
        PACKET_COMPR_TYPE_RDP6 = 2,

        /// <summary>
        ///  RDP 6.1 bulk compression (see  [MS-RDPEGDI] section
        ///  ).
        /// </summary>
        PACKET_COMPR_TYPE_RDP61 = 3,

        /// <summary>
        ///  No compression is supported.
        /// </summary>
        PACKET_COMPR_TYPE_NONE = 15,
    }

    /// <summary>
    ///  The TS_EXTENDED_INFO_PACKET structure contains
    ///  user information specific to RDP 5.0, RDP 5.1, and
    ///  RDP 5.2.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_11_1_1_1.xml
    //  </remarks>
    public partial class TS_EXTENDED_INFO_PACKET
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The numeric socket descriptor
        ///  for the client address type. RDP only uses TCP/IP,
        ///  so this field MUST be set to AF_INET (0x0002).
        /// </summary>
        public clientAddressFamily_Values clientAddressFamily;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  character data in the clientAddress field. This size
        ///  includes the length of the mandatory null terminator.
        /// </summary>
        public ushort cbClientAddress;

        /// <summary>
        ///  Variable length textual representation of the client
        ///  IP address. The maximum allowed length (including the
        ///  mandatory null terminator) is 64 bytes for versions
        ///  of RDP prior to 6.1, and 80 bytes for RDP 6.1 and later.
        /// </summary>
        public string clientAddress;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  character data in the clientDir field. This size includes
        ///  the length of the mandatory null terminator.
        /// </summary>
        public ushort cbClientDir;

        /// <summary>
        ///  Variable length directory that contains the folder path
        ///  on the client machine from which the client software
        ///  is being run. The maximum allowed length is 512 bytes
        ///  (including the mandatory null terminator).
        /// </summary>
        public string clientDir;

        /// <summary>
        ///  A TS_TIME_ZONE_INFORMATION structure that contains time
        ///  zone information for a client. This packet is ignored
        ///  by RDP 5.1 servers and earlier, but is used by RDP
        ///  5.2 and later servers.
        /// </summary>
        public TS_TIME_ZONE_INFORMATION clientTimeZone;

        /// <summary>
        ///  A 32-bit unsigned integer.  This field was added in
        ///  RDP 5.1 and is currently ignored by the server. It
        ///  SHOULD be set to 0.
        /// </summary>
        public uint clientSessionId;

        /// <summary>
        ///  A 32-bit unsigned integer.  It specifies a list of server
        ///  desktop shell features to disable in the remote session
        ///  for improving bandwidth performance. Used by RDP 5.1
        ///  and later servers.
        /// </summary>
        public performanceFlags_Values performanceFlags;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  cookie specified by the autoReconnectCookie field.
        ///  This field is only read by RDP 5.2 and later servers.
        /// </summary>
        public ushort cbAutoReconnectLen;

        /// <summary>
        ///  Buffer containing an ARC_CS_PRIVATE_PACKET structure.
        ///  This buffer is a unique cookie that allows a disconnected
        ///  client to seamlessly reconnect to a previously established
        ///  session (see section   for more details). The autoReconnectCookie
        ///  field is only read by RDP 5.2 and later servers.
        /// </summary>
        public ARC_CS_PRIVATE_PACKET autoReconnectCookie;

        /// <summary>
        ///  This field is reserved for future use and has no affect
        ///  on RDP wire traffic. If this field is present, the
        ///  reserved2 field MUST be present.
        /// </summary>
        public UInt16Class reserved1;

        /// <summary>
        ///  This field is reserved for future use and has no affect
        ///  on RDP wire traffic. This field MUST be present if
        ///  the reserved1 field is present.
        /// </summary>
        public UInt16Class reserved2;

        /// <summary>
        /// The size, in bytes, of the dynamicDSTTimeZoneKeyName field. 
        /// </summary>
        public UInt16Class cbDynamicDSTTimeZoneKeyName;

        /// <summary>
        /// A variable-length array of Unicode characters with no terminating null, 
        /// containing the descriptive name of the Dynamic DST time zone on the client.
        /// </summary>
        public string dynamicDSTTimeZoneKeyName;

        /// <summary>
        /// A 16-bit, unsigned integer that specifies whether Dynamic DST MUST be disabled in the remote session. 
        /// </summary>
        public UInt16Class dynamicDaylightTimeDisabled;
    }

    /// <summary>
    /// The type of clientAddressFamily.
    /// </summary>
    public enum clientAddressFamily_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  AF_INET 0x00002: The clientAddress field contains an IPv4 address.
        /// </summary>
        V1 = 0x0002,

        /// <summary>
        ///  AF_INET6 0x0017: The clientAddress field contains an IPv6 address.
        /// </summary>
        V2 = 0x0017,
    }

    /// <summary>
    /// The type of performanceFlags.
    /// </summary>
    public enum performanceFlags_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Disable desktop wallpaper.
        /// </summary>
        PERF_DISABLE_WALLPAPER = 0x00000001,

        /// <summary>
        ///  Disable full-window drag (only the window outline is
        ///  displayed when the window is moved).
        /// </summary>
        PERF_DISABLE_FULLWINDOWDRAG = 0x00000002,

        /// <summary>
        ///  Disable menu animations.
        /// </summary>
        PERF_DISABLE_MENUANIMATIONS = 0x00000004,

        /// <summary>
        ///  Disable user interface themes.
        /// </summary>
        PERF_DISABLE_THEMING = 0x00000008,

        /// <summary>
        ///  Reserved for future use.
        /// </summary>
        PERF_RESERVED1 = 0x00000010,

        /// <summary>
        ///  Disable mouse cursor shadows.
        /// </summary>
        PERF_DISABLE_CURSOR_SHADOW = 0x00000020,

        /// <summary>
        ///  Disable cursor blinking.
        /// </summary>
        PERF_DISABLE_CURSORSETTINGS = 0x00000040,

        /// <summary>
        ///  Enable font smoothing.
        /// </summary>
        PERF_ENABLE_FONT_SMOOTHING = 0x00000080,

        /// <summary>
        ///  Enable Desktop Composition.
        /// </summary>
        PERF_ENABLE_DESKTOP_COMPOSITION = 0x00000100,

        /// <summary>
        ///  Reserved for future use.
        /// </summary>
        PERF_RESERVED2 = 0x80000000,
    }

    /// <summary>
    ///  The TS_TIME_ZONE_INFORMATION structure contains
    ///  client time zone information. 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_11_1_1_1_1.xml
    //  </remarks>
    public partial struct TS_TIME_ZONE_INFORMATION
    {

        /// <summary>
        ///  A 32-bit unsigned integer. Current bias for local time
        ///  translation on the client, in minutes. The bias is
        ///  the difference, in minutes, between Coordinated Universal
        ///  Time (UTC) and local time. All translations between
        ///  UTC and local time are based on the following formula:UTC
        ///  = local time + bias
        /// </summary>
        public int Bias;

        /// <summary>
        ///  An array of 32 Unicode characters. A description for
        ///  standard time on the client. For example, this field
        ///  could contain the string "Pacific Standard Time" to
        ///  indicate Pacific Standard Time. 
        /// </summary>
        public string StandardName;

        /// <summary>
        ///  A TS_SYSTEMTIME structure that contains the date and
        ///  local time when the transition from daylight saving
        ///  time to standard time occurs on the client. If this
        ///  field is specified, the DaylightDate field is also
        ///  specified.
        /// </summary>
        public TS_SYSTEMTIME StandardDate;

        /// <summary>
        ///  A 32-bit unsigned integer.  Bias value to be used during
        ///  local time translations that occur during standard
        ///  time. This field should be ignored if a value is not
        ///  supplied in the StandardDate field. This value is added
        ///  to the value of the Bias field to form the bias used
        ///  during standard time. In most time zones, the value
        ///  of this field is 0.
        /// </summary>
        public int StandardBias;

        /// <summary>
        ///  A description for daylight time on the client. For example,
        ///  this field could contain "Pacific Daylight Time" to
        ///  indicate Pacific Daylight Time. An array of 32 Unicode
        ///  characters.
        /// </summary>
        public string DaylightName;

        /// <summary>
        ///  A TS_SYSTEMTIME that contains a date and local time
        ///  when the transition from standard time to daylight
        ///  saving time occurs on the client. If this field is
        ///  specified, the StandardDate field is also specified.
        /// </summary>
        public TS_SYSTEMTIME DaylightDate;

        /// <summary>
        ///  A 32-bit unsigned integer.  Bias value to be used during
        ///  local time translations that occur during daylight
        ///  saving time. This field should be ignored if a value
        ///  for the DaylightDate field is not supplied. This value
        ///  is added to the value of the Bias field to form the
        ///  bias used during daylight saving time. In most time
        ///  zones, the value of this field is 60.
        /// </summary>
        public int DaylightBias;
    }

    /// <summary>
    ///  The TS_SYSTEMTIME structure contains a date and
    ///  local time when the transition occurs between daylight
    ///  saving time to standard time occurs or standard time
    ///  to daylight saving time. 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_11_1_1_1_1_1.xml
    //  </remarks>
    public partial struct TS_SYSTEMTIME
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The year when transition
        ///  occurs (1601 to 30827).
        /// </summary>
        public ushort wYear;

        /// <summary>
        ///  A 16-bit unsigned integer. The month when transition
        ///  occurs.
        /// </summary>
        public wMonth_Values wMonth;

        /// <summary>
        ///  A 16-bit unsigned integer. The day of the week when
        ///  transition occurs.
        /// </summary>
        public wDayOfWeek_Values wDayOfWeek;

        /// <summary>
        ///  A 16-bit unsigned integer. The occurrence
        ///  of wDayOfWeek within the month when the transition
        ///  takes place.
        /// </summary>
        public wDay_Values wDay;

        /// <summary>
        ///  A 16-bit unsigned integer. The hour when transition
        ///  occurs (0 to 23).
        /// </summary>
        public ushort wHour;

        /// <summary>
        ///  A 16-bit unsigned integer. The minute when transition
        ///  occurs (0 to 59).
        /// </summary>
        public ushort wMinute;

        /// <summary>
        ///  A 16-bit unsigned integer. The second when transition
        ///  occurs (0 to 59).
        /// </summary>
        public ushort wSecond;

        /// <summary>
        ///  A 16-bit unsigned integer. The millisecond when transition
        ///  occurs (0 to 999).
        /// </summary>
        public ushort wMilliseconds;
    }

    /// <summary>
    /// The type of wMonth.
    /// </summary>
    public enum wMonth_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  January
        /// </summary>
        V1 = 1,

        /// <summary>
        ///  February
        /// </summary>
        V2 = 2,

        /// <summary>
        ///  March
        /// </summary>
        V3 = 3,

        /// <summary>
        ///  April
        /// </summary>
        V4 = 4,

        /// <summary>
        ///  May
        /// </summary>
        V5 = 5,

        /// <summary>
        ///  June
        /// </summary>
        V6 = 6,

        /// <summary>
        ///  July
        /// </summary>
        V7 = 7,

        /// <summary>
        ///  August
        /// </summary>
        V8 = 8,

        /// <summary>
        ///  September
        /// </summary>
        V9 = 9,

        /// <summary>
        ///  October
        /// </summary>
        V10 = 10,

        /// <summary>
        ///  November
        /// </summary>
        V11 = 11,

        /// <summary>
        ///  December
        /// </summary>
        V12 = 12,
    }

    /// <summary>
    /// The type of wDayOfWeek.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum wDayOfWeek_Values : ushort
    {

        /// <summary>
        ///  Sunday
        /// </summary>
        V1 = 0,

        /// <summary>
        ///  Monday
        /// </summary>
        V2 = 1,

        /// <summary>
        ///  Tuesday
        /// </summary>
        V3 = 2,

        /// <summary>
        ///  Wednesday
        /// </summary>
        V4 = 3,

        /// <summary>
        ///  Thursday
        /// </summary>
        V5 = 4,

        /// <summary>
        ///  Friday
        /// </summary>
        V6 = 5,

        /// <summary>
        ///  Saturday
        /// </summary>
        V7 = 6,
    }

    /// <summary>
    /// The type of wDay.
    /// </summary>
    public enum wDay_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  First occurrence of wDayOfWeek
        /// </summary>
        V1 = 1,

        /// <summary>
        ///  Second occurrence of wDayOfWeek
        /// </summary>
        V2 = 2,

        /// <summary>
        ///  Third occurrence of wDayOfWeek
        /// </summary>
        V3 = 3,

        /// <summary>
        ///  Fourth occurrence of wDayOfWeek
        /// </summary>
        V4 = 4,

        /// <summary>
        ///  Last occurrence of wDayOfWeek
        /// </summary>
        V5 = 5,
    }

    /// <summary>
    ///  The License Error (Valid Client) PDU is a Standard
    ///  RDP Connection Sequence PDU sent from server to client
    ///  during the Licensing phase (see section ). This licensing
    ///  PDU indicates that the server will not issue the client
    ///  a license to store and that the Licensing Phase has
    ///  ended successfully. This is one possible message that
    ///  may be sent during the Licensing Phase (see [MS-RDPELE]
    ///  for a detailed discussion of the Remote Desktop Protocol:
    ///  Licensing Extension).       
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_12.xml
    //  </remarks>
    public partial class Server_License_Error_Pdu_Valid_Client : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  Licensing Preamble structure containing header information.
        ///  The bMsgType field of the preamble structure should
        ///  be set to ERROR_ALERT (0xFF).           
        /// </summary>
        public LICENSE_PREAMBLE preamble;

        /// <summary>
        ///  A Licensing Error Message structure. The
        ///  dwErrorCode field of the error message structure MUST
        ///  be set to STATUS_VALID_CLIENT (0x00000007) and the
        ///  dwStateTransition field MUST be set to ST_NO_TRANSITION
        ///  (0x00000002). The bbErrorInfo field MUST contain an
        ///  empty binary BLOB of type BB_ERROR_BLOB (0x0004).
        /// </summary>
        public LICENSE_ERROR_MESSAGE validClientMessage;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public Server_License_Error_Pdu_Valid_Client(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_License_Error_Pdu_Valid_Client()
        {
        }

        public override byte[] ToBytes()
        {
            List<byte> dataList = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(dataList, (byte)preamble.bMsgType);
            RdpbcgrEncoder.EncodeStructure(dataList, (byte)preamble.bVersion);
            RdpbcgrEncoder.EncodeStructure(dataList, preamble.wMsgSize);
            RdpbcgrEncoder.EncodeStructure(dataList, (uint)validClientMessage.dwErrorCode);
            RdpbcgrEncoder.EncodeStructure(dataList, (uint)validClientMessage.dwStateTransition);
            RdpbcgrEncoder.EncodeStructure(dataList, (ushort)validClientMessage.bbErrorInfo.wBlobType);
            RdpbcgrEncoder.EncodeStructure(dataList, validClientMessage.bbErrorInfo.wBlobLen);
            RdpbcgrEncoder.EncodeBytes(dataList, validClientMessage.bbErrorInfo.blobData);

            byte[] dataBuffer = dataList.ToArray();

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, dataBuffer);

            return encodedBytes;
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Server_License_Error_Pdu_Valid_Client cloneServerLicenseErrorPdu =
                new Server_License_Error_Pdu_Valid_Client(serverSessionContext);

            cloneServerLicenseErrorPdu.commonHeader = commonHeader.Clone();
            cloneServerLicenseErrorPdu.preamble = preamble;

            cloneServerLicenseErrorPdu.validClientMessage = validClientMessage;
            cloneServerLicenseErrorPdu.validClientMessage.bbErrorInfo.blobData =
                RdpbcgrUtility.CloneByteArray(validClientMessage.bbErrorInfo.blobData);

            return cloneServerLicenseErrorPdu;
        }
    }

    /// <summary>
    ///  The LICENSE_PREAMBLE structure precedes every licensing
    ///  packet sent on the wire. 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_12_1.xml
    //  </remarks>
    public partial struct LICENSE_PREAMBLE
    {
        /// <summary>
        ///  An 8-bit unsigned integer. A type of the licensing packet.
        ///  For more details about the different licensing packets,
        ///  see [MS-RDPELE].Sent by server:
        /// </summary>
        public bMsgType_Values bMsgType;

        /// <summary>
        ///  An 8-bit unsigned integer. The license protocol version.
        /// </summary>
        public bVersion_Values bVersion;

        /// <summary>
        ///  An 16-bit, unsigned integer. 
        ///  The size in bytes of the licensing packet (including the size of the preamble)
        /// </summary>
        public ushort wMsgSize;
    }

    /// <summary>
    /// The type of bMsgType.
    /// </summary>
    public enum bMsgType_Values : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates a License Request PDU (see [MS-RDPELE]).
        /// </summary>
        LICENSE_REQUEST = 0x01,

        /// <summary>
        ///  Indicates a Platform Challenge PDU (see [MS-RDPELE]).
        /// </summary>
        PLATFORM_CHALLENGE = 0x02,

        /// <summary>
        ///  Indicates a New License PDU (see [MS-RDPELE]).
        /// </summary>
        NEW_LICENSE = 0x03,

        /// <summary>
        ///  Indicates an Upgrade License PDU (see [MS-RDPELE]).
        /// </summary>
        UPGRADE_LICENSE = 0x04,

        /// <summary>
        ///  Indicates a License Info PDU (see [MS-RDPELE]).
        /// </summary>
        LICENSE_INFO = 0x12,

        /// <summary>
        ///  Indicates a New License Request PDU (see [MS-RDPELE]).
        /// </summary>
        NEW_LICENSE_REQUEST = 0x13,

        /// <summary>
        ///  Indicates a Platform Challenge Response PDU (see [MS-RDPELE]).
        /// </summary>
        PLATFORM_CHALLENGE_RESPONSE = 0x15,

        /// <summary>
        ///  Indicates a Licensing Error Message PDU.
        /// </summary>
        ERROR_ALERT = 0xFF,
    }

    /// <summary>
    /// The type of bVersion.
    /// </summary>
    public enum bVersion_Values : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  RDP 4.0
        /// </summary>
        PREAMBLE_VERSION_2_0 = 0x02,

        /// <summary>
        /// RDP 5.0, 5.1, 5.2, 6.0, 6.1, 7.0, 7.1, 8.0, 8.1, 10.0, 10.1, 10.2, 10.3, 10.4, and 10.5
        /// </summary>
        PREAMBLE_VERSION_3_0 = 0x03,

        /// <summary>
        /// Indicates that extended error information using the Licensing Error Message (section 2.2.1.12.1.3) is supported.
        /// </summary>
        EXTENDED_ERROR_MSG_SUPPORTED = 0x80
    }

    /// <summary>
    ///  The LICENSE_BINARY_BLOB structure is used to encapsulate
    ///  arbitrary length binary licensing data.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_12_2.xml
    //  </remarks>
    public partial struct LICENSE_BINARY_BLOB
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The data type of the binary
        ///  information. If wBlobLen is set to 0, then the contents
        ///  of this field SHOULD be ignored.
        /// </summary>
        public wBlobType_Values wBlobType;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  binary information in the blobData field. If wBlobLen
        ///  is set to 0, then the blobData field is not included
        ///  in the Licensing Binary BLOB structure and the contents
        ///  of the wBlobType field SHOULD be ignored.
        /// </summary>
        public ushort wBlobLen;

        /// <summary>
        ///  Variable-length binary data. The size of this data in
        ///  bytes is given by the wBlobLen field. If wBlobLen is
        ///  set to 0, then this field is not included in the Licensing
        ///  Binary BLOB structure.
        /// </summary>
        [Size("wBlobLen")]
        public byte[] blobData;
    }

    /// <summary>
    /// The type of wBlobType.
    /// </summary>
    public enum wBlobType_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Used by License Info PDU and Platform Challenge Response
        ///  PDU (see [MS-RDPELE]).
        /// </summary>
        BB_DATA_BLOB = 0x0001,

        /// <summary>
        ///  Used by License Info PDU and New License Request PDU
        ///  (see [MS-RDPELE]).
        /// </summary>
        BB_RANDOM_BLOB = 0x0002,

        /// <summary>
        ///  Used by License Request PDU (see [MS-RDPELE]).
        /// </summary>
        BB_CERTIFICATE_BLOB = 0x0003,

        /// <summary>
        ///  Used by License Error PDU.
        /// </summary>
        BB_ERROR_BLOB = 0x0004,

        /// <summary>
        ///  Used by Platform Challenge Response PDU and Server Upgrade
        ///  License PDU (see [MS-RDPELE]).
        /// </summary>
        BB_ENCRYPTED_DATA_BLOB = 0x0009,

        /// <summary>
        ///  Used by License Request PDU (see [MS-RDPELE]).
        /// </summary>
        BB_KEY_EXCHG_ALG_BLOB = 0x000D,

        /// <summary>
        ///  Used by License Request PDU ([MS-RDPELE]).
        /// </summary>
        BB_SCOPE_BLOB = 0x000E,

        /// <summary>
        ///  Used by New License Request PDU (see [MS-RDPELE]).
        /// </summary>
        BB_CLIENT_USER_NAME_BLOB = 0x000F,

        /// <summary>
        ///  Used by New License Request PDU (see [MS-RDPELE]).
        /// </summary>
        BB_CLIENT_MACHINE_NAME_BLOB = 0x0010,
    }

    /// <summary>
    ///  The LICENSE_ERROR_MESSAGE structure is used to
    ///  indicate that an error occurred during the licensing
    ///  protocol. Alternatively, it is also used to notify
    ///  the peer of important status information. 				    
    ///  
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_12_3.xml
    //  </remarks>
    public partial struct LICENSE_ERROR_MESSAGE
    {
        /// <summary>
        ///  A 32-bit unsigned integer. The error or status code. Sent
        ///  by client:
        /// </summary>
        public dwErrorCode_Values dwErrorCode;

        /// <summary>
        ///  A 32-bit unsigned integer. The licensing state to transition
        ///  into upon receipt of this message. For more details
        ///  about how this field is used, see [MS-RDPELE].
        /// </summary>
        public dwStateTransition_Values dwStateTransition;

        /// <summary>
        ///  A LICENSE_BINARY_BLOB structure which MUST contain 
        ///  a BLOB of type BB_ERROR_BLOB (0x0004) that includes
        ///  information relevant to the error code specified in
        ///  dwErrorCode.
        /// </summary>
        public LICENSE_BINARY_BLOB bbErrorInfo;
    }

    /// <summary>
    /// The type of dwErrorCode.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum dwErrorCode_Values : uint
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        ERR_NO_ERROR_CODE = 0x00000000,

        /// <summary>
        ///  Possible value.
        /// </summary>
        ERR_INVALID_SERVER_CERTIFICATE = 0x00000001,

        /// <summary>
        ///  Possible value.
        /// </summary>
        ERR_NO_LICENSE = 0x00000002,

        /// <summary>
        ///  Possible value.
        /// </summary>
        ERR_INVALID_SCOPE = 0x00000004,

        /// <summary>
        ///  Possible value.
        /// </summary>
        ERR_NO_LICENSE_SERVER = 0x00000006,

        /// <summary>
        ///  Possible value.
        /// </summary>
        STATUS_VALID_CLIENT = 0x00000007,

        /// <summary>
        ///  Possible value.
        /// </summary>
        ERR_INVALID_CLIENT = 0x00000008,

        /// <summary>
        ///  Possible value.
        /// </summary>
        ERR_INVALID_PRODUCTID = 0x0000000B,

        /// <summary>
        ///  Possible value.
        /// </summary>
        ERR_INVALID_MESSAGE_LEN = 0x0000000C,

        /// <summary>
        ///  Possible value.
        /// </summary>
        ERR_INVALID_MAC = 0x00000003,
    }

    /// <summary>
    /// The type of dwStateTransition.
    /// </summary>
    public enum dwStateTransition_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        ST_TOTAL_ABORT = 0x00000001,

        /// <summary>
        ///  Possible value.
        /// </summary>
        ST_NO_TRANSITION = 0x00000002,

        /// <summary>
        ///  Possible value.
        /// </summary>
        ST_RESET_PHASE_TO_START = 0x00000003,

        /// <summary>
        ///  Possible value.
        /// </summary>
        ST_RESEND_LAST_MESSAGE = 0x00000004,
    }

    /// <summary>
    ///  The Demand Active PDU is a Standard RDP Connection
    ///  Sequence PDU sent from server to client during the
    ///  Capabilities Negotiation phase (see section ). It is
    ///  sent upon successful completion of the Licensing phase
    ///  (see section ) of the connection sequence.      
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_13_1.xml
    //  </remarks>
    public partial class Server_Demand_Active_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Demand Active
        ///  PDU, as specified in section .           
        /// </summary>
        public TS_DEMAND_ACTIVE_PDU demandActivePduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public Server_Demand_Active_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_Demand_Active_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] demandData = EncodeConfirmData(demandActivePduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, demandData, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, demandData);

            return encodedBytes;
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Server_Demand_Active_Pdu cloneServerDemandActivePdu = new Server_Demand_Active_Pdu(serverSessionContext);
            cloneServerDemandActivePdu.commonHeader = commonHeader.Clone();

            if (demandActivePduData != null)
            {
                cloneServerDemandActivePdu.demandActivePduData = new TS_DEMAND_ACTIVE_PDU();
                cloneServerDemandActivePdu.demandActivePduData.shareControlHeader =
                    demandActivePduData.shareControlHeader;
                cloneServerDemandActivePdu.demandActivePduData.shareId = demandActivePduData.shareId;
                cloneServerDemandActivePdu.demandActivePduData.lengthCombinedCapabilities =
                    demandActivePduData.lengthCombinedCapabilities;
                cloneServerDemandActivePdu.demandActivePduData.lengthSourceDescriptor =
                    demandActivePduData.lengthSourceDescriptor;
                cloneServerDemandActivePdu.demandActivePduData.sourceDescriptor =
                    RdpbcgrUtility.CloneByteArray(demandActivePduData.sourceDescriptor);
                cloneServerDemandActivePdu.demandActivePduData.numberCapabilities =
                    demandActivePduData.numberCapabilities;
                cloneServerDemandActivePdu.demandActivePduData.pad2Octets = demandActivePduData.pad2Octets;
                cloneServerDemandActivePdu.demandActivePduData.sessionId = demandActivePduData.sessionId;

                if (demandActivePduData.capabilitySets != null)
                {
                    cloneServerDemandActivePdu.demandActivePduData.capabilitySets = new Collection<ITsCapsSet>();
                    for (int i = 0; i < demandActivePduData.capabilitySets.Count; ++i)
                    {
                        cloneServerDemandActivePdu.demandActivePduData.capabilitySets.Add(
                            RdpbcgrUtility.CloneCapabilitySet(demandActivePduData.capabilitySets[i]));
                    }

                }
            }

            return cloneServerDemandActivePdu;
        }

        /// <summary>
        /// Encode confirmActivePduData field.
        /// </summary>
        /// <param name="confirmActivePduData">The data to be encoded.</param>
        /// <returns>The encoded data.</returns>
        private static byte[] EncodeConfirmData(TS_DEMAND_ACTIVE_PDU demandActivePduData)
        {
            List<byte> demandBuffer = new List<byte>();

            if (demandActivePduData != null)
            {
                RdpbcgrEncoder.EncodeStructure(demandBuffer, demandActivePduData.shareControlHeader);
                RdpbcgrEncoder.EncodeStructure(demandBuffer, demandActivePduData.shareId);
                RdpbcgrEncoder.EncodeStructure(demandBuffer, demandActivePduData.lengthSourceDescriptor);
                RdpbcgrEncoder.EncodeStructure(demandBuffer, demandActivePduData.lengthCombinedCapabilities);
                RdpbcgrEncoder.EncodeBytes(demandBuffer, demandActivePduData.sourceDescriptor);
                RdpbcgrEncoder.EncodeStructure(demandBuffer, demandActivePduData.numberCapabilities);
                RdpbcgrEncoder.EncodeStructure(demandBuffer, demandActivePduData.pad2Octets);

                for (int i = 0; i < demandActivePduData.numberCapabilities; ++i)
                {
                    ITsCapsSet capability = demandActivePduData.capabilitySets[i];
                    RdpbcgrEncoder.EncodeBytes(demandBuffer, capability.ToBytes());
                }
                RdpbcgrEncoder.EncodeStructure(demandBuffer, demandActivePduData.sessionId);
            }

            return demandBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_DEMAND_ACTIVE_PDU structure is a standard
    ///  T.128 Demand Active PDU (see  [T128] section 8.4.1).
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_13_1_1.xml
    //  </remarks>
    public partial class TS_DEMAND_ACTIVE_PDU
    {
        /// <summary>
        ///  Share Control Header containing information about the
        ///  packet. The type subfield of the pduType field of the
        ///  Share Control Header MUST be set to PDUTYPE_DEMANDACTIVEPDU
        ///  (1).
        /// </summary>
        public TS_SHARECONTROLHEADER shareControlHeader;

        /// <summary>
        ///  A 32-bit unsigned integer.  The share identifier for
        ///  the packet (see [T128] section 8.4.2 for more information
        ///  regarding share IDs).
        /// </summary>
        public uint shareId;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  sourceDescriptor field.
        /// </summary>
        public ushort lengthSourceDescriptor;

        /// <summary>
        ///  A 16-bit unsigned integer. The combined size in bytes
        ///  of the numberCapabilities, pad2Octets and capabilitySets
        ///  fields.
        /// </summary>
        public ushort lengthCombinedCapabilities;

        /// <summary>
        ///  The source descriptor. The contents of this field SHOULD
        ///  be set to { 0x52, 0x44, 0x50, 0x00 }, which is the
        ///  ANSI representation of the null-terminated string "RDP"
        ///  in hexadecimal.
        /// </summary>
        public byte[] sourceDescriptor;

        /// <summary>
        ///  A 16-bit unsigned integer. The number of capability
        ///  sets included in the Demand Active_PDU.
        /// </summary>
        public ushort numberCapabilities;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2Octets;

        /// <summary>
        ///  An array of TS_CAPS_SET structures. Collection of capability
        ///  sets, each conforming to the TS_CAPS_SET structure.
        ///  The number of capability sets is specified by the numberCapabilities
        ///  field.
        /// </summary>
        public Collection<ITsCapsSet> capabilitySets;

        /// <summary>
        ///  A 32-bit unsigned integer.  The session identifier.
        ///  This field is ignored by the client and SHOULD be set
        ///  to 0x00000000.
        /// </summary>
        public uint sessionId;
    }

    /// <summary>
    ///  The TS_CAPS_SET structure is used to describe the
    ///  type and size of a capability set exchanged between
    ///  clients and servers. All capability sets conform to
    ///  this basic structure (see section ). 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_13_1_1_1.xml
    //  </remarks>
    public partial struct TS_CAPS_SET
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The type identifier of the
        ///  capability set.
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  Capability set data which conforms to the structure
        ///  of the type given by the capabilitySetType field.
        /// </summary>
        public byte[] capabilityData;
    }

    /// <summary>
    /// The type of capabilitySetType.
    /// </summary>
    public enum capabilitySetType_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  General Capability Set.
        /// </summary>
        CAPSTYPE_GENERAL = 1,

        /// <summary>
        ///  Bitmap Capability Set.
        /// </summary>
        CAPSTYPE_BITMAP = 2,

        /// <summary>
        ///  Order Capability Set.
        /// </summary>
        CAPSTYPE_ORDER = 3,

        /// <summary>
        ///  Revision 1 Bitmap Cache Capability Set.
        /// </summary>
        CAPSTYPE_BITMAPCACHE = 4,

        /// <summary>
        ///  Control Capability Set.
        /// </summary>
        CAPSTYPE_CONTROL = 5,

        /// <summary>
        ///  Window Activation Capability Set.
        /// </summary>
        CAPSTYPE_ACTIVATION = 7,

        /// <summary>
        ///  Pointer Capability Set.
        /// </summary>
        CAPSTYPE_POINTER = 8,

        /// <summary>
        ///  Share Capability Set.
        /// </summary>
        CAPSTYPE_SHARE = 9,

        /// <summary>
        ///  Color Table Cache Capability Set (see  [MS-RDPEGDI]
        ///  section ).
        /// </summary>
        CAPSTYPE_COLORCACHE = 10,

        /// <summary>
        ///  Sound Capability Set.
        /// </summary>
        CAPSTYPE_SOUND = 12,

        /// <summary>
        ///  Input Capability Set.
        /// </summary>
        CAPSTYPE_INPUT = 13,

        /// <summary>
        ///  Font Capability Set.
        /// </summary>
        CAPSTYPE_FONT = 14,

        /// <summary>
        ///  Brush Capability Set.
        /// </summary>
        CAPSTYPE_BRUSH = 15,

        /// <summary>
        ///  Glyph Cache Capability Set.
        /// </summary>
        CAPSTYPE_GLYPHCACHE = 16,

        /// <summary>
        ///  Offscreen Bitmap Cache Capability Set.
        /// </summary>
        CAPSTYPE_OFFSCREENCACHE = 17,

        /// <summary>
        ///  Bitmap Cache Host Support Capability Set.
        /// </summary>
        CAPSTYPE_BITMAPCACHE_HOSTSUPPORT = 18,

        /// <summary>
        ///  Revision 2 Bitmap Cache Capability Set.
        /// </summary>
        CAPSTYPE_BITMAPCACHE_REV2 = 19,

        /// <summary>
        ///  Virtual Channel Capability Set.
        /// </summary>
        CAPSTYPE_VIRTUALCHANNEL = 20,

        /// <summary>
        ///  DrawNineGrid Cache Capability Set ([MS-RDPEGDI] section
        ///  .
        /// </summary>
        CAPSTYPE_DRAWNINEGRIDCACHE = 21,

        /// <summary>
        ///  Draw GDI+ Cache Capability Set ([MS-RDPEGDI] section
        ///  ).
        /// </summary>
        CAPSTYPE_DRAWGDIPLUS = 22,

        /// <summary>
        ///  Remote Programs Capability Set ([MS-RDPERP] section
        ///   ).
        /// </summary>
        CAPSTYPE_RAIL = 23,

        /// <summary>
        ///  Window List Capability Set ([MS-RDPERP] section ).
        /// </summary>
        CAPSTYPE_WINDOW = 24,

        /// <summary>
        ///  Desktop Composition Extension Capability Set (section
        ///  ).
        /// </summary>
        CAPSETTYPE_COMPDESK = 25,

        /// <summary>
        ///  Multifragment Update Capability Set (section ).
        /// </summary>
        CAPSETTYPE_MULTIFRAGMENTUPDATE = 26,

        /// <summary>
        ///  Large Pointer Capability Set (section ).
        /// </summary>
        CAPSETTYPE_LARGE_POINTER = 27,

        /// <summary>
        ///  Surface Commands Capability Set (section ).
        /// </summary>
        CAPSETTYPE_SURFACE_COMMANDS = 28,

        /// <summary>
        ///  Bitmap Codecs Capability Set (section ).
        /// </summary>
        CAPSETTYPE_BITMAP_CODECS = 29,


        CAPSETTYPE_FRAME_ACKNOWLEDGE = 30,
    }

    /// <summary>
    ///  The Confirm Active PDU is a Standard RDP Connection
    ///  Sequence PDU sent from client to server during the
    ///  Capabilities Negotiation phase (see section ). It is
    ///  sent as a response to the Demand Active PDU.      
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_13_2.xml
    //  </remarks>
    public partial class Client_Confirm_Active_Pdu : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Confirm Active PDU, as specified
        ///  in section .           
        /// </summary>
        public TS_CONFIRM_ACTIVE_PDU confirmActivePduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Client_Confirm_Active_Pdu(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Client_Confirm_Active_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] confirmData = EncodeConfirmData(confirmActivePduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, confirmData, context);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Client_Confirm_Active_Pdu cloneConfirmActivePdu = new Client_Confirm_Active_Pdu(context);
            cloneConfirmActivePdu.commonHeader = commonHeader.Clone();

            if (confirmActivePduData != null)
            {
                cloneConfirmActivePdu.confirmActivePduData = confirmActivePduData.Clone();
            }

            return cloneConfirmActivePdu;
        }

        /// <summary>
        /// Encode confirmActivePduData field.
        /// </summary>
        /// <param name="confirmActivePduData">The data to be encoded.</param>
        /// <returns>The encoded data.</returns>
        private static byte[] EncodeConfirmData(TS_CONFIRM_ACTIVE_PDU confirmActivePduData)
        {
            List<byte> confirmBuffer = new List<byte>();

            if (confirmActivePduData != null)
            {
                RdpbcgrEncoder.EncodeStructure(confirmBuffer, confirmActivePduData.shareControlHeader);
                RdpbcgrEncoder.EncodeStructure(confirmBuffer, confirmActivePduData.shareId);
                RdpbcgrEncoder.EncodeStructure(confirmBuffer, (ushort)confirmActivePduData.originatorId);
                RdpbcgrEncoder.EncodeStructure(confirmBuffer, confirmActivePduData.lengthSourceDescriptor);
                RdpbcgrEncoder.EncodeStructure(confirmBuffer, confirmActivePduData.lengthCombinedCapabilities);
                RdpbcgrEncoder.EncodeBytes(confirmBuffer, confirmActivePduData.sourceDescriptor);
                RdpbcgrEncoder.EncodeStructure(confirmBuffer, confirmActivePduData.numberCapabilities);
                RdpbcgrEncoder.EncodeStructure(confirmBuffer, confirmActivePduData.pad2Octets);

                for (int i = 0; i < confirmActivePduData.numberCapabilities; ++i)
                {
                    ITsCapsSet capability = confirmActivePduData.capabilitySets[i];
                    RdpbcgrEncoder.EncodeBytes(confirmBuffer, capability.ToBytes());
                }
            }

            return confirmBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_CONFIRM_ACTIVE_PDU structure is a standard
    ///  T.128 Confirm Active PDU (see  [T128] section 8.4.1).
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_13_2_1.xml
    //  </remarks>
    public partial class TS_CONFIRM_ACTIVE_PDU
    {
        /// <summary>
        ///  Share Control Header containing information about the
        ///  packet.The type subfield of the pduType field of the
        ///  Share Control Header MUST be set to PDUTYPE_CONFIRMACTIVEPDU
        ///  (3).
        /// </summary>
        public TS_SHARECONTROLHEADER shareControlHeader;

        /// <summary>
        ///  A 32-bit unsigned integer.  The share identifier for
        ///  the packet (see [T128] section 8.4.2 for more information
        ///  regarding share IDs).
        /// </summary>
        public uint shareId;

        /// <summary>
        ///  A 16-bit unsigned integer.  The identifier of the packet
        ///  originator. This field MUST be set to the server channel
        ///  ID (in Microsoft RDP server implementations, this value
        ///  is always 0x03EA).
        /// </summary>
        public originatorId_Values originatorId;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  sourceDescriptor field.
        /// </summary>
        public ushort lengthSourceDescriptor;

        /// <summary>
        ///  A 16-bit unsigned integer. The combined size in bytes
        ///  of the numberCapabilities, pad2Octets and capabilitySets
        ///  fields.
        /// </summary>
        public ushort lengthCombinedCapabilities;

        /// <summary>
        ///  Source descriptor. The Microsoft RDP client sets the
        ///  contents of this field to { 0x4D, 0x53, 0x54, 0x53,
        ///  0x43, 0x00 }, which is the ANSI representation of the
        ///  null-terminated string "MSTSC" in hexadecimal.
        /// </summary>
        public byte[] sourceDescriptor;

        /// <summary>
        ///  A 16-bit unsigned integer. Number of capability sets
        ///  included in the Confirm Active PDU.
        /// </summary>
        public ushort numberCapabilities;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2Octets;

        /// <summary>
        ///  An array of TS_CAPS_SET structures. Collection of capability
        ///  sets, each conforming to the TS_CAPS_SET structure.
        ///  The number of capability sets is specified by the numberCapabilities
        ///  field.
        /// </summary>
        public Collection<ITsCapsSet> capabilitySets;

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public TS_CONFIRM_ACTIVE_PDU Clone()
        {
            TS_CONFIRM_ACTIVE_PDU cloneConfirmActivePdu = new TS_CONFIRM_ACTIVE_PDU();

            cloneConfirmActivePdu.lengthCombinedCapabilities = lengthCombinedCapabilities;
            cloneConfirmActivePdu.lengthSourceDescriptor = lengthSourceDescriptor;
            cloneConfirmActivePdu.numberCapabilities = numberCapabilities;
            cloneConfirmActivePdu.originatorId = originatorId;
            cloneConfirmActivePdu.pad2Octets = pad2Octets;
            cloneConfirmActivePdu.shareControlHeader = shareControlHeader;
            cloneConfirmActivePdu.shareId = shareId;
            cloneConfirmActivePdu.sourceDescriptor = RdpbcgrUtility.CloneByteArray(sourceDescriptor);
            if (capabilitySets != null)
            {
                cloneConfirmActivePdu.capabilitySets = new Collection<ITsCapsSet>();
                for (int i = 0; i < capabilitySets.Count; ++i)
                {
                    cloneConfirmActivePdu.capabilitySets.Add(RdpbcgrUtility.CloneCapabilitySet(capabilitySets[i]));
                }
            }

            return cloneConfirmActivePdu;
        }
    }

    /// <summary>
    /// Used for TS_CAPS_SET structures. All capabilities should implement this interface.
    /// </summary>
    public interface ITsCapsSet
    {
        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        byte[] ToBytes();

        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        capabilitySetType_Values GetCapabilityType();
    }

    /// <summary>
    /// The type of originatorId.
    /// </summary>
    public enum originatorId_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x03EA,
    }

    /// <summary>
    ///  The Client Synchronize PDU is a Standard RDP
    ///  Connection Sequence PDU sent from client to server
    ///  during the Connection Finalization phase (see section
    ///  ). It is sent after transmitting the Confirm Active
    ///  PDU.      
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_14.xml
    //  </remarks>
    public partial class Client_Synchronize_Pdu : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Synchronize PDU, as specified
        ///  in section .           
        /// </summary>
        public TS_SYNCHRONIZE_PDU synchronizePduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Client_Synchronize_Pdu(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Client_Synchronize_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] dataBuffer = EncodeSynchronizeData(synchronizePduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, context);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }


        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Client_Synchronize_Pdu cloneSychronizePdu = new Client_Synchronize_Pdu(context);

            cloneSychronizePdu.commonHeader = commonHeader.Clone();
            cloneSychronizePdu.synchronizePduData = synchronizePduData;

            return cloneSychronizePdu;
        }

        /// <summary>
        /// Encode synchronizePduData field.
        /// </summary>
        /// <param name="synchronizePduData">The data to be encoded.</param>
        /// <returns>The encoded data.</returns>
        private static byte[] EncodeSynchronizeData(TS_SYNCHRONIZE_PDU synchronizePduData)
        {
            List<byte> dataBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(dataBuffer, synchronizePduData.shareDataHeader);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, (ushort)synchronizePduData.messageType);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, synchronizePduData.targetUser);

            return dataBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_SYNCHRONIZE_PDU structure is a standard
    ///  T.128 Synchronize PDU (see  [T128] section 8.6.1).
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_14_1.xml
    //  </remarks>
    public partial struct TS_SYNCHRONIZE_PDU
    {
        /// <summary>
        ///  Share Control Header containing information about the
        ///  packet. The type subfield of the pduType field of the
        ///  Share Control Header MUST be set to PDUTYPE_DATAPDU
        ///  (7). The pduType2 field of the Share Data Header MUST
        ///  be set to PDUTYPE2_SYNCHRONIZE (31).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  A 16-bit unsigned integer. The message type. This field
        ///  MUST be set to SYNCMSGTYPE_SYNC (1).
        /// </summary>
        public messageType_Values messageType;

        /// <summary>
        ///  A 16-bit unsigned integer. The MCS channel ID of the
        ///  target user.
        /// </summary>
        public ushort targetUser;
    }

    /// <summary>
    /// The type of messageType.
    /// </summary>
    public enum messageType_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 1,
    }

    /// <summary>
    ///  The Client Control PDU (Cooperate) is a Standard
    ///  RDP Connection Sequence PDU sent from client to server
    ///  during the Connection Finalization phase (see section
    ///  ). It is sent after transmitting the Client Synchronize
    ///  PDU.      
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_15.xml
    //  </remarks>
    public partial class Client_Control_Pdu_Cooperate : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Control PDU, as specified
        ///  in section . The grantId and controlId fields of the
        ///  Control PDU Data MUST both be set to zero, while the
        ///  action field MUST be set to CTRLACTION_COOPERATE (0x0004).
        /// </summary>
        public TS_CONTROL_PDU controlPduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Client_Control_Pdu_Cooperate(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Client_Control_Pdu_Cooperate()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] dataBuffer = EncodeControlData(controlPduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, context);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Client_Control_Pdu_Cooperate cloneControlCoopPdu = new Client_Control_Pdu_Cooperate(context);

            cloneControlCoopPdu.commonHeader = commonHeader.Clone();
            cloneControlCoopPdu.controlPduData = controlPduData;

            return cloneControlCoopPdu;
        }

        /// <summary>
        /// Encode controlPduData field.
        /// </summary>
        /// <param name="controlPduData">The data to be encoded.</param>
        /// <returns>The encoded data.</returns>
        private static byte[] EncodeControlData(TS_CONTROL_PDU controlPduData)
        {
            List<byte> dataBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(dataBuffer, controlPduData.shareDataHeader);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, (ushort)controlPduData.action);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, controlPduData.grantId);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, controlPduData.controlId);

            return dataBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_CONTROL_PDU structure is a standard T.128
    ///  Synchronize PDU (see  [T128] section 8.12).
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_15_1.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 26)]
    public partial struct TS_CONTROL_PDU
    {
        /// <summary>
        ///  Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share
        ///  Control Header MUST be set to PDUTYPE_DATAPDU (7).
        ///  The pduType2 field of the Share Data Header MUST be
        ///  set to PDUTYPE2_CONTROL (20).
        /// </summary>
        [FieldOffset(0)]
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  A 16-bit unsigned integer. The action code.
        /// </summary>
        [FieldOffset(18)]
        public action_Values action;

        /// <summary>
        ///  A 16-bit unsigned integer. The grant identifier.
        /// </summary>
        [FieldOffset(20)]
        public ushort grantId;

        /// <summary>
        ///  A 32-bit unsigned integer.  The control identifier.
        /// </summary>
        [FieldOffset(22)]
        public uint controlId;
    }

    /// <summary>
    /// The type of action.
    /// </summary>
    public enum action_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Request control
        /// </summary>
        CTRLACTION_REQUEST_CONTROL = 0x0001,

        /// <summary>
        ///  Granted control
        /// </summary>
        CTRLACTION_GRANTED_CONTROL = 0x0002,

        /// <summary>
        ///  Detach
        /// </summary>
        CTRLACTION_DETACH = 0x0003,

        /// <summary>
        ///  Cooperate
        /// </summary>
        CTRLACTION_COOPERATE = 0x0004,
    }

    /// <summary>
    ///  The Client Control PDU (Request Control) is a
    ///  Standard RDP Connection Sequence PDU sent from client
    ///  to server during the Connection Finalization phase
    ///  (see section ). It is sent after transmitting the Client
    ///  Control PDU (Cooperate).      
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_16.xml
    //  </remarks>
    public partial class Client_Control_Pdu_Request_Control : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Control PDU, as specified
        ///  in section . The grantId and controlId fields of the
        ///  Control PDU Data MUST both be set to zero, while the
        ///  action field MUST be set to CTRLACTION_REQUEST_CONTROL
        ///  (0x0001).
        /// </summary>
        public TS_CONTROL_PDU controlPduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Client_Control_Pdu_Request_Control(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Client_Control_Pdu_Request_Control()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] dataBuffer = EncodeControlData(controlPduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, context);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Client_Control_Pdu_Request_Control cloneConrolReqPdu = new Client_Control_Pdu_Request_Control(context);

            cloneConrolReqPdu.commonHeader = commonHeader.Clone();
            cloneConrolReqPdu.controlPduData = controlPduData;

            return cloneConrolReqPdu;
        }

        /// <summary>
        /// Encode controlPduData field.
        /// </summary>
        /// <param name="controlPduData">The data to be encoded.</param>
        /// <returns>The encoded data.</returns>
        private static byte[] EncodeControlData(TS_CONTROL_PDU controlPduData)
        {
            List<byte> dataBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(dataBuffer, controlPduData.shareDataHeader);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, (ushort)controlPduData.action);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, controlPduData.grantId);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, controlPduData.controlId);

            return dataBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The Persistent Key List PDU is a Standard RDP
    ///  Connection Sequence PDU sent from client to server
    ///  during the Connection Finalization phase (see section
    ///  ). This PDU MAY be sent after transmitting the Client
    ///  Control PDU (Request Control). It MUST NOT be sent
    ///  to a server which does not advertise support for the
    ///  Bitmap Host Cache Support Capability Set.     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_17.xml
    //  </remarks>
    public partial class Client_Persistent_Key_List_Pdu : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Persistent Key List PDU,
        ///  as specified in section .           
        /// </summary>
        public TS_BITMAPCACHE_PERSISTENT_LIST_PDU persistentKeyListPduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>        
        public Client_Persistent_Key_List_Pdu(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Client_Persistent_Key_List_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] dataBuffer = EncodePersistentKeyListData(persistentKeyListPduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, context);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Client_Persistent_Key_List_Pdu clonePesistentKeyPdu = new Client_Persistent_Key_List_Pdu(context);
            clonePesistentKeyPdu.commonHeader = commonHeader.Clone();

            if (persistentKeyListPduData != null)
            {
                clonePesistentKeyPdu.persistentKeyListPduData = new TS_BITMAPCACHE_PERSISTENT_LIST_PDU();
                clonePesistentKeyPdu.persistentKeyListPduData.shareDataHeader =
                    persistentKeyListPduData.shareDataHeader;
                clonePesistentKeyPdu.persistentKeyListPduData.Pad2 = persistentKeyListPduData.Pad2;
                clonePesistentKeyPdu.persistentKeyListPduData.Pad3 = persistentKeyListPduData.Pad3;
                clonePesistentKeyPdu.persistentKeyListPduData.bBitMask = persistentKeyListPduData.bBitMask;
                clonePesistentKeyPdu.persistentKeyListPduData.numEntriesCache0 =
                    persistentKeyListPduData.numEntriesCache0;
                clonePesistentKeyPdu.persistentKeyListPduData.numEntriesCache1 =
                    persistentKeyListPduData.numEntriesCache1;
                clonePesistentKeyPdu.persistentKeyListPduData.numEntriesCache2 =
                    persistentKeyListPduData.numEntriesCache2;
                clonePesistentKeyPdu.persistentKeyListPduData.numEntriesCache3 =
                    persistentKeyListPduData.numEntriesCache3;
                clonePesistentKeyPdu.persistentKeyListPduData.numEntriesCache4 =
                    persistentKeyListPduData.numEntriesCache4;
                clonePesistentKeyPdu.persistentKeyListPduData.totalEntriesCache0 =
                    persistentKeyListPduData.totalEntriesCache0;
                clonePesistentKeyPdu.persistentKeyListPduData.totalEntriesCache1 =
                    persistentKeyListPduData.totalEntriesCache1;
                clonePesistentKeyPdu.persistentKeyListPduData.totalEntriesCache2 =
                    persistentKeyListPduData.totalEntriesCache2;
                clonePesistentKeyPdu.persistentKeyListPduData.totalEntriesCache3 =
                    persistentKeyListPduData.totalEntriesCache3;
                clonePesistentKeyPdu.persistentKeyListPduData.totalEntriesCache4 =
                    persistentKeyListPduData.totalEntriesCache4;

                if (persistentKeyListPduData.entries != null)
                {
                    clonePesistentKeyPdu.persistentKeyListPduData.entries =
                        new List<TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY>();
                    clonePesistentKeyPdu.persistentKeyListPduData.entries.AddRange(persistentKeyListPduData.entries);
                }
            }

            return clonePesistentKeyPdu;
        }

        /// <summary>
        /// Encode controlPduData field.
        /// </summary>
        /// <param name="persistentKeyListPduData">The data to be encoded.</param>
        /// <returns>The encoded data.</returns>
        private static byte[] EncodePersistentKeyListData(TS_BITMAPCACHE_PERSISTENT_LIST_PDU persistentKeyListPduData)
        {
            List<byte> dataBuffer = new List<byte>();

            if (persistentKeyListPduData != null)
            {
                RdpbcgrEncoder.EncodeStructure(dataBuffer, persistentKeyListPduData.shareDataHeader);
                RdpbcgrEncoder.EncodeStructure(dataBuffer, persistentKeyListPduData.numEntriesCache0);
                RdpbcgrEncoder.EncodeStructure(dataBuffer, persistentKeyListPduData.numEntriesCache1);
                RdpbcgrEncoder.EncodeStructure(dataBuffer, persistentKeyListPduData.numEntriesCache2);
                RdpbcgrEncoder.EncodeStructure(dataBuffer, persistentKeyListPduData.numEntriesCache3);
                RdpbcgrEncoder.EncodeStructure(dataBuffer, persistentKeyListPduData.numEntriesCache4);
                RdpbcgrEncoder.EncodeStructure(dataBuffer, persistentKeyListPduData.totalEntriesCache0);
                RdpbcgrEncoder.EncodeStructure(dataBuffer, persistentKeyListPduData.totalEntriesCache1);
                RdpbcgrEncoder.EncodeStructure(dataBuffer, persistentKeyListPduData.totalEntriesCache2);
                RdpbcgrEncoder.EncodeStructure(dataBuffer, persistentKeyListPduData.totalEntriesCache3);
                RdpbcgrEncoder.EncodeStructure(dataBuffer, persistentKeyListPduData.totalEntriesCache4);
                RdpbcgrEncoder.EncodeStructure(dataBuffer, (byte)persistentKeyListPduData.bBitMask);
                RdpbcgrEncoder.EncodeStructure(dataBuffer, persistentKeyListPduData.Pad2);
                RdpbcgrEncoder.EncodeStructure(dataBuffer, persistentKeyListPduData.Pad3);
                foreach (TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY entry in persistentKeyListPduData.entries)
                {
                    RdpbcgrEncoder.EncodeStructure(dataBuffer, entry);
                }
            }

            return dataBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_BITMAPCACHE_PERSISTENT_LIST_PDU structure
    ///  contains a list of cached bitmap keys saved from Cache
    ///  Bitmap (Revision 2) Orders (see [MS-RDPEGDI] section
    ///  2.2.2.3.1.2.3) which were sent in previous sessions.
    ///  By including a key in the Persistent Key List PDU Data
    ///   the client indicates to the server that it has a local
    ///  copy of the bitmap associated with the key, which implies
    ///  that the server does not need to retransmit the bitmap
    ///  to the client (for more details about the Persistent
    ///  Bitmap Cache, see [MS-RDPEGDI] section 3.1.1.1.1).
    ///  The bitmap keys can be sent in more than one Persistent
    ///  Key List PDU, with each PDU being marked using flags
    ///  in the bBitMask field.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_17_1.xml
    //  </remarks>
    public partial class TS_BITMAPCACHE_PERSISTENT_LIST_PDU
    {

        /// <summary>
        ///  Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share
        ///  Control Header MUST be set to PDUTYPE_DATAPDU (7).
        ///  The pduType2 field of the Share Data Header MUST be
        ///  set to PDUTYPE2_BITMAPCACHE_PERSISTENT_LIST (43).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  A 16-bit unsigned integer. The number of entries for
        ///  bitmap cache 0 in the current Persistent Key List PDU.
        /// </summary>
        public ushort numEntriesCache0;

        /// <summary>
        ///  A 16-bit unsigned integer. The number of entries for
        ///  bitmap cache 1 in the current Persistent Key List PDU.
        /// </summary>
        public ushort numEntriesCache1;

        /// <summary>
        ///  A 16-bit unsigned integer. The number of entries for
        ///  bitmap cache 2 in the current Persistent Key List PDU.
        /// </summary>
        public ushort numEntriesCache2;

        /// <summary>
        ///  A 16-bit unsigned integer. The number of entries for
        ///  bitmap cache 3 in the current Persistent Key List PDU.
        /// </summary>
        public ushort numEntriesCache3;

        /// <summary>
        ///  A 16-bit unsigned integer. The number of entries for
        ///  bitmap cache 4 in the current Persistent Key List PDU.
        /// </summary>
        public ushort numEntriesCache4;

        /// <summary>
        ///  A 16-bit unsigned integer. The total number of entries
        ///  for bitmap cache 0 expected across the entire sequence
        ///  of Persistent Key List PDUs. This value will remain
        ///  unchanged across the sequence.
        /// </summary>
        public ushort totalEntriesCache0;

        /// <summary>
        ///  A 16-bit unsigned integer. The total number of entries
        ///  for bitmap cache 1 expected across the entire sequence
        ///  of Persistent Key List PDUs. This value will remain
        ///  unchanged across the sequence.
        /// </summary>
        public ushort totalEntriesCache1;

        /// <summary>
        ///  A 16-bit unsigned integer. The total number of entries
        ///  for bitmap cache 2 expected across the entire sequence
        ///  of Persistent Key List PDUs. This value will remain
        ///  unchanged across the sequence.
        /// </summary>
        public ushort totalEntriesCache2;

        /// <summary>
        ///  A 16-bit unsigned integer. The total number of entries
        ///  for bitmap cache 3 expected across the entire sequence
        ///  of Persistent Key List PDUs. This value will remain
        ///  unchanged across the sequence.
        /// </summary>
        public ushort totalEntriesCache3;

        /// <summary>
        ///  A 16-bit unsigned integer. The total number of entries
        ///  for bitmap cache 4 expected across the entire sequence
        ///  of Persistent Key List PDUs. This value will remain
        ///  unchanged across the sequence.
        /// </summary>
        public ushort totalEntriesCache4;

        /// <summary>
        ///  An 8-bit unsigned integer. The sequencing flag.
        /// </summary>
        public bBitMask_Values bBitMask;

        /// <summary>
        ///  An 8-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public byte Pad2;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort Pad3;

        /// <summary>
        ///  An array of TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY structures
        ///  which describe 64-bit bitmap keys. The keys MUST be
        ///  arranged in order from low cache number to high cache
        ///  number. For instance, if a PDU contains one key for
        ///  cache 0 and two keys for cache 1, then numEntriesCache0
        ///  will be set to 1, numEntriesCache1 will be set to 2,
        ///  and numEntriesCache2, numEntriesCache3 and numEntriesCache4
        ///  will all be set to zero. The keys will be arranged
        ///  in the following order: (Cache 0, Key 1), (Cache 1,
        ///  Key 1), (Cache 1, Key 2).
        /// </summary>
        public List<TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY> entries;
    }

    /// <summary>
    /// The type of bBitMask.
    /// </summary>
    public enum bBitMask_Values : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates that the PDU is the first in a sequence of
        ///  Persistent Key List PDUs.
        /// </summary>
        PERSIST_FIRST_PDU = 0x01,

        /// <summary>
        ///  Indicates that the PDU is the last in a sequence of
        ///  Persistent Key List PDUs.
        /// </summary>
        PERSIST_LAST_PDU = 0x02,
    }

    /// <summary>
    ///  The TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY structure
    ///  contains a 64-bit bitmap key to be sent back to the
    ///  server.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_17_1_1.xml
    //  </remarks>
    public partial struct TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY
    {
        /// <summary>
        ///  Low 32 bits of the 64-bit persistent bitmap cache key.
        /// </summary>
        public uint Key1;

        /// <summary>
        ///  A 32-bit unsigned integer.  High 32 bits of the 64-bit
        ///  persistent bitmap cache key.
        /// </summary>
        public uint Key2;

        /// <summary>
        ///  constructor.
        /// </summary>
        public TS_BITMAPCACHE_PERSISTENT_LIST_ENTRY(uint key1, uint key2)
        {
            Key1 = key1;
            Key2 = key2;
        }
    }

    /// <summary>
    ///  The Font List PDU is a Standard RDP Connection
    ///  Sequence PDU sent from client to server during the
    ///  Connection Finalization phase (see section ). It is
    ///  sent after transmitting a Persistent Key List PDU or,
    ///  if the Persistent Key List PDU was not sent, it is
    ///  sent after transmitting the Client Control PDU (Request
    ///  Control). 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_18.xml
    //  </remarks>
    public partial class Client_Font_List_Pdu : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Font List PDU, as specified
        ///  in section .
        /// </summary>
        public TS_FONT_LIST_PDU fontListPduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Client_Font_List_Pdu(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Client_Font_List_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] dataBuffer = EncodeFontListData(fontListPduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, context);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Client_Font_List_Pdu cloneFontListPdu = new Client_Font_List_Pdu(context);
            cloneFontListPdu.commonHeader = commonHeader.Clone();

            if (fontListPduData != null)
            {
                cloneFontListPdu.fontListPduData = new TS_FONT_LIST_PDU();
                cloneFontListPdu.fontListPduData.shareDataHeader = fontListPduData.shareDataHeader;
                cloneFontListPdu.fontListPduData.entrySize = fontListPduData.entrySize;
                cloneFontListPdu.fontListPduData.listFlags = fontListPduData.listFlags;
                cloneFontListPdu.fontListPduData.numberFonts = fontListPduData.numberFonts;
                cloneFontListPdu.fontListPduData.totalNumFonts = fontListPduData.totalNumFonts;
            }

            return cloneFontListPdu;
        }

        /// <summary>
        /// Encode controlPduData field.
        /// </summary>
        /// <param name="fontListPduData">The data to be encoded.</param>
        /// <returns>The encoded data.</returns>
        private static byte[] EncodeFontListData(TS_FONT_LIST_PDU fontListPduData)
        {
            List<byte> dataBuffer = new List<byte>();

            if (fontListPduData != null)
            {
                RdpbcgrEncoder.EncodeStructure(dataBuffer, fontListPduData.shareDataHeader);
                RdpbcgrEncoder.EncodeStructure(dataBuffer, fontListPduData.numberFonts);
                RdpbcgrEncoder.EncodeStructure(dataBuffer, fontListPduData.totalNumFonts);
                RdpbcgrEncoder.EncodeStructure(dataBuffer, fontListPduData.listFlags);
                RdpbcgrEncoder.EncodeStructure(dataBuffer, fontListPduData.entrySize);
            }

            return dataBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_FONT_LIST_PDU structure contains information
    ///  that is sent to the server for legacy reasons.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_18_1.xml
    //  </remarks>
    public partial class TS_FONT_LIST_PDU
    {
        /// <summary>
        ///  Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share
        ///  Control Header MUST be set to PDUTYPE_DATAPDU (7).
        ///  The pduType2 field of the Share Data Header MUST be
        ///  set to PDUTYPE2_FONTLIST (39).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  A 16-bit unsigned integer. The number of fonts. This
        ///  field SHOULD be set to 0.
        /// </summary>
        public ushort numberFonts;

        /// <summary>
        ///  A 16-bit unsigned integer. The total number of fonts.
        ///  This field SHOULD be set to 0.
        /// </summary>
        public ushort totalNumFonts;

        /// <summary>
        ///  A 16-bit unsigned integer. The sequence flags. This
        ///  field SHOULD be set to 0x0003, which is the logical
        ///  OR'ed value of FONTLIST_FIRST (0x0001) and FONTLIST_LAST
        ///  (0x0002).
        /// </summary>
        public ushort listFlags;

        /// <summary>
        ///  A 16-bit unsigned integer. The entry size. This field
        ///  SHOULD be set to 0x0032 (50 bytes).
        /// </summary>
        public ushort entrySize;
    }

    /// <summary>
    ///  This type of PDU is RDPELE PDU.
    ///  It is especially used for RDPELE which is based on RDPBCGR.
    /// </summary>
    public class RdpelePdu : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        public LICENSE_PREAMBLE preamble;

        /// <summary>
        ///  The RDPELE message includes preamble and LicensingMessage.
        ///  This field will not be encoded/decoded in RDPBCGR.
        /// </summary>
        public byte[] rdpeleData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public RdpelePdu(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, rdpeleData, context);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }
    }

    /// <summary>
    ///  The Server Synchronize PDU is a Standard RDP
    ///  Connection Sequence PDU sent from server to client
    ///  during the Connection Finalization phase (see section
    ///  ). It is sent after receiving the Confirm Active PDU.
    ///       
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_19.xml
    //  </remarks>
    public partial class Server_Synchronize_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Synchronize PDU as described
        ///  in section .           
        /// </summary>
        public TS_SYNCHRONIZE_PDU synchronizePduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public Server_Synchronize_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_Synchronize_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] dataBuffer = EncodeSynchronizeData(synchronizePduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, dataBuffer);

            return encodedBytes;
        }

        private static byte[] EncodeSynchronizeData(TS_SYNCHRONIZE_PDU synchronizePduData)
        {
            List<byte> dataBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(dataBuffer, synchronizePduData.shareDataHeader);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, (ushort)synchronizePduData.messageType);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, synchronizePduData.targetUser);

            return dataBuffer.ToArray();
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Server_Synchronize_Pdu cloneSychronizePdu = new Server_Synchronize_Pdu(serverSessionContext);

            cloneSychronizePdu.commonHeader = commonHeader.Clone();
            cloneSychronizePdu.synchronizePduData = synchronizePduData;

            return cloneSychronizePdu;
        }
    }

    /// <summary>
    ///  The RDP Negotiation Request structure is used by a client
    ///  to advertise the security protocols which it supports.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_1_1.xml
    //  </remarks>
    public partial class RDP_NEG_REQ
    {

        /// <summary>
        ///  An 8-bit unsigned integer. Negotiation packet type.
        ///  This field MUST be set to 0x01 (TYPE_RDP_NEG_REQ) to
        ///  indicate that the packet is a Negotiation Request.
        /// </summary>
        public type_Values type;

        /// <summary>
        ///  An 8-bit unsigned integer. Negotiation packet flags.
        ///  There are currently no defined flags so the field MUST
        ///  be set to 0x00.
        /// </summary>
        public RDP_NEG_REQ_flags_Values flags;

        /// <summary>
        ///  A 16-bit unsigned integer. Indicates the packet size.
        ///  This field MUST be set to 0x0008 (8 bytes).
        /// </summary>
        public length_Values length;

        /// <summary>
        ///  A 32-bit unsigned integer. Flags indicating the supported
        ///  security protocols.
        /// </summary>
        public requestedProtocols_Values requestedProtocols;
    }

    /// <summary>
    /// The type of type.
    /// </summary>
    public enum type_Values : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x01,
    }

    /// <summary>
    /// The type of flags.
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum RDP_NEG_REQ_flags_Values : byte
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x00,

        /// <summary>
        /// Indicates that the client requires credential-less logon over CredSSP (also known as "restricted admin mode"). 
        /// If the server supports this mode then it is acceptable for the client to send empty credentials in the TSPasswordCreds structure defined in [MS-CSSP] section 2.2.1.2.1.
        /// </summary>
        RESTRICTED_ADMIN_MODE_REQUIRED = 0x01,

        /// <summary>
        /// Indicates that the client requires credential-less logon over CredSSP with redirected authentication over CredSSP (also known as "Remote Credential Guard"). 
        /// If the server supports this mode, the client can send a redirected logon buffer in the TSRemoteGuardCreds structure defined in [MS-CSSP] section 2.2.1.2.3.
        /// </summary>
        REDIRECTED_AUTHENTICATION_MODE_REQUIRED = 0x02,

        /// <summary>
        /// The optional rdpCorrelationInfo field of the 224 Connection Request PDU (section 2.2.1.1) is present.
        /// </summary>
        CORRELATION_INFO_PRESENT = 0x08
    }

    /// <summary>
    /// The type of length.
    /// </summary>
    public enum length_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0008,
    }

    /// <summary>
    /// The type of requestedProtocols.
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum requestedProtocols_Values : uint
    {

        /// <summary>
        ///  Legacy RDP encryption
        /// </summary>
        PROTOCOL_RDP_FLAG = 0x00000000,

        /// <summary>
        ///  TLS 1.0
        /// </summary>
        PROTOCOL_SSL_FLAG = 0x00000001,

        /// <summary>
        ///  CredSSP. If this flag is set, then the PROTOCOL_SSL_FLAG
        ///  (0x00000001) SHOULD also be set, as TLS is a subset
        ///  of CredSSP.
        /// </summary>
        PROTOCOL_HYBRID_FLAG = 0x00000002,

        /// <summary>
        /// RDSTLS protocol (section 5.4.5.3).
        /// </summary>
        PROTOCOL_RDSTLS = 0x00000004,

        /// <summary>
        /// Credential Security Support Provider protocol (CredSSP) 
        /// coupled with the Early User Authorization Result PDU. 
        /// </summary>
        PROTOCOL_HYBRID_EX = 0x00000008,
    }

    public enum selectedProtocols_Values : uint
    {
        /// <summary>
        ///  Legacy RDP encryption
        /// </summary>
        PROTOCOL_RDP_FLAG = 0x00000000,

        /// <summary>
        ///  TLS 1.0
        /// </summary>
        PROTOCOL_SSL_FLAG = 0x00000001,

        /// <summary>
        ///  CredSSP. If this flag is set, then the PROTOCOL_SSL_FLAG
        ///  (0x00000001) SHOULD also be set, as TLS is a subset
        ///  of CredSSP.
        /// </summary>
        PROTOCOL_HYBRID_FLAG = 0x00000002,

        /// <summary>
        /// RDSTLS protocol (section 5.4.5.3).
        /// </summary>
        PROTOCOL_RDSTLS = 0x00000004,

        /// <summary>
        /// Credential Security Support Provider protocol (CredSSP) 
        /// coupled with the Early User Authorization Result PDU. 
        /// </summary>
        PROTOCOL_HYBRID_EX = 0x00000008,
    }

    /// <summary>
    /// The RDP Correlation Info structure is used by a client to propagate connection correlation information to the server. 
    /// This information allows diagnostic tools on the server to track and monitor a specific connection as it is handled by Terminal Services components.
    /// </summary>
    public class RDP_NEG_CORRELATION_INFO
    {
        /// <summary>
        /// An 8-bit, unsigned integer that indicates the packet type.
        /// This field MUST be set to 0x06 (TYPE_RDP_CORRELATION_INFO).
        /// </summary>
        public RDP_NEG_CORRELATION_INFO_Type type;

        /// <summary>
        /// An 8-bit, unsigned integer that contains protocol flags. 
        /// There are currently no defined flags, so this field MUST be set to 0x00.
        /// </summary>
        public byte flags;

        /// <summary>
        /// A 16-bit, unsigned integer that specifies the packet size. 
        /// This field MUST be set to 0x0024 (36 bytes).
        /// </summary>
        public UInt16 length;

        /// <summary>
        /// An array of sixteen 8-bit, unsigned integers that 
        /// specifies a unique identifier to associate with the connection.
        /// </summary>
        public byte[] correlationId;

        /// <summary>
        /// An array of sixteen 8-bit, unsigned integers reserved for future use. 
        /// All sixteen integers within this array MUST be set to zero.
        /// </summary>
        public byte[] reserved;
    }

    /// <summary>
    /// Type of packet.
    /// </summary>
    public enum RDP_NEG_CORRELATION_INFO_Type : byte
    {
        /// <summary>
        /// This field MUST be set to 0x06 (TYPE_RDP_CORRELATION_INFO).
        /// </summary>
        TYPE_RDP_CORRELATION_INFO = 0x06,
    }


    /// <summary>
    ///  The Server Control PDU includes Cooperate and Granted Control.
    /// </summary>
    public class Server_Control_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Control PDU as described
        ///  in section . The grantId and controlId fields of the
        ///  Control PDU Data MUST both be set to zero, while the
        ///  action field MUST be set to CTRLACTION_COOPERATE (0x0004).
        /// </summary>
        public TS_CONTROL_PDU controlPduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public Server_Control_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_Control_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] dataBuffer = EncodeControlData(controlPduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, dataBuffer);

            return encodedBytes;
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Server_Control_Pdu cloneControlCoopPdu = new Server_Control_Pdu();

            cloneControlCoopPdu.commonHeader = commonHeader.Clone();
            cloneControlCoopPdu.controlPduData = controlPduData;

            return cloneControlCoopPdu;
        }

        /// <summary>
        /// Encode controlPduData field.
        /// </summary>
        /// <param name="controlPduData">The data to be encoded.</param>
        /// <returns>The encoded data.</returns>
        private static byte[] EncodeControlData(TS_CONTROL_PDU controlPduData)
        {
            List<byte> dataBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(dataBuffer, controlPduData.shareDataHeader);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, (ushort)controlPduData.action);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, controlPduData.grantId);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, controlPduData.controlId);

            return dataBuffer.ToArray();
        }
    }


    /// <summary>
    ///  The Server Control PDU (Cooperate) is a Standard
    ///  RDP Connection Sequence PDU sent from server to client
    ///  during the Connection Finalization phase (see section
    ///  ). It is sent after transmitting the Server Synchronize
    ///  PDU.       
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_20.xml
    //  </remarks>
    public partial class Server_Control_Pdu_Cooperate : Server_Control_Pdu
    {
        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_Control_Pdu_Cooperate()
        {
        }
    }

    /// <summary>
    ///  The Server Control PDU (Granted Control) is a
    ///  Standard RDP Connection Sequence PDU sent from server
    ///  to client during the Connection Finalization phase
    ///  (see section ). It is sent after transmitting the Server
    ///  Control PDU (Cooperate).      
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_21.xml
    //  </remarks>
    public partial class Server_Control_Pdu_Granted_Control : Server_Control_Pdu
    {
    }

    /// <summary>
    ///  The Font Map PDU is a Standard RDP Connection
    ///  Sequence PDU sent from server to client during the
    ///  Connection Finalization phase (see section ). It is
    ///  sent after transmitting the Server Control PDU (Granted
    ///  Control). This PDU is the last in the connection sequence
    ///  and signals to the client that it can start sending
    ///  input PDUs (see section ) to the server.      
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_22.xml
    //  </remarks>
    public partial class Server_Font_Map_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Font Map PDU, as specified
        ///  in section .
        /// </summary>
        public TS_FONT_MAP_PDU fontMapPduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public Server_Font_Map_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_Font_Map_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] dataBuffer = EncodeFontMapData(fontMapPduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, dataBuffer);

            return encodedBytes;
        }

        /// <summary>
        /// Encode controlPduData field.
        /// </summary>
        /// <param name="fontListPduData">The data to be encoded.</param>
        /// <returns>The encoded data.</returns>
        private static byte[] EncodeFontMapData(TS_FONT_MAP_PDU fontMapPduData)
        {
            List<byte> dataBuffer = new List<byte>();


            RdpbcgrEncoder.EncodeStructure(dataBuffer, fontMapPduData.shareDataHeader);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, fontMapPduData.numberEntries);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, fontMapPduData.totalNumEntries);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, fontMapPduData.mapFlags);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, fontMapPduData.entrySize);

            return dataBuffer.ToArray();
        }

        public override StackPacket Clone()
        {
            Server_Font_Map_Pdu cloneControlCoopPdu = new Server_Font_Map_Pdu();

            cloneControlCoopPdu.commonHeader = commonHeader.Clone();
            cloneControlCoopPdu.fontMapPduData = fontMapPduData;

            return cloneControlCoopPdu;
        }
    }

    /// <summary>
    ///  The TS_FONT_MAP_PDU structure contains information
    ///  that is sent to the server for legacy reasons.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_22_1.xml
    //  </remarks>
    public partial struct TS_FONT_MAP_PDU
    {
        /// <summary>
        ///  Share Data Header. The type subfield of the pduType
        ///  field of the Share Control Header MUST be set to PDUTYPE_DATAPDU
        ///  (7). The pduType2 field of the Share Data Header MUST
        ///  be set to PDUTYPE2_FONTMAP (40).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  A 16-bit unsigned integer. The number of fonts. This
        ///  field SHOULD be set to 0.
        /// </summary>
        public ushort numberEntries;

        /// <summary>
        ///  A 16-bit unsigned integer. The total number of fonts.
        ///  This field SHOULD be set to 0.
        /// </summary>
        public ushort totalNumEntries;

        /// <summary>
        ///  A 16-bit unsigned integer. The sequence flags. This
        ///  field SHOULD be set to 0x0003, which is the logical
        ///  OR'ed value of FONTMAP_FIRST (0x0001) and FONTMAP_LAST
        ///  (0x0002).
        /// </summary>
        public ushort mapFlags;

        /// <summary>
        ///  A 16-bit unsigned integer. The entry size. This field
        ///  SHOULD be set to 0x0004 (4 bytes).
        /// </summary>
        public ushort entrySize;
    }

    /// <summary>
    ///  The RDP Negotiation Response structure is used by a
    ///  server to inform the client of the security protocol
    ///  which it has selected to use for the connection.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_2_1.xml
    //  </remarks>
    public partial class RDP_NEG_RSP
    {
        /// <summary>
        ///  An 8-bit unsigned integer. Negotiation packet type.
        ///  This field MUST be set to 0x02 (TYPE_RDP_NEG_RSP) to
        ///  indicate that the packet is a Negotiation Response.
        /// </summary>
        public RDP_NEG_RSP_type_Values type;

        /// <summary>
        ///  An 8-bit unsigned integer. Negotiation packet flags.
        ///  There are currently no defined flags so the field MUST
        ///  be set to 0x00.
        /// </summary>
        public RDP_NEG_RSP_flags_Values flags;

        /// <summary>
        ///  A 16-bit unsigned integer. Indicates the packet size.
        ///  This field MUST be set to 0x0008 (8 bytes)
        /// </summary>
        public RDP_NEG_RSP_length_Values length;

        /// <summary>
        ///  A 32-bit unsigned integer. Field indicating the selected
        ///  security protocol.
        /// </summary>
        public selectedProtocols_Values selectedProtocol;
    }

    /// <summary>
    /// The type of type.
    /// </summary>
    public enum RDP_NEG_RSP_type_Values : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x02,
    }

    /// <summary>
    /// The type of flags.
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum RDP_NEG_RSP_flags_Values : byte
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        None = 0x00,

        /// <summary>
        ///  The server supports Extended Client Data Blocks in the GCC Conference Create Request user data (section 2.2.1.3).
        /// </summary>
        EXTENDED_CLIENT_DATA_SUPPORTED = 0x01,

        /// <summary>
        /// The server supports the Graphics Pipeline Extension Protocol described in [MS-RDPEGFX] sections 1, 2, and 3.
        /// </summary>
        DYNVC_GFX_PROTOCOL_SUPPORTED = 0x02,

        /// <summary>
        /// An unused flag that is reserved for future use.
        /// </summary>
        NEGRSP_FLAG_RESERVED = 0x04,

        /// <summary>
        /// Indicates that the server supports credential-less logon over CredSSP (also known as "restricted admin mode") 
        /// and it is acceptable for the client to send empty credentials in the TSPasswordCreds structure defined in [MS-CSSP] section 2.2.1.2.1.
        /// </summary>
        RESTRICTED_ADMIN_MODE_SUPPORTED = 0x08,

        /// <summary>
        /// Indicates that the server supports credential-less logon over CredSSP with credential redirection (also known as "Remote Credential Guard"). 
        /// The client can send a redirected logon buffer in the TSRemoteGuardCreds structure defined in [MS-CSSP] section 2.2.1.2.3.
        /// </summary>
        REDIRECTED_AUTHENTICATION_MODE_SUPPORTED = 0x10

    }

    /// <summary>
    /// The type of length.
    /// </summary>
    public enum RDP_NEG_RSP_length_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0008,
    }

    /// <summary>
    ///  The RDP Negotiation Failure structure is used by a server
    ///  to inform the client of a failure that has occurred
    ///  while preparing security for the connection.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_2_2.xml
    //  </remarks>
    public partial class RDP_NEG_FAILURE
    {

        /// <summary>
        ///  An 8-bit unsigned integer. Negotiation packet type.
        ///  This field MUST be set to 0x03 (TYPE_RDP_NEG_FAILURE)
        ///  to indicate that the packet is a Negotiation Failure.
        /// </summary>
        public RDP_NEG_FAILURE_type_Values type;

        /// <summary>
        ///  An 8-bit unsigned integer. Negotiation packet flags.
        ///  There are currently no defined flags so the field MUST
        ///  be set to 0x00.
        /// </summary>
        public RDP_NEG_FAILURE_flags_Values flags;

        /// <summary>
        ///  A 16-bit unsigned integer. Indicates the packet size.
        ///  This field MUST be set to 0x0008 (8 bytes).
        /// </summary>
        public RDP_NEG_FAILURE_length_Values length;

        /// <summary>
        ///  A 32-bit unsigned integer. Field containing the failure
        ///  code.
        /// </summary>
        public failureCode_Values failureCode;
    }

    /// <summary>
    /// The type of type.
    /// </summary>
    public enum RDP_NEG_FAILURE_type_Values : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x03,
    }

    /// <summary>
    /// The type of flags.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum RDP_NEG_FAILURE_flags_Values : byte
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x00,
    }

    /// <summary>
    /// The type of length.
    /// </summary>
    public enum RDP_NEG_FAILURE_length_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0008,
    }

    /// <summary>
    /// The type of failureCode.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum failureCode_Values : uint
    {
        /// <summary>
        ///  No failure.
        /// </summary>
        NO_FAILURE = 0x00000000,

        /// <summary>
        ///  The server requires that the client support  Enhanced
        ///  RDP Security with either TLS 1.0 or CredSSP. If only
        ///  CredSSP was requested then the server only supports
        ///  TLS.
        /// </summary>
        SSL_REQUIRED_BY_SERVER = 0x00000001,

        /// <summary>
        ///  The server is configured to only use RDP Standard Security
        ///   and does not support any External Security Protocols.
        /// </summary>
        SSL_NOT_ALLOWED_BY_SERVER = 0x00000002,

        /// <summary>
        ///  The server does not possess a valid server authentication
        ///  certificate and cannot initialize the External Security
        ///  Protocol Provider (see section ).
        /// </summary>
        SSL_CERT_NOT_ON_SERVER = 0x00000003,

        /// <summary>
        ///  The list of requested security protocols is not consistent
        ///  with the current security protocol in effect. This
        ///  error is only possible when the Direct Approach (see
        ///  sections  and ) is used and an External Security Protocol
        ///  is already being used.
        /// </summary>
        INCONSISTENT_FLAGS = 0x00000004,

        /// <summary>
        ///  The server requires that the client support Enhanced
        ///  RDP Security with CredSSP.
        /// </summary>
        HYBRID_REQUIRED_BY_SERVER = 0x00000005,

        /// <summary>
        /// The server requires that the client support Enhanced RDP Security (section 5.4) 
        /// with TLS 1.0, 1.1 or 1.2 (section 5.4.5.1) and certificate-based client authentication.
        /// </summary>
        SSL_WITH_USER_AUTH_REQUIRED_BY_SERVER = 0x00000006,
    }

    /// <summary>
    ///  The TS_UD_HEADER precedes all data blocks in the client
    ///  and server GCC user data.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_3_1.xml
    //  </remarks>
    public partial struct TS_UD_HEADER
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The type of the data block
        ///  that this header precedes.
        /// </summary>
        public TS_UD_HEADER_type_Values type;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  data block, including this header.
        /// </summary>
        public ushort length;
    }

    /// <summary>
    /// The type of type.
    /// </summary>
    public enum TS_UD_HEADER_type_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  The data block that follows contains Client Core Data.
        /// </summary>
        CS_CORE = 0xC001,

        /// <summary>
        ///  The data block that follows contains Client Security
        ///  Data.
        /// </summary>
        CS_SECURITY = 0xC002,

        /// <summary>
        ///  The data block that follows contains Client Network
        ///  Data.
        /// </summary>
        CS_NET = 0xC003,

        /// <summary>
        ///  The data block that follows contains Client Cluster
        ///  Data.
        /// </summary>
        CS_CLUSTER = 0xC004,

        /// <summary>
        ///  The data block that follows contains Client Monitor
        ///  Data.
        /// </summary>
        CS_MONITOR = 0xC005,

        /// <summary>
        ///  The data block that follows contains Client Message Channel Data.
        /// </summary>
        CS_MCS_MSGCHANNEL = 0xC006,

        /// <summary>
        /// The data block that follows contains Client Monitor Extended Data.
        /// </summary>
        CS_MONITOR_EX = 0xC008,

        /// <summary>
        ///  The data block that follows contains Client Multitransport Channel Data.
        /// </summary>
        CS_MULTITRANSPORT = 0xC00A,

        /// <summary>
        ///  The data block that follows contains Server Core Data.
        /// </summary>
        SC_CORE = 0x0C01,

        /// <summary>
        ///  The data block that follows contains Server Security
        ///  Data.
        /// </summary>
        SC_SECURITY = 0x0C02,

        /// <summary>
        ///  The data block that follows contains Server Network
        ///  Data.
        /// </summary>
        SC_NET = 0x0C03,

        /// <summary>
        ///  The data block that follows contains Server Message Channel Data.
        /// </summary>
        SC_MCS_MSGCHANNEL = 0x0C04,

        /// <summary>
        ///  The data block that follows contains Server Multitransport Channel Data.
        /// </summary>
        SC_MULTITRANSPORT = 0x0C08,
    }

    /// <summary>
    /// The requested orientation of the desktop
    /// </summary>
    public enum TS_UD_CS_CORE_desktopOrientation_values : ushort
    {
        /// <summary>
        /// The desktop is not rotated.
        /// </summary>
        ORIENTATION_LANDSCAPE = 0,

        /// <summary>
        /// The desktop is rotated clockwise by 90 degrees.
        /// </summary>
        ORIENTATION_PORTRAIT = 90,

        /// <summary>
        /// The desktop is rotated clockwise by 180 degrees.
        /// </summary>
        ORIENTATION_LANDSCAPE_FLIPPED = 180,

        /// <summary>
        /// The desktop is rotated clockwise by 270 degrees.
        /// </summary>
        ORIENTATION_PORTRAIT_FLIPPED = 270

    }
    /// <summary>
    ///  The TS_UD_CS_CORE data block contains core client connection-related
    ///  information.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_3_2.xml
    //  </remarks>
    public partial class TS_UD_CS_CORE
    {
        /// <summary>
        ///  GCC user data block header, as specified in section
        ///  . The User Data Header type field MUST be set to CS_CORE
        ///  (0xC001).
        /// </summary>
        public TS_UD_HEADER header;

        /// <summary>
        ///  A 32-bit unsigned integer. Client version number for
        ///  the Remote Desktop Protocol (RDP). The major version
        ///  number is stored in the high 2 bytes, while the minor
        ///  version number is stored in the low 2 bytes.
        /// </summary>
        public TS_UD_CS_CORE_version_Values version;

        /// <summary>
        ///  A 16-bit unsigned integer. The requested desktop width
        ///  in pixels (up to a maximum value of 4096 pixels).
        /// </summary>
        public ushort desktopWidth;

        /// <summary>
        ///  A 16-bit unsigned integer. The requested desktop height
        ///  in pixels (up to a maximum value of 2048 pixels).
        /// </summary>
        public ushort desktopHeight;

        /// <summary>
        ///  A 16-bit unsigned integer. The requested color depth.
        ///  This field MUST be set to RNS_UD_COLOR_8BPP (0xCA01)
        ///  for historical reasons.
        /// </summary>
        public colorDepth_Values colorDepth;

        /// <summary>
        ///  A 16-bit unsigned integer. Secure access sequence. This
        ///  field SHOULD be set to RNS_UD_SAS_DEL (0xAA03).
        /// </summary>
        public ushort SASSequence;

        /// <summary>
        ///  A 32-bit unsigned integer. Keyboard layout (active input
        ///  locale identifier). For a list of possible input locales,
        ///  see [MSDN-MUI].
        /// </summary>
        public uint keyboardLayout;

        /// <summary>
        ///  A 32-bit unsigned integer. Build number of the client.
        /// </summary>
        public uint clientBuild;

        /// <summary>
        ///  Name of the client computer. This field contains up
        ///  to 15 Unicode characters plus a null terminator.
        /// </summary>
        //[StaticSize(16, StaticSizeMode.Elements)]
        public string clientName;

        /// <summary>
        ///  A 32-bit unsigned integer. Keyboard type.
        /// </summary>
        public keyboardType_Values keyboardType;

        /// <summary>
        ///  A 32-bit unsigned integer. The keyboard subtype (an
        ///  original equipment manufacturer-dependent value).
        /// </summary>
        public uint keyboardSubType;

        /// <summary>
        ///  A 32-bit unsigned integer. The number of function keys
        ///  on the keyboard.
        /// </summary>
        public uint keyboardFunctionKey;

        /// <summary>
        ///  A 64-byte field. The Input Method Editor (IME) file
        ///  name associated with the input locale. This field contains
        ///  up to 31 Unicode characters plus a null terminator.
        /// </summary>
        public string imeFileName;

        /// <summary>
        ///  A 16-bit unsigned integer. The requested color depth
        ///  examined by RDP 4.0 and 5.0 version servers.
        /// </summary>
        public UInt16Class postBeta2ColorDepth;

        /// <summary>
        ///  A 16-bit unsigned integer. The client product ID. This
        ///  field SHOULD be initialized to 1.
        /// </summary>
        public UInt16Class clientProductId;

        /// <summary>
        ///  A 32-bit unsigned integer. Serial number. This field
        ///  SHOULD be initialized to 0. If this field is not present,
        ///  then none of the subsequent fields MUST be present.
        /// </summary>
        public UInt32Class serialNumber;

        /// <summary>
        ///  A 16-bit unsigned integer. The requested color depth
        ///  examined by RDP 5.1, 5.2, and 6.0 servers.
        /// </summary>
        public UInt16Class highColorDepth;

        /// <summary>
        ///  A 16-bit unsigned integer. Specifies the high color
        ///  depths that the client is capable of supporting (examined
        ///  by RDP 5.1 and later servers).
        /// </summary>
        public UInt16Class supportedColorDepths;

        /// <summary>
        ///  A 16-bit unsigned integer. It specifies capabilities
        ///  early in the connection sequence.
        /// </summary>
        public UInt16Class earlyCapabilityFlags;

        /// <summary>
        ///  Contains a value which uniquely identifies the client.
        ///  If this field is present, then all of the preceding
        ///  fields MUST also be present. If this field is not present,
        ///  then none of the subsequent fields MUST be present.
        /// </summary>
        public string clientDigProductId;

        /// <summary>
        /// An 8-bit unsigned integer. Hints at the type of network connection 
        /// being used by the client. This field only contains valid data if 
        /// the RNS_UD_CS_VALID_CONNECTION_TYPE (0x0020) flag is present in the 
        /// earlyCapabilityFlags field.
        /// </summary>
        public ByteClass connnectionType;

        /// <summary>
        ///  An 8-bit, unsigned integer. Padding to align the serverSelectedProtocol
        ///  field on the correct byte boundary. If this field is present, 
        ///  then all of the preceding fields MUST also be present. 
        ///  If this field is not present, then none of the subsequent fields MUST be present.
        /// </summary>
        public ByteClass pad1octets;

        /// <summary>
        ///  A 32-bit unsigned integer. It contains the value returned
        ///  by the server in the selectedProtocol field of the
        ///  RDP Negotiation Response structure. In the event that
        ///  an RDP Negotiation Response structure was not sent,
        ///  this field MUST be initialized to PROTOCOL_RDP (0).
        ///  If this field is present, then all of the preceding
        ///  fields MUST also be present.
        /// </summary>
        public UInt32Class serverSelectedProtocol;

        /// <summary>
        /// The requested physical width of the desktop, in millimeters (mm). 
        /// </summary>
        public UInt32Class desktopPhysicalWidth;

        /// <summary>
        /// The requested physical height of the desktop, in millimeters.
        /// </summary>
        public UInt32Class desktopPhysicalHeight;

        /// <summary>
        /// A 16-bit, unsigned integer. The requested orientation of the desktop, in degrees. 
        /// </summary>
        public TS_UD_CS_CORE_desktopOrientation_values desktopOrientation;

        /// <summary>
        /// A 32-bit, unsigned integer. The requested desktop scale factor.
        /// </summary>
        public UInt32Class desktopScaleFactor;

        /// <summary>
        /// A 32-bit, unsigned integer. The requested device scale factor.
        /// </summary>
        public UInt32Class deviceScaleFactor;
    }

    /// <summary>
    /// The type of connnectionType.
    /// </summary>
    public enum ConnnectionType : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Modem (56 Kbps)
        /// </summary>
        CONNECTION_TYPE_MODEM = 1,

        /// <summary>
        /// Low-speed broadband (256 Kbps - 2 Mbps)
        /// </summary>
        CONNECTION_TYPE_BROADBAND_LOW = 2,

        /// <summary>
        /// Satellite (2 Mbps - 16 Mbps with high latency)
        /// </summary>
        CONNECTION_TYPE_SATELLITE = 3,

        /// <summary>
        /// High-speed broadband (2 Mbps - 10 Mbps)
        /// </summary>
        CONNECTION_TYPE_BROADBAND_HIGH = 4,

        /// <summary>
        /// WAN (10 Mbps or higher with high latency)
        /// </summary>
        CONNECTION_TYPE_WAN = 5,

        /// <summary>
        /// LAN (10 Mbps or higher)
        /// </summary>
        CONNECTION_TYPE_LAN = 6,

        /// <summary>
        /// The server SHOULD attempt to detect the connection type. 
        /// </summary>
        CONNECTION_TYPE_AUTODETECT = 7,
    }

    /// <summary>
    /// The type of version.
    /// </summary>
    public enum TS_UD_CS_CORE_version_Values : uint
    {
        /// <summary>
        ///  RDP 4.0 clients
        /// </summary>
        V1 = 0x00080001,

        /// <summary>
        ///  RDP 5.0, 5.1, 5.2, 6.0, 6.1, 7.0, 7.1, 8.0, and 8.1 clients
        /// </summary>
        V2 = 0x00080004,

        /// <summary>
        ///  RDP 10.0 clients
        /// </summary>
        V3 = 0x00080005,

        /// <summary>
        ///  RDP 10.1 clients
        /// </summary>
        V4 = 0x00080006,

        /// <summary>
        ///  RDP 10.2 clients
        /// </summary>
        V5 = 0x00080007,

        /// <summary>
        ///  RDP 10.3 clients
        /// </summary>
        V6 = 0x00080008,

        /// <summary>
        ///  RDP 10.4 clients
        /// </summary>
        V7 = 0x00080009,

        /// <summary>
        ///  RDP 10.5 clients
        /// </summary>
        V8 = 0x0008000A,

        /// <summary>
        ///  RDP 10.6 clients
        /// </summary>
        V9 = 0x0008000B,

        /// <summary>
        /// RDP 10.7 clients
        /// </summary>
        V10 = 0x0008000C,

        /// <summary>
        /// RDP 10.8 clients
        /// </summary>
        V11 = 0x0008000D
    }

    /// <summary>
    /// The type of colorDepth.
    /// </summary>
    public enum colorDepth_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  8 bits-per-pixel
        /// </summary>
        RNS_UD_COLOR_8BPP = 0xCA01,
    }

    /// <summary>
    /// The type of keyboardType.
    /// </summary>
    public enum keyboardType_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  IBM PC/XT or compatible (83-key) keyboard
        /// </summary>
        V1 = 1,

        /// <summary>
        ///  Olivetti "ICO" (102-key) keyboard
        /// </summary>
        V2 = 2,

        /// <summary>
        ///  IBM PC/AT (84-key) and similar keyboards
        /// </summary>
        V3 = 3,

        /// <summary>
        ///  IBM enhanced (101- or 102-key) keyboard
        /// </summary>
        V4 = 4,

        /// <summary>
        ///  Nokia 1050 and similar keyboards
        /// </summary>
        V5 = 5,

        /// <summary>
        ///  Nokia 9140 and similar keyboards
        /// </summary>
        V6 = 6,

        /// <summary>
        ///  Japanese keyboard
        /// </summary>
        V7 = 7,
    }

    /// <summary>
    /// The type of postBeta2ColorDepth.
    /// </summary>
    public enum postBeta2ColorDepth_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  4 bits-per-pixel
        /// </summary>
        RNS_UD_COLOR_4BPP = 0xCA00,

        /// <summary>
        ///  8 bits-per-pixel
        /// </summary>
        RNS_UD_COLOR_8BPP = 0xCA01,

        /// <summary>
        ///  15-bit 555 red, green, blue (RGB) mask (5 bits for red,
        ///  5 bits for green, and 5 bits for blue)
        /// </summary>
        RNS_UD_COLOR_16BPP_555 = 0xCA02,

        /// <summary>
        ///  16-bit 565 RGB mask (5 bits for red, 6 bits for green,
        ///  and 5 bits for blue)
        /// </summary>
        RNS_UD_COLOR_16BPP_565 = 0xCA03,

        /// <summary>
        ///  24-bit RGB mask (8 bits for red, 8 bits for green, and
        ///  8 bits for blue)
        /// </summary>
        RNS_UD_COLOR_24BPP = 0xCA04,
    }

    /// <summary>
    /// The type of highColorDepth.
    /// </summary>
    public enum highColorDepth_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  4 bits-per-pixel
        /// </summary>
        V1 = 4,

        /// <summary>
        ///  8 bits-per-pixel
        /// </summary>
        V2 = 8,

        /// <summary>
        ///  15-bit 555 red, green, blue (RGB) mask (5 bits for red,
        ///  5 bits for green, and 5 bits for blue)
        /// </summary>
        V3 = 15,

        /// <summary>
        ///  16-bit 565 RGB mask (5 bits for red, 6 bits for green,
        ///  and 5 bits for blue)
        /// </summary>
        V4 = 16,

        /// <summary>
        ///  24-bit RGB mask (8 bits for red, 8 bits for green, and
        ///  8 bits for blue)
        /// </summary>
        V5 = 24,
    }

    /// <summary>
    /// The type of supportedColorDepths.
    /// </summary>
    [Flags()]
    public enum supportedColorDepths_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  24-bit RGB mask (8 bits for red, 8 bits for green, and
        ///  8 bits for blue)
        /// </summary>
        RNS_UD_24BPP_SUPPORT = 0x0001,

        /// <summary>
        ///  16-bit 565 RGB mask (5 bits for red, 6 bits for green,
        ///  and 5 bits for blue)
        /// </summary>
        RNS_UD_16BPP_SUPPORT = 0x0002,

        /// <summary>
        ///  15-bit 555 red, green, blue (RGB) mask (5 bits for red,
        ///  5 bits for green, and 5 bits for blue)
        /// </summary>
        RNS_UD_15BPP_SUPPORT = 0x0004,

        /// <summary>
        ///  32-bit RGB mask (8 bits for the alpha channel, 8 bits
        ///  for red, 8 bits for green, and 8 bits for blue)
        /// </summary>
        RNS_UD_32BPP_SUPPORT = 0x0008,
    }

    /// <summary>
    /// The type of earlyCapabilityFlags.
    /// </summary>
    public enum earlyCapabilityFlags_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Client can support the Set Error Info PDU from the server.
        /// </summary>
        RNS_UD_CS_SUPPORT_ERRINFO_PDU = 0x0001,

        /// <summary>
        ///  Indicates that the client is requesting a session color
        ///  depth of 32 bits-per-pixel. This flag is necessary
        ///  as the highColorDepth field does not support a value
        ///  of 32. If this flag is set, the highColorDepth field
        ///  SHOULD be set to 24 to provide an acceptable fallback
        ///  for the scenario where the server does not support
        ///  32 bpp color.
        /// </summary>
        RNS_UD_CS_WANT_32BPP_SESSION = 0x0002,

        /// <summary>
        /// Indicates that the client supports the Server Status Info PDU.
        /// </summary>
        RNS_UD_CS_SUPPORT_STATUSINFO_PDU = 0x0004,
        /// <summary>
        ///  Indicates that the client supports asymmetric keys larger
        ///  than 512-bits for use with the Server Certificate (see
        ///  Section ) sent in the Server Security Data block (see
        ///  section ).
        /// </summary>
        RNS_UD_CS_STRONG_ASYMMETRIC_KEYS = 0x0008,

        /// <summary>
        /// An unused flag that MUST be ignored by the server.
        /// </summary>
        RNS_UD_CS_UNUSED = 0x0010,

        /// <summary>
        /// Indicates that the connectionType field contains valid data.
        /// </summary>
        RNS_UD_CS_VALID_CONNECTION_TYPE = 0x0020,

        /// <summary>
        /// Indicates that the client supports the Monitor Layout PDU.
        /// </summary>
        RNS_UD_CS_SUPPORT_MONITOR_LAYOUT_PDU = 0x0040,

        /// <summary>
        /// Indicates that the client supports network characteristics detection using the network characteristics detection PDUs described in section 2.2.14.
        /// </summary>
        RNS_UD_CS_SUPPORT_NETWORK_AUTODETECT = 0x0080,

        /// <summary>
        /// Indicates that the client supports the Graphics Pipeline Extension Protocol described in [MS-RDPEGFX] sections 1, 2, and 3.
        /// </summary>
        RNS_UD_CS_SUPPORT_DYNVC_GFX_PROTOCOL = 0x0100,

        /// <summary>
        /// Indicates that the client supports Dynamic DST.
        /// </summary>
        RNS_UD_CS_SUPPORT_DYNAMIC_TIME_ZONE = 0x0200,

        /// <summary>
        /// Indicates that the client supports the Heartbeat PDU (section 2.2.16.1).
        /// </summary>
        RNS_UD_CS_SUPPORT_HEARTBEAT_PDU = 0x0400,
    }

    /// <summary>
    ///  The TS_UD_CS_SEC data block contains security-related
    ///  information used to advertise client cryptographic
    ///  support. This information is only relevant when Standard
    ///  RDP Security is in effect, as opposed to  Enhanced
    ///  RDP Security. See sections  and  for a detailed discussion
    ///  of how this information is used.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_3_3.xml
    //  </remarks>
    public partial class TS_UD_CS_SEC
    {

        /// <summary>
        ///  GCC user data block header as described in User Data
        ///  Header. The User Data Header type field MUST be set
        ///  to CS_SECURITY (0xC002).
        /// </summary>
        public TS_UD_HEADER header;

        /// <summary>
        ///  A 32-bit unsigned integer. Cryptographic methods supported
        ///  by the client and used in conjunction with Standard
        ///  RDP Security (see section ).
        /// </summary>
        public encryptionMethod_Values encryptionMethods;

        /// <summary>
        ///  A 32-bit unsigned integer. This field is used exclusively
        ///  for the French locale. In French locale clients, encryptionMethods
        ///  MUST be set to 0 and extEncryptionMethods MUST be set
        ///  to the value to which encryptionMethods would have
        ///  been set. For non-French locale clients, this field
        ///  MUST be set to 0.
        /// </summary>
        public encryptionMethod_Values extEncryptionMethods;
    }

    /// <summary>
    /// The type of encryptionMethod.
    /// </summary>
    [Flags]
    public enum encryptionMethod_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  40-bit session keys should be used to encrypt data (with
        ///  RC4) and generate message authentication codes (MAC).
        /// </summary>
        _40BIT_ENCRYPTION_FLAG = 0x00000001,

        /// <summary>
        ///  128-bit session keys should be used to encrypt data
        ///  (with RC4) and generate MACs.
        /// </summary>
        _128BIT_ENCRYPTION_FLAG = 0x00000002,

        /// <summary>
        ///  56-bit session keys should be used to encrypt data (with
        ///  RC4) and generate MACs.
        /// </summary>
        _56BIT_ENCRYPTION_FLAG = 0x00000008,

        /// <summary>
        ///  All encryption and message authentication code generation
        ///  routines should be FIPS 140-1 compliant.
        /// </summary>
        FIPS_ENCRYPTION_FLAG = 0x00000010,
    }

    /// <summary>
    ///  The TS_UD_CS_NET packet contains a list of requested
    ///  virtual channels.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_3_4.xml
    //  </remarks>
    public partial class TS_UD_CS_NET
    {

        /// <summary>
        ///  A 32-bit unsigned integer. GCC user data block header,
        ///  as specified in User Data Header. The User Data Header
        ///  type field MUST be set to CS_NET (0xC003).
        /// </summary>
        public TS_UD_HEADER header;

        /// <summary>
        ///  A 32-bit unsigned integer. The number of requested static
        ///  virtual channels (the maximum allowed is 30).
        /// </summary>
        public uint channelCount;

        /// <summary>
        ///  A variable length array containing the information for
        ///  requested static virtual channels encapsulated in CHANNEL_DEF
        ///  structures. The number of CHANNEL_DEF structures which
        ///  follows is given by the channelCount field.
        /// </summary>
        public List<CHANNEL_DEF> channelDefArray;
    }

    /// <summary>
    ///  The CHANNEL_DEF packet contains information for a particular
    ///  static virtual channel.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_3_4_1.xml
    //  </remarks>
    public partial struct CHANNEL_DEF
    {
        /// <summary>
        ///  An 8-byte array containing a unique 7-character ANSI
        ///  channel name and a null terminator.
        /// </summary>
        public string name;

        /// <summary>
        ///  A 32-bit unsigned integer. Channel option flags.
        /// </summary>
        public Channel_Options options;
    }

    /// <summary>
    /// The type of options.
    /// </summary>
    public enum Channel_Options : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Absence of this flag indicates that this channel is
        ///  a placeholder and that the server should not actually
        ///  set it up.
        /// </summary>
        INITIALIZED = 0x80000000,

        /// <summary>
        ///  Channel data should be encrypted in the same way as
        ///  RDP input and output data.
        /// </summary>
        ENCRYPT_RDP = 0x40000000,

        /// <summary>
        ///  Server-to-client data should be encrypted. This flag
        ///  is ignored if ENCRYPT_RDP is set.
        /// </summary>
        ENCRYPT_SC = 0x20000000,

        /// <summary>
        ///  Client-to-server data should be encrypted. This flag
        ///  is ignored if ENCRYPT_RDP is set.
        /// </summary>
        ENCRYPT_CS = 0x10000000,

        /// <summary>
        ///  Channel data should be sent with high MCS priority.
        /// </summary>
        PRI_HIGH = 0x08000000,

        /// <summary>
        ///  Channel data should be sent with medium MCS priority.
        /// </summary>
        PRI_MED = 0x04000000,

        /// <summary>
        ///  Channel data should be sent with low MCS priority.
        /// </summary>
        PRI_LOW = 0x02000000,

        /// <summary>
        ///  Virtual channel data should be compressed if RDP data
        ///  is being compressed.
        /// </summary>
        COMPRESS_RDP = 0x00800000,

        /// <summary>
        ///  Virtual channel data should be compressed, regardless
        ///  of RDP compression.
        /// </summary>
        COMPRESS = 0x00400000,

        /// <summary>
        ///  Server virtual channel add-ins should be shown the full
        ///  virtual channel packet header on receipt of data. If
        ///  this option is not set, the server add-ins receive
        ///  only the data stream without headers.
        /// </summary>
        SHOW_PROTOCOL = 0x00200000,

        /// <summary>
        ///  Channel is persistent across remote control transactions.
        /// </summary>
        REMOTE_CONTROL_PERSISTENT = 0x00100000,
    }

    /// <summary>
    ///  The TS_UD_CS_CLUSTER data block is sent by the client
    ///  to the server either to advertise that it can support
    ///  the Server Redirection PDU (see [MS_RDPEGDI] section
    ///  2.2.3.1) or to request a connection to a given session
    ///  identifier.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_3_5.xml
    //  </remarks>
    public partial class TS_UD_CS_CLUSTER
    {

        /// <summary>
        ///  GCC user data block header, as specified in User Data
        ///  Header. The User Data Header type field MUST be set
        ///  to CS_CLUSTER (0xC004).
        /// </summary>
        public TS_UD_HEADER header;

        /// <summary>
        ///  A 32-bit unsigned integer. Cluster information flags.
        /// </summary>
        public Flags_Values Flags;

        /// <summary>
        ///  A 32-bit unsigned integer. If the REDIRECTED_SESSIONID_FIELD_VALID
        ///  flag is set in the Flags field, then the RedirectedSessionID
        ///  field contains a valid session identifier to which
        ///  the client wants to connect.
        /// </summary>
        public uint RedirectedSessionID;
    }

    /// <summary>
    /// The type of Flags.
    /// </summary>
    public enum Flags_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  The client can receive server session redirection packets.
        ///  If this flag is set, the ServerSessionRedirectionVersionMask
        ///  will contain the server session redirection version
        ///  that the client supports.
        /// </summary>
        REDIRECTION_SUPPORTED = 0x00000001,

        /// <summary>
        ///  The server session redirection version that the client
        ///  supports. See the discussion which follows this table
        ///  for more information.
        /// </summary>
        ServerSessionRedirectionVersionMask = 0x0000003C,

        /// <summary>
        ///  The RedirectedSessionID field contains a valid session
        ///  identifier to which the client wants to connect.
        /// </summary>
        REDIRECTED_SESSIONID_FIELD_VALID = 0x00000002,

        /// <summary>
        ///  The client logged on with a smart card.
        /// </summary>
        REDIRECTED_SMARTCARD = 0x00000040,

        /// <summary>
        ///  If REDIRECTION_SUPPORTED is set, server session redirection
        ///  version 3 is supported by the client.
        /// </summary>
        REDIRECTION_VERSION3 = 0x08,

        /// <summary>
        ///  If REDIRECTION_SUPPORTED is set, server session redirection
        ///  version 4 is supported by the client.
        /// </summary>
        REDIRECTION_VERSION4 = 0x0C,
    }

    /// <summary>
    ///  The TS_UD_CS_MONITOR packet describes the client-side display
    ///  monitor layout. This packet is an extended client data block
    ///  and MUST NOT be sent to a server which does not advertise support
    ///  for extended client data blocks by using the EXTENDED_CLIENT_DATA_SUPPORTED
    ///  flag (0x00000001) as described in section 2.2.1.2.1.
    /// </summary>
    public partial class TS_UD_CS_MONITOR
    {
        /// <summary>
        ///  A 32-bit, unsigned integer. GCC user data block header, 
        ///  as specified in User Data Header (section 2.2.1.3.1). 
        ///  The User Data Header type field MUST be set to CS_MONITOR (0xC005).
        /// </summary>
        public TS_UD_HEADER header;

        /// <summary>
        ///  A 32-bit, unsigned integer. 
        ///  This field is unused and reserved for future use.
        /// </summary>
        public uint Flags;

        /// <summary>
        ///  A 32-bit, unsigned integer. The number of display monitor
        ///  definitions in the monitorDefArray field (the maximum allowed is 10).
        /// </summary>
        public uint monitorCount;

        /// <summary>
        ///  A variable-length array containing a series of TS_MONITOR_DEF 
        ///  structures (section 2.2.1.3.6.1) which describe the display monitor 
        ///  layout of the client. The number of TS_MONITOR_DEF structures is 
        ///  given by the monitorCount field.
        /// </summary>
        public Collection<TS_MONITOR_DEF> monitorDefArray;
    }

    /// <summary>
    /// The TS_UD_CS_MCS_MSGCHANNEL packet indicates support for the message channel 
    /// which is used to transport the Initiate Multitransport Request PDU (section 2.2.15.1). 
    /// This packet is an Extended Client Data Block and MUST NOT be sent to a server 
    /// which does not advertise support for Extended Client Data Blocks by using 
    /// the EXTENDED_CLIENT_DATA_SUPPORTED flag (0x00000001) as described in section 2.2.1.2.1.
    /// </summary>
    public partial class TS_UD_CS_MCS_MSGCHANNEL
    {
        /// <summary>
        /// A GCC user data block header, as specified in User Data Header (section 2.2.1.3.1). 
        /// The User Data Header type field MUST be set to CS_MCS_MSGCHANNEL (0xC006).
        /// </summary>
        public TS_UD_HEADER header;

        /// <summary>
        /// A 32-bit, unsigned integer. This field is unused and reserved for future use. 
        /// It MUST be set to zero.
        /// </summary>
        public FLAGS_TS_UD_CS_MCS_MSGCHANNEL flags;
    }

    /// <summary>
    /// Flags of TS_UD_CS_MCS_MSGCHANNEL
    /// </summary>
    public enum FLAGS_TS_UD_CS_MCS_MSGCHANNEL : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,
    }

    /// <summary>
    /// The TS_UD_CS_MULTITRANSPORT packet is used to indicate support for the RDP Multitransport Layer 
    /// ([MS-RDPEMT] section 1.3) and to specify multitransport characteristics. 
    /// This packet is an Extended Client Data Block and MUST NOT be sent to a server which does not advertise support 
    /// for Extended Client Data Blocks by using the EXTENDED_CLIENT_DATA_SUPPORTED flag (0x00000001) as described in section 2.2.1.2.1.
    /// </summary>
    public partial class TS_UD_CS_MULTITRANSPORT
    {
        /// <summary>
        /// A GCC user data block header, as specified in User Data Header (section 2.2.1.3.1). 
        /// The User Data Header type field MUST be set to CS_MULTITRANSPORT (0xC00A).
        /// </summary>
        public TS_UD_HEADER header;

        /// <summary>
        /// A 32-bit, unsigned integer that specifies protocols supported by the client-side multitransport layer.
        /// </summary>
        public MULTITRANSPORT_TYPE_FLAGS flags;

    }

    /// <summary>
    /// The orientation of the monitor
    /// </summary>
    public enum TS_MONITOR_ATTRIBUTES_orientation_values : uint
    {
        /// <summary>
        /// The desktop is not rotated.
        /// </summary>
        ORIENTATION_LANDSCAPE = 0,

        /// <summary>
        /// The desktop is rotated clockwise by 90 degrees.
        /// </summary>
        ORIENTATION_PORTRAIT = 90,

        /// <summary>
        /// The desktop is rotated clockwise by 180 degrees.
        /// </summary>
        ORIENTATION_LANDSCAPE_FLIPPED = 180,

        /// <summary>
        /// The desktop is rotated clockwise by 270 degrees.
        /// </summary>
        ORIENTATION_PORTRAIT_FLIPPED = 270

    }

    /// <summary>
    /// The TS_MONITOR_ATTRIBUTES packet describes extended attributes of a client-side display monitor.
    /// </summary>
    public struct TS_MONITOR_ATTRIBUTES
    {
        /// <summary>
        /// A 32-bit, unsigned integer. The physical width of the monitor, in millimeters (mm). 
        /// This value MUST be ignored if it is less than 10 mm or greater than 10,000 mm or physicalHeight is less than 10 mm or greater than 10,000 mm.
        /// </summary>
        public uint physicalWidth;

        /// <summary>
        /// A 32-bit, unsigned integer. The physical height of the monitor, in millimeters. 
        /// This value MUST be ignored if it is less than 10 mm or greater than 10,000 mm or physicalWidth is less than 10 mm or greater than 10,000 mm.
        /// </summary>
        public uint physicalHeight;

        /// <summary>
        /// A 32-bit, unsigned integer. The orientation of the monitor, in degrees. 
        /// This value MUST be ignored if it is invalid
        /// </summary>
        public TS_MONITOR_ATTRIBUTES_orientation_values orientation;

        /// <summary>
        /// A 32-bit, unsigned integer. The desktop scale factor of the monitor. 
        /// This value MUST be ignored if it is less than 100% or greater than 500% or deviceScaleFactor is not 100%, 140% or 180%
        /// </summary>
        public uint desktopScaleFactor;

        /// <summary>
        /// A 32-bit, unsigned integer. The device scale factor of the monitor. 
        /// This value MUST be ignored if it is not set to 100%, 140% or 180% or desktopScaleFactor is less than 100% or greater than 500%.
        /// </summary>
        public uint deviceScaleFactor;

        public TS_MONITOR_ATTRIBUTES Clone()
        {
            TS_MONITOR_ATTRIBUTES attribute = new TS_MONITOR_ATTRIBUTES();

            attribute.physicalWidth = physicalWidth;
            attribute.physicalHeight = physicalHeight;
            attribute.orientation = orientation;
            attribute.desktopScaleFactor = desktopScaleFactor;
            attribute.deviceScaleFactor = deviceScaleFactor;

            return attribute;
        }
    }

    /// <summary>
    /// The TS_UD_CS_MONITOR_EX packet describes extended attributes of the client-side display monitor layout defined by the Client Monitor Data block (section 2.2.1.3.6). 
    /// This packet is an Extended Client Data Block and MUST NOT be sent to a server 
    /// which does not advertise support for Extended Client Data Blocks by using the EXTENDED_CLIENT_DATA_SUPPORTED flag (0x00000001) as described in section 2.2.1.2.1.
    /// </summary>
    public partial class TS_UD_CS_MONITOR_EX
    {
        /// <summary>
        /// A GCC user data block header, as specified in User Data Header (section 2.2.1.3.1). 
        /// The User Data Header type field MUST be set to CS_MONITOR_EX (0xC008).
        /// </summary>
        public TS_UD_HEADER header;

        /// <summary>
        /// A 32-bit, unsigned integer. This field is unused and reserved for future use. It MUST be set to zero.
        /// </summary>
        public uint flags;

        /// <summary>
        /// A 32-bit, unsigned integer. The size, in bytes, of a single element in the monitorAttributesArray field. 
        /// This field MUST be set to 20 bytes, which is the size of the Monitor Attributes structure (section 2.2.1.3.9.1).
        /// </summary>
        public uint monitorAttributeSize;

        /// <summary>
        /// A 32-bit, unsigned integer. The number of elements in the monitorAttributesArray field. 
        /// This value MUST be the same as the monitorCount field specified in the Client Monitor Data block.
        /// </summary>
        public uint monitorCount;

        /// <summary>
        /// A variable-length array containing a series of Monitor Attribute structures (section 2.2.1.3.9.1) 
        /// which describe extended attributes of each display monitor specified in the Client Monitor Data block. 
        /// The number of Monitor Attribute structures is specified by the monitorCount field.
        /// </summary>
        public TS_MONITOR_ATTRIBUTES[] monitorAttributesArray;
    }

    /// <summary>
    ///  The TS_MONITOR_DEF packet describes the configuration of a client-side 
    ///  display monitor. The x and y coordinates used to describe the monitor 
    ///  position MUST be relative to the upper-left corner of the monitor designated 
    ///  as the "primary display monitor" (this implies that the upper-left corner
    ///  of the primary monitor is always (0, 0)).
    /// </summary>
    public partial struct TS_MONITOR_DEF
    {
        /// <summary>
        ///  A 32-bit, unsigned integer. 
        ///  Specifies the x-coordinate of the upper-left corner of the display monitor.
        /// </summary>
        public uint left;

        /// <summary>
        ///  A 32-bit, unsigned integer. 
        ///  Specifies the y-coordinate of the upper-left corner of the display monitor.
        /// </summary>
        public uint top;

        /// <summary>
        ///  A 32-bit, unsigned integer.
        ///  Specifies the x-coordinate of the lower-right corner of the display monitor.
        /// </summary>
        public uint right;

        /// <summary>
        ///  A 32-bit, unsigned integer. 
        ///  Specifies the y-coordinate of the lower-right corner of the display monitor.
        /// </summary>
        public uint bottom;

        /// <summary>
        ///  A 32-bit, unsigned integer. Monitor configuration flags.
        /// </summary>
        public Flags_TS_MONITOR_DEF flags;
    }

    /// <summary>
    /// The type of flags.
    /// </summary>
    public enum Flags_TS_MONITOR_DEF : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  The top, left, right and bottom fields 
        ///  describe the position of the primary monitor.
        /// </summary>
        TS_MONITOR_PRIMARY = 0x00000001,
    }

    /// <summary>
    /// A 32-bit, unsigned integer that specifies capabilities early in the connection sequence.
    /// </summary>
    public enum SC_earlyCapabilityFlags_Values : uint
    {
        /// <summary>
        /// Indicates that the following key combinations are reserved by the server operating system:
        ///	WIN + Z
        ///	WIN + CTRL + TAB
        ///	WIN + C
        ///	WIN + .
        ///	WIN + SHIFT + .
        /// In addition, the monitor boundaries of the remote session are employed by 
        /// the server operating system to trigger user interface elements via touch or mouse gestures.
        /// </summary>
        RNS_UD_SC_EDGE_ACTIONS_SUPPORTED = 0x00000001,

        /// <summary>
        /// Indicates that the server supports Dynamic DST. Dynamic DST information is provided by the client 
        /// in the cbDynamicDSTTimeZoneKeyName, dynamicDSTTimeZoneKeyName and dynamicDaylightTimeDisabled fields of the Extended Info Packet
        /// </summary>
        RNS_UD_SC_DYNAMIC_DST_SUPPORTED = 0x00000002,

        /// <summary>
        /// Indicates that the following key combinations are reserved by the server operating system:
        /// WIN + Z
        /// WIN + TAB
        /// WIN + A
        /// In addition, the monitor boundaries of the remote session are employed by the server operating system to trigger user interface elements via touch.
        /// </summary>
        RNS_UD_SC_EDGE_ACTIONS_SUPPORTED_V2 = 0x00000004,

    }

    /// <summary>
    ///  The TS_UD_SC_CORE data block contains core server connection-related
    ///  information.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_4_2.xml
    //  </remarks>
    public partial class TS_UD_SC_CORE
    {

        /// <summary>
        ///  GCC user data block header, as specified in User Data
        ///  Header. The User Data Header  type field MUST be set
        ///  to SC_CORE (0x0C01).
        /// </summary>
        public TS_UD_HEADER header;

        /// <summary>
        ///  A 32-bit unsigned integer. The server version number
        ///  for the Remote Desktop Protocol (RDP). The major version
        ///  number is stored in the high 2 bytes, while the minor
        ///  version number is stored in the low 2 bytes.
        /// </summary>
        public TS_UD_SC_CORE_version_Values version;

        /// <summary>
        ///  A 32-bit unsigned integer. Contains the flags sent by
        ///  the client in the requestedProtocols field of the RDP
        ///  Negotiation Request structure. In the event that an
        ///  RDP Negotiation Request structure was not sent, this
        ///  field MUST be initialized to PROTOCOL_RDP (0).
        /// </summary>
        public requestedProtocols_Values clientRequestedProtocols;

        /// <summary>
        /// A 32-bit, unsigned integer that specifies capabilities early in the connection sequence.
        /// If this field is present, all of the preceding fields MUST also be present.
        /// </summary>
        public SC_earlyCapabilityFlags_Values earlyCapabilityFlags;
    }

    /// <summary>
    /// The type of version.
    /// </summary>
    public enum TS_UD_SC_CORE_version_Values : uint
    {
        /// <summary>
        ///  RDP 4.0 servers.
        /// </summary>
        V1 = 0x00080001,

        /// <summary>
        ///  RDP 5.0, 5.1, 5.2 and 6.0 servers.
        /// </summary>
        V2 = 0x00080004,

        /// <summary>
        /// RDP 10.0 servers
        /// </summary>
        V3 = 0x00080005,

        /// <summary>
        /// RDP 10.1 servers
        /// </summary>
        V4 = 0x00080006,

        /// <summary>
        /// RDP 10.2 servers
        /// </summary>
        V5 = 0x00080007,

        /// <summary>
        /// RDP 10.3 servers
        /// </summary>
        V6 = 0x00080008,

        /// <summary>
        /// RDP 10.4 servers
        /// </summary>
        V7 = 0x00080009,

        /// <summary>
        /// RDP 10.5 servers
        /// </summary>
        V8 = 0x0008000A,

        /// <summary>
        /// RDP 10.6 servers
        /// </summary>
        V9 = 0x0008000B,

        /// <summary>
        /// RDP 10.7 servers
        /// </summary>
        V10 = 0x0008000C,

        /// <summary>
        /// RDP 10.8 servers
        /// </summary>
        V11 = 0x0008000D
    }

    /// <summary>
    ///  The TS_UD_SC_SEC1 data block returns negotiated security-related
    ///  information to the client. See sections  and  for a
    ///  detailed d discussion of how this information is used.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_4_3.xml
    //  </remarks>
    public partial class TS_UD_SC_SEC1
    {

        /// <summary>
        ///  GCC user data block header, as specified in User Data
        ///  Header. The User Data Header type field MUST be set
        ///  to SC_SECURITY (0x0C02).
        /// </summary>
        public TS_UD_HEADER header;

        /// <summary>
        ///  A 32-bit unsigned integer. The selected cryptographic
        ///  method to use for the session. When Enhanced RDP Security
        ///  is being used, this field MUST be set to ENCRYPTION_METHOD_NONE
        ///  (0).
        /// </summary>
        public EncryptionMethods encryptionMethod;

        /// <summary>
        ///  A 32-bit unsigned integer. It describes the encryption
        ///  behavior to use for the session. When Enhanced RDP
        ///  Security is being used, this field MUST be set to ENCRYPTION_LEVEL_NONE
        ///  (0).
        /// </summary>
        public EncryptionLevel encryptionLevel;

        /// <summary>
        ///  A 32-bit unsigned integer. The size in bytes of the
        ///  serverRandom field. This field MUST be set to 32 bytes.
        /// </summary>
        public UInt32Class serverRandomLen;

        /// <summary>
        ///  A 32-bit unsigned integer. The size in bytes of the
        ///  serverCertificate field.
        /// </summary>
        public UInt32Class serverCertLen;

        /// <summary>
        ///  The variable-length server random value used to derive
        ///  session keys (see sections  and ). The length in bytes
        ///  is given by the serverRandomLen field.
        /// </summary>
        public byte[] serverRandom;

        /// <summary>
        ///  The variable-length certificate containing the server's
        ///  public key information. The length in bytes is given
        ///  by the serverCertLen field.
        /// </summary>
        public SERVER_CERTIFICATE serverCertificate;


    }

    /// <summary>
    /// The type of encryptionMethod.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum EncryptionMethods : uint
    {
        /// <summary>
        ///  No encryption and message authentication codes will
        ///  be used.
        /// </summary>
        ENCRYPTION_METHOD_NONE = 0x00000000,

        /// <summary>
        ///  40-bit session keys will be used to encrypt data (with
        ///  RC4) and generate message authentication codes (MAC).
        /// </summary>
        ENCRYPTION_METHOD_40BIT = 0x00000001,

        /// <summary>
        ///  128-bit session keys will be used to encrypt data (with
        ///  RC4) and generate MACs.
        /// </summary>
        ENCRYPTION_METHOD_128BIT = 0x00000002,

        /// <summary>
        ///  56-bit session keys will be used to encrypt data (with
        ///  RC4) and generate MACs.
        /// </summary>
        ENCRYPTION_METHOD_56BIT = 0x00000008,

        /// <summary>
        ///  All encryption and message authentication code generation
        ///  routines will be FIPS 140-1 compliant.
        /// </summary>
        ENCRYPTION_METHOD_FIPS = 0x00000010,
    }

    /// <summary>
    /// The type of encryptionLevel.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum EncryptionLevel : uint
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        ENCRYPTION_LEVEL_NONE = 0x00000000,

        /// <summary>
        ///  Possible value.
        /// </summary>
        ENCRYPTION_LEVEL_LOW = 0x00000001,

        /// <summary>
        ///  Possible value.
        /// </summary>
        ENCRYPTION_LEVEL_CLIENT_COMPATIBLE = 0x00000002,

        /// <summary>
        ///  Possible value.
        /// </summary>
        ENCRYPTION_LEVEL_HIGH = 0x00000003,

        /// <summary>
        ///  Possible value.
        /// </summary>
        ENCRYPTION_LEVEL_FIPS = 0x00000004,
    }

    /// <summary>
    /// The TS_UD_SC_MCS_MSGCHANNEL packet is used to specify the ID of the MCS channel 
    /// which transports the Initiate Multitransport Request PDU (section 2.2.15.1).
    /// </summary>
    public partial class TS_UD_SC_MCS_MSGCHANNEL
    {
        /// <summary>
        ///  GCC user data block header, as specified in User Data
        ///  Header. The User Data Header type field MUST be set
        ///  to SC_MCS_MSGCHANNEL (0x0C04).
        /// </summary>
        public TS_UD_HEADER header;

        /// <summary>
        /// A 16-bit, unsigned integer that specifies the ID of the MCS channel 
        /// which transports the Initiate Multitransport Request PDU (section 2.2.15.1).
        /// </summary>
        public ushort MCSChannelID;
    }

    /// <summary>
    /// The TS_UD_CS_MULTITRANSPORT packet is used to indicate support for the RDP Multitransport Layer
    /// ([MS-RDPEMT] section 1.3) and to specify multitransport characteristics.
    /// </summary>
    public partial class TS_UD_SC_MULTITRANSPORT
    {
        /// <summary>
        /// A GCC user data block header, as specified in User Data Header (section 2.2.1.3.1). 
        /// The User Data Header type field MUST be set to SC_MULTITRANSPORT (0x0C06).
        /// </summary>
        public TS_UD_HEADER header;

        /// <summary>
        /// A 32-bit, unsigned integer that specifies protocols supported 
        /// by the server-side multitransport layer.
        /// </summary>
        public MULTITRANSPORT_TYPE_FLAGS flags;

    }

    /// <summary>
    /// A 32-bit, unsigned integer that specifies protocols supported 
    /// by the server-side multitransport layer.
    /// </summary>
    [Flags]
    public enum MULTITRANSPORT_TYPE_FLAGS : uint
    {
        None = 0,

        /// <summary>
        /// RDP-UDP Forward Error Correction (FEC) 
        /// reliable transport ([MS-RDPEUDP] sections 1 to 3).
        /// </summary>
        TRANSPORTTYPE_UDPFECR = 0x01,

        /// <summary>
        /// RDP-UDP FEC lossy transport ([MS-RDPEUDP] sections 1 to 3).
        /// </summary>
        TRANSPORTTYPE_UDPFECL = 0x04,

        /// <summary>
        /// Indicates that tunneling of static virtual channel 
        /// traffic over UDP is supported.
        /// </summary>
        TRANSPORTTYPE_UDP_PREFERRED = 0x100,

        /// <summary>
        /// Indicates that switching dynamic virtual channels from the TCP to UDP transport is supported.
        /// If the server advertises the SOFTSYNC_TCP_TO_UDP flag, then the server MUST support processing success responses in the Initiate Multitransport Response PDU.
        /// </summary>
        SOFTSYNC_TCP_TO_UDP = 0x200,

    }

    /// <summary>
    ///  The PROPRIETARYSERVERCERTIFICATE structure describes
    ///  a signed certificate containing the server's public
    ///  key and conforming to the structure of a Server Certificate.
    ///  For a detailed description of Proprietary Certificates,
    ///  see section .
    /// </summary>
    public partial struct PROPRIETARYSERVERCERTIFICATE
    {
        /// <summary>
        ///  A 32-bit unsigned integer. The signature algorithm identifier.
        ///  This field MUST be set to SIGNATURE_ALG_RSA (0x00000001).
        /// </summary>
        public dwSigAlgId_Values dwSigAlgId;

        /// <summary>
        ///  A 32-bit unsigned integer. The key algorithm identifier.
        ///  This field MUST be set to KEY_EXCHANGE_ALG_RSA (0x00000001).
        /// </summary>
        public dwKeyAlgId_Values dwKeyAlgId;

        /// <summary>
        ///  A 16-bit unsigned integer. The type of data in the PublicKeyBlob
        ///  field. This field MUST be set to BB_RSA_KEY_BLOB (0x0006).
        /// </summary>
        public wPublicKeyBlobType_Values wPublicKeyBlobType;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  PublicKeyBlob field.
        /// </summary>
        public ushort wPublicKeyBlobLen;

        /// <summary>
        ///  Variable-length server public key bytes, formatted using
        ///  the RSA Public Key structure. The length in bytes is
        ///  given by the wPublicKeyBlobLen field.
        /// </summary>
        public RSA_PUBLIC_KEY PublicKeyBlob;

        /// <summary>
        ///  A 16-bit unsigned integer. The type of data in the SignatureBlob
        ///  field. This field is set to BB_RSA_SIGNATURE_BLOB (0x0008).
        /// </summary>
        public wSignatureBlobType_Values wSignatureBlobType;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  SignatureBlob field.
        /// </summary>
        public ushort wSignatureBlobLen;

        /// <summary>
        ///  Variable-length signature of the certificate created
        ///  with the Terminal Services Signing Key (see sections
        ///   and ). The length in bytes is given by the wSignatureBlobLen
        ///  field.
        /// </summary>
        public byte[] SignatureBlob;
    }

    /// <summary>
    ///  The CertBlob packet encapsulates an X.509 certificate
    /// </summary>
    public partial struct CERT_BLOB
    {
        /// <summary>
        ///  A 32-bit integer. This field specifies the number of bytes in abCert
        /// </summary>
        public int cbCert;

        /// <summary>
        ///  A byte array of length cbCert. This field contains binary 
        ///  data representing a single X.509 certificate.
        /// </summary>
        [Size("cbCert")]
        public byte[] abCert;
    }

    /// <summary>
    ///  The X.509 Certificate Chain packet contains a collection of X.509 certificates.
    /// </summary>
    public partial struct X509_CERTIFICATE_CHAIN
    {
        /// <summary>
        ///  A 32-bit integer. This field specifies the number of 
        ///  CertBlob structures in the CertBlobArray field.
        /// </summary>
        public int NumCertBlobs;

        /// <summary>
        ///  An array of CertBlob structures. If the license server 
        ///  was issued an X.509 certificate chain by the clearing 
        ///  house, this array contains all the certificates from 
        ///  that chain, in root-certificate-first order.
        /// </summary>
        [Size("NumCertBlobs")]
        public CERT_BLOB[] CertBlobArray;

        /// <summary>
        ///  A byte array of the length 8 + 4*NumCertBlobs is appended 
        ///  at the end the packet.
        /// </summary>
        [Size("8 + 4 * NumCertBlobs")]
        public byte[] Padding;
    }

    /// <summary>
    /// The type of dwSigAlgId.
    /// </summary>
    public enum dwSigAlgId_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x00000001,
    }

    /// <summary>
    /// The type of dwKeyAlgId.
    /// </summary>
    public enum dwKeyAlgId_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x00000001,
    }

    /// <summary>
    /// The type of wPublicKeyBlobType.
    /// </summary>
    public enum wPublicKeyBlobType_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0006,
    }

    /// <summary>
    /// The type of wSignatureBlobType.
    /// </summary>
    public enum wSignatureBlobType_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0008,
    }

    /// <summary>
    ///  The structure used to describe a public key in a Proprietary
    ///  Certificate.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_4_3_1_1.xml
    //  </remarks>
    public partial struct RSA_PUBLIC_KEY
    {

        /// <summary>
        ///  A 32-bit unsigned integer. The sentinel value. This
        ///  field MUST be set to 0x31415352 ("RSA1" in ANSI when
        ///  the bytes are arranged in little-endian order).
        /// </summary>
        public magic_Values magic;

        /// <summary>
        ///  A 32-bit unsigned integer. The size in bytes of the
        ///  public key modulus.
        /// </summary>
        public uint keylen;

        /// <summary>
        ///  A 32-bit unsigned integer. The number of bits in the
        ///  public key modulus.
        /// </summary>
        public uint bitlen;

        /// <summary>
        ///  A 32-bit unsigned integer. The maximum number of bytes
        ///  that can be encoded using the public key.
        /// </summary>
        public uint datalen;

        /// <summary>
        ///  A 32-bit unsigned integer. The public exponent of the
        ///  public key.
        /// </summary>
        public uint pubExp;

        /// <summary>
        ///  Variable-length modulus of the public key. The length
        ///  in bytes is given by the keylen field.
        /// </summary>
        public byte[] modulus;
    }

    /// <summary>
    /// The type of magic.
    /// </summary>
    public enum magic_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x31415352,
    }

    /// <summary>
    ///  The SERVER_CERTIFICATE structure describes the generic
    ///  server certificate structure to which all server certificates
    ///  present in the Server Security Data conform.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_4_3_a.xml
    //  </remarks>
    public class SERVER_CERTIFICATE
    {
        /// <summary>
        ///  A 32-bit unsigned integer. The certificate version number.
        /// </summary>
        public SERVER_CERTIFICATE_dwVersion_Values dwVersion;

        /// <summary>
        ///  Certificate data. The format of this certificate data
        ///  is determined by the dwVersion field.
        ///  The type could be X509_CERTIFICATE_CHAIN or PROPRIETARYSERVERCERTIFICATE.
        /// </summary>
        public object certData;
    }

    /// <summary>
    /// The type of dwVersion.
    /// </summary>
    [Flags()]
    public enum SERVER_CERTIFICATE_dwVersion_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  The certificate contained in the certData field is a
        ///  Server Proprietary Certificate (see section ).
        /// </summary>
        CERT_CHAIN_VERSION_1 = 0x00000001,

        /// <summary>
        ///  The certificate contained in the certData field is an
        ///  X.509 Certificate (see section ).
        /// </summary>
        CERT_CHAIN_VERSION_2 = 0x00000002,
    }

    /// <summary>
    ///  The TS_UD_SC_NET data block is a reply to the static
    ///  virtual channel list presented in the Client Network
    ///  Data structure.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_1_4_4.xml
    //  </remarks>
    public partial class TS_UD_SC_NET
    {

        /// <summary>
        ///  A GCC user data block header, as specified in section
        ///  User Data Header. The User Data Header type field MUST
        ///  be set to SC_NET (0x0C03).
        /// </summary>
        public TS_UD_HEADER header;

        /// <summary>
        ///  16-bit unsigned integer. The MCS channel identifier
        ///  which the client should join to receive display data
        ///  and send client input (I/O channel).
        /// </summary>
        public ushort MCSChannelId;

        /// <summary>
        ///  16-bit unsigned integer. The number of 16-bit unsigned
        ///  integer MCS channel IDs in the channelIdArray field.
        /// </summary>
        public ushort channelCount;

        /// <summary>
        ///  Variable-length array of MCS channel IDs (each channel
        ///  ID is a 16-bit unsigned integer) which have been allocated
        ///  (the number is given by the channelCount field). Each
        ///  MCS channel ID corresponds in position to the channels
        ///  requested in the Client Network Data structure. A channel
        ///  value of 0 indicates that the channel was not allocated.
        /// </summary>
        public ushort[] channelIdArray;

        /// <summary>
        ///  16-bit unsigned integer. Optional padding. Values in
        ///  this field are ignored. The size in bytes of the Server
        ///  Network Data structure MUST be a multiple of 4. If
        ///  the channelCount field contains an odd value, then
        ///  the size of the channelIdArray (and by implication
        ///  the entire Server Network Data structure) will not
        ///  be a multiple of 4. In this scenario, the Pad field
        ///  MUST be present and it is used to add an additional
        ///  2 bytes to the size of the Server Network Data structure.
        ///  If the channelCount field contains an even value, then
        ///  the Pad field is not required and MUST not be present.
        /// </summary>
        public byte[] Pad;
    }

    /// <summary>
    ///  The MCS Disconnect Provider Ultimatum PDU is an
    ///  MCS PDU sent as part of the disconnection sequences,
    ///  as specified in section . 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_2_1.xml
    //  </remarks>
    public partial class MCS_Disconnect_Provider_Ultimatum_Pdu : RdpbcgrClientPdu
    {
        /// <summary>
        ///  A TPKT Header, as specified in [T123] section 8.
        /// </summary>
        public TpktHeader tpktHeader;

        /// <summary>
        ///  An X.224 Class 0 Data TPDU, as specified in  [X224]
        ///  section 13.7.
        /// </summary>
        public X224 x224Data;

        /// <summary>
        ///  PER-encoded MCS Disconnect Provider Ultimatum PDU, as
        ///  specified in [T125] (the ASN.1 structure definition
        ///  is given in [T125] section 7, part 4).
        /// </summary>
        public DisconnectProviderUltimatum disconnectProvider;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public MCS_Disconnect_Provider_Ultimatum_Pdu(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public MCS_Disconnect_Provider_Ultimatum_Pdu()
        {
            context = null;
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(totalBuffer, tpktHeader);
            RdpbcgrEncoder.EncodeStructure(totalBuffer, x224Data);

            if (disconnectProvider != null)
            {
                DomainMCSPDU mcsDPum = new DomainMCSPDU
                    (DomainMCSPDU.disconnectProviderUltimatum, disconnectProvider);
                RdpbcgrEncoder.EncodeDomainMcsPdu(totalBuffer, mcsDPum);
            }

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            MCS_Disconnect_Provider_Ultimatum_Pdu cloneMcsUltimatePdu =
               new MCS_Disconnect_Provider_Ultimatum_Pdu(context);
            cloneMcsUltimatePdu.tpktHeader = tpktHeader;
            cloneMcsUltimatePdu.x224Data = x224Data;
            if (disconnectProvider != null)
            {
                cloneMcsUltimatePdu.disconnectProvider = new DisconnectProviderUltimatum(disconnectProvider.reason);
            }

            return cloneMcsUltimatePdu;
        }
    }

    /// <summary>
    ///  The MCS Disconnect Provider Ultimatum PDU is an
    ///  MCS PDU sent as part of the disconnection sequences,
    ///  as specified in section . 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_2_1.xml
    //  </remarks>
    public partial class MCS_Disconnect_Provider_Ultimatum_Server_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  A TPKT Header, as specified in [T123] section 8.
        /// </summary>
        public TpktHeader tpktHeader;

        /// <summary>
        ///  An X.224 Class 0 Data TPDU, as specified in  [X224]
        ///  section 13.7.
        /// </summary>
        public X224 x224Data;

        /// <summary>
        ///  PER-encoded MCS Disconnect Provider Ultimatum PDU, as
        ///  specified in [T125] (the ASN.1 structure definition
        ///  is given in [T125] section 7, part 4).
        /// </summary>
        public DisconnectProviderUltimatum disconnectProvider;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public MCS_Disconnect_Provider_Ultimatum_Server_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public MCS_Disconnect_Provider_Ultimatum_Server_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(totalBuffer, tpktHeader);
            RdpbcgrEncoder.EncodeStructure(totalBuffer, x224Data);

            if (disconnectProvider != null)
            {
                DomainMCSPDU mcsDPum = new DomainMCSPDU
                    (DomainMCSPDU.disconnectProviderUltimatum, disconnectProvider);
                RdpbcgrEncoder.EncodeDomainMcsPdu(totalBuffer, mcsDPum);
            }

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes);


            return encodedBytes;
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            MCS_Disconnect_Provider_Ultimatum_Server_Pdu cloneMcsUltimatePdu =
               new MCS_Disconnect_Provider_Ultimatum_Server_Pdu(serverSessionContext);
            cloneMcsUltimatePdu.tpktHeader = tpktHeader;
            cloneMcsUltimatePdu.x224Data = x224Data;
            if (disconnectProvider != null)
            {
                cloneMcsUltimatePdu.disconnectProvider = new DisconnectProviderUltimatum(disconnectProvider.reason);
            }

            return cloneMcsUltimatePdu;
        }
    }

    /// <summary>
    ///  The Shutdown Request PDU is sent by the client
    ///  as part of the disconnection sequences specified in
    ///  section ; specifically as part of the User-Initiated
    ///  on Client disconnect sequence (see section ).     
    ///  
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_2_2.xml
    //  </remarks>
    public partial class Client_Shutdown_Request_Pdu : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Shutdown Request PDU, as
        ///  specified in section .
        /// </summary>
        public TS_SHUTDOWN_REQ_PDU shutdownRequestPduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Client_Shutdown_Request_Pdu(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Client_Shutdown_Request_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] dataBuffer = EncodeShutdownData(shutdownRequestPduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, context);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Client_Shutdown_Request_Pdu cloneShutdownPdu = new Client_Shutdown_Request_Pdu(context);

            cloneShutdownPdu.commonHeader = commonHeader.Clone();
            cloneShutdownPdu.shutdownRequestPduData = shutdownRequestPduData;

            return cloneShutdownPdu;
        }

        /// <summary>
        /// Encode EncodeShutDownData field.
        /// </summary>
        /// <param name="shutdownRequestPduData">The data to be encoded.</param>
        /// <returns>The encoded data.</returns>
        private static byte[] EncodeShutdownData(TS_SHUTDOWN_REQ_PDU shutdownRequestPduData)
        {
            List<byte> dataBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(dataBuffer, shutdownRequestPduData.shareDataHeader);

            return dataBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_SHUTDOWN_REQ_PDU structure contains the
    ///  contents of the Shutdown Request PDU, which is a Share
    ///  Data Header with no PDU body.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_2_2_1.xml
    //  </remarks>
    public partial struct TS_SHUTDOWN_REQ_PDU
    {
        /// <summary>
        ///  Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share
        ///  Control Header MUST be set to PDUTYPE_DATAPDU (7).
        ///  The pduType2 field of the Share Data Header MUST be
        ///  set to PDUTYPE2_SHUTDOWN_REQUEST (36).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;
    }

    /// <summary>
    ///  The Shutdown Request Denied PDU is sent by the
    ///  server as part of the disconnection sequences specified
    ///  in section ; specifically as part of the "User-Initiated
    ///  on Client" disconnect sequence (see section ).    
    ///   
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_2_3.xml
    //  </remarks>
    public partial class Server_Shutdown_Request_Denied_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Shutdown Request Denied PDU,
        ///  as specified in section .           
        /// </summary>
        public TS_SHUTDOWN_DENIED_PDU shutdownRequestDeniedPduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public Server_Shutdown_Request_Denied_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_Shutdown_Request_Denied_Pdu()
        {
        }

        public override StackPacket Clone()
        {
            Server_Shutdown_Request_Denied_Pdu cloneShutDeniedPdu = new Server_Shutdown_Request_Denied_Pdu();
            cloneShutDeniedPdu.commonHeader = commonHeader.Clone();
            cloneShutDeniedPdu.shutdownRequestDeniedPduData.shareDataHeader = shutdownRequestDeniedPduData.shareDataHeader;

            return cloneShutDeniedPdu;
        }

        public override byte[] ToBytes()
        {
            byte[] dataBuffer = EncodeShutdownDeniedData(shutdownRequestDeniedPduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, dataBuffer);

            return encodedBytes;
        }

        private byte[] EncodeShutdownDeniedData(TS_SHUTDOWN_DENIED_PDU shutdownRequestDeniedPduData)
        {
            List<byte> dataBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(dataBuffer, shutdownRequestDeniedPduData.shareDataHeader);

            return dataBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_SHUTDOWN_DENIED_PDU structure contains the contents
    ///  of the Shutdown Request Denied PDU, which is a Share
    ///  Data Header with no PDU body.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_2_3_1.xml
    //  </remarks>
    public partial struct TS_SHUTDOWN_DENIED_PDU
    {
        /// <summary>
        ///  Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share
        ///  Control Header MUST be set to PDUTYPE_DATAPDU (7).
        ///  The pduType2 field of the Share Data Header MUST be
        ///  set to PDUTYPE2_SHUTDOWN_DENIED (37).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;
    }

    /// <summary>
    ///  The Deactivate All PDU is sent from server
    ///  to client to indicate that the connection will be dropped
    ///  or that a capability renegotiation using a Deactivation-Reactivation
    ///  Sequence (see section ) will occur.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_3_1.xml
    //  </remarks>
    public partial class Server_Deactivate_All_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Deactivate All PDU, as specified
        ///  in section .           
        /// </summary>
        public TS_DEACTIVATE_ALL_PDU deactivateAllPduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public Server_Deactivate_All_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_Deactivate_All_Pdu()
        {
        }

        public override StackPacket Clone()
        {
            Server_Deactivate_All_Pdu cloneDeactiveAllPdu = new Server_Deactivate_All_Pdu();
            cloneDeactiveAllPdu.commonHeader = commonHeader.Clone();
            cloneDeactiveAllPdu.deactivateAllPduData = deactivateAllPduData;
            cloneDeactiveAllPdu.deactivateAllPduData.sourceDescriptor = RdpbcgrUtility.CloneByteArray(
                    deactivateAllPduData.sourceDescriptor);

            return cloneDeactiveAllPdu;
        }

        public override byte[] ToBytes()
        {
            byte[] dataBuffer = EncodeDeactivateAllData(deactivateAllPduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, dataBuffer);

            return encodedBytes;
        }

        private byte[] EncodeDeactivateAllData(TS_DEACTIVATE_ALL_PDU deactivateAllPduData)
        {
            List<byte> dataBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(dataBuffer, deactivateAllPduData.shareControlHeader);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, deactivateAllPduData.shareId);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, deactivateAllPduData.lengthSourceDescriptor);
            RdpbcgrEncoder.EncodeBytes(dataBuffer, deactivateAllPduData.sourceDescriptor);

            return dataBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_DEACTIVATE_ALL_PDU structure is a standard T.128
    ///  Deactivate All PDU (see [T128] section 8.4.1).
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_3_1_1.xml
    //  </remarks>
    public partial struct TS_DEACTIVATE_ALL_PDU
    {
        /// <summary>
        ///  Share Control Header containing information about the
        ///  packet.The type subfield of the pduType field of the
        ///  Share Control Header MUST be set to TS_PDUTYPE_DEACTIVATEALLPDU
        ///  (6).
        /// </summary>
        public TS_SHARECONTROLHEADER shareControlHeader;

        /// <summary>
        ///  A 32-bit unsigned integer. The share identifier for
        ///  the packet (see [T128] section 8.4.2 for more information
        ///  regarding share IDs).
        /// </summary>
        public uint shareId;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  sourceDescriptor field.
        /// </summary>
        public ushort lengthSourceDescriptor;

        /// <summary>
        ///  Variable number of bytes. The source descriptor. This
        ///  field SHOULD be set to 0x00.
        /// </summary>
        public byte[] sourceDescriptor;
    }

    /// <summary>
    ///  The Auto-Reconnect Status PDU contains error
    ///  information after a failed auto-reconnection attempt
    ///  has taken place.      
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_4_1.xml
    //  </remarks>
    public partial class Server_Auto_Reconnect_Status_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Auto-Reconnect Status PDU,
        ///  as specified in section .           
        /// </summary>
        public TS_AUTORECONNECT_STATUS_PDU arcStatusPduData;

        public Server_Auto_Reconnect_Status_Pdu()
        {
        }

        public Server_Auto_Reconnect_Status_Pdu(RdpbcgrServerSessionContext sessionContext)
            : base(sessionContext)
        {
        }

        public override StackPacket Clone()
        {
            Server_Auto_Reconnect_Status_Pdu cloneAutoReconnectPdu = new Server_Auto_Reconnect_Status_Pdu();
            cloneAutoReconnectPdu.commonHeader = commonHeader.Clone();
            cloneAutoReconnectPdu.arcStatusPduData.shareDataHeader = arcStatusPduData.shareDataHeader;
            cloneAutoReconnectPdu.arcStatusPduData.arcStatus = arcStatusPduData.arcStatus;

            return cloneAutoReconnectPdu;
        }

        public override byte[] ToBytes()
        {
            byte[] dataBuffer = EncodeAutoReconnectData(arcStatusPduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, dataBuffer);

            return encodedBytes;
        }

        private byte[] EncodeAutoReconnectData(TS_AUTORECONNECT_STATUS_PDU arcStatusPduData)
        {
            List<byte> dataBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(dataBuffer, arcStatusPduData.shareDataHeader);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, arcStatusPduData.arcStatus);

            return dataBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_AUTORECONNECT_STATUS_PDU structure contains
    ///  the contents of the Auto-Reconnect Status PDU, which
    ///  is a Share Data Header with a status field. 				  
    ///    
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_4_1_1.xml
    //  </remarks>
    public partial struct TS_AUTORECONNECT_STATUS_PDU
    {
        /// <summary>
        ///  Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share
        ///  Control Header MUST be set to PDUTYPE_DATAPDU (7).
        ///  The pduType2 field of the Share Data Header MUST be
        ///  set to PDUTYPE2_ARC_STATUS_PDU (50).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  A 32-bit unsigned integer. Error code describing the
        ///  reason for the auto-reconnect failure. Microsoft RDP
        ///  servers populate this field with an NTSTATUS error
        ///  code (see [ERRTRANS] for information on translating
        ///  NTSTATUS error codes to usable text strings) which
        ///  describes the issue which triggered the error.
        /// </summary>
        public uint arcStatus;
    }

    /// <summary>
    ///  The ARC_SC_PRIVATE_PACKET structure contains server-supplied
    ///  information used to seamlessly re-establish a client
    ///  session connection after network interruption. It is
    ///  sent as part of the Save Session Info PDU logon information
    ///  (see section ). 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_4_2.xml
    //  </remarks>
    public partial class ARC_SC_PRIVATE_PACKET
    {
        /// <summary>
        ///  A 32-bit unsigned integer. The length in bytes of the
        ///  Server Auto-Reconnect packet. This field MUST be set
        ///  to 0x0000001C (28 bytes).
        /// </summary>
        public cbLen_Values cbLen;

        /// <summary>
        ///  A 32-bit unsigned integer. The value representing the
        ///  auto-reconnect version.
        /// </summary>
        public Version_Values Version;

        /// <summary>
        ///  A 32-bit unsigned integer. The session identifier for
        ///  reconnection.
        /// </summary>
        public uint LogonId;

        /// <summary>
        ///  Byte buffer containing a 16-byte random number generated
        ///  as a key for secure session reconnection (see section
        ///  ).
        /// </summary>
        public byte[] ArcRandomBits;
    }

    /// <summary>
    /// The type of cbLen.
    /// </summary>
    public enum cbLen_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0000001C,
    }

    /// <summary>
    /// The type of Version.
    /// </summary>
    public enum Version_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Version 1 of auto-reconnect.
        /// </summary>
        AUTO_RECONNECT_VERSION_1 = 0x00000001,
    }

    /// <summary>
    ///  The ARC_CS_PRIVATE_PACKET structure contains the
    ///  client response cookie used to seamlessly re-establish
    ///  a client session connection after network interruption.
    ///  It is sent as part of the extended information of the
    ///  Client Info PDU. 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_4_3.xml
    //  </remarks>
    public partial class ARC_CS_PRIVATE_PACKET
    {
        /// <summary>
        ///  A 32-bit unsigned integer. The length in bytes of the
        ///  Client Auto-Reconnect Packet. This field MUST be set
        ///  to 0x0000001C (28 bytes).
        /// </summary>
        public ARC_CS_PRIVATE_PACKET_cbLen_Values cbLen;

        /// <summary>
        ///  A 32-bit unsigned integer. The value representing the
        ///  auto-reconnect version.
        /// </summary>
        public ARC_CS_PRIVATE_PACKET_Version_Values Version;

        /// <summary>
        ///  A 32-bit unsigned integer. The session identifier for
        ///  reconnection.
        /// </summary>
        public uint LogonId;

        /// <summary>
        ///  Byte buffer containing a 16-byte verifier value derived
        ///  using cryptographic methods (as specified in section
        ///  ) from the ArcRandomBits field of the Server Auto-Reconnect
        ///  packet.
        /// </summary>
        public byte[] SecurityVerifier;
    }

    /// <summary>
    /// The type of cbLen.
    /// </summary>
    public enum ARC_CS_PRIVATE_PACKET_cbLen_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0000001C,
    }

    /// <summary>
    /// The type of Version.
    /// </summary>
    public enum ARC_CS_PRIVATE_PACKET_Version_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Version 1 of auto-reconnect.
        /// </summary>
        AUTO_RECONNECT_VERSION_1 = 0x00000001,
    }

    /// <summary>
    ///  The Status Info PDU is sent by the server to update the client
    ///  with status information. This PDU is only sent to clients that 
    ///  have indicated that they are capable of status updates using 
    ///  the RNS_UD_CS_SUPPORT_STATUSINFO_PDU flag in the Client Core Data
    ///  (section 2.2.1.3.2).
    /// </summary>
    public partial class Server_Status_Info_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  A Share Data Header containing information about the packet. 
        ///  The type subfield of the pduType field of the Share Control
        ///  Header (section 2.2.8.1.1.1.1) MUST be set to PDUTYPE_DATAPDU
        ///  (7). The pduType2 field of the Share Data Header MUST be set 
        ///  to PDUTYPE2_STATUS_INFO_PDU (54), and the pduSource field MUST 
        ///  be set to 0.
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  A 32-bit, unsigned integer. Status code.
        /// </summary>
        public StatusCode_Values statusCode;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public Server_Status_Info_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_Status_Info_Pdu()
        {
        }

        public override StackPacket Clone()
        {
            Server_Status_Info_Pdu cloneStatusInfoPdu = new Server_Status_Info_Pdu();
            cloneStatusInfoPdu.commonHeader = commonHeader.Clone();
            cloneStatusInfoPdu.shareDataHeader = shareDataHeader;
            cloneStatusInfoPdu.statusCode = statusCode;

            return cloneStatusInfoPdu;
        }

        public override byte[] ToBytes()
        {
            List<byte> buffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(buffer, shareDataHeader);
            RdpbcgrEncoder.EncodeStructure(buffer, (uint)statusCode);

            byte[] dataBuffer = buffer.ToArray();

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, dataBuffer);

            return encodedBytes;
        }
    }

    /// <summary>
    /// The type of statusCode.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum StatusCode_Values : uint
    {
        /// <summary>
        ///  Possible value.
        /// </summary>
        TS_STATUS_NO_STATUS = 0x00000000,

        /// <summary>
        ///  Possible value.
        /// </summary>
        TS_STATUS_FINDING_DESTINATION = 0x00000401,

        /// <summary>
        ///  Possible value.
        /// </summary>
        TS_STATUS_LOADING_DESTINATION = 0x00000402,

        /// <summary>
        ///  Possible value.
        /// </summary>
        TS_STATUS_BRINGING_SESSION_ONLINE = 0x00000403,

        /// <summary>
        ///  Possible value.
        /// </summary>
        TS_STATUS_REDIRECTING_TO_DESTINATION = 0x00000404,

        /// <summary>
        ///  Possible value.
        /// </summary>
        TS_STATUS_VM_LOADING = 0x00000501,

        /// <summary>
        /// The destination virtual machine is being resumed from sleep or hibernation.
        /// </summary>
        TS_STATUS_VM_WAKING = 0x00000502,

        /// <summary>
        /// The destination virtual machine is being booted.
        /// </summary>
        TS_STATUS_VM_BOOTING = 0x00000503,

        /// <summary>
        /// Monitoring of the destination virtual machine is being initiated.
        /// </summary>
        TS_STATUS_VM_STARTING_MONITORING = 0x00000504,

        /// <summary>
        /// Monitoring of the destination virtual machine is being reinitiated.
        /// </summary>
        TS_STATUS_VM_RETRYING_MONITORING = 0x00000505,
    }

    /// <summary>
    ///  The Set Error Info PDU is sent by the server
    ///  when there is a connection or disconnection failure.
    ///  This PDU is only sent to clients which have indicated
    ///  that they are capable of handling error reporting using
    ///  the RNS_UD_CS_SUPPORT_ERRINFO_PDU flag in the Client
    ///  Core Data.       
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/_rfc_ms-rdpbcgr2_1_5_1.xml
    //  </remarks>
    public partial class Server_Set_Error_Info_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Set Error Info PDU, as specified
        ///  in section .           
        /// </summary>
        public TS_SET_ERROR_INFO_PDU errorInfoPduData;


        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public Server_Set_Error_Info_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_Set_Error_Info_Pdu()
        {
        }

        public override StackPacket Clone()
        {
            Server_Set_Error_Info_Pdu cloneSetErrorInfoPdu = new Server_Set_Error_Info_Pdu();
            cloneSetErrorInfoPdu.commonHeader = commonHeader.Clone();
            cloneSetErrorInfoPdu.errorInfoPduData.shareDataHeader = errorInfoPduData.shareDataHeader;
            cloneSetErrorInfoPdu.errorInfoPduData.errorInfo = errorInfoPduData.errorInfo;

            return cloneSetErrorInfoPdu;
        }

        public override byte[] ToBytes()
        {
            byte[] dataBuffer = EncodeErrorInfoData(errorInfoPduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, dataBuffer);

            return encodedBytes;
        }

        private byte[] EncodeErrorInfoData(TS_SET_ERROR_INFO_PDU errorInfoPduData)
        {
            List<byte> dataBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(dataBuffer, errorInfoPduData.shareDataHeader);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, (uint)errorInfoPduData.errorInfo);

            return dataBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_SET_ERROR_INFO_PDU structure contains the contents
    ///  of the Set Error Info PDU, which is a Share Data Header
    ///  with an error value field.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_5_1_1.xml
    //  </remarks>
    public partial struct TS_SET_ERROR_INFO_PDU
    {
        /// <summary>
        ///  Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share
        ///  Control Header MUST be set to PDUTYPE_DATAPDU (7).
        ///  The pduType2 field of the Share Data Header MUST be
        ///  set to PDUTYPE2_SET_ERROR_INFO_PDU (47).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  A 32-bit unsigned integer. Error code.Protocol-independent
        ///  codes:
        /// </summary>
        public errorInfo_Values errorInfo;
    }

    /// <summary>
    /// The type of errorInfo.
    /// </summary>
    public enum errorInfo_Values : uint
    {
        /// <summary>
        /// No error has occurred. This code SHOULD be ignored.
        /// </summary>
        ERRINFO_NONE = 0x00000000,

        /// <summary>
        /// The disconnection was initiated by an administrative tool on the server in another session.
        /// </summary>
        ERRINFO_RPC_INITIATED_DISCONNECT = 0x00000001,

        /// <summary>
        /// The disconnection was due to a forced logoff initiated by an administrative tool on the server in another session.
        /// </summary>
        ERRINFO_RPC_INITIATED_LOGOFF = 0x00000002,

        /// <summary>
        /// The idle session limit timer on the server has elapsed.
        /// </summary>
        ERRINFO_IDLE_TIMEOUT = 0x00000003,

        /// <summary>
        /// The active session limit timer on the server has elapsed.
        /// </summary>
        ERRINFO_LOGON_TIMEOUT = 0x00000004,

        /// <summary>
        /// Another user connected to the server, forcing the disconnection of the current connection.
        /// </summary>
        ERRINFO_DISCONNECTED_BY_OTHERCONNECTION = 0x00000005,

        /// <summary>
        /// The server ran out of available memory resources.
        /// </summary>
        ERRINFO_OUT_OF_MEMORY = 0x00000006,

        /// <summary>
        /// The server denied the connection.
        /// </summary>
        ERRINFO_SERVER_DENIED_CONNECTION = 0x00000007,

        /// <summary>
        /// The user cannot connect to the server due to insufficient access privileges.
        /// </summary>
        ERRINFO_SERVER_INSUFFICIENT_PRIVILEGES = 0x00000009,

        /// <summary>
        /// The server does not accept saved user credentials and requires that the user enter their credentials for each connection.
        /// </summary>
        ERRINFO_SERVER_FRESH_CREDENTIALS_REQUIRED = 0x0000000A,

        /// <summary>
        /// The disconnection was initiated by an administrative tool on the server running in the user's session.
        /// </summary>
        ERRINFO_RPC_INITIATED_DISCONNECT_BYUSER = 0x0000000B,

        /// <summary>
        /// The disconnection was initiated by the user logging off his or her session on the server.
        /// </summary>
        ERRINFO_LOGOFF_BY_USER = 0x0000000C,

        /// <summary>
        /// The display driver in the remote session did not report any status within the time allotted for startup.
        /// </summary>
        ERRINFO_CLOSE_STACK_ON_DRIVER_NOT_READY = 0x0000000F,

        /// <summary>
        /// The DWM process running in the remote session terminated unexpectedly.
        /// </summary>
        ERRINFO_SERVER_DWM_CRASH = 0x00000010,

        /// <summary>
        /// The display driver in the remote session was unable to complete all the tasks required for startup.
        /// </summary>
        ERRINFO_CLOSE_STACK_ON_DRIVER_FAILURE = 0x00000011,

        /// <summary>
        /// The display driver in the remote session started up successfully, but due to internal failures was not usable by the remoting stack.
        /// </summary>
        ERRINFO_CLOSE_STACK_ON_DRIVER_IFACE_FAILURE = 0x00000012,

        /// <summary>
        /// The Winlogon process running in the remote session terminated unexpectedly.
        /// </summary>
        ERRINFO_SERVER_WINLOGON_CRASH = 0x00000017,

        /// <summary>
        /// The CSRSS process running in the remote session terminated unexpectedly.
        /// </summary>
        ERRINFO_SERVER_CSRSS_CRASH = 0x00000018,

        /// <summary>
        /// An internal error has occurred in the Terminal Services licensing component.
        /// </summary>
        ERRINFO_LICENSE_INTERNAL = 0x00000100,

        /// <summary>
        /// A Remote Desktop License Server ([MS-RDPELE] section 1.1) could not be found to provide a license.
        /// </summary>
        ERRINFO_LICENSE_NO_LICENSE_SERVER = 0x00000101,

        /// <summary>
        /// There are no Client Access Licenses ([MS-RDPELE] section 1.1) available for the target remote computer.
        /// </summary>
        ERRINFO_LICENSE_NO_LICENSE = 0x00000102,

        /// <summary>
        /// The remote computer received an invalid licensing message from the client. 
        /// </summary>
        ERRINFO_LICENSE_BAD_CLIENT_MSG = 0x00000103,

        /// <summary>
        /// The Client Access License ([MS-RDPELE] section 1.1) stored by the client has been modified. 
        /// </summary>
        ERRINFO_LICENSE_HWID_DOESNT_MATCH_LICENSE = 0x00000104,

        /// <summary>
        /// The Client Access License ([MS-RDPELE] section 1.1) stored by the client is in an invalid format 
        /// </summary>
        ERRINFO_LICENSE_BAD_CLIENT_LICENSE = 0x00000105,

        /// <summary>
        /// Network problems have caused the licensing protocol ([MS-RDPELE] section 1.3.3) to be terminated. 
        /// </summary>
        ERRINFO_LICENSE_CANT_FINISH_PROTOCOL = 0x00000106,

        /// <summary>
        /// The client prematurely ended the licensing protocol ([MS-RDPELE] section 1.3.3).
        /// </summary>
        ERRINFO_LICENSE_CLIENT_ENDED_PROTOCOL = 0x00000107,

        /// <summary>
        /// A licensing message ([MS-RDPELE] sections 2.2 and 5.1) was incorrectly encrypted.
        /// </summary>
        ERRINFO_LICENSE_BAD_CLIENT_ENCRYPTION = 0x00000108,

        /// <summary>
        /// The Client Access License ([MS-RDPELE] section 1.1) stored by the client could not be upgraded or renewed.
        /// </summary>
        ERRINFO_LICENSE_CANT_UPGRADE_LICENSE = 0x00000109,

        /// <summary>
        /// The remote computer is not licensed to accept remote connections.
        /// Protocol-independent codes generated by Connection Broker:
        /// </summary>
        ERRINFO_LICENSE_NO_REMOTE_CONNECTIONS = 0x0000010A,

        /// <summary>
        /// The target endpoint could not be found.
        /// </summary>
        ERRINFO_CB_DESTINATION_NOT_FOUND = 0x00000400,

        /// <summary>
        /// The target endpoint to which the client is being redirected is disconnecting from the Connection Broker.
        /// </summary>
        ERRINFO_CB_LOADING_DESTINATION = 0x00000402,

        /// <summary>
        /// An error occurred while the connection was being redirected to the target endpoint.
        /// </summary>
        ERRINFO_CB_REDIRECTING_TO_DESTINATION = 0x00000404,

        /// <summary>
        /// An error occurred while the target endpoint (a virtual machine) was being awakened.
        /// </summary>
        ERRINFO_CB_SESSION_ONLINE_VM_WAKE = 0x00000405,

        /// <summary>
        /// An error occurred while the target endpoint (a virtual machine) was being started.
        /// </summary>
        ERRINFO_CB_SESSION_ONLINE_VM_BOOT = 0x00000406,

        /// <summary>
        /// The IP address of the target endpoint (a virtual machine) cannot be determined.
        /// </summary>
        ERRINFO_CB_SESSION_ONLINE_VM_NO_DNS = 0x00000407,

        /// <summary>
        /// There are no available endpoints in the pool managed by the Connection Broker.
        /// </summary>
        ERRINFO_CB_DESTINATION_POOL_NOT_FREE = 0x00000408,

        /// <summary>
        /// Processing of the connection has been canceled.
        /// </summary>
        ERRINFO_CB_CONNECTION_CANCELLED = 0x00000409,

        /// <summary>
        /// The settings contained in the routingToken field of the X.224 Connection Request PDU (section 2.2.1.1) cannot be validated.
        /// </summary>
        ERRINFO_CB_CONNECTION_ERROR_INVALID_SETTINGS = 0x00000410,

        /// <summary>
        /// A time-out occurred while the target endpoint (a virtual machine) was being started.
        /// </summary>
        ERRINFO_CB_SESSION_ONLINE_VM_BOOT_TIMEOUT = 0x00000411,

        /// <summary>
        /// A session monitoring error occurred while the target endpoint (a virtual machine) was being started.
        /// </summary>
        ERRINFO_CB_SESSION_ONLINE_VM_SESSMON_FAILED = 0x00000412,

        /// <summary>
        /// Unknown pduType2 field in a received Share Data Header (section 2.2.8.1.1.1.2).
        /// </summary>
        ERRINFO_UNKNOWNPDUTYPE2 = 0x000010C9,

        /// <summary>
        /// Unknown pduType field in a received Share Control Header (section 2.2.8.1.1.1.1).
        /// </summary>
        ERRINFO_UNKNOWNPDUTYPE = 0x000010CA,

        /// <summary>
        /// An out-of-sequence Slow-Path Data PDU (section 2.2.8.1.1.1.1) has been received.
        /// </summary>
        ERRINFO_DATAPDUSEQUENCE = 0x000010CB,

        /// <summary>
        /// An out-of-sequence Slow-Path Non-Data PDU (section 2.2.8.1.1.1.1) has been received.
        /// </summary>
        ERRINFO_CONTROLPDUSEQUENCE = 0x000010CD,

        /// <summary>
        /// A Control PDU (sections 2.2.1.15 and 2.2.1.16) has been received with an invalid action field.
        /// </summary>
        ERRINFO_INVALIDCONTROLPDUACTION = 0x000010CE,

        /// <summary>
        /// One of two possible errors:       A Slow-Path Input Event (section 2.2.8.1.1.3.1.1) has been received with an invalid messageType field.       A Fast-Path Input Event (section 2.2.8.1.2.2) has been received with an invalid eventCode field
        /// </summary>
        ERRINFO_INVALIDINPUTPDUTYPE = 0x000010CF,

        /// <summary>
        /// One of two possible errors:
        /// A Slow-Path Mouse Event (section 2.2.8.1.1.3.1.1.3) or Extended Mouse Event (section 2.2.8.1.1.3.1.1.4) has been received with an invalid pointerFlags field.
        /// A Fast-Path Mouse Event (section 2.2.8.1.2.2.3) or Fast-Path Extended Mouse Event (section 2.2.8.1.2.2.4) has been received with an invalid pointerFlags field.
        /// </summary>
        ERRINFO_INVALIDINPUTPDUMOUSE = 0x000010D0,

        /// <summary>
        /// An invalid Refresh Rect PDU (section 2.2.11.2) has been received.
        /// </summary>
        ERRINFO_INVALIDREFRESHRECTPDU = 0x000010D1,

        /// <summary>
        /// The server failed to construct the GCC Conference Create Response user data (section 2.2.1.4).
        /// </summary>
        ERRINFO_CREATEUSERDATAFAILED = 0x000010D2,

        /// <summary>
        /// Processing during the Channel Connection phase of the RDP Connection Sequence (see section 1.3.1.1 for an overview of the RDP Connection Sequence phases) has failed.
        /// </summary>
        ERRINFO_CONNECTFAILED = 0x000010D3,

        /// <summary>
        /// A Confirm Active PDU (section 2.2.1.13.2) was received from the client with an invalid shareId field.
        /// </summary>
        ERRINFO_CONFIRMACTIVEWRONGSHAREID = 0x000010D4,

        /// <summary>
        /// A Confirm Active PDU (section 2.2.1.13.2) was received from the client with an invalid originatorId field.
        /// </summary>
        ERRINFO_CONFIRMACTIVEWRONGORIGINATOR = 0x000010D5,

        /// <summary>
        /// There is not enough data to process a Persistent Key List PDU (section 2.2.1.17).
        /// </summary>
        ERRINFO_PERSISTENTKEYPDUBADLENGTH = 0x000010DA,

        /// <summary>
        /// A Persistent Key List PDU (section 2.2.1.17) marked as PERSIST_PDU_FIRST (0x01) was received after the reception of a prior Persistent Key List PDU also marked as PERSIST_PDU_FIRST. 
        /// </summary>
        ERRINFO_PERSISTENTKEYPDUILLEGALFIRST = 0x000010DB,

        /// <summary>
        /// A Persistent Key List PDU (section 2.2.1.17) was received which specified a total number of bitmap cache entries larger than 262144. 
        /// </summary>
        ERRINFO_PERSISTENTKEYPDUTOOMANYTOTALKEYS = 0x000010DC,

        /// <summary>
        /// A Persistent Key List PDU (section 2.2.1.17) was received which specified an invalid total number of keys for a bitmap cache (the number of entries that can be stored within each bitmap cache is specified in the Revision 1 or 2 Bitmap Cac
        /// </summary>
        ERRINFO_PERSISTENTKEYPDUTOOMANYCACHEKEYS = 0x000010DD,

        /// <summary>
        /// There is not enough data to process Input Event PDU Data (section 2.2.8.1.1.3.1) or a Fast-Path Input Event PDU (section 2.2.8.1.2).
        /// </summary>
        ERRINFO_INPUTPDUBADLENGTH = 0x000010DE,

        /// <summary>
        /// There is not enough data to process the shareDataHeader, NumInfoBlocks, Pad1, and Pad2 fields of the Bitmap Cache Error PDU Data ([MS-RDPEGDI] section 2.2.2.3.1.1).
        /// </summary>
        ERRINFO_BITMAPCACHEERRORPDUBADLENGTH = 0x000010DF,

        /// <summary>
        /// One of two possible errors:
        /// The dataSignature field of the Fast-Path Input Event PDU (section 2.2.8.1.2) does not contain enough data.
        /// The fipsInformation and dataSignature fields of the Fast-Path Input Event PDU (section 2.2.8.1.2) do not contain enough data.
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT = 0x000010E0,

        /// <summary>
        /// One of two possible errors:
        /// There is not enough data in the Client Network Data (section 2.2.1.3.4) to read the virtual channel configuration data.
        /// There is not enough data to read a complete Channel PDU Header (section 2.2.6.1.1).
        /// </summary>
        ERRINFO_VCHANNELDATATOOSHORT = 0x000010E1,

        /// <summary>
        /// One of four possible errors:
        /// There is not enough data to process Control PDU Data (section 2.2.1.15.1).
        /// There is not enough data to read a complete Share Control Header (section 2.2.8.1.1.1.1).
        /// There is not enough data to read a complete Share Data Header (section 2.2.8.1.1.1.2) of a Slow-Path Data PDU (section 2.2.8.1.1.1.1).
        /// There is not enough data to process Font List PDU Data (section 2.2.1.18.1).
        /// </summary>
        ERRINFO_SHAREDATATOOSHORT = 0x000010E2,

        /// <summary>
        /// One of two possible errors:
        /// There is not enough data to process Suppress Output PDU Data (section 2.2.11.3.1).
        /// The allowDisplayUpdates field of the Suppress Output PDU Data (section 2.2.11.3.1) is invalid.
        /// </summary>
        ERRINFO_BADSUPRESSOUTPUTPDU = 0x000010E3,

        /// <summary>
        /// One of two possible errors:
        /// There is not enough data to read the shareControlHeader, shareId, originatorId, lengthSourceDescriptor, and lengthCombinedCapabilities fields of the Confirm Active PDU Data (section 2.2.1.13.2.1).
        /// There is not enough data to read the sourceDescriptor, numberCapabilities, pad2Octets, and capabilitySets fields of the Confirm Active PDU Data (section 2.2.1.13.2.1).
        /// </summary>
        ERRINFO_CONFIRMACTIVEPDUTOOSHORT = 0x000010E5,

        /// <summary>
        /// There is not enough data to read the capabilitySetType and the lengthCapability fields in a received Capability Set (section 2.2.1.13.1.1.1).
        /// </summary>
        ERRINFO_CAPABILITYSETTOOSMALL = 0x000010E7,

        /// <summary>
        /// A Capability Set (section 2.2.1.13.1.1.1) has been received with a lengthCapability field that contains a value greater than the total length of the data received.
        /// </summary>
        ERRINFO_CAPABILITYSETTOOLARGE = 0x000010E8,

        /// <summary>
        /// One of two possible errors:
        /// Both the colorPointerCacheSize and pointerCacheSize fields in the Pointer Capability Set (section 2.2.7.1.5) are set to zero.
        /// The pointerCacheSize field in the Pointer Capability Set (section 2.2.7.1.5) is not present, and the colorPointerCacheSize field is set to zero.
        /// </summary>
        ERRINFO_NOCURSORCACHE = 0x000010E9,

        /// <summary>
        /// The capabilities received from the client in the Confirm Active PDU (section 2.2.1.13.2) were not accepted by the server.
        /// </summary>
        ERRINFO_BADCAPABILITIES = 0x000010EA,

        /// <summary>
        /// An error occurred while using the bulk compressor (section 3.1.8 and [MS-RDPEGDI] section 3.1.8) to decompress a Virtual Channel PDU (section 2.2.6.1) 
        /// </summary>
        ERRINFO_VIRTUALCHANNELDECOMPRESSIONERR = 0x000010EC,

        /// <summary>
        /// An invalid bulk compression package was specified in the flags field of the Channel PDU Header (section 2.2.6.1.1).
        /// </summary>
        ERRINFO_INVALIDVCCOMPRESSIONTYPE = 0x000010ED,

        /// <summary>
        /// An invalid MCS channel ID was specified in the mcsPdu field of the Virtual Channel PDU (section 2.2.6.1).
        /// </summary>
        ERRINFO_INVALIDCHANNELID = 0x000010EF,

        /// <summary>
        /// The client requested more than the maximum allowed 31 static virtual channels in the Client Network Data (section 2.2.1.3.4).
        /// </summary>
        ERRINFO_VCHANNELSTOOMANY = 0x000010F0,

        /// <summary>
        /// The INFO_RAIL flag (0x00008000) MUST be set in the flags field of the Info Packet (section 2.2.1.11.1.1) as the session on the remote server can only host remote applications.
        /// </summary>
        ERRINFO_REMOTEAPPSNOTENABLED = 0x000010F3,

        /// <summary>
        /// The client sent a Persistent Key List PDU (section 2.2.1.17) without including the prerequisite Revision 2 Bitmap Cache Capability Set (section 2.2.7.1.4.2) in the Confirm Active PDU (section 2.2.1.13.2).
        /// </summary>
        ERRINFO_CACHECAPNOTSET = 0x000010F4,

        /// <summary>
        /// The NumInfoBlocks field in the Bitmap Cache Error PDU Data is inconsistent with the amount of data in the Info field ([MS-RDPEGDI] section 2.2.2.3.1.1). 
        /// </summary>
        ERRINFO_BITMAPCACHEERRORPDUBADLENGTH2 = 0x000010F5,

        /// <summary>
        /// There is not enough data to process an Offscreen Bitmap Cache Error PDU ([MS-RDPEGDI] section 2.2.2.3.2).
        /// </summary>
        ERRINFO_OFFSCRCACHEERRORPDUBADLENGTH = 0x000010F6,

        /// <summary>
        /// There is not enough data to process a DrawNineGrid Cache Error PDU ([MS-RDPEGDI] section 2.2.2.3.3).
        /// </summary>
        ERRINFO_DNGCACHEERRORPDUBADLENGTH = 0x000010F7,

        /// <summary>
        /// There is not enough data to process a GDI+ Error PDU ([MS-RDPEGDI] section 2.2.2.3.4).
        /// </summary>
        ERRINFO_GDIPLUSPDUBADLENGTH = 0x000010F8,

        /// <summary>
        /// There is not enough data to read a Basic Security Header (section 2.2.8.1.1.2.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT2 = 0x00001111,

        /// <summary>
        /// There is not enough data to read a Non-FIPS Security Header (section 2.2.8.1.1.2.2) or FIPS Security Header (section 2.2.8.1.1.2.3).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT3 = 0x00001112,

        /// <summary>
        /// There is not enough data to read the basicSecurityHeader and length fields of the Security Exchange PDU Data (section 2.2.1.10.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT4 = 0x00001113,

        /// <summary>
        /// There is not enough data to read the CodePage, flags, cbDomain, cbUserName, cbPassword, cbAlternateShell, cbWorkingDir, Domain, UserName, Password, AlternateShell, and WorkingDir fields in the Info Packet (section 2.2.1.11.1.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT5 = 0x00001114,

        /// <summary>
        /// There is not enough data to read the CodePage, flags, cbDomain, cbUserName, cbPassword, cbAlternateShell, and cbWorkingDir fields in the Info Packet (section 2.2.1.11.1.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT6 = 0x00001115,

        /// <summary>
        /// There is not enough data to read the clientAddressFamily and cbClientAddress fields in the Extended Info Packet (section 2.2.1.11.1.1.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT7 = 0x00001116,

        /// <summary>
        /// There is not enough data to read the clientAddress field in the Extended Info Packet (section 2.2.1.11.1.1.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT8 = 0x00001117,

        /// <summary>
        /// There is not enough data to read the cbClientDir field in the Extended Info Packet (section 2.2.1.11.1.1.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT9 = 0x00001118,

        /// <summary>
        /// There is not enough data to read the clientDir field in the Extended Info Packet (section 2.2.1.11.1.1.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT10 = 0x00001119,

        /// <summary>
        /// There is not enough data to read the clientTimeZone field in the Extended Info Packet (section 2.2.1.11.1.1.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT11 = 0x0000111A,

        /// <summary>
        /// There is not enough data to read the clientSessionId field in the Extended Info Packet (section 2.2.1.11.1.1.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT12 = 0x0000111B,

        /// <summary>
        /// There is not enough data to read the performanceFlags field in the Extended Info Packet (section 2.2.1.11.1.1.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT13 = 0x0000111C,

        /// <summary>
        /// There is not enough data to read the cbAutoReconnectCookie field in the Extended Info Packet (section 2.2.1.11.1.1.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT14 = 0x0000111D,

        /// <summary>
        /// There is not enough data to read the autoReconnectCookie field in the Extended Info Packet (section 2.2.1.11.1.1.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT15 = 0x0000111E,

        /// <summary>
        /// The cbAutoReconnectCookie field in the Extended Info Packet (section 2.2.1.11.1.1.1) contains a value which is larger than the maximum allowed length of 128 bytes.
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT16 = 0x0000111F,

        /// <summary>
        /// There is not enough data to read the clientAddressFamily and cbClientAddress fields in the Extended Info Packet (section 2.2.1.11.1.1.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT17 = 0x00001120,

        /// <summary>
        /// There is not enough data to read the clientAddress field in the Extended Info Packet (section 2.2.1.11.1.1.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT18 = 0x00001121,

        /// <summary>
        /// There is not enough data to read the cbClientDir field in the Extended Info Packet (section 2.2.1.11.1.1.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT19 = 0x00001122,

        /// <summary>
        /// There is not enough data to read the clientDir field in the Extended Info Packet (section 2.2.1.11.1.1.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT20 = 0x00001123,

        /// <summary>
        /// There is not enough data to read the clientTimeZone field in the Extended Info Packet (section 2.2.1.11.1.1.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT21 = 0x00001124,

        /// <summary>
        /// There is not enough data to read the clientSessionId field in the Extended Info Packet (section 2.2.1.11.1.1.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT22 = 0x00001125,

        /// <summary>
        /// There is not enough data to read the Client Info PDU Data (section 2.2.1.11.1).
        /// </summary>
        ERRINFO_SECURITYDATATOOSHORT23 = 0x00001126,

        /// <summary>
        /// The number of TS_MONITOR_DEF (section 2.2.1.3.6.1) structures present in the monitorDefArray field of the Client Monitor Data (section 2.2.1.3.6) is less than the value specified in monitorCount field.
        /// </summary>
        ERRINFO_BADMONITORDATA = 0x00001129,

        /// <summary>
        /// The server-side decompression buffer is invalid, or the size of the decompressed VC data exceeds the chunking size specified in the Virtual Channel Capability Set (section 2.2.7.1.10).
        /// </summary>
        ERRINFO_VCDECOMPRESSEDREASSEMBLEFAILED = 0x0000112A,

        /// <summary>
        /// The size of a received Virtual Channel PDU (section 2.2.6.1) exceeds the chunking size specified in the Virtual Channel Capability Set (section 2.2.7.1.10).
        /// </summary>
        ERRINFO_VCDATATOOLONG = 0x0000112B,

        /// <summary>
        /// There is not enough data to read a TS_FRAME_ACKNOWLEDGE_PDU ([MS-RDPRFX] section 2.2.3.1).
        /// </summary>
        ERRINFO_BAD_FRAME_ACK_DATA = 0x0000112C,

        /// <summary>
        /// The graphics mode requested by the client is not supported by the server.
        /// </summary>
        ERRINFO_GRAPHICSMODENOTSUPPORTED = 0x0000112D,

        /// <summary>
        /// The server-side graphics subsystem failed to reset.
        /// </summary>
        ERRINFO_GRAPHICSSUBSYSTEMRESETFAILED = 0x0000112E,

        /// <summary>
        /// The server-side graphics subsystem is in an error state and unable to continue graphics encoding.
        /// </summary>
        ERRINFO_GRAPHICSSUBSYSTEMFAILED = 0x0000112F,

        /// <summary>
        /// There is not enough data to read the cbDynamicDSTTimeZoneKeyName field in the Extended Info Packet (section 2.2.1.11.1.1.1).
        /// </summary>
        ERRINFO_TIMEZONEKEYNAMELENGTHTOOSHORT = 0x00001130,

        /// <summary>
        /// The length reported in the cbDynamicDSTTimeZoneKeyName field of the Extended Info Packet (section 2.2.1.11.1.1.1) is too long.
        /// </summary>
        ERRINFO_TIMEZONEKEYNAMELENGTHTOOLONG = 0x00001131,

        /// <summary>
        /// The dynamicDaylightTimeDisabled field is not present in the Extended Info Packet (section 2.2.1.11.1.1.1).
        /// </summary>
        ERRINFO_DYNAMICDSTDISABLEDFIELDMISSING = 0x00001132,

        /// <summary>
        /// An error occurred when processing dynamic virtual channel data ([MS-RDPEDYC] section 3.3.5).
        /// </summary>
        ERRINFO_VCDECODINGERROR = 0x00001133,

        /// <summary>
        /// The width or height of the virtual desktop defined by the monitor layout in the Client Monitor Data (section 2.2.1.3.6) is larger than the maximum allowed value of 32,766.
        /// </summary>
        ERRINFO_VIRTUALDESKTOPTOOLARGE = 0x00001134,

        /// <summary>
        /// The monitor geometry defined by the Client Monitor Data (section 2.2.1.3.6) is invalid.
        /// </summary>
        ERRINFO_MONITORGEOMETRYVALIDATIONFAILED = 0x00001135,

        /// <summary>
        /// The monitorCount field in the Client Monitor Data (section 2.2.1.3.6) is too large.
        /// </summary>
        ERRINFO_INVALIDMONITORCOUNT = 0x00001136,

        /// <summary>
        /// An attempt to update the session keys while using Standard RDP Security mechanisms (section 5.3.7) failed.
        /// </summary>
        ERRINFO_UPDATESESSIONKEYFAILED = 0x00001191,

        /// <summary>
        /// One of two possible error conditions:
        /// Decryption using Standard RDP Security mechanisms (section 5.3.6) failed.
        /// Session key creation using Standard RDP Security mechanisms (section 5.3.5) failed.
        /// </summary>
        ERRINFO_DECRYPTFAILED = 0x00001192,

        /// <summary>
        /// Encryption using Standard RDP Security mechanisms (section 5.3.6) failed.
        /// </summary>
        ERRINFO_ENCRYPTFAILED = 0x00001193,

        /// <summary>
        /// Failed to find a usable Encryption Method (section 5.3.2) in the encryptionMethods field of the Client Security Data (section 2.2.1.4.3).
        /// </summary>
        ERRINFO_ENCPKGMISMATCH = 0x00001194,
    }

    /// <summary>
    ///  Virtual Channel data.
    /// </summary>
    public struct ChannelChunk
    {
        /// <summary>
        ///  Virtual Channel PDU Header structure which contains
        ///  control flags and describes the size of the opaque
        ///  channel data.
        /// </summary>
        public CHANNEL_PDU_HEADER channelPduHeader;

        /// <summary>
        ///  Variable length data to be processed by the static virtual
        ///  channel protocol handler. This field MUST NOT be larger
        ///  than CHANNEL_CHUNK_LENGTH (1600) bytes in size.
        /// </summary>
        public byte[] chunkData;
    }

    /// <summary>
    ///  The Virtual Channel PDU is sent from client to server
    ///  or from server to client and is used to transport data
    ///  between static virtual channel end-points.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1a.xml
    //  </remarks>
    public partial class Virtual_Channel_RAW_Pdu : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  Virtual Channel PDU Header structure which contains
        ///  control flags and describes the size of the opaque
        ///  channel data.
        /// </summary>
        public CHANNEL_PDU_HEADER channelPduHeader;

        /// <summary>
        ///  Variable length data to be processed by the static virtual
        ///  channel protocol handler. This field MUST NOT be larger
        ///  than CHANNEL_CHUNK_LENGTH (1600) bytes in size.
        /// </summary>
        public byte[] virtualChannelData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Virtual_Channel_RAW_Pdu(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Virtual_Channel_RAW_Pdu()
        {
            context = null;
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> channelBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(channelBuffer, channelPduHeader);
            RdpbcgrEncoder.EncodeBytes(channelBuffer, virtualChannelData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, channelBuffer.ToArray(), context);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }
    }

    /// <summary>
    ///  The Virtual Channel PDU is sent from client to server
    ///  or from server to client and is used to transport data
    ///  between static virtual channel end-points.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1a.xml
    //  </remarks>
    public partial class Virtual_Channel_Complete_Pdu : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The static virtual channel Id.
        /// </summary>
        public long channelId;

        /// <summary>
        ///  The reassembled data.
        /// </summary>
        public byte[] virtualChannelData;

        /// <summary>
        ///  The splitted raw PDUs.
        /// </summary>
        public Collection<Virtual_Channel_RAW_Pdu> rawPdus;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Virtual_Channel_Complete_Pdu(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Virtual_Channel_Complete_Pdu()
        {
        }

        /// <summary>
        /// Split the virtualChannelData to several chunk data to send. 
        /// </summary>
        public void SplitToChunks()
        {
            ChannelChunk[] chunks = context.SplitToChunks(channelId, virtualChannelData);
            if (chunks != null && chunks.Length > 0)
            {
                rawPdus = new Collection<Virtual_Channel_RAW_Pdu>();
                for (int i = 0; i < chunks.Length; ++i)
                {
                    Virtual_Channel_RAW_Pdu rawPdu = new Virtual_Channel_RAW_Pdu(context);
                    rawPdu.channelPduHeader = chunks[i].channelPduHeader;
                    rawPdu.virtualChannelData = chunks[i].chunkData;
                    rawPdus.Add(rawPdu);

                    RdpbcgrUtility.FillCommonHeader(ref rawPdus[i].commonHeader,
                                             TS_SECURITY_HEADER_flags_Values.SEC_IGNORE_SEQNO
                                             | TS_SECURITY_HEADER_flags_Values.SEC_RESET_SEQNO,
                                             context,
                                             channelId);
                }
            }
        }
    }

    /// <summary>
    ///  The Virtual Channel PDU is sent from client to server
    ///  or from server to client and is used to transport data
    ///  between static virtual channel end-points.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1a.xml
    //  </remarks>
    public partial class Virtual_Channel_RAW_Server_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  Virtual Channel PDU Header structure which contains
        ///  control flags and describes the size of the opaque
        ///  channel data.
        /// </summary>
        public CHANNEL_PDU_HEADER channelPduHeader;

        /// <summary>
        ///  Variable length data to be processed by the static virtual
        ///  channel protocol handler. This field MUST NOT be larger
        ///  than CHANNEL_CHUNK_LENGTH (1600) bytes in size.
        /// </summary>
        public byte[] virtualChannelData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Virtual_Channel_RAW_Server_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Virtual_Channel_RAW_Server_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> channelBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(channelBuffer, channelPduHeader);
            RdpbcgrEncoder.EncodeBytes(channelBuffer, virtualChannelData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, channelBuffer.ToArray(), serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes);

            return encodedBytes;
        }

        public override StackPacket Clone()
        {
            Virtual_Channel_RAW_Server_Pdu pduClone = new Virtual_Channel_RAW_Server_Pdu();
            pduClone.commonHeader = commonHeader;
            pduClone.channelPduHeader = channelPduHeader;

            pduClone.virtualChannelData = RdpbcgrUtility.CloneByteArray(virtualChannelData);

            return pduClone;
        }
    }

    /// <summary>
    ///  The Virtual Channel PDU is sent from client to server
    ///  or from server to client and is used to transport data
    ///  between static virtual channel end-points.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1a.xml
    //  </remarks>
    public partial class Virtual_Channel_Complete_Server_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The static virtual channel Id.
        /// </summary>
        public long channelId;

        /// <summary>
        ///  The reassembled data.
        /// </summary>
        public byte[] virtualChannelData;

        /// <summary>
        ///  The splitted raw PDUs.
        /// </summary>
        public Collection<Virtual_Channel_RAW_Server_Pdu> rawPdus;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Virtual_Channel_Complete_Server_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Virtual_Channel_Complete_Server_Pdu()
        {
        }

        /// <summary>
        /// Split the virtualChannelData to several chunk data to send. 
        /// </summary>
        public void SplitToChunks()
        {
            ChannelChunk[] chunks = serverSessionContext.SplitToChunks(channelId, virtualChannelData);
            if (chunks != null && chunks.Length > 0)
            {
                rawPdus = new Collection<Virtual_Channel_RAW_Server_Pdu>();
                for (int i = 0; i < chunks.Length; ++i)
                {
                    Virtual_Channel_RAW_Server_Pdu rawPdu = new Virtual_Channel_RAW_Server_Pdu(serverSessionContext);
                    rawPdu.channelPduHeader = chunks[i].channelPduHeader;
                    rawPdu.virtualChannelData = chunks[i].chunkData;
                    rawPdus.Add(rawPdu);

                    RdpbcgrUtility.FillCommonHeader(ref rawPdus[i].commonHeader,
                                             TS_SECURITY_HEADER_flags_Values.SEC_IGNORE_SEQNO
                                             | TS_SECURITY_HEADER_flags_Values.SEC_RESET_SEQNO,
                                             serverSessionContext,
                                             channelId);
                }
            }
        }

        public override StackPacket Clone()
        {
            Virtual_Channel_Complete_Server_Pdu pduClone = new Virtual_Channel_Complete_Server_Pdu();
            pduClone.channelId = channelId;
            pduClone.virtualChannelData = RdpbcgrUtility.CloneByteArray(virtualChannelData);
            if (rawPdus != null)
            {
                pduClone.rawPdus = new Collection<Virtual_Channel_RAW_Server_Pdu>();
                for (int i = 0; i < rawPdus.Count; ++i)
                {
                    pduClone.rawPdus.Add(rawPdus[i]);
                }
            }
            return pduClone;
        }

        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(totalBuffer, channelId);
            RdpbcgrEncoder.EncodeBytes(totalBuffer, virtualChannelData);

            for (int i = 0; i < rawPdus.Count; i++)
            {
                List<byte> channelBuffer = new List<byte>();
                RdpbcgrEncoder.EncodeStructure(channelBuffer, rawPdus[i].channelPduHeader);
                RdpbcgrEncoder.EncodeBytes(channelBuffer, rawPdus[i].virtualChannelData);
                RdpbcgrEncoder.EncodeSlowPathPdu(
                    totalBuffer,
                    rawPdus[i].commonHeader,
                    channelBuffer.ToArray(),
                    serverSessionContext);
            }

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }
    }

    #region Self-defined Capability Sets
    /// <summary>
    /// Color Table Cache Capability Set (see [MS-RDPEGDI] section 2.2.1.1)
    /// </summary>
    public partial struct TS_COLORCACHE_CAPABILITYSET : ITsCapsSet
    {
        /// <summary>
        /// The whole data.
        /// </summary>
        public byte[] rawData;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return rawData;
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType_Values.CAPSTYPE_COLORCACHE;
        }
    }

    /// <summary>
    /// DrawNineGrid Cache Capability Set ([MS-RDPEGDI] section 2.2.1.2)
    /// </summary>
    public partial struct TS_DRAWNINEGRIDCACHE_CAPABILITYSET : ITsCapsSet
    {
        /// <summary>
        /// The whole data.
        /// </summary>
        public byte[] rawData;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return rawData;
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType_Values.CAPSTYPE_DRAWNINEGRIDCACHE;
        }
    }

    /// <summary>
    /// Draw GDI+ Cache Capability Set ([MS-RDPEGDI] section 2.2.1.3)
    /// </summary>
    public partial struct TS_DRAWGRIDPLUS_CAPABILITYSET : ITsCapsSet
    {
        /// <summary>
        /// The whole data.
        /// </summary>
        public byte[] rawData;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return rawData;
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType_Values.CAPSTYPE_DRAWGDIPLUS;
        }
    }

    /// <summary>
    /// Remote Programs Capability Set ([MS-RDPERP] section 2.2.1.1.1)
    /// </summary>
    public partial struct TS_RAIL_CAPABILITYSET : ITsCapsSet
    {
        /// <summary>
        /// The whole data.
        /// </summary>
        public byte[] rawData;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return rawData;
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType_Values.CAPSTYPE_RAIL;
        }
    }

    /// <summary>
    /// Window List Capability Set ([MS-RDPERP] section 2.2.1.1.2)
    /// </summary>
    public partial struct TS_WINDOW_CAPABILITYSET : ITsCapsSet
    {
        /// <summary>
        /// The whole data.
        /// </summary>
        public byte[] rawData;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return rawData;
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType_Values.CAPSTYPE_WINDOW;
        }
    }
    #endregion Self-defined Capability Sets

    /// <summary>
    ///  The TS_GENERAL_CAPABILITYSET structure is used
    ///  to advertise general characteristics and is based on
    ///  the capability set specified in [T128] section 8.2.3.
    ///  This capability is sent by both client and server.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1_1.xml
    //  </remarks>
    public partial struct TS_GENERAL_CAPABILITYSET : ITsCapsSet
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The type of the capability
        ///  set. This field MUST be set to CAPSTYPE_GENERAL (1).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  A 16-bit unsigned integer. The type of platform.
        /// </summary>
        public osMajorType_Values osMajorType;

        /// <summary>
        ///  A 16-bit unsigned integer. The version of the platform
        ///  specified in the osMajorType field.
        /// </summary>
        public osMinorType_Values osMinorType;

        /// <summary>
        ///  A 16-bit unsigned integer. The protocol version. This
        ///  field MUST be set to TS_CAPS_PROTOCOLVERSION (0x0200).
        /// </summary>
        public protocolVersion_Values protocolVersion;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2octetsA;

        /// <summary>
        ///  A 16-bit unsigned integer. General compression types.
        ///  This field MUST be set to 0.
        /// </summary>
        public generalCompressionTypes_Values generalCompressionTypes;

        /// <summary>
        ///  A 16-bit unsigned integer. General capability information.Remote
        ///  Desktop Protocol (RDP) 5.0 and later supports the following
        ///  flags:
        /// </summary>
        public extraFlags_Values extraFlags;

        /// <summary>
        ///  A 16-bit unsigned integer. Support for update capability.
        ///  This field MUST be set to 0.
        /// </summary>
        public updateCapabilityFlag_Values updateCapabilityFlag;

        /// <summary>
        ///  A 16-bit unsigned integer. Support for remote unsharing.
        ///  This field MUST be set to 0.
        /// </summary>
        public remoteUnshareFlag_Values remoteUnshareFlag;

        /// <summary>
        ///  A 16-bit unsigned integer. General compression level.
        ///  This field MUST be set to 0.
        /// </summary>
        public generalCompressionLevel_Values generalCompressionLevel;

        /// <summary>
        ///  An 8-bit unsigned integer. Server-only flag that indicates
        ///  whether the Refresh Rect PDU is supported.
        /// </summary>
        public refreshRectSupport_Values refreshRectSupport;

        /// <summary>
        ///  An 8-bit unsigned integer. Server-only flag that indicates
        ///  whether the Suppress Output PDU is supported.
        /// </summary>
        public suppressOutputSupport_Values suppressOutputSupport;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return RdpbcgrUtility.StructToBytes(this);
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    /// The type of osMajorType.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum osMajorType_Values : ushort
    {

        /// <summary>
        ///  Unspecified platform
        /// </summary>
        OSMAJORTYPE_UNSPECIFIED = 0x0000,

        /// <summary>
        ///  Windows platform
        /// </summary>
        OSMAJORTYPE_WINDOWS = 0x0001,

        /// <summary>
        ///  OS/2 platform
        /// </summary>
        OSMAJORTYPE_OS2 = 0x0002,

        /// <summary>
        ///  Macintosh platform
        /// </summary>
        OSMAJORTYPE_MACINTOSH = 0x0003,

        /// <summary>
        ///  UNIX platform
        /// </summary>
        OSMAJORTYPE_UNIX = 0x0004,

        /// <summary>
        /// iOS platform
        /// </summary>
        OSMAJORTYPE_IOS = 0x0005,

        /// <summary>
        /// OS X platform
        /// </summary>
        OSMAJORTYPE_OSX = 0x0006,

        /// <summary>
        /// Android platform
        /// </summary>
        OSMAJORTYPE_ANDROID = 0x0007,

        /// <summary>
        /// Chrome OS platform
        /// </summary>
        OSMAJORTYPE_CHROME_OS = 0x0008

    }

    /// <summary>
    /// The type of osMinorType.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum osMinorType_Values : ushort
    {

        /// <summary>
        ///  Unspecified version
        /// </summary>
        OSMINORTYPE_UNSPECIFIED = 0x0000,

        /// <summary>
        ///  Windows 3.1x
        /// </summary>
        OSMINORTYPE_WINDOWS_31X = 0x0001,

        /// <summary>
        ///  Windows 95
        /// </summary>
        TOSMINORTYPE_WINDOWS_95 = 0x0002,

        /// <summary>
        ///  Windows NT
        /// </summary>
        OSMINORTYPE_WINDOWS_NT = 0x0003,

        /// <summary>
        ///  OS/2 2.1
        /// </summary>
        OSMINORTYPE_OS2_V21 = 0x0004,

        /// <summary>
        ///  PowerPC
        /// </summary>
        OSMINORTYPE_POWER_PC = 0x0005,

        /// <summary>
        ///  Macintosh
        /// </summary>
        OSMINORTYPE_MACINTOSH = 0x0006,

        /// <summary>
        ///  Native X Server
        /// </summary>
        OSMINORTYPE_NATIVE_XSERVER = 0x0007,

        /// <summary>
        ///  Pseudo X Server
        /// </summary>
        OSMINORTYPE_PSEUDO_XSERVER = 0x0008,

        /// <summary>
        /// Windows RT
        /// </summary>
        OSMINORTYPE_WINDOWS_RT = 0x0009

    }

    /// <summary>
    /// The type of protocolVersion.
    /// </summary>
    public enum protocolVersion_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0200,
    }

    /// <summary>
    /// The type of generalCompressionTypes.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum generalCompressionTypes_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0000,
    }

    /// <summary>
    /// The type of extraFlags.
    /// </summary>
    public enum extraFlags_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Advertiser supports fast-path output.
        /// </summary>
        FASTPATH_OUTPUT_SUPPORTED = 0x0001,

        /// <summary>
        ///  The 8-byte Compressed Data Header MUST NOT be used in
        ///  conjunction with compressed bitmap data.
        /// </summary>
        NO_BITMAP_COMPRESSION_HDR = 0x0400,

        /// <summary>
        ///  Advertiser supports shadow compression. When this flag
        ///  is set, the participating shadow client can support
        ///  data compression during shadowing, provided that the
        ///  compression level matches among the shadow clients.
        ///  RDP 5.0 has no data compression for shadowing.
        /// </summary>
        SHADOW_COMPRESSION_LEVEL = 0x0002,

        /// <summary>
        ///  Advertiser (client or server) supports long-length credentials
        ///  for the user name, password, or domain name.
        /// </summary>
        LONG_CREDENTIALS_SUPPORTED = 0x0004,

        /// <summary>
        ///  Advertiser supports session auto-reconnection. 							This
        ///  flag allows a disconnected client to seamlessly reconnect
        ///  to its original session without the user resupplying
        ///  logon credentials.
        /// </summary>
        AUTORECONNECT_SUPPORTED = 0x0008,

        /// <summary>
        ///  Advertiser supports salted message authentication code
        ///  (MAC) generation (see section ).
        /// </summary>
        ENC_SALTED_CHECKSUM = 0x0010,
    }

    /// <summary>
    /// The type of updateCapabilityFlag.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum updateCapabilityFlag_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0000,
    }

    /// <summary>
    /// The type of remoteUnshareFlag.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum remoteUnshareFlag_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0000,
    }

    /// <summary>
    /// The type of generalCompressionLevel.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum generalCompressionLevel_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0000,
    }

    /// <summary>
    /// The type of refreshRectSupport.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum refreshRectSupport_Values : byte
    {

        /// <summary>
        ///  Server does not support Refresh Rect PDU.
        /// </summary>
        FALSE = 0x00,

        /// <summary>
        ///  Server supports Refresh Rect PDU.
        /// </summary>
        TRUE = 0x01,
    }

    /// <summary>
    /// The type of suppressOutputSupport.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum suppressOutputSupport_Values : byte
    {

        /// <summary>
        ///  Server does not support Suppress Output PDU.
        /// </summary>
        FALSE = 0x00,

        /// <summary>
        ///  Server supports Suppress Output PDU.
        /// </summary>
        TRUE = 0x01,
    }

    /// <summary>
    ///  The TS_OFFSCREEN_CAPABILITYSET structure is used
    ///  to advertise support for off-screen bitmap caching (see
    ///  [MS-RDPEGDI] section 3.1.1.1.5). This capability is
    ///  only sent from client to server. 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1_10.xml
    //  </remarks>
    public partial struct TS_OFFSCREEN_CAPABILITYSET : ITsCapsSet
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The type of the capability
        ///  set. This field MUST be set to CAPSTYPE_OFFSCREENCACHE
        ///  (17).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  A 32-bit unsigned integer. Offscreen bitmap cache support
        ///  level.
        /// </summary>
        public offscreenSupportLevel_Values offscreenSupportLevel;

        /// <summary>
        ///  A 16-bit unsigned integer. The maximum size in kilobytes
        ///  (KB) of the off-screen bitmap cache (largest allowed
        ///  value is 7680 KB).
        /// </summary>
        public ushort offscreenCacheSize;

        /// <summary>
        ///  A 16-bit unsigned integer. The maximum number of cache
        ///  entries (largest allowed value is 500 entries)
        /// </summary>
        public ushort offscreenCacheEntries;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return RdpbcgrUtility.StructToBytes(this);
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    /// The type of offscreenSupportLevel.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum offscreenSupportLevel_Values : uint
    {

        /// <summary>
        ///  Offscreen bitmap cache is not supported.
        /// </summary>
        FALSE = 0x00000000,

        /// <summary>
        ///  Offscreen bitmap cache is supported.
        /// </summary>
        TRUE = 0x00000001,
    }

    /// <summary>
    ///  The TS_VIRTUALCHANNEL_CAPABILITYSET structure is
    ///  used to advertise virtual channel support characteristics.
    ///  This capability is sent by both client and server.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1_11.xml
    //  </remarks>
    public partial struct TS_VIRTUALCHANNEL_CAPABILITYSET : ITsCapsSet
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The type of the capability
        ///  set. This field MUST be set to CAPSTYPE_VIRTUALCHANNEL
        ///  (20).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  A 32-bit unsigned integer. Virtual channel compression
        ///  flags.
        /// </summary>
        public TS_VIRTUALCHANNEL_CAPABILITYSET_flags_Values flags;

        /// <summary>
        /// The chuck size of virtual channel.
        /// </summary>
        public uint VCChunkSize;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            //To support VCChunkSize is not present.
            byte[] dataOrg = RdpbcgrUtility.StructToBytes(this);
            byte[] data = new byte[lengthCapability];
            Array.Copy(dataOrg, data, lengthCapability);
            return data;
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    /// The type of flags.
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum TS_VIRTUALCHANNEL_CAPABILITYSET_flags_Values : uint
    {

        /// <summary>
        ///  Virtual channel compression is not supported.
        /// </summary>
        VCCAPS_NO_COMPR = 0x00000000,

        /// <summary>
        ///  Virtual channel compression is supported for server-to-client
        ///  traffic. The highest compression level supported by
        ///  the client is advertised in the Client Info PDU.
        /// </summary>
        VCCAPS_COMPR_SC = 0x00000001,

        /// <summary>
        ///  Virtual channel compression is supported for client-to-server
        ///  traffic. The compression level is implicitly limited
        ///  to MPPC-8K for scalability reasons.
        /// </summary>
        VCCAPS_COMPR_CS_8K = 0x00000002,
    }

    /// <summary>
    ///  The TS_SOUND_CAPABILITYSET structure advertises
    ///  the ability to play a "beep" sound. This capability
    ///  is only sent from client to server.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1_12.xml
    //  </remarks>
    public partial struct TS_SOUND_CAPABILITYSET : ITsCapsSet
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The type of the capability
        ///  set. This field MUST be set to CAPSTYPE_SOUND (12).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  A 16-bit unsigned integer. Support for sound options.
        ///  
        /// </summary>
        public soundFlags_Values soundFlags;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2octetsA;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return RdpbcgrUtility.StructToBytes(this);
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    /// The type of soundFlags.
    /// </summary>
    public enum soundFlags_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Playing a beep sound is supported.
        /// </summary>
        SOUND_BEEPS_FLAG = 0x0001,
    }

    /// <summary>
    ///  The CHANNEL_PDU_HEADER MUST precede all opaque static
    ///  virtual channel traffic chunks transmitted via RDP
    ///  between client and server.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1_1a.xml
    //  </remarks>
    public partial struct CHANNEL_PDU_HEADER
    {

        /// <summary>
        ///  A 32-bit unsigned integer. The total length in bytes
        ///  of the uncompressed channel data, excluding this header.
        ///  The data can span over multiple frames and the individual
        ///  chunks will need to be reassembled in that case (see
        ///  section ).
        /// </summary>
        public uint length;

        /// <summary>
        ///  A 32-bit unsigned integer. The channel control flags.
        /// </summary>
        public CHANNEL_PDU_HEADER_flags_Values flags;
    }

    /// <summary>
    /// The type of flags.
    /// </summary>
    public enum CHANNEL_PDU_HEADER_flags_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates that the chunk is the first in a sequence.
        /// </summary>
        CHANNEL_FLAG_FIRST = 0x00000001,

        /// <summary>
        ///  Indicates that the chunk is the last in a sequence.
        /// </summary>
        CHANNEL_FLAG_LAST = 0x00000002,

        /// <summary>
        ///  The Channel PDU Header MUST be visible to the application
        ///  endpoint (see section ).
        /// </summary>
        CHANNEL_FLAG_SHOW_PROTOCOL = 0x00000010,

        /// <summary>
        ///  All virtual channel traffic MUST be suspended.
        /// </summary>
        CHANNEL_FLAG_SUSPEND = 0x00000020,

        /// <summary>
        ///  All virtual channel traffic MUST be resumed.
        /// </summary>
        CHANNEL_FLAG_RESUME = 0x00000040,

        /// <summary>
        ///  The virtual channel data is compressed. This value corresponds
        ///  to MPPC bit A (see [RFC2118] Common Details section).
        /// </summary>
        CHANNEL_PACKET_COMPRESSED = 0x00200000,

        /// <summary>
        ///  The decompressed packet MUST be placed at the beginning
        ///  of the history buffer. This value corresponds to MPPC
        ///  bit B (see [RFC2118] Common Details section).
        /// </summary>
        CHANNEL_PACKET_AT_FRONT = 0x00400000,

        /// <summary>
        ///  The history buffer MUST be reinitialized. This value
        ///  corresponds to MPPC bit C (see [RFC2118] Common Details
        ///  section).
        /// </summary>
        CHANNEL_PACKET_FLUSHED = 0x00800000,

        /// <summary>
        ///  Indicates the compression package which was used to
        ///  compress the data. See the discussion which follows
        ///  this table for a list of compression packages.
        /// </summary>
        CompressionTypeMask = 0x000F0000,
    }

    /// <summary>
    ///  The TS_BITMAP_CAPABILITYSET structure is used to
    ///  advertise bitmap-orientated characteristics and is
    ///  based on the capability set specified in [T128] section
    ///  8.2.4. This capability is sent by both client and server.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1_2.xml
    //  </remarks>
    public partial struct TS_BITMAP_CAPABILITYSET : ITsCapsSet
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The type of the capability
        ///  set. This field MUST be set to CAPSTYPE_BITMAP (2).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  A 16-bit unsigned integer. Preferred bits-per-pixel
        ///  (bpp) for the session. In RDP 4.0 and 5.0, this field
        ///  MUST be set to 8 (even for a 16-color session). In
        ///  RDP 5.1 and later, this field MUST be set to the desktop
        ///  color depth that the client requested in the Client
        ///  Core Data (section ).
        /// </summary>
        public ushort preferredBitsPerPixel;

        /// <summary>
        ///  A 16-bit unsigned integer. Indicates whether the client
        ///  can receive 1 bit-per-pixel. This field is ignored
        ///  during capability negotiation and SHOULD be set to
        ///  TRUE (0x0001).
        /// </summary>
        public ushort receive1BitPerPixel;

        /// <summary>
        ///  A 16-bit unsigned integer. Indicates whether the client
        ///  can receive 4 bit-per-pixel. This field is ignored
        ///  during capability negotiation and SHOULD be set to
        ///  TRUE (0x0001).
        /// </summary>
        public ushort receive4BitsPerPixel;

        /// <summary>
        ///  A 16-bit unsigned integer. Indicates whether the client
        ///  can receive 8 bit-per-pixel. This field is ignored
        ///  during capability negotiation and SHOULD be set to
        ///  TRUE (0x0001).
        /// </summary>
        public ushort receive8BitsPerPixel;

        /// <summary>
        ///  A 16-bit unsigned integer. The width of the client desktop.
        ///  This field MAY be set to the desktop width that the
        ///  client requested in the Client Core Data (section ).
        /// </summary>
        public ushort desktopWidth;

        /// <summary>
        ///  A 16-bit unsigned integer. The height of the client
        ///  desktop. This field MAY be set to the desktop height
        ///  that the client requested in the Client Core Data (section
        ///  ).
        /// </summary>
        public ushort desktopHeight;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2octets;

        /// <summary>
        ///  A 16-bit unsigned integer. Indicates whether desktop
        ///  resizing is supported.
        /// </summary>
        public desktopResizeFlag_Values desktopResizeFlag;

        /// <summary>
        ///  A 16-bit unsigned integer. Indicates whether the client
        ///  supports bitmap compression. RDP requires bitmap compression
        ///  and hence this field MUST be set to TRUE (0x0001).
        ///  If it is not set to TRUE, the server MUST NOT continue
        ///  with the connection.
        /// </summary>
        public ushort bitmapCompressionFlag;

        /// <summary>
        ///  An 8-bit unsigned integer. Client support for 16 bits-per-pixel
        ///  color modes. This field is ignored during capability
        ///  negotiation and SHOULD be set to 0.
        /// </summary>
        public byte highColorFlags;

        /// <summary>
        ///  An 8-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public drawingFlags_Values drawingFlags;

        /// <summary>
        ///  A 16-bit unsigned integer. Indicates whether the client
        ///  supports the use of multiple bitmap rectangles. RDP
        ///  requires the use of multiple bitmap rectangles and
        ///  hence this field MUST be set to TRUE (0x0001). If it
        ///  is not set to TRUE, the server MUST NOT continue with
        ///  the connection.
        /// </summary>
        public ushort multipleRectangleSupport;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2octetsB;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return RdpbcgrUtility.StructToBytes(this);
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    /// The type of desktopResizeFlag.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum desktopResizeFlag_Values : ushort
    {

        /// <summary>
        ///  Desktop resizing is not supported.
        /// </summary>
        FALSE = 0x0000,

        /// <summary>
        ///  Desktop resizing is supported.
        /// </summary>
        TRUE = 0x0001,
    }

    /// <summary>
    /// The type of drawingFlags.
    /// </summary>
    public enum drawingFlags_Values : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates support for lossy compression of 32 bpp bitmaps
        ///  by reducing color-fidelity on a per-pixel basis.
        /// </summary>
        DRAW_ALLOW_DYNAMIC_COLOR_FIDELITY = 0x02,

        /// <summary>
        ///  Indicates support for chroma subsampling when compressing
        ///  32 bpp bitmaps.
        /// </summary>
        DRAW_ALLOW_COLOR_SUBSAMPLING = 0x04,

        /// <summary>
        ///  Indicates that the client supports the removal of the
        ///  alpha-channel when compressing 32 bpp bitmaps. In this
        ///  case the alpha is assumed to be 0xFF, that is, the
        ///  bitmap is opaque.
        /// </summary>
        DRAW_ALLOW_SKIP_ALPHA = 0x08,

        /// <summary>
        /// An unused flag that MUST be ignored by the client 
        /// if it is present in the server-to-client Bitmap Capability Set.
        /// </summary>
        DRAW_UNUSED_FLAG = 0x10,

    }

    /// <summary>
    /// The type of orderSupportExFlags.
    /// </summary>
    public enum orderSupportExFlags_values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// The Cache Bitmap (Revision 3) Secondary Drawing Order 
        /// ([MS-RDPEGDI] section 2.2.2.2.1.2.8) is supported.
        /// </summary>
        ORDERFLAGS_EX_CACHE_BITMAP_REV3_SUPPORT = 0x0002,

        /// <summary>
        /// The Frame Marker Alternate Secondary Drawing Order
        /// ([MS-RDPEGDI] section 2.2.2.2.1.3.7) is supported.
        /// </summary>
        ORDERFLAGS_EX_ALTSEC_FRAME_MARKER_SUPPORT = 0x0004,
    }

    /// <summary>
    ///  The TS_ORDER_CAPABILITYSET structure advertises support
    ///  for primary drawing order-related capabilities and
    ///  is based on the capability set specified in [T128]
    ///  section 8.2.5 (for more information about primary drawing
    ///  orders, see [MS-RDPEGDI] section ). This capability
    ///  is sent by both client and server.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1_3.xml
    //  </remarks>
    public partial struct TS_ORDER_CAPABILITYSET : ITsCapsSet
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The type of the capability
        ///  set. This field MUST be set to CAPSTYPE_ORDER (3).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  A 16 element  array of 8-bit unsigned integers. Terminal
        ///  descriptor. This field is ignored during capability
        ///  negotiation and SHOULD be set to all zeros.
        /// </summary>
        public byte[] terminalDescriptor;

        /// <summary>
        ///  A 32-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public uint pad4octetsA;

        /// <summary>
        ///  A 16-bit unsigned integer. X granularity used in conjunction
        ///  with the SaveBitmap Primary Drawing Order (see [MS-RDPEGDI]
        ///  section ). This value is ignored during capability
        ///  negotiation and assumed to be 1.
        /// </summary>
        public ushort desktopSaveXGranularity;

        /// <summary>
        ///  A 16-bit unsigned integer. Y granularity used in conjunction
        ///  with the SaveBitmap Primary Drawing Order (see [MS-RDPEGDI]
        ///  section ). This value is ignored during capability
        ///  negotiation and assumed to be 20.
        /// </summary>
        public ushort desktopSaveYGranularity;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2octetsA;

        /// <summary>
        ///  A 16-bit unsigned integer. Maximum order level. This
        ///  value is ignored during capability negotiation and
        ///  SHOULD be set to ORD_LEVEL_1_ORDERS (1).
        /// </summary>
        public ushort maximumOrderLevel;

        /// <summary>
        ///  A 16-bit unsigned integer. Number of fonts. This value
        ///  is ignored during capability negotiation and SHOULD
        ///  be set to 0.
        /// </summary>
        public ushort numberFonts;

        /// <summary>
        ///  A 16-bit unsigned integer. A 16-bit unsigned integer.
        ///  Support for drawing order options.
        /// </summary>
        public orderFlags_Values orderFlags;

        /// <summary>
        ///  An array of 32 bytes indicating support for various
        ///  primary drawing orders. The indices of this array are
        ///  the negotiation indices for the primary orders specified
        ///  in [MS-RDPEGDI] section .Negotiation IndexPrimary Drawing
        ///  Order or OrdersTS_NEG_DSTBLT_INDEX0x00DstBlt Primary
        ///  Drawing Order (see  [MS-RDPEGDI] section ).TS_NEG_PATBLT_INDEX0x01PatBlt
        ///  Primary Drawing Order (see  [MS-RDPEGDI] section )
        ///  and OpaqueRect Primary Drawing Order (see [MS-RDPEGDI]
        ///  section ).TS_NEG_SCRBLT_INDEX0x02ScrBlt Primary Drawing
        ///  Order (see  [MS-RDPEGDI] section ).TS_NEG_MEMBLT_INDEX0x03MemBlt
        ///  Primary Drawing Order (see  [MS-RDPEGDI] section ).TS_NEG_MEM3BLT_INDEX0x04Mem3Blt
        ///  Primary Drawing Order (see  [MS-RDPEGDI] section ).Unused
        ///  Index0x05The contents of the byte at this index are
        ///  ignored.Unused Index0x06The contents of the byte at
        ///  this index are ignored.TS_NEG_DRAWNINEGRID_INDEX0x07DrawNineGrid
        ///  Primary Drawing Order (see  [MS-RDPEGDI] section ).TS_NEG_LINETO_INDEX0x08LineTo
        ///  Primary Drawing Order (see  [MS-RDPEGDI] section ).TS_NEG_MULTI_DRAWNINEGRID_INDEX0x09MultiDrawNineGrid
        ///  Primary Drawing Order (see [MS-RDPEGDI] section ).Unused
        ///  Index0x0AThe contents of the byte at this index are
        ///  ignored.TS_NEG_SAVEBITMAP_INDEX0x0BSaveBitmap Primary
        ///  Drawing Order (see  [MS-RDPEGDI] section ).Unused Index0x0CThe
        ///  contents of the byte at this index are ignored.Unused
        ///  Index0x0DThe contents of the byte at this index are
        ///  ignored.Unused Index0x0EThe contents of the byte at
        ///  this index are ignored.TS_NEG_MULTIDSTBLT_INDEX0x0FMultiDstBlt
        ///  Primary Drawing Order (see [MS-RDPEGDI] section ).TS_NEG_MULTIPATBLT_INDEX0x10MultiPatBlt
        ///  Primary Drawing Order (see [MS-RDPEGDI] section ).TS_NEG_MULTISCRBLT_INDEX0x11MultiScrBlt
        ///  Primary Drawing Order (see [MS-RDPEGDI] section ).TS_NEG_MULTIOPAQUERECT_INDEX0x12MultiOpaqueRect
        ///  Primary Drawing Order (see [MS-RDPEGDI] section ).TS_NEG_FAST_INDEX_INDEX0x13FastIndex
        ///  Primary Drawing Order (see [MS-RDPEGDI] section ).TS_NEG_POLYGON_SC_INDEX0x14PolygonSC
        ///  Primary Drawing Order (see [MS-RDPEGDI] section ) and
        ///  PolygonCB Primary Drawing Order (see [MS-RDPEGDI] section
        ///  ).TS_NEG_POLYGON_CB_INDEX0x15PolygonCB Primary Drawing
        ///  Order (see [MS-RDPEGDI] section ) and PolygonSC Primary
        ///  Drawing Order (see [MS-RDPEGDI] section ).TS_NEG_POLYLINE_INDEX0x16Polyline
        ///  Primary Drawing Order (see [MS-RDPEGDI] section ).Unused
        ///  Index0x17The contents of the byte at this index are
        ///  ignored.TS_NEG_FAST_GLYPH_INDEX0x18FastGlyph Primary
        ///  Drawing Order (see [MS-RDPEGDI] section ).TS_NEG_ELLIPSE_SC_INDEX0x19EllipseSC
        ///  Primary Drawing Order (see [MS-RDPEGDI] section ) and
        ///  EllipseCB Primary Drawing Order (see [MS-RDPEGDI] section
        ///  ).TS_NEG_ELLIPSE_CB_INDEX0x1AEllipseCB Primary Drawing
        ///  Order (see [MS-RDPEGDI] section ) and EllipseSC Primary
        ///  Drawing Order (see [MS-RDPEGDI] section ).TS_NEG_INDEX_INDEX0x1BGlyphIndex
        ///  Primary Drawing Order (see [MS-RDPEGDI] section ).Unused
        ///  Index0x1CThe contents of the byte at this index are
        ///  ignored.Unused Index0x1DThe contents of the byte at
        ///  this index are ignored.Unused Index0x1EThe contents
        ///  of the byte at this index are ignored.Unused Index0x1FThe
        ///  contents of the byte at this index are ignored.
        /// </summary>
        public byte[] orderSupport;

        /// <summary>
        ///  A 16-bit unsigned integer. Support for text options.
        ///  This value is ignored during capability negotiation
        ///  and SHOULD be set to 0.
        /// </summary>
        public ushort textFlags;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public orderSupportExFlags_values orderSupportExFlags;

        /// <summary>
        ///  A 32-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public uint pad4octetsB;

        /// <summary>
        ///  A 32-bit unsigned integer. The maximum usable size of
        ///  bitmap space for bitmap packing in the SaveBitmap Primary
        ///  Drawing Order (see  [MS-RDPEGDI] section ). This field
        ///  is ignored by the client and assumed to be 230400 bytes
        ///  (480 * 480).
        /// </summary>
        public uint desktopSaveSize;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2octetsC;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2octetsD;

        /// <summary>
        ///  A 16-bit unsigned integer. ANSI codepage descriptor
        ///  being used by the client (for a list of code pages,
        ///  see [MSDN-CP]). This field is ignored by the client
        ///  and SHOULD be set to 0 by the server.
        /// </summary>
        public ushort textANSICodePage;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2octetsE;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            List<byte> encodeBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(encodeBuffer, (ushort)capabilitySetType);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, lengthCapability);
            RdpbcgrEncoder.EncodeBytes(encodeBuffer, terminalDescriptor);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, pad4octetsA);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, desktopSaveXGranularity);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, desktopSaveYGranularity);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, pad2octetsA);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, maximumOrderLevel);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, numberFonts);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, (ushort)orderFlags);
            RdpbcgrEncoder.EncodeBytes(encodeBuffer, orderSupport);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, textFlags);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, (ushort)orderSupportExFlags);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, pad4octetsB);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, desktopSaveSize);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, pad2octetsC);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, pad2octetsD);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, textANSICodePage);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, pad2octetsE);
            return encodeBuffer.ToArray();
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    /// The type of orderFlags.
    /// </summary>
    public enum orderFlags_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates support for negotiating drawing orders in
        ///  the orderSupport field. This flag MUST be set in the
        ///  orderFlags field.
        /// </summary>
        NEGOTIATEORDERSUPPORT = 0x0002,

        /// <summary>
        ///  Indicates support for the order encoding flag for zero
        ///  bounds delta coordinates (see [MS-RDPEGDI] section
        ///  ). This flag MUST be set in the orderFlags field.
        /// </summary>
        ZEROBOUNDSDELTASSUPPORT = 0x0008,

        /// <summary>
        ///  Indicates support for sending color indices (not RGB
        ///  values) in orders.
        /// </summary>
        COLORINDEXSUPPORT = 0x0020,

        /// <summary>
        ///  Indicates that this party can receive only solid and
        ///  pattern brushes.
        /// </summary>
        SOLIDPATTERNBRUSHONLY = 0x0040,

        /// <summary>
        /// Indicates that the orderSupportExFlags field contains valid data.
        /// </summary>
        ORDERFLAGS_EXTRA_FLAGS = 0x0080,
    }

    /// <summary>
    ///  The TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET
    ///  structure is used to advertise support for persistent
    ///  bitmap caching (see [MS-RDPEGDI] section 3.1.1.1.1).
    ///  This capability set is only sent from server to client.
    ///             
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1_4.xml
    //  </remarks>
    public partial struct TS_BITMAPCACHE_HOSTSUPPORT_CAPABILITYSET : ITsCapsSet
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The type of the capability
        ///  set. This field MUST be set to CAPSTYPE_BITMAPCACHE_HOSTSUPPORT
        ///  (18).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  An 8-bit unsigned integer. Cache version. This field
        ///  MUST be set to TS_BITMAPCACHE_REV2 (0x01), which implies
        ///  at a Revision 2 Bitmap Cache (see [MS-RDPEGDI] section
        ///  3.1.1.1.1).
        /// </summary>
        public cacheVersion_Values cacheVersion;

        /// <summary>
        ///  An 8-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public byte pad1;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return RdpbcgrUtility.StructToBytes(this);
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    /// The type of cacheVersion.
    /// </summary>
    public enum cacheVersion_Values : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x01,
    }

    /// <summary>
    ///  The TS_BITMAPCACHE_CAPABILITYSET structure is used to
    ///  advertise support for Revision 1 Bitmap Caches (see
    ///   [MS-RDPEGDI] section 3.1.1.1.1). This capability is
    ///  only sent from client to server. In addition to specifying
    ///  bitmap caching parameters in the Revision 1 Bitmap
    ///  Cache Capability Set, a client MUST also support the
    ///  MemBlt and Mem3Blt Primary Drawing Orders (see [MS-RDPEGDI]
    ///  sections 2.2.2.2.1.1.2.9 and 2.2.2.2.1.1.2.10 respectively)
    ///  in order to receive the Cache Bitmap (Revision 1) Secondary
    ///  Drawing Order (see [MS-RDPEGDI] section 2.2.2.2.1.2.2).
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1_5_1.xml
    //  </remarks>
    public partial struct TS_BITMAPCACHE_CAPABILITYSET : ITsCapsSet
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The type of the capability
        ///  set. This field MUST be set to CAPSTYPE_BITMAPCACHE
        ///  (4).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///               A 16-bit unsigned integer. The length in
        ///  bytes of the capability data, including the size of
        ///  the capabilitySetType and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  A 32-bit unsigned integer.  Padding. Values in this
        ///  field are ignored.
        /// </summary>
        public uint pad1;

        /// <summary>
        ///  A 32-bit unsigned integer.  Padding. Values in this
        ///  field are ignored.
        /// </summary>
        public uint pad2;

        /// <summary>
        ///  A 32-bit unsigned integer.  Padding. Values in this
        ///  field are ignored.
        /// </summary>
        public uint pad3;

        /// <summary>
        ///  A 32-bit unsigned integer.  Padding. Values in this
        ///  field are ignored.
        /// </summary>
        public uint pad4;

        /// <summary>
        ///  A 32-bit unsigned integer.  Padding. Values in this
        ///  field are ignored.
        /// </summary>
        public uint pad5;

        /// <summary>
        ///  A 32-bit unsigned integer.  Padding. Values in this
        ///  field are ignored.
        /// </summary>
        public uint pad6;

        /// <summary>
        ///  A 16-bit unsigned integer. The number of entries in
        ///  Bitmap Cache 1 (maximum allowed value is 200 entries).
        /// </summary>
        public ushort Cache1Entries;

        /// <summary>
        ///  A 16-bit unsigned integer. The maximum cell size in
        ///  Bitmap Cache 1. This field SHOULD be set to 256, corresponding
        ///  to the number of pixels in a 16 x 16 bitmap.
        /// </summary>
        public ushort Cache1MaximumCellSize;

        /// <summary>
        ///  A 16-bit unsigned integer. The number of entries in
        ///  Bitmap Cache 2 (maximum allowed value is 600 entries).
        /// </summary>   
        public ushort Cache2Entries;

        /// <summary>
        ///  A 16-bit unsigned integer. The maximum cell size in
        ///  Bitmap Cache 2. This field SHOULD be set to 1024, corresponding
        ///  to the number of pixels in a 32 x 32 bitmap.
        /// </summary>
        public ushort Cache2MaximumCellSize;

        /// <summary>
        ///  A 16-bit unsigned integer. The number of entries in
        ///  Bitmap Cache 3 (maximum allowed value is 65535 entries).
        /// </summary>
        public ushort Cache3Entries;

        /// <summary>
        ///  A 16-bit unsigned integer. The maximum cell size in
        ///  Bitmap Cache 3. This field SHOULD be set to 4096, corresponding
        ///  to the number of pixels in a 64 x 64 bitmap.
        /// </summary>
        public ushort Cache3MaximumCellSize;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return RdpbcgrUtility.StructToBytes(this);
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    ///  The TS_BITMAPCACHE_CAPABILITYSET_REV2 structure is used
    ///  to advertise support for Revision 2 Bitmap Caches (see
    ///  [MS-RDPEGDI] section 3.1.1.1.1). This capability is
    ///  only sent from client to server. In addition to specifying
    ///  bitmap caching parameters in the Revision 2 Bitmap
    ///  Cache Capability Set, a client MUST also support the
    ///  MemBlt and Mem3Blt Primary Drawing Orders (see [MS-RDPEGDI]
    ///  sections 2.2.2.2.1.1.2.9 and 2.2.2.2.1.1.2.10 respectively)
    ///  in order to receive the Cache Bitmap (Revision 2) Secondary
    ///  Drawing Order (see [MS-RDPEGDI] section 2.2.2.2.1.2.3).
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1_5_2.xml
    //  </remarks>
    public partial struct TS_BITMAPCACHE_CAPABILITYSET_REV2 : ITsCapsSet
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The type of the capability
        ///  set. This field MUST be set to CAPSTYPE_BITMAPCACHE_REV2
        ///  (19).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  A 16-bit unsigned integer. Properties which apply to
        ///  all the bitmap caches.
        /// </summary>
        public CacheFlags_Values CacheFlags;

        /// <summary>
        ///  An 8-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public byte pad2;

        /// <summary>
        ///  An 8-bit unsigned integer. Number of bitmap caches (with
        ///  a maximum allowed value of 5). This field SHOULD be
        ///  set to 3. Note that the bitmap cache cell sizes are
        ///  not specified; they are assumed to be 256, 1024, and
        ///  4096 pixels, in order.
        /// </summary>
        public byte NumCellCaches;

        /// <summary>
        ///  An array of 5 TS_BITMAPCACHE_CELL_CACHE_INFO structures.
        ///  Contains information about each of the different caches.
        ///  The number of valid elements in the array is given
        ///  by the NumCellCaches field.
        /// </summary>
        public TS_BITMAPCACHE_CELL_CACHE_INFO BitmapCache1CellInfo;

        /// <summary>
        ///  An array of 5 TS_BITMAPCACHE_CELL_CACHE_INFO structures.
        ///  Contains information about each of the different caches.
        ///  The number of valid elements in the array is given
        ///  by the NumCellCaches field.
        /// </summary>
        public TS_BITMAPCACHE_CELL_CACHE_INFO BitmapCache2CellInfo;

        /// <summary>
        ///  An array of 5 TS_BITMAPCACHE_CELL_CACHE_INFO structures.
        ///  Contains information about each of the different caches.
        ///  The number of valid elements in the array is given
        ///  by the NumCellCaches field.
        /// </summary>
        public TS_BITMAPCACHE_CELL_CACHE_INFO BitmapCache3CellInfo;

        /// <summary>
        ///  An array of 5 TS_BITMAPCACHE_CELL_CACHE_INFO structures.
        ///  Contains information about each of the different caches.
        ///  The number of valid elements in the array is given
        ///  by the NumCellCaches field.
        /// </summary>
        public TS_BITMAPCACHE_CELL_CACHE_INFO BitmapCache4CellInfo;

        /// <summary>
        ///  An array of 5 TS_BITMAPCACHE_CELL_CACHE_INFO structures.
        ///  Contains information about each of the different caches.
        ///  The number of valid elements in the array is given
        ///  by the NumCellCaches field.
        /// </summary>
        public TS_BITMAPCACHE_CELL_CACHE_INFO BitmapCache5CellInfo;

        /// <summary>
        ///  A 12-element array of 8-bit, unsigned integers. Paddings.
        ///  Values in this field are ignored.
        /// </summary>
        [StaticSize(12)]
        public byte[] Pad3;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            List<byte> encodeBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(encodeBuffer, (ushort)capabilitySetType);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, lengthCapability);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, (ushort)CacheFlags);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, pad2);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, NumCellCaches);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, BitmapCache1CellInfo);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, BitmapCache2CellInfo);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, BitmapCache3CellInfo);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, BitmapCache4CellInfo);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, BitmapCache5CellInfo);
            RdpbcgrEncoder.EncodeBytes(encodeBuffer, Pad3);
            return encodeBuffer.ToArray();
        }


        /// <summary>
        /// get the type of this Capability Type
        /// </summary>
        /// <returns>the capabilitySetType_Values of this capability</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    /// The type of CacheFlags.
    /// </summary>
    [Flags()]
    public enum CacheFlags_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates that the client will send a Persistent Key
        ///  List PDU during the Connection Finalization phase of
        ///  the Standard RDP Connection Sequence (see section ).
        /// </summary>
        PERSISTENT_KEYS_EXPECTED_FLAG = 0x0001,

        /// <summary>
        ///  Indicates that the client supports a cache waiting list.
        ///  If a waiting list is supported, new bitmaps are cached
        ///  on the second hit rather than the first (bitmaps must
        ///  be sent twice before they are cached).
        /// </summary>
        ALLOW_CACHE_WAITING_LIST_FLAG = 0x0002,
    }

    /// <summary>
    ///  The TS_BITMAPCACHE_CELL_CACHE_INFO structure contains
    ///  information about a bitmap cache on the client. 				
    ///      
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1_5_2_1.xml
    //  </remarks>
    public partial struct TS_BITMAPCACHE_CELL_CACHE_INFO
    {

        /// <summary>
        ///  A 31-bit unsigned integer. Indicates the number of entries
        ///  in the cache.
        /// </summary>
        public uint NumEntriesAndK;
    }

    /// <summary>
    ///  The TS_POINTER_CAPABILITYSET structure advertises
    ///  pointer cache sizes and flags, and is based on the
    ///  capability set specified in [T128] section 8.2.11.
    ///  This capability is sent by both client and server.
    ///  				 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1_6.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 10)]
    public partial struct TS_POINTER_CAPABILITYSET : ITsCapsSet
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The type of the capability
        ///  set. This field MUST be set to CAPSTYPE_POINTER (8).
        /// </summary>
        [FieldOffset(0)]
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        [FieldOffset(2)]
        public ushort lengthCapability;

        /// <summary>
        ///  A 16-bit unsigned integer. Indicates support for a color
        ///  pointer. 
        /// </summary>
        [FieldOffset(4)]
        public colorPointerFlag_Values colorPointerFlag;

        /// <summary>
        ///  A 16-bit unsigned integer. The number of available slots
        ///  in the 24 bits-per-pixel color pointer cache used to
        ///  store data received in the Color Pointer Update.
        /// </summary>
        [FieldOffset(6)]
        public ushort colorPointerCacheSize;

        /// <summary>
        ///  A 16-bit unsigned integer. The number of available slots
        ///  in the pointer cache used to store pointer data of
        ///  arbitrary bit depth received in the New Pointer Update. If
        ///  the Pointer Capability Set sent from the client does
        ///  not include this field, the server will not use the
        ///  New Pointer Update.
        /// </summary>
        [FieldOffset(8)]
        public ushort pointerCacheSize;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return RdpbcgrUtility.StructToBytes(this);
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    /// The type of colorPointerFlag.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum colorPointerFlag_Values : ushort
    {

        /// <summary>
        ///  Monochrome mouse cursors are supported.
        /// </summary>
        FALSE = 0x0000,

        /// <summary>
        ///  Color mouse cursors are supported.
        /// </summary>
        TRUE = 0x0001,
    }

    /// <summary>
    ///  The TS_INPUT_CAPABILITYSET structure is used to
    ///  advertise support for input formats and devices. This
    ///  capability is sent by both client and server.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1_7.xml
    //  </remarks>
    public partial struct TS_INPUT_CAPABILITYSET : ITsCapsSet
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The type of the capability
        ///  set. This field MUST be set to CAPSTYPE_INPUT (13).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  A 16-bit unsigned integer. Input support flags.
        /// </summary>
        public inputFlags_Values inputFlags;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2octetsA;

        /// <summary>
        ///  A 32-bit unsigned integer. Keyboard layout (active input
        ///  locale identifier). For a list of possible input locales
        ///  refer to [MSDN-MUI]. This value is only specified in
        ///  the client Input Capability Set and should correspond
        ///  with that sent in the Client Core Data.
        /// </summary>
        public uint keyboardLayout;

        /// <summary>
        ///  A 32-bit unsigned integer. Keyboard type.
        /// </summary>
        public TS_INPUT_CAPABILITYSET_keyboardType_Values keyboardType;

        /// <summary>
        ///               A 32-bit unsigned integer. Keyboard subtype
        ///  (an original equipment manufacturer-dependent value).
        ///  This value is only specified in the client Input Capability
        ///  Set and should correspond with that sent in the Client
        ///  Core Data.
        /// </summary>
        public uint keyboardSubType;

        /// <summary>
        ///  A 32-bit unsigned integer. Number of function keys on
        ///  the keyboard. This value is only specified in the client
        ///  Input Capability Set and should correspond with that
        ///  sent in the Client Core Data.
        /// </summary>
        public uint keyboardFunctionKey;

        /// <summary>
        ///  A 64-byte field. Input Method Editor (IME) file name
        ///  associated with the input locale. This field contains
        ///  up to 31 Unicode characters plus a null terminator
        ///  and is only specified in the client Input Capability
        ///  Set (its contents should correspond with that sent
        ///  in the Client Core Data).
        /// </summary>
        public string imeFileName;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            List<byte> encodeBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(encodeBuffer, (ushort)capabilitySetType);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, lengthCapability);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, (ushort)inputFlags);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, pad2octetsA);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, keyboardLayout);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, (uint)keyboardType);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, keyboardSubType);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, keyboardFunctionKey);
            RdpbcgrEncoder.EncodeUnicodeString(encodeBuffer, imeFileName, ConstValue.INPUT_CAP_IME_FILENAME_SIZE);
            return encodeBuffer.ToArray();
        }


        /// <summary>
        /// get the type of this Capability Type
        /// </summary>
        /// <returns>the capabilitySetType_Values of this capability</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    /// The type of inputFlags.
    /// </summary>
    public enum inputFlags_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates support for using scancodes in the Keyboard
        ///  Event notifications (see sections  and ).
        /// </summary>
        INPUT_FLAG_SCANCODES = 0x0001,

        /// <summary>
        ///  Indicates support for Extended Mouse Event notifications
        ///  (see sections  and ).
        /// </summary>
        INPUT_FLAG_MOUSEX = 0x0004,

        /// <summary>
        ///  Advertised by RDP 5.0 and 5.1 servers. RDP 5.2 and later
        ///  servers advertise the INPUT_FLAG_FASTPATH_INPUT2 flag
        ///  to indicate support for fast-path input.
        /// </summary>
        INPUT_FLAG_FASTPATH_INPUT = 0x0008,

        /// <summary>
        ///  Indicates support for Unicode Keyboard Event notifications
        ///  (see sections  and ).
        /// </summary>
        INPUT_FLAG_UNICODE = 0x0010,

        /// <summary>
        ///  Advertised by RDP 5.2 and later servers. Clients that
        ///  do not support this flag (such as RDP 5.0 and 5.1 clients)
        ///  will not be able to use fast-path input when connecting
        ///  to RDP 5.2 and later servers.
        /// </summary>
        INPUT_FLAG_FASTPATH_INPUT2 = 0x0020,

        /// <summary>
        /// An unused flag that MUST be ignored by the client 
        /// if it is present in the server-to-client Input Capability Set.
        /// </summary>
        INPUT_FLAG_UNUSED1 = 0x0040,

        /// <summary>
        /// An unused flag that MUST be ignored by the server 
        /// if it is present in the client-to-server Input Capability Set.
        /// </summary>
        INPUT_FLAG_UNUSED2 = 0x0080,

        /// <summary>
        /// Indicates support for horizontal mouse wheel notifications 
        /// (see sections 2.2.8.1.1.3.1.1.3 and 2.2.8.1.2.2.3).
        /// </summary>
        TS_INPUT_FLAG_MOUSE_HWHEEL = 0x0100,

        /// <summary>
        /// Indicates support for Quality of Experience (QoE) Timestamp Event notifications
        /// (section 2.2.8.1.2.2.6).
        /// There is no slow-path support for Quality of Experience (QoE) timestamps.
        /// </summary>
        TS_INPUT_FLAG_QOE_TIMESTAMPS = 0x0200,

    }

    /// <summary>
    /// The type of keyboardType.
    /// </summary>
    public enum TS_INPUT_CAPABILITYSET_keyboardType_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  IBM PC/XT or compatible (83-key) keyboard
        /// </summary>
        V1 = 1,

        /// <summary>
        ///  Olivetti "ICO" (102-key) keyboard
        /// </summary>
        V2 = 2,

        /// <summary>
        ///  IBM PC/AT (84-key) or similar keyboard
        /// </summary>
        V3 = 3,

        /// <summary>
        ///  IBM enhanced (101- or 102-key) keyboard
        /// </summary>
        V4 = 4,

        /// <summary>
        ///  Nokia 1050 and similar keyboards
        /// </summary>
        V5 = 5,

        /// <summary>
        ///  Nokia 9140 and similar keyboards
        /// </summary>
        V6 = 6,

        /// <summary>
        ///  Japanese keyboard
        /// </summary>
        V7 = 7,
    }

    /// <summary>
    ///  The TS_BRUSH_CAPABILITYSET advertises client brush
    ///  support. This capability is only sent from client to
    ///  server. 				 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1_8.xml
    //  </remarks>
    public partial struct TS_BRUSH_CAPABILITYSET : ITsCapsSet
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The type of the capability
        ///  set. This field MUST be set to CAPSTYPE_BRUSH (15).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  A 32-bit unsigned integer. The maximum brush level supported
        ///  by the client.
        /// </summary>
        public brushSupportLevel_Values brushSupportLevel;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return RdpbcgrUtility.StructToBytes(this);
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    /// The type of brushSupportLevel.
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum brushSupportLevel_Values : uint
    {

        /// <summary>
        ///  Support for solid-color and monochrome pattern brushes
        ///  with no caching. This is an RDP 4.0 implementation.
        /// </summary>
        BRUSH_DEFAULT = 0x00000000,

        /// <summary>
        ///  Ability to handle color brushes (4 or 8 bit in RDP 5.0,
        ///  RDP 5.1 adds 16 and 24 bit) and caching. Brushes are
        ///  limited to 8-by-8 pixels.
        /// </summary>
        BRUSH_COLOR_8x8 = 0x00000001,

        /// <summary>
        ///  Ability to handle color brushes (4 or 8 bit in RDP 5.0,
        ///  RDP 5.1 adds 16 and 24 bit) and caching. Brushes can
        ///  have arbitrary dimensions.
        /// </summary>
        BRUSH_COLOR_FULL = 0x00000002,
    }

    /// <summary>
    ///  The TS_GLYPHCACHE_CAPABILITYSET structure advertises
    ///  the glyph support level and associated cache sizes.
    ///  This capability is only sent from client to server.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1_9.xml
    //  </remarks>
    public partial struct TS_GLYPHCACHE_CAPABILITYSET : ITsCapsSet
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The type of the capability
        ///  set. This field MUST be set to CAPSTYPE_GLYPHCACHE
        ///  (16).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  An array of 10 TS_CACHE_DEFINITION structures. Glyph
        ///  cache data, up to 10 elements. The maximum number of
        ///  entries allowed in a cache is 254, and the largest
        ///  allowed maximum size of an element is 2048 bytes.
        /// </summary>
        public TS_CACHE_DEFINITION[] GlyphCache;

        /// <summary>
        ///  Fragment cache data. The maximum number of entries allowed
        ///  in the cache is 256, and the largest allowed maximum
        ///  size of an element is 256 bytes.
        /// </summary>
        public TS_CACHE_DEFINITION FragCache;

        /// <summary>
        ///  A 16-bit unsigned integer. The level of glyph support.
        /// </summary>
        public GlyphSupportLevel_Values GlyphSupportLevel;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2octets;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            List<byte> encodeBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(encodeBuffer, (ushort)capabilitySetType);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, lengthCapability);
            foreach (TS_CACHE_DEFINITION cache in GlyphCache)
            {
                RdpbcgrEncoder.EncodeStructure(encodeBuffer, cache);
            }
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, FragCache);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, (ushort)GlyphSupportLevel);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, pad2octets);
            return encodeBuffer.ToArray();
        }


        /// <summary>
        /// get the type of this Capability Type
        /// </summary>
        /// <returns>the capabilitySetType_Values of this capability</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    /// The type of GlyphSupportLevel.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum GlyphSupportLevel_Values : ushort
    {

        /// <summary>
        ///  The client does not support glyph caching. All text
        ///  output will be sent to the client as expensive Bitmap
        ///  Updates (see sections  and ).
        /// </summary>
        GLYPH_SUPPORT_NONE = 0,

        /// <summary>
        ///  Indicates support for Revision 1 Cache Glyph Secondary
        ///  Drawing Orders (see [MS-RDPEGDI] section ).
        /// </summary>
        GLYPH_SUPPORT_PARTIAL = 1,

        /// <summary>
        ///  Indicates support for Revision 1 Cache Glyph Secondary
        ///  Drawing Orders (see  [MS-RDPEGDI] section ).
        /// </summary>
        GLYPH_SUPPORT_FULL = 2,

        /// <summary>
        ///  Indicates support for Revision 2 Cache Glyph Secondary
        ///  Drawing Orders (see [MS-RDPEGDI] section ).
        /// </summary>
        GLYPH_SUPPORT_ENCODE = 3,
    }

    /// <summary>
    ///  The TS_CACHE_DEFINITION structure specifies details
    ///  about a particular cache in the Glyph Capability Set
    ///  structure. 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_1_9_1.xml
    //  </remarks>
    public partial struct TS_CACHE_DEFINITION
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The number of entries in
        ///  the cache.
        /// </summary>
        public ushort CacheEntries;

        /// <summary>
        ///  A 16-bit unsigned integer. The maximum size in bytes
        ///  of an entry in the cache.
        /// </summary>
        public ushort CacheMaximumCellSize;
    }

    /// <summary>
    ///  The TS_CONTROL_CAPABILITYSET structure is used
    ///  by the client to advertise control capabilities and
    ///  is fully described in [T128] section 8.2.10. This capability
    ///  is only sent from client to server and the server ignores
    ///  its contents.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_2_1.xml
    //  </remarks>
    public partial struct TS_CONTROL_CAPABILITYSET : ITsCapsSet
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The type of the capability
        ///  set. This field MUST be set to CAPSTYPE_CONTROL (5).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  A 16-bit unsigned integer. This field SHOULD be set
        ///  to 0.
        /// </summary>
        public ushort controlFlags;

        /// <summary>
        ///  A 16-bit unsigned integer. This field SHOULD be set
        ///  to FALSE (0x0000).
        /// </summary>
        public ushort remoteDetachFlag;

        /// <summary>
        ///  A 16-bit unsigned integer. This field SHOULD be set
        ///  to CONTROLPRIORITY_NEVER (0x0002).
        /// </summary>
        public ushort controlInterest;

        /// <summary>
        ///  A 16-bit unsigned integer. This field SHOULD be set
        ///  to CONTROLPRIORITY_NEVER (0x0002).
        /// </summary>
        public ushort detachInterest;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return RdpbcgrUtility.StructToBytes(this);
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    ///  The TS_WINDOWACTIVATION_CAPABILITYSET structure
    ///  is used by the client to advertise window activation
    ///  characteristics capabilities and is fully specified
    ///  in [T128] section 8.2.9. This capability is only sent
    ///  from client to server and the server ignores its contents.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_2_2.xml
    //  </remarks>
    public partial struct TS_WINDOWACTIVATION_CAPABILITYSET : ITsCapsSet
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The type of the capability
        ///  set. This field MUST be set to CAPSTYPE_ACTIVATION
        ///  (7).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  A 16-bit unsigned integer. This field SHOULD be set
        ///  to FALSE (0x0000).
        /// </summary>
        public ushort helpKeyFlag;

        /// <summary>
        ///  A 16-bit unsigned integer. This field SHOULD be set
        ///  to FALSE (0x0000).
        /// </summary>
        public ushort helpKeyIndexFlag;

        /// <summary>
        ///  A 16-bit unsigned integer. This field SHOULD be set
        ///  to FALSE (0x0000).
        /// </summary>
        public ushort helpExtendedKeyFlag;

        /// <summary>
        ///  A 16-bit unsigned integer. This field SHOULD be set
        ///  to FALSE (0x0000).
        /// </summary>
        public ushort windowManagerKeyFlag;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return RdpbcgrUtility.StructToBytes(this);
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    ///  The TS_SHARE_CAPABILITYSET structure is used to
    ///  advertise the channel ID of the sender and is fully
    ///  specified in [T128] section 8.2.12. This capability
    ///  is sent by both client and server.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_2_3.xml
    //  </remarks>
    public partial struct TS_SHARE_CAPABILITYSET : ITsCapsSet
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The type of the capability
        ///  set. This field MUST be set to CAPSTYPE_SHARE (9).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  A 16-bit unsigned integer. This field SHOULD be set
        ///  to 0 by the client and to the server channel ID by
        ///  the server (in Microsoft RDP server implementations,
        ///  this value is always 0x03EA).
        /// </summary>
        public ushort nodeId;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2octets;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return RdpbcgrUtility.StructToBytes(this);
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    ///  The TS_FONT_CAPABILITYSET structure is used to
    ///  advertise font support options. This capability is
    ///  sent by both client and server.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_6_2_4.xml
    //  </remarks>
    public partial struct TS_FONT_CAPABILITYSET : ITsCapsSet
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The type of the capability
        ///  set. This field MUST be set to CAPSTYPE_FONT (14).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  A 16-bit unsigned integer. The font support options.
        ///  This field SHOULD be set to FONTSUPPORT_FONTLIST (0x0001).
        /// </summary>
        public ushort fontSupportFlags;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2octets;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return RdpbcgrUtility.StructToBytes(this);
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    ///  The TS_SHARECONTROLHEADER header is a T.128 legacy
    ///  mode header (see [T128] section 8.3) present in slow-path
    ///  I/O packets.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_1_1_1.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 6)]
    public partial struct TS_SHARECONTROLHEADER
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The total length of the packet
        ///  in bytes (the length includes the size of the Share
        ///  Control Header).
        /// </summary>
        [FieldOffset(0)]
        public ushort totalLength;

        /// <summary>
        ///  A 16-bit unsigned integer. It contains the PDU type
        ///  and protocol version information. The format of the
        ///  pduType word is described by the following bitmask
        ///  diagram:
        /// </summary>
        [FieldOffset(2)]
        public nested_TS_SHARECONTROLHEADER_pduType pduType;

        /// <summary>
        ///  A 16-bit unsigned integer. The channel ID which is the
        ///  transmission source of the PDU.
        /// </summary>
        [FieldOffset(4)]
        public ushort pduSource;
    }

    /// <summary>
    ///  nested_TS_SHARECONTROLHEADER_pduType nested field.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_1_1_1.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public partial struct nested_TS_SHARECONTROLHEADER_pduType
    {
        /// <summary>
        ///  Least significant 4 bits of the least significant byte.
        ///  This field MUST be set to TS_PROTOCOL_VERSION (0x1).
        /// </summary>
        [FieldOffset(0)]
        public byte typeAndVersionLow;

        /// <summary>
        ///  Most significant byte. This field MUST be set to 0x00.
        /// </summary>
        [FieldOffset(1)]
        public versionHigh_Values versionHigh;
    }

    /// <summary>
    /// The type of Share Control Header.
    /// </summary>
    public enum ShareControlHeaderType : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Demand Active PDU.
        /// </summary>
        PDUTYPE_DEMANDACTIVEPDU = 1,

        /// <summary>
        /// Confirm Active PDU.
        /// </summary>
        PDUTYPE_CONFIRMACTIVEPDU = 3,

        /// <summary>
        /// Deactivate All PDU.
        /// </summary>
        PDUTYPE_DEACTIVATEALLPDU = 6,

        /// <summary>
        /// Data PDU (actual type is revealed by the pduType2 field in the Share Data Header
        /// (section 2.2.8.1.1.1.2) structure).
        /// </summary>
        PDUTYPE_DATAPDU = 7,

        /// <summary>
        /// Enhanced Security Server Redirection PDU.
        /// </summary>
        PDUTYPE_SERVER_REDIR_PKT = 10,
    }

    /// <summary>
    /// Most significant 4 bits of the least significant byte. 
    /// This field MUST be set to TS_PROTOCOL_VERSION (0x1).
    /// </summary>
    public enum ControlHeaderVersionLow : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Most significant 4 bits of the least significant byte. 
        /// </summary>
        TS_PROTOCOL_VERSION = 0x10,
    }

    /// <summary>
    /// Most significant byte. This field MUST be set to 0x00.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum versionHigh_Values : byte
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x00,
    }

    /// <summary>
    ///  The TS_SHAREDATAHEADER header is a T.128 legacy
    ///  mode header (see [T128] section 8.3) present in slow-path
    ///  I/O packets. 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_1_1_2.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 18)]
    public partial struct TS_SHAREDATAHEADER
    {
        /// <summary>
        ///  Share Control Header containing information about the
        ///  packet.
        /// </summary>
        [FieldOffset(0)]
        public TS_SHARECONTROLHEADER shareControlHeader;

        /// <summary>
        ///  A 32-bit unsigned integer.  Share identifier for the
        ///  packet (see [T128] section 8.4.2 for more information
        ///  about share IDs).
        /// </summary>
        [FieldOffset(6)]
        public uint shareId;

        /// <summary>
        ///  An 8-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        [FieldOffset(10)]
        public byte pad1;

        /// <summary>
        ///  An 8-bit unsigned integer. The stream identifier for
        ///  the packet.
        /// </summary>
        [FieldOffset(11)]
        public streamId_Values streamId;

        /// <summary>
        ///  A 16-bit unsigned integer. The uncompressed length of
        ///  the packet in bytes.
        /// </summary>
        [FieldOffset(12)]
        public ushort uncompressedLength;

        /// <summary>
        ///  An 8-bit unsigned integer. The type of data PDU.
        /// </summary>
        [FieldOffset(14)]
        public pduType2_Values pduType2;

        /// <summary>
        ///  An 8-bit unsigned integer. The compression type and
        ///  flags specifying the data following the Share Data
        ///  Header.
        /// </summary>
        [FieldOffset(15)]
        public compressedType_Values compressedType;

        /// <summary>
        ///  A 16-bit unsigned integer. The compressed length of
        ///  the packet in bytes.
        /// </summary>
        [FieldOffset(16)]
        public ushort compressedLength;
    }

    /// <summary>
    /// The type of streamId.
    /// </summary>
    [Flags()]
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum streamId_Values : byte
    {
        /// <summary>
        ///  Undefined stream priority. Refer to test suite bug #8341151
        /// </summary>
        STREAM_UNDEFINED = 0x00,

        /// <summary>
        ///  Low-priority stream.
        /// </summary>
        STREAM_LOW = 0x01,

        /// <summary>
        ///  Medium-priority stream.
        /// </summary>
        STREAM_MED = 0x02,

        /// <summary>
        ///  High-priority stream.
        /// </summary>
        STREAM_HI = 0x04,
    }

    /// <summary>
    /// The type of pduType2.
    /// </summary>
    public enum pduType2_Values : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Update PDU
        /// </summary>
        PDUTYPE2_UPDATE = 2,

        /// <summary>
        ///  Control PDU
        /// </summary>
        PDUTYPE2_CONTROL = 20,

        /// <summary>
        ///  Pointer Update PDU
        /// </summary>
        PDUTYPE2_POINTER = 27,

        /// <summary>
        ///  Input PDU
        /// </summary>
        PDUTYPE2_INPUT = 28,

        /// <summary>
        ///  Synchronize PDU
        /// </summary>
        PDUTYPE2_SYNCHRONIZE = 31,

        /// <summary>
        ///  Refresh Rect PDU
        /// </summary>
        PDUTYPE2_REFRESH_RECT = 33,

        /// <summary>
        ///  Play Sound PDU
        /// </summary>
        PDUTYPE2_PLAY_SOUND = 34,

        /// <summary>
        ///  Suppress Output PDU
        /// </summary>
        PDUTYPE2_SUPPRESS_OUTPUT = 35,

        /// <summary>
        ///  Shutdown Request PDU
        /// </summary>
        PDUTYPE2_SHUTDOWN_REQUEST = 36,

        /// <summary>
        ///  Shutdown Request Denied PDU
        /// </summary>
        PDUTYPE2_SHUTDOWN_DENIED = 37,

        /// <summary>
        ///  Save Session Info PDU
        /// </summary>
        PDUTYPE2_SAVE_SESSION_INFO = 38,

        /// <summary>
        ///  Font List PDU
        /// </summary>
        PDUTYPE2_FONTLIST = 39,

        /// <summary>
        ///  Font Map PDU
        /// </summary>
        PDUTYPE2_FONTMAP = 40,

        /// <summary>
        ///  Set Keyboard Indicators PDU
        /// </summary>
        PDUTYPE2_SET_KEYBOARD_INDICATORS = 41,

        /// <summary>
        ///  Persistent Key List PDU
        /// </summary>
        PDUTYPE2_BITMAPCACHE_PERSISTENT_LIST = 43,

        /// <summary>
        ///  Bitmap Cache Error PDU (see [MS-RDPEGDI] section ).
        /// </summary>
        PDUTYPE2_BITMAPCACHE_ERROR_PDU = 44,

        /// <summary>
        ///  Set Keyboard IME Status PDU
        /// </summary>
        PDUTYPE2_SET_KEYBOARD_IME_STATUS = 45,

        /// <summary>
        ///  Offscreen Bitmap Cache Error PDU (see  [MS-RDPEGDI]
        ///  section ).
        /// </summary>
        PDUTYPE2_OFFSCRCACHE_ERROR_PDU = 46,

        /// <summary>
        ///  Set Error Info PDU
        /// </summary>
        PDUTYPE2_SET_ERROR_INFO_PDU = 47,

        /// <summary>
        ///  DrawNineGrid Cache Error PDU (see  [MS-RDPEGDI] section
        ///  ).
        /// </summary>
        PDUTYPE2_DRAWNINEGRID_ERROR_PDU = 48,

        /// <summary>
        ///  GDI+ Error PDU (see [MS-RDPEGDI] section ).
        /// </summary>
        PDUTYPE2_DRAWGDIPLUS_ERROR_PDU = 49,

        /// <summary>
        ///  Auto-Reconnect Status PDU
        /// </summary>
        PDUTYPE2_ARC_STATUS_PDU = 50,

        /// <summary>
        ///  Status Info PDU
        /// </summary>
        PDUTYPE2_STATUS_INFO_PDU = 54,

        /// <summary>
        ///  Monitor Layout PDU0
        /// </summary>
        PDUTYPE2_MONITOR_LAYOUT_PDU = 55,

        /// <summary>
        /// Frame Acknowledge PDU
        /// </summary>
        PDUTYPE2_FRAME_ACKNOWLEDGE = 0x38
    }

    /// <summary>
    /// The type of compressedType.
    /// </summary>
    public enum compressedType_Values : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates the package which was used for compression.
        ///  See the table which follows for a list of compression
        ///  packages.
        /// </summary>
        CompressionTypeMask = 0x0F,

        /// <summary>
        ///  The payload data is compressed. This value corresponds
        ///  to MPPC bit C (see  [RFC2118] section 3.1).
        /// </summary>
        PACKET_COMPRESSED = 0x20,

        /// <summary>
        ///  The decompressed packet MUST be placed at the beginning
        ///  of the history buffer. This value corresponds to MPPC
        ///  bit B (see [RFC2118] section 3.1).
        /// </summary>
        PACKET_AT_FRONT = 0x40,

        /// <summary>
        ///  The history buffer MUST be reinitialized. This value
        ///  corresponds to MPPC bit A (see [RFC2118] section 3.1).
        /// </summary>
        PACKET_FLUSHED = 0x80,
    }

    /// <summary>
    ///  The TS_SECURITY_HEADER structure is attached to
    ///  server-to-client traffic when the Encryption Level
    ///  selected by the server (see sections  and ) is ENCRYPTION_LEVEL_LOW
    ///  (1). 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_1_2_1.xml
    //  </remarks>
    public partial class TS_SECURITY_HEADER
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The information flags describing
        ///  properties of the attached data.
        /// </summary>
        public TS_SECURITY_HEADER_flags_Values flags;

        /// <summary>
        ///  A 16-bit unsigned integer. This field is reserved for
        ///  future RDP needs. It is currently unused and all values
        ///  are ignored. This field will contain valid data only
        ///  if the SEC_FLAGSHI_VALID bit (0x8000) is set in the
        ///  flags field. If this bit is not set, the flagsHi field
        ///  is uninitialized and can contain any 16-bit unsigned
        ///  integer value.
        /// </summary>
        public ushort flagsHi;

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public virtual TS_SECURITY_HEADER Clone()
        {
            TS_SECURITY_HEADER cloneHeader = new TS_SECURITY_HEADER();
            cloneHeader.flags = flags;
            cloneHeader.flagsHi = flagsHi;

            return cloneHeader;
        }
    }

    /// <summary>
    /// The type od flags.
    /// </summary>
    public enum TS_SECURITY_HEADER_flags_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates that the packet is a Security Exchange PDU.
        ///  This packet type is sent from client to server only.
        ///  The client only sends this packet if it will be encrypting
        ///  further communication and standard RDP security methods
        ///  are in effect.
        /// </summary>
        SEC_EXCHANGE_PKT = 0x0001,

        /// <summary>
        /// Indicates that the packet is an Initiate Multitransport Request PDU (section 2.2.15.1).
        /// </summary>
        SEC_TRANSPORT_REQ = 0x0002,

        /// <summary>
        /// Indicates that the packet is an Initiate Multitransport Error PDU (section 2.2.15.2).
        /// </summary>
        SEC_TRANSPORT_RSP = 0x0004,
        /// <summary>
        ///  Indicates that encryption is being used for the packet.
        /// </summary>
        SEC_ENCRYPT = 0x0008,

        /// <summary>
        ///  This flag is set for legacy reasons when the packet
        ///  is a Confirm Active PDU. Otherwise this flag is never
        ///  used.
        /// </summary>
        SEC_RESET_SEQNO = 0x0010,

        /// <summary>
        ///  This flag is set for legacy reasons when the packet
        ///  is a Confirm Active PDU or a Client Synchronize PDU.
        ///  Otherwise this flag is never used.
        /// </summary>
        SEC_IGNORE_SEQNO = 0x0020,

        /// <summary>
        ///  Indicates that the packet is a Client Info PDU. This
        ///  packet type is sent from client to server only. If
        ///  standard RDP security methods and encryption are in
        ///  effect, then this packet MUST also be encrypted.
        /// </summary>
        SEC_INFO_PKT = 0x0040,

        /// <summary>
        ///  Indicates that the packet is a Licensing PDU.
        /// </summary>
        SEC_LICENSE_PKT = 0x0080,

        /// <summary>
        ///  Indicates to the client that the server is capable of
        ///  processing encrypted licensing packets. It is sent
        ///  by the server together with any licensing PDUs it may
        ///  send (see section ).
        /// </summary>
        SEC_LICENSE_ENCRYPT_CS = 0x0200,

        /// <summary>
        ///  Indicates to the server that the client is capable of
        ///  processing encrypted licensing packets. It is sent
        ///  by the client together with the SEC_EXCHANGE_PKT flag
        ///  when sending a Security Exchange PDU.
        /// </summary>
        SEC_LICENSE_ENCRYPT_SC = 0x0200,

        /// <summary>
        ///  Indicates that the packet is a Standard Security Server
        ///  Redirection PDU (see [MS-RDPEGDI] section 2.2.3.2.1).
        ///  The presence of this flag implies that the PDU is encrypted,
        ///  that is, the SEC_ENCRYPT (0x0008) flag MUST be considered
        ///  to be set.
        /// </summary>
        SEC_REDIRECTION_PKT = 0x0400,

        /// <summary>
        ///  Indicates that the message authentication code (MAC)
        ///  for the PDU was generated using the "salted MAC generation"
        ///  technique (see section ). If this flag is not present,
        ///  then the standard technique was used (see Non-FIPS
        ///  and FIPS).
        /// </summary>
        SEC_SECURE_CHECKSUM = 0x0800,

        /// <summary>
        /// Indicates that the packet is an Auto-Detect Request PDU (section 2.2.14.3) or that the  autoDetectReqData field is present. 
        /// </summary>
        SEC_AUTODETECT_REQ = 0x1000,

        /// <summary>
        /// Indicates that the packet is an Auto-Detect Response PDU (section 2.2.14.4).
        /// </summary>
        SEC_AUTODETECT_RSP = 0x2000,

        /// <summary>
        /// Indicates that the packet is a Heartbeat PDU (section 2.2.16.1). 
        /// </summary>
        SEC_HEARTBEAT = 0x4000,

        /// <summary>
        ///  Indicates that the flagsHi field contains valid data.
        ///  If this flag is not set, then the contents of the flagsHi
        ///  field should be ignored.
        /// </summary>
        SEC_FLAGSHI_VALID = 0x8000,
    }

    /// <summary>
    ///  The TS_SECURITY_HEADER1 structure is attached to
    ///  all client-to-server traffic when the Encryption Level
    ///  selected by the server (see sections  and ) is ENCRYPTION_LEVEL_LOW
    ///  (1), ENCRYPTION_LEVEL_CLIENT_COMPATIBLE (2) or ENCRYPTION_LEVEL_HIGH
    ///  (3). It is attached to all server-to-client traffic
    ///  when the Encryption Level is ENCRYPTION_LEVEL_CLIENT_COMPATIBLE
    ///  (2) or ENCRYPTION_LEVEL_HIGH (3). 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_1_2_2.xml
    //  </remarks>
    public partial class TS_SECURITY_HEADER1 : TS_SECURITY_HEADER
    {
        /// <summary>
        ///  The message authentication code (MAC) generated over
        ///  the packet, using one of the techniques described in
        ///  Non-FIPS.
        /// </summary>
        public byte[] dataSignature;

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override TS_SECURITY_HEADER Clone()
        {
            TS_SECURITY_HEADER1 cloneHeader = new TS_SECURITY_HEADER1();
            cloneHeader.flags = flags;
            cloneHeader.flagsHi = flagsHi;
            cloneHeader.dataSignature = RdpbcgrUtility.CloneByteArray(dataSignature);

            return cloneHeader;
        }
    }

    /// <summary>
    ///  The TS_SECURITY_HEADER2 structure is attached to
    ///  all traffic when the Encryption Level selected by the
    ///  server (see sections  and ) is ENCRYPTION_LEVEL_FIPS
    ///  (4). 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_1_2_3.xml
    //  </remarks>
    public partial class TS_SECURITY_HEADER2 : TS_SECURITY_HEADER
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The length of the FIPS security
        ///  header. This field MUST be set to 0x0010 (16 bytes)
        ///  for legacy reasons.
        /// </summary>
        public TS_SECURITY_HEADER2_length_Values length;

        /// <summary>
        ///  An 8-bit unsigned integer. The version of the FIPS header.
        ///  This field SHOULD be set to TSFIPS_VERSION1 (0x01).
        /// </summary>
        public byte version;

        /// <summary>
        ///  An 8-bit unsigned integer. The number of padding bytes
        ///  of padding appended to the end of the packet prior
        ///  to encryption to make sure that the data to be encrypted
        ///  is a multiple of the 3DES block size (that is, a multiple
        ///  of 8 as the block size is 64 bits).
        /// </summary>
        public byte padlen;

        /// <summary>
        ///  The message authentication code (MAC) generated over
        ///  the packet, using the techniques specified in section
        ///  .
        /// </summary>
        public byte[] dataSignature;

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override TS_SECURITY_HEADER Clone()
        {
            TS_SECURITY_HEADER2 cloneHeader = new TS_SECURITY_HEADER2();
            cloneHeader.flags = flags;
            cloneHeader.flagsHi = flagsHi;
            cloneHeader.length = length;
            cloneHeader.version = version;
            cloneHeader.padlen = padlen;
            cloneHeader.dataSignature = RdpbcgrUtility.CloneByteArray(dataSignature);

            return cloneHeader;
        }
    }

    /// <summary>
    /// The type of length.
    /// </summary>
    public enum TS_SECURITY_HEADER2_length_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0010,
    }

    /// <summary>
    ///  The slow-path Input Event PDU is used to transmit
    ///  input events from client to server.      
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_1_3.xml
    //  </remarks>
    public partial class TS_INPUT_PDU : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share
        ///  Control Header MUST be set to PDUTYPE_DATAPDU (7).
        ///  The pduType2 field of the              Share Data Header
        ///  MUST be set to PDUTYPE2_INPUT (28).           
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  A 16-bit unsigned integer. The number of slow-path input
        ///  events packed together in the slowPathInputEvents field.
        /// </summary>
        public ushort numberEvents;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2Octets;

        /// <summary>
        ///  A collection of slow-path input events to be processed
        ///  by the server. The number of events present in this
        ///  array is given by the numberEvents field.
        /// </summary>
        public Collection<TS_INPUT_EVENT> slowPathInputEvents;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public TS_INPUT_PDU(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public TS_INPUT_PDU()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            byte[] inputPduData = EncodeInputPduData();

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, inputPduData, context);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            TS_INPUT_PDU cloneInputPdu = new TS_INPUT_PDU(context);
            cloneInputPdu.commonHeader = commonHeader.Clone();
            cloneInputPdu.numberEvents = numberEvents;
            cloneInputPdu.pad2Octets = pad2Octets;
            cloneInputPdu.shareDataHeader = shareDataHeader;
            if (slowPathInputEvents != null)
            {
                cloneInputPdu.slowPathInputEvents = new Collection<TS_INPUT_EVENT>();
                for (int count = 0; count < this.slowPathInputEvents.Count; count++)
                {
                    cloneInputPdu.slowPathInputEvents.Add(slowPathInputEvents[count]);
                }
            }
            return cloneInputPdu;
        }

        /// <summary>
        /// Encode InputPduData 
        /// </summary>
        /// <returns>The encoded data.</returns>
        private byte[] EncodeInputPduData()
        {
            List<byte> inputBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(inputBuffer, shareDataHeader);
            RdpbcgrEncoder.EncodeStructure(inputBuffer, numberEvents);
            RdpbcgrEncoder.EncodeStructure(inputBuffer, pad2Octets);
            if (slowPathInputEvents != null)
            {
                foreach (TS_INPUT_EVENT inputEvent in slowPathInputEvents)
                {
                    RdpbcgrEncoder.EncodeStructure(inputBuffer, inputEvent.eventTime);
                    RdpbcgrEncoder.EncodeStructure(inputBuffer, (ushort)inputEvent.messageType);
                    RdpbcgrEncoder.EncodeStructure(inputBuffer, inputEvent.slowPathInputData);
                }
            }

            return inputBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_INPUT_EVENT structure is used to wrap event-specific
    ///  information for all slow-path input events. 				  
    ///    
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_1_3_1.xml
    //  </remarks>
    public partial struct TS_INPUT_EVENT
    {
        /// <summary>
        ///  A 32-bit unsigned integer.  The 32-bit timestamp for
        ///  the input event. This value is ignored by the server.
        /// </summary>
        public uint eventTime;

        /// <summary>
        ///  A 16-bit unsigned integer. The input event type.
        /// </summary>
        public TS_INPUT_EVENT_messageType_Values messageType;

        /// <summary>
        ///  TS_KEYBOARD_EVENT, TS_UNICODE_KEYBOARD_EVENT, TS_POINTER_EVENT,
        ///   TS_POINTERX_EVENT or TS_SYNC_EVENT. The actual contents
        ///  of the slow-path input event (see sections  through
        ///  ).
        /// </summary>
        public object slowPathInputData;
    }

    /// <summary>
    /// The type of messageType.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum TS_INPUT_EVENT_messageType_Values : ushort
    {

        /// <summary>
        ///  Indicates a Synchronize Event.
        /// </summary>
        INPUT_EVENT_SYNC = 0x0000,

        /// <summary>
        /// Indicates an Unused Event (section 2.2.8.1.1.3.1.1.6).
        /// </summary>
        INPUT_EVENT_UNUSED = 0x0002,

        /// <summary>
        ///  Indicates a Keyboard Event.
        /// </summary>
        INPUT_EVENT_SCANCODE = 0x0004,

        /// <summary>
        ///  Indicates a Unicode Keyboard Event.
        /// </summary>
        INPUT_EVENT_UNICODE = 0x0005,

        /// <summary>
        ///  Indicates a Mouse Event.
        /// </summary>
        INPUT_EVENT_MOUSE = 0x8001,

        /// <summary>
        ///  Indicates an Extended Mouse Event.
        /// </summary>
        INPUT_EVENT_MOUSEX = 0x8002,
    }

    /// <summary>
    ///  The TS_KEYBOARD_EVENT structure is a standard T.128
    ///  Keyboard Event (see  [T128] section 8.18.2). RDP keyboard
    ///  input is restricted to keyboard scancodes, unlike the
    ///  code-point or virtual codes supported in T.128 (a scancode
    ///  is an eight-bit value specifying a key location on
    ///  the keyboard). The server accepts a scancode value
    ///  and translates it into the correct character depending
    ///  on the language locale and keyboard layout used in
    ///  the session.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_1_3_1_1.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 6)]
    public partial struct TS_KEYBOARD_EVENT
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The flags describing the
        ///  keyboard event.
        /// </summary>
        [FieldOffset(0)]
        public keyboardFlags_Values keyboardFlags;

        /// <summary>
        ///  A 16-bit unsigned integer. The scancode of the key which
        ///  triggered the event.
        /// </summary>
        [FieldOffset(2)]
        public ushort keyCode;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        [FieldOffset(4)]
        public ushort pad2Octets;
    }

    /// <summary>
    /// The type of keyboardFlags.
    /// </summary>
    public enum keyboardFlags_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  The keystroke message contains an extended scancode.
        ///  For enhanced 101 and 102-key keyboards, extended keys
        ///  include the right ALT and right CTRL keys on the main
        ///  section of the keyboard; the INS, DEL, HOME, END, PAGE
        ///  UP, PAGE DOWN and ARROW keys in the clusters to the
        ///  left of the numeric keypad; and the Divide ("/") and
        ///  ENTER keys in the numeric keypad.
        /// </summary>
        KBDFLAGS_EXTENDED = 0x0100,

        /// <summary>
        /// Used to send keyboard events triggered by the PAUSE key.
        /// A PAUSE key press and release MUST be sent as the following sequence of keyboard events:
        /// CTRL (0x1D) DOWN
        /// NUMLOCK (0x45) DOWN
        /// CTRL (0x1D) UP
        /// NUMLOCK (0x45) UP
        /// The CTRL DOWN and CTRL UP events MUST both include the KBDFLAGS_EXTENDED1 flag.
        /// </summary>
        KBDFLAGS_EXTENDED1 = 0x0200,

        /// <summary>
        ///  Indicates that the key was down prior to this event.
        /// </summary>
        KBDFLAGS_DOWN = 0x4000,

        /// <summary>
        ///  The absence of this flag indicates a key-down event,
        ///  while its presence indicates a key-release event.
        /// </summary>
        KBDFLAGS_RELEASE = 0x8000,
    }

    /// <summary>
    /// The TS_UNUSED_EVENT structure is sent by RDP 4.0, 5.0, 5.1, 5.2, 6.0, 6.1, and 7.0 clients 
    /// if the server erroneously did not indicate support for scancodes in the Input Capability Set 
    /// (TS_INPUT_CAPABILITYSET) (section 2.2.7.1.6).				     
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 6)]
    public partial struct TS_UNUSED_EVENT
    {

        /// <summary>
        ///  A 32-bit, unsigned integer. This field is padding, and the values in this field MUST be ignored.
        /// </summary>
        [FieldOffset(0)]
        public uint pad4Octets;

        /// <summary>
        ///  A 16-bit, unsigned integer. This field is padding, and the values in this field MUST be ignored.
        /// </summary>
        [FieldOffset(4)]
        public ushort pad2Octets;
    }

    /// <summary>
    ///  The TS_UNICODE_KEYBOARD_EVENT structure is used
    ///  to transmit a Unicode input code, as opposed to a keyboard
    ///  scancode. Support for the Unicode Keyboard Event is
    ///  advertised in the Input Capability Set. 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_1_3_1_2.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 6)]
    public partial struct TS_UNICODE_KEYBOARD_EVENT
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The flags describing the Unicode keyboard event.
        /// </summary>
        [FieldOffset(0)]
        public keyboardFlags_Values keyboardFlags;

        /// <summary>
        ///  A 16-bit unsigned integer. The Unicode character input
        ///  code.
        /// </summary>
        [FieldOffset(2)]
        public ushort unicodeCode;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        [FieldOffset(4)]
        public ushort pad2Octets;
    }

    /// <summary>
    ///  The TS_POINTER_EVENT structure is a standard T.128
    ///  Keyboard Event (see [T128] section 8.18.1). RDP adds
    ///  flags to deal with wheel mice and extended mouse buttons.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_1_3_1_3.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 6)]
    public partial struct TS_POINTER_EVENT
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The flags describing the
        ///  pointer event.Mouse wheel event:
        /// </summary>
        [FieldOffset(0)]
        public pointerFlags_Values pointerFlags;

        /// <summary>
        ///  A 16-bit unsigned integer. The x-coordinate of the pointer
        ///  relative to the top-left corner of the server's virtual
        ///  desktop.
        /// </summary>
        [FieldOffset(2)]
        public ushort xPos;

        /// <summary>
        ///  A 16-bit unsigned integer. The y-coordinate of the pointer
        ///  relative to the top-left corner of the server's virtual
        ///  desktop.
        /// </summary>
        [FieldOffset(4)]
        public ushort yPos;
    }

    /// <summary>
    /// The type of pointerFlags.
    /// </summary>
    public enum pointerFlags_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// The event is a horizontal mouse wheel rotation
        /// </summary>
        PTRFLAGS_HWHEEL = 0x0400,

        /// <summary>
        ///  The event is a mouse wheel rotation. The only valid
        ///  flags in a wheel rotation event are PTRFLAGS_WHEEL_NEGATIVE
        ///  and the WheelRotationMask; all other pointer flags
        ///  are ignored.
        /// </summary>
        PTRFLAGS_WHEEL = 0x0200,

        /// <summary>
        ///  The PTRFLAGS_ROTATION_MASK value is negative and must
        ///  be sign-extended before injection at the server.
        /// </summary>
        PTRFLAGS_WHEEL_NEGATIVE = 0x0100,

        /// <summary>
        ///  The bit field describing the number of rotation units
        ///  the mouse wheel was rotated. The value is negative
        ///  if the PTRFLAGS_WHEEL_NEGATIVE flag is set.
        /// </summary>
        WheelRotationMask = 0x01FF,

        /// <summary>
        ///  Indicates that the mouse position should be updated
        ///  to the location specified by the xPos and yPos fields.
        /// </summary>
        PTRFLAGS_MOVE = 0x0800,

        /// <summary>
        ///  Indicates that a click event has occurred at the position
        ///  specified by the xPos and yPos fields. The button flags
        ///  indicate which button has been clicked and at least
        ///  one of these flags MUST be set.
        /// </summary>
        PTRFLAGS_DOWN = 0x8000,

        /// <summary>
        ///  Mouse button 1 (left button) was clicked or released.
        ///  If the PTRFLAGS_DOWN flag is set, then the button was
        ///  clicked, otherwise it was released.
        /// </summary>
        PTRFLAGS_BUTTON1 = 0x1000,

        /// <summary>
        ///  Mouse button 2 (right button) was clicked or released.
        ///  If the PTRFLAGS_DOWN flag is set, then the button was
        ///  clicked, otherwise it was released.
        /// </summary>
        PTRFLAGS_BUTTON2 = 0x2000,

        /// <summary>
        ///  Mouse button 3 (middle button or wheel) was clicked
        ///  or released. If the PTRFLAGS_DOWN flag is set, then
        ///  the button was clicked, otherwise it was released.
        /// </summary>
        PTRFLAGS_BUTTON3 = 0x4000,
    }

    /// <summary>
    ///  The TS_POINTERX_EVENT structure has the same format
    ///  as the TS_POINTER_EVENT. The fields and possible field
    ///  values are all the same, except for the pointerFlags
    ///  field. Support for the Extended Mouse Event is advertised
    ///  in the Input Capability Set. 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_1_3_1_4.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 6)]
    public partial struct TS_POINTERX_EVENT
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The flags describing the
        ///  extended mouse event.
        /// </summary>
        [FieldOffset(0)]
        public TS_POINTERX_EVENT_pointerFlags_Values pointerFlags;

        /// <summary>
        ///  A 16-bit unsigned integer. X-coordinate of the pointer.
        /// </summary>
        [FieldOffset(2)]
        public ushort xPos;

        /// <summary>
        ///  A 16-bit unsigned integer. Y-coordinate of the pointer.
        /// </summary>
        [FieldOffset(4)]
        public ushort yPos;
    }

    /// <summary>
    /// The type of pointerFlags.
    /// </summary>
    public enum TS_POINTERX_EVENT_pointerFlags_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates that a click event has occurred at the position
        ///  specified by the xPos and yPos fields. The button flags
        ///  indicate which button has been clicked and at least
        ///  one of these flags MUST be set.
        /// </summary>
        PTRXFLAGS_DOWN = 0x8000,

        /// <summary>
        ///  Extended mouse button 1 was clicked or released. If
        ///  the PTRXFLAGS_DOWN flag is set, then the button was
        ///  clicked, otherwise it was released.
        /// </summary>
        PTRXFLAGS_BUTTON1 = 0x0001,

        /// <summary>
        ///  Extended mouse button 2 was clicked or released. If
        ///  the PTRXFLAGS_DOWN flag is set, then the button was
        ///  clicked, otherwise it was released.
        /// </summary>
        PTRXFLAGS_BUTTON2 = 0x0002,
    }

    /// <summary>
    ///  The TS_SYNC_EVENT structure is a standard T.128
    ///  Input Synchronize Event (see [T128] section 8.18.6).
    ///  In RDP this event is used to synchronize the values
    ///  of the toggle keys (that is, Caps Lock) and to reset
    ///  the server key state to all keys up. This event is
    ///  typically sent when the client needs to update the
    ///  server with new settings. In current Microsoft RDP
    ///  clients this is done whenever the client window loses
    ///  focus in the client operating system, and then gets
    ///  focus back with possibly new toggle and shift key values.
    ///  The sync is then followed immediately with key-down
    ///  events for whatever keyboard and mouse keys may be
    ///  down.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_1_3_1_5.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 6)]
    public partial struct TS_SYNC_EVENT
    {
        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        [FieldOffset(0)]
        public ushort pad2Octets;

        /// <summary>
        ///  A 32-bit unsigned integer. Flags indicating the "on"
        ///  status of the keyboard toggle keys.
        /// </summary>
        [FieldOffset(2)]
        public toggleFlags_Values toggleFlags;
    }

    /// <summary>
    /// The type of toggleFlags.
    /// </summary>
    [Flags()]
    public enum toggleFlags_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates that the Scroll Lock indicator light SHOULD
        ///  be on.
        /// </summary>
        TS_SYNC_SCROLL_LOCK = 0x00000001,

        /// <summary>
        ///  Indicates that the Num Lock indicator light SHOULD be
        ///  on.
        /// </summary>
        TS_SYNC_NUM_LOCK = 0x00000002,

        /// <summary>
        ///  Indicates that the Caps Lock indicator light SHOULD
        ///  be on.
        /// </summary>
        TS_SYNC_CAPS_LOCK = 0x00000004,

        /// <summary>
        ///  Indicates that the Kana Lock indicator light SHOULD
        ///  be on.
        /// </summary>
        TS_SYNC_KANA_LOCK = 0x00000008,
    }

    /// <summary>
    ///  Fast-path revises client input packets from the
    ///  first byte with the goal of improving bandwidth. The
    ///  TPKT (see [T123]), X.224 (see [X224]) and MCS SDrq
    ///  (see [T125]) headers are replaced, the Security Header
    ///  is collapsed into the fast-path input header, and the
    ///  Share Data Header is replaced by a new fast-path format.
    ///  The contents of the input notification events (see
    ///  section ) are also changed to reduce their size, particularly
    ///  by removing or reducing headers. Support for fast-path
    ///  input is advertised in the Input Capability Set. 				
    ///      
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_2.xml
    //  </remarks>
    public partial class TS_FP_INPUT_PDU : RdpbcgrClientPdu
    {
        /// <summary>
        ///  An 8-bit unsigned integer. One-byte bit-packed header.
        ///  This byte coincides with the first byte of the TPKT
        ///  Header (see [T123] section 8), which is always 0x03.
        ///  Three pieces of information are collapsed into this
        ///  byte:Encryption dataNumber of events in the fast-path
        ///  input PDUAction code The format of the fpInputHeader
        ///  byte is described by the following bitmask diagram:
        /// </summary>
        public nested_TS_FP_INPUT_PDU_fpInputHeader fpInputHeader;

        /// <summary>
        ///  An 8-bit unsigned integer. If the most significant bit
        ///  of the length1 field is not set, then the size of the
        ///  PDU is in the range 1 to 127 bytes and the length1
        ///  field contains the overall PDU length (the length2
        ///  field is not present in this case). However, if the
        ///  most significant bit of the length1 field is set, then
        ///  the overall PDU length is given by the low 7 bits of
        ///  the length1 field concatenated with the 8 bits of the
        ///  length2 field, in big-endian order (the length2 field
        ///  contains the low-order bits).
        /// </summary>
        public byte length1;

        /// <summary>
        ///  An 8-bit unsigned integer. If the most significant bit
        ///  of the length1 field is not set, then the length2 field
        ///  is not present. If the most significant bit of the
        ///  length1 field is set, then the overall PDU length is
        ///  given by the low 7 bits of the length1 field concatenated
        ///  with the 8 bits of the length2 field, in big-endian
        ///  order (the length2 field contains the low-order bits).
        /// </summary>
        public byte length2;

        /// <summary>
        ///  Optional FIPS header information, present when the Encryption
        ///  Level selected by the server (see sections  and ) is
        ///  ENCRYPTION_LEVEL_FIPS (4). The fast-path FIPS information
        ///  structure is specified in section .
        /// </summary>
        public TS_FP_FIPS_INFO fipsInformation;

        /// <summary>
        ///  The message authentication code (MAC) generated over
        ///  the packet using one of the techniques described in
        ///  Non-FIPS (the FASTPATH_INPUT_SECURE_CHECKSUM flag,
        ///  which is set in the fpInputHeader field, describes
        ///  the method used to generate the signature). This field
        ///  is present if the FASTPATH_INPUT_ENCRYPTED flag is
        ///  set in the fpInputHeader field.
        /// </summary>
        public byte[] dataSignature;

        /// <summary>
        ///  An 8-bit unsigned integer. The number of fast-path input
        ///  events packed together in the fpInputEvents field (up
        ///  to 255). This field is present if the numberEvents
        ///  bit field in the fast-path header byte is zero.
        /// </summary>
        public byte numberEvents;

        /// <summary>
        ///  A collection of Fast-Path Input Event structures to
        ///  be processed by the server. The number of events present
        ///  in this array is given by the numberEvents bit field
        ///  in the fast-path header byte, or by the numberEvents
        ///  field in the Fast-Path Input Event PDU (if it is present).
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public Collection<TS_FP_INPUT_EVENT> fpInputEvents;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public TS_FP_INPUT_PDU(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public TS_FP_INPUT_PDU()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();

            byte[] fpInputdata = EncodeFpInputData();
            byte[] encryptedData = null;
            byte[] signature = null;

            if (context.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS
                && fipsInformation.padlen == 0)
            {
                fipsInformation.padlen = (byte)(ConstValue.TRIPLE_DES_PAD
                                       - (fpInputdata.Length % ConstValue.TRIPLE_DES_PAD));
            }

            bool isSalted = fpInputHeader.flags.HasFlag(encryptionFlags_Values.FASTPATH_INPUT_SECURE_CHECKSUM);

            context.Encrypt(fpInputdata, isSalted, out encryptedData, out signature);
            if (dataSignature == null)
            {
                dataSignature = signature;
            }

            byte[] fpHeaderdate = EncodeFpHeaderData(encryptedData.Length);

            RdpbcgrEncoder.EncodeBytes(totalBuffer, fpHeaderdate);
            RdpbcgrEncoder.EncodeBytes(totalBuffer, encryptedData);

            return totalBuffer.ToArray();
        }

        /// <summary>
        /// Encode the fast path header data
        /// </summary>
        /// <param name="dataLength">The length after data signature part.</param>
        /// <returns>the encoded fastPath headerData array</returns>
        private byte[] EncodeFpHeaderData(int dataLength)
        {
            List<byte> fpHeaderData = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(fpHeaderData, fpInputHeader);

            int totalLength = Marshal.SizeOf(fpInputHeader) + Marshal.SizeOf(length1) + dataLength;
            if (context.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS)
            {
                totalLength += Marshal.SizeOf(fipsInformation);
            }

            if (context.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE)
            {
                totalLength += dataSignature.Length;
            }

            // if the most significant bit of the length1 field is set
            if (totalLength > 0x7F)
            {
                ++totalLength;

                // The overall PDU length is given by the low 7 bits of the length1 field 
                // concatenated with the 8 bits of the length2 field, in big-endian order 
                // (the length2 field contains the low-order bits).
                length1 = (byte)((totalLength >> 8) | 0x80);
                length2 = (byte)(totalLength & 0xFF);
                RdpbcgrEncoder.EncodeStructure(fpHeaderData, length1);
                RdpbcgrEncoder.EncodeStructure(fpHeaderData, length2);
            }
            else
            {
                // If the most significant bit of the length1 field is not set, then the size of the PDU is 
                // in the range 1 to 127 bytes and the length1 field contains the overall PDU length 
                // (the length2 field is not present in this case). 
                length1 = (byte)(totalLength);
                length2 = 0;
                RdpbcgrEncoder.EncodeStructure(fpHeaderData, length1);
            }

            if (context.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS)
            {
                RdpbcgrEncoder.EncodeStructure(fpHeaderData, fipsInformation);
            }

            if (fpInputHeader.flags.HasFlag(encryptionFlags_Values.FASTPATH_INPUT_ENCRYPTED))
            {
                RdpbcgrEncoder.EncodeBytes(fpHeaderData, dataSignature);
            }

            if (fpInputHeader.numEvents == 0)
            {
                RdpbcgrEncoder.EncodeStructure(fpHeaderData, numberEvents);
            }
            return fpHeaderData.ToArray();
        }

        /// <summary>
        /// Encode fast path Input data.
        /// </summary>
        /// <returns>the InputData array</returns>
        private byte[] EncodeFpInputData()
        {
            List<byte> fastpathInputData = new List<byte>();

            // If the number of input events is greater than 15, 
            // then the numberEvents bit field in the fast-path header byte 
            // MUST be set to zero, and the numberEvents optional field inserted 
            // after the dataSignature field. This allows up to 255 input events in one PDU.
            if (fpInputEvents.Count > 0x0F)
            {
                RdpbcgrEncoder.EncodeStructure(fastpathInputData, fpInputEvents.Count);
            }

            if (fpInputEvents != null)
            {
                foreach (TS_FP_INPUT_EVENT fpEvent in fpInputEvents)
                {
                    RdpbcgrEncoder.EncodeStructure(fastpathInputData, fpEvent.eventHeader);
                    if (fpEvent.eventData.GetType() != typeof(TS_FP_SYNC_EVENT))
                    {
                        RdpbcgrEncoder.EncodeStructure(fastpathInputData, fpEvent.eventData);
                    }
                    // else type TS_FP_SYNC_EVENT has no following data 
                }
            }

            return fastpathInputData.ToArray();
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            TS_FP_INPUT_PDU cloneFpInputPdu = new TS_FP_INPUT_PDU(context);

            cloneFpInputPdu.dataSignature = RdpbcgrUtility.CloneByteArray(dataSignature);
            cloneFpInputPdu.fpInputHeader = fpInputHeader;
            cloneFpInputPdu.length1 = length1;
            cloneFpInputPdu.length2 = length2;
            cloneFpInputPdu.numberEvents = numberEvents;
            cloneFpInputPdu.fipsInformation = fipsInformation;

            if (fpInputEvents != null)
            {
                cloneFpInputPdu.fpInputEvents = new Collection<TS_FP_INPUT_EVENT>();
                for (int count = 0; count < fpInputEvents.Count; count++)
                {
                    cloneFpInputPdu.fpInputEvents.Add(fpInputEvents[count]);
                }
            }

            return cloneFpInputPdu;
        }
    }

    /// <summary>
    ///  nested_TS_FP_INPUT_PDU_fpInputHeader nested field.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_2.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    public partial struct nested_TS_FP_INPUT_PDU_fpInputHeader
    {
        /// <summary>
        /// Construct TS_FP_INPUT_PDU from a packed byte.
        /// </summary>
        /// <param name="data">The packed byte.</param>
        public nested_TS_FP_INPUT_PDU_fpInputHeader(byte data)
        {
            actionCode = data;
        }

        /// <summary>
        /// Construct TS_FP_INPUT_PDU by each field.
        /// </summary>
        /// <param name="action">The action field.</param>
        /// <param name="numEvents">The numEvents field.</param>
        /// <param name="flags">The flags field.</param>
        public nested_TS_FP_INPUT_PDU_fpInputHeader(actionCode_Values action, int numEvents, encryptionFlags_Values flags)
        {
            var vector = new BitVector32();

            vector[actionField] = (int)action;

            vector[numEventsField] = numEvents;

            vector[flagsField] = (int)flags;

            actionCode = (byte)vector.Data;
        }

        /// <summary>
        ///  Includes actionCode, numberEvents and encryptionFlags.
        /// </summary>
        [FieldOffset(0)]
        private byte actionCode;

        private static BitVector32.Section actionField = BitVector32.CreateSection(0x3);

        private static BitVector32.Section numEventsField = BitVector32.CreateSection(0xF, actionField);

        private static BitVector32.Section flagsField = BitVector32.CreateSection(0x3, numEventsField);

        /// <summary>
        /// A 2-bit, unsigned integer that indicates whether the PDU is in fast-path or slow-path format.
        /// </summary>
        public actionCode_Values action
        {
            get
            {
                var vector = new BitVector32(actionCode);

                return (actionCode_Values)vector[actionField];
            }

            set
            {
                var vector = new BitVector32(actionCode);

                vector[actionField] = (int)value;

                actionCode = (byte)vector.Data;
            }
        }

        /// <summary>
        /// A 4-bit, unsigned integer that collapses the number of fast-path input events packed together in the fpInputEvents field into 4 bits if the number of events is in the range 1 to 15.
        /// If the number of input events is greater than 15, then the numEvents bit field in the fast-path header byte MUST be set to zero, and the numEvents optional field inserted after the dataSignature field. 
        /// This allows up to 255 input events in one PDU.
        /// </summary>
        public int numEvents
        {
            get
            {
                var vector = new BitVector32(actionCode);

                return vector[numEventsField];
            }

            set
            {
                var vector = new BitVector32(actionCode);

                vector[numEventsField] = value;

                actionCode = (byte)vector.Data;
            }
        }

        /// <summary>
        /// A 2-bit, unsigned integer that contains the flags describing the cryptographic parameters of the PDU.
        /// </summary>
        public encryptionFlags_Values flags
        {
            get
            {
                var vector = new BitVector32(actionCode);

                return (encryptionFlags_Values)vector[flagsField];
            }

            set
            {
                var vector = new BitVector32(actionCode);

                vector[flagsField] = (int)value;

                actionCode = (byte)vector.Data;
            }
        }
    }

    /// <summary>
    /// The type of actionCode.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum actionCode_Values : uint
    {

        /// <summary>
        ///  Indicates the PDU is a fast-path input PDU.
        /// </summary>
        FASTPATH_INPUT_ACTION_FASTPATH = 0x0,

        /// <summary>
        ///  Indicates the presence of a TPKT Header initial version
        ///  byte, which implies that the PDU is a slow-path input
        ///  PDU (in this case the full value of the initial byte
        ///  MUST be 0x03).
        /// </summary>
        FASTPATH_INPUT_ACTION_X224 = 0x3,
    }

    /// <summary>
    /// The type of encryptionFlags.
    /// </summary>
    [Flags()]
    public enum encryptionFlags_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates that the MAC signature for the PDU was generated
        ///  using the "salted MAC generation" technique (see section
        ///  ). If this bit is not set, then the standard technique
        ///  was used (see sections  and ).
        /// </summary>
        FASTPATH_INPUT_SECURE_CHECKSUM = 0x1,

        /// <summary>
        ///  Indicates that the PDU contains an 8-byte message authentication
        ///  code (MAC) signature after the optional length2 field
        ///  (that is, the dataSignature field is present) and the
        ///  contents of the PDU are encrypted using the negotiated
        ///  encryption package (see sections  and ).
        /// </summary>
        FASTPATH_INPUT_ENCRYPTED = 0x2,
    }

    /// <summary>
    ///  The TS_FP_FIPS_INFO structure contains fast-path
    ///  information for inclusion in a fast-path header. 				
    ///      
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_2_1.xml
    //  </remarks>
    public partial struct TS_FP_FIPS_INFO
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The length of the FIPS Security
        ///  Header. This field MUST be set to 0x0010 (16 bytes).
        /// </summary>
        public TS_FP_FIPS_INFO_length_Values length;

        /// <summary>
        ///  An 8-bit unsigned integer. The version of the FIPS Header.
        ///  This field SHOULD be set to TSFIPS_VERSION1 (0x01).
        /// </summary>
        public byte version;

        /// <summary>
        ///  An 8-bit unsigned integer. The number of padding bytes
        ///  of padding appended to the end of the packet prior
        ///  to encryption to make sure that the data to be encrypted
        ///  is a multiple of the 3DES block size (that is, a multiple
        ///  of 8 as the block size is 64 bits).
        /// </summary>
        public byte padlen;
    }

    /// <summary>
    /// The type of length.
    /// </summary>
    public enum TS_FP_FIPS_INFO_length_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0010,
    }

    /// <summary>
    ///  The TS_FP_INPUT_EVENT structure is used to describe
    ///  the type and encapsulate the data for a fast-path input
    ///  event sent from client to server. All fast-path input
    ///  events conform to this basic structure (see sections
    ///   to ). 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_2_2.xml
    //  </remarks>
    public partial struct TS_FP_INPUT_EVENT
    {

        /// <summary>
        ///  An 8-bit unsigned integer. One byte bit-packed event
        ///  header. Two pieces of information are collapsed into
        ///  this byte:Fast-path input event typeFlags specific
        ///  to the input eventThe eventHeader field is structured
        ///  as follows:
        /// </summary>
        public nested_TS_FP_INPUT_EVENT_eventHeader eventHeader;

        /// <summary>
        ///  Optional and variable length data specific to the input
        ///  event.
        /// </summary>
        public object eventData;
    }

    /// <summary>
    ///  nested_TS_FP_INPUT_EVENT_eventHeader nested field.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_2_2.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    public partial struct nested_TS_FP_INPUT_EVENT_eventHeader
    {

        /// <summary>
        ///  Includes eventFlags and eventCode.
        /// </summary>
        //[Bit()]
        [FieldOffset(0)]
        public byte eventFlagsAndCode;
    }

    /// <summary>
    /// The type of eventCode.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum eventCode_Values : uint
    {

        /// <summary>
        ///  Indicates a Fast-Path Keyboard Event.
        /// </summary>
        FASTPATH_INPUT_EVENT_SCANCODE = 0x0,

        /// <summary>
        ///  Indicates a Fast-Path Mouse Event.
        /// </summary>
        FASTPATH_INPUT_EVENT_MOUSE = 0x1,

        /// <summary>
        ///  Indicates a Fast-Path Extended Mouse Event.
        /// </summary>
        FASTPATH_INPUT_EVENT_MOUSEX = 0x2,

        /// <summary>
        ///  Indicates a Fast-Path Synchronize Event.
        /// </summary>
        FASTPATH_INPUT_EVENT_SYNC = 0x3,

        /// <summary>
        ///  Indicates a Fast-Path Unicode Keyboard Event.
        /// </summary>
        FASTPATH_INPUT_EVENT_UNICODE = 0x4,

        /// <summary>
        /// Indicates a Fast-Path Quality of Experience (QoE) Timestamp Event.
        /// </summary>
        FASTPATH_INPUT_EVENT_QOE_TIMESTAMP = 0x6,
    }

    /// <summary>
    ///  The TS_FP_KEYBOARD_EVENT structure is the fast-path
    ///  variant of the TS_KEYBOARD_EVENT. 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_2_2_1.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 1)]
    public partial struct TS_FP_KEYBOARD_EVENT
    {
        /// <summary>
        ///  An 8-bit unsigned integer. The scancode of the key which
        ///  triggered the event.
        /// </summary>
        [FieldOffset(0)]
        public byte keyCode;
    }

    /// <summary>
    /// The type of eventFlags.
    /// </summary>
    [Flags()]
    public enum TS_FP_KEYBOARD_EVENT_Eventflags : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// The absence of this flag indicates a key-down event, 
        /// while its presence indicates a key-release event.
        /// </summary>
        FASTPATH_INPUT_KBDFLAGS_RELEASE = 0x01,

        /// <summary>
        /// The keystroke message contains an extended scancode.
        /// </summary>
        FASTPATH_INPUT_KBDFLAGS_EXTENDED = 0x02,

        /// <summary>
        /// Used to send keyboard events triggered by the PAUSE key.
        /// A PAUSE key press and release MUST be sent as the following sequence of keyboard events:
        /// CTRL (0x1D) DOWN
        /// NUMLOCK (0x45) DOWN
        /// CTRL (0x1D) UP
        /// NUMLOCK (0x45) UP
        /// The CTRL DOWN and CTRL UP events MUST both include the FASTPATH_INPUT_KBDFLAGS_EXTENDED1 flag.
        /// </summary>
        FASTPATH_INPUT_KBDFLAGS_EXTENDED1 = 0x04,
    }

    /// <summary>
    ///  The TS_FP_UNICODE_KEYBOARD_EVENT structure is the
    ///  fast-path variant of the TS_UNICODE_KEYBOARD_EVENT
    ///  structure. Support for the Unicode Keyboard Event is
    ///  advertised in the Input Capability Set. 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_2_2_2.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public partial struct TS_FP_UNICODE_KEYBOARD_EVENT
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The Unicode character input
        ///  code.
        /// </summary>
        [FieldOffset(0)]
        public ushort unicodeCode;
    }

    /// <summary>
    /// The TS_FP_QOETIMESTAMP_EVENT structure is used to enable the calculation of
    /// Quality of Experience (QoE) metrics. This event is sent solely for informational
    /// and debugging purposes and MUST NOT be transmitted to the server if the
    /// TS_INPUT_FLAG_QOE_TIMESTAMPS (0x0200) flag was not received in the
    /// Input Capability Set (section 2.2.7.1.6).
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public partial struct TS_FP_QOETIMESTAMP_EVENT
    {
        /// <summary>
        ///  A 32-bit, unsigned integer.
        ///  A client-generated timestamp, in milliseconds,
        ///  that indicates when the current input batch was encoded by the client.
        ///  The value of the first timestamp sent by the client implicitly defines
        ///  the origin for all subsequent timestamps.
        ///  The server is responsible for handling roll-over of the timestamp.
        /// </summary>
        [FieldOffset(0)]
        public uint timestamp;
    }

    /// <summary>
    ///  The TS_FP_POINTER_EVENT structure is the fast-path
    ///  variant of the TS_POINTER_EVENT structure. 				   
    ///   
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_2_2_3.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 6)]
    public partial struct TS_FP_POINTER_EVENT
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The flags describing the
        ///  pointer event. The possible flags are identical to
        ///  those found in the pointerFlags field of the TS_POINTER_EVENT
        ///  structure.
        /// </summary>
        [FieldOffset(0)]
        public ushort pointerFlags;

        /// <summary>
        ///  A 16-bit unsigned integer. The x-coordinate of the pointer.
        /// </summary>
        [FieldOffset(2)]
        public ushort xPos;

        /// <summary>
        ///  A 16-bit unsigned integer. The y-coordinate of the pointer.
        /// </summary>
        [FieldOffset(4)]
        public ushort yPos;
    }

    /// <summary>
    ///  The TS_FP_POINTERX_EVENT structure is the fast-path
    ///  variant of the TS_POINTERX_EVENT structure. Support
    ///  for the Extended Mouse Event is advertised in the Input
    ///  Capability Set. 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_2_2_4.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 6)]
    public partial struct TS_FP_POINTERX_EVENT
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The flags describing the
        ///  pointer event. The possible flags are identical to
        ///  those found in the pointerFlags field of the TS_POINTERX_EVENT
        ///  structure.
        /// </summary>
        [FieldOffset(0)]
        public ushort pointerFlags;

        /// <summary>
        ///  A 16-bit unsigned integer. The x-coordinate of the pointer.
        /// </summary>
        [FieldOffset(2)]
        public ushort xPos;

        /// <summary>
        ///  A 16-bit unsigned integer. The y-coordinate of the pointer.
        /// </summary>
        [FieldOffset(4)]
        public ushort yPos;
    }

    /// <summary>
    ///  The TS_FP_SYNC_EVENT structure is the fast-path
    ///  variant of the TS_SYNC_EVENT structure.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_1_2_2_5.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 0)]
    public partial struct TS_FP_SYNC_EVENT
    {
    }

    /// <summary>
    /// The type of eventFlags.
    /// </summary>
    [Flags()]
    public enum TS_FP_SYNC_EVENT_Eventflags : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that the Scroll Lock indicator light SHOULD be on.
        /// </summary>
        FASTPATH_INPUT_SYNC_SCROLL_LOCK = 0x01,

        /// <summary>
        /// Indicates that the Num Lock indicator light SHOULD be on.
        /// </summary>
        FASTPATH_INPUT_SYNC_NUM_LOCK = 0x02,

        /// <summary>
        /// Indicates that the Caps Lock indicator light SHOULD be on.
        /// </summary>
        FASTPATH_INPUT_SYNC_CAPS_LOCK = 0x04,

        /// <summary>
        /// Indicates that the Kana Lock indicator light SHOULD be on.
        /// </summary>
        FASTPATH_INPUT_SYNC_KANA_LOCK = 0x08,
    }

    /// <summary>
    ///  The Set Keyboard Indicators PDU is sent by the
    ///  server to synchronize the state of the keyboard toggle
    ///  keys (Scroll Lock, Num Lock, and so on). It is similar
    ///  in operation to the Client Synchronize Input Event
    ///  Notification (see sections  and ), but flows in the
    ///  opposite direction.      
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_2_1.xml
    //  </remarks>
    public partial class Server_Set_Keyboard_Indicators_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Set Keyboard Indicators PDU,
        ///  as specified in section .           
        /// </summary>
        public TS_SET_KEYBOARD_INDICATORS_PDU setKeyBdIndicatorsPduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public Server_Set_Keyboard_Indicators_Pdu(RdpbcgrServerSessionContext severSessionContext)
            : base(severSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_Set_Keyboard_Indicators_Pdu()
        {
        }

        public override StackPacket Clone()
        {
            Server_Set_Keyboard_Indicators_Pdu cloneKeyboardIndicatorsPdu = new Server_Set_Keyboard_Indicators_Pdu();
            cloneKeyboardIndicatorsPdu.commonHeader = commonHeader.Clone();
            cloneKeyboardIndicatorsPdu.setKeyBdIndicatorsPduData.shareDataHeader =
                setKeyBdIndicatorsPduData.shareDataHeader;
            cloneKeyboardIndicatorsPdu.setKeyBdIndicatorsPduData.UnitId =
                setKeyBdIndicatorsPduData.UnitId;
            cloneKeyboardIndicatorsPdu.setKeyBdIndicatorsPduData.LedFlags =
                setKeyBdIndicatorsPduData.LedFlags;

            return cloneKeyboardIndicatorsPdu;
        }

        public override byte[] ToBytes()
        {
            byte[] dataBuffer = EncodeKeyboardIndicatorsData(setKeyBdIndicatorsPduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, dataBuffer);

            return encodedBytes;
        }

        private byte[] EncodeKeyboardIndicatorsData(TS_SET_KEYBOARD_INDICATORS_PDU setKeyBdIndicatorsPduData)
        {
            List<byte> dataBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(dataBuffer, setKeyBdIndicatorsPduData.shareDataHeader);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, setKeyBdIndicatorsPduData.UnitId);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, (ushort)setKeyBdIndicatorsPduData.LedFlags);

            return dataBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_SET_KEYBOARD_INDICATORS_PDU structure contains
    ///  the actual contents of the Set Keyboard Indicators
    ///  PDU. The contents of the LedFlags field is identical
    ///  to the flags used in the Client Synchronize Input Event
    ///  Notification (see section ). 				 				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_2_1_1.xml
    //  </remarks>
    public partial struct TS_SET_KEYBOARD_INDICATORS_PDU
    {
        /// <summary>
        ///  Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share
        ///  Control Header MUST be set to PDUTYPE_DATAPDU (7).
        ///  The pduType2 field of the Share Data Header MUST be
        ///  set to PDUTYPE2_SET_KEYBOARD_INDICATORS (41).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  A 16-bit unsigned integer. Hardware related value. This
        ///  field SHOULD be ignored by the client and as a consequence
        ///  SHOULD be set to 0 by the server.
        /// </summary>
        public ushort UnitId;

        /// <summary>
        ///  A 16-bit unsigned integer. The flags indicating the
        ///  "on" status of the keyboard toggle keys.
        /// </summary>
        public LedFlags_Values LedFlags;
    }

    /// <summary>
    /// The type of LedFlags.
    /// </summary>
    [Flags()]
    public enum LedFlags_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates that the Scroll Lock indicator light SHOULD
        ///  be on.
        /// </summary>
        TS_SYNC_SCROLL_LOCK = 0x0001,

        /// <summary>
        ///  Indicates that the Num Lock indicator light SHOULD be
        ///  on.
        /// </summary>
        TS_SYNC_NUM_LOCK = 0x0002,

        /// <summary>
        ///  Indicates that the Caps Lock indicator light SHOULD
        ///  be on.
        /// </summary>
        TS_SYNC_CAPS_LOCK = 0x0004,

        /// <summary>
        ///  Indicates that the Kana Lock indicator light SHOULD
        ///  be on.
        /// </summary>
        TS_SYNC_KANA_LOCK = 0x0008,
    }

    /// <summary>
    ///  The Set Keyboard IME Status PDU  is sent by
    ///  the server when the user session employs input method
    ///  editors (IMEs) and is used to set the IME state. This
    ///  PDU is accepted and ignored by non-IME aware clients.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_2_2.xml
    //  </remarks>
    public partial class Server_Set_Keyboard_IME_Status_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Set Keyboard IME Status PDU,
        ///  as specified in section .           
        /// </summary>
        public TS_SET_KEYBOARD_IME_STATUS_PDU setKeyBdImeStatusPduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public Server_Set_Keyboard_IME_Status_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_Set_Keyboard_IME_Status_Pdu()
        {
        }

        public override StackPacket Clone()
        {
            Server_Set_Keyboard_IME_Status_Pdu cloneImePdu = new Server_Set_Keyboard_IME_Status_Pdu(serverSessionContext);
            cloneImePdu.commonHeader = commonHeader.Clone();
            cloneImePdu.setKeyBdImeStatusPduData.UnitId = setKeyBdImeStatusPduData.UnitId;
            cloneImePdu.setKeyBdImeStatusPduData.shareDataHeader = setKeyBdImeStatusPduData.shareDataHeader;
            cloneImePdu.setKeyBdImeStatusPduData.ImeState = setKeyBdImeStatusPduData.ImeState;
            cloneImePdu.setKeyBdImeStatusPduData.ImeConvMode = setKeyBdImeStatusPduData.ImeConvMode;

            return cloneImePdu;
        }

        public override byte[] ToBytes()
        {
            byte[] dataBuffer = EncodeKeyboardImeData(setKeyBdImeStatusPduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, dataBuffer);

            return encodedBytes;
        }

        private byte[] EncodeKeyboardImeData(TS_SET_KEYBOARD_IME_STATUS_PDU setKeyBdImeStatusPduData)
        {
            List<byte> dataBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(dataBuffer, setKeyBdImeStatusPduData.shareDataHeader);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, setKeyBdImeStatusPduData.UnitId);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, setKeyBdImeStatusPduData.ImeState);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, setKeyBdImeStatusPduData.ImeConvMode);

            return dataBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_SET_KEYBOARD_IME_STATUS_PDU structure contains
    ///  the actual contents of the Set Keyboard IME Status
    ///  PDU. On RDP 5.0 and later clients the latter two fields
    ///  are used as input parameters to a Fujitsu Oyayubi specific
    ///  IME control function of East Asia IME clients. 				
    ///      
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_7_2_2_1.xml
    //  </remarks>
    public partial struct TS_SET_KEYBOARD_IME_STATUS_PDU
    {
        /// <summary>
        ///  Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share
        ///  Control Header MUST be set to PDUTYPE_DATAPDU (7).
        ///  The pduType2 field of the Share Data Header MUST be
        ///  set to PDUTYPE2_SET_KEYBOARD_IME_STATUS (45).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  A 16-bit unsigned integer. The unit identifier for which
        ///  the IME message is intended. This field SHOULD be ignored
        ///  by the client and as a consequence SHOULD be set to
        ///  0 by the server.
        /// </summary>
        public ushort UnitId;

        /// <summary>
        ///  A 32-bit unsigned integer.  Indicates the open or close
        ///  state of the IME.
        /// </summary>
        public uint ImeState;

        /// <summary>
        ///  A 32-bit unsigned integer.  Indicates the IME conversion
        ///  status.
        /// </summary>
        public uint ImeConvMode;
    }

    /// <summary>
    /// A 32-bit, unsigned integer. Indicates the open or close state of the IME.
    /// </summary>
    public enum ImeState_Values : uint
    {
        /// <summary>
        /// Indicates that the IME state is closed.
        /// </summary>
        IME_STATE_CLOSED = 0x00000000,

        /// <summary>
        /// Indicates that the IME state is open.
        /// </summary>
        IME_STATE_OPEN = 0x00000001
    }

    /// <summary>
    /// A 32-bit, unsigned integer. Indicates the IME conversion mode.
    /// </summary>
    [Flags]
    public enum ImeConvMode_Values : uint
    {
        /// <summary>
        /// None of the flags is set.
        /// </summary>
        None,

        ///
        ///Indicates that the input mode is native. If not set, the input mode is alphanumeric.	
        ///
        IME_CMODE_NATIVE = 0x00000001,

        ///
        ///Indicates that the input mode is Katakana. If not set, the input mode is Hiragana.	
        ///
        IME_CMODE_KATAKANA = 0x00000002,

        ///
        ///Indicates that the input mode is full-width. If not set, the input mode is half-width.
        ///	
        IME_CMODE_FULLSHAPE = 0x00000008,

        ///
        ///Indicates that Hanja conversion mode is in effect.	
        ///
        IME_CMODE_HANJACONVERT = 0x00000040,

        ///
        ///Indicates that a soft (on-screen) keyboard is being used.	
        ///
        IME_CMODE_SOFTKBD = 0x00000080,

        ///
        ///Indicates that IME conversion is turned off (i.e. the IME is closed).
        ///	
        IME_CMODE_NOCONVERSION = 0x00000100,

        ///
        ///Indicates that End-User Defined Character (EUDC) conversion mode is in effect.
        ///	
        IME_CMODE_EUDC = 0x00000200,

        ///
        ///Indicates that symbol conversion mode is in effect.
        ///	
        IME_CMODE_SYMBOL = 0x00000400,

        ///
        ///Indicates that fixed conversion mode is in effect.
        ///	
        IME_CMODE_FIXED = 0x00000800
    }


    /// <summary>
    ///  Slow path output PDU, including Graphics PDU and Pointer PDU.
    /// </summary>
    public class SlowPathOutputPdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        /// The slow path updates.
        /// </summary>
        public RdpbcgrSlowPathUpdatePdu[] slowPathUpdates;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public SlowPathOutputPdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public SlowPathOutputPdu()
        {
        }

        public override StackPacket Clone()
        {
            SlowPathOutputPdu pduClone = new SlowPathOutputPdu();
            pduClone.commonHeader = commonHeader;

            if (slowPathUpdates != null)
            {
                Collection<RdpbcgrSlowPathUpdatePdu> updates = new Collection<RdpbcgrSlowPathUpdatePdu>();
                for (int i = 0; i < slowPathUpdates.Length; ++i)
                {
                    updates.Add(slowPathUpdates[i]);
                }

                pduClone.slowPathUpdates = new RdpbcgrSlowPathUpdatePdu[updates.Count];
                for (int i = 0; i < updates.Count; ++i)
                {
                    pduClone.slowPathUpdates[i] = updates[i];
                }
            }
            return pduClone;
        }

        public override byte[] ToBytes()
        {
            byte[] inputPduData = EncodeOutputPduData();

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, inputPduData, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, inputPduData);

            return encodedBytes;
        }

        private byte[] EncodeOutputPduData()
        {
            List<byte> outputBuffer = new List<byte>();
            byte[] data = new byte[1];

            for (int i = 0; i < slowPathUpdates.Length; ++i)
            {
                if (slowPathUpdates[i].GetType() == typeof(TS_UPDATE_PALETTE))
                {
                    RdpbcgrEncoder.EncodeBytes(outputBuffer, EncodePalette((TS_UPDATE_PALETTE)slowPathUpdates[i]));
                }
                else if (slowPathUpdates[i].GetType() == typeof(TS_UPDATE_BITMAP))
                {
                    RdpbcgrEncoder.EncodeBytes(outputBuffer, EncodeBitmap((TS_UPDATE_BITMAP)slowPathUpdates[i]));
                }
                else if (slowPathUpdates[i].GetType() == typeof(TS_UPDATE_SYNC))
                {
                    RdpbcgrEncoder.EncodeBytes(outputBuffer, EncodeSync((TS_UPDATE_SYNC)slowPathUpdates[i]));
                }
                else if (slowPathUpdates[i].GetType() == typeof(TS_POINTER_PDU))
                {
                    RdpbcgrEncoder.EncodeBytes(outputBuffer, EncodePtr((TS_POINTER_PDU)slowPathUpdates[i]));
                }
                else if (slowPathUpdates[i].GetType() == typeof(TS_UPDATE_ORDERS))
                {
                    RdpbcgrEncoder.EncodeBytes(outputBuffer, EncodeOrders((TS_UPDATE_ORDERS)slowPathUpdates[i]));
                }

                //outputBuffer.AddRange(data);
            }
            return outputBuffer.ToArray();
        }

        private byte[] EncodeOrders(TS_UPDATE_ORDERS ordersData)
        {
            List<byte> totalBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(totalBuffer, ordersData.shareDataHeader);

            List<byte> buffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(buffer, (ushort)ordersData.updateType);
            RdpbcgrEncoder.EncodeStructure(buffer, ordersData.pad2OctetA);
            RdpbcgrEncoder.EncodeStructure(buffer, ordersData.numberOrders);
            RdpbcgrEncoder.EncodeStructure(buffer, ordersData.pad2OctetsB);
            RdpbcgrEncoder.EncodeBytes(buffer, ordersData.orderData);

            if (ordersData.shareDataHeader.compressedType != compressedType_Values.None)
            {
                RdpbcgrEncoder.EncodeBytes(
                    totalBuffer, serverSessionContext.Compress(ordersData.shareDataHeader.compressedType, buffer.ToArray()));
            }
            else
            {
                RdpbcgrEncoder.EncodeBytes(totalBuffer, buffer.ToArray());
            }
            return totalBuffer.ToArray();
        }

        private byte[] EncodePalette(TS_UPDATE_PALETTE paletteData)
        {
            List<byte> totalBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(totalBuffer, paletteData.shareDataHeader);

            List<byte> buffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(buffer, (ushort)paletteData.paletteData.updateType);
            RdpbcgrEncoder.EncodeStructure(buffer, paletteData.paletteData.pad2Octets);
            RdpbcgrEncoder.EncodeStructure(buffer, paletteData.paletteData.numberColors);
            if (paletteData.paletteData.paletteEntries != null)
            {
                for (int i = 0; i < paletteData.paletteData.paletteEntries.Length; ++i)
                {
                    RdpbcgrEncoder.EncodeStructure(buffer, paletteData.paletteData.paletteEntries[i].red);
                    RdpbcgrEncoder.EncodeStructure(buffer, paletteData.paletteData.paletteEntries[i].green);
                    RdpbcgrEncoder.EncodeStructure(buffer, paletteData.paletteData.paletteEntries[i].blue);
                }
            }

            if (paletteData.shareDataHeader.compressedType != compressedType_Values.None)
            {
                RdpbcgrEncoder.EncodeBytes(
                    totalBuffer, serverSessionContext.Compress(paletteData.shareDataHeader.compressedType, buffer.ToArray()));
            }
            else
            {
                RdpbcgrEncoder.EncodeBytes(totalBuffer, buffer.ToArray());
            }
            return totalBuffer.ToArray();
        }

        private byte[] EncodeBitmap(TS_UPDATE_BITMAP bitmapData)
        {
            List<byte> totalBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(totalBuffer, bitmapData.shareDataHeader);

            List<byte> buffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(buffer, (ushort)bitmapData.bitmapData.updateType);
            RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapData.numberRectangles);
            if (bitmapData.bitmapData.rectangles != null)
            {
                for (int i = 0; i < bitmapData.bitmapData.rectangles.Length; ++i)
                {
                    RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapData.rectangles[i].destLeft);
                    RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapData.rectangles[i].destTop);
                    RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapData.rectangles[i].destRight);
                    RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapData.rectangles[i].destBottom);
                    RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapData.rectangles[i].width);
                    RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapData.rectangles[i].height);
                    RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapData.rectangles[i].bitsPerPixel);
                    RdpbcgrEncoder.EncodeStructure(buffer, (short)bitmapData.bitmapData.rectangles[i].Flags);
                    RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapData.rectangles[i].bitmapLength);
                    RdpbcgrEncoder.EncodeStructure(
                        buffer, (short)bitmapData.bitmapData.rectangles[i].bitmapComprHdr.cbCompFirstRowSize);
                    RdpbcgrEncoder.EncodeStructure(
                        buffer, bitmapData.bitmapData.rectangles[i].bitmapComprHdr.cbCompMainBodySize);
                    RdpbcgrEncoder.EncodeStructure(
                        buffer, bitmapData.bitmapData.rectangles[i].bitmapComprHdr.cbScanWidth);
                    RdpbcgrEncoder.EncodeStructure(
                        buffer, bitmapData.bitmapData.rectangles[i].bitmapComprHdr.cbUncompressedSize);
                    RdpbcgrEncoder.EncodeBytes(buffer, bitmapData.bitmapData.rectangles[i].bitmapDataStream);
                }
            }

            if (bitmapData.shareDataHeader.compressedType != compressedType_Values.None)
            {
                RdpbcgrEncoder.EncodeBytes(
                    totalBuffer, serverSessionContext.Compress(bitmapData.shareDataHeader.compressedType, buffer.ToArray()));
            }
            else
            {
                RdpbcgrEncoder.EncodeBytes(totalBuffer, buffer.ToArray());
            }
            return totalBuffer.ToArray();
        }

        private byte[] EncodeSync(TS_UPDATE_SYNC syncData)
        {
            List<byte> totalBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(totalBuffer, syncData.shareDataHeader);

            List<byte> buffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(buffer, (ushort)syncData.updateType);
            RdpbcgrEncoder.EncodeStructure(buffer, syncData.pad2Octets);

            if (syncData.shareDataHeader.compressedType != compressedType_Values.None)
            {
                RdpbcgrEncoder.EncodeBytes(
                    totalBuffer, serverSessionContext.Compress(syncData.shareDataHeader.compressedType, buffer.ToArray()));
            }
            else
            {
                RdpbcgrEncoder.EncodeBytes(totalBuffer, buffer.ToArray());
            }
            return totalBuffer.ToArray();
        }

        private byte[] EncodePtr(TS_POINTER_PDU ptrData)
        {
            List<byte> totalBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(totalBuffer, ptrData.shareDataHeader);

            List<byte> buffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(buffer, (ushort)ptrData.messageType);
            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.pad2Octets);
            if (ptrData.pointerAttributeData.GetType() == typeof(byte[]))
            {
                RdpbcgrEncoder.EncodeBytes(buffer, (byte[])ptrData.pointerAttributeData);
            }
            else
            {
                RdpbcgrEncoder.EncodeStructure(buffer, ptrData.pointerAttributeData);
            }


            if (ptrData.shareDataHeader.compressedType != compressedType_Values.None)
            {
                RdpbcgrEncoder.EncodeBytes(
                    totalBuffer, serverSessionContext.Compress(ptrData.shareDataHeader.compressedType, buffer.ToArray()));
            }
            else
            {
                RdpbcgrEncoder.EncodeBytes(totalBuffer, buffer.ToArray());
            }
            return totalBuffer.ToArray();
        }
    }

    /// <summary>
    ///  Update of slow path PDU. 
    ///  It's a base class of Graphics update and Pointer update.
    /// </summary>
    public class RdpbcgrSlowPathUpdatePdu
    {
    }

    /// <summary>
    /// The type of updateType.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum updateType_Values : ushort
    {
        /// <summary>
        ///  Indicates an Orders Update (see [MS-RDPEGDI] section
        ///  2.2.2.2).
        /// </summary>
        UPDATETYPE_ORDERS = 0x0000,

        /// <summary>
        ///  Indicates a Bitmap Graphics Update (see section ).
        /// </summary>
        UPDATETYPE_BITMAP = 0x0001,

        /// <summary>
        ///  Indicates a Palette Update (see section ).
        /// </summary>
        UPDATETYPE_PALETTE = 0x0002,

        /// <summary>
        ///  Indicates a Synchronize Update (see section ).
        /// </summary>
        UPDATETYPE_SYNCHRONIZE = 0x0003,

        /// <summary>
        ///  Indicates a Surface Command Update (see section 2.2.9.1.1.3.1.4).
        /// </summary>
        UPDATETYPE_SURFCMDS = 0x0004,
    }

    /// <summary>
    ///  Indicates an Orders Update (see [MS-RDPEGDI] section 2.2.2.2).
    /// </summary>
    public partial class TS_UPDATE_ORDERS : RdpbcgrSlowPathUpdatePdu
    {
        /// <summary>
        ///  Share Data Header (section 2.2.8.1.1.1.2) containing information 
        ///  about the packet. The type subfield of the pduType field of the 
        ///  Share Control Header (section 2.2.8.1.1.1.1) MUST be set to 
        ///  PDUTYPE_DATAPDU (7). The pduType2 field of the Share Data Header 
        ///  MUST be set to PDUTYPE2_UPDATE (2).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        /// A 16-bit, unsigned integer. The update type. 
        /// This field MUST be set to UPDATETYPE_ORDERS (0x0000).
        /// </summary>
        public updateType_Values updateType;

        /// <summary>
        /// Fields below see [MS-RDPEGDI] section 2.2.2.2.
        /// </summary>
        public ushort pad2OctetA;

        /// <summary>
        /// The number of Orders
        /// </summary>
        public ushort numberOrders;

        /// <summary>
        /// pad.
        /// </summary>
        public ushort pad2OctetsB;

        /// <summary>
        /// The order data.
        /// </summary>
        public byte[] orderData;
    }

    /// <summary>
    ///  The TS_UPDATE_PALETTE_PDU_DATA structure contains
    ///  global palette information that covers the entire session's
    ///  palette (see [T128] section 8.18.6).
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_1_3_1_1.xml
    //  </remarks>
    public partial class TS_UPDATE_PALETTE : RdpbcgrSlowPathUpdatePdu
    {
        /// <summary>
        ///  Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share
        ///  Control Header MUST be set to PDUTYPE_DATAPDU (7).
        ///  The pduType2 field of the Share Data Header MUST be
        ///  set to PDUTYPE2_UPDATE (2).           
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  The actual palette update data, as specified in section 2.2.9.1.1.3.1.1.1.       
        /// </summary>
        public TS_UPDATE_PALETTE_DATA paletteData;
    }

    /// <summary>
    ///  The TS_UPDATE_PALETTE_DATA encapsulates the palette data that defines 
    ///  a Palette Update (section 2.2.9.1.1.3.1.1).
    /// </summary>
    public struct TS_UPDATE_PALETTE_DATA
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The graphics update type.
        ///  This field MUST be set to UPDATETYPE_PALETTE (0x0002).
        /// </summary>
        public updateType_Values updateType;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2Octets;

        /// <summary>
        ///  A 32-bit unsigned integer.  The number of RGB triplets
        ///  in the paletteData field. This field MUST be set to
        ///  NUM_8BPP_PAL_ENTRIES (256).
        /// </summary>
        public uint numberColors;

        /// <summary>
        ///  Array of TS_PALETTE_ENTRY structures. Array of palette
        ///  entries in RGB triplet format (see section ) packed
        ///  on byte boundaries. The number of triplet entries is
        ///  given by the numberColors field - there must be NUM_8BPP_PAL_ENTRIES
        ///  (256) entries.
        /// </summary>
        public TS_PALETTE_ENTRY[] paletteEntries;
    }

    /// <summary>
    ///  The TS_PALETTE_ENTRY structure is used to express
    ///  the red, green and blue components necessary to reproduce
    ///  a color in the additive RGB space.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_1_3_1_1_1.xml
    //  </remarks>
    public partial struct TS_PALETTE_ENTRY
    {

        /// <summary>
        ///  An 8-bit unsigned integer. The red RGB color component.
        /// </summary>
        public byte red;

        /// <summary>
        ///  An 8-bit unsigned integer. The green RGB color component.
        /// </summary>
        public byte green;

        /// <summary>
        ///  An 8-bit unsigned integer. The blue RGB color component.
        /// </summary>
        public byte blue;
    }

    /// <summary>
    ///  The TS_UPDATE_BITMAP_PDU_DATA structure contains
    ///  one or more rectangular clippings taken from the server-side
    ///  screen frame buffer (see [T128] section 8.17).
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_1_3_1_2.xml
    //  </remarks>
    public partial class TS_UPDATE_BITMAP : RdpbcgrSlowPathUpdatePdu
    {
        /// <summary>
        ///  Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share
        ///  Control Header MUST be set to PDUTYPE_DATAPDU (7).
        ///  The pduType2 field of the Share Data Header MUST be
        ///  set to PDUTYPE2_UPDATE (2).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  The actual bitmap update data, as specified in section 2.2.9.1.1.3.1.2.1.
        /// </summary>
        public TS_UPDATE_BITMAP_DATA bitmapData;
    }

    /// <summary>
    ///  The TS_UPDATE_BITMAP_DATA structure encapsulates the bitmap data 
    ///  that defines a Bitmap Update (section 2.2.9.1.1.3.1.2).
    /// </summary>
    public struct TS_UPDATE_BITMAP_DATA
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The graphics update type.
        ///  This field MUST be set to UPDATETYPE_BITMAP (0x0001).
        /// </summary>
        public ushort updateType;

        /// <summary>
        ///  A 16-bit unsigned integer. The number of screen rectangles
        ///  present in the rectangles field.
        /// </summary>
        public ushort numberRectangles;

        /// <summary>
        ///  Variable length array of TS_BITMAP_DATA structures,
        ///  each of which contains a rectangular clipping taken
        ///  from the server-side screen frame buffer. The number
        ///  of screen clippings in the array is specified by the
        ///  numberRectangles field.
        /// </summary>
        //[StaticSize(1, StaticSizeMode.Elements)]
        public TS_BITMAP_DATA[] rectangles;
    }

    /// <summary>
    ///  The TS_BITMAP_DATA structure wraps the bitmap data bytestream
    ///  for a screen area rectangle containing a clipping taken
    ///  from the server-side screen frame buffer.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_1_3_1_2_1.xml
    //  </remarks>
    public partial struct TS_BITMAP_DATA
    {
        /// <summary>
        ///  A 16-bit unsigned integer. Left bound of the rectangle.
        /// </summary>
        public ushort destLeft;

        /// <summary>
        ///  A 16-bit unsigned integer. Top bound of the rectangle.
        /// </summary>
        public ushort destTop;

        /// <summary>
        ///  A 16-bit unsigned integer. Right bound of the rectangle.
        /// </summary>
        public ushort destRight;

        /// <summary>
        ///  A 16-bit unsigned integer. Bottom bound of the rectangle.
        /// </summary>
        public ushort destBottom;

        /// <summary>
        ///  A 16-bit unsigned integer. The width of the rectangle.
        /// </summary>
        public ushort width;

        /// <summary>
        ///  A 16-bit unsigned integer. The height of the rectangle.
        /// </summary>
        public ushort height;

        /// <summary>
        ///  A 16-bit unsigned integer. The color depth of the rectangle
        ///  data in bits-per-pixel.
        /// </summary>
        public ushort bitsPerPixel;

        /// <summary>
        ///  A 16-bit unsigned integer. The flags describing the
        ///  format of the bitmap data in the bitmapDataStream field.
        /// </summary>
        public TS_BITMAP_DATA_Flags_Values Flags;

        /// <summary>
        ///               A 16-bit unsigned integer. The size in
        ///  bytes of the data in the bitmapComprHdr and bitmapDataStream
        ///  fields.
        /// </summary>
        public ushort bitmapLength;

        /// <summary>
        ///  Optional Compressed Data Header structure (see Compressed
        ///  Data Header (TS_CD_HEADER)) specifying the bitmap data
        ///  in the bitmapDataStream. This field MUST be present
        ///  if the BITMAP_COMPRESSION (0x0001) flag is present
        ///  in the Flags field, but the NO_BITMAP_COMPRESSION_HDR
        ///  (0x0400) flag is not.
        /// </summary>
        public TS_CD_HEADER bitmapComprHdr;

        /// <summary>
        ///  A variable-sized array of bytes. Uncompressed bitmap
        ///  data represents a bitmap as a bottom-up, left-to-right
        ///  series of pixels. Each pixel is a whole number of bytes.
        ///  Each row contains a multiple of four bytes (including
        ///  up to three bytes of padding, as necessary). Compressed
        ///   bitmaps not in 32 bits-per-pixel format are compressed
        ///  using Interleaved Run-Length Encoding (RLE) and encapsulated
        ///  in an RLE Compressed Bitmap Stream structure (see
        ///  section )  while compressed bitmap data at a color
        ///  depth of 32 bits-per-pixel is compressed using RDP
        ///  6.0 Bitmap Compression and stored inside an RDP 6.0
        ///  Bitmap Compressed Stream structure (see section  in
        ///  [MS-RDPEGDI]).
        /// </summary>
        public byte[] bitmapDataStream;
    }

    /// <summary>
    /// The type of Flags.
    /// </summary>
    public enum TS_BITMAP_DATA_Flags_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates that the bitmap data is compressed. This implies
        ///  that the bitmapComprHdr field is present if the NO_BITMAP_COMPRESSION_HDR
        ///  (0x0400) flag is not set.
        /// </summary>
        BITMAP_COMPRESSION = 0x0001,

        /// <summary>
        ///  Indicates that the bitmapComprHdr field is not present
        ///  (removed for bandwidth efficiency to save 8 bytes).
        /// </summary>
        NO_BITMAP_COMPRESSION_HDR = 0x0400,
    }

    /// <summary>
    ///  The TS_UPDATE_SYNC_PDU_DATA structure is an artifact
    ///  of the T.128 protocol (see [T128] section 8.6.2) and
    ///  is ignored by current Microsoft RDP client implementations.
    ///  				     
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_1_3_1_3.xml
    //  </remarks>
    public partial class TS_UPDATE_SYNC : RdpbcgrSlowPathUpdatePdu
    {
        /// <summary>
        ///  Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share
        ///  Control Header MUST be set to PDUTYPE_DATAPDU (7).
        ///  The pduType2 field of the Share Data Header MUST be
        ///  set to PDUTYPE2_UPDATE (2).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  A 16-bit unsigned integer. Graphics update type. This
        ///  field MUST be set to UPDATETYPE_SYNCHRONIZE (0x0003).
        /// </summary>
        public ushort updateType;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2Octets;
    }

    /// <summary>
    ///  The Pointer Update PDU is sent from server to client
    ///  and is used to convey pointer information, including
    ///  pointers' bitmap images, use of system or hidden pointers,
    ///  use of cached cursors and position updates.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_1_4.xml
    //  </remarks>
    public partial class TS_POINTER_PDU : RdpbcgrSlowPathUpdatePdu
    {
        /// <summary>
        ///  Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share
        ///  Control Header MUST be set to PDUTYPE_DATAPDU (7).
        ///  The pduType2 field of the Share Data Header MUST be
        ///  set to PDUTYPE2_POINTER (27).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  A 16-bit unsigned integer. Type of pointer update.
        /// </summary>
        public ushort messageType;

        /// <summary>
        ///  A 16-bit unsigned integer. Padding. Values in this field
        ///  are ignored.
        /// </summary>
        public ushort pad2Octets;

        /// <summary>
        ///  TS_POINTERPOSATTRIBUTE (4 bytes), TS_SYSTEMPOINTERATTRIBUTE
        ///  (4 bytes), TS_COLORPOINTERATTRIBUTE (variable number
        ///  of bytes), TS_POINTERATTRIBUTE (variable number of
        ///  bytes) or TS_CACHEDPOINTERATTRIBUTE (2 bytes):The actual
        ///  contents of the slow-path pointer update (see sections
        ///   to ).
        /// </summary>
        public object pointerAttributeData;
    }

    /// <summary>
    /// The type of messageType.
    /// </summary>
    public enum TS_POINTER_PDU_messageType_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates a System Pointer Update.
        /// </summary>
        TS_PTRMSGTYPE_SYSTEM = 0x0001,

        /// <summary>
        ///  Indicates a Pointer Position Update.
        /// </summary>
        TS_PTRMSGTYPE_POSITION = 0x0003,

        /// <summary>
        ///  Indicates a Color Pointer Update.
        /// </summary>
        TS_PTRMSGTYPE_COLOR = 0x0006,

        /// <summary>
        ///  Indicates a Cached Pointer Update.
        /// </summary>
        TS_PTRMSGTYPE_CACHED = 0x0007,

        /// <summary>
        ///  Indicates a New Pointer Update.
        /// </summary>
        TS_PTRMSGTYPE_POINTER = 0x0008,
    }

    /// <summary>
    ///  The TS_POINT16 structure specifies a point relative
    ///  to the top-left corner of the server's virtual desktop.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_1_4_1.xml
    //  </remarks>
    public partial struct TS_POINT16
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The x-coordinate
        ///  relative to the top-left corner of the server's virtual
        ///  desktop.
        /// </summary>
        public ushort xPos;

        /// <summary>
        ///  A 16-bit unsigned integer. The y-coordinate
        ///  relative to the top-left corner of the server's virtual
        ///  desktop.
        /// </summary>
        public ushort yPos;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">xPos</param>
        /// <param name="y">yPos</param>
        public TS_POINT16(ushort x, ushort y)
        {
            this.xPos = x;
            this.yPos = y;
        }
    }

    /// <summary>
    ///  The TS_POINTERPOSATTRIBUTE structure is used to indicate
    ///  that the client pointer should be moved to the specified
    ///  position relative to the top-left corner of the server's
    ///  virtual desktop (see [T128] section 8.14.4).
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_1_4_2.xml
    //  </remarks>
    public partial struct TS_POINTERPOSATTRIBUTE
    {
        /// <summary>
        ///  Point structure containing the new x- and y-coordinates
        ///  of the pointer. 
        /// </summary>
        public TS_POINT16 position;
    }

    /// <summary>
    ///  The TS_SYSTEMPOINTERATTRIBUTE structure is used to hide
    ///  the pointer or to set its shape to that of the operating
    ///  system default (see  [T128] section 8.14.1).
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_1_4_3.xml
    //  </remarks>
    public partial struct TS_SYSTEMPOINTERATTRIBUTE
    {
        /// <summary>
        ///  A 32-bit unsigned integer. The type of system pointer.
        /// </summary>
        public uint systemPointerType;
    }

    /// <summary>
    /// The type of systemPointerType.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum systemPointerType_Values : uint
    {

        /// <summary>
        ///  The hidden pointer.
        /// </summary>
        SYSPTR_NULL = 0x00000000,

        /// <summary>
        ///  The default system pointer.
        /// </summary>
        SYSPTR_DEFAULT = 0x00007F00,
    }

    /// <summary>
    ///  The TS_COLORPOINTERATTRIBUTE structure represents a
    ///  regular T.128 24 bits-per-pixel color pointer, as specified
    ///  in [T128] section 8.14.3. This pointer update is used
    ///  for both monochrome and color pointers in RDP.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_1_4_4.xml
    //  </remarks>
    public partial struct TS_COLORPOINTERATTRIBUTE
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The zero-based cache entry
        ///  in the pointer cache in which to store the pointer
        ///  image. The number of cache entries is negotiated using
        ///  the Pointer Capability Set.
        /// </summary>
        public ushort cacheIndex;

        /// <summary>
        ///  Point structure containing the X and Y coordinates of
        ///  the pointer hotspot.
        /// </summary>
        public TS_POINT16 hotSpot;

        /// <summary>
        ///  A 16-bit unsigned integer. The width of the pointer
        ///  in pixels (the maximum allowed pointer width is 32
        ///  pixels).
        /// </summary>
        public ushort width;

        /// <summary>
        ///  A 16-bit unsigned integer. The height of
        ///  the pointer in pixels (the maximum allowed pointer
        ///  height is 32 pixels).
        /// </summary>
        public ushort height;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes
        ///  of the andMaskData field.
        /// </summary>
        public ushort lengthAndMask;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes
        ///  of the xorMaskData field.
        /// </summary>
        public ushort lengthXorMask;

        /// <summary>
        ///  Variable number of bytes: 				Contains the 24 bits-per-pixel
        ///  bottom-up XOR mask scan-line data. The XOR mask is
        ///  padded to a 2-byte boundary for each encoded scan-line.
        ///  For example, if a 3x3 pixel cursor is being sent, then
        ///  each scan-line will consume 10 bytes (3 pixels per
        ///  scan-line multiplied by 3 bytes per pixel, rounded
        ///  up to the next even number of bytes).
        /// </summary>
        public byte[] xorMaskData;

        /// <summary>
        ///  Variable number of bytes: 				Contains the 1 bit-per-pixel
        ///  bottom-up AND mask scan-line data. The AND mask is
        ///  padded to a 2-byte boundary for each encoded scan-line.
        ///  For example, if a 7x7 pixel cursor is being sent, then
        ///  each scan-line will consume 2 bytes (7 pixels per scan-line
        ///  multiplied by 1 bit per pixel, rounded up to the next
        ///  even number of bytes).
        /// </summary>
        public byte[] andMaskData;

        /// <summary>
        ///  An 8-bit, unsigned integer. Padding. Values in this field MUST be ignored.
        /// </summary>
        public byte pad;
    }

    /// <summary>
    ///  The TS_POINTERATTRIBUTE structure is used to send
    ///  pointer data at an arbitrary color depth. Support for
    ///  the New Pointer Update is advertised in the Pointer
    ///  Capability Set. 				 				 		
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_1_4_5.xml
    //  </remarks>
    public partial struct TS_POINTERATTRIBUTE
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The color depth
        ///  in bits-per-pixel of the XOR mask contained in the
        ///  colorPtrAttr field.
        /// </summary>
        public ushort xorBpp;

        /// <summary>
        ///   Encapsulated Color Pointer Update structure which contains
        ///  information about the pointer. The Color Pointer Update
        ///  fields are all used, as specified in section ; however,
        ///  the XOR mask data alignment packing is slightly different.
        ///  For monochrome (1 bit-per-pixel) pointers the XOR data
        ///  is always padded to a 4-byte boundary per scan line,
        ///  while color pointer XOR data is still packed on a 2-byte
        ///  boundary. Color XOR data is presented in the color
        ///  depth described in the xorBpp field (for 8 bits-per-pixel,
        ///  each byte contains one palette index; for 4 bits-per-pixel
        ///  there are two palette indices per byte).
        /// </summary>
        public TS_COLORPOINTERATTRIBUTE colorPtrAttr;
    }

    /// <summary>
    ///  The TS_CACHEDPOINTERATTRIBUTE structure is used
    ///  to instruct the client to change the current pointer
    ///  shape to one already present in the pointer cache.
    ///  				 				 	
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_1_4_6.xml
    //  </remarks>
    public partial struct TS_CACHEDPOINTERATTRIBUTE
    {

        /// <summary>
        ///  A 16-bit unsigned integer. A zero-based cache
        ///  entry containing the cache index of the cached pointer
        ///  to which the client's pointer should be changed. The
        ///  pointer data should have already been cached using
        ///  either the Color Pointer Update or New Pointer Update.
        /// </summary>
        public ushort cacheIndex;
    }

    /// <summary>
    ///  Fast-path revises server output packets from the first
    ///  byte with the goal of improving bandwidth. The TPKT
    ///  (see [T123]), X.224 (see [X224]) and MCS SDin (see
    ///  [T125]) headers are replaced, the Security Header is
    ///  collapsed into the fast-path output header, and the
    ///  Share Data Header is replaced by a new fast-path format.
    ///  The contents of the graphics and pointer updates (see
    ///  sections  and ) are also changed to reduce their size,
    ///  particularly by removing or reducing headers. Support
    ///  for fast-path output is advertised in the General Capability
    ///  Set.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_2.xml
    //  </remarks>
    public partial class TS_FP_UPDATE_PDU : RdpbcgrServerPdu
    {
        /// <summary>
        ///  An 8-bit unsigned integer. One-byte bit-packed header.
        ///  This byte coincides with the first byte of the TPKT
        ///  Header (see [T123] section 8), which is always 0x03.
        ///  Two pieces of information are collapsed into this byte:Encryption
        ///  dataAction code The format of the fpOutputHeader byte
        ///  is described by the following bitmask diagram:
        /// </summary>
        public byte fpOutputHeader;

        /// <summary>
        ///  An 8-bit unsigned integer. If the most significant bit
        ///  of the length1 field is not set, then the size of the
        ///  PDU is in the range 1 to 127 bytes and the length1
        ///  field contains the overall PDU length (the length2
        ///  field is not present in this case). However, if the
        ///  most significant bit of the length1 field is set, then
        ///  the overall PDU length is given by the low 7 bits of
        ///  the length1 field concatenated with the 8 bits of the
        ///  length2 field, in big-endian order (the length2 field
        ///  contains the low-order bits).
        /// </summary>
        public byte length1;

        /// <summary>
        ///  An 8-bit unsigned integer. If the most significant bit
        ///  of the length1 field is not set, then the length2 field
        ///  is not present. If the most significant bit of the
        ///  length1 field is set, then the overall PDU length is
        ///  given by the low 7 bits of the length1 field concatenated
        ///  with the 8 bits of the length2 field, in big-endian
        ///  order (the length2 field contains the low-order bits).
        /// </summary>
        public byte length2;

        /// <summary>
        ///  Optional FIPS header information, present when
        ///  the Encryption Level selected by the server (see sections
        ///   and ) is ENCRYPTION_LEVEL_FIPS (4). The fast-path
        ///  FIPS information structure is specified in section
        ///  .
        /// </summary>
        public TS_FP_FIPS_INFO fipsInformation;

        /// <summary>
        ///  8 bytes. Message authentication code (MAC) generated
        ///  over the packet using one of the techniques specified
        ///  in section  (the FASTPATH_INPUT_SECURE_CHECKSUM flag,
        ///  which is set in the fpInputHeader field, describes
        ///  the method used to generate the signature). This field
        ///  is present if the FASTPATH_INPUT_ENCRYPTED flag is
        ///  set in the fpInputHeader field.
        /// </summary>
        public byte[] dataSignature;

        /// <summary>
        ///  An array of TS_FP_UPDATE structures (variable
        ///  number of bytes) containing a collection of fast-path
        ///  updates to be processed by the client.
        /// </summary>
        public TS_FP_UPDATE[] fpOutputUpdates;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public TS_FP_UPDATE_PDU(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public TS_FP_UPDATE_PDU()
        {
        }

        public override StackPacket Clone()
        {
            throw new NotImplementedException();
        }

        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();

            byte[] fpOutputdata = EncodeFpOutputData();
            byte[] encryptedData = null;
            byte[] signature = null;


            if (serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW)
            {
                if (serverSessionContext.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS
                    && fipsInformation.padlen == 0)
                {
                    fipsInformation.padlen = (byte)(ConstValue.TRIPLE_DES_PAD
                                           - (fpOutputdata.Length % ConstValue.TRIPLE_DES_PAD));
                }

                // encryptionFlags (2 bits): A higher 2-bit field containing the flags 
                // that describe the cryptographic parameters of the PDU.
                bool isSalted = ((fpOutputHeader >> 6) &
                                 (int)encryptionFlags_Values.FASTPATH_INPUT_SECURE_CHECKSUM)
                                 == (int)encryptionFlags_Values.FASTPATH_INPUT_SECURE_CHECKSUM;
                serverSessionContext.ServerEncrypt(fpOutputdata, isSalted, out encryptedData, out signature);
                if (dataSignature == null)
                {
                    dataSignature = signature;
                }
            }
            else
            {
                encryptedData = fpOutputdata;
            }

            byte[] fpHeaderData = EncodeFpHeaderData(encryptedData.Length);

            RdpbcgrEncoder.EncodeBytes(totalBuffer, fpHeaderData);
            RdpbcgrEncoder.EncodeBytes(totalBuffer, encryptedData);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, fpOutputdata);

            return totalBuffer.ToArray();
        }

        /// <summary>
        /// Encode the fast path header data
        /// </summary>
        /// <param name="dataLength">The length after data signature part.</param>
        /// <returns>the encoded fastPath headerData array</returns>
        private byte[] EncodeFpHeaderData(int dataLength)
        {
            List<byte> fpHeaderData = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(fpHeaderData, fpOutputHeader);

            int totalLength = Marshal.SizeOf(fpOutputHeader) + Marshal.SizeOf(length1) + dataLength;
            if (serverSessionContext.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS)
            {
                totalLength += Marshal.SizeOf(fipsInformation);
            }

            if (serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW)
            {
                totalLength += dataSignature.Length;
            }

            // if the most significant bit of the length1 field is set
            if (totalLength > 0x7F)
            {
                ++totalLength;

                // The overall PDU length is given by the low 7 bits of the length1 field 
                // concatenated with the 8 bits of the length2 field, in big-endian order 
                // (the length2 field contains the low-order bits).
                length1 = (byte)((totalLength >> 8) | 0x80);
                length2 = (byte)(totalLength & 0xFF);
                RdpbcgrEncoder.EncodeStructure(fpHeaderData, length1);
                RdpbcgrEncoder.EncodeStructure(fpHeaderData, length2);
            }
            else
            {
                // If the most significant bit of the length1 field is not set, then the size of the PDU is 
                // in the range 1 to 127 bytes and the length1 field contains the overall PDU length 
                // (the length2 field is not present in this case). 
                length1 = (byte)(totalLength);
                length2 = 0;
                RdpbcgrEncoder.EncodeStructure(fpHeaderData, length1);
            }

            if (serverSessionContext.RdpEncryptionLevel == EncryptionLevel.ENCRYPTION_LEVEL_FIPS)
            {
                RdpbcgrEncoder.EncodeStructure(fpHeaderData, fipsInformation);
            }

            // encryptionFlags (2 bits): A higher 2-bit field containing the flags 
            // that describe the cryptographic parameters of the PDU.
            if (((fpOutputHeader >> 6) & (int)encryptionFlags_Values.FASTPATH_INPUT_ENCRYPTED)
                == (int)encryptionFlags_Values.FASTPATH_INPUT_ENCRYPTED)
            {
                RdpbcgrEncoder.EncodeBytes(fpHeaderData, dataSignature);
            }

            return fpHeaderData.ToArray();
        }

        private byte[] EncodeFpOutputData()
        {
            List<byte> fastpathOutputData = new List<byte>();
            if (fpOutputUpdates != null)
            {
                for (int i = 0; i < fpOutputUpdates.Length; ++i)
                {
                    if (fpOutputUpdates[i].GetType() == typeof(TS_FP_UPDATE_PALETTE))
                    {
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].updateHeader);
                        if ((int)fpOutputUpdates[i].updateHeader >> 6 == 2)
                        {
                            RdpbcgrEncoder.EncodeStructure(fastpathOutputData, (byte)fpOutputUpdates[i].compressionFlags);
                        }
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].size);
                        RdpbcgrEncoder.EncodeBytes(fastpathOutputData,
                            EncodePalette((TS_FP_UPDATE_PALETTE)fpOutputUpdates[i]));
                    }

                    else if (fpOutputUpdates[i].GetType() == typeof(TS_FP_UPDATE_BITMAP))
                    {
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].updateHeader);
                        if ((int)fpOutputUpdates[i].updateHeader >> 6 == 2)
                        {
                            RdpbcgrEncoder.EncodeStructure(fastpathOutputData, (byte)fpOutputUpdates[i].compressionFlags);
                        }
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].size);
                        RdpbcgrEncoder.EncodeBytes(fastpathOutputData,
                            EncodeBitmap((TS_FP_UPDATE_BITMAP)fpOutputUpdates[i]));
                    }

                    else if (fpOutputUpdates[i].GetType() == typeof(TS_FP_UPDATE_SYNCHRONIZE))
                    {
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].updateHeader);
                        if ((int)fpOutputUpdates[i].updateHeader >> 6 == 2)
                        {
                            RdpbcgrEncoder.EncodeStructure(fastpathOutputData, (byte)fpOutputUpdates[i].compressionFlags);
                        }
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].size);
                    }

                    else if (fpOutputUpdates[i].GetType() == typeof(TS_FP_POINTERPOSATTRIBUTE))
                    {
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].updateHeader);
                        if ((int)fpOutputUpdates[i].updateHeader >> 6 == 2)
                        {
                            RdpbcgrEncoder.EncodeStructure(fastpathOutputData, (byte)fpOutputUpdates[i].compressionFlags);
                        }
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].size);
                        RdpbcgrEncoder.EncodeBytes(fastpathOutputData,
                            EncodePtrPos((TS_FP_POINTERPOSATTRIBUTE)fpOutputUpdates[i]));
                    }

                    else if (fpOutputUpdates[i].GetType() == typeof(TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE))
                    {
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].updateHeader);
                        if ((int)fpOutputUpdates[i].updateHeader >> 6 == 2)
                        {
                            RdpbcgrEncoder.EncodeStructure(fastpathOutputData, (byte)fpOutputUpdates[i].compressionFlags);
                        }
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].size);
                    }

                    else if (fpOutputUpdates[i].GetType() == typeof(TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE))
                    {
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].updateHeader);
                        if ((int)fpOutputUpdates[i].updateHeader >> 6 == 2)
                        {
                            RdpbcgrEncoder.EncodeStructure(fastpathOutputData, (byte)fpOutputUpdates[i].compressionFlags);
                        }
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].size);
                    }

                    else if (fpOutputUpdates[i].GetType() == typeof(TS_FP_COLORPOINTERATTRIBUTE))
                    {
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].updateHeader);
                        if ((int)fpOutputUpdates[i].updateHeader >> 6 == 2)
                        {
                            RdpbcgrEncoder.EncodeStructure(fastpathOutputData, (byte)fpOutputUpdates[i].compressionFlags);
                        }
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].size);
                        RdpbcgrEncoder.EncodeBytes(fastpathOutputData,
                            EncodeColorPtr((TS_FP_COLORPOINTERATTRIBUTE)fpOutputUpdates[i]));
                    }

                    else if (fpOutputUpdates[i].GetType() == typeof(TS_FP_POINTERATTRIBUTE))
                    {
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].updateHeader);
                        if ((int)fpOutputUpdates[i].updateHeader >> 6 == 2)
                        {
                            RdpbcgrEncoder.EncodeStructure(fastpathOutputData, (byte)fpOutputUpdates[i].compressionFlags);
                        }
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].size);
                        RdpbcgrEncoder.EncodeBytes(fastpathOutputData,
                            EncodeNewPtr((TS_FP_POINTERATTRIBUTE)fpOutputUpdates[i]));
                    }

                    else if (fpOutputUpdates[i].GetType() == typeof(TS_FP_CACHEDPOINTERATTRIBUTE))
                    {
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].updateHeader);
                        if ((int)fpOutputUpdates[i].updateHeader >> 6 == 2)
                        {
                            RdpbcgrEncoder.EncodeStructure(fastpathOutputData, (byte)fpOutputUpdates[i].compressionFlags);
                        }
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].size);
                        RdpbcgrEncoder.EncodeBytes(fastpathOutputData,
                            EncodeCachedPtr((TS_FP_CACHEDPOINTERATTRIBUTE)fpOutputUpdates[i]));
                    }

                    else if (fpOutputUpdates[i].GetType() == typeof(TS_FP_SURFCMDS))
                    {
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].updateHeader);

                        // The two bit compression flag in updateHeader indicates if the compressionFlags field is present
                        if ((int)fpOutputUpdates[i].updateHeader >> 6 == 2)
                        {
                            RdpbcgrEncoder.EncodeStructure(fastpathOutputData, (byte)fpOutputUpdates[i].compressionFlags);
                        }
                        byte[] surfUpdateData = EncodeSurfCmds((TS_FP_SURFCMDS)fpOutputUpdates[i]);
                        fpOutputUpdates[i].size = (ushort)surfUpdateData.Length;
                        RdpbcgrEncoder.EncodeStructure(fastpathOutputData, fpOutputUpdates[i].size);
                        RdpbcgrEncoder.EncodeBytes(fastpathOutputData, surfUpdateData);
                    }
                }
            }

            return fastpathOutputData.ToArray();
        }

        private byte[] EncodePalette(TS_FP_UPDATE_PALETTE paletteData)
        {
            List<byte> totalBuffer = new List<byte>();

            List<byte> buffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(buffer, (ushort)paletteData.paletteUpdateData.updateType);
            RdpbcgrEncoder.EncodeStructure(buffer, paletteData.paletteUpdateData.pad2Octets);
            RdpbcgrEncoder.EncodeStructure(buffer, paletteData.paletteUpdateData.numberColors);
            if (paletteData.paletteUpdateData.paletteEntries != null)
            {
                for (int i = 0; i < paletteData.paletteUpdateData.paletteEntries.Length; ++i)
                {
                    RdpbcgrEncoder.EncodeStructure(buffer, paletteData.paletteUpdateData.paletteEntries[i].red);
                    RdpbcgrEncoder.EncodeStructure(buffer, paletteData.paletteUpdateData.paletteEntries[i].green);
                    RdpbcgrEncoder.EncodeStructure(buffer, paletteData.paletteUpdateData.paletteEntries[i].blue);
                }
            }

            if (paletteData.compressionFlags != compressedType_Values.None)
            {
                RdpbcgrEncoder.EncodeBytes(
                    totalBuffer, serverSessionContext.Compress(paletteData.compressionFlags, buffer.ToArray()));
            }
            else
            {
                RdpbcgrEncoder.EncodeBytes(totalBuffer, buffer.ToArray());
            }
            return totalBuffer.ToArray();
        }

        private byte[] EncodeBitmap(TS_FP_UPDATE_BITMAP bitmapData)
        {
            List<byte> totalBuffer = new List<byte>();
            List<byte> buffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(buffer, (ushort)bitmapData.bitmapUpdateData.updateType);
            RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapUpdateData.numberRectangles);
            if (bitmapData.bitmapUpdateData.rectangles != null)
            {
                for (int i = 0; i < bitmapData.bitmapUpdateData.rectangles.Length; ++i)
                {
                    RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapUpdateData.rectangles[i].destLeft);
                    RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapUpdateData.rectangles[i].destTop);
                    RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapUpdateData.rectangles[i].destRight);
                    RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapUpdateData.rectangles[i].destBottom);
                    RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapUpdateData.rectangles[i].width);
                    RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapUpdateData.rectangles[i].height);
                    RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapUpdateData.rectangles[i].bitsPerPixel);
                    RdpbcgrEncoder.EncodeStructure(buffer, (ushort)bitmapData.bitmapUpdateData.rectangles[i].Flags);
                    RdpbcgrEncoder.EncodeStructure(buffer, bitmapData.bitmapUpdateData.rectangles[i].bitmapLength);
                    if ((bitmapData.bitmapUpdateData.rectangles[i].Flags & TS_BITMAP_DATA_Flags_Values.NO_BITMAP_COMPRESSION_HDR)
                        == TS_BITMAP_DATA_Flags_Values.None)
                    {
                        RdpbcgrEncoder.EncodeStructure(
                            buffer, (ushort)bitmapData.bitmapUpdateData.rectangles[i].bitmapComprHdr.cbCompFirstRowSize);
                        RdpbcgrEncoder.EncodeStructure(
                            buffer, bitmapData.bitmapUpdateData.rectangles[i].bitmapComprHdr.cbCompMainBodySize);
                        RdpbcgrEncoder.EncodeStructure(
                            buffer, bitmapData.bitmapUpdateData.rectangles[i].bitmapComprHdr.cbScanWidth);
                        RdpbcgrEncoder.EncodeStructure(
                            buffer, bitmapData.bitmapUpdateData.rectangles[i].bitmapComprHdr.cbUncompressedSize);
                    }
                    if (bitmapData.bitmapUpdateData.rectangles[i].bitmapDataStream != null)
                    {
                        if ((bitmapData.bitmapUpdateData.rectangles[i].Flags & TS_BITMAP_DATA_Flags_Values.BITMAP_COMPRESSION)
                            == TS_BITMAP_DATA_Flags_Values.BITMAP_COMPRESSION)
                        {
                            byte[] temp = serverSessionContext.Compress(bitmapData.compressionFlags, bitmapData.bitmapUpdateData.rectangles[i].bitmapDataStream);
                            bitmapData.bitmapUpdateData.rectangles[i].bitmapDataStream = temp;
                            RdpbcgrEncoder.EncodeBytes(buffer, bitmapData.bitmapUpdateData.rectangles[i].bitmapDataStream);
                        }
                        else
                        {
                            RdpbcgrEncoder.EncodeBytes(buffer, bitmapData.bitmapUpdateData.rectangles[i].bitmapDataStream);
                        }
                    }
                }
            }

            if (bitmapData.compressionFlags != compressedType_Values.None)
            {
                RdpbcgrEncoder.EncodeBytes(
                    totalBuffer, serverSessionContext.Compress(bitmapData.compressionFlags, buffer.ToArray()));
            }
            else
            {
                RdpbcgrEncoder.EncodeBytes(totalBuffer, buffer.ToArray());
            }

            return totalBuffer.ToArray();
        }

        private byte[] EncodePtrPos(TS_FP_POINTERPOSATTRIBUTE ptrData)
        {
            List<byte> totalBuffer = new List<byte>();
            List<byte> buffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.pointerPositionUpdateData.position.xPos);
            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.pointerPositionUpdateData.position.yPos);

            if (ptrData.compressionFlags != compressedType_Values.None)
            {
                RdpbcgrEncoder.EncodeBytes(
                    totalBuffer, serverSessionContext.Compress(ptrData.compressionFlags, buffer.ToArray()));
            }
            else
            {
                RdpbcgrEncoder.EncodeBytes(totalBuffer, buffer.ToArray());
            }
            return totalBuffer.ToArray();
        }

        private byte[] EncodeColorPtr(TS_FP_COLORPOINTERATTRIBUTE ptrData)
        {
            List<byte> totalBuffer = new List<byte>();
            List<byte> buffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.colorPointerUpdateData.cacheIndex);
            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.colorPointerUpdateData.hotSpot.xPos);
            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.colorPointerUpdateData.hotSpot.yPos);
            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.colorPointerUpdateData.width);
            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.colorPointerUpdateData.height);
            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.colorPointerUpdateData.lengthAndMask);
            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.colorPointerUpdateData.lengthXorMask);
            if (ptrData.colorPointerUpdateData.andMaskData != null)
            {
                RdpbcgrEncoder.EncodeBytes(buffer, ptrData.colorPointerUpdateData.andMaskData);
            }
            if (ptrData.colorPointerUpdateData.xorMaskData != null)
            {
                RdpbcgrEncoder.EncodeBytes(buffer, ptrData.colorPointerUpdateData.xorMaskData);
            }
            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.colorPointerUpdateData.pad);

            if (ptrData.compressionFlags != compressedType_Values.None)
            {
                RdpbcgrEncoder.EncodeBytes(
                    totalBuffer, serverSessionContext.Compress(ptrData.compressionFlags, buffer.ToArray()));
            }
            else
            {
                RdpbcgrEncoder.EncodeBytes(totalBuffer, buffer.ToArray());
            }
            return totalBuffer.ToArray();
        }

        private byte[] EncodeNewPtr(TS_FP_POINTERATTRIBUTE ptrData)
        {
            List<byte> totalBuffer = new List<byte>();
            List<byte> buffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.newPointerUpdateData.xorBpp);
            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.newPointerUpdateData.colorPtrAttr.cacheIndex);
            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.newPointerUpdateData.colorPtrAttr.hotSpot.xPos);
            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.newPointerUpdateData.colorPtrAttr.hotSpot.yPos);
            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.newPointerUpdateData.colorPtrAttr.width);
            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.newPointerUpdateData.colorPtrAttr.height);
            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.newPointerUpdateData.colorPtrAttr.lengthAndMask);
            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.newPointerUpdateData.colorPtrAttr.lengthXorMask);
            if (ptrData.newPointerUpdateData.colorPtrAttr.xorMaskData != null)
            {
                RdpbcgrEncoder.EncodeBytes(buffer, ptrData.newPointerUpdateData.colorPtrAttr.xorMaskData);
            }
            if (ptrData.newPointerUpdateData.colorPtrAttr.andMaskData != null)
            {
                RdpbcgrEncoder.EncodeBytes(buffer, ptrData.newPointerUpdateData.colorPtrAttr.andMaskData);
            }
            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.newPointerUpdateData.colorPtrAttr.pad);

            if (ptrData.compressionFlags != compressedType_Values.None)
            {
                RdpbcgrEncoder.EncodeBytes(
                    totalBuffer, serverSessionContext.Compress(ptrData.compressionFlags, buffer.ToArray()));
            }
            else
            {
                RdpbcgrEncoder.EncodeBytes(totalBuffer, buffer.ToArray());
            }
            return totalBuffer.ToArray();
        }

        private byte[] EncodeCachedPtr(TS_FP_CACHEDPOINTERATTRIBUTE ptrData)
        {
            List<byte> totalBuffer = new List<byte>();
            List<byte> buffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(buffer, ptrData.cachedPointerUpdateData.cacheIndex);

            if (ptrData.compressionFlags != compressedType_Values.None)
            {
                RdpbcgrEncoder.EncodeBytes(
                    totalBuffer, serverSessionContext.Compress(ptrData.compressionFlags, buffer.ToArray()));
            }
            else
            {
                RdpbcgrEncoder.EncodeBytes(totalBuffer, buffer.ToArray());
            }
            return totalBuffer.ToArray();
        }

        private byte[] EncodeSurfCmds(TS_FP_SURFCMDS surfCmdsData)
        {
            List<byte> totalBuffer = new List<byte>();
            List<byte> buffer = new List<byte>();

            if (surfCmdsData.surfaceCommands != null)
            {
                for (int i = 0; i < surfCmdsData.surfaceCommands.Length; ++i)
                {
                    if (surfCmdsData.surfaceCommands[i].GetType() == typeof(TS_SURFCMD_SET_SURF_BITS))
                    {
                        RdpbcgrEncoder.EncodeBytes(
                            buffer, EncodeSurfBits((TS_SURFCMD_SET_SURF_BITS)surfCmdsData.surfaceCommands[i]));
                    }
                    else if (surfCmdsData.surfaceCommands[i].GetType() == typeof(TS_SURFCMD_STREAM_SURF_BITS))
                    {
                        RdpbcgrEncoder.EncodeBytes(
                            buffer, EncodeSurfStream((TS_SURFCMD_STREAM_SURF_BITS)surfCmdsData.surfaceCommands[i]));
                    }
                    else
                    {
                        RdpbcgrEncoder.EncodeBytes(
                            buffer, EncodeSurfFrameMaker((TS_FRAME_MARKER)surfCmdsData.surfaceCommands[i]));
                    }

                }
            }

            if (surfCmdsData.compressionFlags != compressedType_Values.None)
            {
                RdpbcgrEncoder.EncodeBytes(
                    totalBuffer, serverSessionContext.Compress(surfCmdsData.compressionFlags, buffer.ToArray()));
            }
            else
            {
                RdpbcgrEncoder.EncodeBytes(totalBuffer, buffer.ToArray());
            }

            return totalBuffer.ToArray();
        }

        private byte[] EncodeSurfBits(TS_SURFCMD_SET_SURF_BITS surfBitsData)
        {
            List<byte> buffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(buffer, (ushort)surfBitsData.cmdType);
            RdpbcgrEncoder.EncodeStructure(buffer, surfBitsData.destLeft);
            RdpbcgrEncoder.EncodeStructure(buffer, surfBitsData.destTop);
            RdpbcgrEncoder.EncodeStructure(buffer, surfBitsData.destRight);
            RdpbcgrEncoder.EncodeStructure(buffer, surfBitsData.destBottom);
            RdpbcgrEncoder.EncodeStructure(buffer, surfBitsData.bitmapData.bpp);
            RdpbcgrEncoder.EncodeStructure(buffer, (byte)surfBitsData.bitmapData.flags);
            RdpbcgrEncoder.EncodeStructure(buffer, surfBitsData.bitmapData.reserved);
            RdpbcgrEncoder.EncodeStructure(buffer, surfBitsData.bitmapData.codecID);
            RdpbcgrEncoder.EncodeStructure(buffer, surfBitsData.bitmapData.width);
            RdpbcgrEncoder.EncodeStructure(buffer, surfBitsData.bitmapData.height);
            RdpbcgrEncoder.EncodeStructure(buffer, surfBitsData.bitmapData.bitmapDataLength);
            if (surfBitsData.bitmapData.exBitmapDataHeader != null)
            {
                RdpbcgrEncoder.EncodeStructure(buffer, surfBitsData.bitmapData.exBitmapDataHeader.highUniqueId);
                RdpbcgrEncoder.EncodeStructure(buffer, surfBitsData.bitmapData.exBitmapDataHeader.lowUniqueId);
                RdpbcgrEncoder.EncodeStructure(buffer, surfBitsData.bitmapData.exBitmapDataHeader.tmMilliseconds);
                RdpbcgrEncoder.EncodeStructure(buffer, surfBitsData.bitmapData.exBitmapDataHeader.tmSeconds);
            }
            if (surfBitsData.bitmapData.bitmapData != null)
            {
                RdpbcgrEncoder.EncodeBytes(buffer, surfBitsData.bitmapData.bitmapData);
            }

            return buffer.ToArray();
        }

        private byte[] EncodeSurfStream(TS_SURFCMD_STREAM_SURF_BITS surfStreamData)
        {
            List<byte> buffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(buffer, (ushort)surfStreamData.cmdType);
            RdpbcgrEncoder.EncodeStructure(buffer, surfStreamData.destLeft);
            RdpbcgrEncoder.EncodeStructure(buffer, surfStreamData.destTop);
            RdpbcgrEncoder.EncodeStructure(buffer, surfStreamData.destRight);
            RdpbcgrEncoder.EncodeStructure(buffer, surfStreamData.destBottom);
            RdpbcgrEncoder.EncodeStructure(buffer, surfStreamData.bitmapData.bpp);
            RdpbcgrEncoder.EncodeStructure(buffer, (byte)surfStreamData.bitmapData.flags);
            RdpbcgrEncoder.EncodeStructure(buffer, surfStreamData.bitmapData.reserved);
            RdpbcgrEncoder.EncodeStructure(buffer, surfStreamData.bitmapData.codecID);
            RdpbcgrEncoder.EncodeStructure(buffer, surfStreamData.bitmapData.width);
            RdpbcgrEncoder.EncodeStructure(buffer, surfStreamData.bitmapData.height);
            RdpbcgrEncoder.EncodeStructure(buffer, surfStreamData.bitmapData.bitmapDataLength);
            if (surfStreamData.bitmapData.exBitmapDataHeader != null)
            {
                RdpbcgrEncoder.EncodeStructure(buffer, surfStreamData.bitmapData.exBitmapDataHeader.highUniqueId);
                RdpbcgrEncoder.EncodeStructure(buffer, surfStreamData.bitmapData.exBitmapDataHeader.lowUniqueId);
                RdpbcgrEncoder.EncodeStructure(buffer, surfStreamData.bitmapData.exBitmapDataHeader.tmMilliseconds);
                RdpbcgrEncoder.EncodeStructure(buffer, surfStreamData.bitmapData.exBitmapDataHeader.tmSeconds);
            }
            if (surfStreamData.bitmapData.bitmapData != null)
            {
                RdpbcgrEncoder.EncodeBytes(buffer, surfStreamData.bitmapData.bitmapData);
            }

            return buffer.ToArray();
        }

        private byte[] EncodeSurfFrameMaker(TS_FRAME_MARKER frameMakerData)
        {
            List<byte> buffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(buffer, (ushort)frameMakerData.cmdType);
            RdpbcgrEncoder.EncodeStructure(buffer, (ushort)frameMakerData.frameAction);
            RdpbcgrEncoder.EncodeStructure(buffer, frameMakerData.frameId);
            return buffer.ToArray();
        }
    }

    /// <summary>
    /// The type of actionCode.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum nested_TS_FP_UPDATE_PDU_fpOutputHeader_actionCode_Values : byte
    {

        /// <summary>
        ///  Indicates that the PDU is a fast-path output PDU.
        /// </summary>
        FASTPATH_OUTPUT_ACTION_FASTPATH = 0x0,

        /// <summary>
        ///  Indicates the presence of a TPKT Header (see [T123]
        ///  section 8) initial version byte, which implies that
        ///  the PDU is a slow-path output PDU (in this case the
        ///  full value of the initial byte MUST be 0x03).
        /// </summary>
        FASTPATH_OUTPUT_ACTION_X224 = 0x3,
    }

    /// <summary>
    /// The type of reserved.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum reserved_Values : byte
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// The type of encryptionFlagsChgd.
    /// </summary>
    [Flags()]
    public enum encryptionFlagsChgd_Values : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates that the MAC signature for the PDU was generated
        ///  using the "salted MAC generation" technique (see section
        ///  ). If this bit is not set, then the standard technique
        ///  was used (see sections  and ).
        /// </summary>
        FASTPATH_OUTPUT_SECURE_CHECKSUM = 0x1,

        /// <summary>
        ///  Indicates that the PDU contains an 8-byte message authentication
        ///  code (MAC) signature after the optional length2 field
        ///  (that is, the dataSignature field is present) and the
        ///  contents of the PDU are encrypted using the negotiated
        ///  encryption package (see sections  and ).
        /// </summary>
        FASTPATH_OUTPUT_ENCRYPTED = 0x2,
    }

    /// <summary>
    ///  The TS_FP_UPDATE structure is used to describe and encapsulate
    ///  the data for a fast-path update sent from server to
    ///  client. All fast-path updates conform to this basic
    ///  structure (see sections  to ).
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_2_1.xml
    //  </remarks>
    public partial class TS_FP_UPDATE
    {

        /// <summary>
        ///  An 8-bit unsigned integer.
        ///  Three pieces of information are collapsed into this byte:
        ///  Fast-path update byte
        ///  Fast-path fragment sequencing
        ///  Compression usage indication
        /// </summary>
        public byte updateHeader;

        /// <summary>
        ///  An 8-bit unsigned integer. Optional compression flags.
        ///  The flags used in this field are exactly the same as
        ///  the MPPC flags used in the compressedType field in
        ///  the Share Data Header and have the same meaning.
        /// </summary>
        public compressedType_Values compressionFlags;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  data in the updateData field.
        /// </summary>
        public ushort size;
    }

    /// <summary>
    /// The type of updateCode.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum updateCode_Values : byte
    {

        /// <summary>
        ///  Indicates a Fast-Path Orders Update (see  [MS-RDPEGDI]
        ///  section 2.2.2.3).
        /// </summary>
        FASTPATH_UPDATETYPE_ORDERS = 0x0,

        /// <summary>
        ///  Indicates a Fast-Path Bitmap Update (see section ).
        /// </summary>
        FASTPATH_UPDATETYPE_BITMAP = 0x1,

        /// <summary>
        ///  Indicates a Fast-Path Palette Update (see section ).
        /// </summary>
        FASTPATH_UPDATETYPE_PALETTE = 0x2,

        /// <summary>
        ///  Indicates a Fast-Path Synchronize Update (see section
        ///  ).
        /// </summary>
        FASTPATH_UPDATETYPE_SYNCHRONIZE = 0x3,

        /// <summary>
        ///  Indicates a Fast-Path Surface Commands Update (see section 2.2.9.1.2.1.10).
        /// </summary>
        FASTPATH_UPDATETYPE_SURFCMDS = 0x4,

        /// <summary>
        ///  Indicates a Fast-Path System Pointer Hidden Update (see
        ///  section ).
        /// </summary>
        FASTPATH_UPDATETYPE_PTR_NULL = 0x5,

        /// <summary>
        ///  Indicates a Fast-Path System Pointer Default Update
        ///  (see section ).
        /// </summary>
        FASTPATH_UPDATETYPE_PTR_DEFAULT = 0x6,

        /// <summary>
        ///  Indicates a Fast-Path Pointer Position Update (see section
        ///  ).
        /// </summary>
        FASTPATH_UPDATETYPE_PTR_POSITION = 0x8,

        /// <summary>
        ///  Indicates a Fast-Path Color Pointer Update (see section
        ///  ).
        /// </summary>
        FASTPATH_UPDATETYPE_COLOR = 0x9,

        /// <summary>
        ///  Indicates a Fast-Path Cached Pointer Update (see section
        ///  ).
        /// </summary>
        FASTPATH_UPDATETYPE_CACHED = 0xA,

        /// <summary>
        ///  Indicates a Fast-Path New Pointer Update (see section
        ///  ).
        /// </summary>
        FASTPATH_UPDATETYPE_POINTER = 0xB,
    }

    /// <summary>
    /// The type of reserved.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum nested_TS_FP_UPDATE_updateHeader_reserved_Values : byte
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0,
    }

    /// <summary>
    /// The type of fragmentation.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum fragmentation_Value : ushort
    {
        /// <summary>
        /// The fast-path data in the updateData field is not part of a sequence of fragments.
        /// </summary>
        FASTPATH_FRAGMENT_SINGLE = 0x00,

        /// <summary>
        /// The fast-path data in the updateData field contains the last fragment in a sequence of fragments.
        /// </summary>
        FASTPATH_FRAGMENT_LAST = 0x01,

        /// <summary>
        /// The fast-path data in the updateData field contains the first fragment in a sequence of fragments.
        /// </summary>
        FASTPATH_FRAGMENT_FIRST = 0x02,

        /// <summary>
        /// The fast-path data in the updateData field contains the second or subsequent fragment 
        /// in a sequence of fragments.
        /// </summary>
        FASTPATH_FRAGMENT_NEXT = 0x03,
    }

    /// <summary>
    /// The type of compression.
    /// </summary>
    public enum compression_Values : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Indicates that the compressionFlags field is present.
        /// </summary>
        FASTPATH_OUTPUT_COMPRESSION_USED = 0x2,
    }

    /// <summary>
    /// The TS_UPDATE_SURFCMDS structure encapsulates one or more Surface Command
    /// (section 2.2.9.1.1.3.1.4.1) structures.
    /// </summary>
    public partial class TS_UPDATE_SURFCMDS : RdpbcgrSlowPathUpdatePdu
    {
        /// <summary>
        ///  Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share
        ///  Control Header MUST be set to PDUTYPE_DATAPDU (7).
        ///  The pduType2 field of the Share Data Header MUST be
        ///  set to PDUTYPE2_UPDATE (2).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        /// A 16-bit, unsigned integer. The update type. 
        /// This field MUST be set to UPDATETYPE_SURFCMDS (0x0004).
        /// </summary>
        public updateType_Values updateType;

        /// <summary>
        /// The TS_SURFCMD structure is used to specify the Surface Command 
        /// type and to encapsulate the data for a Surface Command sent from
        /// a server to a client.
        /// </summary>
        public TS_SURFCMD[] surfaceCommands;
    }

    /// <summary>
    /// The TS_SURFCMD structure is used to specify the Surface 
    /// Command type and to encapsulate the data for a Surface 
    /// Command sent from a server to a client. All Surface Commands 
    /// in section 2.2.9.2 conform to this structure.
    /// </summary>
    public class TS_SURFCMD
    {
        /// <summary>
        /// A 16-bit, unsigned integer. Surface Command type.
        /// </summary>
        public cmdType_Values cmdType;
    }

    /// <summary>
    /// The type of cmdType.
    /// </summary>
    public enum cmdType_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Surface Command type.
        /// </summary>
        CMDTYPE_SET_SURFACE_BITS = 0x0001,

        /// <summary>
        /// Frame Marker Command.
        /// </summary>
        CMDTYPE_FRAME_MARKER = 0x0004,

        /// <summary>
        /// Indicates a Stream Surface Bits Command.
        /// </summary>
        CMDTYPE_STREAM_SURFACE_BITS = 0x0006

    }

    /// <summary>
    /// The Set Surface Bits Command is used to transport encoded 
    /// bitmap data destined for a rectangular region of the current 
    /// target surface from an RDP server to an RDP client.
    /// </summary>
    public class TS_SURFCMD_SET_SURF_BITS : TS_SURFCMD
    {
        /// <summary>
        /// A 16-bit, unsigned integer. Left bound of the destination 
        /// rectangle that will contain the decoded bitmap data.
        /// </summary>
        public ushort destLeft;

        /// <summary>
        /// A 16-bit, unsigned integer. Top bound of the destination
        /// rectangle that will contain the decoded bitmap data.
        /// </summary>
        public ushort destTop;

        /// <summary>
        /// A 16-bit, unsigned integer. Right bound of the destination
        /// rectangle that will contain the decoded bitmap data.
        /// </summary>
        public ushort destRight;

        /// <summary>
        /// A 16-bit, unsigned integer. Bottom bound of the destination
        /// rectangle that will contain the decoded bitmap data.
        /// </summary>
        public ushort destBottom;

        /// <summary>
        /// An Extended Bitmap Data (section 2.2.9.2.2) structure that 
        /// contains an encoded bitmap image.
        /// </summary>
        public TS_BITMAP_DATA_EX bitmapData;
    }

    /// <summary>
    /// The TS_BITMAP_DATA_EX structure is used to encapsulate encoded bitmap data.
    /// </summary>
    public struct TS_BITMAP_DATA_EX
    {
        /// <summary>
        /// An 8-bit, unsigned integer. The color depth of the bitmap data in bits-per-pixel.
        /// </summary>
        public byte bpp;

        /// <summary>
        /// An 8-bit, unsigned integer.
        /// </summary>
        public TSBitmapDataExFlags_Values flags;

        /// <summary>
        /// An 8-bit, unsigned integer. This field is reserved for future use. It MUST be set to zero
        /// </summary>
        public byte reserved;

        /// <summary>
        /// An 8-bit, unsigned integer. The client-assigned ID that identifies 
        /// the bitmap codec that was used to encode the bitmap data. Bitmap 
        /// codec parameters are exchanged in the Bitmap Codecs Capability Set 
        /// (section 2.2.7.2.10). If this field is 0, then the bitmap data is 
        /// not encoded and can be used without performing any decoding transformation.
        /// </summary>
        public byte codecID;

        /// <summary>
        /// A 16-bit, unsigned integer. The width of the decoded bitmap image in pixels.
        /// </summary>
        public ushort width;

        /// <summary>
        /// A 16-bit, unsigned integer. The height of the decoded bitmap image in pixels.
        /// </summary>
        public ushort height;

        /// <summary>
        /// A 32-bit, unsigned integer. The size in bytes of the bitmapData field.
        /// </summary>
        public uint bitmapDataLength;

        /// <summary>
        /// An optional Extended Compressed Bitmap Header (section 2.2.9.2.1.1.1) structure that contains nonessential information associated with bitmap data in the bitmapData field. 
        /// This field MUST be present if the EX_COMPRESSED_BITMAP_HEADER_PRESENT (0x01) flag is present.
        /// </summary>
        public TS_COMPRESSED_BITMAP_HEADER_EX exBitmapDataHeader;

        /// <summary>
        /// A variable-size array of bytes containing bitmap data encoded using the
        /// codec identified by the ID in the codecID field.
        /// </summary>
        public byte[] bitmapData;
    }

    /// <summary>
    /// Flags of TS_BITMAP_DATA_EX
    /// </summary>
    public enum TSBitmapDataExFlags_Values : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that the optional exBitmapDataHeader field is present.
        /// </summary>
        EX_COMPRESSED_BITMAP_HEADER_PRESENT = 0x01
    }

    public class TS_COMPRESSED_BITMAP_HEADER_EX
    {
        /// <summary>
        /// A 32-bit, unsigned integer that contains the high-order bits of a unique 64-bit identifier for the bitmap data.
        /// </summary>
        public uint highUniqueId;

        /// <summary>
        /// A 32-bit, unsigned integer that contains the low-order bits of a unique 64-bit identifier for the bitmap data.
        /// </summary>
        public uint lowUniqueId;

        /// <summary>
        /// A 64-bit, unsigned integer that contains the milliseconds component of the timestamp that indicates when the bitmap data was generated. 
        /// The timestamp (composed of the tmMilliseconds and tmSeconds fields), denotes the period of time that has elapsed since January 1, 1970 (midnight UTC/GMT), not counting leap seconds.
        /// </summary>
        public ulong tmMilliseconds;

        /// <summary>
        /// A 64-bit, unsigned integer that contains the seconds component of the timestamp that indicates when the bitmap data was generated. 
        /// The timestamp (composed of the tmMilliseconds and tmSeconds fields), denotes the period of time that has elapsed since January 1, 1970 (midnight UTC/GMT), not counting leap seconds.
        /// </summary>
        public ulong tmSeconds;

    }

    /// <summary>
    /// The Stream Surface Bits Command is used to transport encoded 
    /// bitmap data destined for a rectangular region of the current 
    /// target surface from an RDP server to an RDP client.
    /// </summary>
    public class TS_SURFCMD_STREAM_SURF_BITS : TS_SURFCMD
    {
        /// <summary>
        /// A 16-bit, unsigned integer. Left bound of the destination 
        /// rectangle that will contain the decoded bitmap data.
        /// </summary>
        public ushort destLeft;

        /// <summary>
        /// A 16-bit, unsigned integer. Top bound of the destination
        /// rectangle that will contain the decoded bitmap data.
        /// </summary>
        public ushort destTop;

        /// <summary>
        /// A 16-bit, unsigned integer. Right bound of the destination
        /// rectangle that will contain the decoded bitmap data.
        /// </summary>
        public ushort destRight;

        /// <summary>
        /// A 16-bit, unsigned integer. Bottom bound of the destination
        /// rectangle that will contain the decoded bitmap data.
        /// </summary>
        public ushort destBottom;

        /// <summary>
        /// An Extended Bitmap Data (section 2.2.9.2.2) structure that 
        /// contains an encoded bitmap image.
        /// </summary>
        public TS_BITMAP_DATA_EX bitmapData;
    }

    /// <summary>
    /// The Frame Marker Command is used to group multiple surface commands 
    /// so that these commands can be processed and presented to the user 
    /// as a single entity, a frame.
    /// </summary>
    public class TS_FRAME_MARKER : TS_SURFCMD
    {
        /// <summary>
        /// A 16-bit, unsigned integer. Identifies the beginning and end of a frame.
        /// </summary>
        public frameAction_Values frameAction;

        /// <summary>
        /// A 32-bit, unsigned integer. The ID identifying the frame.
        /// </summary>
        public uint frameId;
    }

    /// <summary>
    /// A 16-bit, unsigned integer. Identifies the beginning and end of a frame.
    /// </summary>
    public enum frameAction_Values : ushort
    {
        /// <summary>
        /// Indicates the start of a new frame.
        /// </summary>
        SURFACECMD_FRAMEACTION_BEGIN = 0x0000,

        /// <summary>
        /// Indicates the end of the current frame.
        /// </summary>
        SURFACECMD_FRAMEACTION_END = 0x0001
    }

    /// <summary>
    ///  The TS_FP_UPDATE_PALETTE structure is the fast-path
    ///  variant of the TS_UPDATE_PALETTE_PDU_DATA structure.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_2_1_1.xml
    //  </remarks>
    public partial class TS_FP_UPDATE_PALETTE : TS_FP_UPDATE
    {
        /// <summary>
        ///  Variable length palette data. Both slow and fast-path
        ///  utilize the same data format, a Palette Update structure
        ///  to represent this information.
        /// </summary>
        public TS_UPDATE_PALETTE_DATA paletteUpdateData;
    }

    /// <summary>
    ///  The TS_FP_UPDATE_BITMAP structure is the fast-path variant
    ///  of the TS_UPDATE_BITMAP_PDU_DATA structure.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_2_1_2.xml
    //  </remarks>
    public partial class TS_FP_UPDATE_BITMAP : TS_FP_UPDATE
    {
        /// <summary>
        ///   Variable length bitmap data. Both slow and fast-path
        ///  utilize the same data format, a Bitmap Update structure
        ///  to represent this information.
        /// </summary>
        public TS_UPDATE_BITMAP_DATA bitmapUpdateData;
    }

    /// <summary>
    ///  The TS_FP_UPDATE_SYNCHRONIZE structure is the fast-path
    ///  variant of the TS_UPDATE_SYNCHRONIZE_PDU_DATA structure.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_2_1_3.xml
    //  </remarks>
    public partial class TS_FP_UPDATE_SYNCHRONIZE : TS_FP_UPDATE
    {
    }

    /// <summary>
    ///  The TS_FP_POINTERPOSATTRIBUTE structure is the fast-path
    ///  variant of the TS_POINTERPOSATTRIBUTE structure (see
    ///  section ).
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_2_1_4.xml
    //  </remarks>
    public partial class TS_FP_POINTERPOSATTRIBUTE : TS_FP_UPDATE
    {
        /// <summary>
        ///   TS_POINTERPOSATTRIBUTE structure (4 bytes): Pointer
        ///  coordinates. Both slow and fast-path utilize the same
        ///  data format, a Pointer Position Update structure to
        ///  represent this information.
        /// </summary>
        public TS_POINTERPOSATTRIBUTE pointerPositionUpdateData;
    }

    /// <summary>
    ///  The TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE structure is
    ///  the fast-path variant of the TS_SYSTEMPOINTERATTRIBUTE
    ///  structure which contains the SYSPTR_NULL (0x00000000)
    ///  flag.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_2_1_5.xml
    //  </remarks>
    public partial class TS_FP_SYSTEMPOINTERHIDDENATTRIBUTE : TS_FP_UPDATE
    {
    }

    /// <summary>
    ///  The TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE structure is
    ///  the fast-path variant of the TS_SYSTEMPOINTERATTRIBUTE
    ///  structure which contains the SYSPTR_DEFAULT (0x00007F00)
    ///  flag.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_2_1_6.xml
    //  </remarks>
    public partial class TS_FP_SYSTEMPOINTERDEFAULTATTRIBUTE : TS_FP_UPDATE
    {
    }

    /// <summary>
    ///  The TS_FP_COLORPOINTERATTRIBUTE structure is the fast-path
    ///  variant of the TS_COLORPOINTERATTRIBUTE structure.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_2_1_7.xml
    //  </remarks>
    public partial class TS_FP_COLORPOINTERATTRIBUTE : TS_FP_UPDATE
    {
        /// <summary>
        ///  Color pointer data. Both slow and fast-path utilize
        ///  the same data format, a Color Pointer Update structure
        ///  to represent this information.
        /// </summary>
        public TS_COLORPOINTERATTRIBUTE colorPointerUpdateData;
    }

    /// <summary>
    ///  The TS_FP_POINTERATTRIBUTE structure is the fast-path
    ///  variant of the TS_POINTERATTRIBUTE structure.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_2_1_8.xml
    //  </remarks>
    public partial class TS_FP_POINTERATTRIBUTE : TS_FP_UPDATE
    {
        /// <summary>
        ///  Color pointer data at arbitrary color depth. Both slow
        ///  and fast-path utilize the same data format, a New Pointer
        ///  Update structure to represent this information.
        /// </summary>
        public TS_POINTERATTRIBUTE newPointerUpdateData;
    }

    /// <summary>
    ///   				The TS_FP_CACHEDPOINTERATTRIBUTE structure is the
    ///  fast-path variant of the TS_CACHEDPOINTERATTRIBUTE
    ///  structure.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_1_2_1_9.xml
    //  </remarks>
    public partial class TS_FP_CACHEDPOINTERATTRIBUTE : TS_FP_UPDATE
    {
        /// <summary>
        ///    Cached pointer data. Both slow and fast-path utilize
        ///  the same data format (a Cached Pointer Update structure)
        ///  to represent this information.
        /// </summary>
        public TS_CACHEDPOINTERATTRIBUTE cachedPointerUpdateData;
    }

    /// <summary>
    ///  The TS_FP_SURFCMDS structure is the fast-path variant of
    ///  the TS_UPDATE_SURFCMDS (section 2.2.9.1.1.3.1.4) structure.
    /// </summary>
    public class TS_FP_SURFCMDS : TS_FP_UPDATE
    {
        /// <summary>
        /// The TS_SURFCMD structure is used to specify the Surface Command 
        /// type and to encapsulate the data for a Surface Command sent from
        /// a server to a client.
        /// </summary>
        public TS_SURFCMD[] surfaceCommands;
    }

    /// <summary>
    ///  The Play Sound PDU instructs the client to play a "beep"
    ///  sound.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_2_1.xml
    //  </remarks>
    public partial class Server_Play_Sound_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Play Sound
        ///  PDU, as specified in section .           
        /// </summary>
        public TS_PLAY_SOUND_PDU_DATA playSoundPduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public Server_Play_Sound_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_Play_Sound_Pdu()
        {
        }

        public override StackPacket Clone()
        {
            Server_Play_Sound_Pdu clonePlaySoundPdu = new Server_Play_Sound_Pdu(serverSessionContext);
            clonePlaySoundPdu.commonHeader = commonHeader.Clone();
            clonePlaySoundPdu.playSoundPduData.shareDataHeader = playSoundPduData.shareDataHeader;
            clonePlaySoundPdu.playSoundPduData.duration = playSoundPduData.duration;
            clonePlaySoundPdu.playSoundPduData.frequency = playSoundPduData.frequency;

            return clonePlaySoundPdu;
        }

        public override byte[] ToBytes()
        {
            byte[] dataBuffer = EncodePlaySoundData(playSoundPduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, dataBuffer);

            return encodedBytes;
        }

        private byte[] EncodePlaySoundData(TS_PLAY_SOUND_PDU_DATA playSoundPduData)
        {
            List<byte> dataBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(dataBuffer, playSoundPduData.shareDataHeader);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, playSoundPduData.duration);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, playSoundPduData.frequency);

            return dataBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_PLAY_SOUND_PDU_DATA structure contains the contents
    ///  of the Play Sound PDU, which is a Share Data Header
    ///  and two fields.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_2_1_1.xml
    //  </remarks>
    public partial struct TS_PLAY_SOUND_PDU_DATA
    {

        /// <summary>
        ///  Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share
        ///  Control Header MUST be set to PDUTYPE_DATAPDU (7).
        ///  The pduType2 field of the Share Data Header MUST be
        ///  set to PDUTYPE2_PLAY_SOUND (34).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  A 32-bit unsigned integer. Duration of the beep the
        ///  client should play.
        /// </summary>
        public uint duration;

        /// <summary>
        ///  A 32-bit unsigned integer. Frequency of the beep the
        ///  client should play.
        /// </summary>
        public uint frequency;
    }

    /// <summary>
    ///  The Save Session Info PDU is used by the server to transmit
    ///  session and user logon information back to the client
    ///  after the user has logged on.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_3_1.xml
    //  </remarks>
    public partial class Server_Save_Session_Info_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Save Session Info PDU, as
        ///  specified in section .           
        /// </summary>
        public TS_SAVE_SESSION_INFO_PDU_DATA saveSessionInfoPduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public Server_Save_Session_Info_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_Save_Session_Info_Pdu()
        {
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Server_Save_Session_Info_Pdu cloneServerSaveSessionInfo = new Server_Save_Session_Info_Pdu();

            cloneServerSaveSessionInfo.commonHeader = commonHeader.Clone();
            cloneServerSaveSessionInfo.saveSessionInfoPduData = saveSessionInfoPduData;

            if (cloneServerSaveSessionInfo.saveSessionInfoPduData.infoData.GetType()
                == typeof(TS_LOGON_INFO_VERSION_2))
            {
                TS_LOGON_INFO_VERSION_2 version2 =
                    (TS_LOGON_INFO_VERSION_2)cloneServerSaveSessionInfo.saveSessionInfoPduData.infoData;
                version2.Pad = RdpbcgrUtility.CloneByteArray(version2.Pad);
                cloneServerSaveSessionInfo.saveSessionInfoPduData.infoData = version2;
            }
            else if (cloneServerSaveSessionInfo.saveSessionInfoPduData.infoData.GetType()
                == typeof(TS_PLAIN_NOTIFY))
            {
                TS_PLAIN_NOTIFY notify = (TS_PLAIN_NOTIFY)cloneServerSaveSessionInfo.saveSessionInfoPduData.infoData;
                notify.Pad = RdpbcgrUtility.CloneByteArray(notify.Pad);
                cloneServerSaveSessionInfo.saveSessionInfoPduData.infoData = notify;
            }
            else if (cloneServerSaveSessionInfo.saveSessionInfoPduData.infoData.GetType()
                == typeof(TS_LOGON_INFO_EXTENDED))
            {
                TS_LOGON_INFO_EXTENDED extended =
                    (TS_LOGON_INFO_EXTENDED)cloneServerSaveSessionInfo.saveSessionInfoPduData.infoData;
                extended.Pad = RdpbcgrUtility.CloneByteArray(extended.Pad);

                TS_LOGON_INFO_EXTENDED srcInfo = (TS_LOGON_INFO_EXTENDED)saveSessionInfoPduData.infoData;
                if (srcInfo.LogonFields != null)
                {
                    TS_LOGON_INFO_FIELD[] logonFields = new TS_LOGON_INFO_FIELD[srcInfo.LogonFields.Length];
                    for (int i = 0; i < srcInfo.LogonFields.Length; ++i)
                    {
                        logonFields[i] = srcInfo.LogonFields[i];
                        if (srcInfo.LogonFields[i].FieldData != null)
                        {
                            if (srcInfo.LogonFields[i].FieldData.GetType() == typeof(TS_LOGON_ERRORS_INFO))
                            {
                                logonFields[i].FieldData = new TS_LOGON_ERRORS_INFO();
                                ((TS_LOGON_ERRORS_INFO)logonFields[i].FieldData).ErrorNotificationData =
                                    ((TS_LOGON_ERRORS_INFO)srcInfo.LogonFields[i].FieldData).ErrorNotificationData;
                                ((TS_LOGON_ERRORS_INFO)logonFields[i].FieldData).ErrorNotificationType =
                                    ((TS_LOGON_ERRORS_INFO)srcInfo.LogonFields[i].FieldData).ErrorNotificationType;
                            }
                            else if (srcInfo.LogonFields[i].FieldData.GetType() == typeof(ARC_SC_PRIVATE_PACKET))
                            {
                                logonFields[i].FieldData = new ARC_SC_PRIVATE_PACKET();
                                ((ARC_SC_PRIVATE_PACKET)logonFields[i].FieldData).cbLen =
                                    ((ARC_SC_PRIVATE_PACKET)srcInfo.LogonFields[i].FieldData).cbLen;
                                ((ARC_SC_PRIVATE_PACKET)logonFields[i].FieldData).LogonId =
                                    ((ARC_SC_PRIVATE_PACKET)srcInfo.LogonFields[i].FieldData).LogonId;
                                ((ARC_SC_PRIVATE_PACKET)logonFields[i].FieldData).Version =
                                    ((ARC_SC_PRIVATE_PACKET)srcInfo.LogonFields[i].FieldData).Version;
                                ((ARC_SC_PRIVATE_PACKET)logonFields[i].FieldData).ArcRandomBits =
                                    RdpbcgrUtility.CloneByteArray(
                                    ((ARC_SC_PRIVATE_PACKET)srcInfo.LogonFields[i].FieldData).ArcRandomBits);
                            }
                        }
                    }

                    extended.LogonFields = logonFields;
                    cloneServerSaveSessionInfo.saveSessionInfoPduData.infoData = extended;
                }
            }

            return cloneServerSaveSessionInfo;
        }

        public override byte[] ToBytes()
        {
            byte[] dataBuffer = EncodeSaveSessionInfoData(saveSessionInfoPduData);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, dataBuffer);

            return encodedBytes;
        }

        private byte[] EncodeSaveSessionInfoData(TS_SAVE_SESSION_INFO_PDU_DATA saveSessionInfoPduData)
        {
            List<byte> dataBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(dataBuffer, saveSessionInfoPduData.shareDataHeader);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, (uint)saveSessionInfoPduData.infoType);
            switch (saveSessionInfoPduData.infoType)
            {
                case infoType_Values.INFOTYPE_LOGON:
                    RdpbcgrEncoder.EncodeBytes(dataBuffer, EncodeLogonInfo((TS_LOGON_INFO)saveSessionInfoPduData.infoData));
                    break;
                case infoType_Values.INFOTYPE_LOGON_LONG:
                    RdpbcgrEncoder.EncodeBytes(dataBuffer, EncodeLogonLongInfo((TS_LOGON_INFO_VERSION_2)saveSessionInfoPduData.infoData));
                    break;
                case infoType_Values.INFOTYPE_LOGON_EXTENDED_INF:
                    RdpbcgrEncoder.EncodeBytes(dataBuffer, EncodeLogonExtended((TS_LOGON_INFO_EXTENDED)saveSessionInfoPduData.infoData));
                    break;
                case infoType_Values.INFOTYPE_LOGON_PLAINNOTIFY:
                    RdpbcgrEncoder.EncodeBytes(dataBuffer, EncodeLogonPlainNotify((TS_PLAIN_NOTIFY)saveSessionInfoPduData.infoData));
                    break;
            }

            return dataBuffer.ToArray();
        }

        private byte[] EncodeLogonInfo(TS_LOGON_INFO logonInfo)
        {
            List<byte> dataBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(dataBuffer, logonInfo.cbDomain);
            RdpbcgrEncoder.EncodeUnicodeString(dataBuffer, logonInfo.Domain, ConstValue.TS_LOGON_INFO_DOMAIN_LENGTH);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, logonInfo.cbUserName);
            RdpbcgrEncoder.EncodeUnicodeString(dataBuffer, logonInfo.UserName, ConstValue.TS_LOGON_INFO_USER_NAME_LENGTH);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, logonInfo.SessionId);

            return dataBuffer.ToArray();
        }

        private byte[] EncodeLogonLongInfo(TS_LOGON_INFO_VERSION_2 logonInfo)
        {
            List<byte> dataBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(dataBuffer, (ushort)logonInfo.Version);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, logonInfo.Size);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, logonInfo.SessionId);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, logonInfo.cbDomain);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, logonInfo.cbUserName);
            RdpbcgrEncoder.EncodeBytes(dataBuffer, logonInfo.Pad);
            RdpbcgrEncoder.EncodeUnicodeString(dataBuffer, logonInfo.Domain, logonInfo.cbDomain);

            RdpbcgrEncoder.EncodeUnicodeString(dataBuffer, logonInfo.UserName, logonInfo.cbUserName);

            return dataBuffer.ToArray();
        }

        private byte[] EncodeLogonPlainNotify(TS_PLAIN_NOTIFY logonInfo)
        {
            List<byte> dataBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeBytes(dataBuffer, logonInfo.Pad);

            return dataBuffer.ToArray();
        }

        private byte[] EncodeLogonExtended(TS_LOGON_INFO_EXTENDED logonInfo)
        {
            List<byte> dataBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(dataBuffer, logonInfo.Length);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, (uint)logonInfo.FieldsPresent);
            for (int i = 0; i < logonInfo.LogonFields.Length; ++i)
            {
                if (logonInfo.LogonFields[i].FieldData.GetType() == typeof(TS_LOGON_ERRORS_INFO))
                {
                    RdpbcgrEncoder.EncodeStructure(dataBuffer, logonInfo.LogonFields[i].cbFieldData);
                    RdpbcgrEncoder.EncodeStructure(dataBuffer,
                        (uint)((TS_LOGON_ERRORS_INFO)logonInfo.LogonFields[i].FieldData).ErrorNotificationType);
                    RdpbcgrEncoder.EncodeStructure(dataBuffer,
                        ((TS_LOGON_ERRORS_INFO)logonInfo.LogonFields[i].FieldData).ErrorNotificationData);
                }
                else if (logonInfo.LogonFields[i].FieldData.GetType() == typeof(ARC_SC_PRIVATE_PACKET))
                {
                    RdpbcgrEncoder.EncodeStructure(dataBuffer, logonInfo.LogonFields[i].cbFieldData);
                    RdpbcgrEncoder.EncodeStructure(dataBuffer,
                        (uint)((ARC_SC_PRIVATE_PACKET)logonInfo.LogonFields[i].FieldData).cbLen);
                    RdpbcgrEncoder.EncodeStructure(dataBuffer,
                        (uint)((ARC_SC_PRIVATE_PACKET)logonInfo.LogonFields[i].FieldData).Version);
                    RdpbcgrEncoder.EncodeStructure(dataBuffer,
                        ((ARC_SC_PRIVATE_PACKET)logonInfo.LogonFields[i].FieldData).LogonId);
                    RdpbcgrEncoder.EncodeBytes(dataBuffer,
                        ((ARC_SC_PRIVATE_PACKET)logonInfo.LogonFields[i].FieldData).ArcRandomBits);
                }
            }
            RdpbcgrEncoder.EncodeBytes(dataBuffer, logonInfo.Pad);

            return dataBuffer.ToArray();
        }
    }

    /// <summary>
    /// Specifies the authorization result
    /// </summary>
    public enum Authorization_Result_value : uint
    {
        /// <summary>
        /// The user has permission to access the server.
        /// </summary>
        AUTHZ_SUCCESS = 0x00000000,

        /// <summary>
        /// The user does not have permission to access the server.
        /// </summary>
        AUTHZ_ACCESS_DENIED = 0x00000005,
    }

    /// <summary>
    /// The Early User Authorization Result PDU is sent from server to client and is used to convey authorization information to the client. 
    /// This PDU is only sent by the server if the client advertised support for it by specifying the PROTOCOL_HYBRID_EX (0x00000008) flag 
    /// in the requestedProtocols field of the RDP Negotiation Request structure and it MUST be sent immediately after the CredSSP handshake has completed.
    /// </summary>
    public partial class Early_User_Authorization_Result_PDU : RdpbcgrServerPdu
    {
        public Authorization_Result_value authorizationResult;

        public Early_User_Authorization_Result_PDU()
        {
        }

        public Early_User_Authorization_Result_PDU(Authorization_Result_value result)
        {
            authorizationResult = result;
        }

        public override StackPacket Clone()
        {
            Early_User_Authorization_Result_PDU resultPdu = new Early_User_Authorization_Result_PDU();
            resultPdu.authorizationResult = this.authorizationResult;
            return resultPdu;
        }

        public override byte[] ToBytes()
        {
            List<byte> dataBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(dataBuffer, (uint)this.authorizationResult);

            byte[] encodedBytes = dataBuffer.ToArray();

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes);

            return encodedBytes;
        }
    }
    /// <summary>
    ///  The TS_SAVE_SESSION_INFO_PDU_DATA structure is
    ///  a wrapper around different classes of user logon information.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_3_1_1.xml
    //  </remarks>
    public partial struct TS_SAVE_SESSION_INFO_PDU_DATA
    {

        /// <summary>
        ///  TS_SHAREDATAHEADER structure (18 bytes): Share Data
        ///  Header containing information about the packet. The
        ///  type subfield of the pduType field of the Share Control
        ///  Header MUST be set to PDUTYPE_DATAPDU (7). The pduType2
        ///  field of the Share Data Header MUST be set to PDUTYPE2_SAVE_SESSION_INFO
        ///  (38).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  A 32-bit unsigned integer. The type of logon information.
        /// </summary>
        public infoType_Values infoType;

        /// <summary>
        ///  TS_LOGON_INFO, TS_LOGON_INFO_VERSION_2 or TS_LOGON_INFO_EXTENDED:
        ///  Variable length logon information structure. The type
        ///  of data which follows depends on the value of the infoType
        ///  field.
        /// </summary>
        public object infoData;
    }

    /// <summary>
    /// The type of infoType.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum infoType_Values : uint
    {

        /// <summary>
        ///  This is a notification that the user has logged on.
        ///  The infoData field which follows contains a Logon Info
        ///  Version 1 structure.
        /// </summary>
        INFOTYPE_LOGON = 0x00000000,

        /// <summary>
        ///  This is a notification that the user has logged on.
        ///  The infoData field which follows contains a Logon Info
        ///  Version 2 structure. This type was added in RDP 5.1
        ///  and SHOULD be used if the LONG_CREDENTIALS_SUPPORTED
        ///  (0x00000004) flag is set in the General Capability
        ///  Set.
        /// </summary>
        INFOTYPE_LOGON_LONG = 0x00000001,

        /// <summary>
        ///  This is a notification that the user has logged on.
        ///  The infoData field which follows contains a Plain Notify
        ///  structure which contains 576 bytes of padding (see
        ///  Section ). This type was added in RDP 5.1.
        /// </summary>
        INFOTYPE_LOGON_PLAINNOTIFY = 0x00000002,

        /// <summary>
        ///  The infoData field which follows contains a Logon Info
        ///  Extended structure. This type was added in RDP 5.2.
        /// </summary>
        INFOTYPE_LOGON_EXTENDED_INF = 0x00000003,
    }

    /// <summary>
    ///  TS_LOGON_INFO is a fixed-length structure which contains
    ///  logon information intended for the client.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_3_1_1_1.xml
    //  </remarks>
    public partial struct TS_LOGON_INFO
    {

        /// <summary>
        ///  A 32-bit unsigned integer. The size of the Unicode character
        ///  data (including the mandatory null terminator), in
        ///  bytes, present in the fixed-length Domain field.
        /// </summary>
        public uint cbDomain;

        /// <summary>
        ///  An array of 26 Unicode characters: Null-terminated Unicode
        ///  string containing the name of the domain to which the
        ///  user is logged on. The length of the character data
        ///  in bytes is given by the cbDomain field.
        /// </summary>
        //[StaticSize(26, StaticSizeMode.Elements)]
        public string Domain;

        /// <summary>
        ///  A 32-bit unsigned integer. Size of the Unicode character
        ///  data (including the mandatory null terminator), in
        ///  bytes, present in the fixed-length UserName field.
        /// </summary>
        public uint cbUserName;

        /// <summary>
        ///  An array of 256 Unicode characters: Null-terminated
        ///  Unicode string containing the username which was used
        ///  to log on. The length of the character data in bytes
        ///  is given by the cbUserName field.
        /// </summary>
        //[StaticSize(256, StaticSizeMode.Elements)]
        public string UserName;

        /// <summary>
        ///  A 32-bit unsigned integer. Optional session ID of the
        ///  session according to the server. Sent by RDP 5.0 and
        ///  later servers.
        /// </summary>
        public uint SessionId;
    }

    /// <summary>
    ///  TS_LOGON_INFO_VERSION_2 is a variable length structure
    ///  which contains logon information intended for the client.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_3_1_1_2.xml
    //  </remarks>
    public partial struct TS_LOGON_INFO_VERSION_2
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The logon version.
        /// </summary>
        public TS_LOGON_INFO_VERSION_2_Version_Values Version;

        /// <summary>
        ///  A 32-bit unsigned integer. The total size in bytes of
        ///  this structure, excluding the Domain and UserName variable
        ///  length fields.
        /// </summary>
        public uint Size;

        /// <summary>
        ///  A 32-bit unsigned integer. The session ID of the session
        ///  according to the server.
        /// </summary>
        public uint SessionId;

        /// <summary>
        ///  A 32-bit unsigned integer. Size, in bytes, of the Domain
        ///  field (including the mandatory null terminator).
        /// </summary>
        public uint cbDomain;

        /// <summary>
        ///  A 32-bit unsigned integer. The size, in bytes, of the
        ///  UserName field (including the mandatory null terminator).
        /// </summary>
        public uint cbUserName;

        /// <summary>
        ///  558 bytes. Padding. Values in this field are ignored.
        /// </summary>
        public byte[] Pad;

        /// <summary>
        ///  Variable length null-terminated Unicode string containing
        ///  the name of the domain to which the user is logged
        ///  on. The size of this field in bytes is given by the
        ///  cbDomain field.
        /// </summary>
        public string Domain;

        /// <summary>
        ///  Variable length null-terminated Unicode string containing
        ///  the user name which was used to log on. The size of
        ///  this field in bytes is given by the cbUserName field.
        /// </summary>
        public string UserName;
    }

    /// <summary>
    /// The type of version.
    /// </summary>
    public enum TS_LOGON_INFO_VERSION_2_Version_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  Version 1
        /// </summary>
        SAVE_SESSION_PDU_VERSION_ONE = 0x0001,
    }

    /// <summary>
    ///   </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_3_1_1_3.xml
    //  </remarks>
    public partial struct TS_LOGON_INFO_EXTENDED
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The total size in bytes of
        ///  this structure, including the variable LogonFields
        ///  field.
        /// </summary>
        public ushort Length;

        /// <summary>
        ///  A 32-bit unsigned integer. The flags indicating which
        ///  fields are present in the LogonFields field. 
        /// </summary>
        public FieldsPresent_Values FieldsPresent;

        /// <summary>
        ///  Extended logon information fields encapsulated in Logon
        ///  Info Field structures. The presence of an information
        ///  field is indicated by the flags within the FieldsPresent
        ///  field of the Logon Info Extended structure. The ordering
        ///  of the fields is implicit and is as follows:Auto-reconnect
        ///  cookie dataLogon notification dataIf a field is not
        ///  present, the next field which is present is read.
        /// </summary>
        public TS_LOGON_INFO_FIELD[] LogonFields;

        /// <summary>
        ///  570 bytes. Padding. Values in this field are ignored.
        /// </summary>
        public byte[] Pad;
    }

    /// <summary>
    /// The type of FieldsPresent.
    /// </summary>
    public enum FieldsPresent_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  An auto-reconnect cookie field is present. The LogonFields
        ///  field of the associated Logon Info structure MUST contain
        ///  a Server Auto-Reconnect structure.
        /// </summary>
        LOGON_EX_AUTORECONNECTCOOKIE = 0x00000001,

        /// <summary>
        ///  A logon error field is present. The LogonFields field
        ///  of the associated Logon Info MUST contain a Logon Errors
        ///  Info structure.
        /// </summary>
        LOGON_EX_LOGONERRORS = 0x00000002,
    }

    /// <summary>
    ///  The TS_LOGON_INFO_FIELD is used to encapsulate extended
    ///  logon information field data of variable length.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_3_1_1_3_1.xml
    //  </remarks>
    public partial struct TS_LOGON_INFO_FIELD
    {

        /// <summary>
        ///  A 32-bit unsigned integer. The size in bytes of the
        ///  variable length data in the FieldData field.
        /// </summary>
        public uint cbFieldData;

        /// <summary>
        ///  Variable length data conforming to the structure for
        ///  the type given in the FieldsPresent field of the Logon
        ///  Info Extended structure.
        /// </summary>
        public object FieldData;
    }

    /// <summary>
    ///  The TS_LOGON_ERRORS_INFO structure contains information
    ///  which describes a logon error notification.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_8_3_1_1_3_1_1.xml
    //  </remarks>
    public partial class TS_LOGON_ERRORS_INFO
    {

        /// <summary>
        ///  A 32-bit unsigned integer. The type code of the notification.
        /// </summary>
        public ErrorNotificationType_Values ErrorNotificationType;

        /// <summary>
        ///  A 32-bit unsigned integer. Error code describing the
        ///  reason for the notification. Microsoft RDP servers
        ///  populate this field with an NTSTATUS error code (see
        ///  [ERRTRANS] for information on translating NTSTATUS
        ///  error codes to usable text strings) which describes
        ///  the issue which triggered the error.
        /// </summary>
        public ErrorNotificationData_Values ErrorNotificationData;
    }

    /// <summary>
    /// The type of ErrorNotificationType.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum ErrorNotificationType_Values : uint
    {

        /// <summary>
        ///  The logon process failed. The logon credentials which
        ///  were supplied are invalid.
        /// </summary>
        LOGON_FAILED_BAD_PASSWORD = 0x00000000,

        /// <summary>
        ///  The logon process failed. The user cannot continue with
        ///  the logon process until the password is changed.
        /// </summary>
        LOGON_FAILED_UPDATE_PASSWORD = 0x00000001,

        /// <summary>
        ///  The logon process failed. The reason for the failure
        ///  can be deduced from the ErrorNotificationData field.
        /// </summary>
        LOGON_FAILED_OTHER = 0x00000002,

        /// <summary>
        ///  The user received a warning during the logon process.
        ///  The reason for the warning can be deduced from the
        ///  ErrorNotificationData field.
        /// </summary>
        LOGON_WARNING = 0x00000003,
    }

    /// <summary>
    /// ErrorNotificationData values
    /// </summary>
    public enum ErrorNotificationData_Values : uint
    {
        /// <summary>
        /// The"Disconnection Refused" dialog is being displayed by Winlogon. 
        /// </summary>
        LOGON_MSG_DISCONNECT_REFUSED = 0xFFFFFFF9,

        /// <summary>
        /// The "No Permission" dialog is being displayed by Winlogon. 
        /// </summary>
        LOGON_MSG_NO_PERMISSION = 0xFFFFFFFA,

        /// <summary>
        /// The "Session Contention" dialog is being displayed by Winlogon. 
        /// </summary>
        LOGON_MSG_BUMP_OPTIONS = 0xFFFFFFFB,

        /// <summary>
        /// The "Session Reconnection" dialog is being displayed by Winlogon. 
        /// </summary>
        LOGON_MSG_RECONNECT_OPTIONS = 0xFFFFFFFC,

        /// <summary>
        /// The session is being terminated. 
        /// </summary>
        LOGON_MSG_SESSION_TERMINATE = 0xFFFFFFFD,

        /// <summary>
        /// The logon process is continuing. 
        /// </summary>
        LOGON_MSG_SESSION_CONTINUE = 0xFFFFFFFE

    }

    /// <summary>
    ///  The TS_RECTANGLE16 structure describes a rectangle expressed
    ///  in inclusive coordinates (the right and bottom coordinates
    ///  are included in the rectangle bounds).
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_9_1.xml
    //  </remarks>
    public partial struct TS_RECTANGLE16
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The leftmost bound of the
        ///  rectangle.
        /// </summary>
        public ushort left;

        /// <summary>
        ///  A 16-bit unsigned integer. The upper bound of the rectangle.
        /// </summary>
        public ushort top;

        /// <summary>
        ///  A 16-bit unsigned integer. The rightmost bound of the
        ///  rectangle.
        /// </summary>
        public ushort right;

        /// <summary>
        ///  A 16-bit unsigned integer. The lower bound of the rectangle.
        /// </summary>
        public ushort bottom;

        /// <summary>
        ///  Constructor to fill all fields.
        /// </summary>
        public TS_RECTANGLE16(ushort left1, ushort top1, ushort right1, ushort bottom1)
        {
            left = left1;
            right = right1;
            top = top1;
            bottom = bottom1;
        }
    }

    /// <summary>
    ///  The TS_CD_HEADER structure is used to describe compressed
    ///  bitmap data.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_9_1_1_3_1_2_1.xml
    //  </remarks>
    public partial struct TS_CD_HEADER
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The field MUST be set to
        ///  0x0000.
        /// </summary>
        public cbCompFirstRowSize_Values cbCompFirstRowSize;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  compressed bitmap data (which follows this header).
        /// </summary>
        public ushort cbCompMainBodySize;

        /// <summary>
        ///  A 16-bit unsigned integer. The width of the bitmap (which
        ///  follows this header) in pixels (this value MUST be
        ///  divisible by 4).
        /// </summary>
        public ushort cbScanWidth;

        /// <summary>
        ///  A 16-bit unsigned integer. The size in bytes of the
        ///  bitmap data (which follows this header) after it has
        ///  been decompressed.
        /// </summary>
        public ushort cbUncompressedSize;
    }

    /// <summary>
    /// The type of cbCompFirstRowSize.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum cbCompFirstRowSize_Values : ushort
    {

        /// <summary>
        ///  Possible value.
        /// </summary>
        V1 = 0x0000,
    }


    /// <summary>
    ///  The Monitor Layout PDU is used by the server to notify
    ///  the client of the monitor layout in the session on the remote server.
    /// </summary>
    public partial class TS_MONITOR_LAYOUT_PDU : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  A Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share Control 
        ///  Header (section 2.2.8.1.1.1.1) MUST be set to PDUTYPE_DATAPDU 
        ///  (7). The pduType2 field of the Share Data Header MUST be set 
        ///  to PDUTYPE2_MONITOR_LAYOUT_PDU (55), and the pduSource field 
        ///  MUST be set to 0.
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  A 32-bit, unsigned integer. The number of display monitor
        ///  definitions in the monitorDefArray field (the maximum allowed is 10).
        /// </summary>
        public uint monitorCount;

        /// <summary>
        ///  A variable-length array containing a series of TS_MONITOR_DEF 
        ///  structures (section 2.2.1.3.6.1) which describe the display monitor 
        ///  layout of the client. The number of TS_MONITOR_DEF structures is 
        ///  given by the monitorCount field.
        /// </summary>
        public TS_MONITOR_DEF[] monitorDefArray;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public TS_MONITOR_LAYOUT_PDU(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public TS_MONITOR_LAYOUT_PDU()
        {
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            TS_MONITOR_LAYOUT_PDU cloneMonitorLayoutpdu = new TS_MONITOR_LAYOUT_PDU();

            cloneMonitorLayoutpdu.commonHeader = commonHeader.Clone();
            cloneMonitorLayoutpdu.shareDataHeader = shareDataHeader;
            cloneMonitorLayoutpdu.monitorCount = monitorCount;
            if (monitorDefArray != null)
            {
                cloneMonitorLayoutpdu.monitorDefArray = (TS_MONITOR_DEF[])monitorDefArray.Clone();
            }

            return cloneMonitorLayoutpdu;
        }

        public override byte[] ToBytes()
        {
            List<byte> buffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(buffer, shareDataHeader);
            RdpbcgrEncoder.EncodeStructure(buffer, monitorCount);
            if (monitorDefArray != null)
            {
                for (int i = 0; i < monitorDefArray.Length; ++i)
                {
                    RdpbcgrEncoder.EncodeStructure(buffer, monitorDefArray[i].left);
                    RdpbcgrEncoder.EncodeStructure(buffer, monitorDefArray[i].top);
                    RdpbcgrEncoder.EncodeStructure(buffer, monitorDefArray[i].right);
                    RdpbcgrEncoder.EncodeStructure(buffer, monitorDefArray[i].bottom);
                    RdpbcgrEncoder.EncodeStructure(buffer, (uint)monitorDefArray[i].flags);
                }
            }

            byte[] dataBuffer = buffer.ToArray();

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }
    }

    /// <summary>
    ///  The Refresh Rect PDU allows the client to request that
    ///  the server redraw one or more rectangles of the session
    ///  screen area. The client can use it to repaint sections
    ///  of the client window that were obscured by other windowed
    ///  applications. Server support for this PDU is indicated
    ///  in the General Capability Set.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_9_2.xml
    //  </remarks>
    public partial class Client_Refresh_Rect_Pdu : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  The actual contents of the Refresh Rect PDU, as specified
        ///  in section .
        /// </summary>
        public TS_REFRESH_RECT_PDU refreshRectPduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Client_Refresh_Rect_Pdu(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Client_Refresh_Rect_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();

            byte[] refreshPduData = EncodeRefreshData(refreshRectPduData);
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, refreshPduData, context);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }


        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Client_Refresh_Rect_Pdu cloneRefreshPdu = new Client_Refresh_Rect_Pdu(context);
            cloneRefreshPdu.commonHeader = commonHeader.Clone();
            cloneRefreshPdu.refreshRectPduData = refreshRectPduData;
            cloneRefreshPdu.refreshRectPduData.pad3Octects = RdpbcgrUtility.CloneByteArray(refreshRectPduData.pad3Octects);
            if (refreshRectPduData.areasToRefresh != null)
            {
                cloneRefreshPdu.refreshRectPduData.areasToRefresh = new Collection<TS_RECTANGLE16>();
                for (int i = 0; i < refreshRectPduData.areasToRefresh.Count; ++i)
                {
                    cloneRefreshPdu.refreshRectPduData.areasToRefresh.Add(refreshRectPduData.areasToRefresh[i]);
                }
            }
            return cloneRefreshPdu;
        }

        /// <summary>
        /// Encode refreshRectPduData field.
        /// </summary>
        /// <param name="refreshRectPduData">The data to be encoded.</param>
        /// <returns>The encoded data.</returns>
        private static byte[] EncodeRefreshData(TS_REFRESH_RECT_PDU refreshRectPduData)
        {
            List<byte> refreshRectBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(refreshRectBuffer, refreshRectPduData.shareDataHeader);
            RdpbcgrEncoder.EncodeStructure(refreshRectBuffer, refreshRectPduData.numberOfAreas);
            RdpbcgrEncoder.EncodeBytes(refreshRectBuffer, refreshRectPduData.pad3Octects);
            if (refreshRectPduData.areasToRefresh != null)
            {
                foreach (TS_RECTANGLE16 reshArea in refreshRectPduData.areasToRefresh)
                {
                    RdpbcgrEncoder.EncodeStructure(refreshRectBuffer, reshArea);
                }
            }

            return refreshRectBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_REFRESH_RECT_PDU structure contains the contents
    ///  of the Refresh Rect PDU, which is a Share Data Header
    ///  and two fields.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_9_2_1.xml
    //  </remarks>
    public partial struct TS_REFRESH_RECT_PDU
    {

        /// <summary>
        ///  A Share Data Header containing information about the
        ///  packet. The type subfield of the pduType field of the
        ///  Share Control Header MUST be set to PDUTYPE_DATAPDU
        ///  (7). The pduType2 field of the Share Data Header MUST
        ///  be set to PDUTYPE2_REFRESH_RECT (33).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  An 8-bit unsigned integer. The number of Inclusive Rectangle
        ///  structures in the areasToRefresh field.
        /// </summary>
        public byte numberOfAreas;

        /// <summary>
        ///  A 3 element array of 8-bit unsigned integer values.
        ///  Padding. Values in this field are ignored.
        /// </summary>
        public byte[] pad3Octects;

        /// <summary>
        ///  An array of TS_RECTANGLE16 structures (variable number
        ///  of bytes). Array of screen area Inclusive Rectangles
        ///  to redraw. The number of rectangles is given by the
        ///  numberOfAreas field.
        /// </summary>
        public Collection<TS_RECTANGLE16> areasToRefresh;
    }

    /// <summary>
    ///  The Suppress Output PDU is sent by the client to toggle
    ///  all display updates from the server. This packet does
    ///  not end the session or socket connection. Typically,
    ///  a client sends this packet when its window is either
    ///  minimized or restored.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_9_3.xml
    //  </remarks>
    public partial class Client_Suppress_Output_Pdu : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  TS_SUPPRESS_OUTPUT_PDU (variable number of bytes):The
        ///  actual contents of the Suppress Output PDU, as specified
        ///  in section .
        /// </summary>
        public TS_SUPPRESS_OUTPUT_PDU suppressOutputPduData;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="clientContext">Specify the context.</param>
        public Client_Suppress_Output_Pdu(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Client_Suppress_Output_Pdu()
        {
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> totalBuffer = new List<byte>();

            byte[] suppressPduData = EncodeSuppressData(suppressOutputPduData);
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, suppressPduData, context);

            return RdpbcgrUtility.ToBytes(totalBuffer);
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Client_Suppress_Output_Pdu cloneSuppresspdu = new Client_Suppress_Output_Pdu(context);
            cloneSuppresspdu.commonHeader = commonHeader.Clone();
            cloneSuppresspdu.suppressOutputPduData = suppressOutputPduData;
            cloneSuppresspdu.suppressOutputPduData.pad3Octects =
                RdpbcgrUtility.CloneByteArray(suppressOutputPduData.pad3Octects);
            return cloneSuppresspdu;
        }

        /// <summary>
        /// Encode refreshRectPduData field.
        /// </summary>
        /// <param name="suppressOutputPduData">The data to be encoded.</param>
        /// <returns>The encoded data.</returns>
        private static byte[] EncodeSuppressData(TS_SUPPRESS_OUTPUT_PDU suppressOutputPduData)
        {
            List<byte> suppressBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(suppressBuffer, suppressOutputPduData.shareDataHeader);
            RdpbcgrEncoder.EncodeStructure(suppressBuffer, (byte)suppressOutputPduData.allowDisplayUpdates);
            RdpbcgrEncoder.EncodeBytes(suppressBuffer, suppressOutputPduData.pad3Octects);
            if (suppressOutputPduData.allowDisplayUpdates == AllowDisplayUpdates_SUPPRESS_OUTPUT.ALLOW_DISPLAY_UPDATES)
            {
                RdpbcgrEncoder.EncodeStructure(suppressBuffer, suppressOutputPduData.desktopRect);
            }

            return suppressBuffer.ToArray();
        }
    }

    /// <summary>
    ///  The TS_SUPPRESS_OUTPUT_PDU structure contains the contents
    ///  of the Suppress Output PDU, which is a Share Data Header
    ///  and two fields.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_1_9_3_1.xml
    //  </remarks>
    public partial struct TS_SUPPRESS_OUTPUT_PDU
    {
        /// <summary>
        ///   AShare Data Header containing information about the
        ///  packet (see section ). The type subfield of the pduType
        ///  field of the Share Control Header MUST be set to PDUTYPE_DATAPDU
        ///  (7). The pduType2 field of the Share Data Header MUST
        ///  be set to PDUTYPE2_SUPPRESS_OUTPUT (35).
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        ///  An 8-bit unsigned integer. If set to zero, all screen
        ///  updates from the server are turned off. Any value greater
        ///  than zero will restore display updates from the server.
        /// </summary>
        public AllowDisplayUpdates_SUPPRESS_OUTPUT allowDisplayUpdates;

        /// <summary>
        ///  A 3 element array of 8-bit unsigned integer values.
        ///  Padding. Values in this field are ignored.
        /// </summary>
        public byte[] pad3Octects;

        /// <summary>
        ///  An Inclusive Rectangle which contains the coordinates
        ///  of the virtual desktop if the suppressOutput field
        ///  is greater than zero. If the suppressOutput field is
        ///  set to zero, this field is not included in the PDU.
        /// </summary>
        public TS_RECTANGLE16 desktopRect;
    }

    /// <summary>
    /// The type of AllowDisplayUpdates.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum AllowDisplayUpdates_SUPPRESS_OUTPUT : byte
    {
        /// <summary>
        /// Turn off display updates from the server.
        /// </summary>
        SUPPRESS_DISPLAY_UPDATES = 0,

        /// <summary>
        /// Turn on display updates from the server.
        /// </summary>
        ALLOW_DISPLAY_UPDATES = 1,
    }

    /// <summary>
    ///  The TS_MULTIFRAGMENTUPDATE_CAPABILITYSET structure is
    ///  used to specify capabilities related to the fragmentation
    ///  and reassembly of Fast-Path Updates (see section ).
    ///  This capability is sent by both client and server.
    ///  
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_2_7_2_5.xml
    //  </remarks>
    public partial struct TS_MULTIFRAGMENTUPDATE_CAPABILITYSET : ITsCapsSet
    {

        /// <summary>
        ///  A 16-bit unsigned integer. Type of the capability set.
        ///  This field MUST be set to CAPSETTYPE_MULTIFRAGMENTUPDATE
        ///  (26).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  A 32-bit unsigned integer. The size of the buffer that
        ///  MUST be used to reassemble the fragments of a Fast-Path
        ///  Update (see section ). The size of this buffer places
        ///  a cap on the size of the largest Fast-Path Update that
        ///  can be fragmented (there MUST always be enough buffer
        ///  space to hold all of the related Fast-Path Update fragments
        ///  for reassembly).
        /// </summary>
        public uint MaxRequestSize;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return RdpbcgrUtility.StructToBytes(this);
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    ///  The TS_LARGE_POINTER_CAPABILITYSET structure is used
    ///  to specify capabilities related to large mouse pointer
    ///  shape support. This capability is sent by both client
    ///  and server.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr2_2_7_2_6.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 6)]
    public partial struct TS_LARGE_POINTER_CAPABILITYSET : ITsCapsSet
    {

        /// <summary>
        ///  A 16-bit unsigned integer. Type of the capability set.
        ///  This field MUST be set to CAPSETTYPE_LARGE_POINTER
        ///  (27).
        /// </summary>
        [FieldOffset(0)]
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data, including the size of the capabilitySetType
        ///  and lengthCapability fields.
        /// </summary>
        [FieldOffset(2)]
        public ushort lengthCapability;

        /// <summary>
        ///  Support for large pointer shapes.
        /// </summary>
        [FieldOffset(4)]
        public largePointerSupportFlags_Values largePointerSupportFlags;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return RdpbcgrUtility.StructToBytes(this);
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    /// The type of largePointerSupportFlags.
    /// </summary>
    public enum largePointerSupportFlags_Values : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  96 pixel by 96 pixel mouse pointer shapes are supported.
        /// </summary>
        LARGE_POINTER_FLAG_96x96 = 0x00000001,
    }

    /// <summary>
    ///  The TS_COMPDESK_CAPABILITYSET structure is used to support
    ///  desktop composition. This capability is sent by both
    ///  client and server.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr_desktop_composition_capability_set.xml
    //  </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 6)]
    public partial struct TS_COMPDESK_CAPABILITYSET : ITsCapsSet
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The type of capability set.
        ///  This field MUST be set to 0x0019 (CAPSETTYPE_COMPDESK).
        /// </summary>
        [FieldOffset(0)]
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data.
        /// </summary>
        [FieldOffset(2)]
        public ushort lengthCapability;

        /// <summary>
        ///  A 16-bit unsigned integer. The desktop composition support
        ///  level.
        /// </summary>
        [FieldOffset(4)]
        public CompDeskSupportLevel_Values CompDeskSupportLevel;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return RdpbcgrUtility.StructToBytes(this);
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    /// The type of CompDeskSupportLevel.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum CompDeskSupportLevel_Values : ushort
    {

        /// <summary>
        ///  Desktop composition services are not supported.
        /// </summary>
        COMPDESK_NOT_SUPPORTED = 0x0000,

        /// <summary>
        ///  Desktop composition services are supported.
        /// </summary>
        COMPDESK_SUPPORTED = 0x0001,
    }

    /// <summary>
    ///  The TS_PLAIN_NOTIFY is a fixed-length structure which
    ///  contains 576 bytes of padding.
    /// </summary>
    //  <remarks>
    //   file:///C:/ts_dev/TestSuites/MS-RDPBCGR/TestSuite/Src/TD/latest_XMLS_16may/RDPBCGR/
    //   _rfc_ms-rdpbcgr_plain_notify__ts_plain_notify_.xml
    //  </remarks>
    public partial struct TS_PLAIN_NOTIFY
    {

        /// <summary>
        ///  576 bytes. Padding. Values in this field are ignored.
        /// </summary>
        public byte[] Pad;
    }

    /// <summary>
    ///  The TS_SURFCMDS_CAPABILITYSET structure advertises support for 
    ///  Surface Commands (section 2.2.9.2). 
    ///  This capability is sent by both the client and the server.
    /// </summary>
    public partial struct TS_SURFCMDS_CAPABILITYSET : ITsCapsSet
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The type of capability set.
        ///  This field MUST be set to 0x001C (CAPSETTYPE_SURFACE_COMMANDS).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  A 32-bit, unsigned integer. Flags indicating which Surface Commands are supported.
        /// </summary>
        public CmdFlags_Values cmdFlags;

        /// <summary>
        ///  This field is reserved for future use and has no affect on the RDP wire traffic.
        /// </summary>
        public uint reserved;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return RdpbcgrUtility.StructToBytes(this);
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    /// The type of CmdFlags.
    /// </summary>
    [Flags]
    public enum CmdFlags_Values : uint
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        ///  The Set Surface Bits Command (section 2.2.9.2.1) is supported.
        /// </summary>
        SURFCMDS_SETSURFACEBITS = 0x00000002,

        /// <summary>
        /// The Frame Marker Command (section 2.2.9.2.3) is supported.
        /// </summary>
        SURFCMDS_FRAMEMARKER = 0x00000010,

        /// <summary>
        /// The Stream Surface Bits Command (section 2.2.9.2.2) is supported.
        /// </summary>
        SURFCMDS_STREAMSURFACEBITS = 0x00000040
    }

    /// <summary>
    ///  The TS_BITMAPCODECS_CAPABILITYSET structure advertises support for bitmap 
    ///  encoding and decoding codecs used in conjunction with the Set Surface Bits 
    ///  Surface Command (section 2.2.9.2.1) and Cache Bitmap (Revision 3) Secondary 
    ///  Drawing Order ([MS-RDPEGDI] section 2.2.2.2.1.2.8).
    ///  This capability is sent by both the client and server.
    /// </summary>
    public partial struct TS_BITMAPCODECS_CAPABILITYSET : ITsCapsSet
    {

        /// <summary>
        ///  A 16-bit unsigned integer. The type of capability set.
        ///  This field MUST be set to 0x001D (CAPSETTYPE_BITMAP_CODECS).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        ///  A variable-length field containing a TS_BITMAPCODECS structure (section 2.2.7.2.10.1).
        /// </summary>
        public TS_BITMAPCODECS supportedBitmapCodecs;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            List<byte> encodeBuffer = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(encodeBuffer, (ushort)capabilitySetType);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, lengthCapability);
            RdpbcgrEncoder.EncodeStructure(encodeBuffer, supportedBitmapCodecs.bitmapCodecCount);
            for (int i = 0; i < supportedBitmapCodecs.bitmapCodecCount; ++i)
            {
                RdpbcgrEncoder.EncodeStructure(encodeBuffer, supportedBitmapCodecs.bitmapCodecArray[i].codecGUID);
                RdpbcgrEncoder.EncodeStructure(encodeBuffer, supportedBitmapCodecs.bitmapCodecArray[i].codecID);
                RdpbcgrEncoder.EncodeStructure(encodeBuffer,
                                               supportedBitmapCodecs.bitmapCodecArray[i].codecPropertiesLength);
                if (supportedBitmapCodecs.bitmapCodecArray[i].codecProperties != null)
                {
                    RdpbcgrEncoder.EncodeBytes(encodeBuffer, supportedBitmapCodecs.bitmapCodecArray[i].codecProperties);
                }
            }

            return encodeBuffer.ToArray();
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    /// The TS_BITMAPCODECS structure contains an array of bitmap codec capabilities.
    /// </summary>
    public struct TS_BITMAPCODECS
    {
        /// <summary>
        /// An 8-bit, unsigned integer. 
        /// The number of bitmap codec capability entries contained 
        /// in the bitmapCodecArray field (the maximum allowed is 255).
        /// </summary>
        public byte bitmapCodecCount;

        /// <summary>
        /// A variable-length array containing a series of TS_BITMAPCODEC structures 
        /// (section 2.2.7.2.10.1.1) that describes the supported bitmap codecs. 
        /// The number of TS_BITMAPCODEC structures contained in the array 
        /// is given by the bitmapCodecCount field.
        /// </summary>
        public TS_BITMAPCODEC[] bitmapCodecArray;
    }

    /// <summary>
    /// The TS_BITMAPCODEC structure is used to describe the encoding parameters of a bitmap codec.
    /// </summary>
    public struct TS_BITMAPCODEC
    {
        /// <summary>
        /// A Globally Unique Identifier (section 2.2.7.2.10.1.1.1) that functions 
        /// as a unique ID for each bitmap codec.
        /// </summary>
        public TS_BITMAPCODEC_GUID codecGUID;

        /// <summary>
        /// An 8-bit unsigned integer. When sent from the client to the server, 
        /// this field contains a unique 8-bit ID that can be used to identify 
        /// the codec in wire traffic associated with the current connection — 
        /// this ID is used in subsequent Set Surface Bits commands (section 2.2.9.2.1)
        /// and Cache Bitmap (Revision 3) orders ([MS-RDPEGDI] section 2.2.2.2.1.2.8).
        /// When sent from the server to the client, the value in this field is ignored 
        /// by the client — the client determines the 8-bit ID to use for the codec.
        /// </summary>
        public byte codecID;

        /// <summary>
        /// A 16-bit, unsigned integer. The size, in bytes, of the codecProperties field.
        /// </summary>
        public ushort codecPropertiesLength;

        /// <summary>
        /// A variable-length array of bytes containing data that describes the encoding
        /// parameter of the bitmap codec.
        /// </summary>
        public byte[] codecProperties;
    }

    /// <summary>
    /// The GUID structure contains 128 bits that represent a globally unique 
    /// identifier that can be used to provide a distinctive reference number.
    /// </summary>
    public struct TS_BITMAPCODEC_GUID
    {
        /// <summary>
        /// A 32-bit, unsigned integer. The first GUID component.
        /// </summary>
        public uint codecGUID1;

        /// <summary>
        /// A 16-bit, unsigned integer. The second GUID component.
        /// </summary>
        public ushort codecGUID2;

        /// <summary>
        /// A 16-bit, unsigned integer. The third GUID component.
        /// </summary>
        public ushort codecGUID3;

        /// <summary>
        /// A 8-bit, unsigned integer. The fourth GUID component.
        /// </summary>
        public byte codecGUID4;

        /// <summary>
        /// A 8-bit, unsigned integer. The fifth GUID component.
        /// </summary>
        public byte codecGUID5;

        /// <summary>
        /// A 8-bit, unsigned integer. The sixth GUID component.
        /// </summary>
        public byte codecGUID6;

        /// <summary>
        /// A 8-bit, unsigned integer. The seventh GUID component.
        /// </summary>
        public byte codecGUID7;

        /// <summary>
        /// A 8-bit, unsigned integer. The eighth GUID component.
        /// </summary>
        public byte codecGUID8;

        /// <summary>
        /// A 8-bit, unsigned integer. The ninth GUID component.
        /// </summary>
        public byte codecGUID9;

        /// <summary>
        /// A 8-bit, unsigned integer. The tenth GUID component.
        /// </summary>
        public byte codecGUID10;

        /// <summary>
        /// A 8-bit, unsigned integer. The eleventh GUID component.
        /// </summary>
        public byte codecGUID11;

    }

    /// <summary>
    /// The TS_FRAME_ACKNOWLEDGE_CAPABILITYSET structure advertises support for frame acknowledgment using the TS_FRAME_ACKNOWLEDGE_PDU (section 2.2.3.1) structure. 
    /// </summary>
    public partial struct TS_FRAME_ACKNOWLEDGE_CAPABILITYSET : ITsCapsSet
    {
        /// <summary>
        ///  A 16-bit unsigned integer. The type of capability set.
        ///  This field MUST be set to 0x001C (CAPSETTYPE_SURFACE_COMMANDS).
        /// </summary>
        public capabilitySetType_Values capabilitySetType;

        /// <summary>
        ///  A 16-bit unsigned integer. The length in bytes of the
        ///  capability data.
        /// </summary>
        public ushort lengthCapability;

        /// <summary>
        /// 
        /// </summary>
        public uint maxUnacknowledgedFrameCount;

        /// <summary>
        /// convert this structure to byte array
        /// </summary>
        /// <returns>the result byte array</returns>
        public byte[] ToBytes()
        {
            return RdpbcgrUtility.StructToBytes(this);
        }


        /// <summary>
        /// to get the type of this struct.
        /// </summary>
        /// <returns>the type of capabilitySetType_Values</returns>
        public capabilitySetType_Values GetCapabilityType()
        {
            return capabilitySetType;
        }
    }

    /// <summary>
    /// The TS_FRAME_ACKNOWLEDGE_PDU structure is a client-to-server PDU sent to the server 
    /// whenever the client receives a Frame Marker Command ([MS-RDPBCGR] section 2.2.9.2.3) 
    /// with the frameAction field set to SURFACECMD_FRAMEACTION_END (0x0001) and 
    /// it has finished processing this particular frame (that is, the surface bits have been rendered on the screen). 
    /// </summary>
    public class TS_FRAME_ACKNOWLEDGE_PDU : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  Share Data Header containing information about the packet.
        ///  The type subfield of the pduType field of the Share
        ///  Control Header MUST be set to PDUTYPE_DATAPDU (7).
        ///  The pduType2 field of the              Share Data Header
        ///  MUST be set to PDUTYPE2_INPUT (28).           
        /// </summary>
        public TS_SHAREDATAHEADER shareDataHeader;

        /// <summary>
        /// A 32-bit unsigned integer. This field specifies the 32-bit identifier of the frame that 
        /// was sent to the client using a Frame Marker Command and is being acknowledged as delivered. 
        /// If frameID has the value 0xFFFFFFFF, the server SHOULD assume that all in-flight frames have been acknowledged.
        /// </summary>
        public uint frameID;
    }

    /// <summary>
    /// Request type of Network Characteristics Detection Request
    /// </summary>
    public enum AUTO_DETECT_REQUEST_TYPE : ushort
    {
        /// <summary>
        /// RDP_RTT_REQUEST
        /// The RTT Measure Request message is encapsulated in the autoDetectReqData field of an Auto-Detect Request PDU (section 2.2.14.3) 
        /// sent after the RDP Connection Sequence (section 1.3.1.1) has completed.
        /// </summary>
        RDP_RTT_REQUEST_AFTER_CONNECTTIME = 0x0001,

        /// <summary>
        /// RDP_RTT_REQUEST
        /// The RTT Measure Request message is encapsulated in the autoDetectReqData field of an Auto-Detect Request PDU 
        /// sent during the Optional Connect-Time Auto-Detection phase of the RDP Connection Sequence.
        /// </summary>
        RDP_RTT_REQUEST_IN_CONNECTTIME = 0x1001,

        /// <summary>
        /// RDP_BW_START
        /// One of two possible meanings:
        /// The Bandwidth Measure Start message is encapsulated in the SubHeaderData field of an RDP_TUNNEL_SUBHEADER ([MS-RDPEMT] section 2.2.1.1.1) structure that is being tunneled over a reliable UDP multitransport connection ([MS-RDPEMT] sections 1.3 and 2.1).
        /// The Bandwidth Measure Start message is encapsulated in the autoDetectReqData field of an Auto-Detect Request PDU (section 2.2.14.3) sent after the RDP Connection Sequence (section 1.3.1.1) has completed.
        /// </summary>
        RDP_BW_START_AFTER_CONNECTTIME_OR_RELIABLEUDP = 0x0014,

        /// <summary>
        /// RDP_BW_START
        /// The Bandwidth Measure Start message is encapsulated in the SubHeaderData field of an RDP_TUNNEL_SUBHEADER ([MS-RDPEMT] section 2.2.1.1.1) structure that is being tunneled over a lossy UDP multitransport connection ([MS-RDPEMT] sections 1.3 and 2.1).
        /// </summary>
        RDP_BW_START_AFTER_CONNECTTIME_OR_LOSSYUDP = 0x0114,

        /// <summary>
        /// RDP_BW_START
        /// The Bandwidth Measure Start message is encapsulated in the autoDetectReqData field of an Auto-Detect Request PDU 
        /// sent during the Optional Connect-Time Auto-Detection phase of the RDP Connection Sequence.
        /// </summary>
        RDP_BW_START_IN_CONNECTTIME = 0x1014,

        /// <summary>
        /// RDP_BW_PAYLOAD
        /// </summary>
        RDP_BW_PAYLOAD = 0x0002,

        /// <summary>
        /// RDP_BW_STOP
        /// One of two possible meanings:
        /// The Bandwidth Measure Stop message is encapsulated in the SubHeaderData field of an RDP_TUNNEL_SUBHEADER ([MS-RDPEMT] section 2.2.1.1.1) structure that is being tunneled over a reliable UDP multitransport connection ([MS-RDPEMT] sections 1.3 and 2.1).
        /// The Bandwidth Measure Stop message is encapsulated in the autoDetectReqData field of an Auto-Detect Request PDU sent after the RDP Connection Sequence has completed.
        /// </summary>
        RDP_BW_STOP_AFTER_CONNECTTIME_OR_RELIABLEUDP = 0x0429,

        /// <summary>
        /// RDP_BW_STOP
        /// The Bandwidth Measure Stop message is encapsulated in the SubHeaderData field of an RDP_TUNNEL_SUBHEADER ([MS-RDPEMT] section 2.2.1.1.1) structure that is being tunneled over a lossy UDP multitransport connection ([MS-RDPEMT] sections 1.3 and 2.1).
        /// </summary>
        RDP_BW_STOP_AFTER_CONNECTTIME_OR_LOSSYUDP = 0x0629,

        /// <summary>
        /// RDP_BW_STOP
        /// RDP_BW_STOP, The Bandwidth Measure Stop message is encapsulated in the autoDetectReqData field of an Auto-Detect Request PDU (section 2.2.14.3) sent during the Optional Connect-Time Auto-Detection phase of the RDP Connection Sequence (section 1.3.1.1). The payloadLength field is present and has a value greater than zero.
        /// </summary>
        RDP_BW_STOP_IN_CONNECTTIME = 0x002B,

        /// <summary>
        /// RDP_NETCHAR_RESULT, The baseRTT and averageRTT fields are present in the Network Characteristics Result message (the bandwidth field is not present).
        /// </summary>
        RDP_NETCHAR_RESULT_BASERTT_AVERAGERTT = 0x0840,

        /// <summary>
        /// RDP_NETCHAR_RESULT, The bandwidth and averageRTT fields are present in the Network Characteristics Result message (the baseRTT field is not present).
        /// </summary>
        RDP_NETCHAR_RESULT_BANDWIDTH_AVERAGERTT = 0x0880,

        /// <summary>
        /// RDP_NETCHAR_RESULT, The baseRTT, bandwidth and averageRTT fields are present in the Network Characteristics Result message.
        /// </summary>
        RDP_NETCHAR_RESULT_BASERTT_BANDWIDTH_AVERAGERTT = 0x08C0,
    }

    /// <summary>
    /// Values of headerTypeId of network detection request
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum HeaderTypeId_Values : byte
    {
        TYPE_ID_AUTODETECT_REQUEST = 0x00,
        TYPE_ID_AUTODETECT_RESPONSE = 0x01,
    }

    /// <summary>
    /// Base class for all network detection request structure
    /// </summary>
    public abstract class NETWORK_DETECTION_REQUEST
    {
        /// <summary>
        /// An 8-bit unsigned integer that specifies the size, in bytes, of the header data.
        /// </summary>
        public byte headerLength;

        /// <summary>
        /// An 8-bit unsigned integer that specifies the high-level type. This field MUST be set to TYPE_ID_AUTODETECT_REQUEST (0x00).
        /// </summary>
        public HeaderTypeId_Values headerTypeId;

        /// <summary>
        /// A 16-bit unsigned integer that specifies the message sequence number.
        /// </summary>
        public ushort sequenceNumber;

        /// <summary>
        /// A 16-bit unsigned integer that specifies a request type code. 
        /// </summary>
        public AUTO_DETECT_REQUEST_TYPE requestType;

        public abstract NETWORK_DETECTION_REQUEST Clone();


    }

    /// <summary>
    /// The RDP_RTT_REQUEST structure is used to initiate a round-trip time measurement operation.
    /// </summary>
    public class RDP_RTT_REQUEST : NETWORK_DETECTION_REQUEST
    {
        /// <summary>
        /// The time(millisecond) of send this pdu, used to calculate RTT.
        /// </summary>
        public DateTime sendTime;

        public override NETWORK_DETECTION_REQUEST Clone()
        {
            RDP_RTT_REQUEST rttReq = new RDP_RTT_REQUEST();
            rttReq.headerLength = this.headerLength;
            rttReq.headerTypeId = this.headerTypeId;
            rttReq.requestType = this.requestType;
            rttReq.sequenceNumber = this.sequenceNumber;

            rttReq.sendTime = this.sendTime;
            return rttReq;
        }
    }

    /// <summary>
    /// The RDP_BW_START structure is used to start a bandwidth measurement operation.
    /// </summary>
    public class RDP_BW_START : NETWORK_DETECTION_REQUEST
    {
        public override NETWORK_DETECTION_REQUEST Clone()
        {
            RDP_BW_START bwStart = new RDP_BW_START();
            bwStart.headerLength = this.headerLength;
            bwStart.headerTypeId = this.headerTypeId;
            bwStart.requestType = this.requestType;
            bwStart.sequenceNumber = this.sequenceNumber;
            return bwStart;
        }
    }

    /// <summary>
    /// The RDP_BW_PAYLOAD structure is used to transfer data associated with a bandwidth measurement operation 
    /// that takes place during the Optional Connect-Time Auto-Detection phase of the RDP Connection Sequence
    /// </summary>
    public class RDP_BW_PAYLOAD : NETWORK_DETECTION_REQUEST
    {
        /// <summary>
        /// A 16-bit unsigned integer that specifies the size, in bytes, of the payload field.
        /// </summary>
        public ushort payloadLength;

        /// <summary>
        /// A variable-length array of bytes that contains random data. The number of bytes in this array is specified by the payloadLength field.
        /// </summary>
        public byte[] payload;

        public override NETWORK_DETECTION_REQUEST Clone()
        {
            RDP_BW_PAYLOAD bwPayload = new RDP_BW_PAYLOAD();
            bwPayload.headerLength = this.headerLength;
            bwPayload.headerTypeId = this.headerTypeId;
            bwPayload.requestType = this.requestType;
            bwPayload.sequenceNumber = this.sequenceNumber;
            bwPayload.payloadLength = this.payloadLength;
            bwPayload.payload = RdpbcgrUtility.CloneByteArray(this.payload);
            return bwPayload;
        }
    }

    /// <summary>
    /// The RDP_BW_STOP structure is used to stop a bandwidth measurement operation.
    /// </summary>
    public class RDP_BW_STOP : NETWORK_DETECTION_REQUEST
    {
        /// <summary>
        /// A 16-bit unsigned integer that specifies the size, in bytes, of the payload field.
        /// </summary>
        public ushort payloadLength;

        /// <summary>
        /// A variable-length array of bytes that contains random data. The number of bytes in this array is specified by the payloadLength field.
        /// </summary>
        public byte[] payload;

        public override NETWORK_DETECTION_REQUEST Clone()
        {
            RDP_BW_STOP bwStop = new RDP_BW_STOP();
            bwStop.headerLength = this.headerLength;
            bwStop.headerTypeId = this.headerTypeId;
            bwStop.requestType = this.requestType;
            bwStop.sequenceNumber = this.sequenceNumber;
            if (this.payloadLength != 0)
            {
                bwStop.payloadLength = this.payloadLength;
                bwStop.payload = RdpbcgrUtility.CloneByteArray(this.payload);
            }
            return bwStop;
        }
    }

    /// <summary>
    /// The RDP_NETCHAR_RESULTS structure is used by the server to send detected network characteristics to the client.
    /// </summary>
    public class RDP_NETCHAR_RESULT : NETWORK_DETECTION_REQUEST
    {
        /// <summary>
        /// An optional 32-bit unsigned integer that specifies the lowest detected round-trip time in milliseconds.
        /// </summary>
        public uint baseRTT;

        /// <summary>
        /// An optional 32-bit unsigned integer that specifies the current bandwidth in kilobits per second.
        /// </summary>
        public uint bandwidth;

        /// <summary>
        /// An optional 32-bit unsigned integer that specifies the current average round-trip time in milliseconds.
        /// </summary>
        public uint averageRTT;

        public override NETWORK_DETECTION_REQUEST Clone()
        {
            RDP_NETCHAR_RESULT netCharRes = new RDP_NETCHAR_RESULT();
            netCharRes.headerLength = this.headerLength;
            netCharRes.headerTypeId = this.headerTypeId;
            netCharRes.requestType = this.requestType;
            netCharRes.sequenceNumber = this.sequenceNumber;

            netCharRes.baseRTT = this.baseRTT;
            netCharRes.bandwidth = this.bandwidth;
            netCharRes.averageRTT = this.averageRTT;

            return netCharRes;
        }
    }

    /// <summary>
    /// Type of Network Characteristics Detection Response
    /// </summary>
    public enum AUTO_DETECT_RESPONSE_TYPE : ushort
    {
        /// <summary>
        /// RTT Measure Response 
        /// </summary>
        RDP_RTT_RESPONSE = 0x0000,

        /// <summary>
        /// Bandwidth Measure Results, The Bandwidth Measure Results message is encapsulated in the autoDetectReqData field of an Auto-Detect Request PDU (section 2.2.14.3) sent during the Optional Connect-Time Auto-Detection phase of the RDP Connection Sequence (see section 1.3.1.1 for an overview of the RDP Connection Sequence phases).
        /// </summary>
        RDP_BW_RESULTS_DURING_CONNECT = 0x0003,

        /// <summary>
        /// Bandwidth Measure Results, One of two possible meanings:
        /// The Bandwidth Measure Results message is encapsulated in the autoDetectReqData field of an Auto-Detect Request PDU (section 2.2.14.3) sent after the Optional Connect-Time Auto-Detection phase of the RDP Connection Sequence (see section 1.3.1.1 for an overview of the RDP Connection Sequence phases).
        /// The Bandwidth Measure Results message is encapsulated in the SubHeaderData field of an RDP_TUNNEL_SUBHEADER ([MS-RDPEMT] section 2.2.1.1.1) structure.
        /// </summary>
        RDP_BW_RESULTS_AFTER_CONNECT = 0x000B,

        /// <summary>
        /// Network Characteristics Sync 
        /// </summary>
        RDP_NETCHAR_SYNC = 0x0018,

    }

    /// <summary>
    /// Base class for all the Network Detection Response Structures
    /// </summary>
    public abstract class NETWORK_DETECTION_RESPONSE
    {
        /// <summary>
        /// An 8-bit unsigned integer that specifies the size, in bytes, of the header data.
        /// </summary>
        public byte headerLength;

        /// <summary>
        /// An 8-bit unsigned integer that specifies the high-level type. This field MUST be set to TYPE_ID_AUTODETECT_REQUEST (0x00).
        /// </summary>
        public HeaderTypeId_Values headerTypeId;

        /// <summary>
        /// A 16-bit unsigned integer that specifies the message sequence number.
        /// </summary>
        public ushort sequenceNumber;

        /// <summary>
        /// A 16-bit unsigned integer that specifies a response type code.
        /// </summary>
        public AUTO_DETECT_RESPONSE_TYPE responseType;

        public abstract NETWORK_DETECTION_RESPONSE Clone();
    }

    /// <summary>
    /// The RDP_RTT_RESPONSE structure is used to respond to round-trip time measurement operations initiated by the RTT Measure Request (section 2.2.14.1.1) message.
    /// </summary>
    public class RDP_RTT_RESPONSE : NETWORK_DETECTION_RESPONSE
    {
        public override NETWORK_DETECTION_RESPONSE Clone()
        {
            RDP_RTT_RESPONSE rttResp = new RDP_RTT_RESPONSE();
            rttResp.headerLength = this.headerLength;
            rttResp.headerTypeId = this.headerTypeId;
            rttResp.sequenceNumber = this.sequenceNumber;
            rttResp.responseType = this.responseType;

            return rttResp;
        }
    }

    /// <summary>
    /// The RDP_BW_RESULTS structure is used to send the results of a bandwidth measurement operation to the initiating end-point.
    /// </summary>
    public class RDP_BW_RESULTS : NETWORK_DETECTION_RESPONSE
    {
        /// <summary>
        /// A 32-bit unsigned integer that specifies the time delta, in milliseconds, between the receipt of the Bandwidth Measure Start and the Bandwidth Measure Stop messages.
        /// </summary>
        public uint timeDelta;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the total data received in the Bandwidth Measure Payload messages.
        /// </summary>
        public uint byteCount;

        public override NETWORK_DETECTION_RESPONSE Clone()
        {
            RDP_BW_RESULTS bwResults = new RDP_BW_RESULTS();
            bwResults.headerLength = this.headerLength;
            bwResults.headerTypeId = this.headerTypeId;
            bwResults.sequenceNumber = this.sequenceNumber;
            bwResults.responseType = this.responseType;

            bwResults.timeDelta = this.timeDelta;
            bwResults.byteCount = this.byteCount;

            return bwResults;
        }
    }

    /// <summary>
    /// The RDP_NETCHAR_SYNC structure is sent in response to the RTT Measure Request (section 2.2.14.1.1) message or Bandwidth Measure Start (section 2.2.14.1.2) message 
    /// and is used to short-circuit connect-time network characteristics detection in the case of an auto-reconnect (section 1.3.1.5 and 2.2.4).
    /// </summary>
    public class RDP_NETCHAR_SYNC : NETWORK_DETECTION_RESPONSE
    {
        /// <summary>
        /// A 32-bit unsigned integer that specifies the previously detected bandwidth in kilobits per second.
        /// </summary>
        public uint bandwidth;

        /// <summary>
        /// A 32-bit unsigned integer that specifies the previously detected round-trip time in milliseconds.
        /// </summary>
        public uint rtt;

        public override NETWORK_DETECTION_RESPONSE Clone()
        {
            RDP_NETCHAR_SYNC netCharSync = new RDP_NETCHAR_SYNC();
            netCharSync.headerLength = this.headerLength;
            netCharSync.headerTypeId = this.headerTypeId;
            netCharSync.sequenceNumber = this.sequenceNumber;
            netCharSync.responseType = this.responseType;

            netCharSync.bandwidth = this.bandwidth;
            netCharSync.rtt = this.rtt;

            return netCharSync;
        }
    }

    /// <summary>
    /// The Auto-Detect Request PDU is sent by the server to the client and is used to 
    /// detect network characteristics such as bandwidth and round-trip time.
    /// </summary>
    public class Server_Auto_Detect_Request_PDU : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        /// A variable-length field that contains auto-detect request data
        /// </summary>
        public NETWORK_DETECTION_REQUEST autoDetectReqData;

        public Server_Auto_Detect_Request_PDU()
        {
        }

        public Server_Auto_Detect_Request_PDU(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// Create an instance of the class that is identical to the current PDU. 
        /// </summary>
        /// <returns>The new instance.</returns>
        public override StackPacket Clone()
        {
            Server_Auto_Detect_Request_PDU serverAutoDetectRequestPdu = new Server_Auto_Detect_Request_PDU();
            serverAutoDetectRequestPdu.commonHeader = commonHeader.Clone();
            serverAutoDetectRequestPdu.autoDetectReqData = autoDetectReqData.Clone();

            return serverAutoDetectRequestPdu;
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> reqDataBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeNetworkDetectionRequest(reqDataBuffer, autoDetectReqData);

            byte[] dataBuffer = reqDataBuffer.ToArray();

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, dataBuffer);

            return encodedBytes;
        }
    }

    /// <summary>
    /// The Auto-Detect Response PDU is sent by the client to the server and is used to 
    /// detect network characteristics such as bandwidth and round-trip time.
    /// </summary>
    public class Client_Auto_Detect_Response_PDU : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        /// A variable-length field that contains auto-detect response data
        /// </summary>
        public NETWORK_DETECTION_RESPONSE autodetectRspPduData;

        public Client_Auto_Detect_Response_PDU()
            : base()
        {
        }

        public Client_Auto_Detect_Response_PDU(RdpbcgrClientContext clientContext)
            : base(clientContext)
        {
        }
        public override StackPacket Clone()
        {
            Client_Auto_Detect_Response_PDU clientAutoDetectResponsePdu = new Client_Auto_Detect_Response_PDU(context);
            clientAutoDetectResponsePdu.commonHeader = commonHeader.Clone();
            clientAutoDetectResponsePdu.autodetectRspPduData = autodetectRspPduData.Clone();

            return clientAutoDetectResponsePdu;
        }

        /// <summary>
        /// Encode this structure into byte array.
        /// </summary>
        /// <returns>The byte array of the structure.</returns>
        public override byte[] ToBytes()
        {
            List<byte> reqDataBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeNetworkDetectionResponse(reqDataBuffer, autodetectRspPduData);

            byte[] dataBuffer = reqDataBuffer.ToArray();

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, context);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            return encodedBytes;
        }

    }

    /// <summary>
    /// Values of requestedProtocol of Server_Initiate_Multitransport_Request_PDU, 
    /// specifies the protocol to use in the transport.
    /// </summary>
    public enum Multitransport_Protocol_value : ushort
    {
        /// <summary>
        /// RDP-UDP Forward Error Correction (FEC) reliable transport ([MS-RDPEUDP] sections 1 to 3).
        /// </summary>
        INITITATE_REQUEST_PROTOCOL_UDPFECR = 0x01,

        /// <summary>
        /// RDP-UDP FEC lossy transport ([MS-RDPEUDP] sections 1 to 3).
        /// </summary>
        INITITATE_REQUEST_PROTOCOL_UDPFECL = 0x02,
    }

    /// <summary>
    /// The Initiate Multitransport Request PDU is sent by the server to the client and 
    /// is used to bootstrap the creation of a side-band channel ([MS-RDPEMT] section 1.3). 
    /// </summary>
    public class Server_Initiate_Multitransport_Request_PDU : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        /// A 32-bit unsigned integer that specifies a unique ID that the server MUST use to associate this Initiate Multitransport Request PDU 
        /// with the Tunnel Create Request PDU sent by the client after the transport has been established.
        /// </summary>
        public uint requestId;

        /// <summary>
        /// A 16-bit unsigned integer that specifies the protocol to use in the transport.
        /// </summary>
        public Multitransport_Protocol_value requestedProtocol;

        /// <summary>
        /// A 16-bit unsigned integer. This field is unused and reserved for future use. It MUST be set to zero.
        /// </summary>
        public ushort reserved;

        /// <summary>
        /// A 16-element array of 8-bit unsigned integers that contains randomly generated data.
        /// </summary>
        public byte[] securityCookie;

        public Server_Initiate_Multitransport_Request_PDU()
        {
        }

        public Server_Initiate_Multitransport_Request_PDU(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {

        }

        public override byte[] ToBytes()
        {
            List<byte> reqDataBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(reqDataBuffer, requestId);
            RdpbcgrEncoder.EncodeStructure(reqDataBuffer, (ushort)requestedProtocol);
            RdpbcgrEncoder.EncodeStructure(reqDataBuffer, reserved);
            RdpbcgrEncoder.EncodeBytes(reqDataBuffer, securityCookie);

            byte[] dataBuffer = reqDataBuffer.ToArray();

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, dataBuffer);

            return encodedBytes;

        }

        public override StackPacket Clone()
        {
            Server_Initiate_Multitransport_Request_PDU requestPDU = new Server_Initiate_Multitransport_Request_PDU(serverSessionContext);
            requestPDU.commonHeader = commonHeader.Clone();
            requestPDU.requestId = requestId;
            requestPDU.requestedProtocol = requestedProtocol;
            requestPDU.reserved = reserved;
            requestPDU.securityCookie = RdpbcgrUtility.CloneByteArray(securityCookie);

            return requestPDU;

        }
    }

    /// <summary>
    /// The Heartbeat PDU is sent by the server to the client and allows the client to 
    /// monitor the state of the connection to the server in real time.
    /// </summary>
    public class Server_Heartbeat_PDU : RdpbcgrServerPdu
    {
        public Server_Heartbeat_PDU()
        {
        }

        public Server_Heartbeat_PDU(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        /// An 8-bit unsigned integer reserved for future use. This field MUST be set to zero.
        /// </summary>
        public byte reserved;

        /// <summary>
        /// An 8-bit unsigned integer that specifies the time (in seconds) between Heartbeat PDUs.
        /// </summary>
        public byte period;

        /// <summary>
        /// An 8-bit unsigned integer that specifies how many missed heartbeats SHOULD trigger a client-side warning. The client MAY ignore this value.
        /// </summary>
        public byte count1;

        /// <summary>
        /// An 8-bit unsigned integer that specifies how many missed heartbeats SHOULD trigger a client-side reconnection attempt. The client MAY ignore this value.
        /// </summary>
        public byte count2;

        public override byte[] ToBytes()
        {
            List<byte> reqDataBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(reqDataBuffer, reserved);
            RdpbcgrEncoder.EncodeStructure(reqDataBuffer, period);
            RdpbcgrEncoder.EncodeStructure(reqDataBuffer, count1);
            RdpbcgrEncoder.EncodeStructure(reqDataBuffer, count2);

            byte[] dataBuffer = reqDataBuffer.ToArray();

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, dataBuffer);

            return encodedBytes;

        }

        public override StackPacket Clone()
        {
            Server_Heartbeat_PDU heartbeatPDU = new Server_Heartbeat_PDU();
            heartbeatPDU.commonHeader = this.commonHeader.Clone();
            heartbeatPDU.reserved = this.reserved;
            heartbeatPDU.period = this.period;
            heartbeatPDU.count1 = this.count1;
            heartbeatPDU.count2 = this.count2;

            return heartbeatPDU;
        }

    }

    /// <summary>
    /// A 32-bit unsigned integer that specifies an error code.
    /// </summary>
    public enum HrResponse_Value : uint
    {
        /// <summary>
        /// Indicates that the client was unable to complete the multitransport initiation request.
        /// </summary>
        E_ABORT = 0x80004004,

        /// <summary>
        /// Indicates that the client was successfully able to complete the multitransport initiation request.
        /// </summary>
        S_OK = 0x00000000,

    }

    /// <summary>
    /// The Initiate Multitransport Response PDU is sent by the client to the server and is used to 
    /// indicate to the server whether the client was able to complete the multitransport initiation request 
    /// associated with the ID specified in the requestId field.
    /// </summary>
    public class Client_Initiate_Multitransport_Response_PDU : RdpbcgrClientPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        /// A 32-bit unsigned integer that MUST contain the ID 
        /// that was sent to the client in the requestId field 
        /// of the associated Initiate Multitransport Request PDU (section 2.2.15.1).
        /// </summary>
        public uint requestId;

        /// <summary>
        /// A 32-bit unsigned integer that specifies an error code.
        /// </summary>
        public HrResponse_Value hrResponse;

        public Client_Initiate_Multitransport_Response_PDU()
            : base()
        {
        }

        public Client_Initiate_Multitransport_Response_PDU(RdpbcgrClientContext context)
            : base(context)
        {
        }

        public override StackPacket Clone()
        {
            Client_Initiate_Multitransport_Response_PDU errorPDU = new Client_Initiate_Multitransport_Response_PDU(context);
            errorPDU.commonHeader = commonHeader.Clone();
            errorPDU.requestId = requestId;
            errorPDU.hrResponse = hrResponse;

            return errorPDU;
        }

        public override byte[] ToBytes()
        {
            List<byte> reqDataBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(reqDataBuffer, requestId);
            RdpbcgrEncoder.EncodeStructure(reqDataBuffer, (uint)hrResponse);

            byte[] dataBuffer = reqDataBuffer.ToArray();

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, context);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            return encodedBytes;
        }
    }

    #region self-defined structures
    /// <summary>
    ///  The Standard Security Server Redirection PDU is sent by
    ///  the server to the client to instruct it to reconnect to
    ///  an existing session on another server. The information 
    ///  required to perform the reconnection is contained in an 
    ///  embedded Server Redirection Packet (see section 2.2.13.1). 
    ///  This PDU MUST NOT be sent if Enhanced RDP Security 
    ///  (see section 5.4) is in effect.
    /// </summary>
    public partial class Server_Redirection_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  Information required by the client to initiate a
        ///  reconnection to a given session on a target server
        ///  encapsulated in a Server Redirection Packet (section 2.2.13.1) structure.
        /// </summary>
        public RDP_SERVER_REDIRECTION_PACKET serverRedirectionPdu;

        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="serverSessionContext">Specify the session context.</param>
        public Server_Redirection_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        /// <summary>
        /// The constructor of the class with no parameter.
        /// </summary>
        public Server_Redirection_Pdu()
        {
        }

        public override StackPacket Clone()
        {
            Server_Redirection_Pdu pduClone = new Server_Redirection_Pdu();
            pduClone.commonHeader = commonHeader;
            pduClone.serverRedirectionPdu = serverRedirectionPdu.Clone();
            return pduClone;
        }

        public override byte[] ToBytes()
        {
            byte[] dataBuffer = serverRedirectionPdu.Encode();

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, dataBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, dataBuffer);

            return encodedBytes;
        }
    }

    /// <summary>
    ///  The Enhanced Security Server Redirection PDU is sent by the 
    ///  server to the client to instruct it to reconnect to an existing
    ///  session on another server. The information required to perform
    ///  the reconnection is contained in an embedded Server Redirection 
    ///  Packet (section 2.2.13.1). This PDU MUST NOT be sent if Standard
    ///  RDP Security (see section 5.3) is in effect. The Standard Security
    ///  Server Redirection PDU (see section 2.2.13.2.1) MUST be used instead.
    /// </summary>
    public partial class Enhanced_Security_Server_Redirection_Pdu : RdpbcgrServerPdu
    {
        /// <summary>
        ///  The slow path header.
        /// </summary>
        public SlowPathPduCommonHeader commonHeader;

        /// <summary>
        ///  A Share Control Header (as specified in section 2.2.8.1.1.1.1)
        ///  containing information on the packet. The type subfield of the 
        ///  pduType field of the Share Control Header MUST be set to 
        ///  PDUTYPE_SERVER_REDIR_PKT (10). The versionHigh and versionLow 
        ///  subfields MUST both be set to 0.
        /// </summary>
        public TS_SHARECONTROLHEADER shareControlHeader;

        /// <summary>
        ///  A 16-bit, unsigned integer. Padding. Values in this field MUST be ignored.
        /// </summary>
        public ushort pad;

        /// <summary>
        ///  Information required by the client to initiate a reconnection 
        ///  to a given session on a target server encapsulated in a Server 
        ///  Redirection Packet (section 2.2.13.1) structure.
        /// </summary>
        public RDP_SERVER_REDIRECTION_PACKET serverRedirectionPdu;

        /// <summary>
        /// An optional 8-bit, unsigned integer. Padding. Values in this field MUST be ignored.
        /// </summary>
        public byte[] pad1Octet;

        public Enhanced_Security_Server_Redirection_Pdu(RdpbcgrServerSessionContext serverSessionContext)
            : base(serverSessionContext)
        {
        }

        public Enhanced_Security_Server_Redirection_Pdu()
        {
        }

        public override StackPacket Clone()
        {
            Enhanced_Security_Server_Redirection_Pdu pduClone = new Enhanced_Security_Server_Redirection_Pdu();
            pduClone.commonHeader = commonHeader;
            pduClone.shareControlHeader = shareControlHeader;
            pduClone.pad = pad;
            pduClone.pad1Octet = RdpbcgrUtility.CloneByteArray(pad1Octet);
            pduClone.serverRedirectionPdu = serverRedirectionPdu.Clone();

            return pduClone;
        }

        public override byte[] ToBytes()
        {
            List<byte> dataBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(dataBuffer, shareControlHeader);
            RdpbcgrEncoder.EncodeStructure(dataBuffer, pad);
            RdpbcgrEncoder.EncodeBytes(dataBuffer, serverRedirectionPdu.Encode());
            RdpbcgrEncoder.EncodeBytes(dataBuffer, pad1Octet);

            byte[] byteBuffer = RdpbcgrUtility.ToBytes(dataBuffer);

            List<byte> totalBuffer = new List<byte>();
            RdpbcgrEncoder.EncodeSlowPathPdu(totalBuffer, commonHeader, byteBuffer, serverSessionContext);

            byte[] encodedBytes = RdpbcgrUtility.ToBytes(totalBuffer);

            // ToDo: Ugly dump message code here
            // ETW Provider Dump Code
            bool isEncrypted = serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_NONE && serverSessionContext.RdpEncryptionLevel != EncryptionLevel.ENCRYPTION_LEVEL_LOW;
            RdpbcgrUtility.ETWProviderDump(this.GetType().Name, encodedBytes, isEncrypted, byteBuffer);

            return encodedBytes;
        }
    }

    public enum RDP_SERVER_REDIRECTION_PACKET_FlagsEnum : ushort
    {
        SEC_REDIRECTION_PKT = 0x0400
    }

    public enum CERTIFICATE_META_ELEMENT_TypeEnum : UInt32
    {
        ELEMENT_TYPE_CERTIFICATE = 32
    }

    public enum CERTIFICATE_META_ELEMENT_EncodingEnum : UInt32
    {
        ENCODING_TYPE_ASN1_DER = 1
    }

    /// <summary>
    /// Certificate Meta Element (CERTIFICATE_META_ELEMENT)
    /// The CERTIFICATE_META_ELEMENT structure specifies an element contained within a Target Certificate Container structure.
    /// </summary>
    public class CERTIFICATE_META_ELEMENT
    {
        /// <summary>
        /// type (4 bytes): A 32-bit, unsigned integer specifying the type of the data in the elementData field.
        /// All values SHOULD be ignored except for ELEMENT_TYPE_CERTIFICATE (32), which indicates that the element is an X.509 certificate.
        /// </summary>
        public UInt32 type;

        /// <summary>
        /// encoding (4 bytes): A 32-bit, unsigned integer specifying the encoding used to serialize the data in the elementData field.
        /// All values SHOULD be ignored except for ENCODING_TYPE_ASN1_DER (1), which indicates that the element is encoded using the ASN.1 DER scheme.
        /// </summary>
        public UInt32 encoding;

        /// <summary>
        /// elementSize (4 bytes): A 32-bit, unsigned integer specifying the size, in bytes, of the elementData field.
        /// </summary>
        public UInt32 elementSize;

        /// <summary>
        /// elementData (variable): A variable-length array of bytes containing the certificate meta element data.
        /// </summary>
        public byte[] elementData;

        public byte[] Encode()
        {
            var result = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(result, type);

            RdpbcgrEncoder.EncodeStructure(result, encoding);

            RdpbcgrEncoder.EncodeStructure(result, elementSize);

            RdpbcgrEncoder.EncodeBytes(result, elementData);

            return result.ToArray();
        }

        public CERTIFICATE_META_ELEMENT Clone()
        {
            var result = new CERTIFICATE_META_ELEMENT();

            result.type = type;

            result.encoding = encoding;

            result.elementSize = elementSize;

            result.elementData = RdpbcgrUtility.CloneByteArray(elementData);

            return result;
        }
    }

    /// <summary>
    /// Target Certificate Container (TARGET_CERTIFICATE_CONTAINER)
    /// The TARGET_CERTIFICATE_CONTAINER structure is used to wrap an X.509 certificate.
    /// It contains an array of Certificate Meta Element structures.
    /// The element of type ELEMENT_TYPE_CERTIFICATE(32) and encoding ENCODING_TYPE_ASN1_DER(1) contains the X.509 certificate.
    /// </summary>
    public class TARGET_CERTIFICATE_CONTAINER
    {
        /// <summary>
        /// elements (variable): An array of Certificate Meta Element structures.
        /// All elements in this array SHOULD be ignored, except for the element of type ELEMENT_TYPE_CERTIFICATE (32) and encoding ENCODING_TYPE_ASN1_DER (1).
        /// </summary>
        public CERTIFICATE_META_ELEMENT[] elements;

        public byte[] Encode()
        {
            var result = elements
                            .Select(element => element.Encode().AsEnumerable())
                            .Aggregate((a, b) => a.Concat(b))
                            .ToArray();

            return result;
        }

        public TARGET_CERTIFICATE_CONTAINER Clone()
        {
            var result = new TARGET_CERTIFICATE_CONTAINER();

            result.elements = elements.Select(element => element.Clone()).ToArray();

            return result;
        }
    }

    /// <summary>
    /// 2.2.13.1	Server Redirection Packet (RDP_SERVER_REDIRECTION_PACKET)
    /// The RDP_SERVER_REDIRECTION_PACKET structure contains information to enable a client 
    /// to reconnect to a session on a specified server.This data is sent to a client in a 
    /// Redirection PDU to enable load-balancing of Remote Desktop sessions across a collection 
    /// of machines. For more information about the load balancing of Remote Desktop sessions, 
    /// see[MSFT - SDLBTS] "Load-Balanced Configurations" and "Revectoring Clients".
    /// </summary>
    public class RDP_SERVER_REDIRECTION_PACKET
    {
        /// <summary>
        /// Flags (2 bytes): A 16-bit unsigned integer. 
        /// The server redirection identifier. 
        /// This field MUST be set to SEC_REDIRECTION_PKT (0x0400).
        /// </summary>
        public RDP_SERVER_REDIRECTION_PACKET_FlagsEnum Flags;

        /// <summary>
        /// Length (2 bytes): A 16-bit unsigned integer. 
        /// The overall length, in bytes, of the Server Redirection Packet structure.
        /// </summary>
        public UInt16 Length;

        /// <summary>
        /// SessionID (4 bytes): A 32-bit unsigned integer. 
        /// The session identifier to which the client MUST reconnect. 
        /// This identifier MUST be specified in the RedirectedSessionID field of the 
        /// Client Cluster Data (section 2.2.1.3.5) if a reconnect attempt takes place. 
        /// The Client Cluster Data is transmitted as part of the MCS Connect Initial PDU (section 2.2.1.3).
        /// </summary>
        public UInt32 SessionID;

        /// <summary>
        /// RedirFlags (4 bytes): A 32-bit unsigned integer. A bit field that contains redirection information flags, 
        /// some of which indicate the presence of additional data at the end of the packet.
        /// </summary>
        public RedirectionFlags RedirFlags;

        /// <summary>
        /// TargetNetAddressLength (4 bytes): A 32-bit unsigned integer. 
        /// The length, in bytes, of the TargetNetAddress field.
        /// </summary>
        public UInt32 TargetNetAddressLength;

        /// <summary>
        /// TargetNetAddress (variable): A variable-length array of bytes containing the IP address of the server 
        /// (for example, "192.168.0.1" using dotted decimal notation) in Unicode format, including a null-terminator.
        /// </summary>
        public string TargetNetAddress;

        /// <summary>
        /// LoadBalanceInfoLength (4 bytes): A 32-bit unsigned integer. The length, in bytes, of the LoadBalanceInfo field.
        /// </summary>
        public UInt32 LoadBalanceInfoLength;

        /// <summary>
        /// LoadBalanceInfo (variable): A variable-length array of bytes containing load balancing information 
        /// that MUST be treated as opaque data by the client and passed to the server if the LB_TARGET_NET_ADDRESS (0x00000001) 
        /// flag is not present in the RedirFlags field and a reconnection takes place. See section 3.2.5.3.1 for details 
        /// on populating the routingToken field of the X.224 Connection Request PDU (section 2.2.1.1).
        /// </summary>
        public byte[] LoadBalanceInfo;

        /// <summary>
        /// UserNameLength (4 bytes): A 32-bit unsigned integer. The length, in bytes, of the UserName field.
        /// </summary>
        public UInt32 UserNameLength;

        /// <summary>
        /// UserName (variable): A variable-length array of bytes containing the username of the user in Unicode format, including a null-terminator.
        /// </summary>
        public string UserName;

        /// <summary>
        /// DomainLength (4 bytes): A 32-bit unsigned integer. The length, in bytes, of the Domain field.
        /// </summary>
        public UInt32 DomainLength;

        /// <summary>
        /// Domain (variable): A variable-length array of bytes containing the domain to which the user connected in Unicode format, including a null-terminator.
        /// </summary>
        public string Domain;

        /// <summary>
        /// PasswordLength (4 bytes): A 32-bit unsigned integer. The length, in bytes, of the Password field.
        /// </summary>
        public UInt32 PasswordLength;

        /// <summary>
        /// Password (variable): A variable-length array of bytes containing the password used by the user in Unicode format, 
        /// including a null-terminator, or a cookie value that MUST be passed to the target server on successful connection.
        /// </summary>
        public byte[] Password;

        /// <summary>
        /// TargetFQDNLength (4 bytes): A 32-bit unsigned integer. The length, in bytes, of the TargetFQDN field.
        /// </summary>
        public UInt32 TargetFQDNLength;

        /// <summary>
        /// TargetFQDN (variable): A variable-length array of bytes containing the fully qualified domain name (FQDN) of the target machine, including a null-terminator.
        /// </summary>
        public string TargetFQDN;

        /// <summary>
        /// TargetNetBiosNameLength (4 bytes): A 32-bit unsigned integer. The length, in bytes, of the TargetNetBiosName field.
        /// </summary>
        public UInt32 TargetNetBiosNameLength;

        /// <summary>
        /// TargetNetBiosName (variable): A variable-length array of bytes containing the NETBIOS name of the target machine, including a null-terminator.
        /// </summary>
        public string TargetNetBiosName;

        /// <summary>
        /// TsvUrlLength (4 bytes): The length, in bytes, of the TsvUrl field.
        /// </summary>
        public UInt32 TsvUrlLength;

        /// <summary>
        /// TsvUrl (variable): A variable-length array of bytes.
        /// If the client has previously sent a TsvUrl field in the LoadBalanceInfo to the server in the expected format, 
        /// then the server will return the same TsvUrl to the client in this field. The client verifies that it is the 
        /// same as the one that it previously passed to the server and if they don't match, the client immediately disconnects the connection.
        /// </summary>
        public byte[] TsvUrl;

        /// <summary>
        /// RedirectionGuidLength (4 bytes): A 32-bit unsigned integer. The length, in bytes, of the RedirectionGuid field.
        /// </summary>
        public UInt32 RedirectionGuidLength;

        /// <summary>
        /// RedirectionGuid (variable): A variable-length array of bytes containing a GUID ([MS-DTYP] section 2.3.2) that functions as a unique identifier for the redirected connection.
        /// </summary>
        public byte[] RedirectionGuid;

        /// <summary>
        /// TargetCertificateLength (4 bytes): A 32-bit unsigned integer. The length, in bytes, of the TargetCertificate field.
        /// </summary>
        public UInt32 TargetCertificateLength;

        /// <summary>
        /// TargetCertificate (variable): A variable-length array of bytes containing a Base64-encoded Target Certificate Container structure in Unicode format that encapsulates the X.509 certificate of the target server.
        /// </summary>
        public byte[] TargetCertificate;

        /// <summary>
        /// TargetNetAddressesLength (4 bytes): A 32-bit unsigned integer. The length, in bytes, of the TargetNetAddresses field.
        /// </summary>
        public UInt32 TargetNetAddressesLength;

        /// <summary>
        /// TargetNetAddresses (variable): A variable-length array of bytes containing the target IP addresses of the server to connect against, 
        /// stored in a Target Net Addresses structure (section 2.2.13.1.1).
        /// </summary>
        public TARGET_NET_ADDRESSES TargetNetAddresses;

        /// <summary>
        /// Pad (8 bytes): An optional 8-element array of 8-bit unsigned integers. Padding. Values in this field MUST be ignored.
        /// </summary>
        public byte[] Pad;

        public byte[] Encode()
        {
            // pack all
            var result = new List<byte>();

            RdpbcgrEncoder.EncodeStructure(result, (ushort)Flags);
            RdpbcgrEncoder.EncodeStructure(result, Length);
            RdpbcgrEncoder.EncodeStructure(result, SessionID);
            RdpbcgrEncoder.EncodeStructure(result, (uint)RedirFlags);

            // optional target net address
            if (RedirFlags.HasFlag(RedirectionFlags.LB_TARGET_NET_ADDRESS))
            {
                RdpbcgrEncoder.EncodeStructure(result, TargetNetAddressLength);
                RdpbcgrEncoder.EncodeUnicodeString(result, TargetNetAddress, TargetNetAddressLength);
            }

            // optional load balance info
            if (RedirFlags.HasFlag(RedirectionFlags.LB_LOAD_BALANCE_INFO))
            {
                RdpbcgrEncoder.EncodeStructure(result, LoadBalanceInfoLength);
                RdpbcgrEncoder.EncodeBytes(result, LoadBalanceInfo);
            }

            // optional user name
            if (RedirFlags.HasFlag(RedirectionFlags.LB_USERNAME))
            {
                RdpbcgrEncoder.EncodeStructure(result, UserNameLength);
                RdpbcgrEncoder.EncodeUnicodeString(result, UserName, UserNameLength);
            }

            // optional domain
            if (RedirFlags.HasFlag(RedirectionFlags.LB_DOMAIN))
            {
                RdpbcgrEncoder.EncodeStructure(result, DomainLength);
                RdpbcgrEncoder.EncodeUnicodeString(result, Domain, DomainLength);
            }

            // optional password
            if (RedirFlags.HasFlag(RedirectionFlags.LB_PASSWORD))
            {
                RdpbcgrEncoder.EncodeStructure(result, PasswordLength);
                RdpbcgrEncoder.EncodeBytes(result, Password);
            }

            // optional target FQDN
            if (RedirFlags.HasFlag(RedirectionFlags.LB_TARGET_FQDN))
            {
                RdpbcgrEncoder.EncodeStructure(result, TargetFQDNLength);
                RdpbcgrEncoder.EncodeUnicodeString(result, TargetFQDN, TargetFQDNLength);
            }

            // optional target netbios name
            if (RedirFlags.HasFlag(RedirectionFlags.LB_TARGET_NETBIOS_NAME))
            {
                RdpbcgrEncoder.EncodeStructure(result, TargetNetBiosNameLength);
                RdpbcgrEncoder.EncodeUnicodeString(result, TargetNetBiosName, TargetNetBiosNameLength);
            }

            // optional tsv URL
            if (RedirFlags.HasFlag(RedirectionFlags.LB_CLIENT_TSV_URL))
            {
                RdpbcgrEncoder.EncodeStructure(result, TsvUrlLength);
                RdpbcgrEncoder.EncodeBytes(result, TsvUrl);
            }

            // optional redirection GUID
            if (RedirFlags.HasFlag(RedirectionFlags.LB_REDIRECTION_GUID))
            {
                RdpbcgrEncoder.EncodeStructure(result, RedirectionGuidLength);
                RdpbcgrEncoder.EncodeBytes(result, RedirectionGuid);
            }

            // optional target certificate
            if (RedirFlags.HasFlag(RedirectionFlags.LB_TARGET_CERTIFICATE))
            {
                RdpbcgrEncoder.EncodeStructure(result, TargetCertificateLength);
                RdpbcgrEncoder.EncodeBytes(result, TargetCertificate);
            }

            // optional target net addresses
            if (RedirFlags.HasFlag(RedirectionFlags.LB_TARGET_NET_ADDRESSES))
            {
                RdpbcgrEncoder.EncodeStructure(result, TargetNetAddressesLength);
                RdpbcgrEncoder.EncodeBytes(result, TargetNetAddresses.Encode());
            }

            // optional pad
            if (Pad != null)
            {
                RdpbcgrEncoder.EncodeBytes(result, Pad);
            }

            return result.ToArray();
        }

        public RDP_SERVER_REDIRECTION_PACKET Clone()
        {
            var result = new RDP_SERVER_REDIRECTION_PACKET();

            result.Flags = Flags;
            result.Length = Length;
            result.SessionID = SessionID;
            result.RedirFlags = RedirFlags;

            // optional target net address
            if (RedirFlags.HasFlag(RedirectionFlags.LB_TARGET_NET_ADDRESS))
            {
                result.TargetNetAddressLength = TargetNetAddressLength;
                result.TargetNetAddress = RdpbcgrUtility.CloneString(TargetNetAddress);
            }

            // optional load balance info
            if (RedirFlags.HasFlag(RedirectionFlags.LB_LOAD_BALANCE_INFO))
            {
                result.LoadBalanceInfoLength = LoadBalanceInfoLength;
                result.LoadBalanceInfo = RdpbcgrUtility.CloneByteArray(LoadBalanceInfo);
            }

            // optional user name
            if (RedirFlags.HasFlag(RedirectionFlags.LB_USERNAME))
            {
                result.UserNameLength = UserNameLength;
                result.UserName = RdpbcgrUtility.CloneString(UserName);
            }

            // optional domain
            if (RedirFlags.HasFlag(RedirectionFlags.LB_DOMAIN))
            {
                result.DomainLength = DomainLength;
                result.Domain = RdpbcgrUtility.CloneString(Domain);
            }

            // optional password
            if (RedirFlags.HasFlag(RedirectionFlags.LB_PASSWORD))
            {
                result.PasswordLength = PasswordLength;
                result.Password = RdpbcgrUtility.CloneByteArray(Password);
            }

            // optional target FQDN
            if (RedirFlags.HasFlag(RedirectionFlags.LB_TARGET_FQDN))
            {
                result.TargetFQDNLength = TargetFQDNLength;
                result.TargetFQDN = RdpbcgrUtility.CloneString(TargetFQDN);
            }

            // optional target netbios name
            if (RedirFlags.HasFlag(RedirectionFlags.LB_TARGET_NETBIOS_NAME))
            {
                result.TargetNetBiosNameLength = TargetNetBiosNameLength;
                result.TargetNetBiosName = RdpbcgrUtility.CloneString(TargetNetBiosName);
            }

            // optional tsv URL
            if (RedirFlags.HasFlag(RedirectionFlags.LB_CLIENT_TSV_URL))
            {
                result.TsvUrlLength = TsvUrlLength;
                result.TsvUrl = RdpbcgrUtility.CloneByteArray(TsvUrl);
            }

            // optional redirection GUID
            if (RedirFlags.HasFlag(RedirectionFlags.LB_REDIRECTION_GUID))
            {
                result.RedirectionGuidLength = RedirectionGuidLength;
                result.RedirectionGuid = RdpbcgrUtility.CloneByteArray(RedirectionGuid);
            }

            // optional target certificate
            if (RedirFlags.HasFlag(RedirectionFlags.LB_TARGET_CERTIFICATE))
            {
                result.TargetCertificateLength = TargetCertificateLength;
                result.TargetCertificate = RdpbcgrUtility.CloneByteArray(TargetCertificate);
            }

            // optional target net addresses
            if (RedirFlags.HasFlag(RedirectionFlags.LB_TARGET_NET_ADDRESSES))
            {
                result.TargetNetAddressesLength = TargetNetAddressesLength;
                result.TargetNetAddresses = TargetNetAddresses.Clone();
            }

            return result;
        }

        public void UpdateLength()
        {
            // Flags, Length, SessionID and RedirFlags
            uint length = 12;

            // optional target net address
            if (RedirFlags.HasFlag(RedirectionFlags.LB_TARGET_NET_ADDRESS))
            {
                TargetNetAddressLength = RdpbcgrEncoder.CalculateUnicodeStringEncodingSize(TargetNetAddress, true);
                length += (4 + TargetNetAddressLength);
            }

            // optional load balance info
            if (RedirFlags.HasFlag(RedirectionFlags.LB_LOAD_BALANCE_INFO))
            {
                LoadBalanceInfoLength = (uint)LoadBalanceInfo.Length;
                length += (4 + LoadBalanceInfoLength);
            }

            // optional user name
            if (RedirFlags.HasFlag(RedirectionFlags.LB_USERNAME))
            {
                UserNameLength = RdpbcgrEncoder.CalculateUnicodeStringEncodingSize(UserName, true);
                length += (4 + UserNameLength);
            }

            // optional domain
            if (RedirFlags.HasFlag(RedirectionFlags.LB_DOMAIN))
            {
                DomainLength = RdpbcgrEncoder.CalculateUnicodeStringEncodingSize(Domain, true);
                length += (4 + DomainLength);
            }

            // optional password
            if (RedirFlags.HasFlag(RedirectionFlags.LB_PASSWORD))
            {
                PasswordLength = (uint)Password.Length;
                length += (4 + PasswordLength);
            }

            // optional target FQDN
            if (RedirFlags.HasFlag(RedirectionFlags.LB_TARGET_FQDN))
            {
                TargetFQDNLength = RdpbcgrEncoder.CalculateUnicodeStringEncodingSize(TargetFQDN, true);
                length += (4 + TargetFQDNLength);
            }

            // optional target netbios name
            if (RedirFlags.HasFlag(RedirectionFlags.LB_TARGET_NETBIOS_NAME))
            {
                TargetNetBiosNameLength = RdpbcgrEncoder.CalculateUnicodeStringEncodingSize(TargetNetBiosName, true);
                length += (4 + TargetNetBiosNameLength);
            }

            // optional tsv URL
            if (RedirFlags.HasFlag(RedirectionFlags.LB_CLIENT_TSV_URL))
            {
                TsvUrlLength = (uint)TsvUrl.Length;
                length += (4 + TsvUrlLength);
            }

            // optional redirection GUID
            if (RedirFlags.HasFlag(RedirectionFlags.LB_REDIRECTION_GUID))
            {
                RedirectionGuidLength = (uint)RedirectionGuid.Length;
                length += (4 + RedirectionGuidLength);
            }

            // optional target certificate
            if (RedirFlags.HasFlag(RedirectionFlags.LB_TARGET_CERTIFICATE))
            {
                TargetCertificateLength = (uint)TargetCertificate.Length;
                length += (4 + TargetCertificateLength);
            }

            // optional target net addresses
            if (RedirFlags.HasFlag(RedirectionFlags.LB_TARGET_NET_ADDRESSES))
            {
                TargetNetAddressesLength = 4;
                TargetNetAddresses.UpdateCount();
                foreach (var item in TargetNetAddresses.address)
                {
                    item.UpdateLength();
                    TargetNetAddressesLength += (4 + item.addressLength);
                }
                length += (4 + TargetNetAddressesLength);
            }

            // optional pad
            if (Pad != null)
            {
                length += (uint)Pad.Length;
            }

            if (length > ushort.MaxValue)
            {
                throw new FormatException("Maximum size exceeded!");
            }

            Length = (ushort)length;
        }
    }

    /// <summary>
    /// Redirection information flags.
    /// </summary>
    [Flags()]
    public enum RedirectionFlags : uint
    {
        /// <summary>
        /// Indicates that the TargetNetAddressLength and TargetNetAddress fields are present.
        /// </summary>
        LB_TARGET_NET_ADDRESS = 0x00000001,

        /// <summary>
        /// Indicates that the LoadBalanceInfoLength and LoadBalanceInfo fields are present.
        /// </summary>
        LB_LOAD_BALANCE_INFO = 0x00000002,

        /// <summary>
        /// Indicates that the UserNameLength and UserName fields are present.
        /// </summary>
        LB_USERNAME = 0x00000004,

        /// <summary>
        /// Indicates that the DomainLength and Domain fields are present.
        /// </summary>
        LB_DOMAIN = 0x00000008,

        /// <summary>
        /// Indicates that the PasswordLength and Password fields are present.
        /// </summary>
        LB_PASSWORD = 0x00000010,

        /// <summary>
        /// The client MUST not store the username in any internal data structures.
        /// </summary>
        LB_DONTSTOREUSERNAME = 0x00000020,

        /// <summary>
        /// Indicates that the user can use a smart card for authentication.
        /// </summary>
        LB_SMARTCARD_LOGON = 0x00000040,

        /// <summary>
        /// Indicates that the contents of the PDU are for informational purposes only.
        /// No actual redirection is required.
        /// </summary>
        LB_NOREDIRECT = 0x00000080,

        /// <summary>
        /// Indicates that the TargetFQDNLength and TargetFQDN fields are present.
        /// </summary>
        LB_TARGET_FQDN = 0x00000100,

        /// <summary>
        /// Indicates that the TargetNetBiosNameLength and TargetNetBiosName fields are present.
        /// </summary>
        LB_TARGET_NETBIOS_NAME = 0x00000200,

        /// <summary>
        /// Indicates that the TargetNetAddressesLength and TargetNetAddresses fields are present.
        /// </summary>
        LB_TARGET_NET_ADDRESSES = 0x00000800,

        /// <summary>
        /// Indicates that the TsvUrlLength and TsvUrl fields are present.
        /// </summary>
        LB_CLIENT_TSV_URL = 0x00001000,

        /// <summary>
        /// Indicates that the server supports redirection based on the TsvUrl present in the LoadBalanceInfo sent by the client
        /// </summary>
        LB_SERVER_TSV_CAPABLE = 0x00002000,

        /// <summary>
        /// Indicates that the data in the Password field is encrypted and contains data 
        /// that SHOULD be used in the RDS Authentication Request PDU with Password Credentials (section 2.2.17.2).
        /// </summary>
        LB_PASSWORD_IS_PK_ENCRYPTED = 0x00004000,

        /// <summary>
        /// Indicates that the RedirectionGuidLength and RedirectionGuid fields are present.
        /// </summary>
        LB_REDIRECTION_GUID = 0x00008000,

        /// <summary>
        /// Indicates that the TargetCertificateLength and TargetCertificate fields are present.
        /// </summary>
        LB_TARGET_CERTIFICATE = 0x00010000
    }

    /// <summary>
    /// The Target Net Address structure holds a Unicode text representation of an IP address.
    /// </summary>
    public class TARGET_NET_ADDRESS
    {
        /// <summary>
        /// A 32-bit, unsigned integer. The length in bytes of the address field.
        /// </summary>
        public uint addressLength;

        /// <summary>
        /// An array of bytes containing an IP address in Unicode format, including a null-terminator.
        /// </summary>
        public string address;

        public byte[] Encode()
        {
            var result = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(result, addressLength);
            RdpbcgrEncoder.EncodeUnicodeString(result, address, addressLength);
            return result.ToArray();
        }

        public TARGET_NET_ADDRESS Clone()
        {
            var result = new TARGET_NET_ADDRESS();
            result.addressLength = addressLength;
            result.address = RdpbcgrUtility.CloneString(address);
            return result;
        }

        public void UpdateLength()
        {
            addressLength = RdpbcgrEncoder.CalculateUnicodeStringEncodingSize(address, true);
        }
    }

    /// <summary>
    /// The TARGET_NET_ADDRESSES structure is used to hold a collection of IP addresses in Unicode format.
    /// </summary>
    public class TARGET_NET_ADDRESSES
    {
        /// <summary>
        /// A 32-bit, unsigned integer. The number of IP addresses present in the address field.
        /// </summary>
        public uint addressCount;

        /// <summary>
        /// An array of Target Net Address (section 2.2.13.1.1.1) structures, each containing an IP address.
        /// </summary>
        public TARGET_NET_ADDRESS[] address;

        public byte[] Encode()
        {
            var result = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(result, addressCount);
            foreach (var item in address)
            {
                RdpbcgrEncoder.EncodeBytes(result, item.Encode());
            }
            return result.ToArray();
        }

        public TARGET_NET_ADDRESSES Clone()
        {
            var result = new TARGET_NET_ADDRESSES();
            result.addressCount = addressCount;
            var addressList = new List<TARGET_NET_ADDRESS>();
            foreach (var item in address)
            {
                addressList.Add(item.Clone());
            }
            result.address = addressList.ToArray();
            return result;
        }

        public void UpdateCount()
        {
            addressCount = (uint)address.Length;
        }
    }

    /// <summary>
    ///  Indicates a Fast-Path Orders Update (see [MS-RDPEGDI] section 2.2.2.3).
    /// </summary>
    public class TS_FP_UPDATE_ORDERS : TS_FP_UPDATE
    {
        /// <summary>
        ///  see [MS-RDPEGDI] section 2.2.2.3
        /// </summary>
        public byte[] updateOrders;
    }

    #region Temporary Enum need Future Redefinition
    /// <summary>
    ///  X224 PDU type.
    /// </summary>
    public enum X224_TPDU_TYPE : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        ConnectionRequest = 0xe0,

        /// <summary>
        /// CC TPDU
        /// </summary>
        ConnectionConfirm = 0xd0,

        /// <summary>
        /// Data TPDU
        /// </summary>
        Data = 0xf0,
    }
    #endregion
    #endregion self-defined structures

    #region RDSTLS

    public enum RDSTLS_VersionEnum : ushort
    {
        RDSTLS_VERSION_1 = 0x0001
    }

    public enum RDSTLS_PduTypeEnum : ushort
    {
        RDSTLS_TYPE_CAPABILITIES = 0x0001,
        RDSTLS_TYPE_AUTHREQ = 0x0002,

        RDSTLS_TYPE_AUTHRSP = 0x0004
    }

    public enum RDSTLS_DataTypeEnum : ushort
    {
        RDSTLS_DATA_CAPABILITIES = 0x0001,
        RDSTLS_DATA_PASSWORD_CREDS = 0x0001,
        RDSTLS_DATA_AUTORECONNECT_COOKIE = 0x0002,
        RDSTLS_DATA_RESULT_CODE = 0x0001
    }

    [StructLayout(LayoutKind.Explicit, Size = 6)]
    public class RDSTLS_CommonHeader
    {
        [FieldOffset(0)]
        public RDSTLS_VersionEnum Version;

        [FieldOffset(2)]
        public RDSTLS_PduTypeEnum PduType;

        [FieldOffset(4)]
        public RDSTLS_DataTypeEnum DataType;

        public RDSTLS_CommonHeader Clone()
        {
            var result = new RDSTLS_CommonHeader();
            result.Version = Version;
            result.PduType = PduType;
            result.DataType = DataType;
            return result;
        }
    }

    /// <summary>
    /// 2.2.17.1	RDSTLS Capabilities PDU
    /// The RDSTLS Capabilities PDU is sent by the server to the client and allows the server to advertise the supported RDSTLS versions.
    /// </summary>
    public class RDSTLS_CapabilitiesPDU : RdpbcgrServerPdu
    {
        /// <summary>
        /// Version (2 bytes): A 16-bit unsigned integer that specifies the RDSTLS version. 
        ///   This field MUST be set to RDSTLS_VERSION_1 (0x0001).
        ///   
        /// PduType(2 bytes): A 16-bit unsigned integer that specifies the RDSTLS PDU type.
        ///   This field MUST be set to RDSTLS_TYPE_CAPABILITIES(0x0001).
        ///   
        /// DataType(2 bytes): A 16-bit unsigned integer that specifies the type of data contained in the PDU.
        ///   This field MUST be set to RDSTLS_DATA_CAPABILITIES (0x0001).
        /// </summary>
        public RDSTLS_CommonHeader Header;

        /// <summary>
        /// SupportedVersions (2 bytes): A 16-bit unsigned integer that specifies the RDSTLS versions supported by the server. 
        /// This field MUST be set to RDSTLS_VERSION_1 (0x0001).
        /// </summary>
        public RDSTLS_VersionEnum SupportedVersions;

        public override StackPacket Clone()
        {
            var resultPdu = new RDSTLS_CapabilitiesPDU();
            resultPdu.Header = Header.Clone();
            resultPdu.SupportedVersions = SupportedVersions;
            return resultPdu;
        }

        public override byte[] ToBytes()
        {
            var result = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(result, Header);
            RdpbcgrEncoder.EncodeStructure(result, (ushort)SupportedVersions);
            return result.ToArray();
        }
    }

    /// <summary>
    /// 2.2.17.2	RDSTLS Authentication Request PDU with Password Credentials
    /// The RDSTLS Authentication Request PDU is sent by the client to the server and is used to request user 
    /// authentication using data acquired from the Server Redirection Packet(section 2.2.13.1).
    /// </summary>
    public class RDSTLS_AuthenticationRequestPDUwithPasswordCredentials : RdpbcgrClientPdu
    {
        /// <summary>
        /// Version (2 bytes): A 16-bit unsigned integer that specifies the RDSTLS version. 
        ///   This field MUST be set to RDSTLS_VERSION_1 (0x0001).
        /// 
        /// PduType(2 bytes): A 16-bit unsigned integer that specifies the RDSTLS PDU type.
        ///   This field MUST be set to RDSTLS_TYPE_AUTHREQ(0x0002).
        ///   
        /// DataType(2 bytes): A 16-bit unsigned integer that specifies the type of data contained in the PDU.
        ///   This field MUST be set to RDSTLS_DATA_PASSWORD_CREDS (0x0001).
        /// </summary>
        public RDSTLS_CommonHeader Header;

        /// <summary>
        /// RedirectionGuidLength (2 bytes): A 16-bit unsigned integer that specifies the length, in bytes, of the RedirectionGuid field.
        /// </summary>
        public UInt16 RedirectionGuidLength;

        /// <summary>
        /// RedirectionGuid (variable): A variable-length array of bytes containing a GUID 
        ///   (as defined in [MS-DTYP] section 2.3.2) that functions as a unique identifier for the current redirected 
        ///   connection. This value SHOULD be acquired from the RedirectionGuid field of the Server Redirection Packet (section 2.2.13.1).
        /// </summary>
        public byte[] RedirectionGuid;

        /// <summary>
        /// UserNameLength (2 bytes): A 16-bit unsigned integer that specifies the length, in bytes, of the UserName field.
        /// </summary>
        public UInt16 UserNameLength;

        /// <summary>
        /// UserName (variable): A variable-length array of bytes containing the username of the user in Unicode format, 
        ///   including a null-terminator. This value SHOULD be acquired from the UserName field of the Server Redirection Packet (section 2.2.13.1).
        /// </summary>
        public string UserName;

        /// <summary>
        /// DomainLength (2 bytes): A 16-bit unsigned integer that specifies the length, in bytes, of the Domain field.
        /// </summary>
        public UInt16 DomainLength;

        /// <summary>
        /// Domain (variable): A variable-length array of bytes containing the domain to which the user connected in Unicode format, 
        ///   including a null-terminator. This value SHOULD be acquired from the Domain field of the Server Redirection Packet (section 2.2.13.1).
        /// </summary>
        public string Domain;

        /// <summary>
        /// PasswordLength (2 bytes): A 16-bit unsigned integer that specifies the length, in bytes, of the Password field.
        /// </summary>
        public UInt16 PasswordLength;

        /// <summary>
        /// Password (variable): A variable-length array of bytes containing an encrypted password blob. 
        ///   This value SHOULD be acquired from the Password field of the Server Redirection Packet (section 2.2.13.1).
        /// </summary>
        public byte[] Password;

        public override StackPacket Clone()
        {
            var resultPdu = new RDSTLS_AuthenticationRequestPDUwithPasswordCredentials();
            resultPdu.Header = Header.Clone();
            resultPdu.RedirectionGuidLength = RedirectionGuidLength;
            resultPdu.RedirectionGuid = RedirectionGuid;
            resultPdu.UserNameLength = UserNameLength;
            resultPdu.UserName = String.Copy(UserName);
            resultPdu.DomainLength = DomainLength;
            resultPdu.Domain = String.Copy(Domain);
            resultPdu.PasswordLength = PasswordLength;
            resultPdu.Password = Password.ToArray();
            return resultPdu;
        }

        public override byte[] ToBytes()
        {
            var result = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(result, Header);
            RdpbcgrEncoder.EncodeStructure(result, RedirectionGuidLength);
            RdpbcgrEncoder.EncodeBytes(result, RedirectionGuid);
            RdpbcgrEncoder.EncodeStructure(result, UserNameLength);
            RdpbcgrEncoder.EncodeUnicodeString(result, UserName, UserNameLength);
            RdpbcgrEncoder.EncodeStructure(result, DomainLength);
            RdpbcgrEncoder.EncodeUnicodeString(result, Domain, DomainLength);
            RdpbcgrEncoder.EncodeStructure(result, PasswordLength);
            RdpbcgrEncoder.EncodeBytes(result, Password);
            return result.ToArray();
        }
    }

    /// <summary>
    /// 2.2.17.3	RDSTLS Authentication Request PDU with Auto-Reconnect Cookie
    /// The RDSTLS Authentication Request PDU is sent by the client to the server and is used to request user 
    /// authentication using an auto-reconnect cookie that was generated as specified in section 5.5.
    /// </summary>
    public class RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookie : RdpbcgrClientPdu
    {
        /// <summary>
        /// Version (2 bytes): A 16-bit unsigned integer that specifies the RDSTLS version. 
        ///   This field MUST be set to RDSTLS_VERSION_1 (0x0001).
        ///   
        /// PduType(2 bytes): A 16-bit unsigned integer that specifies the RDSTLS PDU type.
        ///   This field MUST be set to RDSTLS_TYPE_AUTHREQ(0x0002).
        ///   
        /// DataType(2 bytes): A 16-bit unsigned integer that specifies the type of data contained in the PDU.
        ///   This field MUST be set to RDSTLS_DATA_AUTORECONNECT_COOKIE (0x0002).
        /// </summary>
        public RDSTLS_CommonHeader Header;

        /// <summary>
        /// SessionID (4 bytes): A 32-bit unsigned integer that specifies the identifier of the session to which the client MUST be connected.
        /// </summary>
        public UInt32 SessionID;

        /// <summary>
        /// AutoReconnectCookieLength (2 bytes): A 16-bit unsigned integer that specifies the length, in bytes, of the AutoReconnectCookie field.
        /// </summary>
        public UInt16 AutoReconnectCookieLength;

        /// <summary>
        /// AutoReconnectCookie (variable): A variable-length array of bytes containing an auto-reconnect cookie that was generated as specified in section 5.5.
        /// </summary>
        public byte[] AutoReconnectCookie;

        public override StackPacket Clone()
        {
            var resultPdu = new RDSTLS_AuthenticationRequestPDUwithAutoReconnectCookie();
            resultPdu.Header = Header.Clone();
            resultPdu.SessionID = SessionID;
            resultPdu.AutoReconnectCookieLength = AutoReconnectCookieLength;
            resultPdu.AutoReconnectCookie = AutoReconnectCookie.ToArray();
            return resultPdu;
        }

        public override byte[] ToBytes()
        {
            var result = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(result, Header);
            RdpbcgrEncoder.EncodeStructure(result, SessionID);
            RdpbcgrEncoder.EncodeStructure(result, AutoReconnectCookieLength);
            RdpbcgrEncoder.EncodeBytes(result, AutoReconnectCookie);
            return result.ToArray();
        }
    }

    public enum RDSTLS_ResultCodeEnum : uint
    {
        /// <summary>
        /// User authentication succeeded.
        /// </summary>
        RDSTLS_RESULT_SUCCESS = 0x00000000,

        /// <summary>
        /// The user does not have permission to access the server.
        /// </summary>
        RDSTLS_RESULT_ACCESS_DENIED = 0x00000005,

        /// <summary>
        /// The username is unknown or the supplied password is incorrect.
        /// </summary>
        RDSTLS_RESULT_LOGON_FAILURE = 0x0000052e,

        /// <summary>
        /// The user account has time restrictions and cannot be accessed at this time.
        /// </summary>
        RDSTLS_RESULT_INVALID_LOGON_HOURS = 0x00000530,

        /// <summary>
        /// The password associated with the user account has expired.
        /// </summary>
        RDSTLS_RESULT_PASSWORD_EXPIRED = 0x00000532,

        /// <summary>
        /// The user account is currently disabled.
        /// </summary>
        RDSTLS_RESULT_ACCOUNT_DISABLED = 0x00000533,

        /// <summary>
        /// The password associated with the user account has to be changed.
        /// </summary>
        RDSTLS_RESULT_PASSWORD_MUST_CHANGE = 0x00000773,

        /// <summary>
        /// The user account is currently locked out and cannot be accessed.
        /// </summary>
        RDSTLS_RESULT_ACCOUNT_LOCKED_OUT = 0x00000775
    }

    /// <summary>
    /// 2.2.17.4	RDSTLS Authentication Response PDU
    /// The RDSTLS Authentication Response PDU is sent by the server to the client and is used to indicate the result of user authentication.
    /// </summary>
    public class RDSTLS_AuthenticationResponsePDU : RdpbcgrServerPdu
    {
        /// <summary>
        /// Version (2 bytes): A 16-bit unsigned integer that specifies the RDSTLS version. 
        ///   This field MUST be set to RDSTLS_VERSION_1 (0x0001).
        ///   
        /// PduType(2 bytes): A 16-bit unsigned integer that specifies the RDSTLS PDU type.
        ///   This field MUST be set to RDSTLS_TYPE_AUTHRSP(0x0004).
        ///   
        /// DataType(2 bytes): A 16-bit unsigned integer that specifies the type of data contained in the PDU.
        ///   This field MUST be set to RDSTLS_DATA_RESULT_CODE (0x0001).
        /// </summary>
        public RDSTLS_CommonHeader Header;

        /// <summary>
        /// ResultCode (4 bytes): A 16-bit unsigned integer that specifies the user authentication result.
        /// </summary>
        public RDSTLS_ResultCodeEnum ResultCode;

        public override StackPacket Clone()
        {
            var resultPdu = new RDSTLS_AuthenticationResponsePDU();
            resultPdu.Header = Header.Clone();
            resultPdu.ResultCode = ResultCode;
            return resultPdu;
        }

        public override byte[] ToBytes()
        {
            var result = new List<byte>();
            RdpbcgrEncoder.EncodeStructure(result, Header);
            RdpbcgrEncoder.EncodeStructure(result, (uint)ResultCode);
            return result.ToArray();
        }
    }

    #endregion
}
