// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    [TestClassAttribute()]
    public partial class FileInfoTestCases : PtfTestClassBase
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

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            this.InitializeTestManager();
            this.fsaAdapter = new FSAAdapter();
            this.fsaAdapter.Initialize(BaseTestSite);
            this.fsaAdapter.LogTestCaseDescription(BaseTestSite);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Test environment:");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 1. File System: " + this.fsaAdapter.FileSystem.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 2. Transport: " + this.fsaAdapter.Transport.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 3. Share Path: " + this.fsaAdapter.UncSharePath);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 4. Platform: " + this.fsaAdapter.TestConfig.Platform.ToString());
            this.fsaAdapter.FsaInitial();
        }

        protected override void TestCleanup()
        {
            this.fsaAdapter.Dispose();
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion

        #region Utility

        private long FileTimeToLong(FILETIME time)
        {
            return ((((long)time.dwHighDateTime) << 32) | time.dwLowDateTime) << 0;
        }

        private enum OS_MinusTwo_NotSupported_NTFS
        {
            WindowsServer2008,
            WindowsServer2008R2,
            WindowsServer2012
        }

        private enum OS_MinusTwo_NotSupported_REFS
        {
            WindowsServer2008,
            WindowsServer2008R2,
            WindowsServer2012,
            WindowsServer2012R2
        }

        #endregion
    }
}

