// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// SMB_COM_TRANSACTION2 Model prgoram.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public static partial class BaseModelProgram
    {
        #region SMB_COM_TRANSACTION2 Extensions

        #region Check the previous version

        /// <summary>
        /// Configure the previous version for the designated file identifier.
        /// </summary>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        /// <param name="previousVersion">The previous version list will be configured for this file.</param>
        /// <param name="isSuccess">It indicates whether the checking is successful or not.</param>
        [Rule]
        public static void CheckPreviousVersion(int fId, Set<int> previousVersion, out bool isSuccess)
        {
            Modeling.Condition.IsTrue((smbState != SmbState.End) && (smbState != SmbState.Closed));
            Modeling.Condition.IsTrue(Parameter.isSupportPreviousVersion);
            ModelHelper.CaptureRequirement(
                8554,
                "[In Scanning a Path for a the previous version Token] If a request is a path-based operation (for " +
                "example, SMB_COM_NT_CREATE_ANDX) and has SMB_FLAGS2_REPARSE_PATH set in the Flag2 field of the SMB " +
                "header, then the server MUST perform a parse of the path by checking for the previous version tokens " +
                "(section 2.2.1.1.1).");

            isSuccess = true;
            Modeling.Condition.IsTrue(smbConnection.openedFiles.ContainsKey(fId)
                                        && smbConnection.openedFiles[fId].name == Parameter.fileNames[2]);
            Modeling.Condition.IsTrue(smbConnection.sentRequest.Count == 0);
            Modeling.Condition.IsTrue(previousVersion == new Set<int>(1, 2));
            smbConnection.openedFiles[fId].previousVersionToken = previousVersion;

            // Adding "-1" indicates that an invalid time stamp is provided, so the version of file cannot be found.
            smbConnection.openedFiles[fId].previousVersionToken = smbConnection.openedFiles[fId].previousVersionToken.Add(-1);
        }

        #endregion

        #region TRANS2_QUERY_FILE_INFORMATION

        /// <summary>
        /// TRANS2_QUERY_FILE_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the current share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this request.</param>
        /// <param name="isUsePassthrough">
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        /// <param name="informationLevel">The information level used for this request.</param>
        /// <param name="reserved">
        /// It is ignored by System Under Test (the SUT), used to be tested in traditional test.
        /// </param>
        [Rule]
        public static void Trans2QueryFileInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isUsePassthrough,
            [Domain("InfoLevelQueriedByFid")] InformationLevel informationLevel,
            int fId,
            int reserved)
        {
            Condition.IsTrue(smbConnection.openedFiles.ContainsKey(fId) && smbConnection.openedFiles[fId].treeId == treeId);
            Condition.IsTrue(reserved == 0);
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);
            if (informationLevel == InformationLevel.FileAccessInformation)
            {
                Condition.IsTrue(isUsePassthrough);
            }
            else
            {
                Condition.IsTrue(!isUsePassthrough);
            }

            if (!Parameter.isSupportStream &&
                informationLevel == InformationLevel.SmbQueryFileStreamInfo)
            {
                Requirement.AssumeCaptured("MS-SMB_R2073 and MS-SMB_R2076 will be captured under this condition.");
            }

            smbRequest = new Trans2QueryFileInfoRequest(
                messageId,
                sessionId,
                treeId,
                isSigned,
                fId,
                isUsePassthrough,
                informationLevel);
            Update.UpdateRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// TRANS2_QUERY_FILE_INFORMATION Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void Trans2QueryFileInfoResponse(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            MessageStatus messageStatus)
        {
            Trans2QueryFileInfoRequest request = null;
            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.Trans2QueryFileInfoRequest>(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                messageStatus,
                out request,
                MessageStatus.Success);
            Condition.IsTrue(request.command == Command.Trans2QueryFileInformation);

            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    9319,
                    "[In response. Extensions]A server MUST send a TRANS2_QUERY_FILE_INFORMATION response in reply to " +
                    "an SMB_COM_TRANSACTION2 client request with a TRANS2_QUERY_FILE_INFORMATION subcommand when the " +
                    "request is successful.");
            }

            if (request.informationLevel == InformationLevel.SmbQueryFileStreamInfo)
            {
                Condition.IsTrue(Parameter.isSupportStream);
            }
            if (request.isUsePassthrough)
            {
                Condition.IsTrue(smbConnection.sutCapabilities.Contains(Capabilities.CapInfoLevelPassThru));
                ModelHelper.CaptureRequirement(
                    30048,
                    @"[In Receiving any Information Level] The returned status and response data, if any, are sent to 
                    the client in a Trans2 subcommand response message that corresponds to the same subcommand that 
                    initiated the request.");
                ModelHelper.CaptureRequirement(
                    30028,
                    @"[In Receiving a TRANS2_QUERY_FILE_INFORMATION Request Pass-through Information Levels] If the 
                    client requests a pass-through Information Level, then the processing [Receiving a 
                    TRANS2_QUERY_FILE_INFORMATION Request] follows as specified in section 3.3.5.9.1.");
            }
            Update.UpdateResponse(smbConnection, messageId);
        }


        /// <summary>
        /// TRANS2_QUERY_FILE_INFORMATION Error Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        /// <param name="isRs2073Implemented">It indicates whether RS2073 is implemented.</param>
        /// Disable warning CA1801 isRs2073Implemented is used in Adapter.
        [Rule]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static void ErrorTrans2QueryFileInfoResponse(
            int messageId,
            MessageStatus messageStatus,
            bool isRs2073Implemented)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.Trans2QueryFileInformation);
            Condition.IsTrue(messageStatus == MessageStatus.NotSupported
                                || messageStatus == MessageStatus.InvalidParameter
                                || messageStatus == MessageStatus.NetworkSessionExpired);

            Trans2QueryFileInfoRequest request = (Trans2QueryFileInfoRequest)smbConnection.sentRequest[messageId];
            switch (messageStatus)
            {
                case MessageStatus.InvalidParameter:
                    if (!Parameter.isSupportStream
                            && request.informationLevel == InformationLevel.SmbQueryFileStreamInfo)
                    {
                        Condition.IsTrue(!Parameter.isSupportStream);

                        // If the SUT does not support stream is configured in ServerSetup stage, and adapter is responsible for 
                        // checking the configuration after sever setup. 
                        ModelHelper.CaptureRequirement(
                            2073,
                            "[In Application Requests Querying File Attributes] File Streams: If the FID field in the client request[SMB_COM_TRANSACTION2 request] " +
                            "is on an SMB share that does not support streams, then the server MUST fail the request with STATUS_INVALID_PARAMETER.");
                        ModelHelper.CaptureRequirement(
                            2076,
                            "[In Application Requests Querying File Attributes]If the FileName field in the client request[SMB_COM_TRANSACTION2 request] " +
                            "is on an SMB share that does not support streams, then the server MUST fail the request with STATUS_INVALID_PARAMETER.");
                    }
                    else
                    {
                        Condition.IsTrue(
                            (request.isUsePassthrough)
                                && (!smbConnection.sutCapabilities.Contains(Capabilities.CapInfoLevelPassThru)));
                    }
                    smbState = SmbState.End;
                    break;
                case MessageStatus.NotSupported:
                    if (request.isUsePassthrough)
                    {
                        Condition.IsTrue(smbConnection.sutCapabilities.Contains(Capabilities.CapInfoLevelPassThru));
                    }

                    if (request.informationLevel == InformationLevel.Invalid
                            && Parameter.isSupportInfoLevelPassThrough)
                    {
                        ModelHelper.CaptureRequirement(
                            4290,
                            @"[In Application Requests Querying File Attributes]Pass-through Information Levels:If an 
                            appropriate SMB Information Level is not available, then the server MUST fail the request 
                            with STATUS_NOT_SUPPORTED.");
                    }

                    smbState = SmbState.End;
                    break;
                case MessageStatus.NetworkSessionExpired:
                    smbState = SmbState.Closed;
                    break;
                default:
                    break;
            }
            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion

        #region TRANS2_QUERY_PATH_INFORMATION

        /// <summary>
        /// TRANS2_QUERY_PATH_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this request.</param>
        /// <param name="isUsePassthrough">
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="isReparse">
        /// It indicates whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of the SMB header.
        /// </param>
        /// <param name="informationLevel">The information level used for this request.</param>
        /// <param name="gmtTokenIndex">The index of the GMT token configured by CheckPreviousVersion action.</param>
        [Rule]
        public static void Trans2QueryPathInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isUsePassthrough,
            bool isReparse,
            [Domain("InfoLevelQueriedByPath")] InformationLevel informationLevel,
            int gmtTokenIndex)
        {
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);
            smbRequest = new Trans2QueryPathInfoRequest(
                messageId,
                sessionId,
                treeId,
                isSigned,
                gmtTokenIndex,
                isUsePassthrough,
                isReparse,
                informationLevel);
            Condition.IsTrue(smbConnection.sutCapabilities.Contains(Capabilities.CapNtSmbs));

            bool tokenExist = false;
            foreach (SmbFile file in smbConnection.openedFiles.Values)
            {
                foreach (int i in file.previousVersionToken)
                {
                    if (i == gmtTokenIndex)
                    {
                        tokenExist = true;
                        break;
                    }
                }
                if (tokenExist)
                {
                    break;
                }
            }

            Condition.IsTrue(tokenExist);
            if (informationLevel == InformationLevel.FileAccessInformation)
            {
                Condition.IsTrue(isUsePassthrough);
            }
            else
            {
                Condition.IsTrue(!isUsePassthrough);
            }
            if (gmtTokenIndex == -1)
            {
                if (!Parameter.isSupportInfoLevelPassThrough)
                {
                    Condition.IsTrue(!isUsePassthrough);
                }
            }
            Update.UpdateRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// TRANS2_QUERY_PATH_INFORMATION Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void Trans2QueryPathInfoResponse(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            MessageStatus messageStatus)
        {
            Trans2QueryPathInfoRequest request = null;
            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.Trans2QueryPathInfoRequest>(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                messageStatus,
                out request,
                MessageStatus.Success);
            Condition.IsTrue(request.command == Command.Trans2QueryPathInformation);

            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    9313,
                    "[In  response. Extensions]A server MUST send a TRANS2_QUERY_PATH_INFORMATION response in reply to " +
                    "an SMB_COM_TRANSACTION2 client request with a TRANS2_QUERY_PATH_INFORMATION subcommand when the " +
                    "request is successful, as specified in [MS-CIFS] section 2.2.6.6.2.");
            }

            Condition.IsTrue(request.gmtTokenIndex != -1);
            if (request.informationLevel == InformationLevel.SmbQueryFileStreamInfo)
            {
                Condition.IsTrue(Parameter.isSupportStream);
            }
            if (request.isUsePassthrough)
            {
                Condition.IsTrue(smbConnection.sutCapabilities.Contains(Capabilities.CapInfoLevelPassThru));

                ModelHelper.CaptureRequirement(
                    30029,
                    @"[In Receiving a TRANS2_QUERY_PATH_INFORMATION Request Pass-through Information Levels] If the 
                    client requests a pass-through Information Level, then the processing [Receiving a 
                    TRANS2_QUERY_PATH_INFORMATION Request] follows as specified in section 3.3.5.9.1.");
            }

            Update.UpdateResponse(smbConnection, messageId);
        }


        /// <summary>
        /// TRANS2_QUERY_PATH_INFORMATION Error Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        /// <param name="isRs2076Implemented">It indicates whether RS2076 is implemented.</param>
        [Rule]
        public static void ErrorTrans2QueryPathInfoResponse(
            int messageId,
            MessageStatus messageStatus,
            bool isRs2076Implemented)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.Trans2QueryPathInformation);
            Condition.IsTrue(messageStatus == MessageStatus.NotSupported
                                || messageStatus == MessageStatus.InvalidParameter
                                || messageStatus == MessageStatus.NetworkSessionExpired
                                || messageStatus == MessageStatus.ObjectNameNotFound);

            Trans2QueryPathInfoRequest request = (Trans2QueryPathInfoRequest)smbConnection.sentRequest[messageId];
            switch (messageStatus)
            {
                case MessageStatus.InvalidParameter:
                    Condition.IsTrue(request.gmtTokenIndex != -1);
                    if (request.informationLevel == InformationLevel.SmbQueryFileStreamInfo)
                    {
                        Condition.IsTrue(Parameter.isSupportStream);
                    }
                    Condition.IsTrue(
                        (request.isUsePassthrough)
                            && (!smbConnection.sutCapabilities.Contains(Capabilities.CapInfoLevelPassThru)));

                    // If the SUT supports stream is configured in ServerSetup stage, and adapter is responsible for 
                    // checking the configuration after sever setup.
                    // Based on the latest TD V20101101, the description for RS 2076 has benn changed as below:
                    // "If the FileName field in the client request is on an SMB share that does not support streams, 
                    // then the server MUST fail the request with STATUS_INVALID_PARAMETER.".
                    // Currently only change the capture logic according to MIP's rule.
                    if (!Parameter.isSupportStream)
                    {
                        if (isRs2076Implemented)
                        {
                            ModelHelper.CaptureRequirement(
                                2076,
                                "[In Application Requests Querying File Attributes]If the FileName field in the client " +
                                "request[SMB_COM_TRANSACTION2 request] is on an SMB share that does not support streams, " +
                                "then the server SHOULD<99> fail the request with STATUS_NOT_SUPPORTED.");
                        }
                    }
                    smbState = SmbState.End;
                    break;
                case MessageStatus.NotSupported:
                    Condition.IsTrue(request.informationLevel == InformationLevel.SmbQueryFileStreamInfo);
                    Condition.IsTrue(request.gmtTokenIndex != -1);
                    if (request.isUsePassthrough)
                    {
                        Condition.IsTrue(smbConnection.sutCapabilities.Contains(Capabilities.CapInfoLevelPassThru));
                    }
                    smbState = SmbState.End;
                    break;
                case MessageStatus.ObjectNameNotFound:
                    Condition.IsTrue(request.gmtTokenIndex == -1);
                    if (request.isUsePassthrough)
                    {
                        Condition.IsTrue(smbConnection.sutCapabilities.Contains(Capabilities.CapInfoLevelPassThru));
                    }
                    if (request.informationLevel == InformationLevel.SmbQueryFileStreamInfo)
                    {
                        Condition.IsTrue(Parameter.isSupportStream);
                    }
                    smbState = SmbState.End;
                    break;
                case MessageStatus.NetworkSessionExpired:
                    smbConnection.sessionList.Remove(request.sessionId);
                    smbState = SmbState.Closed;
                    break;
                default:
                    break;
            }
            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion

        #region TRANS2_SET_FILE_INFORMATION

        /// <summary>
        /// TRANS2_SET_FILE_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this request.</param>
        /// <param name="relaceEnable">
        /// It indicates whether the new name or link will replace the original one that exist already.
        /// </param>
        /// <param name="isUsePassthrough">
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="informationLevel">The information level used for this request.</param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        /// <param name="fileName">The new This is used to represent the name of the resource.</param>
        /// <param name="isRootDirecotyNull">It indicates whether root directory is null.</param>
        /// <param name="reserved">It is ignored by the SUT, used to be tested in traditional test.</param>
        [Rule]
        public static void Trans2SetFileInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool relaceEnable,
            bool isUsePassthrough,
            [Domain("InfoLevelSetByFid")] InformationLevel informationLevel,
            int fId,
            string fileName,
            bool isRootDirecotyNull,
            int reserved)
        {
            Condition.IsTrue(smbConnection.openedFiles.ContainsKey(fId) && smbConnection.openedFiles[fId].treeId == treeId);
            Condition.IsTrue(reserved == 0);
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);

            if ((informationLevel == InformationLevel.FileLinkInformation)
                || (informationLevel == InformationLevel.FileRenameInformation)
                || (informationLevel == InformationLevel.Invalid))
            {
                Condition.IsTrue(isUsePassthrough);
            }
            else
            {
                Condition.IsTrue(!isUsePassthrough);
            }

            if (informationLevel == InformationLevel.FileLinkInformation)
            {
                Condition.IsTrue(Parameter.fsType == FileSystemType.Ntfs);
            }

            //Parameter.fileNames[2] = "ExistTest.txt"
            if (relaceEnable &&
                fileName == Parameter.fileNames[2] &&
                informationLevel == InformationLevel.FileRenameInformation)
            {
                Requirement.AssumeCaptured("MS-SMB_R9588 will be captured in successful response.");
            }
            else if (!relaceEnable &&
                fileName == Parameter.fileNames[2] &&
                informationLevel == InformationLevel.FileRenameInformation)
            {
                Requirement.AssumeCaptured("MS-SMB_R9587 will be captured in error response with error code ObjectNameNotCollision.");
            }

            if (fileName == Parameter.fileNames[3] &&
                informationLevel == InformationLevel.FileRenameInformation)
            {
                Requirement.AssumeCaptured("MS-SMB_R30034 will be captured in error response with error code NotSupported.");
            }

            if (informationLevel == InformationLevel.Invalid)
            {
                Requirement.AssumeCaptured("MS-SMB_R30035 will be captured in error response with error code StatusOs2InvalidLevel.");
            }

            smbRequest = new Trans2SetFileInfoRequest(
                messageId,
                sessionId,
                treeId,
                isSigned,
                relaceEnable,
                isUsePassthrough,
                fId,
                fileName,
                informationLevel,
                isRootDirecotyNull);
            Update.UpdateRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// TRANS2_SET_FILE_INFORMATION Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void Trans2SetFileInfoResponse(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            MessageStatus messageStatus)
        {
            Trans2SetFileInfoRequest request = null;
            bool isNameExist = false;
            bool isReplaceDone = false;
            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.Trans2SetFileInfoRequest>(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                messageStatus,
                out request,
                MessageStatus.Success);
            Condition.IsTrue(request.command == Command.Trans2SetFileInformation);

            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    9323,
                    "[In response. Extensions]A server MUST send a TRANS2_SET_FILE_INFORMATION response in reply to an " +
                    "SMB_COM_TRANSACTION2 client request with a TRANS2_SET_FILE_INFORMATION subcommand when the request " +
                    "is successful, as specified in [MS-CIFS] section 2.2.6.9.2.");
            }

            if (request.informationLevel == InformationLevel.SmbQueryFileStreamInfo)
            {
                Condition.IsTrue(Parameter.isSupportStream);
            }
            if (request.isUsePassthrough)
            {
                Condition.IsTrue(smbConnection.sutCapabilities.Contains(Capabilities.CapInfoLevelPassThru));

                ModelHelper.CaptureRequirement(
                    30030,
                    "[In Receiving a TRANS2_SET_FILE_INFORMATION Request Pass-through Information Levels] If the " +
                    "client requests a pass-through Information Level, then the processing [Receiving a " +
                    "TRANS2_SET_FILE_INFORMATION Request] follows as specified in section 3.3.5.9.1.<126>");

                if (messageStatus == MessageStatus.Success)
                {
                    ModelHelper.CaptureRequirement(
                        30036,
                        "[In Receiving a TRANS2_SET_FILE_INFORMATION Request Pass-through Information Levels] " +
                        "<125> Section 3.3.5.9.6: Otherwise [if the server file system support this Information Level], " +
                        "it [the server] must attempt to apply the attributes to the target file and return the success or failure code in the response.");
                }

            }
            if (Parameter.sutPlatform != Platform.NonWindows)
            {
                Condition.IsTrue(request.informationLevel != InformationLevel.FileLinkInformation);
            }
            if (Parameter.fileNames.Contains(request.fileName))
            {
                isNameExist = true;
                Condition.IsTrue(request.replaceEnable);
                Condition.IsTrue(messageStatus == MessageStatus.Success);
                ModelHelper.CaptureRequirement(
                    9588,
                    "<78> Section 2.2.8.4: ReplaceIfExists (1 byte): If the target exists and ReplaceIfExists is TRUE, " +
                    "then the server should attempt to replace the target.");
                isReplaceDone = true;
            }

            Condition.IsTrue(!isNameExist || isReplaceDone);
            smbConnection.openedFiles[request.fId].name = request.fileName;
            Update.UpdateResponse(smbConnection, messageId);
        }


        /// <summary>
        /// TRANS2_SET_FILE_INFORMATION Error Response/
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        /// Disable warning CA1502 because according to Test Case design, excessive class complexity is necessary.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        public static void ErrorTrans2SetFileInfoResponse(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.Trans2SetFileInformation);
            Condition.IsTrue(messageStatus == MessageStatus.InvalidParameter
                                || messageStatus == MessageStatus.ObjectNameNotCollision
                                || messageStatus == MessageStatus.NetworkSessionExpired
                                || messageStatus == MessageStatus.NotSupported
                                || messageStatus == MessageStatus.StatusOs2InvalidLevel);

            Trans2SetFileInfoRequest request = (Trans2SetFileInfoRequest)smbConnection.sentRequest[messageId];
            bool isNameExist = false;
            if ((request.informationLevel == InformationLevel.FileLinkInformation)
                    || (request.informationLevel == InformationLevel.FileRenameInformation))
            {
                isNameExist = Parameter.fileNames.Contains(request.fileName);
            }

            switch (messageStatus)
            {
                case MessageStatus.StatusOs2InvalidLevel:
                    if (request.informationLevel == InformationLevel.Invalid)
                    {
                        //Invalid information Level is not supported by SUT.
                        ModelHelper.CaptureRequirement(
                            30035,
                            @"[In Receiving a TRANS2_SET_FILE_INFORMATION Request Pass-through Information Levels] <125> Section 3.3.5.9.6: " +
                            "If the server file system does not support this Information Level, then it must fail the request with STATUS_OS2_INVALID_LEVEL.");
                    }
                    break;
                case MessageStatus.NotSupported:
                    if (Parameter.sutPlatform != Platform.NonWindows)
                    {
                        //Windows-based servers do not support setting the following NT Information Levels via the pass-through Information Level mechanism.
                        //FileLinkInformation STATUS_NOT_SUPPORTED
                        Condition.IsTrue((request.informationLevel == InformationLevel.FileRenameInformation ||
                                      request.informationLevel == InformationLevel.FileLinkInformation) &&
                                      Parameter.isSupportInfoLevelPassThrough);
                    }

                    if (request.informationLevel != InformationLevel.SmbSetFileBasicInfo
                            && Parameter.isSupportInfoLevelPassThrough)
                    {
                        ModelHelper.CaptureRequirement(
                            8430,
                            @"[In Application Requests Setting File Attributes] Pass-Through Information Levels: If an 
                            appropriate SMB Information Level is not available, then the server MUST fail the request 
                            with STATUS_NOT_SUPPORTED. ");
                    }

                    // File name is @"Dir1\Test1.txt"
                    if (request.fileName == Parameter.fileNames[3] && request.informationLevel == InformationLevel.FileRenameInformation)
                    {
                        //Parameter.fileNames[3] = @"Dir1\Test1.txt", which contains a separator character
                        ModelHelper.CaptureRequirement(
                            30034,
                            "[In Receiving a TRANS2_SET_FILE_INFORMATION Request Pass-through Information Levels] " +
                            "<125> Section 3.3.5.9.6: If the requested Information Class is FileRenameInformation, then the following validation is performed: " +
                            "If the file name pointed to by the FileName parameter of the FILE_RENAME_INFORMATION structure contains a separator character, " +
                            "then the request should be failed with STATUS_NOT_SUPPORTED.");
                    }
                    smbState = SmbState.End;
                    break;
                case MessageStatus.InvalidParameter:
                    if ((request.informationLevel == InformationLevel.FileRenameInformation)
                            || (request.informationLevel == InformationLevel.FileLinkInformation))
                    {
                        if (request.informationLevel == InformationLevel.FileLinkInformation)
                        {
                            Condition.IsTrue(Parameter.sutPlatform == Platform.NonWindows);
                        }
                        Condition.IsTrue((isNameExist && request.replaceEnable) || (!isNameExist));
                    }
                    Console.WriteLine(request.isRootDirectory);
                    if (!request.isRootDirectory
                            && request.informationLevel == InformationLevel.FileRenameInformation)
                    {
                        ModelHelper.CaptureRequirement(
                            30033,
                            "[In Receiving a TRANS2_SET_FILE_INFORMATION Request Pass-through Information Levels] <126> " +
                            "Section 3.3.5.9.6: If the requested Information Class is FileRenameInformation, then the " +
                            "following validation is performed: If RootDirectory is not NULL, then the request must be " +
                            "failed with STATUS_INVALID_PARAMETER.");
                    }
                    smbState = SmbState.End;
                    break;
                case MessageStatus.ObjectNameNotCollision:
                    if (isNameExist && !request.replaceEnable)
                    {
                        ModelHelper.CaptureRequirement(
                            9587,
                            "<78> Section 2.2.8.4: ReplaceIfExists (1 byte): If a target exists and ReplaceIfExists is " +
                            "FALSE, then the operation must be failed with STATUS_OBJECT_NAME_COLLISION.");
                    }
                    smbState = SmbState.End;
                    break;
                case MessageStatus.NetworkSessionExpired:
                    smbConnection.sessionList.Remove(request.sessionId);
                    smbState = SmbState.Closed;
                    break;
                default:
                    break;
            }
            smbConnection.sentRequest.Remove(messageId);
        }
        #endregion

        #region TRANS2_SET_PATH_INFORMATION

        /// <summary>
        /// TRANS2_SET_PATH_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this request.</param>
        /// <param name="isUsePassthrough">
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="isReparse">
        /// It indicates whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of the SMB header.
        /// </param>
        /// <param name="informationLevel">The information level used for this request.</param>
        /// <param name="gmtTokenIndex">The index of the GMT token configured by CheckPreviousVersion action.</param>
        [Rule]
        public static void Trans2SetPathInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isUsePassthrough,
            bool isReparse,
            [Domain("InfoLevelSetByPath")] InformationLevel informationLevel,
            int gmtTokenIndex)
        {
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);
            smbRequest = new Trans2SetPathInfoRequest(
                messageId,
                sessionId,
                treeId,
                isSigned,
                isUsePassthrough,
                isReparse,
                gmtTokenIndex,
                informationLevel);

            bool tokenExist = false;
            foreach (SmbFile file in smbConnection.openedFiles.Values)
            {
                foreach (int i in file.previousVersionToken)
                {
                    if (i == gmtTokenIndex)
                    {
                        tokenExist = true;
                        break;
                    }
                }
                if (tokenExist)
                {
                    break;
                }
            }

            Condition.IsTrue(tokenExist);
            if (informationLevel == InformationLevel.FileAllocationInformation)
            {
                Condition.IsTrue(isUsePassthrough);
            }
            else
            {
                Condition.IsTrue(!isUsePassthrough);
            }
            if (gmtTokenIndex == -1)
            {
                if (!Parameter.isSupportInfoLevelPassThrough)
                {
                    Condition.IsTrue(!isUsePassthrough);
                }
            }
            Update.UpdateRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// TRANS2_SET_PATH_INFORMATION Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void Trans2SetPathInfoResponse(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            MessageStatus messageStatus)
        {
            Trans2SetPathInfoRequest request = null;
            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.Trans2SetPathInfoRequest>(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                messageStatus,
                out request,
                MessageStatus.Success);
            Condition.IsTrue(request.command == Command.Trans2SetPathInforamtion);

            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    9316,
                    "[In  response. Extensions]A server MUST send a TRANS2_SET_PATH_INFORMATION response in reply to " +
                    "an SMB_COM_TRANSACTION2 (section 2.2.4.4) client request with a TRANS2_SET_PATH_INFORMATION " +
                    "subcommand when the request is successful,as specified in [MS-CIFS] section 2.2.6.7.2.");
            }

            Condition.IsTrue(request.gmtTokenIndex != -1);
            if (request.isUsePassthrough)
            {
                Condition.IsTrue(smbConnection.sutCapabilities.Contains(Capabilities.CapInfoLevelPassThru));

                ModelHelper.CaptureRequirement(
                    30031,
                    "[In Receiving a TRANS2_SET_PATH_INFORMATION Request Pass-through Information Levels] If the client " +
                    "requests a pass-through Information Level, then the processing [Receiving a " +
                    "TRANS2_SET_PATH_INFORMATION Request] follows as specified in section 3.3.5.9.1.<127>");
            }
            Update.UpdateResponse(smbConnection, messageId);
        }


        /// <summary>
        /// TRANS2_SET_PATH_INFORMATION Error Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        public static void ErrorTrans2SetPathInfoResponse(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.Trans2SetPathInforamtion);
            Condition.IsTrue(messageStatus == MessageStatus.InvalidParameter
                                || messageStatus == MessageStatus.NetworkSessionExpired
                                || messageStatus == MessageStatus.ObjectNameNotFound
                                || messageStatus == MessageStatus.NotSupported);

            Trans2SetPathInfoRequest request = (Trans2SetPathInfoRequest)smbConnection.sentRequest[messageId];
            switch (messageStatus)
            {
                case MessageStatus.InvalidParameter:
                    Condition.IsTrue(request.gmtTokenIndex != -1);
                    Condition.IsTrue(
                        (request.isUsePassthrough)
                            && (!smbConnection.sutCapabilities.Contains(Capabilities.CapInfoLevelPassThru)));
                    smbState = SmbState.End;
                    break;
                case MessageStatus.ObjectNameNotFound:
                    if (request.isUsePassthrough)
                    {
                        Condition.IsTrue(smbConnection.sutCapabilities.Contains(Capabilities.CapInfoLevelPassThru));
                    }
                    Condition.IsTrue(request.gmtTokenIndex == -1);
                    smbState = SmbState.End;
                    break;
                case MessageStatus.NetworkSessionExpired:
                    smbConnection.sessionList.Remove(request.sessionId);
                    smbState = SmbState.Closed;
                    break;
                case MessageStatus.NotSupported:
                    Condition.IsTrue(request.gmtTokenIndex == -1);
                    smbState = SmbState.End;
                    break;
                default:
                    break;
            }
            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion

        #region TRANS2_QUERY_FS_INFORMATION

        /// <summary>
        /// TRANS2_QUERY_FS_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this request.</param>
        /// <param name="isUsePassthrough">
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="informationLevel">The information level used for this request.</param>
        /// <param name="otherBits">The bits not listed in section.</param>
        /// <param name="reserved">It MUST be ignored upon receipt of the message.</param>
        [Rule]
        public static void Trans2QueryFsInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isUsePassthrough,
            [Domain("InfoLevelQueriedByFs")]  InformationLevel informationLevel,
            bool otherBits,
            int reserved)
        {
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);
            Condition.IsTrue(smbConnection.treeConnectList[treeId].smbShare.shareType == ShareType.Disk);
            Condition.IsTrue(!otherBits);
            Condition.IsTrue(reserved == 0);
            smbRequest = new Trans2QueryFSInfoRequest(
                messageId,
                sessionId,
                treeId,
                isSigned,
                isUsePassthrough,
                informationLevel);
            if (informationLevel == InformationLevel.FileFsSizeInformation)
            {
                Condition.IsTrue(isUsePassthrough);
            }
            else
            {
                Condition.IsTrue(!isUsePassthrough);
            }

            if (informationLevel == InformationLevel.SmbQueryFsAttributeInfo)
            {
                Requirement.AssumeCaptured("Adapter Capture for SMB_QUERY_FS_ATTRIBUTE_INFO imformation level");
            }
            Update.UpdateRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// TRANS2_QUERY_FS_INFORMATION Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void Trans2QueryFsInfoResponse(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            MessageStatus messageStatus)
        {
            Trans2QueryFSInfoRequest request = null;
            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.Trans2QueryFSInfoRequest>(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                messageStatus,
                out request,
                MessageStatus.Success);
            Condition.IsTrue(request.command == Command.Trans2QueryFsInformation);

            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    9291,
                    "[In response. Extensions]A server MUST send a TRANS2_QUERY_FS_INFORMATION response in reply to an " +
                    "SMB_COM_TRANSACTION2 client request with a TRANS2_QUERY_FS_INFORMATION subcommand when the request " +
                    "is successful, as specified in [MS-CIFS] section 2.2.6.4.2.");
            }

            if (request.isUsePassthrough)
            {
                Condition.IsTrue(smbConnection.sutCapabilities.Contains(Capabilities.CapInfoLevelPassThru));
                ModelHelper.CaptureRequirement(
                    30032,
                    @"[In Receiving a TRANS2_QUERY_FS_INFORMATION Request Pass-through Information Levels] If the client
                    requests a pass-through Information Level, then the processing [Receiving a 
                    TRANS2_QUERY_FS_INFORMATION Request] follows as specified in section 3.3.5.9.1.");
            }
            Update.UpdateResponse(smbConnection, messageId);
        }


        /// <summary>
        /// TRANS2_QUERY_FS_INFORMATION Error Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        public static void ErrorTrans2QueryFsInfoResponse(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.Trans2QueryFsInformation);
            Condition.IsTrue(messageStatus == MessageStatus.InvalidParameter
                                || messageStatus == MessageStatus.NetworkSessionExpired
                                || messageStatus == MessageStatus.NotSupported);

            Trans2QueryFSInfoRequest request = (Trans2QueryFSInfoRequest)smbConnection.sentRequest[messageId];
            switch (messageStatus)
            {
                case MessageStatus.InvalidParameter:
                    Condition.IsTrue(
                        (request.isUsePassthrough)
                            && (!smbConnection.sutCapabilities.Contains(Capabilities.CapInfoLevelPassThru)));
                    smbState = SmbState.End;
                    break;
                case MessageStatus.NetworkSessionExpired:
                    smbConnection.sessionList.Remove(request.sessionId);
                    smbState = SmbState.Closed;
                    break;
                case MessageStatus.NotSupported:
                    if (request.informationLevel == InformationLevel.Invalid)
                    {
                        ModelHelper.CaptureRequirement(
                            8438,
                            @"[In Application Requests Querying File System Attributes,The processing of this event is
                            handled as specified in [MS-CIFS] section 3.2.4.25, with the following additions:] If an
                            appropriate SMB Information Level is not available, then the server MUST fail the request
                            with STATUS_NOT_SUPPORTED. ");
                    }
                    break;
                default:
                    break;
            }
            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion

        #region TRANS2_SET_FS_INFORMATION (Additional)

        /// <summary>
        /// TRANS2_SET_FS_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this request.</param>
        /// <param name="fId">It indicates file id</param>
        /// <param name="informationLevel">
        /// It indicates that valid condition and each error condition are contained in the input conditions type.
        /// </param>
        /// <param name="requestPara">TRANS2_SET_FS_INFORMATION request parameter.</param>
        [Rule]
        public static void Trans2SetFsInfoRequestAdditional(
            int messageId,
            int sessionId,
            int treeId,
            int fId,
            bool isSigned,
            [Domain("InfoLevelSetByFsAdditional")] InformationLevel informationLevel,
            Trans2SetFsInfoResponseParameter requestPara)
        {
            Condition.IsTrue(smbConnection.openedFiles.ContainsKey(fId));
            Condition.IsTrue(smbConnection.openedFiles[fId].treeId == treeId);
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);

            if (createFileAccess == 1)
            {
                Condition.IsTrue(requestPara == Trans2SetFsInfoResponseParameter.AccessDenied);
                Condition.IsTrue(informationLevel != InformationLevel.Invalid);
            }
            else
            {
                Condition.IsTrue(requestPara != Trans2SetFsInfoResponseParameter.AccessDenied);
            }


            smbRequest = new Trans2SetFSInfoRequestAdditional(messageId, sessionId, treeId, fId,
                isSigned, informationLevel, requestPara);

            Update.UpdateRequest(smbConnection, smbRequest);
        }

        /// <summary>
        /// TRANS2_SET_FS_INFORMATION Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void Trans2SetFsInfoResponseAdditional(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            MessageStatus messageStatus)
        {
            Trans2SetFSInfoRequestAdditional request;
            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.Trans2SetFSInfoRequestAdditional>(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                messageStatus,
                out request,
                MessageStatus.Success);
            Condition.IsTrue(request.command == Command.Trans2SetFsInforamtionAdditional);
            Condition.IsTrue(request.requestPara == Trans2SetFsInfoResponseParameter.Valid);

            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    9307,
                    @"[In server response]A server MUST send a TRANS2_SET_FS_INFORMATION response in reply to a client
                    TRANS2_SET_FS_INFORMATION subcommand request when the request is successful.");
            }

            Condition.IsTrue(request.treeId == treeId);
            if (request.isUsePassthrough)
            {
                Condition.IsTrue(smbConnection.sutCapabilities.Contains(Capabilities.CapInfoLevelPassThru));
            }
            Update.UpdateResponse(smbConnection, messageId);
        }


        /// <summary>
        /// TRANS2_SET_FS_INFORMATION Error Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void ErrorTrans2SetFsInfoResponseAdditional(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.Trans2SetFsInforamtionAdditional);

            Condition.IsTrue(messageStatus == MessageStatus.AccessDenied
                                || messageStatus == MessageStatus.StatusInvalidHandle
                                || messageStatus == MessageStatus.StatusInsuffServerResources
                                || messageStatus == MessageStatus.StatusDataError
                                || messageStatus == MessageStatus.NetworkSessionExpired
                                || messageStatus == MessageStatus.InvalidParameter
                                || messageStatus == MessageStatus.NotSupported);
            Trans2SetFSInfoRequestAdditional request
                = (Trans2SetFSInfoRequestAdditional)smbConnection.sentRequest[messageId];

            if (messageStatus == MessageStatus.AccessDenied)
            {
                Condition.IsTrue(request.requestPara == Trans2SetFsInfoResponseParameter.AccessDenied);
                ModelHelper.CaptureRequirement(
                    119311,
                    @"[In server response]When SMBSTATUS code is negotiated and Access denied,the server returns 
                    STATUS_ACCESS_DENIED(0xC0000022).");
            }
            else if (messageStatus == MessageStatus.StatusInvalidHandle)
            {
                Condition.IsTrue(request.requestPara == Trans2SetFsInfoResponseParameter.FileIdErrror);
                ModelHelper.CaptureRequirement(
                    119314,
                    @"[In server response]When SMBSTATUS code is negotiated and the Fid supplied is invalid,the server
                    returns STATUS_INVALID_HANDLE(0xC0000008).");
            }
            else if (messageStatus == MessageStatus.StatusInsuffServerResources)
            {
                Condition.IsTrue(request.requestPara == Trans2SetFsInfoResponseParameter.ServerOutOfResource);
                ModelHelper.CaptureRequirement(
                    119318,
                    @"[In server response]When SMBSTATUS code is negotiated and the server is out of resources,the 
                    server returns STATUS_INSUFF_SERVER_RESOURCES(0xC0000205).");
            }
            else if (messageStatus == MessageStatus.StatusDataError)
            {
                ModelHelper.CaptureRequirement(
                    119333,
                    @"[In server response]When SMBSTATUS code is negotiated and Disk I/O error,the server returns
                    STATUS_DATA_ERROR(0xC000003E).");
            }
            else if (messageStatus == MessageStatus.NotSupported)
            {
                Condition.IsTrue(request.informationLevel == InformationLevel.Invalid);
                Condition.IsTrue(request.requestPara == Trans2SetFsInfoResponseParameter.Valid);
                ModelHelper.CaptureRequirement(
                    119320,
                    @"[In Server Response]When SMBSTATUS code is negotiated and The supplied Information Level, 
                    as indicated by the Trans2_Parameters.InformationLevel value, is not a valid pass-through Information Level,
                    the server returns SMBSTATUS class/code pair ERRDOS(0x01)/ERRunsup(0x0032).");
            }
            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion

        #region TRANS2_SET_FS_INFORMATION

        /// <summary>
        /// TRANS2_SET_FS_INFORMATION Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this request.</param>
        /// <param name="isUsePassthrough">
        /// It indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field of the request.
        /// </param>
        /// <param name="informationLevel">The information level used for this request.</param>
        /// <param name="isRequireDisconnectTreeFlags">
        /// It indicates whether the DISCONNECT_TID bit of Flags is set in this request.
        /// </param>
        /// <param name="isRequireNoResponseFlags">
        /// It indicates whether the NO_RESPONSE bit of Flags is set in this request.
        /// </param>
        /// <param name="fId">The SMB file identifier of the target directory.</param>
        /// <param name="otherBits">The bits not listed in section.</param>
        /// <param name="reserved">It MUST be ignored upon receipt of the message.</param>
        [Rule]
        public static void Trans2SetFsInfoRequest(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isUsePassthrough,
            bool isRequireDisconnectTreeFlags,
            bool isRequireNoResponseFlags,
            [Domain("InfoLevelSetByFs")] InformationLevel informationLevel,
            int fId,
            bool otherBits,
            int reserved)
        {
            Condition.IsTrue(smbConnection.openedFiles.ContainsKey(fId));
            Condition.IsTrue(smbConnection.openedFiles[fId].treeId == treeId);
            Condition.IsTrue(isUsePassthrough);
            Condition.IsTrue(otherBits);
            Condition.IsTrue(reserved == 0);
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);
            if (Parameter.clientPlatform != Platform.NonWindows)
            {
                Condition.IsTrue(!isRequireNoResponseFlags && !isRequireDisconnectTreeFlags);
            }
            smbRequest = new Trans2SetFSInfoRequest(
                messageId,
                sessionId,
                treeId,
                isUsePassthrough,
                isRequireDisconnectTreeFlags,
                isRequireNoResponseFlags,
                isSigned,
                informationLevel);
            Update.UpdateRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// TRANS2_SET_FS_INFORMATION Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void Trans2SetFsInfoResponse(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            MessageStatus messageStatus)
        {
            Trans2SetFSInfoRequest request;
            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.Trans2SetFSInfoRequest>(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                messageStatus,
                out request,
                MessageStatus.Success);
            Condition.IsTrue(request.command == Command.Trans2SetFsInformaiton);

            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    9307,
                    @"[In server response]A server MUST send a TRANS2_SET_FS_INFORMATION response in reply to a client
                    TRANS2_SET_FS_INFORMATION subcommand request when the request is successful.");
            }

            Condition.IsTrue(request.treeId == treeId);
            Condition.IsTrue(!request.requireNoResponseFlags);
            if (request.isUsePassthrough)
            {
                Condition.IsTrue(smbConnection.sutCapabilities.Contains(Capabilities.CapInfoLevelPassThru));
                ModelHelper.CaptureRequirement(
                    30026,
                    @"[In Receiving a TRANS2_SET_FS_INFORMATION Request] If the client requests a pass-through 
                    Information Level, then processing [Receiving a TRANS2_SET_FS_INFORMATION Request] follows as 
                    specified in section 3.3.5.9.1.");
            }
            if (request.requireDisconnectTreeFlags)
            {
                smbConnection.treeConnectList.Remove(treeId);
            }
            Update.UpdateResponse(smbConnection, messageId);
        }


        /// <summary>
        /// TRANS2_SET_FS_INFORMATION Error Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        public static void ErrorTrans2SetFsInfoResponse(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.Trans2SetFsInformaiton);
            Condition.IsTrue(messageStatus == MessageStatus.InvalidParameter
                                || messageStatus == MessageStatus.NetworkSessionExpired
                                || messageStatus == MessageStatus.AccessDenied);

            Trans2SetFSInfoRequest request = (Trans2SetFSInfoRequest)smbConnection.sentRequest[messageId];
            switch (messageStatus)
            {
                case MessageStatus.InvalidParameter:
                    Condition.IsTrue(
                        (request.isUsePassthrough)
                            && (!smbConnection.sutCapabilities.Contains(Capabilities.CapInfoLevelPassThru)));
                    smbState = SmbState.End;
                    break;
                case MessageStatus.NetworkSessionExpired:
                    smbConnection.sessionList.Remove(request.sessionId);
                    smbState = SmbState.Closed;
                    break;
                case MessageStatus.AccessDenied:
                    Condition.IsTrue(Parameter.sutPlatform != Platform.NonWindows
                                        || Parameter.clientPlatform != Platform.NonWindows);
                    break;
                case MessageStatus.NotSupported:
                    if (!smbConnection.sutCapabilities.Contains(Capabilities.CapInfoLevelPassThru))
                    {
                        ModelHelper.CaptureRequirement(
                            8440,
                            @"[In Application Requests Setting File System Attributes] If the CAP_INFOLEVEL_PASSTHRU
                            flag in Client.Connection.ServerCapabilities is not set, then the client MUST fail the 
                            request with STATUS_NOT_SUPPORTED.");
                    }
                    break;
                default:
                    break;
            }
            smbConnection.sentRequest.Remove(messageId);
        }
        #endregion

        #region TRANS2_FIND_FIRST2

        /// <summary>
        /// TRANS2_FIND_FIRST2 Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this request.</param>
        /// <param name="isReparse">
        /// It indicates whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of the SMB header.
        /// </param>
        /// <param name="informationLevel">The information level used for this request.</param>
        /// <param name="gmtTokenIndex">The index of the GMT token configured by CheckPreviousVersion action.</param>
        /// <param name="isFlagsKnowsLongNameSet">
        /// It indicates the adpater to set the SMB_FLAGS2_KNOWS_LONG_NAMES flag in SMB header or not.
        /// </param>
        [Rule]
        public static void Trans2FindFirst2Request(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isReparse,
            [Domain("InfoLevelByFind")] InformationLevel informationLevel,
            int gmtTokenIndex,
            bool isFlagsKnowsLongNameSet,
            bool isGMTPatten)
        {
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);
            smbRequest = new Trans2FindFirst2Request(
                messageId,
                sessionId,
                treeId,
                gmtTokenIndex,
                isReparse,
                informationLevel,
                isFlagsKnowsLongNameSet,
                isGMTPatten);

            bool tokenExist = false;
            foreach (SmbFile file in smbConnection.openedFiles.Values)
            {
                foreach (int i in file.previousVersionToken)
                {
                    if (i == gmtTokenIndex)
                    {
                        tokenExist = true;
                        Condition.IsTrue(gmtTokenIndex == i);
                        break;
                    }
                }
                if (tokenExist)
                {
                    break;
                }
            }
            Condition.IsTrue(tokenExist);
            Update.UpdateRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// TRANS2_FIND_FIRST2 Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="isFileIdEqualZero">It indicates whether the file ID equals 0.</param>
        /// <param name="searchHandlerId">Search Handler.</param>
        /// <param name="isReturnEnumPreviousVersion">
        /// It indicates whether the SUT will return the previous version enumeration.
        /// </param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        /// <param name="isRs2398Implemented">It indicates whether RS2398 is implemented.</param>
        /// <param name="isRs4899Implemented">It indicates whether RS4899 is implemented.</param>
        [Rule]
        /// Disable warning CA1502, because there are 30 error situations in this command according to the technical 
        /// document, the cyclomatic complexity cannot be reduced.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public static void Trans2FindFirst2Response(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isFileIdEqualZero,
            int searchHandlerId,
            bool isReturnEnumPreviousVersion,
            MessageStatus messageStatus,
            bool isRs2398Implemented,
            bool isRs4899Implemented)
        {
            Trans2FindFirst2Request request;
            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.Trans2FindFirst2Request>(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                messageStatus,
                out request,
                MessageStatus.Success);
            Condition.IsTrue(request.command == Command.TransFindFirst2);
            Condition.IsTrue(request.treeId == treeId);
            Condition.IsTrue(request.gmtTokenIndex != -1);

            // [Flags2 (2 bytes): SMB_FLAGS2_KNOWS_LONG_NAMES]If a client does not support long names 
            // (i.e. SMB2_FLAGS2_KNOWS_LONG_NAMES is not set), then any TRANS_FIND_FIRST2/FIND_NEXT2 request with an 
            // information level other than SMB_INFO_STANDARD MUST be failed with STATUS_INVALID_PARAMETER.
            if (request.informationLevel != InformationLevel.SmbInfoStandard)
            {
                Condition.IsTrue(request.isFlagsKnowsLongNameSet);
            }

            if (request.informationLevel == InformationLevel.SmbFindFileIdFullDirectoryInfo
                    || request.informationLevel == InformationLevel.SmbFindFileIdBothDirectoryInfo)
            {
                ModelHelper.CaptureRequirement(
                    30038,
                    @"[InReceiving a TRANS2_FIND_FIRST2 Request New Information Levels] <123> Section 3.3.5.9.2:
                    Windows servers support these new Information Levels for directory queries.");
            }
            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    9281,
                    "[In  response. Extensions]A server MUST send a TRANS2_FIND_FIRST2 response in reply to a client " +
                    "TRANS2_FIND_FIRST2 subcommand request when the request is successful, as specified in [MS-CIFS] " +
                    "section 2.2.6.2.2.");
            }

            SmbFile file = new SmbFile();
            foreach (SmbFile searchFile in smbConnection.openedFiles.Values)
            {
                foreach (int i in searchFile.previousVersionToken)
                {
                    if (request.gmtTokenIndex == i)
                    {
                        file = searchFile;
                        Condition.IsTrue(searchHandlerId == i);
                        file.searchHandlerList.Add(searchHandlerId);
                        smbConnection.searchHandlerContainer.Add(searchHandlerId, request.gmtTokenIndex);
                        break;
                    }
                }
            }
            if (request.informationLevel == InformationLevel.SmbFindFileBothDirectoryInfo && request.isGmtPatten)
            {
                if (Parameter.sutPlatform != Platform.NonWindows
                        && Parameter.sutPlatform != Platform.Win2K3
                        && Parameter.sutPlatform != Platform.Win2K3R2
                        && Parameter.sutPlatform != Platform.Win2K8
                        && Parameter.sutPlatform != Platform.Win2K8R2)
                {
                    if (isRs4899Implemented && !isReturnEnumPreviousVersion)
                    {
                        // Though it doesn't return enumeration of the previous version, the operation also succeeds, 
                        // and we can determine the SUT process as a normal FindFirst operation.
                        ModelHelper.CaptureRequirement(
                            4899,
                            "[In Receiving a TRANS2_FIND_FIRST2 Request Enumerating the previous versions]If a scan for " +
                            "the previous version tokens (section 3.3.5.1.1) reveals that the FileName of the " +
                            "TRANS_FIND_FIRST2 request contains the search pattern @GMT-* and the requested Information " +
                            "Level is SMB_FIND_FILE_BOTH_DIRECTORY_INFO, then the server MAY<125> choose to return an " +
                            "enumeration of the previous versions that are valid for the share.");
                    }
                }
            }
            else
            {
                if (!isReturnEnumPreviousVersion)
                {
                    ModelHelper.CaptureRequirement(
                        2402,
                        "[In Receiving a TRANS2_FIND_FIRST2 Request Enumerating the previous versions]If the server " +
                        "chooses not to do this [return an enumeration of the previous versions valid for the share], " +
                        "then the enumeration MUST be processed as a normal TRANS2_FIND_FIRST2 operation.");
                }
            }

            switch (request.informationLevel)
            {
                case InformationLevel.SmbFindFileIdFullDirectoryInfo:
                    // Parameter.isSupportUniqueFileId is configured in ServerSetup action, and the SUT is configured 
                    // according to this.
                    if (!Parameter.isSupportUniqueFileId)
                    {
                        Condition.IsTrue(isFileIdEqualZero);
                    }
                    if (Parameter.sutPlatform == Platform.WinNt
                            || Parameter.sutPlatform == Platform.Win2K
                            || Parameter.sutPlatform == Platform.Win2K3
                            || Parameter.sutPlatform == Platform.Win2K8
                            || Parameter.sutPlatform == Platform.Win2K8R2)
                    {
                        if (isRs2398Implemented)
                        {
                            ModelHelper.CaptureRequirement(
                                2398,
                                @"[In Receiving a TRANS2_FIND_FIRST2 Request New Information Levels]The server SHOULD 
                                allow for the new Information Levels, as specified in section 2.2.2.3.1.");
                        }
                    }
                    break;
                case InformationLevel.SmbFindFileIdBothDirectoryInfo:
                    // Parameter.isSupportUniqueFileId is configured in ServerSetup action, and the SUT is configured 
                    // according to this.
                    if (!Parameter.isSupportUniqueFileId)
                    {
                        Condition.IsTrue(isFileIdEqualZero);
                    }
                    if (Parameter.sutPlatform == Platform.WinNt
                        || Parameter.sutPlatform == Platform.Win2K
                        || Parameter.sutPlatform == Platform.Win2K3
                        || Parameter.sutPlatform == Platform.Win2K8
                        || Parameter.sutPlatform == Platform.Win2K8R2)
                    {
                        if (isRs2398Implemented)
                        {
                            ModelHelper.CaptureRequirement(
                                2398,
                                @"[In Receiving a TRANS2_FIND_FIRST2 Request New Information Levels]The server SHOULD 
                                allow for the new Information Levels, as specified in section 2.2.2.3.1.");

                            if (Parameter.sutPlatform != Platform.NonWindows)
                            {
                                ModelHelper.CaptureRequirement(
                                    102398,
                                    @"[In Receiving a TRANS2_FIND_FIRST2 Request New Information Levels]The server 
                                    allows for the new Information Levels, as specified in section 2.2.2.3.1 in
                                    Windows.");
                            }
                        }
                    }
                    Requirement.AssumeCaptured(
                        "FileIndex (4 bytes): This field MUST be set to 0 when sending a response");
                    break;
                case InformationLevel.SmbFindFileBothDirectoryInfo:
                    break;
                default:
                    break;
            }
            Update.UpdateResponse(smbConnection, messageId);
        }


        /// <summary>
        /// TRANS2_FIND_FIRST2 Error Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        public static void ErrorFindFirst2Response(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(messageStatus == MessageStatus.NetworkSessionExpired
                                || messageStatus == MessageStatus.NotSupported
                                || messageStatus == MessageStatus.ObjectNameNotFound
                                || messageStatus == MessageStatus.InvalidParameter);

            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.TransFindFirst2);
            Trans2FindFirst2Request request = (Trans2FindFirst2Request)smbConnection.sentRequest[messageId];
            switch (messageStatus)
            {
                case MessageStatus.NotSupported:
                    Condition.IsTrue(Parameter.sutPlatform == Platform.NonWindows);
                    Condition.IsTrue(request.gmtTokenIndex != -1);

                    if (request.informationLevel == InformationLevel.SmbFindFileIdBothDirectoryInfo
                            || request.informationLevel == InformationLevel.SmbFindFileIdFullDirectoryInfo)
                    {
                        ModelHelper.CaptureRequirement(
                            4898,
                            "[In Receiving a TRANS2_FIND_FIRST2 Request New Information Levels]If the server does not " +
                            "support the new Information Levels, then it MUST fail the operation with " +
                            "STATUS_NOT_SUPPORTED.<124>");
                    }
                    smbState = SmbState.End;
                    break;
                case MessageStatus.ObjectNameNotFound:
                    if (request.informationLevel == InformationLevel.SmbFindFileIdBothDirectoryInfo
                            || request.informationLevel == InformationLevel.SmbFindFileIdFullDirectoryInfo)
                    {
                        Condition.IsTrue(Parameter.sutPlatform != Platform.NonWindows);
                    }
                    Condition.IsTrue(request.gmtTokenIndex == -1);
                    Condition.IsTrue(request.isFlagsKnowsLongNameSet);
                    smbState = SmbState.End;
                    break;
                case MessageStatus.NetworkSessionExpired:
                    smbConnection.sessionList.Remove(request.sessionId);
                    smbState = SmbState.Closed;
                    break;
                case MessageStatus.InvalidParameter:
                    if (!request.isFlagsKnowsLongNameSet
                            && request.informationLevel != InformationLevel.SmbInfoStandard)
                    {
                        ModelHelper.CaptureRequirement(
                            9280,
                            @"[In Client Request Extensions] InformationLevel (2 bytes): If a client that has not 
                            negotiated long names support requests an InformationLevel other than SMB_INFO_STANDARD,
                            then the server MUST return a status of STATUS_INVALID_PARAMETER(ERRDOS/ERRinvalidparam).");
                    }
                    smbState = SmbState.End;
                    break;
                default:
                    break;
            }
            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion

        #region TRANS2_FIND_NEXT2

        /// <summary>
        /// TRANS2_FIND_NEXT2 Request.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this response.</param>
        /// <param name="isReparse">
        /// It indicates whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of the SMB header.
        /// </param>
        /// <param name="informationLevel">The information level used for this request.</param>
        /// <param name="searchHandlerId">
        /// Search Handler: SID is the value returned by a previous successful TRANS2_FIND_FIRST2 call.
        /// </param>
        /// <param name="gmtTokenIndex">The index of the GMT token configured by CheckPreviousVersion action.</param>
        /// <param name="isFlagsKnowsLongNameSet">
        /// It indicates the adpater to set the SMB_FLAGS2_KNOWS_LONG_NAMES flag in SMB header or not.
        /// </param>
        [Rule]
        public static void Trans2FindNext2Request(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isReparse,
            [Domain("InfoLevelByFind")] InformationLevel informationLevel,
            int searchHandlerId,
            int gmtTokenIndex,
            bool isFlagsKnowsLongNameSet)
        {

            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);
            smbRequest = new Trans2FindNext2Request(
                messageId,
                sessionId,
                treeId,
                searchHandlerId,
                gmtTokenIndex,
                isReparse,
                informationLevel,
                isFlagsKnowsLongNameSet);

            bool isSearchHandlerExist = false;
            foreach (SmbFile searchFile in smbConnection.openedFiles.Values)
            {
                foreach (int i in searchFile.searchHandlerList)
                {
                    if (i == searchHandlerId)
                    {
                        isSearchHandlerExist = true;
                        break;
                    }
                }
                if (isSearchHandlerExist)
                {
                    break;
                }
            }
            Condition.IsTrue(isSearchHandlerExist);
            Condition.IsTrue(gmtTokenIndex == smbConnection.searchHandlerContainer[searchHandlerId]);
            Update.UpdateRequest(smbConnection, smbRequest);
        }


        /// <summary>
        /// TRANS2_FIND_FIRST2 Request for invalid directory token.
        /// </summary>
        /// <param name="messageId"> This is used to associate a response with a request.</param>
        /// <param name="sessionId"> The current session ID for this connection.</param>
        /// <param name="treeId"> The tree ID for the current share connection.</param>
        /// <param name="isSigned"> Indicate whether the message is signed or not for this request.</param>
        /// <param name="isReparse">Indicate whether the SMB_FLAGS2_REPARSE_PATH is set 
        /// in the Flag2 field of the SMB header.</param>
        /// <param name="informationLevel"> The information level used for this request.</param>
        /// <param name="gmtTokenIndex"> The index of the GMT token configured by CheckPreviousVersion action.</param>
        /// <param name="isFlagsKnowsLongNameSet">Indicate whether the SMB_FLAGS2_KNOWS_LONG_NAMES flag 
        /// in SMB header is set or not.</param>
        /// <param name="isGmtPatten"> Indicate whether the GMT Patten is used.</param>
        [Rule]
        public static void Trans2FindFirst2RequestInvalidDirectoryToken(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isReparse,
            InformationLevel informationLevel,
            int gmtTokenIndex,
            bool isFlagsKnowsLongNameSet,
            bool isGmtPatten)
        {
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);
            smbRequest = new Trans2FindFirst2Request(
                messageId,
                sessionId,
                treeId,
                gmtTokenIndex,
                isReparse,
                informationLevel,
                isFlagsKnowsLongNameSet,
                isGmtPatten);
            // -1 here means the invalid token 
            const int invalidToken = -1;
            Condition.IsTrue(gmtTokenIndex == invalidToken);
            Update.UpdateRequest(smbConnection, smbRequest);
            Requirement.AssumeCaptured("Test case for invalid directory token");
        }

        /// <summary>
        /// TRANS2_FIND_FIRST2 Request for invalid file token.
        /// </summary>
        /// <param name="messageId"> This is used to associate a response with a request.</param>
        /// <param name="sessionId"> The current session ID for this connection.</param>
        /// <param name="treeId"> The tree ID for the current share connection.</param>
        /// <param name="isSigned"> Indicate whether the message is signed or not for this request.</param>
        /// <param name="isReparse">Indicate whether the SMB_FLAGS2_REPARSE_PATH is set 
        /// in the Flag2 field of the SMB header.</param>
        /// <param name="informationLevel"> The information level used for this request.</param>
        /// <param name="gmtTokenIndex"> The index of the GMT token configured by CheckPreviousVersion action.</param>
        /// <param name="isFlagsKnowsLongNameSet">Indicate whether the SMB_FLAGS2_KNOWS_LONG_NAMES flag 
        /// in SMB header is set or not.</param>
        /// <param name="isGmtPatten"> Indicate whether the GMT Patten is used.</param>
        [Rule]
        public static void Trans2FindFirst2RequestInvalidFileToken(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isReparse,
            InformationLevel informationLevel,
            int gmtTokenIndex,
            bool isFlagsKnowsLongNameSet,
            bool isGmtPatten)
        {
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);
            smbRequest = new Trans2FindFirst2Request(
                messageId,
                sessionId,
                treeId,
                gmtTokenIndex,
                isReparse,
                informationLevel,
                isFlagsKnowsLongNameSet,
                isGmtPatten);
            // -1 here means the invalid token. 
            const int invalidToken = -1;
            Condition.IsTrue(gmtTokenIndex == invalidToken);
            Update.UpdateRequest(smbConnection, smbRequest);
            Requirement.AssumeCaptured("Test case for invalid file token");
        }

        /// <summary>
        /// TRANS2_FIND_NEXT2 Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="sessionId">The current session ID for this connection.</param>
        /// <param name="treeId">The tree ID for the corrent share connection.</param>
        /// <param name="isSigned">It indicates whether the message is signed or not for this request.</param>
        /// <param name="isFileIdEqualZero">It indicates whether the fileId equal 0.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule]
        public static void Trans2FindNext2Response(
            int messageId,
            int sessionId,
            int treeId,
            bool isSigned,
            bool isFileIdEqualZero,
            MessageStatus messageStatus)
        {
            Trans2FindNext2Request request = null;
            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.Trans2FindNext2Request>(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                messageStatus,
                out request,
                MessageStatus.Success);
            Condition.IsTrue(request.command == Command.TransFindNext2);

            if (messageStatus == MessageStatus.Success)
            {
                ModelHelper.CaptureRequirement(
                    9287,
                    "[In response. Extensions]The server MUST send a TRANS2_FIND_NEXT2 response in reply to a client " +
                    "TRANS2_FIND_NEXT2 subcommand request when the request is successful.");
            }
            Condition.IsTrue(request.treeId == treeId);
            Condition.IsTrue(request.gmtTokenIndex != -1);

            //[Flags2 (2 bytes): SMB_FLAGS2_KNOWS_LONG_NAMES]If a client does not support long names 
            // (i.e. SMB2_FLAGS2_KNOWS_LONG_NAMES is not set), than any TRANS_FIND_FIRST2/FIND_NEXT2 request with an 
            // information level other than SMB_INFO_STANDARD MUST be failed with STATUS_INVALID_PARAMETER.
            if (request.informationLevel != InformationLevel.SmbInfoStandard)
            {
                Condition.IsTrue(request.isFlagsKnowsLongNameSet);
            }

            SmbFile file = new SmbFile();
            foreach (SmbFile searchFile in smbConnection.openedFiles.Values)
            {
                foreach (int i in searchFile.previousVersionToken)
                {
                    if (request.gmtTokenIndex == i)
                    {
                        file = searchFile;
                        file.searchHandlerList.Remove(request.searchHandlerId);
                        break;
                    }
                }
            }

            switch (request.informationLevel)
            {
                case InformationLevel.SmbFindFileIdFullDirectoryInfo:
                    // Parameter.isSupportUniqueFileId is configured in ServerSetup action, and the SUT is configured
                    // according to this.
                    if (!Parameter.isSupportUniqueFileId)
                    {
                        Condition.IsTrue(isFileIdEqualZero);
                    }
                    break;
                case InformationLevel.SmbFindFileIdBothDirectoryInfo:
                    // Parameter.isSupportUniqueFileId is configured in ServerSetup action, and the SUT is configured 
                    // according to this.
                    if (!Parameter.isSupportUniqueFileId)
                    {
                        Condition.IsTrue(isFileIdEqualZero);
                    }
                    break;
                default:
                    break;
            }
            Update.UpdateResponse(smbConnection, messageId);
        }


        /// <summary>
        /// TRANS2_FIND_NEXT2 Error Response.
        /// </summary>
        /// <param name="messageId">This is used to associate a response with a request.</param>
        /// <param name="messageStatus">
        /// It indicates that the status code returned from the SUT is success or failure.
        /// </param>
        [Rule(Action = "ErrorResponse(messageId, messageStatus)")]
        public static void ErrorFindNext2Response(int messageId, MessageStatus messageStatus)
        {
            Condition.IsTrue(smbConnection.sentRequest.ContainsKey(messageId));
            Condition.IsTrue(messageStatus == MessageStatus.NetworkSessionExpired
                                || messageStatus == MessageStatus.ObjectNameNotFound
                                || messageStatus == MessageStatus.InvalidParameter);

            Condition.IsTrue(smbConnection.sentRequest[messageId].command == Command.TransFindNext2);
            Trans2FindNext2Request request = (Trans2FindNext2Request)smbConnection.sentRequest[messageId];
            if (messageStatus == MessageStatus.ObjectNameNotFound)
            {
                Condition.IsTrue(request.gmtTokenIndex == -1);
                Condition.IsTrue(request.isFlagsKnowsLongNameSet);
                smbState = SmbState.End;
            }
            else if (messageStatus == MessageStatus.NetworkSessionExpired)
            {
                Condition.IsTrue(request.isFlagsKnowsLongNameSet);
                smbConnection.sessionList.Remove(request.sessionId);
                smbState = SmbState.Closed;
            }
            if (messageStatus == MessageStatus.InvalidParameter)
            {
                Condition.IsTrue(!request.isFlagsKnowsLongNameSet
                    && request.informationLevel != InformationLevel.SmbInfoStandard);
                smbState = SmbState.End;
            }
            smbConnection.sentRequest.Remove(messageId);
        }

        #endregion

        #region TRANS2_QUERY_PATH_INFORMATION (FSCC)
        /// <summary>
        /// TRANS2_QUERY_PATH_INFORMATION Request
        /// </summary>
        /// <param name="messageId"> This is used to associate a response with a request.</param>
        /// <param name="sessionId"> The current session ID for this connection.</param>
        /// <param name="treeId"> The tree ID for the corrent share connection.</param>
        /// <param name="isSigned"> Indicates whether the message is signed or not for this request.</param>
        /// <param name="isUsePassthrough"> Indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field 
        /// of the request.</param>
        /// <param name="isReparse">Indicates whether the SMB_FLAGS2_REPARSE_PATH is set in the Flag2 field of 
        /// the SMB header.</param>
        /// <param name="informationLevel"> The information level used for this request.</param>
        /// <param name="gmtTokenIndex"> The index of the GMT token configured by CheckPreviousVersion action.</param>
        [Rule]
        public static void FSCCTrans2QueryPathInfoRequest(int messageId,
                                                          int sessionId,
                                                          int treeId,
                                                          bool isSigned,
                                                          FSCCTransaction2QueryPathInforLevel informationLevel)
        {
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);
            smbRequest = new FSCCTrans2QueryPathInfoRequest(messageId, sessionId, treeId, isSigned, informationLevel);
            Condition.IsTrue(smbConnection.sutCapabilities.Contains(Capabilities.CapNtSmbs));

            if (informationLevel == FSCCTransaction2QueryPathInforLevel.FileAccessInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FileAccessInformation");
            }
            else if (informationLevel == FSCCTransaction2QueryPathInforLevel.FileAlignmentInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FileAlignmentInformation");
            }
            else if (informationLevel == FSCCTransaction2QueryPathInforLevel.FileAlternateNameInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FileAlternateNameInformation");
            }
            else if (informationLevel == FSCCTransaction2QueryPathInforLevel.FileAttributeTagInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FileAttributeTagInformation");
            }
            else if (informationLevel == FSCCTransaction2QueryPathInforLevel.FileBasicInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FileBasicInformation");
            }
            else if (informationLevel == FSCCTransaction2QueryPathInforLevel.FileCompressionInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FileCompressionInformation");
            }
            else if (informationLevel == FSCCTransaction2QueryPathInforLevel.FileEaInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FileEaInformation");
            }
            else if (informationLevel == FSCCTransaction2QueryPathInforLevel.FileInternalInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FileInternalInformation");
            }
            else if (informationLevel == FSCCTransaction2QueryPathInforLevel.FileModeInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FileModeInformation");
            }
            else if (informationLevel == FSCCTransaction2QueryPathInforLevel.FileNameInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FileNameInformation");
            }
            else if (informationLevel == FSCCTransaction2QueryPathInforLevel.FileNetworkOpenInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FileNetworkOpenInformation");
            }
            else if (informationLevel == FSCCTransaction2QueryPathInforLevel.FilePositionInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FilePositionInformation");
            }
            else if (informationLevel == FSCCTransaction2QueryPathInforLevel.FileStandardInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FileStandardInformation");
            }
            else if (informationLevel == FSCCTransaction2QueryPathInforLevel.FileStreamInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FileStreamInformation");
            }

            Update.UpdateRequest(smbConnection, smbRequest);
        }

        /// <summary>
        /// TRANS2_QUERY_PATH_INFORMATION Response
        /// </summary>
        /// <param name="messageId"> This is used to associate a response with a request.</param>
        /// <param name="sessionId"> The current session ID for this connection.</param>
        /// <param name="treeId"> The tree ID for the corrent share connection.</param>
        /// <param name="isSigned"> Indicates whether the message is signed or not for this response.</param>
        /// <param name="messageStatus"> Indicate the status code returned from server, success or fail.</param>
        [Rule]
        public static void FSCCTrans2QueryPathInfoResponse(int messageId,
                                                           int sessionId,
                                                           int treeId,
                                                           bool isSigned,
                                                           MessageStatus messageStatus)
        {
            FSCCTrans2QueryPathInfoRequest request;
            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.FSCCTrans2QueryPathInfoRequest>(smbConnection,
                messageId, sessionId, treeId, isSigned, messageStatus, out request, MessageStatus.Success);
            Condition.IsTrue(request.command == Command.FSCCTRANS2_QUERY_PATH_INFORMATION);

            Update.UpdateResponse(smbConnection, messageId);
        }
        #endregion

        #region TRANS2_QUERY_FS_INFORMATION (FSCC)
        /// <summary>
        /// TRANS2_QUERY_FS_INFORMATION Request
        /// </summary>
        /// <param name="messageId"> This is used to associate a response with a request.</param>
        /// <param name="sessionId"> The current session ID for this connection.</param>
        /// <param name="treeId"> The tree ID for the corrent share connection.</param>
        /// <param name="isSigned"> Indicates whether the message is signed or not for this request.</param>
        /// <param name="isUsePassthrough"> Indicates whether adding SMB_INFO_PASSTHROUGH in InformationLevel field 
        /// of the request.</param>
        /// <param name="informationLevel"> The information level used for this request.</param>
        /// <param name="otherBits"> The bits not listed in section</param>
        /// <param name="reserved"> it MUST be ignored upon receipt of the message.</param>
        [Rule]
        public static void FSCCTrans2QueryFSInfoRequest(int messageId,
                                                        int sessionId,
                                                        int treeId,
                                                        bool isSigned,
                                                        FSCCTransaction2QueryFSInforLevel informationLevel)
        {
            Checker.CheckRequest(smbConnection, messageId, sessionId, treeId, isSigned, smbState);
            Condition.IsTrue(smbConnection.treeConnectList[treeId].smbShare.shareType == ShareType.Disk);

            if (informationLevel == FSCCTransaction2QueryFSInforLevel.FileFsAttributeInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FileFsAttributeInformation");
            }
            else if (informationLevel == FSCCTransaction2QueryFSInforLevel.FileFsDeviceInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FileFsDeviceInformation");
            }
            else if (informationLevel == FSCCTransaction2QueryFSInforLevel.FileFsSizeInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FileFsSizeInformation");
            }
            else if (informationLevel == FSCCTransaction2QueryFSInforLevel.FileFsVolumeInformation)
            {
                Requirement.AssumeCaptured("Information level in FSCC is FileFsVolumeInformation");
            }
            smbRequest = new FSCCTrans2QueryFSInfoRequest(messageId, sessionId, treeId, isSigned, informationLevel);

            Update.UpdateRequest(smbConnection, smbRequest);
        }

        /// <summary>
        /// TRANS2_QUERY_FS_INFORMATION Response
        /// </summary>
        /// <param name="messageId"> This is used to associate a response with a request.</param>
        /// <param name="sessionId"> The current session ID for this connection.</param>
        /// <param name="treeId"> The tree ID for the corrent share connection.</param>
        /// <param name="isSigned"> Indicates whether the message is signed or not for this response.</param>
        /// <param name="messageStatus"> Indicate the status code returned from server, success or fail.</param>
        [Rule]
        public static void FSCCTrans2QueryFSInfoResponse(int messageId,
                                                         int sessionId,
                                                         int treeId,
                                                         bool isSigned,
                                                         MessageStatus messageStatus)
        {
            FSCCTrans2QueryFSInfoRequest request;
            Checker.CheckResponse<Microsoft.Protocol.TestSuites.Smb.FSCCTrans2QueryFSInfoRequest>(
                smbConnection,
                messageId,
                sessionId,
                treeId,
                isSigned,
                messageStatus,
                out request,
                MessageStatus.Success);
            Condition.IsTrue(request.command == Command.FSCCTRANS2_QUERY_FS_INFORMATION);

            Update.UpdateResponse(smbConnection, messageId);
        }
        #endregion

        #endregion
    }
}