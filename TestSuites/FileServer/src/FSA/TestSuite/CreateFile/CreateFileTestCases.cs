// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.SpecExplorer.Runtime.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    [TestClassAttribute()]
    public partial class CreateFileTestCases : PtfTestClassBase
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
            this.fsaAdapter.FsaInitial();
        }

        protected override void TestCleanup()
        {
            this.fsaAdapter.Dispose();
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion

    }
}
