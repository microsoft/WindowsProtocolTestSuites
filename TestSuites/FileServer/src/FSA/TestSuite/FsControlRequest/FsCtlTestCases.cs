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
    public partial class FsCtlTestCases : PtfTestClassBase
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

        #region Common Utility For FSCTL test cases

        /// <summary>
        /// To check if current transport supports Integrity.
        /// </summary>
        /// <param name="status">NTStatus code to compare with expected status.</param>
        /// <returns>Return True if supported, False if not supported.</returns>
        private bool IsCurrentTransportSupportIntegrity(MessageStatus status)
        {
            if (this.fsaAdapter.Transport == Transport.SMB)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.NOT_SUPPORTED, status,
                    "This operation is not supported for SMB transport.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// To check if current transport supports CopyOffload.
        /// </summary>
        /// <param name="status">NTStatus code to compare with expected status.</param>
        /// <returns>Return True if supported, False if not supported.</returns>
        private bool IsCurrentTransportSupportCopyOffload(MessageStatus status)
        {
            if (this.fsaAdapter.Transport == Transport.SMB)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.NOT_SUPPORTED, status,
                    "This operation is not supported for SMB transport.");
                return false;
            }
            return true;
        }

        #endregion
    }
}

