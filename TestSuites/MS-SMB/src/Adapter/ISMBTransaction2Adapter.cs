// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// TRANS2_QUERY_FILE_INFORMATION Response handler.
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
    public delegate void Trans2QueryFileInfoResponseHandler(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        MessageStatus messageStatus);

    /// <summary>
    /// TRANS2_QUERY_PATH_INFORMATION Response handler.
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
    public delegate void Trans2QueryPathInfoResponseHandler(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        MessageStatus messageStatus);

    /// <summary>
    /// TRANS2_QUERY_FS_INFORMATION Response handler.
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
    public delegate void Trans2QueryFsInfoResponseHandler(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        MessageStatus messageStatus);

    /// <summary>
    /// TRANS2_SET_FILE_INFORMATION Response handler.
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
    public delegate void Trans2SetFileInfoResponseHandler(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        MessageStatus messageStatus);

    /// <summary>
    /// TRANS2_SET_PATH_INFORMATION Response handler.
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
    public delegate void Trans2SetPathInfoResponseHandler(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        MessageStatus messageStatus);

    /// <summary>
    /// TRANS2_SET_FS_INFORMATION Response handler.
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
    public delegate void Trans2SetFsInfoResponseHandler(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        MessageStatus messageStatus);

    /// <summary>
    /// TRANS2_FIND_FIRST2 response handler.
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
    /// <param name="isFileIdEqualZero">Indicate whether the fileId equals 0.</param>
    /// <param name="searchHandlerId">Search Handler.</param>
    /// <param name="returnEnumPreviousVersion">
    /// Indicate whether the SUT will return the previous version.
    /// </param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// <param name="isRS2398Implemented">If the RS2398 implemented, it is true, else it is false.</param>
    /// <param name="isRS4899Implemented">If the RS4899 implemented, it is true, else it is false.</param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void Trans2FindFirst2ResponseHandler(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        bool isFileIdEqualZero,
        int searchHandlerId,
        bool returnEnumPreviousVersion,
        MessageStatus messageStatus,
        bool isRS2398Implemented,
        bool isRS4899Implemented);

    /// <summary>
    /// Trans2SetFSInfo Response additional handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">
    /// Set this value to 0 to request a new session setup, or set this value to a previously
    /// established session identifier to reauthenticate to an existing session.
    /// </param>
    /// <param name="treeId">
    /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the server that 
    /// the client is accessing.
    /// </param>
    /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void Trans2SetFsInfoResponseAdditionalHandle(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        MessageStatus messageStatus);

    /// <summary>
    /// ErrorTrans2SetFSInfo response additional handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void ErrorTrans2SetFsInfoResponseAdditionalHandle(int messageId, MessageStatus messageStatus);

    /// <summary>
    /// TRANS2_FIND_NEXT2 Response handler.
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">
    /// Set this value to 0 to request a new session setup, or set this value to a previously 
    /// established session identifier to reauthenticate to an existing session.
    /// </param>
    /// <param name="treeId">
    /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the server that 
    /// the client is accessing.
    /// </param>
    /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
    /// <param name="isFileIdEqualZero">Indicate whether the fileId equals 0.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void Trans2FindNext2ResponseHandler(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        bool isFileIdEqualZero,
        MessageStatus messageStatus);

    #region FSCC methods

    /// <summary>
    /// FSCC Trans2 QueryPathInfo Response Handle
    /// </summary>
    /// <param name="messageId">This is used to associate a response with a request.</param>
    /// <param name="sessionId">
    /// Set this value to 0 to request a new session setup, or set this value to a previously 
    /// established session identifier to reauthenticate to an existing session.
    /// </param>
    /// <param name="treeId">
    /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the server that 
    /// the client is accessing.
    /// </param>
    /// <param name="isSigned">Indicate whether the SUT has message signing enabled or required.</param>
    /// <param name="messageStatus">
    /// Indicate that the status code returned from the SUT is success or failure.
    /// </param>
    /// Disable warning CA1009 because according to Test Case design, 
    /// the two parameters, System.Object and System.EventArgs, are unnecessary.
    [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
    public delegate void FSCCTrans2QueryPathInfoResponseHandle(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        MessageStatus messageStatus);

    /// <summary>
    /// TRANS2_QUERY_FS_INFORMATION Response handler
    /// </summary>
    /// <param name="messageId"> This is used to associate a response with a request.</param>
    /// <param name="sessionId"> Set this value to 0 to request a new session setup, or set this value to a 
    /// previously established session identifier to reauthenticate to an existing session.</param>
    /// <param name="treeId"> This field identifies the subdirectory (or tree) (also referred to as a share in 
    /// this document) on the server that the client is accessing.</param>
    /// <param name="isSigned"> Indicates whether the server has message signing enabled or required.</param>
    /// <param name="messageStatus"> Indicate the status code returned from server, success or fail.</param>
    public delegate void FSCCTrans2QueryFSInfoResponseHandle(
        int messageId,
        int sessionId,
        int treeId,
        bool isSigned,
        MessageStatus messageStatus);

    #endregion


    /// <summary>
    /// ISMBTransaction2 Adapter.
    /// </summary>
    public partial interface ISmbAdapter : IAdapter
    {
        /// <summary>
        /// TRANS2_QUERY_FILE_INFORMATION response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event Trans2QueryFileInfoResponseHandler Trans2QueryFileInfoResponse;

        /// <summary>
        /// TRANS2_QUERY_PATH_INFORMATION response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event Trans2QueryPathInfoResponseHandler Trans2QueryPathInfoResponse;

        /// <summary>
        /// TRANS2_QUERY_FS_INFORMATION response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event Trans2QueryFsInfoResponseHandler Trans2QueryFsInfoResponse;

        /// <summary>
        /// TRANS2_SET_FILE_INFORMATION response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event Trans2SetFileInfoResponseHandler Trans2SetFileInfoResponse;

        /// <summary>
        /// TRANS2_SET_PATH_INFORMATION response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event Trans2SetPathInfoResponseHandler Trans2SetPathInfoResponse;

        /// <summary>
        /// TRANS2_SET_FS_INFORMATION response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event Trans2SetFsInfoResponseHandler Trans2SetFsInfoResponse;

        /// <summary>
        /// TRANS2_FIND_FIRST2 response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event Trans2FindFirst2ResponseHandler Trans2FindFirst2Response;

        /// <summary>
        /// TRANS2_FIND_NEXT2 response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event Trans2FindNext2ResponseHandler Trans2FindNext2Response;

        /// <summary>
        /// Trans2SetFSInfo Response additional handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event Trans2SetFsInfoResponseAdditionalHandle Trans2SetFsInfoResponseAdditional;

        /// <summary>
        /// ErrorTrans2SetFSInfo response additional handler.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event ErrorTrans2SetFsInfoResponseAdditionalHandle ErrorTrans2SetFsInfoResponseAdditional;

        /// <summary>
        /// Check previous version.
        /// </summary>
        /// <param name="fid">The file identifier.</param>
        /// <param name="previousVersion">The previous version.</param>
        /// <param name="isSucceed">Indicate whether the checking is successful or not.</param>
        void CheckPreviousVersion(int fid, Microsoft.Modeling.Set<int> previousVersion, out bool isSucceed);

        /// <summary>
        /// TRANS2_QUERY_FILE_INFORMATION Request handler.
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
        /// <param name="isUsePassthrough">
        /// Indicate whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="reserved">The reserved int value.</param>
        void Trans2QueryFileInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isUsePassthrough,
            [Domain("InfoLevelQueriedByFid")] InformationLevel informationLevel,
            int fid,
            int reserved);

        /// <summary>
        /// TRANS2_QUERY_FILE_INFORMATION Request.
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
        /// <param name="isUsePassthrough">
        /// Indicate whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="isReparse">Indicate whether it is reparsed or not.</param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="gmtTokenIndex">The index of the GMT token configured by CheckPreviousVersion action.</param>
        void Trans2QueryPathInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isUsePassthrough,
            bool isReparse,
            [Domain("InfoLevelQueriedByPath")] InformationLevel informationLevel,
            int gmtTokenIndex);

        /// <summary>
        /// TRANS2_QUERY_FS_INFORMATION Request.
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
        /// <param name="isUsePassthrough">
        /// Indicate whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="otherBits">If it contains other bits.</param>
        /// <param name="reserved">The reserved int value.</param>
        void Trans2QueryFsInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isUsePassthrough,
            [Domain("InfoLevelQueriedByFS")] InformationLevel informationLevel,
            bool otherBits,
            int reserved);

        /// <summary>
        /// TRANS2_SET_FILE_INFORMATION Request.
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
        /// <param name="relaceEnable">
        /// Indicate whether the new name or link will replace the original one that exists already.
        /// </param>
        /// <param name="isUsePassthrough">
        /// Indicate whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="isRootDirecotyNull">Whether the root directory is null.</param>
        /// <param name="reserved">The reserved int value.</param>
        void Trans2SetFileInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool relaceEnable,
            bool isUsePassthrough,
            [Domain("InfoLevelSetByFid")] InformationLevel informationLevel,
            int fid,
            string fileName,
            bool isRootDirecotyNull,
            int reserved);

        /// <summary>
        /// TRANS2_SET_PATH_INFORMATION Request.
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
        /// <param name="isUsePassthrough">
        /// Indicate whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="isReparse">Indicate whether it is reparsed or not.</param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="gmtTokenIndex">The index of the GMT token configured by CheckPreviousVersion action.</param>
        void Trans2SetPathInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isUsePassthrough,
            bool isReparse,
            [Domain("InfoLevelSetByPath")] InformationLevel informationLevel,
            int gmtTokenIndex);

        /// <summary>
        /// TRANS2_SET_FS_INFORMATION Request.
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
        /// <param name="isUsePassthrough">
        /// Indicate whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="requireDisconnectTreeFlags">Indicate whether flags set to Disconnect_TID.</param>
        /// <param name="requireNoResponseFlags">Indicate whether flags set to NO_RESPONSE.</param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="fid">The file identifier.</param>
        /// <param name="otherBits">If it contains other bits.</param>
        /// <param name="reserved">The reserved int value.</param>
        void Trans2SetFsInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isUsePassthrough,
            bool requireDisconnectTreeFlags,
            bool requireNoResponseFlags,
            InformationLevel informationLevel,
            int fid,
            bool otherBits,
            int reserved);

        /// <summary>
        /// TRANS2_FIND_FIRST2 Request.
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
        /// <param name="isReparse">Indicate whether it is reparsed or not.</param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="gmtTokenIndex">The index of the GMT token configured by CheckPreviousVersion action.</param>
        /// <param name="isFlagsKnowsLongNameSet">
        /// Indicate the adpater to set the SMB_FLAGS2_KNOWS_LONG_NAMES flag in smb header or not.
        /// </param>
        /// <param name="isGmtPattern">Whether it is GMT pattern.</param>
        void Trans2FindFirst2Request(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isReparse,
            [Domain("InfoLevelByFind")] InformationLevel informationLevel,
            int gmtTokenIndex,
            bool isFlagsKnowsLongNameSet,
            bool isGmtPattern);

        /// <summary>
        /// TRANS2_FIND_FIRST2 Request for invalid directory token.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the current share connection.</param>
        /// <param name="isSigned">Indicate whether the message is signed or not for this request.</param>
        /// <param name="isReparse">
        /// Indicate whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of the SMB header.
        /// </param>
        /// <param name="informationLevel"> The information level used for this request.</param>
        /// <param name="gmtTokenIndex"> The index of the GMT token configured by CheckPreviousVersion action.</param>
        /// <param name="isFlagsKnowsLongNameSet">
        /// Indicate whether the SMB_FLAGS2_KNOWS_LONG_NAMES flag in SMB header is set or not.
        /// </param>
        /// <param name="isGmtPatten">Indicate whether the GMT Patten is used.</param>
        void Trans2FindFirst2RequestInvalidDirectoryToken(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isReparse,
            InformationLevel informationLevel,
            int gmtTokenIndex,
            bool isFlagsKnowsLongNameSet,
            bool isGmtPatten);

        /// <summary>
        /// TRANS2_FIND_FIRST2 Request for invalid file token.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the current share connection.</param>
        /// <param name="isSigned">Indicate whether the message is signed or not for this request.</param>
        /// <param name="isReparse">
        /// Indicate whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of the SMB header.
        /// </param>
        /// <param name="informationLevel"> The information level used for this request.</param>
        /// <param name="gmtTokenIndex"> The index of the GMT token configured by CheckPreviousVersion action.</param>
        /// <param name="isFlagsKnowsLongNameSet">
        /// Indicate whether the SMB_FLAGS2_KNOWS_LONG_NAMES flag in SMB header is set or not.
        /// </param>
        /// <param name="isGmtPatten"> Indicate whether the GMT Patten is used.</param>
        void Trans2FindFirst2RequestInvalidFileToken(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isReparse,
            InformationLevel informationLevel,
            int gmtTokenIndex,
            bool isFlagsKnowsLongNameSet,
            bool isGmtPatten);

        /// <summary>
        /// TRANS2_SET_FS_INFORMATION request additional.
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
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="requestPara">Trans2SetFSInfo response parameter.</param>
        void Trans2SetFsInfoRequestAdditional(
            int messageId,
            int sessionId,
            int treeId,
            int fid,
            bool isSigned,
            [Domain("InfoLevelSetByFS")] InformationLevel informationLevel,
            Trans2SetFsInfoResponseParameter requestPara);

        /// <summary>
        /// TRANS2_FIND_FIRST2 Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a
        /// previously established session identifier to reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the server 
        /// that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="isReparse">Indicate whether it is reparsed or not.</param>
        /// <param name="informationLevel">This can be used to query information from the server.</param>
        /// <param name="sid">The sid.</param>
        /// <param name="gmtTokenIndex">The index of the GMT token configured by CheckPreviousVersion action.</param>
        /// <param name="isFlagsKnowsLongNameSet">
        /// Indicate the adpater to set the SMB_FLAGS2_KNOWS_LONG_NAMES flag in smb header or not.
        /// </param>
        void Trans2FindNext2Request(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isReparse,
            [Domain("InfoLevelByFind")] InformationLevel informationLevel,
            int sid,
            int gmtTokenIndex,
            bool isFlagsKnowsLongNameSet);

        #region FSCC methods

        /// <summary>
        /// FSCC TRANS2_QUERY_PATH_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a
        /// previously established session identifier to reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the server 
        /// that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="informationLevel">Indicate the query path info level</param>
        void FSCCTrans2QueryPathInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            FSCCTransaction2QueryPathInforLevel informationLevel);

        /// <summary>
        /// FSCC TRANS2_QUERY_PATH_INFORMATION response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event FSCCTrans2QueryPathInfoResponseHandle FSCCTrans2QueryPathInfoResponse;




        /// <summary>
        /// FSCC TRANS2_QUERY_FS_INFORMATION response.
        /// </summary>
        /// Disable warning CA1009 because according to Test Case design, 
        /// the two parameters, System.Object and System.EventArgs, are unnecessary.
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        event FSCCTrans2QueryFSInfoResponseHandle FSCCTrans2QueryFSInfoResponse;

        /// <summary>
        /// FSCC TRANS2_QUERY_FS_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a
        /// previously established session identifier to reauthenticate to an existing session.
        /// </param>
        /// <param name="treeId">
        /// This field identifies the subdirectory (or tree) (also referred as a share in this document) on the server 
        /// that the client is accessing.
        /// </param>
        /// <param name="isSigned">
        /// Indicate whether the SUT has message signing enabled or required.
        /// </param>
        /// <param name="informationLevel">Indicate the query FS info level</param>
        void FSCCTrans2QueryFSInfoRequest(int messageId,
                                  int sessionId,
                                  int treeId,
                                  bool isSigned,
                                  FSCCTransaction2QueryFSInforLevel informationLevel);
        #endregion
    }
}
