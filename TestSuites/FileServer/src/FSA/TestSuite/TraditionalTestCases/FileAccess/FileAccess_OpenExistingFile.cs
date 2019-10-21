// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FileAccessTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.FileAccess)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")]
        public void FileAccess_WriteReadOnlyFile()
        {
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            FileAccess_Create_ReadOnlyFile(fileName);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Open the existing file with write access");

            MessageStatus status = this.fsaAdapter.CreateFile(
                        fileName,
                        FileAttribute.NORMAL,
                        CreateOptions.NON_DIRECTORY_FILE,
                        FileAccess.FILE_WRITE_DATA,
                        ShareAccess.FILE_SHARE_READ,
                        CreateDisposition.CREATE);

            //Step 2: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify returned NTSTATUS code.");
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.ACCESS_DENIED, status,
                    "xxxxxxxxxxxxxxxxxxx");

        }

        #endregion

        #region Test Case Utility

        private void FileAccess_Create_ReadOnlyFile(string fileName)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create read only file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create a read only data file");            

            status = this.fsaAdapter.CreateFile(
                        fileName,
                        FileAttribute.READONLY,
                        CreateOptions.NON_DIRECTORY_FILE,
                        FileAccess.FILE_WRITE_DATA,
                        ShareAccess.FILE_SHARE_READ,
                        CreateDisposition.CREATE);

            //Step 2: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Verify returned NTSTATUS code.");
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                    "Create a read only file should succeed.");
        }

        #endregion

    }
}
