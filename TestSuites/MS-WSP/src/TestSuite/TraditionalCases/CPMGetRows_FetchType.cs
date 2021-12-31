// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    public partial class CPMGetRowsTestCases : WspCommonTestBase
    {
        #region Test Cases

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if _fBwdFetch sent in CPMGetRowsIn indicates fetching rows in forward order.")]
        public void CPMGetRows_fBwdFetch_FetchForward()
        {
            const uint rowsToTransfer = 5;

            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with _fBwdFetch set to 0 and expects success.");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                rowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out var getRowsOut1);

            Site.Assert.AreEqual(5U, getRowsOut1._cRowsReturned, "The count of rows returned should be 5 since the RowsToTransfer sent in CPMGetRowsIn is 5.");
            LogCPMGetRowsOut(getRowsOut1);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with _fBwdFetch set to 0 and expects success.");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                rowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out var getRowsOut2);

            Site.Assert.AreEqual(5U, getRowsOut2._cRowsReturned, "The count of rows returned should be 5 since the RowsToTransfer sent in CPMGetRowsIn is 5.");
            LogCPMGetRowsOut(getRowsOut2);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with _fBwdFetch set to 0 and expects success.");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                rowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out var getRowsOut3);

            Site.Assert.AreEqual(3U, getRowsOut3._cRowsReturned, "The count of rows returned should be 3 since there are 3 rows left.");
            LogCPMGetRowsOut(getRowsOut3);

            var verificationAdapter = GetWspAdapterForVerification();

            Site.Log.Add(LogEntryKind.TestStep, "Client for verification sends CPMGetRowsIn with _fBwdFetch set to 0 and expects success.");
            verificationAdapter.CPMGetRowsIn(
                verificationAdapter.GetCursor(verificationAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out var getRowsOutAllRows);
            verificationAdapter.Reset();

            Site.Assert.AreEqual(13U, getRowsOutAllRows._cRowsReturned, "The count of rows returned should be 13 since the total rows count is 13.");
            LogCPMGetRowsOut(getRowsOutAllRows);

            var allItemNamesBy3Requests = getRowsOut1.Rows.Concat(getRowsOut2.Rows).Concat(getRowsOut3.Rows).Select(row => row.Columns[0].Data as string);
            var allItemNamesBy1Request = getRowsOutAllRows.Rows.Select(row => row.Columns[0].Data as string);

            var succeed = allItemNamesBy1Request.SequenceEqual(allItemNamesBy3Requests);
            Site.Assert.IsTrue(succeed, "The rows returned by 3 requests fetching forward separately should be the same as the rows returned by 1 request fetching forward entirely.");
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if _fBwdFetch sent in CPMGetRowsIn indicates fetching rows in forward order at the end of rowset.")]
        public void CPMGetRows_fBwdFetch_FetchForwardAtEndOfRowset()
        {
            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with _fBwdFetch set to 0 and expects success.");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out var getRowsOut1);

            Site.Assert.AreEqual(13U, getRowsOut1._cRowsReturned, "The count of rows returned should be 13 since the total rows count is 13.");
            LogCPMGetRowsOut(getRowsOut1);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with _fBwdFetch set to 0 and expects success.");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out var getRowsOut2);

            Site.Assert.AreEqual(0U, getRowsOut2._cRowsReturned, "The count of rows returned should be 0 since the index is at the end of rowset.");
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if _fBwdFetch sent in CPMGetRowsIn indicates fetching rows in reverse order at the end of rowset.")]
        public void CPMGetRows_fBwdFetch_FetchReverseAtEndOfRowset()
        {
            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with _fBwdFetch set to 0 and expects success.");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out var getRowsOut1);

            Site.Assert.AreEqual(13U, getRowsOut1._cRowsReturned, "The count of rows returned should be 13 since the total rows count is 13.");
            LogCPMGetRowsOut(getRowsOut1);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with _fBwdFetch set to 1 and expects success.");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ReverseOrder,
                (uint)RowSeekType.eRowSeekNext,
                out var getRowsOut2);

            Site.Assert.AreEqual(0U, getRowsOut2._cRowsReturned, "The count of rows returned should be 0 since the index is at the end of rowset.");
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if _fBwdFetch sent in CPMGetRowsIn indicates fetching rows in fordward order and then in reverse order.")]
        public void CPMGetRows_fBwdFetch_FetchForwardThenReverse()
        {
            const uint rowsToTransfer = 5;

            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with _fBwdFetch set to 0 and expects success.");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                rowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out var getRowsOut1);

            Site.Assert.AreEqual(5U, getRowsOut1._cRowsReturned, "The count of rows returned should be 5 since the RowsToTransfer sent in CPMGetRowsIn is 5.");
            LogCPMGetRowsOut(getRowsOut1);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with _fBwdFetch set to 1 and expects success.");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                rowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ReverseOrder,
                (uint)RowSeekType.eRowSeekNext,
                out var getRowsOut2);

            Site.Assert.AreEqual(5U, getRowsOut2._cRowsReturned, "The count of rows returned should be 5 since the RowsToTransfer sent in CPMGetRowsIn is 5.");
            LogCPMGetRowsOut(getRowsOut2);

            var reversedItemNamesInGetRowsOut2 = getRowsOut2.Rows.Reverse().Select(row => row.Columns[0].Data as string);
            var itemNamesInGetRowsOut1 = getRowsOut1.Rows.Select(row => row.Columns[0].Data as string);

            var succeed = itemNamesInGetRowsOut1.SequenceEqual(reversedItemNamesInGetRowsOut2);
            Site.Assert.IsTrue(succeed, "The rows returned in the reverse order by the second request fetching reverse should be the same as the rows returned by the first request fetching forward.");
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if _fBwdFetch sent in CPMGetRowsIn indicates fetching rows in reverse order and then in forward order.")]
        public void CPMGetRows_fBwdFetch_FetchReverseThenForward()
        {
            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with _fBwdFetch set to 1 and expects success.");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ReverseOrder,
                (uint)RowSeekType.eRowSeekNext,
                out var getRowsOut1);

            Site.Assert.AreEqual(0U, getRowsOut1._cRowsReturned, "The count of rows returned should be 0 since the index exceeds the start position of the rowset.");

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with _fBwdFetch set to 0 and expects success.");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out var getRowsOut2);

            Site.Assert.AreEqual(13U, getRowsOut2._cRowsReturned, "The count of rows returned should be 13 since the total rows count is 13.");
            LogCPMGetRowsOut(getRowsOut2);

            var verificationAdapter = GetWspAdapterForVerification();

            Site.Log.Add(LogEntryKind.TestStep, "Client for verification sends CPMGetRowsIn with _fBwdFetch set to 0 and expects success.");
            verificationAdapter.CPMGetRowsIn(
                verificationAdapter.GetCursor(verificationAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out var getRowsOut3);
            verificationAdapter.Reset();

            Site.Assert.AreEqual(13U, getRowsOut3._cRowsReturned, "The count of rows returned should be 13 since the total rows count is 13.");
            LogCPMGetRowsOut(getRowsOut3);

            var itemNamesInGetRowsOut2 = getRowsOut2.Rows.Select(row => row.Columns[0].Data as string);
            var itemNamesInGetRowsOut3 = getRowsOut3.Rows.Select(row => row.Columns[0].Data as string);

            var succeed = itemNamesInGetRowsOut3.SequenceEqual(itemNamesInGetRowsOut2);
            Site.Assert.IsTrue(succeed, "The rows returned by the second request fetching forward entirely should be the same as the rows returned by 1 request fetching forward entirely.");
        }

        #endregion
    }
}
