// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.Messages;
using Microsoft.Protocols.TestTools.Messages.Marshaling;
using Microsoft.Protocols.TestSuites.Frs2DataTypes;
using FRS2Model;
using FileStreamDataParser;

namespace Microsoft.Protocols.TestSuites.MS_FRS2
{
    /// <summary>
    /// This delegate is used to handle AsyncPoll Request.
    /// </summary>
    public delegate void AsyncPollHandler(VVGeneration vvGen);
    /// <summary>
    /// This delegate is used to handle InitializeFileTransferAsync Request.
    /// </summary>
    /// <param name="context">The context handle that represents the requested file replication operation.</param>
    /// <param name="rdcsigLevel">The signature level that describes the file whose replication is in progress.</param>
    /// <param name="isEOF">The value is TRUE if the end of the specified file has been reached and there is no more file data to replicate to the client; otherwise, the value is FALSE.</param> 
    public delegate void InitializeFileTransferAsyncHandler(ServerContext context,
                                                            RDC_Sig_Level rdcsigLevel,
                                                            bool isEOF);
    /// <summary>
    /// This delegate is used to handle RequestUpdates Request.
    /// </summary>
    /// <param name="present">Field within the FRS_UPDATE structure to indicate whether the file exists on the server or has been deleted.</param>
    /// <param name="updateStatus">The value from the UPDATE_STATUS enumeration that describes the status of the requested update.</param>
    public delegate void RequestUpdatesHandler(FilePresense present,
                                           UPDATE_STATUS updateStatus);

    /// <summary>
    /// This delegate is used to handle RawGetFileData Request.
    /// </summary>
    /// <param name="isEOF">The value is TRUE if the end of the specified file has been reached and there is no more file data to replicate to the client; otherwise, the value is FALSE.</param>
    public delegate void RawGetFileDataResponseHandler(bool isEOF);

    /// <summary>
    /// This delegate is used to handle RdcGetFileData Request.
    /// </summary>
    /// <param name="sizeReturned">The size of the data returned in dataBuffer.</param>
    public delegate void RdcGetFileDataHandler(SizeReturned sizeReturned);

    /// <summary>
    /// This delegate is used to handle RequestRecords Request.
    /// </summary>
    /// <param name="status"> The value from the RECORDS_STATUS enumeration that indicates whether more update records are available.</param>
    public delegate void RequestRecordsHandler(RecordsStatus status);

    /// <summary>
    /// IFRS2Managed Adapter Interface. inheirts IAdapter and declares all the methods that are implementation specific
    /// </summary>
    public interface IFRS2ManagedAdapter : IAdapter
    {
        /// <summary>
        /// AsyncPollHandler event corresponds to AsyncPoll Response
        /// </summary>
        event AsyncPollHandler AsyncPollResponseEvent;

        /// <summary>
        /// InitializeFileTransferAsyncHandler event corresponds to InitializeFileTransferAsync Response
        /// </summary>
        event InitializeFileTransferAsyncHandler InitializeFileTransferAsyncEvent;

        /// <summary>
        /// RequestUpdatesHandler event corresponds to RequestUpdates Response
        /// </summary>
        event RequestUpdatesHandler RequestUpdatesEvent;

        /// <summary>
        /// RawGetFileDataHandler event corresponds to RawGetFileData Response
        /// </summary>
        event RawGetFileDataResponseHandler RawGetFileDataResponseEvent;

        /// <summary>
        /// RdcGetFileDataHandler event corresponds to RawGetFileData Response
        /// </summary>
        event RdcGetFileDataHandler RdcGetFileDataEvent;


        /// <summary>
        /// RequestRecordsHandler event corresponds to RequestRecords Response
        /// </summary>
        event RequestRecordsHandler RequestRecordsEvent;


        /// <summary>
        /// Initialization is the pre-requisite for all the scenarios. In order to invoke a protocol call, Initialization must
        /// be called. During the course of Test Suite execution, Initialization must be called only once. Also for this Test Suite,
        /// the accepting state condition enforced is Initialization must be called.
        ///
        /// In order to execute the TS, some configurations/test-objects should be present on the server. Details documented in 
        /// Test Deployement Environment Guide.
        ///
        /// For the model, an equivalent metadata representation is needed. Initialization populates the Maps, which hold the metadata
        /// that are needed for behavior analysis, test case generation and requirements capturing.
        ///
        ///
        /// Initialization populates the following maps.
        /// (1).
        /// Map<int, connectionProperty> serverConn --> Some connections should be established in the server. Stores the connectionIDs 
        ///                                             and whether the corresponding connection is enabled or disabled. 
        ///                                             Key --> connectionID: Value --> connectionProperty.
        ///                                             
        /// (2).
        /// Map<int, FromServer> fromServConn --> Stores the connectionIDs and status of FromServer (TD Section 2.3.11 DN of the  
        ///                                       inbound partner(other msDFSR-Member object). MUST exist)
        ///                                       Key --> connectionID: Value --> status of FromServer
        /// (3).
        /// Map<string,int> replGrpConn --> Some replication groups must be created on the server with respect to the connections.
        ///                                 Key --> Replication Group name (used as string identifiers in Model, adpater maps actual 
        ///                                 replication group name from ptfconfig): Value --> connectionID
        /// (4).
        /// Map<string,ReplicationGroupTypes> replGrpType --> Stores Replication group type property, whether sysvol or protection for the
        ///                                                   replication groups.
        ///													  Key --> Replication Group Name: Value --> Replication group Type (whether
        ///                                                                                             sysvol or protection)
        /// (5).
        /// Map<int, string> folderReplGrp --> Folders need to exist on the server. The folders belong to the replication groups.
        ///                                    Key --> FolderID (same as contentSetID.(used as int identifiers in Model, adpater maps 
        ///                                    abstracted FolderID values to actual FolderIDs/Folder Names from ptfconfig)
        /// (6).
        /// Map<int, accessLevels> folderAccLevel --> The access levels of folders need to be stored.
        ///                                           Key --> FolderID: Value --> Access Level of the folder
        ///
        /// (7).
        /// Map<int, connectionProperty> folderDFRS --> Maintains the ms-DFSR-Enabled property whether enabled or disabled for the
        ///                                             replicated folder
        ///                                             Key --> FolderID: Value --> ms-DFSR connection property enabled or disabled
        ///
        /// EstablishConnection/EstablishSession/AsyncPoll --> In order to reduce test-cases, we are using only valid values for these
        ///                                                    methods. Scenario 7 checks for all possible combinations for these
        ///                                                    methods. Scheme is enforced in order to cut down on redundant test-cases,
        ///                                                    which will be the same for scenario 7 and for other scenarios if not 
        ///                                                    constrained
        /// In model connectionIDs/folderIDs/replication groups and other properties and conectionProperty are used as abstract 
        /// identifiers Since the signature of methods are the same in both Model and Adapter, the test case details are shared 
        /// by both. The adapter maps the actual (real-time test values) depending upon the abstract identifier value that has come
        /// from the cord/test-case.
        /// </summary>
        /// <param name="serverConn">Stores the connectionIDs 
        /// and whether the corresponding connection is enabled or disabled. 
        /// Key --> connectionID: Value --> connectionProperty</param>
        /// <param name="fromServConn">Stores the connectionIDs and status of FromServer (TD Section 2.3.11 DN of the  
        /// inbound partner(other msDFSR-Member object). MUST exist)
        /// Key --> connectionID: Value --> status of FromServer</param>
        /// <param name="replGrpConn">Some replication groups must be created on the server with respect to the connections.
        /// Key --> Replication Group name (used as string identifiers in Model, adpater maps actual 
        /// replication group name from ptfconfig): Value --> connectionID</param>
        /// <param name="replGrpType">Stores Replication group type property, whether sysvol or protection for the
        /// replication groups. Key --> Replication Group Name: Value --> Replication group Type (whether
        /// sysvol or protection)</param>
        /// <param name="folderReplGrp">Folders need to exist on the server. The folders belong to the replication groups.
        /// Key --> FolderID (same as contentSetID.(used as int identifiers in Model, adpater maps 
        /// abstracted FolderID values to actual FolderIDs/Folder Names from ptfconfig)</param>
        /// <param name="folderAccLevel">The access levels of folders need to be stored.
        /// Key --> FolderID: Value --> Access Level of the folder</param>
        /// <param name="folderDFRS">Maintains the ms-DFSR-Enabled property whether enabled or disabled for the
        /// replicated folder. Key --> FolderID: Value --> ms-DFSR connection property enabled or disabled</param>
        /// <returns>error_status_t</returns>
        error_status_t Initialization(OSVersion osVersion,
                                      Map<int, connectionProperty> serverConn,
                                      Map<int, FromServer> fromServConn,
                                      Map<string, int> replGrpConn,
                                      Map<string, ReplicationGroupTypes> replGrpType,
                                      Map<int, string> folderReplGrp,
                                      Map<int, accessLevels> folderAccLevel,
                                      Map<int, connectionProperty> folderDFRS);

        /// <summary>
        /// This method is used for a client to check whether the server is reachable and has been configured to replicate with the server.
        /// </summary>
        /// <param name="replicationID">From the GUID of the replication group</param>
        /// <param name="connectionId">From the GUID of the connection</param>
        /// <returns>error_status_t</returns>

        error_status_t CheckConnectivity(string replicationID, int connectionId);

        /// <summary>
        /// This method establishes a logical connection from a client to a server.
        /// </summary>
        /// <param name="replicationID">From the GUID of the replication group</param>
        /// <param name="connectionId">From the GUID of the connection</param>
        /// <param name="downstreamProtocolVersion">Identifies the version of the protocol implemented by the client.</param>
        /// <param name="upstreamProtocolVersion">Receives the server's protocol version number. Expected values are the same as for downstreamProtocolVersion</param>
        /// <param name="upstreamFlags">A flags bitmask.</param>
        /// <returns>error_status_t</returns>

        error_status_t EstablishConnection(string replicationID,
                                           int connectionId,
                                           ProtocolVersion downstreamProtocolVersion,
                                           out ProtocolVersionReturned upstreamProtocolVersion,
                                           out UpstreamFlagValueReturned upstreamFlags);

        /// <summary>
        /// The EstablishSession method is used to establish a logical relationship on the server for a replicated folder.
        /// </summary>
        /// <param name="connectionId">From the GUID of the connection</param>
        /// <param name="contentSetId">The GUID of the configured replicated folder.</param>
        /// <returns>error_status_t</returns>

        error_status_t EstablishSession(int connectionId, int contentSetId);

        /// <summary>
        /// This method used to register an asynchronous callback for a server to provide version chain vectors.
        /// </summary>
        /// <param name="connectionId">From the GUID of the connection</param>
        /// <returns>error_status_t</returns>

        error_status_t AsyncPoll(int connectionId);

        /// <summary>
        /// This method used to retrieve UIDs and GVSNs that a server persists.
        /// </summary>
        /// <param name="connectionId">From the GUID of the connection</param>
        /// <param name="contentSetId">The GUID of the configured replicated folder.</param>
        /// <returns>error_status_t</returns>

        error_status_t RequestRecords(int connectionId, int contentSetId);

        /// <summary>
        /// This method is used to obtain file metadata in the form of updates from a server.
        /// </summary>
        /// <param name="connectionId">From the GUID of the connection</param>
        /// <param name="contentSetId">The GUID of the configured replicated folder.</param>
        /// <param name="requestType">The value from the UPDATE_REQUEST_TYPE enumeration that indicates the type of replication updates requested.</param>
        /// <param name="versionVectDiff">The abstraction of  FRS_VERSION_VECTOR structures that specifies what updates that the client requires from the server</param>
        /// <returns>error_status_t</returns>

        error_status_t RequestUpdates(int connectionId,
                                      int contentSetId,
                                      //                                       UPDATE_REQUEST_TYPE requestType,
                                      versionVectorDiff versionVectDiff);

        /// <summary>
        /// This method is used to obtain the version chain vector persisted on a server.
        /// </summary>
        /// <param name="connectionId">From the GUID of the connection</param>
        /// <param name="contentSetId">The GUID of the configured replicated folder.</param>
        /// <param name="requestType">The value from the VERSION_REQUEST_TYPE enumeration that describes the type of replication sync to perform.</param>
        /// <param name="changeType">The value from the VERSION_CHANGE_TYPE enumeration that indicates whether to notify change only, change delta, or send the entire version chain vector.</param>
        /// <param name="sequenceNumber">The sequence Number for this request</param>
        /// <param name="vvGeneration">The vvGeneration abstraction is used to calibrate what incarnation of the server's version chain vector is known to the client. </param>
        /// <returns>error_status_t</returns>

        error_status_t RequestVersionVector(int sequenceNumber,
                                            int connectionId,
                                            int contentSetId,
                                            VERSION_REQUEST_TYPE requestType,
                                            VERSION_CHANGE_TYPE changeType,
                                            VVGeneration vvGeneration);

        /// <summary>
        /// This method is used by a client to indicate to a server that it could not process an update.
        /// </summary>
        /// <param name="connectionId">From the GUID of the connection</param>
        /// <param name="cancelData">The FRS_UPDATE_CANCEL_DATA structure that describes an update to cancel.</param>
        /// <param name="contentSetFieldCANCEL_DATA">From the GUID of the connection with Cancel data</param>
        /// <returns>error_status_t</returns>

        error_status_t UpdateCancel(int connectionId,
                                    FRS_UPDATE_CANCEL_DATA cancelData,
                                    int contentSetFieldCANCEL_DATA);

        /// <summary>
        /// This method is used by a client to start a file download.
        /// </summary>
        ///<param name="connectionId">From the GUID of the connection</param>
        ///<param name="frsUpdateContentSetID">From the GUID of the connection</param>
        /// <returns>error_status_t</returns>

        error_status_t InitializeFileTransferAsync(int connectionId,
                                                int frsUpdateContentSetID,
                                                bool rdcCheck);

        /// <summary>
        /// This method is used for to transfer successive segments from a file.
        /// </summary>
        /// <returns>error_status_t</returns>

        error_status_t RawGetFileData();
        /// <summary>
        /// This method is used to obtain RDCsignature data from a server.
        /// </summary>
        /// <param name="offSet"> abstraction at which to retrieve data from the file.</param>
        /// <returns>error_status_t</returns>

        error_status_t RdcGetSignatures(offset offSet);

        /// <summary>
        /// This method is used to register requests for file ranges on a server.
        /// </summary>
        /// <returns>error_status_t</returns>

        error_status_t RdcPushSourceNeeds();

        /// <summary>
        /// This method is used to obtain file ranges whose requests have previously been registered on a server.
        /// </summary>
        /// <returns>error_status_t</returns>

        error_status_t RdcGetFileData(BufferSize bufferSize);

        /// <summary>
        /// This method informs the server that the server context information can be released.
        /// </summary>
        /// <returns>error_status_t</returns>

        error_status_t RdcClose();

        /// <summary>
        /// The RawGetFileDataAsync method is used instead of calling RawGetFileData multiple times to obtain file data.
        /// </summary>
        /// <returns>error_status_t</returns>

        error_status_t RawGetFileDataAsync();

        /// <summary>
        /// The RdcGetFileDataAsync method is used instead of calling RawGetFileData multiple times to obtain file data.
        /// </summary>
        /// <returns>error_status_t</returns>

        error_status_t RdcGetFileDataAsync();

        void BkupFsccValidation();

        void SetTraditionalTestFlag();
        void SetRdcGetSigTestFlag();
        void SetPushSourceNeedsTestFlag();
        void SetRdcCloseTestFlag();
        void SetRdcGetSigFailTestFlag();
        void SetRdcGetSigLevelTestFlag();
        void SetPushSourceNeedsTestFlagForNeedCount();

        void Validate_FSCC_BKUP_Requirements(ReplicatedFileStructure objDataBuffer, bool ParserResult);

        bool InShutdown { get; set; }

        void GeneralInitialize();
    }
}
