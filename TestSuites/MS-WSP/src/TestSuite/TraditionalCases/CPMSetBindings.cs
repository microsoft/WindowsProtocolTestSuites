// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    [TestClass]
    public partial class CPMSetBindingsTestCases : TestClassBase
    {
        private WspAdapter wspAdapter;

        public enum ArgumentType
        {
            AllValid,
            InvalidRowSize,
            InvalidCursor
        }
        private ArgumentType argumentType;

        #region Test Initialize and Cleanup
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }
        #endregion

        #region Test Case Initialize and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();
            wspAdapter = new WspAdapter();
            wspAdapter.Initialize(this.Site);
            wspAdapter.CPMConnectOutResponse += CPMConnectOut;
            wspAdapter.CPMSetBindingsInResponse += CPMSetBindingsOut;
            wspAdapter.CPMCreateQueryOutResponse += CPMCreateQueryOut;
            wspAdapter.CPMGetRowsOut += CPMGetRowsOut;
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        #region Test Cases

        [TestMethod]
        [TestCategory("BVT")]
        [TestCategory("CPMSetBindings")]
        [Description("This test case is designed to test the basic functionality of CPMSetBindings.")]
        public void BVT_CPMSetBindings()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(true);
        }

        [TestMethod]
        [TestCategory("CPMSetBindings")]
        [Description("This test case is designed to verify the server response if invalid row size is sent in CPMSetBindingsIn.")]
        public void CPMSetBindings_InvalidRowSize()
        {

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn with invalid row size and expects NOT SUCCEED.");
            argumentType = ArgumentType.InvalidRowSize;
            wspAdapter.CPMSetBindingsIn(false, true);
            //E_ABORT
        }

        [TestMethod]
        [TestCategory("CPMSetBindings")]
        [Description("This test case is designed to verify the server response if invalid cursor is sent in CPMSetBindingsIn.")]
        public void CPMSetBindings_InvalidCursor()
        {

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn with invalid cursor and expects NOT SUCCEED.");
            argumentType = ArgumentType.InvalidCursor;
            wspAdapter.CPMSetBindingsIn(true, false);
            //ERROR_INVALID_STATUS
        }

        #endregion

        private void CPMConnectOut(uint errorCode)
        {
            Site.Assert.AreEqual((uint)0, errorCode, "CPMConnectIn should succeed.");
        }

        private void CPMCreateQueryOut(uint errorCode)
        {
            Site.Assert.AreEqual((uint)0, errorCode, "CPMCreateQueryIn should succeed.");
        }

        private void CPMGetRowsOut(uint errorCode)
        {
            bool succeed = errorCode == 0 || errorCode == (uint)WspErrorCode.DB_S_ENDOFROWSET ? true : false;
            Site.Assert.IsTrue(succeed, "Server should return succeed or DB_S_ENDOFROWSET for CPMGetRowsIn.");
        }

        private void CPMSetBindingsOut(uint errorCode)
        {
            switch (argumentType)
            {
                case ArgumentType.AllValid:
                    Site.Assert.AreEqual((uint)0, errorCode, "Server should return succeed for CPMSetBindingsIn.");
                    break;
                case ArgumentType.InvalidRowSize:
                    Site.Assert.AreNotEqual((uint)0, errorCode, "Server should not return succeed if row size of CPMSetBindingsIn is invalid.");
                    break;
                case ArgumentType.InvalidCursor:
                    Site.Assert.AreEqual((uint)WspErrorCode.ERROR_INVALID_PARAMETER, errorCode, "Server should return ERROR_INVALID_PARAMETER if the cursor of CPMSetBindingsIn is invalid.");
                    break;
            }
        }
    }
}
