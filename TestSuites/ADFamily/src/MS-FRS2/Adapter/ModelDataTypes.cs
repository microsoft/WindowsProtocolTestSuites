// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using System.Text;


namespace FRS2Model
{
    /// <summary>
    /// Enums for model error codes.
    /// </summary>
    public enum error_status_t
    {
        /// <summary>
        ///  indicates Success condition for an opnum.
        /// </summary>
        ERROR_SUCCESS = 0,
        /// <summary>
        ///  implementation specific error code.
        /// </summary>
        ERROR_FAIL = 1,
        /// <summary>
        /// The client does not have rights to access the file.
        /// </summary>       
        ERROR_ACCESS_DENIED = 0x00000005,
        /// <summary>
        /// The file is already in use by another client.
        /// </summary>
        ERROR_SHARING_VIOLATION = 0x00000020,
        /// <summary>
        /// The client provided an invalid parameter.
        /// </summary>
        ERROR_INVALID_PARAMETER = 0x00000057,
        /// <summary>
        /// The server is too busy to service the request.
        /// </summary>
        ERROR_BUSY = 0x000000AA,
        /// <summary>
        /// The content set was not recognized.
        /// </summary>
        FRS_ERROR_CONTENTSET_GUID_NOT_FOUND = 0x00002329,
        /// <summary>
        /// The file metadata is not synchronized with the file system.
        /// </summary>
        FRS_ERROR_FILE_INFORMATION_IS_STALE = 0x00002340,
        /// <summary>
        /// The connection is invalid.
        /// </summary>
        FRS_ERROR_CONNECTION_INVALID = 0x00002342,
        /// <summary>
        /// The content set was not found.
        /// </summary>
        FRS_ERROR_CONTENTSET_NOT_FOUND = 0x00002344,
        /// <summary>
        /// The file has changed.
        /// </summary>
        FRS_ERROR_FILE_HAS_CHANGED = 0x00002345,
        /// <summary>
        /// The connection is shutting down.
        /// </summary>
        FRS_ERROR_CONNECTION_SHUTDOWN = 0x00002348,
        /// <summary>
        /// Unknown error in RDC.
        /// </summary>
        FRS_ERROR_RDC_GENERIC = 0x0000234B,
        /// <summary>
        /// Paused for backup or restore.
        /// </summary>
        FRS_ERROR_IN_BACKUP_RESTORE = 0x0000234C,
        /// <summary>
        /// RDC is not used.
        /// </summary>
        FRS_ERROR_NO_RDC = 0x00002356,
        /// <summary>
        /// The compressed data is invalid.
        /// </summary>
        FRS_ERROR_XPRESS_INVALID_DATA = 0x00002358,
        /// <summary>
        /// The client's protocol version is incompatible with the server's.
        /// </summary>
        FRS_ERROR_INCOMPATIBLE_VERSION = 0x0000235A,
        /// <summary>
        /// The content set is not ready.
        /// </summary>
        FRS_ERROR_CONTENTSET_NOT_READY = 0x0000235B,
        /// <summary>
        /// The member is invalid.
        /// </summary>
        FRS_ERROR_MEMBER_INVALID = 0x00002360,
        /// <summary>
        /// The update does not exist on the upstream server.
        /// </summary>
        FRS_ERROR_UPDATE_NOT_EXISTS = 0x00002363,
        /// <summary>
        /// The server does not possess the full file contents for the requested file.
        /// </summary>
        FRS_ERROR_RESOURCE_IS_GHOSTED = 0x00002457,
        /// <summary>
        /// The server is not currently participating in the replication of the specified replicated folder
        /// </summary>
        FRS_FRS_ERROR_CSMAN_OFFLINE,
        /// <summary>
        /// The client MUST remain in the Polling connection state, but SHOULD not call EstablishSession 
        /// again for this replicated folder.
        /// </summary>
        FRS_ERROR_CONTENTSET_READ_ONLY
    }

    /// <summary>
    /// Operating System Version of the SUT.
    /// </summary>
    public enum OSVersion
    {
        Windows2008,
        Windows7,
        NonWindows
    }



    /// <summary>
    /// Opnum EstablishConnection has downstreamProtocolVersion as one of the input parameters which can take values
    /// 0x00050000, 0x00050001, or 0x00050002 such that error code FRS_ERROR_INCOMPATIBLE_VERSION(0x0000235A)is thrown
    /// if the 16bits version check fails as per the TD. Hence the version values have been abstracted under enum ProtocolVersion 
    /// with two parameters 'valid' and 'inValid'.
    /// </summary>
    public enum ProtocolVersion
    {
        /// <summary>
        /// value 0x00050000 is considered as valid value.
        /// </summary>
        FRS_COMMUNICATION_PROTOCOL_VERSION_W2K3R2 = 0x00050000,

        FRS_COMMUNICATION_PROTOCOL_VERSION_VISTA = 0x00050001,

        /// <summary>
        /// value 0x00050002 is considered as valid value.
        /// </summary>
        FRS_COMMUNICATION_PROTOCOL_VERSION_LONGHORN_SERVER = 0x00050002,

        /// <summary>
        /// value 0x00050003 is considered as valid value.
        /// </summary>
        FRS_COMMUNICATION_PROTOCOL_VERSION_WIN8_SERVER = 0x00050003,

        /// <summary>
        /// value 0x00050004 is considered as valid value.
        /// </summary>
        FRS_COMMUNICATION_PROTOCOL_VERSION_WIN2012R2_SERVER = 0x00050004,
        /// <summary>
        /// some other value than 0x00050000, 0x00050001, or 0x00050002.
        /// </summary>
        inValid = 0xfffff
    }

    /// <summary>
    /// One of the input parameters of Opnums InitializeFileTransferAsync and InitializeFileDataTransfer is complex structure FRS_UPDATE which is 
    /// obtained as the out parameter of Opnum RequestUpdates when the version request type specified by the client is NORMAL_SYNC or SLAVE_SYNC
    /// and has got parameters like nameConflict,attributes,present,uidVersion,gvsnVersion etc.that contains information about the file being replicated 
    /// such that error code FRS_ERROR_FILE_HAS_CHANGED(0x00002345) is thrown if the present field is set  to 0 that implies that the
    /// file to be replicated on the server has been deleted. Hence the structure FRS_UPDATE has been abstracted by enum present having 
    /// 'fileExists' 'fileDeleted' as its two parameters to denote whether the file to be replicated exists on the Server or not.
    /// </summary>
    public enum FilePresense
    {
        /// <summary>
        ///  present field is set  to 1 that implies that the file to be replicated exists on the Server
        /// </summary>
        fileExists = 1,
        /// <summary>
        /// present field is set  to 0 that implies that the file to be replicated on the Server has been deleted
        /// </summary>
        fileDeleted = 0
    }

    /// <summary>
    /// stagingPolicy parameter in Opnum InitializeFileTransferAsync can take the values SERVER_DEFAULT,STAGING_REQUIRED or RESTAGING_REQUIRED as 
    /// the input and output parameters belonging to structure FRS_REQUESTED_STAGING_POLICY which indicates 
    /// the type of staging requested by the Client. Hence the parameter has been abstracted out with the help of enum FRS_REQUESTED_STAGING_POLICY having its data members 
    /// as specified in the TD.
    /// </summary>
    public enum FRS_REQUESTED_STAGING_POLICY
    {
        /// <summary>
        /// The client indicates to the server that the server is free to use or bypass its cache.
        /// </summary>
        SERVER_DEFAULT = 1,
        /// <summary>
        /// The client indicates to the server to store the served content in its cache.
        /// </summary>
        STAGING_REQUIRED = 2,
        /// <summary>
        /// client indicates to the server to purge existing content from its cache.
        /// </summary>
        RESTAGING_REQUIRED = 3
    }

    /// <summary>
    /// cancelData specifies the identifier for a corresponding requested Update which is no longer required by the Client and need not be sent by the Server. 
    /// In-parameter for UpdateCancel opnum is cancelData belonging to structure FRS_UPDATE_CANCEL_DATA and the Client uses this opnum to indicate the server to
    /// cancel the sending of updates for a replication folder which it had requested previously using RequestUpdates opnum.Hence 'inValid' value in
    /// FRS_UPDATE_CANCEL_DATA enum indicates that the identifier of the update is invalid while 'unSpecified' denotes a valid value.Moreover
    /// cancelType field in FRS_UPDATE_CANCEL_DATA structure can take only one value(Unspecified).Hence that value is considered as the valid value.
    /// </summary>
    public enum FRS_UPDATE_CANCEL_DATA
    {
        /// <summary>
        /// identifier of the FRS_UPDATE_CANCEL_DATA is valid.
        /// </summary>
        valid,
        /// <summary>
        /// identifier of the FRS_UPDATE_CANCEL_DATA is inValid.
        /// </summary>
        inValid
    }

    /// <summary>
    /// One of the output parameters of Opnums InitializeFileTransferAsync and InitializeFileDataTransfer has structurePFRS_SERVER_CONTEXT that 
    /// signifies the context handle that represents the requested file replication operation. This context handle serves as input parameters to Opnums 
    /// like RawGetFileData,RdcGetSignatures,RdcPushSourceNeeds,RdcClose etc. which checks for its validity for their return status to be ERROR_SUCCESS.
    /// Hence the structure has been abstracted by enum ServerContext having 'valid' and 'inValid' as its two parameters to denote the behavioral aspect 
    /// of the Opnums and the protocol.
    /// </summary>
    public enum ServerContext
    {
        /// <summary>
        /// Context handle that represents the requested file replication operation is valid.
        /// </summary>
        ValidContext,
        /// <summary>
        /// Context handle that represents the requested file replication operation is invalid.
        /// </summary>
        InvalidContext
    }

    /// <summary>
    /// One of the input parameters of Opnum RdcGetSignatures is level which denotes the recursion level such that 
    /// a client MUST specify a number in the range 1 to CONFIG_RDC_MAX_LEVELS(8).If the value of level is set to 0
    /// server MUST return ERROR_INVALID_PARAMETER.Hence the input parameter is abstracted by enum Level having 'noValue'
    /// and 'has_Value' as its two parameters.
    /// </summary>
    public enum Level
    {
        /// <summary>
        /// indicates the valid depth of signature generation and RDC recursion.
        /// </summary>
        validValue,
        /// <summary>
        /// indicates the invalid depth of signature generation and RDC recursion.
        /// </summary>
        inValidValue
    }

    /// <summary>
    /// One of the output parameters of Opnum InitializeFileTransferAsync is complex structure RS_RDC_FILEINFO which has 
    /// rdcSignatureLevels as one of its parameters such that when the server sets its value to 1(or TRUE)the client proceeds 
    /// by downloading file content over RDC protocol and if it set to 0(or FALSE)the file content is downloaded normally(using
    /// RawGetFileData and RawGetFileDataAsync Opnums).Hence the structure is abstracted with the enum RDC_Sig_Level with its
    /// corresponding parameters to denote the behavior of the Opnum and the Protocol.
    /// </summary>
    public enum RDC_Sig_Level
    {
        /// <summary>
        /// Client requests Raw(Normal)File transfer
        /// </summary>
        forRAWFileLevel,
        /// <summary>
        /// Client requests Rdc(Compressed)File transfer
        /// </summary>
        forRDCFileLevel
    }

    /// <summary>
    /// One of the input parameters of Opnum RequestUpdates is complex structure UPDATE_REQUEST_TYPE that indicates the type 
    /// of replication updates requested. Depending on the values(or status)of this input parameter the server sets its corresponding
    /// output parameter and also decides whether the opnum should be called repetitively or not. Hence to demonstrate this 
    /// behavioral aspect of the Opnum and the protocol it has been abstracted out with the enum UPDATE_REQUEST_TYPE.
    /// </summary>
    public enum UPDATE_REQUEST_TYPE
    {
        /// <summary>
        /// indicates the UPDATE_REQUEST_TYPE is valid
        /// </summary>
        UPDATE_TYPE_VALID,
        /// <summary>
        /// indicates the UPDATE_REQUEST_TYPE is invalid
        /// </summary>
        UPDATE_TYPE_INVALID
    }
    /// <summary>
    /// One of the Output parameters of Opnum RequestUpdates is complex structure UPDATE_STATUS whose content is set by the server
    /// depending on the value of the input parameter UPDATE_REQUEST_TYPE of RequestUpdates opnum.Hence to explore this behavioral
    /// aspect of the Protocol the out parameter structure has been abstracted out with the enum UPDATE_STATUS.
    /// </summary>
    public enum UPDATE_STATUS
    {
        /// <summary>
        /// There are no more updates that pertain to the argument version chain vector. 
        /// In other words, the server does not have any updates whose versions belong to the version chain vector passed in by the client.
        /// </summary>
        UPDATE_STATUS_DONE,
        /// <summary>
        /// There are potentially more updates 
        /// (tombstone, if the client requested tombstones; live, if the client requested live) from the argument version chain vector.
        /// </summary>
        UPDATE_STATUS_MORE,
    }

    /// <summary>
    /// One of the in-parameters for RequestVersionVector opnum is requestType belonging to VERSION_REQUEST_TYPE by means of which the
    /// client specifies whether the replication session should proceed by calling RequestUpdates or RequestRecords.Hence it has been 
    /// abstracted with the help of enum VERSION_REQUEST_TYPE having its corresponding parameters as specified in the TD.
    /// </summary>
    public enum VERSION_REQUEST_TYPE
    {
        /// <summary>
        ///  Indicates that the client requests a version vector from the server for standard synchronization.
        /// </summary>
        REQUEST_NORMAL_SYNC = 0,
        /// <summary>
        ///  Indicates that the client requests a version vector from the server for Slow Sync.
        /// </summary>
        REQUEST_SLOW_SYNC = 1,
        /// <summary>
        ///  Indicates that the client requests a version vector from the server for selective single master mode.
        /// </summary>
        REQUEST_SLAVE_SYNC = 2//(TDI# 23739)
    }

    /// <summary>
    /// One of the in-parameters for RequestVersionVector opnum is changeType belonging to VERSION_CHANGE_TYPE by which the Client
    /// indicates whether to notify change only, change delta, or send the entire version chain vector. Hence it has been abstracted with
    /// the enum VERSION_CHANGE_TYPE to validate the associated requirements.
    /// </summary>
    public enum VERSION_CHANGE_TYPE
    {
        /// <summary>
        /// The client requests notification only for a change of the server's version chain vector.
        /// </summary>
        CHANGE_NOTIFY = 0,
        /// <summary>
        /// The client requests to receive the full version vector of the server.
        /// </summary>
        CHANGE_ALL = 2,
        /// <summary>
        /// version change type other than CHANGE_NOTIFY,CHANGE_ALL .
        /// </summary>
        other

    }

    /// <summary>
    /// One of the out-parameters for Request Records is RecordsStatus to specify whether all the file records can been send by making 
    /// a single call to RequestRecords or it needs to be called multiple times to retrieve the entire information regarding the requested
    /// replication folder. Hence it has been abstracted with the enum recordsStatus having its corresponding values as specified
    /// in the TD.
    /// </summary>
    public enum RecordsStatus
    {
        /// <summary>
        ///  No more records are waiting to be transmitted on the server.
        /// </summary>
        RECORDS_STATUS_DONE = 1,
        /// <summary>
        ///  More records are waiting to be transmitted on the server.
        /// </summary>
        RECORDS_STATUS_MORE = 2
    }


    /// <summary>
    /// One of the in-parameters for InitializeFileTransferAsync is nameConflict such that the value is set to 0 if the file exists on the Server
    /// and set to 1 if the file has been deleted. Hence it has been abstracted with the help of Enum nameConflict to depict this behavior
    /// of the TD.
    /// </summary>
    public enum NameConflict
    {
        /// <summary>
        /// conflict field is set  to 0 that implies that the file to be replicated exists on the Server
        /// </summary>
        nameConflictExists = 0,
        /// <summary>
        /// conflict field is set  to 1 that implies that the file to be replicated on the Server has been deleted
        /// </summary>
        nameConflictDeleted = 1
    }

    /// <summary>
    /// One of the In-parameters for RequestVersionVector is SequenceNumber and it comes as an out-param of AyncPollResponse event such that
    /// the SequenceNumber is used to pair the response with the corresponding AsyncPoll request. Hence it is abstracted with the enum SequenceNumber
    /// </summary>
    public enum SequenceNumber
    {
        /// <summary>
        /// indicates the valid sequence number
        /// </summary>
        validSequenceNum,
        /// <summary>
        /// indicates the invalid sequence number
        /// </summary>
        inValidSequenceNum
    }

    /// <summary>
    /// One of the In-parameters for RequestVersionVector is VVGeneration which can have the last generation number if requestType is 
    /// REQUEST_NORMAL_SYNC and zero if requestType is REQUEST_SLOW_SYNC or REQUEST_SLAVE_SYNC. HFence it has been abstracted with 
    /// 'value'and 'noValue'parameters.
    /// </summary>
    public enum VVGeneration
    {
        /// <summary>
        /// indicates the valid VVGeneration
        /// </summary>
        ValidValue,
        /// <summary>
        /// indicates the invalid VVGeneration
        /// </summary>
        InvalidValue
    }

    /// <summary>
    /// the abstraction of size of the file data returned in Buffer.
    /// </summary>
    public enum SizeReturned
    {
        /// <summary>
        /// Buffer is empty.
        /// </summary>
        zeroValue = 0,
        /// <summary>
        /// the Buffer is not empty.
        /// </summary>
        nonZeroValue = 1
    }
    /// <summary>
    /// The size, in bytes, of dataBuffer.
    /// </summary>
    public enum BufferSize
    {
        /// <summary>
        /// Represents  valid Buffer size.
        /// </summary>
        validBufSize,
        /// <summary>
        /// Invalid Buffer size.
        /// </summary>
        inValidBufSize
    }

    /// <summary>
    /// The size, in bytes, of the file data returned in dataBuffer.
    /// </summary>
    public enum SizeRead
    {
        /// <summary>
        /// zero based sizeread
        /// </summary>
        zeroValue = 0,
        /// <summary>
        /// nonzero based sizeread
        /// </summary>
        nonZeroValue = 1
    }


    /// <summary>
    /// Folder Access levels.
    /// </summary>
    public enum accessLevels
    {
        /// <summary>
        /// Folder with read only access
        /// </summary>
        readOnly,
        /// <summary>
        /// Folder with Writeonly access
        /// </summary>
        writeOnly,
        /// <summary>
        /// folder with both read and write access.
        /// </summary>
        readWrite,
        /// <summary>
        ///    Used for the purpose of initializing local variables
        /// Have no bearing on model 
        /// </summary>
        notSet
    }
    /// <summary>
    /// Property level for replication group whether enabled or disabled
    /// </summary>
    public enum connectionProperty
    {
        /// <summary>
        /// Property level for replication group with enabled
        /// </summary>
        enabled,
        /// <summary>
        /// Property level for replication group with disabled
        /// </summary>
        disabled,

        /// <summary>
        /// Used for the purpose of initializing local variables
        /// Have no bearing on model
        /// </summary>
        notSet
    }
    /// <summary>
    /// Offset 
    /// </summary>
    public enum offset
    {
        /// <summary>
        /// Indicates Valid Offset
        /// </summary>
        valid,
        /// <summary>
        /// Indicates Invalid offset
        /// </summary>
        inValid
    }
    /// <summary>
    /// ReplicationGroupTypes whether it is sysvol, protection, distribution or other.
    /// </summary>
    public enum ReplicationGroupTypes
    {
        /// <summary>
        /// sysvol ReplicationGroupType
        /// </summary>
        sysvol,
        /// <summary>
        /// protection ReplicationGroupTypes
        /// </summary>
        protection,
        /// <summary>
        /// distribution ReplicationGroupTypes
        /// </summary>
        distribution,
        /// <summary>
        /// ReplicationGroupTypes other than above
        /// </summary>
        other,
        /// <summary>
        ///   Used for the purpose of initializing local variables
        /// Have no bearing on model
        /// </summary>
        notSet
    }
    /// <summary>
    /// Abstraction for version vector structure.
    /// </summary>
    public enum versionVectorDiff
    {
        /// <summary>
        /// Valid version vector diff
        /// </summary>
        valid,
        /// <summary>
        /// Invalid version vector diff
        /// </summary>
        inValid
    }
    /// <summary>
    /// DN of the inbound partner whether it exists or not exists
    /// </summary>
    public enum FromServer
    {
        /// <summary>
        /// DN of the inbound partner is exists
        /// </summary>
        exists,
        /// <summary>
        /// DN of the inbound partner is not exists
        /// </summary>
        notExists,

        /// <summary>
        /// Used for the purpose of initializing local variables
        ///  Have no bearing on model
        /// </summary>
        notSet
    }
    /// <summary>
    /// Property level for replication group whether enabled or disabled
    /// </summary>
    public enum subscriptProperty
    {
        /// <summary>
        /// Property level for replication group is enabled
        /// </summary>
        enabled,
        /// <summary>
        /// Property level for replication group is disabled
        /// </summary>
        disabled,
        /// <summary>
        ///  Used for the purpose of initializing local variables
        /// Have no bearing on model 
        /// </summary>
        notSet
    }


    /// <summary>
    /// returned protocol version of Establish connection
    /// </summary>  
    public enum ProtocolVersionReturned
    {
        /// <summary>
        /// Valid protocol version
        /// </summary>
        Valid,
        /// <summary>
        /// Invalid protocol version
        /// </summary>
        Invalid
    }

    /// <summary>
    /// Receives the server's protocol version number. Expected values are the same as for 
    /// downstreamProtocolVersion
    /// </summary>
    public enum UpstreamFlagValueReturned
    {
        /// <summary>
        /// valid UpstreamFlagValueReturned
        /// </summary>
        Valid,
        /// <summary>
        /// Invalid UpstreamFlagValueReturned
        /// </summary>
        Invalid
    }
    /// <summary>
    /// state to represent the state of RDCGetFiledata 
    /// </summary>
    public enum State
    {
        /// <summary>
        /// The state is set
        /// </summary>
        Set,
        /// <summary>
        ///  Used for the purpose of initializing local variables Have no bearing on model 
        /// </summary>
        NotSet
    }
}
