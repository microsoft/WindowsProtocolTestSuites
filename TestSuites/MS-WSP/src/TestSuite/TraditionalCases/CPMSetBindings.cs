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
            AllValid
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

        //[TestMethod]
        //[TestCategory("CPMSetBindings")]
        //[Description("This test case is designed to verify the server response if invalid Cursor is sent in CPMGetRowsIn.")]
        //public void CPMGetRows_InvalidCursor()
        //{

        //    Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
        //    wspAdapter.CPMConnectInRequest();

        //    Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
        //    wspAdapter.CPMCreateQueryIn(true);

        //    Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
        //    wspAdapter.CPMSetBindingsIn(true, true);

        //    Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with invalid cursor and expects ERROR_INVALID_PARAMETER .");
        //    argumentType = ArgumentType.InvalidCursor;
        //    wspAdapter.CPMGetRowsIn(invalidCursor, validRowsToTransfer, MessageBuilder.rowWidth, validReadBuffer, (uint)FetchType.ForwardOrder, (uint)RowSeekType.eRowSeekNext);
        //}

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
            Site.Assert.AreEqual((uint)0, errorCode, "CPMGetRowsIn should succeed.");
        }

        private void CPMSetBindingsOut(uint errorCode)
        {
            switch (argumentType)
            {
                case ArgumentType.AllValid:
                    Site.Assert.AreEqual((uint)0, errorCode, "Server should return succeed for CPMSetBindingsIn.");
                    break;
                //case ArgumentType.InvalidCursor:
                //    Site.Assert.AreEqual((uint)WspErrorCode.ERROR_INVALID_PARAMETER, errorCode, "Server should return ERROR_INVALID_PARAMETER if Cursor of CPMGetRowsIn is invalid.");
                //    break;
            }
        }
    }
}
