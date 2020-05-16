// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Model
{
    /// <summary>
    /// MS-FSA model program
    /// </summary>
    public static partial class ModelProgram
    {
        #region Common Algorithms

        #region Open file initial

        /// <summary>
        ///  Open file initial
        /// </summary>      
        /// <param name="desiredAccess">Indite desiredAccess</param>
        /// <param name="shareAccess">Indicate shareAccess</param>
        /// <param name="createOption">Indicate createOption</param>
        /// <param name="createDisposition">Indicate createDisposition</param>  
        /// <param name="fileAttribute">Indicate file attribute</param>
        /// <param name="streamFoundType">Indicate if the stream is found or not.</param>
        /// <param name="symbolicLinkType">Indicate if it is symbolic link or not.</param>
        /// <param name="openFileType">Indicate OpenFfileType</param>
        /// <param name="streamTypeNameToOpen">Indicate StreamTypeNameToOpen </param>
        /// <param name="fileNameStatus">Indicate fileName status</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        /// Disable warning CA1502, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        // Disable warning CA1505, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode")]
        public static MessageStatus OpenFileinitial(
            FileAccess desiredAccess,
            ShareAccess shareAccess,
            CreateOptions createOption,
            CreateDisposition createDisposition,
            FileAttribute fileAttribute,
            StreamFoundType streamFoundType,
            SymbolicLinkType symbolicLinkType,
            StreamTypeNameToOpen streamTypeNameToOPen,
            FileType openFileType,
            FileNameStatus fileNameStatus)
        {
            #region phase 1  Parameter Validation

            //If DesiredAccess is not a valid value as specified in MS-SMB2 section 2.2.13.1
            if (desiredAccess == FileAccess.None)
            {
                Helper.CaptureRequirement(366, @"[In Application Requests an Open of a File ,Pseudocode for the operation is as follows:
                    Phase 1 - Parameter Validation:]If any of the bits in the mask 0x0CE0FE00 are set, 
                    the operation MUST be failed with STATUS_ACCESS_DENIED.");
                //If DesiredAccess is zero
                Helper.CaptureRequirement(377, @"[In Application Requests an Open of a File ,Pseudocode for the operation is as follows:
                    Phase 1 - Parameter Validation:]If DesiredAccess is zero, the operation MUST be failed with STATUS_ACCESS_DENIED.");
                return MessageStatus.ACCESS_DENIED;
            }

            //If ShareAccess is not valid values for a file object as specified in MS-SMB2 section 2.2.13
            if (shareAccess == ShareAccess.NOT_VALID_VALUE)
            {
                Helper.CaptureRequirement(2370, @"[In Application Requests an Open of a File ,Pseudocode for the operation is as follows:
                    Phase 1 - Parameter Validation:]The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If ShareAccess are not valid values for a file object as specified in [MS-SMB2] section 2.2.13.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If CreateOptions is not valid values for a file object as specified in MS-SMB2 section 2.2.13
            if (createOption == CreateOptions.NOT_VALID_VALUE)
            {
                Helper.CaptureRequirement(2371, @"[In Application Requests an Open of a File ,Pseudocode for the operation is as follows:
                    Phase 1 - Parameter Validation:]The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If CreateOptions are not valid values for a file object as specified in [MS-SMB2] section 2.2.13.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If CreateDisposition is not valid values for a file object as specified in MS-SMB2 section 2.2.13
            if (createDisposition == CreateDisposition.NOT_VALID_VALUE)
            {
                Helper.CaptureRequirement(2372, @"[In Application Requests an Open of a File ,Pseudocode for the operation is as follows:
                    Phase 1 - Parameter Validation:]The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If CreateDisposition are not valid values for a file object as specified in [MS-SMB2] section 2.2.13.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If FileAttributes is not valid values for a file object as specified in MS-SMB2 section 2.2.13
            if (fileAttribute == FileAttribute.NOT_VALID_VALUE)
            {
                Helper.CaptureRequirement(404, @"[In Application Requests an Open of a File ,Pseudocode for the operation is as follows:
                    Phase 1 - Parameter Validation:]The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If FileAttributes are not valid values for a file object as specified in [MS-SMB2] section 2.2.13.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If CreateOptions.FILE_DIRECTORY_FILE && CreateOptions.FILE_NON_DIRECTORY_FILE.
            //65 indicates CreateOptions.FILE_DIRECTORY_FILE && CreateOptions.FILE_NON_DIRECTORY_FILE
            if (createOption == (CreateOptions.DIRECTORY_FILE | CreateOptions.NON_DIRECTORY_FILE))
            {
                Helper.CaptureRequirement(368, @"[In Application Requests an Open of a File ,Pseudocode for the operation is as follows:
                    Phase 1 - Parameter Validation:]The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If CreateOptions.FILE_DIRECTORY_FILE && CreateOptions.FILE_NON_DIRECTORY_FILE.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If CreateOptions.FILE_SYNCHRONOUS_IO_ALERT && !DesiredAccess.SYNCHRONIZE.
            //updated by meying:desiredAccess == FileAccess.FILE_READ_DATA
            if (createOption == CreateOptions.SYNCHRONOUS_IO_ALERT
               && desiredAccess == FileAccess.FILE_READ_DATA)
            {
                Helper.CaptureRequirement(369, @"[In Application Requests an Open of a File ,Pseudocode for the operation is as follows:
                    Phase 1 - Parameter Validation:]The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If CreateOptions.FILE_SYNCHRONOUS_IO_ALERT && !DesiredAccess.SYNCHRONIZE.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If Create.FILE_SYNCHRONOUS_IO_NONALERT && !DesiredAccess.SYNCHRONIZE.
            //updated by meying:desiredAccess == FileAccess.FILE_READ_DATA
            if (createOption == CreateOptions.SYNCHRONOUS_IO_NONALERT
               && desiredAccess == FileAccess.FILE_READ_DATA)
            {
                Helper.CaptureRequirement(2373, @"[In Application Requests an Open of a File ,Pseudocode for the operation is as follows:
                    Phase 1 - Parameter Validation:]The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If Create.FILE_SYNCHRONOUS_IO_NONALERT&& !DesiredAccess.SYNCHRONIZE.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If CreateOptions.FILE_DELETE_ON_CLOSE && !DesiredAccess.DELETE.
            if (createOption == CreateOptions.DELETE_ON_CLOSE &&
                (desiredAccess == FileAccess.FILE_READ_DATA))
            {
                Helper.CaptureRequirement(371, @"[In Application Requests an Open of a File ,Pseudocode for the operation is as follows:
                    Phase 1 - Parameter Validation:]The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If CreateOptions.FILE_DELETE_ON_CLOSE && !DesiredAccess.DELETE.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //CreateOptions.FILE_SYNCHRONOUS_IO_ALERT && Create.FILE_SYNCHRONOUS_IO_NONALERT
            //48 indicates CreateOptions.FILE_SYNCHRONOUS_IO_ALERT && Create.FILE_SYNCHRONOUS_IO_NONALERT
            if (createOption == (CreateOptions.SYNCHRONOUS_IO_NONALERT | CreateOptions.SYNCHRONOUS_IO_ALERT))
            {
                Helper.CaptureRequirement(373, @"[In Application Requests an Open of a File ,Pseudocode for the operation is as follows:
                    Phase 1 - Parameter Validation:]The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If CreateOptions.FILE_SYNCHRONOUS_IO_ALERT && Create.FILE_SYNCHRONOUS_IO_NONALERT.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If CreateOptions.FILE_DIRECTORY_FILE && CreateDisposition == OVERWRITE.
            if (createOption == CreateOptions.DIRECTORY_FILE &&
                createDisposition == CreateDisposition.OVERWRITE)
            {
                Helper.CaptureRequirement(2375, @"[In Application Requests an Open of a File ,Pseudocode for the operation is as follows:
                    Phase 1 - Parameter Validation:]The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If CreateOptions.FILE_DIRECTORY_FILE && CreateDisposition == OVERWRITE.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If CreateOptions.FILE_DIRECTORY_FILE && CreateDisposition == NONE .
            if (createOption == CreateOptions.DIRECTORY_FILE &&
                createDisposition == CreateDisposition.SUPERSEDE)
            {
                Helper.CaptureRequirement(2374, @"[In Application Requests an Open of a File ,Pseudocode for the operation is as follows:
                    Phase 1 - Parameter Validation:]The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If CreateOptions.FILE_DIRECTORY_FILE && CreateDisposition == SUPERSEDE .");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If CreateOptions.FILE_DIRECTORY_FILE && CreateDisposition == OVERWRITE_IF.
            if (createOption == CreateOptions.DIRECTORY_FILE &&
                createDisposition == CreateDisposition.OVERWRITE_IF)
            {
                Helper.CaptureRequirement(2376, @"[In Application Requests an Open of a File ,Pseudocode for the operation is as follows:
                    Phase 1 - Parameter Validation:]The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If CreateOptions.FILE_DIRECTORY_FILE && CreateDisposition == OVERWRITE_IF).");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If CreateOptions.COMPLETE_IF_OPLOCKED && CreateOptions.FILE_RESERVE_OPFILTER
            //if ((createOption & (CreateOptions.COMPLETE_IF_OPLOCKED | CreateOptions.RESERVE_OPFILTER)) != 0)
            //1048832 indicates CreateOptions.COMPLETE_IF_OPLOCKED && CreateOptions.FILE_RESERVE_OPFILTER
            if (createOption == (CreateOptions.COMPLETE_IF_OPLOCKED | CreateOptions.RESERVE_OPFILTER))
            {
                Helper.CaptureRequirement(375, @"[In Application Requests an Open of a File ,Pseudocode for the operation is as follows:
                    Phase 1 - Parameter Validation:]The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If CreateOptions.COMPLETE_IF_OPLOCKED && CreateOptions.FILE_RESERVE_OPFILTER.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If CreateOptions.FILE_NO_INTERMEDIATE_BUFFERING && DesiredAccess.FILE_APPEND_DATA.
            if (createOption == CreateOptions.NO_INTERMEDIATE_BUFFERING &&
                desiredAccess == FileAccess.FILE_APPEND_DATA)
            {
                Helper.CaptureRequirement(376, @"[In Application Requests an Open of a File ,Phase 1 - Parameter Validation:]
                    The operation MUST be failed with STATUS_INVALID_PARAMETER under any of the following conditions:
                    If CreateOptions.FILE_NO_INTERMEDIATE_BUFFERING && DesiredAccess.FILE_APPEND_DATA.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If PathName is not valid as specified in [MS-FSCC] section 2.1.5.
            if (fileNameStatus == FileNameStatus.NotPathNameValid)
            {
                Helper.CaptureRequirement(379, @"[In Application Requests an Open of a File ,Pseudocode for the operation is as follows:
                    Phase 1 - Parameter Validation:]The operation MUST be failed with STATUS_OBJECT_NAME_INVALID under any of the following conditions:
                    If PathName is not valid as specified in [MS-FSCC] section 2.1.5.");
                return MessageStatus.OBJECT_NAME_INVALID;
            }

            //If PathName contains a trailing backslash and CreateOptions.FILE_NON_DIRECTORY_FILE is true
            if (fileNameStatus == FileNameStatus.BackslashName &&
                createOption == CreateOptions.NON_DIRECTORY_FILE && 
                desiredAccess == FileAccess.FILE_READ_DATA)
            {
                Helper.CaptureRequirement(380, @"[In Application Requests an Open of a File ,Pseudocode for the operation is as follows:
                    Phase 1 - Parameter Validation:]The operation MUST be failed with STATUS_OBJECT_NAME_INVALID under any of the following conditions:
                    If PathName contains a trailing backslash and CreateOptions.FILE_NON_DIRECTORY_FILE is TRUE.");
                return MessageStatus.OBJECT_NAME_INVALID;
            }

            #endregion

            #region phase 2  Volume State

            //If RootOpen.Volume.IsReadOnly && (CreateDisposition == FILE_CREATE) 
            //then the operation MUST be failed with STATUS_MEDIA_WRITE_PROTECTED
            if (isFileVolumeReadOnly && (createDisposition == CreateDisposition.CREATE))
            {
                Helper.CaptureRequirement(2377, @"[In Application Requests an Open of a File,Pseudocode for the operation is as follows: ]
                    If RootOpen.Volume.IsReadOnly && CreateDisposition == FILE_CREATE,then the operation MUST be failed with STATUS_MEDIA_WRITE_PROTECTED.");
                return MessageStatus.MEDIA_WRITE_PROTECTED;
            }

            //If RootOpen.Volume.IsReadOnly && (CreateDisposition == FILE_SUPERSEDE) 
            //then the operation MUST be failed with STATUS_MEDIA_WRITE_PROTECTED
            if (isFileVolumeReadOnly && (createDisposition == CreateDisposition.SUPERSEDE))
            {
                Helper.CaptureRequirement(2378, @"[In Application Requests an Open of a File,Pseudocode for the operation is as follows: ]
                    If RootOpen.Volume.IsReadOnly &&CreateDisposition == FILE_SUPERSEDE, then the operation MUST be failed with STATUS_MEDIA_WRITE_PROTECTED.");
                return MessageStatus.MEDIA_WRITE_PROTECTED;
            }

            //If RootOpen.Volume.IsReadOnly && (CreateDisposition == OVERWRITE) 
            //then the operation MUST be failed with STATUS_MEDIA_WRITE_PROTECTED
            if (isFileVolumeReadOnly && (createDisposition == CreateDisposition.OVERWRITE))
            {
                Helper.CaptureRequirement(2379, @"[In Application Requests an Open of a File,Pseudocode for the operation is as follows: ]
                    If RootOpen.Volume.IsReadOnly &&CreateDisposition == OVERWRITE, then the operation MUST be failed with STATUS_MEDIA_WRITE_PROTECTED.");
                return MessageStatus.MEDIA_WRITE_PROTECTED;
            }

            //If RootOpen.Volume.IsReadOnly && (CreateDisposition == OVERWRITE_IF) 
            //then the operation MUST be failed with STATUS_MEDIA_WRITE_PROTECTED
            if (isFileVolumeReadOnly && (createDisposition == CreateDisposition.OVERWRITE_IF))
            {
                Helper.CaptureRequirement(2380, @"[In Application Requests an Open of a File,Pseudocode for the operation is as follows: ]
                    If RootOpen.Volume.IsReadOnly &&CreateDisposition == OVERWRITE_IF, then the operation MUST be failed with STATUS_MEDIA_WRITE_PROTECTED.");
                return MessageStatus.MEDIA_WRITE_PROTECTED;
            }

            #endregion

            #region Phase 3  Initialization of Open Object

            gOpenRemainingDesiredAccess = desiredAccess;
            gOpenSharingMode = shareAccess;
            //4158 indicates (createOption & (CreateOptions.WRITE_THROUGH |
            //CreateOptions.SEQUENTIAL_ONLY | CreateOptions.NO_INTERMEDIATE_BUFFERING |
            //CreateOptions.SYNCHRONOUS_IO_ALERT | CreateOptions.SYNCHRONOUS_IO_NONALERT |
            //CreateOptions.DELETE_ON_CLOSE));

            gOpenMode = (CreateOptions.WRITE_THROUGH |
            CreateOptions.SEQUENTIAL_ONLY | CreateOptions.NO_INTERMEDIATE_BUFFERING |
            CreateOptions.SYNCHRONOUS_IO_ALERT | CreateOptions.SYNCHRONOUS_IO_NONALERT |
            CreateOptions.DELETE_ON_CLOSE);
            if (gSecurityContext.privilegeSet == PrivilegeSet.SeBackupPrivilege)
            {
                isOpenHasBackupAccess = true;
            }

            if (gSecurityContext.privilegeSet == PrivilegeSet.SeRestorePrivilege)
            {
                isOpenHasRestoreAccess = true;
            }

            if (gSecurityContext.privilegeSet == PrivilegeSet.SeCreateSymbolicLinkPrivilege)
            {
                isOpenHasCreateSymbolicLinkAccess = true;
            }

            if (gSecurityContext.privilegeSet == PrivilegeSet.SeManageVolumePrivilege)
            {
                isOpenHasManageVolumeAccess = true;
            }

            if (gSecurityContext.isSecurityContextSIDsContainWellKnown)
            {
                isOpenIsAdministrator = true;
            }

            #endregion

            #region phase 4  Check for backup/restore intent

            //If CreateOptions.FILE_OPEN_FOR_BACKUP_INTENT is set and (CreateDisposition == 
            //FILE_OPEN || CreateDisposition == FILE_OPEN_IF || CreateDisposition == 
            //FILE_OVERWRITE_IF) and Open.HasBackupAccess is true, then the object store 
            //SHOULD grant backup access as shown in the following pseudocode:
            if (createOption == CreateOptions.OPEN_FOR_BACKUP_INTENT &&
                (createDisposition == CreateDisposition.OPEN ||
                createDisposition == CreateDisposition.OPEN_IF ||
                createDisposition == CreateDisposition.OVERWRITE_IF) && isOpenHasBackupAccess)
            {
                //2164391968 indicates FileAccess.READ_CONTROL |
                //FileAccess.ACCESS_SYSTEM_SECURITY | FileAccess.GENERIC_READ |
                //FileAccess.FILE_TRAVERSE;
                FileAccess BackupAccess = FileAccess.READ_CONTROL |
                FileAccess.ACCESS_SYSTEM_SECURITY | FileAccess.GENERIC_READ |
                FileAccess.FILE_TRAVERSE;

                if (gOpenRemainingDesiredAccess == FileAccess.MAXIMUM_ALLOWED)
                {
                    gOpenGrantedAccess = gOpenGrantedAccess | BackupAccess;
                }
                else
                {
                    gOpenGrantedAccess = gOpenGrantedAccess | (gOpenRemainingDesiredAccess & BackupAccess);
                }

                gOpenRemainingDesiredAccess = gOpenRemainingDesiredAccess & (~gOpenGrantedAccess);
            }

            //If CreateOptions.FILE_OPEN_FOR_BACKUP_INTENT is set and Open.HasRestoreAccess is true, 
            //then the object store SHOULD grant restore access as shown in the following pseudocode:
            if (createOption == CreateOptions.OPEN_FOR_BACKUP_INTENT && isHasRestoreAccess)
            {
                //1091309574 indicates FileAccess.WRITE_DAC | FileAccess.WRITE_OWNER |
                //FileAccess.ACCESS_SYSTEM_SECURITY | FileAccess.GENERIC_WRITE |
                //FileAccess.FILE_ADD_FILE | FileAccess.FILE_ADD_SUBDIRECTORY |
                //FileAccess.DELETE
                FileAccess RestoreAccess = FileAccess.WRITE_DAC | FileAccess.WRITE_OWNER |
                FileAccess.ACCESS_SYSTEM_SECURITY | FileAccess.GENERIC_WRITE |
                FileAccess.FILE_ADD_FILE | FileAccess.FILE_ADD_SUBDIRECTORY |
                FileAccess.DELETE;

                if (gOpenRemainingDesiredAccess == FileAccess.MAXIMUM_ALLOWED)
                {
                    gOpenGrantedAccess = gOpenGrantedAccess | RestoreAccess;
                }
                else
                {
                    gOpenGrantedAccess = gOpenGrantedAccess | (gOpenRemainingDesiredAccess & RestoreAccess);
                }

                gOpenRemainingDesiredAccess = gOpenRemainingDesiredAccess & (~gOpenGrantedAccess);
            }

            #endregion

            #region phase 5  Parse path name

            //If any StreamTypeNamei is "$INDEX_ALLOCATION" and the corresponding 
            //StreamNamei has a value other than an empty string or "$I30"
            if (fileNameStatus == FileNameStatus.StreamTypeNameIsINDEX_ALLOCATION)
            {
                if (sutPlatForm != PlatformType.NoneWindows)
                {
                    Helper.CaptureRequirement(507, @"[In Application Requests an Open of a File ,Phase 5 -- Parse path name:]
                        If any StreamTypeNamei is \""$INDEX_ALLOCATION"" and the corresponding StreamNamei has a value other than an empty string 
                        or \""$I30"", the operation is failed with STATUS_INVALID_PARAMETER in Windows.");
                    return MessageStatus.INVALID_PARAMETER;
                }
                else if (isR507Implemented)
                {
                    Helper.CaptureRequirement(392, @"[In Application Requests an Open of a File ,Phase 5 -- Parse path name:]
                        If any StreamTypeNamei is \""$INDEX_ALLOCATION"" and the corresponding StreamNamei has a value other than an empty string 
                        or \""$I30\"", the operation SHOULD be failed with STATUS_INVALID_PARAMETER.");
                    return MessageStatus.INVALID_PARAMETER;
                }
            }

            #endregion

            #region phase 6  Location of file

            if (streamFoundType == StreamFoundType.StreamIsNotFound)
            {
                //If (CreateDisposition == FILE_OPEN )
                if (createDisposition == CreateDisposition.OPEN)
                {
                    Helper.CaptureRequirement(513, @"[In Application Requests an Open of a File , Pseudocode for the operation is as follows:
                        Phase 6 -- Location of file:] Else:[If such a link is not found:]
                        If CreateDisposition == FILE_OPEN, the operation MUST be failed with STATUS_OBJECT_NAME_NOT_FOUND.");
                    return MessageStatus.OBJECT_NAME_NOT_FOUND;
                }

                //If (CreateDisposition == FILE_OVERWRITE)
                if (createDisposition == CreateDisposition.OVERWRITE)
                {
                    Helper.CaptureRequirement(2395, @"[In Application Requests an Open of a File , Pseudocode for the operation is as follows:
                        Phase 6 -- Location of file:] Else:[If such a link is not found:]If CreateDisposition == FILE_OVERWRITE), 
                        the operation MUST be failed with STATUS_OBJECT_NAME_NOT_FOUND.");
                    return MessageStatus.OBJECT_NAME_NOT_FOUND;
                }

                //If RootOpen.Volume.IsReadOnly 
                if (isFileVolumeReadOnly)
                {
                    Helper.CaptureRequirement(514, @"[In Application Requests an Open of a File , Pseudocode for the operation is as follows:
                        Phase 6 -- Location of file:] Else:[If such a link is not found:]If RootOpen.Volume.IsReadOnly 
                        then the operation MUST be failed with STATUS_MEDIA_WRITE_PROTECTED.");
                    return MessageStatus.MEDIA_WRITE_PROTECTED;
                }

                //Helper.CaptureRequirement(547, @"[In Application Requests an Open of a File , Pseudocode for the operation is as follows:Phase 6 -- Location of file:] for this search is as follows:For i = 1 to n-1:Search ParentFile.DirectoryList for a Link where Link.Name or Link.ShortName matches FileNamei,If no such link is found, the operation MUST be failed with STATUS_OBJECT_PATH_NOT_FOUND.");
                //return MessageStatus.OBJECT_PATH_NOT_FOUND;
            }

            //If Open.GrantedAccess.FILE_TRAVERSE is not set and 
            //AccessCheck( SecurityContext, Link.File.SecurityDescriptor, FILE_TRAVERSE ) 
            //returns FALSE,
            if ((gOpenGrantedAccess & FileAccess.FILE_TRAVERSE) == 0)
            {
                if (sutPlatForm != PlatformType.NoneWindows)
                {
                    Helper.CaptureRequirement(405, @"[In Application Requests an Open of a File , Pseudocode for the operation is as follows:
                        Phase 6 -- Location of file:] Pseudocode for this search:For i = 1 to n-1:
                        If Open.GrantedAccess.FILE_TRAVERSE is not set and AccessCheck( SecurityContext, Link.File.SecurityDescriptor, FILE_TRAVERSE ) 
                        returns FALSE, the operation is not  failed with STATUS_ACCESS_DENIED in Windows.");
                }
                if (isR405Implemented)
                {
                    Helper.CaptureRequirement(397, @"[In Application Requests an Open of a File ,
                        Pseudocode for the operation is as follows:Phase 6 -- Location of file:] 
                        Pseudocode for this search:For i = 1 to n-1:If Open.GrantedAccess.FILE_TRAVERSE is not set 
                        and AccessCheck( SecurityContext, Link.File.SecurityDescriptor, FILE_TRAVERSE ) returns FALSE, the operation MAY be failed with STATUS_ACCESS_DENIED.");
                    return MessageStatus.ACCESS_DENIED;
                }
            }
            //If Link.File.IsSymbolicLink is true
            if (symbolicLinkType == SymbolicLinkType.IsSymbolicLink)
            {
                Helper.CaptureRequirement(399, @"[In Application Requests an Open of a File , 
                     Pseudocode for the operation is as follows:Phase 6 - Location of file:] 
                     Pseudocode for this search:For i = 1 to n-1:If Link.File.IsSymbolicLink is TRUE, 
                     the operation MUST be failed with Status set to STATUS_STOPPED_ON_SYMLINK .");
                return MessageStatus.STOPPED_ON_SYMLINK;
            }

            #endregion

            #region phase 7  Type of file to open

            //If CreateOptions.FILE_DIRECTORY_FILE is true
            if (createOption == CreateOptions.DIRECTORY_FILE)
            {
                gfileTypeToOpen = FileType.DirectoryFile;
            }
            //Else if CreateOptions.FILE_NON_DIRECTORY_FILE is true
            else if (createOption == CreateOptions.NON_DIRECTORY_FILE)
            {
                gfileTypeToOpen = FileType.DataFile;
            }
            //Else if StreamTypeNameToOpen is "$INDEX_ALLOCATION"
            else if (streamTypeNameToOPen == StreamTypeNameToOpen.INDEX_ALLOCATION)
            {
                gfileTypeToOpen = FileType.DirectoryFile;
            }
            //Else if StreamTypeNameToOpen is "$DATA"
            else if (streamTypeNameToOPen == StreamTypeNameToOpen.DATA)
            {
                gfileTypeToOpen = FileType.DataFile;
            }
            //Else if Open.File is not NULL and Open.File.FileType is DirectoryFile
            else if (fileNameStatus == FileNameStatus.OpenFileNotNull &&
                openFileType == FileType.DirectoryFile)
            {
                gfileTypeToOpen = FileType.DirectoryFile;
            }
            //Else if PathName contains a trailing backslash
            else if (fileNameStatus == FileNameStatus.PathNameTraiblack)
            {
                gfileTypeToOpen = FileType.DirectoryFile;
            }
            //else
            else
            {
                gfileTypeToOpen = FileType.DataFile;
            }

            //If FileTypeToOpen is DirectoryFile and Open.File is not NULL and 
            //Open.File.FileType is not DirectoryFile:
            if (gfileTypeToOpen == FileType.DirectoryFile &&
                fileNameStatus == FileNameStatus.OpenFileNotNull &&
                openFileType == FileType.DataFile)
            {
                //If CreateDisposition == FILE_CREATE
                if (createDisposition == CreateDisposition.CREATE)
                {
                    Helper.CaptureRequirement(414, @"[In Application Requests an Open of a File , Pseudocode for the operation is as follows:
                        Phase 7 -- Type of file to open:]If FileTypeToOpen is DirectoryFile and Open.File is not NULL 
                        and Open.File.FileType is not DirectoryFile:If CreateDisposition == FILE_CREATE then the operation MUST be failed with STATUS_OBJECT_NAME_COLLISION.");
                    return MessageStatus.OBJECT_NAME_COLLISION;
                }
                //else
                else
                {
                    Helper.CaptureRequirement(2396, @"[In Application Requests an Open of a File , Phase 7 -- Type of file to open:]
                        If FileTypeToOpen is DirectoryFile and Open.File is not NULL and Open.File.FileType is not DirectoryFile:
                        else[If CreateDisposition != FILE_CREATE] the operation MUST be failed with STATUS_NOT_A_DIRECTORY.");
                    return MessageStatus.NOT_A_DIRECTORY;
                }
            }

            //If FileTypeToOpen is DataFile and StreamNameToOpen is empty and Open.File 
            //is not NULL and Open.File.FileType is DirectoryFile
            if (gfileTypeToOpen == FileType.DataFile &&
                streamTypeNameToOPen == StreamTypeNameToOpen.NULL &&
                fileNameStatus == FileNameStatus.OpenFileNotNull &&
                openFileType == FileType.DirectoryFile)
            {
                Helper.CaptureRequirement(415, @"[In Application Requests an Open of a File , Pseudocode for the operation is as follows:
                    Phase 7 -- Type of file to open:]If FileTypeToOpen is DataFile and StreamNameToOpen is empty and Open.File is not NULL 
                    and Open.File.FileType is DirectoryFile, the operation MUST be failed with STATUS_FILE_IS_A_DIRECTORY.");
                return MessageStatus.FILE_IS_A_DIRECTORY;
            }

            #endregion

            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.1.2.1 Algorithm to Check Access to an Existing File

        /// <summary>
        /// 3.1.5.1.2.1    Algorithm to Check Access to an Existing File
        /// </summary>
        /// <param name="openFileType">The type of file. This value MUST be either DataFile or DirectoryFile.</param>
        /// <param name="fileAttribute">Attributes of the file in the form specified in [MS-FSCC] section 2.6.</param>
        /// <param name="desiredAccess">The desired access, as specified in [MS-SMB2] section 2.2.13.1.</param>
        /// <param name="createOption">A bitmask of options for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="existingOpenModeCreateOption">CreateOption of the existing open</param>
        /// <param name="streamTypeNameToOpen">The stream type name to open used in 3.1.5.1</param>
        /// <param name="shareAccess">A bitmask indicate sharing access for the open file, as specified in [MS-SMB2] section 2.2.13</param>
        /// <param name="existOpenShareModeShareAccess">ShareAccess of the existing open</param>
        /// <returns>An NTSTATUS code that specifies the result of the access check.</returns>
        /// Disable warning CA1502, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        // Disable warning CA1801, because the parameter of "capabilities" is used for extend the model logic, 
        // it will affect the implementation of the model if it is removed.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static MessageStatus CheckExistingFileAccess(
            FileType openFileType,
            FileAttribute fileAttribute,
            FileAccess desiredAccess,
            CreateOptions createOption,
            CreateOptions existingOpenModeCreateOption,
            StreamTypeNameToOpen streamTypeNameToOPen,
            ShareAccess shareAccess,
            ShareAccess existOpenShareModeShareAccess)
        {


            //If Open.File.FileType is DataFile and (File.FileAttributes.FILE_ATTRIBUTE_READONLY && 
            //(DesiredAccess.FILE_WRITE_DATA))
            if (openFileType == FileType.DataFile &&
                existingOpenModeCreateOption == CreateOptions.NON_DIRECTORY_FILE &&
                (fileAttribute == FileAttribute.READONLY &&
                desiredAccess == FileAccess.FILE_WRITE_DATA))
            {
                Helper.CaptureRequirement(2422, @"[In Algorithm to Check Access to an Existing File]Pseudocode for these checks is as follows:
                    If Open.File.FileType is DataFile and (File.FileAttributes.FILE_ATTRIBUTE_READONLY && (DesiredAccess.FILE_APPEND_DATA)), 
                    then return STATUS_ACCESS_DENIED.");
                return MessageStatus.ACCESS_DENIED;
            }

            //If Open.File.FileType is DataFile and (File.FileAttributes.FILE_ATTRIBUTE_READONLY && 
            //(DesiredAccess.FILE_APPEND_DATA))
            if (openFileType == FileType.DataFile &&
                existingOpenModeCreateOption == CreateOptions.NON_DIRECTORY_FILE &&
                (fileAttribute == FileAttribute.READONLY &&
                desiredAccess == FileAccess.FILE_APPEND_DATA))
            {
                Helper.CaptureRequirement(53, @"[In Algorithm to Check Access to an Existing File,Pseudocode for these checks is as follows:]
                    If Open.File.FileType is DataFile and (File.FileAttributes.FILE_ATTRIBUTE_READONLY && (DesiredAccess.FILE_WRITE_DATA )), then return STATUS_ACCESS_DENIED.");
                return MessageStatus.ACCESS_DENIED;
            }

            //If ((File.FileAttributes.FILE_ATTRIBUTE_READONLY ) && 
            //CreateOptions.FILE_DELETE_ON_CLOSE)
            if (fileAttribute == FileAttribute.READONLY &&
                (createOption == CreateOptions.DELETE_ON_CLOSE))
            {
                Helper.CaptureRequirement(54, @"[In Algorithm to Check Access to an Existing File,Pseudocode for these checks is as follows:]
                    If ((File.FileAttributes.FILE_ATTRIBUTE_READONLY) && CreateOptions.FILE_DELETE_ON_CLOSE), then return STATUS_CANNOT_DELETE.");
                return MessageStatus.CANNOT_DELETE;
            }

            //If ((File.Volume.IsReadOnly) && 
            //CreateOptions.FILE_DELETE_ON_CLOSE)
            if (isFileVolumeReadOnly &&
                (createOption == CreateOptions.DELETE_ON_CLOSE))
            {
                Helper.CaptureRequirement(2423, @"[In Algorithm to Check Access to an Existing File,Pseudocode for these checks is as follows:]
                    If (( File.Volume.IsReadOnly) && CreateOptions.FILE_DELETE_ON_CLOSE), then return STATUS_CANNOT_DELETE.");

                return MessageStatus.CANNOT_DELETE;
            }

            //If Open.RemainingDesiredAccess is nonzero
            if (gOpenRemainingDesiredAccess != FileAccess.None)
            {
                //If Open.RemainingDesiredAccess.MAXIMUM_ALLOWED
                if ((gOpenRemainingDesiredAccess & FileAccess.MAXIMUM_ALLOWED) != 0)
                {
                    //For each Access Flag in FILE_ALL_ACCESS, the object store MUST set Open.GrantedAccess.
                    //Access if SecurityContext has Access based on File.SecurityDescriptor
                    //gOpenGrantedAccess = FileAccess.FILE_ALL_ACCESS;
                    //If File.FileAttributes.FILE_ATTRIBUTE_READONLY or File.Volume.IsReadOnly
                    if ((fileAttribute == FileAttribute.READONLY) || isFileVolumeReadOnly)
                    {
                        //then the object store MUST clear (FILE_WRITE_DATA | FILE_APPEND_DATA | 
                        //FILE_ADD_SUBDIRECTORY | FILE_DELETE_CHILD) from Open.GrantedAccess.
                        gOpenGrantedAccess = gOpenGrantedAccess & (~(FileAccess.FILE_WRITE_DATA |
                            FileAccess.FILE_APPEND_DATA | FileAccess.FILE_ADD_SUBDIRECTORY |
                            FileAccess.FILE_DELETE_CHILD));
                    }
                }
                else
                {
                    //For each Access Flag in Open.RemainingDesired.Access, the object store MUST set 
                    //Open.GrantedAccess.Access if SecurityContext has Access based on File.SecurityDescriptor
                    gOpenGrantedAccess = gOpenRemainingDesiredAccess;
                }

                //If (Open.RemainingDesiredAccess.MAXIMUM_ALLOWED || Open.RemainingDesiredAccess.DELETE), 
                //the object store MUST set Open.GrantedAccess.DELETE if AccessCheck( SecurityContext, 
                //Open.Link.ParentFile.SecurityDescriptor, FILE_DELETE_CHILD ) returns true

                if (gOpenRemainingDesiredAccess == FileAccess.MAXIMUM_ALLOWED ||
                    gOpenRemainingDesiredAccess == FileAccess.DELETE)
                {
                    //the object store MUST set Open.GrantedAccess.FILE_READ_ATTRIBUTES
                    gOpenGrantedAccess = FileAccess.DELETE;
                }
                //If (Open.RemainingDesiredAccess.MAXIMUM_ALLOWED || Open.RemainingDesiredAccess.
                //FILE_READ_ATTRIBUTES), the object store MUST set Open.GrantedAccess.FILE_READ_ATTRIBUTES 
                //if AccessCheck( SecurityContext, Open.Link.ParentFile.SecurityDescriptor, FILE_LIST_DIRECTORY )
                //returns true

                if (gOpenRemainingDesiredAccess == FileAccess.MAXIMUM_ALLOWED ||
                    gOpenRemainingDesiredAccess == FileAccess.FILE_READ_ATTRIBUTES)
                {
                    //the object store MUST set Open.GrantedAccess.FILE_READ_ATTRIBUTES
                    gOpenGrantedAccess = FileAccess.FILE_READ_ATTRIBUTES;
                }
                //Open.RemainingDesiredAccess &= ~(Open.GrantedAccess | MAXIMUM_ALLOWED)
                gOpenRemainingDesiredAccess = gOpenRemainingDesiredAccess &
                    (~(gOpenGrantedAccess | FileAccess.MAXIMUM_ALLOWED));
                //If Open.RemainingDesiredAccess is nonzero, then return STATUS_ACCESS_DENIED.
                if (gOpenRemainingDesiredAccess != FileAccess.None)
                {
                    Helper.CaptureRequirement(61, @"[In Algorithm to Check Access to an Existing File,Pseudocode for these checks is as follows:,
                        If Open.RemainingDesiredAccess is nonzero:]If Open.RemainingDesiredAccess is nonzero, then return STATUS_ACCESS_DENIED.");
                    return MessageStatus.ACCESS_DENIED;
                }
            }
            //If Open.SharingMode.FILE_SHARE_DELETE is FALSE and Open.GrantedAccess contains any 
            //one or more of (FILE_EXECUTE | FILE_READ_DATA | FILE_WRITE_DATA | FILE_APPEND_DATA):
            if ((gOpenSharingMode & ShareAccess.FILE_SHARE_DELETE) != 0 &&
                (gOpenGrantedAccess & (FileAccess.FILE_EXECUTE |
                FileAccess.FILE_READ_DATA | FileAccess.FILE_WRITE_DATA |
                FileAccess.FILE_APPEND_DATA)) != 0)
            {
                //For each ExistingOpen is Open.File.OpenList:
                //If ExistingOpen.Mode.FILE_DELETE_ON_CLOSE is true and ExistingOpen.Stream.StreamType is 
                //DirectoryStream 
                if ((existingOpenModeCreateOption & CreateOptions.DELETE_ON_CLOSE) != 0 &&
                    openFileType == FileType.DirectoryFile)
                {
                    Helper.CaptureRequirement(68, @"[In Algorithm to Check Access to an Existing File,Pseudocode for these checks is as follows:]
                        Pseudocode for these checks is as follows:If Open.SharingMode.FILE_SHARE_DELETE is FALSE and Open.
                        GrantedAccess contains any one or more of (FILE_EXECUTE | FILE_READ_DATA | FILE_WRITE_DATA | FILE_APPEND_DATA):
                        For each ExistingOpen is Open.File.OpenList:
                        If ExistingOpen.Mode.FILE_DELETE_ON_CLOSE is TRUE and ExistingOpen.Stream.StreamType is DirectoryStream, then return STATUS_SHARING_VIOLATION.");
                    return MessageStatus.SHARING_VIOLATION;
                }
            }

            //If Open.GrantedAccess.DELETE is true and Open.Stream.StreamType is DirectoryStream
            if ((gOpenGrantedAccess & FileAccess.DELETE) != 0 &&
                (streamTypeNameToOPen == StreamTypeNameToOpen.INDEX_ALLOCATION))
            {
                //For each ExistingOpen in Open.File.OpenList:
                //If ExistingOpen.SharingMode.FILE_SHARE_DELETE is FALSE
                if (existOpenShareModeShareAccess != ShareAccess.FILE_SHARE_DELETE)
                {
                    Helper.CaptureRequirement(66, @"[In Algorithm to Check Access to an Existing File,Pseudocode for these checks is as follows:]
                        If Open.GrantedAccess.DELETE is TRUE and Open.Stream.StreamType is DirectoryStream:For each ExistingOpen in Open.File.OpenList:
                        If ExistingOpen.SharingMode.FILE_SHARE_DELETE is FALSE, then return STATUS_SHARING_VIOLATION.");
                    return MessageStatus.SHARING_VIOLATION;
                }
            }

            Helper.CaptureRequirement(65, @"[In Algorithm to Check Access to an Existing File,Pseudocode for these checks is as follows:,
                if the sharing conflicts check has no violation]Return STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #region 3.1.5.1.2.2 Algorithm to Check Sharing Access to an Existing Stream or Directory

        /// <summary>
        /// 3.1.5.1.2.2    Algorithm to Check Sharing Access to an Existing Stream or Directory
        /// </summary>
        /// <param name="streamFoundType">Indicate if the stream is found or not.</param>
        /// <param name="existOpenDesiredAccess">Indicate FileAccess</param>
        /// <param name="existOpenShareModeShareAccess">Indicate FileAccess</param>
        /// <param name="openDesiredAccess">Indicate file access</param>
        /// <param name="shareAccess">Indicate share access</param>        
        /// <returns>An NTSTATUS code that specifies the result of the access check.</returns>
        /// Disable warning CA1801, because the parameter of "capabilities" is used for extend the model logic, 
        /// which will affect the implementation of the model if it is removed.
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public static MessageStatus CheckShareAccess(
            StreamFoundType streamFoundType,
            ShareAccess existOpenShareModeShareAccess,
            FileAccess openDesiredAccess,
            FileAccess existOpenDesiredAccess,
            ShareAccess shareAccess)
        {
            //If AccessCheck( SecurityContext, Open.Link.ParentFile.SecurityDescriptor, FILE_WRITE_DATA ) 
            //returns FALSE, the object store MUST set Open.SharingMode.FILE_SHARE_READ to true.

            //If ExistingOpen.Stream equals Open.Stream
            if (streamFoundType == StreamFoundType.StreamIsFound)
            {
                //If ExistingOpen.SharingMode.FILE_SHARE_READ is FALSE and Open.DesiredAccess contains 
                //FILE_READ_DATA
                if ((existOpenShareModeShareAccess & ShareAccess.FILE_SHARE_READ) == 0 &&
                    (openDesiredAccess & FileAccess.FILE_READ_DATA) != 0)
                {
                    return MessageStatus.ACCESS_VIOLATION;
                }

                //If ExistingOpen.SharingMode.FILE_SHARE_READ is FALSE and Open.DesiredAccess contains 
                //FILE_EXECUTE
                if ((existOpenShareModeShareAccess & ShareAccess.FILE_SHARE_READ) == 0 &&
                    (openDesiredAccess & FileAccess.FILE_EXECUTE) != 0)
                {
                    Helper.CaptureRequirement(650, @"[In Algorithm to Check Sharing Access to an Existing Stream or Directory,
                        Pseudocode for these checks is as follows,For each ExistingOpen in Open.File.OpenList:If ExistingOpen.Stream equals Open.Stream, 
                        then return STATUS_ACCESS_VIOLATION under any of the following conditions:]
                        If ExistingOpen.SharingMode.FILE_SHARE_WRITE is FALSE and Open.DesiredAccess contains  FILE_EXECUTE.");
                    return MessageStatus.ACCESS_VIOLATION;
                }

                //If ExistingOpen.SharingMode.FILE_SHARE_WRITE is FALSE and Open.DesiredAccess contains 
                //FILE_WRITE_DATA
                if ((existOpenShareModeShareAccess & ShareAccess.FILE_SHARE_WRITE) == 0 &&
                    (openDesiredAccess & FileAccess.FILE_WRITE_DATA) != 0)
                {
                    return MessageStatus.ACCESS_VIOLATION;
                }

                //If ExistingOpen.SharingMode.FILE_SHARE_WRITE is FALSE and Open.DesiredAccess contains 
                //FILE_APPEND_DATA.
                if ((existOpenShareModeShareAccess & ShareAccess.FILE_SHARE_WRITE) == 0 &&
                    (openDesiredAccess & FileAccess.FILE_APPEND_DATA) != 0)
                {
                    return MessageStatus.ACCESS_VIOLATION;
                }

                //If ExistingOpen.SharingMode.FILE_SHARE_DELETE is FALSE and Open.DesiredAccess contains DELETE.
                if ((existOpenShareModeShareAccess & ShareAccess.FILE_SHARE_DELETE) == 0 &&
                    (openDesiredAccess & FileAccess.DELETE) != 0)
                {
                    return MessageStatus.ACCESS_VIOLATION;
                }

                //If Open.SharingMode.FILE_SHARE_READ is FALSE and ExistingOpen.DesiredAccess contains 
                //FILE_READ_DATA
                if ((gOpenSharingMode & ShareAccess.FILE_SHARE_READ) == 0 &&
                    (existOpenDesiredAccess & FileAccess.FILE_READ_DATA) != 0)
                {
                    return MessageStatus.ACCESS_VIOLATION;
                }

                //If Open.SharingMode.FILE_SHARE_READ is FALSE and ExistingOpen.DesiredAccess contains 
                //FILE_EXECUTE.
                if ((gOpenSharingMode & ShareAccess.FILE_SHARE_READ) == 0 &&
                    (existOpenDesiredAccess & FileAccess.FILE_EXECUTE) != 0)
                {
                    return MessageStatus.ACCESS_VIOLATION;
                }

                //If Open.SharingMode.FILE_SHARE_WRITE is FALSE and ExistingOpen.DesiredAccess 
                //contains FILE_WRITE_DATA
                if ((gOpenSharingMode & ShareAccess.FILE_SHARE_WRITE) == 0 &&
                    (existOpenDesiredAccess & FileAccess.FILE_WRITE_DATA) != 0)
                {
                    Helper.CaptureRequirement(653, @"[In Algorithm to Check Sharing Access to an Existing Stream or Directory,
                        Pseudocode for these checks is as follows,For each ExistingOpen in Open.File.OpenList:If ExistingOpen.Stream equals Open.Stream, 
                        then return STATUS_ACCESS_VIOLATION under any of the following conditions:]
                        If Open.SharingMode.FILE_SHARE_WRITE is FALSE and ExistingOpen.DesiredAccess contains  FILE_WRITE_DATA .");
                    return MessageStatus.ACCESS_VIOLATION;
                }

                //If Open.SharingMode.FILE_SHARE_WRITE is FALSE and ExistingOpen.DesiredAccess 
                //contains FILE_APPEND_DATA.
                if ((gOpenSharingMode & ShareAccess.FILE_SHARE_WRITE) == 0 &&
                    (existOpenDesiredAccess & FileAccess.FILE_APPEND_DATA) != 0)
                {
                    return MessageStatus.ACCESS_VIOLATION;
                }

                //If Open.SharingMode.FILE_SHARE_READ is FALSE and ExistingOpen.DesiredAccess contains DELETE.
                if ((gOpenSharingMode & ShareAccess.FILE_SHARE_READ) != 0 &&
                    (existOpenDesiredAccess & FileAccess.DELETE) == 0)
                {
                    return MessageStatus.ACCESS_VIOLATION;
                }
            }

            Helper.CaptureRequirement(656, @"[In Algorithm to Check Sharing Access to an Existing Stream or Directory]
                Pseudocode for these checks is as follows,if the sharing checks has no violation]Return STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

        #endregion

        #region 3.1.5.1.2  Phase 9 Completion of open : Open an Existing File

        /// <summary>
        /// Open an Existing File
        /// </summary>
        /// <param name="shareAccess">The parameter in OpenFileinitial</param>
        /// <param name="desiredAccess">The parameter in OpenFileinitial and 3.1.5.1.2.1</param>
        /// <param name="streamFoundType">Indicate if the stream is found or not.</param>
        /// <param name="symbolicLinkType">Indicate if it is symbolic link or not.</param>
        /// <param name="openFileType">The parameter in OpenFileinitial and 3.1.5.1.2.1</param>
        /// <param name="existingOpenModeCreateOption">Indicate CreateOption</param>
        /// <param name="existOpenShareModeShareAccess">Indicate Share access</param>
        /// <param name="fileNameStatus">The parameter in OpenFileinitial</param>
        /// <param name="existOpenDesiredAccess">The parameter in 3.1.5.1.2.2</param>
        /// <param name="createOption">A bitmask of options for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="createDisposition">The desired disposition for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="streamTypeNameToOpen">StreamTypeNameToOpen</param>
        /// <param name="fileAttribute">A bitmask of file attributes for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="desiredFileAttribute">A bitmask of desired file attributes for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        /// Disable warning CA1502, because this action confuses with the actual model design if modify it.
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [Rule]
        public static MessageStatus OpenExistingFile(
            ShareAccess shareAccess,
            FileAccess desiredAccess,
            StreamFoundType streamFoundType,
            SymbolicLinkType symbolicLinkType,
            FileType openFileType,
            FileNameStatus fileNameStatus,
            CreateOptions existingOpenModeCreateOption,
            ShareAccess existOpenShareModeShareAccess,
            FileAccess existOpenDesiredAccess,
            CreateOptions createOption,
            CreateDisposition createDisposition,
            StreamTypeNameToOpen streamTypeNameToOPen,
            FileAttribute fileAttribute,
            FileAttribute desiredFileAttribute
            )
        {
            MessageStatus statusShareAccess = MessageStatus.SUCCESS;
            MessageStatus statusExistAccess = MessageStatus.SUCCESS;

            //call 3.1.5.1 openInitial
            MessageStatus statusInitial = OpenFileinitial(
            desiredAccess,
            shareAccess,
            createOption,
            createDisposition,
            desiredFileAttribute,
            streamFoundType,
            symbolicLinkType,
            streamTypeNameToOPen,
            openFileType,
            fileNameStatus);

            if (statusInitial != MessageStatus.SUCCESS)
            {
                return statusInitial;
            }

            #region FileTypeToOpen

            //If FileTypeToOpen is DirectoryFile
            if (gfileTypeToOpen == FileType.DirectoryFile)
            {
                //If CreateDisposition is FILE_OPEN
                if (createDisposition == CreateDisposition.OPEN)
                {
                    //call 3.1.5.1.2.1
                    statusExistAccess = CheckExistingFileAccess(
                    openFileType,
                    fileAttribute,
                    desiredAccess,
                    createOption,
                    existingOpenModeCreateOption,
                    streamTypeNameToOPen,
                    shareAccess,
                    existOpenShareModeShareAccess);

                    if (statusExistAccess != MessageStatus.SUCCESS)
                    {
                        Helper.CaptureRequirement(635, @"[In Open of an Existing File,Pseudocode for the operation is as follows:
                            If FileTypeToOpen is DirectoryFile:If CreateDisposition is FILE_OPEN then:]
                            If this[Perform access checks as described in section 3.1.5.1.2.1.] fails, the request MUST be failed with the same status.");
                        return statusExistAccess;
                    }

                    //call 3.1.5.1.2.2
                    statusShareAccess = CheckShareAccess(
                    streamFoundType,
                    existOpenShareModeShareAccess,
                    desiredAccess,
                    existOpenDesiredAccess,
                    shareAccess);

                    if (statusShareAccess != MessageStatus.SUCCESS)
                    {
                        Helper.CaptureRequirement(636, @"[In Open of an Existing File,Pseudocode for the operation is as follows:
                            If OpenFileType is DirectoryFile:If CreateDisposition is FILE_OPEN_IF then:]
                            If this[Perform access checks as described in section 3.1.5.1.2.1.] fails, the request MUST be failed with the same status.");
                        return statusShareAccess;
                    }

                    gCreateAction = CreateAction.OPENED;
                }
                //If CreateDisposition is FILE_OPEN_IF then
                else if (createDisposition == CreateDisposition.OPEN_IF)
                {
                    //call 3.1.5.1.2.1
                    statusExistAccess = CheckExistingFileAccess(
                    openFileType,
                    fileAttribute,
                    desiredAccess,
                    createOption,
                    existingOpenModeCreateOption,
                    streamTypeNameToOPen,
                    shareAccess,
                    existOpenShareModeShareAccess);

                    if (statusExistAccess != MessageStatus.SUCCESS)
                    {
                        Helper.CaptureRequirement(640, @"[In Open of an Existing File,Pseudocode for the operation is as follows:
                            If FileTypeToOpen is DirectoryFile:If CreateDisposition is FILE_OPEN_IF then:]
                            If this[Perform sharing access checks as described in section 3.1.5.1.2.2.] fails, the request MUST be failed with the same status.");
                        return statusExistAccess;
                    }

                    //call 3.1.5.1.2.2
                    statusShareAccess = CheckShareAccess(
                    streamFoundType,
                    existOpenShareModeShareAccess,
                    desiredAccess,
                    existOpenDesiredAccess,
                    shareAccess);

                    if (statusShareAccess != MessageStatus.SUCCESS)
                    {
                        Helper.CaptureRequirement(639, @"[In Open of an Existing File,Pseudocode for the operation is as follows:
                            If FileTypeToOpen is DirectoryFile:If CreateDisposition is FILE_OPEN then:]
                            If this [Perform sharing access checks as described in section 3.1.5.1.2.2.]fails, the request MUST be failed with the same status.");
                        return statusShareAccess;
                    }

                    gCreateAction = CreateAction.OPENED;
                }
                else
                {
                    //Existing directories cannot be overwritten/superseded
                    //If File == File.Volume.RootDirectory 
                    if (isFileEqualRootDirectory)
                    {
                        Helper.CaptureRequirement(578, @"[In Open of an Existing File,Pseudocode for the operation is as follows:
                            If FileTypeToOpen is DirectoryFile:]
                            Else If File == File.Volume.RootDirectory then the operation MUST be failed with STATUS_ACCESS_DENIED.");
                        return MessageStatus.ACCESS_DENIED;
                    }
                    else
                    {
                        Helper.CaptureRequirement(579, @"[In Open of an Existing File,Pseudocode for the operation is as follows:
                            If FileTypeToOpen is DirectoryFile:]
                            else[If File != File.Volume.RootDirectory ] the operation MUST be failed with STATUS_OBJECT_NAME_COLLISION.");
                        return MessageStatus.OBJECT_NAME_COLLISION;
                    }
                }
            }
            //Else if FileTypeToOpen is DataFile
            else if (gfileTypeToOpen == FileType.DataFile)
            {
                #region If Stream was found

                if (streamFoundType == StreamFoundType.StreamIsFound)
                {
                    //If CreateDisposition is FILE_CREATE
                    if (createDisposition == CreateDisposition.CREATE)
                    {
                        Helper.CaptureRequirement(584, @"[In Open of an Existing File,Pseudocode for the operation is as follows:
                            Else if FileTypeToOpen is DataFile,If Stream was found,]If CreateDisposition is FILE_CREATE, 
                            then the operation MUST be failed with STATUS_OBJECT_NAME_COLLISION.");
                        return MessageStatus.OBJECT_NAME_COLLISION;
                    }

                    //If CreateDisposition is FILE_OPEN_IF
                    if (createDisposition == CreateDisposition.OPEN_IF)
                    {
                        //call 3.1.5.1.2.1
                        statusExistAccess = CheckExistingFileAccess(
                        openFileType,
                        fileAttribute,
                        desiredAccess,
                        createOption,
                        existingOpenModeCreateOption,
                        streamTypeNameToOPen,
                        shareAccess,
                        existOpenShareModeShareAccess);

                        if (statusExistAccess != MessageStatus.SUCCESS)
                        {
                            return statusExistAccess;
                        }

                        //call 3.1.5.1.2.2
                        statusShareAccess = CheckShareAccess(
                        streamFoundType,
                        existOpenShareModeShareAccess,
                        desiredAccess,
                        existOpenDesiredAccess,
                        shareAccess);

                        if (statusShareAccess != MessageStatus.SUCCESS)
                        {
                            return statusShareAccess;
                        }

                        gCreateAction = CreateAction.OPENED;
                    }
                    //If CreateDisposition is FILE_OPEN
                    else if (createDisposition == CreateDisposition.OPEN)
                    {

                        //call 3.1.5.1.2.1
                        statusExistAccess = CheckExistingFileAccess(
                        openFileType,
                        fileAttribute,
                        desiredAccess,
                        createOption,
                        existingOpenModeCreateOption,
                        streamTypeNameToOPen,
                        shareAccess,
                        existOpenShareModeShareAccess);

                        if (statusExistAccess != MessageStatus.SUCCESS)
                        {
                            return statusExistAccess;
                        }

                        //call 3.1.5.1.2.2
                        statusShareAccess = CheckShareAccess(
                        streamFoundType,
                        existOpenShareModeShareAccess,
                        desiredAccess,
                        existOpenDesiredAccess,
                        shareAccess);

                        if (statusShareAccess != MessageStatus.SUCCESS)
                        {
                            return statusShareAccess;
                        }
                        gCreateAction = CreateAction.OPENED;
                    }
                    else
                    {
                        //If File.Volume.IsReadOnly is true
                        if (isFileVolumeReadOnly)
                        {
                            Helper.CaptureRequirement(596, @"[In Open of an Existing File,Pseudocode for the operation is as follows:
                                Else if FileTypeToOpen is DataFile:If Stream was found]Else (CreateDisposition is not FILE_OPEN and is not FILE_OPEN_IF)
                                If File.Volume.IsReadOnly is true, the operation MUST be failed with STATUS_MEDIA_WRITE_PROTECTED.");
                            return MessageStatus.MEDIA_WRITE_PROTECTED;
                        }

                        //If Stream.Name is empty
                        if (fileNameStatus == FileNameStatus.StreamNameNull)
                        {
                            //If File.FileAttributes.FILE_ATTRIBUTE_HIDDEN is true and 
                            //DesiredFileAttributes.FILE_ATTRIBUTE_HIDDEN is FALSE
                            if (fileAttribute == FileAttribute.HIDDEN &&
                                desiredFileAttribute != FileAttribute.HIDDEN)
                            {
                                Helper.CaptureRequirement(487, @"[In Open of an Existing File,Pseudocode for these checks is as follows:
                                    Else if FileTypeToOpen is DataFile,If Stream was found,Else (CreateDisposition is not FILE_OPEN and is not FILE_OPEN_IF),
                                    If Stream.Name is empty:]If File.FileAttributes.FILE_ATTRIBUTE_HIDDEN is TRUE 
                                    and DesiredFileAttributes.FILE_ATTRIBUTE_HIDDEN is FALSE, then the operation MUST be failed with STATUS_ACCESS_DENIED.");
                                return MessageStatus.ACCESS_DENIED;
                            }
                            //If File.FileAttributes.FILE_ATTRIBUTE_SYSTEM is true and 
                            //DesiredFileAttributes.FILE_ATTRIBUTE_SYSTEM is FALSE
                            if (fileAttribute == FileAttribute.SYSTEM &&
                                desiredFileAttribute != FileAttribute.SYSTEM)
                            {
                                Helper.CaptureRequirement(488, @"[In Open of an Existing File,Pseudocode for these checks is as follows:
                                    Else if FileTypeToOpen is DataFile,If Stream was found,Else (CreateDisposition is not FILE_OPEN and is not FILE_OPEN_IF),
                                    If Stream.Name is empty:]If File.FileAttributes.FILE_ATTRIBUTE_SYSTEM is TRUE 
                                    and DesiredFileAttributes.FILE_ATTRIBUTE_SYSTEM is FALSE, then the operation MUST be failed with STATUS_ACCESS_DENIED.");
                                return MessageStatus.ACCESS_DENIED;
                            }

                            //Set DesiredFileAttributes.FILE_ATTRIBUTE_ARCHIVE to true.
                            desiredFileAttribute = desiredFileAttribute | FileAttribute.ARCHIVE;

                            //Set DesiredFileAttributes.FILE_ATTRIBUTE_NORMAL to FALSE.
                            desiredFileAttribute = desiredFileAttribute & (~FileAttribute.NORMAL);

                            //Set DesiredFileAttributes.FILE_ATTRIBUTE_NOT_CONTENT_INDEXED to FALSE
                            desiredFileAttribute = desiredFileAttribute &
                                (~FileAttribute.NOT_CONTENT_INDEXED);

                            //If File.FileAttributes.FILE_ATTRIBUTE_ENCRYPTED is true, then 
                            //set DesiredFileAttributes.FILE_ATTRIBUTE_ENCRYPTED to true.
                            if ((fileAttribute & FileAttribute.ENCRYPTED) != 0)
                            {
                                desiredFileAttribute = desiredFileAttribute | FileAttribute.ENCRYPTED;
                            }

                            //If Open.HasRestoreAccess is true
                            if (isOpenHasRestoreAccess)
                            {
                                //set Open.GrantedAccess.FILE_WRITE_EA to true
                                gOpenGrantedAccess = gOpenGrantedAccess | FileAccess.FILE_WRITE_EA;

                                //set Open.GrantedAccess.FILE_WRITE_ATTRIBUTES to true
                                gOpenGrantedAccess = gOpenGrantedAccess | FileAccess.FILE_WRITE_ATTRIBUTES;
                            }
                            else
                            {
                                //set Open.RemainingDesiredAccess
                                gOpenRemainingDesiredAccess = gOpenRemainingDesiredAccess |
                                    FileAccess.FILE_WRITE_EA;

                                //store must set Open.RemainingDesiredAccess.FILE_WRITE_ATTRIBUTES to 
                                //true
                                gOpenRemainingDesiredAccess = gOpenRemainingDesiredAccess |
                                    FileAccess.FILE_WRITE_ATTRIBUTES;
                            }
                        }

                        //If CreateDisposition is FILE_SUPERSEDE
                        if (createDisposition == CreateDisposition.SUPERSEDE)
                        {
                            //If Open.HasRestoreAccess is true
                            if (isOpenHasRestoreAccess)
                            {
                                //set Open.GrantedAccess.DELETE to true
                                gOpenGrantedAccess = gOpenGrantedAccess | FileAccess.DELETE;
                            }
                            else
                            {
                                //Otherwise, the object store must set Open.RemainingDesiredAccess.
                                //FILE_WRITE_DATA to true
                                gOpenRemainingDesiredAccess = gOpenRemainingDesiredAccess | FileAccess.DELETE;
                            }
                        }
                        else
                        {
                            //If Open.HasRestoreAccess is true
                            if (isOpenHasRestoreAccess)
                            {
                                //set Open.GrantedAccess.DELETE to true
                                gOpenGrantedAccess = gOpenGrantedAccess | FileAccess.FILE_WRITE_DATA;
                            }
                            else
                            {
                                //Otherwise, the object store must set Open.RemainingDesiredAccess.
                                //FILE_WRITE_DATA to true
                                gOpenRemainingDesiredAccess = gOpenRemainingDesiredAccess |
                                    FileAccess.FILE_WRITE_DATA;
                            }
                        }

                        gOpenRemainingDesiredAccess = gOpenRemainingDesiredAccess & (~gOpenGrantedAccess);

                        if (createDisposition == CreateDisposition.SUPERSEDE)
                        {
                            gCreateAction = CreateAction.SUPERSEDED;
                        }
                        else
                        {
                            gCreateAction = CreateAction.OVERWRITTEN;
                        }
                    }
                }

                #endregion

                #region Stream not found

                else
                {
                    //If CreateDisposition is FILE_OPEN or FILE_OVERWRITE
                    if (createDisposition == CreateDisposition.OPEN)
                    {
                        Helper.CaptureRequirement(610, @"[In Open of an Existing File,Pseudocode for these checks is as follows:
                            Else if FileTypeToOpen is DataFile,Else (Steam not found)]If CreateDisposition is FILE_OPEN , 
                            the operation MUST be failed with STATUS_OBJECT_NAME_NOT_FOUND.");
                        return MessageStatus.OBJECT_NAME_NOT_FOUND;
                    }

                    //If CreateDisposition is FILE_OPEN or FILE_OVERWRITE
                    if (createDisposition == CreateDisposition.OVERWRITE)
                    {
                        Helper.CaptureRequirement(2416, @"[In Open of an Existing File,Pseudocode for these checks is as follows:
                            Else if FileTypeToOpen is DataFile,Else (Steam not found)]If CreateDisposition is FILE_OVERWRITE, the operation 
                            MUST be failed with STATUS_OBJECT_NAME_NOT_FOUND.");
                        return MessageStatus.OBJECT_NAME_NOT_FOUND;
                    }

                    //If Open.GrantedAccess.FILE_WRITE_DATA is not set and Open.RemainingDesiredAccess.
                    //FILE_WRITE_DATA is not set:
                    if ((gOpenGrantedAccess & FileAccess.FILE_WRITE_DATA) == 0 &&
                        (gOpenRemainingDesiredAccess & FileAccess.FILE_WRITE_DATA) == 0)
                    {
                        //If Open.HasRestoreAccess is true
                        if (isOpenHasRestoreAccess)
                        {
                            //then the object store MUST set Open.GrantedAccess.FILE_WRITE_DATA to true.
                            gOpenGrantedAccess = gOpenGrantedAccess | FileAccess.FILE_WRITE_DATA;
                        }
                        else
                        {
                            //Otherwise, the object store MUST set Open.RemainingDesiredAccess.
                            //FILE_WRITE_DATA to true.
                            gOpenRemainingDesiredAccess = gOpenRemainingDesiredAccess | FileAccess.FILE_WRITE_DATA;
                        }
                    }

                    //call 3.1.5.1.2.1
                    statusExistAccess = CheckExistingFileAccess(
                    openFileType,
                    fileAttribute,
                    desiredAccess,
                    createOption,
                    existingOpenModeCreateOption,
                    streamTypeNameToOPen,
                    shareAccess,
                    existOpenShareModeShareAccess);

                    if (statusExistAccess != MessageStatus.SUCCESS)
                    {
                        Helper.CaptureRequirement(9840, @"[In Open of an Existing File,Pseudocode for these checks is as follows:
                            Else if FileTypeToOpen is DataFile,Else (Steam not found)] If this[Perform access checks as described in section 3.1.5.1.2.1] fails, 
                            the request MUST be failed with the same status.");

                        return statusExistAccess;
                    }

                    //If File.Volume.IsReadOnly is true
                    if (isFileVolumeReadOnly)
                    {
                        Helper.CaptureRequirement(614, @"[In Open of an Existing File,Pseudocode for these checks is as follows:
                            Else if FileTypeToOpen is DataFile,Else (Steam not found)]If File.Volume.IsReadOnly is TRUE, 
                            the operation MUST be failed with STATUS_MEDIA_WRITE_PROTECTED.");
                        return MessageStatus.MEDIA_WRITE_PROTECTED;
                    }

                    //Set CreateAction to FILE_CREATED.
                    gCreateAction = CreateAction.CREATED;
                }

                #endregion

            }

            #endregion

            // If the object store implements encryption:
            if (gSecurityContext.isImplementsEncryption)
            {
                //If (CreateAction is FILE_OVERWRITTEN) and 
                //(Stream.Name is empty) and (DesiredAttributes.FILE_ATTRIBUTE_ENCRYPTED is true) 
                //and (File.FileAttributes.FILE_ATTRIBUTE_ENCRYPTED is FALSE) then:
                if ((gCreateAction == CreateAction.OVERWRITTEN)
                    && (fileNameStatus == FileNameStatus.StreamNameNull) &&
                    ((desiredFileAttribute & FileAttribute.ENCRYPTED) != 0)
                    && ((fileAttribute & FileAttribute.ENCRYPTED) == 0))
                {
                    //If File.OpenList is non-empty
                    //the openList must not empty
                    Helper.CaptureRequirement(623, @"[In Open of an Existing File,Pseudocode for these checks is as follows:]
                        If the object store implements encryption:If (CreateAction is FILE_OVERWRITTEN) and (Stream.Name is empty) and (DesiredAttributes.FILE_ATTRIBUTE_ENCRYPTED is TRUE) and (File.FileAttributes.FILE_ATTRIBUTE_ENCRYPTED is FALSE), then:If File.OpenList is non-empty, then the operation MUST be failed with STATUS_SHARING_VIOLATION.");
                    return MessageStatus.SHARING_VIOLATION;
                }

                //If (CreateAction is FILE_SUPERSEDED) and 
                //(Stream.Name is empty) and (DesiredAttributes.FILE_ATTRIBUTE_ENCRYPTED is true) 
                //and (File.FileAttributes.FILE_ATTRIBUTE_ENCRYPTED is FALSE) then:
                if ((gCreateAction == CreateAction.SUPERSEDED)
                    && (fileNameStatus == FileNameStatus.FileNameNull) &&
                    ((desiredFileAttribute & FileAttribute.ENCRYPTED) != 0)
                    && ((fileAttribute & FileAttribute.ENCRYPTED) == 0))
                {
                    //If File.OpenList is non-empty
                    //the openList must not empty
                    Helper.CaptureRequirement(2417, @"[In Open of an Existing File,Pseudocode for these checks is as follows:]
                        If the object store implements encryption:If (CreateAction is FILE_SUPERSEDED) and (Stream.Name is empty) and (DesiredAttributes.FILE_ATTRIBUTE_ENCRYPTED is TRUE) and (File.FileAttributes.FILE_ATTRIBUTE_ENCRYPTED is FALSE), then:If File.OpenList is non-empty, then the operation MUST be failed with STATUS_SHARING_VIOLATION.");
                    return MessageStatus.SHARING_VIOLATION;
                }
            }

            // If CreateAction is FILE_OVERWRITTEN
            if (gCreateAction == CreateAction.OVERWRITTEN)
            {
                if (fileNameStatus == FileNameStatus.FileNameNull)
                {
                    //Set File.FileAttributes to DesiredFileAttributes
                    fileAttribute = desiredFileAttribute;
                }
            }

            // If CreateAction is FILE_SUPERSEDED
            if (gCreateAction == CreateAction.SUPERSEDED)
            {
                if (fileNameStatus == FileNameStatus.FileNameNull)
                {
                    //Set File.FileAttributes to DesiredFileAttributes
                    fileAttribute = desiredFileAttribute;
                }
            }

            Helper.CaptureRequirement(631, @"[In Open of an Existing File,Pseudocode for these checks is as follows:]
                The object store MUST return :CreateAction set to FILE_OPENED.");
            gCreateAction = CreateAction.OPENED;

            Helper.CaptureRequirement(630, @"[In Open of an Existing File,Pseudocode for these checks is as follows:]
                The object store MUST return:Status set to STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

    }
}
