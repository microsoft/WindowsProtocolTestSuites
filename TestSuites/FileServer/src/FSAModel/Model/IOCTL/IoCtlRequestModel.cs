// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Model
{
    /// <summary>
    /// MS-FSA model program
    /// </summary>
    public static partial class ModelProgram
    {
        public static bool IsImplement_FSCTL_CREATE_OR_GET_OBJECT_ID;
        public static bool IsImplement_FSCTL_DELETE_OBJECT_ID;
        public static bool IsImplement_FSCTL_SET_OBJECT_ID;
        public static bool IsImplement_FSCTL_QUERY_SPARING_INFO;
        public static bool IsImplement_FSCTL_QUERY_FAT_BPB;
        public static bool IsImplement_FSCTL_QUERY_ON_DISK_VOLUME_INFO;
        public static bool IsImplement_SET_ZERO_ON_DEALLOCATION;
        public static bool IsImplement_FSCTL_GET_NTFS_VOLUME_DATA;

        #region GetFileSystemConfig

        /// <summary>
        /// The call part of the method GetIfImplementObjectIdIoctlRequest.
        /// This is a call-return pattern
        /// </summary>
        [Rule(Action = "call GetIfImplementObjectIdIoCtlRequest(out _)")]
        public static void CallGetIfImplementObjectIdIoctlRequest()
        {
        }

        /// <summary>
        /// The return part of the method GetIfImplementObjectIdIoctlRequest.
        /// This is a call-returns pattern. 
        /// </summary>
        /// <param name="isImplemented">True: if this function is implemented.</param>
        [Rule(Action = "return GetIfImplementObjectIdIoCtlRequest(out isImplemented)")]
        public static void ReturnGetIfImplementObjectIdIoctlRequest(bool isImplemented)
        {
            IsImplement_FSCTL_CREATE_OR_GET_OBJECT_ID = isImplemented;
            IsImplement_FSCTL_DELETE_OBJECT_ID = isImplemented;
            IsImplement_FSCTL_SET_OBJECT_ID = isImplemented;

            gisObjectIDsSupported = isImplemented;
        }

        #endregion


        #region 3.1.5.9    Application Requests an FsControl Request

        #region Initialize

        #region ptfConfig

        /// <summary>
        /// True if the ObjectStore implemented the specified functionality.
        /// </summary>
        static bool isObjectImplementedFunctionality;
        /// <summary>
        /// Call of function GetObjectFunctionality
        /// </summary>
        [Rule(Action = "call GetObjectFunctionality(out _ )")]
        public static void CallGetObjectFunctionality()
        {
        }

        /// <summary>
        /// Return of function GetObjectFunctionality
        /// </summary>
        /// <param name="isImplemented">A flag</param>
        [Rule(Action = "return GetObjectFunctionality(out isImplemented)")]
        public static void ReturnGetObjectFunctionality(bool isImplemented)
        {
            isObjectImplementedFunctionality = isImplemented;
        }

        /// <summary>
        /// A boolean that is true if the physical media format for this volume supports ObjectIDs.
        /// </summary>
        static bool isObjectIDsSupportedTrue;

        /// <summary>
        /// Call of function GetObjectIDsSupported
        /// </summary>
        [Rule(Action = "call GetObjectIDsSupported(out _ )")]
        public static void CallGetObjectIDsSupported()
        {
        }

        /// <summary>
        /// Return of function GetObjectIDsSupported
        /// </summary>
        /// <param name="isSupported">A flag</param>
        [Rule(Action = "return GetObjectIDsSupported(out isSupported)")]
        public static void ReturnGetObjectIDsSupported(bool isSupported)
        {
            isObjectIDsSupportedTrue = isSupported;
        }

        /// <summary>
        /// A boolean that is true if the physical media format for this volume supports ReparsePoints.
        /// </summary>
        static bool isReparsePointsSupportedTrue;

        /// <summary>
        /// Call of function GetReparsePointsSupported
        /// </summary>
        [Rule(Action = "call GetReparsePointsSupported(out _ )")]
        public static void CallGetReparsePointsSupported()
        {
        }

        /// <summary>
        /// Return of function GetReparsePointsSupported
        /// </summary>
        /// <param name="isSupported">A flag</param>
        [Rule(Action = "return GetReparsePointsSupported(out isSupported)")]
        public static void ReturnGetReparsePointsSupported(bool isSupported)
        {
            isReparsePointsSupportedTrue = isSupported;
        }

        /// <summary>
        /// A boolean that is true if the Open was performed by a user who is allowed to manage the volume.
        /// </summary>
        static bool openHasManageVolPrivilege;

        /// <summary>
        /// Call of function GetopenHasManageVolPrivilege
        /// </summary>
        [Rule(Action = "call GetopenHasManageVolPrivilege(out _ )")]
        public static void CallGetopenHasManageVolPrivilege()
        {
        }

        /// <summary>
        /// Return of function GetopenHasManageVolPrivilege
        /// </summary>
        /// <param name="isSupported">A flag</param>
        [Rule(Action = "return GetopenHasManageVolPrivilege(out isSupported)")]
        public static void ReturnGetopenHasManageVolPrivilege(bool isSupported)
        {
            openHasManageVolPrivilege = isSupported;
        }

        /// <summary>
        /// A boolean that is true if the Open was performed by a user who is allowed to perform backup operations.
        /// </summary>
        static bool openHasBackupPrivage = false;

        /// <summary>
        /// A boolean that is true if the Open was performed by a user who is allowed to perform restore operations.
        /// </summary>
        static bool isHasRestoreAccess;
        /// <summary>
        /// Call of function GetRestoreAccess
        /// </summary>
        [Rule(Action = "call GetRestoreAccess(out _ )")]
        public static void CallGetRestoreAccess()
        {
        }
        /// <summary>
        /// Rerutn of function GetRestoreAccess
        /// </summary>
        /// <param name="isHas">A flag</param>
        [Rule(Action = "return GetRestoreAccess(out isHas)")]
        public static void ReturnGetRestoreAccess(bool isHas)
        {
            isHasRestoreAccess = isHas;
        }

        /// <summary>
        /// A boolean that is true if the Open was performed by a user who is allowed to create symbolic links.
        /// </summary>
        static bool isHasCreateSymbolicLinkAccessTrue = false;

        /// <summary>
        /// A boolean that is true if the Open was performed by a user who is a member of the 
        /// BUILTIN_ADMINISTRATORS group as specified in [MS-DTYP] section 2.4.2.3.
        /// </summary>
        static bool isIsAdministratorTrue;
        /// <summary>
        /// Call of function GetAdministrator
        /// </summary>
        [Rule(Action = "call GetAdministrator(out _ )")]
        public static void CallGetAdministrator()
        {
        }
        /// <summary>
        /// Return of function GetAdministrator
        /// </summary>
        /// <param name="isGet">A flag</param>
        [Rule(Action = "return GetAdministrator(out isGet)")]
        public static void ReturnGetAdministrator(bool isGet)
        {
            isIsAdministratorTrue = isGet;
        }

        /// <summary>
        /// A boolean that is true if the physical media format for this volume supports Quotas.
        /// </summary>
        static bool IsQuotasSupported = false;

        /// <summary>
        /// A boolean that is true if the Open was performed by a user who is allowed to perform restore operations.
        /// </summary>
        /// Disable warning CA2211, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static bool openHasRestoreAcces;

        #endregion

        #endregion

        #region 3.1.5.9.1  FSCTL_CREATE_OR_GET_OBJECT_ID

        /// <summary>
        /// 3.1.5.9.1    FSCTL_CREATE_OR_GET_OBJECT_ID
        /// </summary>
        /// <param name="bufferSize">Indicate buffer size</param>
        /// <param name="isBytesReturnedSet">True: if the return status is success.</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        /// Disable warning CA1801, because the parameter of "capabilities" is used for extend the model logic, 
        /// which will affect the implementation of the model if it is removed.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        [Rule]
        public static MessageStatus FsCtlCreateOrGetObjId(
            BufferSize bufferSize,
            out bool isBytesReturnedSet
            )
        {
            Condition.IsTrue(bufferSize == BufferSize.LessThanFILE_OBJECTID_BUFFER || bufferSize == BufferSize.BufferSizeSuccess);
            isBytesReturnedSet = false;

            if (!IsImplement_FSCTL_CREATE_OR_GET_OBJECT_ID)
            {
                Helper.CaptureRequirement(951, @"[In FSCTL_CREATE_OR_GET_OBJECT_ID,Pseudocode for the operation is as follows: ]
                    If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<16>");
                return MessageStatus.INVALID_DEVICE_REQUEST;
            }
            if (!gisObjectIDsSupported)
            {
                Helper.CaptureRequirement(4993, @"[In FSCTL_CREATE_OR_GET_OBJECT_ID ] Pseudocode for the operation is as follows:
                    If Open.File.Volume.IsObjectIDsSupported is FALSE, the operation MUST be failed with STATUS_VOLUME_NOT_UPGRADED.");
                return MessageStatus.STATUS_VOLUME_NOT_UPGRADED;
            }
            // If OutputBufferSize is less than the size of FILE_OBJECTID_BUFFER
            if (bufferSize == BufferSize.LessThanFILE_OBJECTID_BUFFER)
            {
                Helper.CaptureRequirement(4994, @"[In FSCTL_CREATE_OR_GET_OBJECT_ID,Pseudocode for the operation is as follows:] 
                    If OutputBufferSize is less than sizeof( FILE_OBJECTID_BUFFER ), the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                return MessageStatus.INVALID_PARAMETER;
            }

            Helper.CaptureRequirement(947, @"[In FSCTL_CREATE_OR_GET_OBJECT_ID ]On completion, the object store MUST return:
                [Status,OutputBuffer,BytesReturned].");
            Helper.CaptureRequirement(965, @"[In FSCTL_CREATE_OR_GET_OBJECT_ID,Pseudocode for the operation is as follows:]
                Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
            Helper.CaptureRequirement(964, @"[In FSCTL_CREATE_OR_GET_OBJECT_ID,Pseudocode for the operation is as follows:]
                Upon successful completion of the operation, the object store MUST return:BytesReturned set to sizeof( FILE_OBJECTID_BUFFER ).");
            isBytesReturnedSet = true;
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.9.2  FSCTL_DELETE_OBJECT_ID

        /// <summary>
        /// 3.1.5.9.2    FSCTL_DELETE_OBJECT_ID
        /// </summary>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus FsCtlDeleteObjId()
        {
            if (!IsImplement_FSCTL_DELETE_OBJECT_ID)
            {
                Helper.CaptureRequirement(4996, @"[In FSCTL_DELETE_OBJECT_ID ] If the object store does not implement this functionality, 
                    the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<19>");
                return MessageStatus.INVALID_DEVICE_REQUEST;
            }

            if (!gisObjectIDsSupported)
            {
                Helper.CaptureRequirement(4997, @"[In FSCTL_DELETE_OBJECT_ID ] Pseudocode for the operation is as follows:
                    If Open.File.Volume.IsObjectIDsSupported is FALSE, the operation MUST be failed with STATUS_VOLUME_NOT_UPGRADED.");
                return MessageStatus.STATUS_VOLUME_NOT_UPGRADED;
            }

            Helper.CaptureRequirement(974, @"[In FSCTL_DELETE_OBJECT_ID,Pseudocode for the operation is as follows: ]Upon successful completion 
                of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
            Helper.CaptureRequirement(1252, @"[In FSCTL_DELETE_OBJECT_ID ]On completion, the object store MUST return:[Status].");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.9.3  FSCTL_DELETE_REPARSE_POINT

        /// <summary>
        /// 3.1.5.9.3    FSCTL_DELETE_REPARSE_POINT
        /// </summary>
        /// <param name="reparseTag">An identifier indicating the type of the reparse point to delete, as defined in [MS-FSCC] section 2.1.2.1.</param>
        /// <param name="reparseGuidEqualOpenGuid">True: if Open.File.ReparseGUID == ReparseGUID</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus FsCtlDeleteReparsePoint(
            ReparseTag reparseTag,
            bool reparseGuidEqualOpenGuid
            )
        {
            Condition.IsTrue(reparseTag == ReparseTag.IO_REPARSE_TAG_RESERVED_ONE || reparseTag == ReparseTag.IO_REPARSE_TAG_RESERVED_ZERO || reparseTag == ReparseTag.EMPTY || reparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG || reparseTag == ReparseTag.NotEqualOpenFileReparseTag);

            if (!isReparsePointsSupportedTrue)
            {
                Helper.CaptureRequirement(5000, @"[In FSCTL_DELETE_REPARSE_POINT,Pseudocode for the operation is as follows:
                    Phase 1 -- Verify the parameters.] 
                    If Open.File.Volume.IsReparsePointsSupported is FALSE, the operation MUST be failed with STATUS_VOLUME_NOT_UPGRADED.");
                return MessageStatus.STATUS_VOLUME_NOT_UPGRADED;
            }

            if (!isObjectImplementedFunctionality)
            {
                Helper.CaptureRequirement(4999, @"[In FSCTL_DELETE_REPARSE_POINT] If the object store does not implement this functionality, 
                    the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<21>");
                return MessageStatus.INVALID_DEVICE_REQUEST;
            }

            //If the ReparseTag is IO_REPARSE_TAG_RESERVED_ZERO, 
            if (reparseTag == ReparseTag.IO_REPARSE_TAG_RESERVED_ONE)
            {
                Helper.CaptureRequirement(984, @"[In FSCTL_DELETE_REPARSE_POINT,Phase 1 - Verify the parameters.]
                    If the ReparseTag is IO_REPARSE_TAG_RESERVED_ZERO , the operation MUST be failed with STATUS_IO_REPARSE_TAG_INVALID.");
                return MessageStatus.IO_REPARSE_TAG_INVALID;
            }

            //If the ReparseTag is IO_REPARSE_TAG_RESERVED_ONE, 
            if (reparseTag == ReparseTag.IO_REPARSE_TAG_RESERVED_ZERO)
            {
                Helper.CaptureRequirement(2437, @"[In FSCTL_DELETE_REPARSE_POINT,
                    Phase 1 -- Verify the parameters.]If the ReparseTag is IO_REPARSE_TAG_RESERVED_ONE, 
                    the operation MUST be failed with STATUS_IO_REPARSE_TAG_INVALID.");
                return MessageStatus.IO_REPARSE_TAG_INVALID;
            }
            //If ReparseTag is a non-Microsoft Reparse Tag, then the ReparseGUID must be a valid GUID; 
            //otherwise the operation MUST be failed with STATUS_IO_REPARSE_DATA_INVALID.
            if (reparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG && reparseGuidEqualOpenGuid)
            {
                Helper.CaptureRequirement(1257, @"[In FSCTL_DELETE_REPARSE_POINT,Pseudocode for the operation is as follows:
                    Phase 1 -- Verify the parameters.]otherwise[If ReparseTag is not a non-Microsoft Reparse Tag] 
                    the operation MUST be failed with STATUS_IO_REPARSE_DATA_INVALID.");
                return MessageStatus.IO_REPARSE_DATA_INVALID;
            }
            if (reparseTag == ReparseTag.NotEqualOpenFileReparseTag)
            {
                Helper.CaptureRequirement(989, @"[In FSCTL_DELETE_REPARSE_POINT,Pseudocode for the operation is as follows:
                    Phase 2 -- Validate that the requested tag deletion type matches with the stored tag type.]
                    If (ReparseTag != Open.File.ReparseTag), the operation MUST be failed with STATUS_IO_REPARSE_TAG_MISMATCH.");
                return MessageStatus.IO_REPARSE_TAG_MISMATCH;
            }
            if ((reparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG) && (!reparseGuidEqualOpenGuid))
            {
                Helper.CaptureRequirement(990, @"[In FSCTL_DELETE_REPARSE_POINT,Pseudocode for the operation is as follows:
                    Phase 2 -- Validate that the requested tag deletion type matches with the stored tag type.]
                    If (ReparseTag is a non-Microsoft Reparse Tag && Open.File.ReparseGUID != ReparseGUID), 
                    the operation MUST be failed with STATUS_REPARSE_ATTRIBUTE_CONFLICT.");
                return MessageStatus.REPARSE_ATTRIBUTE_CONFLICT;
            }
            Helper.CaptureRequirement(1254, @"[In FSCTL_DELETE_REPARSE_POINT]On completion, the object store MUST return:[Status].");
            Helper.CaptureRequirement(5001, @"[In FSCTL_DELETE_REPARSE_POINT,Pseudocode for the operation is as follows:]
                Upon successful completion of the operation, the object store MUST return:status set to STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.9.5  FSCTL_FIND_FILES_BY_SID

        /// <summary>
        /// 3.1.5.9.5  FSCTL_FIND_FILES_BY_SID
        /// </summary>
        /// <param name="bufferSize">To indicate buffer size</param>
        /// <param name="isBytesReturnedSet">The value is true if isBytesReturned is set</param>
        /// <param name="isOutputBufferOffset">The value is true if isOutputBufferOff is set</param>
        /// <param name="linkOwnerSidEqualSID">The value is true if linkownerSid equals to SID</param>
        /// <param name="openFileVolQuoInfoEmpty">The value if true if openFileVolQuoInfo if empty</param>
        /// <param name="outPutBufLessLinkSize">The value if true if output buffer is less than linksize</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        [Rule]
        public static MessageStatus FsCtlFindFilesBySID(
            bool openFileVolQuoInfoEmpty,
            BufferSize bufferSize,
            bool linkOwnerSidEqualSID,
            bool outPutBufLessLinkSize,
            bool isOutputBufferOffset,
            out bool isBytesReturnedSet
            )
        {
            Condition.IsTrue(bufferSize == BufferSize.LessThanFILE_NAME_INFORMATION || bufferSize == BufferSize.BufferSizeSuccess);
            isBytesReturnedSet = false;
            if (!isObjectImplementedFunctionality)
            {
                Helper.CaptureRequirement(5005, @"[In FSCTL_FIND_FILES_BY_SID] If the object store does not implement this functionality, 
                    the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<25> ");

                return MessageStatus.INVALID_DEVICE_REQUEST;
            }

            //  If Open.HasManageVolumePrivilege is FALSE and Open.HasBackupPrivilege is FALSE
            if (!openHasManageVolPrivilege && !openHasBackupPrivage)
            {
                Helper.CaptureRequirement(1022, @"[In FSCTL_FIND_FILES_BY_SID,Pseudocode for the operation is as follows:]
                    If Open.HasManageVolumeAccess is FALSE and Open.HasBackupAccess is FALSE, the operation MUST be failed with STATUS_ACCESS_DENIED.");

                return MessageStatus.ACCESS_DENIED;
            }

            //  If Open.File.Volume.QuotaInformation is empty
            if (openFileVolQuoInfoEmpty)
            {
                Helper.CaptureRequirement(1023, @"[In FSCTL_FIND_FILES_BY_SID,Pseudocode for the operation is as follows:]
                    If Open.File.Volume.QuotaInformation is empty, the operation MUST be failed with STATUS_NO_QUOTAS_FOR_ACCOUNT.");

                return MessageStatus.NO_QUOTAS_FOR_ACCOUNT;
            }

            //  If OutputBufferSize is less than the size of FILE_NAME_INFORMATION
            if (bufferSize == BufferSize.LessThanFILE_NAME_INFORMATION)
            {
                Helper.CaptureRequirement(1024, @"[In FSCTL_FIND_FILES_BY_SID,Pseudocode for the operation is as follows:]
                    If OutputBufferSize is less than sizeof( FILE_NAME_INFORMATION ), the operation MUST be failed with STATUS_INVALID_USER_BUFFER.");
                return MessageStatus.INVALID_USER_BUFFER;
            }

            //  For each Link in Open.File.DirectoryList, starting at Open.FindBySidRestartIndex
            //If Link.File.SecurityDescriptor.OwnerSid is equal to FindBySidData.SID
            if (linkOwnerSidEqualSID)
            {
                //If (OutputBufferLength ?OutputBufferOffset) is less than the size of 
                //(Link.Name + 2 bytes) aligned to 4 bytes:
                if (outPutBufLessLinkSize)
                {
                    if (!isOutputBufferOffset)
                    {
                        Helper.CaptureRequirement(1027, @"[In FSCTL_FIND_FILES_BY_SID,For each Link in Open.File.DirectoryList,
                            starting at Open.FindBySidRestartIndex:If Link.File.SecurityDescriptor.OwnerSid is equal to FindBySidData.SID:
                            If (OutputBufferLength ¨C OutputBufferOffset) is less than the size of (Link.Name + 2 bytes) aligned to 4 bytes:]
                            If OutputBufferOffset is not 0:The operation returns with STATUS_SUCCESS.");
                        isBytesReturnedSet = true;
                        Helper.CaptureRequirement(1034, @"[In FSCTL_FIND_FILES_BY_SID,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:BytesReturned set to OutputBufferOffset.");
                        return MessageStatus.SUCCESS;
                    }
                    else
                    {
                        Helper.CaptureRequirement(1028, @"[In FSCTL_FIND_FILES_BY_SID,Pseudocode for the operation is as follows:
                            For each Link in Open.File.DirectoryList, starting at Open.FindBySidRestartIndex:If Link.File.SecurityDescriptor.
                            OwnerSid is equal to FindBySidData.SID:If (OutputBufferLength ¨C OutputBufferOffset) is less than the size of 
                            (Link.Name + 2 bytes) aligned to 4 bytes:]Else[If OutputBufferOffset is 0:]
                            The operation MUST be failed with STATUS_BUFFER_TOO_SMALL.");

                        return MessageStatus.BUFFER_TOO_SMALL;
                    }
                }

            }

            Helper.CaptureRequirement(1014, @"[In FSCTL_FIND_FILES_BY_SID]On completion, the object store MUST return:
                [Status,OutputBuffer,BytesReturned ].");
            Helper.CaptureRequirement(1035, @"[In FSCTL_FIND_FILES_BY_SID,Pseudocode for the operation is as follows:]
                Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.9.4, 3.1.5.9.6, 3.1.5.9.7,3.1.5.9.8, 3.1.5.9.14, 3.1.5.9.16, 3.1.5.9.17, 3.1.5.9.19, 3.1.5.9.27, 3.1.5.9.29, 3.1.5.9.31
        // 11,12,13,26,29,31 null
        /// <summary>
        /// 3.1.5.9.4       FSCTL_FILESYSTEM_GET_STATISTICS
        /// 3.1.5.9.6       FSCTL_GET_COMPRESSION
        /// 3.1.5.9.7       FSCTL_GET_NTFS_VOLUME_DATA
        /// 3.1.5.9.8       SET_OBJECT_ID
        /// 3.1.5.9.14      FSCTL_QUERY_FAT_BPB
        /// 3.1.5.9.16      FSCTL_QUERY_ON_DISK_VOLUME_INFO
        /// 3.1.5.9.17      FSCTL_QUERY_SPARING_INFO
        /// 3.1.5.9.19      FSCTL_RECALL_FILE
        /// 3.1.5.9.27      FSCTL_SET_SHORT_NAME_BEHAVIOR
        /// 3.1.5.9.29      FSCTL_SET_ZERO_ON_DEALLOCATION
        /// 3.1.5.9.31      FSCTL_WRITE_USN_CLOSE_RECORD
        /// </summary>
        /// <param name="bufferSize">To indicate buffer size</param>
        /// <param name="isBytesReturnedSet">The value is true if BytesReturned is set</param>
        /// <param name="isOutputBufferSizeReturn">The value is true if outputBufferSize returned</param>
        /// <param name="requestType">To indicate FSControl request type</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        /// Disable warning CA1502, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [Rule]
        public static MessageStatus FsCtlForEasyRequest(
            FsControlRequestType requestType,
            BufferSize bufferSize,
            out bool isBytesReturnedSet,
            out bool isOutputBufferSizeReturn
            )
        {
            Condition.IsTrue((requestType == FsControlRequestType.FSCTL_FILESYSTEM_GET_STATISTICS &&
                        (bufferSize == BufferSize.LessThanSizeOf_FILESYSTEM_STATISTICS || bufferSize == BufferSize.BufferSizeSuccess)) ||

                (requestType == FsControlRequestType.FSCTL_GET_COMPRESSION &&
                        (bufferSize == BufferSize.LessThanTwoBytes || bufferSize == BufferSize.BufferSizeSuccess)) ||

                (requestType == FsControlRequestType.GET_NTFS_VOLUME_DATA &&
                        (bufferSize == BufferSize.LessThanNTFS_VOLUME_DATA_BUFFER || bufferSize == BufferSize.BufferSizeSuccess)) ||

                (requestType == FsControlRequestType.QUERY_FAT_BPB &&
                        (bufferSize == BufferSize.LessThan0x24 || bufferSize == BufferSize.BufferSizeSuccess)) ||

                (requestType == FsControlRequestType.QUERY_ON_DISK_VOLUME_INFO &&
                        (bufferSize == BufferSize.LessThanFILE_QUERY_ON_DISK_VOL_INFO_BUFFER || bufferSize == BufferSize.BufferSizeSuccess)) ||

                (requestType == FsControlRequestType.QUERY_SPARING_INFO &&
                        (bufferSize == BufferSize.LessThanFILE_QUERY_SPARING_BUFFER || bufferSize == BufferSize.BufferSizeSuccess)) ||

                (requestType == FsControlRequestType.RECALL_FILE && bufferSize == BufferSize.BufferSizeSuccess) ||

                (requestType == FsControlRequestType.FSCTL_SET_SHORT_NAME_BEHAVIOR && bufferSize == BufferSize.BufferSizeSuccess) ||

                (requestType == FsControlRequestType.SET_ZERO_ON_DEALLOCATION && bufferSize == BufferSize.BufferSizeSuccess) ||

                (requestType == FsControlRequestType.WRITE_USN_CLOSE_RECORD &&
                        (bufferSize == BufferSize.LessThanSizeofUsn || bufferSize == BufferSize.BufferSizeSuccess))
                );

            isBytesReturnedSet = false;
            isOutputBufferSizeReturn = false;
            switch (requestType)
            {
                #region 3.1.5.9.4  FSCTL_FILESYSTEM_GET_STATISTICS
                case FsControlRequestType.FSCTL_FILESYSTEM_GET_STATISTICS:
                    {
                        if (!isObjectImplementedFunctionality)
                        {
                            Helper.CaptureRequirement(5803, @"[In FSCTL_FILESYSTEM_GET_STATISTICS] If the object store does not implement 
                                this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST. <23>");
                            return MessageStatus.INVALID_DEVICE_REQUEST;
                        }

                        if (bufferSize == BufferSize.LessThanSizeOf_FILESYSTEM_STATISTICS)
                        {
                            Requirement.Capture(@"[2.1.5.9.6   FSCTL_FILESYSTEM_GET_STATISTICS] Pseudocode for the operation is as follows:
                                If OutputBufferSize is less than sizeof(FILESYSTEM_STATISTICS), the operation is failed with STATUS_BUFFER_TOO_SMALL.");
                            return MessageStatus.BUFFER_TOO_SMALL;
                        }

                        // If OutputBufferSize is less than the total size of statistics information
                        if (bufferSize == BufferSize.LessThanTotalSizeOfStatistics)
                        {
                            isOutputBufferSizeReturn = true;
                            Helper.CaptureRequirement(1258, @"[In FSCTL_FILESYSTEM_GET_STATISTICS,Pseudocode for the operation is as follows:]
                                If OutputBufferSize is less than the total size of statistics information, then only OutputBufferSize bytes will be returned.");
                            Helper.CaptureRequirement(1259, @"[In FSCTL_FILESYSTEM_GET_STATISTICS,Pseudocode for the operation is as follows:
                                If OutputBufferSize is less than the total size of statistics information]the operation MUST succeed 
                                but return with STATUS_BUFFER_OVERFLOW.");
                            Helper.CaptureRequirement(4038, @"[In FSCTL_FILESYSTEM_GET_STATISTICS,Pseudocode for the operation is as follows:]
                                Upon successful completion of the operation, the object store MUST return:Status set to STATUS_BUFFER_OVERFLOW.");
                            return MessageStatus.BUFFER_OVERFLOW;
                        }

                        Helper.CaptureRequirement(1009, @"[In FSCTL_FILESYSTEM_GET_STATISTICS,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS or STATUS_BUFFER_OVERFLOW.");
                        Helper.CaptureRequirement(999, @"[In FSCTL_FILESYSTEM_GET_STATISTICS]On completion, 
                            the object store MUST return:[Status,OutputBuffer,BytesReturned ].");
                        isBytesReturnedSet = true;
                        isOutputBufferSizeReturn = true;
                        Helper.CaptureRequirement(1008, @"[In FSCTL_FILESYSTEM_GET_STATISTICS,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:BytesReturned set to the number of total number
                            of bytes of statistical information returned.");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region 3.1.5.9.6 FSCTL_GET_COMPRESSION

                case FsControlRequestType.FSCTL_GET_COMPRESSION:
                    {
                        if (!isObjectImplementedFunctionality)
                        {
                            Helper.CaptureRequirement(1043, @"[In FSCTL_GET_COMPRESSION] If the object store does not implement this functionality,
                                the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<26>");
                            return MessageStatus.INVALID_DEVICE_REQUEST;
                        }
                        if (bufferSize == BufferSize.LessThanTwoBytes)
                        {
                            Helper.CaptureRequirement(5008, @"[In FSCTL_GET_COMPRESSION]Pseudocode for the operation is as follows:
                                If OutputBufferSize is less than sizeof( USHORT ) (2 bytes), the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                            return MessageStatus.INVALID_PARAMETER;
                        }
                        Helper.CaptureRequirement(1051, @"[In FSCTL_GET_COMPRESSION]Upon successful completion of the operation, 
                            the object store MUST return:Status set to STATUS_SUCCESS.");
                        Helper.CaptureRequirement(1039, @"[In FSCTL_GET_COMPRESSION]On completion, the object store MUST return:[Status,OutputBuffer ,BytesReturned].");
                        Helper.CaptureRequirement(4037, @"[In FSCTL_GET_COMPRESSION,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        isBytesReturnedSet = true;
                        isOutputBufferSizeReturn = true;
                        Helper.CaptureRequirement(1050, @"[In FSCTL_GET_COMPRESSION,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:BytesReturned set to sizeof( USHORT ) (2 bytes).");
                        return MessageStatus.SUCCESS;

                    }
                #endregion

                #region 3.1.5.9.7  FSCTL_GET_NTFS_VOLUME_DATA
                case FsControlRequestType.GET_NTFS_VOLUME_DATA:
                    {
                        if (!IsImplement_FSCTL_GET_NTFS_VOLUME_DATA)
                        {
                            Helper.CaptureRequirement(5017, @"[In FSCTL_GET_NTFS_VOLUME_DATA] 
                                If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<27> ");
                            return MessageStatus.INVALID_DEVICE_REQUEST;
                        }
                        if (bufferSize == BufferSize.LessThanNTFS_VOLUME_DATA_BUFFER)
                        {
                            Helper.CaptureRequirement(5018, @"[In FSCTL_GET_NTFS_VOLUME_DATA] Pseudocode for the operation is as follows:
                                If OutputBufferSize is less than sizeof( NTFS_VOLUME_DATA_BUFFER ), the operation MUST be failed with STATUS_BUFFER_TOO_SMALL.");
                            return MessageStatus.BUFFER_TOO_SMALL;
                        }

                        isBytesReturnedSet = true;
                        isOutputBufferSizeReturn = true;
                        Helper.CaptureRequirement(5033, @"[In FSCTL_GET_NTFS_VOLUME_DATA,Pseudocode for the operation is as follows:] 
                            Upon successful completion of the operation, the object store MUST return:BytesReturned set to sizeof( NTFS_VOLUME_DATA_BUFFER ).");
                        Helper.CaptureRequirement(5012, @"[In FSCTL_GET_NTFS_VOLUME_DATA] On completion, the object store MUST return:
                            [Status,OutputBuffer,BytesReturned].");
                        Helper.CaptureRequirement(5034, @"[In FSCTL_GET_NTFS_VOLUME_DATA,Pseudocode for the operation is as follows:] 
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        return MessageStatus.SUCCESS;
                    }
                #endregion

                #region 3.1.5.9.8  SET_OBJECT_ID

                //3.1.5.9.8 SET_OBJECT_ID
                case FsControlRequestType.SET_OBJECT_ID:
                    {
                        if (!isObjectImplementedFunctionality)
                        {
                            Helper.CaptureRequirement(5036, @"[In FSCTL_GET_OBJECT_ID]If the object store does not implement this functionality, 
                                the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<29>");
                            return MessageStatus.INVALID_DEVICE_REQUEST;
                        }
                        if (!isObjectIDsSupportedTrue)
                        {
                            Helper.CaptureRequirement(5037, @"[In FSCTL_GET_OBJECT_ID]Pseudocode for the operation is as follows:
                                If Open.File.Volume.IsObjectIDsSupported is FALSE, the operation MUST be failed with STATUS_VOLUME_NOT_UPGRADED.");
                            return MessageStatus.STATUS_VOLUME_NOT_UPGRADED;
                        }
                        if (bufferSize == BufferSize.LessThanFILE_OBJECTID_BUFFER)
                        {
                            Helper.CaptureRequirement(1061, @"[In FSCTL_GET_OBJECT_ID,Pseudocode for the operation is as follows:]
                                If OutputBufferSize is less than sizeof( FILE_OBJECTID_BUFFER ), the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                            return MessageStatus.INVALID_PARAMETER;
                        }

                        Helper.CaptureRequirement(1069, @"[In FSCTL_GET_OBJECT_ID,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        isBytesReturnedSet = true;
                        Helper.CaptureRequirement(1057, @"[In FSCTL_GET_OBJECT_ID]On completion, the object store MUST return:
                            [Status, OutputBuffer,BytesReturned].");
                        Helper.CaptureRequirement(1068, @"[In FSCTL_GET_OBJECT_ID,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return: BytesReturned set to sizeof ( FILE_OBJECTID_BUFFER ).");
                        return MessageStatus.SUCCESS;
                    }
                #endregion

                #region 3.1.5.9.14 FSCTL_QUERY_FAT_BPB
                case FsControlRequestType.QUERY_FAT_BPB:
                    {
                        if (!IsImplement_FSCTL_QUERY_FAT_BPB)
                        {
                            Helper.CaptureRequirement(1117, @"[ In FSCTL_QUERY_FAT_BPB]If this operation is not supported, 
                                this operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<31>");
                            Helper.CaptureRequirement(5944, @"[ In FSCTL_QUERY_FAT_BPB]implement this functionality, 
                                the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<32>");
                            return MessageStatus.INVALID_DEVICE_REQUEST;
                        }

                        if (bufferSize == BufferSize.LessThan0x24)
                        {
                            Helper.CaptureRequirement(1124, @"[ In FSCTL_QUERY_FAT_BPB]Pseudocode for the operation is as follows:
                                If OutputBufferSize is less than 0x24, the operation MUST be failed with STATUS_BUFFER_TOO_SMALL.");
                            return MessageStatus.BUFFER_TOO_SMALL;
                        }

                        isBytesReturnedSet = true;
                        Helper.CaptureRequirement(1126, @"[ In FSCTL_QUERY_FAT_BPB,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:BytesReturned set to 0x24.");
                        Helper.CaptureRequirement(5043, @"[ In FSCTL_QUERY_FAT_BPB,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:The operation returns STATUS_SUCCESS.");
                        Helper.CaptureRequirement(1121, @"[ In FSCTL_QUERY_FAT_BPB]On completion, the object store MUST return:
                            [Status,OutputBuffer,BytesReturned ].");
                        return MessageStatus.SUCCESS;
                    }
                #endregion

                #region 3.1.5.9.16  QUERY_ON_DISK_VOLUME_INFO

                case FsControlRequestType.QUERY_ON_DISK_VOLUME_INFO:
                    {
                        if (!IsImplement_FSCTL_QUERY_ON_DISK_VOLUME_INFO)
                        {
                            Helper.CaptureRequirement(5508, @"[In FSCTL_QUERY_ON_DISK_VOLUME_INFO]If the object store does not implement this functionality, 
                                the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<34>");
                            return MessageStatus.INVALID_DEVICE_REQUEST;
                        }
                        if (bufferSize == BufferSize.LessThanFILE_QUERY_ON_DISK_VOL_INFO_BUFFER)
                        {
                            Helper.CaptureRequirement(5509, @"[In FSCTL_QUERY_ON_DISK_VOLUME_INFO]Pseudocode for the operation is as follows:
                                If OutputBufferSize is less than sizeof( FILE_QUERY_ON_DISK_VOL_INFO_BUFFER ), the operation MUST be failed with STATUS_BUFFER_TOO_SMALL.");
                            return MessageStatus.BUFFER_TOO_SMALL;
                        }

                        isBytesReturnedSet = true;
                        Helper.CaptureRequirement(5521, @"[In FSCTL_QUERY_ON_DISK_VOLUME_INFO,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:BytesReturned set to sizeof( FILE_QUERY_ON_DISK_VOL_INFO_BUFFER ).");
                        Helper.CaptureRequirement(5503, @"[In FSCTL_QUERY_ON_DISK_VOLUME_INFO]On completion, the object store MUST return:
                            [Status,OutputBuffer,BytesReturned].");
                        Helper.CaptureRequirement(5522, @"[In FSCTL_QUERY_ON_DISK_VOLUME_INFO,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        return MessageStatus.SUCCESS;

                    }
                #endregion

                #region 3.1.5.9.17  FSCTL_QUERY_SPARING_INFO

                case FsControlRequestType.QUERY_SPARING_INFO:
                    {
                        if (!IsImplement_FSCTL_QUERY_SPARING_INFO)
                        {
                            Helper.CaptureRequirement(7850, @"[In FSCTL_QUERY_SPARING_INFO]If the object store does not implement this functionality, 
                                the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<35>");
                            return MessageStatus.INVALID_DEVICE_REQUEST;
                        }
                        if (bufferSize == BufferSize.LessThanFILE_QUERY_SPARING_BUFFER)
                        {
                            Helper.CaptureRequirement(3851, @"[In FSCTL_QUERY_SPARING_INFO]Pseudocode for the operation is as follows:
                                If OutputBufferSize is less than sizeof( FILE_QUERY_SPARING_BUFFER ), the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                            return MessageStatus.INVALID_PARAMETER;
                        }

                        isBytesReturnedSet = true;
                        Helper.CaptureRequirement(7934, @"[In FSCTL_QUERY_SPARING_INFO,Pseudocode for the operation is as follows:] 
                            Upon successful completion of the operation, the object store MUST return: BytesReturned set to sizeof(: FILE_QUERY_SPARING_BUFFER ).");
                        Helper.CaptureRequirement(3847, @"[In FSCTL_QUERY_SPARING_INFO]On completion, the object store MUST return:[Status,OutputBuffer].");
                        Helper.CaptureRequirement(3856, @"[In FSCTL_QUERY_SPARING_INFO,Pseudocode for the operation is as follows:
                            Upon successful completion of the operation, the object store MUST return:] Status set to STATUS_SUCCESS.");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region 3.1.5.9.19    FSCTL_RECALL_FILE

                case FsControlRequestType.RECALL_FILE:
                    {
                        if (!isObjectImplementedFunctionality)
                        {
                            Helper.CaptureRequirement(1132, @"[ In FSCTL_RECALL_FILE] If the object store does not implement this functionality, 
                                the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<37>");
                            return MessageStatus.INVALID_DEVICE_REQUEST;
                        }
                        Helper.CaptureRequirement(1263, @"[ In FSCTL_RECALL_FILE]On completion, the object store MUST return:[Status].");
                        Helper.CaptureRequirement(1136, @"[ In FSCTL_RECALL_FILE,If Open.File.FileAttributes.FILE_ATTRIBUTE_OFFLINE is set]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region 3.1.5.9.27 FSCTL_SET_SHORT_NAME_BEHAVIOR
                case FsControlRequestType.FSCTL_SET_SHORT_NAME_BEHAVIOR:
                    {
                        Requirement.Capture(@"[2.1.5.9.34   FSCTL_SET_SHORT_NAME_BEHAVIOR]
                                This control code is reserved for the WinPE <95> environment; the object store MUST return STATUS_INVALID_DEVICE_REQUEST.");
                        return MessageStatus.INVALID_DEVICE_REQUEST;
                    }
                #endregion

                #region 3.1.5.9.29    FSCTL_SET_ZERO_ON_DEALLOCATION

                case FsControlRequestType.SET_ZERO_ON_DEALLOCATION:
                    {
                        if (!IsImplement_SET_ZERO_ON_DEALLOCATION)
                        {
                            Helper.CaptureRequirement(4721, @"[In FSCTL_SET_ZERO_ON_DEALLOCATION] If the object store does not implement this functionality, 
                                the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<51>");
                            return MessageStatus.INVALID_DEVICE_REQUEST;
                        }

                        isBytesReturnedSet = true;
                        isOutputBufferSizeReturn = true;
                        Helper.CaptureRequirement(1394, @"[In FSCTL_SET_ZERO_ON_DEALLOCATION,Pseudocode for the operation is as follows:]
                            TUpon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        Helper.CaptureRequirement(1388, @"[In FSCTL_SET_ZERO_ON_DEALLOCATION]On completion the object store MUST return:[Status].");
                        return MessageStatus.SUCCESS;
                    }

                #endregion

                #region    3.1.5.9.31    FSCTL_WRITE_USN_CLOSE_RECORD

                case FsControlRequestType.WRITE_USN_CLOSE_RECORD:
                    {
                        if (!isObjectImplementedFunctionality)
                        {
                            Helper.CaptureRequirement(3959, @"[In FSCTL_WRITE_USN_CLOSE_RECORD] If the object store does not implement this functionality, 
                                the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<56>");
                            return MessageStatus.INVALID_DEVICE_REQUEST;
                        }
                        // If Open.File.Volume.IsReadOnly is true
                        if (isFileVolumeReadOnly)
                        {
                            Helper.CaptureRequirement(3960, @"[In FSCTL_WRITE_USN_CLOSE_RECORD]Pseudocode for the operation is as follows:
                                If Open.File.Volume.IsReadOnly is TRUE, the operation MUST be failed with STATUS_MEDIA_WRITE_PROTECTED.");
                            return MessageStatus.MEDIA_WRITE_PROTECTED;
                        }
                        if (bufferSize == BufferSize.LessThanSizeofUsn)
                        {
                            Helper.CaptureRequirement(3961, @"[In FSCTL_WRITE_USN_CLOSE_RECORD,Pseudocode for the operation is as follows:]
                                If OutputBufferSize is less than sizeof( Usn ), the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                            return MessageStatus.INVALID_PARAMETER;
                        }
                        // If Open.File.Volume.IsUsnActive is FALSE
                        if (!isFileVolumeUsnJournalActive)
                        {
                            Helper.CaptureRequirement(3962, @"[In FSCTL_WRITE_USN_CLOSE_RECORD,Pseudocode for the operation is as follows:]
                                If Open.File.Volume.IsUsnJournalActive is FALSE, the operation MUST be failed with STATUS_JOURNAL_NOT_ACTIVE.");
                            return MessageStatus.JOURNAL_NOT_ACTIVE;
                        }

                        isBytesReturnedSet = true;
                        isOutputBufferSizeReturn = true;
                        Helper.CaptureRequirement(3965, @"[In FSCTL_WRITE_USN_CLOSE_RECORD,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:BytesReturned set to sizeof( Usn ).");
                        Helper.CaptureRequirement(3955, @"[In FSCTL_WRITE_USN_CLOSE_RECORD]On completion, the object store MUST return:
                            [Status,OutputBuffer,BytesReturned].");
                        Helper.CaptureRequirement(3966, @"[In FSCTL_WRITE_USN_CLOSE_RECORD,Pseudocode for the operation is as follows:]
                            Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
                        return MessageStatus.SUCCESS;
                    }
                #endregion
                default:
                    return MessageStatus.SUCCESS;
            }
        }

        #endregion

        #region 3.1.5.9.9  FSCTL_GET_REPARSE_POINT

        /// <summary>
        /// 3.1.5.9.9    FSCTL_GET_REPARSE_POINT
        /// </summary>
        /// <param name="bufferSize">To indicate buffer size</param>
        /// <param name="isBytesReturnedSet">The value is true if BytesReturned is set</param>
        /// <param name="openFileReparseTag">To indicate ReparseTag</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus FsCtlGetReparsePoint(
            BufferSize bufferSize,
            ReparseTag openFileReparseTag,
            out bool isBytesReturnedSet
            )
        {
            Condition.IsTrue(bufferSize == BufferSize.LessThanREPARSE_DATA_BUFFER || bufferSize == BufferSize.LessThanREPARSE_GUID_DATA_BUFFER || bufferSize == BufferSize.BufferSizeSuccess);
            Condition.IsTrue(openFileReparseTag == ReparseTag.EMPTY ||
                               openFileReparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG ||
                               openFileReparseTag == ReparseTag.Initialize);
            isBytesReturnedSet = false;

            if (!isObjectImplementedFunctionality)
            {
                Helper.CaptureRequirement(5039, @"[In FSCTL_GET_REPARSE_POINT] If the object store does not implement this functionality, 
                    the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<30>");
                return MessageStatus.INVALID_DEVICE_REQUEST;
            }

            if (!isReparsePointsSupportedTrue)
            {
                Helper.CaptureRequirement(5040, @"[In FSCTL_GET_REPARSE_POINT] Pseudocode for the operation is as follows:
                    If Open.File.Volume.IsReparsePointsSupported is FALSE, the operation MUST be failed with STATUS_VOLUME_NOT_UPGRADED.");
                return MessageStatus.STATUS_VOLUME_NOT_UPGRADED;
            }
            //    If Open.File.ReparseTag is empty
            if (openFileReparseTag == ReparseTag.EMPTY)
            {
                Helper.CaptureRequirement(1077, @"[In FSCTL_GET_REPARSE_POINT,Pseudocode for the operation is as follows:
                    Phase 1 : Check whether there is a reparse point on the File]If Open.File.ReparseTag is empty, 
                    the operation MUST be failed with STATUS_NOT_A_REPARSE_POINT.");
                return MessageStatus.NOT_A_REPARSE_POINT;
            }

            //Verify that OutputBufferSize is large enough to contain the reparse point data header
            //If Open.File.ReparseTag is a Microsoft reparse tag as defined in [MS-FSCC] section 2.1.2.1
            //If OutputBufferSize is small than the size of REPARSE_DATA_BUFFER
            if ((openFileReparseTag != ReparseTag.NON_MICROSOFT_RANGE_TAG) && (BufferSize.LessThanREPARSE_DATA_BUFFER == bufferSize))
            {
                Helper.CaptureRequirement(1080, @"[In FSCTL_GET_REPARSE_POINT,Pseudocode for the operation is as follows:
                Phase 2 - Verify that OutputBufferSize is large enough to contain the reparse point data header.]
                If it[ Open.File.ReparseTag] is not, the operation MUST be failed with STATUS_BUFFER_TOO_SMALL.");
                return MessageStatus.BUFFER_TOO_SMALL;
            }

            //If Open.File.ReparseTag is a non-Microsoft reparse tag
            //If OutputBufferSize is small than the size of REPARSE_GUID_DATA_BUFFER
            if ((openFileReparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG) && (BufferSize.LessThanREPARSE_GUID_DATA_BUFFER == bufferSize))
            {
                Helper.CaptureRequirement(1083, @"[In FSCTL_GET_REPARSE_POINT,Pseudocode for the operation is as follows:
                    Phase 2 : Verify that OutputBufferSize is large enough to contain the reparse point data header.]
                    If it[Open.File.ReparseTag] is not, the operation MUST be failed with STATUS_BUFFER TOO_SMALL.");
                return MessageStatus.BUFFER_TOO_SMALL;
            }

            isBytesReturnedSet = true;
            Helper.CaptureRequirement(1090, @"[In FSCTL_GET_REPARSE_POINT,Pseudocode for the operation is as follows:
                Phase 3 : Return the reparse data]Upon successful completion of the operation, the object store MUST return:BytesReturned set to the number of bytes written to OutputBuffer.");
            Helper.CaptureRequirement(1073, @"[In FSCTL_GET_REPARSE_POINT]On completion, the object store MUST return:[OutputBuffer ,BytesCopied,Status ].");
            Helper.CaptureRequirement(1091, @"[In FSCTL_GET_REPARSE_POINT,Pseudocode for the operation is as follows:Phase 3 : Return the reparse data]Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.9.10  FSCTL_GET_RETRIEVAL_POINTERS

        /// <summary>
        /// 3.1.5.9.10  FSCTL_GET_RETRIEVAL_POINTERS
        /// </summary>
        /// <param name="bufferSize">To indicate buffer size</param>
        /// <param name="isBytesReturnedSet">The value is true if BytesReturned is set</param>
        /// <param name="isElementsNotAllCopied">The value is true if elements not all copied.</param>
        /// <param name="isStartingVcnGreatThanAllocationSize">The value is true if startingVcn is great than allocation size</param>
        /// <param name="isStartingVcnNegative">The value is true if startingVcn is negative</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus FsCtlGetRetrivalPoints(
            BufferSize bufferSize,
            bool isStartingVcnNegative,
            bool isStartingVcnGreatThanAllocationSize,
            bool isElementsNotAllCopied,
            out bool isBytesReturnedSet
            )
        {
            Condition.IsTrue(bufferSize == BufferSize.LessThanSTARTING_VCN_INPUT_BUFFER ||
                bufferSize == BufferSize.LessThanRETRIEVAL_POINTERS_BUFFER ||
                bufferSize == BufferSize.BufferSizeSuccess);
            isBytesReturnedSet = false;

            // If the size of StartingVcnBuffer is less than the size of a STARTING_VCN_INPUT_BUFFER in bytes
            if (bufferSize == BufferSize.LessThanSTARTING_VCN_INPUT_BUFFER)
            {
                Helper.CaptureRequirement(1100, @"[In FSCTL_GET_RETRIEVAL_POINTERS,Pseudocode for the operation is as follows:
                    Phase 1 - Verify Parameters]If the size of StartingVcnBuffer is less than sizeof ( STARTING_VCN_INPUT_BUFFER ),
                    the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If OutputBufferSize is smaller than the size of RETRIEVAL_POINTERS_BUFFER
            if (bufferSize == BufferSize.LessThanRETRIEVAL_POINTERS_BUFFER)
            {
                Helper.CaptureRequirement(1101, @"[In FSCTL_GET_RETRIEVAL_POINTERS,Pseudocode for the operation is as follows:
                    Phase 1 - Verify Parameters]If OutputBufferSize is smaller than sizeof( RETRIEVAL_POINTERS_BUFFER ), 
                    the operation MUST be failed with STATUS_BUFFER_TOO_SMALL.");
                return MessageStatus.BUFFER_TOO_SMALL;
            }

            //If StartingVcnBuffer.StartingVcn is negative
            if (isStartingVcnNegative)
            {
                if (!isStartingVcnGreatThanAllocationSize)
                {
                    Helper.CaptureRequirement(1103, @"[In FSCTL_GET_RETRIEVAL_POINTERS,Pseudocode for the operation is as follows:
                        Phase 1 - Verify Parameters]If StartingVcnBuffer.StartingVcn is negative, 
                        the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                    return MessageStatus.INVALID_PARAMETER;
                }
            }

            //If StartingVcnBuffer.StartingVcn is greater than or equal to Open.Stream.AllocationSize divided by Open.File.Volume.ClusterSize
            if (isStartingVcnGreatThanAllocationSize)
            {
                Helper.CaptureRequirement(1104, @"[In FSCTL_GET_RETRIEVAL_POINTERS,Pseudocode for the operation is as follows:
                    Phase 1 - Verify Parameters]If StartingVcnBuffer.StartingVcn is greater than 
                    or equal to Open.Stream.AllocationSize divided by Open.File.Volume.ClusterSize, the operation MUST be failed with STATUS_END_OF_FILE.");
                return MessageStatus.END_OF_FILE;
            }

            //If not all of the elements in Open.Stream.ExtentList were copied into OutputBuffer.Extents
            if (isElementsNotAllCopied)
            {
                return MessageStatus.BUFFER_OVERFLOW;
            }
            isBytesReturnedSet = true;
            Helper.CaptureRequirement(1112, @"Locate and copy the extents into OutputBuffer.]Upon successful completion of the operation, 
                the object store MUST return: BytesReturned set to the number of bytes written to OutputBuffer.");
            Helper.CaptureRequirement(1096, @"[In FSCTL_GET_RETRIEVAL_POINTERS]On completion, the object store MUST return:
                [OutputBuffer,BytesCopied,Status].");
            Helper.CaptureRequirement(5041, @"[In FSCTL_GET_RETRIEVAL_POINTERS,Pseudocode for the operation is as follows:
                Phase 2 - Locate and copy the extents into OutputBuffer.]Upon successful completion of the operation, 
                the object store MUST return:Status set to STATUS_SUCCESS or STATUS_BUFFER_OVERFLOW.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.9.15  FSCTL_QUERY_ALLOCATED_RANGES

        /// <summary>
        /// 3.1.5.9.15    FSCTL_QUERY_ALLOCATED_RANGES
        /// </summary>
        /// <param name="bufferSize">To indicate buffer size</param>
        /// <param name="inputBuffer">To indicate input buffer</param>
        /// <param name="isBytesReturnedSet">The value is true if BytesReturned is set</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus FsCtlQueryAllocatedRanges(
            BufferSize bufferSize,
            BufferLength inputBuffer,
            out bool isBytesReturnedSet
            )
        {
            Condition.IsTrue(bufferSize == BufferSize.LessThanFILE_ALLOCATED_RANGE_BUFFER || bufferSize == BufferSize.OutLessThanFILE_ALLOCATED_RANGE_BUFFER || bufferSize == BufferSize.BufferSizeSuccess);
            isBytesReturnedSet = false;

            if (!isObjectImplementedFunctionality)
            {
                Helper.CaptureRequirement(5045, @"[In FSCTL_QUERY_ALLOCATED_RANGES]If the object store does not implement this functionality, 
                    the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<33>");
                return MessageStatus.INVALID_DEVICE_REQUEST;
            }
            // If OutputBufferSize < sizeof( FILE_ALLOCATED_RANGE_BUFFER )
            if (bufferSize == BufferSize.OutLessThanFILE_ALLOCATED_RANGE_BUFFER)
            {
                Helper.CaptureRequirement(3778, @"[In FSCTL_QUERY_ALLOCATED_RANGES,Pseudocode for the operation is as follows:]
                    If OutputBufferSize < sizeof( FILE_ALLOCATED_RANGE_BUFFER ), the operation MUST be failed with STATUS_BUFFER_TOO_SMALL.");
                return MessageStatus.BUFFER_TOO_SMALL;
            }
            // If InputBufferSize is less than the size of FILE_ALLOCATED_RANGE_BUFFER
            if (bufferSize == BufferSize.LessThanFILE_ALLOCATED_RANGE_BUFFER)
            {
                Helper.CaptureRequirement(3771, @"[In FSCTL_QUERY_ALLOCATED_RANGES,Pseudocode for the operation is as follows:]
                    If InputBufferSize is less than sizeof( FILE_ALLOCATED_RANGE_BUFFER ), the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                return MessageStatus.INVALID_PARAMETER;
            }

            switch (inputBuffer)
            {
                case BufferLength.EqualZero:
                    {
                        Helper.CaptureRequirement(3777, @"[In FSCTL_QUERY_ALLOCATED_RANGES,Pseudocode for the operation is as follows:]
                            If InputBuffer.Length is 0:Return STATUS_SUCCESS.");
                        break;
                    }
                case BufferLength.LessThanZero:
                    {
                        Helper.CaptureRequirement(3774, @"[In FSCTL_QUERY_ALLOCATED_RANGES,Pseudocode for the operation is as follows:]
                            If (InputBuffer.Length < 0), the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                        return MessageStatus.INVALID_PARAMETER;

                    }
                case BufferLength.MoreThanMAXLONGLONG:
                    {
                        Helper.CaptureRequirement(3775, @"[In FSCTL_QUERY_ALLOCATED_RANGES,Pseudocode for the operation is as follows:]
                           If  (InputBuffer.Length > MAXLONGLONG - InputBuffer.FileOffset), the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                        return MessageStatus.INVALID_PARAMETER;

                    }
                case BufferLength.FileOffsetLessThanZero:
                    {
                        Helper.CaptureRequirement(3773, @"[In FSCTL_QUERY_ALLOCATED_RANGES,Pseudocode for the operation is as follows:]
                            If (InputBuffer.FileOffset < 0) , the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                        return MessageStatus.INVALID_PARAMETER;

                    }
            }
            isBytesReturnedSet = true;
            Helper.CaptureRequirement(3765, @"[In FSCTL_QUERY_ALLOCATED_RANGES]On completion, the object store MUST return:
                [Status ,OutputBuffer,BytesReturned ].");
            Helper.CaptureRequirement(3782, @"[In FSCTL_QUERY_ALLOCATED_RANGES,Pseudocode for the operation is as follows:
                If Open.Stream.IsSparse is FALSE:]Return STATUS_SUCCESS.");
            Helper.CaptureRequirement(5047, @"[In FSCTL_QUERY_ALLOCATED_RANGES,Pseudocode for the operation is as follows:]
                Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.9.18  FSCTL_READ_FILE_USN_DATA

        /// <summary>
        /// 3.1.5.9.18    FSCTL_READ_FILE_USN_DATA
        /// </summary>
        ///<param name="bufferSize">To indicate buffer size</param>
        ///<param name="isBytesReturnedSet">The value is true if BytesReturned is set </param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus FsCtlReadFileUSNData(
            BufferSize bufferSize,
            out bool isBytesReturnedSet
            )
        {
            Condition.IsTrue(bufferSize == BufferSize.LessThanUSN_RECORD ||
                               bufferSize == BufferSize.LessThanRecordLength ||
                               bufferSize == BufferSize.BufferSizeSuccess);
            isBytesReturnedSet = false;
            if (!isObjectImplementedFunctionality)
            {
                Helper.CaptureRequirement(7936, @"[In FSCTL_READ_FILE_USN_DATA] If the object store does not implement this functionality, 
                    the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST. <36>");
                return MessageStatus.INVALID_DEVICE_REQUEST;
            }
            // If OutputBufferSize is less than the size of USN_RECORD in bytes
            if (bufferSize == BufferSize.LessThanUSN_RECORD)
            {
                Helper.CaptureRequirement(3866, @"[In FSCTL_READ_FILE_USN_DATA] Pseudocode for the operation is as follows:
                    If OutputBufferSize is less than sizeof( USN_RECORD ), the operation MUST be failed with STATUS_BUFFER_TOO_SMALL.");
                return MessageStatus.BUFFER_TOO_SMALL;
            }

            if (bufferSize == BufferSize.LessThanRecordLength)
            {
                Helper.CaptureRequirement(3873, @"[In FSCTL_READ_FILE_USN_DATA]If OutputBufferSize is less than RecordLength, 
                    the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
                return MessageStatus.INFO_LENGTH_MISMATCH;
            }
            isBytesReturnedSet = true;
            Helper.CaptureRequirement(3888, @"[In FSCTL_READ_FILE_USN_DATA]Upon successful completion of the operation, 
                the object store MUST return:BytesReturned set to RecordLength.");
            Helper.CaptureRequirement(3860, @"[In FSCTL_READ_FILE_USN_DATA]On completion, the object store MUST return:
                [Status ,OutputBuffer ,BytesReturned].");
            Helper.CaptureRequirement(3889, @"[In FSCTL_READ_FILE_USN_DATA]Upon successful completion of the operation, 
                the object store MUST return:Status set to STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.9.20  FSCTL_SET_COMPRESSION

        /// <summary>
        /// 3.1.5.9.20    FSCTL_SET_COMPRESSION
        /// </summary>
        /// <param name="bufferSize">To indicate buffer size</param>
        /// <param name="compressionState">To indicate InputBuffer compression state</param>
        /// <param name="isCompressionSupportEnabled">The value is true if compression support enabled</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus FsCtlSetCompression(
            BufferSize bufferSize,
            InputBufferCompressionState compressionState,
            bool isCompressionSupportEnabled
            )
        {
            Condition.IsTrue(bufferSize == BufferSize.LessThanTwoBytes || bufferSize == BufferSize.BufferSizeSuccess);
            if (!isObjectImplementedFunctionality)
            {
                Helper.CaptureRequirement(7732, @"[ In FSCTL_SET_COMPRESSION]On completion, the object store MUST return:[Status].");
                return MessageStatus.INVALID_DEVICE_REQUEST;
            }

            if (bufferSize == BufferSize.LessThanTwoBytes)
            {
                Helper.CaptureRequirement(1142, @"[ In FSCTL_SET_COMPRESSION]The operation MUST be failed with STATUS_INVALID_PARAMETER 
                    under any of the following conditions:InputBufferSize is less than sizeof( USHORT ) (2 bytes).");
                return MessageStatus.INVALID_PARAMETER;
            }

            if (compressionState == InputBufferCompressionState.NotPrefinedValue)
            {
                Helper.CaptureRequirement(1143, @"[ In FSCTL_SET_COMPRESSION]The operation MUST be failed with STATUS_INVALID_PARAMETER 
                    under any of the following conditions:InputBuffer.CompressionState is not one of the predefined values in [MS-FSCC] section 2.3.45.");
                return MessageStatus.INVALID_PARAMETER;
            }

            if (compressionState != InputBufferCompressionState.COMPRESSION_FORMAT_NONE && !isCompressionSupportEnabled)
            {
                Helper.CaptureRequirement(1144, @"[ In FSCTL_SET_COMPRESSION]Pseudocode for the operation is as follows:
                    If InputBuffer.CompressionState != COMPRESSION_FORMAT_NONE:If compression support is disabled in the object store<39>, 
                    the operation MUST be failed with STATUS_COMPRESSION_DISABLED.");
                return MessageStatus.COMPRESSION_DISABLED;
            }

            Helper.CaptureRequirement(1266, @"[ In FSCTL_SET_COMPRESSION]On completion, the object store MUST return:[Status].");
            Helper.CaptureRequirement(1724, @"[ In FSCTL_SET_COMPRESSION]If (InputBuffer.CompressionState == COMPRESSION_FORMAT_NONE 
                and Open.Stream.IsCompressed is FALSE),the operation MUST immediately return STATUS_SUCCESS.");
            Helper.CaptureRequirement(1725, @"[ In FSCTL_SET_COMPRESSION]If (InputBuffer.CompressionState != COMPRESSION_FORMAT_NONE 
                and Open.Stream.IsCompressed is TRUE), the operation MUST immediately return STATUS_SUCCESS.");
            Helper.CaptureRequirement(1169, @"[ In FSCTL_SET_COMPRESSION]Upon successful completion of the operation, 
                the object store MUST return:[ Status set to STATUS_SUCCESS].");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.9.21  FSCTL_SET_DEFECT_MANAGEMENT

        /// <summary>
        /// 3.1.5.9.21    FSCTL_SET_DEFECT_MANAGEMENT
        /// </summary>
        /// <param name="bufferSize">To indicate buffer size</param>
        /// <param name="isOpenListContainMoreThanOneOpen">The value is true if OpenList contain more than one open</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        /// Disable warning CA1801, because the parameter of "capabilities" is used for extend the model logic, 
        /// which will affect the implementation of the model if it is removed.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        [Rule]
        public static MessageStatus FsCtlSetDefectManagement(
            BufferSize bufferSize,
            bool isOpenListContainMoreThanOneOpen
            )
        {
            Condition.IsTrue(bufferSize == BufferSize.LessThanOneBytes || bufferSize == BufferSize.BufferSizeSuccess);

            if (isObjectImplementedFunctionality)
            {
                Helper.CaptureRequirement(4147, @"[ In FSCTL_SET_DEFECT_MANAGEMENT] If the object store does not implement this functionality 
                    or the target media is not a software defect-managed media, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<40>");
                return MessageStatus.INVALID_DEVICE_REQUEST;
            }

            if (bufferSize == BufferSize.LessThanOneBytes)
            {
                Helper.CaptureRequirement(1178, @"[ In FSCTL_SET_DEFECT_MANAGEMENT, Pseudocode for the operation is as follows]
                    If InputBufferSize is less than sizeof( Boolean ) (1 byte), the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                return MessageStatus.INVALID_PARAMETER;
            }

            Helper.CaptureRequirement(1182, @"[ In FSCTL_SET_DEFECT_MANAGEMENT]Upon successful completion of the operation, 
                the object store MUST return:Status set to STATUS_SUCCESS.");
            Helper.CaptureRequirement(1176, @"[ In FSCTL_SET_DEFECT_MANAGEMENT]On completion, the object store MUST return:Status: 
                An NTSTATUS code that specifies the result.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.9.22  FSCTL_SET_ENCRYPTION

        /// <summary>
        /// 3.1.5.9.22    FSCTL_SET_ENCRYPTION
        /// </summary>
        /// <param name="bufferSize">To indicate buffer size</param>
        /// <param name="encryptionOpteration">To indicate EncryptionOperation</param>
        /// <param name="isIsCompressedTrue">The value is true if IsCompressed is true</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus FsCtlSetEncryption(
            bool isIsCompressedTrue,
            EncryptionOperation encryptionOpteration,
            BufferSize bufferSize
            )
        {
            Condition.IsTrue(bufferSize == BufferSize.LessThanENCRYPTION_BUFFER || bufferSize == BufferSize.BufferSizeSuccess);

            if (!isObjectImplementedFunctionality)
            {
                Helper.CaptureRequirement(3891, @"[In FSCTL_SET_ENCRYPTION]If the object store does not implement encryption, 
                    the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<41>");
                return MessageStatus.INVALID_DEVICE_REQUEST;
            }

            // If InputBufferSize is smaller than BlockAlign( sizeof( ENCRYPTION_BUFFER ), 4 ), 
            if (bufferSize == BufferSize.LessThanENCRYPTION_BUFFER)
            {
                Helper.CaptureRequirement(3899, @"[In FSCTL_SET_ENCRYPTION]If InputBufferSize is smaller than BlockAlign
                    ( sizeof( ENCRYPTION_BUFFER ), 4 ), the operation MUST be failed with STATUS_BUFFER_TOO_SMALL.");
                return MessageStatus.BUFFER_TOO_SMALL;
            }

            // If InputBuffer.EncryptionOperation is not one of the predefined values in [MS-FSCC] section 2.3.47
            if (encryptionOpteration == EncryptionOperation.NOT_VALID_IN_FSCC)
            {
                Helper.CaptureRequirement(3900, @"[In FSCTL_SET_ENCRYPTION]The operation MUST be failed with STATUS_INVALID_PARAMETER 
                    under any of the following conditions:If InputBuffer.EncryptionOperation is not one of the predefined values in [MS-FSCC] section 2.3.47.");
                return MessageStatus.INVALID_PARAMETER;
            }

            // If InputBuffer.EncryptionOperation == STREAM_SET_ENCRYPTION and Open.Stream.IsCompressed is true.
            if ((encryptionOpteration == EncryptionOperation.STREAM_SET_ENCRYPTION) && isIsCompressedTrue)
            {
                Helper.CaptureRequirement(3901, @"[In FSCTL_SET_ENCRYPTION]The operation MUST be failed with STATUS_INVALID_PARAMETER 
                    under any of the following conditions:If InputBuffer.EncryptionOperation == STREAM_SET_ENCRYPTION and Open.Stream.IsCompressed is TRUE.");
                return MessageStatus.INVALID_PARAMETER;
            }

            Helper.CaptureRequirement(3896, @"[In FSCTL_SET_ENCRYPTION]On completion, the object store MUST return:[Status].");
            Helper.CaptureRequirement(3919, @"[In FSCTL_SET_ENCRYPTION]Upon successful completion of this operation, 
                the object store MUST return:Status set to STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.9.23  3.1.5.9.24

        /// <summary>
        /// 3.1.5.9.23    FSCTL_SET_OBJECT_ID
        /// 3.1.5.9.24    FSCTL_SET_OBJECT_ID_EXTENDED
        /// </summary>
        /// <param name="requestType">Fscontrol request type</param>
        /// <param name="bufferSize">To indicate buffer size</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus FsCtlSetObjID(
            FsControlRequestType requestType,
            BufferSize bufferSize
            )
        {
            Condition.IsTrue(bufferSize == BufferSize.NotEqualFILE_OBJECTID_BUFFER || bufferSize == BufferSize.NotEqual48Bytes || bufferSize == BufferSize.BufferSizeSuccess);
            Condition.IsTrue(requestType == FsControlRequestType.SET_OBJECT_ID || requestType == FsControlRequestType.SET_OBJECT_ID_EXTENDED);
            switch (requestType)
            {
                case FsControlRequestType.SET_OBJECT_ID:
                    {
                        #region  23

                        if (!IsImplement_FSCTL_SET_OBJECT_ID)
                        {
                            Helper.CaptureRequirement(4326, @"[ In FSCTL_SET_OBJECT_ID] If the object store does not implement this functionality, 
                                the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<42>");
                            return MessageStatus.INVALID_DEVICE_REQUEST;
                        }

                        if (bufferSize == BufferSize.NotEqualFILE_OBJECTID_BUFFER || bufferSize == BufferSize.NotEqual48Bytes)
                        {
                            Helper.CaptureRequirement(1188, @"[ In FSCTL_SET_OBJECT_ID]Pseudocode for the operation is as follows:
                                If InputBufferSize is not equal to sizeof( FILE_OBJECTID_BUFFER ), the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                            return MessageStatus.INVALID_PARAMETER;
                        }

                        if (!gisObjectIDsSupported)
                        {
                            Helper.CaptureRequirement(4327, @"[ In FSCTL_SET_OBJECT_ID,Pseudocode for the operation is as follows:] 
                                If Open.File.Volume.IsObjectIDsSupported is FALSE, the operation MUST be failed with STATUS_VOLUME_NOT_UPGRADED.");
                            return MessageStatus.STATUS_VOLUME_NOT_UPGRADED;
                        }

                        if (!isHasRestoreAccess)
                        {
                            Helper.CaptureRequirement(1190, @"[ In FSCTL_SET_OBJECT_ID,Pseudocode for the operation is as follows:]
                                If Open.HasRestoreAccess is FALSE, the operation MUST be failed with STATUS_ACCESS_DENIED.");
                            return MessageStatus.ACCESS_DENIED;
                        }


                        Helper.CaptureRequirement(1200, @"[ In FSCTL_SET_OBJECT_ID]Upon successful completion of the operation, 
                            the object store MUST return:Status set to STATUS_SUCCESS.");
                        Helper.CaptureRequirement(1187, @"[ In FSCTL_SET_OBJECT_ID]On completion, the object store MUST return:
                            Status ¨C An NTSTATUS code that specifies the result.");
                        return MessageStatus.SUCCESS;

                        #endregion
                    }
                case FsControlRequestType.SET_OBJECT_ID_EXTENDED:
                    {
                        #region 24

                        if (!IsImplement_FSCTL_SET_OBJECT_ID)
                        {
                            Helper.CaptureRequirement(4329, @"[ In FSCTL_SET_OBJECT_ID_EXTENDED] If the object store does not implement this functionality, 
                                the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<44>");
                            return MessageStatus.INVALID_DEVICE_REQUEST;
                        }

                        if (bufferSize == BufferSize.NotEqual48Bytes)
                        {
                            Helper.CaptureRequirement(1207, @"[ In FSCTL_SET_OBJECT_ID_EXTENDED]Pseudocode for the operation is as follows:
                                If InputBufferSize is not equal to sizeof( ObjectId.ExtendedInfo ) (48 bytes), 
                                the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                            return MessageStatus.INVALID_PARAMETER;
                        }

                        if (!gisObjectIDsSupported)
                        {
                            Helper.CaptureRequirement(4327, @"[ In FSCTL_SET_OBJECT_ID,Pseudocode for the operation is as follows:] 
                                If Open.File.Volume.IsObjectIDsSupported is FALSE, the operation MUST be failed with STATUS_VOLUME_NOT_UPGRADED.");
                            return MessageStatus.STATUS_VOLUME_NOT_UPGRADED;
                        }

                        if (!isHasRestoreAccess)
                        {
                            Helper.CaptureRequirement(1190, @"[ In FSCTL_SET_OBJECT_ID,Pseudocode for the operation is as follows:]
                                If Open.HasRestoreAccess is FALSE, the operation MUST be failed with STATUS_ACCESS_DENIED.");
                            return MessageStatus.ACCESS_DENIED;
                        }

                        Helper.CaptureRequirement(1217, @"[ In FSCTL_SET_OBJECT_ID_EXTENDED]Upon successful completion of this operation, 
                            the object store MUST return:Status set to STATUS_SUCCESS.");
                        Helper.CaptureRequirement(1206, @"[ In FSCTL_SET_OBJECT_ID_EXTENDED]On completion, the object store MUST return:
                            Status ¨C An NTSTATUS code that specifies the result.");
                        return MessageStatus.SUCCESS;

                        #endregion
                    }
                default:
                    return MessageStatus.SUCCESS;
            }
        }

        #endregion

        #region 3.1.5.9.25  FSCTL_SET_REPARSE_POINT

        /// <summary>
        /// 3.1.5.9.25    FSCTL_SET_REPARSE_POINT
        /// </summary>
        /// <param name="bufferSize">To indicate buffer size</param>
        /// <param name="inputReparseTag">To indicate ReparseTag</param>
        /// <param name="isFileReparseTagNotEqualInputBufferReparseTag">The value is true if FileReparseTag not equal InputBufferReparseTag</param>
        /// <param name="isReparseGUIDNotEqual">The value is true if ReparseGUID not equal</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus FsCtlSetReparsePoint(
            ReparseTag inputReparseTag,
            BufferSize bufferSize,
            bool isReparseGUIDNotEqual,
            bool isFileReparseTagNotEqualInputBufferReparseTag
            )
        {
            Condition.IsTrue(bufferSize == BufferSize.LessThan8Bytes ||
                               bufferSize == BufferSize.NotEqualReparseDataLengthPlus24 ||
                               bufferSize == BufferSize.NotEqualReparseDataLengthPlus8 ||
                               bufferSize == BufferSize.BufferSizeSuccess);

            Condition.IsTrue(inputReparseTag == ReparseTag.MOUNT_POINT ||
                               inputReparseTag == ReparseTag.SYMLINK ||
                               inputReparseTag == ReparseTag.IO_REPARSE_TAG_RESERVED_ZERO ||
                               inputReparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG ||
                               inputReparseTag == ReparseTag.EMPTY);

            if (!isObjectImplementedFunctionality)
            {
                Helper.CaptureRequirement(4331, @"[ In FSCTL_SET_REPARSE_POINT] If the object store does not implement this functionality, 
                    the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<46>");
                return MessageStatus.INVALID_DEVICE_REQUEST;
            }

            if (!isReparsePointsSupportedTrue)
            {
                Helper.CaptureRequirement(4332, @"[ In FSCTL_SET_REPARSE_POINT,Pseudocode for the operation is as follows:] 
                    If Open.File.Volume.IsReparsePointsSupported is FALSE, the operation MUST be failed with STATUS_VOLUME_NOT_UPGRADED.");
                return MessageStatus.STATUS_VOLUME_NOT_UPGRADED;
            }

            if (bufferSize == BufferSize.LessThan8Bytes)
            {
                Helper.CaptureRequirement(1226, @"[ In FSCTL_SET_REPARSE_POINT,Pseudocode for the operation is as follows:]
                    If InputBufferSize is smaller than 8 bytes, the operation MUST be failed with STATUS_IO_REPARSE_DATA_INVALID.");
                return MessageStatus.IO_REPARSE_DATA_INVALID;
            }

            if (bufferSize == BufferSize.NotEqualReparseDataLengthPlus8 && bufferSize == BufferSize.NotEqualReparseDataLengthPlus24)
            {
                Helper.CaptureRequirement(1229, @"[ In FSCTL_SET_REPARSE_POINT,Pseudocode for the operation is as follows:] 
                    If (InputBufferSize != InputBuffer.ReparseDataLength + 8) && (InputBufferSize != InputBuffer.ReparseDataLength + 24), 
                    the operation MUST be failed with STATUS_IO_REPARSE_DATA_INVALID.");
                return MessageStatus.IO_REPARSE_DATA_INVALID;
            }

            //If InputBuffer.ReparseTag == IO_REPARSE_TAG_SYMLINK and Open.HasCreateSymLinkPrivilege 
            //is FALSE
            if ((inputReparseTag == ReparseTag.SYMLINK) && (!isHasCreateSymbolicLinkAccessTrue))
            {
                Helper.CaptureRequirement(1231, @"[ In FSCTL_SET_REPARSE_POINT,Pseudocode for the operation is as follows:]
                    If InputBuffer.ReparseTag == IO_REPARSE_TAG_SYMLINK and Open.HasCreateSymbolicLinkAccess is FALSE, 
                    the operation MUST be failed with STATUS_ACCESS_DENIED.");
                return MessageStatus.ACCESS_DENIED;
            }

            //If Open.File.ReparseTag is not empty (indicating that a reparse point is already assigned)
            if (inputReparseTag != ReparseTag.EMPTY)
            {
                //If Open.File.ReparseTag != InputBuffer.ReparseTag
                if (isFileReparseTagNotEqualInputBufferReparseTag)
                {
                    Helper.CaptureRequirement(1237, @"[ In FSCTL_SET_REPARSE_POINT,Pseudocode for the operation is as follows:
                        Phase 2 -- Update the File]If Open.File.ReparseTag is not empty (indicating that a reparse point is already assigned):
                        If Open.File.ReparseTag != InputBuffer.ReparseTag, the operation MUST be failed with STATUS_IO_REPARSE_TAG_MISMATCH.");
                    return MessageStatus.IO_REPARSE_TAG_MISMATCH;
                }

                //If Open.File.ReparseTag is a non-Microsoft tag && Open.File.ReparseGUID is not equal 
                //to InputBuffer.ReparseGUID
                if ((inputReparseTag == ReparseTag.NON_MICROSOFT_RANGE_TAG) && (!isReparseGUIDNotEqual))
                {
                    Helper.CaptureRequirement(1238, @"[ In FSCTL_SET_REPARSE_POINT,Pseudocode for the operation is as follows:
                        Phase 2 -- Update the File]If Open.File.ReparseTag is not empty (indicating that a reparse point is already assigned)
                        If Open.File.ReparseTag is a non-Microsoft tag and Open.File.ReparseGUID is not equal to InputBuffer.ReparseGUID, 
                        the operation MUST be failed with STATUS_REPARSE_ATTRIBUTE_CONFLICT.");
                    return MessageStatus.REPARSE_ATTRIBUTE_CONFLICT;
                }
            }

            Helper.CaptureRequirement(1222, @"[ In FSCTL_SET_REPARSE_POINT]On completion, the object store MUST return:Status:
                 An NTSTATUS code that specifies the result.");
            Helper.CaptureRequirement(1246, @"[In FSCTL_SET_SHORT_NAME_BEHAVIOR]the object store MUST return STATUS_INVALID_DEVICE_REQUEST.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.9.28  FSCTL_SET_ZERO_DATA

        /// <summary>
        /// 3.1.5.9.28    FSCTL_SET_ZERO_DATA
        /// </summary>
        /// <param name="bufferSize">To indicate bufferSize</param>
        /// <param name="inputBuffer">To indicate input buffer</param>        
        /// <param name="isIsDeletedTrue">The value is true if IsDeleted is true</param>
        /// <param name="isConflictDetected">The value is true if conflict is detected</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus FsCtlSetZeroData(
            BufferSize bufferSize,
            InputBuffer_FSCTL_SET_ZERO_DATA inputBuffer,
            bool isIsDeletedTrue,
            bool isConflictDetected
            )
        {
            Condition.IsTrue(bufferSize == BufferSize.LessThanFILE_ZERO_DATA_INFORMATION || bufferSize == BufferSize.BufferSizeSuccess);

            if (!isObjectImplementedFunctionality)
            {
                Helper.CaptureRequirement(4335, @"[In FSCTL_SET_ZERO_DATA] If the object store does not implement this functionality, 
                    the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.<50>");
                return MessageStatus.INVALID_DEVICE_REQUEST;
            }
            if (bufferSize == BufferSize.LessThanFILE_ZERO_DATA_INFORMATION)
            {
                Helper.CaptureRequirement(1302, @"[In FSCTL_SET_ZERO_DATA]The operation MUST be failed with STATUS_INVALID_PARAMETER 
                    under any of the following conditions:InputBufferSize is less than sizeof( FILE_ZERO_DATA_INFORMATION ).");
                return MessageStatus.INVALID_PARAMETER;
            }

            if (inputBuffer == InputBuffer_FSCTL_SET_ZERO_DATA.FileOffsetLessThanZero)
            {
                Helper.CaptureRequirement(1303, @"[In FSCTL_SET_ZERO_DATA]The operation MUST be failed with STATUS_INVALID_PARAMETER 
                    under any of the following conditions:InputBuffer.FileOffset is less than 0.");
                return MessageStatus.INVALID_PARAMETER;
            }

            if (inputBuffer == InputBuffer_FSCTL_SET_ZERO_DATA.BeyondFinalZeroLessThanZero)
            {
                Helper.CaptureRequirement(1304, @"[In FSCTL_SET_ZERO_DATA]The operation MUST be failed with STATUS_INVALID_PARAMETER 
                    under any of the following conditions:InputBuffer.BeyondFinalZero is less than 0.");
                return MessageStatus.INVALID_PARAMETER;
            }

            if (isFileVolumeReadOnly)
            {
                Helper.CaptureRequirement(1307, @"[In FSCTL_SET_ZERO_DATA]Pseudocode for the operation is as follows:
                    If Open.File.Volume.IsReadOnly is TRUE, the operation MUST be failed with STATUS_MEDIA_WRITE_PROTECTED.");
                return MessageStatus.MEDIA_WRITE_PROTECTED;
            }

            if (isIsDeletedTrue)
            {
                Helper.CaptureRequirement(1309, @"[In FSCTL_SET_ZERO_DATA,Pseudocode for the operation is as follows:]While TRUE :
                    If Open.Stream.IsDeleted is TRUE, the operation MUST be failed with STATUS_FILE_DELETED.");
                return MessageStatus.FILE_DELETED;
            }

            if (isConflictDetected)
            {
                Helper.CaptureRequirement(1316, @"[In FSCTL_SET_ZERO_DATA,Pseudocode for the operation is as follows:While TRUE :]
                    If a conflict is detected the operation MUST be failed with STATUS_FILE_LOCK_CONFLICT.");
                return MessageStatus.FILE_LOCK_CONFLICT;
            }

            Helper.CaptureRequirement(1365, @"[In FSCTL_SET_ZERO_DATA,Pseudocode for the operation is as follows:]
                Upon successful completion of the operation, the object store MUST return:Status set to STATUS_SUCCESS.");
            Helper.CaptureRequirement(1569, @"[In FSCTL_SET_ZERO_DATA]On completion, the object store MUST return[Status].");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.9.30  FSCTL_SIS_COPYFILE

        /// <summary>
        /// 3.1.5.9.30  FSCTL_SIS_COPYFILE
        /// </summary>
        /// <param name="bufferSize">To indicate input bufferSize</param>
        /// <param name="inputBuffer">To indicate input buffer</param>
        /// <param name="isCOPYFILE_SIS_LINKTrue">The parameter to indicate COPYFILE_SIS_LINK</param>
        /// <param name="isIsEncryptedTrue">The parameter to indicate IsEncrypted</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        [Rule]
        public static MessageStatus FsctlSisCopyFile(
            BufferSize bufferSize,
            InputBufferFSCTL_SIS_COPYFILE inputBuffer,
            bool isCOPYFILE_SIS_LINKTrue,
            bool isIsEncryptedTrue
            )
        {
            Condition.IsTrue(bufferSize == BufferSize.LessThanSI_COPYFILE || bufferSize == BufferSize.BufferSizeSuccess);
            if (!isObjectImplementedFunctionality)
            {
                Helper.CaptureRequirement(4732, @"[In FSCTL_SIS_COPYFILE] If the object store does not implement this functionality, 
                    the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                return MessageStatus.INVALID_DEVICE_REQUEST;
            }

            if (!isIsAdministratorTrue)
            {
                Helper.CaptureRequirement(4733, @"[In FSCTL_SIS_COPYFILE] Pseudocode for the operation is as follows:
                    If Open.IsAdministrator is FALSE, the operation MUST be failed with STATUS_ACCESS_DEFINED");
                return MessageStatus.ACCESS_DENIED;
            }

            if (isCOPYFILE_SIS_LINKTrue)
            {
                Helper.CaptureRequirement(4755, @"In FSCTL_SIS_COPYFILE,Pseudocode for the operation is as follows:
                    The operation MUST be failed with STATUS_OBJECT_TYPE_MISMATCH under any of the following conditions:]  
                    If SourceOpen.File.ReparseTag is empty and InputBuffer.Flags.COPYFILE_SIS_LINK is TRUE.");
                return MessageStatus.OBJECT_TYPE_MISMATCH;
            }

            if (bufferSize == BufferSize.LessThanSI_COPYFILE)
            {
                Helper.CaptureRequirement(4734, @"[In FSCTL_SIS_COPYFILE,Pseudocode for the operation is as follows:]
                    If InputBufferSizes is less than sizeof( SI_COPYFILE ), the operation MUST be failed with STATUS_INVALID_PARAMETER_1.");
                return MessageStatus.INVALID_PARAMETER;
            }

            if (inputBuffer == InputBufferFSCTL_SIS_COPYFILE.FlagsNotContainCOPYFILE_SIS_LINKAndCOPYFILE_SIS_REPLACE)
            {
                Helper.CaptureRequirement(4735, @"[In FSCTL_SIS_COPYFILE,Pseudocode for the operation is as follows:] 
                    If InputBuffer.Flags contains any flags besides COPYFILE_SIS_LINK and COPYFILE_SIS_REPLACE, 
                    the operation MUST be failed with STATUS_INVALID_PARAMETER_2.");
                return MessageStatus.INVALID_PARAMETER;
            }

            if (inputBuffer == InputBufferFSCTL_SIS_COPYFILE.DestinationFileNameLengthLessThanZero)
            {
                Helper.CaptureRequirement(4737, @"[In FSCTL_SIS_COPYFILE,Pseudocode for the operation is as follows:] 
                    If  InputBuffer.DestinationFileNameLength is <= zero, the operation MUST be failed with STATUS_INVALID_PARAMETER_3.");
                return MessageStatus.INVALID_PARAMETER;
            }

            if (inputBuffer == InputBufferFSCTL_SIS_COPYFILE.DestinationFileNameLengthLargeThanMAXUSHORT)
            {
                Helper.CaptureRequirement(4739, @"[In FSCTL_SIS_COPYFILE,Pseudocode for the operation is as follows:] 
                    If InputBuffer.DestinationFileNameLength is > MAXUSHORT (0xffff), the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                return MessageStatus.INVALID_PARAMETER;
            }

            if (inputBuffer == InputBufferFSCTL_SIS_COPYFILE.InputBufferSizeLessThanOtherPlus)
            {
                Helper.CaptureRequirement(4740, @"[In FSCTL_SIS_COPYFILE,Pseudocode for the operation is as follows:] 
                    If FieldOffset( InputBuffer.SourceFileName ) + InputBuffer.SourceFileNameLength + InputBuffer.DestinationFileNameLength
                    is > InputBufferSize, the operation MUST be failed with STATUS_INVALID_PARAMETER_4.");
                return MessageStatus.INVALID_PARAMETER;
            }

            if (isIsEncryptedTrue)
            {
                return MessageStatus.OBJECT_TYPE_MISMATCH;
            }

            Helper.CaptureRequirement(4726, @"[In FSCTL_SIS_COPYFILE] On completion, the object store MUST return:Status: 
                An NTSTATUS code that specifies the result.");
            Helper.CaptureRequirement(4752, @"[In FSCTL_SIS_COPYFILE,Pseudocode for the operation is as follows:] 
                If the request fails, this operation MUST be failed with the returned STATUS.");
            Helper.CaptureRequirement(4769, @"[In FSCTL_SIS_COPYFILE,Pseudocode for the operation is as follows:] 
                If the request fails, this operation MUST be failed with the returned STATUS.");
            Helper.CaptureRequirement(4773, @"[In FSCTL_SIS_COPYFILE,Pseudocode for the operation is as follows:
                The operation MUST be failed with STATUS_OBJECT_TYPE_MISMATCH under any of the following conditions:] 
                If SourceOpen.Stream.IsEncrypted is TRUE.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #endregion
    }
}
