# Protocol Specifications Reference

This document covers every MS-XXXX protocol referenced in the codebase. For each protocol: identifier, purpose (sourced from Microsoft Learn), codebase location, and key concepts for contributors.

The base URL for all specs: `https://learn.microsoft.com/en-us/openspecs/windows_protocols/`

---

## File Services Protocols

### [MS-SMB2] Server Message Block Protocol Versions 2 and 3

**Purpose:** Specifies the SMB2/SMB3 protocol, which supports sharing of file and print resources between machines. Extends the original SMB protocol with improved performance, security (signing, encryption), and features (leasing, multi-channel, compression, persistent handles).

**Current revision:** 85.0 (March 2026)

**ProtoSDK path:** `ProtoSDK/MS-SMB2/`
**Namespace:** `Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2`
**Test suite path:** `TestSuites/FileServer/src/SMB2/`

**Key concepts for contributors:**
- **Dialect negotiation:** Client offers a list of dialect revisions (2.002, 2.1, 3.0, 3.0.2, 3.1.1); server selects one. The highest shared dialect determines available features.
- **Session setup:** Multi-leg SPNEGO/Kerberos/NTLM exchange. Session key derived here is used for signing and encryption.
- **Tree connect:** Maps a UNC share path to a tree ID for the session. Share capabilities (encryption required, etc.) are returned here.
- **File operations:** CREATE, READ, WRITE, QUERY_INFO, SET_INFO, IOCTL, CLOSE, FLUSH, LOCK, CANCEL
- **Compound requests:** Multiple requests can be chained in one network packet (related or unrelated).
- **Signing:** HMAC-SHA256 (SMB 2.x) or AES-CMAC (SMB 3.x) of the packet using the session signing key.
- **Encryption:** AES-CCM (SMB 3.0/3.0.2) or AES-GCM (SMB 3.1.1) per session or per share.
- **Leasing:** Reduces round-trips by caching file metadata/data with server-granted leases.
- **Resilient/Durable handles:** Allows handles to survive temporary network outages.
- **Multi-channel:** Multiple TCP connections can carry traffic for a single session.
- **FSCTL operations:** File system control requests (e.g., FSCTL_GET_DFSREFERRALS, FSCTL_VALIDATE_NEGOTIATE_INFO, FSCTL_LMR_REQUEST_RESILIENCY).

**Spec link:** https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-smb2/5606ad47-5ee0-437a-817e-70c366052962

---

### [MS-FSCC] File System Control Codes

**Purpose:** Defines the network format of Windows file system structures used by other protocols — notably MS-SMB2 and MS-FSA. Covers FSCTL codes, file information classes, query/set info structures, and reparse point formats.

**Current revision:** 60.0 (November 2025)

**ProtoSDK path:** `ProtoSDK/MS-FSCC/`
**Test suite path:** `TestSuites/FileServer/src/FSA/` (indirectly — FSA tests use FSCC structures)

**Key concepts for contributors:**
- FSCC is a **dependency**, not a standalone test target. It provides the structures for SMB2 QUERY_INFO, SET_INFO, and IOCTL payloads.
- `FileInformationClass` enum values (e.g., `FileBasicInformation`, `FileEndOfFileInformation`) are used extensively in SMB2 QUERY_INFO/SET_INFO.
- Reparse point formats (NTFS junctions, symbolic links, DFS referrals) are defined here.
- File attributes flags, access mask definitions, and directory enumeration result structures come from FSCC.

**Spec link:** https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-fscc/efbfe127-73ad-4140-9967-ec6500e66d5e

---

### [MS-DFSC] Distributed File System (DFS) Referral Protocol

**Purpose:** Specifies the protocol by which a DFS client obtains referrals to navigate a DFS namespace to find the actual file server hosting a resource.

**ProtoSDK path:** `ProtoSDK/MS-DFSC/`
**Test suite path:** `TestSuites/FileServer/src/DFSC/`

**Key concepts:**
- DFS namespaces allow a single UNC path (e.g., `\\domain\share`) to be transparently redirected to multiple servers.
- The client sends a `REQ_GET_DFS_REFERRAL` request via SMB2 IOCTL (FSCTL_GET_DFSREFERRALS).
- The server responds with referral entries listing target servers/paths.
- DFS Link referrals, DFS root referrals, and inter-DFS referrals are distinct response types.

**Spec link:** https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-dfsc/

---

### [MS-FSRVP] File Server Remote VSS Protocol

**Purpose:** Enables a backup application to request shadow copies (snapshots) of file system shares on a remote file server using the VSS (Volume Shadow Copy Service) framework.

**ProtoSDK path:** `ProtoSDK/MS-FSRVP/`
**Test suite path:** `TestSuites/FileServer/src/FSRVP/`

**Key concepts:**
- Uses RPC (via MS-RPCE) over SMB2 named pipe `\PIPE\FssagentRpc`.
- Operations: `IsPathShadowCopied`, `AddToShadowCopySet`, `PrepareShadowCopySet`, `CommitShadowCopySet`, `ExposeShadowCopySet`, `AbortShadowCopySet`.

**Spec link:** https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-fsrvp/

---

### [MS-RSVD] Remote Shared Virtual Disk Protocol

**Purpose:** Enables Hyper-V guests to share virtual hard disk (VHD/VHDX) files over SMB2.

**ProtoSDK path:** `ProtoSDK/MS-RSVD/`
**Test suite path:** `TestSuites/FileServer/src/RSVD/`

**Spec link:** https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-rsvd/

---

### [MS-SQOS] Storage Quality of Service Protocol

**Purpose:** Allows clients to query and set I/O quality-of-service policies on virtual disk files.

**ProtoSDK path:** `ProtoSDK/MS-SQOS/`
**Test suite path:** `TestSuites/FileServer/src/SQOS/`

**Spec link:** https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-sqos/

---

### [MS-SWN] Service Witness Protocol

**Purpose:** Allows SMB2 clients to register for notifications from a witness service so they can proactively move to a healthy cluster node after a failover.

**ProtoSDK path:** `ProtoSDK/MS-SWN/`
**Test suite path:** `TestSuites/FileServer/src/ServerFailover/` (uses SWN for witness-based failover tests)

**Spec link:** https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-swn/

---

### [MS-SMBD] SMB2 Remote Direct Memory Access (RDMA) Transport Protocol

**Purpose:** Specifies how SMB2 packets are delivered over RDMA-capable transports (iWARP, InfiniBand) using Direct Data Placement (DDP). Benefits: reduced CPU overhead, lower latency, improved throughput for large file transfers.

**Current revision:** 16.0 (August 2025)

**ProtoSDK path:** `ProtoSDK/MS-SMBD/`, `ProtoSDK/RDMA/` (C++/CLI Windows), `ProtoSDK/RdmaLinux/` (Linux)
**Test suite path:** `TestSuites/MS-SMBD/src/`

**Key concepts for contributors:**
- SMBD is a framing protocol: it wraps SMB2 messages in SMBD data transfers.
- The SMBD connection negotiation establishes buffer descriptor size, send/receive credits, and maximum fragmented message size.
- **Direct data placement (DDP):** The remote endpoint can RDMA-read/write data directly into application buffers, bypassing intermediate copies.
- Two RDMA verbs used: `Send`/`Receive` for SMBD control and small messages; `RDMA Read`/`RDMA Write` for large data transfers.
- `SmbdClient` in ProtoSDK wraps the `IRdmaEndpoint` adapter. On Windows, `IRdmaEndpoint` is implemented by the C++/CLI `RdmaEndpoint` class. On Linux, by `RdmaLinuxEndpoint`.
- Test cases test both `SMB2overSMBD` (SMB2 semantics) and raw SMBD connection handling.

**Spec link:** https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-smbd/

---

## Remote Desktop Protocols

### [MS-RDPBCGR] Remote Desktop Protocol: Basic Connectivity and Graphics Remoting

**Purpose:** The core RDP protocol. Transfers graphics display from the remote system to the user and transports input (keyboard, mouse) from the user to the remote system. Handles TLS/CredSSP negotiation, session setup, capability exchange, and basic graphics output.

**Current revision:** 62.0 (March 2026)

**ProtoSDK path:** `ProtoSDK/MS-RDPBCGR/`
**Namespace:** `Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpbcgr`
**Test suite path:** `TestSuites/RDP/Client/src/TestSuite/RDPBCGR/`, `TestSuites/RDP/Server/src/TestSuite/RDPBCGR/`

**Key concepts for contributors:**
- **Connection sequence:** X.224 → MCS Connect → GCC Conference Create → Security Exchange → Client Info → Licensing → Capabilities Exchange → Synchronize/Control/Font → Active state.
- **Transport security:** Classic RDP security, TLS, CredSSP (NLA — Network Level Authentication).
- **PDU types:** Share Data Header (slow path), Fast Path (compressed, no share header).
- **Capabilities:** Negotiated at connection time; both client and server advertise capability sets. Test cases verify correct capability handling.
- **Virtual channels:** Static virtual channels (MS-RDPEFS, MS-RDPEGDI, etc.) are established during the connection sequence. Dynamic virtual channels (MS-RDPEDYC) are multiplexed later.
- `RdpbcgrClient` is the main client class in ProtoSDK. It wraps a TCP/TLS transport and handles the entire connection sequence.

**Spec link:** https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-rdpbcgr/5073f4ed-1e93-45e1-b039-6e30c385867c

---

### [MS-RDPEGFX] Remote Desktop Protocol: Graphics Pipeline Extension

**Purpose:** Provides a high-performance graphics remoting channel for RDP, replacing the older GDI-based graphics channel with a codec-based pipeline supporting RemoteFX, ClearCodec, RFX Progressive, and other codecs.

**ProtoSDK path:** `ProtoSDK/MS-RDPEGFX/`
**Test suite path:** `TestSuites/RDP/Client/src/TestSuite/RDPEGFX/`

**Spec link:** https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-rdpegfx/

---

### [MS-RDPEDYC] Remote Desktop Protocol: Dynamic Channel Virtual Channel Extension

**Purpose:** Enables dynamic creation and tear-down of virtual channels over an existing RDP connection without requiring a reconnect. Used by many RDP extensions.

**ProtoSDK path:** `ProtoSDK/MS-RDPEDYC/`
**Test suite path:** `TestSuites/RDP/Client/src/TestSuite/RDPEDYC/`, `TestSuites/RDP/Server/src/TestSuite/RDPEDYC/`

---

### [MS-RDPEMT] Remote Desktop Protocol: Multitransport Extension

**Purpose:** Allows RDP to use additional UDP-based or TCP-based transport connections alongside the main connection to improve performance.

**ProtoSDK path:** `ProtoSDK/MS-RDPEMT/`
**Test suite path:** `TestSuites/RDP/Client/src/TestSuite/RDPEMT/`, `TestSuites/RDP/Server/src/TestSuite/RDPEMT/`

---

### [MS-RDPEUDP] Remote Desktop Protocol: UDP Transport Extension

**Purpose:** Specifies the UDP transport used by the multitransport extension.

**ProtoSDK path:** `ProtoSDK/MS-RDPEUDP/`, `ProtoSDK/MS-RDPEUDP2/`
**Test suite path:** `TestSuites/RDP/Client/src/TestSuite/RDPEUDP/`

---

### Other RDP Extensions

| Identifier | Name | ProtoSDK | Test Suite |
|---|---|---|---|
| MS-RDPELE | Licensing Extension | `ProtoSDK/MS-RDPELE/` | `RDP/Server/TestSuite/RDPELE/` |
| MS-RDPEFS | File System Virtual Channel | `ProtoSDK/MS-RDPEFS/` | — |
| MS-RDPEI | Input Extension | `ProtoSDK/MS-RDPEI/` | `RDP/Client/TestSuite/RDPEI/` |
| MS-RDPEVOR | Video Optimization | `ProtoSDK/MS-RDPEVOR/` | `RDP/Client/TestSuite/RDPEVOR/` |
| MS-RDPRFX | RemoteFX Codec | `ProtoSDK/MS-RDPRFX/` | `RDP/Client/TestSuite/RDPRFX/` |
| MS-RDPEDISP | Display Update Virtual Channel | `ProtoSDK/MS-RDPEDISP/` | `RDP/Client/TestSuite/RDPEDISP/` |
| MS-RDPEGT | Geometry Tracking Virtual Channel | `ProtoSDK/MS-RDPEGT/` | `RDP/Client/TestSuite/RDPEGT/` (within RDPEGFX) |
| MS-RDPEUSB | USB Devices Virtual Channel | `ProtoSDK/MS-RDPEUSB/` | `RDP/Client/TestSuite/RDPEUSB/` |

---

## Authentication and Identity Protocols

### [MS-KILE] Kerberos Protocol Extensions

**Purpose:** Specifies Microsoft's extensions to the standard Kerberos protocol (RFC 4120). Documents Windows-specific behaviors, interactive logon extensions, and authorization information (group memberships) encoded in Kerberos tickets.

**Current revision:** 45.0 (August 2025)

**ProtoSDK path:** `ProtoSDK/KerberosLib/`
**Namespace:** `Microsoft.Protocols.TestTools.StackSdk.Security.Kerberos`
**Test suite path:** `TestSuites/Kerberos/src/TestSuite/KILE/`

**Key concepts for contributors:**
- Microsoft extends RFC 4120 Kerberos with: PADATA types (PA-PAC-REQUEST, PA-PAC-OPTIONS, PA-SVR-REFERRAL-INFO), new error codes, claim-based authorization, resource-based constrained delegation (RBCD).
- The **PAC (Privilege Attribute Certificate)** is a Kerberos authorization data structure carrying user group SIDs, logon info, and claims. It is validated by the resource server.
- KerberosLib uses ASN.1 DER encoding via the `Asn1Base` library.
- Test cases cover: AS-REQ/AS-REP, TGS-REQ/TGS-REP, AP-REQ/AP-REP, error handling, PADATA variations, cross-realm, claims, RBCD.

**Spec link:** https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-kile/2a32282e-dd48-4ad9-a542-609804b02cc9

---

### [MS-PAC] Privilege Attribute Certificate Data Structure

**Purpose:** Defines the PAC structure embedded in Kerberos tickets, including logon information (user SID, group SIDs, logon server, etc.), client claims, device claims, and UPN/DNS information buffers.

**ProtoSDK path:** `ProtoSDK/MS-PAC/`
**Test suite path:** `TestSuites/Kerberos/src/TestSuite/` (PAC validation tests)

**Spec link:** https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-pac/

---

### [MS-KKDCP] Kerberos Key Distribution Center (KDC) Proxy Protocol

**Purpose:** Tunnels Kerberos messages through HTTPS to a KDC proxy, enabling Kerberos authentication from internet-connected clients.

**ProtoSDK path:** Part of `KerberosLib/`
**Test suite path:** `TestSuites/Kerberos/src/TestSuite/KKDCP/`

**Spec link:** https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-kkdcp/

---

### [MS-NLMP] NT LAN Manager (NTLM) Authentication Protocol

**Purpose:** Specifies NTLM, a challenge-response authentication protocol. Used as a fallback when Kerberos is unavailable. Provides authentication and optional session security.

**Current revision:** 36.0 (April 2024)

**ProtoSDK path:** `ProtoSDK/MS-NLMP/`
**Namespace:** `Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp`

**Key concepts:**
- Three messages: NEGOTIATE (client → server), CHALLENGE (server → client), AUTHENTICATE (client → server).
- NTLMv2 uses HMAC-MD5-based response. Session key used for signing/sealing.
- Used by SMB2 session setup when Kerberos is not available (workgroup or when KDC is unreachable).
- ProtoSDK's `MS-NLMP` is used internally by the SSPI layer.

**Spec link:** https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-nlmp/b38c36ed-2804-4868-a9ff-8dd3182128e4

---

### [MS-SPNG] Simple and Protected GSS-API Negotiation Mechanism

**Purpose:** Specifies the Microsoft implementation of SPNEGO — the negotiation wrapper that allows client and server to agree on which security mechanism (Kerberos, NTLM) to use.

**ProtoSDK path:** `ProtoSDK/MS-SPNG/`

**Key concepts:**
- Wraps Kerberos and NTLM tokens inside a SPNEGO `negTokenInit`/`negTokenResp` envelope.
- Used in SMB2 session setup (`NEGOTIATE_TOKEN` field), HTTP authentication, etc.

---

### [MS-CSSP] Credential Security Support Provider (CredSSP) Protocol

**Purpose:** Tunnels credentials from the client to the server after TLS authentication is complete, enabling Network Level Authentication (NLA) for RDP and other services.

**ProtoSDK path:** `ProtoSDK/MS-CSSP/`
**Namespace:** `Microsoft.Protocols.TestTools.StackSdk.Security.Cssp`

**Used by:** `RdpbcgrClient` when `EncryptedProtocol.NegotiationCredSsp` is configured.

---

## Active Directory Protocols

### [MS-ADTS] Active Directory Technical Specification

**Purpose:** The master AD protocol specification covering LDAP operations, security, object schema, DC replication triggers, and many AD behaviors. Underpins the majority of ADFamily test cases.

**Test suite path:** `TestSuites/ADFamily/src/TestSuite/`

**Sub-protocols covered by ADFamily test suite:** MS-ADA1/2/3 (schema attributes), MS-ADLS (lightweight directory services), MS-ADSC (schema), MS-ADTS (core behaviors), MS-DRSR (replication), MS-NRPC (Netlogon), MS-SAMR (account management), MS-APDS (authentication policy), MS-LSAD/MS-LSAT (LSA), MS-FRS2 (file replication).

---

### [MS-DRSR] Directory Replication Service Remote Protocol

**Purpose:** Specifies the RPC protocol used by Active Directory domain controllers to replicate directory data.

**ProtoSDK path:** `ProtoSDK/MS-DRSR/`

---

### [MS-SAMR] Security Account Manager (SAM) Remote Protocol

**Purpose:** Specifies the RPC protocol for managing user accounts, groups, and passwords in a Windows domain. Provides the server-side interface to the SAM database.

**ProtoSDK path:** `ProtoSDK/MS-SAMR/`

---

### [MS-NRPC] Netlogon Remote Protocol

**Purpose:** Specifies the Netlogon protocol used for DC discovery, pass-through authentication, and secure channel management between domain members and DCs.

**ProtoSDK path:** `ProtoSDK/MS-NRPC/`

---

### [MS-ADFSPIP] Active Directory Federation Services and Proxy Integration Protocol

**Purpose:** Specifies the protocol between ADFS Proxy (Web Application Proxy) and the backend ADFS server for proxying authentication requests.

**Test suite path:** `TestSuites/MS-ADFSPIP/src/`

---

## Search and Compression Protocols

### [MS-WSP] Windows Search Protocol

**Purpose:** Allows a client to communicate with a server hosting a Windows Search service (WSS) to issue full-text search queries and retrieve results.

**Current revision:** 41.0 (March 2026)

**ProtoSDK path:** `ProtoSDK/MS-WSP/`
**Namespace:** `Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp`
**Test suite path:** `TestSuites/MS-WSP/src/`

**Key concepts:**
- Queries use OLE DB query language syntax.
- Messages include `CPMConnectIn`, `CPMCreateQueryIn`, `CPMGetRowsIn`, `CPMFetchValueIn` for establishing a search session and iterating results.
- Transport: SMB2 named pipe `\PIPE\MSFTEWDS`.
- `WspBuffer` in ProtoSDK provides a helper for reading/writing WSP-format buffers.

**Spec link:** https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-wsp/

---

### [MS-XCA] Xpress Compression Algorithm

**Purpose:** Specifies three compression algorithm variants used in Windows for SMB2 compression (SMB 3.1.1) and other places: LZ77+Huffman, Plain LZ77, and LZNT1. Emphasizes low CPU cost over maximum compression ratio. Not suitable for image/audio/video.

**Current revision:** 10.0 (April 2024)

**ProtoSDK path:** `ProtoSDK/MS-XCA/`
**Namespace:** `Microsoft.Protocols.TestTools.StackSdk.Compression.Xca`
**Test suite path:** `TestSuites/MS-XCA/src/`

**Key concepts:**
- **LZ77+Huffman:** Primary variant used in SMB 3.1.1 compression. Back-references encoded with LZ77, Huffman-coded for the output stream.
- **Plain LZ77:** Simpler variant; no Huffman coding.
- **LZNT1:** Oldest variant; uses 2-byte control words and variable-length back-references.
- `Xca.cs` is the entry point; take an algorithm enum value and a byte array to compress/decompress.
- MS-XCA tests are pure algorithm tests — no network SUT required.

**Spec link:** https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-xca/

---

## BranchCache Protocols

### [MS-PCCRTP], [MS-PCCRR], [MS-PCHC], [MS-PCCRC]

**Purpose:** Together these protocols define BranchCache — a content caching mechanism that reduces WAN bandwidth by caching frequently accessed content at branch office clients.

- **MS-PCCRTP**: Peer Content Caching and Retrieval: Transport Protocol (HTTP-based)
- **MS-PCCRR**: Peer Content Caching and Retrieval: Retrieval Protocol
- **MS-PCHC**: Peer Content Caching and Retrieval: Hosted Cache Protocol
- **MS-PCCRC**: Peer Content Caching and Retrieval: Content Identification

**Test suite path:** `TestSuites/BranchCache/src/`

---

## RPC Infrastructure

### [MS-RPCE] Remote Procedure Call Protocol Extensions

**Purpose:** Specifies Microsoft's extensions to DCE/RPC for use over SMB2 named pipes and TCP. Used by MS-FSRVP, MS-DRSR, MS-SAMR, MS-NRPC, and many other protocols.

**ProtoSDK path:** `ProtoSDK/MS-RPCE/`
**Namespace:** `Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce`

**Key concepts:**
- RPCE provides bind/alter context, request/response, and fault PDU framing over a transport (typically SMB2 named pipes).
- The `RpceClientTransport` in ProtoSDK wraps an SMB2 tree connect to the `IPC$` share and opens a named pipe, then performs PDU framing.
- Used internally by test suites that exercise RPC-based protocols (FSRVP, DRSR, etc.).

---

## Summary Table

| Identifier | Name | ProtoSDK | Test Suite |
|---|---|---|---|
| MS-SMB2 | SMB Protocol v2/3 | `ProtoSDK/MS-SMB2/` | `TestSuites/FileServer/` |
| MS-SMBD | SMB2 RDMA Transport | `ProtoSDK/MS-SMBD/` | `TestSuites/MS-SMBD/` |
| MS-FSCC | File System Control Codes | `ProtoSDK/MS-FSCC/` | `TestSuites/FileServer/FSA/` |
| MS-DFSC | DFS Referral | `ProtoSDK/MS-DFSC/` | `TestSuites/FileServer/DFSC/` |
| MS-FSRVP | File Server Remote VSS | `ProtoSDK/MS-FSRVP/` | `TestSuites/FileServer/FSRVP/` |
| MS-RSVD | Remote Shared Virtual Disk | `ProtoSDK/MS-RSVD/` | `TestSuites/FileServer/RSVD/` |
| MS-SQOS | Storage QoS | `ProtoSDK/MS-SQOS/` | `TestSuites/FileServer/SQOS/` |
| MS-SWN | Service Witness | `ProtoSDK/MS-SWN/` | `TestSuites/FileServer/ServerFailover/` |
| MS-RDPBCGR | RDP Basic Connectivity | `ProtoSDK/MS-RDPBCGR/` | `TestSuites/RDP/` |
| MS-RDPEGFX | RDP Graphics Pipeline | `ProtoSDK/MS-RDPEGFX/` | `TestSuites/RDP/Client/` |
| MS-RDPEDYC | RDP Dynamic Channels | `ProtoSDK/MS-RDPEDYC/` | `TestSuites/RDP/` |
| MS-RDPEMT | RDP Multitransport | `ProtoSDK/MS-RDPEMT/` | `TestSuites/RDP/` |
| MS-RDPEUDP | RDP UDP Transport | `ProtoSDK/MS-RDPEUDP/` | `TestSuites/RDP/Client/` |
| MS-KILE | Kerberos Extensions | `ProtoSDK/KerberosLib/` | `TestSuites/Kerberos/` |
| MS-PAC | Privilege Attribute Certificate | `ProtoSDK/MS-PAC/` | `TestSuites/Kerberos/` |
| MS-KKDCP | KDC Proxy | `ProtoSDK/KerberosLib/` | `TestSuites/Kerberos/` |
| MS-NLMP | NTLM Authentication | `ProtoSDK/MS-NLMP/` | (used by other suites) |
| MS-SPNG | SPNEGO | `ProtoSDK/MS-SPNG/` | (used by other suites) |
| MS-CSSP | CredSSP | `ProtoSDK/MS-CSSP/` | (used by RDP) |
| MS-DRSR | Directory Replication | `ProtoSDK/MS-DRSR/` | `TestSuites/ADFamily/` |
| MS-SAMR | SAM Remote | `ProtoSDK/MS-SAMR/` | `TestSuites/ADFamily/` |
| MS-NRPC | Netlogon Remote | `ProtoSDK/MS-NRPC/` | `TestSuites/ADFamily/` |
| MS-RPCE | RPC Extensions | `ProtoSDK/MS-RPCE/` | (used by other suites) |
| MS-WSP | Windows Search | `ProtoSDK/MS-WSP/` | `TestSuites/MS-WSP/` |
| MS-XCA | Xpress Compression | `ProtoSDK/MS-XCA/` | `TestSuites/MS-XCA/` |
| MS-ADTS | AD Technical Specification | — | `TestSuites/ADFamily/` |
| MS-ADFSPIP | ADFS Proxy Integration | — | `TestSuites/MS-ADFSPIP/` |
| MS-AZOD | Azure Object Discovery | — | `TestSuites/MS-AZOD/` |
| MS-ADOD | AD Domain Operations | — | `TestSuites/MS-ADOD/` |
