// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// NTTransQueryQuotaResponseHandler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">
    /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
    /// identifier to request reauthenticate to an existing session.
    /// </param>
    /// <param name="treeId">
    /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the server 
    /// that the client is accessing.
    /// </param>
    /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
    /// <param name="quotaInfo"> The amount of quota, in bytes, used by this user.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void NtTransQueryQuotaResponseHandler(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        int quotaInfo,
        MessageStatus messageStatus);

    /// <summary>
    /// NTTransSetQuotaResponseHandler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">
    /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
    /// identifier to request reauthenticate to an existing session.
    /// </param>
    /// <param name="treeId">
    /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the server 
    /// that the client is accessing.
    /// </param>
    /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void NtTransSetQuotaResponseHandler(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        MessageStatus messageStatus);

    /// <summary>
    /// FSCTLSrvEnumSnapshotsResponseHandler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">
    /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
    /// identifier to reauthenticate to an existing session.
    /// </param>
    /// <param name="treeId">
    /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the server 
    /// that the client is accessing.
    /// </param>
    /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// <param name="NumberOfSnapShotsCompared">
    /// Return the comparison result of NumberOfSnapShots and NumberOfSnapShotsReturned.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void FsctlSrvEnumSnapshotsResponseHandler(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        MessageStatus messageStatus,
        IntegerCompare NumberOfSnapShotsCompared);

    /// <summary>
    /// FSCTLSrvRequestResumeKeyResponseHandler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">
    /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
    /// identifier to request reauthenticate to an existing session.
    /// </param>
    /// <param name="treeId">
    /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the server 
    /// that the client is accessing.
    /// </param>
    /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
    /// <param name="copychunkResumeKey">The server resume key for a source file.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void FsctlSrvRequestResumeKeyResponseHandler(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        string copychunkResumeKey,
        MessageStatus messageStatus);

    /// <summary>
    /// FSCTLSrvCopyChunkResponseHandler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">
    /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
    /// identifier to request reauthenticate to an existing session.
    /// </param>
    /// <param name="treeId">
    /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the server 
    /// that the client is accessing.
    /// </param>
    /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void FsctlSrvCopyChunkResponseHandler(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        MessageStatus messageStatus);

    /// <summary>
    /// NTTransactCreateResponseHandle.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">
    /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
    /// identifier to request reauthenticate to an existing session.
    /// </param>
    /// <param name="treeId">
    /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the server 
    /// that the client is accessing.
    /// </param>
    /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void NtTransactCreateResponseHandle(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        MessageStatus messageStatus);

    /// <summary>
    /// NTTransSetQuotaResponseAdditionalHandle.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">
    /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
    /// identifier to request reauthenticate to an existing session.
    /// </param>
    /// <param name="treeId">
    /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the server 
    /// that the client is accessing.
    /// </param>
    /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void NtTransSetQuotaResponseAdditionalHandle(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        MessageStatus messageStatus);

    /// <summary>
    /// ErrorNTTransSetQuotaResponseAdditionalHandle.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void ErrorNtTransSetQuotaResponseAdditionalHandle(
        int messageId,
        MessageStatus messageStatus);

    #region FSCC part



    /// <summary>
    /// NT transact IOCTL response handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">This is used to indicate the connection between the client and the server.</param>
    /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
    /// <param name="isMessageSigned">This is used to indicate whether the message is signed or not.</param>
    /// <param name="statusCode">This is used to indicate the status code returned from server.</param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void FSCCFSCTLNameResponseHandle(int messageId,
                                                     int sessionId,
                                                     int treeId,
                                                     bool isSigned,
                                                     MessageStatus messageStatus);

    #endregion

    /// <summary>
    /// ISMBNTTransaction Adapter.
    /// </summary>
    public partial interface ISmbAdapter : IAdapter
    {
        /// <summary>
        /// NTTrans create response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event NtTransactCreateResponseHandle NtTransactCreateResponse;

        /// <summary>
        /// NTTrans set quota response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event NtTransSetQuotaResponseAdditionalHandle NtTransSetQuotaResponseAdditional;

        /// <summary>
        /// Error NTTrans set quota response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event ErrorNtTransSetQuotaResponseAdditionalHandle ErrorNtTransSetQuotaResponseAdditional;

        /// <summary>
        /// NTTrans queryQuota response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event NtTransQueryQuotaResponseHandler NtTransQueryQuotaResponse;

        /// <summary>
        /// NTTransSet quota response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event NtTransSetQuotaResponseHandler NtTransSetQuotaResponse;

        /// <summary>
        /// FSCTLSrvEnumSnapshots response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event FsctlSrvEnumSnapshotsResponseHandler FsctlSrvEnumSnapshotsResponse;

        /// <summary>
        /// FSCTLSrvRequestResumeKey response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event FsctlSrvRequestResumeKeyResponseHandler FsctlSrvRequestResumeKeyResponse;

        /// <summary>
        /// FSCTLSrvCopyChunk response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event FsctlSrvCopyChunkResponseHandler FsctlSrvCopyChunkResponse;

        /// <summary>
        /// CheckSnapshots Client Request.
        /// </summary>
        /// <param name="fid">The file identifier.</param>
        /// <param name="snapShots">snapShots.</param>
        /// <param name="isSucceed">If it succeeds, true is returned; Otherwise, false is returned.</param>
        void CheckSnapshots(int fid, Microsoft.Modeling.Set<int> snapShots, out bool isSucceed);

        /// <summary>
        /// NT_TRANSACT_QUERY_QUOTA Client Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="returnSingle">
        /// A bool variable, if set, which indicates only a single entry is to be returned instead of filling the 
        /// entire buffer.
        /// </param>
        /// <param name="restartScan">
        /// A bool variable, if set, which indicates that the scan of the quota information is to be restarted.
        /// </param>
        /// <param name="sidListLength">Supplies the length in bytes of the SidList.</param>
        /// <param name="startSidLength">Supplies the length in bytes of the StartSid.</param>
        /// <param name="startSidOffset">
        /// Supplies the offset, in bytes, to the StartSid in the Parameter buffer.
        /// </param>
        void NtTransQueryQuotaRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int fid,
            bool returnSingle,
            bool restartScan,
            int sidListLength,
            int startSidLength,
            int startSidOffset);


        /// <summary>
        /// NT_TRANSACT_SET_QUOTA Client Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="quotaInfo">The amount of quota, in bytes, used by this user.</param>
        void NtTransSetQuotaRequest(
            int messageId,
            int sessionId,
            int treeId,
            int fid,
            bool isSigned,
            int quotaInfo);

        /// <summary>
        /// FSCTL_SRV_ENUMERATE_SNAPSHOTS Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="maxDataCount">This is used to control the MaxDataCount in FSCTL_SRV_ENUMERATE_SNAPSHOTS.</param>
        void FsctlSrvEnumSnapshotsRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int fid,
            MaxDataCount maxDataCount);

        /// <summary>
        /// FSCTL_SRV_REQUEST_RESUME_KEY Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="fid">The file identifier.</param>
        void FsctlSrvRequestResumeKeyRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int fid);

        /// <summary>
        /// NT_TRANSACT_IOCTL Client Request.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="sessionId">Session ID used to identify the session.</param>
        /// <param name="treeId">Tree ID used to identify the tree connection.</param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="copychunkResumeKey">The server resume key for a source file.</param>
        /// <param name="sourceOffset">The offset in the source file to copy from.</param>
        /// <param name="targetOffset">The offset in the target file to copy to.</param>
        /// <param name="length">The number of bytes to copy from the source file to the target file.</param>
        void FsctlSrvCopyChunkRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int fid,
            string copychunkResumeKey,
            int sourceOffset,
            int targetOffset,
            int length);

        /// <summary>
        /// Transfer create request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">Set this value to 0 to request a new session setup, or set this value to a 
        /// previously established session identifier to reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a sharein this document) on the server 
        /// that the client is accessing.
        /// </param>
        /// <param name="impersonationLevel">The impersonation level.</param>
        /// <param name="name">The file name that will be created.</param>
        /// <param name="shareType">The share type.</param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        void NtTransactCreateRequest(
            int messageId,
            int sessionId,
            int treeId,
            [Domain("ImpersonationLevel")] int impersonationLevel,
            [Domain("FileDomain")] string name,
            ShareType shareType,
            bool isSigned);

        /// <summary>
        /// NTTrans set quota request additional.
        /// </summary>
        /// <param name="messageId">Message ID used to identify the message.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session 
        /// identifier to request reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the 
        /// server that the client is accessing.
        /// </param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="requestPara">NTTrans set quota request parameter.</param>
        void NtTransSetQuotaRequestAdditional(
            int messageId,
            int sessionId,
            int treeId,
            int fid,
            bool isSigned,
            NtTransSetQuotaRequestParameter requestPara);

        #region FSCC methods

        /// <summary>
        /// NT transact IOCTL response handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event FSCCFSCTLNameResponseHandle FSCCFSCTLNameResponse;


        void FSCCFSCTLNameRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            int fid,
            FSCCFSCTLName fsctlName);

        #endregion
    }
}
