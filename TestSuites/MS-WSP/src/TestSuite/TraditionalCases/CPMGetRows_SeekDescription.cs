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
        [Description("This test case is designed to verify the server response if an invalid eType is sent in CPMGetRowsIn.")]
        public void CPMGetRows_eType_InvalidEType()
        {
            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn with an invalid eType and expects STATUS_INVALID_PARAMETER.");
            argumentType = ArgumentType.InvalidEType;
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                255U,
               new InvalidSeekDescription(),
                out _);
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if the eType and the SeekDescription sent in CPMGetRowsIn are mismatched and the size of SeekDescription is smaller than what eType indicates.")]
        public void CPMGetRows_eType_MismatchedEType_SmallerSeekDescription()
        {
            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn whose eType and SeekDescription are mismatched and the size of SeekDescription is smaller than what eType indicates and expects E_ABORT.");
            argumentType = ArgumentType.MismatchedETypeSmallerSeekDescription;
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekAt,
                new CRowSeekNext { _cskip = 0 },
                out _);
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server behavior when CRowSeekNext is sent in CPMGetRowsIn.")]
        public void CPMGetRows_eType_eRowSeekNext()
        {
            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn whose SeekDescription is a CRowSeekNext and expects success.");
            Site.Log.Add(LogEntryKind.Debug, $"Field values of the CRowSeekNext structure: _cskip: 2");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                new CRowSeekNext { _cskip = 2 },
                out var getRowsOut1);

            Site.Assert.AreEqual(11U, getRowsOut1._cRowsReturned, "The count of rows returned should be 11 since 2 rows at the start of the rowset are skipped.");
            LogCPMGetRowsOut(getRowsOut1);

            var verificationAdapter = GetWspAdapterForVerification();

            Site.Log.Add(LogEntryKind.TestStep, "Client for verification sends CPMGetRowsIn and expects success.");
            verificationAdapter.CPMGetRowsIn(
                verificationAdapter.GetCursor(verificationAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out var getRowsOut2);
            verificationAdapter.Reset();

            Site.Assert.AreEqual(13U, getRowsOut2._cRowsReturned, "The count of rows returned should be 13 since the total rows count is 13.");
            LogCPMGetRowsOut(getRowsOut2);

            var itemNamesInGetRowsOut1 = getRowsOut1.Rows.Select(row => row.Columns[0].Data as string);
            var itemNamesInGetRowsOut2First2RowsSkipped = getRowsOut2.Rows.Skip(2).Select(row => row.Columns[0].Data as string);

            var succeed = itemNamesInGetRowsOut2First2RowsSkipped.SequenceEqual(itemNamesInGetRowsOut1);
            Site.Assert.IsTrue(succeed, "The rows returned by the first request should be the same as the rows returned by the second request with 2 rows skipped at the start.");
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if the _cskip of CRowSeekNext sent in CPMGetRowsIn is larger than the total rows count.")]
        public void CPMGetRows_eType_eRowSeekNext_SkipAllRows()
        {
            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn whose SeekDescription is a CRowSeekNext with a _cskip larger than the total rows count and expects success.");
            Site.Log.Add(LogEntryKind.Debug, $"Field values of the CRowSeekNext structure: _cskip: 40");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                new CRowSeekNext { _cskip = 40 },
                out var getRowsOut);

            Site.Assert.AreEqual(0U, getRowsOut._cRowsReturned, "The count of rows returned should be 0 since all rows are skipped.");
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server behavior when CRowSeekAtRatio is sent in CPMGetRowsIn.")]
        public void CPMGetRows_eType_eRowSeekAtRatio()
        {
            const uint rowsToTransfer = 5;

            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetQueryStatusExIn and expects success.");
            wspAdapter.CPMGetQueryStatusExIn(out CPMGetQueryStatusExOut response);
            var denominator = response._dwRatioFinishedDenominator;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn whose SeekDescription is a CRowSeekAtRatio with a ratio of 0 and expects success.");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                rowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekAtRatio,
                new CRowSeekAtRatio { _ulNumerator = 0, _ulDenominator = denominator, _hRegion = 0 },
                out var getRowsOut1);

            Site.Assert.AreEqual(5U, getRowsOut1._cRowsReturned, "The count of rows returned should be 5 since the RowsToTransfer sent in CPMGetRowsIn is 5.");
            LogCPMGetRowsOut(getRowsOut1);

            var verificationAdapter = GetWspAdapterForVerification();

            Site.Log.Add(LogEntryKind.TestStep, "Client for verification sends CPMGetRowsIn and expects success.");
            verificationAdapter.CPMGetRowsIn(
                verificationAdapter.GetCursor(verificationAdapter.ClientMachineName),
                rowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekNext,
                out var getRowsOut2);
            verificationAdapter.Reset();

            Site.Assert.AreEqual(5U, getRowsOut2._cRowsReturned, "The count of rows returned should be 5 since the RowsToTransfer sent in CPMGetRowsIn is 5.");
            LogCPMGetRowsOut(getRowsOut2);

            var itemNamesInGetRowsOut1 = getRowsOut1.Rows.Select(row => row.Columns[0].Data as string);
            var itemNamesInGetRowsOut2 = getRowsOut2.Rows.Select(row => row.Columns[0].Data as string);

            var succeed = itemNamesInGetRowsOut2.SequenceEqual(itemNamesInGetRowsOut1);
            Site.Assert.IsTrue(succeed, "The rows returned by the first request should be the same as the rows returned by the second request.");
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if the ratio of CRowSeekAtRatio sent in CPMGetRowsIn is 1.")]
        public void CPMGetRows_eType_eRowSeekAtRatio_SkipAllRows()
        {
            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetQueryStatusExIn and expects success.");
            wspAdapter.CPMGetQueryStatusExIn(out CPMGetQueryStatusExOut response);
            var denominator = response._dwRatioFinishedDenominator;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn whose SeekDescription is a CRowSeekAtRatio with a ratio of 1 and expects success.");
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekAtRatio,
                new CRowSeekAtRatio { _ulNumerator = denominator, _ulDenominator = denominator, _hRegion = 0 },
                out var getRowsOut);

            Site.Assert.AreEqual(0U, getRowsOut._cRowsReturned, "The count of rows returned should be 0 since all rows are skipped.");
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if the _ulDenominator of CRowSeekAtRatio sent in CPMGetRowsIn is 0.")]
        public void CPMGetRows_eType_eRowSeekAtRatio_ZeroDenominator()
        {
            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn whose SeekDescription is a CRowSeekAtRatio whose _ulDenominator is 0 and expects DB_E_BADRATIO.");
            argumentType = ArgumentType.InvalidCRowSeekAtRatio;
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekAtRatio,
                new CRowSeekAtRatio { _ulNumerator = 0, _ulDenominator = 0, _hRegion = 0 },
                out _);
        }

        [TestMethod]
        [TestCategory("CPMGetRows")]
        [Description("This test case is designed to verify the server response if the CRowSeekAtRatio sent in CPMGetRowsIn has a _ulNumerator larger than the _ulDenominator.")]
        public void CPMGetRows_eType_eRowSeekAtRatio_NumeratorLargerThanDenominator()
        {
            PrepareForCPMGetRows();

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetQueryStatusExIn and expects success.");
            wspAdapter.CPMGetQueryStatusExIn(out CPMGetQueryStatusExOut response);
            var denominator = response._dwRatioFinishedDenominator;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn whose SeekDescription is a CRowSeekAtRatio whose _ulNumerator is larger than the _ulDenominator and expects DB_E_BADRATIO.");
            argumentType = ArgumentType.InvalidCRowSeekAtRatio;
            wspAdapter.CPMGetRowsIn(
                wspAdapter.GetCursor(wspAdapter.ClientMachineName),
                validRowsToTransfer,
                validRowWidth,
                validReadBuffer,
                (uint)FetchType.ForwardOrder,
                (uint)RowSeekType.eRowSeekAtRatio,
                new CRowSeekAtRatio { _ulNumerator = denominator + 1, _ulDenominator = denominator, _hRegion = 0 },
                out _);
        }

        #endregion
    }
}
