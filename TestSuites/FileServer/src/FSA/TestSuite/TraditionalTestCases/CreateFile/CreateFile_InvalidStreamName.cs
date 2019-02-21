// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class CreateFileTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CreateFile)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Try to create a directory with invalid stream name and expect failure.")]
        public void CreateFile_InvalidStreamName()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create a directory with invalid stream name");

            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            string streamName = this.fsaAdapter.ComposeRandomFileName(8);
            string randomFile = fileName + ":" + streamName + ":$INDEX_ALLOCATION";

            status = this.fsaAdapter.CreateFile(
                        randomFile,
                        FileAttribute.NORMAL,
                        CreateOptions.DIRECTORY_FILE,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ,
                        CreateDisposition.CREATE);

            //Step 2: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Verify returned NTSTATUS code.");
            if (this.fsaAdapter.FileSystem == FileSystem.NTFS || this.fsaAdapter.FileSystem == FileSystem.REFS)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                    "If StreamTypeNameToOpen is \"$INDEX_ALLOCATION\" and StreamNameToOpen has a value other than an empty string or \"$I30\", the operation SHOULD be failed with STATUS_INVALID_PARAMETER.");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.OBJECT_NAME_INVALID, status,
                    "Only the NTFS and ReFS file systems support complex name suffixes and StreamTypeNames. File systems that do not support this return STATUS_OBJECT_NAME_INVALID.");
            }
        }

        #endregion

    }
}
