// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp.Adapter
{
    /// <summary>
    /// SUT Control Adapter to create, delete and modify files on remote server.
    /// </summary>
    public interface IWspSutAdapter : IAdapter
    {
        /// <summary>
        /// This method is to create a file on remote server machine.
        /// </summary>
        /// <param name="fileName">The name of the file to be created.</param>
        [MethodHelp(@"This method is to create a file on remote server machine.")]
        int CreateFile(string fileName);

        /// <summary>
        /// This method is to delete a file on remote server machine.
        /// </summary>
        /// <param name="fileName">The name of the file to be deleted.</param>
        [MethodHelp(@"This method is to delete a file on remote server machine.")]
        int DeleteFile(string fileName);

        /// <summary>
        /// This method is to modify a file on remote server machine.
        /// </summary>
        /// <param name="fileName">The name of the file to be modified.</param>
        [MethodHelp(@"This method is to modify a file on remote server machine.")]
        int ModifyFile(string fileName);

        /// <summary>
        /// This method is to modify the attributes of a file on remote server machine.
        /// </summary>
        /// <param name="fileName">The name of the file to be modified.</param>
        /// <param name="isReadonly">The file is read-only.</param>
        /// <param name="isHidden">The file is hidden.</param>
        [MethodHelp(@"This method is to modify the attributes of a file on remote server machine.")]
        int ModifyFileAttributes(string fileName, bool isReadonly, bool isHidden);
    }
}
