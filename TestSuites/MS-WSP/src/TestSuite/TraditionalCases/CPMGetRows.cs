// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp.Adapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    [TestClass]
    public partial class CPMGetRowsTestCases : WspCommonTestBase
    {
        private const uint validRowsToTransfer = 40;
        private const uint validReadBuffer = 0x4000;
        private static readonly uint validRowWidth = MessageBuilder.RowWidth;

        public enum ArgumentType
        {
            // All arguments passed during the communication are valid.
            AllValid,
            // The cursor passed in the CPMGetRowsIn is an invalid value.
            InvalidCursor,
            // The CPMGetRowsIn is requested without previous successful CPMSetBindingsIn request.
            WithoutBindings,

            // The RowWidth of the CPMGetRowsIn request is 0.
            ZeroRowWidth,
            // The RowWidth of the CPMGetRowsIn mismatched the _cbRow in the previous CPMSetBindingsIn request.
            MismatchedRowWidth,
            // The RowWidth of the CPMGetRowsIn exceeds the ReadBuffer.
            RowWidthExceedingReadBuffer,

            // The ReadBuffer of the CPMGetRowsIn request is 0.
            ZeroReadBuffer,
            // The ReadBuffer of the CPMGetRowsIn is not enough for a single row to be filled by the server.
            ReadBufferNotEnoughForSingleRow,
            // The ReadBuffer of the CPMGetRowsIn is not enough for all the rows returned indicated by the RowsToTransfer to be filled by the server.
            ReadBufferNotEnoughForAllReturnedRows,
            // The ReadBuffer exceeds the maximum value 0x4000.
            ReadBufferExceedingMaximum,

            // The eType of the CPMGetRowsIn is an invalid value.
            InvalidEType,
            // The actual SeekDescription structure mismatches what the eType indicates and the size of the actual SeekDescription is smaller than the size of what the eType indicates.
            MismatchedETypeSmallerSeekDescription,
            // The ratio indicated by CRowSeekAtRatio of the CPMGetRowsIn is invalid.
            InvalidCRowSeekAtRatio
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

            wspAdapter.CPMGetRowsOutResponse -= EnsureSuccessfulCPMGetRowsOut;
            wspAdapter.CPMGetRowsOutResponse += CPMGetRowsOut;
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
            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out CPMGetRowsOut getRowsOut);

            Site.Assert.AreEqual(13U, getRowsOut._cRowsReturned, "The count of rows returned should be 13 since only 13 files have the content \"test\" in the search scope.");
            LogCPMGetRowsOut(getRowsOut);
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if invalid Cursor is sent in CPMGetRowsIn.")]
        public void CPMGetRows_InvalidCursor()
        {
            const uint invalidCursor = 0;

            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with invalid cursor and expects ERROR_INVALID_PARAMETER.");
            argumentType = ArgumentType.InvalidCursor;
            wspAdapter.CPMGetRowsIn(
                invalidCursor,
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out _);
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if CPMGetRowsIn is sent without bindings.")]
        public void CPMGetRows_WithoutBindings()
        {
            PrepareForCPMGetRows(setBindings: false);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn without bindings and expects E_UNEXPECTED.");
            argumentType = ArgumentType.WithoutBindings;
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out _);
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if the RowsToTransfer sent in CPMGetRowsIn is 0.")]
        public void CPMGetRows_RowsToTransfer_ZeroRowsToTransfer()
        {
            const uint zeroRowsToTransfer = 0;

            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                zeroRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out CPMGetRowsOut getRowsOut);

            Site.Assert.AreEqual(0U, getRowsOut._cRowsReturned, "The count of rows returned should be 0 since the RowsToTransfer sent in CPMGetRowsIn is 0.");
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if the RowsToTransfer sent in CPMGetRowsIn is smaller than the total rows count.")]
        public void CPMGetRows_RowsToTransfer_SmallerThanTotalRowsCount()
        {
            const uint rowsToTransferSmallerThanTotalRowsCount = 3;

            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                rowsToTransferSmallerThanTotalRowsCount,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out CPMGetRowsOut getRowsOut);

            Site.Assert.AreEqual(3U, getRowsOut._cRowsReturned, $"The count of rows returned should be 3 since the RowsToTransfer sent in CPMGetRowsIn is 3.");
            LogCPMGetRowsOut(getRowsOut);
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if RowWidth sent in CPMGetRowsIn is 0.")]
        public void CPMGetRows_RowWidth_ZeroRowWidth()
        {
            const uint zeroRowsWidth = 0;

            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn whose RowWidth is 0 and expects STATUS_INVALID_PARAMETER.");
            argumentType = ArgumentType.ZeroRowWidth;
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                zeroRowsWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out _);
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if the RowWidth sent in CPMGetRowsIn does not match the _cbRow sent in the previous CPMSetBindingsIn.")]
        public void CPMGetRows_RowWidth_MismatchedRowWidth()
        {
            const uint mismatchedRowWidth = 64;

            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with a RowWidth which does not match the _cbRow sent in the previous CPMSetBindingsIn and expects ERROR_INVALID_PARAMETER.");
            argumentType = ArgumentType.MismatchedRowWidth;
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                mismatchedRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out _);
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if the RowWidth sent in CPMGetRowsIn exceeds the ReadBuffer.")]
        public void CPMGetRows_RowWidth_ExceedingReadBuffer()
        {
            const uint readBuffer = 0x2000;
            const uint rowWidthExceedingReadBuffer = readBuffer + 1;

            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with a RowWidth which exceeds the ReadBuffer and expects STATUS_ERROR_INVALID_PARAMETER.");
            argumentType = ArgumentType.RowWidthExceedingReadBuffer;
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                rowWidthExceedingReadBuffer,
                readBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out _);
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if ReadBuffer sent in CPMGetRowsIn is 0.")]
        public void CPMGetRows_ReadBuffer_ZeroReadBuffer()
        {
            const uint zeroReadBuffer = 0;

            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn whose ReadBuffer is 0 and expects STATUS_INVALID_PARAMETER.");
            argumentType = ArgumentType.ZeroReadBuffer;
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                zeroReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out _);
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if a ReadBuffer which is not enough for a single row is sent in CPMGetRowsIn.")]
        public void CPMGetRows_ReadBuffer_NotEnoughForSingleRow()
        {
            uint readBufferNotEnoughForASingleRow = validRowWidth * 3;

            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with a ReadBuffer which is not enough for a single row and expects STATUS_BUFFER_TOO_SMALL.");
            argumentType = ArgumentType.ReadBufferNotEnoughForSingleRow;
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                readBufferNotEnoughForASingleRow,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out _);
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if a ReadBuffer which is not enough for all returned rows is sent in CPMGetRowsIn.")]
        public void CPMGetRows_ReadBuffer_NotEnoughForAllReturnedRows()
        {
            const uint readBufferNotEnoughForAllReturnedRows = 0x200; // The buffer should be filled by a single row.

            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with a ReadBuffer which is not enough for all returned rows and expects DB_S_DIALECTIGNORED.");
            argumentType = ArgumentType.ReadBufferNotEnoughForAllReturnedRows;
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                readBufferNotEnoughForAllReturnedRows,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out var getRowsOut);

            Site.Assert.AreEqual(1U, getRowsOut._cRowsReturned, "The count of rows returned should be 1 since only 1 row can be filled into the ReadBuffer.");
            LogCPMGetRowsOut(getRowsOut);
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if invalid ReadBuffer exceeding the maximum value is sent in CPMGetRowsIn.")]
        public void CPMGetRows_ReadBuffer_ExceedingMaximum()
        {
            const uint readBufferExceedingMaximum = 0x00004001; // The value MUST NOT exceed 0x00004000 otherwise STATUS_INVALID_PARAMETER will be returned as the error code.

            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with invalid ReadBuffer exceeding the maximum value and expects STATUS_INVALID_PARAMETER.");
            argumentType = ArgumentType.ReadBufferExceedingMaximum;
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                readBufferExceedingMaximum,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out _);
        }

        #endregion

        private void PrepareForCPMGetRows(bool setBindings = true)
        {
            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(false);

            if (setBindings)
            {
                Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
                wspAdapter.CPMSetBindingsIn(true, true);
            }
        }

        private WspAdapter GetWspAdapterForVerification()
        {
            var verificationAdapter = new WspAdapter();
            verificationAdapter.Initialize(this.Site);

            verificationAdapter.CPMConnectOutResponse += EnsureSuccessfulCPMConnectOut;
            verificationAdapter.CPMSetBindingsInResponse += EnsureSuccessfulCPMSetBindingsOut;
            verificationAdapter.CPMCreateQueryOutResponse += EnsureSuccessfulCPMCreateQueryOut;

            verificationAdapter.CPMGetRowsOutResponse += CPMGetRowsOut;

            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client for verification sends CPMConnectIn and expects success.");
            verificationAdapter.CPMConnectIn();

            Site.Log.Add(LogEntryKind.TestStep, "Client for verification sends CPMCreateQueryIn and expects success.");
            verificationAdapter.CPMCreateQueryIn(false);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            verificationAdapter.CPMSetBindingsIn(true, true);

            return verificationAdapter;
        }

        private void LogCPMGetRowsOut(CPMGetRowsOut getRowsOut)
        {
            Site.Log.Add(LogEntryKind.TestStep, "The rows returned as below: ");

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine();
            strBuilder.AppendLine("      System.ItemName;  System.ItemFolderNameDisplay");
            for (int i = 0; i < getRowsOut._cRowsReturned; i++)
            {
                strBuilder.Append(string.Format("Row {0}: ", i));
                for (int j = 0; j < getRowsOut.Rows[i].Columns.Length; j++)
                {
                    strBuilder.Append(getRowsOut.Rows[i].Columns[j].Data);
                    strBuilder.Append(";      ");
                }
                strBuilder.AppendLine();
            }

            Site.Log.Add(LogEntryKind.Debug, strBuilder.ToString());
        }

        private void CPMGetRowsOut(uint errorCode)
        {
            switch (argumentType)
            {
                case ArgumentType.AllValid:
                    bool succeed = errorCode == (uint)WspErrorCode.SUCCESS || errorCode == (uint)WspErrorCode.DB_S_ENDOFROWSET ? true : false;
                    Site.Assert.IsTrue(succeed, "Server should return SUCCESS or DB_S_ENDOFROWSET for CPMGetRowsIn.");
                    break;
                case ArgumentType.InvalidCursor:
                    Site.Assert.AreEqual((uint)WspErrorCode.ERROR_INVALID_PARAMETER, errorCode, "Server should return ERROR_INVALID_PARAMETER if Cursor of CPMGetRowsIn is invalid.");
                    break;
                case ArgumentType.WithoutBindings:
                    Site.Assert.AreEqual((uint)WspErrorCode.E_UNEXPECTED, errorCode, "Server should return E_UNEXPECTED if CPMGetRowsIn is sent without bindings.");
                    break;

                case ArgumentType.ZeroRowWidth:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER, errorCode, "Server should return STATUS_INVALID_PARAMETER if RowWidth of CPMGetRowsIn is 0.");
                    break;
                case ArgumentType.MismatchedRowWidth:
                    Site.Assert.AreEqual((uint)WspErrorCode.ERROR_INVALID_PARAMETER, errorCode, "Server should return ERROR_INVALID_PARAMETER if RowWidth of CPMGetRowsIn does not match the _cbRow sent in the previous CPMSetBindingsIn.");
                    break;
                case ArgumentType.RowWidthExceedingReadBuffer:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER, errorCode, "Server should return STATUS_INVALID_PARAMETER if RowWidth of CPMGetRowsIn exceeds the ReadBuffer.");
                    break;

                case ArgumentType.ZeroReadBuffer:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER, errorCode, "Server should return STATUS_INVALID_PARAMETER if ReadBuffer of CPMGetRowsIn is 0.");
                    break;
                case ArgumentType.ReadBufferNotEnoughForSingleRow:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_BUFFER_TOO_SMALL, errorCode, "Server should return STATUS_BUFFER_TOO_SMALL if ReadBuffer of CPMGetRowsIn is not enough to be filled by a single row.");
                    break;
                case ArgumentType.ReadBufferNotEnoughForAllReturnedRows:
                    Site.Assert.AreEqual((uint)WspErrorCode.DB_S_DIALECTIGNORED, errorCode, "Server should return DB_S_DIALECTIGNORED if ReadBuffer of CPMGetRowsIn is not enough for all returned rows.");
                    break;
                case ArgumentType.ReadBufferExceedingMaximum:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER, errorCode, "Server should return STATUS_INVALID_PARAMETER if ReadBuffer of CPMGetRowsIn exceeds the maximum value.");
                    break;

                case ArgumentType.InvalidEType:
                    Site.Assert.AreEqual((uint)WspErrorCode.STATUS_INVALID_PARAMETER, errorCode, "Server should return STATUS_INVALID_PARAMETER if eType of CPMGetRowsIn is invalid.");
                    break;
                case ArgumentType.MismatchedETypeSmallerSeekDescription:
                    Site.Assert.AreEqual((uint)WspErrorCode.E_ABORT, errorCode, "Server should return E_ABORT if eType and SeekDescription of CPMGetRowsIn are mismatched and the size of SeekDescription is smaller than what eType indicates.");
                    break;
                case ArgumentType.InvalidCRowSeekAtRatio:
                    Site.Assert.AreEqual((uint)WspErrorCode.DB_E_BADRATIO, errorCode, "Server should return DB_E_BADRATIO if the ratio of CRowSeekAtRatio in CPMGetRowsIn does not express a ratio between zero and 1.");
                    break;
            }
        }
    }
}
