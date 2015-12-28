// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.CreateClose
{
    public struct CreateCloseConfig
    {
        /// <summary>
        /// Max SMB2 dialect that server supports
        /// </summary>
        public ModelDialectRevision MaxSmbVersionServerSupported;

        /// <summary>
        /// Indicates the platform of SUT
        /// </summary>
        public Platform Platform;

        public override string ToString()
        {
            StringBuilder outputInfo = new StringBuilder();
            outputInfo.AppendFormat("{0}: \r\n", "CreateCloseConfig State");
            outputInfo.AppendFormat("{0}: {1} \r\n", "MaxSmbVersionSupported", this.MaxSmbVersionServerSupported.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "Platform", this.Platform.ToString());

            return outputInfo.ToString();
        }
    }

    /// <summary>
    /// DFS capability of Server
    /// </summary>
    public enum ServerDfsCapability
    {
        /// <summary>
        /// Indicates that DFS is not capable in server
        /// </summary>
        DfsNotCapable,

        /// <summary>
        /// Indicates that DFS is capable in server
        /// </summary>
        DfsCapable
    }

    /// <summary>
    /// Indicates if response of querying file attributes is contained in Close Response
    /// </summary>
    public enum QueryResponseStatus
    {
        /// <summary>
        /// Indicates response of querying file attributes is contained in Close Response
        /// </summary>
        QueryResponseExist,

        /// <summary>
        /// Indicates response of querying file attributes is not contained in Close Response
        /// </summary>
        QueryResponseNotExist
    }

    /// <summary>
    /// Indicates the type of File Name in Create Request
    /// </summary>
    public enum CreateFileNameType
    {
        /// <summary>
        /// Indicates the first character of file name is a path separator character("\")
        /// </summary>
        StartWithPathSeparator,

        /// <summary>
        /// Indicates an normal invalid file name(not symbolic link)
        /// </summary>
        OtherInvalidFileName,

        /// <summary>
        /// Indicates an normal valid file name(not symbolic link nor DFS link)
        /// </summary>
        ValidFileName
    }

    /// <summary>
    /// Indicates if FileOpenReparsePoint is set in CreateOptions
    /// </summary>
    public enum CreateOptionsFileOpenReparsePointType
    {
        /// <summary>
        /// Indicates FileOpenReparsePoint is set
        /// </summary>
        FileOpenReparsePointSet,

        /// <summary>
        /// Indicates FileOpenReparsePoint is not set
        /// </summary>
        FileOpenReparsePointNotSet
    }

    /// <summary>
    /// Indicates if FileDeleteOnClose is set in CreateOptions
    /// </summary>
    public enum CreateOptionsFileDeleteOnCloseType
    {
        /// <summary>
        /// Indicates FileDeleteOnClose is set
        /// </summary>
        FileDeteteOnCloseSet,

        /// <summary>
        /// Indicates FileDeleteOnClose is not set
        /// </summary>
        FileDeteteOnCloseNotSet
    }

    /// <summary>
    /// Indicates the type of Create Context
    /// </summary>
    public enum CreateContextType
    {
        /// <summary>
        /// Indicates there's no create context in Create Request
        /// </summary>
        NoCreateContext,

        /// <summary>
        /// Indicates there's one create context not specified in section 2.2.13.2 of [MS-SMB2]
        /// </summary>
        InvalidCreateContext,

        /// <summary>
        /// Indicates the size of the individual create context is not equal to the DataLength of the create context
        /// </summary>
        InvalidCreateContextSize,

        /// <summary>
        /// Indicates there's one valid create context in Create Request
        /// </summary>
        ValidCreateContext
    }

    /// <summary>
    /// Indicates if the ImpersonationLevel field is valid or not
    /// </summary>
    public enum ImpersonationLevelType
    {
        /// <summary>
        /// Indicates the ImpersonationLevel is invalid
        /// </summary>
        InvalidImpersonationLevel,

        /// <summary>
        /// Indicates the ImpersonationLevel is valid
        /// </summary>
        ValidImpersonationLevel
    }

    /// <summary>
    /// Indicates if SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB is set in the Flags field of Close Request
    /// </summary>
    public enum CloseFlagType
    {
        /// <summary>
        /// Indicates SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB is set
        /// </summary>
        PostQueryAttribSet,

        /// <summary>
        /// Indicates SMB2_CLOSE_FLAG_POSTQUERY_ATTRIB is not set
        /// </summary>
        PostQueryAttribNotSet
    }

    /// <summary>
    /// Indicates if FileId.Volatile is valid or not in Close Request
    /// </summary>
    public enum FileIdVolatileType
    {
        /// <summary>
        /// Indicates FileId.Volatile is invalid
        /// </summary>
        InvalidFileIdVolatile,

        /// <summary>
        /// Indicates FileId.Volatile is valid
        /// </summary>
        ValidFileIdVolatile
    }

    /// <summary>
    /// Indicates if FileId.Persistent is equal to Open.DurableFileId
    /// </summary>
    public enum FileIdPersistentType
    {
        /// <summary>
        /// Indicates FileId.Persistent is equal to Open.DurableFileId
        /// </summary>
        EqualToOpenDurableFileID,

        /// <summary>
        /// Indicates FileId.Persistent is not equal to Open.DurableFileId
        /// </summary>
        NotEqualToOpenDurableFileID
    }

    /// <summary>
    /// Indicates if the file is a directory or not
    /// </summary>
    public enum CreateFileType
    {
        /// <summary>
        /// Indicates the file being created is a directory file.
        /// </summary>
        DirectoryFile,

        /// <summary>
        /// Indicates the file being created is not a directory file
        /// </summary>
        NonDirectoryFile
    }
}
