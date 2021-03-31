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
        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CreateFile)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to create a directory end with backslash and expect success.")]
        public void CreateDirectory_EndWithBackSlash()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create a directory end with with backslash.");

            string fileNameEndWithBackSlash = this.fsaAdapter.ComposeRandomFileName(8) + "\\";

            status = this.fsaAdapter.CreateFile(
                        fileNameEndWithBackSlash,
                        FileAttribute.NORMAL,
                        CreateOptions.DIRECTORY_FILE,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ,
                        CreateDisposition.CREATE);

            //Step 2: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Verify returned NTSTATUS code.");
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                    "Create a directory file end with backslash should succeed.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CreateFile)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Try to create a file end with invalid backslash and expect failure.")]
        public void CreateFile_EndWithInvalidBackSlash()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create a file end with invalid backslash.");

            string fileNameWithInvalidBackSlash = this.fsaAdapter.ComposeRandomFileName(8) + "\\";

            status = this.fsaAdapter.CreateFile(
                        fileNameWithInvalidBackSlash,
                        FileAttribute.NORMAL,
                        CreateOptions.NON_DIRECTORY_FILE,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ,
                        CreateDisposition.CREATE);

            //Step 2: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Verify returned NTSTATUS code.");
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.OBJECT_NAME_INVALID, status,
                    "[MS-FSA] Section 2.1.5.1: The operation MUST be failed with STATUS_OBJECT_NAME_INVALID under any of the following conditions: 1. If PathName is not valid as specified in [MS-FSCC] section 2.1.5; " +
                    "2. If PathName contains a trailing backslash and CreateOptions.FILE_NON_DIRECTORY_FILE is TRUE");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CreateFile)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Try to create a file with double backslash in the middle and expect failure.")]
        public void CreateFile_WithDoubleBackSlashInMiddle()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create a file with double backslash in the middle.");

            string fileNameWithDoubleBackSlash = this.fsaAdapter.ComposeRandomFileName(8) + "\\\\" + this.fsaAdapter.ComposeRandomFileName(8);

            status = this.fsaAdapter.CreateFile(
                        fileNameWithDoubleBackSlash,
                        FileAttribute.NORMAL,
                        CreateOptions.NON_DIRECTORY_FILE,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ,
                        CreateDisposition.CREATE);

            //Step 2: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Verify returned NTSTATUS code.");
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.OBJECT_PATH_NOT_FOUND, status,
                    "[MS-FSA]: Search ParentFile.DirectoryList for a Link where Link.Name or Link.ShortName matches FileNamei. " +
                    "If no such link is found, the operation MUST be failed with STATUS_OBJECT_PATH_NOT_FOUND");
        }

    }
}
