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
        #region capture SHOULD requirements 507

        /// <summary>
        /// Call of check 507 requirement if implement
        /// </summary>
        /// Disable warning CA1811, because this action is called in the certain scenario machines, which is configurable in the Config.cord file.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [Rule(Action = "call CheckIsR507Implemented(out _)")]
        static void CallCheckIsR507Implemented()
        {
        }

        /// <summary>
        /// Return of check 507 requirement if implement
        /// </summary>
        /// <param name="flag">true: if 507 requirement has implemented</param>
        /// Disable warning CA1811, because this action is called in the certain scenario machines, which is configurable in the Config.cord file.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [Rule(Action = "return CheckIsR507Implemented(out flag)")]
        static void ReturnCheckIsR507Implemented(bool flag)
        {
            isR507Implemented = flag;
        }

        #endregion

        #region capture SHOULD requirements 405

        /// <summary>
        /// The call of check 405 requirement if implement
        /// </summary>
        /// Disable warning CA1811, because this action is called in the certain scenario machines, which is configurable in the Config.cord file.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [Rule(Action = "call CheckIsR405Implemented(out _)")]
        static void CallCheckIsR405Implemented()
        {
        }

        /// <summary>
        /// The return of check 507 requirement if implement
        /// </summary>
        /// <param name="flag">true: if 405 requirement has implemented</param>
        /// Disable warning CA1811, because this action is called in the certain scenario machines, which is configurable in the Config.cord file.
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [Rule(Action = "return CheckIsR405Implemented(out flag)")]
        static void ReturnCheckIsR405Implemented(bool flag)
        {
            isR405Implemented = flag;
        }

        #endregion

        #region  3.1.5.1.1  Completion of open Create a new file

        /// <summary>
        /// 3.1.5.1.1 Creation of a New File
        /// </summary>
        /// <param name="desiredFileAttribute">distred file attribute</param>
        /// <param name="createOption">Create option</param>
        /// <param name="streamTypeNameToOpen">the name of stream type to open</param>    
        /// <param name="desiredAccess">A bitmask indicating desired access for the open, as specified in [MS-SMB2] section 2.2.13.1.</param>
        /// <param name="shareAccess">A bitmask indicating sharing access for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="createDisposition">The desired disposition for the open, as specified in [MS-SMB2] section 2.2.13.</param>
        /// <param name="streamFoundType">Indicate if the stream is found or not.</param>
        /// <param name="symbolicLinkType">Indicate if it is symbolic link or not.</param>
        /// <param name="openFileType">Open.File.FileType</param>
        /// <param name="fileNameStatus">FileNameStatus</param>
        /// <returns>An NTSTATUS code that specifies the result.</returns>
        [Rule]
        public static MessageStatus CreateFile(
            FileAttribute desiredFileAttribute,
            CreateOptions createOption,
            StreamTypeNameToOpen streamTypeNameToOPen,
            FileAccess desiredAccess,
            ShareAccess shareAccess,
            CreateDisposition createDisposition,
            StreamFoundType streamFoundType,
            SymbolicLinkType symbolicLinkType,
            FileType openFileType,
            FileNameStatus fileNameStatus
            )
        {
            MessageStatus status = OpenFileinitial(
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

            if (status != MessageStatus.SUCCESS)
            {
                return status;
            }

            #region Pseudocode for the operation

            //If FileTypeToOpen is DirectoryFile and DesiredFileAttributes.FILE_ATTRIBUTE_TEMPORARY is set            
            if (gfileTypeToOpen == FileType.DirectoryFile &&
                desiredFileAttribute == FileAttribute.TEMPORARY)
            {
                Helper.CaptureRequirement(418, @"[In Creation of a New File,Pseudocode for the operation is as follows:]If FileTypeToOpen is DirectoryFile and DesiredFileAttributes.FILE_ATTRIBUTE_TEMPORARY is set, the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                return MessageStatus.INVALID_PARAMETER;
            }

            //If DesiredFileAttributes.FILE_ATTRIBUTE_READONLY and CreateOptions.FILE_DELETE_ON_CLOSE 
            //are both set
            if (desiredFileAttribute == FileAttribute.READONLY &&
                createOption == CreateOptions.DELETE_ON_CLOSE)
            {
                Helper.CaptureRequirement(419, @"[In Creation of a New File,Pseudocode for the operation is as follows:]If DesiredFileAttributes.FILE_ATTRIBUTE_READONLY and CreateOptions.FILE_DELETE_ON_CLOSE are both set, the operation MUST be failed with STATUS_CANNOT_DELETE.");
                return MessageStatus.CANNOT_DELETE;
            }

            //If StreamTypeNameToOpen is non-empty and has a value other than "$DATA" or 
            //"$INDEX_ALLOCATION", the operation MUST be failed with STATUS_ACCESS_DENIED.
            if (streamTypeNameToOPen == StreamTypeNameToOpen.Other)
            {
                Helper.CaptureRequirement(420, @"[In Creation of a New File,Pseudocode for the operation is as follows:]If StreamTypeNameToOpen is non-empty and has a value other than \""$DATA"",the operation MUST be failed with STATUS_OBJECT_NAME_INVALID.");
                return MessageStatus.OBJECT_NAME_INVALID;
            }

            //If Open.RemainingDesiredAccess.ACCESS_SYSTEM_SECURITY is set and 
            //Open.GrantedAccess.ACCESS_SYSTEM_SECURITY is not set and 
            //SecurityContext.PrivilegeSet does not contain "SeSecurityPrivilege"
            //3 indicate SeBackupPrivilege and SeRestorePrivilege
            if (gOpenRemainingDesiredAccess == FileAccess.ACCESS_SYSTEM_SECURITY &&
                gOpenGrantedAccess == FileAccess.FILE_APPEND_DATA &&
                gSecurityContext.privilegeSet == (PrivilegeSet.SeBackupPrivilege | PrivilegeSet.SeRestorePrivilege))
            {
                Helper.CaptureRequirement(422, @"[In Creation of a New File,Pseudocode for the operation is as follows:]If Open.RemainingDesiredAccess.ACCESS_SYSTEM_SECURITY is set and Open.GrantedAccess.ACCESS_SYSTEM_SECURITY is not set and SecurityContext.PrivilegeSet does not contain \""SeSecurityPrivilege"", the operation MUST be failed with STATUS_ACCESS_DENIED.");
                return MessageStatus.ACCESS_DENIED;
            }

            //If FileTypeToOpen is DataFile and Open.GrantedAccess.FILE_ADD_FILE is not set 
            //and AccessCheck( SecurityContext, Open.Link.ParentFile.SecurityDescriptor, 
            //FILE_ADD_FILE ) returns FALSE and Open.HasRestoreAccess is FALSE

            if (gfileTypeToOpen == FileType.DataFile &&
                gOpenGrantedAccess == FileAccess.FILE_ADD_SUBDIRECTORY &&
                !isOpenHasRestoreAccess)
            {
                Helper.CaptureRequirement(423, @"[In Creation of a New File,Pseudocode for the operation is as follows:]
                    If FileTypeToOpen is DataFile and Open.GrantedAccess.FILE_ADD_FILE is not set and 
                    AccessCheck( SecurityContext, Open.Link.ParentFile.SecurityDescriptor, FILE_ADD_FILE ) 
                    returns FALSE and Open.HasRestoreAccess is FALSE, the operation MUST be failed with STATUS_ACCESS_DENIED.");
                return MessageStatus.ACCESS_DENIED;
            }

            //If FileTypeToOpen is DirectoryFile and Open.GrantedAccess.FILE_ADD_SUBDIRECTORY 
            //is not set and AccessCheck( SecurityContext, Open.Link.ParentFile.SecurityDescriptor, 
            //FILE_ADD_SUBDIRECTORY ) returns FALSE and Open.HasRestoreAccess is FALSE

            if (gfileTypeToOpen == FileType.DirectoryFile &&
                gOpenGrantedAccess == FileAccess.FILE_ADD_FILE &&
                !isOpenHasRestoreAccess)
            {
                Helper.CaptureRequirement(424, @"[In Creation of a New File,Pseudocode for the operation is as follows:]
                   If FileTypeToOpen is DirectoryFile and Open.GrantedAccess.FILE_ADD_SUBDIRECTORY is not set and 
                   AccessCheck( SecurityContext, Open.Link.ParentFile.SecurityDescriptor, FILE_ADD_SUBDIRECTORY ) 
                   returns FALSE and Open.HasRestoreAccess is FALSE, the operation MUST be failed with STATUS_ACCESS_DENIED.");
                return MessageStatus.ACCESS_DENIED;
            }

            if (streamTypeNameToOPen == StreamTypeNameToOpen.DATA ||
                streamTypeNameToOPen == StreamTypeNameToOpen.NULL)
            {
                gStreamType = StreamType.DataStream;
            }
            else
            {
                gStreamType = StreamType.DirectoryStream;
            }

            gFileAttribute = desiredFileAttribute;
            Helper.CaptureRequirement(475, @"[In Creation of a New File,Pseudocode for the operation is as follows:]
                The object store MUST return:CreateAction set to FILE_CREATED.");
            gCreateAction = CreateAction.CREATED;

            #endregion

            Helper.CaptureRequirement(474, @"[In Creation of a New File,Pseudocode for the operation is as follows:]
                The object store MUST return :Status set to STATUS_SUCCESS.");
            return MessageStatus.SUCCESS;
        }

        #endregion

    }
}
