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
    public partial class CPMGetRowsTestCases : TestClassBase
    {
        private WspAdapter wspAdapter;
        private const uint validRowsToTransfer = 40;
        private const uint validReadBuffer = 0x4000;

        private const uint invalidReadBuffer = 0x00004001; // The value MUST NOT exceed 0x00004000.
        private const uint invalidRowsWidth = 0;
        private const uint invalidCursor = 0;

        public enum ArgumentType
        {
            AllValid,
            InvalidCursor,
            InvalidRowWidth,
            InvalidReadBuffer
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
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to test the basic functionality of CPMGetRows.")]
        public void BVT_CPMGetRows()
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(wspAdapter.GetCursor(wspAdapter.ClientComputerName), validRowsToTransfer, MessageBuilder.rowWidth, validReadBuffer, (uint)FetchType.ForwardOrder, (uint)RowSeekType.eRowSeekNext);
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if invalid Cursor is sent in CPMGetRowsIn.")]
        public void CPMGetRows_InvalidCursor()
        {

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with invalid cursor and expects ERROR_INVALID_PARAMETER .");
            argumentType = ArgumentType.InvalidCursor;
            wspAdapter.CPMGetRowsIn(invalidCursor, validRowsToTransfer, MessageBuilder.rowWidth, validReadBuffer, (uint)FetchType.ForwardOrder, (uint)RowSeekType.eRowSeekNext);
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if invalid RowWidth is sent in CPMGetRowsIn.")]
        public void CPMGetRows_InvalidRowWidth()
        {

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with invalid RowWidth and expects ERROR_INVALID_PARAMETER .");
            argumentType = ArgumentType.InvalidRowWidth;
            wspAdapter.CPMGetRowsIn(wspAdapter.GetCursor(wspAdapter.ClientComputerName), validRowsToTransfer, invalidRowsWidth, validReadBuffer, (uint)FetchType.ForwardOrder, (uint)RowSeekType.eRowSeekNext);
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if invalid ReadBuffer is sent in CPMGetRowsIn.")]
        public void CPMGetRows_InvalidReadBuffer()
        {

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectInRequest();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(true, true);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with invalid ReadBuffer and expects STATUS_INVALID_PARAMETER .");
            argumentType = ArgumentType.InvalidReadBuffer;
            wspAdapter.CPMGetRowsIn(wspAdapter.GetCursor(wspAdapter.ClientComputerName), validRowsToTransfer, MessageBuilder.rowWidth, invalidReadBuffer, (uint)FetchType.ForwardOrder, (uint)RowSeekType.eRowSeekNext);
        }

        #endregion


        private void CPMSetBindingsOut(uint errorCode)
        {
            Site.Assert.AreEqual((uint)0, errorCode, "CPMSetBindingsIn should succeed.");
        }

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
            switch (argumentType)
            {
                case ArgumentType.AllValid:
                    Site.Assert.AreEqual((uint)0, errorCode, "Server should return succeed for CPMGetRowsIn.");
                    break;
                case ArgumentType.InvalidCursor:
                    Site.Assert.AreEqual((uint)WspErrorCode.ERROR_INVALID_PARAMETER, errorCode, "Server should return ERROR_INVALID_PARAMETER if Cursor of CPMGetRowsIn is invalid.");
                    break;
                case ArgumentType.InvalidRowWidth:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER, errorCode, "Server should return STATUS_INVALID_PARAMETER if RowWidth of CPMGetRowsIn is invalid.");
                    break;
                case ArgumentType.InvalidReadBuffer:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER, errorCode, "Server should return STATUS_INVALID_PARAMETER if ReadBuffer of CPMGetRowsIn is invalid.");
                    break;

            }
        }
    }
}
