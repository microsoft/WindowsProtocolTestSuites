# Diff Report: MS-SMB2_v85.0_2026-03-09_vs_MS-SMB2_v83.0_2025-07-28

**Protocol:** MS-SMB2
**New version:** 85.0 (2026-03-09)
**Old version:** 83.0 (2025-07-28)
**Generated:** 2026-03-30T16:12:21.499464Z

**Flagged changes:** 90
**Suppressed (false positives):** 0

---

## Section 2.2.1.1: SMB2 Packet Header - ASYNC
**Change type:** Modified

### Old Content
```
If the SMB2_FLAGS_ASYNC_COMMAND bit is set in Flags, the header takes the following form.
ProtocolId (4 bytes): The protocol identifier. The value MUST be set to 0x424D53FE, also represented as (in network order) 0xFE, 'S', 'M', and 'B'.
StructureSize (2 bytes): MUST be set to 64, which is the size, in bytes, of the SMB2 header structure.
CreditCharge (2 bytes): In the SMB 2.0.2 dialect, this field MUST NOT be used and MUST be reserved. The sender MUST set this to 0, and the receiver MUST ignore it. In all other dialects, this field indicates the number of credits that this request consumes.
(ChannelSequence,Reserved)/Status (4 bytes): In a request, this field is interpreted in different ways depending on the SMB2 dialect.
In the SMB 3.x dialect family, this field is interpreted as the ChannelSequence field followed by the Reserved field in a request.
ChannelSequence (2 bytes): This field is an indication to the server about the client's Channel change.
Reserved (2 bytes): This field SHOULD be set to zero and the server MUST ignore it on receipt.
In the SMB 2.0.2 and SMB 2.1 dialects, this field is interpreted as the Status field in a request.
Status (4 bytes): The client MUST set this field to 0 and the server MUST ignore it on receipt.
In all SMB dialects for a response this field is interpreted as the Status field. This field can be set to any value. For a list of valid status codes, see [MS-ERREF] section 2.3.
Command (2 bytes): The command code of this packet. This field MUST contain one of the following valid commands:
CreditRequest/CreditResponse (2 bytes): On a request, this field indicates the number of credits the client is requesting. On a response, it indicates the number of credits granted to the client.
Flags (4 bytes): A flags field, which indicates how to process the operation. This field MUST be constructed using the following values:
NextCommand (4 bytes): For a compounded request and response, this field MUST be set to the offset, in bytes, from the beginning of this SMB2 header to the start of the subsequent 8-byte aligned SMB2 header. If this is not a compounded request or response, or this is the last header in a compounded request or response, this value MUST be 0.
MessageId (8 bytes): A value that identifies a message request and response uniquely across all messages that are sent on the same SMB 2 Protocol transport connection.
AsyncId (8 bytes): A unique identification number that is created by the server to handle operations asynchronously, as specified in section 3.3.4.2.
SessionId (8 bytes): Uniquely identifies the established session for the command. This field MUST be set to 0 for an SMB2 NEGOTIATE Request (section 2.2.3) and for an SMB2 NEGOTIATE Response (section 2.2.4).
Signature (16 bytes): The 16-byte signature of the message, if SMB2_FLAGS_SIGNED is set in the Flags field of the SMB2 header and the message is not encrypted. If the message is not signed, this field MUST be 0.
```

### New Content
```
If the SMB2_FLAGS_ASYNC_COMMAND bit is set in Flags, the header takes the following form.
ProtocolId (4 bytes): The protocol identifier. The value MUST be set to 0x424D53FE, also represented as (in network order) 0xFE, 'S', 'M', and 'B'.
StructureSize (2 bytes): MUST be set to 64, which is the size, in bytes, of the SMB2 header structure.
CreditCharge (2 bytes): In the SMB 2.0.2 dialect, this field MUST NOT be used and MUST be reserved. The sender MUST set this to 0, and the receiver MUST ignore it. In all other dialects, this field indicates the number of credits that this request consumes.
(ChannelSequence,Reserved)/Status (4 bytes): In a request, this field is interpreted in different ways depending on the SMB2 dialect.
In the SMB 3.x dialect family, this field is interpreted as the ChannelSequence field followed by the Reserved field in a request.
ChannelSequence (2 bytes): This field is an indication to the server about the client's Channel change.
Reserved (2 bytes): This field SHOULD be set to zero and the server MUST ignore it on receipt.
In the SMB 2.0.2 and SMB 2.1 dialects, this field is interpreted as the Status field in a request.
Status (4 bytes): The client MUST set this field to 0 and the server MUST ignore it on receipt.
In all SMB dialects for a response this field is interpreted as the Status field. This field can be set to any value. For a list of valid status codes, see [MS-ERREF] section 2.3.
Command (2 bytes): The command code of this packet. This field MUST contain one of the following valid commands:
CreditRequest/CreditResponse (2 bytes): On a request, this field indicates the number of credits the client is requesting. On a response, it indicates the number of credits granted to the client.
Flags (4 bytes): A flags field, which indicates how to process the operation. This field MUST be constructed using the following values:
NextCommand (4 bytes): For a compounded request and response, this field MUST be set to the offset, in bytes, from the beginning of this SMB2 header to the start of the subsequent 8-byte aligned SMB2 header. If this is not a compounded request or response, or this is the last header in a compounded request or response, this value MUST be 0.
MessageId (8 bytes): A value that identifies a message request and response uniquely across all messages that are sent on the same SMB 2 Protocol transport connection.
AsyncId (8 bytes): A unique identification number that is created by the server to handle operations asynchronously, as specified in section 3.3.4.2.
SessionId (8 bytes): Uniquely identifies the established session for the command. This field MUST be set to 0 for an SMB2 NEGOTIATE Request (section 2.2.3) and for an SMB2 NEGOTIATE Response (section 2.2.4).
Signature (16 bytes): The 16-byte signature of the message, if SMB2_FLAGS_SIGNED is set in the Flags field of the SMB2 header. If the message is not signed, this field MUST be 0.
```

## Section 2.2.1.2: SMB2 Packet Header - SYNC
**Change type:** Modified

### Old Content
```
If the SMB2_FLAGS_ASYNC_COMMAND bit is not set in Flags, the header takes the following form.
ProtocolId (4 bytes): The protocol identifier. The value MUST be set to 0x424D53FE, also represented as (in network order) 0xFE, 'S', 'M', and 'B'.
StructureSize (2 bytes): This MUST be set to 64, which is the size, in bytes, of the SMB2 header structure.
CreditCharge (2 bytes): In the SMB 2.0.2 dialect, this field MUST NOT be used and MUST be reserved. The sender MUST set this to 0, and the receiver MUST ignore it. In all other dialects, this field indicates the number of credits that this request consumes.
(ChannelSequence,Reserved)/Status (4 bytes): In a request, this field is interpreted in different ways depending on the SMB2 dialect.
In the SMB 3.x dialect family, this field is interpreted as the ChannelSequence field followed by the Reserved field in a request.
ChannelSequence (2 bytes): This field is an indication to the server about the client's Channel change.
Reserved (2 bytes): This field SHOULD be set to zero and the server MUST ignore it on receipt.
In the SMB 2.0.2 and SMB 2.1 dialects, this field is interpreted as the Status field in a request.
Status (4 bytes): The client MUST set this field to 0 and the server MUST ignore it on receipt.
In all SMB dialects for a response this field is interpreted as the Status field. This field can be set to any value. For a list of valid status codes, see [MS-ERREF] section 2.3.
Command (2 bytes): The command code of this packet. This field MUST contain one of the following valid commands.
CreditRequest/CreditResponse (2 bytes): On a request, this field indicates the number of credits the client is requesting. On a response, it indicates the number of credits granted to the client.
Flags (4 bytes): A Flags field indicates how to process the operation. This field MUST be constructed using the following values:
NextCommand (4 bytes): For a compounded request and response, this field MUST be set to the offset, in bytes, from the beginning of this SMB2 header to the start of the subsequent 8-byte aligned SMB2 header. If this is not a compounded request or response, or this is the last header in a compounded request or response, this value MUST be 0.
MessageId (8 bytes): A value that identifies a message request and response uniquely across all messages that are sent on the same SMB 2 Protocol transport connection.
Reserved (4 bytes): The client SHOULD<3> set this field to 0. The server MAY<4> ignore this field on receipt.
TreeId (4 bytes): Uniquely identifies the tree connect for the command. This MUST be 0 for the SMB2 TREE_CONNECT Request. The TreeId can be any unsigned 32-bit integer that is received from a previous SMB2 TREE_CONNECT Response. TreeId SHOULD be set to 0 for the following commands:
SMB2 NEGOTIATE Request
SMB2 NEGOTIATE Response
SMB2 SESSION_SETUP Request
SMB2 SESSION_SETUP Response
SMB2 LOGOFF Request
SMB2 LOGOFF Response
SMB2 ECHO Request
SMB2 ECHO Response
SMB2 CANCEL Request
SessionId (8 bytes): Uniquely identifies the established session for the command. This field MUST be set to 0 for an SMB2 NEGOTIATE Request (section 2.2.3) and for an SMB2 NEGOTIATE Response (section 2.2.4).
Signature (16 bytes): The 16-byte signature of the message, if SMB2_FLAGS_SIGNED is set in the Flags field of the SMB2 header and the message is not encrypted. If the message is not signed, this field MUST be 0.
```

### New Content
```
If the SMB2_FLAGS_ASYNC_COMMAND bit is not set in Flags, the header takes the following form.
ProtocolId (4 bytes): The protocol identifier. The value MUST be set to 0x424D53FE, also represented as (in network order) 0xFE, 'S', 'M', and 'B'.
StructureSize (2 bytes): This MUST be set to 64, which is the size, in bytes, of the SMB2 header structure.
CreditCharge (2 bytes): In the SMB 2.0.2 dialect, this field MUST NOT be used and MUST be reserved. The sender MUST set this to 0, and the receiver MUST ignore it. In all other dialects, this field indicates the number of credits that this request consumes.
(ChannelSequence,Reserved)/Status (4 bytes): In a request, this field is interpreted in different ways depending on the SMB2 dialect.
In the SMB 3.x dialect family, this field is interpreted as the ChannelSequence field followed by the Reserved field in a request.
ChannelSequence (2 bytes): This field is an indication to the server about the client's Channel change.
Reserved (2 bytes): This field SHOULD be set to zero and the server MUST ignore it on receipt.
In the SMB 2.0.2 and SMB 2.1 dialects, this field is interpreted as the Status field in a request.
Status (4 bytes): The client MUST set this field to 0 and the server MUST ignore it on receipt.
In all SMB dialects for a response this field is interpreted as the Status field. This field can be set to any value. For a list of valid status codes, see [MS-ERREF] section 2.3.
Command (2 bytes): The command code of this packet. This field MUST contain one of the following valid commands.
CreditRequest/CreditResponse (2 bytes): On a request, this field indicates the number of credits the client is requesting. On a response, it indicates the number of credits granted to the client.
Flags (4 bytes): A Flags field indicates how to process the operation. This field MUST be constructed using the following values:
NextCommand (4 bytes): For a compounded request and response, this field MUST be set to the offset, in bytes, from the beginning of this SMB2 header to the start of the subsequent 8-byte aligned SMB2 header. If this is not a compounded request or response, or this is the last header in a compounded request or response, this value MUST be 0.
MessageId (8 bytes): A value that identifies a message request and response uniquely across all messages that are sent on the same SMB 2 Protocol transport connection.
Reserved (4 bytes): The client SHOULD<3> set this field to 0. The server MAY<4> ignore this field on receipt.
TreeId (4 bytes): Uniquely identifies the tree connect for the command. This MUST be 0 for the SMB2 TREE_CONNECT Request. The TreeId can be any unsigned 32-bit integer that is received from a previous SMB2 TREE_CONNECT Response. TreeId SHOULD be set to 0 for the following commands:
SMB2 NEGOTIATE Request
SMB2 NEGOTIATE Response
SMB2 SESSION_SETUP Request
SMB2 SESSION_SETUP Response
SMB2 LOGOFF Request
SMB2 LOGOFF Response
SMB2 ECHO Request
SMB2 ECHO Response
SMB2 CANCEL Request
SessionId (8 bytes): Uniquely identifies the established session for the command. This field MUST be set to 0 for an SMB2 NEGOTIATE Request (section 2.2.3) and for an SMB2 NEGOTIATE Response (section 2.2.4).
Signature (16 bytes): The 16-byte signature of the message, if SMB2_FLAGS_SIGNED is set in the Flags field of the SMB2 header. If the message is not signed, this field MUST be 0.
```

## Section 2.2.35: SMB2 CHANGE_NOTIFY Request
**Change type:** Modified

### Old Content
```
The SMB2 CHANGE_NOTIFY Request packet is sent by the client to request change notifications on a directory. This request consists of an SMB2 header, as specified in section 2.2.1, followed by this request structure:
StructureSize (2 bytes): The client MUST set this field to 32, indicating the size of the request structure, not including the header.
Flags (2 bytes): Flags indicating how the operation MUST be processed. This field MUST be either zero or the following value:
OutputBufferLength (4 bytes): The maximum number of bytes the server is allowed to return in the SMB2 CHANGE_NOTIFY Response (section 2.2.36).
FileId (16 bytes): An SMB2_FILEID identifier of the directory to monitor for changes.
CompletionFilter (4 bytes): Specifies the types of changes to monitor. It is valid to choose multiple trigger conditions. In this case, if any condition is met, the client is notified of the change and the CHANGE_NOTIFY operation is completed. This field MUST be constructed using the following values:
Reserved (4 bytes): This field MUST NOT be used and MUST be reserved. The client MUST set this field to 0, and the server MUST ignore it on receipt.
```

### New Content
```
The SMB2 CHANGE_NOTIFY Request packet is sent by the client to request change notifications on a directory. This request consists of an SMB2 header, as specified in section 2.2.1, followed by this request structure:
StructureSize (2 bytes): The client MUST set this field to 32, indicating the size of the request structure, not including the header.
Flags (2 bytes): Flags indicating how the operation MUST be processed. This field MUST be set to one of the following values:
OutputBufferLength (4 bytes): The maximum number of bytes the server is allowed to return in the SMB2 CHANGE_NOTIFY Response (section 2.2.36).
FileId (16 bytes): An SMB2_FILEID identifier of the directory to monitor for changes.
CompletionFilter (4 bytes): Specifies the types of changes to monitor. It is valid to choose multiple trigger conditions. In this case, if any condition is met, the client is notified of the change and the CHANGE_NOTIFY operation is completed. This field MUST be constructed using the following values:
Reserved (4 bytes): This field MUST NOT be used and MUST be reserved. The client MUST set this field to 0, and the server MUST ignore it on receipt.
```

## Section 3.2.4.1.1: Signing the Message
**Change type:** Modified

### Old Content
```
The client MUST sign the message if one of the following conditions is TRUE:
If Connection.Dialect is equal to "2.0.2" or "2.1", the message being sent contains a nonzero value in the SessionId field and the session identified by the SessionId has Session.SigningRequired equal to TRUE.
If Connection.Dialect belongs to 3.x dialect family, the message being sent contains a nonzero value in the SessionId field, the session identified by the SessionId has Session.SigningRequired equal to TRUE, and one of the following conditions is TRUE:
The session identified by SessionId has Session.EncryptData equal to FALSE.
The tree connection identified by the TreeId field has TreeConnect.EncryptData equal to FALSE.
If Session.SigningRequired is FALSE, the client MAY<111> sign the request as specified in subsequent sections.
If the client implements the SMB 3.x dialect family, and if the request is for session set up, the client MUST use Session.SigningKey, and for all other requests the client MUST provide Channel.SigningKey by looking up the Channel in Session.ChannelList, where the connection matches the Channel.Connection. Otherwise, the client MUST use Session.SessionKey for signing the request. The client provides the key for signing, the length of the request, and the request itself, and calculates the signature as specified in section 3.1.4.1. If the client signs the request, it MUST set the SMB2_FLAGS_SIGNED bit in the Flags field of the SMB2 header. If the client encrypts the message, as specified in section 3.1.4.3, then the client MUST set the Signature field of the SMB2 header to zero.
```

### New Content
```
The client MUST sign the message if one of the following conditions is TRUE:
If Connection.Dialect is equal to "2.0.2" or "2.1", the message being sent contains a nonzero value in the SessionId field and the session identified by the SessionId has Session.SigningRequired equal to TRUE.
If Connection.Dialect belongs to 3.x dialect family, the message being sent contains a nonzero value in the SessionId field, the session identified by the SessionId has Session.SigningRequired equal to TRUE, and one of the following conditions is TRUE:
The session identified by SessionId has Session.EncryptData equal to FALSE.
The tree connection identified by the TreeId field has TreeConnect.EncryptData equal to FALSE.
If Session.SigningRequired is FALSE, the client MAY<111> sign the request as specified in subsequent sections.
If the client implements the SMB 3.x dialect family, and if the request is for session set up, the client MUST use Session.SigningKey, and for all other requests the client MUST provide Channel.SigningKey by looking up the Channel in Session.ChannelList, where the connection matches the Channel.Connection. Otherwise, the client MUST use Session.SessionKey for signing the request. The client provides the key for signing, the length of the request, and the request itself, and calculates the signature as specified in section 3.1.4.1. If the client signs the request, it MUST set the SMB2_FLAGS_SIGNED bit in the Flags field of the SMB2 header.
```

## Section 3.2.5.16: Receiving an SMB2 CHANGE_NOTIFY Response
**Change type:** Modified

### Old Content
```
If the Status field of the SMB2 header of the response indicates an error, the client MUST return the received status code to the calling application.
If the Status field of the SMB2 header of the response indicates success, the client MUST copy the received information in the SMB2 CHANGE_NOTIFY Response following the SMB2 header that is described by the OutputBufferOffset and OutputBufferLength into the buffer that is provided by the calling application. The client MUST return success and the OutputBufferLength to the application.
```

### New Content
```
If the Status field of the SMB2 header of the response indicates an error, the client MUST return the received status code to the calling application.
The client SHOULD<200> fail the response with STATUS_INVALID_NETWORK_RESPONSE if any of the following conditions is TRUE:
If the size of the Buffer field in the response is greater than OutputBufferLength provided in the request.
For each FILE_NOTIFY_INFORMATION structure received in the response,
If FILE_NOTIFY_INFORMATION structure is not well-formed as specified in [MS-FSCC] section 2.7.1.
If FileNameLength field is not a multiple of 2.
If FileName field begins with "/" or “\” character or contains double-quote character.
If Flags field in the request is 0 and FileName field contains "/" or “\” character.
If the Status field of the SMB2 header of the response indicates success, the client MUST copy the received information in the SMB2 CHANGE_NOTIFY Response following the SMB2 header that is described by the OutputBufferOffset and OutputBufferLength into the buffer that is provided by the calling application. The client MUST return success and the OutputBufferLength to the application.
```

## Section 3.2.5.18: Receiving an SMB2 SET_INFO Response
**Change type:** Modified

### Old Content
```
If Connection.Dialect belongs to the SMB 3.x dialect family and the status code is STATUS_FILE_NOT_AVAILABLE, and Connection.ServerCapabilities includes SMB2_GLOBAL_CAP_PERSISTENT_HANDLES or SMB2_GLOBAL_CAP_MULTI_CHANNEL, the client MUST look up the request in Connection.OutstandingRequests using the MessageId field of the SMB2 header. If the request is found, the client SHOULD<200> replay the request by setting SMB2_FLAGS_REPLAY_OPERATION bit in the SMB2 header.
The client MUST return the received status code in the Status field of the SMB2 header of the response to the application that issued the request to set information on the file, underlying object store, or named pipe. This applies for requests to set file information, underlying object store information, quota information, and security information.
```

### New Content
```
If Connection.Dialect belongs to the SMB 3.x dialect family and the status code is STATUS_FILE_NOT_AVAILABLE, and Connection.ServerCapabilities includes SMB2_GLOBAL_CAP_PERSISTENT_HANDLES or SMB2_GLOBAL_CAP_MULTI_CHANNEL, the client MUST look up the request in Connection.OutstandingRequests using the MessageId field of the SMB2 header. If the request is found, the client SHOULD<201> replay the request by setting SMB2_FLAGS_REPLAY_OPERATION bit in the SMB2 header.
The client MUST return the received status code in the Status field of the SMB2 header of the response to the application that issued the request to set information on the file, underlying object store, or named pipe. This applies for requests to set file information, underlying object store information, quota information, and security information.
```

## Section 3.2.5.19.2: Receiving a Lease Break Notification
**Change type:** Modified

### Old Content
```
If Connection.Dialect is not "2.0.2", the client MUST verify:
If Connection.SupportsDirectoryLeasing is TRUE or Connection.SupportsFileLeasing is TRUE, the client MUST perform the following:
The client MUST locate the file in the GlobalFileTable using the LeaseKey in the Lease Break Notification. If a file is not found, no further processing is required.
If a File entry is located, the client MUST take action based on the File.LeaseState and the new LeaseState that is received in the Lease Break Notification.
If File.LeaseState includes SMB2_LEASE_WRITE_CACHING and the new LeaseState does not include SMB2_LEASE_WRITE_CACHING, the client MUST flush any cached data associated with this file by issuing one or more SMB2 WRITE requests as described in 3.2.4.7. It MUST also flush out any cached byte-range locks it has on the file by enumerating the File.OpenTable and for each open, send the cached byte-range locks by issuing SMB2 LOCK requests as described in 3.2.4.19.
If File.LeaseState includes SMB2_LEASE_READ_CACHING and the new LeaseState does not include SMB2_LEASE_READ_CACHING, the client MUST notify the application to purge cached data for the File.
If File.LeaseState includes SMB2_LEASE_HANDLE_CACHING and the new LeaseState does not include SMB2_LEASE_HANDLE_CACHING, the client MUST enumerate all handles in File.OpenTable and close any cached handles that have already been closed by the application. The close process is described in 3.2.4.5.
If Connection.Dialect belongs to the SMB 3.x dialect family and File.LeaseState is equal to the new LeaseState and (NewEpoch - File.LeaseEpoch) is greater than 1, the client MUST notify the application to purge cached data for the File.
If Connection.Dialect belongs to the SMB 3.x dialect family and NewEpoch is not equal to File.LeaseEpoch and (NewEpoch – File.LeaseEpoch) is less than or equal to 32767, the client MUST copy the new LeaseState into File.LeaseState. The client MUST set File.LeaseEpoch to NewEpoch.
Otherwise, if Connection.Dialect is "2.1", the new LeaseState granted by the server in the Lease Break Notification MUST be copied to File.LeaseState.
If a lease acknowledgment is required by the server as indicated by the SMB2_NOTIFY_BREAK_LEASE_FLAG_ACK_REQUIRED bit in the Flags field of the Lease Break Notification, the client SHOULD<201> send a Lease Break Acknowledgment request described as follows.
If all open handles on this file are closed (that is, File.OpenTable is empty for this file), the client SHOULD consider it as an implicit acknowledgment of the lease break. No explicit acknowledgment is required.
The client MUST construct a Lease Break Acknowledgment request following the syntax specified in 2.2.24.2. The LeaseKey in the request MUST be set to File.LeaseKey and the LeaseState in the request MUST be set to File.LeaseState.
The client MUST choose an Open from among the remaining opens in File.OpenTable and it MUST be used to send the acknowledgment to the server, via the connection identified by Open.Connection.
The SMB2 header is initialized as follows:
Command MUST be set to SMB2 OPLOCK_BREAK.
The MessageId field is set as specified in section 3.2.4.1.3.
The client MUST set SessionId to Open.TreeConnect.Session.SessionId.
The client MUST set TreeId to Open.TreeConnect.TreeConnectId.
```

### New Content
```
If Connection.Dialect is not "2.0.2", the client MUST verify:
If Connection.SupportsDirectoryLeasing is TRUE or Connection.SupportsFileLeasing is TRUE, the client MUST perform the following:
The client MUST locate the file in the GlobalFileTable using the LeaseKey in the Lease Break Notification. If a file is not found, no further processing is required.
If a File entry is located, the client MUST take action based on the File.LeaseState and the new LeaseState that is received in the Lease Break Notification.
If File.LeaseState includes SMB2_LEASE_WRITE_CACHING and the new LeaseState does not include SMB2_LEASE_WRITE_CACHING, the client MUST flush any cached data associated with this file by issuing one or more SMB2 WRITE requests as described in 3.2.4.7. It MUST also flush out any cached byte-range locks it has on the file by enumerating the File.OpenTable and for each open, send the cached byte-range locks by issuing SMB2 LOCK requests as described in 3.2.4.19.
If File.LeaseState includes SMB2_LEASE_READ_CACHING and the new LeaseState does not include SMB2_LEASE_READ_CACHING, the client MUST notify the application to purge cached data for the File.
If File.LeaseState includes SMB2_LEASE_HANDLE_CACHING and the new LeaseState does not include SMB2_LEASE_HANDLE_CACHING, the client MUST enumerate all handles in File.OpenTable and close any cached handles that have already been closed by the application. The close process is described in 3.2.4.5.
If Connection.Dialect belongs to the SMB 3.x dialect family and File.LeaseState is equal to the new LeaseState and (NewEpoch - File.LeaseEpoch) is greater than 1, the client MUST notify the application to purge cached data for the File.
If Connection.Dialect belongs to the SMB 3.x dialect family and NewEpoch is not equal to File.LeaseEpoch and (NewEpoch – File.LeaseEpoch) is less than or equal to 32767, the client MUST copy the new LeaseState into File.LeaseState. The client MUST set File.LeaseEpoch to NewEpoch.
Otherwise, if Connection.Dialect is "2.1", the new LeaseState granted by the server in the Lease Break Notification MUST be copied to File.LeaseState.
If a lease acknowledgment is required by the server as indicated by the SMB2_NOTIFY_BREAK_LEASE_FLAG_ACK_REQUIRED bit in the Flags field of the Lease Break Notification, the client SHOULD<202> send a Lease Break Acknowledgment request described as follows.
If all open handles on this file are closed (that is, File.OpenTable is empty for this file), the client SHOULD consider it as an implicit acknowledgment of the lease break. No explicit acknowledgment is required.
The client MUST construct a Lease Break Acknowledgment request following the syntax specified in 2.2.24.2. The LeaseKey in the request MUST be set to File.LeaseKey and the LeaseState in the request MUST be set to File.LeaseState.
The client MUST choose an Open from among the remaining opens in File.OpenTable and it MUST be used to send the acknowledgment to the server, via the connection identified by Open.Connection.
The SMB2 header is initialized as follows:
Command MUST be set to SMB2 OPLOCK_BREAK.
The MessageId field is set as specified in section 3.2.4.1.3.
The client MUST set SessionId to Open.TreeConnect.Session.SessionId.
The client MUST set TreeId to Open.TreeConnect.TreeConnectId.
```

## Section 3.2.6.1: Request Expiration Timer Event
**Change type:** Modified

### Old Content
```
When the Request Expiration timer expires, the client MUST walk all connections in the ConnectionTable. For each connection, the client MUST walk the outstanding requests in Connection.OutstandingRequests.
The client MAY<202> choose any time-out it requires based on local policy, the type of request, and network characteristics.
If Request.Timestamp plus the time-out interval exceeds the current time, the client MUST process the request as if it received a failure response from the server and the client SHOULD<203> return an implementation-specific error to the calling application.
The client MAY<204> choose to disconnect the connection as well.
```

### New Content
```
When the Request Expiration timer expires, the client MUST walk all connections in the ConnectionTable. For each connection, the client MUST walk the outstanding requests in Connection.OutstandingRequests.
The client MAY<203> choose any time-out it requires based on local policy, the type of request, and network characteristics.
If Request.Timestamp plus the time-out interval exceeds the current time, the client MUST process the request as if it received a failure response from the server and the client SHOULD<204> return an implementation-specific error to the calling application.
The client MAY<205> choose to disconnect the connection as well.
```

## Section 3.2.7.1: Handling a Network Disconnect
**Change type:** Modified

### Old Content
```
When the underlying transport indicates a disconnect, for each Session in Connection.SessionTable, the client MUST perform the following:
If Connection.Dialect belongs to the SMB 3.x dialect family, and the Session has more than one channel in Session.ChannelList, the client MUST perform the following actions:
The channel entry MUST be removed from the Session.ChannelList, where Channel.Connection matches the disconnected connection.
For each outstanding create request in Connection.OutstandingRequests containing SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 context, the client MUST replay the create request on an alternate channel by setting the SMB2_FLAGS_REPLAY_OPERATION bit in the SMB2 header.
Session.ChannelSequence MUST be incremented by 1.
If Session.Connection matches the disconnected connection, Session.Connection MUST be set to the first entry in Session.ChannelList.
Otherwise, the client MUST perform the following actions:
For each Open in Session.OpenTable:
If Connection.Dialect is not "2.0.2" and Connection.SupportsFileLeasing is TRUE, the client MUST locate the File in the GlobalFileTable by looking up Open.FileName. The client MUST delete the Open from the File.OpenTable.
If Connection.Dialect belongs to the SMB 3.x dialect family and if Connection.SupportsDirectoryLeasing is TRUE, and if all opens in File.OpenTable are deleted and if there is no entry in the GlobalFileTable whose name with its last component removed matches the Open.FileName, then the entry for the File MUST be deleted from the GlobalFileTable, and the File object MUST be freed.
Otherwise, if all opens in File.OpenTable are deleted, then the entry for this File MUST be deleted from the GlobalFileTable, and the File object MUST be freed.
If Open.Durable is not TRUE, the Open MUST be removed from the Session.OpenTable and freed, and the handle generated for the Open MUST be invalidated.
If Open.Durable is TRUE, the Open MUST be removed from the Session.OpenTable, the Open.Connection MUST be set to NULL, and the Open.TreeConnect MUST be set to NULL. The client SHOULD<205> attempt to re-establish the durable open as specified in section 3.2.4.4. If Connection.Dialect belongs to the SMB 3.x dialect family, Open.Durable is TRUE, and the client fails to re-establish the durable open within Open.DurableTimeout milliseconds, the Open MUST be freed and the handle generated for the Open MUST be invalidated.
If Open.ResilientHandle or Open.IsPersistent is TRUE, the client MUST perform the following steps:
Capture the current system time at which the disconnect occurred into Open.LastDisconnectTime.
Attempt to reestablish the durable open as specified in section 3.2.4.4.
If the reestablishment fails with a network error, the client MUST retry the reestablishment of the open for at least Open.ResilientTimeout milliseconds after Open.LastDisconnectTime, before giving up.
If the reestablishment of the durable handle fails, Open.Durable MUST be set to FALSE, Open.ResilientHandle MUST be set to FALSE, the Open MUST be removed from Session.OpenTable and the Open MUST be freed, and the handle generated for the Open MUST be invalidated.
Each TreeConnect in Session.TreeConnectTable MUST be freed, the handle generated for the TreeConnect MUST be invalidated, and the TreeConnect entry MUST be removed from Session.TreeConnectTable.
If Connection.Dialect belongs to the SMB 3.x dialect family, the client MUST free the channel and remove the channel entry in Session.ChannelList.
The client MUST free the Session and invalidate the session handle.
If Connection.Dialect belongs to the SMB 3.x dialect family and if Session.TreeConnectTable is empty in all sessions in the Connection.SessionTable for which Connection.ServerName matches the server name, the client SHOULD invoke the event as specified in [MS-SWN] section 3.2.4.3.
Finally, the connection MUST be removed from the ConnectionTable and freed.
```

### New Content
```
When the underlying transport indicates a disconnect, for each Session in Connection.SessionTable, the client MUST perform the following:
If Connection.Dialect belongs to the SMB 3.x dialect family, and the Session has more than one channel in Session.ChannelList, the client MUST perform the following actions:
The channel entry MUST be removed from the Session.ChannelList, where Channel.Connection matches the disconnected connection.
For each outstanding create request in Connection.OutstandingRequests containing SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 context, the client MUST replay the create request on an alternate channel by setting the SMB2_FLAGS_REPLAY_OPERATION bit in the SMB2 header.
Session.ChannelSequence MUST be incremented by 1.
If Session.Connection matches the disconnected connection, Session.Connection MUST be set to the first entry in Session.ChannelList.
Otherwise, the client MUST perform the following actions:
For each Open in Session.OpenTable:
If Connection.Dialect is not "2.0.2" and Connection.SupportsFileLeasing is TRUE, the client MUST locate the File in the GlobalFileTable by looking up Open.FileName. The client MUST delete the Open from the File.OpenTable.
If Connection.Dialect belongs to the SMB 3.x dialect family and if Connection.SupportsDirectoryLeasing is TRUE, and if all opens in File.OpenTable are deleted and if there is no entry in the GlobalFileTable whose name with its last component removed matches the Open.FileName, then the entry for the File MUST be deleted from the GlobalFileTable, and the File object MUST be freed.
Otherwise, if all opens in File.OpenTable are deleted, then the entry for this File MUST be deleted from the GlobalFileTable, and the File object MUST be freed.
If Open.Durable is not TRUE, the Open MUST be removed from the Session.OpenTable and freed, and the handle generated for the Open MUST be invalidated.
If Open.Durable is TRUE, the Open MUST be removed from the Session.OpenTable, the Open.Connection MUST be set to NULL, and the Open.TreeConnect MUST be set to NULL. The client SHOULD<206> attempt to re-establish the durable open as specified in section 3.2.4.4. If Connection.Dialect belongs to the SMB 3.x dialect family, Open.Durable is TRUE, and the client fails to re-establish the durable open within Open.DurableTimeout milliseconds, the Open MUST be freed and the handle generated for the Open MUST be invalidated.
If Open.ResilientHandle or Open.IsPersistent is TRUE, the client MUST perform the following steps:
Capture the current system time at which the disconnect occurred into Open.LastDisconnectTime.
Attempt to reestablish the durable open as specified in section 3.2.4.4.
If the reestablishment fails with a network error, the client MUST retry the reestablishment of the open for at least Open.ResilientTimeout milliseconds after Open.LastDisconnectTime, before giving up.
If the reestablishment of the durable handle fails, Open.Durable MUST be set to FALSE, Open.ResilientHandle MUST be set to FALSE, the Open MUST be removed from Session.OpenTable and the Open MUST be freed, and the handle generated for the Open MUST be invalidated.
Each TreeConnect in Session.TreeConnectTable MUST be freed, the handle generated for the TreeConnect MUST be invalidated, and the TreeConnect entry MUST be removed from Session.TreeConnectTable.
If Connection.Dialect belongs to the SMB 3.x dialect family, the client MUST free the channel and remove the channel entry in Session.ChannelList.
The client MUST free the Session and invalidate the session handle.
If Connection.Dialect belongs to the SMB 3.x dialect family and if Session.TreeConnectTable is empty in all sessions in the Connection.SessionTable for which Connection.ServerName matches the server name, the client SHOULD invoke the event as specified in [MS-SWN] section 3.2.4.3.
Finally, the connection MUST be removed from the ConnectionTable and freed.
```

## Section 3.3.1.1: Algorithm for Handling Available Message Sequence Numbers by the Server
**Change type:** Modified

### Old Content
```
The server MUST implement an algorithm to manage message sequence numbers. Sequence numbers are used to associate requests with responses, and to determine what requests are allowed for processing. The algorithm MUST meet the following conditions:
When an SMB2 transport connection is first established, the allowable sequence numbers that comprise the valid command window for received messages on that connection MUST be the set { 0 }.
After a sequence number is received, its value MUST never be allowed to be received again. (After the sequence number 0 is received, no other request that uses the sequence number 0 shall be processed.) If the 64-bit sequence wraps, the connection MUST be terminated.
As credits are granted as specified in section 3.3.1.2, the acceptable sequence numbers MUST progress in a monotonically increasing manner. For example, if the set consists of { 0 }, and 3 credits are granted, the valid command window set MUST grow to { 0, 1, 2, 3 }.
The server MUST allow requests to be received out of sequence. For example, if the valid command window set is { 0, 1, 2, 3 }, it is valid to receive a request with sequence number 2 before receiving a request with sequence number 0.
The server MAY limit the maximum range of the acceptable sequence numbers. For example, if the valid command window set is { 0, 1, 2, 3, 4, 5 }, and the server receives requests for 1, 2, 3, 4, and 5, it MAY<206> choose to not grant more credits and keep the valid command window set at { 0 } until the sequence number 0 is received.
The client's request consumes at least one sequence number for any request except the SMB2 CANCEL Request. If the negotiated dialect is SMB 2.1 or SMB 3.x and the request is a multi-credit request, it consumes sequence numbers based on the CreditCharge field in the SMB2 header, as specified in 3.3.5.2.3.
For the client side of this algorithm, see section 3.2.4.1.6.
```

### New Content
```
The server MUST implement an algorithm to manage message sequence numbers. Sequence numbers are used to associate requests with responses, and to determine what requests are allowed for processing. The algorithm MUST meet the following conditions:
When an SMB2 transport connection is first established, the allowable sequence numbers that comprise the valid command window for received messages on that connection MUST be the set { 0 }.
After a sequence number is received, its value MUST never be allowed to be received again. (After the sequence number 0 is received, no other request that uses the sequence number 0 shall be processed.) If the 64-bit sequence wraps, the connection MUST be terminated.
As credits are granted as specified in section 3.3.1.2, the acceptable sequence numbers MUST progress in a monotonically increasing manner. For example, if the set consists of { 0 }, and 3 credits are granted, the valid command window set MUST grow to { 0, 1, 2, 3 }.
The server MUST allow requests to be received out of sequence. For example, if the valid command window set is { 0, 1, 2, 3 }, it is valid to receive a request with sequence number 2 before receiving a request with sequence number 0.
The server MAY limit the maximum range of the acceptable sequence numbers. For example, if the valid command window set is { 0, 1, 2, 3, 4, 5 }, and the server receives requests for 1, 2, 3, 4, and 5, it MAY<207> choose to not grant more credits and keep the valid command window set at { 0 } until the sequence number 0 is received.
The client's request consumes at least one sequence number for any request except the SMB2 CANCEL Request. If the negotiated dialect is SMB 2.1 or SMB 3.x and the request is a multi-credit request, it consumes sequence numbers based on the CreditCharge field in the SMB2 header, as specified in 3.3.5.2.3.
For the client side of this algorithm, see section 3.2.4.1.6.
```

## Section 3.3.1.2: Algorithm for the Granting of Credits
**Change type:** Modified

### Old Content
```
The server MUST implement an algorithm for granting credits to the client. Each credit provides the client the capability to send a request to the server. Multiple credits allow for multiple simultaneous requests. The algorithm MUST meet the following conditions:
The number of credits held by the client MUST be considered as 1 when the connection is established.
The server MUST ensure that the number of credits held by the client is never reduced to zero. If the condition occurs, there is no way for the client to send subsequent requests for more credits.
The server MAY<207> grant any number of credits up to that which the client requests, or more if required by the preceding rule.
The server SHOULD<208> grant the client a non-zero value of credits in response to any non-zero value requested, within administratively configured limits. The server MUST grant the client at least 1 credit when responding to SMB2 NEGOTIATE.
The server MAY<209> vary the number of credits granted to different clients based on quality of service features, such as identity, behavior, or administrator configuration.
```

### New Content
```
The server MUST implement an algorithm for granting credits to the client. Each credit provides the client the capability to send a request to the server. Multiple credits allow for multiple simultaneous requests. The algorithm MUST meet the following conditions:
The number of credits held by the client MUST be considered as 1 when the connection is established.
The server MUST ensure that the number of credits held by the client is never reduced to zero. If the condition occurs, there is no way for the client to send subsequent requests for more credits.
The server MAY<208> grant any number of credits up to that which the client requests, or more if required by the preceding rule.
The server SHOULD<209> grant the client a non-zero value of credits in response to any non-zero value requested, within administratively configured limits. The server MUST grant the client at least 1 credit when responding to SMB2 NEGOTIATE.
The server MAY<210> vary the number of credits granted to different clients based on quality of service features, such as identity, behavior, or administrator configuration.
```

## Section 3.3.1.4: Algorithm for Leasing in an Object Store
**Change type:** Modified

### Old Content
```
If the server implements the SMB 2.1 or SMB 3.x dialect family and supports leasing, the underlying object store needs to implement an algorithm that permits multiple opens to the same object, as described in [MS-FSA] section 2.1.5.1.2, to share the lease state (for valid lease states, see section 3.3.1.12). The algorithm MUST meet the following conditions:
The algorithm MUST permit a create request from the server to the underlying object store to be accompanied by an implementation-specific<210> identifier that indicates the unique server-local context for this lease, which will be referred to as the ClientLeaseId.
The algorithm MUST allow multiple opens to an object that shares the same ClientLeaseId. These opens MUST NOT alter the lease state on an object.
The algorithm MUST permit three different caching capabilities within a lease: READ, WRITE, and HANDLE, with the following semantics:
READ caching permits the SMB2 client to cache data read from the object. Before processing one of the following operations from a client with a different ClientLeaseId, the object store MUST request that the server revoke READ caching. The object store is not required to wait for acknowledgment:
READ caching on a file:
The file is opened in a manner that overwrites the existing file.
Data is written to the file.
The file size is changed.
A byte range lock is requested for the file.
READ caching on a directory:
A new file or directory is added, deleted, or renamed within the directory.
Directory metadata such as timestamps, file attributes, and file sizes are updated.
WRITE caching permits the SMB2 client to cache writes and byte-range locks on an object. Before processing one of the following operations, the underlying object store MUST request that the server revoke WRITE caching, and the object store MUST wait for acknowledgment from the server before proceeding with the operation:
The file is opened by a client with a different ClientLeaseId, and requested access includes any flags other than FILE_READ_ATTRIBUTES, FILE_WRITE_ATTRIBUTES, and SYNCHRONIZE.
HANDLE caching permits one or more SMB2 clients to delay closing handles it holds open, or to defer sending opens. Before processing one of the following operations, the underlying object store MUST request that the server revoke HANDLE caching, and the object store MUST wait for acknowledgment before proceeding with the operation:
HANDLE caching on a file:
A file is opened with an access or share mode incompatible with opens from clients with different ClientLeaseIds.
The parent directory is being renamed.
HANDLE caching on a directory:
The directory is opened with an access/share mode incompatible with opens from a client with a different ClientLeaseId.
Parent directory is renamed or deleted.
The underlying object store SHOULD request that the server revoke multiple lease state flags at the same time if an operation results in the loss of several caching flags.
The algorithm SHOULD support the following combinations of caching flags on a file: No caching, Read caching, Read + Write caching, Read + Handle caching, and Read + Write + Handle caching. The algorithm SHOULD support No caching, Read caching, and Read + Handle caching on a directory.
The algorithm MAY<211> support other combinations of caching flags.
The algorithm MUST allow a client to flow one or more creates with the same ClientLeaseId to the underlying object store during a lease break without blocking the create until the acknowledgment of the lease break is received.
The algorithm SHOULD allow additional lease state flags on subsequent opens with the same ClientLeaseId to permit upgrading the lease state. The algorithm MUST NOT allow the client to release lease state flags on subsequent opens with the same ClientLeaseId to downgrade the lease state.
If the requested lease state is not a superset of the existing lease state flags for this ClientLeaseId, then the requested lease state SHOULD be interpreted as the union of the existing lease state and the requested lease state.
When the underlying object store requests that the server issue a lease break, it MUST also provide a new lease state for the server to pass to the client as part of the lease break packet, based on the operations that caused the lease break to occur.
```

### New Content
```
If the server implements the SMB 2.1 or SMB 3.x dialect family and supports leasing, the underlying object store needs to implement an algorithm that permits multiple opens to the same object, as described in [MS-FSA] section 2.1.5.1.2, to share the lease state (for valid lease states, see section 3.3.1.12). The algorithm MUST meet the following conditions:
The algorithm MUST permit a create request from the server to the underlying object store to be accompanied by an implementation-specific<211> identifier that indicates the unique server-local context for this lease, which will be referred to as the ClientLeaseId.
The algorithm MUST allow multiple opens to an object that shares the same ClientLeaseId. These opens MUST NOT alter the lease state on an object.
The algorithm MUST permit three different caching capabilities within a lease: READ, WRITE, and HANDLE, with the following semantics:
READ caching permits the SMB2 client to cache data read from the object. Before processing one of the following operations from a client with a different ClientLeaseId, the object store MUST request that the server revoke READ caching. The object store is not required to wait for acknowledgment:
READ caching on a file:
The file is opened in a manner that overwrites the existing file.
Data is written to the file.
The file size is changed.
A byte range lock is requested for the file.
READ caching on a directory:
A new file or directory is added, deleted, or renamed within the directory.
Directory metadata such as timestamps, file attributes, and file sizes are updated.
WRITE caching permits the SMB2 client to cache writes and byte-range locks on an object. Before processing one of the following operations, the underlying object store MUST request that the server revoke WRITE caching, and the object store MUST wait for acknowledgment from the server before proceeding with the operation:
The file is opened by a client with an access/share mode incompatible with opens from a client with a different ClientLeaseId, as described in [MS-FSA] section 2.1.4.12.
HANDLE caching permits one or more SMB2 clients to delay closing handles it holds open, or to defer sending opens. Before processing one of the following operations, the underlying object store MUST request that the server revoke HANDLE caching, and the object store MUST wait for acknowledgment before proceeding with the operation:
HANDLE caching on a file:
A file is opened with an access or share mode incompatible with opens from clients with different ClientLeaseIds.
The parent directory is being renamed.
HANDLE caching on a directory:
The directory is opened with an access/share mode incompatible with opens from a client with a different ClientLeaseId.
Parent directory is renamed or deleted.
The underlying object store SHOULD request that the server revoke multiple lease state flags at the same time if an operation results in the loss of several caching flags.
The algorithm SHOULD support the following combinations of caching flags on a file: No caching, Read caching, Read + Write caching, Read + Handle caching, and Read + Write + Handle caching. The algorithm SHOULD support No caching, Read caching, and Read + Handle caching on a directory.
The algorithm MAY<212> support other combinations of caching flags.
The algorithm MUST allow a client to flow one or more creates with the same ClientLeaseId to the underlying object store during a lease break without blocking the create until the acknowledgment of the lease break is received.
The algorithm SHOULD allow additional lease state flags on subsequent opens with the same ClientLeaseId to permit upgrading the lease state. The algorithm MUST NOT allow the client to release lease state flags on subsequent opens with the same ClientLeaseId to downgrade the lease state.
If the requested lease state is not a superset of the existing lease state flags for this ClientLeaseId, then the requested lease state SHOULD be interpreted as the union of the existing lease state and the requested lease state.
When the underlying object store requests that the server issue a lease break, it MUST also provide a new lease state for the server to pass to the client as part of the lease break packet, based on the operations that caused the lease break to occur.
```

## Section 3.3.1.6: Per Share
**Change type:** Modified

### Old Content
```
The server implements the following:
Share.Name: A name for the shared resource on this server.
Share.ServerName: The NetBIOS, fully qualified domain name (FQDN), or textual IPv4 or IPv6 address that the share is associated with. For more information, see [MS-SRVS] section 3.1.1.7.
Share.LocalPath: A path that describes the local resource that is being shared. This MUST be a store that either provides named pipe functionality, or that offers storage and/or retrieval of files. In the case of the latter, it MAY<212> be a device that accepts a file and then processes it in some format, such as a printer.
Share.ConnectSecurity: An authorization policy such as an access control list that describes which users are allowed to connect to this share.
Share.FileSecurity: An authorization policy such as an access control list that describes what actions users that connect to this share are allowed to perform on the shared resource.<213>
Share.CscFlags: The configured offline caching policy for this share. This value MUST be manual caching, automatic caching of files, automatic caching of files and programs, or no offline caching. For more information, see section 2.2.10. For more information about offline caching, see [OFFLINE].
Share.IsDfs: A Boolean that, if set, indicates that this share is configured for DFS. For more information, see [MSDFS].
Share.DoAccessBasedDirectoryEnumeration: A Boolean that, if set, indicates that the results of directory enumerations on this share MUST be trimmed to include only the files and directories that the calling user has the right to access.
Share.AllowNamespaceCaching: A Boolean that, if set, indicates that clients are allowed to cache directory enumeration results for better performance.<214>
Share.ForceSharedDelete: A Boolean that, if set, indicates that all opens on this share MUST include FILE_SHARE_DELETE in the sharing access.
Share.RestrictExclusiveOpens: A Boolean that, if set, indicates that users who request read-only access to a file are not allowed to deny other readers.
Share.Type: The value indicates the type of share. It MUST be one of the values that are listed in [MS-SRVS] section 2.2.2.4.
Share.Remark: A null-terminated Unicode UTF-16 string that specifies an optional comment about the shared resource.
Share.MaxUses: The value indicates the maximum number of concurrent connections that the shared resource can accommodate.
Share.CurrentUses: The value indicates the number of current trees connected to the shared resource.
Share.ForceLevel2Oplock: A Boolean that, if set, indicates that the server does not issue exclusive caching rights on this share.
Share.HashEnabled: A Boolean that, if set, indicates that the share supports hash generation for branch cache retrieval of data.
Share.SnapshotList: The list of available snapshots in this Share.
If the server implements the SMB 3.x dialect family, it MUST implement the following:
Share.CATimeout: The minimum time, in milliseconds, before closing an unreclaimed persistent handle on a continuously available share.
Share.IsCA: A Boolean that, if set, indicates that the share is continuously available.
Share.EncryptData: A Boolean that, if set, indicates that the server requires messages for accessing this share to be encrypted, per the conditions specified in section 3.3.5.2.11.
Share.SupportsIdentityRemoting: A Boolean that, if set, indicates that the share supports identity remoting by the client.
If the server implements the SMB 3.1.1 dialect, it MUST implement the following:
Share.CompressData: A Boolean that, if set, indicates that the server supports compressed read/write messages for accessing this share.
Share.IsolatedTransport: A Boolean that, if set, indicates that the share on the server supports isolated transport.
```

### New Content
```
The server implements the following:
Share.Name: A name for the shared resource on this server.
Share.ServerName: The NetBIOS, fully qualified domain name (FQDN), or textual IPv4 or IPv6 address that the share is associated with. For more information, see [MS-SRVS] section 3.1.1.7.
Share.LocalPath: A path that describes the local resource that is being shared. This MUST be a store that either provides named pipe functionality, or that offers storage and/or retrieval of files. In the case of the latter, it MAY<213> be a device that accepts a file and then processes it in some format, such as a printer.
Share.ConnectSecurity: An authorization policy such as an access control list that describes which users are allowed to connect to this share.
Share.FileSecurity: An authorization policy such as an access control list that describes what actions users that connect to this share are allowed to perform on the shared resource.<214>
Share.CscFlags: The configured offline caching policy for this share. This value MUST be manual caching, automatic caching of files, automatic caching of files and programs, or no offline caching. For more information, see section 2.2.10. For more information about offline caching, see [OFFLINE].
Share.IsDfs: A Boolean that, if set, indicates that this share is configured for DFS. For more information, see [MSDFS].
Share.DoAccessBasedDirectoryEnumeration: A Boolean that, if set, indicates that the results of directory enumerations on this share MUST be trimmed to include only the files and directories that the calling user has the right to access.
Share.AllowNamespaceCaching: A Boolean that, if set, indicates that clients are allowed to cache directory enumeration results for better performance.<215>
Share.ForceSharedDelete: A Boolean that, if set, indicates that all opens on this share MUST include FILE_SHARE_DELETE in the sharing access.
Share.RestrictExclusiveOpens: A Boolean that, if set, indicates that users who request read-only access to a file are not allowed to deny other readers.
Share.Type: The value indicates the type of share. It MUST be one of the values that are listed in [MS-SRVS] section 2.2.2.4.
Share.Remark: A null-terminated Unicode UTF-16 string that specifies an optional comment about the shared resource.
Share.MaxUses: The value indicates the maximum number of concurrent connections that the shared resource can accommodate.
Share.CurrentUses: The value indicates the number of current trees connected to the shared resource.
Share.ForceLevel2Oplock: A Boolean that, if set, indicates that the server does not issue exclusive caching rights on this share.
Share.HashEnabled: A Boolean that, if set, indicates that the share supports hash generation for branch cache retrieval of data.
Share.SnapshotList: The list of available snapshots in this Share.
If the server implements the SMB 3.x dialect family, it MUST implement the following:
Share.CATimeout: The minimum time, in milliseconds, before closing an unreclaimed persistent handle on a continuously available share.
Share.IsCA: A Boolean that, if set, indicates that the share is continuously available.
Share.EncryptData: A Boolean that, if set, indicates that the server requires messages for accessing this share to be encrypted, per the conditions specified in section 3.3.5.2.11.
Share.SupportsIdentityRemoting: A Boolean that, if set, indicates that the share supports identity remoting by the client.
If the server implements the SMB 3.1.1 dialect, it MUST implement the following:
Share.CompressData: A Boolean that, if set, indicates that the server supports compressed read/write messages for accessing this share.
Share.IsolatedTransport: A Boolean that, if set, indicates that the share on the server supports isolated transport.
```

## Section 3.3.1.10: Per Open
**Change type:** Modified

### Old Content
```
The server implements the following:
Open.FileId: A numeric value that uniquely identifies the open handle to a file or a pipe within the scope of a session over which the handle was opened. A 64-bit representation of this value, combined with Open.DurableFileId as described below, form the SMB2_FILEID described in section 2.2.14.1.
Open.FileGlobalId: A numeric value obtained via registration with [MS-SRVS], as specified in [MS-SRVS] section 3.1.6.4.
Open.DurableFileId: A numeric value that uniquely identifies the open handle to a file or a pipe within the scope of all opens granted by the server, as described by the GlobalOpenTable. A 64-bit representation of this value combined with Open.FileId, as described above, form the SMB2_FILEID described in section 2.2.14.1. This value is the persistent portion of the identifier.
Open.Session: A reference to the authenticated session, as specified in section 3.3.1.8, over which this open was performed. If the open is not attached to a session at this time, this value MUST be NULL.
Open.TreeConnect: A reference to the TreeConnect, as specified in section 3.3.1.9, over which the open was performed. If the open is not attached to a TreeConnect at this time, this value MUST be NULL.
Open.Connection: A reference to the connection, as specified in section 3.3.1.7, that created this open. If the open is not attached to a connection at this time, this value MUST be NULL.
Open.LocalOpen: An open of a file or named pipe in the underlying local resource that is used to perform the local operations, such as reading or writing, to the underlying object. For named pipes, Open.LocalOpen is shared between the SMB server and RPC server applications which serve RPC requests on a given named pipe. The higher level interfaces described in sections 3.3.4.5 and 3.3.4.11 require this shared element.
Open.GrantedAccess: The access granted on this open, as defined in section 2.2.13.1.
Open.OplockLevel: The current oplock level for this open. This value MUST be one of the OplockLevel values defined in section 2.2.14: SMB2_OPLOCK_LEVEL_NONE, SMB2_OPLOCK_LEVEL_II, SMB2_OPLOCK_LEVEL_EXCLUSIVE, SMB2_OPLOCK_LEVEL_BATCH, or SMB2_OPLOCK_LEVEL_LEASE.
Open.OplockState: The current oplock state of the file. This value MUST be Held, Breaking, or None.
Open.OplockTimeout: The time value that indicates when an oplock that is breaking and has not received an acknowledgment from the client will be acknowledged by the server.
Open.IsDurable: A Boolean that indicates whether the Open is preserved for reconnect.
Open.DurableOpenTimeout: The time the server waits before closing a handle that has been preserved for durability, if a client has not reclaimed it.
Open.DurableOpenScavengerTimeout: A time stamp value, if non-zero, representing the maximum time to preserve the open for reclaim.
Open.DurableOwner: A security descriptor that holds the original opener of the open. This allows the server to determine if a caller that is trying to reestablish a durable open is allowed to do so. If the server implements SMB 2.1 or SMB 3.x and supports resiliency, this value is also used to enforce security during resilient open reestablishment.
Open.CurrentEaIndex: For extended attribute information, this value indicates the current location in an extended attribute information list and allows for the continuing of an enumeration across multiple requests.
Open.CurrentQuotaIndex: For quota queries, this value indicates the current index in the quota information list and allows for the continuation of an enumeration across multiple requests.
Open.LockCount: A numeric value that indicates the number of locks that are held by current open.
Open.PathName: A variable-length Unicode string that contains the local path name on the server that the open is performed on.
Open.ResumeKey: A 24-byte key that identifies a source file in a server-side data copy operation.
Open.FileName: A Unicode file name supplied by the client for this Open.
Open.CreateOptions: The create options requested by the client for this Open, in the format specified in section 2.2.13.
Open.FileAttributes: The file attributes used by the client for this Open, in the format specified in section 2.2.13.
If the server supports leasing, it MUST implement the following:
Open.ClientGuid: An identifier for the client machine that created this open.
Open.Lease: The lease associated with this open, as defined in 3.3.1.12. This value MUST point to a valid lease, or be set to NULL.
If the server supports resiliency, it MUST implement the following:
Open.IsResilient: A Boolean that indicates whether this open has requested resilient operation.
Open.ResiliencyTimeout: A time-out value that indicates how long the server will hold the file open after a disconnect before releasing the open.
Open.ResilientOpenTimeout: A time-out value that indicates when a handle that has been preserved for resiliency will be closed by the system if a client has not reclaimed it.
Open.LockSequenceArray: An array of 64 entries used to maintain lock sequences for resilient opens. Each entry MUST be assigned an index from the range of 1 to 64. Each entry is a structure with the following elements:
SequenceNumber: A 4-bit integer modulo 16.
Valid: A Boolean, if set to TRUE, indicates that the SequenceNumber element is valid.
If the server implements the SMB 3.x dialect family, it MUST implement the following:
Open.CreateGuid: A 16-byte value that associates this open to a create request.
Open.AppInstanceId: A 16-byte value that associates this open with a calling application.
Open.IsPersistent: A Boolean that indicates whether this open is persistent.
Open.ChannelSequence: A 16-bit identifier indicating the client's Channel change.
Open.OutstandingRequestCount: A numerical value that indicates the number of outstanding requests issued with ChannelSequence equal to Open.ChannelSequence.
Open.OutstandingPreRequestCount: A numerical value that indicates the number of outstanding requests issued with ChannelSequence less than Open.ChannelSequence.
Open.IsReplayEligible: A Boolean that indicates whether the Open is eligible for replay by a CREATE request that can be replayed by reissuing the original CREATE request with the SMB2_FLAGS_REPLAY_OPERATION flag set.
If the server implements the SMB 3.0.2 or SMB 3.1.1 dialect, it MUST implement the following:
Open.IsSharedVHDX: A Boolean that indicates whether this open is a shared virtual disk operation.
If the server implements the SMB 3.1.1 dialect, it MUST implement the following:
Open.ApplicationInstanceVersionHigh: An unsigned 64-bit numeric value representing the most significant value of the application instance version.
Open.ApplicationInstanceVersionLow: An unsigned 64-bit numeric value representing the least significant value of the application instance version.
```

### New Content
```
The server implements the following:
Open.FileId: A numeric value that uniquely identifies the open handle to a file or a pipe within the scope of a session over which the handle was opened. A 64-bit representation of this value, combined with Open.DurableFileId as described below, form the SMB2_FILEID described in section 2.2.14.1.
Open.FileGlobalId: A numeric value obtained via registration with [MS-SRVS], as specified in [MS-SRVS] section 3.1.6.4.
Open.DurableFileId: A numeric value that uniquely identifies the open handle to a file or a pipe within the scope of all opens granted by the server, as described by the GlobalOpenTable. A 64-bit representation of this value combined with Open.FileId, as described above, form the SMB2_FILEID described in section 2.2.14.1. This value is the persistent portion of the identifier.
Open.Session: A reference to the authenticated session, as specified in section 3.3.1.8, over which this open was performed. If the open is not attached to a session at this time, this value MUST be NULL.
Open.TreeConnect: A reference to the TreeConnect, as specified in section 3.3.1.9, over which the open was performed. If the open is not attached to a TreeConnect at this time, this value MUST be NULL.
Open.Connection: A reference to the connection, as specified in section 3.3.1.7, that created this open. If the open is not attached to a connection at this time, this value MUST be NULL.
Open.LocalOpen: An open of a file or named pipe in the underlying local resource that is used to perform the local operations, such as reading or writing, to the underlying object. For named pipes, Open.LocalOpen is shared between the SMB server and RPC server applications which serve RPC requests on a given named pipe. The higher level interfaces described in sections 3.3.4.5 and 3.3.4.11 require this shared element.
Open.GrantedAccess: The access granted on this open, as defined in section 2.2.13.1.
Open.OplockLevel: The current oplock level for this open. This value MUST be one of the OplockLevel values defined in section 2.2.14: SMB2_OPLOCK_LEVEL_NONE, SMB2_OPLOCK_LEVEL_II, SMB2_OPLOCK_LEVEL_EXCLUSIVE, SMB2_OPLOCK_LEVEL_BATCH, or SMB2_OPLOCK_LEVEL_LEASE.
Open.OplockState: The current oplock state of the file. This value MUST be Held, Breaking, or None.
Open.OplockTimeout: The time value that indicates when an oplock that is breaking and has not received an acknowledgment from the client will be acknowledged by the server.
Open.IsDurable: A Boolean that indicates whether the Open is preserved for reconnect.
Open.DurableOpenTimeout: The time the server waits before closing a handle that has been preserved for durability, if a client has not reclaimed it.
Open.DurableOpenScavengerTimeout: A time stamp value, if non-zero, representing the maximum time to preserve the open for reclaim.
Open.DurableOwner: A security descriptor that holds the original opener of the open. This allows the server to determine if a caller that is trying to reestablish a durable open is allowed to do so. If the server implements SMB 2.1 or SMB 3.x and supports resiliency, this value is also used to enforce security during resilient open reestablishment.
Open.CurrentEaIndex: For extended attribute information, this value indicates the current location in an extended attribute information list and allows for the continuing of an enumeration across multiple requests.
Open.CurrentQuotaIndex: For quota queries, this value indicates the current index in the quota information list and allows for the continuation of an enumeration across multiple requests.
Open.LockCount: A numeric value that indicates the number of locks that are held by current open.
Open.PathName: A variable-length Unicode string that contains the local path name on the server that the open is performed on.
Open.ResumeKey: A 24-byte key that identifies a source file in a server-side data copy operation.
Open.FileName: A Unicode file name supplied by the client for this Open.
Open.CreateOptions: The create options requested by the client for this Open, in the format specified in section 2.2.13.
Open.FileAttributes: The file attributes used by the client for this Open, in the format specified in section 2.2.13.
If the server supports leasing or implements the SMB 3.x dialect family, it MUST implement the following:
Open.ClientGuid: An identifier for the client machine that created this open.
If the server supports leasing, it MUST implement the following:
Open.Lease: The lease associated with this open, as defined in 3.3.1.12. This value MUST point to a valid lease, or be set to NULL.
If the server supports resiliency, it MUST implement the following:
Open.IsResilient: A Boolean that indicates whether this open has requested resilient operation.
Open.ResiliencyTimeout: A time-out value that indicates how long the server will hold the file open after a disconnect before releasing the open.
Open.ResilientOpenTimeout: A time-out value that indicates when a handle that has been preserved for resiliency will be closed by the system if a client has not reclaimed it.
Open.LockSequenceArray: An array of 64 entries used to maintain lock sequences for resilient opens. Each entry MUST be assigned an index from the range of 1 to 64. Each entry is a structure with the following elements:
SequenceNumber: A 4-bit integer modulo 16.
Valid: A Boolean, if set to TRUE, indicates that the SequenceNumber element is valid.
If the server implements the SMB 3.x dialect family, it MUST implement the following:
Open.CreateGuid: A 16-byte value that associates this open to a create request.
Open.AppInstanceId: A 16-byte value that associates this open with a calling application.
Open.IsPersistent: A Boolean that indicates whether this open is persistent.
Open.ChannelSequence: A 16-bit identifier indicating the client's Channel change.
Open.OutstandingRequestCount: A numerical value that indicates the number of outstanding requests issued with ChannelSequence equal to Open.ChannelSequence.
Open.OutstandingPreRequestCount: A numerical value that indicates the number of outstanding requests issued with ChannelSequence less than Open.ChannelSequence.
Open.IsReplayEligible: A Boolean that indicates whether the Open is eligible for replay by a CREATE request that can be replayed by reissuing the original CREATE request with the SMB2_FLAGS_REPLAY_OPERATION flag set.
Open.DesiredAccess: The access requested by the client for this Open, as defined in section 2.2.13.1
Open.ShareAccess: The sharing mode requested by the client for this Open, as defined in section 2.2.13
If the server implements the SMB 3.0.2 or SMB 3.1.1 dialect, it MUST implement the following:
Open.IsSharedVHDX: A Boolean that indicates whether this open is a shared virtual disk operation.
If the server implements the SMB 3.1.1 dialect, it MUST implement the following:
Open.ApplicationInstanceVersionHigh: An unsigned 64-bit numeric value representing the most significant value of the application instance version.
Open.ApplicationInstanceVersionLow: An unsigned 64-bit numeric value representing the least significant value of the application instance version.
```

## Section 3.3.1.13: Per Request
**Change type:** Modified

### Old Content
```
The server implements the following:
Request.MessageId: The value of the MessageId field from the SMB2 Header of the client request.
Request.AsyncId: An asynchronous identifier generated for an Asynchronous Operation, as specified in section 3.3.4.2. The identifier MUST uniquely identify this Request among all requests currently being processed asynchronously on a specified SMB2 transport connection. If the request is not being processed asynchronously, this value MUST be set to zero.
Request.CancelRequestId: An implementation-dependent identifier generated by the server to support cancellation of pending requests that are sent to the object store. The identifier MUST be unique among all requests currently being processed by the server and all object store operations being performed by other server applications.<215>
Request.Open: A reference to an Open of a file or named pipe, as specified in section 3.3.1.10. If the request is not associated with an Open at this time, this value MUST be NULL.
If the server implements the SMB 3.x dialect family, it MUST implement the following:
Request.IsEncrypted: A Boolean that, if set, indicates that the request has been encrypted.
Request.TransformSessionId: The SessionId sent by the client in the SMB2 TRANSFORM_HEADER, if the request is encrypted.
If the server implements the SMB 3.1.1 dialect, it implements the following:
Request.CompressReply: A Boolean that, if set, indicates that the reply to this request is eligible for compression.
```

### New Content
```
The server implements the following:
Request.MessageId: The value of the MessageId field from the SMB2 Header of the client request.
Request.AsyncId: An asynchronous identifier generated for an Asynchronous Operation, as specified in section 3.3.4.2. The identifier MUST uniquely identify this Request among all requests currently being processed asynchronously on a specified SMB2 transport connection. If the request is not being processed asynchronously, this value MUST be set to zero.
Request.CancelRequestId: An implementation-dependent identifier generated by the server to support cancellation of pending requests that are sent to the object store. The identifier MUST be unique among all requests currently being processed by the server and all object store operations being performed by other server applications.<216>
Request.Open: A reference to an Open of a file or named pipe, as specified in section 3.3.1.10. If the request is not associated with an Open at this time, this value MUST be NULL.
If the server implements the SMB 3.x dialect family, it MUST implement the following:
Request.IsEncrypted: A Boolean that, if set, indicates that the request has been encrypted.
Request.TransformSessionId: The SessionId sent by the client in the SMB2 TRANSFORM_HEADER, if the request is encrypted.
If the server implements the SMB 3.1.1 dialect, it implements the following:
Request.CompressReply: A Boolean that, if set, indicates that the reply to this request is eligible for compression.
```

## Section 3.3.2.1: Oplock Break Acknowledgment Timer
**Change type:** Modified

### Old Content
```
This timer controls the amount of time the server waits for an oplock break acknowledgment from the client (as specified in section 2.2.24.1) after sending an oplock break notification (as specified in section 2.2.23.1) to the client. The server MUST wait for an interval of time greater than or equal to the oplock break acknowledgment timer. This timer MUST be smaller than the client Request Expiration time, as specified in section 3.2.6.1.<216>
```

### New Content
```
This timer controls the amount of time the server waits for an oplock break acknowledgment from the client (as specified in section 2.2.24.1) after sending an oplock break notification (as specified in section 2.2.23.1) to the client. The server MUST wait for an interval of time greater than or equal to the oplock break acknowledgment timer. This timer MUST be smaller than the client Request Expiration time, as specified in section 3.2.6.1.<217>
```

## Section 3.3.2.2: Durable Open Scavenger Timer
**Change type:** Modified

### Old Content
```
This timer controls the amount of time the server keeps a durable handle active after the underlying transport connection to the client is lost.<217> The server MUST keep the durable handle active for at least this amount of time, except in the cases of an oplock break indicated by the object store as specified in section 3.3.4.6, administrative actions, or resource constraints.
```

### New Content
```
This timer controls the amount of time the server keeps a durable handle active after the underlying transport connection to the client is lost.<218> The server MUST keep the durable handle active for at least this amount of time, except in the cases of an oplock break indicated by the object store as specified in section 3.3.4.6, administrative actions, or resource constraints.
```

## Section 3.3.2.3: Session Expiration Timer
**Change type:** Modified

### Old Content
```
This timer controls the periodic scheduling of searching for sessions that have passed their expiration time. The server SHOULD<218> schedule this timer such that sessions are expired in a timely manner. This timer is also used for scavenging connections on which the NEGOTIATE and SESSION_SETUP have not been performed within a specified time.
```

### New Content
```
This timer controls the periodic scheduling of searching for sessions that have passed their expiration time. The server SHOULD<219> schedule this timer such that sessions are expired in a timely manner. This timer is also used for scavenging connections on which the NEGOTIATE and SESSION_SETUP have not been performed within a specified time.
```

## Section 3.3.2.5: Lease Break Acknowledgment Timer
**Change type:** Modified

### Old Content
```
If the server implements the SMB 2.1 or SMB 3.x dialect family and supports leasing, this timer controls the amount of time the server waits for a Lease Break acknowledgment from the client (as specified in section 2.2.24.2) after sending a lease break notification (as specified in section 2.2.23.2) to the client. The server MUST wait for an interval of time greater than or equal to the lease break acknowledgment timer. This timer MUST be smaller than the client Request Expiration time, as specified in section 3.2.6.1.<219>
```

### New Content
```
If the server implements the SMB 2.1 or SMB 3.x dialect family and supports leasing, this timer controls the amount of time the server waits for a Lease Break acknowledgment from the client (as specified in section 2.2.24.2) after sending a lease break notification (as specified in section 2.2.23.2) to the client. The server MUST wait for an interval of time greater than or equal to the lease break acknowledgment timer. This timer MUST be smaller than the client Request Expiration time, as specified in section 3.2.6.1.<220>
```

## Section 3.3.3: Initialization
**Change type:** Modified

### Old Content
```
The server MUST initialize the following:
All the members in ServerStatistics MUST be set to zero.
SnapshotList MUST be set to empty in all shares in ShareList.
ServerEnabled MUST be set to FALSE.
GlobalOpenTable MUST be set to an empty table.
GlobalSessionTable MUST be set to an empty table.
ServerGuid MUST be set to a newly generated GUID.
ConnectionList MUST be set to an empty list.
ServerStartTime SHOULD<220> be set to zero.
IsDfsCapable MUST be set to FALSE.
ServerSideCopyMaxNumberofChunks MUST be set to an implementation-specific<221> default value.
ServerSideCopyMaxChunkSize MUST be set to an implementation-specific<222> default value.
ServerSideCopyMaxDataSize MUST be set to an implementation-specific<223> default value.
ShareList MUST be set to an empty list.
Open.DurableOpenScavengerTimeout MUST be set to zero.
If the server implements the SMB 2.1 or SMB 3.x dialect family, it MUST initialize the following:
ServerHashLevel MUST be set to an implementation-specific<224> default value.
If the server implements the SMB 2.1 or 3.x dialect family and supports leasing, the server MUST initialize the following:
GlobalLeaseTableList MUST be set to an empty list.
If the server implements the SMB 2.1 or SMB 3.x dialect family and supports resiliency, it MUST implement the following:
MaxResiliencyTimeout SHOULD<225> be set to an implementation-specific default value.
If the server implements the SMB 3.x dialect family, the server MUST initialize the following:
GlobalClientTable MUST be set to an empty list.
EncryptData MUST be set in an implementation-specific manner.
RejectUnencryptedAccess MUST be set in an implementation-specific manner.<226>
IsMultiChannelCapable MUST be set in an implementation-specific manner.<227>
AllowAnonymousAccess MUST be set to an implementation-specific<228> default value.
If the server implements the SMB 3.0.2 or SMB 3.1.1 dialect, the server MUST initialize the following:
IsSharedVHDSupported: MUST be set to FALSE.
If the server implements the SMB 3.1.1 dialect, the server MUST initialize the following:
MaxClusterDialect MUST be set in an implementation-specific manner.
Server.SupportsTreeConnectExtn MUST be set in an implementation-specific<229> manner.
AllowNamedPipeAccessOverQUIC MUST be set in an implementation-specific<230> manner.
The server MUST notify the completion of its initialization to the server service by invoking the event as specified in [MS-SRVS] section 3.1.6.14, providing the string "SMB2" as an input parameter.
IsMutualAuthOverQUICSupported MUST be set in an implementation-specific manner.<231>
ServerCertificateMappingTable MUST be initialized based on administrator configuration.
```

### New Content
```
The server MUST initialize the following:
All the members in ServerStatistics MUST be set to zero.
SnapshotList MUST be set to empty in all shares in ShareList.
ServerEnabled MUST be set to FALSE.
GlobalOpenTable MUST be set to an empty table.
GlobalSessionTable MUST be set to an empty table.
ServerGuid MUST be set to a newly generated GUID.
ConnectionList MUST be set to an empty list.
ServerStartTime SHOULD<221> be set to zero.
IsDfsCapable MUST be set to FALSE.
ServerSideCopyMaxNumberofChunks MUST be set to an implementation-specific<222> default value.
ServerSideCopyMaxChunkSize MUST be set to an implementation-specific<223> default value.
ServerSideCopyMaxDataSize MUST be set to an implementation-specific<224> default value.
ShareList MUST be set to an empty list.
Open.DurableOpenScavengerTimeout MUST be set to zero.
If the server implements the SMB 2.1 or SMB 3.x dialect family, it MUST initialize the following:
ServerHashLevel MUST be set to an implementation-specific<225> default value.
If the server implements the SMB 2.1 or 3.x dialect family and supports leasing, the server MUST initialize the following:
GlobalLeaseTableList MUST be set to an empty list.
If the server implements the SMB 2.1 or SMB 3.x dialect family and supports resiliency, it MUST implement the following:
MaxResiliencyTimeout SHOULD<226> be set to an implementation-specific default value.
If the server implements the SMB 3.x dialect family, the server MUST initialize the following:
GlobalClientTable MUST be set to an empty list.
EncryptData MUST be set in an implementation-specific manner.
RejectUnencryptedAccess MUST be set in an implementation-specific manner.<227>
IsMultiChannelCapable MUST be set in an implementation-specific manner.<228>
AllowAnonymousAccess MUST be set to an implementation-specific<229> default value.
If the server implements the SMB 3.0.2 or SMB 3.1.1 dialect, the server MUST initialize the following:
IsSharedVHDSupported: MUST be set to FALSE.
If the server implements the SMB 3.1.1 dialect, the server MUST initialize the following:
MaxClusterDialect MUST be set in an implementation-specific manner.
Server.SupportsTreeConnectExtn MUST be set in an implementation-specific<230> manner.
AllowNamedPipeAccessOverQUIC MUST be set in an implementation-specific<231> manner.
The server MUST notify the completion of its initialization to the server service by invoking the event as specified in [MS-SRVS] section 3.1.6.14, providing the string "SMB2" as an input parameter.
IsMutualAuthOverQUICSupported MUST be set in an implementation-specific manner.<232>
ServerCertificateMappingTable MUST be initialized based on administrator configuration.
```

## Section 3.3.4.1.1: Signing the Message
**Change type:** Modified

### Old Content
```
The server SHOULD<232> sign the message under the following conditions:
If the request was signed by the client, the response message being sent contains a nonzero SessionId and a zero TreeId in the SMB2 header, and the session identified by SessionId has Session.SigningRequired equal to TRUE.
If the request was signed by the client, the response message being sent contains a nonzero SessionId, and a nonzero TreeId in the SMB2 header, and the session identified by SessionId has Session.SigningRequired equal to TRUE, if either global EncryptData is FALSE or Connection.ClientCapabilities does not include the SMB2_GLOBAL_CAP_ENCRYPTION bit.
If the request was signed by the client, and the response is not an interim response to an asynchronously processed request.
If Connection.Dialect belongs to the SMB 3.x dialect family, and if the response being signed is an SMB2 SESSION_SETUP Response without a status code equal to STATUS_SUCCESS in the header, the server MUST use Session.SigningKey. For all other responses being signed the server MUST provide Channel.SigningKey by looking up the Channel in Session.ChannelList, where the connection matches the Channel.Connection.
Otherwise, the server MUST use Session.SessionKey for signing the response.
The server provides the key for signing, the length of the response, and the response itself, and calculates the signature as specified in section 3.1.4.1. If the server signs the message, it MUST set the SMB2_FLAGS_SIGNED bit in the Flags field of the SMB2 header. If the server encrypts the message, as specified in section 3.1.4.3, the server MUST set the Signature field of the SMB2 header to zero.
```

### New Content
```
If the request was not signed by the client, the server MUST set the Signature field of the SMB2 header to zero and skip the processing in this section.
The server SHOULD<233> sign the message under the following conditions:
If the request was signed by the client, the response message being sent contains a nonzero SessionId and a zero TreeId in the SMB2 header, and the session identified by SessionId has Session.SigningRequired equal to TRUE.
If the request was signed by the client, the response message being sent contains a nonzero SessionId, and a nonzero TreeId in the SMB2 header, and the session identified by SessionId has Session.SigningRequired equal to TRUE, if either global EncryptData is FALSE or Connection.ClientCapabilities does not include the SMB2_GLOBAL_CAP_ENCRYPTION bit.
If the request was signed by the client, and the response is not an interim response to an asynchronously processed request.
If Connection.Dialect belongs to the SMB 3.x dialect family, and if the response being signed is an SMB2 SESSION_SETUP Response without a status code equal to STATUS_SUCCESS in the header, the server MUST use Session.SigningKey. For all other responses being signed the server MUST provide Channel.SigningKey by looking up the Channel in Session.ChannelList, where the connection matches the Channel.Connection.
Otherwise, the server MUST use Session.SessionKey for signing the response.
The server provides the key for signing, the length of the response, and the response itself, and calculates the signature as specified in section 3.1.4.1. If the server signs the message, it MUST set the SMB2_FLAGS_SIGNED bit in the Flags field of the SMB2 header.
```

## Section 3.3.4.1.2: Granting Credits to the Client
**Change type:** Modified

### Old Content
```
As described in section 3.3.1.1, the server maintains a list of message identifiers available for incoming requests. The total number of available message identifiers can change dynamically as the system runs, with the server granting credits based on some local policy.
Based on the CreditRequest specified in the SMB2 header of a client request, the server MUST determine how many credits it will grant the client on each request by using a vendor-specific algorithm as specified in section 3.3.1.2. The server MUST then place the number of credits granted in the CreditResponse field in the SMB2 header of the response.
The server consumes one credit for any request except for the SMB2 CANCEL Request. If the server implements the SMB 2.1 or SMB 3.x dialect family and the request is a multi-credit request, the server MUST consume multiple credits as specified in section 3.3.5.2.3. To maintain the same number of credits already granted, the server returns a value equal to the number of credits consumed by this command. To reduce or increase the number of credits granted, the server respectively returns a value less than or greater than the number of credits consumed by this command.
For an asynchronously processed request, any credits to be granted MUST be granted in the interim response, as specified in section 3.3.4.2.<233>
```

### New Content
```
As described in section 3.3.1.1, the server maintains a list of message identifiers available for incoming requests. The total number of available message identifiers can change dynamically as the system runs, with the server granting credits based on some local policy.
Based on the CreditRequest specified in the SMB2 header of a client request, the server MUST determine how many credits it will grant the client on each request by using a vendor-specific algorithm as specified in section 3.3.1.2. The server MUST then place the number of credits granted in the CreditResponse field in the SMB2 header of the response.
The server consumes one credit for any request except for the SMB2 CANCEL Request. If the server implements the SMB 2.1 or SMB 3.x dialect family and the request is a multi-credit request, the server MUST consume multiple credits as specified in section 3.3.5.2.3. To maintain the same number of credits already granted, the server returns a value equal to the number of credits consumed by this command. To reduce or increase the number of credits granted, the server respectively returns a value less than or greater than the number of credits consumed by this command.
For an asynchronously processed request, any credits to be granted MUST be granted in the interim response, as specified in section 3.3.4.2.<234>
```

## Section 3.3.4.1.3: Sending Compounded Responses
**Change type:** Modified

### Old Content
```
The server MAY<234> compound responses to the client.
To compound responses, the server MUST set the NextCommand in the first response to the offset, in bytes, from the beginning of the SMB2 header of the first response to the beginning of the 8-byte aligned SMB2 header in the subsequent response. This process MUST be done for each response except the final response in the chain, whose NextCommand SHOULD<235> be set to 0. The length of the last response in the compounded responses SHOULD be padded to a multiple of 8 bytes. The server MAY<236> grant credits separately on each response in the compounded chain. Then the entire response chain MUST be sent to the client as a single submission to the underlying transport.
The server SHOULD NOT<237> send the response message when the size is greater than Connection.MaxTransactSize+256.
```

### New Content
```
The server MAY<235> compound responses to the client.
To compound responses, the server MUST set the NextCommand in the first response to the offset, in bytes, from the beginning of the SMB2 header of the first response to the beginning of the 8-byte aligned SMB2 header in the subsequent response. This process MUST be done for each response except the final response in the chain, whose NextCommand SHOULD<236> be set to 0. The length of the last response in the compounded responses SHOULD be padded to a multiple of 8 bytes. The server MAY<237> grant credits separately on each response in the compounded chain. Then the entire response chain MUST be sent to the client as a single submission to the underlying transport.
The server SHOULD NOT<238> send the response message when the size is greater than Connection.MaxTransactSize+256.
```

## Section 3.3.4.1.5: Compressing the Message
**Change type:** Modified

### Old Content
```
If Connection.Dialect is 3.1.1, IsCompressionSupported is TRUE, Connection.CompressionIds is not empty, and Request.CompressReply is TRUE, the server SHOULD<238> process the message as specified in section 3.1.4.4, before sending it to the client.
```

### New Content
```
If Connection.Dialect is 3.1.1, IsCompressionSupported is TRUE, Connection.CompressionIds is not empty, and Request.CompressReply is TRUE, the server SHOULD<239> process the message as specified in section 3.1.4.4, before sending it to the client.
```

## Section 3.3.4.2: Sending an Interim Response for an Asynchronous Operation
**Change type:** Modified

### Old Content
```
The server MAY<239> choose to send an interim response for any request that is received. It SHOULD<240> send an interim response for any request that could potentially block for an indefinite amount of time. If an operation would require asynchronous processing but resources are constrained, the server MAY<241> choose to fail that operation with STATUS_INSUFFICIENT_RESOURCES.
An interim response indicates to the client that the request has been received and a full response will come later. The server SHOULD NOT sign an interim response.
To send an interim response for a request, the server MUST generate an asynchronous identifier for it, and Request.AsyncId MUST be set to this asynchronous identifier.
The identifier MUST be an 8-byte value.
The identifier MUST be unique for all outstanding asynchronous requests on a specified SMB2 transport connection.
The identifier MUST remain valid until the final response for the request is sent.
The identifier MUST NOT be reused until the final response is sent.
The identifier MUST be nonzero.
The server MUST insert the Request in Connection.AsyncCommandList.
The server MUST construct a response packet for the request. The SMB2 header of the response MUST be identical to that in the request with the following changes:
It MUST set the Status field in the SMB2 header to STATUS_PENDING.
The NextCommand field MUST be set to 0 if this is not a compounded response. Otherwise, NextCommand MUST be set as specified in section 3.3.4.1.3.
The server MUST set the SMB2_FLAGS_SERVER_TO_REDIR bit in the Flags field of the SMB2 header.
The server MUST set the SMB2_FLAGS_ASYNC_COMMAND bit in the Flags field of the SMB2 header.
It MUST set the AsyncId field of the SMB2 header to the value that was generated earlier.
It MUST set the CreditResponse field to the number of credits the server chooses to grant for this request, as specified in section 3.3.1.2.
It MUST append an SMB2 ERROR Response following the SMB2 header, as specified in section 2.2.2, with a ByteCount of zero. This response MUST be sent to the client.
```

### New Content
```
The server MAY<240> choose to send an interim response for any request that is received. It SHOULD<241> send an interim response for any request that could potentially block for an indefinite amount of time. If an operation would require asynchronous processing but resources are constrained, the server MAY<242> choose to fail that operation with STATUS_INSUFFICIENT_RESOURCES.
An interim response indicates to the client that the request has been received and a full response will come later. The server SHOULD NOT sign an interim response.
To send an interim response for a request, the server MUST generate an asynchronous identifier for it, and Request.AsyncId MUST be set to this asynchronous identifier.
The identifier MUST be an 8-byte value.
The identifier MUST be unique for all outstanding asynchronous requests on a specified SMB2 transport connection.
The identifier MUST remain valid until the final response for the request is sent.
The identifier MUST NOT be reused until the final response is sent.
The identifier MUST be nonzero.
The server MUST insert the Request in Connection.AsyncCommandList.
The server MUST construct a response packet for the request. The SMB2 header of the response MUST be identical to that in the request with the following changes:
It MUST set the Status field in the SMB2 header to STATUS_PENDING.
The NextCommand field MUST be set to 0 if this is not a compounded response. Otherwise, NextCommand MUST be set as specified in section 3.3.4.1.3.
The server MUST set the SMB2_FLAGS_SERVER_TO_REDIR bit in the Flags field of the SMB2 header.
The server MUST set the SMB2_FLAGS_ASYNC_COMMAND bit in the Flags field of the SMB2 header.
It MUST set the AsyncId field of the SMB2 header to the value that was generated earlier.
It MUST set the CreditResponse field to the number of credits the server chooses to grant for this request, as specified in section 3.3.1.2.
It MUST append an SMB2 ERROR Response following the SMB2 header, as specified in section 2.2.2, with a ByteCount of zero. This response MUST be sent to the client.
```

## Section 3.3.4.4: Sending an Error Response
**Change type:** Modified

### Old Content
```
When the server is responding with a failure to any command sent by the client, the response message MUST be constructed as described here. An error code other than one of the following indicates a failure:
STATUS_MORE_PROCESSING_REQUIRED in an SMB2 SESSION_SETUP Response specified in section 2.2.6.
STATUS_BUFFER_OVERFLOW in an SMB2 QUERY_INFO Response specified in section 2.2.38.
STATUS_BUFFER_OVERFLOW in a FSCTL_PIPE_TRANSCEIVE, FSCTL_PIPE_PEEK or FSCTL_DFS_GET_REFERRALS Response specified in section 2.2.32.<242>
STATUS_BUFFER_OVERFLOW in an SMB2 READ Response on a named pipe specified in section 2.2.20.
STATUS_INVALID_PARAMETER in an FSCTL_SRV_COPYCHUNK or FSCTL_SRV_COPYCHUNK_WRITE response, when returning an SRV_COPYCHUNK_RESPONSE as described in section 3.3.5.15.6.2.
STATUS_NOTIFY_ENUM_DIR in an SMB2 CHANGE_NOTIFY Response specified in section 2.2.36.
The server MUST provide the error code of the failure and a data buffer to be returned with the error. If nothing is specified, the buffer MUST be considered to be zero bytes in length.
The server can return any of the following errors if the server, the file, or the share is not ready to process an I/O request from the client.
STATUS_SERVER_UNAVAILABLE
STATUS_FILE_NOT_AVAILABLE
STATUS_SHARE_UNAVAILABLE
The server MUST construct the SMB2 header of the error response to match the SMB2 header of the request with the following changes:
The Status field of the SMB2 header MUST be set to the error code provided.
The NextCommand field MUST be set to 0. If this response is later combined with other responses into a compounded response, as specified in section 3.3.4.1.3, this value will change later.
The SMB2_FLAGS_SERVER_TO_REDIR bit MUST be set in the Flags field of the SMB2 header.
If Request.AsyncId is nonzero, the server MUST set the AsyncId field to it, and MUST set the SMB2_FLAGS_ASYNC_COMMAND bit in the Flags field, and MUST set the CreditResponse field to 0.
Otherwise, the server MUST set the CreditResponse field to the number of credits the server chooses to grant the request, as specified in section 3.3.1.2.
Following the SMB2 header MUST be an SMB2 ERROR Response structure, as specified in section 2.2.2.
If Connection.Dialect is "3.1.1", the server MUST construct an SMB2 ERROR Response structure as follows:
The ErrorContextCount of this response MUST be set to the number of SMB2 ERROR Context structures to be set in the ErrorData array of the response.
The ByteCount of this response MUST be set to the length of the buffer that is provided as part of the error.
If ErrorContextCount is greater than zero, the server MUST format the ErrorData array of the response as a variable-length array of SMB2 ERROR Context structures as specified in section 2.2.2.1.
Otherwise, the server MUST construct an SMB2 ERROR Response structure as follows:
The ErrorContextCount of this response MUST be set to 0.
The ByteCount of this response MUST be set to the length of the buffer that is provided as part of the error.
If ByteCount is greater than zero, the server MUST format the ErrorData array of the response as described in section 2.2.2.2.
This response MUST then be sent to the client, and the request MUST be removed from Connection.RequestList and freed.
```

### New Content
```
When the server is responding with a failure to any command sent by the client, the response message MUST be constructed as described here. An error code other than one of the following indicates a failure:
STATUS_MORE_PROCESSING_REQUIRED in an SMB2 SESSION_SETUP Response specified in section 2.2.6.
STATUS_BUFFER_OVERFLOW in an SMB2 QUERY_INFO Response specified in section 2.2.38.
STATUS_BUFFER_OVERFLOW in a FSCTL_PIPE_TRANSCEIVE, FSCTL_PIPE_PEEK or FSCTL_DFS_GET_REFERRALS Response specified in section 2.2.32.<243>
STATUS_BUFFER_OVERFLOW in an SMB2 READ Response on a named pipe specified in section 2.2.20.
STATUS_INVALID_PARAMETER in an FSCTL_SRV_COPYCHUNK or FSCTL_SRV_COPYCHUNK_WRITE response, when returning an SRV_COPYCHUNK_RESPONSE as described in section 3.3.5.15.6.2.
STATUS_NOTIFY_ENUM_DIR in an SMB2 CHANGE_NOTIFY Response specified in section 2.2.36.
The server MUST provide the error code of the failure and a data buffer to be returned with the error. If nothing is specified, the buffer MUST be considered to be zero bytes in length.
The server can return any of the following errors if the server, the file, or the share is not ready to process an I/O request from the client.
STATUS_SERVER_UNAVAILABLE
STATUS_FILE_NOT_AVAILABLE
STATUS_SHARE_UNAVAILABLE
The server MUST construct the SMB2 header of the error response to match the SMB2 header of the request with the following changes:
The Status field of the SMB2 header MUST be set to the error code provided.
The NextCommand field MUST be set to 0. If this response is later combined with other responses into a compounded response, as specified in section 3.3.4.1.3, this value will change later.
The SMB2_FLAGS_SERVER_TO_REDIR bit MUST be set in the Flags field of the SMB2 header.
If Request.AsyncId is nonzero, the server MUST set the AsyncId field to it, and MUST set the SMB2_FLAGS_ASYNC_COMMAND bit in the Flags field, and MUST set the CreditResponse field to 0.
Otherwise, the server MUST set the CreditResponse field to the number of credits the server chooses to grant the request, as specified in section 3.3.1.2.
Following the SMB2 header MUST be an SMB2 ERROR Response structure, as specified in section 2.2.2.
If Connection.Dialect is "3.1.1", the server MUST construct an SMB2 ERROR Response structure as follows:
The ErrorContextCount of this response MUST be set to the number of SMB2 ERROR Context structures to be set in the ErrorData array of the response.
The ByteCount of this response MUST be set to the length of the buffer that is provided as part of the error.
If ErrorContextCount is greater than zero, the server MUST format the ErrorData array of the response as a variable-length array of SMB2 ERROR Context structures as specified in section 2.2.2.1.
Otherwise, the server MUST construct an SMB2 ERROR Response structure as follows:
The ErrorContextCount of this response MUST be set to 0.
The ByteCount of this response MUST be set to the length of the buffer that is provided as part of the error.
If ByteCount is greater than zero, the server MUST format the ErrorData array of the response as described in section 2.2.2.2.
This response MUST then be sent to the client, and the request MUST be removed from Connection.RequestList and freed.
```

## Section 3.3.4.6: Object Store Indicates an Oplock Break
**Change type:** Modified

### Old Content
```
The underlying object store on the local resource indicates the breaking of an opportunistic lock, specifying the LocalOpen and the new oplock level, a status code of the oplock break, and optionally expects the new oplock level in return. The new oplock level SHOULD<243> be SMB2_OPLOCK_LEVEL_NONE or SMB2_OPLOCK_LEVEL_II or SMB2_OPLOCK_LEVEL_EXCLUSIVE. The conditions under which each oplock level is to be indicated are described in [MS-FSA] section 2.1.5.18.3.
The server MUST locate the open by walking the GlobalOpenTable to find an entry whose Open.LocalOpen matches the one provided in the oplock break. If no entry is found, the break indication MUST be ignored and the server MUST complete the oplock break call with SMB2_OPLOCK_LEVEL_NONE as the new oplock level.
If an entry is found, the server MUST perform the following:
For the specified Open, the server MUST select the connection as specified in section 3.3.4.1.6. If no connection is available, Open.IsResilient is FALSE, Open.IsDurable is FALSE, and Open.IsPersistent is FALSE, the server SHOULD close the Open as specified in section 3.3.4.17.
The server MUST construct an Oplock Break Notification following the syntax specified in section 2.2.23.1 to send back to the client. The server MUST set the Command in the SMB2 header to SMB2 OPLOCK_BREAK, and the MessageId to 0xFFFFFFFFFFFFFFFF. The server SHOULD<244> set the SessionId in the SMB2 header to Open.Session.SessionId. The server MUST set the TreeId in the SMB2 header to zero. The FileId field of the response structure MUST be set to the values from the Open structure, with the volatile part set to Open.FileId and the persistent part set to Open.DurableFileId. The oplock Level of the response MUST be set to the value provided by the object store. The server MUST set Open.OplockState to Breaking and set Open.OplockTimeout to the current time plus an implementation-specific default value in milliseconds.<245> The message SHOULD NOT be signed.
If the server implements the SMB 3.x dialect family, SMB2 Oplock Break Notification MUST be sent to the client using the first available connection in Open.Session.ChannelList where Channel.Connection is not NULL. If the server fails to send the notification to the client, the server MUST retry the send using an alternate connection, if available, in Open.Session.ChannelList.
Otherwise, SMB2 Oplock Break Notification MUST be sent to the client using Open.Connection.
If the notification could not be sent on any connection, the server MUST complete the oplock break from the underlying object store with SMB2_OPLOCK_LEVEL_NONE as the new oplock level and MUST set Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE and Open.OplockState to None.
If the server succeeds in sending the notification, the server MUST start the oplock break acknowledgment timer as specified in section 3.3.2.1.
```

### New Content
```
The underlying object store on the local resource indicates the breaking of an opportunistic lock, specifying the LocalOpen and the new oplock level, a status code of the oplock break, and optionally expects the new oplock level in return. The new oplock level SHOULD<244> be SMB2_OPLOCK_LEVEL_NONE or SMB2_OPLOCK_LEVEL_II or SMB2_OPLOCK_LEVEL_EXCLUSIVE. The conditions under which each oplock level is to be indicated are described in [MS-FSA] section 2.1.5.18.3.
The server MUST locate the open by walking the GlobalOpenTable to find an entry whose Open.LocalOpen matches the one provided in the oplock break. If no entry is found, the break indication MUST be ignored and the server MUST complete the oplock break call with SMB2_OPLOCK_LEVEL_NONE as the new oplock level.
If an entry is found, the server MUST perform the following:
For the specified Open, the server MUST select the connection as specified in section 3.3.4.1.6. If no connection is available, Open.IsResilient is FALSE, Open.IsDurable is FALSE, and Open.IsPersistent is FALSE, the server SHOULD close the Open as specified in section 3.3.4.17.
The server MUST construct an Oplock Break Notification following the syntax specified in section 2.2.23.1 to send back to the client. The server MUST set the Command in the SMB2 header to SMB2 OPLOCK_BREAK, and the MessageId to 0xFFFFFFFFFFFFFFFF. The server SHOULD<245> set the SessionId in the SMB2 header to Open.Session.SessionId. The server MUST set the TreeId in the SMB2 header to zero. The FileId field of the response structure MUST be set to the values from the Open structure, with the volatile part set to Open.FileId and the persistent part set to Open.DurableFileId. The oplock Level of the response MUST be set to the value provided by the object store. The server MUST set Open.OplockState to Breaking and set Open.OplockTimeout to the current time plus an implementation-specific default value in milliseconds.<246> The message SHOULD NOT be signed.
If the server implements the SMB 3.x dialect family, SMB2 Oplock Break Notification MUST be sent to the client using the first available connection in Open.Session.ChannelList where Channel.Connection is not NULL. If the server fails to send the notification to the client, the server MUST retry the send using an alternate connection, if available, in Open.Session.ChannelList.
Otherwise, SMB2 Oplock Break Notification MUST be sent to the client using Open.Connection.
If Open.IsDurable is TRUE and the notification could not be sent on any connection, the server MUST complete the oplock break from the underlying object store with SMB2_OPLOCK_LEVEL_NONE as the new oplock level and MUST set Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE and Open.OplockState to None. The server MUST close the Open as specified in section 3.3.4.17.
If the server succeeds in sending the notification, the server MUST start the oplock break acknowledgment timer as specified in section 3.3.2.1.
```

## Section 3.3.4.7: Object Store Indicates a Lease Break
**Change type:** Modified

### Old Content
```
The underlying object store indicates the breaking of a lease by specifying the ClientGuid, the ClientLeaseId, and the new lease state. The new lease state MUST be one of NONE, R, RW, and RH.
When the underlying object store indicates the lease break, the server MUST locate the Lease Table by performing a lookup in GlobalLeaseTableList using the provided ClientGuid as the lookup key, and then locate the Lease entry by performing a lookup in the LeaseTable.LeaseList using the provided ClientLeaseId as the lookup key.
If no entry is found, the server MUST complete the lease break call from the underlying object store with "NONE" as the new lease state, and take no further action.
If a Lease entry is found, the server MUST perform the following:
If Lease.LeaseOpens is empty, the server MUST complete the lease break call from the underlying object store with "NONE" as the new lease state, set Lease.LeaseState to "NONE", and take no further action.
If no connection is available among all Opens in Lease.LeaseOpens, the server MUST close every Open as specified in section 3.3.4.17 in one of the following cases:
Open.IsDurable, Open.IsResilient, and Open.IsPersistent are all FALSE.
The new lease state indicated by the object store does not contain SMB2_LEASE_HANDLE_CACHING and Open.IsDurable is TRUE.
Otherwise, the server MUST construct a Lease Break Notification (section 2.2.23.2) message to send to the client.
The server MUST set the Command field in the SMB2 header to SMB2 OPLOCK_BREAK, and the MessageId field to 0xFFFFFFFFFFFFFFFF. The server MUST set the SessionId and TreeId fields in the SMB2 header to 0.
If Lease.LeaseState is SMB2_LEASE_READ_CACHING, the server MUST set the Flags field of the message to zero and MUST set Open.OplockState to “None” for all opens in Lease.LeaseOpens. The server MUST set Lease.Breaking to FALSE, and the LeaseKey field MUST be set to Lease.LeaseKey.
Otherwise, the server MUST set the Flags field of the message to SMB2_NOTIFY_BREAK_LEASE_FLAG_ACK_REQUIRED, indicating to the client that lease acknowledgment is required. The LeaseKey field MUST be set to Lease.LeaseKey. The server MUST set Open.OplockState to “Breaking” for all Opens in Lease.LeaseOpens. The server MUST set the CurrentLeaseState field of the message to Lease.LeaseState, set Lease.Breaking to TRUE, set Lease.BreakToLeaseState and the NewLeaseState field to the new lease state indicated by the object store, and set Lease.LeaseBreakTimeout to the current time plus an implementation-specific<246> default value in milliseconds.
If the server implements the SMB 3.x dialect family and Lease.Version is 2, the server SHOULD<247> set NewEpoch to Lease.Epoch + 1. Otherwise, NewEpoch MUST be set to zero. The server MUST set Lease.Epoch to NewEpoch.
The message SHOULD NOT be signed. The server MUST set Lease.BreakNotification to the newly constructed Lease Break Notification.
The server MUST look up all the connections in ConnectionList where Connection.ClientGuid matches the provided ClientGuid. The server MUST send Lease.BreakNotification using the first available connection. If the server fails to send the notification to the client, the server MUST retry the send using an alternate connection available.
If the server succeeds in sending the Lease Break Notification, the server MUST set Lease.BreakNotification to empty and MUST start the lease break acknowledgment timer as specified in section 3.3.2.5.
Otherwise, the server MUST perform the following steps:
If Open.IsPersistent is TRUE and Lease.LeaseState is not SMB2_LEASE_READ_CACHING, the server MUST take no further action.
Otherwise, the server MUST set Open.Lease.Breaking to FALSE, Lease.Held to FALSE, Open.OplockState to None, Lease.BreakNotification to empty, and MUST complete the lease break call from the underlying object store with "NONE" as the new lease state.
```

### New Content
```
The underlying object store indicates the breaking of a lease by specifying the ClientGuid, the ClientLeaseId, and the new lease state. The new lease state MUST be one of NONE, R, RW, and RH.
When the underlying object store indicates the lease break, the server MUST locate the Lease Table by performing a lookup in GlobalLeaseTableList using the provided ClientGuid as the lookup key, and then locate the Lease entry by performing a lookup in the LeaseTable.LeaseList using the provided ClientLeaseId as the lookup key.
If no entry is found, the server MUST complete the lease break call from the underlying object store with "NONE" as the new lease state, and take no further action.
If a Lease entry is found, the server MUST perform the following:
If Lease.LeaseOpens is empty, the server MUST complete the lease break call from the underlying object store with "NONE" as the new lease state, set Lease.LeaseState to "NONE", and take no further action.
If no connection is available among all Opens in Lease.LeaseOpens, the server MUST close every Open as specified in section 3.3.4.17 in one of the following cases:
Open.IsDurable, Open.IsResilient, and Open.IsPersistent are all FALSE.
The new lease state indicated by the object store does not contain SMB2_LEASE_HANDLE_CACHING and Open.IsDurable is TRUE.
Otherwise, the server MUST construct a Lease Break Notification (section 2.2.23.2) message to send to the client.
The server MUST set the Command field in the SMB2 header to SMB2 OPLOCK_BREAK, and the MessageId field to 0xFFFFFFFFFFFFFFFF. The server MUST set the SessionId and TreeId fields in the SMB2 header to 0.
If Lease.LeaseState is SMB2_LEASE_READ_CACHING, the server MUST set the Flags field of the message to zero and MUST set Open.OplockState to “None” for all opens in Lease.LeaseOpens. The server MUST set Lease.Breaking to FALSE, and the LeaseKey field MUST be set to Lease.LeaseKey.
Otherwise, the server MUST set the Flags field of the message to SMB2_NOTIFY_BREAK_LEASE_FLAG_ACK_REQUIRED, indicating to the client that lease acknowledgment is required. The LeaseKey field MUST be set to Lease.LeaseKey. The server MUST set Open.OplockState to “Breaking” for all Opens in Lease.LeaseOpens. The server MUST set the CurrentLeaseState field of the message to Lease.LeaseState, set Lease.Breaking to TRUE, set Lease.BreakToLeaseState and the NewLeaseState field to the new lease state indicated by the object store, and set Lease.LeaseBreakTimeout to the current time plus an implementation-specific<247> default value in milliseconds.
If the server implements the SMB 3.x dialect family and Lease.Version is 2, the server SHOULD<248> set NewEpoch to Lease.Epoch + 1. Otherwise, NewEpoch MUST be set to zero. The server MUST set Lease.Epoch to NewEpoch.
The message SHOULD NOT be signed. The server MUST set Lease.BreakNotification to the newly constructed Lease Break Notification.
The server MUST look up all the connections in ConnectionList where Connection.ClientGuid matches the provided ClientGuid. The server MUST send Lease.BreakNotification using the first available connection. If the server fails to send the notification to the client, the server MUST retry the send using an alternate connection available.
If the server succeeds in sending the Lease Break Notification, the server MUST set Lease.BreakNotification to empty and MUST start the lease break acknowledgment timer as specified in section 3.3.2.5.
Otherwise, the server MUST perform the following steps:
If Open.IsPersistent is TRUE and Lease.LeaseState is not SMB2_LEASE_READ_CACHING, the server MUST take no further action.
Otherwise, the server MUST set Open.Lease.Breaking to FALSE, Lease.Held to FALSE, Open.OplockState to None, Lease.BreakNotification to empty, and MUST complete the lease break call from the underlying object store with "NONE" as the new lease state.
```

## Section 3.3.4.13: Server Application Registers a Share
**Change type:** Modified

### Old Content
```
The calling application provides a share in SHARE_INFO_503_I structure as specified in [MS-SRVS] section 2.2.4.27 to register a share. The server MUST validate the SHARE_INFO_503_I structure as specified in [MS-SRVS] section 3.1.4.7. If any member in the structure is invalid, the server MUST return STATUS_INVALID_PARAMETER to the calling application. The server MUST look up the Share in the ShareList, where shi503_servername matches Share.ServerName and shi503_netname matches Share.Name. If a matching Share is found, the server MUST fail the call with an implementation-dependent error. Otherwise, the server MUST create a new Share with the following value set and insert it into ShareList and return STATUS_SUCCESS.
Share.Name MUST be set to shi503_netname.
Share.Type MUST be set to shi503_type. The server SHOULD<248> set STYPE_CLUSTER_FS, STYPE_CLUSTER_SOFS, and STYPE_CLUSTER_DFS as specified in [MS-SRVS] section 2.2.2.4 in an implementation-defined manner.
Share.Remark MUST be set to shi503_remark.
Share.LocalPath MUST be set to shi503_path.
Share.ServerName MUST be set to the shi503_servername.
Share.FileSecurity MUST be set to shi503_security_descriptor.
Share.MaxUses MUST be set to shi503_max_uses.
Share.CurrentUses MUST be set to 0.
Share.CscFlags MUST be set to 0.
Share.IsDfs MUST be set to FALSE.
Share.DoAccessBasedDirectoryEnumeration MUST be set to FALSE.
Share.AllowNamespaceCaching MUST be set to FALSE.
Share.ForceSharedDelete MUST be set to FALSE.
Share.RestrictExclusiveOpens MUST be set to FALSE.
Share.ForceLevel2Oplock MUST be set to FALSE.
Share.HashEnabled MUST be set to FALSE.
If the server implements the SMB 3.x dialect family, Share.EncryptData MUST be set to FALSE.
If the server implements the SMB 3.1.1 dialect, Share.CompressData MUST be set to FALSE.
If Share.Name is equal to "IPC$" or Share.Type does not have the STYPE_SPECIAL bit set, as specified in [MS-SRVS] section 2.2.2.4, then Share.ConnectSecurity SHOULD be set to a security descriptor allowing all users. Otherwise, Share.ConnectSecurity SHOULD be set to a security descriptor allowing only administrators.
If the server implements the SMB 3.x dialect family, Share.CATimeout MUST be set to an implementation-specific value.<249>
```

### New Content
```
The calling application provides a share in SHARE_INFO_503_I structure as specified in [MS-SRVS] section 2.2.4.27 to register a share. The server MUST validate the SHARE_INFO_503_I structure as specified in [MS-SRVS] section 3.1.4.7. If any member in the structure is invalid, the server MUST return STATUS_INVALID_PARAMETER to the calling application. The server MUST look up the Share in the ShareList, where shi503_servername matches Share.ServerName and shi503_netname matches Share.Name. If a matching Share is found, the server MUST fail the call with an implementation-dependent error. Otherwise, the server MUST create a new Share with the following value set and insert it into ShareList and return STATUS_SUCCESS.
Share.Name MUST be set to shi503_netname.
Share.Type MUST be set to shi503_type. The server SHOULD<249> set STYPE_CLUSTER_FS, STYPE_CLUSTER_SOFS, and STYPE_CLUSTER_DFS as specified in [MS-SRVS] section 2.2.2.4 in an implementation-defined manner.
Share.Remark MUST be set to shi503_remark.
Share.LocalPath MUST be set to shi503_path.
Share.ServerName MUST be set to the shi503_servername.
Share.FileSecurity MUST be set to shi503_security_descriptor.
Share.MaxUses MUST be set to shi503_max_uses.
Share.CurrentUses MUST be set to 0.
Share.CscFlags MUST be set to 0.
Share.IsDfs MUST be set to FALSE.
Share.DoAccessBasedDirectoryEnumeration MUST be set to FALSE.
Share.AllowNamespaceCaching MUST be set to FALSE.
Share.ForceSharedDelete MUST be set to FALSE.
Share.RestrictExclusiveOpens MUST be set to FALSE.
Share.ForceLevel2Oplock MUST be set to FALSE.
Share.HashEnabled MUST be set to FALSE.
If the server implements the SMB 3.x dialect family, Share.EncryptData MUST be set to FALSE.
If the server implements the SMB 3.1.1 dialect, Share.CompressData MUST be set to FALSE.
If Share.Name is equal to "IPC$" or Share.Type does not have the STYPE_SPECIAL bit set, as specified in [MS-SRVS] section 2.2.2.4, then Share.ConnectSecurity SHOULD be set to a security descriptor allowing all users. Otherwise, Share.ConnectSecurity SHOULD be set to a security descriptor allowing only administrators.
If the server implements the SMB 3.x dialect family, Share.CATimeout MUST be set to an implementation-specific value.<250>
```

## Section 3.3.4.17: Server Application Requests Closing an Open
**Change type:** Modified

### Old Content
```
The calling application provides GlobalFileId as input parameter. The server MUST look up Open in GlobalOpenTable where Open.FileGlobalId is equal to GlobalFileId, and, if the Open is found, the server MUST perform the following:
Remove the Open from the GlobalOpenTable.
If Open.Connection is not NULL, cancel all requests in Open.Connection.RequestList for which Request.Open matches the Open, as specified in section 3.3.5.16.
If Open.IsSharedVHDX is TRUE, close the underlying Open.LocalOpen as specified in [MS-RSVD] section 3.2.5.2.
Close the underlying Open.LocalOpen.
If Open.Session is not NULL, remove the Open from Open.Session.OpenTable.
If Open.TreeConnect is not NULL, decrease Open.TreeConnect.OpenCount by 1.
If Open.Connection.Dialect is not "2.0.2", the server supports leasing, and Open.Lease is not NULL:
The server MUST identify a LeaseTable by enumerating each entry in GlobalLeaseTableList to find the one whose LeaseTable.LeaseList contains Open.Lease.
The server MUST then remove the Open from Open.Lease.LeaseOpens. If this Open is the last open in Open.Lease.LeaseOpens, the server MUST set Open.Lease.Held to FALSE.
If Open.Lease.Held is FALSE:
If Open.Lease.Breaking is TRUE, the server MUST complete the lease break to the underlying object store with NONE as the new lease state. <250>
The server MUST remove the Open.Lease from the LeaseTable.LeaseList and free the Open.Lease.
If LeaseTable.LeaseList is now empty, the server MAY remove the LeaseTable from the GlobalLeaseTableList and free the LeaseTable.
Provide Open.FileGlobalId as the input parameter and deregister the Open by invoking the event specified in [MS-SRVS] section 3.1.6.5.
The Open object is then freed.
Return STATUS_SUCCESS to the calling application.
If no Open is found, the call MUST return an implementation-dependent error.
```

### New Content
```
The calling application provides GlobalFileId as input parameter. The server MUST look up Open in GlobalOpenTable where Open.FileGlobalId is equal to GlobalFileId, and, if the Open is found, the server MUST perform the following:
Remove the Open from the GlobalOpenTable.
If Open.Connection is not NULL, cancel all requests in Open.Connection.RequestList for which Request.Open matches the Open, as specified in section 3.3.5.16.
If Open.IsSharedVHDX is TRUE, close the underlying Open.LocalOpen as specified in [MS-RSVD] section 3.2.5.2.
Close the underlying Open.LocalOpen.
If Open.Session is not NULL, remove the Open from Open.Session.OpenTable.
If Open.TreeConnect is not NULL, decrease Open.TreeConnect.OpenCount by 1.
If Open.Connection.Dialect is not "2.0.2", the server supports leasing, and Open.Lease is not NULL:
The server MUST identify a LeaseTable by enumerating each entry in GlobalLeaseTableList to find the one whose LeaseTable.LeaseList contains Open.Lease.
The server MUST then remove the Open from Open.Lease.LeaseOpens. If this Open is the last open in Open.Lease.LeaseOpens, the server MUST set Open.Lease.Held to FALSE.
If Open.Lease.Held is FALSE:
If Open.Lease.Breaking is TRUE, the server MUST complete the lease break to the underlying object store with NONE as the new lease state. <251>
The server MUST remove the Open.Lease from the LeaseTable.LeaseList and free the Open.Lease.
If LeaseTable.LeaseList is now empty, the server MAY remove the LeaseTable from the GlobalLeaseTableList and free the LeaseTable.
The server MUST send an SMB2 CHANGE_NOTIFY Response, as specified in section 2.2.36, with STATUS_NOTIFY_CLEANUP status code for any pending CHANGE_NOTIFY request associated with the Open that is closed.
Provide Open.FileGlobalId as the input parameter and deregister the Open by invoking the event specified in [MS-SRVS] section 3.1.6.5.
The Open object is then freed.
Return STATUS_SUCCESS to the calling application.
If no Open is found, the call MUST return an implementation-dependent error.
```

## Section 3.3.4.21: Server Application Requests Transport Binding Change
**Change type:** Modified

### Old Content
```
The application provides:
TransportName: A string containing an implementation-specific name of the transport.
ServerName: An optional string containing the name of the server to be used for binding the transport.
EnableFlag: A Boolean flag indicating whether to enable or disable the transport.
The server MUST use implementation-specific<251> means to determine whether TransportName is an eligible transport entry as specified in section 2.1, and if not, the server MUST return ERROR_NOT_SUPPORTED to the caller.
If EnableFlag is TRUE, the server SHOULD obtain binding information for the transport from the appropriate standards assignments as specified in section 1.9 and ServerName <252>and MUST attempt to start listening on the requested transport endpoint.
If EnableFlag is FALSE, the server MUST attempt to stop listening on the transport indicated by TransportName.
If the attempt to start or stop listening on the transport succeeds, the server MUST return STATUS_SUCCESS to the caller. Otherwise, it MUST return an implementation-dependent error.
```

### New Content
```
The application provides:
TransportName: A string containing an implementation-specific name of the transport.
ServerName: An optional string containing the name of the server to be used for binding the transport.
EnableFlag: A Boolean flag indicating whether to enable or disable the transport.
The server MUST use implementation-specific<252> means to determine whether TransportName is an eligible transport entry as specified in section 2.1, and if not, the server MUST return ERROR_NOT_SUPPORTED to the caller.
If EnableFlag is TRUE, the server SHOULD obtain binding information for the transport from the appropriate standards assignments as specified in section 1.9 and ServerName <253>and MUST attempt to start listening on the requested transport endpoint.
If EnableFlag is FALSE, the server MUST attempt to stop listening on the transport indicated by TransportName.
If the attempt to start or stop listening on the transport succeeds, the server MUST return STATUS_SUCCESS to the caller. Otherwise, it MUST return an implementation-dependent error.
```

## Section 3.3.5.1: Accepting an Incoming Connection
**Change type:** Modified

### Old Content
```
If ServerEnabled is FALSE, the server MUST NOT accept any incoming connections.
If IsMutualAuthOverQUICSupported is TRUE and the server receives a connection attempt from the client over QUIC, the server MUST send a certificate chain to the client to be authenticated. The server MUST look up a ServerCertificateMappingEntry in the ServerCertificateMappingTable with ServerCertificateMappingEntry.ServerName matching the server name that QUIC is connected to. If the entry is not found, the server MUST terminate the connection. If the entry is found, the server MUST send ServerCertificateMappingEntry.Certificate and ServerCertificateMappingEntry.RequireClientAuthentication to QUIC and accept the QUIC connection.
If ServerCertificateMappingEntry.RequireClientAuthentication is TRUE, the server MUST authenticate the client in addition to the client authenticating the server. QUIC will not allow the connection to be established unless the client presents a valid and trusted certificate chain to the server.
During a connection attempt over QUIC, QUIC notifies SMB server of the client certificate validation results. If the validation fails, the server MUST terminate the connection. If the validation succeeds and ServerCertificateMappingEntry.SkipClientCertificateAccessCheck is TRUE, the server MUST notify QUIC that the connection MUST be established. If the validation succeeds and ServerCertificateMappingEntry.SkipClientCertificateAccessCheck is FALSE, the server MUST perform the access check algorithm specified in section 3.3.1.18. If the client is denied access to the server, the server MUST pass the access_denied(49) TLS alert code, as specified in [RFC8446], to QUIC and MUST terminate the connection. If the access check fails, the server MUST pass the internal_error(80) TLS alert code to QUIC and MUST terminate the connection. If the access check succeeds, the server MUST establish a connection over QUIC.
When the server accepts an incoming connection from any of its registered transports, it MUST allocate a Connection object for it. The Connection object is initialized as described here.
Connection.CommandSequenceWindow is set to a sequence window, as specified in section 3.3.1.1, with a starting receive sequence of 0 and a window size of 1.
Connection.AsyncCommandList is set to an empty list.
Connection.RequestList is set to an empty list.
Connection.ClientCapabilities is set to 0.
Connection.NegotiateDialect is set to 0xFFFF.
Connection.Dialect is set to "Unknown".
Connection.ShouldSign is set to FALSE.
Connection.ClientName is set to be a null-terminated Unicode string of an IP address if the connection is on TCP port 445, or a NetBIOS host name if the connection is on TCP port 139.
Connection.MaxTransactSize is set to 0.
Connection.SupportsMultiCredit is set to FALSE.
Connection.TransportName is set to the implementation-specific name of the transport used by this connection <253> as obtained by implementation-specific means from the transport that indicated the incoming connection.
Connection.SessionTable MUST be set to an empty table.
Connection.CreationTime is set to the current time.
Connection.ConstrainedConnection, if implemented, MUST be set to TRUE.
Connection.CompressionIds, if implemented, MUST be set to an empty list.
Connection.ServerCertificateMappingEntry MUST be set to ServerCertificateMappingEntry used in QUIC connection establishment.
The server MUST invoke the event specified in [MS-SRVS] section 3.1.6.16 to update the connection count by providing the tuple <Connection.TransportName,TRUE>.
This connection MUST be inserted into the global ConnectionList.
```

### New Content
```
If ServerEnabled is FALSE, the server MUST NOT accept any incoming connections.
If IsMutualAuthOverQUICSupported is TRUE and the server receives a connection attempt from the client over QUIC, the server MUST send a certificate chain to the client to be authenticated. The server MUST look up a ServerCertificateMappingEntry in the ServerCertificateMappingTable with ServerCertificateMappingEntry.ServerName matching the server name that QUIC is connected to. If the entry is not found, the server MUST terminate the connection. If the entry is found, the server MUST send ServerCertificateMappingEntry.Certificate and ServerCertificateMappingEntry.RequireClientAuthentication to QUIC and accept the QUIC connection.
If ServerCertificateMappingEntry.RequireClientAuthentication is TRUE, the server MUST authenticate the client in addition to the client authenticating the server. QUIC will not allow the connection to be established unless the client presents a valid and trusted certificate chain to the server.
During a connection attempt over QUIC, QUIC notifies SMB server of the client certificate validation results. If the validation fails, the server MUST terminate the connection. If the validation succeeds and ServerCertificateMappingEntry.SkipClientCertificateAccessCheck is TRUE, the server MUST notify QUIC that the connection MUST be established. If the validation succeeds and ServerCertificateMappingEntry.SkipClientCertificateAccessCheck is FALSE, the server MUST perform the access check algorithm specified in section 3.3.1.18. If the client is denied access to the server, the server MUST pass the access_denied(49) TLS alert code, as specified in [RFC8446], to QUIC and MUST terminate the connection. If the access check fails, the server MUST pass the internal_error(80) TLS alert code to QUIC and MUST terminate the connection. If the access check succeeds, the server MUST establish a connection over QUIC.
When the server accepts an incoming connection from any of its registered transports, it MUST allocate a Connection object for it. The Connection object is initialized as described here.
Connection.CommandSequenceWindow is set to a sequence window, as specified in section 3.3.1.1, with a starting receive sequence of 0 and a window size of 1.
Connection.AsyncCommandList is set to an empty list.
Connection.RequestList is set to an empty list.
Connection.ClientCapabilities is set to 0.
Connection.NegotiateDialect is set to 0xFFFF.
Connection.Dialect is set to "Unknown".
Connection.ShouldSign is set to FALSE.
Connection.ClientName is set to be a null-terminated Unicode string of an IP address if the connection is on TCP port 445, or a NetBIOS host name if the connection is on TCP port 139.
Connection.MaxTransactSize is set to 0.
Connection.SupportsMultiCredit is set to FALSE.
Connection.TransportName is set to the implementation-specific name of the transport used by this connection <254> as obtained by implementation-specific means from the transport that indicated the incoming connection.
Connection.SessionTable MUST be set to an empty table.
Connection.CreationTime is set to the current time.
Connection.ConstrainedConnection, if implemented, MUST be set to TRUE.
Connection.CompressionIds, if implemented, MUST be set to an empty list.
Connection.ServerCertificateMappingEntry MUST be set to ServerCertificateMappingEntry used in QUIC connection establishment.
The server MUST invoke the event specified in [MS-SRVS] section 3.1.6.16 to update the connection count by providing the tuple <Connection.TransportName,TRUE>.
This connection MUST be inserted into the global ConnectionList.
```

## Section 3.3.5.2.1.1: Decrypting the Message
**Change type:** Modified

### Old Content
```
This section is applicable for only the SMB 3.x dialect family.<257>
If IsEncryptionSupported is TRUE and Connection.CipherId is not zero, the server MUST perform the following:
If the size of the message received from the client is not greater than the size of the SMB2 TRANSFORM_HEADER as specified in section 2.2.41, the server MUST disconnect the connection as specified in section 3.3.7.1.
If the Flags/EncryptionAlgorithm in the SMB2 TRANSFORM_HEADER is not 0x0001, the server MUST disconnect the connection as specified in section 3.3.7.1.
The server MUST look up the session in the Connection.SessionTable using the SessionId in the SMB2 TRANSFORM_HEADER of the request. If the session is not found, the server MUST disconnect the connection as specified in section 3.3.7.1.
If Connection.ConstrainedConnection is set to TRUE and the request is encrypted, then the server MUST disconnect the connection as specified in section 3.3.7.1.
If Connection.ConstrainedConnection is set to FALSE, Session.IsAnonymous or Session.IsGuest is set to TRUE and the request is encrypted, then the server SHOULD<258> disconnect the connection as specified in section 3.3.7.1.
The server MUST decrypt the message using Session.DecryptionKey. If Connection.Dialect is less than "3.1.1", then AES-128-CCM MUST be used, as specified in [RFC4309]. Otherwise, the algorithm specified by the Connection.CipherId MUST be used. The server passes in the Nonce, OriginalMessageSize, Flags/EncryptionAlgorithm, and SessionId fields of the SMB2 TRANSFORM_HEADER as the Optional Authenticated Data input for the algorithm. If decryption succeeds, the server MUST compare the signature in the SMB2 TRANSFORM_HEADER with the signature returned by the decryption algorithm. If the signature verification fails, the server MUST disconnect the connection as specified in section 3.3.7.1. If the signature verification succeeds, the server MUST continue processing the decrypted packet.
If the OriginalMessageSize field in the SMB2 TRANSFORM_HEADER is not equal to the size of the decrypted message, the server SHOULD<259> disconnect the connection as specified in section 3.3.7.1.
If ProtocolId in the header of the decrypted message is 0x424D53FC indicating a nested compressed message, IsCompressionSupported is TRUE, and Connection.CompressionIds is not empty, the server MUST decompress the message as specified in section 3.3.5.2.1.2. If decompression succeeds, the server MUST further validate the message:
The server MUST verify if any of the following conditions are true and, if so, the server MUST disconnect the connection as specified in section 3.3.7.1:
For a singleton request and the first operation of a compounded request,
The size of the decrypted message is less than the size of the SMB2 Header
SMB2_FLAGS_RELATED_OPERATIONS is set in the Flags field of the SMB2 header of the request
The SessionId field in the SMB2 header of the request is not equal to Request.TransformSessionId.
In a compounded request, for each operation in the compounded chain except the first one, SMB2_FLAGS_RELATED_OPERATIONS is not set in the Flags field of the SMB2 header of the operation and SessionId in the SMB2 header of the operation is not equal to Request.TransformSessionId.
In a compounded request, each response in a compounded chain, except the first one, does not start at an 8-byte aligned boundary.
If ProtocolId in the header of the decrypted message is 0x424D53FE indicating an SMB2 header, the server MUST further validate the decrypted message:
The server MUST verify if any of the following conditions are true and, if so, the server MUST disconnect the connection as specified in section 3.3.7.1:
For a singleton request and the first operation of a compounded request,
The size of the decrypted message is less than the size of the SMB2 Header
SMB2_FLAGS_RELATED_OPERATIONS is set in the Flags field of the SMB2 header of the request
The SessionId field in the SMB2 header of the request is not equal to Request.TransformSessionId.
In a compounded request, for each operation in the compounded chain except the first one, SMB2_FLAGS_RELATED_OPERATIONS is not set in the Flags field of the SMB2 header of the operation and SessionId in the SMB2 header of the operation is not equal to Request.TransformSessionId.
Each request in the compounded chain, except the first one, does not start at an 8-byte aligned boundary.
Otherwise the server MUST disconnect the connection as specified in section 3.3.7.1.
```

### New Content
```
This section is applicable for only the SMB 3.x dialect family.<258>
If IsEncryptionSupported is TRUE and Connection.CipherId is not zero, the server MUST perform the following:
If the size of the message received from the client is not greater than the size of the SMB2 TRANSFORM_HEADER as specified in section 2.2.41, the server MUST disconnect the connection as specified in section 3.3.7.1.
If the Flags/EncryptionAlgorithm in the SMB2 TRANSFORM_HEADER is not 0x0001, the server MUST disconnect the connection as specified in section 3.3.7.1.
The server MUST look up the session in the Connection.SessionTable using the SessionId in the SMB2 TRANSFORM_HEADER of the request. If the session is not found, the server MUST disconnect the connection as specified in section 3.3.7.1.
If Connection.ConstrainedConnection is set to TRUE and the request is encrypted, then the server MUST disconnect the connection as specified in section 3.3.7.1.
If Connection.ConstrainedConnection is set to FALSE, Session.IsAnonymous or Session.IsGuest is set to TRUE and the request is encrypted, then the server SHOULD<259> disconnect the connection as specified in section 3.3.7.1.
The server MUST decrypt the message using Session.DecryptionKey. If Connection.Dialect is less than "3.1.1", then AES-128-CCM MUST be used, as specified in [RFC4309]. Otherwise, the algorithm specified by the Connection.CipherId MUST be used. The server passes in the Nonce, OriginalMessageSize, Flags/EncryptionAlgorithm, and SessionId fields of the SMB2 TRANSFORM_HEADER as the Optional Authenticated Data input for the algorithm. If decryption succeeds, the server MUST compare the signature in the SMB2 TRANSFORM_HEADER with the signature returned by the decryption algorithm. If the signature verification fails, the server MUST disconnect the connection as specified in section 3.3.7.1. If the signature verification succeeds, the server MUST continue processing the decrypted packet.
If the OriginalMessageSize field in the SMB2 TRANSFORM_HEADER is not equal to the size of the decrypted message, the server SHOULD<260> disconnect the connection as specified in section 3.3.7.1.
If ProtocolId in the header of the decrypted message is 0x424D53FC indicating a nested compressed message, IsCompressionSupported is TRUE, and Connection.CompressionIds is not empty, the server MUST decompress the message as specified in section 3.3.5.2.1.2. If decompression succeeds, the server MUST further validate the message:
The server MUST verify if any of the following conditions are true and, if so, the server MUST disconnect the connection as specified in section 3.3.7.1:
For a singleton request and the first operation of a compounded request,
The size of the decrypted message is less than the size of the SMB2 Header
SMB2_FLAGS_RELATED_OPERATIONS is set in the Flags field of the SMB2 header of the request
The SessionId field in the SMB2 header of the request is not equal to Request.TransformSessionId.
In a compounded request, for each operation in the compounded chain except the first one, SMB2_FLAGS_RELATED_OPERATIONS is not set in the Flags field of the SMB2 header of the operation and SessionId in the SMB2 header of the operation is not equal to Request.TransformSessionId.
In a compounded request, each response in a compounded chain, except the first one, does not start at an 8-byte aligned boundary.
If ProtocolId in the header of the decrypted message is 0x424D53FE indicating an SMB2 header, the server MUST further validate the decrypted message:
The server MUST verify if any of the following conditions are true and, if so, the server MUST disconnect the connection as specified in section 3.3.7.1:
For a singleton request and the first operation of a compounded request,
The size of the decrypted message is less than the size of the SMB2 Header
SMB2_FLAGS_RELATED_OPERATIONS is set in the Flags field of the SMB2 header of the request
The SessionId field in the SMB2 header of the request is not equal to Request.TransformSessionId.
In a compounded request, for each operation in the compounded chain except the first one, SMB2_FLAGS_RELATED_OPERATIONS is not set in the Flags field of the SMB2 header of the operation and SessionId in the SMB2 header of the operation is not equal to Request.TransformSessionId.
Each request in the compounded chain, except the first one, does not start at an 8-byte aligned boundary.
Otherwise the server MUST disconnect the connection as specified in section 3.3.7.1.
```

## Section 3.3.5.2.1.2: Decompressing the Message
**Change type:** Modified

### Old Content
```
This section is applicable only for the SMB 3.1.1 dialect.<260>
If IsCompressionSupported is TRUE and Connection.CompressionIds is not empty, the server MUST perform the following:
The server MUST disconnect the connection as specified in section 3.3.7.1 if any of the following conditions are satisfied:
If the size of the message received from the client is less than the size of SMB2 COMPRESSION_TRANSFORM_HEADER, specified in section 2.2.42.
If Flags field in SMB2 COMPRESSION_TRANSFORM_HEADER is equal to SMB2_COMPRESSION_FLAG_NONE and Connection.CompressionIds does not contain the CompressionAlgorithm field in the SMB2_COMPRESSION_TRANSFORM_HEADER_UNCHAINED.
If Flags field in SMB2 COMPRESSION_TRANSFORM_HEADER is equal to SMB2_COMPRESSION_FLAG_CHAINED and CompressionAlgorithm in any of the SMB2_COMPRESSION_CHAINED_PAYLOAD_HEADER structures in the chain is neither NONE nor one of the identifiers in Connection.CompressionIds.
If OriginalCompressedSegmentSize in the SMB2 COMPRESSION_TRANSFORM_HEADER is greater than the sum of (256, the size of SMB2 COMPRESSION_TRANSFORM_HEADER, largest of (Connection.MaxReadSize, Connection.MaxWriteSize, and Connection.MaxTransactSize)).
If Connection.SupportsChainedCompression is TRUE and SMB2_COMPRESSION_FLAG_CHAINED is set in the SMB2 COMPRESSION_TRANSFORM_HEADER, the server MUST decompress the data starting at the offset of CompressionAlgorithm field as specified in section 3.1.5.3. Otherwise, the server MUST decompress the data specified at Offset using the algorithm in CompressionAlgorithm field.
The server MUST disconnect the connection as specified in section 3.3.7.1 if any of the following conditions are satisfied:
If decompression fails.
If the size of the decompressed data is not equal to OriginalCompressedSegmentSize.
If the ProtocolId in the decompressed message is not equal to 0x424D53FE.
Otherwise the server MUST disconnect the connection as specified in section 3.3.7.1.
```

### New Content
```
This section is applicable only for the SMB 3.1.1 dialect.<261>
If IsCompressionSupported is TRUE and Connection.CompressionIds is not empty, the server MUST perform the following:
The server MUST disconnect the connection as specified in section 3.3.7.1 if any of the following conditions are satisfied:
If the size of the message received from the client is less than the size of SMB2 COMPRESSION_TRANSFORM_HEADER, specified in section 2.2.42.
If Flags field in SMB2 COMPRESSION_TRANSFORM_HEADER is equal to SMB2_COMPRESSION_FLAG_NONE and Connection.CompressionIds does not contain the CompressionAlgorithm field in the SMB2_COMPRESSION_TRANSFORM_HEADER_UNCHAINED.
If Flags field in SMB2 COMPRESSION_TRANSFORM_HEADER is equal to SMB2_COMPRESSION_FLAG_CHAINED and CompressionAlgorithm in any of the SMB2_COMPRESSION_CHAINED_PAYLOAD_HEADER structures in the chain is neither NONE nor one of the identifiers in Connection.CompressionIds.
If OriginalCompressedSegmentSize in the SMB2 COMPRESSION_TRANSFORM_HEADER is greater than the sum of (256, the size of SMB2 COMPRESSION_TRANSFORM_HEADER, largest of (Connection.MaxReadSize, Connection.MaxWriteSize, and Connection.MaxTransactSize)).
If Connection.SupportsChainedCompression is TRUE and SMB2_COMPRESSION_FLAG_CHAINED is set in the SMB2 COMPRESSION_TRANSFORM_HEADER, the server MUST decompress the data starting at the offset of CompressionAlgorithm field as specified in section 3.1.5.3. Otherwise, the server MUST decompress the data specified at Offset using the algorithm in CompressionAlgorithm field.
The server MUST disconnect the connection as specified in section 3.3.7.1 if any of the following conditions are satisfied:
If decompression fails.
If the size of the decompressed data is not equal to OriginalCompressedSegmentSize.
If the ProtocolId in the decompressed message is not equal to 0x424D53FE.
Otherwise the server MUST disconnect the connection as specified in section 3.3.7.1.
```

## Section 3.3.5.2.3: Verifying the Sequence Number
**Change type:** Modified

### Old Content
```
If the received request is an SMB2 CANCEL, this section MUST be skipped.
If the received request is an SMB_COM_NEGOTIATE, as described in section 1.7, the server MUST assume that MessageId is zero for this request.
The server MUST check that the MessageId for the received request falls within the Connection.CommandSequenceWindow, as specified in section 3.3.1.7.
If Connection.SupportsMultiCredit is TRUE and the CreditCharge field in the SMB2 header is greater than zero, the server MUST check that a number of CreditCharge consecutive sequence numbers starting from MessageId fall within the Connection.CommandSequenceWindow.
If the server determines that the MessageId or the range of MessageIds for the incoming request is not valid, the server SHOULD<261> terminate the connection. Otherwise, the server MUST remove the MessageId or the range of MessageIds from the Connection.CommandSequenceWindow.
```

### New Content
```
If the received request is an SMB2 CANCEL, this section MUST be skipped.
If the received request is an SMB_COM_NEGOTIATE, as described in section 1.7, the server MUST assume that MessageId is zero for this request.
The server MUST check that the MessageId for the received request falls within the Connection.CommandSequenceWindow, as specified in section 3.3.1.7.
If Connection.SupportsMultiCredit is TRUE and the CreditCharge field in the SMB2 header is greater than zero, the server MUST check that a number of CreditCharge consecutive sequence numbers starting from MessageId fall within the Connection.CommandSequenceWindow.
If the server determines that the MessageId or the range of MessageIds for the incoming request is not valid, the server SHOULD<262> terminate the connection. Otherwise, the server MUST remove the MessageId or the range of MessageIds from the Connection.CommandSequenceWindow.
```

## Section 3.3.5.2.4: Verifying the Signature
**Change type:** Modified

### Old Content
```
If Connection.Dialect belongs to the SMB 3.x dialect family and if the decryption in section 3.3.5.2.1.1 succeeds, the server MUST skip the processing in this section.
If the SMB2 header of the SMB2 NEGOTIATE request has the SMB2_FLAGS_SIGNED bit set in the Flags field, the server MUST fail the request with STATUS_INVALID_PARAMETER.
If the SMB2 header of the request has SMB2_FLAGS_SIGNED set in the Flags field and the message is not encrypted, the server MUST verify the signature. If the request is for binding the session, the server MUST look up the session in the GlobalSessionTable using the SessionId in the SMB2 header of the request. For all other requests, the server MUST look up the session in the Connection.SessionTable using the SessionId in the SMB2 header of the request. If the session is not found, the request MUST be failed, as specified in section Sending an Error Response (section 3.3.4.4), with the error code STATUS_USER_SESSION_DELETED. If the session is found, the server MUST verify the signature of the message as specified in section 3.1.5.1.
If Session.Connection.Dialect belongs to the SMB 3.x dialect family, the server MUST use Session.SigningKey if the request is for binding a session, and for all other requests the server MUST use Channel.SigningKey in Session.ChannelList, where Channel.Connection matches the connection on which the request is received.
Otherwise, the server MUST use Session.SessionKey as the session key to verify the signature.
If Session.SigningKey, Channel.SigningKey, or Session.SessionKey is NULL, the server MUST fail the request with STATUS_NOT_SUPPORTED and MUST stop processing the request.
If the signature verification fails, the server MUST fail the request with the error code STATUS_ACCESS_DENIED. The server MAY also disconnect the connection as specified in section 3.3.7.1. If signature verification succeeds, the server MUST continue processing on the packet.<262>
If the SMB2 header of the request does not have SMB2_FLAGS_SIGNED set in the Flags field, the server MUST determine if the client failed to sign a packet that required it. The server MUST look up the session in the GlobalSessionTable using the SessionId in the SMB2 header of the request. If the session is found and Session.SigningRequired is equal to TRUE, the server MUST fail this request with STATUS_ACCESS_DENIED. The server MAY<263> also disconnect the connection, as specified in section 3.3.7.1. If either the session is not found, or Session.SigningRequired is FALSE, the server continues processing on the packet.
If the connection is disconnected, the server MUST remove the connection from the ConnectionList, as specified in section 3.3.7.1.
```

### New Content
```
If the SMB2 header of the SMB2 NEGOTIATE request has the SMB2_FLAGS_SIGNED bit set in the Flags field, the server MUST fail the request with STATUS_INVALID_PARAMETER.
If the SMB2 header of the request has SMB2_FLAGS_SIGNED set in the Flags field and the message is not encrypted, the server MUST verify the signature. If the request is for binding the session, the server MUST look up the session in the GlobalSessionTable using the SessionId in the SMB2 header of the request. For all other requests, the server MUST look up the session in the Connection.SessionTable using the SessionId in the SMB2 header of the request. If the session is not found, the request MUST be failed, as specified in section Sending an Error Response (section 3.3.4.4), with the error code STATUS_USER_SESSION_DELETED. If the session is found, the server MUST verify the signature of the message as specified in section 3.1.5.1.
If Session.Connection.Dialect belongs to the SMB 3.x dialect family, the server MUST use Session.SigningKey if the request is for binding a session, and for all other requests the server MUST use Channel.SigningKey in Session.ChannelList, where Channel.Connection matches the connection on which the request is received.
Otherwise, the server MUST use Session.SessionKey as the session key to verify the signature.
If Session.SigningKey, Channel.SigningKey, or Session.SessionKey is NULL, the server MUST fail the request with STATUS_NOT_SUPPORTED and MUST stop processing the request.
If the signature verification fails, the server MUST fail the request with the error code STATUS_ACCESS_DENIED. The server MAY also disconnect the connection as specified in section 3.3.7.1. If signature verification succeeds, the server MUST continue processing on the packet.<263>
If the SMB2 header of the request does not have SMB2_FLAGS_SIGNED set in the Flags field, the server MUST determine if the client failed to sign a packet that required it. The server MUST look up the session in the GlobalSessionTable using the SessionId in the SMB2 header of the request. If the session is found and Session.SigningRequired is equal to TRUE, the server MUST fail this request with STATUS_ACCESS_DENIED. The server MAY<264> also disconnect the connection, as specified in section 3.3.7.1. If either the session is not found, or Session.SigningRequired is FALSE, the server continues processing on the packet.
If the connection is disconnected, the server MUST remove the connection from the ConnectionList, as specified in section 3.3.7.1.
```

## Section 3.3.5.2.6: Handling Incorrectly Formatted Requests
**Change type:** Modified

### Old Content
```
If the server receives a request that does not conform to the structures outlined in section 2, the server MUST fail the request, as specified in section 3.3.4.4, with the error code STATUS_INVALID_PARAMETER. The server MAY<264> also disconnect the connection.
The server MUST disconnect, as specified in section 3.3.7.1, without sending an error response if any of the following are true:
The Command code in the SMB2 header does not match one of the command codes in the SMB2 header as specified in section 2.2.1.
The server receives a request with a length less than the length of the SMB2 header as specified in section 2.2.1.
```

### New Content
```
If the server receives a request that does not conform to the structures outlined in section 2, the server MUST fail the request, as specified in section 3.3.4.4, with the error code STATUS_INVALID_PARAMETER. The server MAY<265> also disconnect the connection.
The server MUST disconnect, as specified in section 3.3.7.1, without sending an error response if any of the following are true:
The Command code in the SMB2 header does not match one of the command codes in the SMB2 header as specified in section 2.2.1.
The server receives a request with a length less than the length of the SMB2 header as specified in section 2.2.1.
```

## Section 3.3.5.2.7.2: Handling Compounded Related Requests
**Change type:** Modified

### Old Content
```
If SMB2_FLAGS_RELATED_OPERATIONS is set in the Flags field of the SMB2 header of all requests except the first one, the received request MUST be handled as a series of compounded related operations. If the first operation has SMB2_FLAGS_RELATED_OPERATIONS set, the server SHOULD<268> fail processing the compound chain request.
The server MUST handle each individual operation that is described in the chain in order. For the first operation, the identifiers for FileId, SessionId, and TreeId MUST be taken from the received operation. For every subsequent operation, the values used for FileId, SessionId, and TreeId MUST be the ones used in processing the previous operation or generated for the previous resulting response.
When the current operation requires a SessionId or TreeId, and if the previous operation failed to create SessionId or TreeId, or the previous operation does not contain a SessionId or TreeId, the server MUST fail the current operation and all subsequent operations with STATUS_INVALID_PARAMETER.
When the current operation requires a FileId, and if the previous operation neither contains nor generates a FileId, the server MUST fail the current operation and all subsequent operations with STATUS_INVALID_HANDLE.
When the current operation requires a FileId and the previous operation either contains or generates a FileId, if the previous operation fails with an error, the server SHOULD<269> fail the current operation with the same error code returned by the previous operation.
When an operation requires asynchronous processing, the server MUST send an interim response for the current operation as specified in section 3.3.4.2. All the subsequent operations that depend on the current operation MUST also be processed asynchronously.
When all operations are complete, the responses SHOULD be compounded into a single response to return to the client. If the responses are compounded, the server MUST set SMB2_FLAGS_RELATED_OPERATIONS in the Flags field of the SMB2 header of all responses except the first one. This indicates that the response was part of a compounded chain.
```

### New Content
```
If SMB2_FLAGS_RELATED_OPERATIONS is set in the Flags field of the SMB2 header of all requests except the first one, the received request MUST be handled as a series of compounded related operations. If the first operation has SMB2_FLAGS_RELATED_OPERATIONS set, the server SHOULD<269> fail processing the compound chain request.
The server MUST handle each individual operation that is described in the chain in order. For the first operation, the identifiers for FileId, SessionId, and TreeId MUST be taken from the received operation. For every subsequent operation, the values used for FileId, SessionId, and TreeId MUST be the ones used in processing the previous operation or generated for the previous resulting response.
When the current operation requires a SessionId or TreeId, and if the previous operation failed to create SessionId or TreeId, or the previous operation does not contain a SessionId or TreeId, the server MUST fail the current operation and all subsequent operations with STATUS_INVALID_PARAMETER.
When the current operation requires a FileId, and if the previous operation neither contains nor generates a FileId, the server MUST fail the current operation and all subsequent operations with STATUS_INVALID_HANDLE.
When the current operation requires a FileId and the previous operation either contains or generates a FileId, if the previous operation fails with an error, the server SHOULD<270> fail the current operation with the same error code returned by the previous operation.
When an operation requires asynchronous processing, the server MUST send an interim response for the current operation as specified in section 3.3.4.2. All the subsequent operations that depend on the current operation MUST also be processed asynchronously.
When all operations are complete, the responses SHOULD be compounded into a single response to return to the client. If the responses are compounded, the server MUST set SMB2_FLAGS_RELATED_OPERATIONS in the Flags field of the SMB2 header of all responses except the first one. This indicates that the response was part of a compounded chain.
```

## Section 3.3.5.2.9: Verifying the Session
**Change type:** Modified

### Old Content
```
If the server implements the SMB 3.x dialect family, Connection.ConstrainedConnection is TRUE and AllowAnonymousAccess is FALSE, the server MUST disconnect the connection.
The server MUST look up the Session in Connection.SessionTable by using the SessionId in the SMB2 header of the request. If SessionId is not found in Connection.SessionTable, the server MUST fail the request with STATUS_USER_SESSION_DELETED.
If a session is found and Session.State is Expired, the server MUST continue to process the SMB2 LOGOFF, SMB2 CLOSE, and SMB2 LOCK commands. If the command is not one of these, the server SHOULD<270> fail the request with STATUS_NETWORK_SESSION_EXPIRED.
If Session.State is InProgress, the server MUST continue to process the SMB2 LOGOFF, SMB2 CLOSE, and SMB2 LOCK commands. If the command is not one of these, the server MUST fail the request with an implementation-specific<271> error code.
If Connection.Dialect belongs to the SMB 3.x dialect family, and Session.EncryptData is TRUE, the server MUST do the following:
If the server supports the 3.1.1 dialect, locate the Request in the Connection.RequestList for which the Request.MessageId matches the MessageId value in the SMB2 header of the request. 

Otherwise, if the server supports the 3.0 or 3.0.2 dialect, and RejectUnencryptedAccess is TRUE, locate the Request in the Connection.RequestList for which Request.MessageId matches the MessageId value in the SMB2 header of the request.
If Request.IsEncrypted is FALSE, the server MUST fail the request with STATUS_ACCESS_DENIED.
```

### New Content
```
If the server implements the SMB 3.x dialect family, Connection.ConstrainedConnection is TRUE and AllowAnonymousAccess is FALSE, the server MUST disconnect the connection.
The server MUST look up the Session in Connection.SessionTable by using the SessionId in the SMB2 header of the request. If SessionId is not found in Connection.SessionTable, the server MUST fail the request with STATUS_USER_SESSION_DELETED.
If a session is found and Session.State is Expired, the server MUST continue to process the SMB2 LOGOFF, SMB2 CLOSE, and SMB2 LOCK commands. If the command is not one of these, the server SHOULD<271> fail the request with STATUS_NETWORK_SESSION_EXPIRED.
If Session.State is InProgress, the server MUST continue to process the SMB2 LOGOFF, SMB2 CLOSE, and SMB2 LOCK commands. If the command is not one of these, the server MUST fail the request with an implementation-specific<272> error code.
If Connection.Dialect belongs to the SMB 3.x dialect family, and Session.EncryptData is TRUE, the server MUST do the following:
If the server supports the 3.1.1 dialect, locate the Request in the Connection.RequestList for which the Request.MessageId matches the MessageId value in the SMB2 header of the request. 

Otherwise, if the server supports the 3.0 or 3.0.2 dialect, and RejectUnencryptedAccess is TRUE, locate the Request in the Connection.RequestList for which Request.MessageId matches the MessageId value in the SMB2 header of the request.
If Request.IsEncrypted is FALSE, the server MUST fail the request with STATUS_ACCESS_DENIED.
```

## Section 3.3.5.2.10: Verifying the Channel Sequence Number
**Change type:** Modified

### Old Content
```
If Connection.Dialect is equal to "2.0.2" or "2.1", or the command request does not include FileId, this section MUST be skipped.
If the SMB2_FLAGS_REPLAY_OPERATION bit is not set in the Flags field of the SMB2 Header, the server MUST do the following:
If ChannelSequence in the SMB2 Header is equal to Open.ChannelSequence, the server MUST increment Open.OutstandingRequestCount by 1.
Otherwise, if the unsigned difference using 16-bit arithmetic between ChannelSequence in the SMB2 header and Open.ChannelSequence is less than or equal to 0x7FFF, the server MUST do the following:
Increment Open.OutstandingPreRequestCount by Open.OutstandingRequestCount.
Set Open.OutstandingRequestCount to 1.
Set Open.ChannelSequence to ChannelSequence in the SMB2 Header.
Otherwise, the server MUST fail SMB2 WRITE, SET_INFO, and IOCTL requests with STATUS_FILE_NOT_AVAILABLE.
If the SMB2_FLAGS_REPLAY_OPERATION bit is set in the Flags field of the SMB2 Header, the server MUST do the following:
If ChannelSequence in the SMB2 Header is equal to Open.ChannelSequence and the following:
Open.OutstandingPreRequestCount is equal to zero, the server MUST increment Open.OutstandingRequestCount by 1. Otherwise, the server MUST fail the SMB2 WRITE, SET_INFO, and IOCTL requests with STATUS_FILE_NOT_AVAILABLE.
Otherwise, if the unsigned difference using 16-bit arithmetic between ChannelSequence in the SMB2 header and Open.ChannelSequence is less than or equal to 0x7FFF, the server SHOULD<272> perform the following:
Increment Open.OutstandingPreRequestCount by Open.OutstandingRequestCount.
Set Open.ChannelSequence to ChannelSequence in the SMB2 Header.
If Open.OutstandingPreRequestCount is equal to zero, set Open.OutstandingRequestCount to 1. Otherwise, set Open.OutstandingRequestCount to 0 and the server MUST fail the SMB2 WRITE, SET_INFO, and IOCTL requests with STATUS_FILE_NOT_AVAILABLE.
Otherwise, the server MUST fail SMB2 WRITE, SET_INFO, and IOCTL requests with STATUS_FILE_NOT_AVAILABLE.
```

### New Content
```
If Connection.Dialect is equal to "2.0.2" or "2.1", or the command request does not include FileId, this section MUST be skipped.
If the SMB2_FLAGS_REPLAY_OPERATION bit is not set in the Flags field of the SMB2 Header, the server MUST do the following:
If ChannelSequence in the SMB2 Header is equal to Open.ChannelSequence, the server MUST increment Open.OutstandingRequestCount by 1.
Otherwise, if the unsigned difference using 16-bit arithmetic between ChannelSequence in the SMB2 header and Open.ChannelSequence is less than or equal to 0x7FFF, the server MUST do the following:
Increment Open.OutstandingPreRequestCount by Open.OutstandingRequestCount.
Set Open.OutstandingRequestCount to 1.
Set Open.ChannelSequence to ChannelSequence in the SMB2 Header.
Otherwise, the server MUST fail SMB2 WRITE, SET_INFO, and IOCTL requests with STATUS_FILE_NOT_AVAILABLE.
If the SMB2_FLAGS_REPLAY_OPERATION bit is set in the Flags field of the SMB2 Header, the server MUST do the following:
If ChannelSequence in the SMB2 Header is equal to Open.ChannelSequence and the following:
Open.OutstandingPreRequestCount is equal to zero, the server MUST increment Open.OutstandingRequestCount by 1. Otherwise, the server MUST fail the SMB2 WRITE, SET_INFO, and IOCTL requests with STATUS_FILE_NOT_AVAILABLE.
Otherwise, if the unsigned difference using 16-bit arithmetic between ChannelSequence in the SMB2 header and Open.ChannelSequence is less than or equal to 0x7FFF, the server SHOULD<273> perform the following:
Increment Open.OutstandingPreRequestCount by Open.OutstandingRequestCount.
Set Open.ChannelSequence to ChannelSequence in the SMB2 Header.
If Open.OutstandingPreRequestCount is equal to zero, set Open.OutstandingRequestCount to 1. Otherwise, set Open.OutstandingRequestCount to 0 and the server MUST fail the SMB2 WRITE, SET_INFO, and IOCTL requests with STATUS_FILE_NOT_AVAILABLE.
Otherwise, the server MUST fail SMB2 WRITE, SET_INFO, and IOCTL requests with STATUS_FILE_NOT_AVAILABLE.
```

## Section 3.3.5.3.1: SMB 2.1 or SMB 3.x Support
**Change type:** Modified

### Old Content
```
If the server does not implement the SMB 2.1 or 3.x dialect family, processing MUST continue as specified in 3.3.5.3.2.
Otherwise, the server MUST scan the dialects provided for the dialect string "SMB 2.???". If the string is not present, continue to section 3.3.5.3.2. If the string is present, the server MUST respond with an SMB2 NEGOTIATE Response as specified in 2.2.4. If the string is present and the underlying connection is either TCP port 445 or RDMA, Connection.SupportsMultiCredit MUST be set to TRUE.
The server MUST set the command of the SMB2 header to SMB2 NEGOTIATE. All other values MUST be set following the syntax specified in section 2.2.1, and any value not defined there with a default MUST be set to 0. The header is followed by an SMB2 NEGOTIATE Response that MUST be constructed as specified in 2.2.4, with the following specific values:
SecurityMode MUST have the SMB2_NEGOTIATE_SIGNING_ENABLED bit set.
If RequireMessageSigning is TRUE, the server MUST also set SMB2_NEGOTIATE_SIGNING_REQUIRED in the SecurityMode.
DialectRevision MUST be set to 0x02FF.
ServerGuid is set to the global ServerGuid value.
The Capabilities field MUST be set to a combination of zero or more of the following bit values, as specified in section 2.2.4:
SMB2_GLOBAL_CAP_DFS if the server supports the Distributed File System.
SMB2_GLOBAL_CAP_LEASING if the server supports leasing.
SMB2_GLOBAL_CAP_LARGE_MTU if Connection.SupportsMultiCredit is TRUE.
MaxTransactSize is set to the maximum buffer size, in bytes, that the server will accept on this connection for QUERY_INFO, QUERY_DIRECTORY, SET_INFO, and CHANGE_NOTIFY operations. This field is applicable only for buffers sent by the client in SET_INFO requests, or returned from the server in QUERY_INFO, QUERY_DIRECTORY, and CHANGE_NOTIFY responses. This value SHOULD<273> be greater than or equal to 65536. Connection.MaxTransactSize MUST be set to MaxTransactSize.
MaxReadSize is set to the maximum size, in bytes, of the Length in an SMB2 READ Request (2.2.19) that the server will accept on the transport that established this connection. This value SHOULD<274> be greater than or equal to 65536. Connection.MaxReadSize MUST be set to MaxReadSize.
MaxWriteSize is set to the maximum size, in bytes, of the Length in an SMB2 Write Request (2.2.21) that the server will accept on the transport that established this connection. This value SHOULD<275> be greater than or equal to 65536. Connection.MaxWriteSize MUST be set to MaxWriteSize.
SystemTime is set to the current time, in FILETIME format as specified in [MS-DTYP] section 2.3.3.
ServerStartTime SHOULD<276> be set to zero.
SecurityBufferOffset is set to the offset to the Buffer field in the response, in bytes, from the beginning of the SMB2 header.
SecurityBufferLength is set to the length of the data being returned in the Buffer field.
Buffer is filled with a GSS token, generated as follows. Alternatively, an empty Buffer MAY be returned, which elicits client-initiated authentication with an authentication protocol of the client's choice.
The generation of the GSS token for the SMB2 NEGOTIATE Response MUST be done as specified in [MS-SPNG] 3.2.5.2. The server MUST initialize the mechanism with the Integrity, Confidentiality, and Delegate options and use the server-initiated variation as specified in [MS-SPNG] section 3.2.5.2.
Connection.NegotiateDialect MUST be set to 0x02FF, and the response is sent to the client.
```

### New Content
```
If the server does not implement the SMB 2.1 or 3.x dialect family, processing MUST continue as specified in 3.3.5.3.2.
Otherwise, the server MUST scan the dialects provided for the dialect string "SMB 2.???". If the string is not present, continue to section 3.3.5.3.2. If the string is present, the server MUST respond with an SMB2 NEGOTIATE Response as specified in 2.2.4. If the string is present and the underlying connection is either TCP port 445 or RDMA, Connection.SupportsMultiCredit MUST be set to TRUE.
The server MUST set the command of the SMB2 header to SMB2 NEGOTIATE. All other values MUST be set following the syntax specified in section 2.2.1, and any value not defined there with a default MUST be set to 0. The header is followed by an SMB2 NEGOTIATE Response that MUST be constructed as specified in 2.2.4, with the following specific values:
SecurityMode MUST have the SMB2_NEGOTIATE_SIGNING_ENABLED bit set.
If RequireMessageSigning is TRUE, the server MUST also set SMB2_NEGOTIATE_SIGNING_REQUIRED in the SecurityMode.
DialectRevision MUST be set to 0x02FF.
ServerGuid is set to the global ServerGuid value.
The Capabilities field MUST be set to a combination of zero or more of the following bit values, as specified in section 2.2.4:
SMB2_GLOBAL_CAP_DFS if the server supports the Distributed File System.
SMB2_GLOBAL_CAP_LEASING if the server supports leasing.
SMB2_GLOBAL_CAP_LARGE_MTU if Connection.SupportsMultiCredit is TRUE.
MaxTransactSize is set to the maximum buffer size, in bytes, that the server will accept on this connection for QUERY_INFO, QUERY_DIRECTORY, SET_INFO, and CHANGE_NOTIFY operations. This field is applicable only for buffers sent by the client in SET_INFO requests, or returned from the server in QUERY_INFO, QUERY_DIRECTORY, and CHANGE_NOTIFY responses. This value SHOULD<274> be greater than or equal to 65536. Connection.MaxTransactSize MUST be set to MaxTransactSize.
MaxReadSize is set to the maximum size, in bytes, of the Length in an SMB2 READ Request (2.2.19) that the server will accept on the transport that established this connection. This value SHOULD<275> be greater than or equal to 65536. Connection.MaxReadSize MUST be set to MaxReadSize.
MaxWriteSize is set to the maximum size, in bytes, of the Length in an SMB2 Write Request (2.2.21) that the server will accept on the transport that established this connection. This value SHOULD<276> be greater than or equal to 65536. Connection.MaxWriteSize MUST be set to MaxWriteSize.
SystemTime is set to the current time, in FILETIME format as specified in [MS-DTYP] section 2.3.3.
ServerStartTime SHOULD<277> be set to zero.
SecurityBufferOffset is set to the offset to the Buffer field in the response, in bytes, from the beginning of the SMB2 header.
SecurityBufferLength is set to the length of the data being returned in the Buffer field.
Buffer is filled with a GSS token, generated as follows. Alternatively, an empty Buffer MAY be returned, which elicits client-initiated authentication with an authentication protocol of the client's choice.
The generation of the GSS token for the SMB2 NEGOTIATE Response MUST be done as specified in [MS-SPNG] 3.2.5.2. The server MUST initialize the mechanism with the Integrity, Confidentiality, and Delegate options and use the server-initiated variation as specified in [MS-SPNG] section 3.2.5.2.
Connection.NegotiateDialect MUST be set to 0x02FF, and the response is sent to the client.
```

## Section 3.3.5.3.2: SMB 2.0.2 Support
**Change type:** Modified

### Old Content
```
The server MUST scan the dialects provided for the dialect string "SMB 2.002". If the string is present, the client understands SMB2, and the server MUST respond with an SMB2 NEGOTIATE Response. If the string is not present in the dialect list and the server also implements SMB as specified in [MS-SMB], it MUST terminate SMB2 processing on this connection and start SMB processing on this connection. If the string is not present in the dialect list and the server does not implement SMB, the server MUST disconnect the connection, as specified in section 3.3.7.1, without sending a response.
The server MUST set the command of the SMB2 header to SMB2 NEGOTIATE. All other values MUST be set following the syntax specified in section 2.2.1, and any value not defined there with a default MUST be set to 0. The header is followed by an SMB2 NEGOTIATE Response that MUST be constructed as specified in section 2.2.4, with the following specific values:
SecurityMode MUST have the SMB2_NEGOTIATE_SIGNING_ENABLED bit set.
If RequireMessageSigning is TRUE, the server MUST also set SMB2_NEGOTIATE_SIGNING_REQUIRED in the SecurityMode.
DialectRevision MUST be set to 0x0202.
ServerGuid is set to the global ServerGuid value.
If the server supports the Distributed File System, set the SMB2_GLOBAL_CAP_DFS bit in the Capabilities field of the negotiate response.
MaxTransactSize is set to the maximum buffer size,<277> in bytes, that the server will accept on this connection for QUERY_INFO, QUERY_DIRECTORY, SET_INFO, and CHANGE_NOTIFY operations. This field is applicable only for buffers sent by the client in SET_INFO requests, or returned from the server in QUERY_INFO, QUERY_DIRECTORY, and CHANGE_NOTIFY responses. Connection.MaxTransactSize MUST be set to MaxTransactSize.
MaxReadSize is set to the maximum size,<278> in bytes, of the Length in an SMB2 READ Request (2.2.19) that the server will accept on the transport that established this connection. Connection.MaxReadSize MUST be set to MaxReadSize.
MaxWriteSize is set to the maximum size,<279> in bytes, of the Length in an SMB2 WRITE Request (2.2.21) that the server will accept on the transport that established this connection. Connection.MaxWriteSize MUST be set to MaxWriteSize.
SystemTime is set to the current time, in FILETIME format as specified in [MS-DTYP] section 2.3.3.
ServerStartTime SHOULD<280> be set to zero.
SecurityBufferOffset is set to the offset to the Buffer field in the response in bytes from the beginning of the SMB2 header.
SecurityBufferLength is set to the length of the data being returned in the Buffer field.
Buffer is filled with a GSS token, generated as follows. Alternatively, an empty Buffer MAY be returned, which elicits client-initiated authentication with an authentication protocol of the client's choice.
The generation of the GSS token for the SMB2 NEGOTIATE Response MUST be done as specified in [MS-SPNG] section 3.2.5.2. The server MUST initialize the mechanism with the Integrity, Confidentiality, and Delegate options and use the server-initiated variation as specified in [MS-SPNG] section 3.2.5.2.
Connection.Dialect MUST be set to "2.0.2", Connection.NegotiateDialect MUST be set to 0x0202, and the response is sent to the client.
Connection.SupportsMultiCredit MUST be set to FALSE.
```

### New Content
```
The server MUST scan the dialects provided for the dialect string "SMB 2.002". If the string is present, the client understands SMB2, and the server MUST respond with an SMB2 NEGOTIATE Response. If the string is not present in the dialect list and the server also implements SMB as specified in [MS-SMB], it MUST terminate SMB2 processing on this connection and start SMB processing on this connection. If the string is not present in the dialect list and the server does not implement SMB, the server MUST disconnect the connection, as specified in section 3.3.7.1, without sending a response.
The server MUST set the command of the SMB2 header to SMB2 NEGOTIATE. All other values MUST be set following the syntax specified in section 2.2.1, and any value not defined there with a default MUST be set to 0. The header is followed by an SMB2 NEGOTIATE Response that MUST be constructed as specified in section 2.2.4, with the following specific values:
SecurityMode MUST have the SMB2_NEGOTIATE_SIGNING_ENABLED bit set.
If RequireMessageSigning is TRUE, the server MUST also set SMB2_NEGOTIATE_SIGNING_REQUIRED in the SecurityMode.
DialectRevision MUST be set to 0x0202.
ServerGuid is set to the global ServerGuid value.
If the server supports the Distributed File System, set the SMB2_GLOBAL_CAP_DFS bit in the Capabilities field of the negotiate response.
MaxTransactSize is set to the maximum buffer size,<278> in bytes, that the server will accept on this connection for QUERY_INFO, QUERY_DIRECTORY, SET_INFO, and CHANGE_NOTIFY operations. This field is applicable only for buffers sent by the client in SET_INFO requests, or returned from the server in QUERY_INFO, QUERY_DIRECTORY, and CHANGE_NOTIFY responses. Connection.MaxTransactSize MUST be set to MaxTransactSize.
MaxReadSize is set to the maximum size,<279> in bytes, of the Length in an SMB2 READ Request (2.2.19) that the server will accept on the transport that established this connection. Connection.MaxReadSize MUST be set to MaxReadSize.
MaxWriteSize is set to the maximum size,<280> in bytes, of the Length in an SMB2 WRITE Request (2.2.21) that the server will accept on the transport that established this connection. Connection.MaxWriteSize MUST be set to MaxWriteSize.
SystemTime is set to the current time, in FILETIME format as specified in [MS-DTYP] section 2.3.3.
ServerStartTime SHOULD<281> be set to zero.
SecurityBufferOffset is set to the offset to the Buffer field in the response in bytes from the beginning of the SMB2 header.
SecurityBufferLength is set to the length of the data being returned in the Buffer field.
Buffer is filled with a GSS token, generated as follows. Alternatively, an empty Buffer MAY be returned, which elicits client-initiated authentication with an authentication protocol of the client's choice.
The generation of the GSS token for the SMB2 NEGOTIATE Response MUST be done as specified in [MS-SPNG] section 3.2.5.2. The server MUST initialize the mechanism with the Integrity, Confidentiality, and Delegate options and use the server-initiated variation as specified in [MS-SPNG] section 3.2.5.2.
Connection.Dialect MUST be set to "2.0.2", Connection.NegotiateDialect MUST be set to 0x0202, and the response is sent to the client.
Connection.SupportsMultiCredit MUST be set to FALSE.
```

## Section 3.3.5.4: Receiving an SMB2 NEGOTIATE Request
**Change type:** Modified

### Old Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 NEGOTIATE, it MUST process it as follows:
If Connection.NegotiateDialect is 0x0202, 0x0210, 0x0300, 0x0302, or 0x0311, the server MUST disconnect the connection, as specified in section 3.3.7.1, and not reply.
The server MUST set Connection.ClientCapabilities to the capabilities received in the SMB2 NEGOTIATE request.
If the server implements the SMB 3.x dialect family, the server MUST set Connection.ClientSecurityMode to the SecurityMode field of the SMB2 NEGOTIATE Request.
If the server implements the SMB2.1 or 3.x dialect family, the server MUST set Connection.ClientGuid to the ClientGuid field of the SMB2 NEGOTIATE Request.
If SMB2_NEGOTIATE_SIGNING_REQUIRED is set in SecurityMode, the server MUST set Connection.ShouldSign to TRUE.
If the DialectCount of the SMB2 NEGOTIATE Request is 0, the server MUST fail the request with STATUS_INVALID_PARAMETER.
The server MUST select the greatest common dialect between the dialects it implements and the Dialects array of the SMB2 NEGOTIATE request. If a common dialect is not found, the server MUST fail the request with STATUS_NOT_SUPPORTED.
If the server implements the SMB 3.1.1 dialect, the server MUST set Connection.ClientDialects to the Dialects field received in the SMB2 NEGOTIATE request.
If a common dialect is found, the server MUST set Connection.Dialect to "2.0.2", "2.1", "3.0", "3.0.2", or "3.1.1", and Connection.NegotiateDialect to 0x0202, 0x0210, 0x0300, 0x0302, or 0x0311, accordingly, to reflect the dialect selected.
If the Connection.Dialect is "3.1.1", then the server MUST process the NegotiateContextList that is specified by the request's NegotiateContextOffset and NegotiateContextCount fields as follows:
If the NegotiateContextList does not contain exactly one SMB2_PREAUTH_INTEGRITY_CAPABILITIES negotiate context, the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.
If the NegotiateContextList contains more than one SMB2_ENCRYPTION_CAPABILITIES negotiate context, the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.
If the NegotiateContextList contains more than one SMB2_COMPRESSION_CAPABILITIES negotiate context, the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.
If the NegotiateContextList contains more than one SMB2_RDMA_TRANSFORM_CAPABILITIES negotiate context, the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.
If the NegotiateContextList contains more than one SMB2_SIGNING_CAPABILITIES negotiate context, the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.
For each context in the received NegotiateContextList, if the context is SMB2_NETNAME_NEGOTIATE_CONTEXT_ID or any negotiate context other than SMB2_PREAUTH_INTEGRITY_CAPABILITIES, SMB2_COMPRESSION_CAPABILITIES, SMB2_RDMA_TRANSFORM_CAPABILITIES, SMB2_SIGNING_CAPABILITIES, SMB2_TRANSPORT_CAPABILITIES, or SMB2_ENCRYPTION_CAPABILITIES, the server MUST ignore the negotiate context.
Processing the SMB2_PREAUTH_INTEGRITY_CAPABILITIES negotiate context:
If the DataLength of the negotiate context is less than the size of SMB2_PREAUTH_INTEGRITY_CAPABILITIES structure, the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.
If the SMB2_PREAUTH_INTEGRITY_CAPABILITIES HashAlgorithms array does not contain any hash algorithms that the server supports, the server MUST fail the negotiate request with STATUS_SMB_NO_PREAUTH_INTEGRITY_HASH_OVERLAP (0xC05D0000).
The server MUST set Connection.PreauthIntegrityHashId to one of the hash algorithms in the client's SMB2_PREAUTH_INTEGRITY_CAPABILITIES HashAlgorithms array. When more than one hash algorithm is supported by the server, the policy for selecting a hash algorithm from the set of hash algorithms that the client and server support is implementation-dependent.
The server MUST initialize Connection.PreauthIntegrityHashValue with zero.
The server MUST generate a hash using the Connection.PreauthIntegrityHashId algorithm on the string constructed by concatenating Connection.PreauthIntegrityHashValue and the negotiate request message, including all bytes from the request's SMB2 header to the last byte received from the network. The server MUST set Connection.PreauthIntegrityHashValue to the hash value generated above.
Processing the SMB2_ENCRYPTION_CAPABILITIES negotiate context:
If IsEncryptionSupported is FALSE, the server MUST ignore the context.
If the DataLength of the negotiate context is less than the size of the SMB2_ENCRYPTION_CAPABILITIES structure, the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.
The server MUST set Connection.CipherId to one of the ciphers in the client's SMB2_ENCRYPTION_CAPABILITIES Ciphers array in an implementation-specific manner. If the client and server have no common cipher, the server MUST set Connection.CipherId to 0.
Processing the SMB2_COMPRESSION_CAPABILITIES negotiate context:
If IsCompressionSupported is FALSE, the server MUST ignore the context.
The server MUST fail the negotiate request with STATUS_INVALID_PARAMETER if any of the following conditions are satisfied.
If the DataLength of the negotiate context is less than the size of the SMB2_COMPRESSION_CAPABILITIES structure.
If CompressionAlgorithmCount is equal to zero.
The server SHOULD<281> set Connection.CompressionIds to all the supported compression algorithms common to both client and server in the CompressionAlgorithms field, in the order they are received. If the server does not support any of the algorithms provided by the client, Connection.CompressionIds MUST be set to an empty list.
Processing the SMB2_RDMA_TRANSFORM_CAPABILITIES negotiate context:
If IsRDMATransformSupported is FALSE, the server MUST ignore the context.
The server MUST fail the negotiate request with STATUS_INVALID_PARAMETER if any of the following conditions are satisfied:
If the DataLength of the negotiate context is less than the size of the SMB2_RDMA_TRANSFORM_CAPABILITIES structure.
If TransformCount is equal to zero.
The server MUST set Connection.RDMATransformIds to all the supported RDMA transforms common to both client and server in the RDMATransformIds field. If the server does not support any of the RDMA transforms provided by the client, Connection.RDMATransformIds MUST be set to an empty list.
Processing the SMB2_SIGNING_CAPABILITIES negotiate context:
If IsSigningCapabilitiesSupported is FALSE, the server MUST ignore the context.
The server MUST fail the negotiate request with STATUS_INVALID_PARAMETER if any of the following conditions are satisfied:
If the DataLength of the negotiate context is less than the size of the SMB2_SIGNING_CAPABILITIES structure.
If SigningAlgorithmCount is equal to zero.
The server MUST set Connection.SigningAlgorithmId to the supported signing algorithm common to both client and server in the SigningAlgorithms field. If the server does not support any of the signing algorithms provided by the client, Connection.SigningAlgorithmId MUST be set to 1 (AES-CMAC).
Processing the SMB2_TRANSPORT_CAPABILITIES negotiate context:
If IsTransportCapabilitiesSupported is FALSE, the server MUST ignore the context.
If the DataLength of the negotiate context is less than the size of the SMB2_TRANSPORT_CAPABILITIES structure, the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.
If the underlying connection is over QUIC, DisableEncryptionOverSecureTransport is TRUE and SMB2_ACCEPT_TRANSPORT_LEVEL_SECURITY is set in the Flags field, the server MUST set Connection.AcceptTransportSecurity to TRUE.
The server MUST then construct an SMB2 NEGOTIATE Response, as specified in section 2.2.4, with the following specific values, and return STATUS_SUCCESS to the client.
If the common dialect is SMB 2.1 or 3.x dialect family and the underlying connection is either TCP port 445 or RDMA, Connection.SupportsMultiCredit MUST be set to TRUE; otherwise, it MUST be set to FALSE.
SecurityMode MUST have the SMB2_NEGOTIATE_SIGNING_ENABLED bit set.
If RequireMessageSigning is TRUE, the server MUST also set SMB2_NEGOTIATE_SIGNING_REQUIRED in the SecurityMode field.
DialectRevision MUST be set to the common dialect.
ServerGuid is set to the global ServerGuid value.
The Capabilities field MUST be set to a combination of zero or more of the following bit values, as specified in section 2.2.4:
SMB2_GLOBAL_CAP_DFS if the server supports the Distributed File System.
SMB2_GLOBAL_CAP_LEASING if the server supports leasing.
SMB2_GLOBAL_CAP_LARGE_MTU if Connection.SupportsMultiCredit is TRUE.
SMB2_GLOBAL_CAP_MULTI_CHANNEL if Connection.Dialect belongs to the SMB 3.x dialect family, IsMultiChannelCapable is TRUE, and SMB2_GLOBAL_CAP_MULTI_CHANNEL is set in the Capabilities field of the request.
SMB2_GLOBAL_CAP_DIRECTORY_LEASING if Connection.Dialect belongs to the SMB 3.x dialect family, the server supports directory leasing, and SMB2_GLOBAL_CAP_DIRECTORY_LEASING is set in the Capabilities field of the request.
SMB2_GLOBAL_CAP_PERSISTENT_HANDLES if Connection.Dialect belongs to the SMB 3.x dialect family, SMB2_GLOBAL_CAP_PERSISTENT_HANDLES is set in the Capabilities field of the request, and the server supports persistent handles.
SMB2_GLOBAL_CAP_ENCRYPTION if Connection.Dialect is "3.0" or "3.0.2", IsEncryptionSupported is TRUE, the server supports AES-128-CCM encryption algorithm and SMB2_GLOBAL_CAP_ENCRYPTION is set in the Capabilities field of the request.
SMB2_GLOBAL_CAP_NOTIFICATIONS if Connection.Dialect is “3.1.1”, IsServerToClientNotificationsSupported is TRUE, and SMB2_GLOBAL_CAP_NOTIFICATIONS is set in the Capabilities field of the request. If SMB2_GLOBAL_CAP_NOTIFICATIONS is set in the Capabilities field of the response, the server MUST set Connection.SupportsNotifications to TRUE. Otherwise, the server MUST set Connection.SupportsNotifications to FALSE.
MaxTransactSize is set to the maximum buffer size, in bytes, that the server will accept on this connection for QUERY_INFO, QUERY_DIRECTORY, SET_INFO and CHANGE_NOTIFY operations. This field is applicable only for buffers sent by the client in SET_INFO requests, or returned from the server in QUERY_INFO, QUERY_DIRECTORY, and CHANGE_NOTIFY responses. This value SHOULD<282> be greater than or equal to 65536. Connection.MaxTransactSize MUST be set to MaxTransactSize.
MaxReadSize is set to the maximum size, in bytes, of the Length in an SMB2 READ Request (section 2.2.19) that the server will accept on the transport that established this connection. This value SHOULD<283> be greater than or equal to 65536. Connection.MaxReadSize MUST be set to MaxReadSize.
MaxWriteSize is set to the maximum size, in bytes, of the Length in an SMB2 WRITE Request (section 2.2.21) that the server will accept on the transport that established this connection. This value SHOULD<284> be greater than or equal to 65536. Connection.MaxWriteSize MUST be set to MaxWriteSize.
SystemTime is set to the current time, in FILETIME format as specified in [MS-DTYP] section 2.3.3.
ServerStartTime SHOULD<285> be set to zero.
SecurityBufferOffset is set to the offset to the Buffer field in the response, in bytes, from the beginning of the SMB2 header.
SecurityBufferLength is set to the length of the data being returned in the Buffer field.
Buffer is filled with the GSS token, generated as follows. Alternatively, an empty Buffer MAY be returned, which elicits client-initiated authentication with an authentication protocol of the client's choice.
The generation of the GSS token for the SMB2 NEGOTIATE Response MUST be done as specified in [MS-SPNG] section 3.2.5.2. The server MUST initialize the mechanism with the Integrity, Confidentiality, and Delegate options and use the server-initiated variation as specified in [MS-SPNG] section 3.2.5.2.
If Connection.Dialect is "3.1.1", then the server MUST build a NegotiateContextList for its negotiate response as follows:
Building an SMB2_PREAUTH_INTEGRITY_CAPABILITIES negotiate context:
The server MUST add an SMB2_PREAUTH_INTEGRITY_CAPABILITIES negotiate context to the response's NegotiateContextList.
HashAlgorithmCount MUST be set to 1.
SaltLength MUST be set to an implementation-specific<286> number of Salt bytes.
HashAlgorithms[0] MUST be set to Connection.PreauthIntegrityHashId.
The Salt buffer MUST be filled with SaltLength unique bytes that are generated for this response by a cryptographic secure pseudo-random number generator.
Building an SMB2_ENCRYPTION_CAPABILITIES negotiate response context:
If the server received an SMB2_ENCRYPTION_CAPABILITIES negotiate context in the client's negotiate request, the server MUST add an SMB2_ENCRYPTION_CAPABILITIES negotiate context to the response's NegotiateContextList. Note that the server MUST send an SMB2_ENCRYPTION_CAPABILITIES context even if the client and server have no common cipher. This is done so that the client can differentiate between a server that does not support encryption (no SMB2_ENCRYPTION_CAPABILITIES context in the response's NegotiateContextList) and a server that supports encryption but does not share a cipher with the client (an SMB2_ENCRYPTION_CAPABILITIES context in the response's NegotiateContextList that indicates a cipher of 0).
CipherCount MUST be set to 1.
Ciphers[0] MUST be set to Connection.CipherId.
Building an SMB2_COMPRESSION_CAPABILITIES negotiate response context:
If the server processed the SMB2_COMPRESSION_CAPABILITIES negotiate request context, then the server MUST build an SMB2_COMPRESSION_CAPABILITIES negotiate response context by setting the following:
If IsChainedCompressionSupported is TRUE and SMB2_COMPRESSION_CAPABILITIES_FLAG_CHAINED bit is set in Flags field of negotiate request context, SMB2_COMPRESSION_CAPABILITIES_FLAG_CHAINED bit MUST be set in Flags field and Connection.SupportsChainedCompression MUST be set to TRUE.
If Connection.CompressionIds is empty,
The server SHOULD<287> set CompressionAlgorithmCount to 1.
The server SHOULD<288> set CompressionAlgorithms to “NONE”.
Otherwise,
Set CompressionAlgorithmCount to the number of compression algorithms in Connection.CompressionIds.
Set CompressionAlgorithms to Connection.CompressionIds.
Building an SMB2_RDMA_TRANSFORM_CAPABILITIES negotiate response context:
If the server processed the SMB2_RDMA_TRANSFORM_CAPABILITIES negotiate request context, then the server MUST build an SMB2_RDMA_TRANSFORM_CAPABILITIES negotiate response context by setting the following:
If Connection.RDMATransformIds is empty, set TransformCount to 1 and set RDMATransformIds to SMB2_RDMA_TRANSFORM_NONE.
Otherwise, set RDMATransformIds to Connection.RDMATransformIds and set TransformCount to the number of elements in RDMATransformIds.
Building an SMB2_SIGNING_CAPABILITIES negotiate response context:
If the server processed the SMB2_SIGNING_CAPABILITIES negotiate request context, then the server MUST build an SMB2_SIGNING_CAPABILITIES negotiate response context by setting the following:
SigningAlgorithms MUST be set to Connection.SigningAlgorithmId.
SigningAlgorithmCount MUST be set to 1.
Building an SMB2_TRANSPORT_CAPABILITIES negotiate response context:
If the server processed the SMB2_TRANSPORT_CAPABILITIES negotiate request context, then the server MUST build an SMB2_TRANSPORT_CAPABILITIES negotiate response context by setting the following:
If Connection.AcceptTransportSecurity is TRUE, Flags MUST be set to SMB2_ACCEPT_TRANSPORT_LEVEL_SECURITY.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_INSUFFICIENT_RESOURCES
STATUS_INVALID_PARAMETER
STATUS_NOT_SUPPORTED
If the server implements the SMB 3.x dialect family, the server MUST store the value of the SecurityMode field in Connection.ServerSecurityMode and MUST store the value of the Capabilities field in Connection.ServerCapabilities.
If Connection.Dialect is "3.1.1", the server MUST do the following:
The server MUST generate a hash using the Connection.PreauthIntegrityHashId algorithm on the string constructed by concatenating Connection.PreauthIntegrityHashValue and the negotiate response message, including all bytes from the response's SMB2 header to the last byte sent to the network. The server MUST set Connection.PreauthIntegrityHashValue to the hash value generated above.
If Connection.CipherId is nonzero, the server MUST set the SMB2_GLOBAL_CAP_ENCRYPTION flag in Connection.ServerCapabilities.
```

### New Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 NEGOTIATE, it MUST process it as follows:
If Connection.NegotiateDialect is 0x0202, 0x0210, 0x0300, 0x0302, or 0x0311, the server MUST disconnect the connection, as specified in section 3.3.7.1, and not reply.
The server MUST set Connection.ClientCapabilities to the capabilities received in the SMB2 NEGOTIATE request.
If the server implements the SMB 3.x dialect family, the server MUST set Connection.ClientSecurityMode to the SecurityMode field of the SMB2 NEGOTIATE Request.
If the server implements the SMB2.1 or 3.x dialect family, the server MUST set Connection.ClientGuid to the ClientGuid field of the SMB2 NEGOTIATE Request.
If SMB2_NEGOTIATE_SIGNING_REQUIRED is set in SecurityMode, the server MUST set Connection.ShouldSign to TRUE.
If the DialectCount of the SMB2 NEGOTIATE Request is 0, the server MUST fail the request with STATUS_INVALID_PARAMETER.
The server MUST select the greatest common dialect between the dialects it implements and the Dialects array of the SMB2 NEGOTIATE request. If a common dialect is not found, the server MUST fail the request with STATUS_NOT_SUPPORTED.
If the server implements the SMB 3.1.1 dialect, the server MUST set Connection.ClientDialects to the Dialects field received in the SMB2 NEGOTIATE request.
If a common dialect is found, the server MUST set Connection.Dialect to "2.0.2", "2.1", "3.0", "3.0.2", or "3.1.1", and Connection.NegotiateDialect to 0x0202, 0x0210, 0x0300, 0x0302, or 0x0311, accordingly, to reflect the dialect selected.
If the Connection.Dialect is "3.1.1", then the server MUST process the NegotiateContextList that is specified by the request's NegotiateContextOffset and NegotiateContextCount fields as follows:
If the NegotiateContextList does not contain exactly one SMB2_PREAUTH_INTEGRITY_CAPABILITIES negotiate context, the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.
If the NegotiateContextList contains more than one SMB2_ENCRYPTION_CAPABILITIES negotiate context, the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.
If the NegotiateContextList contains more than one SMB2_COMPRESSION_CAPABILITIES negotiate context, the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.
If the NegotiateContextList contains more than one SMB2_RDMA_TRANSFORM_CAPABILITIES negotiate context, the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.
If the NegotiateContextList contains more than one SMB2_SIGNING_CAPABILITIES negotiate context, the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.
For each context in the received NegotiateContextList, if the context is SMB2_NETNAME_NEGOTIATE_CONTEXT_ID or any negotiate context other than SMB2_PREAUTH_INTEGRITY_CAPABILITIES, SMB2_COMPRESSION_CAPABILITIES, SMB2_RDMA_TRANSFORM_CAPABILITIES, SMB2_SIGNING_CAPABILITIES, SMB2_TRANSPORT_CAPABILITIES, or SMB2_ENCRYPTION_CAPABILITIES, the server MUST ignore the negotiate context.
Processing the SMB2_PREAUTH_INTEGRITY_CAPABILITIES negotiate context:
If the DataLength of the negotiate context is less than the size of SMB2_PREAUTH_INTEGRITY_CAPABILITIES structure, the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.
If the SMB2_PREAUTH_INTEGRITY_CAPABILITIES HashAlgorithms array does not contain any hash algorithms that the server supports, the server MUST fail the negotiate request with STATUS_SMB_NO_PREAUTH_INTEGRITY_HASH_OVERLAP (0xC05D0000).
The server MUST set Connection.PreauthIntegrityHashId to one of the hash algorithms in the client's SMB2_PREAUTH_INTEGRITY_CAPABILITIES HashAlgorithms array. When more than one hash algorithm is supported by the server, the policy for selecting a hash algorithm from the set of hash algorithms that the client and server support is implementation-dependent.
The server MUST initialize Connection.PreauthIntegrityHashValue with zero.
The server MUST generate a hash using the Connection.PreauthIntegrityHashId algorithm on the string constructed by concatenating Connection.PreauthIntegrityHashValue and the negotiate request message, including all bytes from the request's SMB2 header to the last byte received from the network. The server MUST set Connection.PreauthIntegrityHashValue to the hash value generated above.
Processing the SMB2_ENCRYPTION_CAPABILITIES negotiate context:
If IsEncryptionSupported is FALSE, the server MUST ignore the context.
If the DataLength of the negotiate context is less than the size of the SMB2_ENCRYPTION_CAPABILITIES structure, the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.
The server MUST set Connection.CipherId to one of the ciphers in the client's SMB2_ENCRYPTION_CAPABILITIES Ciphers array in an implementation-specific manner. If the client and server have no common cipher, the server MUST set Connection.CipherId to 0.
Processing the SMB2_COMPRESSION_CAPABILITIES negotiate context:
If IsCompressionSupported is FALSE, the server MUST ignore the context.
The server MUST fail the negotiate request with STATUS_INVALID_PARAMETER if any of the following conditions are satisfied.
If the DataLength of the negotiate context is less than the size of the SMB2_COMPRESSION_CAPABILITIES structure.
If CompressionAlgorithmCount is equal to zero.
The server SHOULD<282> set Connection.CompressionIds to all the supported compression algorithms common to both client and server in the CompressionAlgorithms field, in the order they are received. If the server does not support any of the algorithms provided by the client, Connection.CompressionIds MUST be set to an empty list.
Processing the SMB2_RDMA_TRANSFORM_CAPABILITIES negotiate context:
If IsRDMATransformSupported is FALSE, the server MUST ignore the context.
The server MUST fail the negotiate request with STATUS_INVALID_PARAMETER if any of the following conditions are satisfied:
If the DataLength of the negotiate context is less than the size of the SMB2_RDMA_TRANSFORM_CAPABILITIES structure.
If TransformCount is equal to zero.
The server MUST set Connection.RDMATransformIds to all the supported RDMA transforms common to both client and server in the RDMATransformIds field. If the server does not support any of the RDMA transforms provided by the client, Connection.RDMATransformIds MUST be set to an empty list.
Processing the SMB2_SIGNING_CAPABILITIES negotiate context:
If IsSigningCapabilitiesSupported is FALSE, the server MUST ignore the context.
The server MUST fail the negotiate request with STATUS_INVALID_PARAMETER if any of the following conditions are satisfied:
If the DataLength of the negotiate context is less than the size of the SMB2_SIGNING_CAPABILITIES structure.
If SigningAlgorithmCount is equal to zero.
The server MUST set Connection.SigningAlgorithmId to the supported signing algorithm common to both client and server in the SigningAlgorithms field. If the server does not support any of the signing algorithms provided by the client, Connection.SigningAlgorithmId MUST be set to 1 (AES-CMAC).
Processing the SMB2_TRANSPORT_CAPABILITIES negotiate context:
If IsTransportCapabilitiesSupported is FALSE, the server MUST ignore the context.
If the DataLength of the negotiate context is less than the size of the SMB2_TRANSPORT_CAPABILITIES structure, the server MUST fail the negotiate request with STATUS_INVALID_PARAMETER.
If the underlying connection is over QUIC, DisableEncryptionOverSecureTransport is TRUE and SMB2_ACCEPT_TRANSPORT_LEVEL_SECURITY is set in the Flags field, the server MUST set Connection.AcceptTransportSecurity to TRUE.
The server MUST then construct an SMB2 NEGOTIATE Response, as specified in section 2.2.4, with the following specific values, and return STATUS_SUCCESS to the client.
If the common dialect is SMB 2.1 or 3.x dialect family and the underlying connection is either TCP port 445 or RDMA, Connection.SupportsMultiCredit MUST be set to TRUE; otherwise, it MUST be set to FALSE.
SecurityMode MUST have the SMB2_NEGOTIATE_SIGNING_ENABLED bit set.
If RequireMessageSigning is TRUE, the server MUST also set SMB2_NEGOTIATE_SIGNING_REQUIRED in the SecurityMode field.
DialectRevision MUST be set to the common dialect.
ServerGuid is set to the global ServerGuid value.
The Capabilities field MUST be set to a combination of zero or more of the following bit values, as specified in section 2.2.4:
SMB2_GLOBAL_CAP_DFS if the server supports the Distributed File System.
SMB2_GLOBAL_CAP_LEASING if the server supports leasing.
SMB2_GLOBAL_CAP_LARGE_MTU if Connection.SupportsMultiCredit is TRUE.
SMB2_GLOBAL_CAP_MULTI_CHANNEL if Connection.Dialect belongs to the SMB 3.x dialect family, IsMultiChannelCapable is TRUE, and SMB2_GLOBAL_CAP_MULTI_CHANNEL is set in the Capabilities field of the request.
SMB2_GLOBAL_CAP_DIRECTORY_LEASING if Connection.Dialect belongs to the SMB 3.x dialect family, the server supports directory leasing, and SMB2_GLOBAL_CAP_DIRECTORY_LEASING is set in the Capabilities field of the request.
SMB2_GLOBAL_CAP_PERSISTENT_HANDLES if Connection.Dialect belongs to the SMB 3.x dialect family, SMB2_GLOBAL_CAP_PERSISTENT_HANDLES is set in the Capabilities field of the request, and the server supports persistent handles.
SMB2_GLOBAL_CAP_ENCRYPTION if Connection.Dialect is "3.0" or "3.0.2", IsEncryptionSupported is TRUE, the server supports AES-128-CCM encryption algorithm and SMB2_GLOBAL_CAP_ENCRYPTION is set in the Capabilities field of the request.
SMB2_GLOBAL_CAP_NOTIFICATIONS if Connection.Dialect is “3.1.1”, IsServerToClientNotificationsSupported is TRUE, and SMB2_GLOBAL_CAP_NOTIFICATIONS is set in the Capabilities field of the request. If SMB2_GLOBAL_CAP_NOTIFICATIONS is set in the Capabilities field of the response, the server MUST set Connection.SupportsNotifications to TRUE. Otherwise, the server MUST set Connection.SupportsNotifications to FALSE.
MaxTransactSize is set to the maximum buffer size, in bytes, that the server will accept on this connection for QUERY_INFO, QUERY_DIRECTORY, SET_INFO and CHANGE_NOTIFY operations. This field is applicable only for buffers sent by the client in SET_INFO requests, or returned from the server in QUERY_INFO, QUERY_DIRECTORY, and CHANGE_NOTIFY responses. This value SHOULD<283> be greater than or equal to 65536. Connection.MaxTransactSize MUST be set to MaxTransactSize.
MaxReadSize is set to the maximum size, in bytes, of the Length in an SMB2 READ Request (section 2.2.19) that the server will accept on the transport that established this connection. This value SHOULD<284> be greater than or equal to 65536. Connection.MaxReadSize MUST be set to MaxReadSize.
MaxWriteSize is set to the maximum size, in bytes, of the Length in an SMB2 WRITE Request (section 2.2.21) that the server will accept on the transport that established this connection. This value SHOULD<285> be greater than or equal to 65536. Connection.MaxWriteSize MUST be set to MaxWriteSize.
SystemTime is set to the current time, in FILETIME format as specified in [MS-DTYP] section 2.3.3.
ServerStartTime SHOULD<286> be set to zero.
SecurityBufferOffset is set to the offset to the Buffer field in the response, in bytes, from the beginning of the SMB2 header.
SecurityBufferLength is set to the length of the data being returned in the Buffer field.
Buffer is filled with the GSS token, generated as follows. Alternatively, an empty Buffer MAY be returned, which elicits client-initiated authentication with an authentication protocol of the client's choice.
The generation of the GSS token for the SMB2 NEGOTIATE Response MUST be done as specified in [MS-SPNG] section 3.2.5.2. The server MUST initialize the mechanism with the Integrity, Confidentiality, and Delegate options and use the server-initiated variation as specified in [MS-SPNG] section 3.2.5.2.
If Connection.Dialect is "3.1.1", then the server MUST build a NegotiateContextList for its negotiate response as follows:
Building an SMB2_PREAUTH_INTEGRITY_CAPABILITIES negotiate context:
The server MUST add an SMB2_PREAUTH_INTEGRITY_CAPABILITIES negotiate context to the response's NegotiateContextList.
HashAlgorithmCount MUST be set to 1.
SaltLength MUST be set to an implementation-specific<287> number of Salt bytes.
HashAlgorithms[0] MUST be set to Connection.PreauthIntegrityHashId.
The Salt buffer MUST be filled with SaltLength unique bytes that are generated for this response by a cryptographic secure pseudo-random number generator.
Building an SMB2_ENCRYPTION_CAPABILITIES negotiate response context:
If the server received an SMB2_ENCRYPTION_CAPABILITIES negotiate context in the client's negotiate request, the server MUST add an SMB2_ENCRYPTION_CAPABILITIES negotiate context to the response's NegotiateContextList. Note that the server MUST send an SMB2_ENCRYPTION_CAPABILITIES context even if the client and server have no common cipher. This is done so that the client can differentiate between a server that does not support encryption (no SMB2_ENCRYPTION_CAPABILITIES context in the response's NegotiateContextList) and a server that supports encryption but does not share a cipher with the client (an SMB2_ENCRYPTION_CAPABILITIES context in the response's NegotiateContextList that indicates a cipher of 0).
CipherCount MUST be set to 1.
Ciphers[0] MUST be set to Connection.CipherId.
Building an SMB2_COMPRESSION_CAPABILITIES negotiate response context:
If the server processed the SMB2_COMPRESSION_CAPABILITIES negotiate request context, then the server MUST build an SMB2_COMPRESSION_CAPABILITIES negotiate response context by setting the following:
If IsChainedCompressionSupported is TRUE and SMB2_COMPRESSION_CAPABILITIES_FLAG_CHAINED bit is set in Flags field of negotiate request context, SMB2_COMPRESSION_CAPABILITIES_FLAG_CHAINED bit MUST be set in Flags field and Connection.SupportsChainedCompression MUST be set to TRUE.
If Connection.CompressionIds is empty,
The server SHOULD<288> set CompressionAlgorithmCount to 1.
The server SHOULD<289> set CompressionAlgorithms to “NONE”.
Otherwise,
Set CompressionAlgorithmCount to the number of compression algorithms in Connection.CompressionIds.
Set CompressionAlgorithms to Connection.CompressionIds.
Building an SMB2_RDMA_TRANSFORM_CAPABILITIES negotiate response context:
If the server processed the SMB2_RDMA_TRANSFORM_CAPABILITIES negotiate request context, then the server MUST build an SMB2_RDMA_TRANSFORM_CAPABILITIES negotiate response context by setting the following:
If Connection.RDMATransformIds is empty, set TransformCount to 1 and set RDMATransformIds to SMB2_RDMA_TRANSFORM_NONE.
Otherwise, set RDMATransformIds to Connection.RDMATransformIds and set TransformCount to the number of elements in RDMATransformIds.
Building an SMB2_SIGNING_CAPABILITIES negotiate response context:
If the server processed the SMB2_SIGNING_CAPABILITIES negotiate request context, then the server MUST build an SMB2_SIGNING_CAPABILITIES negotiate response context by setting the following:
SigningAlgorithms MUST be set to Connection.SigningAlgorithmId.
SigningAlgorithmCount MUST be set to 1.
Building an SMB2_TRANSPORT_CAPABILITIES negotiate response context:
If the server processed the SMB2_TRANSPORT_CAPABILITIES negotiate request context, then the server MUST build an SMB2_TRANSPORT_CAPABILITIES negotiate response context by setting the following:
If Connection.AcceptTransportSecurity is TRUE, Flags MUST be set to SMB2_ACCEPT_TRANSPORT_LEVEL_SECURITY.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_INSUFFICIENT_RESOURCES
STATUS_INVALID_PARAMETER
STATUS_NOT_SUPPORTED
If the server implements the SMB 3.x dialect family, the server MUST store the value of the SecurityMode field in Connection.ServerSecurityMode and MUST store the value of the Capabilities field in Connection.ServerCapabilities.
If Connection.Dialect is "3.1.1", the server MUST do the following:
The server MUST generate a hash using the Connection.PreauthIntegrityHashId algorithm on the string constructed by concatenating Connection.PreauthIntegrityHashValue and the negotiate response message, including all bytes from the response's SMB2 header to the last byte sent to the network. The server MUST set Connection.PreauthIntegrityHashValue to the hash value generated above.
If Connection.CipherId is nonzero, the server MUST set the SMB2_GLOBAL_CAP_ENCRYPTION flag in Connection.ServerCapabilities.
```

## Section 3.3.5.5.3: Handling GSS-API Authentication
**Change type:** Modified

### Old Content
```
The server MUST extract the GSS token from the request. The token is SecurityBufferLength bytes in length and located SecurityBufferOffset bytes from the beginning of the SMB2 header. The server MUST invoke GSS_Accept_sec_context, as specified in [RFC2743], by passing the GSS token to obtain the next GSS output token for the authentication exchange.<291>
If the authentication protocol indicates an error, the server MUST fail the session setup request with the error received by placing the 32-bit NTSTATUS code received into the Status field of the SMB2 header. The server MUST remove the session object from GlobalSessionTable and Connection.SessionTable and deregister the session by invoking the event specified in [MS-SRVS] section 3.1.6.3, providing Session.SessionGlobalId as an input parameter. The server MUST remove the PreauthSession object from Connection.PreauthSessionTable. ServerStatistics.sts0_sopens MUST be decreased by 1. The server MUST close every Open in Session.OpenTable as specified in section 3.3.4.17. The server MUST deregister every TreeConnect in Session.TreeConnectTable by providing the tuple <TreeConnect.Share.ServerName, TreeConnect.Share.Name> and TreeConnect.TreeGlobalId as the input parameters and invoking the event specified in [MS-SRVS] section 3.1.6.7. For each deregistered TreeConnect, TreeConnect.Share.CurrentUses MUST be decreased by 1. All the tree connects in Session.TreeConnectTable MUST be removed and freed. The session object MUST also be freed, and the error response MUST be sent to the client. ServerStatistics.sts0_pwerrors MUST be increased by 1.
The following errors can be returned by the GSS-API interface as specified in [RFC2743]. STATUS_PASSWORD_EXPIRED SHOULD be treated as GSS_S_CREDENTIALS_EXPIRED, SEC_E_INVALID_TOKEN SHOULD be treated as GSS_S_DEFECTIVE_TOKEN, and SEC_E_NO_CREDENTIALS SHOULD be treated as GSS_S_NO_CRED. All other errors SHOULD be treated as a GSS_S_FAILURE error code. A detailed description of these errors is specified in [MS-ERREF].
STATUS_DOWNGRADE_DETECTED
STATUS_NO_SUCH_LOGON_SESSION
SEC_E_WRONG_PRINCIPAL
STATUS_NO_SUCH_USER
STATUS_ACCOUNT_DISABLED
STATUS_ACCOUNT_RESTRICTION
STATUS_ACCOUNT_LOCKED_OUT
STATUS_WRONG_PASSWORD
STATUS_SMARTCARD_WRONG_PIN
STATUS_ACCOUNT_EXPIRED
STATUS_PASSWORD_EXPIRED
STATUS_INVALID_LOGON_HOURS
STATUS_INVALID_WORKSTATION
STATUS_PASSWORD_MUST_CHANGE
STATUS_LOGON_TYPE_NOT_GRANTED
STATUS_PASSWORD_RESTRICTION
STATUS_SMARTCARD_SILENT_CONTEXT
STATUS_SMARTCARD_NO_CARD
STATUS_SMARTCARD_CARD_BLOCKED
STATUS_PKINIT_FAILURE
STATUS_PKINIT_CLIENT_FAILURE
STATUS_PKINIT_NAME_MISMATCH
STATUS_NETLOGON_NOT_STARTED
STATUS_DOMAIN_CONTROLLER_NOT_FOUND
STATUS_NO_SUCH_DOMAIN
STATUS_BAD_NETWORK_PATH
STATUS_TRUST_FAILURE
STATUS_TRUSTED_RELATIONSHIP_FAILURE
STATUS_NETWORK_UNREACHABLE
SEC_E_INVALID_TOKEN
SEC_E_NO_AUTHENTICATING_AUTHORITY
SEC_E_NO_CREDENTIALS
STATUS_INTERNAL_ERROR
STATUS_NO_MEMORY
SEC_E_NOT_OWNER
SEC_E_CERT_WRONG_USAGE
SEC_E_SMARTCARD_LOGON_REQUIRED
SEC_E_SHUTDOWN_IN_PROGRESS
STATUS_LOGON_FAILURE
If the authentication protocol indicates success, the server MUST construct an SMB2 SESSION_SETUP Response, specified in section 2.2.6, as described here:
SMB2_FLAGS_SERVER_TO_REDIR MUST be set in the Flags field of the SMB2 header.
The output token received from the GSS mechanism MUST be returned in the response. SecurityBufferLength indicates the length of the output token, and SecurityBufferOffset indicates its offset, in bytes, from the beginning of the SMB2 header.
Session.SessionId MUST be placed in the SessionId field of the SMB2 header.
If the GSS mechanism indicates that this is the final message in the authentication exchange, the server MUST verify the dialect as follows:
If the server implements the SMB 3.x dialect family and Session.Connection.Dialect is not “2.0.2”, the server MUST look up a client entry in GlobalClientTable using Session.Connection.ClientGuid. If no entry is found, the server MUST create a new Client entry by setting Client.ClientGuid to Session.Connection.ClientGuid and Client.Dialect to Session.Connection.Dialect. The server MUST insert the Client entry into GlobalClientTable. If an entry is found and Client.Dialect is not equal to Session.Connection.Dialect, the server MUST close the newly created Session, as specified in section 3.3.4.12, by providing Session.SessionGlobalId as the input parameter, and fail the session setup request with STATUS_USER_SESSION_DELETED.
If the dialect verification succeeds, the server MUST perform the following:
If Connection.Dialect is "3.1.1" and SMB2_SESSION_FLAG_BINDING is set in the Flags field of the request, the server MUST generate a hash using the Connection.PreauthIntegrityHashId algorithm on the string constructed by concatenating the PreauthSessionTable.PreauthSession.PreauthIntegrityHashValue and the session setup request message, including all bytes from the request's SMB2 header to the last byte received from the network. The server MUST set PreauthSessionTable.PreauthSession.PreauthIntegrityHashValue to the hash value generated above.

Otherwise, the server MUST generate a hash using the Connection.PreauthIntegrityHashId algorithm on the string constructed by concatenating Session.PreauthIntegrityHashValue and the session setup request message, including all bytes from the request's SMB2 header to the last byte received from the network. The server MUST set Session.PreauthIntegrityHashValue to the hash value generated above.
The status code in the SMB2 header of the response MUST be set to STATUS_SUCCESS. If Connection.Dialect belongs to the SMB 3.x dialect family, the server MUST insert the Session into Connection.SessionTable. If Session.ChannelList does not have a channel entry for which Channel.Connection matches the connection on which this request is received, the server MUST allocate a new Channel object with the following values and insert it into Session.ChannelList:
Channel.SigningKey is set to NULL.
Channel.Connection is set to the connection on which this request is received.
If Session.SecurityContext is NULL, it MUST be set to a value representing the user that successfully authenticated this connection. The security context MUST be obtained from the GSS authentication subsystem. If Session.SecurityContext is not NULL or the request is for binding the session, no changes are necessary. The server MUST invoke the GSS_Inquire_context call as specified in [RFC2743] section 2.2.6, passing the Session.SecurityContext as the input parameter, and set Session.UserName to the returned "src_name".
The server MUST invoke the GSS_Inquire_context call as specified in [RFC2743] section 2.2.6, passing the Session.SecurityContext as the context_handle parameter.
If the returned anon_state is TRUE, the server MUST set Session.IsAnonymous to TRUE and the server MAY set the SMB2_SESSION_FLAG_IS_NULL flag in the SessionFlags field of the SMB2 SESSION_SETUP Response.
Otherwise, if the returned src_name corresponds to an implementation-specific guest user,<292> the server MUST set the SMB2_SESSION_FLAG_IS_GUEST in the SessionFlags field of the SMB2 SESSION_SETUP Response and MUST set Session.IsGuest to TRUE.
If the server implements the SMB 3.x dialect family and Session.IsAnonymous is FALSE, the server MUST set Connection.ConstrainedConnection to FALSE.
Session.SigningRequired MUST be set to TRUE under the following conditions:
If the SMB2_NEGOTIATE_SIGNING_REQUIRED bit is set in the SecurityMode field of the client request.
If the SMB2_SESSION_FLAG_IS_GUEST bit is not set in the SessionFlags field and Session.IsAnonymous is FALSE and either Connection.ShouldSign or global RequireMessageSigning is TRUE.
The server MUST query the session key for this authentication from the underlying authentication protocol and store the session key in Session.SessionKey, if Session.SessionKey is NULL. Session.SessionKey MUST be set as specified in section 3.3.1.8, using the value queried from the GSS protocol. If Session.FullSessionKey is empty, Connection.Dialect is “3.1.1”, and Connection.CipherId is AES-256-CCM or AES-256-GCM, Session.FullSessionKey MUST be set to the cryptographic key as queried from the GSS protocol for this authenticated context. For how this value is calculated for Kerberos authentication via GSS-API, see [MS-KILE] section 3.1.1.2. When NTLM authentication via GSS-API is used, Session.SessionKey MUST be set to ExportedSessionKey, see [MS-NLMP] section 3.1.5.1. The server SHOULD choose an authentication mechanism that provides unique and randomly generated session keys in order to secure the integrity of the signing key, encryption key, and decryption key, which are derived using the session key.
If Connection.Dialect belongs to the SMB 3.x dialect family, SMB2_SESSION_FLAG_BINDING is not set in the Flags field of the request, and the request is not for session reauthentication, the server MUST generate Session.SigningKey as specified in section 3.1.4.2 by providing the following inputs:
Session.SessionKey as the key derivation key.
If Connection.Dialect is "3.1.1", the case-sensitive ASCII string "SMBSigningKey" as the label; otherwise, the case-sensitive ASCII string "SMB2AESCMAC" as the label.
The label buffer size in bytes, including the terminating null character. The size of "SMBSigningKey" is 14. The size of "SMB2AESCMAC" is 12.
If Connection.Dialect is "3.1.1", Session.PreauthIntegrityHashValue as the context; otherwise, the case-sensitive ASCII string "SmbSign" as context for the algorithm.
The context buffer size in bytes. If Connection.Dialect is "3.1.1", the size of Session.PreauthIntegrityHashValue. Otherwise, the size of "SmbSign", including the terminating null character, is 8.
If Connection.Dialect belongs to the SMB 3.x dialect family, SMB2_SESSION_FLAG_BINDING is not set in the Flags field of the request, and the request is not for session reauthentication, Session.ApplicationKey MUST be generated as specified in section 3.1.4.2 and passing the following inputs:
Session.SessionKey as the key derivation key.
If Connection.Dialect is "3.1.1", the case-sensitive ASCII string "SMBAppKey" as the label; otherwise, the case-sensitive ASCII string "SMB2APP" as the label.
The label buffer size in bytes, including the terminating null character. The size of "SMBAppKey" is 10. The size of "SMB2APP" is 8.
If Connection.Dialect is "3.1.1", Session.PreauthIntegrityHashValue as the context; otherwise, the case-sensitive ASCII string "SmbRpc" as context for the algorithm.
The context buffer size in bytes. If Connection.Dialect is "3.1.1", the size of Session.PreauthIntegrityHashValue. Otherwise, the size of "SmbRpc", including the terminating null character, is 7.
If Connection.Dialect belongs to the SMB 3.x dialect family, SMB2_SESSION_FLAG_BINDING is set in the Flags field of the request, and the request is not for session reauthentication, the server MUST generate Channel.SigningKey by providing the following input values:
The session key returned by the authentication protocol (in step 6) as the key derivation key.
If Connection.Dialect is "3.1.1", the case-sensitive ASCII string "SMBSigningKey" as the label; otherwise, the case-sensitive ASCII string "SMB2AESCMAC" as the label.
The label buffer size in bytes, including the terminating null character. The size of "SMBSigningKey" is 14. The size of "SMB2AESCMAC" is 12.
If Connection.Dialect is "3.1.1", PreauthSessionTable.PreauthSession.PreauthIntegrityHashValue as the context; otherwise, the case-sensitive ASCII string "SmbSign" as context for the algorithm.
The context buffer size in bytes. If Connection.Dialect is "3.1.1", the size of PreauthSessionTable.PreauthSession.PreauthIntegrityHashValue. Otherwise, the size of "SmbSign", including the terminating null character, is 8.
Otherwise, if Connection.Dialect belongs to the SMB 3.x dialect family and SMB2_SESSION_FLAG_BINDING is not set in the Flags field of the request, the server MUST set Channel.SigningKey as Session.SigningKey.

The server MUST remove the PreauthSession object identified by SessionId from Connection.PreauthSessionTable.
If global EncryptData is TRUE, Connection.Dialect belongs to the SMB 3.x dialect family, Connection.ServerCapabilities includes SMB2_GLOBAL_CAP_ENCRYPTION,  RejectUnencryptedAccess is TRUE, and SMB2_SESSION_FLAG_BINDING is not set in the Flags field of the request, the server MUST do the following:
Set the SMB2_SESSION_FLAG_ENCRYPT_DATA flag in the SessionFlags field of the SMB2 SESSION_SETUP Response.
Set Session.SigningRequired to FALSE.
Set Session.EncryptData to TRUE.
Otherwise,
Set Session.SigningRequired to TRUE.
Set Session.EncryptData to FALSE.
If Connection.Dialect belongs to the SMB 3.x dialect family, SMB2_SESSION_FLAG_BINDING is not set in the Flags field of the request, Session.IsAnonymous and Session.IsGuest are set to FALSE, Connection.ServerCapabilities includes the SMB2_GLOBAL_CAP_ENCRYPTION bit, and the request is not for session reauthentication, the server MUST do the following:
Generate Session.EncryptionKey as specified in section 3.1.4.2 by providing the following inputs:
If Connection.Dialect is “3.1.1” and Connection.CipherId is AES-256-CCM or AES-256-GCM, Session.FullSessionKey as the key derivation key. Otherwise, Session.SessionKey as the key derivation key.
If Connection.Dialect is "3.1.1", the case-sensitive ASCII string "SMBS2CCipherKey" as the label; otherwise, the case-sensitive ASCII string "SMB2AESCCM" as the label.
The label buffer length in bytes, including the terminating null character. The size of "SMBS2CCipherKey" is 16. The size of "SMB2AESCCM" is 11.
If Connection.Dialect is "3.1.1", Session.PreauthIntegrityHashValue as the context; otherwise, the case-sensitive ASCII string "ServerOut" as context for the algorithm.
The context buffer size in bytes. If Connection.Dialect is "3.1.1", the size of Session.PreauthIntegrityHashValue; otherwise, the size of "ServerOut", including the terminating null character, is 10.
Generate Session.DecryptionKey as specified in section 3.1.4.2 by providing the following inputs:
If Connection.Dialect is “3.1.1” and Connection.CipherId is AES-256-CCM or AES-256-GCM, Session.FullSessionKey as the key derivation key. Otherwise, Session.SessionKey as the key derivation key.
If Connection.Dialect is "3.1.1", the case-sensitive ASCII string "SMBC2SCipherKey" as the label; otherwise, the case-sensitive ASCII string "SMB2AESCCM" as the label.
The label buffer length in bytes, including the terminating null character. The size of "SMBC2SCipherKey" is 16. The size of "SMB2AESCCM" is 11.
If Connection.Dialect is "3.1.1", Session.PreauthIntegrityHashValue as the context; otherwise, the case-sensitive ASCII string "ServerIn " as context for the algorithm (note the blank space at the end.)
The context buffer size in bytes. If Connection.Dialect is "3.1.1", the size of Session.PreauthIntegrityHashValue; otherwise, the size of "ServerIn ", including the terminating null character, is 10.
If the SMB2_SESSION_FLAG_IS_GUEST bit is not set in the SessionFlags field, and Session.IsAnonymous is FALSE, the server MUST sign the final session setup response before sending it to the client, as follows:
If Connection.Dialect belongs to the 3.x dialect family, and SMB2_SESSION_FLAG_BINDING is set in the Flags field of the request, the server MUST use Channel.SigningKey.
Otherwise, the server MUST use Session.SigningKey.
If the PreviousSessionId field of the request is not equal to zero, the server MUST take the following actions:
The server MUST look up the old session in GlobalSessionTable, where Session.SessionId matches PreviousSessionId. If no session is found, no other processing is necessary.
If a session is found with Session.SessionId equal to PreviousSessionId, the server MUST determine if the old session and the newly established session are created by the same user by comparing the user identifiers obtained from the Session.SecurityContext on the new and old session.
If the PreviousSessionId and SessionId values in the SMB2 header of the request are equal, the server SHOULD<293> ignore PreviousSessionId and no other processing is required.
Otherwise, if the server determines the authentications were for the same user, the server MUST remove the old session from the GlobalSessionTable and also from the Connection.SessionTable, as specified in section 3.3.7.1.
Otherwise, if the server determines that the authentications were for different users, the server MUST ignore the PreviousSessionId value.
Session.State MUST be set to Valid.
Session.ExpirationTime MUST be set to the expiration time returned by the GSS authentication subsystem. If the GSS authentication subsystem does not return an expiration time, the Session.ExpirationTime is set to infinity.
If Connection.Dialect is “3.1.1” and IsServerToClientNotificationsSupported is TRUE, the server MUST set Session.SupportsNotifications to Connection.SupportsNotifications.
The GSS-API can indicate that this is not the final message in the authentication exchange by using the GSS_S_CONTINUE_NEEDED semantics as specified in [MS-SPNG] section 3.3.1. If the GSS mechanism indicates that this is not the final message of the authentication exchange, the following additional steps MUST be taken:
The status code in the SMB2 header of the response MUST be set to STATUS_MORE_PROCESSING_REQUIRED.
If Connection.Dialect belongs to the SMB 3.x dialect family, and if the SMB2_SESSION_FLAG_BINDING is set in the Flags field of the request, the server MUST sign the response by using Session.SigningKey.
If Connection.Dialect is "3.1.1", SMB2_SESSION_FLAG_BINDING is not set in the Flags field of the request, and this is not a session reauthentication request, the server MUST set the preauthentication hash as follows:
The server MUST generate a hash using the Connection.PreauthIntegrityHashId algorithm on the string constructed by concatenating Session.PreauthIntegrityHashValue and the session setup request message, including all bytes from the request's SMB2 header to the last byte received from the network. The server MUST set Session.PreauthIntegrityHashValue to the hash value generated above.
The server MUST generate a hash using the Connection.PreauthIntegrityHashId algorithm on the string constructed by concatenating Session.PreauthIntegrityHashValue and the session setup response message, including all bytes from the response's SMB2 header to the last byte sent to the network. The server MUST set Session.PreauthIntegrityHashValue to the hash value generated above.
Otherwise, if Connection.Dialect is "3.1.1", SMB2_SESSION_FLAG_BINDING is set in the Flags field of the request, and the server MUST set the preauthentication hash as follows:
The server MUST generate a hash using the Connection.PreauthIntegrityHashId algorithm on the string constructed by concatenating PreauthSessionTable.PreauthSession.PreauthIntegrityHashValue and the session setup request message, including all bytes from the request's SMB2 header to the last byte received from the network. The server MUST set PreauthSessionTable.PreauthSession.PreauthIntegrityHashValue to the hash value generated above.
The server MUST generate a hash using the Connection.PreauthIntegrityHashId algorithm on the string constructed by concatenating PreauthSessionTable.PreauthSession.PreauthIntegrityHashValue and the session setup response message, including all bytes from the response's SMB2 header to the last byte sent to the network. The server MUST set PreauthSessionTable.PreauthSession.PreauthIntegrityHashValue to the hash value generated above.
```

### New Content
```
The server MUST extract the GSS token from the request. The token is SecurityBufferLength bytes in length and located SecurityBufferOffset bytes from the beginning of the SMB2 header. The server MUST invoke GSS_Accept_sec_context, as specified in [RFC2743], by passing the GSS token to obtain the next GSS output token for the authentication exchange.<292>
If the authentication protocol indicates an error, the server MUST fail the session setup request with the error received by placing the 32-bit NTSTATUS code received into the Status field of the SMB2 header. The server MUST remove the session object from GlobalSessionTable and Connection.SessionTable and deregister the session by invoking the event specified in [MS-SRVS] section 3.1.6.3, providing Session.SessionGlobalId as an input parameter. The server MUST remove the PreauthSession object from Connection.PreauthSessionTable. ServerStatistics.sts0_sopens MUST be decreased by 1. The server MUST close every Open in Session.OpenTable as specified in section 3.3.4.17. The server MUST deregister every TreeConnect in Session.TreeConnectTable by providing the tuple <TreeConnect.Share.ServerName, TreeConnect.Share.Name> and TreeConnect.TreeGlobalId as the input parameters and invoking the event specified in [MS-SRVS] section 3.1.6.7. For each deregistered TreeConnect, TreeConnect.Share.CurrentUses MUST be decreased by 1. All the tree connects in Session.TreeConnectTable MUST be removed and freed. The session object MUST also be freed, and the error response MUST be sent to the client. ServerStatistics.sts0_pwerrors MUST be increased by 1.
The following errors can be returned by the GSS-API interface as specified in [RFC2743]. STATUS_PASSWORD_EXPIRED SHOULD be treated as GSS_S_CREDENTIALS_EXPIRED, SEC_E_INVALID_TOKEN SHOULD be treated as GSS_S_DEFECTIVE_TOKEN, and SEC_E_NO_CREDENTIALS SHOULD be treated as GSS_S_NO_CRED. All other errors SHOULD be treated as a GSS_S_FAILURE error code. A detailed description of these errors is specified in [MS-ERREF].
STATUS_DOWNGRADE_DETECTED
STATUS_NO_SUCH_LOGON_SESSION
SEC_E_WRONG_PRINCIPAL
STATUS_NO_SUCH_USER
STATUS_ACCOUNT_DISABLED
STATUS_ACCOUNT_RESTRICTION
STATUS_ACCOUNT_LOCKED_OUT
STATUS_WRONG_PASSWORD
STATUS_SMARTCARD_WRONG_PIN
STATUS_ACCOUNT_EXPIRED
STATUS_PASSWORD_EXPIRED
STATUS_INVALID_LOGON_HOURS
STATUS_INVALID_WORKSTATION
STATUS_PASSWORD_MUST_CHANGE
STATUS_LOGON_TYPE_NOT_GRANTED
STATUS_PASSWORD_RESTRICTION
STATUS_SMARTCARD_SILENT_CONTEXT
STATUS_SMARTCARD_NO_CARD
STATUS_SMARTCARD_CARD_BLOCKED
STATUS_PKINIT_FAILURE
STATUS_PKINIT_CLIENT_FAILURE
STATUS_PKINIT_NAME_MISMATCH
STATUS_NETLOGON_NOT_STARTED
STATUS_DOMAIN_CONTROLLER_NOT_FOUND
STATUS_NO_SUCH_DOMAIN
STATUS_BAD_NETWORK_PATH
STATUS_TRUST_FAILURE
STATUS_TRUSTED_RELATIONSHIP_FAILURE
STATUS_NETWORK_UNREACHABLE
SEC_E_INVALID_TOKEN
SEC_E_NO_AUTHENTICATING_AUTHORITY
SEC_E_NO_CREDENTIALS
STATUS_INTERNAL_ERROR
STATUS_NO_MEMORY
SEC_E_NOT_OWNER
SEC_E_CERT_WRONG_USAGE
SEC_E_SMARTCARD_LOGON_REQUIRED
SEC_E_SHUTDOWN_IN_PROGRESS
STATUS_LOGON_FAILURE
If the authentication protocol indicates success, the server MUST construct an SMB2 SESSION_SETUP Response, specified in section 2.2.6, as described here:
SMB2_FLAGS_SERVER_TO_REDIR MUST be set in the Flags field of the SMB2 header.
The output token received from the GSS mechanism MUST be returned in the response. SecurityBufferLength indicates the length of the output token, and SecurityBufferOffset indicates its offset, in bytes, from the beginning of the SMB2 header.
Session.SessionId MUST be placed in the SessionId field of the SMB2 header.
If the GSS mechanism indicates that this is the final message in the authentication exchange, the server MUST verify the dialect as follows:
If the server implements the SMB 3.x dialect family and Session.Connection.Dialect is not “2.0.2”, the server MUST look up a client entry in GlobalClientTable using Session.Connection.ClientGuid. If no entry is found, the server MUST create a new Client entry by setting Client.ClientGuid to Session.Connection.ClientGuid and Client.Dialect to Session.Connection.Dialect. The server MUST insert the Client entry into GlobalClientTable. If an entry is found and Client.Dialect is not equal to Session.Connection.Dialect, the server MUST close the newly created Session, as specified in section 3.3.4.12, by providing Session.SessionGlobalId as the input parameter, and fail the session setup request with STATUS_USER_SESSION_DELETED.
If the dialect verification succeeds, the server MUST perform the following:
If Connection.Dialect is "3.1.1" and SMB2_SESSION_FLAG_BINDING is set in the Flags field of the request, the server MUST generate a hash using the Connection.PreauthIntegrityHashId algorithm on the string constructed by concatenating the PreauthSessionTable.PreauthSession.PreauthIntegrityHashValue and the session setup request message, including all bytes from the request's SMB2 header to the last byte received from the network. The server MUST set PreauthSessionTable.PreauthSession.PreauthIntegrityHashValue to the hash value generated above.

Otherwise, the server MUST generate a hash using the Connection.PreauthIntegrityHashId algorithm on the string constructed by concatenating Session.PreauthIntegrityHashValue and the session setup request message, including all bytes from the request's SMB2 header to the last byte received from the network. The server MUST set Session.PreauthIntegrityHashValue to the hash value generated above.
The status code in the SMB2 header of the response MUST be set to STATUS_SUCCESS. If Connection.Dialect belongs to the SMB 3.x dialect family, the server MUST insert the Session into Connection.SessionTable. If Session.ChannelList does not have a channel entry for which Channel.Connection matches the connection on which this request is received, the server MUST allocate a new Channel object with the following values and insert it into Session.ChannelList:
Channel.SigningKey is set to NULL.
Channel.Connection is set to the connection on which this request is received.
If Session.SecurityContext is NULL, it MUST be set to a value representing the user that successfully authenticated this connection. The security context MUST be obtained from the GSS authentication subsystem. If Session.SecurityContext is not NULL or the request is for binding the session, no changes are necessary. The server MUST invoke the GSS_Inquire_context call as specified in [RFC2743] section 2.2.6, passing the Session.SecurityContext as the input parameter, and set Session.UserName to the returned "src_name".
The server MUST invoke the GSS_Inquire_context call as specified in [RFC2743] section 2.2.6, passing the Session.SecurityContext as the context_handle parameter.
If the returned anon_state is TRUE, the server MUST set Session.IsAnonymous to TRUE and the server MAY set the SMB2_SESSION_FLAG_IS_NULL flag in the SessionFlags field of the SMB2 SESSION_SETUP Response.
Otherwise, if the returned src_name corresponds to an implementation-specific guest user,<293> the server MUST set the SMB2_SESSION_FLAG_IS_GUEST in the SessionFlags field of the SMB2 SESSION_SETUP Response and MUST set Session.IsGuest to TRUE.
If the server implements the SMB 3.x dialect family and Session.IsAnonymous is FALSE, the server MUST set Connection.ConstrainedConnection to FALSE.
Session.SigningRequired MUST be set to TRUE under the following conditions:
If the SMB2_NEGOTIATE_SIGNING_REQUIRED bit is set in the SecurityMode field of the client request.
If the SMB2_SESSION_FLAG_IS_GUEST bit is not set in the SessionFlags field and Session.IsAnonymous is FALSE and either Connection.ShouldSign or global RequireMessageSigning is TRUE.
The server MUST query the session key for this authentication from the underlying authentication protocol and store the session key in Session.SessionKey, if Session.SessionKey is NULL. Session.SessionKey MUST be set as specified in section 3.3.1.8, using the value queried from the GSS protocol. If Session.FullSessionKey is empty, Connection.Dialect is “3.1.1”, and Connection.CipherId is AES-256-CCM or AES-256-GCM, Session.FullSessionKey MUST be set to the cryptographic key as queried from the GSS protocol for this authenticated context. For how this value is calculated for Kerberos authentication via GSS-API, see [MS-KILE] section 3.1.1.2. When NTLM authentication via GSS-API is used, Session.SessionKey MUST be set to ExportedSessionKey, see [MS-NLMP] section 3.1.5.1. The server SHOULD choose an authentication mechanism that provides unique and randomly generated session keys in order to secure the integrity of the signing key, encryption key, and decryption key, which are derived using the session key.
If Connection.Dialect belongs to the SMB 3.x dialect family, SMB2_SESSION_FLAG_BINDING is not set in the Flags field of the request, and the request is not for session reauthentication, the server MUST generate Session.SigningKey as specified in section 3.1.4.2 by providing the following inputs:
Session.SessionKey as the key derivation key.
If Connection.Dialect is "3.1.1", the case-sensitive ASCII string "SMBSigningKey" as the label; otherwise, the case-sensitive ASCII string "SMB2AESCMAC" as the label.
The label buffer size in bytes, including the terminating null character. The size of "SMBSigningKey" is 14. The size of "SMB2AESCMAC" is 12.
If Connection.Dialect is "3.1.1", Session.PreauthIntegrityHashValue as the context; otherwise, the case-sensitive ASCII string "SmbSign" as context for the algorithm.
The context buffer size in bytes. If Connection.Dialect is "3.1.1", the size of Session.PreauthIntegrityHashValue. Otherwise, the size of "SmbSign", including the terminating null character, is 8.
If Connection.Dialect belongs to the SMB 3.x dialect family, SMB2_SESSION_FLAG_BINDING is not set in the Flags field of the request, and the request is not for session reauthentication, Session.ApplicationKey MUST be generated as specified in section 3.1.4.2 and passing the following inputs:
Session.SessionKey as the key derivation key.
If Connection.Dialect is "3.1.1", the case-sensitive ASCII string "SMBAppKey" as the label; otherwise, the case-sensitive ASCII string "SMB2APP" as the label.
The label buffer size in bytes, including the terminating null character. The size of "SMBAppKey" is 10. The size of "SMB2APP" is 8.
If Connection.Dialect is "3.1.1", Session.PreauthIntegrityHashValue as the context; otherwise, the case-sensitive ASCII string "SmbRpc" as context for the algorithm.
The context buffer size in bytes. If Connection.Dialect is "3.1.1", the size of Session.PreauthIntegrityHashValue. Otherwise, the size of "SmbRpc", including the terminating null character, is 7.
If Connection.Dialect belongs to the SMB 3.x dialect family, SMB2_SESSION_FLAG_BINDING is set in the Flags field of the request, and the request is not for session reauthentication, the server MUST generate Channel.SigningKey by providing the following input values:
The session key returned by the authentication protocol (in step 6) as the key derivation key.
If Connection.Dialect is "3.1.1", the case-sensitive ASCII string "SMBSigningKey" as the label; otherwise, the case-sensitive ASCII string "SMB2AESCMAC" as the label.
The label buffer size in bytes, including the terminating null character. The size of "SMBSigningKey" is 14. The size of "SMB2AESCMAC" is 12.
If Connection.Dialect is "3.1.1", PreauthSessionTable.PreauthSession.PreauthIntegrityHashValue as the context; otherwise, the case-sensitive ASCII string "SmbSign" as context for the algorithm.
The context buffer size in bytes. If Connection.Dialect is "3.1.1", the size of PreauthSessionTable.PreauthSession.PreauthIntegrityHashValue. Otherwise, the size of "SmbSign", including the terminating null character, is 8.
Otherwise, if Connection.Dialect belongs to the SMB 3.x dialect family and SMB2_SESSION_FLAG_BINDING is not set in the Flags field of the request, the server MUST set Channel.SigningKey as Session.SigningKey.

The server MUST remove the PreauthSession object identified by SessionId from Connection.PreauthSessionTable.
If global EncryptData is TRUE, Connection.Dialect belongs to the SMB 3.x dialect family, Connection.ServerCapabilities includes SMB2_GLOBAL_CAP_ENCRYPTION,  RejectUnencryptedAccess is TRUE, and SMB2_SESSION_FLAG_BINDING is not set in the Flags field of the request, the server MUST do the following:
Set the SMB2_SESSION_FLAG_ENCRYPT_DATA flag in the SessionFlags field of the SMB2 SESSION_SETUP Response.
Set Session.SigningRequired to FALSE.
Set Session.EncryptData to TRUE.
Otherwise,
Set Session.SigningRequired to TRUE.
Set Session.EncryptData to FALSE.
If Connection.Dialect belongs to the SMB 3.x dialect family, SMB2_SESSION_FLAG_BINDING is not set in the Flags field of the request, Session.IsAnonymous and Session.IsGuest are set to FALSE, Connection.ServerCapabilities includes the SMB2_GLOBAL_CAP_ENCRYPTION bit, and the request is not for session reauthentication, the server MUST do the following:
Generate Session.EncryptionKey as specified in section 3.1.4.2 by providing the following inputs:
If Connection.Dialect is “3.1.1” and Connection.CipherId is AES-256-CCM or AES-256-GCM, Session.FullSessionKey as the key derivation key. Otherwise, Session.SessionKey as the key derivation key.
If Connection.Dialect is "3.1.1", the case-sensitive ASCII string "SMBS2CCipherKey" as the label; otherwise, the case-sensitive ASCII string "SMB2AESCCM" as the label.
The label buffer length in bytes, including the terminating null character. The size of "SMBS2CCipherKey" is 16. The size of "SMB2AESCCM" is 11.
If Connection.Dialect is "3.1.1", Session.PreauthIntegrityHashValue as the context; otherwise, the case-sensitive ASCII string "ServerOut" as context for the algorithm.
The context buffer size in bytes. If Connection.Dialect is "3.1.1", the size of Session.PreauthIntegrityHashValue; otherwise, the size of "ServerOut", including the terminating null character, is 10.
Generate Session.DecryptionKey as specified in section 3.1.4.2 by providing the following inputs:
If Connection.Dialect is “3.1.1” and Connection.CipherId is AES-256-CCM or AES-256-GCM, Session.FullSessionKey as the key derivation key. Otherwise, Session.SessionKey as the key derivation key.
If Connection.Dialect is "3.1.1", the case-sensitive ASCII string "SMBC2SCipherKey" as the label; otherwise, the case-sensitive ASCII string "SMB2AESCCM" as the label.
The label buffer length in bytes, including the terminating null character. The size of "SMBC2SCipherKey" is 16. The size of "SMB2AESCCM" is 11.
If Connection.Dialect is "3.1.1", Session.PreauthIntegrityHashValue as the context; otherwise, the case-sensitive ASCII string "ServerIn " as context for the algorithm (note the blank space at the end.)
The context buffer size in bytes. If Connection.Dialect is "3.1.1", the size of Session.PreauthIntegrityHashValue; otherwise, the size of "ServerIn ", including the terminating null character, is 10.
If the SMB2_SESSION_FLAG_IS_GUEST bit is not set in the SessionFlags field, and Session.IsAnonymous is FALSE, the server MUST sign the final session setup response before sending it to the client, as follows:
If Connection.Dialect belongs to the 3.x dialect family, and SMB2_SESSION_FLAG_BINDING is set in the Flags field of the request, the server MUST use Channel.SigningKey.
Otherwise, the server MUST use Session.SigningKey.
If the PreviousSessionId field of the request is not equal to zero, the server MUST take the following actions:
The server MUST look up the old session in GlobalSessionTable, where Session.SessionId matches PreviousSessionId. If no session is found, no other processing is necessary.
If a session is found with Session.SessionId equal to PreviousSessionId, the server MUST determine if the old session and the newly established session are created by the same user by comparing the user identifiers obtained from the Session.SecurityContext on the new and old session.
If the PreviousSessionId and SessionId values in the SMB2 header of the request are equal, the server SHOULD<294> ignore PreviousSessionId and no other processing is required.
Otherwise, if the server determines the authentications were for the same user, the server MUST remove the old session from the GlobalSessionTable and also from the Connection.SessionTable, as specified in section 3.3.7.1.
Otherwise, if the server determines that the authentications were for different users, the server MUST ignore the PreviousSessionId value.
Session.State MUST be set to Valid.
Session.ExpirationTime MUST be set to the expiration time returned by the GSS authentication subsystem. If the GSS authentication subsystem does not return an expiration time, the Session.ExpirationTime is set to infinity.
If Connection.Dialect is “3.1.1” and IsServerToClientNotificationsSupported is TRUE, the server MUST set Session.SupportsNotifications to Connection.SupportsNotifications.
The GSS-API can indicate that this is not the final message in the authentication exchange by using the GSS_S_CONTINUE_NEEDED semantics as specified in [MS-SPNG] section 3.3.1. If the GSS mechanism indicates that this is not the final message of the authentication exchange, the following additional steps MUST be taken:
The status code in the SMB2 header of the response MUST be set to STATUS_MORE_PROCESSING_REQUIRED.
If Connection.Dialect belongs to the SMB 3.x dialect family, and if the SMB2_SESSION_FLAG_BINDING is set in the Flags field of the request, the server MUST sign the response by using Session.SigningKey.
If Connection.Dialect is "3.1.1", SMB2_SESSION_FLAG_BINDING is not set in the Flags field of the request, and this is not a session reauthentication request, the server MUST set the preauthentication hash as follows:
The server MUST generate a hash using the Connection.PreauthIntegrityHashId algorithm on the string constructed by concatenating Session.PreauthIntegrityHashValue and the session setup request message, including all bytes from the request's SMB2 header to the last byte received from the network. The server MUST set Session.PreauthIntegrityHashValue to the hash value generated above.
The server MUST generate a hash using the Connection.PreauthIntegrityHashId algorithm on the string constructed by concatenating Session.PreauthIntegrityHashValue and the session setup response message, including all bytes from the response's SMB2 header to the last byte sent to the network. The server MUST set Session.PreauthIntegrityHashValue to the hash value generated above.
Otherwise, if Connection.Dialect is "3.1.1", SMB2_SESSION_FLAG_BINDING is set in the Flags field of the request, and the server MUST set the preauthentication hash as follows:
The server MUST generate a hash using the Connection.PreauthIntegrityHashId algorithm on the string constructed by concatenating PreauthSessionTable.PreauthSession.PreauthIntegrityHashValue and the session setup request message, including all bytes from the request's SMB2 header to the last byte received from the network. The server MUST set PreauthSessionTable.PreauthSession.PreauthIntegrityHashValue to the hash value generated above.
The server MUST generate a hash using the Connection.PreauthIntegrityHashId algorithm on the string constructed by concatenating PreauthSessionTable.PreauthSession.PreauthIntegrityHashValue and the session setup response message, including all bytes from the response's SMB2 header to the last byte sent to the network. The server MUST set PreauthSessionTable.PreauthSession.PreauthIntegrityHashValue to the hash value generated above.
```

## Section 3.3.5.6: Receiving an SMB2 LOGOFF Request
**Change type:** Modified

### Old Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 LOGOFF, message handling MUST proceed as follows.
The server MUST locate the session being logged off, as specified in section 3.3.5.2.9.
For each Open in Session.OpenTable, the server MUST perform the following:
If Open.IsResilient is TRUE, the server MUST do the following:
The server MUST set Open.Session, Open.Connection, and Open.TreeConnect to NULL.
The server MUST set Open.ResilientOpenTimeout to the current time plus Open.ResiliencyTimeOut.
The server SHOULD<294> start or reset the Resilient Open Scavenger Timer, as specified in section 3.3.2.4, under the following conditions:
If the Resilient Open Scavenger Timer is not already active.
If the Resilient Open Scavenger Timer is active and ResilientOpenScavengerExpiryTime is greater than Open.ResilientOpenTimeOut.
In both of the preceding cases, the server MUST set the timer to expire at Open.ResilientOpenTimeOut and MUST set ResilientOpenScavengerExpiryTime to Open.ResilientOpenTimeOut.
If Open.IsDurable is TRUE, the server MUST do the following:
The server MUST set Open.Session, Open.Connection, and Open.TreeConnect to NULL.
The server MUST set Open.DurableOpenScavengerTimeOut to the current time plus Open.DurableOpenTimeOut.
The server MUST start the Durable Open Scavenger Timer, as specified in section 3.3.2.2.
Otherwise the server MUST close the Open as specified in section 3.3.4.17.
Any tree connects in Session.TreeConnectTable of the old session MUST be deregistered by invoking the event specified in [MS-SRVS] section 3.1.6.7, providing the tuple <TreeConnect.Share.ServerName, TreeConnect.Share.Name> and TreeConnect.TreeGlobalId as input parameters, and each of them MUST be freed. For each deregistered TreeConnect, TreeConnect.Share.CurrentUses MUST be decreased by 1.
If Connection.Dialect belongs to the SMB 3.x dialect family, the server MUST remove the session from each Channel.Connection.SessionTable in Session.ChannelList. All channels in Session.ChannelList MUST be removed and freed.
The server MUST remove this session from the GlobalSessionTable and also from the Connection.SessionTable, and deregister the session by invoking the event specified in [MS-SRVS] section 3.1.6.3, providing Session.SessionGlobalId as input parameter. ServerStatistics.sts0_sopens MUST be decreased by 1.
The server MUST construct an SMB2 LOGOFF Response with a status code of STATUS_SUCCESS, following the syntax specified in section 2.2.8, and send it to the client. The session itself is then freed.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_USER_SESSION_DELETED
STATUS_INVALID_PARAMETER
STATUS_NETWORK_SESSION_EXPIRED
STATUS_ACCESS_DENIED
```

### New Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 LOGOFF, message handling MUST proceed as follows.
The server MUST locate the session being logged off, as specified in section 3.3.5.2.9.
For each Open in Session.OpenTable, the server MUST perform the following:
If Open.IsResilient is TRUE, the server MUST do the following:
The server MUST set Open.Session, Open.Connection, and Open.TreeConnect to NULL.
The server MUST set Open.ResilientOpenTimeout to the current time plus Open.ResiliencyTimeOut.
The server SHOULD<295> start or reset the Resilient Open Scavenger Timer, as specified in section 3.3.2.4, under the following conditions:
If the Resilient Open Scavenger Timer is not already active.
If the Resilient Open Scavenger Timer is active and ResilientOpenScavengerExpiryTime is greater than Open.ResilientOpenTimeOut.
In both of the preceding cases, the server MUST set the timer to expire at Open.ResilientOpenTimeOut and MUST set ResilientOpenScavengerExpiryTime to Open.ResilientOpenTimeOut.
If Open.IsDurable is TRUE, the server MUST do the following:
The server MUST set Open.Session, Open.Connection, and Open.TreeConnect to NULL.
The server MUST set Open.DurableOpenScavengerTimeOut to the current time plus Open.DurableOpenTimeOut.
The server MUST start the Durable Open Scavenger Timer, as specified in section 3.3.2.2.
Otherwise the server MUST close the Open as specified in section 3.3.4.17.
Any tree connects in Session.TreeConnectTable of the old session MUST be deregistered by invoking the event specified in [MS-SRVS] section 3.1.6.7, providing the tuple <TreeConnect.Share.ServerName, TreeConnect.Share.Name> and TreeConnect.TreeGlobalId as input parameters, and each of them MUST be freed. For each deregistered TreeConnect, TreeConnect.Share.CurrentUses MUST be decreased by 1.
If Connection.Dialect belongs to the SMB 3.x dialect family, the server MUST remove the session from each Channel.Connection.SessionTable in Session.ChannelList. All channels in Session.ChannelList MUST be removed and freed.
The server MUST remove this session from the GlobalSessionTable and also from the Connection.SessionTable, and deregister the session by invoking the event specified in [MS-SRVS] section 3.1.6.3, providing Session.SessionGlobalId as input parameter. ServerStatistics.sts0_sopens MUST be decreased by 1.
The server MUST construct an SMB2 LOGOFF Response with a status code of STATUS_SUCCESS, following the syntax specified in section 2.2.8, and send it to the client. The session itself is then freed.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_USER_SESSION_DELETED
STATUS_INVALID_PARAMETER
STATUS_NETWORK_SESSION_EXPIRED
STATUS_ACCESS_DENIED
```

## Section 3.3.5.7: Receiving an SMB2 TREE_CONNECT Request
**Change type:** Modified

### Old Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 TREE_CONNECT, message handling proceeds as follows:
The server MUST locate the authenticated session, as specified in section 3.3.5.2.9.
If Connection.Dialect is "3.1.1" and Session.IsAnonymous and Session.IsGuest are set to FALSE and the request is not signed or not encrypted, then the server MUST disconnect the connection.
The server MUST parse the Unicode string in the Buffer field, specified by PathOffset and PathLength fields, to extract the hostname and sharename components, as specified in [MS-DTYP] section 2.2.49. If the Buffer field is not in the format specified in section 2.2.9, the server MUST fail the request with STATUS_INVALID_PARAMETER. Otherwise, the server MUST provide the tuple <hostname, sharename> parsed from the request message to invoke the event specified in [MS-SRVS] section 3.1.6.8, to normalize the hostname by resolving server aliases and evaluating share scope. The server MUST use <normalized hostname, sharename> to look up the Share in ShareList. If no share with a matching share name and server name is found, the server MUST fail the request with STATUS_BAD_NETWORK_NAME. If a share is found, the server MUST do the following:
If Share.Type is STYPE_CLUSTER_FS, STYPE_CLUSTER_SOFS, or STYPE_CLUSTER_DFS as specified in [MS-SRVS] section 2.2.2.4 and Connection.Dialect is greater than MaxClusterDialect and SMB2_TREE_CONNECT_FLAG_CLUSTER_RECONNECT is not set in Flags/Reserved field, the server MUST fail the request with STATUS_SMB_BAD_CLUSTER_DIALECT (0xC05D0001) and if Connection.Dialect is SMB 3.1.1, the server MUST return error data as specified in section 2.2.2 with ByteCount set to 10, ErrorContextCount set to 1, and ErrorData set to SMB2 ERROR Context response formatted as ErrorDataLength set to 2, ErrorId set to 0, and ErrorData set to MaxClusterDialect; otherwise, the server MUST return error data as specified in section 2.2.2 with ByteCount set to 2 and ErrorContextData set to MaxClusterDialect.
If the server implements the SMB 3.x dialect family, EncryptData or Share.EncryptData is TRUE, RejectUnencryptedAccess is TRUE, and Connection.ServerCapabilities does not include SMB2_GLOBAL_CAP_ENCRYPTION, the server MUST fail the request with STATUS_ACCESS_DENIED.
If Connection.Dialect belongs to the SMB 3.x dialect family, Share.EncryptData is TRUE, RejectUnencryptedAccess is TRUE, and Connection.ClientCapabilities does not include the SMB2_GLOBAL_CAP_ENCRYPTION bit, the server MUST fail the request with STATUS_ACCESS_DENIED.
The server MUST determine whether the user represented by Session.SecurityContext is granted access based on the authorization policy specified in Share.ConnectSecurity. If the server determines that it does not grant access, the server MUST fail the request with STATUS_ACCESS_DENIED.
The server MUST provide the tuple <hostname, sharename> to invoke the event specified in [MS-SRVS] section 3.1.6.15 to get the total number of current uses of the share. If the total number of current uses is equal to or greater than Share.MaxUses, the server MUST fail the request with STATUS_REQUEST_NOT_ACCEPTED.
If TreeConnect.Share.Type is STYPE_CLUSTER_SOFS as specified in [MS-SRVS] section 2.2.2.4, Connection.Dialect is "3.1.1" and the SMB2_TREE_CONNECT_FLAG_REDIRECT_TO_OWNER bit is set in the Flags field of the SMB2 TREE_CONNECT request, the server MUST query the underlying object store in an implementation-specific manner to determine whether the share is hosted on this node. If not, the server MUST fail the tree connect request by setting the Status field in SMB2 header to STATUS_BAD_NETWORK_NAME, return error data as specified in section 2.2.2 with ErrorData set to SMB2 ERROR Context response formatted as ErrorId set to SMB2_ERROR_ID_SHARE_REDIRECT, and ErrorContextData set to the Share Redirect error context data as specified in section 2.2.2.2.2 with IPAddrMoveList set to the list of IP addresses determined for where to access the share.
If Connection.Dialect is "3.1.1", Server.SupportsTreeConnectExtn is TRUE, and the SMB2_TREE_CONNECT_FLAG_EXTENSION_PRESENT bit is set in the Flags field of the SMB2 TREE_CONNECT request, the server MUST process the SMB2 tree connect contexts described in section 2.2.9.1. If an SMB2_REMOTED_IDENTITY_TREE_CONNECT context is present and Share.SupportsIdentityRemoting is set, the server MUST perform the following:
If the TicketType is not 0x0001, ignore the context and continue tree connect processing. 

Otherwise, the server MUST obtain User, UserName, Domain, Groups, RestrictedGroups, Privileges, PrimaryGroup, Owner, DefaultDacl, DeviceGroups, UserClaims, and DeviceClaims from the SMB2_REMOTED_IDENTITY_TREE_CONNECT context, and use them to impersonate the remoted identity as specified in [MS-DTYP] section 2.7.1. If successful, set TreeConnect.RemotedIdentitySecurityContext to the impersonated security context.
The server MUST allocate a tree connect object and insert it into Session.TreeConnectTable. The server MUST provide the tuple <hostname, sharename> and MUST register TreeConnect by invoking the event specified in [MS-SRVS] section 3.1.6.6 and assign the return value to TreeConnect.TreeGlobalId. The other initial values MUST be set as follows:
TreeConnect.TreeId MUST be set to a value generated to uniquely identify this tree connect in the Session.TreeConnectTable. The SMB2 server MUST reserve -1 for invalid TreeId.
TreeConnect.Session MUST be set to the session found on the SessionId lookup.
TreeConnect.Share MUST be set to the share found on the lookup.
TreeConnect.OpenCount MUST be set to 0.
TreeConnect.CreationTime MUST be set to current time.
TreeConnect.Share.CurrentUses MUST be increased by 1.
The SMB2 TREE_CONNECT response MUST be constructed following the syntax specified in section 2.2.10, as described here:
ShareFlags MUST be set based on the individual share properties (Share.CscFlags, Share.DoAccessBasedDirectoryEnumeration, Share.AllowNamespaceCaching, Share.ForceSharedDelete, Share.RestrictExclusiveOpens, Share.HashEnabled, Share.ForceLevel2Oplock, Share.IsDfs, Share.EncryptData.)
The server MUST set all flags contained in Share.CscFlags.
The server SHOULD<295> set the SMB2_SHAREFLAG_DFS bit if the per-share property Share.IsDfs is TRUE, indicating that the share is part of a DFS namespace.
The server SHOULD<296> set the SMB2_SHAREFLAG_DFS_ROOT bit if the per-share property Share.IsDfs is TRUE, indicating that the share is part of a DFS namespace.
The server MUST set the SMB2_SHAREFLAG_ACCESS_BASED_DIRECTORY_ENUM bit if Share.DoAccessBasedDirectoryEnumeration is TRUE and ServerHashLevel is not HashDisableAll.
The server MUST set the SMB2_SHAREFLAG_ALLOW_NAMESPACE_CACHING bit if Share.AllowNamespaceCaching is TRUE.
The server MUST set the SMB2_SHAREFLAG_FORCE_SHARED_DELETE bit if Share.ForceSharedDelete is TRUE.
The server MUST set the SMB2_SHAREFLAG_RESTRICT_EXCLUSIVE_OPENS bit if Share.RestrictExclusiveOpens is TRUE.
If Connection.Dialect belongs to the SMB 3.x dialect family, and Share.EncryptData is TRUE, the server MUST do the following:
Set the SMB2_SHAREFLAG_ENCRYPT_DATA bit.
If Share.HashEnabled is TRUE and ServerHashLevel is not HashDisableAll.
If Connection.Dialect belongs to the SMB 3.x dialect family, the server MUST set the SMB2_SHAREFLAG_ENABLE_HASH_V1 and SMB2_SHAREFLAG_ENABLE_HASH_V2 bits in an implementation-specific manner.<297>
Otherwise, it SHOULD<298> set the SMB2_SHAREFLAG_ENABLE_HASH_V1 bit.
The server MUST set the SMB2_SHAREFLAG_FORCE_LEVELII_OPLOCK bit if Share.ForceLevel2Oplock is TRUE.
ShareType MUST be set based on the resource being shared, as indicated by Share.Type:
If this share provides access to named pipes, as indicated by resource type STYPE_IPC as specified in [MS-SRVS] section 2.2.2.4, ShareType MUST be set to SMB2_SHARE_TYPE_PIPE.
If this share provides access to a printer, as indicated by the resource type STYPE_PRINTQ as specified in [MS-SRVS] section 2.2.2.4, ShareType MUST be set to SMB2_SHARE_TYPE_PRINT.
Otherwise, ShareType MUST be set to SMB2_SHARE_TYPE_DISK.
If Share.IsDfs is TRUE, the server MUST set the SMB2_SHARE_CAP_DFS bit in the Capabilities field.
If Connection.Dialect belongs to the SMB 3.x dialect family and Share.IsCA is TRUE, the server MUST set the SMB2_SHARE_CAP_CONTINUOUS_AVAILABILITY bit in the Capabilities field.
If Connection.Dialect belongs to the SMB 3.x dialect family and TreeConnect.Share.Type is STYPE_CLUSTER_SOFS as specified in [MS-SRVS] section 2.2.2.4, the server MUST set the SMB2_SHARE_CAP_SCALEOUT bit in the Capabilities field.
If Connection.Dialect belongs to the SMB 3.x dialect family and TreeConnect.Share.Type is STYPE_CLUSTER_FS, STYPE_CLUSTER_SOFS, or STYPE_CLUSTER_DFS as specified in [MS-SRVS] section 2.2.2.4, the server MUST set the SMB2_SHARE_CAP_CLUSTER bit in the Capabilities field.
If Connection.Dialect is "3.0.2" or "3.1.1", TreeConnect.Share.Type is STYPE_CLUSTER_SOFS as specified in [MS-SRVS] section 2.2.2.4, and TreeConnect.Share is asymmetric, the server MUST set the SMB2_SHARE_CAP_ASYMMETRIC bit in the Capabilities field.
If Connection.Dialect is "3.1.1" and TreeConnect.Share.SupportsIdentityRemoting is set, the server MUST set the SMB2_SHAREFLAG_IDENTITY_REMOTING bit in the ShareFlags field of the SMB2 TREE_CONNECT response.
If Connection.Dialect is "3.1.1", TreeConnect.Share.Type is STYPE_CLUSTER_SOFS as specified in [MS-SRVS] section 2.2.2.4, and the SMB2_TREE_CONNECT_FLAG_REDIRECT_TO_OWNER bit is set in the Flags field of the SMB2 TREE_CONNECT request and the SMB2_SHARE_CAP_ASYMMETRIC bit is set in the Capabilities field, the server SHOULD<299> set the SMB2_SHARE_CAP_REDIRECT_TO_OWNER bit in the Capabilities field.
MaximalAccess MUST be set to the highest access the user described by Session.SecurityContext would have when accessing resources underneath the security descriptor Share.FileSecurity. The server MUST set TreeConnect.MaximalAccess to MaximalAccess.
The response MUST then be sent to the client.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_ACCESS_DENIED
STATUS_INSUFFICIENT_RESOURCES
STATUS_BAD_NETWORK_NAME
STATUS_INVALID_PARAMETER
STATUS_USER_SESSION_DELETED
STATUS_NETWORK_SESSION_EXPIRED
STATUS_SERVER_UNAVAILABLE
```

### New Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 TREE_CONNECT, message handling proceeds as follows:
The server MUST locate the authenticated session, as specified in section 3.3.5.2.9.
If Connection.Dialect is "3.1.1" and Session.IsAnonymous and Session.IsGuest are set to FALSE and the request is not signed or not encrypted, then the server MUST disconnect the connection.
The server MUST parse the Unicode string in the Buffer field, specified by PathOffset and PathLength fields, to extract the hostname and sharename components, as specified in [MS-DTYP] section 2.2.49. If the Buffer field is not in the format specified in section 2.2.9, the server MUST fail the request with STATUS_INVALID_PARAMETER. Otherwise, the server MUST provide the tuple <hostname, sharename> parsed from the request message to invoke the event specified in [MS-SRVS] section 3.1.6.8, to normalize the hostname by resolving server aliases and evaluating share scope. The server MUST use <normalized hostname, sharename> to look up the Share in ShareList. If no share with a matching share name and server name is found, the server MUST fail the request with STATUS_BAD_NETWORK_NAME. If a share is found, the server MUST do the following:
If Share.Type is STYPE_CLUSTER_FS, STYPE_CLUSTER_SOFS, or STYPE_CLUSTER_DFS as specified in [MS-SRVS] section 2.2.2.4 and Connection.Dialect is greater than MaxClusterDialect and SMB2_TREE_CONNECT_FLAG_CLUSTER_RECONNECT is not set in Flags/Reserved field, the server MUST fail the request with STATUS_SMB_BAD_CLUSTER_DIALECT (0xC05D0001) and if Connection.Dialect is SMB 3.1.1, the server MUST return error data as specified in section 2.2.2 with ByteCount set to 10, ErrorContextCount set to 1, and ErrorData set to SMB2 ERROR Context response formatted as ErrorDataLength set to 2, ErrorId set to 0, and ErrorData set to MaxClusterDialect; otherwise, the server MUST return error data as specified in section 2.2.2 with ByteCount set to 2 and ErrorContextData set to MaxClusterDialect.
If the server implements the SMB 3.x dialect family, EncryptData or Share.EncryptData is TRUE, RejectUnencryptedAccess is TRUE, and Connection.ServerCapabilities does not include SMB2_GLOBAL_CAP_ENCRYPTION, the server MUST fail the request with STATUS_ACCESS_DENIED.
If Connection.Dialect belongs to the SMB 3.x dialect family, Share.EncryptData is TRUE, RejectUnencryptedAccess is TRUE, and Connection.ClientCapabilities does not include the SMB2_GLOBAL_CAP_ENCRYPTION bit, the server MUST fail the request with STATUS_ACCESS_DENIED.
The server MUST determine whether the user represented by Session.SecurityContext is granted access based on the authorization policy specified in Share.ConnectSecurity. If the server determines that it does not grant access, the server MUST fail the request with STATUS_ACCESS_DENIED.
The server MUST provide the tuple <hostname, sharename> to invoke the event specified in [MS-SRVS] section 3.1.6.15 to get the total number of current uses of the share. If the total number of current uses is equal to or greater than Share.MaxUses, the server MUST fail the request with STATUS_REQUEST_NOT_ACCEPTED.
If TreeConnect.Share.Type is STYPE_CLUSTER_SOFS as specified in [MS-SRVS] section 2.2.2.4, Connection.Dialect is "3.1.1" and the SMB2_TREE_CONNECT_FLAG_REDIRECT_TO_OWNER bit is set in the Flags field of the SMB2 TREE_CONNECT request, the server MUST query the underlying object store in an implementation-specific manner to determine whether the share is hosted on this node. If not, the server MUST fail the tree connect request by setting the Status field in SMB2 header to STATUS_BAD_NETWORK_NAME, return error data as specified in section 2.2.2 with ErrorData set to SMB2 ERROR Context response formatted as ErrorId set to SMB2_ERROR_ID_SHARE_REDIRECT, and ErrorContextData set to the Share Redirect error context data as specified in section 2.2.2.2.2 with IPAddrMoveList set to the list of IP addresses determined for where to access the share.
If Connection.Dialect is "3.1.1", Server.SupportsTreeConnectExtn is TRUE, and the SMB2_TREE_CONNECT_FLAG_EXTENSION_PRESENT bit is set in the Flags field of the SMB2 TREE_CONNECT request, the server MUST process the SMB2 tree connect contexts described in section 2.2.9.1. If an SMB2_REMOTED_IDENTITY_TREE_CONNECT context is present and Share.SupportsIdentityRemoting is set, the server MUST perform the following:
If the TicketType is not 0x0001, ignore the context and continue tree connect processing. 

Otherwise, the server MUST obtain User, UserName, Domain, Groups, RestrictedGroups, Privileges, PrimaryGroup, Owner, DefaultDacl, DeviceGroups, UserClaims, and DeviceClaims from the SMB2_REMOTED_IDENTITY_TREE_CONNECT context, and use them to impersonate the remoted identity as specified in [MS-DTYP] section 2.7.1. If successful, set TreeConnect.RemotedIdentitySecurityContext to the impersonated security context.
The server MUST allocate a tree connect object and insert it into Session.TreeConnectTable. The server MUST provide the tuple <hostname, sharename> and MUST register TreeConnect by invoking the event specified in [MS-SRVS] section 3.1.6.6 and assign the return value to TreeConnect.TreeGlobalId. The other initial values MUST be set as follows:
TreeConnect.TreeId MUST be set to a value generated to uniquely identify this tree connect in the Session.TreeConnectTable. The SMB2 server MUST reserve -1 for invalid TreeId.
TreeConnect.Session MUST be set to the session found on the SessionId lookup.
TreeConnect.Share MUST be set to the share found on the lookup.
TreeConnect.OpenCount MUST be set to 0.
TreeConnect.CreationTime MUST be set to current time.
TreeConnect.Share.CurrentUses MUST be increased by 1.
The SMB2 TREE_CONNECT response MUST be constructed following the syntax specified in section 2.2.10, as described here:
ShareFlags MUST be set based on the individual share properties (Share.CscFlags, Share.DoAccessBasedDirectoryEnumeration, Share.AllowNamespaceCaching, Share.ForceSharedDelete, Share.RestrictExclusiveOpens, Share.HashEnabled, Share.ForceLevel2Oplock, Share.IsDfs, Share.EncryptData.)
The server MUST set all flags contained in Share.CscFlags.
The server SHOULD<296> set the SMB2_SHAREFLAG_DFS bit if the per-share property Share.IsDfs is TRUE, indicating that the share is part of a DFS namespace.
The server SHOULD<297> set the SMB2_SHAREFLAG_DFS_ROOT bit if the per-share property Share.IsDfs is TRUE, indicating that the share is part of a DFS namespace.
The server MUST set the SMB2_SHAREFLAG_ACCESS_BASED_DIRECTORY_ENUM bit if Share.DoAccessBasedDirectoryEnumeration is TRUE and ServerHashLevel is not HashDisableAll.
The server MUST set the SMB2_SHAREFLAG_ALLOW_NAMESPACE_CACHING bit if Share.AllowNamespaceCaching is TRUE.
The server MUST set the SMB2_SHAREFLAG_FORCE_SHARED_DELETE bit if Share.ForceSharedDelete is TRUE.
The server MUST set the SMB2_SHAREFLAG_RESTRICT_EXCLUSIVE_OPENS bit if Share.RestrictExclusiveOpens is TRUE.
If Connection.Dialect belongs to the SMB 3.x dialect family, and Share.EncryptData is TRUE, the server MUST do the following:
Set the SMB2_SHAREFLAG_ENCRYPT_DATA bit.
If Share.HashEnabled is TRUE and ServerHashLevel is not HashDisableAll.
If Connection.Dialect belongs to the SMB 3.x dialect family, the server MUST set the SMB2_SHAREFLAG_ENABLE_HASH_V1 and SMB2_SHAREFLAG_ENABLE_HASH_V2 bits in an implementation-specific manner.<298>
Otherwise, it SHOULD<299> set the SMB2_SHAREFLAG_ENABLE_HASH_V1 bit.
The server MUST set the SMB2_SHAREFLAG_FORCE_LEVELII_OPLOCK bit if Share.ForceLevel2Oplock is TRUE.
ShareType MUST be set based on the resource being shared, as indicated by Share.Type:
If this share provides access to named pipes, as indicated by resource type STYPE_IPC as specified in [MS-SRVS] section 2.2.2.4, ShareType MUST be set to SMB2_SHARE_TYPE_PIPE.
If this share provides access to a printer, as indicated by the resource type STYPE_PRINTQ as specified in [MS-SRVS] section 2.2.2.4, ShareType MUST be set to SMB2_SHARE_TYPE_PRINT.
Otherwise, ShareType MUST be set to SMB2_SHARE_TYPE_DISK.
If Share.IsDfs is TRUE, the server MUST set the SMB2_SHARE_CAP_DFS bit in the Capabilities field.
If Connection.Dialect belongs to the SMB 3.x dialect family and Share.IsCA is TRUE, the server MUST set the SMB2_SHARE_CAP_CONTINUOUS_AVAILABILITY bit in the Capabilities field.
If Connection.Dialect belongs to the SMB 3.x dialect family and TreeConnect.Share.Type is STYPE_CLUSTER_SOFS as specified in [MS-SRVS] section 2.2.2.4, the server MUST set the SMB2_SHARE_CAP_SCALEOUT bit in the Capabilities field.
If Connection.Dialect belongs to the SMB 3.x dialect family and TreeConnect.Share.Type is STYPE_CLUSTER_FS, STYPE_CLUSTER_SOFS, or STYPE_CLUSTER_DFS as specified in [MS-SRVS] section 2.2.2.4, the server MUST set the SMB2_SHARE_CAP_CLUSTER bit in the Capabilities field.
If Connection.Dialect is "3.0.2" or "3.1.1", TreeConnect.Share.Type is STYPE_CLUSTER_SOFS as specified in [MS-SRVS] section 2.2.2.4, and TreeConnect.Share is asymmetric, the server MUST set the SMB2_SHARE_CAP_ASYMMETRIC bit in the Capabilities field.
If Connection.Dialect is "3.1.1" and TreeConnect.Share.SupportsIdentityRemoting is set, the server MUST set the SMB2_SHAREFLAG_IDENTITY_REMOTING bit in the ShareFlags field of the SMB2 TREE_CONNECT response.
If Connection.Dialect is "3.1.1", TreeConnect.Share.Type is STYPE_CLUSTER_SOFS as specified in [MS-SRVS] section 2.2.2.4, and the SMB2_TREE_CONNECT_FLAG_REDIRECT_TO_OWNER bit is set in the Flags field of the SMB2 TREE_CONNECT request and the SMB2_SHARE_CAP_ASYMMETRIC bit is set in the Capabilities field, the server SHOULD<300> set the SMB2_SHARE_CAP_REDIRECT_TO_OWNER bit in the Capabilities field.
MaximalAccess MUST be set to the highest access the user described by Session.SecurityContext would have when accessing resources underneath the security descriptor Share.FileSecurity. The server MUST set TreeConnect.MaximalAccess to MaximalAccess.
The response MUST then be sent to the client.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_ACCESS_DENIED
STATUS_INSUFFICIENT_RESOURCES
STATUS_BAD_NETWORK_NAME
STATUS_INVALID_PARAMETER
STATUS_USER_SESSION_DELETED
STATUS_NETWORK_SESSION_EXPIRED
STATUS_SERVER_UNAVAILABLE
```

## Section 3.3.5.9.1: Handling the SMB2_CREATE_EA_BUFFER Create Context
**Change type:** Modified

### Old Content
```
The client is requesting that an array of extended attributes be applied to the file that is being created. The server MUST ignore this Create Context for requests to open an existing file, a pipe, or a printer. This create context can be combined with any of those listed here except SMB2_CREATE_DURABLE_HANDLE_RECONNECT.
The processing changes involved for this create context are:
If IsSharedVHDSupported is TRUE and the file name in the Buffer field ends with ":SharedVirtualDisk", the processing changes for this create context are:
In the "Open Execution" phase, this request MUST be processed as specified in [MS-RSVD] section 3.2.5.7 by providing the file name, Open.CreateOptions, and SMB2_CREATE_EA_BUFFER Create Context.
In the "Successful Open Initialization" phase, the server MUST set Open.IsSharedVHDX to TRUE.
Otherwise, in the "Open Execution" phase, the server MUST pass the received extended attributes array to the underlying object store to be stored on the created file.<324> If the object store does not support extended attributes, the server MUST fail the open request with STATUS_EAS_NOT_SUPPORTED.
```

### New Content
```
The client is requesting that an array of extended attributes be applied to the file that is being created. The server MUST ignore this Create Context for requests to open an existing file, a pipe, or a printer. This create context can be combined with any of those listed here except SMB2_CREATE_DURABLE_HANDLE_RECONNECT.
The processing changes involved for this create context are:
If IsSharedVHDSupported is TRUE and the file name in the Buffer field ends with ":SharedVirtualDisk", the processing changes for this create context are:
In the "Open Execution" phase, this request MUST be processed as specified in [MS-RSVD] section 3.2.5.7 by providing the file name, Open.CreateOptions, and SMB2_CREATE_EA_BUFFER Create Context.
In the "Successful Open Initialization" phase, the server MUST set Open.IsSharedVHDX to TRUE.
Otherwise, in the "Open Execution" phase, the server MUST pass the received extended attributes array to the underlying object store to be stored on the created file.<326> If the object store does not support extended attributes, the server MUST fail the open request with STATUS_EAS_NOT_SUPPORTED.
```

## Section 3.3.5.9.2: Handling the SMB2_CREATE_SD_BUFFER Create Context
**Change type:** Modified

### Old Content
```
The client is requesting that a specific security descriptor be applied to the file that is being created. The server MUST ignore this Create Context for requests to open an existing file, a pipe, or a printer.
The processing changes involved for this create context are:
In the "Open Execution" phase, the server MUST pass the received security descriptor to the underlying object store to be stored on the created file.<325> If the object store does not support file security, the value MAY<326> be ignored or STATUS_NOT_SUPPORTED SHOULD be returned to the client.
```

### New Content
```
The client is requesting that a specific security descriptor be applied to the file that is being created. The server MUST ignore this Create Context for requests to open an existing file, a pipe, or a printer.
The processing changes involved for this create context are:
In the "Open Execution" phase, the server MUST pass the received security descriptor to the underlying object store to be stored on the created file.<327> If the object store does not support file security, the value MAY<328> be ignored or STATUS_NOT_SUPPORTED SHOULD be returned to the client.
```

## Section 3.3.5.9.3: Handling the SMB2_CREATE_ALLOCATION_SIZE Create Context
**Change type:** Modified

### Old Content
```
The client is requesting that a specific allocation size be set for the file that is being created. The server SHOULD support this create context request.<327> If the server does not support it, the SMB2_CREATE_ALLOCATION_SIZE create context request MUST be ignored.
The processing changes involved for this create context are:
In the "Open Execution" phase, the server MUST pass the received allocation size to the underlying object store to reserve the requested space for the created file.<328> If the object store does not have sufficient space available to hold a file of the requested size, the server MUST fail the open request with STATUS_DISK_FULL.
```

### New Content
```
The client is requesting that a specific allocation size be set for the file that is being created. The server SHOULD support this create context request.<329> If the server does not support it, the SMB2_CREATE_ALLOCATION_SIZE create context request MUST be ignored.
The processing changes involved for this create context are:
In the "Open Execution" phase, the server MUST pass the received allocation size to the underlying object store to reserve the requested space for the created file.<330> If the object store does not have sufficient space available to hold a file of the requested size, the server MUST fail the open request with STATUS_DISK_FULL.
```

## Section 3.3.5.9.4: Handling the SMB2_CREATE_TIMEWARP_TOKEN Create Context
**Change type:** Modified

### Old Content
```
The client is requesting that the create operation be performed on a snapshot of the underlying object store taken at a previous time.
The processing changes involved for this create context are:
In the "Path Name Validation" phase, the server MUST verify that a snapshot of the underlying object store at the time stamp provided in the create context exists.<329> If it does not, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
In the "Open Execution" phase, the server MUST perform the open on the snapshot of the underlying object store taken at the time specified, instead of using the current view of the object store.<330>
```

### New Content
```
The client is requesting that the create operation be performed on a snapshot of the underlying object store taken at a previous time.
The processing changes involved for this create context are:
In the "Path Name Validation" phase, the server MUST verify that a snapshot of the underlying object store at the time stamp provided in the create context exists.<331> If it does not, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
In the "Open Execution" phase, the server MUST perform the open on the snapshot of the underlying object store taken at the time specified, instead of using the current view of the object store.<332>
```

## Section 3.3.5.9.5: Handling the SMB2_CREATE_QUERY_MAXIMAL_ACCESS_REQUEST Create Context
**Change type:** Modified

### Old Content
```
The client is requesting that the server return maximal access information if the last modified time for the object that was opened, as returned by the underlying object store, is not equal to the time stamp provided by the client in the create context.
The processing changes involved for this create context are:
In the "Response Construction" phase, the server MUST construct an SMB2_CREATE_QUERY_MAXIMAL_ACCESS_RESPONSE create context, following the syntax specified in section 2.2.14.2.5, and include it in the buffer described by the response fields CreateContextsLength and CreateContextsOffset. This structure MUST have the following values set:
If the ChangeTime is not equal to the Timestamp in the request create context, the server MUST calculate the maximal access that the user identified by Session.SecurityContext has on the object that was opened. <331>
If the ChangeTime is equal to the Timestamp in the request create context, the server MUST set QueryStatus to STATUS_NONE_MAPPED and MaximalAccess to zero.
If no time stamp is present in the request, the server MUST return maximal access information unconditionally.
```

### New Content
```
The client is requesting that the server return maximal access information if the last modified time for the object that was opened, as returned by the underlying object store, is not equal to the time stamp provided by the client in the create context.
The processing changes involved for this create context are:
In the "Response Construction" phase, the server MUST construct an SMB2_CREATE_QUERY_MAXIMAL_ACCESS_RESPONSE create context, following the syntax specified in section 2.2.14.2.5, and include it in the buffer described by the response fields CreateContextsLength and CreateContextsOffset. This structure MUST have the following values set:
If the ChangeTime is not equal to the Timestamp in the request create context, the server MUST calculate the maximal access that the user identified by Session.SecurityContext has on the object that was opened. <333>
If the ChangeTime is equal to the Timestamp in the request create context, the server MUST set QueryStatus to STATUS_NONE_MAPPED and MaximalAccess to zero.
If no time stamp is present in the request, the server MUST return maximal access information unconditionally.
```

## Section 3.3.5.9.6: Handling the SMB2_CREATE_DURABLE_HANDLE_REQUEST Create Context
**Change type:** Modified

### Old Content
```
The client is requesting that the open be marked for durable operation. If the underlying object store does not support durable operation, the server MUST ignore the SMB2_CREATE_DURABLE_HANDLE_REQUEST create context.
If the create request also includes an SMB2_CREATE_DURABLE_HANDLE_RECONNECT create context, the server MUST process the create context as specified in section 3.3.5.9.7 and skip this section.
If the create request also includes an SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 or SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context, the server SHOULD<332> fail the create request with STATUS_INVALID_PARAMETER.
If the RequestedOplockLevel field in the create request is not set to SMB2_OPLOCK_LEVEL_BATCH and the create request does not include an SMB2_CREATE_REQUEST_LEASE create context with a LeaseState field that includes the SMB2_LEASE_HANDLE_CACHING bit value, the server MUST ignore this create context and skip this section.
If an SMB2_CREATE_REQUEST_LEASE Create Context or an SMB2_CREATE_REQUEST_LEASE_V2 Create Context is also present in the request and the lease is being requested on a directory, the server MUST ignore this SMB2_CREATE_DURABLE_HANDLE_REQUEST Create Context and skip this section.
The processing changes involved for this create context are:
In the "Successful Open Initialization" phase, if the underlying object store does not grant durability, the server MUST skip the rest of the processing in this phase. Otherwise, the server MUST set Open.IsDurable to TRUE and Open.DurableOwner to a security descriptor accessible only by the user represented by Open.Session.SecurityContext and Open.DurableOpenTimeout MUST be set to an implementation specific value<333>.
In the "Response Construction" phase, the server MUST construct an SMB2_CREATE_DURABLE_HANDLE_RESPONSE response create context, following the syntax specified in section 2.2.14.2.3, and include it in the buffer described by the response CreateContextsLength and CreateContextsOffset.
```

### New Content
```
The client is requesting that the open be marked for durable operation. If the underlying object store does not support durable operation, the server MUST ignore the SMB2_CREATE_DURABLE_HANDLE_REQUEST create context.
If the create request also includes an SMB2_CREATE_DURABLE_HANDLE_RECONNECT create context, the server MUST process the create context as specified in section 3.3.5.9.7 and skip this section.
If the create request also includes an SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 or SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context, the server SHOULD<334> fail the create request with STATUS_INVALID_PARAMETER.
If the RequestedOplockLevel field in the create request is not set to SMB2_OPLOCK_LEVEL_BATCH and the create request does not include an SMB2_CREATE_REQUEST_LEASE create context with a LeaseState field that includes the SMB2_LEASE_HANDLE_CACHING bit value, the server MUST ignore this create context and skip this section.
If an SMB2_CREATE_REQUEST_LEASE Create Context or an SMB2_CREATE_REQUEST_LEASE_V2 Create Context is also present in the request and the lease is being requested on a directory, the server MUST ignore this SMB2_CREATE_DURABLE_HANDLE_REQUEST Create Context and skip this section.
The processing changes involved for this create context are:
In the "Successful Open Initialization" phase, if the underlying object store does not grant durability, the server MUST skip the rest of the processing in this phase. Otherwise, the server MUST set Open.IsDurable to TRUE and Open.DurableOwner to a security descriptor accessible only by the user represented by Open.Session.SecurityContext and Open.DurableOpenTimeout MUST be set to an implementation specific value<335>.
In the "Response Construction" phase, the server MUST construct an SMB2_CREATE_DURABLE_HANDLE_RESPONSE response create context, following the syntax specified in section 2.2.14.2.3, and include it in the buffer described by the response CreateContextsLength and CreateContextsOffset.
```

## Section 3.3.5.9.7: Handling the SMB2_CREATE_DURABLE_HANDLE_RECONNECT Create Context
**Change type:** Modified

### Old Content
```
The client is requesting a reconnect to an existing durable or resilient open.
There is no processing done for "Path Name Validation" or "Open Execution" as listed in the section above.
The processing changes involved for this create context are:
If the create request also includes an SMB2_CREATE_DURABLE_HANDLE_REQUEST create context, the server MUST ignore the SMB2_CREATE_DURABLE_HANDLE_REQUEST create context.
If the create request also contains an SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 or SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context, the server SHOULD<334> fail the request with STATUS_INVALID_PARAMETER.
The server MUST look up an existing open in the GlobalOpenTable by doing a lookup with the FileId.Persistent portion of the create context. If the lookup fails, the server SHOULD<335> fail the request with STATUS_OBJECT_NAME_NOT_FOUND and proceed as specified in "Failed Open Handling" in section 3.3.5.9.
If any Open.Lease is not NULL and Open.ClientGuid is not equal to the ClientGuid of the connection that received this request, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
If Open.Lease is not NULL, Open.Lease.FileDeleteOnClose is FALSE, and Open.Lease.FileName does not match the file name specified in the Buffer field of the SMB2 CREATE request, the server MUST fail the request with STATUS_INVALID_PARAMETER.
If any of the following conditions is TRUE, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
Open.Lease is not NULL and the SMB2_CREATE_REQUEST_LEASE_V2 or the SMB2_CREATE_REQUEST_LEASE create context is not present.
Open.Lease is NULL and the SMB2_CREATE_REQUEST_LEASE_V2 or the SMB2_CREATE_REQUEST_LEASE create context is present.
Open.IsDurable is TRUE, Open.Lease is NULL, and Open.OplockLevel is not equal to SMB2_OPLOCK_LEVEL_BATCH.
Open.IsDurable is TRUE and Open.Lease.LeaseState does not contain SMB2_LEASE_HANDLE_CACHING.
Open.IsDurable is FALSE and Open.IsResilient is FALSE or unimplemented.
Open.Session is not NULL.
The SMB2_CREATE_REQUEST_LEASE_V2 create context is also present in the request, Connection.Dialect belongs to the SMB 3.x dialect family, the server supports directory leasing, Open.Lease is not NULL, and Open.Lease.LeaseKey does not match the LeaseKey provided in the SMB2_CREATE_REQUEST_LEASE_V2 create context.
The SMB2_CREATE_REQUEST_LEASE create context is also present in the request, Connection.Dialect is "2.1" or belongs to the SMB 3.x dialect family, the server supports leasing, Open.Lease is not NULL, and Open.Lease.LeaseKey does not match the LeaseKey provided in the SMB2_CREATE_REQUEST_LEASE create context.
If Open.Lease is not NULL, the server supports leasing and if Lease.Version is 1 and the request does not contain the SMB2_CREATE_REQUEST_LEASE create context or if Lease.Version is 2 and the request does not contain the SMB2_CREATE_REQUEST_LEASE_V2 create context, the server SHOULD<336> fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
If the user represented by Session.SecurityContext is not the same user denoted by Open.DurableOwner, the server MUST fail the request with STATUS_ACCESS_DENIED and proceed as specified in "Failed Open Handling" in section 3.3.5.9.
The server MUST set the Open.Connection to refer to the connection that received this request.
The server MUST set the Open.Session to refer to the session that received this request.
The server MUST set the Open.TreeConnect to refer to the tree connect that received this request, and Open.TreeConnect.OpenCount MUST be increased by 1.
Open.FileId MUST be set to a generated value that uniquely identifies this Open in Session.OpenTable.
The server MUST insert the open into the Session.OpenTable with the Open.FileId as the new key.
The "Successful Open Initialization" and "Oplock Acquisition" phases MUST be skipped, and processing MUST continue as specified in "Response Construction".
In the "Response Construction" phase:
The server MAY<337> construct an SMB2_CREATE_DURABLE_HANDLE_RESPONSE create context, as specified in section 2.2.14.2.3, and include it in the buffer described by the response CreateContextsLength and CreateContextsOffset fields.
If the server supports directory leasing, Open.Lease is not NULL, and Lease.Version is 2, then the server MUST construct an SMB2_CREATE_RESPONSE_LEASE_V2 create context, following the syntax specified in section 2.2.14.2.11, and include it in the buffer described by the response CreateContextsLength and CreateContextsOffset fields. This structure MUST have the following values set:
LeaseKey MUST be set to Lease.LeaseKey.
LeaseState MUST be set to Lease.LeaseState.
If Lease.ParentLeaseKey is not empty, ParentLeaseKey MUST be set to Lease.ParentLeaseKey, and the SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET bit MUST be set in the Flags field of the response.
If the server supports leasing, Open.Lease is not NULL, and Lease.Version is 1, then the server MUST construct an SMB2_CREATE_RESPONSE_LEASE create context, following the syntax specified in section 2.2.14.2.10, and include it in the buffer described by the response CreateContextsLength and CreateContextsOffset fields. This structure MUST have the following values set:
LeaseKey MUST be set to Lease.LeaseKey.
LeaseState MUST be set to Lease.LeaseState.
If Open.IsPersistent is TRUE, Open.Lease.Breaking is TRUE, and Open.Lease.BreakNotification is not empty, the server MUST send Open.Lease.BreakNotification to the client over an available connection in ConnectionList where Open.ClientGuid matches Connection.ClientGuid. If the server succeeds in sending the notification, the server MUST set Open.Lease.BreakNotification to empty and MUST start the lease break acknowledgment timer as specified in section 3.3.2.5.
```

### New Content
```
The client is requesting a reconnect to an existing durable or resilient open.
There is no processing done for "Path Name Validation" or "Open Execution" as listed in the section above.
The processing changes involved for this create context are:
If the create request also includes an SMB2_CREATE_DURABLE_HANDLE_REQUEST create context, the server MUST ignore the SMB2_CREATE_DURABLE_HANDLE_REQUEST create context.
If the create request also contains an SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 or SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context, the server SHOULD<336> fail the request with STATUS_INVALID_PARAMETER.
The server MUST look up an existing open in the GlobalOpenTable by doing a lookup with the FileId.Persistent portion of the create context. If the lookup fails, the server SHOULD<337> fail the request with STATUS_OBJECT_NAME_NOT_FOUND and proceed as specified in "Failed Open Handling" in section 3.3.5.9.
If any Open.Lease is not NULL and Open.ClientGuid is not equal to the ClientGuid of the connection that received this request, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
If Open.Lease is not NULL, Open.Lease.FileDeleteOnClose is FALSE, and Open.Lease.FileName does not match the file name specified in the Buffer field of the SMB2 CREATE request, the server MUST fail the request with STATUS_INVALID_PARAMETER.
If any of the following conditions is TRUE, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
Open.Lease is not NULL and the SMB2_CREATE_REQUEST_LEASE_V2 or the SMB2_CREATE_REQUEST_LEASE create context is not present.
Open.Lease is NULL and the SMB2_CREATE_REQUEST_LEASE_V2 or the SMB2_CREATE_REQUEST_LEASE create context is present.
Open.IsDurable is TRUE, Open.Lease is NULL, and Open.OplockLevel is not equal to SMB2_OPLOCK_LEVEL_BATCH.
Open.IsDurable is TRUE and Open.Lease.LeaseState does not contain SMB2_LEASE_HANDLE_CACHING.
Open.IsDurable is FALSE and Open.IsResilient is FALSE or unimplemented.
Open.Session is not NULL.
The SMB2_CREATE_REQUEST_LEASE_V2 create context is also present in the request, Connection.Dialect belongs to the SMB 3.x dialect family, the server supports directory leasing, Open.Lease is not NULL, and Open.Lease.LeaseKey does not match the LeaseKey provided in the SMB2_CREATE_REQUEST_LEASE_V2 create context.
The SMB2_CREATE_REQUEST_LEASE create context is also present in the request, Connection.Dialect is "2.1" or belongs to the SMB 3.x dialect family, the server supports leasing, Open.Lease is not NULL, and Open.Lease.LeaseKey does not match the LeaseKey provided in the SMB2_CREATE_REQUEST_LEASE create context.
If Open.Lease is not NULL, the server supports leasing and if Lease.Version is 1 and the request does not contain the SMB2_CREATE_REQUEST_LEASE create context or if Lease.Version is 2 and the request does not contain the SMB2_CREATE_REQUEST_LEASE_V2 create context, the server SHOULD<338> fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
If the user represented by Session.SecurityContext is not the same user denoted by Open.DurableOwner, the server MUST fail the request with STATUS_ACCESS_DENIED and proceed as specified in "Failed Open Handling" in section 3.3.5.9.
The server MUST set the Open.Connection to refer to the connection that received this request.
The server MUST set the Open.Session to refer to the session that received this request.
The server MUST set the Open.TreeConnect to refer to the tree connect that received this request, and Open.TreeConnect.OpenCount MUST be increased by 1.
Open.FileId MUST be set to a generated value that uniquely identifies this Open in Session.OpenTable.
The server MUST insert the open into the Session.OpenTable with the Open.FileId as the new key.
The "Successful Open Initialization" and "Oplock Acquisition" phases MUST be skipped, and processing MUST continue as specified in "Response Construction".
In the "Response Construction" phase:
The server MAY<339> construct an SMB2_CREATE_DURABLE_HANDLE_RESPONSE create context, as specified in section 2.2.14.2.3, and include it in the buffer described by the response CreateContextsLength and CreateContextsOffset fields.
If the server supports directory leasing, Open.Lease is not NULL, and Lease.Version is 2, then the server MUST construct an SMB2_CREATE_RESPONSE_LEASE_V2 create context, following the syntax specified in section 2.2.14.2.11, and include it in the buffer described by the response CreateContextsLength and CreateContextsOffset fields. This structure MUST have the following values set:
LeaseKey MUST be set to Lease.LeaseKey.
LeaseState MUST be set to Lease.LeaseState.
If Lease.ParentLeaseKey is not empty, ParentLeaseKey MUST be set to Lease.ParentLeaseKey, and the SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET bit MUST be set in the Flags field of the response.
If the server supports leasing, Open.Lease is not NULL, and Lease.Version is 1, then the server MUST construct an SMB2_CREATE_RESPONSE_LEASE create context, following the syntax specified in section 2.2.14.2.10, and include it in the buffer described by the response CreateContextsLength and CreateContextsOffset fields. This structure MUST have the following values set:
LeaseKey MUST be set to Lease.LeaseKey.
LeaseState MUST be set to Lease.LeaseState.
If Open.IsPersistent is TRUE, Open.Lease.Breaking is TRUE, and Open.Lease.BreakNotification is not empty, the server MUST send Open.Lease.BreakNotification to the client over an available connection in ConnectionList where Open.ClientGuid matches Connection.ClientGuid. If the server succeeds in sending the notification, the server MUST set Open.Lease.BreakNotification to empty and MUST start the lease break acknowledgment timer as specified in section 3.3.2.5.
```

## Section 3.3.5.9.8: Handling the SMB2_CREATE_REQUEST_LEASE Create Context
**Change type:** Modified

### Old Content
```
This section applies only to servers that implement the SMB 2.1 or 3.x dialect family.
If both SMB2_CREATE_DURABLE_HANDLE_RECONNECT and SMB2_CREATE_REQUEST_LEASE create contexts are present in the request, they are processed as specified in section 3.3.5.9.7, and this section does not apply.
If both SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 and SMB2_CREATE_REQUEST_LEASE create contexts are present in the request, they are processed as specified in section 3.3.5.9.12, and this section does not apply.
If the server does not support leasing, the server MUST ignore the SMB2_CREATE_REQUEST_LEASE Create Context request.
If RequestedOplockLevel is not SMB2_OPLOCK_LEVEL_LEASE, the server SHOULD<338> ignore the SMB2_CREATE_REQUEST_LEASE Create Context request.
By specifying a RequestedOplockLevel of SMB2_OPLOCK_LEVEL_LEASE, the client is requesting that a lease be acquired for this open. If the request does not provide an SMB2_CREATE_REQUEST_LEASE Create Context, the lease request MUST be ignored and Open.OplockLevel MUST be set to SMB2_OPLOCK_LEVEL_NONE.
The processing changes involved in acquiring the lease are:
In the "Path Name Validation" phase, the server MUST attempt to locate a Lease Table by performing a lookup in GlobalLeaseTableList using Connection.ClientGuid as the lookup key. If no LeaseTable is found, one MUST be allocated and the following values set:
LeaseTable.ClientGuid is set to Connection.ClientGuid.
LeaseTable.LeaseList is set to an empty list.
If the allocation fails, the create request MUST be failed with STATUS_INSUFFICIENT_RESOURCES.
The server MUST attempt to locate a Lease by performing a lookup in the LeaseTable.LeaseList using the LeaseKey in the SMB2_CREATE_REQUEST_LEASE as the lookup key. If a lease is found, Lease.FileDeleteOnClose is FALSE, and Lease.Filename does not match the file name for the incoming request, the request MUST be failed with STATUS_INVALID_PARAMETER.
If no lease is found, one MUST be allocated with the following values set:
Lease.LeaseKey is set to the LeaseKey in the SMB2_CREATE_REQUEST_LEASE create context.
Lease.ClientLeaseId is set to a value as specified in section 3.3.1.4.
Lease.Filename is set to the file being opened.
Lease.LeaseState is set to NONE.
Lease.BreakToLeaseState is set to NONE.
Lease.LeaseBreakTimeout is set to 0.
Lease.LeaseOpens is set to an empty list.
Lease.Breaking is set to FALSE.
Lease.FileDeleteOnClose is set to FALSE.
If Connection.Dialect belongs to the SMB 3.x dialect family, Lease.Version is set to 1.
If the allocation fails, the create request MUST be failed with STATUS_INSUFFICIENT_RESOURCES. Otherwise, if a LeaseTable was created it MUST be added to the GlobalLeaseTableList, and if a Lease was created it MUST be added to the LeaseTable.LeaseList.
At this point, execution of create continues as described in 3.3.5.9 until the Oplock Acquisition phase.
During "Oplock Acquisition", if the underlying object store does not support leasing, the server SHOULD fall back to requesting a batch oplock instead of a lease and continue processing as described in "Oplock Acquisition". If the underlying object store does support leasing, the following steps are taken:
If TreeConnect.Share.ForceLevel2Oplock is TRUE, and LeaseState includes SMB2_LEASE_WRITE_CACHING, the server MUST clear the bit SMB2_LEASE_WRITE_CACHING in the LeaseState field.
If Connection.Dialect belongs to the SMB 3.x dialect family, TreeConnect.Share.Type is STYPE_CLUSTER_SOFS as specified in [MS-SRVS] section 2.2.2.4, and if LeaseState includes SMB2_LEASE_READ_CACHING, the server MUST set LeaseState to SMB2_LEASE_READ_CACHING, otherwise set LeaseState to SMB2_LEASE_NONE.
If the caching state requested in LeaseState of the SMB2_CREATE_REQUEST_LEASE is not a superset of Lease.LeaseState or if Lease.Breaking is TRUE, the server MUST NOT promote Lease.LeaseState. If the lease state requested is a superset of Lease.LeaseState and Lease.Breaking is FALSE, the server MUST request promotion of the lease state from the underlying object store to the new caching state.<339>
If the object store succeeds this request, Lease.LeaseState MUST be set to the new caching state. If Lease.Breaking is TRUE, the server MUST return the existing Lease.LeaseState to client and set LeaseFlags to be SMB2_LEASE_FLAG_BREAK_IN_PROGRESS. At this point, execution continues as described in section 3.3.5.9 until the "Response Construction" phase.
In the "Response Construction" phase, the server MUST construct an SMB2_CREATE_RESPONSE_LEASE response create context, following the syntax specified in section 2.2.14.2.10, and include it in the buffer described by the response CreateContextsLength and CreateContextsOffset. This structure MUST have the following values set:
LeaseKey MUST be set to Lease.LeaseKey.
LeaseState MUST be set to Lease.LeaseState.
The server MUST set Open.OplockState to Held, set Open.Lease to a reference to Lease, set Open.OplockLevel to SMB2_OPLOCK_LEVEL_LEASE, and add Open to Lease.LeaseOpens. If this Open is the first open in Lease.LeaseOpens, the server MUST set Lease.Held to TRUE. The remainder of open response construction continues as described in "Response Construction".
If Open.Lease is not NULL and CreateOptions field in the CREATE request includes FILE_DELETE_ON_CLOSE, the server MUST set Open.Lease.FileDeleteOnClose to TRUE.
```

### New Content
```
This section applies only to servers that implement the SMB 2.1 or 3.x dialect family.
If both SMB2_CREATE_DURABLE_HANDLE_RECONNECT and SMB2_CREATE_REQUEST_LEASE create contexts are present in the request, they are processed as specified in section 3.3.5.9.7, and this section does not apply.
If both SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 and SMB2_CREATE_REQUEST_LEASE create contexts are present in the request, they are processed as specified in section 3.3.5.9.12, and this section does not apply.
If the server does not support leasing, the server MUST ignore the SMB2_CREATE_REQUEST_LEASE Create Context request.
If RequestedOplockLevel is not SMB2_OPLOCK_LEVEL_LEASE, the server SHOULD<340> ignore the SMB2_CREATE_REQUEST_LEASE Create Context request.
By specifying a RequestedOplockLevel of SMB2_OPLOCK_LEVEL_LEASE, the client is requesting that a lease be acquired for this open. If the request does not provide an SMB2_CREATE_REQUEST_LEASE Create Context, the lease request MUST be ignored and Open.OplockLevel MUST be set to SMB2_OPLOCK_LEVEL_NONE.
The processing changes involved in acquiring the lease are:
In the "Path Name Validation" phase, the server MUST attempt to locate a Lease Table by performing a lookup in GlobalLeaseTableList using Connection.ClientGuid as the lookup key. If no LeaseTable is found, one MUST be allocated and the following values set:
LeaseTable.ClientGuid is set to Connection.ClientGuid.
LeaseTable.LeaseList is set to an empty list.
If the allocation fails, the create request MUST be failed with STATUS_INSUFFICIENT_RESOURCES.
The server MUST attempt to locate a Lease by performing a lookup in the LeaseTable.LeaseList using the LeaseKey in the SMB2_CREATE_REQUEST_LEASE as the lookup key. If a lease is found, Lease.FileDeleteOnClose is FALSE, and Lease.Filename does not match the file name for the incoming request, the request MUST be failed with STATUS_INVALID_PARAMETER.
If no lease is found, one MUST be allocated with the following values set:
Lease.LeaseKey is set to the LeaseKey in the SMB2_CREATE_REQUEST_LEASE create context.
Lease.ClientLeaseId is set to a value as specified in section 3.3.1.4.
Lease.Filename is set to the file being opened.
Lease.LeaseState is set to NONE.
Lease.BreakToLeaseState is set to NONE.
Lease.LeaseBreakTimeout is set to 0.
Lease.LeaseOpens is set to an empty list.
Lease.Breaking is set to FALSE.
Lease.FileDeleteOnClose is set to FALSE.
If Connection.Dialect belongs to the SMB 3.x dialect family, Lease.Version is set to 1.
If the allocation fails, the create request MUST be failed with STATUS_INSUFFICIENT_RESOURCES. Otherwise, if a LeaseTable was created it MUST be added to the GlobalLeaseTableList, and if a Lease was created it MUST be added to the LeaseTable.LeaseList.
At this point, execution of create continues as described in 3.3.5.9 until the Oplock Acquisition phase.
During "Oplock Acquisition", if the underlying object store does not support leasing, the server SHOULD fall back to requesting a batch oplock instead of a lease and continue processing as described in "Oplock Acquisition". If the underlying object store does support leasing, the following steps are taken:
If TreeConnect.Share.ForceLevel2Oplock is TRUE, and LeaseState includes SMB2_LEASE_WRITE_CACHING, the server MUST clear the bit SMB2_LEASE_WRITE_CACHING in the LeaseState field.
If Connection.Dialect belongs to the SMB 3.x dialect family, TreeConnect.Share.Type is STYPE_CLUSTER_SOFS as specified in [MS-SRVS] section 2.2.2.4, and if LeaseState includes SMB2_LEASE_READ_CACHING, the server MUST set LeaseState to SMB2_LEASE_READ_CACHING, otherwise set LeaseState to SMB2_LEASE_NONE.
If the caching state requested in LeaseState of the SMB2_CREATE_REQUEST_LEASE is not a superset of Lease.LeaseState or if Lease.Breaking is TRUE, the server MUST NOT promote Lease.LeaseState. If the lease state requested is a superset of Lease.LeaseState and Lease.Breaking is FALSE, the server MUST request promotion of the lease state from the underlying object store to the new caching state.<341>
If the object store succeeds this request, Lease.LeaseState MUST be set to the new caching state. If Lease.Breaking is TRUE, the server MUST return the existing Lease.LeaseState to client and set LeaseFlags to be SMB2_LEASE_FLAG_BREAK_IN_PROGRESS. At this point, execution continues as described in section 3.3.5.9 until the "Response Construction" phase.
In the "Response Construction" phase, the server MUST construct an SMB2_CREATE_RESPONSE_LEASE response create context, following the syntax specified in section 2.2.14.2.10, and include it in the buffer described by the response CreateContextsLength and CreateContextsOffset. This structure MUST have the following values set:
LeaseKey MUST be set to Lease.LeaseKey.
LeaseState MUST be set to Lease.LeaseState.
The server MUST set Open.OplockState to Held, set Open.Lease to a reference to Lease, set Open.OplockLevel to SMB2_OPLOCK_LEVEL_LEASE, and add Open to Lease.LeaseOpens. If this Open is the first open in Lease.LeaseOpens, the server MUST set Lease.Held to TRUE. The remainder of open response construction continues as described in "Response Construction".
If Open.Lease is not NULL and CreateOptions field in the CREATE request includes FILE_DELETE_ON_CLOSE, the server MUST set Open.Lease.FileDeleteOnClose to TRUE.
```

## Section 3.3.5.9.10: Handling the SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 Create Context
**Change type:** Modified

### Old Content
```
This section applies only to servers that implement the SMB 3.x dialect family.
If the create request also includes an SMB2_CREATE_DURABLE_HANDLE_REQUEST create context, or an SMB2_CREATE_DURABLE_HANDLE_RECONNECT or SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context, the server MUST fail the create request with STATUS_INVALID_PARAMETER.
If the create request also includes the SMB2_CREATE_APP_INSTANCE_ID create context, the server MUST process the SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 create context only after processing the SMB2_CREATE_APP_INSTANCE_ID create context.
The server MUST locate the Open in GlobalOpenTable where Open.IsReplayEligible is TRUE and Open.CreateGuid matches the CreateGuid in the SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 create context, and Open.ClientGuid matches the ClientGuid of the connection that received this request.
If an Open is not found, the server MUST continue the create process specified in the "Open Execution" Phase, and perform the following additional steps:
The server MUST set Open.CreateGuid to the CreateGuid in SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2.
In the "Successful Open Initialization" phase, the server MUST perform the following:
If Open.FileAttributes includes FILE_ATTRIBUTE_DIRECTORY or the underlying object store does not grant durability, the server MUST skip the rest of the processing in this section.
If the underlying object store grants durability, the server MUST perform the following:
The server MUST set Open.IsDurable to TRUE.
The server MUST also set Open.DurableOwner to a security descriptor accessible only by the user represented by Open.Session.SecurityContext.
If the SMB2_DHANDLE_FLAG_PERSISTENT bit is set in the Flags field of the request, TreeConnect.Share.IsCA is TRUE, and Connection.ServerCapabilities includes SMB2_GLOBAL_CAP_PERSISTENT_HANDLES, the server MUST set Open.IsPersistent to TRUE.
The server MUST set Open.IsReplayEligible to TRUE if TreeConnect.Share.Type is STYPE_DISKTREE and any of the following conditions are satisfied:
Open.IsPersistent is TRUE.
Open.FileAttributes does not include FILE_ATTRIBUTE_DIRECTORY and Open.GrantedAccess includes FILE_READ_DATA, FILE_EXECUTE, FILE_WRITE_DATA, FILE_APPEND_DATA, or DELETE access.
If an Open is found and the SMB2_FLAGS_REPLAY_OPERATION bit is not set in the SMB2 header, the server MUST fail the request with STATUS_DUPLICATE_OBJECTID.
If an Open is found and the SMB2_FLAGS_REPLAY_OPERATION bit is set in the SMB2 header, the server MUST set Open.Connection to the connection that received this request.
If Open.IsDurable is TRUE, the server SHOULD<340> construct an SMB2_CREATE_DURABLE_HANDLE_RESPONSE_V2 response create context, with the following values set, as specified in section 2.2.14.2.12.
If Open.IsPersistent is TRUE, the server MUST set the SMB2_DHANDLE_FLAG_PERSISTENT bit in the Flags field.
The Buffer specified by the response MUST include the CreateContextsLength and CreateContextsOffset fields.
If SMB2_FLAGS_REPLAY_OPERATION bit is set in the SMB2 header, Timeout field MUST be set to Open.DurableOpenTimeout.
Otherwise, the server MUST perform the following:
If the Timeout value in the request is not zero, the Timeout value in the response SHOULD<341> be set to whichever is smaller, the Timeout value in the request or 300 seconds.
If the Timeout value in the request is zero, the Timeout value in the response SHOULD<342> be set to an implementation-specific value.
Open.DurableOpenTimeout MUST be set to the Timeout value in the response.
```

### New Content
```
This section applies only to servers that implement the SMB 3.x dialect family.
If the create request also includes an SMB2_CREATE_DURABLE_HANDLE_REQUEST create context, or an SMB2_CREATE_DURABLE_HANDLE_RECONNECT or SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context, the server MUST fail the create request with STATUS_INVALID_PARAMETER.
If the create request also includes the SMB2_CREATE_APP_INSTANCE_ID create context, the server MUST process the SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 create context only after processing the SMB2_CREATE_APP_INSTANCE_ID create context.
The server MUST locate the Open in GlobalOpenTable where Open.IsReplayEligible is TRUE and Open.CreateGuid matches the CreateGuid in the SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 create context, and Open.ClientGuid matches the ClientGuid of the connection that received this request.
If an Open is not found, the server MUST continue the create process specified in the "Open Execution" Phase, and perform the following additional steps:
The server MUST set Open.CreateGuid to the CreateGuid in SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2.
In the "Successful Open Initialization" phase, the server MUST perform the following:
If Open.FileAttributes includes FILE_ATTRIBUTE_DIRECTORY or the underlying object store does not grant durability, the server MUST skip the rest of the processing in this section.
If the underlying object store grants durability, the server MUST perform the following:
The server MUST set Open.IsDurable to TRUE.
The server MUST also set Open.DurableOwner to a security descriptor accessible only by the user represented by Open.Session.SecurityContext.
If the SMB2_DHANDLE_FLAG_PERSISTENT bit is set in the Flags field of the request, TreeConnect.Share.IsCA is TRUE, and Connection.ServerCapabilities includes SMB2_GLOBAL_CAP_PERSISTENT_HANDLES, the server MUST set Open.IsPersistent to TRUE.
The server MUST set Open.IsReplayEligible to TRUE if TreeConnect.Share.Type is STYPE_DISKTREE and any of the following conditions are satisfied:
Open.IsPersistent is TRUE.
Open.FileAttributes does not include FILE_ATTRIBUTE_DIRECTORY and Open.GrantedAccess includes FILE_READ_DATA, FILE_EXECUTE, FILE_WRITE_DATA, FILE_APPEND_DATA, or DELETE access.
If an Open is found and the SMB2_FLAGS_REPLAY_OPERATION bit is not set in the SMB2 header, the server MUST fail the request with STATUS_DUPLICATE_OBJECTID.
If an Open is found and the SMB2_FLAGS_REPLAY_OPERATION bit is set in the SMB2 header, the server MUST set Open.Connection to the connection that received this request.
If Open.IsDurable is TRUE, the server SHOULD<342> construct an SMB2_CREATE_DURABLE_HANDLE_RESPONSE_V2 response create context, with the following values set, as specified in section 2.2.14.2.12.
If Open.IsPersistent is TRUE, the server MUST set the SMB2_DHANDLE_FLAG_PERSISTENT bit in the Flags field.
The Buffer specified by the response MUST include the CreateContextsLength and CreateContextsOffset fields.
If SMB2_FLAGS_REPLAY_OPERATION bit is set in the SMB2 header, Timeout field MUST be set to Open.DurableOpenTimeout.
Otherwise, the server MUST perform the following:
If the Timeout value in the request is not zero, the Timeout value in the response SHOULD<343> be set to whichever is smaller, the Timeout value in the request or 300 seconds.
If the Timeout value in the request is zero, the Timeout value in the response SHOULD<344> be set to an implementation-specific value.
Open.DurableOpenTimeout MUST be set to the Timeout value in the response.
```

## Section 3.3.5.9.11: Handling the SMB2_CREATE_REQUEST_LEASE_V2 Create Context
**Change type:** Modified

### Old Content
```
This section applies only to servers that implement the SMB 3.x dialect family.
If both SMB2_CREATE_DURABLE_HANDLE_RECONNECT and SMB2_CREATE_REQUEST_LEASE_V2 create contexts are present in the request, they are processed as specified in section 3.3.5.9.7, and this section does not apply.
If both SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 and SMB2_CREATE_REQUEST_LEASE_V2 create contexts are present in the request, they are processed as specified in section 3.3.5.9.12, and this section does not apply.
If the server does not support leasing, the server MUST ignore the SMB2_CREATE_REQUEST_LEASE_V2 Create Context request.
If Connection.Dialect does not belong to the SMB 3.x dialect family or if RequestedOplockLevel is not SMB2_OPLOCK_LEVEL_LEASE, the server SHOULD<343> ignore the SMB2_CREATE_REQUEST_LEASE_V2 Create Context request.
By specifying a RequestedOplockLevel of SMB2_OPLOCK_LEVEL_LEASE, the client is requesting that a lease be acquired for this open. If the request does not provide an SMB2_CREATE_REQUEST_LEASE_V2 Create Context, the lease request MUST be ignored and Open.OplockLevel MUST be set to SMB2_OPLOCK_LEVEL_NONE.
The processing changes involved in acquiring the lease are:
In the "Path Name Validation" phase, the server MUST attempt to locate a Lease Table by performing a lookup in GlobalLeaseTableList using Connection.ClientGuid as the lookup key. If no LeaseTable is found, one MUST be allocated and the following values set:
LeaseTable.ClientGuid is set to Connection.ClientGuid.
LeaseTable.LeaseList is set to an empty list.
If the allocation fails, the create request MUST be failed with STATUS_INSUFFICIENT_RESOURCES.
The server MUST attempt to locate a Lease by performing a lookup in the LeaseTable.LeaseList using the LeaseKey in the SMB2_CREATE_REQUEST_LEASE_V2 as the lookup key. If a lease is found, Lease.FileDeleteOnClose is FALSE, and Lease.Filename does not match the file name for the incoming request, the request MUST be failed with STATUS_INVALID_PARAMETER.
If a lease is found, the server MUST construct an SMB2_CREATE_RESPONSE_LEASE_V2 response create context as specified below.
If no lease is found, one MUST be allocated with the following values set:
Lease.LeaseKey is set to the LeaseKey in the SMB2_CREATE_REQUEST_LEASE_V2 create context.
If the SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET bit is set in the Flags field of the request, Lease.ParentLeaseKey MUST be set to the ParentLeaseKey of the request.
Lease.ClientLeaseId is set to a value as specified in section 3.3.1.4
Lease.Filename is set to the file being opened.
Lease.LeaseState is set to NONE.
Lease.BreakToLeaseState is set to NONE.
Lease.LeaseBreakTimeout is set to 0.
Lease.LeaseOpens is set to an empty list.
Lease.Breaking is set to FALSE.
Lease.Epoch is set to 0.
Lease.FileDeleteOnClose is set to FALSE.
Lease.Version is set to 2.
If the allocation fails, the create request MUST be failed with STATUS_INSUFFICIENT_RESOURCES. Otherwise, if a LeaseTable was created it MUST be added to the GlobalLeaseTableList, and if a Lease was created it MUST be added to the LeaseTable.LeaseList.
At this point, execution of create continues as described in 3.3.5.9 until the "Oplock Acquisition" phase.
During "Oplock Acquisition", if the underlying object store does not support leasing, the server SHOULD fall back to requesting a batch oplock instead of a lease and continue processing as described in "Oplock Acquisition". If the underlying object store does support leasing, the following steps are taken:
If TreeConnect.Share.ForceLevel2Oplock is TRUE, and LeaseState includes SMB2_LEASE_WRITE_CACHING, the server MUST clear the bit SMB2_LEASE_WRITE_CACHING in the LeaseState field.
If the FileAttributes field in the request includes FILE_ATTRIBUTE_DIRECTORY and LeaseState includes SMB2_LEASE_WRITE_CACHING, the server MUST clear the bit SMB2_LEASE_WRITE_CACHING in the LeaseState field.
If TreeConnect.Share.Type is STYPE_CLUSTER_SOFS as specified in [MS-SRVS] section 2.2.2.4, and if LeaseState includes SMB2_LEASE_READ_CACHING, the server MUST set LeaseState to SMB2_LEASE_READ_CACHING, otherwise set LeaseState to SMB2_LEASE_NONE.
If the caching state requested in LeaseState of the SMB2_CREATE_REQUEST_LEASE_V2 is not a superset of Lease.LeaseState or if Lease.Breaking is TRUE, the server MUST NOT promote Lease.LeaseState. If the lease state requested is a superset of Lease.LeaseState and Lease.Breaking is FALSE, the server MUST request promotion of the lease state from the underlying object store to the new caching state.<344>
If the object store succeeds this request, Lease.LeaseState MUST be set to the new caching state. The server MUST increment Lease.Epoch by 1. If Lease.Breaking is TRUE, the server MUST return the existing Lease.LeaseState to client and set Flags to be SMB2_LEASE_FLAG_BREAK_IN_PROGRESS. At this point, execution continues as described in section 3.3.5.9 until the "Response Construction" phase.
In the "Response Construction" phase, the server MUST construct an SMB2_CREATE_RESPONSE_LEASE_V2 response create context, following the syntax specified in section 2.2.14.2.11, and include it in the buffer described by the response CreateContextsLength and CreateContextsOffset. This structure MUST have the following values set:
LeaseKey MUST be set to Lease.LeaseKey.
LeaseState MUST be set to Lease.LeaseState.
If Lease.ParentLeaseKey is not empty, ParentLeaseKey MUST be set to Lease.ParentLeaseKey, and the SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET bit MUST be set in the Flags field of the response.
Epoch MUST be set to Lease.Epoch.
The server MUST set Open.OplockState to Held, set Open.Lease to a reference to Lease, set Open.OplockLevel to SMB2_OPLOCK_LEVEL_LEASE, and add Open to Lease.LeaseOpens. If this Open is the first open in Lease.LeaseOpens, the server MUST set Lease.Held to TRUE. The remainder of open response construction continues as described in the "Response Construction" phase.
If Open.Lease is not NULL and CreateOptions field in the CREATE request includes FILE_DELETE_ON_CLOSE, the server MUST set Open.Lease.FileDeleteOnClose to TRUE.
```

### New Content
```
This section applies only to servers that implement the SMB 3.x dialect family.
If both SMB2_CREATE_DURABLE_HANDLE_RECONNECT and SMB2_CREATE_REQUEST_LEASE_V2 create contexts are present in the request, they are processed as specified in section 3.3.5.9.7, and this section does not apply.
If both SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 and SMB2_CREATE_REQUEST_LEASE_V2 create contexts are present in the request, they are processed as specified in section 3.3.5.9.12, and this section does not apply.
If the server does not support leasing, the server MUST ignore the SMB2_CREATE_REQUEST_LEASE_V2 Create Context request.
If Connection.Dialect does not belong to the SMB 3.x dialect family or if RequestedOplockLevel is not SMB2_OPLOCK_LEVEL_LEASE, the server SHOULD<345> ignore the SMB2_CREATE_REQUEST_LEASE_V2 Create Context request.
By specifying a RequestedOplockLevel of SMB2_OPLOCK_LEVEL_LEASE, the client is requesting that a lease be acquired for this open. If the request does not provide an SMB2_CREATE_REQUEST_LEASE_V2 Create Context, the lease request MUST be ignored and Open.OplockLevel MUST be set to SMB2_OPLOCK_LEVEL_NONE.
The processing changes involved in acquiring the lease are:
In the "Path Name Validation" phase, the server MUST attempt to locate a Lease Table by performing a lookup in GlobalLeaseTableList using Connection.ClientGuid as the lookup key. If no LeaseTable is found, one MUST be allocated and the following values set:
LeaseTable.ClientGuid is set to Connection.ClientGuid.
LeaseTable.LeaseList is set to an empty list.
If the allocation fails, the create request MUST be failed with STATUS_INSUFFICIENT_RESOURCES.
The server MUST attempt to locate a Lease by performing a lookup in the LeaseTable.LeaseList using the LeaseKey in the SMB2_CREATE_REQUEST_LEASE_V2 as the lookup key. If a lease is found, Lease.FileDeleteOnClose is FALSE, and Lease.Filename does not match the file name for the incoming request, the request MUST be failed with STATUS_INVALID_PARAMETER.
If a lease is found, the server MUST construct an SMB2_CREATE_RESPONSE_LEASE_V2 response create context as specified below.
If no lease is found, one MUST be allocated with the following values set:
Lease.LeaseKey is set to the LeaseKey in the SMB2_CREATE_REQUEST_LEASE_V2 create context.
If the SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET bit is set in the Flags field of the request, Lease.ParentLeaseKey MUST be set to the ParentLeaseKey of the request.
Lease.ClientLeaseId is set to a value as specified in section 3.3.1.4
Lease.Filename is set to the file being opened.
Lease.LeaseState is set to NONE.
Lease.BreakToLeaseState is set to NONE.
Lease.LeaseBreakTimeout is set to 0.
Lease.LeaseOpens is set to an empty list.
Lease.Breaking is set to FALSE.
Lease.Epoch is set to 0.
Lease.FileDeleteOnClose is set to FALSE.
Lease.Version is set to 2.
If the allocation fails, the create request MUST be failed with STATUS_INSUFFICIENT_RESOURCES. Otherwise, if a LeaseTable was created it MUST be added to the GlobalLeaseTableList, and if a Lease was created it MUST be added to the LeaseTable.LeaseList.
At this point, execution of create continues as described in 3.3.5.9 until the "Oplock Acquisition" phase.
During "Oplock Acquisition", if the underlying object store does not support leasing, the server SHOULD fall back to requesting a batch oplock instead of a lease and continue processing as described in "Oplock Acquisition". If the underlying object store does support leasing, the following steps are taken:
If TreeConnect.Share.ForceLevel2Oplock is TRUE, and LeaseState includes SMB2_LEASE_WRITE_CACHING, the server MUST clear the bit SMB2_LEASE_WRITE_CACHING in the LeaseState field.
If the FileAttributes field in the request includes FILE_ATTRIBUTE_DIRECTORY and LeaseState includes SMB2_LEASE_WRITE_CACHING, the server MUST clear the bit SMB2_LEASE_WRITE_CACHING in the LeaseState field.
If TreeConnect.Share.Type is STYPE_CLUSTER_SOFS as specified in [MS-SRVS] section 2.2.2.4, and if LeaseState includes SMB2_LEASE_READ_CACHING, the server MUST set LeaseState to SMB2_LEASE_READ_CACHING, otherwise set LeaseState to SMB2_LEASE_NONE.
If the caching state requested in LeaseState of the SMB2_CREATE_REQUEST_LEASE_V2 is not a superset of Lease.LeaseState or if Lease.Breaking is TRUE, the server MUST NOT promote Lease.LeaseState. If the lease state requested is a superset of Lease.LeaseState and Lease.Breaking is FALSE, the server MUST request promotion of the lease state from the underlying object store to the new caching state.<346>
If the object store succeeds this request, Lease.LeaseState MUST be set to the new caching state. The server MUST increment Lease.Epoch by 1. If Lease.Breaking is TRUE, the server MUST return the existing Lease.LeaseState to client and set Flags to be SMB2_LEASE_FLAG_BREAK_IN_PROGRESS. At this point, execution continues as described in section 3.3.5.9 until the "Response Construction" phase.
In the "Response Construction" phase, the server MUST construct an SMB2_CREATE_RESPONSE_LEASE_V2 response create context, following the syntax specified in section 2.2.14.2.11, and include it in the buffer described by the response CreateContextsLength and CreateContextsOffset. This structure MUST have the following values set:
LeaseKey MUST be set to Lease.LeaseKey.
LeaseState MUST be set to Lease.LeaseState.
If Lease.ParentLeaseKey is not empty, ParentLeaseKey MUST be set to Lease.ParentLeaseKey, and the SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET bit MUST be set in the Flags field of the response.
Epoch MUST be set to Lease.Epoch.
The server MUST set Open.OplockState to Held, set Open.Lease to a reference to Lease, set Open.OplockLevel to SMB2_OPLOCK_LEVEL_LEASE, and add Open to Lease.LeaseOpens. If this Open is the first open in Lease.LeaseOpens, the server MUST set Lease.Held to TRUE. The remainder of open response construction continues as described in the "Response Construction" phase.
If Open.Lease is not NULL and CreateOptions field in the CREATE request includes FILE_DELETE_ON_CLOSE, the server MUST set Open.Lease.FileDeleteOnClose to TRUE.
```

## Section 3.3.5.9.12: Handling the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 Create Context
**Change type:** Modified

### Old Content
```
This section applies only to servers that implement the SMB 3.x dialect family.
There is no processing done for "Path Name Validation" as listed in section 3.3.5.9.
The processing changes involved for this create context are:
The server MUST look up an existing Open in the GlobalOpenTable by doing a lookup with the FileId.Persistent portion of the create context.
If the lookup fails:
If the request includes the SMB2_DHANDLE_FLAG_PERSISTENT bit in the Flags field of the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context, TreeConnect.Share.IsCA is TRUE, and Connection.ServerCapabilities includes SMB2_GLOBAL_CAP_PERSISTENT_HANDLES, the server MUST look up an existing Open in the GlobalOpenTable by doing a lookup with the CreateGuid of the create context. If the lookup fails, the server SHOULD<345> fail the request with STATUS_OBJECT_NAME_NOT_FOUND and proceed as specified in "Failed Open Handling" in section 3.3.5.9.
Otherwise, the server SHOULD<346> fail the request with STATUS_OBJECT_NAME_NOT_FOUND and proceed as specified in "Failed Open Handling" in section 3.3.5.9.
If any of the following conditions is TRUE, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND:
Open.Lease is not NULL and Open.ClientGuid is not equal to the ClientGuid of the connection that received this request.
If Open.IsPersistent is TRUE and the SMB2_DHANDLE_FLAG_PERSISTENT bit is not set in the Flags field of the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 Create Context, the server SHOULD<347> fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
Open.CreateGuid is not equal to the CreateGuid in the request.
Open.IsDurable is FALSE and Open.IsResilient is FALSE or unimplemented.
Open.Session is not NULL.
Open.Lease is NULL and the SMB2_CREATE_REQUEST_LEASE or SMB2_CREATE_REQUEST_LEASE_V2 create context is present.
Open.IsDurable is TRUE, Open.Lease is NULL, and Open.OplockLevel is not equal to SMB2_OPLOCK_LEVEL_BATCH.
Open.Lease is NOT NULL and the SMB2_CREATE_REQUEST_LEASE or SMB2_CREATE_REQUEST_LEASE_V2 create context is not present.
Open.IsDurable is TRUE and Open.Lease.LeaseState does not contain SMB2_LEASE_HANDLE_CACHING.
The SMB2_CREATE_REQUEST_LEASE_V2 create context is also present in the request, the server supports directory leasing, and Open.Lease.LeaseKey does not match the LeaseKey provided in the SMB2_CREATE_REQUEST_LEASE_V2 create context.
The SMB2_CREATE_REQUEST_LEASE create context is also present in the request, the server supports leasing, and Open.Lease.LeaseKey does not match the LeaseKey provided in the SMB2_CREATE_REQUEST_LEASE create context.
If Open.Lease is not NULL, the server supports leasing, Lease.Version is 1, and the request does not contain the SMB2_CREATE_REQUEST_LEASE create context, or if Lease.Version is 2 and the request does not contain the SMB2_CREATE_REQUEST_LEASE_V2 create context, the server SHOULD<348> fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
If any of the following conditions is TRUE, the server MUST fail the request with STATUS_INVALID_PARAMETER:
The CREATE request also contains the SMB2_CREATE_DURABLE_HANDLE_REQUEST context, the SMB2_CREATE_DURABLE_HANDLE_RECONNECT context, or the SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 context.
Open.Lease is not NULL, Open.Lease.FileDeleteOnClose is FALSE, and Open.Lease.FileName does not match the file name specified in the Buffer field of the SMB2 CREATE request.
If Open.IsPersistent is FALSE and the SMB2_DHANDLE_FLAG_PERSISTENT bit is set in the Flags field of the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 Create Context, the server SHOULD<349> fail the request with STATUS_INVALID_PARAMETER.
If the user represented by Session.SecurityContext is not the same user denoted by Open.DurableOwner, the server MUST fail the request with STATUS_ACCESS_DENIED and proceed as specified in "Failed Open Handling" in section 3.3.5.9.
The server MUST set the Open.Connection to refer to the connection that received this request.
The server MUST set the Open.Session to refer to the session that received this request.
The server MUST set the Open.TreeConnect to refer to the tree connect that received this request, and Open.TreeConnect.OpenCount MUST be increased by 1.
Open.FileId MUST be set to a generated value that uniquely identifies this Open in Session.OpenTable.
The server MUST insert the Open into the Session.OpenTable with the Open.FileId as the new key.
If Open.IsSharedVHDX and Open.IsPersistent are TRUE, the request MUST be processed as specified in [MS-RSVD] section 3.2.5.1 by providing Open.LocalOpen.
The "Successful Open Initialization" and "Oplock Acquisition" phases MUST be skipped, and processing MUST continue as specified in "Response Construction".
In the "Response Construction" phase:
If the server supports directory leasing, Open.Lease is not NULL, and Lease.Version is 2, then the server MUST construct an SMB2_CREATE_RESPONSE_LEASE_V2 create context that follows the syntax specified in section 2.2.14.2.11, and include it in the buffer described by the response CreateContextsLength and CreateContextsOffset fields. This structure MUST have the following values set:
LeaseKey MUST be set to Lease.LeaseKey.
LeaseState MUST be set to Lease.LeaseState.
If Lease.ParentLeaseKey is not empty, ParentLeaseKey MUST be set to Lease.ParentLeaseKey, and the SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET bit MUST be set in the Flags field of the response.
Epoch SHOULD<350> be set to Lease.Epoch.
If the server supports leasing, Open.Lease is not NULL, and Lease.Version is 1, then the server MUST construct an SMB2_CREATE_RESPONSE_LEASE create context that follows the syntax specified in section 2.2.14.2.10, and include it in the buffer described by the response CreateContextsLength and CreateContextsOffset fields. This structure MUST have the following values set:
LeaseKey MUST be set to Lease.LeaseKey.
LeaseState MUST be set to Lease.LeaseState.
If Open.IsPersistent is TRUE, Open.Lease.Breaking is TRUE, and Open.Lease.BreakNotification is not empty, the server MUST send Open.Lease.BreakNotification to the client over an available connection in ConnectionList where Open.ClientGuid matches Connection.ClientGuid. If the server succeeds in sending the notification, the server MUST set Open.Lease.BreakNotification to empty and MUST start the lease break acknowledgment timer as specified in section 3.3.2.5.
```

### New Content
```
This section applies only to servers that implement the SMB 3.x dialect family.
There is no processing done for "Path Name Validation" as listed in section 3.3.5.9.
The processing changes involved for this create context are:
The server MUST look up an existing Open in the GlobalOpenTable by doing a lookup with the FileId.Persistent portion of the create context.
If the lookup fails:
If the request includes the SMB2_DHANDLE_FLAG_PERSISTENT bit in the Flags field of the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context, TreeConnect.Share.IsCA is TRUE, and Connection.ServerCapabilities includes SMB2_GLOBAL_CAP_PERSISTENT_HANDLES, the server MUST look up an existing Open in the GlobalOpenTable by doing a lookup with the CreateGuid of the create context. If the lookup fails, the server SHOULD<347> fail the request with STATUS_OBJECT_NAME_NOT_FOUND and proceed as specified in "Failed Open Handling" in section 3.3.5.9.
Otherwise, the server SHOULD<348> fail the request with STATUS_OBJECT_NAME_NOT_FOUND and proceed as specified in "Failed Open Handling" in section 3.3.5.9.
If any of the following conditions is TRUE, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND:
Open.Lease is not NULL and Open.ClientGuid is not equal to the ClientGuid of the connection that received this request.
If Open.IsPersistent is TRUE and the SMB2_DHANDLE_FLAG_PERSISTENT bit is not set in the Flags field of the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 Create Context, the server SHOULD<349> fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
Open.CreateGuid is not equal to the CreateGuid in the request.
Open.IsDurable is FALSE and Open.IsResilient is FALSE or unimplemented.
Open.Session is not NULL.
Open.Lease is NULL and the SMB2_CREATE_REQUEST_LEASE or SMB2_CREATE_REQUEST_LEASE_V2 create context is present.
Open.IsDurable is TRUE, Open.Lease is NULL, and Open.OplockLevel is not equal to SMB2_OPLOCK_LEVEL_BATCH.
Open.Lease is NOT NULL and the SMB2_CREATE_REQUEST_LEASE or SMB2_CREATE_REQUEST_LEASE_V2 create context is not present.
The SMB2_CREATE_REQUEST_LEASE_V2 create context is also present in the request, the server supports directory leasing, and Open.Lease.LeaseKey does not match the LeaseKey provided in the SMB2_CREATE_REQUEST_LEASE_V2 create context.
The SMB2_CREATE_REQUEST_LEASE create context is also present in the request, the server supports leasing, and Open.Lease.LeaseKey does not match the LeaseKey provided in the SMB2_CREATE_REQUEST_LEASE create context.
If Open.IsDurable is TRUE and Open.Lease.LeaseState does not contain SMB2_LEASE_HANDLE_CACHING, the server SHOULD<350> fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
If Open.Lease is not NULL, the server supports leasing, Lease.Version is 1, and the request does not contain the SMB2_CREATE_REQUEST_LEASE create context, or if Lease.Version is 2 and the request does not contain the SMB2_CREATE_REQUEST_LEASE_V2 create context, the server SHOULD<351> fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
If any of the following conditions is TRUE, the server MUST fail the request with STATUS_INVALID_PARAMETER:
The CREATE request also contains the SMB2_CREATE_DURABLE_HANDLE_REQUEST context, the SMB2_CREATE_DURABLE_HANDLE_RECONNECT context, or the SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 context.
Open.Lease is not NULL, Open.Lease.FileDeleteOnClose is FALSE, and Open.Lease.FileName does not match the file name specified in the Buffer field of the SMB2 CREATE request.
If Open.IsPersistent is FALSE and the SMB2_DHANDLE_FLAG_PERSISTENT bit is set in the Flags field of the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 Create Context, the server SHOULD<352> fail the request with STATUS_INVALID_PARAMETER.
If the user represented by Session.SecurityContext is not the same user denoted by Open.DurableOwner, the server MUST fail the request with STATUS_ACCESS_DENIED and proceed as specified in "Failed Open Handling" in section 3.3.5.9.
The server MUST set the Open.Connection to refer to the connection that received this request.
The server MUST set the Open.Session to refer to the session that received this request.
The server MUST set the Open.TreeConnect to refer to the tree connect that received this request, and Open.TreeConnect.OpenCount MUST be increased by 1.
Open.FileId MUST be set to a generated value that uniquely identifies this Open in Session.OpenTable.
The server MUST insert the Open into the Session.OpenTable with the Open.FileId as the new key.
If Open.IsSharedVHDX and Open.IsPersistent are TRUE, the request MUST be processed as specified in [MS-RSVD] section 3.2.5.1 by providing Open.LocalOpen.
The "Successful Open Initialization" and "Oplock Acquisition" phases MUST be skipped, and processing MUST continue as specified in "Response Construction".
In the "Response Construction" phase:
If the server supports directory leasing, Open.Lease is not NULL, and Lease.Version is 2, then the server MUST construct an SMB2_CREATE_RESPONSE_LEASE_V2 create context that follows the syntax specified in section 2.2.14.2.11, and include it in the buffer described by the response CreateContextsLength and CreateContextsOffset fields. This structure MUST have the following values set:
LeaseKey MUST be set to Lease.LeaseKey.
LeaseState MUST be set to Lease.LeaseState.
If Lease.ParentLeaseKey is not empty, ParentLeaseKey MUST be set to Lease.ParentLeaseKey, and the SMB2_LEASE_FLAG_PARENT_LEASE_KEY_SET bit MUST be set in the Flags field of the response.
Epoch SHOULD<353> be set to Lease.Epoch.
If the server supports leasing, Open.Lease is not NULL, and Lease.Version is 1, then the server MUST construct an SMB2_CREATE_RESPONSE_LEASE create context that follows the syntax specified in section 2.2.14.2.10, and include it in the buffer described by the response CreateContextsLength and CreateContextsOffset fields. This structure MUST have the following values set:
LeaseKey MUST be set to Lease.LeaseKey.
LeaseState MUST be set to Lease.LeaseState.
If Open.IsPersistent is TRUE, Open.Lease.Breaking is TRUE, and Open.Lease.BreakNotification is not empty, the server MUST send Open.Lease.BreakNotification to the client over an available connection in ConnectionList where Open.ClientGuid matches Connection.ClientGuid. If the server succeeds in sending the notification, the server MUST set Open.Lease.BreakNotification to empty and MUST start the lease break acknowledgment timer as specified in section 3.3.2.5.
```

## Section 3.3.5.9.13: Handling the SMB2_CREATE_APP_INSTANCE_ID and SMB2_CREATE_APP_INSTANCE_VERSION Create Contexts
**Change type:** Modified

### Old Content
```
This section applies only to servers that implement the SMB 3.x dialect family.
If the create request also includes the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context, the server MUST process the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context as specified in section 3.3.5.9.12, and this section MUST be skipped.
The server MAY validate the StructureSize field of the create context.
The server MUST attempt to locate an Open in GlobalOpenTable where:
AppInstanceId in the request is equal to Open.AppInstanceId.
Target path name is equal to Open.PathName.
Open.TreeConnect.Share is equal to TreeConnect.Share.
Open.Session.Connection.ClientGuid is not equal to the current Connection.ClientGuid.
If an Open is found, Connection.Dialect is "3.1.1", the request includes the SMB2_CREATE_APP_INSTANCE_VERSION context, Open.ApplicationInstanceVersionHigh and Open.ApplicationInstanceVersionLow are not empty, and either of the following is true, then the CREATE operation MUST be failed with STATUS_FILE_FORCED_CLOSED (0xC00000B6):
Open.ApplicationInstanceVersionHigh is greater than the AppInstanceVersionHigh field in the SMB2_CREATE_APP_INSTANCE_VERSION create context.
Open.ApplicationInstanceVersionHigh is equal to the AppInstanceVersionHigh and Open.ApplicationInstanceVersionLow is greater than or equal to the AppInstanceVersionLow fields provided in the SMB2_CREATE_APP_INSTANCE_VERSION create context.
If the server implements SMB dialect 3.1.1, an Open is found, Open.ApplicationInstanceVersionHigh and Open.ApplicationInstanceVersionLow are not empty, and the request does not include the SMB2_CREATE_APP_INSTANCE_VERSION create context, then the CREATE operation MUST be failed with STATUS_FILE_FORCED_CLOSED (0xC00000B6).
If an Open is found, the server MUST calculate the maximal access that the user, identified by Session.SecurityContext, has on the file being opened.<351> If the maximal access includes GENERIC_READ access, the server MUST close the open as specified in 3.3.4.17.
If Open.CreateGuid is NULL, and Open.TreeConnect.Share.IsCA is FALSE, the server SHOULD<352> close the open as specified in section 3.3.4.17.
The server MUST then continue the create process specified in the "Open Execution" Phase.
```

### New Content
```
This section applies only to servers that implement the SMB 3.x dialect family.
If the create request also includes the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context, the server MUST process the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 create context as specified in section 3.3.5.9.12, and this section MUST be skipped.
The server MAY validate the StructureSize field of the create context.
The server MUST attempt to locate an Open in GlobalOpenTable where:
AppInstanceId in the request is equal to Open.AppInstanceId.
Target path name is equal to Open.PathName.
Open.TreeConnect.Share is equal to TreeConnect.Share.
Open.Session.Connection.ClientGuid is not equal to the current Connection.ClientGuid.
If an Open is found, Connection.Dialect is "3.1.1", the request includes the SMB2_CREATE_APP_INSTANCE_VERSION context, Open.ApplicationInstanceVersionHigh and Open.ApplicationInstanceVersionLow are not empty, and either of the following is true, then the CREATE operation MUST be failed with STATUS_FILE_FORCED_CLOSED (0xC00000B6):
Open.ApplicationInstanceVersionHigh is greater than the AppInstanceVersionHigh field in the SMB2_CREATE_APP_INSTANCE_VERSION create context.
Open.ApplicationInstanceVersionHigh is equal to the AppInstanceVersionHigh and Open.ApplicationInstanceVersionLow is greater than or equal to the AppInstanceVersionLow fields provided in the SMB2_CREATE_APP_INSTANCE_VERSION create context.
If the server implements SMB dialect 3.1.1, an Open is found, Open.ApplicationInstanceVersionHigh and Open.ApplicationInstanceVersionLow are not empty, and the request does not include the SMB2_CREATE_APP_INSTANCE_VERSION create context, then the CREATE operation MUST be failed with STATUS_FILE_FORCED_CLOSED (0xC00000B6).
If an Open is found, the server MUST calculate the maximal access that the user, identified by Session.SecurityContext, has on the file being opened.<354> If the maximal access includes GENERIC_READ access, the server MUST close the open as specified in 3.3.4.17.
If Open.CreateGuid is NULL, and Open.TreeConnect.Share.IsCA is FALSE, the server SHOULD<355> close the open as specified in section 3.3.4.17.
The server MUST then continue the create process specified in the "Open Execution" Phase.
```

## Section 3.3.5.10: Receiving an SMB2 CLOSE Request
**Change type:** Modified

### Old Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 CLOSE, message handling proceeds as follows:
The server MAY<353> validate the open before session verification.
The server MUST locate the session, as specified in section 3.3.5.2.9.
Next, the server MUST locate the open being closed by performing a lookup in the Session.OpenTable, using FileId.Volatile of the request as the lookup key. If no open is found, or if Open.DurableFileId is not equal to FileId.Persistent, the server MUST fail the request with STATUS_FILE_CLOSED.
The server MUST locate the tree connection, as specified in section 3.3.5.2.11.
The server MUST locate the Request in Connection.RequestList for which Request.MessageId matches the MessageId value in the SMB2 header and set Request.Open to the Open.
If SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB is set in the Flags field of the request, the server MUST query the creation time, last access time, last write time, change time, allocation size in bytes, end of file in bytes, and file attributes of the file from the underlying object store in an implementation-specific manner<354>.
The server MUST close the Open as specified in section 3.3.4.17.
The server then MUST construct the response following the syntax specified in section 2.2.16. The values MUST be set as follows:
If the attributes of the file were requested and can be fetched, the server MUST set the Flags field to SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB. Otherwise Flags MUST be set to 0.
If SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB was set:
CreationTime, LastAccessTime, LastWriteTime, ChangeTime, AllocationSize, EndofFile, and FileAttributes MUST be set to the values returned from the attribute query.
If SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB was not set:
CreationTime, LastAccessTime, LastWriteTime, ChangeTime, AllocationSize, EndofFile, and FileAttributes MUST all be set to 0.
The response MUST then be sent to the client.
The Server MUST send an SMB2 CHANGE_NOTIFY Response with STATUS_NOTIFY_CLEANUP status code for all pending CHANGE_NOTIFY requests associated with the FileId that is closed.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_INSUFFICIENT_RESOURCES
STATUS_FILE_CLOSED
STATUS_NETWORK_NAME_DELETED
STATUS_USER_SESSION_DELETED
STATUS_INVALID_PARAMETER
STATUS_NETWORK_SESSION_EXPIRED
STATUS_ACCESS_DENIED
```

### New Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 CLOSE, message handling proceeds as follows:
The server MAY<356> validate the open before session verification.
The server MUST locate the session, as specified in section 3.3.5.2.9.
Next, the server MUST locate the open being closed by performing a lookup in the Session.OpenTable, using FileId.Volatile of the request as the lookup key. If no open is found, or if Open.DurableFileId is not equal to FileId.Persistent, the server MUST fail the request with STATUS_FILE_CLOSED.
The server MUST locate the tree connection, as specified in section 3.3.5.2.11.
The server MUST locate the Request in Connection.RequestList for which Request.MessageId matches the MessageId value in the SMB2 header and set Request.Open to the Open.
If SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB is set in the Flags field of the request, the server MUST query the creation time, last access time, last write time, change time, allocation size in bytes, end of file in bytes, and file attributes of the file from the underlying object store in an implementation-specific manner<357>.
The server MUST close the Open as specified in section 3.3.4.17.
The server then MUST construct the response following the syntax specified in section 2.2.16. The values MUST be set as follows:
If the attributes of the file were requested and can be fetched, the server MUST set the Flags field to SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB. Otherwise Flags MUST be set to 0.
If SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB was set:
CreationTime, LastAccessTime, LastWriteTime, ChangeTime, AllocationSize, EndofFile, and FileAttributes MUST be set to the values returned from the attribute query.
If SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB was not set:
CreationTime, LastAccessTime, LastWriteTime, ChangeTime, AllocationSize, EndofFile, and FileAttributes MUST all be set to 0.
The response MUST then be sent to the client.
The Server MUST send an SMB2 CHANGE_NOTIFY Response with STATUS_NOTIFY_CLEANUP status code for all pending CHANGE_NOTIFY requests associated with the FileId that is closed.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_INSUFFICIENT_RESOURCES
STATUS_FILE_CLOSED
STATUS_NETWORK_NAME_DELETED
STATUS_USER_SESSION_DELETED
STATUS_INVALID_PARAMETER
STATUS_NETWORK_SESSION_EXPIRED
STATUS_ACCESS_DENIED
```

## Section 3.3.5.11: Receiving an SMB2 FLUSH Request
**Change type:** Modified

### Old Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 FLUSH, message handling proceeds as follows:
The server MUST locate the session, as specified in section 3.3.5.2.9.
The server MUST locate the tree connection, as specified in section 3.3.5.2.11.
Next the server MUST locate the open being flushed by performing a lookup in the Session.OpenTable, using the FileId.Volatile of the request as the lookup key. If no open is found, or if Open.DurableFileId is not equal to FileId.Persistent, the server MUST fail the request with STATUS_FILE_CLOSED. Otherwise, the server MUST locate the Request in Connection.RequestList for which Request.MessageId matches the MessageId value in the SMB2 header, and set Request.Open to the Open.
If Open.IsPersistent is FALSE and Open.IsReplayEligible is TRUE, the server MUST set Open.IsReplayEligible to FALSE.
If the Open is on a file and Open.GrantedAccess includes neither FILE_WRITE_DATA nor FILE_APPEND_DATA, the server MUST fail the request with STATUS_ACCESS_DENIED.
If the Open is on a directory and Open.GrantedAccess includes neither FILE_ADD_FILE nor FILE_ADD_SUBDIRECTORY, the server MUST fail the request with STATUS_ACCESS_DENIED.
If Open.IsPersistent is TRUE, the server MUST succeed the operation and MUST respond with an SMB2 FLUSH Response specified in section 2.2.18.
Otherwise, the server MUST issue a request to the underlying object store to flush any cached data for Open.LocalOpen.<355> If this is a file, the object store MUST propagate any cached data to underlying storage. If this is a named pipe, the server MUST wait for all data written to the pipe to be consumed by a reader. This operation MUST block until the flush is complete. (The server SHOULD<356> choose to handle this request asynchronously, as specified in section 3.3.4.2.)
If the operation succeeds, the server MUST initialize a response following the syntax specified in section 2.2.18.
If the operation fails, the server MUST return the error code to the client.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_INSUFFICIENT_RESOURCES
STATUS_ACCESS_DENIED
STATUS_FILE_CLOSED
STATUS_NETWORK_NAME_DELETED
STATUS_USER_SESSION_DELETED
STATUS_NETWORK_SESSION_EXPIRED
STATUS_INVALID_PARAMETER
STATUS_PIPE_BROKEN
STATUS_DISK_FULL
STATUS_CANCELLED
```

### New Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 FLUSH, message handling proceeds as follows:
The server MUST locate the session, as specified in section 3.3.5.2.9.
The server MUST locate the tree connection, as specified in section 3.3.5.2.11.
Next the server MUST locate the open being flushed by performing a lookup in the Session.OpenTable, using the FileId.Volatile of the request as the lookup key. If no open is found, or if Open.DurableFileId is not equal to FileId.Persistent, the server MUST fail the request with STATUS_FILE_CLOSED. Otherwise, the server MUST locate the Request in Connection.RequestList for which Request.MessageId matches the MessageId value in the SMB2 header, and set Request.Open to the Open.
If the server implements the SMB 3.x dialect family and Open.IsReplayEligible is TRUE, the server MUST set Open.IsReplayEligible to FALSE.
If the Open is on a file and Open.GrantedAccess includes neither FILE_WRITE_DATA nor FILE_APPEND_DATA, the server MUST fail the request with STATUS_ACCESS_DENIED.
If the Open is on a directory and Open.GrantedAccess includes neither FILE_ADD_FILE nor FILE_ADD_SUBDIRECTORY, the server MUST fail the request with STATUS_ACCESS_DENIED.
If Open.IsPersistent is TRUE, the server MUST succeed the operation and MUST respond with an SMB2 FLUSH Response specified in section 2.2.18.
Otherwise, the server MUST issue a request to the underlying object store to flush any cached data for Open.LocalOpen.<358> If this is a file, the object store MUST propagate any cached data to underlying storage. If this is a named pipe, the server MUST wait for all data written to the pipe to be consumed by a reader. This operation MUST block until the flush is complete. (The server SHOULD<359> choose to handle this request asynchronously, as specified in section 3.3.4.2.)
If the operation succeeds, the server MUST initialize a response following the syntax specified in section 2.2.18.
If the operation fails, the server MUST return the error code to the client.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_INSUFFICIENT_RESOURCES
STATUS_ACCESS_DENIED
STATUS_FILE_CLOSED
STATUS_NETWORK_NAME_DELETED
STATUS_USER_SESSION_DELETED
STATUS_NETWORK_SESSION_EXPIRED
STATUS_INVALID_PARAMETER
STATUS_PIPE_BROKEN
STATUS_DISK_FULL
STATUS_CANCELLED
```

## Section 3.3.5.12: Receiving an SMB2 READ Request
**Change type:** Modified

### Old Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 READ, message handling proceeds as follows:
The server MUST locate the session, as specified in section 3.3.5.2.9.
The server MUST locate the tree connection, as specified in section 3.3.5.2.11.
Next the server MUST locate the open that is being read from, by performing a lookup in the Session.OpenTable, using the FileId.Volatile of the request as the lookup key. If no open is found, or if Open.DurableFileId is not equal to FileId.Persistent, the server MUST fail the request with STATUS_FILE_CLOSED. Otherwise, the server MUST locate the Request in Connection.RequestList for which Request.MessageId matches the MessageId value in the SMB2 header, and set Request.Open to the Open.
If Open.IsPersistent is FALSE and Open.IsReplayEligible is TRUE, the server MUST set Open.IsReplayEligible to FALSE.
If Open.GrantedAccess does not allow for FILE_READ_DATA, the request MUST be failed with STATUS_ACCESS_DENIED.
The server SHOULD<357> fail the request with STATUS_INVALID_PARAMETER if the Length field is greater than Connection.MaxReadSize.
If Connection.SupportsMultiCredit is TRUE the server MUST validate CreditCharge based on Length, as specified in section 3.3.5.2.5. If the validation fails, it MUST fail the read request with STATUS_INVALID_PARAMETER.
If the server implements the SMB 3.0.2 or SMB 3.1.1 dialect, the read is being executed on a named pipe, and the SMB2_READFLAG_READ_UNBUFFERED bit is set in the Flags field, the server MUST fail the request with STATUS_INVALID_PARAMETER.
If Connection.Dialect belongs to the SMB 3.x dialect family and if any of the following conditions are TRUE, the server MUST fail the request with STATUS_INVALID_PARAMETER:
Connection.Dialect is "3.0.2" or "3.1.1" and Channel is not equal to SMB2_CHANNEL_RDMA_V1_INVALIDATE or SMB2_CHANNEL_RDMA_V1 or SMB2_CHANNEL_NONE.
Connection.Dialect is "3.0" and Channel is not equal to SMB2_CHANNEL_RDMA_V1 or SMB2_CHANNEL_NONE.
Channel is equal to SMB2_CHANNEL_RDMA_V1 or SMB2_CHANNEL_RDMA_V1_INVALIDATE and any of the following conditions is TRUE:
The underlying Connection is not RDMA.
Length, ReadChannelInfoOffset, or ReadChannelInfoLength is equal to 0.
The server MUST issue a read to the underlying object store represented by Open.LocalOpen for the length, in bytes, given by Length, at the offset, in bytes, from the beginning of the file, provided in Offset. If the server implements the SMB 3.0.2 or SMB 3.1.1 dialect and if the SMB2_READFLAG_READ_UNBUFFERED bit is set in the Flags field of the request, the server SHOULD<358> indicate to the underlying object store not to buffer the read data.
If the read is being executed on a named pipe, and the pipe is in blocking mode (the default), the operation could block for a long time, so the server MAY<359> choose to handle it asynchronously, as specified in section 3.3.4.2. To query a pipe's blocking mode, use the FilePipeInformation file information class, as specified in [MS-FSCC] section 2.4.37. To change a pipe's blocking mode, use an SMB2 SET_INFO Request with the FilePipeInformation file information class, as specified in [MS-FSCC] section 2.4.37.<360> If the read is not finished in 0.5 milliseconds, the server MUST send an interim response to the client.
If the read fails, the server MUST fail the request using the error code received from the read operation. If the underlying object store returns fewer bytes than specified by the MinimumCount field of the request, the server MUST fail the request with STATUS_END_OF_FILE.
If the read succeeds, the server MUST construct a read response using the syntax specified in section 2.2.20 with the following values.
If the request Channel field contains the value SMB2_CHANNEL_NONE, then:
DataOffset MUST be set to the offset into the response, in bytes, from the beginning of the SMB2 header where the data is located.
If the number of bytes returned from the underlying object store is more than the Length field in the request, DataLength MUST be set to the Length field of the request. Otherwise, DataLength MUST be set to the number of bytes returned from the underlying object store.
The data MUST be copied into the response.
DataRemaining MUST be set to zero.
If IsCompressionSupported is TRUE, Connection.CompressionIds is not empty, underlying Connection is not RDMA, and Flags field in the request includes SMB2_READFLAG_REQUEST_COMPRESSED, Request.CompressReply MUST be set to TRUE.
If the request Channel field contains the value SMB2_CHANNEL_RDMA_V1 or SMB2_CHANNEL_RDMA_V1_INVALIDATE, the server MUST do the following:
If Connection.Dialect is “3.1.1” and Connection.RDMATransformIds is not empty, the server MUST do the following:
Construct SMB2_RDMA_TRANSFORM structure by setting Channel to SMB2_CHANNEL_NONE, TransformCount to 1, and RdmaDescriptorOffset and RdmaDescriptorLength to zero.
If Request.IsEncrypted is TRUE and Connection.RDMATransformIds includes SMB2_RDMA_TRANSFORM_ENCRYPTION, the server MUST construct an SMB2_RDMA_CRYPTO_TRANSFORM structure with the following values:
Set TransformType to SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION.
If Connection.CipherID is AES-128-CCM or AES-256-CCM, Nonce MUST be set to an 11-byte implementation-specific value. If Connection.CipherID is AES-128-GCM or AES-256-GCM, Nonce MUST be set to a 12-byte implementation-specific value.
Set Signature to a value generated using the algorithm specified in Connection.CipherID with the following inputs:
Nonce.
Read data to be signed.
Session.EncryptionKey, as the key for signing.
Otherwise, if SMB2_FLAGS_SIGNED bit is set in the Flags field of the SMB2 header and Connection.RDMATransformIds includes SMB2_RDMA_TRANSFORM_SIGNING, the server MUST construct an SMB2_RDMA_CRYPTO_TRANSFORM structure with the following values:
Set TransformType to SMB2_RDMA_TRANSFORM_TYPE_SIGNING.
Set Signature to a value generated using the algorithm specified in Connection.SigningAlgorithmId, as specified in section 3.1.4.1. If Connection.SigningAlgorithmId is AES-GMAC, Nonce set to 12-byte implementation specific value MUST be used for Signature generation. Otherwise, Nonce MUST be set to empty.
Set SignatureLength to the length of the Signature field.
Set NonceLength to the length of the Nonce field.
Buffer field of the response MUST be set to the constructed SMB2_RDMA_TRANSFORM structure, followed by the constructed SMB2_RDMA_CRYPTO_TRANSFORM structure.
DataLength field of the response MUST be set to the sum of the sizes of SMB2_RDMA_TRANSFORM and SMB2_RDMA_CRYPTO_TRANSFORM structures.
SMB2_READFLAG_RESPONSE_RDMA_TRANSFORM bit in the Flags field of the response MUST be set.
If TransformType in SMB2_RDMA_CRYPTO_TRANSFORM structure is SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION, the read data MUST be encrypted using Session.EncryptionKey with the algorithm specified in Connection.CipherId.
Otherwise, DataLength MUST be set to zero.
The DataOffset field MUST be set to the offset into the response, in bytes, from the beginning of the SMB2 header to the Buffer field.
If the number of bytes returned from the underlying object store is more than the Length field in the request, DataRemaining MUST be set to the Length field of the request. Otherwise, DataRemaining MUST be set to the number of bytes returned from the underlying object store.
The data MUST NOT be copied into the response.
The data MUST be sent via the processing specified in [MS-SMBD] section 3.1.4.5, providing the Connection, the data, and the array of SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures passed in the request at offset ReadChannelInfoOffset and of length ReadChannelInfoLength fields.
The response MUST then be sent to the client. If the request Channel field contains the value SMB2_CHANNEL_RDMA_V1_INVALIDATE, then the Token in the first element of the array of SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures passed in the request MUST additionally be supplied, as specified in [MS-SMBD] section 3.1.4.2.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_INSUFFICIENT_RESOURCES
STATUS_ACCESS_DENIED
STATUS_FILE_CLOSED
STATUS_NETWORK_NAME_DELETED
STATUS_USER_SESSION_DELETED
STATUS_NETWORK_SESSION_EXPIRED
STATUS_INVALID_PARAMETER
STATUS_END_OF_FILE
STATUS_PIPE_BROKEN
STATUS_BUFFER_OVERFLOW
STATUS_CANCELLED
STATUS_FILE_LOCK_CONFLICT
```

### New Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 READ, message handling proceeds as follows:
The server MUST locate the session, as specified in section 3.3.5.2.9.
The server MUST locate the tree connection, as specified in section 3.3.5.2.11.
Next the server MUST locate the open that is being read from, by performing a lookup in the Session.OpenTable, using the FileId.Volatile of the request as the lookup key. If no open is found, or if Open.DurableFileId is not equal to FileId.Persistent, the server MUST fail the request with STATUS_FILE_CLOSED. Otherwise, the server MUST locate the Request in Connection.RequestList for which Request.MessageId matches the MessageId value in the SMB2 header, and set Request.Open to the Open.
If the server implements the SMB 3.x dialect family and Open.IsReplayEligible is TRUE, the server MUST set Open.IsReplayEligible to FALSE.
If Open.GrantedAccess does not allow for FILE_READ_DATA, the request MUST be failed with STATUS_ACCESS_DENIED.
The server SHOULD<360> fail the request with STATUS_INVALID_PARAMETER if the Length field is greater than Connection.MaxReadSize.
If Connection.SupportsMultiCredit is TRUE the server MUST validate CreditCharge based on Length, as specified in section 3.3.5.2.5. If the validation fails, it MUST fail the read request with STATUS_INVALID_PARAMETER.
If the server implements the SMB 3.0.2 or SMB 3.1.1 dialect, the read is being executed on a named pipe, and the SMB2_READFLAG_READ_UNBUFFERED bit is set in the Flags field, the server MUST fail the request with STATUS_INVALID_PARAMETER.
If Connection.Dialect belongs to the SMB 3.x dialect family and if any of the following conditions are TRUE, the server MUST fail the request with STATUS_INVALID_PARAMETER:
Connection.Dialect is "3.0.2" or "3.1.1" and Channel is not equal to SMB2_CHANNEL_RDMA_V1_INVALIDATE or SMB2_CHANNEL_RDMA_V1 or SMB2_CHANNEL_NONE.
Connection.Dialect is "3.0" and Channel is not equal to SMB2_CHANNEL_RDMA_V1 or SMB2_CHANNEL_NONE.
Channel is equal to SMB2_CHANNEL_RDMA_V1 or SMB2_CHANNEL_RDMA_V1_INVALIDATE and any of the following conditions is TRUE:
The underlying Connection is not RDMA.
Length, ReadChannelInfoOffset, or ReadChannelInfoLength is equal to 0.
The server MUST issue a read to the underlying object store represented by Open.LocalOpen for the length, in bytes, given by Length, at the offset, in bytes, from the beginning of the file, provided in Offset. If the server implements the SMB 3.0.2 or SMB 3.1.1 dialect and if the SMB2_READFLAG_READ_UNBUFFERED bit is set in the Flags field of the request, the server SHOULD<361> indicate to the underlying object store not to buffer the read data.
If the read is being executed on a named pipe, and the pipe is in blocking mode (the default), the operation could block for a long time, so the server MAY<362> choose to handle it asynchronously, as specified in section 3.3.4.2. To query a pipe's blocking mode, use the FilePipeInformation file information class, as specified in [MS-FSCC] section 2.4.37. To change a pipe's blocking mode, use an SMB2 SET_INFO Request with the FilePipeInformation file information class, as specified in [MS-FSCC] section 2.4.37.<363> If the read is not finished in 0.5 milliseconds, the server MUST send an interim response to the client.
If the read fails, the server MUST fail the request using the error code received from the read operation. If the underlying object store returns fewer bytes than specified by the MinimumCount field of the request, the server MUST fail the request with STATUS_END_OF_FILE.
If the read succeeds, the server MUST construct a read response using the syntax specified in section 2.2.20 with the following values.
If the request Channel field contains the value SMB2_CHANNEL_NONE, then:
DataOffset MUST be set to the offset into the response, in bytes, from the beginning of the SMB2 header where the data is located.
If the number of bytes returned from the underlying object store is more than the Length field in the request, DataLength MUST be set to the Length field of the request. Otherwise, DataLength MUST be set to the number of bytes returned from the underlying object store.
The data MUST be copied into the response.
DataRemaining MUST be set to zero.
If IsCompressionSupported is TRUE, Connection.CompressionIds is not empty, underlying Connection is not RDMA, and Flags field in the request includes SMB2_READFLAG_REQUEST_COMPRESSED, Request.CompressReply MUST be set to TRUE.
If the request Channel field contains the value SMB2_CHANNEL_RDMA_V1 or SMB2_CHANNEL_RDMA_V1_INVALIDATE, the server MUST do the following:
If Connection.Dialect is “3.1.1” and Connection.RDMATransformIds is not empty, the server MUST do the following:
Construct SMB2_RDMA_TRANSFORM structure by setting Channel to SMB2_CHANNEL_NONE, TransformCount to 1, and RdmaDescriptorOffset and RdmaDescriptorLength to zero.
If Request.IsEncrypted is TRUE and Connection.RDMATransformIds includes SMB2_RDMA_TRANSFORM_ENCRYPTION, the server MUST construct an SMB2_RDMA_CRYPTO_TRANSFORM structure with the following values:
Set TransformType to SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION.
If Connection.CipherID is AES-128-CCM or AES-256-CCM, Nonce MUST be set to an 11-byte implementation-specific value. If Connection.CipherID is AES-128-GCM or AES-256-GCM, Nonce MUST be set to a 12-byte implementation-specific value.
Set Signature to a value generated using the algorithm specified in Connection.CipherID with the following inputs:
Nonce.
Read data to be signed.
Session.EncryptionKey, as the key for signing.
Otherwise, if SMB2_FLAGS_SIGNED bit is set in the Flags field of the SMB2 header and Connection.RDMATransformIds includes SMB2_RDMA_TRANSFORM_SIGNING, the server MUST construct an SMB2_RDMA_CRYPTO_TRANSFORM structure with the following values:
Set TransformType to SMB2_RDMA_TRANSFORM_TYPE_SIGNING.
Set Signature to a value generated using the algorithm specified in Connection.SigningAlgorithmId, as specified in section 3.1.4.1. If Connection.SigningAlgorithmId is AES-GMAC, Nonce set to 12-byte implementation specific value MUST be used for Signature generation. Otherwise, Nonce MUST be set to empty.
Set SignatureLength to the length of the Signature field.
Set NonceLength to the length of the Nonce field.
Buffer field of the response MUST be set to the constructed SMB2_RDMA_TRANSFORM structure, followed by the constructed SMB2_RDMA_CRYPTO_TRANSFORM structure.
DataLength field of the response MUST be set to the sum of the sizes of SMB2_RDMA_TRANSFORM and SMB2_RDMA_CRYPTO_TRANSFORM structures.
SMB2_READFLAG_RESPONSE_RDMA_TRANSFORM bit in the Flags field of the response MUST be set.
If TransformType in SMB2_RDMA_CRYPTO_TRANSFORM structure is SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION, the read data MUST be encrypted using Session.EncryptionKey with the algorithm specified in Connection.CipherId.
Otherwise, DataLength MUST be set to zero.
The DataOffset field MUST be set to the offset into the response, in bytes, from the beginning of the SMB2 header to the Buffer field.
If the number of bytes returned from the underlying object store is more than the Length field in the request, DataRemaining MUST be set to the Length field of the request. Otherwise, DataRemaining MUST be set to the number of bytes returned from the underlying object store.
The data MUST NOT be copied into the response.
The data MUST be sent via the processing specified in [MS-SMBD] section 3.1.4.5, providing the Connection, the data, and the array of SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures passed in the request at offset ReadChannelInfoOffset and of length ReadChannelInfoLength fields.
The response MUST then be sent to the client. If the request Channel field contains the value SMB2_CHANNEL_RDMA_V1_INVALIDATE, then the Token in the first element of the array of SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures passed in the request MUST additionally be supplied, as specified in [MS-SMBD] section 3.1.4.2.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_INSUFFICIENT_RESOURCES
STATUS_ACCESS_DENIED
STATUS_FILE_CLOSED
STATUS_NETWORK_NAME_DELETED
STATUS_USER_SESSION_DELETED
STATUS_NETWORK_SESSION_EXPIRED
STATUS_INVALID_PARAMETER
STATUS_END_OF_FILE
STATUS_PIPE_BROKEN
STATUS_BUFFER_OVERFLOW
STATUS_CANCELLED
STATUS_FILE_LOCK_CONFLICT
```

## Section 3.3.5.13: Receiving an SMB2 WRITE Request
**Change type:** Modified

### Old Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 WRITE, message handling proceeds as follows:
The server MUST locate the session, as specified in section 3.3.5.2.9.
The server MUST locate the tree connection, as specified in section 3.3.5.2.11.
Next the server MUST locate the open being written to by performing a lookup in the Session.OpenTable, using FileId.Volatile of the request as the lookup key. If no open is found, or if Open.DurableFileId is not equal to FileId.Persistent, the server MUST fail the request with STATUS_FILE_CLOSED. Otherwise, the server MUST locate the Request in Connection.RequestList for which Request.MessageId matches the MessageId value in the SMB2 Header, and set Request.Open to the Open.
If Open.IsPersistent is FALSE and Open.IsReplayEligible is TRUE, the server MUST set Open.IsReplayEligible to FALSE.
If the range being written to is within the existing file size and Open.GrantedAccess does not include FILE_WRITE_DATA, or if the range being written to extends the file size and Open.GrantedAccess does not include FILE_APPEND_DATA, the server SHOULD<361> fail the request with STATUS_ACCESS_DENIED.
The server SHOULD<362> fail the request with STATUS_INVALID_PARAMETER if the Length field is greater than Connection.MaxWriteSize.
If Connection.Dialect belongs to the SMB 3.x dialect family and if any of the following conditions are TRUE, the server MUST fail the request with STATUS_INVALID_PARAMETER:
Connection.Dialect is "3.1.1" and one of the following conditions is TRUE:
IsRDMATransformSupported is TRUE and Channel is not equal to SMB2_CHANNEL_RDMA_TRANSFORM, SMB2_CHANNEL_RDMA_V1_INVALIDATE, SMB2_CHANNEL_RDMA_V1, or SMB2_CHANNEL_NONE.
Channel is not equal to SMB2_CHANNEL_RDMA_V1_INVALIDATE, SMB2_CHANNEL_RDMA_V1, or SMB2_CHANNEL_NONE.
Connection.Dialect is "3.0.2" and Channel is not equal to SMB2_CHANNEL_RDMA_V1_INVALIDATE or SMB2_CHANNEL_RDMA_V1 or SMB2_CHANNEL_NONE.
Connection.Dialect is "3.0" and Channel is not equal to SMB2_CHANNEL_RDMA_V1 or SMB2_CHANNEL_NONE.
Channel is equal to SMB2_CHANNEL_RDMA_V1, SMB2_CHANNEL_RDMA_V1_INVALIDATE, or SMB2_CHANNEL_RDMA_TRANSFORM and any of the following conditions is TRUE:
The underlying Connection is not RDMA.
Length or DataOffset are not equal to 0.
RemainingBytes, WriteChannelInfoOffset, or WriteChannelInfoLength are equal to 0.
If Channel is equal to SMB2_CHANNEL_NONE and DataOffset is greater than 0x100, the server MUST fail the request with STATUS_INVALID_PARAMETER.
If Channel is equal to SMB2_CHANNEL_NONE and the number of bytes received in Buffer is less than (DataOffset + Length), the server MUST fail the request with STATUS_INVALID_PARAMETER.
If Connection.SupportsMultiCredit is TRUE, the server MUST validate CreditCharge based on Length, as specified in section 3.3.5.2.5. If the validation fails, it MUST fail the write request with STATUS_INVALID_PARAMETER.
If the server implements the SMB 3.x dialect family, and if a write is being executed on a named pipe and the Flags field is set to SMB2_WRITEFLAG_WRITE_UNBUFFERED or SMB2_WRITEFLAG_WRITE_THROUGH, the server MUST fail the request with STATUS_INVALID_PARAMETER.
The server SHOULD<363> ignore undefined bits in the Flags field.
If the server implements the SMB 3.0.2 or SMB 3.1.1 dialect, Connection.Dialect is not "3.0.2" or "3.1.1", and the SMB2_WRITEFLAG_WRITE_UNBUFFERED bit is set in the Flags field, the server MUST ignore the bit.
If Connection.Dialect belongs to the SMB 3.x dialect family and the Channel field contains the value SMB2_CHANNEL_RDMA_V1, SMB2_CHANNEL_RDMA_V1_INVALIDATE, or SMB2_CHANNEL_RDMA_TRANSFORM, the server MUST do the following:
If Connection.Dialect is "3.1.1" and Channel is equal to SMB2_CHANNEL_RDMA_TRANSFORM, the server MUST return STATUS_INVALID_PARAMETER to the client in the following conditions:
Connection.RDMATransformIds is empty.
WriteChannelInfoLength is less than the size of SMB2_RDMA_TRANSFORM structure.
TransformCount is 0 in SMB2_RDMA_TRANSFORM structure.
Connection.RDMATransformIds does not contain SMB2_RDMA_TRANSFORM_ENCRYPTION, and SMB2_RDMA_CRYPTO_TRANSFORM with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION is present.
Connection.RDMATransformIds does not contain SMB2_RDMA_TRANSFORM_SIGNING, and SMB2_RDMA_CRYPTO_TRANSFORM with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_SIGNING is present.
SMB2_RDMA_CRYPTO_TRANSFORM with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION is present and Request.IsEncrypted is FALSE.
More than one SMB2_RDMA_CRYPTO_TRANSFORM structures with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION or SMB2_RDMA_TRANSFORM_TYPE_SIGNING are present.
More than one SMB2_RDMA_CRYPTO_TRANSFORM structures with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION are present.
Two SMB2_RDMA_CRYPTO_TRANSFORM structures with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION and SMB2_RDMA_TRANSFORM_TYPE_SIGNING are present.
SMB2_RDMA_CRYPTO_TRANSFORM with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_SIGNING is present and SMB2_FLAGS_SIGNED bit is not set in the Flags field of the SMB2 header.
An array of SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures does not begin at the first 8-byte aligned offset after SMB2_RDMA_CRYPTO_TRANSFORM structure from the beginning of the Buffer field.
SMB2_RDMA_TRANSFORM structure is followed by a transform not specified in section 2.2.43.
The server MUST return STATUS_INVALID_PARAMETER to the client in the following conditions:
RemainingBytes field is greater than Connection.MaxWriteSize.
Length field of the first SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structure is zero.
Sum of the values of Length fields in all SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures is less than RemainingBytes.
If Channel is equal to SMB2_CHANNEL_RDMA_TRANSFORM, Connection.RDMATransformIds includes SMB2_RDMA_TRANSFORM_ENCRYPTION, and SMB2_RDMA_CRYPTO_TRANSFORM with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION is present, the data MUST first be obtained via the processing specified in [MS-SMBD] section 3.1.4.6, providing the Connection, a newly allocated buffer to receive the data, and the array of SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures passed in the request at offset RdmaDescriptorOffset and of length RdmaDescriptorLength fields of SMB2_RDMA_TRANSFORM structure.
The server MUST fail the request with STATUS_AUTH_TAG_MISMATCH if one of the following is TRUE:
SignatureLength field is greater than 16.
Connection.CipherId is AES-128-CCM or AES-256-CCM and NonceLength field is not equal to 11.
Connection.CipherId is AES-128-GCM or AES-256-GCM and NonceLength field is not equal to 12.
The data obtained MUST be decrypted using the algorithm specified in Connection.CipherId and Session.DecryptionKey by passing encrypted data and Signature and Nonce, from the received SMB2_RDMA_CRYPTO_TRANSFORM structure. If the size of the decrypted data is not equal to RemainingBytes field in the request, the server MUST fail the request with STATUS_BAD_DATA.
If Channel is equal to SMB2_CHANNEL_RDMA_TRANSFORM, Connection.RDMATransformIds includes SMB2_RDMA_TRANSFORM_SIGNING, and SMB2_RDMA_CRYPTO_TRANSFORM with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_SIGNING is present, the data MUST first be obtained via the processing specified in [MS-SMBD] section 3.1.4.6, providing the Connection, a newly allocated buffer to receive the data, and the array of SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures passed in the request at offset RdmaDescriptorOffset and of length RdmaDescriptorLength fields of SMB2_RDMA_TRANSFORM structure. The server MUST verify the received data as specified in section 3.1.5.1 except that the computed signature is compared with the value in Signature field of SMB2_RDMA_CRYPTO_TRANSFORM. If the signature verification fails, the server MUST fail the request with STATUS_INVALID_SIGNATURE.
Otherwise, the data MUST be first obtained via the processing specified in [MS-SMBD] section 3.1.4.6, providing the Connection, a newly allocated buffer to receive the data, and the array of SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures passed in the request at offset WriteChannelInfoOffset and of length WriteChannelInfoLength fields.
If Connection.Dialect is "3.0.2" or "3.1.1", SMB2_WRITEFLAG_WRITE_THROUGH is set in the Flags field of the request, SMB2_WRITEFLAG_WRITE_UNBUFFERED is not set in the Flags field of the request, and Open.CreateOptions doesn't include the FILE_NO_INTERMEDIATE_BUFFERING bit, the server MUST fail the request with STATUS_INVALID_PARAMETER.
If Connection.Dialect is "2.1" or "3.0", SMB2_WRITEFLAG_WRITE_THROUGH is set in the Flags field of the request, and Open.CreateOptions doesn't include the FILE_NO_INTERMEDIATE_BUFFERING bit, the server MUST fail the request with STATUS_INVALID_PARAMETER.
The server MUST issue a write to the underlying object store represented by Open.LocalOpen for the length, in bytes, given by Length, at the offset, in bytes, from the beginning of the file, provided in Offset. If Connection.Dialect is not "2.0.2", and SMB2_WRITEFLAG_WRITE_THROUGH is set in the Flags field of the SMB2 WRITE Request, the server SHOULD<364> indicate to the underlying object store that the write is to be written to underlying storage before completion is returned. If the server implements the SMB 3.0.2 or SMB 3.1.1 dialect, and if the SMB2_WRITEFLAG_WRITE_UNBUFFERED bit is set in the Flags field of the request, the server SHOULD indicate to the underlying object store that the write data is not to be buffered.
If the write is being executed on a named pipe, and the pipe is in blocking mode (the default), the operation could block for a long time, so the server MAY<365> choose to handle it asynchronously, as specified in section 3.3.4.2. To query a pipe's blocking mode, use the FilePipeInformation file information class, as specified in [MS-FSCC] section 2.4.37. To change a pipe's blocking mode, use an SMB2 SET_INFO Request with the FilePipeInformation file information class, as specified in [MS-FSCC] section 2.4.37.
If the write fails, the server MUST fail the request with the error code received from the write.
If the write succeeds, the server MUST construct a write response following the syntax specified in section 2.2.22 with the following values:
Count MUST be set to the number of bytes written.
Remaining MUST be set to zero.
WriteChannelInfoOffset MUST be set to zero.
WriteChannelInfoLength MUST be set to zero.
The response MUST then be sent to the client.
The Token in the first element of the array of SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures passed in the request MUST additionally be supplied, as specified in [MS-SMBD] section 3.1.4.2, if any of the following conditions is TRUE:
Channel field in the request is equal to SMB2_CHANNEL_RDMA_TRANSFORM, and the Channel field in SMB2_RDMA_TRANSFORM structure is equal to SMB2_CHANNEL_RDMA_V1_INVALIDATE.
Channel field in the request is equal to SMB2_CHANNEL_RDMA_V1_INVALIDATE.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_INSUFFICIENT_RESOURCES
STATUS_ACCESS_DENIED
STATUS_FILE_CLOSED
STATUS_NETWORK_NAME_DELETED
STATUS_USER_SESSION_DELETED
STATUS_NETWORK_SESSION_EXPIRED
STATUS_INVALID_PARAMETER
STATUS_PIPE_BROKEN
STATUS_DISK_FULL
STATUS_CANCELLED
STATUS_FILE_LOCK_CONFLICT
```

### New Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 WRITE, message handling proceeds as follows:
The server MUST locate the session, as specified in section 3.3.5.2.9.
The server MUST locate the tree connection, as specified in section 3.3.5.2.11.
Next the server MUST locate the open being written to by performing a lookup in the Session.OpenTable, using FileId.Volatile of the request as the lookup key. If no open is found, or if Open.DurableFileId is not equal to FileId.Persistent, the server MUST fail the request with STATUS_FILE_CLOSED. Otherwise, the server MUST locate the Request in Connection.RequestList for which Request.MessageId matches the MessageId value in the SMB2 Header, and set Request.Open to the Open.
If the server implements the SMB 3.x dialect family and Open.IsReplayEligible is TRUE, the server MUST set Open.IsReplayEligible to FALSE.
If the range being written to is within the existing file size and Open.GrantedAccess does not include FILE_WRITE_DATA, or if the range being written to extends the file size and Open.GrantedAccess does not include FILE_APPEND_DATA, the server SHOULD<364> fail the request with STATUS_ACCESS_DENIED.
The server SHOULD<365> fail the request with STATUS_INVALID_PARAMETER if the Length field is greater than Connection.MaxWriteSize.
If Connection.Dialect belongs to the SMB 3.x dialect family and if any of the following conditions are TRUE, the server MUST fail the request with STATUS_INVALID_PARAMETER:
Connection.Dialect is "3.1.1" and one of the following conditions is TRUE:
IsRDMATransformSupported is TRUE and Channel is not equal to SMB2_CHANNEL_RDMA_TRANSFORM, SMB2_CHANNEL_RDMA_V1_INVALIDATE, SMB2_CHANNEL_RDMA_V1, or SMB2_CHANNEL_NONE.
Channel is not equal to SMB2_CHANNEL_RDMA_V1_INVALIDATE, SMB2_CHANNEL_RDMA_V1, or SMB2_CHANNEL_NONE.
Connection.Dialect is "3.0.2" and Channel is not equal to SMB2_CHANNEL_RDMA_V1_INVALIDATE or SMB2_CHANNEL_RDMA_V1 or SMB2_CHANNEL_NONE.
Connection.Dialect is "3.0" and Channel is not equal to SMB2_CHANNEL_RDMA_V1 or SMB2_CHANNEL_NONE.
Channel is equal to SMB2_CHANNEL_RDMA_V1, SMB2_CHANNEL_RDMA_V1_INVALIDATE, or SMB2_CHANNEL_RDMA_TRANSFORM and any of the following conditions is TRUE:
The underlying Connection is not RDMA.
Length or DataOffset are not equal to 0.
RemainingBytes, WriteChannelInfoOffset, or WriteChannelInfoLength are equal to 0.
If Channel is equal to SMB2_CHANNEL_NONE and DataOffset is greater than 0x100, the server MUST fail the request with STATUS_INVALID_PARAMETER.
If Channel is equal to SMB2_CHANNEL_NONE and the number of bytes received in Buffer is less than (DataOffset + Length), the server MUST fail the request with STATUS_INVALID_PARAMETER.
If Connection.SupportsMultiCredit is TRUE, the server MUST validate CreditCharge based on Length, as specified in section 3.3.5.2.5. If the validation fails, it MUST fail the write request with STATUS_INVALID_PARAMETER.
If the server implements the SMB 3.x dialect family, and if a write is being executed on a named pipe and the Flags field is set to SMB2_WRITEFLAG_WRITE_UNBUFFERED or SMB2_WRITEFLAG_WRITE_THROUGH, the server MUST fail the request with STATUS_INVALID_PARAMETER.
The server SHOULD<366> ignore undefined bits in the Flags field.
If the server implements the SMB 3.0.2 or SMB 3.1.1 dialect, Connection.Dialect is not "3.0.2" or "3.1.1", and the SMB2_WRITEFLAG_WRITE_UNBUFFERED bit is set in the Flags field, the server MUST ignore the bit.
If Connection.Dialect belongs to the SMB 3.x dialect family and the Channel field contains the value SMB2_CHANNEL_RDMA_V1, SMB2_CHANNEL_RDMA_V1_INVALIDATE, or SMB2_CHANNEL_RDMA_TRANSFORM, the server MUST do the following:
If Connection.Dialect is "3.1.1" and Channel is equal to SMB2_CHANNEL_RDMA_TRANSFORM, the server MUST return STATUS_INVALID_PARAMETER to the client in the following conditions:
Connection.RDMATransformIds is empty.
WriteChannelInfoLength is less than the size of SMB2_RDMA_TRANSFORM structure.
TransformCount is 0 in SMB2_RDMA_TRANSFORM structure.
Connection.RDMATransformIds does not contain SMB2_RDMA_TRANSFORM_ENCRYPTION, and SMB2_RDMA_CRYPTO_TRANSFORM with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION is present.
Connection.RDMATransformIds does not contain SMB2_RDMA_TRANSFORM_SIGNING, and SMB2_RDMA_CRYPTO_TRANSFORM with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_SIGNING is present.
SMB2_RDMA_CRYPTO_TRANSFORM with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION is present and Request.IsEncrypted is FALSE.
More than one SMB2_RDMA_CRYPTO_TRANSFORM structures with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION or SMB2_RDMA_TRANSFORM_TYPE_SIGNING are present.
More than one SMB2_RDMA_CRYPTO_TRANSFORM structures with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION are present.
Two SMB2_RDMA_CRYPTO_TRANSFORM structures with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION and SMB2_RDMA_TRANSFORM_TYPE_SIGNING are present.
SMB2_RDMA_CRYPTO_TRANSFORM with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_SIGNING is present and SMB2_FLAGS_SIGNED bit is not set in the Flags field of the SMB2 header.
An array of SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures does not begin at the first 8-byte aligned offset after SMB2_RDMA_CRYPTO_TRANSFORM structure from the beginning of the Buffer field.
SMB2_RDMA_TRANSFORM structure is followed by a transform not specified in section 2.2.43.
The server MUST return STATUS_INVALID_PARAMETER to the client in the following conditions:
RemainingBytes field is greater than Connection.MaxWriteSize.
Length field of the first SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structure is zero.
Sum of the values of Length fields in all SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures is less than RemainingBytes.
If Channel is equal to SMB2_CHANNEL_RDMA_TRANSFORM, Connection.RDMATransformIds includes SMB2_RDMA_TRANSFORM_ENCRYPTION, and SMB2_RDMA_CRYPTO_TRANSFORM with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_ENCRYPTION is present, the data MUST first be obtained via the processing specified in [MS-SMBD] section 3.1.4.6, providing the Connection, a newly allocated buffer to receive the data, and the array of SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures passed in the request at offset RdmaDescriptorOffset and of length RdmaDescriptorLength fields of SMB2_RDMA_TRANSFORM structure.
The server MUST fail the request with STATUS_AUTH_TAG_MISMATCH if one of the following is TRUE:
SignatureLength field is greater than 16.
Connection.CipherId is AES-128-CCM or AES-256-CCM and NonceLength field is not equal to 11.
Connection.CipherId is AES-128-GCM or AES-256-GCM and NonceLength field is not equal to 12.
The data obtained MUST be decrypted using the algorithm specified in Connection.CipherId and Session.DecryptionKey by passing encrypted data and Signature and Nonce, from the received SMB2_RDMA_CRYPTO_TRANSFORM structure. If the size of the decrypted data is not equal to RemainingBytes field in the request, the server MUST fail the request with STATUS_BAD_DATA.
If Channel is equal to SMB2_CHANNEL_RDMA_TRANSFORM, Connection.RDMATransformIds includes SMB2_RDMA_TRANSFORM_SIGNING, and SMB2_RDMA_CRYPTO_TRANSFORM with TransformType equal to SMB2_RDMA_TRANSFORM_TYPE_SIGNING is present, the data MUST first be obtained via the processing specified in [MS-SMBD] section 3.1.4.6, providing the Connection, a newly allocated buffer to receive the data, and the array of SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures passed in the request at offset RdmaDescriptorOffset and of length RdmaDescriptorLength fields of SMB2_RDMA_TRANSFORM structure. The server MUST verify the received data as specified in section 3.1.5.1 except that the computed signature is compared with the value in Signature field of SMB2_RDMA_CRYPTO_TRANSFORM. If the signature verification fails, the server MUST fail the request with STATUS_INVALID_SIGNATURE.
Otherwise, the data MUST be first obtained via the processing specified in [MS-SMBD] section 3.1.4.6, providing the Connection, a newly allocated buffer to receive the data, and the array of SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures passed in the request at offset WriteChannelInfoOffset and of length WriteChannelInfoLength fields.
If Connection.Dialect is "3.0.2" or "3.1.1", SMB2_WRITEFLAG_WRITE_THROUGH is set in the Flags field of the request, SMB2_WRITEFLAG_WRITE_UNBUFFERED is not set in the Flags field of the request, and Open.CreateOptions doesn't include the FILE_NO_INTERMEDIATE_BUFFERING bit, the server MUST fail the request with STATUS_INVALID_PARAMETER.
If Connection.Dialect is "2.1" or "3.0", SMB2_WRITEFLAG_WRITE_THROUGH is set in the Flags field of the request, and Open.CreateOptions doesn't include the FILE_NO_INTERMEDIATE_BUFFERING bit, the server MUST fail the request with STATUS_INVALID_PARAMETER.
The server MUST issue a write to the underlying object store represented by Open.LocalOpen for the length, in bytes, given by Length, at the offset, in bytes, from the beginning of the file, provided in Offset. If Connection.Dialect is not "2.0.2", and SMB2_WRITEFLAG_WRITE_THROUGH is set in the Flags field of the SMB2 WRITE Request, the server SHOULD<367> indicate to the underlying object store that the write is to be written to underlying storage before completion is returned. If the server implements the SMB 3.0.2 or SMB 3.1.1 dialect, and if the SMB2_WRITEFLAG_WRITE_UNBUFFERED bit is set in the Flags field of the request, the server SHOULD indicate to the underlying object store that the write data is not to be buffered.
If the write is being executed on a named pipe, and the pipe is in blocking mode (the default), the operation could block for a long time, so the server MAY<368> choose to handle it asynchronously, as specified in section 3.3.4.2. To query a pipe's blocking mode, use the FilePipeInformation file information class, as specified in [MS-FSCC] section 2.4.37. To change a pipe's blocking mode, use an SMB2 SET_INFO Request with the FilePipeInformation file information class, as specified in [MS-FSCC] section 2.4.37.
If the write fails, the server MUST fail the request with the error code received from the write.
If the write succeeds, the server MUST construct a write response following the syntax specified in section 2.2.22 with the following values:
Count MUST be set to the number of bytes written.
Remaining MUST be set to zero.
WriteChannelInfoOffset MUST be set to zero.
WriteChannelInfoLength MUST be set to zero.
The response MUST then be sent to the client.
The Token in the first element of the array of SMB_DIRECT_BUFFER_DESCRIPTOR_V1 structures passed in the request MUST additionally be supplied, as specified in [MS-SMBD] section 3.1.4.2, if any of the following conditions is TRUE:
Channel field in the request is equal to SMB2_CHANNEL_RDMA_TRANSFORM, and the Channel field in SMB2_RDMA_TRANSFORM structure is equal to SMB2_CHANNEL_RDMA_V1_INVALIDATE.
Channel field in the request is equal to SMB2_CHANNEL_RDMA_V1_INVALIDATE.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_INSUFFICIENT_RESOURCES
STATUS_ACCESS_DENIED
STATUS_FILE_CLOSED
STATUS_NETWORK_NAME_DELETED
STATUS_USER_SESSION_DELETED
STATUS_NETWORK_SESSION_EXPIRED
STATUS_INVALID_PARAMETER
STATUS_PIPE_BROKEN
STATUS_DISK_FULL
STATUS_CANCELLED
STATUS_FILE_LOCK_CONFLICT
```

## Section 3.3.5.14.1: Processing Unlocks
**Change type:** Modified

### Old Content
```
For each SMB2_LOCK_ELEMENT entry in the Locks array, if either SMB2_LOCKFLAG_SHARED_LOCK or SMB2_LOCKFLAG_EXCLUSIVE_LOCK is set, the server MUST fail the request with STATUS_INVALID_PARAMETER and stop processing further entries in the Locks array, and all successfully processed unlock operations will not be rolled back.
If SMB2_LOCKFLAG_FAIL_IMMEDIATELY is set, the server MAY<368> ignore this flag.
The server MUST issue the byte-range unlock request to the underlying object store using Open.LocalOpen, and passing the Offset and Length (in bytes) from the SMB2_LOCK_ELEMENT entry.<369> If the unlock operation fails, the server MUST fail the operation with the error code received from the object store and stop processing further entries in the Locks array.
Otherwise, the server MUST decrease Open.LockCount by 1. If there are remaining entries in the Locks array, the server MUST continue processing the next entry in the Locks array as specified above.
After all entries are successfully unlocked, if Connection.Dialect is not "2.0.2" and if Open.IsResilient or Open.IsDurable, or Open.IsPersistent is TRUE or Connection.ServerCapabilities includes the SMB2_GLOBAL_CAP_MULTI_CHANNEL bit, the server MUST set Valid to TRUE and set SequenceNumber to LockSequenceNumber in the entry specified by Open.LockSequenceArray[LockSequenceIndex] to indicate that the unlock request has been successfully processed by the server.
The server MUST construct an SMB2 LOCK Response following the syntax specified in section 2.2.27, and the SMB2 LOCK Response MUST be sent to the client.
```

### New Content
```
For each SMB2_LOCK_ELEMENT entry in the Locks array, if either SMB2_LOCKFLAG_SHARED_LOCK or SMB2_LOCKFLAG_EXCLUSIVE_LOCK is set, the server MUST fail the request with STATUS_INVALID_PARAMETER and stop processing further entries in the Locks array, and all successfully processed unlock operations will not be rolled back.
If SMB2_LOCKFLAG_FAIL_IMMEDIATELY is set, the server MAY<371> ignore this flag.
The server MUST issue the byte-range unlock request to the underlying object store using Open.LocalOpen, and passing the Offset and Length (in bytes) from the SMB2_LOCK_ELEMENT entry.<372> If the unlock operation fails, the server MUST fail the operation with the error code received from the object store and stop processing further entries in the Locks array.
Otherwise, the server MUST decrease Open.LockCount by 1. If there are remaining entries in the Locks array, the server MUST continue processing the next entry in the Locks array as specified above.
After all entries are successfully unlocked, if Connection.Dialect is not "2.0.2" and if Open.IsResilient or Open.IsDurable, or Open.IsPersistent is TRUE or Connection.ServerCapabilities includes the SMB2_GLOBAL_CAP_MULTI_CHANNEL bit, the server MUST set Valid to TRUE and set SequenceNumber to LockSequenceNumber in the entry specified by Open.LockSequenceArray[LockSequenceIndex] to indicate that the unlock request has been successfully processed by the server.
The server MUST construct an SMB2 LOCK Response following the syntax specified in section 2.2.27, and the SMB2 LOCK Response MUST be sent to the client.
```

## Section 3.3.5.14.2: Processing Locks
**Change type:** Modified

### Old Content
```
If the Locks array has more than one entry and the Flags field in any of these entries does not have SMB2_LOCKFLAG_FAIL_IMMEDIATELY set, the server SHOULD<370> fail the request with STATUS_INVALID_PARAMETER. For each SMB2_LOCK_ELEMENT entry in the Locks array, if SMB2_LOCKFLAG_UNLOCK is set, the server MUST fail the request with STATUS_INVALID_PARAMETER and stop processing further entries in the Locks array. All successfully processed Lock operations are not rolled back. For combinations of Lock Flags other than those that are defined in the Flags field of section 2.2.26.1, the server SHOULD fail the request with STATUS_INVALID_PARAMETER.
The server MUST issue a byte-range lock request to the underlying object store using Open.LocalOpen and passing the Offset and Length (in bytes) from the SMB2_LOCK_ELEMENT entry.<371> If SMB2_LOCKFLAG_SHARED_LOCK is set, the lock MUST be acquired in a manner that allows read operations and other shared lock operations from other opens, but disallows writes to the region specified by the lock. If SMB2_LOCKFLAG_EXCLUSIVE_LOCK is set, the lock MUST be acquired in a manner that does not allow read, write, or lock operations from other opens for the range specified.<372>
If the range being locked is already locked by another open in a way that does not allow this open to take a lock on the range, and if SMB2_LOCKFLAG_FAIL_IMMEDIATELY is set, the server MUST fail the request with STATUS_LOCK_NOT_GRANTED and MUST unlock any ranges locked as part of processing the previous entries in the Locks array of this request. It MUST decrement Open.LockCount by the number of locks unlocked. It MUST stop processing any remaining entries in the Locks array and MUST fail the operation with the error code received from the lock operation.
Otherwise, the server MUST increase Open.LockCount by 1. If there are remaining entries in the Locks array, the server MUST continue processing the next entry in the Locks array as described previously.
If Connection.Dialect is not "2.0.2" and if Open.IsResilient or Open.IsDurable, or Open.IsPersistent is TRUE or Connection.ServerCapabilities includes SMB2_GLOBAL_CAP_MULTI_CHANNEL bit, the server MUST set Valid to TRUE and set SequenceNumber to LockSequenceNumber in the entry specified by Open.LockSequenceArray[LockSequenceIndex] to indicate that the lock request has been successfully processed by the server.
The server MUST construct an SMB2 LOCK Response following the syntax specified in section 2.2.27, and the SMB2 LOCK Response MUST be sent to the client.
```

### New Content
```
If the Locks array has more than one entry and the Flags field in any of these entries does not have SMB2_LOCKFLAG_FAIL_IMMEDIATELY set, the server SHOULD<373> fail the request with STATUS_INVALID_PARAMETER. For each SMB2_LOCK_ELEMENT entry in the Locks array, if SMB2_LOCKFLAG_UNLOCK is set, the server MUST fail the request with STATUS_INVALID_PARAMETER and stop processing further entries in the Locks array. All successfully processed Lock operations are not rolled back. For combinations of Lock Flags other than those that are defined in the Flags field of section 2.2.26.1, the server SHOULD fail the request with STATUS_INVALID_PARAMETER.
The server MUST issue a byte-range lock request to the underlying object store using Open.LocalOpen and passing the Offset and Length (in bytes) from the SMB2_LOCK_ELEMENT entry.<374> If SMB2_LOCKFLAG_SHARED_LOCK is set, the lock MUST be acquired in a manner that allows read operations and other shared lock operations from other opens, but disallows writes to the region specified by the lock. If SMB2_LOCKFLAG_EXCLUSIVE_LOCK is set, the lock MUST be acquired in a manner that does not allow read, write, or lock operations from other opens for the range specified.<375>
If the range being locked is already locked by another open in a way that does not allow this open to take a lock on the range, and if SMB2_LOCKFLAG_FAIL_IMMEDIATELY is set, the server MUST fail the request with STATUS_LOCK_NOT_GRANTED and MUST unlock any ranges locked as part of processing the previous entries in the Locks array of this request. It MUST decrement Open.LockCount by the number of locks unlocked. It MUST stop processing any remaining entries in the Locks array and MUST fail the operation with the error code received from the lock operation.
Otherwise, the server MUST increase Open.LockCount by 1. If there are remaining entries in the Locks array, the server MUST continue processing the next entry in the Locks array as described previously.
If Connection.Dialect is not "2.0.2" and if Open.IsResilient or Open.IsDurable, or Open.IsPersistent is TRUE or Connection.ServerCapabilities includes SMB2_GLOBAL_CAP_MULTI_CHANNEL bit, the server MUST set Valid to TRUE and set SequenceNumber to LockSequenceNumber in the entry specified by Open.LockSequenceArray[LockSequenceIndex] to indicate that the lock request has been successfully processed by the server.
The server MUST construct an SMB2 LOCK Response following the syntax specified in section 2.2.27, and the SMB2 LOCK Response MUST be sent to the client.
```

## Section 3.3.5.15.1: Handling an Enumeration of Previous Versions Request
**Change type:** Modified

### Old Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 IOCTL and a CtlCode of FSCTL_SRV_ENUMERATE_SNAPSHOTS, message handling proceeds as follows:
If the MaxOutputResponse of the request is less than 16 bytes, the server MUST fail the request with STATUS_INVALID_PARAMETER.
The server SHOULD<379> refresh the snapshot list by querying the timestamps of available previous versions of the share. The server MUST construct Share.SnapshotList so that the list contains only the snapshots that are active.
The server MUST calculate the size required to return the SRV_SNAPSHOT_ARRAY structure containing the previous version array based on the number of previous versions of the file available in the listed snapshots in Share.SnapshotList as constructed in the previous paragraph.
If there are no previous versions of the file available or if the size required in bytes is greater than the MaxOutputResponse received in the SMB2 IOCTL request, the server MUST construct an SRV_SNAPSHOT_ARRAY structure following the syntax specified in section 2.2.32.2, with the following values:
NumberOfSnapShots MUST be set to the number of previous versions of the file available in the listed snapshots in Share.SnapshotList.
NumberOfSnapShotsReturned MUST be set to 0.
SnapShotArraySize SHOULD<380> be set to the size, in bytes, required to receive all of the previous version timestamps of the file listed in Share.SnapshotList.
Otherwise, the server MUST construct an SRV_SNAPSHOT_ARRAY structure following the syntax specified in section 2.2.32.2, with the following values:
NumberOfSnapShots MUST be set to the number of previous versions of the file available in the listed snapshots in Share.SnapshotList.
NumberOfSnapShotsReturned MUST be set to the number of previous version timestamps being returned in the SnapShots array.
SnapShotArraySize MUST be set to the size, in bytes, of the SnapShots array.
The SnapShots array MUST list the time stamps in textual GMT format for all of the previous version timestamps listed in Share.SnapshotList, as specified in section 2.2.32.2.
The server MUST then construct an SMB2 IOCTL response following the syntax specified in section 2.2.32, with the following values:
CtlCode MUST be set to FSCTL_SRV_ENUMERATE_SNAPSHOTS.
FileId.Persistent MUST be set to Open.DurableFileId. FileId.Volatile MUST be set to Open.FileId.
InputOffset SHOULD be set to the offset, in bytes, from the beginning of the SMB2 header to the Buffer[] field of the response.
InputCount SHOULD be set to zero.
OutputOffset MUST be set to InputOffset + InputCount, rounded up to a multiple of 8.
OutputCount MUST be set to the size of the SRV_SNAPSHOT_ARRAY that is constructed, as specified above.
Flags MUST be set to zero.
The server MUST copy the constructed SRV_SNAPSHOT_ARRAY into the Buffer field at the OutputOffset computed above.
The response MUST be sent to the client.
```

### New Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 IOCTL and a CtlCode of FSCTL_SRV_ENUMERATE_SNAPSHOTS, message handling proceeds as follows:
If the MaxOutputResponse of the request is less than 16 bytes, the server MUST fail the request with STATUS_INVALID_PARAMETER.
The server SHOULD<382> refresh the snapshot list by querying the timestamps of available previous versions of the share. The server MUST construct Share.SnapshotList so that the list contains only the snapshots that are active.
The server MUST calculate the size required to return the SRV_SNAPSHOT_ARRAY structure containing the previous version array based on the number of previous versions of the file available in the listed snapshots in Share.SnapshotList as constructed in the previous paragraph.
If there are no previous versions of the file available or if the size required in bytes is greater than the MaxOutputResponse received in the SMB2 IOCTL request, the server MUST construct an SRV_SNAPSHOT_ARRAY structure following the syntax specified in section 2.2.32.2, with the following values:
NumberOfSnapShots MUST be set to the number of previous versions of the file available in the listed snapshots in Share.SnapshotList.
NumberOfSnapShotsReturned MUST be set to 0.
SnapShotArraySize SHOULD<383> be set to the size, in bytes, required to receive all of the previous version timestamps of the file listed in Share.SnapshotList.
Otherwise, the server MUST construct an SRV_SNAPSHOT_ARRAY structure following the syntax specified in section 2.2.32.2, with the following values:
NumberOfSnapShots MUST be set to the number of previous versions of the file available in the listed snapshots in Share.SnapshotList.
NumberOfSnapShotsReturned MUST be set to the number of previous version timestamps being returned in the SnapShots array.
SnapShotArraySize MUST be set to the size, in bytes, of the SnapShots array.
The SnapShots array MUST list the time stamps in textual GMT format for all of the previous version timestamps listed in Share.SnapshotList, as specified in section 2.2.32.2.
The server MUST then construct an SMB2 IOCTL response following the syntax specified in section 2.2.32, with the following values:
CtlCode MUST be set to FSCTL_SRV_ENUMERATE_SNAPSHOTS.
FileId.Persistent MUST be set to Open.DurableFileId. FileId.Volatile MUST be set to Open.FileId.
InputOffset SHOULD be set to the offset, in bytes, from the beginning of the SMB2 header to the Buffer[] field of the response.
InputCount SHOULD be set to zero.
OutputOffset MUST be set to InputOffset + InputCount, rounded up to a multiple of 8.
OutputCount MUST be set to the size of the SRV_SNAPSHOT_ARRAY that is constructed, as specified above.
Flags MUST be set to zero.
The server MUST copy the constructed SRV_SNAPSHOT_ARRAY into the Buffer field at the OutputOffset computed above.
The response MUST be sent to the client.
```

## Section 3.3.5.15.2: Handling a DFS Referral Information Request
**Change type:** Modified

### Old Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 IOCTL, and a CtlCode of FSCTL_DFS_GET_REFERRALS or FSCTL_DFS_GET_REFERRALS_EX, message handling proceeds as follows:
If IsDfsCapable is set to FALSE, the server MUST return STATUS_FS_DRIVER_REQUIRED to the client.
The server MUST invoke the event as specified in [MS-DFSC] section 3.2.4.2 and pass the following:
The IP address of the client.
The buffer containing the DFS referral request packet.
IsExtendedReferral: Set to TRUE when CtlCode is FSCTL_DFS_GET_REFERRALS_EX.
The maximum size of the response data buffer that will be accepted by the client, as indicated by MaxOutputResponse field in the request.
If DFS returns a failure, the server MUST fail the request with the error code received from DFS. If the error returned from DFS is STATUS_BUFFER_OVERFLOW, the server SHOULD<381> copy the data returned by DFS into a normal FSCTL_GET_DFS_REFERRALS response and return STATUS_BUFFER_OVERFLOW to the client as noted in sections 3.3.4.4 and 3.3.5.15.
If DFS returns success and a response buffer containing the referrals, the server MUST then construct an SMB2 IOCTL response following the syntax specified in section 2.2.32, with the following values:
CtlCode MUST be set to the CtlCode in the request.
FileId MUST be set to { 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF }.
InputOffset SHOULD be set to the offset, in bytes, from the beginning of the SMB2 header to the Buffer[] field of the response.
InputCount SHOULD be set to zero.
OutputOffset MUST be set to InputOffset + InputCount, rounded up to a multiple of 8.
OutputCount MUST be set to the number of bytes received from DFS.
Flags MUST be set to zero.
The server MUST copy the buffer that was received from DFS into the Buffer field at the OutputOffset computed above.
The response MUST be sent to the client.
```

### New Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 IOCTL, and a CtlCode of FSCTL_DFS_GET_REFERRALS or FSCTL_DFS_GET_REFERRALS_EX, message handling proceeds as follows:
If IsDfsCapable is set to FALSE, the server MUST return STATUS_FS_DRIVER_REQUIRED to the client.
The server MUST invoke the event as specified in [MS-DFSC] section 3.2.4.2 and pass the following:
The IP address of the client.
The buffer containing the DFS referral request packet.
IsExtendedReferral: Set to TRUE when CtlCode is FSCTL_DFS_GET_REFERRALS_EX.
The maximum size of the response data buffer that will be accepted by the client, as indicated by MaxOutputResponse field in the request.
If DFS returns a failure, the server MUST fail the request with the error code received from DFS. If the error returned from DFS is STATUS_BUFFER_OVERFLOW, the server SHOULD<384> copy the data returned by DFS into a normal FSCTL_GET_DFS_REFERRALS response and return STATUS_BUFFER_OVERFLOW to the client as noted in sections 3.3.4.4 and 3.3.5.15.
If DFS returns success and a response buffer containing the referrals, the server MUST then construct an SMB2 IOCTL response following the syntax specified in section 2.2.32, with the following values:
CtlCode MUST be set to the CtlCode in the request.
FileId MUST be set to { 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF }.
InputOffset SHOULD be set to the offset, in bytes, from the beginning of the SMB2 header to the Buffer[] field of the response.
InputCount SHOULD be set to zero.
OutputOffset MUST be set to InputOffset + InputCount, rounded up to a multiple of 8.
OutputCount MUST be set to the number of bytes received from DFS.
Flags MUST be set to zero.
The server MUST copy the buffer that was received from DFS into the Buffer field at the OutputOffset computed above.
The response MUST be sent to the client.
```

## Section 3.3.5.15.3: Handling a Pipe Transaction Request
**Change type:** Modified

### Old Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 IOCTL, and a CtlCode of FSCTL_PIPE_TRANSCEIVE, message handling proceeds as follows.
If the share on which the request is being executed is not a named pipe share, the server SHOULD<382> fail the request with STATUS_NOT_SUPPORTED.
The server MUST attempt to write the number of bytes specified in the request by the InputCount field into the named pipe. If the write attempt fails, the server MUST fail the request returning the error code received from the named pipe.
The server MUST then attempt to read the number of bytes specified in the request by MaxOutputResponse from the named pipe. If the read attempt fails, the server MUST fail the request returning the error code received from the named pipe. For more information on reading from a pipe, see section 3.3.5.12.
If the read/write attempt is not finished in 1 millisecond, the server MUST send an interim response to the client. If the read/write attempt succeeds,<383> the server MUST then construct an SMB2 IOCTL response following the syntax specified in section 2.2.32, with the following values:
CtlCode MUST be set to FSCTL_PIPE_TRANSCEIVE.
FileId.Persistent MUST be set to Open.DurableFileId. FileId.Volatile MUST be set to Open.FileId.
InputOffset SHOULD be set to the offset, in bytes, from the beginning of the SMB2 header to the Buffer[] field of the response.
InputCount SHOULD<384> be set to zero.
If any data was read from the pipe, OutputOffset MUST be set to InputOffset + InputCount, rounded up to a multiple of 8. Otherwise, OutputOffset SHOULD<385> be set to zero.
OutputCount MUST be set to the number of bytes read from the pipe. If no data is to be returned, the server MUST set OutputCount to zero.
Flags MUST be set to zero.
The server MUST copy the bytes read into the Buffer field at the OutputOffset computed above.
The response MUST be sent to the client.
```

### New Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 IOCTL, and a CtlCode of FSCTL_PIPE_TRANSCEIVE, message handling proceeds as follows.
If the share on which the request is being executed is not a named pipe share, the server SHOULD<385> fail the request with STATUS_NOT_SUPPORTED.
The server MUST attempt to write the number of bytes specified in the request by the InputCount field into the named pipe. If the write attempt fails, the server MUST fail the request returning the error code received from the named pipe.
The server MUST then attempt to read the number of bytes specified in the request by MaxOutputResponse from the named pipe. If the read attempt fails, the server MUST fail the request returning the error code received from the named pipe. For more information on reading from a pipe, see section 3.3.5.12.
If the read/write attempt is not finished in 1 millisecond, the server MUST send an interim response to the client. If the read/write attempt succeeds,<386> the server MUST then construct an SMB2 IOCTL response following the syntax specified in section 2.2.32, with the following values:
CtlCode MUST be set to FSCTL_PIPE_TRANSCEIVE.
FileId.Persistent MUST be set to Open.DurableFileId. FileId.Volatile MUST be set to Open.FileId.
InputOffset SHOULD be set to the offset, in bytes, from the beginning of the SMB2 header to the Buffer[] field of the response.
InputCount SHOULD<387> be set to zero.
If any data was read from the pipe, OutputOffset MUST be set to InputOffset + InputCount, rounded up to a multiple of 8. Otherwise, OutputOffset SHOULD<388> be set to zero.
OutputCount MUST be set to the number of bytes read from the pipe. If no data is to be returned, the server MUST set OutputCount to zero.
Flags MUST be set to zero.
The server MUST copy the bytes read into the Buffer field at the OutputOffset computed above.
The response MUST be sent to the client.
```

## Section 3.3.5.15.4: Handling a Peek at Pipe Data Request
**Change type:** Modified

### Old Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 IOCTL, and a CtlCode of FSCTL_PIPE_PEEK, message handling proceeds as follows:
The server MUST attempt to read the number of bytes specified in the request by MaxOutputResponse from the named pipe without removing the bytes from the pipe. If the read attempt fails, the server MUST fail the request and return the error code received from the named pipe. An FSCTL_PIPE_PEEK MUST never block. A MaxOutputResponse value of zero is allowed.
If the share on which the request is being executed is not a named pipe share, the server SHOULD<386> fail the request with STATUS_NOT_SUPPORTED.
If the read attempt succeeds, the server MUST then construct an SMB2 IOCTL response by following the syntax specified in section 2.2.32, with the following values:
CtlCode MUST be set to FSCTL_PIPE_PEEK.
FileId.Persistent MUST be set to Open.DurableFileId. FileId.Volatile MUST be set to Open.FileId.
InputOffset SHOULD be set to the offset, in bytes, from the beginning of the SMB2 header to the Buffer[] field of the response.
InputCount SHOULD be set to zero.
If any data was read from the pipe, OutputOffset MUST be set to InputOffset + InputCount, rounded up to a multiple of 8. Otherwise, OutputOffset SHOULD<387> be set to zero.
OutputCount MUST be set to the number of bytes read from the pipe.
Flags MUST be set to zero.
The server MUST copy the bytes read into the Buffer field at the OutputOffset computed above.
The response MUST be sent to the client.
```

### New Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 IOCTL, and a CtlCode of FSCTL_PIPE_PEEK, message handling proceeds as follows:
The server MUST attempt to read the number of bytes specified in the request by MaxOutputResponse from the named pipe without removing the bytes from the pipe. If the read attempt fails, the server MUST fail the request and return the error code received from the named pipe. An FSCTL_PIPE_PEEK MUST never block. A MaxOutputResponse value of zero is allowed.
If the share on which the request is being executed is not a named pipe share, the server SHOULD<389> fail the request with STATUS_NOT_SUPPORTED.
If the read attempt succeeds, the server MUST then construct an SMB2 IOCTL response by following the syntax specified in section 2.2.32, with the following values:
CtlCode MUST be set to FSCTL_PIPE_PEEK.
FileId.Persistent MUST be set to Open.DurableFileId. FileId.Volatile MUST be set to Open.FileId.
InputOffset SHOULD be set to the offset, in bytes, from the beginning of the SMB2 header to the Buffer[] field of the response.
InputCount SHOULD be set to zero.
If any data was read from the pipe, OutputOffset MUST be set to InputOffset + InputCount, rounded up to a multiple of 8. Otherwise, OutputOffset SHOULD<390> be set to zero.
OutputCount MUST be set to the number of bytes read from the pipe.
Flags MUST be set to zero.
The server MUST copy the bytes read into the Buffer field at the OutputOffset computed above.
The response MUST be sent to the client.
```

## Section 3.3.5.15.5: Handling a Source File Key Request
**Change type:** Modified

### Old Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 IOCTL, and a CtlCode of FSCTL_SRV_REQUEST_RESUME_KEY, message handling proceeds as follows.
The SRV_REQUEST_RESUME_KEY Response is an opaque 24 byte blob followed by optional context as described in 2.2.32.3.<388>
The server MUST provide a 24-byte value that is used to uniquely identify the open. The server SHOULD use Open.DurableFileId, or alternately, MAY use an internally generated value that is unique for all opens on the server.<389> The server MUST set the Open.ResumeKey and ResumeKey values in the SRV_REQUEST_RESUME_KEY Response to the generated value.
If the maximum output buffer size specified is too small to contain an SRV_REQUEST_RESUME_KEY structure, the server MUST return the status STATUS_INVALID_PARAMETER.
The server MUST construct an SMB2 IOCTL response following the syntax specified in section 2.2.32, with the following values:
CtlCode MUST be set to FSCTL_SRV_REQUEST_RESUME_KEY.
FileId.Persistent MUST be set to Open.DurableFileId. FileId.Volatile MUST be set to Open.FileId.
InputOffset SHOULD be set to the offset, in bytes, from the beginning of the SMB2 header to the Buffer[] field of the response.
InputCount SHOULD be set to zero.
OutputOffset MUST be set to InputOffset + InputCount, rounded up to a multiple of 8.
OutputCount MUST be set to 32.
Flags MUST be set to zero.
The server MUST copy the constructed SRV_REQUEST_RESUME_KEY that is used to identify the open into the Buffer field at the OutputOffset computed above.
The response MUST be sent to the client.
```

### New Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 IOCTL, and a CtlCode of FSCTL_SRV_REQUEST_RESUME_KEY, message handling proceeds as follows.
The SRV_REQUEST_RESUME_KEY Response is an opaque 24 byte blob followed by optional context as described in 2.2.32.3.<391>
The server MUST provide a 24-byte value that is used to uniquely identify the open. The server SHOULD use Open.DurableFileId, or alternately, MAY use an internally generated value that is unique for all opens on the server.<392> The server MUST set the Open.ResumeKey and ResumeKey values in the SRV_REQUEST_RESUME_KEY Response to the generated value.
If the maximum output buffer size specified is too small to contain an SRV_REQUEST_RESUME_KEY structure, the server MUST return the status STATUS_INVALID_PARAMETER.
The server MUST construct an SMB2 IOCTL response following the syntax specified in section 2.2.32, with the following values:
CtlCode MUST be set to FSCTL_SRV_REQUEST_RESUME_KEY.
FileId.Persistent MUST be set to Open.DurableFileId. FileId.Volatile MUST be set to Open.FileId.
InputOffset SHOULD be set to the offset, in bytes, from the beginning of the SMB2 header to the Buffer[] field of the response.
InputCount SHOULD be set to zero.
OutputOffset MUST be set to InputOffset + InputCount, rounded up to a multiple of 8.
OutputCount MUST be set to 32.
Flags MUST be set to zero.
The server MUST copy the constructed SRV_REQUEST_RESUME_KEY that is used to identify the open into the Buffer field at the OutputOffset computed above.
The response MUST be sent to the client.
```

## Section 3.3.5.15.7: Handling a Content Information Retrieval Request
**Change type:** Modified

### Old Content
```
When the server receives a request that has an SMB2 header with a Command value equal to SMB2 IOCTL and a CtlCode of FSCTL_SRV_READ_HASH, message handling proceeds as follows:
The server MUST fail the SRV_READ_HASH request (section 2.2.31.2) with the error code specified in the following cases:
If the server does not support SRV_READ_HASH requests, it MUST fail the request with STATUS_NOT_SUPPORTED.<391>
If the server supports SRV_READ_HASH requests but does not have the branch cache feature available, it SHOULD<392> fail the request with STATUS_HASH_NOT_PRESENT.
The server MUST fail the request with error STATUS_BUFFER_TOO_SMALL if any of the following cases:
InputCount in the request is less than the size of a SRV_READ_HASH request
HashRetrievalType is SRV_HASH_RETRIEVE_HASH_BASED and MaxOutputResponse in the request is less than the size of the SRV_HASH_RETRIEVE_HASH_BASED structure
HashRetrievalType is SRV_HASH_RETRIEVE_FILE_BASED and MaxOutputResponse in the request is less than the size of the SRV_HASH_RETRIEVE_FILE_BASED structure
The server MUST fail the SRV_READ_HASH request with an error of STATUS_INVALID_PARAMETER in the following cases:
If the HashType field of the SRV_READ_HASH request is not equal to SRV_HASH_TYPE_PEER_DIST.
If the server implements only the SMB 2.1 dialect and the HashVersion field is not equal to SRV_HASH_VER_1.
If the server implements the SMB 3.x dialect family and the HashVersion field is not equal to either SRV_HASH_VER_1 or SRV_HASH_VER_2.
If the HashRetrievalType field is not equal to SRV_HASH_RETRIEVE_HASH_BASED or SRV_HASH_RETRIEVE_FILE_BASED.
If the HashVersion field is equal to SRV_HASH_VER_1 and the HashRetrievalType field is not equal to SRV_HASH_RETRIEVE_HASH_BASED.
If the HashVersion field is equal to SRV_HASH_VER_2 and the HashRetrievalType field is not equal to SRV_HASH_RETRIEVE_FILE_BASED.
If ServerHashLevel is HashDisableAll, the server MUST fail the SRV_READ_HASH request with error code STATUS_HASH_NOT_SUPPORTED.
If the HashRetrievalType is SRV_HASH_RETRIEVE_HASH_BASED the server MUST open the Content Information File from the object store for the object represented by Open.LocalOpen with the specified offset. If the Content Information File open fails, the server MUST fail the request with STATUS_HASH_NOT_PRESENT.
If the HashRetrievalType is SRV_HASH_RETRIEVE_FILE_BASED the server MUST open the Content Information File from the object store for the object represented by Open.LocalOpen. If the Content Information File open fails, the server MUST fail the request with STATUS_HASH_NOT_PRESENT.
If ServerHashLevel is HashEnableShare and Open.TreeConnect.Share.HashEnabled is FALSE, the server MUST fail the SRV_READ_HASH request with error code STATUS_HASH_NOT_SUPPORTED.
If HashRetrievalType is SRV_HASH_RETRIEVE_HASH_BASED, the Length MUST be set to min[(MaxOutputResponse-16), Length in the request]. If HashRetrievalType is SRV_HASH_RETRIEVE_FILE_BASED, the Length MUST be set to min[(MaxOutputResponse-24), Length in the request].
The server MUST open the Content Information File from the object store for the object represented by Open.LocalOpen and read Length number of bytes at the specified Offset. If the Content Information File open fails, the server MUST fail the SRV_READ_HASH request with the error code returned by object store.
If the Content Information File open succeeds, the server MUST verify the following:
If the Content Information File is empty, the server MUST fail the SRV_READ_HASH request with the error code STATUS_HASH_NOT_PRESENT.
If HashRetrievalType is SRV_HASH_RETRIEVE_HASH_BASED and the Offset field of the SRV_READ_HASH request is equal to or beyond the end of the Content Information File, the server MUST fail the SRV_READ_HASH request with error code STATUS_END_OF_FILE.
If the HashRetrievalType is SRV_HASH_RETRIEVE_FILE_BASED and Offset field of the SRV_READ_HASH request is equal to or beyond the end of the file represented by Open.LocalOpen, the server MUST fail the SRV_READ_HASH request with error code STATUS_END_OF_FILE.
The Content Information File MUST start with a valid HASH_HEADER as specified in section 2.2.32.4.1.
If the HashType field in the HASH_HEADER is not equal to the HashRetrievalType field of the SRV_READ_HASH request, the server MUST fail the SRV_READ_HASH request with the error code STATUS_HASH_NOT_PRESENT.
If the HashVersion field in the HASH_HEADER is not equal to the HashVersion field of the SRV_READ_HASH request, the server MUST fail the SRV_READ_HASH request with the error code STATUS_HASH_NOT_PRESENT.
If the Dirty field in the HASH_HEADER is a nonzero value, the server MUST fail the SRV_READ_HASH request with the error code STATUS_HASH_NOT_PRESENT.
If the server implements the SMB 3.x dialect family and the HashVersion field in the SRV_READ_HASH Request is SRV_HASH_VER_2, the server MUST set HashBlobLength in the HASH_HEADER to zero.
If the Content Information File is verified successfully, the server MUST construct an SMB2 IOCTL response following the syntax specified in section 2.2.32, with the following values:
CtlCode MUST be set to FSCTL_SRV_READ_HASH.
FileId.Persistent MUST be set to Open.DurableFileId.
FileId.Volatile MUST be set to Open.FileId.
InputOffset SHOULD be set to the offset, in bytes, from the beginning of the SMB2 header to the Buffer[] field of the response.
InputCount SHOULD be set to 0.
OutputOffset MUST be set to InputOffset + InputCount, rounded up to a multiple of 8.
OutputCount MUST be set to the size of SRV_READ_HASH Response, including the variable length for Content Information.
Flags MUST be set to zero.
If the HashRetrievalType is SRV_HASH_RETRIEVE_HASH_BASED, the server MUST copy a SRV_READ_HASH Response following the syntax specified in section 2.2.32.4.2 into the Buffer field at the OutputOffset computed above. The server MUST set the Offset to the Offset field in the SRV_READ_HASH request and BufferLength to the length of the returned content.
If the HashRetrievalType is SRV_HASH_RETRIEVE_FILE_BASED, the server MUST copy a SRV_READ_HASH Response following the syntax specified in section 2.2.32.4.3 into the Buffer field at the OutputOffset computed above. The server SHOULD<393> set the FileDataOffset and FileDataLength fields to the offset and length of the region of the object that is covered by the returned content. If the Offset field in the SRV_READ_HASH request is zero, the server MUST also copy the HASH_HEADER from the Content Information File, as specified in section 2.2.32.4.1, at the beginning of the Buffer[] field of the response.
```

### New Content
```
When the server receives a request that has an SMB2 header with a Command value equal to SMB2 IOCTL and a CtlCode of FSCTL_SRV_READ_HASH, message handling proceeds as follows:
The server MUST fail the SRV_READ_HASH request (section 2.2.31.2) with the error code specified in the following cases:
If the server does not support SRV_READ_HASH requests, it MUST fail the request with STATUS_NOT_SUPPORTED.<394>
If the server supports SRV_READ_HASH requests but does not have the branch cache feature available, it SHOULD<395> fail the request with STATUS_HASH_NOT_PRESENT.
The server MUST fail the request with error STATUS_BUFFER_TOO_SMALL if any of the following cases:
InputCount in the request is less than the size of a SRV_READ_HASH request
HashRetrievalType is SRV_HASH_RETRIEVE_HASH_BASED and MaxOutputResponse in the request is less than the size of the SRV_HASH_RETRIEVE_HASH_BASED structure
HashRetrievalType is SRV_HASH_RETRIEVE_FILE_BASED and MaxOutputResponse in the request is less than the size of the SRV_HASH_RETRIEVE_FILE_BASED structure
The server MUST fail the SRV_READ_HASH request with an error of STATUS_INVALID_PARAMETER in the following cases:
If the HashType field of the SRV_READ_HASH request is not equal to SRV_HASH_TYPE_PEER_DIST.
If the server implements only the SMB 2.1 dialect and the HashVersion field is not equal to SRV_HASH_VER_1.
If the server implements the SMB 3.x dialect family and the HashVersion field is not equal to either SRV_HASH_VER_1 or SRV_HASH_VER_2.
If the HashRetrievalType field is not equal to SRV_HASH_RETRIEVE_HASH_BASED or SRV_HASH_RETRIEVE_FILE_BASED.
If the HashVersion field is equal to SRV_HASH_VER_1 and the HashRetrievalType field is not equal to SRV_HASH_RETRIEVE_HASH_BASED.
If the HashVersion field is equal to SRV_HASH_VER_2 and the HashRetrievalType field is not equal to SRV_HASH_RETRIEVE_FILE_BASED.
If ServerHashLevel is HashDisableAll, the server MUST fail the SRV_READ_HASH request with error code STATUS_HASH_NOT_SUPPORTED.
If the HashRetrievalType is SRV_HASH_RETRIEVE_HASH_BASED the server MUST open the Content Information File from the object store for the object represented by Open.LocalOpen with the specified offset. If the Content Information File open fails, the server MUST fail the request with STATUS_HASH_NOT_PRESENT.
If the HashRetrievalType is SRV_HASH_RETRIEVE_FILE_BASED the server MUST open the Content Information File from the object store for the object represented by Open.LocalOpen. If the Content Information File open fails, the server MUST fail the request with STATUS_HASH_NOT_PRESENT.
If ServerHashLevel is HashEnableShare and Open.TreeConnect.Share.HashEnabled is FALSE, the server MUST fail the SRV_READ_HASH request with error code STATUS_HASH_NOT_SUPPORTED.
If HashRetrievalType is SRV_HASH_RETRIEVE_HASH_BASED, the Length MUST be set to min[(MaxOutputResponse-16), Length in the request]. If HashRetrievalType is SRV_HASH_RETRIEVE_FILE_BASED, the Length MUST be set to min[(MaxOutputResponse-24), Length in the request].
The server MUST open the Content Information File from the object store for the object represented by Open.LocalOpen and read Length number of bytes at the specified Offset. If the Content Information File open fails, the server MUST fail the SRV_READ_HASH request with the error code returned by object store.
If the Content Information File open succeeds, the server MUST verify the following:
If the Content Information File is empty, the server MUST fail the SRV_READ_HASH request with the error code STATUS_HASH_NOT_PRESENT.
If HashRetrievalType is SRV_HASH_RETRIEVE_HASH_BASED and the Offset field of the SRV_READ_HASH request is equal to or beyond the end of the Content Information File, the server MUST fail the SRV_READ_HASH request with error code STATUS_END_OF_FILE.
If the HashRetrievalType is SRV_HASH_RETRIEVE_FILE_BASED and Offset field of the SRV_READ_HASH request is equal to or beyond the end of the file represented by Open.LocalOpen, the server MUST fail the SRV_READ_HASH request with error code STATUS_END_OF_FILE.
The Content Information File MUST start with a valid HASH_HEADER as specified in section 2.2.32.4.1.
If the HashType field in the HASH_HEADER is not equal to the HashRetrievalType field of the SRV_READ_HASH request, the server MUST fail the SRV_READ_HASH request with the error code STATUS_HASH_NOT_PRESENT.
If the HashVersion field in the HASH_HEADER is not equal to the HashVersion field of the SRV_READ_HASH request, the server MUST fail the SRV_READ_HASH request with the error code STATUS_HASH_NOT_PRESENT.
If the Dirty field in the HASH_HEADER is a nonzero value, the server MUST fail the SRV_READ_HASH request with the error code STATUS_HASH_NOT_PRESENT.
If the server implements the SMB 3.x dialect family and the HashVersion field in the SRV_READ_HASH Request is SRV_HASH_VER_2, the server MUST set HashBlobLength in the HASH_HEADER to zero.
If the Content Information File is verified successfully, the server MUST construct an SMB2 IOCTL response following the syntax specified in section 2.2.32, with the following values:
CtlCode MUST be set to FSCTL_SRV_READ_HASH.
FileId.Persistent MUST be set to Open.DurableFileId.
FileId.Volatile MUST be set to Open.FileId.
InputOffset SHOULD be set to the offset, in bytes, from the beginning of the SMB2 header to the Buffer[] field of the response.
InputCount SHOULD be set to 0.
OutputOffset MUST be set to InputOffset + InputCount, rounded up to a multiple of 8.
OutputCount MUST be set to the size of SRV_READ_HASH Response, including the variable length for Content Information.
Flags MUST be set to zero.
If the HashRetrievalType is SRV_HASH_RETRIEVE_HASH_BASED, the server MUST copy a SRV_READ_HASH Response following the syntax specified in section 2.2.32.4.2 into the Buffer field at the OutputOffset computed above. The server MUST set the Offset to the Offset field in the SRV_READ_HASH request and BufferLength to the length of the returned content.
If the HashRetrievalType is SRV_HASH_RETRIEVE_FILE_BASED, the server MUST copy a SRV_READ_HASH Response following the syntax specified in section 2.2.32.4.3 into the Buffer field at the OutputOffset computed above. The server SHOULD<396> set the FileDataOffset and FileDataLength fields to the offset and length of the region of the object that is covered by the returned content. If the Offset field in the SRV_READ_HASH request is zero, the server MUST also copy the HASH_HEADER from the Content Information File, as specified in section 2.2.32.4.1, at the beginning of the Buffer[] field of the response.
```

## Section 3.3.5.15.8: Handling a Pass-Through Operation Request
**Change type:** Modified

### Old Content
```
Pass-through requests are I/O Control requests and File System Control (FSCTL) requests with a CtlCode value that is not specified in section 2.2.31. As noted in section 3.3.5.15, the server MUST fail I/O Control requests with STATUS_NOT_SUPPORTED.
Pass-through FSCTL requests fall further into two types, those for which a CtlCode value matches an FSCTL function number defined in [MS-FSCC] section 2.3, and those that do not. When the latter type of pass-through request does not meet the private FSCTL requirements of [MS-FSCC] section 2.3, the server MUST NOT pass the request to the underlying object store and MUST fail the request by sending a response of STATUS_NOT_SUPPORTED.
Otherwise, when the server receives a pass-through FSCTL request, the server SHOULD<394> pass it through to the underlying object store.
The server MUST pass the following to the underlying object store: CtlCode, the input buffer described by InputOffset and InputCount, the output buffer described by OutputOffset and OutputCount, the MaxOutputResponse as the maximum output buffer size, in bytes, for the response, and MaxInputResponse as the maximum input buffer size, in bytes, for the response. Where the CtlCode value matches an FSCTL function number defined in [MS-FSCC], the server SHOULD verify that the above buffers and sizes conform to the requirements of the corresponding structures defined in [MS-FSCC] section 2.3, and use the FileId from the SMB2 IOCTL request to obtain the handle described in [MS-FSCC] section 2.3 to pass to the object store. Where the CtlCode value is not defined in [MS-FSCC], the server SHOULD<395> ensure that the other requirements for private FSCTLs defined in [MS-FSCC] are met.
If the underlying object store returns a failure, the server MUST fail the request and send a response with an error code, as specified in [MS-ERREF] section 2.2.
Note that a successful FSCTL pass-through request could return 0 bytes of output buffer data, and have OutputCount set to 0. Similarly, it is possible for a valid FSCTL pass-through request to send 0 bytes of input buffer data, depending on the requirements of the FSCTL.
If the operation succeeds, the server MUST then construct an SMB2 IOCTL Response following the syntax specified in section 2.2.32, with the following values:
CtlCode MUST be set to the CtlCode of the request.
FileId.Persistent MUST be set to Open.DurableFileId. FileId.Volatile MUST be set to Open.FileId.
InputOffset SHOULD be set to the offset, in bytes, from the beginning of the SMB2 header to the Buffer[] field of the response.
InputCount MUST be set to the number of input bytes the object store is returning to the client.
If the object store is returning output data to the client, OutputOffset MUST be set to InputOffset + InputCount, rounded up to a multiple of 8. Otherwise, OutputOffset SHOULD<396> be set to zero.
The server MUST set the OutputCount to the actual number of bytes returned by the underlying object store in the output buffer.
Flags MUST be set to zero.
The server MUST copy the input and output response bytes into the ranges in Buffer described by InputOffset/InputCount and OutputOffset/OutputCount.
The response MUST be sent to the client.
```

### New Content
```
Pass-through requests are I/O Control requests and File System Control (FSCTL) requests with a CtlCode value that is not specified in section 2.2.31. As noted in section 3.3.5.15, the server MUST fail I/O Control requests with STATUS_NOT_SUPPORTED.
Pass-through FSCTL requests fall further into two types, those for which a CtlCode value matches an FSCTL function number defined in [MS-FSCC] section 2.3, and those that do not. When the latter type of pass-through request does not meet the private FSCTL requirements of [MS-FSCC] section 2.3, the server MUST NOT pass the request to the underlying object store and MUST fail the request by sending a response of STATUS_NOT_SUPPORTED.
Otherwise, when the server receives a pass-through FSCTL request, the server SHOULD<397> pass it through to the underlying object store.
The server MUST pass the following to the underlying object store: CtlCode, the input buffer described by InputOffset and InputCount, the output buffer described by OutputOffset and OutputCount, the MaxOutputResponse as the maximum output buffer size, in bytes, for the response, and MaxInputResponse as the maximum input buffer size, in bytes, for the response. Where the CtlCode value matches an FSCTL function number defined in [MS-FSCC], the server SHOULD verify that the above buffers and sizes conform to the requirements of the corresponding structures defined in [MS-FSCC] section 2.3, and use the FileId from the SMB2 IOCTL request to obtain the handle described in [MS-FSCC] section 2.3 to pass to the object store. Where the CtlCode value is not defined in [MS-FSCC], the server SHOULD<398> ensure that the other requirements for private FSCTLs defined in [MS-FSCC] are met.
If the underlying object store returns a failure, the server MUST fail the request and send a response with an error code, as specified in [MS-ERREF] section 2.2.
Note that a successful FSCTL pass-through request could return 0 bytes of output buffer data, and have OutputCount set to 0. Similarly, it is possible for a valid FSCTL pass-through request to send 0 bytes of input buffer data, depending on the requirements of the FSCTL.
If the operation succeeds, the server MUST then construct an SMB2 IOCTL Response following the syntax specified in section 2.2.32, with the following values:
CtlCode MUST be set to the CtlCode of the request.
FileId.Persistent MUST be set to Open.DurableFileId. FileId.Volatile MUST be set to Open.FileId.
InputOffset SHOULD be set to the offset, in bytes, from the beginning of the SMB2 header to the Buffer[] field of the response.
InputCount MUST be set to the number of input bytes the object store is returning to the client.
If the object store is returning output data to the client, OutputOffset MUST be set to InputOffset + InputCount, rounded up to a multiple of 8. Otherwise, OutputOffset SHOULD<399> be set to zero.
The server MUST set the OutputCount to the actual number of bytes returned by the underlying object store in the output buffer.
Flags MUST be set to zero.
The server MUST copy the input and output response bytes into the ranges in Buffer described by InputOffset/InputCount and OutputOffset/OutputCount.
The response MUST be sent to the client.
```

## Section 3.3.5.15.9: Handling a Resiliency Request
**Change type:** Modified

### Old Content
```
This section applies only to servers that implement the SMB 2.1 or the SMB 3.x dialect family.
When the server receives a request with an SMB2 header with a Command value equal to SMB2 IOCTL and a CtlCode FSCTL_LMR_REQUEST_RESILIENCY, message handling proceeds as follows.
If Open.Connection.Dialect is "2.0.2", the server MAY<397> fail the request with STATUS_INVALID_DEVICE_REQUEST.
Otherwise, if the server does not support FSCTL_LMR_REQUEST_RESILIENCY requests, the server SHOULD fail the request with STATUS_NOT_SUPPORTED.
If InputCount is smaller than the size of the NETWORK_RESILIENCY_REQUEST request as specified in section 2.2.31.3, or if the requested Timeout in seconds is greater than MaxResiliencyTimeout in seconds, the request MUST be failed with STATUS_INVALID_PARAMETER.
Open.IsDurable MUST be set to FALSE. Open.IsResilient MUST be set to TRUE. If the value of the Timeout field specified in NETWORK_RESILIENCY_REQUEST of the request is not zero, Open.ResiliencyTimeout MUST be set to the value of the Timeout field; otherwise, Open.ResiliencyTimeout SHOULD be set to an implementation-specific value.<398> Open.DurableOwner MUST be set to a security descriptor accessible only by the user represented by Open.Session.SecurityContext.
The server MUST construct an SMB2 IOCTL response following the syntax specified in section 2.2.32, with the following values:
CtlCode MUST be set to FSCTL_LMR_REQUEST_RESILIENCY.
FileId.Persistent MUST be set to Open.DurableFileId. FileId.Volatile MUST be set to Open.FileId.
InputOffset SHOULD be set to the offset, in bytes, from the beginning of the SMB2 header to the Buffer[] field of the response.
InputCount SHOULD be set to zero.
OutputOffset MUST be set to InputOffset + InputCount, rounded up to a multiple of 8.
OutputCount MUST be set to zero.
Flags MUST be set to zero.
The response MUST be sent to the client.
```

### New Content
```
This section applies only to servers that implement the SMB 2.1 or the SMB 3.x dialect family.
When the server receives a request with an SMB2 header with a Command value equal to SMB2 IOCTL and a CtlCode FSCTL_LMR_REQUEST_RESILIENCY, message handling proceeds as follows.
If Open.Connection.Dialect is "2.0.2", the server MAY<400> fail the request with STATUS_INVALID_DEVICE_REQUEST.
Otherwise, if the server does not support FSCTL_LMR_REQUEST_RESILIENCY requests, the server SHOULD fail the request with STATUS_NOT_SUPPORTED.
If InputCount is smaller than the size of the NETWORK_RESILIENCY_REQUEST request as specified in section 2.2.31.3, or if the requested Timeout in seconds is greater than MaxResiliencyTimeout in seconds, the request MUST be failed with STATUS_INVALID_PARAMETER.
Open.IsDurable MUST be set to FALSE. Open.IsResilient MUST be set to TRUE. If the value of the Timeout field specified in NETWORK_RESILIENCY_REQUEST of the request is not zero, Open.ResiliencyTimeout MUST be set to the value of the Timeout field; otherwise, Open.ResiliencyTimeout SHOULD be set to an implementation-specific value.<401> Open.DurableOwner MUST be set to a security descriptor accessible only by the user represented by Open.Session.SecurityContext.
The server MUST construct an SMB2 IOCTL response following the syntax specified in section 2.2.32, with the following values:
CtlCode MUST be set to FSCTL_LMR_REQUEST_RESILIENCY.
FileId.Persistent MUST be set to Open.DurableFileId. FileId.Volatile MUST be set to Open.FileId.
InputOffset SHOULD be set to the offset, in bytes, from the beginning of the SMB2 header to the Buffer[] field of the response.
InputCount SHOULD be set to zero.
OutputOffset MUST be set to InputOffset + InputCount, rounded up to a multiple of 8.
OutputCount MUST be set to zero.
Flags MUST be set to zero.
The response MUST be sent to the client.
```

## Section 3.3.5.15.13: Handling a Set Reparse Point Request
**Change type:** Modified

### Old Content
```
This section applies only to servers that implement the SMB 3.x dialect family.
When the server receives a request that contains an SMB2 header with a Command value equal to SMB2 IOCTL and a CtlCode of FSCTL_SET_REPARSE_POINT, message handling proceeds as follows:
If the ReparseTag field in FSCTL_SET_REPARSE_POINT, as specified in [MS-FSCC] section 2.3.81, is not IO_REPARSE_TAG_SYMLINK, the server SHOULD verify that the caller has the required permissions to execute this FSCTL.<399> If the caller does not have the required permissions, the server MUST fail the call with an error code of STATUS_ACCESS_DENIED.
The server MUST process this request as a pass-through operation as specified in section 3.3.5.15.8.
```

### New Content
```
This section applies only to servers that implement the SMB 3.x dialect family.
When the server receives a request that contains an SMB2 header with a Command value equal to SMB2 IOCTL and a CtlCode of FSCTL_SET_REPARSE_POINT, message handling proceeds as follows:
If the ReparseTag field in FSCTL_SET_REPARSE_POINT, as specified in [MS-FSCC] section 2.3.81, is not IO_REPARSE_TAG_SYMLINK, the server SHOULD verify that the caller has the required permissions to execute this FSCTL.<402> If the caller does not have the required permissions, the server MUST fail the call with an error code of STATUS_ACCESS_DENIED.
The server MUST process this request as a pass-through operation as specified in section 3.3.5.15.8.
```

## Section 3.3.5.16: Receiving an SMB2 CANCEL Request
**Change type:** Modified

### Old Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 CANCEL, message handling proceeds as follows:
An SMB2 CANCEL Request does not contain a sequence number that MUST be checked. Thus, the server MUST NOT process the received packet as specified in section 3.3.5.2.3.
If SMB2_FLAGS_SIGNED bit is set in the Flags field of the SMB2 header of the cancel request, the server MUST verify the session, as specified in section 3.3.5.2.9.
If SMB2_FLAGS_ASYNC_COMMAND is set in the Flags field of the SMB2 header of the cancel request, the server SHOULD<400> search for a request in Connection.AsyncCommandList where Request.AsyncId matches the AsyncId of the incoming cancel request. If SMB2_FLAGS_ASYNC_COMMAND is not set, then the server MUST search for a request in Connection.RequestList where Request.MessageId matches the MessageId of the incoming cancel request.
If a request is not found, the server MUST stop processing for this cancel request. No response is sent.
If a request is found, the server SHOULD<401> attempt to cancel the request that was found, referred to here as the target request. If the target request is successfully canceled, the target request MUST be failed by sending an ERROR response packet as specified in section 2.2.2, with the status field of the SMB2 header (specified in section 2.2.1) set to STATUS_CANCELLED. If the target request is not successfully canceled, processing of the target request MUST continue and no response is sent to the cancel request.
The cancel request indicates that the client is required to get a response for the target request, whether successful or not. The server MUST expedite the cancellation request by following the above steps.
```

### New Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 CANCEL, message handling proceeds as follows:
An SMB2 CANCEL Request does not contain a sequence number that MUST be checked. Thus, the server MUST NOT process the received packet as specified in section 3.3.5.2.3.
If SMB2_FLAGS_SIGNED bit is set in the Flags field of the SMB2 header of the cancel request, the server MUST verify the session, as specified in section 3.3.5.2.9.
If SMB2_FLAGS_ASYNC_COMMAND is set in the Flags field of the SMB2 header of the cancel request, the server SHOULD<403> search for a request in Connection.AsyncCommandList where Request.AsyncId matches the AsyncId of the incoming cancel request. If SMB2_FLAGS_ASYNC_COMMAND is not set, then the server MUST search for a request in Connection.RequestList where Request.MessageId matches the MessageId of the incoming cancel request.
If a request is not found, the server MUST stop processing for this cancel request. No response is sent.
If a request is found, the server SHOULD<404> attempt to cancel the request that was found, referred to here as the target request. If the target request is successfully canceled, the target request MUST be failed by sending an ERROR response packet as specified in section 2.2.2, with the status field of the SMB2 header (specified in section 2.2.1) set to STATUS_CANCELLED. If the target request is not successfully canceled, processing of the target request MUST continue and no response is sent to the cancel request.
The cancel request indicates that the client is required to get a response for the target request, whether successful or not. The server MUST expedite the cancellation request by following the above steps.
```

## Section 3.3.5.17: Receiving an SMB2 ECHO Request
**Change type:** Modified

### Old Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 ECHO, message handling proceeds as follows:
If Connection.SessionTable is empty, the server SHOULD<402> disconnect the connection.
The server MUST verify the session, as specified in section 3.3.5.2.9, if any of the following conditions is TRUE:
SMB2_FLAGS_SIGNED bit is set in the Flags field of the SMB2 header of the request.
The request is not encrypted, and the SessionId field of the SMB2 header of the request is not zero.
The server MUST construct an SMB2 ECHO Response following the syntax specified in section 2.2.29 and MUST send it to the client.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_INVALID_PARAMETER
```

### New Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 ECHO, message handling proceeds as follows:
If Connection.SessionTable is empty, the server SHOULD<405> disconnect the connection.
The server MUST verify the session, as specified in section 3.3.5.2.9, if any of the following conditions is TRUE:
SMB2_FLAGS_SIGNED bit is set in the Flags field of the SMB2 header of the request.
The request is not encrypted, and the SessionId field of the SMB2 header of the request is not zero.
The server MUST construct an SMB2 ECHO Response following the syntax specified in section 2.2.29 and MUST send it to the client.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_INVALID_PARAMETER
```

## Section 3.3.5.18: Receiving an SMB2 QUERY_DIRECTORY Request
**Change type:** Modified

### Old Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 QUERY_DIRECTORY, message handling proceeds as follows:
The server MUST locate the session, as specified in section 3.3.5.2.9.
The server MUST locate the tree connection, as specified in section 3.3.5.2.11.
Next, the server MUST locate the open for the directory to be queried by performing a lookup in the Session.OpenTable, using the FileId.Volatile of the request as the lookup key. If no open is found, or if Open.DurableFileId is not equal to FileId.Persistent, the server MUST fail the request with STATUS_FILE_CLOSED. Otherwise, the server MUST locate the Request in Connection.RequestList for which Request.MessageId matches the MessageId value in the SMB2 header, and set Request.Open to the Open.
If Open.IsPersistent is FALSE and Open.IsReplayEligible is TRUE, the server MUST set Open.IsReplayEligible to FALSE.
If the open is not an open to a directory, the server MUST process the request as follows:
If SMB2_REOPEN is set in the Flags field of the SMB2 QUERY_DIRECTORY request, the request MUST be failed with an implementation-specific error code.<403>
Otherwise, the request MUST be failed with STATUS_INVALID_PARAMETER.
If OutputBufferLength is greater than Connection.MaxTransactSize, the server SHOULD<404> fail the request with STATUS_INVALID_PARAMETER.
If Connection.SupportsMultiCredit is TRUE, the server MUST validate CreditCharge based on OutputBufferLength, as specified in section 3.3.5.2.5. If the validation fails, it MUST fail the request with STATUS_INVALID_PARAMETER.
If Open.GrantedAccess does not include FILE_LIST_DIRECTORY, the operation MUST be failed with STATUS_ACCESS_DENIED.
The information classes supported are specified in [MS-FSCC] section 2.4. The supported classes for the query are:
FileDirectoryInformation
FileFullDirectoryInformation
FileBothDirectoryInformation
FileIdFullDirectoryInformation
FileIdBothDirectoryInformation
FileNamesInformation
FileIdExtdDirectoryInformation
FileId64ExtdDirectoryInformation
FileId64ExtdBothDirectoryInformation
FileIdAllExtdDirectoryInformation
FileIdAllExtdBothDirectoryInformation
If any other information class is specified in the FileInformationClass field of the SMB2 QUERY_DIRECTORY Request, the server MUST fail the operation with STATUS_INVALID_INFO_CLASS. If the information class requested is not supported by the server, the server MUST fail the request with STATUS_NOT_SUPPORTED.
If SMB2_RESTART_SCANS or SMB2_REOPEN is set in the Flags field of the SMB2 QUERY_DIRECTORY Request, the server MUST restart the scan with the search pattern specified, in an implementation-specific manner<405>.
If SMB2_RETURN_SINGLE_ENTRY is set in the Flags field of the request, the server MUST return only a single entry.
The server MUST invoke the query directory procedure from the underlying object store in an implementation-specific manner<406>.
The server MAY<407> choose to support resuming enumerations by index number, if SMB2_INDEX_SPECIFIED is set in the Flags field and an index number is specified in the FileIndex field of the SMB2 QUERY_DIRECTORY Request.
If TreeConnect.Share.DoAccessBasedDirectoryEnumeration is TRUE and the object store supports security, the server MUST also exclude entries for which the user represented by Session.SecurityContext is not granted GENERIC_READ and FILE_LIST_DIRECTORY access.
Otherwise, the server MUST construct an SMB2_QUERY_DIRECTORY Response following the syntax specified in section 2.2.34, with the following values:
OutputBufferOffset MUST be set to the offset, in bytes, from the beginning of the SMB2 header where the enumeration data is being placed, the offset to Buffer[].
OutputBufferLength MUST be set to the length, in bytes, of the result of the enumeration.
The enumeration data MUST be copied into Buffer[].
The response MUST be sent to the client.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_INSUFFICIENT_RESOURCES
STATUS_ACCESS_DENIED
STATUS_FILE_CLOSED
STATUS_NETWORK_NAME_DELETED
STATUS_USER_SESSION_DELETED
STATUS_NETWORK_SESSION_EXPIRED
STATUS_INVALID_PARAMETER
STATUS_INVALID_INFO_CLASS
STATUS_NO_SUCH_FILE
STATUS_CANCELLED
STATUS_NOT_SUPPORTED
STATUS_OBJECT_NAME_INVALID
STATUS_VOLUME_DISMOUNTED
STATUS_INVALID_INFO_CLASS
STATUS_FILE_CORRUPT_ERROR
STATUS_NO_MORE_FILES
```

### New Content
```
When the server receives a request with an SMB2 header with a Command value equal to SMB2 QUERY_DIRECTORY, message handling proceeds as follows:
The server MUST locate the session, as specified in section 3.3.5.2.9.
The server MUST locate the tree connection, as specified in section 3.3.5.2.11.
Next, the server MUST locate the open for the directory to be queried by performing a lookup in the Session.OpenTable, using the FileId.Volatile of the request as the lookup key. If no open is found, or if Open.DurableFileId is not equal to FileId.Persistent, the server MUST fail the request with STATUS_FILE_CLOSED. Otherwise, the server MUST locate the Request in Connection.RequestList for which Request.MessageId matches the MessageId value in the SMB2 header, and set Request.Open to the Open.
If the server implements the SMB 3.x dialect family and Open.IsReplayEligible is TRUE, the server MUST set Open.IsReplayEligible to FALSE.
If the open is not an open to a directory, the server MUST process the request as follows:
If SMB2_REOPEN is set in the Flags field of the SMB2 QUERY_DIRECTORY request, the request MUST be failed with an implementation-specific error code.<406>
Otherwise, the request MUST be failed with STATUS_INVALID_PARAMETER.
If OutputBufferLength is greater than Connection.MaxTransactSize, the server SHOULD<407> fail the request with STATUS_INVALID_PARAMETER.
If Connection.SupportsMultiCredit is TRUE, the server MUST validate CreditCharge based on OutputBufferLength, as specified in section 3.3.5.2.5. If the validation fails, it MUST fail the request with STATUS_INVALID_PARAMETER.
If Open.GrantedAccess does not include FILE_LIST_DIRECTORY, the operation MUST be failed with STATUS_ACCESS_DENIED.
The information classes supported are specified in [MS-FSCC] section 2.4. The supported classes for the query are:
FileDirectoryInformation
FileFullDirectoryInformation
FileBothDirectoryInformation
FileIdFullDirectoryInformation
FileIdBothDirectoryInformation
FileNamesInformation
FileIdExtdDirectoryInformation
FileId64ExtdDirectoryInformation
FileId64ExtdBothDirectoryInformation
FileIdAllExtdDirectoryInformation
FileIdAllExtdBothDirectoryInformation
If any other information class is specified in the FileInformationClass field of the SMB2 QUERY_DIRECTORY Request, the server MUST fail the operation with STATUS_INVALID_INFO_CLASS. If the information class requested is not supported by the server, the server MUST fail the request with STATUS_NOT_SUPPORTED.
If SMB2_RESTART_SCANS or SMB2_REOPEN is set in the Flags field of the SMB2 QUERY_DIRECTORY Request, the server MUST restart the scan with the search pattern specified, in an implementation-specific manner<408>.
If SMB2_RETURN_SINGLE_ENTRY is set in the Flags field of the request, the server MUST return only a single entry.
The server MUST invoke the query directory procedure from the underlying object store in an implementation-specific manner<409>.
The server MAY<410> choose to support resuming enumerations by index number, if SMB2_INDEX_SPECIFIED is set in the Flags field and an index number is specified in the FileIndex field of the SMB2 QUERY_DIRECTORY Request.
If TreeConnect.Share.DoAccessBasedDirectoryEnumeration is TRUE and the object store supports security, the server MUST also exclude entries for which the user represented by Session.SecurityContext is not granted GENERIC_READ and FILE_LIST_DIRECTORY access.
Otherwise, the server MUST construct an SMB2_QUERY_DIRECTORY Response following the syntax specified in section 2.2.34, with the following values:
OutputBufferOffset MUST be set to the offset, in bytes, from the beginning of the SMB2 header where the enumeration data is being placed, the offset to Buffer[].
OutputBufferLength MUST be set to the length, in bytes, of the result of the enumeration.
The enumeration data MUST be copied into Buffer[].
The response MUST be sent to the client.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_INSUFFICIENT_RESOURCES
STATUS_ACCESS_DENIED
STATUS_FILE_CLOSED
STATUS_NETWORK_NAME_DELETED
STATUS_USER_SESSION_DELETED
STATUS_NETWORK_SESSION_EXPIRED
STATUS_INVALID_PARAMETER
STATUS_INVALID_INFO_CLASS
STATUS_NO_SUCH_FILE
STATUS_CANCELLED
STATUS_NOT_SUPPORTED
STATUS_OBJECT_NAME_INVALID
STATUS_VOLUME_DISMOUNTED
STATUS_INVALID_INFO_CLASS
STATUS_FILE_CORRUPT_ERROR
STATUS_NO_MORE_FILES
```

## Section 3.3.5.19: Receiving an SMB2 CHANGE_NOTIFY Request
**Change type:** Modified

### Old Content
```
When the server receives a request that has an SMB2 header with a Command value equal to SMB2 CHANGE_NOTIFY, message handling proceeds as follows.
The server MUST locate the session, as specified in section 3.3.5.2.9.
The server MUST locate the tree connection, as specified in section 3.3.5.2.11.
Next, the server MUST locate the open on which the client is requesting a change notification by performing a lookup in the Session.OpenTable, using the FileId.Volatile of the request as the lookup key. If no open is found, or if Open.DurableFileId is not equal to FileId.Persistent, the server MUST fail the request with STATUS_FILE_CLOSED. Otherwise, the server MUST locate the Request in Connection.RequestList for which Request.MessageId matches the MessageId value in the SMB2 header, and set Request.Open to the Open.
If Open.IsPersistent is FALSE and Open.IsReplayEligible is TRUE, the server MUST set Open.IsReplayEligible to FALSE.
If OutputBufferLength is greater than Connection.MaxTransactSize, the server SHOULD<408> fail the request with STATUS_INVALID_PARAMETER.
If Connection.SupportsMultiCredit is TRUE, the server MUST validate CreditCharge based on OutputBufferLength, as specified in section 3.3.5.2.5. If the validation fails, it MUST fail the request with STATUS_INVALID_PARAMETER.
If the open is not an open to a directory, the request MUST be failed with STATUS_INVALID_PARAMETER.
If Open.GrantedAccess does not include FILE_LIST_DIRECTORY, the operation MUST be failed with STATUS_ACCESS_DENIED.
Because change notify operations are not guaranteed to complete within a deterministic amount of time, the server SHOULD<409> handle this operation asynchronously as specified in section 3.3.4.2.
If the underlying object store does not support change notifications, the server MUST fail this request with STATUS_NOT_SUPPORTED.
The server MUST register a change notification on the underlying object store for the directory that is specified by Open.LocalOpen, using the completion filter supplied in the CompletionFilter field of the client request.<410> If SMB2_WATCH_TREE is set in the Flags field of the client request, the server MUST request that the change notify monitor all subtrees of the directory that is specified by Open.LocalOpen. The server indicates the maximum amount of notification data that it can accept by passing in the OutputBufferLength that is received from the client. An OutputBufferLength of zero indicates that the client allows the occurrence of an event but the client does not allow the notification data details. A Change notification request processed by the server with invalid bits in the CompletionFilter field MUST ignore the invalid bits and process the valid bits. If there are no valid bits in the CompletionFilter, the request will remain pending until the change notification is canceled or the directory handle is closed.
The server MUST process a change notification request in the object store as specified by the algorithm in section 3.3.1.3.
The server MUST send an SMB2 CHANGE_NOTIFY Response only if a change occurs. An SMB2 CHANGE_NOTIFY Request (section 2.2.35) will result in, at most, one response from the server. The server can choose to aggregate multiple changes into the same response. The server MUST include at least one FILE_NOTIFY_INFORMATION structure if it detects a change.
If the server is unable to copy the results into the buffer of the SMB2 CHANGE_NOTIFY Response, then the server MUST construct the response as described below, with an OutputBufferLength of zero, and set the Status in the SMB2 header to STATUS_NOTIFY_ENUM_DIR.
If the object store returns an error, the server MUST fail the request with the error code received.
If the object store returns success, the server MUST construct an SMB2 CHANGE_NOTIFY Response following the syntax that is specified in section 2.2.36 with the following values:
OutputBufferOffset MUST be set to the offset, in bytes, from the beginning of the SMB2 header where the enumeration data is being placed, the offset to Buffer[].
OutputBufferLength MUST be set to the length, in bytes, of the result of the enumeration. It is valid for length to be 0, indicating a change occurred but it could not be fit within the buffer.
The change data MUST be copied into Buffer[].
The response MUST be sent to the client.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_INSUFFICIENT_RESOURCES
STATUS_ACCESS_DENIED
STATUS_FILE_CLOSED
STATUS_NETWORK_NAME_DELETED
STATUS_USER_SESSION_DELETED
STATUS_NETWORK_SESSION_EXPIRED
STATUS_CANCELLED
STATUS_INVALID_PARAMETER
STATUS_NOTIFY_ENUM_DIR
```

### New Content
```
When the server receives a request that has an SMB2 header with a Command value equal to SMB2 CHANGE_NOTIFY, message handling proceeds as follows.
The server MUST locate the session, as specified in section 3.3.5.2.9.
The server MUST locate the tree connection, as specified in section 3.3.5.2.11.
Next, the server MUST locate the open on which the client is requesting a change notification by performing a lookup in the Session.OpenTable, using the FileId.Volatile of the request as the lookup key. If no open is found, or if Open.DurableFileId is not equal to FileId.Persistent, the server MUST fail the request with STATUS_FILE_CLOSED. Otherwise, the server MUST locate the Request in Connection.RequestList for which Request.MessageId matches the MessageId value in the SMB2 header, and set Request.Open to the Open.
If the server implements the SMB 3.x dialect family and Open.IsReplayEligible is TRUE, the server MUST set Open.IsReplayEligible to FALSE.
If OutputBufferLength is greater than Connection.MaxTransactSize, the server SHOULD<411> fail the request with STATUS_INVALID_PARAMETER.
If Connection.SupportsMultiCredit is TRUE, the server MUST validate CreditCharge based on OutputBufferLength, as specified in section 3.3.5.2.5. If the validation fails, it MUST fail the request with STATUS_INVALID_PARAMETER.
If the open is not an open to a directory, the request MUST be failed with STATUS_INVALID_PARAMETER.
If Open.GrantedAccess does not include FILE_LIST_DIRECTORY, the operation MUST be failed with STATUS_ACCESS_DENIED.
Because change notify operations are not guaranteed to complete within a deterministic amount of time, the server SHOULD<412> handle this operation asynchronously as specified in section 3.3.4.2.
If the underlying object store does not support change notifications, the server MUST fail this request with STATUS_NOT_SUPPORTED.
The server MUST register a change notification on the underlying object store for the directory that is specified by Open.LocalOpen, using the completion filter supplied in the CompletionFilter field of the client request.<413> If SMB2_WATCH_TREE is set in the Flags field of the client request, the server MUST request that the change notify monitor all subtrees of the directory that is specified by Open.LocalOpen. The server indicates the maximum amount of notification data that it can accept by passing in the OutputBufferLength that is received from the client. An OutputBufferLength of zero indicates that the client allows the occurrence of an event but the client does not allow the notification data details. A Change notification request processed by the server with invalid bits in the CompletionFilter field MUST ignore the invalid bits and process the valid bits. If there are no valid bits in the CompletionFilter, the request will remain pending until the change notification is canceled or the directory handle is closed.
The server MUST process a change notification request in the object store as specified by the algorithm in section 3.3.1.3.
The server MUST send an SMB2 CHANGE_NOTIFY Response only if a change occurs. An SMB2 CHANGE_NOTIFY Request (section 2.2.35) will result in, at most, one response from the server. The server can choose to aggregate multiple changes into the same response. The server MUST include at least one FILE_NOTIFY_INFORMATION structure if it detects a change.
If the server is unable to copy the results into the buffer of the SMB2 CHANGE_NOTIFY Response, then the server MUST construct the response as described below, with an OutputBufferLength of zero, and set the Status in the SMB2 header to STATUS_NOTIFY_ENUM_DIR.
If the object store returns an error, the server MUST fail the request with the error code received.
If the object store returns success, the server MUST construct an SMB2 CHANGE_NOTIFY Response following the syntax that is specified in section 2.2.36 with the following values:
OutputBufferOffset MUST be set to the offset, in bytes, from the beginning of the SMB2 header where the enumeration data is being placed, the offset to Buffer[].
OutputBufferLength MUST be set to the length, in bytes, of the result of the enumeration. It is valid for length to be 0, indicating a change occurred but it could not be fit within the buffer.
The change data MUST be copied into Buffer[].
The response MUST be sent to the client.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_SUCCESS
STATUS_INSUFFICIENT_RESOURCES
STATUS_ACCESS_DENIED
STATUS_FILE_CLOSED
STATUS_NETWORK_NAME_DELETED
STATUS_USER_SESSION_DELETED
STATUS_NETWORK_SESSION_EXPIRED
STATUS_CANCELLED
STATUS_INVALID_PARAMETER
STATUS_NOTIFY_ENUM_DIR
```

## Section 3.3.5.20.1: Handling SMB2_0_INFO_FILE
**Change type:** Modified

### Old Content
```
The information classes that are supported for querying files are listed in section 2.2.37. Documentation for these is provided in [MS-FSCC] section 2.4.
Requests for information classes that are not listed in section 2.2.37 but which are documented in section 2.4 of [MS-FSCC] SHOULD<412> be failed with STATUS_NOT_SUPPORTED.
Requests for information classes not documented in [MS-FSCC] section 2.4 SHOULD<413> be failed with STATUS_INVALID_INFO_CLASS.
If the server does not implement the SMB 3.x dialect family and the request is for the FileIdInformation information class, the server MUST fail the request with STATUS_NOT_SUPPORTED.
For FileNormalizedNameInformation information class requests, if not supported by the server implementation<414>, or if Connection.Dialect is "2.0.2", "2.1" or "3.0.2", the server MUST fail the request with STATUS_NOT_SUPPORTED.
If the request is for the FilePositionInformation information class, the SMB2 server SHOULD<415> set the CurrentByteOffset field to zero. The CurrentByteOffset field is part of the FILE_POSITION_INFORMATION structure specified in section 2.4.40 of [MS-FSCC].
If the object store supports security and the information class is FileBasicInformation, FileAllInformation, FilePipeInformation, FilePipeLocalInformation, FilePipeRemoteInformation, FileNetworkOpenInformation, or FileAttributeTagInformation, and Open.GrantedAccess does not include FILE_READ_ATTRIBUTES, the server MUST fail the request with STATUS_ACCESS_DENIED.
If the object store supports security and the information class is FileFullEaInformation and Open.GrantedAccess does not include FILE_READ_EA, the server MUST fail the request with STATUS_ACCESS_DENIED.
The server MUST query the information requested from the underlying object store.<416>
If the information class is FileAllInformation, the server SHOULD<417> return an empty FileNameInformation by setting FileNameLength field to zero and FileName field to an empty string. If the store does not support the data requested, the server MUST fail the request with STATUS_NOT_SUPPORTED.
If the information class is FileNormalizedNameInformation, the server MUST convert the information returned from the underlying object store to a normalized path name, as defined in [MS-FSCC] section 2.1.5, in an implementation-specific manner. If the normalized path name is not relative to TreeConnect.Share.LocalPath, the server MUST fail the request with STATUS_NOT_SUPPORTED. Otherwise, the server MUST return the normalized path name.
Depending on the information class, the output data consists of a fixed portion followed by optional variable-length data. If the OutputBufferLength given in the client request is zero or is insufficient to hold the fixed-length part of the information requested, the server MUST fail the request with STATUS_INFO_LENGTH_MISMATCH and MUST return error data as specified in section 2.2.2 with ByteCount set to 8, ErrorDataLength set to 0, and ErrorId set to 0 if Connection.Dialect is "3.1.1"; otherwise, ByteCount set to zero.
If the underlying object store returns an error, the server MUST fail the request with the error code received.
If the underlying object store returns only a portion of the variable-length data, the server MUST construct a response as described below but set the Status field in the SMB2 header to STATUS_BUFFER_OVERFLOW. If FileFullEaInformation is being queried and the requested entries do not fit in the Buffer field of the response, the server MUST construct a response as described below but set the Status field in the SMB2 header to STATUS_BUFFER_OVERFLOW.
If the underlying object store returns the information successfully, the server MUST construct an SMB2 QUERY_INFO Response with the following values:
OutputBufferOffset MUST be set to the offset, in bytes, from the beginning of the SMB2 header to the attribute data at Buffer[].
OutputBufferLength MUST be set to the length of the attribute data being returned to the client.
The data MUST be placed in the response in Buffer[].
The response MUST then be sent to the client.
FullEaList: The list of extended attribute entries maintained by underlying object store.
EaIndex: Index of the EA in FullEaList to start enumerating EA entries. It starts from 1.
EaList: The list of FILE_GET_EA_INFORMATION structures as specified in [MS-FSCC] section 2.4.16.1.
If the object store supports security and the information class is set to FileFullEaInformation, the server MUST return one or more extended attribute entries associated with the current Open, as follows:
If EaList is specified by the client, the server MUST query the EA entries from FullEaList through the EA names in EaList until the buffer is full or has run to the end of EaList. The EaList is contained at the offset InputBufferOffset, starting from the SMB2 header with the length set to InputBufferLength.
If SL_INDEX_SPECIFIED is not set in the Flags field and EaList is not specified, the server MUST enumerate the EA entries from FullEaList starting at Open.CurrentEaIndex until the buffer is full or has run out of the EA entries in FullEaList. Open.CurrentEaIndex MUST be incremented by the number of EA entries returned to the client.
If SL_RESTART_SCAN is set in the Flags field, the server MUST ignore it if either SL_INDEX_SPECIFIED is set in the Flags field or EaList is specified by the client. Otherwise, the server MUST set Open.CurrentEaIndex to 1.
If SL_INDEX_SPECIFIED is set in the Flags field, it SHOULD be ignored by the server if EaList is specified by the client. Otherwise, the server MUST use EaIndex as the starting index in FullEaList to enumerate the EA entries until the buffer is full or has run out of the EA entries in FullEaList. If an out-of-range EaIndex is specified, the server MUST fail the request with STATUS_NONEXISTENT_EA_ENTRY.
If SL_RETURN_SINGLE_ENTRY is set in the Flags field, the server MUST return the single EA entry to the client.
```

### New Content
```
The information classes that are supported for querying files are listed in section 2.2.37. Documentation for these is provided in [MS-FSCC] section 2.4.
Requests for information classes that are not listed in section 2.2.37 but which are documented in section 2.4 of [MS-FSCC] SHOULD<415> be failed with STATUS_NOT_SUPPORTED.
Requests for information classes not documented in [MS-FSCC] section 2.4 SHOULD<416> be failed with STATUS_INVALID_INFO_CLASS.
If the server does not implement the SMB 3.x dialect family and the request is for the FileIdInformation information class, the server MUST fail the request with STATUS_NOT_SUPPORTED.
For FileNormalizedNameInformation information class requests, if not supported by the server implementation<417>, or if Connection.Dialect is "2.0.2", "2.1" or "3.0.2", the server MUST fail the request with STATUS_NOT_SUPPORTED.
If the request is for the FilePositionInformation information class, the SMB2 server SHOULD<418> set the CurrentByteOffset field to zero. The CurrentByteOffset field is part of the FILE_POSITION_INFORMATION structure specified in section 2.4.40 of [MS-FSCC].
If the object store supports security and the information class is FileBasicInformation, FileAllInformation, FilePipeInformation, FilePipeLocalInformation, FilePipeRemoteInformation, FileNetworkOpenInformation, or FileAttributeTagInformation, and Open.GrantedAccess does not include FILE_READ_ATTRIBUTES, the server MUST fail the request with STATUS_ACCESS_DENIED.
If the object store supports security and the information class is FileFullEaInformation and Open.GrantedAccess does not include FILE_READ_EA, the server MUST fail the request with STATUS_ACCESS_DENIED.
The server MUST query the information requested from the underlying object store.<419>
If the information class is FileAllInformation, the server SHOULD<420> return an empty FileNameInformation by setting FileNameLength field to zero and FileName field to an empty string. If the store does not support the data requested, the server MUST fail the request with STATUS_NOT_SUPPORTED.
If the information class is FileNormalizedNameInformation, the server MUST convert the information returned from the underlying object store to a normalized path name, as defined in [MS-FSCC] section 2.1.5, in an implementation-specific manner. If the normalized path name is not relative to TreeConnect.Share.LocalPath, the server MUST fail the request with STATUS_NOT_SUPPORTED. Otherwise, the server MUST return the normalized path name.
Depending on the information class, the output data consists of a fixed portion followed by optional variable-length data. If the OutputBufferLength given in the client request is zero or is insufficient to hold the fixed-length part of the information requested, the server MUST fail the request with STATUS_INFO_LENGTH_MISMATCH and MUST return error data as specified in section 2.2.2 with ByteCount set to 8, ErrorDataLength set to 0, and ErrorId set to 0 if Connection.Dialect is "3.1.1"; otherwise, ByteCount set to zero.
If the underlying object store returns an error, the server MUST fail the request with the error code received.
If the underlying object store returns only a portion of the variable-length data, the server MUST construct a response as described below but set the Status field in the SMB2 header to STATUS_BUFFER_OVERFLOW. If FileFullEaInformation is being queried and the requested entries do not fit in the Buffer field of the response, the server MUST construct a response as described below but set the Status field in the SMB2 header to STATUS_BUFFER_OVERFLOW.
If the underlying object store returns the information successfully, the server MUST construct an SMB2 QUERY_INFO Response with the following values:
OutputBufferOffset MUST be set to the offset, in bytes, from the beginning of the SMB2 header to the attribute data at Buffer[].
OutputBufferLength MUST be set to the length of the attribute data being returned to the client.
The data MUST be placed in the response in Buffer[].
The response MUST then be sent to the client.
FullEaList: The list of extended attribute entries maintained by underlying object store.
EaIndex: Index of the EA in FullEaList to start enumerating EA entries. It starts from 1.
EaList: The list of FILE_GET_EA_INFORMATION structures as specified in [MS-FSCC] section 2.4.16.1.
If the object store supports security and the information class is set to FileFullEaInformation, the server MUST return one or more extended attribute entries associated with the current Open, as follows:
If EaList is specified by the client, the server MUST query the EA entries from FullEaList through the EA names in EaList until the buffer is full or has run to the end of EaList. The EaList is contained at the offset InputBufferOffset, starting from the SMB2 header with the length set to InputBufferLength.
If SL_INDEX_SPECIFIED is not set in the Flags field and EaList is not specified, the server MUST enumerate the EA entries from FullEaList starting at Open.CurrentEaIndex until the buffer is full or has run out of the EA entries in FullEaList. Open.CurrentEaIndex MUST be incremented by the number of EA entries returned to the client.
If SL_RESTART_SCAN is set in the Flags field, the server MUST ignore it if either SL_INDEX_SPECIFIED is set in the Flags field or EaList is specified by the client. Otherwise, the server MUST set Open.CurrentEaIndex to 1.
If SL_INDEX_SPECIFIED is set in the Flags field, it SHOULD be ignored by the server if EaList is specified by the client. Otherwise, the server MUST use EaIndex as the starting index in FullEaList to enumerate the EA entries until the buffer is full or has run out of the EA entries in FullEaList. If an out-of-range EaIndex is specified, the server MUST fail the request with STATUS_NONEXISTENT_EA_ENTRY.
If SL_RETURN_SINGLE_ENTRY is set in the Flags field, the server MUST return the single EA entry to the client.
```

## Section 3.3.5.20.2: Handling SMB2_0_INFO_FILESYSTEM
**Change type:** Modified

### Old Content
```
The information classes that are supported for querying file systems are listed in section 2.2.37. Documentation for these is provided in [MS-FSCC] section 2.5.
Requests for information classes not listed in section 2.2.37 but documented in [MS-FSCC] section 2.5 SHOULD be failed with STATUS_NOT_SUPPORTED.
Requests for information classes not documented in [MS-FSCC] section 2.5 SHOULD be failed with STATUS_INVALID_INFO_CLASS.
The server MUST query the information requested from the underlying volume that hosts the open in the object store.<418> If the store does not support the data requested, the server MUST fail the request with STATUS_NOT_SUPPORTED.
Depending on the information class, the output data consists of a fixed portion followed by optional variable-length data. If the OutputBufferLength given in the client request is either zero or is insufficient to hold the fixed length part of the information requested, the server MUST fail the request with STATUS_INFO_LENGTH_MISMATCH and MUST return error data, as specified in section 2.2.2 with ByteCount set to 8, ErrorDataLength set to 0, and ErrorId set to 0 if Connection.Dialect is "3.1.1"; otherwise, ByteCount set to zero.
If the underlying object store returns an error, the server MUST fail the request with the error code received.
If the underlying object store returns only a portion of the variable-length data, the server MUST construct a success response as described below but set the Status in the SMB2 header to STATUS_BUFFER_OVERFLOW.
If the underlying object store returns the information successfully, the server MUST construct an SMB2 QUERY_INFO Response with the following values:
OutputBufferOffset MUST be set to the offset, in bytes, from the beginning of the SMB2 header to the attribute data at Buffer[].
OutputBufferLength MUST be set to the length of the attribute data being returned to the client.
The data MUST be placed in the response in Buffer[]. If FileInfoClass is FileFsAttributeInformation, the server SHOULD<419> clear the bits FILE_SUPPORTS_USN_JOURNAL, FILE_SUPPORTS_OPEN_BY_FILE_ID, FILE_SUPPORTS_TRANSACTIONS, FILE_RETURNS_CLEANUP_RESULT_INFO, FILE_SUPPORTS_POSIX_UNLINK_RENAME in FileSystemAttributes field, specified in FileFsAttributeInformation structure in [MS-FSCC] section 2.5.1, in Buffer[].
The response MUST then be sent to the client.<420>
```

### New Content
```
The information classes that are supported for querying file systems are listed in section 2.2.37. Documentation for these is provided in [MS-FSCC] section 2.5.
Requests for information classes not listed in section 2.2.37 but documented in [MS-FSCC] section 2.5 SHOULD be failed with STATUS_NOT_SUPPORTED.
Requests for information classes not documented in [MS-FSCC] section 2.5 SHOULD be failed with STATUS_INVALID_INFO_CLASS.
The server MUST query the information requested from the underlying volume that hosts the open in the object store.<421> If the store does not support the data requested, the server MUST fail the request with STATUS_NOT_SUPPORTED.
Depending on the information class, the output data consists of a fixed portion followed by optional variable-length data. If the OutputBufferLength given in the client request is either zero or is insufficient to hold the fixed length part of the information requested, the server MUST fail the request with STATUS_INFO_LENGTH_MISMATCH and MUST return error data, as specified in section 2.2.2 with ByteCount set to 8, ErrorDataLength set to 0, and ErrorId set to 0 if Connection.Dialect is "3.1.1"; otherwise, ByteCount set to zero.
If the underlying object store returns an error, the server MUST fail the request with the error code received.
If the underlying object store returns only a portion of the variable-length data, the server MUST construct a success response as described below but set the Status in the SMB2 header to STATUS_BUFFER_OVERFLOW.
If the underlying object store returns the information successfully, the server MUST construct an SMB2 QUERY_INFO Response with the following values:
OutputBufferOffset MUST be set to the offset, in bytes, from the beginning of the SMB2 header to the attribute data at Buffer[].
OutputBufferLength MUST be set to the length of the attribute data being returned to the client.
The data MUST be placed in the response in Buffer[]. If FileInfoClass is FileFsAttributeInformation, the server SHOULD<422> clear the bits FILE_SUPPORTS_USN_JOURNAL, FILE_SUPPORTS_OPEN_BY_FILE_ID, FILE_SUPPORTS_TRANSACTIONS, FILE_RETURNS_CLEANUP_RESULT_INFO, FILE_SUPPORTS_POSIX_UNLINK_RENAME in FileSystemAttributes field, specified in FileFsAttributeInformation structure in [MS-FSCC] section 2.5.1, in Buffer[].
The response MUST then be sent to the client.<423>
```

## Section 3.3.5.20.3: Handling SMB2_0_INFO_SECURITY
**Change type:** Modified

### Old Content
```
This section assumes knowledge about security concepts, as described in [MS-WPO] section 9 and specified in [MS-DTYP].
The server MUST ignore any flag value in the AdditionalInformation field that is not specified in section 2.2.37.
The server SHOULD<421> call into the underlying object store to query the security descriptor for the object.
The fields required in the resulting security descriptor are denoted by the flags given in the AdditionalInformation field of the request.
If the OutputBufferLength given in the client request is either zero or is insufficient to hold the information requested, the server MUST fail the request with STATUS_BUFFER_TOO_SMALL. If Connection.Dialect is "3.1.1", the server MUST return error data containing the buffer size, in bytes, that would be required to return the requested information, as specified in section 2.2.2, with ByteCount set to 12, ErrorContextCount set to 1, and ErrorData set to SMB2 ERROR Context response with ErrorDataLength set to 4, ErrorId set to 0, and ErrorContextData is set to the buffer size, in bytes, indicating the minimum required buffer length; otherwise, the server MUST return error data with ByteCount set to 4 and ErrorData set to a 4-byte value indicating the minimum required buffer length. The server MUST NOT return STATUS_BUFFER_OVERFLOW with an incomplete security descriptor to the client as in the previous cases. If the underlying object store returns an error, the server MUST fail the request with the error code received.
If the underlying object store returns the information successfully, the server MUST construct an SMB2 QUERY_INFO Response with the following values:
OutputBufferOffset MUST be set to the offset, in bytes, from the beginning of the SMB2 header to the attribute data at Buffer[].
OutputBufferLength MUST be set to the length of the attribute data being returned to the client.
The security descriptor MUST be placed in the response in Buffer[].
The response MUST then be sent to the client.
```

### New Content
```
This section assumes knowledge about security concepts, as described in [MS-WPO] section 9 and specified in [MS-DTYP].
The server MUST ignore any flag value in the AdditionalInformation field that is not specified in section 2.2.37.
The server SHOULD<424> call into the underlying object store to query the security descriptor for the object.
The fields required in the resulting security descriptor are denoted by the flags given in the AdditionalInformation field of the request.
If the OutputBufferLength given in the client request is either zero or is insufficient to hold the information requested, the server MUST fail the request with STATUS_BUFFER_TOO_SMALL. If Connection.Dialect is "3.1.1", the server MUST return error data containing the buffer size, in bytes, that would be required to return the requested information, as specified in section 2.2.2, with ByteCount set to 12, ErrorContextCount set to 1, and ErrorData set to SMB2 ERROR Context response with ErrorDataLength set to 4, ErrorId set to 0, and ErrorContextData is set to the buffer size, in bytes, indicating the minimum required buffer length; otherwise, the server MUST return error data with ByteCount set to 4 and ErrorData set to a 4-byte value indicating the minimum required buffer length. The server MUST NOT return STATUS_BUFFER_OVERFLOW with an incomplete security descriptor to the client as in the previous cases. If the underlying object store returns an error, the server MUST fail the request with the error code received.
If the underlying object store returns the information successfully, the server MUST construct an SMB2 QUERY_INFO Response with the following values:
OutputBufferOffset MUST be set to the offset, in bytes, from the beginning of the SMB2 header to the attribute data at Buffer[].
OutputBufferLength MUST be set to the length of the attribute data being returned to the client.
The security descriptor MUST be placed in the response in Buffer[].
The response MUST then be sent to the client.
```

## Section 3.3.5.20.4: Handling SMB2_0_INFO_QUOTA
**Change type:** Modified

### Old Content
```
The server's object store MAY support quotas that are associated with a security principal. If the server exposes support for quotas, it MUST allow security principals to be identified using security identifiers (SIDs) in the format that is specified in [MS-DTYP] section 2.4.2.2.<422>
If the underlying object store does not support user quotas, the server MUST fail the request with STATUS_NOT_SUPPORTED.
The server MUST verify that the InputBufferOffset and InputBufferLength of the client request describe an SMB2_QUERY_QUOTA_INFO structure following the syntax specified in section 2.2.37.1. If not, the server MUST fail the request with STATUS_INVALID_PARAMETER.
The server MUST query the quota information retrieved from the underlying volume that hosts the open in the object store.<423>
FullQuotaList:  The list of the volume's quota information entries maintained by the underlying object store.
SidList:  The list of FILE_GET_QUOTA_INFORMATION structures as specified in [MS-FSCC] section 2.4.41.1.
If ReturnSingle is TRUE, the server MUST return at most a single quota information entry to the client.
If SidListLength is nonzero, the server MUST ignore the values of StartSidOffset and StartSidLength, and enumerate the quota information entries for all the SIDs specified in SidList. If SidList is not a list of FILE_GET_QUOTA_INFORMATION structures linked via the NextEntryOffset field, the server MUST fail the request with STATUS_INVALID_PARAMETER. If the server can't find the corresponding quota information entry through the SID specified in the FILE_GET_QUOTA_INFORMATION structure, then the server MUST return FILE_QUOTA_INFORMATION for the SID with the following fields set to zero: ChangeTime, QuotaUsed, QuotaThreshold, and QuotaLimit.
If SidListLength is zero, SidBuffer.StartSid is nonzero and StartSidLength is nonzero, the server SHOULD enumerate the quota information entries for the SIDs following the StartSid.
If StartSidLength or StartSidOffset or SidListLength are nonzero, the server MUST ignore the value of RestartScan.
If StartSidLength and StartSidOffset and SidListLength are all zero, the server MUST check the value of RestartScan. If RestartScan is TRUE, the server MUST set Open.CurrentQuotaIndex to 1. The server MUST use Open.CurrentQuotaIndex as the starting index in FullQuotaList to enumerate the quota information entries until the buffer is full or has run out of the quota information entries in FullQuotaList. Open.CurrentQuotaIndex MUST be incremented by the number of quota information entries returned to the client.
The server MUST return STATUS_SUCCESS if at least one FILE_QUOTA_INFORMATION entry is returned.
If the OutputBufferLength given in the client request is either zero or is insufficient to hold single FILE_QUOTA_INFORMATION entry, the server MUST fail the request with STATUS_BUFFER_TOO_SMALL and return error data, as specified in section 2.2.2, with ByteCount set to zero.
If the underlying object store returns STATUS_NO_MORE_ENTRIES, indicating that no information was returned, the server MUST set the same error in the Status field of the SMB2 header. The server MUST also construct an SMB2 QUERY_INFO Response with OutputBufferOffset, OutputBufferLength and Buffer set to 0.
If the underlying object store returns any other error, the server MUST fail the entire request with the error code received.
If the underlying object store returns the information successfully, the server MUST construct an SMB2 QUERY_INFO Response with the following values:
OutputBufferOffset MUST be set to the offset, in bytes, from the beginning of the SMB2 header to the attribute data at Buffer[].
OutputBufferLength MUST be set to the length of the attribute data being returned to the client.
The data MUST be placed in the response in Buffer[].
The response MUST then be sent to the client.
```

### New Content
```
The server's object store MAY support quotas that are associated with a security principal. If the server exposes support for quotas, it MUST allow security principals to be identified using security identifiers (SIDs) in the format that is specified in [MS-DTYP] section 2.4.2.2.<425>
If the underlying object store does not support user quotas, the server MUST fail the request with STATUS_NOT_SUPPORTED.
The server MUST verify that the InputBufferOffset and InputBufferLength of the client request describe an SMB2_QUERY_QUOTA_INFO structure following the syntax specified in section 2.2.37.1. If not, the server MUST fail the request with STATUS_INVALID_PARAMETER.
The server MUST query the quota information retrieved from the underlying volume that hosts the open in the object store.<426>
FullQuotaList:  The list of the volume's quota information entries maintained by the underlying object store.
SidList:  The list of FILE_GET_QUOTA_INFORMATION structures as specified in [MS-FSCC] section 2.4.41.1.
If ReturnSingle is TRUE, the server MUST return at most a single quota information entry to the client.
If SidListLength is nonzero, the server MUST ignore the values of StartSidOffset and StartSidLength, and enumerate the quota information entries for all the SIDs specified in SidList. If SidList is not a list of FILE_GET_QUOTA_INFORMATION structures linked via the NextEntryOffset field, the server MUST fail the request with STATUS_INVALID_PARAMETER. If the server can't find the corresponding quota information entry through the SID specified in the FILE_GET_QUOTA_INFORMATION structure, then the server MUST return FILE_QUOTA_INFORMATION for the SID with the following fields set to zero: ChangeTime, QuotaUsed, QuotaThreshold, and QuotaLimit.
If SidListLength is zero, SidBuffer.StartSid is nonzero and StartSidLength is nonzero, the server SHOULD enumerate the quota information entries for the SIDs following the StartSid.
If StartSidLength or StartSidOffset or SidListLength are nonzero, the server MUST ignore the value of RestartScan.
If StartSidLength and StartSidOffset and SidListLength are all zero, the server MUST check the value of RestartScan. If RestartScan is TRUE, the server MUST set Open.CurrentQuotaIndex to 1. The server MUST use Open.CurrentQuotaIndex as the starting index in FullQuotaList to enumerate the quota information entries until the buffer is full or has run out of the quota information entries in FullQuotaList. Open.CurrentQuotaIndex MUST be incremented by the number of quota information entries returned to the client.
The server MUST return STATUS_SUCCESS if at least one FILE_QUOTA_INFORMATION entry is returned.
If the OutputBufferLength given in the client request is either zero or is insufficient to hold single FILE_QUOTA_INFORMATION entry, the server MUST fail the request with STATUS_BUFFER_TOO_SMALL and return error data, as specified in section 2.2.2, with ByteCount set to zero.
If the underlying object store returns STATUS_NO_MORE_ENTRIES, indicating that no information was returned, the server MUST set the same error in the Status field of the SMB2 header. The server MUST also construct an SMB2 QUERY_INFO Response with OutputBufferOffset, OutputBufferLength and Buffer set to 0.
If the underlying object store returns any other error, the server MUST fail the entire request with the error code received.
If the underlying object store returns the information successfully, the server MUST construct an SMB2 QUERY_INFO Response with the following values:
OutputBufferOffset MUST be set to the offset, in bytes, from the beginning of the SMB2 header to the attribute data at Buffer[].
OutputBufferLength MUST be set to the length of the attribute data being returned to the client.
The data MUST be placed in the response in Buffer[].
The response MUST then be sent to the client.
```

## Section 3.3.5.21.1: Handling SMB2_0_INFO_FILE
**Change type:** Modified

### Old Content
```
The information classes that are supported for setting file information are listed in section 2.2.39. Documentation for these is provided in [MS-FSCC] section 2.4.
Requests for information classes documented in [MS-FSCC] section 2.4 with "Set" not specified in the Uses column are not allowed and SHOULD be failed with STATUS_INVALID_INFO_CLASS.
Requests for information classes not documented in section 2.4 of [MS-FSCC] SHOULD<425> be failed with STATUS_INVALID_INFO_CLASS.
Requests for information classes not listed in section 2.2.39 but documented in [MS-FSCC] section 2.4 with "Set" specified in the Uses column are not allowed and SHOULD be failed with STATUS_NOT_SUPPORTED.
If FileInfoClass is FileRenameInformation, the server does the following:
If the size of the buffer is less than the size of FILE_RENAME_INFORMATION_TYPE_2 as specified in [MS-FSCC] section 2.4.42.2, the server MUST fail the request with STATUS_INFO_LENGTH_MISMATCH.
If the file name pointed to by the FileName parameter of the FILE_RENAME_INFORMATION_TYPE_2, as specified in [MS-FSCC] section 2.4.42.2, contains a separator character, then the server MUST fail the request with STATUS_NOT_SUPPORTED.
If the RootDirectory field of FILE_RENAME_INFORMATION_TYPE_2 as specified in [MS-FSCC] section 2.4.42.2 is zero, the FileName field MUST specify a full pathname as specified in [MS-FSCC] section 2.1.5 to be assigned to the file. If the RootDirectory field is not zero, the server MUST return STATUS_INVALID_PARAMETER.
If the object store supports security and FileInfoClass is FileBasicInformation or FilePipeInformation, and Open.GrantedAccess does not include FILE_WRITE_ATTRIBUTES, the server MUST fail the request with STATUS_ACCESS_DENIED.
If the object store supports security and FileInfoClass is FileRenameInformation, FileDispositionInformation, or FileShortNameInformation, and Open.GrantedAccess does not include DELETE, the server MUST fail the request with STATUS_ACCESS_DENIED.
If the object store supports security and FileInfoClass is FileFullEaInformation, and Open.GrantedAccess does not include FILE_WRITE_EA, the server MUST fail the request with STATUS_ACCESS_DENIED.
If the object store supports security and FileInfoClass is FileFullEaInformation and the EA buffer in the Buffer field is not in a valid format, the server MUST fail the request with STATUS_EA_LIST_INCONSISTENT.
If the object store supports security and FileInfoClass is FileAllocationInformation, FileEndOfFileInformation, or FileValidDataLengthInformation, and Open.GrantedAccess does not include FILE_WRITE_DATA, the server MUST fail the request with STATUS_ACCESS_DENIED.
The server MUST apply the information requested to the underlying object store.<426> If the store does not support the information class requested, the server MUST fail the request with STATUS_NOT_SUPPORTED.
If the underlying object store returns an error, the server MUST fail the request with the error code received.
Otherwise, the server MUST initialize an SMB2 SET_INFO Response following the syntax given in section 2.2.40.
If the underlying object store returns successfully, FileInfoClass is FileDispositionInformation, Connection.Dialect is not “2.0.2” , and Open.Lease is not NULL, the server MUST set Open.Lease.FileDeleteOnClose to TRUE.
If the underlying object store returns successfully, FileInfoClass is FileRenameInformation, Connection.Dialect is not “2.0.2”, and Open.Lease is not NULL, the server MUST update Open.Lease.Filename to the new name for the file and Open.Lease.FileDeleteOnClose to FALSE.
The response MUST then be sent to the client.
```

### New Content
```
The information classes that are supported for setting file information are listed in section 2.2.39. Documentation for these is provided in [MS-FSCC] section 2.4.
Requests for information classes documented in [MS-FSCC] section 2.4 with "Set" not specified in the Uses column are not allowed and SHOULD be failed with STATUS_INVALID_INFO_CLASS.
Requests for information classes not documented in section 2.4 of [MS-FSCC] SHOULD<428> be failed with STATUS_INVALID_INFO_CLASS.
Requests for information classes not listed in section 2.2.39 but documented in [MS-FSCC] section 2.4 with "Set" specified in the Uses column are not allowed and SHOULD be failed with STATUS_NOT_SUPPORTED.
If FileInfoClass is FileRenameInformation, the server does the following:
If the size of the buffer is less than the size of FILE_RENAME_INFORMATION_TYPE_2 as specified in [MS-FSCC] section 2.4.42.2, the server MUST fail the request with STATUS_INFO_LENGTH_MISMATCH.
If the file name pointed to by the FileName parameter of the FILE_RENAME_INFORMATION_TYPE_2, as specified in [MS-FSCC] section 2.4.42.2, contains a separator character, then the server MUST fail the request with STATUS_NOT_SUPPORTED.
If the RootDirectory field of FILE_RENAME_INFORMATION_TYPE_2 as specified in [MS-FSCC] section 2.4.42.2 is zero, the FileName field MUST specify a full pathname as specified in [MS-FSCC] section 2.1.5 to be assigned to the file. If the RootDirectory field is not zero, the server MUST return STATUS_INVALID_PARAMETER.
If the object store supports security and FileInfoClass is FileBasicInformation or FilePipeInformation, and Open.GrantedAccess does not include FILE_WRITE_ATTRIBUTES, the server MUST fail the request with STATUS_ACCESS_DENIED.
If the object store supports security and FileInfoClass is FileRenameInformation, FileDispositionInformation, or FileShortNameInformation, and Open.GrantedAccess does not include DELETE, the server MUST fail the request with STATUS_ACCESS_DENIED.
If the object store supports security and FileInfoClass is FileFullEaInformation, and Open.GrantedAccess does not include FILE_WRITE_EA, the server MUST fail the request with STATUS_ACCESS_DENIED.
If the object store supports security and FileInfoClass is FileFullEaInformation and the EA buffer in the Buffer field is not in a valid format, the server MUST fail the request with STATUS_EA_LIST_INCONSISTENT.
If the object store supports security and FileInfoClass is FileAllocationInformation, FileEndOfFileInformation, or FileValidDataLengthInformation, and Open.GrantedAccess does not include FILE_WRITE_DATA, the server MUST fail the request with STATUS_ACCESS_DENIED.
The server MUST apply the information requested to the underlying object store.<429> If the store does not support the information class requested, the server MUST fail the request with STATUS_NOT_SUPPORTED.
If the underlying object store returns an error, the server MUST fail the request with the error code received.
Otherwise, the server MUST initialize an SMB2 SET_INFO Response following the syntax given in section 2.2.40.
If the underlying object store returns successfully, FileInfoClass is FileDispositionInformation, Connection.Dialect is not “2.0.2” , and Open.Lease is not NULL, the server MUST set Open.Lease.FileDeleteOnClose to TRUE.
If the underlying object store returns successfully, FileInfoClass is FileRenameInformation, Connection.Dialect is not “2.0.2”, and Open.Lease is not NULL, the server MUST update Open.Lease.Filename to the new name for the file and Open.Lease.FileDeleteOnClose to FALSE.
The response MUST then be sent to the client.
```

## Section 3.3.5.21.2: Handling SMB2_0_INFO_FILESYSTEM
**Change type:** Modified

### Old Content
```
The information classes that are supported for setting underlying object store information are listed in section 2.2.39. Documentation for these is provided [MS-FSCC] section 2.5. Requests for information classes not listed in section 2.2.39 but documented in section 2.5 of [MS-FSCC] for Uses of "Set" or "LOCAL" MUST be failed with STATUS_NOT_SUPPORTED. Requests for information classes not documented in section 2.5 of [MS-FSCC] or documented in section 2.5 of [MS-FSCC] for Uses of only "Query" MUST be failed with STATUS_INVALID_INFO_CLASS.
If the object store supports security and the information class is FileFsControlInformation or FileFsObjectIdInformation and Open.GrantedAccess does not include FILE_WRITE_DATA, the server MUST fail the request with STATUS_ACCESS_DENIED.
The server MUST apply the information requested to the underlying object store.<427> If the underlying object store returns an error, the server MUST fail the request with the error code received. Otherwise, the server MUST initialize an SMB2 SET_INFO Response following the syntax given in section 2.2.40. The response MUST then be sent to the client.
```

### New Content
```
The information classes that are supported for setting underlying object store information are listed in section 2.2.39. Documentation for these is provided [MS-FSCC] section 2.5. Requests for information classes not listed in section 2.2.39 but documented in section 2.5 of [MS-FSCC] for Uses of "Set" or "LOCAL" MUST be failed with STATUS_NOT_SUPPORTED. Requests for information classes not documented in section 2.5 of [MS-FSCC] or documented in section 2.5 of [MS-FSCC] for Uses of only "Query" MUST be failed with STATUS_INVALID_INFO_CLASS.
If the object store supports security and the information class is FileFsControlInformation or FileFsObjectIdInformation and Open.GrantedAccess does not include FILE_WRITE_DATA, the server MUST fail the request with STATUS_ACCESS_DENIED.
The server MUST apply the information requested to the underlying object store.<430> If the underlying object store returns an error, the server MUST fail the request with the error code received. Otherwise, the server MUST initialize an SMB2 SET_INFO Response following the syntax given in section 2.2.40. The response MUST then be sent to the client.
```

## Section 3.3.5.21.3: Handling SMB2_0_INFO_SECURITY
**Change type:** Modified

### Old Content
```
The following section assumes knowledge about security concepts as described in [MS-WPO] section 9 and specified in [MS-DTYP].<428>
The server MUST ignore any flag value in the AdditionalInformation field that is not specified in section 2.2.39.
If SACL_SECURITY_INFORMATION is set in the AdditionalInformation field of the request, and Open.GrantedAccess does not include ACCESS_SYSTEM_SECURITY, the server MUST fail the request with STATUS_ACCESS_DENIED.
If DACL_SECURITY_INFORMATION is set in the AdditionalInformation field of the request, and Open.GrantedAccess does not include WRITE_DAC, the server MUST fail the request with STATUS_ACCESS_DENIED.
If the object store supports security, either LABEL_SECURITY_INFORMATION, GROUP_SECURITY_INFORMATION, or OWNER_SECURITY_INFORMATION is set in the AdditionalInformation field of the request, and Open.GrantedAccess does not include WRITE_OWNER, the server MUST fail the request with STATUS_ACCESS_DENIED.
If ATTRIBUTE_SECURITY_INFORMATION is set in the AdditionalInformation field of the request, and Open.GrantedAccess does not include WRITE_DAC, the server SHOULD<429> fail the request with STATUS_ACCESS_DENIED.
If SCOPE_SECURITY_INFORMATION is set in the AdditionalInformation field of the request, and Open.GrantedAccess does not include ACCESS_SYSTEM_SECURITY, the server SHOULD<430> fail the request with STATUS_ACCESS_DENIED.
If BACKUP_SECURITY_INFORMATION is set in the AdditionalInformation field of the request, and Open.GrantedAccess does not include WRITE_DAC, WRITE_OWNER and ACCESS_SYSTEM_SECURITY the server SHOULD<431> fail the request with STATUS_ACCESS_DENIED.
The server MUST call into the underlying object store to set the security on the object.<432>
The fields being applied in the provided security descriptor are denoted by the flags given in the AdditionalInformation field of the request.
If the underlying object store returns an error, the server MUST fail the request with the error code received.
Otherwise, the server MUST initialize an SMB2 SET_INFO Response following the syntax given in section 2.2.40.
The response MUST then be sent to the client.
```

### New Content
```
The following section assumes knowledge about security concepts as described in [MS-WPO] section 9 and specified in [MS-DTYP].<431>
The server MUST ignore any flag value in the AdditionalInformation field that is not specified in section 2.2.39.
If SACL_SECURITY_INFORMATION is set in the AdditionalInformation field of the request, and Open.GrantedAccess does not include ACCESS_SYSTEM_SECURITY, the server MUST fail the request with STATUS_ACCESS_DENIED.
If DACL_SECURITY_INFORMATION is set in the AdditionalInformation field of the request, and Open.GrantedAccess does not include WRITE_DAC, the server MUST fail the request with STATUS_ACCESS_DENIED.
If the object store supports security, either LABEL_SECURITY_INFORMATION, GROUP_SECURITY_INFORMATION, or OWNER_SECURITY_INFORMATION is set in the AdditionalInformation field of the request, and Open.GrantedAccess does not include WRITE_OWNER, the server MUST fail the request with STATUS_ACCESS_DENIED.
If ATTRIBUTE_SECURITY_INFORMATION is set in the AdditionalInformation field of the request, and Open.GrantedAccess does not include WRITE_DAC, the server SHOULD<432> fail the request with STATUS_ACCESS_DENIED.
If SCOPE_SECURITY_INFORMATION is set in the AdditionalInformation field of the request, and Open.GrantedAccess does not include ACCESS_SYSTEM_SECURITY, the server SHOULD<433> fail the request with STATUS_ACCESS_DENIED.
If BACKUP_SECURITY_INFORMATION is set in the AdditionalInformation field of the request, and Open.GrantedAccess does not include WRITE_DAC, WRITE_OWNER and ACCESS_SYSTEM_SECURITY the server SHOULD<434> fail the request with STATUS_ACCESS_DENIED.
The server MUST call into the underlying object store to set the security on the object.<435>
The fields being applied in the provided security descriptor are denoted by the flags given in the AdditionalInformation field of the request.
If the underlying object store returns an error, the server MUST fail the request with the error code received.
Otherwise, the server MUST initialize an SMB2 SET_INFO Response following the syntax given in section 2.2.40.
The response MUST then be sent to the client.
```

## Section 3.3.5.21.4: Handling SMB2_0_INFO_QUOTA
**Change type:** Modified

### Old Content
```
The server's object store MAY support quotas associated with a security principal. If the server exposes support for quotas, it MUST allow security principals to be identified using security identifiers (SIDs) in the format specified in [MS-DTYP] section 2.4.2.2.<433>
If the object store does not support quotas, the server MUST fail the request with STATUS_NOT_SUPPORTED.
If the user represented by Session.SecurityContext is not granted the right to manage quotas on the underlying volume in the object store, the server MUST fail the request with STATUS_ACCESS_DENIED.
The server MUST apply the provided quota information to the underlying volume that hosts the open in the object store.<434>
If the underlying object store returns an error, the server MUST fail the request with the error code received.
Otherwise, the server MUST initialize an SMB2 SET_INFO Response following the syntax given in section 2.2.40.
The response MUST then be sent to the client.
```

### New Content
```
The server's object store MAY support quotas associated with a security principal. If the server exposes support for quotas, it MUST allow security principals to be identified using security identifiers (SIDs) in the format specified in [MS-DTYP] section 2.4.2.2.<436>
If the object store does not support quotas, the server MUST fail the request with STATUS_NOT_SUPPORTED.
If the user represented by Session.SecurityContext is not granted the right to manage quotas on the underlying volume in the object store, the server MUST fail the request with STATUS_ACCESS_DENIED.
The server MUST apply the provided quota information to the underlying volume that hosts the open in the object store.<437>
If the underlying object store returns an error, the server MUST fail the request with the error code received.
Otherwise, the server MUST initialize an SMB2 SET_INFO Response following the syntax given in section 2.2.40.
The response MUST then be sent to the client.
```

## Section 3.3.5.22.1: Processing an Oplock Acknowledgment
**Change type:** Modified

### Old Content
```
The server MUST locate the session, as specified in section 3.3.5.2.9.
The server MUST locate the tree connection, as specified in section 3.3.5.2.11.
Next, the server MUST locate the open on which the client is acknowledging an oplock break by performing a lookup in Session.OpenTable using FileId.Volatile of the request as the lookup key. If no open is found, or if Open.DurableFileId is not equal to FileId.Persistent, the server MUST fail the request with STATUS_FILE_CLOSED. Otherwise, the server MUST locate the Request in Connection.RequestList for which Request.MessageId matches the MessageId value in the SMB2 header, and set Request.Open to the Open.
If Open.IsPersistent is FALSE and Open.IsReplayEligible is TRUE, the server MUST set Open.IsReplayEligible to FALSE.
If Open.OplockState is not Breaking, the server MUST stop processing the acknowledgment, and send an error response with STATUS_INVALID_DEVICE_STATE.
If the OplockLevel in the acknowledgment is SMB2_OPLOCK_LEVEL_LEASE, the server MUST complete the oplock break request received from the object store as described in section 3.3.4.6, with a new level SMB2_OPLOCK_LEVEL_NONE in an implementation-specific manner,<435> and set Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE, and Open.OplockState to None, send an error response with STATUS_INVALID_PARAMETER and stop processing.
If any of the following conditions is TRUE, the server MUST complete the oplock break request received from the object store, as described in section 3.3.4.6, with a new level SMB2_OPLOCK_LEVEL_NONE in an implementation-specific manner<436>, set Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE and Open.OplockState to None, send an error response with STATUS_INVALID_OPLOCK_PROTOCOL, and stop processing:
If Open.OplockLevel is SMB2_OPLOCK_LEVEL_EXCLUSIVE, and if OplockLevel is not SMB2_OPLOCK_LEVEL_II or SMB2_OPLOCK_LEVEL_NONE.
If Open.OplockLevel is SMB2_OPLOCK_LEVEL_BATCH and if OplockLevel is not SMB2_OPLOCK_LEVEL_II, or SMB2_OPLOCK_LEVEL_NONE, or SMB2_OPLOCK_LEVEL_EXCLUSIVE.
If Open.OplockLevel is SMB2_OPLOCK_LEVEL_II, and OplockLevel is not SMB2_OPLOCK_LEVEL_NONE.
If OplockLevel is SMB2_OPLOCK_LEVEL_EXCLUSIVE, the server MUST complete the oplock break request received from the object store as described in section 3.3.4.6, with a new level SMB2_OPLOCK_LEVEL_NONE in an implementation-specific manner.<437>
If OplockLevel is SMB2_OPLOCK_LEVEL_II or SMB2_OPLOCK_LEVEL_NONE, the server MUST complete the oplock break request received from the object store as described in section 3.3.4.6, with a new level received in OplockLevel in an implementation-specific manner.<438>
If the object store indicates an error, the server MUST set the Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE, the Open.OplockState to None, send the error response with the error code received, and stop processing.
If the object store indicates success, the server MUST update Open.OplockLevel and Open.OplockState as follows:
If OplockLevel is SMB2_OPLOCK_LEVEL_EXCLUSIVE, set Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE and Open.OplockState to None.
If OplockLevel is SMB2_OPLOCK_LEVEL_II, set Open.OplockLevel to SMB2_OPLOCK_LEVEL_II and Open.OplockState to Held.
If OplockLevel is SMB2_OPLOCK_LEVEL_NONE, set Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE and the Open.OplockState to None.
The server then MUST construct an oplock break response using the syntax specified in section 2.2.25.1 with the following value:
OplockLevel MUST be set to Open.OplockLevel.
This response MUST then be sent to the client.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_ACCESS_DENIED
STATUS_FILE_CLOSED
STATUS_INVALID_OPLOCK_PROTOCOL
STATUS_INVALID_PARAMETER
STATUS_INVALID_DEVICE_STATE
STATUS_NETWORK_NAME_DELETED
STATUS_USER_SESSION_DELETED
```

### New Content
```
The server MUST locate the session, as specified in section 3.3.5.2.9.
The server MUST locate the tree connection, as specified in section 3.3.5.2.11.
Next, the server MUST locate the open on which the client is acknowledging an oplock break by performing a lookup in Session.OpenTable using FileId.Volatile of the request as the lookup key. If no open is found, or if Open.DurableFileId is not equal to FileId.Persistent, the server MUST fail the request with STATUS_FILE_CLOSED. Otherwise, the server MUST locate the Request in Connection.RequestList for which Request.MessageId matches the MessageId value in the SMB2 header, and set Request.Open to the Open.
If the server implements the SMB 3.x dialect family and Open.IsReplayEligible is TRUE, the server MUST set Open.IsReplayEligible to FALSE.
If Open.OplockState is not Breaking, the server MUST stop processing the acknowledgment, and send an error response with STATUS_INVALID_DEVICE_STATE.
If the OplockLevel in the acknowledgment is SMB2_OPLOCK_LEVEL_LEASE, the server MUST complete the oplock break request received from the object store as described in section 3.3.4.6, with a new level SMB2_OPLOCK_LEVEL_NONE in an implementation-specific manner,<438> and set Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE, and Open.OplockState to None, send an error response with STATUS_INVALID_PARAMETER and stop processing.
If any of the following conditions is TRUE, the server MUST complete the oplock break request received from the object store, as described in section 3.3.4.6, with a new level SMB2_OPLOCK_LEVEL_NONE in an implementation-specific manner<439>, set Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE and Open.OplockState to None, send an error response with STATUS_INVALID_OPLOCK_PROTOCOL, and stop processing:
If Open.OplockLevel is SMB2_OPLOCK_LEVEL_EXCLUSIVE, and if OplockLevel is not SMB2_OPLOCK_LEVEL_II or SMB2_OPLOCK_LEVEL_NONE.
If Open.OplockLevel is SMB2_OPLOCK_LEVEL_BATCH and if OplockLevel is not SMB2_OPLOCK_LEVEL_II, or SMB2_OPLOCK_LEVEL_NONE, or SMB2_OPLOCK_LEVEL_EXCLUSIVE.
If Open.OplockLevel is SMB2_OPLOCK_LEVEL_II, and OplockLevel is not SMB2_OPLOCK_LEVEL_NONE.
If OplockLevel is SMB2_OPLOCK_LEVEL_EXCLUSIVE, the server MUST complete the oplock break request received from the object store as described in section 3.3.4.6, with a new level SMB2_OPLOCK_LEVEL_NONE in an implementation-specific manner.<440>
If OplockLevel is SMB2_OPLOCK_LEVEL_II or SMB2_OPLOCK_LEVEL_NONE, the server MUST complete the oplock break request received from the object store as described in section 3.3.4.6, with a new level received in OplockLevel in an implementation-specific manner.<441>
If the object store indicates an error, the server MUST set the Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE, the Open.OplockState to None, send the error response with the error code received, and stop processing.
If the object store indicates success, the server MUST update Open.OplockLevel and Open.OplockState as follows:
If OplockLevel is SMB2_OPLOCK_LEVEL_EXCLUSIVE, set Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE and Open.OplockState to None.
If OplockLevel is SMB2_OPLOCK_LEVEL_II, set Open.OplockLevel to SMB2_OPLOCK_LEVEL_II and Open.OplockState to Held.
If OplockLevel is SMB2_OPLOCK_LEVEL_NONE, set Open.OplockLevel to SMB2_OPLOCK_LEVEL_NONE and the Open.OplockState to None.
The server then MUST construct an oplock break response using the syntax specified in section 2.2.25.1 with the following value:
OplockLevel MUST be set to Open.OplockLevel.
This response MUST then be sent to the client.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_ACCESS_DENIED
STATUS_FILE_CLOSED
STATUS_INVALID_OPLOCK_PROTOCOL
STATUS_INVALID_PARAMETER
STATUS_INVALID_DEVICE_STATE
STATUS_NETWORK_NAME_DELETED
STATUS_USER_SESSION_DELETED
```

## Section 3.3.5.22.2: Processing a Lease Acknowledgment
**Change type:** Modified

### Old Content
```
The server MUST locate the session, as specified in section 3.3.5.2.9.
The server MUST locate the tree connection, as specified in section 3.3.5.2.11.
Next, the server MUST locate the Lease Table by performing a lookup in GlobalLeaseTableList using Connection.ClientGuid as the lookup key. If no lease table is found, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
The server MUST locate the lease on which the client is acknowledging a lease break by performing a lookup in LeaseTable.LeaseList using the LeaseKey of the request as the lookup key. If no lease is found, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
If there is an Open in Lease.LeaseOpens where Open.IsPersistent is FALSE and Open.IsReplayEligible is TRUE, the server MUST set Open.IsReplayEligible to FALSE.
If Lease.Breaking is FALSE, the server MUST fail the request with STATUS_UNSUCCESSFUL.
If LeaseState is not a subset of Lease.BreakToLeaseState, the server MUST fail the request with STATUS_REQUEST_NOT_ACCEPTED.
The server completes the lease break request received from the object store as described in section 3.3.4.7. The server MUST set Lease.LeaseState to LeaseState received in the request, Open.OplockState to “Held”, and Lease.Breaking to FALSE.
The server then MUST construct a lease break response using the syntax specified in section 2.2.25.2 with the following values:
LeaseKey MUST be set to Lease.LeaseKey.
LeaseState MUST be set to Lease.LeaseState.
This response MUST then be sent to the client.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_ACCESS_DENIED
STATUS_OBJECT_NAME_NOT_FOUND
STATUS_INVALID_OPLOCK_PROTOCOL
STATUS_INVALID_PARAMETER
STATUS_INVALID_DEVICE_STATE
STATUS_NETWORK_NAME_DELETED
STATUS_USER_SESSION_DELETED
```

### New Content
```
The server MUST locate the session, as specified in section 3.3.5.2.9.
The server MUST locate the tree connection, as specified in section 3.3.5.2.11.
Next, the server MUST locate the Lease Table by performing a lookup in GlobalLeaseTableList using Connection.ClientGuid as the lookup key. If no lease table is found, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
The server MUST locate the lease on which the client is acknowledging a lease break by performing a lookup in LeaseTable.LeaseList using the LeaseKey of the request as the lookup key. If no lease is found, the server MUST fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
If the server implements the SMB 3.x dialect family and there is an Open in Lease.LeaseOpens where Open.IsReplayEligible is TRUE, the server MUST set Open.IsReplayEligible to FALSE.
If Lease.Breaking is FALSE, the server MUST fail the request with STATUS_UNSUCCESSFUL.
If LeaseState is not a subset of Lease.BreakToLeaseState, the server MUST fail the request with STATUS_REQUEST_NOT_ACCEPTED.
The server completes the lease break request received from the object store as described in section 3.3.4.7. The server MUST set Lease.LeaseState to LeaseState received in the request, Open.OplockState to “Held”, and Lease.Breaking to FALSE.
The server then MUST construct a lease break response using the syntax specified in section 2.2.25.2 with the following values:
LeaseKey MUST be set to Lease.LeaseKey.
LeaseState MUST be set to Lease.LeaseState.
This response MUST then be sent to the client.
The status code returned by this operation MUST be one of those defined in [MS-ERREF]. Common status codes returned by this operation include:
STATUS_ACCESS_DENIED
STATUS_OBJECT_NAME_NOT_FOUND
STATUS_INVALID_OPLOCK_PROTOCOL
STATUS_INVALID_PARAMETER
STATUS_INVALID_DEVICE_STATE
STATUS_NETWORK_NAME_DELETED
STATUS_USER_SESSION_DELETED
```

## Section 3.3.6.3: Session Expiration Timer Event
**Change type:** Modified

### Old Content
```
When the session expiration timer expires, the server MUST walk each Session in the GlobalSessionTable. If the Session.State is Valid and the Session.ExpirationTime has passed, the Session.State MUST be set to Expired and ServerStatistics.sts0_stimedout MUST be increased by 1. For each Connection in the global ConnectionList where the current time minus Connection.CreationTime is more than an implementation-specific time-out,<439> the server MUST disconnect the Connection, as specified in section 3.3.7.1, if any of the following conditions are TRUE:
Connection.Dialect is "Unknown".
Connection.Dialect is not "Unknown", and Connection.SessionTable is empty.
Connection.Dialect is not "Unknown", Connection.SessionTable is not empty, and there is no Session in Connection.SessionList where Session.State is Valid or Expired.
```

### New Content
```
When the session expiration timer expires, the server MUST walk each Session in the GlobalSessionTable. If the Session.State is Valid and the Session.ExpirationTime has passed, the Session.State MUST be set to Expired and ServerStatistics.sts0_stimedout MUST be increased by 1. For each Connection in the global ConnectionList where the current time minus Connection.CreationTime is more than an implementation-specific time-out,<442> the server MUST disconnect the Connection, as specified in section 3.3.7.1, if any of the following conditions are TRUE:
Connection.Dialect is "Unknown".
Connection.Dialect is not "Unknown", and Connection.SessionTable is empty.
Connection.Dialect is not "Unknown", Connection.SessionTable is not empty, and there is no Session in Connection.SessionList where Session.State is Valid or Expired.
```

## Section 3.3.7.1: Handling Loss of a Connection
**Change type:** Modified

### Old Content
```
When the underlying transport indicates loss of a connection or after the server initiates a transport disconnect, for each session in Connection.SessionTable, the server MUST perform the following:
If Connection.Dialect belongs to the SMB 3.x dialect family and if the Session has more than one channel in Session.ChannelList, the server MUST perform the following action:
All requests in Session.Channel.Connection.RequestList MUST be canceled. The server SHOULD<440> pass the CancelRequestId to the object store to request cancellation of the pending operation.
The channel entry MUST be removed from the Session.ChannelList where Channel.Connection matches the disconnected connection.
If Session.Connection matches the disconnected connection, Session.Connection MUST be set to the first entry in Session.ChannelList.
Otherwise, the server MUST perform the following actions:
The server MUST iterate over the Session.OpenTable and determine whether each Open is to be preserved for reconnect. If any of the following conditions is satisfied, it indicates that the Open is to be preserved for reconnect.
Open.IsResilient is TRUE.
Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_BATCH and Open.OplockState is equal to Held, and Open.IsDurable is TRUE.
Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_LEASE, Lease.LeaseState contains SMB2_LEASE_HANDLE_CACHING, Open.OplockState is equal to Held, and Open.IsDurable is TRUE.
Open.IsPersistent is TRUE.
If the Open is to be preserved for reconnect, perform the following actions:
Set Open.Connection to NULL, Open.Session to NULL, Open.TreeConnect to NULL.
If Open.IsResilient is TRUE, set Open.ResilientOpenTimeOut to the current time plus Open.ResiliencyTimeout. The server SHOULD<441> start or reset the Resilient Open Scavenger Timer, as specified in section 3.3.2.4, under the following conditions:
If the Resilient Open Scavenger Timer is not already active.
If the Resilient Open Scavenger Timer is active and ResilientOpenScavengerExpiryTime is greater than Open.ResilientOpenTimeOut.
In both of the preceding cases, the server MUST set the timer to expire at Open.ResilientOpenTimeOut and MUST set ResilientOpenScavengerExpiryTime to Open.ResilientOpenTimeOut.
If Open.IsDurable is TRUE, the server MUST do the following:
The server MUST set Open.DurableOpenScavengerTimeout to the system time plus Open.DurableOpenTimeOut.
The server MUST start the durable open scavenger timer, as specified in sections 3.3.2.2.
If the Open is not to be preserved for reconnect, the server MUST close the Open as specified in section 3.3.4.17.
The server MUST disconnect every TreeConnect in Session.TreeConnectTable and deregister the TreeConnect by invoking the event specified in [MS-SRVS] section 3.1.6.7, providing the tuple <TreeConnect.Share.ServerName, TreeConnect.Share.Name> and  TreeConnect.TreeGlobalId as the input parameters, and the TreeConnect MUST be removed from Session.TreeConnectTable and freed. For each deregistered TreeConnect, TreeConnect.Share.CurrentUses MUST be decreased by 1.
The server MUST deregister the Session by invoking the event specified in [MS-SRVS] section 3.1.6.3, providing Session.SessionGlobalId as the input parameter, and the Session MUST be removed from GlobalSessionTable and freed. ServerStatistics.sts0_sopens MUST be decreased by 1.
All requests in Connection.RequestList MUST be canceled. The server SHOULD<442> pass the CancelRequestId to the object store to request cancellation of the pending operation.
The server MUST invoke the event specified in [MS-SRVS] section 3.1.6.16 to update the connection count by providing the tuple <Connection.TransportName,FALSE>.
The connection MUST be removed from ConnectionList and MUST be freed.
If the server implements the SMB 3.x dialect family, the server MUST enumerate all connections in ConnectionList using the removed Connection.ClientGuid where Connection.Dialect is not “2.0.2”. If no Connection entry is found, the server MAY remove the Client entry identified by Connection.ClientGuid from GlobalClientTable.
```

### New Content
```
When the underlying transport indicates loss of a connection or after the server initiates a transport disconnect, for each session in Connection.SessionTable, the server MUST perform the following:
If Connection.Dialect belongs to the SMB 3.x dialect family and if the Session has more than one channel in Session.ChannelList, the server MUST perform the following action:
All requests in Session.Channel.Connection.RequestList MUST be canceled. The server SHOULD<443> pass the CancelRequestId to the object store to request cancellation of the pending operation.
The channel entry MUST be removed from the Session.ChannelList where Channel.Connection matches the disconnected connection.
If Session.Connection matches the disconnected connection, Session.Connection MUST be set to the first entry in Session.ChannelList.
Otherwise, the server MUST perform the following actions:
The server MUST iterate over the Session.OpenTable and determine whether each Open is to be preserved for reconnect. If any of the following conditions is satisfied, it indicates that the Open is to be preserved for reconnect.
Open.IsResilient is TRUE.
Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_BATCH and Open.OplockState is equal to Held, and Open.IsDurable is TRUE.
Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_LEASE, Lease.LeaseState contains SMB2_LEASE_HANDLE_CACHING, Open.OplockState is equal to Held, and Open.IsDurable is TRUE.
Open.IsPersistent is TRUE.
If the Open is to be preserved for reconnect, perform the following actions:
Set Open.Connection to NULL, Open.Session to NULL, Open.TreeConnect to NULL.
If Open.IsResilient is TRUE, set Open.ResilientOpenTimeOut to the current time plus Open.ResiliencyTimeout. The server SHOULD<444> start or reset the Resilient Open Scavenger Timer, as specified in section 3.3.2.4, under the following conditions:
If the Resilient Open Scavenger Timer is not already active.
If the Resilient Open Scavenger Timer is active and ResilientOpenScavengerExpiryTime is greater than Open.ResilientOpenTimeOut.
In both of the preceding cases, the server MUST set the timer to expire at Open.ResilientOpenTimeOut and MUST set ResilientOpenScavengerExpiryTime to Open.ResilientOpenTimeOut.
If Open.IsDurable is TRUE, the server MUST do the following:
The server MUST set Open.DurableOpenScavengerTimeout to the system time plus Open.DurableOpenTimeOut.
The server MUST start the durable open scavenger timer, as specified in sections 3.3.2.2.
If the Open is not to be preserved for reconnect, the server MUST close the Open as specified in section 3.3.4.17.
The server MUST disconnect every TreeConnect in Session.TreeConnectTable and deregister the TreeConnect by invoking the event specified in [MS-SRVS] section 3.1.6.7, providing the tuple <TreeConnect.Share.ServerName, TreeConnect.Share.Name> and  TreeConnect.TreeGlobalId as the input parameters, and the TreeConnect MUST be removed from Session.TreeConnectTable and freed. For each deregistered TreeConnect, TreeConnect.Share.CurrentUses MUST be decreased by 1.
The server MUST deregister the Session by invoking the event specified in [MS-SRVS] section 3.1.6.3, providing Session.SessionGlobalId as the input parameter, and the Session MUST be removed from GlobalSessionTable and freed. ServerStatistics.sts0_sopens MUST be decreased by 1.
All requests in Connection.RequestList MUST be canceled. The server SHOULD<445> pass the CancelRequestId to the object store to request cancellation of the pending operation.
The server MUST invoke the event specified in [MS-SRVS] section 3.1.6.16 to update the connection count by providing the tuple <Connection.TransportName,FALSE>.
The connection MUST be removed from ConnectionList and MUST be freed.
If the server implements the SMB 3.x dialect family, the server MUST enumerate all connections in ConnectionList using the removed Connection.ClientGuid where Connection.Dialect is not “2.0.2”. If no Connection entry is found, the server MAY remove the Client entry identified by Connection.ClientGuid from GlobalClientTable.
```

## Section 6: Appendix A: Product Behavior
**Change type:** Modified

### Old Content
```
The information in this specification is applicable to the following Microsoft products or supplemental software. References to product versions include updates to those products.
The terms "earlier" and "later", when used with a product version, refer to either all preceding versions or all subsequent versions, respectively. The term "through" refers to the inclusive range of versions. Applicable Microsoft products are listed chronologically in this section.
Windows Client
Windows Vista operating system
Windows 7 operating system
Windows 8 operating system
Windows 8.1 operating system
Windows 10 operating system
Windows 11 operating system
Windows Server
Windows Server 2008 operating system
Windows Server 2008 R2 operating system
Windows Server 2012 operating system
Windows Server 2012 R2 operating system
Windows Server 2016 operating system
Windows Server operating system
Windows Server 2019 operating system
Windows Server 2022 operating system
Windows Server 2025 operating system
Exceptions, if any, are noted in this section. If an update version, service pack or Knowledge Base (KB) number appears with a product name, the behavior changed in that update. The new behavior also applies to subsequent updates unless otherwise specified. If a product edition appears with the product version, behavior is different in that product edition.
Unless otherwise specified, any statement of optional behavior in this specification that is prescribed using the terms "SHOULD" or "SHOULD NOT" implies product behavior in accordance with the SHOULD or SHOULD NOT prescription. Unless otherwise specified, the term "MAY" implies that the product does not follow the prescription.
<1> Section 1.6: The following table illustrates the support of SMB 2 protocol on various Windows operating system versions.
Windows Vista RTM implemented dialect 2.000, which was not interoperable and was obsoleted by Windows Vista SP1.
<2> Section 2.1:  Windows 11, version 24H2 operating system and later and Windows Server 2025 and later SMB2 servers allow listening on any configured port only when the transport is QUIC.
Windows 11, version 24H2 and later and Windows Server 2025 and later SMB2 clients can connect to an SMB2 server that allows listening on any configured port over TCP, RDMA and QUIC transports.
By default, Windows SMB2 clients and servers always use a single stream per QUIC connection. Sending or receiving more than one stream per connection will be blocked by the underlying transport QUIC.
<3> Section 2.2.1.2: Windows clients set this field to 0xFEFF.
<4> Section 2.2.1.2: Windows servers do not use this field in the request processing and return the value received in the request.
<5> Section 2.2.2: Windows 10 v1703 operating system and prior and Windows Server 2016 and prior set ErrorData to one uninitialized byte when ByteCount is zero.
<6> Section 2.2.2.2.1: Windows-based servers will never follow a symlink. It is the client's responsibility to evaluate the symlink and access the actual file using the symlink. Windows-based servers only return STATUS_STOPPED_ON_SYMLINK when the open fails due to presence of a symlink.
<7> Section 2.2.2.2.1: Windows-based servers will return an absolute target to a local resource in the format of "\??\C:\..." where C: is the drive mount point on the local system and ... is replaced by the remainder of the path to the target.
<8> Section 2.2.3: Windows-based SMB2 servers fail the request and return STATUS_INVALID_PARAMETER, if the DialectCount field is greater than 64.
<9> Section 2.2.3: Windows 8.1 operating system and later and Windows Server 2012 R2 operating system and later fail the request with STATUS_NOT_SUPPORTED if the Reserved field is set to a nonzero value.
<10> Section 2.2.3: Windows Vista SP1 and Windows Server 2008 do not support this dialect revision.
<11> Section 2.2.3: Windows Vista SP1, Windows Server 2008, Windows 7, and Windows Server 2008 R2 do not support this dialect revision.
<12> Section 2.2.3: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, and Windows Server 2012 do not support the SMB 3.0.2 dialect.
<13> Section 2.2.3: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 do not support the SMB 3.1.1 dialect.
<14> Section 2.2.3.1: Windows 10 v1809 operating system operating system and prior and Windows Server v1809 operating system operating system and prior do not send or process SMB2_COMPRESSION_CAPABILITIES.
<15> Section 2.2.3.1: Windows 10 v1809 operating system and prior and Windows Server v1809 operating system and prior do not send or process SMB2_NETNAME_NEGOTIATE_CONTEXT_ID.
<16> Section 2.2.3.1: Windows 10 v1909 operating system and prior and Windows Server v1909 operating system and prior do not send or process SMB2_TRANSPORT_CAPABILITIES.
<17> Section 2.2.3.1: Windows 10 operating system and prior and Windows Server v20H2 operating system and prior do not send or process SMB2_RDMA_TRANSFORM_CAPABILITIES.
<18> Section 2.2.3.1: Windows 10 operating system and prior and Windows Server v20H2 operating system and prior do not send or process SMB2_SIGNING_CAPABILITIES.
<19> Section 2.2.3.1.5:  Windows 10 v2004 operating system, Windows 10 v20H2 operating system, Windows Server v2004 operating system, and Windows Server v20H2 do not send or process SMB2_ACCEPT_TRANSPORT_LEVEL_SECURITY.
<20> Section 2.2.4: Windows Vista SP1 and Windows Server 2008 do not support this dialect revision.
<21> Section 2.2.4: Windows Vista SP1, Windows Server 2008, Windows 7 and Windows Server 2008 R2 do not support this dialect revision.
<22> Section 2.2.4: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, and Windows Server 2012 do not support this dialect revision.
<23> Section 2.2.4: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 do not support the SMB 3.1.1 dialect.
<24> Section 2.2.4: The "SMB 2.???" dialect string is not supported by SMB2 clients and servers in Windows Vista SP1 and Windows Server 2008.
<25> Section 2.2.4: Windows-based SMB2 servers can set this field to any value.
<26> Section 2.2.4: Windows–based SMB2 servers generate a new ServerGuid each time they are started.
<27> Section 2.2.4: Windows clients do not enforce the MaxTransactSize value.
<28> Section 2.2.5: Windows-based clients always set the Capabilities field to SMB2_GLOBAL_CAP_DFS(0x00000001) and the server will ignore them on receipt.
<29> Section 2.2.5: Windows clients set the Buffer with a token as produced by the NTLM authentication protocol in the case, see [MS-NLMP] section 3.1.5.1.
<30> Section 2.2.6: Windows clients set the Buffer with a token as produced by the NTLM authentication protocol in the case, see [MS-NLMP] section 3.1.5.1.
<31> Section 2.2.9: Windows 10 v1703 and prior and Windows Server 2016 and prior do not support the SMB2_TREE_CONNECT_FLAG_REDIRECT_TO_OWNER flag.
<32> Section 2.2.9: The Windows SMB 2 Protocol client translates any names of the form \\server\pipe to \\server\IPC$ before sending a request on the network.
<33> Section 2.2.10: SMB2_SHAREFLAG_FORCE_LEVELII_OPLOCK is not supported on Windows Vista SP1 and Windows Server 2008.
<34> Section 2.2.10: Windows 10 v1703 and prior and Windows Server 2016 and prior do not send or process this flag.
<35> Section 2.2.10: Windows 10 operating system and prior and Windows Server v20H2 operating system and prior do not send or process this flag.
<36> Section 2.2.13: Windows-based clients never use exclusive oplocks. Because there are no situations where the client would require an exclusive oplock where it would not also require an SMB2_OPLOCK_LEVEL_BATCH, it always requests an SMB2_OPLOCK_LEVEL_BATCH.
<37> Section 2.2.13: When opening a printer file or a named pipe, Windows-based servers ignore these ShareAccess values.
<38> Section 2.2.13: When opening a printer object, Windows-based servers ignore this value.
<39> Section 2.2.13: When opening a printer object, Windows-based servers ignore this value.
<40> Section 2.2.13: When opening a printer object, Windows-based servers ignore this value.
<41> Section 2.2.13: Windows-based servers reserve all bits that are not specified in the table. If any of the reserved bits are set, STATUS_NOT_SUPPORTED is returned.
<42> Section 2.2.13: Windows SMB2 clients do not initialize this bit. The bit contains the value specified by the caller when requesting the open.
<43> Section 2.2.13: Windows SMB2 clients do not initialize this bit. The bit contains the value specified by the caller when requesting the open.
<44> Section 2.2.13: Windows SMB2 clients do not initialize this bit. The bit contains the value specified by the caller when requesting the open.
<45> Section 2.2.13: Windows SMB2 clients do not initialize this bit. The bit contains the value specified by the caller when requesting the open.
<46> Section 2.2.13: Windows SMB2 clients do not initialize this bit. The bit contains the value specified by the caller when requesting the open.
<47> Section 2.2.13: Windows Vista SP1, Windows Server 2008, Windows 7, Windows 8, and Windows 8.1-based clients will set this bit when it is requested by the application.
<48> Section 2.2.13.1.1: Windows sets this flag to the value passed in by the higher-level application.
<49> Section 2.2.13.1.1: Windows 7 operating system and later and Windows Server 2008 R2 operating system and later do not ignore the SYNCHRONIZE bit, and pass it to the underlying object store. If the caller requests SYNCHRONIZE in the DesiredAccess parameter, but the SYNCHRONIZE access is not granted to the caller for the object being created or opened, the underlying object store fails the request and returns STATUS_ACCESS_DENIED. When SYNCHRONIZE access is granted, the SYNCHRONIZE bit is returned in MaximalAccess field of SMB2_CREATE_QUERY_MAXIMAL_ACCESS_RESPONSE with no other behavior.
<50> Section 2.2.13.1.1: Windows fails the create request with STATUS_ACCESS_DENIED if the caller does not have the SeSecurityPrivilege, as specified in [MS-LSAD] section 3.1.1.2.1.
<51> Section 2.2.13.1.2: Windows sets this flag to the value passed in by the higher-level application.
<52> Section 2.2.13.1.2: Windows 7 operating system and later and Windows Server 2008 R2 operating system and later do not ignore the SYNCHRONIZE bit, and pass it to the underlying object store. If the caller requests SYNCHRONIZE in the DesiredAccess parameter, but the SYNCHRONIZE access is not granted to the caller for the object being created or opened, the underlying object store fails the request and returns STATUS_ACCESS_DENIED. When SYNCHRONIZE access is granted, the SYNCHRONIZE bit is returned in MaximalAccess field of SMB2_CREATE_QUERY_MAXIMAL_ACCESS_RESPONSE (section 2.2.14.2.5) with no other behavior.
<53> Section 2.2.13.1.2: Windows fails the create request with STATUS_ACCESS_DENIED if the caller does not have the SeSecurityPrivilege, as specified in [MS-LSAD] section 3.1.1.2.1.
<54> Section 2.2.13.2: If DataLength is 0, Windows-based clients set this field to any value.
<55> Section 2.2.13.2.8: Windows 7 operating system and later and Windows Server 2008 R2 operating system and later acting as SMB servers support the following combinations of values: 0, READ, READ | WRITE, READ | HANDLE, READ | WRITE | HANDLE.
<56> Section 2.2.13.2.10: Windows Server 2012 operating system and later support the following combinations of values: 0, READ, READ | WRITE, READ | HANDLE, READ | WRITE | HANDLE.
<57> Section 2.2.14: Windows-based SMB2 servers always return FILE_OPENED for pipes with successful opens.
<58> Section 2.2.14: Windows-based SMB2 servers can set this field to any value.
<59> Section 2.2.14.2.11: Windows 8 operating system and later and Windows Server 2012 operating system and later set this field to an arbitrary value.
<60> Section 2.2.19:  Windows 10 v1809 and prior and Windows Server v1809 and prior do not send or process SMB2_READFLAG_REQUEST_COMPRESSED flag.
<61> Section 2.2.20: Windows 10 operating system and prior and Windows Server v20H2 operating system and prior do not send or process SMB2_READFLAG_RESPONSE_RDMA_TRANSFORM flag.
<62> Section 2.2.21: Windows 10 operating system and prior and Windows Server v20H2 operating system and prior do not send or process SMB2_CHANNEL_RDMA_TRANSFORM flag.
<63> Section 2.2.24.2: Windows clients always set the LeaseState in the Lease Break Acknowledgment to be equal to the LeaseState in the Lease Break Notification from the server.
<64> Section 2.2.31: Windows clients set the OutputOffset field equal to the InputOffset field.
<65> Section 2.2.31.1.1: Windows clients set this field to an arbitrary value.
<66> Section 2.2.32: Windows–based SMB2 servers set InputCount to the same value as the value received in the IOCTL request for the following FSCTLs.
FSCTL_FIND_FILES_BY_SID
FSCTL_GET_RETRIEVAL_POINTERS
FSCTL_QUERY_ALLOCATED_RANGES
FSCTL_READ_FILE_USN_DATA
FSCTL_RECALL_FILE
FSCTL_WRITE_USN_CLOSE_RECORD
Windows clients ignore the InputCount field.
<67> Section 2.2.32: Windows–based SMB2 servers set OutputOffset to InputOffset + InputCount, rounded up to a multiple of 8.
<68> Section 2.2.32.2: Windows-based SMB2 server will place 2 extra bytes set to zero in the SRV_SNAPSHOT_ARRAY response, if NumberOfSnapShotsReturned is zero.
<69> Section 2.2.32.3: Windows-based servers always send 4 bytes of zero for the Context field.
<70> Section 2.2.32.4.1: Windows–based SMB2 servers and clients do not check SourceFileName. It is ignored.
<71> Section 2.2.32.5.1.2: Windows 10 v1709 operating system through Windows 10 v1909 and Windows Server v1709 operating system through Windows Server v1909 set this field to any value.
<72> Section 2.2.33: Windows 10 operating system and prior and Windows Server 2022 operating system and prior do not send or process this information class.
<73> Section 2.2.33:  Windows 11, version 23H2 operating system and prior and Windows Server 2022 and prior do not send or process FileId64ExtdDirectoryInformation information class.
<74> Section 2.2.33:  Windows 11, version 23H2 and prior and Windows Server 2022 and prior do not send or process FileId64ExtdBothDirectoryInformation information class.
<75> Section 2.2.33:  Windows 11, version 23H2 and prior and Windows Server 2022 and prior do not send or process FileIdAllExtdDirectoryInformation information class.
<76> Section 2.2.33:  Windows 11, version 23H2 and prior and Windows Server 2022 and prior do not send or process FileIdAllExtdBothDirectoryInformation information class.
<77> Section 2.2.33: SMB2 wildcard characters are based on Windows wildcard characters, as described in [MS-FSA] section 2.1.4.4, Algorithm for Determining if a FileName Is in an Expression. For more information on wildcard behavior in Windows, see [FSBO] section 7.
<78> Section 2.2.37: Windows SMB2 servers ignore the FileInfoClass field for quota queries. Windows SMB2 clients set the FileInfoClass field to 0x20 for quota queries.
<79> Section 2.2.37: Windows clients set this value to the offset from the start of the SMB2 header to the beginning of the Buffer field.
<80> Section 2.2.37: Windows clients send a 1-byte buffer of 0 when InputBufferLength is set to 0.
<81> Section 2.2.37.1: Windows-based clients never send a request using the SidBuffer format 2.
<82> Section 2.2.39: Windows-based servers will fail the request with STATUS_INVALID_PARAMETER if BufferOffset is less than 0x60 or greater than 0xA0.
<83> Section 2.2.41: Windows 8 operating system and later and Windows Server 2012 operating system and later set this field to an arbitrary value.
<84> Section 2.2.42.1: Windows 10 v1809 and prior and Windows Server v1809 and prior do not send or process SMB2 COMPRESSION_TRANSFORM_HEADER_UNCHAINED.
<85> Section 2.2.42.2: Windows 10 v1909 and prior and Windows Server v1909 and prior do not send or process SMB2_COMPRESSION_TRANSFORM_HEADER_CHAINED.
<86> Section 2.2.42.2.1: Windows 10 v1909 and prior and Windows Server v1909 and prior do not send or process SMB2_COMPRESSION_CHAINED_PAYLOAD_HEADER.
<87> Section 2.2.42.2.2: Windows 10 v1909 and prior and Windows Server v1909 and prior do not send or process SMB2_COMPRESSION_PATTERN_PAYLOAD_V1.
<88> Section 2.2.43: Windows 10 operating system and prior and Windows Server v20H2 operating system and prior do not send or process RDMA transforms.
<89> Section 2.2.43.1: Windows 10 operating system and prior and Windows Server v20H2 operating system and prior do not send or process RDMA transforms.
<90> Section 3.1.3: By default, Windows-based servers set the RequireMessageSigning value to TRUE for domain controllers and FALSE for all other machines.
<91> Section 3.1.3: Windows 8 and later and Windows Server 2012 and later set IsEncryptionSupported to TRUE.
<92> Section 3.1.3: Windows 10 v1903 operating system and later and Windows Server v1903 operating system and later set IsCompressionSupported to TRUE.
<93> Section 3.1.3: Windows 10 v2004 and later and Windows Server v2004 and later operating systems set IsChainedCompressionSupported to TRUE.
<94> Section 3.1.3: Windows 11 operating system and later and Windows Server 2022 operating system and later set IsRDMATransformSupported to TRUE.
<95> Section 3.1.3: Windows 11 operating system and later and Windows Server 2022 operating system and later set this to TRUE.
<96> Section 3.1.3:  Windows 11 and later and Windows Server 2022 and later set IsSigningCapabilitiesSupported to TRUE.
<97> Section 3.1.3:  Windows 10 v2004 and later and Windows Server v2004 and later set IsTransportCapabilitiesSupported to TRUE.
<98> Section 3.1.3:  Windows 11, version 24H2 and later and Windows Server 2022, 23H2 operating system and later set IsServerToClientNotificationsSupported to TRUE.
<99> Section 3.1.4.3: Windows-based clients and servers do not encrypt the message if the connection is NetBIOS over TCP.
<100> Section 3.1.4.4: Windows-based clients and servers do not compress the message if the connection is over RDMA.
<101> Section 3.1.4.4: Windows-based clients choose to selectively compress only segments of SMB2 requests with large payloads, whose size is greater than 4096 bytes.
<102> Section 3.2.1.2: Windows clients do not enforce the MaxTransactSize value.
<103> Section 3.2.2.1: The Windows-based client implements this timer with a default value of 60 seconds. The client does not enforce this timer for the following commands:
Named Pipe Read
Named Pipe Write
Directory Change Notifications
Blocking byte range lock requests
FSCTLs: FSCTL_PIPE_PEEK, FSCTL_PIPE_TRANSCEIVE, FSCTL_PIPE_WAIT
<104> Section 3.2.2.2: The Windows-based clients scan existing connections every 10 seconds and disconnect idle connects that have no open files and that have had no activity for 10 or more seconds.
<105> Section 3.2.2.3: Windows clients set this timer to 600 seconds, except Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2 clients, which do not implement this timer.
<106> Section 3.2.3:  Windows clients set RejectGuestAccess to TRUE by default.
<107> Section 3.2.3:  Windows clients set AllowInsecureGuestAccess to FALSE by default.
<108> Section 3.2.3: Windows 8 operating system and later and Windows Server 2012 operating system and later clients set this based on a stored value in the registry.
<109> Section 3.2.3: Windows 10 v1903 and later, and Windows Server v1903 and later set this to FALSE.
<110> Section 3.2.3:  Windows 11 with [MSKB-5035854], Windows 11, version 22H2 operating system with [MSKB-5035942], Windows Server 2022 with [MSKB-5035857], Windows Server 2022, 23H2, Windows 11, version 24H2 and later, and Windows Server 2025 and later set IsMutualAuthOverQUICSupported to TRUE.
<111> Section 3.2.4.1.1: A client can selectively sign requests, and the server will sign the corresponding responses.
<112> Section 3.2.4.1.2: Windows-based clients require a minimum of 4 credits.
<113> Section 3.2.4.1.2: The Windows-based client will request credits up to a configurable maximum of 128 by default. A Windows-based client sends a CreditRequest value of 0 for an SMB2 NEGOTIATE Request and expects the server to grant at least 1 credit. In subsequent requests, the client will request credits sufficient to maintain its total outstanding limit at the configured maximum.
<114> Section 3.2.4.1.3: Windows 7 operating system and later and Windows Server 2008 R2 operating system and later SMB2 clients will block any newly initiated multi-credit requests that exceed the shortage, but will send out other requests that can be satisfied using the available credits.
<115> Section 3.2.4.1.3: Windows-based clients set the MessageId field to 0, when the AsyncId field is set to an asynchronous identifier of the request.
<116> Section 3.2.4.1.4: Windows-based clients do not send compounded CREATE + READ/WRITE requests when the payload size of the WRITE request or the anticipated response of the READ request is greater than 65536.
<117> Section 3.2.4.1.4: Windows SMB2 Server allows a mix of related and unrelated compound requests in the same transport send. Upon encountering a request with SMB2_FLAGS_RELATED_OPERATIONS not set Windows SMB2 Server treats it as the start of a chain.
<118> Section 3.2.4.1.4: The Windows-based client does not send unrelated compounded requests.
<119> Section 3.2.4.1.4: Windows-based clients will compound certain related requests to improve performance, by combining a Create with another operation, such as an attribute query.
<120> Section 3.2.4.1.5: Windows 7 and Windows Server 2008 R2 SMB2 clients set CreditCharge to 1 for IOCTL requests.
<121> Section 3.2.4.1.5: Windows 7 operating system and later and Windows Server 2012 operating system and later based SMB2 clients set the CreditCharge field to 1 if Connection.SupportsMultiCredit is FALSE.
<122> Section 3.2.4.1.7: Windows-based clients choose the Channel with the least value of Channel.Connection.OutstandingRequests.
<123> Section 3.2.4.1.8: Windows 10 v20H2 operating system and prior and Windows Server v20H2 operating system and prior encrypt the message as specified in section 3.1.4.3 before sending.
<124> Section 3.2.4.1.9:  Windows 10 v1903 and later, and Windows Server v1903 and later do not compress SMB2 NEGOTIATE request and SMB2 OPLOCK_BREAK Acknowledgment.
<125> Section 3.2.4.2: Windows-based clients always set up a new transport connection when establishing a new session to a server.
<126> Section 3.2.4.2: Windows will reuse an existing session only if the access is by the same logged-on user and the Connection.ServerName matches the application-supplied ServerName.
<127> Section 3.2.4.2: Windows will reuse the connection to establish a new session, if a connection is available and Connection.ServerName matches the application-supplied ServerName
<128> Section 3.2.4.2.1:  Windows clients initiate new transport connections to the server with Direct TCP and NetBIOS over TCP. Windows 10 v1511 Enterprise operating system and Windows Server 2012 operating system and later do not initiate a new transport connection with RDMA, but do after a multichannel exchange if a suitable interface is available.
<129> Section 3.2.4.2.1: Windows Vista SP1 and Windows Server 2008 clients enumerate all transports, send a Direct TCP connection request, and then, after 500 milliseconds, send connection requests to all other eligible addresses and all other NetBIOS over TCP transports.
Windows 7 and Windows Server 2008 R2 clients enumerate all transports, send a Direct TCP connection request, and then, after 1,000 milliseconds, send connection requests to all other eligible addresses and all other NetBIOS over TCP transports.
Windows 8 operating system and later and Windows Server 2012 operating system and later clients look up a server entry in ServerList where Server.ServerName matches the ServerName to which the connection is established. If no entry is found, the clients enumerate all transports, send a Direct TCP connection request, and then, after 1,000 milliseconds, send connection requests to all other eligible addresses over Direct TCP and NetBIOS over TCP transports. If an entry is found, the clients send a Direct TCP connection request, and then, after 1,000 milliseconds, enumerate all transports and send connection requests to all Direct TCP addresses.
In each case, the first successful connection is used and all others are closed.
<130> Section 3.2.4.2.2: The Windows-based client will initiate a multi-protocol negotiation unless it has previously negotiated with this server and the negotiated server's DialectRevision is equal to 0x0202, 0x0210, 0x0300, 0x0302, or 0x0311. In the latter case, it will initiate an SMB2-Only negotiate.
<131> Section 3.2.4.2.2.2: Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 set Dialects array to all the dialects the client implements and DialectCount to the number of dialects in Dialects array.
<132> Section 3.2.4.2.2.2:  Windows 8 operating system, Windows 8.1, Windows Server 2012 operating system and Windows Server 2012 R2 always set SMB2_GLOBAL_CAP_ENCRYPTION in the Capabilities field if IsEncryptionSupported is TRUE.
<133> Section 3.2.4.2.2.2:  Windows 10, Windows 11 without [MSKB-5036894], Windows 11 v22H2 without [MSKB-5036980], Windows 11, version 23H2 without [MSKB-5036980], Windows Server 2016, Windows Server 2022 without [MSKB-5036909] and Windows Server 2022, 23H2 without [MSKB-5036910] always set SMB2_GLOBAL_CAP_ENCRYPTION in the Capabilities field if IsEncryptionSupported is TRUE.
<134> Section 3.2.4.2.2.2: Windows 10, and Windows Server 2016 operating system and later use 32 bytes of Salt.
<135> Section 3.2.4.2.2.2: Windows 10 and Windows Server 2016 through Windows Server v20H2 initialize with AES-128-GCM(0x0002), followed by AES-128-CCM(0x0001).
Windows 11 operating system and later and Windows Server 2022 operating system and later initialize with AES-128-GCM(0x0002), followed by AES-128-CCM(0x0001), followed by AES-256-GCM(0x0004), followed by AES-256-CCM(0x0003).
<136> Section 3.2.4.2.2.2: Windows 10 v1903, Windows 10 v1909, Windows Server v1903, and Windows Server v1909 operating systems initialize with LZ77(0x0002) followed by LZ77+Huffman(0x0003) followed by LZNT1(0x0001).
Windows 10 v2004 through Windows 11, version 23H2 and Windows Server v2004 through Windows Server 2022, 23H2 initialize with Pattern_V1(0x0004), followed by LZ77(0x0002), followed by LZ77+Huffman(0x0003), followed by LZNT1(0x0001).
Windows 11, version 24H2 and later and Windows Server 2025 and later initialize with Pattern_V1(0x0004), followed by LZ77(0x0002), followed by LZ77+Huffman(0x0003), followed by LZNT1(0x0001), followed by LZ4(0x0005).
<137> Section 3.2.4.2.2.2: Windows 11 operating system and later and Windows Server 2022 operating system and later set RDMATransformIds to SMB2_RDMA_TRANSFORM_ENCRYPTION (0x0001) and SMB2_RDMA_TRANSFORM_SIGNING (0x0002).
<138> Section 3.2.4.2.2.2: Windows 10 v1809 and prior and Windows Server v1809 and prior do not support SMB2_NETNAME_NEGOTIATE_CONTEXT_ID.
<139> Section 3.2.4.2.2.2: Windows 11 operating system and later and Windows Server 2022 operating system and later initialize with AES-GMAC(0x0002), followed by AES-CMAC(0x0001), followed by HMAC-SHA256(0x0000).
<140> Section 3.2.4.2.3: Windows-based clients implement the first option that is specified.
<141> Section 3.2.4.2.3: All the GSS-API tokens used by Windows SMB2 clients are up to 4Kbytes in size. SMB2 servers always instruct the GSS_API server to expect the GSS_C_FRAGMENT_TO_FIT.
<142> Section 3.2.4.2.3.1: Windows-based clients implement the first option that is specified.
<143> Section 3.2.4.2.3.1: All the GSS-API tokens used by Windows SMB2 clients are up to 4Kbytes in size. SMB2 servers always instruct the GSS_API server to expect the GSS_C_FRAGMENT_TO_FIT.
<144> Section 3.2.4.2.4: Windows 11 v22H2 without [MSKB-5037853], Windows 11, version 23H2 without [MSKB-5037853] and Windows Server 2022, 23H2 without [MSKB-5037781] set SMB2_TREE_CONNECT_FLAG_REDIRECT_TO_OWNER bit in the Flags field of SMB2 TREE_CONNECT request if the share previously connected includes either SMB2_SHAREFLAG_ISOLATED_TRANSPORT flag or SMB2_SHARE_CAP_ASYMMETRIC capability.
<145> Section 3.2.4.3: Windows clients set File.LeaseKey to a newly generated GUID as specified in [MS-DTYP] section 2.3.4.2.
<146> Section 3.2.4.3:  On Windows 7 operating system and Windows Server 2008 R2, a 128-bit ClientLeaseId is generated by an arithmetic combination of LeaseKey and ClientGuid, which is passed to the object store at open/create time. On Windows 8 operating system and later and Windows Server 2012 operating system and later, the LeaseKey in the request is used as the ClientLeaseId.
<147> Section 3.2.4.3:  Although not required, failure to include the lease context can result in a lease break.
<148> Section 3.2.4.3: On Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2, the Lease.ClientLeaseId and Lease.ParentLeaseKey are passed to the object store in the form of TargetOplockKey and ParentOplockKey. A new or existing lease is thereby associated with the resulting open.
To acquire or promote the lease as dictated by the SMB2_CREATE_REQUEST_LEASE_V2 Create Context, a subsequent object store call is invoked as described in. [MS-FSA] section 2.1.5.18 Server Requests an Oplock. The Open parameter passed is the Open result from the above operation, and the Type parameter is LEVEL_GRANULAR to indicate a Lease request. The RequestedOplockLevel field is constructed to include zero or more bits as follows.
The Status code returned indicates whether the requested lease was granted.
<149> Section 3.2.4.3: Windows clients set File.LeaseKey to a newly generated GUID as specified in [MS-DTYP] section 2.3.4.2.
<150> Section 3.2.4.3: Microsoft Windows lease-aware clients always include SMB2_OPLOCK_LEVEL_LEASE if the open can potentially cause a lease break.
<151> Section 3.2.4.3:  Windows-based clients will request a batch oplock for file creates when application does not provide a requested oplock level, or an exclusive oplock is specified, or a lease is requested.
<152> Section 3.2.4.3.5: Windows 8 operating system and later and Windows Server 2012 operating system and later clients set this to zero.
<153> Section 3.2.4.3.8: A Windows client application requestsSMB2_LEASE_READ_CACHING and SMB2_LEASE_HANDLE_CACHING when a file is opened for read access. In addition, a Windows client application requests SMB2_LEASE_WRITE_CACHING if the file is being opened for write access.
<154> Section 3.2.4.6:  Windows-based clients set MinimumCount field to 0.
<155> Section 3.2.4.6:  Windows-based clients will try to send multiple read commands at the same time, starting at the lowest offset and working to the highest.
<156> Section 3.2.4.6:  Windows-based clients default to 4 KB.
<157> Section 3.2.4.7: Windows-based clients set the DataOffset field to 0x70, which indicates that the payload is always placed at the beginning of the Buffer field.
<158> Section 3.2.4.7: Windows-based clients will try to send multiple write commands at the same time, starting at the lowest offset and working to the highest.
<159> Section 3.2.4.7: Windows-based clients default to 4 KB.
<160> Section 3.2.4.8: Windows clients set this value to the offset from the start of the SMB2 header to the beginning of the Buffer field.
<161> Section 3.2.4.9: In a SET_INFO request where FileInfoClass is set to FileRenameInformation, Windows Vista SP1, Windows Server 2008, Windows 7, and Windows Server 2008 R2 clients append up to 4 additional padding bytes set to arbitrary values.
<162> Section 3.2.4.10: Windows clients set this value to the offset from the start of the SMB2 header to the beginning of the Buffer field.
<163> Section 3.2.4.12: Windows clients set this value to the offset from the start of the SMB2 header to the beginning of the Buffer field.
<164> Section 3.2.4.14: Windows-based clients will set StartSidLength and StartSidOffset to any value.
<165> Section 3.2.4.17: The Windows SMB2 server implementation closes and reopens the directory handle in order to "reset" the enumeration state. So any outstanding operations on the directory handle will be failed with a STATUS_FILE_CLOSED error.
<166> Section 3.2.4.20: Windows 7 and Windows Server 2008 R2 SMB2 clients set CreditCharge to 1 for IOCTL requests.
<167> Section 3.2.4.20.2.1: Windows clients set this field to InputOffset + InputCount, rounded up to a multiple of 8 bytes.
<168> Section 3.2.4.20.2.2: Windows applications use FSCTL_SRV_COPYCHUNK if the target file handle has FILE_READ_DATA access. Otherwise, they use the FSCTL_SRV_COPYCHUNK_WRITE.
<169> Section 3.2.4.20.2.2: Windows clients set the OutputOffset field to InputOffset + InputCount, rounded up to a multiple of 8 bytes.
<170> Section 3.2.4.20.3: Windows clients set the OutputOffset field to InputOffset + InputCount, rounded up to a multiple of 8 bytes.
<171> Section 3.2.4.20.4: Windows clients set the OutputOffset field to InputOffset + InputCount, rounded up to a multiple of 8 bytes.
<172> Section 3.2.4.20.5: Windows clients set the OutputOffset field to InputOffset + InputCount, rounded up to a multiple of 8 bytes.
<173> Section 3.2.4.20.6: Windows-based SMB2 servers pass File System Control requests through to the local object store but do not support I/O Control requests and fail such requests with STATUS_NOT_SUPPORTED.
<174> Section 3.2.4.20.6: Windows clients set the OutputOffset field to InputOffset + InputCount, rounded up to a multiple of 8 bytes.
<175> Section 3.2.4.20.7: Windows clients set the OutputOffset field to the sum of the values of the InputOffset and the InputCount fields, rounded up to a multiple of 8 bytes.
<176> Section 3.2.4.20.8: Windows clients set the OutputOffset field to InputOffset + InputCount, rounded up to a multiple of 8 bytes.
<177> Section 3.2.4.20.10: Windows clients set this to 64 kilobytes.
<178> Section 3.2.4.20.11: Windows clients set the OutputOffset field to InputOffset.
<179> Section 3.2.4.24: Windows based clients set the MessageId field to 0, when the AsyncId field is set to an asynchronous identifier of the request.
<180> Section 3.2.5.1: For the following error codes, Windows-based clients will retry the operation up to three times and then retry the operation every 5 seconds until the count of milliseconds specified by Open.ResilientTimeout is exceeded:
STATUS_SERVER_UNAVAILABLE
STATUS_FILE_NOT_AVAILABLE
STATUS_SHARE_UNAVAILABLE
<181> Section 3.2.5.1.1.1: Windows-based clients discard the message if it is encrypted and the connection is NetBIOS over TCP.
<182> Section 3.2.5.1.1.1: Windows 8.1 and Windows Server 2012 R2 continue to process the entire compound response if SMB2_FLAGS_RELATED_OPERATIONS is set in the Flags field of the SMB2 header of the response.
<183> Section 3.2.5.1.1.2: Windows-based clients discard the message if it is compressed and the connection is over RDMA.
<184> Section 3.2.5.1.5: Windows clients extend the Request Expiration Timer for requests being processed asynchronously as follows:
If the registry value ExtendedSessTimeout in HKLM\System\CurrentControlSet\Services\LanmanWorkStation\Parameters\ is set, the clients use the same value. Otherwise, the clients extend the expiration time to four times the value of default session timeout.
Windows Vista SP1, Windows Server 2008, Windows 7 and Windows Server 2008 R2 never enforce a timeout on SMB2 CHANGE_NOTIFY requests, SMB2 LOCK requests without the SMB2_LOCKFLAG_FAIL_IMMEDIATELY flag, SMB2 READ requests on named pipes, SMB2 WRITE requests on named pipes, and the FSCTL_PIPE_PEEK, FSCTL_PIPE_TRANSCEIVE and FSCTL_PIPE_WAIT named pipe FSCTLs.
<185> Section 3.2.5.1.7: Windows-based clients will not disconnect the connection, but will simply fail the request.
<186> Section 3.2.5.1.8: Windows-based SMB 2 Protocol clients do not check the validity of the command in the response.
<187> Section 3.2.5.1.9: Windows-based clients ignore 8-byte alignment boundary checking in a compounded chain.
<188> Section 3.2.5.2: Windows-based clients will not use the MaxTransactSize and will use the ServerGuid to determine if the client and server are the same machine.
<189> Section 3.2.5.2:  Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 disconnect the connection if MaxTransactSize, MaxReadSize, or MaxWriteSize is less than 4096.
<190> Section 3.2.5.2:  Windows 10 v1903 through Windows 10 v20H2 and Windows Server v1903 through Windows Server v20H2 will disconnect the connection.
<191> Section 3.2.5.3.3:  Windows 8, Windows 8.1, Windows Server 2012, and Windows Server 2012 R2 operating system do not perform this verification.
<192> Section 3.2.5.5: Windows 10 v1507 operating system through Windows 10 v1909, Windows Server 2016 operating system and later set Dialects array to all the dialects the client implements.
<193> Section 3.2.5.7: Windows 8 operating system and later and Windows Server 2012 operating system and later replay the create operation up to three times or until all channels in the session are disconnected.
<194> Section 3.2.5.12: Windows 8 operating system and later and Windows Server 2012 operating system and later replay the write operation up to three times or until all channels in the session are disconnected.
<195> Section 3.2.5.14: Windows 8 operating system and later and Windows Server 2012 operating system and later replay the IOCTL operation up to three times or until all of the channels in the session are disconnected.
<196> Section 3.2.5.14: If the OutputCount field in an SMB2 IOCTL Response is 0 and the OutputOffset exceeds the size of the SMB2 response, Windows clients will return STATUS_INVALID_NETWORK_RESPONSE to the application.
<197> Section 3.2.5.14.9: Windows clients enable TCP keepalives to detect broken connections.
<198> Section 3.2.5.14.11: Windows-based SMB2 clients will choose the interfaces using the following criteria:
Skip the interfaces in NETWORK_INTERFACE_INFO Response where IfIndex is 0.
For each interface returned in NETWORK_INTERFACE_INFO Response, if the interface has both link-local and non-link-local IP addresses, skip the link-local IP address.
If there is one or more multiple link-local addresses (suppose there are Y such interfaces), select local interfaces which have only link-local addresses (suppose there are X such local interfaces).
Build a destination address list, include all server non-link-local addresses and X*Y server link-local addresses.
For each RDMA capable address pair, duplicate the address pair, one for RDMA and one for Direct TCP.
Sort address pairs by which address pair is best suited for connection between client and server.
For each address pair, compute
Link speed of the pair = min( link speed of local interface, link speed of remote interface)
RSS capable = RSS capable of local interface and RSS capable of remote interface
If there are RDMA capable address pairs, select them.
Otherwise if there are RSS capable address pairs, select them.
Otherwise select remaining address pairs.
Select the pairs with the highest link speed from the selected address pairs.
Select local/remote address pairs so that all eligible local/remote interfaces are used and the connections are distributed among local and remote interfaces.
By default, Windows clients create four connections per RSS-capable address pair or two connections per RDMA-capable address pair or only a single connection when the address pair is neither RSS-capable nor RDMA-capable.
<199> Section 3.2.5.14.11: By default, Windows 8 and later will try to establish alternate channels if Connection.OutstandingRequests exceeds 8. By default, Windows Server 2012 operating system and later will try to establish alternate channels if Connection.OutstandingRequests exceeds 1.
<200> Section 3.2.5.18: Windows 8 operating system and later and Windows Server 2012 operating system and later replay the SetInfo operation up to three times or until all of the channels in the session are disconnected.
<201> Section 3.2.5.19.2:  Windows clients do not send a Lease Break Acknowledgement when they have an outstanding SMB2 CREATE Request on the same File.
<202> Section 3.2.6.1: Windows clients use a default time-out of 60 seconds.
<203> Section 3.2.6.1: Windows-based clients return a STATUS_CONNECTION_DISCONNECTED error code to the calling application.
<204> Section 3.2.6.1: The Windows-based clients will disconnect the connection.
<205> Section 3.2.7.1:  When the reestablishment of the durable handle fails with a network error, Windows clients retry the reestablishment three times.
<206> Section 3.3.1.1: Windows-based servers will limit the maximum range of sequence numbers. If a client has been granted 10 credits, the server will not allow the difference between the smallest available sequence number and the largest available sequence number to exceed 2*10 = 20. Therefore, if the client has sequence number 10 available and does not send it, the server will stop granting credits as the client nears sequence number 30, and eventually will grant no further credits until the client sends sequence number 10.
<207> Section 3.3.1.2: A Windows-based server will grant some portion of the client request based on available resources and the number of credits the client is currently taking advantage of. A Windows–based server grants credits based on usage but will attempt to enforce fairness if there are insufficient credits.
<208> Section 3.3.1.2: Windows-based SMB2 servers support a configurable minimum credit limit below which the client is unconditionally granted all credits it requests, and a configurable maximum credit limit above which credits are never granted, as follows:
<209> Section 3.3.1.2: A Windows–based server does not currently scale credits based on quality of service features.
<210> Section 3.3.1.4:  On Windows 7 and Windows Server 2008 R2, a 128-bit ClientLeaseId is generated by an arithmetic combination of LeaseKey and ClientGuid, which is passed to the object store at open/create time. On Windows 8 operating system and later and Windows Server 2012 operating system and later, the LeaseKey in the request is used as the ClientLeaseId.
<211> Section 3.3.1.4: Windows 7 operating system and later and Windows Server 2008 R2 operating system and later based SMB2 servers support only the levels described above, and Windows 7 operating system and later and Windows Server 2008 R2 operating system and later based SMB2 clients request only those levels.
<212> Section 3.3.1.6: Windows-based servers allow the sharing of both printers and traditional file shares.
<213> Section 3.3.1.6: In Windows, this abstract state element contains the security descriptor for the share.
<214> Section 3.3.1.6: Windows-based SMB2 clients do not cache directory enumeration results.
<215> Section 3.3.1.13: The Windows SMB2 server allocates an I/O request (IRP) structure which it uses to locally request action from the object store. The Request.CancelRequestId is set to the unique address of this structure.
<216> Section 3.3.2.1: Windows SMB2 servers set this timer to 35 seconds.
<217> Section 3.3.2.2: Windows-based SMB2 servers set this timer to a constant value of 16 minutes.
<218> Section 3.3.2.3: Windows-based servers implement this timer with a constant value of 45 seconds.
<219> Section 3.3.2.5:  Windows SMB2 servers set this timer to 35 seconds.
<220> Section 3.3.3:  Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, Windows Server 2012 R2, Windows 10 v1507 through Windows 10 v1703, and Windows Server 2016 set the ServerStartTime to the time at which the SMB2 server was started.
<221> Section 3.3.3: Windows-based SMB2 servers set this value to 256.
<222> Section 3.3.3: Windows-based SMB2 servers set this value to 1 MB.
<223> Section 3.3.3: Windows-based SMB2 servers set this value to 16 MB.
<224> Section 3.3.3: Windows-based servers initialize ServerHashLevel based on a stored value in the registry.
<225> Section 3.3.3: Windows 7 operating system and later and Windows Server 2008 R2 operating system and later SMB2 servers provide a constant maximum resiliency time-out of 300000 milliseconds.
<226> Section 3.3.3: Windows 8 operating system and later and Windows Server 2012 operating system and later by default, set RejectUnencryptedAccess to TRUE. If the registry value RejectUnencryptedAccess under HKLM\System\CurrentControlSet\Services\LanmanServer\Parameters\ is set to zero, RejectUnencryptedAccess is set to FALSE.
<227> Section 3.3.3: Windows 8 operating system and later and Windows Server 2012 operating system and later set IsMultiChannelCapable to TRUE.
<228> Section 3.3.3:  Windows 8 operating system and later and Windows Server 2012 operating system and later initialize AllowAnonymousAccess based on a stored value in the registry.
<229> Section 3.3.3: Windows 10 v1709, Windows Server operating system operating system and later set this value to TRUE.
<230> Section 3.3.3: By default, Windows 11 operating system and later and Windows Server 2022 operating system and later set AllowNamedPipeAccessOverQUIC to FALSE.
<231> Section 3.3.3:  Windows 11 with [MSKB-5035854], Windows 11 v22H2 with [MSKB-5035942], Windows Server 2022 with [MSKB-5035857], Windows Server 2022, 23H2, Windows 11, version 24H2 and later, and Windows Server 2025 and later set IsMutualAuthOverQUICSupported to TRUE.
Windows 11 with [MSKB-5035854], Windows 11 v22H2 with [MSKB-5035942], Windows Server 2022 with [MSKB-5035857], Windows Server 2022, 23H2 with [MSKB-5035856], Windows 11, version 24H2 and later, and Windows Server 2025 and later support Client Access Control capability over QUIC.
<232> Section 3.3.4.1.1: Windows-based servers always sign the final session setup response when the user is neither anonymous nor guest.
Windows 8, Windows Server 2012, Windows 8.1 without [MSKB-2976995] and Windows Server 2012 R2 without [MSKB-2976995] servers fail to sign responses other than SMB2_NEGOTIATE, SMB2_SESSION_SETUP, and SMB2_TREE_CONNECT when Session.SigningRequired is TRUE, global EncryptData is TRUE, RejectUnencryptedAccess is FALSE and either Connection.Dialect is "2.0.2" or "2.1" or Connection.ClientCapabilities does not include SMB2_GLOBAL_CAP_ENCRYPTION.
<233> Section 3.3.4.1.2: For an asynchronously processed request, Windows-based servers grant credits on the interim response and do not grant credits on the final response. The interim response grants credits to keep the transaction from stalling in case the client is out of credits.
<234> Section 3.3.4.1.3: The Windows-based server compounds responses for any received compounded operations. Otherwise, it does not compound responses.
<235> Section 3.3.4.1.3: When there are not enough credits to process a subsequent compounded request, Windows SMB2 servers set the NextCommand field to the size of the last SMB2 response message including the SMB2 header.
<236> Section 3.3.4.1.3: Windows-based servers grant all credits in the final response of the compounded chain, and grant 0 credits in all responses other than the final response.
<237> Section 3.3.4.1.3: Windows-based servers do not calculate the size of the response message; servers depend on the transport to send the response message.
<238> Section 3.3.4.1.5: Windows 10 v2004, Windows 10 v20H2, Windows Server v2004, and Windows Server v20H2 do not compress the message if Connection.CompressionIds does not include LZNT1, LZ77 and LZ77+Huffman algorithms.
<239> Section 3.3.4.2: Windows-based servers send interim responses for the following operations if they cannot be completed immediately:
SMB2_CREATE, if the underlying object store indicates an Oplock/Lease Break Notification or if access/sharing modes are incompatible with another existing open
SMB2_CHANGE_NOTIFY
Byte Range Lock
Named Pipe Read on a blocking named pipe
Named Pipe Write on a blocking named pipe
Large file write
FSCTL_PIPE_TRANSCEIVE
FSCTL_SRV_COPYCHUNK or FSCTL_SRV_COPYCHUNK_WRITE, when oplock break happens
SMB2 FLUSH on a named pipe
FSCTL_GET_DFS_REFERRALS
<240> Section 3.3.4.2: Windows-based servers incorrectly process the FSCTL_PIPE_WAIT request on named pipes synchronously.
<241> Section 3.3.4.2: Windows-based servers enforce a configurable blocking operation credit, which defaults to 64 on Windows Vista SP1 operating system and later, and defaults to 512 on Windows Server 2008 operating system and later.
<242> Section 3.3.4.4: For Windows 7 operating system and later and Windows Server 2008 R2 operating system and later, STATUS_BUFFER_OVERFLOW will be returned for FSCTL_GET_RETRIEVAL_POINTERS and FSCTL_GET_REPARSE_POINT, along with the ones mentioned in section 3.3.4.4.
<243> Section 3.3.4.6: In Windows-based SMB2 servers, underlying object store never breaks opportunistic lock to SMB2_OPLOCK_LEVEL_EXCLUSIVE oplock level.
<244> Section 3.3.4.6: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 set the SessionId in the SMB2 header to zero.
<245> Section 3.3.4.6: Windows-based SMB2 servers set Open.OplockTimeout to the current time plus 35000 milliseconds. If Open.IsPersistent is TRUE, Open.OplockTimeout is set to the current time plus 60000 milliseconds.
<246> Section 3.3.4.7: Windows-based SMB2 servers set Lease.LeaseBreakTimeout to the current time plus 35000 milliseconds. If Open.IsPersistent is TRUE, Windows 8 and Windows Server 2012 set Lease.LeaseBreakTimeout to the current time plus 60000 milliseconds. If Open.IsPersistent is TRUE, Windows 8.1 operating system and later and Windows Server 2012 R2 operating system and later set Lease.LeaseBreakTimeout to the current time plus 180000 milliseconds.
<247> Section 3.3.4.7:  Windows 8 through Windows 10 v1909, Windows Server 2012 through Windows Server v1909, Windows 10 v2004 through Windows 10, version 22H2 operating system without [MSKB-5037849], Windows Server v2004 through Windows Server v20H2 without [MSKB-5037849], Windows 11 without [MSKB-5039213], Windows 11 v22H2 without [MSKB-5037853], Windows 11, version 23H2 without [MSKB-5037853], Windows 11, version 24H2 without [MSKB-5040529], Windows Server 2022 without [MSKB-5039227] and Windows Server 2022, 23H2 without [MSKB-5039236] and later do not increment Lease.Epoch when setting NewEpoch in Lease Break Notification in the following cases:
On the server initiated close of an open which is the last open in Open.Lease.LeaseOpens, the server sends a Lease Break Notification to break the lease by setting NewLeaseState to SMB2_LEASE_NONE and Status field in the SMB2 Header to STATUS_FILE_CLOSED.
While handling a Lease Break Acknowledgment, due to a conflicting open, if the object store does not grant WRITE_CACHING or HANDLE_CACHING, as specified in [MS-FSA] section 2.1.5.19, the server sends another Lease Break Notification to further downgrade the lease state.
<248> Section 3.3.4.13: Windows Server 2012 and Windows Server 2012 R2 set these bits as appropriate for shared volume configurations.
<249> Section 3.3.4.13: By default, Windows 8 operating system and later and Windows Server 2012 operating system and later set Share.CATimeout to zero.
<250> Section 3.3.4.17: Windows Lease break is described in [MS-FSA] section 2.1.5.18. The Open parameter passed is the Open.Local value from the current close operation, the Type parameter is LEVEL_GRANULAR to indicate a Lease request, and the RequestedOplockLevel parameter is zero.
Windows servers never send SMB2 Lease Break Notification to the client when the Open is being closed.
<251> Section 3.3.4.21: For each supported transport type as listed in section 2.1, the Windows SMB2 server attempts to form an association with the specified device with local calls specific to each supported transport type and rejects the entry if none of the associations succeed.
<252> Section 3.3.4.21: On Windows, ServerName is used only when the transport is NetBIOS over TCP.
<253> Section 3.3.5.1: Possible Windows-specific values for Connection.TransportName are listed in a product behavior note attached to [MS-SRVS] section 2.2.4.96.
<254> Section 3.3.5.2: Windows performs cancellation of in-progress requests via the interface in [MS-FSA] section 2.1.5.20, Server Requests Canceling an Operation, passing Request.CancelRequestId as an input parameter.
<255> Section 3.3.5.2:  Windows 10 v1903 and later, and Windows Server v1903 and later set this to TRUE.
<256> Section 3.3.5.2: Windows 7 without [MSKB-2536275], and Windows Server 2008 R2 without [MSKB-2536275] terminate the connection when the size of the request is greater than 64*1024 bytes.
Windows Vista SP1 and Windows Server 2008 on Direct TCP transport disconnect the connection if the size of the message exceeds 128*1024 bytes, and Windows Vista SP1 and Windows Server 2008 on NetBIOS over TCP transport will disconnect the connection if the size of the message exceeds 64*1024 bytes.
<257> Section 3.3.5.2.1.1: Windows-based servers will discard the message if it is encrypted and the connection is NetBIOS over TCP.
<258> Section 3.3.5.2.1.1: Windows-based servers will not disconnect the connection.
<259> Section 3.3.5.2.1.1: Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 disconnect the connection if OriginalMessageSize is greater than 1028 kilobytes.
<260> Section 3.3.5.2.1.2: Windows-based servers discard the message if it is compressed and the connection is over RDMA.
<261> Section 3.3.5.2.3: For an SMB2 Write request with an invalid MessageId, Windows 8 and Windows Server 2012 will stop processing the request and any further requests on that connection.
<262> Section 3.3.5.2.4: Windows-based servers will not disconnect the connection due to a mismatched signature.
<263> Section 3.3.5.2.4: Windows-based servers will not disconnect the connection due to an unsigned packet.
<264> Section 3.3.5.2.6: Windows-based servers will disconnect the connection when it processes packets that are smaller than the SMB2 header or packets that contain an invalid SMB2 command. For all other validations, it will not disconnect the connection but simply return the error.
<265> Section 3.3.5.2.7: In Windows Vista and later, and Windows Server 2008 and later, when an operation in a compound request requires asynchronous processing, Windows-based servers fail them with STATUS_INTERNAL_ERROR except for the following two cases: when a create request in the compound request triggers an oplock break, or when the operation is last in the compound request.
In all SMB2 servers, if a create request in a compound chain is processed asynchronously due to an oplock break, Windows-based servers send an interim response to the client. If there are one or more conflicting create operations in a compounded request, Windows-based servers send an oplock break notification for the completed create prior to sending any response, and the level of the broken oplock is not updated in all prior create responses in the compound response.
<266> Section 3.3.5.2.7: Windows-based servers ignore 8-byte alignment boundary checking in a compounded chain.
<267> Section 3.3.5.2.7: Windows-based SMB2 servers allow a mix of related and unrelated compound requests in the same transport send. Upon encountering a request with SMB2_FLAGS_RELATED_OPERATIONS not set, a Windows-based SMB2 server treats it as the start of a chain.
<268> Section 3.3.5.2.7.2: If SMB2_FLAGS_RELATED_OPERATIONS is present in the first request, Windows-based servers fail all related requests in the compounded chain with error STATUS_INVALID_PARAMETER.
<269> Section 3.3.5.2.7.2: If the previous session expired, Windows Vista SP1, Windows Server 2008, Windows 7, and Windows Server 2008 R2 servers fail the next request in the compounded chain with STATUS_NETWORK_SESSION_EXPIRED, and the subsequent requests in the compounded chain will be failed with STATUS_INVALID_PARAMETER.
<270> Section 3.3.5.2.9: Windows Vista SP1, Windows Server 2008, Windows 7, and Windows Server 2008 R2 servers do not fail the request if the SMB2 header of the request has SMB2_FLAGS_SIGNED set in the Flags field and the request is not an SMB2 LOCK request as specified in section 2.2.26.
<271> Section 3.3.5.2.9: Windows-based servers fail the request with 0x80090302 when the authentication method is GSS-API.
<272> Section 3.3.5.2.10: Windows 8 and Windows Server 2012 perform the following:
If Open.OutstandingPreRequestCount is equal to zero,
Set Open.ChannelSequence to ChannelSequence in the SMB2 Header.
Increment Open.OutstandingPreRequestCount by Open.OutstandingRequestCount.
Set Open.OutstandingRequestCount to 1.
Otherwise, fail the request with STATUS_FILE_NOT_AVAILABLE.
<273> Section 3.3.5.3.1: If the underlying transport is NETBIOS over TCP, Windows-based servers set MaxTransactSize to 65536. Otherwise, MaxTransactSize is set based on the following table.
<274> Section 3.3.5.3.1: If the underlying transport is NETBIOS over TCP, Windows-based servers set MaxReadSize to 65536. Otherwise, MaxReadSize is set based on the following table.
<275> Section 3.3.5.3.1: If the underlying transport is NETBIOS over TCP, Windows-based servers set MaxWriteSize to 65536. Otherwise, MaxWriteSize is based on the following table.
<276> Section 3.3.5.3.1: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, Windows Server 2012 R2, Windows 10 v1507 through Windows 10 v1703, and Windows Server 2016 set the ServerStartTime to the global ServerStartTime value.
<277> Section 3.3.5.3.2: Windows-based servers set this to a default value of 65536.
<278> Section 3.3.5.3.2: Windows-based servers set MaxReadSize to a default value of 65536.
<279> Section 3.3.5.3.2: Windows-based servers set MaxWriteSize to a default value of 65536.
<280> Section 3.3.5.3.2: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, Windows Server 2012 R2, Windows 10 v1507 through Windows 10 v1703, and Windows Server 2016 set the ServerStartTime to the global ServerStartTime value.
<281> Section 3.3.5.4: Windows 10 v1903, Windows 10 v1909, Windows Server v1903, and Windows Server v1909 only set CompressionAlgorithms to the first common algorithm supported by the client and server.
Windows 10 v2004 and Windows Server v2004 select a common pattern scanning algorithm and the first common compression algorithm, specified in section 2.2.3.1.3, supported by the client and server.
<282> Section 3.3.5.4: If the underlying transport is NETBIOS over TCP, Windows-based servers set MaxTransactSize to 65536. Otherwise, MaxTransactSize is set based on the following table.
<283> Section 3.3.5.4: If the underlying transport is NETBIOS over TCP, Windows-based servers set MaxReadSize to 65536. Otherwise, MaxReadSize is set based on the following table.
<284> Section 3.3.5.4: If the underlying transport is NETBIOS over TCP, Windows-based servers set MaxWriteSize to 65536. Otherwise, MaxWriteSize is set based on the following table.
<285> Section 3.3.5.4:  Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, Windows Server 2012 R2, Windows 10 v1507 through Windows 10 v1703, and Windows Server 2016 set the ServerStartTime to the global ServerStartTime value.
<286> Section 3.3.5.4: Windows 10, and Windows Server 2016 operating system and later use 32 bytes of Salt.
<287> Section 3.3.5.4: Windows 10 v2004, Windows Server v2004, Windows 10 v20H2, Windows Server v20H2, and Windows 10 v21H1 operating system operating systems without [MSKB-5001391] set CompressionAlgorithmCount to 0.
<288> Section 3.3.5.4: Windows 10 v2004, Windows Server v2004, Windows 10 v20H2, Windows Server v20H2, and Windows 10 v21H1 operating systems without [MSKB-5001391] set CompressionAlgorithms to empty.
<289> Section 3.3.5.5: Windows 8 and Windows Server 2012 look up the session in GlobalSessionTable using the SessionId from the SMB2 header if the SMB2_SESSION_FLAG_BINDING bit is set in the Flags field of the request. If the session is found, the server fails the request with STATUS_REQUEST_NOT_ACCEPTED. If the session is not found, the server fails the request with STATUS_USER_SESSION_DELETED.
<290> Section 3.3.5.5: Windows Vista SP1 and Windows Server 2008 servers fail the session setup request with STATUS_REQUEST_NOT_ACCEPTED.
<291> Section 3.3.5.5.3: Windows Vista SP1 operating system and later and Windows Server 2008 operating system and later will also accept raw Kerberos messages and implicit NTLM messages as part of GSS authentication.
<292> Section 3.3.5.5.3: Windows by default uses the guest account to represent guest users. Alternatively, any user account that is a member of the well-known BUILTIN_GUESTS or DOMAIN_GUESTS group (see [MS-DTYP] section 2.4.2.4) is considered a guest account.
<293> Section 3.3.5.5.3: Windows 7 and Windows Server 2008 R2 remove the current session from GlobalSessionTable and Connection.SessionTable but the SESSION_SETUP request succeeds, if the PreviousSessionId and SessionId values in the SMB2 header of the request are equal and the authentications were for the same user. Further requests using this SessionId will fail with STATUS_USER_SESSION_DELETED.
<294> Section 3.3.5.6: Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 servers will not reset ResilientOpenScavengerExpiryTime.
<295> Section 3.3.5.7: Windows-based SMB2 servers do not set this bit in the ShareFlags field.
<296> Section 3.3.5.7: Windows-based SMB2 servers do not set this bit in the ShareFlags field.
<297> Section 3.3.5.7: Windows Server 2012 and Windows Server 2012 R2 set these two bits based on group policy settings.
<298> Section 3.3.5.7: Windows Vista SP1 and Windows Server 2008 do not support the SMB2_SHAREFLAG_ENABLE_HASH_V1 bit.
<299> Section 3.3.5.7: Windows Server v1709 and later support the SMB2_SHARE_CAP_REDIRECT_TO_OWNER bit.
<300> Section 3.3.5.9:  If Open.ClientGuid is not equal to the ClientGuid of the connection that received this request, Open.Lease.LeaseState is equal to RWH, or Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_BATCH, Windows-based servers will attempt to break the lease/oplock and return STATUS_PENDING to process the create request asynchronously. Otherwise, if Open.Lease.LeaseState does not include SMB2_LEASE_HANDLE_CACHING and Open.OplockLevel is not equal to SMB2_OPLOCK_LEVEL_BATCH, Windows-based servers return STATUS_FILE_NOT_AVAILABLE.
<301> Section 3.3.5.9: Windows Vista and Windows Server 2008 validate the create requests before session verification as described in the "Create Context Validation" phase in section 3.3.5.9.
<302> Section 3.3.5.9: Windows-based servers accept the path names containing Dot Directory Names specified in [MS-FSCC] section 2.1.5.1 and attempt to normalize the path name by removing the pathname components of "."  and "..". Windows-based servers fail the CREATE request with STATUS_INVALID_PARAMETER if the file name in the Buffer field of the request begins in the form "subfolder\..\", for example "x\..\y.txt".
<303> Section 3.3.5.9: Windows-based SMB2 servers fail an SMB2 CREATE request with STATUS_ACCESS_DENIED if the file name in the request is one of the following: "LPT1", "LPT2", "LPT3","LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "PRN", "AUX", "NUL", "CON", and "CLOCK$".
<304> Section 3.3.5.9: Windows-based servers ignore DesiredAccess values other than FILE_WRITE_DATA, FILE_APPEND_DATA and GENERIC_WRITE if any one of these values is specified.
<305> Section 3.3.5.9: Windows-based servers fail requests having a CreateDisposition of FILE_OPEN or FILE_OVERWRITE, but ignore values of FILE_SUPERSEDE, FILE_OPEN_IF and FILE_OVERWRITE_IF.
<306> Section 3.3.5.9:  Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 do not perform this verification and continue to process the request.
<307> Section 3.3.5.9: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, and Windows Server 2012 do not perform this verification.
<308> Section 3.3.5.9: Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2 operating systems do not perform this verification and continue to process the request.
<309> Section 3.3.5.9: Windows-based SMB2 servers check only for FILE_WRITE_DATA, FILE_WRITE_ATTRIBUTES, FILE_WRITE_EA, and FILE_APPEND_DATA in the DesiredAccess field.
<310> Section 3.3.5.9: Windows performs the access check by mapping SMB2 parameters to the object store parameters as described in [MS-FSA] section 2.1.4.14 AccessCheck -- Algorithm to Perform a General Access Check.
<311> Section 3.3.5.9: Windows Vista SP1 and Windows Server 2008 do not support the SMB2_SHAREFLAG_FORCE_LEVELII_OPLOCK flag and ignore the TreeConnect.Share.ForceLevel2Oplock value.
<312> Section 3.3.5.9: Windows performs the following open/create mappings from SMB2 parameters to the object store as described in [MS-FSA] section 2.1.5.1 Server Requests an Open of a File.
Windows performs the following mappings from object store results to SMB2 response.
<313> Section 3.3.5.9: Windows-based servers will receive the data from the local create operation for constructing the error response when a symbolic link is present in the target path name.
<314> Section 3.3.5.9: Windows Oplock acquisition is described in [MS-FSA] section 2.1.5.18. Oplock acquisition is an optional step in open/create processing; the Open parameter passed is the Open.Local result from the open or create operation, and the Type parameter is mapped as follows.
The Status code returned indicates whether the requested oplock was granted.
<315> Section 3.3.5.9: Windows obtains CreationTime from the object store FileBasicInformation [MS-FSA] section 2.1.5.12.6 and [MS-FSCC] section 2.4.7.
<316> Section 3.3.5.9: Windows obtains LastAccessTime from the object store FileBasicInformation [MS-FSA] section 2.1.5.12.6 and [MS-FSCC] section 2.4.7.
<317> Section 3.3.5.9: Windows obtains LastWriteTime from the object store FileBasicInformation [MS-FSA] section 2.1.5.12.6 and [MS-FSCC] section 2.4.7.
<318> Section 3.3.5.9: Windows obtains ChangeTime from the object store FileBasicInformation [MS-FSA] section 2.1.5.12.6 and [MS-FSCC] section 2.4.7.
<319> Section 3.3.5.9: Windows obtains AllocationSize from the object store FileStandardInformation [MS-FSA] section 2.1.5.12.27 and [MS-FSCC] section 2.4.47.
<320> Section 3.3.5.9: Windows-based SMB2 servers will set AllocationSize to any value for the named pipe.
<321> Section 3.3.5.9: Windows obtains EndOfFile from the object store FileStandardInformation [MS-FSA] section 2.1.5.12.27 and [MS-FSCC] section 2.4.47.
<322> Section 3.3.5.9: Windows-based SMB2 servers will set EndofFile to any value for the named pipe.
<323> Section 3.3.5.9: Windows obtains FileAttributes from the object store FileBasicInformation [MS-FSA] section 2.1.5.12.6 and [MS-FSCC] section 2.4.7.
<324> Section 3.3.5.9.1: Windows sets extended attributes on a newly created file with the FSCTL_SET_OBJECT_ID_EXTENDED FSCTL [MS-FSA] section 2.1.5.10.36 and [MS-FSCC] section 2.3.81.
<325> Section 3.3.5.9.2: Windows sets security attributes on a newly created file with the Application requests setting of security information [MS-FSA] section 2.1.5.17.
<326> Section 3.3.5.9.2: Windows will ignore security descriptors if the underlying object store does not support them.
<327> Section 3.3.5.9.3: Windows-based servers support this request.
<328> Section 3.3.5.9.3: Windows sets allocation size on a newly created file with the FileAllocationInformation [MS-FSA] section 2.1.5.15.1 and [MS-FSCC] section 2.4.4, after converting bytes to volume cluster size.
<329> Section 3.3.5.9.4: Windows validates that a snapshot with the time stamp provided exists by forming a FileBothDirectoryInformation object store request for the file including the provided @GMT token in the path, as described in [MS-SMB] section 2.2.1.1.1 and [MS-FSA] section 2.1.5.6.3.1.
<330> Section 3.3.5.9.4: Windows opens a file on a snapshot with the time stamp provided by the file including the provided @GMT token in the path, as described in [MS-SMB] section 2.2.1.1.1 and [MS-FSA] section 2.1.5.1.
<331> Section 3.3.5.9.5: Windows computes the MaximalAccess to return by querying the security attributes of the file with [MS-FSA] section 2.1.5.14, and performing an access check against the credentials provided by the request. QueryStatus is set to the Status returned in that operation.
<332> Section 3.3.5.9.6: Windows Vista SP1, Windows 7, Windows Server 2008, and Windows Server 2008 R2 ignore undefined create contexts.
<333> Section 3.3.5.9.6:  Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2 set Open.DurableOpenTimeout to 16 minutes. Windows 8, Windows Server 2012, Windows 8.1, Windows Server 2012 R2, Windows 10, Windows Server 2016, and Windows Server set Open.DurableOpenTimeout to 2 minutes.
<334> Section 3.3.5.9.7: Windows Vista SP1, Windows Server 2008, Windows 7 and Windows Server 2008 R2 ignore undefined create contexts.
<335> Section 3.3.5.9.7: If the Session was established by invalidating the previous session by specifying PreviousSessionId in the SMB2 SESSION_SETUP request, Windows 8.1 and Windows Server 2012 R2 close the durable opens established on the previous session.
<336> Section 3.3.5.9.7: Windows 8, Windows Server 2012, Windows 8.1 and Windows Server 2012 R2 do not perform lease version verification.
<337> Section 3.3.5.9.7: Windows Vista SP1, Windows Server 2008, Windows 7, and Windows Server 2008 R2 servers respond with the SMB2_CREATE_DURABLE_HANDLE_RESPONSE create context after a successful reconnect of a durable open.
<338> Section 3.3.5.9.8: Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 do not ignore the SMB2_CREATE_REQUEST_LEASE create context when RequestedOplockLevel is not equal to SMB2_OPLOCK_LEVEL_LEASE.
<339> Section 3.3.5.9.8: On Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2, the Lease.ClientLeaseId is passed to the object store when processing continues at open/create time. A new or existing lease is thereby associated with the resulting open.
To acquire or promote the lease as dictated by the SMB2_CREATE_REQUEST_LEASE Create Context, a subsequent object store call is invoked as described in [MS-FSA] section 2.1.5.18. The Open parameter passed is an internally-managed open that refers to the same file, stream, and oplock key as Open.LocalOpen but is otherwise distinct from Open.LocalOpen, and the Type parameter is LEVEL_GRANULAR to indicate a Lease request. The RequestedOplockLevel parameter is constructed to include zero or more bits as follows.
The Status code returned indicates whether the requested lease was granted.
<340> Section 3.3.5.9.10:  Windows-based servers send the SMB2_CREATE_DURABLE_HANDLE_RESPONSE_V2 response create context to the client if any of the following conditions is satisfied:
Open.IsPersistent is TRUE
Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_BATCH
Open.Lease.LeaseState contains SMB2_LEASE_HANDLE_CACHING
<341> Section 3.3.5.9.10:  If the Timeout value in the request is not zero, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 SMB2 servers set Timeout to the Timeout value in the request.
<342> Section 3.3.5.9.10:  If the Timeout value in the request is zero and Share.CATimeout is not zero, Windows 8, Windows Server 2012, Windows 8.1, Windows Server 2012 R2, Windows 10, Windows Server 2016, and Windows Server SMB2 servers set Timeout to Share.CATimeout. If the Timeout value in the request is zero and Share.CATimeout is zero, Windows 8 and Windows Server 2012 SMB2 servers set Timeout to 60 seconds.
<343> Section 3.3.5.9.11: Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 servers do not ignore the SMB2_CREATE_REQUEST_LEASE_V2 create context when Connection.Dialect is equal to "2.1" or if RequestedOplockLevel is not equal to SMB2_OPLOCK_LEVEL_LEASE.
<344> Section 3.3.5.9.11:  On Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2, the Lease.ClientLeaseId and Lease.ParentLeaseKey are passed to the object store in the form of TargetOplockKey and ParentOplockKey. A new or existing lease is thereby associated with the resulting open.
To acquire or promote the lease as dictated by the SMB2_CREATE_REQUEST_LEASE_V2 Create Context, a subsequent object store call is invoked as described in [MS-FSA] section 2.1.5.18 Server Requests an Oplock. The Open parameter passed is an internally-managed open that refers to the same file, stream, and oplock key as Open.LocalOpen but is otherwise distinct from Open.LocalOpen, and the Type parameter is LEVEL_GRANULAR to indicate a Lease request. The RequestedOplockLevel field is constructed to include zero or more bits as follows.
The Status code returned indicates whether the requested lease was granted.
<345> Section 3.3.5.9.12: Windows 8 with [KB2770917] and Windows Server 2012 with [KB2770917] fail the CREATE request with STATUS_INVALID_PARAMETER.
<346> Section 3.3.5.9.12: If the Session was established by specifying PreviousSessionId in the SMB2 SESSION_SETUP request, therefore invalidating the previous session, Windows 8.1 and Windows Server 2012 R2 close the durable opens established on the previous session.
<347> Section 3.3.5.9.12: If Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_BATCH or Open.Lease.LeaseState includes SMB2_LEASE_HANDLE_CACHING, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 continue to process the request.
<348> Section 3.3.5.9.12: Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 do not perform Lease version verification.
<349> Section 3.3.5.9.12:  Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 do not perform this verification and continue to process the request.
<350> Section 3.3.5.9.12: When an open, with Open.IsPersistent set to TRUE, is being reconnected due to server failover, Windows Server 2012 and later perform the following:
If Lease.LeaseState includes SMB2_LEASE_WRITE_CACHING, Epoch and Lease.Epoch are set to Epoch field in the Create Context request.
If Lease.LeaseState does not include SMB2_LEASE_WRITE_CACHING, Epoch and Lease.Epoch are set to Epoch field in the Create Context request incremented by 1.
<351> Section 3.3.5.9.13: Windows SMB3 servers compute the maximal access to return by querying the security attributes of the file with [MS-FSA] section 2.1.5.14, and performing an access check against the credentials provided by the request.
<352> Section 3.3.5.9.13:  Windows Server 2012 and Windows Server 2012 R2 servers do not close the open.
<353> Section 3.3.5.10: Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2 validate the open before verifying the session.
<354> Section 3.3.5.10: Windows obtains FileNetworkOpenInformation from the object store as described in [MS-FSA] section 2.1.5.12.21 and [MS-FSCC] section 2.4.34.
Windows-based servers do not return an updated ChangeTime unless Open.GrantedAccess includes FILE_WRITE_DATA, FILE_WRITE_ATTRIBUTES, FILE_WRITE_EA, or FILE_APPEND_DATA and any prior WRITE/SET_INFO operations were performed on that Open.
<355> Section 3.3.5.11: Windows flushes any cached data to the file with Server Requests Flushing Cached Data [MS-FSA] section 2.1.5.7.
<356> Section 3.3.5.11: If the request target is a named pipe or file, Windows-based servers handle this request asynchronously.
<357> Section 3.3.5.12: Windows 7 and Windows Server 2008 R2 fail the request with STATUS_BUFFER_OVERFLOW if the Length field is greater than Connection.MaxReadSize. Windows Vista SP1 and Windows Server 2008 will fail the request with STATUS_BUFFER_OVERFLOW if the Length field is greater than 524288.
<358> Section 3.3.5.12: Windows reads from a file with Server Requests a Read [MS-FSA] section 2.1.5.3.
<359> Section 3.3.5.12: Windows SMB2 servers send an interim response to the client and handle the read asynchronously if the read is not finished in 0.5 milliseconds.
<360> Section 3.3.5.12: Windows-based servers handle the following commands asynchronously: SMB2 Create (section 2.2.13) when this create would result in an oplock break, SMB2 IOCTL Request (section 2.2.31) for FSCTL_PIPE_TRANSCEIVE if it blocks for more than 1 millisecond, SMB2 IOCTL Request for FSCTL_SRV_COPYCHUNK or FSCTL_SRV_COPYCHUNK_WRITE (section 2.2.31) when oplock break happens, SMB2 Change_Notify Request (section 2.2.35) if it blocks for more than 0.5 milliseconds, SMB2 Read request (section 2.2.19) for named pipes if it blocks for more than 0.5 milliseconds, SMB2 Write request (section 2.2.21) for named pipes if it blocks for more than 0.5 milliseconds, SMB2 Write Request (section 2.2.21) for large file write, SMB2 lock request (section 2.2.26) if the SMB2_LOCKFLAG_FAIL_IMMEDIATELY flag is not set, and SMB2 FLUSH Request (section 2.2.17) for named pipes.
<361> Section 3.3.5.13: Windows SMB2 servers allow the operation when either FILE_APPEND_DATA or FILE_WRITE_DATA is set in Open.GrantedAccess.
<362> Section 3.3.5.13: Windows 7 and Windows Server 2008 R2 fail the request with STATUS_BUFFER_OVERFLOW instead of STATUS_INVALID_PARAMETER if the Length field is greater than Connection.MaxWriteSize. Windows Vista SP1 and Windows Server 2008 do not validate the Length field in SMB2 Write Request.
<363> Section 3.3.5.13: If the Flags field contains any bit values other than those specified in section 2.2.21, Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, and Windows Server 2012 fail the request with STATUS_INVALID_PARAMETER.
<364> Section 3.3.5.13: Windows writes to a file with Server Requests a Write [MS-FSA] section 2.1.5.4.
<365> Section 3.3.5.13: Windows-based servers handle the following commands asynchronously:
SMB2 CREATE Request (section 3.3.5.9) when this create would result in an oplock break.
SMB2 IOCTL Request (section 3.3.5.15) for FSCTL_PIPE_TRANSCEIVE if it blocks for more than 1 millisecond. For FSCTL_SRV_COPYCHUNK or FSCTL_SRV_COPYCHUNK_WRITE, when an oplock break happens.
SMB2 CHANGE_NOTIFY Request (section 3.3.5.19) if it blocks for more than 0.5 milliseconds.
SMB2 READ Request (section 3.3.5.12) for named pipes if it blocks for more than 0.5 milliseconds.
SMB2 WRITE Request (section 3.3.5.13) for named pipes if it blocks for more than 0.5 milliseconds.
SMB2 WRITE Request (section 3.3.5.13) for large file write.
SMB2 LOCK Request (section 3.3.5.14) if the SMB2_LOCKFLAG_FAIL_IMMEDIATELY flag is not set.
SMB2 FLUSH Request (section 3.3.5.11) for named pipes.
<366> Section 3.3.5.14: Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2 validate the open before verifying the session.
<367> Section 3.3.5.14:  Windows 7 and Windows Server 2008 R2 perform lock sequence verification only when Open.IsResilient is TRUE.
Windows 8 through Windows 10 v1909 and Windows Server 2012 through Windows Server v1909 perform lock sequence verification only when Open.IsResilient or Open.IsPersistent is TRUE.
<368> Section 3.3.5.14.1: Windows-based servers ignore this value while processing Unlocks.
<369> Section 3.3.5.14.1: Windows processes unlock with Server Requests unlock of a Byte-Range [MS-FSA] section 2.1.5.9.
<370> Section 3.3.5.14.2: Windows-based servers check for SMB2_LOCKFLAG_FAIL_IMMEDIATELY only for the first element of the Locks array.
<371> Section 3.3.5.14.2: Refer to [FSBO] for implementation-specific details of how byte range locks can be implemented.
<372> Section 3.3.5.14.2: Windows processes lock with Server Requests a Byte-Range Lock [MS-FSA] section 2.1.5.8.
<373> Section 3.3.5.15: Windows Vista SP1 and Windows Server 2008 SMB2 servers fail an IOCTL request with STATUS_INVALID_PARAMETER if [ max(InputCount, MaxInputResponse) + max(OutputCount, MaxOutputResponse) ] is greater than 262144.
<374> Section 3.3.5.15: Windows 8 and later and Windows Server 2012 and later do not fail the request.
<375> Section 3.3.5.15: Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2 fail the request with STATUS_INVALID_PARAMETER in the following cases:
If OutputCount is not equal to zero and OutputOffset is greater than zero but less than (size of SMB2 header + size of the SMB2 IOCTL request not including Buffer).
If OutputCount is not equal to zero and OutputOffset is greater than size of SMB2 Message.
If OutputCount is not equal to zero and OutputOffset is not rounded up to a multiple of 8 bytes.
If (OutputOffset + OutputCount) is greater than size of SMB2 Message.
If OutputCount is greater than zero and OutputOffset is less than (InputOffset + InputCount).
Windows 7 and Windows Server 2008 R2 fail the request with STATUS_INVALID_PARAMETER if OutputOffset or OutputCount is greater than size of SMB2 Message.
<376> Section 3.3.5.15: Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 SMB2 servers copy the OutputCount bytes into the output buffer for the following FSCTLs:
FSCTL_GET_RETRIEVAL_POINTERS
FSCTL_GET_REPARSE_POINT
FSCTL_PIPE_TRANSCEIVE
FSCTL_PIPE_PEEK
FSCTL_DFS_GET_REFERRALS
Windows Vista SP1 and Windows Server 2008 SMB2 servers copy the OutputCount bytes into the output buffer for the following FSCTLs:
FSCTL_PIPE_TRANSCEIVE
FSCTL_PIPE_PEEK
FSCTL_DFS_GET_REFERRALS
All other FSCTL commands will be failed with error STATUS_BUFFER_OVERFLOW through error response specified in section 2.2.2.
<377> Section 3.3.5.15: Windows 8 and later and Windows Server 2012 and later allow only the CtlCode values, as specified in section 2.2.31, and the following CtlCode values, as specified in [MS-FSCC] section 2.3.
Windows 8.1 and later and Windows Server 2012 R2 and later allow these additional CtlCode values, as specified in [MS-RSVD].
Windows 10 and later and Windows Server 2016 and later allow the additional CtlCode value, as specified in [MS-RSVD].
Windows 10 and later and Windows Server 2016 and later allow the additional CtlCode value, as specified in [MS-FSCC].
Windows 10 v1607 operating system and later and Windows Server 2016 operating system and later allow the additional CtlCode value, as specified in [MS-FSCC].
Windows 10 v1803 operating system and later and Windows Server v1803 operating system and later allow the additional CtlCode value, as specified in [MS-FSCC].
Windows 10 and later and Windows Server 2016 and later allow the additional CtlCode value, as specified in [MS-SQOS].
Windows 11 operating system and later and Windows Server 2022 operating system and later allow the additional CtlCode value, as specified in [MS-FSCC].
Windows 11 and later and Windows Server 2022 and later allow the additional CtlCode value, as specified in [MS-FSCC].
<378> Section 3.3.5.15: For the following FSCTLs, Windows Vista SP1, Windows Server 2008, Windows 7, and Windows Server 2008 R2 return STATUS_FILE_CLOSED instead of STATUS_INVALID_DEVICE_REQUEST:
FSCTL_QUERY_NETWORK_INTERFACE_INFO
FSCTL_DFS_GET_REFERRALS_EX
FSCTL_VALIDATE_NEGOTIATE_INFO
<379> Section 3.3.5.15.1: If MaxOutputResponse is not 16 bytes, Windows-based servers do not refresh the snapshots.
<380> Section 3.3.5.15.1: Windows-based SMB2 servers will place two extra bytes set to zero in the SnapShots array and set SnapShotArraySize to two, if NumberOfSnapShots is zero.
<381> Section 3.3.5.15.2: A Windows-based DFS server does not return any data to the caller if the buffer supplied to FSCTL_GET_DFS_REFERRALS is too small.
<382> Section 3.3.5.15.3: Windows-based servers return STATUS_INVALID_DEVICE_REQUEST if the FSCTL_PIPE_TRANSCEIVE being executed is not a named pipe share.
<383> Section 3.3.5.15.3: Windows SMB2 servers send an interim response to the client if the read/write attempt is not finished in 1 millisecond.
<384> Section 3.3.5.15.3: Some Windows–based SMB2 servers return the input buffer that was received in the request as part of the response. Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 will not return the input buffer that was received in the request, and the InputCount field is always zero. Windows Vista SP1 and Windows Server 2008 will send back the input buffer based on the InputOffset and InputCount fields indicated in the request.
<385> Section 3.3.5.15.3: Windows–based SMB2 servers set OutputOffset to InputOffset + InputCount, rounded up to a multiple of 8.
<386> Section 3.3.5.15.4: Windows-based servers return STATUS_INVALID_DEVICE_REQUEST, if FSCTL_PIPE_PEEK request being executed is not a named pipe share.
<387> Section 3.3.5.15.4: Windows SMB2 servers will set OutputOffset to InputOffset + InputCount, rounded up to a multiple of 8.
<388> Section 3.3.5.15.5: Windows-based servers do not support any additional contexts.
<389> Section 3.3.5.15.5: Windows-based servers construct the 24-byte blob using Open.DurableFileId and other pieces of information which include the process ID of the caller and a timestamp.
<390> Section 3.3.5.15.6: Windows Vista SP1, Windows Server 2008, Windows 7, and Windows Server 2008 R2 do not verify byte-range locks on both source and destination files.
<391> Section 3.3.5.15.7: Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 servers support the FSCTL_SRV_READ_HASH request.
<392> Section 3.3.5.15.7: When the branch cache feature is available and the file size is less than 65,536 bytes, Windows servers fail the request with STATUS_HASH_NOT_PRESENT.
<393> Section 3.3.5.15.7:  Windows-based servers set the FileDataOffset field to the starting offset from the segment covering the Offset requested in the SRV_READ_HASH request.
<394> Section 3.3.5.15.8: The following FSCTLs are explicitly blocked by Windows-based SMB2 server and are not passed through to the object store. They are failed with STATUS_NOT_SUPPORTED.
FSCTL_REQUEST_OPLOCK_LEVEL_1 (0x00090000)
FSCTL_REQUEST_OPLOCK_LEVEL_2 (0x00090004)
FSCTL_REQUEST_BATCH_OPLOCK (0x00090008)
FSCTL_REQUEST_FILTER_OPLOCK (0x0009005C)
FSCTL_OPLOCK_BREAK_ACKNOWLEDGE (0x0009000C)
FSCTL_OPBATCH_ACK_CLOSE_PENDING (0x00090010)
FSCTL_OPLOCK_BREAK_NOTIFY (0x00090014)
FSCTL_MOVE_FILE (0x00090074)
FSCTL_QUERY_RETRIEVAL_POINTERS (0x0009003B)
FSCTL_PIPE_ASSIGN_EVENT (0x00110000)
FSCTL_GET_VOLUME_BITMAP (0x0009006F)
FSCTL_GET_NTFS_FILE_RECORD (0x00090068)
FSCTL_INVALIDATE_VOLUMES (0x00090054)
FSCTL_READ_USN_JOURNAL (0x000900BB)
FSCTL_CREATE_USN_JOURNAL (0x000900E7)
FSCTL_QUERY_USN_JOURNAL (0x000900F4)
FSCTL_DELETE_USN_JOURNAL (0x000900F8)
FSCTL_ENUM_USN_DATA (0x000900B3)
FSCTL_QUERY_DEPENDENT_VOLUME (0x000901F0)
FSCTL_SD_GLOBAL_CHANGE (0x000901F4)
FSCTL_GET_BOOT_AREA_INFO (0x00090230)
FSCTL_GET_RETRIEVAL_POINTER_BASE (0x00090234)
FSCTL_SET_PERSISTENT_VOLUME_STATE (0x00090238)
FSCTL_QUERY_PERSISTENT_VOLUME_STATE (0x0009023C)
FSCTL_REQUEST_OPLOCK (0x00090240)
FSCTL_TXFS_MODIFY_RM (0x00098144)
FSCTL_TXFS_QUERY_RM_INFORMATION (0x00094148)
FSCTL_TXFS_ROLLFORWARD_REDO (0x00098150)
FSCTL_TXFS_ROLLFORWARD_UNDO (0x00098154)
FSCTL_TXFS_START_RM (0x00098158)
FSCTL_TXFS_SHUTDOWN_RM (0x0009815C)
FSCTL_TXFS_READ_BACKUP_INFORMATION (0x00094160)
FSCTL_TXFS_WRITE_BACKUP_INFORMATION (0x00098164)
FSCTL_TXFS_CREATE_SECONDARY_RM (0x00098168)
FSCTL_TXFS_GET_METADATA_INFO (0x0009416C)
FSCTL_TXFS_GET_TRANSACTED_VERSION (0x00094170)
FSCTL_TXFS_SAVEPOINT_INFORMATION (0x00098178)
FSCTL_TXFS_CREATE_MINIVERSION (0x0009817C)
FSCTL_TXFS_TRANSACTION_ACTIVE (0x0009418C)
FSCTL_TXFS_LIST_TRANSACTIONS (0x000941E4)
FSCTL_TXFS_READ_BACKUP_INFORMATION2 (0x000901F8)
FSCTL_TXFS_WRITE_BACKUP_INFORMATION2 (0x00090200)
FSCTL_QUERY_FILE_REGIONS (0x00090284)
FSCTL_IS_CSV_FILE (0x00090248)
FSCTL_IS_FILE_ON_CSV_VOLUME (0x0009025C)
Windows 10 v1511 operating system and prior and Windows Server 2012 R2 operating system and prior block FSCTL_MARK_HANDLE (0x000900FC) and do not pass it through to the object store. The request is failed with STATUS_NOT_SUPPORTED.
Windows Vista SP1, Windows 7, Windows Server 2008, and Windows Server 2008 R2 fail FSCTLs whose transfer type is METHOD_NEITHER with error STATUS_NOT_SUPPORTED except the following ones. For more information about FSCTL transfer type, see [MSDN-IoCtlCodes].
FSCTL_PIPE_TRANSCEIVE (0x0011C017)
FSCTL_QUERY_ALLOCATED_RANGES (0x000940CF)
FSCTL_WRITE_USN_CLOSE_RECORD (0x000900EF)
FSCTL_READ_FILE_USN_DATA (0x000900EB)
FSCTL_GET_RETRIEVAL_POINTERS (0x00090073)
FSCTL_FIND_FILES_BY_SID (0x0009008F)
FSCTL_SRV_READ_HASH (0x001441BB)
<395> Section 3.3.5.15.8: Windows performs passthrough FSCTL operations via Server Requests an FsControl Request [MS-FSA] section 2.1.5.10.
<396> Section 3.3.5.15.8: Windows–based SMB2 servers will set OutputOffset to InputOffset + InputCount, rounded up to a multiple of 8.
<397> Section 3.3.5.15.9: Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 servers process the FSCTL_LMR_REQUEST_RESILIENCY request regardless of the negotiated dialect.
<398> Section 3.3.5.15.9: Windows 7 and Windows Server 2008 R2 servers keep the resilient handle open indefinitely when the requested Timeout value is equal to zero. Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 servers set a constant value of 120 seconds.
<399> Section 3.3.5.15.13: Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 require that the caller is a member of the Administrators group.
<400> Section 3.3.5.16: Windows-based servers use only the 30 least significant bits of AsyncId to look up a request in Connection.AsyncCommandList.
<401> Section 3.3.5.16: When being handled by an object store, Windows performs cancellation of in-progress requests via the interface in [MS-FSA] section 2.1.5.20, Server Requests Canceling an Operation, passing Request.CancelRequestId as an input parameter. Windows does not attempt to cancel other in-progress requests.
<402> Section 3.3.5.17: Windows Vista SP1, Windows 7, Windows Server 2008, and Windows Server 2008 R2 servers do not disconnect the connection.
<403> Section 3.3.5.18: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 fail the request with STATUS_NOT_SUPPORTED.
<404> Section 3.3.5.18:  Windows-based SMB2 servers fail the request with STATUS_INVALID_PARAMETER if OutputBufferLength is greater than 65536.
<405> Section 3.3.5.18: Windows Vista SP1, Windows Server 2008, Windows 7, and Windows Server 2008 R2 close and reopen the directory handle prior to processing the request.
<406> Section 3.3.5.18: Windows-based servers perform query directory requests, as specified in [MS-FSA] section 2.1.5.6 with the following input parameters:
Open is set to Open.LocalOpen.
FileInformationClass is set to the InformationClass that is received in the SMB2 QUERY_DIRECTORY Request.
OutputBufferSize is set to the OutputBufferLength that is received in the SMB2 QUERY_DIRECTORY Request.
If SMB2_RESTART_SCANS or SMB2_REOPEN is set in the Flags field of the SMB2 QUERY_DIRECTORY Request, RestartScan is set to TRUE.
If SMB2_RETURN_SINGLE_ENTRY is set in the Flags field of the request, ReturnSingleEntry is set to TRUE.
FileIndex is set to 0.
FileNamePattern is set to the search pattern specified in the SMB2 QUERY_DIRECTORY by FileNameOffset and FileNameLength.
When SMB2_REOPEN is set in the Flags field of SMB2 QUERY_DIRECTORY request and the object store does not return any files, Windows 10 v1803 through Windows 11, and Windows Server 2019 fail the request with STATUS_NO_MORE_FILES.
When SMB2_REOPEN is set in the Flags field of SMB2 QUERY_DIRECTORY request and the object store does not return any files, Windows 11 v22H2 and Windows 11, version 23H2 with [MSKB-5062663], Windows 11, version 24H2 with [MSKB-5062660] and later, Windows Server 2022 with [MSKB-5063880] and Windows Server 2022, 23H2 with [MSKB-5063899] and Windows Server 2025 with [MSKB-5062660] and later fail the request with STATUS_NO_SUCH_FILE.
<407> Section 3.3.5.18:  Windows-based servers ignore SMB2_INDEX_SPECIFIED in Flags field and FileIndex value.
<408> Section 3.3.5.19: Windows-based SMB2 servers fail the request with STATUS_INVALID_PARAMETER if OutputBufferLength is greater than 65536.
<409> Section 3.3.5.19: Windows-based servers handle the following commands asynchronously: SMB2 Create (section 2.2.13) when this create would result in an oplock break, SMB2 IOCTL Request (section 2.2.31) for FSCTL_PIPE_TRANSCEIVE if it blocks for more than 1 millisecond, SMB2 IOCTL Request for FSCTL_SRV_COPYCHUNK or FSCTL_SRV_COPYCHUNK_WRITE (section 2.2.31) when oplock break happens, SMB2 Change_Notify Request (section 2.2.35) if it blocks for more than 0.5 milliseconds, SMB2 Read Request (section 2.2.19) for named pipes if it blocks for more than 0.5 milliseconds, SMB2 Write Request (section 2.2.21) for named pipes if it blocks for more than 0.5 milliseconds, SMB2 Write Request (section 2.2.21) for large file write, SMB2 lock Request (section 2.2.26) if the SMB2_LOCKFLAG_FAIL_IMMEDIATELY flag is not set, and SMB2 FLUSH Request (section 2.2.17) for named pipes.
<410> Section 3.3.5.19: Windows requests ChangeNotify processing via Server Requests Change Notifications for a Directory in [MS-FSA] section 2.1.5.11. If the SMB2_WATCH_TREE flag is set, the WatchTree boolean is passed as TRUE. ChangeNotify notification is reported as described in [MS-FSA] section 2.1.5.11.1.
<411> Section 3.3.5.20: Windows-based SMB2 servers fail the request with STATUS_INVALID_PARAMETER if OutputBufferLength is greater than 65536.
<412> Section 3.3.5.20.1: Windows-based SMB2 servers fail the following request levels with STATUS_INVALID_INFO_CLASS instead of STATUS_NOT_SUPPORTED: 1, 2, 3, 10, 11, 12, 13, 19, 20, 27, 31, 36, 37, 38, 39, 40, 50.
<413> Section 3.3.5.20.1: Windows-based SMB2 servers fail the following request levels with STATUS_NOT_SUPPORTED instead of STATUS_INVALID_INFO_CLASS: 41, 43, 47, 49, 51, and 53. Windows-based SMB2 servers fail requests of level 52 with STATUS_INFO_LENGTH_MISMATCH.
<414> Section 3.3.5.20.1:  Windows 10 v1709, Windows Server v1709 and prior do not support the FileNormalizedNameInformation information class.
<415> Section 3.3.5.20.1: Windows-based SMB2 servers will set CurrentByteOffset to any value.
<416> Section 3.3.5.20.1: Windows performs SMB2 GET_INFO SMB2_0_INFO_FILE processing as specified in the subsection of [MS-FSA] section 2.1.5.12, corresponding to the requested FILE_INFORMATION_CLASS value of the FileInfoClass request field, as listed in section 2.2.37.
<417> Section 3.3.5.20.1: If the information class is FileAllInformation, Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 return an absolute path to the file name as part of FileNameInformation.
<418> Section 3.3.5.20.2: Windows performs SMB2 GET_INFO SMB2_0_INFO_FILESYSTEM processing via the subsection of [MS-FSA] section 2.1.5.13 corresponding to the requested FS_INFORMATION_CLASS value of the FileInfoClass request field, as listed in section 2.2.37.
<419> Section 3.3.5.20.2:  Windows 7 through Windows 11 and Windows Server 2008 R2 operating system through Windows Server 2022 SMB2 servers do not clear the bits FILE_RETURNS_CLEANUP_RESULT_INFO, FILE_SUPPORTS_POSIX_UNLINK_RENAME before sending to the client.
<420> Section 3.3.5.20.2: SetFsInfo calls to Windows-based servers fail with STATUS_ACCESS_DENIED because Windows-based servers do not allow setting volume information over the network.
<421> Section 3.3.5.20.3: Windows performs SMB2 GET_INFO SMB2_0_INFO_SECURITY processing via Server Requests a Query of Security Information ([MS-FSA] section 2.1.5.14).
<422> Section 3.3.5.20.4: Windows-based servers do support quotas, if configured.
<423> Section 3.3.5.20.4: Windows performs SMB2 GET_INFO SMB2_0_INFO_QUOTA processing via Server Requests a Query of Quota Information ([MS-FSA] section 2.1.5.21).
<424> Section 3.3.5.21: Windows-based SMB2 servers fail the request with STATUS_INVALID_PARAMETER if BufferLength is greater than 65536.
<425> Section 3.3.5.21.1: Windows-based SMB2 servers fail the following request levels with STATUS_NOT_SUPPORTED instead of STATUS_INVALID_INFO_CLASS: 30, 41, 42, 43.
<426> Section 3.3.5.21.1: Windows performs SMB2 SET_INFO SMB2_0_INFO_FILE processing via the subsection of [MS-FSA] section 2.1.5.15 corresponding to the requested FILE_INFORMATION_CLASS value of the FileInfoClass request field, as listed in section 2.2.37.
<427> Section 3.3.5.21.2: Windows performs SMB2 SET_INFO SMB2_0_INFO_FILESYSTEM processing via the subsection of [MS-FSA] section 2.1.5.16 corresponding to the requested FS_INFORMATION_CLASS value of the FileInfoClass request field, as listed in section 2.2.37.
<428> Section 3.3.5.21.3: If the underlying object store does not support object security based on Access Control Lists (as specified in [MS-DTYP] section 2.4.5), it returns STATUS_SUCCESS.
<429> Section 3.3.5.21.3: Windows Server 2008, Windows 7 and Windows Server 2008 R2 ignore the ATTRIBUTE_SECURITY_INFORMATION flag value.
<430> Section 3.3.5.21.3:  Windows Server 2008, Windows 7 and Windows Server 2008 R2 ignore the SCOPE_SECURITY_INFORMATION flag value.
<431> Section 3.3.5.21.3:  Windows Server 2008, Windows 7 and Windows Server 2008 R2 ignore the BACKUP_SECURITY_INFORMATION flag value.
<432> Section 3.3.5.21.3: Windows performs SMB2 SET_INFO SMB2_0_INFO_SECURITY processing via Server Requests Setting of Security Information [MS-FSA] section 2.1.5.17.
<433> Section 3.3.5.21.4: Windows-based servers do support quotas, if configured.
<434> Section 3.3.5.21.4: Windows performs SMB2 SET_INFO SMB2_0_INFO_QUOTA processing via Server Requests Setting of Quota Information ([MS-FSA] section 2.1.5.22).
<435> Section 3.3.5.22.1: Windows-based servers complete the oplock break indication request with the object store by providing the following SMB2 parameters as input parameters, as specified [MS-FSA] section 2.1.5.19:
<436> Section 3.3.5.22.1: Windows-based servers complete the oplock break indication request with the object store by providing the following SMB2 parameters as input parameters, as specified [MS-FSA] section 2.1.5.19:
<437> Section 3.3.5.22.1: Windows-based servers complete the oplock break indication request with the object store by providing the following SMB2 parameters as input parameters, as specified [MS-FSA] section 2.1.5.19:
<438> Section 3.3.5.22.1: If multiple conflicting Opens occur before an Oplock Acknowledgment for the first oplock break is received, that change the server oplock state to a level that is lower than the pending notification, the server fails the Oplock Acknowledgment with STATUS_REQUEST_NOT_ACCEPTED. Windows-based servers complete the oplock break indication request with the object store by providing the following SMB2 parameters as input parameters, as specified in [MS-FSA] section 2.1.5.19:
<439> Section 3.3.6.3: Windows-based servers use a constant time-out value of 45 seconds.
<440> Section 3.3.7.1: Windows performs cancellation of in-progress requests via the interface in [MS-FSA] section 2.1.5.20, Server Requests Canceling an Operation, passing Request.CancelRequestId as an input parameter.
<441> Section 3.3.7.1: Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 servers will not reset ResilientOpenScavengerExpiryTime.
<442> Section 3.3.7.1: Windows performs cancellation of in-progress requests via the interface in [MS-FSA] section 2.1.5.20, Server Requests Canceling an Operation, passing Request.CancelRequestId as an input parameter.
```

### New Content
```
The information in this specification is applicable to the following Microsoft products or supplemental software. References to product versions include updates to those products.
The terms "earlier" and "later", when used with a product version, refer to either all preceding versions or all subsequent versions, respectively. The term "through" refers to the inclusive range of versions. Applicable Microsoft products are listed chronologically in this section.
Windows Client
Windows Vista operating system
Windows 7 operating system
Windows 8 operating system
Windows 8.1 operating system
Windows 10 operating system
Windows 11 operating system
Windows Server
Windows Server 2008 operating system
Windows Server 2008 R2 operating system
Windows Server 2012 operating system
Windows Server 2012 R2 operating system
Windows Server 2016 operating system
Windows Server operating system
Windows Server 2019 operating system
Windows Server 2022 operating system
Windows Server 2025 operating system
Exceptions, if any, are noted in this section. If an update version, service pack or Knowledge Base (KB) number appears with a product name, the behavior changed in that update. The new behavior also applies to subsequent updates unless otherwise specified. If a product edition appears with the product version, behavior is different in that product edition.
Unless otherwise specified, any statement of optional behavior in this specification that is prescribed using the terms "SHOULD" or "SHOULD NOT" implies product behavior in accordance with the SHOULD or SHOULD NOT prescription. Unless otherwise specified, the term "MAY" implies that the product does not follow the prescription.
<1> Section 1.6: The following table illustrates the support of SMB 2 protocol on various Windows operating system versions.
Windows Vista RTM implemented dialect 2.000, which was not interoperable and was obsoleted by Windows Vista SP1.
<2> Section 2.1:  Windows 11, version 24H2 operating system and later and Windows Server 2025 and later SMB2 servers allow listening on any configured port only when the transport is QUIC.
Windows 11, version 24H2 and later and Windows Server 2025 and later SMB2 clients can connect to an SMB2 server that allows listening on any configured port over TCP, RDMA and QUIC transports.
By default, Windows SMB2 clients and servers always use a single stream per QUIC connection. Sending or receiving more than one stream per connection will be blocked by the underlying transport QUIC.
<3> Section 2.2.1.2: Windows clients set this field to 0xFEFF.
<4> Section 2.2.1.2: Windows servers do not use this field in the request processing and return the value received in the request.
<5> Section 2.2.2: Windows 10 v1703 operating system and prior and Windows Server 2016 and prior set ErrorData to one uninitialized byte when ByteCount is zero.
<6> Section 2.2.2.2.1: Windows-based servers will never follow a symlink. It is the client's responsibility to evaluate the symlink and access the actual file using the symlink. Windows-based servers only return STATUS_STOPPED_ON_SYMLINK when the open fails due to presence of a symlink.
<7> Section 2.2.2.2.1: Windows-based servers will return an absolute target to a local resource in the format of "\??\C:\..." where C: is the drive mount point on the local system and ... is replaced by the remainder of the path to the target.
<8> Section 2.2.3: Windows-based SMB2 servers fail the request and return STATUS_INVALID_PARAMETER, if the DialectCount field is greater than 64.
<9> Section 2.2.3: Windows 8.1 operating system and later and Windows Server 2012 R2 operating system and later fail the request with STATUS_NOT_SUPPORTED if the Reserved field is set to a nonzero value.
<10> Section 2.2.3: Windows Vista SP1 and Windows Server 2008 do not support this dialect revision.
<11> Section 2.2.3: Windows Vista SP1, Windows Server 2008, Windows 7, and Windows Server 2008 R2 do not support this dialect revision.
<12> Section 2.2.3: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, and Windows Server 2012 do not support the SMB 3.0.2 dialect.
<13> Section 2.2.3: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 do not support the SMB 3.1.1 dialect.
<14> Section 2.2.3.1: Windows 10 v1809 operating system operating system and prior and Windows Server v1809 operating system operating system and prior do not send or process SMB2_COMPRESSION_CAPABILITIES.
<15> Section 2.2.3.1: Windows 10 v1809 operating system and prior and Windows Server v1809 operating system and prior do not send or process SMB2_NETNAME_NEGOTIATE_CONTEXT_ID.
<16> Section 2.2.3.1: Windows 10 v1909 operating system and prior and Windows Server v1909 operating system and prior do not send or process SMB2_TRANSPORT_CAPABILITIES.
<17> Section 2.2.3.1: Windows 10 operating system and prior and Windows Server v20H2 operating system and prior do not send or process SMB2_RDMA_TRANSFORM_CAPABILITIES.
<18> Section 2.2.3.1: Windows 10 operating system and prior and Windows Server v20H2 operating system and prior do not send or process SMB2_SIGNING_CAPABILITIES.
<19> Section 2.2.3.1.5:  Windows 10 v2004 operating system, Windows 10 v20H2 operating system, Windows Server v2004 operating system, and Windows Server v20H2 do not send or process SMB2_ACCEPT_TRANSPORT_LEVEL_SECURITY.
<20> Section 2.2.4: Windows Vista SP1 and Windows Server 2008 do not support this dialect revision.
<21> Section 2.2.4: Windows Vista SP1, Windows Server 2008, Windows 7 and Windows Server 2008 R2 do not support this dialect revision.
<22> Section 2.2.4: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, and Windows Server 2012 do not support this dialect revision.
<23> Section 2.2.4: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 do not support the SMB 3.1.1 dialect.
<24> Section 2.2.4: The "SMB 2.???" dialect string is not supported by SMB2 clients and servers in Windows Vista SP1 and Windows Server 2008.
<25> Section 2.2.4: Windows-based SMB2 servers can set this field to any value.
<26> Section 2.2.4: Windows–based SMB2 servers generate a new ServerGuid each time they are started.
<27> Section 2.2.4: Windows clients do not enforce the MaxTransactSize value.
<28> Section 2.2.5: Windows-based clients always set the Capabilities field to SMB2_GLOBAL_CAP_DFS(0x00000001) and the server will ignore them on receipt.
<29> Section 2.2.5: Windows clients set the Buffer with a token as produced by the NTLM authentication protocol in the case, see [MS-NLMP] section 3.1.5.1.
<30> Section 2.2.6: Windows clients set the Buffer with a token as produced by the NTLM authentication protocol in the case, see [MS-NLMP] section 3.1.5.1.
<31> Section 2.2.9: Windows 10 v1703 and prior and Windows Server 2016 and prior do not support the SMB2_TREE_CONNECT_FLAG_REDIRECT_TO_OWNER flag.
<32> Section 2.2.9: The Windows SMB 2 Protocol client translates any names of the form \\server\pipe to \\server\IPC$ before sending a request on the network.
<33> Section 2.2.10: SMB2_SHAREFLAG_FORCE_LEVELII_OPLOCK is not supported on Windows Vista SP1 and Windows Server 2008.
<34> Section 2.2.10: Windows 10 v1703 and prior and Windows Server 2016 and prior do not send or process this flag.
<35> Section 2.2.10: Windows 10 operating system and prior and Windows Server v20H2 operating system and prior do not send or process this flag.
<36> Section 2.2.13: Windows-based clients never use exclusive oplocks. Because there are no situations where the client would require an exclusive oplock where it would not also require an SMB2_OPLOCK_LEVEL_BATCH, it always requests an SMB2_OPLOCK_LEVEL_BATCH.
<37> Section 2.2.13: When opening a printer file or a named pipe, Windows-based servers ignore these ShareAccess values.
<38> Section 2.2.13: When opening a printer object, Windows-based servers ignore this value.
<39> Section 2.2.13: When opening a printer object, Windows-based servers ignore this value.
<40> Section 2.2.13: When opening a printer object, Windows-based servers ignore this value.
<41> Section 2.2.13: Windows-based servers reserve all bits that are not specified in the table. If any of the reserved bits are set, STATUS_NOT_SUPPORTED is returned.
<42> Section 2.2.13: Windows SMB2 clients do not initialize this bit. The bit contains the value specified by the caller when requesting the open.
<43> Section 2.2.13: Windows SMB2 clients do not initialize this bit. The bit contains the value specified by the caller when requesting the open.
<44> Section 2.2.13: Windows SMB2 clients do not initialize this bit. The bit contains the value specified by the caller when requesting the open.
<45> Section 2.2.13: Windows SMB2 clients do not initialize this bit. The bit contains the value specified by the caller when requesting the open.
<46> Section 2.2.13: Windows SMB2 clients do not initialize this bit. The bit contains the value specified by the caller when requesting the open.
<47> Section 2.2.13: Windows Vista SP1, Windows Server 2008, Windows 7, Windows 8, and Windows 8.1-based clients will set this bit when it is requested by the application.
<48> Section 2.2.13.1.1: Windows sets this flag to the value passed in by the higher-level application.
<49> Section 2.2.13.1.1: Windows 7 operating system and later and Windows Server 2008 R2 operating system and later do not ignore the SYNCHRONIZE bit, and pass it to the underlying object store. If the caller requests SYNCHRONIZE in the DesiredAccess parameter, but the SYNCHRONIZE access is not granted to the caller for the object being created or opened, the underlying object store fails the request and returns STATUS_ACCESS_DENIED. When SYNCHRONIZE access is granted, the SYNCHRONIZE bit is returned in MaximalAccess field of SMB2_CREATE_QUERY_MAXIMAL_ACCESS_RESPONSE with no other behavior.
<50> Section 2.2.13.1.1: Windows fails the create request with STATUS_ACCESS_DENIED if the caller does not have the SeSecurityPrivilege, as specified in [MS-LSAD] section 3.1.1.2.1.
<51> Section 2.2.13.1.2: Windows sets this flag to the value passed in by the higher-level application.
<52> Section 2.2.13.1.2: Windows 7 operating system and later and Windows Server 2008 R2 operating system and later do not ignore the SYNCHRONIZE bit, and pass it to the underlying object store. If the caller requests SYNCHRONIZE in the DesiredAccess parameter, but the SYNCHRONIZE access is not granted to the caller for the object being created or opened, the underlying object store fails the request and returns STATUS_ACCESS_DENIED. When SYNCHRONIZE access is granted, the SYNCHRONIZE bit is returned in MaximalAccess field of SMB2_CREATE_QUERY_MAXIMAL_ACCESS_RESPONSE (section 2.2.14.2.5) with no other behavior.
<53> Section 2.2.13.1.2: Windows fails the create request with STATUS_ACCESS_DENIED if the caller does not have the SeSecurityPrivilege, as specified in [MS-LSAD] section 3.1.1.2.1.
<54> Section 2.2.13.2: If DataLength is 0, Windows-based clients set this field to any value.
<55> Section 2.2.13.2.8: Windows 7 operating system and later and Windows Server 2008 R2 operating system and later acting as SMB servers support the following combinations of values: 0, READ, READ | WRITE, READ | HANDLE, READ | WRITE | HANDLE.
<56> Section 2.2.13.2.10: Windows Server 2012 operating system and later support the following combinations of values: 0, READ, READ | WRITE, READ | HANDLE, READ | WRITE | HANDLE.
<57> Section 2.2.14: Windows-based SMB2 servers always return FILE_OPENED for pipes with successful opens.
<58> Section 2.2.14: Windows-based SMB2 servers can set this field to any value.
<59> Section 2.2.14.2.11: Windows 8 operating system and later and Windows Server 2012 operating system and later set this field to an arbitrary value.
<60> Section 2.2.19:  Windows 10 v1809 and prior and Windows Server v1809 and prior do not send or process SMB2_READFLAG_REQUEST_COMPRESSED flag.
<61> Section 2.2.20: Windows 10 operating system and prior and Windows Server v20H2 operating system and prior do not send or process SMB2_READFLAG_RESPONSE_RDMA_TRANSFORM flag.
<62> Section 2.2.21: Windows 10 operating system and prior and Windows Server v20H2 operating system and prior do not send or process SMB2_CHANNEL_RDMA_TRANSFORM flag.
<63> Section 2.2.24.2: Windows clients always set the LeaseState in the Lease Break Acknowledgment to be equal to the LeaseState in the Lease Break Notification from the server.
<64> Section 2.2.31: Windows clients set the OutputOffset field equal to the InputOffset field.
<65> Section 2.2.31.1.1: Windows clients set this field to an arbitrary value.
<66> Section 2.2.32: Windows–based SMB2 servers set InputCount to the same value as the value received in the IOCTL request for the following FSCTLs.
FSCTL_FIND_FILES_BY_SID
FSCTL_GET_RETRIEVAL_POINTERS
FSCTL_QUERY_ALLOCATED_RANGES
FSCTL_READ_FILE_USN_DATA
FSCTL_RECALL_FILE
FSCTL_WRITE_USN_CLOSE_RECORD
Windows clients ignore the InputCount field.
<67> Section 2.2.32: Windows–based SMB2 servers set OutputOffset to InputOffset + InputCount, rounded up to a multiple of 8.
<68> Section 2.2.32.2: Windows-based SMB2 server will place 2 extra bytes set to zero in the SRV_SNAPSHOT_ARRAY response, if NumberOfSnapShotsReturned is zero.
<69> Section 2.2.32.3: Windows-based servers always send 4 bytes of zero for the Context field.
<70> Section 2.2.32.4.1: Windows–based SMB2 servers and clients do not check SourceFileName. It is ignored.
<71> Section 2.2.32.5.1.2: Windows 10 v1709 operating system through Windows 10 v1909 and Windows Server v1709 operating system through Windows Server v1909 set this field to any value.
<72> Section 2.2.33: Windows 10 operating system and prior and Windows Server 2022 operating system and prior do not send or process this information class.
<73> Section 2.2.33:  Windows 11, version 23H2 operating system and prior and Windows Server 2022 and prior do not send or process FileId64ExtdDirectoryInformation information class.
<74> Section 2.2.33:  Windows 11, version 23H2 and prior and Windows Server 2022 and prior do not send or process FileId64ExtdBothDirectoryInformation information class.
<75> Section 2.2.33:  Windows 11, version 23H2 and prior and Windows Server 2022 and prior do not send or process FileIdAllExtdDirectoryInformation information class.
<76> Section 2.2.33:  Windows 11, version 23H2 and prior and Windows Server 2022 and prior do not send or process FileIdAllExtdBothDirectoryInformation information class.
<77> Section 2.2.33: SMB2 wildcard characters are based on Windows wildcard characters, as described in [MS-FSA] section 2.1.4.4, Algorithm for Determining if a FileName Is in an Expression. For more information on wildcard behavior in Windows, see [FSBO] section 7.
<78> Section 2.2.37: Windows SMB2 servers ignore the FileInfoClass field for quota queries. Windows SMB2 clients set the FileInfoClass field to 0x20 for quota queries.
<79> Section 2.2.37: Windows clients set this value to the offset from the start of the SMB2 header to the beginning of the Buffer field.
<80> Section 2.2.37: Windows clients send a 1-byte buffer of 0 when InputBufferLength is set to 0.
<81> Section 2.2.37.1: Windows-based clients never send a request using the SidBuffer format 2.
<82> Section 2.2.39: Windows-based servers will fail the request with STATUS_INVALID_PARAMETER if BufferOffset is less than 0x60 or greater than 0xA0.
<83> Section 2.2.41: Windows 8 operating system and later and Windows Server 2012 operating system and later set this field to an arbitrary value.
<84> Section 2.2.42.1: Windows 10 v1809 and prior and Windows Server v1809 and prior do not send or process SMB2 COMPRESSION_TRANSFORM_HEADER_UNCHAINED.
<85> Section 2.2.42.2: Windows 10 v1909 and prior and Windows Server v1909 and prior do not send or process SMB2_COMPRESSION_TRANSFORM_HEADER_CHAINED.
<86> Section 2.2.42.2.1: Windows 10 v1909 and prior and Windows Server v1909 and prior do not send or process SMB2_COMPRESSION_CHAINED_PAYLOAD_HEADER.
<87> Section 2.2.42.2.2: Windows 10 v1909 and prior and Windows Server v1909 and prior do not send or process SMB2_COMPRESSION_PATTERN_PAYLOAD_V1.
<88> Section 2.2.43: Windows 10 operating system and prior and Windows Server v20H2 operating system and prior do not send or process RDMA transforms.
<89> Section 2.2.43.1: Windows 10 operating system and prior and Windows Server v20H2 operating system and prior do not send or process RDMA transforms.
<90> Section 3.1.3: By default, Windows-based servers set the RequireMessageSigning value to TRUE for domain controllers and FALSE for all other machines.
<91> Section 3.1.3: Windows 8 and later and Windows Server 2012 and later set IsEncryptionSupported to TRUE.
<92> Section 3.1.3: Windows 10 v1903 operating system and later and Windows Server v1903 operating system and later set IsCompressionSupported to TRUE.
<93> Section 3.1.3: Windows 10 v2004 and later and Windows Server v2004 and later operating systems set IsChainedCompressionSupported to TRUE.
<94> Section 3.1.3: Windows 11 operating system and later and Windows Server 2022 operating system and later set IsRDMATransformSupported to TRUE.
<95> Section 3.1.3: Windows 11 operating system and later and Windows Server 2022 operating system and later set this to TRUE.
<96> Section 3.1.3:  Windows 11 and later and Windows Server 2022 and later set IsSigningCapabilitiesSupported to TRUE.
<97> Section 3.1.3:  Windows 10 v2004 and later and Windows Server v2004 and later set IsTransportCapabilitiesSupported to TRUE.
<98> Section 3.1.3:  Windows 11, version 24H2 and later and Windows Server 2022, 23H2 operating system and later set IsServerToClientNotificationsSupported to TRUE.
<99> Section 3.1.4.3: Windows-based clients and servers do not encrypt the message if the connection is NetBIOS over TCP.
<100> Section 3.1.4.4: Windows-based clients and servers do not compress the message if the connection is over RDMA.
<101> Section 3.1.4.4: Windows-based clients choose to selectively compress only segments of SMB2 requests with large payloads, whose size is greater than 4096 bytes.
<102> Section 3.2.1.2: Windows clients do not enforce the MaxTransactSize value.
<103> Section 3.2.2.1: The Windows-based client implements this timer with a default value of 60 seconds. The client does not enforce this timer for the following commands:
Named Pipe Read
Named Pipe Write
Directory Change Notifications
Blocking byte range lock requests
FSCTLs: FSCTL_PIPE_PEEK, FSCTL_PIPE_TRANSCEIVE, FSCTL_PIPE_WAIT
<104> Section 3.2.2.2: The Windows-based clients scan existing connections every 10 seconds and disconnect idle connects that have no open files and that have had no activity for 10 or more seconds.
<105> Section 3.2.2.3: Windows clients set this timer to 600 seconds, except Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2 clients, which do not implement this timer.
<106> Section 3.2.3:  Windows clients set RejectGuestAccess to TRUE by default.
<107> Section 3.2.3:  Windows clients set AllowInsecureGuestAccess to FALSE by default.
<108> Section 3.2.3: Windows 8 operating system and later and Windows Server 2012 operating system and later clients set this based on a stored value in the registry.
<109> Section 3.2.3: Windows 10 v1903 and later, and Windows Server v1903 and later set this to FALSE.
<110> Section 3.2.3:  Windows 11 with [MSKB-5035854], Windows 11, version 22H2 operating system with [MSKB-5035942], Windows Server 2022 with [MSKB-5035857], Windows Server 2022, 23H2, Windows 11, version 24H2 and later, and Windows Server 2025 and later set IsMutualAuthOverQUICSupported to TRUE.
<111> Section 3.2.4.1.1: A client can selectively sign requests, and the server will sign the corresponding responses.
<112> Section 3.2.4.1.2: Windows-based clients require a minimum of 4 credits.
<113> Section 3.2.4.1.2: The Windows-based client will request credits up to a configurable maximum of 128 by default. A Windows-based client sends a CreditRequest value of 0 for an SMB2 NEGOTIATE Request and expects the server to grant at least 1 credit. In subsequent requests, the client will request credits sufficient to maintain its total outstanding limit at the configured maximum.
<114> Section 3.2.4.1.3: Windows 7 operating system and later and Windows Server 2008 R2 operating system and later SMB2 clients will block any newly initiated multi-credit requests that exceed the shortage, but will send out other requests that can be satisfied using the available credits.
<115> Section 3.2.4.1.3: Windows-based clients set the MessageId field to 0, when the AsyncId field is set to an asynchronous identifier of the request.
<116> Section 3.2.4.1.4: Windows-based clients do not send compounded CREATE + READ/WRITE requests when the payload size of the WRITE request or the anticipated response of the READ request is greater than 65536.
<117> Section 3.2.4.1.4: Windows SMB2 Server allows a mix of related and unrelated compound requests in the same transport send. Upon encountering a request with SMB2_FLAGS_RELATED_OPERATIONS not set Windows SMB2 Server treats it as the start of a chain.
<118> Section 3.2.4.1.4: The Windows-based client does not send unrelated compounded requests.
<119> Section 3.2.4.1.4: Windows-based clients will compound certain related requests to improve performance, by combining a Create with another operation, such as an attribute query.
<120> Section 3.2.4.1.5: Windows 7 and Windows Server 2008 R2 SMB2 clients set CreditCharge to 1 for IOCTL requests.
<121> Section 3.2.4.1.5: Windows 7 operating system and later and Windows Server 2012 operating system and later based SMB2 clients set the CreditCharge field to 1 if Connection.SupportsMultiCredit is FALSE.
<122> Section 3.2.4.1.7: Windows-based clients choose the Channel with the least value of Channel.Connection.OutstandingRequests.
<123> Section 3.2.4.1.8: Windows 10 v20H2 operating system and prior and Windows Server v20H2 operating system and prior encrypt the message as specified in section 3.1.4.3 before sending.
<124> Section 3.2.4.1.9:  Windows 10 v1903 and later, and Windows Server v1903 and later do not compress SMB2 NEGOTIATE request and SMB2 OPLOCK_BREAK Acknowledgment.
<125> Section 3.2.4.2: Windows-based clients always set up a new transport connection when establishing a new session to a server.
<126> Section 3.2.4.2: Windows will reuse an existing session only if the access is by the same logged-on user and the Connection.ServerName matches the application-supplied ServerName.
<127> Section 3.2.4.2: Windows will reuse the connection to establish a new session, if a connection is available and Connection.ServerName matches the application-supplied ServerName
<128> Section 3.2.4.2.1:  Windows clients initiate new transport connections to the server with Direct TCP and NetBIOS over TCP. Windows 10 v1511 Enterprise operating system and Windows Server 2012 operating system and later do not initiate a new transport connection with RDMA, but do after a multichannel exchange if a suitable interface is available.
<129> Section 3.2.4.2.1: Windows Vista SP1 and Windows Server 2008 clients enumerate all transports, send a Direct TCP connection request, and then, after 500 milliseconds, send connection requests to all other eligible addresses and all other NetBIOS over TCP transports.
Windows 7 and Windows Server 2008 R2 clients enumerate all transports, send a Direct TCP connection request, and then, after 1,000 milliseconds, send connection requests to all other eligible addresses and all other NetBIOS over TCP transports.
Windows 8 operating system and later and Windows Server 2012 operating system and later clients look up a server entry in ServerList where Server.ServerName matches the ServerName to which the connection is established. If no entry is found, the clients enumerate all transports, send a Direct TCP connection request, and then, after 1,000 milliseconds, send connection requests to all other eligible addresses over Direct TCP and NetBIOS over TCP transports. If an entry is found, the clients send a Direct TCP connection request, and then, after 1,000 milliseconds, enumerate all transports and send connection requests to all Direct TCP addresses.
In each case, the first successful connection is used and all others are closed.
<130> Section 3.2.4.2.2: The Windows-based client will initiate a multi-protocol negotiation unless it has previously negotiated with this server and the negotiated server's DialectRevision is equal to 0x0202, 0x0210, 0x0300, 0x0302, or 0x0311. In the latter case, it will initiate an SMB2-Only negotiate.
<131> Section 3.2.4.2.2.2: Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 set Dialects array to all the dialects the client implements and DialectCount to the number of dialects in Dialects array.
<132> Section 3.2.4.2.2.2:  Windows 8 operating system, Windows 8.1, Windows Server 2012 operating system and Windows Server 2012 R2 always set SMB2_GLOBAL_CAP_ENCRYPTION in the Capabilities field if IsEncryptionSupported is TRUE.
<133> Section 3.2.4.2.2.2:  Windows 10, Windows 11 without [MSKB-5036894], Windows 11 v22H2 without [MSKB-5036980], Windows 11, version 23H2 without [MSKB-5036980], Windows Server 2016, Windows Server 2022 without [MSKB-5036909] and Windows Server 2022, 23H2 without [MSKB-5036910] always set SMB2_GLOBAL_CAP_ENCRYPTION in the Capabilities field if IsEncryptionSupported is TRUE.
<134> Section 3.2.4.2.2.2: Windows 10, and Windows Server 2016 operating system and later use 32 bytes of Salt.
<135> Section 3.2.4.2.2.2: Windows 10 and Windows Server 2016 through Windows Server v20H2 initialize with AES-128-GCM(0x0002), followed by AES-128-CCM(0x0001).
Windows 11 operating system and later and Windows Server 2022 operating system and later initialize with AES-128-GCM(0x0002), followed by AES-128-CCM(0x0001), followed by AES-256-GCM(0x0004), followed by AES-256-CCM(0x0003).
<136> Section 3.2.4.2.2.2: Windows 10 v1903, Windows 10 v1909, Windows Server v1903, and Windows Server v1909 operating systems initialize with LZ77(0x0002) followed by LZ77+Huffman(0x0003) followed by LZNT1(0x0001).
Windows 10 v2004 through Windows 11, version 23H2 and Windows Server v2004 through Windows Server 2022, 23H2 initialize with Pattern_V1(0x0004), followed by LZ77(0x0002), followed by LZ77+Huffman(0x0003), followed by LZNT1(0x0001).
Windows 11, version 24H2 and later and Windows Server 2025 and later initialize with Pattern_V1(0x0004), followed by LZ77(0x0002), followed by LZ77+Huffman(0x0003), followed by LZNT1(0x0001), followed by LZ4(0x0005).
<137> Section 3.2.4.2.2.2: Windows 11 operating system and later and Windows Server 2022 operating system and later set RDMATransformIds to SMB2_RDMA_TRANSFORM_ENCRYPTION (0x0001) and SMB2_RDMA_TRANSFORM_SIGNING (0x0002).
<138> Section 3.2.4.2.2.2: Windows 10 v1809 and prior and Windows Server v1809 and prior do not support SMB2_NETNAME_NEGOTIATE_CONTEXT_ID.
<139> Section 3.2.4.2.2.2: Windows 11 operating system and later and Windows Server 2022 operating system and later initialize with AES-GMAC(0x0002), followed by AES-CMAC(0x0001), followed by HMAC-SHA256(0x0000).
<140> Section 3.2.4.2.3: Windows-based clients implement the first option that is specified.
<141> Section 3.2.4.2.3: All the GSS-API tokens used by Windows SMB2 clients are up to 4Kbytes in size. SMB2 servers always instruct the GSS_API server to expect the GSS_C_FRAGMENT_TO_FIT.
<142> Section 3.2.4.2.3.1: Windows-based clients implement the first option that is specified.
<143> Section 3.2.4.2.3.1: All the GSS-API tokens used by Windows SMB2 clients are up to 4Kbytes in size. SMB2 servers always instruct the GSS_API server to expect the GSS_C_FRAGMENT_TO_FIT.
<144> Section 3.2.4.2.4: Windows 11 v22H2 without [MSKB-5037853], Windows 11, version 23H2 without [MSKB-5037853] and Windows Server 2022, 23H2 without [MSKB-5037781] set SMB2_TREE_CONNECT_FLAG_REDIRECT_TO_OWNER bit in the Flags field of SMB2 TREE_CONNECT request if the share previously connected includes either SMB2_SHAREFLAG_ISOLATED_TRANSPORT flag or SMB2_SHARE_CAP_ASYMMETRIC capability.
<145> Section 3.2.4.3: Windows clients set File.LeaseKey to a newly generated GUID as specified in [MS-DTYP] section 2.3.4.2.
<146> Section 3.2.4.3:  On Windows 7 operating system and Windows Server 2008 R2, a 128-bit ClientLeaseId is generated by an arithmetic combination of LeaseKey and ClientGuid, which is passed to the object store at open/create time. On Windows 8 operating system and later and Windows Server 2012 operating system and later, the LeaseKey in the request is used as the ClientLeaseId.
<147> Section 3.2.4.3:  Although not required, failure to include the lease context can result in a lease break.
<148> Section 3.2.4.3: On Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2, the Lease.ClientLeaseId and Lease.ParentLeaseKey are passed to the object store in the form of TargetOplockKey and ParentOplockKey. A new or existing lease is thereby associated with the resulting open.
To acquire or promote the lease as dictated by the SMB2_CREATE_REQUEST_LEASE_V2 Create Context, a subsequent object store call is invoked as described in. [MS-FSA] section 2.1.5.18 Server Requests an Oplock. The Open parameter passed is the Open result from the above operation, and the Type parameter is LEVEL_GRANULAR to indicate a Lease request. The RequestedOplockLevel field is constructed to include zero or more bits as follows.
The Status code returned indicates whether the requested lease was granted.
<149> Section 3.2.4.3: Windows clients set File.LeaseKey to a newly generated GUID as specified in [MS-DTYP] section 2.3.4.2.
<150> Section 3.2.4.3: Microsoft Windows lease-aware clients always include SMB2_OPLOCK_LEVEL_LEASE if the open can potentially cause a lease break.
<151> Section 3.2.4.3:  Windows-based clients will request a batch oplock for file creates when application does not provide a requested oplock level, or an exclusive oplock is specified, or a lease is requested.
<152> Section 3.2.4.3.5: Windows 8 operating system and later and Windows Server 2012 operating system and later clients set this to zero.
<153> Section 3.2.4.3.8: A Windows client application requestsSMB2_LEASE_READ_CACHING and SMB2_LEASE_HANDLE_CACHING when a file is opened for read access. In addition, a Windows client application requests SMB2_LEASE_WRITE_CACHING if the file is being opened for write access.
<154> Section 3.2.4.6:  Windows-based clients set MinimumCount field to 0.
<155> Section 3.2.4.6:  Windows-based clients will try to send multiple read commands at the same time, starting at the lowest offset and working to the highest.
<156> Section 3.2.4.6:  Windows-based clients default to 4 KB.
<157> Section 3.2.4.7: Windows-based clients set the DataOffset field to 0x70, which indicates that the payload is always placed at the beginning of the Buffer field.
<158> Section 3.2.4.7: Windows-based clients will try to send multiple write commands at the same time, starting at the lowest offset and working to the highest.
<159> Section 3.2.4.7: Windows-based clients default to 4 KB.
<160> Section 3.2.4.8: Windows clients set this value to the offset from the start of the SMB2 header to the beginning of the Buffer field.
<161> Section 3.2.4.9: In a SET_INFO request where FileInfoClass is set to FileRenameInformation, Windows Vista SP1, Windows Server 2008, Windows 7, and Windows Server 2008 R2 clients append up to 4 additional padding bytes set to arbitrary values.
<162> Section 3.2.4.10: Windows clients set this value to the offset from the start of the SMB2 header to the beginning of the Buffer field.
<163> Section 3.2.4.12: Windows clients set this value to the offset from the start of the SMB2 header to the beginning of the Buffer field.
<164> Section 3.2.4.14: Windows-based clients will set StartSidLength and StartSidOffset to any value.
<165> Section 3.2.4.17: The Windows SMB2 server implementation closes and reopens the directory handle in order to "reset" the enumeration state. So any outstanding operations on the directory handle will be failed with a STATUS_FILE_CLOSED error.
<166> Section 3.2.4.20: Windows 7 and Windows Server 2008 R2 SMB2 clients set CreditCharge to 1 for IOCTL requests.
<167> Section 3.2.4.20.2.1: Windows clients set this field to InputOffset + InputCount, rounded up to a multiple of 8 bytes.
<168> Section 3.2.4.20.2.2: Windows applications use FSCTL_SRV_COPYCHUNK if the target file handle has FILE_READ_DATA access. Otherwise, they use the FSCTL_SRV_COPYCHUNK_WRITE.
<169> Section 3.2.4.20.2.2: Windows clients set the OutputOffset field to InputOffset + InputCount, rounded up to a multiple of 8 bytes.
<170> Section 3.2.4.20.3: Windows clients set the OutputOffset field to InputOffset + InputCount, rounded up to a multiple of 8 bytes.
<171> Section 3.2.4.20.4: Windows clients set the OutputOffset field to InputOffset + InputCount, rounded up to a multiple of 8 bytes.
<172> Section 3.2.4.20.5: Windows clients set the OutputOffset field to InputOffset + InputCount, rounded up to a multiple of 8 bytes.
<173> Section 3.2.4.20.6: Windows-based SMB2 servers pass File System Control requests through to the local object store but do not support I/O Control requests and fail such requests with STATUS_NOT_SUPPORTED.
<174> Section 3.2.4.20.6: Windows clients set the OutputOffset field to InputOffset + InputCount, rounded up to a multiple of 8 bytes.
<175> Section 3.2.4.20.7: Windows clients set the OutputOffset field to the sum of the values of the InputOffset and the InputCount fields, rounded up to a multiple of 8 bytes.
<176> Section 3.2.4.20.8: Windows clients set the OutputOffset field to InputOffset + InputCount, rounded up to a multiple of 8 bytes.
<177> Section 3.2.4.20.10: Windows clients set this to 64 kilobytes.
<178> Section 3.2.4.20.11: Windows clients set the OutputOffset field to InputOffset.
<179> Section 3.2.4.24: Windows based clients set the MessageId field to 0, when the AsyncId field is set to an asynchronous identifier of the request.
<180> Section 3.2.5.1: For the following error codes, Windows-based clients will retry the operation up to three times and then retry the operation every 5 seconds until the count of milliseconds specified by Open.ResilientTimeout is exceeded:
STATUS_SERVER_UNAVAILABLE
STATUS_FILE_NOT_AVAILABLE
STATUS_SHARE_UNAVAILABLE
<181> Section 3.2.5.1.1.1: Windows-based clients discard the message if it is encrypted and the connection is NetBIOS over TCP.
<182> Section 3.2.5.1.1.1: Windows 8.1 and Windows Server 2012 R2 continue to process the entire compound response if SMB2_FLAGS_RELATED_OPERATIONS is set in the Flags field of the SMB2 header of the response.
<183> Section 3.2.5.1.1.2: Windows-based clients discard the message if it is compressed and the connection is over RDMA.
<184> Section 3.2.5.1.5: Windows clients extend the Request Expiration Timer for requests being processed asynchronously as follows:
If the registry value ExtendedSessTimeout in HKLM\System\CurrentControlSet\Services\LanmanWorkStation\Parameters\ is set, the clients use the same value. Otherwise, the clients extend the expiration time to four times the value of default session timeout.
Windows Vista SP1, Windows Server 2008, Windows 7 and Windows Server 2008 R2 never enforce a timeout on SMB2 CHANGE_NOTIFY requests, SMB2 LOCK requests without the SMB2_LOCKFLAG_FAIL_IMMEDIATELY flag, SMB2 READ requests on named pipes, SMB2 WRITE requests on named pipes, and the FSCTL_PIPE_PEEK, FSCTL_PIPE_TRANSCEIVE and FSCTL_PIPE_WAIT named pipe FSCTLs.
<185> Section 3.2.5.1.7: Windows-based clients will not disconnect the connection, but will simply fail the request.
<186> Section 3.2.5.1.8: Windows-based SMB 2 Protocol clients do not check the validity of the command in the response.
<187> Section 3.2.5.1.9: Windows-based clients ignore 8-byte alignment boundary checking in a compounded chain.
<188> Section 3.2.5.2: Windows-based clients will not use the MaxTransactSize and will use the ServerGuid to determine if the client and server are the same machine.
<189> Section 3.2.5.2:  Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 disconnect the connection if MaxTransactSize, MaxReadSize, or MaxWriteSize is less than 4096.
<190> Section 3.2.5.2:  Windows 10 v1903 through Windows 10 v20H2 and Windows Server v1903 through Windows Server v20H2 will disconnect the connection.
<191> Section 3.2.5.3.3:  Windows 8, Windows 8.1, Windows Server 2012, and Windows Server 2012 R2 operating system do not perform this verification.
<192> Section 3.2.5.5: Windows 10 v1507 operating system through Windows 10 v1909, Windows Server 2016 operating system and later set Dialects array to all the dialects the client implements.
<193> Section 3.2.5.7: Windows 8 operating system and later and Windows Server 2012 operating system and later replay the create operation up to three times or until all channels in the session are disconnected.
<194> Section 3.2.5.12: Windows 8 operating system and later and Windows Server 2012 operating system and later replay the write operation up to three times or until all channels in the session are disconnected.
<195> Section 3.2.5.14: Windows 8 operating system and later and Windows Server 2012 operating system and later replay the IOCTL operation up to three times or until all of the channels in the session are disconnected.
<196> Section 3.2.5.14: If the OutputCount field in an SMB2 IOCTL Response is 0 and the OutputOffset exceeds the size of the SMB2 response, Windows clients will return STATUS_INVALID_NETWORK_RESPONSE to the application.
<197> Section 3.2.5.14.9: Windows clients enable TCP keepalives to detect broken connections.
<198> Section 3.2.5.14.11: Windows-based SMB2 clients will choose the interfaces using the following criteria:
Skip the interfaces in NETWORK_INTERFACE_INFO Response where IfIndex is 0.
For each interface returned in NETWORK_INTERFACE_INFO Response, if the interface has both link-local and non-link-local IP addresses, skip the link-local IP address.
If there is one or more multiple link-local addresses (suppose there are Y such interfaces), select local interfaces which have only link-local addresses (suppose there are X such local interfaces).
Build a destination address list, include all server non-link-local addresses and X*Y server link-local addresses.
For each RDMA capable address pair, duplicate the address pair, one for RDMA and one for Direct TCP.
Sort address pairs by which address pair is best suited for connection between client and server.
For each address pair, compute
Link speed of the pair = min( link speed of local interface, link speed of remote interface)
RSS capable = RSS capable of local interface and RSS capable of remote interface
If there are RDMA capable address pairs, select them.
Otherwise if there are RSS capable address pairs, select them.
Otherwise select remaining address pairs.
Select the pairs with the highest link speed from the selected address pairs.
Select local/remote address pairs so that all eligible local/remote interfaces are used and the connections are distributed among local and remote interfaces.
By default, Windows clients create four connections per RSS-capable address pair or two connections per RDMA-capable address pair or only a single connection when the address pair is neither RSS-capable nor RDMA-capable.
<199> Section 3.2.5.14.11: By default, Windows 8 and later will try to establish alternate channels if Connection.OutstandingRequests exceeds 8. By default, Windows Server 2012 operating system and later will try to establish alternate channels if Connection.OutstandingRequests exceeds 1.
<200> Section 3.2.5.16:  Windows SMB2 clients without [MSFT-CVE-2025-29956] do not perform this validation.
<201> Section 3.2.5.18: Windows 8 operating system and later and Windows Server 2012 operating system and later replay the SetInfo operation up to three times or until all of the channels in the session are disconnected.
<202> Section 3.2.5.19.2:  Windows clients do not send a Lease Break Acknowledgement when they have an outstanding SMB2 CREATE Request on the same File.
<203> Section 3.2.6.1: Windows clients use a default time-out of 60 seconds.
<204> Section 3.2.6.1: Windows-based clients return a STATUS_CONNECTION_DISCONNECTED error code to the calling application.
<205> Section 3.2.6.1: The Windows-based clients will disconnect the connection.
<206> Section 3.2.7.1:  When the reestablishment of the durable handle fails with a network error, Windows clients retry the reestablishment three times.
<207> Section 3.3.1.1: Windows-based servers will limit the maximum range of sequence numbers. If a client has been granted 10 credits, the server will not allow the difference between the smallest available sequence number and the largest available sequence number to exceed 2*10 = 20. Therefore, if the client has sequence number 10 available and does not send it, the server will stop granting credits as the client nears sequence number 30, and eventually will grant no further credits until the client sends sequence number 10.
<208> Section 3.3.1.2: A Windows-based server will grant some portion of the client request based on available resources and the number of credits the client is currently taking advantage of. A Windows–based server grants credits based on usage but will attempt to enforce fairness if there are insufficient credits.
<209> Section 3.3.1.2: Windows-based SMB2 servers support a configurable minimum credit limit below which the client is unconditionally granted all credits it requests, and a configurable maximum credit limit above which credits are never granted, as follows:
<210> Section 3.3.1.2: A Windows–based server does not currently scale credits based on quality of service features.
<211> Section 3.3.1.4:  On Windows 7 and Windows Server 2008 R2, a 128-bit ClientLeaseId is generated by an arithmetic combination of LeaseKey and ClientGuid, which is passed to the object store at open/create time. On Windows 8 operating system and later and Windows Server 2012 operating system and later, the LeaseKey in the request is used as the ClientLeaseId.
<212> Section 3.3.1.4: Windows 7 operating system and later and Windows Server 2008 R2 operating system and later based SMB2 servers support only the levels described above, and Windows 7 operating system and later and Windows Server 2008 R2 operating system and later based SMB2 clients request only those levels.
<213> Section 3.3.1.6: Windows-based servers allow the sharing of both printers and traditional file shares.
<214> Section 3.3.1.6: In Windows, this abstract state element contains the security descriptor for the share.
<215> Section 3.3.1.6: Windows-based SMB2 clients do not cache directory enumeration results.
<216> Section 3.3.1.13: The Windows SMB2 server allocates an I/O request (IRP) structure which it uses to locally request action from the object store. The Request.CancelRequestId is set to the unique address of this structure.
<217> Section 3.3.2.1: Windows SMB2 servers set this timer to 35 seconds.
<218> Section 3.3.2.2: Windows-based SMB2 servers set this timer to a constant value of 16 minutes.
<219> Section 3.3.2.3: Windows-based servers implement this timer with a constant value of 45 seconds.
<220> Section 3.3.2.5:  Windows SMB2 servers set this timer to 35 seconds.
<221> Section 3.3.3:  Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, Windows Server 2012 R2, Windows 10 v1507 through Windows 10 v1703, and Windows Server 2016 set the ServerStartTime to the time at which the SMB2 server was started.
<222> Section 3.3.3: Windows-based SMB2 servers set this value to 256.
<223> Section 3.3.3: Windows-based SMB2 servers set this value to 1 MB.
<224> Section 3.3.3: Windows-based SMB2 servers set this value to 16 MB.
<225> Section 3.3.3: Windows-based servers initialize ServerHashLevel based on a stored value in the registry.
<226> Section 3.3.3: Windows 7 operating system and later and Windows Server 2008 R2 operating system and later SMB2 servers provide a constant maximum resiliency time-out of 300000 milliseconds.
<227> Section 3.3.3: Windows 8 operating system and later and Windows Server 2012 operating system and later by default, set RejectUnencryptedAccess to TRUE. If the registry value RejectUnencryptedAccess under HKLM\System\CurrentControlSet\Services\LanmanServer\Parameters\ is set to zero, RejectUnencryptedAccess is set to FALSE.
<228> Section 3.3.3: Windows 8 operating system and later and Windows Server 2012 operating system and later set IsMultiChannelCapable to TRUE.
<229> Section 3.3.3:  Windows 8 operating system and later and Windows Server 2012 operating system and later initialize AllowAnonymousAccess based on a stored value in the registry.
<230> Section 3.3.3: Windows 10 v1709, Windows Server operating system operating system and later set this value to TRUE.
<231> Section 3.3.3: By default, Windows 11 operating system and later and Windows Server 2022 operating system and later set AllowNamedPipeAccessOverQUIC to FALSE.
<232> Section 3.3.3:  Windows 11 with [MSKB-5035854], Windows 11 v22H2 with [MSKB-5035942], Windows Server 2022 with [MSKB-5035857], Windows Server 2022, 23H2, Windows 11, version 24H2 and later, and Windows Server 2025 and later set IsMutualAuthOverQUICSupported to TRUE.
Windows 11 with [MSKB-5035854], Windows 11 v22H2 with [MSKB-5035942], Windows Server 2022 with [MSKB-5035857], Windows Server 2022, 23H2 with [MSKB-5035856], Windows 11, version 24H2 and later, and Windows Server 2025 and later support Client Access Control capability over QUIC.
<233> Section 3.3.4.1.1: Windows-based servers always sign the final session setup response when the user is neither anonymous nor guest.
Windows 8, Windows Server 2012, Windows 8.1 without [MSKB-2976995] and Windows Server 2012 R2 without [MSKB-2976995] servers fail to sign responses other than SMB2_NEGOTIATE, SMB2_SESSION_SETUP, and SMB2_TREE_CONNECT when Session.SigningRequired is TRUE, global EncryptData is TRUE, RejectUnencryptedAccess is FALSE and either Connection.Dialect is "2.0.2" or "2.1" or Connection.ClientCapabilities does not include SMB2_GLOBAL_CAP_ENCRYPTION.
<234> Section 3.3.4.1.2: For an asynchronously processed request, Windows-based servers grant credits on the interim response and do not grant credits on the final response. The interim response grants credits to keep the transaction from stalling in case the client is out of credits.
<235> Section 3.3.4.1.3: The Windows-based server compounds responses for any received compounded operations. Otherwise, it does not compound responses.
<236> Section 3.3.4.1.3: When there are not enough credits to process a subsequent compounded request, Windows SMB2 servers set the NextCommand field to the size of the last SMB2 response message including the SMB2 header.
<237> Section 3.3.4.1.3: Windows-based servers grant all credits in the final response of the compounded chain, and grant 0 credits in all responses other than the final response.
<238> Section 3.3.4.1.3: Windows-based servers do not calculate the size of the response message; servers depend on the transport to send the response message.
<239> Section 3.3.4.1.5: Windows 10 v2004, Windows 10 v20H2, Windows Server v2004, and Windows Server v20H2 do not compress the message if Connection.CompressionIds does not include LZNT1, LZ77 and LZ77+Huffman algorithms.
<240> Section 3.3.4.2: Windows-based servers send interim responses for the following operations if they cannot be completed immediately:
SMB2_CREATE, if the underlying object store indicates an Oplock/Lease Break Notification or if access/sharing modes are incompatible with another existing open
SMB2_CHANGE_NOTIFY
Byte Range Lock
Named Pipe Read on a blocking named pipe
Named Pipe Write on a blocking named pipe
Large file write
FSCTL_PIPE_TRANSCEIVE
FSCTL_SRV_COPYCHUNK or FSCTL_SRV_COPYCHUNK_WRITE, when oplock break happens
SMB2 FLUSH on a named pipe
FSCTL_GET_DFS_REFERRALS
<241> Section 3.3.4.2: Windows-based servers incorrectly process the FSCTL_PIPE_WAIT request on named pipes synchronously.
<242> Section 3.3.4.2: Windows-based servers enforce a configurable blocking operation credit, which defaults to 64 on Windows Vista SP1 operating system and later, and defaults to 512 on Windows Server 2008 operating system and later.
<243> Section 3.3.4.4: For Windows 7 operating system and later and Windows Server 2008 R2 operating system and later, STATUS_BUFFER_OVERFLOW will be returned for FSCTL_GET_RETRIEVAL_POINTERS and FSCTL_GET_REPARSE_POINT, along with the ones mentioned in section 3.3.4.4.
<244> Section 3.3.4.6: In Windows-based SMB2 servers, underlying object store never breaks opportunistic lock to SMB2_OPLOCK_LEVEL_EXCLUSIVE oplock level.
<245> Section 3.3.4.6: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 set the SessionId in the SMB2 header to zero.
<246> Section 3.3.4.6: Windows-based SMB2 servers set Open.OplockTimeout to the current time plus 35000 milliseconds. If Open.IsPersistent is TRUE, Open.OplockTimeout is set to the current time plus 60000 milliseconds.
<247> Section 3.3.4.7: Windows-based SMB2 servers set Lease.LeaseBreakTimeout to the current time plus 35000 milliseconds. If Open.IsPersistent is TRUE, Windows 8 and Windows Server 2012 set Lease.LeaseBreakTimeout to the current time plus 60000 milliseconds. If Open.IsPersistent is TRUE, Windows 8.1 operating system and later and Windows Server 2012 R2 operating system and later set Lease.LeaseBreakTimeout to the current time plus 180000 milliseconds.
<248> Section 3.3.4.7:  Windows 8 through Windows 10 v1909, Windows Server 2012 through Windows Server v1909, Windows 10 v2004 through Windows 10, version 22H2 operating system without [MSKB-5037849], Windows Server v2004 through Windows Server v20H2 without [MSKB-5037849], Windows 11 without [MSKB-5039213], Windows 11 v22H2 without [MSKB-5037853], Windows 11, version 23H2 without [MSKB-5037853], Windows 11, version 24H2 without [MSKB-5040529], Windows Server 2022 without [MSKB-5039227] and Windows Server 2022, 23H2 without [MSKB-5039236] and later do not increment Lease.Epoch when setting NewEpoch in Lease Break Notification in the following cases:
On the server initiated close of an open which is the last open in Open.Lease.LeaseOpens, the server sends a Lease Break Notification to break the lease by setting NewLeaseState to SMB2_LEASE_NONE and Status field in the SMB2 Header to STATUS_FILE_CLOSED.
While handling a Lease Break Acknowledgment, due to a conflicting open, if the object store does not grant WRITE_CACHING or HANDLE_CACHING, as specified in [MS-FSA] section 2.1.5.19, the server sends another Lease Break Notification to further downgrade the lease state.
<249> Section 3.3.4.13: Windows Server 2012 and Windows Server 2012 R2 set these bits as appropriate for shared volume configurations.
<250> Section 3.3.4.13: By default, Windows 8 operating system and later and Windows Server 2012 operating system and later set Share.CATimeout to zero.
<251> Section 3.3.4.17: Windows Lease break is described in [MS-FSA] section 2.1.5.18. The Open parameter passed is the Open.Local value from the current close operation, the Type parameter is LEVEL_GRANULAR to indicate a Lease request, and the RequestedOplockLevel parameter is zero.
Windows servers never send SMB2 Lease Break Notification to the client when the Open is being closed.
<252> Section 3.3.4.21: For each supported transport type as listed in section 2.1, the Windows SMB2 server attempts to form an association with the specified device with local calls specific to each supported transport type and rejects the entry if none of the associations succeed.
<253> Section 3.3.4.21: On Windows, ServerName is used only when the transport is NetBIOS over TCP.
<254> Section 3.3.5.1: Possible Windows-specific values for Connection.TransportName are listed in a product behavior note attached to [MS-SRVS] section 2.2.4.96.
<255> Section 3.3.5.2: Windows performs cancellation of in-progress requests via the interface in [MS-FSA] section 2.1.5.20, Server Requests Canceling an Operation, passing Request.CancelRequestId as an input parameter.
<256> Section 3.3.5.2:  Windows 10 v1903 and later, and Windows Server v1903 and later set this to TRUE.
<257> Section 3.3.5.2: Windows 7 without [MSKB-2536275], and Windows Server 2008 R2 without [MSKB-2536275] terminate the connection when the size of the request is greater than 64*1024 bytes.
Windows Vista SP1 and Windows Server 2008 on Direct TCP transport disconnect the connection if the size of the message exceeds 128*1024 bytes, and Windows Vista SP1 and Windows Server 2008 on NetBIOS over TCP transport will disconnect the connection if the size of the message exceeds 64*1024 bytes.
<258> Section 3.3.5.2.1.1: Windows-based servers will discard the message if it is encrypted and the connection is NetBIOS over TCP.
<259> Section 3.3.5.2.1.1: Windows-based servers will not disconnect the connection.
<260> Section 3.3.5.2.1.1: Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 disconnect the connection if OriginalMessageSize is greater than 1028 kilobytes.
<261> Section 3.3.5.2.1.2: Windows-based servers discard the message if it is compressed and the connection is over RDMA.
<262> Section 3.3.5.2.3: For an SMB2 Write request with an invalid MessageId, Windows 8 and Windows Server 2012 will stop processing the request and any further requests on that connection.
<263> Section 3.3.5.2.4: Windows-based servers will not disconnect the connection due to a mismatched signature.
<264> Section 3.3.5.2.4: Windows-based servers will not disconnect the connection due to an unsigned packet.
<265> Section 3.3.5.2.6: Windows-based servers will disconnect the connection when it processes packets that are smaller than the SMB2 header or packets that contain an invalid SMB2 command. For all other validations, it will not disconnect the connection but simply return the error.
<266> Section 3.3.5.2.7: In Windows Vista and later, and Windows Server 2008 and later, when an operation in a compound request requires asynchronous processing, Windows-based servers fail them with STATUS_INTERNAL_ERROR except for the following two cases: when a create request in the compound request triggers an oplock break, or when the operation is last in the compound request.
In all SMB2 servers, if a create request in a compound chain is processed asynchronously due to an oplock break, Windows-based servers send an interim response to the client. If there are one or more conflicting create operations in a compounded request, Windows-based servers send an oplock break notification for the completed create prior to sending any response, and the level of the broken oplock is not updated in all prior create responses in the compound response.
<267> Section 3.3.5.2.7: Windows-based servers ignore 8-byte alignment boundary checking in a compounded chain.
<268> Section 3.3.5.2.7: Windows-based SMB2 servers allow a mix of related and unrelated compound requests in the same transport send. Upon encountering a request with SMB2_FLAGS_RELATED_OPERATIONS not set, a Windows-based SMB2 server treats it as the start of a chain.
<269> Section 3.3.5.2.7.2: If SMB2_FLAGS_RELATED_OPERATIONS is present in the first request, Windows-based servers fail all related requests in the compounded chain with error STATUS_INVALID_PARAMETER.
<270> Section 3.3.5.2.7.2: If the previous session expired, Windows Vista SP1, Windows Server 2008, Windows 7, and Windows Server 2008 R2 servers fail the next request in the compounded chain with STATUS_NETWORK_SESSION_EXPIRED, and the subsequent requests in the compounded chain will be failed with STATUS_INVALID_PARAMETER.
<271> Section 3.3.5.2.9: Windows Vista SP1, Windows Server 2008, Windows 7, and Windows Server 2008 R2 servers do not fail the request if the SMB2 header of the request has SMB2_FLAGS_SIGNED set in the Flags field and the request is not an SMB2 LOCK request as specified in section 2.2.26.
<272> Section 3.3.5.2.9: Windows-based servers fail the request with 0x80090302 when the authentication method is GSS-API.
<273> Section 3.3.5.2.10: Windows 8 and Windows Server 2012 perform the following:
If Open.OutstandingPreRequestCount is equal to zero,
Set Open.ChannelSequence to ChannelSequence in the SMB2 Header.
Increment Open.OutstandingPreRequestCount by Open.OutstandingRequestCount.
Set Open.OutstandingRequestCount to 1.
Otherwise, fail the request with STATUS_FILE_NOT_AVAILABLE.
<274> Section 3.3.5.3.1: If the underlying transport is NETBIOS over TCP, Windows-based servers set MaxTransactSize to 65536. Otherwise, MaxTransactSize is set based on the following table.
<275> Section 3.3.5.3.1: If the underlying transport is NETBIOS over TCP, Windows-based servers set MaxReadSize to 65536. Otherwise, MaxReadSize is set based on the following table.
<276> Section 3.3.5.3.1: If the underlying transport is NETBIOS over TCP, Windows-based servers set MaxWriteSize to 65536. Otherwise, MaxWriteSize is based on the following table.
<277> Section 3.3.5.3.1: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, Windows Server 2012 R2, Windows 10 v1507 through Windows 10 v1703, and Windows Server 2016 set the ServerStartTime to the global ServerStartTime value.
<278> Section 3.3.5.3.2: Windows-based servers set this to a default value of 65536.
<279> Section 3.3.5.3.2: Windows-based servers set MaxReadSize to a default value of 65536.
<280> Section 3.3.5.3.2: Windows-based servers set MaxWriteSize to a default value of 65536.
<281> Section 3.3.5.3.2: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, Windows Server 2012 R2, Windows 10 v1507 through Windows 10 v1703, and Windows Server 2016 set the ServerStartTime to the global ServerStartTime value.
<282> Section 3.3.5.4: Windows 10 v1903, Windows 10 v1909, Windows Server v1903, and Windows Server v1909 only set CompressionAlgorithms to the first common algorithm supported by the client and server.
Windows 10 v2004 and Windows Server v2004 select a common pattern scanning algorithm and the first common compression algorithm, specified in section 2.2.3.1.3, supported by the client and server.
<283> Section 3.3.5.4: If the underlying transport is NETBIOS over TCP, Windows-based servers set MaxTransactSize to 65536. Otherwise, MaxTransactSize is set based on the following table.
<284> Section 3.3.5.4: If the underlying transport is NETBIOS over TCP, Windows-based servers set MaxReadSize to 65536. Otherwise, MaxReadSize is set based on the following table.
<285> Section 3.3.5.4: If the underlying transport is NETBIOS over TCP, Windows-based servers set MaxWriteSize to 65536. Otherwise, MaxWriteSize is set based on the following table.
<286> Section 3.3.5.4:  Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, Windows Server 2012 R2, Windows 10 v1507 through Windows 10 v1703, and Windows Server 2016 set the ServerStartTime to the global ServerStartTime value.
<287> Section 3.3.5.4: Windows 10, and Windows Server 2016 operating system and later use 32 bytes of Salt.
<288> Section 3.3.5.4: Windows 10 v2004, Windows Server v2004, Windows 10 v20H2, Windows Server v20H2, and Windows 10 v21H1 operating system operating systems without [MSKB-5001391] set CompressionAlgorithmCount to 0.
<289> Section 3.3.5.4: Windows 10 v2004, Windows Server v2004, Windows 10 v20H2, Windows Server v20H2, and Windows 10 v21H1 operating systems without [MSKB-5001391] set CompressionAlgorithms to empty.
<290> Section 3.3.5.5: Windows 8 and Windows Server 2012 look up the session in GlobalSessionTable using the SessionId from the SMB2 header if the SMB2_SESSION_FLAG_BINDING bit is set in the Flags field of the request. If the session is found, the server fails the request with STATUS_REQUEST_NOT_ACCEPTED. If the session is not found, the server fails the request with STATUS_USER_SESSION_DELETED.
<291> Section 3.3.5.5: Windows Vista SP1 and Windows Server 2008 servers fail the session setup request with STATUS_REQUEST_NOT_ACCEPTED.
<292> Section 3.3.5.5.3: Windows Vista SP1 operating system and later and Windows Server 2008 operating system and later will also accept raw Kerberos messages and implicit NTLM messages as part of GSS authentication.
<293> Section 3.3.5.5.3: Windows by default uses the guest account to represent guest users. Alternatively, any user account that is a member of the well-known BUILTIN_GUESTS or DOMAIN_GUESTS group (see [MS-DTYP] section 2.4.2.4) is considered a guest account.
<294> Section 3.3.5.5.3: Windows 7 and Windows Server 2008 R2 remove the current session from GlobalSessionTable and Connection.SessionTable but the SESSION_SETUP request succeeds, if the PreviousSessionId and SessionId values in the SMB2 header of the request are equal and the authentications were for the same user. Further requests using this SessionId will fail with STATUS_USER_SESSION_DELETED.
<295> Section 3.3.5.6: Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 servers will not reset ResilientOpenScavengerExpiryTime.
<296> Section 3.3.5.7: Windows-based SMB2 servers do not set this bit in the ShareFlags field.
<297> Section 3.3.5.7: Windows-based SMB2 servers do not set this bit in the ShareFlags field.
<298> Section 3.3.5.7: Windows Server 2012 and Windows Server 2012 R2 set these two bits based on group policy settings.
<299> Section 3.3.5.7: Windows Vista SP1 and Windows Server 2008 do not support the SMB2_SHAREFLAG_ENABLE_HASH_V1 bit.
<300> Section 3.3.5.7: Windows Server v1709 and later support the SMB2_SHARE_CAP_REDIRECT_TO_OWNER bit.
<301> Section 3.3.5.9:  If Open.ClientGuid is not equal to the ClientGuid of the connection that received this request, Open.Lease.LeaseState is equal to RWH, or Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_BATCH, Windows-based servers will attempt to break the lease/oplock and return STATUS_PENDING to process the create request asynchronously. Otherwise, if Open.Lease.LeaseState does not include SMB2_LEASE_HANDLE_CACHING and Open.OplockLevel is not equal to SMB2_OPLOCK_LEVEL_BATCH, Windows-based servers return STATUS_FILE_NOT_AVAILABLE.
<302> Section 3.3.5.9: Windows Vista and Windows Server 2008 validate the create requests before session verification as described in the "Create Context Validation" phase in section 3.3.5.9.
<303> Section 3.3.5.9: Windows-based servers accept the path names containing Dot Directory Names specified in [MS-FSCC] section 2.1.5.1 and attempt to normalize the path name by removing the pathname components of "."  and "..". Windows-based servers fail the CREATE request with STATUS_INVALID_PARAMETER if the file name in the Buffer field of the request begins in the form "subfolder\..\", for example "x\..\y.txt".
<304> Section 3.3.5.9: Windows-based SMB2 servers fail an SMB2 CREATE request with STATUS_ACCESS_DENIED if the file name in the request is one of the following: "LPT1", "LPT2", "LPT3","LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "PRN", "AUX", "NUL", "CON", and "CLOCK$".
<305> Section 3.3.5.9: Windows-based servers ignore DesiredAccess values other than FILE_WRITE_DATA, FILE_APPEND_DATA and GENERIC_WRITE if any one of these values is specified.
<306> Section 3.3.5.9: Windows-based servers fail requests having a CreateDisposition of FILE_OPEN or FILE_OVERWRITE, but ignore values of FILE_SUPERSEDE, FILE_OPEN_IF and FILE_OVERWRITE_IF.
<307> Section 3.3.5.9:  Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 do not perform this verification and continue to process the request.
<308> Section 3.3.5.9: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, and Windows Server 2012 do not perform this verification.
<309> Section 3.3.5.9: Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2 operating systems do not perform this verification and continue to process the request.
<310> Section 3.3.5.9: Windows-based SMB2 servers check only for FILE_WRITE_DATA, FILE_WRITE_ATTRIBUTES, FILE_WRITE_EA, and FILE_APPEND_DATA in the DesiredAccess field.
<311> Section 3.3.5.9: Windows performs the access check by mapping SMB2 parameters to the object store parameters as described in [MS-FSA] section 2.1.4.14 AccessCheck -- Algorithm to Perform a General Access Check.
<312> Section 3.3.5.9: Windows Vista SP1 and Windows Server 2008 do not support the SMB2_SHAREFLAG_FORCE_LEVELII_OPLOCK flag and ignore the TreeConnect.Share.ForceLevel2Oplock value.
<313> Section 3.3.5.9: Windows performs the following open/create mappings from SMB2 parameters to the object store as described in [MS-FSA] section 2.1.5.1 Server Requests an Open of a File.
Windows performs the following mappings from object store results to SMB2 response.
If TreeConnect.Share.Type is STYPE_DISKTREE, CreateDisposition is FILE_SUPERSEDE, FILE_OVERWRITE or FILE_OVERWRITE_IF, DesiredAccess does not include FILE_WRITE_DATA, FILE_APPEND_DATA or GENERIC_WRITE, and there is an existing Open in GlobalOpenTable where Open.FileName matches the FileName in the request, Open.TreeConnect.Share.IsCA is TRUE, Open.ShareAccess does not include FILE_SHARE_WRITE, Windows 8 and later and Windows Server 2012 and later fail the request with STATUS_OBJECT_NAME_NOT_FOUND.
<314> Section 3.3.5.9: Windows-based servers will receive the data from the local create operation for constructing the error response when a symbolic link is present in the target path name.
<315> Section 3.3.5.9:  If TreeConnect.Share.Type is STYPE_DISKTREE, CreateDisposition is FILE_SUPERSEDE, FILE_OVERWRITE or FILE_OVERWRITE_IF, DesiredAccess does not include FILE_WRITE_DATA, FILE_APPEND_DATA or GENERIC_WRITE, and there is an existing Open in GlobalOpenTable where Open.FileName matches the FileName in the request, Open.TreeConnect.Share.IsCA is TRUE, Open.ShareAccess does not include FILE_SHARE_WRITE, Windows 8 and later and Windows Server 2012 and later will retry the create with disposition FILE_OPEN.
<316> Section 3.3.5.9: Windows Oplock acquisition is described in [MS-FSA] section 2.1.5.18. Oplock acquisition is an optional step in open/create processing; the Open parameter passed is the Open.Local result from the open or create operation, and the Type parameter is mapped as follows.
The Status code returned indicates whether the requested oplock was granted.
<317> Section 3.3.5.9: Windows obtains CreationTime from the object store FileBasicInformation [MS-FSA] section 2.1.5.12.6 and [MS-FSCC] section 2.4.7.
<318> Section 3.3.5.9: Windows obtains LastAccessTime from the object store FileBasicInformation [MS-FSA] section 2.1.5.12.6 and [MS-FSCC] section 2.4.7.
<319> Section 3.3.5.9: Windows obtains LastWriteTime from the object store FileBasicInformation [MS-FSA] section 2.1.5.12.6 and [MS-FSCC] section 2.4.7.
<320> Section 3.3.5.9: Windows obtains ChangeTime from the object store FileBasicInformation [MS-FSA] section 2.1.5.12.6 and [MS-FSCC] section 2.4.7.
<321> Section 3.3.5.9: Windows obtains AllocationSize from the object store FileStandardInformation [MS-FSA] section 2.1.5.12.27 and [MS-FSCC] section 2.4.47.
<322> Section 3.3.5.9: Windows-based SMB2 servers will set AllocationSize to any value for the named pipe.
<323> Section 3.3.5.9: Windows obtains EndOfFile from the object store FileStandardInformation [MS-FSA] section 2.1.5.12.27 and [MS-FSCC] section 2.4.47.
<324> Section 3.3.5.9: Windows-based SMB2 servers will set EndofFile to any value for the named pipe.
<325> Section 3.3.5.9: Windows obtains FileAttributes from the object store FileBasicInformation [MS-FSA] section 2.1.5.12.6 and [MS-FSCC] section 2.4.7.
<326> Section 3.3.5.9.1: Windows sets extended attributes on a newly created file with the FSCTL_SET_OBJECT_ID_EXTENDED FSCTL [MS-FSA] section 2.1.5.10.36 and [MS-FSCC] section 2.3.81.
<327> Section 3.3.5.9.2: Windows sets security attributes on a newly created file with the Application requests setting of security information [MS-FSA] section 2.1.5.17.
<328> Section 3.3.5.9.2: Windows will ignore security descriptors if the underlying object store does not support them.
<329> Section 3.3.5.9.3: Windows-based servers support this request.
<330> Section 3.3.5.9.3: Windows sets allocation size on a newly created file with the FileAllocationInformation [MS-FSA] section 2.1.5.15.1 and [MS-FSCC] section 2.4.4, after converting bytes to volume cluster size.
<331> Section 3.3.5.9.4: Windows validates that a snapshot with the time stamp provided exists by forming a FileBothDirectoryInformation object store request for the file including the provided @GMT token in the path, as described in [MS-SMB] section 2.2.1.1.1 and [MS-FSA] section 2.1.5.6.3.1.
<332> Section 3.3.5.9.4: Windows opens a file on a snapshot with the time stamp provided by the file including the provided @GMT token in the path, as described in [MS-SMB] section 2.2.1.1.1 and [MS-FSA] section 2.1.5.1.
<333> Section 3.3.5.9.5: Windows computes the MaximalAccess to return by querying the security attributes of the file with [MS-FSA] section 2.1.5.14, and performing an access check against the credentials provided by the request. QueryStatus is set to the Status returned in that operation.
<334> Section 3.3.5.9.6: Windows Vista SP1, Windows 7, Windows Server 2008, and Windows Server 2008 R2 ignore undefined create contexts.
<335> Section 3.3.5.9.6:  Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2 set Open.DurableOpenTimeout to 16 minutes. Windows 8, Windows Server 2012, Windows 8.1, Windows Server 2012 R2, Windows 10, Windows Server 2016, and Windows Server set Open.DurableOpenTimeout to 2 minutes.
<336> Section 3.3.5.9.7: Windows Vista SP1, Windows Server 2008, Windows 7 and Windows Server 2008 R2 ignore undefined create contexts.
<337> Section 3.3.5.9.7: If the Session was established by invalidating the previous session by specifying PreviousSessionId in the SMB2 SESSION_SETUP request, Windows 8.1 and Windows Server 2012 R2 close the durable opens established on the previous session.
<338> Section 3.3.5.9.7: Windows 8, Windows Server 2012, Windows 8.1 and Windows Server 2012 R2 do not perform lease version verification.
<339> Section 3.3.5.9.7: Windows Vista SP1, Windows Server 2008, Windows 7, and Windows Server 2008 R2 servers respond with the SMB2_CREATE_DURABLE_HANDLE_RESPONSE create context after a successful reconnect of a durable open.
<340> Section 3.3.5.9.8: Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 do not ignore the SMB2_CREATE_REQUEST_LEASE create context when RequestedOplockLevel is not equal to SMB2_OPLOCK_LEVEL_LEASE.
<341> Section 3.3.5.9.8: On Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2, the Lease.ClientLeaseId is passed to the object store when processing continues at open/create time. A new or existing lease is thereby associated with the resulting open.
To acquire or promote the lease as dictated by the SMB2_CREATE_REQUEST_LEASE Create Context, a subsequent object store call is invoked as described in [MS-FSA] section 2.1.5.18. The Open parameter passed is an internally-managed open that refers to the same file, stream, and oplock key as Open.LocalOpen but is otherwise distinct from Open.LocalOpen, and the Type parameter is LEVEL_GRANULAR to indicate a Lease request. The RequestedOplockLevel parameter is constructed to include zero or more bits as follows.
The Status code returned indicates whether the requested lease was granted.
<342> Section 3.3.5.9.10:  Windows-based servers send the SMB2_CREATE_DURABLE_HANDLE_RESPONSE_V2 response create context to the client if any of the following conditions is satisfied:
Open.IsPersistent is TRUE
Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_BATCH
Open.Lease.LeaseState contains SMB2_LEASE_HANDLE_CACHING
<343> Section 3.3.5.9.10:  If the Timeout value in the request is not zero, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 SMB2 servers set Timeout to the Timeout value in the request.
<344> Section 3.3.5.9.10:  If the Timeout value in the request is zero and Share.CATimeout is not zero, Windows 8, Windows Server 2012, Windows 8.1, Windows Server 2012 R2, Windows 10, Windows Server 2016, and Windows Server SMB2 servers set Timeout to Share.CATimeout. If the Timeout value in the request is zero and Share.CATimeout is zero, Windows 8 and Windows Server 2012 SMB2 servers set Timeout to 60 seconds.
<345> Section 3.3.5.9.11: Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 servers do not ignore the SMB2_CREATE_REQUEST_LEASE_V2 create context when Connection.Dialect is equal to "2.1" or if RequestedOplockLevel is not equal to SMB2_OPLOCK_LEVEL_LEASE.
<346> Section 3.3.5.9.11:  On Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2, the Lease.ClientLeaseId and Lease.ParentLeaseKey are passed to the object store in the form of TargetOplockKey and ParentOplockKey. A new or existing lease is thereby associated with the resulting open.
To acquire or promote the lease as dictated by the SMB2_CREATE_REQUEST_LEASE_V2 Create Context, a subsequent object store call is invoked as described in [MS-FSA] section 2.1.5.18 Server Requests an Oplock. The Open parameter passed is an internally-managed open that refers to the same file, stream, and oplock key as Open.LocalOpen but is otherwise distinct from Open.LocalOpen, and the Type parameter is LEVEL_GRANULAR to indicate a Lease request. The RequestedOplockLevel field is constructed to include zero or more bits as follows.
The Status code returned indicates whether the requested lease was granted.
<347> Section 3.3.5.9.12: Windows 8 with [KB2770917] and Windows Server 2012 with [KB2770917] fail the CREATE request with STATUS_INVALID_PARAMETER.
<348> Section 3.3.5.9.12: If the Session was established by specifying PreviousSessionId in the SMB2 SESSION_SETUP request, therefore invalidating the previous session, Windows 8.1 and Windows Server 2012 R2 close the durable opens established on the previous session.
<349> Section 3.3.5.9.12: If Open.OplockLevel is equal to SMB2_OPLOCK_LEVEL_BATCH or Open.Lease.LeaseState includes SMB2_LEASE_HANDLE_CACHING, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 continue to process the request.
<350> Section 3.3.5.9.12:  If Open.IsPersistent is TRUE, Open.Lease.LeaseState does not contain SMB2_LEASE_HANDLE_CACHING, Open.OplockLevel is not equal to SMB2_OPLOCK_LEVEL_BATCH, SMB2_DHANDLE_FLAG_PERSISTENT bit is set in the Flags field of the SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 Create Context and there is another existing Open in the GlobalOpenTable on the same file with same LeaseKey and Open.IsPersistent is TRUE, Windows 8 and later and Windows Server 2012 and later fail the request with STATUS_FILE_NOT_AVAILABLE.
<351> Section 3.3.5.9.12: Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 do not perform Lease version verification.
<352> Section 3.3.5.9.12:  Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 do not perform this verification and continue to process the request.
<353> Section 3.3.5.9.12: When an open, with Open.IsPersistent set to TRUE, is being reconnected due to server failover, Windows 8 through Windows 11, version 23H2 and Windows Server 2012 through Windows Server 2022, 23H2 perform the following:
If Lease.LeaseState includes SMB2_LEASE_WRITE_CACHING, Epoch and Lease.Epoch are set to Epoch field in the Create Context request.
If Lease.LeaseState does not include SMB2_LEASE_WRITE_CACHING, Epoch and Lease.Epoch are set to Epoch field in the Create Context request incremented by 1.
When an open, with Open.IsPersistent set to TRUE, is being reconnected due to server failover, Windows 11, version 24H2 and Windows Server 2025 perform the following:
If LeaseState in the Create Context request is 0, Epoch is set to Lease.Epoch.
If LeaseState in the Create Context request includes SMB2_LEASE_WRITE_CACHING and is successfully restored to Lease.LeaseState, Epoch is set to Lease.Epoch.
If LeaseState in the Create Context request does not include SMB2_LEASE_WRITE_CACHING and Lease.LeaseState is not 0, Lease.LeaseEpoch is incremented by 1 and Epoch is set to Lease.Epoch.
<354> Section 3.3.5.9.13: Windows SMB3 servers compute the maximal access to return by querying the security attributes of the file with [MS-FSA] section 2.1.5.14, and performing an access check against the credentials provided by the request.
<355> Section 3.3.5.9.13:  Windows Server 2012 and Windows Server 2012 R2 servers do not close the open.
<356> Section 3.3.5.10: Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2 validate the open before verifying the session.
<357> Section 3.3.5.10: Windows obtains FileNetworkOpenInformation from the object store as described in [MS-FSA] section 2.1.5.12.21 and [MS-FSCC] section 2.4.34.
Windows-based servers do not return an updated ChangeTime unless Open.GrantedAccess includes FILE_WRITE_DATA, FILE_WRITE_ATTRIBUTES, FILE_WRITE_EA, or FILE_APPEND_DATA and any prior WRITE/SET_INFO operations were performed on that Open.
<358> Section 3.3.5.11: Windows flushes any cached data to the file with Server Requests Flushing Cached Data [MS-FSA] section 2.1.5.7.
<359> Section 3.3.5.11: If the request target is a named pipe or file, Windows-based servers handle this request asynchronously.
<360> Section 3.3.5.12: Windows 7 and Windows Server 2008 R2 fail the request with STATUS_BUFFER_OVERFLOW if the Length field is greater than Connection.MaxReadSize. Windows Vista SP1 and Windows Server 2008 will fail the request with STATUS_BUFFER_OVERFLOW if the Length field is greater than 524288.
<361> Section 3.3.5.12: Windows reads from a file with Server Requests a Read [MS-FSA] section 2.1.5.3.
<362> Section 3.3.5.12: Windows SMB2 servers send an interim response to the client and handle the read asynchronously if the read is not finished in 0.5 milliseconds.
<363> Section 3.3.5.12: Windows-based servers handle the following commands asynchronously: SMB2 Create (section 2.2.13) when this create would result in an oplock break, SMB2 IOCTL Request (section 2.2.31) for FSCTL_PIPE_TRANSCEIVE if it blocks for more than 1 millisecond, SMB2 IOCTL Request for FSCTL_SRV_COPYCHUNK or FSCTL_SRV_COPYCHUNK_WRITE (section 2.2.31) when oplock break happens, SMB2 Change_Notify Request (section 2.2.35) if it blocks for more than 0.5 milliseconds, SMB2 Read request (section 2.2.19) for named pipes if it blocks for more than 0.5 milliseconds, SMB2 Write request (section 2.2.21) for named pipes if it blocks for more than 0.5 milliseconds, SMB2 Write Request (section 2.2.21) for large file write, SMB2 lock request (section 2.2.26) if the SMB2_LOCKFLAG_FAIL_IMMEDIATELY flag is not set, and SMB2 FLUSH Request (section 2.2.17) for named pipes.
<364> Section 3.3.5.13: Windows SMB2 servers allow the operation when either FILE_APPEND_DATA or FILE_WRITE_DATA is set in Open.GrantedAccess.
<365> Section 3.3.5.13: Windows 7 and Windows Server 2008 R2 fail the request with STATUS_BUFFER_OVERFLOW instead of STATUS_INVALID_PARAMETER if the Length field is greater than Connection.MaxWriteSize. Windows Vista SP1 and Windows Server 2008 do not validate the Length field in SMB2 Write Request.
<366> Section 3.3.5.13: If the Flags field contains any bit values other than those specified in section 2.2.21, Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, and Windows Server 2012 fail the request with STATUS_INVALID_PARAMETER.
<367> Section 3.3.5.13: Windows writes to a file with Server Requests a Write [MS-FSA] section 2.1.5.4.
<368> Section 3.3.5.13: Windows-based servers handle the following commands asynchronously:
SMB2 CREATE Request (section 3.3.5.9) when this create would result in an oplock break.
SMB2 IOCTL Request (section 3.3.5.15) for FSCTL_PIPE_TRANSCEIVE if it blocks for more than 1 millisecond. For FSCTL_SRV_COPYCHUNK or FSCTL_SRV_COPYCHUNK_WRITE, when an oplock break happens.
SMB2 CHANGE_NOTIFY Request (section 3.3.5.19) if it blocks for more than 0.5 milliseconds.
SMB2 READ Request (section 3.3.5.12) for named pipes if it blocks for more than 0.5 milliseconds.
SMB2 WRITE Request (section 3.3.5.13) for named pipes if it blocks for more than 0.5 milliseconds.
SMB2 WRITE Request (section 3.3.5.13) for large file write.
SMB2 LOCK Request (section 3.3.5.14) if the SMB2_LOCKFLAG_FAIL_IMMEDIATELY flag is not set.
SMB2 FLUSH Request (section 3.3.5.11) for named pipes.
<369> Section 3.3.5.14: Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2 validate the open before verifying the session.
<370> Section 3.3.5.14:  Windows 7 and Windows Server 2008 R2 perform lock sequence verification only when Open.IsResilient is TRUE.
Windows 8 through Windows 10 v1909 and Windows Server 2012 through Windows Server v1909 perform lock sequence verification only when Open.IsResilient or Open.IsPersistent is TRUE.
<371> Section 3.3.5.14.1: Windows-based servers ignore this value while processing Unlocks.
<372> Section 3.3.5.14.1: Windows processes unlock with Server Requests unlock of a Byte-Range [MS-FSA] section 2.1.5.9.
<373> Section 3.3.5.14.2: Windows-based servers check for SMB2_LOCKFLAG_FAIL_IMMEDIATELY only for the first element of the Locks array.
<374> Section 3.3.5.14.2: Refer to [FSBO] for implementation-specific details of how byte range locks can be implemented.
<375> Section 3.3.5.14.2: Windows processes lock with Server Requests a Byte-Range Lock [MS-FSA] section 2.1.5.8.
<376> Section 3.3.5.15: Windows Vista SP1 and Windows Server 2008 SMB2 servers fail an IOCTL request with STATUS_INVALID_PARAMETER if [ max(InputCount, MaxInputResponse) + max(OutputCount, MaxOutputResponse) ] is greater than 262144.
<377> Section 3.3.5.15: Windows 8 and later and Windows Server 2012 and later do not fail the request.
<378> Section 3.3.5.15: Windows Vista, Windows Server 2008, Windows 7, and Windows Server 2008 R2 fail the request with STATUS_INVALID_PARAMETER in the following cases:
If OutputCount is not equal to zero and OutputOffset is greater than zero but less than (size of SMB2 header + size of the SMB2 IOCTL request not including Buffer).
If OutputCount is not equal to zero and OutputOffset is greater than size of SMB2 Message.
If OutputCount is not equal to zero and OutputOffset is not rounded up to a multiple of 8 bytes.
If (OutputOffset + OutputCount) is greater than size of SMB2 Message.
If OutputCount is greater than zero and OutputOffset is less than (InputOffset + InputCount).
Windows 7 and Windows Server 2008 R2 fail the request with STATUS_INVALID_PARAMETER if OutputOffset or OutputCount is greater than size of SMB2 Message.
<379> Section 3.3.5.15: Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 SMB2 servers copy the OutputCount bytes into the output buffer for the following FSCTLs:
FSCTL_GET_RETRIEVAL_POINTERS
FSCTL_GET_REPARSE_POINT
FSCTL_PIPE_TRANSCEIVE
FSCTL_PIPE_PEEK
FSCTL_DFS_GET_REFERRALS
Windows Vista SP1 and Windows Server 2008 SMB2 servers copy the OutputCount bytes into the output buffer for the following FSCTLs:
FSCTL_PIPE_TRANSCEIVE
FSCTL_PIPE_PEEK
FSCTL_DFS_GET_REFERRALS
All other FSCTL commands will be failed with error STATUS_BUFFER_OVERFLOW through error response specified in section 2.2.2.
<380> Section 3.3.5.15: Windows 8 and later and Windows Server 2012 and later allow only the CtlCode values, as specified in section 2.2.31, and the following CtlCode values, as specified in [MS-FSCC] section 2.3.
Windows 8.1 and later and Windows Server 2012 R2 and later allow these additional CtlCode values, as specified in [MS-RSVD].
Windows 10 and later and Windows Server 2016 and later allow the additional CtlCode value, as specified in [MS-RSVD].
Windows 10 and later and Windows Server 2016 and later allow the additional CtlCode value, as specified in [MS-FSCC].
Windows 10 v1607 operating system and later and Windows Server 2016 operating system and later allow the additional CtlCode value, as specified in [MS-FSCC].
Windows 10 v1803 operating system and later and Windows Server v1803 operating system and later allow the additional CtlCode value, as specified in [MS-FSCC].
Windows 10 and later and Windows Server 2016 and later allow the additional CtlCode value, as specified in [MS-SQOS].
Windows 11 operating system and later and Windows Server 2022 operating system and later allow the additional CtlCode value, as specified in [MS-FSCC].
Windows 11 and later and Windows Server 2022 and later allow the additional CtlCode value, as specified in [MS-FSCC].
<381> Section 3.3.5.15: For the following FSCTLs, Windows Vista SP1, Windows Server 2008, Windows 7, and Windows Server 2008 R2 return STATUS_FILE_CLOSED instead of STATUS_INVALID_DEVICE_REQUEST:
FSCTL_QUERY_NETWORK_INTERFACE_INFO
FSCTL_DFS_GET_REFERRALS_EX
FSCTL_VALIDATE_NEGOTIATE_INFO
<382> Section 3.3.5.15.1: If MaxOutputResponse is not 16 bytes, Windows-based servers do not refresh the snapshots.
<383> Section 3.3.5.15.1: Windows-based SMB2 servers will place two extra bytes set to zero in the SnapShots array and set SnapShotArraySize to two, if NumberOfSnapShots is zero.
<384> Section 3.3.5.15.2: A Windows-based DFS server does not return any data to the caller if the buffer supplied to FSCTL_GET_DFS_REFERRALS is too small.
<385> Section 3.3.5.15.3: Windows-based servers return STATUS_INVALID_DEVICE_REQUEST if the FSCTL_PIPE_TRANSCEIVE being executed is not a named pipe share.
<386> Section 3.3.5.15.3: Windows SMB2 servers send an interim response to the client if the read/write attempt is not finished in 1 millisecond.
<387> Section 3.3.5.15.3: Some Windows–based SMB2 servers return the input buffer that was received in the request as part of the response. Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 will not return the input buffer that was received in the request, and the InputCount field is always zero. Windows Vista SP1 and Windows Server 2008 will send back the input buffer based on the InputOffset and InputCount fields indicated in the request.
<388> Section 3.3.5.15.3: Windows–based SMB2 servers set OutputOffset to InputOffset + InputCount, rounded up to a multiple of 8.
<389> Section 3.3.5.15.4: Windows-based servers return STATUS_INVALID_DEVICE_REQUEST, if FSCTL_PIPE_PEEK request being executed is not a named pipe share.
<390> Section 3.3.5.15.4: Windows SMB2 servers will set OutputOffset to InputOffset + InputCount, rounded up to a multiple of 8.
<391> Section 3.3.5.15.5: Windows-based servers do not support any additional contexts.
<392> Section 3.3.5.15.5: Windows-based servers construct the 24-byte blob using Open.DurableFileId and other pieces of information which include the process ID of the caller and a timestamp.
<393> Section 3.3.5.15.6: Windows Vista SP1, Windows Server 2008, Windows 7, and Windows Server 2008 R2 do not verify byte-range locks on both source and destination files.
<394> Section 3.3.5.15.7: Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 servers support the FSCTL_SRV_READ_HASH request.
<395> Section 3.3.5.15.7: When the branch cache feature is available and the file size is less than 65,536 bytes, Windows servers fail the request with STATUS_HASH_NOT_PRESENT.
<396> Section 3.3.5.15.7:  Windows-based servers set the FileDataOffset field to the starting offset from the segment covering the Offset requested in the SRV_READ_HASH request.
<397> Section 3.3.5.15.8: The following FSCTLs are explicitly blocked by Windows-based SMB2 server and are not passed through to the object store. They are failed with STATUS_NOT_SUPPORTED.
FSCTL_REQUEST_OPLOCK_LEVEL_1 (0x00090000)
FSCTL_REQUEST_OPLOCK_LEVEL_2 (0x00090004)
FSCTL_REQUEST_BATCH_OPLOCK (0x00090008)
FSCTL_REQUEST_FILTER_OPLOCK (0x0009005C)
FSCTL_OPLOCK_BREAK_ACKNOWLEDGE (0x0009000C)
FSCTL_OPBATCH_ACK_CLOSE_PENDING (0x00090010)
FSCTL_OPLOCK_BREAK_NOTIFY (0x00090014)
FSCTL_MOVE_FILE (0x00090074)
FSCTL_QUERY_RETRIEVAL_POINTERS (0x0009003B)
FSCTL_PIPE_ASSIGN_EVENT (0x00110000)
FSCTL_GET_VOLUME_BITMAP (0x0009006F)
FSCTL_GET_NTFS_FILE_RECORD (0x00090068)
FSCTL_INVALIDATE_VOLUMES (0x00090054)
FSCTL_READ_USN_JOURNAL (0x000900BB)
FSCTL_CREATE_USN_JOURNAL (0x000900E7)
FSCTL_QUERY_USN_JOURNAL (0x000900F4)
FSCTL_DELETE_USN_JOURNAL (0x000900F8)
FSCTL_ENUM_USN_DATA (0x000900B3)
FSCTL_QUERY_DEPENDENT_VOLUME (0x000901F0)
FSCTL_SD_GLOBAL_CHANGE (0x000901F4)
FSCTL_GET_BOOT_AREA_INFO (0x00090230)
FSCTL_GET_RETRIEVAL_POINTER_BASE (0x00090234)
FSCTL_SET_PERSISTENT_VOLUME_STATE (0x00090238)
FSCTL_QUERY_PERSISTENT_VOLUME_STATE (0x0009023C)
FSCTL_REQUEST_OPLOCK (0x00090240)
FSCTL_TXFS_MODIFY_RM (0x00098144)
FSCTL_TXFS_QUERY_RM_INFORMATION (0x00094148)
FSCTL_TXFS_ROLLFORWARD_REDO (0x00098150)
FSCTL_TXFS_ROLLFORWARD_UNDO (0x00098154)
FSCTL_TXFS_START_RM (0x00098158)
FSCTL_TXFS_SHUTDOWN_RM (0x0009815C)
FSCTL_TXFS_READ_BACKUP_INFORMATION (0x00094160)
FSCTL_TXFS_WRITE_BACKUP_INFORMATION (0x00098164)
FSCTL_TXFS_CREATE_SECONDARY_RM (0x00098168)
FSCTL_TXFS_GET_METADATA_INFO (0x0009416C)
FSCTL_TXFS_GET_TRANSACTED_VERSION (0x00094170)
FSCTL_TXFS_SAVEPOINT_INFORMATION (0x00098178)
FSCTL_TXFS_CREATE_MINIVERSION (0x0009817C)
FSCTL_TXFS_TRANSACTION_ACTIVE (0x0009418C)
FSCTL_TXFS_LIST_TRANSACTIONS (0x000941E4)
FSCTL_TXFS_READ_BACKUP_INFORMATION2 (0x000901F8)
FSCTL_TXFS_WRITE_BACKUP_INFORMATION2 (0x00090200)
FSCTL_QUERY_FILE_REGIONS (0x00090284)
FSCTL_IS_CSV_FILE (0x00090248)
FSCTL_IS_FILE_ON_CSV_VOLUME (0x0009025C)
Windows 10 v1511 operating system and prior and Windows Server 2012 R2 operating system and prior block FSCTL_MARK_HANDLE (0x000900FC) and do not pass it through to the object store. The request is failed with STATUS_NOT_SUPPORTED.
Windows Vista SP1, Windows 7, Windows Server 2008, and Windows Server 2008 R2 fail FSCTLs whose transfer type is METHOD_NEITHER with error STATUS_NOT_SUPPORTED except the following ones. For more information about FSCTL transfer type, see [MSDN-IoCtlCodes].
FSCTL_PIPE_TRANSCEIVE (0x0011C017)
FSCTL_QUERY_ALLOCATED_RANGES (0x000940CF)
FSCTL_WRITE_USN_CLOSE_RECORD (0x000900EF)
FSCTL_READ_FILE_USN_DATA (0x000900EB)
FSCTL_GET_RETRIEVAL_POINTERS (0x00090073)
FSCTL_FIND_FILES_BY_SID (0x0009008F)
FSCTL_SRV_READ_HASH (0x001441BB)
<398> Section 3.3.5.15.8: Windows performs passthrough FSCTL operations via Server Requests an FsControl Request [MS-FSA] section 2.1.5.10.
<399> Section 3.3.5.15.8: Windows–based SMB2 servers will set OutputOffset to InputOffset + InputCount, rounded up to a multiple of 8.
<400> Section 3.3.5.15.9: Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 servers process the FSCTL_LMR_REQUEST_RESILIENCY request regardless of the negotiated dialect.
<401> Section 3.3.5.15.9: Windows 7 and Windows Server 2008 R2 servers keep the resilient handle open indefinitely when the requested Timeout value is equal to zero. Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 servers set a constant value of 120 seconds.
<402> Section 3.3.5.15.13: Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 require that the caller is a member of the Administrators group.
<403> Section 3.3.5.16: Windows-based servers use only the 30 least significant bits of AsyncId to look up a request in Connection.AsyncCommandList.
<404> Section 3.3.5.16: When being handled by an object store, Windows performs cancellation of in-progress requests via the interface in [MS-FSA] section 2.1.5.20, Server Requests Canceling an Operation, passing Request.CancelRequestId as an input parameter. Windows does not attempt to cancel other in-progress requests.
<405> Section 3.3.5.17: Windows Vista SP1, Windows 7, Windows Server 2008, and Windows Server 2008 R2 servers do not disconnect the connection.
<406> Section 3.3.5.18: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 fail the request with STATUS_NOT_SUPPORTED.
<407> Section 3.3.5.18:  Windows-based SMB2 servers fail the request with STATUS_INVALID_PARAMETER if OutputBufferLength is greater than 65536.
<408> Section 3.3.5.18: Windows Vista SP1, Windows Server 2008, Windows 7, and Windows Server 2008 R2 close and reopen the directory handle prior to processing the request.
<409> Section 3.3.5.18: Windows-based servers perform query directory requests, as specified in [MS-FSA] section 2.1.5.6 with the following input parameters:
Open is set to Open.LocalOpen.
FileInformationClass is set to the InformationClass that is received in the SMB2 QUERY_DIRECTORY Request.
OutputBufferSize is set to the OutputBufferLength that is received in the SMB2 QUERY_DIRECTORY Request.
If SMB2_RESTART_SCANS or SMB2_REOPEN is set in the Flags field of the SMB2 QUERY_DIRECTORY Request, RestartScan is set to TRUE.
If SMB2_RETURN_SINGLE_ENTRY is set in the Flags field of the request, ReturnSingleEntry is set to TRUE.
FileIndex is set to 0.
FileNamePattern is set to the search pattern specified in the SMB2 QUERY_DIRECTORY by FileNameOffset and FileNameLength.
When SMB2_REOPEN is set in the Flags field of SMB2 QUERY_DIRECTORY request and the object store does not return any files, Windows 10 v1803 through Windows 11, and Windows Server 2019 fail the request with STATUS_NO_MORE_FILES.
When SMB2_REOPEN is set in the Flags field of SMB2 QUERY_DIRECTORY request and the object store does not return any files, Windows 11 v22H2 and Windows 11, version 23H2 with [MSKB-5062663], Windows 11, version 24H2 with [MSKB-5062660] and later, Windows Server 2022 with [MSKB-5063880] and Windows Server 2022, 23H2 with [MSKB-5063899] and Windows Server 2025 with [MSKB-5062660] and later fail the request with STATUS_NO_SUCH_FILE.
<410> Section 3.3.5.18:  Windows-based servers ignore SMB2_INDEX_SPECIFIED in Flags field and FileIndex value.
<411> Section 3.3.5.19: Windows-based SMB2 servers fail the request with STATUS_INVALID_PARAMETER if OutputBufferLength is greater than 65536.
<412> Section 3.3.5.19: Windows-based servers handle the following commands asynchronously: SMB2 Create (section 2.2.13) when this create would result in an oplock break, SMB2 IOCTL Request (section 2.2.31) for FSCTL_PIPE_TRANSCEIVE if it blocks for more than 1 millisecond, SMB2 IOCTL Request for FSCTL_SRV_COPYCHUNK or FSCTL_SRV_COPYCHUNK_WRITE (section 2.2.31) when oplock break happens, SMB2 Change_Notify Request (section 2.2.35) if it blocks for more than 0.5 milliseconds, SMB2 Read Request (section 2.2.19) for named pipes if it blocks for more than 0.5 milliseconds, SMB2 Write Request (section 2.2.21) for named pipes if it blocks for more than 0.5 milliseconds, SMB2 Write Request (section 2.2.21) for large file write, SMB2 lock Request (section 2.2.26) if the SMB2_LOCKFLAG_FAIL_IMMEDIATELY flag is not set, and SMB2 FLUSH Request (section 2.2.17) for named pipes.
<413> Section 3.3.5.19: Windows requests ChangeNotify processing via Server Requests Change Notifications for a Directory in [MS-FSA] section 2.1.5.11. If the SMB2_WATCH_TREE flag is set, the WatchTree boolean is passed as TRUE. ChangeNotify notification is reported as described in [MS-FSA] section 2.1.5.11.1.
<414> Section 3.3.5.20: Windows-based SMB2 servers fail the request with STATUS_INVALID_PARAMETER if OutputBufferLength is greater than 65536.
<415> Section 3.3.5.20.1: Windows-based SMB2 servers fail the following request levels with STATUS_INVALID_INFO_CLASS instead of STATUS_NOT_SUPPORTED: 1, 2, 3, 10, 11, 12, 13, 19, 20, 27, 31, 36, 37, 38, 39, 40, 50.
<416> Section 3.3.5.20.1: Windows-based SMB2 servers fail the following request levels with STATUS_NOT_SUPPORTED instead of STATUS_INVALID_INFO_CLASS: 41, 43, 47, 49, 51, and 53. Windows-based SMB2 servers fail requests of level 52 with STATUS_INFO_LENGTH_MISMATCH.
<417> Section 3.3.5.20.1:  Windows 10 v1709, Windows Server v1709 and prior do not support the FileNormalizedNameInformation information class.
<418> Section 3.3.5.20.1: Windows-based SMB2 servers will set CurrentByteOffset to any value.
<419> Section 3.3.5.20.1: Windows performs SMB2 GET_INFO SMB2_0_INFO_FILE processing as specified in the subsection of [MS-FSA] section 2.1.5.12, corresponding to the requested FILE_INFORMATION_CLASS value of the FileInfoClass request field, as listed in section 2.2.37.
<420> Section 3.3.5.20.1: If the information class is FileAllInformation, Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 return an absolute path to the file name as part of FileNameInformation.
<421> Section 3.3.5.20.2: Windows performs SMB2 GET_INFO SMB2_0_INFO_FILESYSTEM processing via the subsection of [MS-FSA] section 2.1.5.13 corresponding to the requested FS_INFORMATION_CLASS value of the FileInfoClass request field, as listed in section 2.2.37.
<422> Section 3.3.5.20.2:  Windows 7 through Windows 11 and Windows Server 2008 R2 operating system through Windows Server 2022 SMB2 servers do not clear the bits FILE_RETURNS_CLEANUP_RESULT_INFO, FILE_SUPPORTS_POSIX_UNLINK_RENAME before sending to the client.
<423> Section 3.3.5.20.2: SetFsInfo calls to Windows-based servers fail with STATUS_ACCESS_DENIED because Windows-based servers do not allow setting volume information over the network.
<424> Section 3.3.5.20.3: Windows performs SMB2 GET_INFO SMB2_0_INFO_SECURITY processing via Server Requests a Query of Security Information ([MS-FSA] section 2.1.5.14).
<425> Section 3.3.5.20.4: Windows-based servers do support quotas, if configured.
<426> Section 3.3.5.20.4: Windows performs SMB2 GET_INFO SMB2_0_INFO_QUOTA processing via Server Requests a Query of Quota Information ([MS-FSA] section 2.1.5.21).
<427> Section 3.3.5.21: Windows-based SMB2 servers fail the request with STATUS_INVALID_PARAMETER if BufferLength is greater than 65536.
<428> Section 3.3.5.21.1: Windows-based SMB2 servers fail the following request levels with STATUS_NOT_SUPPORTED instead of STATUS_INVALID_INFO_CLASS: 30, 41, 42, 43.
<429> Section 3.3.5.21.1: Windows performs SMB2 SET_INFO SMB2_0_INFO_FILE processing via the subsection of [MS-FSA] section 2.1.5.15 corresponding to the requested FILE_INFORMATION_CLASS value of the FileInfoClass request field, as listed in section 2.2.37.
<430> Section 3.3.5.21.2: Windows performs SMB2 SET_INFO SMB2_0_INFO_FILESYSTEM processing via the subsection of [MS-FSA] section 2.1.5.16 corresponding to the requested FS_INFORMATION_CLASS value of the FileInfoClass request field, as listed in section 2.2.37.
<431> Section 3.3.5.21.3: If the underlying object store does not support object security based on Access Control Lists (as specified in [MS-DTYP] section 2.4.5), it returns STATUS_SUCCESS.
<432> Section 3.3.5.21.3: Windows Server 2008, Windows 7 and Windows Server 2008 R2 ignore the ATTRIBUTE_SECURITY_INFORMATION flag value.
<433> Section 3.3.5.21.3:  Windows Server 2008, Windows 7 and Windows Server 2008 R2 ignore the SCOPE_SECURITY_INFORMATION flag value.
<434> Section 3.3.5.21.3:  Windows Server 2008, Windows 7 and Windows Server 2008 R2 ignore the BACKUP_SECURITY_INFORMATION flag value.
<435> Section 3.3.5.21.3: Windows performs SMB2 SET_INFO SMB2_0_INFO_SECURITY processing via Server Requests Setting of Security Information [MS-FSA] section 2.1.5.17.
<436> Section 3.3.5.21.4: Windows-based servers do support quotas, if configured.
<437> Section 3.3.5.21.4: Windows performs SMB2 SET_INFO SMB2_0_INFO_QUOTA processing via Server Requests Setting of Quota Information ([MS-FSA] section 2.1.5.22).
<438> Section 3.3.5.22.1: Windows-based servers complete the oplock break indication request with the object store by providing the following SMB2 parameters as input parameters, as specified [MS-FSA] section 2.1.5.19:
<439> Section 3.3.5.22.1: Windows-based servers complete the oplock break indication request with the object store by providing the following SMB2 parameters as input parameters, as specified [MS-FSA] section 2.1.5.19:
<440> Section 3.3.5.22.1: Windows-based servers complete the oplock break indication request with the object store by providing the following SMB2 parameters as input parameters, as specified [MS-FSA] section 2.1.5.19:
<441> Section 3.3.5.22.1: If multiple conflicting Opens occur before an Oplock Acknowledgment for the first oplock break is received, that change the server oplock state to a level that is lower than the pending notification, the server fails the Oplock Acknowledgment with STATUS_REQUEST_NOT_ACCEPTED. Windows-based servers complete the oplock break indication request with the object store by providing the following SMB2 parameters as input parameters, as specified in [MS-FSA] section 2.1.5.19:
<442> Section 3.3.6.3: Windows-based servers use a constant time-out value of 45 seconds.
<443> Section 3.3.7.1: Windows performs cancellation of in-progress requests via the interface in [MS-FSA] section 2.1.5.20, Server Requests Canceling an Operation, passing Request.CancelRequestId as an input parameter.
<444> Section 3.3.7.1: Windows 7, Windows Server 2008 R2, Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 servers will not reset ResilientOpenScavengerExpiryTime.
<445> Section 3.3.7.1: Windows performs cancellation of in-progress requests via the interface in [MS-FSA] section 2.1.5.20, Server Requests Canceling an Operation, passing Request.CancelRequestId as an input parameter.
```
