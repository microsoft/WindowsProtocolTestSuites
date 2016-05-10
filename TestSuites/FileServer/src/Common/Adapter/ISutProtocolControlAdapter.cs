// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter
{
    public interface ISutProtocolControlAdapter : IAdapter
    {
        [MethodHelp("Create a directory in the specified share. \r\nThen returns True if the directory creation succeeded, otherwise False")]
        bool CreateDirectory(string share, string directoryName);

        [MethodHelp("Delete the directory from the specified share.")]
        void DeleteDirectory(string share, string directoryName);

        /// <summary>
        /// Create a file in the share.
        /// </summary>
        /// <param name="share">UNC path of the share.</param>
        /// <param name="fileName">The file name to create.</param>
        /// <param name="content">The content of the created file.</param>
        /// <returns>Return true if success, otherwise return false.</returns>
        [MethodHelp("Create a file in the specified share; and write the content to the file (don't write content if the content parameter is empty). \r\nThen return the result with True if the file was created successfully, otherwise reply False.")]
        bool CreateFile(string share, string fileName, string content);

        /// <summary>
        /// Copy the file to the share.
        /// </summary>
        /// <param name="share">UNC path of the share.</param>
        /// <param name="fileName">The file name to copy.</param>
        [MethodHelp("Copy the file to the specified share.")]
        void CopyFile(string share, string fileName);

        [MethodHelp("Delete the file from the specified share.")]
        void DeleteFile(string share, string fileName);

        [MethodHelp("Check if a share path is available.")]
        bool CheckIfShareIsAvailable(string share);
    }
}
