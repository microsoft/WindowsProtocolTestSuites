// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.SpecExplorer.Runtime.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite.TraditionalTestCases.QueryDirectory
{

    [TestClassAttribute()]
    public partial class QueryDirectoryTestCases : PtfTestClassBase
    {
        #region Variables
        private FSAAdapter fsaAdapter;
        #endregion

        #region Class Initialization and Cleanup
        [ClassInitializeAttribute()]
        public static void ClassInitialize(TestContext context)
        {
            PtfTestClassBase.Initialize(context);
        }

        [ClassCleanupAttribute()]
        public static void ClassCleanup()
        {
            PtfTestClassBase.Cleanup();
        }
        #endregion

        protected string CurrentTestCaseName
        {
            get
            {
                string fullName = (string)Site.TestProperties["CurrentTestCaseName"];
                return fullName.Split('.').LastOrDefault();
            }
        }

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            this.InitializeTestManager();
            this.fsaAdapter = new FSAAdapter();
            this.fsaAdapter.Initialize(BaseTestSite);
            this.fsaAdapter.LogTestCaseDescription(BaseTestSite);
            //Need to connect to RootDirectory for query or set quota info.
            this.fsaAdapter.ShareName = this.fsaAdapter.RootDirectory;
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Test environment:");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 1. File System: " + this.fsaAdapter.FileSystem.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 2. Transport: " + this.fsaAdapter.Transport.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 3. Share Path: " + this.fsaAdapter.UncSharePath);
            this.fsaAdapter.FsaInitial();
        }

        protected override void TestCleanup()
        {
            this.fsaAdapter.Dispose();
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion


        #region Test cases        
        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create file with ::$INDEX_ALLOCATION as suffix and query directory info.")]
        public void Fs_CreateFIle_QueryDirectory_Suffix_INDEX_ALLOCATION()
        {
            // Create a new directory with name as suffix
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);

            fileName = fileName + @"::$INDEX_ALLOCATION";

            MessageStatus status = CreateDirectory(fileName);

            this.fsaAdapter.AssertAreEqual(this.Manager, true,
              (status == MessageStatus.SUCCESS),
              "Create directory with name " + fileName + " is expected to succeed.");

           status = QueryDirectory(this.fsaAdapter.UncSharePath);

            this.fsaAdapter.AssertAreEqual(this.Manager, true,
            (status == MessageStatus.SUCCESS),
            "Query directory with file name " + fileName + " is expected to succeed.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create file with :$I30:$INDEX_ALLOCATION as suffix and query directory info.")]
        public void Fs_CreateFIle_QueryDirectory_Suffix_I30_INDEX_ALLOCATION()
        {
            // Create a new directory with name as suffix
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);

            fileName = fileName + @":$I30:$INDEX_ALLOCATION";

            MessageStatus status = CreateDirectory(fileName);

            this.fsaAdapter.AssertAreEqual(this.Manager, true,
              (status == MessageStatus.SUCCESS),
              "Create directory with name " + fileName + " is expected to succeed.");

            status = QueryDirectory(this.fsaAdapter.UncSharePath);

            this.fsaAdapter.AssertAreEqual(this.Manager, true,
            (status == MessageStatus.SUCCESS),
            "Query directory with file name " + fileName + " is expected to succeed.");
        }    

        public MessageStatus CreateFile(string fileName)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create a file with type: CreateOptions.NON_DIRECTORY_FILE and FileAttribute.NORMAL and name: " + fileName);

            MessageStatus status = MessageStatus.SUCCESS;
          
            status = this.fsaAdapter.CreateFile(
                        fileName,
                        (FileAttribute) 0,
                        CreateOptions.NON_DIRECTORY_FILE,
                        (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                        (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE| ShareAccess.FILE_SHARE_DELETE),
                        CreateDisposition.OPEN_IF);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create file and return with status " + status.ToString());

            return status;
        }

        public MessageStatus CreateDirectory(string fileName)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create a directory with type: CreateOptions.DIRECTORY_FILE and name: " + fileName);

            MessageStatus status = MessageStatus.SUCCESS;

            status = this.fsaAdapter.CreateFile(
                        fileName,
                        FileAttribute.DIRECTORY,
                        CreateOptions.DIRECTORY_FILE,
                        (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                        (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE),
                        CreateDisposition.OPEN_IF);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Create directory and return with status " + status.ToString());

            return status;
        }
               

        public MessageStatus QueryDirectory(string path, string searchPattern = "*", FileInfoClass fileinfoClass = FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION)
        {            
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Query a directory information: " + path);

            MessageStatus status = this.fsaAdapter.QueryDirectoryInfo(
              searchPattern,
              FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION,
              false,
              false,
              false,
              false);
            
            BaseTestSite.Log.Add(LogEntryKind.TestStep, string.Format("Query directory with search pattern {0} and return with status {1}. ", searchPattern, status.ToString()));

            return status;
        }

       
        #endregion
    }
}
