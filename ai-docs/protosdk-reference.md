# ProtoSDK Reference

## What ProtoSDK Is

ProtoSDK (`ProtoSDK/`) is the protocol implementation library. It provides:
- Message/PDU data structures for each protocol
- Encode (serialize to bytes) and decode (deserialize from bytes) methods
- Client and server state machines
- Transport binding over TCP, NetBIOS, RDMA

ProtoSDK has **no dependency on any test framework**. It can be used independently. Test suites reference it as a project reference (within the same repo build) or as a NuGet package.

Root path: `c:\Users\jomitiran\source\repos\WindowsProtocolTestSuites\ProtoSDK\`

## Namespace Convention

All ProtoSDK code lives under `Microsoft.Protocols.TestTools.StackSdk.*`:

| Namespace | Location |
|---|---|
| `Microsoft.Protocols.TestTools.StackSdk` | `ProtoSDK/Common/` — base types |
| `Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2` | `ProtoSDK/MS-SMB2/` |
| `Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr` | `ProtoSDK/MS-RDPBCGR/` |
| `Microsoft.Protocols.TestTools.StackSdk.Security.Kerberos` | `ProtoSDK/KerberosLib/` |
| `Microsoft.Protocols.TestTools.StackSdk.FileAccessService` | `ProtoSDK/FileAccessService/` |
| `Microsoft.Protocols.TestTools.StackSdk.Transport` | `ProtoSDK/TransportStack/` |
| `Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd` | `ProtoSDK/MS-SMBD/` |
| `Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp` | `ProtoSDK/MS-WSP/` |

## Core Base Classes

### `StackPacket` (`ProtoSDK/Common/StackPacket.cs`)

Abstract base for all packets. Holds raw `byte[] PacketBytes`. Every protocol packet derives from this.

```csharp
public abstract class StackPacket
{
    public byte[] PacketBytes { get; }
    public abstract StackPacket Clone();
    public abstract byte[] ToBytes();
}
```

### `BasePDU` and `PduMarshaler` (`ProtoSDK/Common/PduMarshaler.cs`)

`BasePDU` is the abstract base for PDUs that use the marshaler-based encode/decode pattern:

```csharp
public abstract class BasePDU
{
    public abstract void Encode(PduMarshaler marshaler);
    public abstract bool Decode(PduMarshaler marshaler);
}
```

`PduMarshaler` wraps a `MemoryStream` and provides typed read/write helpers (`WriteUInt16`, `ReadBytes`, `WriteUnicodeString`, etc.), with endian control. This is the primary encoding mechanism for RDP, Kerberos, and other protocols.

### `TypeMarshal` (`ProtoSDK/Common/TypeMarshal.cs`)

A reflection-based marshaler that serializes C# structs decorated with `[StructLayout]` attributes directly to/from byte arrays. Used heavily in SMB2 where messages map to C# `struct` types mirroring the wire format.

## Per-Protocol Breakdown

### MS-SMB2 (`ProtoSDK/MS-SMB2/`)

**Namespace:** `Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2`

The most feature-rich protocol implementation in the SDK.

**Key files:**
- `Smb2.csproj` — project file
- `Smb2Decoder.cs` — packet decoder; parses raw bytes into typed packet objects
- `Smb2Consts.cs` — all protocol constants (command codes, flags, capabilities, etc.)
- `Smb2Utility.cs` — helper methods (e.g., `GetShareName`, `GetFileName`)
- `CustomTypes.cs` — enum and flag type definitions
- `DataTypes/` — C# struct definitions mirroring SMB2 wire structures
- `Packets/` — per-command packet classes (`Smb2NegotiateRequestPacket`, `Smb2CreateResponsePacket`, etc.)
- `Client/` — client-side state machine

**Client (`MS-SMB2/Client/`):**
- `Smb2Client.cs` — main client class; manages connections, sessions, tree connects
- `Smb2ClientConnection.cs` — per-connection state
- `Smb2ClientSession.cs` — per-session state (authentication context, session key)
- `Smb2ClientTreeConnect.cs` — per-tree-connect state
- `Smb2ClientOpen.cs` — per-file-open state
- `Smb2ClientTransport.cs` — TCP transport binding
- `ReceivedPackets` (in `Smb2Client.cs`) — maps MessageId to waiting response using `AutoResetEvent`

**Encoding pattern:** SMB2 messages are C# `struct` types with explicit field layout. `TypeMarshal.ToBytes()` serializes them. Responses are decoded by `Smb2Decoder.DecodePacket()` which reads the Command field from the header and dispatches to per-command deserializers.

**Compound requests:** `Smb2CompoundPacket` wraps multiple `Smb2SinglePacket` instances. The client handles MessageId registration for each embedded packet.

**IOCTL output-buffer control:** `Smb2ClientTransport` exposes `SendIoctlPayload(code, payload, maxOutputResponse)` and `ExpectIoctlPayload(out status, out payload, out outputCount)` (plus the DFSC-specific `SendDfscPayload(payload, isEX, maxOutputResponse)` / `ExpectDfscPayload(..., out outputCount)`). Reuse these — instead of the default 4096-byte `MaxOutputResponse` overloads — to exercise output-buffer boundaries such as `FSCTL_GET_DFS_REFERRALS` returning `STATUS_BUFFER_OVERFLOW` with `OutputCount == 0` when the referral buffer is too small (MS-SMB2 3.3.5.15.2, Appendix A note 384). `DfscClient` surfaces the same capability via `SendAndRecieveDFSCReferralMessages(out status, out outputCount, timeout, maxOutputResponse, ...)`.

### MS-RDPBCGR (`ProtoSDK/MS-RDPBCGR/`)

**Namespace:** `Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr`

The base RDP connectivity protocol. Other RDP protocols (RDPEGFX, RDPEDYC, etc.) layer on top.

**Key files:**
- `RdpbcgrClient.cs` — full RDP client, handles TLS/CredSSP negotiation, MCS, GCC, capabilities exchange
- `RdpbcgrClientDecoder.cs` — incoming PDU decoder
- `RdpbcgrClientContext.cs` — tracks connection state (server capabilities, session keys, etc.)
- `RdpbcgrEncoder.cs` — outgoing PDU encoder
- `Types.cs` — PDU structure definitions
- `ConstValue.cs` — protocol constants

**Encoding pattern:** RDP uses `BasePDU` / `PduMarshaler`. Each PDU class implements `Encode(PduMarshaler)` and `Decode(PduMarshaler)`. The decoder reads the PDU type indicator and dispatches.

**Virtual channels:** `StaticVirtualChannelManager.cs` manages static virtual channels. `ClientStaticVirtualChannel.cs` represents a single channel. Extensions like RDPEDYC multiplex dynamic virtual channels over static ones.

### KerberosLib (`ProtoSDK/KerberosLib/`)

**Namespace:** `Microsoft.Protocols.TestTools.StackSdk.Security.Kerberos`

**Key files:**
- `KerberosConnection.cs` — manages the Kerberos exchange with a KDC (AS-REQ/AS-REP, TGS-REQ/TGS-REP, AP-REQ/AP-REP)
- `KerberosContext.cs` — per-connection context (encryption keys, tickets, etc.)
- `KerberosUtility.cs` — helpers (encryption, checksum, ticket parsing)
- `Asn1Code/` — ASN.1 message classes generated from the Kerberos ASN.1 schema (RFC 4120 + MS extensions)
- `Types/` — type definitions

**Encoding pattern:** Kerberos messages are ASN.1 DER encoded. The `Asn1Base/` library provides ASN.1 encode/decode. Kerberos PDUs derive from `Asn1Sequence`, `Asn1Choice`, etc.

### MS-SMBD (`ProtoSDK/MS-SMBD/`)

**Namespace:** `Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd`

**Key files:**
- `SmbdClient.cs` — SMBD client; wraps an RDMA connection
- `SmbdConnection.cs` — connection state and framing
- `SmbdConnectionEndpoint.cs` — RDMA endpoint abstraction
- `SmbdMessages.cs` — SMBD message structures
- `SmbdTypes.cs` — type and constant definitions

**RDMA adapter:**
- `ProtoSDK/RDMA/` — C++/CLI wrapper around the NetworkDirect DDK (`ndspi.h`) for Windows
- `ProtoSDK/RdmaLinux/` — C# Linux RDMA adapter (added recently for Linux interop testing)

### MS-WSP (`ProtoSDK/MS-WSP/`)

**Namespace:** `Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp`

**Key files:**
- `Client/` — WSP client state machine
- `Messages/` — per-message PDU classes
- `Structures/` — shared structure types
- `WspBuffer.cs` — buffer helper

### MS-XCA (`ProtoSDK/MS-XCA/`)

**Namespace:** `Microsoft.Protocols.TestTools.StackSdk.Compression.Xca`

Pure compression/decompression implementation. No network transport involved.

**Key files:**
- `Xca.cs` — main entry point; dispatches to algorithm variants
- `LZ77Huffman/` — LZ77+Huffman algorithm
- `PlainLZ77/` — Plain LZ77 algorithm
- `LZNT1/` — LZNT1 algorithm

### TransportStack (`ProtoSDK/TransportStack/`)

**Namespace:** `Microsoft.Protocols.TestTools.StackSdk.Transport`

The common transport abstraction. Provides:
- `TransportStack.cs` — core stack managing send/receive threads
- `Socket/` — TCP socket implementation
- `Stream/` — stream-based transport (used for SSL/TLS)
- `Netbios/` — NetBIOS over TCP transport
- `DecodePacketCallback` — delegate for protocol-specific packet framing
- `TransportConfig` / `StreamConfig` — configuration types

Protocols call `TransportStack.AddTransport(config)` and then `ExpectPacket()` / `SendPacket()`.

### Common (`ProtoSDK/Common/`)

Shared utilities used across all protocols:

| File | Purpose |
|---|---|
| `NtStatus.cs` | NTSTATUS code enum |
| `Win32ErrorCode_32.cs` | Win32 error codes |
| `EndianUtility.cs` | Big/little endian conversion helpers |
| `TypeMarshal.cs` | Reflection-based struct marshaler |
| `PduMarshaler.cs` | Stream-based PDU marshaler + `BasePDU` |
| `StackPacket.cs` | Abstract packet base class |
| `DtypUtility.cs` | DTYPE security descriptor utilities |
| `DtypSecurityStructures.cs` | SID, ACL, security descriptor structures |
| `Int3264.cs` | Platform-independent 32/64-bit integer |
| `Logging.cs` | Trace logging helpers |
| `Rpc/` | RPC transport and stub helpers |

## Instrumentation

ProtoSDK uses .NET `EventSource` for ETW/EventPipe tracing. There is one `EventSource` class per protocol, named with the `Microsoft-WindowsProtocolsTestSuite-<ProtocolName>` prefix. See `ProtoSDK/README.md` for the full convention.

To collect traces for a specific protocol:
```
dotnet-trace collect --providers Microsoft-WindowsProtocolsTestSuite-Kerberos --process-id <pid>
```

## Key Code Pattern: SMB2 Request/Response

```csharp
// 1. Create and configure the client
var client = new Smb2Client(timeout);
client.ConnectToServer(transport, serverName, serverIp);

// 2. Negotiate
uint status = client.Negotiate(
    dialects,
    securityMode,
    clientGuid,
    out DialectRevision selectedDialect,
    out byte[] serverGssToken,
    out Packet_Header responseHeader,
    out NEGOTIATE_Response negotiateResponse);

// 3. Session setup (Kerberos or NTLM via SSPI)
status = client.SessionSetup(
    Packet_Header_Flags_Values.NONE,
    SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
    SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
    0,
    clientGssToken,
    out serverGssToken,
    out SESSION_SETUP_Response sessionSetupResponse);

// 4. Tree connect, Create, Read/Write, Close, Tree Disconnect, Logoff
```

## Key Code Pattern: RDP PDU Decode

```csharp
// In RdpbcgrClientDecoder.cs
byte[] rawData = transport.ExpectBytes(timeout);
var pdu = DecodeServerPdu(rawData);
// pdu is cast to specific type: TS_SHARECONTROLHEADER, etc.
```

## Key Code Pattern: BasePDU Encode/Decode

```csharp
public class MyPdu : BasePDU
{
    public ushort Length;
    public byte[] Data;

    public override void Encode(PduMarshaler marshaler)
    {
        marshaler.WriteUInt16(Length);
        marshaler.WriteBytes(Data);
    }

    public override bool Decode(PduMarshaler marshaler)
    {
        Length = marshaler.ReadUInt16();
        Data = marshaler.ReadBytes(Length);
        return true;
    }
}
```
